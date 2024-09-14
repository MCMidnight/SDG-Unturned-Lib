using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000383 RID: 899
	public class VendorSellingNameAscendingComparator : IComparer<VendorSellingBase>
	{
		// Token: 0x06001BF2 RID: 7154 RVA: 0x00063EBC File Offset: 0x000620BC
		public int Compare(VendorSellingBase a, VendorSellingBase b)
		{
			string displayName = a.displayName;
			string displayName2 = b.displayName;
			if (displayName == null || displayName2 == null)
			{
				return 0;
			}
			return displayName.CompareTo(displayName2);
		}
	}
}
