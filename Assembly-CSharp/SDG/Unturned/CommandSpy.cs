using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003CC RID: 972
	public class CommandSpy : Command
	{
		// Token: 0x06001D3E RID: 7486 RVA: 0x0006A484 File Offset: 0x00068684
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 1)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			SteamPlayer steamPlayer;
			if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out steamPlayer) || steamPlayer.player == null)
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", componentsFromSerial[0]));
				return;
			}
			steamPlayer.player.sendScreenshot(executorID, null);
			CommandWindow.Log(this.localization.format("SpyText", steamPlayer.playerID.playerName));
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x0006A534 File Offset: 0x00068734
		public CommandSpy(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("SpyCommandText");
			this._info = this.localization.format("SpyInfoText");
			this._help = this.localization.format("SpyHelpText");
		}
	}
}
