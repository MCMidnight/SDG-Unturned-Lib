using System;

namespace SDG.Unturned
{
	// Token: 0x020006AE RID: 1710
	public class ServerListComparer_GoldInverted : ServerListComparer_GoldDefault
	{
		// Token: 0x0600393B RID: 14651 RVA: 0x0010CF54 File Offset: 0x0010B154
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
