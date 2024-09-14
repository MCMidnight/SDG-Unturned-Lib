using System;
using System.Collections.Generic;
using SDG.Framework.Water;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000829 RID: 2089
	public class Zombie : MonoBehaviour
	{
		/// <summary>
		/// Overrides hat item from zombie table with a specific item ID.
		/// </summary>
		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06004711 RID: 18193 RVA: 0x001A8383 File Offset: 0x001A6583
		private ushort hatID
		{
			get
			{
				if (!this.speciality.IsDLVolatile())
				{
					return 0;
				}
				return 960;
			}
		}

		/// <summary>
		/// Overrides gear item from zombie table with a specific item ID.
		/// </summary>
		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06004712 RID: 18194 RVA: 0x001A8399 File Offset: 0x001A6599
		private ushort gearID
		{
			get
			{
				if (!this.speciality.IsDLVolatile())
				{
					return 0;
				}
				return 961;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06004713 RID: 18195 RVA: 0x001A83AF File Offset: 0x001A65AF
		// (set) Token: 0x06004714 RID: 18196 RVA: 0x001A83B8 File Offset: 0x001A65B8
		public byte move
		{
			get
			{
				return this._move;
			}
			set
			{
				this._move = value;
				this.moveAnim = "Move_" + this.move.ToString();
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x06004715 RID: 18197 RVA: 0x001A83EA File Offset: 0x001A65EA
		// (set) Token: 0x06004716 RID: 18198 RVA: 0x001A83F4 File Offset: 0x001A65F4
		public byte idle
		{
			get
			{
				return this._idle;
			}
			set
			{
				this._idle = value;
				this.idleAnim = "Idle_" + this.idle.ToString();
			}
		}

		/// <summary>
		/// Add or remove from ticking list if needed.
		/// Separated from updateTicking in order to move once after first spawn.
		/// </summary>
		// Token: 0x06004717 RID: 18199 RVA: 0x001A8428 File Offset: 0x001A6628
		private void setTicking(bool wantsToTick)
		{
			if (wantsToTick)
			{
				if (!this.isTicking)
				{
					this.isTicking = true;
					ZombieManager.tickingZombies.Add(this);
					this.lastTick = Time.time;
					return;
				}
			}
			else if (this.isTicking)
			{
				this.isTicking = false;
				ZombieManager.tickingZombies.RemoveFast(this);
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x06004718 RID: 18200 RVA: 0x001A8479 File Offset: 0x001A6679
		// (set) Token: 0x06004719 RID: 18201 RVA: 0x001A8484 File Offset: 0x001A6684
		public bool isHunting
		{
			get
			{
				return this._isHunting;
			}
			set
			{
				if (value != this.isHunting)
				{
					this._isHunting = value;
					if (this.isHunting)
					{
						this.needsTickForPlacement = false;
						this.setTicking(true);
						if (this.speciality == EZombieSpeciality.FLANKER_FRIENDLY)
						{
							ZombieManager.sendZombieSpeciality(this, EZombieSpeciality.FLANKER_STALK);
							return;
						}
					}
					else
					{
						if (!this.needsTickForPlacement)
						{
							this.setTicking(false);
						}
						if (this.isWandering)
						{
							this.isWandering = false;
							ZombieManager.wanderingCount--;
						}
						if (this.speciality == EZombieSpeciality.FLANKER_STALK)
						{
							ZombieManager.sendZombieSpeciality(this, EZombieSpeciality.FLANKER_FRIENDLY);
						}
					}
				}
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x0600471A RID: 18202 RVA: 0x001A8502 File Offset: 0x001A6702
		public float lastDead
		{
			get
			{
				return this._lastDead;
			}
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x001A850A File Offset: 0x001A670A
		public float GetHealth()
		{
			return (float)this.health;
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x001A8513 File Offset: 0x001A6713
		public float GetMaxHealth()
		{
			return (float)this.maxHealth;
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x0600471D RID: 18205 RVA: 0x001A851C File Offset: 0x001A671C
		public bool isHyper
		{
			get
			{
				return this.zombieRegion.isHyper && this.speciality != EZombieSpeciality.BOSS_ALL && this.speciality != EZombieSpeciality.BOSS_BUAK_FINAL;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x0600471E RID: 18206 RVA: 0x001A8544 File Offset: 0x001A6744
		public bool isRadioactive
		{
			get
			{
				return this.zombieRegion.isRadioactive;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x0600471F RID: 18207 RVA: 0x001A8551 File Offset: 0x001A6751
		public bool isBoss
		{
			get
			{
				return this.speciality.IsBoss();
			}
		}

		/// <summary>
		/// Boss zombies are considered mega as well.
		/// </summary>
		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06004720 RID: 18208 RVA: 0x001A855E File Offset: 0x001A675E
		public bool isMega
		{
			get
			{
				return this.speciality == EZombieSpeciality.MEGA || this.isBoss || this.speciality == EZombieSpeciality.BOSS_ALL;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06004721 RID: 18209 RVA: 0x001A857D File Offset: 0x001A677D
		public bool isCutesy
		{
			get
			{
				return this.speciality == EZombieSpeciality.SPIRIT;
			}
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x001A858C File Offset: 0x001A678C
		private float GetHorizontalAttackRangeSquared()
		{
			if (this.barricade != null)
			{
				return Zombie.ATTACK_BARRICADE * (float)(this.isMega ? 2 : 1);
			}
			if (this.targetObstructionVehicle != null)
			{
				return Zombie.ATTACK_VEHICLE * (float)(this.isMega ? 2 : 1);
			}
			if (!(this.targetPassengerVehicle != null))
			{
				Player player = this.player;
				if (!(((player != null) ? player.movement.getVehicle() : null) != null))
				{
					return Zombie.ATTACK_PLAYER * ((this.speciality == EZombieSpeciality.NORMAL) ? 0.5f : 1f) * (float)(this.isMega ? 2 : 1);
				}
			}
			return Zombie.ATTACK_VEHICLE * (float)(this.isMega ? 2 : 1);
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x001A8647 File Offset: 0x001A6847
		private float GetVerticalAttackRange()
		{
			return (this.isHyper ? 3.5f : 2.1f) * (this.isMega ? 1.5f : 1f);
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x001A8674 File Offset: 0x001A6874
		public void tellAlive(byte newType, byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition, byte newAngle)
		{
			this.type = newType;
			this.speciality = (EZombieSpeciality)newSpeciality;
			this.shirt = newShirt;
			this.pants = newPants;
			this.hat = newHat;
			this.gear = newGear;
			this.isDead = false;
			this.SetCountedAsAliveInZombieRegion(true);
			this.SetCountedAsAliveBossInZombieRegion(this.isBoss);
			base.transform.position = newPosition;
			base.transform.rotation = Quaternion.Euler(0f, (float)(newAngle * 2), 0f);
			this.updateDifficulty();
			this.updateLife();
			this.apply();
			this.updateEffects();
			this.updateVisibility(this.speciality != EZombieSpeciality.FLANKER_STALK && this.speciality != EZombieSpeciality.SPIRIT && this.speciality != EZombieSpeciality.BOSS_SPIRIT, false);
			this.updateStates();
			if (Provider.isServer)
			{
				this.reset();
			}
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x001A874C File Offset: 0x001A694C
		public void tellDead(Vector3 newRagdoll, ERagdollEffect ragdollEffect)
		{
			this.isDead = true;
			this.SetCountedAsAliveInZombieRegion(false);
			this.SetCountedAsAliveBossInZombieRegion(false);
			if (this.zombieRegion.hasBeacon && Provider.isServer)
			{
				BeaconManager.checkBeacon(this.bound).despawnAlive();
			}
			this._lastDead = Time.realtimeSinceStartup;
			this.updateLife();
			if (Provider.isServer)
			{
				this.stop();
			}
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x001A87B0 File Offset: 0x001A69B0
		[Obsolete]
		public void tellState(Vector3 newPosition, byte newAngle)
		{
			this.tellState(newPosition, (float)newAngle * 2f);
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x001A87C1 File Offset: 0x001A69C1
		public void tellState(Vector3 newPosition, float newYaw)
		{
			this.lastUpdatedPos = newPosition;
			this.lastUpdatedAngle = newYaw;
			this.interpPositionTarget = newPosition;
			this.interpYawTarget = newYaw;
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x001A87E0 File Offset: 0x001A69E0
		public void tellSpeciality(EZombieSpeciality newSpeciality)
		{
			this.speciality = newSpeciality;
			this.SetCountedAsAliveBossInZombieRegion(!this.isDead && this.isBoss);
			this.updateEffects();
			this.updateVisibility(this.speciality != EZombieSpeciality.FLANKER_STALK && this.speciality != EZombieSpeciality.SPIRIT && this.speciality != EZombieSpeciality.BOSS_SPIRIT, true);
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x001A883C File Offset: 0x001A6A3C
		public void askThrow()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastSpecial = Time.time;
			this.isThrowingBoulder = true;
			this.isPlayingBoulder = true;
			this.boulderItem = ((GameObject)Object.Instantiate(Resources.Load("Characters/Mega_Boulder_Item"))).transform;
			this.boulderItem.name = "Boulder";
			this.boulderItem.parent = this.rightHook;
			this.boulderItem.localPosition = Vector3.zero;
			this.boulderItem.localRotation = Quaternion.Euler(0f, 0f, 90f);
			this.boulderItem.localScale = Vector3.one;
			Object.Destroy(this.boulderItem.gameObject, 2f);
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x001A8900 File Offset: 0x001A6B00
		public void askBoulder(Vector3 origin, Vector3 direction)
		{
			if (this.isDead)
			{
				return;
			}
			Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Characters/Mega_Boulder_Projectile_Server"))).transform;
			transform.name = "Boulder";
			EffectManager.RegisterDebris(transform.gameObject);
			transform.position = origin;
			transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler((float)Random.Range(0, 2) * 180f, (float)Random.Range(0, 2) * 180f, (float)Random.Range(0, 2) * 180f);
			transform.localScale = Vector3.one * 1.75f;
			transform.GetComponent<Rigidbody>().AddForce(direction * 1500f);
			transform.GetComponent<Rigidbody>().AddRelativeTorque(Random.Range(-500f, 500f), Random.Range(-500f, 500f), Random.Range(-500f, 500f), ForceMode.Force);
			transform.Find("Trap").gameObject.AddComponent<Boulder>();
			Object.Destroy(transform.gameObject, 8f);
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x001A8A15 File Offset: 0x001A6C15
		public void askSpit()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastSpecial = Time.time;
			this.isSpittingAcid = true;
			this.isPlayingSpit = true;
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x001A8A3C File Offset: 0x001A6C3C
		public void askAcid(Vector3 origin, Vector3 direction)
		{
			if (this.isDead)
			{
				return;
			}
			Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Characters/Acid_Projectile_Server"))).transform;
			transform.name = "Acid";
			EffectManager.RegisterDebris(transform.gameObject);
			transform.position = origin;
			transform.rotation = Quaternion.LookRotation(direction);
			transform.GetComponent<Rigidbody>().AddForce(direction * 1000f);
			transform.Find("Trap").gameObject.AddComponent<Acid>().effectGuid = ((this.speciality == EZombieSpeciality.BOSS_NUCLEAR) ? Zombie.Zombie_7_Ref : Zombie.Zombie_3_Ref).GUID;
			Object.Destroy(transform.gameObject, 8f);
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x001A8AF1 File Offset: 0x001A6CF1
		public void askCharge()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastSpecial = Time.time;
			this.isChargingSpark = true;
			this.isPlayingCharge = true;
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x001A8B18 File Offset: 0x001A6D18
		public void askSpark(Vector3 target)
		{
			if (this.isDead)
			{
				return;
			}
			Vector3 vector = target - this.sparkSystem.transform.position;
			Vector3 normalized = vector.normalized;
			EffectAsset effectAsset = Assets.find<EffectAsset>(Zombie.Zombie_4_Ref);
			if (effectAsset != null)
			{
				Transform transform = EffectManager.effect(effectAsset, this.sparkSystem.transform.position + normalized * 2f, normalized);
				if (transform != null)
				{
					transform.GetComponent<ParticleSystem>().main.startLifetime = (vector.magnitude - 2f) / 128f;
				}
			}
			EffectAsset effectAsset2 = Assets.find<EffectAsset>(Zombie.Zombie_6_Ref);
			if (effectAsset2 != null)
			{
				EffectManager.effect(effectAsset2, target, -normalized);
			}
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x001A8BD8 File Offset: 0x001A6DD8
		public void askStomp()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastSpecial = Time.time;
			this.isStompingWind = true;
			this.isPlayingWind = true;
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x001A8C08 File Offset: 0x001A6E08
		public void askBreath()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastSpecial = Time.time;
			this.isBreathingFire = true;
			this.isPlayingFire = true;
			this.fireDamage = 0f;
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x001A8C44 File Offset: 0x001A6E44
		public void askAttack(byte id)
		{
			if (this.isDead)
			{
				return;
			}
			this.lastAttack = Time.time;
			this.specialAttackDelay = Random.Range(2f, 4f);
			this.isPlayingAttack = true;
			if (this.speciality == EZombieSpeciality.FLANKER_FRIENDLY || this.speciality == EZombieSpeciality.FLANKER_STALK)
			{
				this.updateVisibility(true, true);
			}
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x001A8C9C File Offset: 0x001A6E9C
		public void askStartle(byte id)
		{
			if (this.isDead)
			{
				return;
			}
			this.lastStartle = Time.time;
			this.specialStartleDelay = Random.Range(1f, 2f);
			this.isPlayingStartle = true;
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x001A8CD9 File Offset: 0x001A6ED9
		public void askStun(byte id)
		{
			if (this.isDead)
			{
				return;
			}
			this.lastStun = Time.time;
			this.isPlayingStun = true;
		}

		/// <summary>
		/// If damage exceeds this value, stun the zombie.
		/// </summary>
		// Token: 0x06004734 RID: 18228 RVA: 0x001A8CF8 File Offset: 0x001A6EF8
		public int getStunDamageThreshold()
		{
			if (this.isMega)
			{
				int num = (this.difficulty != null) ? this.difficulty.Mega_Stun_Threshold : -1;
				if (num > 0)
				{
					return num;
				}
				return 150;
			}
			else
			{
				int num2 = (this.difficulty != null) ? this.difficulty.Normal_Stun_Threshold : -1;
				if (num2 > 0)
				{
					return num2;
				}
				return 20;
			}
		}

		/// <summary>
		/// Used to kill night-only zombies at dawn.
		/// </summary>
		// Token: 0x06004735 RID: 18229 RVA: 0x001A8D50 File Offset: 0x001A6F50
		public void killWithFireExplosion()
		{
			if (this.isDead)
			{
				return;
			}
			EPlayerKill eplayerKill;
			uint num;
			DamageTool.damageZombie(DamageZombieParameters.makeInstakill(this), out eplayerKill, out num);
			if (!this.isDead)
			{
				return;
			}
			if (this.burner != null)
			{
				EffectAsset effectAsset = Assets.find<EffectAsset>(Zombie.Zombie_2_Ref);
				if (effectAsset != null)
				{
					EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
					{
						position = this.burner.position,
						relevantDistance = EffectManager.MEDIUM
					});
				}
				List<EPlayerKill> list;
				DamageTool.explode(base.transform.position + new Vector3(0f, 0.25f, 0f), 4f, EDeathCause.BURNER, CSteamID.Nil, 40f, 0f, 40f, 0f, 0f, 0f, 0f, 0f, out list, EExplosionDamageType.ZOMBIE_FIRE, 4f, true, false, EDamageOrigin.Flamable_Zombie_Explosion, ERagdollEffect.NONE);
			}
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x001A8E34 File Offset: 0x001A7034
		public void askDamage(ushort amount, Vector3 newRagdoll, out EPlayerKill kill, out uint xp, bool trackKill = true, bool dropLoot = true, EZombieStunOverride stunOverride = EZombieStunOverride.None, ERagdollEffect ragdollEffect = ERagdollEffect.NONE)
		{
			kill = EPlayerKill.NONE;
			xp = 0U;
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (!this.isDead)
			{
				if (this.zombieRegion.hasBeacon)
				{
					amount = MathfEx.CeilToUShort((float)amount / ((float)Mathf.Max(1, BeaconManager.checkBeacon(this.bound).initialParticipants) * 1.5f));
				}
				if (amount >= this.health)
				{
					this.health = 0;
				}
				else
				{
					this.health -= amount;
				}
				this.ragdoll = newRagdoll;
				if (this.health == 0)
				{
					if (this.isMega)
					{
						kill = EPlayerKill.MEGA;
					}
					else
					{
						kill = EPlayerKill.ZOMBIE;
					}
					xp = LevelZombies.tables[(int)this.type].xp;
					if (this.zombieRegion.hasBeacon)
					{
						xp = (uint)(xp * Provider.modeConfigData.Zombies.Beacon_Experience_Multiplier);
					}
					else
					{
						if (LightingManager.isFullMoon)
						{
							xp = (uint)(xp * Provider.modeConfigData.Zombies.Full_Moon_Experience_Multiplier);
						}
						if (dropLoot)
						{
							ZombieManager.dropLoot(this);
						}
					}
					ZombieManager.sendZombieDead(this, this.ragdoll, ragdollEffect);
					if (this.isRadioactive)
					{
						List<EPlayerKill> list;
						DamageTool.explode(base.transform.position + new Vector3(0f, 0.25f, 0f), 2f, EDeathCause.ACID, CSteamID.Nil, 20f, 0f, 20f, 0f, 0f, 0f, 0f, 0f, out list, EExplosionDamageType.ZOMBIE_ACID, 2f, true, false, EDamageOrigin.Radioactive_Zombie_Explosion, ERagdollEffect.NONE);
					}
					if (this.speciality == EZombieSpeciality.BURNER || this.speciality == EZombieSpeciality.BOSS_FIRE || this.speciality == EZombieSpeciality.BOSS_MAGMA || this.speciality == EZombieSpeciality.BOSS_BUAK_FIRE)
					{
						List<EPlayerKill> list2;
						DamageTool.explode(base.transform.position + new Vector3(0f, 0.25f, 0f), 4f, EDeathCause.BURNER, CSteamID.Nil, 40f, 0f, 40f, 0f, 0f, 0f, 0f, 0f, out list2, EExplosionDamageType.ZOMBIE_FIRE, 4f, true, false, EDamageOrigin.Flamable_Zombie_Explosion, ERagdollEffect.NONE);
					}
					if (trackKill)
					{
						for (int i = 0; i < Provider.clients.Count; i++)
						{
							SteamPlayer steamPlayer = Provider.clients[i];
							if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead && (steamPlayer.player.movement.bound == this.bound || steamPlayer.player.movement.bound == 255))
							{
								steamPlayer.player.quests.trackZombieKill(this);
							}
						}
					}
				}
				else if (Provider.modeConfigData.Zombies.Can_Stun)
				{
					if (stunOverride == EZombieStunOverride.None && !Provider.modeConfigData.Zombies.Only_Critical_Stuns)
					{
						if ((int)amount > this.getStunDamageThreshold())
						{
							this.stun();
						}
					}
					else if (stunOverride == EZombieStunOverride.Always)
					{
						this.stun();
					}
				}
				this.lastRegen = Time.time;
			}
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x001A9154 File Offset: 0x001A7354
		public void sendRevive(byte type, byte speciality, byte shirt, byte pants, byte hat, byte gear, Vector3 position, float angle)
		{
			ZombieManager.sendZombieAlive(this, type, speciality, shirt, pants, hat, gear, position, MeasurementTool.angleToByte(angle));
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x001A9179 File Offset: 0x001A7379
		public bool checkAlert(Player newPlayer)
		{
			return this.player != newPlayer;
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x001A9188 File Offset: 0x001A7388
		public void alert(Player newPlayer)
		{
			if (this.isDead)
			{
				return;
			}
			if (this.player == newPlayer)
			{
				return;
			}
			if (this.player == null)
			{
				if (!this.isHunting && !this.isLeaving)
				{
					if (this.speciality == EZombieSpeciality.CRAWLER)
					{
						if (Random.value < 0.5f)
						{
							ZombieManager.sendZombieStartle(this, 3);
						}
						else
						{
							ZombieManager.sendZombieStartle(this, 6);
						}
					}
					else if (this.speciality == EZombieSpeciality.SPRINTER)
					{
						if (Random.value < 0.5f)
						{
							ZombieManager.sendZombieStartle(this, 4);
						}
						else
						{
							ZombieManager.sendZombieStartle(this, 5);
						}
					}
					else
					{
						ZombieManager.sendZombieStartle(this, (byte)Random.Range(0, 3));
					}
				}
				this.isHunting = true;
				this.huntType = EHuntType.PLAYER;
				this.isPulled = true;
				this.lastPull = Time.time;
				if (this.isWandering)
				{
					this.isWandering = false;
					ZombieManager.wanderingCount--;
				}
				this.isLeaving = false;
				this.isMoving = false;
				this.isStuck = false;
				this.lastHunted = Time.time;
				this.lastStuck = Time.time;
				this.player = newPlayer;
				this.target.position = this.player.transform.position;
				this.seeker.canSearch = true;
				this.seeker.canMove = true;
				if (this.isMega)
				{
					this.path = EZombiePath.RUSH;
				}
				else if (this.speciality == EZombieSpeciality.FLANKER_FRIENDLY || this.speciality == EZombieSpeciality.FLANKER_STALK)
				{
					if ((double)Random.value < 0.5)
					{
						this.path = EZombiePath.LEFT_FLANK;
					}
					else
					{
						this.path = EZombiePath.RIGHT_FLANK;
					}
				}
				else if (this.player.agro % 3 == 0)
				{
					this.path = EZombiePath.RUSH;
				}
				else if ((double)Random.value < 0.5)
				{
					this.path = EZombiePath.LEFT;
				}
				else
				{
					this.path = EZombiePath.RIGHT;
				}
				this.player.agro++;
				return;
			}
			if ((newPlayer.transform.position - base.transform.position).sqrMagnitude < (this.player.transform.position - base.transform.position).sqrMagnitude)
			{
				this.player.agro--;
				this.player = newPlayer;
				this.target.position = this.player.transform.position;
				if (this.isMega)
				{
					this.path = EZombiePath.RUSH;
				}
				else if (this.player.agro % 3 == 0)
				{
					this.path = EZombiePath.RUSH;
				}
				else if ((double)Random.value < 0.5)
				{
					this.path = EZombiePath.LEFT;
				}
				else
				{
					this.path = EZombiePath.RIGHT;
				}
				this.player.agro++;
			}
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x001A9440 File Offset: 0x001A7640
		public void alert(Vector3 newPosition, bool isStartling)
		{
			if (this.isDead)
			{
				return;
			}
			if (this.player == null)
			{
				if (!this.isHunting)
				{
					if (isStartling)
					{
						if (this.speciality == EZombieSpeciality.CRAWLER)
						{
							if (Random.value < 0.5f)
							{
								ZombieManager.sendZombieStartle(this, 3);
							}
							else
							{
								ZombieManager.sendZombieStartle(this, 6);
							}
						}
						else if (this.speciality == EZombieSpeciality.SPRINTER)
						{
							if (Random.value < 0.5f)
							{
								ZombieManager.sendZombieStartle(this, 4);
							}
							else
							{
								ZombieManager.sendZombieStartle(this, 5);
							}
						}
						else
						{
							ZombieManager.sendZombieStartle(this, (byte)Random.Range(0, 3));
						}
						this.isPulled = true;
						this.lastPull = Time.time;
						if (this.isWandering)
						{
							this.isWandering = false;
							ZombieManager.wanderingCount--;
						}
					}
					this.isHunting = true;
					this.huntType = EHuntType.POINT;
					this.isLeaving = false;
					this.isMoving = false;
					this.isStuck = false;
					this.lastHunted = Time.time;
					this.lastStuck = Time.time;
					this.target.position = newPosition;
					this.seeker.canSearch = true;
					this.seeker.canMove = true;
					return;
				}
				if ((newPosition - base.transform.position).sqrMagnitude < (this.target.position - base.transform.position).sqrMagnitude)
				{
					this.target.position = newPosition;
				}
			}
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x001A95AC File Offset: 0x001A77AC
		public void updateStates()
		{
			this.lastUpdatedPos = base.transform.position;
			this.lastUpdatedAngle = base.transform.rotation.eulerAngles.y;
			this.interpPositionTarget = this.lastUpdatedPos;
			this.interpYawTarget = this.lastUpdatedAngle;
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x001A9600 File Offset: 0x001A7800
		private void stop()
		{
			this.isMoving = false;
			this.isAttacking = false;
			this.isHunting = false;
			this.isStuck = false;
			this.lastStuck = Time.time;
			if (this.player != null)
			{
				this.player.agro--;
			}
			this.player = null;
			this.barricade = null;
			this.structure = null;
			this.targetObstructionVehicle = null;
			this.targetPassengerVehicle = null;
			this.seeker.canSearch = false;
			this.seeker.canMove = false;
			this.target.position = base.transform.position;
			this.seeker.stop();
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x001A96B4 File Offset: 0x001A78B4
		private void stun()
		{
			this.isStunned = true;
			this.isMoving = false;
			this.seeker.canMove = false;
			if (this.speciality == EZombieSpeciality.CRAWLER)
			{
				float value = Random.value;
				if (value < 0.33f)
				{
					ZombieManager.sendZombieStun(this, 5);
					return;
				}
				if (value < 0.66f)
				{
					ZombieManager.sendZombieStun(this, 7);
					return;
				}
				ZombieManager.sendZombieStun(this, 8);
				return;
			}
			else
			{
				if (this.speciality != EZombieSpeciality.SPRINTER)
				{
					ZombieManager.sendZombieStun(this, (byte)Random.Range(0, 5));
					return;
				}
				float value2 = Random.value;
				if (value2 < 0.33f)
				{
					ZombieManager.sendZombieStun(this, 6);
					return;
				}
				if (value2 < 0.66f)
				{
					ZombieManager.sendZombieStun(this, 9);
					return;
				}
				ZombieManager.sendZombieStun(this, 10);
				return;
			}
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x001A975C File Offset: 0x001A795C
		private void leave(bool quick)
		{
			this.isLeaving = true;
			this.lastLeave = Time.time;
			if (quick)
			{
				this.leaveTime = Random.Range(0.5f, 1f);
			}
			else
			{
				this.leaveTime = Random.Range(3f, 6f);
			}
			this.leaveTo = base.transform.position - 16f * (this.target.position - base.transform.position).normalized + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
			if (!LevelNavigation.checkNavigation(this.leaveTo))
			{
				this.leaveTo = base.transform.position + 16f * (this.target.position - base.transform.position).normalized + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
			}
			if (!LevelNavigation.checkNavigation(this.leaveTo))
			{
				this.leaveTo = base.transform.position;
			}
			this.stop();
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x001A98BC File Offset: 0x001A7ABC
		private void updateEffects()
		{
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x001A98CC File Offset: 0x001A7ACC
		public float getBulletResistance()
		{
			EZombieSpeciality ezombieSpeciality = this.speciality;
			if (ezombieSpeciality - EZombieSpeciality.SPIRIT <= 1 || ezombieSpeciality == EZombieSpeciality.BOSS_ELVER_STOMPER)
			{
				return 0.1f;
			}
			return 1f;
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x001A98F8 File Offset: 0x001A7AF8
		private void updateVisibility(bool newVisible, bool playEffect)
		{
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x001A9908 File Offset: 0x001A7B08
		private void apply()
		{
			if (this.isMega)
			{
				this.SetCapsuleRadiusAndHeight(0.75f, 2f);
				if (Provider.isServer)
				{
					this.seeker.speed = 6f;
					return;
				}
			}
			else
			{
				this.SetCapsuleRadiusAndHeight(0.4f, 2f);
				if (Provider.isServer)
				{
					if (this.speciality == EZombieSpeciality.CRAWLER)
					{
						if (Provider.modeConfigData.Zombies.Slow_Movement)
						{
							this.seeker.speed = 2.5f;
							return;
						}
						this.seeker.speed = 3f;
						return;
					}
					else if (this.speciality == EZombieSpeciality.SPRINTER || this.speciality.IsDLVolatile())
					{
						if (Provider.modeConfigData.Zombies.Slow_Movement)
						{
							this.seeker.speed = 6f;
							return;
						}
						this.seeker.speed = 6.5f;
						return;
					}
					else if (this.speciality == EZombieSpeciality.FLANKER_FRIENDLY || this.speciality == EZombieSpeciality.FLANKER_STALK)
					{
						if (Provider.modeConfigData.Zombies.Slow_Movement)
						{
							this.seeker.speed = 5.5f;
							return;
						}
						this.seeker.speed = 6f;
						return;
					}
					else
					{
						if (Provider.modeConfigData.Zombies.Slow_Movement)
						{
							this.seeker.speed = 4.5f;
							return;
						}
						this.seeker.speed = 5.5f;
					}
				}
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06004743 RID: 18243 RVA: 0x001A9A5E File Offset: 0x001A7C5E
		// (set) Token: 0x06004744 RID: 18244 RVA: 0x001A9A66 File Offset: 0x001A7C66
		public ZombieDifficultyAsset difficulty { get; private set; }

		/// <summary>
		/// Cache difficulty asset (if any) for this zombie's current type and bound.
		/// Allows difficulty assets to override certain zombie behaviors.
		/// Called after bound/type is initialized, and after type changes during respawn.
		/// </summary>
		// Token: 0x06004745 RID: 18245 RVA: 0x001A9A70 File Offset: 0x001A7C70
		private void updateDifficulty()
		{
			if (!Provider.isServer)
			{
				return;
			}
			this.difficulty = ZombieManager.getDifficultyInBound(this.bound);
			if (this.difficulty == null && (int)this.type < LevelZombies.tables.Count)
			{
				this.difficulty = LevelZombies.tables[(int)this.type].resolveDifficulty();
			}
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x001A9ACC File Offset: 0x001A7CCC
		private void updateLife()
		{
			CharacterController component = base.GetComponent<CharacterController>();
			if (component != null)
			{
				component.SetDetectCollisionsDeferred(!this.isDead);
			}
			base.GetComponent<Collider>().enabled = !this.isDead;
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x001A9B0C File Offset: 0x001A7D0C
		private void reset()
		{
			this.target.position = base.transform.position;
			this.lastTarget = Time.time;
			this.lastLeave = Time.time;
			this.lastSpecial = Time.time;
			this.lastAttack = Time.time;
			this.lastStartle = Time.time;
			this.lastStun = Time.time;
			this.lastStuck = Time.time;
			this.cameFrom = base.transform.position;
			this.isPulled = false;
			this.pullDelay = Random.Range(24f, 96f);
			this.specialStartleDelay = Random.Range(1f, 2f);
			this.specialAttackDelay = Random.Range(2f, 4f);
			this.specialUseDelay = Random.Range(4f, 8f);
			this.flashbangDelay = 10f;
			this.isThrowingBoulder = false;
			this.isSpittingAcid = false;
			this.isChargingSpark = false;
			this.isStompingWind = false;
			this.isBreathingFire = false;
			this.isPlayingBoulder = false;
			this.isPlayingSpit = false;
			this.isPlayingCharge = false;
			this.isPlayingWind = false;
			this.isPlayingFire = false;
			this.isPlayingAttack = false;
			this.isPlayingStartle = false;
			this.isPlayingStun = false;
			this.isMoving = false;
			this.isAttacking = false;
			this.isHunting = false;
			this.isLeaving = false;
			this.isStunned = false;
			this.isStuck = false;
			this.leaveTo = base.transform.position;
			if (this.player != null)
			{
				this.player.agro--;
			}
			this.player = null;
			this.barricade = null;
			this.structure = null;
			this.targetObstructionVehicle = null;
			this.targetPassengerVehicle = null;
			this.seeker.canSearch = false;
			this.seeker.canMove = false;
			this.health = LevelZombies.tables[(int)this.type].health;
			if (this.speciality == EZombieSpeciality.CRAWLER || this.speciality.IsDLVolatile())
			{
				this.health = (ushort)((float)this.health * 1.5f);
			}
			else if (this.speciality == EZombieSpeciality.SPRINTER)
			{
				this.health = (ushort)((float)this.health * 0.5f);
			}
			else if (this.speciality == EZombieSpeciality.BOSS_ALL || this.speciality == EZombieSpeciality.BOSS_MAGMA)
			{
				this.health = 12000;
			}
			else if (this.speciality == EZombieSpeciality.BOSS_ELVER_STOMPER)
			{
				this.health = 4600;
			}
			else if (this.speciality == EZombieSpeciality.BOSS_KUWAIT)
			{
				this.health = 60000;
			}
			else if (this.speciality == EZombieSpeciality.BOSS_BUAK_WIND)
			{
				this.health = 6000;
			}
			else if (this.speciality == EZombieSpeciality.BOSS_BUAK_FIRE)
			{
				this.health = 6200;
			}
			else if (this.speciality == EZombieSpeciality.BOSS_BUAK_ELECTRIC)
			{
				this.health = 6400;
			}
			else if (this.speciality == EZombieSpeciality.BOSS_BUAK_FINAL)
			{
				this.health = 7000;
			}
			else if (this.isBoss)
			{
				this.health = 6000;
			}
			if (Level.info.type == ELevelType.HORDE)
			{
				this.health += (ushort)(Mathf.Min(ZombieManager.waveIndex - 1, 20) * 10);
			}
			this.maxHealth = this.health;
			this.needsTickForPlacement = true;
			this.setTicking(true);
		}

		/// <summary>
		/// Called when zombie does not have a target, but has been stuck for a period.
		/// </summary>
		// Token: 0x06004748 RID: 18248 RVA: 0x001A9E5C File Offset: 0x001A805C
		private void findTargetWhileStuck()
		{
			bool can_Target_Structures = Provider.modeConfigData.Zombies.Can_Target_Structures;
			bool can_Target_Barricades = Provider.modeConfigData.Zombies.Can_Target_Barricades;
			bool flag = Provider.modeConfigData.Zombies.Can_Target_Vehicles;
			flag &= (this.speciality != EZombieSpeciality.BOSS_KUWAIT);
			if (can_Target_Structures || can_Target_Barricades)
			{
				Zombie.regionsInRadius.Clear();
				Regions.getRegionsInRadius(base.transform.position, 4f, Zombie.regionsInRadius);
			}
			if (can_Target_Structures)
			{
				Zombie.structuresInRadius.Clear();
				StructureManager.getStructuresInRadius(base.transform.position, 16f, Zombie.regionsInRadius, Zombie.structuresInRadius);
				if (Zombie.structuresInRadius.Count > 0)
				{
					this.structure = Zombie.structuresInRadius[0];
					return;
				}
			}
			if (flag)
			{
				Zombie.vehiclesInRadius.Clear();
				VehicleManager.getVehiclesInRadius(base.transform.position, 16f, Zombie.vehiclesInRadius);
				if (Zombie.vehiclesInRadius.Count > 0 && Zombie.vehiclesInRadius[0].asset != null && Zombie.vehiclesInRadius[0].asset.isVulnerableToEnvironment)
				{
					this.targetObstructionVehicle = Zombie.vehiclesInRadius[0];
					return;
				}
			}
			if (can_Target_Barricades)
			{
				Zombie.barricadesInRadius.Clear();
				BarricadeManager.getBarricadesInRadius(base.transform.position, 16f, Zombie.regionsInRadius, Zombie.barricadesInRadius);
				if (Zombie.barricadesInRadius.Count > 0)
				{
					this.barricade = Zombie.barricadesInRadius[0];
					return;
				}
			}
		}

		// Token: 0x06004749 RID: 18249 RVA: 0x001A9FD8 File Offset: 0x001A81D8
		public void tick()
		{
			if (this.needsTickForPlacement)
			{
				this.needsTickForPlacement = false;
				this.setTicking(false);
				base.GetComponent<CharacterController>().Move(Vector3.down);
				return;
			}
			float num = Time.time - this.lastTick;
			this.lastTick = Time.time;
			this.lastPull = Time.time;
			if (this.isStunned)
			{
				return;
			}
			this.undergroundTestTimer -= num;
			if (this.undergroundTestTimer < 0f)
			{
				this.undergroundTestTimer = Random.Range(30f, 60f);
				if (!UndergroundAllowlist.IsPositionWithinValidHeight(base.transform.position, 0.1f))
				{
					ZombieManager.teleportZombieBackIntoMap(this);
					return;
				}
			}
			if (this.huntType == EHuntType.PLAYER)
			{
				if (this.player == null)
				{
					this.stop();
					return;
				}
			}
			else if (this.huntType == EHuntType.POINT && !this.isMoving && Time.time - this.lastHunted > 3f)
			{
				this.stop();
				return;
			}
			if (this.player != null)
			{
				if (this.player.life.isDead)
				{
					this.leave(false);
					return;
				}
				if (this.player.movement.nav == 255 || (this.player.stance.stance == EPlayerStance.SWIM && !WaterUtility.isPointUnderwater(base.transform.position)))
				{
					this.leave(true);
					return;
				}
			}
			if (this.targetObstructionVehicle != null && this.targetObstructionVehicle.isDead)
			{
				this.targetObstructionVehicle = null;
			}
			if (this.targetPassengerVehicle != null && this.targetPassengerVehicle.isDead)
			{
				this.targetPassengerVehicle = null;
			}
			if (this.isStuck)
			{
				float num2 = Time.time - this.lastStuck;
				if (num2 > 1f && this.barricade == null && this.structure == null && this.targetObstructionVehicle == null && this.targetPassengerVehicle == null)
				{
					this.findTargetWhileStuck();
				}
				if (num2 > 5f && this.zombieRegion.hasBeacon && Time.time - this.lastAttack > 10f)
				{
					this.lastStuck = Time.time;
					ZombieManager.teleportZombieBackIntoMap(this);
					return;
				}
			}
			float num3;
			float num4;
			if (this.barricade != null)
			{
				num3 = MathfEx.HorizontalDistanceSquared(this.barricade.position, base.transform.position);
				num4 = Mathf.Abs(this.barricade.position.y - base.transform.position.y);
				this.target.position = this.barricade.position;
				this.seeker.canTurn = false;
				this.seeker.targetDirection = this.barricade.position - base.transform.position;
			}
			else if (this.structure != null)
			{
				num3 = 0f;
				num4 = 0f;
				this.target.position = base.transform.position;
				this.seeker.canTurn = false;
				this.seeker.targetDirection = this.structure.position - base.transform.position;
			}
			else if (this.targetObstructionVehicle != null)
			{
				num3 = MathfEx.HorizontalDistanceSquared(this.targetObstructionVehicle.transform.position, base.transform.position);
				num4 = Mathf.Abs(this.targetObstructionVehicle.transform.position.y - base.transform.position.y);
				this.target.position = this.targetObstructionVehicle.transform.position;
				this.seeker.canTurn = false;
				this.seeker.targetDirection = this.targetObstructionVehicle.transform.position - base.transform.position;
			}
			else if (this.player != null)
			{
				this.targetPassengerVehicle = ((this.speciality != EZombieSpeciality.BOSS_KUWAIT) ? this.player.movement.getVehicle() : null);
				if (this.targetPassengerVehicle != null)
				{
					if (this.targetPassengerVehicle.isDead)
					{
						this.targetPassengerVehicle = null;
					}
					else if (this.targetPassengerVehicle.asset == null || !this.targetPassengerVehicle.asset.isVulnerableToEnvironment)
					{
						this.targetPassengerVehicle = null;
					}
				}
				if (this.targetPassengerVehicle != null)
				{
					num3 = MathfEx.HorizontalDistanceSquared(this.targetPassengerVehicle.transform.position, base.transform.position);
					num4 = Mathf.Abs(this.targetPassengerVehicle.transform.position.y - base.transform.position.y);
					this.target.position = this.targetPassengerVehicle.transform.position;
					this.seeker.canTurn = false;
					this.seeker.targetDirection = this.targetPassengerVehicle.transform.position - base.transform.position;
				}
				else
				{
					num3 = MathfEx.HorizontalDistanceSquared(this.player.transform.position, base.transform.position);
					num4 = Mathf.Abs(this.player.transform.position.y - base.transform.position.y);
					this.target.position = this.player.transform.position;
					if (this.path == EZombiePath.LEFT_FLANK)
					{
						if (num3 > 100f)
						{
							this.seeker.canTurn = true;
							this.target.position += this.player.transform.right * 9f + this.player.transform.forward * -4f;
						}
						else if (num3 > 20f || Vector3.Dot((base.transform.position - this.player.transform.position).normalized, this.player.transform.forward) > 0f)
						{
							this.seeker.canTurn = true;
							this.target.position += this.player.transform.right * 3f + this.player.transform.forward * -3f;
						}
						else if (num3 > 4f)
						{
							this.seeker.canTurn = true;
							this.target.position -= this.player.transform.forward;
						}
						else
						{
							this.seeker.canTurn = false;
							this.seeker.targetDirection = this.player.transform.position - base.transform.position;
						}
					}
					else if (this.path == EZombiePath.RIGHT_FLANK)
					{
						if (num3 > 100f)
						{
							this.seeker.canTurn = true;
							this.target.position += this.player.transform.right * -9f + this.player.transform.forward * -4f;
						}
						else if (num3 > 20f || Vector3.Dot((base.transform.position - this.player.transform.position).normalized, this.player.transform.forward) > 0f)
						{
							this.seeker.canTurn = true;
							this.target.position += this.player.transform.right * -3f + this.player.transform.forward * -3f;
						}
						else if (num3 > 4f)
						{
							this.seeker.canTurn = true;
							this.target.position -= this.player.transform.forward;
						}
						else
						{
							this.seeker.canTurn = false;
							this.seeker.targetDirection = this.player.transform.position - base.transform.position;
						}
					}
					else if (this.path == EZombiePath.LEFT)
					{
						if (num3 > 4f)
						{
							this.seeker.canTurn = true;
							this.target.position -= base.transform.right;
						}
						else
						{
							this.seeker.canTurn = false;
							this.seeker.targetDirection = this.player.transform.position - base.transform.position;
						}
					}
					else if (this.path == EZombiePath.RIGHT)
					{
						if (num3 > 4f)
						{
							this.seeker.canTurn = true;
							this.target.position += base.transform.right;
						}
						else
						{
							this.seeker.canTurn = false;
							this.seeker.targetDirection = this.player.transform.position - base.transform.position;
						}
					}
					else if (this.path == EZombiePath.RUSH)
					{
						if (num3 > 4f)
						{
							this.seeker.canTurn = true;
							this.target.position -= base.transform.forward;
						}
						else
						{
							this.seeker.canTurn = false;
							this.seeker.targetDirection = this.player.transform.position - base.transform.position;
						}
					}
				}
			}
			else
			{
				num3 = MathfEx.HorizontalDistanceSquared(this.target.position, base.transform.position);
				num4 = Mathf.Abs(this.target.position.y - base.transform.position.y);
				this.seeker.canTurn = true;
			}
			this.isMoving = (num3 > 3f);
			if (!this.isWandering && num3 > 4096f && (this.player == null || !this.zombieRegion.HasInfiniteAgroRange))
			{
				this.leave(false);
				return;
			}
			if (this.player != null || this.barricade != null || this.structure != null || this.targetObstructionVehicle != null || this.targetPassengerVehicle != null)
			{
				if (this.player != null && Time.time - this.lastStartle > this.specialStartleDelay && Time.time - this.lastAttack > this.specialAttackDelay && Time.time - this.lastSpecial > this.specialUseDelay)
				{
					Zombie.availableAbilityChoices.Clear();
					if ((this.speciality == EZombieSpeciality.MEGA || this.speciality == EZombieSpeciality.BOSS_KUWAIT || this.speciality == EZombieSpeciality.BOSS_ALL || this.speciality == EZombieSpeciality.BOSS_BUAK_FINAL) && num3 > 20f)
					{
						Zombie.availableAbilityChoices.Add(Zombie.EAbilityChoice.ThrowBoulder);
					}
					if (this.speciality == EZombieSpeciality.ACID || this.speciality == EZombieSpeciality.BOSS_NUCLEAR || this.speciality == EZombieSpeciality.BOSS_ALL || this.speciality == EZombieSpeciality.BOSS_BUAK_FINAL)
					{
						Zombie.availableAbilityChoices.Add(Zombie.EAbilityChoice.SpitAcid);
					}
					if ((this.speciality == EZombieSpeciality.BOSS_WIND || this.speciality == EZombieSpeciality.BOSS_BUAK_WIND || this.speciality == EZombieSpeciality.BOSS_ELVER_STOMPER || this.speciality == EZombieSpeciality.BOSS_ALL || this.speciality == EZombieSpeciality.BOSS_BUAK_FINAL) && num3 < 144f)
					{
						Zombie.availableAbilityChoices.Add(Zombie.EAbilityChoice.Stomp);
					}
					if ((this.speciality == EZombieSpeciality.BOSS_FIRE || this.speciality == EZombieSpeciality.BOSS_MAGMA || this.speciality == EZombieSpeciality.BOSS_BUAK_FIRE || this.speciality == EZombieSpeciality.BOSS_ALL || this.speciality == EZombieSpeciality.BOSS_BUAK_FINAL) && num3 < 529f)
					{
						Zombie.availableAbilityChoices.Add(Zombie.EAbilityChoice.BreatheFire);
					}
					if ((this.speciality == EZombieSpeciality.BOSS_ELECTRIC || this.speciality == EZombieSpeciality.BOSS_BUAK_ELECTRIC || this.speciality == EZombieSpeciality.BOSS_ALL || this.speciality == EZombieSpeciality.BOSS_BUAK_FINAL) && num3 > 4f && num3 < 4096f)
					{
						Zombie.availableAbilityChoices.Add(Zombie.EAbilityChoice.ElectricShock);
					}
					if ((this.speciality == EZombieSpeciality.BOSS_KUWAIT || this.speciality.IsFromBuakMap()) && Time.time - this.lastFlashbang > this.flashbangDelay && num3 > 4f && num3 < 1024f)
					{
						Zombie.availableAbilityChoices.Add(Zombie.EAbilityChoice.Flashbang);
					}
					if (Zombie.availableAbilityChoices.Count > 0)
					{
						this.lastSpecial = Time.time;
						Zombie.EAbilityChoice eabilityChoice = Zombie.availableAbilityChoices.RandomOrDefault<Zombie.EAbilityChoice>();
						if (eabilityChoice == Zombie.EAbilityChoice.ThrowBoulder)
						{
							this.specialUseDelay = Random.Range(6f, 12f);
							if (this.speciality == EZombieSpeciality.BOSS_KUWAIT || this.speciality == EZombieSpeciality.BOSS_BUAK_FINAL)
							{
								this.specialUseDelay *= 0.5f;
							}
							this.seeker.canMove = false;
							ZombieManager.sendZombieThrow(this);
						}
						else if (eabilityChoice == Zombie.EAbilityChoice.SpitAcid)
						{
							this.specialUseDelay = Random.Range(4f, 8f);
							this.seeker.canMove = false;
							ZombieManager.sendZombieSpit(this);
						}
						else if (eabilityChoice == Zombie.EAbilityChoice.Stomp)
						{
							this.specialUseDelay = Random.Range(4f, 8f);
							this.seeker.canMove = false;
							ZombieManager.sendZombieStomp(this);
						}
						else if (eabilityChoice == Zombie.EAbilityChoice.BreatheFire)
						{
							this.specialUseDelay = Random.Range(4f, 8f);
							this.seeker.canMove = false;
							ZombieManager.sendZombieBreath(this);
						}
						else if (eabilityChoice == Zombie.EAbilityChoice.ElectricShock)
						{
							this.specialUseDelay = Random.Range(4f, 8f);
							this.seeker.canMove = false;
							ZombieManager.sendZombieCharge(this);
						}
						else if (eabilityChoice == Zombie.EAbilityChoice.Flashbang)
						{
							this.specialUseDelay = Random.Range(1f, 2f);
							this.lastFlashbang = Time.time;
							this.flashbangDelay = Random.Range(30f, 45f);
							EffectAsset effectAsset = (this.speciality == EZombieSpeciality.BOSS_KUWAIT) ? Zombie.KuwaitBossFlashbangRef.Find() : Zombie.BuakBossFlashbangRef.Find();
							if (effectAsset != null)
							{
								EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
								{
									reliable = true,
									position = base.transform.position + new Vector3(0f, 5f, 0f)
								});
							}
							else
							{
								UnturnedLog.warn("Missing built-in zombie flashbang effect");
							}
						}
					}
				}
				if ((this.structure != null || num3 < this.GetHorizontalAttackRangeSquared()) && num4 < this.GetVerticalAttackRange())
				{
					if (this.speciality == EZombieSpeciality.SPRINTER || Time.time - this.lastTarget > 0.5f)
					{
						if (this.isAttacking)
						{
							if (Time.time - this.lastAttack > this.attackTime / 2f)
							{
								this.isAttacking = false;
								byte b = (byte)((float)LevelZombies.tables[(int)this.type].damage * (this.isHyper ? 1.5f : 1f));
								b = (byte)((float)b * Provider.modeConfigData.Zombies.Damage_Multiplier);
								if (this.speciality == EZombieSpeciality.CRAWLER)
								{
									b = (byte)((float)b * 2f);
								}
								else if (this.speciality == EZombieSpeciality.SPRINTER)
								{
									b = (byte)((float)b * 0.75f);
								}
								if (this.structure != null)
								{
									StructureManager.damage(this.structure, (this.target.position - base.transform.position).normalized * (float)b, (float)b, 1f, true, default(CSteamID), EDamageOrigin.Zombie_Swipe);
									if (this.structure == null || !this.structure.CompareTag("Structure"))
									{
										this.structure = null;
										this.isStuck = false;
										this.lastStuck = Time.time;
									}
								}
								else if (this.barricade != null)
								{
									BarricadeManager.damage(this.barricade, (float)b, 1f, true, default(CSteamID), EDamageOrigin.Zombie_Swipe);
								}
								else if (this.targetObstructionVehicle != null)
								{
									VehicleManager.damage(this.targetObstructionVehicle, (float)b, 1f, true, default(CSteamID), EDamageOrigin.Zombie_Swipe);
								}
								else if (this.targetPassengerVehicle != null)
								{
									VehicleManager.damage(this.targetPassengerVehicle, (float)b, 1f, true, default(CSteamID), EDamageOrigin.Zombie_Swipe);
								}
								else if (this.player != null)
								{
									if (this.player.skills.boost == EPlayerBoost.HARDENED)
									{
										b = (byte)((float)b * 0.75f);
									}
									if (this.isMega)
									{
										if (this.player.clothing.hatAsset != null)
										{
											ItemClothingAsset hatAsset = this.player.clothing.hatAsset;
											if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && this.player.clothing.hatQuality > 0)
											{
												PlayerClothing clothing = this.player.clothing;
												clothing.hatQuality -= 1;
												this.player.clothing.sendUpdateHatQuality();
											}
											float num5 = hatAsset.armor + (1f - hatAsset.armor) * (1f - (float)this.player.clothing.hatQuality / 100f);
											b = (byte)((float)b * num5);
										}
										else if (this.player.clothing.vestAsset != null)
										{
											ItemClothingAsset vestAsset = this.player.clothing.vestAsset;
											if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && this.player.clothing.vestQuality > 0)
											{
												PlayerClothing clothing2 = this.player.clothing;
												clothing2.vestQuality -= 1;
												this.player.clothing.sendUpdateVestQuality();
											}
											float num6 = vestAsset.armor + (1f - vestAsset.armor) * (1f - (float)this.player.clothing.vestQuality / 100f);
											b = (byte)((float)b * num6);
										}
										else if (this.player.clothing.shirtAsset != null)
										{
											ItemClothingAsset shirtAsset = this.player.clothing.shirtAsset;
											if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && this.player.clothing.shirtQuality > 0)
											{
												PlayerClothing clothing3 = this.player.clothing;
												clothing3.shirtQuality -= 1;
												this.player.clothing.sendUpdateShirtQuality();
											}
											float num7 = shirtAsset.armor + (1f - shirtAsset.armor) * (1f - (float)this.player.clothing.shirtQuality / 100f);
											b = (byte)((float)b * num7);
										}
									}
									else if (this.speciality == EZombieSpeciality.NORMAL)
									{
										if (this.player.clothing.vestAsset != null)
										{
											ItemClothingAsset vestAsset2 = this.player.clothing.vestAsset;
											if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && this.player.clothing.vestQuality > 0)
											{
												PlayerClothing clothing4 = this.player.clothing;
												clothing4.vestQuality -= 1;
												this.player.clothing.sendUpdateVestQuality();
											}
											float num8 = vestAsset2.armor + (1f - vestAsset2.armor) * (1f - (float)this.player.clothing.vestQuality / 100f);
											b = (byte)((float)b * num8);
										}
										else if (this.player.clothing.shirtAsset != null)
										{
											ItemClothingAsset shirtAsset2 = this.player.clothing.shirtAsset;
											if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && this.player.clothing.shirtQuality > 0)
											{
												PlayerClothing clothing5 = this.player.clothing;
												clothing5.shirtQuality -= 1;
												this.player.clothing.sendUpdateShirtQuality();
											}
											float num9 = shirtAsset2.armor + (1f - shirtAsset2.armor) * (1f - (float)this.player.clothing.shirtQuality / 100f);
											b = (byte)((float)b * num9);
										}
									}
									else if (this.speciality == EZombieSpeciality.CRAWLER)
									{
										if (this.player.clothing.pantsAsset != null)
										{
											ItemClothingAsset pantsAsset = this.player.clothing.pantsAsset;
											if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && this.player.clothing.pantsQuality > 0)
											{
												PlayerClothing clothing6 = this.player.clothing;
												clothing6.pantsQuality -= 1;
												this.player.clothing.sendUpdatePantsQuality();
											}
											float num10 = pantsAsset.armor + (1f - pantsAsset.armor) * (1f - (float)this.player.clothing.pantsQuality / 100f);
											b = (byte)((float)b * num10);
										}
									}
									else if (this.speciality == EZombieSpeciality.SPRINTER)
									{
										if (this.player.clothing.vestAsset != null)
										{
											ItemClothingAsset vestAsset3 = this.player.clothing.vestAsset;
											if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && this.player.clothing.vestQuality > 0)
											{
												PlayerClothing clothing7 = this.player.clothing;
												clothing7.vestQuality -= 1;
												this.player.clothing.sendUpdateVestQuality();
											}
											float num11 = vestAsset3.armor + (1f - vestAsset3.armor) * (1f - (float)this.player.clothing.vestQuality / 100f);
											b = (byte)((float)b * num11);
										}
										else if (this.player.clothing.shirtAsset != null)
										{
											ItemClothingAsset shirtAsset3 = this.player.clothing.shirtAsset;
											if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && this.player.clothing.shirtQuality > 0)
											{
												PlayerClothing clothing8 = this.player.clothing;
												clothing8.shirtQuality -= 1;
												this.player.clothing.sendUpdateShirtQuality();
											}
											float num12 = shirtAsset3.armor + (1f - shirtAsset3.armor) * (1f - (float)this.player.clothing.shirtQuality / 100f);
											b = (byte)((float)b * num12);
										}
										else if (this.player.clothing.pantsAsset != null)
										{
											ItemClothingAsset pantsAsset2 = this.player.clothing.pantsAsset;
											if (Provider.modeConfigData.Items.ShouldClothingTakeDamage && this.player.clothing.pantsQuality > 0)
											{
												PlayerClothing clothing9 = this.player.clothing;
												clothing9.pantsQuality -= 1;
												this.player.clothing.sendUpdatePantsQuality();
											}
											float num13 = pantsAsset2.armor + (1f - pantsAsset2.armor) * (1f - (float)this.player.clothing.pantsQuality / 100f);
											b = (byte)((float)b * num13);
										}
									}
									EPlayerKill eplayerKill;
									DamageTool.damage(this.player, EDeathCause.ZOMBIE, ELimb.SKULL, Provider.server, (this.target.position - base.transform.position).normalized, (float)b, 1f, out eplayerKill, true, false, ERagdollEffect.NONE);
									this.player.life.askInfect((byte)((float)(b / 3) * (1f - this.player.skills.mastery(1, 2) * 0.5f)));
								}
							}
						}
						else if (Time.time - this.lastAttack > 1f)
						{
							this.isAttacking = true;
							if (this.speciality == EZombieSpeciality.CRAWLER)
							{
								ZombieManager.sendZombieAttack(this, 5);
							}
							else if (this.speciality == EZombieSpeciality.SPRINTER)
							{
								ZombieManager.sendZombieAttack(this, (byte)Random.Range(6, 9));
							}
							else
							{
								ZombieManager.sendZombieAttack(this, (byte)Random.Range(0, 5));
							}
						}
					}
				}
				else
				{
					this.lastTarget = Time.time;
					this.isAttacking = false;
				}
			}
			if (this.seeker != null)
			{
				this.seeker.move(num);
			}
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x001AB8B0 File Offset: 0x001A9AB0
		private void Update()
		{
			if (this.isDead)
			{
				return;
			}
			if (Provider.isServer)
			{
				if (!this.isUpdated)
				{
					if (Mathf.Abs(this.lastUpdatedPos.x - base.transform.position.x) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.y - base.transform.position.y) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedPos.z - base.transform.position.z) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatedAngle - base.transform.rotation.eulerAngles.y) > 1f)
					{
						this.lastUpdatedPos = base.transform.position;
						this.lastUpdatedAngle = base.transform.rotation.eulerAngles.y;
						this.isUpdated = true;
						ZombieRegion zombieRegion = this.zombieRegion;
						zombieRegion.updates += 1;
						this.isStuck = false;
						this.lastStuck = Time.time;
					}
					else if (!this.isStuck)
					{
						if (this.isMoving)
						{
							this.isStuck = true;
						}
						else if (this.zombieRegion.HasInfiniteAgroRange && this.player != null && (this.player.transform.position - base.transform.position).sqrMagnitude > 4f)
						{
							this.isStuck = true;
						}
					}
				}
				if (this.isPulled && Time.time - this.lastPull > this.pullDelay)
				{
					this.lastPull = Time.time;
					this.pullDelay = Random.Range(24f, 96f);
					if (!this.isLeaving && ZombieManager.canSpareWanderer)
					{
						float f = Random.value * 3.1415927f * 2f;
						float num = Random.Range(0.5f, 1f);
						this.isWandering = true;
						ZombieManager.wanderingCount++;
						this.isPulled = false;
						this.alert(this.cameFrom + new Vector3(Mathf.Cos(f) * num, 0f, Mathf.Sin(f) * num), false);
					}
				}
			}
			else
			{
				if (Mathf.Abs(this.lastUpdatedPos.x - base.transform.position.x) > 0.01f || Mathf.Abs(this.lastUpdatedPos.y - base.transform.position.y) > 0.01f || Mathf.Abs(this.lastUpdatedPos.z - base.transform.position.z) > 0.01f)
				{
					this.isMoving = true;
				}
				else
				{
					this.isMoving = false;
				}
				base.transform.position = Vector3.Lerp(base.transform.position, this.interpPositionTarget, Time.deltaTime * 10f);
				base.transform.rotation = Quaternion.Euler(0f, Mathf.LerpAngle(base.transform.rotation.eulerAngles.y, this.interpYawTarget, Time.deltaTime * 10f), 0f);
			}
			if ((this.isThrowingBoulder || this.isSpittingAcid || this.isBreathingFire || this.isChargingSpark) && Provider.isServer && this.player != null)
			{
				Vector3 normalized = (this.player.transform.position - base.transform.position).normalized;
				normalized.y = 0f;
				Quaternion rotation = Quaternion.LookRotation(normalized);
				base.transform.rotation = rotation;
			}
			if (this.isThrowingBoulder && Time.time - this.lastSpecial > this.throwTime)
			{
				this.isThrowingBoulder = false;
				if (this.boulderItem != null)
				{
					Object.Destroy(this.boulderItem.gameObject);
				}
				if (Provider.isServer)
				{
					this.seeker.canMove = true;
					if (this.player != null)
					{
						Vector3 a = this.player.transform.position - base.transform.position;
						float magnitude = a.magnitude;
						a += Vector3.up * magnitude * 0.1f;
						float magnitude2 = this.player.movement.velocity.magnitude;
						if (magnitude2 > 0.1f)
						{
							Vector3 a2 = this.player.movement.velocity / magnitude2;
							a += a2 * magnitude * Random.Range(0.1f, 0.2f);
						}
						Vector3 direction = a / magnitude;
						ZombieManager.sendZombieBoulder(this, base.transform.position + Vector3.up * base.transform.localScale.y * 1.9f, direction);
					}
					else
					{
						ZombieManager.sendZombieBoulder(this, base.transform.position + Vector3.up * base.transform.localScale.y * 1.9f, Vector3.forward);
					}
				}
			}
			if (this.isSpittingAcid && Time.time - this.lastSpecial > this.acidTime)
			{
				this.isSpittingAcid = false;
				if (Provider.isServer)
				{
					this.seeker.canMove = true;
					if (this.player != null)
					{
						Vector3 a3 = this.player.transform.position - base.transform.position;
						float magnitude3 = a3.magnitude;
						a3 += Vector3.up * magnitude3 * 0.25f;
						ZombieManager.sendZombieAcid(this, base.transform.position + Vector3.up * base.transform.localScale.y * 1.75f, a3.normalized);
					}
					else
					{
						ZombieManager.sendZombieAcid(this, base.transform.position + Vector3.up * base.transform.localScale.y * 1.75f, Vector3.forward);
					}
				}
			}
			if (this.isChargingSpark && Time.time - this.lastSpecial > this.sparkTime)
			{
				this.isChargingSpark = false;
				if (Provider.isServer && this.player != null)
				{
					Vector3 vector = this.player.look.aim.position;
					Vector3 direction2 = vector - (base.transform.position + new Vector3(0f, 2f, 0f));
					RaycastHit raycastHit;
					if (Physics.Raycast(new Ray(base.transform.position + new Vector3(0f, 2f, 0f), direction2), out raycastHit, direction2.magnitude - 0.025f, RayMasks.BLOCK_SENTRY))
					{
						vector = raycastHit.point + raycastHit.normal;
					}
					float barricadeDamage = Provider.modeConfigData.Zombies.Can_Target_Barricades ? 250f : 0f;
					float structureDamage = Provider.modeConfigData.Zombies.Can_Target_Structures ? 250f : 0f;
					float vehicleDamage = Provider.modeConfigData.Zombies.Can_Target_Vehicles ? 250f : 0f;
					List<EPlayerKill> list;
					DamageTool.explode(vector, 5f, EDeathCause.SPARK, CSteamID.Nil, 25f, 0f, 0f, barricadeDamage, structureDamage, vehicleDamage, 250f, 250f, out list, EExplosionDamageType.ZOMBIE_ELECTRIC, 4f, true, false, EDamageOrigin.Zombie_Electric_Shock, ERagdollEffect.NONE);
					ZombieManager.sendZombieSpark(this, vector);
				}
			}
			if (this.isStompingWind && Time.time - this.lastSpecial > this.windTime)
			{
				this.isStompingWind = false;
				if (Provider.isServer)
				{
					this.seeker.canMove = true;
					float barricadeDamage2 = Provider.modeConfigData.Zombies.Can_Target_Barricades ? 500f : 0f;
					float structureDamage2 = Provider.modeConfigData.Zombies.Can_Target_Structures ? 500f : 0f;
					float vehicleDamage2 = Provider.modeConfigData.Zombies.Can_Target_Vehicles ? 500f : 0f;
					List<EPlayerKill> list2;
					DamageTool.explode(base.transform.position + new Vector3(0f, 1.5f, 0f), 10f, EDeathCause.BOULDER, CSteamID.Nil, 60f, 0f, 0f, barricadeDamage2, structureDamage2, vehicleDamage2, 500f, 500f, out list2, EExplosionDamageType.ZOMBIE_ACID, 32f, true, false, EDamageOrigin.Zombie_Stomp, ERagdollEffect.NONE);
					EffectAsset effectAsset = Boulder.Metal_2_Ref.Find();
					if (effectAsset != null)
					{
						EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
						{
							relevantDistance = EffectManager.MEDIUM,
							position = base.transform.position
						});
					}
				}
			}
			if (this.isBreathingFire)
			{
				if (Provider.isServer && this.isBreathingFire)
				{
					this.fireDamage += Time.deltaTime * 50f;
					if (this.fireDamage > 1f)
					{
						float playerDamage = this.fireDamage;
						float num2 = this.fireDamage * 10f;
						this.fireDamage = 0f;
						float barricadeDamage3 = Provider.modeConfigData.Zombies.Can_Target_Barricades ? num2 : 0f;
						float structureDamage3 = Provider.modeConfigData.Zombies.Can_Target_Structures ? num2 : 0f;
						float vehicleDamage3 = Provider.modeConfigData.Zombies.Can_Target_Vehicles ? num2 : 0f;
						List<EPlayerKill> list3;
						DamageTool.explode(base.transform.position + new Vector3(0f, 1.25f, 0f) + base.transform.forward * 3f, 2f, EDeathCause.BURNER, CSteamID.Nil, playerDamage, 0f, 0f, barricadeDamage3, structureDamage3, vehicleDamage3, num2, num2, out list3, EExplosionDamageType.ZOMBIE_FIRE, 4f, false, false, EDamageOrigin.Zombie_Fire_Breath, ERagdollEffect.NONE);
						DamageTool.explode(base.transform.position + new Vector3(0f, 1.25f, 0f) + base.transform.forward * 7f, 3f, EDeathCause.BURNER, CSteamID.Nil, playerDamage, 0f, 0f, barricadeDamage3, structureDamage3, vehicleDamage3, num2, num2, out list3, EExplosionDamageType.ZOMBIE_FIRE, 4f, false, false, EDamageOrigin.Zombie_Fire_Breath, ERagdollEffect.NONE);
						DamageTool.explode(base.transform.position + new Vector3(0f, 1.25f, 0f) + base.transform.forward * 12f, 4f, EDeathCause.BURNER, CSteamID.Nil, playerDamage, 0f, 0f, barricadeDamage3, structureDamage3, vehicleDamage3, num2, num2, out list3, EExplosionDamageType.ZOMBIE_FIRE, 4f, false, false, EDamageOrigin.Zombie_Fire_Breath, ERagdollEffect.NONE);
					}
				}
				if (Time.time - this.lastSpecial > this.fireTime)
				{
					this.isBreathingFire = false;
					if (this.fireSystem != null)
					{
						this.fireSystem.emission.enabled = false;
					}
					if (Provider.isServer)
					{
						this.seeker.canMove = true;
					}
				}
			}
			if (this.isPlayingBoulder)
			{
				if (Time.time - this.lastSpecial > this.boulderTime)
				{
					this.isPlayingBoulder = false;
				}
			}
			else if (this.isPlayingSpit)
			{
				if (Time.time - this.lastSpecial > this.spitTime)
				{
					this.isPlayingSpit = false;
				}
			}
			else if (this.isPlayingCharge)
			{
				if (Time.time - this.lastSpecial > this.chargeTime)
				{
					this.isPlayingCharge = false;
					if (Provider.isServer)
					{
						this.seeker.canMove = true;
					}
				}
			}
			else if (this.isPlayingWind)
			{
				if (Time.time - this.lastSpecial > this.windTime)
				{
					this.isPlayingWind = false;
				}
			}
			else if (this.isPlayingFire)
			{
				if (Time.time - this.lastSpecial > this.fireTime)
				{
					this.isPlayingFire = false;
				}
			}
			else if (this.isPlayingAttack)
			{
				if (Time.time - this.lastAttack > this.attackTime)
				{
					if (this.speciality == EZombieSpeciality.FLANKER_FRIENDLY || this.speciality == EZombieSpeciality.FLANKER_STALK)
					{
						this.updateVisibility(false, true);
					}
					this.isPlayingAttack = false;
				}
			}
			else if (this.isPlayingStartle)
			{
				if (Time.time - this.lastStartle > this.startleTime)
				{
					this.isPlayingStartle = false;
				}
			}
			else if (this.isPlayingStun && Time.time - this.lastStun > this.stunTime)
			{
				this.isPlayingStun = false;
			}
			if (Provider.isServer && this.health < this.maxHealth && Time.time - this.lastRegen > LevelZombies.tables[(int)this.type].regen)
			{
				this.lastRegen = Time.time;
				this.health += 1;
			}
			if (Provider.isServer)
			{
				if (this.isStunned)
				{
					if (Time.time - this.lastStun <= 1f)
					{
						return;
					}
					this.lastTarget = Time.time;
					this.lastStuck = Time.time;
					this.isStunned = false;
					this.seeker.canMove = true;
				}
				if (this.isLeaving && Time.time - this.lastLeave > this.leaveTime)
				{
					this.alert(this.leaveTo, false);
					this.isLeaving = false;
				}
			}
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x001AC6AC File Offset: 0x001AA8AC
		private void onHyperUpdated(bool isHyper)
		{
			if (this.eyes != null)
			{
				this.eyes.gameObject.SetActive(isHyper);
			}
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x001AC6CD File Offset: 0x001AA8CD
		public void init()
		{
			this.awake();
			this.start();
			this.SetCountedAsAliveInZombieRegion(!this.isDead);
			this.SetCountedAsAliveBossInZombieRegion(!this.isDead && this.isBoss);
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x001AC704 File Offset: 0x001AA904
		private void start()
		{
			if (Provider.isServer)
			{
				this.seeker = base.GetComponent<AIPath>();
				base.GetComponent<CharacterController>().enableOverlapRecovery = false;
				this.target = base.transform.Find("Target");
				this.target.parent = null;
				this.seeker.target = this.target;
				this.seeker.canSmooth = false;
				this.reset();
			}
			else
			{
				this.lastUpdatedPos = base.transform.position;
				this.lastUpdatedAngle = base.transform.rotation.eulerAngles.y;
				this.interpPositionTarget = this.lastUpdatedPos;
				this.interpYawTarget = this.lastUpdatedAngle;
			}
			this.lastGroan = Time.time + Random.Range(4f, 16f);
			if (this.isMega)
			{
				this.groanDelay = Random.Range(2f, 4f);
			}
			else
			{
				this.groanDelay = Random.Range(4f, 8f);
			}
			this.updateDifficulty();
			this.updateLife();
			this.apply();
			this.updateEffects();
			this.updateVisibility(this.speciality != EZombieSpeciality.FLANKER_STALK && this.speciality != EZombieSpeciality.SPIRIT && this.speciality != EZombieSpeciality.BOSS_SPIRIT, false);
			this.updateStates();
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x001AC858 File Offset: 0x001AAA58
		private void awake()
		{
			this.throwTime = 1f;
			this.acidTime = 1f;
			this.windTime = 0.9f;
			this.fireTime = 2.75f;
			this.chargeTime = 1.8f;
			this.sparkTime = 1.2f;
			this.boulderTime = 1f;
			this.spitTime = 1f;
			this.attackTime = 0.5f;
			this.startleTime = 0.5f;
			this.stunTime = 0.5f;
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x001AC8E0 File Offset: 0x001AAAE0
		private void OnDestroy()
		{
			if (Provider.isServer)
			{
				this.isHunting = false;
			}
			if (this.target != null && this.target.parent != this)
			{
				Object.Destroy(this.target.gameObject);
			}
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x001AC92C File Offset: 0x001AAB2C
		private void PlayOneShot(AudioClip[] clips)
		{
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x001AC930 File Offset: 0x001AAB30
		private float GetRandomPitch()
		{
			float num;
			if (this.isMega)
			{
				num = Random.Range(0.5f, 0.7f);
			}
			else if (this.isCutesy)
			{
				num = Random.Range(1.3f, 1.4f);
			}
			else
			{
				num = Random.Range(0.9f, 1.1f);
			}
			if (this.isHyper)
			{
				num *= 0.9f;
			}
			return num;
		}

		/// <summary>
		/// Helper to prevent mistakes or plugins from breaking alive zombie count.
		/// </summary>
		// Token: 0x06004752 RID: 18258 RVA: 0x001AC992 File Offset: 0x001AAB92
		private void SetCountedAsAliveInZombieRegion(bool newValue)
		{
			if (this.isCountedAsAliveInZombieRegion != newValue)
			{
				this.isCountedAsAliveInZombieRegion = newValue;
				if (newValue)
				{
					this.zombieRegion.alive++;
					return;
				}
				this.zombieRegion.alive--;
			}
		}

		/// <summary>
		/// Helper to prevent mistakes or plugins from breaking alive boss zombie count.
		/// </summary>
		// Token: 0x06004753 RID: 18259 RVA: 0x001AC9CE File Offset: 0x001AABCE
		private void SetCountedAsAliveBossInZombieRegion(bool newValue)
		{
			if (this.isCountedAsAliveBossInZombieRegion != newValue)
			{
				this.isCountedAsAliveBossInZombieRegion = newValue;
				if (newValue)
				{
					this.zombieRegion.aliveBossZombieCount++;
					return;
				}
				this.zombieRegion.aliveBossZombieCount--;
			}
		}

		/// <summary>
		/// 2023-01-31: set height to 2 rather than adjusting per-zombie-type. Tall zombies (megas) couldn't
		/// get through doorways, and short zombies (crawlers) could get underneath objects they shouldn't
		/// like gas tanks. Zombies were also stacking on top of eachother a bit too much.
		/// </summary>
		// Token: 0x06004754 RID: 18260 RVA: 0x001ACA0C File Offset: 0x001AAC0C
		private void SetCapsuleRadiusAndHeight(float radius, float height)
		{
			if (Provider.isServer)
			{
				CharacterController component = base.GetComponent<CharacterController>();
				if (component != null)
				{
					component.radius = radius;
					component.center = new Vector3(0f, height * 0.5f, 0f);
					component.height = height;
					return;
				}
			}
			else
			{
				CapsuleCollider component2 = base.GetComponent<CapsuleCollider>();
				if (component2 != null)
				{
					component2.radius = radius;
					component2.center = new Vector3(0f, height * 0.5f, 0f);
					component2.height = height;
				}
			}
		}

		// Token: 0x04003076 RID: 12406
		private static List<RegionCoordinate> regionsInRadius = new List<RegionCoordinate>(4);

		// Token: 0x04003077 RID: 12407
		private static List<Transform> structuresInRadius = new List<Transform>();

		// Token: 0x04003078 RID: 12408
		private static List<InteractableVehicle> vehiclesInRadius = new List<InteractableVehicle>();

		// Token: 0x04003079 RID: 12409
		private static List<Transform> barricadesInRadius = new List<Transform>();

		// Token: 0x0400307A RID: 12410
		private static readonly float ATTACK_BARRICADE = 16f;

		// Token: 0x0400307B RID: 12411
		private static readonly float ATTACK_VEHICLE = 16f;

		// Token: 0x0400307C RID: 12412
		private static readonly float ATTACK_PLAYER = 2f;

		// Token: 0x0400307D RID: 12413
		private Transform skeleton;

		// Token: 0x0400307E RID: 12414
		private Transform rightHook;

		// Token: 0x0400307F RID: 12415
		private SkinnedMeshRenderer renderer_0;

		// Token: 0x04003080 RID: 12416
		private SkinnedMeshRenderer renderer_1;

		// Token: 0x04003081 RID: 12417
		private Transform eyes;

		// Token: 0x04003082 RID: 12418
		private Transform radiation;

		// Token: 0x04003083 RID: 12419
		private Transform burner;

		// Token: 0x04003084 RID: 12420
		private Transform acid;

		// Token: 0x04003085 RID: 12421
		private Transform acidNuclear;

		// Token: 0x04003086 RID: 12422
		private Transform electric;

		// Token: 0x04003087 RID: 12423
		private ParticleSystem sparkSystem;

		// Token: 0x04003088 RID: 12424
		private ParticleSystem fireSystem;

		// Token: 0x04003089 RID: 12425
		private AudioSource fireAudio;

		// Token: 0x0400308A RID: 12426
		private Material skinMaterial;

		// Token: 0x0400308B RID: 12427
		private Transform attachmentModel_0;

		// Token: 0x0400308C RID: 12428
		private Transform attachmentModel_1;

		// Token: 0x0400308D RID: 12429
		private Material attachmentMaterial_0;

		// Token: 0x0400308E RID: 12430
		private Material attachmentMaterial_1;

		// Token: 0x0400308F RID: 12431
		public ushort id;

		// Token: 0x04003090 RID: 12432
		public byte bound;

		// Token: 0x04003091 RID: 12433
		public byte type;

		// Token: 0x04003092 RID: 12434
		public EZombieSpeciality speciality;

		// Token: 0x04003093 RID: 12435
		public byte shirt;

		// Token: 0x04003094 RID: 12436
		public byte pants;

		// Token: 0x04003095 RID: 12437
		public byte hat;

		// Token: 0x04003096 RID: 12438
		public byte gear;

		// Token: 0x04003097 RID: 12439
		private byte _move;

		// Token: 0x04003098 RID: 12440
		private string moveAnim;

		// Token: 0x04003099 RID: 12441
		private byte _idle;

		// Token: 0x0400309A RID: 12442
		public string idleAnim;

		// Token: 0x0400309B RID: 12443
		public bool isUpdated;

		// Token: 0x0400309C RID: 12444
		private AIPath seeker;

		// Token: 0x0400309D RID: 12445
		private Player player;

		// Token: 0x0400309E RID: 12446
		private Transform barricade;

		// Token: 0x0400309F RID: 12447
		private Transform structure;

		/// <summary>
		/// If zombie is stuck this was a nearby vehicle potentially blocking our path.
		/// </summary>
		// Token: 0x040030A0 RID: 12448
		private InteractableVehicle targetObstructionVehicle;

		/// <summary>
		/// If target player is passenger in a vehicle this is their vehicle.
		/// </summary>
		// Token: 0x040030A1 RID: 12449
		private InteractableVehicle targetPassengerVehicle;

		// Token: 0x040030A2 RID: 12450
		private Transform target;

		// Token: 0x040030A3 RID: 12451
		private Animation animator;

		// Token: 0x040030A4 RID: 12452
		private float lastHunted;

		// Token: 0x040030A5 RID: 12453
		private float lastTarget;

		// Token: 0x040030A6 RID: 12454
		private float lastLeave;

		// Token: 0x040030A7 RID: 12455
		private float lastSpecial;

		// Token: 0x040030A8 RID: 12456
		private float lastAttack;

		// Token: 0x040030A9 RID: 12457
		private float lastStartle;

		// Token: 0x040030AA RID: 12458
		private float lastStun;

		// Token: 0x040030AB RID: 12459
		private float lastGroan;

		// Token: 0x040030AC RID: 12460
		private float lastRegen;

		// Token: 0x040030AD RID: 12461
		private float lastStuck;

		// Token: 0x040030AE RID: 12462
		private Vector3 cameFrom;

		// Token: 0x040030AF RID: 12463
		private bool isPulled;

		// Token: 0x040030B0 RID: 12464
		private float lastPull;

		// Token: 0x040030B1 RID: 12465
		private float pullDelay;

		// Token: 0x040030B2 RID: 12466
		private float groanDelay;

		// Token: 0x040030B3 RID: 12467
		private float leaveTime;

		// Token: 0x040030B4 RID: 12468
		private float throwTime;

		// Token: 0x040030B5 RID: 12469
		private float boulderTime;

		// Token: 0x040030B6 RID: 12470
		private float spitTime;

		// Token: 0x040030B7 RID: 12471
		private float acidTime;

		// Token: 0x040030B8 RID: 12472
		private float chargeTime;

		// Token: 0x040030B9 RID: 12473
		private float sparkTime;

		// Token: 0x040030BA RID: 12474
		private float windTime;

		// Token: 0x040030BB RID: 12475
		private float fireTime;

		// Token: 0x040030BC RID: 12476
		private float attackTime;

		// Token: 0x040030BD RID: 12477
		private float startleTime;

		// Token: 0x040030BE RID: 12478
		private float stunTime;

		// Token: 0x040030BF RID: 12479
		private bool isThrowingBoulder;

		// Token: 0x040030C0 RID: 12480
		private bool isSpittingAcid;

		// Token: 0x040030C1 RID: 12481
		private bool isChargingSpark;

		// Token: 0x040030C2 RID: 12482
		private bool isStompingWind;

		// Token: 0x040030C3 RID: 12483
		private bool isBreathingFire;

		// Token: 0x040030C4 RID: 12484
		private bool isPlayingBoulder;

		// Token: 0x040030C5 RID: 12485
		private bool isPlayingSpit;

		// Token: 0x040030C6 RID: 12486
		private bool isPlayingCharge;

		// Token: 0x040030C7 RID: 12487
		private bool isPlayingWind;

		// Token: 0x040030C8 RID: 12488
		private bool isPlayingFire;

		// Token: 0x040030C9 RID: 12489
		private bool isPlayingAttack;

		// Token: 0x040030CA RID: 12490
		private bool isPlayingStartle;

		// Token: 0x040030CB RID: 12491
		private bool isPlayingStun;

		// Token: 0x040030CC RID: 12492
		private Vector3 lastUpdatedPos;

		// Token: 0x040030CD RID: 12493
		private float lastUpdatedAngle;

		// Token: 0x040030CE RID: 12494
		private Vector3 interpPositionTarget;

		// Token: 0x040030CF RID: 12495
		private float interpYawTarget;

		// Token: 0x040030D0 RID: 12496
		private bool isMoving;

		// Token: 0x040030D1 RID: 12497
		private bool isAttacking;

		// Token: 0x040030D2 RID: 12498
		private bool isVisible;

		// Token: 0x040030D3 RID: 12499
		private bool isWandering;

		// Token: 0x040030D4 RID: 12500
		private bool isTicking;

		// Token: 0x040030D5 RID: 12501
		private bool _isHunting;

		// Token: 0x040030D6 RID: 12502
		private EHuntType huntType;

		// Token: 0x040030D7 RID: 12503
		private bool isLeaving;

		// Token: 0x040030D8 RID: 12504
		private bool isStunned;

		// Token: 0x040030D9 RID: 12505
		private bool isStuck;

		// Token: 0x040030DA RID: 12506
		private Vector3 leaveTo;

		// Token: 0x040030DB RID: 12507
		private float _lastDead;

		// Token: 0x040030DC RID: 12508
		public bool isDead;

		// Token: 0x040030DD RID: 12509
		private ushort health;

		// Token: 0x040030DE RID: 12510
		private ushort maxHealth;

		// Token: 0x040030DF RID: 12511
		private Vector3 ragdoll;

		// Token: 0x040030E0 RID: 12512
		private EZombiePath path;

		// Token: 0x040030E1 RID: 12513
		private float specialStartleDelay;

		// Token: 0x040030E2 RID: 12514
		private float specialAttackDelay;

		// Token: 0x040030E3 RID: 12515
		private float specialUseDelay;

		/// <summary>
		/// Yeah it seems kinda ugly to pollute all zombies with this code... zombie rewrite eventually please.
		/// </summary>
		// Token: 0x040030E4 RID: 12516
		private float flashbangDelay;

		// Token: 0x040030E5 RID: 12517
		private float lastFlashbang;

		// Token: 0x040030E6 RID: 12518
		private Transform boulderItem;

		// Token: 0x040030E7 RID: 12519
		private float fireDamage;

		// Token: 0x040030E8 RID: 12520
		private bool hasUpdateVisibilityBeenCalledYet;

		// Token: 0x040030EA RID: 12522
		private bool needsTickForPlacement;

		/// <summary>
		/// Reduces frequency of UndergroundAllowlist checks because it can be expensive with lots of entities and volumes. 
		/// </summary>
		// Token: 0x040030EB RID: 12523
		private float undergroundTestTimer = 10f;

		// Token: 0x040030EC RID: 12524
		private float lastTick;

		// Token: 0x040030ED RID: 12525
		internal ZombieRegion zombieRegion;

		// Token: 0x040030EE RID: 12526
		private bool isCountedAsAliveInZombieRegion;

		// Token: 0x040030EF RID: 12527
		private bool isCountedAsAliveBossInZombieRegion;

		// Token: 0x040030F0 RID: 12528
		private static readonly AssetReference<EffectAsset> KuwaitBossFlashbangRef = new AssetReference<EffectAsset>("5436f56485c841a7bbec8e79a163ad19");

		// Token: 0x040030F1 RID: 12529
		private static readonly AssetReference<EffectAsset> BuakBossFlashbangRef = new AssetReference<EffectAsset>("b7acfd045ceb40c1b84788cb9159d0f2");

		// Token: 0x040030F2 RID: 12530
		private static readonly AssetReference<EffectAsset> Zombie_0_Ref = new AssetReference<EffectAsset>("000f550dc3d44586b7fc0f6e5b2530d9");

		// Token: 0x040030F3 RID: 12531
		private static readonly AssetReference<EffectAsset> Zombie_1_Ref = new AssetReference<EffectAsset>("f2f0d31897024317b32b58c00c1f78dd");

		// Token: 0x040030F4 RID: 12532
		private static readonly AssetReference<EffectAsset> Zombie_2_Ref = new AssetReference<EffectAsset>("469414f0a1b047c58732bb6076b0c035");

		// Token: 0x040030F5 RID: 12533
		private static readonly AssetReference<EffectAsset> Zombie_3_Ref = new AssetReference<EffectAsset>("ae477aac40b64d3c8ce8e538daffecf5");

		// Token: 0x040030F6 RID: 12534
		private static readonly AssetReference<EffectAsset> Zombie_4_Ref = new AssetReference<EffectAsset>("9fd759eda4b746dfb9f2599bf8f27219");

		// Token: 0x040030F7 RID: 12535
		private static readonly AssetReference<EffectAsset> Zombie_5_Ref = new AssetReference<EffectAsset>("50872061be8e411ea28780fcb7aa7cef");

		// Token: 0x040030F8 RID: 12536
		private static readonly AssetReference<EffectAsset> Zombie_6_Ref = new AssetReference<EffectAsset>("23363b069ad740819f1d7131656f8ca7");

		// Token: 0x040030F9 RID: 12537
		private static readonly AssetReference<EffectAsset> Zombie_7_Ref = new AssetReference<EffectAsset>("36b272f5be8c4427b0fdd0625f361c15");

		// Token: 0x040030FA RID: 12538
		private static List<Zombie.EAbilityChoice> availableAbilityChoices = new List<Zombie.EAbilityChoice>();

		// Token: 0x02000A2B RID: 2603
		private enum EAbilityChoice
		{
			// Token: 0x0400354F RID: 13647
			ThrowBoulder,
			// Token: 0x04003550 RID: 13648
			SpitAcid,
			// Token: 0x04003551 RID: 13649
			Stomp,
			// Token: 0x04003552 RID: 13650
			BreatheFire,
			// Token: 0x04003553 RID: 13651
			ElectricShock,
			// Token: 0x04003554 RID: 13652
			Flashbang
		}
	}
}
