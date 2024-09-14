using System;
using System.Collections.Generic;
using System.Diagnostics;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200062D RID: 1581
	public class PlayerInventory : PlayerCaller
	{
		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x060032F7 RID: 13047 RVA: 0x000E757C File Offset: 0x000E577C
		// (set) Token: 0x060032F8 RID: 13048 RVA: 0x000E7584 File Offset: 0x000E5784
		public Items[] items { get; private set; }

		/// <summary>
		/// Should be called every time something changes in the inventory.
		/// </summary>
		// Token: 0x060032F9 RID: 13049 RVA: 0x000E758D File Offset: 0x000E578D
		protected void incrementUpdateIndex()
		{
			this.receivedUpdateIndex++;
		}

		/// <summary>
		/// Helper to prevent checking the inventory every frame for systems that don't use events.
		/// </summary>
		// Token: 0x060032FA RID: 13050 RVA: 0x000E759D File Offset: 0x000E579D
		public bool doesSearchNeedRefresh(ref int index)
		{
			if (index == this.receivedUpdateIndex)
			{
				return false;
			}
			index = this.receivedUpdateIndex;
			return true;
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x060032FB RID: 13051 RVA: 0x000E75B4 File Offset: 0x000E57B4
		public bool shouldInventoryStopGestureCloseStorage
		{
			get
			{
				return !this.isStorageTrunk;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x060032FC RID: 13052 RVA: 0x000E75BF File Offset: 0x000E57BF
		public bool shouldInteractCloseStorage
		{
			get
			{
				return !this.isStorageTrunk;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x060032FD RID: 13053 RVA: 0x000E75CA File Offset: 0x000E57CA
		public bool shouldStorageOpenDashboard
		{
			get
			{
				return !this.isStorageTrunk;
			}
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x000E75D5 File Offset: 0x000E57D5
		public byte getWidth(byte page)
		{
			if ((int)page < this.items.Length)
			{
				return this.items[(int)page].width;
			}
			return 0;
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x000E75F1 File Offset: 0x000E57F1
		public byte getHeight(byte page)
		{
			if ((int)page < this.items.Length)
			{
				return this.items[(int)page].height;
			}
			return 0;
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x000E760D File Offset: 0x000E580D
		public byte getItemCount(byte page)
		{
			if ((int)page < this.items.Length)
			{
				return this.items[(int)page].getItemCount();
			}
			return 0;
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x000E7629 File Offset: 0x000E5829
		public ItemJar getItem(byte page, byte index)
		{
			if ((int)page < this.items.Length)
			{
				return this.items[(int)page].getItem(index);
			}
			return null;
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x000E7646 File Offset: 0x000E5846
		public byte getIndex(byte page, byte x, byte y)
		{
			if ((int)page < this.items.Length)
			{
				return this.items[(int)page].getIndex(x, y);
			}
			return byte.MaxValue;
		}

		// Token: 0x06003303 RID: 13059 RVA: 0x000E7668 File Offset: 0x000E5868
		public byte findIndex(byte page, byte x, byte y, out byte find_x, out byte find_y)
		{
			find_x = byte.MaxValue;
			find_y = byte.MaxValue;
			return this.items[(int)page].findIndex(x, y, out find_x, out find_y);
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x000E768D File Offset: 0x000E588D
		public void updateAmount(byte page, byte index, byte newAmount)
		{
			if (page >= PlayerInventory.PAGES || this.items == null || this.items[(int)page] == null)
			{
				return;
			}
			this.items[(int)page].updateAmount(index, newAmount);
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x000E76BC File Offset: 0x000E58BC
		public void updateQuality(byte page, byte index, byte newQuality)
		{
			if (page >= PlayerInventory.PAGES || this.items == null || this.items[(int)page] == null)
			{
				return;
			}
			this.items[(int)page].updateQuality(index, newQuality);
			ItemJar item = this.items[(int)page].getItem(index);
			if (item != null && base.player.equipment.checkSelection(page, item.x, item.y))
			{
				base.player.equipment.quality = newQuality;
			}
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x000E7735 File Offset: 0x000E5935
		public void updateState(byte page, byte index, byte[] newState)
		{
			if (page >= PlayerInventory.PAGES || this.items == null || this.items[(int)page] == null)
			{
				return;
			}
			this.items[(int)page].updateState(index, newState);
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x000E7764 File Offset: 0x000E5964
		public List<InventorySearch> search(EItemType type)
		{
			List<InventorySearch> list = new List<InventorySearch>();
			this.search(list, type);
			return list;
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x000E7780 File Offset: 0x000E5980
		public void search(List<InventorySearch> search, EItemType type)
		{
			for (byte b = PlayerInventory.SLOTS; b < PlayerInventory.PAGES - 2; b += 1)
			{
				this.items[(int)b].search(search, type);
			}
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x000E77B4 File Offset: 0x000E59B4
		[Obsolete]
		public List<InventorySearch> search(EItemType type, ushort[] calibers)
		{
			return this.search(type, calibers, true);
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x000E77C0 File Offset: 0x000E59C0
		public List<InventorySearch> search(EItemType type, ushort[] calibers, bool allowZeroCaliber)
		{
			List<InventorySearch> list = new List<InventorySearch>();
			foreach (ushort caliber in calibers)
			{
				this.search(list, type, caliber, allowZeroCaliber);
			}
			return list;
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x000E77F2 File Offset: 0x000E59F2
		[Obsolete]
		public void search(List<InventorySearch> search, EItemType type, ushort caliber)
		{
			this.search(search, type, caliber, true);
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x000E7800 File Offset: 0x000E5A00
		public void search(List<InventorySearch> search, EItemType type, ushort caliber, bool allowZeroCaliber)
		{
			for (byte b = PlayerInventory.SLOTS; b < PlayerInventory.PAGES - 2; b += 1)
			{
				this.items[(int)b].search(search, type, caliber, allowZeroCaliber);
			}
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x000E7838 File Offset: 0x000E5A38
		public List<InventorySearch> search(ushort id, bool findEmpty, bool findHealthy)
		{
			List<InventorySearch> list = new List<InventorySearch>();
			this.search(list, id, findEmpty, findHealthy);
			return list;
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x000E7858 File Offset: 0x000E5A58
		public void search(List<InventorySearch> search, ushort id, bool findEmpty, bool findHealthy)
		{
			for (byte b = PlayerInventory.SLOTS; b < PlayerInventory.PAGES - 2; b += 1)
			{
				this.items[(int)b].search(search, id, findEmpty, findHealthy);
			}
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x000E7890 File Offset: 0x000E5A90
		public List<InventorySearch> search(List<InventorySearch> search)
		{
			List<InventorySearch> list = new List<InventorySearch>();
			for (int i = 0; i < search.Count; i++)
			{
				InventorySearch inventorySearch = search[i];
				bool flag = true;
				for (int j = 0; j < list.Count; j++)
				{
					InventorySearch inventorySearch2 = list[j];
					if (inventorySearch2.jar.item.id == inventorySearch.jar.item.id && inventorySearch2.jar.item.amount == inventorySearch.jar.item.amount && inventorySearch2.jar.item.quality == inventorySearch.jar.item.quality)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(inventorySearch);
				}
			}
			return list;
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x000E7960 File Offset: 0x000E5B60
		public InventorySearch has(ushort id)
		{
			for (byte b = 0; b < PlayerInventory.PAGES - 1; b += 1)
			{
				InventorySearch inventorySearch = this.items[(int)b].has(id);
				if (inventorySearch != null)
				{
					return inventorySearch;
				}
			}
			return null;
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x000E7998 File Offset: 0x000E5B98
		public bool tryAddItem(Item item, byte x, byte y, byte page, byte rot)
		{
			if (page >= PlayerInventory.PAGES - 1)
			{
				return false;
			}
			if (item == null)
			{
				return false;
			}
			ItemAsset asset = item.GetAsset();
			if (asset == null || asset.isPro)
			{
				return false;
			}
			if (page < PlayerInventory.SLOTS && !asset.slot.canEquipInPage(page))
			{
				return false;
			}
			if (page < PlayerInventory.SLOTS)
			{
				rot = 0;
			}
			if (x == 255 && y == 255)
			{
				if (!this.items[(int)page].tryAddItem(item))
				{
					return false;
				}
			}
			else
			{
				if (this.items[(int)page].getItemCount() >= 200)
				{
					return false;
				}
				if (!this.items[(int)page].checkSpaceEmpty(x, y, asset.size_x, asset.size_y, rot))
				{
					return false;
				}
				this.items[(int)page].addItem(x, y, rot, item);
			}
			if (page < PlayerInventory.SLOTS)
			{
				base.player.equipment.sendSlot(page);
			}
			return true;
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x000E7A7C File Offset: 0x000E5C7C
		public bool tryAddItem(Item item, bool auto)
		{
			return this.tryAddItem(item, auto, true);
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x000E7A87 File Offset: 0x000E5C87
		public bool tryAddItem(Item item, bool auto, bool playEffect)
		{
			return this.tryAddItemAuto(item, auto, auto, auto, playEffect);
		}

		/// <summary>
		/// Helper for tryAddItemAuto.
		/// </summary>
		// Token: 0x06003314 RID: 13076 RVA: 0x000E7A94 File Offset: 0x000E5C94
		private bool tryAddItemEquip(Item item, byte page)
		{
			if (this.items[(int)page].tryAddItem(item))
			{
				base.player.equipment.sendSlot(page);
				if (!base.player.equipment.HasValidUseable)
				{
					base.player.equipment.ServerEquip(page, 0, 0);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x000E7AEC File Offset: 0x000E5CEC
		public bool tryAddItemAuto(Item item, bool autoEquipWeapon, bool autoEquipUseable, bool autoEquipClothing, bool playEffect)
		{
			if (item == null)
			{
				return false;
			}
			ItemAsset asset = item.GetAsset();
			if (asset == null || asset.isPro)
			{
				return false;
			}
			if (autoEquipWeapon && asset.canPlayerEquip)
			{
				if (asset.slot.canEquipAsSecondary() && this.tryAddItemEquip(item, 1))
				{
					return true;
				}
				if (asset.slot.canEquipAsPrimary() && this.tryAddItemEquip(item, 0))
				{
					return true;
				}
			}
			if (autoEquipClothing)
			{
				if (base.player.clothing.hatAsset == null && asset.type == EItemType.HAT)
				{
					base.player.clothing.askWearHat(item.id, item.quality, item.state, playEffect);
					return true;
				}
				if (base.player.clothing.shirtAsset == null && asset.type == EItemType.SHIRT)
				{
					base.player.clothing.askWearShirt(item.id, item.quality, item.state, playEffect);
					return true;
				}
				if (base.player.clothing.pantsAsset == null && asset.type == EItemType.PANTS)
				{
					base.player.clothing.askWearPants(item.id, item.quality, item.state, playEffect);
					return true;
				}
				if (base.player.clothing.backpackAsset == null && asset.type == EItemType.BACKPACK)
				{
					base.player.clothing.askWearBackpack(item.id, item.quality, item.state, playEffect);
					return true;
				}
				if (base.player.clothing.vestAsset == null && asset.type == EItemType.VEST)
				{
					base.player.clothing.askWearVest(item.id, item.quality, item.state, playEffect);
					return true;
				}
				if (base.player.clothing.maskAsset == null && asset.type == EItemType.MASK)
				{
					base.player.clothing.askWearMask(item.id, item.quality, item.state, playEffect);
					return true;
				}
				if (base.player.clothing.glassesAsset == null && asset.type == EItemType.GLASSES)
				{
					base.player.clothing.askWearGlasses(item.id, item.quality, item.state, playEffect);
					return true;
				}
			}
			for (byte b = PlayerInventory.SLOTS; b < PlayerInventory.PAGES - 2; b += 1)
			{
				if (this.items[(int)b].tryAddItem(item))
				{
					if (autoEquipUseable && !base.player.equipment.HasValidUseable && asset.slot.canEquipInPage(b) && asset.canPlayerEquip)
					{
						ItemJar item2 = this.items[(int)b].getItem(this.items[(int)b].getItemCount() - 1);
						base.player.equipment.ServerEquip(b, item2.x, item2.y);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x000E7DAF File Offset: 0x000E5FAF
		public void forceAddItem(Item item, bool auto)
		{
			this.forceAddItemAuto(item, auto, auto, auto);
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x000E7DBB File Offset: 0x000E5FBB
		public void forceAddItemAuto(Item item, bool autoEquipWeapon, bool autoEquipUseable, bool autoEquipClothing)
		{
			this.forceAddItemAuto(item, autoEquipWeapon, autoEquipUseable, autoEquipClothing, true);
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x000E7DC9 File Offset: 0x000E5FC9
		public void forceAddItem(Item item, bool auto, bool playEffect)
		{
			if (!this.tryAddItemAuto(item, auto, auto, auto, playEffect))
			{
				ItemManager.dropItem(item, base.transform.position, false, true, true);
			}
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x000E7DEC File Offset: 0x000E5FEC
		public void forceAddItemAuto(Item item, bool autoEquipWeapon, bool autoEquipUseable, bool autoEquipClothing, bool playEffect)
		{
			if (!this.tryAddItemAuto(item, autoEquipWeapon, autoEquipUseable, autoEquipClothing, playEffect))
			{
				ItemManager.dropItem(item, base.transform.position, false, true, true);
			}
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x000E7E11 File Offset: 0x000E6011
		public void replaceItems(byte page, Items replacement)
		{
			this.items[(int)page] = replacement;
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x000E7E1C File Offset: 0x000E601C
		public void removeItem(byte page, byte index)
		{
			this.items[(int)page].removeItem(index);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x000E7E2C File Offset: 0x000E602C
		public bool checkSpaceEmpty(byte page, byte x, byte y, byte size_x, byte size_y, byte rot)
		{
			return page >= 0 && page < PlayerInventory.PAGES && this.items[(int)page].checkSpaceEmpty(x, y, size_x, size_y, rot);
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000E7E54 File Offset: 0x000E6054
		public bool checkSpaceDrag(byte page, byte old_x, byte old_y, byte oldRot, byte new_x, byte new_y, byte newRot, byte size_x, byte size_y, bool checkSame)
		{
			return page >= 0 && page < PlayerInventory.PAGES && this.items[(int)page].checkSpaceDrag(old_x, old_y, oldRot, new_x, new_y, newRot, size_x, size_y, checkSame);
		}

		/// <summary>
		/// Given an item coordinate (page, x, y) could a new item take the place of an old (existing) item without
		/// overlapping other item(s) space? Always true for equipment slots (page less than SLOTS).
		/// For example if oldSize is (1, 2) rot 0, and newSize is (2, 1) rot 1, then they can swap.
		/// </summary>
		// Token: 0x0600331E RID: 13086 RVA: 0x000E7E8C File Offset: 0x000E608C
		public bool checkSpaceSwap(byte page, byte x, byte y, byte oldSize_X, byte oldSize_Y, byte oldRot, byte newSize_X, byte newSize_Y, byte newRot)
		{
			return page >= 0 && page < PlayerInventory.PAGES && this.items[(int)page].checkSpaceSwap(x, y, oldSize_X, oldSize_Y, oldRot, newSize_X, newSize_Y, newRot);
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x000E7EC2 File Offset: 0x000E60C2
		public bool tryFindSpace(byte page, byte size_x, byte size_y, out byte x, out byte y, out byte rot)
		{
			x = 0;
			y = 0;
			rot = 0;
			return page >= 0 && page < PlayerInventory.PAGES && this.items[(int)page].tryFindSpace(size_x, size_y, out x, out y, out rot);
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x000E7EF4 File Offset: 0x000E60F4
		public bool tryFindSpace(byte size_x, byte size_y, out byte page, out byte x, out byte y, out byte rot)
		{
			x = 0;
			y = 0;
			rot = 0;
			for (page = PlayerInventory.SLOTS; page < PlayerInventory.PAGES - 1; page += 1)
			{
				if (this.items[(int)page].tryFindSpace(size_x, size_y, out x, out y, out rot))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x000E7F43 File Offset: 0x000E6143
		[Obsolete]
		public void askDragItem(CSteamID steamID, byte page_0, byte x_0, byte y_0, byte page_1, byte x_1, byte y_1, byte rot_1)
		{
			this.ReceiveDragItem(page_0, x_0, y_0, page_1, x_1, y_1, rot_1);
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x000E7F58 File Offset: 0x000E6158
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10, legacyName = "askDragItem")]
		public void ReceiveDragItem(byte page_0, byte x_0, byte y_0, byte page_1, byte x_1, byte y_1, byte rot_1)
		{
			if (base.player.equipment.checkSelection(page_0, x_0, y_0))
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				base.player.equipment.dequip();
			}
			else if (base.player.equipment.checkSelection(page_1, x_1, y_1))
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				base.player.equipment.dequip();
			}
			if (page_0 < 0 || page_0 >= PlayerInventory.PAGES - 1)
			{
				return;
			}
			if (this.items[(int)page_0] == null)
			{
				return;
			}
			byte index = this.items[(int)page_0].getIndex(x_0, y_0);
			if (index == 255)
			{
				return;
			}
			if (page_1 < 0 || page_1 >= PlayerInventory.PAGES - 1)
			{
				return;
			}
			if (this.items[(int)page_1] == null)
			{
				return;
			}
			if (this.getItemCount(page_1) >= 200)
			{
				return;
			}
			ItemJar item = this.items[(int)page_0].getItem(index);
			if (item == null)
			{
				return;
			}
			if (!this.checkSpaceDrag(page_1, x_0, y_0, item.rot, x_1, y_1, rot_1, item.size_x, item.size_y, page_0 == page_1))
			{
				return;
			}
			ItemAsset asset = item.GetAsset();
			if (asset == null)
			{
				return;
			}
			if (page_1 < PlayerInventory.SLOTS && !asset.slot.canEquipInPage(page_1))
			{
				return;
			}
			if (page_1 < PlayerInventory.SLOTS)
			{
				rot_1 = 0;
			}
			this.removeItem(page_0, index);
			this.items[(int)page_1].addItem(x_1, y_1, rot_1, item.item);
			if (page_0 < PlayerInventory.SLOTS)
			{
				base.player.equipment.sendSlot(page_0);
			}
			if (page_1 < PlayerInventory.SLOTS)
			{
				base.player.equipment.sendSlot(page_1);
			}
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x000E8100 File Offset: 0x000E6300
		[Obsolete]
		public void askSwapItem(CSteamID steamID, byte page_0, byte x_0, byte y_0, byte rot_0, byte page_1, byte x_1, byte y_1, byte rot_1)
		{
			this.ReceiveSwapItem(page_0, x_0, y_0, rot_0, page_1, x_1, y_1, rot_1);
		}

		/// <summary>
		/// Swap coordinates of two existing items.
		/// Rotation is provided to handle differently shaped items e.g. a 1x2 item with a 2x1 item. 
		/// </summary>
		// Token: 0x06003324 RID: 13092 RVA: 0x000E8124 File Offset: 0x000E6324
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10, legacyName = "askSwapItem")]
		public void ReceiveSwapItem(byte page_0, byte x_0, byte y_0, byte rot_0, byte page_1, byte x_1, byte y_1, byte rot_1)
		{
			if (base.player.equipment.checkSelection(page_0, x_0, y_0))
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				base.player.equipment.dequip();
			}
			else if (base.player.equipment.checkSelection(page_1, x_1, y_1))
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				base.player.equipment.dequip();
			}
			if (page_0 == page_1 && x_0 == x_1 && y_0 == y_1 && rot_0 == rot_1)
			{
				return;
			}
			if (page_0 < 0 || page_0 >= PlayerInventory.PAGES - 1)
			{
				return;
			}
			if (this.items[(int)page_0] == null)
			{
				return;
			}
			byte index = this.items[(int)page_0].getIndex(x_0, y_0);
			if (index == 255)
			{
				return;
			}
			if (page_1 < 0 || page_1 >= PlayerInventory.PAGES - 1)
			{
				return;
			}
			if (this.items[(int)page_1] == null)
			{
				return;
			}
			byte b = this.items[(int)page_1].getIndex(x_1, y_1);
			if (b == 255)
			{
				return;
			}
			ItemJar item = this.items[(int)page_0].getItem(index);
			if (item == null)
			{
				return;
			}
			ItemJar item2 = this.items[(int)page_1].getItem(b);
			if (item2 == null)
			{
				return;
			}
			if (item == item2)
			{
				return;
			}
			if (!this.checkSpaceSwap(page_0, x_0, y_0, item.size_x, item.size_y, item.rot, item2.size_x, item2.size_y, rot_0))
			{
				return;
			}
			if (!this.checkSpaceSwap(page_1, x_1, y_1, item2.size_x, item2.size_y, item2.rot, item.size_x, item.size_y, rot_1))
			{
				return;
			}
			ItemAsset asset = item.GetAsset();
			if (asset == null)
			{
				return;
			}
			if (page_1 < PlayerInventory.SLOTS && !asset.slot.canEquipInPage(page_1))
			{
				return;
			}
			ItemAsset asset2 = item2.GetAsset();
			if (asset2 == null)
			{
				return;
			}
			if (page_0 < PlayerInventory.SLOTS && !asset2.slot.canEquipInPage(page_0))
			{
				return;
			}
			this.removeItem(page_0, index);
			if (page_0 == page_1 && b > index)
			{
				b -= 1;
			}
			this.removeItem(page_1, b);
			if (page_0 < PlayerInventory.SLOTS)
			{
				rot_0 = 0;
			}
			if (page_1 < PlayerInventory.SLOTS)
			{
				rot_1 = 0;
			}
			this.items[(int)page_0].addItem(x_0, y_0, rot_0, item2.item);
			this.items[(int)page_1].addItem(x_1, y_1, rot_1, item.item);
			if (page_0 < PlayerInventory.SLOTS)
			{
				base.player.equipment.sendSlot(page_0);
			}
			if (page_1 < PlayerInventory.SLOTS)
			{
				base.player.equipment.sendSlot(page_1);
			}
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x000E8398 File Offset: 0x000E6598
		public void sendDragItem(byte page_0, byte x_0, byte y_0, byte page_1, byte x_1, byte y_1, byte rot_1)
		{
			PlayerInventory.SendDragItem.Invoke(base.GetNetId(), ENetReliability.Unreliable, page_0, x_0, y_0, page_1, x_1, y_1, rot_1);
		}

		/// <summary>
		/// Swap coordinates of two existing items.
		/// Rotation is provided to handle differently shaped items e.g. a 1x2 item with a 2x1 item. 
		/// </summary>
		// Token: 0x06003326 RID: 13094 RVA: 0x000E83C4 File Offset: 0x000E65C4
		public void sendSwapItem(byte page_0, byte x_0, byte y_0, byte rot_0, byte page_1, byte x_1, byte y_1, byte rot_1)
		{
			PlayerInventory.SendSwapItem.Invoke(base.GetNetId(), ENetReliability.Unreliable, page_0, x_0, y_0, rot_0, page_1, x_1, y_1, rot_1);
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x000E83EF File Offset: 0x000E65EF
		[Obsolete]
		public void askDropItem(CSteamID steamID, byte page, byte x, byte y)
		{
			this.ReceiveDropItem(page, x, y);
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x000E83FC File Offset: 0x000E65FC
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10, legacyName = "askDropItem")]
		public void ReceiveDropItem(byte page, byte x, byte y)
		{
			if (base.player.equipment.checkSelection(page, x, y))
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				base.player.equipment.dequip();
			}
			if (page < 0 || page >= PlayerInventory.PAGES - 1)
			{
				return;
			}
			if (this.items[(int)page] == null)
			{
				return;
			}
			if (this.items == null)
			{
				return;
			}
			byte index = this.items[(int)page].getIndex(x, y);
			if (index == 255)
			{
				return;
			}
			ItemJar item = this.items[(int)page].getItem(index);
			if (item == null || item.item == null)
			{
				return;
			}
			ItemAsset asset = item.GetAsset();
			if (asset == null)
			{
				return;
			}
			bool allowManualDrop = asset.allowManualDrop;
			DropItemRequestHandler dropItemRequestHandler = this.onDropItemRequested;
			if (dropItemRequestHandler != null)
			{
				dropItemRequestHandler(this, item.item, ref allowManualDrop);
			}
			if (!allowManualDrop)
			{
				return;
			}
			ItemManager.dropItem(item.item, base.transform.position + base.transform.forward * 0.5f, true, true, false);
			this.removeItem(page, index);
			if (page < PlayerInventory.SLOTS)
			{
				base.player.equipment.sendSlot(page);
			}
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x000E851C File Offset: 0x000E671C
		public void sendDropItem(byte page, byte x, byte y)
		{
			PlayerInventory.SendDropItem.Invoke(base.GetNetId(), ENetReliability.Unreliable, page, x, y);
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x000E8532 File Offset: 0x000E6732
		[Obsolete]
		public void tellUpdateAmount(CSteamID steamID, byte page, byte index, byte amount)
		{
			this.ReceiveUpdateAmount(page, index, amount);
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x000E853E File Offset: 0x000E673E
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdateAmount")]
		public void ReceiveUpdateAmount(byte page, byte index, byte amount)
		{
			this.updateAmount(page, index, amount);
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x000E8549 File Offset: 0x000E6749
		[Obsolete]
		public void tellUpdateQuality(CSteamID steamID, byte page, byte index, byte quality)
		{
			this.ReceiveUpdateQuality(page, index, quality);
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x000E8555 File Offset: 0x000E6755
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdateQuality")]
		public void ReceiveUpdateQuality(byte page, byte index, byte quality)
		{
			this.updateQuality(page, index, quality);
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x000E8560 File Offset: 0x000E6760
		[Obsolete]
		public void tellUpdateInvState(CSteamID steamID, byte page, byte index, byte[] state)
		{
			this.ReceiveUpdateInvState(page, index, state);
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x000E856C File Offset: 0x000E676C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellUpdateInvState")]
		public void ReceiveUpdateInvState(byte page, byte index, byte[] state)
		{
			this.updateState(page, index, state);
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x000E8578 File Offset: 0x000E6778
		[Obsolete]
		public void tellItemAdd(CSteamID steamID, byte page, byte x, byte y, byte rot, ushort id, byte amount, byte quality, byte[] state)
		{
			this.ReceiveItemAdd(page, x, y, rot, id, amount, quality, state);
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x000E8599 File Offset: 0x000E6799
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellItemAdd")]
		public void ReceiveItemAdd(byte page, byte x, byte y, byte rot, ushort id, byte amount, byte quality, byte[] state)
		{
			if (page >= PlayerInventory.PAGES || this.items == null || this.items[(int)page] == null)
			{
				return;
			}
			this.items[(int)page].addItem(x, y, rot, new Item(id, amount, quality, state));
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x000E85D4 File Offset: 0x000E67D4
		[Obsolete]
		public void tellItemRemove(CSteamID steamID, byte page, byte x, byte y)
		{
			this.ReceiveItemRemove(page, x, y);
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x000E85E0 File Offset: 0x000E67E0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellItemRemove")]
		public void ReceiveItemRemove(byte page, byte x, byte y)
		{
			if (page >= PlayerInventory.PAGES || this.items == null || this.items[(int)page] == null)
			{
				return;
			}
			byte index = this.items[(int)page].getIndex(x, y);
			if (index == 255)
			{
				return;
			}
			this.items[(int)page].removeItem(index);
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x000E862F File Offset: 0x000E682F
		[Obsolete]
		public void tellSize(CSteamID steamID, byte page, byte newWidth, byte newHeight)
		{
			this.ReceiveSize(page, newWidth, newHeight);
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x000E863B File Offset: 0x000E683B
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellSize")]
		public void ReceiveSize(byte page, byte newWidth, byte newHeight)
		{
			if (page >= PlayerInventory.PAGES || this.items == null || this.items[(int)page] == null)
			{
				return;
			}
			this.items[(int)page].resize(newWidth, newHeight);
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x000E8667 File Offset: 0x000E6867
		[Obsolete]
		public void tellStoraging(CSteamID steamID)
		{
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x000E866C File Offset: 0x000E686C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveStoraging(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			reader.ReadBit(ref this.isStorageTrunk);
			byte newWidth;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newWidth);
			byte newHeight;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newHeight);
			this.items[(int)PlayerInventory.STORAGE].resize(newWidth, newHeight);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			for (byte b2 = 0; b2 < b; b2 += 1)
			{
				byte x;
				SystemNetPakReaderEx.ReadUInt8(reader, ref x);
				byte y;
				SystemNetPakReaderEx.ReadUInt8(reader, ref y);
				byte rot;
				SystemNetPakReaderEx.ReadUInt8(reader, ref rot);
				ushort newID;
				SystemNetPakReaderEx.ReadUInt16(reader, ref newID);
				byte newAmount;
				SystemNetPakReaderEx.ReadUInt8(reader, ref newAmount);
				byte newQuality;
				SystemNetPakReaderEx.ReadUInt8(reader, ref newQuality);
				byte b3;
				SystemNetPakReaderEx.ReadUInt8(reader, ref b3);
				byte[] array = new byte[(int)b3];
				reader.ReadBytes(array);
				this.items[(int)PlayerInventory.STORAGE].addItem(x, y, rot, new Item(newID, newAmount, newQuality, array));
			}
			this.isStoring = (this.items[(int)PlayerInventory.STORAGE].height > 0);
			if (this.isStoring)
			{
				InventoryStored inventoryStored = this.onInventoryStored;
				if (inventoryStored == null)
				{
					return;
				}
				inventoryStored();
			}
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x000E8776 File Offset: 0x000E6976
		[Obsolete]
		public void tellInventory(CSteamID steamID)
		{
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x000E8778 File Offset: 0x000E6978
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveInventory(in ClientInvocationContext context)
		{
			Player.isLoadingInventory = false;
			NetPakReader reader = context.reader;
			for (byte b = 0; b < PlayerInventory.PAGES - 2; b += 1)
			{
				byte newWidth;
				SystemNetPakReaderEx.ReadUInt8(reader, ref newWidth);
				byte newHeight;
				SystemNetPakReaderEx.ReadUInt8(reader, ref newHeight);
				this.items[(int)b].resize(newWidth, newHeight);
				byte b2;
				SystemNetPakReaderEx.ReadUInt8(reader, ref b2);
				for (byte b3 = 0; b3 < b2; b3 += 1)
				{
					byte x;
					SystemNetPakReaderEx.ReadUInt8(reader, ref x);
					byte y;
					SystemNetPakReaderEx.ReadUInt8(reader, ref y);
					byte rot;
					SystemNetPakReaderEx.ReadUInt8(reader, ref rot);
					ushort newID;
					SystemNetPakReaderEx.ReadUInt16(reader, ref newID);
					byte newAmount;
					SystemNetPakReaderEx.ReadUInt8(reader, ref newAmount);
					byte newQuality;
					SystemNetPakReaderEx.ReadUInt8(reader, ref newQuality);
					byte b4;
					SystemNetPakReaderEx.ReadUInt8(reader, ref b4);
					byte[] array = new byte[(int)b4];
					reader.ReadBytes(array);
					this.items[(int)b].addItem(x, y, rot, new Item(newID, newAmount, newQuality, array));
				}
			}
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x000E8858 File Offset: 0x000E6A58
		[Obsolete]
		public void askInventory(CSteamID steamID)
		{
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x000E885C File Offset: 0x000E6A5C
		internal void SendInitialPlayerState(SteamPlayer client)
		{
			if (base.channel.IsLocalPlayer)
			{
				Player.isLoadingInventory = false;
				for (byte b = 0; b < PlayerInventory.PAGES - 2; b += 1)
				{
					InventoryResized inventoryResized = this.onInventoryResized;
					if (inventoryResized != null)
					{
						inventoryResized(b, this.items[(int)b].width, this.items[(int)b].height);
					}
					for (byte b2 = 0; b2 < this.items[(int)b].getItemCount(); b2 += 1)
					{
						ItemJar item = this.items[(int)b].getItem(b2);
						this.onItemAdded(b, b2, item);
					}
				}
			}
			else if (client == base.channel.owner)
			{
				PlayerInventory.SendInventory.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, delegate(NetPakWriter writer)
				{
					for (byte b3 = 0; b3 < PlayerInventory.PAGES - 2; b3 += 1)
					{
						SystemNetPakWriterEx.WriteUInt8(writer, this.items[(int)b3].width);
						SystemNetPakWriterEx.WriteUInt8(writer, this.items[(int)b3].height);
						SystemNetPakWriterEx.WriteUInt8(writer, this.items[(int)b3].getItemCount());
						for (byte b4 = 0; b4 < this.items[(int)b3].getItemCount(); b4 += 1)
						{
							ItemJar item2 = this.items[(int)b3].getItem(b4);
							SystemNetPakWriterEx.WriteUInt8(writer, item2.x);
							SystemNetPakWriterEx.WriteUInt8(writer, item2.y);
							SystemNetPakWriterEx.WriteUInt8(writer, item2.rot);
							SystemNetPakWriterEx.WriteUInt16(writer, item2.item.id);
							SystemNetPakWriterEx.WriteUInt8(writer, item2.item.amount);
							SystemNetPakWriterEx.WriteUInt8(writer, item2.item.quality);
							SystemNetPakWriterEx.WriteUInt8(writer, (byte)item2.item.state.Length);
							writer.WriteBytes(item2.item.state);
						}
					}
				});
			}
			this.ownerHasInventory = true;
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x000E8928 File Offset: 0x000E6B28
		public void sendStorage()
		{
			if (base.channel.IsLocalPlayer)
			{
				this.onInventoryResized(PlayerInventory.STORAGE, this.items[(int)PlayerInventory.STORAGE].width, this.items[(int)PlayerInventory.STORAGE].height);
				if (this.items[(int)PlayerInventory.STORAGE].height > 0)
				{
					InventoryStored inventoryStored = this.onInventoryStored;
					if (inventoryStored != null)
					{
						inventoryStored();
					}
				}
				for (byte b = 0; b < this.items[(int)PlayerInventory.STORAGE].getItemCount(); b += 1)
				{
					ItemJar item = this.items[(int)PlayerInventory.STORAGE].getItem(b);
					this.onItemAdded(PlayerInventory.STORAGE, b, item);
				}
				return;
			}
			PlayerInventory.SendStoraging.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.owner.transportConnection, delegate(NetPakWriter writer)
			{
				writer.WriteBit(this.isStorageTrunk);
				SystemNetPakWriterEx.WriteUInt8(writer, this.items[(int)PlayerInventory.STORAGE].width);
				SystemNetPakWriterEx.WriteUInt8(writer, this.items[(int)PlayerInventory.STORAGE].height);
				SystemNetPakWriterEx.WriteUInt8(writer, this.items[(int)PlayerInventory.STORAGE].getItemCount());
				for (byte b2 = 0; b2 < this.items[(int)PlayerInventory.STORAGE].getItemCount(); b2 += 1)
				{
					ItemJar item2 = this.items[(int)PlayerInventory.STORAGE].getItem(b2);
					SystemNetPakWriterEx.WriteUInt8(writer, item2.x);
					SystemNetPakWriterEx.WriteUInt8(writer, item2.y);
					SystemNetPakWriterEx.WriteUInt8(writer, item2.rot);
					SystemNetPakWriterEx.WriteUInt16(writer, item2.item.id);
					SystemNetPakWriterEx.WriteUInt8(writer, item2.item.amount);
					SystemNetPakWriterEx.WriteUInt8(writer, item2.item.quality);
					SystemNetPakWriterEx.WriteUInt8(writer, (byte)item2.item.state.Length);
					writer.WriteBytes(item2.item.state);
				}
			});
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x000E8A08 File Offset: 0x000E6C08
		public void updateItems(byte page, Items newItems)
		{
			if (this.items[(int)page] != null)
			{
				Items items = this.items[(int)page];
				items.onItemsResized = (ItemsResized)Delegate.Remove(items.onItemsResized, new ItemsResized(this.onItemsResized));
				Items items2 = this.items[(int)page];
				items2.onItemUpdated = (ItemUpdated)Delegate.Remove(items2.onItemUpdated, new ItemUpdated(this.onItemUpdated));
				Items items3 = this.items[(int)page];
				items3.onItemAdded = (ItemAdded)Delegate.Remove(items3.onItemAdded, new ItemAdded(this.onItemAdded));
				Items items4 = this.items[(int)page];
				items4.onItemRemoved = (ItemRemoved)Delegate.Remove(items4.onItemRemoved, new ItemRemoved(this.onItemRemoved));
				Items items5 = this.items[(int)page];
				items5.onStateUpdated = (StateUpdated)Delegate.Remove(items5.onStateUpdated, new StateUpdated(this.onItemStateUpdated));
			}
			if (newItems != null)
			{
				this.items[(int)page] = newItems;
				Items items6 = this.items[(int)page];
				items6.onItemsResized = (ItemsResized)Delegate.Combine(items6.onItemsResized, new ItemsResized(this.onItemsResized));
				Items items7 = this.items[(int)page];
				items7.onItemUpdated = (ItemUpdated)Delegate.Combine(items7.onItemUpdated, new ItemUpdated(this.onItemUpdated));
				Items items8 = this.items[(int)page];
				items8.onItemAdded = (ItemAdded)Delegate.Combine(items8.onItemAdded, new ItemAdded(this.onItemAdded));
				Items items9 = this.items[(int)page];
				items9.onItemRemoved = (ItemRemoved)Delegate.Combine(items9.onItemRemoved, new ItemRemoved(this.onItemRemoved));
				Items items10 = this.items[(int)page];
				items10.onStateUpdated = (StateUpdated)Delegate.Combine(items10.onStateUpdated, new StateUpdated(this.onItemStateUpdated));
				return;
			}
			this.items[(int)page] = new Items(PlayerInventory.STORAGE);
			Items items11 = this.items[(int)page];
			items11.onItemsResized = (ItemsResized)Delegate.Combine(items11.onItemsResized, new ItemsResized(this.onItemsResized));
			Items items12 = this.items[(int)page];
			items12.onItemUpdated = (ItemUpdated)Delegate.Combine(items12.onItemUpdated, new ItemUpdated(this.onItemUpdated));
			Items items13 = this.items[(int)page];
			items13.onItemAdded = (ItemAdded)Delegate.Combine(items13.onItemAdded, new ItemAdded(this.onItemAdded));
			Items items14 = this.items[(int)page];
			items14.onItemRemoved = (ItemRemoved)Delegate.Combine(items14.onItemRemoved, new ItemRemoved(this.onItemRemoved));
			Items items15 = this.items[(int)page];
			items15.onStateUpdated = (StateUpdated)Delegate.Combine(items15.onStateUpdated, new StateUpdated(this.onItemStateUpdated));
			InventoryResized inventoryResized = this.onInventoryResized;
			if (inventoryResized == null)
			{
				return;
			}
			inventoryResized(page, 0, 0);
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x000E8CC0 File Offset: 0x000E6EC0
		public void sendUpdateAmount(byte page, byte x, byte y, byte amount)
		{
			byte index = this.getIndex(page, x, y);
			this.updateAmount(page, index, amount);
			if (!base.channel.IsLocalPlayer && this.ownerHasInventory)
			{
				PlayerInventory.SendUpdateAmount.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), page, index, amount);
			}
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x000E8D18 File Offset: 0x000E6F18
		public void sendUpdateQuality(byte page, byte x, byte y, byte quality)
		{
			byte index = this.getIndex(page, x, y);
			this.updateQuality(page, index, quality);
			if (!base.channel.IsLocalPlayer && this.ownerHasInventory)
			{
				PlayerInventory.SendUpdateQuality.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), page, index, quality);
			}
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x000E8D70 File Offset: 0x000E6F70
		public void sendUpdateInvState(byte page, byte x, byte y, byte[] state)
		{
			byte index = this.getIndex(page, x, y);
			this.updateState(page, index, state);
			if (!base.channel.IsLocalPlayer && this.ownerHasInventory)
			{
				PlayerInventory.SendUpdateInvState.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), page, index, state);
			}
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x000E8DC8 File Offset: 0x000E6FC8
		private void sendItemAdd(byte page, ItemJar jar)
		{
			PlayerInventory.SendItemAdd.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), page, jar.x, jar.y, jar.rot, jar.item.id, jar.item.amount, jar.item.quality, jar.item.state);
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x000E8E30 File Offset: 0x000E7030
		private void sendItemRemove(byte page, ItemJar jar)
		{
			PlayerInventory.SendItemRemove.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), page, jar.x, jar.y);
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x000E8E5C File Offset: 0x000E705C
		private void bestowLoadout()
		{
			if (PlayerInventory.loadout != null && PlayerInventory.loadout.Length != 0)
			{
				for (int i = 0; i < PlayerInventory.loadout.Length; i++)
				{
					this.tryAddItem(new Item(PlayerInventory.loadout[i], EItemOrigin.ADMIN), true, false);
				}
			}
			else if (Level.info != null)
			{
				if (PlayerInventory.skillsets != null && PlayerInventory.skillsets[(int)((byte)base.channel.owner.skillset)] != null && PlayerInventory.skillsets[(int)((byte)base.channel.owner.skillset)].Length != 0)
				{
					for (int j = 0; j < PlayerInventory.skillsets[(int)((byte)base.channel.owner.skillset)].Length; j++)
					{
						this.tryAddItem(new Item(PlayerInventory.skillsets[(int)((byte)base.channel.owner.skillset)][j], EItemOrigin.WORLD), true, false);
					}
				}
				else if (Level.info.type == ELevelType.HORDE)
				{
					for (int k = 0; k < PlayerInventory.HORDE.Length; k++)
					{
						this.tryAddItem(new Item(PlayerInventory.HORDE[k], EItemOrigin.ADMIN), true, false);
					}
				}
			}
			if (Level.info != null)
			{
				foreach (ArenaLoadout arenaLoadout in Level.info.configData.Spawn_Loadouts)
				{
					for (ushort num = 0; num < arenaLoadout.Amount; num += 1)
					{
						ushort num2 = SpawnTableTool.ResolveLegacyId(arenaLoadout.Table_ID, EAssetType.ITEM, new Func<string>(this.OnGetSpawnLoadoutErrorContext));
						if (num2 != 0)
						{
							this.tryAddItemAuto(new Item(num2, true), true, false, true, false);
						}
					}
				}
			}
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x000E900C File Offset: 0x000E720C
		private string OnGetSpawnLoadoutErrorContext()
		{
			return "level config spawn loadout";
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x000E9014 File Offset: 0x000E7214
		private void onShirtUpdated(ushort id, byte quality, byte[] state)
		{
			if (id != 0)
			{
				ItemBagAsset itemBagAsset = Assets.find(EAssetType.ITEM, id) as ItemBagAsset;
				if (itemBagAsset != null)
				{
					this.items[(int)PlayerInventory.SHIRT].resize(itemBagAsset.width, itemBagAsset.height);
					return;
				}
			}
			else
			{
				this.items[(int)PlayerInventory.SHIRT].resize(0, 0);
			}
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x000E9068 File Offset: 0x000E7268
		private void onPantsUpdated(ushort id, byte quality, byte[] state)
		{
			if (id != 0)
			{
				ItemBagAsset itemBagAsset = Assets.find(EAssetType.ITEM, id) as ItemBagAsset;
				if (itemBagAsset != null)
				{
					this.items[(int)PlayerInventory.PANTS].resize(itemBagAsset.width, itemBagAsset.height);
					return;
				}
			}
			else
			{
				this.items[(int)PlayerInventory.PANTS].resize(0, 0);
			}
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x000E90BC File Offset: 0x000E72BC
		private void onBackpackUpdated(ushort id, byte quality, byte[] state)
		{
			if (id != 0)
			{
				ItemBagAsset itemBagAsset = Assets.find(EAssetType.ITEM, id) as ItemBagAsset;
				if (itemBagAsset != null)
				{
					this.items[(int)PlayerInventory.BACKPACK].resize(itemBagAsset.width, itemBagAsset.height);
					return;
				}
			}
			else
			{
				this.items[(int)PlayerInventory.BACKPACK].resize(0, 0);
			}
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x000E9110 File Offset: 0x000E7310
		private void onVestUpdated(ushort id, byte quality, byte[] state)
		{
			if (id != 0)
			{
				ItemBagAsset itemBagAsset = Assets.find(EAssetType.ITEM, id) as ItemBagAsset;
				if (itemBagAsset != null)
				{
					this.items[(int)PlayerInventory.VEST].resize(itemBagAsset.width, itemBagAsset.height);
					return;
				}
			}
			else
			{
				this.items[(int)PlayerInventory.VEST].resize(0, 0);
			}
		}

		/// <summary>
		/// Called from player movement to close storage that has moved away.
		/// </summary>
		// Token: 0x06003349 RID: 13129 RVA: 0x000E9164 File Offset: 0x000E7364
		public void closeDistantStorage()
		{
			if (!this.isStoring)
			{
				return;
			}
			if (this.isStorageTrunk)
			{
				return;
			}
			if (this.storage == null)
			{
				return;
			}
			if (!this.storage.shouldCloseWhenOutsideRange)
			{
				return;
			}
			Vector3 position = this.storage.transform.position;
			if ((base.transform.position - position).sqrMagnitude > 400f)
			{
				this.closeStorage();
			}
		}

		/// <summary>
		/// Serverside open a storage crate and notify client. 
		/// </summary>
		// Token: 0x0600334A RID: 13130 RVA: 0x000E91D8 File Offset: 0x000E73D8
		public void openStorage(InteractableStorage newStorage)
		{
			if (this.isStoring)
			{
				this.closeStorage();
			}
			newStorage.isOpen = true;
			newStorage.opener = base.player;
			this.isStoring = true;
			this.isStorageTrunk = false;
			this.storage = newStorage;
			this.updateItems(PlayerInventory.STORAGE, this.storage.items);
			this.sendStorage();
		}

		/// <summary>
		/// Serverside grant access to car trunk storage and notify client.
		/// </summary>
		// Token: 0x0600334B RID: 13131 RVA: 0x000E9237 File Offset: 0x000E7437
		public void openTrunk(Items trunkItems)
		{
			if (this.isStoring)
			{
				this.closeStorage();
			}
			this.isStoring = true;
			this.isStorageTrunk = true;
			this.storage = null;
			this.updateItems(PlayerInventory.STORAGE, trunkItems);
			this.sendStorage();
		}

		/// <summary>
		/// Serverside revoke trunk access and notify client.
		/// </summary>
		// Token: 0x0600334C RID: 13132 RVA: 0x000E926E File Offset: 0x000E746E
		public void closeTrunk()
		{
			if (!this.isStorageTrunk)
			{
				return;
			}
			this.closeStorageAndNotifyClient();
		}

		/// <summary>
		/// Called on both client and server, as well as by storage itself when destroyed.
		/// </summary>
		// Token: 0x0600334D RID: 13133 RVA: 0x000E9280 File Offset: 0x000E7480
		public void closeStorage()
		{
			if (!this.isStoring)
			{
				return;
			}
			this.isStoring = false;
			this.isStorageTrunk = false;
			if (this.storage != null)
			{
				if (Provider.isServer)
				{
					this.storage.isOpen = false;
					this.storage.opener = null;
				}
				this.storage = null;
			}
			this.updateItems(PlayerInventory.STORAGE, null);
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x000E92E4 File Offset: 0x000E74E4
		public void closeStorageAndNotifyClient()
		{
			if (this.isStoring)
			{
				this.closeStorage();
				this.sendStorage();
			}
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x000E92FC File Offset: 0x000E74FC
		private void onLifeUpdated(bool isDead)
		{
			if ((Provider.isServer || base.channel.IsLocalPlayer) && isDead)
			{
				this.closeStorage();
			}
			if (Provider.isServer)
			{
				if (base.player.life.wasPvPDeath ? Provider.modeConfigData.Players.Lose_Weapons_PvP : Provider.modeConfigData.Players.Lose_Weapons_PvE)
				{
					if (isDead)
					{
						this.items[0].resize(0, 0);
						this.items[1].resize(0, 0);
					}
					else
					{
						this.items[0].resize(1, 1);
						this.items[1].resize(1, 1);
					}
				}
				if (base.player.life.wasPvPDeath ? Provider.modeConfigData.Players.Lose_Clothes_PvP : Provider.modeConfigData.Players.Lose_Clothes_PvE)
				{
					if (isDead)
					{
						for (byte b = PlayerInventory.SLOTS; b < PlayerInventory.PAGES - 2; b += 1)
						{
							this.items[(int)b].resize(0, 0);
						}
						return;
					}
					this.items[2].resize(5, 3);
					this.bestowLoadout();
					return;
				}
				else if (isDead)
				{
					float num = base.player.life.wasPvPDeath ? Provider.modeConfigData.Players.Lose_Items_PvP : Provider.modeConfigData.Players.Lose_Items_PvE;
					for (byte b2 = PlayerInventory.SLOTS; b2 < PlayerInventory.PAGES - 2; b2 += 1)
					{
						if (this.items[(int)b2].getItemCount() > 0)
						{
							for (int i = (int)(this.items[(int)b2].getItemCount() - 1); i >= 0; i--)
							{
								if (Random.value < num)
								{
									ItemManager.dropItem(this.items[(int)b2].getItem((byte)i).item, base.transform.position, false, true, true);
									this.items[(int)b2].removeItem((byte)i);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x000E94D4 File Offset: 0x000E76D4
		private void onItemsResized(byte page, byte newWidth, byte newHeight)
		{
			if (!base.channel.IsLocalPlayer && Provider.isServer && this.ownerHasInventory)
			{
				PlayerInventory.SendSize.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), page, newWidth, newHeight);
			}
			InventoryResized inventoryResized = this.onInventoryResized;
			if (inventoryResized != null)
			{
				inventoryResized(page, newWidth, newHeight);
			}
			this.incrementUpdateIndex();
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x000E9536 File Offset: 0x000E7736
		private void onItemUpdated(byte page, byte index, ItemJar jar)
		{
			InventoryUpdated inventoryUpdated = this.onInventoryUpdated;
			if (inventoryUpdated != null)
			{
				inventoryUpdated(page, index, jar);
			}
			this.incrementUpdateIndex();
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x000E9552 File Offset: 0x000E7752
		private void onItemAdded(byte page, byte index, ItemJar jar)
		{
			if (!base.channel.IsLocalPlayer && Provider.isServer && this.ownerHasInventory)
			{
				this.sendItemAdd(page, jar);
			}
			InventoryAdded inventoryAdded = this.onInventoryAdded;
			if (inventoryAdded != null)
			{
				inventoryAdded(page, index, jar);
			}
			this.incrementUpdateIndex();
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x000E9594 File Offset: 0x000E7794
		private void onItemRemoved(byte page, byte index, ItemJar jar)
		{
			if (Provider.isServer)
			{
				if (!base.channel.IsLocalPlayer && this.ownerHasInventory)
				{
					this.sendItemRemove(page, jar);
				}
				if (base.player.equipment.checkSelection(page, jar.x, jar.y))
				{
					base.player.equipment.dequip();
				}
			}
			InventoryRemoved inventoryRemoved = this.onInventoryRemoved;
			if (inventoryRemoved != null)
			{
				inventoryRemoved(page, index, jar);
			}
			this.incrementUpdateIndex();
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x000E9610 File Offset: 0x000E7810
		private void onItemDiscarded(byte page, byte index, ItemJar jar)
		{
			bool flag = true;
			if (base.player.life.isDead)
			{
				ItemAsset asset = jar.GetAsset();
				if (asset == null || !asset.shouldDropOnDeath)
				{
					flag = false;
				}
			}
			if (Provider.isServer)
			{
				if (!base.channel.IsLocalPlayer && this.ownerHasInventory)
				{
					this.sendItemRemove(page, jar);
				}
				if (base.player.equipment.checkSelection(page, jar.x, jar.y))
				{
					base.player.equipment.dequip();
				}
				InventoryRemoved inventoryRemoved = this.onInventoryRemoved;
				if (inventoryRemoved != null)
				{
					inventoryRemoved(page, index, jar);
				}
				if (flag)
				{
					ItemManager.dropItem(jar.item, base.transform.position, false, true, true);
				}
			}
			this.incrementUpdateIndex();
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x000E96CE File Offset: 0x000E78CE
		private void onItemStateUpdated()
		{
			InventoryStateUpdated inventoryStateUpdated = this.onInventoryStateUpdated;
			if (inventoryStateUpdated != null)
			{
				inventoryStateUpdated();
			}
			this.incrementUpdateIndex();
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x000E96E7 File Offset: 0x000E78E7
		private void OnDestroy()
		{
			this.closeStorage();
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x000E96F0 File Offset: 0x000E78F0
		internal void InitializePlayer()
		{
			this.items = new Items[(int)PlayerInventory.PAGES];
			for (byte b = 0; b < PlayerInventory.PAGES - 1; b += 1)
			{
				this.items[(int)b] = new Items(b);
				Items items = this.items[(int)b];
				items.onItemsResized = (ItemsResized)Delegate.Combine(items.onItemsResized, new ItemsResized(this.onItemsResized));
				Items items2 = this.items[(int)b];
				items2.onItemUpdated = (ItemUpdated)Delegate.Combine(items2.onItemUpdated, new ItemUpdated(this.onItemUpdated));
				Items items3 = this.items[(int)b];
				items3.onItemAdded = (ItemAdded)Delegate.Combine(items3.onItemAdded, new ItemAdded(this.onItemAdded));
				Items items4 = this.items[(int)b];
				items4.onItemRemoved = (ItemRemoved)Delegate.Combine(items4.onItemRemoved, new ItemRemoved(this.onItemRemoved));
				Items items5 = this.items[(int)b];
				items5.onStateUpdated = (StateUpdated)Delegate.Combine(items5.onStateUpdated, new StateUpdated(this.onItemStateUpdated));
			}
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				PlayerLife life = base.player.life;
				life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
			}
			if (Provider.isServer)
			{
				PlayerClothing clothing = base.player.clothing;
				clothing.onShirtUpdated = (ShirtUpdated)Delegate.Combine(clothing.onShirtUpdated, new ShirtUpdated(this.onShirtUpdated));
				PlayerClothing clothing2 = base.player.clothing;
				clothing2.onPantsUpdated = (PantsUpdated)Delegate.Combine(clothing2.onPantsUpdated, new PantsUpdated(this.onPantsUpdated));
				PlayerClothing clothing3 = base.player.clothing;
				clothing3.onBackpackUpdated = (BackpackUpdated)Delegate.Combine(clothing3.onBackpackUpdated, new BackpackUpdated(this.onBackpackUpdated));
				PlayerClothing clothing4 = base.player.clothing;
				clothing4.onVestUpdated = (VestUpdated)Delegate.Combine(clothing4.onVestUpdated, new VestUpdated(this.onVestUpdated));
				for (byte b2 = 0; b2 < PlayerInventory.PAGES - 1; b2 += 1)
				{
					this.items[(int)b2].onItemDiscarded = new ItemDiscarded(this.onItemDiscarded);
				}
				this.load();
			}
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x000E9930 File Offset: 0x000E7B30
		public void load()
		{
			this.wasLoadCalled = true;
			if (!PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Inventory.dat") || Level.info.type != ELevelType.SURVIVAL)
			{
				this.items[0].loadSize(1, 1);
				this.items[1].loadSize(1, 1);
				this.items[2].loadSize(5, 3);
				this.items[(int)PlayerInventory.BACKPACK].loadSize(0, 0);
				this.items[(int)PlayerInventory.VEST].loadSize(0, 0);
				this.items[(int)PlayerInventory.SHIRT].loadSize(0, 0);
				this.items[(int)PlayerInventory.PANTS].loadSize(0, 0);
				this.items[(int)PlayerInventory.STORAGE].loadSize(0, 0);
				this.bestowLoadout();
				return;
			}
			Block block = PlayerSavedata.readBlock(base.channel.owner.playerID, "/Player/Inventory.dat", 0);
			byte b = block.readByte();
			if (b > 3)
			{
				for (byte b2 = 0; b2 < PlayerInventory.PAGES - 2; b2 += 1)
				{
					this.items[(int)b2].loadSize(block.readByte(), block.readByte());
					byte b3 = block.readByte();
					for (byte b4 = 0; b4 < b3; b4 += 1)
					{
						byte x = block.readByte();
						byte y = block.readByte();
						byte rot = 0;
						if (b > 4)
						{
							rot = block.readByte();
						}
						ushort num = block.readUInt16();
						byte newAmount = block.readByte();
						byte newQuality = block.readByte();
						byte[] newState = block.readByteArray();
						if (Assets.find(EAssetType.ITEM, num) is ItemAsset)
						{
							this.items[(int)b2].loadItem(x, y, rot, new Item(num, newAmount, newQuality, newState));
						}
					}
				}
				return;
			}
			this.items[0].loadSize(1, 1);
			this.items[1].loadSize(1, 1);
			this.items[2].loadSize(5, 3);
			this.items[(int)PlayerInventory.BACKPACK].loadSize(0, 0);
			this.items[(int)PlayerInventory.VEST].loadSize(0, 0);
			this.items[(int)PlayerInventory.SHIRT].loadSize(0, 0);
			this.items[(int)PlayerInventory.PANTS].loadSize(0, 0);
			this.items[(int)PlayerInventory.STORAGE].loadSize(0, 0);
			this.bestowLoadout();
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x000E9B7C File Offset: 0x000E7D7C
		public void save()
		{
			if (!this.wasLoadCalled)
			{
				return;
			}
			bool flag = base.player.life.wasPvPDeath ? Provider.modeConfigData.Players.Lose_Weapons_PvP : Provider.modeConfigData.Players.Lose_Weapons_PvE;
			bool flag2 = base.player.life.wasPvPDeath ? Provider.modeConfigData.Players.Lose_Clothes_PvP : Provider.modeConfigData.Players.Lose_Clothes_PvE;
			if (base.player.life.isDead && flag && flag2)
			{
				if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Inventory.dat"))
				{
					PlayerSavedata.deleteFile(base.channel.owner.playerID, "/Player/Inventory.dat");
					return;
				}
			}
			else
			{
				Block block = new Block();
				block.writeByte(PlayerInventory.SAVEDATA_VERSION);
				for (byte b = 0; b < PlayerInventory.PAGES - 2; b += 1)
				{
					bool flag3;
					if (base.player.life.isDead)
					{
						if (b < PlayerInventory.SLOTS)
						{
							flag3 = flag;
						}
						else
						{
							flag3 = flag2;
						}
					}
					else
					{
						flag3 = false;
					}
					byte value;
					byte value2;
					byte b2;
					if (this.items[(int)b] == null || flag3)
					{
						value = 0;
						value2 = 0;
						b2 = 0;
					}
					else
					{
						value = this.items[(int)b].width;
						value2 = this.items[(int)b].height;
						b2 = this.items[(int)b].getItemCount();
					}
					block.writeByte(value);
					block.writeByte(value2);
					block.writeByte(b2);
					for (byte b3 = 0; b3 < b2; b3 += 1)
					{
						ItemJar item = this.items[(int)b].getItem(b3);
						block.writeByte((item != null) ? item.x : 0);
						block.writeByte((item != null) ? item.y : 0);
						block.writeByte((item != null) ? item.rot : 0);
						block.writeUInt16((item != null) ? item.item.id : 0);
						block.writeByte((item != null) ? item.item.amount : 0);
						block.writeByte((item != null) ? item.item.quality : 0);
						block.writeByteArray((item != null) ? item.item.state : new byte[0]);
					}
				}
				PlayerSavedata.writeBlock(base.channel.owner.playerID, "/Player/Inventory.dat", block);
			}
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x000E9DE2 File Offset: 0x000E7FE2
		[Conditional("LOG_INVENTORY_RPC_FAILURES")]
		private void LogRPCFailure(string format, params object[] args)
		{
			UnturnedLog.warn(format, args);
		}

		// Token: 0x04001D61 RID: 7521
		public static readonly ushort[] LOADOUT = new ushort[0];

		// Token: 0x04001D62 RID: 7522
		public static readonly ushort[] HORDE = new ushort[]
		{
			97,
			98,
			98,
			98
		};

		// Token: 0x04001D63 RID: 7523
		public static readonly ushort[][] SKILLSETS_SERVER = new ushort[][]
		{
			new ushort[0],
			new ushort[0],
			new ushort[0],
			new ushort[0],
			new ushort[0],
			new ushort[0],
			new ushort[0],
			new ushort[0],
			new ushort[0],
			new ushort[0],
			new ushort[0]
		};

		// Token: 0x04001D64 RID: 7524
		public static readonly ushort[][] SKILLSETS_CLIENT = new ushort[][]
		{
			new ushort[]
			{
				180,
				214
			},
			new ushort[]
			{
				233,
				234,
				241
			},
			new ushort[]
			{
				223,
				224,
				225
			},
			new ushort[]
			{
				1171,
				1172
			},
			new ushort[]
			{
				242,
				243,
				244
			},
			new ushort[]
			{
				510,
				511,
				509
			},
			new ushort[]
			{
				211,
				213
			},
			new ushort[]
			{
				232,
				2,
				240
			},
			new ushort[]
			{
				230,
				231,
				239
			},
			new ushort[]
			{
				1156,
				1157
			},
			new ushort[]
			{
				311,
				312
			}
		};

		// Token: 0x04001D65 RID: 7525
		public static readonly ushort[][] SKILLSETS_HERO = new ushort[][]
		{
			new ushort[]
			{
				180,
				214
			},
			new ushort[]
			{
				233,
				234,
				241,
				104
			},
			new ushort[]
			{
				223,
				224,
				225,
				10,
				112,
				99
			},
			new ushort[]
			{
				1171,
				1172,
				1169,
				334,
				297,
				1027
			},
			new ushort[]
			{
				242,
				243,
				244,
				101,
				1034
			},
			new ushort[]
			{
				510,
				511,
				509
			},
			new ushort[]
			{
				211,
				213,
				16
			},
			new ushort[]
			{
				232,
				2,
				240,
				138
			},
			new ushort[]
			{
				230,
				231,
				239,
				137
			},
			new ushort[]
			{
				1156,
				1157,
				434,
				122,
				1036
			},
			new ushort[]
			{
				311,
				312
			}
		};

		// Token: 0x04001D66 RID: 7526
		public static readonly byte SAVEDATA_VERSION = 5;

		// Token: 0x04001D67 RID: 7527
		public static readonly byte SLOTS = 2;

		// Token: 0x04001D68 RID: 7528
		public static readonly byte PAGES = 9;

		// Token: 0x04001D69 RID: 7529
		public static readonly byte BACKPACK = 3;

		// Token: 0x04001D6A RID: 7530
		public static readonly byte VEST = 4;

		// Token: 0x04001D6B RID: 7531
		public static readonly byte SHIRT = 5;

		// Token: 0x04001D6C RID: 7532
		public static readonly byte PANTS = 6;

		// Token: 0x04001D6D RID: 7533
		public static readonly byte STORAGE = 7;

		// Token: 0x04001D6E RID: 7534
		public static readonly byte AREA = 8;

		// Token: 0x04001D6F RID: 7535
		public static ushort[] loadout = PlayerInventory.LOADOUT;

		// Token: 0x04001D70 RID: 7536
		public static ushort[][] skillsets = PlayerInventory.SKILLSETS_SERVER;

		/// <summary>
		/// Every time the inventory changes this number is incremented.
		/// While a little messy, the idea is to prevent inventory checks from happening every frame.
		/// </summary>
		// Token: 0x04001D72 RID: 7538
		protected int receivedUpdateIndex;

		// Token: 0x04001D73 RID: 7539
		public bool isStoring;

		// Token: 0x04001D74 RID: 7540
		public bool isStorageTrunk;

		// Token: 0x04001D75 RID: 7541
		public InteractableStorage storage;

		/// <summary>
		/// Did owner call askInventory yet?
		/// Prevents duplicate tell_X RPCs from being sent to owner prior to initial sync.
		/// Ideally should be cleaned up with netcode refactor. (Client should not need to ask server for initial state.)
		/// </summary>
		// Token: 0x04001D76 RID: 7542
		private bool ownerHasInventory;

		// Token: 0x04001D77 RID: 7543
		public InventoryResized onInventoryResized;

		// Token: 0x04001D78 RID: 7544
		public InventoryUpdated onInventoryUpdated;

		// Token: 0x04001D79 RID: 7545
		public InventoryAdded onInventoryAdded;

		// Token: 0x04001D7A RID: 7546
		public InventoryRemoved onInventoryRemoved;

		// Token: 0x04001D7B RID: 7547
		public InventoryStored onInventoryStored;

		// Token: 0x04001D7C RID: 7548
		public InventoryStateUpdated onInventoryStateUpdated;

		// Token: 0x04001D7D RID: 7549
		public DropItemRequestHandler onDropItemRequested;

		// Token: 0x04001D7E RID: 7550
		private static readonly ServerInstanceMethod<byte, byte, byte, byte, byte, byte, byte> SendDragItem = ServerInstanceMethod<byte, byte, byte, byte, byte, byte, byte>.Get(typeof(PlayerInventory), "ReceiveDragItem");

		// Token: 0x04001D7F RID: 7551
		private static readonly ServerInstanceMethod<byte, byte, byte, byte, byte, byte, byte, byte> SendSwapItem = ServerInstanceMethod<byte, byte, byte, byte, byte, byte, byte, byte>.Get(typeof(PlayerInventory), "ReceiveSwapItem");

		// Token: 0x04001D80 RID: 7552
		private static readonly ServerInstanceMethod<byte, byte, byte> SendDropItem = ServerInstanceMethod<byte, byte, byte>.Get(typeof(PlayerInventory), "ReceiveDropItem");

		// Token: 0x04001D81 RID: 7553
		private static readonly ClientInstanceMethod<byte, byte, byte> SendUpdateAmount = ClientInstanceMethod<byte, byte, byte>.Get(typeof(PlayerInventory), "ReceiveUpdateAmount");

		// Token: 0x04001D82 RID: 7554
		private static readonly ClientInstanceMethod<byte, byte, byte> SendUpdateQuality = ClientInstanceMethod<byte, byte, byte>.Get(typeof(PlayerInventory), "ReceiveUpdateQuality");

		// Token: 0x04001D83 RID: 7555
		private static readonly ClientInstanceMethod<byte, byte, byte[]> SendUpdateInvState = ClientInstanceMethod<byte, byte, byte[]>.Get(typeof(PlayerInventory), "ReceiveUpdateInvState");

		// Token: 0x04001D84 RID: 7556
		private static readonly ClientInstanceMethod<byte, byte, byte, byte, ushort, byte, byte, byte[]> SendItemAdd = ClientInstanceMethod<byte, byte, byte, byte, ushort, byte, byte, byte[]>.Get(typeof(PlayerInventory), "ReceiveItemAdd");

		// Token: 0x04001D85 RID: 7557
		private static readonly ClientInstanceMethod<byte, byte, byte> SendItemRemove = ClientInstanceMethod<byte, byte, byte>.Get(typeof(PlayerInventory), "ReceiveItemRemove");

		// Token: 0x04001D86 RID: 7558
		private static readonly ClientInstanceMethod<byte, byte, byte> SendSize = ClientInstanceMethod<byte, byte, byte>.Get(typeof(PlayerInventory), "ReceiveSize");

		// Token: 0x04001D87 RID: 7559
		private static readonly ClientInstanceMethod SendStoraging = ClientInstanceMethod.Get(typeof(PlayerInventory), "ReceiveStoraging");

		// Token: 0x04001D88 RID: 7560
		private static readonly ClientInstanceMethod SendInventory = ClientInstanceMethod.Get(typeof(PlayerInventory), "ReceiveInventory");

		// Token: 0x04001D89 RID: 7561
		private bool wasLoadCalled;
	}
}
