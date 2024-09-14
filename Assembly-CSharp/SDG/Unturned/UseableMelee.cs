using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007E8 RID: 2024
	public class UseableMelee : Useable
	{
		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06004573 RID: 17779 RVA: 0x0019D75C File Offset: 0x0019B95C
		public ItemMeleeAsset equippedMeleeAsset
		{
			get
			{
				return base.player.equipment.asset as ItemMeleeAsset;
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06004574 RID: 17780 RVA: 0x0019D774 File Offset: 0x0019B974
		private bool isUseable
		{
			get
			{
				if (this.swingMode == ESwingMode.WEAK)
				{
					return base.player.input.simulation - this.startedUse > this.weakAttackAnimLengthFrames;
				}
				return this.swingMode == ESwingMode.STRONG && base.player.input.simulation - this.startedUse > this.strongAttackAnimLengthFrames;
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06004575 RID: 17781 RVA: 0x0019D7D4 File Offset: 0x0019B9D4
		private bool isDamageable
		{
			get
			{
				if (this.swingMode == ESwingMode.WEAK)
				{
					return base.player.input.simulation - this.startedUse > this.weakAttackAnimLengthFrames * this.equippedMeleeAsset.weak;
				}
				return this.swingMode == ESwingMode.STRONG && base.player.input.simulation - this.startedUse > this.strongAttackAnimLengthFrames * this.equippedMeleeAsset.strong;
			}
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x0019D854 File Offset: 0x0019BA54
		private void swing()
		{
			this.startedUse = base.player.input.simulation;
			this.startedSwing = Time.realtimeSinceStartup;
			this.isUsing = true;
			this.isSwinging = true;
			if (this.swingMode == ESwingMode.WEAK)
			{
				base.player.animator.play("Weak", false);
				this.playUseSoundTime = Time.timeAsDouble + (double)(this.weakAttackAnimLengthSeconds * this.equippedMeleeAsset.weak);
				return;
			}
			if (this.swingMode == ESwingMode.STRONG)
			{
				base.player.animator.play("Strong", false);
				this.playUseSoundTime = Time.timeAsDouble + (double)(this.strongAttackAnimLengthSeconds * this.equippedMeleeAsset.strong);
			}
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x0019D90C File Offset: 0x0019BB0C
		private void startSwing()
		{
			this.startedUse = base.player.input.simulation;
			this.startedSwing = Time.realtimeSinceStartup;
			this.isUsing = true;
			this.isSwinging = true;
			base.player.animator.play("Start_Swing", false);
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x0019D95E File Offset: 0x0019BB5E
		private void stopSwing()
		{
			this.isUsing = false;
			this.isSwinging = false;
			base.player.animator.play("Stop_Swing", false);
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x0019D984 File Offset: 0x0019BB84
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveSpawnMeleeImpact(Vector3 position, Vector3 normal, string materialName, Transform colliderTransform)
		{
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x0019D986 File Offset: 0x0019BB86
		internal void ServerSpawnMeleeImpact(Vector3 position, Vector3 normal, string materialName, Transform colliderTransform, List<ITransportConnection> transportConnections)
		{
			position += normal * Random.Range(0.04f, 0.06f);
			UseableMelee.SendSpawnMeleeImpact.Invoke(base.GetNetId(), ENetReliability.Unreliable, transportConnections, position, normal, materialName, colliderTransform);
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x0019D9BD File Offset: 0x0019BBBD
		[Obsolete]
		public void askInteractMelee(CSteamID steamID)
		{
			this.ReceiveInteractMelee();
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x0019D9C8 File Offset: 0x0019BBC8
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10, legacyName = "askInteractMelee")]
		public void ReceiveInteractMelee()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (base.player.equipment.asset == null)
			{
				return;
			}
			if (!this.equippedMeleeAsset.isLight)
			{
				return;
			}
			this.interact = !this.interact;
			base.player.equipment.state[0] = (this.interact ? 1 : 0);
			base.player.equipment.sendUpdateState();
			EffectManager.TriggerFiremodeEffect(base.transform.position);
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x0019DA57 File Offset: 0x0019BC57
		[Obsolete]
		public void askSwingStart(CSteamID steamID)
		{
			this.ReceivePlaySwingStart();
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x0019DA5F File Offset: 0x0019BC5F
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askSwingStart")]
		public void ReceivePlaySwingStart()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.startSwing();
			}
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x0019DA79 File Offset: 0x0019BC79
		[Obsolete]
		public void askSwingStop(CSteamID steamID)
		{
			this.ReceivePlaySwingStop();
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x0019DA81 File Offset: 0x0019BC81
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askSwingStop")]
		public void ReceivePlaySwingStop()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.stopSwing();
			}
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x0019DA9B File Offset: 0x0019BC9B
		[Obsolete]
		public void askSwing(CSteamID steamID, byte mode)
		{
			this.ReceivePlaySwing((ESwingMode)mode);
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x0019DAA4 File Offset: 0x0019BCA4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askSwing")]
		public void ReceivePlaySwing(ESwingMode mode)
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.swingMode = mode;
				this.swing();
			}
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x0019DAC8 File Offset: 0x0019BCC8
		private void fire()
		{
			float num = (float)base.player.equipment.quality / 100f;
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, this.equippedMeleeAsset.alertRadius);
			}
			if (base.channel.IsLocalPlayer)
			{
				int num2;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Shot", out num2))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Shot", num2 + 1);
				}
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), ((ItemWeaponAsset)base.player.equipment.asset).range, RayMasks.DAMAGE_CLIENT, base.player);
				if (raycastInfo.player != null && this.equippedMeleeAsset.playerDamageMultiplier.damage > 1f && (DamageTool.isPlayerAllowedToDamagePlayer(base.player, raycastInfo.player) || this.equippedMeleeAsset.bypassAllowedToDamagePlayer))
				{
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
					}
					if (raycastInfo.limb == ELimb.SKULL && Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num2))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Headshots", num2 + 1);
					}
					PlayerUI.hitmark(raycastInfo.point, false, (raycastInfo.limb == ELimb.SKULL) ? EPlayerHit.CRITICAL : EPlayerHit.ENTITIY);
				}
				else if ((raycastInfo.zombie != null && this.equippedMeleeAsset.zombieDamageMultiplier.damage > 1f) || (raycastInfo.animal != null && this.equippedMeleeAsset.animalDamageMultiplier.damage > 1f))
				{
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
					}
					if (raycastInfo.limb == ELimb.SKULL && Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num2))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Headshots", num2 + 1);
					}
					PlayerUI.hitmark(raycastInfo.point, false, (raycastInfo.limb == ELimb.SKULL) ? EPlayerHit.CRITICAL : EPlayerHit.ENTITIY);
				}
				else if (raycastInfo.vehicle != null && this.equippedMeleeAsset.vehicleDamage > 1f)
				{
					if (this.equippedMeleeAsset.isRepair)
					{
						if (!raycastInfo.vehicle.isExploded && !raycastInfo.vehicle.isRepaired && raycastInfo.vehicle.canPlayerRepair(base.player))
						{
							if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
							{
								Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
							}
							PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
						}
					}
					else if (!raycastInfo.vehicle.isDead && raycastInfo.vehicle.asset != null && raycastInfo.vehicle.canBeDamaged && (raycastInfo.vehicle.asset.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
					{
						if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
						{
							Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
						}
						PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
					}
				}
				else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Barricade") && this.equippedMeleeAsset.barricadeDamage > 1f)
				{
					BarricadeDrop barricadeDrop = BarricadeDrop.FindByRootFast(raycastInfo.transform);
					if (barricadeDrop != null)
					{
						ItemBarricadeAsset asset = barricadeDrop.asset;
						if (asset != null)
						{
							if (this.equippedMeleeAsset.isRepair)
							{
								Interactable2HP component = raycastInfo.transform.GetComponent<Interactable2HP>();
								if (component != null && asset.isRepairable && component.hp < 100)
								{
									if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
									{
										Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
									}
									PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
								}
							}
							else if (asset.canBeDamaged && (asset.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
							{
								if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
								{
									Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
								}
								PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
							}
						}
					}
				}
				else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Structure") && this.equippedMeleeAsset.structureDamage > 1f)
				{
					StructureDrop structureDrop = StructureDrop.FindByRootFast(raycastInfo.transform);
					if (structureDrop != null)
					{
						ItemStructureAsset asset2 = structureDrop.asset;
						if (asset2 != null)
						{
							if (this.equippedMeleeAsset.isRepair)
							{
								Interactable2HP component2 = raycastInfo.transform.GetComponent<Interactable2HP>();
								if (component2 != null && asset2.isRepairable && component2.hp < 100)
								{
									if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
									{
										Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
									}
									PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
								}
							}
							else if (asset2.canBeDamaged && (asset2.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
							{
								if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
								{
									Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
								}
								PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
							}
						}
					}
				}
				else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Resource") && this.equippedMeleeAsset.resourceDamage > 1f)
				{
					byte x;
					byte y;
					ushort index;
					if (ResourceManager.tryGetRegion(raycastInfo.transform, out x, out y, out index))
					{
						ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);
						bool flag = resourceSpawnpoint.asset.vulnerableToAllMeleeWeapons || this.equippedMeleeAsset.hasBladeID(resourceSpawnpoint.asset.bladeID);
						if (resourceSpawnpoint != null && !resourceSpawnpoint.isDead && flag)
						{
							if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
							{
								Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
							}
							PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
						}
					}
				}
				else if (raycastInfo.transform != null && this.equippedMeleeAsset.objectDamage > 1f)
				{
					InteractableObjectRubble componentInParent = raycastInfo.transform.GetComponentInParent<InteractableObjectRubble>();
					if (componentInParent != null)
					{
						raycastInfo.transform = componentInParent.transform;
						raycastInfo.section = componentInParent.getSection(raycastInfo.collider.transform);
						if (componentInParent.IsSectionIndexValid(raycastInfo.section) && !componentInParent.isSectionDead(raycastInfo.section) && this.equippedMeleeAsset.hasBladeID(componentInParent.asset.rubbleBladeID) && (componentInParent.asset.rubbleIsVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
						{
							if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
							{
								Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
							}
							PlayerUI.hitmark(raycastInfo.point, false, EPlayerHit.BUILD);
						}
					}
				}
				if (!this.equippedMeleeAsset.allowFleshFx && (raycastInfo.player != null || raycastInfo.animal != null || raycastInfo.zombie != null))
				{
					raycastInfo.material = EPhysicsMaterial.NONE;
					raycastInfo.materialName = string.Empty;
				}
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Melee);
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Melee);
				if (input == null)
				{
					return;
				}
				if ((input.point - base.player.look.aim.position).sqrMagnitude > MathfEx.Square(this.equippedMeleeAsset.range + 4f))
				{
					return;
				}
				if ((!this.equippedMeleeAsset.isRepair || !this.equippedMeleeAsset.isRepeated) && !string.IsNullOrEmpty(input.materialName))
				{
					this.ServerSpawnMeleeImpact(input.point, input.normal, input.materialName, input.colliderTransform, base.channel.GatherOwnerAndClientConnectionsWithinSphere(input.point, EffectManager.SMALL));
				}
				EPlayerKill eplayerKill = EPlayerKill.NONE;
				uint num3 = 0U;
				float num4 = 1f;
				num4 *= 1f + base.channel.owner.player.skills.mastery(0, 0) * 0.5f;
				num4 *= ((this.swingMode == ESwingMode.STRONG) ? this.equippedMeleeAsset.strength : 1f);
				num4 *= ((num < 0.5f) ? (0.5f + num) : 1f);
				ERagdollEffect useableRagdollEffect = base.player.equipment.getUseableRagdollEffect();
				if (input.type != ERaycastInfoType.NONE && input.type != ERaycastInfoType.SKIP && Provider.modeConfigData.Items.ShouldWeaponTakeDamage && base.player.equipment.quality > 0 && Random.value < ((ItemWeaponAsset)base.player.equipment.asset).durability)
				{
					if (base.player.equipment.quality > ((ItemWeaponAsset)base.player.equipment.asset).wear)
					{
						PlayerEquipment equipment = base.player.equipment;
						equipment.quality -= ((ItemWeaponAsset)base.player.equipment.asset).wear;
					}
					else
					{
						base.player.equipment.quality = 0;
					}
					base.player.equipment.sendUpdateQuality();
				}
				if (input.type == ERaycastInfoType.PLAYER)
				{
					if (input.player != null && (DamageTool.isPlayerAllowedToDamagePlayer(base.player, input.player) || this.equippedMeleeAsset.bypassAllowedToDamagePlayer))
					{
						IDamageMultiplier playerDamageMultiplier = this.equippedMeleeAsset.playerDamageMultiplier;
						DamagePlayerParameters parameters = DamagePlayerParameters.make(input.player, EDeathCause.MELEE, input.direction, playerDamageMultiplier, input.limb);
						parameters.killer = base.channel.owner.playerID.steamID;
						parameters.times = num4;
						parameters.respectArmor = true;
						parameters.trackKill = true;
						parameters.ragdollEffect = useableRagdollEffect;
						this.equippedMeleeAsset.initPlayerDamageParameters(ref parameters);
						if (base.player.input.IsUnderFakeLagPenalty)
						{
							parameters.times *= Provider.configData.Server.Fake_Lag_Damage_Penalty_Multiplier;
						}
						DamageTool.damagePlayer(parameters, out eplayerKill);
					}
				}
				else if (input.type == ERaycastInfoType.ZOMBIE)
				{
					if (input.zombie != null)
					{
						EZombieStunOverride ezombieStunOverride = this.equippedMeleeAsset.zombieStunOverride;
						if (Provider.modeConfigData.Zombies.Only_Critical_Stuns && ezombieStunOverride == EZombieStunOverride.None && this.swingMode == ESwingMode.STRONG)
						{
							ezombieStunOverride = EZombieStunOverride.Always;
						}
						IDamageMultiplier zombieOrPlayerDamageMultiplier = this.equippedMeleeAsset.zombieOrPlayerDamageMultiplier;
						DamageZombieParameters parameters2 = DamageZombieParameters.make(input.zombie, input.direction, zombieOrPlayerDamageMultiplier, input.limb);
						parameters2.times = num4;
						parameters2.allowBackstab = true;
						parameters2.respectArmor = true;
						parameters2.instigator = base.player;
						parameters2.zombieStunOverride = ezombieStunOverride;
						parameters2.ragdollEffect = useableRagdollEffect;
						if (base.player.movement.nav != 255)
						{
							parameters2.AlertPosition = new Vector3?(base.transform.position);
						}
						DamageTool.damageZombie(parameters2, out eplayerKill, out num3);
					}
				}
				else if (input.type == ERaycastInfoType.ANIMAL)
				{
					if (input.animal != null)
					{
						IDamageMultiplier animalOrPlayerDamageMultiplier = this.equippedMeleeAsset.animalOrPlayerDamageMultiplier;
						DamageAnimalParameters parameters3 = DamageAnimalParameters.make(input.animal, input.direction, animalOrPlayerDamageMultiplier, input.limb);
						parameters3.times = num4;
						parameters3.instigator = base.player;
						parameters3.ragdollEffect = useableRagdollEffect;
						parameters3.AlertPosition = new Vector3?(base.transform.position);
						DamageTool.damageAnimal(parameters3, out eplayerKill, out num3);
					}
				}
				else if (input.type == ERaycastInfoType.VEHICLE)
				{
					if (input.vehicle != null && input.vehicle.asset != null)
					{
						if (this.equippedMeleeAsset.isRepair)
						{
							if (!input.vehicle.isExploded && !input.vehicle.isRepaired && input.vehicle.canPlayerRepair(base.player))
							{
								num4 *= 1f + base.channel.owner.player.skills.mastery(2, 6);
								DamageTool.damage(input.vehicle, true, input.point, this.equippedMeleeAsset.isRepair, this.equippedMeleeAsset.vehicleDamage, num4 * Provider.modeConfigData.Vehicles.Melee_Repair_Multiplier, true, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Melee);
							}
						}
						else if (input.vehicle.canBeDamaged && (input.vehicle.asset.isVulnerable || this.equippedMeleeAsset.isInvulnerable))
						{
							DamageTool.damage(input.vehicle, true, input.point, this.equippedMeleeAsset.isRepair, this.equippedMeleeAsset.vehicleDamage, num4 * Provider.modeConfigData.Vehicles.Melee_Damage_Multiplier, true, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Melee);
						}
					}
				}
				else if (input.type == ERaycastInfoType.BARRICADE)
				{
					if (input.transform != null && input.transform.CompareTag("Barricade"))
					{
						BarricadeDrop barricadeDrop2 = BarricadeDrop.FindByRootFast(input.transform);
						if (barricadeDrop2 != null)
						{
							ItemBarricadeAsset asset3 = barricadeDrop2.asset;
							if (asset3 != null)
							{
								if (this.equippedMeleeAsset.isRepair)
								{
									if (asset3.isRepairable)
									{
										num4 *= 1f + base.channel.owner.player.skills.mastery(2, 6);
										DamageTool.damage(input.transform, true, this.equippedMeleeAsset.barricadeDamage, num4 * Provider.modeConfigData.Barricades.Melee_Repair_Multiplier, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Unknown);
									}
								}
								else if (asset3.canBeDamaged && (asset3.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
								{
									DamageTool.damage(input.transform, false, this.equippedMeleeAsset.barricadeDamage, num4 * Provider.modeConfigData.Barricades.Melee_Damage_Multiplier, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Melee);
								}
							}
						}
					}
				}
				else if (input.type == ERaycastInfoType.STRUCTURE)
				{
					if (input.transform != null && input.transform.CompareTag("Structure"))
					{
						StructureDrop structureDrop2 = StructureDrop.FindByRootFast(input.transform);
						if (structureDrop2 != null)
						{
							ItemStructureAsset asset4 = structureDrop2.asset;
							if (asset4 != null)
							{
								if (this.equippedMeleeAsset.isRepair)
								{
									if (asset4.isRepairable)
									{
										num4 *= 1f + base.channel.owner.player.skills.mastery(2, 6);
										DamageTool.damage(input.transform, true, input.direction, this.equippedMeleeAsset.structureDamage, num4 * Provider.modeConfigData.Structures.Melee_Repair_Multiplier, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Melee);
									}
								}
								else if (asset4.canBeDamaged && (asset4.isVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
								{
									DamageTool.damage(input.transform, false, input.direction, this.equippedMeleeAsset.structureDamage, num4 * Provider.modeConfigData.Structures.Melee_Damage_Multiplier, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Melee);
								}
							}
						}
					}
				}
				else if (input.type == ERaycastInfoType.RESOURCE)
				{
					if (input.transform != null && input.transform.CompareTag("Resource"))
					{
						num4 *= 1f + base.channel.owner.player.skills.mastery(2, 2) * 0.5f;
						byte x2;
						byte y2;
						ushort index2;
						if (ResourceManager.tryGetRegion(input.transform, out x2, out y2, out index2))
						{
							ResourceSpawnpoint resourceSpawnpoint2 = ResourceManager.getResourceSpawnpoint(x2, y2, index2);
							bool flag2 = resourceSpawnpoint2.asset.vulnerableToAllMeleeWeapons || this.equippedMeleeAsset.hasBladeID(resourceSpawnpoint2.asset.bladeID);
							if (resourceSpawnpoint2 != null && !resourceSpawnpoint2.isDead && flag2)
							{
								DamageTool.damage(input.transform, input.direction, this.equippedMeleeAsset.resourceDamage, num4, 1f + base.channel.owner.player.skills.mastery(2, 2) * 0.5f, out eplayerKill, out num3, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Melee);
							}
						}
					}
				}
				else if (input.type == ERaycastInfoType.OBJECT && input.transform != null && input.section < 255)
				{
					InteractableObjectRubble componentInParent2 = input.transform.GetComponentInParent<InteractableObjectRubble>();
					if (componentInParent2 != null && componentInParent2.IsSectionIndexValid(input.section) && !componentInParent2.isSectionDead(input.section) && this.equippedMeleeAsset.hasBladeID(componentInParent2.asset.rubbleBladeID) && (componentInParent2.asset.rubbleIsVulnerable || ((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable))
					{
						DamageTool.damage(componentInParent2.transform, input.direction, input.section, this.equippedMeleeAsset.objectDamage, num4, out eplayerKill, out num3, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Melee);
					}
				}
				if (input.type != ERaycastInfoType.PLAYER && input.type != ERaycastInfoType.ZOMBIE && input.type != ERaycastInfoType.ANIMAL && !base.player.life.isAggressor)
				{
					float num5 = this.equippedMeleeAsset.range + Provider.modeConfigData.Players.Ray_Aggressor_Distance;
					num5 *= num5;
					float num6 = Provider.modeConfigData.Players.Ray_Aggressor_Distance;
					num6 *= num6;
					Vector3 forward = base.player.look.aim.forward;
					for (int i = 0; i < Provider.clients.Count; i++)
					{
						if (Provider.clients[i] != base.channel.owner)
						{
							Player player = Provider.clients[i].player;
							if (!(player == null))
							{
								Vector3 vector = player.look.aim.position - base.player.look.aim.position;
								Vector3 a = Vector3.Project(vector, forward);
								if (a.sqrMagnitude < num5 && (a - vector).sqrMagnitude < num6)
								{
									base.player.life.markAggressive(false, true);
								}
							}
						}
					}
				}
				if (Level.info.type == ELevelType.HORDE)
				{
					if (input.zombie != null)
					{
						if (input.limb == ELimb.SKULL)
						{
							base.player.skills.askPay(10U);
						}
						else
						{
							base.player.skills.askPay(5U);
						}
					}
					if (eplayerKill == EPlayerKill.ZOMBIE)
					{
						if (input.limb == ELimb.SKULL)
						{
							base.player.skills.askPay(50U);
							return;
						}
						base.player.skills.askPay(25U);
						return;
					}
				}
				else
				{
					if (eplayerKill == EPlayerKill.PLAYER && Level.info.type == ELevelType.ARENA)
					{
						base.player.skills.askPay(100U);
					}
					base.player.sendStat(eplayerKill);
					if (num3 > 0U)
					{
						base.player.skills.askPay(num3);
					}
				}
			}
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x0019F17C File Offset: 0x0019D37C
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy || base.player.quests.IsCutsceneModeActive())
			{
				return false;
			}
			if (this.equippedMeleeAsset.isRepeated)
			{
				if (!this.isSwinging)
				{
					this.swingMode = ESwingMode.WEAK;
					this.startSwing();
					if (Provider.isServer)
					{
						UseableMelee.SendPlaySwingStart.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
					}
					return true;
				}
			}
			else if (this.isUseable)
			{
				base.player.equipment.isBusy = true;
				this.startedUse = base.player.input.simulation;
				this.startedSwing = Time.realtimeSinceStartup;
				this.isUsing = true;
				this.swingMode = ESwingMode.WEAK;
				this.swing();
				if (Provider.isServer)
				{
					UseableMelee.SendPlaySwing.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner(), this.swingMode);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x0019F274 File Offset: 0x0019D474
		public override void stopPrimary()
		{
			if (base.player.equipment.isBusy || base.player.quests.IsCutsceneModeActive())
			{
				return;
			}
			if (this.equippedMeleeAsset.isRepeated && this.isSwinging)
			{
				this.stopSwing();
				if (Provider.isServer)
				{
					UseableMelee.SendPlaySwingStop.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
			}
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x0019F2E4 File Offset: 0x0019D4E4
		public override bool startSecondary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (!this.equippedMeleeAsset.isRepeated && this.isUseable && (float)base.player.life.stamina >= (float)this.equippedMeleeAsset.stamina * (1f - base.player.skills.mastery(0, 4) * 0.75f))
			{
				base.player.life.askTire((byte)((float)this.equippedMeleeAsset.stamina * (1f - base.player.skills.mastery(0, 4) * 0.5f)));
				base.player.equipment.isBusy = true;
				this.swingMode = ESwingMode.STRONG;
				this.swing();
				if (Provider.isServer)
				{
					UseableMelee.SendPlaySwing.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner(), this.swingMode);
				}
				return true;
			}
			return false;
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06004587 RID: 17799 RVA: 0x0019F3E7 File Offset: 0x0019D5E7
		public override bool canInspect
		{
			get
			{
				return !this.isUsing && !this.isSwinging;
			}
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x0019F3FC File Offset: 0x0019D5FC
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			if (this.equippedMeleeAsset.isLight)
			{
				this.interact = (base.player.equipment.state[0] == 1);
				if (base.channel.IsLocalPlayer)
				{
					this.firstLightHook = base.player.equipment.firstModel.Find("Model_0").Find("Light");
					this.firstLightHook.tag = "Viewmodel";
					this.firstLightHook.gameObject.layer = 11;
					Transform transform = this.firstLightHook.Find("Light");
					if (transform != null)
					{
						transform.tag = "Viewmodel";
						transform.gameObject.layer = 11;
					}
					PlayerUI.message(EPlayerMessage.LIGHT, "", 2f);
				}
				this.thirdLightHook = base.player.equipment.thirdModel.Find("Model_0").Find("Light");
				LightLODTool.applyLightLOD(this.thirdLightHook);
				if (base.channel.IsLocalPlayer && this.thirdLightHook != null)
				{
					Transform transform2 = this.thirdLightHook.Find("Light");
					if (transform2 != null)
					{
						this.firstFakeLight = Object.Instantiate<GameObject>(transform2.gameObject).transform;
						this.firstFakeLight.name = "Emitter";
					}
				}
			}
			else
			{
				this.firstLightHook = null;
				this.thirdLightHook = null;
			}
			this.updateAttachments();
			if (this.equippedMeleeAsset.isRepeated)
			{
				if (base.channel.IsLocalPlayer && base.player.equipment.firstModel.Find("Hit") != null)
				{
					this.firstEmitter = base.player.equipment.firstModel.Find("Hit").GetComponent<ParticleSystem>();
					this.firstEmitter.tag = "Viewmodel";
					this.firstEmitter.gameObject.layer = 11;
				}
				if (base.player.equipment.thirdModel.Find("Hit") != null)
				{
					this.thirdEmitter = base.player.equipment.thirdModel.Find("Hit").GetComponent<ParticleSystem>();
				}
				this.weakAttackAnimLengthSeconds = base.player.animator.GetAnimationLength("Start_Swing", true);
				this.strongAttackAnimLengthSeconds = base.player.animator.GetAnimationLength("Stop_Swing", true);
			}
			else
			{
				this.weakAttackAnimLengthSeconds = base.player.animator.GetAnimationLength("Weak", true);
				this.strongAttackAnimLengthSeconds = base.player.animator.GetAnimationLength("Strong", true);
			}
			this.weakAttackAnimLengthFrames = (uint)(this.weakAttackAnimLengthSeconds / PlayerInput.RATE);
			this.strongAttackAnimLengthFrames = (uint)(this.strongAttackAnimLengthSeconds / PlayerInput.RATE);
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x0019F6F4 File Offset: 0x0019D8F4
		public override void dequip()
		{
			base.player.disableItemSpotLight();
			if (base.channel.IsLocalPlayer)
			{
				base.player.animator.viewmodelCameraLocalPositionOffset = Vector3.zero;
				if (this.firstFakeLight != null)
				{
					Object.Destroy(this.firstFakeLight.gameObject);
					this.firstFakeLight = null;
				}
			}
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x0019F753 File Offset: 0x0019D953
		public override void updateState(byte[] newState)
		{
			if (this.equippedMeleeAsset.isLight)
			{
				this.interact = (newState[0] == 1);
			}
			this.updateAttachments();
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x0019F774 File Offset: 0x0019D974
		public override void tick()
		{
			if (!base.player.equipment.IsEquipAnimationFinished)
			{
				return;
			}
			if (base.channel.IsLocalPlayer)
			{
				if (this.isSwinging)
				{
					if (this.equippedMeleeAsset.isRepeated && !this.equippedMeleeAsset.isRepair)
					{
						base.player.animator.viewmodelCameraLocalPositionOffset = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));
					}
					else
					{
						base.player.animator.viewmodelCameraLocalPositionOffset = Vector3.zero;
					}
				}
				if (InputEx.GetKeyDown(ControlsSettings.tactical) && this.equippedMeleeAsset.isLight)
				{
					UseableMelee.SendInteractMelee.Invoke(base.GetNetId(), ENetReliability.Unreliable);
				}
			}
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x0019F84C File Offset: 0x0019DA4C
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isDamageable)
			{
				if (this.equippedMeleeAsset.isRepeated)
				{
					this.startedUse = base.player.input.simulation;
				}
				else
				{
					base.player.equipment.isBusy = false;
					this.isUsing = false;
				}
				this.fire();
			}
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x0019F8AC File Offset: 0x0019DAAC
		private void updateAttachments()
		{
			if (this.equippedMeleeAsset.isLight)
			{
				if (this.interact && this.equippedMeleeAsset != null)
				{
					base.player.enableItemSpotLight(this.equippedMeleeAsset.lightConfig);
					return;
				}
				base.player.disableItemSpotLight();
			}
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x0019F8F8 File Offset: 0x0019DAF8
		private void Update()
		{
			if (base.channel.IsLocalPlayer && this.firstFakeLight != null && this.thirdLightHook != null)
			{
				this.firstFakeLight.position = this.thirdLightHook.position;
				if (this.firstFakeLight.gameObject.activeSelf != (base.player.look.perspective == EPlayerPerspective.FIRST && this.thirdLightHook.gameObject.activeSelf))
				{
					this.firstFakeLight.gameObject.SetActive(base.player.look.perspective == EPlayerPerspective.FIRST && this.thirdLightHook.gameObject.activeSelf);
				}
			}
		}

		// Token: 0x04002EE0 RID: 12000
		private uint startedUse;

		// Token: 0x04002EE1 RID: 12001
		private float startedSwing;

		// Token: 0x04002EE2 RID: 12002
		private float weakAttackAnimLengthSeconds;

		// Token: 0x04002EE3 RID: 12003
		private float strongAttackAnimLengthSeconds;

		// Token: 0x04002EE4 RID: 12004
		private uint weakAttackAnimLengthFrames;

		// Token: 0x04002EE5 RID: 12005
		private uint strongAttackAnimLengthFrames;

		/// <summary>
		/// For non-repeat weapons the "Use" audio clip is played once time reaches this point.
		/// </summary>
		// Token: 0x04002EE6 RID: 12006
		private double playUseSoundTime;

		// Token: 0x04002EE7 RID: 12007
		private bool isUsing;

		// Token: 0x04002EE8 RID: 12008
		private bool isSwinging;

		// Token: 0x04002EE9 RID: 12009
		private ESwingMode swingMode;

		// Token: 0x04002EEA RID: 12010
		private ParticleSystem firstEmitter;

		// Token: 0x04002EEB RID: 12011
		private ParticleSystem thirdEmitter;

		// Token: 0x04002EEC RID: 12012
		private Transform firstLightHook;

		// Token: 0x04002EED RID: 12013
		private Transform thirdLightHook;

		// Token: 0x04002EEE RID: 12014
		private Transform firstFakeLight;

		// Token: 0x04002EEF RID: 12015
		private bool interact;

		// Token: 0x04002EF0 RID: 12016
		private static ClientInstanceMethod<Vector3, Vector3, string, Transform> SendSpawnMeleeImpact = ClientInstanceMethod<Vector3, Vector3, string, Transform>.Get(typeof(UseableMelee), "ReceiveSpawnMeleeImpact");

		// Token: 0x04002EF1 RID: 12017
		private static readonly ServerInstanceMethod SendInteractMelee = ServerInstanceMethod.Get(typeof(UseableMelee), "ReceiveInteractMelee");

		// Token: 0x04002EF2 RID: 12018
		private static readonly ClientInstanceMethod SendPlaySwingStart = ClientInstanceMethod.Get(typeof(UseableMelee), "ReceivePlaySwingStart");

		// Token: 0x04002EF3 RID: 12019
		private static readonly ClientInstanceMethod SendPlaySwingStop = ClientInstanceMethod.Get(typeof(UseableMelee), "ReceivePlaySwingStop");

		// Token: 0x04002EF4 RID: 12020
		private static readonly ClientInstanceMethod<ESwingMode> SendPlaySwing = ClientInstanceMethod<ESwingMode>.Get(typeof(UseableMelee), "ReceivePlaySwing");
	}
}
