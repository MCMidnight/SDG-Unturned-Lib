using System;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000275 RID: 629
	internal static class ClientMessageHandler_Verify
	{
		// Token: 0x06001276 RID: 4726 RVA: 0x000403C4 File Offset: 0x0003E5C4
		internal static void ReadMessage(NetPakReader reader)
		{
			Provider.isWaitingForConnectResponse = false;
			SteamNetworkingIdentity serverIdentity = default(SteamNetworkingIdentity);
			serverIdentity.SetSteamID(Provider.server);
			byte[] ticket = Provider.openTicket(serverIdentity);
			if (ticket == null)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_EMPTY;
				Provider.RequestDisconnect("opening Steam auth ticket failed");
				return;
			}
			UnturnedLog.info("Authenticating with server");
			NetMessages.SendMessageToServer(EServerMessage.Authenticate, ENetReliability.Reliable, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt16(writer, (ushort)ticket.Length);
				writer.WriteBytes(ticket);
				ClientMessageHandler_Verify.WriteEconomyDetails(writer);
			});
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x00040434 File Offset: 0x0003E634
		private static void WriteEconomyDetails(NetPakWriter writer)
		{
			if (Provider.provider.economyService.wearingResult == SteamInventoryResult_t.Invalid)
			{
				SystemNetPakWriterEx.WriteUInt16(writer, 0);
				return;
			}
			uint num;
			bool flag = SteamInventory.SerializeResult(Provider.provider.economyService.wearingResult, null, out num);
			if (flag && num <= 65535U)
			{
				byte[] array = new byte[num];
				if (!SteamInventory.SerializeResult(Provider.provider.economyService.wearingResult, array, out num))
				{
					UnturnedLog.warn("SteamInventory.SerializeResult returned false the second time");
				}
				SystemNetPakWriterEx.WriteUInt16(writer, (ushort)num);
				writer.WriteBytes(array);
				SteamInventory.DestroyResult(Provider.provider.economyService.wearingResult);
				Provider.provider.economyService.wearingResult = SteamInventoryResult_t.Invalid;
				return;
			}
			SteamInventory.DestroyResult(Provider.provider.economyService.wearingResult);
			Provider.provider.economyService.wearingResult = SteamInventoryResult_t.Invalid;
			Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_ECON_SERIALIZE;
			Provider.RequestDisconnect(flag ? "SteamInventory.SerializeResult length too large!" : "SteamInventory.SerializeResult failed");
		}
	}
}
