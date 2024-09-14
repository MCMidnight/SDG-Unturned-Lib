using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001BA RID: 442
	public static class EDeathCause_NetEnum
	{
		// Token: 0x06000DD8 RID: 3544 RVA: 0x00030A70 File Offset: 0x0002EC70
		public static bool ReadEnum(this NetPakReader reader, out EDeathCause value)
		{
			uint num;
			bool result = reader.ReadBits(5, ref num);
			if (num <= 29U)
			{
				value = (EDeathCause)num;
				return result;
			}
			value = EDeathCause.BLEEDING;
			return false;
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00030A98 File Offset: 0x0002EC98
		public static bool WriteEnum(this NetPakWriter writer, EDeathCause value)
		{
			return writer.WriteBits((uint)value, 5);
		}
	}
}
