using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003CA RID: 970
	public class CommandShutdown : Command
	{
		// Token: 0x06001D3A RID: 7482 RVA: 0x0006A200 File Offset: 0x00068400
		protected override void execute(CSteamID executorID, string parameter)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length > 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			if (componentsFromSerial.Length == 0)
			{
				Provider.shutdown();
				return;
			}
			int timer;
			if (!int.TryParse(componentsFromSerial[0], ref timer))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
				return;
			}
			string explanation = "";
			if (componentsFromSerial.Length > 1)
			{
				explanation = componentsFromSerial[1];
			}
			Provider.shutdown(timer, explanation);
			CommandWindow.LogError(this.localization.format("ShutdownText", parameter));
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x0006A28C File Offset: 0x0006848C
		public CommandShutdown(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ShutdownCommandText");
			this._info = this.localization.format("ShutdownInfoText");
			this._help = this.localization.format("ShutdownHelpText");
		}
	}
}
