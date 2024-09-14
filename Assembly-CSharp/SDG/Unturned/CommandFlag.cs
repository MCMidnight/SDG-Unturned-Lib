using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003A6 RID: 934
	public class CommandFlag : Command
	{
		// Token: 0x06001CEA RID: 7402 RVA: 0x000679B4 File Offset: 0x00065BB4
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
			if (componentsFromSerial.Length < 2 || componentsFromSerial.Length > 3)
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
			ushort num;
			if (!ushort.TryParse(componentsFromSerial[flag ? 0 : 1], ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[flag ? 0 : 1]));
				return;
			}
			short num2;
			if (!short.TryParse(componentsFromSerial[flag ? 1 : 2], ref num2))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[flag ? 1 : 2]));
				return;
			}
			steamPlayer.player.quests.sendSetFlag(num, num2);
			CommandWindow.Log(this.localization.format("FlagText", steamPlayer.playerID.playerName, num, num2));
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x00067AFC File Offset: 0x00065CFC
		public CommandFlag(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("FlagCommandText");
			this._info = this.localization.format("FlagInfoText");
			this._help = this.localization.format("FlagHelpText");
		}
	}
}
