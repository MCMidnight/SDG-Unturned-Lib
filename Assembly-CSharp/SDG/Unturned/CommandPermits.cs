using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003BD RID: 957
	public class CommandPermits : Command
	{
		// Token: 0x06001D1F RID: 7455 RVA: 0x000694F4 File Offset: 0x000676F4
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (SteamWhitelist.list.Count == 0)
			{
				CommandWindow.LogError(this.localization.format("NoPermitsErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("PermitsText"));
			for (int i = 0; i < SteamWhitelist.list.Count; i++)
			{
				SteamWhitelistID steamWhitelistID = SteamWhitelist.list[i];
				CommandWindow.Log(this.localization.format("PermitNameText", steamWhitelistID.steamID, steamWhitelistID.tag));
				CommandWindow.Log(this.localization.format("PermitJudgeText", steamWhitelistID.judgeID));
			}
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x000695A0 File Offset: 0x000677A0
		public CommandPermits(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PermitsCommandText");
			this._info = this.localization.format("PermitsInfoText");
			this._help = this.localization.format("PermitsHelpText");
		}
	}
}
