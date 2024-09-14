using System;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000E8 RID: 232
	public struct FoliageBounds
	{
		// Token: 0x060005AA RID: 1450 RVA: 0x00015790 File Offset: 0x00013990
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.min.ToString(),
				", ",
				this.max.ToString(),
				"]"
			});
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x000157E8 File Offset: 0x000139E8
		public FoliageBounds(FoliageCoord newMin, FoliageCoord newMax)
		{
			this.min = newMin;
			this.max = newMax;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x000157F8 File Offset: 0x000139F8
		public FoliageBounds(Bounds worldBounds)
		{
			this.min = new FoliageCoord(worldBounds.min);
			this.max = new FoliageCoord(worldBounds.max);
		}

		// Token: 0x04000208 RID: 520
		public FoliageCoord min;

		// Token: 0x04000209 RID: 521
		public FoliageCoord max;
	}
}
