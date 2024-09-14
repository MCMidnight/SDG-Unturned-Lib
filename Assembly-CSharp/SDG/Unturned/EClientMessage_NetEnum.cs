using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001B7 RID: 439
	public static class EClientMessage_NetEnum
	{
		// Token: 0x06000DD2 RID: 3538 RVA: 0x000309C0 File Offset: 0x0002EBC0
		public static bool ReadEnum(this NetPakReader reader, out EClientMessage value)
		{
			uint num;
			bool result = reader.ReadBits(5, ref num);
			if (num <= 17U)
			{
				value = (EClientMessage)num;
				return result;
			}
			value = EClientMessage.UPDATE_RELIABLE_BUFFER;
			return false;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x000309E8 File Offset: 0x0002EBE8
		public static bool WriteEnum(this NetPakWriter writer, EClientMessage value)
		{
			return writer.WriteBits((uint)value, 5);
		}
	}
}
