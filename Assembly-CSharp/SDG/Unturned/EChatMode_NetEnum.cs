using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001B6 RID: 438
	public static class EChatMode_NetEnum
	{
		// Token: 0x06000DD0 RID: 3536 RVA: 0x00030984 File Offset: 0x0002EB84
		public static bool ReadEnum(this NetPakReader reader, out EChatMode value)
		{
			uint num;
			bool result = reader.ReadBits(3, ref num);
			if (num <= 4U)
			{
				value = (EChatMode)num;
				return result;
			}
			value = EChatMode.GLOBAL;
			return false;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x000309A8 File Offset: 0x0002EBA8
		public static bool WriteEnum(this NetPakWriter writer, EChatMode value)
		{
			return writer.WriteBits((uint)value, 3);
		}
	}
}
