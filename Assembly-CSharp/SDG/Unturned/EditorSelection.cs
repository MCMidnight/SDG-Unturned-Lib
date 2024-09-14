using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000408 RID: 1032
	public class EditorSelection
	{
		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x000703D1 File Offset: 0x0006E5D1
		public Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000703D9 File Offset: 0x0006E5D9
		public EditorSelection(Transform newTransform, Vector3 newFromPosition, Quaternion newFromRotation, Vector3 newFromScale)
		{
			this._transform = newTransform;
			this.fromPosition = newFromPosition;
			this.fromRotation = newFromRotation;
			this.fromScale = newFromScale;
		}

		// Token: 0x04000EB4 RID: 3764
		private Transform _transform;

		// Token: 0x04000EB5 RID: 3765
		public Vector3 fromPosition;

		// Token: 0x04000EB6 RID: 3766
		public Quaternion fromRotation;

		// Token: 0x04000EB7 RID: 3767
		public Vector3 fromScale;

		// Token: 0x04000EB8 RID: 3768
		public Matrix4x4 relativeToPivot;
	}
}
