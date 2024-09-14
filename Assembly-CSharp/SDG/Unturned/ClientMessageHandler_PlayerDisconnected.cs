using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000270 RID: 624
	internal static class ClientMessageHandler_PlayerDisconnected
	{
		// Token: 0x06001271 RID: 4721 RVA: 0x0003FEAC File Offset: 0x0003E0AC
		internal static void ReadMessage(NetPakReader reader)
		{
			byte index;
			if (SystemNetPakReaderEx.ReadUInt8(reader, ref index))
			{
				Provider.removePlayer(index);
			}
		}
	}
}
