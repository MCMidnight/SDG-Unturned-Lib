using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SDG.NetTransport;
using SDG.SteamworksProvider.Services.Store;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000793 RID: 1939
	public class MenuDashboardUI
	{
		// Token: 0x06004059 RID: 16473 RVA: 0x0014B134 File Offset: 0x00149334
		public static void open()
		{
			if (MenuDashboardUI.active)
			{
				return;
			}
			MenuDashboardUI.active = true;
			MenuDashboardUI.container.AnimateIntoView();
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x0014B14E File Offset: 0x0014934E
		public static void close()
		{
			if (!MenuDashboardUI.active)
			{
				return;
			}
			MenuDashboardUI.active = false;
			MenuDashboardUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x0014B172 File Offset: 0x00149372
		private static void onClickedPlayButton(ISleekElement button)
		{
			MenuPlayUI.open();
			MenuDashboardUI.close();
			MenuTitleUI.close();
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0014B183 File Offset: 0x00149383
		private static void onClickedSurvivorsButton(ISleekElement button)
		{
			MenuSurvivorsUI.open();
			MenuDashboardUI.close();
			MenuTitleUI.close();
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x0014B194 File Offset: 0x00149394
		private static void onClickedConfigurationButton(ISleekElement button)
		{
			MenuConfigurationUI.open();
			MenuDashboardUI.close();
			MenuTitleUI.close();
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x0014B1A5 File Offset: 0x001493A5
		private static void onClickedWorkshopButton(ISleekElement button)
		{
			MenuWorkshopUI.open();
			MenuDashboardUI.close();
			MenuTitleUI.close();
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x0014B1B6 File Offset: 0x001493B6
		private static void onClickedExitButton(ISleekElement button)
		{
			MenuPauseUI.open();
			MenuDashboardUI.close();
			MenuTitleUI.close();
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x0014B1C7 File Offset: 0x001493C7
		private static void onClickedProButton(ISleekElement button)
		{
			Provider.provider.storeService.open(new SteamworksStorePackageID(Provider.PRO_ID.m_AppId));
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x0014B1E7 File Offset: 0x001493E7
		private static void onClickedAlertButton(ISleekElement button)
		{
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x0014B1EC File Offset: 0x001493EC
		private static void InsertSteamBbCode(ISleekElement parent, string contents, bool useLinkFiltering)
		{
			if (string.IsNullOrEmpty(contents))
			{
				return;
			}
			SteamBBCodeUtils.removeYouTubePreviews(ref contents);
			SteamBBCodeUtils.removeCodeFormatting(ref contents);
			int num = 0;
			for (int i = 0; i < 1000; i++)
			{
				int num2 = contents.IndexOf("[h1]", num + 1);
				int num3 = contents.IndexOf("[b]", num + 1);
				int num4;
				if (num2 == -1 && num3 == -1)
				{
					num4 = -1;
				}
				else if (num2 == -1)
				{
					num4 = num3;
				}
				else if (num3 == -1)
				{
					num4 = num2;
				}
				else if (num2 < num3)
				{
					num4 = num2;
				}
				else
				{
					num4 = num3;
				}
				string text;
				if (num4 == -1)
				{
					text = contents.Substring(num);
				}
				else
				{
					text = contents.Substring(num, num4 - num);
				}
				List<SubcontentInfo> list = new List<SubcontentInfo>();
				int num5 = 0;
				for (int j = 0; j < 1000; j++)
				{
					int num6 = text.IndexOf("[img]", num5);
					int num7 = text.IndexOf("[url=", num5);
					if (num6 == -1 && num7 == -1)
					{
						list.Add(new SubcontentInfo
						{
							content = text.Substring(num5)
						});
						break;
					}
					int num8;
					bool flag;
					if (num6 == -1)
					{
						num8 = num7;
						flag = false;
					}
					else if (num7 == -1)
					{
						num8 = num6;
						flag = true;
					}
					else if (num6 < num7)
					{
						num8 = num6;
						flag = true;
					}
					else
					{
						num8 = num7;
						flag = false;
					}
					list.Add(new SubcontentInfo
					{
						content = text.Substring(num5, num8 - num5)
					});
					int num10;
					if (flag)
					{
						int num9 = text.IndexOf("[/img]", num6);
						string url = text.Substring(num6 + 5, num9 - num6 - 5);
						list.Add(new SubcontentInfo
						{
							url = url,
							isImage = true
						});
						num10 = num9;
					}
					else
					{
						int num11 = text.IndexOf("[/url]", num7);
						int num12 = text.IndexOf("]", num7);
						string url2 = text.Substring(num7 + 5, num12 - num7 - 5);
						string content = text.Substring(num12 + 1, num11 - num12 - 1);
						list.Add(new SubcontentInfo
						{
							content = content,
							url = url2,
							isLink = true
						});
						num10 = num11;
					}
					num5 = num10 + 6;
				}
				foreach (SubcontentInfo subcontentInfo in list)
				{
					if (subcontentInfo.isImage)
					{
						SleekWebImage sleekWebImage = new SleekWebImage();
						sleekWebImage.UseManualLayout = false;
						sleekWebImage.UseWidthLayoutOverride = true;
						sleekWebImage.UseHeightLayoutOverride = true;
						sleekWebImage.useImageDimensions = true;
						subcontentInfo.url = subcontentInfo.url.Replace("{STEAM_CLAN_IMAGE}", "https://clan.cloudflare.steamstatic.com/images/");
						sleekWebImage.Refresh(subcontentInfo.url, false);
						parent.AddChild(sleekWebImage);
					}
					else if (subcontentInfo.isLink)
					{
						if (!useLinkFiltering || WebUtils.CanParseThirdPartyUrl(subcontentInfo.url, true, true))
						{
							parent.AddChild(new SleekWebLinkButton
							{
								Text = subcontentInfo.content,
								Url = subcontentInfo.url,
								UseManualLayout = false,
								UseChildAutoLayout = 1,
								UseHeightLayoutOverride = true,
								ExpandChildren = true,
								SizeOffset_Y = 30f,
								useLinkFiltering = useLinkFiltering
							});
						}
						else
						{
							UnturnedLog.warn("Ignoring potentially unsafe link in BBcode: {0}", new object[]
							{
								subcontentInfo.url
							});
						}
					}
					else
					{
						subcontentInfo.content = subcontentInfo.content.TrimStart(new char[]
						{
							'\r',
							'\n'
						});
						subcontentInfo.content = subcontentInfo.content.Replace("[b]", "<b>");
						subcontentInfo.content = subcontentInfo.content.Replace("[/b]", "</b>");
						subcontentInfo.content = subcontentInfo.content.Replace("[i]", "<i>");
						subcontentInfo.content = subcontentInfo.content.Replace("[/i]", "</i>");
						subcontentInfo.content = subcontentInfo.content.Replace("[list]", "");
						subcontentInfo.content = subcontentInfo.content.Replace("[/list]", "");
						subcontentInfo.content = subcontentInfo.content.Replace("[*]", "- ");
						subcontentInfo.content = subcontentInfo.content.Replace("[h1]", "<size=14>");
						subcontentInfo.content = subcontentInfo.content.Replace("[/h1]", "</size>");
						subcontentInfo.content = subcontentInfo.content.TrimEnd(new char[]
						{
							'\r',
							'\n'
						});
						if (!string.IsNullOrEmpty(subcontentInfo.content))
						{
							string[] array = subcontentInfo.content.Split(new char[]
							{
								'\r',
								'\n'
							});
							string text2 = string.Empty;
							string[] array2 = array;
							for (int k = 0; k < array2.Length; k++)
							{
								string text3 = array2[k].Trim();
								if (!string.IsNullOrEmpty(text3))
								{
									if (text3.StartsWith("- "))
									{
										if (!string.IsNullOrEmpty(text2))
										{
											text2 += "\n";
										}
										text2 += text3;
									}
									else
									{
										if (!string.IsNullOrEmpty(text2))
										{
											ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
											sleekLabel.Text = text2;
											sleekLabel.UseManualLayout = false;
											sleekLabel.AllowRichText = true;
											sleekLabel.TextAlignment = 0;
											parent.AddChild(sleekLabel);
										}
										text2 = text3;
										ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
										sleekLabel2.Text = text2;
										sleekLabel2.UseManualLayout = false;
										sleekLabel2.AllowRichText = true;
										sleekLabel2.TextAlignment = 0;
										parent.AddChild(sleekLabel2);
										text2 = string.Empty;
									}
								}
							}
							if (!string.IsNullOrEmpty(text2))
							{
								ISleekLabel sleekLabel3 = Glazier.Get().CreateLabel();
								sleekLabel3.Text = text2;
								sleekLabel3.UseManualLayout = false;
								sleekLabel3.AllowRichText = true;
								sleekLabel3.TextAlignment = 0;
								parent.AddChild(sleekLabel3);
							}
						}
					}
				}
				if (num4 == -1)
				{
					break;
				}
				num = num4;
			}
		}

		/// <summary>
		/// Called after newsResponse is updated from web request.
		/// </summary>
		// Token: 0x06004063 RID: 16483 RVA: 0x0014B828 File Offset: 0x00149A28
		private static void receiveNewsResponse()
		{
			for (int i = 0; i < MenuDashboardUI.newsResponse.AppNews.NewsItems.Length; i++)
			{
				NewsItem newsItem = MenuDashboardUI.newsResponse.AppNews.NewsItems[i];
				if (newsItem != null)
				{
					ISleekBox sleekBox = Glazier.Get().CreateBox();
					sleekBox.SizeScale_X = 1f;
					sleekBox.UseManualLayout = false;
					sleekBox.UseChildAutoLayout = 1;
					sleekBox.ChildAutoLayoutPadding = 5f;
					ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
					sleekLabel.Text = newsItem.Title;
					sleekLabel.UseManualLayout = false;
					sleekLabel.TextAlignment = 0;
					sleekLabel.FontSize = 4;
					sleekBox.AddChild(sleekLabel);
					DateTime dateTime;
					dateTime..ctor(1970, 1, 1, 0, 0, 0, 0, 1);
					dateTime = dateTime.AddSeconds((double)newsItem.Date).ToLocalTime();
					ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
					sleekLabel2.Text = MenuDashboardUI.localization.format("News_Author", dateTime, newsItem.Author);
					sleekLabel2.UseManualLayout = false;
					sleekLabel2.TextAlignment = 0;
					sleekLabel2.FontSize = 0;
					sleekLabel2.TextColor = new SleekColor(3, 0.5f);
					sleekBox.AddChild(sleekLabel2);
					try
					{
						MenuDashboardUI.InsertSteamBbCode(sleekBox, newsItem.Contents, false);
					}
					catch (Exception e)
					{
						UnturnedLog.warn("Announcement description mis-formatted! Nelson messed up.");
						UnturnedLog.exception(e);
					}
					sleekBox.AddChild(new SleekWebLinkButton
					{
						Text = MenuDashboardUI.localization.format("News_Comments_Link"),
						Url = newsItem.URL,
						UseManualLayout = false,
						UseChildAutoLayout = 1,
						UseHeightLayoutOverride = true,
						ExpandChildren = true,
						SizeOffset_Y = 30f
					});
					MenuDashboardUI.mainScrollView.AddChild(sleekBox);
					if (i == 0)
					{
						long num;
						if (ConvenientSavedata.get().read("Newest_Announcement", out num))
						{
							MenuDashboardUI.hasNewAnnouncement = (num != newsItem.Date);
						}
						else
						{
							MenuDashboardUI.hasNewAnnouncement = true;
						}
						if (MenuDashboardUI.hasNewAnnouncement)
						{
							ConvenientSavedata.get().write("Newest_Announcement", newsItem.Date);
							sleekBox.SetAsFirstSibling();
							MenuDashboardUI.newAnnouncement = sleekBox;
						}
					}
				}
			}
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x0014BA50 File Offset: 0x00149C50
		private static void OnUpdateDetected(string versionString, bool isRollback)
		{
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.PositionOffset_X = 210f;
			sleekBox.PositionOffset_Y = MenuDashboardUI.mainHeaderOffset;
			sleekBox.SizeOffset_Y = 40f;
			sleekBox.SizeOffset_X = -210f;
			sleekBox.SizeScale_X = 1f;
			sleekBox.FontSize = 3;
			MenuDashboardUI.container.AddChild(sleekBox);
			string key = isRollback ? "RollbackAvailable" : "UpdateAvailable";
			string text = MenuDashboardUI.localization.format(key, versionString);
			RichTextUtil.replaceNewlineMarkup(ref text);
			sleekBox.Text = text;
			MenuDashboardUI.mainHeaderOffset += sleekBox.SizeOffset_Y + 10f;
			MenuDashboardUI.mainScrollView.PositionOffset_Y += sleekBox.SizeOffset_Y + 10f;
			MenuDashboardUI.mainScrollView.SizeOffset_Y -= sleekBox.SizeOffset_Y + 10f;
		}

		/// <summary>
		/// Read News.txt file from Cloud directory to preview on main menu.
		/// </summary>
		// Token: 0x06004065 RID: 16485 RVA: 0x0014BB30 File Offset: 0x00149D30
		private static bool readNewsPreview()
		{
			string text = Path.Combine(ReadWrite.PATH, "Cloud", "News.txt");
			if (!File.Exists(text))
			{
				return false;
			}
			string contents = File.ReadAllText(text);
			NewsItem newsItem = new NewsItem();
			newsItem.Author = "Preview";
			newsItem.Title = "Preview";
			newsItem.Contents = contents;
			newsItem.Date = DateTimeEx.ToUnixTimeSeconds(DateTime.UtcNow);
			MenuDashboardUI.newsResponse = new NewsResponse();
			MenuDashboardUI.newsResponse.AppNews = new AppNews();
			MenuDashboardUI.newsResponse.AppNews.NewsItems = new NewsItem[]
			{
				newsItem
			};
			MenuDashboardUI.receiveNewsResponse();
			return true;
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0014BBCE File Offset: 0x00149DCE
		internal static void receiveSteamNews(string data)
		{
			MenuDashboardUI.newsResponse = JsonConvert.DeserializeObject<NewsResponse>(data);
			MenuDashboardUI.receiveNewsResponse();
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x0014BBE0 File Offset: 0x00149DE0
		private static void spawnFeaturedWorkshopArticle()
		{
		}

		/// <summary>
		/// Helper for handlePopularItemResults.
		/// If player has not dismissed item at index then proceed with query and return true.
		/// </summary>
		// Token: 0x06004068 RID: 16488 RVA: 0x0014BBE4 File Offset: 0x00149DE4
		private static bool featurePopularItem(uint index)
		{
			SteamUGCDetails_t steamUGCDetails_t;
			if (!SteamUGC.GetQueryUGCResult(MenuDashboardUI.popularWorkshopHandle, index, out steamUGCDetails_t))
			{
				UnturnedLog.warn(string.Format("Unable to get popular workshop item details for index {0}", index));
				return false;
			}
			if (steamUGCDetails_t.m_eResult != EResult.k_EResultOK)
			{
				UnturnedLog.warn(string.Format("Error retrieving details for popular workshop file {0}: {1}", steamUGCDetails_t.m_nPublishedFileId, steamUGCDetails_t.m_eResult));
				return false;
			}
			if (steamUGCDetails_t.m_bBanned)
			{
				UnturnedLog.warn(string.Format("Ignoring popular workshop file {0} because it was banned", steamUGCDetails_t.m_nPublishedFileId));
				return false;
			}
			if (steamUGCDetails_t.m_eVisibility != ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic)
			{
				UnturnedLog.warn(string.Format("Ignoring popular workshop file {0} because visibility is {1}", steamUGCDetails_t.m_nPublishedFileId, steamUGCDetails_t.m_eVisibility));
				return false;
			}
			if (LocalNews.wasWorkshopItemDismissed(steamUGCDetails_t.m_nPublishedFileId.m_PublishedFileId))
			{
				return false;
			}
			MenuDashboardUI.queryFeaturedItem(steamUGCDetails_t.m_nPublishedFileId);
			return true;
		}

		/// <summary>
		/// Successfully queried popular workshop items.
		/// Tries to decide on an item that player has not dismissed.
		/// </summary>
		// Token: 0x06004069 RID: 16489 RVA: 0x0014BCBA File Offset: 0x00149EBA
		private static void handlePopularItemResults(SteamUGCQueryCompleted_t callback)
		{
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x0014BCBC File Offset: 0x00149EBC
		private static void onPopularQueryCompleted(SteamUGCQueryCompleted_t callback, bool io)
		{
			if (io)
			{
				UnturnedLog.warn("IO error while querying popular workshop items");
				return;
			}
			if (callback.m_eResult == EResult.k_EResultOK)
			{
				MenuDashboardUI.handlePopularItemResults(callback);
				return;
			}
			UnturnedLog.warn("Error while querying popular workshop items: " + callback.m_eResult.ToString());
		}

		/// <summary>
		/// Response about the item we decided to display.
		/// </summary>
		// Token: 0x0600406B RID: 16491 RVA: 0x0014BD08 File Offset: 0x00149F08
		private static void onFeaturedQueryCompleted(SteamUGCQueryCompleted_t callback, bool io)
		{
			if (io)
			{
				UnturnedLog.warn("IO error while querying featured workshop item");
				return;
			}
			if (callback.m_eResult == EResult.k_EResultOK)
			{
				UnturnedLog.info("Received workshop file details for news feed");
				try
				{
					MenuDashboardUI.spawnFeaturedWorkshopArticle();
					return;
				}
				catch (Exception e)
				{
					UnturnedLog.warn("Workshop news article spawn failed!");
					UnturnedLog.exception(e);
					return;
				}
			}
			UnturnedLog.warn("Error while querying featured workshop item: " + callback.m_eResult.ToString());
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x0014BD80 File Offset: 0x00149F80
		private static void onSteamUGCQueryCompleted(SteamUGCQueryCompleted_t callback, bool io)
		{
			if (callback.m_handle == MenuDashboardUI.popularWorkshopHandle)
			{
				MenuDashboardUI.onPopularQueryCompleted(callback, io);
				SteamUGC.ReleaseQueryUGCRequest(MenuDashboardUI.popularWorkshopHandle);
				MenuDashboardUI.popularWorkshopHandle = UGCQueryHandle_t.Invalid;
				return;
			}
			if (callback.m_handle == MenuDashboardUI.featuredWorkshopHandle)
			{
				MenuDashboardUI.onFeaturedQueryCompleted(callback, io);
				SteamUGC.ReleaseQueryUGCRequest(MenuDashboardUI.featuredWorkshopHandle);
				MenuDashboardUI.featuredWorkshopHandle = UGCQueryHandle_t.Invalid;
			}
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0014BDEC File Offset: 0x00149FEC
		protected static void queryFeaturedItem(PublishedFileId_t publishedFileID)
		{
			UnturnedLog.info("Requesting workshop file details for news feed ({0})", new object[]
			{
				publishedFileID
			});
			if (MenuDashboardUI.featuredWorkshopHandle != UGCQueryHandle_t.Invalid)
			{
				SteamUGC.ReleaseQueryUGCRequest(MenuDashboardUI.featuredWorkshopHandle);
				MenuDashboardUI.featuredWorkshopHandle = UGCQueryHandle_t.Invalid;
			}
			MenuDashboardUI.featuredWorkshopHandle = SteamUGC.CreateQueryUGCDetailsRequest(new PublishedFileId_t[]
			{
				publishedFileID
			}, 1U);
			SteamUGC.SetReturnLongDescription(MenuDashboardUI.featuredWorkshopHandle, true);
			SteamUGC.SetReturnChildren(MenuDashboardUI.featuredWorkshopHandle, true);
			SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(MenuDashboardUI.featuredWorkshopHandle);
			MenuDashboardUI.steamUGCQueryCompletedFeatured.Set(hAPICall, null);
		}

		/// <summary>
		/// Submit query for recently trending popular workshop items.
		/// </summary>
		// Token: 0x0600406E RID: 16494 RVA: 0x0014BE80 File Offset: 0x0014A080
		private static void queryPopularWorkshopItems()
		{
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x0014BE84 File Offset: 0x0014A084
		private void OnClickedBattlEyeButton(ISleekElement element)
		{
			try
			{
				ReadWrite.OpenFileBrowser(new DirectoryInfo(ReadWrite.PATH).CreateSubdirectory("BattlEye").FullName);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception opening BattlEye folder");
			}
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x0014BED0 File Offset: 0x0014A0D0
		private static void OnPricesReceived()
		{
			ItemStore itemStore = ItemStore.Get();
			int[] array;
			SleekItemStoreMainMenuButton.ELabelType elabelType;
			if (itemStore.HasNewListings)
			{
				array = itemStore.GetNewListingIndices();
				elabelType = SleekItemStoreMainMenuButton.ELabelType.New;
			}
			else if (ItemStore.Get().HasDiscountedListings)
			{
				array = itemStore.GetDiscountedListingIndices();
				elabelType = SleekItemStoreMainMenuButton.ELabelType.Sale;
			}
			else if (ItemStore.Get().HasFeaturedListings && Random.value < 0.5f)
			{
				array = itemStore.GetFeaturedListingIndices();
				elabelType = SleekItemStoreMainMenuButton.ELabelType.None;
			}
			else
			{
				array = null;
				elabelType = SleekItemStoreMainMenuButton.ELabelType.None;
			}
			ItemStore.Listing listing;
			if (array == null)
			{
				ItemStore.Listing[] listings = itemStore.GetListings();
				listing = listings[Random.Range(0, listings.Length)];
				Guid inventoryItemGuid = Provider.provider.economyService.getInventoryItemGuid(listing.itemdefid);
				if (inventoryItemGuid != default(Guid) && Assets.find<ItemKeyAsset>(inventoryItemGuid) != null)
				{
					return;
				}
			}
			else
			{
				int[] excludedListingIndices = itemStore.GetExcludedListingIndices();
				if (excludedListingIndices != null)
				{
					List<int> list = new List<int>(array);
					foreach (int num in excludedListingIndices)
					{
						list.Remove(num);
					}
					if (list.Count < 1)
					{
						return;
					}
					array = list.ToArray();
				}
				int num2 = array.RandomOrDefault<int>();
				listing = itemStore.GetListings()[num2];
				if (elabelType == SleekItemStoreMainMenuButton.ELabelType.New && ItemStoreSavedata.WasNewListingSeen(listing.itemdefid))
				{
					elabelType = SleekItemStoreMainMenuButton.ELabelType.None;
				}
			}
			SleekItemStoreMainMenuButton sleekItemStoreMainMenuButton = new SleekItemStoreMainMenuButton(listing, elabelType);
			sleekItemStoreMainMenuButton.PositionOffset_Y = 410f;
			sleekItemStoreMainMenuButton.SizeOffset_X = 200f;
			sleekItemStoreMainMenuButton.SizeOffset_Y = 50f;
			MenuDashboardUI.container.AddChild(sleekItemStoreMainMenuButton);
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x0014C039 File Offset: 0x0014A239
		public void OnDestroy()
		{
			ItemStore.Get().OnPricesReceived -= new Action(MenuDashboardUI.OnPricesReceived);
			this.playUI.OnDestroy();
			this.survivorsUI.OnDestroy();
			this.workshopUI.OnDestroy();
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x0014C074 File Offset: 0x0014A274
		public MenuDashboardUI()
		{
			if (MenuDashboardUI.icons != null)
			{
				MenuDashboardUI.icons.unload();
			}
			MenuDashboardUI.localization = Localization.read("/Menu/MenuDashboard.dat");
			TransportBase.OnGetMessage = new TransportBase.GetMessageCallback(MenuDashboardUI.localization.format);
			MenuDashboardUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/MenuDashboard/MenuDashboard.unity3d");
			MenuUI.copyNotificationButton.icon = MenuDashboardUI.icons.load<Texture2D>("Clipboard");
			MenuUI.copyNotificationButton.text = MenuDashboardUI.localization.format("Copy_Notification_Label");
			MenuUI.copyNotificationButton.tooltip = MenuDashboardUI.localization.format("Copy_Notification_Tooltip");
			if (SteamUser.BLoggedOn())
			{
				MenuDashboardUI.hasNewAnnouncement = false;
				MenuUI.instance.StartCoroutine(MenuUI.instance.CheckForUpdates(new Action<string, bool>(MenuDashboardUI.OnUpdateDetected)));
				if (MenuDashboardUI.steamUGCQueryCompletedPopular == null)
				{
					MenuDashboardUI.steamUGCQueryCompletedPopular = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(MenuDashboardUI.onSteamUGCQueryCompleted));
				}
				if (MenuDashboardUI.steamUGCQueryCompletedFeatured == null)
				{
					MenuDashboardUI.steamUGCQueryCompletedFeatured = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(MenuDashboardUI.onSteamUGCQueryCompleted));
				}
				if (MenuDashboardUI.popularWorkshopHandle != UGCQueryHandle_t.Invalid)
				{
					SteamUGC.ReleaseQueryUGCRequest(MenuDashboardUI.popularWorkshopHandle);
					MenuDashboardUI.popularWorkshopHandle = UGCQueryHandle_t.Invalid;
				}
			}
			MenuDashboardUI.container = new SleekFullscreenBox();
			MenuDashboardUI.container.PositionOffset_X = 10f;
			MenuDashboardUI.container.PositionOffset_Y = 10f;
			MenuDashboardUI.container.SizeOffset_X = -20f;
			MenuDashboardUI.container.SizeOffset_Y = -20f;
			MenuDashboardUI.container.SizeScale_X = 1f;
			MenuDashboardUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuDashboardUI.container);
			MenuDashboardUI.active = true;
			MenuDashboardUI.playButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Play"));
			MenuDashboardUI.playButton.PositionOffset_Y = 170f;
			MenuDashboardUI.playButton.SizeOffset_X = 200f;
			MenuDashboardUI.playButton.SizeOffset_Y = 50f;
			MenuDashboardUI.playButton.text = MenuDashboardUI.localization.format("PlayButtonText");
			MenuDashboardUI.playButton.tooltip = MenuDashboardUI.localization.format("PlayButtonTooltip");
			MenuDashboardUI.playButton.onClickedButton += new ClickedButton(MenuDashboardUI.onClickedPlayButton);
			MenuDashboardUI.playButton.fontSize = 3;
			MenuDashboardUI.playButton.iconColor = 2;
			MenuDashboardUI.container.AddChild(MenuDashboardUI.playButton);
			MenuDashboardUI.survivorsButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Survivors"));
			MenuDashboardUI.survivorsButton.PositionOffset_Y = 230f;
			MenuDashboardUI.survivorsButton.SizeOffset_X = 200f;
			MenuDashboardUI.survivorsButton.SizeOffset_Y = 50f;
			MenuDashboardUI.survivorsButton.text = MenuDashboardUI.localization.format("SurvivorsButtonText");
			MenuDashboardUI.survivorsButton.tooltip = MenuDashboardUI.localization.format("SurvivorsButtonTooltip");
			MenuDashboardUI.survivorsButton.onClickedButton += new ClickedButton(MenuDashboardUI.onClickedSurvivorsButton);
			MenuDashboardUI.survivorsButton.fontSize = 3;
			MenuDashboardUI.survivorsButton.iconColor = 2;
			MenuDashboardUI.container.AddChild(MenuDashboardUI.survivorsButton);
			MenuDashboardUI.configurationButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Configuration"));
			MenuDashboardUI.configurationButton.PositionOffset_Y = 290f;
			MenuDashboardUI.configurationButton.SizeOffset_X = 200f;
			MenuDashboardUI.configurationButton.SizeOffset_Y = 50f;
			MenuDashboardUI.configurationButton.text = MenuDashboardUI.localization.format("ConfigurationButtonText");
			MenuDashboardUI.configurationButton.tooltip = MenuDashboardUI.localization.format("ConfigurationButtonTooltip");
			MenuDashboardUI.configurationButton.onClickedButton += new ClickedButton(MenuDashboardUI.onClickedConfigurationButton);
			MenuDashboardUI.configurationButton.fontSize = 3;
			MenuDashboardUI.configurationButton.iconColor = 2;
			MenuDashboardUI.container.AddChild(MenuDashboardUI.configurationButton);
			MenuDashboardUI.workshopButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Workshop"));
			MenuDashboardUI.workshopButton.PositionOffset_Y = 350f;
			MenuDashboardUI.workshopButton.SizeOffset_X = 200f;
			MenuDashboardUI.workshopButton.SizeOffset_Y = 50f;
			MenuDashboardUI.workshopButton.text = MenuDashboardUI.localization.format("WorkshopButtonText");
			MenuDashboardUI.workshopButton.tooltip = MenuDashboardUI.localization.format("WorkshopButtonTooltip");
			MenuDashboardUI.workshopButton.onClickedButton += new ClickedButton(MenuDashboardUI.onClickedWorkshopButton);
			MenuDashboardUI.workshopButton.fontSize = 3;
			MenuDashboardUI.workshopButton.iconColor = 2;
			MenuDashboardUI.container.AddChild(MenuDashboardUI.workshopButton);
			MenuDashboardUI.exitButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuDashboardUI.exitButton.PositionOffset_Y = -50f;
			MenuDashboardUI.exitButton.PositionScale_Y = 1f;
			MenuDashboardUI.exitButton.SizeOffset_X = 200f;
			MenuDashboardUI.exitButton.SizeOffset_Y = 50f;
			MenuDashboardUI.exitButton.text = MenuDashboardUI.localization.format("ExitButtonText");
			MenuDashboardUI.exitButton.tooltip = MenuDashboardUI.localization.format("ExitButtonTooltip");
			MenuDashboardUI.exitButton.onClickedButton += new ClickedButton(MenuDashboardUI.onClickedExitButton);
			MenuDashboardUI.exitButton.fontSize = 3;
			MenuDashboardUI.exitButton.iconColor = 2;
			MenuDashboardUI.container.AddChild(MenuDashboardUI.exitButton);
			MenuDashboardUI.mainScrollView = Glazier.Get().CreateScrollView();
			MenuDashboardUI.mainScrollView.PositionOffset_X = 210f;
			MenuDashboardUI.mainScrollView.PositionOffset_Y = 170f;
			MenuDashboardUI.mainScrollView.SizeScale_X = 1f;
			MenuDashboardUI.mainScrollView.SizeScale_Y = 1f;
			MenuDashboardUI.mainScrollView.SizeOffset_X = -210f;
			MenuDashboardUI.mainScrollView.SizeOffset_Y = -170f;
			MenuDashboardUI.mainScrollView.ScaleContentToWidth = true;
			MenuDashboardUI.container.AddChild(MenuDashboardUI.mainScrollView);
			if (!Glazier.Get().SupportsAutomaticLayout)
			{
				ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
				sleekLabel.Text = "Featured workshop file and news feed are no longer supported when using the -Glazier=IMGUI launch option.";
				sleekLabel.FontSize = 4;
				sleekLabel.SizeScale_X = 1f;
				sleekLabel.SizeOffset_Y = 100f;
				MenuDashboardUI.mainScrollView.ContentSizeOffset = new Vector2(0f, 100f);
				MenuDashboardUI.mainScrollView.AddChild(sleekLabel);
			}
			else
			{
				MenuDashboardUI.mainScrollView.ContentUseManualLayout = false;
			}
			if (!Provider.isPro)
			{
				MenuDashboardUI.proButton = Glazier.Get().CreateButton();
				MenuDashboardUI.proButton.PositionOffset_X = 210f;
				MenuDashboardUI.proButton.PositionOffset_Y = -100f;
				MenuDashboardUI.proButton.PositionScale_Y = 1f;
				MenuDashboardUI.proButton.SizeOffset_Y = 100f;
				MenuDashboardUI.proButton.SizeOffset_X = -210f;
				MenuDashboardUI.proButton.SizeScale_X = 1f;
				MenuDashboardUI.proButton.TooltipText = MenuDashboardUI.localization.format("Pro_Button_Tooltip");
				MenuDashboardUI.proButton.BackgroundColor = SleekColor.BackgroundIfLight(Palette.PRO);
				MenuDashboardUI.proButton.TextColor = Palette.PRO;
				MenuDashboardUI.proButton.OnClicked += new ClickedButton(MenuDashboardUI.onClickedProButton);
				MenuDashboardUI.container.AddChild(MenuDashboardUI.proButton);
				MenuDashboardUI.proLabel = Glazier.Get().CreateLabel();
				MenuDashboardUI.proLabel.SizeScale_X = 1f;
				MenuDashboardUI.proLabel.SizeOffset_Y = 50f;
				MenuDashboardUI.proLabel.Text = MenuDashboardUI.localization.format("Pro_Title");
				MenuDashboardUI.proLabel.TextColor = Palette.PRO;
				MenuDashboardUI.proLabel.FontSize = 4;
				MenuDashboardUI.proButton.AddChild(MenuDashboardUI.proLabel);
				MenuDashboardUI.featureLabel = Glazier.Get().CreateLabel();
				MenuDashboardUI.featureLabel.PositionOffset_Y = 50f;
				MenuDashboardUI.featureLabel.SizeOffset_Y = -50f;
				MenuDashboardUI.featureLabel.SizeScale_X = 1f;
				MenuDashboardUI.featureLabel.SizeScale_Y = 1f;
				MenuDashboardUI.featureLabel.Text = MenuDashboardUI.localization.format("Pro_Button");
				MenuDashboardUI.featureLabel.TextColor = Palette.PRO;
				MenuDashboardUI.proButton.AddChild(MenuDashboardUI.featureLabel);
				MenuDashboardUI.mainScrollView.SizeOffset_Y -= 110f;
			}
			MenuDashboardUI.mainHeaderOffset = 170f;
			MenuDashboardUI.alertBox = null;
			string text;
			if (SteamApps.GetCurrentBetaName(out text, 64) && string.Equals(text, "preview", 3))
			{
				this.CreatePreviewBranchChangelogButton();
			}
			if (!Dedicator.hasBattlEye)
			{
				MenuDashboardUI.battlEyeButton = Glazier.Get().CreateButton();
				MenuDashboardUI.battlEyeButton.PositionOffset_X = 210f;
				MenuDashboardUI.battlEyeButton.PositionOffset_Y = MenuDashboardUI.mainHeaderOffset;
				MenuDashboardUI.battlEyeButton.SizeOffset_Y = 60f;
				MenuDashboardUI.battlEyeButton.SizeOffset_X = -210f;
				MenuDashboardUI.battlEyeButton.SizeScale_X = 1f;
				MenuDashboardUI.battlEyeButton.OnClicked += new ClickedButton(this.OnClickedBattlEyeButton);
				MenuDashboardUI.container.AddChild(MenuDashboardUI.battlEyeButton);
				MenuDashboardUI.battlEyeIcon = Glazier.Get().CreateImage();
				MenuDashboardUI.battlEyeIcon.PositionOffset_X = 10f;
				MenuDashboardUI.battlEyeIcon.PositionOffset_Y = 10f;
				MenuDashboardUI.battlEyeIcon.SizeOffset_X = 40f;
				MenuDashboardUI.battlEyeIcon.SizeOffset_Y = 40f;
				MenuDashboardUI.battlEyeIcon.Texture = MenuDashboardUI.icons.load<Texture2D>("BattlEye");
				MenuDashboardUI.battlEyeButton.AddChild(MenuDashboardUI.battlEyeIcon);
				MenuDashboardUI.battlEyeHeaderLabel = Glazier.Get().CreateLabel();
				MenuDashboardUI.battlEyeHeaderLabel.PositionOffset_X = 60f;
				MenuDashboardUI.battlEyeHeaderLabel.SizeScale_X = 1f;
				MenuDashboardUI.battlEyeHeaderLabel.SizeOffset_X = -60f;
				MenuDashboardUI.battlEyeHeaderLabel.SizeOffset_Y = 30f;
				MenuDashboardUI.battlEyeHeaderLabel.Text = MenuDashboardUI.localization.format("BattlEye_Header");
				MenuDashboardUI.battlEyeHeaderLabel.FontSize = 3;
				MenuDashboardUI.battlEyeButton.AddChild(MenuDashboardUI.battlEyeHeaderLabel);
				MenuDashboardUI.battlEyeBodyLabel = Glazier.Get().CreateLabel();
				MenuDashboardUI.battlEyeBodyLabel.PositionOffset_X = 60f;
				MenuDashboardUI.battlEyeBodyLabel.PositionOffset_Y = 20f;
				MenuDashboardUI.battlEyeBodyLabel.SizeOffset_X = -60f;
				MenuDashboardUI.battlEyeBodyLabel.SizeOffset_Y = -20f;
				MenuDashboardUI.battlEyeBodyLabel.SizeScale_X = 1f;
				MenuDashboardUI.battlEyeBodyLabel.SizeScale_Y = 1f;
				MenuDashboardUI.battlEyeBodyLabel.Text = MenuDashboardUI.localization.format("BattlEye_Body");
				MenuDashboardUI.battlEyeButton.AddChild(MenuDashboardUI.battlEyeBodyLabel);
				MenuDashboardUI.mainHeaderOffset += MenuDashboardUI.battlEyeButton.SizeOffset_Y + 10f;
				MenuDashboardUI.mainScrollView.PositionOffset_Y += MenuDashboardUI.battlEyeButton.SizeOffset_Y + 10f;
				MenuDashboardUI.mainScrollView.SizeOffset_Y -= MenuDashboardUI.battlEyeButton.SizeOffset_Y + 10f;
			}
			ItemStore.Get().OnPricesReceived += new Action(MenuDashboardUI.OnPricesReceived);
			this.pauseUI = new MenuPauseUI();
			this.creditsUI = new MenuCreditsUI();
			this.titleUI = new MenuTitleUI();
			this.playUI = new MenuPlayUI();
			this.survivorsUI = new MenuSurvivorsUI();
			this.configUI = new MenuConfigurationUI();
			this.workshopUI = new MenuWorkshopUI();
			if (Provider.connectionFailureInfo != ESteamConnectionFailureInfo.NONE)
			{
				ESteamConnectionFailureInfo esteamConnectionFailureInfo = Provider.connectionFailureInfo;
				string connectionFailureReason = Provider.connectionFailureReason;
				uint connectionFailureDuration = Provider.connectionFailureDuration;
				int serverInvalidItemsCount = Provider.provider.workshopService.serverInvalidItemsCount;
				Provider.resetConnectionFailure();
				Provider.provider.workshopService.resetServerInvalidItems();
				if (serverInvalidItemsCount > 0)
				{
					bool flag = esteamConnectionFailureInfo == ESteamConnectionFailureInfo.MAP || esteamConnectionFailureInfo == ESteamConnectionFailureInfo.HASH_LEVEL || esteamConnectionFailureInfo - ESteamConnectionFailureInfo.BARRICADE <= 2;
					if (flag)
					{
						UnturnedLog.info("Connection failure {0} is asset related and therefore probably caused by the {1} download-restricted workshop item(s)", new object[]
						{
							esteamConnectionFailureInfo,
							serverInvalidItemsCount
						});
						esteamConnectionFailureInfo = ESteamConnectionFailureInfo.WORKSHOP_DOWNLOAD_RESTRICTION;
					}
				}
				string text2;
				switch (esteamConnectionFailureInfo)
				{
				case ESteamConnectionFailureInfo.SHUTDOWN:
					text2 = (string.IsNullOrEmpty(connectionFailureReason) ? MenuDashboardUI.localization.format("Shutdown") : MenuDashboardUI.localization.format("Shutdown_Reason", connectionFailureReason));
					break;
				case ESteamConnectionFailureInfo.MAP:
					text2 = MenuDashboardUI.localization.format("Map");
					break;
				case ESteamConnectionFailureInfo.BANNED:
					text2 = MenuDashboardUI.localization.format("Banned", connectionFailureDuration, connectionFailureReason);
					break;
				case ESteamConnectionFailureInfo.KICKED:
					text2 = MenuDashboardUI.localization.format("Kicked", connectionFailureReason);
					break;
				case ESteamConnectionFailureInfo.WHITELISTED:
					text2 = MenuDashboardUI.localization.format("Whitelisted");
					break;
				case ESteamConnectionFailureInfo.PASSWORD:
					text2 = MenuDashboardUI.localization.format("Password");
					break;
				case ESteamConnectionFailureInfo.FULL:
					text2 = MenuDashboardUI.localization.format("Full");
					break;
				case ESteamConnectionFailureInfo.HASH_LEVEL:
					text2 = MenuDashboardUI.localization.format("Hash_Level");
					break;
				case ESteamConnectionFailureInfo.HASH_ASSEMBLY:
					text2 = MenuDashboardUI.localization.format("Hash_Assembly");
					break;
				case ESteamConnectionFailureInfo.VERSION:
					text2 = MenuDashboardUI.localization.format("Version", connectionFailureReason, Provider.APP_VERSION);
					break;
				case ESteamConnectionFailureInfo.PRO_SERVER:
					text2 = MenuDashboardUI.localization.format("Pro_Server");
					break;
				case ESteamConnectionFailureInfo.PRO_CHARACTER:
					text2 = MenuDashboardUI.localization.format("Pro_Character");
					break;
				case ESteamConnectionFailureInfo.PRO_DESYNC:
					text2 = MenuDashboardUI.localization.format("Pro_Desync");
					break;
				case ESteamConnectionFailureInfo.PRO_APPEARANCE:
					text2 = MenuDashboardUI.localization.format("Pro_Appearance");
					break;
				case ESteamConnectionFailureInfo.AUTH_VERIFICATION:
					text2 = MenuDashboardUI.localization.format("Auth_Verification");
					break;
				case ESteamConnectionFailureInfo.AUTH_NO_STEAM:
					text2 = MenuDashboardUI.localization.format("Auth_No_Steam");
					break;
				case ESteamConnectionFailureInfo.AUTH_LICENSE_EXPIRED:
					text2 = MenuDashboardUI.localization.format("Auth_License_Expired");
					break;
				case ESteamConnectionFailureInfo.AUTH_VAC_BAN:
					text2 = MenuDashboardUI.localization.format("Auth_VAC_Ban");
					break;
				case ESteamConnectionFailureInfo.AUTH_ELSEWHERE:
					text2 = MenuDashboardUI.localization.format("Auth_Elsewhere");
					break;
				case ESteamConnectionFailureInfo.AUTH_TIMED_OUT:
					text2 = MenuDashboardUI.localization.format("Auth_Timed_Out");
					break;
				case ESteamConnectionFailureInfo.AUTH_USED:
					text2 = MenuDashboardUI.localization.format("Auth_Used");
					break;
				case ESteamConnectionFailureInfo.AUTH_NO_USER:
					text2 = MenuDashboardUI.localization.format("Auth_No_User");
					break;
				case ESteamConnectionFailureInfo.AUTH_PUB_BAN:
					text2 = MenuDashboardUI.localization.format("Auth_Pub_Ban");
					break;
				case ESteamConnectionFailureInfo.AUTH_ECON_SERIALIZE:
					text2 = MenuDashboardUI.localization.format("Auth_Econ_Serialize");
					break;
				case ESteamConnectionFailureInfo.AUTH_ECON_DESERIALIZE:
					text2 = MenuDashboardUI.localization.format("Auth_Econ_Deserialize");
					break;
				case ESteamConnectionFailureInfo.AUTH_ECON_VERIFY:
					text2 = MenuDashboardUI.localization.format("Auth_Econ_Verify");
					break;
				case ESteamConnectionFailureInfo.AUTH_EMPTY:
					text2 = MenuDashboardUI.localization.format("Auth_Empty");
					break;
				case ESteamConnectionFailureInfo.ALREADY_CONNECTED:
					text2 = MenuDashboardUI.localization.format("Already_Connected");
					break;
				case ESteamConnectionFailureInfo.ALREADY_PENDING:
					text2 = MenuDashboardUI.localization.format("Already_Pending");
					break;
				case ESteamConnectionFailureInfo.LATE_PENDING:
					text2 = MenuDashboardUI.localization.format("Late_Pending");
					break;
				case ESteamConnectionFailureInfo.NOT_PENDING:
					text2 = MenuDashboardUI.localization.format("Not_Pending");
					break;
				case ESteamConnectionFailureInfo.NAME_PLAYER_SHORT:
					text2 = MenuDashboardUI.localization.format("Name_Player_Short");
					break;
				case ESteamConnectionFailureInfo.NAME_PLAYER_LONG:
					text2 = MenuDashboardUI.localization.format("Name_Player_Long");
					break;
				case ESteamConnectionFailureInfo.NAME_PLAYER_INVALID:
					text2 = MenuDashboardUI.localization.format("Name_Player_Invalid");
					break;
				case ESteamConnectionFailureInfo.NAME_PLAYER_NUMBER:
					text2 = MenuDashboardUI.localization.format("Name_Player_Number");
					break;
				case ESteamConnectionFailureInfo.NAME_CHARACTER_SHORT:
					text2 = MenuDashboardUI.localization.format("Name_Character_Short");
					break;
				case ESteamConnectionFailureInfo.NAME_CHARACTER_LONG:
					text2 = MenuDashboardUI.localization.format("Name_Character_Long");
					break;
				case ESteamConnectionFailureInfo.NAME_CHARACTER_INVALID:
					text2 = MenuDashboardUI.localization.format("Name_Character_Invalid");
					break;
				case ESteamConnectionFailureInfo.NAME_CHARACTER_NUMBER:
					text2 = MenuDashboardUI.localization.format("Name_Character_Number");
					break;
				case ESteamConnectionFailureInfo.TIMED_OUT:
					text2 = MenuDashboardUI.localization.format("Timed_Out");
					break;
				case ESteamConnectionFailureInfo.PING:
					text2 = connectionFailureReason;
					break;
				case ESteamConnectionFailureInfo.PLUGIN:
					text2 = (string.IsNullOrEmpty(connectionFailureReason) ? MenuDashboardUI.localization.format("Plugin") : MenuDashboardUI.localization.format("Plugin_Reason", connectionFailureReason));
					break;
				case ESteamConnectionFailureInfo.BARRICADE:
					text2 = MenuDashboardUI.localization.format("Barricade", connectionFailureReason);
					break;
				case ESteamConnectionFailureInfo.STRUCTURE:
					text2 = MenuDashboardUI.localization.format("Structure", connectionFailureReason);
					break;
				case ESteamConnectionFailureInfo.VEHICLE:
					text2 = MenuDashboardUI.localization.format("Vehicle", connectionFailureReason);
					break;
				case ESteamConnectionFailureInfo.CLIENT_MODULE_DESYNC:
					text2 = MenuDashboardUI.localization.format("Client_Module_Desync");
					break;
				case ESteamConnectionFailureInfo.SERVER_MODULE_DESYNC:
					text2 = MenuDashboardUI.localization.format("Server_Module_Desync");
					break;
				case ESteamConnectionFailureInfo.BATTLEYE_BROKEN:
					text2 = MenuDashboardUI.localization.format("BattlEye_Broken");
					break;
				case ESteamConnectionFailureInfo.BATTLEYE_UPDATE:
					text2 = MenuDashboardUI.localization.format("BattlEye_Update");
					break;
				case ESteamConnectionFailureInfo.BATTLEYE_UNKNOWN:
					text2 = MenuDashboardUI.localization.format("BattlEye_Unknown");
					break;
				case ESteamConnectionFailureInfo.LEVEL_VERSION:
					text2 = connectionFailureReason;
					break;
				case ESteamConnectionFailureInfo.ECON_HASH:
					text2 = MenuDashboardUI.localization.format("Econ_Hash");
					break;
				case ESteamConnectionFailureInfo.HASH_MASTER_BUNDLE:
					text2 = MenuDashboardUI.localization.format("Master_Bundle_Hash", connectionFailureReason);
					break;
				case ESteamConnectionFailureInfo.REJECT_UNKNOWN:
					text2 = MenuDashboardUI.localization.format("Reject_Unknown", connectionFailureReason);
					break;
				case ESteamConnectionFailureInfo.WORKSHOP_DOWNLOAD_RESTRICTION:
					text2 = MenuDashboardUI.localization.format("Workshop_Download_Restriction", serverInvalidItemsCount);
					break;
				case ESteamConnectionFailureInfo.WORKSHOP_ADVERTISEMENT_MISMATCH:
					text2 = MenuDashboardUI.localization.format("Workshop_Advertisement_Mismatch");
					break;
				case ESteamConnectionFailureInfo.CUSTOM:
					text2 = connectionFailureReason;
					break;
				case ESteamConnectionFailureInfo.LATE_PENDING_STEAM_AUTH:
					text2 = MenuDashboardUI.localization.format("Late_Pending_Steam_Auth");
					break;
				case ESteamConnectionFailureInfo.LATE_PENDING_STEAM_ECON:
					text2 = MenuDashboardUI.localization.format("Late_Pending_Steam_Econ");
					break;
				case ESteamConnectionFailureInfo.LATE_PENDING_STEAM_GROUPS:
					text2 = MenuDashboardUI.localization.format("Late_Pending_Steam_Groups");
					break;
				case ESteamConnectionFailureInfo.NAME_PRIVATE_LONG:
					text2 = MenuDashboardUI.localization.format("Name_Private_Long");
					break;
				case ESteamConnectionFailureInfo.NAME_PRIVATE_INVALID:
					text2 = MenuDashboardUI.localization.format("Name_Private_Invalid");
					break;
				case ESteamConnectionFailureInfo.NAME_PRIVATE_NUMBER:
					text2 = MenuDashboardUI.localization.format("Name_Private_Number");
					break;
				case ESteamConnectionFailureInfo.TIMED_OUT_LOGIN:
					text2 = MenuDashboardUI.localization.format("Timed_Out_Login");
					break;
				case ESteamConnectionFailureInfo.HASH_RESOURCES:
					text2 = MenuDashboardUI.localization.format("Hash_Resources");
					break;
				case ESteamConnectionFailureInfo.AUTH_NETWORK_IDENTITY_FAILURE:
					text2 = MenuDashboardUI.localization.format("Auth_Network_Identity_Failure");
					break;
				case ESteamConnectionFailureInfo.SERVER_MAP_ADVERTISEMENT_MISMATCH:
					text2 = MenuDashboardUI.localization.format("Server_Map_Advertisement_Mismatch");
					break;
				case ESteamConnectionFailureInfo.SERVER_VAC_ADVERTISEMENT_MISMATCH:
					text2 = MenuDashboardUI.localization.format("Server_VAC_Advertisement_Mismatch");
					break;
				case ESteamConnectionFailureInfo.SERVER_BATTLEYE_ADVERTISEMENT_MISMATCH:
					text2 = MenuDashboardUI.localization.format("Server_BattlEye_Advertisement_Mismatch");
					break;
				case ESteamConnectionFailureInfo.SERVER_MAXPLAYERS_ADVERTISEMENT_MISMATCH:
					text2 = MenuDashboardUI.localization.format("Server_MaxPlayers_Advertisement_Mismatch");
					break;
				case ESteamConnectionFailureInfo.SERVER_CAMERAMODE_ADVERTISEMENT_MISMATCH:
					text2 = MenuDashboardUI.localization.format("Server_CameraMode_Advertisement_Mismatch");
					break;
				case ESteamConnectionFailureInfo.SERVER_PVP_ADVERTISEMENT_MISMATCH:
					text2 = MenuDashboardUI.localization.format("Server_PvP_Advertisement_Mismatch");
					break;
				case ESteamConnectionFailureInfo.SKIN_COLOR_WITHIN_THRESHOLD_OF_TERRAIN_COLOR:
					text2 = MenuDashboardUI.localization.format("SkinColorWithinThresholdOfTerrainColor");
					break;
				default:
					text2 = MenuDashboardUI.localization.format("Failure_Unknown", esteamConnectionFailureInfo, connectionFailureReason);
					break;
				}
				if (string.IsNullOrEmpty(text2))
				{
					text2 = string.Format("Error: {0} Reason: {1}", esteamConnectionFailureInfo, connectionFailureReason);
				}
				MenuUI.alert(text2);
				UnturnedLog.info(text2);
			}
			if (SteamUser.BLoggedOn() && Glazier.Get().SupportsAutomaticLayout && !MenuDashboardUI.readNewsPreview())
			{
				MenuUI.instance.StartCoroutine(MenuUI.instance.requestSteamNews());
			}
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x0014D424 File Offset: 0x0014B624
		private void OnClickedPreviewBranchChangelog(ISleekElement button)
		{
			Provider.provider.browserService.open("https://support.smartlydressedgames.com/hc/en-us/articles/12462494977172");
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0014D43C File Offset: 0x0014B63C
		private void CreatePreviewBranchChangelogButton()
		{
			ISleekButton sleekButton = Glazier.Get().CreateButton();
			sleekButton.PositionOffset_X = 210f;
			sleekButton.PositionOffset_Y = MenuDashboardUI.mainHeaderOffset;
			sleekButton.SizeOffset_Y = 60f;
			sleekButton.SizeOffset_X = -210f;
			sleekButton.SizeScale_X = 1f;
			sleekButton.Text = "Click here to open preview branch changelog.";
			sleekButton.OnClicked += new ClickedButton(this.OnClickedPreviewBranchChangelog);
			MenuDashboardUI.container.AddChild(sleekButton);
			MenuDashboardUI.mainHeaderOffset += sleekButton.SizeOffset_Y + 10f;
			MenuDashboardUI.mainScrollView.PositionOffset_Y += sleekButton.SizeOffset_Y + 10f;
			MenuDashboardUI.mainScrollView.SizeOffset_Y -= sleekButton.SizeOffset_Y + 10f;
		}

		// Token: 0x04002932 RID: 10546
		public static Local localization;

		// Token: 0x04002933 RID: 10547
		public static Bundle icons;

		// Token: 0x04002934 RID: 10548
		private static SleekFullscreenBox container;

		// Token: 0x04002935 RID: 10549
		public static bool active;

		// Token: 0x04002936 RID: 10550
		private static SleekButtonIcon playButton;

		// Token: 0x04002937 RID: 10551
		private static SleekButtonIcon survivorsButton;

		// Token: 0x04002938 RID: 10552
		private static SleekButtonIcon configurationButton;

		// Token: 0x04002939 RID: 10553
		private static SleekButtonIcon workshopButton;

		// Token: 0x0400293A RID: 10554
		private static SleekButtonIcon exitButton;

		// Token: 0x0400293B RID: 10555
		private static ISleekScrollView mainScrollView;

		// Token: 0x0400293C RID: 10556
		private static ISleekButton proButton;

		// Token: 0x0400293D RID: 10557
		private static ISleekLabel proLabel;

		// Token: 0x0400293E RID: 10558
		private static ISleekLabel featureLabel;

		// Token: 0x0400293F RID: 10559
		private static ISleekButton battlEyeButton;

		// Token: 0x04002940 RID: 10560
		private static ISleekImage battlEyeIcon;

		// Token: 0x04002941 RID: 10561
		private static ISleekLabel battlEyeHeaderLabel;

		// Token: 0x04002942 RID: 10562
		private static ISleekLabel battlEyeBodyLabel;

		// Token: 0x04002943 RID: 10563
		private static ISleekButton alertBox;

		// Token: 0x04002944 RID: 10564
		private static ISleekImage alertImage;

		// Token: 0x04002945 RID: 10565
		private static SleekWebImage alertWebImage;

		// Token: 0x04002946 RID: 10566
		private static ISleekLabel alertHeaderLabel;

		// Token: 0x04002947 RID: 10567
		private static ISleekLabel alertLinkLabel;

		// Token: 0x04002948 RID: 10568
		private static ISleekLabel dismissAlertLabel;

		// Token: 0x04002949 RID: 10569
		private static ISleekLabel alertBodyLabel;

		// Token: 0x0400294A RID: 10570
		private static float mainHeaderOffset;

		// Token: 0x0400294B RID: 10571
		private static NewsResponse newsResponse;

		/// <summary>
		/// Has a new announcement been posted by the developer?
		/// If so, it is given priority over the featured workshop item.
		/// </summary>
		// Token: 0x0400294C RID: 10572
		private static bool hasNewAnnouncement;

		// Token: 0x0400294D RID: 10573
		private static ISleekElement newAnnouncement;

		// Token: 0x0400294E RID: 10574
		private static UGCQueryHandle_t popularWorkshopHandle = UGCQueryHandle_t.Invalid;

		// Token: 0x0400294F RID: 10575
		private static UGCQueryHandle_t featuredWorkshopHandle = UGCQueryHandle_t.Invalid;

		/// <summary>
		/// Nelson 2024-04-23: A concerned player raised the issue that mature content could potentially be returned in
		/// popular item results. Steam excludes certain mature content by default, but just in case, we check for these
		/// words and hide if contained in title.
		/// </summary>
		// Token: 0x04002950 RID: 10576
		private static string[] featuredWorkshopTitleBannedWords = new string[]
		{
			"drug",
			"alcohol",
			"cigarette",
			"heroin",
			"cocaine"
		};

		// Token: 0x04002951 RID: 10577
		private static CallResult<SteamUGCQueryCompleted_t> steamUGCQueryCompletedPopular;

		// Token: 0x04002952 RID: 10578
		private static CallResult<SteamUGCQueryCompleted_t> steamUGCQueryCompletedFeatured;

		// Token: 0x04002953 RID: 10579
		private MenuPauseUI pauseUI;

		// Token: 0x04002954 RID: 10580
		private MenuCreditsUI creditsUI;

		// Token: 0x04002955 RID: 10581
		private MenuTitleUI titleUI;

		// Token: 0x04002956 RID: 10582
		private MenuPlayUI playUI;

		// Token: 0x04002957 RID: 10583
		private MenuSurvivorsUI survivorsUI;

		// Token: 0x04002958 RID: 10584
		private MenuConfigurationUI configUI;

		// Token: 0x04002959 RID: 10585
		private MenuWorkshopUI workshopUI;

		// Token: 0x0400295A RID: 10586
		private const string STEAM_CLAN_IMAGE = "https://clan.cloudflare.steamstatic.com/images/";
	}
}
