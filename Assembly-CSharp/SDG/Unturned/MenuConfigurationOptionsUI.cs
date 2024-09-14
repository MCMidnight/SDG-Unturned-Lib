using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200078E RID: 1934
	public class MenuConfigurationOptionsUI
	{
		// Token: 0x06004016 RID: 16406 RVA: 0x00147DC9 File Offset: 0x00145FC9
		public static void open()
		{
			if (MenuConfigurationOptionsUI.active)
			{
				return;
			}
			MenuConfigurationOptionsUI.active = true;
			MenuConfigurationOptionsUI.updateAll();
			MenuConfigurationOptionsUI.container.AnimateIntoView();
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x00147DE8 File Offset: 0x00145FE8
		public static void close()
		{
			if (!MenuConfigurationOptionsUI.active)
			{
				return;
			}
			MenuSettings.SaveOptionsIfLoaded();
			MenuConfigurationOptionsUI.active = false;
			MenuConfigurationOptionsUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x00147E14 File Offset: 0x00146014
		private static string FormatFieldOfViewTooltip()
		{
			float desiredVerticalFieldOfView = OptionsSettings.DesiredVerticalFieldOfView;
			if (MenuConfigurationOptionsUI.localization.has("FOV_Slider_LabelV2_Value"))
			{
				float f = Camera.VerticalToHorizontalFieldOfView(desiredVerticalFieldOfView, ScreenEx.GetCurrentAspectRatio());
				return MenuConfigurationOptionsUI.localization.format("FOV_Slider_LabelV2_Name") + "\n" + MenuConfigurationOptionsUI.localization.format("FOV_Slider_LabelV2_Value", Mathf.RoundToInt(f), Mathf.RoundToInt(desiredVerticalFieldOfView));
			}
			return MenuConfigurationOptionsUI.localization.format("FOV_Slider_Label", Mathf.RoundToInt(desiredVerticalFieldOfView));
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x00147E9D File Offset: 0x0014609D
		private static void onDraggedFOVSlider(ISleekSlider slider, float state)
		{
			OptionsSettings.fov = state;
			OptionsSettings.apply();
			MenuConfigurationOptionsUI.fovLabel.Text = MenuConfigurationOptionsUI.FormatFieldOfViewTooltip();
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x00147EB9 File Offset: 0x001460B9
		private static void onToggledDebugToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.debug = state;
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x00147EC1 File Offset: 0x001460C1
		private static void onToggledTimerToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.timer = state;
			OptionsSettings.apply();
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x00147ECE File Offset: 0x001460CE
		private static void onToggledGoreToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.gore = state;
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x00147ED6 File Offset: 0x001460D6
		private static void onToggledFilterToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.filter = state;
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x00147EDE File Offset: 0x001460DE
		private static void onToggledChatTextToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.chatText = state;
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x00147EE6 File Offset: 0x001460E6
		private static void onToggledChatVoiceInToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.chatVoiceIn = state;
			MenuConfigurationOptionsUI.chatVoiceOutToggle.IsInteractable = state;
			MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle.IsInteractable = (OptionsSettings.chatVoiceIn && OptionsSettings.chatVoiceOut);
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x00147F12 File Offset: 0x00146112
		private static void onToggledChatVoiceOutToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.chatVoiceOut = state;
			MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle.IsInteractable = (OptionsSettings.chatVoiceIn && state);
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x00147F2B File Offset: 0x0014612B
		private static void onToggledChatVoiceAlwaysRecordingToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.VoiceAlwaysRecording = state;
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x00147F33 File Offset: 0x00146133
		private static void onToggledHintsToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.hints = state;
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x00147F3B File Offset: 0x0014613B
		private static void onToggledStreamerToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.streamer = state;
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x00147F43 File Offset: 0x00146143
		private static void onToggledFeaturedWorkshopToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.featuredWorkshop = state;
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x00147F4B File Offset: 0x0014614B
		private static void onToggledShowHotbarToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.showHotbar = state;
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00147F53 File Offset: 0x00146153
		private static void onToggledPauseWhenUnfocusedToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.pauseWhenUnfocused = state;
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x00147F5B File Offset: 0x0014615B
		private static void onToggledNametagFadeOutToggle(ISleekToggle toggle, bool state)
		{
			OptionsSettings.shouldNametagFadeOut = state;
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x00147F63 File Offset: 0x00146163
		private static void OnScreenshotSizeMultiplierChanged(ISleekInt32Field field, int value)
		{
			OptionsSettings.screenshotSizeMultiplier = value;
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x00147F6B File Offset: 0x0014616B
		private static void OnScreenshotSupersamplingChanged(ISleekToggle toggle, bool state)
		{
			OptionsSettings.enableScreenshotSupersampling = state;
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x00147F73 File Offset: 0x00146173
		private static void OnScreenshotsWhileLoadingChanged(ISleekToggle toggle, bool state)
		{
			OptionsSettings.enableScreenshotsOnLoadingScreen = state;
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x00147F7B File Offset: 0x0014617B
		private static void OnUseStaticCrosshairChanged(ISleekToggle toggle, bool state)
		{
			OptionsSettings.useStaticCrosshair = state;
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x00147F83 File Offset: 0x00146183
		private static void OnStaticCrosshairSizeChanged(ISleekSlider slider, float state)
		{
			OptionsSettings.staticCrosshairSize = state;
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x00147F8B File Offset: 0x0014618B
		private static void OnCrosshairShapeChanged(SleekButtonState button, int index)
		{
			OptionsSettings.crosshairShape = (ECrosshairShape)index;
			if (PlayerLifeUI.crosshair != null)
			{
				PlayerLifeUI.crosshair.SynchronizeImages();
			}
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x00147FA4 File Offset: 0x001461A4
		private static void onSwappedMetricState(SleekButtonState button, int index)
		{
			OptionsSettings.metric = (index == 1);
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x00147FAF File Offset: 0x001461AF
		private static void onSwappedTalkState(SleekButtonState button, int index)
		{
			OptionsSettings.talk = (index == 1);
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x00147FBA File Offset: 0x001461BA
		private static void onSwappedUIState(SleekButtonState button, int index)
		{
			OptionsSettings.proUI = (index == 1);
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x00147FC5 File Offset: 0x001461C5
		private static void onSwappedHitmarkerState(SleekButtonState button, int index)
		{
			OptionsSettings.ShouldHitmarkersFollowWorldPosition = (index == 1);
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x00147FD0 File Offset: 0x001461D0
		private static void onSwappedHitmarkerStyleState(SleekButtonState button, int index)
		{
			OptionsSettings.hitmarkerStyle = (EHitmarkerStyle)index;
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x00147FD8 File Offset: 0x001461D8
		private static void onSwappedVehicleThirdPersonCameraModeState(SleekButtonState button, int index)
		{
			OptionsSettings.vehicleThirdPersonCameraMode = (EVehicleThirdPersonCameraMode)index;
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x00147FE0 File Offset: 0x001461E0
		private static void onSwappedAircraftThirdPersonCameraModeState(SleekButtonState button, int index)
		{
			OptionsSettings.vehicleAircraftThirdPersonCameraMode = (EVehicleThirdPersonCameraMode)index;
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x00147FE8 File Offset: 0x001461E8
		private static void onCrosshairColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.crosshairColor = color;
			if (PlayerLifeUI.crosshair != null)
			{
				PlayerLifeUI.crosshair.SynchronizeCustomColors();
			}
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x00148001 File Offset: 0x00146201
		private static void onHitmarkerColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.hitmarkerColor = color;
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x00148009 File Offset: 0x00146209
		private static void onCriticalHitmarkerColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.criticalHitmarkerColor = color;
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x00148011 File Offset: 0x00146211
		private static void onCursorColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.cursorColor = color;
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x00148019 File Offset: 0x00146219
		private static void onBackgroundColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.backgroundColor = color;
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x00148021 File Offset: 0x00146221
		private static void onForegroundColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.foregroundColor = color;
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x00148029 File Offset: 0x00146229
		private static void onFontColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.fontColor = color;
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x00148031 File Offset: 0x00146231
		private static void onShadowColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.shadowColor = color;
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x00148039 File Offset: 0x00146239
		private static void onBadColorPicked(SleekColorPicker picker, Color color)
		{
			OptionsSettings.badColor = color;
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x00148041 File Offset: 0x00146241
		private static void OnDontShowOnlineSafetyMenuAgainToggled(ISleekToggle toggle, bool value)
		{
			OptionsSettings.wantsToHideOnlineSafetyMenu = value;
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x00148049 File Offset: 0x00146249
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
			MenuConfigurationOptionsUI.close();
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x00148077 File Offset: 0x00146277
		private static void onClickedDefaultButton(ISleekElement button)
		{
			OptionsSettings.restoreDefaults();
			MenuConfigurationOptionsUI.updateAll();
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x00148084 File Offset: 0x00146284
		private static void updateAll()
		{
			MenuConfigurationOptionsUI.fovSlider.Value = OptionsSettings.fov;
			MenuConfigurationOptionsUI.fovLabel.Text = MenuConfigurationOptionsUI.FormatFieldOfViewTooltip();
			MenuConfigurationOptionsUI.debugToggle.Value = OptionsSettings.debug;
			MenuConfigurationOptionsUI.timerToggle.Value = OptionsSettings.timer;
			MenuConfigurationOptionsUI.goreToggle.Value = OptionsSettings.gore;
			MenuConfigurationOptionsUI.filterToggle.Value = OptionsSettings.filter;
			MenuConfigurationOptionsUI.chatTextToggle.Value = OptionsSettings.chatText;
			MenuConfigurationOptionsUI.chatVoiceInToggle.Value = OptionsSettings.chatVoiceIn;
			MenuConfigurationOptionsUI.chatVoiceOutToggle.Value = OptionsSettings.chatVoiceOut;
			MenuConfigurationOptionsUI.chatVoiceOutToggle.IsInteractable = OptionsSettings.chatVoiceIn;
			MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle.Value = OptionsSettings.VoiceAlwaysRecording;
			MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle.IsInteractable = (OptionsSettings.chatVoiceIn && OptionsSettings.chatVoiceOut);
			MenuConfigurationOptionsUI.hintsToggle.Value = OptionsSettings.hints;
			MenuConfigurationOptionsUI.streamerToggle.Value = OptionsSettings.streamer;
			MenuConfigurationOptionsUI.featuredWorkshopToggle.Value = OptionsSettings.featuredWorkshop;
			MenuConfigurationOptionsUI.showHotbarToggle.Value = OptionsSettings.showHotbar;
			MenuConfigurationOptionsUI.pauseWhenUnfocusedToggle.Value = OptionsSettings.pauseWhenUnfocused;
			MenuConfigurationOptionsUI.nametagFadeOutToggle.Value = OptionsSettings.shouldNametagFadeOut;
			MenuConfigurationOptionsUI.screenshotSizeMultiplierField.Value = OptionsSettings.screenshotSizeMultiplier;
			MenuConfigurationOptionsUI.screenshotSupersamplingToggle.Value = OptionsSettings.enableScreenshotSupersampling;
			MenuConfigurationOptionsUI.screenshotsWhileLoadingToggle.Value = OptionsSettings.enableScreenshotsOnLoadingScreen;
			MenuConfigurationOptionsUI.staticCrosshairToggle.Value = OptionsSettings.useStaticCrosshair;
			MenuConfigurationOptionsUI.staticCrosshairSizeSlider.Value = OptionsSettings.staticCrosshairSize;
			MenuConfigurationOptionsUI.crosshairShapeButton.state = (int)OptionsSettings.crosshairShape;
			MenuConfigurationOptionsUI.metricButton.state = (OptionsSettings.metric ? 1 : 0);
			MenuConfigurationOptionsUI.talkButton.state = (OptionsSettings.talk ? 1 : 0);
			MenuConfigurationOptionsUI.uiButton.state = (OptionsSettings.proUI ? 1 : 0);
			MenuConfigurationOptionsUI.hitmarkerButton.state = (OptionsSettings.ShouldHitmarkersFollowWorldPosition ? 1 : 0);
			MenuConfigurationOptionsUI.hitmarkerStyleButton.state = (int)OptionsSettings.hitmarkerStyle;
			MenuConfigurationOptionsUI.vehicleThirdPersonCameraModeButton.state = (int)OptionsSettings.vehicleThirdPersonCameraMode;
			MenuConfigurationOptionsUI.aircraftThirdPersonCameraModeButton.state = (int)OptionsSettings.vehicleAircraftThirdPersonCameraMode;
			MenuConfigurationOptionsUI.crosshairColorPicker.state = OptionsSettings.crosshairColor;
			MenuConfigurationOptionsUI.hitmarkerColorPicker.state = OptionsSettings.hitmarkerColor;
			MenuConfigurationOptionsUI.criticalHitmarkerColorPicker.state = OptionsSettings.criticalHitmarkerColor;
			MenuConfigurationOptionsUI.cursorColorPicker.state = OptionsSettings.cursorColor;
			MenuConfigurationOptionsUI.backgroundColorPicker.state = OptionsSettings.backgroundColor;
			MenuConfigurationOptionsUI.foregroundColorPicker.state = OptionsSettings.foregroundColor;
			MenuConfigurationOptionsUI.fontColorPicker.state = OptionsSettings.fontColor;
			MenuConfigurationOptionsUI.shadowColorPicker.state = OptionsSettings.shadowColor;
			MenuConfigurationOptionsUI.badColorPicker.state = OptionsSettings.badColor;
			MenuConfigurationOptionsUI.dontShowOnlineSafetyMenuAgainToggle.Value = OptionsSettings.wantsToHideOnlineSafetyMenu;
		}

		// Token: 0x06004042 RID: 16450 RVA: 0x0014831C File Offset: 0x0014651C
		public MenuConfigurationOptionsUI()
		{
			MenuConfigurationOptionsUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationOptions.dat");
			MenuConfigurationOptionsUI.container = new SleekFullscreenBox();
			MenuConfigurationOptionsUI.container.PositionOffset_X = 10f;
			MenuConfigurationOptionsUI.container.PositionOffset_Y = 10f;
			MenuConfigurationOptionsUI.container.PositionScale_Y = 1f;
			MenuConfigurationOptionsUI.container.SizeOffset_X = -20f;
			MenuConfigurationOptionsUI.container.SizeOffset_Y = -20f;
			MenuConfigurationOptionsUI.container.SizeScale_X = 1f;
			MenuConfigurationOptionsUI.container.SizeScale_Y = 1f;
			if (Provider.isConnected)
			{
				PlayerUI.container.AddChild(MenuConfigurationOptionsUI.container);
			}
			else if (Level.isEditor)
			{
				EditorUI.window.AddChild(MenuConfigurationOptionsUI.container);
			}
			else
			{
				MenuUI.container.AddChild(MenuConfigurationOptionsUI.container);
			}
			MenuConfigurationOptionsUI.active = false;
			MenuConfigurationOptionsUI.optionsBox = Glazier.Get().CreateScrollView();
			MenuConfigurationOptionsUI.optionsBox.PositionOffset_X = -250f;
			MenuConfigurationOptionsUI.optionsBox.PositionOffset_Y = 100f;
			MenuConfigurationOptionsUI.optionsBox.PositionScale_X = 0.5f;
			MenuConfigurationOptionsUI.optionsBox.SizeOffset_X = 530f;
			MenuConfigurationOptionsUI.optionsBox.SizeOffset_Y = -200f;
			MenuConfigurationOptionsUI.optionsBox.SizeScale_Y = 1f;
			MenuConfigurationOptionsUI.optionsBox.ScaleContentToWidth = true;
			MenuConfigurationOptionsUI.container.AddChild(MenuConfigurationOptionsUI.optionsBox);
			float num = 0f;
			MenuConfigurationOptionsUI.debugToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.debugToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.debugToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.debugToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.debugToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Debug_Toggle_Label"), 1);
			MenuConfigurationOptionsUI.debugToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledDebugToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.debugToggle);
			num += 50f;
			MenuConfigurationOptionsUI.timerToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.timerToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.timerToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.timerToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.timerToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Timer_Toggle_Label"), 1);
			MenuConfigurationOptionsUI.timerToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledTimerToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.timerToggle);
			num += 50f;
			MenuConfigurationOptionsUI.goreToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.goreToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.goreToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.goreToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.goreToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Gore_Toggle_Label"), 1);
			MenuConfigurationOptionsUI.goreToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledGoreToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.goreToggle);
			num += 50f;
			MenuConfigurationOptionsUI.filterToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.filterToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.filterToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.filterToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.filterToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Filter_Toggle_Label"), 1);
			MenuConfigurationOptionsUI.filterToggle.TooltipText = MenuConfigurationOptionsUI.localization.format("Filter_Toggle_Tooltip");
			MenuConfigurationOptionsUI.filterToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledFilterToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.filterToggle);
			num += 50f;
			MenuConfigurationOptionsUI.chatTextToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.chatTextToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.chatTextToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.chatTextToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.chatTextToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Chat_Text_Toggle_Label"), 1);
			MenuConfigurationOptionsUI.chatTextToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledChatTextToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.chatTextToggle);
			num += 50f;
			MenuConfigurationOptionsUI.chatVoiceInToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.chatVoiceInToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.chatVoiceInToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.chatVoiceInToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.chatVoiceInToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Chat_Voice_In_Toggle_Label"), 1);
			MenuConfigurationOptionsUI.chatVoiceInToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledChatVoiceInToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.chatVoiceInToggle);
			num += 50f;
			MenuConfigurationOptionsUI.chatVoiceOutToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.chatVoiceOutToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.chatVoiceOutToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.chatVoiceOutToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.chatVoiceOutToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Chat_Voice_Out_Toggle_Label"), 1);
			MenuConfigurationOptionsUI.chatVoiceOutToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledChatVoiceOutToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.chatVoiceOutToggle);
			num += 50f;
			MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("VoiceAlwaysRecording_Label"), 1);
			MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle.TooltipText = MenuConfigurationOptionsUI.localization.format("VoiceAlwaysRecording_Tooltip");
			MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledChatVoiceAlwaysRecordingToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.chatVoiceAlwaysRecordingToggle);
			num += 50f;
			MenuConfigurationOptionsUI.hintsToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.hintsToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.hintsToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.hintsToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.hintsToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Hints_Toggle_Label"), 1);
			MenuConfigurationOptionsUI.hintsToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledHintsToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.hintsToggle);
			num += 50f;
			MenuConfigurationOptionsUI.streamerToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.streamerToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.streamerToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.streamerToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.streamerToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Streamer_Toggle_Label"), 1);
			MenuConfigurationOptionsUI.streamerToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledStreamerToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.streamerToggle);
			num += 50f;
			MenuConfigurationOptionsUI.featuredWorkshopToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.featuredWorkshopToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.featuredWorkshopToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.featuredWorkshopToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.featuredWorkshopToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Featured_Workshop_Toggle_Label"), 1);
			MenuConfigurationOptionsUI.featuredWorkshopToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledFeaturedWorkshopToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.featuredWorkshopToggle);
			num += 50f;
			MenuConfigurationOptionsUI.showHotbarToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.showHotbarToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.showHotbarToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.showHotbarToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.showHotbarToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Show_Hotbar_Toggle_Label"), 1);
			MenuConfigurationOptionsUI.showHotbarToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledShowHotbarToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.showHotbarToggle);
			num += 50f;
			MenuConfigurationOptionsUI.pauseWhenUnfocusedToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.pauseWhenUnfocusedToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.pauseWhenUnfocusedToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.pauseWhenUnfocusedToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.pauseWhenUnfocusedToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Pause_When_Unfocused_Label"), 1);
			MenuConfigurationOptionsUI.pauseWhenUnfocusedToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledPauseWhenUnfocusedToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.pauseWhenUnfocusedToggle);
			num += 50f;
			MenuConfigurationOptionsUI.nametagFadeOutToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.nametagFadeOutToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.nametagFadeOutToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.nametagFadeOutToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.nametagFadeOutToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("Nametag_Fade_Out_Label"), 1);
			MenuConfigurationOptionsUI.nametagFadeOutToggle.TooltipText = MenuConfigurationOptionsUI.localization.format("Nametag_Fade_Out_Tooltip");
			MenuConfigurationOptionsUI.nametagFadeOutToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.onToggledNametagFadeOutToggle);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.nametagFadeOutToggle);
			num += 50f;
			MenuConfigurationOptionsUI.fovSlider = Glazier.Get().CreateSlider();
			MenuConfigurationOptionsUI.fovSlider.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.fovSlider.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.fovSlider.SizeOffset_Y = 20f;
			MenuConfigurationOptionsUI.fovSlider.Orientation = 0;
			MenuConfigurationOptionsUI.fovSlider.OnValueChanged += new Dragged(MenuConfigurationOptionsUI.onDraggedFOVSlider);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.fovSlider);
			MenuConfigurationOptionsUI.fovLabel = Glazier.Get().CreateLabel();
			MenuConfigurationOptionsUI.fovLabel.PositionOffset_X = 5f;
			MenuConfigurationOptionsUI.fovLabel.PositionOffset_Y = -30f;
			MenuConfigurationOptionsUI.fovLabel.PositionScale_X = 1f;
			MenuConfigurationOptionsUI.fovLabel.PositionScale_Y = 0.5f;
			MenuConfigurationOptionsUI.fovLabel.SizeOffset_X = 300f;
			MenuConfigurationOptionsUI.fovLabel.SizeOffset_Y = 60f;
			MenuConfigurationOptionsUI.fovLabel.TextAlignment = 3;
			MenuConfigurationOptionsUI.fovLabel.TextContrastContext = 2;
			MenuConfigurationOptionsUI.fovLabel.Text = MenuConfigurationOptionsUI.FormatFieldOfViewTooltip();
			MenuConfigurationOptionsUI.fovSlider.AddChild(MenuConfigurationOptionsUI.fovLabel);
			num += 30f;
			MenuConfigurationOptionsUI.screenshotSizeMultiplierField = Glazier.Get().CreateInt32Field();
			MenuConfigurationOptionsUI.screenshotSizeMultiplierField.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.screenshotSizeMultiplierField.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.screenshotSizeMultiplierField.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.screenshotSizeMultiplierField.AddLabel(MenuConfigurationOptionsUI.localization.format("ScreenshotSizeMultiplier_Label"), 1);
			MenuConfigurationOptionsUI.screenshotSizeMultiplierField.TooltipText = MenuConfigurationOptionsUI.localization.format("ScreenshotSizeMultiplier_Tooltip");
			MenuConfigurationOptionsUI.screenshotSizeMultiplierField.OnValueChanged += new TypedInt32(MenuConfigurationOptionsUI.OnScreenshotSizeMultiplierChanged);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.screenshotSizeMultiplierField);
			num += 40f;
			MenuConfigurationOptionsUI.screenshotSupersamplingToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.screenshotSupersamplingToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.screenshotSupersamplingToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.screenshotSupersamplingToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.screenshotSupersamplingToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("ScreenshotSupersampling_Label"), 1);
			MenuConfigurationOptionsUI.screenshotSupersamplingToggle.TooltipText = MenuConfigurationOptionsUI.localization.format("ScreenshotSupersampling_Tooltip");
			MenuConfigurationOptionsUI.screenshotSupersamplingToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.OnScreenshotSupersamplingChanged);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.screenshotSupersamplingToggle);
			num += 50f;
			MenuConfigurationOptionsUI.screenshotsWhileLoadingToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.screenshotsWhileLoadingToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.screenshotsWhileLoadingToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.screenshotsWhileLoadingToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.screenshotsWhileLoadingToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("ScreenshotsWhileLoading_Label"), 1);
			MenuConfigurationOptionsUI.screenshotsWhileLoadingToggle.TooltipText = MenuConfigurationOptionsUI.localization.format("ScreenshotsWhileLoading_Tooltip");
			MenuConfigurationOptionsUI.screenshotsWhileLoadingToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.OnScreenshotsWhileLoadingChanged);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.screenshotsWhileLoadingToggle);
			num += 50f;
			MenuConfigurationOptionsUI.staticCrosshairToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.staticCrosshairToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.staticCrosshairToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.staticCrosshairToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.staticCrosshairToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("UseStaticCrosshair_Label"), 1);
			MenuConfigurationOptionsUI.staticCrosshairToggle.TooltipText = MenuConfigurationOptionsUI.localization.format("UseStaticCrosshair_Tooltip");
			MenuConfigurationOptionsUI.staticCrosshairToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.OnUseStaticCrosshairChanged);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.staticCrosshairToggle);
			num += 50f;
			MenuConfigurationOptionsUI.staticCrosshairSizeSlider = Glazier.Get().CreateSlider();
			MenuConfigurationOptionsUI.staticCrosshairSizeSlider.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.staticCrosshairSizeSlider.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.staticCrosshairSizeSlider.SizeOffset_Y = 20f;
			MenuConfigurationOptionsUI.staticCrosshairSizeSlider.Orientation = 0;
			MenuConfigurationOptionsUI.staticCrosshairSizeSlider.AddLabel(MenuConfigurationOptionsUI.localization.format("StaticCrosshairSize_Label"), 1);
			MenuConfigurationOptionsUI.staticCrosshairSizeSlider.OnValueChanged += new Dragged(MenuConfigurationOptionsUI.OnStaticCrosshairSizeChanged);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.staticCrosshairSizeSlider);
			num += 30f;
			MenuConfigurationOptionsUI.crosshairShapeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("CrosshairShape_Line")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("CrosshairShape_Classic"))
			});
			MenuConfigurationOptionsUI.crosshairShapeButton.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.crosshairShapeButton.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.crosshairShapeButton.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.crosshairShapeButton.state = (int)OptionsSettings.crosshairShape;
			MenuConfigurationOptionsUI.crosshairShapeButton.AddLabel(MenuConfigurationOptionsUI.localization.format("CrosshairShape_Label"), 1);
			MenuConfigurationOptionsUI.crosshairShapeButton.onSwappedState = new SwappedState(MenuConfigurationOptionsUI.OnCrosshairShapeChanged);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.crosshairShapeButton);
			num += 40f;
			MenuConfigurationOptionsUI.talkButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Talk_Off")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Talk_On"))
			});
			MenuConfigurationOptionsUI.talkButton.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.talkButton.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.talkButton.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.talkButton.state = (OptionsSettings.talk ? 1 : 0);
			MenuConfigurationOptionsUI.talkButton.tooltip = MenuConfigurationOptionsUI.localization.format("Talk_Tooltip");
			MenuConfigurationOptionsUI.talkButton.AddLabel(MenuConfigurationOptionsUI.localization.format("Talk_Label"), 1);
			MenuConfigurationOptionsUI.talkButton.onSwappedState = new SwappedState(MenuConfigurationOptionsUI.onSwappedTalkState);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.talkButton);
			num += 40f;
			MenuConfigurationOptionsUI.metricButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Metric_Off")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Metric_On"))
			});
			MenuConfigurationOptionsUI.metricButton.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.metricButton.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.metricButton.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.metricButton.state = (OptionsSettings.metric ? 1 : 0);
			MenuConfigurationOptionsUI.metricButton.tooltip = MenuConfigurationOptionsUI.localization.format("Metric_Tooltip");
			MenuConfigurationOptionsUI.metricButton.AddLabel(MenuConfigurationOptionsUI.localization.format("Metric_Label"), 1);
			MenuConfigurationOptionsUI.metricButton.onSwappedState = new SwappedState(MenuConfigurationOptionsUI.onSwappedMetricState);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.metricButton);
			num += 40f;
			MenuConfigurationOptionsUI.uiButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("UI_Free")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("UI_Pro"))
			});
			MenuConfigurationOptionsUI.uiButton.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.uiButton.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.uiButton.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.uiButton.tooltip = MenuConfigurationOptionsUI.localization.format("UI_Tooltip");
			MenuConfigurationOptionsUI.uiButton.AddLabel(MenuConfigurationOptionsUI.localization.format("UI_Label"), 1);
			MenuConfigurationOptionsUI.uiButton.onSwappedState = new SwappedState(MenuConfigurationOptionsUI.onSwappedUIState);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.uiButton);
			num += 40f;
			MenuConfigurationOptionsUI.hitmarkerButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Hitmarker_Static")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("Hitmarker_Dynamic"))
			});
			MenuConfigurationOptionsUI.hitmarkerButton.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.hitmarkerButton.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.hitmarkerButton.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.hitmarkerButton.tooltip = MenuConfigurationOptionsUI.localization.format("Hitmarker_Tooltip");
			MenuConfigurationOptionsUI.hitmarkerButton.AddLabel(MenuConfigurationOptionsUI.localization.format("Hitmarker_Label"), 1);
			MenuConfigurationOptionsUI.hitmarkerButton.onSwappedState = new SwappedState(MenuConfigurationOptionsUI.onSwappedHitmarkerState);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.hitmarkerButton);
			num += 40f;
			MenuConfigurationOptionsUI.hitmarkerStyleButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("HitmarkerStyle_Animated")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("HitmarkerStyle_Classic"))
			});
			MenuConfigurationOptionsUI.hitmarkerStyleButton.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.hitmarkerStyleButton.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.hitmarkerStyleButton.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.hitmarkerStyleButton.AddLabel(MenuConfigurationOptionsUI.localization.format("HitmarkerStyle_Label"), 1);
			MenuConfigurationOptionsUI.hitmarkerStyleButton.onSwappedState = new SwappedState(MenuConfigurationOptionsUI.onSwappedHitmarkerStyleState);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.hitmarkerStyleButton);
			num += 40f;
			MenuConfigurationOptionsUI.vehicleThirdPersonCameraModeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("VehicleThirdPersonCameraMode_RotationDetached")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("VehicleThirdPersonCameraMode_RotationAttached"))
			});
			MenuConfigurationOptionsUI.vehicleThirdPersonCameraModeButton.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.vehicleThirdPersonCameraModeButton.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.vehicleThirdPersonCameraModeButton.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.vehicleThirdPersonCameraModeButton.AddLabel(MenuConfigurationOptionsUI.localization.format("VehicleThirdPersonCameraMode_Label"), 1);
			MenuConfigurationOptionsUI.vehicleThirdPersonCameraModeButton.onSwappedState = new SwappedState(MenuConfigurationOptionsUI.onSwappedVehicleThirdPersonCameraModeState);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.vehicleThirdPersonCameraModeButton);
			num += 40f;
			MenuConfigurationOptionsUI.aircraftThirdPersonCameraModeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationOptionsUI.localization.format("VehicleThirdPersonCameraMode_RotationDetached")),
				new GUIContent(MenuConfigurationOptionsUI.localization.format("VehicleThirdPersonCameraMode_RotationAttached"))
			});
			MenuConfigurationOptionsUI.aircraftThirdPersonCameraModeButton.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.aircraftThirdPersonCameraModeButton.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.aircraftThirdPersonCameraModeButton.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.aircraftThirdPersonCameraModeButton.AddLabel(MenuConfigurationOptionsUI.localization.format("AircraftThirdPersonCameraMode_Label"), 1);
			MenuConfigurationOptionsUI.aircraftThirdPersonCameraModeButton.onSwappedState = new SwappedState(MenuConfigurationOptionsUI.onSwappedAircraftThirdPersonCameraModeState);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.aircraftThirdPersonCameraModeButton);
			num += 40f;
			MenuConfigurationOptionsUI.crosshairBox = Glazier.Get().CreateBox();
			MenuConfigurationOptionsUI.crosshairBox.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.crosshairBox.SizeOffset_X = 240f;
			MenuConfigurationOptionsUI.crosshairBox.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.crosshairBox.Text = MenuConfigurationOptionsUI.localization.format("Crosshair_Box");
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.crosshairBox);
			num += 40f;
			MenuConfigurationOptionsUI.crosshairColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.crosshairColorPicker.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.crosshairColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onCrosshairColorPicked);
			MenuConfigurationOptionsUI.crosshairColorPicker.SetAllowAlpha(true);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.crosshairColorPicker);
			num += 160f;
			MenuConfigurationOptionsUI.hitmarkerBox = Glazier.Get().CreateBox();
			MenuConfigurationOptionsUI.hitmarkerBox.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.hitmarkerBox.SizeOffset_X = 240f;
			MenuConfigurationOptionsUI.hitmarkerBox.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.hitmarkerBox.Text = MenuConfigurationOptionsUI.localization.format("Hitmarker_Box");
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.hitmarkerBox);
			num += 40f;
			MenuConfigurationOptionsUI.hitmarkerColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.hitmarkerColorPicker.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.hitmarkerColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onHitmarkerColorPicked);
			MenuConfigurationOptionsUI.hitmarkerColorPicker.SetAllowAlpha(true);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.hitmarkerColorPicker);
			num += 160f;
			MenuConfigurationOptionsUI.criticalHitmarkerBox = Glazier.Get().CreateBox();
			MenuConfigurationOptionsUI.criticalHitmarkerBox.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.criticalHitmarkerBox.SizeOffset_X = 240f;
			MenuConfigurationOptionsUI.criticalHitmarkerBox.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.criticalHitmarkerBox.Text = MenuConfigurationOptionsUI.localization.format("Critical_Hitmarker_Box");
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.criticalHitmarkerBox);
			num += 40f;
			MenuConfigurationOptionsUI.criticalHitmarkerColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.criticalHitmarkerColorPicker.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.criticalHitmarkerColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onCriticalHitmarkerColorPicked);
			MenuConfigurationOptionsUI.criticalHitmarkerColorPicker.SetAllowAlpha(true);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.criticalHitmarkerColorPicker);
			num += 160f;
			MenuConfigurationOptionsUI.cursorBox = Glazier.Get().CreateBox();
			MenuConfigurationOptionsUI.cursorBox.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.cursorBox.SizeOffset_X = 240f;
			MenuConfigurationOptionsUI.cursorBox.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.cursorBox.Text = MenuConfigurationOptionsUI.localization.format("Cursor_Box");
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.cursorBox);
			num += 40f;
			MenuConfigurationOptionsUI.cursorColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.cursorColorPicker.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.cursorColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onCursorColorPicked);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.cursorColorPicker);
			num += 130f;
			MenuConfigurationOptionsUI.backgroundBox = Glazier.Get().CreateBox();
			MenuConfigurationOptionsUI.backgroundBox.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.backgroundBox.SizeOffset_X = 240f;
			MenuConfigurationOptionsUI.backgroundBox.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.backgroundBox.Text = MenuConfigurationOptionsUI.localization.format("Background_Box");
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.backgroundBox);
			num += 40f;
			MenuConfigurationOptionsUI.backgroundColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.backgroundColorPicker.PositionOffset_Y = num;
			if (Provider.isPro)
			{
				MenuConfigurationOptionsUI.backgroundColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onBackgroundColorPicked);
			}
			else
			{
				Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				ISleekImage sleekImage = Glazier.Get().CreateImage();
				sleekImage.PositionOffset_X = -40f;
				sleekImage.PositionOffset_Y = -40f;
				sleekImage.PositionScale_X = 0.5f;
				sleekImage.PositionScale_Y = 0.5f;
				sleekImage.SizeOffset_X = 80f;
				sleekImage.SizeOffset_Y = 80f;
				sleekImage.Texture = bundle.load<Texture2D>("Lock_Large");
				MenuConfigurationOptionsUI.backgroundColorPicker.AddChild(sleekImage);
				bundle.unload();
			}
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.backgroundColorPicker);
			num += 130f;
			MenuConfigurationOptionsUI.foregroundBox = Glazier.Get().CreateBox();
			MenuConfigurationOptionsUI.foregroundBox.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.foregroundBox.SizeOffset_X = 240f;
			MenuConfigurationOptionsUI.foregroundBox.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.foregroundBox.Text = MenuConfigurationOptionsUI.localization.format("Foreground_Box");
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.foregroundBox);
			num += 40f;
			MenuConfigurationOptionsUI.foregroundColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.foregroundColorPicker.PositionOffset_Y = num;
			if (Provider.isPro)
			{
				MenuConfigurationOptionsUI.foregroundColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onForegroundColorPicked);
			}
			else
			{
				Bundle bundle2 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				ISleekImage sleekImage2 = Glazier.Get().CreateImage();
				sleekImage2.PositionOffset_X = -40f;
				sleekImage2.PositionOffset_Y = -40f;
				sleekImage2.PositionScale_X = 0.5f;
				sleekImage2.PositionScale_Y = 0.5f;
				sleekImage2.SizeOffset_X = 80f;
				sleekImage2.SizeOffset_Y = 80f;
				sleekImage2.Texture = bundle2.load<Texture2D>("Lock_Large");
				MenuConfigurationOptionsUI.foregroundColorPicker.AddChild(sleekImage2);
				bundle2.unload();
			}
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.foregroundColorPicker);
			num += 130f;
			MenuConfigurationOptionsUI.fontBox = Glazier.Get().CreateBox();
			MenuConfigurationOptionsUI.fontBox.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.fontBox.SizeOffset_X = 240f;
			MenuConfigurationOptionsUI.fontBox.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.fontBox.Text = MenuConfigurationOptionsUI.localization.format("Font_Box");
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.fontBox);
			num += 40f;
			MenuConfigurationOptionsUI.fontColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.fontColorPicker.PositionOffset_Y = num;
			if (Provider.isPro)
			{
				MenuConfigurationOptionsUI.fontColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onFontColorPicked);
			}
			else
			{
				Bundle bundle3 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				ISleekImage sleekImage3 = Glazier.Get().CreateImage();
				sleekImage3.PositionOffset_X = -40f;
				sleekImage3.PositionOffset_Y = -40f;
				sleekImage3.PositionScale_X = 0.5f;
				sleekImage3.PositionScale_Y = 0.5f;
				sleekImage3.SizeOffset_X = 80f;
				sleekImage3.SizeOffset_Y = 80f;
				sleekImage3.Texture = bundle3.load<Texture2D>("Lock_Large");
				MenuConfigurationOptionsUI.fontColorPicker.AddChild(sleekImage3);
				bundle3.unload();
			}
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.fontColorPicker);
			num += 130f;
			MenuConfigurationOptionsUI.shadowBox = Glazier.Get().CreateBox();
			MenuConfigurationOptionsUI.shadowBox.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.shadowBox.SizeOffset_X = 240f;
			MenuConfigurationOptionsUI.shadowBox.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.shadowBox.Text = MenuConfigurationOptionsUI.localization.format("Shadow_Box");
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.shadowBox);
			num += MenuConfigurationOptionsUI.shadowBox.SizeOffset_Y + 10f;
			MenuConfigurationOptionsUI.shadowColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.shadowColorPicker.PositionOffset_Y = num;
			if (Provider.isPro)
			{
				MenuConfigurationOptionsUI.shadowColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onShadowColorPicked);
			}
			else
			{
				Bundle bundle4 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				ISleekImage sleekImage4 = Glazier.Get().CreateImage();
				sleekImage4.PositionOffset_X = -40f;
				sleekImage4.PositionOffset_Y = -40f;
				sleekImage4.PositionScale_X = 0.5f;
				sleekImage4.PositionScale_Y = 0.5f;
				sleekImage4.SizeOffset_X = 80f;
				sleekImage4.SizeOffset_Y = 80f;
				sleekImage4.Texture = bundle4.load<Texture2D>("Lock_Large");
				MenuConfigurationOptionsUI.shadowColorPicker.AddChild(sleekImage4);
				bundle4.unload();
			}
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.shadowColorPicker);
			num += MenuConfigurationOptionsUI.shadowColorPicker.SizeOffset_Y + 10f;
			MenuConfigurationOptionsUI.badColorBox = Glazier.Get().CreateBox();
			MenuConfigurationOptionsUI.badColorBox.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.badColorBox.SizeOffset_X = 240f;
			MenuConfigurationOptionsUI.badColorBox.SizeOffset_Y = 30f;
			MenuConfigurationOptionsUI.badColorBox.Text = MenuConfigurationOptionsUI.localization.format("Bad_Color_Box");
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.badColorBox);
			num += MenuConfigurationOptionsUI.badColorBox.SizeOffset_Y + 10f;
			MenuConfigurationOptionsUI.badColorPicker = new SleekColorPicker();
			MenuConfigurationOptionsUI.badColorPicker.PositionOffset_Y = num;
			if (Provider.isPro)
			{
				MenuConfigurationOptionsUI.badColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onBadColorPicked);
			}
			else
			{
				Bundle bundle5 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				ISleekImage sleekImage5 = Glazier.Get().CreateImage();
				sleekImage5.PositionOffset_X = -40f;
				sleekImage5.PositionOffset_Y = -40f;
				sleekImage5.PositionScale_X = 0.5f;
				sleekImage5.PositionScale_Y = 0.5f;
				sleekImage5.SizeOffset_X = 80f;
				sleekImage5.SizeOffset_Y = 80f;
				sleekImage5.Texture = bundle5.load<Texture2D>("Lock_Large");
				MenuConfigurationOptionsUI.badColorPicker.AddChild(sleekImage5);
				bundle5.unload();
			}
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.badColorPicker);
			num += MenuConfigurationOptionsUI.badColorPicker.SizeOffset_Y;
			MenuConfigurationOptionsUI.dontShowOnlineSafetyMenuAgainToggle = Glazier.Get().CreateToggle();
			MenuConfigurationOptionsUI.dontShowOnlineSafetyMenuAgainToggle.PositionOffset_Y = num;
			MenuConfigurationOptionsUI.dontShowOnlineSafetyMenuAgainToggle.SizeOffset_X = 40f;
			MenuConfigurationOptionsUI.dontShowOnlineSafetyMenuAgainToggle.SizeOffset_Y = 40f;
			MenuConfigurationOptionsUI.dontShowOnlineSafetyMenuAgainToggle.AddLabel(MenuConfigurationOptionsUI.localization.format("DontShowOnlineSafetyMenuAgain_Label"), 1);
			MenuConfigurationOptionsUI.dontShowOnlineSafetyMenuAgainToggle.TooltipText = MenuConfigurationOptionsUI.localization.format("DontShowOnlineSafetyMenuAgain_Tooltip");
			MenuConfigurationOptionsUI.dontShowOnlineSafetyMenuAgainToggle.OnValueChanged += new Toggled(MenuConfigurationOptionsUI.OnDontShowOnlineSafetyMenuAgainToggled);
			MenuConfigurationOptionsUI.optionsBox.AddChild(MenuConfigurationOptionsUI.dontShowOnlineSafetyMenuAgainToggle);
			num += 50f;
			MenuConfigurationOptionsUI.optionsBox.ContentSizeOffset = new Vector2(0f, num - 10f);
			MenuConfigurationOptionsUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuConfigurationOptionsUI.backButton.PositionOffset_Y = -50f;
			MenuConfigurationOptionsUI.backButton.PositionScale_Y = 1f;
			MenuConfigurationOptionsUI.backButton.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.backButton.SizeOffset_Y = 50f;
			MenuConfigurationOptionsUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuConfigurationOptionsUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuConfigurationOptionsUI.backButton.onClickedButton += new ClickedButton(MenuConfigurationOptionsUI.onClickedBackButton);
			MenuConfigurationOptionsUI.backButton.fontSize = 3;
			MenuConfigurationOptionsUI.backButton.iconColor = 2;
			MenuConfigurationOptionsUI.container.AddChild(MenuConfigurationOptionsUI.backButton);
			MenuConfigurationOptionsUI.defaultButton = Glazier.Get().CreateButton();
			MenuConfigurationOptionsUI.defaultButton.PositionOffset_X = -200f;
			MenuConfigurationOptionsUI.defaultButton.PositionOffset_Y = -50f;
			MenuConfigurationOptionsUI.defaultButton.PositionScale_X = 1f;
			MenuConfigurationOptionsUI.defaultButton.PositionScale_Y = 1f;
			MenuConfigurationOptionsUI.defaultButton.SizeOffset_X = 200f;
			MenuConfigurationOptionsUI.defaultButton.SizeOffset_Y = 50f;
			MenuConfigurationOptionsUI.defaultButton.Text = MenuPlayConfigUI.localization.format("Default");
			MenuConfigurationOptionsUI.defaultButton.TooltipText = MenuPlayConfigUI.localization.format("Default_Tooltip");
			MenuConfigurationOptionsUI.defaultButton.OnClicked += new ClickedButton(MenuConfigurationOptionsUI.onClickedDefaultButton);
			MenuConfigurationOptionsUI.defaultButton.FontSize = 3;
			MenuConfigurationOptionsUI.container.AddChild(MenuConfigurationOptionsUI.defaultButton);
		}

		// Token: 0x040028E8 RID: 10472
		private static Local localization;

		// Token: 0x040028E9 RID: 10473
		private static SleekFullscreenBox container;

		// Token: 0x040028EA RID: 10474
		public static bool active;

		// Token: 0x040028EB RID: 10475
		private static SleekButtonIcon backButton;

		// Token: 0x040028EC RID: 10476
		private static ISleekButton defaultButton;

		// Token: 0x040028ED RID: 10477
		private static ISleekScrollView optionsBox;

		// Token: 0x040028EE RID: 10478
		private static ISleekSlider fovSlider;

		// Token: 0x040028EF RID: 10479
		private static ISleekLabel fovLabel;

		// Token: 0x040028F0 RID: 10480
		private static ISleekToggle debugToggle;

		// Token: 0x040028F1 RID: 10481
		private static ISleekToggle timerToggle;

		// Token: 0x040028F2 RID: 10482
		private static ISleekToggle goreToggle;

		// Token: 0x040028F3 RID: 10483
		private static ISleekToggle filterToggle;

		// Token: 0x040028F4 RID: 10484
		private static ISleekToggle chatTextToggle;

		// Token: 0x040028F5 RID: 10485
		private static ISleekToggle chatVoiceInToggle;

		// Token: 0x040028F6 RID: 10486
		private static ISleekToggle chatVoiceOutToggle;

		// Token: 0x040028F7 RID: 10487
		private static ISleekToggle chatVoiceAlwaysRecordingToggle;

		// Token: 0x040028F8 RID: 10488
		private static ISleekToggle hintsToggle;

		// Token: 0x040028F9 RID: 10489
		private static ISleekToggle streamerToggle;

		// Token: 0x040028FA RID: 10490
		private static ISleekToggle featuredWorkshopToggle;

		// Token: 0x040028FB RID: 10491
		private static ISleekToggle showHotbarToggle;

		// Token: 0x040028FC RID: 10492
		private static ISleekToggle pauseWhenUnfocusedToggle;

		// Token: 0x040028FD RID: 10493
		private static ISleekToggle nametagFadeOutToggle;

		// Token: 0x040028FE RID: 10494
		private static ISleekInt32Field screenshotSizeMultiplierField;

		// Token: 0x040028FF RID: 10495
		private static ISleekToggle screenshotSupersamplingToggle;

		// Token: 0x04002900 RID: 10496
		private static ISleekToggle screenshotsWhileLoadingToggle;

		// Token: 0x04002901 RID: 10497
		private static ISleekToggle staticCrosshairToggle;

		// Token: 0x04002902 RID: 10498
		private static ISleekSlider staticCrosshairSizeSlider;

		// Token: 0x04002903 RID: 10499
		private static SleekButtonState crosshairShapeButton;

		// Token: 0x04002904 RID: 10500
		private static SleekButtonState metricButton;

		// Token: 0x04002905 RID: 10501
		private static SleekButtonState talkButton;

		// Token: 0x04002906 RID: 10502
		private static SleekButtonState uiButton;

		// Token: 0x04002907 RID: 10503
		private static SleekButtonState hitmarkerButton;

		// Token: 0x04002908 RID: 10504
		private static SleekButtonState hitmarkerStyleButton;

		// Token: 0x04002909 RID: 10505
		private static SleekButtonState vehicleThirdPersonCameraModeButton;

		// Token: 0x0400290A RID: 10506
		private static SleekButtonState aircraftThirdPersonCameraModeButton;

		// Token: 0x0400290B RID: 10507
		private static ISleekBox crosshairBox;

		// Token: 0x0400290C RID: 10508
		private static SleekColorPicker crosshairColorPicker;

		// Token: 0x0400290D RID: 10509
		private static ISleekBox hitmarkerBox;

		// Token: 0x0400290E RID: 10510
		private static SleekColorPicker hitmarkerColorPicker;

		// Token: 0x0400290F RID: 10511
		private static ISleekBox criticalHitmarkerBox;

		// Token: 0x04002910 RID: 10512
		private static SleekColorPicker criticalHitmarkerColorPicker;

		// Token: 0x04002911 RID: 10513
		private static ISleekBox cursorBox;

		// Token: 0x04002912 RID: 10514
		private static SleekColorPicker cursorColorPicker;

		// Token: 0x04002913 RID: 10515
		private static ISleekBox backgroundBox;

		// Token: 0x04002914 RID: 10516
		private static SleekColorPicker backgroundColorPicker;

		// Token: 0x04002915 RID: 10517
		private static ISleekBox foregroundBox;

		// Token: 0x04002916 RID: 10518
		private static SleekColorPicker foregroundColorPicker;

		// Token: 0x04002917 RID: 10519
		private static ISleekBox fontBox;

		// Token: 0x04002918 RID: 10520
		private static SleekColorPicker fontColorPicker;

		// Token: 0x04002919 RID: 10521
		private static ISleekBox shadowBox;

		// Token: 0x0400291A RID: 10522
		private static SleekColorPicker shadowColorPicker;

		// Token: 0x0400291B RID: 10523
		private static ISleekBox badColorBox;

		// Token: 0x0400291C RID: 10524
		private static SleekColorPicker badColorPicker;

		// Token: 0x0400291D RID: 10525
		private static ISleekToggle dontShowOnlineSafetyMenuAgainToggle;
	}
}
