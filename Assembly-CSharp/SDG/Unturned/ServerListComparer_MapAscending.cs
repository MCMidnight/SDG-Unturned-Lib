using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by map name A to Z.
	/// </summary>
	// Token: 0x02000699 RID: 1689
	public class ServerListComparer_MapAscending : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x06003911 RID: 14609 RVA: 0x0010CB6F File Offset: 0x0010AD6F
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			if (string.Equals(lhs.map, rhs.map))
			{
				return lhs.name.CompareTo(rhs.name);
			}
			return lhs.map.CompareTo(rhs.map);
		}
	}
}
