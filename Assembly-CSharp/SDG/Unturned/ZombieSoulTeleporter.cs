using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000528 RID: 1320
	public class ZombieSoulTeleporter : MonoBehaviour
	{
		// Token: 0x0600293F RID: 10559 RVA: 0x000AFABE File Offset: 0x000ADCBE
		private IEnumerator teleport()
		{
			yield return new WaitForSeconds(3f);
			if (this.target != null)
			{
				ZombieSoulTeleporter.nearbyPlayers.Clear();
				PlayerTool.getPlayersInRadius(base.transform.position, this.sqrRadius, ZombieSoulTeleporter.nearbyPlayers);
				for (int i = 0; i < ZombieSoulTeleporter.nearbyPlayers.Count; i++)
				{
					Player player = ZombieSoulTeleporter.nearbyPlayers[i];
					if (!player.life.isDead)
					{
						short num;
						short num2;
						if (player.quests.getFlag(211, out num) && num == 1 && player.quests.getFlag(212, out num2) && num2 == 1 && player.quests.getQuestStatus(213) != ENPCQuestStatus.COMPLETED)
						{
							player.quests.sendSetFlag(214, 0);
							player.quests.sendAddQuest(213);
							player.teleportToLocationUnsafe(this.targetBoss.position, this.targetBoss.rotation.eulerAngles.y);
						}
						else
						{
							player.teleportToLocationUnsafe(this.target.position, this.target.rotation.eulerAngles.y);
							if (player.equipment.HasValidUseable)
							{
								player.equipment.dequip();
							}
							player.equipment.canEquip = false;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x000AFAD0 File Offset: 0x000ADCD0
		private void onZombieLifeUpdated(Zombie zombie)
		{
			if (!zombie.isDead)
			{
				return;
			}
			if ((zombie.transform.position - base.transform.position).sqrMagnitude > this.sqrRadius)
			{
				return;
			}
			EffectManager.sendEffect(this.collectEffect, 16f, zombie.transform.position + Vector3.up, (base.transform.position - zombie.transform.position + Vector3.up).normalized);
			this.soulsCollected += 1;
			if (this.soulsCollected >= this.soulsNeeded)
			{
				EffectManager.sendEffect(this.teleportEffect, 16f, base.transform.position);
				this.soulsCollected = 0;
				base.StartCoroutine("teleport");
			}
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x000AFBB0 File Offset: 0x000ADDB0
		private void OnEnable()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (this.region != null)
			{
				return;
			}
			byte b;
			if (LevelNavigation.tryGetBounds(base.transform.position, out b))
			{
				this.region = ZombieManager.regions[(int)b];
			}
			if (this.region == null)
			{
				return;
			}
			if (!this.isListening)
			{
				ZombieRegion zombieRegion = this.region;
				zombieRegion.onZombieLifeUpdated = (ZombieLifeUpdated)Delegate.Combine(zombieRegion.onZombieLifeUpdated, new ZombieLifeUpdated(this.onZombieLifeUpdated));
				this.isListening = true;
			}
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x000AFC30 File Offset: 0x000ADE30
		private void OnDisable()
		{
			if (this.isListening && this.region != null)
			{
				ZombieRegion zombieRegion = this.region;
				zombieRegion.onZombieLifeUpdated = (ZombieLifeUpdated)Delegate.Remove(zombieRegion.onZombieLifeUpdated, new ZombieLifeUpdated(this.onZombieLifeUpdated));
				this.isListening = false;
			}
			this.region = null;
		}

		// Token: 0x040015FE RID: 5630
		private static List<Player> nearbyPlayers = new List<Player>();

		// Token: 0x040015FF RID: 5631
		public Transform target;

		// Token: 0x04001600 RID: 5632
		public Transform targetBoss;

		// Token: 0x04001601 RID: 5633
		public float sqrRadius;

		// Token: 0x04001602 RID: 5634
		public byte soulsNeeded;

		// Token: 0x04001603 RID: 5635
		public ushort collectEffect;

		// Token: 0x04001604 RID: 5636
		public ushort teleportEffect;

		// Token: 0x04001605 RID: 5637
		private ZombieRegion region;

		// Token: 0x04001606 RID: 5638
		private byte soulsCollected;

		// Token: 0x04001607 RID: 5639
		private bool isListening;
	}
}
