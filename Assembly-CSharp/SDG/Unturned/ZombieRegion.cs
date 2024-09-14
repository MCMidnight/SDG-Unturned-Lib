using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005AD RID: 1453
	public class ZombieRegion
	{
		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002F42 RID: 12098 RVA: 0x000CFAC3 File Offset: 0x000CDCC3
		public List<Zombie> zombies
		{
			get
			{
				return this._zombies;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002F43 RID: 12099 RVA: 0x000CFACB File Offset: 0x000CDCCB
		// (set) Token: 0x06002F44 RID: 12100 RVA: 0x000CFAD3 File Offset: 0x000CDCD3
		public byte nav { get; protected set; }

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06002F45 RID: 12101 RVA: 0x000CFADC File Offset: 0x000CDCDC
		// (set) Token: 0x06002F46 RID: 12102 RVA: 0x000CFAE4 File Offset: 0x000CDCE4
		public bool hasBeacon
		{
			get
			{
				return this._hasBeacon;
			}
			set
			{
				if (value != this._hasBeacon)
				{
					this._hasBeacon = value;
					HyperUpdated hyperUpdated = this.onHyperUpdated;
					if (hyperUpdated == null)
					{
						return;
					}
					hyperUpdated(this.isHyper);
				}
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002F47 RID: 12103 RVA: 0x000CFB0C File Offset: 0x000CDD0C
		public bool isHyper
		{
			get
			{
				return LightingManager.isFullMoon || this.hasBeacon;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002F48 RID: 12104 RVA: 0x000CFB1D File Offset: 0x000CDD1D
		public bool HasInfiniteAgroRange
		{
			get
			{
				return this.hasBeacon || (this.flagData != null && this.flagData.hyperAgro);
			}
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000CFB3E File Offset: 0x000CDD3E
		public int GetAliveBossZombieCount()
		{
			return this.aliveBossZombieCount;
		}

		/// <summary>
		/// Allow another quest to spawn a boss zombie immediately.
		/// </summary>
		// Token: 0x06002F4A RID: 12106 RVA: 0x000CFB46 File Offset: 0x000CDD46
		public void resetQuestBossTimer()
		{
			this.lastBossTime = -1f;
		}

		/// <summary>
		/// Kills the boss zombie if nobody is around, if the boss was killed it calls UpdateBoss.
		/// </summary>
		// Token: 0x06002F4B RID: 12107 RVA: 0x000CFB54 File Offset: 0x000CDD54
		public void UpdateRegion()
		{
			if (this.bossZombie == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead)
				{
					if (steamPlayer.player.movement.bound == this.nav)
					{
						flag = true;
					}
					if (steamPlayer.player.movement.nav == this.nav)
					{
						flag2 = true;
					}
					if (flag && flag2)
					{
						break;
					}
				}
			}
			if (flag)
			{
				if (this.bossZombie.isDead)
				{
					this.bossZombie = null;
					if (flag2)
					{
						this.UpdateBoss();
						return;
					}
				}
			}
			else
			{
				EPlayerKill eplayerKill;
				uint num;
				this.bossZombie.askDamage(50000, Vector3.up, out eplayerKill, out num, false, false, EZombieStunOverride.None, ERagdollEffect.NONE);
				this.resetQuestBossTimer();
			}
		}

		/// <summary>
		/// Checks for players in the area with quests and spawns boss zombies accordingly.
		/// </summary>
		// Token: 0x06002F4C RID: 12108 RVA: 0x000CFC60 File Offset: 0x000CDE60
		public void UpdateBoss()
		{
			if (this.bossZombie != null)
			{
				return;
			}
			bool flag = this.lastBossTime < 0f || Time.time - this.lastBossTime > Provider.modeConfigData.Zombies.Quest_Boss_Respawn_Interval;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead && steamPlayer.player.movement.nav == this.nav)
				{
					for (int j = 0; j < steamPlayer.player.quests.questsList.Count; j++)
					{
						PlayerQuest playerQuest = steamPlayer.player.quests.questsList[j];
						if (playerQuest != null && playerQuest.asset != null)
						{
							for (int k = 0; k < playerQuest.asset.conditions.Length; k++)
							{
								NPCZombieKillsCondition npczombieKillsCondition = playerQuest.asset.conditions[k] as NPCZombieKillsCondition;
								if (npczombieKillsCondition != null && npczombieKillsCondition.nav == this.nav && npczombieKillsCondition.spawn && !npczombieKillsCondition.isConditionMet(steamPlayer.player))
								{
									bool usesBossInterval = npczombieKillsCondition.usesBossInterval;
									if (!usesBossInterval || flag)
									{
										int num = Mathf.Min(this.zombies.Count, npczombieKillsCondition.spawnQuantity);
										int num2 = 0;
										foreach (Zombie zombie in this.zombies)
										{
											if (zombie != null && !zombie.isDead && zombie.speciality == npczombieKillsCondition.zombie)
											{
												num2++;
											}
										}
										int num3 = LevelZombies.FindTableIndexByUniqueId(npczombieKillsCondition.LevelTableUniqueId);
										ZombieTable zombieTable = (num3 >= 0) ? LevelZombies.tables[num3] : null;
										int l;
										for (l = num2; l < num; l++)
										{
											Zombie zombie2 = null;
											for (int m = 0; m < this.zombies.Count; m++)
											{
												Zombie zombie3 = this.zombies[m];
												if (zombie3 != null && zombie3.isDead)
												{
													zombie2 = zombie3;
													break;
												}
											}
											if (zombie2 == null)
											{
												for (int n = 0; n < this.zombies.Count; n++)
												{
													Zombie zombie4 = this.zombies[n];
													if (zombie4 != null && !zombie4.isDead && zombie4.speciality != npczombieKillsCondition.zombie && !zombie4.isHunting)
													{
														zombie2 = zombie4;
														break;
													}
												}
											}
											if (zombie2 == null)
											{
												for (int num4 = 0; num4 < this.zombies.Count; num4++)
												{
													Zombie zombie5 = this.zombies[num4];
													if (zombie5 != null && !zombie5.isDead && zombie5.speciality != npczombieKillsCondition.zombie)
													{
														zombie2 = zombie5;
														break;
													}
												}
											}
											if (zombie2 != null)
											{
												Vector3 position = zombie2.transform.position;
												if (zombie2.isDead)
												{
													for (int num5 = 0; num5 < 10; num5++)
													{
														ZombieSpawnpoint zombieSpawnpoint = LevelZombies.zombies[(int)this.nav][Random.Range(0, LevelZombies.zombies[(int)this.nav].Count)];
														if (SafezoneManager.checkPointValid(zombieSpawnpoint.point))
														{
															break;
														}
														position = zombieSpawnpoint.point;
														position.y += 0.1f;
													}
												}
												byte type = zombie2.type;
												byte shirt = zombie2.shirt;
												byte pants = zombie2.pants;
												byte hat = zombie2.hat;
												byte gear = zombie2.gear;
												if (zombieTable != null)
												{
													type = (byte)num3;
													zombieTable.GetSpawnClothingParameters(out shirt, out pants, out hat, out gear);
												}
												zombie2.sendRevive(type, (byte)npczombieKillsCondition.zombie, shirt, pants, hat, gear, position, Random.Range(0f, 360f));
												if (usesBossInterval)
												{
													this.bossZombie = zombie2;
												}
											}
										}
										string[] array = new string[12];
										array[0] = "Spawned ";
										array[1] = l.ToString();
										array[2] = " ";
										array[3] = npczombieKillsCondition.zombie.ToString();
										array[4] = " zombies in nav ";
										array[5] = this.nav.ToString();
										array[6] = " for quest ";
										array[7] = playerQuest.id.ToString();
										array[8] = ", isBoss ";
										array[9] = usesBossInterval.ToString();
										array[10] = " boss = ";
										int num6 = 11;
										Zombie zombie6 = this.bossZombie;
										array[num6] = ((zombie6 != null) ? zombie6.ToString() : null);
										UnturnedLog.info(string.Concat(array));
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x000D0188 File Offset: 0x000CE388
		private void onMoonUpdated(bool isFullMoon)
		{
			HyperUpdated hyperUpdated = this.onHyperUpdated;
			if (hyperUpdated == null)
			{
				return;
			}
			hyperUpdated(this.isHyper);
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x000D01A0 File Offset: 0x000CE3A0
		public void destroy()
		{
			ushort num = 0;
			while ((int)num < this.zombies.Count)
			{
				Object.Destroy(this.zombies[(int)num].gameObject);
				num += 1;
			}
			this.zombies.Clear();
			this.hasMega = false;
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x000D01EC File Offset: 0x000CE3EC
		public void init()
		{
			LightingManager.onMoonUpdated = (MoonUpdated)Delegate.Combine(LightingManager.onMoonUpdated, new MoonUpdated(this.onMoonUpdated));
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x000D0210 File Offset: 0x000CE410
		public ZombieRegion(byte newNav)
		{
			this._zombies = new List<Zombie>();
			this.nav = newNav;
			if ((int)this.nav < LevelNavigation.flagData.Count)
			{
				this.flagData = LevelNavigation.flagData[(int)this.nav];
			}
			this.updates = 0;
			this.respawnZombieIndex = 0;
			this.alive = 0;
			this.isNetworked = false;
			this.lastMega = -1000f;
			this.hasMega = false;
		}

		// Token: 0x0400195A RID: 6490
		public HyperUpdated onHyperUpdated;

		// Token: 0x0400195B RID: 6491
		public ZombieLifeUpdated onZombieLifeUpdated;

		// Token: 0x0400195C RID: 6492
		private List<Zombie> _zombies;

		// Token: 0x0400195E RID: 6494
		public FlagData flagData;

		// Token: 0x0400195F RID: 6495
		public ushort updates;

		// Token: 0x04001960 RID: 6496
		public ushort respawnZombieIndex;

		/// <summary>
		/// Number of alive zombies.
		/// </summary>
		// Token: 0x04001961 RID: 6497
		public int alive;

		// Token: 0x04001962 RID: 6498
		public bool isNetworked;

		// Token: 0x04001963 RID: 6499
		public float lastMega;

		// Token: 0x04001964 RID: 6500
		public bool hasMega;

		// Token: 0x04001965 RID: 6501
		private bool _hasBeacon;

		// Token: 0x04001966 RID: 6502
		public bool isRadioactive;

		// Token: 0x04001967 RID: 6503
		private Zombie bossZombie;

		/// <summary>
		/// Last time a quest boss was spawned.
		/// </summary>
		// Token: 0x04001968 RID: 6504
		private float lastBossTime = -1f;

		// Token: 0x04001969 RID: 6505
		internal int aliveBossZombieCount;
	}
}
