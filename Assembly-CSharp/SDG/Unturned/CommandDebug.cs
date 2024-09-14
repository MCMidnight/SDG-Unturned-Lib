using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003A0 RID: 928
	public class CommandDebug : Command
	{
		// Token: 0x06001CD6 RID: 7382 RVA: 0x00066C74 File Offset: 0x00064E74
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("DebugText"));
			CommandWindow.Log(this.localization.format("DebugUPSText", Mathf.CeilToInt((float)Provider.debugUPS / 50f * 100f)));
			CommandWindow.Log(this.localization.format("DebugTPSText", Mathf.CeilToInt((float)Provider.debugTPS / 50f * 100f)));
			CommandWindow.Log(this.localization.format("DebugZombiesText", ZombieManager.tickingZombies.Count));
			CommandWindow.Log(this.localization.format("DebugAnimalsText", AnimalManager.tickingAnimals.Count));
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x00066D60 File Offset: 0x00064F60
		public CommandDebug(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("DebugCommandText");
			this._info = this.localization.format("DebugInfoText");
			this._help = this.localization.format("DebugHelpText");
		}
	}
}
