using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200047E RID: 1150
	[Obsolete("Replaced by MarkForReplicationUpdate. Will be removed in a future release.")]
	public struct VehicleStateUpdate
	{
		// Token: 0x06002319 RID: 8985 RVA: 0x000895E9 File Offset: 0x000877E9
		public VehicleStateUpdate(Vector3 pos, Quaternion rot)
		{
			this.pos = pos;
			this.rot = rot;
		}

		// Token: 0x04001171 RID: 4465
		public Vector3 pos;

		// Token: 0x04001172 RID: 4466
		public Quaternion rot;
	}
}
