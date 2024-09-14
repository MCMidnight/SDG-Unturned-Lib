using System;
using System.Collections.Generic;
using SDG.Framework.Water;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200065E RID: 1630
	public class PlayerStance : PlayerCaller
	{
		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x0600360D RID: 13837 RVA: 0x000F93E3 File Offset: 0x000F75E3
		// (set) Token: 0x0600360E RID: 13838 RVA: 0x000F93EB File Offset: 0x000F75EB
		public EPlayerStance stance
		{
			get
			{
				return this._stance;
			}
			set
			{
				this.checkStance(value, true);
			}
		}

		/// <summary>
		/// Invoked after any player's stance changes (not including loading).
		/// </summary>
		// Token: 0x140000CB RID: 203
		// (add) Token: 0x0600360F RID: 13839 RVA: 0x000F93F8 File Offset: 0x000F75F8
		// (remove) Token: 0x06003610 RID: 13840 RVA: 0x000F942C File Offset: 0x000F762C
		public static event Action<PlayerStance> OnStanceChanged_Global;

		/// <returns>Distance zombies can detect this player within.</returns>
		// Token: 0x06003611 RID: 13841 RVA: 0x000F9460 File Offset: 0x000F7660
		public float GetStealthDetectionRadius()
		{
			if (base.player.movement.nav != 255 && ZombieManager.regions[(int)base.player.movement.nav].isHyper)
			{
				return 24f;
			}
			if (this.stance == EPlayerStance.DRIVING)
			{
				if (base.player.movement.getVehicle().sirensOn)
				{
					return PlayerStance.DETECT_FORWARD;
				}
				return PlayerStance.DETECT_FORWARD * base.player.movement.getVehicle().GetReplicatedForwardSpeedPercentageOfTargetSpeed();
			}
			else
			{
				if (this.stance == EPlayerStance.SITTING)
				{
					return 0f;
				}
				if (this.stance == EPlayerStance.SPRINT)
				{
					return PlayerStance.DETECT_SPRINT * (base.player.movement.isMoving ? PlayerStance.DETECT_MOVE : 1f);
				}
				if (this.stance == EPlayerStance.STAND || this.stance == EPlayerStance.SWIM)
				{
					float num = 1f - base.player.skills.mastery(1, 0) * 0.5f;
					return PlayerStance.DETECT_STAND * (base.player.movement.isMoving ? PlayerStance.DETECT_MOVE : 1f) * num;
				}
				float num2 = 1f - base.player.skills.mastery(1, 0) * 0.75f;
				if (this.stance == EPlayerStance.CROUCH || this.stance == EPlayerStance.CLIMB)
				{
					return PlayerStance.DETECT_CROUCH * (base.player.movement.isMoving ? PlayerStance.DETECT_MOVE : 1f) * num2;
				}
				if (this.stance == EPlayerStance.PRONE)
				{
					return PlayerStance.DETECT_PRONE * (base.player.movement.isMoving ? PlayerStance.DETECT_MOVE : 1f) * num2;
				}
				return 0f;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06003612 RID: 13842 RVA: 0x000F960A File Offset: 0x000F780A
		[Obsolete("Renamed to GetStealthDetectionRadius.")]
		public float radius
		{
			get
			{
				return this.GetStealthDetectionRadius();
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06003613 RID: 13843 RVA: 0x000F9612 File Offset: 0x000F7812
		public bool crouch
		{
			get
			{
				return this._localWantsToCrouch;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06003614 RID: 13844 RVA: 0x000F961A File Offset: 0x000F781A
		public bool prone
		{
			get
			{
				return this._localWantsToProne;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06003615 RID: 13845 RVA: 0x000F9622 File Offset: 0x000F7822
		public bool sprint
		{
			get
			{
				return this._localWantsToSprint;
			}
		}

		/// <summary>
		/// Older, cached version of areEyesUnderwater.
		/// </summary>
		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06003616 RID: 13846 RVA: 0x000F962A File Offset: 0x000F782A
		public bool isSubmerged
		{
			get
			{
				return this._isSubmerged;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06003617 RID: 13847 RVA: 0x000F9632 File Offset: 0x000F7832
		internal bool canCurrentStanceTransitionToClimbing
		{
			get
			{
				return this.stance == EPlayerStance.STAND || this.stance == EPlayerStance.SPRINT || this.stance == EPlayerStance.SWIM;
			}
		}

		/// <summary>
		/// Return false if there are any external restrictions (e.g. reloading, handcuffed) preventing climbing.
		/// </summary>
		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06003618 RID: 13848 RVA: 0x000F9651 File Offset: 0x000F7851
		internal bool isAllowedToStartClimbing
		{
			get
			{
				return !base.player.equipment.isBusy && base.player.animator.gesture != EPlayerGesture.ARREST_START;
			}
		}

		/// <summary>
		/// Test whether bottom of controller is currently inside a water volume.
		/// </summary>
		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06003619 RID: 13849 RVA: 0x000F967E File Offset: 0x000F787E
		public bool areFeetUnderwater
		{
			get
			{
				return WaterUtility.isPointUnderwater(base.transform.position);
			}
		}

		/// <summary>
		/// Test whether viewpoint is currently inside a water volume.
		/// </summary>
		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x0600361A RID: 13850 RVA: 0x000F9690 File Offset: 0x000F7890
		public bool areEyesUnderwater
		{
			get
			{
				return WaterUtility.isPointUnderwater(base.player.look.aim.position);
			}
		}

		/// <summary>
		/// Test whether body is currently inside a water volume.
		/// Enters the swimming stance while true.
		/// </summary>
		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x0600361B RID: 13851 RVA: 0x000F96AC File Offset: 0x000F78AC
		public bool isBodyUnderwater
		{
			get
			{
				return WaterUtility.isPointUnderwater(base.transform.position + new Vector3(0f, 1.25f, 0f));
			}
		}

		/// <summary>
		/// Draw debug capsule matching the player size.
		/// </summary>
		// Token: 0x0600361C RID: 13852 RVA: 0x000F96D8 File Offset: 0x000F78D8
		public static void drawCapsule(Vector3 position, float height, Color color, float lifespan = 0f)
		{
			Vector3 begin = position + new Vector3(0f, PlayerStance.RADIUS, 0f);
			Vector3 end = position + new Vector3(0f, height - PlayerStance.RADIUS, 0f);
			RuntimeGizmos.Get().Capsule(begin, end, PlayerStance.RADIUS, color, lifespan, EGizmoLayer.World);
		}

		/// <summary>
		/// Draw standing-height debug capsule matching the player size.
		/// </summary>
		// Token: 0x0600361D RID: 13853 RVA: 0x000F9734 File Offset: 0x000F7934
		public static void drawStandingCapsule(Vector3 position, Color color, float lifespan = 0f)
		{
			Vector3 begin = position + new Vector3(0f, PlayerStance.RADIUS, 0f);
			Vector3 end = position + new Vector3(0f, PlayerMovement.HEIGHT_STAND - PlayerStance.RADIUS, 0f);
			RuntimeGizmos.Get().Capsule(begin, end, PlayerStance.RADIUS, color, lifespan, EGizmoLayer.World);
		}

		/// <summary>
		/// Is there enough height for our capsule at a position?
		/// </summary>
		// Token: 0x0600361E RID: 13854 RVA: 0x000F9794 File Offset: 0x000F7994
		public static bool hasHeightClearanceAtPosition(Vector3 position, float height)
		{
			Vector3 start = position + new Vector3(0f, PlayerStance.RADIUS + 0.01f, 0f);
			Vector3 end = position + new Vector3(0f, height - PlayerStance.RADIUS, 0f);
			return !Physics.CheckCapsule(start, end, PlayerStance.RADIUS, RayMasks.BLOCK_STANCE & -21, QueryTriggerInteraction.Ignore) && !Physics.Linecast(position + new Vector3(0f, height, 0f), position + new Vector3(0f, 0.01f, 0f), 1048576);
		}

		/// <summary>
		/// Could a standing player capsule fit at the given position?
		/// </summary>
		// Token: 0x0600361F RID: 13855 RVA: 0x000F9835 File Offset: 0x000F7A35
		public static bool hasStandingHeightClearanceAtPosition(Vector3 position)
		{
			return PlayerStance.hasHeightClearanceAtPosition(position, PlayerMovement.HEIGHT_STAND);
		}

		/// <summary>
		/// Could a crouching player capsule fit at the given position?
		/// </summary>
		// Token: 0x06003620 RID: 13856 RVA: 0x000F9842 File Offset: 0x000F7A42
		public static bool hasCrouchingHeightClearanceAtPosition(Vector3 position)
		{
			return PlayerStance.hasHeightClearanceAtPosition(position, PlayerMovement.HEIGHT_CROUCH);
		}

		/// <summary>
		/// Could a prone player capsule fit at the given position?
		/// </summary>
		// Token: 0x06003621 RID: 13857 RVA: 0x000F984F File Offset: 0x000F7A4F
		public static bool hasProneHeightClearanceAtPosition(Vector3 position)
		{
			return PlayerStance.hasHeightClearanceAtPosition(position, PlayerMovement.HEIGHT_PRONE);
		}

		/// <summary>
		/// Could a standing player capsule teleport to the given position?
		/// </summary>
		// Token: 0x06003622 RID: 13858 RVA: 0x000F985C File Offset: 0x000F7A5C
		public static bool hasTeleportClearanceAtPosition(Vector3 position)
		{
			return PlayerStance.hasHeightClearanceAtPosition(position, PlayerMovement.HEIGHT_STAND + 0.5f);
		}

		/// <summary>
		/// Is there any compatible stance that can fit at position?
		/// </summary>
		// Token: 0x06003623 RID: 13859 RVA: 0x000F986F File Offset: 0x000F7A6F
		public static bool getStanceForPosition(Vector3 position, ref EPlayerStance stance)
		{
			if (PlayerStance.hasStandingHeightClearanceAtPosition(position))
			{
				stance = EPlayerStance.STAND;
				return true;
			}
			if (PlayerStance.hasCrouchingHeightClearanceAtPosition(position))
			{
				stance = EPlayerStance.CROUCH;
				return true;
			}
			if (PlayerStance.hasProneHeightClearanceAtPosition(position))
			{
				stance = EPlayerStance.PRONE;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Using our capsule's current height would there be enough space at a given position?
		/// </summary>
		// Token: 0x06003624 RID: 13860 RVA: 0x000F989C File Offset: 0x000F7A9C
		public bool wouldHaveHeightClearanceAtPosition(Vector3 position, float padding = 0f)
		{
			CharacterController controller = base.player.movement.controller;
			float num = (controller != null) ? controller.height : PlayerMovement.HEIGHT_STAND;
			return PlayerStance.hasHeightClearanceAtPosition(position, num + padding);
		}

		/// <summary>
		/// Does capsule have appropriate clearance for a pending height change?
		/// </summary>
		// Token: 0x06003625 RID: 13861 RVA: 0x000F98DA File Offset: 0x000F7ADA
		public bool hasHeightClearance(float height)
		{
			return PlayerStance.hasHeightClearanceAtPosition(base.transform.position, height);
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000F98ED File Offset: 0x000F7AED
		private EPlayerHeight getHeightForStance(EPlayerStance testStance)
		{
			if (testStance == EPlayerStance.CROUCH)
			{
				return EPlayerHeight.CROUCH;
			}
			if (testStance == EPlayerStance.PRONE)
			{
				return EPlayerHeight.PRONE;
			}
			return EPlayerHeight.STAND;
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x000F98FC File Offset: 0x000F7AFC
		internal void internalSetStance(EPlayerStance newStance)
		{
			if (this._stance != newStance)
			{
				this._stance = newStance;
				EPlayerHeight heightForStance = this.getHeightForStance(newStance);
				base.player.movement.setSize(heightForStance);
				StanceUpdated stanceUpdated = this.onStanceUpdated;
				if (stanceUpdated == null)
				{
					return;
				}
				stanceUpdated();
			}
		}

		/// <summary>
		/// Replicate stance to clients.
		/// </summary>
		// Token: 0x06003628 RID: 13864 RVA: 0x000F9944 File Offset: 0x000F7B44
		private void replicateStance(bool notifyOwner)
		{
			if (notifyOwner)
			{
				PlayerStance.SendStance.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), this.stance);
				return;
			}
			PlayerStance.SendStance.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GatherRemoteClientConnectionsExcludingOwner(), this.stance);
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x000F9993 File Offset: 0x000F7B93
		public void checkStance(EPlayerStance newStance)
		{
			this.checkStance(newStance, false);
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x000F99A0 File Offset: 0x000F7BA0
		public void checkStance(EPlayerStance newStance, bool all)
		{
			if (base.player.movement.getVehicle() != null && newStance != EPlayerStance.DRIVING && newStance != EPlayerStance.SITTING)
			{
				return;
			}
			if (newStance == this.stance)
			{
				return;
			}
			if ((newStance == EPlayerStance.PRONE || newStance == EPlayerStance.CROUCH) && this._inShallows)
			{
				return;
			}
			if ((this.stance == EPlayerStance.CROUCH && newStance == EPlayerStance.STAND) || (this.stance == EPlayerStance.PRONE && (newStance == EPlayerStance.CROUCH || newStance == EPlayerStance.STAND)))
			{
				if (Time.realtimeSinceStartup - this.lastStance <= PlayerStance.COOLDOWN)
				{
					return;
				}
				this.lastStance = Time.realtimeSinceStartup;
			}
			if (newStance != EPlayerStance.DRIVING && newStance != EPlayerStance.SITTING)
			{
				EPlayerHeight height = base.player.movement.height;
				EPlayerHeight heightForStance = this.getHeightForStance(newStance);
				if (heightForStance != height)
				{
					if (heightForStance == EPlayerHeight.STAND)
					{
						if (!this.hasHeightClearance(PlayerMovement.HEIGHT_STAND))
						{
							return;
						}
					}
					else if (heightForStance == EPlayerHeight.CROUCH && height == EPlayerHeight.PRONE && !this.hasHeightClearance(PlayerMovement.HEIGHT_CROUCH))
					{
						return;
					}
				}
			}
			if (Provider.isServer)
			{
				if (base.player.animator.gesture == EPlayerGesture.INVENTORY_START)
				{
					if (newStance != EPlayerStance.STAND && newStance != EPlayerStance.SPRINT && newStance != EPlayerStance.CROUCH)
					{
						base.player.animator.sendGesture(EPlayerGesture.INVENTORY_STOP, false);
					}
				}
				else if (base.player.animator.gesture == EPlayerGesture.SURRENDER_START)
				{
					base.player.animator.sendGesture(EPlayerGesture.SURRENDER_STOP, true);
				}
				else if (base.player.animator.gesture == EPlayerGesture.REST_START)
				{
					base.player.animator.sendGesture(EPlayerGesture.REST_STOP, true);
				}
			}
			this.internalSetStance(newStance);
			if (Provider.isServer)
			{
				this.replicateStance(all);
			}
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x000F9B14 File Offset: 0x000F7D14
		public bool adjustStanceOrTeleportIfStuck()
		{
			EPlayerStance stance = this.stance;
			if (PlayerStance.getStanceForPosition(base.transform.position, ref stance))
			{
				this.internalSetStance(stance);
				this.replicateStance(true);
				return true;
			}
			return base.player.teleportToRandomSpawnPoint();
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x000F9B58 File Offset: 0x000F7D58
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2)]
		public void ReceiveClimbRequest(in ServerInvocationContext context, Vector3 direction)
		{
			if (!this.canCurrentStanceTransitionToClimbing)
			{
				return;
			}
			if (!this.isAllowedToStartClimbing)
			{
				return;
			}
			RaycastHit raycastHit;
			if (!Physics.SphereCast(new Ray(base.player.look.aim.position, direction), PlayerStance.RADIUS, out raycastHit, 4f, RayMasks.LADDER_INTERACT) || raycastHit.collider == null)
			{
				return;
			}
			if (!raycastHit.collider.CompareTag("Ladder"))
			{
				return;
			}
			RaycastHit raycastHit2;
			if (!Physics.Raycast(new Ray(base.player.look.aim.position, direction), out raycastHit2, 4f, RayMasks.LADDER_INTERACT) || raycastHit2.collider == null)
			{
				return;
			}
			if (!raycastHit2.collider.CompareTag("Ladder"))
			{
				return;
			}
			float num = Vector3.Dot(raycastHit2.normal, raycastHit2.collider.transform.up);
			if (Mathf.Abs(num) <= 0.9f)
			{
				return;
			}
			if (Mathf.Abs(Vector3.Dot(Vector3.up, raycastHit2.collider.transform.up)) > 0.1f)
			{
				return;
			}
			Vector3 vector = new Vector3(raycastHit2.collider.transform.position.x, raycastHit2.point.y - 0.5f - 0.5f - 0.1f, raycastHit2.collider.transform.position.z) + raycastHit2.normal * 0.65f;
			float num2 = PlayerMovement.HEIGHT_STAND + 0.1f + 0.5f;
			Vector3 end = vector + new Vector3(0f, num2 * 0.5f, 0f);
			RaycastHit raycastHit3;
			if (Physics.Linecast(raycastHit2.point, end, out raycastHit3, RayMasks.BLOCK_STANCE, QueryTriggerInteraction.Collide))
			{
				return;
			}
			if (!PlayerStance.hasHeightClearanceAtPosition(vector, num2))
			{
				return;
			}
			float num3 = raycastHit2.collider.transform.rotation.eulerAngles.y;
			if (num < 0f)
			{
				num3 += 180f;
			}
			base.player.teleportToLocation(vector, num3);
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000F9D72 File Offset: 0x000F7F72
		[Obsolete]
		public void tellStance(CSteamID steamID, byte newStance)
		{
			this.ReceiveStance((EPlayerStance)newStance);
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000F9D7C File Offset: 0x000F7F7C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellStance")]
		public void ReceiveStance(EPlayerStance newStance)
		{
			this.internalSetStance(newStance);
			if (this.stance == EPlayerStance.CROUCH)
			{
				if (ControlsSettings.crouching == EControlMode.TOGGLE)
				{
					this._localWantsToCrouch = true;
					this._localWantsToProne = false;
				}
			}
			else if (this.stance == EPlayerStance.PRONE && ControlsSettings.proning == EControlMode.TOGGLE)
			{
				this._localWantsToCrouch = false;
				this._localWantsToProne = true;
			}
			Action<PlayerStance> onStanceChanged_Global = PlayerStance.OnStanceChanged_Global;
			if (onStanceChanged_Global == null)
			{
				return;
			}
			onStanceChanged_Global.Invoke(this);
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000F9DE0 File Offset: 0x000F7FE0
		[Obsolete]
		public void askStance(CSteamID steamID)
		{
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000F9DE2 File Offset: 0x000F7FE2
		internal void SendInitialPlayerState(SteamPlayer client)
		{
			PlayerStance.SendStance.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, this.stance);
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000F9E01 File Offset: 0x000F8001
		internal void SendInitialPlayerState(List<ITransportConnection> transportConnections)
		{
			PlayerStance.SendStance.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, this.stance);
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x000F9E1C File Offset: 0x000F801C
		public void simulate(uint simulation, bool inputCrouch, bool inputProne, bool inputSprint)
		{
			this._isSubmerged = this.areEyesUnderwater;
			this._inShallows = this.areFeetUnderwater;
			if (this.stance == EPlayerStance.CLIMB || (this.canCurrentStanceTransitionToClimbing && this.isAllowedToStartClimbing))
			{
				Physics.Raycast(new Ray(base.transform.position + Vector3.up * 0.5f, base.transform.forward), out this.ladder, 0.75f, RayMasks.LADDER_INTERACT);
				if (this.ladder.collider != null && this.ladder.collider.transform.CompareTag("Ladder") && Mathf.Abs(Vector3.Dot(this.ladder.normal, this.ladder.collider.transform.up)) > 0.9f)
				{
					if (this.stance != EPlayerStance.CLIMB)
					{
						Vector3 vector = new Vector3(this.ladder.collider.transform.position.x, this.ladder.point.y - 0.5f, this.ladder.collider.transform.position.z) + this.ladder.normal * 0.5f;
						if (!Physics.CapsuleCast(base.transform.position + new Vector3(0f, PlayerStance.RADIUS, 0f), base.transform.position + new Vector3(0f, PlayerMovement.HEIGHT_STAND - PlayerStance.RADIUS, 0f), PlayerStance.RADIUS, (vector - base.transform.position).normalized, (vector - base.transform.position).magnitude, RayMasks.BLOCK_LADDER, QueryTriggerInteraction.Ignore) && !Physics.CheckCapsule(vector + new Vector3(0f, PlayerStance.RADIUS, 0f), vector + new Vector3(0f, PlayerMovement.HEIGHT_STAND - PlayerStance.RADIUS, 0f), PlayerStance.RADIUS, RayMasks.BLOCK_LADDER, QueryTriggerInteraction.Ignore))
						{
							base.transform.position = vector;
							this.checkStance(EPlayerStance.CLIMB);
						}
					}
					if (this.stance == EPlayerStance.CLIMB)
					{
						return;
					}
				}
				else if (this.stance == EPlayerStance.CLIMB)
				{
					this.checkStance(EPlayerStance.STAND);
				}
			}
			if (this.isBodyUnderwater)
			{
				if (this.stance != EPlayerStance.SWIM)
				{
					this.checkStance(EPlayerStance.SWIM);
				}
				return;
			}
			if (this._inShallows)
			{
				if (this.stance != EPlayerStance.STAND && this.stance != EPlayerStance.SPRINT)
				{
					this.checkStance(EPlayerStance.STAND);
				}
			}
			else if (this.stance == EPlayerStance.SWIM)
			{
				this.checkStance(EPlayerStance.STAND);
			}
			if (this.stance != EPlayerStance.CLIMB && this.stance != EPlayerStance.SITTING && this.stance != EPlayerStance.DRIVING)
			{
				if (inputCrouch || (base.player.animator.gesture == EPlayerGesture.REST_START && !inputProne))
				{
					if (this.stance != EPlayerStance.CROUCH)
					{
						this.checkStance(EPlayerStance.CROUCH);
					}
				}
				else if (inputProne)
				{
					if (this.stance != EPlayerStance.PRONE)
					{
						this.checkStance(EPlayerStance.PRONE);
					}
				}
				else if (this.stance == EPlayerStance.CROUCH || this.stance == EPlayerStance.PRONE)
				{
					this.checkStance(EPlayerStance.STAND);
				}
				bool flag = true;
				UseableGun useableGun = base.player.equipment.useable as UseableGun;
				if (useableGun != null)
				{
					ItemGunAsset itemGunAsset = base.player.equipment.asset as ItemGunAsset;
					if (itemGunAsset != null)
					{
						flag = (itemGunAsset.canAimDuringSprint || !useableGun.isAiming);
					}
				}
				if (inputSprint && !base.player.life.isBroken && base.player.life.stamina > 0 && flag && base.player.movement.isMoving)
				{
					if (this.stance == EPlayerStance.STAND)
					{
						this.checkStance(EPlayerStance.SPRINT);
						return;
					}
				}
				else if (this.stance == EPlayerStance.SPRINT)
				{
					this.checkStance(EPlayerStance.STAND);
				}
			}
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000FA203 File Offset: 0x000F8403
		private void onLifeUpdated(bool isDead)
		{
			if (!isDead)
			{
				this.checkStance(EPlayerStance.STAND);
			}
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x000FA210 File Offset: 0x000F8410
		private void Update()
		{
			if (base.channel.IsLocalPlayer && !PlayerUI.window.showCursor)
			{
				if (!base.player.look.IsControllingFreecam)
				{
					if (InputEx.GetKey(ControlsSettings.stance))
					{
						if (this.isHolding)
						{
							if (Time.realtimeSinceStartup - this.lastHold > 0.33f)
							{
								this._localWantsToCrouch = false;
								this._localWantsToProne = true;
							}
						}
						else
						{
							this.isHolding = true;
							this.lastHold = Time.realtimeSinceStartup;
						}
					}
					else if (this.isHolding)
					{
						if (Time.realtimeSinceStartup - this.lastHold < 0.33f)
						{
							if (this.crouch)
							{
								this._localWantsToCrouch = false;
								this._localWantsToProne = false;
							}
							else
							{
								this._localWantsToCrouch = true;
								this._localWantsToProne = false;
							}
						}
						this.isHolding = false;
					}
					if (base.player.animator.gesture == EPlayerGesture.REST_START)
					{
						if (InputEx.GetKeyDown(ControlsSettings.crouch))
						{
							base.player.animator.sendGesture(EPlayerGesture.REST_STOP, true);
							this._localWantsToCrouch = true;
							if (this._localWantsToProne)
							{
								this._localWantsToProne = false;
							}
						}
					}
					else if (ControlsSettings.crouching == EControlMode.TOGGLE)
					{
						if (InputEx.GetKeyDown(ControlsSettings.crouch))
						{
							this._localWantsToCrouch = !this.crouch;
							if (this._localWantsToCrouch)
							{
								this._localWantsToProne = false;
							}
						}
					}
					else
					{
						this._localWantsToCrouch = InputEx.GetKey(ControlsSettings.crouch);
						if (this._localWantsToCrouch)
						{
							this._localWantsToProne = false;
						}
					}
					if (ControlsSettings.proning == EControlMode.TOGGLE)
					{
						if (InputEx.GetKeyDown(ControlsSettings.prone))
						{
							this._localWantsToProne = !this.prone;
							if (this._localWantsToProne)
							{
								this._localWantsToCrouch = false;
							}
						}
					}
					else
					{
						this._localWantsToProne = InputEx.GetKey(ControlsSettings.prone);
						if (this._localWantsToProne)
						{
							this._localWantsToCrouch = false;
						}
					}
					if (ControlsSettings.sprinting == EControlMode.TOGGLE)
					{
						if (InputEx.GetKeyDown(ControlsSettings.sprint))
						{
							this._localWantsToSprint = !this.sprint;
						}
					}
					else
					{
						this._localWantsToSprint = InputEx.GetKey(ControlsSettings.sprint);
					}
					this.localWantsToSteadyAim = InputEx.GetKey(ControlsSettings.sprint);
				}
				if ((this.stance == EPlayerStance.PRONE || this.stance == EPlayerStance.CROUCH) && InputEx.GetKey(ControlsSettings.jump))
				{
					this._localWantsToCrouch = false;
					this._localWantsToProne = false;
				}
				if (this._inShallows || this.stance == EPlayerStance.SWIM || this.stance == EPlayerStance.CLIMB || this.stance == EPlayerStance.SITTING || this.stance == EPlayerStance.DRIVING)
				{
					this._localWantsToCrouch = false;
					this._localWantsToProne = false;
				}
				if (ControlsSettings.sprinting == EControlMode.TOGGLE && base.player.movement.input_x == 0 && base.player.movement.input_y == 0)
				{
					this._localWantsToSprint = false;
				}
				if (PlayerUI.window.showCursor)
				{
					this._localWantsToSprint = false;
					this.localWantsToSteadyAim = false;
				}
			}
			if (Provider.isServer && (double)(Time.realtimeSinceStartup - this.lastDetect) > 0.1)
			{
				this.lastDetect = Time.realtimeSinceStartup;
				if (base.player.life.IsAlive)
				{
					AlertTool.alert(base.player, base.transform.position, this.GetStealthDetectionRadius(), this.stance != EPlayerStance.SPRINT && this.stance != EPlayerStance.DRIVING, base.player.look.aim.forward, base.player.isSpotOn);
				}
			}
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x000FA560 File Offset: 0x000F8760
		internal void InitializePlayer()
		{
			this._stance = EPlayerStance.STAND;
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				this.lastStance = 0f;
				PlayerLife life = base.player.life;
				life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
			}
			if (Provider.isServer)
			{
				this.internalSetStance(this.initialStance);
			}
		}

		// Token: 0x04001F14 RID: 7956
		public static readonly float COOLDOWN = 0.5f;

		// Token: 0x04001F15 RID: 7957
		public static readonly float RADIUS = 0.4f;

		// Token: 0x04001F16 RID: 7958
		public static readonly float DETECT_MOVE = 1.1f;

		// Token: 0x04001F17 RID: 7959
		public static readonly float DETECT_FORWARD = 48f;

		// Token: 0x04001F18 RID: 7960
		public static readonly float DETECT_BACKWARD = 24f;

		// Token: 0x04001F19 RID: 7961
		public static readonly float DETECT_SPRINT = 20f;

		// Token: 0x04001F1A RID: 7962
		public static readonly float DETECT_STAND = 12f;

		// Token: 0x04001F1B RID: 7963
		public static readonly float DETECT_CROUCH = 6f;

		// Token: 0x04001F1C RID: 7964
		public static readonly float DETECT_PRONE = 3f;

		// Token: 0x04001F1D RID: 7965
		public StanceUpdated onStanceUpdated;

		// Token: 0x04001F1E RID: 7966
		private EPlayerStance _stance;

		/// <summary>
		/// Stance to fit available space when loading in.
		/// </summary>
		// Token: 0x04001F20 RID: 7968
		public EPlayerStance initialStance = EPlayerStance.STAND;

		// Token: 0x04001F21 RID: 7969
		private float lastStance;

		// Token: 0x04001F22 RID: 7970
		private float lastDetect;

		// Token: 0x04001F23 RID: 7971
		private float lastHold;

		// Token: 0x04001F24 RID: 7972
		private bool isHolding;

		// Token: 0x04001F25 RID: 7973
		private bool _localWantsToCrouch;

		// Token: 0x04001F26 RID: 7974
		private bool _localWantsToProne;

		// Token: 0x04001F27 RID: 7975
		private bool _localWantsToSprint;

		// Token: 0x04001F28 RID: 7976
		internal bool localWantsToSteadyAim;

		// Token: 0x04001F29 RID: 7977
		private bool _isSubmerged;

		// Token: 0x04001F2A RID: 7978
		private bool _inShallows;

		// Token: 0x04001F2B RID: 7979
		private RaycastHit ladder;

		/// <summary>
		/// Regular interact ray still hits the ladder, but we only allow climbing within a smaller range to make its
		/// teleport less powerful.
		/// </summary>
		// Token: 0x04001F2C RID: 7980
		internal const float LADDER_INTERACT_RANGE = 4f;

		/// <summary>
		/// Ladder forward ray is 0.75m, so we move slightly less than that away from the ladder.
		/// </summary>
		// Token: 0x04001F2D RID: 7981
		internal const float LADDER_INTERACT_TELEPORT_OFFSET = 0.65f;

		// Token: 0x04001F2E RID: 7982
		internal static readonly ServerInstanceMethod<Vector3> SendClimbRequest = ServerInstanceMethod<Vector3>.Get(typeof(PlayerStance), "ReceiveClimbRequest");

		// Token: 0x04001F2F RID: 7983
		private static readonly ClientInstanceMethod<EPlayerStance> SendStance = ClientInstanceMethod<EPlayerStance>.Get(typeof(PlayerStance), "ReceiveStance");
	}
}
