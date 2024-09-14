using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000794 RID: 1940
	public class MenuPauseUI
	{
		// Token: 0x06004076 RID: 16502 RVA: 0x0014D558 File Offset: 0x0014B758
		public static void open()
		{
			if (MenuPauseUI.active)
			{
				return;
			}
			MenuPauseUI.active = true;
			MenuPauseUI.container.AnimateIntoView();
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x0014D572 File Offset: 0x0014B772
		public static void close()
		{
			if (!MenuPauseUI.active)
			{
				return;
			}
			MenuPauseUI.active = false;
			MenuPauseUI.container.AnimateOutOfView(0f, -1f);
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x0014D596 File Offset: 0x0014B796
		private static void onClickedReturnButton(ISleekElement button)
		{
			MenuPauseUI.close();
			MenuDashboardUI.open();
			MenuTitleUI.open();
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x0014D5A7 File Offset: 0x0014B7A7
		private static void onClickedQuitButton(SleekButtonIconConfirm button)
		{
			Provider.QuitGame("clicked quit in main menu");
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x0014D5B3 File Offset: 0x0014B7B3
		private static void onClickedSupportButton(ISleekElement button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("https://support.smartlydressedgames.com/hc/en-us");
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x0014D5EF File Offset: 0x0014B7EF
		private static void onClickedTwitterButton(ISleekElement button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("https://twitter.com/SDGNelson");
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x0014D62B File Offset: 0x0014B82B
		private static void onClickedSteamButton(ISleekElement button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("http://steamcommunity.com/app/304930/announcements/");
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x0014D667 File Offset: 0x0014B867
		private static void onClickedCreditsButton(ISleekElement button)
		{
			MenuPauseUI.close();
			MenuCreditsUI.open();
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x0014D673 File Offset: 0x0014B873
		private static void onClickedForumButton(ISleekElement button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("https://forum.smartlydressedgames.com/");
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x0014D6AF File Offset: 0x0014B8AF
		private static void onClickedBlogButton(ISleekElement button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("https://blog.smartlydressedgames.com/");
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x0014D6EB File Offset: 0x0014B8EB
		private static void onClickedWikiButton(ISleekElement button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("https://unturned.wiki.gg");
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x0014D728 File Offset: 0x0014B928
		public MenuPauseUI()
		{
			MenuPauseUI.localization = Localization.read("/Menu/MenuPause.dat");
			if (MenuPauseUI.icons != null)
			{
				MenuPauseUI.icons.unload();
				MenuPauseUI.icons = null;
			}
			MenuPauseUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/MenuPause/MenuPause.unity3d");
			MenuPauseUI.container = new SleekFullscreenBox();
			MenuPauseUI.container.PositionOffset_X = 10f;
			MenuPauseUI.container.PositionOffset_Y = 10f;
			MenuPauseUI.container.PositionScale_Y = -1f;
			MenuPauseUI.container.SizeOffset_X = -20f;
			MenuPauseUI.container.SizeOffset_Y = -20f;
			MenuPauseUI.container.SizeScale_X = 1f;
			MenuPauseUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPauseUI.container);
			MenuPauseUI.active = false;
			MenuPauseUI.quitButton = new SleekButtonIconConfirm(MenuPauseUI.icons.load<Texture2D>("Quit"), MenuPauseUI.localization.format("Exit_Button"), MenuPauseUI.localization.format("Exit_Button_Tooltip"), MenuPauseUI.localization.format("Return_Button"), string.Empty);
			MenuPauseUI.quitButton.PositionOffset_X = -100f;
			MenuPauseUI.quitButton.PositionOffset_Y = -265f;
			MenuPauseUI.quitButton.PositionScale_X = 0.5f;
			MenuPauseUI.quitButton.PositionScale_Y = 0.5f;
			MenuPauseUI.quitButton.SizeOffset_X = 200f;
			MenuPauseUI.quitButton.SizeOffset_Y = 50f;
			MenuPauseUI.quitButton.text = MenuPauseUI.localization.format("Exit_Button");
			MenuPauseUI.quitButton.tooltip = MenuPauseUI.localization.format("Exit_Button_Tooltip");
			SleekButtonIconConfirm sleekButtonIconConfirm = MenuPauseUI.quitButton;
			sleekButtonIconConfirm.onConfirmed = (Confirm)Delegate.Combine(sleekButtonIconConfirm.onConfirmed, new Confirm(MenuPauseUI.onClickedQuitButton));
			MenuPauseUI.quitButton.fontSize = 3;
			MenuPauseUI.quitButton.iconColor = 2;
			MenuPauseUI.container.AddChild(MenuPauseUI.quitButton);
			MenuPauseUI.returnButton = new SleekButtonIcon(MenuPauseUI.icons.load<Texture2D>("Return"));
			MenuPauseUI.returnButton.PositionOffset_X = -100f;
			MenuPauseUI.returnButton.PositionOffset_Y = -205f;
			MenuPauseUI.returnButton.PositionScale_X = 0.5f;
			MenuPauseUI.returnButton.PositionScale_Y = 0.5f;
			MenuPauseUI.returnButton.SizeOffset_X = 200f;
			MenuPauseUI.returnButton.SizeOffset_Y = 50f;
			MenuPauseUI.returnButton.text = MenuPauseUI.localization.format("Return_Button");
			MenuPauseUI.returnButton.tooltip = MenuPauseUI.localization.format("Return_Button_Tooltip");
			MenuPauseUI.returnButton.onClickedButton += new ClickedButton(MenuPauseUI.onClickedReturnButton);
			MenuPauseUI.returnButton.fontSize = 3;
			MenuPauseUI.returnButton.iconColor = 2;
			MenuPauseUI.container.AddChild(MenuPauseUI.returnButton);
			MenuPauseUI.supportButton = new SleekButtonIcon(MenuPauseUI.icons.load<Texture2D>("Support"));
			MenuPauseUI.supportButton.PositionOffset_X = -100f;
			MenuPauseUI.supportButton.PositionOffset_Y = -145f;
			MenuPauseUI.supportButton.PositionScale_X = 0.5f;
			MenuPauseUI.supportButton.PositionScale_Y = 0.5f;
			MenuPauseUI.supportButton.SizeOffset_X = 200f;
			MenuPauseUI.supportButton.SizeOffset_Y = 50f;
			MenuPauseUI.supportButton.text = MenuPauseUI.localization.format("Support_Label");
			MenuPauseUI.supportButton.tooltip = MenuPauseUI.localization.format("Support_Tooltip");
			MenuPauseUI.supportButton.onClickedButton += new ClickedButton(MenuPauseUI.onClickedSupportButton);
			MenuPauseUI.supportButton.fontSize = 3;
			MenuPauseUI.supportButton.iconColor = 2;
			MenuPauseUI.container.AddChild(MenuPauseUI.supportButton);
			MenuPauseUI.twitterButton = new SleekButtonIcon(MenuPauseUI.icons.load<Texture2D>("Twitter"));
			MenuPauseUI.twitterButton.PositionOffset_X = -100f;
			MenuPauseUI.twitterButton.PositionOffset_Y = -85f;
			MenuPauseUI.twitterButton.PositionScale_X = 0.5f;
			MenuPauseUI.twitterButton.PositionScale_Y = 0.5f;
			MenuPauseUI.twitterButton.SizeOffset_X = 200f;
			MenuPauseUI.twitterButton.SizeOffset_Y = 50f;
			MenuPauseUI.twitterButton.text = MenuPauseUI.localization.format("Twitter_Button");
			MenuPauseUI.twitterButton.tooltip = MenuPauseUI.localization.format("Twitter_Button_Tooltip");
			MenuPauseUI.twitterButton.onClickedButton += new ClickedButton(MenuPauseUI.onClickedTwitterButton);
			MenuPauseUI.twitterButton.fontSize = 3;
			MenuPauseUI.container.AddChild(MenuPauseUI.twitterButton);
			MenuPauseUI.steamButton = new SleekButtonIcon(MenuPauseUI.icons.load<Texture2D>("Steam"));
			MenuPauseUI.steamButton.PositionOffset_X = -100f;
			MenuPauseUI.steamButton.PositionOffset_Y = -25f;
			MenuPauseUI.steamButton.PositionScale_X = 0.5f;
			MenuPauseUI.steamButton.PositionScale_Y = 0.5f;
			MenuPauseUI.steamButton.SizeOffset_X = 200f;
			MenuPauseUI.steamButton.SizeOffset_Y = 50f;
			MenuPauseUI.steamButton.text = MenuPauseUI.localization.format("Steam_Button");
			MenuPauseUI.steamButton.tooltip = MenuPauseUI.localization.format("Steam_Button_Tooltip");
			MenuPauseUI.steamButton.onClickedButton += new ClickedButton(MenuPauseUI.onClickedSteamButton);
			MenuPauseUI.steamButton.fontSize = 3;
			MenuPauseUI.steamButton.iconColor = 2;
			MenuPauseUI.container.AddChild(MenuPauseUI.steamButton);
			MenuPauseUI.forumButton = new SleekButtonIcon(MenuPauseUI.icons.load<Texture2D>("Forum"));
			MenuPauseUI.forumButton.PositionOffset_X = -100f;
			MenuPauseUI.forumButton.PositionOffset_Y = 35f;
			MenuPauseUI.forumButton.PositionScale_X = 0.5f;
			MenuPauseUI.forumButton.PositionScale_Y = 0.5f;
			MenuPauseUI.forumButton.SizeOffset_X = 200f;
			MenuPauseUI.forumButton.SizeOffset_Y = 50f;
			MenuPauseUI.forumButton.text = MenuPauseUI.localization.format("Forum_Button");
			MenuPauseUI.forumButton.tooltip = MenuPauseUI.localization.format("Forum_Button_Tooltip");
			MenuPauseUI.forumButton.onClickedButton += new ClickedButton(MenuPauseUI.onClickedForumButton);
			MenuPauseUI.forumButton.fontSize = 3;
			MenuPauseUI.forumButton.iconColor = 2;
			MenuPauseUI.container.AddChild(MenuPauseUI.forumButton);
			MenuPauseUI.blogButton = new SleekButtonIcon(MenuPauseUI.icons.load<Texture2D>("Blog"));
			MenuPauseUI.blogButton.PositionOffset_X = -100f;
			MenuPauseUI.blogButton.PositionOffset_Y = 95f;
			MenuPauseUI.blogButton.PositionScale_X = 0.5f;
			MenuPauseUI.blogButton.PositionScale_Y = 0.5f;
			MenuPauseUI.blogButton.SizeOffset_X = 200f;
			MenuPauseUI.blogButton.SizeOffset_Y = 50f;
			MenuPauseUI.blogButton.text = MenuPauseUI.localization.format("Blog_Button");
			MenuPauseUI.blogButton.tooltip = MenuPauseUI.localization.format("Blog_Button_Tooltip");
			MenuPauseUI.blogButton.onClickedButton += new ClickedButton(MenuPauseUI.onClickedBlogButton);
			MenuPauseUI.blogButton.fontSize = 3;
			MenuPauseUI.container.AddChild(MenuPauseUI.blogButton);
			MenuPauseUI.wikiButton = new SleekButtonIcon(MenuPauseUI.icons.load<Texture2D>("Wiki"));
			MenuPauseUI.wikiButton.PositionOffset_X = -100f;
			MenuPauseUI.wikiButton.PositionOffset_Y = 155f;
			MenuPauseUI.wikiButton.PositionScale_X = 0.5f;
			MenuPauseUI.wikiButton.PositionScale_Y = 0.5f;
			MenuPauseUI.wikiButton.SizeOffset_X = 200f;
			MenuPauseUI.wikiButton.SizeOffset_Y = 50f;
			MenuPauseUI.wikiButton.text = MenuPauseUI.localization.format("Wiki_Button");
			MenuPauseUI.wikiButton.tooltip = MenuPauseUI.localization.format("Wiki_Button_Tooltip");
			MenuPauseUI.wikiButton.onClickedButton += new ClickedButton(MenuPauseUI.onClickedWikiButton);
			MenuPauseUI.wikiButton.fontSize = 3;
			MenuPauseUI.wikiButton.iconColor = 2;
			MenuPauseUI.container.AddChild(MenuPauseUI.wikiButton);
			MenuPauseUI.creditsButton = new SleekButtonIcon(MenuPauseUI.icons.load<Texture2D>("Credits"));
			MenuPauseUI.creditsButton.PositionOffset_X = -100f;
			MenuPauseUI.creditsButton.PositionOffset_Y = 215f;
			MenuPauseUI.creditsButton.PositionScale_X = 0.5f;
			MenuPauseUI.creditsButton.PositionScale_Y = 0.5f;
			MenuPauseUI.creditsButton.SizeOffset_X = 200f;
			MenuPauseUI.creditsButton.SizeOffset_Y = 50f;
			MenuPauseUI.creditsButton.text = MenuPauseUI.localization.format("Credits_Button");
			MenuPauseUI.creditsButton.tooltip = MenuPauseUI.localization.format("Credits_Button_Tooltip");
			MenuPauseUI.creditsButton.onClickedButton += new ClickedButton(MenuPauseUI.onClickedCreditsButton);
			MenuPauseUI.creditsButton.fontSize = 3;
			MenuPauseUI.creditsButton.iconColor = 2;
			MenuPauseUI.container.AddChild(MenuPauseUI.creditsButton);
		}

		// Token: 0x0400295B RID: 10587
		public static Local localization;

		// Token: 0x0400295C RID: 10588
		public static Bundle icons;

		// Token: 0x0400295D RID: 10589
		private static SleekFullscreenBox container;

		// Token: 0x0400295E RID: 10590
		public static bool active;

		// Token: 0x0400295F RID: 10591
		private static SleekButtonIcon returnButton;

		// Token: 0x04002960 RID: 10592
		private static SleekButtonIconConfirm quitButton;

		// Token: 0x04002961 RID: 10593
		private static SleekButtonIcon supportButton;

		// Token: 0x04002962 RID: 10594
		private static SleekButtonIcon twitterButton;

		// Token: 0x04002963 RID: 10595
		private static SleekButtonIcon steamButton;

		// Token: 0x04002964 RID: 10596
		private static SleekButtonIcon creditsButton;

		// Token: 0x04002965 RID: 10597
		private static SleekButtonIcon forumButton;

		// Token: 0x04002966 RID: 10598
		private static SleekButtonIcon blogButton;

		// Token: 0x04002967 RID: 10599
		private static SleekButtonIcon wikiButton;
	}
}
