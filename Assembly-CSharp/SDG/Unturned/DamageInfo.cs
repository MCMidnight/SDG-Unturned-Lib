using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200074B RID: 1867
	public class DamageInfo
	{
		// Token: 0x06003D1B RID: 15643 RVA: 0x00123D30 File Offset: 0x00121F30
		public void update(RaycastHit hit)
		{
			this.transform = hit.transform;
			this.collider = hit.collider;
			this.distance = hit.distance;
			this.point = hit.point;
			this.normal = hit.normal;
		}

		// Token: 0x04002668 RID: 9832
		public Transform transform;

		// Token: 0x04002669 RID: 9833
		public Collider collider;

		// Token: 0x0400266A RID: 9834
		public float distance;

		// Token: 0x0400266B RID: 9835
		public Vector3 point;

		// Token: 0x0400266C RID: 9836
		public Vector3 normal;

		// Token: 0x0400266D RID: 9837
		public Player player;

		// Token: 0x0400266E RID: 9838
		public Zombie zombie;

		// Token: 0x0400266F RID: 9839
		public InteractableVehicle vehicle;
	}
}
