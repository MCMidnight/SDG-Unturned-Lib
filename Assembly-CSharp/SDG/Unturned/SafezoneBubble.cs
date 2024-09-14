using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200058D RID: 1421
	public class SafezoneBubble
	{
		// Token: 0x06002D7A RID: 11642 RVA: 0x000C64FB File Offset: 0x000C46FB
		public SafezoneBubble(Vector3 newOrigin, float newSqrRadius)
		{
			this.origin = newOrigin;
			this.sqrRadius = newSqrRadius;
		}

		// Token: 0x04001888 RID: 6280
		public Vector3 origin;

		// Token: 0x04001889 RID: 6281
		public float sqrRadius;
	}
}
