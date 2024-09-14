using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003CF RID: 975
	public class CommandTime : Command
	{
		// Token: 0x06001D46 RID: 7494 RVA: 0x0006AB08 File Offset: 0x00068D08
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			if (Provider.isServer && Level.info.type == ELevelType.HORDE)
			{
				CommandWindow.LogError(this.localization.format("HordeErrorText"));
				return;
			}
			if (Provider.isServer && Level.info.type == ELevelType.ARENA)
			{
				CommandWindow.LogError(this.localization.format("ArenaErrorText"));
				return;
			}
			uint num;
			if (!uint.TryParse(parameter, ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
				return;
			}
			LightingManager.time = num;
			CommandWindow.Log(this.localization.format("TimeText", num));
		}

		// Token: 0x06001D47 RID: 7495 RVA: 0x0006ABC8 File Offset: 0x00068DC8
		public CommandTime(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("TimeCommandText");
			this._info = this.localization.format("TimeInfoText");
			this._help = this.localization.format("TimeHelpText");
		}
	}
}
