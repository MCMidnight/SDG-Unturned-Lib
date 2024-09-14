using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020005F9 RID: 1529
	public class InventorySearchAmountAscendingComparator : IComparer<InventorySearch>
	{
		// Token: 0x06003049 RID: 12361 RVA: 0x000D4C8F File Offset: 0x000D2E8F
		public int Compare(InventorySearch a, InventorySearch b)
		{
			return (int)(a.jar.item.amount - b.jar.item.amount);
		}
	}
}
