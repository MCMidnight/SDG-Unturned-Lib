using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006A7 RID: 1703
	public class ServerListComparer_CombatDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x0600392D RID: 14637 RVA: 0x0010CE47 File Offset: 0x0010B047
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.isPvP == rhs.isPvP)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			if (!lhs.isPvP)
			{
				return -1;
			}
			return 1;
		}
	}
}
