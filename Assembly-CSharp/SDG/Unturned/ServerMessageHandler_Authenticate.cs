using System;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000278 RID: 632
	internal static class ServerMessageHandler_Authenticate
	{
		// Token: 0x06001281 RID: 4737 RVA: 0x00040A4C File Offset: 0x0003EC4C
		internal static void ReadMessage(ITransportConnection transportConnection, NetPakReader reader)
		{
			SteamPending steamPending = Provider.findPendingPlayer(transportConnection);
			if (steamPending == null || !steamPending.hasSentVerifyPacket)
			{
				Provider.reject(transportConnection, ESteamRejection.NOT_PENDING);
				return;
			}
			if (Provider.clients.Count + 1 > (int)Provider.maxPlayers)
			{
				Provider.reject(transportConnection, ESteamRejection.SERVER_FULL);
				return;
			}
			UnturnedLog.info(string.Format("Received authentication request from queued player {0}", steamPending.playerID));
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num);
			byte[] array = new byte[(int)num];
			reader.ReadBytes(array);
			if (Dedicator.offlineOnly)
			{
				steamPending.assignedPro = steamPending.isPro;
				steamPending.assignedAdmin = SteamAdminlist.checkAdmin(steamPending.playerID.steamID);
				steamPending.hasAuthentication = true;
				UnturnedLog.info(string.Format("Skipping Steam authentication for queued player {0} because we are running offline-only", steamPending.playerID));
			}
			else
			{
				if (!Provider.verifyTicket(steamPending.playerID.steamID, array))
				{
					Provider.reject(transportConnection, ESteamRejection.AUTH_VERIFICATION);
					return;
				}
				UnturnedLog.info(string.Format("Submitted Steam authentication request for queued player {0}", steamPending.playerID));
			}
			if (!ServerMessageHandler_Authenticate.ReadEconomyDetails(steamPending, reader))
			{
				return;
			}
			if (steamPending.playerID.group == CSteamID.Nil || Dedicator.offlineOnly)
			{
				steamPending.hasGroup = true;
			}
			else if (!SteamGameServer.RequestUserGroupStatus(steamPending.playerID.steamID, steamPending.playerID.group))
			{
				steamPending.playerID.group = CSteamID.Nil;
				steamPending.hasGroup = true;
			}
			else
			{
				UnturnedLog.info(string.Format("Submitted Steam group request for queued player {0}", steamPending.playerID));
			}
			if (steamPending.canAcceptYet)
			{
				Provider.accept(steamPending);
			}
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00040BC8 File Offset: 0x0003EDC8
		private static bool ReadEconomyDetails(SteamPending player, NetPakReader reader)
		{
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num);
			if (num > 0)
			{
				byte[] array = new byte[(int)num];
				reader.ReadBytes(array);
				if (!SteamGameServerInventory.DeserializeResult(out player.inventoryResult, array, (uint)num, false))
				{
					Provider.reject(player.transportConnection, ESteamRejection.AUTH_ECON_DESERIALIZE);
					return false;
				}
			}
			else
			{
				player.shirtItem = 0;
				player.pantsItem = 0;
				player.hatItem = 0;
				player.backpackItem = 0;
				player.vestItem = 0;
				player.maskItem = 0;
				player.glassesItem = 0;
				player.skinItems = new int[0];
				player.skinTags = new string[0];
				player.skinDynamicProps = new string[0];
				player.packageShirt = 0UL;
				player.packagePants = 0UL;
				player.packageHat = 0UL;
				player.packageBackpack = 0UL;
				player.packageVest = 0UL;
				player.packageMask = 0UL;
				player.packageGlasses = 0UL;
				player.packageSkins = new ulong[0];
				player.inventoryResult = SteamInventoryResult_t.Invalid;
				player.inventoryDetails = new SteamItemDetails_t[0];
				player.hasProof = true;
			}
			return true;
		}
	}
}
