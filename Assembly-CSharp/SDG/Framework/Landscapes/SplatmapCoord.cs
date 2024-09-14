using System;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000AD RID: 173
	public struct SplatmapCoord : IEquatable<SplatmapCoord>
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x0001312B File Offset: 0x0001132B
		public static bool operator ==(SplatmapCoord a, SplatmapCoord b)
		{
			return a.x == b.x && a.y == b.y;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001314B File Offset: 0x0001134B
		public static bool operator !=(SplatmapCoord a, SplatmapCoord b)
		{
			return !(a == b);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00013158 File Offset: 0x00011358
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			SplatmapCoord splatmapCoord = (SplatmapCoord)obj;
			return this.x.Equals(splatmapCoord.x) && this.y.Equals(splatmapCoord.y);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00013197 File Offset: 0x00011397
		public override int GetHashCode()
		{
			return this.x ^ this.y;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x000131A8 File Offset: 0x000113A8
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

		// Token: 0x06000496 RID: 1174 RVA: 0x000131F4 File Offset: 0x000113F4
		public bool Equals(SplatmapCoord other)
		{
			return this.x == other.x && this.y == other.y;
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00013214 File Offset: 0x00011414
		public SplatmapCoord(int new_x, int new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00013224 File Offset: 0x00011424
		public SplatmapCoord(LandscapeCoord tileCoord, Vector3 worldPosition)
		{
			this.x = Mathf.Clamp(Mathf.FloorToInt((worldPosition.z - (float)tileCoord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.SPLATMAP_RESOLUTION), 0, Landscape.SPLATMAP_RESOLUTION_MINUS_ONE);
			this.y = Mathf.Clamp(Mathf.FloorToInt((worldPosition.x - (float)tileCoord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE * (float)Landscape.SPLATMAP_RESOLUTION), 0, Landscape.SPLATMAP_RESOLUTION_MINUS_ONE);
		}

		// Token: 0x040001E0 RID: 480
		public int x;

		// Token: 0x040001E1 RID: 481
		public int y;
	}
}
