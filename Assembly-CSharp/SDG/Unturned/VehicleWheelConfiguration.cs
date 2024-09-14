using System;

namespace SDG.Unturned
{
	// Token: 0x02000379 RID: 889
	internal class VehicleWheelConfiguration : IDatParseable
	{
		// Token: 0x06001AB6 RID: 6838 RVA: 0x00060460 File Offset: 0x0005E660
		public bool TryParse(IDatNode node)
		{
			DatDictionary datDictionary = node as DatDictionary;
			if (datDictionary != null)
			{
				this.wheelColliderPath = datDictionary.GetString("WheelColliderPath", null);
				this.isColliderSteered = datDictionary.ParseBool("IsColliderSteered", false);
				this.isColliderPowered = datDictionary.ParseBool("IsColliderPowered", false);
				this.modelPath = datDictionary.GetString("ModelPath", null);
				this.isModelSteered = datDictionary.ParseBool("IsModelSteered", false);
				this.modelUseColliderPose = datDictionary.ParseBool("ModelUseColliderPose", false);
				this.modelRadius = datDictionary.ParseFloat("ModelRadius", -1f);
				this.copyColliderRpmIndex = datDictionary.ParseInt32("CopyColliderRpmIndex", -1);
				return true;
			}
			return false;
		}

		/// <summary>
		/// If true, this configuration was created by <see cref="!:InteractableVehicle.BuildAutomaticWheelConfiguration" />.
		/// Otherwise, this configuration was loaded from the vehicle asset file.
		/// </summary>
		// Token: 0x04000C64 RID: 3172
		public bool wasAutomaticallyGenerated;

		/// <summary>
		/// Transform path relative to Vehicle prefab with WheelCollider component.
		/// </summary>
		// Token: 0x04000C65 RID: 3173
		public string wheelColliderPath;

		/// <summary>
		/// If true, WheelCollider's steerAngle is set according to steering wheel.
		/// </summary>
		// Token: 0x04000C66 RID: 3174
		public bool isColliderSteered;

		/// <summary>
		/// If true, WheelCollider's motorTorque is set according to accelerator input.
		/// </summary>
		// Token: 0x04000C67 RID: 3175
		public bool isColliderPowered;

		/// <summary>
		/// Transform path relative to Vehicle prefab. Animated to match WheelCollider state.
		/// </summary>
		// Token: 0x04000C68 RID: 3176
		public string modelPath;

		/// <summary>
		/// If true, model is animated according to steering input.
		/// Only kept for backwards compatibility. Prior to wheel configurations, only certain WheelColliders actually
		/// received steering input, while multiple models would appear to steer. For example, the APC's front 4 wheels
		/// appeared to rotate but only the front 2 actually affected physics.
		/// </summary>
		// Token: 0x04000C69 RID: 3177
		public bool isModelSteered;

		/// <summary>
		/// If true, model ignores isModelSteered and instead uses WheelCollider.GetWorldPose when simulating or the
		/// replicated state from the server when not simulating. Defaults to false.
		/// </summary>
		// Token: 0x04000C6A RID: 3178
		public bool modelUseColliderPose;

		/// <summary>
		/// If greater than zero, visual-only wheels (without a collider) like the extra wheels of the Snowmobile use
		/// this radius to calculate their rolling speed.
		/// </summary>
		// Token: 0x04000C6B RID: 3179
		public float modelRadius;

		/// <summary>
		/// If set, visual-only wheels without a collider (like the back wheels of the snowmobile) can copy RPM from
		/// a wheel that does have a collider. Requires modelRadius to also be set.
		/// </summary>
		// Token: 0x04000C6C RID: 3180
		public int copyColliderRpmIndex;
	}
}
