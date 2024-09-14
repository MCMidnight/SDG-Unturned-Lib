using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by max player count high to low.
	/// </summary>
	// Token: 0x0200069D RID: 1693
	public class ServerListComparer_MaxPlayersDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003919 RID: 14617 RVA: 0x0010CC0C File Offset: 0x0010AE0C
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (lhs.maxPlayers != rhs.maxPlayers)
			{
				return rhs.maxPlayers - lhs.maxPlayers;
			}
			if (lhs.players == rhs.players)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			return rhs.players - lhs.players;
		}
	}
}
