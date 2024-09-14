using System;
using SDG.Provider.Services.Matchmaking;
using SDG.SteamworksProvider.Services.Multiplayer;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Matchmaking
{
	// Token: 0x02000022 RID: 34
	public class SteamworksServerInfoRequestHandle : IServerInfoRequestHandle
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x000040D4 File Offset: 0x000022D4
		public void onServerResponded(gameserveritem_t server)
		{
			SteamworksServerInfoRequestResult result = new SteamworksServerInfoRequestResult(new SteamworksServerInfo(server));
			this.triggerCallback(result);
			this.cleanupQuery();
			SteamworksMatchmakingService.serverInfoRequestHandles.Remove(this);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004108 File Offset: 0x00002308
		public void onServerFailedToRespond()
		{
			SteamworksServerInfoRequestResult result = new SteamworksServerInfoRequestResult(null);
			this.triggerCallback(result);
			this.cleanupQuery();
			SteamworksMatchmakingService.serverInfoRequestHandles.Remove(this);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004135 File Offset: 0x00002335
		public void triggerCallback(IServerInfoRequestResult result)
		{
			if (this.callback == null)
			{
				return;
			}
			this.callback(this, result);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000414D File Offset: 0x0000234D
		private void cleanupQuery()
		{
			SteamMatchmakingServers.CancelServerQuery(this.query);
			this.query = HServerQuery.Invalid;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004165 File Offset: 0x00002365
		public SteamworksServerInfoRequestHandle(ServerInfoRequestReadyCallback newCallback)
		{
			this.callback = newCallback;
		}

		// Token: 0x0400005F RID: 95
		public HServerQuery query;

		// Token: 0x04000060 RID: 96
		public ISteamMatchmakingPingResponse pingResponse;

		// Token: 0x04000061 RID: 97
		private ServerInfoRequestReadyCallback callback;
	}
}
