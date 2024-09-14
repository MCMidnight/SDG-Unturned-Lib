using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003D3 RID: 979
	public class CommandUnban : Command
	{
		// Token: 0x06001D4F RID: 7503 RVA: 0x0006AE98 File Offset: 0x00069098
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
			if (!Provider.requestUnbanPlayer(executorID, csteamID))
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", csteamID));
				return;
			}
			CommandWindow.Log(this.localization.format("UnbanText", csteamID));
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x0006AF24 File Offset: 0x00069124
		public CommandUnban(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("UnbanCommandText");
			this._info = this.localization.format("UnbanInfoText");
			this._help = this.localization.format("UnbanHelpText");
		}
	}
}
