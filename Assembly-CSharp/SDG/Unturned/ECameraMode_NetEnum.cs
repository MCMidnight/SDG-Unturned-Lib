using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001B5 RID: 437
	public static class ECameraMode_NetEnum
	{
		// Token: 0x06000DCE RID: 3534 RVA: 0x00030948 File Offset: 0x0002EB48
		public static bool ReadEnum(this NetPakReader reader, out ECameraMode value)
		{
			uint num;
			bool result = reader.ReadBits(3, ref num);
			if (num <= 4U)
			{
				value = (ECameraMode)num;
				return result;
			}
			value = ECameraMode.FIRST;
			return false;
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0003096C File Offset: 0x0002EB6C
		public static bool WriteEnum(this NetPakWriter writer, ECameraMode value)
		{
			return writer.WriteBits((uint)value, 3);
		}
	}
}
