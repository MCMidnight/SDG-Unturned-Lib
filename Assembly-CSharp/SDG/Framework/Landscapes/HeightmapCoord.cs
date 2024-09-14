using System;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	// Token: 0x0200009A RID: 154
	public struct HeightmapCoord : IEquatable<HeightmapCoord>
	{
		// Token: 0x060003C5 RID: 965 RVA: 0x0000F113 File Offset: 0x0000D313
		public static bool operator ==(HeightmapCoord a, HeightmapCoord b)
		{
			return a.x == b.x && a.y == b.y;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000F133 File Offset: 0x0000D333
		public static bool operator !=(HeightmapCoord a, HeightmapCoord b)
		{
			return !(a == b);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000F140 File Offset: 0x0000D340
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			HeightmapCoord heightmapCoord = (HeightmapCoord)obj;
			return this.x == heightmapCoord.x && this.y == heightmapCoord.y;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000F177 File Offset: 0x0000D377
		public override int GetHashCode()
		{
			return this.x ^ this.y;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000F188 File Offset: 0x0000D388
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				")"
			});
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000F1D4 File Offset: 0x0000D3D4
		public bool Equals(HeightmapCoord other)
		{
			return this.x == other.x && this.y == other.y;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000F1F4 File Offset: 0x0000D3F4
		public HeightmapCoord(int new_x, int new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000F204 File Offset: 0x0000D404
		public HeightmapCoord(LandscapeCoord tileCoord, Vector3 worldPosition)
		{
			this.x = Mathf.Clamp(Mathf.RoundToInt((worldPosition.z - (float)tileCoord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE), 0, Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE);
			this.y = Mathf.Clamp(Mathf.RoundToInt((worldPosition.x - (float)tileCoord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE), 0, Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE);
		}

		// Token: 0x0400019E RID: 414
		public int x;

		// Token: 0x0400019F RID: 415
		public int y;
	}
}
