using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001BF RID: 447
	public static class ENPCHoliday_NetEnum
	{
		// Token: 0x06000DE2 RID: 3554 RVA: 0x00030B94 File Offset: 0x0002ED94
		public static bool ReadEnum(this NetPakReader reader, out ENPCHoliday value)
		{
			uint num;
			bool result = reader.ReadBits(3, ref num);
			if (num <= 6U)
			{
				value = (ENPCHoliday)num;
				return result;
			}
			value = ENPCHoliday.NONE;
			return false;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00030BB8 File Offset: 0x0002EDB8
		public static bool WriteEnum(this NetPakWriter writer, ENPCHoliday value)
		{
			return writer.WriteBits((uint)value, 3);
		}
	}
}
