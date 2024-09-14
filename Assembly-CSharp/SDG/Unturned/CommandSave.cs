using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003C6 RID: 966
	public class CommandSave : Command
	{
		// Token: 0x06001D32 RID: 7474 RVA: 0x00069F04 File Offset: 0x00068104
		protected override void execute(CSteamID executorID, string parameter)
		{
			SaveManager.save();
			CommandWindow.Log(this.localization.format("SaveText"));
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x00069F20 File Offset: 0x00068120
		public CommandSave(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("SaveCommandText");
			this._info = this.localization.format("SaveInfoText");
			this._help = this.localization.format("SaveHelpText");
		}
	}
}
