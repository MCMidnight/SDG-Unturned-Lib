using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000551 RID: 1361
	public class ClaimBubble : ClaimBase
	{
		// Token: 0x06002B22 RID: 11042 RVA: 0x000B81B4 File Offset: 0x000B63B4
		public ClaimBubble(Vector3 newOrigin, float newSqrRadius, ulong newOwner, ulong newGroup) : base(newOwner, newGroup)
		{
			this.origin = newOrigin;
			this.sqrRadius = newSqrRadius;
		}

		// Token: 0x04001700 RID: 5888
		public Vector3 origin;

		// Token: 0x04001701 RID: 5889
		public float sqrRadius;
	}
}
