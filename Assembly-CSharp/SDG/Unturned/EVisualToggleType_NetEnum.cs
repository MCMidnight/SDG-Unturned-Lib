using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001CE RID: 462
	public static class EVisualToggleType_NetEnum
	{
		// Token: 0x06000E00 RID: 3584 RVA: 0x00030F14 File Offset: 0x0002F114
		public static bool ReadEnum(this NetPakReader reader, out EVisualToggleType value)
		{
			uint num;
			bool result = reader.ReadBits(2, ref num);
			if (num <= 2U)
			{
				value = (EVisualToggleType)num;
				return result;
			}
			value = EVisualToggleType.COSMETIC;
			return false;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00030F38 File Offset: 0x0002F138
		public static bool WriteEnum(this NetPakWriter writer, EVisualToggleType value)
		{
			return writer.WriteBits((uint)value, 2);
		}
	}
}
