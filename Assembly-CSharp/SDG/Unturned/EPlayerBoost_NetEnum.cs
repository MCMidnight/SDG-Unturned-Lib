using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001C1 RID: 449
	public static class EPlayerBoost_NetEnum
	{
		// Token: 0x06000DE6 RID: 3558 RVA: 0x00030C10 File Offset: 0x0002EE10
		public static bool ReadEnum(this NetPakReader reader, out EPlayerBoost value)
		{
			uint num;
			bool result = reader.ReadBits(3, ref num);
			if (num <= 4U)
			{
				value = (EPlayerBoost)num;
				return result;
			}
			value = EPlayerBoost.NONE;
			return false;
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00030C34 File Offset: 0x0002EE34
		public static bool WriteEnum(this NetPakWriter writer, EPlayerBoost value)
		{
			return writer.WriteBits((uint)value, 3);
		}
	}
}
