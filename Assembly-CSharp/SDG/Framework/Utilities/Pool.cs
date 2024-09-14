using System;
using System.Collections.Generic;

namespace SDG.Framework.Utilities
{
	// Token: 0x02000082 RID: 130
	public class Pool<T>
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600031E RID: 798 RVA: 0x0000C554 File Offset: 0x0000A754
		// (remove) Token: 0x0600031F RID: 799 RVA: 0x0000C58C File Offset: 0x0000A78C
		public event Pool<T>.PoolClaimedHandler claimed;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000320 RID: 800 RVA: 0x0000C5C4 File Offset: 0x0000A7C4
		// (remove) Token: 0x06000321 RID: 801 RVA: 0x0000C5FC File Offset: 0x0000A7FC
		public event Pool<T>.PoolReleasedHandler released;

		/// <summary>
		/// Number of items in underlying queue.
		/// </summary>
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000C631 File Offset: 0x0000A831
		public int count
		{
			get
			{
				return this.pool.Count;
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000C63E File Offset: 0x0000A83E
		public void empty()
		{
			this.pool.Clear();
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000C64B File Offset: 0x0000A84B
		public void warmup(uint count)
		{
			this.warmup(count, null);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000C658 File Offset: 0x0000A858
		public void warmup(uint count, Pool<T>.PoolClaimHandler callback)
		{
			if (callback == null)
			{
				callback = new Pool<T>.PoolClaimHandler(this.handleClaim);
			}
			for (uint num = 0U; num < count; num += 1U)
			{
				T item = callback(this);
				this.release(item);
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000C691 File Offset: 0x0000A891
		public T claim()
		{
			return this.claim(null);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000C69C File Offset: 0x0000A89C
		public T claim(Pool<T>.PoolClaimHandler callback)
		{
			T t;
			if (this.pool.Count > 0)
			{
				t = this.pool.Dequeue();
			}
			else if (callback != null)
			{
				t = callback(this);
			}
			else
			{
				t = this.handleClaim(this);
			}
			this.triggerClaimed(t);
			return t;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000C6E2 File Offset: 0x0000A8E2
		public void release(T item)
		{
			this.release(item, null);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000C6EC File Offset: 0x0000A8EC
		public void release(T item, Pool<T>.PoolReleasedHandler callback)
		{
			if (item == null)
			{
				return;
			}
			if (callback != null)
			{
				callback(this, item);
			}
			this.triggerReleased(item);
			this.pool.Enqueue(item);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000C715 File Offset: 0x0000A915
		protected T handleClaim(Pool<T> pool)
		{
			return Activator.CreateInstance<T>();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000C71C File Offset: 0x0000A91C
		protected void triggerClaimed(T item)
		{
			Pool<T>.PoolClaimedHandler poolClaimedHandler = this.claimed;
			if (poolClaimedHandler == null)
			{
				return;
			}
			poolClaimedHandler(this, item);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000C730 File Offset: 0x0000A930
		protected void triggerReleased(T item)
		{
			Pool<T>.PoolReleasedHandler poolReleasedHandler = this.released;
			if (poolReleasedHandler == null)
			{
				return;
			}
			poolReleasedHandler(this, item);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000C744 File Offset: 0x0000A944
		public Pool()
		{
			this.pool = new Queue<T>();
		}

		// Token: 0x04000159 RID: 345
		protected Queue<T> pool;

		// Token: 0x02000853 RID: 2131
		// (Invoke) Token: 0x060047C2 RID: 18370
		public delegate T PoolClaimHandler(Pool<T> pool);

		// Token: 0x02000854 RID: 2132
		// (Invoke) Token: 0x060047C6 RID: 18374
		public delegate void PoolReleaseHandler(Pool<T> pool, T item);

		// Token: 0x02000855 RID: 2133
		// (Invoke) Token: 0x060047CA RID: 18378
		public delegate void PoolClaimedHandler(Pool<T> pool, T item);

		// Token: 0x02000856 RID: 2134
		// (Invoke) Token: 0x060047CE RID: 18382
		public delegate void PoolReleasedHandler(Pool<T> pool, T item);
	}
}
