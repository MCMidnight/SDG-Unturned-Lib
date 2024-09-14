using System;
using System.Globalization;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	// Token: 0x02000080 RID: 128
	public static class PaletteUtility
	{
		// Token: 0x06000317 RID: 791 RVA: 0x0000C248 File Offset: 0x0000A448
		public static string toRGB(Color color)
		{
			Color32 color2 = color;
			return "#" + color2.r.ToString("X2") + color2.g.ToString("X2") + color2.b.ToString("X2");
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000C29C File Offset: 0x0000A49C
		public static string toRGBA(Color color)
		{
			Color32 color2 = color;
			return string.Concat(new string[]
			{
				"#",
				color2.r.ToString("X2"),
				color2.g.ToString("X2"),
				color2.b.ToString("X2"),
				color2.a.ToString("X2")
			});
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000C314 File Offset: 0x0000A514
		public static bool tryParse(string value, out Color color)
		{
			color = Color.white;
			if (!string.IsNullOrEmpty(value))
			{
				switch (value.Length)
				{
				case 6:
				{
					uint num;
					if (uint.TryParse(value, 515, CultureInfo.CurrentCulture, ref num))
					{
						color.r = (float)((byte)(num >> 16 & 255U));
						color.g = (float)((byte)(num >> 8 & 255U));
						color.b = (float)((byte)(num & 255U));
						color.a = 255f;
						return true;
					}
					break;
				}
				case 7:
				{
					uint num;
					if (uint.TryParse(value.Substring(1, value.Length - 1), 515, CultureInfo.CurrentCulture, ref num))
					{
						color.r = (float)((byte)(num >> 16 & 255U));
						color.g = (float)((byte)(num >> 8 & 255U));
						color.b = (float)((byte)(num & 255U));
						color.a = 255f;
						return true;
					}
					break;
				}
				case 8:
				{
					uint num;
					if (uint.TryParse(value, 515, CultureInfo.CurrentCulture, ref num))
					{
						color.r = (float)((byte)(num >> 24 & 255U));
						color.g = (float)((byte)(num >> 16 & 255U));
						color.b = (float)((byte)(num >> 8 & 255U));
						color.a = (float)((byte)(num & 255U));
						return true;
					}
					break;
				}
				case 9:
				{
					uint num;
					if (uint.TryParse(value.Substring(1, value.Length - 1), 515, CultureInfo.CurrentCulture, ref num))
					{
						color.r = (float)((byte)(num >> 24 & 255U));
						color.g = (float)((byte)(num >> 16 & 255U));
						color.b = (float)((byte)(num >> 8 & 255U));
						color.a = (float)((byte)(num & 255U));
						return true;
					}
					break;
				}
				}
			}
			return false;
		}
	}
}
