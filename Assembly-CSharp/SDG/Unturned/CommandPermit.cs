using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003BC RID: 956
	public class CommandPermit : Command
	{
		// Token: 0x06001D1D RID: 7453 RVA: 0x000693FC File Offset: 0x000675FC
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			CSteamID csteamID;
			if (!PlayerTool.tryGetSteamID(componentsFromSerial[0], out csteamID))
			{
				CommandWindow.LogError(this.localization.format("InvalidSteamIDErrorText", componentsFromSerial[0]));
				return;
			}
			SteamWhitelist.whitelist(csteamID, componentsFromSerial[1], executorID);
			CommandWindow.Log(this.localization.format("PermitText", csteamID, componentsFromSerial[1]));
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x00069498 File Offset: 0x00067698
		public CommandPermit(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PermitCommandText");
			this._info = this.localization.format("PermitInfoText");
			this._help = this.localization.format("PermitHelpText");
		}
	}
}
