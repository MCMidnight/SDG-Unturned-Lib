using System;
using SDG.Framework.Utilities;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000A2 RID: 162
	public static class LandscapeHoleCopyPool
	{
		// Token: 0x06000430 RID: 1072 RVA: 0x0001110D File Offset: 0x0000F30D
		public static void empty()
		{
			LandscapeHoleCopyPool.pool.empty();
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00011119 File Offset: 0x0000F319
		public static void warmup(uint count)
		{
			LandscapeHoleCopyPool.pool.warmup(count, new Pool<bool[,]>.PoolClaimHandler(LandscapeHoleCopyPool.handlePoolClaim));
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00011132 File Offset: 0x0000F332
		public static bool[,] claim()
		{
			return LandscapeHoleCopyPool.pool.claim(new Pool<bool[,]>.PoolClaimHandler(LandscapeHoleCopyPool.handlePoolClaim));
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001114A File Offset: 0x0000F34A
		public static void release(bool[,] copy)
		{
			LandscapeHoleCopyPool.pool.release(copy, new Pool<bool[,]>.PoolReleasedHandler(LandscapeHoleCopyPool.handlePoolRelease));
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00011163 File Offset: 0x0000F363
		private static bool[,] handlePoolClaim(Pool<bool[,]> pool)
		{
			return new bool[256, 256];
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00011174 File Offset: 0x0000F374
		private static void handlePoolRelease(Pool<bool[,]> pool, bool[,] copy)
		{
		}

		// Token: 0x040001C5 RID: 453
		private static Pool<bool[,]> pool = new Pool<bool[,]>();
	}
}
