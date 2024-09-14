using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200041F RID: 1055
	public class WorkzoneSelection
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001F5C RID: 8028 RVA: 0x0007954F File Offset: 0x0007774F
		public Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x00079557 File Offset: 0x00077757
		public WorkzoneSelection(Transform newTransform)
		{
			this._transform = newTransform;
		}

		// Token: 0x04000F9E RID: 3998
		private Transform _transform;

		// Token: 0x04000F9F RID: 3999
		public Vector3 preTransformPosition;

		// Token: 0x04000FA0 RID: 4000
		public Quaternion preTransformRotation;
	}
}
