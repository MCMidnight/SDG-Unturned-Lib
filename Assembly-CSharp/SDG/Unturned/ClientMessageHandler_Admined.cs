using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000267 RID: 615
	internal static class ClientMessageHandler_Admined
	{
		// Token: 0x06001265 RID: 4709 RVA: 0x0003F5E8 File Offset: 0x0003D7E8
		internal static void ReadMessage(NetPakReader reader)
		{
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			SteamPlayer steamPlayer = PlayerTool.findSteamPlayerByChannel((int)b);
			if (steamPlayer != null)
			{
				steamPlayer.isAdmin = true;
				return;
			}
			UnturnedLog.error("Admined unable to find channel {0}", new object[]
			{
				b
			});
		}
	}
}
