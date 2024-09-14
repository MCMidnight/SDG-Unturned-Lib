using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003A4 RID: 932
	public class CommandExperience : Command
	{
		// Token: 0x06001CE6 RID: 7398 RVA: 0x000677B0 File Offset: 0x000659B0
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
			uint num;
			if (!uint.TryParse(componentsFromSerial[flag ? 0 : 1], ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[flag ? 0 : 1]));
				return;
			}
			steamPlayer.player.skills.askAward(num);
			CommandWindow.Log(this.localization.format("ExperienceText", steamPlayer.playerID.playerName, num));
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x000678C0 File Offset: 0x00065AC0
		public CommandExperience(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ExperienceCommandText");
			this._info = this.localization.format("ExperienceInfoText");
			this._help = this.localization.format("ExperienceHelpText");
		}
	}
}
