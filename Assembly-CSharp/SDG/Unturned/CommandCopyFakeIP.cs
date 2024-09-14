using System;
using Steamworks;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200039C RID: 924
	public class CommandCopyFakeIP : Command
	{
		// Token: 0x06001CCE RID: 7374 RVA: 0x000668EC File Offset: 0x00064AEC
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
			if (!Provider.configData.Server.Use_FakeIP)
			{
				CommandWindow.Log("Cannot copy Fake IP to clipboard because it's turned off in the server config.");
				return;
			}
			SteamNetworkingFakeIPResult_t steamNetworkingFakeIPResult_t;
			SteamGameServerNetworkingSockets.GetFakeIP(0, out steamNetworkingFakeIPResult_t);
			if (steamNetworkingFakeIPResult_t.m_eResult == EResult.k_EResultBusy)
			{
				CommandWindow.Log("Cannot copy Fake IP to clipboard because it's not ready yet.");
				return;
			}
			if (steamNetworkingFakeIPResult_t.m_eResult == EResult.k_EResultOK)
			{
				string text = new IPv4Address(steamNetworkingFakeIPResult_t.m_unIP).ToString();
				string text2 = string.Format("{0}:{1}", text, steamNetworkingFakeIPResult_t.m_unPorts[0]);
				GUIUtility.systemCopyBuffer = text2;
				CommandWindow.Log("Copied Fake IP (" + text2 + ") to clipboard");
				return;
			}
			CommandWindow.LogError(string.Format("Copy Fake IP to clipboard fatal result: {0}", steamNetworkingFakeIPResult_t.m_eResult));
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x000669BA File Offset: 0x00064BBA
		public CommandCopyFakeIP(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = "CopyFakeIP";
			this._info = "CopyFakeIP";
			this._help = "Copies the Fake IP to the system clipboard. Your friends can join the server by Fake IP without port forwarding.";
		}
	}
}
