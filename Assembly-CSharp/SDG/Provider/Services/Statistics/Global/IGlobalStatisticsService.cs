using System;

namespace SDG.Provider.Services.Statistics.Global
{
	// Token: 0x02000045 RID: 69
	public interface IGlobalStatisticsService : IService
	{
		/// <summary>
		/// Triggered when the global statistics are available.
		/// </summary>
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060001DE RID: 478
		// (remove) Token: 0x060001DF RID: 479
		event GlobalStatisticsRequestReady onGlobalStatisticsRequestReady;

		/// <summary>
		/// Checks the global total of the statistic with this name.
		/// </summary>
		/// <param name="name">The name of the statistic.</param>
		/// <param name="data">The value of the statistic.</param>
		/// <returns>Whether the check succesfully executed.</returns>
		// Token: 0x060001E0 RID: 480
		bool getStatistic(string name, out long data);

		/// <summary>
		/// Checks the global total of the statistic with this name.
		/// </summary>
		/// <param name="name">The name of the statistic.</param>
		/// <param name="data">The value of the statistic.</param>
		/// <returns>Whether the check succesfully executed.</returns>
		// Token: 0x060001E1 RID: 481
		bool getStatistic(string name, out double data);

		/// <summary>
		/// Requests the global statistics.
		/// </summary>
		/// <returns>Whether the refresh succesfully executed.</returns>
		// Token: 0x060001E2 RID: 482
		bool requestStatistics();
	}
}
