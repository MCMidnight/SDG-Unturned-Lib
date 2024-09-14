using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x020007A6 RID: 1958
	public class MenuServerPasswordUI
	{
		// Token: 0x0600419C RID: 16796 RVA: 0x0015F75C File Offset: 0x0015D95C
		public static void open(SteamServerAdvertisement newServerInfo, List<PublishedFileId_t> newExpectedWorkshopItems)
		{
			if (MenuServerPasswordUI.isActive)
			{
				return;
			}
			MenuServerPasswordUI.isActive = true;
			MenuServerPasswordUI.container.AnimateIntoView();
			MenuServerPasswordUI.serverInfo = newServerInfo;
			MenuServerPasswordUI.expectedWorkshopItems = newExpectedWorkshopItems;
			MenuServerPasswordUI.connectButton.IsClickable = false;
			MenuServerPasswordUI.passwordField.Text = string.Empty;
			MenuServerPasswordUI.passwordField.IsPasswordField = true;
			MenuServerPasswordUI.showPasswordToggle.Value = false;
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x0015F7BD File Offset: 0x0015D9BD
		public static void close()
		{
			if (!MenuServerPasswordUI.isActive)
			{
				return;
			}
			MenuServerPasswordUI.isActive = false;
			MenuServerPasswordUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x0015F7E4 File Offset: 0x0015D9E4
		public MenuServerPasswordUI()
		{
			MenuServerPasswordUI.localization = Localization.read("/Menu/Play/MenuServerPassword.dat");
			MenuServerPasswordUI.container = new SleekFullscreenBox();
			MenuServerPasswordUI.container.PositionOffset_X = 10f;
			MenuServerPasswordUI.container.PositionOffset_Y = 10f;
			MenuServerPasswordUI.container.PositionScale_Y = 1f;
			MenuServerPasswordUI.container.SizeOffset_X = -20f;
			MenuServerPasswordUI.container.SizeOffset_Y = -20f;
			MenuServerPasswordUI.container.SizeScale_X = 1f;
			MenuServerPasswordUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuServerPasswordUI.container);
			MenuServerPasswordUI.isActive = false;
			MenuServerPasswordUI.explanationLabel = Glazier.Get().CreateLabel();
			MenuServerPasswordUI.explanationLabel.PositionOffset_Y = -75f;
			MenuServerPasswordUI.explanationLabel.PositionScale_X = 0.25f;
			MenuServerPasswordUI.explanationLabel.PositionScale_Y = 0.5f;
			MenuServerPasswordUI.explanationLabel.SizeScale_X = 0.5f;
			MenuServerPasswordUI.explanationLabel.SizeOffset_Y = 30f;
			MenuServerPasswordUI.explanationLabel.TextContrastContext = 2;
			MenuServerPasswordUI.explanationLabel.Text = MenuServerPasswordUI.localization.format("Explanation");
			MenuServerPasswordUI.container.AddChild(MenuServerPasswordUI.explanationLabel);
			MenuServerPasswordUI.passwordField = Glazier.Get().CreateStringField();
			MenuServerPasswordUI.passwordField.PositionOffset_X = -100f;
			MenuServerPasswordUI.passwordField.PositionOffset_Y = -35f;
			MenuServerPasswordUI.passwordField.PositionScale_X = 0.5f;
			MenuServerPasswordUI.passwordField.PositionScale_Y = 0.5f;
			MenuServerPasswordUI.passwordField.SizeOffset_X = 200f;
			MenuServerPasswordUI.passwordField.SizeOffset_Y = 30f;
			MenuServerPasswordUI.passwordField.AddLabel(MenuServerPasswordUI.localization.format("Password_Label"), 1);
			MenuServerPasswordUI.passwordField.IsPasswordField = true;
			MenuServerPasswordUI.passwordField.MaxLength = 0;
			MenuServerPasswordUI.passwordField.OnTextChanged += new Typed(MenuServerPasswordUI.OnTypedPasswordField);
			MenuServerPasswordUI.passwordField.OnTextSubmitted += new Entered(MenuServerPasswordUI.OnPasswordFieldSubmitted);
			MenuServerPasswordUI.container.AddChild(MenuServerPasswordUI.passwordField);
			MenuServerPasswordUI.showPasswordToggle = Glazier.Get().CreateToggle();
			MenuServerPasswordUI.showPasswordToggle.PositionOffset_X = -100f;
			MenuServerPasswordUI.showPasswordToggle.PositionOffset_Y = 5f;
			MenuServerPasswordUI.showPasswordToggle.PositionScale_X = 0.5f;
			MenuServerPasswordUI.showPasswordToggle.PositionScale_Y = 0.5f;
			MenuServerPasswordUI.showPasswordToggle.SizeOffset_X = 40f;
			MenuServerPasswordUI.showPasswordToggle.SizeOffset_Y = 40f;
			MenuServerPasswordUI.showPasswordToggle.OnValueChanged += new Toggled(MenuServerPasswordUI.OnToggledShowPassword);
			MenuServerPasswordUI.showPasswordToggle.AddLabel(MenuServerPasswordUI.localization.format("Show_Password_Label"), 1);
			MenuServerPasswordUI.container.AddChild(MenuServerPasswordUI.showPasswordToggle);
			MenuServerPasswordUI.connectButton = Glazier.Get().CreateButton();
			MenuServerPasswordUI.connectButton.PositionOffset_X = -100f;
			MenuServerPasswordUI.connectButton.PositionOffset_Y = 55f;
			MenuServerPasswordUI.connectButton.PositionScale_X = 0.5f;
			MenuServerPasswordUI.connectButton.PositionScale_Y = 0.5f;
			MenuServerPasswordUI.connectButton.SizeOffset_X = 200f;
			MenuServerPasswordUI.connectButton.SizeOffset_Y = 30f;
			MenuServerPasswordUI.connectButton.Text = MenuServerPasswordUI.localization.format("Connect_Button");
			MenuServerPasswordUI.connectButton.TooltipText = MenuServerPasswordUI.localization.format("Connect_Button");
			MenuServerPasswordUI.connectButton.OnClicked += new ClickedButton(MenuServerPasswordUI.OnClickedConnectButton);
			MenuServerPasswordUI.container.AddChild(MenuServerPasswordUI.connectButton);
			MenuServerPasswordUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuServerPasswordUI.backButton.PositionOffset_Y = -50f;
			MenuServerPasswordUI.backButton.PositionScale_Y = 1f;
			MenuServerPasswordUI.backButton.SizeOffset_X = 200f;
			MenuServerPasswordUI.backButton.SizeOffset_Y = 50f;
			MenuServerPasswordUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuServerPasswordUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuServerPasswordUI.backButton.onClickedButton += new ClickedButton(MenuServerPasswordUI.OnClickedBackButton);
			MenuServerPasswordUI.backButton.fontSize = 3;
			MenuServerPasswordUI.backButton.iconColor = 2;
			MenuServerPasswordUI.container.AddChild(MenuServerPasswordUI.backButton);
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x0015FC20 File Offset: 0x0015DE20
		private static void OnClickedConnectButton(ISleekElement button)
		{
			if (!string.IsNullOrEmpty(MenuServerPasswordUI.passwordField.Text))
			{
				Provider.connect(new ServerConnectParameters(new IPv4Address(MenuServerPasswordUI.serverInfo.ip), MenuServerPasswordUI.serverInfo.queryPort, MenuServerPasswordUI.serverInfo.connectionPort, MenuServerPasswordUI.passwordField.Text), MenuServerPasswordUI.serverInfo, MenuServerPasswordUI.expectedWorkshopItems);
			}
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x0015FC7F File Offset: 0x0015DE7F
		private static void OnToggledShowPassword(ISleekToggle toggle, bool show)
		{
			MenuServerPasswordUI.passwordField.IsPasswordField = !show;
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x0015FC8F File Offset: 0x0015DE8F
		private static void OnTypedPasswordField(ISleekField field, string text)
		{
			MenuServerPasswordUI.connectButton.IsClickable = !string.IsNullOrEmpty(text);
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x0015FCA4 File Offset: 0x0015DEA4
		private static void OnPasswordFieldSubmitted(ISleekField field)
		{
			MenuServerPasswordUI.OnClickedConnectButton(MenuServerPasswordUI.connectButton);
		}

		// Token: 0x060041A3 RID: 16803 RVA: 0x0015FCB0 File Offset: 0x0015DEB0
		private static void OnClickedBackButton(ISleekElement button)
		{
			MenuPlayServerInfoUI.OpenWithoutRefresh();
			MenuServerPasswordUI.close();
		}

		// Token: 0x04002A8B RID: 10891
		private static Local localization;

		// Token: 0x04002A8C RID: 10892
		private static SleekFullscreenBox container;

		// Token: 0x04002A8D RID: 10893
		public static bool isActive;

		// Token: 0x04002A8E RID: 10894
		private static SteamServerAdvertisement serverInfo;

		// Token: 0x04002A8F RID: 10895
		private static List<PublishedFileId_t> expectedWorkshopItems;

		// Token: 0x04002A90 RID: 10896
		private static SleekButtonIcon backButton;

		// Token: 0x04002A91 RID: 10897
		private static ISleekLabel explanationLabel;

		// Token: 0x04002A92 RID: 10898
		private static ISleekField passwordField;

		// Token: 0x04002A93 RID: 10899
		private static ISleekToggle showPasswordToggle;

		// Token: 0x04002A94 RID: 10900
		private static ISleekButton connectButton;
	}
}
