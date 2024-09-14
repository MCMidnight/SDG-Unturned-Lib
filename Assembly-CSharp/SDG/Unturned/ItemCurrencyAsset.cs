using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Associates items of the same currency, e.g. dollars or bullets.
	/// </summary>
	// Token: 0x020002DC RID: 732
	public class ItemCurrencyAsset : Asset
	{
		/// <summary>
		/// String to format value {0} into.
		/// </summary>
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x00050E44 File Offset: 0x0004F044
		// (set) Token: 0x060015D1 RID: 5585 RVA: 0x00050E4C File Offset: 0x0004F04C
		public string valueFormat { get; protected set; }

		/// <summary>
		/// String to format value {0} of total {1} into if not otherwise specified in NPC condition.
		/// </summary>
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x00050E55 File Offset: 0x0004F055
		// (set) Token: 0x060015D3 RID: 5587 RVA: 0x00050E5D File Offset: 0x0004F05D
		public string defaultConditionFormat { get; protected set; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060015D4 RID: 5588 RVA: 0x00050E66 File Offset: 0x0004F066
		// (set) Token: 0x060015D5 RID: 5589 RVA: 0x00050E6E File Offset: 0x0004F06E
		public ItemCurrencyAsset.Entry[] entries { get; protected set; }

		/// <summary>
		/// Sum up value of each currency item in player's inventory.
		/// </summary>
		// Token: 0x060015D6 RID: 5590 RVA: 0x00050E78 File Offset: 0x0004F078
		public uint getInventoryValue(Player player)
		{
			uint num = 0U;
			foreach (ItemCurrencyAsset.Entry entry in this.entries)
			{
				AssetReference<ItemAsset> item = entry.item;
				ItemAsset itemAsset = item.Find();
				if (itemAsset != null)
				{
					ItemCurrencyAsset.search.Clear();
					player.inventory.search(ItemCurrencyAsset.search, itemAsset.id, false, true);
					foreach (InventorySearch inventorySearch in ItemCurrencyAsset.search)
					{
						num += (uint)inventorySearch.jar.item.amount * entry.value;
					}
				}
			}
			return num;
		}

		/// <summary>
		/// Does player have access to items covering certain value?
		/// </summary>
		// Token: 0x060015D7 RID: 5591 RVA: 0x00050F40 File Offset: 0x0004F140
		public bool canAfford(Player player, uint value)
		{
			return this.getInventoryValue(player) >= value;
		}

		/// <summary>
		/// Add items to player's inventory to reward value.
		/// </summary>
		// Token: 0x060015D8 RID: 5592 RVA: 0x00050F50 File Offset: 0x0004F150
		public void grantValue(Player player, uint requiredValue)
		{
			if (requiredValue < 1U)
			{
				return;
			}
			for (int i = this.entries.Length - 1; i >= 0; i--)
			{
				ItemCurrencyAsset.Entry entry = this.entries[i];
				ItemAsset itemAsset = entry.item.Find();
				if (itemAsset != null && requiredValue >= entry.value)
				{
					uint num = requiredValue / entry.value;
					ItemTool.tryForceGiveItem(player, itemAsset.id, (byte)num);
					requiredValue -= num * entry.value;
					if (requiredValue == 0U)
					{
						return;
					}
				}
			}
		}

		/// <summary>
		/// Remove items from player's inventory to pay required value.
		/// </summary>
		// Token: 0x060015D9 RID: 5593 RVA: 0x00050FC8 File Offset: 0x0004F1C8
		public bool spendValue(Player player, uint requiredValue)
		{
			if (!this.canAfford(player, requiredValue))
			{
				return false;
			}
			uint num = 0U;
			foreach (ItemCurrencyAsset.Entry entry in this.entries)
			{
				AssetReference<ItemAsset> item = entry.item;
				ItemAsset itemAsset = item.Find();
				if (itemAsset != null)
				{
					uint num2 = (requiredValue - num - 1U) / entry.value + 1U;
					List<InventorySearch> list = new List<InventorySearch>();
					player.inventory.search(list, itemAsset.id, false, true);
					foreach (InventorySearch inventorySearch in list)
					{
						uint num3 = inventorySearch.deleteAmount(player, num2);
						num2 -= num3;
						num += num3 * entry.value;
						if (num2 == 0U)
						{
							break;
						}
					}
					if (num >= requiredValue)
					{
						break;
					}
				}
			}
			if (num > requiredValue)
			{
				uint requiredValue2 = num - requiredValue;
				this.grantValue(player, requiredValue2);
			}
			return true;
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x000510C0 File Offset: 0x0004F2C0
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.valueFormat = data.GetString("ValueFormat", null);
			this.defaultConditionFormat = data.GetString("DefaultConditionFormat", null);
			if (string.IsNullOrEmpty(this.defaultConditionFormat) && !string.IsNullOrEmpty(this.valueFormat))
			{
				this.defaultConditionFormat = this.valueFormat + " / " + this.valueFormat.Replace("{0", "{1");
			}
			DatList datList;
			if (data.TryGetList("Entries", out datList))
			{
				int count = datList.Count;
				this.entries = new ItemCurrencyAsset.Entry[count];
				for (int i = 0; i < count; i++)
				{
					ItemCurrencyAsset.Entry entry = default(ItemCurrencyAsset.Entry);
					DatDictionary datDictionary = datList[i] as DatDictionary;
					if (datDictionary != null)
					{
						entry.item = datDictionary.ParseStruct<AssetReference<ItemAsset>>("Item", default(AssetReference<ItemAsset>));
						entry.value = datDictionary.ParseUInt32("Value", 0U);
						if (datDictionary.ContainsKey("Is_Visible_In_Vendor_Menu"))
						{
							entry.isVisibleInVendorMenu = datDictionary.ParseBool("Is_Visible_In_Vendor_Menu", false);
						}
						else
						{
							entry.isVisibleInVendorMenu = true;
						}
					}
					this.entries[i] = entry;
				}
			}
			else
			{
				this.entries = new ItemCurrencyAsset.Entry[0];
			}
			Array.Sort<ItemCurrencyAsset.Entry>(this.entries, ItemCurrencyAsset.valueComparer);
		}

		// Token: 0x04000926 RID: 2342
		private static List<InventorySearch> search = new List<InventorySearch>();

		// Token: 0x04000927 RID: 2343
		private static ItemCurrencyComparer valueComparer = new ItemCurrencyComparer();

		// Token: 0x0200091F RID: 2335
		public struct Entry
		{
			// Token: 0x0400325D RID: 12893
			public AssetReference<ItemAsset> item;

			// Token: 0x0400325E RID: 12894
			public uint value;

			/// <summary>
			/// Should this item/value be shown in the list of vendor currency items?
			/// Useful to hide modded item stacks e.g. a stack of 100x $20 bills.
			/// </summary>
			// Token: 0x0400325F RID: 12895
			public bool isVisibleInVendorMenu;
		}
	}
}
