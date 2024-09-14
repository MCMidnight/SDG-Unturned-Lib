using System;

namespace SDG.Unturned
{
	// Token: 0x020002DA RID: 730
	public class ItemCloudAsset : ItemAsset
	{
		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x00050775 File Offset: 0x0004E975
		public float gravity
		{
			get
			{
				return this._gravity;
			}
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x0005077D File Offset: 0x0004E97D
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._gravity = data.ParseFloat("Gravity", 0f);
		}

		// Token: 0x04000910 RID: 2320
		private float _gravity;
	}
}
