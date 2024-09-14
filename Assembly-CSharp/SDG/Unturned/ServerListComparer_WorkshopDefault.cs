using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006AB RID: 1707
	public class ServerListComparer_WorkshopDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003935 RID: 14645 RVA: 0x0010CED7 File Offset: 0x0010B0D7
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.isWorkshop == rhs.isWorkshop)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			if (!lhs.isWorkshop)
			{
				return -1;
			}
			return 1;
		}
	}
}
