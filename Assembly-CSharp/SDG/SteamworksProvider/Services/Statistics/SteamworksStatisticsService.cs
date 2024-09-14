using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Statistics;
using SDG.Provider.Services.Statistics.Global;
using SDG.Provider.Services.Statistics.User;
using SDG.SteamworksProvider.Services.Statistics.Global;
using SDG.SteamworksProvider.Services.Statistics.User;

namespace SDG.SteamworksProvider.Services.Statistics
{
	// Token: 0x0200001A RID: 26
	public class SteamworksStatisticsService : IStatisticsService, IService
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007A RID: 122 RVA: 0x0000378A File Offset: 0x0000198A
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00003792 File Offset: 0x00001992
		public IUserStatisticsService userStatisticsService { get; protected set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007C RID: 124 RVA: 0x0000379B File Offset: 0x0000199B
		// (set) Token: 0x0600007D RID: 125 RVA: 0x000037A3 File Offset: 0x000019A3
		public IGlobalStatisticsService globalStatisticsService { get; protected set; }

		// Token: 0x0600007E RID: 126 RVA: 0x000037AC File Offset: 0x000019AC
		public void initialize()
		{
			this.userStatisticsService.initialize();
			this.globalStatisticsService.initialize();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000037C4 File Offset: 0x000019C4
		public void update()
		{
			this.userStatisticsService.update();
			this.globalStatisticsService.update();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000037DC File Offset: 0x000019DC
		public void shutdown()
		{
			this.userStatisticsService.shutdown();
			this.globalStatisticsService.shutdown();
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000037F4 File Offset: 0x000019F4
		public SteamworksStatisticsService()
		{
			this.userStatisticsService = new SteamworksUserStatisticsService();
			this.globalStatisticsService = new SteamworksGlobalStatisticsService();
		}
	}
}
