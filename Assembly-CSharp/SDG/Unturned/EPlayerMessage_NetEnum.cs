using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001C4 RID: 452
	public static class EPlayerMessage_NetEnum
	{
		// Token: 0x06000DEC RID: 3564 RVA: 0x00030CBC File Offset: 0x0002EEBC
		public static bool ReadEnum(this NetPakReader reader, out EPlayerMessage value)
		{
			uint num;
			bool result = reader.ReadBits(7, ref num);
			if (num <= 114U)
			{
				value = (EPlayerMessage)num;
				return result;
			}
			value = EPlayerMessage.NONE;
			return false;
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00030CE4 File Offset: 0x0002EEE4
		public static bool WriteEnum(this NetPakWriter writer, EPlayerMessage value)
		{
			return writer.WriteBits((uint)value, 7);
		}
	}
}
