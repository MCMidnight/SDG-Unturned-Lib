using System;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	// Token: 0x02000084 RID: 132
	public struct SphereVolume : IShapeVolume
	{
		// Token: 0x06000335 RID: 821 RVA: 0x0000C7EC File Offset: 0x0000A9EC
		public bool containsPoint(Vector3 point)
		{
			if (Mathf.Abs(point.x - this.center.x) >= this.radius)
			{
				return false;
			}
			if (Mathf.Abs(point.y - this.center.y) >= this.radius)
			{
				return false;
			}
			if (Mathf.Abs(point.z - this.center.z) >= this.radius)
			{
				return false;
			}
			float num = this.radius * this.radius;
			return (point - this.center).sqrMagnitude < num;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000C884 File Offset: 0x0000AA84
		public Bounds worldBounds
		{
			get
			{
				float num = this.radius * 2f;
				return new Bounds(this.center, new Vector3(num, num, num));
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000C8B1 File Offset: 0x0000AAB1
		public float internalVolume
		{
			get
			{
				return 4.1887903f * this.radius * this.radius * this.radius;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000C8CD File Offset: 0x0000AACD
		public float surfaceArea
		{
			get
			{
				return 12.566371f * this.radius * this.radius;
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000C8E2 File Offset: 0x0000AAE2
		public SphereVolume(Vector3 newCenter, float newRadius)
		{
			this.center = newCenter;
			this.radius = newRadius;
		}

		// Token: 0x0400015B RID: 347
		public Vector3 center;

		// Token: 0x0400015C RID: 348
		public float radius;
	}
}
