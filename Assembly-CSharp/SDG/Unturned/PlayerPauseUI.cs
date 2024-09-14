using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007CE RID: 1998
	public class PlayerPauseUI
	{
		// Token: 0x060043ED RID: 17389 RVA: 0x00184C88 File Offset: 0x00182E88
		public static void open()
		{
			if (PlayerPauseUI.active)
			{
				return;
			}
			PlayerPauseUI.active = true;
			PlayerPauseUI.lastLeave = Time.realtimeSinceStartup;
			if (Level.info != null)
			{
				string localizedName = Level.info.getLocalizedName();
				string text;
				if (Provider.isServer)
				{
					text = PlayerPauseUI.localization.format("Offline");
				}
				else
				{
					if (Provider.IsVacActiveOnCurrentServer)
					{
						text = PlayerPauseUI.localization.format("VAC_Secure");
					}
					else
					{
						text = PlayerPauseUI.localization.format("VAC_Insecure");
					}
					if (Provider.IsBattlEyeActiveOnCurrentServer)
					{
						text = text + " + " + PlayerPauseUI.localization.format("BattlEye_Secure");
					}
					else
					{
						text = text + " + " + PlayerPauseUI.localization.format("BattlEye_Insecure");
					}
				}
				PlayerPauseUI.serverBox.Text = PlayerPauseUI.localization.format("Server_WithVersion", new object[]
				{
					localizedName,
					Level.version,
					OptionsSettings.streamer ? PlayerPauseUI.localization.format("Streamer") : Provider.serverName,
					text
				});
			}
			PlayerPauseUI.container.AnimateIntoView();
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x00184D9C File Offset: 0x00182F9C
		public static void close()
		{
			if (!PlayerPauseUI.active)
			{
				return;
			}
			PlayerPauseUI.active = false;
			PlayerPauseUI.exitButton.reset();
			PlayerPauseUI.quitButton.reset();
			PlayerPauseUI.suicideButton.reset();
			PlayerPauseUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x00184DE9 File Offset: 0x00182FE9
		public static void closeAndGotoAppropriateHUD()
		{
			PlayerPauseUI.close();
			if (Player.player.life.isDead)
			{
				PlayerDeathUI.open(false);
				return;
			}
			PlayerLifeUI.open();
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x00184E0D File Offset: 0x0018300D
		private static void onClickedReturnButton(ISleekElement button)
		{
			PlayerPauseUI.closeAndGotoAppropriateHUD();
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x00184E14 File Offset: 0x00183014
		private static void onClickedOptionsButton(ISleekElement button)
		{
			PlayerPauseUI.close();
			MenuConfigurationOptionsUI.open();
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x00184E20 File Offset: 0x00183020
		private static void onClickedDisplayButton(ISleekElement button)
		{
			PlayerPauseUI.close();
			MenuConfigurationDisplayUI.open();
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x00184E2C File Offset: 0x0018302C
		private static void onClickedGraphicsButton(ISleekElement button)
		{
			PlayerPauseUI.close();
			MenuConfigurationGraphicsUI.open();
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x00184E38 File Offset: 0x00183038
		private static void onClickedControlsButton(ISleekElement button)
		{
			PlayerPauseUI.close();
			MenuConfigurationControlsUI.open();
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x00184E44 File Offset: 0x00183044
		private static void onClickedAudioButton(ISleekElement button)
		{
			PlayerPauseUI.close();
			PlayerPauseUI.audioMenu.open();
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x00184E58 File Offset: 0x00183058
		private static void onClickedSpyRefreshButton(ISleekElement button)
		{
			EChatMode mode = EChatMode.GLOBAL;
			string text = "/spy ";
			CSteamID csteamID = PlayerPauseUI.spySteamID;
			ChatManager.sendChat(mode, text + csteamID.ToString());
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x00184E88 File Offset: 0x00183088
		private static void onClickedSpySlayButton(ISleekElement button)
		{
			EChatMode mode = EChatMode.GLOBAL;
			string text = "/slay ";
			CSteamID csteamID = PlayerPauseUI.spySteamID;
			ChatManager.sendChat(mode, text + csteamID.ToString() + "/Screenshot Evidence");
		}

		/// <summary>
		/// Exit button only needs to wait for timer in certain conditions.
		/// </summary>
		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x060043F8 RID: 17400 RVA: 0x00184EC0 File Offset: 0x001830C0
		public static bool shouldExitButtonRespectTimer
		{
			get
			{
				return !Provider.isServer && Provider.isPvP && Provider.clients.Count >= 2 && !(Player.player == null) && !Player.player.life.isDead && (!Player.player.movement.isSafe || !Player.player.movement.isSafeInfo.noWeapons);
			}
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x00184F3A File Offset: 0x0018313A
		private static void onClickedExitButton(SleekButtonIconConfirm button)
		{
			if (PlayerPauseUI.shouldExitButtonRespectTimer && Time.realtimeSinceStartup - PlayerPauseUI.lastLeave < Provider.modeConfigData.Gameplay.Timer_Exit)
			{
				return;
			}
			Provider.RequestDisconnect("clicked exit button from in-game pause menu");
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x00184F6C File Offset: 0x0018316C
		private static void onClickedQuitButton(SleekButtonIconConfirm button)
		{
			if (PlayerPauseUI.shouldExitButtonRespectTimer && Time.realtimeSinceStartup - PlayerPauseUI.lastLeave < Provider.modeConfigData.Gameplay.Timer_Exit)
			{
				return;
			}
			Provider.QuitGame("clicked quit from in-game pause menu");
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x00184FA0 File Offset: 0x001831A0
		private static void onClickedSuicideButton(SleekButtonIconConfirm button)
		{
			if (((Level.info != null && Level.info.type == ELevelType.SURVIVAL) || !Player.player.movement.isSafe || !Player.player.movement.isSafeInfo.noWeapons) && Provider.modeConfigData.Gameplay.Can_Suicide)
			{
				PlayerPauseUI.closeAndGotoAppropriateHUD();
				Player.player.life.sendSuicide();
			}
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0018500C File Offset: 0x0018320C
		private static void onClickedFavoriteButton(ISleekElement button)
		{
			Provider.toggleCurrentServerFavorited();
			PlayerPauseUI.updateFavorite();
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x00185018 File Offset: 0x00183218
		private static void OnClickedBookmarkButton(ISleekElement button)
		{
			Provider.ToggleCurrentServerBookmarked();
			PlayerPauseUI.UpdateBookmarkButton();
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x00185024 File Offset: 0x00183224
		private static void onClickedQuicksaveButton(ISleekElement button)
		{
			SaveManager.save();
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x0018502C File Offset: 0x0018322C
		private static void updateFavorite()
		{
			if (Provider.isCurrentServerFavorited)
			{
				PlayerPauseUI.favoriteButton.text = PlayerPauseUI.localization.format("Favorite_Off_Button_Text");
				PlayerPauseUI.favoriteButton.icon = PlayerPauseUI.icons.load<Texture2D>("Favorite_Off");
				return;
			}
			PlayerPauseUI.favoriteButton.text = PlayerPauseUI.localization.format("Favorite_On_Button_Text");
			PlayerPauseUI.favoriteButton.icon = PlayerPauseUI.icons.load<Texture2D>("Favorite_On");
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x001850A8 File Offset: 0x001832A8
		private static void UpdateBookmarkButton()
		{
			if (Provider.IsCurrentServerBookmarked)
			{
				PlayerPauseUI.bookmarkButton.text = MenuPlayServerInfoUI.localization.format("Bookmark_Off_Button");
				PlayerPauseUI.bookmarkButton.icon = MenuPlayUI.serverListUI.icons.load<Texture2D>("Bookmark_Remove");
				return;
			}
			PlayerPauseUI.bookmarkButton.text = MenuPlayServerInfoUI.localization.format("Bookmark_On_Button");
			PlayerPauseUI.bookmarkButton.icon = MenuPlayUI.serverListUI.icons.load<Texture2D>("Bookmark_Add");
		}

		// Token: 0x06004401 RID: 17409 RVA: 0x0018512C File Offset: 0x0018332C
		private static void onSpyReady(CSteamID steamID, byte[] data)
		{
			PlayerPauseUI.spySteamID = steamID;
			Texture2D texture2D = new Texture2D(640, 480, TextureFormat.RGB24, false);
			texture2D.name = "Spy";
			texture2D.filterMode = FilterMode.Trilinear;
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			ImageConversion.LoadImage(texture2D, data, true);
			PlayerPauseUI.spyImage.Texture = texture2D;
			PlayerPauseUI.returnButton.PositionOffset_X = -435f;
			PlayerPauseUI.optionsButton.PositionOffset_X = -435f;
			PlayerPauseUI.displayButton.PositionOffset_X = -435f;
			PlayerPauseUI.graphicsButton.PositionOffset_X = -435f;
			PlayerPauseUI.controlsButton.PositionOffset_X = -435f;
			PlayerPauseUI.audioButton.PositionOffset_X = -435f;
			PlayerPauseUI.exitButton.PositionOffset_X = -435f;
			PlayerPauseUI.quitButton.PositionOffset_X = -435f;
			PlayerPauseUI.suicideButton.PositionOffset_X = -435f;
			PlayerPauseUI.spyBox.PositionOffset_X = -225f;
			PlayerPauseUI.spyBox.IsVisible = true;
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x00185220 File Offset: 0x00183420
		internal void OnDestroy()
		{
			ClientMessageHandler_Accepted.OnGameplayConfigReceived -= new Action(this.OnGameplayConfigReceived);
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x00185233 File Offset: 0x00183433
		private void OnGameplayConfigReceived()
		{
			this.SyncSuicideButtonAvailable();
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x0018523C File Offset: 0x0018343C
		private void SyncSuicideButtonAvailable()
		{
			bool can_Suicide = Provider.modeConfigData.Gameplay.Can_Suicide;
			PlayerPauseUI.suicideButton.isClickable = can_Suicide;
			PlayerPauseUI.suicideDisabledLabel.IsVisible = !can_Suicide;
		}

		// Token: 0x06004405 RID: 17413 RVA: 0x00185274 File Offset: 0x00183474
		public PlayerPauseUI()
		{
			PlayerPauseUI.localization = Localization.read("/Player/PlayerPause.dat");
			if (PlayerPauseUI.icons != null)
			{
				PlayerPauseUI.icons.unload();
				PlayerPauseUI.icons = null;
			}
			PlayerPauseUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerPause/PlayerPause.unity3d");
			PlayerPauseUI.container = new SleekFullscreenBox();
			PlayerPauseUI.container.PositionScale_Y = 1f;
			PlayerPauseUI.container.PositionOffset_X = 10f;
			PlayerPauseUI.container.PositionOffset_Y = 10f;
			PlayerPauseUI.container.SizeOffset_X = -20f;
			PlayerPauseUI.container.SizeOffset_Y = -20f;
			PlayerPauseUI.container.SizeScale_X = 1f;
			PlayerPauseUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerPauseUI.container);
			PlayerPauseUI.active = false;
			int num = -265;
			PlayerPauseUI.returnButton = new SleekButtonIcon(PlayerPauseUI.icons.load<Texture2D>("Return"));
			PlayerPauseUI.returnButton.PositionOffset_X = -100f;
			PlayerPauseUI.returnButton.PositionOffset_Y = (float)num;
			PlayerPauseUI.returnButton.PositionScale_X = 0.5f;
			PlayerPauseUI.returnButton.PositionScale_Y = 0.5f;
			PlayerPauseUI.returnButton.SizeOffset_X = 200f;
			PlayerPauseUI.returnButton.SizeOffset_Y = 50f;
			PlayerPauseUI.returnButton.text = PlayerPauseUI.localization.format("Return_Button_Text");
			PlayerPauseUI.returnButton.tooltip = PlayerPauseUI.localization.format("Return_Button_Tooltip");
			PlayerPauseUI.returnButton.iconColor = 2;
			PlayerPauseUI.returnButton.onClickedButton += new ClickedButton(PlayerPauseUI.onClickedReturnButton);
			PlayerPauseUI.returnButton.fontSize = 3;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.returnButton);
			num += 60;
			PlayerPauseUI.optionsButton = new SleekButtonIcon(PlayerPauseUI.icons.load<Texture2D>("Options"));
			PlayerPauseUI.optionsButton.PositionOffset_X = -100f;
			PlayerPauseUI.optionsButton.PositionOffset_Y = (float)num;
			PlayerPauseUI.optionsButton.PositionScale_X = 0.5f;
			PlayerPauseUI.optionsButton.PositionScale_Y = 0.5f;
			PlayerPauseUI.optionsButton.SizeOffset_X = 200f;
			PlayerPauseUI.optionsButton.SizeOffset_Y = 50f;
			PlayerPauseUI.optionsButton.text = PlayerPauseUI.localization.format("Options_Button_Text");
			PlayerPauseUI.optionsButton.tooltip = PlayerPauseUI.localization.format("Options_Button_Tooltip");
			PlayerPauseUI.optionsButton.onClickedButton += new ClickedButton(PlayerPauseUI.onClickedOptionsButton);
			PlayerPauseUI.optionsButton.iconColor = 2;
			PlayerPauseUI.optionsButton.fontSize = 3;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.optionsButton);
			num += 60;
			PlayerPauseUI.displayButton = new SleekButtonIcon(PlayerPauseUI.icons.load<Texture2D>("Display"));
			PlayerPauseUI.displayButton.PositionOffset_X = -100f;
			PlayerPauseUI.displayButton.PositionOffset_Y = (float)num;
			PlayerPauseUI.displayButton.PositionScale_X = 0.5f;
			PlayerPauseUI.displayButton.PositionScale_Y = 0.5f;
			PlayerPauseUI.displayButton.SizeOffset_X = 200f;
			PlayerPauseUI.displayButton.SizeOffset_Y = 50f;
			PlayerPauseUI.displayButton.text = PlayerPauseUI.localization.format("Display_Button_Text");
			PlayerPauseUI.displayButton.tooltip = PlayerPauseUI.localization.format("Display_Button_Tooltip");
			PlayerPauseUI.displayButton.iconColor = 2;
			PlayerPauseUI.displayButton.onClickedButton += new ClickedButton(PlayerPauseUI.onClickedDisplayButton);
			PlayerPauseUI.displayButton.fontSize = 3;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.displayButton);
			num += 60;
			PlayerPauseUI.graphicsButton = new SleekButtonIcon(PlayerPauseUI.icons.load<Texture2D>("Graphics"));
			PlayerPauseUI.graphicsButton.PositionOffset_X = -100f;
			PlayerPauseUI.graphicsButton.PositionOffset_Y = (float)num;
			PlayerPauseUI.graphicsButton.PositionScale_X = 0.5f;
			PlayerPauseUI.graphicsButton.PositionScale_Y = 0.5f;
			PlayerPauseUI.graphicsButton.SizeOffset_X = 200f;
			PlayerPauseUI.graphicsButton.SizeOffset_Y = 50f;
			PlayerPauseUI.graphicsButton.text = PlayerPauseUI.localization.format("Graphics_Button_Text");
			PlayerPauseUI.graphicsButton.tooltip = PlayerPauseUI.localization.format("Graphics_Button_Tooltip");
			PlayerPauseUI.graphicsButton.iconColor = 2;
			PlayerPauseUI.graphicsButton.onClickedButton += new ClickedButton(PlayerPauseUI.onClickedGraphicsButton);
			PlayerPauseUI.graphicsButton.fontSize = 3;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.graphicsButton);
			num += 60;
			PlayerPauseUI.controlsButton = new SleekButtonIcon(PlayerPauseUI.icons.load<Texture2D>("Controls"));
			PlayerPauseUI.controlsButton.PositionOffset_X = -100f;
			PlayerPauseUI.controlsButton.PositionOffset_Y = (float)num;
			PlayerPauseUI.controlsButton.PositionScale_X = 0.5f;
			PlayerPauseUI.controlsButton.PositionScale_Y = 0.5f;
			PlayerPauseUI.controlsButton.SizeOffset_X = 200f;
			PlayerPauseUI.controlsButton.SizeOffset_Y = 50f;
			PlayerPauseUI.controlsButton.text = PlayerPauseUI.localization.format("Controls_Button_Text");
			PlayerPauseUI.controlsButton.tooltip = PlayerPauseUI.localization.format("Controls_Button_Tooltip");
			PlayerPauseUI.controlsButton.iconColor = 2;
			PlayerPauseUI.controlsButton.onClickedButton += new ClickedButton(PlayerPauseUI.onClickedControlsButton);
			PlayerPauseUI.controlsButton.fontSize = 3;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.controlsButton);
			num += 60;
			PlayerPauseUI.audioButton = new SleekButtonIcon(PlayerPauseUI.icons.load<Texture2D>("Audio"));
			PlayerPauseUI.audioButton.PositionOffset_X = -100f;
			PlayerPauseUI.audioButton.PositionOffset_Y = (float)num;
			PlayerPauseUI.audioButton.PositionScale_X = 0.5f;
			PlayerPauseUI.audioButton.PositionScale_Y = 0.5f;
			PlayerPauseUI.audioButton.SizeOffset_X = 200f;
			PlayerPauseUI.audioButton.SizeOffset_Y = 50f;
			PlayerPauseUI.audioButton.text = PlayerPauseUI.localization.format("Audio_Button_Text");
			PlayerPauseUI.audioButton.tooltip = PlayerPauseUI.localization.format("Audio_Button_Tooltip");
			PlayerPauseUI.audioButton.iconColor = 2;
			PlayerPauseUI.audioButton.onClickedButton += new ClickedButton(PlayerPauseUI.onClickedAudioButton);
			PlayerPauseUI.audioButton.fontSize = 3;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.audioButton);
			num += 60;
			PlayerPauseUI.suicideButton = new SleekButtonIconConfirm(PlayerPauseUI.icons.load<Texture2D>("Suicide"), PlayerPauseUI.localization.format("Suicide_Button_Confirm"), PlayerPauseUI.localization.format("Suicide_Button_Confirm_Tooltip"), PlayerPauseUI.localization.format("Suicide_Button_Deny"), PlayerPauseUI.localization.format("Suicide_Button_Deny_Tooltip"));
			PlayerPauseUI.suicideButton.PositionOffset_X = -100f;
			PlayerPauseUI.suicideButton.PositionOffset_Y = (float)num;
			PlayerPauseUI.suicideButton.PositionScale_X = 0.5f;
			PlayerPauseUI.suicideButton.PositionScale_Y = 0.5f;
			PlayerPauseUI.suicideButton.SizeOffset_X = 200f;
			PlayerPauseUI.suicideButton.SizeOffset_Y = 50f;
			PlayerPauseUI.suicideButton.text = PlayerPauseUI.localization.format("Suicide_Button_Text");
			PlayerPauseUI.suicideButton.tooltip = PlayerPauseUI.localization.format("Suicide_Button_Tooltip");
			PlayerPauseUI.suicideButton.iconColor = 2;
			PlayerPauseUI.suicideButton.onConfirmed = new Confirm(PlayerPauseUI.onClickedSuicideButton);
			PlayerPauseUI.suicideButton.fontSize = 3;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.suicideButton);
			PlayerPauseUI.suicideDisabledLabel = Glazier.Get().CreateLabel();
			PlayerPauseUI.suicideDisabledLabel.PositionOffset_X = -100f;
			PlayerPauseUI.suicideDisabledLabel.PositionOffset_Y = (float)num;
			PlayerPauseUI.suicideDisabledLabel.PositionScale_X = 0.5f;
			PlayerPauseUI.suicideDisabledLabel.PositionScale_Y = 0.5f;
			PlayerPauseUI.suicideDisabledLabel.SizeOffset_X = 200f;
			PlayerPauseUI.suicideDisabledLabel.SizeOffset_Y = 50f;
			PlayerPauseUI.suicideDisabledLabel.Text = PlayerPauseUI.localization.format("Suicide_Disabled");
			PlayerPauseUI.suicideDisabledLabel.TextColor = 6;
			PlayerPauseUI.suicideDisabledLabel.FontSize = 4;
			PlayerPauseUI.suicideDisabledLabel.TextContrastContext = 1;
			PlayerPauseUI.suicideDisabledLabel.IsVisible = false;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.suicideDisabledLabel);
			num += 60;
			PlayerPauseUI.exitButton = new SleekButtonIconConfirm(PlayerPauseUI.icons.load<Texture2D>("Exit"), PlayerPauseUI.localization.format("Exit_Button_Text"), PlayerPauseUI.localization.format("Exit_Button_Tooltip"), PlayerPauseUI.localization.format("Return_Button_Text"), string.Empty);
			PlayerPauseUI.exitButton.PositionOffset_X = -100f;
			PlayerPauseUI.exitButton.PositionOffset_Y = (float)num;
			PlayerPauseUI.exitButton.PositionScale_X = 0.5f;
			PlayerPauseUI.exitButton.PositionScale_Y = 0.5f;
			PlayerPauseUI.exitButton.SizeOffset_X = 200f;
			PlayerPauseUI.exitButton.SizeOffset_Y = 50f;
			PlayerPauseUI.exitButton.text = PlayerPauseUI.localization.format("Exit_Button_Text");
			PlayerPauseUI.exitButton.tooltip = PlayerPauseUI.localization.format("Exit_Button_Tooltip");
			PlayerPauseUI.exitButton.iconColor = 2;
			SleekButtonIconConfirm sleekButtonIconConfirm = PlayerPauseUI.exitButton;
			sleekButtonIconConfirm.onConfirmed = (Confirm)Delegate.Combine(sleekButtonIconConfirm.onConfirmed, new Confirm(PlayerPauseUI.onClickedExitButton));
			PlayerPauseUI.exitButton.fontSize = 3;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.exitButton);
			num += 60;
			PlayerPauseUI.quitButton = new SleekButtonIconConfirm(MenuPauseUI.icons.load<Texture2D>("Quit"), PlayerPauseUI.localization.format("Quit_Button"), PlayerPauseUI.localization.format("Quit_Button_Tooltip"), PlayerPauseUI.localization.format("Return_Button_Text"), string.Empty);
			PlayerPauseUI.quitButton.PositionOffset_X = -100f;
			PlayerPauseUI.quitButton.PositionOffset_Y = (float)num;
			PlayerPauseUI.quitButton.PositionScale_X = 0.5f;
			PlayerPauseUI.quitButton.PositionScale_Y = 0.5f;
			PlayerPauseUI.quitButton.SizeOffset_X = 200f;
			PlayerPauseUI.quitButton.SizeOffset_Y = 50f;
			PlayerPauseUI.quitButton.text = PlayerPauseUI.localization.format("Quit_Button");
			PlayerPauseUI.quitButton.tooltip = PlayerPauseUI.localization.format("Quit_Button_Tooltip");
			PlayerPauseUI.quitButton.iconColor = 2;
			SleekButtonIconConfirm sleekButtonIconConfirm2 = PlayerPauseUI.quitButton;
			sleekButtonIconConfirm2.onConfirmed = (Confirm)Delegate.Combine(sleekButtonIconConfirm2.onConfirmed, new Confirm(PlayerPauseUI.onClickedQuitButton));
			PlayerPauseUI.quitButton.fontSize = 3;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.quitButton);
			num += 60;
			PlayerPauseUI.spyBox = Glazier.Get().CreateBox();
			PlayerPauseUI.spyBox.PositionOffset_Y = -310f;
			PlayerPauseUI.spyBox.PositionScale_X = 0.5f;
			PlayerPauseUI.spyBox.PositionScale_Y = 0.5f;
			PlayerPauseUI.spyBox.SizeOffset_X = 660f;
			PlayerPauseUI.spyBox.SizeOffset_Y = 500f;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.spyBox);
			PlayerPauseUI.spyBox.IsVisible = false;
			PlayerPauseUI.spyImage = Glazier.Get().CreateImage();
			PlayerPauseUI.spyImage.PositionOffset_X = 10f;
			PlayerPauseUI.spyImage.PositionOffset_Y = 10f;
			PlayerPauseUI.spyImage.SizeOffset_X = 640f;
			PlayerPauseUI.spyImage.SizeOffset_Y = 480f;
			PlayerPauseUI.spyBox.AddChild(PlayerPauseUI.spyImage);
			PlayerPauseUI.spyRefreshButton = Glazier.Get().CreateButton();
			PlayerPauseUI.spyRefreshButton.PositionOffset_X = -205f;
			PlayerPauseUI.spyRefreshButton.PositionOffset_Y = 10f;
			PlayerPauseUI.spyRefreshButton.PositionScale_X = 0.5f;
			PlayerPauseUI.spyRefreshButton.PositionScale_Y = 1f;
			PlayerPauseUI.spyRefreshButton.SizeOffset_X = 200f;
			PlayerPauseUI.spyRefreshButton.SizeOffset_Y = 50f;
			PlayerPauseUI.spyRefreshButton.Text = PlayerPauseUI.localization.format("Spy_Refresh_Button_Text");
			PlayerPauseUI.spyRefreshButton.TooltipText = PlayerPauseUI.localization.format("Spy_Refresh_Button_Tooltip");
			PlayerPauseUI.spyRefreshButton.OnClicked += new ClickedButton(PlayerPauseUI.onClickedSpyRefreshButton);
			PlayerPauseUI.spyRefreshButton.FontSize = 3;
			PlayerPauseUI.spyBox.AddChild(PlayerPauseUI.spyRefreshButton);
			PlayerPauseUI.spySlayButton = Glazier.Get().CreateButton();
			PlayerPauseUI.spySlayButton.PositionOffset_X = 5f;
			PlayerPauseUI.spySlayButton.PositionOffset_Y = 10f;
			PlayerPauseUI.spySlayButton.PositionScale_X = 0.5f;
			PlayerPauseUI.spySlayButton.PositionScale_Y = 1f;
			PlayerPauseUI.spySlayButton.SizeOffset_X = 200f;
			PlayerPauseUI.spySlayButton.SizeOffset_Y = 50f;
			PlayerPauseUI.spySlayButton.Text = PlayerPauseUI.localization.format("Spy_Slay_Button_Text");
			PlayerPauseUI.spySlayButton.TooltipText = PlayerPauseUI.localization.format("Spy_Slay_Button_Tooltip");
			PlayerPauseUI.spySlayButton.OnClicked += new ClickedButton(PlayerPauseUI.onClickedSpySlayButton);
			PlayerPauseUI.spySlayButton.FontSize = 3;
			PlayerPauseUI.spyBox.AddChild(PlayerPauseUI.spySlayButton);
			PlayerPauseUI.serverBox = Glazier.Get().CreateBox();
			PlayerPauseUI.serverBox.PositionOffset_Y = -50f;
			PlayerPauseUI.serverBox.PositionScale_Y = 1f;
			PlayerPauseUI.serverBox.SizeOffset_X = -5f;
			PlayerPauseUI.serverBox.SizeOffset_Y = 50f;
			PlayerPauseUI.serverBox.SizeScale_X = 0.75f;
			PlayerPauseUI.serverBox.FontSize = 3;
			PlayerPauseUI.container.AddChild(PlayerPauseUI.serverBox);
			if (Provider.isServer)
			{
				PlayerPauseUI.quicksaveButton = Glazier.Get().CreateButton();
				PlayerPauseUI.quicksaveButton.PositionScale_X = 0.75f;
				PlayerPauseUI.quicksaveButton.PositionOffset_Y = -50f;
				PlayerPauseUI.quicksaveButton.PositionOffset_X = 5f;
				PlayerPauseUI.quicksaveButton.PositionScale_Y = 1f;
				PlayerPauseUI.quicksaveButton.SizeOffset_X = -5f;
				PlayerPauseUI.quicksaveButton.SizeOffset_Y = 50f;
				PlayerPauseUI.quicksaveButton.SizeScale_X = 0.25f;
				PlayerPauseUI.quicksaveButton.Text = PlayerPauseUI.localization.format("Quicksave_Button");
				PlayerPauseUI.quicksaveButton.TooltipText = PlayerPauseUI.localization.format("Quicksave_Button_Tooltip");
				PlayerPauseUI.quicksaveButton.FontSize = 3;
				PlayerPauseUI.quicksaveButton.OnClicked += new ClickedButton(PlayerPauseUI.onClickedQuicksaveButton);
				PlayerPauseUI.container.AddChild(PlayerPauseUI.quicksaveButton);
				PlayerPauseUI.favoriteButton = null;
				PlayerPauseUI.bookmarkButton = null;
			}
			else
			{
				PlayerPauseUI.quicksaveButton = null;
				PlayerPauseUI.favoriteButton = null;
				PlayerPauseUI.bookmarkButton = null;
				if (Provider.CanFavoriteCurrentServer)
				{
					PlayerPauseUI.favoriteButton = new SleekButtonIcon(null);
					PlayerPauseUI.favoriteButton.PositionOffset_Y = -50f;
					PlayerPauseUI.favoriteButton.PositionScale_Y = 1f;
					PlayerPauseUI.favoriteButton.SizeOffset_Y = 50f;
					PlayerPauseUI.favoriteButton.tooltip = PlayerPauseUI.localization.format("Favorite_Button_Tooltip");
					PlayerPauseUI.favoriteButton.fontSize = 3;
					PlayerPauseUI.favoriteButton.onClickedButton += new ClickedButton(PlayerPauseUI.onClickedFavoriteButton);
					PlayerPauseUI.container.AddChild(PlayerPauseUI.favoriteButton);
				}
				if (Provider.CanBookmarkCurrentServer)
				{
					PlayerPauseUI.bookmarkButton = new SleekButtonIcon(null, 40);
					PlayerPauseUI.bookmarkButton.PositionOffset_Y = -50f;
					PlayerPauseUI.bookmarkButton.PositionScale_Y = 1f;
					PlayerPauseUI.bookmarkButton.SizeOffset_Y = 50f;
					PlayerPauseUI.bookmarkButton.tooltip = MenuPlayServerInfoUI.localization.format("Bookmark_Button_Tooltip");
					PlayerPauseUI.bookmarkButton.fontSize = 3;
					PlayerPauseUI.bookmarkButton.onClickedButton += new ClickedButton(PlayerPauseUI.OnClickedBookmarkButton);
					PlayerPauseUI.container.AddChild(PlayerPauseUI.bookmarkButton);
				}
				if (PlayerPauseUI.favoriteButton != null && PlayerPauseUI.bookmarkButton != null)
				{
					PlayerPauseUI.favoriteButton.PositionScale_X = 0.5f;
					PlayerPauseUI.favoriteButton.PositionOffset_X = 5f;
					PlayerPauseUI.favoriteButton.SizeOffset_X = -10f;
					PlayerPauseUI.favoriteButton.SizeScale_X = 0.25f;
					PlayerPauseUI.bookmarkButton.PositionScale_X = 0.75f;
					PlayerPauseUI.bookmarkButton.PositionOffset_X = 5f;
					PlayerPauseUI.bookmarkButton.SizeOffset_X = -5f;
					PlayerPauseUI.bookmarkButton.SizeScale_X = 0.25f;
					PlayerPauseUI.serverBox.SizeScale_X = 0.5f;
				}
				else if (PlayerPauseUI.favoriteButton != null)
				{
					PlayerPauseUI.favoriteButton.PositionScale_X = 0.75f;
					PlayerPauseUI.favoriteButton.PositionOffset_X = 5f;
					PlayerPauseUI.favoriteButton.SizeOffset_X = -5f;
					PlayerPauseUI.favoriteButton.SizeScale_X = 0.25f;
				}
				else if (PlayerPauseUI.bookmarkButton != null)
				{
					PlayerPauseUI.bookmarkButton.PositionScale_X = 0.75f;
					PlayerPauseUI.bookmarkButton.PositionOffset_X = 5f;
					PlayerPauseUI.bookmarkButton.SizeOffset_X = -5f;
					PlayerPauseUI.bookmarkButton.SizeScale_X = 0.25f;
				}
				else
				{
					PlayerPauseUI.serverBox.SizeScale_X = 1f;
					PlayerPauseUI.serverBox.SizeOffset_X = 0f;
				}
			}
			new MenuConfigurationOptionsUI();
			new MenuConfigurationDisplayUI();
			new MenuConfigurationGraphicsUI();
			new MenuConfigurationControlsUI();
			PlayerPauseUI.audioMenu = new MenuConfigurationAudioUI();
			PlayerPauseUI.audioMenu.PositionOffset_X = 10f;
			PlayerPauseUI.audioMenu.PositionOffset_Y = 10f;
			PlayerPauseUI.audioMenu.PositionScale_Y = 1f;
			PlayerPauseUI.audioMenu.SizeOffset_X = -20f;
			PlayerPauseUI.audioMenu.SizeOffset_Y = -20f;
			PlayerPauseUI.audioMenu.SizeScale_X = 1f;
			PlayerPauseUI.audioMenu.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerPauseUI.audioMenu);
			if (PlayerPauseUI.favoriteButton != null)
			{
				PlayerPauseUI.updateFavorite();
			}
			if (PlayerPauseUI.bookmarkButton != null)
			{
				PlayerPauseUI.UpdateBookmarkButton();
			}
			Player.onSpyReady = new PlayerSpyReady(PlayerPauseUI.onSpyReady);
			ClientMessageHandler_Accepted.OnGameplayConfigReceived += new Action(this.OnGameplayConfigReceived);
			this.SyncSuicideButtonAvailable();
		}

		// Token: 0x04002D60 RID: 11616
		private static SleekFullscreenBox container;

		// Token: 0x04002D61 RID: 11617
		public static Local localization;

		// Token: 0x04002D62 RID: 11618
		private static Bundle icons;

		// Token: 0x04002D63 RID: 11619
		public static bool active;

		// Token: 0x04002D64 RID: 11620
		private static SleekButtonIcon returnButton;

		// Token: 0x04002D65 RID: 11621
		private static SleekButtonIcon optionsButton;

		// Token: 0x04002D66 RID: 11622
		private static SleekButtonIcon displayButton;

		// Token: 0x04002D67 RID: 11623
		private static SleekButtonIcon graphicsButton;

		// Token: 0x04002D68 RID: 11624
		private static SleekButtonIcon controlsButton;

		// Token: 0x04002D69 RID: 11625
		private static SleekButtonIcon audioButton;

		// Token: 0x04002D6A RID: 11626
		public static SleekButtonIconConfirm exitButton;

		// Token: 0x04002D6B RID: 11627
		public static SleekButtonIconConfirm quitButton;

		// Token: 0x04002D6C RID: 11628
		private static SleekButtonIconConfirm suicideButton;

		// Token: 0x04002D6D RID: 11629
		private static ISleekLabel suicideDisabledLabel;

		// Token: 0x04002D6E RID: 11630
		private static ISleekBox spyBox;

		// Token: 0x04002D6F RID: 11631
		private static ISleekImage spyImage;

		// Token: 0x04002D70 RID: 11632
		private static ISleekButton spyRefreshButton;

		// Token: 0x04002D71 RID: 11633
		private static ISleekButton spySlayButton;

		// Token: 0x04002D72 RID: 11634
		private static ISleekBox serverBox;

		// Token: 0x04002D73 RID: 11635
		private static SleekButtonIcon favoriteButton;

		// Token: 0x04002D74 RID: 11636
		private static SleekButtonIcon bookmarkButton;

		// Token: 0x04002D75 RID: 11637
		private static ISleekButton quicksaveButton;

		// Token: 0x04002D76 RID: 11638
		private static CSteamID spySteamID;

		// Token: 0x04002D77 RID: 11639
		public static float lastLeave;

		// Token: 0x04002D78 RID: 11640
		internal static MenuConfigurationAudioUI audioMenu;
	}
}
