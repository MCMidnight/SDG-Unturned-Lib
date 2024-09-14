using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000762 RID: 1890
	internal class OwnershipTool
	{
		// Token: 0x06003DCE RID: 15822 RVA: 0x0012B24B File Offset: 0x0012944B
		public static bool checkToggle(ulong player, ulong group)
		{
			return false;
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x0012B24E File Offset: 0x0012944E
		public static bool checkToggle(CSteamID player_0, ulong player_1, CSteamID group_0, ulong group_1)
		{
			bool isServer = Provider.isServer;
			return player_0.m_SteamID == player_1 || (group_0 != CSteamID.Nil && group_0.m_SteamID == group_1);
		}
	}
}
