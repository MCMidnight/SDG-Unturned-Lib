using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003C4 RID: 964
	public class CommandReputation : Command
	{
		// Token: 0x06001D2E RID: 7470 RVA: 0x00069D0C File Offset: 0x00067F0C
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			if (!Provider.hasCheats)
			{
				CommandWindow.LogError(this.localization.format("CheatsErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length < 1 || componentsFromSerial.Length > 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			bool flag = false;
			SteamPlayer steamPlayer;
			if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out steamPlayer))
			{
				steamPlayer = PlayerTool.getSteamPlayer(executorID);
				if (steamPlayer == null)
				{
					CommandWindow.LogError(this.localization.format("NoPlayerErrorText", componentsFromSerial[0]));
					return;
				}
				flag = true;
			}
			int num;
			if (!int.TryParse(componentsFromSerial[flag ? 0 : 1], ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[flag ? 0 : 1]));
				return;
			}
			steamPlayer.player.skills.askRep(num);
			string text = num.ToString();
			if (num > 0)
			{
				text = "+" + text;
			}
			CommandWindow.Log(this.localization.format("ReputationText", steamPlayer.playerID.playerName, text));
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x00069E30 File Offset: 0x00068030
		public CommandReputation(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ReputationCommandText");
			this._info = this.localization.format("ReputationInfoText");
			this._help = this.localization.format("ReputationHelpText");
		}
	}
}
