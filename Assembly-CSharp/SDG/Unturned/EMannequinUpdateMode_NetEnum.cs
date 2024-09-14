using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001BE RID: 446
	public static class EMannequinUpdateMode_NetEnum
	{
		// Token: 0x06000DE0 RID: 3552 RVA: 0x00030B60 File Offset: 0x0002ED60
		public static bool ReadEnum(this NetPakReader reader, out EMannequinUpdateMode value)
		{
			uint num;
			bool result = reader.ReadBits(2, ref num);
			value = (EMannequinUpdateMode)num;
			return result;
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00030B7C File Offset: 0x0002ED7C
		public static bool WriteEnum(this NetPakWriter writer, EMannequinUpdateMode value)
		{
			return writer.WriteBits((uint)value, 2);
		}
	}
}
