using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by ping low to high.
	/// </summary>
	// Token: 0x020006A1 RID: 1697
	public class ServerListComparer_PingAscending : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003921 RID: 14625 RVA: 0x0010CCFC File Offset: 0x0010AEFC
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.sortingPing != rhs.sortingPing)
			{
				return lhs.sortingPing - rhs.sortingPing;
			}
			if (lhs.players == rhs.players)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			return rhs.players - lhs.players;
		}
	}
}
