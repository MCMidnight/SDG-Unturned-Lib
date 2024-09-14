using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000777 RID: 1911
	public class AssetNameAscendingComparator : IComparer<Asset>
	{
		// Token: 0x06003E8E RID: 16014 RVA: 0x00132AB3 File Offset: 0x00130CB3
		public int Compare(Asset a, Asset b)
		{
			return a.name.CompareTo(b.name);
		}
	}
}
