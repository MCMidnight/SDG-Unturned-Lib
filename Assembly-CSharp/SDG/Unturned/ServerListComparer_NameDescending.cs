using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by name Z to A.
	/// </summary>
	// Token: 0x02000698 RID: 1688
	public class ServerListComparer_NameDescending : ServerListComparer_NameAscending
	{
		// Token: 0x0600390F RID: 14607 RVA: 0x0010CB5C File Offset: 0x0010AD5C
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
