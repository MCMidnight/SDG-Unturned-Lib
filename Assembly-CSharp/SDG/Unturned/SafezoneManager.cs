using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200058E RID: 1422
	public class SafezoneManager : MonoBehaviour
	{
		// Token: 0x06002D7B RID: 11643 RVA: 0x000C6514 File Offset: 0x000C4714
		public static bool checkPointValid(Vector3 point)
		{
			for (int i = 0; i < SafezoneManager.bubbles.Count; i++)
			{
				SafezoneBubble safezoneBubble = SafezoneManager.bubbles[i];
				if ((safezoneBubble.origin - point).sqrMagnitude < safezoneBubble.sqrRadius)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000C6564 File Offset: 0x000C4764
		public static SafezoneBubble registerBubble(Vector3 origin, float radius)
		{
			SafezoneBubble safezoneBubble = new SafezoneBubble(origin, radius * radius);
			SafezoneManager.bubbles.Add(safezoneBubble);
			return safezoneBubble;
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x000C6587 File Offset: 0x000C4787
		public static void deregisterBubble(SafezoneBubble bubble)
		{
			SafezoneManager.bubbles.Remove(bubble);
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x000C6595 File Offset: 0x000C4795
		private void onLevelLoaded(int level)
		{
			SafezoneManager.bubbles = new List<SafezoneBubble>();
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x000C65A1 File Offset: 0x000C47A1
		private void Start()
		{
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
		}

		// Token: 0x0400188A RID: 6282
		private static List<SafezoneBubble> bubbles;
	}
}
