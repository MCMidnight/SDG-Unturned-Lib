using System;

namespace SDG.Unturned
{
	// Token: 0x020006ED RID: 1773
	public class StatusData
	{
		// Token: 0x06003B0F RID: 15119 RVA: 0x00114598 File Offset: 0x00112798
		public StatusData()
		{
			this.Achievements = new AchievementStatusData();
			this.Game = new GameStatusData();
			this.Holidays = new HolidayStatusData();
			this.Menu = new MenuStatusData();
			this.News = new NewsStatusData();
			this.Maps = new MapsStatusData();
		}

		// Token: 0x040024E3 RID: 9443
		public AchievementStatusData Achievements;

		// Token: 0x040024E4 RID: 9444
		public GameStatusData Game;

		// Token: 0x040024E5 RID: 9445
		public HolidayStatusData Holidays;

		// Token: 0x040024E6 RID: 9446
		public MenuStatusData Menu;

		// Token: 0x040024E7 RID: 9447
		public NewsStatusData News;

		// Token: 0x040024E8 RID: 9448
		public MapsStatusData Maps;
	}
}
