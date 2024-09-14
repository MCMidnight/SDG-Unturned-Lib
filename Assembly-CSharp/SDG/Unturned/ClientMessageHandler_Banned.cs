using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000268 RID: 616
	internal static class ClientMessageHandler_Banned
	{
		// Token: 0x06001266 RID: 4710 RVA: 0x0003F62C File Offset: 0x0003D82C
		internal static void ReadMessage(NetPakReader reader)
		{
			string text;
			SystemNetPakReaderEx.ReadString(reader, ref text, 11);
			uint num;
			SystemNetPakReaderEx.ReadUInt32(reader, ref num);
			Provider._connectionFailureInfo = ESteamConnectionFailureInfo.BANNED;
			Provider._connectionFailureReason = text;
			Provider._connectionFailureDuration = num;
			Provider.RequestDisconnect(string.Format("Banned from server. Reason: \"{0}\" Duration: {1}", text, TimeSpan.FromSeconds(num)));
		}
	}
}
