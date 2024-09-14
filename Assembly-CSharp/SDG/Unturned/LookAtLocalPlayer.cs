using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005CF RID: 1487
	public class LookAtLocalPlayer : MonoBehaviour
	{
		// Token: 0x06002FFD RID: 12285 RVA: 0x000D404D File Offset: 0x000D224D
		private void LateUpdate()
		{
			if (Player.player != null)
			{
				base.transform.LookAt(Player.player.look.aim);
			}
		}
	}
}
