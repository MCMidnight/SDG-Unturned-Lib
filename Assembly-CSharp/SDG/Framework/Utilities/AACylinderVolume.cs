using System;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	// Token: 0x0200007B RID: 123
	public struct AACylinderVolume : IShapeVolume
	{
		// Token: 0x06000302 RID: 770 RVA: 0x0000C070 File Offset: 0x0000A270
		public bool containsPoint(Vector3 point)
		{
			float num = this.height / 2f;
			if (point.y > this.center.y - num && point.y < this.center.y + num)
			{
				float num2 = this.radius * this.radius;
				return (new Vector2(point.x, point.z) - new Vector2(this.center.x, this.center.z)).sqrMagnitude < num2;
			}
			return false;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000C100 File Offset: 0x0000A300
		public Bounds worldBounds
		{
			get
			{
				float num = this.radius * 2f;
				return new Bounds(this.center, new Vector3(num, this.height, num));
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000C132 File Offset: 0x0000A332
		public float internalVolume
		{
			get
			{
				return this.height * 3.1415927f * this.radius * this.radius;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000C14E File Offset: 0x0000A34E
		public float surfaceArea
		{
			get
			{
				return 3.1415927f * this.radius * this.radius;
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000C163 File Offset: 0x0000A363
		public AACylinderVolume(Vector3 newCenter, float newRadius, float newHeight)
		{
			this.center = newCenter;
			this.radius = newRadius;
			this.height = newHeight;
		}

		// Token: 0x04000151 RID: 337
		public Vector3 center;

		// Token: 0x04000152 RID: 338
		public float radius;

		// Token: 0x04000153 RID: 339
		public float height;
	}
}
