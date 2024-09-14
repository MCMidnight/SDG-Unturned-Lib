using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003D6 RID: 982
	public class CommandVehicle : Command
	{
		// Token: 0x06001D55 RID: 7509 RVA: 0x0006B0DC File Offset: 0x000692DC
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
			string text = componentsFromSerial[flag ? 0 : 1];
			Guid guid;
			Asset asset;
			ushort id;
			if (Guid.TryParse(text, ref guid))
			{
				asset = Assets.find(guid);
			}
			else if (ushort.TryParse(text, ref id))
			{
				asset = Assets.find(EAssetType.VEHICLE, id);
			}
			else
			{
				asset = this.FindByString(text);
			}
			if (asset == null)
			{
				CommandWindow.LogError(this.localization.format("NoVehicleIDErrorText", text));
				return;
			}
			InteractableVehicle interactableVehicle = VehicleTool.SpawnVehicleForPlayer(steamPlayer.player, asset);
			if (interactableVehicle == null)
			{
				CommandWindow.LogError(this.localization.format("NoVehicleIDErrorText", asset.FriendlyName));
				return;
			}
			CommandWindow.Log(this.localization.format("VehicleText", steamPlayer.playerID.playerName, interactableVehicle.asset.FriendlyName));
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x0006B23C File Offset: 0x0006943C
		public CommandVehicle(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("VehicleCommandText");
			this._info = this.localization.format("VehicleInfoText");
			this._help = this.localization.format("VehicleHelpText");
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x0006B298 File Offset: 0x00069498
		private Asset FindByString(string input)
		{
			input = input.Trim();
			if (string.IsNullOrEmpty(input))
			{
				return null;
			}
			List<VehicleAsset> list = new List<VehicleAsset>();
			Assets.find<VehicleAsset>(list);
			foreach (VehicleAsset vehicleAsset in list)
			{
				if (string.Equals(input, vehicleAsset.name, 3))
				{
					return vehicleAsset;
				}
			}
			foreach (VehicleAsset vehicleAsset2 in list)
			{
				if (string.Equals(input, vehicleAsset2.vehicleName, 3))
				{
					return vehicleAsset2;
				}
			}
			foreach (VehicleAsset vehicleAsset3 in list)
			{
				if (vehicleAsset3.name.Contains(input, 3))
				{
					return vehicleAsset3;
				}
			}
			foreach (VehicleAsset vehicleAsset4 in list)
			{
				if (vehicleAsset4.vehicleName.Contains(input, 3))
				{
					return vehicleAsset4;
				}
			}
			return null;
		}
	}
}
