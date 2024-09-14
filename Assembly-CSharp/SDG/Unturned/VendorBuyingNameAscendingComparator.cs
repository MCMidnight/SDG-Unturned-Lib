using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000380 RID: 896
	public class VendorBuyingNameAscendingComparator : IComparer<VendorBuying>
	{
		// Token: 0x06001BD0 RID: 7120 RVA: 0x0006397C File Offset: 0x00061B7C
		public int Compare(VendorBuying a, VendorBuying b)
		{
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, a.id) as ItemAsset;
			ItemAsset itemAsset2 = Assets.find(EAssetType.ITEM, b.id) as ItemAsset;
			if (itemAsset == null || itemAsset2 == null)
			{
				return 0;
			}
			return itemAsset.itemName.CompareTo(itemAsset2.itemName);
		}
	}
}
