using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002D2 RID: 722
	public class ItemBackpackAsset : ItemBagAsset
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x0004EECE File Offset: 0x0004D0CE
		public GameObject backpack
		{
			get
			{
				return this._backpack;
			}
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0004EED6 File Offset: 0x0004D0D6
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0004EEE4 File Offset: 0x0004D0E4
		protected override AudioReference GetDefaultInventoryAudio()
		{
			if (base.width <= 3 || base.height <= 3)
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/LightMetalEquipment.asset");
			}
			if (base.width <= 6 || base.height <= 6)
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/MediumMetalEquipment.asset");
			}
			return new AudioReference("core.masterbundle", "Sounds/Inventory/HeavyMetalEquipment.asset");
		}

		// Token: 0x040008B7 RID: 2231
		protected GameObject _backpack;
	}
}
