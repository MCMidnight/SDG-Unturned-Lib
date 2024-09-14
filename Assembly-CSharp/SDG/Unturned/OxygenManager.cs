using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000584 RID: 1412
	public class OxygenManager : MonoBehaviour
	{
		// Token: 0x06002D3B RID: 11579 RVA: 0x000C4DD4 File Offset: 0x000C2FD4
		public static bool checkPointBreathable(Vector3 point)
		{
			for (int i = 0; i < OxygenManager.bubbles.Count; i++)
			{
				OxygenBubble oxygenBubble = OxygenManager.bubbles[i];
				if (!(oxygenBubble.origin == null) && (oxygenBubble.origin.position - point).sqrMagnitude < oxygenBubble.sqrRadius)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000C4E34 File Offset: 0x000C3034
		public static OxygenBubble registerBubble(Transform origin, float radius)
		{
			OxygenBubble oxygenBubble = new OxygenBubble(origin, radius * radius);
			OxygenManager.bubbles.Add(oxygenBubble);
			return oxygenBubble;
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000C4E57 File Offset: 0x000C3057
		public static void deregisterBubble(OxygenBubble bubble)
		{
			OxygenManager.bubbles.Remove(bubble);
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000C4E65 File Offset: 0x000C3065
		private void onLevelLoaded(int level)
		{
			OxygenManager.bubbles = new List<OxygenBubble>();
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x000C4E71 File Offset: 0x000C3071
		private void Start()
		{
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
		}

		// Token: 0x04001868 RID: 6248
		private static List<OxygenBubble> bubbles;
	}
}
