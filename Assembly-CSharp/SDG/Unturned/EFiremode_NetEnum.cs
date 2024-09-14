using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001BB RID: 443
	public static class EFiremode_NetEnum
	{
		// Token: 0x06000DDA RID: 3546 RVA: 0x00030AB0 File Offset: 0x0002ECB0
		public static bool ReadEnum(this NetPakReader reader, out EFiremode value)
		{
			uint num;
			bool result = reader.ReadBits(2, ref num);
			value = (EFiremode)num;
			return result;
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00030ACC File Offset: 0x0002ECCC
		public static bool WriteEnum(this NetPakWriter writer, EFiremode value)
		{
			return writer.WriteBits((uint)value, 2);
		}
	}
}
