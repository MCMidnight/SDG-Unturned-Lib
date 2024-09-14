using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001B8 RID: 440
	public static class EClientPlatform_NetEnum
	{
		// Token: 0x06000DD4 RID: 3540 RVA: 0x00030A00 File Offset: 0x0002EC00
		public static bool ReadEnum(this NetPakReader reader, out EClientPlatform value)
		{
			uint num;
			bool result = reader.ReadBits(2, ref num);
			if (num <= 2U)
			{
				value = (EClientPlatform)num;
				return result;
			}
			value = EClientPlatform.Windows;
			return false;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00030A24 File Offset: 0x0002EC24
		public static bool WriteEnum(this NetPakWriter writer, EClientPlatform value)
		{
			return writer.WriteBits((uint)value, 2);
		}
	}
}
