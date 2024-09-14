using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000393 RID: 915
	public class CommandAirdrop : Command
	{
		// Token: 0x06001CBB RID: 7355 RVA: 0x00065F28 File Offset: 0x00064128
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!LevelManager.hasAirdrop)
			{
				return;
			}
			LevelManager.airdropFrequency = 0U;
			CommandWindow.Log(this.localization.format("AirdropText"));
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x00065F50 File Offset: 0x00064150
		public CommandAirdrop(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("AirdropCommandText");
			this._info = this.localization.format("AirdropInfoText");
			this._help = this.localization.format("AirdropHelpText");
		}
	}
}
