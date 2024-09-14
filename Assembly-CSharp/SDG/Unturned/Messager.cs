using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004FC RID: 1276
	public class Messager : MonoBehaviour
	{
		// Token: 0x0600280E RID: 10254 RVA: 0x000A958F File Offset: 0x000A778F
		private void OnTriggerStay(Collider other)
		{
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x000A9591 File Offset: 0x000A7791
		private void Update()
		{
			if (Time.realtimeSinceStartup - this.lastTrigger < 0.5f)
			{
				PlayerUI.hint(null, this.message);
			}
		}

		// Token: 0x04001538 RID: 5432
		public EPlayerMessage message;

		// Token: 0x04001539 RID: 5433
		private float lastTrigger;
	}
}
