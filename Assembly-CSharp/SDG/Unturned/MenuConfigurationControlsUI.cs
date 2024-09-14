using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200078B RID: 1931
	public class MenuConfigurationControlsUI
	{
		// Token: 0x06003FC7 RID: 16327 RVA: 0x001435F5 File Offset: 0x001417F5
		public static void open()
		{
			if (MenuConfigurationControlsUI.active)
			{
				return;
			}
			MenuConfigurationControlsUI.active = true;
			MenuConfigurationControlsUI.container.AnimateIntoView();
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x0014360F File Offset: 0x0014180F
		public static void close()
		{
			if (!MenuConfigurationControlsUI.active)
			{
				return;
			}
			MenuConfigurationControlsUI.active = false;
			MenuConfigurationControlsUI.binding = byte.MaxValue;
			MenuSettings.SaveControlsIfLoaded();
			MenuConfigurationControlsUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x00143642 File Offset: 0x00141842
		public static void cancel()
		{
			MenuConfigurationControlsUI.binding = byte.MaxValue;
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x0014364E File Offset: 0x0014184E
		public static void bind(KeyCode key)
		{
			ControlsSettings.bind(MenuConfigurationControlsUI.binding, key);
			MenuConfigurationControlsUI.updateButton(MenuConfigurationControlsUI.binding);
			MenuConfigurationControlsUI.cancel();
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x0014366C File Offset: 0x0014186C
		public static string getKeyCodeText(KeyCode key)
		{
			if (MenuConfigurationControlsUI.localizationKeyCodes == null)
			{
				MenuConfigurationControlsUI.localizationKeyCodes = Localization.read("/Shared/KeyCodes.dat");
			}
			string text = key.ToString();
			if (MenuConfigurationControlsUI.localizationKeyCodes.has(text))
			{
				text = MenuConfigurationControlsUI.localizationKeyCodes.format(text);
			}
			return text;
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x001436B8 File Offset: 0x001418B8
		public static void updateButton(byte index)
		{
			string keyCodeText = MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.bindings[(int)index].key);
			MenuConfigurationControlsUI.buttons[(int)index].Text = MenuConfigurationControlsUI.localization.format("Key_" + index.ToString() + "_Button", keyCodeText);
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x00143704 File Offset: 0x00141904
		private static void onTypedSensitivityField(ISleekFloat32Field field, float state)
		{
			ControlsSettings.mouseAimSensitivity = state;
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x0014370C File Offset: 0x0014190C
		private static void onTypedProjectionRatioCoefficientField(ISleekFloat32Field field, float state)
		{
			ControlsSettings.projectionRatioCoefficient = state;
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x00143714 File Offset: 0x00141914
		private static void onToggledInvertToggle(ISleekToggle toggle, bool state)
		{
			ControlsSettings.invert = state;
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x0014371C File Offset: 0x0014191C
		private static void onToggledInvertFlightToggle(ISleekToggle toggle, bool state)
		{
			ControlsSettings.invertFlight = state;
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x00143724 File Offset: 0x00141924
		private static void onSwappedAimingState(SleekButtonState button, int index)
		{
			ControlsSettings.aiming = (EControlMode)index;
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x0014372C File Offset: 0x0014192C
		private static void onSwappedCrouchingState(SleekButtonState button, int index)
		{
			ControlsSettings.crouching = (EControlMode)index;
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x00143734 File Offset: 0x00141934
		private static void onSwappedProningState(SleekButtonState button, int index)
		{
			ControlsSettings.proning = (EControlMode)index;
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x0014373C File Offset: 0x0014193C
		private static void onSwappedSprintingState(SleekButtonState button, int index)
		{
			ControlsSettings.sprinting = (EControlMode)index;
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x00143744 File Offset: 0x00141944
		private static void onSwappedLeaningState(SleekButtonState button, int index)
		{
			ControlsSettings.leaning = (EControlMode)index;
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x0014374C File Offset: 0x0014194C
		private static void OnSwappedVoiceMode(SleekButtonState button, int index)
		{
			ControlsSettings.voiceMode = (EControlMode)index;
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x00143754 File Offset: 0x00141954
		private static void OnSwappedSensitivityScalingMode(SleekButtonState button, int index)
		{
			ControlsSettings.sensitivityScalingMode = (ESensitivityScalingMode)index;
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x0014375C File Offset: 0x0014195C
		private static void onClickedKeyButton(ISleekElement button)
		{
			MenuConfigurationControlsUI.binding = 0;
			while ((int)MenuConfigurationControlsUI.binding < MenuConfigurationControlsUI.buttons.Length && MenuConfigurationControlsUI.buttons[(int)MenuConfigurationControlsUI.binding] != button)
			{
				MenuConfigurationControlsUI.binding += 1;
			}
			(button as ISleekButton).Text = MenuConfigurationControlsUI.localization.format("Key_" + MenuConfigurationControlsUI.binding.ToString() + "_Button", "?");
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x001437D0 File Offset: 0x001419D0
		public static void bindOnGUI()
		{
			if (MenuConfigurationControlsUI.binding != 255)
			{
				if (Event.current.type == 4)
				{
					if (Event.current.keyCode == KeyCode.Backspace)
					{
						MenuConfigurationControlsUI.updateButton(MenuConfigurationControlsUI.binding);
						MenuConfigurationControlsUI.cancel();
						return;
					}
					if (Event.current.keyCode != KeyCode.Escape && Event.current.keyCode != KeyCode.Insert)
					{
						MenuConfigurationControlsUI.bind(Event.current.keyCode);
						return;
					}
				}
				else if (Event.current.type == null)
				{
					if (Event.current.button == 0)
					{
						MenuConfigurationControlsUI.bind(KeyCode.Mouse0);
						return;
					}
					if (Event.current.button == 1)
					{
						MenuConfigurationControlsUI.bind(KeyCode.Mouse1);
						return;
					}
					if (Event.current.button == 2)
					{
						MenuConfigurationControlsUI.bind(KeyCode.Mouse2);
						return;
					}
					if (Event.current.button == 3)
					{
						MenuConfigurationControlsUI.bind(KeyCode.Mouse3);
						return;
					}
					if (Event.current.button == 4)
					{
						MenuConfigurationControlsUI.bind(KeyCode.Mouse4);
						return;
					}
					if (Event.current.button == 5)
					{
						MenuConfigurationControlsUI.bind(KeyCode.Mouse5);
						return;
					}
					if (Event.current.button == 6)
					{
						MenuConfigurationControlsUI.bind(KeyCode.Mouse6);
						return;
					}
				}
				else if (Event.current.shift)
				{
					MenuConfigurationControlsUI.bind(KeyCode.LeftShift);
				}
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x00143918 File Offset: 0x00141B18
		public static void bindUpdate()
		{
			if (MenuConfigurationControlsUI.binding != 255)
			{
				if (InputEx.GetKeyDown(KeyCode.Mouse3))
				{
					MenuConfigurationControlsUI.bind(KeyCode.Mouse3);
					return;
				}
				if (InputEx.GetKeyDown(KeyCode.Mouse4))
				{
					MenuConfigurationControlsUI.bind(KeyCode.Mouse4);
					return;
				}
				if (InputEx.GetKeyDown(KeyCode.Mouse5))
				{
					MenuConfigurationControlsUI.bind(KeyCode.Mouse5);
					return;
				}
				if (InputEx.GetKeyDown(KeyCode.Mouse6))
				{
					MenuConfigurationControlsUI.bind(KeyCode.Mouse6);
				}
			}
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x0014398C File Offset: 0x00141B8C
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
			MenuConfigurationControlsUI.close();
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x001439BA File Offset: 0x00141BBA
		private static void onClickedDefaultButton(ISleekElement button)
		{
			ControlsSettings.restoreDefaults();
			MenuConfigurationControlsUI.updateAll();
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x001439C8 File Offset: 0x00141BC8
		private static void updateAll()
		{
			byte b = 0;
			while ((int)b < MenuConfigurationControlsUI.layouts.Length)
			{
				byte b2 = 0;
				while ((int)b2 < MenuConfigurationControlsUI.layouts[(int)b].Length)
				{
					MenuConfigurationControlsUI.updateButton(MenuConfigurationControlsUI.layouts[(int)b][(int)b2]);
					b2 += 1;
				}
				b += 1;
			}
			MenuConfigurationControlsUI.leaningButton.state = (int)ControlsSettings.leaning;
			MenuConfigurationControlsUI.sprintingButton.state = (int)ControlsSettings.sprinting;
			MenuConfigurationControlsUI.proningButton.state = (int)ControlsSettings.proning;
			MenuConfigurationControlsUI.crouchingButton.state = (int)ControlsSettings.crouching;
			MenuConfigurationControlsUI.aimingButton.state = (int)ControlsSettings.aiming;
			MenuConfigurationControlsUI.sensitivityField.Value = ControlsSettings.mouseAimSensitivity;
			MenuConfigurationControlsUI.projectionRatioCoefficientField.Value = ControlsSettings.projectionRatioCoefficient;
			MenuConfigurationControlsUI.voiceModeButton.state = (int)ControlsSettings.voiceMode;
			MenuConfigurationControlsUI.invertToggle.Value = ControlsSettings.invert;
			MenuConfigurationControlsUI.invertFlightToggle.Value = ControlsSettings.invert;
			MenuConfigurationControlsUI.sensitivityScalingModeButton.state = (int)ControlsSettings.sensitivityScalingMode;
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x00143AB0 File Offset: 0x00141CB0
		public MenuConfigurationControlsUI()
		{
			MenuConfigurationControlsUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationControls.dat");
			MenuConfigurationControlsUI.container = new SleekFullscreenBox();
			MenuConfigurationControlsUI.container.PositionOffset_X = 10f;
			MenuConfigurationControlsUI.container.PositionOffset_Y = 10f;
			MenuConfigurationControlsUI.container.PositionScale_Y = 1f;
			MenuConfigurationControlsUI.container.SizeOffset_X = -20f;
			MenuConfigurationControlsUI.container.SizeOffset_Y = -20f;
			MenuConfigurationControlsUI.container.SizeScale_X = 1f;
			MenuConfigurationControlsUI.container.SizeScale_Y = 1f;
			if (Provider.isConnected)
			{
				PlayerUI.container.AddChild(MenuConfigurationControlsUI.container);
			}
			else if (Level.isEditor)
			{
				EditorUI.window.AddChild(MenuConfigurationControlsUI.container);
			}
			else
			{
				MenuUI.container.AddChild(MenuConfigurationControlsUI.container);
			}
			MenuConfigurationControlsUI.active = false;
			MenuConfigurationControlsUI.binding = byte.MaxValue;
			MenuConfigurationControlsUI.controlsBox = Glazier.Get().CreateScrollView();
			MenuConfigurationControlsUI.controlsBox.PositionOffset_X = -200f;
			MenuConfigurationControlsUI.controlsBox.PositionOffset_Y = 100f;
			MenuConfigurationControlsUI.controlsBox.PositionScale_X = 0.5f;
			MenuConfigurationControlsUI.controlsBox.SizeOffset_X = 430f;
			MenuConfigurationControlsUI.controlsBox.SizeOffset_Y = -200f;
			MenuConfigurationControlsUI.controlsBox.SizeScale_Y = 1f;
			MenuConfigurationControlsUI.controlsBox.ScaleContentToWidth = true;
			MenuConfigurationControlsUI.container.AddChild(MenuConfigurationControlsUI.controlsBox);
			int num = 0;
			MenuConfigurationControlsUI.invertToggle = Glazier.Get().CreateToggle();
			MenuConfigurationControlsUI.invertToggle.PositionOffset_Y = (float)num;
			MenuConfigurationControlsUI.invertToggle.SizeOffset_X = 40f;
			MenuConfigurationControlsUI.invertToggle.SizeOffset_Y = 40f;
			MenuConfigurationControlsUI.invertToggle.AddLabel(MenuConfigurationControlsUI.localization.format("Invert_Toggle_Label"), 1);
			MenuConfigurationControlsUI.invertToggle.OnValueChanged += new Toggled(MenuConfigurationControlsUI.onToggledInvertToggle);
			MenuConfigurationControlsUI.controlsBox.AddChild(MenuConfigurationControlsUI.invertToggle);
			num += 50;
			MenuConfigurationControlsUI.invertFlightToggle = Glazier.Get().CreateToggle();
			MenuConfigurationControlsUI.invertFlightToggle.PositionOffset_Y = (float)num;
			MenuConfigurationControlsUI.invertFlightToggle.SizeOffset_X = 40f;
			MenuConfigurationControlsUI.invertFlightToggle.SizeOffset_Y = 40f;
			MenuConfigurationControlsUI.invertFlightToggle.AddLabel(MenuConfigurationControlsUI.localization.format("Invert_Flight_Toggle_Label"), 1);
			MenuConfigurationControlsUI.invertFlightToggle.OnValueChanged += new Toggled(MenuConfigurationControlsUI.onToggledInvertFlightToggle);
			MenuConfigurationControlsUI.controlsBox.AddChild(MenuConfigurationControlsUI.invertFlightToggle);
			num += 50;
			MenuConfigurationControlsUI.sensitivityField = Glazier.Get().CreateFloat32Field();
			MenuConfigurationControlsUI.sensitivityField.PositionOffset_Y = (float)num;
			MenuConfigurationControlsUI.sensitivityField.SizeOffset_X = 200f;
			MenuConfigurationControlsUI.sensitivityField.SizeOffset_Y = 30f;
			MenuConfigurationControlsUI.sensitivityField.AddLabel(MenuConfigurationControlsUI.localization.format("Sensitivity_Field_Label"), 1);
			MenuConfigurationControlsUI.sensitivityField.OnValueChanged += new TypedSingle(MenuConfigurationControlsUI.onTypedSensitivityField);
			MenuConfigurationControlsUI.controlsBox.AddChild(MenuConfigurationControlsUI.sensitivityField);
			num += 40;
			MenuConfigurationControlsUI.sensitivityScalingModeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("SensitivityScalingMode_ProjectionRatio"), MenuConfigurationControlsUI.localization.format("SensitivityScalingMode_ProjectionRatio_Tooltip")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("SensitivityScalingMode_ZoomFactor"), MenuConfigurationControlsUI.localization.format("SensitivityScalingMode_ZoomFactor_Tooltip")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("SensitivityScalingMode_Legacy"), MenuConfigurationControlsUI.localization.format("SensitivityScalingMode_Legacy_Tooltip")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("SensitivityScalingMode_None"), MenuConfigurationControlsUI.localization.format("SensitivityScalingMode_None_Tooltip"))
			});
			MenuConfigurationControlsUI.sensitivityScalingModeButton.PositionOffset_Y = (float)num;
			MenuConfigurationControlsUI.sensitivityScalingModeButton.SizeOffset_X = 200f;
			MenuConfigurationControlsUI.sensitivityScalingModeButton.SizeOffset_Y = 30f;
			MenuConfigurationControlsUI.sensitivityScalingModeButton.AddLabel(MenuConfigurationControlsUI.localization.format("SensitivityScalingMode_Label"), 1);
			MenuConfigurationControlsUI.sensitivityScalingModeButton.onSwappedState = new SwappedState(MenuConfigurationControlsUI.OnSwappedSensitivityScalingMode);
			MenuConfigurationControlsUI.sensitivityScalingModeButton.UseContentTooltip = true;
			MenuConfigurationControlsUI.controlsBox.AddChild(MenuConfigurationControlsUI.sensitivityScalingModeButton);
			num += 40;
			MenuConfigurationControlsUI.projectionRatioCoefficientField = Glazier.Get().CreateFloat32Field();
			MenuConfigurationControlsUI.projectionRatioCoefficientField.PositionOffset_Y = (float)num;
			MenuConfigurationControlsUI.projectionRatioCoefficientField.SizeOffset_X = 200f;
			MenuConfigurationControlsUI.projectionRatioCoefficientField.SizeOffset_Y = 30f;
			MenuConfigurationControlsUI.projectionRatioCoefficientField.TooltipText = MenuConfigurationControlsUI.localization.format("ProjectionRatioCoefficient_Tooltip");
			MenuConfigurationControlsUI.projectionRatioCoefficientField.AddLabel(MenuConfigurationControlsUI.localization.format("ProjectionRatioCoefficient_Label"), 1);
			MenuConfigurationControlsUI.projectionRatioCoefficientField.OnValueChanged += new TypedSingle(MenuConfigurationControlsUI.onTypedProjectionRatioCoefficientField);
			MenuConfigurationControlsUI.controlsBox.AddChild(MenuConfigurationControlsUI.projectionRatioCoefficientField);
			num += 40;
			MenuConfigurationControlsUI.aimingButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
			});
			MenuConfigurationControlsUI.aimingButton.PositionOffset_Y = (float)num;
			MenuConfigurationControlsUI.aimingButton.SizeOffset_X = 200f;
			MenuConfigurationControlsUI.aimingButton.SizeOffset_Y = 30f;
			MenuConfigurationControlsUI.aimingButton.AddLabel(MenuConfigurationControlsUI.localization.format("Aiming_Label"), 1);
			MenuConfigurationControlsUI.aimingButton.onSwappedState = new SwappedState(MenuConfigurationControlsUI.onSwappedAimingState);
			MenuConfigurationControlsUI.controlsBox.AddChild(MenuConfigurationControlsUI.aimingButton);
			num += 40;
			MenuConfigurationControlsUI.crouchingButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
			});
			MenuConfigurationControlsUI.crouchingButton.PositionOffset_Y = (float)num;
			MenuConfigurationControlsUI.crouchingButton.SizeOffset_X = 200f;
			MenuConfigurationControlsUI.crouchingButton.SizeOffset_Y = 30f;
			MenuConfigurationControlsUI.crouchingButton.AddLabel(MenuConfigurationControlsUI.localization.format("Crouching_Label"), 1);
			MenuConfigurationControlsUI.crouchingButton.onSwappedState = new SwappedState(MenuConfigurationControlsUI.onSwappedCrouchingState);
			MenuConfigurationControlsUI.controlsBox.AddChild(MenuConfigurationControlsUI.crouchingButton);
			num += 40;
			MenuConfigurationControlsUI.proningButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
			});
			MenuConfigurationControlsUI.proningButton.PositionOffset_Y = (float)num;
			MenuConfigurationControlsUI.proningButton.SizeOffset_X = 200f;
			MenuConfigurationControlsUI.proningButton.SizeOffset_Y = 30f;
			MenuConfigurationControlsUI.proningButton.AddLabel(MenuConfigurationControlsUI.localization.format("Proning_Label"), 1);
			MenuConfigurationControlsUI.proningButton.onSwappedState = new SwappedState(MenuConfigurationControlsUI.onSwappedProningState);
			MenuConfigurationControlsUI.controlsBox.AddChild(MenuConfigurationControlsUI.proningButton);
			num += 40;
			MenuConfigurationControlsUI.sprintingButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
			});
			MenuConfigurationControlsUI.sprintingButton.PositionOffset_Y = (float)num;
			MenuConfigurationControlsUI.sprintingButton.SizeOffset_X = 200f;
			MenuConfigurationControlsUI.sprintingButton.SizeOffset_Y = 30f;
			MenuConfigurationControlsUI.sprintingButton.AddLabel(MenuConfigurationControlsUI.localization.format("Sprinting_Label"), 1);
			MenuConfigurationControlsUI.sprintingButton.onSwappedState = new SwappedState(MenuConfigurationControlsUI.onSwappedSprintingState);
			MenuConfigurationControlsUI.controlsBox.AddChild(MenuConfigurationControlsUI.sprintingButton);
			num += 40;
			MenuConfigurationControlsUI.leaningButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
			});
			MenuConfigurationControlsUI.leaningButton.PositionOffset_Y = (float)num;
			MenuConfigurationControlsUI.leaningButton.SizeOffset_X = 200f;
			MenuConfigurationControlsUI.leaningButton.SizeOffset_Y = 30f;
			MenuConfigurationControlsUI.leaningButton.AddLabel(MenuConfigurationControlsUI.localization.format("Leaning_Label"), 1);
			MenuConfigurationControlsUI.leaningButton.onSwappedState = new SwappedState(MenuConfigurationControlsUI.onSwappedLeaningState);
			MenuConfigurationControlsUI.controlsBox.AddChild(MenuConfigurationControlsUI.leaningButton);
			num += 40;
			MenuConfigurationControlsUI.voiceModeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
				new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
			});
			MenuConfigurationControlsUI.voiceModeButton.PositionOffset_Y = (float)num;
			MenuConfigurationControlsUI.voiceModeButton.SizeOffset_X = 200f;
			MenuConfigurationControlsUI.voiceModeButton.SizeOffset_Y = 30f;
			MenuConfigurationControlsUI.voiceModeButton.AddLabel(MenuConfigurationControlsUI.localization.format("Voice_Mode_Label"), 1);
			MenuConfigurationControlsUI.voiceModeButton.onSwappedState = new SwappedState(MenuConfigurationControlsUI.OnSwappedVoiceMode);
			MenuConfigurationControlsUI.controlsBox.AddChild(MenuConfigurationControlsUI.voiceModeButton);
			num += 40;
			MenuConfigurationControlsUI.buttons = new ISleekButton[ControlsSettings.bindings.Length];
			byte b = 0;
			while ((int)b < MenuConfigurationControlsUI.layouts.Length)
			{
				ISleekBox sleekBox = Glazier.Get().CreateBox();
				sleekBox.PositionOffset_Y = (float)num;
				sleekBox.SizeOffset_Y = 30f;
				sleekBox.SizeScale_X = 1f;
				sleekBox.Text = MenuConfigurationControlsUI.localization.format("Layout_" + b.ToString());
				MenuConfigurationControlsUI.controlsBox.AddChild(sleekBox);
				num += 40;
				byte b2 = 0;
				while ((int)b2 < MenuConfigurationControlsUI.layouts[(int)b].Length)
				{
					ISleekButton sleekButton = Glazier.Get().CreateButton();
					sleekButton.PositionOffset_Y = (float)(40 + b2 * 30);
					sleekButton.SizeOffset_Y = 30f;
					sleekButton.SizeScale_X = 1f;
					sleekButton.OnClicked += new ClickedButton(MenuConfigurationControlsUI.onClickedKeyButton);
					sleekBox.AddChild(sleekButton);
					num += 30;
					MenuConfigurationControlsUI.buttons[(int)MenuConfigurationControlsUI.layouts[(int)b][(int)b2]] = sleekButton;
					b2 += 1;
				}
				num += 10;
				b += 1;
			}
			MenuConfigurationControlsUI.controlsBox.ContentSizeOffset = new Vector2(0f, (float)(num - 10));
			MenuConfigurationControlsUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuConfigurationControlsUI.backButton.PositionOffset_Y = -50f;
			MenuConfigurationControlsUI.backButton.PositionScale_Y = 1f;
			MenuConfigurationControlsUI.backButton.SizeOffset_X = 200f;
			MenuConfigurationControlsUI.backButton.SizeOffset_Y = 50f;
			MenuConfigurationControlsUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuConfigurationControlsUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuConfigurationControlsUI.backButton.onClickedButton += new ClickedButton(MenuConfigurationControlsUI.onClickedBackButton);
			MenuConfigurationControlsUI.backButton.fontSize = 3;
			MenuConfigurationControlsUI.backButton.iconColor = 2;
			MenuConfigurationControlsUI.container.AddChild(MenuConfigurationControlsUI.backButton);
			MenuConfigurationControlsUI.defaultButton = Glazier.Get().CreateButton();
			MenuConfigurationControlsUI.defaultButton.PositionOffset_X = -200f;
			MenuConfigurationControlsUI.defaultButton.PositionOffset_Y = -50f;
			MenuConfigurationControlsUI.defaultButton.PositionScale_X = 1f;
			MenuConfigurationControlsUI.defaultButton.PositionScale_Y = 1f;
			MenuConfigurationControlsUI.defaultButton.SizeOffset_X = 200f;
			MenuConfigurationControlsUI.defaultButton.SizeOffset_Y = 50f;
			MenuConfigurationControlsUI.defaultButton.Text = MenuPlayConfigUI.localization.format("Default");
			MenuConfigurationControlsUI.defaultButton.TooltipText = MenuPlayConfigUI.localization.format("Default_Tooltip");
			MenuConfigurationControlsUI.defaultButton.OnClicked += new ClickedButton(MenuConfigurationControlsUI.onClickedDefaultButton);
			MenuConfigurationControlsUI.defaultButton.FontSize = 3;
			MenuConfigurationControlsUI.container.AddChild(MenuConfigurationControlsUI.defaultButton);
			MenuConfigurationControlsUI.updateAll();
		}

		// Token: 0x04002899 RID: 10393
		private static byte[][] layouts = new byte[][]
		{
			new byte[]
			{
				ControlsSettings.UP,
				ControlsSettings.DOWN,
				ControlsSettings.LEFT,
				ControlsSettings.RIGHT,
				ControlsSettings.JUMP,
				ControlsSettings.SPRINT
			},
			new byte[]
			{
				ControlsSettings.CROUCH,
				ControlsSettings.PRONE,
				ControlsSettings.STANCE,
				ControlsSettings.LEAN_LEFT,
				ControlsSettings.LEAN_RIGHT,
				ControlsSettings.PERSPECTIVE,
				ControlsSettings.GESTURE
			},
			new byte[]
			{
				ControlsSettings.INTERACT,
				ControlsSettings.PRIMARY,
				ControlsSettings.SECONDARY
			},
			new byte[]
			{
				ControlsSettings.RELOAD,
				ControlsSettings.ATTACH,
				ControlsSettings.FIREMODE,
				ControlsSettings.TACTICAL,
				ControlsSettings.VISION,
				ControlsSettings.INSPECT,
				ControlsSettings.ROTATE,
				ControlsSettings.DEQUIP
			},
			new byte[]
			{
				ControlsSettings.VOICE,
				ControlsSettings.GLOBAL,
				ControlsSettings.LOCAL,
				ControlsSettings.GROUP
			},
			new byte[]
			{
				ControlsSettings.HUD,
				ControlsSettings.OTHER,
				ControlsSettings.DASHBOARD,
				ControlsSettings.INVENTORY,
				ControlsSettings.CRAFTING,
				ControlsSettings.SKILLS,
				ControlsSettings.MAP,
				ControlsSettings.QUESTS,
				ControlsSettings.PLAYERS
			},
			new byte[]
			{
				ControlsSettings.LOCKER,
				ControlsSettings.ROLL_LEFT,
				ControlsSettings.ROLL_RIGHT,
				ControlsSettings.PITCH_UP,
				ControlsSettings.PITCH_DOWN,
				ControlsSettings.YAW_LEFT,
				ControlsSettings.YAW_RIGHT,
				ControlsSettings.THRUST_INCREASE,
				ControlsSettings.THRUST_DECREASE
			},
			new byte[]
			{
				ControlsSettings.MODIFY,
				ControlsSettings.SNAP,
				ControlsSettings.FOCUS,
				ControlsSettings.ASCEND,
				ControlsSettings.DESCEND,
				ControlsSettings.TOOL_0,
				ControlsSettings.TOOL_1,
				ControlsSettings.TOOL_2,
				ControlsSettings.TOOL_3,
				ControlsSettings.TERMINAL,
				ControlsSettings.SCREENSHOT,
				ControlsSettings.REFRESH_ASSETS,
				ControlsSettings.CLIPBOARD_DEBUG
			},
			new byte[]
			{
				ControlsSettings.PLUGIN_0,
				ControlsSettings.PLUGIN_1,
				ControlsSettings.PLUGIN_2,
				ControlsSettings.PLUGIN_3,
				ControlsSettings.PLUGIN_4,
				74
			},
			new byte[]
			{
				64,
				65,
				66,
				67,
				68,
				69,
				70,
				71,
				72,
				73
			}
		};

		// Token: 0x0400289A RID: 10394
		private static Local localization;

		// Token: 0x0400289B RID: 10395
		private static Local localizationKeyCodes;

		// Token: 0x0400289C RID: 10396
		private static SleekFullscreenBox container;

		// Token: 0x0400289D RID: 10397
		public static bool active;

		// Token: 0x0400289E RID: 10398
		private static SleekButtonIcon backButton;

		// Token: 0x0400289F RID: 10399
		private static ISleekButton defaultButton;

		// Token: 0x040028A0 RID: 10400
		private static ISleekFloat32Field sensitivityField;

		// Token: 0x040028A1 RID: 10401
		private static SleekButtonState sensitivityScalingModeButton;

		// Token: 0x040028A2 RID: 10402
		private static ISleekFloat32Field projectionRatioCoefficientField;

		// Token: 0x040028A3 RID: 10403
		private static ISleekToggle invertToggle;

		// Token: 0x040028A4 RID: 10404
		private static ISleekToggle invertFlightToggle;

		// Token: 0x040028A5 RID: 10405
		private static ISleekScrollView controlsBox;

		// Token: 0x040028A6 RID: 10406
		private static ISleekButton[] buttons;

		// Token: 0x040028A7 RID: 10407
		private static SleekButtonState aimingButton;

		// Token: 0x040028A8 RID: 10408
		private static SleekButtonState crouchingButton;

		// Token: 0x040028A9 RID: 10409
		private static SleekButtonState proningButton;

		// Token: 0x040028AA RID: 10410
		private static SleekButtonState sprintingButton;

		// Token: 0x040028AB RID: 10411
		private static SleekButtonState leaningButton;

		// Token: 0x040028AC RID: 10412
		private static SleekButtonState voiceModeButton;

		// Token: 0x040028AD RID: 10413
		public static byte binding = byte.MaxValue;
	}
}
