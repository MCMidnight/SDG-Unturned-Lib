using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001C3 RID: 451
	public static class EPlayerGroupRank_NetEnum
	{
		// Token: 0x06000DEA RID: 3562 RVA: 0x00030C80 File Offset: 0x0002EE80
		public static bool ReadEnum(this NetPakReader reader, out EPlayerGroupRank value)
		{
			uint num;
			bool result = reader.ReadBits(2, ref num);
			if (num <= 2U)
			{
				value = (EPlayerGroupRank)num;
				return result;
			}
			value = EPlayerGroupRank.MEMBER;
			return false;
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00030CA4 File Offset: 0x0002EEA4
		public static bool WriteEnum(this NetPakWriter writer, EPlayerGroupRank value)
		{
			return writer.WriteBits((uint)value, 2);
		}
	}
}
