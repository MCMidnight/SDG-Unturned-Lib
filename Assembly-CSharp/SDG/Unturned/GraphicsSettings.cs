using System;
using System.Collections.Generic;
using HighlightingSystem;
using SDG.Framework.Foliage;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace SDG.Unturned
{
	// Token: 0x020006D0 RID: 1744
	public class GraphicsSettings
	{
		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06003A1B RID: 14875 RVA: 0x00110568 File Offset: 0x0010E768
		// (set) Token: 0x06003A1C RID: 14876 RVA: 0x00110570 File Offset: 0x0010E770
		public static bool uncapLandmarks
		{
			get
			{
				return GraphicsSettings._uncapLandmarks;
			}
			set
			{
				GraphicsSettings._uncapLandmarks = value;
				GraphicsSettings.apply("changed uncapLandmarks");
				UnturnedLog.info("Set uncap_landmarks to: " + GraphicsSettings.uncapLandmarks.ToString());
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003A1D RID: 14877 RVA: 0x001105AC File Offset: 0x0010E7AC
		public static float effect
		{
			get
			{
				if (GraphicsSettings.effectQuality == EGraphicQuality.ULTRA)
				{
					return Random.Range(48f, 80f);
				}
				if (GraphicsSettings.effectQuality == EGraphicQuality.HIGH)
				{
					return Random.Range(40f, 56f);
				}
				if (GraphicsSettings.effectQuality == EGraphicQuality.MEDIUM)
				{
					return Random.Range(28f, 36f);
				}
				if (GraphicsSettings.effectQuality == EGraphicQuality.LOW)
				{
					return Random.Range(14f, 18f);
				}
				return 0f;
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06003A1E RID: 14878 RVA: 0x0011061E File Offset: 0x0010E81E
		// (set) Token: 0x06003A1F RID: 14879 RVA: 0x0011062A File Offset: 0x0010E82A
		public static GraphicsSettingsResolution resolution
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.Resolution;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.Resolution = value;
				GraphicsSettings.changeResolution = true;
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06003A20 RID: 14880 RVA: 0x0011063D File Offset: 0x0010E83D
		// (set) Token: 0x06003A21 RID: 14881 RVA: 0x00110649 File Offset: 0x0010E849
		public static FullScreenMode fullscreenMode
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.FullscreenMode;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.FullscreenMode = value;
				GraphicsSettings.changeResolution = true;
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06003A22 RID: 14882 RVA: 0x0011065C File Offset: 0x0010E85C
		// (set) Token: 0x06003A23 RID: 14883 RVA: 0x00110668 File Offset: 0x0010E868
		public static bool buffer
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsVSyncEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsVSyncEnabled = value;
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06003A24 RID: 14884 RVA: 0x00110678 File Offset: 0x0010E878
		// (set) Token: 0x06003A25 RID: 14885 RVA: 0x00110724 File Offset: 0x0010E924
		public static float userInterfaceScale
		{
			get
			{
				if (!GraphicsSettings.didCacheUIScaleOverride)
				{
					GraphicsSettings.didCacheUIScaleOverride = true;
					if (GraphicsSettings.uiScale.hasValue && GraphicsSettings.uiScale.value.IsFinite())
					{
						GraphicsSettings.cachedUIScaleOverride = new float?(GraphicsSettings.uiScale.value);
					}
					else if (Provider.preferenceData != null && Provider.preferenceData.Graphics != null)
					{
						float override_UI_Scale = Provider.preferenceData.Graphics.Override_UI_Scale;
						if (override_UI_Scale.IsFinite() && override_UI_Scale > 0f)
						{
							GraphicsSettings.cachedUIScaleOverride = new float?(override_UI_Scale);
						}
					}
				}
				if (GraphicsSettings.cachedUIScaleOverride != null)
				{
					return GraphicsSettings.cachedUIScaleOverride.Value;
				}
				return GraphicsSettings.graphicsSettingsData.UserInterfaceScale;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.UserInterfaceScale = value;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06003A26 RID: 14886 RVA: 0x00110731 File Offset: 0x0010E931
		// (set) Token: 0x06003A27 RID: 14887 RVA: 0x0011073D File Offset: 0x0010E93D
		public static bool UseTargetFrameRate
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.UseTargetFrameRate;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.UseTargetFrameRate = value;
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x0011074A File Offset: 0x0010E94A
		// (set) Token: 0x06003A29 RID: 14889 RVA: 0x00110756 File Offset: 0x0010E956
		public static int TargetFrameRate
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.TargetFrameRate;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.TargetFrameRate = value;
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06003A2A RID: 14890 RVA: 0x00110763 File Offset: 0x0010E963
		// (set) Token: 0x06003A2B RID: 14891 RVA: 0x0011076F File Offset: 0x0010E96F
		public static bool UseUnfocusedTargetFrameRate
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.UseUnfocusedTargetFrameRate;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.UseUnfocusedTargetFrameRate = value;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06003A2C RID: 14892 RVA: 0x0011077C File Offset: 0x0010E97C
		// (set) Token: 0x06003A2D RID: 14893 RVA: 0x00110788 File Offset: 0x0010E988
		public static int UnfocusedTargetFrameRate
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.UnfocusedTargetFrameRate;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.UnfocusedTargetFrameRate = value;
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06003A2E RID: 14894 RVA: 0x00110795 File Offset: 0x0010E995
		// (set) Token: 0x06003A2F RID: 14895 RVA: 0x001107A1 File Offset: 0x0010E9A1
		public static EAntiAliasingType antiAliasingType
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.AntiAliasingType5;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.AntiAliasingType5 = value;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06003A30 RID: 14896 RVA: 0x001107AE File Offset: 0x0010E9AE
		// (set) Token: 0x06003A31 RID: 14897 RVA: 0x001107BA File Offset: 0x0010E9BA
		public static EAnisotropicFilteringMode anisotropicFilteringMode
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.AnisotropicFilteringMode;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.AnisotropicFilteringMode = value;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06003A32 RID: 14898 RVA: 0x001107C7 File Offset: 0x0010E9C7
		// (set) Token: 0x06003A33 RID: 14899 RVA: 0x001107D3 File Offset: 0x0010E9D3
		public static bool isAmbientOcclusionEnabled
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsAmbientOcclusionEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsAmbientOcclusionEnabled = value;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06003A34 RID: 14900 RVA: 0x001107E0 File Offset: 0x0010E9E0
		// (set) Token: 0x06003A35 RID: 14901 RVA: 0x001107EC File Offset: 0x0010E9EC
		public static bool bloom
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsBloomEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsBloomEnabled = value;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06003A36 RID: 14902 RVA: 0x001107F9 File Offset: 0x0010E9F9
		// (set) Token: 0x06003A37 RID: 14903 RVA: 0x00110805 File Offset: 0x0010EA05
		public static bool chromaticAberration
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsChromaticAberrationEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsChromaticAberrationEnabled = value;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06003A38 RID: 14904 RVA: 0x00110812 File Offset: 0x0010EA12
		// (set) Token: 0x06003A39 RID: 14905 RVA: 0x0011081E File Offset: 0x0010EA1E
		public static bool filmGrain
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsFilmGrainEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsFilmGrainEnabled = value;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06003A3A RID: 14906 RVA: 0x0011082B File Offset: 0x0010EA2B
		// (set) Token: 0x06003A3B RID: 14907 RVA: 0x00110837 File Offset: 0x0010EA37
		public static bool blend
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsNiceBlendEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsNiceBlendEnabled = value;
			}
		}

		/// <summary>
		/// Distance to use terrain shaders before fallback to a baked texture.
		/// </summary>
		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06003A3C RID: 14908 RVA: 0x00110844 File Offset: 0x0010EA44
		public static float terrainBasemapDistance
		{
			get
			{
				return (float)(GraphicsSettings.blend ? 512 : 256);
			}
		}

		/// <summary>
		/// Higher error reduces vertex density as distance increases.
		/// </summary>
		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06003A3D RID: 14909 RVA: 0x0011085C File Offset: 0x0010EA5C
		public static float terrainHeightmapPixelError
		{
			get
			{
				switch (GraphicsSettings.terrainQuality)
				{
				case EGraphicQuality.LOW:
					return 64f;
				case EGraphicQuality.MEDIUM:
					return 32f;
				case EGraphicQuality.HIGH:
					return 16f;
				case EGraphicQuality.ULTRA:
					return 8f;
				default:
					UnturnedLog.warn("Unknown terrain quality {0} in terrainHeightmapPixelError", new object[]
					{
						GraphicsSettings.terrainQuality
					});
					return 1f;
				}
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06003A3E RID: 14910 RVA: 0x001108C3 File Offset: 0x0010EAC3
		// (set) Token: 0x06003A3F RID: 14911 RVA: 0x001108CF File Offset: 0x0010EACF
		public static bool grassDisplacement
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsGrassDisplacementEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsGrassDisplacementEnabled = value;
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06003A40 RID: 14912 RVA: 0x001108DC File Offset: 0x0010EADC
		// (set) Token: 0x06003A41 RID: 14913 RVA: 0x001108E8 File Offset: 0x0010EAE8
		public static bool foliageFocus
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsFoliageFocusEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsFoliageFocusEnabled = value;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06003A42 RID: 14914 RVA: 0x001108F5 File Offset: 0x0010EAF5
		// (set) Token: 0x06003A43 RID: 14915 RVA: 0x00110901 File Offset: 0x0010EB01
		public static EGraphicQuality landmarkQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.LandmarkQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.LandmarkQuality = value;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06003A44 RID: 14916 RVA: 0x0011090E File Offset: 0x0010EB0E
		// (set) Token: 0x06003A45 RID: 14917 RVA: 0x0011091A File Offset: 0x0010EB1A
		public static bool ragdolls
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsRagdollsEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsRagdollsEnabled = value;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06003A46 RID: 14918 RVA: 0x00110927 File Offset: 0x0010EB27
		// (set) Token: 0x06003A47 RID: 14919 RVA: 0x00110933 File Offset: 0x0010EB33
		public static bool debris
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsDebrisEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsDebrisEnabled = value;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06003A48 RID: 14920 RVA: 0x00110940 File Offset: 0x0010EB40
		// (set) Token: 0x06003A49 RID: 14921 RVA: 0x0011094C File Offset: 0x0010EB4C
		public static bool blast
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsBlastEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsBlastEnabled = value;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06003A4A RID: 14922 RVA: 0x00110959 File Offset: 0x0010EB59
		// (set) Token: 0x06003A4B RID: 14923 RVA: 0x00110965 File Offset: 0x0010EB65
		public static bool puddle
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsPuddleEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsPuddleEnabled = value;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06003A4C RID: 14924 RVA: 0x00110972 File Offset: 0x0010EB72
		// (set) Token: 0x06003A4D RID: 14925 RVA: 0x0011097E File Offset: 0x0010EB7E
		public static bool glitter
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsGlitterEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsGlitterEnabled = value;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06003A4E RID: 14926 RVA: 0x0011098B File Offset: 0x0010EB8B
		// (set) Token: 0x06003A4F RID: 14927 RVA: 0x00110997 File Offset: 0x0010EB97
		public static bool triplanar
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsTriplanarMappingEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsTriplanarMappingEnabled = value;
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06003A50 RID: 14928 RVA: 0x001109A4 File Offset: 0x0010EBA4
		// (set) Token: 0x06003A51 RID: 14929 RVA: 0x001109B0 File Offset: 0x0010EBB0
		public static bool skyboxReflection
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsSkyboxReflectionEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsSkyboxReflectionEnabled = value;
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06003A52 RID: 14930 RVA: 0x001109BD File Offset: 0x0010EBBD
		// (set) Token: 0x06003A53 RID: 14931 RVA: 0x001109C9 File Offset: 0x0010EBC9
		public static bool IsItemIconAntiAliasingEnabled
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.IsItemIconAntiAliasingEnabled;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.IsItemIconAntiAliasingEnabled = value;
			}
		}

		/// <summary>
		/// Multiplier for far clip plane distance.
		/// Clamped within [0, 1] range to prevent editing config files for an advantage.
		/// </summary>
		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06003A54 RID: 14932 RVA: 0x001109D6 File Offset: 0x0010EBD6
		// (set) Token: 0x06003A55 RID: 14933 RVA: 0x001109FA File Offset: 0x0010EBFA
		public static float NormalizedFarClipDistance
		{
			get
			{
				return Mathf.Clamp01(Level.isEditor ? GraphicsSettings.graphicsSettingsData.EditorFarClipDistance : GraphicsSettings.graphicsSettingsData.FarClipDistance);
			}
			set
			{
				if (Level.isEditor)
				{
					GraphicsSettings.graphicsSettingsData.EditorFarClipDistance = Mathf.Clamp01(value);
					return;
				}
				GraphicsSettings.graphicsSettingsData.FarClipDistance = Mathf.Clamp01(value);
			}
		}

		/// <summary>
		/// Multiplier for draw distance.
		/// Clamped within [0, 1] range to prevent editing config files for an advantage.
		/// </summary>
		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06003A56 RID: 14934 RVA: 0x00110A24 File Offset: 0x0010EC24
		// (set) Token: 0x06003A57 RID: 14935 RVA: 0x00110A35 File Offset: 0x0010EC35
		public static float normalizedDrawDistance
		{
			get
			{
				return Mathf.Clamp01(GraphicsSettings.graphicsSettingsData.DrawDistance);
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.DrawDistance = Mathf.Clamp01(value);
			}
		}

		/// <summary>
		/// Multiplier for draw distance of optional super-low LOD models.
		/// Clamped within [0, 1] range to prevent editing config files for an advantage.
		/// </summary>
		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06003A58 RID: 14936 RVA: 0x00110A47 File Offset: 0x0010EC47
		// (set) Token: 0x06003A59 RID: 14937 RVA: 0x00110A58 File Offset: 0x0010EC58
		public static float normalizedLandmarkDrawDistance
		{
			get
			{
				return Mathf.Clamp01(GraphicsSettings.graphicsSettingsData.LandmarkDistance);
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.LandmarkDistance = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06003A5A RID: 14938 RVA: 0x00110A6A File Offset: 0x0010EC6A
		// (set) Token: 0x06003A5B RID: 14939 RVA: 0x00110A76 File Offset: 0x0010EC76
		public static EGraphicQuality effectQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.EffectQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.EffectQuality = value;
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06003A5C RID: 14940 RVA: 0x00110A83 File Offset: 0x0010EC83
		// (set) Token: 0x06003A5D RID: 14941 RVA: 0x00110A9D File Offset: 0x0010EC9D
		public static EGraphicQuality foliageQuality
		{
			get
			{
				if (GraphicsSettings.graphicsSettingsData.FoliageQuality2 == EGraphicQuality.OFF)
				{
					return EGraphicQuality.LOW;
				}
				return GraphicsSettings.graphicsSettingsData.FoliageQuality2;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.FoliageQuality2 = value;
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06003A5E RID: 14942 RVA: 0x00110AAA File Offset: 0x0010ECAA
		// (set) Token: 0x06003A5F RID: 14943 RVA: 0x00110AB6 File Offset: 0x0010ECB6
		public static EGraphicQuality sunShaftsQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.SunShaftsQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.SunShaftsQuality = value;
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06003A60 RID: 14944 RVA: 0x00110AC3 File Offset: 0x0010ECC3
		// (set) Token: 0x06003A61 RID: 14945 RVA: 0x00110AE5 File Offset: 0x0010ECE5
		public static EGraphicQuality lightingQuality
		{
			get
			{
				if (Level.isVR && GraphicsSettings.graphicsSettingsData.LightingQuality == EGraphicQuality.ULTRA)
				{
					return EGraphicQuality.HIGH;
				}
				return GraphicsSettings.graphicsSettingsData.LightingQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.LightingQuality = value;
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06003A62 RID: 14946 RVA: 0x00110AF2 File Offset: 0x0010ECF2
		// (set) Token: 0x06003A63 RID: 14947 RVA: 0x00110AFE File Offset: 0x0010ECFE
		public static EGraphicQuality reflectionQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.ScreenSpaceReflectionQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.ScreenSpaceReflectionQuality = value;
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06003A64 RID: 14948 RVA: 0x00110B0B File Offset: 0x0010ED0B
		// (set) Token: 0x06003A65 RID: 14949 RVA: 0x00110B17 File Offset: 0x0010ED17
		public static EGraphicQuality planarReflectionQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.PlanarReflectionQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.PlanarReflectionQuality = value;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06003A66 RID: 14950 RVA: 0x00110B24 File Offset: 0x0010ED24
		// (set) Token: 0x06003A67 RID: 14951 RVA: 0x00110B46 File Offset: 0x0010ED46
		public static EGraphicQuality waterQuality
		{
			get
			{
				if (Level.isVR && GraphicsSettings.graphicsSettingsData.WaterQuality == EGraphicQuality.ULTRA)
				{
					return EGraphicQuality.HIGH;
				}
				return GraphicsSettings.graphicsSettingsData.WaterQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.WaterQuality = value;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06003A68 RID: 14952 RVA: 0x00110B53 File Offset: 0x0010ED53
		// (set) Token: 0x06003A69 RID: 14953 RVA: 0x00110B5F File Offset: 0x0010ED5F
		public static EGraphicQuality scopeQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.ScopeQuality2;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.ScopeQuality2 = value;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06003A6A RID: 14954 RVA: 0x00110B6C File Offset: 0x0010ED6C
		// (set) Token: 0x06003A6B RID: 14955 RVA: 0x00110B78 File Offset: 0x0010ED78
		public static EGraphicQuality outlineQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.OutlineQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.OutlineQuality = value;
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06003A6C RID: 14956 RVA: 0x00110B85 File Offset: 0x0010ED85
		// (set) Token: 0x06003A6D RID: 14957 RVA: 0x00110B91 File Offset: 0x0010ED91
		public static EGraphicQuality terrainQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.TerrainQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.TerrainQuality = value;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06003A6E RID: 14958 RVA: 0x00110B9E File Offset: 0x0010ED9E
		// (set) Token: 0x06003A6F RID: 14959 RVA: 0x00110BAA File Offset: 0x0010EDAA
		public static EGraphicQuality windQuality
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.WindQuality;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.WindQuality = value;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06003A70 RID: 14960 RVA: 0x00110BB7 File Offset: 0x0010EDB7
		// (set) Token: 0x06003A71 RID: 14961 RVA: 0x00110BC3 File Offset: 0x0010EDC3
		public static ETreeGraphicMode treeMode
		{
			get
			{
				return GraphicsSettings.graphicsSettingsData.TreeMode;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.TreeMode = value;
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06003A72 RID: 14962 RVA: 0x00110BD0 File Offset: 0x0010EDD0
		// (set) Token: 0x06003A73 RID: 14963 RVA: 0x00110BF8 File Offset: 0x0010EDF8
		public static ERenderMode renderMode
		{
			get
			{
				ERenderMode renderMode = GraphicsSettings.graphicsSettingsData.RenderMode2;
				if (renderMode <= ERenderMode.FORWARD)
				{
					return GraphicsSettings.graphicsSettingsData.RenderMode2;
				}
				return ERenderMode.FORWARD;
			}
			set
			{
				GraphicsSettings.graphicsSettingsData.RenderMode2 = value;
			}
		}

		// Token: 0x140000D8 RID: 216
		// (add) Token: 0x06003A74 RID: 14964 RVA: 0x00110C08 File Offset: 0x0010EE08
		// (remove) Token: 0x06003A75 RID: 14965 RVA: 0x00110C3C File Offset: 0x0010EE3C
		public static event GraphicsSettingsApplied graphicsSettingsApplied;

		// Token: 0x06003A76 RID: 14966 RVA: 0x00110C70 File Offset: 0x0010EE70
		public static void applyResolution()
		{
			if (Application.isEditor)
			{
				return;
			}
			bool flag = false;
			string commandLine = Environment.CommandLine;
			if (flag | commandLine.IndexOf("-screen-width", 3) >= 0 | commandLine.IndexOf("-screen-height", 3) >= 0 | commandLine.IndexOf("-screen-fullscreen", 3) >= 0 | commandLine.IndexOf("-window-mode", 3) >= 0)
			{
				UnturnedLog.info("Ignoring game resolution settings because Unity built-in command-line options were set");
				return;
			}
			int num = GraphicsSettings.resolution.Width;
			int num2 = GraphicsSettings.resolution.Height;
			int num3 = 0;
			if (GraphicsSettings.clWidth.hasValue)
			{
				num = GraphicsSettings.clWidth.value;
			}
			else if (GraphicsSettings.valveWidth.hasValue)
			{
				num = GraphicsSettings.valveWidth.value;
			}
			else if (Provider.preferenceData.Graphics.Override_Resolution_Width > 0)
			{
				num = Provider.preferenceData.Graphics.Override_Resolution_Width;
			}
			if (GraphicsSettings.clHeight.hasValue)
			{
				num2 = GraphicsSettings.clHeight.value;
			}
			else if (GraphicsSettings.valveHeight.hasValue)
			{
				num2 = GraphicsSettings.valveHeight.value;
			}
			else if (Provider.preferenceData.Graphics.Override_Resolution_Height > 0)
			{
				num2 = Provider.preferenceData.Graphics.Override_Resolution_Height;
			}
			if (GraphicsSettings.clRefreshRate.hasValue && GraphicsSettings.clRefreshRate.value > 0)
			{
				num3 = GraphicsSettings.clRefreshRate.value;
			}
			else if (Provider.preferenceData.Graphics.Override_Refresh_Rate > 0)
			{
				num3 = Provider.preferenceData.Graphics.Override_Refresh_Rate;
			}
			if (GraphicsSettings.clWidth.hasValue != GraphicsSettings.clHeight.hasValue)
			{
				UnturnedLog.warn("Mismatch of {0} and {1}", new object[]
				{
					GraphicsSettings.clWidth.key,
					GraphicsSettings.clHeight.key
				});
			}
			if (GraphicsSettings.valveWidth.hasValue != GraphicsSettings.valveHeight.hasValue)
			{
				UnturnedLog.warn("Mismatch of {0} and {1}", new object[]
				{
					GraphicsSettings.valveWidth.key,
					GraphicsSettings.valveHeight.key
				});
			}
			FullScreenMode fullScreenMode = GraphicsSettings.fullscreenMode;
			if (GraphicsSettings.clFullscreenMode.hasValue)
			{
				if (Enum.IsDefined(typeof(FullScreenMode), GraphicsSettings.clFullscreenMode.value))
				{
					fullScreenMode = (FullScreenMode)GraphicsSettings.clFullscreenMode.value;
				}
				else
				{
					UnturnedLog.warn(string.Format("Invalid fullscreen mode on command-line: {0}", GraphicsSettings.clFullscreenMode.value));
				}
			}
			else if (Provider.preferenceData.Graphics.Override_Fullscreen_Mode >= 0)
			{
				if (Enum.IsDefined(typeof(FullScreenMode), Provider.preferenceData.Graphics.Override_Fullscreen_Mode))
				{
					fullScreenMode = (FullScreenMode)Provider.preferenceData.Graphics.Override_Fullscreen_Mode;
				}
				else
				{
					UnturnedLog.warn(string.Format("Invalid fullscreen mode in config: {0}", Provider.preferenceData.Graphics.Override_Fullscreen_Mode));
				}
			}
			if (GraphicsSettings.fullscreenMode == FullScreenMode.ExclusiveFullScreen && num3 > 0)
			{
				UnturnedLog.info(string.Format("Requesting resolution change: {0} {1} x {2} @ {3} hz", new object[]
				{
					fullScreenMode,
					num,
					num2,
					num3
				}));
				Screen.SetResolution(num, num2, fullScreenMode, num3);
				return;
			}
			UnturnedLog.info(string.Format("Requesting resolution change: {0} {1} x {2}", fullScreenMode, num, num2));
			Screen.SetResolution(num, num2, fullScreenMode);
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00110FB9 File Offset: 0x0010F1B9
		internal static void ApplyVSyncAndTargetFrameRate()
		{
			QualitySettings.vSyncCount = (GraphicsSettings.buffer ? 1 : 0);
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x00110FCC File Offset: 0x0010F1CC
		public static void apply(string reason)
		{
			UnturnedLog.info("Applying graphics settings ({0})", new object[]
			{
				reason
			});
			if (GraphicsSettings.changeResolution)
			{
				GraphicsSettings.changeResolution = false;
				GraphicsSettings.applyResolution();
			}
			if (LevelLighting.sun != null)
			{
				if (GraphicsSettings.lightingQuality == EGraphicQuality.ULTRA || GraphicsSettings.lightingQuality == EGraphicQuality.HIGH)
				{
					LevelLighting.sun.GetComponent<Light>().shadowNormalBias = 0f;
				}
				else
				{
					LevelLighting.sun.GetComponent<Light>().shadowNormalBias = 0.5f;
				}
			}
			QualitySettings.SetQualityLevel((int)((byte)GraphicsSettings.lightingQuality + 1), true);
			GraphicsSettings.ApplyVSyncAndTargetFrameRate();
			switch (GraphicsSettings.anisotropicFilteringMode)
			{
			case EAnisotropicFilteringMode.DISABLED:
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
				break;
			case EAnisotropicFilteringMode.PER_TEXTURE:
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
				break;
			case EAnisotropicFilteringMode.FORCED_ON:
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
				break;
			}
			float num = (0.3f + GraphicsSettings.NormalizedFarClipDistance * 0.7f) * 2048f;
			if (GraphicsSettings.clFarClipDistance.hasValue)
			{
				num = Mathf.Clamp(GraphicsSettings.clFarClipDistance.value, 16f, 2048f);
			}
			float num2 = num + 725f;
			float num3 = 256f + GraphicsSettings.normalizedDrawDistance * 256f;
			num3 = Mathf.Min(num3, num);
			float[] array = new float[]
			{
				0f,
				0f,
				0f,
				0f,
				num2,
				0f,
				0f,
				0f,
				Level.isEditor ? num3 : 0f,
				0f,
				num3,
				0f,
				num3,
				num3 * 0.125f,
				num3,
				num3,
				num3 * 0.5f,
				num3 * 0.125f,
				num,
				num3,
				num2,
				0f,
				Level.isEditor ? num3 : 0f,
				num3,
				0f,
				0f,
				num3,
				num3,
				num3,
				0f,
				num3,
				num
			};
			float num4 = Mathf.Max(0f, num - num3) * GraphicsSettings.normalizedLandmarkDrawDistance;
			if (GraphicsSettings.landmarkQuality >= EGraphicQuality.LOW)
			{
				if (GraphicsSettings.uncapLandmarks)
				{
					array[15] = num;
				}
				else
				{
					array[15] += num4;
				}
			}
			if (GraphicsSettings.landmarkQuality >= EGraphicQuality.MEDIUM)
			{
				if (GraphicsSettings.uncapLandmarks)
				{
					array[14] = num;
				}
				else
				{
					array[14] += num4;
				}
			}
			if (GraphicsSettings.landmarkQuality >= EGraphicQuality.ULTRA)
			{
				if (GraphicsSettings.uncapLandmarks)
				{
					array[19] = num;
				}
				else
				{
					array[19] += num4;
				}
			}
			if (Level.isEditor)
			{
				num *= 2f;
				for (int i = 0; i < 32; i++)
				{
					array[i] *= 2f;
				}
			}
			if (!LevelObjects.shouldInstantlyLoad && !LevelGround.shouldInstantlyLoad)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						if (LevelObjects.regions != null && !LevelObjects.regions[(int)b, (int)b2])
						{
							List<LevelObject> list = LevelObjects.objects[(int)b, (int)b2];
							for (int j = 0; j < list.Count; j++)
							{
								LevelObject levelObject = list[j];
								if (levelObject != null)
								{
									levelObject.UpdateSkyboxActive();
								}
							}
						}
						if (LevelGround.regions != null && !LevelGround.regions[(int)b, (int)b2])
						{
							List<ResourceSpawnpoint> list2 = LevelGround.trees[(int)b, (int)b2];
							for (int k = 0; k < list2.Count; k++)
							{
								ResourceSpawnpoint resourceSpawnpoint = list2[k];
								if (resourceSpawnpoint != null)
								{
									if (GraphicsSettings.landmarkQuality >= EGraphicQuality.MEDIUM)
									{
										resourceSpawnpoint.enableSkybox();
									}
									else
									{
										resourceSpawnpoint.disableSkybox();
									}
								}
							}
						}
					}
				}
			}
			QualitySettings.lodBias = 2f + GraphicsSettings.normalizedDrawDistance * 3f + Mathf.Clamp(Provider.preferenceData.Graphics.LOD_Bias, 0f, 5f);
			LODGroupManager.Get().SynchronizeLODBias();
			QualitySettings.skinWeights = SkinWeights.FourBones;
			if (MainCamera.instance != null)
			{
				MainCamera.instance.renderingPath = ((GraphicsSettings.renderMode == ERenderMode.DEFERRED) ? RenderingPath.DeferredShading : RenderingPath.Forward);
				MainCamera.instance.allowHDR = true;
				MainCamera.instance.allowMSAA = false;
				SunShaftsCs component = MainCamera.instance.GetComponent<SunShaftsCs>();
				if (component != null)
				{
					if (GraphicsSettings.sunShaftsQuality == EGraphicQuality.LOW)
					{
						component.resolution = 0;
					}
					else if (GraphicsSettings.sunShaftsQuality == EGraphicQuality.MEDIUM)
					{
						component.resolution = 1;
					}
					else if (GraphicsSettings.sunShaftsQuality == EGraphicQuality.HIGH)
					{
						component.resolution = 2;
					}
					component.enabled = (GraphicsSettings.sunShaftsQuality > EGraphicQuality.OFF);
				}
				HighlightingRenderer component2 = MainCamera.instance.GetComponent<HighlightingRenderer>();
				if (component2 != null)
				{
					if (GraphicsSettings.outlineQuality == EGraphicQuality.LOW)
					{
						component2.downsampleFactor = 4;
						component2.iterations = 1;
						component2.blurMinSpread = 0.75f;
						component2.blurSpread = 0f;
						component2.blurIntensity = 0.25f;
					}
					else if (GraphicsSettings.outlineQuality == EGraphicQuality.MEDIUM)
					{
						component2.downsampleFactor = 4;
						component2.iterations = 2;
						component2.blurMinSpread = 0.5f;
						component2.blurSpread = 0.25f;
						component2.blurIntensity = 0.25f;
					}
					else if (GraphicsSettings.outlineQuality == EGraphicQuality.HIGH)
					{
						component2.downsampleFactor = 2;
						component2.iterations = 2;
						component2.blurMinSpread = 1f;
						component2.blurSpread = 0.5f;
						component2.blurIntensity = 0.25f;
					}
					else if (GraphicsSettings.outlineQuality == EGraphicQuality.ULTRA)
					{
						component2.downsampleFactor = 1;
						component2.iterations = 3;
						component2.blurMinSpread = 0.5f;
						component2.blurSpread = 0.5f;
						component2.blurIntensity = 0.25f;
					}
				}
				MainCamera.instance.farClipPlane = num;
				MainCamera.instance.layerCullDistances = array;
				MainCamera.instance.layerCullSpherical = true;
				if (Player.player != null)
				{
					Player.player.look.scopeCamera.farClipPlane = num;
					Player.player.look.scopeCamera.layerCullDistances = array;
					Player.player.look.scopeCamera.layerCullSpherical = true;
					Player.player.look.scopeCamera.depthTextureMode = DepthTextureMode.Depth;
					Player.player.look.updateScope(GraphicsSettings.scopeQuality);
					Player.player.look.scopeCamera.renderingPath = ((GraphicsSettings.renderMode == ERenderMode.DEFERRED) ? RenderingPath.DeferredShading : RenderingPath.Forward);
					Player.player.look.scopeCamera.allowHDR = true;
					Player.player.look.scopeCamera.allowMSAA = false;
					Player.player.animator.viewmodelCamera.renderingPath = ((GraphicsSettings.renderMode == ERenderMode.DEFERRED) ? RenderingPath.DeferredShading : RenderingPath.Forward);
					Player.player.animator.viewmodelCamera.allowHDR = true;
					Player.player.animator.viewmodelCamera.allowMSAA = false;
				}
			}
			switch (GraphicsSettings.foliageQuality)
			{
			case EGraphicQuality.OFF:
				FoliageSettings.enabled = false;
				FoliageSettings.drawDistance = 0;
				FoliageSettings.instanceDensity = 0f;
				FoliageSettings.drawFocusDistance = 0;
				FoliageSettings.focusDistance = 0f;
				break;
			case EGraphicQuality.LOW:
				FoliageSettings.enabled = true;
				FoliageSettings.drawDistance = 2;
				FoliageSettings.instanceDensity = 0.25f;
				FoliageSettings.drawFocusDistance = 1;
				FoliageSettings.focusDistance = num;
				break;
			case EGraphicQuality.MEDIUM:
				FoliageSettings.enabled = true;
				FoliageSettings.drawDistance = 3;
				FoliageSettings.instanceDensity = 0.5f;
				FoliageSettings.drawFocusDistance = 2;
				FoliageSettings.focusDistance = num;
				break;
			case EGraphicQuality.HIGH:
				FoliageSettings.enabled = true;
				FoliageSettings.drawDistance = 4;
				FoliageSettings.instanceDensity = 0.75f;
				FoliageSettings.drawFocusDistance = 3;
				FoliageSettings.focusDistance = num;
				break;
			case EGraphicQuality.ULTRA:
				FoliageSettings.enabled = true;
				FoliageSettings.drawDistance = 5;
				FoliageSettings.instanceDensity = 1f;
				FoliageSettings.drawFocusDistance = 4;
				FoliageSettings.focusDistance = num;
				break;
			default:
				FoliageSettings.enabled = true;
				FoliageSettings.drawDistance = 2;
				FoliageSettings.instanceDensity = 0.25f;
				FoliageSettings.drawFocusDistance = 1;
				FoliageSettings.focusDistance = num;
				UnturnedLog.error("Unknown foliage quality: " + GraphicsSettings.foliageQuality.ToString());
				break;
			}
			FoliageSettings.drawFocus = GraphicsSettings.foliageFocus;
			if (GraphicsSettings.waterQuality == EGraphicQuality.LOW || GraphicsSettings.waterQuality == EGraphicQuality.MEDIUM)
			{
				Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
				Shader.DisableKeyword("WATER_EDGEBLEND_ON");
			}
			else if (GraphicsSettings.waterQuality == EGraphicQuality.HIGH || GraphicsSettings.waterQuality == EGraphicQuality.ULTRA)
			{
				if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
				{
					Shader.EnableKeyword("WATER_EDGEBLEND_ON");
					Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
				}
				else
				{
					Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
					Shader.DisableKeyword("WATER_EDGEBLEND_ON");
				}
			}
			LevelLighting.isSkyboxReflectionEnabled = GraphicsSettings.skyboxReflection;
			if (GraphicsSettings.windQuality > EGraphicQuality.OFF)
			{
				Shader.EnableKeyword("NICE_FOLIAGE_ON");
				Shader.EnableKeyword("GRASS_WIND_ON");
			}
			else
			{
				Shader.DisableKeyword("NICE_FOLIAGE_ON");
				Shader.DisableKeyword("GRASS_WIND_ON");
			}
			if (GraphicsSettings.windQuality > EGraphicQuality.LOW)
			{
				Shader.EnableKeyword("ENABLE_WIND");
			}
			else
			{
				Shader.DisableKeyword("ENABLE_WIND");
			}
			switch (GraphicsSettings.windQuality)
			{
			case EGraphicQuality.OFF:
				Shader.SetGlobalInt("_MaxWindQuality", 0);
				break;
			case EGraphicQuality.LOW:
				Shader.SetGlobalInt("_MaxWindQuality", 1);
				break;
			case EGraphicQuality.MEDIUM:
				Shader.SetGlobalInt("_MaxWindQuality", 2);
				break;
			case EGraphicQuality.HIGH:
				Shader.SetGlobalInt("_MaxWindQuality", 3);
				break;
			case EGraphicQuality.ULTRA:
				Shader.SetGlobalInt("_MaxWindQuality", 4);
				break;
			}
			if (GraphicsSettings.grassDisplacement)
			{
				Shader.EnableKeyword("GRASS_DISPLACEMENT_ON");
			}
			else
			{
				Shader.DisableKeyword("GRASS_DISPLACEMENT_ON");
			}
			if (Level.info != null && Level.info.configData != null && Level.info.configData.Terrain_Snow_Sparkle && GraphicsSettings.glitter)
			{
				Shader.EnableKeyword("IS_SNOWING");
			}
			else
			{
				Shader.DisableKeyword("IS_SNOWING");
			}
			if (GraphicsSettings.triplanar)
			{
				Shader.EnableKeyword("TRIPLANAR_MAPPING_ON");
			}
			else
			{
				Shader.DisableKeyword("TRIPLANAR_MAPPING_ON");
			}
			GraphicsSettings.planarReflectionUpdateIndex++;
			UnturnedPostProcess.instance.applyUserSettings();
			GraphicsSettingsApplied graphicsSettingsApplied = GraphicsSettings.graphicsSettingsApplied;
			if (graphicsSettingsApplied != null)
			{
				graphicsSettingsApplied();
			}
			UnturnedLog.info("Applied graphics settings");
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x001119BC File Offset: 0x0010FBBC
		public static void restoreDefaults()
		{
			FullScreenMode fullscreenMode = FullScreenMode.Windowed;
			bool isVSyncEnabled = false;
			GraphicsSettingsResolution resolution = new GraphicsSettingsResolution();
			if (GraphicsSettings.graphicsSettingsData != null)
			{
				fullscreenMode = GraphicsSettings.graphicsSettingsData.FullscreenMode;
				isVSyncEnabled = GraphicsSettings.graphicsSettingsData.IsVSyncEnabled;
				resolution = GraphicsSettings.graphicsSettingsData.Resolution;
			}
			GraphicsSettings.graphicsSettingsData = new GraphicsSettingsData();
			GraphicsSettings.graphicsSettingsData.FullscreenMode = fullscreenMode;
			GraphicsSettings.graphicsSettingsData.IsVSyncEnabled = isVSyncEnabled;
			GraphicsSettings.graphicsSettingsData.Resolution = resolution;
			GraphicsSettings.fixDefaultResolution();
			GraphicsSettings.apply("restoring defaults");
		}

		/// <summary>
		/// Called after loading graphics settings from disk so that their values can be adjusted.
		/// </summary>
		// Token: 0x06003A7A RID: 14970 RVA: 0x00111A38 File Offset: 0x0010FC38
		private static void validateSettings()
		{
			if (GraphicsSettings.graphicsSettingsData.UserInterfaceScale.IsFinite())
			{
				float userInterfaceScale = GraphicsSettings.graphicsSettingsData.UserInterfaceScale;
				float num = Mathf.Clamp(userInterfaceScale, 0.5f, 2f);
				if (userInterfaceScale != num)
				{
					UnturnedLog.info(string.Format("Clamped UI scale from {0} to {1}", userInterfaceScale, num));
				}
				GraphicsSettings.graphicsSettingsData.UserInterfaceScale = num;
			}
			else
			{
				UnturnedLog.info(string.Format("Reset UI scale (was {0})", GraphicsSettings.graphicsSettingsData.UserInterfaceScale));
				GraphicsSettings.graphicsSettingsData.UserInterfaceScale = 1f;
			}
			GraphicsSettings.fixDefaultResolution();
		}

		/// <summary>
		/// If default resolution is zero, try falling back to a higher one.
		/// Used when restoring defaults and validating loaded settings.
		/// </summary>
		// Token: 0x06003A7B RID: 14971 RVA: 0x00111AD0 File Offset: 0x0010FCD0
		private static void fixDefaultResolution()
		{
			GraphicsSettingsResolution resolution = GraphicsSettings.graphicsSettingsData.Resolution;
			if (resolution == null || resolution.Width < 1 || resolution.Height < 1)
			{
				GraphicsSettings.graphicsSettingsData.Resolution = new GraphicsSettingsResolution(ScreenEx.GetHighestRecommendedResolution());
				UnturnedLog.info(string.Format("Restored default resolution to {0}x{1}", GraphicsSettings.graphicsSettingsData.Resolution.Width, GraphicsSettings.graphicsSettingsData.Resolution.Height));
			}
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x00111B48 File Offset: 0x0010FD48
		public static void load()
		{
			if (ReadWrite.fileExists("/Settings/Graphics.json", true))
			{
				try
				{
					GraphicsSettings.graphicsSettingsData = ReadWrite.deserializeJSON<GraphicsSettingsData>("/Settings/Graphics.json", true);
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "Unable to parse Graphics.json! consider validating with a JSON linter");
					GraphicsSettings.graphicsSettingsData = null;
				}
				if (GraphicsSettings.graphicsSettingsData == null)
				{
					GraphicsSettings.restoreDefaults();
				}
				else
				{
					GraphicsSettings.validateSettings();
				}
			}
			else
			{
				GraphicsSettings.restoreDefaults();
			}
			if (GraphicsSettings.graphicsSettingsData.EffectQuality == EGraphicQuality.OFF)
			{
				GraphicsSettings.graphicsSettingsData.EffectQuality = EGraphicQuality.MEDIUM;
			}
			if (!Application.isEditor)
			{
				Resolution highestRecommendedResolution = ScreenEx.GetHighestRecommendedResolution();
				if (GraphicsSettings.resolution.Width > highestRecommendedResolution.width || GraphicsSettings.resolution.Height > highestRecommendedResolution.height)
				{
					GraphicsSettings.resolution = new GraphicsSettingsResolution(highestRecommendedResolution);
				}
			}
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x00111C08 File Offset: 0x0010FE08
		public static void save()
		{
			ReadWrite.serializeJSON<GraphicsSettingsData>("/Settings/Graphics.json", true, GraphicsSettings.graphicsSettingsData);
		}

		// Token: 0x04002306 RID: 8966
		private static bool _uncapLandmarks = false;

		// Token: 0x04002307 RID: 8967
		public const float EFFECT_ULTRA = 64f;

		// Token: 0x04002308 RID: 8968
		public const float EFFECT_HIGH = 48f;

		// Token: 0x04002309 RID: 8969
		public const float EFFECT_MEDIUM = 32f;

		// Token: 0x0400230A RID: 8970
		public const float EFFECT_LOW = 16f;

		/// <summary>
		/// Planar reflection component updates its culling distance and culling mask when this is incremented.
		/// </summary>
		// Token: 0x0400230B RID: 8971
		public static int planarReflectionUpdateIndex;

		// Token: 0x0400230C RID: 8972
		private static GraphicsSettingsData graphicsSettingsData = new GraphicsSettingsData();

		/// <summary>
		/// Overrides in-game UI scale setting.
		/// </summary>
		// Token: 0x0400230D RID: 8973
		private static CommandLineFloat uiScale = new CommandLineFloat("-ui_scale");

		// Token: 0x0400230E RID: 8974
		private static bool didCacheUIScaleOverride = false;

		// Token: 0x0400230F RID: 8975
		private static float? cachedUIScaleOverride = default(float?);

		// Token: 0x04002310 RID: 8976
		private static CommandLineInt clTargetFrameRate = new CommandLineInt("-FrameRateLimit");

		/// <summary>
		/// Added for players who want to see if they can get better performance with a ridiculously low max draw distance.
		/// </summary>
		// Token: 0x04002311 RID: 8977
		private static CommandLineFloat clFarClipDistance = new CommandLineFloat("-FarClipDistance");

		// Token: 0x04002313 RID: 8979
		private static bool changeResolution;

		// Token: 0x04002314 RID: 8980
		private static CommandLineInt valveWidth = new CommandLineInt("-w");

		// Token: 0x04002315 RID: 8981
		private static CommandLineInt valveHeight = new CommandLineInt("-h");

		// Token: 0x04002316 RID: 8982
		private static CommandLineInt clWidth = new CommandLineInt("-width");

		// Token: 0x04002317 RID: 8983
		private static CommandLineInt clHeight = new CommandLineInt("-height");

		// Token: 0x04002318 RID: 8984
		private static CommandLineInt clFullscreenMode = new CommandLineInt("-fullscreenmode");

		// Token: 0x04002319 RID: 8985
		private static CommandLineInt clRefreshRate = new CommandLineInt("-refreshrate");

		// Token: 0x0400231A RID: 8986
		private static bool hasAppliedTargetFrameRate = false;

		// Token: 0x0400231B RID: 8987
		private static int lastAppliedTargetFrameRate = -1;

		// Token: 0x0400231C RID: 8988
		private static bool hasBoundApplicationFocusChangedEvent;
	}
}
