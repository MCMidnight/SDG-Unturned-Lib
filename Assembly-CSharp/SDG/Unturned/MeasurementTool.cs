using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200075C RID: 1884
	public class MeasurementTool
	{
		// Token: 0x06003DA6 RID: 15782 RVA: 0x00128A61 File Offset: 0x00126C61
		public static float speedToKPH(float speed)
		{
			return speed * 3.6f;
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x00128A6A File Offset: 0x00126C6A
		public static float KPHToMPH(float kph)
		{
			return kph / 1.609344f;
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x00128A73 File Offset: 0x00126C73
		public static float KtoM(float k)
		{
			return k * 0.621371f;
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x00128A7C File Offset: 0x00126C7C
		public static float MtoYd(float m)
		{
			return m * 1.09361f;
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x00128A85 File Offset: 0x00126C85
		public static int MtoYd(int m)
		{
			return (int)((float)m * 1.09361f);
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x00128A90 File Offset: 0x00126C90
		public static long MtoYd(long m)
		{
			return (long)((float)m * 1.09361f);
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x00128A9B File Offset: 0x00126C9B
		public static string FormatLengthString(float length)
		{
			if (OptionsSettings.metric)
			{
				return string.Format("{0} m", Mathf.RoundToInt(length));
			}
			return string.Format("{0} yd", Mathf.RoundToInt(MeasurementTool.MtoYd(length)));
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x00128AD4 File Offset: 0x00126CD4
		public static byte angleToByte(float angle)
		{
			if (angle < 0f)
			{
				return (byte)((360f + angle % 360f) / 2f);
			}
			return (byte)(angle % 360f / 2f);
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x00128B01 File Offset: 0x00126D01
		public static float byteToAngle(byte angle)
		{
			return (float)angle * 2f;
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x00128B0B File Offset: 0x00126D0B
		[Obsolete("Newer code should not be using this, instead NetPak should handle it.")]
		public static byte angleToByte2(float angle)
		{
			if (angle < 0f)
			{
				return (byte)((360f + angle % 360f) / 1.5f);
			}
			return (byte)(angle % 360f / 1.5f);
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x00128B38 File Offset: 0x00126D38
		[Obsolete("Newer code should not be using this, instead NetPak should handle it.")]
		public static float byteToAngle2(byte angle)
		{
			return (float)angle * 1.5f;
		}
	}
}
