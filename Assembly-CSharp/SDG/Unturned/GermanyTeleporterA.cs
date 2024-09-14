using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004C7 RID: 1223
	public class GermanyTeleporterA : MonoBehaviour
	{
		// Token: 0x06002577 RID: 9591 RVA: 0x000950D6 File Offset: 0x000932D6
		protected virtual IEnumerator teleport()
		{
			yield return new WaitForSeconds(1f);
			if (this.target != null)
			{
				GermanyTeleporterA.nearbyPlayers.Clear();
				PlayerTool.getPlayersInRadius(base.transform.position, this.sqrRadius, GermanyTeleporterA.nearbyPlayers);
				for (int i = 0; i < GermanyTeleporterA.nearbyPlayers.Count; i++)
				{
					Player player = GermanyTeleporterA.nearbyPlayers[i];
					if (!player.life.isDead)
					{
						if (player.quests.getQuestStatus(248) != ENPCQuestStatus.COMPLETED)
						{
							player.quests.sendAddQuest(248);
						}
						player.teleportToLocationUnsafe(this.target.position, this.target.rotation.eulerAngles.y);
					}
				}
			}
			yield break;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000950E8 File Offset: 0x000932E8
		protected virtual void handleEventTriggered(Player player, string id)
		{
			if (id != this.eventID)
			{
				return;
			}
			if (Time.realtimeSinceStartup - this.lastTeleport < 5f)
			{
				return;
			}
			this.lastTeleport = Time.realtimeSinceStartup;
			EffectManager.sendEffect(this.teleportEffect, 16f, base.transform.position);
			base.StartCoroutine("teleport");
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x0009514A File Offset: 0x0009334A
		protected virtual void OnEnable()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (!this.isListening)
			{
				NPCEventManager.onEvent += this.handleEventTriggered;
				this.isListening = true;
			}
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x00095175 File Offset: 0x00093375
		protected virtual void OnDisable()
		{
			if (this.isListening)
			{
				NPCEventManager.onEvent -= this.handleEventTriggered;
				this.isListening = false;
			}
		}

		// Token: 0x0400133C RID: 4924
		protected static List<Player> nearbyPlayers = new List<Player>();

		// Token: 0x0400133D RID: 4925
		public Transform target;

		// Token: 0x0400133E RID: 4926
		public float sqrRadius;

		// Token: 0x0400133F RID: 4927
		public string eventID;

		// Token: 0x04001340 RID: 4928
		public ushort teleportEffect;

		// Token: 0x04001341 RID: 4929
		private float lastTeleport;

		// Token: 0x04001342 RID: 4930
		private bool isListening;
	}
}
