using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Restricts which items can be crafted.
	/// </summary>
	// Token: 0x020002A1 RID: 673
	public class CraftingBlacklistAsset : Asset
	{
		// Token: 0x06001440 RID: 5184 RVA: 0x0004AFB8 File Offset: 0x000491B8
		public bool isBlueprintBlacklisted(Blueprint blueprint)
		{
			if (!this.allowCoreBlueprints && blueprint.sourceItem.origin == Assets.coreOrigin)
			{
				return true;
			}
			foreach (CraftingBlacklistAsset.BlacklistedBlueprint blacklistedBlueprint in this.blueprints)
			{
				if (blacklistedBlueprint.index == (int)blueprint.id)
				{
					AssetReference<ItemAsset> assetRef = blacklistedBlueprint.assetRef;
					if (assetRef.isReferenceTo(blueprint.sourceItem))
					{
						return true;
					}
				}
			}
			if (this.resolvedInputItems == null && this.inputItems != null)
			{
				this.resolvedInputItems = new List<ushort>(this.inputItems.Count);
				foreach (AssetReference<ItemAsset> assetReference in this.inputItems)
				{
					ItemAsset itemAsset = assetReference.Find();
					if (itemAsset != null)
					{
						this.resolvedInputItems.Add(itemAsset.id);
					}
				}
			}
			if (this.resolvedInputItems != null)
			{
				foreach (BlueprintSupply blueprintSupply in blueprint.supplies)
				{
					if (this.resolvedInputItems.Contains(blueprintSupply.id))
					{
						return true;
					}
				}
			}
			if (this.resolvedOutputItems == null && this.outputItems != null)
			{
				this.resolvedOutputItems = new List<ushort>(this.outputItems.Count);
				foreach (AssetReference<ItemAsset> assetReference2 in this.outputItems)
				{
					ItemAsset itemAsset2 = assetReference2.Find();
					if (itemAsset2 != null)
					{
						this.resolvedOutputItems.Add(itemAsset2.id);
					}
				}
			}
			if (this.resolvedOutputItems != null)
			{
				foreach (BlueprintOutput blueprintOutput in blueprint.outputs)
				{
					if (this.resolvedOutputItems.Contains(blueprintOutput.id))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0004B1D4 File Offset: 0x000493D4
		protected void readList(DatDictionary reader, List<AssetReference<ItemAsset>> list, string key)
		{
			DatList datList;
			if (reader.TryGetList(key, out datList))
			{
				using (List<IDatNode>.Enumerator enumerator = datList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AssetReference<ItemAsset> assetReference;
						if (enumerator.Current.TryParseStruct(out assetReference) && assetReference.isValid)
						{
							list.Add(assetReference);
						}
					}
				}
			}
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x0004B240 File Offset: 0x00049440
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.readList(data, this.inputItems, "Input_Items");
			this.readList(data, this.outputItems, "Output_Items");
			DatList datList;
			if (data.TryGetList("Blueprints", out datList))
			{
				this.blueprints = datList.ParseListOfStructs<CraftingBlacklistAsset.BlacklistedBlueprint>();
			}
			this.allowCoreBlueprints = data.ParseBool("Allow_Core_Blueprints", true);
		}

		/// <summary>
		/// Restrict blueprints that consume these items.
		/// </summary>
		// Token: 0x040006F3 RID: 1779
		protected List<AssetReference<ItemAsset>> inputItems = new List<AssetReference<ItemAsset>>();

		/// <summary>
		/// Restrict blueprints that generate these items.
		/// </summary>
		// Token: 0x040006F4 RID: 1780
		protected List<AssetReference<ItemAsset>> outputItems = new List<AssetReference<ItemAsset>>();

		// Token: 0x040006F5 RID: 1781
		protected List<ushort> resolvedInputItems;

		// Token: 0x040006F6 RID: 1782
		protected List<ushort> resolvedOutputItems;

		/// <summary>
		/// If false, blueprints on vanilla/core/built-in items are not allowed. Defaults to true.
		/// </summary>
		// Token: 0x040006F7 RID: 1783
		protected bool allowCoreBlueprints = true;

		/// <summary>
		/// Restrict specific blueprints.
		/// </summary>
		// Token: 0x040006F8 RID: 1784
		protected List<CraftingBlacklistAsset.BlacklistedBlueprint> blueprints = new List<CraftingBlacklistAsset.BlacklistedBlueprint>();

		// Token: 0x02000919 RID: 2329
		protected struct BlacklistedBlueprint : IDatParseable
		{
			// Token: 0x06004A6F RID: 19055 RVA: 0x001B16E8 File Offset: 0x001AF8E8
			public bool TryParse(IDatNode node)
			{
				DatDictionary datDictionary = node as DatDictionary;
				if (datDictionary == null)
				{
					return false;
				}
				this.assetRef = datDictionary.ParseStruct<AssetReference<ItemAsset>>("Item", default(AssetReference<ItemAsset>));
				this.index = datDictionary.ParseInt32("Blueprint", 0);
				return this.assetRef.isValid && this.index >= 0;
			}

			// Token: 0x06004A70 RID: 19056 RVA: 0x001B1748 File Offset: 0x001AF948
			public BlacklistedBlueprint(AssetReference<ItemAsset> assetRef, int index)
			{
				this.assetRef = assetRef;
				this.index = index;
			}

			// Token: 0x04003251 RID: 12881
			public AssetReference<ItemAsset> assetRef;

			// Token: 0x04003252 RID: 12882
			public int index;
		}
	}
}
