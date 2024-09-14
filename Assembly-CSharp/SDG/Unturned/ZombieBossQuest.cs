using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000524 RID: 1316
	public class ZombieBossQuest : MonoBehaviour
	{
		// Token: 0x0600292D RID: 10541 RVA: 0x000AF58C File Offset: 0x000AD78C
		private IEnumerator teleport()
		{
			yield return new WaitForSeconds(3f);
			if (this.target != null)
			{
				ZombieBossQuest.nearbyPlayers.Clear();
				PlayerTool.getPlayersInRadius(base.transform.position, this.sqrRadius, ZombieBossQuest.nearbyPlayers);
				for (int i = 0; i < ZombieBossQuest.nearbyPlayers.Count; i++)
				{
					Player player = ZombieBossQuest.nearbyPlayers[i];
					if (!player.life.isDead)
					{
						player.quests.sendRemoveQuest(213);
						player.quests.setFlag(213, 1);
						player.teleportToLocationUnsafe(this.target.position, this.target.rotation.eulerAngles.y);
					}
				}
			}
			yield break;
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000AF59C File Offset: 0x000AD79C
		private void onPlayerLifeUpdated(Player player)
		{
			if (player == null || player.life.IsAlive)
			{
				return;
			}
			if ((player.transform.position - base.transform.position).sqrMagnitude > this.sqrRadius)
			{
				return;
			}
			player.quests.sendRemoveQuest(213);
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x000AF5FC File Offset: 0x000AD7FC
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
			EffectManager.sendEffect(this.teleportEffect, 16f, zombie.transform.position + Vector3.up);
			base.StartCoroutine("teleport");
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x000AF670 File Offset: 0x000AD870
		private void OnEnable()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (!this.isListeningPlayer)
			{
				PlayerLife.onPlayerLifeUpdated = (PlayerLifeUpdated)Delegate.Combine(PlayerLife.onPlayerLifeUpdated, new PlayerLifeUpdated(this.onPlayerLifeUpdated));
				this.isListeningPlayer = true;
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
			if (!this.isListeningZombie)
			{
				ZombieRegion zombieRegion = this.region;
				zombieRegion.onZombieLifeUpdated = (ZombieLifeUpdated)Delegate.Combine(zombieRegion.onZombieLifeUpdated, new ZombieLifeUpdated(this.onZombieLifeUpdated));
				this.isListeningZombie = true;
			}
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x000AF720 File Offset: 0x000AD920
		private void OnDisable()
		{
			if (this.isListeningPlayer)
			{
				PlayerLife.onPlayerLifeUpdated = (PlayerLifeUpdated)Delegate.Remove(PlayerLife.onPlayerLifeUpdated, new PlayerLifeUpdated(this.onPlayerLifeUpdated));
				this.isListeningPlayer = false;
			}
			if (this.isListeningZombie && this.region != null)
			{
				ZombieRegion zombieRegion = this.region;
				zombieRegion.onZombieLifeUpdated = (ZombieLifeUpdated)Delegate.Remove(zombieRegion.onZombieLifeUpdated, new ZombieLifeUpdated(this.onZombieLifeUpdated));
				this.isListeningZombie = false;
			}
			this.region = null;
		}

		// Token: 0x040015EB RID: 5611
		private static List<Player> nearbyPlayers = new List<Player>();

		// Token: 0x040015EC RID: 5612
		public Transform target;

		// Token: 0x040015ED RID: 5613
		public float sqrRadius;

		// Token: 0x040015EE RID: 5614
		public ushort teleportEffect;

		// Token: 0x040015EF RID: 5615
		private ZombieRegion region;

		// Token: 0x040015F0 RID: 5616
		private bool isListeningPlayer;

		// Token: 0x040015F1 RID: 5617
		private bool isListeningZombie;
	}
}
