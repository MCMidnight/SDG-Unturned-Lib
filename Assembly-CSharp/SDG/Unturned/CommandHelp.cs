using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003AB RID: 939
	public class CommandHelp : Command
	{
		// Token: 0x06001CF9 RID: 7417 RVA: 0x000680E8 File Offset: 0x000662E8
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (string.IsNullOrEmpty(parameter))
			{
				CommandWindow.Log(this.localization.format("HelpText"));
				string text = "";
				for (int i = 0; i < Commander.commands.Count; i++)
				{
					if (!string.IsNullOrEmpty(Commander.commands[i].info))
					{
						text += Commander.commands[i].info;
						if (i < Commander.commands.Count - 1)
						{
							text += "\n";
						}
					}
				}
				CommandWindow.Log(text);
				return;
			}
			int j = 0;
			while (j < Commander.commands.Count)
			{
				if (parameter.ToLower() == Commander.commands[j].command.ToLower())
				{
					if (executorID == CSteamID.Nil)
					{
						CommandWindow.Log(Commander.commands[j].info);
						CommandWindow.Log(Commander.commands[j].help);
						return;
					}
					ChatManager.say(executorID, Commander.commands[j].info, Palette.SERVER, EChatMode.SAY, false);
					ChatManager.say(executorID, Commander.commands[j].help, Palette.SERVER, EChatMode.SAY, false);
					return;
				}
				else
				{
					j++;
				}
			}
			if (executorID == CSteamID.Nil)
			{
				CommandWindow.Log(this.localization.format("NoCommandErrorText", parameter));
				return;
			}
			ChatManager.say(executorID, this.localization.format("NoCommandErrorText", parameter), Palette.SERVER, EChatMode.SAY, false);
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x00068274 File Offset: 0x00066474
		public CommandHelp(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("HelpCommandText");
			this._info = this.localization.format("HelpInfoText");
			this._help = this.localization.format("HelpHelpText");
		}
	}
}
