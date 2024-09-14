using System;
using System.Collections.Generic;
using SDG.Provider.Services;
using SDG.Provider.Services.Matchmaking;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Matchmaking
{
	// Token: 0x02000021 RID: 33
	public class SteamworksMatchmakingService : Service, IMatchmakingService, IService
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x00004074 File Offset: 0x00002274
		public IServerInfoRequestHandle requestServerInfo(uint ip, ushort port, ServerInfoRequestReadyCallback callback)
		{
			SteamworksServerInfoRequestHandle steamworksServerInfoRequestHandle = new SteamworksServerInfoRequestHandle(callback);
			ISteamMatchmakingPingResponse steamMatchmakingPingResponse = new ISteamMatchmakingPingResponse(new ISteamMatchmakingPingResponse.ServerResponded(steamworksServerInfoRequestHandle.onServerResponded), new ISteamMatchmakingPingResponse.ServerFailedToRespond(steamworksServerInfoRequestHandle.onServerFailedToRespond));
			steamworksServerInfoRequestHandle.pingResponse = steamMatchmakingPingResponse;
			HServerQuery query = SteamMatchmakingServers.PingServer(ip, port + 1, steamMatchmakingPingResponse);
			steamworksServerInfoRequestHandle.query = query;
			SteamworksMatchmakingService.serverInfoRequestHandles.Add(steamworksServerInfoRequestHandle);
			return steamworksServerInfoRequestHandle;
		}

		// Token: 0x0400005E RID: 94
		public static List<SteamworksServerInfoRequestHandle> serverInfoRequestHandles;
	}
}
