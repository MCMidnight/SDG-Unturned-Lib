using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003BE RID: 958
	public class CommandPlayers : Command
	{
		// Token: 0x06001D21 RID: 7457 RVA: 0x000695FC File Offset: 0x000677FC
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (Provider.clients.Count == 0)
			{
				CommandWindow.LogError(this.localization.format("NoPlayersErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("PlayersText"));
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				CommandWindow.Log(this.localization.format("PlayerIDText", new object[]
				{
					steamPlayer.playerID.steamID,
					steamPlayer.playerID.playerName,
					steamPlayer.playerID.characterName,
					(int)(steamPlayer.ping * 1000f)
				}));
			}
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000696C0 File Offset: 0x000678C0
		public CommandPlayers(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("PlayersCommandText");
			this._info = this.localization.format("PlayersInfoText");
			this._help = this.localization.format("PlayersHelpText");
		}
	}
}
