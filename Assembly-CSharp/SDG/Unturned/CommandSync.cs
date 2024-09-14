using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003CD RID: 973
	public class CommandSync : Command
	{
		// Token: 0x06001D40 RID: 7488 RVA: 0x0006A590 File Offset: 0x00068790
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			PlayerSavedata.hasSync = true;
			CommandWindow.Log(this.localization.format("SyncText"));
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x0006A5CC File Offset: 0x000687CC
		public CommandSync(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("SyncCommandText");
			this._info = this.localization.format("SyncInfoText");
			this._help = this.localization.format("SyncHelpText");
		}
	}
}
