using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200016B RID: 363
	internal static class GlazierResources_IMGUI
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x000200C8 File Offset: 0x0001E2C8
		public static GUISkin ActiveSkin
		{
			get
			{
				return OptionsSettings.proUI ? GlazierResources_IMGUI.darkTheme : GlazierResources_IMGUI.lightTheme;
			}
		}

		// Token: 0x0400038E RID: 910
		private static StaticResourceRef<GUISkin> lightTheme = new StaticResourceRef<GUISkin>("UI/Glazier_IMGUI/LightTheme");

		// Token: 0x0400038F RID: 911
		private static StaticResourceRef<GUISkin> darkTheme = new StaticResourceRef<GUISkin>("UI/Glazier_IMGUI/DarkTheme");
	}
}
