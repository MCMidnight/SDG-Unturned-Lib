using System;

namespace SDG.Unturned
{
	// Token: 0x020006AA RID: 1706
	public class ServerListComparer_PasswordInverted : ServerListComparer_PasswordDefault
	{
		// Token: 0x06003933 RID: 14643 RVA: 0x0010CEC4 File Offset: 0x0010B0C4
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
