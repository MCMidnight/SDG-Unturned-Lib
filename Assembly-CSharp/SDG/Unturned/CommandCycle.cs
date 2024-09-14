using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200039E RID: 926
	public class CommandCycle : Command
	{
		// Token: 0x06001CD2 RID: 7378 RVA: 0x00066A70 File Offset: 0x00064C70
		protected override void execute(CSteamID executorID, string parameter)
		{
			uint num;
			if (!uint.TryParse(parameter, ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
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
			LightingManager.cycle = num;
			CommandWindow.Log(this.localization.format("CycleText", num));
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x00066B14 File Offset: 0x00064D14
		public CommandCycle(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("CycleCommandText");
			this._info = this.localization.format("CycleInfoText");
			this._help = this.localization.format("CycleHelpText");
		}
	}
}
