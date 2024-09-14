using System;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Represents a vehicle the vendor is selling to players.
	/// </summary>
	// Token: 0x02000386 RID: 902
	public class VendorSellingVehicle : VendorSellingBase
	{
		/// <summary>
		/// Returned asset is not necessarily a vehicle asset yet: It can also be a VehicleRedirectorAsset which the
		/// vehicle spawner requires to properly set paint color.
		/// </summary>
		// Token: 0x06001C00 RID: 7168 RVA: 0x0006415F File Offset: 0x0006235F
		public Asset FindAsset()
		{
			return Assets.FindBaseVehicleAssetByGuidOrLegacyId(base.TargetAssetGuid, base.id);
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x00064174 File Offset: 0x00062374
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

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001C02 RID: 7170 RVA: 0x000641A8 File Offset: 0x000623A8
		public override string displayName
		{
			get
			{
				VehicleAsset vehicleAsset = this.FindVehicleAssetAndHandleRedirects();
				if (vehicleAsset == null)
				{
					return null;
				}
				return vehicleAsset.vehicleName;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001C03 RID: 7171 RVA: 0x000641C8 File Offset: 0x000623C8
		public override EItemRarity rarity
		{
			get
			{
				VehicleAsset vehicleAsset = this.FindVehicleAssetAndHandleRedirects();
				if (vehicleAsset == null)
				{
					return EItemRarity.COMMON;
				}
				return vehicleAsset.rarity;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001C04 RID: 7172 RVA: 0x000641E7 File Offset: 0x000623E7
		public override bool hasIcon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001C05 RID: 7173 RVA: 0x000641EA File Offset: 0x000623EA
		// (set) Token: 0x06001C06 RID: 7174 RVA: 0x000641F2 File Offset: 0x000623F2
		public string spawnpoint { get; protected set; }

		/// <summary>
		/// If set, takes priority over VehicleRedirectorAsset's paint color and over VehicleAsset's default paint color.
		/// </summary>
		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001C07 RID: 7175 RVA: 0x000641FB File Offset: 0x000623FB
		// (set) Token: 0x06001C08 RID: 7176 RVA: 0x00064203 File Offset: 0x00062403
		public Color32? paintColor { get; protected set; }

		// Token: 0x06001C09 RID: 7177 RVA: 0x0006420C File Offset: 0x0006240C
		public override void buy(Player player)
		{
			base.buy(player);
			Asset asset = this.FindAsset();
			if (asset == null)
			{
				return;
			}
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
				UnturnedLog.error("Failed to find vendor selling spawnpoint: " + this.spawnpoint);
				point = VehicleTool.GetPositionForVehicle(player);
				rotation = player.transform.rotation;
			}
			VehicleManager.spawnLockedVehicleForPlayerV2(asset, point, rotation, player, this.paintColor);
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x00064297 File Offset: 0x00062497
		public VendorSellingVehicle(VendorAsset newOuterAsset, byte newIndex, Guid newTargetAssetGuid, ushort newTargetAssetLegacyId, uint newCost, string newSpawnpoint, Color32? newPaintColor, INPCCondition[] newConditions, NPCRewardsList newRewardsList) : base(newOuterAsset, newIndex, newTargetAssetGuid, newTargetAssetLegacyId, newCost, newConditions, newRewardsList)
		{
			this.spawnpoint = newSpawnpoint;
			this.paintColor = newPaintColor;
		}
	}
}
