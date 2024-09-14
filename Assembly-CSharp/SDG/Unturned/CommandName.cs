using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003B7 RID: 951
	public class CommandName : Command
	{
		// Token: 0x06001D12 RID: 7442 RVA: 0x00068FCC File Offset: 0x000671CC
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (parameter.Length < (int)CommandName.MIN_LENGTH)
			{
				CommandWindow.LogError(this.localization.format("MinLengthErrorText", CommandName.MIN_LENGTH));
				return;
			}
			if (parameter.Length > (int)CommandName.MAX_LENGTH)
			{
				CommandWindow.LogError(this.localization.format("MaxLengthErrorText", CommandName.MAX_LENGTH));
				return;
			}
			Provider.serverName = parameter;
			CommandWindow.Log(this.localization.format("NameText", parameter));
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x00069050 File Offset: 0x00067250
		public CommandName(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("NameCommandText");
			this._info = this.localization.format("NameInfoText");
			this._help = this.localization.format("NameHelpText");
		}

		// Token: 0x04000DC7 RID: 3527
		private static readonly byte MIN_LENGTH = 5;

		// Token: 0x04000DC8 RID: 3528
		private static readonly byte MAX_LENGTH = 50;
	}
}
