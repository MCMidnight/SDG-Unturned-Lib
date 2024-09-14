using System;
using SDG.NetTransport;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003B2 RID: 946
	public class CommandLogTransportConnections : Command
	{
		// Token: 0x06001D07 RID: 7431 RVA: 0x00068AE8 File Offset: 0x00066CE8
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (executorID != CSteamID.Nil)
			{
				return;
			}
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				ITransportConnection transportConnection = steamPlayer.transportConnection;
				if (transportConnection == null)
				{
					CommandWindow.Log(string.Format("Client {0} has no transport connection", steamPlayer.playerID));
				}
				else
				{
					CommandWindow.Log(string.Format("{0} - {1}", transportConnection, transportConnection.GetAddressString(true)));
				}
			}
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x00068B84 File Offset: 0x00066D84
		public CommandLogTransportConnections(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = "LogTransportConnections";
			this._info = string.Empty;
			this._help = string.Empty;
		}
	}
}
