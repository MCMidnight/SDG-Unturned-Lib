using System;
using UnityEngine;

namespace SDG.Framework.Devkit.Transactions
{
	// Token: 0x0200013B RID: 315
	public class DevkitTransformChangeParentTransaction : IDevkitTransaction
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x0001CD59 File Offset: 0x0001AF59
		public bool delta
		{
			get
			{
				return this.parentBefore.parent != this.parentAfter.parent;
			}
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001CD76 File Offset: 0x0001AF76
		public void undo()
		{
			this.parentBefore.set(this.transform);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001CD89 File Offset: 0x0001AF89
		public void redo()
		{
			this.parentAfter.set(this.transform);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001CD9C File Offset: 0x0001AF9C
		public void begin()
		{
			this.parentBefore = new TransformDelta(this.transform.parent);
			this.parentBefore.get(this.transform);
			this.transform.parent = this.parentAfter.parent;
			this.parentAfter.get(this.transform);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0001CDF7 File Offset: 0x0001AFF7
		public void end()
		{
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0001CDF9 File Offset: 0x0001AFF9
		public void forget()
		{
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001CDFB File Offset: 0x0001AFFB
		public DevkitTransformChangeParentTransaction(Transform newTransform, Transform newParent)
		{
			this.transform = newTransform;
			this.parentAfter = new TransformDelta(newParent);
		}

		// Token: 0x040002E2 RID: 738
		protected Transform transform;

		// Token: 0x040002E3 RID: 739
		protected TransformDelta parentBefore;

		// Token: 0x040002E4 RID: 740
		protected TransformDelta parentAfter;
	}
}
