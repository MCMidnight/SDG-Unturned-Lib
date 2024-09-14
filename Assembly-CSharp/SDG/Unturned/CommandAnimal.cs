using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000394 RID: 916
	public class CommandAnimal : Command
	{
		// Token: 0x06001CBD RID: 7357 RVA: 0x00065FAC File Offset: 0x000641AC
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
			if (componentsFromSerial.Length < 1 || componentsFromSerial.Length > 3)
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
				CommandWindow.LogError(this.localization.format("InvalidAnimalIDErrorText", componentsFromSerial[flag ? 0 : 1]));
				return;
			}
			if (!AnimalManager.giveAnimal(steamPlayer.player, num))
			{
				CommandWindow.LogError(this.localization.format("NoAnimalIDErrorText", num));
				return;
			}
			CommandWindow.Log(this.localization.format("AnimalText", steamPlayer.playerID.playerName, num));
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000660D4 File Offset: 0x000642D4
		public CommandAnimal(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("AnimalCommandText");
			this._info = this.localization.format("AnimalInfoText");
			this._help = this.localization.format("AnimalHelpText");
		}
	}
}
