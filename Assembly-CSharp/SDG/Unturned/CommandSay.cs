using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003C7 RID: 967
	public class CommandSay : Command
	{
		// Token: 0x06001D34 RID: 7476 RVA: 0x00069F7C File Offset: 0x0006817C
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 1 && componentsFromSerial.Length != 4)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			if (componentsFromSerial.Length == 1)
			{
				ChatManager.say(componentsFromSerial[0], Palette.SERVER, false);
				return;
			}
			if (componentsFromSerial.Length == 4)
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
				ChatManager.say(componentsFromSerial[0], new Color((float)b / 255f, (float)b2 / 255f, (float)b3 / 255f), false);
			}
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x0006A084 File Offset: 0x00068284
		public CommandSay(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("SayCommandText");
			this._info = this.localization.format("SayInfoText");
			this._help = this.localization.format("SayHelpText");
		}
	}
}
