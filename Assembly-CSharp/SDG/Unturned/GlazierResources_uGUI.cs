using System;
using TMPro;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000185 RID: 389
	internal static class GlazierResources_uGUI
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x000246EF File Offset: 0x000228EF
		public static GlazierTheme_uGUI Theme
		{
			get
			{
				if (OptionsSettings.proUI)
				{
					return GlazierResources_uGUI.darkTheme;
				}
				return GlazierResources_uGUI.lightTheme;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00024703 File Offset: 0x00022903
		// (set) Token: 0x06000AE2 RID: 2786 RVA: 0x0002470A File Offset: 0x0002290A
		public static StaticResourceRef<Sprite> TooltipShadowSprite { get; private set; } = new StaticResourceRef<Sprite>("UI/Glazier_uGUI/TooltipShadow");

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x00024712 File Offset: 0x00022912
		// (set) Token: 0x06000AE4 RID: 2788 RVA: 0x00024719 File Offset: 0x00022919
		public static StaticResourceRef<TMP_FontAsset> Font { get; private set; } = new StaticResourceRef<TMP_FontAsset>("UI/Glazier_uGUI/LiberationSans");

		// Token: 0x0400041C RID: 1052
		private static GlazierTheme_uGUI lightTheme = new GlazierTheme_uGUI("UI/Glazier_uGUI/LightTheme");

		// Token: 0x0400041D RID: 1053
		private static GlazierTheme_uGUI darkTheme = new GlazierTheme_uGUI("UI/Glazier_uGUI/DarkTheme");

		// Token: 0x04000420 RID: 1056
		public static StaticResourceRef<Material> FontMaterial_Default = new StaticResourceRef<Material>("UI/Glazier_uGUI/Font_Default");

		// Token: 0x04000421 RID: 1057
		public static StaticResourceRef<Material> FontMaterial_Outline = new StaticResourceRef<Material>("UI/Glazier_uGUI/Font_Outline");

		// Token: 0x04000422 RID: 1058
		public static StaticResourceRef<Material> FontMaterial_Shadow = new StaticResourceRef<Material>("UI/Glazier_uGUI/Font_Shadow");

		// Token: 0x04000423 RID: 1059
		public static StaticResourceRef<Material> FontMaterial_Tooltip = new StaticResourceRef<Material>("UI/Glazier_uGUI/Font_Tooltip");
	}
}
