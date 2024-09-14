using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003DA RID: 986
	public class CommandWhitelisted : Command
	{
		// Token: 0x06001D5E RID: 7518 RVA: 0x0006B920 File Offset: 0x00069B20
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.isWhitelisted = true;
			CommandWindow.Log(this.localization.format("WhitelistedText"));
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x0006B95C File Offset: 0x00069B5C
		public CommandWhitelisted(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("WhitelistedCommandText");
			this._info = this.localization.format("WhitelistedInfoText");
			this._help = this.localization.format("WhitelistedHelpText");
		}
	}
}
