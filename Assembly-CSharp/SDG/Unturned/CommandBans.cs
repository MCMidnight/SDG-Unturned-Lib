using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000397 RID: 919
	public class CommandBans : Command
	{
		// Token: 0x06001CC3 RID: 7363 RVA: 0x000663FC File Offset: 0x000645FC
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (SteamBlacklist.list.Count == 0)
			{
				CommandWindow.LogError(this.localization.format("NoBansErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("BansText"));
			for (int i = 0; i < SteamBlacklist.list.Count; i++)
			{
				SteamBlacklistID steamBlacklistID = SteamBlacklist.list[i];
				CommandWindow.Log(this.localization.format("BanNameText", steamBlacklistID.playerID));
				CommandWindow.Log(this.localization.format("BanJudgeText", steamBlacklistID.judgeID));
				CommandWindow.Log(this.localization.format("BanStatusText", steamBlacklistID.reason, steamBlacklistID.duration, steamBlacklistID.getTime()));
			}
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000664D8 File Offset: 0x000646D8
		public CommandBans(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("BansCommandText");
			this._info = this.localization.format("BansInfoText");
			this._help = this.localization.format("BansHelpText");
		}
	}
}
