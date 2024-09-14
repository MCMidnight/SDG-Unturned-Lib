using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003B8 RID: 952
	public class CommandNight : Command
	{
		// Token: 0x06001D15 RID: 7445 RVA: 0x000690BC File Offset: 0x000672BC
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
			LightingManager.time = (uint)(LightingManager.cycle * (LevelLighting.bias + LevelLighting.transition));
			CommandWindow.Log(this.localization.format("NightText"));
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x00069168 File Offset: 0x00067368
		public CommandNight(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("NightCommandText");
			this._info = this.localization.format("NightInfoText");
			this._help = this.localization.format("NightHelpText");
		}
	}
}
