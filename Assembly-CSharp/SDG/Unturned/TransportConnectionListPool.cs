using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Pool lists to avoid loopback re-using an existing list.
	/// Callers do not need to manually return lists because they are reset before each frame.
	/// </summary>
	// Token: 0x02000264 RID: 612
	internal static class TransportConnectionListPool
	{
		// Token: 0x0600125E RID: 4702 RVA: 0x0003EE04 File Offset: 0x0003D004
		public static PooledTransportConnectionList Get()
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			PooledTransportConnectionList pooledTransportConnectionList;
			if (TransportConnectionListPool.available.Count > 0)
			{
				pooledTransportConnectionList = TransportConnectionListPool.available.GetAndRemoveTail<PooledTransportConnectionList>();
				if (pooledTransportConnectionList.Count > 0)
				{
					pooledTransportConnectionList.Clear();
					int frameCount = Time.frameCount;
					if (frameCount != TransportConnectionListPool.lastWarningFrameNumber)
					{
						TransportConnectionListPool.lastWarningFrameNumber = frameCount;
						UnturnedLog.warn("PooledConnectionList was used after end of frame! Plugins should not hold onto these lists.");
					}
				}
			}
			else
			{
				pooledTransportConnectionList = new PooledTransportConnectionList((int)Provider.maxPlayers);
			}
			TransportConnectionListPool.claimed.Add(pooledTransportConnectionList);
			return pooledTransportConnectionList;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0003EE74 File Offset: 0x0003D074
		public static void ReleaseAll()
		{
			foreach (PooledTransportConnectionList pooledTransportConnectionList in TransportConnectionListPool.claimed)
			{
				pooledTransportConnectionList.Clear();
				TransportConnectionListPool.available.Add(pooledTransportConnectionList);
			}
			TransportConnectionListPool.claimed.Clear();
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0003EEDC File Offset: 0x0003D0DC
		static TransportConnectionListPool()
		{
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(TransportConnectionListPool.OnLogMemoryUsage));
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x0003EF18 File Offset: 0x0003D118
		private static void OnLogMemoryUsage(List<string> results)
		{
			results.Add(string.Format("Transport connection list pool size: {0}", TransportConnectionListPool.available.Count));
			results.Add(string.Format("Transport connection list active count: {0}", TransportConnectionListPool.claimed.Count));
		}

		// Token: 0x040005BA RID: 1466
		private static List<PooledTransportConnectionList> available = new List<PooledTransportConnectionList>();

		// Token: 0x040005BB RID: 1467
		private static List<PooledTransportConnectionList> claimed = new List<PooledTransportConnectionList>();

		// Token: 0x040005BC RID: 1468
		private static int lastWarningFrameNumber = -1;
	}
}
