using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000396 RID: 918
	public class CommandBan : Command
	{
		// Token: 0x06001CC1 RID: 7361 RVA: 0x0006621C File Offset: 0x0006441C
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 1 && componentsFromSerial.Length != 2 && componentsFromSerial.Length != 3)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			CSteamID csteamID;
			if (!PlayerTool.tryGetSteamID(componentsFromSerial[0], out csteamID))
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", componentsFromSerial[0]));
				return;
			}
			ITransportConnection transportConnection = Provider.findTransportConnection(csteamID);
			uint ipToBan = 0U;
			if (transportConnection != null)
			{
				transportConnection.TryGetIPv4Address(out ipToBan);
			}
			SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(csteamID);
			IEnumerable<byte[]> hwidsToBan;
			if (steamPlayer != null)
			{
				hwidsToBan = steamPlayer.playerID.GetHwids();
			}
			else
			{
				hwidsToBan = null;
			}
			if (componentsFromSerial.Length == 1)
			{
				Provider.requestBanPlayer(executorID, csteamID, ipToBan, hwidsToBan, this.localization.format("BanTextReason"), SteamBlacklist.PERMANENT);
				CommandWindow.Log(this.localization.format("BanTextPermanent", csteamID));
				return;
			}
			if (componentsFromSerial.Length == 2)
			{
				Provider.requestBanPlayer(executorID, csteamID, ipToBan, hwidsToBan, componentsFromSerial[1], SteamBlacklist.PERMANENT);
				CommandWindow.Log(this.localization.format("BanTextPermanent", csteamID));
				return;
			}
			uint num;
			if (!uint.TryParse(componentsFromSerial[2], ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[2]));
				return;
			}
			Provider.requestBanPlayer(executorID, csteamID, ipToBan, hwidsToBan, componentsFromSerial[1], num);
			CommandWindow.Log(this.localization.format("BanText", csteamID, num));
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x000663A0 File Offset: 0x000645A0
		public CommandBan(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("BanCommandText");
			this._info = this.localization.format("BanInfoText");
			this._help = this.localization.format("BanHelpText");
		}
	}
}
