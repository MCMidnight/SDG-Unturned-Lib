using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006B1 RID: 1713
	public class ServerListComparer_MonetizationDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003941 RID: 14657 RVA: 0x0010CFB0 File Offset: 0x0010B1B0
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.monetization == rhs.monetization)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			int num = this.orderMap[(int)lhs.monetization];
			int num2 = this.orderMap[(int)rhs.monetization];
			return num - num2;
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x0010CFFC File Offset: 0x0010B1FC
		public ServerListComparer_MonetizationDefault()
		{
			this.orderMap = new int[5];
			this.orderMap[2] = 0;
			this.orderMap[3] = 1;
			this.orderMap[0] = 2;
			this.orderMap[4] = 3;
			this.orderMap[1] = 4;
		}

		// Token: 0x040021F7 RID: 8695
		private int[] orderMap;
	}
}
