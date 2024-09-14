using System;
using SDG.Framework.Modules;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003B6 RID: 950
	public class CommandModules : Command
	{
		// Token: 0x06001D10 RID: 7440 RVA: 0x00068E88 File Offset: 0x00067088
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (ModuleHook.modules.Count == 0)
			{
				CommandWindow.LogError(this.localization.format("NoModulesErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("ModulesText"));
			CommandWindow.Log(this.localization.format("SeparatorText"));
			for (int i = 0; i < ModuleHook.modules.Count; i++)
			{
				Module module = ModuleHook.modules[i];
				if (module != null)
				{
					ModuleConfig config = module.config;
					if (config != null)
					{
						Local local = Localization.tryRead(config.DirectoryPath, false);
						CommandWindow.Log(local.format("Name"));
						CommandWindow.Log(this.localization.format("Version", config.Version));
						CommandWindow.Log(local.format("Description"));
						CommandWindow.Log(this.localization.format("SeparatorText"));
					}
				}
			}
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x00068F70 File Offset: 0x00067170
		public CommandModules(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ModulesCommandText");
			this._info = this.localization.format("ModulesInfoText");
			this._help = this.localization.format("ModulesHelpText");
		}
	}
}
