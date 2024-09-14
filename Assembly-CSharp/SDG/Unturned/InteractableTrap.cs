using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000473 RID: 1139
	public class InteractableTrap : Interactable
	{
		// Token: 0x060022F2 RID: 8946 RVA: 0x00089034 File Offset: 0x00087234
		public override void updateState(Asset asset, byte[] state)
		{
			ItemTrapAsset itemTrapAsset = (ItemTrapAsset)asset;
			this.range2 = itemTrapAsset.range2;
			this.playerDamage = itemTrapAsset.playerDamage;
			this.zombieDamage = itemTrapAsset.zombieDamage;
			this.animalDamage = itemTrapAsset.animalDamage;
			this.barricadeDamage = itemTrapAsset.barricadeDamage;
			this.structureDamage = itemTrapAsset.structureDamage;
			this.vehicleDamage = itemTrapAsset.vehicleDamage;
			this.resourceDamage = itemTrapAsset.resourceDamage;
			this.objectDamage = itemTrapAsset.objectDamage;
			this.setupDelay = itemTrapAsset.trapSetupDelay;
			this.cooldown = itemTrapAsset.trapCooldown;
			this.trapDetonationEffectGuid = itemTrapAsset.trapDetonationEffectGuid;
			this.explosion2 = itemTrapAsset.explosion2;
			this.explosionLaunchSpeed = itemTrapAsset.explosionLaunchSpeed;
			this.isBroken = itemTrapAsset.isBroken;
			this.isExplosive = itemTrapAsset.isExplosive;
			if (((ItemTrapAsset)asset).damageTires)
			{
				base.transform.parent.GetOrAddComponent<InteractableTrapDamageTires>();
			}
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x00089126 File Offset: 0x00087326
		public override bool checkInteractable()
		{
			return false;
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x00089129 File Offset: 0x00087329
		private void OnEnable()
		{
			this.lastActive = Time.realtimeSinceStartup;
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x00089138 File Offset: 0x00087338
		private void OnTriggerEnter(Collider other)
		{
			if (other.isTrigger)
			{
				return;
			}
			if (Time.realtimeSinceStartup - this.lastActive < this.setupDelay)
			{
				return;
			}
			if (base.transform.parent != null && other.transform == base.transform.parent)
			{
				return;
			}
			if (Time.realtimeSinceStartup - this.lastTriggered < this.cooldown)
			{
				return;
			}
			this.lastTriggered = Time.realtimeSinceStartup;
			if (Provider.isServer)
			{
				if (this.isExplosive)
				{
					bool flag = !other.transform.CompareTag("Player") || (Provider.isPvP && (other.transform.parent == null || !other.transform.parent.CompareTag("Vehicle"))) || this.explosionLaunchSpeed > 0.01f;
					if (flag)
					{
						Vector3 position = base.transform.position;
						List<EPlayerKill> list;
						DamageTool.explode(new ExplosionParameters(position, this.range2, EDeathCause.LANDMINE, CSteamID.Nil)
						{
							playerDamage = this.playerDamage,
							zombieDamage = this.zombieDamage,
							animalDamage = this.animalDamage,
							barricadeDamage = this.barricadeDamage,
							structureDamage = this.structureDamage,
							vehicleDamage = this.vehicleDamage,
							resourceDamage = this.resourceDamage,
							objectDamage = this.objectDamage,
							damageOrigin = EDamageOrigin.Trap_Explosion,
							launchSpeed = this.explosionLaunchSpeed
						}, out list);
						EffectAsset effectAsset = Assets.FindEffectAssetByGuidOrLegacyId(this.trapDetonationEffectGuid, this.explosion2);
						if (effectAsset != null)
						{
							EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
							{
								position = position,
								relevantDistance = EffectManager.LARGE
							});
							return;
						}
					}
				}
				else if (other.transform.CompareTag("Player"))
				{
					if (Provider.isPvP && (other.transform.parent == null || !other.transform.parent.CompareTag("Vehicle")))
					{
						Player player = DamageTool.getPlayer(other.transform);
						if (player != null)
						{
							EPlayerKill eplayerKill;
							DamageTool.damage(player, EDeathCause.SHRED, ELimb.SPINE, CSteamID.Nil, Vector3.up, this.playerDamage, 1f, out eplayerKill, true, true, ERagdollEffect.NONE);
							if (this.isBroken)
							{
								player.life.breakLegs();
							}
							DamageTool.ServerSpawnLegacyImpact(base.transform.position + Vector3.up, Vector3.down, "Flesh", null, Provider.GatherClientConnectionsWithinSphere(base.transform.position, EffectManager.SMALL));
							Transform parent = base.transform.parent;
							float damage = 5f;
							float times = 1f;
							bool armor = false;
							SteamChannel channel = player.channel;
							CSteamID? csteamID;
							if (channel == null)
							{
								csteamID = default(CSteamID?);
							}
							else
							{
								SteamPlayer owner = channel.owner;
								csteamID = ((owner != null) ? new CSteamID?(owner.playerID.steamID) : default(CSteamID?));
							}
							CSteamID? csteamID2 = csteamID;
							BarricadeManager.damage(parent, damage, times, armor, csteamID2.GetValueOrDefault(), EDamageOrigin.Trap_Wear_And_Tear);
							return;
						}
					}
				}
				else if (other.transform.CompareTag("Agent"))
				{
					Zombie zombie = DamageTool.getZombie(other.transform);
					if (zombie != null)
					{
						EPlayerKill eplayerKill2;
						uint num;
						DamageTool.damageZombie(new DamageZombieParameters(zombie, base.transform.forward, this.zombieDamage)
						{
							instigator = this
						}, out eplayerKill2, out num);
						DamageTool.ServerSpawnLegacyImpact(base.transform.position + Vector3.up, Vector3.down, zombie.isRadioactive ? "Alien" : "Flesh", null, Provider.GatherClientConnectionsWithinSphere(base.transform.position, EffectManager.SMALL));
						BarricadeManager.damage(base.transform.parent, zombie.isHyper ? 10f : 5f, 1f, false, default(CSteamID), EDamageOrigin.Trap_Wear_And_Tear);
						return;
					}
					Animal animal = DamageTool.getAnimal(other.transform);
					if (animal != null)
					{
						EPlayerKill eplayerKill3;
						uint num2;
						DamageTool.damageAnimal(new DamageAnimalParameters(animal, base.transform.forward, this.animalDamage)
						{
							instigator = this
						}, out eplayerKill3, out num2);
						DamageTool.ServerSpawnLegacyImpact(base.transform.position + Vector3.up, Vector3.down, "Flesh", null, Provider.GatherClientConnectionsWithinSphere(base.transform.position, EffectManager.SMALL));
						BarricadeManager.damage(base.transform.parent, 5f, 1f, false, default(CSteamID), EDamageOrigin.Trap_Wear_And_Tear);
					}
				}
			}
		}

		// Token: 0x04001157 RID: 4439
		private float range2;

		// Token: 0x04001158 RID: 4440
		private float playerDamage;

		// Token: 0x04001159 RID: 4441
		private float zombieDamage;

		// Token: 0x0400115A RID: 4442
		private float animalDamage;

		// Token: 0x0400115B RID: 4443
		private float barricadeDamage;

		// Token: 0x0400115C RID: 4444
		private float structureDamage;

		// Token: 0x0400115D RID: 4445
		private float vehicleDamage;

		// Token: 0x0400115E RID: 4446
		private float resourceDamage;

		// Token: 0x0400115F RID: 4447
		private float objectDamage;

		// Token: 0x04001160 RID: 4448
		private float setupDelay = 0.25f;

		// Token: 0x04001161 RID: 4449
		private float cooldown;

		// Token: 0x04001162 RID: 4450
		private float explosionLaunchSpeed;

		// Token: 0x04001163 RID: 4451
		public Guid trapDetonationEffectGuid;

		/// <summary>
		/// Kept because lots of modders have been using this script in Unity,
		/// so removing legacy effect id would break their content.
		/// </summary>
		// Token: 0x04001164 RID: 4452
		private ushort explosion2;

		// Token: 0x04001165 RID: 4453
		private bool isBroken;

		// Token: 0x04001166 RID: 4454
		private bool isExplosive;

		// Token: 0x04001167 RID: 4455
		private float lastActive;

		// Token: 0x04001168 RID: 4456
		private float lastTriggered;
	}
}
