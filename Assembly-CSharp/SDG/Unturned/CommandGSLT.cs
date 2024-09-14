using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003AA RID: 938
	public class CommandGSLT : Command
	{
		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x00068040 File Offset: 0x00066240
		// (set) Token: 0x06001CF6 RID: 7414 RVA: 0x00068047 File Offset: 0x00066247
		public static string loginToken { get; private set; }

		// Token: 0x06001CF7 RID: 7415 RVA: 0x0006804F File Offset: 0x0006624F
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			CommandGSLT.loginToken = parameter;
			CommandWindow.Log(this.localization.format("SetText"));
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x0006808C File Offset: 0x0006628C
		public CommandGSLT(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("GSLTCommandText");
			this._info = this.localization.format("GSLTInfoText");
			this._help = this.localization.format("GSLTHelpText");
		}
	}
}
