using System;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200034C RID: 844
	public class NPCVehicleReward : INPCReward
	{
		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x0600194B RID: 6475 RVA: 0x0005ACC0 File Offset: 0x00058EC0
		// (set) Token: 0x0600194C RID: 6476 RVA: 0x0005ACC8 File Offset: 0x00058EC8
		public Guid VehicleGuid { get; protected set; }

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600194D RID: 6477 RVA: 0x0005ACD1 File Offset: 0x00058ED1
		// (set) Token: 0x0600194E RID: 6478 RVA: 0x0005ACD9 File Offset: 0x00058ED9
		[Obsolete]
		public ushort id { get; protected set; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600194F RID: 6479 RVA: 0x0005ACE2 File Offset: 0x00058EE2
		// (set) Token: 0x06001950 RID: 6480 RVA: 0x0005ACEA File Offset: 0x00058EEA
		public string spawnpoint { get; protected set; }

		/// <summary>
		/// If set, takes priority over VehicleRedirectorAsset's paint color and over VehicleAsset's default paint color.
		/// </summary>
		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001951 RID: 6481 RVA: 0x0005ACF3 File Offset: 0x00058EF3
		// (set) Token: 0x06001952 RID: 6482 RVA: 0x0005ACFB File Offset: 0x00058EFB
		public Color32? paintColor { get; protected set; }

		/// <summary>
		/// Returned asset is not necessarily a vehicle asset yet: It can also be a VehicleRedirectorAsset which the
		/// vehicle spawner requires to properly set paint color.
		/// </summary>
		// Token: 0x06001953 RID: 6483 RVA: 0x0005AD04 File Offset: 0x00058F04
		public Asset FindAsset()
		{
			return Assets.FindBaseVehicleAssetByGuidOrLegacyId(this.VehicleGuid, this.id);
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0005AD18 File Offset: 0x00058F18
		public VehicleAsset FindVehicleAssetAndHandleRedirects()
		{
			Asset asset = this.FindAsset();
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			if (vehicleRedirectorAsset != null)
			{
				asset = vehicleRedirectorAsset.TargetVehicle.Find();
			}
			return asset as VehicleAsset;
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0005AD4C File Offset: 0x00058F4C
		public override void GrantReward(Player player)
		{
			Spawnpoint spawnpoint = SpawnpointSystemV2.Get().FindSpawnpoint(this.spawnpoint);
			Vector3 point;
			Quaternion rotation;
			if (spawnpoint != null)
			{
				point = spawnpoint.transform.position;
				rotation = spawnpoint.transform.rotation;
			}
			else
			{
				UnturnedLog.error("Failed to find NPC vehicle reward spawnpoint: " + this.spawnpoint);
				point = VehicleTool.GetPositionForVehicle(player);
				rotation = player.transform.rotation;
			}
			VehicleManager.spawnLockedVehicleForPlayerV2(this.FindAsset(), point, rotation, player, this.paintColor);
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x0005ADCC File Offset: 0x00058FCC
		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Vehicle");
			}
			VehicleAsset vehicleAsset = this.FindVehicleAssetAndHandleRedirects();
			string arg;
			if (vehicleAsset != null)
			{
				arg = string.Concat(new string[]
				{
					"<color=",
					Palette.hex(ItemTool.getRarityColorUI(vehicleAsset.rarity)),
					">",
					vehicleAsset.vehicleName,
					"</color>"
				});
			}
			else
			{
				arg = "?";
			}
			return Local.FormatText(this.text, arg);
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0005AE60 File Offset: 0x00059060
		public override ISleekElement createUI(Player player)
		{
			string text = this.formatReward(player);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			if (this.FindVehicleAssetAndHandleRedirects() == null)
			{
				return null;
			}
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.SizeOffset_Y = 30f;
			sleekBox.SizeScale_X = 1f;
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_X = 5f;
			sleekLabel.SizeOffset_X = -10f;
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.SizeScale_Y = 1f;
			sleekLabel.TextAlignment = 3;
			sleekLabel.TextColor = 4;
			sleekLabel.TextContrastContext = 1;
			sleekLabel.AllowRichText = true;
			sleekLabel.Text = text;
			sleekBox.AddChild(sleekLabel);
			return sleekBox;
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x0005AF0F File Offset: 0x0005910F
		public NPCVehicleReward(Guid newVehicleGuid, ushort newID, string newSpawnpoint, Color32? newPaintColor, string newText) : base(newText)
		{
			this.VehicleGuid = newVehicleGuid;
			this.id = newID;
			this.spawnpoint = newSpawnpoint;
			this.paintColor = newPaintColor;
		}
	}
}
