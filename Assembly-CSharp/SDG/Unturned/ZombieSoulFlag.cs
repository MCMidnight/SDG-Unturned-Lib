using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000527 RID: 1319
	public class ZombieSoulFlag : MonoBehaviour
	{
		// Token: 0x0600293A RID: 10554 RVA: 0x000AF858 File Offset: 0x000ADA58
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
			ZombieSoulFlag.nearbyPlayers.Clear();
			PlayerTool.getPlayersInRadius(base.transform.position, this.sqrRadius, ZombieSoulFlag.nearbyPlayers);
			for (int i = 0; i < ZombieSoulFlag.nearbyPlayers.Count; i++)
			{
				Player player = ZombieSoulFlag.nearbyPlayers[i];
				short num;
				if (!player.life.isDead && player.quests.getFlag(this.flagPlaced, out num) && num == 1)
				{
					EffectManager.sendEffect(this.collectEffect, player.channel.GetOwnerTransportConnection(), zombie.transform.position + Vector3.up, (base.transform.position - zombie.transform.position + Vector3.up).normalized);
					short num2;
					player.quests.getFlag(this.flagKills, out num2);
					num2 += 1;
					player.quests.sendSetFlag(this.flagKills, num2);
					if (num2 >= (short)this.soulsNeeded)
					{
						EffectManager.sendEffect(this.teleportEffect, player.channel.GetOwnerTransportConnection(), base.transform.position);
						player.quests.sendSetFlag(this.flagPlaced, 2);
					}
				}
			}
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x000AF9D8 File Offset: 0x000ADBD8
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

		// Token: 0x0600293C RID: 10556 RVA: 0x000AFA58 File Offset: 0x000ADC58
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

		// Token: 0x040015F5 RID: 5621
		private static List<Player> nearbyPlayers = new List<Player>();

		// Token: 0x040015F6 RID: 5622
		public ushort flagPlaced;

		// Token: 0x040015F7 RID: 5623
		public ushort flagKills;

		// Token: 0x040015F8 RID: 5624
		public float sqrRadius;

		// Token: 0x040015F9 RID: 5625
		public byte soulsNeeded;

		// Token: 0x040015FA RID: 5626
		public ushort collectEffect;

		// Token: 0x040015FB RID: 5627
		public ushort teleportEffect;

		// Token: 0x040015FC RID: 5628
		private ZombieRegion region;

		// Token: 0x040015FD RID: 5629
		private bool isListening;
	}
}
