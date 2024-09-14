using System;
using System.Collections.Generic;
using SDG.Provider;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Moved icon code from MenuTitleUI to here.
	/// </summary>
	// Token: 0x02000802 RID: 2050
	public class IconUtils
	{
		/// <summary>
		/// These directories are excluded from source control and Steam depots so they might not exist yet.
		/// </summary>
		// Token: 0x06004645 RID: 17989 RVA: 0x001A34E8 File Offset: 0x001A16E8
		public static void CreateExtrasDirectory()
		{
			ReadWrite.createFolder("/Extras/Econ");
			ReadWrite.createFolder("/Extras/Icons");
			ReadWrite.createFolder("/Extras/CosmeticPreviews_2048x2048");
			ReadWrite.createFolder("/Extras/CosmeticPreviews_400x400");
			ReadWrite.createFolder("/Extras/CosmeticPreviews_200x200");
			ReadWrite.createFolder("/Extras/OutfitPreviews_2048x2048");
			ReadWrite.createFolder("/Extras/OutfitPreviews_400x400");
			ReadWrite.createFolder("/Extras/OutfitPreviews_200x200");
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x001A3548 File Offset: 0x001A1748
		public static ItemDefIconInfo getItemDefIcon(ushort itemID, ushort vehicleID, ushort skinID)
		{
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, itemID) as ItemAsset;
			VehicleAsset vehicleAsset = VehicleTool.FindVehicleByLegacyIdAndHandleRedirects(vehicleID);
			if (itemAsset == null && vehicleAsset == null)
			{
				UnturnedLog.warn("Could not find a matching item ({0}) or vehicle ({1}) asset!", new object[]
				{
					itemID,
					vehicleID
				});
				return null;
			}
			return IconUtils.getItemDefIcon(itemAsset, vehicleAsset, skinID);
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x001A359C File Offset: 0x001A179C
		public static ItemDefIconInfo getItemDefIcon(ItemAsset itemAsset, VehicleAsset vehicleAsset, ushort skinID)
		{
			ItemDefIconInfo itemDefIconInfo = new ItemDefIconInfo();
			if (skinID != 0)
			{
				SkinAsset skinAsset = Assets.find(EAssetType.SKIN, skinID) as SkinAsset;
				if (skinAsset == null)
				{
					UnturnedLog.warn("Couldn't find a skin asset for: " + skinID.ToString());
					return null;
				}
				ushort id;
				if (vehicleAsset != null)
				{
					id = vehicleAsset.id;
				}
				else
				{
					id = itemAsset.id;
				}
				string text;
				if (vehicleAsset != null)
				{
					text = vehicleAsset.sharedSkinName;
				}
				else
				{
					text = itemAsset.name;
				}
				itemDefIconInfo.extraPath = string.Concat(new string[]
				{
					ReadWrite.PATH,
					"/Extras/Econ/",
					text,
					"_",
					id.ToString(),
					"_",
					skinAsset.name,
					"_",
					skinAsset.id.ToString()
				});
				if (vehicleAsset != null)
				{
					VehicleTool.getIcon(vehicleAsset.id, skinAsset.id, vehicleAsset, skinAsset, 200, 200, true, new VehicleIconReady(itemDefIconInfo.onSmallItemIconReady));
					VehicleTool.getIcon(vehicleAsset.id, skinAsset.id, vehicleAsset, skinAsset, 400, 400, true, new VehicleIconReady(itemDefIconInfo.onLargeItemIconReady));
				}
				else
				{
					ItemTool.getIcon(itemAsset.id, skinAsset.id, 100, itemAsset.getState(), itemAsset, skinAsset, string.Empty, string.Empty, 200, 200, true, true, new ItemIconReady(itemDefIconInfo.onSmallItemIconReady));
					ItemTool.getIcon(itemAsset.id, skinAsset.id, 100, itemAsset.getState(), itemAsset, skinAsset, string.Empty, string.Empty, 400, 400, true, true, new ItemIconReady(itemDefIconInfo.onLargeItemIconReady));
				}
			}
			else
			{
				if (itemAsset != null && string.IsNullOrEmpty(itemAsset.proPath))
				{
					UnturnedLog.error(string.Concat(new string[]
					{
						"Failed to find pro path for: ",
						itemAsset.id.ToString(),
						" ",
						((vehicleAsset != null) ? new ushort?(vehicleAsset.id) : default(ushort?)).ToString(),
						" ",
						skinID.ToString()
					}));
					return null;
				}
				itemDefIconInfo.extraPath = string.Concat(new string[]
				{
					ReadWrite.PATH,
					"/Extras/Econ/",
					itemAsset.name,
					"_",
					itemAsset.id.ToString()
				});
				ItemTool.getIcon(itemAsset.id, 0, 100, itemAsset.getState(), itemAsset, null, string.Empty, string.Empty, 200, 200, true, true, new ItemIconReady(itemDefIconInfo.onSmallItemIconReady));
				ItemTool.getIcon(itemAsset.id, 0, 100, itemAsset.getState(), itemAsset, null, string.Empty, string.Empty, 400, 400, true, true, new ItemIconReady(itemDefIconInfo.onLargeItemIconReady));
			}
			IconUtils.icons.Add(itemDefIconInfo);
			return itemDefIconInfo;
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x001A3878 File Offset: 0x001A1A78
		public static void captureItemIcon(ItemAsset itemAsset)
		{
			if (itemAsset == null)
			{
				return;
			}
			ExtraItemIconInfo extraItemIconInfo = new ExtraItemIconInfo();
			extraItemIconInfo.extraPath = string.Concat(new string[]
			{
				ReadWrite.PATH,
				"/Extras/Icons/",
				itemAsset.name,
				"_",
				itemAsset.id.ToString()
			});
			ItemTool.getIcon(itemAsset.id, 0, 100, itemAsset.getState(), itemAsset, null, string.Empty, string.Empty, (int)itemAsset.size_x * 512, (int)itemAsset.size_y * 512, false, true, new ItemIconReady(extraItemIconInfo.onItemIconReady));
			IconUtils.extraIcons.Add(extraItemIconInfo);
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x001A3920 File Offset: 0x001A1B20
		public static void captureAllItemIcons()
		{
			List<ItemAsset> list = new List<ItemAsset>();
			Assets.find<ItemAsset>(list);
			foreach (ItemAsset itemAsset in list)
			{
				IconUtils.captureItemIcon(itemAsset);
			}
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x001A3978 File Offset: 0x001A1B78
		public static void CaptureAllSkinIcons()
		{
			foreach (UnturnedEconInfo unturnedEconInfo in TempSteamworksEconomy.econInfo)
			{
				if (unturnedEconInfo.item_skin != 0)
				{
					ItemAsset itemAsset = Assets.find(unturnedEconInfo.item_guid) as ItemAsset;
					VehicleAsset vehicleAsset = Assets.find(unturnedEconInfo.vehicle_guid) as VehicleAsset;
					IconUtils.getItemDefIcon(itemAsset, vehicleAsset, (ushort)unturnedEconInfo.item_skin);
				}
			}
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x001A39FC File Offset: 0x001A1BFC
		public static void CaptureCosmeticPreviews()
		{
			IconUtils.InitCapturePreview();
			IconUtils.cosmeticPreviewCapture.CaptureCosmetics();
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x001A3A0D File Offset: 0x001A1C0D
		public static void CaptureAllOutfitPreviews()
		{
			IconUtils.InitCapturePreview();
			IconUtils.cosmeticPreviewCapture.CaptureAllOutfits();
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x001A3A1E File Offset: 0x001A1C1E
		public static void CaptureOutfitPreview(Guid guid)
		{
			IconUtils.InitCapturePreview();
			IconUtils.cosmeticPreviewCapture.CaptureOutfit(guid);
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x001A3A30 File Offset: 0x001A1C30
		private static void InitCapturePreview()
		{
			if (IconUtils.cosmeticPreviewGameObject == null)
			{
				IconUtils.cosmeticPreviewGameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Characters/CosmeticPreviewCapture"), new Vector3(-1000f, 0f, 0f), Quaternion.Euler(90f, 0f, 0f));
				IconUtils.cosmeticPreviewCapture = IconUtils.cosmeticPreviewGameObject.GetComponent<CosmeticPreviewCapture>();
			}
		}

		// Token: 0x04002F46 RID: 12102
		public static List<ItemDefIconInfo> icons = new List<ItemDefIconInfo>();

		// Token: 0x04002F47 RID: 12103
		public static List<ExtraItemIconInfo> extraIcons = new List<ExtraItemIconInfo>();

		// Token: 0x04002F48 RID: 12104
		private static GameObject cosmeticPreviewGameObject;

		// Token: 0x04002F49 RID: 12105
		private static CosmeticPreviewCapture cosmeticPreviewCapture;
	}
}
