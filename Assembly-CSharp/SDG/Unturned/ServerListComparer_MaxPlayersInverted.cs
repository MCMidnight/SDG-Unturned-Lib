using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort servers by max player count low to high.
	/// </summary>
	// Token: 0x0200069E RID: 1694
	public class ServerListComparer_MaxPlayersInverted : ServerListComparer_MaxPlayersDefault
	{
		// Token: 0x0600391B RID: 14619 RVA: 0x0010CC6A File Offset: 0x0010AE6A
		public override int Compare(SteamServerAdvertisement lhs, SteamServerAdvertisement rhs)
		{
			return -base.Compare(lhs, rhs);
		}
	}
}
