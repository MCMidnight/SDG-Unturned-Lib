using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200076E RID: 1902
	public class VehicleTool : MonoBehaviour
	{
		/// <summary>
		/// Handles VehicleRedirectorAsset (if any) and returns actual vehicle asset (if any).
		/// </summary>
		// Token: 0x06003E1D RID: 15901 RVA: 0x0012EC78 File Offset: 0x0012CE78
		public static VehicleAsset FindVehicleByLegacyIdAndHandleRedirects(ushort legacyId)
		{
			Asset asset = Assets.find(EAssetType.VEHICLE, legacyId);
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			if (vehicleRedirectorAsset != null)
			{
				asset = vehicleRedirectorAsset.TargetVehicle.Find();
			}
			return asset as VehicleAsset;
		}

		/// <summary>
		/// Handles VehicleRedirectorAsset returning load paint color override (if any) and returns actual vehicle asset (if any).
		/// </summary>
		// Token: 0x06003E1E RID: 15902 RVA: 0x0012ECAC File Offset: 0x0012CEAC
		public static VehicleAsset FindVehicleByLegacyIdAndHandleRedirectsWithLoadColor(ushort legacyId, out Color32 paintColor)
		{
			paintColor = new Color32(0, 0, 0, 0);
			Asset asset = Assets.find(EAssetType.VEHICLE, legacyId);
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			if (vehicleRedirectorAsset != null)
			{
				if (vehicleRedirectorAsset.LoadPaintColor != null)
				{
					paintColor = vehicleRedirectorAsset.LoadPaintColor.Value;
				}
				asset = vehicleRedirectorAsset.TargetVehicle.Find();
			}
			return asset as VehicleAsset;
		}

		/// <summary>
		/// Handles VehicleRedirectorAsset returning spawn paint color override (if any) and returns actual vehicle asset (if any).
		/// </summary>
		// Token: 0x06003E1F RID: 15903 RVA: 0x0012ED14 File Offset: 0x0012CF14
		public static VehicleAsset FindVehicleByLegacyIdAndHandleRedirectsWithSpawnColor(ushort legacyId, out Color32 paintColor)
		{
			paintColor = new Color32(0, 0, 0, 0);
			Asset asset = Assets.find(EAssetType.VEHICLE, legacyId);
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			if (vehicleRedirectorAsset != null)
			{
				if (vehicleRedirectorAsset.SpawnPaintColor != null)
				{
					paintColor = vehicleRedirectorAsset.SpawnPaintColor.Value;
				}
				asset = vehicleRedirectorAsset.TargetVehicle.Find();
			}
			return asset as VehicleAsset;
		}

		/// <summary>
		/// Handles VehicleRedirectorAsset (if any) and returns actual vehicle asset (if any).
		/// </summary>
		// Token: 0x06003E20 RID: 15904 RVA: 0x0012ED7C File Offset: 0x0012CF7C
		public static VehicleAsset FindVehicleByGuidAndHandleRedirects(Guid guid)
		{
			Asset asset = Assets.find(guid);
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			if (vehicleRedirectorAsset != null)
			{
				asset = vehicleRedirectorAsset.TargetVehicle.Find();
			}
			return asset as VehicleAsset;
		}

		/// <summary>
		/// Handles VehicleRedirectorAsset returning load paint color override (if any) and returns actual vehicle asset (if any).
		/// </summary>
		// Token: 0x06003E21 RID: 15905 RVA: 0x0012EDB0 File Offset: 0x0012CFB0
		public static VehicleAsset FindVehicleByGuidAndHandleRedirectsWithLoadColor(Guid guid, out Color32 paintColor)
		{
			paintColor = new Color32(0, 0, 0, 0);
			Asset asset = Assets.find(guid);
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			if (vehicleRedirectorAsset != null)
			{
				if (vehicleRedirectorAsset.LoadPaintColor != null)
				{
					paintColor = vehicleRedirectorAsset.LoadPaintColor.Value;
				}
				asset = vehicleRedirectorAsset.TargetVehicle.Find();
			}
			return asset as VehicleAsset;
		}

		/// <summary>
		/// Handles VehicleRedirectorAsset returning spawn paint color override (if any) and returns actual vehicle asset (if any).
		/// </summary>
		// Token: 0x06003E22 RID: 15906 RVA: 0x0012EE18 File Offset: 0x0012D018
		public static VehicleAsset FindVehicleByGuidAndHandleRedirectsWithSpawnColor(Guid guid, out Color32 paintColor)
		{
			paintColor = new Color32(0, 0, 0, 0);
			Asset asset = Assets.find(guid);
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			if (vehicleRedirectorAsset != null)
			{
				if (vehicleRedirectorAsset.SpawnPaintColor != null)
				{
					paintColor = vehicleRedirectorAsset.SpawnPaintColor.Value;
				}
				asset = vehicleRedirectorAsset.TargetVehicle.Find();
			}
			return asset as VehicleAsset;
		}

		/// <summary>
		/// Handles VehicleRedirectorAsset (if any) and returns actual vehicle asset (if any).
		/// </summary>
		// Token: 0x06003E23 RID: 15907 RVA: 0x0012EE80 File Offset: 0x0012D080
		public static VehicleAsset HandleRedirects(Asset asset)
		{
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			if (vehicleRedirectorAsset != null)
			{
				asset = vehicleRedirectorAsset.TargetVehicle.Find();
			}
			return asset as VehicleAsset;
		}

		/// <summary>
		/// Handles VehicleRedirectorAsset returning load paint color override (if any) and returns actual vehicle asset (if any).
		/// </summary>
		// Token: 0x06003E24 RID: 15908 RVA: 0x0012EEB0 File Offset: 0x0012D0B0
		public static VehicleAsset HandleRedirectsWithLoadColor(Asset asset, out Color32 paintColor)
		{
			paintColor = new Color32(0, 0, 0, 0);
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			if (vehicleRedirectorAsset != null)
			{
				if (vehicleRedirectorAsset.LoadPaintColor != null)
				{
					paintColor = vehicleRedirectorAsset.LoadPaintColor.Value;
				}
				asset = vehicleRedirectorAsset.TargetVehicle.Find();
			}
			return asset as VehicleAsset;
		}

		/// <summary>
		/// Handles VehicleRedirectorAsset returning spawn paint color override (if any) and returns actual vehicle asset (if any).
		/// </summary>
		// Token: 0x06003E25 RID: 15909 RVA: 0x0012EF10 File Offset: 0x0012D110
		public static VehicleAsset HandleRedirectsWithSpawnColor(Asset asset, out Color32 paintColor)
		{
			paintColor = new Color32(0, 0, 0, 0);
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			if (vehicleRedirectorAsset != null)
			{
				if (vehicleRedirectorAsset.SpawnPaintColor != null)
				{
					paintColor = vehicleRedirectorAsset.SpawnPaintColor.Value;
				}
				asset = vehicleRedirectorAsset.TargetVehicle.Find();
			}
			return asset as VehicleAsset;
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x0012EF70 File Offset: 0x0012D170
		public static Transform getVehicle(ushort id, ushort skin, ushort mythic, VehicleAsset vehicleAsset, SkinAsset skinAsset)
		{
			GameObject gameObject = (vehicleAsset != null) ? vehicleAsset.GetOrLoadModel() : null;
			if (gameObject != null)
			{
				if (id != vehicleAsset.id)
				{
					UnturnedLog.error("ID and asset ID are not in sync!");
				}
				Transform transform = Object.Instantiate<GameObject>(gameObject).transform;
				transform.name = id.ToString();
				if (skinAsset != null)
				{
					InteractableVehicle interactableVehicle = transform.gameObject.AddComponent<InteractableVehicle>();
					interactableVehicle.id = id;
					interactableVehicle.skinID = skin;
					interactableVehicle.mythicID = mythic;
					interactableVehicle.fuel = 10000;
					interactableVehicle.isExploded = false;
					interactableVehicle.health = 10000;
					interactableVehicle.batteryCharge = 10000;
					interactableVehicle.safeInit(vehicleAsset);
					interactableVehicle.updateFires();
					interactableVehicle.updateSkin();
				}
				return transform;
			}
			Transform transform2 = new GameObject().transform;
			transform2.name = id.ToString();
			transform2.tag = "Vehicle";
			transform2.gameObject.layer = 26;
			return transform2;
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x0012F054 File Offset: 0x0012D254
		public static void getIcon(ushort id, ushort skin, VehicleAsset vehicleAsset, SkinAsset skinAsset, int x, int y, bool readableOnCPU, VehicleIconReady callback)
		{
			if (vehicleAsset != null && id != vehicleAsset.id)
			{
				UnturnedLog.error("ID and vehicle asset ID are not in sync!");
			}
			if (skinAsset != null && skin != skinAsset.id)
			{
				UnturnedLog.error("ID and skin asset ID are not in sync!");
			}
			VehicleIconInfo vehicleIconInfo = new VehicleIconInfo();
			vehicleIconInfo.id = id;
			vehicleIconInfo.skin = skin;
			vehicleIconInfo.vehicleAsset = vehicleAsset;
			vehicleIconInfo.skinAsset = skinAsset;
			vehicleIconInfo.x = x;
			vehicleIconInfo.y = y;
			vehicleIconInfo.readableOnCPU = readableOnCPU;
			vehicleIconInfo.callback = callback;
			VehicleTool.icons.Enqueue(vehicleIconInfo);
		}

		// Token: 0x06003E28 RID: 15912 RVA: 0x0012F0DC File Offset: 0x0012D2DC
		internal static Vector3 GetPositionForVehicle(Player player)
		{
			Vector3 vector = player.transform.position + player.transform.forward * 6f;
			RaycastHit raycastHit;
			Physics.Raycast(vector + Vector3.up * 16f, Vector3.down, out raycastHit, 32f, RayMasks.BLOCK_VEHICLE);
			if (raycastHit.collider != null)
			{
				vector.y = raycastHit.point.y + 16f;
			}
			return vector;
		}

		/// <summary>
		/// Supports redirects by VehicleRedirectorAsset. If redirector's SpawnPaintColor is set, that color is used.
		/// </summary>
		// Token: 0x06003E29 RID: 15913 RVA: 0x0012F164 File Offset: 0x0012D364
		public static InteractableVehicle SpawnVehicleForPlayer(Player player, Asset asset)
		{
			if (player == null || asset == null)
			{
				return null;
			}
			Vector3 positionForVehicle = VehicleTool.GetPositionForVehicle(player);
			return VehicleManager.spawnVehicleV2(asset, positionForVehicle, player.transform.rotation);
		}

		/// <summary>
		/// Supports redirects by VehicleRedirectorAsset. If redirector's SpawnPaintColor is set, that color is used.
		/// </summary>
		/// <returns>true if matching vehicle asset was found. (Not necessarily whether vehicle was spawned.)</returns>
		// Token: 0x06003E2A RID: 15914 RVA: 0x0012F198 File Offset: 0x0012D398
		public static bool giveVehicle(Player player, ushort id)
		{
			if (VehicleTool.FindVehicleByLegacyIdAndHandleRedirects(id) != null)
			{
				Vector3 positionForVehicle = VehicleTool.GetPositionForVehicle(player);
				VehicleManager.spawnVehicleV2(id, positionForVehicle, player.transform.rotation);
				return true;
			}
			return false;
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x0012F1CC File Offset: 0x0012D3CC
		private void Update()
		{
			if (VehicleTool.icons == null || VehicleTool.icons.Count == 0)
			{
				return;
			}
			VehicleIconInfo vehicleIconInfo = VehicleTool.icons.Dequeue();
			if (vehicleIconInfo == null)
			{
				return;
			}
			if (vehicleIconInfo.vehicleAsset == null)
			{
				return;
			}
			Transform vehicle = VehicleTool.getVehicle(vehicleIconInfo.id, vehicleIconInfo.skin, 0, vehicleIconInfo.vehicleAsset, vehicleIconInfo.skinAsset);
			vehicle.position = new Vector3(-256f, -256f, 0f);
			Transform transform = vehicle.Find("Icon2");
			if (transform == null)
			{
				Object.Destroy(vehicle.gameObject);
				Assets.reportError(vehicleIconInfo.vehicleAsset, "missing 'Icon2' Transform");
				return;
			}
			float size2_z = vehicleIconInfo.vehicleAsset.size2_z;
			Texture2D texture = ItemTool.captureIcon(vehicleIconInfo.id, vehicleIconInfo.skin, vehicle, transform, vehicleIconInfo.x, vehicleIconInfo.y, size2_z, vehicleIconInfo.readableOnCPU);
			VehicleIconReady callback = vehicleIconInfo.callback;
			if (callback == null)
			{
				return;
			}
			callback(texture);
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x0012F2B5 File Offset: 0x0012D4B5
		private void Start()
		{
			VehicleTool.icons = new Queue<VehicleIconInfo>();
		}

		// Token: 0x04002719 RID: 10009
		private static Queue<VehicleIconInfo> icons;
	}
}
