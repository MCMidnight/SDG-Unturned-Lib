using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001CC RID: 460
	public static class ESteamRejection_NetEnum
	{
		// Token: 0x06000DFC RID: 3580 RVA: 0x00030EA0 File Offset: 0x0002F0A0
		public static bool ReadEnum(this NetPakReader reader, out ESteamRejection value)
		{
			uint num;
			bool result = reader.ReadBits(6, ref num);
			if (num <= 48U)
			{
				value = (ESteamRejection)num;
				return result;
			}
			value = ESteamRejection.SERVER_FULL;
			return false;
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00030EC8 File Offset: 0x0002F0C8
		public static bool WriteEnum(this NetPakWriter writer, ESteamRejection value)
		{
			return writer.WriteBits((uint)value, 6);
		}
	}
}
