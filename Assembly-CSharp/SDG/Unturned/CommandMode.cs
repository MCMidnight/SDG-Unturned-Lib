using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003B5 RID: 949
	public class CommandMode : Command
	{
		// Token: 0x06001D0E RID: 7438 RVA: 0x00068D64 File Offset: 0x00066F64
		protected override void execute(CSteamID executorID, string parameter)
		{
			string text = parameter.ToLower();
			EGameMode mode;
			if (text == this.localization.format("ModeEasy").ToLower())
			{
				mode = EGameMode.EASY;
			}
			else if (text == this.localization.format("ModeNormal").ToLower())
			{
				mode = EGameMode.NORMAL;
			}
			else
			{
				if (!(text == this.localization.format("ModeHard").ToLower()))
				{
					CommandWindow.LogError(this.localization.format("NoModeErrorText", text));
					return;
				}
				mode = EGameMode.HARD;
			}
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.mode = mode;
			CommandWindow.Log(this.localization.format("ModeText", text));
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x00068E2C File Offset: 0x0006702C
		public CommandMode(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ModeCommandText");
			this._info = this.localization.format("ModeInfoText");
			this._help = this.localization.format("ModeHelpText");
		}
	}
}
