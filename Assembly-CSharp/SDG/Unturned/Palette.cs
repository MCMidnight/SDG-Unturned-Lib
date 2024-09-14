using System;
using System.Globalization;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003EB RID: 1003
	public class Palette
	{
		// Token: 0x06001DDF RID: 7647 RVA: 0x0006CB7F File Offset: 0x0006AD7F
		public static string hex(Color32 color)
		{
			return "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x0006CBC0 File Offset: 0x0006ADC0
		public static Color hex(string color)
		{
			uint num;
			if (!string.IsNullOrEmpty(color) && color.Length == 7 && uint.TryParse(color.Substring(1, color.Length - 1), 515, CultureInfo.CurrentCulture, ref num))
			{
				byte b = (byte)(num >> 16 & 255U);
				uint num2 = num >> 8 & 255U;
				uint num3 = num & 255U;
				return new Color32(b, (byte)num2, (byte)num3, byte.MaxValue);
			}
			return Color.white;
		}

		// Token: 0x04000DF5 RID: 3573
		public static readonly Color SERVER = Color.green;

		// Token: 0x04000DF6 RID: 3574
		public static readonly Color ADMIN = Color.cyan;

		// Token: 0x04000DF7 RID: 3575
		public static readonly Color PRO = new Color(0.8235294f, 0.7490196f, 0.13333334f);

		// Token: 0x04000DF8 RID: 3576
		public static readonly Color COLOR_W = new Color(0.7058824f, 0.7058824f, 0.7058824f);

		// Token: 0x04000DF9 RID: 3577
		public static readonly Color COLOR_R = new Color(0.7490196f, 0.12156863f, 0.12156863f);

		// Token: 0x04000DFA RID: 3578
		public static readonly Color COLOR_G = new Color(0.12156863f, 0.5294118f, 0.12156863f);

		// Token: 0x04000DFB RID: 3579
		public static readonly Color COLOR_B = new Color(0.19607843f, 0.59607846f, 0.78431374f);

		// Token: 0x04000DFC RID: 3580
		public static readonly Color COLOR_O = new Color(0.67058825f, 0.5019608f, 0.09803922f);

		// Token: 0x04000DFD RID: 3581
		public static readonly Color COLOR_Y = new Color(0.8627451f, 0.7058824f, 0.07450981f);

		// Token: 0x04000DFE RID: 3582
		public static readonly Color COLOR_P = new Color(0.41568628f, 0.27450982f, 0.42745098f);

		// Token: 0x04000DFF RID: 3583
		public static readonly Color AMBIENT = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x04000E00 RID: 3584
		public static readonly Color MYTHICAL = new Color(0.98039216f, 0.19607843f, 0.09803922f);
	}
}
