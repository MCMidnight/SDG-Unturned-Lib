using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003AC RID: 940
	public class CommandHideAdmins : Command
	{
		// Token: 0x06001CFB RID: 7419 RVA: 0x000682D0 File Offset: 0x000664D0
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.hideAdmins = true;
			CommandWindow.Log(this.localization.format("HideAdminsText"));
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x0006830C File Offset: 0x0006650C
		public CommandHideAdmins(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("HideAdminsCommandText");
			this._info = this.localization.format("HideAdminsInfoText");
			this._help = this.localization.format("HideAdminsHelpText");
		}
	}
}
