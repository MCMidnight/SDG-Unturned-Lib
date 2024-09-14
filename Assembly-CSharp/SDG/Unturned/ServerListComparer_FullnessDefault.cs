using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by normalized player count high to low.
	/// </summary>
	// Token: 0x0200069F RID: 1695
	public class ServerListComparer_FullnessDefault : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x0600391D RID: 14621 RVA: 0x0010CC80 File Offset: 0x0010AE80
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			float normalizedPlayerCount = lhs.NormalizedPlayerCount;
			float normalizedPlayerCount2 = rhs.NormalizedPlayerCount;
			if (!MathfEx.IsNearlyEqual(normalizedPlayerCount, normalizedPlayerCount2, 0.01f))
			{
				return normalizedPlayerCount2.CompareTo(normalizedPlayerCount);
			}
			if (lhs.players == rhs.players)
			{
				return lhs.name.CompareTo(rhs.name);
			}
			return rhs.players - lhs.players;
		}
	}
}
