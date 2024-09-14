using System;
using TMPro;

namespace SDG.Unturned
{
	// Token: 0x0200081D RID: 2077
	public static class TextMeshProUtils
	{
		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x060046EC RID: 18156 RVA: 0x001A7DFC File Offset: 0x001A5FFC
		// (set) Token: 0x060046ED RID: 18157 RVA: 0x001A7E03 File Offset: 0x001A6003
		public static StaticResourceRef<TMP_FontAsset> DefaultFont { get; private set; } = new StaticResourceRef<TMP_FontAsset>("UI/LiberationSans SDF with CJK");

		// Token: 0x060046EE RID: 18158 RVA: 0x001A7E0B File Offset: 0x001A600B
		public static void FixupFont(TextMeshPro component)
		{
			if (component.font == null || component.font.name.Equals("LiberationSans SDF"))
			{
				component.font = TextMeshProUtils.DefaultFont;
			}
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x001A7E42 File Offset: 0x001A6042
		public static void FixupFont(TextMeshProUGUI component)
		{
			if (component.font == null || component.font.name.Equals("LiberationSans SDF"))
			{
				component.font = TextMeshProUtils.DefaultFont;
			}
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x001A7E79 File Offset: 0x001A6079
		public static void FixupFont(TMP_InputField component)
		{
			if (component.fontAsset == null || component.fontAsset.name.Equals("LiberationSans SDF"))
			{
				component.fontAsset = TextMeshProUtils.DefaultFont;
			}
		}

		// Token: 0x04003045 RID: 12357
		public const string DefaultFontName = "LiberationSans SDF";
	}
}
