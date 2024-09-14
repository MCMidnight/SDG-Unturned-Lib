using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SDG.Unturned
{
	// Token: 0x020006D6 RID: 1750
	public class OptionsSettings
	{
		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06003AB3 RID: 15027 RVA: 0x00112016 File Offset: 0x00110216
		// (set) Token: 0x06003AB4 RID: 15028 RVA: 0x0011201D File Offset: 0x0011021D
		public static float fov
		{
			get
			{
				return OptionsSettings._fov;
			}
			set
			{
				OptionsSettings._fov = value;
				OptionsSettings.CacheVerticalFOV();
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06003AB5 RID: 15029 RVA: 0x0011202A File Offset: 0x0011022A
		public static float DesiredVerticalFieldOfView
		{
			get
			{
				return OptionsSettings._cachedVerticalFOV;
			}
		}

		/// <summary>
		/// Prior to 3.22.8.0 all scopes/optics had a base fov of 90 degrees.
		/// </summary>
		// Token: 0x06003AB6 RID: 15030 RVA: 0x00112031 File Offset: 0x00110231
		public static float GetZoomBaseFieldOfView()
		{
			if (ControlsSettings.sensitivityScalingMode != ESensitivityScalingMode.Legacy)
			{
				return OptionsSettings.DesiredVerticalFieldOfView;
			}
			return 90f;
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x00112048 File Offset: 0x00110248
		private static void CacheVerticalFOV()
		{
			if (Provider.preferenceData != null && Provider.preferenceData.Graphics != null && Provider.preferenceData.Graphics.Override_Vertical_Field_Of_View > 0.5f)
			{
				OptionsSettings._cachedVerticalFOV = Mathf.Clamp(Provider.preferenceData.Graphics.Override_Vertical_Field_Of_View, 1f, 100f);
				return;
			}
			OptionsSettings._cachedVerticalFOV = (float)OptionsSettings.MIN_FOV + (float)OptionsSettings.MAX_FOV * OptionsSettings._fov;
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06003AB8 RID: 15032 RVA: 0x001120BA File Offset: 0x001102BA
		// (set) Token: 0x06003AB9 RID: 15033 RVA: 0x001120C1 File Offset: 0x001102C1
		public static float UnfocusedVolume
		{
			get
			{
				return UnturnedMasterVolume.UnfocusedVolume;
			}
			set
			{
				UnturnedMasterVolume.UnfocusedVolume = value;
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06003ABA RID: 15034 RVA: 0x001120C9 File Offset: 0x001102C9
		// (set) Token: 0x06003ABB RID: 15035 RVA: 0x001120D0 File Offset: 0x001102D0
		public static float MusicMasterVolume
		{
			get
			{
				return OptionsSettings._musicMasterVolume;
			}
			set
			{
				OptionsSettings._musicMasterVolume = value;
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06003ABC RID: 15036 RVA: 0x001120D8 File Offset: 0x001102D8
		// (set) Token: 0x06003ABD RID: 15037 RVA: 0x001120DF File Offset: 0x001102DF
		public static float gameVolume
		{
			get
			{
				return OptionsSettings._gameVolume;
			}
			set
			{
				OptionsSettings._gameVolume = value;
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06003ABE RID: 15038 RVA: 0x001120E7 File Offset: 0x001102E7
		// (set) Token: 0x06003ABF RID: 15039 RVA: 0x001120EE File Offset: 0x001102EE
		public static float voiceVolume
		{
			get
			{
				return OptionsSettings._voiceVolume;
			}
			set
			{
				OptionsSettings._voiceVolume = value;
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06003AC0 RID: 15040 RVA: 0x001120F6 File Offset: 0x001102F6
		// (set) Token: 0x06003AC1 RID: 15041 RVA: 0x001120FD File Offset: 0x001102FD
		public static float MainMenuMusicVolume
		{
			get
			{
				return OptionsSettings._mainMenuMusicVolume;
			}
			set
			{
				OptionsSettings._mainMenuMusicVolume = value;
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06003AC2 RID: 15042 RVA: 0x00112105 File Offset: 0x00110305
		// (set) Token: 0x06003AC3 RID: 15043 RVA: 0x0011210C File Offset: 0x0011030C
		public static float AtmosphereVolume
		{
			get
			{
				return OptionsSettings._atmosphereVolume;
			}
			set
			{
				OptionsSettings._atmosphereVolume = value;
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06003AC4 RID: 15044 RVA: 0x00112114 File Offset: 0x00110314
		// (set) Token: 0x06003AC5 RID: 15045 RVA: 0x0011211B File Offset: 0x0011031B
		public static bool metric
		{
			get
			{
				return OptionsSettings._metric;
			}
			set
			{
				if (OptionsSettings._metric != value)
				{
					OptionsSettings._metric = value;
					Action onUnitSystemChanged = OptionsSettings.OnUnitSystemChanged;
					if (onUnitSystemChanged == null)
					{
						return;
					}
					onUnitSystemChanged.Invoke();
				}
			}
		}

		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06003AC6 RID: 15046 RVA: 0x0011213C File Offset: 0x0011033C
		// (remove) Token: 0x06003AC7 RID: 15047 RVA: 0x00112170 File Offset: 0x00110370
		public static event Action OnUnitSystemChanged;

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x001121A3 File Offset: 0x001103A3
		// (set) Token: 0x06003AC9 RID: 15049 RVA: 0x001121AA File Offset: 0x001103AA
		public static bool proUI
		{
			get
			{
				return SleekCustomization.darkTheme;
			}
			set
			{
				SleekCustomization.darkTheme = value;
				Action onThemeChanged = OptionsSettings.OnThemeChanged;
				if (onThemeChanged == null)
				{
					return;
				}
				onThemeChanged.Invoke();
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06003ACA RID: 15050 RVA: 0x001121C1 File Offset: 0x001103C1
		// (set) Token: 0x06003ACB RID: 15051 RVA: 0x001121C8 File Offset: 0x001103C8
		public static bool ShouldHitmarkersFollowWorldPosition
		{
			get
			{
				return OptionsSettings.hitmarker;
			}
			set
			{
				OptionsSettings.hitmarker = value;
			}
		}

		/// <summary>
		/// If false, call Start and Stop recording before and after push-to-talk key is pressed. This was the
		/// original default behavior, but causes a hitch for some players. As a workaround we can always keep
		/// the microphone rolling and only send data when the push-to-talk key is held. (public issue #4248)
		/// </summary>
		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06003ACC RID: 15052 RVA: 0x001121D0 File Offset: 0x001103D0
		// (set) Token: 0x06003ACD RID: 15053 RVA: 0x001121D7 File Offset: 0x001103D7
		public static bool VoiceAlwaysRecording
		{
			get
			{
				return OptionsSettings._voiceAlwaysRecording;
			}
			set
			{
				if (OptionsSettings._voiceAlwaysRecording != value)
				{
					OptionsSettings._voiceAlwaysRecording = value;
					Action onVoiceAlwaysRecordingChanged = OptionsSettings.OnVoiceAlwaysRecordingChanged;
					if (onVoiceAlwaysRecordingChanged == null)
					{
						return;
					}
					onVoiceAlwaysRecordingChanged.Invoke();
				}
			}
		}

		// Token: 0x140000DA RID: 218
		// (add) Token: 0x06003ACE RID: 15054 RVA: 0x001121F8 File Offset: 0x001103F8
		// (remove) Token: 0x06003ACF RID: 15055 RVA: 0x0011222C File Offset: 0x0011042C
		public static event Action OnVoiceAlwaysRecordingChanged;

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06003AD0 RID: 15056 RVA: 0x0011225F File Offset: 0x0011045F
		// (set) Token: 0x06003AD1 RID: 15057 RVA: 0x00112266 File Offset: 0x00110466
		public static Color cursorColor
		{
			get
			{
				return SleekCustomization.cursorColor;
			}
			set
			{
				SleekCustomization.cursorColor = value;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06003AD2 RID: 15058 RVA: 0x0011226E File Offset: 0x0011046E
		// (set) Token: 0x06003AD3 RID: 15059 RVA: 0x00112275 File Offset: 0x00110475
		public static Color backgroundColor
		{
			get
			{
				return SleekCustomization.backgroundColor;
			}
			set
			{
				SleekCustomization.backgroundColor = value;
				Action onCustomColorsChanged = OptionsSettings.OnCustomColorsChanged;
				if (onCustomColorsChanged == null)
				{
					return;
				}
				onCustomColorsChanged.Invoke();
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06003AD4 RID: 15060 RVA: 0x0011228C File Offset: 0x0011048C
		// (set) Token: 0x06003AD5 RID: 15061 RVA: 0x00112293 File Offset: 0x00110493
		public static Color foregroundColor
		{
			get
			{
				return SleekCustomization.foregroundColor;
			}
			set
			{
				SleekCustomization.foregroundColor = value;
				Action onCustomColorsChanged = OptionsSettings.OnCustomColorsChanged;
				if (onCustomColorsChanged == null)
				{
					return;
				}
				onCustomColorsChanged.Invoke();
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06003AD6 RID: 15062 RVA: 0x001122AA File Offset: 0x001104AA
		// (set) Token: 0x06003AD7 RID: 15063 RVA: 0x001122B1 File Offset: 0x001104B1
		public static Color fontColor
		{
			get
			{
				return SleekCustomization.fontColor;
			}
			set
			{
				SleekCustomization.fontColor = value;
				Action onCustomColorsChanged = OptionsSettings.OnCustomColorsChanged;
				if (onCustomColorsChanged == null)
				{
					return;
				}
				onCustomColorsChanged.Invoke();
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x001122C8 File Offset: 0x001104C8
		// (set) Token: 0x06003AD9 RID: 15065 RVA: 0x001122CF File Offset: 0x001104CF
		public static Color shadowColor
		{
			get
			{
				return SleekCustomization.shadowColor;
			}
			set
			{
				SleekCustomization.shadowColor = value;
				Action onCustomColorsChanged = OptionsSettings.OnCustomColorsChanged;
				if (onCustomColorsChanged == null)
				{
					return;
				}
				onCustomColorsChanged.Invoke();
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06003ADA RID: 15066 RVA: 0x001122E6 File Offset: 0x001104E6
		// (set) Token: 0x06003ADB RID: 15067 RVA: 0x001122ED File Offset: 0x001104ED
		public static Color badColor
		{
			get
			{
				return SleekCustomization.badColor;
			}
			set
			{
				SleekCustomization.badColor = value;
				Action onCustomColorsChanged = OptionsSettings.OnCustomColorsChanged;
				if (onCustomColorsChanged == null)
				{
					return;
				}
				onCustomColorsChanged.Invoke();
			}
		}

		/// <summary>
		/// Invoked when custom UI colors are set.
		/// </summary>
		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06003ADC RID: 15068 RVA: 0x00112304 File Offset: 0x00110504
		// (remove) Token: 0x06003ADD RID: 15069 RVA: 0x00112338 File Offset: 0x00110538
		public static event Action OnCustomColorsChanged;

		/// <summary>
		/// Invoked when dark/light theme is set.
		/// </summary>
		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06003ADE RID: 15070 RVA: 0x0011236C File Offset: 0x0011056C
		// (remove) Token: 0x06003ADF RID: 15071 RVA: 0x001123A0 File Offset: 0x001105A0
		public static event Action OnThemeChanged;

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x001123D3 File Offset: 0x001105D3
		internal static bool ShouldShowOnlineSafetyMenu
		{
			get
			{
				return (!OptionsSettings.wantsToHideOnlineSafetyMenu || OptionsSettings.onlineSafetyMenuProceedCount < 1) && !OptionsSettings.didProceedThroughOnlineSafetyMenuThisSession;
			}
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x001123F0 File Offset: 0x001105F0
		public static void apply()
		{
			if (!Level.isLoaded && MainCamera.instance != null && !Level.isVR && !Dedicator.isVR)
			{
				MainCamera.instance.fieldOfView = OptionsSettings.DesiredVerticalFieldOfView;
			}
			if (SceneManager.GetActiveScene().buildIndex <= Level.BUILD_INDEX_MENU)
			{
				MenuConfigurationOptions.apply();
			}
			UnturnedMasterVolume.preferredVolume = OptionsSettings.volume;
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x00112454 File Offset: 0x00110654
		public static void RestoreAudioDefaults()
		{
			OptionsSettings.volume = 1f;
			OptionsSettings.UnfocusedVolume = 0.5f;
			OptionsSettings.gameVolume = 0.7f;
			OptionsSettings.MusicMasterVolume = 0.7f;
			OptionsSettings.loadingScreenMusicVolume = 0.5f;
			OptionsSettings.deathMusicVolume = 0.7f;
			OptionsSettings.MainMenuMusicVolume = 0.7f;
			OptionsSettings.ambientMusicVolume = 0.7f;
			OptionsSettings.voiceVolume = 0.7f;
			OptionsSettings.AtmosphereVolume = 0.7f;
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x001124C8 File Offset: 0x001106C8
		public static void restoreDefaults()
		{
			OptionsSettings.splashscreen = true;
			OptionsSettings.timer = false;
			OptionsSettings.fov = 0.75f;
			OptionsSettings.debug = false;
			OptionsSettings.gore = true;
			OptionsSettings.filter = true;
			OptionsSettings.chatText = true;
			OptionsSettings.chatVoiceIn = false;
			OptionsSettings.chatVoiceOut = false;
			OptionsSettings.metric = true;
			OptionsSettings.talk = false;
			OptionsSettings.hints = true;
			OptionsSettings.proUI = true;
			OptionsSettings.ShouldHitmarkersFollowWorldPosition = false;
			OptionsSettings.streamer = false;
			OptionsSettings.featuredWorkshop = true;
			OptionsSettings.showHotbar = true;
			OptionsSettings.pauseWhenUnfocused = true;
			OptionsSettings.screenshotSizeMultiplier = 1;
			OptionsSettings.enableScreenshotSupersampling = true;
			OptionsSettings.enableScreenshotsOnLoadingScreen = true;
			OptionsSettings.useStaticCrosshair = false;
			OptionsSettings.staticCrosshairSize = 0.1f;
			OptionsSettings.crosshairShape = ECrosshairShape.Line;
			OptionsSettings.hitmarkerStyle = EHitmarkerStyle.Animated;
			OptionsSettings.vehicleThirdPersonCameraMode = EVehicleThirdPersonCameraMode.RotationDetached;
			OptionsSettings.vehicleAircraftThirdPersonCameraMode = EVehicleThirdPersonCameraMode.RotationAttached;
			OptionsSettings.crosshairColor = new Color(1f, 1f, 1f, 0.5f);
			OptionsSettings.hitmarkerColor = new Color(1f, 1f, 1f, 0.5f);
			OptionsSettings.criticalHitmarkerColor = new Color(1f, 0f, 0f, 0.5f);
			OptionsSettings.cursorColor = Color.white;
			OptionsSettings.backgroundColor = new Color(0.9f, 0.9f, 0.9f);
			OptionsSettings.foregroundColor = new Color(0.9f, 0.9f, 0.9f);
			OptionsSettings.fontColor = new Color(0.9f, 0.9f, 0.9f);
			OptionsSettings.shadowColor = Color.black;
			OptionsSettings.badColor = Palette.COLOR_R;
			OptionsSettings._voiceAlwaysRecording = false;
			OptionsSettings.shouldNametagFadeOut = true;
			OptionsSettings.wantsToHideOnlineSafetyMenu = false;
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x00112654 File Offset: 0x00110854
		public static void load()
		{
			OptionsSettings.restoreDefaults();
			OptionsSettings.RestoreAudioDefaults();
			if (ReadWrite.fileExists("/Options.dat", true))
			{
				Block block = ReadWrite.readBlock("/Options.dat", true, 0);
				if (block != null)
				{
					byte b = block.readByte();
					if (b > 2)
					{
						bool flag = block.readBoolean();
						if (b < 31)
						{
							OptionsSettings.splashscreen = true;
						}
						else
						{
							OptionsSettings.splashscreen = block.readBoolean();
						}
						if (b < 20)
						{
							OptionsSettings.timer = false;
						}
						else
						{
							OptionsSettings.timer = block.readBoolean();
						}
						if (b < 10)
						{
							block.readBoolean();
						}
						if (b > 7)
						{
							OptionsSettings.fov = block.readSingle();
						}
						else
						{
							OptionsSettings.fov = block.readSingle() * 0.5f;
						}
						if (b < 24)
						{
							OptionsSettings.fov *= 1.5f;
							OptionsSettings.fov = Mathf.Clamp01(OptionsSettings.fov);
						}
						if (b > 4)
						{
							OptionsSettings.volume = block.readSingle();
						}
						else
						{
							OptionsSettings.volume = 1f;
						}
						if (b > 22)
						{
							OptionsSettings.voiceVolume = block.readSingle();
							if (b < 36)
							{
								OptionsSettings.voiceVolume = Mathf.Min(OptionsSettings.voiceVolume, 1f);
							}
						}
						else
						{
							OptionsSettings.voiceVolume = 0.7f;
						}
						if (b >= 37)
						{
							OptionsSettings.loadingScreenMusicVolume = block.readSingle();
						}
						else
						{
							OptionsSettings.loadingScreenMusicVolume = 0.5f;
						}
						OptionsSettings.debug = block.readBoolean();
						OptionsSettings.gore = block.readBoolean();
						OptionsSettings.filter = block.readBoolean();
						if (b < 57)
						{
							OptionsSettings.filter = true;
						}
						OptionsSettings.chatText = block.readBoolean();
						if (b > 8)
						{
							OptionsSettings.chatVoiceIn = block.readBoolean();
						}
						else
						{
							OptionsSettings.chatVoiceIn = false;
						}
						if (b < 57)
						{
							OptionsSettings.chatVoiceIn = false;
						}
						OptionsSettings.chatVoiceOut = block.readBoolean();
						if (b < 57)
						{
							OptionsSettings.chatVoiceOut = false;
						}
						OptionsSettings.metric = block.readBoolean();
						if (b > 24)
						{
							OptionsSettings.talk = block.readBoolean();
						}
						else
						{
							OptionsSettings.talk = false;
						}
						if (b > 3)
						{
							OptionsSettings.hints = block.readBoolean();
						}
						else
						{
							OptionsSettings.hints = true;
						}
						bool flag2 = b <= 13 || block.readBoolean();
						if (b > 12)
						{
							OptionsSettings.proUI = block.readBoolean();
						}
						else
						{
							OptionsSettings.proUI = true;
						}
						if (b > 20)
						{
							OptionsSettings.ShouldHitmarkersFollowWorldPosition = block.readBoolean();
						}
						else
						{
							OptionsSettings.ShouldHitmarkersFollowWorldPosition = false;
						}
						if (b > 21)
						{
							OptionsSettings.streamer = block.readBoolean();
						}
						else
						{
							OptionsSettings.streamer = false;
						}
						if (b > 25)
						{
							OptionsSettings.featuredWorkshop = block.readBoolean();
						}
						else
						{
							OptionsSettings.featuredWorkshop = true;
						}
						if (b > 28 && b < 46)
						{
							block.readBoolean();
						}
						if (b > 29)
						{
							OptionsSettings.showHotbar = block.readBoolean();
						}
						else
						{
							OptionsSettings.showHotbar = true;
						}
						if (b > 32)
						{
							OptionsSettings.pauseWhenUnfocused = block.readBoolean();
						}
						else
						{
							OptionsSettings.pauseWhenUnfocused = true;
						}
						if (b > 27 && b < 46)
						{
							block.readInt32();
						}
						if (b > 26 && b < 46)
						{
							block.readInt32();
						}
						if (b > 6)
						{
							OptionsSettings.crosshairColor = block.readColor();
							OptionsSettings.hitmarkerColor = block.readColor();
							OptionsSettings.criticalHitmarkerColor = block.readColor();
							OptionsSettings.cursorColor = block.readColor();
						}
						else
						{
							OptionsSettings.crosshairColor = Color.white;
							OptionsSettings.hitmarkerColor = Color.white;
							OptionsSettings.criticalHitmarkerColor = Color.red;
							OptionsSettings.cursorColor = Color.white;
						}
						if (b > 18)
						{
							OptionsSettings.backgroundColor = block.readColor();
							OptionsSettings.foregroundColor = block.readColor();
							OptionsSettings.fontColor = block.readColor();
						}
						else
						{
							OptionsSettings.backgroundColor = new Color(0.9f, 0.9f, 0.9f);
							OptionsSettings.foregroundColor = new Color(0.9f, 0.9f, 0.9f);
							OptionsSettings.fontColor = new Color(0.9f, 0.9f, 0.9f);
						}
						if (b > 33)
						{
							OptionsSettings.shadowColor = block.readColor();
						}
						else
						{
							OptionsSettings.shadowColor = Color.black;
						}
						if (b > 34)
						{
							OptionsSettings.badColor = block.readColor();
						}
						else
						{
							OptionsSettings.badColor = Palette.COLOR_R;
						}
						if (b < 38)
						{
							OptionsSettings.screenshotSizeMultiplier = 1;
						}
						else
						{
							OptionsSettings.screenshotSizeMultiplier = block.readInt32();
						}
						if (b < 39)
						{
							OptionsSettings.enableScreenshotSupersampling = true;
						}
						else
						{
							OptionsSettings.enableScreenshotSupersampling = block.readBoolean();
						}
						if (b < 40)
						{
							OptionsSettings.enableScreenshotsOnLoadingScreen = true;
						}
						else
						{
							OptionsSettings.enableScreenshotsOnLoadingScreen = block.readBoolean();
						}
						if (b < 41)
						{
							OptionsSettings.useStaticCrosshair = false;
							OptionsSettings.staticCrosshairSize = 0.25f;
						}
						else
						{
							OptionsSettings.useStaticCrosshair = block.readBoolean();
							OptionsSettings.staticCrosshairSize = block.readSingle();
						}
						if (b < 42)
						{
							OptionsSettings.crosshairShape = ECrosshairShape.Line;
						}
						else
						{
							OptionsSettings.crosshairShape = (ECrosshairShape)block.readByte();
						}
						if (b < 43)
						{
							OptionsSettings.crosshairColor.a = 0.5f;
							OptionsSettings.hitmarkerColor.a = 0.5f;
							OptionsSettings.criticalHitmarkerColor.a = 0.5f;
						}
						else
						{
							OptionsSettings.crosshairColor.a = (float)block.readByte() / 255f;
							OptionsSettings.hitmarkerColor.a = (float)block.readByte() / 255f;
							OptionsSettings.criticalHitmarkerColor.a = (float)block.readByte() / 255f;
						}
						if (b < 45)
						{
							OptionsSettings.hitmarkerStyle = EHitmarkerStyle.Animated;
						}
						else
						{
							OptionsSettings.hitmarkerStyle = (EHitmarkerStyle)block.readByte();
						}
						if (b >= 47)
						{
							OptionsSettings._voiceAlwaysRecording = block.readBoolean();
							if (b < 48)
							{
								OptionsSettings._voiceAlwaysRecording = false;
							}
						}
						else
						{
							OptionsSettings._voiceAlwaysRecording = false;
						}
						if (b >= 49)
						{
							OptionsSettings.shouldNametagFadeOut = block.readBoolean();
						}
						else
						{
							OptionsSettings.shouldNametagFadeOut = true;
						}
						if (b >= 50)
						{
							OptionsSettings.gameVolume = block.readSingle();
						}
						else
						{
							OptionsSettings.gameVolume = 0.7f;
						}
						if (b >= 51)
						{
							OptionsSettings.UnfocusedVolume = block.readSingle();
						}
						else
						{
							OptionsSettings.UnfocusedVolume = 0.5f;
						}
						if (b >= 52)
						{
							OptionsSettings.vehicleThirdPersonCameraMode = (EVehicleThirdPersonCameraMode)block.readByte();
						}
						else
						{
							OptionsSettings.vehicleThirdPersonCameraMode = EVehicleThirdPersonCameraMode.RotationDetached;
						}
						if (b >= 53)
						{
							OptionsSettings.deathMusicVolume = block.readSingle();
							OptionsSettings.MainMenuMusicVolume = block.readSingle();
							OptionsSettings.ambientMusicVolume = block.readSingle();
						}
						else if (flag)
						{
							OptionsSettings.deathMusicVolume = 0.7f;
							OptionsSettings.MainMenuMusicVolume = 0.7f;
							OptionsSettings.ambientMusicVolume = 0.7f;
						}
						else
						{
							OptionsSettings.deathMusicVolume = 0f;
							OptionsSettings.MainMenuMusicVolume = 0f;
							OptionsSettings.ambientMusicVolume = 0f;
						}
						if (b >= 54)
						{
							OptionsSettings.MusicMasterVolume = block.readSingle();
						}
						else
						{
							OptionsSettings.MusicMasterVolume = 0.7f;
						}
						if (b >= 55)
						{
							OptionsSettings.AtmosphereVolume = block.readSingle();
						}
						else
						{
							OptionsSettings.AtmosphereVolume = (flag2 ? 0.7f : 0f);
						}
						if (b >= 56)
						{
							OptionsSettings.vehicleAircraftThirdPersonCameraMode = (EVehicleThirdPersonCameraMode)block.readByte();
						}
						else
						{
							OptionsSettings.vehicleAircraftThirdPersonCameraMode = EVehicleThirdPersonCameraMode.RotationAttached;
						}
						if (b >= 57)
						{
							OptionsSettings.onlineSafetyMenuProceedCount = block.readInt32();
							OptionsSettings.wantsToHideOnlineSafetyMenu = block.readBoolean();
						}
						if (!Provider.isPro)
						{
							OptionsSettings.backgroundColor = new Color(0.9f, 0.9f, 0.9f);
							OptionsSettings.foregroundColor = new Color(0.9f, 0.9f, 0.9f);
							OptionsSettings.fontColor = new Color(0.9f, 0.9f, 0.9f);
							OptionsSettings.shadowColor = Color.black;
							OptionsSettings.badColor = Palette.COLOR_R;
						}
						return;
					}
				}
			}
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x00112D00 File Offset: 0x00110F00
		public static void save()
		{
			Block block = new Block();
			block.writeByte(57);
			block.writeBoolean(false);
			block.writeBoolean(OptionsSettings.splashscreen);
			block.writeBoolean(OptionsSettings.timer);
			block.writeSingle(OptionsSettings.fov);
			block.writeSingle(OptionsSettings.volume);
			block.writeSingle(OptionsSettings.voiceVolume);
			block.writeSingle(OptionsSettings.loadingScreenMusicVolume);
			block.writeBoolean(OptionsSettings.debug);
			block.writeBoolean(OptionsSettings.gore);
			block.writeBoolean(OptionsSettings.filter);
			block.writeBoolean(OptionsSettings.chatText);
			block.writeBoolean(OptionsSettings.chatVoiceIn);
			block.writeBoolean(OptionsSettings.chatVoiceOut);
			block.writeBoolean(OptionsSettings.metric);
			block.writeBoolean(OptionsSettings.talk);
			block.writeBoolean(OptionsSettings.hints);
			block.writeBoolean(false);
			block.writeBoolean(OptionsSettings.proUI);
			block.writeBoolean(OptionsSettings.ShouldHitmarkersFollowWorldPosition);
			block.writeBoolean(OptionsSettings.streamer);
			block.writeBoolean(OptionsSettings.featuredWorkshop);
			block.writeBoolean(OptionsSettings.showHotbar);
			block.writeBoolean(OptionsSettings.pauseWhenUnfocused);
			block.writeColor(OptionsSettings.crosshairColor);
			block.writeColor(OptionsSettings.hitmarkerColor);
			block.writeColor(OptionsSettings.criticalHitmarkerColor);
			block.writeColor(OptionsSettings.cursorColor);
			block.writeColor(OptionsSettings.backgroundColor);
			block.writeColor(OptionsSettings.foregroundColor);
			block.writeColor(OptionsSettings.fontColor);
			block.writeColor(OptionsSettings.shadowColor);
			block.writeColor(OptionsSettings.badColor);
			block.writeInt32(OptionsSettings.screenshotSizeMultiplier);
			block.writeBoolean(OptionsSettings.enableScreenshotSupersampling);
			block.writeBoolean(OptionsSettings.enableScreenshotsOnLoadingScreen);
			block.writeBoolean(OptionsSettings.useStaticCrosshair);
			block.writeSingle(OptionsSettings.staticCrosshairSize);
			block.writeByte((byte)OptionsSettings.crosshairShape);
			block.writeByte(MathfEx.RoundAndClampToByte(OptionsSettings.crosshairColor.a * 255f));
			block.writeByte(MathfEx.RoundAndClampToByte(OptionsSettings.hitmarkerColor.a * 255f));
			block.writeByte(MathfEx.RoundAndClampToByte(OptionsSettings.criticalHitmarkerColor.a * 255f));
			block.writeByte((byte)OptionsSettings.hitmarkerStyle);
			block.writeBoolean(OptionsSettings._voiceAlwaysRecording);
			block.writeBoolean(OptionsSettings.shouldNametagFadeOut);
			block.writeSingle(OptionsSettings.gameVolume);
			block.writeSingle(OptionsSettings.UnfocusedVolume);
			block.writeByte((byte)OptionsSettings.vehicleThirdPersonCameraMode);
			block.writeSingle(OptionsSettings.deathMusicVolume);
			block.writeSingle(OptionsSettings.MainMenuMusicVolume);
			block.writeSingle(OptionsSettings.ambientMusicVolume);
			block.writeSingle(OptionsSettings.MusicMasterVolume);
			block.writeSingle(OptionsSettings.AtmosphereVolume);
			block.writeByte((byte)OptionsSettings.vehicleAircraftThirdPersonCameraMode);
			block.writeInt32(OptionsSettings.onlineSafetyMenuProceedCount);
			block.writeBoolean(OptionsSettings.wantsToHideOnlineSafetyMenu);
			ReadWrite.writeBlock("/Options.dat", true, block);
		}

		// Token: 0x04002353 RID: 9043
		private const byte SAVEDATA_VERSION_ADDED_LOADING_SCREEN_MUSIC = 37;

		// Token: 0x04002354 RID: 9044
		private const byte SAVEDATA_VERSION_ADDED_SCREENSHOT_SIZE = 38;

		// Token: 0x04002355 RID: 9045
		private const byte SAVEDATA_VERSION_ADDED_SCREENSHOT_SUPERSAMPLING = 39;

		// Token: 0x04002356 RID: 9046
		private const byte SAVEDATA_VERSION_ADDED_LOADING_SCREEN_SCREENSHOTS = 40;

		// Token: 0x04002357 RID: 9047
		private const byte SAVEDATA_VERSION_ADDED_STATIC_CROSSHAIR = 41;

		// Token: 0x04002358 RID: 9048
		private const byte SAVEDATA_VERSION_ADDED_CROSSHAIR_SHAPE = 42;

		// Token: 0x04002359 RID: 9049
		private const byte SAVEDATA_VERSION_ADDED_CROSSHAIR_AND_HITMARKER_ALPHA = 43;

		// Token: 0x0400235A RID: 9050
		private const byte SAVEDATA_VERSION_ADDED_HITMARKER_STYLE = 44;

		/// <summary>
		/// Unfortunately the version which added hitmarker style saved but didn't actually load (sigh).
		/// </summary>
		// Token: 0x0400235B RID: 9051
		private const byte SAVEDATA_VERSION_ADDED_HITMARKER_STYLE_FIX = 45;

		// Token: 0x0400235C RID: 9052
		private const byte SAVEDATA_VERSION_REMOVED_MATCHMAKING = 46;

		// Token: 0x0400235D RID: 9053
		private const byte SAVEDATA_VERSION_ADDED_VOICE_ALWAYS_RECORDING = 47;

		/// <summary>
		/// Nelson 2023-12-28: this option was causing players to crash in the 3.23.14.0 update. Hopefully
		/// it's resolved for the patch, but to be safe it will default to false.
		/// </summary>
		// Token: 0x0400235E RID: 9054
		private const byte SAVEDATA_VERSION_RESET_VOICE_ALWAYS_RECORDING = 48;

		// Token: 0x0400235F RID: 9055
		private const byte SAVEDATA_VERSION_ADDED_NAMETAG_FADEOUT_OPT = 49;

		// Token: 0x04002360 RID: 9056
		private const byte SAVEDATA_VERSION_ADDED_GAME_VOLUME = 50;

		// Token: 0x04002361 RID: 9057
		private const byte SAVEDATA_VERSION_ADDED_UNFOCUSED_VOLUME = 51;

		// Token: 0x04002362 RID: 9058
		private const byte SAVEDATA_VERSION_ADDED_VEHICLE_THIRD_PERSON_CAMERA_MODE = 52;

		// Token: 0x04002363 RID: 9059
		private const byte SAVEDATA_VERSION_REPLACTED_MUSIC_TOGGLE_WITH_VOLUMES = 53;

		// Token: 0x04002364 RID: 9060
		private const byte SAVEDATA_VERSION_ADDED_MUSIC_MASTER_VOLUME = 54;

		// Token: 0x04002365 RID: 9061
		private const byte SAVEDATA_VERSION_ADDED_ATMOSPHERE_VOLUME = 55;

		// Token: 0x04002366 RID: 9062
		private const byte SAVEDATA_VERSION_SEPARATED_AIRCRAFT_THIRD_PERSON_CAMERA_MODE = 56;

		// Token: 0x04002367 RID: 9063
		private const byte SAVEDATA_VERSION_ADDED_ONLINE_SAFETY_MENU = 57;

		// Token: 0x04002368 RID: 9064
		private const byte SAVEDATA_VERSION_NEWEST = 57;

		// Token: 0x04002369 RID: 9065
		public static readonly byte SAVEDATA_VERSION = 57;

		// Token: 0x0400236A RID: 9066
		public static readonly byte MIN_FOV = 60;

		// Token: 0x0400236B RID: 9067
		public static readonly byte MAX_FOV = 40;

		// Token: 0x0400236C RID: 9068
		private static float _fov;

		// Token: 0x0400236D RID: 9069
		private static float _cachedVerticalFOV;

		// Token: 0x0400236E RID: 9070
		public const float DEFAULT_MASTER_VOLUME = 1f;

		// Token: 0x0400236F RID: 9071
		public static float volume;

		// Token: 0x04002370 RID: 9072
		public const float DEFAULT_UNFOCUSED_VOLUME = 0.5f;

		// Token: 0x04002371 RID: 9073
		public const float DEFAULT_MUSIC_MASTER_VOLUME = 0.7f;

		// Token: 0x04002372 RID: 9074
		private static float _musicMasterVolume;

		// Token: 0x04002373 RID: 9075
		private const float DEFAULT_GAME_VOLUME = 0.7f;

		// Token: 0x04002374 RID: 9076
		private static float _gameVolume;

		// Token: 0x04002375 RID: 9077
		private const float DEFAULT_VOICE_VOLUME = 0.7f;

		// Token: 0x04002376 RID: 9078
		private static float _voiceVolume;

		// Token: 0x04002377 RID: 9079
		private const float DEFAULT_LOADING_SCREEN_MUSIC_VOLUME = 0.5f;

		// Token: 0x04002378 RID: 9080
		public static float loadingScreenMusicVolume;

		// Token: 0x04002379 RID: 9081
		public const float DEFAULT_DEATH_MUSIC_VOLUME = 0.7f;

		// Token: 0x0400237A RID: 9082
		public static float deathMusicVolume;

		// Token: 0x0400237B RID: 9083
		public const float DEFAULT_MAIN_MENU_MUSIC_VOLUME = 0.7f;

		// Token: 0x0400237C RID: 9084
		private static float _mainMenuMusicVolume;

		// Token: 0x0400237D RID: 9085
		public const float DEFAULT_ATMOSPHERE_VOLUME = 0.7f;

		// Token: 0x0400237E RID: 9086
		private static float _atmosphereVolume;

		// Token: 0x0400237F RID: 9087
		public const float DEFAULT_AMBIENT_MUSIC_VOLUME = 0.7f;

		// Token: 0x04002380 RID: 9088
		public static float ambientMusicVolume;

		// Token: 0x04002381 RID: 9089
		public static bool debug;

		// Token: 0x04002382 RID: 9090
		public static bool splashscreen;

		// Token: 0x04002383 RID: 9091
		public static bool timer;

		// Token: 0x04002384 RID: 9092
		public static bool gore;

		// Token: 0x04002385 RID: 9093
		public static bool filter;

		// Token: 0x04002386 RID: 9094
		public static bool chatText;

		// Token: 0x04002387 RID: 9095
		public static bool chatVoiceIn;

		// Token: 0x04002388 RID: 9096
		public static bool chatVoiceOut;

		// Token: 0x04002389 RID: 9097
		private static bool _metric;

		// Token: 0x0400238B RID: 9099
		public static bool talk;

		// Token: 0x0400238C RID: 9100
		public static bool hints;

		// Token: 0x0400238D RID: 9101
		public static bool ambience;

		// Token: 0x0400238E RID: 9102
		private static bool _voiceAlwaysRecording;

		/// <summary>
		/// If true, group member name labels fade out when near the center of the screen.
		/// Defaults to true.
		/// </summary>
		// Token: 0x04002390 RID: 9104
		public static bool shouldNametagFadeOut;

		// Token: 0x04002391 RID: 9105
		[Obsolete("Renamed to ShouldHitmarkersFollowWorldPosition")]
		public static bool hitmarker;

		// Token: 0x04002392 RID: 9106
		public static bool streamer;

		// Token: 0x04002393 RID: 9107
		public static bool featuredWorkshop;

		// Token: 0x04002394 RID: 9108
		public static bool showHotbar;

		// Token: 0x04002395 RID: 9109
		public static bool pauseWhenUnfocused;

		// Token: 0x04002396 RID: 9110
		public static int screenshotSizeMultiplier;

		// Token: 0x04002397 RID: 9111
		public static bool enableScreenshotSupersampling;

		// Token: 0x04002398 RID: 9112
		public static bool enableScreenshotsOnLoadingScreen;

		// Token: 0x04002399 RID: 9113
		public static bool useStaticCrosshair;

		// Token: 0x0400239A RID: 9114
		public static float staticCrosshairSize;

		// Token: 0x0400239B RID: 9115
		public static ECrosshairShape crosshairShape;

		/// <summary>
		/// Controls whether hitmarkers are animated outward (newer) or just a static image ("classic"). 
		/// </summary>
		// Token: 0x0400239C RID: 9116
		public static EHitmarkerStyle hitmarkerStyle;

		/// <summary>
		/// Determines how camera follows vehicle in third-person view.
		/// </summary>
		// Token: 0x0400239D RID: 9117
		public static EVehicleThirdPersonCameraMode vehicleThirdPersonCameraMode;

		/// <summary>
		/// Determines how camera follows aircraft vehicle in third-person view.
		/// </summary>
		// Token: 0x0400239E RID: 9118
		public static EVehicleThirdPersonCameraMode vehicleAircraftThirdPersonCameraMode;

		// Token: 0x0400239F RID: 9119
		public static Color crosshairColor;

		// Token: 0x040023A0 RID: 9120
		public static Color hitmarkerColor;

		// Token: 0x040023A1 RID: 9121
		public static Color criticalHitmarkerColor;

		/// <summary>
		/// Number of times the player has clicked "Proceed" in the online safety menu.
		/// </summary>
		// Token: 0x040023A4 RID: 9124
		public static int onlineSafetyMenuProceedCount;

		/// <summary>
		/// If true, "don't show again" is checked in the online safety menu.
		/// </summary>
		// Token: 0x040023A5 RID: 9125
		public static bool wantsToHideOnlineSafetyMenu;

		/// <summary>
		/// Prevents menu from being shown twice without a restart.
		/// </summary>
		// Token: 0x040023A6 RID: 9126
		internal static bool didProceedThroughOnlineSafetyMenuThisSession;
	}
}
