using System;
using System.Collections.Generic;
using SDG.Framework.Water;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000282 RID: 642
	public class Animal : MonoBehaviour
	{
		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060012B1 RID: 4785 RVA: 0x00042D6F File Offset: 0x00040F6F
		// (set) Token: 0x060012B2 RID: 4786 RVA: 0x00042D77 File Offset: 0x00040F77
		public Vector3 target { get; private set; }

		// Token: 0x060012B3 RID: 4787 RVA: 0x00042D80 File Offset: 0x00040F80
		private void updateTicking()
		{
			if (this.isFleeing || this.isWandering || this.isHunting)
			{
				if (this.isTicking)
				{
					return;
				}
				this.isTicking = true;
				AnimalManager.tickingAnimals.Add(this);
				this.lastTick = Time.timeAsDouble;
				return;
			}
			else
			{
				if (!this.isTicking)
				{
					return;
				}
				this.isTicking = false;
				AnimalManager.tickingAnimals.RemoveFast(this);
				return;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060012B4 RID: 4788 RVA: 0x00042DE8 File Offset: 0x00040FE8
		public bool isFleeing
		{
			get
			{
				return this._isFleeing;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060012B5 RID: 4789 RVA: 0x00042DF0 File Offset: 0x00040FF0
		// (set) Token: 0x060012B6 RID: 4790 RVA: 0x00042DF8 File Offset: 0x00040FF8
		public bool isHunting { get; private set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060012B7 RID: 4791 RVA: 0x00042E01 File Offset: 0x00041001
		public float lastDead
		{
			get
			{
				return this._lastDead;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060012B8 RID: 4792 RVA: 0x00042E09 File Offset: 0x00041009
		public bool IsAlive
		{
			get
			{
				return !this.isDead;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060012B9 RID: 4793 RVA: 0x00042E14 File Offset: 0x00041014
		public AnimalAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x00042E1C File Offset: 0x0004101C
		public float GetHealth()
		{
			return (float)this.health;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x00042E25 File Offset: 0x00041025
		public Player GetTargetPlayer()
		{
			return this.currentTargetPlayer;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x00042E30 File Offset: 0x00041030
		public void askEat()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastEat = Time.timeAsDouble;
			this.eatDelay = Random.Range(4f, 8f);
			this.isPlayingEat = true;
			if (this.asset.shouldPlayAnimsOnDedicatedServer)
			{
				string text;
				if (this.asset.eatAnimVariantsCount == 1)
				{
					text = "Eat";
				}
				else
				{
					text = "Eat_" + Random.Range(0, this.asset.eatAnimVariantsCount).ToString();
				}
				AnimationClip clip = this.animator.GetClip(text);
				if (clip != null)
				{
					this.eatTime = clip.length;
					this.animator.Play(text);
					return;
				}
				if (Assets.shouldValidateAssets)
				{
					Assets.reportError(this.asset, "missing AnimationClip \"" + text + "\"");
				}
			}
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x00042F10 File Offset: 0x00041110
		public void askGlance()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastGlance = Time.timeAsDouble;
			this.glanceDelay = Random.Range(4f, 8f);
			this.isPlayingGlance = true;
			if (this.asset.shouldPlayAnimsOnDedicatedServer)
			{
				string text = "Glance_" + Random.Range(0, this.asset.glanceAnimVariantsCount).ToString();
				AnimationClip clip = this.animator.GetClip(text);
				if (clip != null)
				{
					this.glanceTime = clip.length;
					this.animator.Play(text);
					return;
				}
				if (Assets.shouldValidateAssets)
				{
					Assets.reportError(this.asset, "missing AnimationClip \"" + text + "\"");
				}
			}
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00042FD8 File Offset: 0x000411D8
		public void PlayStartleAnimation()
		{
			if (this.isDead)
			{
				return;
			}
			this.startleAnimationCompletionTime = Time.timeAsDouble + 0.5;
			this.isPlayingStartleAnimation = true;
			if (this.asset.shouldPlayAnimsOnDedicatedServer)
			{
				string text;
				if (this.asset.startleAnimVariantsCount == 1)
				{
					text = "Startle";
				}
				else
				{
					text = "Startle_" + Random.Range(0, this.asset.startleAnimVariantsCount).ToString();
				}
				AnimationClip clip = this.animator.GetClip(text);
				if (clip != null)
				{
					this.startleAnimationCompletionTime = Time.timeAsDouble + (double)clip.length;
					this.animator.Play(text);
					return;
				}
				if (Assets.shouldValidateAssets)
				{
					Assets.reportError(this.asset, "missing AnimationClip \"" + text + "\"");
				}
			}
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x000430B4 File Offset: 0x000412B4
		public void askAttack()
		{
			if (this.isDead)
			{
				return;
			}
			this.lastAttack = Time.timeAsDouble;
			this.isPlayingAttack = true;
			if (this.asset.shouldPlayAnimsOnDedicatedServer)
			{
				string text;
				if (this.asset.attackAnimVariantsCount == 1)
				{
					text = "Attack";
				}
				else
				{
					text = "Attack_" + Random.Range(0, this.asset.attackAnimVariantsCount).ToString();
				}
				AnimationClip clip = this.animator.GetClip(text);
				if (clip != null)
				{
					this.attackDuration = clip.length;
					this.attackInterval = Mathf.Max(this.attackDuration, this.asset.attackInterval);
					this.animator.Play(text);
				}
				else if (Assets.shouldValidateAssets)
				{
					Assets.reportError(this.asset, "missing AnimationClip \"" + text + "\"");
				}
				if (this.asset != null && this.asset.roars != null && this.asset.roars.Length != 0 && Time.timeAsDouble - this.startedRoar > 1.0)
				{
					this.startedRoar = Time.timeAsDouble;
				}
			}
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x000431E0 File Offset: 0x000413E0
		public void askPanic()
		{
			if (this.isDead)
			{
				return;
			}
			if (this.asset.shouldPlayAnimsOnDedicatedServer && this.asset != null && this.asset.panics != null && this.asset.panics.Length != 0 && Time.timeAsDouble - this.startedPanic > 1.0)
			{
				this.startedPanic = Time.timeAsDouble;
			}
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x00043248 File Offset: 0x00041448
		public void askDamage(ushort amount, Vector3 newRagdoll, out EPlayerKill kill, out uint xp, bool trackKill = true, bool dropLoot = true, ERagdollEffect ragdollEffect = ERagdollEffect.NONE)
		{
			kill = EPlayerKill.NONE;
			xp = 0U;
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (this.IsAlive)
			{
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
					kill = EPlayerKill.ANIMAL;
					if (this.asset != null)
					{
						xp = this.asset.rewardXP;
					}
					if (dropLoot)
					{
						AnimalManager.dropLoot(this);
					}
					AnimalManager.sendAnimalDead(this, this.ragdoll, ragdollEffect);
					if (trackKill)
					{
						for (int i = 0; i < Provider.clients.Count; i++)
						{
							SteamPlayer steamPlayer = Provider.clients[i];
							if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead && (steamPlayer.player.transform.position - base.transform.position).sqrMagnitude < 262144f)
							{
								steamPlayer.player.quests.trackAnimalKill(this);
							}
						}
					}
				}
				else if (this.asset != null && this.asset.panics != null && this.asset.panics.Length != 0)
				{
					AnimalManager.sendAnimalPanic(this);
				}
				this.lastRegen = Time.timeAsDouble;
			}
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x000433C1 File Offset: 0x000415C1
		public void sendRevive(Vector3 position, float angle)
		{
			AnimalManager.sendAnimalAlive(this, position, MeasurementTool.angleToByte(angle));
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x000433D0 File Offset: 0x000415D0
		private bool checkTargetValid(Vector3 point)
		{
			if (!Level.checkSafeIncludingClipVolumes(point))
			{
				return false;
			}
			float height = LevelGround.getHeight(point);
			return !WaterUtility.isPointUnderwater(new Vector3(point.x, height - 1f, point.z));
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00043410 File Offset: 0x00041610
		private Vector3 getFleeTarget(Vector3 normal)
		{
			Vector3 vector = base.transform.position + normal * 64f + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
			if (this.checkTargetValid(vector))
			{
				return vector;
			}
			Vector3 vector2 = base.transform.position + normal * 32f + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
			if (this.checkTargetValid(vector2))
			{
				return vector2;
			}
			vector2 = base.transform.position + normal * -32f + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
			if (this.checkTargetValid(vector2))
			{
				return vector2;
			}
			vector2 = base.transform.position + normal * -16f + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
			if (this.checkTargetValid(vector2))
			{
				return vector2;
			}
			return vector;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00043570 File Offset: 0x00041770
		private void getWanderTarget()
		{
			Vector3 vector;
			if (this.isStuck)
			{
				vector = base.transform.position + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
				if (!this.checkTargetValid(vector))
				{
					return;
				}
			}
			else if (this.pack != null)
			{
				if ((base.transform.position - this.pack.getAverageAnimalPoint()).sqrMagnitude > 256f)
				{
					vector = this.pack.getAverageAnimalPoint() + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
				}
				else
				{
					Vector3 wanderDirection = this.pack.getWanderDirection();
					vector = base.transform.position + wanderDirection * Random.Range(6f, 8f) + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
					if (!this.checkTargetValid(vector))
					{
						vector = base.transform.position - wanderDirection * Random.Range(6f, 8f) + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
						if (!this.checkTargetValid(vector))
						{
							return;
						}
						this.pack.wanderAngle += Random.Range(160f, 200f);
					}
					else
					{
						this.pack.wanderAngle += Random.Range(-20f, 20f);
					}
				}
			}
			else
			{
				vector = base.transform.position + new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
			}
			this.target = vector;
			this.isWandering = true;
			this.updateTicking();
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00043799 File Offset: 0x00041999
		public bool checkAlert(Player potentialTargetPlayer)
		{
			return this.currentTargetPlayer != potentialTargetPlayer;
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x000437A8 File Offset: 0x000419A8
		public void alertPlayer(Player potentialTargetPlayer, bool sendToPack)
		{
			if (sendToPack && this.pack != null)
			{
				for (int i = 0; i < this.pack.animals.Count; i++)
				{
					Animal animal = this.pack.animals[i];
					if (!(animal == null) && !(animal == this))
					{
						animal.alertPlayer(potentialTargetPlayer, false);
					}
				}
			}
			if (this.isDead)
			{
				return;
			}
			if (this.currentTargetPlayer == potentialTargetPlayer)
			{
				return;
			}
			if (!this.isHunting)
			{
				AnimalManager.sendAnimalStartle(this);
			}
			if (this.currentTargetPlayer == null)
			{
				this._isFleeing = false;
				this.isWandering = false;
				this.isHunting = true;
				this.updateTicking();
				this.lastStuck = Time.timeAsDouble;
				this.currentTargetPlayer = potentialTargetPlayer;
				return;
			}
			if ((potentialTargetPlayer.transform.position - base.transform.position).sqrMagnitude < (this.currentTargetPlayer.transform.position - base.transform.position).sqrMagnitude)
			{
				this._isFleeing = false;
				this.isWandering = false;
				this.isHunting = true;
				this.updateTicking();
				this.currentTargetPlayer = potentialTargetPlayer;
			}
		}

		/// <summary>
		/// Alert this animal that it was damaged from a given position.
		/// Offensive animals investigate the position, whereas other animals run away.
		/// </summary>
		// Token: 0x060012C8 RID: 4808 RVA: 0x000438D8 File Offset: 0x00041AD8
		public void alertDamagedFromPoint(Vector3 point)
		{
			if (this.asset != null && this.asset.behaviour == EAnimalBehaviour.OFFENSE)
			{
				this.alertGoToPoint(point, true);
				return;
			}
			this.alertRunAwayFromPoint(point, true);
		}

		/// <summary>
		/// Alerts this animal that it needs to run away.
		/// </summary>
		/// <param name="newPosition">The position to run away from.</param>
		// Token: 0x060012C9 RID: 4809 RVA: 0x00043904 File Offset: 0x00041B04
		public void alertRunAwayFromPoint(Vector3 newPosition, bool sendToPack)
		{
			this.alertDirection((base.transform.position - newPosition).normalized, sendToPack);
		}

		/// <summary>
		/// Keep for plugin backwards compatibility.
		/// </summary>
		// Token: 0x060012CA RID: 4810 RVA: 0x00043931 File Offset: 0x00041B31
		[Obsolete("Clarified with alertRunAwayFromPoint, alertGoToPoint and alertDamagedFromPoint.")]
		public void alertPoint(Vector3 newPosition, bool sendToPack)
		{
			this.alertRunAwayFromPoint(newPosition, sendToPack);
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0004393C File Offset: 0x00041B3C
		public void alertDirection(Vector3 newDirection, bool sendToPack)
		{
			if (sendToPack && this.pack != null)
			{
				for (int i = 0; i < this.pack.animals.Count; i++)
				{
					Animal animal = this.pack.animals[i];
					if (!(animal == null) && !(animal == this))
					{
						animal.alertDirection(newDirection, false);
					}
				}
			}
			if (this.isDead)
			{
				return;
			}
			if (this.isStuck)
			{
				return;
			}
			if (this.isHunting)
			{
				return;
			}
			if (!this.isFleeing)
			{
				AnimalManager.sendAnimalStartle(this);
			}
			this._isFleeing = true;
			this.isWandering = false;
			this.isHunting = false;
			this.updateTicking();
			this.target = this.getFleeTarget(newDirection);
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x000439EC File Offset: 0x00041BEC
		public void alertGoToPoint(Vector3 point, bool sendToPack)
		{
			if (sendToPack && this.pack != null)
			{
				for (int i = 0; i < this.pack.animals.Count; i++)
				{
					Animal animal = this.pack.animals[i];
					if (!(animal == null) && !(animal == this))
					{
						animal.alertGoToPoint(point, false);
					}
				}
			}
			if (this.isDead)
			{
				return;
			}
			if (this.isFleeing || this.isHunting)
			{
				return;
			}
			this.lastWander = Time.timeAsDouble;
			this._isFleeing = false;
			this.isWandering = true;
			this.isHunting = false;
			this.target = point;
			this.updateTicking();
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00043A94 File Offset: 0x00041C94
		private void stop()
		{
			this.isMoving = false;
			this.isRunning = false;
			this._isFleeing = false;
			this.isWandering = false;
			this.isHunting = false;
			this.updateTicking();
			this.isStuck = false;
			this.currentTargetPlayer = null;
			this.target = base.transform.position;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00043AEC File Offset: 0x00041CEC
		public void tellAlive(Vector3 newPosition, byte newAngle)
		{
			this.isDead = false;
			base.transform.position = newPosition;
			base.transform.rotation = Quaternion.Euler(0f, (float)(newAngle * 2), 0f);
			this.updateLife();
			this.updateStates();
			this.reset();
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x00043B3C File Offset: 0x00041D3C
		public void tellDead(Vector3 newRagdoll, ERagdollEffect ragdollEffect)
		{
			this.isDead = true;
			this._lastDead = Time.realtimeSinceStartup;
			this.updateLife();
			if (Provider.isServer)
			{
				this.stop();
			}
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00043B63 File Offset: 0x00041D63
		[Obsolete]
		public void tellState(Vector3 newPosition, byte newAngle)
		{
			this.tellState(newPosition, (float)newAngle * 2f);
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x00043B74 File Offset: 0x00041D74
		public void tellState(Vector3 newPosition, float newAngle)
		{
			this.lastUpdatePos = newPosition;
			this.lastUpdateAngle = newAngle;
			if (this.nsb != null)
			{
				this.nsb.addNewSnapshot(new YawSnapshotInfo(newPosition, newAngle));
			}
			if (this.isPlayingEat || this.isPlayingGlance)
			{
				this.isPlayingEat = false;
				this.isPlayingGlance = false;
				this.animator.Stop();
			}
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x00043BD4 File Offset: 0x00041DD4
		private void updateLife()
		{
			if (this.controller != null)
			{
				this.controller.SetDetectCollisionsDeferred(this.IsAlive);
			}
			if (this.asset.shouldPlayAnimsOnDedicatedServer)
			{
				if (this.renderer_0 != null)
				{
					this.renderer_0.enabled = this.IsAlive;
				}
				if (this.renderer_1 != null)
				{
					this.renderer_1.enabled = this.IsAlive;
				}
				this.skeleton.gameObject.SetActive(this.IsAlive);
			}
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				component.enabled = this.IsAlive;
			}
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00043C80 File Offset: 0x00041E80
		public void updateStates()
		{
			this.lastUpdatePos = base.transform.position;
			this.lastUpdateAngle = base.transform.rotation.eulerAngles.y;
			if (this.nsb != null)
			{
				this.nsb.updateLastSnapshot(new YawSnapshotInfo(base.transform.position, base.transform.rotation.eulerAngles.y));
			}
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00043CF8 File Offset: 0x00041EF8
		private void reset()
		{
			this.target = base.transform.position;
			this.lastWander = Time.timeAsDouble;
			this.lastStuck = Time.timeAsDouble;
			this.isPlayingEat = false;
			this.isPlayingGlance = false;
			this.isPlayingStartleAnimation = false;
			this.isMoving = false;
			this.isRunning = false;
			this._isFleeing = false;
			this.isWandering = false;
			this.isHunting = false;
			this.updateTicking();
			this.isStuck = false;
			this.health = this.asset.health;
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x00043D84 File Offset: 0x00041F84
		private void move(float delta)
		{
			Vector3 vector = this.target - base.transform.position;
			vector.y = 0f;
			Vector3 forward = vector;
			float magnitude = vector.magnitude;
			bool flag = magnitude > 0.75f;
			if (this.asset.shouldPlayAnimsOnDedicatedServer && flag && !this.isMoving)
			{
				if (this.isPlayingEat)
				{
					this.animator.Stop();
					this.isPlayingEat = false;
				}
				if (this.isPlayingGlance)
				{
					this.animator.Stop();
					this.isPlayingGlance = false;
				}
			}
			this.isMoving = flag;
			this.isRunning = (this.isMoving && (this.isFleeing || this.isHunting));
			float num = Mathf.Clamp01(magnitude / 0.6f);
			Vector3 forward2 = base.transform.forward;
			float a = Vector3.Dot(vector.normalized, forward2);
			float num2 = (this.isRunning ? this.asset.speedRun : this.asset.speedWalk) * Mathf.Max(a, 0.05f) * num;
			if (Time.deltaTime > 0f)
			{
				num2 = Mathf.Clamp(num2, 0f, magnitude / (Time.deltaTime * 2f));
			}
			vector = base.transform.forward * num2;
			vector.y = Physics.gravity.y * 2f;
			if (!this.isMoving)
			{
				vector.x = 0f;
				vector.z = 0f;
				if (!this.isStuck)
				{
					this._isFleeing = false;
					this.isWandering = false;
					this.updateTicking();
				}
			}
			else
			{
				Quaternion quaternion = base.transform.rotation;
				Quaternion b = Quaternion.LookRotation(forward);
				Vector3 eulerAngles = Quaternion.Slerp(quaternion, b, 8f * delta).eulerAngles;
				eulerAngles.z = 0f;
				eulerAngles.x = 0f;
				quaternion = Quaternion.Euler(eulerAngles);
				base.transform.rotation = quaternion;
			}
			CharacterController characterController = this.controller;
			if (characterController == null)
			{
				return;
			}
			characterController.Move(vector * delta);
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x00043FA0 File Offset: 0x000421A0
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
					if (Mathf.Abs(this.lastUpdatePos.x - base.transform.position.x) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatePos.y - base.transform.position.y) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatePos.z - base.transform.position.z) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdateAngle - base.transform.rotation.eulerAngles.y) > 1f)
					{
						this.lastUpdatePos = base.transform.position;
						this.lastUpdateAngle = base.transform.rotation.eulerAngles.y;
						this.isUpdated = true;
						AnimalManager.updates += 1;
						if (this.isStuck && Time.timeAsDouble - this.lastStuck > 0.5)
						{
							this.isStuck = false;
							this.lastStuck = Time.timeAsDouble;
						}
					}
					else if (this.isMoving)
					{
						if (Time.timeAsDouble - this.lastStuck > 0.125)
						{
							this.isStuck = true;
						}
					}
					else
					{
						this.isStuck = false;
						this.lastStuck = Time.timeAsDouble;
					}
				}
			}
			else
			{
				if (Mathf.Abs(this.lastUpdatePos.x - base.transform.position.x) > 0.01f || Mathf.Abs(this.lastUpdatePos.y - base.transform.position.y) > 0.01f || Mathf.Abs(this.lastUpdatePos.z - base.transform.position.z) > 0.01f)
				{
					if (!this.isMoving)
					{
						if (this.isPlayingEat)
						{
							this.animator.Stop();
							this.isPlayingEat = false;
						}
						if (this.isPlayingGlance)
						{
							this.animator.Stop();
							this.isPlayingGlance = false;
						}
					}
					this.isMoving = true;
					this.isRunning = ((this.lastUpdatePos - base.transform.position).sqrMagnitude > 1f);
				}
				else
				{
					this.isMoving = false;
					this.isRunning = false;
				}
				if (this.nsb != null)
				{
					YawSnapshotInfo currentSnapshot = this.nsb.getCurrentSnapshot();
					base.transform.position = currentSnapshot.pos;
					base.transform.rotation = Quaternion.Euler(0f, currentSnapshot.yaw, 0f);
				}
			}
			if (this.asset.shouldPlayAnimsOnDedicatedServer && !this.isMoving && !this.isPlayingEat && !this.isPlayingGlance && !this.isPlayingAttack && Time.timeAsDouble - this.lastAttack > (double)this.attackInterval + 0.5)
			{
				if (Time.timeAsDouble - this.lastEat > (double)this.eatDelay)
				{
					this.askEat();
				}
				else if (Time.timeAsDouble - this.lastGlance > (double)this.glanceDelay)
				{
					this.askGlance();
				}
			}
			if (Provider.isServer)
			{
				if (this.isStuck)
				{
					if (Time.timeAsDouble - this.lastStuck > 0.75)
					{
						this.lastStuck = Time.timeAsDouble;
						this.getWanderTarget();
					}
				}
				else if (!this.isFleeing && !this.isHunting)
				{
					if (Time.timeAsDouble - this.lastWander > (double)this.wanderDelay)
					{
						this.lastWander = Time.timeAsDouble;
						this.wanderDelay = Random.Range(8f, 16f);
						this.getWanderTarget();
					}
				}
				else
				{
					this.lastWander = Time.timeAsDouble;
				}
			}
			if (this.isPlayingEat)
			{
				if (Time.timeAsDouble - this.lastEat > (double)this.eatTime)
				{
					this.isPlayingEat = false;
				}
			}
			else if (this.isPlayingGlance)
			{
				if (Time.timeAsDouble - this.lastGlance > (double)this.glanceTime)
				{
					this.isPlayingGlance = false;
				}
			}
			else if (this.isPlayingStartleAnimation)
			{
				if (Time.timeAsDouble > this.startleAnimationCompletionTime)
				{
					this.isPlayingStartleAnimation = false;
				}
			}
			else if (this.isPlayingAttack)
			{
				if (Time.timeAsDouble - this.lastAttack > (double)this.attackDuration)
				{
					this.isPlayingAttack = false;
				}
			}
			else if (this.asset.shouldPlayAnimsOnDedicatedServer)
			{
				if (this.isRunning && this.hasRunAnimation)
				{
					this.animator.Play("Run");
				}
				else if (this.isMoving && this.hasWalkAnimation)
				{
					this.animator.Play("Walk");
				}
				else if (this.hasIdleAnimation)
				{
					this.animator.Play("Idle");
				}
			}
			if (Provider.isServer && this.health < this.asset.health && Time.timeAsDouble - this.lastRegen > (double)this.asset.regen)
			{
				this.lastRegen = Time.timeAsDouble;
				this.health += 1;
			}
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x000444E4 File Offset: 0x000426E4
		public void tick()
		{
			float num = (float)(Time.timeAsDouble - this.lastTick);
			this.lastTick = Time.timeAsDouble;
			this.undergroundTestTimer -= num;
			if (this.undergroundTestTimer < 0f)
			{
				this.undergroundTestTimer = Random.Range(30f, 60f);
				if (!UndergroundAllowlist.IsPositionWithinValidHeight(base.transform.position, 0.1f))
				{
					AnimalManager.TeleportAnimalBackIntoMap(this);
					return;
				}
			}
			if (this.isHunting)
			{
				if (this.currentTargetPlayer != null && this.currentTargetPlayer.life.IsAlive && this.currentTargetPlayer.stance.stance != EPlayerStance.SWIM)
				{
					this.target = this.currentTargetPlayer.transform.position;
					float num2 = MathfEx.HorizontalDistanceSquared(this.target, base.transform.position);
					float num3 = Mathf.Abs(this.target.y - base.transform.position.y);
					if (num2 < ((this.currentTargetPlayer.movement.getVehicle() != null) ? this.asset.horizontalVehicleAttackRangeSquared : this.asset.horizontalAttackRangeSquared) && num3 < this.asset.verticalAttackRange)
					{
						if (Time.timeAsDouble - this.lastTarget > 0.30000001192092896)
						{
							if (this.isAttacking)
							{
								if (Time.timeAsDouble - this.lastAttack > (double)(this.attackDuration * 0.5f))
								{
									this.isAttacking = false;
									byte b = this.asset.damage;
									b = (byte)((float)b * Provider.modeConfigData.Animals.Damage_Multiplier);
									if (this.currentTargetPlayer.movement.getVehicle() != null)
									{
										if (this.currentTargetPlayer.movement.getVehicle().asset != null && this.currentTargetPlayer.movement.getVehicle().asset.isVulnerableToEnvironment)
										{
											VehicleManager.damage(this.currentTargetPlayer.movement.getVehicle(), (float)b, 1f, true, default(CSteamID), EDamageOrigin.Animal_Attack);
										}
									}
									else
									{
										EPlayerKill eplayerKill;
										DamageTool.damagePlayer(new DamagePlayerParameters(this.currentTargetPlayer)
										{
											cause = EDeathCause.ANIMAL,
											killer = Provider.server,
											direction = (this.target - base.transform.position).normalized,
											damage = (float)b,
											respectArmor = true
										}, out eplayerKill);
									}
								}
							}
							else if (Time.timeAsDouble - this.lastAttack > (double)this.attackInterval)
							{
								this.isAttacking = true;
								AnimalManager.sendAnimalAttack(this);
							}
						}
					}
					else if (num2 > 4096f)
					{
						this.currentTargetPlayer = null;
						this.isHunting = false;
						this.updateTicking();
					}
					else
					{
						this.lastTarget = Time.timeAsDouble;
						this.isAttacking = false;
					}
				}
				else
				{
					this.currentTargetPlayer = null;
					this.isHunting = false;
					this.updateTicking();
				}
				this.lastWander = Time.timeAsDouble;
			}
			this.move(num);
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x00044800 File Offset: 0x00042A00
		public void init()
		{
			this._asset = (Assets.find(EAssetType.ANIMAL, this.id) as AnimalAsset);
			this.attackDuration = 0.5f;
			this.attackInterval = this.asset.attackInterval;
			this.eatTime = 0.5f;
			this.glanceTime = 0.5f;
			if (this.asset.shouldPlayAnimsOnDedicatedServer)
			{
				this.animator = base.transform.Find("Character").GetComponent<Animation>();
				this.skeleton = this.animator.transform.Find("Skeleton");
				if (this.animator.transform.Find("Model_0") != null)
				{
					this.renderer_0 = this.animator.transform.Find("Model_0").GetComponent<Renderer>();
				}
				if (this.animator.transform.Find("Model_1"))
				{
					this.renderer_1 = this.animator.transform.Find("Model_1").GetComponent<Renderer>();
				}
				if (this.animator != null)
				{
					this.hasIdleAnimation = (this.animator.GetClip("Idle") != null);
					this.hasRunAnimation = (this.animator.GetClip("Run") != null);
					this.hasWalkAnimation = (this.animator.GetClip("Walk") != null);
				}
			}
			if (Provider.isServer)
			{
				this.controller = base.GetComponent<CharacterController>();
				if (this.controller != null)
				{
					this.controller.enableOverlapRecovery = false;
				}
				else
				{
					Assets.reportError(this.asset, "missing CharacterController component");
				}
			}
			else
			{
				this.nsb = new NetworkSnapshotBuffer<YawSnapshotInfo>(Provider.UPDATE_TIME, Provider.UPDATE_DELAY);
			}
			this.reset();
			this.lastEat = Time.timeAsDouble + (double)Random.Range(4f, 16f);
			this.lastGlance = Time.timeAsDouble + (double)Random.Range(4f, 16f);
			this.lastWander = Time.timeAsDouble + (double)Random.Range(8f, 32f);
			this.eatDelay = Random.Range(4f, 8f);
			this.glanceDelay = Random.Range(4f, 8f);
			this.wanderDelay = Random.Range(8f, 16f);
			this.updateLife();
			this.updateStates();
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x00044A72 File Offset: 0x00042C72
		[Obsolete("Renamed to PlayStartleAnimation")]
		public void askStartle()
		{
			this.PlayStartleAnimation();
		}

		// Token: 0x040005EE RID: 1518
		private Animation animator;

		// Token: 0x040005EF RID: 1519
		private Transform skeleton;

		// Token: 0x040005F0 RID: 1520
		private Renderer renderer_0;

		// Token: 0x040005F1 RID: 1521
		private Renderer renderer_1;

		// Token: 0x040005F2 RID: 1522
		private double lastEat;

		// Token: 0x040005F3 RID: 1523
		private double lastGlance;

		// Token: 0x040005F4 RID: 1524
		private double startleAnimationCompletionTime;

		// Token: 0x040005F5 RID: 1525
		private double lastWander;

		// Token: 0x040005F6 RID: 1526
		private double lastStuck;

		// Token: 0x040005F7 RID: 1527
		private double lastTarget;

		// Token: 0x040005F8 RID: 1528
		private double lastAttack;

		// Token: 0x040005F9 RID: 1529
		private double lastRegen;

		// Token: 0x040005FA RID: 1530
		private float eatTime;

		// Token: 0x040005FB RID: 1531
		private float glanceTime;

		// Token: 0x040005FC RID: 1532
		private float attackDuration;

		// Token: 0x040005FD RID: 1533
		private float attackInterval;

		// Token: 0x040005FE RID: 1534
		private double startedRoar;

		// Token: 0x040005FF RID: 1535
		private double startedPanic;

		// Token: 0x04000600 RID: 1536
		private float eatDelay;

		// Token: 0x04000601 RID: 1537
		private float glanceDelay;

		// Token: 0x04000602 RID: 1538
		private float wanderDelay;

		// Token: 0x04000603 RID: 1539
		private bool isPlayingEat;

		// Token: 0x04000604 RID: 1540
		private bool isPlayingGlance;

		// Token: 0x04000605 RID: 1541
		private bool isPlayingStartleAnimation;

		// Token: 0x04000606 RID: 1542
		private bool isPlayingAttack;

		// Token: 0x04000607 RID: 1543
		private Player currentTargetPlayer;

		// Token: 0x04000609 RID: 1545
		private Vector3 lastUpdatePos;

		// Token: 0x0400060A RID: 1546
		private float lastUpdateAngle;

		// Token: 0x0400060B RID: 1547
		private NetworkSnapshotBuffer<YawSnapshotInfo> nsb;

		// Token: 0x0400060C RID: 1548
		private bool isMoving;

		// Token: 0x0400060D RID: 1549
		private bool isRunning;

		// Token: 0x0400060E RID: 1550
		private bool isTicking;

		// Token: 0x0400060F RID: 1551
		private bool _isFleeing;

		// Token: 0x04000610 RID: 1552
		private bool isWandering;

		// Token: 0x04000612 RID: 1554
		private bool isStuck;

		// Token: 0x04000613 RID: 1555
		private bool isAttacking;

		// Token: 0x04000614 RID: 1556
		private float _lastDead;

		// Token: 0x04000615 RID: 1557
		public bool isDead;

		// Token: 0x04000616 RID: 1558
		public ushort index;

		// Token: 0x04000617 RID: 1559
		public ushort id;

		// Token: 0x04000618 RID: 1560
		public PackInfo pack;

		// Token: 0x04000619 RID: 1561
		private ushort health;

		// Token: 0x0400061A RID: 1562
		private Vector3 ragdoll;

		// Token: 0x0400061B RID: 1563
		private AnimalAsset _asset;

		// Token: 0x0400061C RID: 1564
		private CharacterController controller;

		/// <summary>
		/// Whether this animal was updated in this network tick and needs to be resent.
		/// </summary>
		// Token: 0x0400061D RID: 1565
		public bool isUpdated;

		/// <summary>
		/// Reduces frequency of UndergroundAllowlist checks because it can be expensive with lots of entities and volumes. 
		/// </summary>
		// Token: 0x0400061E RID: 1566
		private float undergroundTestTimer = 10f;

		// Token: 0x0400061F RID: 1567
		private double lastTick;

		// Token: 0x04000620 RID: 1568
		private bool hasIdleAnimation;

		// Token: 0x04000621 RID: 1569
		private bool hasRunAnimation;

		// Token: 0x04000622 RID: 1570
		private bool hasWalkAnimation;
	}
}
