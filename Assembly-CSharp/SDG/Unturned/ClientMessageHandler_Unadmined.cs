using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000274 RID: 628
	internal static class ClientMessageHandler_Unadmined
	{
		// Token: 0x06001275 RID: 4725 RVA: 0x00040380 File Offset: 0x0003E580
		internal static void ReadMessage(NetPakReader reader)
		{
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			SteamPlayer steamPlayer = PlayerTool.findSteamPlayerByChannel((int)b);
			if (steamPlayer != null)
			{
				steamPlayer.isAdmin = false;
				return;
			}
			UnturnedLog.error("Unadmined unable to find channel {0}", new object[]
			{
				b
			});
		}
	}
}
