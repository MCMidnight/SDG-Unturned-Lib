using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007B5 RID: 1973
	public class MenuWorkshopLocalizationUI
	{
		// Token: 0x0600423C RID: 16956 RVA: 0x001693A2 File Offset: 0x001675A2
		public static void open()
		{
			if (MenuWorkshopLocalizationUI.active)
			{
				return;
			}
			MenuWorkshopLocalizationUI.active = true;
			Localization.refresh();
			MenuWorkshopLocalizationUI.refresh();
			MenuWorkshopLocalizationUI.container.AnimateIntoView();
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x001693C6 File Offset: 0x001675C6
		public static void close()
		{
			if (!MenuWorkshopLocalizationUI.active)
			{
				return;
			}
			MenuWorkshopLocalizationUI.active = false;
			MenuWorkshopLocalizationUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x001693EC File Offset: 0x001675EC
		private static void refresh()
		{
			MenuWorkshopLocalizationUI.messageBox.RemoveAllChildren();
			for (int i = 0; i < Localization.messages.Count; i++)
			{
				ISleekBox sleekBox = Glazier.Get().CreateBox();
				sleekBox.PositionOffset_Y = (float)(i * 30);
				sleekBox.SizeOffset_Y = 30f;
				sleekBox.SizeScale_X = 1f;
				sleekBox.Text = Localization.messages[i];
				MenuWorkshopLocalizationUI.messageBox.AddChild(sleekBox);
			}
			MenuWorkshopLocalizationUI.messageBox.ContentSizeOffset = new Vector2(0f, (float)(Localization.messages.Count * 30));
			MenuWorkshopLocalizationUI.infoBox.IsVisible = (Localization.messages.Count == 0);
		}

		// Token: 0x0600423F RID: 16959 RVA: 0x00169499 File Offset: 0x00167699
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuWorkshopUI.open();
			MenuWorkshopLocalizationUI.close();
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x001694A5 File Offset: 0x001676A5
		private static void onClickedRefreshButton(ISleekElement button)
		{
			Localization.refresh();
			MenuWorkshopLocalizationUI.refresh();
		}

		// Token: 0x06004241 RID: 16961 RVA: 0x001694B4 File Offset: 0x001676B4
		public MenuWorkshopLocalizationUI()
		{
			MenuWorkshopLocalizationUI.localization = Localization.read("/Menu/Workshop/MenuWorkshopLocalization.dat");
			MenuWorkshopLocalizationUI.container = new SleekFullscreenBox();
			MenuWorkshopLocalizationUI.container.PositionOffset_X = 10f;
			MenuWorkshopLocalizationUI.container.PositionOffset_Y = 10f;
			MenuWorkshopLocalizationUI.container.PositionScale_Y = 1f;
			MenuWorkshopLocalizationUI.container.SizeOffset_X = -20f;
			MenuWorkshopLocalizationUI.container.SizeOffset_Y = -20f;
			MenuWorkshopLocalizationUI.container.SizeScale_X = 1f;
			MenuWorkshopLocalizationUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuWorkshopLocalizationUI.container);
			MenuWorkshopLocalizationUI.active = false;
			MenuWorkshopLocalizationUI.headerBox = Glazier.Get().CreateBox();
			MenuWorkshopLocalizationUI.headerBox.SizeOffset_Y = 50f;
			MenuWorkshopLocalizationUI.headerBox.SizeScale_X = 1f;
			MenuWorkshopLocalizationUI.headerBox.FontSize = 3;
			MenuWorkshopLocalizationUI.headerBox.Text = MenuWorkshopLocalizationUI.localization.format("Header", Provider.language, "English");
			MenuWorkshopLocalizationUI.container.AddChild(MenuWorkshopLocalizationUI.headerBox);
			MenuWorkshopLocalizationUI.infoBox = Glazier.Get().CreateBox();
			MenuWorkshopLocalizationUI.infoBox.PositionOffset_Y = 60f;
			MenuWorkshopLocalizationUI.infoBox.SizeOffset_Y = 50f;
			MenuWorkshopLocalizationUI.infoBox.SizeScale_X = 1f;
			MenuWorkshopLocalizationUI.infoBox.FontSize = 3;
			MenuWorkshopLocalizationUI.infoBox.Text = MenuWorkshopLocalizationUI.localization.format("No_Differences");
			MenuWorkshopLocalizationUI.container.AddChild(MenuWorkshopLocalizationUI.infoBox);
			MenuWorkshopLocalizationUI.infoBox.IsVisible = false;
			MenuWorkshopLocalizationUI.messageBox = Glazier.Get().CreateScrollView();
			MenuWorkshopLocalizationUI.messageBox.PositionOffset_Y = 60f;
			MenuWorkshopLocalizationUI.messageBox.SizeOffset_Y = -120f;
			MenuWorkshopLocalizationUI.messageBox.SizeScale_X = 1f;
			MenuWorkshopLocalizationUI.messageBox.SizeScale_Y = 1f;
			MenuWorkshopLocalizationUI.messageBox.ScaleContentToWidth = true;
			MenuWorkshopLocalizationUI.container.AddChild(MenuWorkshopLocalizationUI.messageBox);
			MenuWorkshopLocalizationUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuWorkshopLocalizationUI.backButton.PositionOffset_Y = -50f;
			MenuWorkshopLocalizationUI.backButton.PositionScale_Y = 1f;
			MenuWorkshopLocalizationUI.backButton.SizeOffset_X = 200f;
			MenuWorkshopLocalizationUI.backButton.SizeOffset_Y = 50f;
			MenuWorkshopLocalizationUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuWorkshopLocalizationUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuWorkshopLocalizationUI.backButton.onClickedButton += new ClickedButton(MenuWorkshopLocalizationUI.onClickedBackButton);
			MenuWorkshopLocalizationUI.backButton.fontSize = 3;
			MenuWorkshopLocalizationUI.backButton.iconColor = 2;
			MenuWorkshopLocalizationUI.container.AddChild(MenuWorkshopLocalizationUI.backButton);
			MenuWorkshopLocalizationUI.refreshButton = Glazier.Get().CreateButton();
			MenuWorkshopLocalizationUI.refreshButton.PositionOffset_X = -200f;
			MenuWorkshopLocalizationUI.refreshButton.PositionOffset_Y = -50f;
			MenuWorkshopLocalizationUI.refreshButton.PositionScale_X = 1f;
			MenuWorkshopLocalizationUI.refreshButton.PositionScale_Y = 1f;
			MenuWorkshopLocalizationUI.refreshButton.SizeOffset_X = 200f;
			MenuWorkshopLocalizationUI.refreshButton.SizeOffset_Y = 50f;
			MenuWorkshopLocalizationUI.refreshButton.Text = MenuWorkshopLocalizationUI.localization.format("Refresh");
			MenuWorkshopLocalizationUI.refreshButton.TooltipText = MenuWorkshopLocalizationUI.localization.format("Refresh_Tooltip");
			MenuWorkshopLocalizationUI.refreshButton.OnClicked += new ClickedButton(MenuWorkshopLocalizationUI.onClickedRefreshButton);
			MenuWorkshopLocalizationUI.refreshButton.FontSize = 3;
			MenuWorkshopLocalizationUI.container.AddChild(MenuWorkshopLocalizationUI.refreshButton);
		}

		// Token: 0x04002B6F RID: 11119
		private static Local localization;

		// Token: 0x04002B70 RID: 11120
		private static SleekFullscreenBox container;

		// Token: 0x04002B71 RID: 11121
		public static bool active;

		// Token: 0x04002B72 RID: 11122
		private static SleekButtonIcon backButton;

		// Token: 0x04002B73 RID: 11123
		private static ISleekButton refreshButton;

		// Token: 0x04002B74 RID: 11124
		private static ISleekBox headerBox;

		// Token: 0x04002B75 RID: 11125
		private static ISleekBox infoBox;

		// Token: 0x04002B76 RID: 11126
		private static ISleekScrollView messageBox;
	}
}
