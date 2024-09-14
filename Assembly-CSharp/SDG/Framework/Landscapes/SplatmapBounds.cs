using System;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000AC RID: 172
	public struct SplatmapBounds
	{
		// Token: 0x0600048E RID: 1166 RVA: 0x00012FBC File Offset: 0x000111BC
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

		// Token: 0x0600048F RID: 1167 RVA: 0x00013014 File Offset: 0x00011214
		public SplatmapBounds(SplatmapCoord newMin, SplatmapCoord newMax)
		{
			this.min = newMin;
			this.max = newMax;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00013024 File Offset: 0x00011224
		public SplatmapBounds(LandscapeCoord tileCoord, Bounds worldBounds)
		{
			int new_x = Mathf.Clamp(Mathf.FloorToInt((worldBounds.min.z - (float)tileCoord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.SPLATMAP_RESOLUTION), 0, Landscape.SPLATMAP_RESOLUTION_MINUS_ONE);
			int new_x2 = Mathf.Clamp(Mathf.FloorToInt((worldBounds.max.z - (float)tileCoord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.SPLATMAP_RESOLUTION), 0, Landscape.SPLATMAP_RESOLUTION_MINUS_ONE);
			int new_y = Mathf.Clamp(Mathf.FloorToInt((worldBounds.min.x - (float)tileCoord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.SPLATMAP_RESOLUTION), 0, Landscape.SPLATMAP_RESOLUTION_MINUS_ONE);
			int new_y2 = Mathf.Clamp(Mathf.FloorToInt((worldBounds.max.x - (float)tileCoord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.SPLATMAP_RESOLUTION), 0, Landscape.SPLATMAP_RESOLUTION_MINUS_ONE);
			this.min = new SplatmapCoord(new_x, new_y);
			this.max = new SplatmapCoord(new_x2, new_y2);
		}

		// Token: 0x040001DE RID: 478
		public SplatmapCoord min;

		// Token: 0x040001DF RID: 479
		public SplatmapCoord max;
	}
}
