using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Multiplayer.Client;
using SDG.Provider.Services.Multiplayer.Server;
using SDG.SteamworksProvider.Services.Multiplayer.Client;
using SDG.SteamworksProvider.Services.Multiplayer.Server;

namespace SDG.SteamworksProvider.Services.Multiplayer
{
	// Token: 0x0200001D RID: 29
	public class SteamworksMultiplayerService : IMultiplayerService, IService
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00003A52 File Offset: 0x00001C52
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00003A5A File Offset: 0x00001C5A
		public IClientMultiplayerService clientMultiplayerService { get; protected set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00003A63 File Offset: 0x00001C63
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00003A6B File Offset: 0x00001C6B
		public IServerMultiplayerService serverMultiplayerService { get; protected set; }

		// Token: 0x06000098 RID: 152 RVA: 0x00003A74 File Offset: 0x00001C74
		public void initialize()
		{
			this.serverMultiplayerService.initialize();
			if (!this.appInfo.isDedicated)
			{
				this.clientMultiplayerService.initialize();
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003A99 File Offset: 0x00001C99
		public void update()
		{
			this.serverMultiplayerService.update();
			if (!this.appInfo.isDedicated)
			{
				this.clientMultiplayerService.update();
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003ABE File Offset: 0x00001CBE
		public void shutdown()
		{
			this.serverMultiplayerService.shutdown();
			if (!this.appInfo.isDedicated)
			{
				this.clientMultiplayerService.shutdown();
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003AE3 File Offset: 0x00001CE3
		public SteamworksMultiplayerService(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
			this.serverMultiplayerService = new SteamworksServerMultiplayerService(this.appInfo);
			if (!this.appInfo.isDedicated)
			{
				this.clientMultiplayerService = new SteamworksClientMultiplayerService();
			}
		}

		// Token: 0x04000046 RID: 70
		private SteamworksAppInfo appInfo;
	}
}
