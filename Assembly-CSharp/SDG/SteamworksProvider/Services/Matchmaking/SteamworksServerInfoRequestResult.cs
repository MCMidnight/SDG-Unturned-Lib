using System;
using SDG.Provider.Services.Matchmaking;
using SDG.Provider.Services.Multiplayer;
using SDG.SteamworksProvider.Services.Multiplayer;

namespace SDG.SteamworksProvider.Services.Matchmaking
{
	// Token: 0x02000023 RID: 35
	public class SteamworksServerInfoRequestResult : IServerInfoRequestResult
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00004174 File Offset: 0x00002374
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x0000417C File Offset: 0x0000237C
		public IServerInfo serverInfo { get; protected set; }

		// Token: 0x060000D9 RID: 217 RVA: 0x00004185 File Offset: 0x00002385
		public SteamworksServerInfoRequestResult(SteamworksServerInfo newServerInfo)
		{
			this.serverInfo = newServerInfo;
		}
	}
}
