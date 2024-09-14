using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006D1 RID: 1745
	public class GraphicsSettingsData
	{
		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06003A80 RID: 14976 RVA: 0x00111CE5 File Offset: 0x0010FEE5
		// (set) Token: 0x06003A81 RID: 14977 RVA: 0x00111CED File Offset: 0x0010FEED
		public GraphicsSettingsResolution Resolution { get; set; }

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06003A82 RID: 14978 RVA: 0x00111CF6 File Offset: 0x0010FEF6
		// (set) Token: 0x06003A83 RID: 14979 RVA: 0x00111CFE File Offset: 0x0010FEFE
		public bool IsVSyncEnabled { get; set; }

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06003A84 RID: 14980 RVA: 0x00111D07 File Offset: 0x0010FF07
		// (set) Token: 0x06003A85 RID: 14981 RVA: 0x00111D0F File Offset: 0x0010FF0F
		public bool IsBloomEnabled { get; set; }

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06003A86 RID: 14982 RVA: 0x00111D18 File Offset: 0x0010FF18
		// (set) Token: 0x06003A87 RID: 14983 RVA: 0x00111D20 File Offset: 0x0010FF20
		public bool IsChromaticAberrationEnabled { get; set; }

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06003A88 RID: 14984 RVA: 0x00111D29 File Offset: 0x0010FF29
		// (set) Token: 0x06003A89 RID: 14985 RVA: 0x00111D31 File Offset: 0x0010FF31
		public bool IsFilmGrainEnabled { get; set; }

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06003A8A RID: 14986 RVA: 0x00111D3A File Offset: 0x0010FF3A
		// (set) Token: 0x06003A8B RID: 14987 RVA: 0x00111D42 File Offset: 0x0010FF42
		public bool IsNiceBlendEnabled { get; set; }

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06003A8C RID: 14988 RVA: 0x00111D4B File Offset: 0x0010FF4B
		// (set) Token: 0x06003A8D RID: 14989 RVA: 0x00111D53 File Offset: 0x0010FF53
		public float DrawDistance { get; set; }

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06003A8E RID: 14990 RVA: 0x00111D5C File Offset: 0x0010FF5C
		// (set) Token: 0x06003A8F RID: 14991 RVA: 0x00111D64 File Offset: 0x0010FF64
		public float LandmarkDistance { get; set; }

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06003A90 RID: 14992 RVA: 0x00111D6D File Offset: 0x0010FF6D
		// (set) Token: 0x06003A91 RID: 14993 RVA: 0x00111D75 File Offset: 0x0010FF75
		public EAntiAliasingType AntiAliasingType5 { get; set; }

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06003A92 RID: 14994 RVA: 0x00111D7E File Offset: 0x0010FF7E
		// (set) Token: 0x06003A93 RID: 14995 RVA: 0x00111D86 File Offset: 0x0010FF86
		public EAnisotropicFilteringMode AnisotropicFilteringMode { get; set; }

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06003A94 RID: 14996 RVA: 0x00111D8F File Offset: 0x0010FF8F
		// (set) Token: 0x06003A95 RID: 14997 RVA: 0x00111D97 File Offset: 0x0010FF97
		public EGraphicQuality EffectQuality { get; set; }

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06003A96 RID: 14998 RVA: 0x00111DA0 File Offset: 0x0010FFA0
		// (set) Token: 0x06003A97 RID: 14999 RVA: 0x00111DA8 File Offset: 0x0010FFA8
		public EGraphicQuality FoliageQuality2 { get; set; }

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06003A98 RID: 15000 RVA: 0x00111DB1 File Offset: 0x0010FFB1
		// (set) Token: 0x06003A99 RID: 15001 RVA: 0x00111DB9 File Offset: 0x0010FFB9
		public EGraphicQuality SunShaftsQuality { get; set; }

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06003A9A RID: 15002 RVA: 0x00111DC2 File Offset: 0x0010FFC2
		// (set) Token: 0x06003A9B RID: 15003 RVA: 0x00111DCA File Offset: 0x0010FFCA
		public EGraphicQuality LightingQuality { get; set; }

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06003A9C RID: 15004 RVA: 0x00111DD3 File Offset: 0x0010FFD3
		// (set) Token: 0x06003A9D RID: 15005 RVA: 0x00111DDB File Offset: 0x0010FFDB
		public EGraphicQuality ScreenSpaceReflectionQuality { get; set; }

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06003A9E RID: 15006 RVA: 0x00111DE4 File Offset: 0x0010FFE4
		// (set) Token: 0x06003A9F RID: 15007 RVA: 0x00111DEC File Offset: 0x0010FFEC
		public EGraphicQuality PlanarReflectionQuality { get; set; }

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06003AA0 RID: 15008 RVA: 0x00111DF5 File Offset: 0x0010FFF5
		// (set) Token: 0x06003AA1 RID: 15009 RVA: 0x00111DFD File Offset: 0x0010FFFD
		public EGraphicQuality WaterQuality { get; set; }

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06003AA2 RID: 15010 RVA: 0x00111E06 File Offset: 0x00110006
		// (set) Token: 0x06003AA3 RID: 15011 RVA: 0x00111E0E File Offset: 0x0011000E
		public EGraphicQuality ScopeQuality2 { get; set; }

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06003AA4 RID: 15012 RVA: 0x00111E17 File Offset: 0x00110017
		// (set) Token: 0x06003AA5 RID: 15013 RVA: 0x00111E1F File Offset: 0x0011001F
		public EGraphicQuality OutlineQuality { get; set; }

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06003AA6 RID: 15014 RVA: 0x00111E28 File Offset: 0x00110028
		// (set) Token: 0x06003AA7 RID: 15015 RVA: 0x00111E30 File Offset: 0x00110030
		public EGraphicQuality TerrainQuality { get; set; }

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x00111E39 File Offset: 0x00110039
		// (set) Token: 0x06003AA9 RID: 15017 RVA: 0x00111E41 File Offset: 0x00110041
		public EGraphicQuality WindQuality { get; set; }

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06003AAA RID: 15018 RVA: 0x00111E4A File Offset: 0x0011004A
		// (set) Token: 0x06003AAB RID: 15019 RVA: 0x00111E52 File Offset: 0x00110052
		public ETreeGraphicMode TreeMode { get; set; }

		// Token: 0x06003AAC RID: 15020 RVA: 0x00111E5C File Offset: 0x0011005C
		public GraphicsSettingsData()
		{
			this.Resolution = new GraphicsSettingsResolution();
			this.FullscreenMode = FullScreenMode.FullScreenWindow;
			this.IsVSyncEnabled = false;
			this.UserInterfaceScale = 1f;
			this.UseTargetFrameRate = false;
			this.TargetFrameRate = 1000;
			this.UseUnfocusedTargetFrameRate = false;
			this.UnfocusedTargetFrameRate = 1000;
			this.IsAmbientOcclusionEnabled = false;
			this.IsBloomEnabled = false;
			this.IsChromaticAberrationEnabled = false;
			this.IsFilmGrainEnabled = false;
			this.IsNiceBlendEnabled = true;
			this.IsGrassDisplacementEnabled = false;
			this.IsFoliageFocusEnabled = false;
			this.IsRagdollsEnabled = true;
			this.IsDebrisEnabled = true;
			this.IsBlastEnabled = true;
			this.IsPuddleEnabled = true;
			this.IsGlitterEnabled = true;
			this.IsTriplanarMappingEnabled = true;
			this.IsSkyboxReflectionEnabled = false;
			this.IsItemIconAntiAliasingEnabled = false;
			this.FarClipDistance = 0.333333f;
			this.EditorFarClipDistance = 1f;
			this.DrawDistance = 1f;
			this.LandmarkDistance = 0f;
			this.AntiAliasingType5 = EAntiAliasingType.OFF;
			this.AnisotropicFilteringMode = EAnisotropicFilteringMode.FORCED_ON;
			this.EffectQuality = EGraphicQuality.MEDIUM;
			this.FoliageQuality2 = EGraphicQuality.LOW;
			this.SunShaftsQuality = EGraphicQuality.OFF;
			this.LightingQuality = EGraphicQuality.LOW;
			this.ScreenSpaceReflectionQuality = EGraphicQuality.OFF;
			this.PlanarReflectionQuality = EGraphicQuality.MEDIUM;
			this.WaterQuality = EGraphicQuality.LOW;
			this.ScopeQuality2 = EGraphicQuality.OFF;
			this.OutlineQuality = EGraphicQuality.LOW;
			this.TerrainQuality = EGraphicQuality.MEDIUM;
			this.WindQuality = EGraphicQuality.OFF;
			this.TreeMode = ETreeGraphicMode.LEGACY;
			this.RenderMode2 = ERenderMode.FORWARD;
			this.LandmarkQuality = EGraphicQuality.OFF;
		}

		// Token: 0x0400231E RID: 8990
		public FullScreenMode FullscreenMode;

		// Token: 0x04002320 RID: 8992
		public float UserInterfaceScale;

		// Token: 0x04002321 RID: 8993
		public bool UseTargetFrameRate;

		// Token: 0x04002322 RID: 8994
		public int TargetFrameRate;

		// Token: 0x04002323 RID: 8995
		public bool UseUnfocusedTargetFrameRate;

		// Token: 0x04002324 RID: 8996
		public int UnfocusedTargetFrameRate;

		// Token: 0x04002325 RID: 8997
		public bool IsAmbientOcclusionEnabled;

		// Token: 0x0400232A RID: 9002
		public bool IsGrassDisplacementEnabled;

		// Token: 0x0400232B RID: 9003
		public bool IsFoliageFocusEnabled;

		// Token: 0x0400232C RID: 9004
		public bool IsRagdollsEnabled;

		// Token: 0x0400232D RID: 9005
		public bool IsDebrisEnabled;

		// Token: 0x0400232E RID: 9006
		public bool IsBlastEnabled;

		// Token: 0x0400232F RID: 9007
		public bool IsPuddleEnabled;

		// Token: 0x04002330 RID: 9008
		public bool IsGlitterEnabled;

		// Token: 0x04002331 RID: 9009
		public bool IsTriplanarMappingEnabled;

		// Token: 0x04002332 RID: 9010
		public bool IsSkyboxReflectionEnabled;

		// Token: 0x04002333 RID: 9011
		public bool IsItemIconAntiAliasingEnabled;

		/// <summary>
		/// Far clip plane multiplier in-game.
		/// </summary>
		// Token: 0x04002334 RID: 9012
		public float FarClipDistance;

		/// <summary>
		/// Far clip plane multiplier in level editor.
		/// </summary>
		// Token: 0x04002335 RID: 9013
		public float EditorFarClipDistance;

		// Token: 0x04002346 RID: 9030
		public ERenderMode RenderMode2;

		// Token: 0x04002347 RID: 9031
		public EGraphicQuality LandmarkQuality;
	}
}
