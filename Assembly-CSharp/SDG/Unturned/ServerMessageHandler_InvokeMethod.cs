using System;
using SDG.NetPak;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200027C RID: 636
	internal static class ServerMessageHandler_InvokeMethod
	{
		// Token: 0x06001288 RID: 4744 RVA: 0x00040FAC File Offset: 0x0003F1AC
		internal static void ReadMessage(ITransportConnection transportConnection, NetPakReader reader)
		{
			uint num;
			if (!reader.ReadBits(NetReflection.serverMethodsBitCount, ref num))
			{
				Provider.refuseGarbageConnection(transportConnection, "unable to read method index");
				return;
			}
			if (num >= NetReflection.serverMethodsLength)
			{
				Provider.refuseGarbageConnection(transportConnection, "out of bounds method index");
				return;
			}
			SteamPlayer steamPlayer = Provider.findPlayer(transportConnection);
			if (steamPlayer == null)
			{
				if (NetMessages.shouldLogBadMessages)
				{
					UnturnedLog.info(string.Format("Ignoring InvokeMethod message from {0} because there is no associated player", transportConnection));
				}
				return;
			}
			ServerMethodInfo serverMethodInfo = NetReflection.serverMethods[(int)num];
			ServerInvocationContext serverInvocationContext = new ServerInvocationContext(ServerInvocationContext.EOrigin.Remote, steamPlayer, reader, serverMethodInfo);
			if (serverMethodInfo.rateLimitIndex >= 0)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				float num2 = steamPlayer.rpcAllowedTimes[serverMethodInfo.rateLimitIndex];
				if (realtimeSinceStartup < num2)
				{
					steamPlayer.rpcHitCount[serverMethodInfo.rateLimitIndex]++;
					int num3 = Mathf.Max(2, Provider.configData.Server.Rate_Limit_Kick_Threshold);
					if (steamPlayer.rpcHitCount[serverMethodInfo.rateLimitIndex] >= num3)
					{
						serverInvocationContext.Kick(string.Format("significantly exceeded {0} rate limit ({1} times in {2} seconds)", serverMethodInfo, num3, serverMethodInfo.customAttribute.ratelimitSeconds));
					}
					return;
				}
				steamPlayer.rpcAllowedTimes[serverMethodInfo.rateLimitIndex] = realtimeSinceStartup + serverMethodInfo.customAttribute.ratelimitSeconds;
				steamPlayer.rpcHitCount[serverMethodInfo.rateLimitIndex] = 0;
			}
			try
			{
				steamPlayer.timeLastPacketWasReceivedFromClient = Time.realtimeSinceStartup;
				serverMethodInfo.readMethod(serverInvocationContext);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception invoking {0} from client {1}:", new object[]
				{
					serverMethodInfo,
					transportConnection
				});
			}
		}
	}
}
