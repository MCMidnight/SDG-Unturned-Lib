using System;

namespace SDG.Unturned
{
	// Token: 0x020006A4 RID: 1700
	public class ServerListComparer_AnticheatInverted : ServerListComparer_AnticheatDefault
	{
		// Token: 0x06003927 RID: 14631 RVA: 0x0010CDCA File Offset: 0x0010AFCA
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
