using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000606 RID: 1542
	public class PlayerAnimator : PlayerCaller
	{
		/// <summary>
		/// Invoked after tellGesture is called with the new gesture.
		/// </summary>
		// Token: 0x140000AC RID: 172
		// (add) Token: 0x060030F0 RID: 12528 RVA: 0x000D6FA8 File Offset: 0x000D51A8
		// (remove) Token: 0x060030F1 RID: 12529 RVA: 0x000D6FDC File Offset: 0x000D51DC
		public static event Action<PlayerAnimator, EPlayerGesture> OnGestureChanged_Global;

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x060030F2 RID: 12530 RVA: 0x000D700F File Offset: 0x000D520F
		public Transform firstSkeleton
		{
			get
			{
				return this._firstSkeleton;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x060030F3 RID: 12531 RVA: 0x000D7017 File Offset: 0x000D5217
		public Transform thirdSkeleton
		{
			get
			{
				return this._thirdSkeleton;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x060030F4 RID: 12532 RVA: 0x000D701F File Offset: 0x000D521F
		public bool leanLeft
		{
			get
			{
				return this.inputWantsToLeanLeft;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x060030F5 RID: 12533 RVA: 0x000D7027 File Offset: 0x000D5227
		public bool leanRight
		{
			get
			{
				return this.inputWantsToLeanRight;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x060030F6 RID: 12534 RVA: 0x000D702F File Offset: 0x000D522F
		public int lean
		{
			get
			{
				return this._lean;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x060030F7 RID: 12535 RVA: 0x000D7037 File Offset: 0x000D5237
		public float shoulder
		{
			get
			{
				return this._shoulder;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x060030F8 RID: 12536 RVA: 0x000D703F File Offset: 0x000D523F
		public float shoulder2
		{
			get
			{
				return this._shoulder2;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x060030F9 RID: 12537 RVA: 0x000D7047 File Offset: 0x000D5247
		public bool side
		{
			get
			{
				return this.inputWantsThirdPersonCameraOnLeftSide;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x060030FA RID: 12538 RVA: 0x000D704F File Offset: 0x000D524F
		public EPlayerGesture gesture
		{
			get
			{
				return this._gesture;
			}
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x000D7058 File Offset: 0x000D5258
		public void AddEquippedItemAnimation(AnimationClip clip, Transform firstPersonModel, Transform thirdPersonModel, Transform characterModel)
		{
			if (clip == null)
			{
				return;
			}
			if (this.firstAnimator != null)
			{
				this.firstAnimator.AddEquippedItemAnimation(clip, firstPersonModel);
			}
			if (this.thirdAnimator != null)
			{
				this.thirdAnimator.AddEquippedItemAnimation(clip, thirdPersonModel);
			}
			if (this.characterAnimator != null)
			{
				this.characterAnimator.AddEquippedItemAnimation(clip, characterModel);
			}
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x000D70C4 File Offset: 0x000D52C4
		public void removeAnimation(AnimationClip clip)
		{
			if (clip == null)
			{
				return;
			}
			if (this.firstAnimator != null)
			{
				this.firstAnimator.removeAnimation(clip);
			}
			if (this.thirdAnimator != null)
			{
				this.thirdAnimator.removeAnimation(clip);
			}
			if (this.characterAnimator != null)
			{
				this.characterAnimator.removeAnimation(clip);
			}
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x000D712C File Offset: 0x000D532C
		public void setAnimationSpeed(string name, float speed)
		{
			if (this.firstAnimator != null)
			{
				this.firstAnimator.setAnimationSpeed(name, speed);
			}
			if (this.thirdAnimator != null)
			{
				this.thirdAnimator.setAnimationSpeed(name, speed);
			}
			if (this.characterAnimator != null)
			{
				this.characterAnimator.setAnimationSpeed(name, speed);
			}
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x000D718A File Offset: 0x000D538A
		public float getAnimationLength(string name)
		{
			return this.GetAnimationLength(name, true);
		}

		/// <param name="scaled">If true, include current animation speed modifier.</param>
		// Token: 0x060030FF RID: 12543 RVA: 0x000D7194 File Offset: 0x000D5394
		public float GetAnimationLength(string name, bool scaled = true)
		{
			if (this.firstAnimator != null)
			{
				return this.firstAnimator.GetAnimationLength(name, scaled);
			}
			if (this.thirdAnimator != null)
			{
				return this.thirdAnimator.GetAnimationLength(name, scaled);
			}
			return 0f;
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x000D71D4 File Offset: 0x000D53D4
		public bool checkExists(string name)
		{
			if (this.firstAnimator != null)
			{
				return this.firstAnimator.checkExists(name);
			}
			if (this.thirdAnimator != null)
			{
				return this.thirdAnimator.checkExists(name);
			}
			return this.characterAnimator != null && this.characterAnimator.checkExists(name);
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x000D7234 File Offset: 0x000D5434
		public void play(string name, bool smooth)
		{
			bool flag = false;
			if (this.firstAnimator != null)
			{
				flag |= this.firstAnimator.play(name, smooth);
			}
			if (this.thirdAnimator != null)
			{
				flag |= this.thirdAnimator.play(name, smooth);
			}
			if (this.characterAnimator != null)
			{
				flag |= this.characterAnimator.play(name, smooth);
			}
			if (flag && this.gesture != EPlayerGesture.NONE)
			{
				this._gesture = EPlayerGesture.NONE;
			}
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x000D72B0 File Offset: 0x000D54B0
		public void stop(string name)
		{
			if (this.firstAnimator != null)
			{
				this.firstAnimator.stop(name);
			}
			if (this.thirdAnimator != null)
			{
				this.thirdAnimator.stop(name);
			}
			if (this.characterAnimator != null)
			{
				this.characterAnimator.stop(name);
			}
		}

		// Token: 0x06003103 RID: 12547 RVA: 0x000D730C File Offset: 0x000D550C
		public void mixAnimation(string name)
		{
			if (this.firstAnimator != null)
			{
				this.firstAnimator.mixAnimation(name);
			}
			if (this.thirdAnimator != null)
			{
				this.thirdAnimator.mixAnimation(name);
			}
			if (this.characterAnimator != null)
			{
				this.characterAnimator.mixAnimation(name);
			}
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x000D7367 File Offset: 0x000D5567
		public void mixAnimation(string name, bool mixLeftShoulder, bool mixRightShoulder)
		{
			this.mixAnimation(name, mixLeftShoulder, mixRightShoulder, false);
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x000D7374 File Offset: 0x000D5574
		public void mixAnimation(string name, bool mixLeftShoulder, bool mixRightShoulder, bool mixSkull)
		{
			if (this.firstAnimator != null)
			{
				this.firstAnimator.mixAnimation(name, mixLeftShoulder, mixRightShoulder, mixSkull);
			}
			if (this.thirdAnimator != null)
			{
				this.thirdAnimator.mixAnimation(name, mixLeftShoulder, mixRightShoulder, mixSkull);
			}
			if (this.characterAnimator != null)
			{
				this.characterAnimator.mixAnimation(name, mixLeftShoulder, mixRightShoulder, mixSkull);
			}
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x000D73DC File Offset: 0x000D55DC
		public void AddRecoilViewmodelCameraOffset(float shake_x, float shake_y, float shake_z)
		{
			this.recoilViewmodelCameraOffset.currentPosition.x = this.recoilViewmodelCameraOffset.currentPosition.x + shake_x;
			this.recoilViewmodelCameraOffset.currentPosition.y = this.recoilViewmodelCameraOffset.currentPosition.y + shake_y;
			this.recoilViewmodelCameraOffset.currentPosition.z = this.recoilViewmodelCameraOffset.currentPosition.z + shake_z;
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x000D7428 File Offset: 0x000D5628
		public void AddRecoilViewmodelCameraRotation(float cameraYaw, float cameraPitch)
		{
			this.recoilViewmodelCameraRotation.currentPosition.x = this.recoilViewmodelCameraRotation.currentPosition.x + cameraPitch * this.recoilViewmodelCameraMask.x;
			this.recoilViewmodelCameraRotation.currentPosition.y = this.recoilViewmodelCameraRotation.currentPosition.y + cameraYaw * this.recoilViewmodelCameraMask.y;
			this.recoilViewmodelCameraRotation.currentPosition.z = this.recoilViewmodelCameraRotation.currentPosition.z + cameraYaw * this.recoilViewmodelCameraMask.z;
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x000D7498 File Offset: 0x000D5698
		public void AddBayonetViewmodelCameraOffset(float fling_x, float fling_y, float fling_z)
		{
			this.bayonetViewmodelCameraOffset.x = this.bayonetViewmodelCameraOffset.x + fling_x;
			this.bayonetViewmodelCameraOffset.y = this.bayonetViewmodelCameraOffset.y + fling_y;
			this.bayonetViewmodelCameraOffset.z = this.bayonetViewmodelCameraOffset.z + fling_z;
		}

		/// <summary>
		/// At this point camera is already being shook, we just add some of the same shake to viewmodel for secondary motion.
		/// </summary>
		// Token: 0x06003109 RID: 12553 RVA: 0x000D74CC File Offset: 0x000D56CC
		internal void FlinchFromExplosion(Vector3 worldRotationAxis, float adjustedMagnitudeDegrees)
		{
			Vector3 axis = this.viewmodelCameraTransform.InverseTransformDirection(worldRotationAxis);
			adjustedMagnitudeDegrees *= 0.25f;
			this.viewmodelTargetExplosionLocalRotation.currentRotation = this.viewmodelTargetExplosionLocalRotation.currentRotation * Quaternion.AngleAxis(adjustedMagnitudeDegrees, axis);
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x0600310A RID: 12554 RVA: 0x000D7514 File Offset: 0x000D5714
		public float bob
		{
			get
			{
				if (Player.player.stance.stance == EPlayerStance.SPRINT)
				{
					return PlayerAnimator.BOB_SPRINT * this.blendedViewmodelSwayMultiplier;
				}
				if (Player.player.stance.stance == EPlayerStance.STAND)
				{
					return PlayerAnimator.BOB_STAND * this.blendedViewmodelSwayMultiplier;
				}
				if (Player.player.stance.stance == EPlayerStance.CROUCH)
				{
					return PlayerAnimator.BOB_CROUCH * this.blendedViewmodelSwayMultiplier;
				}
				if (Player.player.stance.stance == EPlayerStance.PRONE)
				{
					return PlayerAnimator.BOB_PRONE * this.blendedViewmodelSwayMultiplier;
				}
				if (Player.player.stance.stance == EPlayerStance.SWIM)
				{
					return PlayerAnimator.BOB_SWIM * this.blendedViewmodelSwayMultiplier;
				}
				return 0f;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x0600310B RID: 12555 RVA: 0x000D75C4 File Offset: 0x000D57C4
		public float tilt
		{
			get
			{
				if (Player.player.stance.stance == EPlayerStance.SPRINT)
				{
					return PlayerAnimator.TILT_SPRINT * (1f - this.blendedViewmodelSwayMultiplier / 2f);
				}
				if (Player.player.stance.stance == EPlayerStance.STAND)
				{
					return PlayerAnimator.TILT_STAND * (1f - this.blendedViewmodelSwayMultiplier / 2f);
				}
				if (Player.player.stance.stance == EPlayerStance.CROUCH)
				{
					return PlayerAnimator.TILT_CROUCH * (1f - this.blendedViewmodelSwayMultiplier / 2f);
				}
				if (Player.player.stance.stance == EPlayerStance.PRONE)
				{
					return PlayerAnimator.TILT_PRONE * (1f - this.blendedViewmodelSwayMultiplier / 2f);
				}
				if (Player.player.stance.stance == EPlayerStance.SWIM)
				{
					return PlayerAnimator.TILT_SWIM * (1f - this.blendedViewmodelSwayMultiplier / 2f);
				}
				return 0f;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x0600310C RID: 12556 RVA: 0x000D76B0 File Offset: 0x000D58B0
		public float roll
		{
			get
			{
				if (Player.player.stance.stance == EPlayerStance.SPRINT)
				{
					return Mathf.Sin(PlayerAnimator.TILT_SPRINT * Time.time * 0.25f) * PlayerAnimator.TILT_SPRINT;
				}
				if (Player.player.stance.stance == EPlayerStance.STAND)
				{
					return Mathf.Sin(PlayerAnimator.TILT_STAND * Time.time * 0.5f) * PlayerAnimator.TILT_STAND * 0.5f;
				}
				if (Player.player.stance.stance == EPlayerStance.SWIM)
				{
					return Mathf.Sin(PlayerAnimator.TILT_SWIM * Time.time * 0.25f) * PlayerAnimator.TILT_SWIM * 0.25f;
				}
				return 0f;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x0600310D RID: 12557 RVA: 0x000D775C File Offset: 0x000D595C
		public float speed
		{
			get
			{
				if (Player.player.stance.stance == EPlayerStance.SPRINT)
				{
					return PlayerAnimator.SPEED_SPRINT;
				}
				if (Player.player.stance.stance == EPlayerStance.STAND)
				{
					return PlayerAnimator.SPEED_STAND;
				}
				if (Player.player.stance.stance == EPlayerStance.CROUCH)
				{
					return PlayerAnimator.SPEED_CROUCH;
				}
				if (Player.player.stance.stance == EPlayerStance.PRONE)
				{
					return PlayerAnimator.SPEED_PRONE;
				}
				if (Player.player.stance.stance == EPlayerStance.SWIM)
				{
					return PlayerAnimator.SPEED_SWIM;
				}
				return 0f;
			}
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x000D77E8 File Offset: 0x000D59E8
		private void onLifeUpdated(bool isDead)
		{
			if (this.gesture != EPlayerGesture.NONE)
			{
				if (this.gesture == EPlayerGesture.INVENTORY_START)
				{
					this.stop("Gesture_Inventory");
				}
				else if (this.gesture == EPlayerGesture.SURRENDER_START)
				{
					this.stop("Gesture_Surrender");
				}
				else if (this.gesture == EPlayerGesture.ARREST_START)
				{
					this.stop("Gesture_Arrest");
				}
				else if (this.gesture == EPlayerGesture.REST_START)
				{
					this.stop("Gesture_Rest");
				}
				this.captorID = CSteamID.Nil;
				this.captorItem = 0;
				this.captorStrength = 0;
				this._gesture = EPlayerGesture.NONE;
				GestureUpdated gestureUpdated = this.onGestureUpdated;
				if (gestureUpdated != null)
				{
					gestureUpdated(this.gesture);
				}
			}
			if (base.channel.IsLocalPlayer)
			{
				this.UpdateLocalPlayerModelVisibility(isDead, base.player.look.perspective, base.player.quests.IsCutsceneModeActive());
				return;
			}
			this.thirdSkeleton.gameObject.SetActive(!isDead);
		}

		/// <summary>
		/// Called by clothing to make mesh renderers visible.
		/// </summary>
		// Token: 0x0600310F RID: 12559 RVA: 0x000D78D8 File Offset: 0x000D5AD8
		public void NotifyClothingIsVisible()
		{
			this.isHiddenWaitingForClothing = false;
			bool isLocalPlayer = base.channel.IsLocalPlayer;
		}

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x06003110 RID: 12560 RVA: 0x000D78F0 File Offset: 0x000D5AF0
		// (remove) Token: 0x06003111 RID: 12561 RVA: 0x000D7924 File Offset: 0x000D5B24
		public static event Action<PlayerAnimator> OnLeanChanged_Global;

		// Token: 0x06003112 RID: 12562 RVA: 0x000D7957 File Offset: 0x000D5B57
		[Obsolete]
		public void tellLean(CSteamID steamID, byte newLean)
		{
			this.ReceiveLean(newLean);
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x000D7960 File Offset: 0x000D5B60
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellLean")]
		public void ReceiveLean(byte newLean)
		{
			this._lean = (int)(newLean - 1);
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x000D796B File Offset: 0x000D5B6B
		[Obsolete]
		public void tellGesture(CSteamID steamID, byte id)
		{
			this.ReceiveGesture((EPlayerGesture)id);
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x000D7974 File Offset: 0x000D5B74
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellGesture")]
		public void ReceiveGesture(EPlayerGesture newGesture)
		{
			if (newGesture == EPlayerGesture.INVENTORY_START && this.gesture == EPlayerGesture.NONE)
			{
				this.play("Gesture_Inventory", true);
				this._gesture = EPlayerGesture.INVENTORY_START;
			}
			else if (newGesture == EPlayerGesture.INVENTORY_STOP && this.gesture == EPlayerGesture.INVENTORY_START)
			{
				this.stop("Gesture_Inventory");
				this._gesture = EPlayerGesture.NONE;
			}
			else if (newGesture == EPlayerGesture.PICKUP)
			{
				this.play("Gesture_Pickup", false);
				this._gesture = EPlayerGesture.NONE;
			}
			else if (newGesture == EPlayerGesture.PUNCH_LEFT)
			{
				this.play("Punch_Left", false);
				this._gesture = EPlayerGesture.NONE;
			}
			else if (newGesture == EPlayerGesture.PUNCH_RIGHT)
			{
				this.play("Punch_Right", false);
				this._gesture = EPlayerGesture.NONE;
			}
			else if (newGesture == EPlayerGesture.SURRENDER_START && this.gesture == EPlayerGesture.NONE)
			{
				this.play("Gesture_Surrender", true);
				this._gesture = EPlayerGesture.SURRENDER_START;
			}
			else if (newGesture == EPlayerGesture.SURRENDER_STOP && this.gesture == EPlayerGesture.SURRENDER_START)
			{
				this.stop("Gesture_Surrender");
				this._gesture = EPlayerGesture.NONE;
			}
			else if (newGesture == EPlayerGesture.REST_START && this.gesture == EPlayerGesture.NONE)
			{
				this.play("Gesture_Rest", true);
				this._gesture = EPlayerGesture.REST_START;
			}
			else if (newGesture == EPlayerGesture.REST_STOP && this.gesture == EPlayerGesture.REST_START)
			{
				this.stop("Gesture_Rest");
				this._gesture = EPlayerGesture.NONE;
			}
			else if (newGesture == EPlayerGesture.ARREST_START)
			{
				this.play("Gesture_Arrest", true);
				this._gesture = EPlayerGesture.ARREST_START;
			}
			else if (newGesture == EPlayerGesture.ARREST_STOP && this.gesture == EPlayerGesture.ARREST_START)
			{
				this.stop("Gesture_Arrest");
				this._gesture = EPlayerGesture.NONE;
			}
			else if (newGesture == EPlayerGesture.POINT && this.gesture == EPlayerGesture.NONE)
			{
				this.play("Gesture_Point", false);
				this._gesture = EPlayerGesture.NONE;
			}
			else if (newGesture == EPlayerGesture.WAVE && this.gesture == EPlayerGesture.NONE)
			{
				this.play("Gesture_Wave", false);
				this._gesture = EPlayerGesture.NONE;
			}
			else if (newGesture == EPlayerGesture.SALUTE && this.gesture == EPlayerGesture.NONE)
			{
				this.play("Gesture_Salute", false);
				this._gesture = EPlayerGesture.NONE;
			}
			else if (newGesture == EPlayerGesture.FACEPALM && this.gesture == EPlayerGesture.NONE)
			{
				this.play("Gesture_Facepalm", false);
				this._gesture = EPlayerGesture.NONE;
			}
			GestureUpdated gestureUpdated = this.onGestureUpdated;
			if (gestureUpdated == null)
			{
				return;
			}
			gestureUpdated(this.gesture);
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x000D7B90 File Offset: 0x000D5D90
		[Obsolete]
		public void askGesture(CSteamID steamID, byte id)
		{
			this.ReceiveGestureRequest((EPlayerGesture)id);
		}

		/// <summary>
		/// Rate limit is relatively high because this RPC handles open/close inventory notification.
		/// </summary>
		// Token: 0x06003117 RID: 12567 RVA: 0x000D7B9C File Offset: 0x000D5D9C
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 15, legacyName = "askGesture")]
		public void ReceiveGestureRequest(EPlayerGesture newGesture)
		{
			if (newGesture == EPlayerGesture.INVENTORY_STOP && base.player.inventory.isStoring && base.player.inventory.shouldInventoryStopGestureCloseStorage)
			{
				base.player.inventory.closeStorage();
			}
			if (this.gesture == EPlayerGesture.ARREST_START)
			{
				return;
			}
			if (base.player.equipment.HasValidUseable)
			{
				return;
			}
			if (base.player.stance.stance == EPlayerStance.PRONE || base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
			{
				return;
			}
			if (newGesture == EPlayerGesture.INVENTORY_START || newGesture == EPlayerGesture.INVENTORY_STOP || newGesture == EPlayerGesture.SURRENDER_START || newGesture == EPlayerGesture.SURRENDER_STOP || newGesture == EPlayerGesture.POINT || newGesture == EPlayerGesture.WAVE || newGesture == EPlayerGesture.SALUTE || newGesture == EPlayerGesture.FACEPALM || newGesture == EPlayerGesture.REST_START || newGesture == EPlayerGesture.REST_STOP)
			{
				bool flag = newGesture != EPlayerGesture.INVENTORY_START && newGesture != EPlayerGesture.INVENTORY_STOP;
				this.sendGesture(newGesture, flag);
				if (!flag && this.onInventoryGesture != null)
				{
					this.onInventoryGesture(newGesture == EPlayerGesture.INVENTORY_START);
				}
			}
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x000D7C98 File Offset: 0x000D5E98
		public void sendGesture(EPlayerGesture gesture, bool all)
		{
			if (!Provider.isServer)
			{
				if (gesture != EPlayerGesture.INVENTORY_STOP)
				{
					if (base.player.equipment.HasValidUseable)
					{
						return;
					}
					if (base.player.stance.stance == EPlayerStance.PRONE || base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
					{
						return;
					}
				}
				PlayerAnimator.SendGestureRequest.Invoke(base.GetNetId(), ENetReliability.Reliable, gesture);
				return;
			}
			if (gesture == EPlayerGesture.REST_START && base.player.stance.stance != EPlayerStance.CROUCH)
			{
				if (base.player.stance.stance != EPlayerStance.STAND && base.player.stance.stance != EPlayerStance.PRONE)
				{
					return;
				}
				base.player.stance.checkStance(EPlayerStance.CROUCH, true);
				if (base.player.stance.stance != EPlayerStance.CROUCH)
				{
					return;
				}
			}
			if (all)
			{
				PlayerAnimator.SendGesture.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), gesture);
			}
			else
			{
				PlayerAnimator.SendGesture.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GatherRemoteClientConnectionsExcludingOwner(), gesture);
			}
			Action<PlayerAnimator, EPlayerGesture> onGestureChanged_Global = PlayerAnimator.OnGestureChanged_Global;
			if (onGestureChanged_Global == null)
			{
				return;
			}
			onGestureChanged_Global.TryInvoke("OnGestureChanged_Global", this, gesture);
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x000D7DC8 File Offset: 0x000D5FC8
		private void updateState(CharacterAnimator charAnim)
		{
			if (base.player.movement.isMoving)
			{
				if (base.player.stance.stance == EPlayerStance.CLIMB)
				{
					charAnim.state("Move_Climb");
					return;
				}
				if (base.player.stance.stance == EPlayerStance.SWIM)
				{
					charAnim.state("Move_Swim");
					return;
				}
				if (base.player.stance.stance == EPlayerStance.SPRINT)
				{
					charAnim.state("Move_Run");
					return;
				}
				if (base.player.stance.stance == EPlayerStance.STAND)
				{
					charAnim.state("Move_Walk");
					return;
				}
				if (base.player.stance.stance == EPlayerStance.CROUCH)
				{
					charAnim.state("Move_Crouch");
					return;
				}
				if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					charAnim.state("Move_Prone");
					return;
				}
			}
			else if (base.player.stance.stance == EPlayerStance.DRIVING)
			{
				if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().asset.hasZip)
				{
					charAnim.state("Idle_Zip");
					return;
				}
				if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().asset.hasBicycle)
				{
					charAnim.state("Idle_Bicycle");
					charAnim.setAnimationSpeed("Idle_Bicycle", base.player.movement.getVehicle().ReplicatedForwardVelocity * base.player.movement.getVehicle().asset.bicycleAnimSpeed);
					return;
				}
				if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().asset.isReclined)
				{
					charAnim.state("Idle_Reclined");
					return;
				}
				charAnim.state("Idle_Drive");
				return;
			}
			else if (base.player.stance.stance == EPlayerStance.SITTING)
			{
				if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null)
				{
					charAnim.state("Idle_Drive");
					return;
				}
				charAnim.state("Idle_Sit");
				return;
			}
			else
			{
				if (base.player.stance.stance == EPlayerStance.CLIMB)
				{
					charAnim.state("Idle_Climb");
					return;
				}
				if (base.player.stance.stance == EPlayerStance.SWIM)
				{
					charAnim.state("Idle_Swim");
					return;
				}
				if (base.player.stance.stance == EPlayerStance.STAND || base.player.stance.stance == EPlayerStance.SPRINT)
				{
					charAnim.state("Idle_Stand");
					return;
				}
				if (base.player.stance.stance == EPlayerStance.CROUCH)
				{
					charAnim.state("Idle_Crouch");
					return;
				}
				if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					charAnim.state("Idle_Prone");
				}
			}
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x000D80E0 File Offset: 0x000D62E0
		private void updateHuman(HumanAnimator humanAnim)
		{
			humanAnim.lean = (float)(base.player.channel.owner.IsLeftHanded ? (-(float)this.lean) : this.lean);
			if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
			{
				humanAnim.pitch = 90f;
			}
			else
			{
				humanAnim.pitch = base.player.look.pitch;
			}
			if (base.player.stance.stance == EPlayerStance.CROUCH)
			{
				humanAnim.offset = 0.1f;
			}
			else if (base.player.stance.stance == EPlayerStance.PRONE)
			{
				humanAnim.offset = 0.2f;
			}
			else
			{
				humanAnim.offset = 0f;
			}
			if (!base.channel.IsLocalPlayer && Provider.isServer)
			{
				humanAnim.force();
			}
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x000D81C8 File Offset: 0x000D63C8
		private void onLanded(float velocity)
		{
			if (velocity < 0f)
			{
				if (base.player.movement.totalGravityMultiplier < 0.67f)
				{
					velocity = Mathf.Max(velocity, -5f);
				}
				else
				{
					velocity = Mathf.Max(velocity, -30f);
				}
				this.viewmodelCameraMovementLocalRotation.currentPosition.x = velocity * -0.5f;
			}
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x000D8228 File Offset: 0x000D6428
		private bool isLeanSpaceEmpty(Vector3 direction)
		{
			Vector3 vector = base.transform.position + base.transform.up * base.player.look.heightLook;
			float radius = PlayerStance.RADIUS;
			float d = 1.2f - radius;
			Vector3 point = vector + direction * d;
			return Physics.OverlapCapsuleNonAlloc(vector, point, radius, PlayerAnimator.leanHits, RayMasks.BLOCK_LEAN) == 0;
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x000D8298 File Offset: 0x000D6498
		private bool ShouldSnapLeanRotationToZero()
		{
			if (this.leanObstructed)
			{
				return true;
			}
			if (this._lean == 1)
			{
				this.leanObstructed = !this.isLeanSpaceEmpty(-base.transform.right);
			}
			else if (this._lean == -1)
			{
				this.leanObstructed = !this.isLeanSpaceEmpty(base.transform.right);
			}
			return this.leanObstructed;
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000D8304 File Offset: 0x000D6504
		public void simulate(uint simulation, bool inputLeanLeft, bool inputLeanRight)
		{
			if (base.player.stance.stance != EPlayerStance.CLIMB && base.player.stance.stance != EPlayerStance.SPRINT && base.player.stance.stance != EPlayerStance.DRIVING && base.player.stance.stance != EPlayerStance.SITTING)
			{
				if (inputLeanLeft)
				{
					if (this.isLeanSpaceEmpty(-base.transform.right))
					{
						this._lean = 1;
						this.leanObstructed = false;
					}
					else
					{
						this._lean = 0;
						this.leanObstructed = true;
					}
				}
				else if (inputLeanRight)
				{
					if (this.isLeanSpaceEmpty(base.transform.right))
					{
						this._lean = -1;
						this.leanObstructed = false;
					}
					else
					{
						this._lean = 0;
						this.leanObstructed = true;
					}
				}
				else
				{
					this._lean = 0;
					this.leanObstructed = false;
				}
			}
			else
			{
				this._lean = 0;
				this.leanObstructed = false;
			}
			if (this.lastLean != this.lean)
			{
				this.lastLean = this.lean;
				if (Provider.isServer)
				{
					if ((this.lean == -1 || this.lean == 1) && this.captorStrength > 0)
					{
						this.captorStrength -= 1;
						if (this.captorStrength == 0)
						{
							this.captorID = CSteamID.Nil;
							this.captorItem = 0;
							this.sendGesture(EPlayerGesture.ARREST_STOP, true);
							EffectAsset effectAsset = PlayerAnimator.Metal_1_Ref.Find();
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
					PlayerAnimator.SendLean.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner(), (byte)(this.lean + 1));
					PlayerAnimator.OnLeanChanged_Global.TryInvoke("OnLeanChanged_Global", this);
				}
			}
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x000D84DD File Offset: 0x000D66DD
		[Obsolete]
		public void askEmote(CSteamID steamID)
		{
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x000D84DF File Offset: 0x000D66DF
		internal void SendInitialPlayerState(SteamPlayer client)
		{
			if (this.gesture != EPlayerGesture.NONE)
			{
				PlayerAnimator.SendGesture.Invoke(base.GetNetId(), ENetReliability.Reliable, client.transportConnection, this.gesture);
			}
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x000D8506 File Offset: 0x000D6706
		internal void SendInitialPlayerState(List<ITransportConnection> transportConnections)
		{
			if (this.gesture != EPlayerGesture.NONE)
			{
				PlayerAnimator.SendGesture.Invoke(base.GetNetId(), ENetReliability.Reliable, transportConnections, this.gesture);
			}
		}

		/// <summary>
		/// Nelson 2024-03-20: Adding this method because (at the time of writing) first and third-person renderers
		/// and skeletons are activated/enabled in InitializePlayer, onPerspectiveUpdated, and onLifeUpdated, and I
		/// want them to be consistent with the addition of the new NPC Cutscene Mode option.
		/// </summary>
		// Token: 0x06003122 RID: 12578 RVA: 0x000D8528 File Offset: 0x000D6728
		private void UpdateLocalPlayerModelVisibility(bool isDead, EPlayerPerspective perspective, bool isCutsceneModeActive)
		{
			bool flag = !isDead && perspective == EPlayerPerspective.FIRST && !isCutsceneModeActive;
			bool flag2 = !isDead && perspective == EPlayerPerspective.THIRD;
			if (!this.hasCalledUpdateLocalPlayerModelVisibility || this.wasLocalPlayerFirstPersonModelVisible != flag)
			{
				this.wasLocalPlayerFirstPersonModelVisible = flag;
				if (this.firstRenderer_0 != null)
				{
					this.firstRenderer_0.enabled = flag;
				}
				this.firstSkeleton.gameObject.SetActive(flag);
			}
			if (!this.hasCalledUpdateLocalPlayerModelVisibility || this.wasLocalPlayerThirdPersonModelVisible != flag2)
			{
				this.wasLocalPlayerThirdPersonModelVisible = flag2;
				if (this.thirdRenderer_0 != null)
				{
					this.thirdRenderer_0.enabled = flag2;
				}
				if (this.thirdRenderer_1 != null)
				{
					this.thirdRenderer_1.enabled = flag2;
				}
				this.thirdSkeleton.gameObject.SetActive(flag2);
			}
			this.hasCalledUpdateLocalPlayerModelVisibility = true;
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x000D85F5 File Offset: 0x000D67F5
		internal void NotifyLocalPlayerCutsceneModeActiveChanged(bool isCutsceneModeActive)
		{
			this.UpdateLocalPlayerModelVisibility(base.player.life.isDead, base.player.look.perspective, isCutsceneModeActive);
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x000D861E File Offset: 0x000D681E
		private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
		{
			this.UpdateLocalPlayerModelVisibility(base.player.life.isDead, newPerspective, base.player.quests.IsCutsceneModeActive());
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x000D8648 File Offset: 0x000D6848
		private void Update()
		{
			if (base.channel.IsLocalPlayer)
			{
				if (!PlayerUI.window.showCursor)
				{
					if (!base.player.look.IsControllingFreecam)
					{
						if (ControlsSettings.leaning == EControlMode.TOGGLE)
						{
							if (InputEx.GetKeyDown(ControlsSettings.leanLeft))
							{
								if (base.player.look.perspective == EPlayerPerspective.FIRST || this.side)
								{
									if (this.leanLeft)
									{
										this.inputWantsToLeanLeft = false;
										this.inputWantsToLeanRight = false;
									}
									else
									{
										this.inputWantsToLeanLeft = true;
										this.inputWantsToLeanRight = false;
									}
								}
								if (!this.side && this.leanRight)
								{
									this.inputWantsToLeanLeft = false;
									this.inputWantsToLeanRight = false;
								}
								this.inputWantsThirdPersonCameraOnLeftSide = true;
							}
							if (InputEx.GetKeyDown(ControlsSettings.leanRight))
							{
								if (base.player.look.perspective == EPlayerPerspective.FIRST || !this.side)
								{
									if (this.leanRight)
									{
										this.inputWantsToLeanLeft = false;
										this.inputWantsToLeanRight = false;
									}
									else
									{
										this.inputWantsToLeanLeft = false;
										this.inputWantsToLeanRight = true;
									}
								}
								if (this.side && this.leanLeft)
								{
									this.inputWantsToLeanLeft = false;
									this.inputWantsToLeanRight = false;
								}
								this.inputWantsThirdPersonCameraOnLeftSide = false;
							}
						}
						else
						{
							if (InputEx.GetKeyDown(ControlsSettings.leanLeft))
							{
								this.inputWantsThirdPersonCameraOnLeftSide = true;
								this.lastCameraSideInputRealtime = Time.realtimeSinceStartup;
							}
							if (InputEx.GetKeyDown(ControlsSettings.leanRight))
							{
								this.inputWantsThirdPersonCameraOnLeftSide = false;
								this.lastCameraSideInputRealtime = Time.realtimeSinceStartup;
							}
							if (InputEx.GetKey(ControlsSettings.leanLeft))
							{
								if (base.player.look.perspective == EPlayerPerspective.FIRST || Time.realtimeSinceStartup - this.lastCameraSideInputRealtime > 0.075f)
								{
									this.inputWantsToLeanLeft = true;
								}
								else
								{
									this.inputWantsToLeanLeft = false;
								}
							}
							else
							{
								this.inputWantsToLeanLeft = false;
							}
							if (InputEx.GetKey(ControlsSettings.leanRight))
							{
								if (base.player.look.perspective == EPlayerPerspective.FIRST || Time.realtimeSinceStartup - this.lastCameraSideInputRealtime > 0.075f)
								{
									this.inputWantsToLeanRight = true;
								}
								else
								{
									this.inputWantsToLeanRight = false;
								}
							}
							else
							{
								this.inputWantsToLeanRight = false;
							}
						}
					}
				}
				else
				{
					this.inputWantsToLeanLeft = false;
					this.inputWantsToLeanRight = false;
				}
				if (this.firstAnimator != null)
				{
					if (this.firstAnimator.getAnimationPlaying())
					{
						this.firstAnimator.state("Idle_Stand");
					}
					else
					{
						this.updateState(this.firstAnimator);
					}
				}
				if (this.thirdAnimator != null)
				{
					this.updateState(this.thirdAnimator);
					this.updateHuman((HumanAnimator)this.thirdAnimator);
				}
				this.blendedViewmodelSwayMultiplier = Mathf.Lerp(this.blendedViewmodelSwayMultiplier, this.viewmodelSwayMultiplier, 16f * Time.deltaTime);
				this.blendedViewmodelOffsetPreferenceMultiplier = Mathf.Lerp(this.blendedViewmodelOffsetPreferenceMultiplier, this.viewmodelOffsetPreferenceMultiplier, 16f * Time.deltaTime);
				if (base.player.movement.isMoving)
				{
					this.viewmodelMovementOffset.targetPosition.x = Mathf.Sin(this.speed * Time.time) * this.bob;
					this.viewmodelMovementOffset.targetPosition.y = Mathf.Abs(this.viewmodelMovementOffset.targetPosition.x);
				}
				else
				{
					this.viewmodelMovementOffset.targetPosition = Vector2.zero;
				}
				this.viewmodelMovementOffset.Update(Time.deltaTime);
				Vector3 b;
				float d;
				this.GetAimingViewmodelAlignment(out b, out d);
				this.blendedViewmodelCameraLocalPositionOffset = Vector3.Lerp(this.blendedViewmodelCameraLocalPositionOffset, this.viewmodelCameraLocalPositionOffset - this.recoilViewmodelCameraOffset.currentPosition - this.bayonetViewmodelCameraOffset, 16f * Time.deltaTime);
				this.recoilViewmodelCameraOffset.Update(Time.deltaTime);
				this.bayonetViewmodelCameraOffset = Vector3.Lerp(this.bayonetViewmodelCameraOffset, Vector3.zero, 16f * Time.deltaTime);
				this.desiredViewmodelCameraLocalPosition.x = -this.viewmodelMovementOffset.currentPosition.y - this.blendedViewmodelCameraLocalPositionOffset.y;
				this.desiredViewmodelCameraLocalPosition.y = this.viewmodelMovementOffset.currentPosition.x + this.blendedViewmodelCameraLocalPositionOffset.x;
				this.desiredViewmodelCameraLocalPosition.z = this.blendedViewmodelCameraLocalPositionOffset.z;
				this.desiredViewmodelCameraLocalPosition.x = this.desiredViewmodelCameraLocalPosition.x + Provider.preferenceData.Viewmodel.Offset_Vertical * this.blendedViewmodelOffsetPreferenceMultiplier;
				this.desiredViewmodelCameraLocalPosition.y = this.desiredViewmodelCameraLocalPosition.y + Provider.preferenceData.Viewmodel.Offset_Horizontal * this.blendedViewmodelOffsetPreferenceMultiplier;
				this.desiredViewmodelCameraLocalPosition.z = this.desiredViewmodelCameraLocalPosition.z - Provider.preferenceData.Viewmodel.Offset_Depth * this.blendedViewmodelOffsetPreferenceMultiplier;
				if (base.player.stance.stance == EPlayerStance.DRIVING)
				{
					this.viewmodelCameraLocalPosition.x = Mathf.Lerp(this.viewmodelCameraLocalPosition.x, -this.turretViewmodelCameraLocalPositionOffset.y - 0.65f - Mathf.Abs(base.player.look.yaw) / 90f * 0.25f, 8f * Time.deltaTime);
					this.viewmodelCameraLocalPosition.y = Mathf.Lerp(this.viewmodelCameraLocalPosition.y, this.turretViewmodelCameraLocalPositionOffset.x + (float)(base.channel.owner.IsLeftHanded ? -1 : 1) * base.player.movement.getVehicle().AnimatedSteeringAngle * -0.01f, 8f * Time.deltaTime);
					this.viewmodelCameraLocalPosition.z = Mathf.Lerp(this.viewmodelCameraLocalPosition.z, this.turretViewmodelCameraLocalPositionOffset.z - 0.25f, 8f * Time.deltaTime);
				}
				else
				{
					this.viewmodelCameraLocalPosition.x = this.desiredViewmodelCameraLocalPosition.x - 0.45f;
					this.viewmodelCameraLocalPosition.y = this.desiredViewmodelCameraLocalPosition.y;
					this.viewmodelCameraLocalPosition.z = this.desiredViewmodelCameraLocalPosition.z;
				}
				this.AddNearDeathViewmodelShake(ref this.viewmodelCameraLocalPosition);
				this.viewmodelCameraTransform.localPosition = this.viewmodelCameraLocalPosition + b;
				if (base.player.movement.isMoving)
				{
					this.viewmodelCameraMovementLocalRotation.targetPosition.x = base.player.movement.move.z * this.tilt * this.viewmodelSwayMultiplier + this.roll * this.viewmodelSwayMultiplier;
					this.viewmodelCameraMovementLocalRotation.targetPosition.y = base.player.movement.move.x * this.tilt + this.roll * this.viewmodelSwayMultiplier;
				}
				else
				{
					this.viewmodelCameraMovementLocalRotation.targetPosition = Vector2.zero;
				}
				if (!base.player.movement.isGrounded)
				{
					this.viewmodelCameraMovementLocalRotation.targetPosition.x = this.viewmodelCameraMovementLocalRotation.targetPosition.x - 5f;
				}
				this.viewmodelCameraMovementLocalRotation.Update(Time.deltaTime);
				this.viewmodelCameraLocalRotation.x = this.viewmodelCameraMovementLocalRotation.currentPosition.x;
				this.viewmodelCameraLocalRotation.y = 0f;
				this.viewmodelCameraLocalRotation.z = this.viewmodelCameraMovementLocalRotation.currentPosition.y;
				this.viewmodelCameraLocalRotation += this.recoilViewmodelCameraRotation.currentPosition;
				this.recoilViewmodelCameraRotation.Update(Time.deltaTime);
				float num = Mathf.DeltaAngle(base.player.look.pitch, this.lastFramePitchInput);
				this.lastFramePitchInput = base.player.look.pitch;
				float num2 = Mathf.DeltaAngle(base.player.look.yaw, this.lastFrameYawInput);
				this.lastFrameYawInput = base.player.look.yaw;
				this.rotationInputViewmodelRoll.Update(Time.deltaTime);
				this.rotationInputViewmodelRoll.currentPosition.x = this.rotationInputViewmodelRoll.currentPosition.x + num * -0.03f * this.viewmodelSwayMultiplier;
				this.rotationInputViewmodelRoll.currentPosition.y = this.rotationInputViewmodelRoll.currentPosition.y + num2 * -0.015f * this.viewmodelSwayMultiplier;
				this.rotationInputViewmodelRoll.currentPosition.z = this.rotationInputViewmodelRoll.currentPosition.z + num2 * -0.05f;
				this.rotationInputViewmodelRoll.currentPosition = MathfEx.Clamp(this.rotationInputViewmodelRoll.currentPosition, -10f, 10f);
				this.viewmodelCameraLocalRotation += this.rotationInputViewmodelRoll.currentPosition;
				this.viewmodelItemInertiaRotation.Update(Time.deltaTime);
				if (base.player.look.perspective == EPlayerPerspective.FIRST && base.player.equipment.firstModel != null)
				{
					ItemAsset asset = base.player.equipment.asset;
					if (asset != null && asset.shouldProcedurallyAnimateInertia)
					{
						Vector3 a = this.viewmodelParentTransform.transform.InverseTransformPoint(base.player.equipment.firstModel.position);
						if (this.lastFrameHadItemPosition)
						{
							Vector3 vector = a - this.lastFrameItemPosition;
							this.viewmodelItemInertiaRotation.currentPosition.x = this.viewmodelItemInertiaRotation.currentPosition.x + vector.y * this.viewmodelItemInertiaMask.x;
							this.viewmodelItemInertiaRotation.currentPosition.y = this.viewmodelItemInertiaRotation.currentPosition.y + vector.x * this.viewmodelItemInertiaMask.y;
							this.viewmodelItemInertiaRotation.currentPosition.z = this.viewmodelItemInertiaRotation.currentPosition.z + vector.x * this.viewmodelItemInertiaMask.z;
						}
						this.lastFrameItemPosition = a;
						this.lastFrameHadItemPosition = true;
						goto IL_986;
					}
				}
				this.lastFrameHadItemPosition = false;
				IL_986:
				this.viewmodelItemInertiaRotation.currentPosition = MathfEx.Clamp(this.viewmodelItemInertiaRotation.currentPosition, -5f, 5f);
				this.viewmodelCameraLocalRotation += this.viewmodelItemInertiaRotation.currentPosition * d;
				this.viewmodelSmoothedExplosionLocalRotation = Quaternion.Lerp(this.viewmodelSmoothedExplosionLocalRotation, this.viewmodelTargetExplosionLocalRotation.currentRotation, this.viewmodelExplosionSmoothingSpeed * Time.deltaTime);
				this.viewmodelTargetExplosionLocalRotation.Update(Time.deltaTime);
				if (base.player.stance.stance == EPlayerStance.DRIVING)
				{
					this.viewmodelCameraTransform.localRotation = Quaternion.Lerp(this.viewmodelCameraTransform.localRotation, Quaternion.Euler(base.player.look.yaw * 60f / MainCamera.instance.fieldOfView * (float)(base.channel.owner.IsLeftHanded ? 1 : -1), (base.player.look.pitch - 90f) * 60f / MainCamera.instance.fieldOfView, 90f + base.player.movement.getVehicle().AnimatedSteeringAngle * (float)(base.channel.owner.IsLeftHanded ? -1 : 1)), 8f * Time.deltaTime);
				}
				else if (base.player.stance.stance == EPlayerStance.CLIMB)
				{
					this.viewmodelCameraTransform.localRotation = Quaternion.Lerp(this.viewmodelCameraTransform.localRotation, Quaternion.Euler(0f, (base.player.look.pitch - 90f) * 60f / MainCamera.instance.fieldOfView, 90f), 8f * Time.deltaTime);
				}
				else
				{
					this.viewmodelCameraTransform.localRotation = this.viewmodelTargetExplosionLocalRotation.currentRotation * Quaternion.Euler(this.viewmodelCameraLocalRotation.y, -this.viewmodelCameraLocalRotation.x, this.viewmodelCameraLocalRotation.z + 90f);
				}
				if (this.ShouldSnapLeanRotationToZero())
				{
					base.player.first.transform.localRotation = Quaternion.identity;
				}
				else
				{
					base.player.first.transform.localRotation = Quaternion.Lerp(base.player.first.transform.localRotation, Quaternion.Euler(0f, 0f, (float)this.lean * HumanAnimator.LEAN), 4f * Time.deltaTime);
				}
				this.viewmodelCamera.fieldOfView = Mathf.Lerp(Provider.preferenceData.Viewmodel.Field_Of_View_Aim, Provider.preferenceData.Viewmodel.Field_Of_View_Hip, this.blendedViewmodelOffsetPreferenceMultiplier);
				if (Provider.modeConfigData.Gameplay.Allow_Shoulder_Camera)
				{
					this._shoulder = Mathf.Lerp(this.shoulder, (float)(this.side ? -1 : 1), 8f * Time.deltaTime);
				}
				else
				{
					this._shoulder = 0f;
				}
				this._shoulder2 = Mathf.Lerp(this.shoulder2, (float)(-(float)this.lean), 8f * Time.deltaTime);
			}
			else if (this.thirdAnimator != null)
			{
				this.updateState(this.thirdAnimator);
				this.updateHuman((HumanAnimator)this.thirdAnimator);
			}
			if (this.characterAnimator != null)
			{
				this.updateState(this.characterAnimator);
				this.updateHuman(this.characterAnimator);
			}
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x000D9358 File Offset: 0x000D7558
		internal void InitializePlayer()
		{
			this.isHiddenWaitingForClothing = true;
			if (base.channel.IsLocalPlayer)
			{
				if (base.player.first != null)
				{
					this.viewmodelParentTransform = new GameObject().transform;
					this.viewmodelParentTransform.name = "View";
					this.viewmodelParentTransform.transform.localPosition = Vector3.zero;
					this.firstAnimator = MainCamera.instance.transform.Find("Viewmodel").GetComponent<CharacterAnimator>();
					Vector3 localPosition = this.firstAnimator.transform.localPosition;
					Quaternion localRotation = this.firstAnimator.transform.localRotation;
					this.firstAnimator.transform.parent = this.viewmodelParentTransform;
					this.firstAnimator.transform.localPosition = localPosition;
					this.firstAnimator.transform.localRotation = localRotation;
					this.firstAnimator.transform.localScale = new Vector3((float)(base.channel.owner.IsLeftHanded ? -1 : 1), 1f, 1f);
					this.firstRenderer_0 = (SkinnedMeshRenderer)this.firstAnimator.transform.Find("Model_0").GetComponent<Renderer>();
					this._firstSkeleton = this.firstAnimator.transform.Find("Skeleton");
				}
				if (base.player.third != null)
				{
					this.thirdAnimator = base.player.third.GetComponent<CharacterAnimator>();
					this.thirdAnimator.transform.localScale = new Vector3((float)(base.channel.owner.IsLeftHanded ? -1 : 1), 1f, 1f);
					this.thirdRenderer_0 = (SkinnedMeshRenderer)this.thirdAnimator.transform.Find("Model_0").GetComponent<Renderer>();
					this.thirdRenderer_1 = (SkinnedMeshRenderer)this.thirdAnimator.transform.Find("Model_1").GetComponent<Renderer>();
					this._thirdSkeleton = this.thirdAnimator.transform.Find("Skeleton");
					this.thirdSkeleton.Find("Spine").GetComponent<Collider>().enabled = false;
					this.thirdSkeleton.Find("Spine").Find("Skull").GetComponent<Collider>().enabled = false;
					this.thirdSkeleton.Find("Spine").Find("Left_Shoulder").Find("Left_Arm").GetComponent<Collider>().enabled = false;
					this.thirdSkeleton.Find("Spine").Find("Right_Shoulder").Find("Right_Arm").GetComponent<Collider>().enabled = false;
					this.thirdSkeleton.Find("Left_Hip").Find("Left_Leg").GetComponent<Collider>().enabled = false;
					this.thirdSkeleton.Find("Right_Hip").Find("Right_Leg").GetComponent<Collider>().enabled = false;
				}
				if (Provider.cameraMode == ECameraMode.THIRD)
				{
					this.UpdateLocalPlayerModelVisibility(false, EPlayerPerspective.THIRD, base.player.quests.IsCutsceneModeActive());
				}
				else
				{
					this.UpdateLocalPlayerModelVisibility(false, EPlayerPerspective.FIRST, base.player.quests.IsCutsceneModeActive());
				}
				this.viewmodelCameraTransform = this.firstSkeleton.Find("Spine").Find("Skull").Find("ViewmodelCamera");
				this.viewmodelCamera = this.viewmodelCameraTransform.GetComponent<Camera>();
				UnturnedPostProcess.instance.setOverlayCamera(this.viewmodelCamera);
				this.viewmodelCameraLocalPositionOffset = Vector3.zero;
				this.turretViewmodelCameraLocalPositionOffset = Vector3.zero;
				this.scopeSway = Vector3.zero;
				this.bayonetViewmodelCameraOffset = Vector3.zero;
				this.viewmodelCameraLocalPosition = Vector3.zero;
				this.viewmodelTargetExplosionLocalRotation.currentRotation = Quaternion.identity;
				this.viewmodelTargetExplosionLocalRotation.targetRotation = Quaternion.identity;
				this.blendedViewmodelSwayMultiplier = 1f;
				this.viewmodelSwayMultiplier = 1f;
				this.blendedViewmodelOffsetPreferenceMultiplier = 1f;
				this.viewmodelOffsetPreferenceMultiplier = 1f;
				if (base.player.character != null)
				{
					this.characterAnimator = base.player.character.GetComponent<HumanAnimator>();
					this.characterAnimator.transform.localScale = new Vector3((float)(base.channel.owner.IsLeftHanded ? -1 : 1), 1f, 1f);
				}
				PlayerMovement movement = base.player.movement;
				movement.onLanded = (Landed)Delegate.Combine(movement.onLanded, new Landed(this.onLanded));
				this.inputWantsThirdPersonCameraOnLeftSide = base.player.channel.owner.IsLeftHanded;
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
			}
			else if (base.player.third != null)
			{
				this.thirdAnimator = base.player.third.GetComponent<CharacterAnimator>();
				this.thirdAnimator.transform.localScale = new Vector3((float)(base.channel.owner.IsLeftHanded ? -1 : 1), 1f, 1f);
				this._thirdSkeleton = this.thirdAnimator.transform.Find("Skeleton");
			}
			this.thirdSkeleton.gameObject.SetActive(true);
			this.mixAnimation("Gesture_Inventory", true, true, true);
			this.mixAnimation("Gesture_Pickup", false, true);
			this.mixAnimation("Punch_Left", true, false);
			this.mixAnimation("Punch_Right", false, true);
			this.mixAnimation("Gesture_Point", false, true);
			this.mixAnimation("Gesture_Surrender", true, true);
			this.mixAnimation("Gesture_Arrest", true, true);
			this.mixAnimation("Gesture_Wave", true, true, true);
			this.mixAnimation("Gesture_Salute", false, true);
			this.mixAnimation("Gesture_Rest");
			this.mixAnimation("Gesture_Facepalm", false, true, true);
			PlayerLife life = base.player.life;
			life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
			if (Provider.isServer)
			{
				this.load();
			}
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x000D9998 File Offset: 0x000D7B98
		private void AddNearDeathViewmodelShake(ref Vector3 position)
		{
			if (base.player.life.health < 25)
			{
				Vector3 a = new Vector3(Random.Range(-0.005f, 0.005f), Random.Range(-0.005f, 0.005f), Random.Range(-0.005f, 0.005f));
				float d = 1f - (float)Player.player.life.health / 25f;
				float d2 = 1f - base.player.skills.mastery(1, 3) * 0.75f;
				position += a * d * d2;
			}
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x000D9A4C File Offset: 0x000D7C4C
		private void GetAimingViewmodelAlignment(out Vector3 aimingAlignmentOffset, out float aimingInertaMultiplier)
		{
			aimingAlignmentOffset = Vector3.zero;
			aimingInertaMultiplier = 1f;
			UseableGun useableGun = base.player.equipment.useable as UseableGun;
			if (useableGun != null)
			{
				Transform transform;
				Vector3 position;
				float num;
				useableGun.GetAimingViewmodelAlignment(out transform, out position, out num);
				if (transform != null && num > 0f)
				{
					Vector3 position2 = transform.TransformPoint(position);
					aimingAlignmentOffset = this.viewmodelCameraTransform.parent.InverseTransformPoint(position2);
					aimingAlignmentOffset.x += 0.45f;
					aimingAlignmentOffset *= num;
					aimingInertaMultiplier -= num;
				}
			}
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x000D9AEC File Offset: 0x000D7CEC
		public void load()
		{
			this.wasLoadCalled = true;
			if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Anim.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				Block block = PlayerSavedata.readBlock(base.channel.owner.playerID, "/Player/Anim.dat", 0);
				int num = (int)block.readByte();
				this._gesture = (EPlayerGesture)block.readByte();
				this.captorID = block.readSteamID();
				if (num > 1)
				{
					this.captorItem = block.readUInt16();
				}
				else
				{
					this.captorItem = 0;
				}
				this.captorStrength = block.readUInt16();
				if (this.gesture != EPlayerGesture.ARREST_START)
				{
					this._gesture = EPlayerGesture.NONE;
				}
				return;
			}
			this._gesture = EPlayerGesture.NONE;
			this.captorID = CSteamID.Nil;
			this.captorItem = 0;
			this.captorStrength = 0;
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x000D9BB8 File Offset: 0x000D7DB8
		public void save()
		{
			if (!this.wasLoadCalled)
			{
				return;
			}
			if (base.player.life.isDead)
			{
				if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Anim.dat"))
				{
					PlayerSavedata.deleteFile(base.channel.owner.playerID, "/Player/Anim.dat");
					return;
				}
			}
			else
			{
				Block block = new Block();
				block.writeByte(PlayerAnimator.SAVEDATA_VERSION);
				block.writeByte((byte)this.gesture);
				block.writeSteamID(this.captorID);
				block.writeUInt16(this.captorItem);
				block.writeUInt16(this.captorStrength);
				PlayerSavedata.writeBlock(base.channel.owner.playerID, "/Player/Anim.dat", block);
			}
		}

		// Token: 0x04001BCA RID: 7114
		public static readonly byte SAVEDATA_VERSION = 2;

		// Token: 0x04001BCB RID: 7115
		private static readonly float BOB_SPRINT = 0.075f;

		// Token: 0x04001BCC RID: 7116
		private static readonly float BOB_STAND = 0.05f;

		// Token: 0x04001BCD RID: 7117
		private static readonly float BOB_CROUCH = 0.025f;

		// Token: 0x04001BCE RID: 7118
		private static readonly float BOB_PRONE = 0.0125f;

		// Token: 0x04001BCF RID: 7119
		private static readonly float BOB_SWIM = 0.025f;

		// Token: 0x04001BD0 RID: 7120
		private static readonly float TILT_SPRINT = 5f;

		// Token: 0x04001BD1 RID: 7121
		private static readonly float TILT_STAND = 3f;

		// Token: 0x04001BD2 RID: 7122
		private static readonly float TILT_CROUCH = 2f;

		// Token: 0x04001BD3 RID: 7123
		private static readonly float TILT_PRONE = 1f;

		// Token: 0x04001BD4 RID: 7124
		private static readonly float TILT_SWIM = 10f;

		// Token: 0x04001BD5 RID: 7125
		private static readonly float SPEED_SPRINT = 10f;

		// Token: 0x04001BD6 RID: 7126
		private static readonly float SPEED_STAND = 8f;

		// Token: 0x04001BD7 RID: 7127
		private static readonly float SPEED_CROUCH = 6f;

		// Token: 0x04001BD8 RID: 7128
		private static readonly float SPEED_PRONE = 4f;

		// Token: 0x04001BD9 RID: 7129
		private static readonly float SPEED_SWIM = 6f;

		// Token: 0x04001BDA RID: 7130
		public GestureUpdated onGestureUpdated;

		/// <summary>
		/// Empty transform created at the world origin.
		/// The first-person Viewmodel transform is re-parented to this.
		/// </summary>
		// Token: 0x04001BDC RID: 7132
		public Transform viewmodelParentTransform;

		// Token: 0x04001BDD RID: 7133
		private CharacterAnimator firstAnimator;

		// Token: 0x04001BDE RID: 7134
		private CharacterAnimator thirdAnimator;

		// Token: 0x04001BDF RID: 7135
		private HumanAnimator characterAnimator;

		// Token: 0x04001BE0 RID: 7136
		private SkinnedMeshRenderer firstRenderer_0;

		// Token: 0x04001BE1 RID: 7137
		private SkinnedMeshRenderer thirdRenderer_0;

		// Token: 0x04001BE2 RID: 7138
		private SkinnedMeshRenderer thirdRenderer_1;

		// Token: 0x04001BE3 RID: 7139
		private Transform _firstSkeleton;

		// Token: 0x04001BE4 RID: 7140
		private Transform _thirdSkeleton;

		/// <summary>
		/// Child of the first-person skull transform.
		/// </summary>
		// Token: 0x04001BE5 RID: 7141
		public Transform viewmodelCameraTransform;

		/// <summary>
		/// Camera near world origin masking the first-person arms and weapon.
		/// </summary>
		// Token: 0x04001BE6 RID: 7142
		public Camera viewmodelCamera;

		/// <summary>
		/// Constant (non-animated) offset. Used by gun to center the 3D sights on screen, and by chainsaw to shake the viewmodel.
		/// </summary>
		// Token: 0x04001BE7 RID: 7143
		public Vector3 viewmodelCameraLocalPositionOffset;

		/// <summary>
		/// Used to hide viewmodel arms while using a vehicle turret gun.
		/// </summary>
		// Token: 0x04001BE8 RID: 7144
		public Vector3 turretViewmodelCameraLocalPositionOffset;

		/// <summary>
		/// Offsets main camera and aim rotation while aiming with a scoped gun.
		/// </summary>
		// Token: 0x04001BE9 RID: 7145
		public Vector3 scopeSway;

		/// <summary>
		/// Animated toward viewmodelSwayMultiplier.
		/// </summary>
		// Token: 0x04001BEA RID: 7146
		private float blendedViewmodelSwayMultiplier;

		/// <summary>
		/// Small number (0.1) while aiming, 1 while not aiming.
		/// Reduces viewmodel animation while aiming to make 3D sights more usable.
		/// </summary>
		// Token: 0x04001BEB RID: 7147
		public float viewmodelSwayMultiplier;

		/// <summary>
		/// Animated toward viewmodelOffsetPreferenceMultiplier.
		/// </summary>
		// Token: 0x04001BEC RID: 7148
		private float blendedViewmodelOffsetPreferenceMultiplier;

		/// <summary>
		/// 0 while aiming, 1 while not aiming.
		/// Players can customize the 3D position of the viewmodel on screen, but this needs
		/// to be blended out while aiming down sights otherwise it would not line up with
		/// the center of the screen.
		/// </summary>
		// Token: 0x04001BED RID: 7149
		public float viewmodelOffsetPreferenceMultiplier;

		/// <summary>
		/// Animated toward viewmodelCameraLocalPositionOffset, recoil, and bayonet offsets.
		/// </summary>
		// Token: 0x04001BEE RID: 7150
		private Vector3 blendedViewmodelCameraLocalPositionOffset;

		/// <summary>
		/// Abruptly offset when gun is fired, then animated back toward zero.
		/// </summary>
		// Token: 0x04001BEF RID: 7151
		public Rk4Spring3 recoilViewmodelCameraOffset;

		/// <summary>
		/// Abruptly offset when gun is fired, then animated back toward zero.
		/// x = pitch, y = yaw, z = roll
		/// </summary>
		// Token: 0x04001BF0 RID: 7152
		public Rk4Spring3 recoilViewmodelCameraRotation;

		// Token: 0x04001BF1 RID: 7153
		public Vector3 recoilViewmodelCameraMask = Vector3.one;

		/// <summary>
		/// Abruptly offset when bayonet is used, then animated back toward zero.
		/// </summary>
		// Token: 0x04001BF2 RID: 7154
		private Vector3 bayonetViewmodelCameraOffset;

		/// <summary>
		/// Animated while player is moving.
		/// </summary>
		// Token: 0x04001BF3 RID: 7155
		public Rk4Spring2 viewmodelMovementOffset;

		/// <summary>
		/// Blended from multiple viewmodel parameters and then applied to viewmodelCameraTransform.
		/// </summary>
		// Token: 0x04001BF4 RID: 7156
		private Vector3 viewmodelCameraLocalPosition;

		// Token: 0x04001BF5 RID: 7157
		public Rk4SpringQ viewmodelTargetExplosionLocalRotation;

		/// <summary>
		/// Smoothing adds some initial blend-in which felt nicer for explosion rumble.
		/// </summary>
		// Token: 0x04001BF6 RID: 7158
		private Quaternion viewmodelSmoothedExplosionLocalRotation = Quaternion.identity;

		// Token: 0x04001BF7 RID: 7159
		public float viewmodelExplosionSmoothingSpeed;

		/// <summary>
		/// Meshes are disabled until clothing is received.
		/// </summary>
		// Token: 0x04001BF8 RID: 7160
		private bool isHiddenWaitingForClothing;

		/// <summary>
		/// Target viewmodelCameraLocalPosition except while driving.
		/// </summary>
		// Token: 0x04001BF9 RID: 7161
		private Vector3 desiredViewmodelCameraLocalPosition;

		/// <summary>
		/// Animated while playing is moving.
		/// x = pitch, y = roll
		/// </summary>
		// Token: 0x04001BFA RID: 7162
		public Rk4Spring2 viewmodelCameraMovementLocalRotation;

		// Token: 0x04001BFB RID: 7163
		private Vector3 viewmodelCameraLocalRotation;

		/// <summary>
		/// Used to measure change in pitch between frames.
		/// </summary>
		// Token: 0x04001BFC RID: 7164
		private float lastFramePitchInput;

		/// <summary>
		/// Used to measure change in yaw between frames.
		/// </summary>
		// Token: 0x04001BFD RID: 7165
		private float lastFrameYawInput;

		/// <summary>
		/// Animated according to change in pitch/yaw input between frames so that gun rolls slightly while turning.
		/// </summary>
		// Token: 0x04001BFE RID: 7166
		public Rk4Spring3 rotationInputViewmodelRoll;

		// Token: 0x04001BFF RID: 7167
		private bool lastFrameHadItemPosition;

		// Token: 0x04001C00 RID: 7168
		private Vector3 lastFrameItemPosition;

		/// <summary>
		/// Animated according to change in item position between frames so that animations have more inertia.
		/// </summary>
		// Token: 0x04001C01 RID: 7169
		public Rk4Spring3 viewmodelItemInertiaRotation;

		/// <summary>
		/// Degrees per meter of item distance travelled.
		/// Pitch is driven by vertical displacement, yaw and roll are driven by horizontal.
		/// x = pitch, y = yaw, z = roll
		/// </summary>
		// Token: 0x04001C02 RID: 7170
		public Vector3 viewmodelItemInertiaMask;

		// Token: 0x04001C03 RID: 7171
		private bool inputWantsToLeanLeft;

		// Token: 0x04001C04 RID: 7172
		private bool inputWantsToLeanRight;

		// Token: 0x04001C05 RID: 7173
		internal bool leanObstructed;

		/// <summary>
		/// In third-person this delays leaning in case player only wanted
		/// to switch camera side without leaning.
		/// </summary>
		// Token: 0x04001C06 RID: 7174
		private float lastCameraSideInputRealtime;

		// Token: 0x04001C07 RID: 7175
		private int lastLean;

		// Token: 0x04001C08 RID: 7176
		private int _lean;

		// Token: 0x04001C09 RID: 7177
		private float _shoulder;

		// Token: 0x04001C0A RID: 7178
		private float _shoulder2;

		// Token: 0x04001C0B RID: 7179
		private bool inputWantsThirdPersonCameraOnLeftSide;

		// Token: 0x04001C0C RID: 7180
		private EPlayerGesture _gesture;

		// Token: 0x04001C0D RID: 7181
		public CSteamID captorID;

		// Token: 0x04001C0E RID: 7182
		public ushort captorItem;

		// Token: 0x04001C0F RID: 7183
		public ushort captorStrength;

		// Token: 0x04001C11 RID: 7185
		private static readonly ClientInstanceMethod<byte> SendLean = ClientInstanceMethod<byte>.Get(typeof(PlayerAnimator), "ReceiveLean");

		// Token: 0x04001C12 RID: 7186
		private static readonly ClientInstanceMethod<EPlayerGesture> SendGesture = ClientInstanceMethod<EPlayerGesture>.Get(typeof(PlayerAnimator), "ReceiveGesture");

		/// <summary>
		/// Event for server plugins to monitor whether player is in-inventory.
		/// </summary>
		// Token: 0x04001C13 RID: 7187
		public PlayerAnimator.InventoryGestureListener onInventoryGesture;

		// Token: 0x04001C14 RID: 7188
		private static readonly ServerInstanceMethod<EPlayerGesture> SendGestureRequest = ServerInstanceMethod<EPlayerGesture>.Get(typeof(PlayerAnimator), "ReceiveGestureRequest");

		// Token: 0x04001C15 RID: 7189
		private static Collider[] leanHits = new Collider[1];

		// Token: 0x04001C16 RID: 7190
		private static readonly AssetReference<EffectAsset> Metal_1_Ref = new AssetReference<EffectAsset>("805bb3b0752749d1b5cf9959d17e104e");

		// Token: 0x04001C17 RID: 7191
		private bool hasCalledUpdateLocalPlayerModelVisibility;

		// Token: 0x04001C18 RID: 7192
		private bool wasLocalPlayerFirstPersonModelVisible;

		// Token: 0x04001C19 RID: 7193
		private bool wasLocalPlayerThirdPersonModelVisible;

		// Token: 0x04001C1A RID: 7194
		private bool wasLoadCalled;

		// Token: 0x020009A2 RID: 2466
		// (Invoke) Token: 0x06004BDB RID: 19419
		public delegate void InventoryGestureListener(bool InInventory);
	}
}
