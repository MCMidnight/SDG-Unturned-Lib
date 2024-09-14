using System;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000EC RID: 236
	public class FoliageInfoCollectionAsset : Asset
	{
		// Token: 0x060005CA RID: 1482 RVA: 0x00015F00 File Offset: 0x00014100
		public virtual void bakeFoliage(FoliageBakeSettings bakeSettings, IFoliageSurface surface, Bounds bounds, float weight)
		{
			foreach (FoliageInfoCollectionAsset.FoliageInfoCollectionElement foliageInfoCollectionElement in this.elements)
			{
				FoliageInfoAsset foliageInfoAsset = Assets.find<FoliageInfoAsset>(foliageInfoCollectionElement.asset);
				if (foliageInfoAsset != null)
				{
					foliageInfoAsset.bakeFoliage(bakeSettings, surface, bounds, weight, foliageInfoCollectionElement.weight);
				}
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00015F6C File Offset: 0x0001416C
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			DatList datList;
			if (data.TryGetList("Foliage", out datList))
			{
				this.elements = datList.ParseListOfStructs<FoliageInfoCollectionAsset.FoliageInfoCollectionElement>();
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00015F9D File Offset: 0x0001419D
		public FoliageInfoCollectionAsset()
		{
			this.elements = new List<FoliageInfoCollectionAsset.FoliageInfoCollectionElement>();
		}

		// Token: 0x0400021E RID: 542
		public List<FoliageInfoCollectionAsset.FoliageInfoCollectionElement> elements;

		// Token: 0x02000864 RID: 2148
		public struct FoliageInfoCollectionElement : IDatParseable
		{
			// Token: 0x06004805 RID: 18437 RVA: 0x001AE3B4 File Offset: 0x001AC5B4
			public bool TryParse(IDatNode node)
			{
				DatDictionary datDictionary = node as DatDictionary;
				if (datDictionary != null)
				{
					this.asset = datDictionary.ParseStruct<AssetReference<FoliageInfoAsset>>("Asset", default(AssetReference<FoliageInfoAsset>));
					this.weight = datDictionary.ParseFloat("Weight", 1f);
					return true;
				}
				return false;
			}

			// Token: 0x04003167 RID: 12647
			public AssetReference<FoliageInfoAsset> asset;

			// Token: 0x04003168 RID: 12648
			public float weight;
		}
	}
}
