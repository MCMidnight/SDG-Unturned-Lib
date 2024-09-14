using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003D4 RID: 980
	public class CommandUnlockNpcAchievement : Command
	{
		// Token: 0x06001D51 RID: 7505 RVA: 0x0006AF80 File Offset: 0x00069180
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
			steamPlayer.player.sendAchievementUnlocked(parameter);
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x0006AFC2 File Offset: 0x000691C2
		public CommandUnlockNpcAchievement(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = "UnlockNpcAchievement";
			this._info = string.Empty;
			this._help = string.Empty;
		}
	}
}
