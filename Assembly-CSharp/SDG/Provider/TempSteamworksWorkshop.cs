using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SDG.SteamworksProvider;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace SDG.Provider
{
	// Token: 0x02000038 RID: 56
	public class TempSteamworksWorkshop
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000783F File Offset: 0x00005A3F
		public bool canOpenWorkshop
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007842 File Offset: 0x00005A42
		public void open(PublishedFileId_t id)
		{
			SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/filedetails/?id=" + id.m_PublishedFileId.ToString(), EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00007860 File Offset: 0x00005A60
		public List<SteamContent> ugc
		{
			get
			{
				return this._ugc;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00007868 File Offset: 0x00005A68
		public List<SteamPublished> published
		{
			get
			{
				return this._published;
			}
		}

		/// <summary>
		/// Get compatibility version from workshop query, or zero if unset.
		/// </summary>
		// Token: 0x06000190 RID: 400 RVA: 0x00007870 File Offset: 0x00005A70
		public static byte getCompatibilityVersion(UGCQueryHandle_t queryHandle, uint index)
		{
			uint queryUGCNumKeyValueTags = SteamGameServerUGC.GetQueryUGCNumKeyValueTags(queryHandle, index);
			uint num = 0U;
			while (num < queryUGCNumKeyValueTags)
			{
				string text;
				string text2;
				if (SteamGameServerUGC.GetQueryUGCKeyValueTag(queryHandle, index, num, out text, 255U, out text2, 255U) && text.Equals(TempSteamworksWorkshop.COMPATIBILITY_VERSION_KVTAG, 3))
				{
					byte result;
					if (byte.TryParse(text2, 511, CultureInfo.InvariantCulture, ref result))
					{
						return result;
					}
					UnturnedLog.warn("Unable to parse workshop item compatibility version from '{0}'", new object[]
					{
						text2
					});
					return 0;
				}
				else
				{
					num += 1U;
				}
			}
			return 0;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000078E8 File Offset: 0x00005AE8
		private static void DumpDetails(in SteamUGCDetails_t details)
		{
			string format = "{0} \"{1}\"";
			object[] array = new object[2];
			array[0] = details.m_nPublishedFileId;
			int num = 1;
			SteamUGCDetails_t steamUGCDetails_t = details;
			array[num] = steamUGCDetails_t.m_rgchTitle;
			UnturnedLog.info(format, array);
			UnturnedLog.info("\tBanned: {0}", new object[]
			{
				details.m_bBanned
			});
			UnturnedLog.info("\tResult: {0}", new object[]
			{
				details.m_eResult
			});
			UnturnedLog.info("\tVisibility: {0}", new object[]
			{
				details.m_eVisibility
			});
		}

		/// <summary>
		/// Save the details from a workshop query for lookup later.
		/// Allows game to inspect the installed files before deciding if they are
		/// compatible, since maps and localization are not affected by unity upgrades.
		/// Previously the compatibility test occurred before downloading the content.
		/// </summary>
		// Token: 0x06000192 RID: 402 RVA: 0x00007980 File Offset: 0x00005B80
		public static bool cacheDetails(UGCQueryHandle_t queryHandle, uint index, out CachedUGCDetails cachedDetails)
		{
			cachedDetails = default(CachedUGCDetails);
			SteamUGCDetails_t steamUGCDetails_t;
			bool queryUGCResult = SteamGameServerUGC.GetQueryUGCResult(queryHandle, index, out steamUGCDetails_t);
			if (queryUGCResult)
			{
				PublishedFileId_t nPublishedFileId = steamUGCDetails_t.m_nPublishedFileId;
				byte compatibilityVersion = TempSteamworksWorkshop.getCompatibilityVersion(queryHandle, index);
				cachedDetails.fileId = nPublishedFileId;
				cachedDetails.name = steamUGCDetails_t.m_rgchTitle;
				cachedDetails.compatibilityVersion = compatibilityVersion;
				cachedDetails.isBannedOrPrivate = (steamUGCDetails_t.m_bBanned || steamUGCDetails_t.m_eVisibility == ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate || steamUGCDetails_t.m_eResult == EResult.k_EResultAccessDenied);
				cachedDetails.updateTimestamp = MathfEx.Max(steamUGCDetails_t.m_rtimeCreated, steamUGCDetails_t.m_rtimeUpdated);
				TempSteamworksWorkshop.cachedUGCDetails[nPublishedFileId.m_PublishedFileId] = cachedDetails;
				if (!string.IsNullOrEmpty(cachedDetails.name))
				{
					AssetOrigin assetOrigin = Assets.FindWorkshopFileOrigin(nPublishedFileId.m_PublishedFileId);
					if (assetOrigin != null)
					{
						assetOrigin.name = string.Format("Workshop File \"{0}\" ({1})", cachedDetails.name, cachedDetails.fileId);
						return queryUGCResult;
					}
				}
			}
			else
			{
				UnturnedLog.warn("Unable to get query UGC result for caching");
			}
			return queryUGCResult;
		}

		/// <summary>
		/// Get cached workshop item details.
		/// </summary>
		// Token: 0x06000193 RID: 403 RVA: 0x00007A66 File Offset: 0x00005C66
		public static bool getCachedDetails(PublishedFileId_t fileId, out CachedUGCDetails cachedDetails)
		{
			return TempSteamworksWorkshop.cachedUGCDetails.TryGetValue(fileId.m_PublishedFileId, ref cachedDetails);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007A7C File Offset: 0x00005C7C
		public static AssetOrigin FindOrAddOrigin(ulong fileId)
		{
			AssetOrigin assetOrigin = Assets.FindOrAddWorkshopFileOrigin(fileId, true);
			CachedUGCDetails cachedUGCDetails;
			if (TempSteamworksWorkshop.cachedUGCDetails.TryGetValue(fileId, ref cachedUGCDetails) && !string.IsNullOrEmpty(cachedUGCDetails.name))
			{
				assetOrigin.name = string.Format("Workshop File \"{0}\" ({1})", cachedUGCDetails.name, cachedUGCDetails.fileId);
			}
			return assetOrigin;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00007AD0 File Offset: 0x00005CD0
		public static bool isCompatible(PublishedFileId_t fileId, ESteamUGCType type, string dir, out string explanation)
		{
			CachedUGCDetails cachedUGCDetails;
			if (!TempSteamworksWorkshop.getCachedDetails(fileId, out cachedUGCDetails))
			{
				explanation = null;
				return true;
			}
			bool flag;
			if (type != ESteamUGCType.MAP)
			{
				flag = (type != ESteamUGCType.LOCALIZATION);
			}
			else
			{
				flag = Directory.Exists(dir + "/Bundles");
			}
			if (flag)
			{
				if (cachedUGCDetails.compatibilityVersion < 2)
				{
					explanation = string.Format("Workshop version of \"{0}\" has not yet been updated from Unity 5.5 and cannot be loaded.", cachedUGCDetails.GetTitle());
					return false;
				}
				if (cachedUGCDetails.compatibilityVersion > 5)
				{
					explanation = string.Format("Workshop version of \"{0}\" has been updated to an unknown future version of Unity and cannot be loaded.", cachedUGCDetails.GetTitle());
					return false;
				}
			}
			explanation = null;
			return true;
		}

		/// <summary>
		/// Should caller skip loading a given workshop file?
		///
		/// Used to skip workshop version of map if the map is locally installed,
		/// e.g. Canyon Arena moved to workshop and auto-subscribed.
		/// </summary>
		// Token: 0x06000196 RID: 406 RVA: 0x00007B51 File Offset: 0x00005D51
		public static bool shouldIgnoreFile(PublishedFileId_t fileId, out string explanation)
		{
			if (fileId == TempSteamworksWorkshop.FRANCE && ReadWrite.fileExists("/Maps/France/Config.json", false, true))
			{
				explanation = "non-Workshop version of France is still installed";
				return true;
			}
			explanation = null;
			return false;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007B7C File Offset: 0x00005D7C
		private void onCreateItemResult(CreateItemResult_t callback, bool io)
		{
			if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement)
			{
				MenuUI.alert(MenuDashboardUI.localization.format("UGC_NeedsToAcceptWorkshopLegalAgreement"));
				return;
			}
			if (callback.m_eResult != EResult.k_EResultOK)
			{
				MenuUI.alert(MenuDashboardUI.localization.format("UGC_UnknownResult", callback.m_eResult));
				return;
			}
			if (io)
			{
				MenuUI.alert(MenuDashboardUI.localization.format("UGC_IOError"));
				return;
			}
			this.publishedFileID = callback.m_nPublishedFileId;
			this.updateUGC();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007BFC File Offset: 0x00005DFC
		private void onSubmitItemUpdateResult(SubmitItemUpdateResult_t callback, bool io)
		{
			if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement)
			{
				MenuUI.alert(MenuDashboardUI.localization.format("UGC_NeedsToAcceptWorkshopLegalAgreement"));
				return;
			}
			if (callback.m_eResult != EResult.k_EResultOK)
			{
				MenuUI.alert(MenuDashboardUI.localization.format("UGC_UnknownResult", callback.m_eResult));
				return;
			}
			if (io)
			{
				MenuUI.alert(MenuDashboardUI.localization.format("UGC_IOError"));
				return;
			}
			MenuUI.alert(MenuDashboardUI.localization.format("UGC_Success"));
			Provider.provider.workshopService.open(this.publishedFileID);
			this.refreshPublished();
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007C98 File Offset: 0x00005E98
		private void onQueryCompleted(SteamUGCQueryCompleted_t callback, bool io)
		{
			if (callback.m_eResult != EResult.k_EResultOK || io)
			{
				return;
			}
			if (callback.m_unNumResultsReturned < 1U)
			{
				return;
			}
			for (uint num = 0U; num < callback.m_unNumResultsReturned; num += 1U)
			{
				SteamUGCDetails_t steamUGCDetails_t;
				SteamUGC.GetQueryUGCResult(this.ugcRequest, num, out steamUGCDetails_t);
				SteamPublished steamPublished = new SteamPublished(steamUGCDetails_t.m_rgchTitle, steamUGCDetails_t.m_nPublishedFileId);
				this.published.Add(steamPublished);
			}
			TempSteamworksWorkshop.PublishedAdded publishedAdded = this.onPublishedAdded;
			if (publishedAdded != null)
			{
				publishedAdded();
			}
			this.cleanupUGCRequest();
			this.shouldRequestAnotherPage = true;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007D20 File Offset: 0x00005F20
		public void update()
		{
			if (this.shouldRequestAnotherPage)
			{
				this.shouldRequestAnotherPage = false;
				this.ugcRequestPage += 1U;
				this.ugcRequest = SteamUGC.CreateQueryUserUGCRequest(Provider.client.GetAccountID(), EUserUGCList.k_EUserUGCList_Published, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items, EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderAsc, SteamUtils.GetAppID(), SteamUtils.GetAppID(), this.ugcRequestPage);
				SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(this.ugcRequest);
				this.queryCompleted.Set(hAPICall, null);
			}
			ulong num;
			ulong num2;
			if (this.currentlyDownloadingFileId != PublishedFileId_t.Invalid && SteamUGC.GetItemDownloadInfo(this.currentlyDownloadingFileId, out num, out num2) && num2 > 0UL)
			{
				this.currentlyDownloadingFileEstimatedProgress = (float)(num / num2);
				if (num >= num2)
				{
					this.currentlyDownloadingFileId = PublishedFileId_t.Invalid;
				}
				this.UpdateEstimatedDownloadProgress();
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007DDC File Offset: 0x00005FDC
		private void UpdateEstimatedDownloadProgress()
		{
			float num = this.progressPerFileDownloaded * (float)(this.totalNumberOfFilesToDownload - this.installing.Count) + this.progressPerFileDownloaded * this.currentlyDownloadingFileEstimatedProgress;
			int num2 = Mathf.RoundToInt(num * 100f);
			if (this.previousEstimatedDownloadProgress != num2)
			{
				this.previousEstimatedDownloadProgress = num2;
				LoadingUI.NotifyDownloadProgress(num);
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007E35 File Offset: 0x00006035
		private void OnFinishedDownloadingItems()
		{
			if (Assets.ShouldWaitForNewAssetsToFinishLoading)
			{
				UnturnedLog.info("Client UGC waiting for assets to finish loading...");
				Assets.OnNewAssetsFinishedLoading = (Action)Delegate.Combine(Assets.OnNewAssetsFinishedLoading, new Action(this.OnNewAssetsFinishedLoading));
				return;
			}
			this.OnNewAssetsFinishedLoading();
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007E6F File Offset: 0x0000606F
		private void OnNewAssetsFinishedLoading()
		{
			Assets.OnNewAssetsFinishedLoading = (Action)Delegate.Remove(Assets.OnNewAssetsFinishedLoading, new Action(this.OnNewAssetsFinishedLoading));
			Provider.launch();
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00007E98 File Offset: 0x00006098
		public void downloadNextItem()
		{
			if (this.installing.Count == 0)
			{
				LoadingUI.SetIsDownloading(false);
				this.OnFinishedDownloadingItems();
				return;
			}
			PublishedFileId_t publishedFileId_t = this.installing[0];
			CachedUGCDetails cachedUGCDetails;
			string downloadFileName;
			if (TempSteamworksWorkshop.getCachedDetails(publishedFileId_t, out cachedUGCDetails))
			{
				downloadFileName = cachedUGCDetails.GetTitle();
			}
			else
			{
				string text = "Unknown ID ";
				PublishedFileId_t publishedFileId_t2 = publishedFileId_t;
				downloadFileName = text + publishedFileId_t2.ToString();
			}
			LoadingUI.SetDownloadFileName(downloadFileName);
			this.currentlyDownloadingFileId = publishedFileId_t;
			this.currentlyDownloadingFileEstimatedProgress = 0f;
			SteamUGC.DownloadItem(publishedFileId_t, true);
		}

		/// <summary>
		/// Helper for downloadServerItems.
		/// Called for each workshop item we want to download for the server.
		/// </summary>
		// Token: 0x0600019F RID: 415 RVA: 0x00007F1C File Offset: 0x0000611C
		private void enqueueServerItemDownloadOrInstallFromCache(PublishedFileId_t fileId)
		{
			bool flag = this.isInstalledItemAlreadyRegistered(fileId);
			string text;
			if (TempSteamworksWorkshop.shouldIgnoreFile(fileId, out text))
			{
				UnturnedLog.info("Ignoring server download {0} because '{1}'", new object[]
				{
					fileId,
					text
				});
				return;
			}
			ulong num;
			string path;
			uint num2;
			if (SteamUGC.GetItemInstallInfo(fileId, out num, out path, 1024U, out num2) && ReadWrite.folderExists(path, false))
			{
				CachedUGCDetails cachedUGCDetails;
				if ((SteamUGC.GetItemState(fileId) & 8U) == 8U)
				{
					if (flag)
					{
						UnturnedLog.info(string.Format("Server workshop file {0} is already loaded, but was flagged as needing update", fileId));
						return;
					}
					UnturnedLog.info("Server workshop item {0} found in cache, but was flagged as needing update", new object[]
					{
						fileId
					});
					this.installing.Add(fileId);
					return;
				}
				else if (TempSteamworksWorkshop.getCachedDetails(fileId, out cachedUGCDetails) && cachedUGCDetails.updateTimestamp > num2)
				{
					if (flag)
					{
						UnturnedLog.info("Server workshop file {0} is already loaded, but remote ({1}) is newer than local ({2})", new object[]
						{
							fileId,
							DateTimeEx.FromUtcUnixTimeSeconds(cachedUGCDetails.updateTimestamp).ToLocalTime(),
							DateTimeEx.FromUtcUnixTimeSeconds(num2).ToLocalTime()
						});
						return;
					}
					UnturnedLog.info("Server workshop item {0} found in cache, but remote ({1}) is newer than local ({2})", new object[]
					{
						fileId,
						DateTimeEx.FromUtcUnixTimeSeconds(cachedUGCDetails.updateTimestamp).ToLocalTime(),
						DateTimeEx.FromUtcUnixTimeSeconds(num2).ToLocalTime()
					});
					this.installing.Add(fileId);
					return;
				}
				else if (!flag)
				{
					string text2 = "Installing cached server workshop item: ";
					PublishedFileId_t publishedFileId_t = fileId;
					UnturnedLog.info(text2 + publishedFileId_t.ToString());
					this.installItemDownloadedFromServer(fileId, path);
					return;
				}
			}
			else
			{
				this.installing.Add(fileId);
			}
		}

		/// <summary>
		/// Called once we know which items the server is allowed to use (queryServerItems),
		/// or the query has failed in which case we proceed with all items it told us.
		/// </summary>
		// Token: 0x060001A0 RID: 416 RVA: 0x000080C8 File Offset: 0x000062C8
		private void downloadServerItems(List<PublishedFileId_t> itemIDs)
		{
			this.installing = new List<PublishedFileId_t>();
			Assets.loadingStats.Reset();
			foreach (PublishedFileId_t fileId in itemIDs)
			{
				this.enqueueServerItemDownloadOrInstallFromCache(fileId);
			}
			if (this.installing.Count < 1)
			{
				UnturnedLog.info("Server has {0} valid workshop item(s), but we already have them downloaded", new object[]
				{
					itemIDs.Count
				});
				this.OnFinishedDownloadingItems();
				return;
			}
			UnturnedLog.info("Server has {0} valid workshop item(s), of which {1} need to be downloaded", new object[]
			{
				itemIDs.Count,
				this.installing.Count
			});
			this.totalNumberOfFilesToDownload = this.installing.Count;
			this.progressPerFileDownloaded = 1f / (float)this.totalNumberOfFilesToDownload;
			this.previousEstimatedDownloadProgress = 0;
			LoadingUI.SetIsDownloading(true);
			LoadingUI.NotifyDownloadProgress(0f);
			this.downloadNextItem();
		}

		/// <summary>
		/// Is currently connected server allowed to auto-download the workshop item?
		/// Requested by mod authors so that they can whitelist/blacklist access.
		/// </summary>
		// Token: 0x060001A1 RID: 417 RVA: 0x000081D0 File Offset: 0x000063D0
		private bool testDownloadRestrictions(UGCQueryHandle_t queryHandle, uint resultIndex, uint ip, string itemDisplayText)
		{
			if (ip == 0U)
			{
				return true;
			}
			EWorkshopDownloadRestrictionResult restrictionResult = WorkshopDownloadRestrictions.getRestrictionResult(queryHandle, resultIndex, ip);
			switch (restrictionResult)
			{
			case EWorkshopDownloadRestrictionResult.NoRestrictions:
				return true;
			case EWorkshopDownloadRestrictionResult.NotWhitelisted:
				UnturnedLog.warn("Server is not authorized in the IP whitelist for " + itemDisplayText);
				return false;
			case EWorkshopDownloadRestrictionResult.Blacklisted:
				UnturnedLog.warn("Server is blocked in IP blacklist from downloading " + itemDisplayText);
				return false;
			case EWorkshopDownloadRestrictionResult.Allowed:
				UnturnedLog.info("Server is authorized to download " + itemDisplayText);
				return true;
			case EWorkshopDownloadRestrictionResult.Banned:
				UnturnedLog.warn("Workshop file is banned " + itemDisplayText);
				return false;
			case EWorkshopDownloadRestrictionResult.PrivateVisibility:
				UnturnedLog.warn("Workshop file is private " + itemDisplayText);
				return false;
			default:
				UnturnedLog.warn("Unknown restriction result '{0}' for '{1}'", new object[]
				{
					restrictionResult,
					itemDisplayText
				});
				return true;
			}
		}

		/// <summary>
		/// Successfully queried details of the items current server is using.
		/// Ensure server has permission to use these items, then proceed with downloading.
		/// Also caches item titles for use on the loading screen.
		/// </summary>
		// Token: 0x060001A2 RID: 418 RVA: 0x0000828C File Offset: 0x0000648C
		private void handleServerItemsQuerySuccess(SteamUGCQueryCompleted_t callback)
		{
			string ipfromUInt = Parser.getIPFromUInt32(this.serverDownloadIP);
			UnturnedLog.info("Server's allowed IP for Workshop downloads: " + ipfromUInt);
			this.serverPendingIDs = new List<PublishedFileId_t>((int)callback.m_unNumResultsReturned);
			for (uint num = 0U; num < callback.m_unNumResultsReturned; num += 1U)
			{
				CachedUGCDetails cachedUGCDetails;
				TempSteamworksWorkshop.cacheDetails(callback.m_handle, num, out cachedUGCDetails);
				if (this.testDownloadRestrictions(callback.m_handle, num, this.serverDownloadIP, cachedUGCDetails.GetTitle()))
				{
					this.serverPendingIDs.Add(cachedUGCDetails.fileId);
				}
				else
				{
					int serverInvalidItemsCount = this.serverInvalidItemsCount + 1;
					this.serverInvalidItemsCount = serverInvalidItemsCount;
				}
			}
			this.downloadServerItems(this.serverPendingIDs);
		}

		/// <summary>
		/// IO or bad result occurred when querying items the current server is using.
		/// We do not know the file details, but we proceed with downloading them all.
		/// </summary>
		// Token: 0x060001A3 RID: 419 RVA: 0x00008331 File Offset: 0x00006531
		private void handleServerItemsQueryFailed()
		{
			this.downloadServerItems(this.serverPendingIDs);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008340 File Offset: 0x00006540
		private void onServerItemsQueryCompleted(SteamUGCQueryCompleted_t callback, bool ioFailure)
		{
			if (callback.m_handle != this.serverItemsQueryHandle)
			{
				return;
			}
			if (ioFailure)
			{
				UnturnedLog.error("IO error querying workshop for server items!");
				this.handleServerItemsQueryFailed();
			}
			else if (callback.m_eResult == EResult.k_EResultOK)
			{
				this.handleServerItemsQuerySuccess(callback);
			}
			else
			{
				UnturnedLog.error("Error querying workshop for server items: " + callback.m_eResult.ToString());
				this.handleServerItemsQueryFailed();
			}
			SteamUGC.ReleaseQueryUGCRequest(this.serverItemsQueryHandle);
			this.serverItemsQueryHandle = UGCQueryHandle_t.Invalid;
		}

		/// <summary>
		/// Number of items currently connected server was not authorized to download.
		/// </summary>
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000083C6 File Offset: 0x000065C6
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x000083CE File Offset: 0x000065CE
		public int serverInvalidItemsCount { get; protected set; }

		/// <summary>
		/// Called prior to downloading, and after a connection failure.
		/// </summary>
		// Token: 0x060001A7 RID: 423 RVA: 0x000083D7 File Offset: 0x000065D7
		public void resetServerInvalidItems()
		{
			this.serverPendingIDs = null;
			this.serverInvalidItemsCount = 0;
		}

		/// <summary>
		/// Client now knows the published file IDs the server is using, but
		/// queries the workshop for additional information before installing.
		/// </summary>
		// Token: 0x060001A8 RID: 424 RVA: 0x000083E8 File Offset: 0x000065E8
		public void queryServerWorkshopItems(List<PublishedFileId_t> fileIDs, uint serverIP)
		{
			this.serverPendingIDs = fileIDs;
			this.serverDownloadIP = serverIP;
			this.serverItemsQueryHandle = SteamUGC.CreateQueryUGCDetailsRequest(fileIDs.ToArray(), (uint)fileIDs.Count);
			SteamUGC.SetReturnKeyValueTags(this.serverItemsQueryHandle, true);
			SteamUGC.SetAllowCachedResponse(this.serverItemsQueryHandle, 60U);
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(this.serverItemsQueryHandle);
			this.serverItemsQueryCompleted.Set(hAPICall, null);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00008450 File Offset: 0x00006650
		private void installItemDownloadedFromServer(PublishedFileId_t fileId, string path)
		{
			ESteamUGCType esteamUGCType;
			if (WorkshopTool.detectUGCMetaType(path, false, out esteamUGCType))
			{
				this.ugc.Add(new SteamContent(fileId, path, esteamUGCType));
				this.LoadFileIfAssetStartupAlreadyRan(fileId, path, esteamUGCType);
				return;
			}
			string text = "Unable to determine UGC type for downloaded item: ";
			PublishedFileId_t publishedFileId_t = fileId;
			UnturnedLog.warn(text + publishedFileId_t.ToString());
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000084A4 File Offset: 0x000066A4
		private void onItemDownloaded(DownloadItemResult_t callback)
		{
			if (this.installing == null || this.installing.Count == 0)
			{
				return;
			}
			if (callback.m_unAppID.m_AppId != this.appInfo.id)
			{
				return;
			}
			string text = "Workshop item downloaded: ";
			PublishedFileId_t nPublishedFileId = callback.m_nPublishedFileId;
			UnturnedLog.info(text + nPublishedFileId.ToString());
			if (callback.m_nPublishedFileId == this.currentlyDownloadingFileId)
			{
				this.currentlyDownloadingFileId = PublishedFileId_t.Invalid;
				this.currentlyDownloadingFileEstimatedProgress = 0f;
			}
			this.installing.Remove(callback.m_nPublishedFileId);
			this.UpdateEstimatedDownloadProgress();
			if (callback.m_eResult == EResult.k_EResultOK)
			{
				string text2;
				ulong num;
				string text3;
				uint num2;
				if (this.isInstalledItemAlreadyRegistered(callback.m_nPublishedFileId))
				{
					UnturnedLog.warn("Already registered newly downloaded workshop item '{0}', so ignoring this callback", new object[]
					{
						callback.m_nPublishedFileId
					});
				}
				else if (TempSteamworksWorkshop.shouldIgnoreFile(callback.m_nPublishedFileId, out text2))
				{
					UnturnedLog.info("Ignoring newly downloaded workshop item {0} because '{1}'", new object[]
					{
						callback.m_nPublishedFileId,
						text2
					});
				}
				else if (SteamUGC.GetItemInstallInfo(callback.m_nPublishedFileId, out num, out text3, 1024U, out num2))
				{
					if (ReadWrite.folderExists(text3, false))
					{
						this.installItemDownloadedFromServer(callback.m_nPublishedFileId, text3);
					}
					else
					{
						UnturnedLog.warn("Finished downloading workshop item {0}, but unable to find the files on disk ({1})", new object[]
						{
							callback.m_nPublishedFileId,
							text3
						});
					}
				}
				else
				{
					UnturnedLog.warn("Finished downloading workshop item {0}, but unable get install info", new object[]
					{
						callback.m_nPublishedFileId
					});
				}
			}
			else
			{
				UnturnedLog.warn("Download workshop item {0} failed, result: {1}", new object[]
				{
					callback.m_nPublishedFileId,
					callback.m_eResult
				});
			}
			this.downloadNextItem();
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00008660 File Offset: 0x00006860
		private void onItemInstalled(ItemInstalled_t callback)
		{
			if (callback.m_unAppID.m_AppId != this.appInfo.id)
			{
				return;
			}
			string text = "Workshop item installed: ";
			PublishedFileId_t nPublishedFileId = callback.m_nPublishedFileId;
			UnturnedLog.info(text + nPublishedFileId.ToString());
			if (this.isInstalledItemAlreadyRegistered(callback.m_nPublishedFileId))
			{
				UnturnedLog.warn("Already registered newly installed workshop item '{0}', so ignoring this callback", new object[]
				{
					callback.m_nPublishedFileId
				});
				return;
			}
			string text2;
			if (TempSteamworksWorkshop.shouldIgnoreFile(callback.m_nPublishedFileId, out text2))
			{
				UnturnedLog.info("Ignoring newly installed workshop item because '{0}'", new object[]
				{
					text2
				});
				return;
			}
			string text3;
			if (!this.getInstalledItemPath(callback.m_nPublishedFileId, out text3))
			{
				UnturnedLog.warn("Unable to determine newly installed workshop item '{0}' file path", new object[]
				{
					callback.m_nPublishedFileId
				});
				return;
			}
			ESteamUGCType esteamUGCType;
			if (!WorkshopTool.detectUGCMetaType(text3, false, out esteamUGCType))
			{
				UnturnedLog.warn("Unable to determine newly installed workshop item '{0}' type", new object[]
				{
					callback.m_nPublishedFileId
				});
				return;
			}
			this.ugc.Add(new SteamContent(callback.m_nPublishedFileId, text3, esteamUGCType));
			this.LoadFileIfAssetStartupAlreadyRan(callback.m_nPublishedFileId, text3, esteamUGCType);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000877C File Offset: 0x0000697C
		private void LoadFileIfAssetStartupAlreadyRan(PublishedFileId_t fileId, string path, ESteamUGCType type)
		{
			if (!((type == ESteamUGCType.MAP) ? Assets.hasLoadedMaps : Assets.hasLoadedUgc))
			{
				UnturnedLog.info(string.Format("Workshop file {0} not requesting load because asset refresh is in progress", fileId));
				return;
			}
			if (type != ESteamUGCType.MAP)
			{
				if (type != ESteamUGCType.LOCALIZATION)
				{
					Assets.RequestAddSearchLocation(path, TempSteamworksWorkshop.FindOrAddOrigin(fileId.m_PublishedFileId));
				}
				return;
			}
			WorkshopTool.loadMapBundlesAndContent(path, fileId.m_PublishedFileId);
			Level.broadcastLevelsRefreshed();
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000087DC File Offset: 0x000069DC
		private void cleanupUGCRequest()
		{
			if (this.ugcRequest == UGCQueryHandle_t.Invalid)
			{
				return;
			}
			SteamUGC.ReleaseQueryUGCRequest(this.ugcRequest);
			this.ugcRequest = UGCQueryHandle_t.Invalid;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00008808 File Offset: 0x00006A08
		public void prepareUGC(string name, string description, string path, string preview, string change, ESteamUGCType type, string tag, string allowedIPs, ESteamUGCVisibility visibility)
		{
			bool verified = File.Exists(path + "/Skin.kvt");
			this.prepareUGC(name, description, path, preview, change, type, tag, allowedIPs, visibility, verified);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000883C File Offset: 0x00006A3C
		public void prepareUGC(string name, string description, string path, string preview, string change, ESteamUGCType type, string tag, string allowedIPs, ESteamUGCVisibility visibility, bool verified)
		{
			this.ugcName = name;
			this.ugcDescription = description;
			this.ugcPath = path;
			this.ugcPreview = preview;
			this.ugcChange = change;
			this.ugcType = type;
			this.ugcTag = tag;
			this.ugcAllowedIPs = allowedIPs;
			this.ugcVisibility = visibility;
			this.ugcVerified = verified;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00008896 File Offset: 0x00006A96
		public void prepareUGC(PublishedFileId_t id)
		{
			this.publishedFileID = id;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000088A0 File Offset: 0x00006AA0
		public void createUGC(bool ugcFor)
		{
			SteamAPICall_t hAPICall = SteamUGC.CreateItem(SteamUtils.GetAppID(), ugcFor ? EWorkshopFileType.k_EWorkshopFileTypeMicrotransaction : EWorkshopFileType.k_EWorkshopFileTypeFirst);
			this.createItemResult.Set(hAPICall, null);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000088CC File Offset: 0x00006ACC
		public void updateUGC()
		{
			UGCUpdateHandle_t ugcupdateHandle_t = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), this.publishedFileID);
			if (this.ugcType == ESteamUGCType.MAP)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Map.meta", false, false, new byte[1]);
			}
			else if (this.ugcType == ESteamUGCType.LOCALIZATION)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Localization.meta", false, false, new byte[1]);
			}
			else if (this.ugcType == ESteamUGCType.OBJECT)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Object.meta", false, false, new byte[1]);
			}
			else if (this.ugcType == ESteamUGCType.ITEM)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Item.meta", false, false, new byte[1]);
			}
			else if (this.ugcType == ESteamUGCType.VEHICLE)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Vehicle.meta", false, false, new byte[1]);
			}
			else if (this.ugcType == ESteamUGCType.SKIN)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Skin.meta", false, false, new byte[1]);
			}
			SteamUGC.SetItemContent(ugcupdateHandle_t, this.ugcPath);
			if (this.ugcDescription.Length > 0)
			{
				SteamUGC.SetItemDescription(ugcupdateHandle_t, this.ugcDescription);
			}
			if (this.ugcPreview.Length > 0)
			{
				SteamUGC.SetItemPreview(ugcupdateHandle_t, this.ugcPreview);
			}
			List<string> list = new List<string>();
			if (this.ugcType == ESteamUGCType.MAP)
			{
				list.Add("Map");
			}
			else if (this.ugcType == ESteamUGCType.LOCALIZATION)
			{
				list.Add("Localization");
			}
			else if (this.ugcType == ESteamUGCType.OBJECT)
			{
				list.Add("Object");
			}
			else if (this.ugcType == ESteamUGCType.ITEM)
			{
				list.Add("Item");
			}
			else if (this.ugcType == ESteamUGCType.VEHICLE)
			{
				list.Add("Vehicle");
			}
			else if (this.ugcType == ESteamUGCType.SKIN)
			{
				list.Add("Skin");
			}
			if (this.ugcTag != null && this.ugcTag.Length > 0)
			{
				list.Add(this.ugcTag);
			}
			if (this.ugcVerified)
			{
				list.Add("Verified");
			}
			SteamUGC.SetItemTags(ugcupdateHandle_t, list.ToArray());
			if (this.ugcName.Length > 0)
			{
				SteamUGC.SetItemTitle(ugcupdateHandle_t, this.ugcName);
			}
			SteamUGC.RemoveItemKeyValueTags(ugcupdateHandle_t, WorkshopDownloadRestrictions.IP_RESTRICTIONS_KVTAG);
			if (!string.IsNullOrEmpty(this.ugcAllowedIPs))
			{
				SteamUGC.AddItemKeyValueTag(ugcupdateHandle_t, WorkshopDownloadRestrictions.IP_RESTRICTIONS_KVTAG, this.ugcAllowedIPs);
			}
			SteamUGC.RemoveItemKeyValueTags(ugcupdateHandle_t, TempSteamworksWorkshop.COMPATIBILITY_VERSION_KVTAG);
			SteamUGC.AddItemKeyValueTag(ugcupdateHandle_t, TempSteamworksWorkshop.COMPATIBILITY_VERSION_KVTAG, 5.ToString());
			if (this.ugcVisibility == ESteamUGCVisibility.PUBLIC)
			{
				SteamUGC.SetItemVisibility(ugcupdateHandle_t, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic);
			}
			else if (this.ugcVisibility == ESteamUGCVisibility.FRIENDS_ONLY)
			{
				SteamUGC.SetItemVisibility(ugcupdateHandle_t, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityFriendsOnly);
			}
			else if (this.ugcVisibility == ESteamUGCVisibility.PRIVATE)
			{
				SteamUGC.SetItemVisibility(ugcupdateHandle_t, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate);
			}
			else if (this.ugcVisibility == ESteamUGCVisibility.UNLISTED)
			{
				SteamUGC.SetItemVisibility(ugcupdateHandle_t, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityUnlisted);
			}
			SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(ugcupdateHandle_t, this.ugcChange);
			this.submitItemUpdateResult.Set(hAPICall, null);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00008BB4 File Offset: 0x00006DB4
		private bool isInstalledItemAlreadyRegistered(PublishedFileId_t fileId)
		{
			using (List<SteamContent>.Enumerator enumerator = this.ugc.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.publishedFileID == fileId)
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Get path to an already-installed workshop item.
		/// </summary>
		/// <returns>True if the path was found.</returns>
		// Token: 0x060001B4 RID: 436 RVA: 0x00008C14 File Offset: 0x00006E14
		private bool getInstalledItemPath(PublishedFileId_t fileId, out string path)
		{
			EItemState itemState = (EItemState)SteamUGC.GetItemState(fileId);
			if ((itemState & EItemState.k_EItemStateInstalled) != EItemState.k_EItemStateInstalled)
			{
				UnturnedLog.warn("Installed item {0} state flags missing k_EItemStateInstalled: {1}", new object[]
				{
					fileId,
					itemState
				});
			}
			ulong num;
			uint num2;
			return SteamUGC.GetItemInstallInfo(fileId, out num, out path, 1024U, out num2) && ReadWrite.folderExists(path, false);
		}

		/// <summary>
		/// Used during startup to register subscribed workshop items.
		/// Given a workshop item file id, if its files exist on disk then register it.
		/// </summary>
		// Token: 0x060001B5 RID: 437 RVA: 0x00008C6C File Offset: 0x00006E6C
		private void registerInstalledItem(PublishedFileId_t fileId)
		{
			if (this.isInstalledItemAlreadyRegistered(fileId))
			{
				return;
			}
			string text;
			if (TempSteamworksWorkshop.shouldIgnoreFile(fileId, out text))
			{
				UnturnedLog.info("Ignoring subscribed item {0} because '{1}'", new object[]
				{
					fileId,
					text
				});
				return;
			}
			string text2;
			if (!this.getInstalledItemPath(fileId, out text2))
			{
				UnturnedLog.warn("Unable to register installed item during startup: {0}\nPath:{1}", new object[]
				{
					fileId,
					text2
				});
				return;
			}
			ESteamUGCType esteamUGCType;
			if (!WorkshopTool.detectUGCMetaType(text2, false, out esteamUGCType))
			{
				string text3 = "Unable to determine UGC type for installed item: ";
				PublishedFileId_t publishedFileId_t = fileId;
				UnturnedLog.warn(text3 + publishedFileId_t.ToString());
				return;
			}
			string error;
			if (!TempSteamworksWorkshop.isCompatible(fileId, esteamUGCType, text2, out error))
			{
				Assets.reportError(error);
				return;
			}
			this.ugc.Add(new SteamContent(fileId, text2, esteamUGCType));
			if (!LocalWorkshopSettings.get().getEnabled(fileId))
			{
				return;
			}
			this.LoadFileIfAssetStartupAlreadyRan(fileId, text2, esteamUGCType);
		}

		/// <summary>
		/// Called when subscribed items callback was successful to register all compatible files.
		/// </summary>
		// Token: 0x060001B6 RID: 438 RVA: 0x00008D3C File Offset: 0x00006F3C
		private void handleSubscribedItemsCallbackSuccess(SteamUGCQueryCompleted_t callback)
		{
			UnturnedLog.info(string.Format("Received details for {0} subscribed workshop file(s)", callback.m_unNumResultsReturned));
			for (uint num = 0U; num < callback.m_unNumResultsReturned; num += 1U)
			{
				CachedUGCDetails cachedUGCDetails;
				if (TempSteamworksWorkshop.cacheDetails(callback.m_handle, num, out cachedUGCDetails))
				{
					UnturnedLog.info(string.Format("Subscribed workshop file {0} of {1}: \"{2}\" ({3})", new object[]
					{
						num + 1U,
						callback.m_unNumResultsReturned,
						cachedUGCDetails.name,
						cachedUGCDetails.fileId
					}));
					this.registerInstalledItem(cachedUGCDetails.fileId);
				}
			}
		}

		/// <summary>
		/// Called when subscribed items callback did not execute as expected,
		/// maybe because steam's servers are offline. In this case we can't check
		/// compatibility so we register all the locally subscribed items as compatible.
		/// </summary>
		// Token: 0x060001B7 RID: 439 RVA: 0x00008DD8 File Offset: 0x00006FD8
		private void handleSubscribedItemsCallbackFailed()
		{
			UnturnedLog.info("Registering {0} locally subscribed item(s)", new object[]
			{
				this.locallySubscribedFileIds.Length
			});
			foreach (PublishedFileId_t fileId in this.locallySubscribedFileIds)
			{
				this.registerInstalledItem(fileId);
			}
		}

		/// <summary>
		/// Register any localization-type workshop content before waiting for the steam callbacks.
		/// Important so that localizations are available for loading screens and whatnot during startup.
		/// Any items we register now will be skipped later.
		/// </summary>
		// Token: 0x060001B8 RID: 440 RVA: 0x00008E2C File Offset: 0x0000702C
		private void registerLocalizations()
		{
			foreach (PublishedFileId_t fileId in this.locallySubscribedFileIds)
			{
				string path;
				ESteamUGCType esteamUGCType;
				if (this.getInstalledItemPath(fileId, out path) && WorkshopTool.detectUGCMetaType(path, false, out esteamUGCType) && esteamUGCType == ESteamUGCType.LOCALIZATION)
				{
					this.registerInstalledItem(fileId);
				}
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008E78 File Offset: 0x00007078
		private void onSubscribedQueryCompleted(SteamUGCQueryCompleted_t callback, bool ioFailure)
		{
			if (callback.m_handle != this.subscribedQueryHandle)
			{
				return;
			}
			if (!ioFailure)
			{
				if (callback.m_eResult == EResult.k_EResultOK)
				{
					this.handleSubscribedItemsCallbackSuccess(callback);
				}
				else
				{
					UnturnedLog.error("Encountered an error when querying workshop for subscribed items: " + callback.m_eResult.ToString());
					this.handleSubscribedItemsCallbackFailed();
				}
			}
			else
			{
				UnturnedLog.error("Encountered an IO error when querying workshop for subscribed items!");
				this.handleSubscribedItemsCallbackFailed();
			}
			SteamUGC.ReleaseQueryUGCRequest(this.subscribedQueryHandle);
			this.subscribedQueryHandle = UGCQueryHandle_t.Invalid;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00008F00 File Offset: 0x00007100
		public void refreshUGC()
		{
			this._ugc = new List<SteamContent>();
			uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
			if (numSubscribedItems < 1U)
			{
				UnturnedLog.info("Found zero workshop file subscriptions");
				return;
			}
			if (TempSteamworksWorkshop.shouldIgnoreSubscribedItems)
			{
				UnturnedLog.info("Ignoring all workshop file subscriptions");
				return;
			}
			this.locallySubscribedFileIds = new PublishedFileId_t[numSubscribedItems];
			SteamUGC.GetSubscribedItems(this.locallySubscribedFileIds, numSubscribedItems);
			UnturnedLog.info("Subscribed workshop file ID(s): " + string.Join<PublishedFileId_t>(", ", this.locallySubscribedFileIds));
			this.registerLocalizations();
			UnturnedLog.info("Querying details for subscribed workshop files...");
			this.subscribedQueryHandle = SteamUGC.CreateQueryUGCDetailsRequest(this.locallySubscribedFileIds, numSubscribedItems);
			SteamUGC.SetReturnKeyValueTags(this.subscribedQueryHandle, true);
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(this.subscribedQueryHandle);
			this.subscribedQueryCompleted.Set(hAPICall, null);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00008FC4 File Offset: 0x000071C4
		public void refreshPublished()
		{
			TempSteamworksWorkshop.PublishedRemoved publishedRemoved = this.onPublishedRemoved;
			if (publishedRemoved != null)
			{
				publishedRemoved();
			}
			this.cleanupUGCRequest();
			this._published = new List<SteamPublished>();
			this.ugcRequestPage = 1U;
			this.shouldRequestAnotherPage = false;
			this.ugcRequest = SteamUGC.CreateQueryUserUGCRequest(Provider.client.GetAccountID(), EUserUGCList.k_EUserUGCList_Published, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items, EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderAsc, SteamUtils.GetAppID(), SteamUtils.GetAppID(), this.ugcRequestPage);
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(this.ugcRequest);
			this.queryCompleted.Set(hAPICall, null);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00009048 File Offset: 0x00007248
		public bool getSubscribed(ulong fileId)
		{
			if (this.ugc == null)
			{
				return false;
			}
			PublishedFileId_t publishedFileId_t = new PublishedFileId_t(fileId);
			bool result;
			if (this.ingameSubscriptions.TryGetValue(publishedFileId_t, ref result))
			{
				return result;
			}
			using (List<SteamContent>.Enumerator enumerator = this.ugc.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.publishedFileID == publishedFileId_t)
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Called by us when we subscribe to an item from in-game.
		/// If item already exists on-disk steam doesn't always call onItemInstalled, so we do our own check and potentially load.
		/// </summary>
		// Token: 0x060001BD RID: 445 RVA: 0x000090CC File Offset: 0x000072CC
		private void gameSubscribed(PublishedFileId_t fileId)
		{
			if (this.isInstalledItemAlreadyRegistered(fileId))
			{
				return;
			}
			EItemState itemState = (EItemState)SteamUGC.GetItemState(fileId);
			if ((itemState & EItemState.k_EItemStateInstalled) != EItemState.k_EItemStateInstalled)
			{
				return;
			}
			if ((itemState & EItemState.k_EItemStateDownloading) == EItemState.k_EItemStateDownloading)
			{
				return;
			}
			if ((itemState & EItemState.k_EItemStateDownloadPending) == EItemState.k_EItemStateDownloading)
			{
				return;
			}
			UnturnedLog.info("Triggering a fake onItemInstalled callback for {0} because game subscribed to a pre-installed item", new object[]
			{
				fileId
			});
			ItemInstalled_t callback;
			callback.m_unAppID.m_AppId = this.appInfo.id;
			callback.m_nPublishedFileId = fileId;
			this.onItemInstalled(callback);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009144 File Offset: 0x00007344
		public void setSubscribed(ulong fileId, bool subscribe)
		{
			PublishedFileId_t publishedFileId_t = new PublishedFileId_t(fileId);
			if (subscribe)
			{
				SteamUGC.SubscribeItem(publishedFileId_t);
				UnturnedLog.info("Game subscribed to " + fileId.ToString());
				this.gameSubscribed(publishedFileId_t);
			}
			else
			{
				SteamUGC.UnsubscribeItem(publishedFileId_t);
				UnturnedLog.info("Game un-subscribed from " + fileId.ToString());
			}
			this.ingameSubscriptions[publishedFileId_t] = subscribe;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000091AC File Offset: 0x000073AC
		public TempSteamworksWorkshop(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
			this.downloaded = new List<PublishedFileId_t>();
			if (!this.appInfo.isDedicated)
			{
				this.createItemResult = CallResult<CreateItemResult_t>.Create(new CallResult<CreateItemResult_t>.APIDispatchDelegate(this.onCreateItemResult));
				this.submitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(this.onSubmitItemUpdateResult));
				this.queryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.onQueryCompleted));
				this.itemDownloaded = Callback<DownloadItemResult_t>.Create(new Callback<DownloadItemResult_t>.DispatchDelegate(this.onItemDownloaded));
				this.itemInstalled = Callback<ItemInstalled_t>.Create(new Callback<ItemInstalled_t>.DispatchDelegate(this.onItemInstalled));
				this.subscribedQueryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.onSubscribedQueryCompleted));
				this.serverItemsQueryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.onServerItemsQueryCompleted));
			}
		}

		/// <summary>
		/// Used when transitioning Unity versions breaks asset bundles. Replaced by AssetBundleVersion const values.
		/// </summary>
		// Token: 0x040000B7 RID: 183
		[Obsolete]
		public static readonly byte COMPATIBILITY_VERSION = 3;

		/// <summary>
		/// Workshop item key-value tag storing the version number.
		/// </summary>
		// Token: 0x040000B8 RID: 184
		public static readonly string COMPATIBILITY_VERSION_KVTAG = "compatibility_version";

		// Token: 0x040000B9 RID: 185
		private SteamworksAppInfo appInfo;

		// Token: 0x040000BA RID: 186
		public TempSteamworksWorkshop.PublishedAdded onPublishedAdded;

		// Token: 0x040000BB RID: 187
		public TempSteamworksWorkshop.PublishedRemoved onPublishedRemoved;

		// Token: 0x040000BC RID: 188
		private PublishedFileId_t publishedFileID;

		// Token: 0x040000BD RID: 189
		private UGCQueryHandle_t ugcRequest;

		// Token: 0x040000BE RID: 190
		private uint ugcRequestPage;

		// Token: 0x040000BF RID: 191
		private bool shouldRequestAnotherPage;

		// Token: 0x040000C0 RID: 192
		private string ugcName;

		// Token: 0x040000C1 RID: 193
		private string ugcDescription;

		// Token: 0x040000C2 RID: 194
		private string ugcPath;

		// Token: 0x040000C3 RID: 195
		private string ugcPreview;

		// Token: 0x040000C4 RID: 196
		private string ugcChange;

		// Token: 0x040000C5 RID: 197
		private ESteamUGCType ugcType;

		// Token: 0x040000C6 RID: 198
		private string ugcTag;

		// Token: 0x040000C7 RID: 199
		private string ugcAllowedIPs;

		// Token: 0x040000C8 RID: 200
		private ESteamUGCVisibility ugcVisibility;

		// Token: 0x040000C9 RID: 201
		private bool ugcVerified;

		// Token: 0x040000CA RID: 202
		public int totalNumberOfFilesToDownload;

		// Token: 0x040000CB RID: 203
		private float progressPerFileDownloaded;

		// Token: 0x040000CC RID: 204
		public List<PublishedFileId_t> downloaded;

		// Token: 0x040000CD RID: 205
		public List<PublishedFileId_t> installing;

		// Token: 0x040000CE RID: 206
		private List<SteamContent> _ugc;

		// Token: 0x040000CF RID: 207
		private List<SteamPublished> _published;

		/// <summary>
		/// Maps published file id to name, version, etc.
		/// </summary>
		// Token: 0x040000D0 RID: 208
		private static Dictionary<ulong, CachedUGCDetails> cachedUGCDetails = new Dictionary<ulong, CachedUGCDetails>();

		// Token: 0x040000D1 RID: 209
		private static readonly PublishedFileId_t FRANCE = new PublishedFileId_t(1975500516UL);

		// Token: 0x040000D2 RID: 210
		private CallResult<CreateItemResult_t> createItemResult;

		// Token: 0x040000D3 RID: 211
		private CallResult<SubmitItemUpdateResult_t> submitItemUpdateResult;

		// Token: 0x040000D4 RID: 212
		private CallResult<SteamUGCQueryCompleted_t> queryCompleted;

		// Token: 0x040000D5 RID: 213
		private float currentlyDownloadingFileEstimatedProgress;

		// Token: 0x040000D6 RID: 214
		private int previousEstimatedDownloadProgress;

		// Token: 0x040000D7 RID: 215
		private PublishedFileId_t currentlyDownloadingFileId;

		// Token: 0x040000D8 RID: 216
		private UGCQueryHandle_t serverItemsQueryHandle;

		// Token: 0x040000D9 RID: 217
		private CallResult<SteamUGCQueryCompleted_t> serverItemsQueryCompleted;

		/// <summary>
		/// File IDs the client knows the server is using. Fallback in-case the query fails.
		/// </summary>
		// Token: 0x040000DA RID: 218
		internal List<PublishedFileId_t> serverPendingIDs;

		/// <summary>
		/// IP of the currently connected server, or zero if unable to retrieve from network system.
		/// Used for testing download restrictions.
		/// </summary>
		// Token: 0x040000DB RID: 219
		protected uint serverDownloadIP;

		// Token: 0x040000DD RID: 221
		private Callback<DownloadItemResult_t> itemDownloaded;

		/// <summary>
		/// Callback when player subscribes to an item and it finishes downloading.
		/// Different than the game-managed DownloadItem calls.
		/// </summary>
		// Token: 0x040000DE RID: 222
		private Callback<ItemInstalled_t> itemInstalled;

		/// <summary>
		/// Workshop file ids we were locally subscribed to during startup.
		/// These items are queried for compatibility before registering.
		/// </summary>
		// Token: 0x040000DF RID: 223
		private PublishedFileId_t[] locallySubscribedFileIds;

		// Token: 0x040000E0 RID: 224
		private UGCQueryHandle_t subscribedQueryHandle;

		// Token: 0x040000E1 RID: 225
		private CallResult<SteamUGCQueryCompleted_t> subscribedQueryCompleted;

		/// <summary>
		/// If specified, player's workshop file subscriptions are not registered at startup.
		/// </summary>
		// Token: 0x040000E2 RID: 226
		private static CommandLineFlag shouldIgnoreSubscribedItems = new CommandLineFlag(false, "-NoWorkshopSubscriptions");

		/// <summary>
		/// Map of subscriptions added/removed by the player through the in-game client API, as opposed to the web browser.
		/// </summary>
		// Token: 0x040000E3 RID: 227
		private Dictionary<PublishedFileId_t, bool> ingameSubscriptions = new Dictionary<PublishedFileId_t, bool>();

		// Token: 0x0200084C RID: 2124
		// (Invoke) Token: 0x060047B5 RID: 18357
		public delegate void PublishedAdded();

		// Token: 0x0200084D RID: 2125
		// (Invoke) Token: 0x060047B9 RID: 18361
		public delegate void PublishedRemoved();
	}
}
