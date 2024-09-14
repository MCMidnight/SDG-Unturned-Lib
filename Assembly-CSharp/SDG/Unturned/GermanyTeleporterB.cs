using System;
using System.Collections;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004C8 RID: 1224
	public class GermanyTeleporterB : GermanyTeleporterA
	{
		// Token: 0x0600257D RID: 9597 RVA: 0x000951AC File Offset: 0x000933AC
		protected override IEnumerator teleport()
		{
			yield return new WaitForSeconds(1f);
			if (this.target != null)
			{
				GermanyTeleporterA.nearbyPlayers.Clear();
				PlayerTool.getPlayersInRadius(base.transform.position, this.sqrRadius, GermanyTeleporterA.nearbyPlayers);
				for (int i = 0; i < GermanyTeleporterA.nearbyPlayers.Count; i++)
				{
					Player player = GermanyTeleporterA.nearbyPlayers[i];
					if (!player.life.isDead && player.quests.getQuestStatus(248) == ENPCQuestStatus.COMPLETED)
					{
						player.teleportToLocationUnsafe(this.target.position, this.target.rotation.eulerAngles.y);
					}
				}
			}
			yield break;
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x000951BC File Offset: 0x000933BC
		private void onPlayerLifeUpdated(Player player)
		{
			if (player == null || player.life.IsAlive)
			{
				return;
			}
			if ((player.transform.position - base.transform.position).sqrMagnitude > this.sqrBossRadius)
			{
				return;
			}
			if (player.quests.getQuestStatus(248) == ENPCQuestStatus.COMPLETED)
			{
				return;
			}
			player.quests.sendRemoveQuest(248);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00095230 File Offset: 0x00093430
		private void onZombieLifeUpdated(Zombie zombie)
		{
			if (!zombie.isDead)
			{
				return;
			}
			if ((zombie.transform.position - base.transform.position).sqrMagnitude > this.sqrBossRadius)
			{
				return;
			}
			GermanyTeleporterA.nearbyPlayers.Clear();
			PlayerTool.getPlayersInRadius(base.transform.position, this.sqrBossRadius, GermanyTeleporterA.nearbyPlayers);
			for (int i = 0; i < GermanyTeleporterA.nearbyPlayers.Count; i++)
			{
				Player player = GermanyTeleporterA.nearbyPlayers[i];
				if (!player.life.isDead)
				{
					player.quests.sendRemoveQuest(248);
					player.quests.sendSetFlag(248, 1);
				}
			}
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x000952E8 File Offset: 0x000934E8
		protected override void OnEnable()
		{
			base.OnEnable();
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
			this.region = ZombieManager.regions[this.navIndex];
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

		// Token: 0x06002581 RID: 9601 RVA: 0x0009538C File Offset: 0x0009358C
		protected override void OnDisable()
		{
			base.OnDisable();
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

		// Token: 0x04001343 RID: 4931
		public float sqrBossRadius;

		// Token: 0x04001344 RID: 4932
		public int navIndex;

		// Token: 0x04001345 RID: 4933
		private ZombieRegion region;

		// Token: 0x04001346 RID: 4934
		private bool isListeningPlayer;

		// Token: 0x04001347 RID: 4935
		private bool isListeningZombie;
	}
}
