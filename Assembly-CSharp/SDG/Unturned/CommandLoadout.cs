using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003AF RID: 943
	public class CommandLoadout : Command
	{
		// Token: 0x06001D01 RID: 7425 RVA: 0x000685A0 File Offset: 0x000667A0
		protected override void execute(CSteamID executorID, string parameter)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length < 1)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			byte b;
			if (!byte.TryParse(componentsFromSerial[0], ref b) || (b != 255 && b > 10))
			{
				CommandWindow.LogError(this.localization.format("InvalidSkillsetIDErrorText", componentsFromSerial[0]));
				return;
			}
			ushort[] array = new ushort[componentsFromSerial.Length - 1];
			for (int i = 1; i < componentsFromSerial.Length; i++)
			{
				ushort num;
				if (!ushort.TryParse(componentsFromSerial[i], ref num))
				{
					CommandWindow.LogError(this.localization.format("InvalidItemIDErrorText", componentsFromSerial[i]));
					return;
				}
				array[i - 1] = num;
			}
			if (b == 255)
			{
				PlayerInventory.loadout = array;
			}
			else
			{
				PlayerInventory.skillsets[(int)b] = array;
			}
			CommandWindow.Log(this.localization.format("LoadoutText", b));
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x0006867C File Offset: 0x0006687C
		public CommandLoadout(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("LoadoutCommandText");
			this._info = this.localization.format("LoadoutInfoText");
			this._help = this.localization.format("LoadoutHelpText");
		}
	}
}
