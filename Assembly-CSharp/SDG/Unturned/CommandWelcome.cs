using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003D9 RID: 985
	public class CommandWelcome : Command
	{
		// Token: 0x06001D5C RID: 7516 RVA: 0x0006B7BC File Offset: 0x000699BC
		protected override void execute(CSteamID executorID, string parameter)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 1 && componentsFromSerial.Length != 4)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			ChatManager.welcomeText = componentsFromSerial[0];
			if (componentsFromSerial.Length == 1)
			{
				ChatManager.welcomeColor = Palette.SERVER;
			}
			else if (componentsFromSerial.Length == 4)
			{
				byte b;
				if (!byte.TryParse(componentsFromSerial[1], ref b))
				{
					CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[0]));
					return;
				}
				byte b2;
				if (!byte.TryParse(componentsFromSerial[2], ref b2))
				{
					CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[1]));
					return;
				}
				byte b3;
				if (!byte.TryParse(componentsFromSerial[3], ref b3))
				{
					CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[2]));
					return;
				}
				ChatManager.welcomeColor = new Color((float)b / 255f, (float)b2 / 255f, (float)b3 / 255f);
			}
			CommandWindow.Log(this.localization.format("WelcomeText", componentsFromSerial[0]));
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x0006B8C4 File Offset: 0x00069AC4
		public CommandWelcome(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("WelcomeCommandText");
			this._info = this.localization.format("WelcomeInfoText");
			this._help = this.localization.format("WelcomeHelpText");
		}
	}
}
