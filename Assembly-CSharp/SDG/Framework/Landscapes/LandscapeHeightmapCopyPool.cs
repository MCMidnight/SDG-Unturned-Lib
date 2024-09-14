using System;
using SDG.Framework.Utilities;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000A0 RID: 160
	public static class LandscapeHeightmapCopyPool
	{
		// Token: 0x06000422 RID: 1058 RVA: 0x00010FC6 File Offset: 0x0000F1C6
		public static void empty()
		{
			LandscapeHeightmapCopyPool.pool.empty();
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00010FD2 File Offset: 0x0000F1D2
		public static void warmup(uint count)
		{
			LandscapeHeightmapCopyPool.pool.warmup(count, new Pool<float[,]>.PoolClaimHandler(LandscapeHeightmapCopyPool.handlePoolClaim));
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00010FEB File Offset: 0x0000F1EB
		public static float[,] claim()
		{
			return LandscapeHeightmapCopyPool.pool.claim(new Pool<float[,]>.PoolClaimHandler(LandscapeHeightmapCopyPool.handlePoolClaim));
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00011003 File Offset: 0x0000F203
		public static void release(float[,] copy)
		{
			LandscapeHeightmapCopyPool.pool.release(copy, new Pool<float[,]>.PoolReleasedHandler(LandscapeHeightmapCopyPool.handlePoolRelease));
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001101C File Offset: 0x0000F21C
		private static float[,] handlePoolClaim(Pool<float[,]> pool)
		{
			return new float[Landscape.HEIGHTMAP_RESOLUTION, Landscape.HEIGHTMAP_RESOLUTION];
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001102D File Offset: 0x0000F22D
		private static void handlePoolRelease(Pool<float[,]> pool, float[,] copy)
		{
		}

		// Token: 0x040001C2 RID: 450
		private static Pool<float[,]> pool = new Pool<float[,]>();
	}
}
