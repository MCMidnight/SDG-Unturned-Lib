using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003B3 RID: 947
	public class CommandMap : Command
	{
		// Token: 0x06001D09 RID: 7433 RVA: 0x00068BB4 File Offset: 0x00066DB4
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.map = parameter;
			CommandWindow.Log(this.localization.format("MapText", parameter));
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x00068BF0 File Offset: 0x00066DF0
		public CommandMap(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("MapCommandText");
			this._info = this.localization.format("MapInfoText");
			this._help = this.localization.format("MapHelpText");
		}
	}
}
