using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005B1 RID: 1457
	public class Customization
	{
		// Token: 0x06002F7C RID: 12156 RVA: 0x000D1C8C File Offset: 0x000CFE8C
		public static bool checkSkin(Color color)
		{
			for (int i = 0; i < Customization.SKINS.Length; i++)
			{
				if (Mathf.Abs(color.r - Customization.SKINS[i].r) < 0.01f && Mathf.Abs(color.g - Customization.SKINS[i].g) < 0.01f && Mathf.Abs(color.b - Customization.SKINS[i].b) < 0.01f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x000D1D18 File Offset: 0x000CFF18
		public static bool checkColor(Color color)
		{
			for (int i = 0; i < Customization.COLORS.Length; i++)
			{
				if (Mathf.Abs(color.r - Customization.COLORS[i].r) < 0.01f && Mathf.Abs(color.g - Customization.COLORS[i].g) < 0.01f && Mathf.Abs(color.b - Customization.COLORS[i].b) < 0.01f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400199B RID: 6555
		public static readonly byte FREE_CHARACTERS = 1;

		// Token: 0x0400199C RID: 6556
		public static readonly byte PRO_CHARACTERS = 4;

		// Token: 0x0400199D RID: 6557
		public static readonly byte FACES_FREE = 10;

		// Token: 0x0400199E RID: 6558
		public static readonly byte HAIRS_FREE = 5;

		// Token: 0x0400199F RID: 6559
		public static readonly byte BEARDS_FREE = 5;

		// Token: 0x040019A0 RID: 6560
		public static readonly byte FACES_PRO = 22;

		// Token: 0x040019A1 RID: 6561
		public static readonly byte HAIRS_PRO = 18;

		// Token: 0x040019A2 RID: 6562
		public static readonly byte BEARDS_PRO = 11;

		// Token: 0x040019A3 RID: 6563
		public static readonly Color[] SKINS = new Color[]
		{
			new Color(0.95686275f, 0.9019608f, 0.8235294f),
			new Color(0.8509804f, 0.7921569f, 0.7058824f),
			new Color(0.74509805f, 0.64705884f, 0.50980395f),
			new Color(0.6156863f, 0.53333336f, 0.41960785f),
			new Color(0.5803922f, 0.4627451f, 0.29411766f),
			new Color(0.4392157f, 0.3764706f, 0.28627452f),
			new Color(0.3254902f, 0.2784314f, 0.21176471f),
			new Color(0.29411766f, 0.23921569f, 0.19215687f),
			new Color(0.2f, 0.17254902f, 0.14509805f),
			new Color(0.13725491f, 0.12156863f, 0.10980392f)
		};

		// Token: 0x040019A4 RID: 6564
		public static readonly Color[] COLORS = new Color[]
		{
			new Color(0.84313726f, 0.84313726f, 0.84313726f),
			new Color(0.75686276f, 0.75686276f, 0.75686276f),
			new Color(0.8039216f, 0.7529412f, 0.54901963f),
			new Color(0.6745098f, 0.41568628f, 0.22352941f),
			new Color(0.4f, 0.3137255f, 0.21568628f),
			new Color(0.34117648f, 0.27058825f, 0.18431373f),
			new Color(0.2784314f, 0.22352941f, 0.15686275f),
			new Color(0.20784314f, 0.17254902f, 0.13333334f),
			new Color(0.21568628f, 0.21568628f, 0.21568628f),
			new Color(0.09803922f, 0.09803922f, 0.09803922f)
		};

		// Token: 0x040019A5 RID: 6565
		public static readonly Color[] MARKER_COLORS = new Color[]
		{
			Palette.COLOR_B,
			Palette.COLOR_G,
			Palette.COLOR_O,
			Palette.COLOR_P,
			Palette.COLOR_R,
			Palette.COLOR_Y
		};

		// Token: 0x040019A6 RID: 6566
		public static readonly byte SKILLSETS = 11;
	}
}
