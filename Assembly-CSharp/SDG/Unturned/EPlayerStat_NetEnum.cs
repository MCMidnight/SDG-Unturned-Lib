using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001C7 RID: 455
	public static class EPlayerStat_NetEnum
	{
		// Token: 0x06000DF2 RID: 3570 RVA: 0x00030D70 File Offset: 0x0002EF70
		public static bool ReadEnum(this NetPakReader reader, out EPlayerStat value)
		{
			uint num;
			bool result = reader.ReadBits(5, ref num);
			if (num <= 18U)
			{
				value = (EPlayerStat)num;
				return result;
			}
			value = EPlayerStat.NONE;
			return false;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00030D98 File Offset: 0x0002EF98
		public static bool WriteEnum(this NetPakWriter writer, EPlayerStat value)
		{
			return writer.WriteBits((uint)value, 5);
		}
	}
}
