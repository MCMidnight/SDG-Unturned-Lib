using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200039B RID: 923
	public class CommandCheats : Command
	{
		// Token: 0x06001CCC RID: 7372 RVA: 0x00066856 File Offset: 0x00064A56
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.hasCheats = true;
			CommandWindow.Log(this.localization.format("CheatsText"));
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x00066890 File Offset: 0x00064A90
		public CommandCheats(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("CheatsCommandText");
			this._info = this.localization.format("CheatsInfoText");
			this._help = this.localization.format("CheatsHelpText");
		}
	}
}
