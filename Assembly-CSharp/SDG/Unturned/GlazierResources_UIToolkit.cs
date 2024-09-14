using System;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x020001A0 RID: 416
	internal static class GlazierResources_UIToolkit
	{
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00029F96 File Offset: 0x00028196
		public static ThemeStyleSheet Theme
		{
			get
			{
				if (OptionsSettings.proUI)
				{
					return GlazierResources_UIToolkit.DarkTheme;
				}
				return GlazierResources_UIToolkit.LightTheme;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x00029FB4 File Offset: 0x000281B4
		// (set) Token: 0x06000C7B RID: 3195 RVA: 0x00029FBB File Offset: 0x000281BB
		public static StaticResourceRef<ThemeStyleSheet> LightTheme { get; private set; } = new StaticResourceRef<ThemeStyleSheet>("UI/Glazier_UIToolkit/UnturnedLightTheme");

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x00029FC3 File Offset: 0x000281C3
		// (set) Token: 0x06000C7D RID: 3197 RVA: 0x00029FCA File Offset: 0x000281CA
		public static StaticResourceRef<ThemeStyleSheet> DarkTheme { get; private set; } = new StaticResourceRef<ThemeStyleSheet>("UI/Glazier_UIToolkit/UnturnedDarkTheme");
	}
}
