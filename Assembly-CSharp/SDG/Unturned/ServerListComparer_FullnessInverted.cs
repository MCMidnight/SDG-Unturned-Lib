using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by normalized player count low to high.
	/// </summary>
	// Token: 0x020006A0 RID: 1696
	public class ServerListComparer_FullnessInverted : ServerListComparer_FullnessDefault
	{
		// Token: 0x0600391F RID: 14623 RVA: 0x0010CCE7 File Offset: 0x0010AEE7
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
