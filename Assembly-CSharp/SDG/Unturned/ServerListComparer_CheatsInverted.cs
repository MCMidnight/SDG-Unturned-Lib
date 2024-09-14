using System;

namespace SDG.Unturned
{
	// Token: 0x020006B0 RID: 1712
	public class ServerListComparer_CheatsInverted : ServerListComparer_CheatsDefault
	{
		// Token: 0x0600393F RID: 14655 RVA: 0x0010CF9C File Offset: 0x0010B19C
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
