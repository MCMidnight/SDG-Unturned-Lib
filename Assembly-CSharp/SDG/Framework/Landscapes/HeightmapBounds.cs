using System;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	// Token: 0x02000099 RID: 153
	public struct HeightmapBounds
	{
		// Token: 0x060003C2 RID: 962 RVA: 0x0000EFA4 File Offset: 0x0000D1A4
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

		// Token: 0x060003C3 RID: 963 RVA: 0x0000EFFC File Offset: 0x0000D1FC
		public HeightmapBounds(HeightmapCoord newMin, HeightmapCoord newMax)
		{
			this.min = newMin;
			this.max = newMax;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000F00C File Offset: 0x0000D20C
		public HeightmapBounds(LandscapeCoord tileCoord, Bounds worldBounds)
		{
			int new_x = Mathf.Clamp(Mathf.FloorToInt((worldBounds.min.z - (float)tileCoord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE), 0, Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE);
			int new_x2 = Mathf.Clamp(Mathf.CeilToInt((worldBounds.max.z - (float)tileCoord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE), 0, Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE);
			int new_y = Mathf.Clamp(Mathf.FloorToInt((worldBounds.min.x - (float)tileCoord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE), 0, Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE);
			int new_y2 = Mathf.Clamp(Mathf.CeilToInt((worldBounds.max.x - (float)tileCoord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE), 0, Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE);
			this.min = new HeightmapCoord(new_x, new_y);
			this.max = new HeightmapCoord(new_x2, new_y2);
		}

		// Token: 0x0400019C RID: 412
		public HeightmapCoord min;

		// Token: 0x0400019D RID: 413
		public HeightmapCoord max;
	}
}
