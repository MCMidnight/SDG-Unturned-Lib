using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000488 RID: 1160
	public class Wheel
	{
		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x0600242B RID: 9259 RVA: 0x00090193 File Offset: 0x0008E393
		public InteractableVehicle vehicle
		{
			get
			{
				return this._vehicle;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x0600242C RID: 9260 RVA: 0x0009019B File Offset: 0x0008E39B
		// (set) Token: 0x0600242D RID: 9261 RVA: 0x000901A3 File Offset: 0x0008E3A3
		public int index { get; private set; }

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x0600242E RID: 9262 RVA: 0x000901AC File Offset: 0x0008E3AC
		public WheelCollider wheel
		{
			get
			{
				return this._wheel;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x000901B4 File Offset: 0x0008E3B4
		public bool isSteered
		{
			get
			{
				return this._isSteered;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002430 RID: 9264 RVA: 0x000901BC File Offset: 0x0008E3BC
		public bool isGrounded
		{
			get
			{
				return this._isGrounded;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x000901C4 File Offset: 0x0008E3C4
		// (set) Token: 0x06002432 RID: 9266 RVA: 0x000901CC File Offset: 0x0008E3CC
		public bool isAlive
		{
			get
			{
				return this._isAlive;
			}
			set
			{
				if (this.isAlive == value)
				{
					return;
				}
				this._isAlive = value;
				if (this.model != null)
				{
					this.model.gameObject.SetActive(this.isAlive);
				}
				this.UpdateColliderEnabled();
				this.triggerAliveChanged();
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x0009021A File Offset: 0x0008E41A
		public bool IsDead
		{
			get
			{
				return !this._isAlive;
			}
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x00090225 File Offset: 0x0008E425
		private void triggerAliveChanged()
		{
			WheelAliveChangedHandler wheelAliveChangedHandler = this.aliveChanged;
			if (wheelAliveChangedHandler == null)
			{
				return;
			}
			wheelAliveChangedHandler(this);
		}

		/// <summary>
		/// Turn on/off physics as needed. Overridden by isAlive.
		/// </summary>
		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x00090238 File Offset: 0x0008E438
		// (set) Token: 0x06002436 RID: 9270 RVA: 0x00090240 File Offset: 0x0008E440
		public bool isPhysical
		{
			get
			{
				return this._isPhysical;
			}
			set
			{
				this._isPhysical = value;
				this.UpdateColliderEnabled();
			}
		}

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06002437 RID: 9271 RVA: 0x00090250 File Offset: 0x0008E450
		// (remove) Token: 0x06002438 RID: 9272 RVA: 0x00090288 File Offset: 0x0008E488
		public event WheelAliveChangedHandler aliveChanged;

		// Token: 0x06002439 RID: 9273 RVA: 0x000902BD File Offset: 0x0008E4BD
		public void askRepair()
		{
			if (this.isAlive)
			{
				return;
			}
			this.isAlive = true;
			this.vehicle.sendTireAliveMaskUpdate();
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000902DC File Offset: 0x0008E4DC
		public void askDamage()
		{
			if (!this.isAlive)
			{
				return;
			}
			this.isAlive = false;
			this.vehicle.sendTireAliveMaskUpdate();
			EffectAsset effectAsset = Wheel.Rubber_0_Ref.Find();
			if (effectAsset != null)
			{
				TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
				parameters.relevantDistance = EffectManager.SMALL;
				parameters.position = this.wheel.transform.position;
				parameters.SetDirection(this.wheel.transform.up);
				EffectManager.triggerEffect(parameters);
			}
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x0009035D File Offset: 0x0008E55D
		private void UpdateColliderEnabled()
		{
			if (this.wheel != null)
			{
				this.wheel.gameObject.SetActive(this.isPhysical && this.isAlive);
			}
		}

		/// <summary>
		/// Called after construction and on all clients and server when a player stops driving.
		/// </summary>
		// Token: 0x0600243C RID: 9276 RVA: 0x00090390 File Offset: 0x0008E590
		internal void Reset()
		{
			this.latestLocalSteeringInput = 0f;
			this.latestLocalAccelerationInput = 0f;
			this.latestLocalBrakingInput = false;
			if (this.wheel != null)
			{
				this.wheel.steerAngle = 0f;
				this.wheel.motorTorque = 0f;
				this.wheel.brakeTorque = this.vehicle.asset.brake * 0.25f;
				WheelFrictionCurve sidewaysFriction = this.wheel.sidewaysFriction;
				sidewaysFriction.stiffness = 0.25f;
				this.wheel.sidewaysFriction = sidewaysFriction;
				WheelFrictionCurve forwardFriction = this.wheel.forwardFriction;
				forwardFriction.stiffness = 0.25f;
				this.wheel.forwardFriction = forwardFriction;
			}
		}

		/// <summary>
		/// Called when vehicles explodes.
		/// </summary>
		// Token: 0x0600243D RID: 9277 RVA: 0x00090454 File Offset: 0x0008E654
		internal void Explode()
		{
			if (this.model == null || this.IsDead)
			{
				return;
			}
			Collider component = this.model.GetComponent<Collider>();
			if (component == null)
			{
				return;
			}
			EffectManager.RegisterDebris(this.model.gameObject);
			this.model.transform.parent = null;
			component.enabled = true;
			Rigidbody orAddComponent = this.model.gameObject.GetOrAddComponent<Rigidbody>();
			orAddComponent.interpolation = RigidbodyInterpolation.Interpolate;
			orAddComponent.collisionDetectionMode = CollisionDetectionMode.Discrete;
			orAddComponent.drag = 0.5f;
			orAddComponent.angularDrag = 0.1f;
			Object.Destroy(this.model.gameObject, 8f);
			if (this.index % 2 == 0)
			{
				orAddComponent.AddForce(-this.model.right * 512f + Vector3.up * 128f);
				return;
			}
			orAddComponent.AddForce(this.model.right * 512f + Vector3.up * 128f);
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x00090570 File Offset: 0x0008E770
		internal void UpdateGrounded()
		{
			if (this.wheel == null)
			{
				return;
			}
			this._isGrounded = this.wheel.GetGroundHit(ref this.mostRecentGroundHit);
			if (this._isGrounded)
			{
				string materialName = PhysicsTool.GetMaterialName(this.mostRecentGroundHit);
				this.replicatedGroundMaterial = PhysicsMaterialNetTable.GetNetId(materialName);
				return;
			}
			this.replicatedGroundMaterial = PhysicsMaterialNetId.NULL;
		}

		/// <summary>
		/// Called during FixedUpdate if vehicle is driven by the local player.
		/// </summary>
		// Token: 0x0600243F RID: 9279 RVA: 0x000905CF File Offset: 0x0008E7CF
		internal void ClientSimulate(float input_x, float input_y, bool inputBrake, float delta)
		{
			if (this.wheel == null)
			{
				return;
			}
			if (this.isSteered)
			{
				this.latestLocalSteeringInput = input_x;
			}
			this.latestLocalAccelerationInput = input_y;
			this.latestLocalBrakingInput = inputBrake;
			this.UpdateGrounded();
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x00090604 File Offset: 0x0008E804
		internal void OnVehicleDestroyed()
		{
			if (this.motionEffectInstances != null)
			{
				foreach (TireMotionEffectInstance tireMotionEffectInstance in this.motionEffectInstances)
				{
					tireMotionEffectInstance.DestroyEffect();
					Wheel.motionEffectInstancesPool.Add(tireMotionEffectInstance);
				}
				this.motionEffectInstances.Clear();
				this.motionEffectInstances = null;
				this.currentGroundEffect = null;
			}
			if (this.model != null && this.model.transform.parent == null)
			{
				Object.Destroy(this.model.gameObject);
			}
		}

		/// <summary>
		/// Calculate suspension state from GetWorldPose result.
		///
		/// Nelson 2024-03-25: Originally we used the result of GetWorldPose for the model transform and calculated
		/// the suspension state from it because I thought Unity was internally using the spring position that isn't
		/// (currently) exposed to the API. Whether or not it is, it seems fine to calculate the spring position using
		/// the ground hit point instead. We switched entirely away from GetWorldPose so that the wheel can retain
		/// its roll angle when transitioning between locally simulated and replicated.
		/// </summary>
		// Token: 0x06002441 RID: 9281 RVA: 0x000906B8 File Offset: 0x0008E8B8
		private float CalculateNormalizedSuspensionPosition(Vector3 worldPosePosition)
		{
			if (this._wheel.suspensionDistance > 1E-45f)
			{
				Vector3 b = this._wheel.transform.TransformPoint(this._wheel.center);
				Vector3 rhs = -this._wheel.transform.up;
				return Mathf.Clamp01(Vector3.Dot(worldPosePosition - b, rhs) / this._wheel.suspensionDistance);
			}
			return 0f;
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x0009072D File Offset: 0x0008E92D
		private float CalculateNormalizedSuspensionPosition(float distanceAlongSuspension)
		{
			if (this._wheel.suspensionDistance > 1E-45f)
			{
				return Mathf.Clamp01(distanceAlongSuspension / this._wheel.suspensionDistance);
			}
			return 0f;
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0009075C File Offset: 0x0008E95C
		private TireMotionEffectInstance FindOrAddMotionEffect(string materialName)
		{
			foreach (TireMotionEffectInstance tireMotionEffectInstance in this.motionEffectInstances)
			{
				if (tireMotionEffectInstance.materialName == materialName)
				{
					return tireMotionEffectInstance;
				}
			}
			TireMotionEffectInstance tireMotionEffectInstance2;
			if (Wheel.motionEffectInstancesPool.Count > 0)
			{
				tireMotionEffectInstance2 = Wheel.motionEffectInstancesPool.GetAndRemoveTail<TireMotionEffectInstance>();
				tireMotionEffectInstance2.Reset();
			}
			else
			{
				tireMotionEffectInstance2 = new TireMotionEffectInstance();
			}
			tireMotionEffectInstance2.materialName = materialName;
			this.motionEffectInstances.Add(tireMotionEffectInstance2);
			return tireMotionEffectInstance2;
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x000907F8 File Offset: 0x0008E9F8
		private void UpdateMotionEffect(Vector3 groundHitPosition, bool isVisualGrounded)
		{
			if (this.motionEffectInstances == null)
			{
				return;
			}
			string materialName = PhysicsMaterialNetTable.GetMaterialName(this.replicatedGroundMaterial);
			TireMotionEffectInstance tireMotionEffectInstance;
			if (string.IsNullOrEmpty(materialName))
			{
				tireMotionEffectInstance = null;
			}
			else if (this.currentGroundEffect == null || this.currentGroundEffect.materialName != materialName)
			{
				tireMotionEffectInstance = this.FindOrAddMotionEffect(materialName);
			}
			else
			{
				tireMotionEffectInstance = this.currentGroundEffect;
			}
			if (this.currentGroundEffect != tireMotionEffectInstance)
			{
				if (this.currentGroundEffect != null)
				{
					this.currentGroundEffect.StopParticleSystem();
				}
				this.currentGroundEffect = tireMotionEffectInstance;
				if (this.currentGroundEffect != null)
				{
					this.currentGroundEffect.hasTriedToInstantiateEffect = false;
				}
			}
			if (this.currentGroundEffect != null)
			{
				if (isVisualGrounded)
				{
					if (!this.currentGroundEffect.hasTriedToInstantiateEffect && !MathfEx.IsNearlyZero(this.vehicle.ReplicatedForwardVelocity, 0.1f))
					{
						this.currentGroundEffect.InstantiateEffect();
					}
					if (this.currentGroundEffect.particleSystem != null)
					{
						Vector3 up = this._wheel.transform.up;
						Vector3 b = this._wheel.transform.forward * -Mathf.Sign(this.vehicle.AnimatedForwardVelocity);
						float t = this.vehicle.GetAnimatedForwardSpeedPercentageOfTargetSpeed() * 0.5f;
						Quaternion rotation = Quaternion.LookRotation(Vector3.Lerp(up, b, t));
						this.currentGroundEffect.transform.SetPositionAndRotation(groundHitPosition, rotation);
						if (this.currentGroundEffect.isReadyToPlay && !this.currentGroundEffect.particleSystem.isPlaying)
						{
							this.currentGroundEffect.particleSystem.Play();
						}
						this.currentGroundEffect.isReadyToPlay = true;
					}
				}
				else
				{
					this.currentGroundEffect.StopParticleSystem();
				}
			}
			for (int i = this.motionEffectInstances.Count - 1; i >= 0; i--)
			{
				TireMotionEffectInstance tireMotionEffectInstance2 = this.motionEffectInstances[i];
				if (tireMotionEffectInstance2 != this.currentGroundEffect && (tireMotionEffectInstance2.particleSystem == null || !tireMotionEffectInstance2.particleSystem.IsAlive()))
				{
					tireMotionEffectInstance2.DestroyEffect();
					this.motionEffectInstances.RemoveAtFast(i);
					Wheel.motionEffectInstancesPool.Add(tireMotionEffectInstance2);
				}
			}
		}

		/// <summary>
		/// Called during Update on dedicated server only if replicated suspension state is enabled.
		/// </summary>
		// Token: 0x06002445 RID: 9285 RVA: 0x00090A04 File Offset: 0x0008EC04
		internal void UpdateServerSuspensionAndPhysicsMaterial()
		{
			if (this._wheel != null)
			{
				this._isGrounded = this._wheel.GetGroundHit(ref this.mostRecentGroundHit);
				float distanceAlongSuspension;
				if (this._isGrounded)
				{
					Vector3 b = this._wheel.transform.TransformPoint(this._wheel.center);
					Vector3 rhs = -this.vehicle.transform.up;
					distanceAlongSuspension = Vector3.Dot(this.mostRecentGroundHit.point - b, rhs) - this._wheel.radius;
					string materialName = PhysicsTool.GetMaterialName(this.mostRecentGroundHit);
					this.replicatedGroundMaterial = PhysicsMaterialNetTable.GetNetId(materialName);
				}
				else
				{
					distanceAlongSuspension = this._wheel.suspensionDistance;
					this.replicatedGroundMaterial = PhysicsMaterialNetId.NULL;
				}
				this.replicatedSuspensionState = this.CalculateNormalizedSuspensionPosition(distanceAlongSuspension);
			}
		}

		/// <summary>
		/// Set replicated suspension state AND animated suspension state when vehicle is first received.
		/// </summary>
		/// <param name="state"></param>
		// Token: 0x06002446 RID: 9286 RVA: 0x00090AD5 File Offset: 0x0008ECD5
		internal void TeleportSuspensionState(float state)
		{
			this.replicatedSuspensionState = state;
			this.animatedSuspensionState = state;
		}

		/// <summary>
		/// Called during Update on client.
		/// </summary>
		// Token: 0x06002447 RID: 9287 RVA: 0x00090AE8 File Offset: 0x0008ECE8
		internal void UpdateModel(float deltaTime)
		{
			if (!this.config.modelUseColliderPose || !(this._wheel != null))
			{
				if (this.config.modelRadius > 1E-45f)
				{
					if (this._isPhysical && this.config.copyColliderRpmIndex >= 0)
					{
						Wheel wheelAtIndex = this.vehicle.GetWheelAtIndex(this.config.copyColliderRpmIndex);
						if (wheelAtIndex != null && wheelAtIndex.wheel != null && wheelAtIndex.wheel.radius > 1E-45f)
						{
							float num = wheelAtIndex.wheel.radius * wheelAtIndex.wheel.rpm / this.config.modelRadius / 60f * 360f * deltaTime;
							this.rollAngleDegrees += num;
							this.rollAngleDegrees = (this.rollAngleDegrees % 360f + 360f) % 360f;
						}
					}
					else
					{
						float num2 = this.vehicle.AnimatedForwardVelocity * deltaTime;
						float num3 = 6.2831855f * this.config.modelRadius;
						float num4 = num2 / num3 * 360f;
						this.rollAngleDegrees += num4;
						this.rollAngleDegrees = (this.rollAngleDegrees % 360f + 360f) % 360f;
					}
				}
				else
				{
					this.rollAngleDegrees += this.vehicle.AnimatedForwardVelocity * 45f * deltaTime;
					this.rollAngleDegrees = (this.rollAngleDegrees % 360f + 360f) % 360f;
				}
				this.model.localRotation = this.rest;
				if (this.config.isModelSteered)
				{
					this.model.Rotate(0f, this.vehicle.AnimatedSteeringAngle, 0f, Space.Self);
				}
				this.model.Rotate(this.rollAngleDegrees, 0f, 0f, Space.Self);
				return;
			}
			Vector3 vector = this._wheel.transform.TransformPoint(this._wheel.center);
			Vector3 vector2 = -this.vehicle.transform.up;
			Vector3 onNormal = -this._wheel.transform.up;
			if (this._isPhysical)
			{
				WheelHit hit;
				float num5;
				if (this._wheel.GetGroundHit(ref hit))
				{
					Vector3 point = hit.point;
					num5 = Vector3.Dot(point - vector, vector2) - this._wheel.radius;
					string materialName = PhysicsTool.GetMaterialName(hit);
					this.replicatedGroundMaterial = PhysicsMaterialNetTable.GetNetId(materialName);
					this.UpdateMotionEffect(point, true);
				}
				else
				{
					num5 = this._wheel.suspensionDistance;
					this.replicatedGroundMaterial = PhysicsMaterialNetId.NULL;
					this.UpdateMotionEffect(Vector3.zero, false);
				}
				Vector3 b = Vector3.Project(vector2 * num5, onNormal);
				Vector3 position = vector + b;
				float num6 = this._wheel.rpm / 60f * 360f * deltaTime;
				this.rollAngleDegrees += num6;
				this.rollAngleDegrees = (this.rollAngleDegrees % 360f + 360f) % 360f;
				Quaternion quaternion = this.rest;
				quaternion = Quaternion.AngleAxis(this.rollAngleDegrees, Vector3.right) * quaternion;
				quaternion = Quaternion.AngleAxis(this.wheel.steerAngle, Vector3.up) * quaternion;
				Quaternion rotation = this.model.parent.TransformRotation(quaternion);
				this.model.SetPositionAndRotation(position, rotation);
				this.replicatedSuspensionState = this.CalculateNormalizedSuspensionPosition(num5);
				this.animatedSuspensionState = this.replicatedSuspensionState;
				return;
			}
			float t = 1f - Mathf.Pow(2f, -13f * Time.deltaTime);
			this.animatedSuspensionState = Mathf.Lerp(this.animatedSuspensionState, this.replicatedSuspensionState, t);
			float num7 = this.animatedSuspensionState * this._wheel.suspensionDistance;
			Vector3.Project(vector2 * num7, onNormal);
			Vector3 position2 = vector + vector2 * num7;
			if (this._wheel.radius > 1E-45f)
			{
				float num8 = this.vehicle.AnimatedForwardVelocity * deltaTime;
				float num9 = 6.2831855f * this._wheel.radius;
				float num10 = num8 / num9 * 360f;
				this.rollAngleDegrees += num10;
				this.rollAngleDegrees = (this.rollAngleDegrees % 360f + 360f) % 360f;
			}
			Quaternion quaternion2 = this.rest;
			quaternion2 = Quaternion.AngleAxis(this.rollAngleDegrees, Vector3.right) * quaternion2;
			if (this.config.isColliderSteered)
			{
				quaternion2 = Quaternion.AngleAxis(this.vehicle.AnimatedSteeringAngle, Vector3.up) * quaternion2;
			}
			Quaternion rotation2 = this.model.parent.TransformRotation(quaternion2);
			this.model.SetPositionAndRotation(position2, rotation2);
			if (this.animatedSuspensionState < 0.99f)
			{
				this.UpdateMotionEffect(vector + vector2 * (num7 + this._wheel.radius), true);
				return;
			}
			this.UpdateMotionEffect(Vector3.zero, false);
		}

		/// <summary>
		/// Called during Update if vehicle is driven by the local player.
		/// </summary>
		// Token: 0x06002448 RID: 9288 RVA: 0x00091010 File Offset: 0x0008F210
		internal void UpdateLocallyDriven(float delta, float availableTorque)
		{
			if (this.wheel == null)
			{
				return;
			}
			float num = Mathf.Lerp(this.vehicle.asset.steerMax, this.vehicle.asset.steerMin, this.vehicle.GetReplicatedForwardSpeedPercentageOfTargetSpeed());
			float target = this.latestLocalSteeringInput * num;
			float maxDelta = this.vehicle.asset.SteeringAngleTurnSpeed * delta;
			this.wheel.steerAngle = Mathf.MoveTowards(this.wheel.steerAngle, target, maxDelta);
			WheelFrictionCurve sidewaysFriction = this.wheel.sidewaysFriction;
			WheelFrictionCurve forwardFriction = this.wheel.forwardFriction;
			if (this.vehicle.asset.hasSleds)
			{
				sidewaysFriction.stiffness = Mathf.Lerp(this.wheel.sidewaysFriction.stiffness, 0.25f, 4f * delta);
				forwardFriction.stiffness = Mathf.Lerp(this.wheel.forwardFriction.stiffness, 0.25f, 4f * delta);
			}
			else
			{
				float num2 = Mathf.Lerp(1f, this.stiffnessTractionMultiplier, this.vehicle.slip);
				sidewaysFriction.stiffness = Mathf.Lerp(this.wheel.sidewaysFriction.stiffness, this.stiffnessSideways * num2, 4f * delta);
				forwardFriction.stiffness = Mathf.Lerp(this.wheel.forwardFriction.stiffness, this.stiffnessForward * num2, 4f * delta);
			}
			this.wheel.sidewaysFriction = sidewaysFriction;
			this.wheel.forwardFriction = forwardFriction;
			bool flag = false;
			float num3;
			bool flag2;
			if (this.latestLocalAccelerationInput > 0.01f)
			{
				if (this.vehicle.ReplicatedForwardVelocity > -0.05f)
				{
					if (this.vehicle.asset.UsesEngineRpmAndGears)
					{
						num3 = availableTorque * this.latestLocalAccelerationInput;
					}
					else
					{
						num3 = this.vehicle.asset.TargetForwardVelocity * this.latestLocalAccelerationInput * this.motorTorqueMultiplier;
						if (this.vehicle.ReplicatedForwardVelocity > this.vehicle.asset.TargetForwardVelocity)
						{
							num3 *= this.motorTorqueClampMultiplier;
						}
					}
					flag2 = false;
				}
				else
				{
					num3 = 0f;
					flag2 = true;
				}
			}
			else if (this.latestLocalAccelerationInput < -0.01f)
			{
				if (this.vehicle.ReplicatedForwardVelocity < 0.05f)
				{
					if (this.vehicle.asset.UsesEngineRpmAndGears)
					{
						num3 = availableTorque * this.latestLocalAccelerationInput;
					}
					else
					{
						num3 = this.vehicle.asset.TargetReverseVelocity * -this.latestLocalAccelerationInput * this.motorTorqueMultiplier;
						if (this.vehicle.ReplicatedForwardVelocity < this.vehicle.asset.TargetReverseVelocity)
						{
							num3 *= this.motorTorqueClampMultiplier;
						}
					}
					flag2 = false;
				}
				else
				{
					num3 = 0f;
					flag2 = true;
				}
			}
			else
			{
				num3 = 0f;
				flag2 = false;
				flag = true;
			}
			if (this.isPowered)
			{
				this.wheel.motorTorque = num3;
			}
			else
			{
				this.wheel.motorTorque = 0f;
			}
			if (this.hasBrakes && (flag2 || this.latestLocalBrakingInput))
			{
				float num4 = Mathf.Lerp(1f, this.brakeTorqueTractionMultiplier, this.vehicle.slip);
				num4 *= this.brakeTorqueMultiplier;
				this.wheel.brakeTorque = this.vehicle.asset.brake * num4;
				return;
			}
			if (flag)
			{
				this.wheel.brakeTorque = 1f;
				return;
			}
			this.wheel.brakeTorque = 0f;
		}

		/// <summary>
		/// Called during Update on the server while vehicle is driven by player.
		/// </summary>
		// Token: 0x06002449 RID: 9289 RVA: 0x000913A4 File Offset: 0x0008F5A4
		internal void CheckForTraps()
		{
			RaycastHit raycastHit;
			Physics.Raycast(new Ray(this.wheel.transform.position, -this.wheel.transform.up), out raycastHit, this.wheel.suspensionDistance + this.wheel.radius, 134217728);
			if (raycastHit.transform != null && raycastHit.transform.CompareTag("Barricade") && raycastHit.transform.GetComponent<InteractableTrapDamageTires>() != null)
			{
				this.askDamage();
			}
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x0009143C File Offset: 0x0008F63C
		internal Wheel(InteractableVehicle newVehicle, int newIndex, WheelCollider newWheel, Transform newModel, VehicleWheelConfiguration newConfiguration)
		{
			this._vehicle = newVehicle;
			this.index = newIndex;
			this._wheel = newWheel;
			this.model = newModel;
			this.config = newConfiguration;
			if (this.wheel != null)
			{
				if (this.config.wasAutomaticallyGenerated)
				{
					this.wheel.forceAppPointDistance = 0f;
				}
				this.replicatedSuspensionState = this.wheel.suspensionSpring.targetPosition;
				this.animatedSuspensionState = this.replicatedSuspensionState;
				if (this.config.modelUseColliderPose)
				{
					this.motionEffectInstances = new List<TireMotionEffectInstance>();
				}
				this.currentGroundEffect = null;
			}
			this._isSteered = this.config.isColliderSteered;
			this.isPowered = this.config.isColliderPowered;
			this.hasBrakes = true;
			this.isAlive = true;
			if (this.model != null)
			{
				this.rest = this.model.localRotation;
			}
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x0009157C File Offset: 0x0008F77C
		[Obsolete("Should not have been public.")]
		public void checkForTraps()
		{
			this.CheckForTraps();
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x00091584 File Offset: 0x0008F784
		[Obsolete("Should not have been public.")]
		public void update(float delta)
		{
			this.UpdateLocallyDriven(delta, 0f);
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x00091592 File Offset: 0x0008F792
		[Obsolete("Should not have been public.")]
		public void simulate(float input_x, float input_y, bool inputBrake, float delta)
		{
			this.ClientSimulate(input_x, input_y, inputBrake, delta);
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x0009159F File Offset: 0x0008F79F
		[Obsolete("Should not have been public.")]
		public void reset()
		{
			this.Reset();
		}

		// Token: 0x0400122D RID: 4653
		private InteractableVehicle _vehicle;

		// Token: 0x0400122F RID: 4655
		private WheelCollider _wheel;

		// Token: 0x04001230 RID: 4656
		public Transform model;

		// Token: 0x04001231 RID: 4657
		public Quaternion rest;

		// Token: 0x04001232 RID: 4658
		private bool _isSteered;

		// Token: 0x04001233 RID: 4659
		public bool isPowered;

		// Token: 0x04001234 RID: 4660
		private VehicleWheelConfiguration config;

		/// <summary>
		/// Does this wheel affect brake torque?
		/// </summary>
		// Token: 0x04001235 RID: 4661
		public bool hasBrakes;

		// Token: 0x04001236 RID: 4662
		private bool _isGrounded;

		// Token: 0x04001237 RID: 4663
		internal WheelHit mostRecentGroundHit;

		// Token: 0x04001238 RID: 4664
		private bool _isAlive;

		// Token: 0x04001239 RID: 4665
		public float stiffnessTractionMultiplier = 0.25f;

		// Token: 0x0400123A RID: 4666
		public float stiffnessSideways = 1f;

		// Token: 0x0400123B RID: 4667
		public float stiffnessForward = 2f;

		// Token: 0x0400123C RID: 4668
		public float motorTorqueMultiplier = 1f;

		// Token: 0x0400123D RID: 4669
		public float motorTorqueClampMultiplier = 0.5f;

		// Token: 0x0400123E RID: 4670
		public float brakeTorqueMultiplier = 1f;

		// Token: 0x0400123F RID: 4671
		public float brakeTorqueTractionMultiplier = 0.5f;

		// Token: 0x04001240 RID: 4672
		private float latestLocalSteeringInput;

		// Token: 0x04001241 RID: 4673
		private float latestLocalAccelerationInput;

		// Token: 0x04001242 RID: 4674
		private bool latestLocalBrakingInput;

		// Token: 0x04001243 RID: 4675
		private bool _isPhysical;

		/// <summary>
		/// [0.0, 1.0] normalized position of wheel along suspension.
		/// </summary>
		// Token: 0x04001244 RID: 4676
		internal float replicatedSuspensionState;

		/// <summary>
		/// [0.0, 1.0] normalized position animated toward replicatedSuspensionState.
		/// </summary>
		// Token: 0x04001245 RID: 4677
		private float animatedSuspensionState;

		// Token: 0x04001246 RID: 4678
		internal PhysicsMaterialNetId replicatedGroundMaterial;

		/// <summary>
		/// [0, 360] angle of rotation around wheel axle. Measured in degrees because Quaternion.AngleAxis takes degrees.
		///
		/// We track rather than using GetWorldPose so that we can alternate between using replicated and simulated
		/// results without snapping transforms.
		/// </summary>
		// Token: 0x04001247 RID: 4679
		private float rollAngleDegrees;

		/// <summary>
		/// List is created if this wheel has a collider and uses collider pose. Null when vehicle is destroyed to
		/// prevent creation of more effects.
		/// </summary>
		// Token: 0x04001248 RID: 4680
		private List<TireMotionEffectInstance> motionEffectInstances;

		/// <summary>
		/// Instance corresponding to current ground material. Doesn't necessarily mean the particle system is active.
		/// </summary>
		// Token: 0x04001249 RID: 4681
		private TireMotionEffectInstance currentGroundEffect;

		// Token: 0x0400124A RID: 4682
		private static List<TireMotionEffectInstance> motionEffectInstancesPool = new List<TireMotionEffectInstance>();

		// Token: 0x0400124C RID: 4684
		private static readonly AssetReference<EffectAsset> Rubber_0_Ref = new AssetReference<EffectAsset>("a87c5007b22542dcbf3599ee3faceadd");
	}
}
