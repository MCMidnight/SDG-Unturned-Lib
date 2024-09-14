using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001D0 RID: 464
	public static class EZombieSpeciality_NetEnum
	{
		// Token: 0x06000E04 RID: 3588 RVA: 0x00030F8C File Offset: 0x0002F18C
		public static bool ReadEnum(this NetPakReader reader, out EZombieSpeciality value)
		{
			uint num;
			bool result = reader.ReadBits(5, ref num);
			if (num <= 24U)
			{
				value = (EZombieSpeciality)num;
				return result;
			}
			value = EZombieSpeciality.NONE;
			return false;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00030FB4 File Offset: 0x0002F1B4
		public static bool WriteEnum(this NetPakWriter writer, EZombieSpeciality value)
		{
			return writer.WriteBits((uint)value, 5);
		}
	}
}
