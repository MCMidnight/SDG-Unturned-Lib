using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Adds custom data to base physics material asset.
	/// For example how a vanilla material should respond to custom laser guns.
	/// </summary>
	// Token: 0x0200035A RID: 858
	public class PhysicsMaterialExtensionAsset : PhysicsMaterialAssetBase
	{
		// Token: 0x060019F2 RID: 6642 RVA: 0x0005D6FC File Offset: 0x0005B8FC
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.baseRef = data.ParseStruct<AssetReference<PhysicsMaterialAsset>>("Base", default(AssetReference<PhysicsMaterialAsset>));
			PhysicMaterialCustomData.RegisterAsset(this);
		}

		// Token: 0x04000BE0 RID: 3040
		public AssetReference<PhysicsMaterialAsset> baseRef;
	}
}
