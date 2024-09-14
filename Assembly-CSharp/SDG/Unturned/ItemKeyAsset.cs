using System;

namespace SDG.Unturned
{
	// Token: 0x020002EC RID: 748
	public class ItemKeyAsset : ItemAsset
	{
		// Token: 0x06001668 RID: 5736 RVA: 0x00053311 File Offset: 0x00051511
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.exchangeWithTargetItem = data.ContainsKey("Exchange_With_Target_Item");
		}

		// Token: 0x040009B8 RID: 2488
		public bool exchangeWithTargetItem;
	}
}
