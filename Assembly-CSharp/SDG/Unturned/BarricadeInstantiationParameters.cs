using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200053E RID: 1342
	internal struct BarricadeInstantiationParameters : IComparable<BarricadeInstantiationParameters>
	{
		// Token: 0x060029F8 RID: 10744 RVA: 0x000B2AFF File Offset: 0x000B0CFF
		public int CompareTo(BarricadeInstantiationParameters other)
		{
			return this.sortOrder.CompareTo(other.sortOrder);
		}

		// Token: 0x04001694 RID: 5780
		public BarricadeRegion region;

		// Token: 0x04001695 RID: 5781
		public Guid assetId;

		// Token: 0x04001696 RID: 5782
		public byte[] state;

		// Token: 0x04001697 RID: 5783
		public Vector3 position;

		// Token: 0x04001698 RID: 5784
		public Quaternion rotation;

		// Token: 0x04001699 RID: 5785
		public byte hp;

		// Token: 0x0400169A RID: 5786
		public ulong owner;

		// Token: 0x0400169B RID: 5787
		public ulong group;

		// Token: 0x0400169C RID: 5788
		public NetId netId;

		// Token: 0x0400169D RID: 5789
		public float sortOrder;
	}
}
