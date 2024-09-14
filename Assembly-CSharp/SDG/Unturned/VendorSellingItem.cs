using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Represents an item the vendor is selling to players.
	/// </summary>
	// Token: 0x02000385 RID: 901
	public class VendorSellingItem : VendorSellingBase
	{
		// Token: 0x06001BF8 RID: 7160 RVA: 0x00064001 File Offset: 0x00062201
		public ItemAsset FindItemAsset()
		{
			return Assets.FindItemByGuidOrLegacyId<ItemAsset>(base.TargetAssetGuid, base.id);
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001BF9 RID: 7161 RVA: 0x00064014 File Offset: 0x00062214
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

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001BFA RID: 7162 RVA: 0x00064034 File Offset: 0x00062234
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

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x00064054 File Offset: 0x00062254
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

		// Token: 0x06001BFC RID: 7164 RVA: 0x00064074 File Offset: 0x00062274
		public override void buy(Player player)
		{
			base.buy(player);
			ItemAsset itemAsset = this.FindItemAsset();
			if (itemAsset == null)
			{
				return;
			}
			player.inventory.forceAddItem(new Item(itemAsset.id, EItemOrigin.ADMIN), false, false);
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000640AC File Offset: 0x000622AC
		public override void format(Player player, out ushort total)
		{
			total = 0;
			ItemAsset itemAsset = this.FindItemAsset();
			if (itemAsset == null)
			{
				return;
			}
			VendorSellingItem.search.Clear();
			player.inventory.search(VendorSellingItem.search, itemAsset.id, false, true);
			foreach (InventorySearch inventorySearch in VendorSellingItem.search)
			{
				total += (ushort)inventorySearch.jar.item.amount;
			}
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x00064140 File Offset: 0x00062340
		public VendorSellingItem(VendorAsset newOuterAsset, byte newIndex, Guid newTargetAssetGuid, ushort newTargetAssetLegacyId, uint newCost, INPCCondition[] newConditions, NPCRewardsList newRewardsList) : base(newOuterAsset, newIndex, newTargetAssetGuid, newTargetAssetLegacyId, newCost, newConditions, newRewardsList)
		{
		}

		// Token: 0x04000D25 RID: 3365
		private static List<InventorySearch> search = new List<InventorySearch>();
	}
}
