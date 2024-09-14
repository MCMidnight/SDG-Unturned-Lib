using System;
using System.Collections.Generic;
using System.IO;
using SDG.Framework.Utilities;
using SDG.Provider;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020002A3 RID: 675
	public static class DedicatedUGC
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x0004B2D7 File Offset: 0x000494D7
		// (set) Token: 0x06001449 RID: 5193 RVA: 0x0004B2DE File Offset: 0x000494DE
		public static List<SteamContent> ugc { get; private set; }

		/// <summary>
		/// Broadcasts once all workshop assets are finished installing.
		/// </summary>
		// Token: 0x14000072 RID: 114
		// (add) Token: 0x0600144A RID: 5194 RVA: 0x0004B2E8 File Offset: 0x000494E8
		// (remove) Token: 0x0600144B RID: 5195 RVA: 0x0004B31C File Offset: 0x0004951C
		public static event DedicatedUGCInstalledHandler installed;

		// Token: 0x0600144C RID: 5196 RVA: 0x0004B34F File Offset: 0x0004954F
		public static void registerItemInstallation(ulong id)
		{
			DedicatedUGC.enqueueItemToQuery(new PublishedFileId_t(id));
		}

		/// <summary>
		/// Called once the server is done registering items it wants to install.
		/// </summary>
		/// <param name="onlyFromCache">True when running in offline-only mode.</param>
		// Token: 0x0600144D RID: 5197 RVA: 0x0004B360 File Offset: 0x00049560
		public static void beginInstallingItems(bool onlyFromCache)
		{
			CommandWindow.Log(DedicatedUGC.itemsToQuery.Count.ToString() + " workshop item(s) requested");
			if (DedicatedUGC.itemsToQuery.Count == 0)
			{
				DedicatedUGC.OnFinishedDownloadingItems();
				return;
			}
			Assets.loadingStats.Reset();
			if (onlyFromCache)
			{
				DedicatedUGC.installItemsToQueryFromCache();
				return;
			}
			DedicatedUGC.submitQuery();
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x0004B3B8 File Offset: 0x000495B8
		private static uint maxQueryRetries
		{
			get
			{
				return WorkshopDownloadConfig.get().Max_Query_Retries;
			}
		}

		/// <summary>
		/// Enqueue an item if we have not queried it yet. This guards against querying an item
		/// that is in two separate collections leading to duplicates.
		/// </summary>
		// Token: 0x0600144F RID: 5199 RVA: 0x0004B3C4 File Offset: 0x000495C4
		private static bool enqueueItemToQuery(PublishedFileId_t item)
		{
			if (DedicatedUGC.itemsQueried.Contains(item.m_PublishedFileId))
			{
				return false;
			}
			DedicatedUGC.itemsToQuery.Enqueue(item);
			DedicatedUGC.itemsQueried.Add(item.m_PublishedFileId);
			return true;
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0004B3F7 File Offset: 0x000495F7
		private static void enqueueItemToDownload(PublishedFileId_t item)
		{
			if (DedicatedUGC.itemsToDownload.Contains(item))
			{
				UnturnedLog.warn("Tried to enqueue {0} for download more than once", new object[]
				{
					item
				});
				return;
			}
			DedicatedUGC.itemsToDownload.Enqueue(item);
		}

		/// <returns>True if item was installed from cache.</returns>
		// Token: 0x06001451 RID: 5201 RVA: 0x0004B42C File Offset: 0x0004962C
		private static bool installFromCache(PublishedFileId_t fileId)
		{
			ulong num;
			string path;
			uint num2;
			if (SteamGameServerUGC.GetItemInstallInfo(fileId, out num, out path, 1024U, out num2) && ReadWrite.folderExists(path, false))
			{
				if ((SteamGameServerUGC.GetItemState(fileId) & 8U) == 8U)
				{
					CommandWindow.LogFormat("Workshop item {0} found in cache, but was flagged as needing update", new object[]
					{
						fileId
					});
				}
				else
				{
					CachedUGCDetails cachedUGCDetails;
					if (!TempSteamworksWorkshop.getCachedDetails(fileId, out cachedUGCDetails) || cachedUGCDetails.updateTimestamp <= num2)
					{
						string text = "Workshop item found in cache: ";
						PublishedFileId_t publishedFileId_t = fileId;
						CommandWindow.Log(text + publishedFileId_t.ToString());
						DedicatedUGC.installDownloadedItem(fileId, path);
						return true;
					}
					CommandWindow.LogFormat("Workshop item {0} found in cache, but remote ({1}) is newer than local ({2})", new object[]
					{
						fileId,
						DateTimeEx.FromUtcUnixTimeSeconds(cachedUGCDetails.updateTimestamp).ToLocalTime(),
						DateTimeEx.FromUtcUnixTimeSeconds(num2).ToLocalTime()
					});
				}
			}
			return false;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0004B510 File Offset: 0x00049710
		private static void installNextItem()
		{
			if (DedicatedUGC.itemsToDownload.Count == 0)
			{
				DedicatedUGC.OnFinishedDownloadingItems();
				return;
			}
			PublishedFileId_t fileId = DedicatedUGC.itemsToDownload.Dequeue();
			bool flag = false;
			if (WorkshopDownloadConfig.get().Use_Cached_Downloads)
			{
				flag = DedicatedUGC.installFromCache(fileId);
			}
			if (flag)
			{
				DedicatedUGC.installNextItem();
				return;
			}
			DedicatedUGC.currentDownload = fileId;
			string text = "Downloading workshop item: ";
			PublishedFileId_t publishedFileId_t = DedicatedUGC.currentDownload;
			CommandWindow.Log(text + publishedFileId_t.ToString());
			if (!SteamGameServerUGC.DownloadItem(DedicatedUGC.currentDownload, true))
			{
				CommandWindow.Log("Unable to download item!");
				DedicatedUGC.installNextItem();
			}
		}

		/// <summary>
		/// Used in offline-only mode.
		/// </summary>
		// Token: 0x06001453 RID: 5203 RVA: 0x0004B59C File Offset: 0x0004979C
		private static void installItemsToQueryFromCache()
		{
			CommandWindow.Log("Only installing cached workshop files (no query / download)");
			while (DedicatedUGC.itemsToQuery.Count > 0)
			{
				PublishedFileId_t publishedFileId_t = DedicatedUGC.itemsToQuery.Dequeue();
				if (!DedicatedUGC.installFromCache(publishedFileId_t))
				{
					CommandWindow.LogFormat("Unable to find workshop item in cache: {0}", new object[]
					{
						publishedFileId_t
					});
				}
			}
			DedicatedUGC.OnFinishedDownloadingItems();
		}

		/// <summary>
		/// Prepare a query that will request metadata for all the workshop items we want to install.
		/// This allows us to check if the items are allowed to be auto-downloaded to this server, and to
		/// detect any child or dependent items.
		///
		/// Waits for onQueryCompleted.
		/// </summary>
		// Token: 0x06001454 RID: 5204 RVA: 0x0004B5F4 File Offset: 0x000497F4
		private static void submitQuery()
		{
			CommandWindow.Log("Submitting workshop query for " + DedicatedUGC.itemsToQuery.Count.ToString() + " item(s)...");
			DedicatedUGC.itemsPendingQuery = DedicatedUGC.itemsToQuery.ToArray();
			DedicatedUGC.itemsToQuery.Clear();
			DedicatedUGC.submitQueryHelper(DedicatedUGC.itemsPendingQuery);
		}

		/// <summary>
		/// Re-submit previous query after a query failure.
		/// </summary>
		// Token: 0x06001455 RID: 5205 RVA: 0x0004B64C File Offset: 0x0004984C
		private static void resubmitQuery()
		{
			DedicatedUGC.queryRetryCount += 1U;
			CommandWindow.LogFormat("Re-submitting ({0} of {1}) workshop query for {2} item(s)...", new object[]
			{
				DedicatedUGC.queryRetryCount,
				DedicatedUGC.maxQueryRetries,
				DedicatedUGC.itemsPendingQuery.Length
			});
			DedicatedUGC.submitQueryHelper(DedicatedUGC.itemsPendingQuery);
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0004B6A8 File Offset: 0x000498A8
		private static void submitQueryHelper(PublishedFileId_t[] fileIDs)
		{
			DedicatedUGC.queryHandle = SteamGameServerUGC.CreateQueryUGCDetailsRequest(fileIDs, (uint)fileIDs.Length);
			SteamGameServerUGC.SetReturnKeyValueTags(DedicatedUGC.queryHandle, true);
			SteamGameServerUGC.SetReturnChildren(DedicatedUGC.queryHandle, true);
			uint query_Cache_Max_Age_Seconds = WorkshopDownloadConfig.get().Query_Cache_Max_Age_Seconds;
			if (query_Cache_Max_Age_Seconds > 0U)
			{
				SteamGameServerUGC.SetAllowCachedResponse(DedicatedUGC.queryHandle, query_Cache_Max_Age_Seconds);
			}
			SteamAPICall_t hAPICall = SteamGameServerUGC.SendQueryUGCRequest(DedicatedUGC.queryHandle);
			DedicatedUGC.queryCompleted.Set(hAPICall, null);
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0004B710 File Offset: 0x00049910
		private static bool testDownloadRestrictions(UGCQueryHandle_t queryHandle, uint resultIndex, uint ip, string itemDisplayText)
		{
			EWorkshopDownloadRestrictionResult restrictionResult = WorkshopDownloadRestrictions.getRestrictionResult(queryHandle, resultIndex, ip);
			switch (restrictionResult)
			{
			case EWorkshopDownloadRestrictionResult.NoRestrictions:
				return true;
			case EWorkshopDownloadRestrictionResult.NotWhitelisted:
				CommandWindow.LogWarning("Not authorized in the IP whitelist for " + itemDisplayText);
				return false;
			case EWorkshopDownloadRestrictionResult.Blacklisted:
				CommandWindow.LogWarning("Blocked in IP blacklist from downloading " + itemDisplayText);
				return false;
			case EWorkshopDownloadRestrictionResult.Allowed:
				CommandWindow.Log("Authorized to download " + itemDisplayText);
				return true;
			case EWorkshopDownloadRestrictionResult.Banned:
				CommandWindow.LogWarning("Workshop file is banned " + itemDisplayText);
				return false;
			case EWorkshopDownloadRestrictionResult.PrivateVisibility:
				CommandWindow.LogWarning("Workshop file is private " + itemDisplayText);
				return false;
			default:
				CommandWindow.LogWarningFormat("Unknown restriction result '{0}' for '{1}'", new object[]
				{
					restrictionResult,
					itemDisplayText
				});
				return false;
			}
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0004B7C0 File Offset: 0x000499C0
		private static void OnNextFrameResubmitQuery()
		{
			TimeUtility.updated -= DedicatedUGC.OnNextFrameResubmitQuery;
			DedicatedUGC.resubmitQuery();
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0004B7D8 File Offset: 0x000499D8
		private static void OnNextFrameSubmitQuery()
		{
			TimeUtility.updated -= DedicatedUGC.OnNextFrameSubmitQuery;
			DedicatedUGC.submitQuery();
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0004B7F0 File Offset: 0x000499F0
		private static void onQueryCompleted(SteamUGCQueryCompleted_t callback, bool ioFailure)
		{
			if (callback.m_handle != DedicatedUGC.queryHandle)
			{
				return;
			}
			bool flag;
			if (!ioFailure)
			{
				if (callback.m_eResult == EResult.k_EResultOK)
				{
					flag = false;
					CommandWindow.Log("Workshop query yielded " + callback.m_unNumResultsReturned.ToString() + " result(s)");
					uint ip;
					SteamGameServer.GetPublicIP().TryGetIPv4Address(out ip);
					string ipfromUInt = Parser.getIPFromUInt32(ip);
					CommandWindow.Log("This server's allowed IP for Workshop downloads: " + ipfromUInt);
					for (uint num = 0U; num < callback.m_unNumResultsReturned; num += 1U)
					{
						SteamUGCDetails_t steamUGCDetails_t;
						if (!SteamGameServerUGC.GetQueryUGCResult(DedicatedUGC.queryHandle, num, out steamUGCDetails_t))
						{
							CommandWindow.LogWarning(string.Format("Workshop query unable to get details for result index {0}", num));
						}
						else
						{
							PublishedFileId_t nPublishedFileId = steamUGCDetails_t.m_nPublishedFileId;
							string text = nPublishedFileId.ToString() + " '" + steamUGCDetails_t.m_rgchTitle + "'";
							if (steamUGCDetails_t.m_eResult != EResult.k_EResultOK)
							{
								CommandWindow.LogWarning(string.Format("Error {0} querying workshop file {1}", steamUGCDetails_t.m_eResult, text));
							}
							else if (DedicatedUGC.testDownloadRestrictions(DedicatedUGC.queryHandle, num, ip, text))
							{
								CachedUGCDetails cachedUGCDetails;
								TempSteamworksWorkshop.cacheDetails(DedicatedUGC.queryHandle, num, out cachedUGCDetails);
								if (steamUGCDetails_t.m_eFileType != EWorkshopFileType.k_EWorkshopFileTypeCollection)
								{
									CommandWindow.Log(text + " queued for download");
									DedicatedUGC.enqueueItemToDownload(steamUGCDetails_t.m_nPublishedFileId);
								}
								uint unNumChildren = steamUGCDetails_t.m_unNumChildren;
								if (unNumChildren > 0U)
								{
									if (WorkshopDownloadConfig.get().Ignore_Children_File_IDs.Contains(steamUGCDetails_t.m_nPublishedFileId.m_PublishedFileId))
									{
										CommandWindow.LogFormat("Ignoring {0} children of {1}", new object[]
										{
											unNumChildren,
											text
										});
									}
									else
									{
										CommandWindow.Log(text + " has " + unNumChildren.ToString() + " children");
										PublishedFileId_t[] array = new PublishedFileId_t[unNumChildren];
										if (SteamGameServerUGC.GetQueryUGCChildren(DedicatedUGC.queryHandle, num, array, unNumChildren))
										{
											foreach (PublishedFileId_t publishedFileId_t in array)
											{
												CommandWindow.LogFormat(DedicatedUGC.enqueueItemToQuery(publishedFileId_t) ? "\t{0}" : "\t{0} (already queued)", new object[]
												{
													publishedFileId_t
												});
											}
										}
									}
								}
							}
						}
					}
				}
				else
				{
					flag = true;
					CommandWindow.LogError("Encountered an error when querying workshop: " + callback.m_eResult.ToString());
				}
			}
			else
			{
				flag = true;
				CommandWindow.LogError("Encountered an IO error when querying workshop!");
			}
			SteamGameServerUGC.ReleaseQueryUGCRequest(DedicatedUGC.queryHandle);
			DedicatedUGC.queryHandle = UGCQueryHandle_t.Invalid;
			if (flag)
			{
				if (DedicatedUGC.queryRetryCount < DedicatedUGC.maxQueryRetries)
				{
					TimeUtility.updated += DedicatedUGC.OnNextFrameResubmitQuery;
					return;
				}
				CommandWindow.LogWarning("Reached maximum workshop query retry count!");
				Provider.QuitGame("reached maximum workshop query retry count");
				return;
			}
			else
			{
				if (DedicatedUGC.itemsToQuery.Count > 0)
				{
					TimeUtility.updated += DedicatedUGC.OnNextFrameSubmitQuery;
					return;
				}
				CommandWindow.Log(DedicatedUGC.itemsToDownload.Count.ToString() + " workshop item(s) to download...");
				DedicatedUGC.installNextItem();
				return;
			}
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0004BAE4 File Offset: 0x00049CE4
		private static void installDownloadedItem(PublishedFileId_t fileId, string path)
		{
			ESteamUGCType esteamUGCType;
			if (WorkshopTool.detectUGCMetaType(path, false, out esteamUGCType))
			{
				CommandWindow.LogFormat("Installing workshop item: {0}", new object[]
				{
					fileId
				});
				string text;
				if (!TempSteamworksWorkshop.isCompatible(fileId, esteamUGCType, path, out text))
				{
					CommandWindow.LogWarning(text);
				}
				string text2;
				if (TempSteamworksWorkshop.shouldIgnoreFile(fileId, out text2))
				{
					CommandWindow.LogFormat("Ignoring downloaded workshop item {0} because '{1}'", Array.Empty<object>());
				}
				else
				{
					DedicatedUGC.ugc.Add(new SteamContent(fileId, path, esteamUGCType));
					if (esteamUGCType != ESteamUGCType.MAP)
					{
						if (esteamUGCType != ESteamUGCType.LOCALIZATION)
						{
							Assets.RequestAddSearchLocation(path, TempSteamworksWorkshop.FindOrAddOrigin(fileId.m_PublishedFileId));
						}
					}
					else
					{
						WorkshopTool.loadMapBundlesAndContent(path, fileId.m_PublishedFileId);
					}
					CommandWindow.LogFormat("Installed workshop item: {0}", new object[]
					{
						fileId
					});
				}
				uint timestamp = 0U;
				CachedUGCDetails cachedUGCDetails;
				if (TempSteamworksWorkshop.getCachedDetails(fileId, out cachedUGCDetails))
				{
					timestamp = cachedUGCDetails.updateTimestamp;
				}
				Provider.registerServerUsingWorkshopFileId(fileId.m_PublishedFileId, timestamp);
				return;
			}
			string text3 = "Unable to determine UGC type for downloaded item: ";
			PublishedFileId_t publishedFileId_t = fileId;
			CommandWindow.LogWarning(text3 + publishedFileId_t.ToString());
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0004BBD8 File Offset: 0x00049DD8
		private static void onItemDownloaded(DownloadItemResult_t callback)
		{
			if (callback.m_nPublishedFileId != DedicatedUGC.currentDownload)
			{
				return;
			}
			if (callback.m_eResult == EResult.k_EResultOK)
			{
				CommandWindow.Log("Successfully downloaded workshop item: " + callback.m_nPublishedFileId.m_PublishedFileId.ToString());
				ulong num;
				string text;
				uint num2;
				if (SteamGameServerUGC.GetItemInstallInfo(callback.m_nPublishedFileId, out num, out text, 1024U, out num2))
				{
					if (ReadWrite.folderExists(text, false))
					{
						DedicatedUGC.installDownloadedItem(callback.m_nPublishedFileId, text);
					}
					else
					{
						CommandWindow.LogWarningFormat("Finished downloading workshop item {0}, but unable to find the files on disk ({1})", new object[]
						{
							callback.m_nPublishedFileId,
							text
						});
					}
				}
				else
				{
					CommandWindow.LogWarningFormat("Finished downloading workshop item {0}, but unable to get install info", new object[]
					{
						callback.m_nPublishedFileId
					});
				}
			}
			else
			{
				CommandWindow.LogWarningFormat("Download workshop item {0} failed, result: {1}", new object[]
				{
					callback.m_nPublishedFileId,
					callback.m_eResult
				});
			}
			DedicatedUGC.installNextItem();
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0004BCC8 File Offset: 0x00049EC8
		public static void initialize()
		{
			DedicatedUGC.ugc = new List<SteamContent>();
			DedicatedUGC.itemsQueried = new HashSet<ulong>();
			DedicatedUGC.itemsToQuery = new Queue<PublishedFileId_t>();
			DedicatedUGC.itemsToDownload = new Queue<PublishedFileId_t>();
			DedicatedUGC.queryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(DedicatedUGC.onQueryCompleted));
			DedicatedUGC.itemDownloaded = Callback<DownloadItemResult_t>.CreateGameServer(new Callback<DownloadItemResult_t>.DispatchDelegate(DedicatedUGC.onItemDownloaded));
			string text = string.Concat(new string[]
			{
				ReadWrite.PATH,
				ServerSavedata.directory,
				"/",
				Provider.serverID,
				"/Workshop/Steam"
			});
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			CommandWindow.Log("Workshop install folder: " + text);
			SteamGameServerUGC.BInitWorkshopForGameServer((DepotId_t)Provider.APP_ID.m_AppId, text);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0004BD92 File Offset: 0x00049F92
		private static void OnFinishedDownloadingItems()
		{
			if (Assets.ShouldWaitForNewAssetsToFinishLoading)
			{
				UnturnedLog.info("Server UGC waiting for assets to finish loading...");
				Assets.OnNewAssetsFinishedLoading = (Action)Delegate.Combine(Assets.OnNewAssetsFinishedLoading, new Action(DedicatedUGC.OnNewAssetsFinishedLoading));
				return;
			}
			DedicatedUGC.OnNewAssetsFinishedLoading();
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0004BDCC File Offset: 0x00049FCC
		private static void OnNewAssetsFinishedLoading()
		{
			Assets.OnNewAssetsFinishedLoading = (Action)Delegate.Remove(Assets.OnNewAssetsFinishedLoading, new Action(DedicatedUGC.OnNewAssetsFinishedLoading));
			if (!DedicatedUGC.linkedSpawns)
			{
				DedicatedUGC.linkedSpawns = true;
				Assets.linkSpawns();
			}
			if (!DedicatedUGC.initializedValidation)
			{
				DedicatedUGC.initializedValidation = true;
				Assets.initializeMasterBundleValidation();
			}
			DedicatedUGCInstalledHandler dedicatedUGCInstalledHandler = DedicatedUGC.installed;
			if (dedicatedUGCInstalledHandler == null)
			{
				return;
			}
			dedicatedUGCInstalledHandler();
		}

		/// <summary>
		/// Request for details about the pending items.
		/// </summary>
		// Token: 0x040006FB RID: 1787
		private static UGCQueryHandle_t queryHandle;

		/// <summary>
		/// File IDs of all the items we have enqueued for query.
		/// </summary>
		// Token: 0x040006FC RID: 1788
		private static HashSet<ulong> itemsQueried;

		/// <summary>
		/// Built from user-specified workshop item IDs, and then expanded as the query results
		/// arrive with details about any dependent or child items.
		/// </summary>
		// Token: 0x040006FD RID: 1789
		private static Queue<PublishedFileId_t> itemsToQuery;

		/// <summary>
		/// File IDs requested by the latest query submitted.
		/// </summary>
		// Token: 0x040006FE RID: 1790
		private static PublishedFileId_t[] itemsPendingQuery;

		/// <summary>
		/// Number of times we've tried re-submitted failed queries.
		/// </summary>
		// Token: 0x040006FF RID: 1791
		private static uint queryRetryCount;

		/// <summary>
		/// Built as the valid list of items arrive.
		/// </summary>
		// Token: 0x04000700 RID: 1792
		private static Queue<PublishedFileId_t> itemsToDownload;

		/// <summary>
		/// ID of the latest item we requested for download so that we can test if the callback is for us.
		/// </summary>
		// Token: 0x04000701 RID: 1793
		private static PublishedFileId_t currentDownload;

		// Token: 0x04000702 RID: 1794
		private static CallResult<SteamUGCQueryCompleted_t> queryCompleted;

		// Token: 0x04000703 RID: 1795
		private static Callback<DownloadItemResult_t> itemDownloaded;

		// Token: 0x04000704 RID: 1796
		private static bool linkedSpawns;

		// Token: 0x04000705 RID: 1797
		private static bool initializedValidation;
	}
}
