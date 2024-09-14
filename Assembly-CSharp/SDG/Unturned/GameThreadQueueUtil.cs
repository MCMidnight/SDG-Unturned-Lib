using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200081F RID: 2079
	internal class GameThreadQueueUtil : MonoBehaviour
	{
		// Token: 0x060046F8 RID: 18168 RVA: 0x001A7F31 File Offset: 0x001A6131
		internal static void Setup()
		{
			GameObject gameObject = new GameObject("ThreadUtil");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			GameThreadQueueUtil.instance = gameObject.AddComponent<GameThreadQueueUtil>();
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x001A7F58 File Offset: 0x001A6158
		internal static void QueueGameThreadWorkItem(WaitCallback callback, object state)
		{
			GameThreadQueueUtil.instance.workItems.Enqueue(new GameThreadQueueUtil.WorkItem
			{
				callback = callback,
				state = state
			});
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x001A7F90 File Offset: 0x001A6190
		private void Update()
		{
			GameThreadQueueUtil.WorkItem workItem;
			if (this.workItems.TryDequeue(ref workItem) && workItem.callback != null)
			{
				workItem.callback.Invoke(workItem.state);
			}
		}

		// Token: 0x04003048 RID: 12360
		private ConcurrentQueue<GameThreadQueueUtil.WorkItem> workItems = new ConcurrentQueue<GameThreadQueueUtil.WorkItem>();

		// Token: 0x04003049 RID: 12361
		private static GameThreadQueueUtil instance;

		// Token: 0x02000A2A RID: 2602
		private struct WorkItem
		{
			// Token: 0x0400354C RID: 13644
			public WaitCallback callback;

			// Token: 0x0400354D RID: 13645
			public object state;
		}
	}
}
