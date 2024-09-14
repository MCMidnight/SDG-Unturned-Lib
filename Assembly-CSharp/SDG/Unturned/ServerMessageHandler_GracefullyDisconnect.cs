using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200027B RID: 635
	internal static class ServerMessageHandler_GracefullyDisconnect
	{
		// Token: 0x06001287 RID: 4743 RVA: 0x00040F58 File Offset: 0x0003F158
		internal static void ReadMessage(ITransportConnection transportConnection, NetPakReader reader)
		{
			SteamPlayer steamPlayer = Provider.findPlayer(transportConnection);
			if (steamPlayer == null)
			{
				if (NetMessages.shouldLogBadMessages)
				{
					UnturnedLog.info(string.Format("Ignoring GracefullyDisconnect message from {0} because there is no associated player", transportConnection));
				}
				return;
			}
			UnturnedLog.info(string.Format("Removing player {0} after graceful disconnect message", transportConnection));
			Provider.dismiss(steamPlayer.playerID.steamID);
		}
	}
}
