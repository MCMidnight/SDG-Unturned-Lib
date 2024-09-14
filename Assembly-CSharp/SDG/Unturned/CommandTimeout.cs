using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003D0 RID: 976
	public class CommandTimeout : Command
	{
		// Token: 0x06001D48 RID: 7496 RVA: 0x0006AC24 File Offset: 0x00068E24
		protected override void execute(CSteamID executorID, string parameter)
		{
			ushort num;
			if (!ushort.TryParse(parameter, ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
				return;
			}
			if (num < CommandTimeout.MIN_NUMBER)
			{
				CommandWindow.LogError(this.localization.format("MinNumberErrorText", CommandTimeout.MIN_NUMBER));
				return;
			}
			if (num > CommandTimeout.MAX_NUMBER)
			{
				CommandWindow.LogError(this.localization.format("MaxNumberErrorText", CommandTimeout.MAX_NUMBER));
				return;
			}
			if (Provider.configData != null)
			{
				Provider.configData.Server.Max_Ping_Milliseconds = (uint)num;
			}
			CommandWindow.Log(this.localization.format("TimeoutText", num));
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x0006ACD4 File Offset: 0x00068ED4
		public CommandTimeout(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("TimeoutCommandText");
			this._info = this.localization.format("TimeoutInfoText");
			this._help = this.localization.format("TimeoutHelpText");
		}

		// Token: 0x04000DCA RID: 3530
		private static readonly ushort MIN_NUMBER = 50;

		// Token: 0x04000DCB RID: 3531
		private static readonly ushort MAX_NUMBER = 10000;
	}
}
