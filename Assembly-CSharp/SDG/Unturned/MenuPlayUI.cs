using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007A5 RID: 1957
	public class MenuPlayUI
	{
		// Token: 0x06004191 RID: 16785 RVA: 0x0015ED36 File Offset: 0x0015CF36
		public static void open()
		{
			if (MenuPlayUI.active)
			{
				return;
			}
			MenuPlayUI.active = true;
			MenuPlayUI.container.AnimateIntoView();
		}

		// Token: 0x06004192 RID: 16786 RVA: 0x0015ED50 File Offset: 0x0015CF50
		public static void close()
		{
			if (!MenuPlayUI.active)
			{
				return;
			}
			MenuPlayUI.active = false;
			MenuPlayUI.tutorialButton.reset();
			MenuPlayUI.container.AnimateOutOfView(0f, -1f);
		}

		// Token: 0x06004193 RID: 16787 RVA: 0x0015ED7E File Offset: 0x0015CF7E
		private static void onClickedConnectButton(ISleekElement button)
		{
			MenuPlayUI.onlineSafetyUI.OpenIfNecessary(EOnlineSafetyDestination.Connect);
			MenuPlayUI.close();
		}

		// Token: 0x06004194 RID: 16788 RVA: 0x0015ED90 File Offset: 0x0015CF90
		private static void onClickedServersButton(ISleekElement button)
		{
			MenuPlayUI.onlineSafetyUI.OpenIfNecessary(EOnlineSafetyDestination.ServerList);
			MenuPlayUI.close();
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x0015EDA2 File Offset: 0x0015CFA2
		private static void OnClickedServerBookmarksButton(ISleekElement button)
		{
			MenuPlayUI.onlineSafetyUI.OpenIfNecessary(EOnlineSafetyDestination.Bookmarks);
			MenuPlayUI.close();
		}

		// Token: 0x06004196 RID: 16790 RVA: 0x0015EDB4 File Offset: 0x0015CFB4
		private static void onClickedSingleplayerButton(ISleekElement button)
		{
			MenuPlaySingleplayerUI.open();
			MenuPlayUI.close();
		}

		// Token: 0x06004197 RID: 16791 RVA: 0x0015EDC0 File Offset: 0x0015CFC0
		private static void onClickedLobbiesButton(ISleekElement button)
		{
			MenuPlayUI.onlineSafetyUI.OpenIfNecessary(EOnlineSafetyDestination.Lobby);
			MenuPlayUI.close();
		}

		// Token: 0x06004198 RID: 16792 RVA: 0x0015EDD4 File Offset: 0x0015CFD4
		private static void onClickedTutorialButton(ISleekElement button)
		{
			if (ReadWrite.folderExists("/Worlds/Singleplayer_" + Characters.selected.ToString() + "/Level/Tutorial"))
			{
				ReadWrite.deleteFolder("/Worlds/Singleplayer_" + Characters.selected.ToString() + "/Level/Tutorial");
			}
			if (ReadWrite.folderExists(string.Concat(new string[]
			{
				"/Worlds/Singleplayer_",
				Characters.selected.ToString(),
				"/Players/",
				Provider.user.ToString(),
				"_",
				Characters.selected.ToString(),
				"/Tutorial"
			})))
			{
				ReadWrite.deleteFolder(string.Concat(new string[]
				{
					"/Worlds/Singleplayer_",
					Characters.selected.ToString(),
					"/Players/",
					Provider.user.ToString(),
					"_",
					Characters.selected.ToString(),
					"/Tutorial"
				}));
			}
			Provider.map = "Tutorial";
			Provider.singleplayer(EGameMode.TUTORIAL, false);
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x0015EF04 File Offset: 0x0015D104
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuDashboardUI.open();
			MenuTitleUI.open();
			MenuPlayUI.close();
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x0015EF15 File Offset: 0x0015D115
		public void OnDestroy()
		{
			this.connectUI.OnDestroy();
			this.serverInfoUI.OnDestroy();
			this.singleplayerUI.OnDestroy();
			this.lobbiesUI.OnDestroy();
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x0015EF44 File Offset: 0x0015D144
		public MenuPlayUI()
		{
			Local local = Localization.read("/Menu/Play/MenuPlay.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlay/MenuPlay.unity3d");
			MenuPlayUI.container = new SleekFullscreenBox();
			MenuPlayUI.container.PositionOffset_X = 10f;
			MenuPlayUI.container.PositionOffset_Y = 10f;
			MenuPlayUI.container.PositionScale_Y = -1f;
			MenuPlayUI.container.SizeOffset_X = -20f;
			MenuPlayUI.container.SizeOffset_Y = -20f;
			MenuPlayUI.container.SizeScale_X = 1f;
			MenuPlayUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPlayUI.container);
			MenuPlayUI.active = false;
			float num = 0f;
			ISleekElement sleekElement = Glazier.Get().CreateFrame();
			sleekElement.PositionOffset_X = -100f;
			sleekElement.PositionScale_X = 0.5f;
			sleekElement.PositionScale_Y = 0.5f;
			sleekElement.SizeOffset_X = 200f;
			MenuPlayUI.container.AddChild(sleekElement);
			MenuPlayUI.tutorialButton = new SleekButtonIconConfirm(bundle.load<Texture2D>("Tutorial"), local.format("Tutorial_Confirm_Label"), local.format("Tutorial_Confirm_Tooltip"), local.format("Tutorial_Deny_Label"), local.format("Tutorial_Deny_Tooltip"), 40);
			MenuPlayUI.tutorialButton.PositionOffset_Y = num;
			MenuPlayUI.tutorialButton.SizeOffset_X = 200f;
			MenuPlayUI.tutorialButton.SizeOffset_Y = 50f;
			MenuPlayUI.tutorialButton.text = local.format("TutorialButtonText");
			MenuPlayUI.tutorialButton.tooltip = local.format("TutorialButtonTooltip");
			SleekButtonIconConfirm sleekButtonIconConfirm = MenuPlayUI.tutorialButton;
			sleekButtonIconConfirm.onConfirmed = (Confirm)Delegate.Combine(sleekButtonIconConfirm.onConfirmed, new Confirm(MenuPlayUI.onClickedTutorialButton));
			MenuPlayUI.tutorialButton.fontSize = 3;
			MenuPlayUI.tutorialButton.iconColor = 2;
			sleekElement.AddChild(MenuPlayUI.tutorialButton);
			num += MenuPlayUI.tutorialButton.SizeOffset_Y;
			num += 10f;
			MenuPlayUI.singleplayerButton = new SleekButtonIcon(bundle.load<Texture2D>("Singleplayer"));
			MenuPlayUI.singleplayerButton.PositionOffset_Y = num;
			MenuPlayUI.singleplayerButton.SizeOffset_X = 200f;
			MenuPlayUI.singleplayerButton.SizeOffset_Y = 50f;
			MenuPlayUI.singleplayerButton.text = local.format("SingleplayerButtonText");
			MenuPlayUI.singleplayerButton.tooltip = local.format("SingleplayerButtonTooltip");
			MenuPlayUI.singleplayerButton.onClickedButton += new ClickedButton(MenuPlayUI.onClickedSingleplayerButton);
			MenuPlayUI.singleplayerButton.iconColor = 2;
			MenuPlayUI.singleplayerButton.fontSize = 3;
			sleekElement.AddChild(MenuPlayUI.singleplayerButton);
			num += MenuPlayUI.singleplayerButton.SizeOffset_Y;
			num += 10f;
			MenuPlayUI.serversButton = new SleekButtonIcon(bundle.load<Texture2D>("Servers"));
			MenuPlayUI.serversButton.PositionOffset_Y = num;
			MenuPlayUI.serversButton.SizeOffset_X = 200f;
			MenuPlayUI.serversButton.SizeOffset_Y = 50f;
			MenuPlayUI.serversButton.text = local.format("ServersButtonText");
			MenuPlayUI.serversButton.tooltip = local.format("ServersButtonTooltip");
			MenuPlayUI.serversButton.iconColor = 2;
			MenuPlayUI.serversButton.onClickedButton += new ClickedButton(MenuPlayUI.onClickedServersButton);
			MenuPlayUI.serversButton.fontSize = 3;
			sleekElement.AddChild(MenuPlayUI.serversButton);
			num += MenuPlayUI.serversButton.SizeOffset_Y;
			num += 10f;
			MenuPlayUI.connectButton = new SleekButtonIcon(bundle.load<Texture2D>("Connect"));
			MenuPlayUI.connectButton.PositionOffset_Y = num;
			MenuPlayUI.connectButton.SizeOffset_X = 200f;
			MenuPlayUI.connectButton.SizeOffset_Y = 50f;
			MenuPlayUI.connectButton.text = local.format("ConnectButtonText");
			MenuPlayUI.connectButton.tooltip = local.format("ConnectButtonTooltip");
			MenuPlayUI.connectButton.iconColor = 2;
			MenuPlayUI.connectButton.onClickedButton += new ClickedButton(MenuPlayUI.onClickedConnectButton);
			MenuPlayUI.connectButton.fontSize = 3;
			sleekElement.AddChild(MenuPlayUI.connectButton);
			num += MenuPlayUI.connectButton.SizeOffset_Y;
			num += 10f;
			MenuPlayUI.serverBookmarksButton = new SleekButtonIcon(bundle.load<Texture2D>("Bookmarks"), 40);
			MenuPlayUI.serverBookmarksButton.PositionOffset_Y = num;
			MenuPlayUI.serverBookmarksButton.SizeOffset_X = 200f;
			MenuPlayUI.serverBookmarksButton.SizeOffset_Y = 50f;
			MenuPlayUI.serverBookmarksButton.text = local.format("ServerBookmarksButtonText");
			MenuPlayUI.serverBookmarksButton.tooltip = local.format("ServerBookmarksButtonTooltip");
			MenuPlayUI.serverBookmarksButton.iconColor = 2;
			MenuPlayUI.serverBookmarksButton.onClickedButton += new ClickedButton(MenuPlayUI.OnClickedServerBookmarksButton);
			MenuPlayUI.serverBookmarksButton.fontSize = 3;
			sleekElement.AddChild(MenuPlayUI.serverBookmarksButton);
			num += MenuPlayUI.serverBookmarksButton.SizeOffset_Y;
			num += 10f;
			MenuPlayUI.lobbiesButton = new SleekButtonIcon(bundle.load<Texture2D>("Lobbies"));
			MenuPlayUI.lobbiesButton.PositionOffset_Y = num;
			MenuPlayUI.lobbiesButton.SizeOffset_X = 200f;
			MenuPlayUI.lobbiesButton.SizeOffset_Y = 50f;
			MenuPlayUI.lobbiesButton.text = local.format("LobbiesButtonText");
			MenuPlayUI.lobbiesButton.tooltip = local.format("LobbiesButtonTooltip");
			MenuPlayUI.lobbiesButton.onClickedButton += new ClickedButton(MenuPlayUI.onClickedLobbiesButton);
			MenuPlayUI.lobbiesButton.iconColor = 2;
			MenuPlayUI.lobbiesButton.fontSize = 3;
			sleekElement.AddChild(MenuPlayUI.lobbiesButton);
			num += MenuPlayUI.lobbiesButton.SizeOffset_Y;
			num += 10f;
			MenuPlayUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuPlayUI.backButton.PositionOffset_Y = num;
			MenuPlayUI.backButton.SizeOffset_X = 200f;
			MenuPlayUI.backButton.SizeOffset_Y = 50f;
			MenuPlayUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuPlayUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuPlayUI.backButton.onClickedButton += new ClickedButton(MenuPlayUI.onClickedBackButton);
			MenuPlayUI.backButton.fontSize = 3;
			MenuPlayUI.backButton.iconColor = 2;
			sleekElement.AddChild(MenuPlayUI.backButton);
			num += MenuPlayUI.backButton.SizeOffset_Y;
			sleekElement.SizeOffset_Y = num;
			sleekElement.PositionOffset_Y = -(num / 2f);
			bundle.unload();
			this.connectUI = new MenuPlayConnectUI();
			MenuPlayUI.serverListUI = new MenuPlayServersUI();
			MenuPlayUI.serverListUI.PositionOffset_X = 10f;
			MenuPlayUI.serverListUI.PositionOffset_Y = 10f;
			MenuPlayUI.serverListUI.PositionScale_Y = 1f;
			MenuPlayUI.serverListUI.SizeOffset_X = -20f;
			MenuPlayUI.serverListUI.SizeOffset_Y = -20f;
			MenuPlayUI.serverListUI.SizeScale_X = 1f;
			MenuPlayUI.serverListUI.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPlayUI.serverListUI);
			MenuPlayUI.serverBookmarksUI = new MenuPlayServerBookmarksUI();
			MenuPlayUI.serverBookmarksUI.PositionOffset_X = 10f;
			MenuPlayUI.serverBookmarksUI.PositionOffset_Y = 10f;
			MenuPlayUI.serverBookmarksUI.PositionScale_Y = 1f;
			MenuPlayUI.serverBookmarksUI.SizeOffset_X = -20f;
			MenuPlayUI.serverBookmarksUI.SizeOffset_Y = -20f;
			MenuPlayUI.serverBookmarksUI.SizeScale_X = 1f;
			MenuPlayUI.serverBookmarksUI.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPlayUI.serverBookmarksUI);
			MenuPlayUI.onlineSafetyUI = new MenuPlayOnlineSafetyUI();
			MenuPlayUI.onlineSafetyUI.PositionOffset_X = 10f;
			MenuPlayUI.onlineSafetyUI.PositionOffset_Y = 10f;
			MenuPlayUI.onlineSafetyUI.PositionScale_Y = 1f;
			MenuPlayUI.onlineSafetyUI.SizeOffset_X = -20f;
			MenuPlayUI.onlineSafetyUI.SizeOffset_Y = -20f;
			MenuPlayUI.onlineSafetyUI.SizeScale_X = 1f;
			MenuPlayUI.onlineSafetyUI.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPlayUI.onlineSafetyUI);
			this.serverInfoUI = new MenuPlayServerInfoUI();
			this.singleplayerUI = new MenuPlaySingleplayerUI();
			this.lobbiesUI = new MenuPlayLobbiesUI();
		}

		// Token: 0x04002A7B RID: 10875
		private static SleekFullscreenBox container;

		// Token: 0x04002A7C RID: 10876
		public static bool active;

		// Token: 0x04002A7D RID: 10877
		private static SleekButtonIcon connectButton;

		// Token: 0x04002A7E RID: 10878
		private static SleekButtonIcon serversButton;

		// Token: 0x04002A7F RID: 10879
		private static SleekButtonIcon serverBookmarksButton;

		// Token: 0x04002A80 RID: 10880
		private static SleekButtonIcon singleplayerButton;

		// Token: 0x04002A81 RID: 10881
		private static SleekButtonIcon lobbiesButton;

		// Token: 0x04002A82 RID: 10882
		private static SleekButtonIconConfirm tutorialButton;

		// Token: 0x04002A83 RID: 10883
		private static SleekButtonIcon backButton;

		// Token: 0x04002A84 RID: 10884
		private MenuPlayConnectUI connectUI;

		// Token: 0x04002A85 RID: 10885
		public static MenuPlayServersUI serverListUI;

		// Token: 0x04002A86 RID: 10886
		public static MenuPlayServerBookmarksUI serverBookmarksUI;

		// Token: 0x04002A87 RID: 10887
		public static MenuPlayOnlineSafetyUI onlineSafetyUI;

		// Token: 0x04002A88 RID: 10888
		private MenuPlayServerInfoUI serverInfoUI;

		// Token: 0x04002A89 RID: 10889
		private MenuPlaySingleplayerUI singleplayerUI;

		// Token: 0x04002A8A RID: 10890
		private MenuPlayLobbiesUI lobbiesUI;
	}
}
