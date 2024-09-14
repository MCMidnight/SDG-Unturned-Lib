using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003AE RID: 942
	public class CommandKill : Command
	{
		// Token: 0x06001CFF RID: 7423 RVA: 0x000684A0 File Offset: 0x000666A0
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			SteamPlayer steamPlayer;
			if (!PlayerTool.tryGetSteamPlayer(parameter, out steamPlayer))
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", parameter));
				return;
			}
			if (steamPlayer.player != null)
			{
				EPlayerKill eplayerKill;
				steamPlayer.player.life.askDamage(101, Vector3.up * 101f, EDeathCause.KILL, ELimb.SKULL, executorID, out eplayerKill);
			}
			CommandWindow.Log(this.localization.format("KillText", steamPlayer.playerID.playerName));
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x00068544 File Offset: 0x00066744
		public CommandKill(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("KillCommandText");
			this._info = this.localization.format("KillInfoText");
			this._help = this.localization.format("KillHelpText");
		}
	}
}
