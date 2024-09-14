using System;

namespace SDG.Unturned
{
	// Token: 0x020006B4 RID: 1716
	public class ServerListComparer_PluginsInverted : ServerListComparer_PluginsDefault
	{
		// Token: 0x06003947 RID: 14663 RVA: 0x0010D0DE File Offset: 0x0010B2DE
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
