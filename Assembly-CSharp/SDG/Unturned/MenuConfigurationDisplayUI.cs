using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200078C RID: 1932
	public class MenuConfigurationDisplayUI
	{
		// Token: 0x06003FE0 RID: 16352 RVA: 0x0014489A File Offset: 0x00142A9A
		public static void open()
		{
			if (MenuConfigurationDisplayUI.active)
			{
				return;
			}
			MenuConfigurationDisplayUI.active = true;
			MenuConfigurationDisplayUI.container.AnimateIntoView();
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x001448B4 File Offset: 0x00142AB4
		public static void close()
		{
			if (!MenuConfigurationDisplayUI.active)
			{
				return;
			}
			MenuConfigurationDisplayUI.active = false;
			MenuSettings.SaveGraphicsIfLoaded();
			MenuConfigurationDisplayUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x001448E0 File Offset: 0x00142AE0
		private static void onClickedResolutionButton(ISleekElement button)
		{
			int num = Mathf.FloorToInt((button.PositionOffset_Y - 300f) / 40f);
			Resolution resolution = ScreenEx.GetRecommendedResolutions()[num];
			GraphicsSettings.resolution = new GraphicsSettingsResolution(resolution);
			GraphicsSettings.apply(string.Format("changed resolution to {0} x {1} [{2} Hz]", resolution.width, resolution.height, resolution.refreshRate));
		}

		// Token: 0x06003FE3 RID: 16355 RVA: 0x00144950 File Offset: 0x00142B50
		private static void onSwappedFullscreenState(SleekButtonState button, int index)
		{
			FullScreenMode fullScreenMode;
			switch (index)
			{
			case 0:
				fullScreenMode = FullScreenMode.ExclusiveFullScreen;
				goto IL_1E;
			case 2:
				fullScreenMode = FullScreenMode.Windowed;
				goto IL_1E;
			}
			fullScreenMode = FullScreenMode.FullScreenWindow;
			IL_1E:
			GraphicsSettings.fullscreenMode = fullScreenMode;
			GraphicsSettings.apply("changed fullscreen mode");
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x0014498B File Offset: 0x00142B8B
		private static void onToggledBufferToggle(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.buffer = state;
			GraphicsSettings.apply("changed vsync");
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x0014499D File Offset: 0x00142B9D
		private static void onTypedUserInterfaceScale(ISleekFloat32Field field, float state)
		{
			GraphicsSettings.userInterfaceScale = Mathf.Clamp(state, 0.5f, 2f);
			GraphicsSettings.apply("changed UI scale");
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x001449BE File Offset: 0x00142BBE
		private static void OnToggledTargetFrameRate(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.UseTargetFrameRate = state;
			GraphicsSettings.apply("changed use target frame rate");
			MenuConfigurationDisplayUI.SynchronizeTargetFrameRateVisibility();
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x001449D5 File Offset: 0x00142BD5
		private static void OnTypedTargetFrameRate(ISleekUInt32Field field, uint state)
		{
			GraphicsSettings.TargetFrameRate = (int)state;
			GraphicsSettings.apply("changed target frame rate");
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x001449E7 File Offset: 0x00142BE7
		private static void OnToggledUnfocusedTargetFrameRate(ISleekToggle toggle, bool state)
		{
			GraphicsSettings.UseUnfocusedTargetFrameRate = state;
			GraphicsSettings.apply("changed use unfocused target frame rate");
			MenuConfigurationDisplayUI.SynchronizeTargetFrameRateVisibility();
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x001449FE File Offset: 0x00142BFE
		private static void OnTypedUnfocusedTargetFrameRate(ISleekUInt32Field field, uint state)
		{
			GraphicsSettings.UnfocusedTargetFrameRate = (int)state;
			GraphicsSettings.apply("changed unfocused target frame rate");
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x00144A10 File Offset: 0x00142C10
		private static void SynchronizeTargetFrameRateVisibility()
		{
			MenuConfigurationDisplayUI.targetFrameRateField.IsVisible = GraphicsSettings.UseTargetFrameRate;
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateToggle.IsVisible = MenuConfigurationDisplayUI.targetFrameRateField.IsVisible;
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateField.IsVisible = (GraphicsSettings.UseUnfocusedTargetFrameRate && MenuConfigurationDisplayUI.unfocusedTargetFrameRateToggle.IsVisible);
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x00144A5E File Offset: 0x00142C5E
		private static void onClickedBackButton(ISleekElement button)
		{
			if (Player.player != null)
			{
				PlayerPauseUI.open();
			}
			else if (Level.isEditor)
			{
				EditorPauseUI.open();
			}
			else
			{
				MenuConfigurationUI.open();
			}
			MenuConfigurationDisplayUI.close();
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x00144A8C File Offset: 0x00142C8C
		public MenuConfigurationDisplayUI()
		{
			MenuConfigurationDisplayUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationDisplay.dat");
			MenuConfigurationDisplayUI.container = new SleekFullscreenBox();
			MenuConfigurationDisplayUI.container.PositionOffset_X = 10f;
			MenuConfigurationDisplayUI.container.PositionOffset_Y = 10f;
			MenuConfigurationDisplayUI.container.PositionScale_Y = 1f;
			MenuConfigurationDisplayUI.container.SizeOffset_X = -20f;
			MenuConfigurationDisplayUI.container.SizeOffset_Y = -20f;
			MenuConfigurationDisplayUI.container.SizeScale_X = 1f;
			MenuConfigurationDisplayUI.container.SizeScale_Y = 1f;
			if (Provider.isConnected)
			{
				PlayerUI.container.AddChild(MenuConfigurationDisplayUI.container);
			}
			else if (Level.isEditor)
			{
				EditorUI.window.AddChild(MenuConfigurationDisplayUI.container);
			}
			else
			{
				MenuUI.container.AddChild(MenuConfigurationDisplayUI.container);
			}
			MenuConfigurationDisplayUI.active = false;
			Resolution[] recommendedResolutions = ScreenEx.GetRecommendedResolutions();
			MenuConfigurationDisplayUI.resolutionsBox = Glazier.Get().CreateScrollView();
			MenuConfigurationDisplayUI.resolutionsBox.PositionOffset_X = -200f;
			MenuConfigurationDisplayUI.resolutionsBox.PositionOffset_Y = 100f;
			MenuConfigurationDisplayUI.resolutionsBox.PositionScale_X = 0.5f;
			MenuConfigurationDisplayUI.resolutionsBox.SizeOffset_X = 430f;
			MenuConfigurationDisplayUI.resolutionsBox.SizeOffset_Y = -200f;
			MenuConfigurationDisplayUI.resolutionsBox.SizeScale_Y = 1f;
			MenuConfigurationDisplayUI.resolutionsBox.ScaleContentToWidth = true;
			MenuConfigurationDisplayUI.resolutionsBox.ContentSizeOffset = new Vector2(0f, (float)(300 + recommendedResolutions.Length * 40 - 10));
			MenuConfigurationDisplayUI.container.AddChild(MenuConfigurationDisplayUI.resolutionsBox);
			MenuConfigurationDisplayUI.buttons = new ISleekButton[recommendedResolutions.Length];
			byte b = 0;
			while ((int)b < MenuConfigurationDisplayUI.buttons.Length)
			{
				Resolution resolution = recommendedResolutions[(int)b];
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_Y = (float)(300 + (int)(b * 40));
				sleekButton.SizeOffset_Y = 30f;
				sleekButton.SizeScale_X = 1f;
				sleekButton.OnClicked += new ClickedButton(MenuConfigurationDisplayUI.onClickedResolutionButton);
				sleekButton.Text = string.Concat(new string[]
				{
					resolution.width.ToString(),
					" x ",
					resolution.height.ToString(),
					" [",
					resolution.refreshRate.ToString(),
					"Hz]"
				});
				MenuConfigurationDisplayUI.resolutionsBox.AddChild(sleekButton);
				MenuConfigurationDisplayUI.buttons[(int)b] = sleekButton;
				b += 1;
			}
			MenuConfigurationDisplayUI.fullscreenMode = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationDisplayUI.localization.format("Fullscreen_Mode_Exclusive")),
				new GUIContent(MenuConfigurationDisplayUI.localization.format("Fullscreen_Mode_Borderless")),
				new GUIContent(MenuConfigurationDisplayUI.localization.format("Fullscreen_Mode_Windowed"))
			});
			MenuConfigurationDisplayUI.fullscreenMode.SizeOffset_X = 200f;
			MenuConfigurationDisplayUI.fullscreenMode.SizeOffset_Y = 30f;
			MenuConfigurationDisplayUI.fullscreenMode.AddLabel(MenuConfigurationDisplayUI.localization.format("Fullscreen_Mode_Label"), 1);
			MenuConfigurationDisplayUI.fullscreenMode.tooltip = MenuConfigurationDisplayUI.localization.format("Fullscreen_Mode_Tooltip");
			switch (GraphicsSettings.fullscreenMode)
			{
			case FullScreenMode.ExclusiveFullScreen:
				MenuConfigurationDisplayUI.fullscreenMode.state = 0;
				goto IL_34C;
			case FullScreenMode.Windowed:
				MenuConfigurationDisplayUI.fullscreenMode.state = 2;
				goto IL_34C;
			}
			MenuConfigurationDisplayUI.fullscreenMode.state = 1;
			IL_34C:
			MenuConfigurationDisplayUI.fullscreenMode.onSwappedState = new SwappedState(MenuConfigurationDisplayUI.onSwappedFullscreenState);
			MenuConfigurationDisplayUI.resolutionsBox.AddChild(MenuConfigurationDisplayUI.fullscreenMode);
			MenuConfigurationDisplayUI.bufferToggle = Glazier.Get().CreateToggle();
			MenuConfigurationDisplayUI.bufferToggle.PositionOffset_Y = 50f;
			MenuConfigurationDisplayUI.bufferToggle.SizeOffset_X = 40f;
			MenuConfigurationDisplayUI.bufferToggle.SizeOffset_Y = 40f;
			MenuConfigurationDisplayUI.bufferToggle.AddLabel(MenuConfigurationDisplayUI.localization.format("Buffer_Toggle_Label"), 1);
			MenuConfigurationDisplayUI.bufferToggle.Value = GraphicsSettings.buffer;
			MenuConfigurationDisplayUI.bufferToggle.OnValueChanged += new Toggled(MenuConfigurationDisplayUI.onToggledBufferToggle);
			MenuConfigurationDisplayUI.resolutionsBox.AddChild(MenuConfigurationDisplayUI.bufferToggle);
			MenuConfigurationDisplayUI.userInterfaceScaleField = Glazier.Get().CreateFloat32Field();
			MenuConfigurationDisplayUI.userInterfaceScaleField.PositionOffset_Y = 100f;
			MenuConfigurationDisplayUI.userInterfaceScaleField.SizeOffset_X = 200f;
			MenuConfigurationDisplayUI.userInterfaceScaleField.SizeOffset_Y = 30f;
			MenuConfigurationDisplayUI.userInterfaceScaleField.AddLabel(MenuConfigurationDisplayUI.localization.format("User_Interface_Scale_Field_Label"), 1);
			MenuConfigurationDisplayUI.userInterfaceScaleField.Value = GraphicsSettings.userInterfaceScale;
			MenuConfigurationDisplayUI.userInterfaceScaleField.OnValueSubmitted += new TypedSingle(MenuConfigurationDisplayUI.onTypedUserInterfaceScale);
			MenuConfigurationDisplayUI.resolutionsBox.AddChild(MenuConfigurationDisplayUI.userInterfaceScaleField);
			MenuConfigurationDisplayUI.targetFrameRateToggle = Glazier.Get().CreateToggle();
			MenuConfigurationDisplayUI.targetFrameRateToggle.PositionOffset_Y = 140f;
			MenuConfigurationDisplayUI.targetFrameRateToggle.SizeOffset_X = 40f;
			MenuConfigurationDisplayUI.targetFrameRateToggle.SizeOffset_Y = 40f;
			MenuConfigurationDisplayUI.targetFrameRateToggle.AddLabel(MenuConfigurationDisplayUI.localization.format("UseTargetFrameRate_Toggle_Label"), 1);
			MenuConfigurationDisplayUI.targetFrameRateToggle.Value = GraphicsSettings.UseTargetFrameRate;
			MenuConfigurationDisplayUI.targetFrameRateToggle.OnValueChanged += new Toggled(MenuConfigurationDisplayUI.OnToggledTargetFrameRate);
			MenuConfigurationDisplayUI.resolutionsBox.AddChild(MenuConfigurationDisplayUI.targetFrameRateToggle);
			MenuConfigurationDisplayUI.targetFrameRateField = Glazier.Get().CreateUInt32Field();
			MenuConfigurationDisplayUI.targetFrameRateField.PositionOffset_Y = 180f;
			MenuConfigurationDisplayUI.targetFrameRateField.SizeOffset_X = 200f;
			MenuConfigurationDisplayUI.targetFrameRateField.SizeOffset_Y = 30f;
			MenuConfigurationDisplayUI.targetFrameRateField.AddLabel(MenuConfigurationDisplayUI.localization.format("TargetFrameRate_Field_Label"), 1);
			MenuConfigurationDisplayUI.targetFrameRateField.Value = (uint)GraphicsSettings.TargetFrameRate;
			MenuConfigurationDisplayUI.targetFrameRateField.OnValueChanged += new TypedUInt32(MenuConfigurationDisplayUI.OnTypedTargetFrameRate);
			MenuConfigurationDisplayUI.resolutionsBox.AddChild(MenuConfigurationDisplayUI.targetFrameRateField);
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateToggle = Glazier.Get().CreateToggle();
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateToggle.PositionOffset_Y = 220f;
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateToggle.SizeOffset_X = 40f;
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateToggle.SizeOffset_Y = 40f;
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateToggle.AddLabel(MenuConfigurationDisplayUI.localization.format("UseUnfocusedTargetFrameRate_Toggle_Label"), 1);
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateToggle.Value = GraphicsSettings.UseUnfocusedTargetFrameRate;
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateToggle.OnValueChanged += new Toggled(MenuConfigurationDisplayUI.OnToggledUnfocusedTargetFrameRate);
			MenuConfigurationDisplayUI.resolutionsBox.AddChild(MenuConfigurationDisplayUI.unfocusedTargetFrameRateToggle);
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateField = Glazier.Get().CreateUInt32Field();
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateField.PositionOffset_Y = 260f;
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateField.SizeOffset_X = 200f;
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateField.SizeOffset_Y = 30f;
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateField.AddLabel(MenuConfigurationDisplayUI.localization.format("UnfocusedTargetFrameRate_Field_Label"), 1);
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateField.Value = (uint)GraphicsSettings.UnfocusedTargetFrameRate;
			MenuConfigurationDisplayUI.unfocusedTargetFrameRateField.OnValueChanged += new TypedUInt32(MenuConfigurationDisplayUI.OnTypedUnfocusedTargetFrameRate);
			MenuConfigurationDisplayUI.resolutionsBox.AddChild(MenuConfigurationDisplayUI.unfocusedTargetFrameRateField);
			MenuConfigurationDisplayUI.SynchronizeTargetFrameRateVisibility();
			MenuConfigurationDisplayUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuConfigurationDisplayUI.backButton.PositionOffset_Y = -50f;
			MenuConfigurationDisplayUI.backButton.PositionScale_Y = 1f;
			MenuConfigurationDisplayUI.backButton.SizeOffset_X = 200f;
			MenuConfigurationDisplayUI.backButton.SizeOffset_Y = 50f;
			MenuConfigurationDisplayUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuConfigurationDisplayUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuConfigurationDisplayUI.backButton.onClickedButton += new ClickedButton(MenuConfigurationDisplayUI.onClickedBackButton);
			MenuConfigurationDisplayUI.backButton.fontSize = 3;
			MenuConfigurationDisplayUI.backButton.iconColor = 2;
			MenuConfigurationDisplayUI.container.AddChild(MenuConfigurationDisplayUI.backButton);
		}

		// Token: 0x040028AE RID: 10414
		private static Local localization;

		// Token: 0x040028AF RID: 10415
		private static SleekFullscreenBox container;

		// Token: 0x040028B0 RID: 10416
		public static bool active;

		// Token: 0x040028B1 RID: 10417
		private static SleekButtonIcon backButton;

		// Token: 0x040028B2 RID: 10418
		private static ISleekScrollView resolutionsBox;

		// Token: 0x040028B3 RID: 10419
		private static ISleekButton[] buttons;

		// Token: 0x040028B4 RID: 10420
		private static SleekButtonState fullscreenMode;

		// Token: 0x040028B5 RID: 10421
		private static ISleekToggle bufferToggle;

		// Token: 0x040028B6 RID: 10422
		private static ISleekFloat32Field userInterfaceScaleField;

		// Token: 0x040028B7 RID: 10423
		private static ISleekToggle targetFrameRateToggle;

		// Token: 0x040028B8 RID: 10424
		private static ISleekUInt32Field targetFrameRateField;

		// Token: 0x040028B9 RID: 10425
		private static ISleekToggle unfocusedTargetFrameRateToggle;

		// Token: 0x040028BA RID: 10426
		private static ISleekUInt32Field unfocusedTargetFrameRateField;
	}
}
