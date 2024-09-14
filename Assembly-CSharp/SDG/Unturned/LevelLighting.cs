using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Water;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.ImageEffects;

namespace SDG.Unturned
{
	// Token: 0x020004E7 RID: 1255
	public class LevelLighting
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060026AB RID: 9899 RVA: 0x0009E4C9 File Offset: 0x0009C6C9
		public static bool enableUnderwaterEffects
		{
			get
			{
				return !Level.isEditor || LevelLighting._editorWantsUnderwaterEffects;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060026AC RID: 9900 RVA: 0x0009E4D9 File Offset: 0x0009C6D9
		// (set) Token: 0x060026AD RID: 9901 RVA: 0x0009E4E0 File Offset: 0x0009C6E0
		public static bool EditorWantsUnderwaterEffects
		{
			get
			{
				return LevelLighting._editorWantsUnderwaterEffects;
			}
			set
			{
				LevelLighting._editorWantsUnderwaterEffects = value;
				ConvenientSavedata.get().write("EditorWantsUnderwaterEffects", value);
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060026AE RID: 9902 RVA: 0x0009E4F8 File Offset: 0x0009C6F8
		// (set) Token: 0x060026AF RID: 9903 RVA: 0x0009E4FF File Offset: 0x0009C6FF
		public static bool EditorWantsWaterSurface
		{
			get
			{
				return LevelLighting._editorWantsWaterSurface;
			}
			set
			{
				if (LevelLighting._editorWantsWaterSurface != value)
				{
					LevelLighting._editorWantsWaterSurface = value;
					ConvenientSavedata.get().write("EditorWantsWaterSurfaceVisible", value);
					if (Level.isEditor)
					{
						VolumeManager<WaterVolume, WaterVolumeManager>.Get().ForceUpdateEditorVisibility();
					}
				}
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x0009E530 File Offset: 0x0009C730
		// (set) Token: 0x060026B1 RID: 9905 RVA: 0x0009E537 File Offset: 0x0009C737
		public static float azimuth
		{
			get
			{
				return LevelLighting._azimuth;
			}
			set
			{
				LevelLighting._azimuth = value;
				LevelLighting.updateLighting();
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x0009E544 File Offset: 0x0009C744
		public static float transition
		{
			get
			{
				return LevelLighting._transition;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060026B3 RID: 9907 RVA: 0x0009E54B File Offset: 0x0009C74B
		// (set) Token: 0x060026B4 RID: 9908 RVA: 0x0009E554 File Offset: 0x0009C754
		public static float bias
		{
			get
			{
				return LevelLighting._bias;
			}
			set
			{
				LevelLighting._bias = value;
				if (LevelLighting.bias < 1f - LevelLighting.bias)
				{
					LevelLighting._transition = LevelLighting.bias / 2f * LevelLighting.fade;
				}
				else
				{
					LevelLighting._transition = (1f - LevelLighting.bias) / 2f * LevelLighting.fade;
				}
				LevelLighting.updateLighting();
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060026B5 RID: 9909 RVA: 0x0009E5B2 File Offset: 0x0009C7B2
		// (set) Token: 0x060026B6 RID: 9910 RVA: 0x0009E5BC File Offset: 0x0009C7BC
		public static float fade
		{
			get
			{
				return LevelLighting._fade;
			}
			set
			{
				LevelLighting._fade = value;
				if (LevelLighting.bias < 1f - LevelLighting.bias)
				{
					LevelLighting._transition = LevelLighting.bias / 2f * LevelLighting.fade;
				}
				else
				{
					LevelLighting._transition = (1f - LevelLighting.bias) / 2f * LevelLighting.fade;
				}
				LevelLighting.updateLighting();
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060026B7 RID: 9911 RVA: 0x0009E61A File Offset: 0x0009C81A
		// (set) Token: 0x060026B8 RID: 9912 RVA: 0x0009E624 File Offset: 0x0009C824
		public static float time
		{
			get
			{
				return LevelLighting._time;
			}
			set
			{
				float num = Mathf.Min(Mathf.Abs(value - LevelLighting._time), value + 1f - LevelLighting._time);
				LevelLighting.skyboxNeedsReflectionUpdate = (LevelLighting.skyboxNeedsReflectionUpdate || num > 0.05f);
				LevelLighting._time = value;
				LevelLighting.updateLighting();
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060026BA RID: 9914 RVA: 0x0009E67A File Offset: 0x0009C87A
		// (set) Token: 0x060026B9 RID: 9913 RVA: 0x0009E672 File Offset: 0x0009C872
		public static float wind
		{
			get
			{
				return LevelLighting._wind;
			}
			set
			{
				LevelLighting._wind = value;
			}
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x0009E684 File Offset: 0x0009C884
		private static LevelLighting.CustomWeatherInstance FindWeatherInstanceByAsset(WeatherAssetBase asset)
		{
			foreach (LevelLighting.CustomWeatherInstance customWeatherInstance in LevelLighting.customWeatherInstances)
			{
				if (customWeatherInstance.asset == asset)
				{
					return customWeatherInstance;
				}
			}
			return null;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x0009E6E0 File Offset: 0x0009C8E0
		[Obsolete("Renamed to GetActiveWeatherAsset")]
		public static WeatherAssetBase getCustomWeather()
		{
			return LevelLighting.GetActiveWeatherAsset();
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x0009E6E7 File Offset: 0x0009C8E7
		public static WeatherAssetBase GetActiveWeatherAsset()
		{
			if (LevelLighting.activeCustomWeather == null)
			{
				return null;
			}
			return LevelLighting.activeCustomWeather.asset;
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x0009E6FC File Offset: 0x0009C8FC
		public static float GetActiveWeatherGlobalBlendAlpha()
		{
			if (LevelLighting.activeCustomWeather == null)
			{
				return 0f;
			}
			return LevelLighting.activeCustomWeather.component.globalBlendAlpha;
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x0009E71C File Offset: 0x0009C91C
		public static bool GetActiveWeatherNetState(out WeatherAssetBase asset, out float blendAlpha, out NetId netId)
		{
			if (LevelLighting.activeCustomWeather != null)
			{
				asset = LevelLighting.activeCustomWeather.asset;
				blendAlpha = LevelLighting.activeCustomWeather.component.globalBlendAlpha;
				netId = LevelLighting.activeCustomWeather.component.GetNetId();
				return true;
			}
			asset = null;
			blendAlpha = 0f;
			netId = default(NetId);
			return false;
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x0009E776 File Offset: 0x0009C976
		public static bool IsWeatherActive(WeatherAssetBase asset)
		{
			return LevelLighting.activeCustomWeather != null && LevelLighting.activeCustomWeather.asset == asset;
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x0009E78E File Offset: 0x0009C98E
		public static bool IsWeatherTransitioningIn(WeatherAssetBase asset)
		{
			return LevelLighting.activeCustomWeather != null && !LevelLighting.activeCustomWeather.component.isFullyTransitionedIn && LevelLighting.activeCustomWeather.asset == asset;
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x0009E7B7 File Offset: 0x0009C9B7
		public static bool IsWeatherFullyTransitionedIn(WeatherAssetBase asset)
		{
			return LevelLighting.activeCustomWeather != null && LevelLighting.activeCustomWeather.component.isFullyTransitionedIn && LevelLighting.activeCustomWeather.asset == asset;
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x0009E7E0 File Offset: 0x0009C9E0
		public static bool IsWeatherTransitioningOut(WeatherAssetBase asset)
		{
			LevelLighting.CustomWeatherInstance customWeatherInstance = LevelLighting.FindWeatherInstanceByAsset(asset);
			return customWeatherInstance != null && !customWeatherInstance.component.isWeatherActive;
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x0009E807 File Offset: 0x0009CA07
		public static bool IsWeatherFullyTransitionedOut(WeatherAssetBase asset)
		{
			return LevelLighting.FindWeatherInstanceByAsset(asset) == null;
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x0009E814 File Offset: 0x0009CA14
		public static bool IsWeatherTransitioning(WeatherAssetBase asset)
		{
			LevelLighting.CustomWeatherInstance customWeatherInstance = LevelLighting.FindWeatherInstanceByAsset(asset);
			return customWeatherInstance != null && !customWeatherInstance.component.isFullyTransitionedIn;
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x0009E83C File Offset: 0x0009CA3C
		public static float GetWeatherGlobalBlendAlpha(WeatherAssetBase asset)
		{
			LevelLighting.CustomWeatherInstance customWeatherInstance = LevelLighting.FindWeatherInstanceByAsset(asset);
			if (customWeatherInstance == null)
			{
				return 0f;
			}
			return customWeatherInstance.component.globalBlendAlpha;
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x0009E864 File Offset: 0x0009CA64
		internal static bool GetWeatherStateForListeners(Guid assetGuid, out bool isActive, out bool isFullyTransitionedIn)
		{
			foreach (LevelLighting.CustomWeatherInstance customWeatherInstance in LevelLighting.customWeatherInstances)
			{
				if (customWeatherInstance.asset.GUID == assetGuid)
				{
					isActive = customWeatherInstance.component.isWeatherActive;
					isFullyTransitionedIn = customWeatherInstance.component.isFullyTransitionedIn;
					return true;
				}
			}
			isActive = false;
			isFullyTransitionedIn = false;
			return false;
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x0009E8EC File Offset: 0x0009CAEC
		[Obsolete]
		public static void setCustomWeather(WeatherAssetBase asset)
		{
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x0009E8F0 File Offset: 0x0009CAF0
		internal static void SetActiveWeatherAsset(WeatherAssetBase asset, float blendAlpha, NetId netId)
		{
			if (LevelLighting.activeCustomWeather != null)
			{
				if (LevelLighting.activeCustomWeather.asset == asset)
				{
					return;
				}
				LevelLighting.activeCustomWeather.component.OnBeginTransitionOut();
				WeatherEventListenerManager.InvokeBeginTransitionOut(LevelLighting.activeCustomWeather.asset.GUID);
				WeatherEventListenerManager.InvokeStatusChange(LevelLighting.activeCustomWeather.asset, EWeatherStatusChange.BeginTransitionOut);
				LevelLighting.activeCustomWeather.component.isWeatherActive = false;
				LevelLighting.activeCustomWeather = null;
			}
			if (asset == null)
			{
				return;
			}
			foreach (LevelLighting.CustomWeatherInstance customWeatherInstance in LevelLighting.customWeatherInstances)
			{
				if (customWeatherInstance.asset.GUID == asset.GUID)
				{
					LevelLighting.activeCustomWeather = customWeatherInstance;
					break;
				}
			}
			if (LevelLighting.activeCustomWeather == null)
			{
				LevelLighting.activeCustomWeather = new LevelLighting.CustomWeatherInstance();
				LevelLighting.activeCustomWeather.asset = asset;
				LevelLighting.activeCustomWeather.netId = netId;
				LevelLighting.activeCustomWeather.initialize();
				LevelLighting.customWeatherInstances.Add(LevelLighting.activeCustomWeather);
			}
			LevelLighting.activeCustomWeather.component.isWeatherActive = true;
			WeatherEventListenerManager.InvokeBeginTransitionIn(LevelLighting.activeCustomWeather.asset.GUID);
			WeatherEventListenerManager.InvokeStatusChange(asset, EWeatherStatusChange.BeginTransitionIn);
			LevelLighting.activeCustomWeather.component.globalBlendAlpha = blendAlpha;
			LevelLighting.activeCustomWeather.component.OnBeginTransitionIn();
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060026CA RID: 9930 RVA: 0x0009EA48 File Offset: 0x0009CC48
		// (set) Token: 0x060026CB RID: 9931 RVA: 0x0009EA4F File Offset: 0x0009CC4F
		[Obsolete]
		public static float christmasyness { get; private set; }

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x060026CC RID: 9932 RVA: 0x0009EA57 File Offset: 0x0009CC57
		// (set) Token: 0x060026CD RID: 9933 RVA: 0x0009EA5E File Offset: 0x0009CC5E
		[Obsolete]
		public static float blizzardyness { get; private set; }

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x0009EA66 File Offset: 0x0009CC66
		// (set) Token: 0x060026CF RID: 9935 RVA: 0x0009EA6D File Offset: 0x0009CC6D
		[Obsolete]
		public static float mistyness { get; private set; }

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x0009EA75 File Offset: 0x0009CC75
		// (set) Token: 0x060026D1 RID: 9937 RVA: 0x0009EA7C File Offset: 0x0009CC7C
		[Obsolete]
		public static float drizzlyness { get; private set; }

		/// <summary>
		/// Hash of lighting config.
		/// Prevents using the level editor to make night time look like day.
		/// </summary>
		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060026D2 RID: 9938 RVA: 0x0009EA84 File Offset: 0x0009CC84
		// (set) Token: 0x060026D3 RID: 9939 RVA: 0x0009EA8B File Offset: 0x0009CC8B
		public static byte[] hash { get; private set; }

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x060026D4 RID: 9940 RVA: 0x0009EA93 File Offset: 0x0009CC93
		public static LightingInfo[] times
		{
			get
			{
				return LevelLighting._times;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x0009EA9A File Offset: 0x0009CC9A
		// (set) Token: 0x060026D6 RID: 9942 RVA: 0x0009EAA1 File Offset: 0x0009CCA1
		public static float seaLevel
		{
			get
			{
				return LevelLighting._seaLevel;
			}
			set
			{
				LevelLighting._seaLevel = value;
				LevelLighting.UpdateBubblesActive();
				LevelLighting.UpdateLegacyWaterTransform();
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060026D7 RID: 9943 RVA: 0x0009EAB3 File Offset: 0x0009CCB3
		// (set) Token: 0x060026D8 RID: 9944 RVA: 0x0009EABA File Offset: 0x0009CCBA
		public static float snowLevel
		{
			get
			{
				return LevelLighting._snowLevel;
			}
			set
			{
				LevelLighting._snowLevel = value;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060026D9 RID: 9945 RVA: 0x0009EAC2 File Offset: 0x0009CCC2
		// (set) Token: 0x060026DA RID: 9946 RVA: 0x0009EAC9 File Offset: 0x0009CCC9
		public static ELightingVision vision
		{
			get
			{
				return LevelLighting._vision;
			}
			set
			{
				if (value != LevelLighting._vision)
				{
					LevelLighting._vision = value;
					LevelLighting.skyboxNeedsReflectionUpdate = true;
				}
			}
		}

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x060026DB RID: 9947 RVA: 0x0009EAE0 File Offset: 0x0009CCE0
		// (remove) Token: 0x060026DC RID: 9948 RVA: 0x0009EB14 File Offset: 0x0009CD14
		public static event LevelLighting.IsSeaChangedHandler isSeaChanged;

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060026DD RID: 9949 RVA: 0x0009EB47 File Offset: 0x0009CD47
		// (set) Token: 0x060026DE RID: 9950 RVA: 0x0009EB4E File Offset: 0x0009CD4E
		public static bool isSea
		{
			get
			{
				return LevelLighting._isSea;
			}
			protected set
			{
				if (LevelLighting.isSea == value)
				{
					return;
				}
				LevelLighting._isSea = value;
				LevelLighting.IsSeaChangedHandler isSeaChangedHandler = LevelLighting.isSeaChanged;
				if (isSeaChangedHandler != null)
				{
					isSeaChangedHandler(LevelLighting.isSea);
				}
				LevelLighting.skyboxNeedsReflectionUpdate = true;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060026DF RID: 9951 RVA: 0x0009EB7A File Offset: 0x0009CD7A
		// (set) Token: 0x060026E0 RID: 9952 RVA: 0x0009EB81 File Offset: 0x0009CD81
		public static Color skyboxSky { get; private set; }

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060026E1 RID: 9953 RVA: 0x0009EB89 File Offset: 0x0009CD89
		// (set) Token: 0x060026E2 RID: 9954 RVA: 0x0009EB90 File Offset: 0x0009CD90
		public static Color skyboxEquator { get; private set; }

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x060026E3 RID: 9955 RVA: 0x0009EB98 File Offset: 0x0009CD98
		public static AudioSource effectAudio
		{
			get
			{
				return LevelLighting._effectAudio;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060026E4 RID: 9956 RVA: 0x0009EB9F File Offset: 0x0009CD9F
		public static AudioSource dayAudio
		{
			get
			{
				return LevelLighting._dayAudio;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x060026E5 RID: 9957 RVA: 0x0009EBA6 File Offset: 0x0009CDA6
		public static AudioSource nightAudio
		{
			get
			{
				return LevelLighting._nightAudio;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060026E6 RID: 9958 RVA: 0x0009EBAD File Offset: 0x0009CDAD
		public static AudioSource waterAudio
		{
			get
			{
				return LevelLighting._waterAudio;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060026E7 RID: 9959 RVA: 0x0009EBB4 File Offset: 0x0009CDB4
		public static AudioSource windAudio
		{
			get
			{
				return LevelLighting._windAudio;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060026E8 RID: 9960 RVA: 0x0009EBBB File Offset: 0x0009CDBB
		public static AudioSource belowAudio
		{
			get
			{
				return LevelLighting._belowAudio;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x0009EBC2 File Offset: 0x0009CDC2
		// (set) Token: 0x060026EA RID: 9962 RVA: 0x0009EBC9 File Offset: 0x0009CDC9
		public static bool isSkyboxReflectionEnabled
		{
			get
			{
				return LevelLighting._isSkyboxReflectionEnabled;
			}
			set
			{
				LevelLighting._isSkyboxReflectionEnabled = value;
				LevelLighting.updateSkyboxReflections();
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x0009EBD6 File Offset: 0x0009CDD6
		public static Transform bubbles
		{
			get
			{
				return LevelLighting._bubbles;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x060026EC RID: 9964 RVA: 0x0009EBDD File Offset: 0x0009CDDD
		public static WindZone windZone
		{
			get
			{
				return LevelLighting._windZone;
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x0009EBE4 File Offset: 0x0009CDE4
		// (set) Token: 0x060026EE RID: 9966 RVA: 0x0009EBEB File Offset: 0x0009CDEB
		public static byte moon
		{
			get
			{
				return LevelLighting._moon;
			}
			set
			{
				LevelLighting._moon = value;
			}
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x0009EBF3 File Offset: 0x0009CDF3
		public static void setEnabled(bool isEnabled)
		{
			if (LevelLighting.sun != null)
			{
				LevelLighting.sunLight.enabled = isEnabled;
			}
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x0009EC0D File Offset: 0x0009CE0D
		public static bool isPositionSnowy(Vector3 position)
		{
			return Level.info != null && Level.info.configData.Use_Legacy_Snow_Height && LevelLighting.snowLevel > 0.01f && position.y > LevelLighting.snowLevel * Level.TERRAIN;
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x0009EC4A File Offset: 0x0009CE4A
		[Obsolete("Replaced by WaterUtility")]
		public static bool isPositionUnderwater(Vector3 position)
		{
			return Level.info != null && Level.info.configData.Use_Legacy_Water && LevelLighting.seaLevel < 0.99f && position.y < LevelLighting.seaLevel * Level.TERRAIN;
		}

		/// <summary>
		/// If global ocean plane is enabled then return the worldspace height,
		/// otherwise return the optional default value. Default for volume based
		/// water is -1024, but atmosphere measure uses a default of zero.
		/// </summary>
		// Token: 0x060026F2 RID: 9970 RVA: 0x0009EC87 File Offset: 0x0009CE87
		public static float getWaterSurfaceElevation(float defaultValue = -1024f)
		{
			if (Level.info != null && Level.info.configData.Use_Legacy_Water && LevelLighting.seaLevel < 0.99f)
			{
				return LevelLighting.seaLevel * Level.TERRAIN;
			}
			return defaultValue;
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x0009ECBC File Offset: 0x0009CEBC
		public static void setSeaVector(string name, Vector4 vector)
		{
			foreach (WaterVolume waterVolume in VolumeManager<WaterVolume, WaterVolumeManager>.Get().InternalGetAllVolumes())
			{
				if (!(waterVolume.sharedMaterial == null))
				{
					waterVolume.sharedMaterial.SetVector(name, vector);
				}
			}
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x0009ED28 File Offset: 0x0009CF28
		public static Color getSeaColor(string name)
		{
			WaterVolume firstOrDefaultWaterVolume = LevelLighting.GetFirstOrDefaultWaterVolume();
			Color? color;
			if (firstOrDefaultWaterVolume == null)
			{
				color = default(Color?);
			}
			else
			{
				Material sharedMaterial = firstOrDefaultWaterVolume.sharedMaterial;
				color = ((sharedMaterial != null) ? new Color?(sharedMaterial.GetColor(name)) : default(Color?));
			}
			Color? color2 = color;
			if (color2 == null)
			{
				return Vector4.zero;
			}
			return color2.GetValueOrDefault();
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x0009ED84 File Offset: 0x0009CF84
		public static void setSeaColor(string name, Color color)
		{
			foreach (WaterVolume waterVolume in VolumeManager<WaterVolume, WaterVolumeManager>.Get().InternalGetAllVolumes())
			{
				if (!(waterVolume.sharedMaterial == null))
				{
					waterVolume.sharedMaterial.SetColor(name, color);
				}
			}
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x0009EDF0 File Offset: 0x0009CFF0
		public static float getSeaFloat(string name)
		{
			WaterVolume firstOrDefaultWaterVolume = LevelLighting.GetFirstOrDefaultWaterVolume();
			float? num;
			if (firstOrDefaultWaterVolume == null)
			{
				num = default(float?);
			}
			else
			{
				Material sharedMaterial = firstOrDefaultWaterVolume.sharedMaterial;
				num = ((sharedMaterial != null) ? new float?(sharedMaterial.GetFloat(name)) : default(float?));
			}
			float? num2 = num;
			return num2.GetValueOrDefault();
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x0009EE38 File Offset: 0x0009D038
		public static void setSeaFloat(string name, float value)
		{
			foreach (WaterVolume waterVolume in VolumeManager<WaterVolume, WaterVolumeManager>.Get().InternalGetAllVolumes())
			{
				if (!(waterVolume.sharedMaterial == null))
				{
					waterVolume.sharedMaterial.SetFloat(name, value);
				}
			}
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x0009EEA4 File Offset: 0x0009D0A4
		private static WaterVolume GetFirstOrDefaultWaterVolume()
		{
			IReadOnlyList<WaterVolume> readOnlyList = VolumeManager<WaterVolume, WaterVolumeManager>.Get().InternalGetAllVolumes();
			if (readOnlyList.Count <= 0)
			{
				return null;
			}
			return readOnlyList[0];
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x0009EED0 File Offset: 0x0009D0D0
		public static void updateLighting()
		{
			if (LevelLighting.sun == null)
			{
				return;
			}
			float num = 0f;
			LevelLighting.setSeaVector("_WorldLightDir", LevelLighting.sun.forward);
			int num2;
			int num3;
			float value;
			if (LevelLighting.time < LevelLighting.bias)
			{
				LevelLighting.sun.rotation = Quaternion.Euler(LevelLighting.time / LevelLighting.bias * 180f, LevelLighting.azimuth, 0f);
				if (LevelLighting.time < LevelLighting.transition)
				{
					LevelLighting.dayVolume = Mathf.Lerp(0.5f, 1f, LevelLighting.time / LevelLighting.transition);
					LevelLighting.nightVolume = Mathf.Lerp(0.5f, 0f, LevelLighting.time / LevelLighting.transition);
					num2 = 0;
					num3 = 1;
					num = LevelLighting.time / LevelLighting.transition;
					LevelLighting.setSeaColor("_Foam", Color.Lerp(LevelLighting.FOAM_DAWN, LevelLighting.FOAM_MIDDAY, LevelLighting.time / LevelLighting.transition));
					LevelLighting.setSeaFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_DAWN, LevelLighting.SPECULAR_MIDDAY, LevelLighting.time / LevelLighting.transition));
					RenderSettings.reflectionIntensity = Mathf.Lerp(LevelLighting.REFLECTION_DAWN, LevelLighting.REFLECTION_MIDDAY, LevelLighting.time / LevelLighting.transition);
				}
				else if (LevelLighting.time < LevelLighting.bias - LevelLighting.transition)
				{
					LevelLighting.dayVolume = 1f;
					LevelLighting.nightVolume = 0f;
					num2 = -1;
					num3 = 1;
					LevelLighting.setSeaColor("_Foam", LevelLighting.FOAM_MIDDAY);
					LevelLighting.setSeaFloat("_Shininess", LevelLighting.SPECULAR_MIDDAY);
					RenderSettings.reflectionIntensity = LevelLighting.REFLECTION_MIDDAY;
				}
				else
				{
					LevelLighting.dayVolume = Mathf.Lerp(1f, 0.5f, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition);
					LevelLighting.nightVolume = Mathf.Lerp(0f, 0.5f, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition);
					num2 = 1;
					num3 = 2;
					num = (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition;
					LevelLighting.setSeaColor("_Foam", Color.Lerp(LevelLighting.FOAM_MIDDAY, LevelLighting.FOAM_DUSK, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition));
					LevelLighting.setSeaFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_MIDDAY, LevelLighting.SPECULAR_DUSK, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition));
					RenderSettings.reflectionIntensity = Mathf.Lerp(LevelLighting.REFLECTION_MIDDAY, LevelLighting.REFLECTION_DUSK, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition);
				}
				value = 1f;
				LevelLighting.auroraBorealisTargetIntensity = 0f;
			}
			else
			{
				LevelLighting.sun.rotation = Quaternion.Euler(180f + (LevelLighting.time - LevelLighting.bias) / (1f - LevelLighting.bias) * 180f, LevelLighting.azimuth, 0f);
				if (LevelLighting.time < LevelLighting.bias + LevelLighting.transition)
				{
					LevelLighting.dayVolume = Mathf.Lerp(0.5f, 0f, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition);
					LevelLighting.nightVolume = Mathf.Lerp(0.5f, 1f, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition);
					num2 = 2;
					num3 = 3;
					num = (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition;
					LevelLighting.setSeaColor("_Foam", Color.Lerp(LevelLighting.FOAM_DUSK, LevelLighting.FOAM_MIDNIGHT, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition));
					LevelLighting.setSeaFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_DUSK, LevelLighting.SPECULAR_MIDNIGHT, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition));
					RenderSettings.reflectionIntensity = Mathf.Lerp(LevelLighting.REFLECTION_DUSK, LevelLighting.REFLECTION_MIDNIGHT, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition);
					value = Mathf.Lerp(1f, 0.05f, num);
					LevelLighting.auroraBorealisTargetIntensity = 0f;
				}
				else if (LevelLighting.time < 1f - LevelLighting.transition)
				{
					LevelLighting.dayVolume = 0f;
					LevelLighting.nightVolume = 1f;
					num2 = -1;
					num3 = 3;
					LevelLighting.setSeaColor("_Foam", LevelLighting.FOAM_MIDNIGHT);
					LevelLighting.setSeaFloat("_Shininess", LevelLighting.SPECULAR_MIDNIGHT);
					RenderSettings.reflectionIntensity = LevelLighting.REFLECTION_MIDNIGHT;
					value = 0.05f;
					LevelLighting.auroraBorealisTargetIntensity = 1f;
				}
				else
				{
					LevelLighting.dayVolume = Mathf.Lerp(0f, 0.5f, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition);
					LevelLighting.nightVolume = Mathf.Lerp(1f, 0.5f, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition);
					num2 = 3;
					num3 = 0;
					num = (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition;
					LevelLighting.setSeaColor("_Foam", Color.Lerp(LevelLighting.FOAM_MIDNIGHT, LevelLighting.FOAM_DAWN, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition));
					LevelLighting.setSeaFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_MIDNIGHT, LevelLighting.SPECULAR_DAWN, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition));
					RenderSettings.reflectionIntensity = Mathf.Lerp(LevelLighting.REFLECTION_MIDNIGHT, LevelLighting.REFLECTION_DAWN, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition);
					value = Mathf.Lerp(0.05f, 1f, num);
					LevelLighting.auroraBorealisTargetIntensity = 0f;
				}
			}
			LightingInfo lightingInfo = (num2 < 0) ? null : LevelLighting.times[num2];
			LightingInfo lightingInfo2 = LevelLighting.times[num3];
			float num4;
			float value2;
			if (lightingInfo == null)
			{
				LevelLighting.sunLight.color = lightingInfo2.colors[0];
				LevelLighting.sunLight.intensity = lightingInfo2.singles[0];
				num4 = lightingInfo2.singles[3];
				LevelLighting.setSeaColor("_BaseColor", lightingInfo2.colors[1]);
				LevelLighting.setSeaColor("_ReflectionColor", lightingInfo2.colors[1]);
				RenderSettings.ambientSkyColor = lightingInfo2.colors[6];
				RenderSettings.ambientEquatorColor = lightingInfo2.colors[7];
				RenderSettings.ambientGroundColor = lightingInfo2.colors[8];
				LevelLighting.skyboxSky = lightingInfo2.colors[3];
				LevelLighting.skyboxEquator = lightingInfo2.colors[4];
				LevelLighting.skyboxGround = lightingInfo2.colors[5];
				LevelLighting.cloudRimColor = lightingInfo2.colors[9];
				LevelLighting.particleLightingColor = lightingInfo2.colors[11];
				LevelLighting.raysColor = lightingInfo2.colors[10];
				LevelLighting.raysIntensity = lightingInfo2.singles[4] * 4f;
				LevelLighting.levelFogColor = lightingInfo2.colors[2];
				LevelLighting.levelFogIntensity = lightingInfo2.singles[1];
				value2 = lightingInfo2.singles[2];
			}
			else
			{
				LevelLighting.sunLight.color = Color.Lerp(lightingInfo.colors[0], lightingInfo2.colors[0], num);
				LevelLighting.sunLight.intensity = Mathf.Lerp(lightingInfo.singles[0], lightingInfo2.singles[0], num);
				num4 = Mathf.Lerp(lightingInfo.singles[3], lightingInfo2.singles[3], num);
				LevelLighting.setSeaColor("_BaseColor", Color.Lerp(lightingInfo.colors[1], lightingInfo2.colors[1], num));
				LevelLighting.setSeaColor("_ReflectionColor", Color.Lerp(lightingInfo.colors[1], lightingInfo2.colors[1], num));
				RenderSettings.ambientSkyColor = Color.Lerp(lightingInfo.colors[6], lightingInfo2.colors[6], num);
				RenderSettings.ambientEquatorColor = Color.Lerp(lightingInfo.colors[7], lightingInfo2.colors[7], num);
				RenderSettings.ambientGroundColor = Color.Lerp(lightingInfo.colors[8], lightingInfo2.colors[8], num);
				LevelLighting.skyboxSky = Color.Lerp(lightingInfo.colors[3], lightingInfo2.colors[3], num);
				LevelLighting.skyboxEquator = Color.Lerp(lightingInfo.colors[4], lightingInfo2.colors[4], num);
				LevelLighting.skyboxGround = Color.Lerp(lightingInfo.colors[5], lightingInfo2.colors[5], num);
				LevelLighting.cloudRimColor = Color.Lerp(lightingInfo.colors[9], lightingInfo2.colors[9], num);
				LevelLighting.particleLightingColor = Color.Lerp(lightingInfo.colors[11], lightingInfo2.colors[11], num);
				LevelLighting.raysColor = Color.Lerp(lightingInfo.colors[10], lightingInfo2.colors[10], num);
				LevelLighting.raysIntensity = Mathf.Lerp(lightingInfo.singles[4], lightingInfo2.singles[4], num) * 4f;
				LevelLighting.levelFogColor = Color.Lerp(lightingInfo.colors[2], lightingInfo2.colors[2], num);
				LevelLighting.levelFogIntensity = Mathf.Lerp(lightingInfo.singles[1], lightingInfo2.singles[1], num);
				value2 = Mathf.Lerp(lightingInfo.singles[2], lightingInfo2.singles[2], num);
			}
			LevelLighting.cloudColor = LevelLighting.cloudRimColor;
			LevelLighting.levelAtmosphericFog = 0f;
			float num5 = 1f;
			float num6 = 1f;
			foreach (LevelLighting.CustomWeatherInstance customWeatherInstance in LevelLighting.customWeatherInstances)
			{
				customWeatherInstance.component.UpdateLightingTime(num2, num3, num);
				if (customWeatherInstance.component.overrideFog)
				{
					float t = Mathf.Pow(customWeatherInstance.component.EffectBlendAlpha, customWeatherInstance.component.fogBlendExponent);
					LevelLighting.levelFogColor = Color.Lerp(LevelLighting.levelFogColor, customWeatherInstance.component.fogColor, t);
					LevelLighting.levelFogIntensity = Mathf.Lerp(LevelLighting.levelFogIntensity, customWeatherInstance.component.fogDensity, t);
					if (customWeatherInstance.component.overrideAtmosphericFog)
					{
						LevelLighting.levelAtmosphericFog = Mathf.Lerp(LevelLighting.levelAtmosphericFog, 1f, t);
					}
				}
				if (customWeatherInstance.component.overrideCloudColors)
				{
					float t2 = Mathf.Pow(customWeatherInstance.component.EffectBlendAlpha, customWeatherInstance.component.cloudBlendExponent);
					LevelLighting.cloudColor = Color.Lerp(LevelLighting.cloudColor, customWeatherInstance.component.cloudColor, t2);
					LevelLighting.cloudRimColor = Color.Lerp(LevelLighting.cloudRimColor, customWeatherInstance.component.cloudRimColor, t2);
				}
				num5 = Mathf.Lerp(num5, customWeatherInstance.component.shadowStrengthMultiplier, customWeatherInstance.component.EffectBlendAlpha);
				num6 = Mathf.Lerp(num6, customWeatherInstance.component.brightnessMultiplier, customWeatherInstance.component.EffectBlendAlpha);
			}
			if (LevelLighting.localBlendingFog)
			{
				LevelLighting.levelFogColor = Color.Lerp(LevelLighting.levelFogColor, LevelLighting.localFogColor, LevelLighting.localFogBlend);
				LevelLighting.levelFogIntensity = Mathf.Lerp(LevelLighting.levelFogIntensity, LevelLighting.localFogIntensity, LevelLighting.localFogBlend);
				LevelLighting.levelAtmosphericFog = Mathf.Lerp(LevelLighting.levelAtmosphericFog, LevelLighting.localAtmosphericFog, LevelLighting.localFogBlend);
			}
			LevelLighting.sunLight.shadowStrength = num4 * num5;
			if (num6 != 1f)
			{
				LevelLighting.setSeaColor("_Foam", LevelLighting.getSeaColor("_Foam") * num6);
				LevelLighting.setSeaFloat("_Shininess", LevelLighting.getSeaFloat("_Shininess") * num6);
				LevelLighting.setSeaColor("_BaseColor", LevelLighting.getSeaColor("_BaseColor") * num6);
				LevelLighting.setSeaColor("_ReflectionColor", LevelLighting.getSeaColor("_ReflectionColor") * num6);
				LevelLighting.sunLight.intensity *= num6;
				RenderSettings.ambientSkyColor *= num6;
				RenderSettings.ambientEquatorColor *= num6;
				RenderSettings.ambientGroundColor *= num6;
				LevelLighting.skyboxSky *= num6;
				LevelLighting.skyboxEquator *= num6;
				LevelLighting.skyboxGround *= num6;
				LevelLighting.particleLightingColor *= num6;
			}
			if (LevelLighting.localBlendingLight)
			{
				LevelLighting.setSeaColor("_Foam", Color.Lerp(LevelLighting.getSeaColor("_Foam"), Color.black, LevelLighting.localLightingBlend * LevelLighting.PITCH_DARK_WATER_BLEND));
				LevelLighting.setSeaFloat("_Shininess", Mathf.Lerp(LevelLighting.getSeaFloat("_Shininess"), 0f, LevelLighting.localLightingBlend * LevelLighting.PITCH_DARK_WATER_BLEND));
				LevelLighting.setSeaColor("_BaseColor", Color.Lerp(LevelLighting.getSeaColor("_BaseColor"), Color.black, LevelLighting.localLightingBlend * LevelLighting.PITCH_DARK_WATER_BLEND));
				LevelLighting.setSeaColor("_ReflectionColor", Color.Lerp(LevelLighting.getSeaColor("_ReflectionColor"), Color.black, LevelLighting.localLightingBlend * LevelLighting.PITCH_DARK_WATER_BLEND));
				LevelLighting.sunLight.color = Color.Lerp(LevelLighting.sunLight.color, Color.black, LevelLighting.localLightingBlend);
				LevelLighting.sunLight.intensity = Mathf.Lerp(LevelLighting.sunLight.intensity, 0f, LevelLighting.localLightingBlend);
				LevelLighting.sunLight.shadowStrength = Mathf.Lerp(LevelLighting.sunLight.shadowStrength, 0f, LevelLighting.localLightingBlend);
				RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, Color.black, LevelLighting.localLightingBlend);
				RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, Color.black, LevelLighting.localLightingBlend);
				RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, Color.black, LevelLighting.localLightingBlend);
				RenderSettings.ambientMode = AmbientMode.Trilight;
				LevelLighting.skyboxSky = Color.Lerp(LevelLighting.skyboxSky, Color.black, LevelLighting.localLightingBlend);
				LevelLighting.skyboxEquator = Color.Lerp(LevelLighting.skyboxEquator, Color.black, LevelLighting.localLightingBlend);
				LevelLighting.skyboxGround = Color.Lerp(LevelLighting.skyboxGround, Color.black, LevelLighting.localLightingBlend);
				LevelLighting.cloudRimColor = Color.Lerp(LevelLighting.cloudRimColor, Color.black, LevelLighting.localLightingBlend);
				LevelLighting.particleLightingColor = Color.Lerp(LevelLighting.particleLightingColor, Color.black, LevelLighting.localLightingBlend);
			}
			LevelLighting.setSeaColor("_SpecularColor", LevelLighting.sunLight.color);
			if (LevelLighting.vision == ELightingVision.MILITARY || LevelLighting.vision == ELightingVision.CIVILIAN)
			{
				LevelLighting.setSeaColor("_BaseColor", LevelLighting.nightvisionColor);
				LevelLighting.setSeaColor("_ReflectionColor", LevelLighting.nightvisionColor);
				RenderSettings.ambientSkyColor = LevelLighting.nightvisionColor;
				RenderSettings.ambientEquatorColor = LevelLighting.nightvisionColor;
				RenderSettings.ambientGroundColor = LevelLighting.nightvisionColor;
				RenderSettings.ambientMode = AmbientMode.Trilight;
				LevelLighting.skyboxSky = LevelLighting.nightvisionColor;
				LevelLighting.skyboxEquator = LevelLighting.nightvisionColor;
				LevelLighting.skyboxGround = LevelLighting.nightvisionColor;
				LevelLighting.cloudRimColor = LevelLighting.nightvisionColor;
				LevelLighting.levelFogColor = LevelLighting.nightvisionColor;
				LevelLighting.levelFogIntensity = Mathf.Max(LevelLighting.levelFogIntensity, LevelLighting.nightvisionFogIntensity);
				if (LevelLighting.localBlendingLight)
				{
					RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, Color.black, LevelLighting.localLightingBlend / 2f);
					RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientSkyColor, Color.black, LevelLighting.localLightingBlend / 2f);
					RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientSkyColor, Color.black, LevelLighting.localLightingBlend / 2f);
					LevelLighting.skyboxSky = Color.Lerp(LevelLighting.skyboxSky, Color.black, LevelLighting.localLightingBlend / 2f);
					LevelLighting.skyboxEquator = Color.Lerp(LevelLighting.skyboxEquator, Color.black, LevelLighting.localLightingBlend / 2f);
					LevelLighting.skyboxGround = Color.Lerp(LevelLighting.skyboxGround, Color.black, LevelLighting.localLightingBlend / 2f);
					LevelLighting.cloudRimColor = Color.Lerp(LevelLighting.cloudRimColor, Color.black, LevelLighting.localLightingBlend / 2f);
				}
			}
			if (MainCamera.instance != null)
			{
				SunShaftsCs component = MainCamera.instance.GetComponent<SunShaftsCs>();
				if (component != null)
				{
					component.sunTransform = LevelLighting.sunFlare;
					component.sunColor = LevelLighting.raysColor;
				}
			}
			LevelLighting.skybox.SetVector("_SkyHackAmbientEquator", RenderSettings.ambientEquatorColor.linear);
			LevelLighting.skybox.SetVector("_SkyHackAmbientGround", RenderSettings.ambientGroundColor.linear);
			LevelLighting.skybox.SetVector("_SunDirection", LevelLighting.sun.forward);
			LevelLighting.skybox.SetColor("_SunColor", Color.Lerp(LevelLighting.sunLight.color, Color.white, 0.5f));
			LevelLighting.skybox.SetFloat("_StarsCutoff", value);
			LevelLighting.skybox.SetVector("_MoonDirection", -LevelLighting.sun.forward);
			LevelLighting.skybox.SetVector("_MoonLightDirection", LevelLighting.moons[(int)LevelLighting._moon].forward);
			LevelLighting.skybox.SetColor("_CloudColor", LevelLighting.cloudColor);
			LevelLighting.skybox.SetColor("_CloudRimColor", LevelLighting.cloudRimColor);
			LevelLighting.skybox.SetFloat("_CloudIntensity", value2);
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x000A0018 File Offset: 0x0009E218
		private static void updateHolidayWeatherRestrictions()
		{
			if (Level.shouldUseHolidayRedirects)
			{
				LevelLighting.canRain = false;
				LevelLighting.canSnow = false;
			}
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x000A0030 File Offset: 0x0009E230
		public static void load(ushort size)
		{
			LevelLighting.vision = ELightingVision.NONE;
			LevelLighting.isSea = false;
			LevelLighting.localEffectNode = null;
			LevelLighting.currentEffectAsset = null;
			LevelLighting.localPlayingEffect = false;
			LevelLighting.localBlendingLight = false;
			LevelLighting.localLightingBlend = 1f;
			LevelLighting.localBlendingFog = false;
			LevelLighting.localFogBlend = 1f;
			LevelLighting.auroraBorealisCurrentIntensity = 0f;
			LevelLighting.auroraBorealisTargetIntensity = 0f;
			LevelLighting.currentAudioVolume = 0f;
			LevelLighting.targetAudioVolume = 0f;
			LevelLighting.nextAudioVolumeChangeTime = -1f;
			LevelLighting.customWeatherInstances.Clear();
			LevelLighting.activeCustomWeather = null;
			LevelLighting.legacyWater = null;
			LevelLighting.legacyWaterTransform = null;
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Lighting.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Environment/Lighting.dat", false, false, 0);
				byte b = block.readByte();
				LevelLighting._azimuth = block.readSingle();
				LevelLighting._bias = block.readSingle();
				LevelLighting._fade = block.readSingle();
				LevelLighting._time = block.readSingle();
				LevelLighting.moon = block.readByte();
				if (b >= 5)
				{
					LevelLighting._seaLevel = block.readSingle();
					LevelLighting._snowLevel = block.readSingle();
					if (b > 6)
					{
						LevelLighting.canRain = block.readBoolean();
					}
					else
					{
						LevelLighting.canRain = false;
					}
					if (b > 10)
					{
						LevelLighting.canSnow = block.readBoolean();
					}
					else
					{
						LevelLighting.canSnow = false;
					}
					if (b < 8)
					{
						LevelLighting.rainFreq = 1f;
						LevelLighting.rainDur = 1f;
					}
					else
					{
						LevelLighting.rainFreq = block.readSingle();
						LevelLighting.rainDur = block.readSingle();
					}
					if (b < 11)
					{
						LevelLighting.snowFreq = 1f;
						LevelLighting.snowDur = 1f;
					}
					else
					{
						LevelLighting.snowFreq = block.readSingle();
						LevelLighting.snowDur = block.readSingle();
					}
					LevelLighting._times = new LightingInfo[4];
					for (int i = 0; i < LevelLighting.times.Length; i++)
					{
						Color[] array = new Color[12];
						float[] array2 = new float[5];
						if (b > 9)
						{
							for (int j = 0; j < array.Length; j++)
							{
								array[j] = block.readColor();
							}
							for (int k = 0; k < array2.Length; k++)
							{
								array2[k] = block.readSingle();
							}
						}
						else if (b > 8)
						{
							for (int l = 0; l < array.Length - 1; l++)
							{
								array[l] = block.readColor();
							}
							array[11] = array[3];
							for (int m = 0; m < array2.Length; m++)
							{
								array2[m] = block.readSingle();
							}
						}
						else
						{
							if (b >= 6)
							{
								for (int n = 0; n < array.Length - 2; n++)
								{
									array[n] = block.readColor();
								}
							}
							else
							{
								for (int num = 0; num < array.Length - 3; num++)
								{
									array[num] = block.readColor();
								}
								array[9] = array[2];
							}
							array[10] = array[0];
							array[11] = array[3];
							for (int num2 = 0; num2 < array2.Length - 1; num2++)
							{
								array2[num2] = block.readSingle();
							}
							array2[4] = 0.25f;
						}
						if (b < 12)
						{
							array2[1] = Mathf.Min(array2[1], 0.33f);
						}
						LightingInfo lightingInfo = new LightingInfo(array, array2);
						LevelLighting.times[i] = lightingInfo;
					}
				}
				else
				{
					LevelLighting._times = new LightingInfo[4];
					for (int num3 = 0; num3 < LevelLighting.times.Length; num3++)
					{
						Color[] newColors = new Color[12];
						float[] newSingles = new float[5];
						LightingInfo lightingInfo2 = new LightingInfo(newColors, newSingles);
						LevelLighting.times[num3] = lightingInfo2;
					}
					LevelLighting.times[0].colors[3] = block.readColor();
					LevelLighting.times[1].colors[3] = block.readColor();
					LevelLighting.times[2].colors[3] = block.readColor();
					LevelLighting.times[3].colors[3] = block.readColor();
					LevelLighting.times[0].colors[4] = LevelLighting.times[0].colors[3];
					LevelLighting.times[1].colors[4] = LevelLighting.times[1].colors[3];
					LevelLighting.times[2].colors[4] = LevelLighting.times[2].colors[3];
					LevelLighting.times[3].colors[4] = LevelLighting.times[3].colors[3];
					LevelLighting.times[0].colors[5] = LevelLighting.times[0].colors[3];
					LevelLighting.times[1].colors[5] = LevelLighting.times[1].colors[3];
					LevelLighting.times[2].colors[5] = LevelLighting.times[2].colors[3];
					LevelLighting.times[3].colors[5] = LevelLighting.times[3].colors[3];
					LevelLighting.times[0].colors[6] = block.readColor();
					LevelLighting.times[1].colors[6] = block.readColor();
					LevelLighting.times[2].colors[6] = block.readColor();
					LevelLighting.times[3].colors[6] = block.readColor();
					LevelLighting.times[0].colors[7] = LevelLighting.times[0].colors[6];
					LevelLighting.times[1].colors[7] = LevelLighting.times[1].colors[6];
					LevelLighting.times[2].colors[7] = LevelLighting.times[2].colors[6];
					LevelLighting.times[3].colors[7] = LevelLighting.times[3].colors[6];
					LevelLighting.times[0].colors[8] = LevelLighting.times[0].colors[6];
					LevelLighting.times[1].colors[8] = LevelLighting.times[1].colors[6];
					LevelLighting.times[2].colors[8] = LevelLighting.times[2].colors[6];
					LevelLighting.times[3].colors[8] = LevelLighting.times[3].colors[6];
					LevelLighting.times[0].colors[2] = block.readColor();
					LevelLighting.times[1].colors[2] = block.readColor();
					LevelLighting.times[2].colors[2] = block.readColor();
					LevelLighting.times[3].colors[2] = block.readColor();
					LevelLighting.times[0].colors[0] = block.readColor();
					LevelLighting.times[1].colors[0] = block.readColor();
					LevelLighting.times[2].colors[0] = block.readColor();
					LevelLighting.times[3].colors[0] = block.readColor();
					LevelLighting.times[0].singles[0] = block.readSingle();
					LevelLighting.times[1].singles[0] = block.readSingle();
					LevelLighting.times[2].singles[0] = block.readSingle();
					LevelLighting.times[3].singles[0] = block.readSingle();
					LevelLighting.times[0].singles[1] = block.readSingle();
					LevelLighting.times[1].singles[1] = block.readSingle();
					LevelLighting.times[2].singles[1] = block.readSingle();
					LevelLighting.times[3].singles[1] = block.readSingle();
					LevelLighting.times[0].singles[2] = block.readSingle();
					LevelLighting.times[1].singles[2] = block.readSingle();
					LevelLighting.times[2].singles[2] = block.readSingle();
					LevelLighting.times[3].singles[2] = block.readSingle();
					LevelLighting.times[0].singles[3] = block.readSingle();
					LevelLighting.times[1].singles[3] = block.readSingle();
					LevelLighting.times[2].singles[3] = block.readSingle();
					LevelLighting.times[3].singles[3] = block.readSingle();
					if (b > 2)
					{
						LevelLighting._seaLevel = block.readSingle();
					}
					else
					{
						LevelLighting._seaLevel = block.readSingle() / 2f;
					}
					if (b > 1)
					{
						LevelLighting._snowLevel = block.readSingle();
					}
					else
					{
						LevelLighting._snowLevel = 0f;
					}
					LevelLighting.canRain = false;
					LevelLighting.canSnow = false;
					LevelLighting.times[0].colors[1] = block.readColor();
					LevelLighting.times[1].colors[1] = block.readColor();
					LevelLighting.times[2].colors[1] = block.readColor();
					LevelLighting.times[3].colors[1] = block.readColor();
				}
				LevelLighting.hash = block.getHash();
			}
			else
			{
				LevelLighting._azimuth = 0.2f;
				LevelLighting._bias = 0.5f;
				LevelLighting._fade = 1f;
				LevelLighting._time = LevelLighting.bias / 2f;
				LevelLighting.moon = 0;
				LevelLighting._seaLevel = 1f;
				LevelLighting._snowLevel = 0f;
				LevelLighting.canRain = true;
				LevelLighting.canSnow = false;
				LevelLighting.rainFreq = 1f;
				LevelLighting.rainDur = 1f;
				LevelLighting.snowFreq = 1f;
				LevelLighting.snowDur = 1f;
				LevelLighting._times = new LightingInfo[4];
				for (int num4 = 0; num4 < LevelLighting.times.Length; num4++)
				{
					Color[] newColors2 = new Color[12];
					float[] newSingles2 = new float[5];
					LightingInfo lightingInfo3 = new LightingInfo(newColors2, newSingles2);
					LevelLighting.times[num4] = lightingInfo3;
				}
				LevelLighting.hash = new byte[20];
			}
			if (LevelLighting.bias < 1f - LevelLighting.bias)
			{
				LevelLighting._transition = LevelLighting.bias / 2f * LevelLighting.fade;
			}
			else
			{
				LevelLighting._transition = (1f - LevelLighting.bias) / 2f * LevelLighting.fade;
			}
			LevelLighting.times[0].colors[1].a = 0.25f;
			LevelLighting.times[1].colors[1].a = 0.5f;
			LevelLighting.times[2].colors[1].a = 0.75f;
			LevelLighting.times[3].colors[1].a = 0.9f;
			if (Level.info.configData.Use_Legacy_Water)
			{
				GameObject gameObject = new GameObject();
				LevelLighting.legacyWaterTransform = gameObject.transform;
				LevelLighting.legacyWaterTransform.parent = Level.level;
				LevelLighting.legacyWater = gameObject.AddComponent<WaterVolume>();
				LevelLighting.legacyWater.isManagedByLighting = true;
				LevelLighting.legacyWater.isSeaLevel = true;
				LevelLighting.legacyWater.isSurfaceVisible = true;
				LevelLighting.legacyWater.isReflectionVisible = true;
				LevelLighting.UpdateLegacyWaterTransform();
			}
			LevelLighting.init = false;
			LevelLighting.updateHolidayWeatherRestrictions();
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x000A0BA0 File Offset: 0x0009EDA0
		public static void save()
		{
			Block block = new Block();
			block.writeByte(LevelLighting.SAVEDATA_VERSION);
			block.writeSingle(LevelLighting.azimuth);
			block.writeSingle(LevelLighting.bias);
			block.writeSingle(LevelLighting.fade);
			block.writeSingle(LevelLighting.time);
			block.writeByte(LevelLighting.moon);
			block.writeSingle(LevelLighting.seaLevel);
			block.writeSingle(LevelLighting.snowLevel);
			block.writeBoolean(LevelLighting.canRain);
			block.writeBoolean(LevelLighting.canSnow);
			block.writeSingle(LevelLighting.rainFreq);
			block.writeSingle(LevelLighting.rainDur);
			block.writeSingle(LevelLighting.snowFreq);
			block.writeSingle(LevelLighting.snowDur);
			for (int i = 0; i < LevelLighting.times.Length; i++)
			{
				LightingInfo lightingInfo = LevelLighting.times[i];
				for (int j = 0; j < lightingInfo.colors.Length; j++)
				{
					block.writeColor(lightingInfo.colors[j]);
				}
				for (int k = 0; k < lightingInfo.singles.Length; k++)
				{
					block.writeSingle(lightingInfo.singles[k]);
				}
			}
			ReadWrite.writeBlock(Level.info.path + "/Environment/Lighting.dat", false, false, block);
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x000A0CD0 File Offset: 0x0009EED0
		private static void UpdateLegacyWaterTransform()
		{
			if (LevelLighting.legacyWater == null)
			{
				return;
			}
			if (LevelLighting.seaLevel < 0.99f)
			{
				float num = (float)Level.size * 2f;
				float num2 = LevelLighting.seaLevel * Level.TERRAIN;
				LevelLighting.legacyWaterTransform.position = new Vector3(0f, -512f + num2 * 0.5f, 0f);
				LevelLighting.legacyWaterTransform.localScale = new Vector3(num, 1024f + num2, num);
				LevelLighting.legacyWater.gameObject.SetActive(true);
				return;
			}
			LevelLighting.legacyWater.gameObject.SetActive(false);
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x000A0D70 File Offset: 0x0009EF70
		private static void UpdateBubblesActive()
		{
			bool flag = !Level.info.configData.Use_Legacy_Water || LevelLighting.seaLevel < 0.99f;
			if (!Level.info.configData.Use_Vanilla_Bubbles)
			{
				flag = false;
			}
			LevelLighting.bubbles.gameObject.SetActive(flag);
			if (flag)
			{
				LevelLighting.bubbles.GetComponent<ParticleSystem>().Play();
			}
		}

		/// <summary>
		/// Ticked on dedicated server as well as client so that server can listen for weather events.
		/// </summary>
		/// <param name="localVolumeMask">On dedicated server this is always 0xFFFFFFFF.</param>
		// Token: 0x060026FF RID: 9983 RVA: 0x000A0DD4 File Offset: 0x0009EFD4
		public static void tickCustomWeatherBlending(uint localVolumeMask)
		{
			int frameCount = Time.frameCount;
			if (frameCount == LevelLighting.tickedWeatherBlendingFrame)
			{
				return;
			}
			LevelLighting.tickedWeatherBlendingFrame = frameCount;
			float deltaTime = Time.deltaTime;
			for (int i = LevelLighting.customWeatherInstances.Count - 1; i >= 0; i--)
			{
				LevelLighting.CustomWeatherInstance customWeatherInstance = LevelLighting.customWeatherInstances[i];
				bool flag = (customWeatherInstance.asset.volumeMask & localVolumeMask) > 0U;
				if (!customWeatherInstance.component.hasTickedBlending)
				{
					customWeatherInstance.component.hasTickedBlending = true;
					customWeatherInstance.component.localVolumeBlendAlpha = (flag ? customWeatherInstance.component.globalBlendAlpha : 0f);
				}
				if (flag && customWeatherInstance.component.isWeatherActive)
				{
					customWeatherInstance.component.localVolumeBlendAlpha = Mathf.Min(1f, customWeatherInstance.component.localVolumeBlendAlpha + deltaTime / Mathf.Max(0.1f, customWeatherInstance.asset.fadeInDuration));
				}
				else
				{
					customWeatherInstance.component.localVolumeBlendAlpha = Mathf.Max(0f, customWeatherInstance.component.localVolumeBlendAlpha - deltaTime / Mathf.Max(0.1f, customWeatherInstance.asset.fadeOutDuration));
				}
				if (customWeatherInstance.component.isWeatherActive)
				{
					customWeatherInstance.component.globalBlendAlpha += deltaTime / Mathf.Max(0.1f, customWeatherInstance.asset.fadeInDuration);
					if (customWeatherInstance.component.globalBlendAlpha >= 1f)
					{
						customWeatherInstance.component.globalBlendAlpha = 1f;
						if (!customWeatherInstance.component.isFullyTransitionedIn)
						{
							customWeatherInstance.component.isFullyTransitionedIn = true;
							WeatherEventListenerManager.InvokeEndTransitionIn(customWeatherInstance.asset.GUID);
							WeatherEventListenerManager.InvokeStatusChange(customWeatherInstance.component.asset, EWeatherStatusChange.EndTransitionIn);
							customWeatherInstance.component.OnEndTransitionIn();
							customWeatherInstance.eventBlendAlpha = 1f;
							WeatherEventListenerManager.InvokeBlendAlphaChanged(customWeatherInstance.component.asset, 1f);
						}
					}
					else if (customWeatherInstance.component.globalBlendAlpha - customWeatherInstance.eventBlendAlpha >= 0.01f)
					{
						customWeatherInstance.eventBlendAlpha = customWeatherInstance.component.globalBlendAlpha;
						WeatherEventListenerManager.InvokeBlendAlphaChanged(customWeatherInstance.asset, customWeatherInstance.component.globalBlendAlpha);
					}
				}
				else
				{
					customWeatherInstance.component.isFullyTransitionedIn = false;
					customWeatherInstance.component.globalBlendAlpha -= deltaTime / Mathf.Max(0.1f, customWeatherInstance.asset.fadeOutDuration);
					if (customWeatherInstance.component.globalBlendAlpha <= 0f)
					{
						customWeatherInstance.component.globalBlendAlpha = 0f;
						WeatherEventListenerManager.InvokeEndTransitionOut(customWeatherInstance.component.asset.GUID);
						WeatherEventListenerManager.InvokeStatusChange(customWeatherInstance.component.asset, EWeatherStatusChange.EndTransitionOut);
						customWeatherInstance.component.OnEndTransitionOut();
						customWeatherInstance.eventBlendAlpha = 0f;
						WeatherEventListenerManager.InvokeBlendAlphaChanged(customWeatherInstance.component.asset, 0f);
						customWeatherInstance.teardown();
						LevelLighting.customWeatherInstances.RemoveAtFast(i);
					}
					else if (customWeatherInstance.eventBlendAlpha - customWeatherInstance.component.globalBlendAlpha >= 0.01f)
					{
						customWeatherInstance.eventBlendAlpha = customWeatherInstance.component.globalBlendAlpha;
						WeatherEventListenerManager.InvokeBlendAlphaChanged(customWeatherInstance.asset, customWeatherInstance.component.globalBlendAlpha);
					}
				}
			}
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x000A10FF File Offset: 0x0009F2FF
		public static void updateLocal()
		{
			LevelLighting.updateLocal(LevelLighting.localPoint, LevelLighting.localWindOverride, LevelLighting.localEffectNode);
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000A1118 File Offset: 0x0009F318
		public static void updateLocal(Vector3 point, float windOverride, IAmbianceNode effectNode)
		{
			LevelLighting.localPoint = point;
			LevelLighting.localWindOverride = windOverride;
			if (effectNode != LevelLighting.localEffectNode)
			{
				if (effectNode != null)
				{
					EffectAsset effectAsset = effectNode.GetEffectAsset();
					if (LevelLighting.localEffectNode == null || effectAsset != LevelLighting.currentEffectAsset)
					{
						LevelLighting.currentEffectAsset = effectAsset;
						if (effectAsset != null && effectAsset.effect != null)
						{
							AudioSource component = effectAsset.effect.GetComponent<AudioSource>();
							if (component != null)
							{
								if (!effectAsset.isMusic || OptionsSettings.ambientMusicVolume > 0f)
								{
									LevelLighting.effectAudio.clip = component.clip;
									LevelLighting.effectAudio.Play();
									LevelLighting.localPlayingEffect = true;
								}
								else
								{
									LevelLighting.localPlayingEffect = false;
								}
							}
							else
							{
								LevelLighting.localPlayingEffect = false;
							}
						}
						else
						{
							LevelLighting.localPlayingEffect = false;
						}
					}
				}
				else
				{
					LevelLighting.localPlayingEffect = false;
				}
			}
			LevelLighting.localEffectNode = effectNode;
			if (LevelLighting.localEffectNode != null && LevelLighting.localEffectNode.noLighting && !Level.isEditor)
			{
				LevelLighting.localLightingBlend = Mathf.Lerp(LevelLighting.localLightingBlend, 1f, 0.25f * Time.deltaTime);
				LevelLighting.localBlendingLight = true;
			}
			else
			{
				LevelLighting.localLightingBlend = Mathf.Lerp(LevelLighting.localLightingBlend, 0f, 0.25f * Time.deltaTime);
				if (LevelLighting.localLightingBlend < 0.01f)
				{
					LevelLighting.localLightingBlend = 0f;
					LevelLighting.localBlendingLight = false;
				}
			}
			AmbianceVolume ambianceVolume = LevelLighting.localEffectNode as AmbianceVolume;
			if (ambianceVolume != null && ambianceVolume.overrideFog)
			{
				LevelLighting.localFogBlend = Mathf.Lerp(LevelLighting.localFogBlend, 1f, 0.05f * Time.deltaTime);
				LevelLighting.localBlendingFog = true;
				LevelLighting.localFogColor = ambianceVolume.fogColor;
				LevelLighting.localFogIntensity = ambianceVolume.fogIntensity;
				LevelLighting.localAtmosphericFog = (ambianceVolume.overrideAtmosphericFog ? ambianceVolume.fogIntensity : 0f);
			}
			else
			{
				LevelLighting.localFogBlend = Mathf.Lerp(LevelLighting.localFogBlend, 0f, 0.125f * Time.deltaTime);
				if (LevelLighting.localFogBlend < 0.01f)
				{
					LevelLighting.localFogBlend = 0f;
					LevelLighting.localBlendingFog = false;
				}
			}
			uint num;
			if (ambianceVolume != null)
			{
				num = ambianceVolume.weatherMask;
			}
			else
			{
				LevelAsset asset = Level.getAsset();
				num = ((asset != null) ? asset.globalWeatherMask : uint.MaxValue);
			}
			if (Level.info != null && Level.info.configData != null)
			{
				if (!Level.info.configData.Use_Rain_Volumes)
				{
					num |= 1U;
				}
				if (!Level.info.configData.Use_Snow_Volumes)
				{
					num |= 2U;
				}
				if (Level.info.configData.Use_Legacy_Snow_Height)
				{
					if (LevelLighting.isPositionSnowy(point))
					{
						num |= 2U;
					}
					else
					{
						num &= 4294967293U;
					}
				}
			}
			LevelLighting.tickCustomWeatherBlending(num);
			if (!LevelLighting.init)
			{
				LevelLighting.init = true;
				LevelLighting.resetCachedValues();
				LevelLighting.updateLighting();
				LevelLighting.bubbles.GetComponent<ParticleSystem>().Play();
				if (LevelLighting.dayAudio.clip != null)
				{
					LevelLighting.dayAudio.Play();
				}
				if (LevelLighting.nightAudio.clip != null)
				{
					LevelLighting.nightAudio.Play();
				}
				if (LevelLighting.waterAudio.clip != null)
				{
					LevelLighting.waterAudio.Play();
				}
				if (LevelLighting.windAudio.clip != null)
				{
					LevelLighting.windAudio.Play();
				}
				if (LevelLighting.belowAudio.clip != null)
				{
					LevelLighting.belowAudio.Play();
				}
			}
			LevelLighting.lighting.position = point;
			LevelLighting.setSkyColor(LevelLighting.skyboxSky);
			LevelLighting.setEquatorColor(LevelLighting.skyboxEquator);
			LevelLighting.setGroundColor(LevelLighting.skyboxGround);
			float num2 = WaterUtility.getWaterSurfaceElevation(point);
			if (!LevelLighting.enableUnderwaterEffects)
			{
				num2 = -1024f;
			}
			if (LevelLighting.enableUnderwaterEffects && WaterUtility.isPointUnderwater(point))
			{
				LevelLighting.waterAudio.volume = 0f;
				LevelLighting.belowAudio.volume = 1f;
				LevelLighting.isSea = true;
			}
			else
			{
				if (point.y < num2 + 8f && (LevelLighting.localEffectNode == null || !LevelLighting.localEffectNode.noWater))
				{
					LevelLighting.waterAudio.volume = Mathf.Lerp(0f, 0.25f, 1f - (point.y - num2) / 8f);
					LevelLighting.belowAudio.volume = 0f;
				}
				else
				{
					LevelLighting.waterAudio.volume = 0f;
					LevelLighting.belowAudio.volume = 0f;
				}
				LevelLighting.isSea = false;
			}
			if (LevelLighting.isSea)
			{
				RenderSettings.fogColor = LevelLighting.getSeaColor("_BaseColor");
				RenderSettings.fogDensity = 0.075f;
				LevelLighting.setAtmosphericFog(1f);
			}
			else
			{
				RenderSettings.fogColor = LevelLighting.levelFogColor;
				RenderSettings.fogDensity = Mathf.Pow(LevelLighting.levelFogIntensity, 3f) * 0.025f;
				LevelLighting.setAtmosphericFog(LevelLighting.levelAtmosphericFog);
			}
			LevelLighting.auroraBorealisCurrentIntensity = Mathf.Clamp01(Mathf.Lerp(LevelLighting.auroraBorealisCurrentIntensity, LevelLighting.auroraBorealisTargetIntensity, 0.1f * Time.deltaTime));
			LevelLighting.skybox.SetFloat("_AuroraBorealisIntensity", LevelLighting.auroraBorealisCurrentIntensity);
			LevelLighting.setAlphaParticleLightingColor(LevelLighting.particleLightingColor);
			if (LevelLighting.isSea)
			{
				Color fogColor = RenderSettings.fogColor;
				LevelLighting.setSkyColor(fogColor);
				LevelLighting.setEquatorColor(fogColor);
				LevelLighting.setGroundColor(fogColor);
			}
			if (LevelLighting.puddles != null)
			{
				float num3 = 0f;
				float num4 = 0f;
				foreach (LevelLighting.CustomWeatherInstance customWeatherInstance in LevelLighting.customWeatherInstances)
				{
					num3 = Mathf.Max(num3, customWeatherInstance.component.puddleWaterLevel * customWeatherInstance.component.EffectBlendAlpha);
					num4 = Mathf.Max(num4, customWeatherInstance.component.puddleIntensity * customWeatherInstance.component.EffectBlendAlpha);
				}
				if (num3 > LevelLighting.puddles.Water_Level)
				{
					LevelLighting.puddles.Water_Level = Mathf.Lerp(LevelLighting.puddles.Water_Level, num3, 0.2f * Time.deltaTime);
				}
				else
				{
					LevelLighting.puddles.Water_Level = Mathf.Lerp(LevelLighting.puddles.Water_Level, num3, 0.025f * Time.deltaTime);
				}
				LevelLighting.puddles.Intensity = num4;
			}
			if (Time.time > LevelLighting.nextAudioVolumeChangeTime)
			{
				LevelLighting.nextAudioVolumeChangeTime = Time.time + (float)Random.Range(15, 60);
				LevelLighting.targetAudioVolume = Random.Range(LevelLighting.AUDIO_MIN, LevelLighting.AUDIO_MAX);
			}
			LevelLighting.currentAudioVolume = Mathf.Lerp(LevelLighting.currentAudioVolume, LevelLighting.targetAudioVolume, 0.1f * Time.deltaTime);
			float b;
			if (LevelLighting.localPlayingEffect)
			{
				if (LevelLighting.currentEffectAsset != null && LevelLighting.currentEffectAsset.isMusic)
				{
					b = OptionsSettings.ambientMusicVolume;
				}
				else
				{
					b = 1f;
				}
			}
			else
			{
				b = 0f;
			}
			LevelLighting.effectAudio.volume = Mathf.Lerp(LevelLighting.effectAudio.volume, b, Level.isEditor ? 1f : (0.5f * Time.deltaTime));
			float volume = LevelLighting.effectAudio.volume;
			float num5 = 0f;
			float b2 = 0.15f;
			foreach (LevelLighting.CustomWeatherInstance customWeatherInstance2 in LevelLighting.customWeatherInstances)
			{
				customWeatherInstance2.component.UpdateWeather();
			}
			float num6 = 1f - num5;
			LevelLighting.windAudio.volume = windOverride;
			LevelLighting.dayAudio.volume = Mathf.Lerp(LevelLighting.dayAudio.volume, LevelLighting.dayVolume * LevelLighting.currentAudioVolume * (1f - LevelLighting.waterAudio.volume * 4f) * (1f - LevelLighting.belowAudio.volume) * (1f - LevelLighting.windAudio.volume) * (1f - LevelLighting.effectAudio.volume) * num6, 0.5f * Time.deltaTime);
			LevelLighting.nightAudio.volume = Mathf.Lerp(LevelLighting.nightAudio.volume, LevelLighting.nightVolume * LevelLighting.currentAudioVolume * (1f - LevelLighting.waterAudio.volume * 4f) * (1f - LevelLighting.belowAudio.volume) * (1f - LevelLighting.windAudio.volume) * (1f - LevelLighting.effectAudio.volume) * num6, 0.5f * Time.deltaTime);
			LevelLighting.windZone.transform.rotation = Quaternion.Slerp(LevelLighting.windZone.transform.rotation, Quaternion.Euler(0f, LevelLighting.wind, 0f), 0.5f * Time.deltaTime);
			LevelLighting.windZone.windMain = Mathf.Lerp(LevelLighting.windZone.windMain, b2, 0.5f * Time.deltaTime);
			point.y = Mathf.Min(point.y - 16f, num2 - 32f);
			LevelLighting.bubbles.position = point;
			if (LevelLighting.skyboxNeedsColorUpdate)
			{
				LevelLighting.updateSkyboxColors();
			}
			if (LevelLighting.skyboxNeedsReflectionUpdate)
			{
				LevelLighting.lastSkyboxReflectionUpdate = Time.time;
				LevelLighting.skyboxNeedsReflectionUpdate = false;
				if (LevelLighting.vision != ELightingVision.CIVILIAN && LevelLighting.vision != ELightingVision.MILITARY && !LevelLighting.localBlendingLight)
				{
					if (Provider.preferenceData != null && Provider.preferenceData.Graphics.Use_Skybox_Ambience)
					{
						RenderSettings.ambientMode = AmbientMode.Skybox;
						DynamicGI.UpdateEnvironment();
					}
					else
					{
						RenderSettings.ambientMode = AmbientMode.Trilight;
					}
				}
				LevelLighting.isReflectionBuilding = true;
				LevelLighting.isReflectionBuildingVision = true;
			}
			else if (Time.time - LevelLighting.lastSkyboxReflectionUpdate > 3f)
			{
				LevelLighting.skyboxNeedsReflectionUpdate = true;
			}
			LevelLighting.updateSkyboxReflections();
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000A1A58 File Offset: 0x0009FC58
		private static void renderSkyboxReflection(Cubemap target, ref int index, ref bool isBuilding)
		{
			if (!isBuilding)
			{
				return;
			}
			if (target == null || LevelLighting.reflectionCamera == null)
			{
				return;
			}
			int faceMask = 1 << index;
			index++;
			if (index > 5)
			{
				index = 0;
				isBuilding = false;
			}
			LevelLighting.reflectionCamera.RenderToCubemap(target, faceMask);
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x000A1AA8 File Offset: 0x0009FCA8
		public static void updateSkyboxReflections()
		{
			if (!LevelLighting.isSkyboxReflectionEnabled)
			{
				RenderSettings.customReflection = null;
				return;
			}
			if (LevelLighting.vision == ELightingVision.NONE)
			{
				LevelLighting.renderSkyboxReflection(LevelLighting.reflectionMap, ref LevelLighting.reflectionIndex, ref LevelLighting.isReflectionBuilding);
				RenderSettings.customReflection = LevelLighting.reflectionMap;
				return;
			}
			LevelLighting.renderSkyboxReflection(LevelLighting.reflectionMapVision, ref LevelLighting.reflectionIndexVision, ref LevelLighting.isReflectionBuildingVision);
			RenderSettings.customReflection = LevelLighting.reflectionMapVision;
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000A1B08 File Offset: 0x0009FD08
		private static void setAtmosphericFog(float newFog)
		{
			if (!MathfEx.IsNearlyEqual(LevelLighting.cachedAtmosphericFog, newFog, 0.001f))
			{
				LevelLighting.cachedAtmosphericFog = newFog;
				Shader.SetGlobalFloat("_AtmosphericFog", newFog);
			}
			if (MainCamera.instance != null)
			{
				SunShaftsCs component = MainCamera.instance.GetComponent<SunShaftsCs>();
				if (component != null)
				{
					component.sunShaftIntensity = 1f - newFog;
				}
			}
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000A1B66 File Offset: 0x0009FD66
		private static void setAlphaParticleLightingColor(Color newColor)
		{
			if (!MathfEx.IsNearlyEqual(newColor, LevelLighting.cachedAlphaParticleLightingColor, 0.002f))
			{
				LevelLighting.cachedAlphaParticleLightingColor = newColor;
				Shader.SetGlobalColor("_AlphaParticleLightingColor", newColor);
			}
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000A1B8B File Offset: 0x0009FD8B
		private static void setSkyColor(Color skyColor)
		{
			if (!MathfEx.IsNearlyEqual(skyColor, LevelLighting.cachedSkyColor, 0.002f))
			{
				LevelLighting.cachedSkyColor = skyColor;
				LevelLighting.skyboxNeedsColorUpdate = true;
			}
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000A1BAB File Offset: 0x0009FDAB
		private static void setEquatorColor(Color equatorColor)
		{
			if (!MathfEx.IsNearlyEqual(equatorColor, LevelLighting.cachedEquatorColor, 0.002f))
			{
				LevelLighting.cachedEquatorColor = equatorColor;
				LevelLighting.skyboxNeedsColorUpdate = true;
			}
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000A1BCB File Offset: 0x0009FDCB
		private static void setGroundColor(Color groundColor)
		{
			if (!MathfEx.IsNearlyEqual(groundColor, LevelLighting.cachedGroundColor, 0.002f))
			{
				LevelLighting.cachedGroundColor = groundColor;
				LevelLighting.skyboxNeedsColorUpdate = true;
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000A1BEC File Offset: 0x0009FDEC
		private static void updateSkyboxColors()
		{
			LevelLighting.skyboxNeedsColorUpdate = false;
			LevelLighting.skybox.SetColor("_SkyColor", LevelLighting.cachedSkyColor);
			LevelLighting.skybox.SetColor("_EquatorColor", LevelLighting.cachedEquatorColor);
			LevelLighting.skybox.SetColor("_GroundColor", LevelLighting.cachedGroundColor);
			LevelLighting.setSeaColor("_SkyColor", LevelLighting.cachedSkyColor);
			LevelLighting.setSeaColor("_EquatorColor", LevelLighting.cachedEquatorColor);
			LevelLighting.setSeaColor("_GroundColor", LevelLighting.cachedGroundColor);
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000A1C68 File Offset: 0x0009FE68
		private static void resetCachedValues()
		{
			LevelLighting.cachedAtmosphericFog = -1f;
			LevelLighting.cachedAlphaParticleLightingColor = new Color(-1f, -1f, -1f);
			LevelLighting.cachedSkyColor = new Color(-1f, -1f, -1f);
			LevelLighting.cachedEquatorColor = new Color(-1f, -1f, -1f);
			LevelLighting.cachedGroundColor = new Color(-1f, -1f, -1f);
		}

		/// <summary>
		/// Reset any global shader properties that may affect the main menu.
		/// </summary>
		// Token: 0x0600270B RID: 9995 RVA: 0x000A1CE3 File Offset: 0x0009FEE3
		public static void resetForMainMenu()
		{
			LevelLighting.setAtmosphericFog(0f);
			LevelLighting.setAlphaParticleLightingColor(Color.white);
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000A1CFC File Offset: 0x0009FEFC
		static LevelLighting()
		{
			bool editorWantsUnderwaterEffects;
			if (ConvenientSavedata.get().read("EditorWantsUnderwaterEffects", out editorWantsUnderwaterEffects))
			{
				LevelLighting._editorWantsUnderwaterEffects = editorWantsUnderwaterEffects;
			}
			bool editorWantsWaterSurface;
			if (ConvenientSavedata.get().read("EditorWantsWaterSurfaceVisible", out editorWantsWaterSurface))
			{
				LevelLighting._editorWantsWaterSurface = editorWantsWaterSurface;
			}
		}

		// Token: 0x04001445 RID: 5189
		private static bool _editorWantsUnderwaterEffects = true;

		// Token: 0x04001446 RID: 5190
		private static bool _editorWantsWaterSurface = true;

		// Token: 0x04001447 RID: 5191
		public static readonly byte SAVEDATA_VERSION = 12;

		// Token: 0x04001448 RID: 5192
		public static readonly byte MOON_CYCLES = 5;

		// Token: 0x04001449 RID: 5193
		[Obsolete("Never used?")]
		public static readonly float CLOUDS = 2f;

		// Token: 0x0400144A RID: 5194
		public static readonly float AUDIO_MIN = 0.075f;

		// Token: 0x0400144B RID: 5195
		public static readonly float AUDIO_MAX = 0.15f;

		// Token: 0x0400144C RID: 5196
		private static readonly Color FOAM_DAWN = new Color(0.125f, 0f, 0f, 0f);

		// Token: 0x0400144D RID: 5197
		private static readonly Color FOAM_MIDDAY = new Color(0.25f, 0f, 0f, 0f);

		// Token: 0x0400144E RID: 5198
		private static readonly Color FOAM_DUSK = new Color(0.05f, 0f, 0f, 0f);

		// Token: 0x0400144F RID: 5199
		private static readonly Color FOAM_MIDNIGHT = new Color(0.01f, 0f, 0f, 0f);

		// Token: 0x04001450 RID: 5200
		private static readonly float SPECULAR_DAWN = 5f;

		// Token: 0x04001451 RID: 5201
		private static readonly float SPECULAR_MIDDAY = 50f;

		// Token: 0x04001452 RID: 5202
		private static readonly float SPECULAR_DUSK = 5f;

		// Token: 0x04001453 RID: 5203
		private static readonly float SPECULAR_MIDNIGHT = 50f;

		// Token: 0x04001454 RID: 5204
		private static readonly float PITCH_DARK_WATER_BLEND = 0.9f;

		// Token: 0x04001455 RID: 5205
		private static readonly float REFLECTION_DAWN = 0.75f;

		// Token: 0x04001456 RID: 5206
		private static readonly float REFLECTION_MIDDAY = 0.75f;

		// Token: 0x04001457 RID: 5207
		private static readonly float REFLECTION_DUSK = 0.5f;

		// Token: 0x04001458 RID: 5208
		private static readonly float REFLECTION_MIDNIGHT = 0.5f;

		// Token: 0x04001459 RID: 5209
		internal static readonly Color NIGHTVISION_MILITARY = new Color32(20, 120, 80, 0);

		// Token: 0x0400145A RID: 5210
		internal static readonly Color NIGHTVISION_CIVILIAN = new Color(0.4f, 0.4f, 0.4f, 0f);

		// Token: 0x0400145B RID: 5211
		private static float _azimuth;

		// Token: 0x0400145C RID: 5212
		private static float _transition;

		// Token: 0x0400145D RID: 5213
		private static float _bias;

		// Token: 0x0400145E RID: 5214
		private static float _fade;

		// Token: 0x0400145F RID: 5215
		private static float _time;

		// Token: 0x04001460 RID: 5216
		private static float _wind;

		/// <summary>
		/// Kept for backwards compatibility with mod hooks, plugins, and events.
		/// </summary>
		// Token: 0x04001461 RID: 5217
		public static ELightingRain rainyness;

		/// <summary>
		/// Kept for backwards compatibility with mod hooks, plugins, and events.
		/// </summary>
		// Token: 0x04001462 RID: 5218
		public static ELightingSnow snowyness;

		// Token: 0x04001463 RID: 5219
		private static List<LevelLighting.CustomWeatherInstance> customWeatherInstances = new List<LevelLighting.CustomWeatherInstance>();

		// Token: 0x04001464 RID: 5220
		private static LevelLighting.CustomWeatherInstance activeCustomWeather = null;

		// Token: 0x0400146A RID: 5226
		private static LightingInfo[] _times;

		// Token: 0x0400146B RID: 5227
		private static float _seaLevel;

		// Token: 0x0400146C RID: 5228
		private static float _snowLevel;

		// Token: 0x0400146D RID: 5229
		public static float rainFreq;

		// Token: 0x0400146E RID: 5230
		public static float rainDur;

		// Token: 0x0400146F RID: 5231
		public static float snowFreq;

		// Token: 0x04001470 RID: 5232
		public static float snowDur;

		// Token: 0x04001471 RID: 5233
		public static bool canRain;

		// Token: 0x04001472 RID: 5234
		public static bool canSnow;

		// Token: 0x04001473 RID: 5235
		private static ELightingVision _vision;

		// Token: 0x04001474 RID: 5236
		public static Color nightvisionColor;

		// Token: 0x04001475 RID: 5237
		public static float nightvisionFogIntensity;

		// Token: 0x04001477 RID: 5239
		protected static bool _isSea;

		// Token: 0x04001478 RID: 5240
		private static Material skybox;

		// Token: 0x04001479 RID: 5241
		private static Transform lighting;

		// Token: 0x0400147A RID: 5242
		private static Rain puddles;

		// Token: 0x0400147B RID: 5243
		private static float auroraBorealisCurrentIntensity;

		// Token: 0x0400147C RID: 5244
		private static float auroraBorealisTargetIntensity;

		// Token: 0x0400147F RID: 5247
		private static Color skyboxGround;

		// Token: 0x04001480 RID: 5248
		private static Color cloudColor;

		// Token: 0x04001481 RID: 5249
		private static Color cloudRimColor;

		// Token: 0x04001482 RID: 5250
		private static bool skyboxNeedsReflectionUpdate;

		// Token: 0x04001483 RID: 5251
		private static float lastSkyboxReflectionUpdate;

		// Token: 0x04001484 RID: 5252
		private static Color particleLightingColor;

		// Token: 0x04001485 RID: 5253
		private static Color raysColor;

		// Token: 0x04001486 RID: 5254
		private static float raysIntensity;

		/// <summary>
		/// Level designed target fog color.
		/// </summary>
		// Token: 0x04001487 RID: 5255
		private static Color levelFogColor;

		/// <summary>
		/// Level designed target fog intensity.
		/// </summary>
		// Token: 0x04001488 RID: 5256
		private static float levelFogIntensity;

		/// <summary>
		/// Level designed target atmospheric fog intensity.
		/// </summary>
		// Token: 0x04001489 RID: 5257
		private static float levelAtmosphericFog;

		// Token: 0x0400148A RID: 5258
		public static Transform sun;

		// Token: 0x0400148B RID: 5259
		private static Light sunLight;

		// Token: 0x0400148C RID: 5260
		private static Transform sunFlare;

		// Token: 0x0400148D RID: 5261
		private static AudioSource _effectAudio;

		// Token: 0x0400148E RID: 5262
		private static AudioSource _dayAudio;

		// Token: 0x0400148F RID: 5263
		private static AudioSource _nightAudio;

		// Token: 0x04001490 RID: 5264
		private static AudioSource _waterAudio;

		// Token: 0x04001491 RID: 5265
		private static AudioSource _windAudio;

		// Token: 0x04001492 RID: 5266
		private static AudioSource _belowAudio;

		// Token: 0x04001493 RID: 5267
		private static float currentAudioVolume;

		// Token: 0x04001494 RID: 5268
		private static float targetAudioVolume;

		// Token: 0x04001495 RID: 5269
		private static float nextAudioVolumeChangeTime;

		// Token: 0x04001496 RID: 5270
		private static float dayVolume;

		// Token: 0x04001497 RID: 5271
		private static float nightVolume;

		// Token: 0x04001498 RID: 5272
		private static Camera reflectionCamera;

		// Token: 0x04001499 RID: 5273
		private static Cubemap reflectionMap;

		// Token: 0x0400149A RID: 5274
		private static Cubemap reflectionMapVision;

		// Token: 0x0400149B RID: 5275
		private static int reflectionIndex;

		// Token: 0x0400149C RID: 5276
		private static int reflectionIndexVision;

		// Token: 0x0400149D RID: 5277
		private static bool isReflectionBuilding;

		// Token: 0x0400149E RID: 5278
		private static bool isReflectionBuildingVision;

		// Token: 0x0400149F RID: 5279
		private static bool _isSkyboxReflectionEnabled;

		// Token: 0x040014A0 RID: 5280
		private static Transform _bubbles;

		// Token: 0x040014A1 RID: 5281
		private static WindZone _windZone;

		// Token: 0x040014A2 RID: 5282
		private static WaterVolume legacyWater;

		// Token: 0x040014A3 RID: 5283
		private static Transform legacyWaterTransform;

		// Token: 0x040014A4 RID: 5284
		private static Transform[] moons;

		// Token: 0x040014A5 RID: 5285
		private static byte _moon;

		// Token: 0x040014A6 RID: 5286
		private static bool init;

		// Token: 0x040014A7 RID: 5287
		private static Vector3 localPoint;

		// Token: 0x040014A8 RID: 5288
		private static float localWindOverride;

		// Token: 0x040014A9 RID: 5289
		private static IAmbianceNode localEffectNode;

		// Token: 0x040014AA RID: 5290
		private static EffectAsset currentEffectAsset;

		// Token: 0x040014AB RID: 5291
		private static bool localPlayingEffect;

		// Token: 0x040014AC RID: 5292
		private static bool localBlendingLight;

		// Token: 0x040014AD RID: 5293
		private static float localLightingBlend;

		// Token: 0x040014AE RID: 5294
		private static bool localBlendingFog;

		// Token: 0x040014AF RID: 5295
		private static float localFogBlend;

		// Token: 0x040014B0 RID: 5296
		private static Color localFogColor;

		// Token: 0x040014B1 RID: 5297
		private static float localFogIntensity;

		// Token: 0x040014B2 RID: 5298
		private static float localAtmosphericFog;

		// Token: 0x040014B3 RID: 5299
		private static int tickedWeatherBlendingFrame;

		// Token: 0x040014B4 RID: 5300
		private static float cachedAtmosphericFog;

		// Token: 0x040014B5 RID: 5301
		private static Color cachedAlphaParticleLightingColor;

		// Token: 0x040014B6 RID: 5302
		internal static Color cachedSkyColor;

		// Token: 0x040014B7 RID: 5303
		internal static Color cachedEquatorColor;

		// Token: 0x040014B8 RID: 5304
		internal static Color cachedGroundColor;

		// Token: 0x040014B9 RID: 5305
		private static bool skyboxNeedsColorUpdate = false;

		// Token: 0x02000959 RID: 2393
		private class CustomWeatherInstance
		{
			// Token: 0x06004B0D RID: 19213 RVA: 0x001B30D0 File Offset: 0x001B12D0
			public void initialize()
			{
				this.gameObject = new GameObject(this.asset.name);
				this.gameObject.transform.parent = LevelLighting.lighting;
				this.gameObject.transform.localPosition = Vector3.zero;
				this.component = (this.gameObject.AddComponent(this.asset.componentType) as WeatherComponentBase);
				this.component.asset = this.asset;
				this.component.netId = this.netId;
				if (!this.netId.IsNull())
				{
					NetIdRegistry.Assign(this.netId, this.component);
				}
				this.component.InitializeWeather();
				if (this.asset.hasLightning && !this.netId.IsNull())
				{
					LightningWeatherComponent lightningWeatherComponent = this.gameObject.AddComponent<LightningWeatherComponent>();
					lightningWeatherComponent.weatherComponent = this.component;
					lightningWeatherComponent.netId = this.netId + 1U;
					NetIdRegistry.Assign(lightningWeatherComponent.netId, lightningWeatherComponent);
				}
			}

			// Token: 0x06004B0E RID: 19214 RVA: 0x001B31DC File Offset: 0x001B13DC
			public void teardown()
			{
				if (this.component != null)
				{
					this.component.PreDestroyWeather();
					this.component = null;
				}
				if (this.gameObject != null)
				{
					Object.Destroy(this.gameObject);
					this.gameObject = null;
				}
				if (!this.netId.IsNull())
				{
					NetIdRegistry.Release(this.netId);
					this.netId.Clear();
				}
			}

			// Token: 0x0400332F RID: 13103
			public WeatherAssetBase asset;

			// Token: 0x04003330 RID: 13104
			public NetId netId;

			/// <summary>
			/// [0, 1] used to avoid invoking BlendAlphaChanged every frame.
			/// Compared against globalBlendAlpha not taking into account local volume.
			/// </summary>
			// Token: 0x04003331 RID: 13105
			public float eventBlendAlpha;

			// Token: 0x04003332 RID: 13106
			public GameObject gameObject;

			// Token: 0x04003333 RID: 13107
			public WeatherComponentBase component;
		}

		// Token: 0x0200095A RID: 2394
		// (Invoke) Token: 0x06004B11 RID: 19217
		public delegate void IsSeaChangedHandler(bool isSea);
	}
}
