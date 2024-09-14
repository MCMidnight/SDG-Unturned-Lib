using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000483 RID: 1155
	public class TemperatureTrigger : MonoBehaviour
	{
		// Token: 0x0600241D RID: 9245 RVA: 0x00090036 File Offset: 0x0008E236
		private void OnEnable()
		{
			if (this.bubble != null)
			{
				return;
			}
			this.bubble = TemperatureManager.registerBubble(base.transform, base.transform.localScale.x, this.temperature);
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x00090068 File Offset: 0x0008E268
		private void OnDisable()
		{
			if (this.bubble == null)
			{
				return;
			}
			TemperatureManager.deregisterBubble(this.bubble);
			this.bubble = null;
		}

		// Token: 0x04001221 RID: 4641
		public EPlayerTemperature temperature;

		// Token: 0x04001222 RID: 4642
		private TemperatureBubble bubble;
	}
}
