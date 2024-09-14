using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003BB RID: 955
	public class CommandPassword : Command
	{
		// Token: 0x06001D1B RID: 7451 RVA: 0x00069300 File Offset: 0x00067500
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			if (string.IsNullOrEmpty(parameter))
			{
				Provider.serverPassword = string.Empty;
				CommandWindow.Log(this.localization.format("DisableText"));
				return;
			}
			Provider.serverPassword = parameter.Trim();
			if (this.localization.has("PasswordTextV2"))
			{
				CommandWindow.Log(this.localization.format("PasswordTextV2"));
				return;
			}
			CommandWindow.Log(this.localization.format("PasswordText", "******"));
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x000693A0 File Offset: 0x000675A0
		public CommandPassword(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PasswordCommandText");
			this._info = this.localization.format("PasswordInfoText");
			this._help = this.localization.format("PasswordHelpText");
		}
	}
}
