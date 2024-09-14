using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000569 RID: 1385
	internal struct ItemInstantiationParameters : IComparable<ItemInstantiationParameters>
	{
		// Token: 0x06002C2A RID: 11306 RVA: 0x000BE42D File Offset: 0x000BC62D
		public int CompareTo(ItemInstantiationParameters other)
		{
			return this.sortOrder.CompareTo(other.sortOrder);
		}

		// Token: 0x040017B0 RID: 6064
		public byte region_x;

		// Token: 0x040017B1 RID: 6065
		public byte region_y;

		// Token: 0x040017B2 RID: 6066
		public ushort assetId;

		// Token: 0x040017B3 RID: 6067
		public byte amount;

		// Token: 0x040017B4 RID: 6068
		public byte quality;

		// Token: 0x040017B5 RID: 6069
		public byte[] state;

		// Token: 0x040017B6 RID: 6070
		public Vector3 point;

		// Token: 0x040017B7 RID: 6071
		public uint instanceID;

		// Token: 0x040017B8 RID: 6072
		public float sortOrder;

		// Token: 0x040017B9 RID: 6073
		public bool shouldPlayEffect;
	}
}
