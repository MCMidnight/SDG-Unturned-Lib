using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200030C RID: 780
	public class ItemVestAsset : ItemBagAsset
	{
		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001784 RID: 6020 RVA: 0x00055E97 File Offset: 0x00054097
		public GameObject vest
		{
			get
			{
				return this._vest;
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00055E9F File Offset: 0x0005409F
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
		}

		// Token: 0x04000A7B RID: 2683
		protected GameObject _vest;
	}
}
