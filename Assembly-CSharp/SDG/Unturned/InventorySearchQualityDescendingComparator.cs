using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020005F8 RID: 1528
	public class InventorySearchQualityDescendingComparator : IComparer<InventorySearch>
	{
		// Token: 0x06003047 RID: 12359 RVA: 0x000D4C64 File Offset: 0x000D2E64
		public int Compare(InventorySearch a, InventorySearch b)
		{
			return (int)(b.jar.item.quality - a.jar.item.quality);
		}
	}
}
