using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006B3 RID: 1715
	public class ServerListComparer_PluginsDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003945 RID: 14661 RVA: 0x0010D05C File Offset: 0x0010B25C
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.pluginFramework == rhs.pluginFramework)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			int num = this.orderMap[(int)lhs.pluginFramework];
			int num2 = this.orderMap[(int)rhs.pluginFramework];
			return num - num2;
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x0010D0A6 File Offset: 0x0010B2A6
		public ServerListComparer_PluginsDefault()
		{
			this.orderMap = new int[4];
			this.orderMap[0] = 0;
			this.orderMap[1] = 1;
			this.orderMap[2] = 1;
			this.orderMap[3] = 1;
		}

		// Token: 0x040021F8 RID: 8696
		private int[] orderMap;
	}
}
