using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006A9 RID: 1705
	public class ServerListComparer_PasswordDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003931 RID: 14641 RVA: 0x0010CE8F File Offset: 0x0010B08F
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.isPassworded == rhs.isPassworded)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			if (!lhs.isPassworded)
			{
				return -1;
			}
			return 1;
		}
	}
}
