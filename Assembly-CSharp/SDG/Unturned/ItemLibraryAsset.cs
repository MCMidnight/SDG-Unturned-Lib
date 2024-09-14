using System;

namespace SDG.Unturned
{
	// Token: 0x020002ED RID: 749
	public class ItemLibraryAsset : ItemBarricadeAsset
	{
		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x00053335 File Offset: 0x00051535
		public uint capacity
		{
			get
			{
				return this._capacity;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x0600166B RID: 5739 RVA: 0x0005333D File Offset: 0x0005153D
		public byte tax
		{
			get
			{
				return this._tax;
			}
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x00053345 File Offset: 0x00051545
		public override byte[] getState(EItemOrigin origin)
		{
			return new byte[20];
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x0005334E File Offset: 0x0005154E
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._capacity = data.ParseUInt32("Capacity", 0U);
			this._tax = data.ParseUInt8("Tax", 0);
		}

		// Token: 0x040009B9 RID: 2489
		protected uint _capacity;

		// Token: 0x040009BA RID: 2490
		protected byte _tax;
	}
}
