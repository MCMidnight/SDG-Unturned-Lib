using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002F8 RID: 760
	public class ItemPantsAsset : ItemBagAsset
	{
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x060016C4 RID: 5828 RVA: 0x00053E7C File Offset: 0x0005207C
		public Texture2D pants
		{
			get
			{
				return this._pants;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00053E84 File Offset: 0x00052084
		public Texture2D emission
		{
			get
			{
				return this._emission;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x060016C6 RID: 5830 RVA: 0x00053E8C File Offset: 0x0005208C
		public Texture2D metallic
		{
			get
			{
				return this._metallic;
			}
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00053E94 File Offset: 0x00052094
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
		}

		// Token: 0x040009F5 RID: 2549
		protected Texture2D _pants;

		// Token: 0x040009F6 RID: 2550
		protected Texture2D _emission;

		// Token: 0x040009F7 RID: 2551
		protected Texture2D _metallic;
	}
}
