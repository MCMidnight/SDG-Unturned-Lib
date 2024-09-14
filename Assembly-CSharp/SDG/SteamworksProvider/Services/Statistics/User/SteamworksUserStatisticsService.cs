using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Statistics.User;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Statistics.User
{
	// Token: 0x0200001B RID: 27
	public class SteamworksUserStatisticsService : Service, IUserStatisticsService, IService
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000082 RID: 130 RVA: 0x00003814 File Offset: 0x00001A14
		// (remove) Token: 0x06000083 RID: 131 RVA: 0x0000384C File Offset: 0x00001A4C
		public event UserStatisticsRequestReady onUserStatisticsRequestReady;

		// Token: 0x06000084 RID: 132 RVA: 0x00003881 File Offset: 0x00001A81
		private void triggerUserStatisticsRequestReady(ICommunityEntity entityID)
		{
			UserStatisticsRequestReady userStatisticsRequestReady = this.onUserStatisticsRequestReady;
			if (userStatisticsRequestReady == null)
			{
				return;
			}
			userStatisticsRequestReady(entityID);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003894 File Offset: 0x00001A94
		public bool getStatistic(string name, out int data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return SteamUserStats.GetStat(name, out data);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000038AB File Offset: 0x00001AAB
		public bool setStatistic(string name, int data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			bool result = SteamUserStats.SetStat(name, data);
			SteamUserStats.StoreStats();
			return result;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000038C8 File Offset: 0x00001AC8
		public bool getStatistic(string name, out float data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return SteamUserStats.GetStat(name, out data);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000038DF File Offset: 0x00001ADF
		public bool setStatistic(string name, float data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			bool result = SteamUserStats.SetStat(name, data);
			SteamUserStats.StoreStats();
			return result;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000038FC File Offset: 0x00001AFC
		public bool requestStatistics()
		{
			SteamUserStats.RequestCurrentStats();
			return true;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003905 File Offset: 0x00001B05
		public SteamworksUserStatisticsService()
		{
			this.userStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.onUserStatsReceived));
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003924 File Offset: 0x00001B24
		private void onUserStatsReceived(UserStatsReceived_t callback)
		{
			if (callback.m_nGameID != (ulong)SteamUtils.GetAppID().m_AppId)
			{
				return;
			}
			SteamworksCommunityEntity steamworksCommunityEntity = new SteamworksCommunityEntity(callback.m_steamIDUser);
			this.triggerUserStatisticsRequestReady(steamworksCommunityEntity);
		}

		// Token: 0x04000041 RID: 65
		private Callback<UserStatsReceived_t> userStatsReceivedCallback;
	}
}
