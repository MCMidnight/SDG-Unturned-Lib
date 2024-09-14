using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001C2 RID: 450
	public static class EPlayerGesture_NetEnum
	{
		// Token: 0x06000DE8 RID: 3560 RVA: 0x00030C4C File Offset: 0x0002EE4C
		public static bool ReadEnum(this NetPakReader reader, out EPlayerGesture value)
		{
			uint num;
			bool result = reader.ReadBits(4, ref num);
			value = (EPlayerGesture)num;
			return result;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00030C68 File Offset: 0x0002EE68
		public static bool WriteEnum(this NetPakWriter writer, EPlayerGesture value)
		{
			return writer.WriteBits((uint)value, 4);
		}
	}
}
