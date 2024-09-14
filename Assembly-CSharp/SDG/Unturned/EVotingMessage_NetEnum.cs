using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001CF RID: 463
	public static class EVotingMessage_NetEnum
	{
		// Token: 0x06000E02 RID: 3586 RVA: 0x00030F50 File Offset: 0x0002F150
		public static bool ReadEnum(this NetPakReader reader, out EVotingMessage value)
		{
			uint num;
			bool result = reader.ReadBits(3, ref num);
			if (num <= 4U)
			{
				value = (EVotingMessage)num;
				return result;
			}
			value = EVotingMessage.OFF;
			return false;
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00030F74 File Offset: 0x0002F174
		public static bool WriteEnum(this NetPakWriter writer, EVotingMessage value)
		{
			return writer.WriteBits((uint)value, 3);
		}
	}
}
