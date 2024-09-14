using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001CD RID: 461
	public static class ESwingMode_NetEnum
	{
		// Token: 0x06000DFE RID: 3582 RVA: 0x00030EE0 File Offset: 0x0002F0E0
		public static bool ReadEnum(this NetPakReader reader, out ESwingMode value)
		{
			uint num;
			bool result = reader.ReadBits(1, ref num);
			value = (ESwingMode)num;
			return result;
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00030EFC File Offset: 0x0002F0FC
		public static bool WriteEnum(this NetPakWriter writer, ESwingMode value)
		{
			return writer.WriteBits((uint)value, 1);
		}
	}
}
