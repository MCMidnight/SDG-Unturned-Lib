using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003B4 RID: 948
	public class CommandMaxPlayers : Command
	{
		// Token: 0x06001D0B RID: 7435 RVA: 0x00068C4C File Offset: 0x00066E4C
		protected override void execute(CSteamID executorID, string parameter)
		{
			byte b;
			if (!byte.TryParse(parameter, ref b))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
				return;
			}
			if (b < CommandMaxPlayers.MIN_NUMBER)
			{
				CommandWindow.LogError(this.localization.format("MinNumberErrorText", CommandMaxPlayers.MIN_NUMBER));
				return;
			}
			if (b > CommandMaxPlayers.MAX_NUMBER)
			{
				CommandWindow.LogError(this.localization.format("MaxNumberErrorText", CommandMaxPlayers.MAX_NUMBER));
				return;
			}
			Provider.maxPlayers = b;
			CommandWindow.Log(this.localization.format("MaxPlayersText", b));
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x00068CEC File Offset: 0x00066EEC
		public CommandMaxPlayers(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("MaxPlayersCommandText");
			this._info = this.localization.format("MaxPlayersInfoText");
			this._help = this.localization.format("MaxPlayersHelpText");
		}

		// Token: 0x04000DC4 RID: 3524
		public static readonly byte MIN_NUMBER = 1;

		// Token: 0x04000DC5 RID: 3525
		[Obsolete]
		public static readonly byte RECOMMENDED_NUMBER = 24;

		// Token: 0x04000DC6 RID: 3526
		public static readonly byte MAX_NUMBER = 200;
	}
}
