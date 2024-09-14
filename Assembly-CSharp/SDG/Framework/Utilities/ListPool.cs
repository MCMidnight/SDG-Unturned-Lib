using System;
using System.Collections.Generic;

namespace SDG.Framework.Utilities
{
	// Token: 0x0200007E RID: 126
	public static class ListPool<T>
	{
		// Token: 0x0600030D RID: 781 RVA: 0x0000C17A File Offset: 0x0000A37A
		public static void empty()
		{
			ListPool<T>.pool.empty();
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000C186 File Offset: 0x0000A386
		public static void warmup(uint count)
		{
			ListPool<T>.pool.warmup(count, new Pool<List<T>>.PoolClaimHandler(ListPool<T>.handlePoolClaim));
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000C19F File Offset: 0x0000A39F
		public static List<T> claim()
		{
			return ListPool<T>.pool.claim(new Pool<List<T>>.PoolClaimHandler(ListPool<T>.handlePoolClaim));
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000C1B7 File Offset: 0x0000A3B7
		public static void release(List<T> list)
		{
			ListPool<T>.pool.release(list, new Pool<List<T>>.PoolReleasedHandler(ListPool<T>.handlePoolRelease));
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000C1D0 File Offset: 0x0000A3D0
		private static List<T> handlePoolClaim(Pool<List<T>> pool)
		{
			return new List<T>();
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000C1D7 File Offset: 0x0000A3D7
		private static void handlePoolRelease(Pool<List<T>> pool, List<T> list)
		{
			list.Clear();
		}

		// Token: 0x04000154 RID: 340
		private static Pool<List<T>> pool = new Pool<List<T>>();
	}
}
