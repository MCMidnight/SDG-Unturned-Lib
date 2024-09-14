using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006AF RID: 1711
	public class ServerListComparer_CheatsDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x0600393D RID: 14653 RVA: 0x0010CF67 File Offset: 0x0010B167
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.hasCheats == rhs.hasCheats)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			if (!lhs.hasCheats)
			{
				return -1;
			}
			return 1;
		}
	}
}
