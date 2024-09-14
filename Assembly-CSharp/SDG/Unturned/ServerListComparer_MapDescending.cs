using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by map name Z to A.
	/// </summary>
	// Token: 0x0200069A RID: 1690
	public class ServerListComparer_MapDescending : ServerListComparer_MapAscending
	{
		// Token: 0x06003913 RID: 14611 RVA: 0x0010CBAF File Offset: 0x0010ADAF
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
