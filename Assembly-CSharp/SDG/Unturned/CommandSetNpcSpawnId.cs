using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003C9 RID: 969
	public class CommandSetNpcSpawnId : Command
	{
		// Token: 0x06001D38 RID: 7480 RVA: 0x0006A188 File Offset: 0x00068388
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
			steamPlayer.player.quests.npcSpawnId = parameter;
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x0006A1CF File Offset: 0x000683CF
		public CommandSetNpcSpawnId(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = "SetNpcSpawn";
			this._info = string.Empty;
			this._help = string.Empty;
		}
	}
}
