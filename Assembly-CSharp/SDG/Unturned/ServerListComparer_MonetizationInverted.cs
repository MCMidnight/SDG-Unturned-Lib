using System;

namespace SDG.Unturned
{
	// Token: 0x020006B2 RID: 1714
	public class ServerListComparer_MonetizationInverted : ServerListComparer_MonetizationDefault
	{
		// Token: 0x06003943 RID: 14659 RVA: 0x0010D048 File Offset: 0x0010B248
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
