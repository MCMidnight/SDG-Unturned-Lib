using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by ping high to low.
	/// </summary>
	// Token: 0x020006A2 RID: 1698
	public class ServerListComparer_PingDescending : ServerListComparer_PingAscending
	{
		// Token: 0x06003923 RID: 14627 RVA: 0x0010CD5A File Offset: 0x0010AF5A
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
