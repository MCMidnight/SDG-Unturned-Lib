using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by player count high to low.
	/// </summary>
	// Token: 0x0200069B RID: 1691
	public class ServerListComparer_PlayersDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003915 RID: 14613 RVA: 0x0010CBC2 File Offset: 0x0010ADC2
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.players == rhs.players)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			return rhs.players - lhs.players;
		}
	}
}
