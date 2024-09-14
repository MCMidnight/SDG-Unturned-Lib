using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001CA RID: 458
	public static class ERaycastInfoUsage_NetEnum
	{
		// Token: 0x06000DF8 RID: 3576 RVA: 0x00030E30 File Offset: 0x0002F030
		public static bool ReadEnum(this NetPakReader reader, out ERaycastInfoUsage value)
		{
			uint num;
			bool result = reader.ReadBits(4, ref num);
			value = (ERaycastInfoUsage)num;
			return result;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00030E4C File Offset: 0x0002F04C
		public static bool WriteEnum(this NetPakWriter writer, ERaycastInfoUsage value)
		{
			return writer.WriteBits((uint)value, 4);
		}
	}
}
