using System;
using SDG.Provider.Services.Statistics.Global;
using SDG.Provider.Services.Statistics.User;

namespace SDG.Provider.Services.Statistics
{
	// Token: 0x02000041 RID: 65
	public interface IStatisticsService : IService
	{
		/// <summary>
		/// Current user statistics implementation.
		/// </summary>
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001CD RID: 461
		IUserStatisticsService userStatisticsService { get; }

		/// <summary>
		/// Current global statistics implementation.
		/// </summary>
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001CE RID: 462
		IGlobalStatisticsService globalStatisticsService { get; }
	}
}
