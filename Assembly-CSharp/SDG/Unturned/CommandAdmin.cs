using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000391 RID: 913
	public class CommandAdmin : Command
	{
		// Token: 0x06001CB7 RID: 7351 RVA: 0x00065D58 File Offset: 0x00063F58
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
			SteamAdminlist.admin(csteamID, executorID);
			CommandWindow.Log(this.localization.format("AdminText", csteamID));
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x00065DC8 File Offset: 0x00063FC8
		public CommandAdmin(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("AdminCommandText");
			this._info = this.localization.format("AdminInfoText");
			this._help = this.localization.format("AdminHelpText");
		}
	}
}
