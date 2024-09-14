using System;
using System.Collections;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200051D RID: 1309
	public class VolumeTeleporter : MonoBehaviour
	{
		// Token: 0x060028FF RID: 10495 RVA: 0x000AEF63 File Offset: 0x000AD163
		private IEnumerator teleport()
		{
			yield return new WaitForSeconds(3f);
			if (this.target != null && this.playerTeleported != null && this.playerTeleported.life.IsAlive)
			{
				this.playerTeleported.teleportToLocation(this.target.position, this.target.rotation.eulerAngles.y);
				if (this.playerTeleported.equipment.HasValidUseable)
				{
					this.playerTeleported.equipment.dequip();
				}
				this.playerTeleported.equipment.canEquip = true;
			}
			this.playerTeleported = null;
			yield break;
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x000AEF74 File Offset: 0x000AD174
		private void OnTriggerEnter(Collider other)
		{
			if (Provider.isServer && other.transform.CompareTag("Player") && this.playerTeleported == null)
			{
				EffectManager.sendEffect(this.teleportEffect, 16f, this.effectHook.position);
				this.playerTeleported = DamageTool.getPlayer(other.transform);
				base.StartCoroutine("teleport");
			}
		}

		// Token: 0x040015C9 RID: 5577
		public string achievement;

		// Token: 0x040015CA RID: 5578
		public Transform target;

		// Token: 0x040015CB RID: 5579
		public ushort teleportEffect;

		// Token: 0x040015CC RID: 5580
		public Transform effectHook;

		// Token: 0x040015CD RID: 5581
		private Player playerTeleported;
	}
}
