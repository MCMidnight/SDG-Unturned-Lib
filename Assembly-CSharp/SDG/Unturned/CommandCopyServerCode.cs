using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200039D RID: 925
	public class CommandCopyServerCode : Command
	{
		// Token: 0x06001CD0 RID: 7376 RVA: 0x000669EC File Offset: 0x00064BEC
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
			string text = SteamGameServer.GetSteamID().ToString();
			GUIUtility.systemCopyBuffer = text;
			CommandWindow.Log("Copied server code (" + text + ") to clipboard");
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x00066A3E File Offset: 0x00064C3E
		public CommandCopyServerCode(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = "CopyServerCode";
			this._info = "CopyServerCode";
			this._help = "Copies the Server Code to the system clipboard. Your friends can join the server by Server Code without port forwarding.";
		}
	}
}
