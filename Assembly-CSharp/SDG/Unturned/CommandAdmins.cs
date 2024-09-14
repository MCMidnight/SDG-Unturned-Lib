using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000392 RID: 914
	public class CommandAdmins : Command
	{
		// Token: 0x06001CB9 RID: 7353 RVA: 0x00065E24 File Offset: 0x00064024
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (SteamAdminlist.list.Count == 0)
			{
				CommandWindow.LogError(this.localization.format("NoAdminsErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("AdminsText"));
			for (int i = 0; i < SteamAdminlist.list.Count; i++)
			{
				SteamAdminID steamAdminID = SteamAdminlist.list[i];
				CommandWindow.Log(this.localization.format("AdminNameText", steamAdminID.playerID));
				CommandWindow.Log(this.localization.format("AdminJudgeText", steamAdminID.judgeID));
			}
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x00065ECC File Offset: 0x000640CC
		public CommandAdmins(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("AdminsCommandText");
			this._info = this.localization.format("AdminsInfoText");
			this._help = this.localization.format("AdminsHelpText");
		}
	}
}
