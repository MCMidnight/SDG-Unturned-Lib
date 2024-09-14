using System;

namespace SDG.Framework.Utilities
{
	/// <summary>
	/// Pool of objects that implement the IPoolable interface.
	///
	/// Useful for types that do not need special construction,
	/// and want notification when claimed and released.
	/// </summary>
	// Token: 0x02000083 RID: 131
	public static class PoolablePool<T> where T : IPoolable
	{
		// Token: 0x0600032E RID: 814 RVA: 0x0000C757 File Offset: 0x0000A957
		public static void empty()
		{
			PoolablePool<T>.pool.empty();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000C763 File Offset: 0x0000A963
		public static void warmup(uint count)
		{
			PoolablePool<T>.pool.warmup(count, new Pool<T>.PoolClaimHandler(PoolablePool<T>.handlePoolClaim));
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000C77C File Offset: 0x0000A97C
		public static T claim()
		{
			T result = PoolablePool<T>.pool.claim(new Pool<T>.PoolClaimHandler(PoolablePool<T>.handlePoolClaim));
			result.poolClaim();
			return result;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000C7AE File Offset: 0x0000A9AE
		public static void release(T poolable)
		{
			PoolablePool<T>.pool.release(poolable, new Pool<T>.PoolReleasedHandler(PoolablePool<T>.handlePoolRelease));
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000C7C7 File Offset: 0x0000A9C7
		private static T handlePoolClaim(Pool<T> pool)
		{
			return Activator.CreateInstance<T>();
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000C7CE File Offset: 0x0000A9CE
		private static void handlePoolRelease(Pool<T> pool, T poolable)
		{
			poolable.poolRelease();
		}

		// Token: 0x0400015A RID: 346
		private static Pool<T> pool = new Pool<T>();
	}
}
