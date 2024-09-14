using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Statistics.Global;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Statistics.Global
{
	// Token: 0x0200001C RID: 28
	public class SteamworksGlobalStatisticsService : Service, IGlobalStatisticsService, IService
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600008C RID: 140 RVA: 0x00003960 File Offset: 0x00001B60
		// (remove) Token: 0x0600008D RID: 141 RVA: 0x00003998 File Offset: 0x00001B98
		public event GlobalStatisticsRequestReady onGlobalStatisticsRequestReady;

		// Token: 0x0600008E RID: 142 RVA: 0x000039CD File Offset: 0x00001BCD
		private void triggerGlobalStatisticsRequestReady()
		{
			GlobalStatisticsRequestReady globalStatisticsRequestReady = this.onGlobalStatisticsRequestReady;
			if (globalStatisticsRequestReady == null)
			{
				return;
			}
			globalStatisticsRequestReady();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000039DF File Offset: 0x00001BDF
		public bool getStatistic(string name, out long data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return SteamUserStats.GetGlobalStat(name, out data);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000039F6 File Offset: 0x00001BF6
		public bool getStatistic(string name, out double data)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return SteamUserStats.GetGlobalStat(name, out data);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003A0D File Offset: 0x00001C0D
		public bool requestStatistics()
		{
			SteamUserStats.RequestGlobalStats(0);
			return true;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003A17 File Offset: 0x00001C17
		public SteamworksGlobalStatisticsService()
		{
			this.globalStatsReceived = Callback<GlobalStatsReceived_t>.Create(new Callback<GlobalStatsReceived_t>.DispatchDelegate(this.onGlobalStatsReceived));
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003A36 File Offset: 0x00001C36
		private void onGlobalStatsReceived(GlobalStatsReceived_t callback)
		{
			if (callback.m_nGameID != (ulong)SteamUtils.GetAppID().m_AppId)
			{
				return;
			}
			this.triggerGlobalStatisticsRequestReady();
		}

		// Token: 0x04000043 RID: 67
		private Callback<GlobalStatsReceived_t> globalStatsReceived;
	}
}
