using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by player count low to high.
	/// </summary>
	// Token: 0x0200069C RID: 1692
	public class ServerListComparer_PlayersInverted : ServerListComparer_PlayersDefault
	{
		// Token: 0x06003917 RID: 14615 RVA: 0x0010CBF9 File Offset: 0x0010ADF9
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
