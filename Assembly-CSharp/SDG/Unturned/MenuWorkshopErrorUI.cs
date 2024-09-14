using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007B4 RID: 1972
	public class MenuWorkshopErrorUI
	{
		// Token: 0x06004234 RID: 16948 RVA: 0x00168EBD File Offset: 0x001670BD
		public static void open()
		{
			if (MenuWorkshopErrorUI.active)
			{
				return;
			}
			MenuWorkshopErrorUI.active = true;
			MenuWorkshopErrorUI.refresh();
			MenuWorkshopErrorUI.container.AnimateIntoView();
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x00168EDC File Offset: 0x001670DC
		public static void close()
		{
			if (!MenuWorkshopErrorUI.active)
			{
				return;
			}
			MenuWorkshopErrorUI.active = false;
			MenuWorkshopErrorUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x00168F00 File Offset: 0x00167100
		private static void refresh()
		{
			MenuWorkshopErrorUI.errorBox.NotifyDataChanged();
			List<string> reportedErrorsList = Assets.getReportedErrorsList();
			MenuWorkshopErrorUI.infoBox.IsVisible = (reportedErrorsList.Count == 0);
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x00168F30 File Offset: 0x00167130
		private static void OnClickedBrowseLogs(ISleekElement button)
		{
			ReadWrite.OpenFileBrowser(ReadWrite.folderPath(Logs.getLogFilePath()));
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x00168F41 File Offset: 0x00167141
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuWorkshopUI.open();
			MenuWorkshopErrorUI.close();
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x00168F4D File Offset: 0x0016714D
		private static void onClickedRefreshButton(ISleekElement button)
		{
			MenuWorkshopErrorUI.refresh();
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x00168F54 File Offset: 0x00167154
		private static ISleekElement onCreateErrorMessage(string message)
		{
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.Text = message;
			return sleekBox;
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x00168F68 File Offset: 0x00167168
		public MenuWorkshopErrorUI()
		{
			MenuWorkshopErrorUI.localization = Localization.read("/Menu/Workshop/MenuWorkshopError.dat");
			MenuWorkshopErrorUI.container = new SleekFullscreenBox();
			MenuWorkshopErrorUI.container.PositionOffset_X = 10f;
			MenuWorkshopErrorUI.container.PositionOffset_Y = 10f;
			MenuWorkshopErrorUI.container.PositionScale_Y = 1f;
			MenuWorkshopErrorUI.container.SizeOffset_X = -20f;
			MenuWorkshopErrorUI.container.SizeOffset_Y = -20f;
			MenuWorkshopErrorUI.container.SizeScale_X = 1f;
			MenuWorkshopErrorUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuWorkshopErrorUI.container);
			MenuWorkshopErrorUI.active = false;
			MenuWorkshopErrorUI.headerBox = Glazier.Get().CreateBox();
			MenuWorkshopErrorUI.headerBox.SizeOffset_Y = 50f;
			MenuWorkshopErrorUI.headerBox.SizeScale_X = 1f;
			MenuWorkshopErrorUI.headerBox.FontSize = 3;
			MenuWorkshopErrorUI.headerBox.Text = MenuWorkshopErrorUI.localization.format("Header");
			MenuWorkshopErrorUI.container.AddChild(MenuWorkshopErrorUI.headerBox);
			if (ReadWrite.SupportsOpeningFileBrowser)
			{
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_X = -210f;
				sleekButton.PositionOffset_Y = -15f;
				sleekButton.PositionScale_X = 1f;
				sleekButton.PositionScale_Y = 0.5f;
				sleekButton.SizeOffset_X = 200f;
				sleekButton.SizeOffset_Y = 30f;
				sleekButton.Text = MenuWorkshopErrorUI.localization.format("BrowseLogs_Label");
				sleekButton.TooltipText = MenuWorkshopErrorUI.localization.format("BrowseLogs_Tooltip");
				sleekButton.OnClicked += new ClickedButton(MenuWorkshopErrorUI.OnClickedBrowseLogs);
				MenuWorkshopErrorUI.headerBox.AddChild(sleekButton);
			}
			MenuWorkshopErrorUI.infoBox = Glazier.Get().CreateBox();
			MenuWorkshopErrorUI.infoBox.PositionOffset_Y = 60f;
			MenuWorkshopErrorUI.infoBox.SizeOffset_Y = 50f;
			MenuWorkshopErrorUI.infoBox.SizeScale_X = 1f;
			MenuWorkshopErrorUI.infoBox.FontSize = 3;
			MenuWorkshopErrorUI.infoBox.Text = MenuWorkshopErrorUI.localization.format("No_Errors");
			MenuWorkshopErrorUI.container.AddChild(MenuWorkshopErrorUI.infoBox);
			MenuWorkshopErrorUI.infoBox.IsVisible = false;
			MenuWorkshopErrorUI.errorBox = new SleekList<string>();
			MenuWorkshopErrorUI.errorBox.PositionOffset_Y = 60f;
			MenuWorkshopErrorUI.errorBox.SizeOffset_Y = -120f;
			MenuWorkshopErrorUI.errorBox.SizeScale_X = 1f;
			MenuWorkshopErrorUI.errorBox.SizeScale_Y = 1f;
			MenuWorkshopErrorUI.errorBox.itemHeight = 50;
			MenuWorkshopErrorUI.errorBox.onCreateElement = new SleekList<string>.CreateElement(MenuWorkshopErrorUI.onCreateErrorMessage);
			MenuWorkshopErrorUI.errorBox.SetData(Assets.getReportedErrorsList());
			MenuWorkshopErrorUI.container.AddChild(MenuWorkshopErrorUI.errorBox);
			MenuWorkshopErrorUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuWorkshopErrorUI.backButton.PositionOffset_Y = -50f;
			MenuWorkshopErrorUI.backButton.PositionScale_Y = 1f;
			MenuWorkshopErrorUI.backButton.SizeOffset_X = 200f;
			MenuWorkshopErrorUI.backButton.SizeOffset_Y = 50f;
			MenuWorkshopErrorUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuWorkshopErrorUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuWorkshopErrorUI.backButton.onClickedButton += new ClickedButton(MenuWorkshopErrorUI.onClickedBackButton);
			MenuWorkshopErrorUI.backButton.fontSize = 3;
			MenuWorkshopErrorUI.backButton.iconColor = 2;
			MenuWorkshopErrorUI.container.AddChild(MenuWorkshopErrorUI.backButton);
			MenuWorkshopErrorUI.refreshButton = Glazier.Get().CreateButton();
			MenuWorkshopErrorUI.refreshButton.PositionOffset_X = -200f;
			MenuWorkshopErrorUI.refreshButton.PositionOffset_Y = -50f;
			MenuWorkshopErrorUI.refreshButton.PositionScale_X = 1f;
			MenuWorkshopErrorUI.refreshButton.PositionScale_Y = 1f;
			MenuWorkshopErrorUI.refreshButton.SizeOffset_X = 200f;
			MenuWorkshopErrorUI.refreshButton.SizeOffset_Y = 50f;
			MenuWorkshopErrorUI.refreshButton.Text = MenuWorkshopErrorUI.localization.format("Refresh");
			MenuWorkshopErrorUI.refreshButton.TooltipText = MenuWorkshopErrorUI.localization.format("Refresh_Tooltip");
			MenuWorkshopErrorUI.refreshButton.OnClicked += new ClickedButton(MenuWorkshopErrorUI.onClickedRefreshButton);
			MenuWorkshopErrorUI.refreshButton.FontSize = 3;
			MenuWorkshopErrorUI.container.AddChild(MenuWorkshopErrorUI.refreshButton);
		}

		// Token: 0x04002B67 RID: 11111
		private static Local localization;

		// Token: 0x04002B68 RID: 11112
		private static SleekFullscreenBox container;

		// Token: 0x04002B69 RID: 11113
		public static bool active;

		// Token: 0x04002B6A RID: 11114
		private static SleekButtonIcon backButton;

		// Token: 0x04002B6B RID: 11115
		private static ISleekButton refreshButton;

		// Token: 0x04002B6C RID: 11116
		private static ISleekBox headerBox;

		// Token: 0x04002B6D RID: 11117
		private static ISleekBox infoBox;

		// Token: 0x04002B6E RID: 11118
		private static SleekList<string> errorBox;
	}
}
