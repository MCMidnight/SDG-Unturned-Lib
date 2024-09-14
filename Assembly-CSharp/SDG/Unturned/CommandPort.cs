using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003BF RID: 959
	public class CommandPort : Command
	{
		// Token: 0x06001D23 RID: 7459 RVA: 0x0006971C File Offset: 0x0006791C
		protected override void execute(CSteamID executorID, string parameter)
		{
			ushort num;
			if (!ushort.TryParse(parameter, ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
				return;
			}
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.port = num;
			CommandWindow.Log(this.localization.format("PortText", num));
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x00069788 File Offset: 0x00067988
		public CommandPort(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PortCommandText");
			this._info = this.localization.format("PortInfoText");
			this._help = this.localization.format("PortHelpText");
		}
	}
}
