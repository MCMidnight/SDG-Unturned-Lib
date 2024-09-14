using System;
using SDG.Framework.Foliage;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace SDG.Unturned
{
	// Token: 0x0200063E RID: 1598
	public class PlayerLook : PlayerCaller
	{
		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06003415 RID: 13333 RVA: 0x000EE1CC File Offset: 0x000EC3CC
		public float heightLook
		{
			get
			{
				if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					return PlayerLook.HEIGHT_LOOK_SIT;
				}
				if (base.player.stance.stance == EPlayerStance.STAND || base.player.stance.stance == EPlayerStance.SPRINT || base.player.stance.stance == EPlayerStance.CLIMB || base.player.stance.stance == EPlayerStance.SWIM || base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					return PlayerLook.HEIGHT_LOOK_STAND;
				}
				if (base.player.stance.stance == EPlayerStance.CROUCH)
				{
					return PlayerLook.HEIGHT_LOOK_CROUCH;
				}
				if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					return PlayerLook.HEIGHT_LOOK_PRONE;
				}
				return 0f;
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06003416 RID: 13334 RVA: 0x000EE2B4 File Offset: 0x000EC4B4
		private float heightCamera
		{
			get
			{
				if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					return PlayerLook.HEIGHT_CAMERA_SIT;
				}
				if (base.player.stance.stance == EPlayerStance.STAND || base.player.stance.stance == EPlayerStance.SPRINT || base.player.stance.stance == EPlayerStance.CLIMB || base.player.stance.stance == EPlayerStance.SWIM || base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					return PlayerLook.HEIGHT_CAMERA_STAND;
				}
				if (base.player.stance.stance == EPlayerStance.CROUCH)
				{
					return PlayerLook.HEIGHT_CAMERA_CROUCH;
				}
				if (base.player.stance.stance == EPlayerStance.PRONE)
				{
					return PlayerLook.HEIGHT_CAMERA_PRONE;
				}
				return 0f;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06003417 RID: 13335 RVA: 0x000EE39B File Offset: 0x000EC59B
		public Camera characterCamera
		{
			get
			{
				return this._characterCamera;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06003418 RID: 13336 RVA: 0x000EE3A3 File Offset: 0x000EC5A3
		public Camera scopeCamera
		{
			get
			{
				return this._scopeCamera;
			}
		}

		/// <summary>
		/// Material instantiated when dual-render scopes are enabled.
		/// Overrides the material of the gun sight attachment.
		/// </summary>
		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06003419 RID: 13337 RVA: 0x000EE3AB File Offset: 0x000EC5AB
		// (set) Token: 0x0600341A RID: 13338 RVA: 0x000EE3B3 File Offset: 0x000EC5B3
		public Material scopeMaterial { get; private set; }

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x0600341B RID: 13339 RVA: 0x000EE3BC File Offset: 0x000EC5BC
		public bool isScopeActive
		{
			get
			{
				return this._isScopeActive;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600341C RID: 13340 RVA: 0x000EE3C4 File Offset: 0x000EC5C4
		public Transform aim
		{
			get
			{
				return this._aim;
			}
		}

		/// <summary>
		/// Unintuitively (to say the least), a pitch of 0 is up, 90 is forward, and 180 is down.
		/// </summary>
		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x0600341D RID: 13341 RVA: 0x000EE3CC File Offset: 0x000EC5CC
		public float pitch
		{
			get
			{
				return this._pitch;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x000EE3D4 File Offset: 0x000EC5D4
		public float yaw
		{
			get
			{
				return this._yaw;
			}
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x000EE3DC File Offset: 0x000EC5DC
		internal void TeleportYaw(float newYaw)
		{
			this._yaw = newYaw;
			this.clampYaw();
			base.transform.localRotation = Quaternion.Euler(0f, this._yaw, 0f);
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x000EE40B File Offset: 0x000EC60B
		public float look_x
		{
			get
			{
				return this._look_x;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x000EE413 File Offset: 0x000EC613
		public float look_y
		{
			get
			{
				return this._look_y;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06003422 RID: 13346 RVA: 0x000EE41B File Offset: 0x000EC61B
		public float orbitPitch
		{
			get
			{
				return this._orbitPitch;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06003423 RID: 13347 RVA: 0x000EE423 File Offset: 0x000EC623
		public float orbitYaw
		{
			get
			{
				return this._orbitYaw;
			}
		}

		/// <summary>
		/// Should player stats be visible in spectator mode?
		/// </summary>
		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06003424 RID: 13348 RVA: 0x000EE42B File Offset: 0x000EC62B
		// (set) Token: 0x06003425 RID: 13349 RVA: 0x000EE433 File Offset: 0x000EC633
		public bool areSpecStatsVisible { get; protected set; }

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x000EE43C File Offset: 0x000EC63C
		public bool IsLocallyUsingFreecam
		{
			get
			{
				return this.IsControllingFreecam || this.isTracking || this.isLocking || this.isFocusing;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06003427 RID: 13351 RVA: 0x000EE45E File Offset: 0x000EC65E
		public EPlayerPerspective perspective
		{
			get
			{
				return this._perspective;
			}
		}

		/// <summary>
		/// Get point-of-view in world-space.
		/// </summary>
		// Token: 0x06003428 RID: 13352 RVA: 0x000EE466 File Offset: 0x000EC666
		public Vector3 getEyesPosition()
		{
			return this.aim.position;
		}

		/// <summary>
		/// Get point of view in worldspace without the left/right leaning modifier.
		/// </summary>
		// Token: 0x06003429 RID: 13353 RVA: 0x000EE473 File Offset: 0x000EC673
		public Vector3 GetEyesPositionWithoutLeaning()
		{
			return base.transform.TransformPoint(this.aim.localPosition);
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000EE48C File Offset: 0x000EC68C
		public void updateScope(EGraphicQuality quality)
		{
			bool flag = false;
			int num = 0;
			switch (quality)
			{
			case EGraphicQuality.LOW:
				flag = true;
				num = 256;
				break;
			case EGraphicQuality.MEDIUM:
				flag = true;
				num = 512;
				break;
			case EGraphicQuality.HIGH:
				flag = true;
				num = 1024;
				break;
			case EGraphicQuality.ULTRA:
				flag = true;
				num = 2048;
				break;
			}
			if (flag)
			{
				if (this.scopeRenderTexture != null && this.scopeRenderTexture.width != num)
				{
					Object.Destroy(this.scopeRenderTexture);
					this.scopeRenderTexture = null;
				}
				if (this.scopeRenderTexture == null)
				{
					GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB;
					GraphicsFormat depthStencilFormat = GraphicsFormat.D24_UNorm_S8_UInt;
					this.scopeRenderTexture = new RenderTexture(num, num, colorFormat, depthStencilFormat);
					this.scopeRenderTexture.name = "Dual-Render Scope";
					this.scopeRenderTexture.hideFlags = HideFlags.HideAndDontSave;
				}
			}
			else if (this.scopeRenderTexture != null)
			{
				Object.Destroy(this.scopeRenderTexture);
				this.scopeRenderTexture = null;
			}
			this.scopeCamera.targetTexture = this.scopeRenderTexture;
			if (quality != EGraphicQuality.OFF)
			{
				if (this.scopeMaterial == null)
				{
					this.scopeMaterial = Object.Instantiate<Material>(Resources.Load<Material>("Materials/Scope"));
				}
				this.scopeMaterial.SetTexture("_MainTex", this.scopeCamera.targetTexture);
				this.scopeMaterial.SetTexture("_EmissionMap", this.scopeCamera.targetTexture);
			}
			this.scopeCamera.enabled = (this.isScopeActive && this.scopeCamera.targetTexture != null && this.scopeVision == ELightingVision.NONE);
			if (base.player.equipment.asset != null && base.player.equipment.asset.type == EItemType.GUN)
			{
				base.player.equipment.useable.updateState(base.player.equipment.state);
			}
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000EE660 File Offset: 0x000EC860
		public void enableScope(float zoom, ItemSightAsset sightAsset)
		{
			this.scopeCameraZoomFactor = zoom;
			this._isScopeActive = true;
			this.scopeVision = sightAsset.vision;
			this.scopeNightvisionColor = sightAsset.nightvisionColor;
			this.scopeNightvisionFogIntensity = sightAsset.nightvisionFogIntensity;
			this.scopeCamera.enabled = (this.scopeCamera.targetTexture != null && this.scopeVision == ELightingVision.NONE);
			this.scopeCamera.GetComponent<GrayscaleEffect>().blend = ((this.scopeVision == ELightingVision.CIVILIAN) ? 1f : 0f);
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x000EE6EE File Offset: 0x000EC8EE
		public void disableScope()
		{
			this.scopeCamera.enabled = false;
			this._isScopeActive = false;
			this.scopeVision = ELightingVision.NONE;
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x000EE70A File Offset: 0x000EC90A
		public void enableOverlay()
		{
			if (this.scopeVision == ELightingVision.NONE)
			{
				return;
			}
			if (this.scopeCamera.targetTexture != null)
			{
				return;
			}
			this.ApplyScopeVisionToLighting();
			this.isOverlayActive = true;
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x000EE736 File Offset: 0x000EC936
		[Obsolete("this was never supported server-side")]
		public void setPerspective(EPlayerPerspective newPerspective)
		{
			throw new NotSupportedException("this was never supported server-side");
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x000EE744 File Offset: 0x000EC944
		private void setActivePerspective(EPlayerPerspective newPerspective)
		{
			this._perspective = newPerspective;
			if (this.perspective == EPlayerPerspective.FIRST)
			{
				MainCamera.instance.transform.parent = base.player.first;
				MainCamera.instance.transform.localPosition = Vector3.up * this.eyes;
				this.IsControllingFreecam = false;
				this.isTracking = false;
				this.isLocking = false;
				this.isFocusing = false;
				base.player.ClientSetAdminUsageFlagActive(EPlayerAdminUsageFlags.Freecam, false);
				if (PlayerWorkzoneUI.active)
				{
					PlayerWorkzoneUI.close();
					PlayerLifeUI.open();
				}
			}
			else
			{
				MainCamera.instance.transform.parent = base.player.transform;
			}
			PerspectiveUpdated perspectiveUpdated = this.onPerspectiveUpdated;
			if (perspectiveUpdated != null)
			{
				perspectiveUpdated(this.perspective);
			}
			UnturnedPostProcess.instance.notifyPerspectiveChanged();
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x000EE810 File Offset: 0x000ECA10
		private void ApplyScopeVisionToLighting()
		{
			this.tempVision = LevelLighting.vision;
			this.tempNightvisionColor = LevelLighting.nightvisionColor;
			this.tempNightvisionFogIntensity = LevelLighting.nightvisionFogIntensity;
			LevelLighting.vision = this.scopeVision;
			LevelLighting.nightvisionColor = this.scopeNightvisionColor;
			LevelLighting.nightvisionFogIntensity = this.scopeNightvisionFogIntensity;
			LevelLighting.updateLighting();
			LevelLighting.updateLocal();
			PlayerLifeUI.updateGrayscale();
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x000EE86E File Offset: 0x000ECA6E
		public void disableOverlay()
		{
			if (!this.isOverlayActive)
			{
				return;
			}
			this.isOverlayActive = false;
			base.player.equipment.updateVision();
		}

		/// <summary>
		/// This is only used after capturing dual-render scope, not when exiting scope overlay.
		/// Otherwise the lighting vision may have changed between entering and exiting the scope.
		/// </summary>
		// Token: 0x06003432 RID: 13362 RVA: 0x000EE890 File Offset: 0x000ECA90
		private void RestoreSavedLightingVision()
		{
			LevelLighting.vision = this.tempVision;
			LevelLighting.nightvisionColor = this.tempNightvisionColor;
			LevelLighting.nightvisionFogIntensity = this.tempNightvisionFogIntensity;
			LevelLighting.updateLighting();
			LevelLighting.updateLocal();
			PlayerLifeUI.updateGrayscale();
			this.tempVision = ELightingVision.NONE;
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x000EE8C9 File Offset: 0x000ECAC9
		public void enableZoom(float zoom)
		{
			this.mainCameraZoomFactor = zoom;
			this.isZoomed = true;
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x000EE8D9 File Offset: 0x000ECAD9
		public void disableZoom()
		{
			this.mainCameraZoomFactor = 0f;
			this.isZoomed = false;
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x000EE8F0 File Offset: 0x000ECAF0
		public void updateRot()
		{
			if (this.pitch < 0f)
			{
				this.angle = 0;
			}
			else if (this.pitch > 180f)
			{
				this.angle = 180;
			}
			else
			{
				this.angle = (byte)this.pitch;
			}
			this.rot = MeasurementTool.angleToByte(this.yaw);
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x000EE94C File Offset: 0x000ECB4C
		public void updateLook()
		{
			this._pitch = 90f;
			this._yaw = base.transform.localRotation.eulerAngles.y;
			this.updateRot();
			if (base.channel.IsLocalPlayer && this.perspective == EPlayerPerspective.FIRST)
			{
				MainCamera.instance.transform.localRotation = Quaternion.Euler(this.pitch - 90f, 0f, 0f);
				MainCamera.instance.transform.localPosition = Vector3.up * this.eyes;
			}
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x000EE9E6 File Offset: 0x000ECBE6
		public void recoil(float x, float y, float h, float v)
		{
			this._yaw += x;
			this._pitch -= y;
			this.recoil_x += x * h;
			this.recoil_y += y * v;
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000EEA28 File Offset: 0x000ECC28
		public void simulate(float look_x, float look_y, float delta)
		{
			this._pitch = look_y;
			this._yaw = look_x;
			this.clampPitch();
			this.clampYaw();
			this.updateRot();
			if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
			{
				base.transform.localRotation = Quaternion.identity;
			}
			else
			{
				base.transform.localRotation = Quaternion.Euler(0f, this.yaw, 0f);
			}
			if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null)
			{
				Passenger passenger = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()];
				if (passenger.turretYaw != null)
				{
					passenger.turretYaw.localRotation = passenger.rotationYaw * Quaternion.Euler(0f, this.yaw, 0f);
				}
				if (passenger.turretPitch != null)
				{
					passenger.turretPitch.localRotation = passenger.rotationPitch * Quaternion.Euler(this.pitch - 90f, 0f, 0f);
				}
			}
			this.updateAim(delta);
		}

		/// <summary>
		/// Clamp _pitch within the [0, 180] range.
		/// </summary>
		// Token: 0x06003439 RID: 13369 RVA: 0x000EEBA0 File Offset: 0x000ECDA0
		private void clampPitch()
		{
			Passenger vehicleSeat = base.player.movement.getVehicleSeat();
			float min;
			float max;
			if (vehicleSeat != null)
			{
				if (vehicleSeat.turret != null)
				{
					min = vehicleSeat.turret.pitchMin;
					max = vehicleSeat.turret.pitchMax;
				}
				else
				{
					min = PlayerLook.MIN_ANGLE_SIT;
					max = PlayerLook.MAX_ANGLE_SIT;
				}
			}
			else if (base.player.stance.stance == EPlayerStance.STAND || base.player.stance.stance == EPlayerStance.SPRINT)
			{
				min = PlayerLook.MIN_ANGLE_STAND;
				max = PlayerLook.MAX_ANGLE_STAND;
			}
			else if (base.player.stance.stance == EPlayerStance.CLIMB)
			{
				min = PlayerLook.MIN_ANGLE_CLIMB;
				max = PlayerLook.MAX_ANGLE_CLIMB;
			}
			else if (base.player.stance.stance == EPlayerStance.SWIM)
			{
				min = PlayerLook.MIN_ANGLE_SWIM;
				max = PlayerLook.MAX_ANGLE_SWIM;
			}
			else if (base.player.stance.stance == EPlayerStance.CROUCH)
			{
				min = PlayerLook.MIN_ANGLE_CROUCH;
				max = PlayerLook.MAX_ANGLE_CROUCH;
			}
			else if (base.player.stance.stance == EPlayerStance.PRONE)
			{
				min = PlayerLook.MIN_ANGLE_PRONE;
				max = PlayerLook.MAX_ANGLE_PRONE;
			}
			else
			{
				min = 0f;
				max = 180f;
			}
			this._pitch = Mathf.Clamp(this._pitch, min, max);
		}

		/// <summary>
		/// Clamp yaw while seated, and keep within the [-360, 360] range.
		/// </summary>
		// Token: 0x0600343A RID: 13370 RVA: 0x000EECD0 File Offset: 0x000ECED0
		private void clampYaw()
		{
			this._yaw %= 360f;
			Passenger vehicleSeat = base.player.movement.getVehicleSeat();
			if (vehicleSeat == null)
			{
				return;
			}
			float min;
			float max;
			if (vehicleSeat.turret != null)
			{
				min = vehicleSeat.turret.yawMin;
				max = vehicleSeat.turret.yawMax;
			}
			else if (base.player.stance.stance == EPlayerStance.DRIVING)
			{
				min = -160f;
				max = 160f;
			}
			else
			{
				min = -90f;
				max = 90f;
			}
			this._yaw = Mathf.Clamp(this._yaw, min, max);
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x000EED68 File Offset: 0x000ECF68
		public void updateAim(float delta)
		{
			if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret.useAimCamera)
			{
				Passenger passenger = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()];
				if (passenger.turretAim != null)
				{
					this.aim.position = passenger.turretAim.position;
					this.aim.rotation = passenger.turretAim.rotation;
					return;
				}
			}
			else
			{
				this.aim.localPosition = Vector3.Lerp(this.aim.localPosition, Vector3.up * this.heightLook, 4f * delta);
				if (base.player.stance.stance == EPlayerStance.SITTING || base.player.stance.stance == EPlayerStance.DRIVING)
				{
					this.aim.parent.localRotation = Quaternion.Euler(0f, this.yaw, 0f);
				}
				else if (base.player.animator.leanObstructed)
				{
					this.aim.parent.localRotation = Quaternion.identity;
				}
				else
				{
					this.aim.parent.localRotation = Quaternion.Lerp(this.aim.parent.localRotation, Quaternion.Euler(0f, 0f, (float)base.player.animator.lean * HumanAnimator.LEAN), 4f * delta);
				}
				this.aim.localRotation = Quaternion.Euler(this.pitch - 90f + base.player.animator.scopeSway.x, base.player.animator.scopeSway.y, 0f);
			}
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x000EEFA0 File Offset: 0x000ED1A0
		internal void FlinchFromDamage(byte damageAmount, Vector3 worldDirection)
		{
			Camera instance = MainCamera.instance;
			if (instance == null)
			{
				return;
			}
			Vector3 normalized = Vector3.Cross(Vector3.up, worldDirection).normalized;
			Vector3 axis = instance.transform.InverseTransformDirection(normalized);
			float num = (float)Mathf.Min((int)damageAmount, 25) * 0.5f;
			float num2 = 1f - base.player.skills.mastery(1, 3) * 0.75f;
			this.flinchLocalRotation *= Quaternion.AngleAxis(num * num2, axis);
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x000EF02C File Offset: 0x000ED22C
		internal void FlinchFromExplosion(Vector3 position, float radius, float magnitudeDegrees)
		{
			Camera instance = MainCamera.instance;
			if (instance == null)
			{
				return;
			}
			Vector3 a = instance.transform.position - position;
			float magnitude = a.magnitude;
			if (magnitude <= 0f || magnitude >= radius)
			{
				return;
			}
			Vector3 vector = a / magnitude;
			Vector3 normalized = Vector3.Cross(Vector3.up, vector).normalized;
			Vector3 axis = instance.transform.InverseTransformDirection(normalized);
			float num = 1f - base.player.skills.mastery(1, 3) * 0.5f;
			float num2 = 1f - MathfEx.Square(magnitude / radius);
			magnitudeDegrees *= num * num2;
			this.targetExplosionLocalRotation.currentRotation = this.targetExplosionLocalRotation.currentRotation * Quaternion.AngleAxis(magnitudeDegrees, axis);
			base.player.animator.FlinchFromExplosion(vector, magnitudeDegrees);
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x000EF10C File Offset: 0x000ED30C
		private void onVisionUpdated(bool isViewing)
		{
			if (isViewing)
			{
				this.yawInputMultiplier = (((double)Random.value < 0.25) ? -1f : 1f);
				this.pitchInputMultiplier = (((double)Random.value < 0.25) ? -1f : 1f);
				return;
			}
			this.yawInputMultiplier = 1f;
			this.pitchInputMultiplier = 1f;
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x000EF17C File Offset: 0x000ED37C
		private void onLifeUpdated(bool isDead)
		{
			if (isDead)
			{
				PlayerLook.killcam = base.transform.rotation.eulerAngles.y;
			}
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000EF1AC File Offset: 0x000ED3AC
		private EVehicleThirdPersonCameraMode GetVehicleThirdPersonCameraMode(InteractableVehicle vehicle)
		{
			if (vehicle != null && vehicle.asset != null)
			{
				EEngine engine = vehicle.asset.engine;
				if (engine - EEngine.PLANE <= 2)
				{
					return OptionsSettings.vehicleAircraftThirdPersonCameraMode;
				}
			}
			return OptionsSettings.vehicleThirdPersonCameraMode;
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x000EF1E8 File Offset: 0x000ED3E8
		private EVehicleThirdPersonCameraMode GetCurrentVehicleThirdPersonCameraMode()
		{
			InteractableVehicle vehicle = base.player.movement.getVehicle();
			return this.GetVehicleThirdPersonCameraMode(vehicle);
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x000EF210 File Offset: 0x000ED410
		private void onSeated(bool isDriver, bool inVehicle, bool wasVehicle, InteractableVehicle oldVehicle, InteractableVehicle newVehicle)
		{
			if (!wasVehicle)
			{
				this._orbitPitch = 22.5f;
				if (this.GetVehicleThirdPersonCameraMode(newVehicle) == EVehicleThirdPersonCameraMode.RotationDetached)
				{
					this._orbitYaw = ((newVehicle != null) ? newVehicle.transform.rotation.eulerAngles.y : 0f);
				}
				else
				{
					this._orbitYaw = 0f;
				}
			}
			if (Provider.cameraMode == ECameraMode.VEHICLE && this.perspective == EPlayerPerspective.THIRD && !isDriver)
			{
				this.setActivePerspective(EPlayerPerspective.FIRST);
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06003443 RID: 13379 RVA: 0x000EF287 File Offset: 0x000ED487
		public bool canUseFreecam
		{
			get
			{
				return this.allowFreecamWithoutAdmin || base.channel.owner.isAdmin;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06003444 RID: 13380 RVA: 0x000EF2A3 File Offset: 0x000ED4A3
		public bool canUseWorkzone
		{
			get
			{
				return this.allowWorkzoneWithoutAdmin || base.channel.owner.isAdmin;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06003445 RID: 13381 RVA: 0x000EF2BF File Offset: 0x000ED4BF
		public bool canUseSpecStats
		{
			get
			{
				return this.allowSpecStatsWithoutAdmin || base.channel.owner.isAdmin;
			}
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x000EF2DB File Offset: 0x000ED4DB
		[Obsolete]
		public void tellFreecamAllowed(CSteamID senderId, bool isAllowed)
		{
			this.ReceiveFreecamAllowed(isAllowed);
		}

		/// <summary>
		/// Called from the server to allow spectating without admin powers.
		/// Only used by plugins.
		/// </summary>
		// Token: 0x06003447 RID: 13383 RVA: 0x000EF2E4 File Offset: 0x000ED4E4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellFreecamAllowed")]
		public void ReceiveFreecamAllowed(bool isAllowed)
		{
			this.allowFreecamWithoutAdmin = isAllowed;
			if (!this.canUseFreecam && this.IsLocallyUsingFreecam)
			{
				this.IsControllingFreecam = false;
				this.isTracking = false;
				this.isLocking = false;
				this.isFocusing = false;
				base.player.ClientSetAdminUsageFlagActive(EPlayerAdminUsageFlags.Freecam, false);
			}
		}

		/// <summary>
		/// Allow use of spectator mode without admin powers.
		/// Only used by plugins.
		/// </summary>
		// Token: 0x06003448 RID: 13384 RVA: 0x000EF331 File Offset: 0x000ED531
		public void sendFreecamAllowed(bool isAllowed)
		{
			this.allowFreecamWithoutAdmin = isAllowed;
			PlayerLook.SendFreecamAllowed.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), isAllowed);
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x000EF357 File Offset: 0x000ED557
		[Obsolete]
		public void tellWorkzoneAllowed(CSteamID senderId, bool isAllowed)
		{
			this.ReceiveWorkzoneAllowed(isAllowed);
		}

		/// <summary>
		/// Called from the server to allow workzone without admin powers.
		/// Only used by plugins.
		/// </summary>
		// Token: 0x0600344A RID: 13386 RVA: 0x000EF360 File Offset: 0x000ED560
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellWorkzoneAllowed")]
		public void ReceiveWorkzoneAllowed(bool isAllowed)
		{
			this.allowWorkzoneWithoutAdmin = isAllowed;
			if (!this.canUseWorkzone && PlayerWorkzoneUI.active)
			{
				PlayerWorkzoneUI.close();
				PlayerLifeUI.open();
			}
		}

		/// <summary>
		/// Allow use of workzone mode without admin powers.
		/// Only used by plugins.
		/// </summary>
		// Token: 0x0600344B RID: 13387 RVA: 0x000EF382 File Offset: 0x000ED582
		public void sendWorkzoneAllowed(bool isAllowed)
		{
			this.allowWorkzoneWithoutAdmin = isAllowed;
			PlayerLook.SendWorkzoneAllowed.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), isAllowed);
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x000EF3A8 File Offset: 0x000ED5A8
		[Obsolete]
		public void tellSpecStatsAllowed(CSteamID senderId, bool isAllowed)
		{
			this.ReceiveSpecStatsAllowed(isAllowed);
		}

		/// <summary>
		/// Called from the server to allow spectator overlays without admin powers.
		/// Only used by plugins.
		/// </summary>
		// Token: 0x0600344D RID: 13389 RVA: 0x000EF3B1 File Offset: 0x000ED5B1
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellSpecStatsAllowed")]
		public void ReceiveSpecStatsAllowed(bool isAllowed)
		{
			this.allowSpecStatsWithoutAdmin = isAllowed;
			if (!this.canUseSpecStats)
			{
				this.areSpecStatsVisible = false;
				base.player.ClientSetAdminUsageFlagActive(EPlayerAdminUsageFlags.SpectatorStatsOverlay, false);
			}
		}

		/// <summary>
		/// Allow use of spectator overlay mode without admin powers.
		/// Only used by plugins.
		/// </summary>
		// Token: 0x0600344E RID: 13390 RVA: 0x000EF3D6 File Offset: 0x000ED5D6
		public void sendSpecStatsAllowed(bool isAllowed)
		{
			this.allowSpecStatsWithoutAdmin = isAllowed;
			PlayerLook.SendSpecStatsAllowed.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), isAllowed);
		}

		/// <summary>
		/// Sweep a sphere to find collisions blocking the third-person camera.
		/// </summary>
		/// <returns>Valid world-space camera position.</returns>
		// Token: 0x0600344F RID: 13391 RVA: 0x000EF3FC File Offset: 0x000ED5FC
		private Vector3 sphereCastCamera(Vector3 origin, Vector3 direction, float length, int layerMask)
		{
			int num = Physics.SphereCastNonAlloc(new Ray(origin, direction), 0.39f, PlayerLook.sweepHits, length, layerMask, QueryTriggerInteraction.Ignore);
			float num2 = length;
			for (int i = 0; i < num; i++)
			{
				num2 = Mathf.Min(num2, PlayerLook.sweepHits[i].distance);
			}
			return origin + direction * num2;
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x000EF458 File Offset: 0x000ED658
		private void Update()
		{
			if (base.channel.IsLocalPlayer)
			{
				if (InputEx.GetKey(KeyCode.LeftShift))
				{
					if (this.canUseFreecam)
					{
						if (InputEx.GetKeyDown(KeyCode.F1))
						{
							this.IsControllingFreecam = !this.IsControllingFreecam;
							if (this.IsControllingFreecam && !this.isTracking && !this.isLocking && !this.isFocusing)
							{
								this.isTracking = true;
							}
							base.player.ClientSetAdminUsageFlagActive(EPlayerAdminUsageFlags.Freecam, this.IsLocallyUsingFreecam);
						}
						if (InputEx.GetKeyDown(KeyCode.F2))
						{
							this.isTracking = !this.isTracking;
							if (this.isTracking)
							{
								this.isLocking = false;
								this.isFocusing = false;
							}
							base.player.ClientSetAdminUsageFlagActive(EPlayerAdminUsageFlags.Freecam, this.IsLocallyUsingFreecam);
						}
						if (InputEx.GetKeyDown(KeyCode.F3))
						{
							this.isLocking = !this.isLocking;
							if (this.isLocking)
							{
								this.isTracking = false;
								this.isFocusing = false;
								this.lockPosition = base.player.first.position;
							}
							base.player.ClientSetAdminUsageFlagActive(EPlayerAdminUsageFlags.Freecam, this.IsLocallyUsingFreecam);
						}
						if (InputEx.GetKeyDown(KeyCode.F4))
						{
							this.isFocusing = !this.isFocusing;
							if (this.isFocusing)
							{
								this.isTracking = false;
								this.isLocking = false;
								this.lockPosition = base.player.first.position;
							}
							base.player.ClientSetAdminUsageFlagActive(EPlayerAdminUsageFlags.Freecam, this.IsLocallyUsingFreecam);
						}
						if (InputEx.GetKeyDown(KeyCode.F5))
						{
							this.isSmoothing = !this.isSmoothing;
						}
					}
					if (InputEx.GetKeyDown(KeyCode.F6))
					{
						if (PlayerWorkzoneUI.active)
						{
							PlayerWorkzoneUI.close();
							PlayerLifeUI.open();
						}
						else if (this.canUseWorkzone && this.perspective == EPlayerPerspective.THIRD)
						{
							PlayerWorkzoneUI.open();
							PlayerLifeUI.close();
						}
					}
					if (InputEx.GetKeyDown(KeyCode.F7))
					{
						if (this.areSpecStatsVisible)
						{
							this.areSpecStatsVisible = false;
						}
						else if (this.canUseSpecStats)
						{
							this.areSpecStatsVisible = true;
						}
						base.player.ClientSetAdminUsageFlagActive(EPlayerAdminUsageFlags.SpectatorStatsOverlay, this.areSpecStatsVisible);
					}
				}
				float heightLook = this.heightLook;
				this.eyes = Mathf.Lerp(this.eyes, heightLook, 4f * Time.deltaTime);
				if (base.player.movement.controller != null)
				{
					float min = 0.39499998f;
					float max = base.player.movement.controller.height - 0.39f - 0.005f;
					this.thirdPersonEyeHeight = Mathf.Lerp(this.thirdPersonEyeHeight, Mathf.Clamp(heightLook, min, max), 4f * Time.deltaTime);
				}
				Camera instance = MainCamera.instance;
				if (base.player.life.IsAlive && !PlayerUI.window.showCursor)
				{
					if (InputEx.GetKeyDown(ControlsSettings.perspective) && (Provider.cameraMode == ECameraMode.BOTH || (Provider.cameraMode == ECameraMode.VEHICLE && base.player.stance.stance == EPlayerStance.DRIVING)))
					{
						EPlayerPerspective activePerspective;
						if (this.perspective == EPlayerPerspective.FIRST)
						{
							activePerspective = EPlayerPerspective.THIRD;
						}
						else
						{
							activePerspective = EPlayerPerspective.FIRST;
						}
						this.setActivePerspective(activePerspective);
					}
					if (this.IsLocallyUsingFreecam)
					{
						if (this.perspective != EPlayerPerspective.THIRD)
						{
							this.setActivePerspective(EPlayerPerspective.THIRD);
						}
					}
					else if ((Provider.cameraMode == ECameraMode.FIRST || (Provider.cameraMode == ECameraMode.VEHICLE && base.player.stance.stance != EPlayerStance.DRIVING)) && this.perspective != EPlayerPerspective.FIRST)
					{
						this.setActivePerspective(EPlayerPerspective.FIRST);
					}
				}
				float zoomBaseFieldOfView = OptionsSettings.GetZoomBaseFieldOfView();
				if (this.IsLocallyUsingFreecam)
				{
					if (this.freecamVerticalFieldOfView < 0.1f)
					{
						this.freecamVerticalFieldOfView = OptionsSettings.DesiredVerticalFieldOfView;
					}
					if (this.isSmoothing)
					{
						instance.fieldOfView = Mathf.Lerp(instance.fieldOfView, this.freecamVerticalFieldOfView, 4f * Time.deltaTime);
					}
					else
					{
						instance.fieldOfView = this.freecamVerticalFieldOfView;
					}
				}
				else
				{
					instance.fieldOfView = Mathf.Lerp(instance.fieldOfView, (this.mainCameraZoomFactor > 0f) ? (zoomBaseFieldOfView / this.mainCameraZoomFactor) : (OptionsSettings.DesiredVerticalFieldOfView + (float)((base.player.stance.stance == EPlayerStance.SPRINT) ? 10 : 0)), 8f * Time.deltaTime);
				}
				if (this.isScopeActive && this.scopeCamera != null && this.scopeCameraZoomFactor > 0f)
				{
					this.scopeCamera.fieldOfView = zoomBaseFieldOfView / this.scopeCameraZoomFactor;
				}
				this._look_x = 0f;
				this._look_y = 0f;
				if (PlayerUI.window.isCursorLocked && !this.isIgnoringInput)
				{
					if (this.IsControllingFreecam)
					{
						if (!base.player.workzone.isBuilding || InputEx.GetKey(ControlsSettings.secondary))
						{
							float num = 1f;
							ESensitivityScalingMode sensitivityScalingMode = ControlsSettings.sensitivityScalingMode;
							if (sensitivityScalingMode != ESensitivityScalingMode.ProjectionRatio)
							{
								if (sensitivityScalingMode - ESensitivityScalingMode.ZoomFactor <= 1)
								{
									float num2 = OptionsSettings.DesiredVerticalFieldOfView / instance.fieldOfView;
									if (num2 > 0f)
									{
										num = 1f / num2;
									}
								}
							}
							else
							{
								float f = 0.017453292f * instance.fieldOfView * 0.5f;
								float f2 = 0.017453292f * OptionsSettings.DesiredVerticalFieldOfView * 0.5f;
								float projectionRatioCoefficient = ControlsSettings.projectionRatioCoefficient;
								num = Mathf.Atan(projectionRatioCoefficient * Mathf.Tan(f)) / Mathf.Atan(projectionRatioCoefficient * Mathf.Tan(f2));
							}
							this._orbitYaw += ControlsSettings.mouseAimSensitivity * num * Input.GetAxis("mouse_x") * this.yawInputMultiplier;
							if (ControlsSettings.invert)
							{
								this._orbitPitch += ControlsSettings.mouseAimSensitivity * num * Input.GetAxis("mouse_y") * this.pitchInputMultiplier;
							}
							else
							{
								this._orbitPitch -= ControlsSettings.mouseAimSensitivity * num * Input.GetAxis("mouse_y") * this.pitchInputMultiplier;
							}
						}
					}
					else
					{
						if (this.perspective == EPlayerPerspective.FIRST || this.isTracking || this.isLocking || this.isFocusing)
						{
							this._look_x = ControlsSettings.mouseAimSensitivity * Input.GetAxis("mouse_x") * this.yawInputMultiplier;
							this._look_y = ControlsSettings.mouseAimSensitivity * -Input.GetAxis("mouse_y") * this.pitchInputMultiplier;
						}
						if (InputEx.GetKey(ControlsSettings.rollLeft))
						{
							this._look_x = ((base.player.movement.getVehicle() != null) ? (-base.player.movement.getVehicle().asset.airTurnResponsiveness) : -1f);
						}
						else if (InputEx.GetKey(ControlsSettings.rollRight))
						{
							this._look_x = ((base.player.movement.getVehicle() != null) ? base.player.movement.getVehicle().asset.airTurnResponsiveness : 1f);
						}
						if (InputEx.GetKey(ControlsSettings.pitchUp))
						{
							this._look_y = ((base.player.movement.getVehicle() != null) ? (-base.player.movement.getVehicle().asset.airTurnResponsiveness) : -1f);
						}
						else if (InputEx.GetKey(ControlsSettings.pitchDown))
						{
							this._look_y = ((base.player.movement.getVehicle() != null) ? base.player.movement.getVehicle().asset.airTurnResponsiveness : 1f);
						}
						if (ControlsSettings.invertFlight)
						{
							this._look_y *= -1f;
						}
						float num3 = 1f;
						ESensitivityScalingMode sensitivityScalingMode = ControlsSettings.sensitivityScalingMode;
						if (sensitivityScalingMode != ESensitivityScalingMode.ProjectionRatio)
						{
							if (sensitivityScalingMode - ESensitivityScalingMode.ZoomFactor <= 1)
							{
								if (this.shouldUseZoomFactorForSensitivity)
								{
									if (this.isScopeActive && this.perspective == EPlayerPerspective.FIRST && this.scopeCameraZoomFactor > 0f)
									{
										num3 = 1f / this.scopeCameraZoomFactor;
									}
									else if (this.mainCameraZoomFactor > 0f)
									{
										num3 = 1f / this.mainCameraZoomFactor;
									}
								}
							}
						}
						else
						{
							float num4 = (this.shouldUseZoomFactorForSensitivity && this.isScopeActive && this.perspective == EPlayerPerspective.FIRST && this.scopeCameraZoomFactor > 0f) ? this.scopeCamera.fieldOfView : instance.fieldOfView;
							float f3 = 0.017453292f * num4 * 0.5f;
							float f4 = 0.017453292f * OptionsSettings.DesiredVerticalFieldOfView * 0.5f;
							float projectionRatioCoefficient2 = ControlsSettings.projectionRatioCoefficient;
							num3 = Mathf.Atan(projectionRatioCoefficient2 * Mathf.Tan(f3)) / Mathf.Atan(projectionRatioCoefficient2 * Mathf.Tan(f4));
						}
						if (base.player.movement.getVehicle() != null && this.perspective == EPlayerPerspective.THIRD)
						{
							this._orbitYaw += ControlsSettings.mouseAimSensitivity * Input.GetAxis("mouse_x") * this.yawInputMultiplier;
							this._orbitYaw = this.orbitYaw % 360f;
						}
						else if (base.player.movement.getVehicle() == null || !base.player.movement.getVehicle().asset.hasLockMouse || !base.player.movement.getVehicle().isDriver)
						{
							this._yaw += ControlsSettings.mouseAimSensitivity * num3 * Input.GetAxis("mouse_x") * this.yawInputMultiplier;
						}
						if (base.player.movement.getVehicle() != null && this.perspective == EPlayerPerspective.THIRD)
						{
							if (ControlsSettings.invert)
							{
								this._orbitPitch += ControlsSettings.mouseAimSensitivity * Input.GetAxis("mouse_y") * this.pitchInputMultiplier;
							}
							else
							{
								this._orbitPitch -= ControlsSettings.mouseAimSensitivity * Input.GetAxis("mouse_y") * this.pitchInputMultiplier;
							}
						}
						else if (base.player.movement.getVehicle() == null || !base.player.movement.getVehicle().asset.hasLockMouse || !base.player.movement.getVehicle().isDriver)
						{
							if (ControlsSettings.invert)
							{
								this._pitch += ControlsSettings.mouseAimSensitivity * num3 * Input.GetAxis("mouse_y") * this.pitchInputMultiplier;
							}
							else
							{
								this._pitch -= ControlsSettings.mouseAimSensitivity * num3 * Input.GetAxis("mouse_y") * this.pitchInputMultiplier;
							}
						}
					}
				}
				if (float.IsInfinity(this.yaw) || float.IsNaN(this.yaw))
				{
					this._yaw = 0f;
				}
				if (float.IsInfinity(this.pitch) || float.IsNaN(this.pitch))
				{
					this._pitch = 90f;
				}
				if (float.IsInfinity(this.orbitYaw) || float.IsNaN(this.orbitYaw))
				{
					this._orbitYaw = 0f;
				}
				if (float.IsInfinity(this.orbitPitch) || float.IsNaN(this.orbitPitch))
				{
					this._orbitPitch = 0f;
				}
				float num5 = Mathf.Lerp(this.recoil_x, 0f, 4f * Time.deltaTime);
				float num6 = num5 - this.recoil_x;
				this.recoil_x = num5;
				float num7 = Mathf.Lerp(this.recoil_y, 0f, 4f * Time.deltaTime);
				float num8 = num7 - this.recoil_y;
				this.recoil_y = num7;
				this._yaw += num6;
				this._pitch -= num8;
				this.flinchLocalRotation = Quaternion.Lerp(this.flinchLocalRotation, Quaternion.identity, 4f * Time.deltaTime);
				this.smoothedExplosionLocalRotation = Quaternion.Lerp(this.smoothedExplosionLocalRotation, this.targetExplosionLocalRotation.currentRotation, this.explosionSmoothingSpeed * Time.deltaTime);
				this.targetExplosionLocalRotation.Update(Time.deltaTime);
				this.clampPitch();
				this.clampYaw();
				if (this.orbitPitch > 90f)
				{
					this._orbitPitch = 90f;
				}
				else if (this.orbitPitch < -90f)
				{
					this._orbitPitch = -90f;
				}
				PlayerLook._characterYaw = Mathf.Lerp(PlayerLook._characterYaw, PlayerLook.characterYaw + 180f, 4f * Time.deltaTime);
				this.characterCamera.transform.rotation = Quaternion.Euler(20f, PlayerLook._characterYaw, 0f);
				this.characterCamera.transform.position = base.player.character.position - this.characterCamera.transform.forward * 3.5f + Vector3.up * PlayerLook.characterHeight;
				if (base.player.life.isDead)
				{
					PlayerLook.killcam += -16f * Time.deltaTime;
					instance.transform.rotation = Quaternion.Lerp(instance.transform.rotation, Quaternion.Euler(32f, PlayerLook.killcam, 0f), 2f * Time.deltaTime);
				}
				else
				{
					if ((base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING) && this.perspective == EPlayerPerspective.THIRD)
					{
						if (this.GetCurrentVehicleThirdPersonCameraMode() == EVehicleThirdPersonCameraMode.RotationDetached)
						{
							instance.transform.rotation = Quaternion.Euler(this.orbitPitch, this.orbitYaw, 0f);
						}
						else
						{
							instance.transform.localRotation = Quaternion.Euler(this.orbitPitch, this.orbitYaw, 0f);
						}
					}
					else if (base.player.stance.stance == EPlayerStance.DRIVING)
					{
						instance.transform.localRotation = Quaternion.Euler(this.pitch - 90f, this.yaw / 10f, 0f);
						instance.transform.Rotate(base.transform.up, this.yaw, Space.World);
					}
					else if (base.player.stance.stance == EPlayerStance.SITTING)
					{
						instance.transform.localRotation = Quaternion.Euler(this.pitch - 90f + base.player.animator.scopeSway.x, base.player.animator.scopeSway.y, 0f);
						instance.transform.Rotate(base.transform.up, this.yaw, Space.World);
					}
					else
					{
						if (this.perspective == EPlayerPerspective.FIRST)
						{
							instance.transform.localRotation = this.smoothedExplosionLocalRotation * this.flinchLocalRotation * Quaternion.Euler(this.pitch - 90f + base.player.animator.scopeSway.x, base.player.animator.scopeSway.y, 0f);
						}
						else
						{
							instance.transform.localRotation = this.smoothedExplosionLocalRotation * this.flinchLocalRotation * Quaternion.Euler(this.pitch - 90f + base.player.animator.scopeSway.x, base.player.animator.shoulder * -5f + base.player.animator.scopeSway.y, 0f);
						}
						base.transform.localRotation = Quaternion.Euler(0f, this.yaw, 0f);
					}
					if (this.IsLocallyUsingFreecam)
					{
						if (this.isFocusing)
						{
							Vector3 a = base.player.first.position + Vector3.up;
							Vector3 b = this.lockPosition + this.orbitPosition;
							Quaternion quaternion = Quaternion.LookRotation((a - b).normalized);
							if (this.isSmoothing)
							{
								this.smoothRotation = Quaternion.Lerp(this.smoothRotation, quaternion, 4f * Time.deltaTime);
								instance.transform.rotation = this.smoothRotation;
							}
							else
							{
								instance.transform.rotation = quaternion;
							}
						}
						else if (this.isSmoothing)
						{
							this.smoothRotation = Quaternion.Lerp(this.smoothRotation, Quaternion.Euler(this.orbitPitch, this.orbitYaw, 0f), 4f * Time.deltaTime);
							instance.transform.rotation = this.smoothRotation;
						}
						else
						{
							instance.transform.rotation = Quaternion.Euler(this.orbitPitch, this.orbitYaw, 0f);
						}
					}
				}
				if (base.player.life.isDead)
				{
					Vector3 origin = base.player.first.position + Vector3.up;
					Vector3 direction = -instance.transform.forward;
					float length = 4f;
					instance.transform.position = this.sphereCastCamera(origin, direction, length, RayMasks.BLOCK_KILLCAM);
				}
				else
				{
					if (this.IsLocallyUsingFreecam)
					{
						if (this.isLocking || this.isFocusing)
						{
							instance.transform.position = this.lockPosition + this.orbitPosition;
						}
						else if (this.IsControllingFreecam || this.isTracking)
						{
							if (this.isSmoothing)
							{
								this.smoothPosition = Vector3.Lerp(this.smoothPosition, this.orbitPosition, 4f * Time.deltaTime);
								instance.transform.position = base.player.first.position + this.smoothPosition;
							}
							else
							{
								instance.transform.position = base.player.first.position + this.orbitPosition;
							}
						}
					}
					else if ((base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING) && this.perspective == EPlayerPerspective.THIRD)
					{
						Vector3 origin2 = base.player.first.transform.position + Vector3.up * this.eyes;
						Transform transform = base.player.movement.getVehicle().transform.Find("Camera_Focus");
						if (transform != null)
						{
							origin2 = transform.position;
						}
						float length2 = base.player.movement.getVehicle().asset.camFollowDistance + Mathf.Abs(base.player.movement.getVehicle().AnimatedForwardVelocity) * 0.1f;
						Vector3 direction2 = -instance.transform.forward;
						instance.transform.position = this.sphereCastCamera(origin2, direction2, length2, RayMasks.BLOCK_VEHICLECAM);
					}
					else if (base.player.stance.stance == EPlayerStance.DRIVING)
					{
						float num9 = base.player.movement.getVehicle().asset.camDriverOffset + base.player.movement.getVehicle().asset.camPassengerOffset;
						if (this.yaw > 0f)
						{
							instance.transform.localPosition = Vector3.Lerp(instance.transform.localPosition, Vector3.up * (this.heightLook + num9) - Vector3.left * this.yaw / 360f, 4f * Time.deltaTime);
						}
						else
						{
							instance.transform.localPosition = Vector3.Lerp(instance.transform.localPosition, Vector3.up * (this.heightLook + num9) - Vector3.left * this.yaw / 240f, 4f * Time.deltaTime);
						}
					}
					else if (this.perspective == EPlayerPerspective.FIRST)
					{
						float num10;
						if (base.player.stance.stance == EPlayerStance.SITTING && base.player.movement.getVehicle() != null)
						{
							num10 = base.player.movement.getVehicle().asset.camPassengerOffset;
						}
						else
						{
							num10 = 0f;
							Vector3 origin3 = base.player.first.position + new Vector3(0f, PlayerLook.HEIGHT_LOOK_PRONE - 0.25f, 0f);
							Vector3 up = Vector3.up;
							float maxDistance = PlayerMovement.HEIGHT_STAND - PlayerLook.HEIGHT_LOOK_PRONE - 0.25f;
							RaycastHit raycastHit;
							if (Physics.SphereCast(origin3, 0.25f, up, out raycastHit, maxDistance, RayMasks.BLOCK_PLAYERCAM_1P, QueryTriggerInteraction.Ignore))
							{
								float b2 = raycastHit.point.y - base.player.first.position.y - 0.25f;
								this.eyes = Mathf.Min(this.eyes, b2);
							}
						}
						instance.transform.localPosition = new Vector3(0f, this.eyes + num10, 0f);
					}
					else
					{
						Vector3 direction3;
						if (Provider.modeConfigData.Gameplay.Allow_Shoulder_Camera)
						{
							direction3 = instance.transform.forward * -1.5f + instance.transform.up * 0.25f + instance.transform.right * base.player.animator.shoulder * 1f;
						}
						else
						{
							direction3 = instance.transform.forward * -1.5f + instance.transform.up * 0.5f + instance.transform.right * base.player.animator.shoulder2 * 0.5f;
						}
						direction3.Normalize();
						Vector3 origin4 = base.player.first.position + new Vector3(0f, this.thirdPersonEyeHeight, 0f);
						float length3 = 2f;
						instance.transform.position = this.sphereCastCamera(origin4, direction3, length3, RayMasks.BLOCK_PLAYERCAM);
					}
					PlayerLook.characterHeight = Mathf.Lerp(PlayerLook.characterHeight, this.heightCamera, 4f * Time.deltaTime);
				}
				if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().asset.engine == EEngine.PLANE && base.player.movement.getVehicle().AnimatedForwardVelocity > 16f)
				{
					LevelLighting.updateLocal(instance.transform.position, Mathf.Lerp(0f, 1f, (base.player.movement.getVehicle().AnimatedForwardVelocity - 16f) / 8f), base.player.movement.effectNode);
				}
				else if (base.player.movement.getVehicle() != null && (base.player.movement.getVehicle().asset.engine == EEngine.HELICOPTER || base.player.movement.getVehicle().asset.engine == EEngine.BLIMP) && base.player.movement.getVehicle().AnimatedForwardVelocity > 4f)
				{
					LevelLighting.updateLocal(instance.transform.position, Mathf.Lerp(0f, 1f, (base.player.movement.getVehicle().AnimatedForwardVelocity - 8f) / 8f), base.player.movement.effectNode);
				}
				else
				{
					LevelLighting.updateLocal(instance.transform.position, 0f, base.player.movement.effectNode);
				}
				base.player.animator.viewmodelParentTransform.rotation = instance.transform.rotation;
				if (this.isScopeActive && this.scopeCamera.targetTexture != null && this.scopeVision != ELightingVision.NONE)
				{
					this.ApplyScopeVisionToLighting();
					this.scopeCamera.Render();
					this.RestoreSavedLightingVision();
				}
				if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null)
				{
					Passenger passenger = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()];
					if (passenger.turretYaw != null)
					{
						passenger.turretYaw.localRotation = passenger.rotationYaw * Quaternion.Euler(0f, this.yaw, 0f);
					}
					if (passenger.turretPitch != null)
					{
						passenger.turretPitch.localRotation = passenger.rotationPitch * Quaternion.Euler(this.pitch - 90f, 0f, 0f);
					}
					if (this.perspective == EPlayerPerspective.FIRST && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret.useAimCamera)
					{
						instance.transform.position = passenger.turretAim.position;
						instance.transform.rotation = passenger.turretAim.rotation;
					}
				}
				if (FoliageSettings.drawFocus)
				{
					if (!this.isZoomed && (!this.isScopeActive || !(this.scopeCamera.targetTexture != null)))
					{
						FoliageSystem.isFocused = false;
						return;
					}
					FoliageSystem.isFocused = true;
					RaycastHit raycastHit2;
					if (Physics.Raycast(MainCamera.instance.transform.position, MainCamera.instance.transform.forward, out raycastHit2, FoliageSettings.focusDistance, RayMasks.FOLIAGE_FOCUS))
					{
						FoliageSystem.focusPosition = raycastHit2.point;
						if (this.isScopeActive && this.scopeCamera.targetTexture != null)
						{
							FoliageSystem.focusCamera = this.scopeCamera;
							return;
						}
						FoliageSystem.focusCamera = MainCamera.instance;
						return;
					}
				}
			}
			else if (!Provider.isServer)
			{
				if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
				{
					base.transform.localRotation = Quaternion.identity;
				}
				else
				{
					this._pitch = base.player.movement.snapshot.pitch;
					this._yaw = base.player.movement.snapshot.yaw;
					base.transform.localRotation = Quaternion.Euler(0f, this.yaw, 0f);
				}
				if (base.player.movement.getVehicle() != null && base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()].turret != null)
				{
					Passenger passenger2 = base.player.movement.getVehicle().passengers[(int)base.player.movement.getSeat()];
					if (passenger2.turretYaw != null)
					{
						passenger2.turretYaw.localRotation = passenger2.rotationYaw * Quaternion.Euler(0f, base.player.movement.snapshot.yaw, 0f);
					}
					if (passenger2.turretPitch != null)
					{
						passenger2.turretPitch.localRotation = passenger2.rotationPitch * Quaternion.Euler(base.player.movement.snapshot.pitch - 90f, 0f, 0f);
					}
				}
			}
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x000F100C File Offset: 0x000EF20C
		internal void InitializePlayer()
		{
			this._aim = base.transform.Find("Aim").Find("Fire");
			this.updateLook();
			this.yawInputMultiplier = 1f;
			this.pitchInputMultiplier = 1f;
			if (base.channel.IsLocalPlayer)
			{
				if (Provider.cameraMode == ECameraMode.THIRD)
				{
					this._perspective = EPlayerPerspective.THIRD;
					MainCamera.instance.transform.parent = base.player.transform;
				}
				else
				{
					this._perspective = EPlayerPerspective.FIRST;
				}
				MainCamera.instance.fieldOfView = OptionsSettings.DesiredVerticalFieldOfView;
				this.targetExplosionLocalRotation.currentRotation = Quaternion.identity;
				this.targetExplosionLocalRotation.targetRotation = Quaternion.identity;
				PlayerLook.characterHeight = 0f;
				PlayerLook._characterYaw = 180f;
				PlayerLook.characterYaw = 0f;
				if (base.player.character != null)
				{
					this._characterCamera = base.player.character.Find("Camera").GetComponent<Camera>();
					this._characterCamera.eventMask = 0;
				}
				this._scopeCamera = MainCamera.instance.transform.Find("Scope").GetComponent<Camera>();
				this.scopeCamera.layerCullDistances = MainCamera.instance.layerCullDistances;
				this.scopeCamera.layerCullSpherical = MainCamera.instance.layerCullSpherical;
				this.scopeCamera.fieldOfView = 10f;
				this.scopeCamera.eventMask = 0;
				UnturnedPostProcess.instance.setScopeCamera(this.scopeCamera);
				LevelLighting.updateLighting();
				PlayerLife life = base.player.life;
				life.onVisionUpdated = (VisionUpdated)Delegate.Combine(life.onVisionUpdated, new VisionUpdated(this.onVisionUpdated));
				PlayerLife life2 = base.player.life;
				life2.onLifeUpdated = (LifeUpdated)Delegate.Combine(life2.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
				PlayerMovement movement = base.player.movement;
				movement.onSeated = (Seated)Delegate.Combine(movement.onSeated, new Seated(this.onSeated));
			}
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x000F1224 File Offset: 0x000EF424
		private void OnDestroy()
		{
			if (this.scopeRenderTexture != null)
			{
				Object.Destroy(this.scopeRenderTexture);
				this.scopeRenderTexture = null;
			}
			if (this.scopeMaterial != null)
			{
				Object.Destroy(this.scopeMaterial);
				this.scopeMaterial = null;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06003453 RID: 13395 RVA: 0x000F1271 File Offset: 0x000EF471
		[Obsolete]
		public bool isCam
		{
			get
			{
				return this.IsLocallyUsingFreecam;
			}
		}

		// Token: 0x04001DF0 RID: 7664
		private static readonly float HEIGHT_LOOK_SIT = 1.6f;

		// Token: 0x04001DF1 RID: 7665
		private static readonly float HEIGHT_LOOK_STAND = 1.75f;

		// Token: 0x04001DF2 RID: 7666
		private static readonly float HEIGHT_LOOK_CROUCH = 1.2f;

		// Token: 0x04001DF3 RID: 7667
		private static readonly float HEIGHT_LOOK_PRONE = 0.35f;

		// Token: 0x04001DF4 RID: 7668
		private static readonly float HEIGHT_CAMERA_SIT = 0.7f;

		// Token: 0x04001DF5 RID: 7669
		private static readonly float HEIGHT_CAMERA_STAND = 1.05f;

		// Token: 0x04001DF6 RID: 7670
		private static readonly float HEIGHT_CAMERA_CROUCH = 0.95f;

		// Token: 0x04001DF7 RID: 7671
		private static readonly float HEIGHT_CAMERA_PRONE = 0.3f;

		// Token: 0x04001DF8 RID: 7672
		private static readonly float MIN_ANGLE_SIT = 60f;

		// Token: 0x04001DF9 RID: 7673
		private static readonly float MAX_ANGLE_SIT = 120f;

		// Token: 0x04001DFA RID: 7674
		private static readonly float MIN_ANGLE_CLIMB = 45f;

		// Token: 0x04001DFB RID: 7675
		private static readonly float MAX_ANGLE_CLIMB = 100f;

		// Token: 0x04001DFC RID: 7676
		private static readonly float MIN_ANGLE_SWIM = 45f;

		// Token: 0x04001DFD RID: 7677
		private static readonly float MAX_ANGLE_SWIM = 135f;

		// Token: 0x04001DFE RID: 7678
		private static readonly float MIN_ANGLE_STAND = 0f;

		// Token: 0x04001DFF RID: 7679
		private static readonly float MAX_ANGLE_STAND = 180f;

		// Token: 0x04001E00 RID: 7680
		private static readonly float MIN_ANGLE_CROUCH = 20f;

		// Token: 0x04001E01 RID: 7681
		private static readonly float MAX_ANGLE_CROUCH = 160f;

		// Token: 0x04001E02 RID: 7682
		private static readonly float MIN_ANGLE_PRONE = 60f;

		// Token: 0x04001E03 RID: 7683
		private static readonly float MAX_ANGLE_PRONE = 120f;

		// Token: 0x04001E04 RID: 7684
		public PerspectiveUpdated onPerspectiveUpdated;

		// Token: 0x04001E05 RID: 7685
		private Camera _characterCamera;

		// Token: 0x04001E06 RID: 7686
		private Camera _scopeCamera;

		// Token: 0x04001E08 RID: 7688
		private bool _isScopeActive;

		// Token: 0x04001E09 RID: 7689
		private bool isOverlayActive;

		// Token: 0x04001E0A RID: 7690
		private ELightingVision scopeVision;

		// Token: 0x04001E0B RID: 7691
		private Color scopeNightvisionColor;

		// Token: 0x04001E0C RID: 7692
		private float scopeNightvisionFogIntensity;

		// Token: 0x04001E0D RID: 7693
		private ELightingVision tempVision;

		// Token: 0x04001E0E RID: 7694
		private Color tempNightvisionColor;

		// Token: 0x04001E0F RID: 7695
		private float tempNightvisionFogIntensity;

		// Token: 0x04001E10 RID: 7696
		private Transform _aim;

		// Token: 0x04001E11 RID: 7697
		private static float characterHeight;

		// Token: 0x04001E12 RID: 7698
		private static float _characterYaw;

		// Token: 0x04001E13 RID: 7699
		public static float characterYaw;

		// Token: 0x04001E14 RID: 7700
		private static float killcam;

		// Token: 0x04001E15 RID: 7701
		private float yawInputMultiplier;

		// Token: 0x04001E16 RID: 7702
		private float pitchInputMultiplier;

		// Token: 0x04001E17 RID: 7703
		private float _pitch = 90f;

		// Token: 0x04001E18 RID: 7704
		private float _yaw;

		// Token: 0x04001E19 RID: 7705
		private float _look_x;

		// Token: 0x04001E1A RID: 7706
		private float _look_y;

		// Token: 0x04001E1B RID: 7707
		private float _orbitPitch;

		// Token: 0x04001E1C RID: 7708
		private float _orbitYaw;

		// Token: 0x04001E1D RID: 7709
		public float orbitSpeed = 16f;

		/// <summary>
		/// Reset to actual fov when first used.
		/// </summary>
		// Token: 0x04001E1E RID: 7710
		public float freecamVerticalFieldOfView = -1f;

		// Token: 0x04001E1F RID: 7711
		public Vector3 lockPosition;

		// Token: 0x04001E20 RID: 7712
		public Vector3 orbitPosition;

		/// <summary>
		/// If true, freecam controls take input priority.
		/// Previously named isOrbiting.
		/// </summary>
		// Token: 0x04001E21 RID: 7713
		public bool IsControllingFreecam;

		// Token: 0x04001E22 RID: 7714
		public bool isTracking;

		// Token: 0x04001E23 RID: 7715
		public bool isLocking;

		// Token: 0x04001E24 RID: 7716
		public bool isFocusing;

		// Token: 0x04001E25 RID: 7717
		public bool isSmoothing;

		// Token: 0x04001E27 RID: 7719
		public bool isIgnoringInput;

		// Token: 0x04001E28 RID: 7720
		private Vector3 smoothPosition;

		// Token: 0x04001E29 RID: 7721
		private Quaternion smoothRotation;

		// Token: 0x04001E2A RID: 7722
		public byte angle;

		// Token: 0x04001E2B RID: 7723
		public byte rot;

		// Token: 0x04001E2C RID: 7724
		private float recoil_x;

		// Token: 0x04001E2D RID: 7725
		private float recoil_y;

		// Token: 0x04001E2E RID: 7726
		public byte lastAngle;

		// Token: 0x04001E2F RID: 7727
		public byte lastRot;

		// Token: 0x04001E30 RID: 7728
		private Quaternion flinchLocalRotation;

		// Token: 0x04001E31 RID: 7729
		public Rk4SpringQ targetExplosionLocalRotation;

		/// <summary>
		/// Smoothing adds some initial blend-in which felt nicer for explosion rumble.
		/// </summary>
		// Token: 0x04001E32 RID: 7730
		private Quaternion smoothedExplosionLocalRotation = Quaternion.identity;

		// Token: 0x04001E33 RID: 7731
		public float explosionSmoothingSpeed;

		// Token: 0x04001E34 RID: 7732
		internal float mainCameraZoomFactor;

		// Token: 0x04001E35 RID: 7733
		private float scopeCameraZoomFactor;

		// Token: 0x04001E36 RID: 7734
		private float eyes;

		/// <summary>
		/// Slightly clamped third-person version of "eyes" value to prevent sweep from hitting floor.
		/// </summary>
		// Token: 0x04001E37 RID: 7735
		private float thirdPersonEyeHeight;

		// Token: 0x04001E38 RID: 7736
		public bool shouldUseZoomFactorForSensitivity;

		// Token: 0x04001E39 RID: 7737
		private EPlayerPerspective _perspective;

		// Token: 0x04001E3A RID: 7738
		private RenderTexture scopeRenderTexture;

		// Token: 0x04001E3B RID: 7739
		protected bool isZoomed;

		/// <summary>
		/// Can spectating be used without admin powers?
		/// Plugins can enable spectator mode.
		/// </summary>
		// Token: 0x04001E3C RID: 7740
		protected bool allowFreecamWithoutAdmin;

		/// <summary>
		/// Can workzone be used without admin powers?
		/// Plugins can enable workzone permissions.
		/// </summary>
		// Token: 0x04001E3D RID: 7741
		protected bool allowWorkzoneWithoutAdmin;

		/// <summary>
		/// Can spectator overlays be used without admin powers?
		/// Plugins can enable specstats permissions.
		/// </summary>
		// Token: 0x04001E3E RID: 7742
		protected bool allowSpecStatsWithoutAdmin;

		// Token: 0x04001E3F RID: 7743
		private static readonly ClientInstanceMethod<bool> SendFreecamAllowed = ClientInstanceMethod<bool>.Get(typeof(PlayerLook), "ReceiveFreecamAllowed");

		// Token: 0x04001E40 RID: 7744
		private static readonly ClientInstanceMethod<bool> SendWorkzoneAllowed = ClientInstanceMethod<bool>.Get(typeof(PlayerLook), "ReceiveWorkzoneAllowed");

		// Token: 0x04001E41 RID: 7745
		private static readonly ClientInstanceMethod<bool> SendSpecStatsAllowed = ClientInstanceMethod<bool>.Get(typeof(PlayerLook), "ReceiveSpecStatsAllowed");

		/// <summary>
		/// Multiple hits are necessary because the first returned hit is not always the closest.
		/// </summary>
		// Token: 0x04001E42 RID: 7746
		private static RaycastHit[] sweepHits = new RaycastHit[8];

		// Token: 0x04001E43 RID: 7747
		private const float NEAR_CLIP_SWEEP_RADIUS = 0.39f;
	}
}
