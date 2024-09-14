using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200078F RID: 1935
	public class MenuConfigurationUI
	{
		// Token: 0x06004043 RID: 16451 RVA: 0x0014A10E File Offset: 0x0014830E
		public static void open()
		{
			if (MenuConfigurationUI.active)
			{
				return;
			}
			MenuConfigurationUI.active = true;
			MenuConfigurationUI.container.AnimateIntoView();
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x0014A128 File Offset: 0x00148328
		public static void close()
		{
			if (!MenuConfigurationUI.active)
			{
				return;
			}
			MenuConfigurationUI.active = false;
			MenuConfigurationUI.container.AnimateOutOfView(0f, -1f);
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x0014A14C File Offset: 0x0014834C
		private static void onClickedOptionsButton(ISleekElement button)
		{
			MenuConfigurationOptionsUI.open();
			MenuConfigurationUI.close();
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x0014A158 File Offset: 0x00148358
		private static void onClickedDisplayButton(ISleekElement button)
		{
			MenuConfigurationDisplayUI.open();
			MenuConfigurationUI.close();
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x0014A164 File Offset: 0x00148364
		private static void onClickedGraphicsButton(ISleekElement button)
		{
			MenuConfigurationGraphicsUI.open();
			MenuConfigurationUI.close();
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x0014A170 File Offset: 0x00148370
		private static void onClickedControlsButton(ISleekElement button)
		{
			MenuConfigurationControlsUI.open();
			MenuConfigurationUI.close();
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0014A17C File Offset: 0x0014837C
		private static void onClickedAudioButton(ISleekElement button)
		{
			MenuConfigurationUI.audioMenu.open();
			MenuConfigurationUI.close();
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x0014A18D File Offset: 0x0014838D
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuDashboardUI.open();
			MenuTitleUI.open();
			MenuConfigurationUI.close();
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x0014A1A0 File Offset: 0x001483A0
		public MenuConfigurationUI()
		{
			Local local = Localization.read("/Menu/Configuration/MenuConfiguration.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Configuration/MenuConfiguration/MenuConfiguration.unity3d");
			MenuConfigurationUI.container = new SleekFullscreenBox();
			MenuConfigurationUI.container.PositionOffset_X = 10f;
			MenuConfigurationUI.container.PositionOffset_Y = 10f;
			MenuConfigurationUI.container.PositionScale_Y = -1f;
			MenuConfigurationUI.container.SizeOffset_X = -20f;
			MenuConfigurationUI.container.SizeOffset_Y = -20f;
			MenuConfigurationUI.container.SizeScale_X = 1f;
			MenuConfigurationUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuConfigurationUI.container);
			MenuConfigurationUI.active = false;
			int num = -185;
			MenuConfigurationUI.optionsButton = new SleekButtonIcon(bundle.load<Texture2D>("Options"));
			MenuConfigurationUI.optionsButton.PositionOffset_X = -100f;
			MenuConfigurationUI.optionsButton.PositionOffset_Y = (float)num;
			MenuConfigurationUI.optionsButton.PositionScale_X = 0.5f;
			MenuConfigurationUI.optionsButton.PositionScale_Y = 0.5f;
			MenuConfigurationUI.optionsButton.SizeOffset_X = 200f;
			MenuConfigurationUI.optionsButton.SizeOffset_Y = 50f;
			MenuConfigurationUI.optionsButton.text = local.format("Options_Button_Text");
			MenuConfigurationUI.optionsButton.tooltip = local.format("Options_Button_Tooltip");
			MenuConfigurationUI.optionsButton.onClickedButton += new ClickedButton(MenuConfigurationUI.onClickedOptionsButton);
			MenuConfigurationUI.optionsButton.fontSize = 3;
			MenuConfigurationUI.optionsButton.iconColor = 2;
			MenuConfigurationUI.container.AddChild(MenuConfigurationUI.optionsButton);
			num += 60;
			MenuConfigurationUI.displayButton = new SleekButtonIcon(bundle.load<Texture2D>("Display"));
			MenuConfigurationUI.displayButton.PositionOffset_X = -100f;
			MenuConfigurationUI.displayButton.PositionOffset_Y = (float)num;
			MenuConfigurationUI.displayButton.PositionScale_X = 0.5f;
			MenuConfigurationUI.displayButton.PositionScale_Y = 0.5f;
			MenuConfigurationUI.displayButton.SizeOffset_X = 200f;
			MenuConfigurationUI.displayButton.SizeOffset_Y = 50f;
			MenuConfigurationUI.displayButton.text = local.format("Display_Button_Text");
			MenuConfigurationUI.displayButton.tooltip = local.format("Display_Button_Tooltip");
			MenuConfigurationUI.displayButton.onClickedButton += new ClickedButton(MenuConfigurationUI.onClickedDisplayButton);
			MenuConfigurationUI.displayButton.fontSize = 3;
			MenuConfigurationUI.displayButton.iconColor = 2;
			MenuConfigurationUI.container.AddChild(MenuConfigurationUI.displayButton);
			num += 60;
			MenuConfigurationUI.graphicsButton = new SleekButtonIcon(bundle.load<Texture2D>("Graphics"));
			MenuConfigurationUI.graphicsButton.PositionOffset_X = -100f;
			MenuConfigurationUI.graphicsButton.PositionOffset_Y = (float)num;
			MenuConfigurationUI.graphicsButton.PositionScale_X = 0.5f;
			MenuConfigurationUI.graphicsButton.PositionScale_Y = 0.5f;
			MenuConfigurationUI.graphicsButton.SizeOffset_X = 200f;
			MenuConfigurationUI.graphicsButton.SizeOffset_Y = 50f;
			MenuConfigurationUI.graphicsButton.text = local.format("Graphics_Button_Text");
			MenuConfigurationUI.graphicsButton.tooltip = local.format("Graphics_Button_Tooltip");
			MenuConfigurationUI.graphicsButton.onClickedButton += new ClickedButton(MenuConfigurationUI.onClickedGraphicsButton);
			MenuConfigurationUI.graphicsButton.fontSize = 3;
			MenuConfigurationUI.graphicsButton.iconColor = 2;
			MenuConfigurationUI.container.AddChild(MenuConfigurationUI.graphicsButton);
			num += 60;
			MenuConfigurationUI.controlsButton = new SleekButtonIcon(bundle.load<Texture2D>("Controls"));
			MenuConfigurationUI.controlsButton.PositionOffset_X = -100f;
			MenuConfigurationUI.controlsButton.PositionOffset_Y = (float)num;
			MenuConfigurationUI.controlsButton.PositionScale_X = 0.5f;
			MenuConfigurationUI.controlsButton.PositionScale_Y = 0.5f;
			MenuConfigurationUI.controlsButton.SizeOffset_X = 200f;
			MenuConfigurationUI.controlsButton.SizeOffset_Y = 50f;
			MenuConfigurationUI.controlsButton.text = local.format("Controls_Button_Text");
			MenuConfigurationUI.controlsButton.tooltip = local.format("Controls_Button_Tooltip");
			MenuConfigurationUI.controlsButton.onClickedButton += new ClickedButton(MenuConfigurationUI.onClickedControlsButton);
			MenuConfigurationUI.controlsButton.fontSize = 3;
			MenuConfigurationUI.controlsButton.iconColor = 2;
			MenuConfigurationUI.container.AddChild(MenuConfigurationUI.controlsButton);
			num += 60;
			MenuConfigurationUI.audioButton = new SleekButtonIcon(bundle.load<Texture2D>("Audio"));
			MenuConfigurationUI.audioButton.PositionOffset_X = -100f;
			MenuConfigurationUI.audioButton.PositionOffset_Y = (float)num;
			MenuConfigurationUI.audioButton.PositionScale_X = 0.5f;
			MenuConfigurationUI.audioButton.PositionScale_Y = 0.5f;
			MenuConfigurationUI.audioButton.SizeOffset_X = 200f;
			MenuConfigurationUI.audioButton.SizeOffset_Y = 50f;
			MenuConfigurationUI.audioButton.text = local.format("Audio_Button_Text");
			MenuConfigurationUI.audioButton.tooltip = local.format("Audio_Button_Tooltip");
			MenuConfigurationUI.audioButton.onClickedButton += new ClickedButton(MenuConfigurationUI.onClickedAudioButton);
			MenuConfigurationUI.audioButton.fontSize = 3;
			MenuConfigurationUI.audioButton.iconColor = 2;
			MenuConfigurationUI.container.AddChild(MenuConfigurationUI.audioButton);
			num += 60;
			MenuConfigurationUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuConfigurationUI.backButton.PositionOffset_X = -100f;
			MenuConfigurationUI.backButton.PositionOffset_Y = (float)num;
			MenuConfigurationUI.backButton.PositionScale_X = 0.5f;
			MenuConfigurationUI.backButton.PositionScale_Y = 0.5f;
			MenuConfigurationUI.backButton.SizeOffset_X = 200f;
			MenuConfigurationUI.backButton.SizeOffset_Y = 50f;
			MenuConfigurationUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuConfigurationUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuConfigurationUI.backButton.onClickedButton += new ClickedButton(MenuConfigurationUI.onClickedBackButton);
			MenuConfigurationUI.backButton.fontSize = 3;
			MenuConfigurationUI.backButton.iconColor = 2;
			MenuConfigurationUI.container.AddChild(MenuConfigurationUI.backButton);
			num += 60;
			bundle.unload();
			new MenuConfigurationOptionsUI();
			new MenuConfigurationDisplayUI();
			new MenuConfigurationGraphicsUI();
			new MenuConfigurationControlsUI();
			MenuConfigurationUI.audioMenu = new MenuConfigurationAudioUI();
			MenuConfigurationUI.audioMenu.PositionOffset_X = 10f;
			MenuConfigurationUI.audioMenu.PositionOffset_Y = 10f;
			MenuConfigurationUI.audioMenu.PositionScale_Y = 1f;
			MenuConfigurationUI.audioMenu.SizeOffset_X = -20f;
			MenuConfigurationUI.audioMenu.SizeOffset_Y = -20f;
			MenuConfigurationUI.audioMenu.SizeScale_X = 1f;
			MenuConfigurationUI.audioMenu.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuConfigurationUI.audioMenu);
		}

		// Token: 0x0400291E RID: 10526
		private static SleekFullscreenBox container;

		// Token: 0x0400291F RID: 10527
		public static bool active;

		// Token: 0x04002920 RID: 10528
		private static SleekButtonIcon optionsButton;

		// Token: 0x04002921 RID: 10529
		private static SleekButtonIcon displayButton;

		// Token: 0x04002922 RID: 10530
		private static SleekButtonIcon graphicsButton;

		// Token: 0x04002923 RID: 10531
		private static SleekButtonIcon controlsButton;

		// Token: 0x04002924 RID: 10532
		private static SleekButtonIcon audioButton;

		// Token: 0x04002925 RID: 10533
		private static SleekButtonIcon backButton;

		// Token: 0x04002926 RID: 10534
		internal static MenuConfigurationAudioUI audioMenu;
	}
}
