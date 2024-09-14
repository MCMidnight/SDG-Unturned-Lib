using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003C8 RID: 968
	public class CommandScheduledShutdownInfo : Command
	{
		// Token: 0x06001D36 RID: 7478 RVA: 0x0006A0E0 File Offset: 0x000682E0
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (executorID != CSteamID.Nil)
			{
				return;
			}
			if (Provider.autoShutdownManager.isScheduledShutdownEnabled)
			{
				CommandWindow.Log(string.Format("Shutdown is scheduled for {0} ({1:g} from now)", Provider.autoShutdownManager.scheduledShutdownTime.ToLocalTime(), Provider.autoShutdownManager.scheduledShutdownTime - DateTime.UtcNow));
				return;
			}
			CommandWindow.Log("Scheduled shutdown is disabled");
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x0006A156 File Offset: 0x00068356
		public CommandScheduledShutdownInfo(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = "ScheduledShutdownInfo";
			this._info = string.Empty;
			this._help = string.Empty;
		}
	}
}
