using System;

namespace SDG.Unturned
{
	// Token: 0x020006AC RID: 1708
	public class ServerListComparer_WorkshopInverted : ServerListComparer_WorkshopDefault
	{
		// Token: 0x06003937 RID: 14647 RVA: 0x0010CF0C File Offset: 0x0010B10C
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
