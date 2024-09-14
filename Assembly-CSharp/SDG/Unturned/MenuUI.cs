using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;

namespace SDG.Unturned
{
	// Token: 0x020005BE RID: 1470
	public class MenuUI : MonoBehaviour
	{
		/// <summary>
		/// Remove any existing item alert widgets.
		/// </summary>
		// Token: 0x06002FB3 RID: 12211 RVA: 0x000D2948 File Offset: 0x000D0B48
		private static void removeItemAlerts()
		{
			if (MenuUI.itemAlerts == null)
			{
				return;
			}
			foreach (SleekInventory sleekInventory in MenuUI.itemAlerts)
			{
				MenuUI.alertBox.RemoveChild(sleekInventory);
			}
			MenuUI.itemAlerts = null;
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x000D2988 File Offset: 0x000D0B88
		private static void alertText()
		{
			if (MenuUI.alertBox == null || MenuUI.originLabel == null)
			{
				return;
			}
			MenuUI.alertBox.PositionOffset_Y = -50f;
			MenuUI.alertBox.SizeOffset_Y = 100f;
			MenuUI.copyNotificationButton.IsVisible = true;
			MenuUI.originLabel.IsVisible = false;
			MenuUI.removeItemAlerts();
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x000D29E0 File Offset: 0x000D0BE0
		private static void alertItem()
		{
			if (MenuUI.alertBox == null || MenuUI.originLabel == null)
			{
				return;
			}
			MenuUI.alertBox.Text = "";
			MenuUI.alertBox.PositionOffset_Y = -150f;
			MenuUI.alertBox.SizeOffset_Y = 300f;
			MenuUI.copyNotificationButton.IsVisible = false;
			MenuUI.originLabel.IsVisible = true;
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x000D2A3F File Offset: 0x000D0C3F
		private static void internalOpenAlert()
		{
			if (MenuUI.alertBox != null)
			{
				MenuUI.alertBox.AnimatePositionScale(0f, 0.5f, 1, 20f);
			}
			if (MenuUI.container != null)
			{
				MenuUI.container.AnimateOutOfView(-1f, 0f);
			}
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x000D2A80 File Offset: 0x000D0C80
		private static void updateDismissButton(bool canBeDismissed)
		{
			if (MenuUI.dismissNotificationButton != null)
			{
				if (Provider.provider.matchmakingService.isAttemptingServerQuery)
				{
					MenuUI.dismissNotificationButton.Text = MenuPlayConnectUI.localization.format("Cancel_Attempt_Label");
					MenuUI.dismissNotificationButton.TooltipText = MenuPlayConnectUI.localization.format("Cancel_Attempt_Tooltip");
				}
				else
				{
					MenuUI.dismissNotificationButton.Text = MenuDashboardUI.localization.format("Dismiss_Notification_Label");
					MenuUI.dismissNotificationButton.TooltipText = MenuDashboardUI.localization.format("Dismiss_Notification_Tooltip");
				}
				MenuUI.dismissNotificationButton.IsVisible = canBeDismissed;
			}
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x000D2B19 File Offset: 0x000D0D19
		public static void openAlert(string message, bool canBeDismissed = true)
		{
			MenuUI.alertText();
			if (MenuUI.alertBox != null)
			{
				MenuUI.alertBox.Text = message;
			}
			MenuUI.updateDismissButton(canBeDismissed);
			MenuUI.internalOpenAlert();
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x000D2B3D File Offset: 0x000D0D3D
		public static void closeAlert()
		{
			MenuUI.removeItemAlerts();
			if (MenuUI.alertBox != null)
			{
				MenuUI.alertBox.AnimatePositionScale(1f, 0.5f, 1, 20f);
			}
			if (MenuUI.container != null)
			{
				MenuUI.container.AnimateIntoView();
			}
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x000D2B76 File Offset: 0x000D0D76
		public static void alert(string message)
		{
			MenuUI.openAlert(message, true);
			MenuUI.isAlerting = true;
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x000D2B88 File Offset: 0x000D0D88
		public static void alert(string origin, ulong instanceId, int itemDefId, ushort quantity)
		{
			SteamItemDetails_t steamItemDetails_t = default(SteamItemDetails_t);
			steamItemDetails_t.m_itemId.m_SteamItemInstanceID = instanceId;
			steamItemDetails_t.m_iDefinition.m_SteamItemDef = itemDefId;
			steamItemDetails_t.m_unQuantity = quantity;
			List<SteamItemDetails_t> list = new List<SteamItemDetails_t>();
			list.Add(steamItemDetails_t);
			MenuUI.alertNewItems(origin, list);
		}

		/// <summary>
		/// Open fullscreen alert showcasing newly granted items.
		/// Uses first item for title color, so items should be sorted by priority.
		/// </summary>
		// Token: 0x06002FBC RID: 12220 RVA: 0x000D2BD4 File Offset: 0x000D0DD4
		public static void alertNewItems(string origin, List<SteamItemDetails_t> grantedItems)
		{
			if (MenuUI.originLabel != null)
			{
				MenuUI.originLabel.Text = origin;
				MenuUI.originLabel.TextColor = Provider.provider.economyService.getInventoryColor(grantedItems[0].m_iDefinition.m_SteamItemDef);
			}
			MenuUI.removeItemAlerts();
			MenuUI.itemAlerts = new SleekInventory[grantedItems.Count];
			int num = -100;
			for (int i = 0; i < grantedItems.Count; i++)
			{
				SteamItemDetails_t steamItemDetails_t = grantedItems[i];
				bool flag = i == 0;
				int num2 = flag ? 200 : 100;
				SleekInventory sleekInventory = new SleekInventory();
				sleekInventory.PositionOffset_X = (float)num;
				sleekInventory.PositionOffset_Y = (float)(flag ? 75 : 125);
				sleekInventory.PositionScale_X = 0.5f;
				sleekInventory.SizeOffset_X = (float)num2;
				sleekInventory.SizeOffset_Y = (float)num2;
				MenuUI.alertBox.AddChild(sleekInventory);
				sleekInventory.updateInventory(steamItemDetails_t.m_itemId.m_SteamItemInstanceID, steamItemDetails_t.m_iDefinition.m_SteamItemDef, steamItemDetails_t.m_unQuantity, false, flag);
				MenuUI.itemAlerts[i] = sleekInventory;
				num += num2 + 5;
			}
			MenuUI.alertItem();
			MenuUI.updateDismissButton(true);
			MenuUI.internalOpenAlert();
			MenuUI.isAlerting = true;
		}

		/// <summary>
		/// Open fullscreen alert showcasing newly granted items.
		/// </summary>
		// Token: 0x06002FBD RID: 12221 RVA: 0x000D2D04 File Offset: 0x000D0F04
		public static void alertPurchasedItems(string origin, List<SteamItemDetails_t> grantedItems)
		{
			if (MenuUI.originLabel != null)
			{
				MenuUI.originLabel.Text = origin;
				MenuUI.originLabel.TextColor = ItemStore.PremiumColor;
			}
			MenuUI.removeItemAlerts();
			MenuUI.itemAlerts = new SleekInventory[grantedItems.Count];
			int num = grantedItems.Count * -100;
			for (int i = 0; i < grantedItems.Count; i++)
			{
				SteamItemDetails_t steamItemDetails_t = grantedItems[i];
				SleekInventory sleekInventory = new SleekInventory();
				sleekInventory.PositionOffset_X = (float)num;
				sleekInventory.PositionOffset_Y = 75f;
				sleekInventory.PositionScale_X = 0.5f;
				sleekInventory.SizeOffset_X = 200f;
				sleekInventory.SizeOffset_Y = 200f;
				MenuUI.alertBox.AddChild(sleekInventory);
				sleekInventory.updateInventory(steamItemDetails_t.m_itemId.m_SteamItemInstanceID, steamItemDetails_t.m_iDefinition.m_SteamItemDef, steamItemDetails_t.m_unQuantity, false, true);
				MenuUI.itemAlerts[i] = sleekInventory;
				num += 200;
			}
			MenuUI.alertItem();
			MenuUI.updateDismissButton(true);
			MenuUI.internalOpenAlert();
			MenuUI.isAlerting = true;
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x000D2E04 File Offset: 0x000D1004
		private static void onClickedDismissNotification(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.isAttemptingServerQuery)
			{
				Provider.provider.matchmakingService.cancel();
				MenuUI.closeAlert();
				MenuUI.isAlerting = false;
				return;
			}
			if (MenuSurvivorsClothingUI.isCrafting)
			{
				return;
			}
			MenuUI.closeAlert();
			MenuUI.isAlerting = false;
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x000D2E50 File Offset: 0x000D1050
		private static void OnClickedCopyNotification(ISleekElement button)
		{
			GUIUtility.systemCopyBuffer = MenuUI.alertBox.Text;
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x000D2E64 File Offset: 0x000D1064
		public static void closeAll()
		{
			MenuPauseUI.close();
			MenuCreditsUI.close();
			MenuTitleUI.close();
			MenuDashboardUI.close();
			MenuPlayUI.close();
			MenuPlaySingleplayerUI.close();
			MenuPlayLobbiesUI.close();
			MenuPlayConnectUI.close();
			MenuPlayServersUI.serverListFiltersUI.close();
			MenuPlayServersUI.mapFiltersUI.close();
			MenuPlayUI.serverListUI.close();
			MenuPlayUI.serverBookmarksUI.close();
			MenuPlayUI.onlineSafetyUI.close();
			MenuPlayServerInfoUI.close();
			MenuServerPasswordUI.close();
			MenuPlayConfigUI.close();
			MenuSurvivorsUI.close();
			ItemStoreDetailsMenu.instance.Close();
			ItemStoreCartMenu.instance.Close();
			ItemStoreMenu.instance.Close();
			MenuSurvivorsCharacterUI.close();
			MenuSurvivorsAppearanceUI.close();
			MenuSurvivorsClothingUI.close();
			MenuSurvivorsGroupUI.close();
			MenuSurvivorsClothingBoxUI.close();
			MenuSurvivorsClothingDeleteUI.close();
			MenuSurvivorsClothingInspectUI.close();
			MenuSurvivorsClothingItemUI.close();
			MenuConfigurationUI.close();
			MenuConfigurationOptionsUI.close();
			MenuConfigurationDisplayUI.close();
			MenuConfigurationGraphicsUI.close();
			MenuConfigurationControlsUI.close();
			MenuConfigurationUI.audioMenu.close();
			MenuWorkshopUI.close();
			MenuWorkshopEditorUI.close();
			MenuWorkshopSubmitUI.close();
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x000D2F57 File Offset: 0x000D1157
		private void OnEnable()
		{
			MenuUI.instance = this;
			base.useGUILayout = false;
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x000D2F66 File Offset: 0x000D1166
		internal void Menu_OnGUI()
		{
			if (MenuUI.window != null)
			{
				Glazier.Get().Root = MenuUI.window;
			}
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x000D2F7E File Offset: 0x000D117E
		private void OnGUI()
		{
			MenuConfigurationControlsUI.bindOnGUI();
		}

		/// <summary>
		/// Handle esc/back key press.
		/// Still really messy, but this used to be inside a huge nested if/elseif in Update.
		/// </summary>
		// Token: 0x06002FC4 RID: 12228 RVA: 0x000D2F88 File Offset: 0x000D1188
		private void escapeMenu()
		{
			if (Provider.provider.matchmakingService.isAttemptingServerQuery)
			{
				Provider.provider.matchmakingService.cancel();
				MenuUI.closeAlert();
				MenuUI.isAlerting = false;
				return;
			}
			if (MenuSurvivorsClothingUI.isCrafting)
			{
				return;
			}
			if (MenuUI.isAlerting)
			{
				MenuUI.closeAlert();
				MenuUI.isAlerting = false;
				return;
			}
			if (MenuPauseUI.active)
			{
				MenuPauseUI.close();
				MenuDashboardUI.open();
				MenuTitleUI.open();
				return;
			}
			if (MenuCreditsUI.active)
			{
				MenuCreditsUI.close();
				MenuPauseUI.open();
				return;
			}
			if (MenuTitleUI.active)
			{
				MenuPauseUI.open();
				MenuDashboardUI.close();
				MenuTitleUI.close();
				return;
			}
			if (MenuPlayConfigUI.active)
			{
				MenuPlayConfigUI.close();
				MenuPlaySingleplayerUI.open();
				return;
			}
			if (MenuServerPasswordUI.isActive)
			{
				MenuServerPasswordUI.close();
				MenuPlayServerInfoUI.OpenWithoutRefresh();
				return;
			}
			if (MenuPlayServerInfoUI.active)
			{
				MenuPlayServerInfoUI.close();
				switch (MenuPlayServerInfoUI.openContext)
				{
				case MenuPlayServerInfoUI.EServerInfoOpenContext.CONNECT:
					MenuPlayConnectUI.open();
					return;
				case MenuPlayServerInfoUI.EServerInfoOpenContext.SERVERS:
					MenuPlayUI.serverListUI.open(false);
					return;
				case MenuPlayServerInfoUI.EServerInfoOpenContext.BOOKMARKS:
					MenuPlayUI.serverBookmarksUI.open();
					return;
				default:
					UnturnedLog.info("Unknown server info open context: {0}", new object[]
					{
						MenuPlayServerInfoUI.openContext
					});
					return;
				}
			}
			else
			{
				if (MenuPlayServersUI.mapFiltersUI.active)
				{
					MenuPlayServersUI.mapFiltersUI.close();
					MenuPlayServersUI.mapFiltersUI.OpenPreviousMenu();
					return;
				}
				if (MenuPlayServersUI.serverListFiltersUI.active)
				{
					MenuPlayServersUI.serverListFiltersUI.close();
					MenuPlayUI.serverListUI.open(true);
					return;
				}
				if (MenuPlayConnectUI.active || MenuPlayUI.serverListUI.active || MenuPlaySingleplayerUI.active || MenuPlayLobbiesUI.active || MenuPlayUI.serverBookmarksUI.active || MenuPlayUI.onlineSafetyUI.active)
				{
					MenuPlayConnectUI.close();
					MenuPlayUI.serverListUI.close();
					MenuPlaySingleplayerUI.close();
					MenuPlayLobbiesUI.close();
					MenuPlayUI.serverBookmarksUI.close();
					MenuPlayUI.onlineSafetyUI.close();
					MenuPlayUI.open();
					return;
				}
				if (ItemStoreCartMenu.instance.IsOpen)
				{
					ItemStoreCartMenu.instance.Close();
					ItemStoreMenu.instance.Open();
					return;
				}
				if (ItemStoreDetailsMenu.instance.IsOpen)
				{
					ItemStoreDetailsMenu.instance.Close();
					ItemStoreMenu.instance.Open();
					return;
				}
				if (ItemStoreMenu.instance.IsOpen)
				{
					ItemStoreMenu.instance.Close();
					MenuSurvivorsClothingUI.open();
					return;
				}
				if (MenuSurvivorsClothingItemUI.active)
				{
					MenuSurvivorsClothingItemUI.close();
					MenuSurvivorsClothingUI.open();
					return;
				}
				if (MenuSurvivorsClothingBoxUI.active)
				{
					if (MenuSurvivorsClothingBoxUI.isUnboxing)
					{
						MenuSurvivorsClothingBoxUI.skipAnimation();
						return;
					}
					MenuSurvivorsClothingBoxUI.close();
					MenuSurvivorsClothingItemUI.open();
					return;
				}
				else
				{
					if (MenuSurvivorsClothingInspectUI.active || MenuSurvivorsClothingDeleteUI.active)
					{
						MenuSurvivorsClothingInspectUI.close();
						MenuSurvivorsClothingDeleteUI.close();
						MenuSurvivorsClothingItemUI.open();
						return;
					}
					if (MenuSurvivorsCharacterUI.active || MenuSurvivorsAppearanceUI.active || MenuSurvivorsGroupUI.active || MenuSurvivorsClothingUI.active)
					{
						MenuSurvivorsCharacterUI.close();
						MenuSurvivorsAppearanceUI.close();
						MenuSurvivorsGroupUI.close();
						MenuSurvivorsClothingUI.close();
						MenuSurvivorsUI.open();
						return;
					}
					if (MenuConfigurationOptionsUI.active || MenuConfigurationControlsUI.active || MenuConfigurationGraphicsUI.active || MenuConfigurationDisplayUI.active || MenuConfigurationUI.audioMenu.active)
					{
						MenuConfigurationOptionsUI.close();
						MenuConfigurationControlsUI.close();
						MenuConfigurationGraphicsUI.close();
						MenuConfigurationDisplayUI.close();
						MenuConfigurationUI.audioMenu.close();
						MenuConfigurationUI.open();
						return;
					}
					if (MenuWorkshopSubmitUI.active || MenuWorkshopEditorUI.active || MenuWorkshopErrorUI.active || MenuWorkshopLocalizationUI.active || MenuWorkshopSpawnsUI.active || MenuWorkshopSubscriptionsUI.active)
					{
						MenuWorkshopSubmitUI.close();
						MenuWorkshopEditorUI.close();
						MenuWorkshopErrorUI.close();
						MenuWorkshopLocalizationUI.close();
						MenuWorkshopSpawnsUI.close();
						MenuWorkshopSubscriptionsUI.instance.close();
						MenuWorkshopUI.open();
						return;
					}
					MenuPlayUI.close();
					MenuSurvivorsUI.close();
					MenuConfigurationUI.close();
					MenuWorkshopUI.close();
					MenuDashboardUI.open();
					MenuTitleUI.open();
					return;
				}
			}
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x000D32F0 File Offset: 0x000D14F0
		private void tickInput()
		{
			if (MenuConfigurationControlsUI.binding != 255)
			{
				return;
			}
			if (InputEx.GetKeyDown(KeyCode.F1))
			{
				MenuWorkshopUI.toggleIconTools();
			}
			if (InputEx.ConsumeKeyDown(KeyCode.Escape))
			{
				this.escapeMenu();
			}
			if (MenuUI.window != null)
			{
				if (InputEx.GetKeyDown(ControlsSettings.screenshot))
				{
					Provider.RequestScreenshot();
				}
				if (InputEx.GetKeyDown(ControlsSettings.hud))
				{
					MenuUI.window.isEnabled = !MenuUI.window.isEnabled;
					MenuUI.window.drawCursorWhileDisabled = false;
				}
				InputEx.GetKeyDown(ControlsSettings.terminal);
			}
			if (InputEx.GetKeyDown(ControlsSettings.refreshAssets))
			{
				Assets.RequestReloadAllAssets();
			}
			if (InputEx.GetKeyDown(ControlsSettings.clipboardDebug))
			{
				if (MenuSurvivorsAppearanceUI.active)
				{
					string text = string.Empty;
					text = text + "Face " + Characters.active.face.ToString();
					text = text + "\nHair " + Characters.active.hair.ToString();
					text = text + "\nBeard " + Characters.active.beard.ToString();
					text = text + "\nColor_Skin " + Palette.hex(Characters.active.skin);
					text = text + "\nColor_Hair " + Palette.hex(Characters.active.color);
					if (Characters.active.hand)
					{
						text += "\nBackward";
					}
					GUIUtility.systemCopyBuffer = text;
					return;
				}
				if (MenuPlayServerInfoUI.active)
				{
					GUIUtility.systemCopyBuffer = MenuPlayServerInfoUI.GetClipboardData();
				}
			}
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x000D346C File Offset: 0x000D166C
		private void Update()
		{
			if (MenuUI.window == null)
			{
				return;
			}
			MenuConfigurationControlsUI.bindUpdate();
			MenuSurvivorsClothingBoxUI.update();
			this.tickInput();
			MenuUI.window.showCursor = true;
			if (MenuPlayUI.active || MenuPlayConnectUI.active || MenuPlayUI.serverListUI.active || MenuPlayServersUI.serverListFiltersUI.active || MenuPlayServersUI.mapFiltersUI.active || MenuPlayServerInfoUI.active || MenuServerPasswordUI.isActive || MenuPlaySingleplayerUI.active || MenuPlayLobbiesUI.active || MenuPlayConfigUI.active || MenuPlayUI.serverBookmarksUI.active || MenuPlayUI.onlineSafetyUI.active)
			{
				this.target = this.play;
			}
			else if (MenuSurvivorsUI.active || MenuSurvivorsCharacterUI.active || MenuSurvivorsAppearanceUI.active || MenuSurvivorsGroupUI.active || MenuSurvivorsClothingUI.active || MenuSurvivorsClothingItemUI.active || MenuSurvivorsClothingInspectUI.active || MenuSurvivorsClothingDeleteUI.active || MenuSurvivorsClothingBoxUI.active || ItemStoreMenu.instance.IsOpen || ItemStoreCartMenu.instance.IsOpen || ItemStoreDetailsMenu.instance.IsOpen)
			{
				this.target = this.survivors;
			}
			else if (MenuConfigurationUI.active || MenuConfigurationOptionsUI.active || MenuConfigurationControlsUI.active || MenuConfigurationGraphicsUI.active || MenuConfigurationDisplayUI.active || MenuConfigurationUI.audioMenu.active)
			{
				this.target = this.configuration;
			}
			else if (MenuWorkshopUI.active || MenuWorkshopSubmitUI.active || MenuWorkshopEditorUI.active || MenuWorkshopErrorUI.active || MenuWorkshopLocalizationUI.active || MenuWorkshopSpawnsUI.active || MenuWorkshopSubscriptionsUI.active)
			{
				this.target = this.workshop;
			}
			else
			{
				this.target = this.title;
			}
			if (!(this.target == this.title))
			{
				MenuUI.hasTitled = true;
				base.transform.position = Vector3.Lerp(base.transform.position, this.target.position, Time.deltaTime * 4f);
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.target.rotation, Time.deltaTime * 4f);
				return;
			}
			if (MenuUI.hasTitled)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, this.target.position, Time.deltaTime * 4f);
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.target.rotation, Time.deltaTime * 4f);
				return;
			}
			base.transform.position = Vector3.Lerp(base.transform.position, this.target.position, Time.deltaTime);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.target.rotation, Time.deltaTime);
		}

		/// <summary>
		/// Despite being newer code, this is obviously not ideal. Previously the news request was using the Steam HTTP
		/// API which might have been the cause of some crashes, so it was quickly converted to Unity web request instead.
		/// </summary>
		// Token: 0x06002FC7 RID: 12231 RVA: 0x000D3755 File Offset: 0x000D1955
		internal IEnumerator requestSteamNews()
		{
			int num = Provider.statusData.News.Announcements_Count;
			if (num < 1)
			{
				UnturnedLog.warn("Not requesting Steam community announcements because count is zero");
				yield break;
			}
			if (num > 10)
			{
				num = 10;
				UnturnedLog.warn("Clamping Steam community announcements to {0}", new object[]
				{
					num
				});
			}
			if (!Provider.allowWebRequests)
			{
				UnturnedLog.warn("Not requesting Steam community announcements because web requests are disabled");
				yield break;
			}
			string text = "http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002?appid=304930&count={0}&feeds=steam_community_announcements";
			text = string.Format(text, num.ToString("D"));
			using (UnityWebRequest request = UnityWebRequest.Get(text))
			{
				request.timeout = 15;
				UnturnedLog.info("Requesting {0} Steam community announcements", new object[]
				{
					num
				});
				yield return request.SendWebRequest();
				if (request.result != 1)
				{
					UnturnedLog.warn("Error requesting news: {0}", new object[]
					{
						request.error
					});
				}
				else
				{
					try
					{
						UnturnedLog.info("Received Steam community announcements");
						MenuDashboardUI.receiveSteamNews(request.downloadHandler.text);
					}
					catch (Exception e)
					{
						UnturnedLog.exception(e, "News web query handled improperly!");
					}
				}
			}
			UnityWebRequest request = null;
			yield break;
			yield break;
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000D375D File Offset: 0x000D195D
		internal IEnumerator CheckForUpdates(Action<string, bool> callback)
		{
			if (Application.isEditor)
			{
				yield break;
			}
			if (!Provider.allowWebRequests)
			{
				UnturnedLog.warn("Not checking for updates because web requests are disabled");
				yield break;
			}
			string text;
			if (!SteamApps.GetCurrentBetaName(out text, 64) || string.IsNullOrWhiteSpace(text))
			{
				UnturnedLog.warn("Unable to get current Steam beta name, defaulting to \"public\"");
				text = "public";
			}
			UnturnedLog.info("Checking for updates on Steam beta branch \"" + text + "\"...");
			string text2 = "https://smartlydressedgames.com/unturned-steam-versions/" + text + ".txt";
			using (UnityWebRequest request = UnityWebRequest.Get(text2))
			{
				request.timeout = 30;
				yield return request.SendWebRequest();
				if (request.result == 1)
				{
					string text3 = request.downloadHandler.text;
					uint num;
					if (Parser.TryGetUInt32FromIP(text3, out num))
					{
						if (num != Provider.APP_VERSION_PACKED)
						{
							if (num > Provider.APP_VERSION_PACKED)
							{
								UnturnedLog.info("Detected newer game version: " + text3);
							}
							else
							{
								UnturnedLog.info("Detected rollback to older game version: " + text3);
							}
							bool flag = num < Provider.APP_VERSION_PACKED;
							callback.Invoke(text3, flag);
						}
						else
						{
							UnturnedLog.info("Game version is up-to-date");
						}
					}
					else
					{
						UnturnedLog.info("Unable to parse newest game version \"" + text3 + "\"");
					}
				}
				else
				{
					UnturnedLog.warn("Network error checking for updates: \"" + request.error + "\"");
				}
			}
			UnityWebRequest request = null;
			yield break;
			yield break;
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000D376C File Offset: 0x000D196C
		internal void customStart()
		{
			Time.timeScale = 1f;
			AudioListener.pause = false;
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x000D377E File Offset: 0x000D197E
		private void OnDestroy()
		{
			if (MenuUI.window == null)
			{
				return;
			}
			if (this.dashboard != null)
			{
				this.dashboard.OnDestroy();
			}
			if (!Provider.isApplicationQuitting)
			{
				MenuUI.window.InternalDestroy();
			}
			MenuUI.window = null;
		}

		// Token: 0x040019B6 RID: 6582
		public static SleekWindow window;

		// Token: 0x040019B7 RID: 6583
		public static SleekFullscreenBox container;

		// Token: 0x040019B8 RID: 6584
		private static ISleekBox alertBox;

		// Token: 0x040019B9 RID: 6585
		private static ISleekLabel originLabel;

		// Token: 0x040019BA RID: 6586
		private static ISleekButton dismissNotificationButton;

		// Token: 0x040019BB RID: 6587
		internal static SleekButtonIcon copyNotificationButton;

		// Token: 0x040019BC RID: 6588
		private static SleekInventory[] itemAlerts;

		// Token: 0x040019BD RID: 6589
		private static bool isAlerting;

		// Token: 0x040019BE RID: 6590
		private Transform title;

		// Token: 0x040019BF RID: 6591
		private Transform play;

		// Token: 0x040019C0 RID: 6592
		private Transform survivors;

		// Token: 0x040019C1 RID: 6593
		private Transform configuration;

		// Token: 0x040019C2 RID: 6594
		private Transform workshop;

		// Token: 0x040019C3 RID: 6595
		private Transform target;

		// Token: 0x040019C4 RID: 6596
		private static bool hasPanned;

		// Token: 0x040019C5 RID: 6597
		private static bool hasTitled;

		// Token: 0x040019C6 RID: 6598
		internal static MenuUI instance;

		// Token: 0x040019C7 RID: 6599
		private MenuDashboardUI dashboard;
	}
}
