using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020005FA RID: 1530
	public class InventorySearchAmountDescendingComparator : IComparer<InventorySearch>
	{
		// Token: 0x0600304B RID: 12363 RVA: 0x000D4CBA File Offset: 0x000D2EBA
		public int Compare(InventorySearch a, InventorySearch b)
		{
			return (int)(b.jar.item.amount - a.jar.item.amount);
		}
	}
}
