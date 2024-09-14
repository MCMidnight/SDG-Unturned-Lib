using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003D5 RID: 981
	public class CommandUnpermit : Command
	{
		// Token: 0x06001D53 RID: 7507 RVA: 0x0006AFF4 File Offset: 0x000691F4
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			CSteamID csteamID;
			if (!PlayerTool.tryGetSteamID(parameter, out csteamID))
			{
				CommandWindow.LogError(this.localization.format("InvalidSteamIDErrorText", parameter));
				return;
			}
			if (!SteamWhitelist.unwhitelist(csteamID))
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", csteamID));
				return;
			}
			CommandWindow.Log(this.localization.format("UnpermitText", csteamID));
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x0006B080 File Offset: 0x00069280
		public CommandUnpermit(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("UnpermitCommandText");
			this._info = this.localization.format("UnpermitInfoText");
			this._help = this.localization.format("UnpermitHelpText");
		}
	}
}
