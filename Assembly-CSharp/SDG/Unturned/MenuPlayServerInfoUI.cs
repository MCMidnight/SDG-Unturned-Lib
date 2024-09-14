using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using SDG.Provider;
using Steamworks;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x020007A1 RID: 1953
	public class MenuPlayServerInfoUI
	{
		// Token: 0x060040DC RID: 16604 RVA: 0x00151FCC File Offset: 0x001501CC
		private static void onUGCQueryCompleted(SteamUGCQueryCompleted_t callback, bool io)
		{
			if (callback.m_eResult != EResult.k_EResultOK || io)
			{
				return;
			}
			for (uint num = 0U; num < callback.m_unNumResultsReturned; num += 1U)
			{
				CachedUGCDetails cachedUGCDetails;
				if (TempSteamworksWorkshop.cacheDetails(callback.m_handle, num, out cachedUGCDetails))
				{
					string title = cachedUGCDetails.GetTitle();
					ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
					sleekLabel.PositionOffset_X = 5f;
					sleekLabel.PositionOffset_Y = (float)(num * 20U);
					sleekLabel.SizeOffset_Y = 30f;
					sleekLabel.SizeScale_X = 1f;
					sleekLabel.TextAlignment = 3;
					sleekLabel.Text = title;
					sleekLabel.TextColor = (cachedUGCDetails.isBannedOrPrivate ? 6 : 3);
					MenuPlayServerInfoUI.ugcBox.AddChild(sleekLabel);
					MenuPlayServerInfoUI.ServerInfoViewWorkshopButton serverInfoViewWorkshopButton = new MenuPlayServerInfoUI.ServerInfoViewWorkshopButton(cachedUGCDetails.fileId, title);
					serverInfoViewWorkshopButton.PositionOffset_X = -45f;
					serverInfoViewWorkshopButton.PositionOffset_Y = sleekLabel.PositionOffset_Y + 5f;
					serverInfoViewWorkshopButton.PositionScale_X = 1f;
					MenuPlayServerInfoUI.ugcBox.AddChild(serverInfoViewWorkshopButton);
					SleekWorkshopSubscriptionButton sleekWorkshopSubscriptionButton = new SleekWorkshopSubscriptionButton();
					sleekWorkshopSubscriptionButton.PositionOffset_X = -25f;
					sleekWorkshopSubscriptionButton.PositionOffset_Y = sleekLabel.PositionOffset_Y + 5f;
					sleekWorkshopSubscriptionButton.PositionScale_X = 1f;
					sleekWorkshopSubscriptionButton.SizeOffset_X = 20f;
					sleekWorkshopSubscriptionButton.SizeOffset_Y = 20f;
					sleekWorkshopSubscriptionButton.subscribeText = MenuPlayServerInfoUI.localization.format("Subscribe");
					sleekWorkshopSubscriptionButton.unsubscribeText = MenuPlayServerInfoUI.localization.format("Unsubscribe");
					sleekWorkshopSubscriptionButton.subscribeTooltip = MenuPlayServerInfoUI.localization.format("Subscribe_Tooltip", title);
					sleekWorkshopSubscriptionButton.unsubscribeTooltip = MenuPlayServerInfoUI.localization.format("Unsubscribe_Tooltip", title);
					sleekWorkshopSubscriptionButton.fileId = cachedUGCDetails.fileId;
					sleekWorkshopSubscriptionButton.synchronizeText();
					MenuPlayServerInfoUI.ugcBox.AddChild(sleekWorkshopSubscriptionButton);
				}
			}
			MenuPlayServerInfoUI.ugcBox.SizeOffset_Y = (float)(callback.m_unNumResultsReturned * 20U + 10U);
			MenuPlayServerInfoUI.ugcTitle.IsVisible = true;
			MenuPlayServerInfoUI.ugcBox.IsVisible = true;
			MenuPlayServerInfoUI.updateDetails();
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x060040DD RID: 16605 RVA: 0x001521BE File Offset: 0x001503BE
		// (set) Token: 0x060040DE RID: 16606 RVA: 0x001521C5 File Offset: 0x001503C5
		public static MenuPlayServerInfoUI.EServerInfoOpenContext openContext { get; private set; }

		// Token: 0x060040DF RID: 16607 RVA: 0x001521D0 File Offset: 0x001503D0
		public static string GetClipboardData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Name: " + MenuPlayServerInfoUI.serverInfo.name);
			stringBuilder.AppendLine("Description: " + MenuPlayServerInfoUI.serverInfo.descText);
			stringBuilder.AppendLine("Thumbnail: " + MenuPlayServerInfoUI.serverInfo.thumbnailURL);
			stringBuilder.AppendLine("Address: " + Parser.getIPFromUInt32(MenuPlayServerInfoUI.serverInfo.ip));
			stringBuilder.AppendLine(string.Format("Connection Port: {0}", MenuPlayServerInfoUI.serverInfo.connectionPort));
			stringBuilder.AppendLine(string.Format("Query Port: {0}", MenuPlayServerInfoUI.serverInfo.queryPort));
			stringBuilder.AppendLine(string.Format("SteamId: {0} ({1})", MenuPlayServerInfoUI.serverInfo.steamID, MenuPlayServerInfoUI.serverInfo.steamID.GetEAccountType()));
			stringBuilder.AppendLine(string.Format("Ping: {0}ms", MenuPlayServerInfoUI.serverInfo.ping));
			if (MenuPlayServerInfoUI.expectedWorkshopItems == null)
			{
				stringBuilder.AppendLine("Workshop files unknown");
			}
			else
			{
				stringBuilder.AppendLine(string.Format("{0} workshop file(s):", MenuPlayServerInfoUI.expectedWorkshopItems.Count));
				for (int i = 0; i < MenuPlayServerInfoUI.expectedWorkshopItems.Count; i++)
				{
					stringBuilder.AppendLine(string.Format("{0}: {1}", i, MenuPlayServerInfoUI.expectedWorkshopItems[i]));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x00152362 File Offset: 0x00150562
		public static void OpenWithoutRefresh()
		{
			if (MenuPlayServerInfoUI.active)
			{
				return;
			}
			MenuPlayServerInfoUI.active = true;
			MenuPlayServerInfoUI.container.AnimateIntoView();
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x0015237C File Offset: 0x0015057C
		public static void open(SteamServerAdvertisement newServerInfo, string newServerPassword, MenuPlayServerInfoUI.EServerInfoOpenContext newOpenContext)
		{
			if (MenuPlayServerInfoUI.active)
			{
				return;
			}
			MenuPlayServerInfoUI.active = true;
			MenuPlayServerInfoUI.openContext = newOpenContext;
			MenuPlayServerInfoUI.serverInfo = newServerInfo;
			MenuPlayServerInfoUI.serverPassword = newServerPassword;
			MenuPlayServerInfoUI.expectedWorkshopItems = null;
			MenuPlayServerInfoUI.linkUrls = null;
			bool flag = false;
			IPv4Address pv4Address;
			pv4Address..ctor(MenuPlayServerInfoUI.serverInfo.ip);
			MenuPlayServerInfoUI.serverBookmarkHost = null;
			bool flag2 = !MenuPlayServerInfoUI.serverInfo.steamID.BPersistentGameServerAccount() && pv4Address.IsWideAreaNetwork;
			flag2 &= (MenuPlayServerInfoUI.serverInfo.infoSource != SteamServerAdvertisement.EInfoSource.LanServerList);
			if (flag2)
			{
				UnturnedLog.info(string.Format("{0} is not logged in ({1}) and IP ({2}) is WAN", MenuPlayServerInfoUI.serverInfo.name, MenuPlayServerInfoUI.serverInfo.steamID, pv4Address));
			}
			MenuPlayServerInfoUI.notLoggedInWarningButton.IsVisible = flag2;
			if (flag2)
			{
				MenuPlayServerInfoUI.joinButton.IsVisible = false;
				MenuPlayServerInfoUI.joinDisabledBox.IsVisible = true;
				MenuPlayServerInfoUI.joinDisabledBox.Text = MenuPlayServerInfoUI.localization.format("NotLoggedInBlock_Label");
				MenuPlayServerInfoUI.joinDisabledBox.TooltipText = MenuPlayServerInfoUI.localization.format("NotLoggedInBlock_Tooltip");
			}
			else if (flag)
			{
				MenuPlayServerInfoUI.joinButton.IsVisible = false;
				MenuPlayServerInfoUI.joinDisabledBox.IsVisible = true;
				MenuPlayServerInfoUI.joinDisabledBox.Text = MenuPlayServerInfoUI.localization.format("ServerBlacklisted_Label");
				MenuPlayServerInfoUI.joinDisabledBox.TooltipText = MenuPlayServerInfoUI.localization.format("ServerBlacklisted_Tooltip");
			}
			else
			{
				MenuPlayServerInfoUI.joinButton.IsVisible = true;
				MenuPlayServerInfoUI.joinDisabledBox.IsVisible = false;
			}
			MenuPlayServerInfoUI.reset();
			MenuPlayServerInfoUI.serverFavorited = Provider.GetServerIsFavorited(MenuPlayServerInfoUI.serverInfo.ip, MenuPlayServerInfoUI.serverInfo.queryPort);
			MenuPlayServerInfoUI.updateFavorite();
			MenuPlayServerInfoUI.UpdateBookmarkButton();
			MenuPlayServerInfoUI.updatePlayers();
			Provider.provider.matchmakingService.refreshPlayers(MenuPlayServerInfoUI.serverInfo.ip, MenuPlayServerInfoUI.serverInfo.queryPort);
			Provider.provider.matchmakingService.refreshPlayers(MenuPlayServerInfoUI.serverInfo.ip, MenuPlayServerInfoUI.serverInfo.queryPort);
			MenuPlayServerInfoUI.updateRules();
			Provider.provider.matchmakingService.refreshRules(MenuPlayServerInfoUI.serverInfo.ip, MenuPlayServerInfoUI.serverInfo.queryPort);
			MenuPlayServerInfoUI.updateServerInfo();
			MenuPlayServerInfoUI.UpdateVisibleButtons();
			MenuPlayServerInfoUI.container.AnimateIntoView();
		}

		// Token: 0x060040E2 RID: 16610 RVA: 0x001525A0 File Offset: 0x001507A0
		public static void close()
		{
			if (!MenuPlayServerInfoUI.active)
			{
				return;
			}
			MenuPlayServerInfoUI.active = false;
			MenuPlayServerInfoUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x001525C4 File Offset: 0x001507C4
		private static void onClickedJoinButton(ISleekElement button)
		{
			IPv4Address address;
			address..ctor(MenuPlayServerInfoUI.serverInfo.ip);
			if (MenuPlayServerInfoUI.serverInfo.isPassworded && string.IsNullOrEmpty(MenuPlayServerInfoUI.serverPassword))
			{
				MenuServerPasswordUI.open(MenuPlayServerInfoUI.serverInfo, MenuPlayServerInfoUI.expectedWorkshopItems);
				MenuPlayServerInfoUI.close();
				return;
			}
			Provider.connect(new ServerConnectParameters(address, MenuPlayServerInfoUI.serverInfo.queryPort, MenuPlayServerInfoUI.serverInfo.connectionPort, MenuPlayServerInfoUI.serverPassword), MenuPlayServerInfoUI.serverInfo, MenuPlayServerInfoUI.expectedWorkshopItems);
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x0015263D File Offset: 0x0015083D
		private static void onClickedFavoriteButton(ISleekElement button)
		{
			MenuPlayServerInfoUI.serverFavorited = !MenuPlayServerInfoUI.serverFavorited;
			Provider.SetServerIsFavorited(MenuPlayServerInfoUI.serverInfo.ip, MenuPlayServerInfoUI.serverInfo.connectionPort, MenuPlayServerInfoUI.serverInfo.queryPort, MenuPlayServerInfoUI.serverFavorited);
			MenuPlayServerInfoUI.updateFavorite();
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x00152679 File Offset: 0x00150879
		private static void OnClickedBookmarkButton(ISleekElement button)
		{
			MenuPlayServerInfoUI.UpdateBookmarkButton();
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x00152680 File Offset: 0x00150880
		private static void onClickedRefreshButton(ISleekElement button)
		{
			MenuPlayServerInfoUI.updatePlayers();
			Provider.provider.matchmakingService.refreshPlayers(MenuPlayServerInfoUI.serverInfo.ip, MenuPlayServerInfoUI.serverInfo.queryPort);
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x001526AC File Offset: 0x001508AC
		private static void onClickedCancelButton(ISleekElement button)
		{
			switch (MenuPlayServerInfoUI.openContext)
			{
			case MenuPlayServerInfoUI.EServerInfoOpenContext.CONNECT:
				MenuPlayConnectUI.open();
				break;
			case MenuPlayServerInfoUI.EServerInfoOpenContext.SERVERS:
				MenuPlayUI.serverListUI.open(false);
				break;
			case MenuPlayServerInfoUI.EServerInfoOpenContext.BOOKMARKS:
				MenuPlayUI.serverBookmarksUI.open();
				break;
			}
			MenuPlayServerInfoUI.close();
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x001526F6 File Offset: 0x001508F6
		private static void onMasterServerQueryRefreshed(SteamServerAdvertisement server)
		{
			MenuPlayServerInfoUI.serverInfo = server;
			MenuPlayServerInfoUI.updateServerInfo();
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x00152703 File Offset: 0x00150903
		private static void reset()
		{
			MenuPlayServerInfoUI.titleDescriptionLabel.Text = "";
			MenuPlayServerInfoUI.titleIconImage.Clear();
			MenuPlayServerInfoUI.serverDescriptionBox.Text = "";
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x00152730 File Offset: 0x00150930
		private static void updateServerInfo()
		{
			MenuPlayServerInfoUI.titleNameLabel.TextColor = (MenuPlayServerInfoUI.serverInfo.isPro ? new SleekColor(Palette.PRO) : new SleekColor(3));
			MenuPlayServerInfoUI.titleNameLabel.Text = MenuPlayServerInfoUI.serverInfo.name;
			int num = 0;
			MenuPlayServerInfoUI.serverWorkshopLabel.Text = MenuPlayServerInfoUI.localization.format("Workshop", MenuPlayServerInfoUI.localization.format(MenuPlayServerInfoUI.serverInfo.isWorkshop ? "Yes" : "No"));
			num += 20;
			MenuPlayServerInfoUI.serverCombatLabel.Text = MenuPlayServerInfoUI.localization.format("Combat", MenuPlayServerInfoUI.localization.format(MenuPlayServerInfoUI.serverInfo.isPvP ? "PvP" : "PvE"));
			num += 20;
			string text;
			switch (MenuPlayServerInfoUI.serverInfo.cameraMode)
			{
			case ECameraMode.FIRST:
				text = MenuPlayServerInfoUI.localization.format("First");
				break;
			case ECameraMode.THIRD:
				text = MenuPlayServerInfoUI.localization.format("Third");
				break;
			case ECameraMode.BOTH:
				text = MenuPlayServerInfoUI.localization.format("Both");
				break;
			case ECameraMode.VEHICLE:
				text = MenuPlayServerInfoUI.localization.format("Vehicle");
				break;
			default:
				text = string.Empty;
				break;
			}
			MenuPlayServerInfoUI.serverPerspectiveLabel.Text = MenuPlayServerInfoUI.localization.format("Perspective", text);
			MenuPlayServerInfoUI.serverPerspectiveLabel.IsVisible = !string.IsNullOrEmpty(text);
			num += (MenuPlayServerInfoUI.serverPerspectiveLabel.IsVisible ? 20 : 0);
			string text2;
			if (MenuPlayServerInfoUI.serverInfo.IsVACSecure)
			{
				text2 = MenuPlayServerInfoUI.localization.format("VAC_Secure");
			}
			else
			{
				text2 = MenuPlayServerInfoUI.localization.format("VAC_Insecure");
			}
			if (MenuPlayServerInfoUI.serverInfo.IsBattlEyeSecure)
			{
				text2 = text2 + " + " + MenuPlayServerInfoUI.localization.format("BattlEye_Secure");
			}
			else
			{
				text2 = text2 + " + " + MenuPlayServerInfoUI.localization.format("BattlEye_Insecure");
			}
			MenuPlayServerInfoUI.serverSecurityLabel.PositionOffset_Y = (float)num;
			MenuPlayServerInfoUI.serverSecurityLabel.Text = MenuPlayServerInfoUI.localization.format("Security", text2);
			num += 20;
			string arg;
			switch (MenuPlayServerInfoUI.serverInfo.mode)
			{
			case EGameMode.EASY:
				arg = MenuPlayServerInfoUI.localization.format("Easy");
				break;
			case EGameMode.NORMAL:
				arg = MenuPlayServerInfoUI.localization.format("Normal");
				break;
			case EGameMode.HARD:
				arg = MenuPlayServerInfoUI.localization.format("Hard");
				break;
			default:
				arg = string.Empty;
				break;
			}
			MenuPlayServerInfoUI.serverModeLabel.PositionOffset_Y = (float)num;
			MenuPlayServerInfoUI.serverModeLabel.Text = MenuPlayServerInfoUI.localization.format("Mode", arg);
			num += 20;
			MenuPlayServerInfoUI.serverCheatsLabel.PositionOffset_Y = (float)num;
			MenuPlayServerInfoUI.serverCheatsLabel.Text = MenuPlayServerInfoUI.localization.format("Cheats", MenuPlayServerInfoUI.localization.format(MenuPlayServerInfoUI.serverInfo.hasCheats ? "Yes" : "No"));
			num += 20;
			if (MenuPlayServerInfoUI.serverInfo.monetization != EServerMonetizationTag.Unspecified)
			{
				MenuPlayServerInfoUI.serverMonetizationLabel.IsVisible = true;
				MenuPlayServerInfoUI.serverMonetizationLabel.PositionOffset_Y = (float)num;
				switch (MenuPlayServerInfoUI.serverInfo.monetization)
				{
				case EServerMonetizationTag.None:
					MenuPlayServerInfoUI.serverMonetizationLabel.Text = MenuPlayServerInfoUI.localization.format("Monetization_None");
					break;
				case EServerMonetizationTag.NonGameplay:
					MenuPlayServerInfoUI.serverMonetizationLabel.Text = MenuPlayServerInfoUI.localization.format("Monetization_NonGameplay");
					break;
				case EServerMonetizationTag.Monetized:
					MenuPlayServerInfoUI.serverMonetizationLabel.Text = MenuPlayServerInfoUI.localization.format("Monetization_Monetized");
					break;
				default:
					MenuPlayServerInfoUI.serverMonetizationLabel.Text = "unknown: " + MenuPlayServerInfoUI.serverInfo.monetization.ToString();
					break;
				}
				num += 20;
			}
			else
			{
				MenuPlayServerInfoUI.serverMonetizationLabel.IsVisible = false;
			}
			MenuPlayServerInfoUI.serverPingLabel.Text = MenuPlayServerInfoUI.localization.format("QueryPing", MenuPlayServerInfoUI.serverInfo.ping);
			MenuPlayServerInfoUI.serverPingLabel.PositionOffset_Y = (float)num;
			num += 20;
			MenuPlayServerInfoUI.serverBox.SizeOffset_Y = (float)(num + 10);
			MenuPlayServerInfoUI.updateDetails();
			LevelInfo level = Level.getLevel(MenuPlayServerInfoUI.serverInfo.map);
			if (level != null)
			{
				Local local = level.getLocalization();
				if (local != null)
				{
					string text3 = local.format("Description");
					text3 = ItemTool.filterRarityRichText(text3);
					RichTextUtil.replaceNewlineMarkup(ref text3);
					MenuPlayServerInfoUI.mapDescriptionBox.Text = text3;
				}
				if (local != null && local.has("Name"))
				{
					MenuPlayServerInfoUI.mapNameBox.Text = MenuPlayServerInfoUI.localization.format("Map", local.format("Name"));
				}
				else
				{
					MenuPlayServerInfoUI.mapNameBox.Text = MenuPlayServerInfoUI.localization.format("Map", MenuPlayServerInfoUI.serverInfo.map);
				}
				string previewImageFilePath = level.GetPreviewImageFilePath();
				if (!string.IsNullOrEmpty(previewImageFilePath))
				{
					MenuPlayServerInfoUI.mapPreviewImage.SetTextureAndShouldDestroy(ReadWrite.readTextureFromFile(previewImageFilePath, EReadTextureFromFileMode.UI), true);
					return;
				}
			}
			else
			{
				MenuPlayServerInfoUI.mapDescriptionBox.Text = string.Empty;
				MenuPlayServerInfoUI.mapNameBox.Text = MenuPlayServerInfoUI.serverInfo.map;
				MenuPlayServerInfoUI.mapPreviewImage.SetTextureAndShouldDestroy(null, true);
			}
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x00152C48 File Offset: 0x00150E48
		private static void updateFavorite()
		{
			MenuPlayServerInfoUI.favoriteButton.IsVisible = !MenuPlayServerInfoUI.serverInfo.IsAddressUsingSteamFakeIP();
			if (MenuPlayServerInfoUI.serverFavorited)
			{
				MenuPlayServerInfoUI.favoriteButton.Text = MenuPlayServerInfoUI.localization.format("Favorite_Off_Button");
				return;
			}
			MenuPlayServerInfoUI.favoriteButton.Text = MenuPlayServerInfoUI.localization.format("Favorite_On_Button");
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x00152CA6 File Offset: 0x00150EA6
		private static void UpdateBookmarkButton()
		{
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x00152CA8 File Offset: 0x00150EA8
		private static void updatePlayers()
		{
			MenuPlayServerInfoUI.playersScrollBox.RemoveAllChildren();
			MenuPlayServerInfoUI.playersOffset = 0;
			MenuPlayServerInfoUI.playerCount = 0;
			MenuPlayServerInfoUI.playerCountBox.Text = MenuPlayServerInfoUI.localization.format("Players", MenuPlayServerInfoUI.playerCount, MenuPlayServerInfoUI.serverInfo.maxPlayers);
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x00152D00 File Offset: 0x00150F00
		private static void onPlayersQueryRefreshed(string name, int score, float time)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)time);
			string text = string.Empty;
			if (timeSpan.Days > 0)
			{
				text = text + " " + timeSpan.Days.ToString() + "d";
			}
			if (timeSpan.Hours > 0)
			{
				text = text + " " + timeSpan.Hours.ToString() + "h";
			}
			if (timeSpan.Minutes > 0)
			{
				text = text + " " + timeSpan.Minutes.ToString() + "m";
			}
			if (timeSpan.Seconds > 0)
			{
				text = text + " " + timeSpan.Seconds.ToString() + "s";
			}
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.PositionOffset_Y = (float)MenuPlayServerInfoUI.playersOffset;
			sleekBox.SizeOffset_Y = 30f;
			sleekBox.SizeScale_X = 1f;
			MenuPlayServerInfoUI.playersScrollBox.AddChild(sleekBox);
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_X = 5f;
			sleekLabel.SizeOffset_X = -10f;
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.SizeScale_Y = 1f;
			sleekLabel.TextAlignment = 3;
			sleekLabel.Text = name;
			sleekBox.AddChild(sleekLabel);
			ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
			sleekLabel2.PositionOffset_X = -5f;
			sleekLabel2.SizeOffset_X = -10f;
			sleekLabel2.SizeScale_X = 1f;
			sleekLabel2.SizeScale_Y = 1f;
			sleekLabel2.TextAlignment = 5;
			sleekLabel2.Text = text;
			sleekBox.AddChild(sleekLabel2);
			MenuPlayServerInfoUI.playersOffset += 40;
			MenuPlayServerInfoUI.playersScrollBox.ContentSizeOffset = new Vector2(0f, (float)(MenuPlayServerInfoUI.playersOffset - 10));
			MenuPlayServerInfoUI.playerCount++;
			MenuPlayServerInfoUI.playerCountBox.Text = MenuPlayServerInfoUI.localization.format("Players", MenuPlayServerInfoUI.playerCount, MenuPlayServerInfoUI.serverInfo.maxPlayers);
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x00152F08 File Offset: 0x00151108
		private static void updateRules()
		{
			MenuPlayServerInfoUI.linksFrame.RemoveAllChildren();
			MenuPlayServerInfoUI.linksFrame.IsVisible = false;
			MenuPlayServerInfoUI.ugcTitle.IsVisible = false;
			MenuPlayServerInfoUI.ugcBox.RemoveAllChildren();
			MenuPlayServerInfoUI.ugcBox.IsVisible = false;
			MenuPlayServerInfoUI.configTitle.IsVisible = false;
			MenuPlayServerInfoUI.configBox.RemoveAllChildren();
			MenuPlayServerInfoUI.configBox.IsVisible = false;
			MenuPlayServerInfoUI.rocketTitle.IsVisible = false;
			MenuPlayServerInfoUI.rocketBox.RemoveAllChildren();
			MenuPlayServerInfoUI.rocketBox.IsVisible = false;
			MenuPlayServerInfoUI.updateDetails();
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x00152F90 File Offset: 0x00151190
		private static void onRulesQueryRefreshed(Dictionary<string, string> rulesMap)
		{
			if (rulesMap == null)
			{
				return;
			}
			string text;
			if (rulesMap.TryGetValue("Browser_Icon", ref text) && !string.IsNullOrEmpty(text))
			{
				MenuPlayServerInfoUI.titleIconImage.Refresh(text, true);
			}
			string text2;
			if (rulesMap.TryGetValue("Browser_Desc_Hint", ref text2) && !string.IsNullOrEmpty(text2))
			{
				ProfanityFilter.ApplyFilter(OptionsSettings.filter, ref text2);
				MenuPlayServerInfoUI.titleDescriptionLabel.Text = text2;
			}
			string text3;
			if (rulesMap.TryGetValue("BookmarkHost", ref text3))
			{
				MenuPlayServerInfoUI.serverBookmarkHost = text3;
			}
			string text4;
			int num;
			if (rulesMap.TryGetValue("Browser_Desc_Full_Count", ref text4) && int.TryParse(text4, 511, CultureInfo.InvariantCulture, ref num) && num > 0)
			{
				string text5 = string.Empty;
				for (int i = 0; i < num; i++)
				{
					string text6;
					if (rulesMap.TryGetValue("Browser_Desc_Full_Line_" + i.ToString(), ref text6))
					{
						text5 += text6;
					}
				}
				string text7;
				if (ConvertEx.TryDecodeBase64AsUtf8String(text5, ref text7))
				{
					if (!string.IsNullOrEmpty(text7))
					{
						ProfanityFilter.ApplyFilter(OptionsSettings.filter, ref text7);
						RichTextUtil.replaceNewlineMarkup(ref text7);
						MenuPlayServerInfoUI.serverDescriptionBox.Text = text7;
					}
				}
				else
				{
					UnturnedLog.error("Unable to convert server browser Base64 string: \"" + text5 + "\"");
				}
			}
			MenuPlayServerInfoUI.linkUrls = new List<string>();
			string text8;
			int num2;
			if (rulesMap.TryGetValue("Custom_Links_Count", ref text8) && int.TryParse(text8, 511, CultureInfo.InvariantCulture, ref num2) && num2 > 0)
			{
				int num3 = 0;
				for (int j = 0; j < num2; j++)
				{
					string text9;
					string text10;
					string text11;
					string text12;
					string text13;
					if (!rulesMap.TryGetValue("Custom_Link_Message_" + j.ToString(), ref text9))
					{
						UnturnedLog.warn("Skipping link index {0} because message is missing", new object[]
						{
							j
						});
					}
					else if (string.IsNullOrEmpty(text9))
					{
						UnturnedLog.warn("Skipping link index {0} because message is empty", new object[]
						{
							j
						});
					}
					else if (!rulesMap.TryGetValue("Custom_Link_Url_" + j.ToString(), ref text10))
					{
						UnturnedLog.warn("Skipping link index {0} because url is missing", new object[]
						{
							j
						});
					}
					else if (string.IsNullOrEmpty(text10))
					{
						UnturnedLog.warn("Skipping link index {0} because url is empty", new object[]
						{
							j
						});
					}
					else if (!ConvertEx.TryDecodeBase64AsUtf8String(text9, ref text11))
					{
						UnturnedLog.warn("Skipping link index {0} because unable to decode message Base64: \"{1}\"", new object[]
						{
							j,
							text9
						});
					}
					else if (!ConvertEx.TryDecodeBase64AsUtf8String(text10, ref text12))
					{
						UnturnedLog.warn("Skipping link index {0} because unable to decode url Base64: \"{1}\"", new object[]
						{
							j,
							text10
						});
					}
					else if (!WebUtils.ParseThirdPartyUrl(text12, out text13, true, true))
					{
						UnturnedLog.warn("Ignoring potentially unsafe link index {0} url {1}", new object[]
						{
							j,
							text12
						});
					}
					else
					{
						ProfanityFilter.ApplyFilter(OptionsSettings.filter, ref text11);
						MenuPlayServerInfoUI.linkUrls.Add(text13);
						ISleekButton sleekButton = Glazier.Get().CreateButton();
						sleekButton.PositionOffset_Y += (float)num3;
						sleekButton.SizeScale_X = 1f;
						sleekButton.SizeOffset_Y = 30f;
						sleekButton.AllowRichText = true;
						sleekButton.Text = text11;
						sleekButton.TooltipText = text12;
						sleekButton.TextColor = 4;
						sleekButton.OnClicked += new ClickedButton(MenuPlayServerInfoUI.OnClickedLinkButton);
						MenuPlayServerInfoUI.linksFrame.AddChild(sleekButton);
						num3 += 30;
					}
				}
				if (num3 > 0)
				{
					MenuPlayServerInfoUI.linksFrame.SizeOffset_Y = (float)num3;
					MenuPlayServerInfoUI.linksFrame.IsVisible = true;
				}
			}
			string text14;
			if (rulesMap.TryGetValue("rocketplugins", ref text14) && !string.IsNullOrEmpty(text14))
			{
				string[] array = text14.Split(',', 0);
				MenuPlayServerInfoUI.rocketBox.SizeOffset_Y = (float)(array.Length * 20 + 10);
				for (int k = 0; k < array.Length; k++)
				{
					ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
					sleekLabel.PositionOffset_X = 5f;
					sleekLabel.PositionOffset_Y = (float)(k * 20);
					sleekLabel.SizeOffset_Y = 30f;
					sleekLabel.SizeScale_X = 1f;
					sleekLabel.TextAlignment = 3;
					sleekLabel.Text = array[k];
					MenuPlayServerInfoUI.rocketBox.AddChild(sleekLabel);
				}
				if (MenuPlayServerInfoUI.serverInfo.pluginFramework == SteamServerAdvertisement.EPluginFramework.Rocket)
				{
					MenuPlayServerInfoUI.rocketTitle.Text = MenuPlayServerInfoUI.localization.format("Plugins_Rocket");
				}
				else if (MenuPlayServerInfoUI.serverInfo.pluginFramework == SteamServerAdvertisement.EPluginFramework.OpenMod)
				{
					MenuPlayServerInfoUI.rocketTitle.Text = MenuPlayServerInfoUI.localization.format("Plugins_OpenMod");
				}
				else
				{
					MenuPlayServerInfoUI.rocketTitle.Text = MenuPlayServerInfoUI.localization.format("Plugins_Unknown");
				}
				MenuPlayServerInfoUI.rocketTitle.IsVisible = true;
				MenuPlayServerInfoUI.rocketBox.IsVisible = true;
			}
			MenuPlayServerInfoUI.expectedWorkshopItems = new List<PublishedFileId_t>(0);
			string text15;
			int num4;
			if (rulesMap.TryGetValue("Mod_Count", ref text15) && int.TryParse(text15, 511, CultureInfo.InvariantCulture, ref num4) && num4 > 0)
			{
				string text16 = string.Empty;
				for (int l = 0; l < num4; l++)
				{
					string text17;
					if (rulesMap.TryGetValue("Mod_" + l.ToString(), ref text17))
					{
						text16 += text17;
					}
				}
				string[] array2 = text16.Split(',', 0);
				MenuPlayServerInfoUI.expectedWorkshopItems = new List<PublishedFileId_t>(array2.Length);
				for (int m = 0; m < array2.Length; m++)
				{
					ulong value;
					if (ulong.TryParse(array2[m], 511, CultureInfo.InvariantCulture, ref value))
					{
						MenuPlayServerInfoUI.expectedWorkshopItems.Add(new PublishedFileId_t(value));
					}
				}
				MenuPlayServerInfoUI.detailsHandle = SteamUGC.CreateQueryUGCDetailsRequest(MenuPlayServerInfoUI.expectedWorkshopItems.ToArray(), (uint)MenuPlayServerInfoUI.expectedWorkshopItems.Count);
				SteamUGC.SetAllowCachedResponse(MenuPlayServerInfoUI.detailsHandle, 60U);
				SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(MenuPlayServerInfoUI.detailsHandle);
				MenuPlayServerInfoUI.ugcQueryCompleted.Set(hAPICall, null);
			}
			string text18;
			int num5;
			if (rulesMap.TryGetValue("Cfg_Count", ref text18) && int.TryParse(text18, 511, CultureInfo.InvariantCulture, ref num5) && num5 > 0)
			{
				int num6 = 0;
				for (int n = 0; n < num5; n++)
				{
					string text19;
					if (rulesMap.TryGetValue("Cfg_" + n.ToString(CultureInfo.InvariantCulture), ref text19))
					{
						int num7 = text19.IndexOf('.');
						int num8 = text19.IndexOf('=', num7 + 1);
						if (num7 >= 0 && num8 >= 0)
						{
							string fieldName = text19.Substring(0, num7);
							string fieldName2 = text19.Substring(num7 + 1, num8 - num7 - 1);
							string text20 = text19.Substring(num8 + 1);
							string text21 = null;
							float num9;
							int num10;
							if (text20 == "T")
							{
								text21 = MenuPlayServerInfoUI.localization.format("Yes");
							}
							else if (text20 == "F")
							{
								text21 = MenuPlayServerInfoUI.localization.format("No");
							}
							else if (float.TryParse(text20, 511, CultureInfo.InvariantCulture, ref num9))
							{
								text21 = num9.ToString();
							}
							else if (int.TryParse(text20, 511, CultureInfo.InvariantCulture, ref num10))
							{
								text21 = num10.ToString();
							}
							if (string.IsNullOrEmpty(text21))
							{
								ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
								sleekLabel2.PositionOffset_X = 5f;
								sleekLabel2.PositionOffset_Y = (float)num6;
								sleekLabel2.SizeOffset_X = -10f;
								sleekLabel2.SizeOffset_Y = 30f;
								sleekLabel2.SizeScale_X = 1f;
								sleekLabel2.TextAlignment = 3;
								sleekLabel2.TextColor = Color.red;
								sleekLabel2.Text = text19;
								MenuPlayServerInfoUI.configBox.AddChild(sleekLabel2);
							}
							else
							{
								ISleekLabel sleekLabel3 = Glazier.Get().CreateLabel();
								sleekLabel3.PositionOffset_X = 5f;
								sleekLabel3.PositionOffset_Y = (float)num6;
								sleekLabel3.SizeOffset_X = -5f;
								sleekLabel3.SizeOffset_Y = 30f;
								sleekLabel3.SizeScale_X = 0.25f;
								sleekLabel3.TextAlignment = 5;
								sleekLabel3.Text = MenuPlayConfigUI.sanitizeName(fieldName);
								sleekLabel3.TextColor = new SleekColor(3, 0.5f);
								MenuPlayServerInfoUI.configBox.AddChild(sleekLabel3);
								ISleekLabel sleekLabel4 = Glazier.Get().CreateLabel();
								sleekLabel4.PositionOffset_X = 5f;
								sleekLabel4.PositionOffset_Y = (float)num6;
								sleekLabel4.PositionScale_X = 0.25f;
								sleekLabel4.SizeOffset_X = -5f;
								sleekLabel4.SizeOffset_Y = 30f;
								sleekLabel4.SizeScale_X = 0.75f;
								sleekLabel4.TextAlignment = 3;
								sleekLabel4.Text = MenuPlayServerInfoUI.localization.format("Rule", MenuPlayConfigUI.sanitizeName(fieldName2), text21);
								MenuPlayServerInfoUI.configBox.AddChild(sleekLabel4);
							}
							num6 += 20;
						}
					}
				}
				MenuPlayServerInfoUI.configBox.SizeOffset_Y = (float)(num6 + 10);
				if (num6 > 0)
				{
					MenuPlayServerInfoUI.configTitle.IsVisible = true;
					MenuPlayServerInfoUI.configBox.IsVisible = true;
				}
			}
			string text22;
			uint num11;
			if (rulesMap.TryGetValue("GameVersion", ref text22) && Parser.TryGetUInt32FromIP(text22, out num11) && Provider.APP_VERSION_PACKED != num11)
			{
				MenuPlayServerInfoUI.joinButton.IsVisible = false;
				MenuPlayServerInfoUI.joinDisabledBox.IsVisible = true;
				if (num11 > Provider.APP_VERSION_PACKED)
				{
					MenuPlayServerInfoUI.joinDisabledBox.Text = MenuPlayServerInfoUI.localization.format("ServerNewerVersion_Label", text22);
					MenuPlayServerInfoUI.joinDisabledBox.TooltipText = MenuPlayServerInfoUI.localization.format("ServerNewerVersion_Tooltip");
				}
				else
				{
					MenuPlayServerInfoUI.joinDisabledBox.Text = MenuPlayServerInfoUI.localization.format("ServerOlderVersion_Label", text22);
					MenuPlayServerInfoUI.joinDisabledBox.TooltipText = MenuPlayServerInfoUI.localization.format("ServerOlderVersion_Tooltip");
				}
			}
			MenuPlayServerInfoUI.updateDetails();
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x0015391C File Offset: 0x00151B1C
		private static void updateDetails()
		{
			float num = 0f;
			if (MenuPlayServerInfoUI.hostBanWarningButton.IsVisible)
			{
				MenuPlayServerInfoUI.hostBanWarningButton.PositionOffset_X = num;
				num += MenuPlayServerInfoUI.hostBanWarningButton.SizeOffset_Y + 10f;
			}
			if (MenuPlayServerInfoUI.notLoggedInWarningButton.IsVisible)
			{
				MenuPlayServerInfoUI.notLoggedInWarningButton.PositionOffset_X = num;
				num += MenuPlayServerInfoUI.notLoggedInWarningButton.SizeOffset_Y + 10f;
			}
			if (MenuPlayServerInfoUI.linksFrame.IsVisible)
			{
				MenuPlayServerInfoUI.linksFrame.PositionOffset_Y = num;
				num += MenuPlayServerInfoUI.linksFrame.SizeOffset_Y + 10f;
			}
			if (MenuPlayServerInfoUI.serverTitle.IsVisible)
			{
				MenuPlayServerInfoUI.serverTitle.PositionOffset_Y = num;
				num += 40f;
			}
			if (MenuPlayServerInfoUI.serverBox.IsVisible)
			{
				MenuPlayServerInfoUI.serverBox.PositionOffset_Y = num;
				num += MenuPlayServerInfoUI.serverBox.SizeOffset_Y + 10f;
			}
			if (MenuPlayServerInfoUI.ugcTitle.IsVisible)
			{
				MenuPlayServerInfoUI.ugcTitle.PositionOffset_Y = num;
				num += MenuPlayServerInfoUI.ugcTitle.SizeOffset_Y + 10f;
			}
			if (MenuPlayServerInfoUI.ugcBox.IsVisible)
			{
				MenuPlayServerInfoUI.ugcBox.PositionOffset_Y = num;
				num += MenuPlayServerInfoUI.ugcBox.SizeOffset_Y + 10f;
			}
			if (MenuPlayServerInfoUI.configTitle.IsVisible)
			{
				MenuPlayServerInfoUI.configTitle.PositionOffset_Y = num;
				num += MenuPlayServerInfoUI.configTitle.SizeOffset_Y + 10f;
			}
			if (MenuPlayServerInfoUI.configBox.IsVisible)
			{
				MenuPlayServerInfoUI.configBox.PositionOffset_Y = num;
				num += MenuPlayServerInfoUI.configBox.SizeOffset_Y + 10f;
			}
			if (MenuPlayServerInfoUI.rocketTitle.IsVisible)
			{
				MenuPlayServerInfoUI.rocketTitle.PositionOffset_Y = num;
				num += MenuPlayServerInfoUI.rocketTitle.SizeOffset_Y + 10f;
			}
			if (MenuPlayServerInfoUI.rocketBox.IsVisible)
			{
				MenuPlayServerInfoUI.rocketBox.PositionOffset_Y = num;
				num += MenuPlayServerInfoUI.rocketBox.SizeOffset_Y + 10f;
			}
			MenuPlayServerInfoUI.detailsScrollBox.ContentSizeOffset = new Vector2(0f, num - 10f);
		}

		/// <summary>
		/// Adjusts width and spacing of buttons along the bottom of the screen.
		/// Favorite and bookmark buttons can be hidden depending whether the necessary server details are set.
		/// </summary>
		// Token: 0x060040F2 RID: 16626 RVA: 0x00153B10 File Offset: 0x00151D10
		private static void UpdateVisibleButtons()
		{
			int num = 3;
			if (MenuPlayServerInfoUI.favoriteButton.IsVisible)
			{
				num++;
			}
			if (MenuPlayServerInfoUI.bookmarkButton.IsVisible)
			{
				num++;
			}
			float num2 = 1f / (float)num;
			MenuPlayServerInfoUI.joinButton.SizeScale_X = num2;
			MenuPlayServerInfoUI.joinDisabledBox.SizeScale_X = num2;
			float num3 = num2;
			if (MenuPlayServerInfoUI.favoriteButton.IsVisible)
			{
				MenuPlayServerInfoUI.favoriteButton.PositionScale_X = num3;
				MenuPlayServerInfoUI.favoriteButton.SizeScale_X = num2;
				num3 += num2;
			}
			if (MenuPlayServerInfoUI.bookmarkButton.IsVisible)
			{
				MenuPlayServerInfoUI.bookmarkButton.PositionScale_X = num3;
				MenuPlayServerInfoUI.bookmarkButton.SizeScale_X = num2;
				num3 += num2;
			}
			MenuPlayServerInfoUI.refreshButton.PositionScale_X = num3;
			MenuPlayServerInfoUI.refreshButton.SizeScale_X = num2;
			num3 += num2;
			MenuPlayServerInfoUI.cancelButton.PositionScale_X = 1f - num2;
			MenuPlayServerInfoUI.cancelButton.SizeScale_X = num2;
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x00153BE4 File Offset: 0x00151DE4
		private static void OnClickedLinkButton(ISleekElement button)
		{
			int num = MenuPlayServerInfoUI.linksFrame.FindIndexOfChild(button);
			Provider.openURL(MenuPlayServerInfoUI.linkUrls[num]);
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x00153C0D File Offset: 0x00151E0D
		private static void OnClickedHostBanWarning(ISleekElement button)
		{
			Provider.openURL("https://docs.smartlydressedgames.com/en/stable/servers/server-hosting-rules.html");
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x00153C19 File Offset: 0x00151E19
		private static void OnClickedNotLoggedInWarning(ISleekElement button)
		{
			Provider.openURL("https://docs.smartlydressedgames.com/en/stable/servers/game-server-login-tokens.html");
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x00153C28 File Offset: 0x00151E28
		public void OnDestroy()
		{
			TempSteamworksMatchmaking matchmakingService = Provider.provider.matchmakingService;
			matchmakingService.onMasterServerQueryRefreshed = (TempSteamworksMatchmaking.MasterServerQueryRefreshed)Delegate.Remove(matchmakingService.onMasterServerQueryRefreshed, new TempSteamworksMatchmaking.MasterServerQueryRefreshed(MenuPlayServerInfoUI.onMasterServerQueryRefreshed));
			TempSteamworksMatchmaking matchmakingService2 = Provider.provider.matchmakingService;
			matchmakingService2.onPlayersQueryRefreshed = (TempSteamworksMatchmaking.PlayersQueryRefreshed)Delegate.Remove(matchmakingService2.onPlayersQueryRefreshed, new TempSteamworksMatchmaking.PlayersQueryRefreshed(MenuPlayServerInfoUI.onPlayersQueryRefreshed));
			TempSteamworksMatchmaking matchmakingService3 = Provider.provider.matchmakingService;
			matchmakingService3.onRulesQueryRefreshed = (TempSteamworksMatchmaking.RulesQueryRefreshed)Delegate.Remove(matchmakingService3.onRulesQueryRefreshed, new TempSteamworksMatchmaking.RulesQueryRefreshed(MenuPlayServerInfoUI.onRulesQueryRefreshed));
			if (MenuPlayServerInfoUI.ugcQueryCompleted != null)
			{
				MenuPlayServerInfoUI.ugcQueryCompleted.Dispose();
				MenuPlayServerInfoUI.ugcQueryCompleted = null;
			}
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x00153CD0 File Offset: 0x00151ED0
		public MenuPlayServerInfoUI()
		{
			MenuPlayServerInfoUI.localization = Localization.read("/Menu/Play/MenuPlayServerInfo.dat");
			MenuPlayServerInfoUI.container = new SleekFullscreenBox();
			MenuPlayServerInfoUI.container.PositionOffset_X = 10f;
			MenuPlayServerInfoUI.container.PositionOffset_Y = 10f;
			MenuPlayServerInfoUI.container.PositionScale_Y = 1f;
			MenuPlayServerInfoUI.container.SizeOffset_X = -20f;
			MenuPlayServerInfoUI.container.SizeOffset_Y = -20f;
			MenuPlayServerInfoUI.container.SizeScale_X = 1f;
			MenuPlayServerInfoUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPlayServerInfoUI.container);
			MenuPlayServerInfoUI.active = false;
			MenuPlayServerInfoUI.infoContainer = Glazier.Get().CreateFrame();
			MenuPlayServerInfoUI.infoContainer.PositionOffset_Y = 94f;
			MenuPlayServerInfoUI.infoContainer.SizeOffset_Y = -154f;
			MenuPlayServerInfoUI.infoContainer.SizeScale_X = 1f;
			MenuPlayServerInfoUI.infoContainer.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.container.AddChild(MenuPlayServerInfoUI.infoContainer);
			MenuPlayServerInfoUI.buttonsContainer = Glazier.Get().CreateFrame();
			MenuPlayServerInfoUI.buttonsContainer.PositionOffset_Y = -50f;
			MenuPlayServerInfoUI.buttonsContainer.PositionScale_Y = 1f;
			MenuPlayServerInfoUI.buttonsContainer.SizeOffset_Y = 50f;
			MenuPlayServerInfoUI.buttonsContainer.SizeScale_X = 1f;
			MenuPlayServerInfoUI.container.AddChild(MenuPlayServerInfoUI.buttonsContainer);
			MenuPlayServerInfoUI.playersContainer = Glazier.Get().CreateFrame();
			MenuPlayServerInfoUI.playersContainer.SizeOffset_X = 280f;
			MenuPlayServerInfoUI.playersContainer.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.infoContainer.AddChild(MenuPlayServerInfoUI.playersContainer);
			MenuPlayServerInfoUI.detailsContainer = Glazier.Get().CreateFrame();
			MenuPlayServerInfoUI.detailsContainer.PositionOffset_X = 290f;
			MenuPlayServerInfoUI.detailsContainer.SizeOffset_X = -MenuPlayServerInfoUI.detailsContainer.PositionOffset_X - 350f;
			MenuPlayServerInfoUI.detailsContainer.SizeScale_X = 1f;
			MenuPlayServerInfoUI.detailsContainer.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.infoContainer.AddChild(MenuPlayServerInfoUI.detailsContainer);
			MenuPlayServerInfoUI.mapContainer = Glazier.Get().CreateFrame();
			MenuPlayServerInfoUI.mapContainer.PositionOffset_X = -340f;
			MenuPlayServerInfoUI.mapContainer.PositionScale_X = 1f;
			MenuPlayServerInfoUI.mapContainer.SizeOffset_X = 340f;
			MenuPlayServerInfoUI.mapContainer.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.infoContainer.AddChild(MenuPlayServerInfoUI.mapContainer);
			MenuPlayServerInfoUI.titleBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.titleBox.SizeOffset_Y = 84f;
			MenuPlayServerInfoUI.titleBox.SizeScale_X = 1f;
			MenuPlayServerInfoUI.container.AddChild(MenuPlayServerInfoUI.titleBox);
			MenuPlayServerInfoUI.titleIconImage = new SleekWebImage();
			MenuPlayServerInfoUI.titleIconImage.PositionOffset_X = 10f;
			MenuPlayServerInfoUI.titleIconImage.PositionOffset_Y = 10f;
			MenuPlayServerInfoUI.titleIconImage.SizeOffset_X = 64f;
			MenuPlayServerInfoUI.titleIconImage.SizeOffset_Y = 64f;
			MenuPlayServerInfoUI.titleBox.AddChild(MenuPlayServerInfoUI.titleIconImage);
			float positionOffset_X = (MenuPlayServerInfoUI.playersContainer.SizeOffset_X - MenuPlayServerInfoUI.mapContainer.SizeOffset_X) / 2f;
			MenuPlayServerInfoUI.titleNameLabel = Glazier.Get().CreateLabel();
			MenuPlayServerInfoUI.titleNameLabel.PositionOffset_X = positionOffset_X;
			MenuPlayServerInfoUI.titleNameLabel.PositionOffset_Y = 5f;
			MenuPlayServerInfoUI.titleNameLabel.SizeOffset_Y = 40f;
			MenuPlayServerInfoUI.titleNameLabel.SizeScale_X = 1f;
			MenuPlayServerInfoUI.titleNameLabel.FontSize = 4;
			MenuPlayServerInfoUI.titleBox.AddChild(MenuPlayServerInfoUI.titleNameLabel);
			MenuPlayServerInfoUI.titleDescriptionLabel = Glazier.Get().CreateLabel();
			MenuPlayServerInfoUI.titleDescriptionLabel.PositionOffset_X = positionOffset_X;
			MenuPlayServerInfoUI.titleDescriptionLabel.PositionOffset_Y = 45f;
			MenuPlayServerInfoUI.titleDescriptionLabel.SizeOffset_Y = 34f;
			MenuPlayServerInfoUI.titleDescriptionLabel.SizeScale_X = 1f;
			MenuPlayServerInfoUI.titleDescriptionLabel.AllowRichText = true;
			MenuPlayServerInfoUI.titleDescriptionLabel.TextColor = 4;
			MenuPlayServerInfoUI.titleDescriptionLabel.TextContrastContext = 1;
			MenuPlayServerInfoUI.titleBox.AddChild(MenuPlayServerInfoUI.titleDescriptionLabel);
			MenuPlayServerInfoUI.playerCountBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.playerCountBox.SizeScale_X = 1f;
			MenuPlayServerInfoUI.playerCountBox.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.playersContainer.AddChild(MenuPlayServerInfoUI.playerCountBox);
			MenuPlayServerInfoUI.playersScrollBox = Glazier.Get().CreateScrollView();
			MenuPlayServerInfoUI.playersScrollBox.PositionOffset_Y = 40f;
			MenuPlayServerInfoUI.playersScrollBox.SizeScale_X = 1f;
			MenuPlayServerInfoUI.playersScrollBox.SizeOffset_Y = -40f;
			MenuPlayServerInfoUI.playersScrollBox.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.playersScrollBox.ScaleContentToWidth = true;
			MenuPlayServerInfoUI.playersContainer.AddChild(MenuPlayServerInfoUI.playersScrollBox);
			MenuPlayServerInfoUI.detailsBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.detailsBox.SizeScale_X = 1f;
			MenuPlayServerInfoUI.detailsBox.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.detailsBox.Text = MenuPlayServerInfoUI.localization.format("Details");
			MenuPlayServerInfoUI.detailsContainer.AddChild(MenuPlayServerInfoUI.detailsBox);
			MenuPlayServerInfoUI.detailsScrollBox = Glazier.Get().CreateScrollView();
			MenuPlayServerInfoUI.detailsScrollBox.PositionOffset_Y = 40f;
			MenuPlayServerInfoUI.detailsScrollBox.SizeScale_X = 1f;
			MenuPlayServerInfoUI.detailsScrollBox.SizeOffset_Y = -40f;
			MenuPlayServerInfoUI.detailsScrollBox.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.detailsScrollBox.ScaleContentToWidth = true;
			MenuPlayServerInfoUI.detailsContainer.AddChild(MenuPlayServerInfoUI.detailsScrollBox);
			MenuPlayServerInfoUI.hostBanWarningButton = Glazier.Get().CreateButton();
			MenuPlayServerInfoUI.hostBanWarningButton.SizeOffset_Y = 60f;
			MenuPlayServerInfoUI.hostBanWarningButton.SizeScale_X = 1f;
			MenuPlayServerInfoUI.hostBanWarningButton.IsVisible = false;
			MenuPlayServerInfoUI.hostBanWarningButton.OnClicked += new ClickedButton(MenuPlayServerInfoUI.OnClickedHostBanWarning);
			MenuPlayServerInfoUI.detailsScrollBox.AddChild(MenuPlayServerInfoUI.hostBanWarningButton);
			MenuPlayServerInfoUI.notLoggedInWarningButton = Glazier.Get().CreateButton();
			MenuPlayServerInfoUI.notLoggedInWarningButton.SizeOffset_Y = 60f;
			MenuPlayServerInfoUI.notLoggedInWarningButton.SizeScale_X = 1f;
			MenuPlayServerInfoUI.notLoggedInWarningButton.IsVisible = false;
			MenuPlayServerInfoUI.notLoggedInWarningButton.OnClicked += new ClickedButton(MenuPlayServerInfoUI.OnClickedNotLoggedInWarning);
			ISleekButton sleekButton = MenuPlayServerInfoUI.notLoggedInWarningButton;
			sleekButton.Text += MenuPlayServerInfoUI.localization.format("NotLoggedInMessage");
			MenuPlayServerInfoUI.notLoggedInWarningButton.TextColor = 6;
			MenuPlayServerInfoUI.detailsScrollBox.AddChild(MenuPlayServerInfoUI.notLoggedInWarningButton);
			MenuPlayServerInfoUI.notLoggedInWarningButton.IsVisible = false;
			MenuPlayServerInfoUI.linksFrame = Glazier.Get().CreateFrame();
			MenuPlayServerInfoUI.linksFrame.PositionOffset_Y = 40f;
			MenuPlayServerInfoUI.linksFrame.SizeScale_X = 1f;
			MenuPlayServerInfoUI.detailsScrollBox.AddChild(MenuPlayServerInfoUI.linksFrame);
			MenuPlayServerInfoUI.serverTitle = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.serverTitle.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.serverTitle.SizeScale_X = 1f;
			MenuPlayServerInfoUI.serverTitle.Text = MenuPlayServerInfoUI.localization.format("Server");
			MenuPlayServerInfoUI.detailsScrollBox.AddChild(MenuPlayServerInfoUI.serverTitle);
			MenuPlayServerInfoUI.serverBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.serverBox.PositionOffset_Y = 40f;
			MenuPlayServerInfoUI.serverBox.SizeScale_X = 1f;
			MenuPlayServerInfoUI.serverBox.SizeOffset_Y = 130f;
			MenuPlayServerInfoUI.detailsScrollBox.AddChild(MenuPlayServerInfoUI.serverBox);
			MenuPlayServerInfoUI.serverWorkshopLabel = Glazier.Get().CreateLabel();
			MenuPlayServerInfoUI.serverWorkshopLabel.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.serverWorkshopLabel.PositionOffset_Y = 0f;
			MenuPlayServerInfoUI.serverWorkshopLabel.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.serverWorkshopLabel.SizeScale_X = 1f;
			MenuPlayServerInfoUI.serverWorkshopLabel.TextAlignment = 3;
			MenuPlayServerInfoUI.serverBox.AddChild(MenuPlayServerInfoUI.serverWorkshopLabel);
			MenuPlayServerInfoUI.serverCombatLabel = Glazier.Get().CreateLabel();
			MenuPlayServerInfoUI.serverCombatLabel.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.serverCombatLabel.PositionOffset_Y = 20f;
			MenuPlayServerInfoUI.serverCombatLabel.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.serverCombatLabel.SizeScale_X = 1f;
			MenuPlayServerInfoUI.serverCombatLabel.TextAlignment = 3;
			MenuPlayServerInfoUI.serverBox.AddChild(MenuPlayServerInfoUI.serverCombatLabel);
			MenuPlayServerInfoUI.serverPerspectiveLabel = Glazier.Get().CreateLabel();
			MenuPlayServerInfoUI.serverPerspectiveLabel.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.serverPerspectiveLabel.PositionOffset_Y = 40f;
			MenuPlayServerInfoUI.serverPerspectiveLabel.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.serverPerspectiveLabel.SizeScale_X = 1f;
			MenuPlayServerInfoUI.serverPerspectiveLabel.TextAlignment = 3;
			MenuPlayServerInfoUI.serverBox.AddChild(MenuPlayServerInfoUI.serverPerspectiveLabel);
			MenuPlayServerInfoUI.serverSecurityLabel = Glazier.Get().CreateLabel();
			MenuPlayServerInfoUI.serverSecurityLabel.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.serverSecurityLabel.PositionOffset_Y = 60f;
			MenuPlayServerInfoUI.serverSecurityLabel.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.serverSecurityLabel.SizeScale_X = 1f;
			MenuPlayServerInfoUI.serverSecurityLabel.TextAlignment = 3;
			MenuPlayServerInfoUI.serverBox.AddChild(MenuPlayServerInfoUI.serverSecurityLabel);
			MenuPlayServerInfoUI.serverModeLabel = Glazier.Get().CreateLabel();
			MenuPlayServerInfoUI.serverModeLabel.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.serverModeLabel.PositionOffset_Y = 80f;
			MenuPlayServerInfoUI.serverModeLabel.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.serverModeLabel.SizeScale_X = 1f;
			MenuPlayServerInfoUI.serverModeLabel.TextAlignment = 3;
			MenuPlayServerInfoUI.serverBox.AddChild(MenuPlayServerInfoUI.serverModeLabel);
			MenuPlayServerInfoUI.serverCheatsLabel = Glazier.Get().CreateLabel();
			MenuPlayServerInfoUI.serverCheatsLabel.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.serverCheatsLabel.PositionOffset_Y = 100f;
			MenuPlayServerInfoUI.serverCheatsLabel.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.serverCheatsLabel.SizeScale_X = 1f;
			MenuPlayServerInfoUI.serverCheatsLabel.TextAlignment = 3;
			MenuPlayServerInfoUI.serverBox.AddChild(MenuPlayServerInfoUI.serverCheatsLabel);
			MenuPlayServerInfoUI.serverMonetizationLabel = Glazier.Get().CreateLabel();
			MenuPlayServerInfoUI.serverMonetizationLabel.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.serverMonetizationLabel.PositionOffset_Y = 100f;
			MenuPlayServerInfoUI.serverMonetizationLabel.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.serverMonetizationLabel.SizeScale_X = 1f;
			MenuPlayServerInfoUI.serverMonetizationLabel.TextAlignment = 3;
			MenuPlayServerInfoUI.serverBox.AddChild(MenuPlayServerInfoUI.serverMonetizationLabel);
			MenuPlayServerInfoUI.serverPingLabel = Glazier.Get().CreateLabel();
			MenuPlayServerInfoUI.serverPingLabel.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.serverPingLabel.PositionOffset_Y = 100f;
			MenuPlayServerInfoUI.serverPingLabel.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.serverPingLabel.SizeScale_X = 1f;
			MenuPlayServerInfoUI.serverPingLabel.TextAlignment = 3;
			MenuPlayServerInfoUI.serverBox.AddChild(MenuPlayServerInfoUI.serverPingLabel);
			MenuPlayServerInfoUI.ugcTitle = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.ugcTitle.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.ugcTitle.SizeScale_X = 1f;
			MenuPlayServerInfoUI.ugcTitle.Text = MenuPlayServerInfoUI.localization.format("UGC");
			MenuPlayServerInfoUI.detailsScrollBox.AddChild(MenuPlayServerInfoUI.ugcTitle);
			MenuPlayServerInfoUI.ugcTitle.IsVisible = false;
			MenuPlayServerInfoUI.ugcBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.ugcBox.SizeScale_X = 1f;
			MenuPlayServerInfoUI.detailsScrollBox.AddChild(MenuPlayServerInfoUI.ugcBox);
			MenuPlayServerInfoUI.ugcBox.IsVisible = false;
			MenuPlayServerInfoUI.configTitle = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.configTitle.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.configTitle.SizeScale_X = 1f;
			MenuPlayServerInfoUI.configTitle.Text = MenuPlayServerInfoUI.localization.format("Config");
			MenuPlayServerInfoUI.detailsScrollBox.AddChild(MenuPlayServerInfoUI.configTitle);
			MenuPlayServerInfoUI.configTitle.IsVisible = false;
			MenuPlayServerInfoUI.configBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.configBox.SizeScale_X = 1f;
			MenuPlayServerInfoUI.detailsScrollBox.AddChild(MenuPlayServerInfoUI.configBox);
			MenuPlayServerInfoUI.configBox.IsVisible = false;
			MenuPlayServerInfoUI.rocketTitle = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.rocketTitle.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.rocketTitle.SizeScale_X = 1f;
			MenuPlayServerInfoUI.detailsScrollBox.AddChild(MenuPlayServerInfoUI.rocketTitle);
			MenuPlayServerInfoUI.rocketTitle.IsVisible = false;
			MenuPlayServerInfoUI.rocketBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.rocketBox.SizeScale_X = 1f;
			MenuPlayServerInfoUI.detailsScrollBox.AddChild(MenuPlayServerInfoUI.rocketBox);
			MenuPlayServerInfoUI.rocketBox.IsVisible = false;
			MenuPlayServerInfoUI.mapNameBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.mapNameBox.SizeOffset_X = 340f;
			MenuPlayServerInfoUI.mapNameBox.SizeOffset_Y = 30f;
			MenuPlayServerInfoUI.mapContainer.AddChild(MenuPlayServerInfoUI.mapNameBox);
			MenuPlayServerInfoUI.mapPreviewBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.mapPreviewBox.PositionOffset_Y = 40f;
			MenuPlayServerInfoUI.mapPreviewBox.SizeOffset_X = 340f;
			MenuPlayServerInfoUI.mapPreviewBox.SizeOffset_Y = 200f;
			MenuPlayServerInfoUI.mapContainer.AddChild(MenuPlayServerInfoUI.mapPreviewBox);
			MenuPlayServerInfoUI.mapPreviewImage = Glazier.Get().CreateImage();
			MenuPlayServerInfoUI.mapPreviewImage.PositionOffset_X = 10f;
			MenuPlayServerInfoUI.mapPreviewImage.PositionOffset_Y = 10f;
			MenuPlayServerInfoUI.mapPreviewImage.SizeOffset_X = -20f;
			MenuPlayServerInfoUI.mapPreviewImage.SizeOffset_Y = -20f;
			MenuPlayServerInfoUI.mapPreviewImage.SizeScale_X = 1f;
			MenuPlayServerInfoUI.mapPreviewImage.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.mapPreviewBox.AddChild(MenuPlayServerInfoUI.mapPreviewImage);
			MenuPlayServerInfoUI.mapDescriptionBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.mapDescriptionBox.PositionOffset_Y = 250f;
			MenuPlayServerInfoUI.mapDescriptionBox.SizeOffset_X = 340f;
			MenuPlayServerInfoUI.mapDescriptionBox.SizeOffset_Y = 140f;
			MenuPlayServerInfoUI.mapDescriptionBox.TextAlignment = 1;
			MenuPlayServerInfoUI.mapDescriptionBox.AllowRichText = true;
			MenuPlayServerInfoUI.mapDescriptionBox.TextColor = 4;
			MenuPlayServerInfoUI.mapContainer.AddChild(MenuPlayServerInfoUI.mapDescriptionBox);
			MenuPlayServerInfoUI.serverDescriptionBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.serverDescriptionBox.PositionOffset_Y = 400f;
			MenuPlayServerInfoUI.serverDescriptionBox.SizeOffset_X = 340f;
			MenuPlayServerInfoUI.serverDescriptionBox.SizeOffset_Y = -400f;
			MenuPlayServerInfoUI.serverDescriptionBox.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.serverDescriptionBox.TextAlignment = 1;
			MenuPlayServerInfoUI.serverDescriptionBox.AllowRichText = true;
			MenuPlayServerInfoUI.serverDescriptionBox.TextColor = 4;
			MenuPlayServerInfoUI.serverDescriptionBox.TextContrastContext = 1;
			MenuPlayServerInfoUI.mapContainer.AddChild(MenuPlayServerInfoUI.serverDescriptionBox);
			MenuPlayServerInfoUI.joinButton = Glazier.Get().CreateButton();
			MenuPlayServerInfoUI.joinButton.SizeOffset_X = -5f;
			MenuPlayServerInfoUI.joinButton.SizeScale_X = 0.2f;
			MenuPlayServerInfoUI.joinButton.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.joinButton.Text = MenuPlayServerInfoUI.localization.format("Join_Button");
			MenuPlayServerInfoUI.joinButton.TooltipText = MenuPlayServerInfoUI.localization.format("Join_Button_Tooltip");
			MenuPlayServerInfoUI.joinButton.OnClicked += new ClickedButton(MenuPlayServerInfoUI.onClickedJoinButton);
			MenuPlayServerInfoUI.joinButton.FontSize = 3;
			MenuPlayServerInfoUI.buttonsContainer.AddChild(MenuPlayServerInfoUI.joinButton);
			MenuPlayServerInfoUI.joinDisabledBox = Glazier.Get().CreateBox();
			MenuPlayServerInfoUI.joinDisabledBox.SizeOffset_X = -5f;
			MenuPlayServerInfoUI.joinDisabledBox.SizeScale_X = 0.2f;
			MenuPlayServerInfoUI.joinDisabledBox.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.joinDisabledBox.TextColor = 6;
			MenuPlayServerInfoUI.buttonsContainer.AddChild(MenuPlayServerInfoUI.joinDisabledBox);
			MenuPlayServerInfoUI.joinDisabledBox.IsVisible = false;
			MenuPlayServerInfoUI.favoriteButton = Glazier.Get().CreateButton();
			MenuPlayServerInfoUI.favoriteButton.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.favoriteButton.PositionScale_X = 0.2f;
			MenuPlayServerInfoUI.favoriteButton.SizeOffset_X = -10f;
			MenuPlayServerInfoUI.favoriteButton.SizeScale_X = 0.2f;
			MenuPlayServerInfoUI.favoriteButton.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.favoriteButton.TooltipText = MenuPlayServerInfoUI.localization.format("Favorite_Button_Tooltip");
			MenuPlayServerInfoUI.favoriteButton.OnClicked += new ClickedButton(MenuPlayServerInfoUI.onClickedFavoriteButton);
			MenuPlayServerInfoUI.favoriteButton.FontSize = 3;
			MenuPlayServerInfoUI.buttonsContainer.AddChild(MenuPlayServerInfoUI.favoriteButton);
			MenuPlayServerInfoUI.bookmarkButton = new SleekButtonIcon(null, 40);
			MenuPlayServerInfoUI.bookmarkButton.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.bookmarkButton.PositionScale_X = 0.4f;
			MenuPlayServerInfoUI.bookmarkButton.SizeOffset_X = -10f;
			MenuPlayServerInfoUI.bookmarkButton.SizeScale_X = 0.2f;
			MenuPlayServerInfoUI.bookmarkButton.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.bookmarkButton.tooltip = MenuPlayServerInfoUI.localization.format("Bookmark_Button_Tooltip");
			MenuPlayServerInfoUI.bookmarkButton.onClickedButton += new ClickedButton(MenuPlayServerInfoUI.OnClickedBookmarkButton);
			MenuPlayServerInfoUI.bookmarkButton.fontSize = 3;
			MenuPlayServerInfoUI.buttonsContainer.AddChild(MenuPlayServerInfoUI.bookmarkButton);
			MenuPlayServerInfoUI.refreshButton = Glazier.Get().CreateButton();
			MenuPlayServerInfoUI.refreshButton.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.refreshButton.PositionScale_X = 0.6f;
			MenuPlayServerInfoUI.refreshButton.SizeOffset_X = -10f;
			MenuPlayServerInfoUI.refreshButton.SizeScale_X = 0.2f;
			MenuPlayServerInfoUI.refreshButton.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.refreshButton.Text = MenuPlayServerInfoUI.localization.format("Refresh_Button");
			MenuPlayServerInfoUI.refreshButton.TooltipText = MenuPlayServerInfoUI.localization.format("Refresh_Button_Tooltip");
			MenuPlayServerInfoUI.refreshButton.OnClicked += new ClickedButton(MenuPlayServerInfoUI.onClickedRefreshButton);
			MenuPlayServerInfoUI.refreshButton.FontSize = 3;
			MenuPlayServerInfoUI.buttonsContainer.AddChild(MenuPlayServerInfoUI.refreshButton);
			MenuPlayServerInfoUI.cancelButton = Glazier.Get().CreateButton();
			MenuPlayServerInfoUI.cancelButton.PositionOffset_X = 5f;
			MenuPlayServerInfoUI.cancelButton.PositionScale_X = 0.8f;
			MenuPlayServerInfoUI.cancelButton.SizeOffset_X = -5f;
			MenuPlayServerInfoUI.cancelButton.SizeScale_X = 0.2f;
			MenuPlayServerInfoUI.cancelButton.SizeScale_Y = 1f;
			MenuPlayServerInfoUI.cancelButton.Text = MenuPlayServerInfoUI.localization.format("Cancel_Button");
			MenuPlayServerInfoUI.cancelButton.TooltipText = MenuPlayServerInfoUI.localization.format("Cancel_Button_Tooltip");
			MenuPlayServerInfoUI.cancelButton.OnClicked += new ClickedButton(MenuPlayServerInfoUI.onClickedCancelButton);
			MenuPlayServerInfoUI.cancelButton.FontSize = 3;
			MenuPlayServerInfoUI.buttonsContainer.AddChild(MenuPlayServerInfoUI.cancelButton);
			TempSteamworksMatchmaking matchmakingService = Provider.provider.matchmakingService;
			matchmakingService.onMasterServerQueryRefreshed = (TempSteamworksMatchmaking.MasterServerQueryRefreshed)Delegate.Combine(matchmakingService.onMasterServerQueryRefreshed, new TempSteamworksMatchmaking.MasterServerQueryRefreshed(MenuPlayServerInfoUI.onMasterServerQueryRefreshed));
			TempSteamworksMatchmaking matchmakingService2 = Provider.provider.matchmakingService;
			matchmakingService2.onPlayersQueryRefreshed = (TempSteamworksMatchmaking.PlayersQueryRefreshed)Delegate.Combine(matchmakingService2.onPlayersQueryRefreshed, new TempSteamworksMatchmaking.PlayersQueryRefreshed(MenuPlayServerInfoUI.onPlayersQueryRefreshed));
			TempSteamworksMatchmaking matchmakingService3 = Provider.provider.matchmakingService;
			matchmakingService3.onRulesQueryRefreshed = (TempSteamworksMatchmaking.RulesQueryRefreshed)Delegate.Combine(matchmakingService3.onRulesQueryRefreshed, new TempSteamworksMatchmaking.RulesQueryRefreshed(MenuPlayServerInfoUI.onRulesQueryRefreshed));
			if (MenuPlayServerInfoUI.ugcQueryCompleted == null)
			{
				MenuPlayServerInfoUI.ugcQueryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(MenuPlayServerInfoUI.onUGCQueryCompleted));
			}
			MenuPlayServerInfoUI.passwordUI = new MenuServerPasswordUI();
		}

		// Token: 0x040029C2 RID: 10690
		internal static Local localization;

		// Token: 0x040029C3 RID: 10691
		private static SleekFullscreenBox container;

		// Token: 0x040029C4 RID: 10692
		public static bool active;

		// Token: 0x040029C5 RID: 10693
		private static ISleekElement infoContainer;

		// Token: 0x040029C6 RID: 10694
		private static ISleekElement playersContainer;

		// Token: 0x040029C7 RID: 10695
		private static ISleekElement detailsContainer;

		// Token: 0x040029C8 RID: 10696
		private static ISleekElement mapContainer;

		// Token: 0x040029C9 RID: 10697
		private static ISleekElement buttonsContainer;

		// Token: 0x040029CA RID: 10698
		private static ISleekBox titleBox;

		// Token: 0x040029CB RID: 10699
		private static SleekWebImage titleIconImage;

		// Token: 0x040029CC RID: 10700
		private static ISleekLabel titleNameLabel;

		// Token: 0x040029CD RID: 10701
		private static ISleekLabel titleDescriptionLabel;

		// Token: 0x040029CE RID: 10702
		private static ISleekBox playerCountBox;

		// Token: 0x040029CF RID: 10703
		private static ISleekScrollView playersScrollBox;

		// Token: 0x040029D0 RID: 10704
		private static ISleekBox detailsBox;

		// Token: 0x040029D1 RID: 10705
		private static ISleekScrollView detailsScrollBox;

		// Token: 0x040029D2 RID: 10706
		private static ISleekButton hostBanWarningButton;

		// Token: 0x040029D3 RID: 10707
		private static ISleekButton notLoggedInWarningButton;

		// Token: 0x040029D4 RID: 10708
		private static ISleekElement linksFrame;

		// Token: 0x040029D5 RID: 10709
		private static ISleekBox serverTitle;

		// Token: 0x040029D6 RID: 10710
		private static ISleekBox serverBox;

		// Token: 0x040029D7 RID: 10711
		private static ISleekLabel serverWorkshopLabel;

		// Token: 0x040029D8 RID: 10712
		private static ISleekLabel serverCombatLabel;

		// Token: 0x040029D9 RID: 10713
		private static ISleekLabel serverPerspectiveLabel;

		// Token: 0x040029DA RID: 10714
		private static ISleekLabel serverSecurityLabel;

		// Token: 0x040029DB RID: 10715
		private static ISleekLabel serverModeLabel;

		// Token: 0x040029DC RID: 10716
		private static ISleekLabel serverCheatsLabel;

		// Token: 0x040029DD RID: 10717
		private static ISleekLabel serverMonetizationLabel;

		// Token: 0x040029DE RID: 10718
		private static ISleekLabel serverPingLabel;

		// Token: 0x040029DF RID: 10719
		private static ISleekBox ugcTitle;

		// Token: 0x040029E0 RID: 10720
		private static ISleekBox ugcBox;

		// Token: 0x040029E1 RID: 10721
		private static ISleekBox configTitle;

		// Token: 0x040029E2 RID: 10722
		private static ISleekBox configBox;

		// Token: 0x040029E3 RID: 10723
		private static ISleekBox rocketTitle;

		// Token: 0x040029E4 RID: 10724
		private static ISleekBox rocketBox;

		// Token: 0x040029E5 RID: 10725
		private static ISleekBox mapNameBox;

		// Token: 0x040029E6 RID: 10726
		private static ISleekBox mapPreviewBox;

		// Token: 0x040029E7 RID: 10727
		private static ISleekImage mapPreviewImage;

		// Token: 0x040029E8 RID: 10728
		private static ISleekBox mapDescriptionBox;

		// Token: 0x040029E9 RID: 10729
		private static ISleekBox serverDescriptionBox;

		// Token: 0x040029EA RID: 10730
		private static ISleekButton joinButton;

		// Token: 0x040029EB RID: 10731
		private static ISleekBox joinDisabledBox;

		// Token: 0x040029EC RID: 10732
		private static ISleekButton favoriteButton;

		// Token: 0x040029ED RID: 10733
		private static SleekButtonIcon bookmarkButton;

		// Token: 0x040029EE RID: 10734
		private static ISleekButton refreshButton;

		// Token: 0x040029EF RID: 10735
		private static ISleekButton cancelButton;

		// Token: 0x040029F0 RID: 10736
		private static SteamServerAdvertisement serverInfo;

		// Token: 0x040029F1 RID: 10737
		private static string serverPassword;

		// Token: 0x040029F2 RID: 10738
		private static bool serverFavorited;

		/// <summary>
		/// DNS entry to use if adding a bookmark for this server.
		/// </summary>
		// Token: 0x040029F3 RID: 10739
		private static string serverBookmarkHost;

		// Token: 0x040029F4 RID: 10740
		private static List<PublishedFileId_t> expectedWorkshopItems;

		// Token: 0x040029F5 RID: 10741
		private static List<string> linkUrls;

		// Token: 0x040029F6 RID: 10742
		private static int playersOffset;

		// Token: 0x040029F7 RID: 10743
		private static int playerCount;

		// Token: 0x040029F8 RID: 10744
		private static UGCQueryHandle_t detailsHandle;

		// Token: 0x040029F9 RID: 10745
		private static CallResult<SteamUGCQueryCompleted_t> ugcQueryCompleted;

		// Token: 0x040029FB RID: 10747
		private static MenuServerPasswordUI passwordUI;

		// Token: 0x02000A06 RID: 2566
		public enum EServerInfoOpenContext
		{
			// Token: 0x040034FD RID: 13565
			CONNECT,
			// Token: 0x040034FE RID: 13566
			SERVERS,
			// Token: 0x040034FF RID: 13567
			BOOKMARKS
		}

		// Token: 0x02000A07 RID: 2567
		private class ServerInfoViewWorkshopButton : SleekWrapper
		{
			// Token: 0x06004D49 RID: 19785 RVA: 0x001B9320 File Offset: 0x001B7520
			public ServerInfoViewWorkshopButton(PublishedFileId_t fileId, string name)
			{
				this.fileId = fileId;
				base.SizeOffset_X = 20f;
				base.SizeOffset_Y = 20f;
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.SizeScale_X = 1f;
				sleekButton.SizeScale_Y = 1f;
				sleekButton.OnClicked += new ClickedButton(this.onClickedButton);
				sleekButton.TooltipText = MenuWorkshopSubscriptionsUI.localization.format("View_Tooltip", name);
				base.AddChild(sleekButton);
				ISleekSprite sleekSprite = Glazier.Get().CreateSprite();
				sleekSprite.PositionOffset_X = 5f;
				sleekSprite.PositionOffset_Y = 5f;
				sleekSprite.SizeOffset_X = 10f;
				sleekSprite.SizeOffset_Y = 10f;
				sleekSprite.Sprite = MenuDashboardUI.icons.load<Sprite>("External_Link_Sprite");
				sleekSprite.DrawMethod = 2;
				sleekButton.AddChild(sleekSprite);
			}

			// Token: 0x06004D4A RID: 19786 RVA: 0x001B93FC File Offset: 0x001B75FC
			private void onClickedButton(ISleekElement button)
			{
				string text = "http://steamcommunity.com/sharedfiles/filedetails/?id=";
				PublishedFileId_t publishedFileId_t = this.fileId;
				string url = text + publishedFileId_t.ToString();
				Provider.provider.browserService.open(url);
			}

			// Token: 0x04003500 RID: 13568
			public PublishedFileId_t fileId;
		}
	}
}
