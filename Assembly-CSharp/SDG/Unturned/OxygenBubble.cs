using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000583 RID: 1411
	public class OxygenBubble
	{
		// Token: 0x06002D3A RID: 11578 RVA: 0x000C4DBD File Offset: 0x000C2FBD
		public OxygenBubble(Transform newOrigin, float newSqrRadius)
		{
			this.origin = newOrigin;
			this.sqrRadius = newSqrRadius;
		}

		// Token: 0x04001866 RID: 6246
		public Transform origin;

		// Token: 0x04001867 RID: 6247
		public float sqrRadius;
	}
}
