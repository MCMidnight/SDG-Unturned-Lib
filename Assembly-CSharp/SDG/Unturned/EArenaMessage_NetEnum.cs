using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001B4 RID: 436
	public static class EArenaMessage_NetEnum
	{
		// Token: 0x06000DCC RID: 3532 RVA: 0x00030914 File Offset: 0x0002EB14
		public static bool ReadEnum(this NetPakReader reader, out EArenaMessage value)
		{
			uint num;
			bool result = reader.ReadBits(3, ref num);
			value = (EArenaMessage)num;
			return result;
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00030930 File Offset: 0x0002EB30
		public static bool WriteEnum(this NetPakWriter writer, EArenaMessage value)
		{
			return writer.WriteBits((uint)value, 3);
		}
	}
}
