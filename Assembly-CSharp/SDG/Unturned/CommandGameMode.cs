using System;
using Steamworks;

namespace SDG.Unturned
{
	/// <summary>
	/// Essentially deprecated for now.
	/// </summary>
	// Token: 0x020003A7 RID: 935
	public class CommandGameMode : Command
	{
		// Token: 0x06001CEC RID: 7404 RVA: 0x00067B58 File Offset: 0x00065D58
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("GameModeText", parameter));
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x00067B90 File Offset: 0x00065D90
		public CommandGameMode(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("GameModeCommandText");
			this._info = this.localization.format("GameModeInfoText");
			this._help = this.localization.format("GameModeHelpText");
		}
	}
}
