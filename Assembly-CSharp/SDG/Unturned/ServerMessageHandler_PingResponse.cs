using System;
using SDG.NetPak;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200027E RID: 638
	internal static class ServerMessageHandler_PingResponse
	{
		// Token: 0x0600128A RID: 4746 RVA: 0x00041280 File Offset: 0x0003F480
		internal static void ReadMessage(ITransportConnection transportConnection, NetPakReader reader)
		{
			SteamPlayer steamPlayer = Provider.findPlayer(transportConnection);
			if (steamPlayer != null)
			{
				if (steamPlayer.timeLastPingRequestWasSentToClient > 0f)
				{
					float deltaTime = Time.deltaTime;
					steamPlayer.timeLastPacketWasReceivedFromClient = Time.realtimeSinceStartup;
					steamPlayer.lag(Time.realtimeSinceStartup - steamPlayer.timeLastPingRequestWasSentToClient - deltaTime);
					steamPlayer.timeLastPingRequestWasSentToClient = -1f;
				}
				return;
			}
			if (NetMessages.shouldLogBadMessages)
			{
				UnturnedLog.info(string.Format("Ignoring PingResponse message from {0} because there is no associated player", transportConnection));
			}
		}
	}
}
