using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200026C RID: 620
	internal static class ClientMessageHandler_Kicked
	{
		// Token: 0x0600126C RID: 4716 RVA: 0x0003FB2C File Offset: 0x0003DD2C
		internal static void ReadMessage(NetPakReader reader)
		{
			string text;
			SystemNetPakReaderEx.ReadString(reader, ref text, 11);
			Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
			Provider._connectionFailureReason = text;
			Provider.RequestDisconnect("Kicked from server. Reason: \"" + text + "\"");
		}
	}
}
