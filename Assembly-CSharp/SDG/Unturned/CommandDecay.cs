using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003A1 RID: 929
	public class CommandDecay : Command
	{
		// Token: 0x06001CD8 RID: 7384 RVA: 0x00066DBC File Offset: 0x00064FBC
		protected override void execute(CSteamID executorID, string parameter)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			uint num;
			if (!uint.TryParse(componentsFromSerial[0], ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
				return;
			}
			uint num2;
			if (!uint.TryParse(componentsFromSerial[1], ref num2))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
				return;
			}
			CommandWindow.Log(this.localization.format("DecayText"));
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x00066E4C File Offset: 0x0006504C
		public CommandDecay(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("DecayCommandText");
			this._info = this.localization.format("DecayInfoText");
			this._help = this.localization.format("DecayHelpText");
		}
	}
}
