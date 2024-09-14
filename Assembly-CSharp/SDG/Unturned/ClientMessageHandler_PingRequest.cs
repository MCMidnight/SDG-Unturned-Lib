using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200026D RID: 621
	internal static class ClientMessageHandler_PingRequest
	{
		// Token: 0x0600126D RID: 4717 RVA: 0x0003FB65 File Offset: 0x0003DD65
		internal static void ReadMessage(NetPakReader reader)
		{
			NetMessages.SendMessageToServer(EServerMessage.PingResponse, ENetReliability.Unreliable, delegate(NetPakWriter writer)
			{
			});
		}
	}
}
