using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000552 RID: 1362
	public class ClaimPlant : ClaimBase
	{
		// Token: 0x06002B23 RID: 11043 RVA: 0x000B81CD File Offset: 0x000B63CD
		public ClaimPlant(Transform newParent, ulong newOwner, ulong newGroup) : base(newOwner, newGroup)
		{
			this.parent = newParent;
		}

		// Token: 0x04001702 RID: 5890
		public Transform parent;
	}
}
