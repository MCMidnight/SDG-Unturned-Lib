using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001C6 RID: 454
	public static class EPlayerStance_NetEnum
	{
		// Token: 0x06000DF0 RID: 3568 RVA: 0x00030D3C File Offset: 0x0002EF3C
		public static bool ReadEnum(this NetPakReader reader, out EPlayerStance value)
		{
			uint num;
			bool result = reader.ReadBits(3, ref num);
			value = (EPlayerStance)num;
			return result;
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00030D58 File Offset: 0x0002EF58
		public static bool WriteEnum(this NetPakWriter writer, EPlayerStance value)
		{
			return writer.WriteBits((uint)value, 3);
		}
	}
}
