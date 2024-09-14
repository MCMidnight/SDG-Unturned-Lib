using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000272 RID: 626
	internal static class ClientMessageHandler_Rejected
	{
		// Token: 0x06001273 RID: 4723 RVA: 0x0003FF34 File Offset: 0x0003E134
		internal static void ReadMessage(NetPakReader reader)
		{
			Provider.isWaitingForConnectResponse = false;
			ESteamRejection esteamRejection;
			reader.ReadEnum(out esteamRejection);
			string text;
			SystemNetPakReaderEx.ReadString(reader, ref text, 11);
			Provider._connectionFailureReason = string.Empty;
			if (esteamRejection == ESteamRejection.WHITELISTED)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.WHITELISTED;
			}
			else if (esteamRejection == ESteamRejection.WRONG_PASSWORD)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PASSWORD;
			}
			else if (esteamRejection == ESteamRejection.SERVER_FULL)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.FULL;
			}
			else if (esteamRejection == ESteamRejection.WRONG_HASH_LEVEL)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.HASH_LEVEL;
			}
			else if (esteamRejection == ESteamRejection.WRONG_HASH_ASSEMBLY)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.HASH_ASSEMBLY;
			}
			else if (esteamRejection == ESteamRejection.WRONG_VERSION)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.VERSION;
				Provider._connectionFailureReason = text;
			}
			else if (esteamRejection == ESteamRejection.PRO_SERVER)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO_SERVER;
			}
			else if (esteamRejection == ESteamRejection.PRO_CHARACTER)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO_CHARACTER;
			}
			else if (esteamRejection == ESteamRejection.PRO_DESYNC)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO_DESYNC;
			}
			else if (esteamRejection == ESteamRejection.PRO_APPEARANCE)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO_APPEARANCE;
			}
			else if (esteamRejection == ESteamRejection.AUTH_VERIFICATION)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_VERIFICATION;
			}
			else if (esteamRejection == ESteamRejection.AUTH_NO_STEAM)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_NO_STEAM;
			}
			else if (esteamRejection == ESteamRejection.AUTH_LICENSE_EXPIRED)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_LICENSE_EXPIRED;
			}
			else if (esteamRejection == ESteamRejection.AUTH_VAC_BAN)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_VAC_BAN;
			}
			else if (esteamRejection == ESteamRejection.AUTH_ELSEWHERE)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_ELSEWHERE;
			}
			else if (esteamRejection == ESteamRejection.AUTH_TIMED_OUT)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_TIMED_OUT;
			}
			else if (esteamRejection == ESteamRejection.AUTH_USED)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_USED;
			}
			else if (esteamRejection == ESteamRejection.AUTH_NO_USER)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_NO_USER;
			}
			else if (esteamRejection == ESteamRejection.AUTH_PUB_BAN)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_PUB_BAN;
			}
			else if (esteamRejection == ESteamRejection.AUTH_NETWORK_IDENTITY_FAILURE)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_NETWORK_IDENTITY_FAILURE;
			}
			else if (esteamRejection == ESteamRejection.AUTH_ECON_DESERIALIZE)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_ECON_DESERIALIZE;
			}
			else if (esteamRejection == ESteamRejection.AUTH_ECON_VERIFY)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_ECON_VERIFY;
			}
			else if (esteamRejection == ESteamRejection.ALREADY_CONNECTED)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.ALREADY_CONNECTED;
			}
			else if (esteamRejection == ESteamRejection.ALREADY_PENDING)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.ALREADY_PENDING;
			}
			else if (esteamRejection == ESteamRejection.LATE_PENDING)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.LATE_PENDING;
			}
			else if (esteamRejection == ESteamRejection.NOT_PENDING)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NOT_PENDING;
			}
			else if (esteamRejection == ESteamRejection.NAME_PLAYER_SHORT)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_SHORT;
			}
			else if (esteamRejection == ESteamRejection.NAME_PLAYER_LONG)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_LONG;
			}
			else if (esteamRejection == ESteamRejection.NAME_PLAYER_INVALID)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_INVALID;
			}
			else if (esteamRejection == ESteamRejection.NAME_PLAYER_NUMBER)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_NUMBER;
			}
			else if (esteamRejection == ESteamRejection.NAME_CHARACTER_SHORT)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_SHORT;
			}
			else if (esteamRejection == ESteamRejection.NAME_CHARACTER_LONG)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_LONG;
			}
			else if (esteamRejection == ESteamRejection.NAME_CHARACTER_INVALID)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_INVALID;
			}
			else if (esteamRejection == ESteamRejection.NAME_CHARACTER_NUMBER)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_NUMBER;
			}
			else if (esteamRejection == ESteamRejection.PING)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PING;
				if (MenuDashboardUI.localization.has("PingV2"))
				{
					int num = (Provider.CurrentServerAdvertisement != null) ? Provider.CurrentServerAdvertisement.ping : -1;
					Provider._connectionFailureReason = MenuDashboardUI.localization.format("PingV2", num, text);
				}
				else
				{
					Provider._connectionFailureReason = MenuDashboardUI.localization.format("Ping");
				}
			}
			else if (esteamRejection == ESteamRejection.PLUGIN)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PLUGIN;
				Provider._connectionFailureReason = text;
			}
			else if (esteamRejection == ESteamRejection.CLIENT_MODULE_DESYNC)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.CLIENT_MODULE_DESYNC;
			}
			else if (esteamRejection == ESteamRejection.SERVER_MODULE_DESYNC)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.SERVER_MODULE_DESYNC;
			}
			else if (esteamRejection == ESteamRejection.WRONG_LEVEL_VERSION)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.LEVEL_VERSION;
				Provider._connectionFailureReason = MenuDashboardUI.localization.format("Level_Version", text, Level.info.getLocalizedName(), Level.version);
			}
			else if (esteamRejection == ESteamRejection.WRONG_HASH_ECON)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.ECON_HASH;
			}
			else if (esteamRejection == ESteamRejection.WRONG_HASH_MASTER_BUNDLE)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.HASH_MASTER_BUNDLE;
				Provider._connectionFailureReason = text;
			}
			else if (esteamRejection == ESteamRejection.LATE_PENDING_STEAM_AUTH)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.LATE_PENDING_STEAM_AUTH;
			}
			else if (esteamRejection == ESteamRejection.LATE_PENDING_STEAM_ECON)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.LATE_PENDING_STEAM_ECON;
			}
			else if (esteamRejection == ESteamRejection.LATE_PENDING_STEAM_GROUPS)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.LATE_PENDING_STEAM_GROUPS;
			}
			else if (esteamRejection == ESteamRejection.NAME_PRIVATE_LONG)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PRIVATE_LONG;
			}
			else if (esteamRejection == ESteamRejection.NAME_PRIVATE_INVALID)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PRIVATE_INVALID;
			}
			else if (esteamRejection == ESteamRejection.NAME_PRIVATE_NUMBER)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PRIVATE_NUMBER;
			}
			else if (esteamRejection == ESteamRejection.WRONG_HASH_RESOURCES)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.HASH_RESOURCES;
			}
			else if (esteamRejection == ESteamRejection.SKIN_COLOR_WITHIN_THRESHOLD_OF_TERRAIN_COLOR)
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.SKIN_COLOR_WITHIN_THRESHOLD_OF_TERRAIN_COLOR;
			}
			else
			{
				Provider._connectionFailureInfo = ESteamConnectionFailureInfo.REJECT_UNKNOWN;
				Provider._connectionFailureReason = esteamRejection.ToString();
			}
			Provider.RequestDisconnect(string.Format("Rejected by server ({0}) --- Reason: \"{1}\" Explanation: \"{2}\"", esteamRejection, Provider.connectionFailureReason, text));
		}
	}
}
