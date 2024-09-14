using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006B5 RID: 1717
	public class ServerListComparer_UtilityScore : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003949 RID: 14665 RVA: 0x0010D0F4 File Offset: 0x0010B2F4
		public int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (MathfEx.IsNearlyEqual(lhs.utilityScore, rhs.utilityScore, 0.001f))
			{
				return lhs.steamID.GetHashCode().CompareTo(rhs.steamID.GetHashCode());
			}
			return -lhs.utilityScore.CompareTo(rhs.utilityScore);
		}
	}
}
