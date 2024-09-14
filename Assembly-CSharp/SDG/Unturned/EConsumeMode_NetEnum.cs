using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001B9 RID: 441
	public static class EConsumeMode_NetEnum
	{
		// Token: 0x06000DD6 RID: 3542 RVA: 0x00030A3C File Offset: 0x0002EC3C
		public static bool ReadEnum(this NetPakReader reader, out EConsumeMode value)
		{
			uint num;
			bool result = reader.ReadBits(1, ref num);
			value = (EConsumeMode)num;
			return result;
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00030A58 File Offset: 0x0002EC58
		public static bool WriteEnum(this NetPakWriter writer, EConsumeMode value)
		{
			return writer.WriteBits((uint)value, 1);
		}
	}
}
