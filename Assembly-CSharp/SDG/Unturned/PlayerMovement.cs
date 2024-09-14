using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Landscapes;
using SDG.Framework.Water;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000647 RID: 1607
	public class PlayerMovement : PlayerCaller
	{
		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x0600346F RID: 13423 RVA: 0x000F13F2 File Offset: 0x000EF5F2
		// (set) Token: 0x06003470 RID: 13424 RVA: 0x000F1400 File Offset: 0x000EF600
		public static bool forceTrustClient
		{
			get
			{
				return GameplayConfigData._forceTrustClient;
			}
			set
			{
				GameplayConfigData._forceTrustClient.value = value;
				UnturnedLog.info("Set ForceTrustClient to: " + PlayerMovement.forceTrustClient.ToString());
			}
		}

		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06003471 RID: 13425 RVA: 0x000F1434 File Offset: 0x000EF634
		// (remove) Token: 0x06003472 RID: 13426 RVA: 0x000F146C File Offset: 0x000EF66C
		public event PlayerNavChanged PlayerNavChanged;

		// Token: 0x06003473 RID: 13427 RVA: 0x000F14A1 File Offset: 0x000EF6A1
		private void TriggerPlayerNavChanged(byte oldNav, byte newNav)
		{
			if (this.PlayerNavChanged == null)
			{
				return;
			}
			this.PlayerNavChanged(this, oldNav, newNav);
		}

		/// <summary>
		/// Note: Only UpdateCharacterControllerEnabled should modify whether controller is enabled.
		/// (turning off and back on is fine though)
		/// </summary>
		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06003474 RID: 13428 RVA: 0x000F14BA File Offset: 0x000EF6BA
		// (set) Token: 0x06003475 RID: 13429 RVA: 0x000F14C2 File Offset: 0x000EF6C2
		public CharacterController controller { get; protected set; }

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003476 RID: 13430 RVA: 0x000F14CB File Offset: 0x000EF6CB
		public float totalGravityMultiplier
		{
			get
			{
				return this.itemGravityMultiplier * this.pluginGravityMultiplier;
			}
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x06003477 RID: 13431 RVA: 0x000F14DC File Offset: 0x000EF6DC
		public float totalSpeedMultiplier
		{
			get
			{
				float num = this.pluginSpeedMultiplier * base.player.clothing.movementSpeedMultiplier;
				ItemAsset asset = base.player.equipment.asset;
				float num2 = num * ((asset != null) ? asset.equipableMovementSpeedMultiplier : 1f);
				Useable useable = base.player.equipment.useable;
				return num2 * ((useable != null) ? useable.movementSpeedMultiplier : 1f);
			}
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06003478 RID: 13432 RVA: 0x000F1542 File Offset: 0x000EF742
		[Obsolete]
		public LandscapeHoleVolume landscapeHoleVolume
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06003479 RID: 13433 RVA: 0x000F1545 File Offset: 0x000EF745
		internal bool CanEnterTeleporter
		{
			get
			{
				return base.player.life.IsAlive && this.getVehicle() == null;
			}
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x000F1568 File Offset: 0x000EF768
		private void DoTeleport(Transform inputTransform, Transform outputTransform)
		{
			Vector3 position = inputTransform.InverseTransformPoint(base.transform.position);
			Quaternion localRotation = inputTransform.InverseTransformRotation(base.transform.rotation);
			base.transform.position = outputTransform.TransformPoint(position);
			float y = outputTransform.TransformRotation(localRotation).eulerAngles.y;
			base.player.look.TeleportYaw(y);
			Vector3 position2 = inputTransform.InverseTransformPoint(this.lastUpdatePos);
			this.lastUpdatePos = outputTransform.TransformPoint(position2);
			base.player.PostTeleport();
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x000F15F8 File Offset: 0x000EF7F8
		internal void EnterCollisionTeleporter(CollisionTeleporter teleporter)
		{
			Transform transform = teleporter.transform;
			Transform destinationTransform = teleporter.DestinationTransform;
			this.DoTeleport(transform, destinationTransform);
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x000F161C File Offset: 0x000EF81C
		internal void EnterTeleporterVolume(TeleporterEntranceVolume entrance, TeleporterExitVolume exit)
		{
			Transform transform = entrance.transform;
			Transform transform2 = exit.transform;
			this.DoTeleport(transform, transform2);
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x000F163F File Offset: 0x000EF83F
		internal void UpdateCharacterControllerEnabled()
		{
			if (this.controller != null)
			{
				this.controller.enabled = (this.vehicle == null && base.player.life.IsAlive);
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x0600347E RID: 13438 RVA: 0x000F167B File Offset: 0x000EF87B
		public bool isGrounded
		{
			get
			{
				return this._isGrounded;
			}
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x0600347F RID: 13439 RVA: 0x000F1683 File Offset: 0x000EF883
		// (set) Token: 0x06003480 RID: 13440 RVA: 0x000F168B File Offset: 0x000EF88B
		public bool isSafe
		{
			get
			{
				return this._isSafe;
			}
			set
			{
				this._isSafe = value;
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06003481 RID: 13441 RVA: 0x000F1694 File Offset: 0x000EF894
		// (set) Token: 0x06003482 RID: 13442 RVA: 0x000F169C File Offset: 0x000EF89C
		public bool isRadiated
		{
			get
			{
				return this._isRadiated;
			}
			set
			{
				this._isRadiated = value;
			}
		}

		/// <summary>
		/// Valid while isRadiated.
		/// </summary>
		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x06003483 RID: 13443 RVA: 0x000F16A5 File Offset: 0x000EF8A5
		// (set) Token: 0x06003484 RID: 13444 RVA: 0x000F16AD File Offset: 0x000EF8AD
		public IDeadzoneNode ActiveDeadzone { get; private set; }

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x06003485 RID: 13445 RVA: 0x000F16B6 File Offset: 0x000EF8B6
		// (set) Token: 0x06003486 RID: 13446 RVA: 0x000F16BE File Offset: 0x000EF8BE
		public HordePurchaseVolume purchaseNode
		{
			get
			{
				return this._purchaseNode;
			}
			set
			{
				this._purchaseNode = value;
			}
		}

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x06003487 RID: 13447 RVA: 0x000F16C7 File Offset: 0x000EF8C7
		// (set) Token: 0x06003488 RID: 13448 RVA: 0x000F16CF File Offset: 0x000EF8CF
		public IAmbianceNode effectNode { get; private set; }

		/// <summary>
		/// Set according to volume or level global asset fallback.
		/// </summary>
		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06003489 RID: 13449 RVA: 0x000F16D8 File Offset: 0x000EF8D8
		// (set) Token: 0x0600348A RID: 13450 RVA: 0x000F16E0 File Offset: 0x000EF8E0
		public uint WeatherMask { get; protected set; }

		// Token: 0x0600348B RID: 13451 RVA: 0x000F16E9 File Offset: 0x000EF8E9
		public void setSize(EPlayerHeight newHeight)
		{
			if (newHeight == this.height)
			{
				return;
			}
			this.height = newHeight;
			this.applySize();
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x000F1704 File Offset: 0x000EF904
		private void applySize()
		{
			float num;
			switch (this.height)
			{
			case EPlayerHeight.STAND:
				num = PlayerMovement.HEIGHT_STAND;
				break;
			case EPlayerHeight.CROUCH:
				num = PlayerMovement.HEIGHT_CROUCH;
				break;
			case EPlayerHeight.PRONE:
				num = PlayerMovement.HEIGHT_PRONE;
				break;
			default:
				num = 2f;
				break;
			}
			if ((base.channel.IsLocalPlayer || Provider.isServer) && this.controller != null)
			{
				this.controller.height = num;
				this.controller.center = new Vector3(0f, num / 2f, 0f);
				if (this.wasSizeAppliedYet)
				{
					base.transform.localPosition += new Vector3(0f, 0.02f, 0f);
				}
				this.wasSizeAppliedYet = true;
			}
		}

		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x0600348D RID: 13453 RVA: 0x000F17D2 File Offset: 0x000EF9D2
		public bool isMoving
		{
			get
			{
				return this._isMoving;
			}
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x0600348E RID: 13454 RVA: 0x000F17DC File Offset: 0x000EF9DC
		public float speed
		{
			get
			{
				if (base.player.stance.stance == EPlayerStance.SWIM)
				{
					return PlayerMovement.SPEED_SWIM * (1f + base.player.skills.mastery(0, 5) * 0.25f) * this.totalSpeedMultiplier;
				}
				float num = 1f + base.player.skills.mastery(0, 4) * 0.25f;
				if (base.player.stance.stance == EPlayerStance.CLIMB)
				{
					return PlayerMovement.SPEED_CLIMB * num * this.totalSpeedMultiplier;
				}
				if (base.player.stance.stance == EPlayerStance.SPRINT)
				{
					return PlayerMovement.SPEED_SPRINT * num * this.totalSpeedMultiplier;
				}
				if (base.player.stance.stance == EPlayerStance.STAND)
				{
					return PlayerMovement.SPEED_STAND * num * this.totalSpeedMultiplier;
				}
				if (base.player.stance.stance == EPlayerStance.CROUCH)
				{
					return PlayerMovement.SPEED_CROUCH * num * this.totalSpeedMultiplier;
				}
				if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					return PlayerMovement.SPEED_PRONE * num * this.totalSpeedMultiplier;
				}
				return 0f;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x0600348F RID: 13455 RVA: 0x000F18F5 File Offset: 0x000EFAF5
		public Vector3 move
		{
			get
			{
				return this._move;
			}
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06003490 RID: 13456 RVA: 0x000F18FD File Offset: 0x000EFAFD
		public byte region_x
		{
			get
			{
				return this._region_x;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06003491 RID: 13457 RVA: 0x000F1905 File Offset: 0x000EFB05
		public byte region_y
		{
			get
			{
				return this._region_y;
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06003492 RID: 13458 RVA: 0x000F190D File Offset: 0x000EFB0D
		public byte bound
		{
			get
			{
				return this._bound;
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06003493 RID: 13459 RVA: 0x000F1915 File Offset: 0x000EFB15
		public byte nav
		{
			get
			{
				return this._nav;
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06003494 RID: 13460 RVA: 0x000F191D File Offset: 0x000EFB1D
		public LoadedRegion[,] loadedRegions
		{
			get
			{
				return this._loadedRegions;
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06003495 RID: 13461 RVA: 0x000F1925 File Offset: 0x000EFB25
		public LoadedBound[] loadedBounds
		{
			get
			{
				return this._loadedBounds;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06003496 RID: 13462 RVA: 0x000F192D File Offset: 0x000EFB2D
		public float fall
		{
			get
			{
				return this.velocity.y;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06003497 RID: 13463 RVA: 0x000F193A File Offset: 0x000EFB3A
		[Obsolete]
		public Vector3 real
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06003498 RID: 13464 RVA: 0x000F1947 File Offset: 0x000EFB47
		public byte horizontal
		{
			get
			{
				return this._horizontal;
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06003499 RID: 13465 RVA: 0x000F194F File Offset: 0x000EFB4F
		public byte vertical
		{
			get
			{
				return this._vertical;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x0600349A RID: 13466 RVA: 0x000F1957 File Offset: 0x000EFB57
		public bool jump
		{
			get
			{
				return this._jump;
			}
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000F195F File Offset: 0x000EFB5F
		public InteractableVehicle getVehicle()
		{
			return this.vehicle;
		}

		/// <summary>
		/// Get seat (if any), otherwise null.
		/// </summary>
		// Token: 0x0600349C RID: 13468 RVA: 0x000F1968 File Offset: 0x000EFB68
		public Passenger getVehicleSeat()
		{
			if (!(this.vehicle != null) || this.vehicle.passengers == null || (int)this.seat >= this.vehicle.passengers.Length)
			{
				return null;
			}
			return this.vehicle.passengers[(int)this.seat];
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000F19B9 File Offset: 0x000EFBB9
		public byte getSeat()
		{
			return this.seat;
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000F19C4 File Offset: 0x000EFBC4
		private void updateVehicle()
		{
			InteractableVehicle interactableVehicle = this.vehicle;
			this.vehicle = this.pendingVehicle;
			this.seat = this.pendingSeatIndex;
			bool flag = this.vehicle != null && this.seat == 0;
			if (this.vehicle == null)
			{
				base.player.transform.parent = this.pendingSeatTransform;
				base.player.ReceiveTeleport(this.pendingSeatPosition, this.pendingSeatAngle);
			}
			if (base.channel.IsLocalPlayer)
			{
				bool flag2;
				if (flag && Level.info != null && Level.info.name.ToLower() != "tutorial" && Provider.provider.achievementsService.getAchievement("Wheel", out flag2) && !flag2)
				{
					Provider.provider.achievementsService.setAchievement("Wheel");
				}
				if (this.vehicle != null)
				{
					PlayerUI.disableDot();
					UseableGun useableGun = base.player.equipment.useable as UseableGun;
					if (useableGun != null)
					{
						useableGun.UpdateCrosshairEnabled();
					}
				}
				else
				{
					UseableGun useableGun2 = base.player.equipment.useable as UseableGun;
					if (useableGun2 != null)
					{
						useableGun2.UpdateCrosshairEnabled();
					}
					else
					{
						PlayerUI.enableDot();
					}
				}
			}
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				this.UpdateCharacterControllerEnabled();
				if (this.vehicle != null)
				{
					if (flag)
					{
						base.player.stance.checkStance(EPlayerStance.DRIVING);
					}
					else
					{
						base.player.stance.checkStance(EPlayerStance.SITTING);
					}
				}
				else
				{
					base.player.stance.checkStance(EPlayerStance.STAND);
				}
			}
			if (base.channel.IsLocalPlayer)
			{
				Seated seated = this.onSeated;
				if (seated != null)
				{
					seated(flag, this.vehicle != null, interactableVehicle != null, interactableVehicle, this.vehicle);
				}
				if (flag && this.onVehicleUpdated != null)
				{
					ushort newFuel;
					ushort maxFuel;
					this.vehicle.getDisplayFuel(out newFuel, out maxFuel);
					this.onVehicleUpdated(!this.vehicle.isUnderwater && !this.vehicle.isDead, newFuel, maxFuel, this.vehicle.AnimatedForwardVelocity, this.vehicle.asset.TargetReverseVelocity, this.vehicle.asset.TargetForwardVelocity, this.vehicle.health, this.vehicle.asset.health, this.vehicle.batteryCharge);
				}
				if (this.vehicle != null)
				{
					if (flag)
					{
						if (interactableVehicle == null)
						{
							PlayerUI.message(EPlayerMessage.VEHICLE_EXIT, "", 2f);
						}
						else
						{
							PlayerUI.message(EPlayerMessage.VEHICLE_SWAP, "", 2f);
						}
					}
					else
					{
						PlayerUI.message(EPlayerMessage.VEHICLE_SWAP, "", 2f);
					}
				}
			}
			if (this.vehicle != null)
			{
				base.player.transform.parent = this.pendingSeatTransform;
				base.player.transform.localPosition = this.pendingSeatPosition;
				base.player.transform.localRotation = Quaternion.identity;
				base.player.look.updateLook();
			}
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x000F1CF4 File Offset: 0x000EFEF4
		public void setVehicle(InteractableVehicle newVehicle, byte newSeat, Transform newSeatingTransform, Vector3 newSeatingPosition, byte newSeatingAngle, bool forceUpdate)
		{
			this.hasPendingVehicleChange = true;
			this.pendingVehicle = newVehicle;
			this.pendingSeatIndex = newSeat;
			this.pendingSeatTransform = newSeatingTransform;
			this.pendingSeatPosition = newSeatingPosition;
			this.pendingSeatAngle = newSeatingAngle;
			if ((base.channel.IsLocalPlayer || Provider.isServer) && base.player.life.IsAlive && !forceUpdate)
			{
				return;
			}
			this.updateVehicle();
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000F1D5E File Offset: 0x000EFF5E
		[Obsolete]
		public void tellPluginGravityMultiplier(CSteamID steamID, float newPluginGravityMultiplier)
		{
			this.ReceivePluginGravityMultiplier(newPluginGravityMultiplier);
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x000F1D67 File Offset: 0x000EFF67
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellPluginGravityMultiplier")]
		public void ReceivePluginGravityMultiplier(float newPluginGravityMultiplier)
		{
			this.pluginGravityMultiplier = newPluginGravityMultiplier;
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000F1D70 File Offset: 0x000EFF70
		public void sendPluginGravityMultiplier(float newPluginGravityMultiplier)
		{
			this.pluginGravityMultiplier = newPluginGravityMultiplier;
			if (!base.channel.IsLocalPlayer)
			{
				PlayerMovement.SendPluginGravityMultiplier.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), newPluginGravityMultiplier);
			}
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000F1DA3 File Offset: 0x000EFFA3
		[Obsolete]
		public void tellPluginJumpMultiplier(CSteamID steamID, float newPluginJumpMultiplier)
		{
			this.ReceivePluginJumpMultiplier(newPluginJumpMultiplier);
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000F1DAC File Offset: 0x000EFFAC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellPluginJumpMultiplier")]
		public void ReceivePluginJumpMultiplier(float newPluginJumpMultiplier)
		{
			this.pluginJumpMultiplier = newPluginJumpMultiplier;
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000F1DB5 File Offset: 0x000EFFB5
		public void sendPluginJumpMultiplier(float newPluginJumpMultiplier)
		{
			this.pluginJumpMultiplier = newPluginJumpMultiplier;
			if (!base.channel.IsLocalPlayer)
			{
				PlayerMovement.SendPluginJumpMultiplier.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), newPluginJumpMultiplier);
			}
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000F1DE8 File Offset: 0x000EFFE8
		[Obsolete]
		public void tellPluginSpeedMultiplier(CSteamID steamID, float newPluginSpeedMultiplier)
		{
			this.ReceivePluginSpeedMultiplier(newPluginSpeedMultiplier);
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x000F1DF1 File Offset: 0x000EFFF1
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellPluginSpeedMultiplier")]
		public void ReceivePluginSpeedMultiplier(float newPluginSpeedMultiplier)
		{
			this.pluginSpeedMultiplier = newPluginSpeedMultiplier;
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x000F1DFA File Offset: 0x000EFFFA
		public void sendPluginSpeedMultiplier(float newPluginSpeedMultiplier)
		{
			this.pluginSpeedMultiplier = newPluginSpeedMultiplier;
			if (!base.channel.IsLocalPlayer)
			{
				PlayerMovement.SendPluginSpeedMultiplier.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), newPluginSpeedMultiplier);
			}
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000F1E30 File Offset: 0x000F0030
		public void tellState(Vector3 newPosition, byte newPitch, byte newYaw)
		{
			if (base.channel.IsLocalPlayer)
			{
				return;
			}
			this.checkGround(newPosition);
			this.lastUpdatePos = newPosition;
			if (this.nsb != null)
			{
				this.nsb.addNewSnapshot(new PitchYawSnapshotInfo(newPosition, (float)newPitch, (float)newYaw * 2f));
			}
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000F1E7C File Offset: 0x000F007C
		public void updateMovement()
		{
			this.lastUpdatePos = base.transform.localPosition;
			if (this.nsb != null)
			{
				this.nsb.updateLastSnapshot(new PitchYawSnapshotInfo(this.lastUpdatePos, base.player.look.pitch, base.player.look.yaw));
			}
			this.pendingLaunchVelocity = Vector3.zero;
			this.velocity = Vector3.zero;
			this.mostRecentControllerColliderHit = null;
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000F1EF8 File Offset: 0x000F00F8
		private void checkGround(Vector3 position)
		{
			this.materialName = null;
			this.materialIsWater = false;
			int block_COLLISION = RayMasks.BLOCK_COLLISION;
			float num = PlayerStance.RADIUS - 0.001f;
			CharacterController controller = this.controller;
			float maxDistance = ((controller != null) ? controller.skinWidth : 0f) + 0.025f;
			Physics.SphereCast(new Ray(position + new Vector3(0f, num, 0f), Vector3.down), num, out this.ground, maxDistance, block_COLLISION, QueryTriggerInteraction.Ignore);
			this._isGrounded = (this.ground.transform != null);
			if ((base.channel.IsLocalPlayer || Provider.isServer) && this.controller.enabled && this.controller.isGrounded)
			{
				this._isGrounded = true;
			}
			if (base.player.stance.stance == EPlayerStance.CLIMB || base.player.stance.stance == EPlayerStance.SWIM || base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
			{
				this._isGrounded = true;
			}
			if (base.player.stance.stance == EPlayerStance.CLIMB)
			{
				this.materialName = "Tile";
				return;
			}
			if (base.player.stance.stance == EPlayerStance.SWIM || WaterUtility.isPointUnderwater(base.transform.position))
			{
				this.materialName = "Water";
				this.materialIsWater = true;
				return;
			}
			if (this.ground.transform != null)
			{
				if (this.ground.transform.CompareTag("Ground"))
				{
					this.materialName = PhysicsTool.GetTerrainMaterialName(base.transform.position);
					return;
				}
				Collider collider = this.ground.collider;
				string text;
				if (collider == null)
				{
					text = null;
				}
				else
				{
					PhysicMaterial sharedMaterial = collider.sharedMaterial;
					text = ((sharedMaterial != null) ? sharedMaterial.name : null);
				}
				this.materialName = text;
			}
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x000F20D0 File Offset: 0x000F02D0
		private void onVisionUpdated(bool isViewing)
		{
			if (isViewing)
			{
				this.warp_x = (((double)Random.value < 0.25) ? -1 : 1);
				this.warp_y = (((double)Random.value < 0.25) ? -1 : 1);
				return;
			}
			this.warp_x = 1;
			this.warp_y = 1;
		}

		/// <summary>
		/// Serverside force player to exit vehicle regardless of safe exit points.
		/// </summary>
		/// <returns>True if player was seated in vehicle.</returns>
		// Token: 0x060034AD RID: 13485 RVA: 0x000F2128 File Offset: 0x000F0328
		public bool forceRemoveFromVehicle()
		{
			byte b;
			Vector3 point;
			byte angle;
			if (this.vehicle != null && base.channel != null && base.channel.owner != null && this.vehicle.forceRemovePlayer(out b, base.channel.owner.playerID.steamID, out point, out angle))
			{
				VehicleManager.sendExitVehicle(this.vehicle, b, point, angle, true);
				return true;
			}
			if (this.hasPendingVehicleChange && this.pendingVehicle != null)
			{
				byte angle2 = MeasurementTool.angleToByte(base.transform.rotation.eulerAngles.y);
				VehicleManager.sendExitVehicle(this.pendingVehicle, this.pendingSeatIndex, base.transform.position, angle2, true);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Dedicated server simulate while input queue is empty.
		/// </summary>
		// Token: 0x060034AE RID: 13486 RVA: 0x000F21ED File Offset: 0x000F03ED
		public void simulate()
		{
			this.updateRegionAndBound();
			if (base.channel.IsLocalPlayer)
			{
				this.lastUpdatePos = base.transform.position;
			}
			if (this.hasPendingVehicleChange)
			{
				this.hasPendingVehicleChange = false;
				this.updateVehicle();
				return;
			}
		}

		/// <summary>
		/// Dedicated server simulate driving input.
		/// </summary>
		// Token: 0x060034AF RID: 13487 RVA: 0x000F222C File Offset: 0x000F042C
		public void simulate(uint simulation, int recov, bool inputBrake, bool inputStamina, Vector3 point, Quaternion rotation, float newSpeed, float newForwardVelocity, float newSteeringInput, float newVelocityInput, float delta)
		{
			this.updateRegionAndBound();
			if (base.channel.IsLocalPlayer)
			{
				this.lastUpdatePos = base.transform.position;
			}
			this.velocity = Vector3.zero;
			this.pendingLaunchVelocity = Vector3.zero;
			this.mostRecentControllerColliderHit = null;
			if (this.hasPendingVehicleChange)
			{
				this.hasPendingVehicleChange = false;
				this.updateVehicle();
				return;
			}
			if (base.player.stance.stance == EPlayerStance.DRIVING && this.vehicle != null)
			{
				this.vehicle.simulate(simulation, recov, inputStamina, point, rotation, newSpeed, newForwardVelocity, newSteeringInput, newVelocityInput, delta);
			}
		}

		/// <summary>
		/// Client and dedicated server simulate walking input.
		/// </summary>
		// Token: 0x060034B0 RID: 13488 RVA: 0x000F22D0 File Offset: 0x000F04D0
		public void simulate(uint simulation, int recov, int input_x, int input_y, float look_x, float look_y, bool inputJump, bool inputSprint, float deltaTime)
		{
			this.updateRegionAndBound();
			if (base.channel.IsLocalPlayer)
			{
				this.lastUpdatePos = base.transform.position;
			}
			if (this.hasPendingVehicleChange)
			{
				this.hasPendingVehicleChange = false;
				this.updateVehicle();
			}
			this._move.x = (float)input_x;
			this._move.z = (float)input_y;
			if (base.player.stance.stance == EPlayerStance.SITTING)
			{
				this._isMoving = false;
				this.checkGround(base.transform.position);
				this.mostRecentControllerColliderHit = null;
				this.velocity = Vector3.zero;
				this.pendingLaunchVelocity = Vector3.zero;
				if (this.getVehicle() != null && this.getVehicle().passengers[(int)this.getSeat()].turret != null && (Mathf.Abs((int)(base.player.look.lastAngle - base.player.look.angle)) > 1 || Mathf.Abs((int)(base.player.look.lastRot - base.player.look.rot)) > 1))
				{
					base.player.look.lastAngle = base.player.look.angle;
					base.player.look.lastRot = base.player.look.rot;
					if (this.canAddSimulationResultsToUpdates)
					{
						this.mostRecentlyAddedUpdate = new PlayerStateUpdate(base.transform.position, base.player.look.angle, base.player.look.rot);
						this.hasMostRecentlyAddedUpdate = true;
						this.updates.Add(this.mostRecentlyAddedUpdate);
					}
				}
				return;
			}
			if (base.player.stance.stance == EPlayerStance.DRIVING)
			{
				this._isMoving = false;
				this.checkGround(base.transform.position);
				this.mostRecentControllerColliderHit = null;
				this.velocity = Vector3.zero;
				this.pendingLaunchVelocity = Vector3.zero;
				if (base.channel.IsLocalPlayer)
				{
					this.vehicle.simulate(simulation, recov, input_x, input_y, look_x, look_y, inputJump, inputSprint, deltaTime);
					if (this.onVehicleUpdated != null)
					{
						ushort newFuel;
						ushort maxFuel;
						this.vehicle.getDisplayFuel(out newFuel, out maxFuel);
						this.onVehicleUpdated(!this.vehicle.isUnderwater && !this.vehicle.isDead, newFuel, maxFuel, this.vehicle.ReplicatedForwardVelocity, this.vehicle.asset.TargetReverseVelocity, this.vehicle.asset.TargetForwardVelocity, this.vehicle.health, this.vehicle.asset.health, this.vehicle.batteryCharge);
					}
				}
				return;
			}
			if (base.player.stance.stance == EPlayerStance.CLIMB)
			{
				this._isMoving = ((double)Mathf.Abs(this.move.x) > 0.1 || (double)Mathf.Abs(this.move.z) > 0.1);
				this.checkGround(base.transform.position);
				this.pendingLaunchVelocity = Vector3.zero;
				this.velocity = new Vector3(0f, this._move.z * this.speed * 0.5f, 0f);
				this.mostRecentControllerColliderHit = null;
				if (this.controller.enabled)
				{
					this.controller.CheckedMove(this.velocity * deltaTime);
				}
			}
			else if (base.player.stance.stance == EPlayerStance.SWIM)
			{
				this._isMoving = ((double)Mathf.Abs(this.move.x) > 0.1 || (double)Mathf.Abs(this.move.z) > 0.1);
				this.checkGround(base.transform.position);
				this.pendingLaunchVelocity = Vector3.zero;
				if (base.player.stance.isSubmerged || (base.player.look.pitch > 110f && (double)this.move.z > 0.1))
				{
					this.velocity = base.player.look.aim.rotation * this.move.normalized * this.speed * 1.5f;
					if (inputJump)
					{
						this.velocity.y = PlayerMovement.SWIM * this.pluginJumpMultiplier;
					}
					this.mostRecentControllerColliderHit = null;
					if (this.controller.enabled)
					{
						this.controller.CheckedMove(this.velocity * deltaTime);
					}
				}
				else
				{
					bool flag;
					float num;
					WaterUtility.getUnderwaterInfo(base.transform.position, out flag, out num);
					this.velocity = base.transform.rotation * this.move.normalized * this.speed * 1.5f;
					this.velocity.y = (num - 1.275f - base.transform.position.y) / 8f;
					this.mostRecentControllerColliderHit = null;
					if (this.controller.enabled)
					{
						this.controller.CheckedMove(this.velocity * deltaTime);
					}
				}
			}
			else
			{
				this._isMoving = ((double)Mathf.Abs(this.move.x) > 0.1 || (double)Mathf.Abs(this.move.z) > 0.1);
				bool isGrounded = this.isGrounded;
				this.checkGround(base.transform.position);
				bool flag2 = false;
				bool flag3 = false;
				Vector3 rhs = Vector3.up;
				if (this.isGrounded && this.ground.normal.y > 0f)
				{
					float num2 = Vector3.Angle(Vector3.up, this.ground.normal);
					float num3 = 59f;
					if (Level.info != null && Level.info.configData != null && Level.info.configData.Max_Walkable_Slope > -0.5f)
					{
						num3 = Level.info.configData.Max_Walkable_Slope;
					}
					flag3 = (num2 > num3);
					rhs = this.ground.normal;
				}
				if (!flag3 && this.mostRecentControllerColliderHit != null && this.mostRecentControllerColliderHit.collider != null && this.mostRecentControllerColliderHit.gameObject != null && this.mostRecentControllerColliderHit.normal.y > 0f && this.mostRecentControllerColliderHit.gameObject.CompareTag("Agent"))
				{
					flag3 = true;
					rhs = this.mostRecentControllerColliderHit.normal;
				}
				if (flag3)
				{
					Vector3 normalized = Vector3.Cross(Vector3.Cross(Vector3.up, rhs).normalized, rhs).normalized;
					this.velocity += normalized * 16f * deltaTime;
					flag2 = true;
				}
				else
				{
					Vector3 vector = base.transform.rotation * this.move.normalized * this.speed;
					if (this.isGrounded)
					{
						PhysicsMaterialCharacterFrictionProperties characterFrictionProperties = PhysicMaterialCustomData.GetCharacterFrictionProperties(this.materialName);
						if (characterFrictionProperties.mode == EPhysicsMaterialCharacterFrictionMode.ImmediatelyResponsive)
						{
							vector = Vector3.Cross(Vector3.Cross(Vector3.up, vector).normalized, this.ground.normal).normalized * this.speed;
							vector.y = Mathf.Min(vector.y, 0f);
							this.velocity = vector;
						}
						else
						{
							Vector3 a = Vector3.ProjectOnPlane(this.velocity, this.ground.normal);
							float magnitude = a.magnitude;
							Vector3 a2 = Vector3.Cross(Vector3.Cross(Vector3.up, vector).normalized, this.ground.normal).normalized * this.speed;
							a2 *= characterFrictionProperties.maxSpeedMultiplier;
							float magnitude2 = a2.magnitude;
							float maxMagnitude;
							if (magnitude > magnitude2)
							{
								float num4 = -2f * characterFrictionProperties.decelerationMultiplier;
								maxMagnitude = Mathf.Max(magnitude2, magnitude + num4 * deltaTime);
							}
							else
							{
								maxMagnitude = magnitude2;
							}
							Vector3 a3 = a2 * characterFrictionProperties.accelerationMultiplier;
							Vector3 vector2 = a + a3 * deltaTime;
							this.velocity = vector2.ClampMagnitude(maxMagnitude);
							flag2 = true;
						}
					}
					else
					{
						this.velocity.y = this.velocity.y + Physics.gravity.y * ((this.fall <= 0f) ? this.totalGravityMultiplier : 1f) * deltaTime * 3f;
						float a4 = (this.totalGravityMultiplier < 0.99f) ? (Physics.gravity.y * 2f * this.totalGravityMultiplier) : -100f;
						this.velocity.y = Mathf.Max(a4, this.velocity.y);
						float horizontalMagnitude = vector.GetHorizontalMagnitude();
						Vector3 horizontal = this.velocity.GetHorizontal();
						float horizontalMagnitude2 = this.velocity.GetHorizontalMagnitude();
						float maxMagnitude2;
						if (horizontalMagnitude2 > horizontalMagnitude)
						{
							float num5 = 2f * Provider.modeConfigData.Gameplay.AirStrafing_Deceleration_Multiplier;
							maxMagnitude2 = Mathf.Max(horizontalMagnitude, horizontalMagnitude2 - num5 * deltaTime);
						}
						else
						{
							maxMagnitude2 = horizontalMagnitude;
						}
						Vector3 a5 = vector * (4f * Provider.modeConfigData.Gameplay.AirStrafing_Acceleration_Multiplier);
						Vector3 vector3 = horizontal + a5 * deltaTime;
						vector3 = vector3.ClampHorizontalMagnitude(maxMagnitude2);
						this.velocity.x = vector3.x;
						this.velocity.z = vector3.z;
						flag2 = true;
					}
				}
				if (inputJump && this.isGrounded && !base.player.life.isBroken && (float)base.player.life.stamina >= 10f * (1f - base.player.skills.mastery(0, 6) * 0.5f) && (base.player.stance.stance == EPlayerStance.STAND || base.player.stance.stance == EPlayerStance.SPRINT) && !MathfEx.IsNearlyZero(this.pluginJumpMultiplier, 0.001f))
				{
					this.velocity.y = PlayerMovement.JUMP * (1f + base.player.skills.mastery(0, 6) * 0.25f) * this.pluginJumpMultiplier;
					base.player.life.askTire((byte)(10f * (1f - base.player.skills.mastery(0, 6) * 0.5f)));
				}
				this.velocity += this.pendingLaunchVelocity;
				this.pendingLaunchVelocity = Vector3.zero;
				if (base.channel.IsLocalPlayer && LoadingUI.isBlocked)
				{
					this.velocity = Vector3.zero;
				}
				else
				{
					Vector3 position = base.transform.position;
					this.mostRecentControllerColliderHit = null;
					if (this.controller.enabled)
					{
						this.controller.CheckedMove(this.velocity * deltaTime);
					}
					if (!isGrounded)
					{
						this.checkGround(base.transform.position);
						if (this.isGrounded)
						{
							Landed landed = this.onLanded;
							if (landed != null)
							{
								landed(this.velocity.y);
							}
						}
					}
					else if (this.velocity.y < 0.01f)
					{
						int block_COLLISION = RayMasks.BLOCK_COLLISION;
						float num6 = PlayerStance.RADIUS - 0.001f;
						float maxDistance = this.controller.stepOffset + this.controller.skinWidth;
						RaycastHit raycastHit;
						if (Physics.SphereCast(new Ray(base.transform.position + new Vector3(0f, num6, 0f), Vector3.down), num6, out raycastHit, maxDistance, block_COLLISION, QueryTriggerInteraction.Ignore))
						{
							float num7 = raycastHit.distance - this.controller.skinWidth;
							if (num7 > Mathf.Epsilon)
							{
								Vector3 normal = raycastHit.normal;
								float num8 = 59f;
								if (Level.info != null && Level.info.configData != null && Level.info.configData.Max_Walkable_Slope > -0.5f)
								{
									num8 = Level.info.configData.Max_Walkable_Slope;
								}
								if (Vector3.Angle(Vector3.up, normal) < num8)
								{
									base.transform.position += new Vector3(0f, -num7, 0f);
								}
							}
						}
					}
					if (flag2)
					{
						this.velocity = (base.transform.position - position) / deltaTime;
					}
				}
			}
			if (Level.info != null && Level.info.configData.Use_Legacy_Clip_Borders)
			{
				Vector3 position2 = base.transform.position;
				float num9 = (float)Level.size / 2f - (float)Level.border;
				float num10 = num9 + 8f;
				bool flag4 = false;
				if (position2.x > -num10 && position2.x < -num9)
				{
					position2.x = -num9 + 1f;
					flag4 = true;
				}
				else if (position2.x < num10 && position2.x > num9)
				{
					position2.x = num9 - 1f;
					flag4 = true;
				}
				if (position2.z > -num10 && position2.z < -num9)
				{
					position2.z = -num9 + 1f;
					flag4 = true;
				}
				else if (position2.z < num10 && position2.z > num9)
				{
					position2.z = num9 - 1f;
					flag4 = true;
				}
				if (flag4)
				{
					position2.y += 8f;
				}
				position2.y = Mathf.Clamp(position2.y, 0f, Level.HEIGHT);
				base.transform.position = position2;
			}
			if (Provider.isServer && !this.bypassUndergroundWhitelist && !base.channel.owner.isAdmin)
			{
				Vector3 position3 = base.transform.position;
				if (UndergroundAllowlist.AdjustPosition(ref position3, 0.5f, 0.1f))
				{
					base.transform.position = position3;
				}
			}
			if (!base.channel.IsLocalPlayer && Provider.isServer && this.updates != null)
			{
				Vector3 position4 = base.transform.position;
				if (Mathf.Abs((int)(base.player.look.lastAngle - base.player.look.angle)) > 1 || Mathf.Abs((int)(base.player.look.lastRot - base.player.look.rot)) > 1 || Mathf.Abs(this.lastUpdatePos.x - position4.x) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatePos.y - position4.y) > Provider.UPDATE_DISTANCE || Mathf.Abs(this.lastUpdatePos.z - position4.z) > Provider.UPDATE_DISTANCE)
				{
					base.player.look.lastAngle = base.player.look.angle;
					base.player.look.lastRot = base.player.look.rot;
					this.lastUpdatePos = position4;
					if (this.canAddSimulationResultsToUpdates)
					{
						this.mostRecentlyAddedUpdate = new PlayerStateUpdate(position4, base.player.look.angle, base.player.look.rot);
						this.hasMostRecentlyAddedUpdate = true;
						this.updates.Add(this.mostRecentlyAddedUpdate);
					}
				}
			}
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000F32F0 File Offset: 0x000F14F0
		private void Update()
		{
			if (this.nsb != null)
			{
				this.snapshot = this.nsb.getCurrentSnapshot();
			}
			if (base.channel.IsLocalPlayer)
			{
				if (!PlayerUI.window.showCursor && !LoadingUI.isBlocked)
				{
					this._jump = InputEx.GetKey(ControlsSettings.jump);
					if (this.getVehicle() != null)
					{
						if (InputEx.GetKeyDown(ControlsSettings.locker))
						{
							VehicleManager.sendVehicleLock();
						}
						if (InputEx.GetKeyDown(ControlsSettings.primary))
						{
							VehicleManager.sendVehicleHorn();
						}
						if (InputEx.GetKeyDown(ControlsSettings.secondary))
						{
							VehicleManager.sendVehicleHeadlights();
						}
						if (InputEx.GetKeyDown(ControlsSettings.other))
						{
							VehicleManager.sendVehicleBonus();
						}
					}
					if (this.getVehicle() != null && this.getVehicle().asset != null && (this.getVehicle().asset.engine == EEngine.PLANE || this.getVehicle().asset.engine == EEngine.HELICOPTER || this.getVehicle().asset.engine == EEngine.BLIMP))
					{
						if (InputEx.GetKey(ControlsSettings.yawLeft))
						{
							this.input_x = -1;
						}
						else if (InputEx.GetKey(ControlsSettings.yawRight))
						{
							this.input_x = 1;
						}
						else
						{
							this.input_x = 0;
						}
						if (InputEx.GetKey(ControlsSettings.thrustIncrease))
						{
							this.input_y = 1;
						}
						else if (InputEx.GetKey(ControlsSettings.thrustDecrease))
						{
							this.input_y = -1;
						}
						else
						{
							this.input_y = 0;
						}
					}
					else
					{
						if (InputEx.GetKey(ControlsSettings.left))
						{
							this.input_x = -1;
						}
						else if (InputEx.GetKey(ControlsSettings.right))
						{
							this.input_x = 1;
						}
						else
						{
							this.input_x = 0;
						}
						if (InputEx.GetKey(ControlsSettings.up))
						{
							this.input_y = 1;
						}
						else if (InputEx.GetKey(ControlsSettings.down))
						{
							this.input_y = -1;
						}
						else
						{
							this.input_y = 0;
						}
					}
				}
				else
				{
					this._jump = false;
					this.input_x = 0;
					this.input_y = 0;
				}
				this.input_x *= this.warp_x;
				this.input_y *= this.warp_y;
				if (base.player.look.IsControllingFreecam)
				{
					this._jump = false;
					this._horizontal = 1;
					this._vertical = 1;
				}
				else
				{
					this._horizontal = (byte)(this.input_x + 1);
					this._vertical = (byte)(this.input_y + 1);
				}
			}
			if (base.channel.IsLocalPlayer)
			{
				if (base.player.look.IsControllingFreecam && (!base.player.workzone.isBuilding || InputEx.GetKey(ControlsSettings.secondary)))
				{
					if (InputEx.GetKey(ControlsSettings.other))
					{
						if (base.player.look.freecamVerticalFieldOfView > 0f)
						{
							base.player.look.freecamVerticalFieldOfView = Mathf.Clamp(base.player.look.freecamVerticalFieldOfView + Input.GetAxis("mouse_z") * 5f, 1f, 179f);
						}
					}
					else
					{
						base.player.look.orbitSpeed = Mathf.Clamp(base.player.look.orbitSpeed + Input.GetAxis("mouse_z") * 0.2f * base.player.look.orbitSpeed, 0.5f, 2048f);
					}
					Vector3 b = MainCamera.instance.transform.right * (float)this.input_x * Time.deltaTime * base.player.look.orbitSpeed;
					if (base.player.look.isFocusing)
					{
						Vector3 a = base.player.first.position + Vector3.up;
						Vector3 b2 = base.player.look.lockPosition + base.player.look.orbitPosition;
						float horizontalMagnitude = (a - b2).GetHorizontalMagnitude();
						Vector3 vector = base.player.look.orbitPosition + b;
						Vector3 b3 = base.player.look.lockPosition + vector;
						float num = (a - b3).GetHorizontalMagnitude();
						if (num < 0.001f)
						{
							num = 1f;
						}
						float num2 = horizontalMagnitude / num;
						vector.x *= num2;
						vector.z *= num2;
						base.player.look.orbitPosition = vector;
					}
					else
					{
						base.player.look.orbitPosition += b;
					}
					base.player.look.orbitPosition += MainCamera.instance.transform.forward * (float)this.input_y * Time.deltaTime * base.player.look.orbitSpeed;
					float d;
					if (InputEx.GetKey(ControlsSettings.ascend))
					{
						d = 1f;
					}
					else if (InputEx.GetKey(ControlsSettings.descend))
					{
						d = -1f;
					}
					else
					{
						d = 0f;
					}
					base.player.look.orbitPosition += Vector3.up * d * Time.deltaTime * base.player.look.orbitSpeed;
				}
				if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					base.player.first.localPosition = Vector3.zero;
					base.player.third.localPosition = Vector3.zero;
				}
				else
				{
					base.player.first.position = Vector3.Lerp(this.lastUpdatePos, base.transform.position, (Time.realtimeSinceStartup - base.player.input.tick) / PlayerInput.RATE);
					base.player.third.position = base.player.first.position;
				}
				base.player.look.aim.parent.transform.position = base.player.first.position;
				if (this.vehicle != null)
				{
					if ((base.transform.position - this.lastStatPos).sqrMagnitude > 1024f)
					{
						this.lastStatPos = base.transform.position;
					}
					else if (Time.realtimeSinceStartup - this.lastStatTime > 1f)
					{
						this.lastStatTime = Time.realtimeSinceStartup;
						if ((base.transform.position - this.lastStatPos).sqrMagnitude > 0.1f)
						{
							int num3;
							if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Travel_Vehicle", out num3))
							{
								Provider.provider.statisticsService.userStatisticsService.setStatistic("Travel_Vehicle", num3 + (int)(base.transform.position - this.lastStatPos).magnitude);
							}
							this.lastStatPos = base.transform.position;
						}
					}
				}
				else if ((base.transform.position - this.lastStatPos).sqrMagnitude > 256f)
				{
					this.lastStatPos = base.transform.position;
				}
				else if (Time.realtimeSinceStartup - this.lastStatTime > 1f)
				{
					this.lastStatTime = Time.realtimeSinceStartup;
					if ((base.transform.position - this.lastStatPos).sqrMagnitude > 0.1f)
					{
						int num4;
						if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Travel_Foot", out num4))
						{
							Provider.provider.statisticsService.userStatisticsService.setStatistic("Travel_Foot", num4 + (int)(base.transform.position - this.lastStatPos).magnitude);
						}
						this.lastStatPos = base.transform.position;
					}
				}
			}
			else if (!Provider.isServer)
			{
				if (base.player.stance.stance == EPlayerStance.SITTING || base.player.stance.stance == EPlayerStance.DRIVING)
				{
					this._isMoving = false;
					base.transform.localPosition = Vector3.zero;
				}
				else
				{
					if (Mathf.Abs(this.lastUpdatePos.x - base.transform.position.x) > 0.01f || Mathf.Abs(this.lastUpdatePos.y - base.transform.position.y) > 0.01f || Mathf.Abs(this.lastUpdatePos.z - base.transform.position.z) > 0.01f)
					{
						this._isMoving = true;
					}
					else
					{
						this._isMoving = false;
					}
					base.transform.localPosition = this.snapshot.pos;
				}
			}
			if (!base.channel.IsLocalPlayer && base.player.third != null)
			{
				if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					base.player.third.localPosition = new Vector3(0f, -0.1f, 0f);
					return;
				}
				base.player.third.localPosition = Vector3.zero;
			}
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x000F3C88 File Offset: 0x000F1E88
		private void updateRegionAndBound()
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(base.transform.position, out b, out b2) && (b != this.region_x || b2 != this.region_y))
			{
				byte region_x = this.region_x;
				byte region_y = this.region_y;
				this._region_x = b;
				this._region_y = b2;
				this.updateRegionOld_X = region_x;
				this.updateRegionOld_Y = region_y;
				this.updateRegionNew_X = b;
				this.updateRegionNew_Y = b2;
				this.updateRegionIndex = 0;
			}
			if (this.updateRegionIndex < 6)
			{
				bool flag = true;
				PlayerRegionUpdated playerRegionUpdated = this.onRegionUpdated;
				if (playerRegionUpdated != null)
				{
					playerRegionUpdated(base.player, this.updateRegionOld_X, this.updateRegionOld_Y, this.updateRegionNew_X, this.updateRegionNew_Y, this.updateRegionIndex, ref flag);
				}
				if (flag)
				{
					this.updateRegionIndex += 1;
				}
			}
			byte b3;
			LevelNavigation.tryGetBounds(base.transform.position, out b3);
			if (b3 != this.bound)
			{
				byte bound = this.bound;
				this._bound = b3;
				PlayerBoundUpdated playerBoundUpdated = this.onBoundUpdated;
				if (playerBoundUpdated != null)
				{
					playerBoundUpdated(base.player, bound, b3);
				}
			}
			if (Provider.isServer)
			{
				byte b4;
				LevelNavigation.tryGetNavigation(base.transform.position, out b4);
				if (b4 != this.nav)
				{
					byte nav = this.nav;
					this._nav = b4;
					this.TriggerPlayerNavChanged(nav, b4);
				}
			}
			bool flag2 = LevelNodes.isPointInsideSafezone(base.transform.position, out this.isSafeInfo);
			bool flag3 = false;
			IDeadzoneNode activeDeadzone = null;
			HordePurchaseVolume firstOverlappingVolume = VolumeManager<HordePurchaseVolume, HordePurchaseVolumeManager>.Get().GetFirstOverlappingVolume(base.transform.position);
			this.effectNode = null;
			this.inSnow = LevelLighting.isPositionSnowy(base.transform.position);
			AmbianceVolume firstOverlappingVolume2 = VolumeManager<AmbianceVolume, AmbianceVolumeManager>.Get().GetFirstOverlappingVolume(base.transform.position);
			if (firstOverlappingVolume2 != null)
			{
				this.effectNode = firstOverlappingVolume2;
				if (!this.inSnow && Level.info != null && Level.info.configData.Use_Snow_Volumes)
				{
					this.inSnow = ((firstOverlappingVolume2.weatherMask & 2U) > 0U);
				}
				this.WeatherMask = firstOverlappingVolume2.weatherMask;
			}
			else
			{
				LevelAsset asset = Level.getAsset();
				this.WeatherMask = ((asset != null) ? asset.globalWeatherMask : uint.MaxValue);
			}
			this.inSnow &= (LevelLighting.snowyness == ELightingSnow.BLIZZARD);
			DeadzoneVolume mostDangerousOverlappingVolume = VolumeManager<DeadzoneVolume, DeadzoneVolumeManager>.Get().GetMostDangerousOverlappingVolume(base.transform.position);
			if (mostDangerousOverlappingVolume != null)
			{
				flag3 = true;
				activeDeadzone = mostDangerousOverlappingVolume;
			}
			if (flag2 != this.isSafe)
			{
				this._isSafe = flag2;
				SafetyUpdated safetyUpdated = this.onSafetyUpdated;
				if (safetyUpdated != null)
				{
					safetyUpdated(this.isSafe);
				}
			}
			this.ActiveDeadzone = activeDeadzone;
			if (flag3 != this.isRadiated)
			{
				this._isRadiated = flag3;
				RadiationUpdated radiationUpdated = this.onRadiationUpdated;
				if (radiationUpdated != null)
				{
					radiationUpdated(this.isRadiated);
				}
			}
			if (firstOverlappingVolume != this.purchaseNode)
			{
				this._purchaseNode = firstOverlappingVolume;
				PurchaseUpdated purchaseUpdated = this.onPurchaseUpdated;
				if (purchaseUpdated != null)
				{
					purchaseUpdated(this.purchaseNode);
				}
			}
			base.player.inventory.closeDistantStorage();
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x000F3F84 File Offset: 0x000F2184
		internal void InitializePlayer()
		{
			this.itemGravityMultiplier = 1f;
			this.pluginGravityMultiplier = 1f;
			this.pluginSpeedMultiplier = 1f;
			this._region_x = byte.MaxValue;
			this._region_y = byte.MaxValue;
			this._bound = byte.MaxValue;
			this._nav = byte.MaxValue;
			if (base.channel.IsLocalPlayer || Provider.isServer)
			{
				this._loadedRegions = new LoadedRegion[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						this.loadedRegions[(int)b, (int)b2] = new LoadedRegion();
					}
				}
				this._loadedBounds = new LoadedBound[LevelNavigation.bounds.Count];
				byte b3 = 0;
				while ((int)b3 < LevelNavigation.bounds.Count)
				{
					this.loadedBounds[(int)b3] = new LoadedBound();
					b3 += 1;
				}
			}
			this.warp_x = 1;
			this.warp_y = 1;
			if (Provider.isServer || base.channel.IsLocalPlayer)
			{
				this.controller = base.GetComponent<CharacterController>();
				this.controller.enableOverlapRecovery = false;
			}
			if (Provider.isServer)
			{
				PlayerLife life = base.player.life;
				life.onVisionUpdated = (VisionUpdated)Delegate.Combine(life.onVisionUpdated, new VisionUpdated(this.onVisionUpdated));
			}
			else
			{
				this.nsb = new NetworkSnapshotBuffer<PitchYawSnapshotInfo>(Provider.UPDATE_TIME, Provider.UPDATE_DELAY);
			}
			this.applySize();
			base.gameObject.AddComponent<Rigidbody>();
			base.GetComponent<Rigidbody>().useGravity = false;
			base.GetComponent<Rigidbody>().isKinematic = true;
			this.updateMovement();
			this.updates = new List<PlayerStateUpdate>();
			this.canAddSimulationResultsToUpdates = true;
			this.lastFootstep = Time.time;
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x000F4147 File Offset: 0x000F2347
		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			this.mostRecentControllerColliderHit = hit;
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x000F4150 File Offset: 0x000F2350
		private void OnDrawGizmos()
		{
			if (this.nsb == null)
			{
				return;
			}
			for (int i = 0; i < this.nsb.snapshots.Length; i++)
			{
				if (this.nsb.snapshots[i].timestamp <= 0.01f)
				{
					return;
				}
				PitchYawSnapshotInfo info = this.nsb.snapshots[i].info;
				Gizmos.DrawLine(info.pos, info.pos + Vector3.up * 2f);
			}
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x000F41D8 File Offset: 0x000F23D8
		private void OnDestroy()
		{
			this.updates = null;
		}

		// Token: 0x04001E4B RID: 7755
		public static readonly float HEIGHT_STAND = 2f;

		// Token: 0x04001E4C RID: 7756
		public static readonly float HEIGHT_CROUCH = 1.2f;

		// Token: 0x04001E4D RID: 7757
		public static readonly float HEIGHT_PRONE = 0.8f;

		// Token: 0x04001E4E RID: 7758
		public Landed onLanded;

		// Token: 0x04001E4F RID: 7759
		public Seated onSeated;

		// Token: 0x04001E50 RID: 7760
		public VehicleUpdated onVehicleUpdated;

		// Token: 0x04001E51 RID: 7761
		public SafetyUpdated onSafetyUpdated;

		// Token: 0x04001E52 RID: 7762
		public RadiationUpdated onRadiationUpdated;

		// Token: 0x04001E53 RID: 7763
		public PurchaseUpdated onPurchaseUpdated;

		// Token: 0x04001E54 RID: 7764
		public PlayerRegionUpdated onRegionUpdated;

		// Token: 0x04001E55 RID: 7765
		public PlayerBoundUpdated onBoundUpdated;

		// Token: 0x04001E57 RID: 7767
		private static readonly float SPEED_CLIMB = 4.5f;

		// Token: 0x04001E58 RID: 7768
		private static readonly float SPEED_SWIM = 3f;

		// Token: 0x04001E59 RID: 7769
		private static readonly float SPEED_SPRINT = 7f;

		// Token: 0x04001E5A RID: 7770
		private static readonly float SPEED_STAND = 4.5f;

		// Token: 0x04001E5B RID: 7771
		private static readonly float SPEED_CROUCH = 2.5f;

		// Token: 0x04001E5C RID: 7772
		private static readonly float SPEED_PRONE = 1.5f;

		/// <summary>
		/// Jump speed = sqrt(2 * jump height * gravity)
		/// Jump height = (jump speed ^ 2) / (2 * gravity)
		/// With 7 speed and 9.81 * 3 gravity = apex height of 1.66496772
		/// Nelson 2024-08-19: Increased slightly to boost jump height by ~10cm after floor snapping update.
		/// </summary>
		// Token: 0x04001E5D RID: 7773
		private static readonly float JUMP = 7.2f;

		// Token: 0x04001E5E RID: 7774
		private static readonly float SWIM = 3f;

		// Token: 0x04001E60 RID: 7776
		[Obsolete("Was current value of interpolated aiming speed multiplier.")]
		public float _multiplier;

		// Token: 0x04001E61 RID: 7777
		[Obsolete("Was target value of interpolated aiming speed multiplier.")]
		public float multiplier;

		// Token: 0x04001E62 RID: 7778
		public float itemGravityMultiplier;

		// Token: 0x04001E63 RID: 7779
		public float pluginGravityMultiplier;

		// Token: 0x04001E64 RID: 7780
		public float pluginSpeedMultiplier;

		// Token: 0x04001E65 RID: 7781
		public float pluginJumpMultiplier = 1f;

		// Token: 0x04001E66 RID: 7782
		private float lastFootstep;

		// Token: 0x04001E67 RID: 7783
		private bool _isGrounded;

		// Token: 0x04001E68 RID: 7784
		private bool _isSafe;

		// Token: 0x04001E69 RID: 7785
		public SafezoneNode isSafeInfo;

		// Token: 0x04001E6A RID: 7786
		private bool _isRadiated;

		// Token: 0x04001E6C RID: 7788
		private HordePurchaseVolume _purchaseNode;

		// Token: 0x04001E6E RID: 7790
		[Obsolete]
		public bool inRain;

		// Token: 0x04001E6F RID: 7791
		public bool inSnow;

		// Token: 0x04001E71 RID: 7793
		private string materialName;

		// Token: 0x04001E72 RID: 7794
		private bool materialIsWater;

		// Token: 0x04001E73 RID: 7795
		public RaycastHit ground;

		// Token: 0x04001E74 RID: 7796
		internal EPlayerHeight height;

		// Token: 0x04001E75 RID: 7797
		private bool wasSizeAppliedYet;

		// Token: 0x04001E76 RID: 7798
		private bool _isMoving;

		// Token: 0x04001E77 RID: 7799
		private Vector3 _move;

		// Token: 0x04001E78 RID: 7800
		private byte _region_x;

		// Token: 0x04001E79 RID: 7801
		private byte _region_y;

		// Token: 0x04001E7A RID: 7802
		private byte _bound;

		// Token: 0x04001E7B RID: 7803
		private byte _nav;

		// Token: 0x04001E7C RID: 7804
		private byte updateRegionOld_X;

		// Token: 0x04001E7D RID: 7805
		private byte updateRegionOld_Y;

		// Token: 0x04001E7E RID: 7806
		private byte updateRegionNew_X;

		// Token: 0x04001E7F RID: 7807
		private byte updateRegionNew_Y;

		// Token: 0x04001E80 RID: 7808
		private byte updateRegionIndex;

		// Token: 0x04001E81 RID: 7809
		private LoadedRegion[,] _loadedRegions;

		// Token: 0x04001E82 RID: 7810
		private LoadedBound[] _loadedBounds;

		// Token: 0x04001E83 RID: 7811
		internal Vector3 velocity;

		// Token: 0x04001E84 RID: 7812
		public Vector3 pendingLaunchVelocity;

		// Token: 0x04001E85 RID: 7813
		private Vector3 lastUpdatePos;

		// Token: 0x04001E86 RID: 7814
		public PitchYawSnapshotInfo snapshot;

		// Token: 0x04001E87 RID: 7815
		private NetworkSnapshotBuffer<PitchYawSnapshotInfo> nsb;

		// Token: 0x04001E88 RID: 7816
		private byte _horizontal;

		// Token: 0x04001E89 RID: 7817
		private byte _vertical;

		// Token: 0x04001E8A RID: 7818
		private int warp_x;

		// Token: 0x04001E8B RID: 7819
		private int warp_y;

		// Token: 0x04001E8C RID: 7820
		internal int input_x;

		// Token: 0x04001E8D RID: 7821
		internal int input_y;

		// Token: 0x04001E8E RID: 7822
		private bool _jump;

		/// <summary>
		/// Was set to true during teleport, and restored to false during the next movement tick.
		///
		/// Server pauses movement when this is set until next client update that matches,
		/// in order to prevent rubberbanding following a teleport.
		/// </summary>
		// Token: 0x04001E8F RID: 7823
		[Obsolete]
		public bool isAllowed;

		// Token: 0x04001E90 RID: 7824
		[Obsolete]
		public bool isUpdated;

		// Token: 0x04001E91 RID: 7825
		public List<PlayerStateUpdate> updates;

		// Token: 0x04001E92 RID: 7826
		public bool canAddSimulationResultsToUpdates;

		/// <summary>
		/// Used instead of actual position to avoid revealing admins in "vanish" mode.
		/// </summary>
		// Token: 0x04001E93 RID: 7827
		internal PlayerStateUpdate mostRecentlyAddedUpdate;

		// Token: 0x04001E94 RID: 7828
		internal bool hasMostRecentlyAddedUpdate;

		/// <summary>
		/// Flag for plugins to allow maintenance access underneath the map.
		/// </summary>
		// Token: 0x04001E95 RID: 7829
		public bool bypassUndergroundWhitelist;

		// Token: 0x04001E96 RID: 7830
		internal bool hasPendingVehicleChange;

		// Token: 0x04001E97 RID: 7831
		private InteractableVehicle pendingVehicle;

		// Token: 0x04001E98 RID: 7832
		private byte pendingSeatIndex;

		// Token: 0x04001E99 RID: 7833
		private Transform pendingSeatTransform;

		// Token: 0x04001E9A RID: 7834
		private Vector3 pendingSeatPosition;

		// Token: 0x04001E9B RID: 7835
		private byte pendingSeatAngle;

		// Token: 0x04001E9C RID: 7836
		private Vector3 lastStatPos;

		// Token: 0x04001E9D RID: 7837
		private float lastStatTime;

		// Token: 0x04001E9E RID: 7838
		private InteractableVehicle vehicle;

		// Token: 0x04001E9F RID: 7839
		private byte seat;

		// Token: 0x04001EA0 RID: 7840
		private static readonly ClientInstanceMethod<float> SendPluginGravityMultiplier = ClientInstanceMethod<float>.Get(typeof(PlayerMovement), "ReceivePluginGravityMultiplier");

		// Token: 0x04001EA1 RID: 7841
		private static readonly ClientInstanceMethod<float> SendPluginJumpMultiplier = ClientInstanceMethod<float>.Get(typeof(PlayerMovement), "ReceivePluginJumpMultiplier");

		// Token: 0x04001EA2 RID: 7842
		private static readonly ClientInstanceMethod<float> SendPluginSpeedMultiplier = ClientInstanceMethod<float>.Get(typeof(PlayerMovement), "ReceivePluginSpeedMultiplier");

		/// <summary>
		/// In the future this can probably replace checkGround for locally simulated character?
		/// (Unturned only started using OnControllerColliderHit on 2023-01-31)
		///
		/// 2023-02-28: be careful with .gameObject property because it returns .collider.gameObject
		/// which can cause a null reference exception. (public issue #3726)
		/// </summary>
		// Token: 0x04001EA3 RID: 7843
		private ControllerColliderHit mostRecentControllerColliderHit;
	}
}
