using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003B9 RID: 953
	public class CommandNpcEvent : Command
	{
		// Token: 0x06001D17 RID: 7447 RVA: 0x000691C4 File Offset: 0x000673C4
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (!Provider.hasCheats)
			{
				return;
			}
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(executorID);
			if (steamPlayer == null || steamPlayer.player == null)
			{
				return;
			}
			NPCEventManager.broadcastEvent(steamPlayer.player, parameter, true);
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x00069207 File Offset: 0x00067407
		public CommandNpcEvent(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = "NpcEvent";
			this._info = string.Empty;
			this._help = string.Empty;
		}
	}
}
