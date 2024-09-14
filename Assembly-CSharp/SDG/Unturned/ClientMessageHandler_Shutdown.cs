using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000273 RID: 627
	internal static class ClientMessageHandler_Shutdown
	{
		// Token: 0x06001274 RID: 4724 RVA: 0x00040344 File Offset: 0x0003E544
		internal static void ReadMessage(NetPakReader reader)
		{
			string text;
			SystemNetPakReaderEx.ReadString(reader, ref text, 11);
			Provider._connectionFailureInfo = ESteamConnectionFailureInfo.SHUTDOWN;
			Provider._connectionFailureReason = text;
			Provider.RequestDisconnect("Server was shutdown --- Reason: \"" + text + "\"");
		}
	}
}
