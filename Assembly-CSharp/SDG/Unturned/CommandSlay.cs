using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003CB RID: 971
	public class CommandSlay : Command
	{
		// Token: 0x06001D3C RID: 7484 RVA: 0x0006A2E8 File Offset: 0x000684E8
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 1 && componentsFromSerial.Length != 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			SteamPlayer steamPlayer;
			if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out steamPlayer))
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", componentsFromSerial[0]));
				return;
			}
			uint ipv4AddressOrZero = steamPlayer.getIPv4AddressOrZero();
			if (componentsFromSerial.Length == 1)
			{
				Provider.requestBanPlayer(executorID, steamPlayer.playerID.steamID, ipv4AddressOrZero, steamPlayer.playerID.GetHwids(), this.localization.format("SlayTextReason"), SteamBlacklist.PERMANENT);
			}
			else if (componentsFromSerial.Length == 2)
			{
				Provider.requestBanPlayer(executorID, steamPlayer.playerID.steamID, ipv4AddressOrZero, steamPlayer.playerID.GetHwids(), componentsFromSerial[1], SteamBlacklist.PERMANENT);
			}
			if (steamPlayer.player != null)
			{
				EPlayerKill eplayerKill;
				steamPlayer.player.life.askDamage(101, Vector3.up * 101f, EDeathCause.KILL, ELimb.SKULL, executorID, out eplayerKill);
			}
			CommandWindow.Log(this.localization.format("SlayText", steamPlayer.playerID.playerName));
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x0006A428 File Offset: 0x00068628
		public CommandSlay(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("SlayCommandText");
			this._info = this.localization.format("SlayInfoText");
			this._help = this.localization.format("SlayHelpText");
		}
	}
}
