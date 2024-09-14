using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003C1 RID: 961
	public class CommandQuest : Command
	{
		// Token: 0x06001D27 RID: 7463 RVA: 0x0006987C File Offset: 0x00067A7C
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			if (!Provider.hasCheats)
			{
				CommandWindow.LogError(this.localization.format("CheatsErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length < 1 || componentsFromSerial.Length > 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			bool flag = false;
			SteamPlayer steamPlayer;
			if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out steamPlayer))
			{
				steamPlayer = PlayerTool.getSteamPlayer(executorID);
				if (steamPlayer == null)
				{
					CommandWindow.LogError(this.localization.format("NoPlayerErrorText", componentsFromSerial[0]));
					return;
				}
				flag = true;
			}
			QuestAsset questAsset = null;
			string text = componentsFromSerial[flag ? 0 : 1];
			Guid guid;
			ushort num;
			if (Guid.TryParse(text, ref guid))
			{
				questAsset = Assets.find<QuestAsset>(guid);
			}
			else if (!ushort.TryParse(text, ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", text));
				return;
			}
			if (questAsset != null)
			{
				steamPlayer.player.quests.ServerAddQuest(questAsset);
			}
			CommandWindow.Log(this.localization.format("QuestText", steamPlayer.playerID.playerName, ((questAsset != null) ? questAsset.FriendlyName : null) ?? text));
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x000699AC File Offset: 0x00067BAC
		public CommandQuest(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("QuestCommandText");
			this._info = this.localization.format("QuestInfoText");
			this._help = this.localization.format("QuestHelpText");
		}
	}
}
