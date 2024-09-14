using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000398 RID: 920
	public class CommandBind : Command
	{
		// Token: 0x06001CC5 RID: 7365 RVA: 0x00066534 File Offset: 0x00064734
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Parser.checkIP(parameter))
			{
				CommandWindow.LogError(this.localization.format("InvalidIPErrorText", parameter));
				return;
			}
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.ip = Parser.getUInt32FromIP(parameter);
			Provider.bindAddress = parameter;
			CommandWindow.Log(this.localization.format("BindText", parameter));
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000665A4 File Offset: 0x000647A4
		public CommandBind(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("BindCommandText");
			this._info = this.localization.format("BindInfoText");
			this._help = this.localization.format("BindHelpText");
		}
	}
}
