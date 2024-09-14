using System;
using UnityEngine;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x0200013A RID: 314
	public class TransformDelta
	{
		// Token: 0x06000809 RID: 2057 RVA: 0x0001CCF2 File Offset: 0x0001AEF2
		public void get(Transform transform)
		{
			this.localPosition = transform.localPosition;
			this.localRotation = transform.localRotation;
			this.localScale = transform.localScale;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001CD18 File Offset: 0x0001AF18
		public void set(Transform transform)
		{
			transform.parent = this.parent;
			transform.localPosition = this.localPosition;
			transform.localRotation = this.localRotation;
			transform.localScale = this.localScale;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001CD4A File Offset: 0x0001AF4A
		public TransformDelta(Transform newParent)
		{
			this.parent = newParent;
		}

		// Token: 0x040002DE RID: 734
		public Transform parent;

		// Token: 0x040002DF RID: 735
		public Vector3 localPosition;

		// Token: 0x040002E0 RID: 736
		public Quaternion localRotation;

		// Token: 0x040002E1 RID: 737
		public Vector3 localScale;
	}
}
