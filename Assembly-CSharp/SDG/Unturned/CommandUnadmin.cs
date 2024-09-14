using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003D2 RID: 978
	public class CommandUnadmin : Command
	{
		// Token: 0x06001D4D RID: 7501 RVA: 0x0006ADD0 File Offset: 0x00068FD0
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
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", parameter));
				return;
			}
			SteamAdminlist.unadmin(csteamID);
			CommandWindow.Log(this.localization.format("UnadminText", csteamID));
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x0006AE3C File Offset: 0x0006903C
		public CommandUnadmin(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("UnadminCommandText");
			this._info = this.localization.format("UnadminInfoText");
			this._help = this.localization.format("UnadminHelpText");
		}
	}
}
