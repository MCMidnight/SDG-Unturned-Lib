using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by name A to Z.
	/// </summary>
	// Token: 0x02000697 RID: 1687
	public class ServerListComparer_NameAscending : IComparer<SteamServerAdvertisement>
	{
		// Token: 0x0600390D RID: 14605 RVA: 0x0010CB41 File Offset: 0x0010AD41
		public virtual int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return lhs.name.CompareTo(rhs.name);
		}
	}
}
