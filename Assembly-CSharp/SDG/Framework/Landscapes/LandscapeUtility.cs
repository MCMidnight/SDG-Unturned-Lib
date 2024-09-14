using System;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000AB RID: 171
	public class LandscapeUtility
	{
		/// <summary>
		/// If a heightmap coordinate is out of bounds the tile/heightamp coordinate will be adjusted so that it is in bounds again.
		/// </summary>
		// Token: 0x0600048B RID: 1163 RVA: 0x00012E6C File Offset: 0x0001106C
		public static void cleanHeightmapCoord(ref LandscapeCoord tileCoord, ref HeightmapCoord heightmapCoord)
		{
			if (heightmapCoord.x < 0)
			{
				tileCoord.y--;
				heightmapCoord.x += Landscape.HEIGHTMAP_RESOLUTION;
			}
			if (heightmapCoord.y < 0)
			{
				tileCoord.x--;
				heightmapCoord.y += Landscape.HEIGHTMAP_RESOLUTION;
			}
			if (heightmapCoord.x >= Landscape.HEIGHTMAP_RESOLUTION)
			{
				tileCoord.y++;
				heightmapCoord.x -= Landscape.HEIGHTMAP_RESOLUTION;
			}
			if (heightmapCoord.y >= Landscape.HEIGHTMAP_RESOLUTION)
			{
				tileCoord.x++;
				heightmapCoord.y -= Landscape.HEIGHTMAP_RESOLUTION;
			}
		}

		/// <summary>
		/// If a splatmap coordinate is out of bounds the tile/splatmap coordinate will be adjusted so that it is in bounds again.
		/// </summary>
		// Token: 0x0600048C RID: 1164 RVA: 0x00012F10 File Offset: 0x00011110
		public static void cleanSplatmapCoord(ref LandscapeCoord tileCoord, ref SplatmapCoord splatmapCoord)
		{
			if (splatmapCoord.x < 0)
			{
				tileCoord.y--;
				splatmapCoord.x += Landscape.SPLATMAP_RESOLUTION;
			}
			if (splatmapCoord.y < 0)
			{
				tileCoord.x--;
				splatmapCoord.y += Landscape.SPLATMAP_RESOLUTION;
			}
			if (splatmapCoord.x >= Landscape.SPLATMAP_RESOLUTION)
			{
				tileCoord.y++;
				splatmapCoord.x -= Landscape.SPLATMAP_RESOLUTION;
			}
			if (splatmapCoord.y >= Landscape.SPLATMAP_RESOLUTION)
			{
				tileCoord.x++;
				splatmapCoord.y -= Landscape.SPLATMAP_RESOLUTION;
			}
		}
	}
}
