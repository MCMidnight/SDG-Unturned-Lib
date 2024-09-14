using System;

namespace SDG.Provider.Services.Statistics.User
{
	// Token: 0x02000042 RID: 66
	public interface IUserStatisticsService : IService
	{
		/// <summary>
		/// Triggered when the user's statistics are available.
		/// </summary>
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060001CF RID: 463
		// (remove) Token: 0x060001D0 RID: 464
		event UserStatisticsRequestReady onUserStatisticsRequestReady;

		/// <summary>
		/// Checks the current user's statistics with this name.
		/// </summary>
		/// <param name="name">The name of the statistic.</param>
		/// <param name="data">The value of the statistic.</param>
		/// <returns>Whether the check succesfully executed.</returns>
		// Token: 0x060001D1 RID: 465
		bool getStatistic(string name, out int data);

		/// <summary>
		/// Assigns the current user's statistics with this name.
		/// </summary>
		/// <param name="name">The name of the statistic.</param>
		/// <param name="data">The value of the statistic.</param>
		/// <returns>Whether the check succesfully executed.</returns>
		// Token: 0x060001D2 RID: 466
		bool setStatistic(string name, int data);

		/// <summary>
		/// Checks the current user's statistics with this name.
		/// </summary>
		/// <param name="name">The name of the statistic.</param>
		/// <param name="data">The value of the statistic.</param>
		/// <returns>Whether the check succesfully executed.</returns>
		// Token: 0x060001D3 RID: 467
		bool getStatistic(string name, out float data);

		/// <summary>
		/// Assigns the current user's statistics with this name.
		/// </summary>
		/// <param name="name">The name of the statistic.</param>
		/// <param name="data">The value of the statistic.</param>
		/// <returns>Whether the check succesfully executed.</returns>
		// Token: 0x060001D4 RID: 468
		bool setStatistic(string name, float data);

		/// <summary>
		/// Requests the user's statistics.
		/// </summary>
		/// <returns>Whether the refresh succesfully executed.</returns>
		// Token: 0x060001D5 RID: 469
		bool requestStatistics();
	}
}
