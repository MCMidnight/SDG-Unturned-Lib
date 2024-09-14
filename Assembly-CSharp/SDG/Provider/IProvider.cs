using System;
using SDG.Provider.Services.Achievements;
using SDG.Provider.Services.Browser;
using SDG.Provider.Services.Cloud;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Statistics;
using SDG.Provider.Services.Store;
using SDG.Provider.Services.Translation;

namespace SDG.Provider
{
	// Token: 0x0200002E RID: 46
	public interface IProvider
	{
		/// <summary>
		/// Current achievements implementation.
		/// </summary>
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000116 RID: 278
		IAchievementsService achievementsService { get; }

		/// <summary>
		/// Current browser implementation.
		/// </summary>
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000117 RID: 279
		IBrowserService browserService { get; }

		/// <summary>
		/// Current cloud implementation.
		/// </summary>
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000118 RID: 280
		ICloudService cloudService { get; }

		/// <summary>
		/// Current community implementation.
		/// </summary>
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000119 RID: 281
		ICommunityService communityService { get; }

		/// <summary>
		/// Current economy implementation.
		/// </summary>
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600011A RID: 282
		TempSteamworksEconomy economyService { get; }

		/// <summary>
		/// Current matchmaking implementation.
		/// </summary>
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600011B RID: 283
		TempSteamworksMatchmaking matchmakingService { get; }

		/// <summary>
		/// Current multiplayer implementation.
		/// </summary>
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600011C RID: 284
		IMultiplayerService multiplayerService { get; }

		/// <summary>
		/// Current statistics implementation.
		/// </summary>
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600011D RID: 285
		IStatisticsService statisticsService { get; }

		/// <summary>
		/// Current store implementation.
		/// </summary>
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600011E RID: 286
		IStoreService storeService { get; }

		/// <summary>
		/// Current translation implementation.
		/// </summary>
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600011F RID: 287
		ITranslationService translationService { get; }

		/// <summary>
		/// Current workshop implementation.
		/// </summary>
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000120 RID: 288
		TempSteamworksWorkshop workshopService { get; }

		/// <summary>
		/// Initialize this provider's external API. Should be called before using provider features.
		/// </summary>
		/// <exception cref="T:System.Exception">Thrown if initializing fails.</exception>
		// Token: 0x06000121 RID: 289
		void intialize();

		/// <summary>
		/// Update this provider's external API. Should be called every frame if using provider features.
		/// </summary>
		// Token: 0x06000122 RID: 290
		void update();

		/// <summary>
		/// Shutdown this provider's external API. Should be called before closing the program if using provider features.
		/// </summary>
		// Token: 0x06000123 RID: 291
		void shutdown();
	}
}
