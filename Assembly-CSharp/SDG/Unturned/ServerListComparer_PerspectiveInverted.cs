using System;

namespace SDG.Unturned
{
	// Token: 0x020006A6 RID: 1702
	public class ServerListComparer_PerspectiveInverted : ServerListComparer_PerspectiveDefault
	{
		// Token: 0x0600392B RID: 14635 RVA: 0x0010CE34 File Offset: 0x0010B034
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
