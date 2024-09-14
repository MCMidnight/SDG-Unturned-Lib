using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001BC RID: 444
	public static class EGameMode_NetEnum
	{
		// Token: 0x06000DDC RID: 3548 RVA: 0x00030AE4 File Offset: 0x0002ECE4
		public static bool ReadEnum(this NetPakReader reader, out EGameMode value)
		{
			uint num;
			bool result = reader.ReadBits(3, ref num);
			if (num <= 4U)
			{
				value = (EGameMode)num;
				return result;
			}
			value = EGameMode.EASY;
			return false;
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00030B08 File Offset: 0x0002ED08
		public static bool WriteEnum(this NetPakWriter writer, EGameMode value)
		{
			return writer.WriteBits((uint)value, 3);
		}
	}
}
