using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005DD RID: 1501
	public struct TransformSnapshotInfo : ISnapshotInfo<TransformSnapshotInfo>
	{
		// Token: 0x0600302B RID: 12331 RVA: 0x000D4811 File Offset: 0x000D2A11
		public void lerp(TransformSnapshotInfo target, float delta, out TransformSnapshotInfo result)
		{
			result = default(TransformSnapshotInfo);
			result.pos = Vector3.Lerp(this.pos, target.pos, delta);
			result.rot = Quaternion.Slerp(this.rot, target.rot, delta);
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000D484A File Offset: 0x000D2A4A
		public TransformSnapshotInfo(Vector3 pos, Quaternion rot)
		{
			this.pos = pos;
			this.rot = rot;
		}

		// Token: 0x04001A3D RID: 6717
		public Vector3 pos;

		// Token: 0x04001A3E RID: 6718
		public Quaternion rot;
	}
}
