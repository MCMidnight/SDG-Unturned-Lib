using System;
using SDG.Provider.Services.Browser;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200079B RID: 1947
	public class MenuPlayLobbiesUI
	{
		// Token: 0x060040B6 RID: 16566 RVA: 0x001504F9 File Offset: 0x0014E6F9
		public static void open()
		{
			if (MenuPlayLobbiesUI.active)
			{
				return;
			}
			MenuPlayLobbiesUI.active = true;
			if (Lobbies.inLobby)
			{
				MenuPlayLobbiesUI.setWaitingForLobby(false);
				MenuPlayLobbiesUI.refresh();
			}
			else
			{
				MenuPlayLobbiesUI.setWaitingForLobby(true);
				Lobbies.createLobby();
			}
			MenuPlayLobbiesUI.container.AnimateIntoView();
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x00150532 File Offset: 0x0014E732
		public static void close()
		{
			if (!MenuPlayLobbiesUI.active)
			{
				return;
			}
			MenuPlayLobbiesUI.active = false;
			MenuSettings.save();
			MenuPlayLobbiesUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x0015055C File Offset: 0x0014E75C
		private static void refresh()
		{
			MenuPlayLobbiesUI.membersBox.RemoveAllChildren();
			int lobbyMemberCount = Lobbies.getLobbyMemberCount();
			for (int i = 0; i < lobbyMemberCount; i++)
			{
				MenuPlayLobbiesUI.SleekLobbyPlayerButton sleekLobbyPlayerButton = new MenuPlayLobbiesUI.SleekLobbyPlayerButton(Lobbies.getLobbyMemberByIndex(i));
				sleekLobbyPlayerButton.PositionOffset_Y = (float)(i * 50);
				sleekLobbyPlayerButton.SizeOffset_Y = 50f;
				sleekLobbyPlayerButton.SizeScale_X = 1f;
				MenuPlayLobbiesUI.membersBox.AddChild(sleekLobbyPlayerButton);
			}
			MenuPlayLobbiesUI.membersBox.ContentSizeOffset = new Vector2(0f, (float)(lobbyMemberCount * 50));
		}

		// Token: 0x060040B9 RID: 16569 RVA: 0x001505D6 File Offset: 0x0014E7D6
		private static void handleLobbiesRefreshed()
		{
			if (!MenuPlayLobbiesUI.active)
			{
				return;
			}
			MenuPlayLobbiesUI.refresh();
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x001505E5 File Offset: 0x0014E7E5
		private static void handleLobbiesEntered()
		{
			if (MenuPlayLobbiesUI.active)
			{
				MenuPlayLobbiesUI.setWaitingForLobby(false);
				return;
			}
			MenuUI.closeAll();
			MenuPlayLobbiesUI.open();
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x001505FF File Offset: 0x0014E7FF
		private static void onClickedInviteButton(ISleekElement button)
		{
			if (!Lobbies.canOpenInvitations)
			{
				MenuUI.alert(MenuPlayLobbiesUI.localization.format("Overlay"));
				return;
			}
			Lobbies.openInvitations();
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x00150622 File Offset: 0x0014E822
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuPlayUI.open();
			MenuPlayLobbiesUI.close();
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x0015062E File Offset: 0x0014E82E
		private static void setWaitingForLobby(bool waiting)
		{
			MenuPlayLobbiesUI.inviteButton.isClickable = !waiting;
			MenuPlayLobbiesUI.waitingLabel.IsVisible = waiting;
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x00150649 File Offset: 0x0014E849
		public void OnDestroy()
		{
			Lobbies.lobbiesRefreshed -= MenuPlayLobbiesUI.handleLobbiesRefreshed;
			Lobbies.lobbiesEntered -= MenuPlayLobbiesUI.handleLobbiesEntered;
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x00150670 File Offset: 0x0014E870
		public MenuPlayLobbiesUI()
		{
			MenuPlayLobbiesUI.localization = Localization.read("/Menu/Play/MenuPlayLobbies.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlayLobbies/MenuPlayLobbies.unity3d");
			MenuPlayLobbiesUI.container = new SleekFullscreenBox();
			MenuPlayLobbiesUI.container.PositionOffset_X = 10f;
			MenuPlayLobbiesUI.container.PositionOffset_Y = 10f;
			MenuPlayLobbiesUI.container.PositionScale_Y = 1f;
			MenuPlayLobbiesUI.container.SizeOffset_X = -20f;
			MenuPlayLobbiesUI.container.SizeOffset_Y = -20f;
			MenuPlayLobbiesUI.container.SizeScale_X = 1f;
			MenuPlayLobbiesUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPlayLobbiesUI.container);
			MenuPlayLobbiesUI.active = false;
			bundle.unload();
			MenuPlayLobbiesUI.membersLabel = Glazier.Get().CreateLabel();
			MenuPlayLobbiesUI.membersLabel.PositionOffset_X = -200f;
			MenuPlayLobbiesUI.membersLabel.PositionOffset_Y = 100f;
			MenuPlayLobbiesUI.membersLabel.PositionScale_X = 0.5f;
			MenuPlayLobbiesUI.membersLabel.SizeOffset_X = 400f;
			MenuPlayLobbiesUI.membersLabel.SizeOffset_Y = 50f;
			MenuPlayLobbiesUI.membersLabel.Text = MenuPlayLobbiesUI.localization.format("Members");
			MenuPlayLobbiesUI.membersLabel.FontSize = 3;
			MenuPlayLobbiesUI.container.AddChild(MenuPlayLobbiesUI.membersLabel);
			MenuPlayLobbiesUI.membersBox = Glazier.Get().CreateScrollView();
			MenuPlayLobbiesUI.membersBox.PositionOffset_X = -200f;
			MenuPlayLobbiesUI.membersBox.PositionOffset_Y = 150f;
			MenuPlayLobbiesUI.membersBox.PositionScale_X = 0.5f;
			MenuPlayLobbiesUI.membersBox.SizeOffset_X = 430f;
			MenuPlayLobbiesUI.membersBox.SizeOffset_Y = -300f;
			MenuPlayLobbiesUI.membersBox.SizeScale_Y = 1f;
			MenuPlayLobbiesUI.membersBox.ScaleContentToWidth = true;
			MenuPlayLobbiesUI.container.AddChild(MenuPlayLobbiesUI.membersBox);
			MenuPlayLobbiesUI.inviteButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Invite"));
			MenuPlayLobbiesUI.inviteButton.PositionOffset_X = -200f;
			MenuPlayLobbiesUI.inviteButton.PositionOffset_Y = -150f;
			MenuPlayLobbiesUI.inviteButton.PositionScale_X = 0.5f;
			MenuPlayLobbiesUI.inviteButton.PositionScale_Y = 1f;
			MenuPlayLobbiesUI.inviteButton.SizeOffset_X = 400f;
			MenuPlayLobbiesUI.inviteButton.SizeOffset_Y = 50f;
			MenuPlayLobbiesUI.inviteButton.text = MenuPlayLobbiesUI.localization.format("Invite_Button");
			MenuPlayLobbiesUI.inviteButton.tooltip = MenuPlayLobbiesUI.localization.format("Invite_Button_Tooltip");
			MenuPlayLobbiesUI.inviteButton.onClickedButton += new ClickedButton(MenuPlayLobbiesUI.onClickedInviteButton);
			MenuPlayLobbiesUI.inviteButton.fontSize = 3;
			MenuPlayLobbiesUI.inviteButton.iconColor = 2;
			MenuPlayLobbiesUI.container.AddChild(MenuPlayLobbiesUI.inviteButton);
			MenuPlayLobbiesUI.waitingLabel = Glazier.Get().CreateLabel();
			MenuPlayLobbiesUI.waitingLabel.PositionOffset_X = -200f;
			MenuPlayLobbiesUI.waitingLabel.PositionOffset_Y = -200f;
			MenuPlayLobbiesUI.waitingLabel.PositionScale_X = 0.5f;
			MenuPlayLobbiesUI.waitingLabel.PositionScale_Y = 1f;
			MenuPlayLobbiesUI.waitingLabel.SizeOffset_X = 400f;
			MenuPlayLobbiesUI.waitingLabel.SizeOffset_Y = 50f;
			MenuPlayLobbiesUI.waitingLabel.Text = MenuPlayLobbiesUI.localization.format("Waiting");
			MenuPlayLobbiesUI.waitingLabel.IsVisible = false;
			MenuPlayLobbiesUI.container.AddChild(MenuPlayLobbiesUI.waitingLabel);
			MenuPlayLobbiesUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuPlayLobbiesUI.backButton.PositionOffset_Y = -50f;
			MenuPlayLobbiesUI.backButton.PositionScale_Y = 1f;
			MenuPlayLobbiesUI.backButton.SizeOffset_X = 200f;
			MenuPlayLobbiesUI.backButton.SizeOffset_Y = 50f;
			MenuPlayLobbiesUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuPlayLobbiesUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuPlayLobbiesUI.backButton.onClickedButton += new ClickedButton(MenuPlayLobbiesUI.onClickedBackButton);
			MenuPlayLobbiesUI.backButton.fontSize = 3;
			MenuPlayLobbiesUI.backButton.iconColor = 2;
			MenuPlayLobbiesUI.container.AddChild(MenuPlayLobbiesUI.backButton);
			Lobbies.lobbiesRefreshed += MenuPlayLobbiesUI.handleLobbiesRefreshed;
			Lobbies.lobbiesEntered += MenuPlayLobbiesUI.handleLobbiesEntered;
		}

		// Token: 0x04002996 RID: 10646
		public static Local localization;

		// Token: 0x04002997 RID: 10647
		private static SleekFullscreenBox container;

		// Token: 0x04002998 RID: 10648
		public static bool active;

		// Token: 0x04002999 RID: 10649
		private static ISleekLabel membersLabel;

		// Token: 0x0400299A RID: 10650
		private static ISleekScrollView membersBox;

		// Token: 0x0400299B RID: 10651
		private static SleekButtonIcon inviteButton;

		// Token: 0x0400299C RID: 10652
		private static ISleekLabel waitingLabel;

		// Token: 0x0400299D RID: 10653
		private static SleekButtonIcon backButton;

		// Token: 0x02000A05 RID: 2565
		public class SleekLobbyPlayerButton : SleekWrapper
		{
			// Token: 0x06004D47 RID: 19783 RVA: 0x001B9164 File Offset: 0x001B7364
			private void onClickedPlayerButton(ISleekElement button)
			{
				IBrowserService browserService = Provider.provider.browserService;
				string text = "http://steamcommunity.com/profiles/";
				CSteamID csteamID = this.steamID;
				browserService.open(text + csteamID.ToString());
			}

			// Token: 0x06004D48 RID: 19784 RVA: 0x001B91A0 File Offset: 0x001B73A0
			public SleekLobbyPlayerButton(CSteamID newSteamID)
			{
				this.steamID = newSteamID;
				this.button = Glazier.Get().CreateButton();
				this.button.SizeScale_X = 1f;
				this.button.SizeScale_Y = 1f;
				this.button.OnClicked += new ClickedButton(this.onClickedPlayerButton);
				base.AddChild(this.button);
				this.avatarImage = Glazier.Get().CreateImage();
				this.avatarImage.PositionOffset_X = 9f;
				this.avatarImage.PositionOffset_Y = 9f;
				this.avatarImage.SizeOffset_X = 32f;
				this.avatarImage.SizeOffset_Y = 32f;
				this.avatarImage.Texture = Provider.provider.communityService.getIcon(this.steamID, false);
				this.avatarImage.ShouldDestroyTexture = true;
				this.button.AddChild(this.avatarImage);
				this.nameLabel = Glazier.Get().CreateLabel();
				this.nameLabel.PositionOffset_X = 40f;
				this.nameLabel.SizeOffset_X = -40f;
				this.nameLabel.SizeScale_X = 1f;
				this.nameLabel.SizeScale_Y = 1f;
				this.nameLabel.Text = SteamFriends.GetFriendPersonaName(this.steamID);
				this.nameLabel.FontSize = 3;
				this.button.AddChild(this.nameLabel);
			}

			// Token: 0x040034F8 RID: 13560
			private CSteamID steamID;

			// Token: 0x040034F9 RID: 13561
			private ISleekButton button;

			// Token: 0x040034FA RID: 13562
			private ISleekImage avatarImage;

			// Token: 0x040034FB RID: 13563
			private ISleekLabel nameLabel;
		}
	}
}
