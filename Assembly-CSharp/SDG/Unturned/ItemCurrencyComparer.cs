using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Sort currency entries by value.
	/// </summary>
	// Token: 0x020002DD RID: 733
	internal class ItemCurrencyComparer : Comparer<ItemCurrencyAsset.Entry>
	{
		// Token: 0x060015DD RID: 5597 RVA: 0x00051236 File Offset: 0x0004F436
		public override int Compare(ItemCurrencyAsset.Entry x, ItemCurrencyAsset.Entry y)
		{
			return x.value.CompareTo(y.value);
		}
	}
}
