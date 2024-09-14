using System;

namespace SDG.Unturned
{
	// Token: 0x02000354 RID: 852
	public class OutfitAsset : Asset
	{
		// Token: 0x060019DE RID: 6622 RVA: 0x0005CCC0 File Offset: 0x0005AEC0
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			DatList datList;
			if (data.TryGetList("Items", out datList))
			{
				this.itemAssets = datList.ParseArrayOfStructs<AssetReference<ItemAsset>>(default(AssetReference<ItemAsset>));
				return;
			}
			this.itemAssets = new AssetReference<ItemAsset>[0];
		}

		// Token: 0x04000BC8 RID: 3016
		public AssetReference<ItemAsset>[] itemAssets;
	}
}
