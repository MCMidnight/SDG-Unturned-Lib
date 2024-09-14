using System;
using TMPro;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000190 RID: 400
	internal static class GlazierUtils_uGUI
	{
		// Token: 0x06000B95 RID: 2965 RVA: 0x00026E2C File Offset: 0x0002502C
		public static TextAlignmentOptions TextAnchorToTMP(TextAnchor textAnchor)
		{
			switch (textAnchor)
			{
			case 0:
				return 257;
			case 1:
				return 258;
			case 2:
				return 260;
			case 3:
				return 513;
			case 5:
				return 516;
			case 6:
				return 1025;
			case 7:
				return 1026;
			case 8:
				return 1028;
			}
			return 514;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00026E9C File Offset: 0x0002509C
		public static float GetFontSize(ESleekFontSize fontSize)
		{
			switch (fontSize)
			{
			case 0:
				return 8f;
			case 1:
				return 10f;
			case 3:
				return 14f;
			case 4:
				return 20f;
			case 5:
				return 50f;
			}
			return 12f;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00026EEC File Offset: 0x000250EC
		public static FontStyles GetFontStyleFlags(FontStyle fontStyle)
		{
			switch (fontStyle)
			{
			case 1:
				return 1;
			case 2:
				return 2;
			case 3:
				return 3;
			}
			return 0;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00026F0D File Offset: 0x0002510D
		public static float GetCharacterSpacing(ETextContrastStyle shadowStyle)
		{
			switch (shadowStyle)
			{
			default:
				return 0f;
			case 1:
				return 10f;
			case 3:
				return 15f;
			}
		}
	}
}
