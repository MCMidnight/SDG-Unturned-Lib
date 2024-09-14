using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001BD RID: 445
	public static class ELimb_NetEnum
	{
		// Token: 0x06000DDE RID: 3550 RVA: 0x00030B20 File Offset: 0x0002ED20
		public static bool ReadEnum(this NetPakReader reader, out ELimb value)
		{
			uint num;
			bool result = reader.ReadBits(4, ref num);
			if (num <= 13U)
			{
				value = (ELimb)num;
				return result;
			}
			value = ELimb.LEFT_FOOT;
			return false;
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00030B48 File Offset: 0x0002ED48
		public static bool WriteEnum(this NetPakWriter writer, ELimb value)
		{
			return writer.WriteBits((uint)value, 4);
		}
	}
}
