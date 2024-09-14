using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001CB RID: 459
	public static class EServerMessage_NetEnum
	{
		// Token: 0x06000DFA RID: 3578 RVA: 0x00030E64 File Offset: 0x0002F064
		public static bool ReadEnum(this NetPakReader reader, out EServerMessage value)
		{
			uint num;
			bool result = reader.ReadBits(4, ref num);
			if (num <= 8U)
			{
				value = (EServerMessage)num;
				return result;
			}
			value = EServerMessage.GetWorkshopFiles;
			return false;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00030E88 File Offset: 0x0002F088
		public static bool WriteEnum(this NetPakWriter writer, EServerMessage value)
		{
			return writer.WriteBits((uint)value, 4);
		}
	}
}
