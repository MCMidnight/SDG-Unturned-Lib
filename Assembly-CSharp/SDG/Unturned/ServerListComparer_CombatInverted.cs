using System;

namespace SDG.Unturned
{
	// Token: 0x020006A8 RID: 1704
	public class ServerListComparer_CombatInverted : ServerListComparer_CombatDefault
	{
		// Token: 0x0600392F RID: 14639 RVA: 0x0010CE7C File Offset: 0x0010B07C
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
