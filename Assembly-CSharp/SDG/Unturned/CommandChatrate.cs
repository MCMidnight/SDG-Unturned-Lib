using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200039A RID: 922
	public class CommandChatrate : Command
	{
		// Token: 0x06001CC9 RID: 7369 RVA: 0x00066744 File Offset: 0x00064944
		protected override void execute(CSteamID executorID, string parameter)
		{
			float num;
			if (!float.TryParse(parameter, ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
				return;
			}
			if (num < CommandChatrate.MIN_NUMBER)
			{
				CommandWindow.LogError(this.localization.format("MinNumberErrorText", CommandChatrate.MIN_NUMBER));
				return;
			}
			if (num > CommandChatrate.MAX_NUMBER)
			{
				CommandWindow.LogError(this.localization.format("MaxNumberErrorText", CommandChatrate.MAX_NUMBER));
				return;
			}
			ChatManager.chatrate = num;
			CommandWindow.Log(this.localization.format("ChatrateText", num));
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x000667E4 File Offset: 0x000649E4
		public CommandChatrate(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ChatrateCommandText");
			this._info = this.localization.format("ChatrateInfoText");
			this._help = this.localization.format("ChatrateHelpText");
		}

		// Token: 0x04000DBE RID: 3518
		private static readonly float MIN_NUMBER = 0f;

		// Token: 0x04000DBF RID: 3519
		private static readonly float MAX_NUMBER = 60f;
	}
}
