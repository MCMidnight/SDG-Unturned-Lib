using System;
using SDG.NetTransport;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003A2 RID: 930
	public class CommandEffectUI : Command
	{
		// Token: 0x06001CDA RID: 7386 RVA: 0x00066EA8 File Offset: 0x000650A8
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (executorID == CSteamID.Nil && Provider.clients.Count > 0)
			{
				executorID = Provider.clients[0].playerID.steamID;
			}
			ITransportConnection transportConnection = Provider.findTransportConnection(executorID);
			if (transportConnection == null)
			{
				return;
			}
			string[] array = parameter.Split('/', 0);
			string text = (array.Length != 0) ? array[0] : parameter;
			ushort num;
			if (!ushort.TryParse(text, ref num))
			{
				if (text.Equals("clearall", 3))
				{
					UnturnedLog.info("Clearing all effects");
					EffectManager.askEffectClearAll();
				}
				return;
			}
			if (array.Length < 2)
			{
				EffectManager.sendUIEffect(num, 1, transportConnection, true);
				return;
			}
			if (array.Length == 2)
			{
				if (array[1].Equals("clearbyid", 2))
				{
					UnturnedLog.info("Clearing UI effects with ID {0}", new object[]
					{
						num
					});
					EffectManager.askEffectClearByID(num, transportConnection);
					return;
				}
				EffectManager.sendUIEffect(num, 1, transportConnection, true, array[1]);
				return;
			}
			else
			{
				if (array.Length == 3)
				{
					EffectManager.sendUIEffect(num, 1, transportConnection, true, array[1], array[2]);
					return;
				}
				if (array.Length == 4)
				{
					EffectManager.sendUIEffect(num, 1, transportConnection, true, array[1], array[2], array[3]);
					return;
				}
				if (array.Length == 5)
				{
					EffectManager.sendUIEffect(num, 1, transportConnection, true, array[1], array[2], array[3], array[4]);
				}
				return;
			}
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x00066FD4 File Offset: 0x000651D4
		public CommandEffectUI(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("EffectCommandText");
			this._info = this.localization.format("EffectInfoText");
			this._help = this.localization.format("EffectHelpText");
		}
	}
}
