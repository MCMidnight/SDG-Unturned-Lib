using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003AD RID: 941
	public class CommandKick : Command
	{
		// Token: 0x06001CFD RID: 7421 RVA: 0x00068368 File Offset: 0x00066568
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 1 && componentsFromSerial.Length != 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			SteamPlayer steamPlayer;
			if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out steamPlayer))
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", componentsFromSerial[0]));
				return;
			}
			if (componentsFromSerial.Length == 1)
			{
				Provider.kick(steamPlayer.playerID.steamID, this.localization.format("KickTextReason"));
			}
			else if (componentsFromSerial.Length == 2)
			{
				Provider.kick(steamPlayer.playerID.steamID, componentsFromSerial[1]);
			}
			CommandWindow.Log(this.localization.format("KickText", steamPlayer.playerID.playerName));
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x00068444 File Offset: 0x00066644
		public CommandKick(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("KickCommandText");
			this._info = this.localization.format("KickInfoText");
			this._help = this.localization.format("KickHelpText");
		}
	}
}
