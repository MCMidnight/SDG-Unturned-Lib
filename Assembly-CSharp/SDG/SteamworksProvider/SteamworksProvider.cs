using System;
using SDG.Provider;
using SDG.Provider.Services.Achievements;
using SDG.Provider.Services.Browser;
using SDG.Provider.Services.Cloud;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Statistics;
using SDG.Provider.Services.Store;
using SDG.Provider.Services.Translation;
using SDG.SteamworksProvider.Services.Achievements;
using SDG.SteamworksProvider.Services.Browser;
using SDG.SteamworksProvider.Services.Cloud;
using SDG.SteamworksProvider.Services.Community;
using SDG.SteamworksProvider.Services.Multiplayer;
using SDG.SteamworksProvider.Services.Statistics;
using SDG.SteamworksProvider.Services.Store;
using SDG.SteamworksProvider.Services.Translation;
using Steamworks;

namespace SDG.SteamworksProvider
{
	// Token: 0x02000015 RID: 21
	public class SteamworksProvider : IProvider
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003265 File Offset: 0x00001465
		// (set) Token: 0x06000051 RID: 81 RVA: 0x0000326D File Offset: 0x0000146D
		public IAchievementsService achievementsService { get; protected set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003276 File Offset: 0x00001476
		// (set) Token: 0x06000053 RID: 83 RVA: 0x0000327E File Offset: 0x0000147E
		public IBrowserService browserService { get; protected set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003287 File Offset: 0x00001487
		// (set) Token: 0x06000055 RID: 85 RVA: 0x0000328F File Offset: 0x0000148F
		public ICloudService cloudService { get; protected set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003298 File Offset: 0x00001498
		// (set) Token: 0x06000057 RID: 87 RVA: 0x000032A0 File Offset: 0x000014A0
		public ICommunityService communityService { get; protected set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000032A9 File Offset: 0x000014A9
		// (set) Token: 0x06000059 RID: 89 RVA: 0x000032B1 File Offset: 0x000014B1
		public TempSteamworksEconomy economyService { get; protected set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000032BA File Offset: 0x000014BA
		// (set) Token: 0x0600005B RID: 91 RVA: 0x000032C2 File Offset: 0x000014C2
		public TempSteamworksMatchmaking matchmakingService { get; protected set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000032CB File Offset: 0x000014CB
		// (set) Token: 0x0600005D RID: 93 RVA: 0x000032D3 File Offset: 0x000014D3
		public IMultiplayerService multiplayerService { get; protected set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000032DC File Offset: 0x000014DC
		// (set) Token: 0x0600005F RID: 95 RVA: 0x000032E4 File Offset: 0x000014E4
		public IStatisticsService statisticsService { get; protected set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000060 RID: 96 RVA: 0x000032ED File Offset: 0x000014ED
		// (set) Token: 0x06000061 RID: 97 RVA: 0x000032F5 File Offset: 0x000014F5
		public IStoreService storeService { get; protected set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000032FE File Offset: 0x000014FE
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00003306 File Offset: 0x00001506
		public ITranslationService translationService { get; protected set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000064 RID: 100 RVA: 0x0000330F File Offset: 0x0000150F
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00003317 File Offset: 0x00001517
		public TempSteamworksWorkshop workshopService { get; protected set; }

		// Token: 0x06000066 RID: 102 RVA: 0x00003320 File Offset: 0x00001520
		public void intialize()
		{
			if (!this.appInfo.isDedicated)
			{
				if (SteamAPI.RestartAppIfNecessary((AppId_t)this.appInfo.id))
				{
					throw new Exception("Restarting app from Steam.");
				}
				if (!SteamAPI.Init())
				{
					throw new Exception("Steam API initialization failed.");
				}
			}
			this.initializeServices();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003374 File Offset: 0x00001574
		public void update()
		{
			if (this.multiplayerService.serverMultiplayerService.isHosting)
			{
				GameServer.RunCallbacks();
			}
			if (!this.appInfo.isDedicated)
			{
				SteamAPI.RunCallbacks();
			}
			this.updateServices();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000033A5 File Offset: 0x000015A5
		public void shutdown()
		{
			if (!this.appInfo.isDedicated)
			{
				SteamAPI.Shutdown();
			}
			this.shutdownServices();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000033C0 File Offset: 0x000015C0
		private void constructServices()
		{
			this.achievementsService = new SteamworksAchievementsService();
			this.economyService = new TempSteamworksEconomy(this.appInfo);
			this.multiplayerService = new SteamworksMultiplayerService(this.appInfo);
			this.statisticsService = new SteamworksStatisticsService();
			this.workshopService = new TempSteamworksWorkshop(this.appInfo);
			if (!this.appInfo.isDedicated)
			{
				this.browserService = new SteamworksBrowserService();
				this.cloudService = new SteamworksCloudService();
				this.communityService = new SteamworksCommunityService();
				this.matchmakingService = new TempSteamworksMatchmaking();
				this.storeService = new SteamworksStoreService(this.appInfo);
				this.translationService = new SteamworksTranslationService();
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000346C File Offset: 0x0000166C
		private void initializeServices()
		{
			if (this.achievementsService != null)
			{
				this.achievementsService.initialize();
			}
			if (this.multiplayerService != null)
			{
				this.multiplayerService.initialize();
			}
			if (this.statisticsService != null)
			{
				this.statisticsService.initialize();
			}
			if (!this.appInfo.isDedicated)
			{
				if (this.browserService != null)
				{
					this.browserService.initialize();
				}
				if (this.cloudService != null)
				{
					this.cloudService.initialize();
				}
				if (this.communityService != null)
				{
					this.communityService.initialize();
				}
				if (this.economyService != null)
				{
					this.economyService.initializeClient();
				}
				if (this.storeService != null)
				{
					this.storeService.initialize();
				}
				if (this.translationService != null)
				{
					this.translationService.initialize();
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003534 File Offset: 0x00001734
		private void updateServices()
		{
			if (this.achievementsService != null)
			{
				this.achievementsService.update();
			}
			if (this.multiplayerService != null)
			{
				this.multiplayerService.update();
			}
			if (this.statisticsService != null)
			{
				this.statisticsService.update();
			}
			if (this.workshopService != null)
			{
				this.workshopService.update();
			}
			if (!this.appInfo.isDedicated)
			{
				if (this.browserService != null)
				{
					this.browserService.update();
				}
				if (this.cloudService != null)
				{
					this.cloudService.update();
				}
				if (this.communityService != null)
				{
					this.communityService.update();
				}
				if (this.storeService != null)
				{
					this.storeService.update();
				}
				if (this.translationService != null)
				{
					this.translationService.update();
				}
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000035FC File Offset: 0x000017FC
		private void shutdownServices()
		{
			if (this.achievementsService != null)
			{
				this.achievementsService.shutdown();
			}
			if (this.multiplayerService != null)
			{
				this.multiplayerService.shutdown();
			}
			if (this.statisticsService != null)
			{
				this.statisticsService.shutdown();
			}
			if (!this.appInfo.isDedicated)
			{
				if (this.browserService != null)
				{
					this.browserService.shutdown();
				}
				if (this.cloudService != null)
				{
					this.cloudService.shutdown();
				}
				if (this.communityService != null)
				{
					this.communityService.shutdown();
				}
				if (this.storeService != null)
				{
					this.storeService.shutdown();
				}
				if (this.translationService != null)
				{
					this.translationService.shutdown();
				}
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000036AE File Offset: 0x000018AE
		public SteamworksProvider(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
			this.constructServices();
		}

		// Token: 0x0400003A RID: 58
		private SteamworksAppInfo appInfo;
	}
}
