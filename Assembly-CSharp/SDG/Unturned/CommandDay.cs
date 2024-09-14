using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200039F RID: 927
	public class CommandDay : Command
	{
		// Token: 0x06001CD4 RID: 7380 RVA: 0x00066B70 File Offset: 0x00064D70
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
			LightingManager.time = (uint)(LightingManager.cycle * LevelLighting.transition);
			CommandWindow.Log(this.localization.format("DayText"));
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x00066C18 File Offset: 0x00064E18
		public CommandDay(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("DayCommandText");
			this._info = this.localization.format("DayInfoText");
			this._help = this.localization.format("DayHelpText");
		}
	}
}
