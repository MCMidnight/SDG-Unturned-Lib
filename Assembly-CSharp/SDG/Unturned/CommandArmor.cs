using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000395 RID: 917
	public class CommandArmor : Command
	{
		// Token: 0x06001CBF RID: 7359 RVA: 0x00066130 File Offset: 0x00064330
		protected override void execute(CSteamID executorID, string parameter)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			float num;
			if (!float.TryParse(componentsFromSerial[0], ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
				return;
			}
			float num2;
			if (!float.TryParse(componentsFromSerial[1], ref num2))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
				return;
			}
			CommandWindow.Log(this.localization.format("ArmorText"));
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x000661C0 File Offset: 0x000643C0
		public CommandArmor(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ArmorCommandText");
			this._info = this.localization.format("ArmorInfoText");
			this._help = this.localization.format("ArmorHelpText");
		}
	}
}
