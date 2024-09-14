using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200059B RID: 1435
	internal struct StructureInstantiationParameters : IComparable<StructureInstantiationParameters>
	{
		// Token: 0x06002DC2 RID: 11714 RVA: 0x000C6F40 File Offset: 0x000C5140
		public int CompareTo(StructureInstantiationParameters other)
		{
			return this.sortOrder.CompareTo(other.sortOrder);
		}

		// Token: 0x040018A4 RID: 6308
		public StructureRegion region;

		// Token: 0x040018A5 RID: 6309
		public Guid assetId;

		// Token: 0x040018A6 RID: 6310
		public Vector3 position;

		// Token: 0x040018A7 RID: 6311
		public Quaternion rotation;

		// Token: 0x040018A8 RID: 6312
		public byte hp;

		// Token: 0x040018A9 RID: 6313
		public ulong owner;

		// Token: 0x040018AA RID: 6314
		public ulong group;

		// Token: 0x040018AB RID: 6315
		public NetId netId;

		// Token: 0x040018AC RID: 6316
		public float sortOrder;
	}
}
