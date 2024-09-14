using System;
using SDG.NetPak;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200027D RID: 637
	internal static class ServerMessageHandler_PingRequest
	{
		// Token: 0x06001289 RID: 4745 RVA: 0x00041124 File Offset: 0x0003F324
		internal static void ReadMessage(ITransportConnection transportConnection, NetPakReader reader)
		{
			int i = 0;
			while (i < Provider.pending.Count)
			{
				if (transportConnection.Equals(Provider.pending[i].transportConnection))
				{
					if (Provider.pending[i].averagePingRequestsReceivedPerSecond > Provider.PING_REQUEST_INTERVAL * 2f)
					{
						if (NetMessages.shouldLogBadMessages)
						{
							UnturnedLog.info(string.Format("Ignoring PingRequest message from {0} because they exceeded rate limit", transportConnection));
						}
						return;
					}
					Provider.pending[i].lastReceivedPingRequestRealtime = Time.realtimeSinceStartup;
					Provider.pending[i].incrementNumPingRequestsReceived();
					NetMessages.SendMessageToClient(EClientMessage.PingResponse, ENetReliability.Unreliable, transportConnection, delegate(NetPakWriter writer)
					{
					});
					return;
				}
				else
				{
					i++;
				}
			}
			SteamPlayer steamPlayer = Provider.findPlayer(transportConnection);
			if (steamPlayer == null)
			{
				if (NetMessages.shouldLogBadMessages)
				{
					UnturnedLog.info(string.Format("Ignoring PingRequest message from {0} because there is no associated player", transportConnection));
				}
				return;
			}
			if (steamPlayer.averagePingRequestsReceivedPerSecond > Provider.PING_REQUEST_INTERVAL * 2f)
			{
				if (NetMessages.shouldLogBadMessages)
				{
					UnturnedLog.info(string.Format("Ignoring PingRequest message from {0} because they exceeded rate limit", transportConnection));
				}
				return;
			}
			steamPlayer.lastReceivedPingRequestRealtime = Time.realtimeSinceStartup;
			steamPlayer.incrementNumPingRequestsReceived();
			NetMessages.SendMessageToClient(EClientMessage.PingResponse, ENetReliability.Unreliable, transportConnection, delegate(NetPakWriter writer)
			{
			});
		}
	}
}
