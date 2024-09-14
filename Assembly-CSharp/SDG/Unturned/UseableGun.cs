using System;
using System.Collections.Generic;
using SDG.Framework.Water;
using SDG.NetTransport;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	// Token: 0x020007E5 RID: 2021
	public class UseableGun : Useable
	{
		/// <returns>Whether plugin allowed attachment.</returns>
		// Token: 0x060044E0 RID: 17632 RVA: 0x0018FF5C File Offset: 0x0018E15C
		private bool changeAttachmentRequested(UseableGun.ChangeAttachmentRequestHandler handler, Item oldItem, ItemJar newItem)
		{
			if (handler != null)
			{
				bool result = true;
				handler(base.player.equipment, this, oldItem, newItem, ref result);
				return result;
			}
			return true;
		}

		// Token: 0x140000EE RID: 238
		// (add) Token: 0x060044E1 RID: 17633 RVA: 0x0018FF88 File Offset: 0x0018E188
		// (remove) Token: 0x060044E2 RID: 17634 RVA: 0x0018FFBC File Offset: 0x0018E1BC
		public static event UseableGun.ChangeAttachmentRequestHandler onChangeSightRequested;

		// Token: 0x060044E3 RID: 17635 RVA: 0x0018FFEF File Offset: 0x0018E1EF
		private bool changeSightRequested(Item oldItem, ItemJar newItem)
		{
			return this.changeAttachmentRequested(UseableGun.onChangeSightRequested, oldItem, newItem);
		}

		// Token: 0x140000EF RID: 239
		// (add) Token: 0x060044E4 RID: 17636 RVA: 0x00190000 File Offset: 0x0018E200
		// (remove) Token: 0x060044E5 RID: 17637 RVA: 0x00190034 File Offset: 0x0018E234
		public static event UseableGun.ChangeAttachmentRequestHandler onChangeTacticalRequested;

		// Token: 0x060044E6 RID: 17638 RVA: 0x00190067 File Offset: 0x0018E267
		private bool changeTacticalRequested(Item oldItem, ItemJar newItem)
		{
			return this.changeAttachmentRequested(UseableGun.onChangeTacticalRequested, oldItem, newItem);
		}

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x060044E7 RID: 17639 RVA: 0x00190078 File Offset: 0x0018E278
		// (remove) Token: 0x060044E8 RID: 17640 RVA: 0x001900AC File Offset: 0x0018E2AC
		public static event UseableGun.ChangeAttachmentRequestHandler onChangeGripRequested;

		// Token: 0x060044E9 RID: 17641 RVA: 0x001900DF File Offset: 0x0018E2DF
		private bool changeGripRequested(Item oldItem, ItemJar newItem)
		{
			return this.changeAttachmentRequested(UseableGun.onChangeGripRequested, oldItem, newItem);
		}

		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x060044EA RID: 17642 RVA: 0x001900F0 File Offset: 0x0018E2F0
		// (remove) Token: 0x060044EB RID: 17643 RVA: 0x00190124 File Offset: 0x0018E324
		public static event UseableGun.ChangeAttachmentRequestHandler onChangeBarrelRequested;

		// Token: 0x060044EC RID: 17644 RVA: 0x00190157 File Offset: 0x0018E357
		private bool changeBarrelRequested(Item oldItem, ItemJar newItem)
		{
			return this.changeAttachmentRequested(UseableGun.onChangeBarrelRequested, oldItem, newItem);
		}

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x060044ED RID: 17645 RVA: 0x00190168 File Offset: 0x0018E368
		// (remove) Token: 0x060044EE RID: 17646 RVA: 0x0019019C File Offset: 0x0018E39C
		public static event UseableGun.ChangeAttachmentRequestHandler onChangeMagazineRequested;

		// Token: 0x060044EF RID: 17647 RVA: 0x001901CF File Offset: 0x0018E3CF
		private bool changeMagazineRequested(Item oldItem, ItemJar newItem)
		{
			return this.changeAttachmentRequested(UseableGun.onChangeMagazineRequested, oldItem, newItem);
		}

		/// <summary>
		/// Plugin-only event when bullet is fired on server.
		/// </summary>
		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x060044F0 RID: 17648 RVA: 0x001901E0 File Offset: 0x0018E3E0
		// (remove) Token: 0x060044F1 RID: 17649 RVA: 0x00190214 File Offset: 0x0018E414
		public static event UseableGun.BulletSpawnedHandler onBulletSpawned;

		/// <summary>
		/// Plugin-only event when bullet hit is received from client.
		/// </summary>
		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x060044F2 RID: 17650 RVA: 0x00190248 File Offset: 0x0018E448
		// (remove) Token: 0x060044F3 RID: 17651 RVA: 0x0019027C File Offset: 0x0018E47C
		public static event UseableGun.BulletHitHandler onBulletHit;

		/// <summary>
		/// Plugin-only event when projectile is spawned on server.
		/// </summary>
		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x060044F4 RID: 17652 RVA: 0x001902B0 File Offset: 0x0018E4B0
		// (remove) Token: 0x060044F5 RID: 17653 RVA: 0x001902E4 File Offset: 0x0018E4E4
		public static event UseableGun.ProjectileSpawnedHandler onProjectileSpawned;

		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x060044F6 RID: 17654 RVA: 0x00190318 File Offset: 0x0018E518
		// (remove) Token: 0x060044F7 RID: 17655 RVA: 0x0019034C File Offset: 0x0018E54C
		public static event Action<UseableGun> OnReloading_Global;

		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x060044F8 RID: 17656 RVA: 0x00190380 File Offset: 0x0018E580
		// (remove) Token: 0x060044F9 RID: 17657 RVA: 0x001903B4 File Offset: 0x0018E5B4
		public static event Action<UseableGun> OnAimingChanged_Global;

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x060044FA RID: 17658 RVA: 0x001903E7 File Offset: 0x0018E5E7
		// (set) Token: 0x060044FB RID: 17659 RVA: 0x001903EF File Offset: 0x0018E5EF
		public bool isAiming { get; protected set; }

		/// <summary>
		/// Should stat modifiers from the current tactical attachment be used?
		/// </summary>
		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060044FC RID: 17660 RVA: 0x001903F8 File Offset: 0x0018E5F8
		private bool shouldEnableTacticalStats
		{
			get
			{
				ItemTacticalAsset tacticalAsset = this.thirdAttachments.tacticalAsset;
				return tacticalAsset != null && ((!tacticalAsset.isLaser && !tacticalAsset.isLight && !tacticalAsset.isRangefinder) || this.interact);
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060044FD RID: 17661 RVA: 0x00190436 File Offset: 0x0018E636
		public ItemGunAsset equippedGunAsset
		{
			get
			{
				return base.player.equipment.asset as ItemGunAsset;
			}
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x0019044D File Offset: 0x0018E64D
		protected VehicleTurretEventHook GetVehicleTurretEventHook()
		{
			if (!base.player.equipment.isTurret)
			{
				return null;
			}
			Passenger vehicleSeat = base.player.movement.getVehicleSeat();
			if (vehicleSeat == null)
			{
				return null;
			}
			return vehicleSeat.turretEventHook;
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x0019047E File Offset: 0x0018E67E
		[Obsolete]
		public void askFiremode(CSteamID steamID, byte id)
		{
			this.ReceiveChangeFiremode((EFiremode)id);
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x00190488 File Offset: 0x0018E688
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askFiremode")]
		public void ReceiveChangeFiremode(EFiremode newFiremode)
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isFired)
			{
				return;
			}
			if (this.isReloading || this.isHammering || this.isUnjamming || this.needsRechamber)
			{
				return;
			}
			if (base.player.equipment.asset == null)
			{
				return;
			}
			if (newFiremode == EFiremode.SAFETY)
			{
				if (this.equippedGunAsset.hasSafety)
				{
					this.firemode = newFiremode;
				}
			}
			else if (newFiremode == EFiremode.SEMI)
			{
				if (this.equippedGunAsset.hasSemi)
				{
					this.firemode = newFiremode;
				}
			}
			else if (newFiremode == EFiremode.AUTO)
			{
				if (this.equippedGunAsset.hasAuto)
				{
					this.firemode = newFiremode;
				}
			}
			else if (newFiremode == EFiremode.BURST && this.equippedGunAsset.hasBurst)
			{
				this.firemode = newFiremode;
			}
			base.player.equipment.state[11] = (byte)this.firemode;
			base.player.equipment.sendUpdateState();
			EffectManager.TriggerFiremodeEffect(base.transform.position);
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x00190584 File Offset: 0x0018E784
		public void askInteractGun()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isFired)
			{
				return;
			}
			if (this.isReloading || this.isHammering || this.isUnjamming || this.needsRechamber)
			{
				return;
			}
			if (this.thirdAttachments.tacticalAsset == null)
			{
				return;
			}
			if (this.thirdAttachments.tacticalAsset.isMelee)
			{
				if (!this.isSprinting && (!base.player.movement.isSafe || !base.player.movement.isSafeInfo.noWeapons) && this.firemode != EFiremode.SAFETY)
				{
					this.isJabbing = true;
					return;
				}
			}
			else
			{
				this.interact = !this.interact;
				base.player.equipment.state[12] = (this.interact ? 1 : 0);
				base.player.equipment.sendUpdateState();
				EffectManager.TriggerFiremodeEffect(base.transform.position);
			}
		}

		/// <summary>
		/// Original barrel and magazine assets are supplied because they may have already been deleted. Barrel is only
		/// valid if quality was greater than zero.
		/// </summary>
		// Token: 0x06004502 RID: 17666 RVA: 0x00190684 File Offset: 0x0018E884
		private void project(Vector3 origin, Vector3 direction, ItemBarrelAsset barrelAsset, ItemMagazineAsset magazineAsset)
		{
			if (this.gunshotAudioSource != null)
			{
				this.playGunshot();
			}
			if (barrelAsset == null || !barrelAsset.isBraked)
			{
				if (this.firstMuzzleEmitter != null && base.player.look.perspective == EPlayerPerspective.FIRST && !this.equippedGunAsset.isTurret)
				{
					this.firstMuzzleEmitter.Emit(1);
					Light component = this.firstMuzzleEmitter.GetComponent<Light>();
					if (component != null)
					{
						component.enabled = true;
					}
					if (this.firstFakeLight != null)
					{
						component = this.firstFakeLight.GetComponent<Light>();
						if (component != null)
						{
							component.enabled = true;
						}
					}
				}
				if (this.thirdMuzzleEmitter != null && (!base.channel.IsLocalPlayer || base.player.look.perspective == EPlayerPerspective.THIRD || this.equippedGunAsset.isTurret))
				{
					this.thirdMuzzleEmitter.Emit(1);
					Light component2 = this.thirdMuzzleEmitter.GetComponent<Light>();
					if (component2 != null)
					{
						component2.enabled = true;
					}
				}
			}
			float num = 1f;
			float num2 = 1f;
			float num3 = 1f;
			if (magazineAsset != null)
			{
				num *= magazineAsset.projectileDamageMultiplier;
				num2 *= magazineAsset.projectileBlastRadiusMultiplier;
				num3 *= magazineAsset.projectileLaunchForceMultiplier;
			}
			Transform transform = Object.Instantiate<GameObject>(this.equippedGunAsset.projectile).transform;
			transform.name = "Projectile";
			EffectManager.RegisterDebris(transform.gameObject);
			transform.position = origin;
			transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);
			Rigidbody component3 = transform.GetComponent<Rigidbody>();
			if (component3 != null)
			{
				component3.AddForce(direction * this.equippedGunAsset.ballisticForce * num3);
				component3.collisionDetectionMode = CollisionDetectionMode.Continuous;
			}
			if (base.channel.IsLocalPlayer && transform.GetComponent<AudioSource>() != null)
			{
				transform.GetComponent<AudioSource>().maxDistance = 512f;
			}
			if (this.equippedGunAsset.action == EAction.Bolt || this.equippedGunAsset.action == EAction.Pump)
			{
				this.needsRechamber = true;
			}
			Rocket rocket = transform.gameObject.AddComponent<Rocket>();
			rocket.ignoreTransform = base.transform;
			if (Provider.isServer)
			{
				rocket.killer = base.channel.owner.playerID.steamID;
				rocket.range = this.equippedGunAsset.range * num2;
				rocket.playerDamage = this.equippedGunAsset.playerDamageMultiplier.damage * num;
				rocket.zombieDamage = this.equippedGunAsset.zombieDamageMultiplier.damage * num;
				rocket.animalDamage = this.equippedGunAsset.animalDamageMultiplier.damage * num;
				rocket.barricadeDamage = this.equippedGunAsset.barricadeDamage * num;
				rocket.structureDamage = this.equippedGunAsset.structureDamage * num;
				rocket.vehicleDamage = this.equippedGunAsset.vehicleDamage * num;
				rocket.resourceDamage = this.equippedGunAsset.resourceDamage * num;
				rocket.objectDamage = this.equippedGunAsset.objectDamage * num;
				if (magazineAsset != null && !magazineAsset.IsExplosionEffectRefNull())
				{
					rocket.explosionEffectGuid = magazineAsset.explosionEffectGuid;
					rocket.explosion = magazineAsset.explosion;
				}
				else
				{
					rocket.explosionEffectGuid = this.equippedGunAsset.projectileExplosionEffectGuid;
					rocket.explosion = this.equippedGunAsset.explosion;
				}
				rocket.penetrateBuildables = this.equippedGunAsset.projectilePenetrateBuildables;
				rocket.explosionLaunchSpeed = this.equippedGunAsset.projectileExplosionLaunchSpeed;
				rocket.ragdollEffect = base.player.equipment.getUseableRagdollEffect();
			}
			Object.Destroy(transform.gameObject, this.equippedGunAsset.projectileLifespan);
			this.lastShot = Time.realtimeSinceStartup;
			UseableGun.ProjectileSpawnedHandler projectileSpawnedHandler = UseableGun.onProjectileSpawned;
			if (projectileSpawnedHandler != null)
			{
				projectileSpawnedHandler(this, transform.gameObject);
			}
			this.InvokeModHookShotFiredEvents();
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x00190A7B File Offset: 0x0018EC7B
		[Obsolete]
		public void askProject(CSteamID steamID, Vector3 origin, Vector3 direction, ushort barrelId, ushort magazineId)
		{
			this.ReceivePlayProject(origin, direction, barrelId, magazineId);
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x00190A8C File Offset: 0x0018EC8C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askProject")]
		public void ReceivePlayProject(Vector3 origin, Vector3 direction, ushort barrelId, ushort magazineId)
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				ItemBarrelAsset barrelAsset = Assets.find(EAssetType.ITEM, barrelId) as ItemBarrelAsset;
				ItemMagazineAsset magazineAsset = Assets.find(EAssetType.ITEM, magazineId) as ItemMagazineAsset;
				this.project(origin, direction, barrelAsset, magazineAsset);
			}
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x00190AD0 File Offset: 0x0018ECD0
		private void trace(Vector3 pos, Vector3 dir)
		{
			if (this.tracerEmitter == null)
			{
				return;
			}
			if (this.thirdAttachments.barrelModel != null && this.thirdAttachments.barrelAsset.isBraked && base.player.equipment.state[16] > 0)
			{
				return;
			}
			this.tracerEmitter.transform.position = pos;
			this.tracerEmitter.transform.rotation = Quaternion.LookRotation(dir);
			this.tracerEmitter.Emit(1);
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x00190B5C File Offset: 0x0018ED5C
		private void playGunshot()
		{
			AudioClip shoot = this.equippedGunAsset.shoot;
			float num = 1f;
			float num2 = this.equippedGunAsset.gunshotRolloffDistance;
			if (this.thirdAttachments.barrelAsset != null && base.player.equipment.state[16] > 0)
			{
				if (this.thirdAttachments.barrelAsset.shoot != null)
				{
					shoot = this.thirdAttachments.barrelAsset.shoot;
				}
				num *= this.thirdAttachments.barrelAsset.volume;
				num2 *= this.thirdAttachments.barrelAsset.gunshotRolloffDistanceMultiplier;
			}
			this.gunshotAudioSource.clip = shoot;
			this.gunshotAudioSource.volume = num;
			this.gunshotAudioSource.maxDistance = num2;
			this.gunshotAudioSource.pitch = Random.Range(0.975f, 1.025f);
			if (this.gunshotAudioSource.clip != null)
			{
				this.gunshotAudioSource.PlayOneShot(this.gunshotAudioSource.clip);
			}
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x00190C60 File Offset: 0x0018EE60
		private void shoot()
		{
			if (this.gunshotAudioSource != null)
			{
				this.playGunshot();
			}
			if (this.equippedGunAsset.action == EAction.Trigger || this.equippedGunAsset.action == EAction.Minigun)
			{
				if (this.firstShellEmitter != null && base.player.look.perspective == EPlayerPerspective.FIRST && !this.equippedGunAsset.isTurret)
				{
					this.firstShellEmitter.Emit(1);
				}
				if (this.thirdShellEmitter != null)
				{
					this.thirdShellEmitter.Emit(1);
				}
			}
			if (this.thirdAttachments.barrelModel == null || !this.thirdAttachments.barrelAsset.isBraked || base.player.equipment.state[16] == 0)
			{
				if (this.firstMuzzleEmitter != null && base.player.look.perspective == EPlayerPerspective.FIRST && !this.equippedGunAsset.isTurret)
				{
					this.firstMuzzleEmitter.Emit(1);
					this.firstMuzzleEmitter.GetComponent<Light>().enabled = true;
					if (this.firstFakeLight != null)
					{
						this.firstFakeLight.GetComponent<Light>().enabled = true;
					}
				}
				if (this.thirdMuzzleEmitter != null && (!base.channel.IsLocalPlayer || base.player.look.perspective == EPlayerPerspective.THIRD || this.equippedGunAsset.isTurret))
				{
					this.thirdMuzzleEmitter.Emit(1);
					this.thirdMuzzleEmitter.GetComponent<Light>().enabled = true;
				}
			}
			if (!base.channel.IsLocalPlayer)
			{
				if (this.equippedGunAsset.range < 32f)
				{
					this.trace(base.player.look.aim.position + base.player.look.aim.forward * 32f, base.player.look.aim.forward);
				}
				else
				{
					this.trace(base.player.look.aim.position + base.player.look.aim.forward * Random.Range(32f, Mathf.Min(64f, this.equippedGunAsset.range)), base.player.look.aim.forward);
				}
			}
			this.lastShot = Time.realtimeSinceStartup;
			if (this.equippedGunAsset.action == EAction.Bolt || this.equippedGunAsset.action == EAction.Pump)
			{
				this.needsRechamber = true;
			}
			if (this.thirdAttachments.barrelAsset != null && this.thirdAttachments.barrelAsset.durability > 0)
			{
				if (this.thirdAttachments.barrelAsset.durability > base.player.equipment.state[16])
				{
					base.player.equipment.state[16] = 0;
				}
				else
				{
					byte[] state = base.player.equipment.state;
					int num = 16;
					state[num] -= this.thirdAttachments.barrelAsset.durability;
				}
				if (base.channel.IsLocalPlayer || Provider.isServer)
				{
					base.player.equipment.updateState();
				}
			}
			this.InvokeModHookShotFiredEvents();
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x00190FB8 File Offset: 0x0018F1B8
		[Obsolete]
		public void askShoot(CSteamID steamID)
		{
			this.ReceivePlayShoot();
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x00190FC0 File Offset: 0x0018F1C0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askShoot")]
		public void ReceivePlayShoot()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.shoot();
			}
		}

		/// <summary>
		/// Called on server and owning client.
		/// </summary>
		// Token: 0x0600450A RID: 17674 RVA: 0x00190FDC File Offset: 0x0018F1DC
		private void fire()
		{
			float num = (float)base.player.equipment.quality / 100f;
			if (!this.equippedGunAsset.infiniteAmmo)
			{
				if (this.ammo < this.equippedGunAsset.ammoPerShot)
				{
					throw new Exception("Insufficient ammo");
				}
				this.ammo -= this.equippedGunAsset.ammoPerShot;
				if (this.equippedGunAsset.action != EAction.String)
				{
					base.player.equipment.state[10] = this.ammo;
					base.player.equipment.updateState();
				}
			}
			if (base.channel.IsLocalPlayer && this.ammo < this.equippedGunAsset.ammoPerShot)
			{
				PlayerUI.message(EPlayerMessage.RELOAD, "", 2f);
			}
			if (!this.isAiming)
			{
				base.player.equipment.uninspect();
			}
			if (Provider.isServer)
			{
				UseableGun.SendPlayShoot.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsWithinSphereExcludingOwner(base.transform.position, EffectManager.INSANE));
				this.lastShot = Time.realtimeSinceStartup;
				if (!base.channel.IsLocalPlayer)
				{
					this.InvokeModHookShotFiredEvents();
				}
				if (this.equippedGunAsset.action == EAction.Bolt || this.equippedGunAsset.action == EAction.Pump)
				{
					this.needsRechamber = true;
				}
				if (this.thirdAttachments.barrelAsset == null || !this.thirdAttachments.barrelAsset.isSilenced || base.player.equipment.state[16] == 0)
				{
					AlertTool.alert(base.transform.position, this.equippedGunAsset.alertRadius);
				}
				if (Provider.modeConfigData.Items.ShouldWeaponTakeDamage && base.player.equipment.quality > 0 && Random.value < ((ItemWeaponAsset)base.player.equipment.asset).durability)
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
			}
			if (base.channel.IsLocalPlayer)
			{
				if (!base.player.look.IsLocallyUsingFreecam && base.player.look.perspective == EPlayerPerspective.THIRD)
				{
					RaycastHit raycastHit;
					Physics.Raycast(new Ray(MainCamera.instance.transform.position, MainCamera.instance.transform.forward), out raycastHit, 512f, RayMasks.DAMAGE_CLIENT);
					if (raycastHit.transform != null)
					{
						if (Vector3.Dot(raycastHit.point - base.player.look.aim.position, MainCamera.instance.transform.forward) > 0f)
						{
							base.player.look.aim.rotation = Quaternion.LookRotation(raycastHit.point - base.player.look.aim.position);
						}
					}
					else
					{
						base.player.look.aim.rotation = Quaternion.LookRotation(MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 512f - base.player.look.aim.position);
					}
				}
				if (this.equippedGunAsset.projectile == null)
				{
					Quaternion quaternion = base.player.look.aim.rotation;
					if (base.player.look.perspective == EPlayerPerspective.FIRST)
					{
						Quaternion rhs = Quaternion.Euler(base.player.animator.recoilViewmodelCameraRotation.currentPosition);
						quaternion *= rhs;
					}
					float halfAngleRadians = this.CalculateSpreadAngleRadians(num, this.GetSimulationAimAlpha());
					byte b = (this.thirdAttachments.magazineAsset != null) ? this.thirdAttachments.magazineAsset.pellets : 1;
					for (byte b2 = 0; b2 < b; b2 += 1)
					{
						BulletInfo bulletInfo = new BulletInfo();
						bulletInfo.origin = base.player.look.aim.position;
						bulletInfo.position = bulletInfo.origin;
						Vector3 a = quaternion * RandomEx.GetRandomForwardVectorInCone(halfAngleRadians);
						bulletInfo.velocity = a * this.equippedGunAsset.muzzleVelocity;
						bulletInfo.pellet = b2;
						bulletInfo.quality = num;
						bulletInfo.barrelAsset = this.thirdAttachments.barrelAsset;
						bulletInfo.magazineAsset = this.thirdAttachments.magazineAsset;
						this.bullets.Add(bulletInfo);
						int num2;
						if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Shot", out num2))
						{
							Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Shot", num2 + 1);
						}
					}
				}
				else
				{
					Vector3 forward = base.player.look.aim.forward;
					RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, forward), 512f, RayMasks.DAMAGE_CLIENT, base.player);
					if (raycastInfo.transform != null)
					{
						base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Gun);
					}
					Vector3 vector = base.player.look.aim.position;
					RaycastHit raycastHit2;
					if (!Physics.Raycast(new Ray(vector, forward), out raycastHit2, 1f, RayMasks.DAMAGE_SERVER))
					{
						vector += forward;
					}
					this.project(vector, forward, this.thirdAttachments.barrelAsset, this.thirdAttachments.magazineAsset);
					int num3;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Shot", out num3))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Shot", num3 + 1);
					}
				}
				float num4 = Random.Range(this.equippedGunAsset.recoilMin_x, this.equippedGunAsset.recoilMax_x) * ((num < 0.5f) ? (1f + (1f - num * 2f)) : 1f);
				float num5 = Random.Range(this.equippedGunAsset.recoilMin_y, this.equippedGunAsset.recoilMax_y) * ((num < 0.5f) ? (1f + (1f - num * 2f)) : 1f);
				float num6 = Random.Range(this.equippedGunAsset.shakeMin_x, this.equippedGunAsset.shakeMax_x);
				float num7 = Random.Range(this.equippedGunAsset.shakeMin_y, this.equippedGunAsset.shakeMax_y);
				float num8 = Random.Range(this.equippedGunAsset.shakeMin_z, this.equippedGunAsset.shakeMax_z);
				num4 *= 1f - base.player.skills.mastery(0, 1) * 0.5f;
				num5 *= 1f - base.player.skills.mastery(0, 1) * 0.5f;
				if (this.isAiming)
				{
					num4 *= this.equippedGunAsset.aimingRecoilMultiplier;
					num5 *= this.equippedGunAsset.aimingRecoilMultiplier;
				}
				if (this.thirdAttachments.sightAsset != null)
				{
					if (this.isAiming)
					{
						num4 *= this.thirdAttachments.sightAsset.aimingRecoilMultiplier;
						num5 *= this.thirdAttachments.sightAsset.aimingRecoilMultiplier;
					}
					num4 *= this.thirdAttachments.sightAsset.recoil_x;
					num5 *= this.thirdAttachments.sightAsset.recoil_y;
					num6 *= this.thirdAttachments.sightAsset.shake;
					num7 *= this.thirdAttachments.sightAsset.shake;
					num8 *= this.thirdAttachments.sightAsset.shake;
				}
				if (this.thirdAttachments.tacticalAsset != null && this.shouldEnableTacticalStats)
				{
					if (this.isAiming)
					{
						num4 *= this.thirdAttachments.tacticalAsset.aimingRecoilMultiplier;
						num5 *= this.thirdAttachments.tacticalAsset.aimingRecoilMultiplier;
					}
					num4 *= this.thirdAttachments.tacticalAsset.recoil_x;
					num5 *= this.thirdAttachments.tacticalAsset.recoil_y;
					num6 *= this.thirdAttachments.tacticalAsset.shake;
					num7 *= this.thirdAttachments.tacticalAsset.shake;
					num8 *= this.thirdAttachments.tacticalAsset.shake;
				}
				if (this.thirdAttachments.gripAsset != null && (!this.thirdAttachments.gripAsset.ShouldOnlyAffectAimWhileProne || base.player.stance.stance == EPlayerStance.PRONE))
				{
					if (this.isAiming)
					{
						num4 *= this.thirdAttachments.gripAsset.aimingRecoilMultiplier;
						num5 *= this.thirdAttachments.gripAsset.aimingRecoilMultiplier;
					}
					num4 *= this.thirdAttachments.gripAsset.recoil_x;
					num5 *= this.thirdAttachments.gripAsset.recoil_y;
					num6 *= this.thirdAttachments.gripAsset.shake;
					num7 *= this.thirdAttachments.gripAsset.shake;
					num8 *= this.thirdAttachments.gripAsset.shake;
				}
				if (this.thirdAttachments.barrelAsset != null)
				{
					if (this.isAiming)
					{
						num4 *= this.thirdAttachments.barrelAsset.aimingRecoilMultiplier;
						num5 *= this.thirdAttachments.barrelAsset.aimingRecoilMultiplier;
					}
					num4 *= this.thirdAttachments.barrelAsset.recoil_x;
					num5 *= this.thirdAttachments.barrelAsset.recoil_y;
					num6 *= this.thirdAttachments.barrelAsset.shake;
					num7 *= this.thirdAttachments.barrelAsset.shake;
					num8 *= this.thirdAttachments.barrelAsset.shake;
				}
				if (this.thirdAttachments.magazineAsset != null)
				{
					if (this.isAiming)
					{
						num4 *= this.thirdAttachments.magazineAsset.aimingRecoilMultiplier;
						num5 *= this.thirdAttachments.magazineAsset.aimingRecoilMultiplier;
					}
					num4 *= this.thirdAttachments.magazineAsset.recoil_x;
					num5 *= this.thirdAttachments.magazineAsset.recoil_y;
					num6 *= this.thirdAttachments.magazineAsset.shake;
					num7 *= this.thirdAttachments.magazineAsset.shake;
					num8 *= this.thirdAttachments.magazineAsset.shake;
				}
				this.applyRecoilMagnitudeModifiers(ref num4);
				this.applyRecoilMagnitudeModifiers(ref num5);
				if (base.player.stance.stance == EPlayerStance.CROUCH)
				{
					num6 *= UseableGun.SHAKE_CROUCH;
					num7 *= UseableGun.SHAKE_CROUCH;
					num8 *= UseableGun.SHAKE_CROUCH;
				}
				else if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					num6 *= UseableGun.SHAKE_PRONE;
					num7 *= UseableGun.SHAKE_PRONE;
					num8 *= UseableGun.SHAKE_PRONE;
				}
				if (base.player.look.perspective == EPlayerPerspective.THIRD)
				{
					num4 *= Provider.modeConfigData.Gameplay.ThirdPerson_RecoilMultiplier;
					num5 *= Provider.modeConfigData.Gameplay.ThirdPerson_RecoilMultiplier;
				}
				base.player.look.recoil(num4, num5, this.equippedGunAsset.recover_x, this.equippedGunAsset.recover_y);
				base.player.animator.AddRecoilViewmodelCameraOffset(num6, num7, num8);
				base.player.animator.AddRecoilViewmodelCameraRotation(num4, num5);
				this.updateInfo();
				if (this.equippedGunAsset.projectile == null)
				{
					this.shoot();
				}
			}
			if (Provider.isServer)
			{
				if (!base.channel.IsLocalPlayer && this.thirdAttachments.barrelAsset != null && this.thirdAttachments.barrelAsset.durability > 0)
				{
					if (this.thirdAttachments.barrelAsset.durability > base.player.equipment.state[16])
					{
						base.player.equipment.state[16] = 0;
					}
					else
					{
						byte[] state = base.player.equipment.state;
						int num9 = 16;
						state[num9] -= this.thirdAttachments.barrelAsset.durability;
					}
					base.player.equipment.updateState();
				}
				this.equippedGunAsset.GrantShootQuestRewards(base.player);
				if (this.equippedGunAsset.projectile == null)
				{
					byte b3 = (this.thirdAttachments.magazineAsset != null) ? this.thirdAttachments.magazineAsset.pellets : 1;
					for (byte b4 = 0; b4 < b3; b4 += 1)
					{
						BulletInfo bulletInfo2;
						if (base.channel.IsLocalPlayer)
						{
							bulletInfo2 = this.bullets[this.bullets.Count - (int)b3 + (int)b4];
						}
						else
						{
							bulletInfo2 = new BulletInfo();
							bulletInfo2.origin = base.player.look.aim.position;
							bulletInfo2.ApproximatePlayerAimDirection = base.player.look.aim.forward;
							bulletInfo2.position = bulletInfo2.origin;
							bulletInfo2.pellet = b4;
							bulletInfo2.quality = num;
							bulletInfo2.barrelAsset = this.thirdAttachments.barrelAsset;
							bulletInfo2.magazineAsset = this.thirdAttachments.magazineAsset;
							this.bullets.Add(bulletInfo2);
							UseableGun.BulletSpawnedHandler bulletSpawnedHandler = UseableGun.onBulletSpawned;
							if (bulletSpawnedHandler != null)
							{
								bulletSpawnedHandler(this, bulletInfo2);
							}
						}
						if (this.thirdAttachments.magazineAsset != null && this.thirdAttachments.magazineAsset.isExplosive)
						{
							if (this.equippedGunAsset.action == EAction.String)
							{
								base.player.equipment.state[8] = 0;
								base.player.equipment.state[9] = 0;
								base.player.equipment.state[10] = 0;
								base.player.equipment.state[17] = 0;
								base.player.equipment.sendUpdateState();
							}
						}
						else if (this.equippedGunAsset.action == EAction.String)
						{
							if (base.player.equipment.state[17] > 0)
							{
								byte b5 = (this.thirdAttachments.magazineAsset != null) ? this.thirdAttachments.magazineAsset.stuck : 1;
								if (base.player.equipment.state[17] > b5)
								{
									byte[] state2 = base.player.equipment.state;
									int num10 = 17;
									state2[num10] -= b5;
								}
								else
								{
									base.player.equipment.state[17] = 0;
								}
								bulletInfo2.dropID = this.thirdAttachments.magazineID;
								bulletInfo2.dropAmount = base.player.equipment.state[10];
								bulletInfo2.dropQuality = base.player.equipment.state[17];
							}
							base.player.equipment.state[8] = 0;
							base.player.equipment.state[9] = 0;
							base.player.equipment.state[10] = 0;
							base.player.equipment.sendUpdateState();
						}
					}
				}
				else
				{
					ItemBarrelAsset itemBarrelAsset = (base.player.equipment.state[16] > 0) ? this.thirdAttachments.barrelAsset : null;
					ItemMagazineAsset magazineAsset = this.thirdAttachments.magazineAsset;
					if (base.player.input.hasInputs())
					{
						InputInfo input = base.player.input.getInput(false, ERaycastInfoUsage.Gun);
						if (input != null && input.transform != null)
						{
							base.player.look.aim.LookAt(input.point);
						}
					}
					if (this.ammo == 0 && this.equippedGunAsset.shouldDeleteEmptyMagazines)
					{
						base.player.equipment.state[8] = 0;
						base.player.equipment.state[9] = 0;
						base.player.equipment.state[10] = 0;
						base.player.equipment.sendUpdateState();
					}
					if (!base.channel.IsLocalPlayer)
					{
						Vector3 vector2 = base.player.look.aim.position;
						Vector3 forward2 = base.player.look.aim.forward;
						RaycastHit raycastHit3;
						if (!Physics.Raycast(new Ray(vector2, forward2), out raycastHit3, 1f, RayMasks.DAMAGE_SERVER))
						{
							vector2 += forward2;
						}
						this.project(vector2, forward2, itemBarrelAsset, magazineAsset);
						UseableGun.SendPlayProject.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner(), vector2, forward2, (itemBarrelAsset != null) ? itemBarrelAsset.id : 0, (magazineAsset != null) ? magazineAsset.id : 0);
					}
					base.player.life.markAggressive(false, true);
				}
			}
			if (this.equippedGunAsset.canEverJam && Provider.isServer && num < this.equippedGunAsset.jamQualityThreshold)
			{
				float t = 1f - num / this.equippedGunAsset.jamQualityThreshold;
				float num11 = Mathf.Lerp(0f, this.equippedGunAsset.jamMaxChance, t);
				if (Random.value < num11)
				{
					UseableGun.SendPlayChamberJammed.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), this.ammo);
				}
			}
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x001921B8 File Offset: 0x001903B8
		private void jab()
		{
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
			if (base.channel.IsLocalPlayer)
			{
				AudioClip audioClip = UseableGun.jabClipRef.loadAsset(true);
				if (audioClip == null)
				{
					UnturnedLog.warn("Missing built-in bayonet audio");
				}
				base.player.animator.AddBayonetViewmodelCameraOffset(0f, 0f, 0.8f);
				base.player.playSound(audioClip, 0.5f);
				int num;
				if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Shot", out num))
				{
					Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Shot", num + 1);
				}
				RaycastInfo raycastInfo = DamageTool.raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), 2f, RayMasks.DAMAGE_CLIENT, base.player);
				if (raycastInfo.player != null && (DamageTool.isPlayerAllowedToDamagePlayer(base.player, raycastInfo.player) || this.equippedGunAsset.bypassAllowedToDamagePlayer))
				{
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num + 1);
					}
					if (raycastInfo.limb == ELimb.SKULL && Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Headshots", num + 1);
					}
					PlayerUI.hitmark(raycastInfo.point, false, (raycastInfo.limb == ELimb.SKULL) ? EPlayerHit.CRITICAL : EPlayerHit.ENTITIY);
				}
				else if (raycastInfo.zombie != null || raycastInfo.animal != null)
				{
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num + 1);
					}
					if (raycastInfo.limb == ELimb.SKULL && Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Headshots", num + 1);
					}
					PlayerUI.hitmark(raycastInfo.point, false, (raycastInfo.limb == ELimb.SKULL) ? EPlayerHit.CRITICAL : EPlayerHit.ENTITIY);
				}
				base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Bayonet);
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return;
				}
				InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Bayonet);
				if (input == null)
				{
					return;
				}
				if ((input.point - base.player.look.aim.position).sqrMagnitude > 36f)
				{
					return;
				}
				if (!string.IsNullOrEmpty(input.materialName))
				{
					DamageTool.ServerSpawnLegacyImpact(input.point, input.normal, input.materialName, input.colliderTransform, base.channel.GatherOwnerAndClientConnectionsWithinSphere(input.point, EffectManager.SMALL));
				}
				EPlayerKill eplayerKill = EPlayerKill.NONE;
				uint num2 = 0U;
				float num3 = 1f;
				num3 *= 1f + base.channel.owner.player.skills.mastery(0, 0) * 0.5f;
				ERagdollEffect useableRagdollEffect = base.player.equipment.getUseableRagdollEffect();
				if (input.type == ERaycastInfoType.PLAYER)
				{
					if (input.player != null && (DamageTool.isPlayerAllowedToDamagePlayer(base.player, input.player) || this.equippedGunAsset.bypassAllowedToDamagePlayer))
					{
						IDamageMultiplier damage_PLAYER_MULTIPLIER = UseableGun.DAMAGE_PLAYER_MULTIPLIER;
						DamagePlayerParameters parameters = DamagePlayerParameters.make(input.player, EDeathCause.MELEE, input.direction, damage_PLAYER_MULTIPLIER, input.limb);
						parameters.killer = base.channel.owner.playerID.steamID;
						parameters.times = num3;
						parameters.respectArmor = true;
						parameters.trackKill = true;
						parameters.ragdollEffect = useableRagdollEffect;
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
						IDamageMultiplier damage_ZOMBIE_MULTIPLIER = UseableGun.DAMAGE_ZOMBIE_MULTIPLIER;
						DamageZombieParameters parameters2 = DamageZombieParameters.make(input.zombie, input.direction, damage_ZOMBIE_MULTIPLIER, input.limb);
						parameters2.times = num3;
						parameters2.allowBackstab = true;
						parameters2.respectArmor = true;
						parameters2.instigator = base.player;
						parameters2.zombieStunOverride = this.equippedGunAsset.zombieStunOverride;
						parameters2.ragdollEffect = useableRagdollEffect;
						if (base.player.movement.nav != 255)
						{
							parameters2.AlertPosition = new Vector3?(base.transform.position);
						}
						DamageTool.damageZombie(parameters2, out eplayerKill, out num2);
					}
				}
				else if (input.type == ERaycastInfoType.ANIMAL && input.animal != null)
				{
					IDamageMultiplier damage_ANIMAL_MULTIPLIER = UseableGun.DAMAGE_ANIMAL_MULTIPLIER;
					DamageAnimalParameters parameters3 = DamageAnimalParameters.make(input.animal, input.direction, damage_ANIMAL_MULTIPLIER, input.limb);
					parameters3.times = num3;
					parameters3.instigator = base.player;
					parameters3.ragdollEffect = useableRagdollEffect;
					parameters3.AlertPosition = new Vector3?(base.transform.position);
					DamageTool.damageAnimal(parameters3, out eplayerKill, out num2);
				}
				if (input.type != ERaycastInfoType.PLAYER && input.type != ERaycastInfoType.ZOMBIE && input.type != ERaycastInfoType.ANIMAL && !base.player.life.isAggressor)
				{
					float num4 = 2f + Provider.modeConfigData.Players.Ray_Aggressor_Distance;
					num4 *= num4;
					float num5 = Provider.modeConfigData.Players.Ray_Aggressor_Distance;
					num5 *= num5;
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
								if (a.sqrMagnitude < num4 && (a - vector).sqrMagnitude < num5)
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
					if (num2 > 0U)
					{
						base.player.skills.askPay(num2);
					}
				}
			}
		}

		/// <summary>
		/// Calculate damage multiplier for individual bullet.
		/// </summary>
		// Token: 0x0600450C RID: 17676 RVA: 0x0019296C File Offset: 0x00190B6C
		private float getBulletDamageMultiplier(ref BulletInfo bullet)
		{
			float num = (bullet.quality < 0.5f) ? (0.5f + bullet.quality) : 1f;
			if (bullet.magazineAsset != null)
			{
				num *= bullet.magazineAsset.ballisticDamageMultiplier;
			}
			if (this.thirdAttachments.sightAsset != null)
			{
				num *= this.thirdAttachments.sightAsset.ballisticDamageMultiplier;
			}
			if (this.thirdAttachments.tacticalAsset != null && this.shouldEnableTacticalStats)
			{
				num *= this.thirdAttachments.tacticalAsset.ballisticDamageMultiplier;
			}
			if (this.thirdAttachments.barrelAsset != null)
			{
				num *= this.thirdAttachments.barrelAsset.ballisticDamageMultiplier;
			}
			if (this.thirdAttachments.gripAsset != null)
			{
				num *= this.thirdAttachments.gripAsset.ballisticDamageMultiplier;
			}
			return num;
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x00192A40 File Offset: 0x00190C40
		private void ballistics()
		{
			if (this.equippedGunAsset.projectile != null || this.bullets == null)
			{
				return;
			}
			if (base.channel.IsLocalPlayer)
			{
				for (int i = 0; i < this.bullets.Count; i++)
				{
					BulletInfo bulletInfo = this.bullets[i];
					byte b = (bulletInfo.magazineAsset != null) ? bulletInfo.magazineAsset.pellets : 1;
					if (base.channel.IsLocalPlayer)
					{
						EPlayerHit eplayerHit = EPlayerHit.NONE;
						byte pellet = bulletInfo.pellet;
						Ray ray = new Ray(bulletInfo.position, bulletInfo.velocity);
						float range = Provider.modeConfigData.Gameplay.Ballistics ? (bulletInfo.velocity.magnitude * 0.02f) : this.equippedGunAsset.range;
						RaycastInfo raycastInfo = DamageTool.raycast(ray, range, RayMasks.DAMAGE_CLIENT, base.player);
						if (raycastInfo.player != null && this.equippedGunAsset.playerDamageMultiplier.damage > 1f && (DamageTool.isPlayerAllowedToDamagePlayer(base.player, raycastInfo.player) || this.equippedGunAsset.bypassAllowedToDamagePlayer))
						{
							if (eplayerHit != EPlayerHit.CRITICAL)
							{
								eplayerHit = ((raycastInfo.limb == ELimb.SKULL) ? EPlayerHit.CRITICAL : EPlayerHit.ENTITIY);
							}
							PlayerUI.hitmark(raycastInfo.point, b > 1, (raycastInfo.limb == ELimb.SKULL) ? EPlayerHit.CRITICAL : EPlayerHit.ENTITIY);
						}
						else if (raycastInfo.zombie != null && this.equippedGunAsset.zombieDamageMultiplier.damage > 1f)
						{
							EPlayerHit eplayerHit2 = (raycastInfo.limb == ELimb.SKULL) ? EPlayerHit.CRITICAL : EPlayerHit.ENTITIY;
							if (raycastInfo.zombie.getBulletResistance() < 0.2f)
							{
								eplayerHit2 = EPlayerHit.GHOST;
							}
							if (eplayerHit != EPlayerHit.CRITICAL)
							{
								eplayerHit = eplayerHit2;
							}
							PlayerUI.hitmark(raycastInfo.point, b > 1, eplayerHit2);
						}
						else if (raycastInfo.animal != null && this.equippedGunAsset.animalDamageMultiplier.damage > 1f)
						{
							if (eplayerHit != EPlayerHit.CRITICAL)
							{
								eplayerHit = ((raycastInfo.limb == ELimb.SKULL) ? EPlayerHit.CRITICAL : EPlayerHit.ENTITIY);
							}
							PlayerUI.hitmark(raycastInfo.point, b > 1, (raycastInfo.limb == ELimb.SKULL) ? EPlayerHit.CRITICAL : EPlayerHit.ENTITIY);
						}
						else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Barricade") && this.equippedGunAsset.barricadeDamage > 1f)
						{
							BarricadeDrop barricadeDrop = BarricadeDrop.FindByRootFast(raycastInfo.transform);
							if (barricadeDrop != null)
							{
								ItemBarricadeAsset asset = barricadeDrop.asset;
								if (asset != null && asset.canBeDamaged && (asset.isVulnerable || this.CanDamageInvulnerableEntities))
								{
									if (eplayerHit == EPlayerHit.NONE)
									{
										eplayerHit = EPlayerHit.BUILD;
									}
									PlayerUI.hitmark(raycastInfo.point, b > 1, EPlayerHit.BUILD);
								}
							}
						}
						else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Structure") && this.equippedGunAsset.structureDamage > 1f)
						{
							StructureDrop structureDrop = StructureDrop.FindByRootFast(raycastInfo.transform);
							if (structureDrop != null)
							{
								ItemStructureAsset asset2 = structureDrop.asset;
								if (asset2 != null && asset2.canBeDamaged && (asset2.isVulnerable || this.CanDamageInvulnerableEntities))
								{
									if (eplayerHit == EPlayerHit.NONE)
									{
										eplayerHit = EPlayerHit.BUILD;
									}
									PlayerUI.hitmark(raycastInfo.point, b > 1, EPlayerHit.BUILD);
								}
							}
						}
						else if (raycastInfo.vehicle != null && !raycastInfo.vehicle.isDead && this.equippedGunAsset.vehicleDamage > 1f)
						{
							if (raycastInfo.vehicle.asset != null && raycastInfo.vehicle.canBeDamaged && (raycastInfo.vehicle.asset.isVulnerable || this.CanDamageInvulnerableEntities))
							{
								if (eplayerHit == EPlayerHit.NONE)
								{
									eplayerHit = EPlayerHit.BUILD;
								}
								PlayerUI.hitmark(raycastInfo.point, b > 1, EPlayerHit.BUILD);
							}
						}
						else if (raycastInfo.transform != null && raycastInfo.transform.CompareTag("Resource") && this.equippedGunAsset.resourceDamage > 1f)
						{
							byte x;
							byte y;
							ushort index;
							if (ResourceManager.tryGetRegion(raycastInfo.transform, out x, out y, out index))
							{
								ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);
								if (resourceSpawnpoint != null && !resourceSpawnpoint.isDead && this.equippedGunAsset.hasBladeID(resourceSpawnpoint.asset.bladeID))
								{
									if (eplayerHit == EPlayerHit.NONE)
									{
										eplayerHit = EPlayerHit.BUILD;
									}
									PlayerUI.hitmark(raycastInfo.point, b > 1, EPlayerHit.BUILD);
								}
							}
						}
						else if (raycastInfo.transform != null && this.equippedGunAsset.objectDamage > 1f)
						{
							InteractableObjectRubble componentInParent = raycastInfo.transform.GetComponentInParent<InteractableObjectRubble>();
							if (componentInParent != null)
							{
								raycastInfo.transform = componentInParent.transform;
								raycastInfo.section = componentInParent.getSection(raycastInfo.collider.transform);
								if (componentInParent.IsSectionIndexValid(raycastInfo.section) && !componentInParent.isSectionDead(raycastInfo.section) && this.equippedGunAsset.hasBladeID(componentInParent.asset.rubbleBladeID) && (componentInParent.asset.rubbleIsVulnerable || this.CanDamageInvulnerableEntities))
								{
									if (eplayerHit == EPlayerHit.NONE)
									{
										eplayerHit = EPlayerHit.BUILD;
									}
									PlayerUI.hitmark(raycastInfo.point, b > 1, EPlayerHit.BUILD);
								}
							}
						}
						if (Provider.modeConfigData.Gameplay.Ballistics)
						{
							if (bulletInfo.steps > 0 || this.equippedGunAsset.ballisticSteps <= 1)
							{
								Vector3 direction = bulletInfo.GetDirection();
								if (this.equippedGunAsset.ballisticTravel < 32f)
								{
									this.trace(bulletInfo.position + direction * 32f, direction);
								}
								else
								{
									this.trace(bulletInfo.position + direction * Random.Range(32f, this.equippedGunAsset.ballisticTravel), direction);
								}
							}
						}
						else if (this.equippedGunAsset.range < 32f)
						{
							this.trace(ray.origin + ray.direction * 32f, ray.direction);
						}
						else
						{
							this.trace(ray.origin + ray.direction * Random.Range(32f, Mathf.Min(64f, this.equippedGunAsset.range)), ray.direction);
						}
						if (base.player.input.isRaycastInvalid(raycastInfo))
						{
							float num = Physics.gravity.y;
							if (bulletInfo.barrelAsset != null)
							{
								num *= bulletInfo.barrelAsset.ballisticDrop;
							}
							num *= this.equippedGunAsset.bulletGravityMultiplier;
							bulletInfo.position += bulletInfo.velocity * 0.02f;
							bulletInfo.velocity = new Vector3(bulletInfo.velocity.x, bulletInfo.velocity.y + num * 0.02f, bulletInfo.velocity.z);
						}
						else
						{
							if (eplayerHit != EPlayerHit.NONE)
							{
								int num2;
								if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num2))
								{
									Provider.provider.statisticsService.userStatisticsService.setStatistic("Accuracy_Hit", num2 + 1);
								}
								if (eplayerHit == EPlayerHit.CRITICAL && Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num2))
								{
									Provider.provider.statisticsService.userStatisticsService.setStatistic("Headshots", num2 + 1);
								}
							}
							base.player.input.sendRaycast(raycastInfo, ERaycastInfoUsage.Gun);
							bulletInfo.steps = 254;
						}
					}
				}
			}
			if (Provider.isServer)
			{
				while (this.bullets.Count > 0)
				{
					BulletInfo bulletInfo2 = this.bullets[0];
					byte b2 = (bulletInfo2.magazineAsset != null) ? bulletInfo2.magazineAsset.pellets : 1;
					if (!base.player.input.hasInputs())
					{
						break;
					}
					InputInfo input = base.player.input.getInput(true, ERaycastInfoUsage.Gun);
					if (input == null || this.equippedGunAsset == null)
					{
						break;
					}
					if (!base.channel.IsLocalPlayer)
					{
						if (Provider.modeConfigData.Gameplay.Ballistics)
						{
							if ((input.point - bulletInfo2.position).magnitude > this.equippedGunAsset.ballisticTravel * (float)((long)(bulletInfo2.steps + 1) + (long)((ulong)PlayerInput.SAMPLES)) + 4f)
							{
								this.bullets.RemoveAt(0);
								continue;
							}
						}
						else if ((input.point - base.player.look.aim.position).sqrMagnitude > MathfEx.Square(this.equippedGunAsset.range + 4f))
						{
							break;
						}
					}
					if (UseableGun.onBulletHit != null)
					{
						bool flag = true;
						UseableGun.onBulletHit(this, bulletInfo2, input, ref flag);
						if (!flag)
						{
							this.bullets.RemoveAt(0);
							continue;
						}
					}
					if (!string.IsNullOrEmpty(input.materialName))
					{
						if (bulletInfo2.magazineAsset != null && !bulletInfo2.magazineAsset.IsImpactEffectRefNull())
						{
							DamageTool.ServerTriggerImpactEffectForMagazinesV2(bulletInfo2.magazineAsset.FindImpactEffectAsset(), input.point, input.normal, base.channel.owner);
						}
						else
						{
							DamageTool.ServerSpawnBulletImpact(input.point, input.normal, input.materialName, input.colliderTransform, base.channel.owner, base.channel.GatherOwnerAndClientConnectionsWithinSphere(input.point, EffectManager.SMALL));
						}
					}
					EPlayerKill eplayerKill = EPlayerKill.NONE;
					uint num3 = 0U;
					float num4 = this.getBulletDamageMultiplier(ref bulletInfo2);
					float value = Vector3.Distance(bulletInfo2.origin, input.point);
					float a = this.equippedGunAsset.range * this.equippedGunAsset.damageFalloffRange;
					float b3 = this.equippedGunAsset.range * this.equippedGunAsset.damageFalloffMaxRange;
					float t = Mathf.InverseLerp(a, b3, value);
					num4 *= Mathf.Lerp(1f, this.equippedGunAsset.damageFalloffMultiplier, t);
					ERagdollEffect useableRagdollEffect = base.player.equipment.getUseableRagdollEffect();
					if (input.type == ERaycastInfoType.PLAYER)
					{
						if (input.player != null && (DamageTool.isPlayerAllowedToDamagePlayer(base.player, input.player) || this.equippedGunAsset.bypassAllowedToDamagePlayer))
						{
							bool flag2 = input.limb == ELimb.SKULL && this.equippedGunAsset.instakillHeadshots && Provider.modeConfigData.Players.Allow_Instakill_Headshots;
							IDamageMultiplier playerDamageMultiplier = this.equippedGunAsset.playerDamageMultiplier;
							DamagePlayerParameters parameters = DamagePlayerParameters.make(input.player, EDeathCause.GUN, input.direction * Mathf.Ceil((float)b2 / 2f), playerDamageMultiplier, input.limb);
							parameters.killer = base.channel.owner.playerID.steamID;
							parameters.times = num4;
							parameters.respectArmor = !flag2;
							parameters.trackKill = true;
							parameters.ragdollEffect = useableRagdollEffect;
							this.equippedGunAsset.initPlayerDamageParameters(ref parameters);
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
							bool flag3 = input.limb == ELimb.SKULL && this.equippedGunAsset.instakillHeadshots && Provider.modeConfigData.Zombies.Weapons_Use_Player_Damage && Provider.modeConfigData.Players.Allow_Instakill_Headshots;
							Vector3 direction2 = input.direction * Mathf.Ceil((float)b2 / 2f);
							IDamageMultiplier zombieOrPlayerDamageMultiplier = this.equippedGunAsset.zombieOrPlayerDamageMultiplier;
							DamageZombieParameters parameters2 = DamageZombieParameters.make(input.zombie, direction2, zombieOrPlayerDamageMultiplier, input.limb);
							parameters2.times = num4 * input.zombie.getBulletResistance();
							parameters2.allowBackstab = false;
							parameters2.respectArmor = !flag3;
							parameters2.instigator = base.player;
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
							Vector3 direction3 = input.direction * Mathf.Ceil((float)b2 / 2f);
							IDamageMultiplier animalOrPlayerDamageMultiplier = this.equippedGunAsset.animalOrPlayerDamageMultiplier;
							DamageAnimalParameters parameters3 = DamageAnimalParameters.make(input.animal, direction3, animalOrPlayerDamageMultiplier, input.limb);
							parameters3.times = num4;
							parameters3.instigator = base.player;
							parameters3.ragdollEffect = useableRagdollEffect;
							parameters3.AlertPosition = new Vector3?(base.transform.position);
							DamageTool.damageAnimal(parameters3, out eplayerKill, out num3);
						}
					}
					else if (input.type == ERaycastInfoType.VEHICLE)
					{
						if (input.vehicle != null && input.vehicle.asset != null && input.vehicle.canBeDamaged && (input.vehicle.asset.isVulnerable || this.CanDamageInvulnerableEntities))
						{
							float num5 = this.CanDamageInvulnerableEntities ? Provider.modeConfigData.Vehicles.Gun_Highcal_Damage_Multiplier : Provider.modeConfigData.Vehicles.Gun_Lowcal_Damage_Multiplier;
							DamageTool.damage(input.vehicle, true, input.point, false, this.equippedGunAsset.vehicleDamage, num4 * num5, true, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Gun);
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
								if (asset3 != null && asset3.canBeDamaged && (asset3.isVulnerable || this.CanDamageInvulnerableEntities))
								{
									float num6 = this.CanDamageInvulnerableEntities ? Provider.modeConfigData.Barricades.Gun_Highcal_Damage_Multiplier : Provider.modeConfigData.Barricades.Gun_Lowcal_Damage_Multiplier;
									DamageTool.damage(input.transform, false, this.equippedGunAsset.barricadeDamage, num4 * num6, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Gun);
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
								if (asset4 != null && asset4.canBeDamaged && (asset4.isVulnerable || this.CanDamageInvulnerableEntities))
								{
									float num7 = this.CanDamageInvulnerableEntities ? Provider.modeConfigData.Structures.Gun_Highcal_Damage_Multiplier : Provider.modeConfigData.Structures.Gun_Lowcal_Damage_Multiplier;
									DamageTool.damage(input.transform, false, input.direction * Mathf.Ceil((float)b2 / 2f), this.equippedGunAsset.structureDamage, num4 * num7, out eplayerKill, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Gun);
								}
							}
						}
					}
					else if (input.type == ERaycastInfoType.RESOURCE)
					{
						byte x2;
						byte y2;
						ushort index2;
						if (input.transform != null && input.transform.CompareTag("Resource") && ResourceManager.tryGetRegion(input.transform, out x2, out y2, out index2))
						{
							ResourceSpawnpoint resourceSpawnpoint2 = ResourceManager.getResourceSpawnpoint(x2, y2, index2);
							if (resourceSpawnpoint2 != null && !resourceSpawnpoint2.isDead && this.equippedGunAsset.hasBladeID(resourceSpawnpoint2.asset.bladeID))
							{
								DamageTool.damage(input.transform, input.direction * Mathf.Ceil((float)b2 / 2f), this.equippedGunAsset.resourceDamage, num4, 1f, out eplayerKill, out num3, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Gun);
							}
						}
					}
					else if (input.type == ERaycastInfoType.OBJECT && input.transform != null && input.section < 255)
					{
						InteractableObjectRubble componentInParent2 = input.transform.GetComponentInParent<InteractableObjectRubble>();
						if (componentInParent2 != null && componentInParent2.IsSectionIndexValid(input.section) && !componentInParent2.isSectionDead(input.section) && this.equippedGunAsset.hasBladeID(componentInParent2.asset.rubbleBladeID) && (componentInParent2.asset.rubbleIsVulnerable || this.CanDamageInvulnerableEntities))
						{
							DamageTool.damage(componentInParent2.transform, input.direction, input.section, this.equippedGunAsset.objectDamage, num4, out eplayerKill, out num3, base.channel.owner.playerID.steamID, EDamageOrigin.Useable_Gun);
						}
					}
					if (input.type != ERaycastInfoType.PLAYER && input.type != ERaycastInfoType.ZOMBIE && input.type != ERaycastInfoType.ANIMAL && !base.player.life.isAggressor)
					{
						float num8 = this.equippedGunAsset.range + Provider.modeConfigData.Players.Ray_Aggressor_Distance;
						num8 *= num8;
						float num9 = Provider.modeConfigData.Players.Ray_Aggressor_Distance;
						num9 *= num9;
						Vector3 normalized = (bulletInfo2.position - base.player.look.aim.position).normalized;
						for (int j = 0; j < Provider.clients.Count; j++)
						{
							if (Provider.clients[j] != base.channel.owner)
							{
								Player player = Provider.clients[j].player;
								if (!(player == null))
								{
									Vector3 vector = player.look.aim.position - base.player.look.aim.position;
									Vector3 a2 = Vector3.Project(vector, normalized);
									if (a2.sqrMagnitude < num8 && (a2 - vector).sqrMagnitude < num9)
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
							}
							else
							{
								base.player.skills.askPay(25U);
							}
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
					Vector3 vector2 = input.point + input.normal * 0.25f;
					if (bulletInfo2.magazineAsset != null && bulletInfo2.magazineAsset.isExplosive)
					{
						UseableGun.DetonateExplosiveMagazine(bulletInfo2.magazineAsset, vector2, base.player, useableRagdollEffect);
					}
					if (bulletInfo2.dropID != 0)
					{
						ItemManager.dropItem(new Item(bulletInfo2.dropID, bulletInfo2.dropAmount, bulletInfo2.dropQuality), vector2, false, true, false);
					}
					this.bullets.RemoveAt(0);
				}
			}
			if (base.player.equipment.asset != null)
			{
				if (Provider.modeConfigData.Gameplay.Ballistics)
				{
					for (int k = this.bullets.Count - 1; k >= 0; k--)
					{
						BulletInfo bulletInfo3 = this.bullets[k];
						bulletInfo3.steps += 1;
						if (bulletInfo3.steps >= this.equippedGunAsset.ballisticSteps)
						{
							this.bullets.RemoveAt(k);
						}
					}
					return;
				}
				this.bullets.Clear();
			}
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x00193F38 File Offset: 0x00192138
		[Obsolete]
		public void askAttachSight(CSteamID steamID, byte page, byte x, byte y, byte[] hash)
		{
			this.ReceiveAttachSight(page, x, y, hash);
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x00193F48 File Offset: 0x00192148
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askAttachSight")]
		public void ReceiveAttachSight(byte page, byte x, byte y, byte[] hash)
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isFired)
			{
				return;
			}
			if (this.isReloading || this.isHammering || this.isUnjamming || this.needsRechamber)
			{
				return;
			}
			if (base.player.equipment.asset == null)
			{
				return;
			}
			if (!this.equippedGunAsset.hasSight)
			{
				return;
			}
			Item item = null;
			if (this.thirdAttachments.sightAsset != null)
			{
				item = new Item(this.thirdAttachments.sightID, false, base.player.equipment.state[13]);
			}
			if (page != 255)
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index != 255)
				{
					ItemJar item2 = base.player.inventory.getItem(page, index);
					ItemCaliberAsset asset = item2.GetAsset<ItemCaliberAsset>();
					if (asset == null)
					{
						return;
					}
					if (asset.shouldVerifyHash && !Hash.verifyHash(hash, asset.hash))
					{
						return;
					}
					if (asset.calibers.Length != 0)
					{
						bool flag = false;
						byte b = 0;
						while ((int)b < asset.calibers.Length)
						{
							byte b2 = 0;
							while ((int)b2 < this.equippedGunAsset.attachmentCalibers.Length)
							{
								if (asset.calibers[(int)b] == this.equippedGunAsset.attachmentCalibers[(int)b2])
								{
									flag = true;
									break;
								}
								b2 += 1;
							}
							b += 1;
						}
						if (!flag)
						{
							return;
						}
					}
					else if (this.equippedGunAsset.requiresNonZeroAttachmentCaliber)
					{
						return;
					}
					if (!this.changeSightRequested(item, item2))
					{
						return;
					}
					Buffer.BlockCopy(BitConverter.GetBytes(item2.item.id), 0, base.player.equipment.state, 0, 2);
					base.player.equipment.state[13] = item2.item.quality;
					base.player.inventory.removeItem(page, index);
					if (item != null)
					{
						base.player.inventory.forceAddItem(item, true);
					}
					base.player.equipment.sendUpdateState();
					EffectManager.TriggerFiremodeEffect(base.transform.position);
					return;
				}
			}
			if (!this.changeSightRequested(item, null))
			{
				return;
			}
			if (item != null)
			{
				base.player.inventory.forceAddItem(item, true);
			}
			base.player.equipment.state[0] = 0;
			base.player.equipment.state[1] = 0;
			base.player.equipment.sendUpdateState();
			EffectManager.TriggerFiremodeEffect(base.transform.position);
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x001941B8 File Offset: 0x001923B8
		[Obsolete]
		public void askAttachTactical(CSteamID steamID, byte page, byte x, byte y, byte[] hash)
		{
			this.ReceiveAttachTactical(page, x, y, hash);
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x001941C8 File Offset: 0x001923C8
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askAttachTactical")]
		public void ReceiveAttachTactical(byte page, byte x, byte y, byte[] hash)
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isFired)
			{
				return;
			}
			if (this.isReloading || this.isHammering || this.isUnjamming || this.needsRechamber)
			{
				return;
			}
			if (base.player.equipment.asset == null)
			{
				return;
			}
			if (!this.equippedGunAsset.hasTactical)
			{
				return;
			}
			Item item = null;
			if (this.thirdAttachments.tacticalAsset != null)
			{
				item = new Item(this.thirdAttachments.tacticalID, false, base.player.equipment.state[14]);
			}
			if (page != 255)
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index != 255)
				{
					ItemJar item2 = base.player.inventory.getItem(page, index);
					ItemCaliberAsset asset = item2.GetAsset<ItemCaliberAsset>();
					if (asset == null)
					{
						return;
					}
					if (asset.shouldVerifyHash && !Hash.verifyHash(hash, asset.hash))
					{
						return;
					}
					if (asset.calibers.Length != 0)
					{
						bool flag = false;
						byte b = 0;
						while ((int)b < asset.calibers.Length)
						{
							byte b2 = 0;
							while ((int)b2 < this.equippedGunAsset.attachmentCalibers.Length)
							{
								if (asset.calibers[(int)b] == this.equippedGunAsset.attachmentCalibers[(int)b2])
								{
									flag = true;
									break;
								}
								b2 += 1;
							}
							b += 1;
						}
						if (!flag)
						{
							return;
						}
					}
					else if (this.equippedGunAsset.requiresNonZeroAttachmentCaliber)
					{
						return;
					}
					if (!this.changeTacticalRequested(item, item2))
					{
						return;
					}
					Buffer.BlockCopy(BitConverter.GetBytes(item2.item.id), 0, base.player.equipment.state, 2, 2);
					base.player.equipment.state[14] = item2.item.quality;
					base.player.inventory.removeItem(page, index);
					if (item != null)
					{
						base.player.inventory.forceAddItem(item, true);
					}
					base.player.equipment.sendUpdateState();
					EffectManager.TriggerFiremodeEffect(base.transform.position);
					return;
				}
			}
			if (!this.changeTacticalRequested(item, null))
			{
				return;
			}
			if (item != null)
			{
				base.player.inventory.forceAddItem(item, true);
			}
			base.player.equipment.state[2] = 0;
			base.player.equipment.state[3] = 0;
			base.player.equipment.sendUpdateState();
			EffectManager.TriggerFiremodeEffect(base.transform.position);
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x00194438 File Offset: 0x00192638
		[Obsolete]
		public void askAttachGrip(CSteamID steamID, byte page, byte x, byte y, byte[] hash)
		{
			this.ReceiveAttachGrip(page, x, y, hash);
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x00194448 File Offset: 0x00192648
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askAttachGrip")]
		public void ReceiveAttachGrip(byte page, byte x, byte y, byte[] hash)
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isFired)
			{
				return;
			}
			if (this.isReloading || this.isHammering || this.isUnjamming || this.needsRechamber)
			{
				return;
			}
			if (base.player.equipment.asset == null)
			{
				return;
			}
			if (!this.equippedGunAsset.hasGrip)
			{
				return;
			}
			Item item = null;
			if (this.thirdAttachments.gripAsset != null)
			{
				item = new Item(this.thirdAttachments.gripID, false, base.player.equipment.state[15]);
			}
			if (page != 255)
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index != 255)
				{
					ItemJar item2 = base.player.inventory.getItem(page, index);
					ItemCaliberAsset asset = item2.GetAsset<ItemCaliberAsset>();
					if (asset == null)
					{
						return;
					}
					if (asset.shouldVerifyHash && !Hash.verifyHash(hash, asset.hash))
					{
						return;
					}
					if (asset.calibers.Length != 0)
					{
						bool flag = false;
						byte b = 0;
						while ((int)b < asset.calibers.Length)
						{
							byte b2 = 0;
							while ((int)b2 < this.equippedGunAsset.attachmentCalibers.Length)
							{
								if (asset.calibers[(int)b] == this.equippedGunAsset.attachmentCalibers[(int)b2])
								{
									flag = true;
									break;
								}
								b2 += 1;
							}
							b += 1;
						}
						if (!flag)
						{
							return;
						}
					}
					else if (this.equippedGunAsset.requiresNonZeroAttachmentCaliber)
					{
						return;
					}
					if (!this.changeGripRequested(item, item2))
					{
						return;
					}
					Buffer.BlockCopy(BitConverter.GetBytes(item2.item.id), 0, base.player.equipment.state, 4, 2);
					base.player.equipment.state[15] = item2.item.quality;
					base.player.inventory.removeItem(page, index);
					if (item != null)
					{
						base.player.inventory.forceAddItem(item, true);
					}
					base.player.equipment.sendUpdateState();
					EffectManager.TriggerFiremodeEffect(base.transform.position);
					return;
				}
			}
			if (!this.changeGripRequested(item, null))
			{
				return;
			}
			if (item != null)
			{
				base.player.inventory.forceAddItem(item, true);
			}
			base.player.equipment.state[4] = 0;
			base.player.equipment.state[5] = 0;
			base.player.equipment.sendUpdateState();
			EffectManager.TriggerFiremodeEffect(base.transform.position);
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x001946B8 File Offset: 0x001928B8
		[Obsolete]
		public void askAttachBarrel(CSteamID steamID, byte page, byte x, byte y, byte[] hash)
		{
			this.ReceiveAttachBarrel(page, x, y, hash);
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x001946C8 File Offset: 0x001928C8
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askAttachBarrel")]
		public void ReceiveAttachBarrel(byte page, byte x, byte y, byte[] hash)
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isFired)
			{
				return;
			}
			if (this.isReloading || this.isHammering || this.isUnjamming || this.needsRechamber)
			{
				return;
			}
			if (base.player.equipment.asset == null)
			{
				return;
			}
			if (!this.equippedGunAsset.hasBarrel)
			{
				return;
			}
			Item item = null;
			if (this.thirdAttachments.barrelAsset != null)
			{
				item = new Item(this.thirdAttachments.barrelID, false, base.player.equipment.state[16]);
			}
			if (page != 255)
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index != 255)
				{
					ItemJar item2 = base.player.inventory.getItem(page, index);
					ItemCaliberAsset asset = item2.GetAsset<ItemCaliberAsset>();
					if (asset == null)
					{
						return;
					}
					if (asset.shouldVerifyHash && !Hash.verifyHash(hash, asset.hash))
					{
						return;
					}
					if (asset.calibers.Length != 0)
					{
						bool flag = false;
						byte b = 0;
						while ((int)b < asset.calibers.Length)
						{
							byte b2 = 0;
							while ((int)b2 < this.equippedGunAsset.attachmentCalibers.Length)
							{
								if (asset.calibers[(int)b] == this.equippedGunAsset.attachmentCalibers[(int)b2])
								{
									flag = true;
									break;
								}
								b2 += 1;
							}
							b += 1;
						}
						if (!flag)
						{
							return;
						}
					}
					else if (this.equippedGunAsset.requiresNonZeroAttachmentCaliber)
					{
						return;
					}
					if (!this.changeBarrelRequested(item, item2))
					{
						return;
					}
					Buffer.BlockCopy(BitConverter.GetBytes(item2.item.id), 0, base.player.equipment.state, 6, 2);
					base.player.equipment.state[16] = item2.item.quality;
					base.player.inventory.removeItem(page, index);
					if (item != null)
					{
						base.player.inventory.forceAddItem(item, true);
					}
					base.player.equipment.sendUpdateState();
					EffectManager.TriggerFiremodeEffect(base.transform.position);
					return;
				}
			}
			if (!this.changeBarrelRequested(item, null))
			{
				return;
			}
			if (item != null)
			{
				base.player.inventory.forceAddItem(item, true);
			}
			base.player.equipment.state[6] = 0;
			base.player.equipment.state[7] = 0;
			base.player.equipment.sendUpdateState();
			EffectManager.TriggerFiremodeEffect(base.transform.position);
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x00194938 File Offset: 0x00192B38
		[Obsolete]
		public void askAttachMagazine(CSteamID steamID, byte page, byte x, byte y, byte[] hash)
		{
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x0019493C File Offset: 0x00192B3C
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askAttachMagazine")]
		public void ReceiveAttachMagazine(in ServerInvocationContext context, byte page, byte x, byte y, byte[] hash)
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (this.isFired)
			{
				return;
			}
			if (this.isReloading || this.isHammering || this.isUnjamming || this.needsRechamber)
			{
				return;
			}
			if (base.player.equipment.asset == null)
			{
				return;
			}
			if (!this.equippedGunAsset.allowMagazineChange)
			{
				return;
			}
			Item item = null;
			if (this.thirdAttachments.magazineAsset != null && (this.ammo > 0 || (!this.equippedGunAsset.shouldDeleteEmptyMagazines && !this.thirdAttachments.magazineAsset.deleteEmpty)))
			{
				byte newAmount = base.player.equipment.state[10];
				if (this.thirdAttachments.magazineAsset.shouldFillAfterDetach)
				{
					newAmount = this.thirdAttachments.magazineAsset.amount;
				}
				item = new Item(this.thirdAttachments.magazineID, newAmount, base.player.equipment.state[17]);
			}
			if (page != 255)
			{
				byte index = base.player.inventory.getIndex(page, x, y);
				if (index != 255)
				{
					ItemJar item2 = base.player.inventory.getItem(page, index);
					ItemCaliberAsset asset = item2.GetAsset<ItemCaliberAsset>();
					if (asset == null)
					{
						return;
					}
					if (asset.shouldVerifyHash && !Hash.verifyHash(hash, asset.hash))
					{
						return;
					}
					if (asset.calibers.Length != 0)
					{
						bool flag = false;
						byte b = 0;
						while ((int)b < asset.calibers.Length)
						{
							byte b2 = 0;
							while ((int)b2 < this.equippedGunAsset.magazineCalibers.Length)
							{
								if (asset.calibers[(int)b] == this.equippedGunAsset.magazineCalibers[(int)b2])
								{
									flag = true;
									break;
								}
								b2 += 1;
							}
							b += 1;
						}
						if (!flag)
						{
							return;
						}
					}
					else if (this.equippedGunAsset.requiresNonZeroAttachmentCaliber)
					{
						return;
					}
					if (!this.changeMagazineRequested(item, item2))
					{
						return;
					}
					bool flag2 = this.ammo == 0;
					this.ammo = item2.item.amount;
					Buffer.BlockCopy(BitConverter.GetBytes(item2.item.id), 0, base.player.equipment.state, 8, 2);
					base.player.equipment.state[10] = item2.item.amount;
					base.player.equipment.state[17] = item2.item.quality;
					base.player.inventory.removeItem(page, index);
					if (item != null)
					{
						base.player.inventory.forceAddItem(item, true);
					}
					base.player.equipment.sendUpdateState();
					UseableGun.SendPlayReload.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), flag2 && this.equippedGunAsset.hammer != null);
					EffectManager.TriggerFiremodeEffect(base.transform.position);
					return;
				}
			}
			if (!this.changeMagazineRequested(item, null))
			{
				return;
			}
			if (item != null)
			{
				base.player.inventory.forceAddItem(item, true);
			}
			base.player.equipment.state[8] = 0;
			base.player.equipment.state[9] = 0;
			base.player.equipment.state[10] = 0;
			base.player.equipment.sendUpdateState();
			UseableGun.SendPlayReload.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), this.equippedGunAsset.hammer != null);
			EffectManager.TriggerFiremodeEffect(base.transform.position);
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x00194CBC File Offset: 0x00192EBC
		private void hammer()
		{
			base.player.equipment.isBusy = true;
			this.isHammering = true;
			this.startedHammer = Time.realtimeSinceStartup;
			float num = 1f;
			num += base.player.skills.mastery(0, 2) * 0.5f;
			if (this.thirdAttachments.magazineAsset != null)
			{
				num *= this.thirdAttachments.magazineAsset.speed;
			}
			base.player.playSound(this.equippedGunAsset.hammer, num, 0f);
			this.updateAnimationSpeeds(num);
			base.player.animator.play("Hammer", false);
			VehicleTurretEventHook vehicleTurretEventHook = this.GetVehicleTurretEventHook();
			if (vehicleTurretEventHook != null)
			{
				UnityEvent onChamberingStarted = vehicleTurretEventHook.OnChamberingStarted;
				if (onChamberingStarted != null)
				{
					onChamberingStarted.TryInvoke(this);
				}
			}
			foreach (UseableGunEventHook useableGunEventHook in this.EnumerateEventComponents())
			{
				UnityEvent onChamberingStarted2 = useableGunEventHook.OnChamberingStarted;
				if (onChamberingStarted2 != null)
				{
					onChamberingStarted2.TryInvoke(this);
				}
			}
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x00194DD0 File Offset: 0x00192FD0
		[Obsolete]
		public void askReload(CSteamID steamID, bool newHammer)
		{
			this.ReceivePlayReload(newHammer);
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x00194DDC File Offset: 0x00192FDC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askReload")]
		public void ReceivePlayReload(bool newHammer)
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				if (this.isAiming)
				{
					this.isAiming = false;
					this.stopAim();
				}
				if (this.isAttaching)
				{
					this.isAttaching = false;
					this.stopAttach();
				}
				this.isShooting = false;
				this.isSprinting = false;
				base.player.equipment.isBusy = true;
				this.needsHammer = newHammer;
				this.isReloading = true;
				this.startedReload = Time.realtimeSinceStartup;
				float num = 1f;
				num += base.player.skills.mastery(0, 2) * 0.5f;
				if (this.thirdAttachments.magazineAsset != null)
				{
					num *= this.thirdAttachments.magazineAsset.speed;
				}
				base.player.playSound(this.equippedGunAsset.reload, num, 0f);
				this.updateAnimationSpeeds(num);
				base.player.animator.play("Reload", false);
				this.needsUnplace = true;
				this.needsReplace = true;
				if (this.equippedGunAsset.action == EAction.Break)
				{
					this.needsUnload = true;
				}
				UseableGun.OnReloading_Global.TryInvoke("OnReloading_Global", this);
				VehicleTurretEventHook vehicleTurretEventHook = this.GetVehicleTurretEventHook();
				if (vehicleTurretEventHook != null)
				{
					UnityEvent onReloadingStarted = vehicleTurretEventHook.OnReloadingStarted;
					if (onReloadingStarted != null)
					{
						onReloadingStarted.TryInvoke(this);
					}
				}
				foreach (UseableGunEventHook useableGunEventHook in this.EnumerateEventComponents())
				{
					UnityEvent onReloadingStarted2 = useableGunEventHook.OnReloadingStarted;
					if (onReloadingStarted2 != null)
					{
						onReloadingStarted2.TryInvoke(this);
					}
				}
			}
		}

		/// <summary>
		/// Requested for plugin use.
		/// </summary>
		// Token: 0x0600451B RID: 17691 RVA: 0x00194F74 File Offset: 0x00193174
		public void ServerPlayReload(bool shouldHammer)
		{
			shouldHammer &= (this.equippedGunAsset.hammer != null);
			UseableGun.SendPlayReload.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), shouldHammer);
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x00194FA2 File Offset: 0x001931A2
		[Obsolete]
		public void askPlayChamberJammed(CSteamID steamID, byte correctedAmmo)
		{
			this.ReceivePlayChamberJammed(correctedAmmo);
		}

		/// <summary>
		/// Request from the server to play a gun jammed animation.
		/// Since client can't predict chamber jams we fixup the predicted ammo count.
		/// </summary>
		// Token: 0x0600451D RID: 17693 RVA: 0x00194FAC File Offset: 0x001931AC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askPlayChamberJammed")]
		public void ReceivePlayChamberJammed(byte correctedAmmo)
		{
			if (!base.player.equipment.IsEquipAnimationFinished)
			{
				return;
			}
			if (this.isAiming)
			{
				this.isAiming = false;
				this.stopAim();
			}
			if (this.isAttaching)
			{
				this.isAttaching = false;
				this.stopAttach();
			}
			this.isShooting = false;
			this.isSprinting = false;
			base.player.equipment.isBusy = true;
			this.isUnjamming = true;
			this.startedUnjammingChamber = Time.realtimeSinceStartup;
			float num = 1f;
			base.player.playSound(this.equippedGunAsset.chamberJammedSound, num, 0f);
			this.updateAnimationSpeeds(num);
			base.player.animator.play(this.equippedGunAsset.unjamChamberAnimName, false);
			this.ammo = correctedAmmo;
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x00195074 File Offset: 0x00193274
		[Obsolete]
		public void askAimStart(CSteamID steamID)
		{
			this.ReceivePlayAimStart();
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x0019507C File Offset: 0x0019327C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askAimStart")]
		public void ReceivePlayAimStart()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.startAim();
			}
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x00195096 File Offset: 0x00193296
		[Obsolete]
		public void askAimStop(CSteamID steamID)
		{
			this.ReceivePlayAimStop();
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x0019509E File Offset: 0x0019329E
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askAimStop")]
		public void ReceivePlayAimStop()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.stopAim();
			}
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x001950B8 File Offset: 0x001932B8
		public override bool startPrimary()
		{
			if ((!this.isShooting && !this.isReloading && !this.isHammering && !this.isUnjamming && !this.isAttaching && !this.needsRechamber && this.firemode != EFiremode.SAFETY && !base.player.equipment.isBusy && !base.player.quests.IsCutsceneModeActive()) & (!this.isSprinting || this.equippedGunAsset.canAimDuringSprint))
			{
				if (this.equippedGunAsset.action == EAction.String)
				{
					if (this.thirdAttachments.nockHook != null || this.isAiming)
					{
						this.isShooting = true;
					}
				}
				else if (this.equippedGunAsset.action == EAction.Minigun)
				{
					if (this.isAiming)
					{
						this.isShooting = true;
					}
				}
				else
				{
					this.isShooting = true;
				}
			}
			if (this.isShooting)
			{
				this.wasTriggerJustPulled = true;
				if (this.fireDelayCounter < 1)
				{
					this.fireDelayCounter = this.equippedGunAsset.fireDelay;
					if (this.fireDelayCounter > 0 && base.channel.IsLocalPlayer && this.equippedGunAsset.fireDelaySound != null)
					{
						base.player.playSound(this.equippedGunAsset.fireDelaySound, 1f, 0f);
					}
				}
			}
			return this.isShooting;
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x0019520F File Offset: 0x0019340F
		public override void stopPrimary()
		{
			this.isShooting = false;
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x00195218 File Offset: 0x00193418
		public override bool startSecondary()
		{
			if ((!this.isAiming && !this.isReloading && !this.isHammering && !this.isUnjamming && !this.isAttaching && !this.needsRechamber && this.firemode > EFiremode.SAFETY) & (!this.isSprinting || this.equippedGunAsset.canAimDuringSprint))
			{
				this.isAiming = true;
				this.startAim();
				if (Provider.isServer)
				{
					UseableGun.SendPlayAimStart.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
			}
			return this.isAiming;
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x001952B0 File Offset: 0x001934B0
		public override void stopSecondary()
		{
			if (this.isAiming)
			{
				if (this.equippedGunAsset.action == EAction.Minigun && this.isShooting)
				{
					this.isShooting = false;
				}
				this.isAiming = false;
				this.stopAim();
				if (Provider.isServer)
				{
					UseableGun.SendPlayAimStop.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06004526 RID: 17702 RVA: 0x00195314 File Offset: 0x00193514
		public override bool canInspect
		{
			get
			{
				return !this.isShooting && !this.isReloading && !this.isHammering && !this.isUnjamming && !this.isSprinting && !this.isAttaching && !this.isAiming && !this.needsRechamber;
			}
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x00195364 File Offset: 0x00193564
		public override void equip()
		{
			this.lastShot = float.MaxValue;
			Transform firstModel = base.player.equipment.firstModel;
			this.firstEventComponent = ((firstModel != null) ? firstModel.GetComponent<UseableGunEventHook>() : null);
			Transform thirdModel = base.player.equipment.thirdModel;
			this.thirdEventComponent = ((thirdModel != null) ? thirdModel.GetComponent<UseableGunEventHook>() : null);
			Transform characterModel = base.player.equipment.characterModel;
			this.characterEventComponent = ((characterModel != null) ? characterModel.GetComponent<UseableGunEventHook>() : null);
			if (base.channel.IsLocalPlayer)
			{
				this.firstAttachments = base.player.equipment.firstModel.gameObject.GetComponent<Attachments>();
				this.firstMinigunBarrel = this.firstAttachments.transform.Find("Model_1");
				Transform transform = this.firstAttachments.transform.FindChildRecursive("Ammo_Counter");
				if (transform != null)
				{
					this.firstAmmoCounter = transform.GetComponent<Text>();
					transform.parent.gameObject.SetActive(true);
					transform.parent.gameObject.layer = 11;
					transform.gameObject.layer = 11;
				}
				if (this.firstAttachments.rope != null)
				{
					this.firstAttachments.rope.gameObject.SetActive(true);
				}
				if (this.firstAttachments.ejectHook != null && this.equippedGunAsset.action != EAction.String && this.equippedGunAsset.action != EAction.Rocket)
				{
					EffectAsset effectAsset = this.equippedGunAsset.FindShellEffectAsset();
					if (effectAsset != null)
					{
						Transform transform2 = EffectManager.InstantiateFromPool(effectAsset).transform;
						transform2.name = "Emitter";
						transform2.parent = this.firstAttachments.ejectHook;
						transform2.localPosition = Vector3.zero;
						transform2.localRotation = Quaternion.identity;
						transform2.tag = "Viewmodel";
						transform2.gameObject.layer = 11;
						this.firstShellEmitter = transform2.GetComponent<ParticleSystem>();
					}
				}
				if (this.firstAttachments.barrelHook != null)
				{
					EffectAsset effectAsset2 = this.equippedGunAsset.FindMuzzleEffectAsset();
					if (effectAsset2 != null)
					{
						Transform transform3 = EffectManager.InstantiateFromPool(effectAsset2).transform;
						transform3.name = "Emitter";
						transform3.parent = this.firstAttachments.barrelHook;
						transform3.localPosition = Vector3.zero;
						transform3.localRotation = Quaternion.identity;
						transform3.tag = "Viewmodel";
						transform3.gameObject.layer = 11;
						this.firstMuzzleEmitter = transform3.GetComponent<ParticleSystem>();
						this.firstMuzzleEmitter.main.simulationSpace = 0;
						Light component = transform3.GetComponent<Light>();
						if (component != null)
						{
							component.enabled = false;
							component.cullingMask = 2048;
						}
					}
				}
				if (this.equippedGunAsset.isTurret)
				{
					base.player.animator.viewmodelCameraLocalPositionOffset = Vector3.up;
				}
				base.player.animator.turretViewmodelCameraLocalPositionOffset = Vector3.zero;
			}
			this.thirdAttachments = base.player.equipment.thirdModel.gameObject.GetComponent<Attachments>();
			if (base.channel.IsLocalPlayer)
			{
				Transform transform4 = this.thirdAttachments.transform.FindChildRecursive("Ammo_Counter");
				if (transform4 != null)
				{
					this.thirdAmmoCounter = transform4.GetComponent<Text>();
					transform4.parent.gameObject.SetActive(true);
					transform4.parent.gameObject.layer = 10;
					transform4.gameObject.layer = 10;
				}
			}
			this.thirdMinigunBarrel = this.thirdAttachments.transform.Find("Model_1");
			if (this.thirdAttachments.ejectHook != null && this.equippedGunAsset.action != EAction.String && this.equippedGunAsset.action != EAction.Rocket)
			{
				EffectAsset effectAsset3 = this.equippedGunAsset.FindShellEffectAsset();
				if (effectAsset3 != null)
				{
					Transform transform5 = EffectManager.InstantiateFromPool(effectAsset3).transform;
					transform5.name = "Emitter";
					transform5.localPosition = Vector3.zero;
					this.thirdShellEmitter = transform5.GetComponent<ParticleSystem>();
					this.thirdShellRenderer = transform5.GetComponent<ParticleSystemRenderer>();
					if (base.channel.IsLocalPlayer)
					{
						this.thirdShellEmitter.collision.enabled = true;
						ParticleSystem.TriggerModule trigger = this.thirdShellEmitter.trigger;
						trigger.enabled = true;
						trigger.inside = 0;
						trigger.outside = 0;
						trigger.enter = 2;
						trigger.exit = 0;
						List<WaterVolume> list = VolumeManager<WaterVolume, WaterVolumeManager>.Get().InternalGetAllVolumes();
						for (int i = 0; i < list.Count; i++)
						{
							trigger.SetCollider(i, list[i].volumeCollider);
						}
						if (base.player.look.perspective == EPlayerPerspective.FIRST)
						{
							this.thirdShellRenderer.forceRenderingOff = true;
						}
					}
				}
			}
			if (this.thirdAttachments.barrelHook != null)
			{
				EffectAsset effectAsset4 = this.equippedGunAsset.FindMuzzleEffectAsset();
				if (effectAsset4 != null)
				{
					Transform transform6 = EffectManager.InstantiateFromPool(effectAsset4).transform;
					transform6.name = "Emitter";
					transform6.parent = (this.equippedGunAsset.isTurret ? null : this.thirdAttachments.barrelHook);
					transform6.localPosition = Vector3.zero;
					transform6.localRotation = Quaternion.identity;
					this.thirdMuzzleEmitter = transform6.GetComponent<ParticleSystem>();
					Light component2 = transform6.GetComponent<Light>();
					if (component2 != null)
					{
						component2.enabled = false;
						component2.cullingMask = -2049;
					}
				}
				if (base.channel.IsLocalPlayer && effectAsset4 != null)
				{
					this.firstFakeLight = Object.Instantiate<GameObject>(effectAsset4.effect).transform;
					this.firstFakeLight.name = "Emitter";
					Light component3 = this.firstFakeLight.GetComponent<Light>();
					if (component3 != null)
					{
						component3.enabled = false;
						component3.cullingMask = -2049;
					}
				}
			}
			this.ammo = base.player.equipment.state[10];
			this.firemode = (EFiremode)base.player.equipment.state[11];
			this.interact = (base.player.equipment.state[12] == 1);
			this.updateAttachments(true);
			this.startedReload = float.MaxValue;
			this.startedHammer = float.MaxValue;
			if (base.channel.IsLocalPlayer)
			{
				if (this.firemode == EFiremode.SAFETY)
				{
					PlayerUI.message(EPlayerMessage.SAFETY, "", 2f);
				}
				else if (this.ammo < this.equippedGunAsset.ammoPerShot)
				{
					PlayerUI.message(EPlayerMessage.RELOAD, "", 2f);
				}
				if (this.firstAttachments.reticuleHook != null)
				{
					this.originalReticuleHookLocalPosition = this.firstAttachments.reticuleHook.localPosition;
				}
				else
				{
					this.originalReticuleHookLocalPosition = Vector3.zero;
				}
				this.localization = Localization.read("/Player/Useable/PlayerUseableGun.dat");
				if (this.icons != null)
				{
					this.icons.unload();
				}
				this.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/Useable/PlayerUseableGun/PlayerUseableGun.unity3d");
				if (this.equippedGunAsset.hasSight)
				{
					this.sightButton = new SleekButtonIcon(this.icons.load<Texture2D>("Sight"));
					this.sightButton.PositionOffset_X = -25f;
					this.sightButton.PositionOffset_Y = -25f;
					this.sightButton.SizeOffset_X = 50f;
					this.sightButton.SizeOffset_Y = 50f;
					this.sightButton.onClickedButton += new ClickedButton(this.onClickedSightHookButton);
					PlayerUI.container.AddChild(this.sightButton);
					this.sightButton.IsVisible = false;
				}
				if (this.equippedGunAsset.hasTactical)
				{
					this.tacticalButton = new SleekButtonIcon(this.icons.load<Texture2D>("Tactical"));
					this.tacticalButton.PositionOffset_X = -25f;
					this.tacticalButton.PositionOffset_Y = -25f;
					this.tacticalButton.SizeOffset_X = 50f;
					this.tacticalButton.SizeOffset_Y = 50f;
					this.tacticalButton.onClickedButton += new ClickedButton(this.onClickedTacticalHookButton);
					PlayerUI.container.AddChild(this.tacticalButton);
					this.tacticalButton.IsVisible = false;
				}
				if (this.equippedGunAsset.hasGrip)
				{
					this.gripButton = new SleekButtonIcon(this.icons.load<Texture2D>("Grip"));
					this.gripButton.PositionOffset_X = -25f;
					this.gripButton.PositionOffset_Y = -25f;
					this.gripButton.SizeOffset_X = 50f;
					this.gripButton.SizeOffset_Y = 50f;
					this.gripButton.onClickedButton += new ClickedButton(this.onClickedGripHookButton);
					PlayerUI.container.AddChild(this.gripButton);
					this.gripButton.IsVisible = false;
				}
				if (this.equippedGunAsset.hasBarrel)
				{
					this.barrelButton = new SleekButtonIcon(this.icons.load<Texture2D>("Barrel"));
					this.barrelButton.PositionOffset_X = -25f;
					this.barrelButton.PositionOffset_Y = -25f;
					this.barrelButton.SizeOffset_X = 50f;
					this.barrelButton.SizeOffset_Y = 50f;
					this.barrelButton.onClickedButton += new ClickedButton(this.onClickedBarrelHookButton);
					PlayerUI.container.AddChild(this.barrelButton);
					this.barrelButton.IsVisible = false;
					this.barrelQualityLabel = Glazier.Get().CreateLabel();
					this.barrelQualityLabel.PositionOffset_Y = -30f;
					this.barrelQualityLabel.PositionScale_Y = 1f;
					this.barrelQualityLabel.SizeOffset_Y = 30f;
					this.barrelQualityLabel.SizeScale_X = 1f;
					this.barrelQualityLabel.TextAlignment = 6;
					this.barrelQualityLabel.FontSize = 1;
					this.barrelButton.AddChild(this.barrelQualityLabel);
					this.barrelQualityLabel.IsVisible = false;
					this.barrelQualityImage = Glazier.Get().CreateImage();
					this.barrelQualityImage.PositionOffset_X = -15f;
					this.barrelQualityImage.PositionOffset_Y = -15f;
					this.barrelQualityImage.PositionScale_X = 1f;
					this.barrelQualityImage.PositionScale_Y = 1f;
					this.barrelQualityImage.SizeOffset_X = 10f;
					this.barrelQualityImage.SizeOffset_Y = 10f;
					this.barrelQualityImage.Texture = PlayerDashboardInventoryUI.icons.load<Texture2D>("Quality_1");
					this.barrelButton.AddChild(this.barrelQualityImage);
					this.barrelQualityImage.IsVisible = false;
				}
				if (this.equippedGunAsset.allowMagazineChange)
				{
					this.magazineButton = new SleekButtonIcon(this.icons.load<Texture2D>("Magazine"));
					this.magazineButton.PositionOffset_X = -25f;
					this.magazineButton.PositionOffset_Y = -25f;
					this.magazineButton.SizeOffset_X = 50f;
					this.magazineButton.SizeOffset_Y = 50f;
					this.magazineButton.onClickedButton += new ClickedButton(this.onClickedMagazineHookButton);
					PlayerUI.container.AddChild(this.magazineButton);
					this.magazineButton.IsVisible = false;
					this.magazineQualityLabel = Glazier.Get().CreateLabel();
					this.magazineQualityLabel.PositionOffset_Y = -30f;
					this.magazineQualityLabel.PositionScale_Y = 1f;
					this.magazineQualityLabel.SizeOffset_Y = 30f;
					this.magazineQualityLabel.SizeScale_X = 1f;
					this.magazineQualityLabel.TextAlignment = 6;
					this.magazineQualityLabel.FontSize = 1;
					this.magazineQualityLabel.TextContrastContext = 1;
					this.magazineButton.AddChild(this.magazineQualityLabel);
					this.magazineQualityLabel.IsVisible = false;
					this.magazineQualityImage = Glazier.Get().CreateImage();
					this.magazineQualityImage.PositionOffset_X = -15f;
					this.magazineQualityImage.PositionOffset_Y = -15f;
					this.magazineQualityImage.PositionScale_X = 1f;
					this.magazineQualityImage.PositionScale_Y = 1f;
					this.magazineQualityImage.SizeOffset_X = 10f;
					this.magazineQualityImage.SizeOffset_Y = 10f;
					this.magazineQualityImage.Texture = PlayerDashboardInventoryUI.icons.load<Texture2D>("Quality_1");
					this.magazineButton.AddChild(this.magazineQualityImage);
					this.magazineQualityImage.IsVisible = false;
				}
				this.icons.unload();
				this.infoBox = Glazier.Get().CreateBox();
				this.infoBox.PositionOffset_Y = -70f;
				this.infoBox.PositionScale_X = 0.7f;
				this.infoBox.PositionScale_Y = 1f;
				this.infoBox.SizeOffset_Y = 70f;
				this.infoBox.SizeScale_X = 0.3f;
				PlayerLifeUI.container.AddChild(this.infoBox);
				this.ammoLabel = Glazier.Get().CreateLabel();
				this.ammoLabel.SizeScale_X = 0.35f;
				this.ammoLabel.SizeScale_Y = 1f;
				this.ammoLabel.FontSize = 4;
				this.infoBox.AddChild(this.ammoLabel);
				this.firemodeLabel = Glazier.Get().CreateLabel();
				this.firemodeLabel.PositionOffset_Y = 5f;
				this.firemodeLabel.PositionScale_X = 0.35f;
				this.firemodeLabel.SizeScale_X = 0.65f;
				this.firemodeLabel.SizeScale_Y = 0.5f;
				this.infoBox.AddChild(this.firemodeLabel);
				this.attachLabel = Glazier.Get().CreateLabel();
				this.attachLabel.PositionOffset_Y = -5f;
				this.attachLabel.PositionScale_X = 0.35f;
				this.attachLabel.PositionScale_Y = 0.5f;
				this.attachLabel.SizeScale_X = 0.65f;
				this.attachLabel.SizeScale_Y = 0.5f;
				this.attachLabel.TextContrastContext = 1;
				this.infoBox.AddChild(this.attachLabel);
				base.player.onLocalPluginWidgetFlagsChanged += this.OnLocalPluginWidgetFlagsChanged;
				this.UpdateInfoBoxVisibility();
				this.updateInfo();
			}
			base.player.animator.play("Equip", false);
			if (base.player.channel.IsLocalPlayer)
			{
				PlayerUI.disableDot();
				PlayerStance stance = base.player.stance;
				stance.onStanceUpdated = (StanceUpdated)Delegate.Combine(stance.onStanceUpdated, new StanceUpdated(this.UpdateCrosshairEnabled));
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
				OptionsSettings.OnUnitSystemChanged += new Action(this.SyncScopeDistanceMarkerText);
			}
			if ((base.channel.IsLocalPlayer || Provider.isServer) && this.equippedGunAsset.projectile == null)
			{
				this.bullets = new List<BulletInfo>();
			}
			this.aimAccuracy = 0;
			this.steadyAccuracy = 0U;
			this.canSteady = true;
			this.swayTime = Time.time;
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x001962A0 File Offset: 0x001944A0
		public override void dequip()
		{
			if (this.infoBox != null)
			{
				if (this.sightButton != null)
				{
					PlayerUI.container.RemoveChild(this.sightButton);
				}
				if (this.tacticalButton != null)
				{
					PlayerUI.container.RemoveChild(this.tacticalButton);
				}
				if (this.gripButton != null)
				{
					PlayerUI.container.RemoveChild(this.gripButton);
				}
				if (this.barrelButton != null)
				{
					PlayerUI.container.RemoveChild(this.barrelButton);
				}
				if (this.magazineButton != null)
				{
					PlayerUI.container.RemoveChild(this.magazineButton);
				}
				if (this.rangeLabel != null)
				{
					this.rangeLabel.Parent.RemoveChild(this.rangeLabel);
				}
				PlayerLifeUI.container.RemoveChild(this.infoBox);
				base.player.onLocalPluginWidgetFlagsChanged -= this.OnLocalPluginWidgetFlagsChanged;
			}
			base.player.disableItemSpotLight();
			if (base.channel.IsLocalPlayer)
			{
				if (this.equippedGunAsset.isTurret)
				{
					base.player.animator.viewmodelCameraLocalPositionOffset = Vector3.zero;
				}
				base.player.animator.turretViewmodelCameraLocalPositionOffset = Vector3.zero;
				if (this.gunshotAudioSource != null)
				{
					Object.Destroy(this.gunshotAudioSource);
				}
				if (this.whir != null)
				{
					Object.Destroy(this.whir);
				}
				this.DestroyLaser();
				if (this.isAiming)
				{
					this.stopAim();
				}
				if (this.isAttaching)
				{
					this.stopAttach();
				}
				PlayerUI.isLocked = false;
				if (this.isAttaching)
				{
					PlayerLifeUI.open();
				}
				if (base.player.movement.getVehicle() == null)
				{
					PlayerUI.enableDot();
				}
				PlayerUI.disableCrosshair();
				base.player.look.disableScope();
				PlayerStance stance = base.player.stance;
				stance.onStanceUpdated = (StanceUpdated)Delegate.Remove(stance.onStanceUpdated, new StanceUpdated(this.UpdateCrosshairEnabled));
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Remove(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
				OptionsSettings.OnUnitSystemChanged -= new Action(this.SyncScopeDistanceMarkerText);
				if (this.firstFakeLight != null)
				{
					Object.Destroy(this.firstFakeLight.gameObject);
				}
				if (this.firstFakeLight_0 != null)
				{
					Object.Destroy(this.firstFakeLight_0.gameObject);
				}
				if (this.firstFakeLight_1 != null)
				{
					Object.Destroy(this.firstFakeLight_1.gameObject);
				}
			}
			if (this.tracerEmitter != null)
			{
				EffectManager.DestroyIntoPool(this.tracerEmitter.gameObject);
				this.tracerEmitter = null;
			}
			if (this.firstMuzzleEmitter != null)
			{
				EffectManager.DestroyIntoPool(this.firstMuzzleEmitter.gameObject);
				this.firstMuzzleEmitter = null;
			}
			if (this.firstShellEmitter != null)
			{
				EffectManager.DestroyIntoPool(this.firstShellEmitter.gameObject);
				this.firstShellEmitter = null;
			}
			if (this.thirdMuzzleEmitter != null)
			{
				EffectManager.DestroyIntoPool(this.thirdMuzzleEmitter.gameObject);
				this.thirdMuzzleEmitter = null;
			}
			if (this.thirdShellEmitter != null)
			{
				if (base.channel.IsLocalPlayer)
				{
					this.thirdShellEmitter.collision.enabled = false;
					this.thirdShellEmitter.trigger.enabled = false;
				}
				if (this.thirdShellRenderer != null)
				{
					this.thirdShellRenderer.forceRenderingOff = false;
				}
				EffectManager.DestroyIntoPool(this.thirdShellEmitter.gameObject);
				this.thirdShellEmitter = null;
			}
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x0019662C File Offset: 0x0019482C
		public override void tick()
		{
			if (base.channel.IsLocalPlayer && this.firstAttachments.rope != null)
			{
				if (this.firstAttachments.leftHook != null)
				{
					this.firstAttachments.rope.SetPosition(0, this.firstAttachments.leftHook.position);
				}
				if (this.firstAttachments.nockHook != null)
				{
					if (this.firstAttachments.magazineModel != null && this.firstAttachments.magazineModel.gameObject.activeSelf)
					{
						this.firstAttachments.rope.SetPosition(1, this.firstAttachments.nockHook.position);
					}
					else if (this.firstAttachments.restHook != null)
					{
						this.firstAttachments.rope.SetPosition(1, this.firstAttachments.restHook.position);
					}
				}
				else if (this.isAiming)
				{
					this.firstAttachments.rope.SetPosition(1, base.player.equipment.firstRightHook.position);
				}
				else if ((this.isAttaching || this.isSprinting || base.player.equipment.isInspecting) && this.firstAttachments.magazineModel != null && this.firstAttachments.magazineModel.gameObject.activeSelf && this.firstAttachments.restHook != null)
				{
					this.firstAttachments.rope.SetPosition(1, this.firstAttachments.restHook.position);
				}
				else if (this.firstAttachments.leftHook != null)
				{
					this.firstAttachments.rope.SetPosition(1, this.firstAttachments.leftHook.position);
				}
				if (this.firstAttachments.rightHook != null)
				{
					this.firstAttachments.rope.SetPosition(2, this.firstAttachments.rightHook.position);
				}
			}
			if (!base.player.equipment.IsEquipAnimationFinished)
			{
				return;
			}
			if ((double)(Time.realtimeSinceStartup - this.lastShot) > 0.05)
			{
				if (this.firstMuzzleEmitter != null)
				{
					Light component = this.firstMuzzleEmitter.GetComponent<Light>();
					if (component)
					{
						component.enabled = false;
					}
				}
				if (this.thirdMuzzleEmitter != null)
				{
					Light component2 = this.thirdMuzzleEmitter.GetComponent<Light>();
					if (component2)
					{
						component2.enabled = false;
					}
				}
				if (this.firstFakeLight != null)
				{
					Light component3 = this.firstFakeLight.GetComponent<Light>();
					if (component3)
					{
						component3.enabled = false;
					}
				}
			}
			if ((base.player.stance.stance == EPlayerStance.SPRINT && base.player.movement.isMoving) || this.firemode == EFiremode.SAFETY)
			{
				if (!this.isShooting && !this.isSprinting && !this.isReloading && !this.isHammering && !this.isUnjamming && !this.isAttaching && !this.isAiming && !this.needsRechamber)
				{
					this.isSprinting = true;
					base.player.animator.play("Sprint_Start", false);
				}
			}
			else if (this.isSprinting)
			{
				this.isSprinting = false;
				if (!this.isAiming)
				{
					base.player.animator.play("Sprint_Stop", false);
				}
			}
			if (base.channel.IsLocalPlayer)
			{
				if (InputEx.GetKeyUp(ControlsSettings.attach) && this.isAttaching)
				{
					this.isAttaching = false;
					base.player.animator.play("Attach_Stop", false);
					this.stopAttach();
				}
				if (InputEx.GetKeyDown(ControlsSettings.tactical))
				{
					this.fireTacticalInput = true;
				}
				if (!PlayerUI.window.showCursor)
				{
					if (InputEx.ConsumeKeyDown(ControlsSettings.attach) && !this.isShooting && !this.isAttaching && !this.isSprinting && !this.isReloading && !this.isHammering && !this.isUnjamming && !this.isAiming && !this.needsRechamber)
					{
						this.isAttaching = true;
						base.player.animator.play("Attach_Start", false);
						this.updateAttach();
						this.startAttach();
					}
					if (InputEx.GetKeyDown(ControlsSettings.reload) && !this.isShooting && !this.isReloading && !this.isHammering && !this.isUnjamming && !this.isSprinting && !this.isAttaching && !this.isAiming && !this.needsRechamber)
					{
						bool allowZeroCaliber = !this.equippedGunAsset.requiresNonZeroAttachmentCaliber;
						this.magazineSearch = base.player.inventory.search(EItemType.MAGAZINE, this.equippedGunAsset.magazineCalibers, allowZeroCaliber);
						if (this.magazineSearch.Count > 0)
						{
							byte b = 0;
							byte b2 = byte.MaxValue;
							byte b3 = 0;
							while ((int)b3 < this.magazineSearch.Count)
							{
								if (this.magazineSearch[(int)b3].jar.item.amount > b)
								{
									b = this.magazineSearch[(int)b3].jar.item.amount;
									b2 = b3;
								}
								b3 += 1;
							}
							if (b2 != 255)
							{
								ItemAsset asset = this.magazineSearch[(int)b2].GetAsset();
								if (asset != null)
								{
									UseableGun.SendAttachMagazine.Invoke(base.GetNetId(), ENetReliability.Unreliable, this.magazineSearch[(int)b2].page, this.magazineSearch[(int)b2].jar.x, this.magazineSearch[(int)b2].jar.y, asset.hash);
								}
							}
						}
					}
					if (InputEx.GetKeyDown(ControlsSettings.firemode) && !this.isAiming)
					{
						if (this.firemode == EFiremode.SAFETY)
						{
							if (this.equippedGunAsset.hasSemi)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.SEMI);
							}
							else if (this.equippedGunAsset.hasAuto)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.AUTO);
							}
							else if (this.equippedGunAsset.hasBurst)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.BURST);
							}
							PlayerUI.message(EPlayerMessage.NONE, "", 2f);
						}
						else if (this.firemode == EFiremode.SEMI)
						{
							if (this.equippedGunAsset.hasAuto)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.AUTO);
							}
							else if (this.equippedGunAsset.hasBurst)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.BURST);
							}
							else if (this.equippedGunAsset.hasSafety)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.SAFETY);
								PlayerUI.message(EPlayerMessage.SAFETY, "", 2f);
							}
						}
						else if (this.firemode == EFiremode.AUTO)
						{
							if (this.equippedGunAsset.hasBurst)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.BURST);
							}
							else if (this.equippedGunAsset.hasSafety)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.SAFETY);
								PlayerUI.message(EPlayerMessage.SAFETY, "", 2f);
							}
							else if (this.equippedGunAsset.hasSemi)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.SEMI);
							}
						}
						else if (this.firemode == EFiremode.BURST)
						{
							if (this.equippedGunAsset.hasSafety)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.SAFETY);
								PlayerUI.message(EPlayerMessage.SAFETY, "", 2f);
							}
							else if (this.equippedGunAsset.hasSemi)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.SEMI);
							}
							else if (this.equippedGunAsset.hasAuto)
							{
								UseableGun.SendChangeFiremode.Invoke(base.GetNetId(), ENetReliability.Reliable, EFiremode.AUTO);
							}
						}
					}
				}
				if (this.isAttaching)
				{
					if (this.sightButton != null)
					{
						if (base.player.look.perspective == EPlayerPerspective.FIRST && !this.equippedGunAsset.isTurret)
						{
							Vector3 v = base.player.animator.viewmodelCamera.WorldToViewportPoint(this.firstAttachments.sightHook.position + this.firstAttachments.sightHook.up * 0.05f + this.firstAttachments.sightHook.forward * 0.05f);
							Vector2 vector = PlayerUI.container.ViewportToNormalizedPosition(v);
							this.sightButton.PositionScale_X = vector.x;
							this.sightButton.PositionScale_Y = vector.y;
						}
						else
						{
							this.sightButton.PositionScale_X = 0.667f;
							this.sightButton.PositionScale_Y = 0.75f;
						}
					}
					if (this.tacticalButton != null)
					{
						if (base.player.look.perspective == EPlayerPerspective.FIRST && !this.equippedGunAsset.isTurret)
						{
							Vector3 v2 = base.player.animator.viewmodelCamera.WorldToViewportPoint(this.firstAttachments.tacticalHook.position);
							Vector2 vector2 = PlayerUI.container.ViewportToNormalizedPosition(v2);
							this.tacticalButton.PositionScale_X = vector2.x;
							this.tacticalButton.PositionScale_Y = vector2.y;
						}
						else
						{
							this.tacticalButton.PositionScale_X = 0.5f;
							this.tacticalButton.PositionScale_Y = 0.25f;
						}
					}
					if (this.gripButton != null)
					{
						if (base.player.look.perspective == EPlayerPerspective.FIRST && !this.equippedGunAsset.isTurret)
						{
							Vector3 v3 = base.player.animator.viewmodelCamera.WorldToViewportPoint(this.firstAttachments.gripHook.position + this.firstAttachments.gripHook.forward * -0.05f);
							Vector2 vector3 = PlayerUI.container.ViewportToNormalizedPosition(v3);
							this.gripButton.PositionScale_X = vector3.x;
							this.gripButton.PositionScale_Y = vector3.y;
						}
						else
						{
							this.gripButton.PositionScale_X = 0.75f;
							this.gripButton.PositionScale_Y = 0.25f;
						}
					}
					if (this.barrelButton != null)
					{
						if (base.player.look.perspective == EPlayerPerspective.FIRST && !this.equippedGunAsset.isTurret)
						{
							Vector3 v4 = base.player.animator.viewmodelCamera.WorldToViewportPoint(this.firstAttachments.barrelHook.position + this.firstAttachments.barrelHook.up * 0.05f);
							Vector2 vector4 = PlayerUI.container.ViewportToNormalizedPosition(v4);
							this.barrelButton.PositionScale_X = vector4.x;
							this.barrelButton.PositionScale_Y = vector4.y;
						}
						else
						{
							this.barrelButton.PositionScale_X = 0.25f;
							this.barrelButton.PositionScale_Y = 0.25f;
						}
					}
					if (this.magazineButton != null)
					{
						if (base.player.look.perspective == EPlayerPerspective.FIRST && !this.equippedGunAsset.isTurret)
						{
							Vector2 vector5 = base.player.animator.viewmodelCamera.WorldToViewportPoint(this.firstAttachments.magazineHook.position + this.firstAttachments.magazineHook.forward * -0.1f);
							Vector2 vector6 = PlayerUI.container.ViewportToNormalizedPosition(vector5);
							this.magazineButton.PositionScale_X = vector6.x;
							this.magazineButton.PositionScale_Y = vector6.y;
						}
						else
						{
							this.magazineButton.PositionScale_X = 0.334f;
							this.magazineButton.PositionScale_Y = 0.75f;
						}
					}
				}
				if (this.rangeLabel != null)
				{
					if (PlayerLifeUI.scopeOverlay.IsVisible)
					{
						this.rangeLabel.PositionOffset_X = -300f;
						this.rangeLabel.PositionOffset_Y = 100f;
						this.rangeLabel.PositionScale_X = 0.5f;
						this.rangeLabel.PositionScale_Y = 0.5f;
						this.rangeLabel.TextAlignment = 2;
					}
					else
					{
						Vector3 v5;
						if (base.player.look.perspective == EPlayerPerspective.FIRST && this.firstAttachments.lightHook != null)
						{
							v5 = base.player.animator.viewmodelCamera.WorldToViewportPoint(this.firstAttachments.lightHook.position);
						}
						else if (this.thirdAttachments.lightHook != null)
						{
							v5 = MainCamera.instance.WorldToViewportPoint(this.thirdAttachments.lightHook.position);
						}
						else
						{
							v5 = Vector3.zero;
						}
						Vector2 vector7 = PlayerLifeUI.container.ViewportToNormalizedPosition(v5);
						this.rangeLabel.PositionOffset_X = -100f;
						this.rangeLabel.PositionOffset_Y = -15f;
						this.rangeLabel.PositionScale_X = vector7.x;
						this.rangeLabel.PositionScale_Y = vector7.y;
						this.rangeLabel.TextAlignment = 4;
					}
					this.rangeLabel.IsVisible = true;
				}
			}
			if (this.needsRechamber && Time.realtimeSinceStartup - this.lastShot > 0.25f && !this.isAiming)
			{
				this.needsRechamber = false;
				base.player.equipment.isBusy = false;
				this.lastRechamber = Time.realtimeSinceStartup;
				this.needsEject = true;
				this.hammer();
			}
			if (this.needsEject && Time.realtimeSinceStartup - this.lastRechamber > 0.45f)
			{
				this.needsEject = false;
				if (this.firstShellEmitter != null && base.player.look.perspective == EPlayerPerspective.FIRST && !this.equippedGunAsset.isTurret)
				{
					this.firstShellEmitter.Emit(1);
				}
				if (this.thirdShellEmitter != null)
				{
					this.thirdShellEmitter.Emit(1);
				}
			}
			if (this.needsUnload && Time.realtimeSinceStartup - this.startedReload > 0.5f)
			{
				this.needsUnload = false;
				if (this.firstShellEmitter != null && base.player.look.perspective == EPlayerPerspective.FIRST && !this.equippedGunAsset.isTurret)
				{
					this.firstShellEmitter.Emit((int)this.equippedGunAsset.ammoMax);
				}
				if (this.thirdShellEmitter != null)
				{
					this.thirdShellEmitter.Emit((int)this.equippedGunAsset.ammoMax);
				}
			}
			if (this.needsUnplace && Time.realtimeSinceStartup - this.startedReload > this.reloadTime * this.equippedGunAsset.unplace)
			{
				this.needsUnplace = false;
				if (base.channel.IsLocalPlayer && this.firstAttachments.magazineModel != null)
				{
					this.firstAttachments.magazineModel.gameObject.SetActive(false);
				}
				if (this.thirdAttachments.magazineModel != null)
				{
					this.thirdAttachments.magazineModel.gameObject.SetActive(false);
				}
			}
			if (this.needsReplace && Time.realtimeSinceStartup - this.startedReload > this.reloadTime * this.equippedGunAsset.replace)
			{
				this.needsReplace = false;
				if (base.channel.IsLocalPlayer && this.firstAttachments.magazineModel != null)
				{
					this.firstAttachments.magazineModel.gameObject.SetActive(true);
				}
				if (this.thirdAttachments.magazineModel != null)
				{
					this.thirdAttachments.magazineModel.gameObject.SetActive(true);
				}
			}
			if (this.isReloading && Time.realtimeSinceStartup - this.startedReload > this.reloadTime)
			{
				this.isReloading = false;
				if (this.needsHammer)
				{
					this.hammer();
				}
				else
				{
					base.player.equipment.isBusy = false;
				}
			}
			if (this.isHammering && Time.realtimeSinceStartup - this.startedHammer > this.hammerTime)
			{
				this.isHammering = false;
				base.player.equipment.isBusy = false;
			}
			if (this.isUnjamming && Time.realtimeSinceStartup - this.startedUnjammingChamber > this.unjamChamberDuration)
			{
				this.isUnjamming = false;
				base.player.equipment.isBusy = false;
			}
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x001976C4 File Offset: 0x001958C4
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isFired && Time.realtimeSinceStartup - this.lastShot > 0.15f)
			{
				this.isFired = false;
				if (!this.needsRechamber)
				{
					base.player.equipment.isBusy = false;
				}
			}
			if (!this.canSteady && !inputSteady && base.player.life.oxygen > 10)
			{
				this.canSteady = true;
			}
			if (this.isAiming && this.thirdAttachments.sightAsset != null && this.thirdAttachments.sightAsset.zoom > 2f && base.player.life.oxygen > 0 && this.canSteady && inputSteady)
			{
				if (this.steadyAccuracy < 4U)
				{
					this.steadyAccuracy += 1U;
				}
				base.player.life.askSuffocate(5 - base.player.skills.skills[0][5].level / 2);
				if (base.player.life.oxygen == 0)
				{
					this.canSteady = false;
				}
			}
			else if (this.steadyAccuracy > 0U)
			{
				this.steadyAccuracy -= 1U;
			}
			if (base.channel.IsLocalPlayer && base.player.equipment.IsEquipAnimationFinished && this.fireTacticalInput)
			{
				if (!this.isReloading && !this.isHammering && !this.isUnjamming && !this.needsRechamber && this.thirdAttachments.tacticalAsset != null)
				{
					if (this.thirdAttachments.tacticalAsset.isMelee)
					{
						if (!this.isSprinting && (!base.player.movement.isSafe || !base.player.movement.isSafeInfo.noWeapons) && this.firemode != EFiremode.SAFETY)
						{
							if (!Provider.isServer)
							{
								this.isJabbing = true;
							}
							base.player.input.keys[8] = true;
						}
					}
					else if (this.thirdAttachments.tacticalAsset.isLight || this.thirdAttachments.tacticalAsset.isLaser || this.thirdAttachments.tacticalAsset.isRangefinder)
					{
						base.player.input.keys[8] = true;
					}
				}
				this.fireTacticalInput = false;
			}
			if (Provider.isServer && base.player.input.keys[8])
			{
				this.askInteractGun();
			}
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x00197944 File Offset: 0x00195B44
		private void tockShoot(uint clock)
		{
			if (this.firemode == EFiremode.SAFETY | this.isReloading | this.isHammering | this.isUnjamming | (base.player.stance.stance == EPlayerStance.SPRINT && !this.equippedGunAsset.canAimDuringSprint) | this.isAttaching | (!base.player.equipment.asset.canUseUnderwater && (base.player.stance.isSubmerged || base.player.stance.stance == EPlayerStance.SWIM)))
			{
				this.bursts = 0;
				this.fireDelayCounter = 0;
				this.isShooting = false;
				this.wasTriggerJustPulled = false;
				return;
			}
			bool flag = this.isShooting || this.wasTriggerJustPulled;
			this.wasTriggerJustPulled = false;
			if (this.fireDelayCounter > 1)
			{
				this.fireDelayCounter--;
				return;
			}
			if (this.fireDelayCounter > 0)
			{
				this.fireDelayCounter = 0;
				flag = true;
			}
			if (this.firemode == EFiremode.SEMI)
			{
				this.isShooting = false;
			}
			if (this.firemode == EFiremode.BURST)
			{
				this.isShooting = false;
				if (flag)
				{
					this.bursts += this.equippedGunAsset.bursts;
				}
			}
			int num = (int)this.equippedGunAsset.firerate;
			if (this.thirdAttachments.sightAsset != null)
			{
				num -= this.thirdAttachments.sightAsset.FirerateOffset;
			}
			if (this.thirdAttachments.tacticalAsset != null && this.shouldEnableTacticalStats)
			{
				num -= this.thirdAttachments.tacticalAsset.FirerateOffset;
			}
			if (this.thirdAttachments.gripAsset != null)
			{
				num -= this.thirdAttachments.gripAsset.FirerateOffset;
			}
			if (this.thirdAttachments.barrelAsset != null)
			{
				num -= this.thirdAttachments.barrelAsset.FirerateOffset;
			}
			if (this.thirdAttachments.magazineAsset != null)
			{
				num -= this.thirdAttachments.magazineAsset.FirerateOffset;
			}
			num = Mathf.Max(num, 0);
			if ((ulong)(clock - this.lastFire) > (ulong)((long)num))
			{
				if (this.bursts > 0)
				{
					this.bursts--;
				}
				if (this.ammo >= this.equippedGunAsset.ammoPerShot)
				{
					this.isFired = true;
					this.lastFire = clock;
					base.player.equipment.isBusy = true;
					this.fire();
					return;
				}
				if (Provider.isServer)
				{
					EffectManager.TriggerFiremodeEffect(base.transform.position);
				}
				this.bursts = 0;
				this.isShooting = false;
			}
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x00197BB9 File Offset: 0x00195DB9
		private void tockJab(uint clock)
		{
			this.isJabbing = false;
			if (clock - this.lastJab > 25U)
			{
				this.lastJab = clock;
				this.jab();
			}
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x00197BDC File Offset: 0x00195DDC
		public override void tock(uint clock)
		{
			if (this.isShooting || this.wasTriggerJustPulled || this.bursts > 0 || this.fireDelayCounter > 0)
			{
				this.tockShoot(clock);
			}
			if (this.isJabbing)
			{
				this.tockJab(clock);
			}
			this.ballistics();
			if (this.isAiming)
			{
				if (this.aimAccuracy < this.maxAimingAccuracy)
				{
					this.aimAccuracy++;
					return;
				}
			}
			else if (this.aimAccuracy > 0)
			{
				this.aimAccuracy--;
			}
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x00197C64 File Offset: 0x00195E64
		public override void updateState(byte[] newState)
		{
			this.ammo = newState[10];
			this.firemode = (EFiremode)newState[11];
			this.interact = (newState[12] == 1);
			bool wasMagazineModelVisible = this.thirdAttachments.magazineModel != null && this.thirdAttachments.magazineModel.gameObject.activeSelf;
			if (base.channel.IsLocalPlayer)
			{
				this.firstAttachments.updateAttachments(newState, true);
			}
			this.thirdAttachments.updateAttachments(newState, false);
			this.updateAttachments(wasMagazineModelVisible);
			if (base.channel.IsLocalPlayer)
			{
				if (this.firstAttachments.reticuleHook != null)
				{
					this.originalReticuleHookLocalPosition = this.firstAttachments.reticuleHook.localPosition;
				}
				else
				{
					this.originalReticuleHookLocalPosition = Vector3.zero;
				}
			}
			if (this.infoBox != null)
			{
				if (this.isAttaching)
				{
					this.updateAttach();
				}
				this.updateInfo();
			}
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x00197D4C File Offset: 0x00195F4C
		private void updateAnimationSpeeds(float speed)
		{
			base.player.animator.setAnimationSpeed("Reload", speed);
			this.reloadTime = base.player.animator.GetAnimationLength("Reload", true);
			this.reloadTime = Mathf.Max(this.reloadTime, this.equippedGunAsset.reloadTime / speed);
			base.player.animator.setAnimationSpeed("Hammer", speed);
			this.hammerTime = base.player.animator.GetAnimationLength("Hammer", true);
			base.player.animator.setAnimationSpeed("Scope", speed);
			this.hammerTime = Mathf.Max(this.hammerTime, this.equippedGunAsset.hammerTime / speed);
			this.unjamChamberDuration = base.player.animator.GetAnimationLength(this.equippedGunAsset.unjamChamberAnimName, true);
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x00197E34 File Offset: 0x00196034
		private void updateAttachments(bool wasMagazineModelVisible)
		{
			if (base.channel.IsLocalPlayer)
			{
				ClientAssetIntegrity.QueueRequest(this.firstAttachments.sightAsset);
				ClientAssetIntegrity.QueueRequest(this.firstAttachments.tacticalAsset);
				ClientAssetIntegrity.QueueRequest(this.firstAttachments.gripAsset);
				ClientAssetIntegrity.QueueRequest(this.firstAttachments.barrelAsset);
				ClientAssetIntegrity.QueueRequest(this.firstAttachments.magazineAsset);
				if (this.firstAttachments.tacticalAsset != null)
				{
					if (this.firstAttachments.tacticalAsset.isLaser)
					{
						if (!this.wasLaser)
						{
							PlayerUI.message(EPlayerMessage.LASER, "", 2f);
						}
						this.wasLaser = true;
					}
					else
					{
						this.wasLaser = false;
					}
					if (this.firstAttachments.tacticalAsset.isLight)
					{
						if (!this.wasLight)
						{
							PlayerUI.message(EPlayerMessage.LIGHT, "", 2f);
						}
						this.wasLight = true;
					}
					else
					{
						this.wasLight = false;
					}
					if (this.firstAttachments.tacticalAsset.isRangefinder)
					{
						if (!this.wasRange)
						{
							PlayerUI.message(EPlayerMessage.RANGEFINDER, "", 2f);
						}
						this.wasRange = true;
					}
					else
					{
						this.wasRange = false;
					}
					if (this.firstAttachments.tacticalAsset.isMelee)
					{
						if (!this.wasBayonet)
						{
							PlayerUI.message(EPlayerMessage.BAYONET, "", 2f);
						}
						this.wasBayonet = true;
					}
					else
					{
						this.wasBayonet = false;
					}
				}
				else
				{
					this.wasLaser = false;
					this.wasLight = false;
					this.wasRange = false;
					this.wasBayonet = false;
				}
				if (this.firstAttachments.tacticalAsset != null && this.firstAttachments.tacticalAsset.isRangefinder && this.interact)
				{
					if (this.rangeLabel == null)
					{
						this.rangeLabel = Glazier.Get().CreateLabel();
						this.rangeLabel.SizeOffset_X = 200f;
						this.rangeLabel.SizeOffset_Y = 30f;
						this.rangeLabel.TextContrastContext = 2;
						PlayerUI.window.AddChild(this.rangeLabel);
						this.rangeLabel.IsVisible = false;
					}
				}
				else if (this.rangeLabel != null)
				{
					this.rangeLabel.Parent.RemoveChild(this.rangeLabel);
					this.rangeLabel = null;
				}
				if (this.firstFakeLight_0 != null)
				{
					Object.Destroy(this.firstFakeLight_0.gameObject);
					this.firstFakeLight_0 = null;
				}
				if (this.thirdAttachments.lightHook != null)
				{
					Transform transform = this.thirdAttachments.lightHook.Find("Light");
					if (transform != null)
					{
						this.firstFakeLight_0 = Object.Instantiate<GameObject>(transform.gameObject).transform;
						this.firstFakeLight_0.name = "Emitter";
					}
				}
				if (this.firstFakeLight_1 != null)
				{
					Object.Destroy(this.firstFakeLight_1.gameObject);
					this.firstFakeLight_1 = null;
				}
				if (this.thirdAttachments.light2Hook != null)
				{
					Transform transform2 = this.thirdAttachments.light2Hook.Find("Light");
					if (transform2 != null)
					{
						this.firstFakeLight_1 = Object.Instantiate<GameObject>(transform2.gameObject).transform;
						this.firstFakeLight_1.name = "Emitter";
					}
				}
			}
			if (this.firstMuzzleEmitter != null)
			{
				if (this.firstAttachments.barrelModel != null)
				{
					Transform transform3 = this.firstAttachments.barrelModel.Find("Muzzle");
					if (transform3 != null)
					{
						this.firstMuzzleEmitter.transform.position = transform3.position;
					}
					else
					{
						this.firstMuzzleEmitter.transform.localPosition = Vector3.up * 0.25f;
					}
				}
				else
				{
					this.firstMuzzleEmitter.transform.localPosition = Vector3.zero;
				}
			}
			if (this.thirdMuzzleEmitter != null)
			{
				if (this.thirdAttachments.barrelModel != null)
				{
					Transform transform4 = this.thirdAttachments.barrelModel.Find("Muzzle");
					if (transform4 != null)
					{
						this.thirdMuzzleEmitter.transform.position = transform4.position;
					}
					else
					{
						this.thirdMuzzleEmitter.transform.localPosition = Vector3.up * 0.25f;
					}
				}
				else
				{
					this.thirdMuzzleEmitter.transform.localPosition = Vector3.zero;
				}
			}
			Attachments attachments = this.thirdAttachments;
			if (((attachments != null) ? attachments.magazineAsset : null) != null)
			{
				EffectAsset effectAsset = this.thirdAttachments.magazineAsset.FindTracerEffectAsset();
				if (this.currentTracerEffectAsset != effectAsset)
				{
					if (this.tracerEmitter != null)
					{
						EffectManager.DestroyIntoPool(this.tracerEmitter.gameObject);
						this.tracerEmitter = null;
					}
					this.currentTracerEffectAsset = effectAsset;
					if (effectAsset != null)
					{
						Transform transform5 = EffectManager.InstantiateFromPool(effectAsset).transform;
						transform5.name = "Tracer";
						transform5.localPosition = Vector3.zero;
						transform5.localRotation = Quaternion.identity;
						this.tracerEmitter = transform5.GetComponent<ParticleSystem>();
					}
				}
			}
			if (base.channel.IsLocalPlayer && this.firstAttachments.magazineModel != null)
			{
				this.firstAttachments.magazineModel.gameObject.SetActive(wasMagazineModelVisible);
			}
			if (this.thirdAttachments.magazineModel != null)
			{
				this.thirdAttachments.magazineModel.gameObject.SetActive(wasMagazineModelVisible);
			}
			if (this.thirdAttachments.tacticalAsset != null && this.thirdAttachments.tacticalAsset.isLight && this.interact)
			{
				base.player.enableItemSpotLight(this.thirdAttachments.tacticalAsset.lightConfig);
			}
			else
			{
				base.player.disableItemSpotLight();
			}
			if (base.channel.IsLocalPlayer)
			{
				if (this.firstAttachments.sightAsset != null)
				{
					this.firstPersonZoomFactor = this.firstAttachments.sightAsset.zoom;
					this.thirdPersonZoomFactor = this.thirdAttachments.sightAsset.thirdPersonZoomFactor;
					this.shouldZoomUsingEyes = this.firstAttachments.sightAsset.shouldZoomUsingEyes;
					if (this.firstAttachments.scopeHook != null)
					{
						base.player.look.enableScope(this.firstPersonZoomFactor, this.firstAttachments.sightAsset);
						Renderer component = this.firstAttachments.scopeHook.GetComponent<Renderer>();
						if (component != null)
						{
							component.enabled = (GraphicsSettings.scopeQuality > EGraphicQuality.OFF);
							component.sharedMaterial = base.player.look.scopeMaterial;
						}
						this.firstAttachments.scopeHook.gameObject.SetActive(true);
						if (base.channel.owner.IsLeftHanded)
						{
							Vector3 localScale = this.firstAttachments.scopeHook.localScale;
							localScale.x *= -1f;
							this.firstAttachments.scopeHook.localScale = localScale;
						}
					}
					else
					{
						base.player.look.disableScope();
					}
				}
				else
				{
					this.firstPersonZoomFactor = 1f;
					this.thirdPersonZoomFactor = 1.25f;
					this.shouldZoomUsingEyes = false;
					base.player.look.disableScope();
				}
				this.UpdateCrosshairEnabled();
			}
			this.UpdateMovementSpeedMultiplier();
			this.UpdateAimInDuration();
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x00198560 File Offset: 0x00196760
		private void applyRecoilMagnitudeModifiers(ref float value)
		{
			if (base.player.stance.stance == EPlayerStance.SPRINT)
			{
				value *= this.equippedGunAsset.recoilSprint;
			}
			else if (base.player.stance.stance == EPlayerStance.CROUCH)
			{
				value *= this.equippedGunAsset.recoilCrouch;
			}
			else if (base.player.stance.stance == EPlayerStance.PRONE)
			{
				value *= this.equippedGunAsset.recoilProne;
			}
			else if (base.player.stance.stance == EPlayerStance.SWIM)
			{
				value *= this.equippedGunAsset.recoilSwimming;
			}
			if (!base.player.movement.isGrounded)
			{
				value *= this.equippedGunAsset.recoilMidair;
			}
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x00198621 File Offset: 0x00196821
		internal float CalculateBulletGravity()
		{
			return Physics.gravity.y * this.equippedGunAsset.bulletGravityMultiplier;
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x0019863C File Offset: 0x0019683C
		internal float CalculateSpreadAngleRadians()
		{
			float quality = (float)base.player.equipment.quality / 100f;
			float interpolatedAimAlpha = this.GetInterpolatedAimAlpha();
			return this.CalculateSpreadAngleRadians(quality, interpolatedAimAlpha);
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x00198670 File Offset: 0x00196870
		internal float CalculateSpreadAngleRadians(float quality, float aimAlpha)
		{
			float num = this.equippedGunAsset.baseSpreadAngleRadians;
			num *= ((quality < 0.5f) ? (1f + (1f - quality * 2f)) : 1f);
			num *= Mathf.Lerp(1f, this.equippedGunAsset.spreadAim, aimAlpha);
			num *= 1f - base.player.skills.mastery(0, 1) * 0.5f;
			if (this.thirdAttachments.sightAsset != null && (!this.thirdAttachments.sightAsset.ShouldOnlyAffectAimWhileProne || base.player.stance.stance == EPlayerStance.PRONE))
			{
				num *= Mathf.Lerp(1f, this.thirdAttachments.sightAsset.spread, aimAlpha);
			}
			if (this.thirdAttachments.tacticalAsset != null && this.shouldEnableTacticalStats && (!this.thirdAttachments.tacticalAsset.ShouldOnlyAffectAimWhileProne || base.player.stance.stance == EPlayerStance.PRONE))
			{
				num *= this.thirdAttachments.tacticalAsset.spread;
			}
			if (this.thirdAttachments.gripAsset != null && (!this.thirdAttachments.gripAsset.ShouldOnlyAffectAimWhileProne || base.player.stance.stance == EPlayerStance.PRONE))
			{
				num *= this.thirdAttachments.gripAsset.spread;
			}
			if (this.thirdAttachments.barrelAsset != null && (!this.thirdAttachments.barrelAsset.ShouldOnlyAffectAimWhileProne || base.player.stance.stance == EPlayerStance.PRONE))
			{
				num *= this.thirdAttachments.barrelAsset.spread;
			}
			if (this.thirdAttachments.magazineAsset != null && (!this.thirdAttachments.magazineAsset.ShouldOnlyAffectAimWhileProne || base.player.stance.stance == EPlayerStance.PRONE))
			{
				num *= this.thirdAttachments.magazineAsset.spread;
			}
			if (base.player.stance.stance == EPlayerStance.SPRINT)
			{
				num *= this.equippedGunAsset.spreadSprint;
			}
			else if (base.player.stance.stance == EPlayerStance.CROUCH)
			{
				num *= this.equippedGunAsset.spreadCrouch;
			}
			else if (base.player.stance.stance == EPlayerStance.PRONE)
			{
				num *= this.equippedGunAsset.spreadProne;
			}
			else if (base.player.stance.stance == EPlayerStance.SWIM)
			{
				num *= this.equippedGunAsset.spreadSwimming;
			}
			if (base.player.look.perspective == EPlayerPerspective.THIRD)
			{
				num *= Provider.modeConfigData.Gameplay.ThirdPerson_SpreadMultiplier;
			}
			if (!base.player.movement.isGrounded)
			{
				num *= this.equippedGunAsset.spreadMidair;
			}
			return num;
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x00198924 File Offset: 0x00196B24
		internal void UpdateCrosshairEnabled()
		{
			if ((!this.equippedGunAsset.isTurret && this.equippedGunAsset.action != EAction.Minigun && ((this.isAiming && base.player.look.perspective == EPlayerPerspective.FIRST && (this.equippedGunAsset.action != EAction.String || this.thirdAttachments.sightHook != null)) || this.isAttaching)) || (base.player.movement.getVehicle() != null && base.player.look.perspective != EPlayerPerspective.FIRST))
			{
				PlayerUI.disableCrosshair();
				return;
			}
			PlayerUI.enableCrosshair();
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x001989C4 File Offset: 0x00196BC4
		private void updateAttach()
		{
			if (this.sightButton != null)
			{
				bool allowZeroCaliber = !this.equippedGunAsset.requiresNonZeroAttachmentCaliber;
				this.sightSearch = base.player.inventory.search(EItemType.SIGHT, this.equippedGunAsset.attachmentCalibers, allowZeroCaliber);
				if (this.sightJars != null)
				{
					this.sightButton.RemoveChild(this.sightJars);
				}
				this.sightJars = new SleekJars(100f, this.sightSearch, 0f);
				this.sightJars.SizeScale_X = 1f;
				this.sightJars.SizeScale_Y = 1f;
				this.sightJars.onClickedJar = new ClickedJar(this.onClickedSightJar);
				this.sightButton.AddChild(this.sightJars);
				if (this.thirdAttachments.sightAsset != null)
				{
					Color rarityColorUI = ItemTool.getRarityColorUI(this.thirdAttachments.sightAsset.rarity);
					this.sightButton.backgroundColor = SleekColor.BackgroundIfLight(rarityColorUI);
					this.sightButton.textColor = rarityColorUI;
					this.sightButton.tooltip = this.thirdAttachments.sightAsset.itemName;
					this.sightButton.iconColor = rarityColorUI;
				}
				else
				{
					this.sightButton.backgroundColor = 1;
					this.sightButton.textColor = 2;
					this.sightButton.tooltip = this.localization.format("Sight_Hook_Tooltip");
					this.sightButton.iconColor = 2;
				}
			}
			if (this.tacticalButton != null)
			{
				bool allowZeroCaliber2 = !this.equippedGunAsset.requiresNonZeroAttachmentCaliber;
				this.tacticalSearch = base.player.inventory.search(EItemType.TACTICAL, this.equippedGunAsset.attachmentCalibers, allowZeroCaliber2);
				if (this.tacticalJars != null)
				{
					this.tacticalButton.RemoveChild(this.tacticalJars);
				}
				this.tacticalJars = new SleekJars(100f, this.tacticalSearch, 0f);
				this.tacticalJars.SizeScale_X = 1f;
				this.tacticalJars.SizeScale_Y = 1f;
				this.tacticalJars.onClickedJar = new ClickedJar(this.onClickedTacticalJar);
				this.tacticalButton.AddChild(this.tacticalJars);
				if (this.thirdAttachments.tacticalAsset != null)
				{
					Color rarityColorUI2 = ItemTool.getRarityColorUI(this.thirdAttachments.tacticalAsset.rarity);
					this.tacticalButton.backgroundColor = SleekColor.BackgroundIfLight(rarityColorUI2);
					this.tacticalButton.textColor = rarityColorUI2;
					this.tacticalButton.tooltip = this.thirdAttachments.tacticalAsset.itemName;
					this.tacticalButton.iconColor = rarityColorUI2;
				}
				else
				{
					this.tacticalButton.backgroundColor = 1;
					this.tacticalButton.textColor = 2;
					this.tacticalButton.tooltip = this.localization.format("Tactical_Hook_Tooltip");
					this.tacticalButton.iconColor = 2;
				}
			}
			if (this.gripButton != null)
			{
				bool allowZeroCaliber3 = !this.equippedGunAsset.requiresNonZeroAttachmentCaliber;
				this.gripSearch = base.player.inventory.search(EItemType.GRIP, this.equippedGunAsset.attachmentCalibers, allowZeroCaliber3);
				if (this.gripJars != null)
				{
					this.gripButton.RemoveChild(this.gripJars);
				}
				this.gripJars = new SleekJars(100f, this.gripSearch, 0f);
				this.gripJars.SizeScale_X = 1f;
				this.gripJars.SizeScale_Y = 1f;
				this.gripJars.onClickedJar = new ClickedJar(this.onClickedGripJar);
				this.gripButton.AddChild(this.gripJars);
				if (this.thirdAttachments.gripAsset != null)
				{
					Color rarityColorUI3 = ItemTool.getRarityColorUI(this.thirdAttachments.gripAsset.rarity);
					this.gripButton.backgroundColor = SleekColor.BackgroundIfLight(rarityColorUI3);
					this.gripButton.textColor = rarityColorUI3;
					this.gripButton.tooltip = this.thirdAttachments.gripAsset.itemName;
					this.gripButton.iconColor = rarityColorUI3;
				}
				else
				{
					this.gripButton.backgroundColor = 1;
					this.gripButton.textColor = 2;
					this.gripButton.tooltip = this.localization.format("Grip_Hook_Tooltip");
					this.gripButton.iconColor = 2;
				}
			}
			if (this.barrelButton != null)
			{
				bool allowZeroCaliber4 = !this.equippedGunAsset.requiresNonZeroAttachmentCaliber;
				this.barrelSearch = base.player.inventory.search(EItemType.BARREL, this.equippedGunAsset.attachmentCalibers, allowZeroCaliber4);
				if (this.barrelJars != null)
				{
					this.barrelButton.RemoveChild(this.barrelJars);
				}
				this.barrelJars = new SleekJars(100f, this.barrelSearch, 0f);
				this.barrelJars.SizeScale_X = 1f;
				this.barrelJars.SizeScale_Y = 1f;
				this.barrelJars.onClickedJar = new ClickedJar(this.onClickedBarrelJar);
				this.barrelButton.AddChild(this.barrelJars);
				if (this.thirdAttachments.barrelAsset != null)
				{
					Color rarityColorUI4 = ItemTool.getRarityColorUI(this.thirdAttachments.barrelAsset.rarity);
					this.barrelButton.backgroundColor = SleekColor.BackgroundIfLight(rarityColorUI4);
					this.barrelButton.textColor = rarityColorUI4;
					this.barrelButton.tooltip = this.thirdAttachments.barrelAsset.itemName;
					this.barrelButton.iconColor = rarityColorUI4;
				}
				else
				{
					this.barrelButton.backgroundColor = 1;
					this.barrelButton.textColor = 2;
					this.barrelButton.tooltip = this.localization.format("Barrel_Hook_Tooltip");
					this.barrelButton.iconColor = 2;
				}
				if (this.thirdAttachments.barrelAsset != null && this.thirdAttachments.barrelAsset.showQuality)
				{
					this.barrelQualityImage.TintColor = ItemTool.getQualityColor((float)base.player.equipment.state[16] / 100f);
					this.barrelQualityLabel.Text = base.player.equipment.state[16].ToString() + "%";
					this.barrelQualityLabel.TextColor = this.barrelQualityImage.TintColor;
					this.barrelQualityLabel.IsVisible = true;
					this.barrelQualityImage.IsVisible = true;
				}
				else
				{
					this.barrelQualityLabel.IsVisible = false;
					this.barrelQualityImage.IsVisible = false;
				}
			}
			if (this.magazineButton != null)
			{
				bool allowZeroCaliber5 = !this.equippedGunAsset.requiresNonZeroAttachmentCaliber;
				this.magazineSearch = base.player.inventory.search(EItemType.MAGAZINE, this.equippedGunAsset.magazineCalibers, allowZeroCaliber5);
				if (this.magazineJars != null)
				{
					this.magazineButton.RemoveChild(this.magazineJars);
				}
				this.magazineJars = new SleekJars(100f, this.magazineSearch, 0f);
				this.magazineJars.SizeScale_X = 1f;
				this.magazineJars.SizeScale_Y = 1f;
				this.magazineJars.onClickedJar = new ClickedJar(this.onClickedMagazineJar);
				this.magazineButton.AddChild(this.magazineJars);
				if (this.thirdAttachments.magazineAsset != null)
				{
					Color rarityColorUI5 = ItemTool.getRarityColorUI(this.thirdAttachments.magazineAsset.rarity);
					this.magazineButton.backgroundColor = SleekColor.BackgroundIfLight(rarityColorUI5);
					this.magazineButton.textColor = rarityColorUI5;
					this.magazineButton.tooltip = this.thirdAttachments.magazineAsset.itemName;
					this.magazineButton.iconColor = rarityColorUI5;
				}
				else
				{
					this.magazineButton.backgroundColor = 1;
					this.magazineButton.textColor = 2;
					this.magazineButton.tooltip = this.localization.format("Magazine_Hook_Tooltip");
					this.magazineButton.iconColor = 2;
				}
				if (this.thirdAttachments.magazineAsset != null && this.thirdAttachments.magazineAsset.showQuality)
				{
					this.magazineQualityImage.TintColor = ItemTool.getQualityColor((float)base.player.equipment.state[17] / 100f);
					this.magazineQualityLabel.Text = base.player.equipment.state[17].ToString() + "%";
					this.magazineQualityLabel.TextColor = this.magazineQualityImage.TintColor;
					this.magazineQualityLabel.IsVisible = true;
					this.magazineQualityImage.IsVisible = true;
					return;
				}
				this.magazineQualityLabel.IsVisible = false;
				this.magazineQualityImage.IsVisible = false;
			}
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x001992E4 File Offset: 0x001974E4
		private void updateInfo()
		{
			this.ammoLabel.TextColor = ((this.ammo < this.equippedGunAsset.ammoPerShot) ? 6 : 3);
			this.ammoLabel.Text = this.localization.format("Ammo", this.ammo, (int)((this.thirdAttachments.magazineAsset != null) ? this.thirdAttachments.magazineAsset.amount : 0));
			if (this.firstAmmoCounter != null)
			{
				this.firstAmmoCounter.text = this.ammo.ToString();
			}
			if (this.thirdAmmoCounter != null)
			{
				this.thirdAmmoCounter.text = this.ammo.ToString();
			}
			if (this.firemode == EFiremode.SAFETY)
			{
				this.firemodeLabel.Text = this.localization.format("Firemode", this.localization.format("Safety"), ControlsSettings.firemode);
			}
			else if (this.firemode == EFiremode.SEMI)
			{
				this.firemodeLabel.Text = this.localization.format("Firemode", this.localization.format("Semi"), ControlsSettings.firemode);
			}
			else if (this.firemode == EFiremode.AUTO)
			{
				this.firemodeLabel.Text = this.localization.format("Firemode", this.localization.format("Auto"), ControlsSettings.firemode);
			}
			else if (this.firemode == EFiremode.BURST)
			{
				this.firemodeLabel.Text = this.localization.format("Firemode", this.localization.format("Burst"), ControlsSettings.firemode);
			}
			this.attachLabel.Text = this.localization.format("Attach", (this.thirdAttachments.magazineAsset != null) ? this.thirdAttachments.magazineAsset.itemName : this.localization.format("None"), ControlsSettings.attach);
			if (this.thirdAttachments.magazineAsset != null)
			{
				this.attachLabel.TextColor = ItemTool.getRarityColorUI(this.thirdAttachments.magazineAsset.rarity);
				return;
			}
			this.attachLabel.TextColor = 3;
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x00199544 File Offset: 0x00197744
		private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
		{
			this.UpdateCrosshairEnabled();
			if (newPerspective == EPlayerPerspective.THIRD)
			{
				if (this.isAiming)
				{
					PlayerUI.updateScope(false);
					base.player.look.enableZoom(this.thirdPersonZoomFactor);
					base.player.look.disableOverlay();
				}
				else
				{
					base.player.look.disableZoom();
				}
			}
			else if (this.isAiming)
			{
				if (this.shouldZoomUsingEyes)
				{
					base.player.look.enableZoom(this.firstPersonZoomFactor);
				}
				else if (GraphicsSettings.scopeQuality == EGraphicQuality.OFF && PlayerLifeUI.scopeOverlay.scopeImage.Texture != null)
				{
					PlayerUI.updateScope(true);
					base.player.look.enableZoom(this.firstPersonZoomFactor);
					base.player.look.enableOverlay();
				}
				else
				{
					base.player.look.disableZoom();
				}
			}
			else
			{
				base.player.look.disableZoom();
			}
			if (this.thirdShellRenderer != null)
			{
				this.thirdShellRenderer.forceRenderingOff = (newPerspective == EPlayerPerspective.FIRST);
			}
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x0019965C File Offset: 0x0019785C
		private void SyncScopeDistanceMarkerText()
		{
			foreach (UseableGun.DistanceMarker distanceMarker in this.scopeDistanceMarkers)
			{
				if (!(distanceMarker.textComponent == null))
				{
					if (OptionsSettings.metric)
					{
						distanceMarker.textComponent.text = string.Format("{0} m", distanceMarker.distance);
					}
					else
					{
						distanceMarker.textComponent.text = string.Format("{0} yd", Mathf.RoundToInt(MeasurementTool.MtoYd(distanceMarker.distance)));
					}
				}
			}
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x0019970C File Offset: 0x0019790C
		private void onClickedSightHookButton(ISleekElement button)
		{
			UseableGun.SendAttachSight.Invoke(base.GetNetId(), ENetReliability.Unreliable, byte.MaxValue, byte.MaxValue, byte.MaxValue, new byte[0]);
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x00199734 File Offset: 0x00197934
		private void onClickedTacticalHookButton(ISleekElement button)
		{
			UseableGun.SendAttachTactical.Invoke(base.GetNetId(), ENetReliability.Unreliable, byte.MaxValue, byte.MaxValue, byte.MaxValue, new byte[0]);
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x0019975C File Offset: 0x0019795C
		private void onClickedGripHookButton(ISleekElement button)
		{
			UseableGun.SendAttachGrip.Invoke(base.GetNetId(), ENetReliability.Unreliable, byte.MaxValue, byte.MaxValue, byte.MaxValue, new byte[0]);
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x00199784 File Offset: 0x00197984
		private void onClickedBarrelHookButton(ISleekElement button)
		{
			UseableGun.SendAttachBarrel.Invoke(base.GetNetId(), ENetReliability.Unreliable, byte.MaxValue, byte.MaxValue, byte.MaxValue, new byte[0]);
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x001997AC File Offset: 0x001979AC
		private void onClickedMagazineHookButton(ISleekElement button)
		{
			UseableGun.SendAttachMagazine.Invoke(base.GetNetId(), ENetReliability.Unreliable, byte.MaxValue, byte.MaxValue, byte.MaxValue, new byte[0]);
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x001997D4 File Offset: 0x001979D4
		private void onClickedSightJar(SleekJars jars, int index)
		{
			ItemAsset asset = this.sightSearch[index].GetAsset();
			if (asset == null)
			{
				return;
			}
			UseableGun.SendAttachSight.Invoke(base.GetNetId(), ENetReliability.Unreliable, this.sightSearch[index].page, this.sightSearch[index].jar.x, this.sightSearch[index].jar.y, asset.hash);
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x0019984C File Offset: 0x00197A4C
		private void onClickedTacticalJar(SleekJars jars, int index)
		{
			ItemAsset asset = this.tacticalSearch[index].GetAsset();
			if (asset == null)
			{
				return;
			}
			UseableGun.SendAttachTactical.Invoke(base.GetNetId(), ENetReliability.Unreliable, this.tacticalSearch[index].page, this.tacticalSearch[index].jar.x, this.tacticalSearch[index].jar.y, asset.hash);
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x001998C4 File Offset: 0x00197AC4
		private void onClickedGripJar(SleekJars jars, int index)
		{
			ItemAsset asset = this.gripSearch[index].GetAsset();
			if (asset == null)
			{
				return;
			}
			UseableGun.SendAttachGrip.Invoke(base.GetNetId(), ENetReliability.Unreliable, this.gripSearch[index].page, this.gripSearch[index].jar.x, this.gripSearch[index].jar.y, asset.hash);
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x0019993C File Offset: 0x00197B3C
		private void onClickedBarrelJar(SleekJars jars, int index)
		{
			ItemAsset asset = this.barrelSearch[index].GetAsset();
			if (asset == null)
			{
				return;
			}
			UseableGun.SendAttachBarrel.Invoke(base.GetNetId(), ENetReliability.Unreliable, this.barrelSearch[index].page, this.barrelSearch[index].jar.x, this.barrelSearch[index].jar.y, asset.hash);
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x001999B4 File Offset: 0x00197BB4
		private void onClickedMagazineJar(SleekJars jars, int index)
		{
			ItemAsset asset = this.magazineSearch[index].GetAsset();
			if (asset == null)
			{
				return;
			}
			UseableGun.SendAttachMagazine.Invoke(base.GetNetId(), ENetReliability.Unreliable, this.magazineSearch[index].page, this.magazineSearch[index].jar.x, this.magazineSearch[index].jar.y, asset.hash);
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x00199A2C File Offset: 0x00197C2C
		private void startAim()
		{
			this.UpdateMovementSpeedMultiplier();
			if (base.channel.IsLocalPlayer)
			{
				base.player.animator.viewmodelSwayMultiplier = 0.1f;
				base.player.animator.viewmodelOffsetPreferenceMultiplier = 0f;
				if (!this.equippedGunAsset.isTurret && this.equippedGunAsset.action != EAction.Minigun)
				{
					if (GraphicsSettings.scopeQuality == EGraphicQuality.OFF && this.firstAttachments.sightModel != null && this.firstAttachments.scopeHook != null && this.firstAttachments.scopeHook.Find("Reticule") != null)
					{
						Texture mainTexture = this.firstAttachments.scopeHook.Find("Reticule").GetComponent<Renderer>().sharedMaterial.mainTexture;
						if (mainTexture.width <= 64)
						{
							PlayerLifeUI.scopeOverlay.scopeImage.PositionOffset_X = (float)(-(float)mainTexture.width / 2);
							PlayerLifeUI.scopeOverlay.scopeImage.PositionOffset_Y = (float)(-(float)mainTexture.height / 2);
							PlayerLifeUI.scopeOverlay.scopeImage.PositionScale_X = 0.5f;
							PlayerLifeUI.scopeOverlay.scopeImage.PositionScale_Y = 0.5f;
							PlayerLifeUI.scopeOverlay.scopeImage.SizeOffset_X = (float)mainTexture.width;
							PlayerLifeUI.scopeOverlay.scopeImage.SizeOffset_Y = (float)mainTexture.height;
							PlayerLifeUI.scopeOverlay.scopeImage.SizeScale_X = 0f;
							PlayerLifeUI.scopeOverlay.scopeImage.SizeScale_Y = 0f;
						}
						else
						{
							PlayerLifeUI.scopeOverlay.scopeImage.PositionOffset_X = 0f;
							PlayerLifeUI.scopeOverlay.scopeImage.PositionOffset_Y = 0f;
							PlayerLifeUI.scopeOverlay.scopeImage.PositionScale_X = 0f;
							PlayerLifeUI.scopeOverlay.scopeImage.PositionScale_Y = 0f;
							PlayerLifeUI.scopeOverlay.scopeImage.SizeOffset_X = 0f;
							PlayerLifeUI.scopeOverlay.scopeImage.SizeOffset_Y = 0f;
							if (this.firstAttachments.sightAsset.shouldOffsetScopeOverlayByOneTexel)
							{
								PlayerLifeUI.scopeOverlay.scopeImage.SizeScale_X = 1f + 1f / (float)mainTexture.width;
								PlayerLifeUI.scopeOverlay.scopeImage.SizeScale_Y = 1f + 1f / (float)mainTexture.height;
							}
							else
							{
								PlayerLifeUI.scopeOverlay.scopeImage.SizeScale_X = 1f;
								PlayerLifeUI.scopeOverlay.scopeImage.SizeScale_Y = 1f;
							}
						}
						PlayerLifeUI.scopeOverlay.scopeImage.Texture = mainTexture;
						if (this.firstAttachments.aimHook.parent.Find("Reticule") != null)
						{
							Color criticalHitmarkerColor = OptionsSettings.criticalHitmarkerColor;
							criticalHitmarkerColor.a = 1f;
							PlayerLifeUI.scopeOverlay.scopeImage.TintColor = criticalHitmarkerColor;
						}
						else
						{
							PlayerLifeUI.scopeOverlay.scopeImage.TintColor = 0;
						}
						base.player.animator.viewmodelCameraLocalPositionOffset = Vector3.up;
					}
					else
					{
						PlayerLifeUI.scopeOverlay.scopeImage.Texture = null;
						base.player.animator.viewmodelCameraLocalPositionOffset = Vector3.zero;
					}
				}
				else
				{
					PlayerLifeUI.scopeOverlay.scopeImage.Texture = null;
				}
				if (this.equippedGunAsset.isTurret)
				{
					base.player.animator.turretViewmodelCameraLocalPositionOffset = Vector3.up;
				}
				base.player.look.shouldUseZoomFactorForSensitivity = true;
				if (this.equippedGunAsset.isTurret || this.equippedGunAsset.action == EAction.Minigun || this.shouldZoomUsingEyes)
				{
					if (base.player.look.perspective == EPlayerPerspective.FIRST)
					{
						base.player.look.enableZoom(this.firstPersonZoomFactor);
					}
					else if (base.player.look.perspective == EPlayerPerspective.THIRD)
					{
						base.player.look.enableZoom(this.thirdPersonZoomFactor);
					}
				}
				else if (base.player.look.perspective == EPlayerPerspective.FIRST)
				{
					if (GraphicsSettings.scopeQuality == EGraphicQuality.OFF && PlayerLifeUI.scopeOverlay.scopeImage.Texture != null)
					{
						PlayerUI.updateScope(true);
						base.player.look.enableZoom(this.firstPersonZoomFactor);
						base.player.look.enableOverlay();
					}
				}
				else if (base.player.look.perspective == EPlayerPerspective.THIRD)
				{
					base.player.look.enableZoom(this.thirdPersonZoomFactor);
				}
				this.UpdateCrosshairEnabled();
				PlayerUI.instance.groupUI.IsVisible = false;
			}
			base.player.playSound(this.equippedGunAsset.aim, 1f, 0f);
			this.isMinigunSpinning = true;
			base.player.animator.play("Aim_Start", false);
			UseableGun.OnAimingChanged_Global.TryInvoke("OnAimingChanged_Global", this);
			VehicleTurretEventHook vehicleTurretEventHook = this.GetVehicleTurretEventHook();
			if (vehicleTurretEventHook != null)
			{
				UnityEvent onAimingStarted = vehicleTurretEventHook.OnAimingStarted;
				if (onAimingStarted != null)
				{
					onAimingStarted.TryInvoke(this);
				}
			}
			if (base.channel.IsLocalPlayer)
			{
				VehicleTurretEventHook vehicleTurretEventHook2 = this.GetVehicleTurretEventHook();
				if (vehicleTurretEventHook2 != null)
				{
					UnityEvent onAimingStarted_Local = vehicleTurretEventHook2.OnAimingStarted_Local;
					if (onAimingStarted_Local != null)
					{
						onAimingStarted_Local.TryInvoke(this);
					}
				}
			}
			foreach (UseableGunEventHook useableGunEventHook in this.EnumerateEventComponents())
			{
				UnityEvent onAimingStarted2 = useableGunEventHook.OnAimingStarted;
				if (onAimingStarted2 != null)
				{
					onAimingStarted2.TryInvoke(this);
				}
			}
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x00199FBC File Offset: 0x001981BC
		private void stopAim()
		{
			this.UpdateMovementSpeedMultiplier();
			if (base.channel.IsLocalPlayer)
			{
				if (!this.equippedGunAsset.isTurret)
				{
					base.player.animator.viewmodelCameraLocalPositionOffset = Vector3.zero;
				}
				base.player.animator.turretViewmodelCameraLocalPositionOffset = Vector3.zero;
				base.player.animator.scopeSway = Vector3.zero;
				base.player.animator.viewmodelSwayMultiplier = 1f;
				base.player.animator.viewmodelOffsetPreferenceMultiplier = 1f;
				PlayerUI.updateScope(false);
				base.player.look.shouldUseZoomFactorForSensitivity = false;
				base.player.look.disableZoom();
				base.player.look.disableOverlay();
				this.UpdateCrosshairEnabled();
				PlayerUI.instance.groupUI.IsVisible = true;
			}
			this.isMinigunSpinning = false;
			base.player.animator.play("Aim_Stop", false);
			UseableGun.OnAimingChanged_Global.TryInvoke("OnAimingChanged_Global", this);
			VehicleTurretEventHook vehicleTurretEventHook = this.GetVehicleTurretEventHook();
			if (vehicleTurretEventHook != null)
			{
				UnityEvent onAimingStopped = vehicleTurretEventHook.OnAimingStopped;
				if (onAimingStopped != null)
				{
					onAimingStopped.TryInvoke(this);
				}
			}
			if (base.channel.IsLocalPlayer)
			{
				VehicleTurretEventHook vehicleTurretEventHook2 = this.GetVehicleTurretEventHook();
				if (vehicleTurretEventHook2 != null)
				{
					UnityEvent onAimingStopped_Local = vehicleTurretEventHook2.OnAimingStopped_Local;
					if (onAimingStopped_Local != null)
					{
						onAimingStopped_Local.TryInvoke(this);
					}
				}
			}
			foreach (UseableGunEventHook useableGunEventHook in this.EnumerateEventComponents())
			{
				UnityEvent onAimingStopped2 = useableGunEventHook.OnAimingStopped;
				if (onAimingStopped2 != null)
				{
					onAimingStopped2.TryInvoke(this);
				}
			}
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x0019A160 File Offset: 0x00198360
		private void startAttach()
		{
			PlayerUI.isLocked = true;
			PlayerLifeUI.close();
			if (this.sightButton != null)
			{
				this.sightButton.IsVisible = true;
			}
			if (this.tacticalButton != null)
			{
				this.tacticalButton.IsVisible = true;
			}
			if (this.gripButton != null)
			{
				this.gripButton.IsVisible = true;
			}
			if (this.barrelButton != null)
			{
				this.barrelButton.IsVisible = true;
			}
			if (this.magazineButton != null)
			{
				this.magazineButton.IsVisible = true;
			}
			this.UpdateCrosshairEnabled();
			if (base.channel.IsLocalPlayer)
			{
				VehicleTurretEventHook vehicleTurretEventHook = this.GetVehicleTurretEventHook();
				if (vehicleTurretEventHook == null)
				{
					return;
				}
				UnityEvent onInspectingAttachmentsStarted_Local = vehicleTurretEventHook.OnInspectingAttachmentsStarted_Local;
				if (onInspectingAttachmentsStarted_Local == null)
				{
					return;
				}
				onInspectingAttachmentsStarted_Local.TryInvoke(this);
			}
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x0019A20C File Offset: 0x0019840C
		private void stopAttach()
		{
			PlayerUI.isLocked = false;
			PlayerLifeUI.open();
			if (this.sightButton != null)
			{
				this.sightButton.IsVisible = false;
			}
			if (this.tacticalButton != null)
			{
				this.tacticalButton.IsVisible = false;
			}
			if (this.gripButton != null)
			{
				this.gripButton.IsVisible = false;
			}
			if (this.barrelButton != null)
			{
				this.barrelButton.IsVisible = false;
			}
			if (this.magazineButton != null)
			{
				this.magazineButton.IsVisible = false;
			}
			this.UpdateCrosshairEnabled();
			if (base.channel.IsLocalPlayer)
			{
				VehicleTurretEventHook vehicleTurretEventHook = this.GetVehicleTurretEventHook();
				if (vehicleTurretEventHook == null)
				{
					return;
				}
				UnityEvent onInspectingAttachmentsStopped_Local = vehicleTurretEventHook.OnInspectingAttachmentsStopped_Local;
				if (onInspectingAttachmentsStopped_Local == null)
				{
					return;
				}
				onInspectingAttachmentsStopped_Local.TryInvoke(this);
			}
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x0019A2B8 File Offset: 0x001984B8
		private void Update()
		{
			if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null)
			{
				Transform turretAim = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turretAim;
				if (turretAim != null)
				{
					Transform transform = turretAim.Find("Barrel");
					Transform transform2 = turretAim.Find("Eject");
					if (this.thirdMuzzleEmitter != null && transform != null)
					{
						this.thirdMuzzleEmitter.transform.position = transform.position;
						this.thirdMuzzleEmitter.transform.rotation = transform.rotation;
					}
					if (this.thirdShellEmitter != null && transform2 != null)
					{
						this.thirdShellEmitter.transform.position = transform2.position;
						this.thirdShellEmitter.transform.rotation = transform2.rotation;
					}
				}
			}
			else if (this.thirdShellEmitter != null)
			{
				this.thirdShellEmitter.transform.SetPositionAndRotation(this.thirdAttachments.ejectHook.position, this.thirdAttachments.ejectHook.rotation);
			}
			if (base.channel.IsLocalPlayer)
			{
				if (this.firstFakeLight != null && this.thirdMuzzleEmitter != null)
				{
					this.firstFakeLight.position = this.thirdMuzzleEmitter.transform.position;
				}
				if (this.firstFakeLight_0 != null && this.thirdAttachments.lightHook != null)
				{
					this.firstFakeLight_0.position = this.thirdAttachments.lightHook.position;
					if (this.firstFakeLight_0.gameObject.activeSelf != (base.player.look.perspective == EPlayerPerspective.FIRST && this.thirdAttachments.lightHook.gameObject.activeSelf))
					{
						this.firstFakeLight_0.gameObject.SetActive(base.player.look.perspective == EPlayerPerspective.FIRST && this.thirdAttachments.lightHook.gameObject.activeSelf);
					}
				}
				if (this.firstFakeLight_1 != null && this.thirdAttachments.light2Hook != null)
				{
					this.firstFakeLight_1.position = this.thirdAttachments.light2Hook.position;
					if (this.firstFakeLight_1.gameObject.activeSelf != (base.player.look.perspective == EPlayerPerspective.FIRST && this.thirdAttachments.light2Hook.gameObject.activeSelf))
					{
						this.firstFakeLight_1.gameObject.SetActive(base.player.look.perspective == EPlayerPerspective.FIRST && this.thirdAttachments.light2Hook.gameObject.activeSelf);
					}
				}
				this.swayTime += Time.deltaTime * (1f - this.steadyAccuracy / 4f);
				if (this.isAiming && this.firstAttachments.sightAsset != null)
				{
					float num = (1f - 1f / this.firstAttachments.sightAsset.zoom) * 1.25f;
					num *= 1f - base.player.skills.mastery(0, 5) * 0.5f;
					if (this.thirdAttachments != null && this.thirdAttachments.tacticalAsset != null && this.shouldEnableTacticalStats && (!this.thirdAttachments.tacticalAsset.ShouldOnlyAffectAimWhileProne || base.player.stance.stance == EPlayerStance.PRONE))
					{
						num *= this.thirdAttachments.tacticalAsset.sway;
					}
					if (this.thirdAttachments != null && this.thirdAttachments.gripAsset != null && (!this.thirdAttachments.gripAsset.ShouldOnlyAffectAimWhileProne || base.player.stance.stance == EPlayerStance.PRONE))
					{
						num *= this.thirdAttachments.gripAsset.sway;
					}
					if (this.thirdAttachments != null && this.thirdAttachments.barrelAsset != null && (!this.thirdAttachments.barrelAsset.ShouldOnlyAffectAimWhileProne || base.player.stance.stance == EPlayerStance.PRONE))
					{
						num *= this.thirdAttachments.barrelAsset.sway;
					}
					if (this.thirdAttachments != null && this.thirdAttachments.magazineAsset != null && (!this.thirdAttachments.magazineAsset.ShouldOnlyAffectAimWhileProne || base.player.stance.stance == EPlayerStance.PRONE))
					{
						num *= this.thirdAttachments.magazineAsset.sway;
					}
					if (base.player.stance.stance == EPlayerStance.CROUCH)
					{
						num *= UseableGun.SWAY_CROUCH;
					}
					else if (base.player.stance.stance == EPlayerStance.PRONE)
					{
						num *= UseableGun.SWAY_PRONE;
					}
					base.player.animator.scopeSway = Vector3.Lerp(base.player.animator.scopeSway, new Vector3(Mathf.Sin(0.75f * this.swayTime) * num, Mathf.Sin(1f * this.swayTime) * num, 0f), Time.deltaTime * 4f);
				}
				else
				{
					base.player.animator.scopeSway = Vector3.Lerp(base.player.animator.scopeSway, Vector3.zero, Time.deltaTime * 4f);
				}
				if (this.firstAttachments.reticuleHook != null && this.firstAttachments.sightAsset != null && this.firstAttachments.sightAsset.isHolographic)
				{
					this.UpdateHolographicReticulePosition();
				}
				if (this.scopeDistanceMarkers != null && this.scopeDistanceMarkers.Count > 0)
				{
					this.UpdateScopeDistanceMarkers();
				}
			}
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x0019A8EC File Offset: 0x00198AEC
		internal void GetAimingViewmodelAlignment(out Transform alignmentTransform, out Vector3 alignmentOffset, out float alpha)
		{
			alignmentTransform = null;
			alignmentOffset = Vector3.zero;
			alpha = this.GetInterpolatedAimAlpha();
			if (this.equippedGunAsset.isTurret || this.equippedGunAsset.action == EAction.Minigun)
			{
				return;
			}
			if (this.firstAttachments != null)
			{
				if (this.firstAttachments.aimHook != null)
				{
					alignmentTransform = this.firstAttachments.aimHook;
					return;
				}
				if (this.firstAttachments.viewHook != null)
				{
					alignmentTransform = this.firstAttachments.viewHook;
					return;
				}
				alignmentTransform = this.firstAttachments.sightHook;
				if (this.equippedGunAsset.hasSight)
				{
					alignmentOffset = new Vector3(0f, -0.04f, 0.01f);
				}
			}
		}

		/// <summary>
		/// This is a bit of a hack... aimAccuracy is [0, maxAimingAccuracy] and changed during each FixedUpdate call,
		/// but was used in some gameplay display features like holo sight, laser, ADS, etc. (yes, should
		/// be de-coupled from FixedUpdate but that is its own issue) To smooth this out we interpolate
		/// slightly behind the aimAccuracy value depending on the time since FixedUpdate.
		/// </summary>
		// Token: 0x0600454A RID: 17738 RVA: 0x0019A9B0 File Offset: 0x00198BB0
		private float GetInterpolatedAimAlpha()
		{
			float num = (float)((Time.timeAsDouble - Time.fixedTimeAsDouble) / (double)Time.fixedDeltaTime);
			if (this.isAiming)
			{
				if (this.aimAccuracy < this.maxAimingAccuracy)
				{
					return MathfEx.SmootherStep01((float)this.aimAccuracy * this.maxAimingAccuracyReciprocal + num * this.maxAimingAccuracyReciprocal);
				}
				return 1f;
			}
			else
			{
				if (this.aimAccuracy > 0)
				{
					return MathfEx.SmootherStep01((float)this.aimAccuracy * this.maxAimingAccuracyReciprocal - num * this.maxAimingAccuracyReciprocal);
				}
				return 0f;
			}
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x0019AA35 File Offset: 0x00198C35
		private float GetSimulationAimAlpha()
		{
			return (float)this.aimAccuracy * this.maxAimingAccuracyReciprocal;
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x0019AA48 File Offset: 0x00198C48
		private void UpdateInfoBoxVisibility()
		{
			bool flag = base.player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowUseableGunStatus);
			if (Level.info != null && Level.info.configData != null)
			{
				flag &= Level.info.configData.PlayerUI_GunVisible;
			}
			this.infoBox.IsVisible = flag;
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x0019AA98 File Offset: 0x00198C98
		private void OnLocalPluginWidgetFlagsChanged(Player player, EPluginWidgetFlags oldFlags)
		{
			EPluginWidgetFlags pluginWidgetFlags = player.pluginWidgetFlags;
			if ((oldFlags & EPluginWidgetFlags.ShowUseableGunStatus) != (pluginWidgetFlags & EPluginWidgetFlags.ShowUseableGunStatus))
			{
				this.UpdateInfoBoxVisibility();
			}
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x0019AAC2 File Offset: 0x00198CC2
		private IEnumerable<UseableGunEventHook> EnumerateEventComponents()
		{
			if (this.firstEventComponent)
			{
				yield return this.firstEventComponent;
			}
			if (this.thirdEventComponent)
			{
				yield return this.thirdEventComponent;
			}
			if (this.characterEventComponent)
			{
				yield return this.characterEventComponent;
			}
			yield break;
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x0019AAD4 File Offset: 0x00198CD4
		private void InvokeModHookShotFiredEvents()
		{
			VehicleTurretEventHook vehicleTurretEventHook = this.GetVehicleTurretEventHook();
			if (vehicleTurretEventHook != null)
			{
				UnityEvent onShotFired = vehicleTurretEventHook.OnShotFired;
				if (onShotFired != null)
				{
					onShotFired.TryInvoke(this);
				}
			}
			foreach (UseableGunEventHook useableGunEventHook in this.EnumerateEventComponents())
			{
				UnityEvent onShotFired2 = useableGunEventHook.OnShotFired;
				if (onShotFired2 != null)
				{
					onShotFired2.TryInvoke(this);
				}
			}
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x0019AB48 File Offset: 0x00198D48
		private void ClearScopeDistanceMarkers()
		{
			if (this.scopeDistanceMarkers != null)
			{
				this.scopeDistanceMarkers.Clear();
			}
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x0019AB60 File Offset: 0x00198D60
		private void InstantiateScopeDistanceMarkers()
		{
			if (this.scopeDistanceMarkers == null)
			{
				this.scopeDistanceMarkers = new List<UseableGun.DistanceMarker>();
			}
			if (this.firstAttachments.scopeHook == null)
			{
				return;
			}
			Transform transform = this.firstAttachments.scopeHook.Find("Reticule");
			if (transform == null)
			{
				return;
			}
			if (UseableGun.scopeDistanceMarkerMaterial == null)
			{
				UseableGun.scopeDistanceMarkerMaterial = new Material(Shader.Find("Sprites/Default"));
				UseableGun.scopeDistanceMarkerMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			foreach (ItemSightAsset.DistanceMarker distanceMarker in this.firstAttachments.sightAsset.distanceMarkers)
			{
				UseableGun.DistanceMarker distanceMarker2 = new UseableGun.DistanceMarker();
				distanceMarker2.isActive = true;
				distanceMarker2.distance = distanceMarker.distance;
				distanceMarker2.transform = new GameObject(string.Format("DistanceMarker_{0}m", distanceMarker.distance))
				{
					layer = 11
				}.transform;
				distanceMarker2.transform.SetParent(transform, false);
				distanceMarker2.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
				GameObject gameObject = new GameObject("Line");
				gameObject.layer = 11;
				gameObject.transform.SetParent(distanceMarker2.transform, false);
				distanceMarker2.lineComponent = gameObject.AddComponent<LineRenderer>();
				distanceMarker2.lineComponent.alignment = LineAlignment.Local;
				distanceMarker2.lineComponent.endColor = distanceMarker.color;
				distanceMarker2.lineComponent.startColor = distanceMarker.color;
				distanceMarker2.lineComponent.useWorldSpace = false;
				distanceMarker2.lineComponent.shadowCastingMode = ShadowCastingMode.Off;
				distanceMarker2.lineComponent.widthMultiplier = 0.005f;
				distanceMarker2.lineComponent.sharedMaterial = UseableGun.scopeDistanceMarkerMaterial;
				if (distanceMarker.side == ItemSightAsset.DistanceMarker.ESide.Right)
				{
					distanceMarker2.lineComponent.SetPositions(new Vector3[]
					{
						new Vector3(distanceMarker.lineOffset * 2f, 0f, 0f),
						new Vector3((distanceMarker.lineOffset + distanceMarker.lineWidth) * 2f, 0f, 0f)
					});
				}
				else
				{
					distanceMarker2.lineComponent.SetPositions(new Vector3[]
					{
						new Vector3(distanceMarker.lineOffset * -2f, 0f, 0f),
						new Vector3((distanceMarker.lineOffset + distanceMarker.lineWidth) * -2f, 0f, 0f)
					});
				}
				if (distanceMarker.hasLabel)
				{
					GameObject gameObject2 = new GameObject("Text");
					gameObject2.layer = 11;
					gameObject2.transform.SetParent(distanceMarker2.transform, false);
					distanceMarker2.textComponent = gameObject2.AddComponent<TextMeshPro>();
					distanceMarker2.textComponent.color = distanceMarker.color;
					distanceMarker2.textComponent.fontSize = 0.35f;
					distanceMarker2.textComponent.fontStyle = 1;
					RectTransform rectTransform = gameObject2.GetRectTransform();
					if (distanceMarker.side == ItemSightAsset.DistanceMarker.ESide.Right)
					{
						rectTransform.localPosition = new Vector3((distanceMarker.lineOffset + distanceMarker.lineWidth) * 2f + 0.01f, 0f, 0f);
						distanceMarker2.textComponent.alignment = 4097;
						rectTransform.pivot = new Vector3(0f, 0.5f);
					}
					else
					{
						rectTransform.localPosition = new Vector3((distanceMarker.lineOffset + distanceMarker.lineWidth) * -2f - 0.01f, 0f, 0f);
						distanceMarker2.textComponent.alignment = 4100;
						rectTransform.pivot = new Vector3(1f, 0.5f);
					}
				}
				this.scopeDistanceMarkers.Add(distanceMarker2);
			}
			this.SyncScopeDistanceMarkerText();
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x0019AF68 File Offset: 0x00199168
		private void UpdateScopeDistanceMarkers()
		{
			float fieldOfView = base.player.look.scopeCamera.fieldOfView;
			float num = 0.017453292f * fieldOfView;
			float muzzleVelocity = this.equippedGunAsset.muzzleVelocity;
			float gravity = this.CalculateBulletGravity();
			foreach (UseableGun.DistanceMarker distanceMarker in this.scopeDistanceMarkers)
			{
				float num2 = Mathf.Abs(SleekScopeOverlay.CalcAngle(muzzleVelocity, distanceMarker.distance, gravity)) / num * -2f;
				distanceMarker.transform.localPosition = new Vector3(0f, num2, 0f);
				bool flag = num2 < -0.01f && num2 > -0.9f;
				if (distanceMarker.isActive != flag)
				{
					distanceMarker.isActive = flag;
					distanceMarker.transform.gameObject.SetActive(flag);
				}
			}
		}

		/// <summary>
		/// Holographic sights follow the true aiming direction regardless of viewmodel animation.
		/// </summary>
		// Token: 0x06004553 RID: 17747 RVA: 0x0019B068 File Offset: 0x00199268
		private void UpdateHolographicReticulePosition()
		{
			this.firstAttachments.reticuleHook.localPosition = this.originalReticuleHookLocalPosition;
			Plane plane = new Plane(this.firstAttachments.reticuleHook.forward, this.firstAttachments.reticuleHook.position);
			Quaternion rhs = Quaternion.Euler(base.player.animator.recoilViewmodelCameraRotation.currentPosition);
			Vector3 vector = base.player.animator.viewmodelCameraTransform.rotation * rhs * Vector3.forward;
			Vector3 position = base.player.animator.viewmodelCameraTransform.position;
			float d;
			if (plane.Raycast(new Ray(position, vector), out d))
			{
				Vector3 position2 = position + vector * d;
				Vector3 b = this.firstAttachments.reticuleHook.parent.InverseTransformPoint(position2);
				this.firstAttachments.reticuleHook.localPosition = Vector3.Lerp(this.originalReticuleHookLocalPosition, b, this.GetInterpolatedAimAlpha());
			}
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x0019B168 File Offset: 0x00199368
		private void UpdateMovementSpeedMultiplier()
		{
			this.movementSpeedMultiplier = 1f;
			if (this.isAiming)
			{
				this.movementSpeedMultiplier *= this.equippedGunAsset.aimingMovementSpeedMultiplier;
			}
			if (this.thirdAttachments.barrelAsset != null)
			{
				this.movementSpeedMultiplier *= this.thirdAttachments.barrelAsset.equipableMovementSpeedMultiplier;
				if (this.isAiming)
				{
					this.movementSpeedMultiplier *= this.thirdAttachments.barrelAsset.aimingMovementSpeedMultiplier;
				}
			}
			if (this.thirdAttachments.tacticalAsset != null)
			{
				this.movementSpeedMultiplier *= this.thirdAttachments.tacticalAsset.equipableMovementSpeedMultiplier;
				if (this.isAiming)
				{
					this.movementSpeedMultiplier *= this.thirdAttachments.tacticalAsset.aimingMovementSpeedMultiplier;
				}
			}
			if (this.thirdAttachments.sightAsset != null)
			{
				this.movementSpeedMultiplier *= this.thirdAttachments.sightAsset.equipableMovementSpeedMultiplier;
				if (this.isAiming)
				{
					this.movementSpeedMultiplier *= this.thirdAttachments.sightAsset.aimingMovementSpeedMultiplier;
				}
			}
			if (this.thirdAttachments.magazineAsset != null)
			{
				this.movementSpeedMultiplier *= this.thirdAttachments.magazineAsset.equipableMovementSpeedMultiplier;
				if (this.isAiming)
				{
					this.movementSpeedMultiplier *= this.thirdAttachments.magazineAsset.aimingMovementSpeedMultiplier;
				}
			}
			if (this.thirdAttachments.gripAsset != null)
			{
				this.movementSpeedMultiplier *= this.thirdAttachments.gripAsset.equipableMovementSpeedMultiplier;
				if (this.isAiming)
				{
					this.movementSpeedMultiplier *= this.thirdAttachments.gripAsset.aimingMovementSpeedMultiplier;
				}
			}
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x0019B32C File Offset: 0x0019952C
		private void UpdateAimInDuration()
		{
			float num = this.equippedGunAsset.aimInDuration;
			if (this.thirdAttachments.barrelAsset != null)
			{
				num *= this.thirdAttachments.barrelAsset.aimDurationMultiplier;
			}
			if (this.thirdAttachments.tacticalAsset != null)
			{
				num *= this.thirdAttachments.tacticalAsset.aimDurationMultiplier;
			}
			if (this.thirdAttachments.sightAsset != null)
			{
				num *= this.thirdAttachments.sightAsset.aimDurationMultiplier;
			}
			if (this.thirdAttachments.magazineAsset != null)
			{
				num *= this.thirdAttachments.magazineAsset.aimDurationMultiplier;
			}
			if (this.thirdAttachments.gripAsset != null)
			{
				num *= this.thirdAttachments.gripAsset.aimDurationMultiplier;
			}
			this.maxAimingAccuracy = Mathf.Clamp(Mathf.RoundToInt(num * 50f), 1, 200);
			this.maxAimingAccuracyReciprocal = 1f / (float)this.maxAimingAccuracy;
			if (this.aimAccuracy > this.maxAimingAccuracy)
			{
				this.aimAccuracy = this.maxAimingAccuracy;
			}
			if (this.equippedGunAsset.shouldScaleAimAnimations)
			{
				float num2 = (float)this.maxAimingAccuracy / 50f;
				float animationLength = base.player.animator.GetAnimationLength("Aim_Start", false);
				base.player.animator.setAnimationSpeed("Aim_Start", animationLength / num2);
				float animationLength2 = base.player.animator.GetAnimationLength("Aim_Stop", false);
				base.player.animator.setAnimationSpeed("Aim_Stop", animationLength2 / num2);
			}
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x0019B4A8 File Offset: 0x001996A8
		private void DestroyLaser()
		{
		}

		/// <summary>
		/// Code common for regular gun and sentry gun.
		/// </summary>
		// Token: 0x06004557 RID: 17751 RVA: 0x0019B4AC File Offset: 0x001996AC
		internal static void DetonateExplosiveMagazine(ItemMagazineAsset magazineAsset, Vector3 position, Player instigatingPlayer, ERagdollEffect ragdollEffect)
		{
			EffectAsset effectAsset = magazineAsset.FindExplosionEffect();
			if (effectAsset != null)
			{
				EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
				{
					position = position,
					relevantDistance = EffectManager.MEDIUM,
					wasInstigatedByPlayer = true
				});
			}
			CSteamID killer = (instigatingPlayer != null) ? instigatingPlayer.channel.owner.playerID.steamID : CSteamID.Nil;
			List<EPlayerKill> list;
			DamageTool.explode(new ExplosionParameters(position, magazineAsset.range, EDeathCause.SPLASH, killer)
			{
				playerDamage = magazineAsset.playerDamage,
				zombieDamage = magazineAsset.zombieDamage,
				animalDamage = magazineAsset.animalDamage,
				barricadeDamage = magazineAsset.barricadeDamage,
				structureDamage = magazineAsset.structureDamage,
				vehicleDamage = magazineAsset.vehicleDamage,
				resourceDamage = magazineAsset.resourceDamage,
				objectDamage = magazineAsset.objectDamage,
				damageOrigin = EDamageOrigin.Bullet_Explosion,
				ragdollEffect = ragdollEffect,
				launchSpeed = magazineAsset.explosionLaunchSpeed
			}, out list);
			if (instigatingPlayer != null)
			{
				foreach (EPlayerKill kill in list)
				{
					instigatingPlayer.sendStat(kill);
				}
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06004558 RID: 17752 RVA: 0x0019B600 File Offset: 0x00199800
		private bool CanDamageInvulnerableEntities
		{
			get
			{
				if (((ItemWeaponAsset)base.player.equipment.asset).isInvulnerable)
				{
					return true;
				}
				Attachments attachments = this.thirdAttachments;
				bool? flag;
				if (attachments == null)
				{
					flag = default(bool?);
				}
				else
				{
					ItemBarrelAsset barrelAsset = attachments.barrelAsset;
					flag = ((barrelAsset != null) ? new bool?(barrelAsset.CanDamageInvulernableEntities) : default(bool?));
				}
				bool? flag2 = flag;
				if (flag2.GetValueOrDefault())
				{
					return true;
				}
				Attachments attachments2 = this.thirdAttachments;
				bool? flag3;
				if (attachments2 == null)
				{
					flag3 = default(bool?);
				}
				else
				{
					ItemTacticalAsset tacticalAsset = attachments2.tacticalAsset;
					flag3 = ((tacticalAsset != null) ? new bool?(tacticalAsset.CanDamageInvulernableEntities) : default(bool?));
				}
				flag2 = flag3;
				if (flag2.GetValueOrDefault())
				{
					return true;
				}
				Attachments attachments3 = this.thirdAttachments;
				bool? flag4;
				if (attachments3 == null)
				{
					flag4 = default(bool?);
				}
				else
				{
					ItemGripAsset gripAsset = attachments3.gripAsset;
					flag4 = ((gripAsset != null) ? new bool?(gripAsset.CanDamageInvulernableEntities) : default(bool?));
				}
				flag2 = flag4;
				if (flag2.GetValueOrDefault())
				{
					return true;
				}
				Attachments attachments4 = this.thirdAttachments;
				bool? flag5;
				if (attachments4 == null)
				{
					flag5 = default(bool?);
				}
				else
				{
					ItemSightAsset sightAsset = attachments4.sightAsset;
					flag5 = ((sightAsset != null) ? new bool?(sightAsset.CanDamageInvulernableEntities) : default(bool?));
				}
				flag2 = flag5;
				if (flag2.GetValueOrDefault())
				{
					return true;
				}
				Attachments attachments5 = this.thirdAttachments;
				bool? flag6;
				if (attachments5 == null)
				{
					flag6 = default(bool?);
				}
				else
				{
					ItemMagazineAsset magazineAsset = attachments5.magazineAsset;
					flag6 = ((magazineAsset != null) ? new bool?(magazineAsset.CanDamageInvulernableEntities) : default(bool?));
				}
				flag2 = flag6;
				return flag2.GetValueOrDefault();
			}
		}

		// Token: 0x04002E39 RID: 11833
		private static readonly PlayerDamageMultiplier DAMAGE_PLAYER_MULTIPLIER = new PlayerDamageMultiplier(40f, 0.6f, 0.6f, 0.8f, 1.1f);

		// Token: 0x04002E3A RID: 11834
		private static readonly ZombieDamageMultiplier DAMAGE_ZOMBIE_MULTIPLIER = new ZombieDamageMultiplier(40f, 0.3f, 0.3f, 0.6f, 1.1f);

		// Token: 0x04002E3B RID: 11835
		private static readonly AnimalDamageMultiplier DAMAGE_ANIMAL_MULTIPLIER = new AnimalDamageMultiplier(40f, 0.3f, 0.6f, 1.1f);

		// Token: 0x04002E3C RID: 11836
		private static readonly float SHAKE_CROUCH = 0.85f;

		// Token: 0x04002E3D RID: 11837
		private static readonly float SHAKE_PRONE = 0.7f;

		// Token: 0x04002E3E RID: 11838
		private static readonly float SWAY_CROUCH = 0.85f;

		// Token: 0x04002E3F RID: 11839
		private static readonly float SWAY_PRONE = 0.7f;

		// Token: 0x04002E40 RID: 11840
		private Local localization;

		// Token: 0x04002E41 RID: 11841
		private Bundle icons;

		// Token: 0x04002E42 RID: 11842
		private SleekButtonIcon sightButton;

		// Token: 0x04002E43 RID: 11843
		private SleekJars sightJars;

		// Token: 0x04002E44 RID: 11844
		private SleekButtonIcon tacticalButton;

		// Token: 0x04002E45 RID: 11845
		private SleekJars tacticalJars;

		// Token: 0x04002E46 RID: 11846
		private SleekButtonIcon gripButton;

		// Token: 0x04002E47 RID: 11847
		private SleekJars gripJars;

		// Token: 0x04002E48 RID: 11848
		private SleekButtonIcon barrelButton;

		// Token: 0x04002E49 RID: 11849
		private ISleekLabel barrelQualityLabel;

		// Token: 0x04002E4A RID: 11850
		private ISleekImage barrelQualityImage;

		// Token: 0x04002E4B RID: 11851
		private SleekJars barrelJars;

		// Token: 0x04002E4C RID: 11852
		private SleekButtonIcon magazineButton;

		// Token: 0x04002E4D RID: 11853
		private ISleekLabel magazineQualityLabel;

		// Token: 0x04002E4E RID: 11854
		private ISleekImage magazineQualityImage;

		// Token: 0x04002E4F RID: 11855
		private SleekJars magazineJars;

		// Token: 0x04002E50 RID: 11856
		private ISleekLabel rangeLabel;

		// Token: 0x04002E51 RID: 11857
		private ISleekBox infoBox;

		// Token: 0x04002E52 RID: 11858
		private ISleekLabel ammoLabel;

		// Token: 0x04002E53 RID: 11859
		private ISleekLabel firemodeLabel;

		// Token: 0x04002E54 RID: 11860
		private ISleekLabel attachLabel;

		// Token: 0x04002E55 RID: 11861
		internal Attachments firstAttachments;

		// Token: 0x04002E56 RID: 11862
		private ParticleSystem firstShellEmitter;

		// Token: 0x04002E57 RID: 11863
		private ParticleSystem firstMuzzleEmitter;

		// Token: 0x04002E58 RID: 11864
		private Transform firstFakeLight;

		// Token: 0x04002E59 RID: 11865
		private Transform firstFakeLight_0;

		// Token: 0x04002E5A RID: 11866
		private Transform firstFakeLight_1;

		// Token: 0x04002E5B RID: 11867
		private Attachments thirdAttachments;

		// Token: 0x04002E5C RID: 11868
		private ParticleSystem thirdShellEmitter;

		// Token: 0x04002E5D RID: 11869
		private ParticleSystemRenderer thirdShellRenderer;

		// Token: 0x04002E5E RID: 11870
		private ParticleSystem thirdMuzzleEmitter;

		// Token: 0x04002E5F RID: 11871
		private float minigunSpeed;

		// Token: 0x04002E60 RID: 11872
		private float minigunDistance;

		// Token: 0x04002E61 RID: 11873
		private Transform firstMinigunBarrel;

		// Token: 0x04002E62 RID: 11874
		private Transform thirdMinigunBarrel;

		// Token: 0x04002E63 RID: 11875
		private Text firstAmmoCounter;

		// Token: 0x04002E64 RID: 11876
		private Text thirdAmmoCounter;

		// Token: 0x04002E65 RID: 11877
		private EffectAsset currentTracerEffectAsset;

		// Token: 0x04002E66 RID: 11878
		private ParticleSystem tracerEmitter;

		// Token: 0x04002E67 RID: 11879
		private AudioSource gunshotAudioSource;

		// Token: 0x04002E68 RID: 11880
		private AudioSource whir;

		/// <summary>
		/// reticuleHook.localPosition after instantiation, or zero if null.
		/// </summary>
		// Token: 0x04002E69 RID: 11881
		private Vector3 originalReticuleHookLocalPosition;

		// Token: 0x04002E6A RID: 11882
		private bool isShooting;

		/// <summary>
		/// True if startPrimary was called this simulation frame.
		/// Allows gun to shoot even if stopPrimary is called immediately afterwards.
		/// </summary>
		// Token: 0x04002E6B RID: 11883
		private bool wasTriggerJustPulled;

		// Token: 0x04002E6C RID: 11884
		private bool isJabbing;

		// Token: 0x04002E6E RID: 11886
		private bool isMinigunSpinning;

		// Token: 0x04002E6F RID: 11887
		private bool isSprinting;

		// Token: 0x04002E70 RID: 11888
		private bool isReloading;

		// Token: 0x04002E71 RID: 11889
		private bool isHammering;

		// Token: 0x04002E72 RID: 11890
		private bool isAttaching;

		// Token: 0x04002E73 RID: 11891
		private bool isUnjamming;

		// Token: 0x04002E74 RID: 11892
		private float lastShot;

		// Token: 0x04002E75 RID: 11893
		private float lastRechamber;

		// Token: 0x04002E76 RID: 11894
		private uint lastFire;

		// Token: 0x04002E77 RID: 11895
		private uint lastJab;

		// Token: 0x04002E78 RID: 11896
		private bool isFired;

		// Token: 0x04002E79 RID: 11897
		private int bursts;

		/// <summary>
		/// Remaining calls to tock before firing.
		/// </summary>
		// Token: 0x04002E7A RID: 11898
		private int fireDelayCounter;

		// Token: 0x04002E7B RID: 11899
		private int aimAccuracy;

		// Token: 0x04002E7C RID: 11900
		private uint steadyAccuracy;

		// Token: 0x04002E7D RID: 11901
		private bool canSteady;

		// Token: 0x04002E7E RID: 11902
		private float swayTime;

		// Token: 0x04002E7F RID: 11903
		private List<BulletInfo> bullets;

		// Token: 0x04002E80 RID: 11904
		private float startedReload;

		// Token: 0x04002E81 RID: 11905
		private float startedHammer;

		// Token: 0x04002E82 RID: 11906
		private float startedUnjammingChamber;

		// Token: 0x04002E83 RID: 11907
		private float reloadTime;

		// Token: 0x04002E84 RID: 11908
		private float hammerTime;

		// Token: 0x04002E85 RID: 11909
		private float unjamChamberDuration;

		// Token: 0x04002E86 RID: 11910
		private bool needsHammer;

		// Token: 0x04002E87 RID: 11911
		private bool needsRechamber;

		// Token: 0x04002E88 RID: 11912
		private bool needsEject;

		// Token: 0x04002E89 RID: 11913
		private bool needsUnload;

		// Token: 0x04002E8A RID: 11914
		private bool needsUnplace;

		// Token: 0x04002E8B RID: 11915
		private bool needsReplace;

		/// <summary>
		/// Is the tactical attachment toggle on?
		/// e.g. True when the laser is enabled.
		/// </summary>
		// Token: 0x04002E8C RID: 11916
		private bool interact;

		// Token: 0x04002E8D RID: 11917
		private byte ammo;

		// Token: 0x04002E8E RID: 11918
		private EFiremode firemode;

		// Token: 0x04002E8F RID: 11919
		private List<InventorySearch> sightSearch;

		// Token: 0x04002E90 RID: 11920
		private List<InventorySearch> tacticalSearch;

		// Token: 0x04002E91 RID: 11921
		private List<InventorySearch> gripSearch;

		// Token: 0x04002E92 RID: 11922
		private List<InventorySearch> barrelSearch;

		// Token: 0x04002E93 RID: 11923
		private List<InventorySearch> magazineSearch;

		/// <summary>
		/// Factor e.g. 2 is a 2x multiplier.
		/// Prior to 2022-04-11 this was the target field of view. (90/fov)
		/// </summary>
		// Token: 0x04002E94 RID: 11924
		private float firstPersonZoomFactor;

		/// <summary>
		/// Zoom multiplier in third-person.
		/// </summary>
		// Token: 0x04002E95 RID: 11925
		private float thirdPersonZoomFactor = 1.25f;

		/// <summary>
		/// Whether main camera field of view should zoom without scope camera / scope overlay.
		/// </summary>
		// Token: 0x04002E96 RID: 11926
		private bool shouldZoomUsingEyes;

		// Token: 0x04002E97 RID: 11927
		private float crosshair;

		// Token: 0x04002E98 RID: 11928
		private bool wasLaser;

		// Token: 0x04002E99 RID: 11929
		private bool wasLight;

		// Token: 0x04002E9A RID: 11930
		private bool wasRange;

		// Token: 0x04002E9B RID: 11931
		private bool wasBayonet;

		// Token: 0x04002E9C RID: 11932
		private bool inRange;

		// Token: 0x04002E9D RID: 11933
		private bool fireTacticalInput;

		// Token: 0x04002E9E RID: 11934
		private RaycastHit contact;

		// Token: 0x04002E9F RID: 11935
		private UseableGunEventHook firstEventComponent;

		// Token: 0x04002EA0 RID: 11936
		private UseableGunEventHook thirdEventComponent;

		// Token: 0x04002EA1 RID: 11937
		private UseableGunEventHook characterEventComponent;

		// Token: 0x04002EA2 RID: 11938
		private static readonly ServerInstanceMethod<EFiremode> SendChangeFiremode = ServerInstanceMethod<EFiremode>.Get(typeof(UseableGun), "ReceiveChangeFiremode");

		// Token: 0x04002EA3 RID: 11939
		private static readonly ClientInstanceMethod<Vector3, Vector3, ushort, ushort> SendPlayProject = ClientInstanceMethod<Vector3, Vector3, ushort, ushort>.Get(typeof(UseableGun), "ReceivePlayProject");

		// Token: 0x04002EA4 RID: 11940
		private static readonly ClientInstanceMethod SendPlayShoot = ClientInstanceMethod.Get(typeof(UseableGun), "ReceivePlayShoot");

		// Token: 0x04002EA5 RID: 11941
		private static MasterBundleReference<AudioClip> jabClipRef = new MasterBundleReference<AudioClip>("core.masterbundle", "Sounds/MeleeAttack_01.mp3");

		// Token: 0x04002EA6 RID: 11942
		internal const float BALLISTICS_DELTA_TIME = 0.02f;

		// Token: 0x04002EA7 RID: 11943
		private static readonly ServerInstanceMethod<byte, byte, byte, byte[]> SendAttachSight = ServerInstanceMethod<byte, byte, byte, byte[]>.Get(typeof(UseableGun), "ReceiveAttachSight");

		// Token: 0x04002EA8 RID: 11944
		private static readonly ServerInstanceMethod<byte, byte, byte, byte[]> SendAttachTactical = ServerInstanceMethod<byte, byte, byte, byte[]>.Get(typeof(UseableGun), "ReceiveAttachTactical");

		// Token: 0x04002EA9 RID: 11945
		private static readonly ServerInstanceMethod<byte, byte, byte, byte[]> SendAttachGrip = ServerInstanceMethod<byte, byte, byte, byte[]>.Get(typeof(UseableGun), "ReceiveAttachGrip");

		// Token: 0x04002EAA RID: 11946
		private static readonly ServerInstanceMethod<byte, byte, byte, byte[]> SendAttachBarrel = ServerInstanceMethod<byte, byte, byte, byte[]>.Get(typeof(UseableGun), "ReceiveAttachBarrel");

		// Token: 0x04002EAB RID: 11947
		private static readonly ServerInstanceMethod<byte, byte, byte, byte[]> SendAttachMagazine = ServerInstanceMethod<byte, byte, byte, byte[]>.Get(typeof(UseableGun), "ReceiveAttachMagazine");

		// Token: 0x04002EAC RID: 11948
		private static readonly ClientInstanceMethod<bool> SendPlayReload = ClientInstanceMethod<bool>.Get(typeof(UseableGun), "ReceivePlayReload");

		// Token: 0x04002EAD RID: 11949
		private static readonly ClientInstanceMethod<byte> SendPlayChamberJammed = ClientInstanceMethod<byte>.Get(typeof(UseableGun), "ReceivePlayChamberJammed");

		// Token: 0x04002EAE RID: 11950
		private static readonly ClientInstanceMethod SendPlayAimStart = ClientInstanceMethod.Get(typeof(UseableGun), "ReceivePlayAimStart");

		// Token: 0x04002EAF RID: 11951
		private static readonly ClientInstanceMethod SendPlayAimStop = ClientInstanceMethod.Get(typeof(UseableGun), "ReceivePlayAimStop");

		// Token: 0x04002EB0 RID: 11952
		private int maxAimingAccuracy;

		// Token: 0x04002EB1 RID: 11953
		private float maxAimingAccuracyReciprocal;

		// Token: 0x04002EB2 RID: 11954
		private List<UseableGun.DistanceMarker> scopeDistanceMarkers;

		// Token: 0x04002EB3 RID: 11955
		private static Material scopeDistanceMarkerMaterial;

		// Token: 0x04002EB4 RID: 11956
		internal const float DEFAULT_THIRD_PERSON_ZOOM_FACTOR = 1.25f;

		// Token: 0x02000A16 RID: 2582
		// (Invoke) Token: 0x06004D6C RID: 19820
		public delegate void ChangeAttachmentRequestHandler(PlayerEquipment equipment, UseableGun gun, Item oldItem, ItemJar newItem, ref bool shouldAllow);

		// Token: 0x02000A17 RID: 2583
		// (Invoke) Token: 0x06004D70 RID: 19824
		public delegate void BulletSpawnedHandler(UseableGun gun, BulletInfo bullet);

		// Token: 0x02000A18 RID: 2584
		// (Invoke) Token: 0x06004D74 RID: 19828
		public delegate void BulletHitHandler(UseableGun gun, BulletInfo bullet, InputInfo hit, ref bool shouldAllow);

		// Token: 0x02000A19 RID: 2585
		// (Invoke) Token: 0x06004D78 RID: 19832
		public delegate void ProjectileSpawnedHandler(UseableGun sender, GameObject projectile);

		// Token: 0x02000A1A RID: 2586
		private class DistanceMarker
		{
			// Token: 0x04003519 RID: 13593
			public bool isActive;

			// Token: 0x0400351A RID: 13594
			public float distance;

			// Token: 0x0400351B RID: 13595
			public Transform transform;

			// Token: 0x0400351C RID: 13596
			public LineRenderer lineComponent;

			// Token: 0x0400351D RID: 13597
			public TextMeshPro textComponent;
		}
	}
}
