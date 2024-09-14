using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003D1 RID: 977
	public class CommandToggleNpcCutsceneMode : Command
	{
		// Token: 0x06001D4B RID: 7499 RVA: 0x0006AD44 File Offset: 0x00068F44
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
			steamPlayer.player.quests.ServerSetCutsceneModeActive(!steamPlayer.player.quests.IsCutsceneModeActive());
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x0006AD9D File Offset: 0x00068F9D
		public CommandToggleNpcCutsceneMode(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = "ToggleNpcCutsceneMode";
			this._info = string.Empty;
			this._help = string.Empty;
		}
	}
}
