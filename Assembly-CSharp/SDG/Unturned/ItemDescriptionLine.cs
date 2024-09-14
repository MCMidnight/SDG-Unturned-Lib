using System;

namespace SDG.Unturned
{
	// Token: 0x020002CE RID: 718
	public struct ItemDescriptionLine : IComparable<ItemDescriptionLine>
	{
		// Token: 0x060014E8 RID: 5352 RVA: 0x0004D76D File Offset: 0x0004B96D
		public int CompareTo(ItemDescriptionLine other)
		{
			if (this.sortOrder == other.sortOrder)
			{
				return this.text.CompareTo(other.text);
			}
			return this.sortOrder.CompareTo(other.sortOrder);
		}

		// Token: 0x04000862 RID: 2146
		public string text;

		// Token: 0x04000863 RID: 2147
		public int sortOrder;
	}
}
