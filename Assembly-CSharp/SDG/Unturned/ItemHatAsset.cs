using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002EB RID: 747
	public class ItemHatAsset : ItemGearAsset
	{
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001665 RID: 5733 RVA: 0x000532F6 File Offset: 0x000514F6
		public GameObject hat
		{
			get
			{
				return this._hat;
			}
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x000532FE File Offset: 0x000514FE
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
		}

		// Token: 0x040009B7 RID: 2487
		protected GameObject _hat;
	}
}
