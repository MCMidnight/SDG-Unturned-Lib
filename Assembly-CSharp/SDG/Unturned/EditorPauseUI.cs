using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200077B RID: 1915
	public class EditorPauseUI
	{
		// Token: 0x06003EC6 RID: 16070 RVA: 0x0013526F File Offset: 0x0013346F
		public static void open()
		{
			if (EditorPauseUI.active)
			{
				return;
			}
			EditorPauseUI.active = true;
			EditorPauseUI.container.AnimateIntoView();
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x00135289 File Offset: 0x00133489
		public static void close()
		{
			if (!EditorPauseUI.active)
			{
				return;
			}
			EditorPauseUI.active = false;
			EditorPauseUI.exitButton.reset();
			EditorPauseUI.quitButton.reset();
			EditorPauseUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x001352C1 File Offset: 0x001334C1
		private static void onClickedSaveButton(ISleekElement button)
		{
			Level.save();
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x001352C8 File Offset: 0x001334C8
		private static void onClickedMapButton(ISleekElement button)
		{
			Level.CaptureSatelliteImage();
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x001352CF File Offset: 0x001334CF
		private static void onClickedChartButton(ISleekElement button)
		{
			Level.CaptureChartImage();
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x001352D8 File Offset: 0x001334D8
		private static void onClickedLegacyButton(ISleekElement button)
		{
			ushort value = EditorPauseUI.legacyIDField.Value;
			if (value == 0)
			{
				return;
			}
			SpawnTableTool.export(value, true);
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x001352FC File Offset: 0x001334FC
		private static void onClickedProxyButton(ISleekElement button)
		{
			ushort value = EditorPauseUI.proxyIDField.Value;
			if (value == 0)
			{
				return;
			}
			SpawnTableTool.export(value, false);
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x0013531F File Offset: 0x0013351F
		private static void onClickedOptionsButton(ISleekElement button)
		{
			EditorPauseUI.close();
			MenuConfigurationOptionsUI.open();
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x0013532B File Offset: 0x0013352B
		private static void onClickedDisplayButton(ISleekElement button)
		{
			EditorPauseUI.close();
			MenuConfigurationDisplayUI.open();
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x00135337 File Offset: 0x00133537
		private static void onClickedGraphicsButton(ISleekElement button)
		{
			EditorPauseUI.close();
			MenuConfigurationGraphicsUI.open();
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x00135343 File Offset: 0x00133543
		private static void onClickedControlsButton(ISleekElement button)
		{
			EditorPauseUI.close();
			MenuConfigurationControlsUI.open();
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0013534F File Offset: 0x0013354F
		private static void onClickedAudioButton(ISleekElement button)
		{
			EditorPauseUI.close();
			EditorPauseUI.audioMenu.open();
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x00135360 File Offset: 0x00133560
		private static void onClickedExitButton(SleekButtonIconConfirm button)
		{
			Level.exit();
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x00135367 File Offset: 0x00133567
		private static void onClickedQuitButton(SleekButtonIconConfirm button)
		{
			Provider.QuitGame("clicked quit in level editor");
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x00135374 File Offset: 0x00133574
		public EditorPauseUI()
		{
			Local local = Localization.read("/Editor/EditorPause.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorPause/EditorPause.unity3d");
			EditorPauseUI.container = new SleekFullscreenBox();
			EditorPauseUI.container.PositionOffset_X = 10f;
			EditorPauseUI.container.PositionOffset_Y = 10f;
			EditorPauseUI.container.PositionScale_X = 1f;
			EditorPauseUI.container.SizeOffset_X = -20f;
			EditorPauseUI.container.SizeOffset_Y = -20f;
			EditorPauseUI.container.SizeScale_X = 1f;
			EditorPauseUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorPauseUI.container);
			EditorPauseUI.active = false;
			EditorPauseUI.saveButton = new SleekButtonIcon(bundle.load<Texture2D>("Save"));
			EditorPauseUI.saveButton.PositionOffset_X = -100f;
			EditorPauseUI.saveButton.PositionOffset_Y = -115f;
			EditorPauseUI.saveButton.PositionScale_X = 0.5f;
			EditorPauseUI.saveButton.PositionScale_Y = 0.5f;
			EditorPauseUI.saveButton.SizeOffset_X = 200f;
			EditorPauseUI.saveButton.SizeOffset_Y = 30f;
			EditorPauseUI.saveButton.text = local.format("Save_Button");
			EditorPauseUI.saveButton.tooltip = local.format("Save_Button_Tooltip");
			EditorPauseUI.saveButton.onClickedButton += new ClickedButton(EditorPauseUI.onClickedSaveButton);
			EditorPauseUI.container.AddChild(EditorPauseUI.saveButton);
			EditorPauseUI.mapButton = new SleekButtonIcon(bundle.load<Texture2D>("Map"));
			EditorPauseUI.mapButton.PositionOffset_X = -100f;
			EditorPauseUI.mapButton.PositionOffset_Y = -75f;
			EditorPauseUI.mapButton.PositionScale_X = 0.5f;
			EditorPauseUI.mapButton.PositionScale_Y = 0.5f;
			EditorPauseUI.mapButton.SizeOffset_X = 200f;
			EditorPauseUI.mapButton.SizeOffset_Y = 30f;
			EditorPauseUI.mapButton.text = local.format("Map_Button");
			EditorPauseUI.mapButton.tooltip = local.format("Map_Button_Tooltip");
			EditorPauseUI.mapButton.onClickedButton += new ClickedButton(EditorPauseUI.onClickedMapButton);
			EditorPauseUI.container.AddChild(EditorPauseUI.mapButton);
			EditorPauseUI.chartButton = new SleekButtonIcon(bundle.load<Texture2D>("Chart"));
			EditorPauseUI.chartButton.PositionOffset_X = -100f;
			EditorPauseUI.chartButton.PositionOffset_Y = -35f;
			EditorPauseUI.chartButton.PositionScale_X = 0.5f;
			EditorPauseUI.chartButton.PositionScale_Y = 0.5f;
			EditorPauseUI.chartButton.SizeOffset_X = 200f;
			EditorPauseUI.chartButton.SizeOffset_Y = 30f;
			EditorPauseUI.chartButton.text = local.format("Chart_Button");
			EditorPauseUI.chartButton.tooltip = local.format("Chart_Button_Tooltip");
			EditorPauseUI.chartButton.onClickedButton += new ClickedButton(EditorPauseUI.onClickedChartButton);
			EditorPauseUI.container.AddChild(EditorPauseUI.chartButton);
			EditorPauseUI.legacyIDField = Glazier.Get().CreateUInt16Field();
			EditorPauseUI.legacyIDField.PositionOffset_X = -100f;
			EditorPauseUI.legacyIDField.PositionOffset_Y = 5f;
			EditorPauseUI.legacyIDField.PositionScale_X = 0.5f;
			EditorPauseUI.legacyIDField.PositionScale_Y = 0.5f;
			EditorPauseUI.legacyIDField.SizeOffset_X = 50f;
			EditorPauseUI.legacyIDField.SizeOffset_Y = 30f;
			EditorPauseUI.container.AddChild(EditorPauseUI.legacyIDField);
			EditorPauseUI.legacyButton = Glazier.Get().CreateButton();
			EditorPauseUI.legacyButton.PositionOffset_X = -40f;
			EditorPauseUI.legacyButton.PositionOffset_Y = 5f;
			EditorPauseUI.legacyButton.PositionScale_X = 0.5f;
			EditorPauseUI.legacyButton.PositionScale_Y = 0.5f;
			EditorPauseUI.legacyButton.SizeOffset_X = 140f;
			EditorPauseUI.legacyButton.SizeOffset_Y = 30f;
			EditorPauseUI.legacyButton.Text = local.format("Legacy_Spawns");
			EditorPauseUI.legacyButton.TooltipText = local.format("Legacy_Spawns_Tooltip");
			EditorPauseUI.legacyButton.OnClicked += new ClickedButton(EditorPauseUI.onClickedLegacyButton);
			EditorPauseUI.container.AddChild(EditorPauseUI.legacyButton);
			EditorPauseUI.proxyIDField = Glazier.Get().CreateUInt16Field();
			EditorPauseUI.proxyIDField.PositionOffset_X = -100f;
			EditorPauseUI.proxyIDField.PositionOffset_Y = 45f;
			EditorPauseUI.proxyIDField.PositionScale_X = 0.5f;
			EditorPauseUI.proxyIDField.PositionScale_Y = 0.5f;
			EditorPauseUI.proxyIDField.SizeOffset_X = 50f;
			EditorPauseUI.proxyIDField.SizeOffset_Y = 30f;
			EditorPauseUI.container.AddChild(EditorPauseUI.proxyIDField);
			EditorPauseUI.proxyButton = Glazier.Get().CreateButton();
			EditorPauseUI.proxyButton.PositionOffset_X = -40f;
			EditorPauseUI.proxyButton.PositionOffset_Y = 45f;
			EditorPauseUI.proxyButton.PositionScale_X = 0.5f;
			EditorPauseUI.proxyButton.PositionScale_Y = 0.5f;
			EditorPauseUI.proxyButton.SizeOffset_X = 140f;
			EditorPauseUI.proxyButton.SizeOffset_Y = 30f;
			EditorPauseUI.proxyButton.Text = local.format("Proxy_Spawns");
			EditorPauseUI.proxyButton.TooltipText = local.format("Proxy_Spawns_Tooltip");
			EditorPauseUI.proxyButton.OnClicked += new ClickedButton(EditorPauseUI.onClickedProxyButton);
			EditorPauseUI.container.AddChild(EditorPauseUI.proxyButton);
			Local local2 = Localization.read("/Player/PlayerPause.dat");
			Bundle bundle2 = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerPause/PlayerPause.unity3d");
			EditorPauseUI.optionsButton = new SleekButtonIcon(bundle2.load<Texture2D>("Options"));
			EditorPauseUI.optionsButton.PositionOffset_X = 110f;
			EditorPauseUI.optionsButton.PositionOffset_Y = -115f;
			EditorPauseUI.optionsButton.PositionScale_X = 0.5f;
			EditorPauseUI.optionsButton.PositionScale_Y = 0.5f;
			EditorPauseUI.optionsButton.SizeOffset_X = 200f;
			EditorPauseUI.optionsButton.SizeOffset_Y = 50f;
			EditorPauseUI.optionsButton.text = local2.format("Options_Button_Text");
			EditorPauseUI.optionsButton.tooltip = local2.format("Options_Button_Tooltip");
			EditorPauseUI.optionsButton.onClickedButton += new ClickedButton(EditorPauseUI.onClickedOptionsButton);
			EditorPauseUI.optionsButton.iconColor = 2;
			EditorPauseUI.optionsButton.fontSize = 3;
			EditorPauseUI.container.AddChild(EditorPauseUI.optionsButton);
			EditorPauseUI.displayButton = new SleekButtonIcon(bundle2.load<Texture2D>("Display"));
			EditorPauseUI.displayButton.PositionOffset_X = 110f;
			EditorPauseUI.displayButton.PositionOffset_Y = -55f;
			EditorPauseUI.displayButton.PositionScale_X = 0.5f;
			EditorPauseUI.displayButton.PositionScale_Y = 0.5f;
			EditorPauseUI.displayButton.SizeOffset_X = 200f;
			EditorPauseUI.displayButton.SizeOffset_Y = 50f;
			EditorPauseUI.displayButton.text = local2.format("Display_Button_Text");
			EditorPauseUI.displayButton.tooltip = local2.format("Display_Button_Tooltip");
			EditorPauseUI.displayButton.iconColor = 2;
			EditorPauseUI.displayButton.onClickedButton += new ClickedButton(EditorPauseUI.onClickedDisplayButton);
			EditorPauseUI.displayButton.fontSize = 3;
			EditorPauseUI.container.AddChild(EditorPauseUI.displayButton);
			EditorPauseUI.graphicsButton = new SleekButtonIcon(bundle2.load<Texture2D>("Graphics"));
			EditorPauseUI.graphicsButton.PositionOffset_X = 110f;
			EditorPauseUI.graphicsButton.PositionOffset_Y = 5f;
			EditorPauseUI.graphicsButton.PositionScale_X = 0.5f;
			EditorPauseUI.graphicsButton.PositionScale_Y = 0.5f;
			EditorPauseUI.graphicsButton.SizeOffset_X = 200f;
			EditorPauseUI.graphicsButton.SizeOffset_Y = 50f;
			EditorPauseUI.graphicsButton.text = local2.format("Graphics_Button_Text");
			EditorPauseUI.graphicsButton.tooltip = local2.format("Graphics_Button_Tooltip");
			EditorPauseUI.graphicsButton.iconColor = 2;
			EditorPauseUI.graphicsButton.onClickedButton += new ClickedButton(EditorPauseUI.onClickedGraphicsButton);
			EditorPauseUI.graphicsButton.fontSize = 3;
			EditorPauseUI.container.AddChild(EditorPauseUI.graphicsButton);
			EditorPauseUI.controlsButton = new SleekButtonIcon(bundle2.load<Texture2D>("Controls"));
			EditorPauseUI.controlsButton.PositionOffset_X = 110f;
			EditorPauseUI.controlsButton.PositionOffset_Y = 65f;
			EditorPauseUI.controlsButton.PositionScale_X = 0.5f;
			EditorPauseUI.controlsButton.PositionScale_Y = 0.5f;
			EditorPauseUI.controlsButton.SizeOffset_X = 200f;
			EditorPauseUI.controlsButton.SizeOffset_Y = 50f;
			EditorPauseUI.controlsButton.text = local2.format("Controls_Button_Text");
			EditorPauseUI.controlsButton.tooltip = local2.format("Controls_Button_Tooltip");
			EditorPauseUI.controlsButton.iconColor = 2;
			EditorPauseUI.controlsButton.onClickedButton += new ClickedButton(EditorPauseUI.onClickedControlsButton);
			EditorPauseUI.controlsButton.fontSize = 3;
			EditorPauseUI.container.AddChild(EditorPauseUI.controlsButton);
			EditorPauseUI.audioButton = new SleekButtonIcon(bundle2.load<Texture2D>("Audio"));
			EditorPauseUI.audioButton.PositionOffset_X = 110f;
			EditorPauseUI.audioButton.PositionOffset_Y = 125f;
			EditorPauseUI.audioButton.PositionScale_X = 0.5f;
			EditorPauseUI.audioButton.PositionScale_Y = 0.5f;
			EditorPauseUI.audioButton.SizeOffset_X = 200f;
			EditorPauseUI.audioButton.SizeOffset_Y = 50f;
			EditorPauseUI.audioButton.text = local2.format("Audio_Button_Text");
			EditorPauseUI.audioButton.tooltip = local2.format("Audio_Button_Tooltip");
			EditorPauseUI.audioButton.iconColor = 2;
			EditorPauseUI.audioButton.onClickedButton += new ClickedButton(EditorPauseUI.onClickedAudioButton);
			EditorPauseUI.audioButton.fontSize = 3;
			EditorPauseUI.container.AddChild(EditorPauseUI.audioButton);
			bundle2.unload();
			EditorPauseUI.exitButton = new SleekButtonIconConfirm(bundle.load<Texture2D>("Exit"), local.format("Exit_Button"), local.format("Exit_Button_Tooltip"), "Cancel", string.Empty);
			EditorPauseUI.exitButton.PositionOffset_X = -100f;
			EditorPauseUI.exitButton.PositionOffset_Y = 85f;
			EditorPauseUI.exitButton.PositionScale_X = 0.5f;
			EditorPauseUI.exitButton.PositionScale_Y = 0.5f;
			EditorPauseUI.exitButton.SizeOffset_X = 200f;
			EditorPauseUI.exitButton.SizeOffset_Y = 30f;
			EditorPauseUI.exitButton.text = local.format("Exit_Button");
			EditorPauseUI.exitButton.tooltip = local.format("Exit_Button_Tooltip");
			SleekButtonIconConfirm sleekButtonIconConfirm = EditorPauseUI.exitButton;
			sleekButtonIconConfirm.onConfirmed = (Confirm)Delegate.Combine(sleekButtonIconConfirm.onConfirmed, new Confirm(EditorPauseUI.onClickedExitButton));
			EditorPauseUI.container.AddChild(EditorPauseUI.exitButton);
			EditorPauseUI.quitButton = new SleekButtonIconConfirm(MenuPauseUI.icons.load<Texture2D>("Quit"), MenuPauseUI.localization.format("Exit_Button"), MenuPauseUI.localization.format("Exit_Button_Tooltip"), "Cancel", string.Empty);
			EditorPauseUI.quitButton.PositionOffset_X = -100f;
			EditorPauseUI.quitButton.PositionOffset_Y = 125f;
			EditorPauseUI.quitButton.PositionScale_X = 0.5f;
			EditorPauseUI.quitButton.PositionScale_Y = 0.5f;
			EditorPauseUI.quitButton.SizeOffset_X = 200f;
			EditorPauseUI.quitButton.SizeOffset_Y = 50f;
			EditorPauseUI.quitButton.text = MenuPauseUI.localization.format("Exit_Button");
			EditorPauseUI.quitButton.tooltip = MenuPauseUI.localization.format("Exit_Button_Tooltip");
			SleekButtonIconConfirm sleekButtonIconConfirm2 = EditorPauseUI.quitButton;
			sleekButtonIconConfirm2.onConfirmed = (Confirm)Delegate.Combine(sleekButtonIconConfirm2.onConfirmed, new Confirm(EditorPauseUI.onClickedQuitButton));
			EditorPauseUI.quitButton.fontSize = 3;
			EditorPauseUI.quitButton.iconColor = 2;
			EditorPauseUI.container.AddChild(EditorPauseUI.quitButton);
			new MenuConfigurationOptionsUI();
			new MenuConfigurationDisplayUI();
			new MenuConfigurationGraphicsUI();
			new MenuConfigurationControlsUI();
			EditorPauseUI.audioMenu = new MenuConfigurationAudioUI();
			EditorPauseUI.audioMenu.PositionOffset_X = 10f;
			EditorPauseUI.audioMenu.PositionOffset_Y = 10f;
			EditorPauseUI.audioMenu.PositionScale_Y = 1f;
			EditorPauseUI.audioMenu.SizeOffset_X = -20f;
			EditorPauseUI.audioMenu.SizeOffset_Y = -20f;
			EditorPauseUI.audioMenu.SizeScale_X = 1f;
			EditorPauseUI.audioMenu.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorPauseUI.audioMenu);
			bundle.unload();
		}

		// Token: 0x04002799 RID: 10137
		private static SleekFullscreenBox container;

		// Token: 0x0400279A RID: 10138
		public static bool active;

		// Token: 0x0400279B RID: 10139
		private static SleekButtonIcon saveButton;

		// Token: 0x0400279C RID: 10140
		private static SleekButtonIcon mapButton;

		// Token: 0x0400279D RID: 10141
		private static SleekButtonIconConfirm exitButton;

		// Token: 0x0400279E RID: 10142
		private static SleekButtonIconConfirm quitButton;

		// Token: 0x0400279F RID: 10143
		private static ISleekUInt16Field legacyIDField;

		// Token: 0x040027A0 RID: 10144
		private static ISleekButton legacyButton;

		// Token: 0x040027A1 RID: 10145
		private static ISleekUInt16Field proxyIDField;

		// Token: 0x040027A2 RID: 10146
		private static ISleekButton proxyButton;

		// Token: 0x040027A3 RID: 10147
		private static SleekButtonIcon chartButton;

		// Token: 0x040027A4 RID: 10148
		private static SleekButtonIcon optionsButton;

		// Token: 0x040027A5 RID: 10149
		private static SleekButtonIcon displayButton;

		// Token: 0x040027A6 RID: 10150
		private static SleekButtonIcon graphicsButton;

		// Token: 0x040027A7 RID: 10151
		private static SleekButtonIcon controlsButton;

		// Token: 0x040027A8 RID: 10152
		private static SleekButtonIcon audioButton;

		// Token: 0x040027A9 RID: 10153
		internal static MenuConfigurationAudioUI audioMenu;
	}
}
