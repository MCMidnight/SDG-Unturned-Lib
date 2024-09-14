using System;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000EA RID: 234
	public class FoliageCut
	{
		// Token: 0x060005B8 RID: 1464 RVA: 0x000159AC File Offset: 0x00013BAC
		public bool ContainsPoint(Vector3 position)
		{
			float num = this.height * 0.5f;
			if (position.y > this.center.y - num && position.y < this.center.y + num)
			{
				float num2 = this.radius * this.radius;
				return (new Vector2(position.x, position.z) - new Vector2(this.center.x, this.center.z)).sqrMagnitude < num2;
			}
			return false;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00015A3C File Offset: 0x00013C3C
		public FoliageCut(Vector3 center, float radius, float height)
		{
			this.center = center;
			this.radius = radius;
			this.height = height;
			float num = radius * 2f;
			Bounds worldBounds = new Bounds(center, new Vector3(num, height, num));
			this.foliageBounds = new FoliageBounds(worldBounds);
		}

		// Token: 0x0400020D RID: 525
		internal FoliageBounds foliageBounds;

		// Token: 0x0400020E RID: 526
		private Vector3 center;

		// Token: 0x0400020F RID: 527
		private float radius;

		// Token: 0x04000210 RID: 528
		private float height;
	}
}
