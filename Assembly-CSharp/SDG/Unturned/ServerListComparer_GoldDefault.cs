using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006AD RID: 1709
	public class ServerListComparer_GoldDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003939 RID: 14649 RVA: 0x0010CF1F File Offset: 0x0010B11F
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.isPro == rhs.isPro)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			if (!lhs.isPro)
			{
				return 1;
			}
			return -1;
		}
	}
}
