﻿using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003D7 RID: 983
	public class CommandVotify : Command
	{
		// Token: 0x06001D58 RID: 7512 RVA: 0x0006B400 File Offset: 0x00069600
		protected override void execute(CSteamID executorID, string parameter)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length != 6)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			bool voteAllowed;
			if (componentsFromSerial[0].ToLower() == "y")
			{
				voteAllowed = true;
			}
			else
			{
				if (!(componentsFromSerial[0].ToLower() == "n"))
				{
					CommandWindow.LogError(this.localization.format("InvalidBooleanErrorText", componentsFromSerial[0]));
					return;
				}
				voteAllowed = false;
			}
			float votePassCooldown;
			if (!float.TryParse(componentsFromSerial[1], ref votePassCooldown))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[1]));
				return;
			}
			float voteFailCooldown;
			if (!float.TryParse(componentsFromSerial[2], ref voteFailCooldown))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[2]));
				return;
			}
			float voteDuration;
			if (!float.TryParse(componentsFromSerial[3], ref voteDuration))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[3]));
				return;
			}
			float votePercentage;
			if (!float.TryParse(componentsFromSerial[4], ref votePercentage))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[4]));
				return;
			}
			byte votePlayers;
			if (!byte.TryParse(componentsFromSerial[5], ref votePlayers))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[5]));
				return;
			}
			ChatManager.voteAllowed = voteAllowed;
			ChatManager.votePassCooldown = votePassCooldown;
			ChatManager.voteFailCooldown = voteFailCooldown;
			ChatManager.voteDuration = voteDuration;
			ChatManager.votePercentage = votePercentage;
			ChatManager.votePlayers = votePlayers;
			CommandWindow.Log(this.localization.format("VotifyText"));
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x0006B574 File Offset: 0x00069774
		public CommandVotify(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("VotifyCommandText");
			this._info = this.localization.format("VotifyInfoText");
			this._help = this.localization.format("VotifyHelpText");
		}
	}
}
