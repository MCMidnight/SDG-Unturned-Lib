using System;
using SDG.Framework.Utilities;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000A8 RID: 168
	public static class LandscapeSplatmapCopyPool
	{
		// Token: 0x06000450 RID: 1104 RVA: 0x00011900 File Offset: 0x0000FB00
		public static void empty()
		{
			LandscapeSplatmapCopyPool.pool.empty();
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001190C File Offset: 0x0000FB0C
		public static void warmup(uint count)
		{
			LandscapeSplatmapCopyPool.pool.warmup(count, new Pool<float[,,]>.PoolClaimHandler(LandscapeSplatmapCopyPool.handlePoolClaim));
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00011925 File Offset: 0x0000FB25
		public static float[,,] claim()
		{
			return LandscapeSplatmapCopyPool.pool.claim(new Pool<float[,,]>.PoolClaimHandler(LandscapeSplatmapCopyPool.handlePoolClaim));
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0001193D File Offset: 0x0000FB3D
		public static void release(float[,,] copy)
		{
			LandscapeSplatmapCopyPool.pool.release(copy, new Pool<float[,,]>.PoolReleasedHandler(LandscapeSplatmapCopyPool.handlePoolRelease));
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00011956 File Offset: 0x0000FB56
		private static float[,,] handlePoolClaim(Pool<float[,,]> pool)
		{
			return new float[Landscape.SPLATMAP_RESOLUTION, Landscape.SPLATMAP_RESOLUTION, Landscape.SPLATMAP_LAYERS];
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001196C File Offset: 0x0000FB6C
		private static void handlePoolRelease(Pool<float[,,]> pool, float[,,] copy)
		{
		}

		// Token: 0x040001CE RID: 462
		private static Pool<float[,,]> pool = new Pool<float[,,]>();
	}
}
