using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003A5 RID: 933
	public class CommandFilter : Command
	{
		// Token: 0x06001CE8 RID: 7400 RVA: 0x0006791C File Offset: 0x00065B1C
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.filterName = true;
			CommandWindow.Log(this.localization.format("FilterText"));
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x00067958 File Offset: 0x00065B58
		public CommandFilter(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("FilterCommandText");
			this._info = this.localization.format("FilterInfoText");
			this._help = this.localization.format("FilterHelpText");
		}
	}
}
