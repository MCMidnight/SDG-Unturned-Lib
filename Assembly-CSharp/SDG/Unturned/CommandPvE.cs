using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003C0 RID: 960
	public class CommandPvE : Command
	{
		// Token: 0x06001D25 RID: 7461 RVA: 0x000697E4 File Offset: 0x000679E4
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.isPvP = false;
			CommandWindow.Log(this.localization.format("PvEText"));
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x00069820 File Offset: 0x00067A20
		public CommandPvE(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PvECommandText");
			this._info = this.localization.format("PvEInfoText");
			this._help = this.localization.format("PvEHelpText");
		}
	}
}
