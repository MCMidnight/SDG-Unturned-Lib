using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200059E RID: 1438
	public class TemperatureBubble
	{
		// Token: 0x06002E08 RID: 11784 RVA: 0x000C8D10 File Offset: 0x000C6F10
		public TemperatureBubble(Transform newOrigin, float newSqrRadius, EPlayerTemperature newTemperature)
		{
			this.origin = newOrigin;
			this.sqrRadius = newSqrRadius;
			this.temperature = newTemperature;
		}

		// Token: 0x040018D0 RID: 6352
		public Transform origin;

		// Token: 0x040018D1 RID: 6353
		public float sqrRadius;

		// Token: 0x040018D2 RID: 6354
		public EPlayerTemperature temperature;
	}
}
