using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005DE RID: 1502
	public struct YawSnapshotInfo : ISnapshotInfo<YawSnapshotInfo>
	{
		// Token: 0x0600302D RID: 12333 RVA: 0x000D485A File Offset: 0x000D2A5A
		public void lerp(YawSnapshotInfo target, float delta, out YawSnapshotInfo result)
		{
			result = default(YawSnapshotInfo);
			result.pos = Vector3.Lerp(this.pos, target.pos, delta);
			result.yaw = Mathf.LerpAngle(this.yaw, target.yaw, delta);
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x000D4893 File Offset: 0x000D2A93
		public YawSnapshotInfo(Vector3 pos, float yaw)
		{
			this.pos = pos;
			this.yaw = yaw;
		}

		// Token: 0x04001A3F RID: 6719
		public Vector3 pos;

		// Token: 0x04001A40 RID: 6720
		public float yaw;
	}
}
