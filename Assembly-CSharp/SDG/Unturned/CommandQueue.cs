using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003C2 RID: 962
	public class CommandQueue : Command
	{
		// Token: 0x06001D29 RID: 7465 RVA: 0x00069A08 File Offset: 0x00067C08
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (parameter == "a")
			{
				SteamPending steamPending = new SteamPending();
				if (Provider.pending.Count == 1)
				{
					steamPending.sendVerifyPacket();
				}
				Provider.pending.Add(steamPending);
				CommandWindow.Log("add dummy");
				return;
			}
			if (parameter == "r")
			{
				Provider.reject(CSteamID.Nil, ESteamRejection.PING);
				CommandWindow.Log("rmv dummy");
				return;
			}
			if (parameter == "ad")
			{
				for (int i = 0; i < 12; i++)
				{
					Provider.pending.Add(new SteamPending(null, new SteamPlayerID(CSteamID.Nil, 0, "dummy", "dummy", "dummy", CSteamID.Nil), true, 0, 0, 0, Color.white, Color.white, Color.white, false, 0UL, 0UL, 0UL, 0UL, 0UL, 0UL, 0UL, new ulong[0], EPlayerSkillset.NONE, "english", CSteamID.Nil, EClientPlatform.Windows));
					Provider.accept(new SteamPlayerID(CSteamID.Nil, 1, "dummy", "dummy", "dummy", CSteamID.Nil), true, true, 0, 0, 0, Color.white, Color.white, Color.white, false, 0, 0, 0, 0, 0, 0, 0, new int[0], new string[0], new string[0], EPlayerSkillset.NONE, "english", CSteamID.Nil, EClientPlatform.Windows);
				}
			}
			else if (parameter == "rd")
			{
				for (int j = Provider.clients.Count - 1; j >= 0; j--)
				{
					Provider.kick(CSteamID.Nil, "");
				}
			}
			byte b;
			if (!byte.TryParse(parameter, ref b))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", parameter));
				return;
			}
			if (b > CommandQueue.MAX_NUMBER)
			{
				CommandWindow.LogError(this.localization.format("MaxNumberErrorText", CommandQueue.MAX_NUMBER));
				return;
			}
			Provider.queueSize = b;
			CommandWindow.Log(this.localization.format("QueueText", b));
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x00069BF8 File Offset: 0x00067DF8
		public CommandQueue(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("QueueCommandText");
			this._info = this.localization.format("QueueInfoText");
			this._help = this.localization.format("QueueHelpText");
		}

		// Token: 0x04000DC9 RID: 3529
		public static readonly byte MAX_NUMBER = 64;
	}
}
