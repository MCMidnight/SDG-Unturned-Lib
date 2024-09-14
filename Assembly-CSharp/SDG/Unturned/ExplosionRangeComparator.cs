using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200074D RID: 1869
	public class ExplosionRangeComparator : IComparer<Transform>
	{
		// Token: 0x06003D1D RID: 15645 RVA: 0x00123D88 File Offset: 0x00121F88
		public int Compare(Transform a, Transform b)
		{
			return Mathf.RoundToInt(((a.position - this.point).sqrMagnitude - (b.position - this.point).sqrMagnitude) * 100f);
		}

		// Token: 0x04002675 RID: 9845
		public Vector3 point;
	}
}
