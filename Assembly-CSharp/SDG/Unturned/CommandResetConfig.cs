using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003C5 RID: 965
	public class CommandResetConfig : Command
	{
		// Token: 0x06001D30 RID: 7472 RVA: 0x00069E8C File Offset: 0x0006808C
		protected override void execute(CSteamID executorID, string parameter)
		{
			Provider.resetConfig();
			CommandWindow.Log(this.localization.format("ResetConfigText"));
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x00069EA8 File Offset: 0x000680A8
		public CommandResetConfig(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ResetConfigCommandText");
			this._info = this.localization.format("ResetConfigInfoText");
			this._help = this.localization.format("ResetConfigHelpText");
		}
	}
}
