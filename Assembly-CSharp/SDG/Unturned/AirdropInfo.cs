using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200056F RID: 1391
	public class AirdropInfo
	{
		// Token: 0x040017EE RID: 6126
		public Transform model;

		// Token: 0x040017EF RID: 6127
		public ushort id;

		// Token: 0x040017F0 RID: 6128
		public Vector3 state;

		// Token: 0x040017F1 RID: 6129
		public Vector3 direction;

		// Token: 0x040017F2 RID: 6130
		public float speed;

		// Token: 0x040017F3 RID: 6131
		public float delay;

		// Token: 0x040017F4 RID: 6132
		public float force;

		// Token: 0x040017F5 RID: 6133
		public bool dropped;

		/// <summary>
		/// Calculated position (not directly replaced) to spawn falling box.
		/// </summary>
		// Token: 0x040017F6 RID: 6134
		public Vector3 dropPosition;
	}
}
