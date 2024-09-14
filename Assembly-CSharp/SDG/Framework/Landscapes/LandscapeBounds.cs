using System;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	// Token: 0x0200009E RID: 158
	public struct LandscapeBounds
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x00010DA4 File Offset: 0x0000EFA4
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

		// Token: 0x06000415 RID: 1045 RVA: 0x00010DFC File Offset: 0x0000EFFC
		public LandscapeBounds(LandscapeCoord newMin, LandscapeCoord newMax)
		{
			this.min = newMin;
			this.max = newMax;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00010E0C File Offset: 0x0000F00C
		public LandscapeBounds(Bounds worldBounds)
		{
			this.min = new LandscapeCoord(worldBounds.min);
			this.max = new LandscapeCoord(worldBounds.max);
		}

		// Token: 0x040001BD RID: 445
		public LandscapeCoord min;

		// Token: 0x040001BE RID: 446
		public LandscapeCoord max;
	}
}
