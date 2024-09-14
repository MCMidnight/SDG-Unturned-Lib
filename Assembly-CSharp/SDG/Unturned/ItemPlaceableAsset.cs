using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Common base for barricades and structures.
	/// 2023-01-16: not ideal to be adding this so late in development, but at least it is a step in the right direction.
	/// </summary>
	// Token: 0x020002F9 RID: 761
	public class ItemPlaceableAsset : ItemAsset
	{
		/// <summary>
		/// Item recovered when picked up below 100% health.
		/// </summary>
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x00053EA7 File Offset: 0x000520A7
		// (set) Token: 0x060016CA RID: 5834 RVA: 0x00053EAF File Offset: 0x000520AF
		public AssetReference<ItemAsset> salvageItemRef { get; protected set; }

		/// <summary>
		/// Minimum number of items to drop when destroyed.
		/// </summary>
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060016CB RID: 5835 RVA: 0x00053EB8 File Offset: 0x000520B8
		// (set) Token: 0x060016CC RID: 5836 RVA: 0x00053EC0 File Offset: 0x000520C0
		public int minItemsDroppedOnDestroy { get; protected set; }

		/// <summary>
		/// Maximum number of items to drop when destroyed.
		/// </summary>
		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060016CD RID: 5837 RVA: 0x00053EC9 File Offset: 0x000520C9
		// (set) Token: 0x060016CE RID: 5838 RVA: 0x00053ED1 File Offset: 0x000520D1
		public int maxItemsDroppedOnDestroy { get; protected set; }

		/// <summary>
		/// Spawn table for items dropped when destroyed.
		/// </summary>
		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060016CF RID: 5839 RVA: 0x00053EDA File Offset: 0x000520DA
		// (set) Token: 0x060016D0 RID: 5840 RVA: 0x00053EE2 File Offset: 0x000520E2
		public AssetReference<SpawnAsset> itemDroppedOnDestroy { get; protected set; }

		// Token: 0x060016D1 RID: 5841 RVA: 0x00053EEC File Offset: 0x000520EC
		public ItemAsset FindSalvageItemAsset()
		{
			if (this.salvageItemRef.isValid)
			{
				return this.salvageItemRef.Find();
			}
			return this.FindDefaultSalvageItemAsset();
		}

		/// <summary>
		/// By default a crafting ingredient is salvaged.
		/// </summary>
		// Token: 0x060016D2 RID: 5842 RVA: 0x00053F20 File Offset: 0x00052120
		internal ItemAsset FindDefaultSalvageItemAsset()
		{
			foreach (Blueprint blueprint in base.blueprints)
			{
				if (blueprint.outputs.Length == 1 && blueprint.outputs[0].id == this.id)
				{
					BlueprintSupply blueprintSupply = blueprint.supplies[Random.Range(0, blueprint.supplies.Length)];
					return Assets.find(EAssetType.ITEM, blueprintSupply.id) as ItemAsset;
				}
			}
			return null;
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00053FBC File Offset: 0x000521BC
		internal void SpawnItemDropsOnDestroy(Vector3 position)
		{
			int num = Random.Range(this.minItemsDroppedOnDestroy, this.maxItemsDroppedOnDestroy + 1);
			num = Mathf.Clamp(num, 0, 100);
			if (num < 1)
			{
				return;
			}
			SpawnAsset spawnAsset = this.itemDroppedOnDestroy.Find();
			if (spawnAsset == null)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = SpawnTableTool.ResolveLegacyId(spawnAsset, EAssetType.ITEM, new Func<string>(this.OnGetItemDroppedOnDestroySpawnTableErrorContext));
				if (num2 > 0)
				{
					ItemManager.dropItem(new Item(num2, EItemOrigin.NATURE), position + new Vector3(Random.Range(-2f, 2f), 2f, Random.Range(-2f, 2f)), false, true, true);
				}
			}
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00054064 File Offset: 0x00052264
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.salvageItemRef = data.readAssetReference("SalvageItem");
			this.minItemsDroppedOnDestroy = data.ParseInt32("Min_Items_Dropped_On_Destroy", 0);
			this.maxItemsDroppedOnDestroy = data.ParseInt32("Max_Items_Dropped_On_Destroy", 0);
			this.itemDroppedOnDestroy = data.readAssetReference("Item_Dropped_On_Destroy");
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x000540C0 File Offset: 0x000522C0
		private string OnGetItemDroppedOnDestroySpawnTableErrorContext()
		{
			return this.FriendlyName + " items dropped on destroy";
		}
	}
}
