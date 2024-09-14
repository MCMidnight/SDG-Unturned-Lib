using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006BB RID: 1723
	public struct RegionBounds
	{
		// Token: 0x06003986 RID: 14726 RVA: 0x0010DE88 File Offset: 0x0010C088
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

		// Token: 0x06003987 RID: 14727 RVA: 0x0010DEE0 File Offset: 0x0010C0E0
		public RegionBounds(RegionCoord newMin, RegionCoord newMax)
		{
			this.min = newMin;
			this.max = newMax;
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x0010DEF0 File Offset: 0x0010C0F0
		public RegionBounds(Bounds worldBounds)
		{
			this.min = new RegionCoord(worldBounds.min);
			this.min.ClampIntoBounds();
			this.max = new RegionCoord(worldBounds.max);
			this.max.ClampIntoBounds();
		}

		// Token: 0x0400223A RID: 8762
		public RegionCoord min;

		// Token: 0x0400223B RID: 8763
		public RegionCoord max;
	}
}
