using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020005F7 RID: 1527
	public class InventorySearchQualityAscendingComparator : IComparer<InventorySearch>
	{
		// Token: 0x06003045 RID: 12357 RVA: 0x000D4C39 File Offset: 0x000D2E39
		public int Compare(InventorySearch a, InventorySearch b)
		{
			return (int)(a.jar.item.quality - b.jar.item.quality);
		}
	}
}
