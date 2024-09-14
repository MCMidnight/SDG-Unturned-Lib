using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003A8 RID: 936
	public class CommandGive : Command
	{
		// Token: 0x06001CEE RID: 7406 RVA: 0x00067BEC File Offset: 0x00065DEC
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			if (!Provider.hasCheats)
			{
				CommandWindow.LogError(this.localization.format("CheatsErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length < 1 || componentsFromSerial.Length > 3)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			bool flag = false;
			SteamPlayer steamPlayer;
			if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out steamPlayer))
			{
				steamPlayer = PlayerTool.getSteamPlayer(executorID);
				if (steamPlayer == null)
				{
					CommandWindow.LogError(this.localization.format("NoPlayerErrorText", componentsFromSerial[0]));
					return;
				}
				flag = true;
			}
			uint num = 1U;
			if (flag)
			{
				if (componentsFromSerial.Length > 1 && !uint.TryParse(componentsFromSerial[1], ref num))
				{
					CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[1]));
					return;
				}
			}
			else if (componentsFromSerial.Length > 2 && !uint.TryParse(componentsFromSerial[2], ref num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", componentsFromSerial[2]));
				return;
			}
			string text = componentsFromSerial[flag ? 0 : 1];
			Guid guid;
			if (Guid.TryParse(text, ref guid))
			{
				Asset asset = Assets.find(guid);
				this.GiveAsset(steamPlayer, asset, num);
				return;
			}
			ushort itemID;
			if (ushort.TryParse(text, ref itemID))
			{
				this.giveItem(steamPlayer, itemID, (byte)num);
				return;
			}
			Asset asset2 = this.FindByString(text);
			this.GiveAsset(steamPlayer, asset2, num);
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x00067D44 File Offset: 0x00065F44
		private void GiveAsset(SteamPlayer player, Asset asset, uint amount)
		{
			if (asset is ItemAsset)
			{
				this.giveItem(player, asset.id, (byte)amount);
				return;
			}
			ItemCurrencyAsset itemCurrencyAsset = asset as ItemCurrencyAsset;
			if (itemCurrencyAsset != null)
			{
				itemCurrencyAsset.grantValue(player.player, amount);
			}
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x00067D80 File Offset: 0x00065F80
		private void giveItem(SteamPlayer player, ushort itemID, byte amount)
		{
			if (!ItemTool.tryForceGiveItem(player.player, itemID, amount))
			{
				CommandWindow.LogError(this.localization.format("NoItemIDErrorText", itemID));
				return;
			}
			CommandWindow.Log(this.localization.format("GiveText", player.playerID.playerName, itemID, amount));
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x00067DE4 File Offset: 0x00065FE4
		private Asset FindByString(string input)
		{
			input = input.Trim();
			if (string.IsNullOrEmpty(input))
			{
				return null;
			}
			List<ItemAsset> list = new List<ItemAsset>();
			Assets.find<ItemAsset>(list);
			foreach (ItemAsset itemAsset in list)
			{
				if (string.Equals(input, itemAsset.name, 3))
				{
					return itemAsset;
				}
			}
			foreach (ItemAsset itemAsset2 in list)
			{
				if (string.Equals(input, itemAsset2.itemName, 3))
				{
					return itemAsset2;
				}
			}
			foreach (ItemAsset itemAsset3 in list)
			{
				if (itemAsset3.name.Contains(input, 3))
				{
					return itemAsset3;
				}
			}
			foreach (ItemAsset itemAsset4 in list)
			{
				if (itemAsset4.itemName.Contains(input, 3))
				{
					return itemAsset4;
				}
			}
			return null;
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x00067F4C File Offset: 0x0006614C
		public CommandGive(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("GiveCommandText");
			this._info = this.localization.format("GiveInfoText");
			this._help = this.localization.format("GiveHelpText");
		}
	}
}
