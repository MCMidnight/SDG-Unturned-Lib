using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003A9 RID: 937
	public class CommandGold : Command
	{
		// Token: 0x06001CF3 RID: 7411 RVA: 0x00067FA8 File Offset: 0x000661A8
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.isGold = true;
			CommandWindow.Log(this.localization.format("GoldText"));
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x00067FE4 File Offset: 0x000661E4
		public CommandGold(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("GoldCommandText");
			this._info = this.localization.format("GoldInfoText");
			this._help = this.localization.format("GoldHelpText");
		}
	}
}
