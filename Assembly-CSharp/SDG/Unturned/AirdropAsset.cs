using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000285 RID: 645
	public class AirdropAsset : Asset
	{
		// Token: 0x0600130E RID: 4878 RVA: 0x00045FF4 File Offset: 0x000441F4
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.barricadeRef = data.ParseStruct<AssetReference<ItemBarricadeAsset>>("Landed_Barricade", default(AssetReference<ItemBarricadeAsset>));
			this.model = data.ParseStruct<MasterBundleReference<GameObject>>("Carepackage_Prefab", default(MasterBundleReference<GameObject>));
		}

		// Token: 0x04000658 RID: 1624
		public static AssetReference<AirdropAsset> defaultAirdrop = new AssetReference<AirdropAsset>("229440c249dc490ba26ce71e8a59d5c6");

		/// <summary>
		/// Interactable storage barricade to spawn at the drop position.
		/// </summary>
		// Token: 0x04000659 RID: 1625
		public AssetReference<ItemBarricadeAsset> barricadeRef;

		/// <summary>
		/// Prefab to spawn falling from the aircraft.
		/// </summary>
		// Token: 0x0400065A RID: 1626
		public MasterBundleReference<GameObject> model;
	}
}
