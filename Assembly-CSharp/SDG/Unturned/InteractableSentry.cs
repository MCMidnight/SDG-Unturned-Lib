using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200046B RID: 1131
	public class InteractableSentry : InteractableStorage
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x0008558F File Offset: 0x0008378F
		// (set) Token: 0x06002283 RID: 8835 RVA: 0x00085597 File Offset: 0x00083797
		public ItemSentryAsset sentryAsset { get; private set; }

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002284 RID: 8836 RVA: 0x000855A0 File Offset: 0x000837A0
		public ESentryMode sentryMode
		{
			get
			{
				return this.sentryAsset.sentryMode;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002285 RID: 8837 RVA: 0x000855AD File Offset: 0x000837AD
		public bool isPowered
		{
			get
			{
				return !(this.power == null) && (!this.sentryAsset.requiresPower || this.power.isWired);
			}
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x000855DC File Offset: 0x000837DC
		private void trace(Vector3 pos, Vector3 dir)
		{
			if (this.tracerEmitter == null)
			{
				return;
			}
			if (this.attachments.barrelModel != null && this.attachments.barrelAsset.isBraked && this.displayItem.state[16] > 0)
			{
				return;
			}
			this.tracerEmitter.transform.position = pos;
			this.tracerEmitter.transform.rotation = Quaternion.LookRotation(dir);
			this.tracerEmitter.Emit(1);
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x00085664 File Offset: 0x00083864
		public void shoot()
		{
			this.lastAlert = Time.timeAsDouble;
			this.lastShot = Time.timeAsDouble;
			if (this.attachments.barrelAsset != null && this.attachments.barrelAsset.durability > 0)
			{
				if (this.attachments.barrelAsset.durability > this.displayItem.state[16])
				{
					this.displayItem.state[16] = 0;
					return;
				}
				byte[] state = this.displayItem.state;
				int num = 16;
				state[num] -= this.attachments.barrelAsset.durability;
			}
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000856FE File Offset: 0x000838FE
		public void alert(float newYaw, float newPitch)
		{
			this.targetYaw = newYaw;
			this.targetPitch = newPitch;
			this.lastAlert = Time.timeAsDouble;
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x0008571C File Offset: 0x0008391C
		public override void updateState(Asset asset, byte[] state)
		{
			this.sentryAsset = (asset as ItemSentryAsset);
			if (!this.hasInitializedSentryComponents)
			{
				this.hasInitializedSentryComponents = true;
				this.yawTransform = base.transform.Find("Yaw");
				if (this.yawTransform != null)
				{
					this.pitchTransform = this.yawTransform.Find("Pitch");
					if (this.pitchTransform != null)
					{
						this.aimTransform = this.pitchTransform.Find("Aim");
						Transform transform = this.aimTransform.Find("Spot");
						if (transform != null)
						{
							this.spotGameObject = transform.gameObject;
						}
					}
				}
				Transform transform2 = base.transform.FindChildRecursive("On");
				if (transform2 != null)
				{
					this.onGameObject = transform2.gameObject;
				}
				Transform transform3 = base.transform.FindChildRecursive("On_Model");
				if (transform3 != null)
				{
					this.onModelGameObject = transform3.gameObject;
					Renderer component = this.onModelGameObject.GetComponent<Renderer>();
					this.onMaterial = ((component != null) ? component.material : null);
				}
				Transform transform4 = base.transform.FindChildRecursive("Off");
				if (transform4 != null)
				{
					this.offGameObject = transform4.gameObject;
				}
				Transform transform5 = base.transform.FindChildRecursive("Off_Model");
				if (transform5 != null)
				{
					this.offModelGameObject = transform5.gameObject;
					Renderer component2 = this.offModelGameObject.GetComponent<Renderer>();
					this.offMaterial = ((component2 != null) ? component2.material : null);
				}
			}
			this.isAlert = false;
			this.lastAlert = 0.0;
			this.targetYaw = HousingConnections.GetModelYaw(base.transform);
			this.yaw = this.targetYaw;
			this.targetPitch = 0f;
			this.pitch = this.targetPitch;
			this.targetPlayer = null;
			this.targetAnimal = null;
			this.targetZombie = null;
			base.updateState(asset, state);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x00085908 File Offset: 0x00083B08
		public override void refreshDisplay()
		{
			base.refreshDisplay();
			this.hasWeapon = false;
			this.attachments = null;
			this.gunshotAudioSource = null;
			this.destroyEffects();
			if (this.spotGameObject != null)
			{
				this.spotGameObject.SetActive(false);
			}
			if (this.displayAsset == null || this.displayAsset.type != EItemType.GUN || ((ItemGunAsset)this.displayAsset).action == EAction.String || ((ItemGunAsset)this.displayAsset).action == EAction.Rocket)
			{
				return;
			}
			this.hasWeapon = true;
			this.attachments = this.displayModel.gameObject.GetComponent<Attachments>();
			this.interact = (this.displayItem.state[12] == 1);
			if (this.attachments.ejectHook != null && ((ItemGunAsset)this.displayAsset).action != EAction.String && ((ItemGunAsset)this.displayAsset).action != EAction.Rocket)
			{
				EffectAsset effectAsset = ((ItemGunAsset)this.displayAsset).FindShellEffectAsset();
				if (effectAsset != null)
				{
					Transform transform = EffectManager.InstantiateFromPool(effectAsset).transform;
					transform.name = "Emitter";
					transform.parent = this.attachments.ejectHook;
					transform.localPosition = Vector3.zero;
					transform.localRotation = Quaternion.identity;
					this.shellEmitter = transform.GetComponent<ParticleSystem>();
				}
			}
			if (this.attachments.barrelHook != null)
			{
				EffectAsset effectAsset2 = ((ItemGunAsset)this.displayAsset).FindMuzzleEffectAsset();
				if (effectAsset2 != null)
				{
					Transform transform2 = EffectManager.InstantiateFromPool(effectAsset2).transform;
					transform2.name = "Emitter";
					transform2.parent = this.attachments.barrelHook;
					transform2.localPosition = Vector3.zero;
					transform2.localRotation = Quaternion.identity;
					this.muzzleEmitter = transform2.GetComponent<ParticleSystem>();
					this.muzzleLight = transform2.GetComponent<Light>();
					if (this.muzzleLight != null)
					{
						this.muzzleLight.enabled = false;
						this.muzzleLight.cullingMask = -2049;
					}
				}
			}
			if (this.muzzleEmitter != null)
			{
				if (this.attachments.barrelModel != null)
				{
					this.muzzleEmitter.transform.localPosition = Vector3.up * 0.25f;
				}
				else
				{
					this.muzzleEmitter.transform.localPosition = Vector3.zero;
				}
			}
			if (this.attachments.magazineAsset != null)
			{
				EffectAsset effectAsset3 = this.attachments.magazineAsset.FindTracerEffectAsset();
				if (effectAsset3 != null)
				{
					Transform transform3 = EffectManager.InstantiateFromPool(effectAsset3).transform;
					transform3.name = "Tracer";
					transform3.localPosition = Vector3.zero;
					transform3.localRotation = Quaternion.identity;
					this.tracerEmitter = transform3.GetComponent<ParticleSystem>();
				}
			}
			int num = (int)((ItemGunAsset)this.displayAsset).firerate;
			if (this.attachments.sightAsset != null)
			{
				num -= this.attachments.sightAsset.FirerateOffset;
			}
			if (this.attachments.tacticalAsset != null)
			{
				num -= this.attachments.tacticalAsset.FirerateOffset;
			}
			if (this.attachments.gripAsset != null)
			{
				num -= this.attachments.gripAsset.FirerateOffset;
			}
			if (this.attachments.barrelAsset != null)
			{
				num -= this.attachments.barrelAsset.FirerateOffset;
			}
			if (this.attachments.magazineAsset != null)
			{
				num -= this.attachments.magazineAsset.FirerateOffset;
			}
			num = Mathf.Max(num, 1);
			this.fireTime = (float)num;
			this.fireTime /= 50f;
			this.fireTime *= 3.33f;
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x00085CB0 File Offset: 0x00083EB0
		private void Update()
		{
			if (Provider.isServer && this.isPowered)
			{
				Vector3 vector = base.transform.position + new Vector3(0f, 0.65f, 0f);
				if (Time.timeAsDouble - this.lastScan > 0.10000000149011612)
				{
					this.lastScan = Time.timeAsDouble;
					float num = this.sentryAsset.detectionRadius;
					float num2 = this.sentryAsset.targetLossRadius;
					if (this.hasWeapon)
					{
						float range = ((ItemWeaponAsset)this.displayAsset).range;
						num = Mathf.Min(num, range);
						num2 = Mathf.Min(num2, range);
					}
					float num3 = num * num;
					float num4 = num2 * num2;
					float num5 = num3;
					bool flag = false;
					Player x = null;
					Zombie x2 = null;
					Animal x3 = null;
					if (Provider.isPvP)
					{
						float sqrRadius = (this.targetPlayer != null) ? num4 : num5;
						InteractableSentry.playersInRadius.Clear();
						PlayerTool.getPlayersInRadius(vector, sqrRadius, InteractableSentry.playersInRadius);
						for (int i = 0; i < InteractableSentry.playersInRadius.Count; i++)
						{
							Player player = InteractableSentry.playersInRadius[i];
							if (!(player.channel.owner.playerID.steamID == base.owner) && !player.quests.isMemberOfGroup(base.group) && !player.life.isDead && player.animator.gesture != EPlayerGesture.ARREST_START && (!player.movement.isSafe || !player.movement.isSafeInfo.noWeapons) && player.movement.canAddSimulationResultsToUpdates && (!(x != null) || player.animator.gesture != EPlayerGesture.SURRENDER_START) && (this.sentryMode != ESentryMode.FRIENDLY || Time.realtimeSinceStartup - player.equipment.lastPunching < 2f || (player.equipment.HasValidUseable && player.equipment.asset != null && player.equipment.asset.shouldFriendlySentryTargetUser)))
							{
								float sqrMagnitude = (player.look.aim.position - vector).sqrMagnitude;
								if (!(player != this.targetPlayer) || sqrMagnitude <= num5)
								{
									Vector3 a = player.look.aim.position - vector;
									float magnitude = a.magnitude;
									Vector3 vector2 = a / magnitude;
									if (!(player != this.targetPlayer) || Vector3.Dot(vector2, this.aimTransform.forward) >= 0.5f)
									{
										if (magnitude > 0.025f)
										{
											RaycastHit raycastHit;
											Physics.Raycast(new Ray(vector, vector2), out raycastHit, magnitude - 0.025f, RayMasks.BLOCK_SENTRY);
											if (raycastHit.transform != null && raycastHit.transform != base.transform)
											{
												goto IL_356;
											}
											Physics.Raycast(new Ray(vector + vector2 * (magnitude - 0.025f), -vector2), out raycastHit, magnitude - 0.025f, RayMasks.DAMAGE_SERVER);
											if (raycastHit.transform != null && raycastHit.transform != base.transform)
											{
												goto IL_356;
											}
										}
										num5 = sqrMagnitude;
										x = player;
										flag = true;
									}
								}
							}
							IL_356:;
						}
					}
					float sqrRadius2 = (!flag && this.targetZombie != null) ? num4 : num5;
					InteractableSentry.zombiesInRadius.Clear();
					ZombieManager.getZombiesInRadius(vector, sqrRadius2, InteractableSentry.zombiesInRadius);
					for (int j = 0; j < InteractableSentry.zombiesInRadius.Count; j++)
					{
						Zombie zombie = InteractableSentry.zombiesInRadius[j];
						if (!zombie.isDead && zombie.isHunting)
						{
							Vector3 a2 = zombie.transform.position;
							switch (zombie.speciality)
							{
							case EZombieSpeciality.NORMAL:
								a2 += new Vector3(0f, 1.75f, 0f);
								break;
							case EZombieSpeciality.MEGA:
								a2 += new Vector3(0f, 2.625f, 0f);
								break;
							case EZombieSpeciality.CRAWLER:
								a2 += new Vector3(0f, 0.25f, 0f);
								break;
							case EZombieSpeciality.SPRINTER:
								a2 += new Vector3(0f, 1f, 0f);
								break;
							}
							float sqrMagnitude2 = (a2 - vector).sqrMagnitude;
							if (!(zombie != this.targetZombie) || sqrMagnitude2 <= num5)
							{
								Vector3 a3 = a2 - vector;
								float magnitude2 = a3.magnitude;
								Vector3 vector3 = a3 / magnitude2;
								if (!(zombie != this.targetZombie) || Vector3.Dot(vector3, this.aimTransform.forward) >= 0.5f)
								{
									if (magnitude2 > 0.025f)
									{
										RaycastHit raycastHit2;
										Physics.Raycast(new Ray(vector, vector3), out raycastHit2, magnitude2 - 0.025f, RayMasks.BLOCK_SENTRY);
										if (raycastHit2.transform != null && raycastHit2.transform != base.transform)
										{
											goto IL_59F;
										}
										Physics.Raycast(new Ray(vector + vector3 * (magnitude2 - 0.025f), -vector3), out raycastHit2, magnitude2 - 0.025f, RayMasks.DAMAGE_SERVER);
										if (raycastHit2.transform != null && raycastHit2.transform != base.transform)
										{
											goto IL_59F;
										}
									}
									num5 = sqrMagnitude2;
									x = null;
									x2 = zombie;
									flag = true;
								}
							}
						}
						IL_59F:;
					}
					float sqrRadius3 = (!flag && this.targetAnimal != null) ? num4 : num5;
					InteractableSentry.animalsInRadius.Clear();
					AnimalManager.getAnimalsInRadius(vector, sqrRadius3, InteractableSentry.animalsInRadius);
					for (int k = 0; k < InteractableSentry.animalsInRadius.Count; k++)
					{
						Animal animal = InteractableSentry.animalsInRadius[k];
						if (!animal.isDead)
						{
							Vector3 position = animal.transform.position;
							float sqrMagnitude3 = (position - vector).sqrMagnitude;
							if (!(animal != this.targetAnimal) || sqrMagnitude3 <= num5)
							{
								Vector3 a4 = position - vector;
								float magnitude3 = a4.magnitude;
								Vector3 vector4 = a4 / magnitude3;
								if (!(animal != this.targetAnimal) || Vector3.Dot(vector4, this.aimTransform.forward) >= 0.5f)
								{
									if (magnitude3 > 0.025f)
									{
										RaycastHit raycastHit3;
										Physics.Raycast(new Ray(vector, vector4), out raycastHit3, magnitude3 - 0.025f, RayMasks.BLOCK_SENTRY);
										if (raycastHit3.transform != null && raycastHit3.transform != base.transform)
										{
											goto IL_741;
										}
										Physics.Raycast(new Ray(vector + vector4 * (magnitude3 - 0.025f), -vector4), out raycastHit3, magnitude3 - 0.025f, RayMasks.DAMAGE_SERVER);
										if (raycastHit3.transform != null && raycastHit3.transform != base.transform)
										{
											goto IL_741;
										}
									}
									num5 = sqrMagnitude3;
									x = null;
									x2 = null;
									x3 = animal;
								}
							}
						}
						IL_741:;
					}
					if (x != this.targetPlayer || x2 != this.targetZombie || x3 != this.targetAnimal)
					{
						this.targetPlayer = x;
						this.targetZombie = x2;
						this.targetAnimal = x3;
						this.lastFire = Time.timeAsDouble + 0.1;
					}
				}
				if (this.targetPlayer != null)
				{
					ESentryMode sentryMode = this.sentryMode;
					if (sentryMode > ESentryMode.FRIENDLY)
					{
						if (sentryMode == ESentryMode.HOSTILE)
						{
							this.isFiring = true;
						}
					}
					else
					{
						this.isFiring = (this.targetPlayer.animator.gesture != EPlayerGesture.SURRENDER_START);
					}
					this.isAiming = true;
				}
				else if (this.targetZombie != null)
				{
					this.isFiring = true;
					this.isAiming = true;
				}
				else if (this.targetAnimal != null)
				{
					ESentryMode sentryMode = this.sentryMode;
					if (sentryMode > ESentryMode.FRIENDLY)
					{
						if (sentryMode == ESentryMode.HOSTILE)
						{
							this.isFiring = true;
						}
					}
					else
					{
						this.isFiring = this.targetAnimal.isHunting;
					}
					this.isAiming = true;
				}
				else
				{
					this.isFiring = false;
					this.isAiming = false;
				}
				if (this.isAiming && Time.timeAsDouble - this.lastAim > (double)Provider.UPDATE_TIME)
				{
					this.lastAim = Time.timeAsDouble;
					Transform x4 = null;
					Vector3 vector5 = Vector3.zero;
					if (this.targetPlayer != null)
					{
						x4 = this.targetPlayer.transform;
						vector5 = this.targetPlayer.look.aim.position;
					}
					else if (this.targetZombie != null)
					{
						x4 = this.targetZombie.transform;
						vector5 = this.targetZombie.transform.position;
						switch (this.targetZombie.speciality)
						{
						case EZombieSpeciality.NORMAL:
							vector5 += new Vector3(0f, 1.75f, 0f);
							break;
						case EZombieSpeciality.MEGA:
							vector5 += new Vector3(0f, 2.625f, 0f);
							break;
						case EZombieSpeciality.CRAWLER:
							vector5 += new Vector3(0f, 0.25f, 0f);
							break;
						case EZombieSpeciality.SPRINTER:
							vector5 += new Vector3(0f, 1f, 0f);
							break;
						}
					}
					else if (this.targetAnimal != null)
					{
						x4 = this.targetAnimal.transform;
						vector5 = this.targetAnimal.transform.position + Vector3.up;
					}
					if (x4 != null)
					{
						float num6 = Mathf.Atan2(vector5.x - vector.x, vector5.z - vector.z) * 57.29578f;
						float num7 = Mathf.Sin((vector5.y - vector.y) / (vector5 - vector).magnitude) * 57.29578f;
						BarricadeManager.sendAlertSentry(base.transform, num6, num7);
					}
				}
				if (this.isFiring && this.hasWeapon && !this.isOpen)
				{
					bool flag2 = this.sentryAsset.infiniteAmmo || ((ItemGunAsset)this.displayAsset).infiniteAmmo;
					bool flag3 = flag2 || this.displayItem.state[10] >= ((ItemGunAsset)this.displayAsset).ammoPerShot;
					if (flag3 && Time.timeAsDouble - this.lastFire > (double)this.fireTime)
					{
						this.lastFire += (double)this.fireTime;
						if (Time.timeAsDouble - this.lastFire > (double)this.fireTime)
						{
							this.lastFire = Time.timeAsDouble;
						}
						float quality = (float)this.displayItem.quality / 100f;
						if (this.attachments.magazineAsset == null)
						{
							return;
						}
						if (!flag2)
						{
							byte[] state = this.displayItem.state;
							int num8 = 10;
							state[num8] -= ((ItemGunAsset)this.displayAsset).ammoPerShot;
						}
						if (this.attachments.barrelAsset == null || !this.attachments.barrelAsset.isSilenced || this.displayItem.state[16] == 0)
						{
							AlertTool.alert(base.transform.position, 48f);
						}
						if (!this.sentryAsset.infiniteQuality && Provider.modeConfigData.Items.ShouldWeaponTakeDamage && this.displayItem.quality > 0 && Random.value < ((ItemWeaponAsset)this.displayAsset).durability)
						{
							if (this.displayItem.quality > ((ItemWeaponAsset)this.displayAsset).wear)
							{
								Item displayItem = this.displayItem;
								displayItem.quality -= ((ItemWeaponAsset)this.displayAsset).wear;
							}
							else
							{
								this.displayItem.quality = 0;
							}
						}
						if (((ItemGunAsset)this.displayAsset).projectile == null)
						{
							float num9 = this.CalculateSpreadAngleRadians(quality);
							BarricadeManager.sendShootSentry(base.transform);
							float bulletDamageMultiplier = this.GetBulletDamageMultiplier(quality);
							byte pellets = this.attachments.magazineAsset.pellets;
							for (byte b = 0; b < pellets; b += 1)
							{
								EPlayerKill eplayerKill = EPlayerKill.NONE;
								uint num10 = 0U;
								Transform transform = null;
								float num11 = 0f;
								if (this.targetPlayer != null)
								{
									transform = this.targetPlayer.transform;
								}
								else if (this.targetZombie != null)
								{
									transform = this.targetZombie.transform;
								}
								else if (this.targetAnimal != null)
								{
									transform = this.targetAnimal.transform;
								}
								if (transform != null)
								{
									num11 = (transform.position - base.transform.position).magnitude;
								}
								float num12 = Mathf.Clamp01(num11 / ((ItemWeaponAsset)this.displayAsset).range);
								float num13 = 1f - num12;
								num13 *= this.CalculateChanceToHitSpreadMultiplier(num9);
								num13 *= 0.75f;
								if (transform == null || Random.value > num13)
								{
									Vector3 randomForwardVectorInCone = RandomEx.GetRandomForwardVectorInCone(num9);
									Vector3 direction = this.aimTransform.TransformDirection(randomForwardVectorInCone);
									RaycastInfo raycastInfo = DamageTool.raycast(new Ray(this.aimTransform.position, direction), ((ItemWeaponAsset)this.displayAsset).range, RayMasks.DAMAGE_SERVER);
									if (!(raycastInfo.transform == null))
									{
										Vector3 point = raycastInfo.point;
										Vector3 normal = raycastInfo.normal;
										string materialName = raycastInfo.materialName;
										Collider collider = raycastInfo.collider;
										DamageTool.ServerSpawnBulletImpact(point, normal, materialName, (collider != null) ? collider.transform : null, null, Provider.GatherClientConnectionsWithinSphere(raycastInfo.point, EffectManager.SMALL));
										if (raycastInfo.vehicle != null)
										{
											DamageTool.damage(raycastInfo.vehicle, false, Vector3.zero, false, ((ItemGunAsset)this.displayAsset).vehicleDamage, bulletDamageMultiplier, true, out eplayerKill, default(CSteamID), EDamageOrigin.Sentry);
										}
										else if (raycastInfo.transform != null)
										{
											if (raycastInfo.transform.CompareTag("Barricade"))
											{
												BarricadeDrop barricadeDrop = BarricadeDrop.FindByRootFast(raycastInfo.transform);
												if (barricadeDrop != null)
												{
													ItemBarricadeAsset asset = barricadeDrop.asset;
													if (asset != null && asset.canBeDamaged && (asset.isVulnerable || ((ItemWeaponAsset)this.displayAsset).isInvulnerable))
													{
														DamageTool.damage(raycastInfo.transform, false, ((ItemGunAsset)this.displayAsset).barricadeDamage, bulletDamageMultiplier, out eplayerKill, default(CSteamID), EDamageOrigin.Sentry);
													}
												}
											}
											else if (raycastInfo.transform.CompareTag("Structure"))
											{
												StructureDrop structureDrop = StructureDrop.FindByRootFast(raycastInfo.transform);
												if (structureDrop != null)
												{
													ItemStructureAsset asset2 = structureDrop.asset;
													if (asset2 != null && asset2.canBeDamaged && (asset2.isVulnerable || ((ItemWeaponAsset)this.displayAsset).isInvulnerable))
													{
														DamageTool.damage(raycastInfo.transform, false, raycastInfo.direction * Mathf.Ceil((float)this.attachments.magazineAsset.pellets / 2f), ((ItemGunAsset)this.displayAsset).structureDamage, bulletDamageMultiplier, out eplayerKill, default(CSteamID), EDamageOrigin.Sentry);
													}
												}
											}
											else if (raycastInfo.transform.CompareTag("Resource"))
											{
												byte x5;
												byte y;
												ushort index;
												if (ResourceManager.tryGetRegion(raycastInfo.transform, out x5, out y, out index))
												{
													ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x5, y, index);
													if (resourceSpawnpoint != null && !resourceSpawnpoint.isDead && ((ItemWeaponAsset)this.displayAsset).hasBladeID(resourceSpawnpoint.asset.bladeID))
													{
														DamageTool.damage(raycastInfo.transform, raycastInfo.direction * Mathf.Ceil((float)this.attachments.magazineAsset.pellets / 2f), ((ItemGunAsset)this.displayAsset).resourceDamage, bulletDamageMultiplier, 1f, out eplayerKill, out num10, default(CSteamID), EDamageOrigin.Sentry);
													}
												}
											}
											else if (raycastInfo.section < 255)
											{
												InteractableObjectRubble componentInParent = raycastInfo.transform.GetComponentInParent<InteractableObjectRubble>();
												if (componentInParent != null && componentInParent.IsSectionIndexValid(raycastInfo.section) && !componentInParent.isSectionDead(raycastInfo.section) && ((ItemWeaponAsset)this.displayAsset).hasBladeID(componentInParent.asset.rubbleBladeID) && (componentInParent.asset.rubbleIsVulnerable || ((ItemWeaponAsset)this.displayAsset).isInvulnerable))
												{
													DamageTool.damage(componentInParent.transform, raycastInfo.direction, raycastInfo.section, ((ItemGunAsset)this.displayAsset).objectDamage, bulletDamageMultiplier, out eplayerKill, out num10, default(CSteamID), EDamageOrigin.Sentry);
												}
											}
										}
										if (this.attachments.magazineAsset != null && this.attachments.magazineAsset.isExplosive)
										{
											Vector3 position2 = raycastInfo.point + raycastInfo.normal * 0.25f;
											UseableGun.DetonateExplosiveMagazine(this.attachments.magazineAsset, position2, null, ERagdollEffect.NONE);
										}
									}
								}
								else
								{
									Vector3 vector6 = Vector3.zero;
									if (this.targetPlayer != null)
									{
										vector6 = this.targetPlayer.look.aim.position;
									}
									else if (this.targetZombie != null)
									{
										vector6 = this.targetZombie.transform.position;
										switch (this.targetZombie.speciality)
										{
										case EZombieSpeciality.NORMAL:
											vector6 += new Vector3(0f, 1.75f, 0f);
											break;
										case EZombieSpeciality.MEGA:
											vector6 += new Vector3(0f, 2.625f, 0f);
											break;
										case EZombieSpeciality.CRAWLER:
											vector6 += new Vector3(0f, 0.25f, 0f);
											break;
										case EZombieSpeciality.SPRINTER:
											vector6 += new Vector3(0f, 1f, 0f);
											break;
										}
									}
									else if (this.targetAnimal != null)
									{
										vector6 = this.targetAnimal.transform.position + Vector3.up;
									}
									DamageTool.ServerSpawnBulletImpact(vector6, -this.aimTransform.forward, "Flesh_Dynamic", null, null, Provider.GatherClientConnectionsWithinSphere(vector6, EffectManager.SMALL));
									Vector3 direction2 = this.aimTransform.forward * Mathf.Ceil((float)this.attachments.magazineAsset.pellets / 2f);
									if (this.targetPlayer != null)
									{
										DamageTool.damage(this.targetPlayer, EDeathCause.SENTRY, ELimb.SPINE, base.owner, direction2, ((ItemGunAsset)this.displayAsset).playerDamageMultiplier, bulletDamageMultiplier, true, out eplayerKill, true, ERagdollEffect.NONE);
									}
									else if (this.targetZombie != null)
									{
										IDamageMultiplier zombieOrPlayerDamageMultiplier = ((ItemGunAsset)this.displayAsset).zombieOrPlayerDamageMultiplier;
										DamageZombieParameters parameters = DamageZombieParameters.make(this.targetZombie, direction2, zombieOrPlayerDamageMultiplier, ELimb.SPINE);
										parameters.times = bulletDamageMultiplier;
										parameters.allowBackstab = false;
										parameters.respectArmor = true;
										parameters.instigator = this;
										DamageTool.damageZombie(parameters, out eplayerKill, out num10);
									}
									else if (this.targetAnimal != null)
									{
										IDamageMultiplier animalOrPlayerDamageMultiplier = ((ItemGunAsset)this.displayAsset).animalOrPlayerDamageMultiplier;
										DamageAnimalParameters parameters2 = DamageAnimalParameters.make(this.targetAnimal, direction2, animalOrPlayerDamageMultiplier, ELimb.SPINE);
										parameters2.times = bulletDamageMultiplier;
										parameters2.instigator = this;
										DamageTool.damageAnimal(parameters2, out eplayerKill, out num10);
									}
									if (this.attachments.magazineAsset != null && this.attachments.magazineAsset.isExplosive)
									{
										Vector3 position3 = vector6 + this.aimTransform.forward * -0.25f;
										UseableGun.DetonateExplosiveMagazine(this.attachments.magazineAsset, position3, null, ERagdollEffect.NONE);
									}
								}
							}
						}
						base.rebuildState();
					}
				}
			}
			bool flag4 = Time.timeAsDouble - this.lastAlert < 1.0;
			if (flag4 != this.isAlert)
			{
				this.isAlert = flag4;
				if (!this.isAlert)
				{
					this.targetYaw = HousingConnections.GetModelYaw(base.transform);
				}
			}
			if (this.power != null)
			{
				if (this.isPowered)
				{
					if (this.isAlert)
					{
						this.lastDrift = Time.timeAsDouble;
						this.yaw = Mathf.LerpAngle(this.yaw, this.targetYaw, 4f * Time.deltaTime);
					}
					else
					{
						this.yaw = Mathf.LerpAngle(this.yaw, this.targetYaw + Mathf.Sin((float)(Time.timeAsDouble - this.lastDrift)) * 60f, 4f * Time.deltaTime);
					}
					this.pitch = Mathf.LerpAngle(this.pitch, this.targetPitch, 4f * Time.deltaTime);
					if (this.yawTransform != null)
					{
						this.yawTransform.rotation = Quaternion.Euler(-90f, 0f, this.yaw);
					}
					if (this.pitchTransform != null)
					{
						this.pitchTransform.localRotation = Quaternion.Euler(0f, -90f, this.pitch);
					}
				}
				if (this.onGameObject != null)
				{
					this.onGameObject.SetActive(this.isAlert && this.isPowered);
				}
				if (this.onModelGameObject != null)
				{
					this.onModelGameObject.SetActive(this.isAlert);
				}
				if (this.offGameObject != null)
				{
					this.offGameObject.SetActive(!this.isAlert && this.isPowered);
				}
				if (this.offModelGameObject != null)
				{
					this.offModelGameObject.SetActive(!this.isAlert);
				}
			}
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x00087338 File Offset: 0x00085538
		private void destroyEffects()
		{
			if (this.tracerEmitter != null)
			{
				EffectManager.DestroyIntoPool(this.tracerEmitter.gameObject);
				this.tracerEmitter = null;
			}
			if (this.muzzleEmitter != null)
			{
				EffectManager.DestroyIntoPool(this.muzzleEmitter.gameObject);
				this.muzzleEmitter = null;
			}
			this.muzzleLight = null;
			if (this.shellEmitter != null)
			{
				EffectManager.DestroyIntoPool(this.shellEmitter.gameObject);
				this.shellEmitter = null;
			}
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x000873BC File Offset: 0x000855BC
		private void OnDestroy()
		{
			this.destroyEffects();
			if (this.onMaterial != null)
			{
				Object.DestroyImmediate(this.onMaterial);
				this.onMaterial = null;
			}
			if (this.offMaterial != null)
			{
				Object.DestroyImmediate(this.offMaterial);
				this.offMaterial = null;
			}
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x0008740F File Offset: 0x0008560F
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveShoot()
		{
			this.shoot();
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x00087417 File Offset: 0x00085617
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveAlert(byte yaw, byte pitch)
		{
			this.alert(MeasurementTool.byteToAngle(yaw), MeasurementTool.byteToAngle(pitch));
		}

		/// <summary>
		/// Calculate damage multiplier for individual bullet.
		/// </summary>
		// Token: 0x06002290 RID: 8848 RVA: 0x0008742C File Offset: 0x0008562C
		private float GetBulletDamageMultiplier(float quality)
		{
			float num = (quality < 0.5f) ? (0.5f + quality) : 1f;
			if (this.attachments.magazineAsset != null)
			{
				num *= this.attachments.magazineAsset.ballisticDamageMultiplier;
			}
			if (this.attachments.sightAsset != null)
			{
				num *= this.attachments.sightAsset.ballisticDamageMultiplier;
			}
			if (this.attachments.tacticalAsset != null)
			{
				num *= this.attachments.tacticalAsset.ballisticDamageMultiplier;
			}
			if (this.attachments.barrelAsset != null)
			{
				num *= this.attachments.barrelAsset.ballisticDamageMultiplier;
			}
			if (this.attachments.gripAsset != null)
			{
				num *= this.attachments.gripAsset.ballisticDamageMultiplier;
			}
			return num;
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x000874F4 File Offset: 0x000856F4
		private float CalculateSpreadAngleRadians(float quality)
		{
			float num = ((ItemGunAsset)this.displayAsset).baseSpreadAngleRadians;
			num *= ((ItemGunAsset)this.displayAsset).spreadAim;
			num *= ((quality < 0.5f) ? (1f + (1f - quality * 2f)) : 1f);
			if (this.attachments.tacticalAsset != null && this.interact)
			{
				num *= this.attachments.tacticalAsset.spread;
			}
			if (this.attachments.gripAsset != null)
			{
				num *= this.attachments.gripAsset.spread;
			}
			if (this.attachments.barrelAsset != null)
			{
				num *= this.attachments.barrelAsset.spread;
			}
			if (this.attachments.magazineAsset != null)
			{
				num *= this.attachments.magazineAsset.spread;
			}
			return num;
		}

		/// <summary>
		/// Each shot has a percentage chance to hit the target. Higher values are more likely to hit. e.g., it
		/// decreases from 1.0 at point blank to 0.0 at the weapon's maximum range. This chance is affected by the
		/// gun's spread.
		/// </summary>
		// Token: 0x06002292 RID: 8850 RVA: 0x000875D3 File Offset: 0x000857D3
		private float CalculateChanceToHitSpreadMultiplier(float spreadAngleRadians)
		{
			if (spreadAngleRadians > 1.5707964f)
			{
				return 0f;
			}
			return Mathf.Cos(spreadAngleRadians);
		}

		// Token: 0x040010F3 RID: 4339
		private static List<Player> playersInRadius = new List<Player>();

		// Token: 0x040010F4 RID: 4340
		private static List<Zombie> zombiesInRadius = new List<Zombie>();

		// Token: 0x040010F5 RID: 4341
		private static List<Animal> animalsInRadius = new List<Animal>();

		// Token: 0x040010F6 RID: 4342
		public InteractablePower power;

		// Token: 0x040010F7 RID: 4343
		private bool hasWeapon;

		// Token: 0x040010F8 RID: 4344
		private bool interact;

		// Token: 0x040010F9 RID: 4345
		private Attachments attachments;

		// Token: 0x040010FA RID: 4346
		private AudioSource gunshotAudioSource;

		// Token: 0x040010FB RID: 4347
		private ParticleSystem shellEmitter;

		// Token: 0x040010FC RID: 4348
		private ParticleSystem muzzleEmitter;

		// Token: 0x040010FD RID: 4349
		private Light muzzleLight;

		// Token: 0x040010FE RID: 4350
		private ParticleSystem tracerEmitter;

		// Token: 0x040010FF RID: 4351
		private Transform yawTransform;

		// Token: 0x04001100 RID: 4352
		private Transform pitchTransform;

		// Token: 0x04001101 RID: 4353
		private Transform aimTransform;

		// Token: 0x04001102 RID: 4354
		private GameObject onGameObject;

		// Token: 0x04001103 RID: 4355
		private GameObject onModelGameObject;

		// Token: 0x04001104 RID: 4356
		private Material onMaterial;

		// Token: 0x04001105 RID: 4357
		private GameObject offGameObject;

		// Token: 0x04001106 RID: 4358
		private GameObject offModelGameObject;

		// Token: 0x04001107 RID: 4359
		private Material offMaterial;

		// Token: 0x04001108 RID: 4360
		private GameObject spotGameObject;

		// Token: 0x04001109 RID: 4361
		private Player targetPlayer;

		// Token: 0x0400110A RID: 4362
		private Zombie targetZombie;

		// Token: 0x0400110B RID: 4363
		private Animal targetAnimal;

		// Token: 0x0400110C RID: 4364
		private float targetYaw;

		// Token: 0x0400110D RID: 4365
		private float yaw;

		// Token: 0x0400110E RID: 4366
		private float targetPitch;

		// Token: 0x0400110F RID: 4367
		private float pitch;

		// Token: 0x04001110 RID: 4368
		private bool isAlert;

		// Token: 0x04001111 RID: 4369
		private double lastAlert;

		// Token: 0x04001112 RID: 4370
		private bool isFiring;

		// Token: 0x04001113 RID: 4371
		private double lastFire;

		// Token: 0x04001114 RID: 4372
		private float fireTime;

		// Token: 0x04001115 RID: 4373
		private bool isAiming;

		// Token: 0x04001116 RID: 4374
		private double lastAim;

		// Token: 0x04001117 RID: 4375
		private double lastScan;

		// Token: 0x04001118 RID: 4376
		private double lastDrift;

		// Token: 0x04001119 RID: 4377
		private double lastShot;

		// Token: 0x0400111B RID: 4379
		internal static readonly ClientInstanceMethod SendShoot = ClientInstanceMethod.Get(typeof(InteractableSentry), "ReceiveShoot");

		// Token: 0x0400111C RID: 4380
		internal static readonly ClientInstanceMethod<byte, byte> SendAlert = ClientInstanceMethod<byte, byte>.Get(typeof(InteractableSentry), "ReceiveAlert");

		// Token: 0x0400111D RID: 4381
		private bool hasInitializedSentryComponents;
	}
}
