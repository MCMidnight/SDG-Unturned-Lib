using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000462 RID: 1122
	public class RubbleInfo
	{
		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x0600221B RID: 8731 RVA: 0x000842BB File Offset: 0x000824BB
		public bool isDead
		{
			get
			{
				return this.health == 0;
			}
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x000842C6 File Offset: 0x000824C6
		public void askDamage(ushort amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (amount >= this.health)
			{
				this.health = 0;
				return;
			}
			this.health -= amount;
		}

		// Token: 0x040010CD RID: 4301
		public float lastDead;

		// Token: 0x040010CE RID: 4302
		public ushort health;

		// Token: 0x040010CF RID: 4303
		public Transform section;

		// Token: 0x040010D0 RID: 4304
		public GameObject aliveGameObject;

		// Token: 0x040010D1 RID: 4305
		public GameObject deadGameObject;

		// Token: 0x040010D2 RID: 4306
		public RubbleRagdollInfo[] ragdolls;

		// Token: 0x040010D3 RID: 4307
		public Transform effectTransform;
	}
}
