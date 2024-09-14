using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000645 RID: 1605
	public struct PlayerStateUpdate
	{
		// Token: 0x0600346E RID: 13422 RVA: 0x000F13DB File Offset: 0x000EF5DB
		public PlayerStateUpdate(Vector3 pos, byte angle, byte rot)
		{
			this.pos = pos;
			this.angle = angle;
			this.rot = rot;
		}

		// Token: 0x04001E44 RID: 7748
		public Vector3 pos;

		// Token: 0x04001E45 RID: 7749
		public byte angle;

		// Token: 0x04001E46 RID: 7750
		public byte rot;
	}
}
