using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005DF RID: 1503
	public struct PitchYawSnapshotInfo : ISnapshotInfo<PitchYawSnapshotInfo>
	{
		// Token: 0x0600302F RID: 12335 RVA: 0x000D48A4 File Offset: 0x000D2AA4
		public void lerp(PitchYawSnapshotInfo target, float delta, out PitchYawSnapshotInfo result)
		{
			result = default(PitchYawSnapshotInfo);
			result.pos = Vector3.Lerp(this.pos, target.pos, delta);
			result.pitch = Mathf.LerpAngle(this.pitch, target.pitch, delta);
			result.yaw = Mathf.LerpAngle(this.yaw, target.yaw, delta);
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x000D4900 File Offset: 0x000D2B00
		public PitchYawSnapshotInfo(Vector3 pos, float pitch, float yaw)
		{
			this.pos = pos;
			this.pitch = pitch;
			this.yaw = yaw;
		}

		// Token: 0x04001A41 RID: 6721
		public Vector3 pos;

		// Token: 0x04001A42 RID: 6722
		public float pitch;

		// Token: 0x04001A43 RID: 6723
		public float yaw;
	}
}
