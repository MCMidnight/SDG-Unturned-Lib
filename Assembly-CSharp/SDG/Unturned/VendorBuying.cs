using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Represents an item the vendor is buying from players.
	/// </summary>
	// Token: 0x02000381 RID: 897
	public class VendorBuying : VendorElement
	{
		// Token: 0x06001BD2 RID: 7122 RVA: 0x000639CE File Offset: 0x00061BCE
		public ItemAsset FindItemAsset()
		{
			return Assets.FindItemByGuidOrLegacyId<ItemAsset>(base.TargetAssetGuid, base.id);
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x000639E4 File Offset: 0x00061BE4
		public override string displayName
		{
			get
			{
				ItemAsset itemAsset = this.FindItemAsset();
				if (itemAsset == null)
				{
					return null;
				}
				return itemAsset.itemName;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001BD4 RID: 7124 RVA: 0x00063A04 File Offset: 0x00061C04
		public override string displayDesc
		{
			get
			{
				ItemAsset itemAsset = this.FindItemAsset();
				if (itemAsset == null)
				{
					return null;
				}
				return itemAsset.itemDescription;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x00063A24 File Offset: 0x00061C24
		public override EItemRarity rarity
		{
			get
			{
				ItemAsset itemAsset = this.FindItemAsset();
				if (itemAsset == null)
				{
					return EItemRarity.COMMON;
				}
				return itemAsset.rarity;
			}
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x00063A44 File Offset: 0x00061C44
		public bool canSell(Player player)
		{
			ItemAsset itemAsset = this.FindItemAsset();
			if (itemAsset == null)
			{
				return false;
			}
			VendorBuying.search.Clear();
			player.inventory.search(VendorBuying.search, itemAsset.id, false, true);
			ushort num = 0;
			foreach (InventorySearch inventorySearch in VendorBuying.search)
			{
				num += (ushort)inventorySearch.jar.item.amount;
			}
			return num >= (ushort)itemAsset.amount;
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x00063AE0 File Offset: 0x00061CE0
		public void sell(Player player)
		{
			ItemAsset itemAsset = this.FindItemAsset();
			if (itemAsset == null)
			{
				return;
			}
			VendorBuying.search.Clear();
			player.inventory.search(VendorBuying.search, itemAsset.id, false, true);
			VendorBuying.search.Sort(VendorBuying.qualityAscendingComparator);
			ushort num = (ushort)itemAsset.amount;
			foreach (InventorySearch inventorySearch in VendorBuying.search)
			{
				if (player.equipment.checkSelection(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y))
				{
					player.equipment.dequip();
				}
				if ((ushort)inventorySearch.jar.item.amount > num)
				{
					player.inventory.sendUpdateAmount(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y, (byte)((ushort)inventorySearch.jar.item.amount - num));
					break;
				}
				num -= (ushort)inventorySearch.jar.item.amount;
				player.inventory.sendUpdateAmount(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y, 0);
				player.crafting.removeItem(inventorySearch.page, inventorySearch.jar);
				if (inventorySearch.page < PlayerInventory.SLOTS)
				{
					player.equipment.sendSlot(inventorySearch.page);
				}
				if (num == 0)
				{
					break;
				}
			}
			if (base.outerAsset.currency.isValid)
			{
				ItemCurrencyAsset itemCurrencyAsset = base.outerAsset.currency.Find();
				if (itemCurrencyAsset != null)
				{
					itemCurrencyAsset.grantValue(player, base.cost);
					return;
				}
			}
			else
			{
				player.skills.askAward(base.cost);
			}
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x00063CD0 File Offset: 0x00061ED0
		public void format(Player player, out ushort total, out byte amount)
		{
			ItemAsset itemAsset = this.FindItemAsset();
			if (itemAsset == null)
			{
				total = 0;
				amount = 0;
				return;
			}
			VendorBuying.search.Clear();
			player.inventory.search(VendorBuying.search, itemAsset.id, false, true);
			total = 0;
			byte b = 0;
			while ((int)b < VendorBuying.search.Count)
			{
				total += (ushort)VendorBuying.search[(int)b].jar.item.amount;
				b += 1;
			}
			amount = itemAsset.amount;
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x00063D51 File Offset: 0x00061F51
		public VendorBuying(VendorAsset newOuterAsset, byte newIndex, Guid newTargetAssetGuid, ushort newTargetAssetLegacyId, uint newCost, INPCCondition[] newConditions, NPCRewardsList newRewardsList) : base(newOuterAsset, newIndex, newTargetAssetGuid, newTargetAssetLegacyId, newCost, newConditions, newRewardsList)
		{
		}

		// Token: 0x04000D1C RID: 3356
		private static InventorySearchQualityAscendingComparator qualityAscendingComparator = new InventorySearchQualityAscendingComparator();

		// Token: 0x04000D1D RID: 3357
		private static List<InventorySearch> search = new List<InventorySearch>();
	}
}
