using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003BA RID: 954
	public class CommandOwner : Command
	{
		// Token: 0x06001D19 RID: 7449 RVA: 0x00069238 File Offset: 0x00067438
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			CSteamID csteamID;
			if (!PlayerTool.tryGetSteamID(parameter, out csteamID))
			{
				CommandWindow.LogError(this.localization.format("InvalidSteamIDErrorText", parameter));
				return;
			}
			SteamAdminlist.ownerID = csteamID;
			CommandWindow.Log(this.localization.format("OwnerText", csteamID));
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x000692A4 File Offset: 0x000674A4
		public CommandOwner(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("OwnerCommandText");
			this._info = this.localization.format("OwnerInfoText");
			this._help = this.localization.format("OwnerHelpText");
		}
	}
}
