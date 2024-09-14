using System;
using System.Diagnostics;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Overrides vehicle physics values in bulk without building asset bundles.
	/// </summary>
	// Token: 0x0200037D RID: 893
	public class VehiclePhysicsProfileAsset : Asset
	{
		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x00062A61 File Offset: 0x00060C61
		// (set) Token: 0x06001B8A RID: 7050 RVA: 0x00062A69 File Offset: 0x00060C69
		public float? rootMassOverride { get; protected set; }

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001B8B RID: 7051 RVA: 0x00062A72 File Offset: 0x00060C72
		// (set) Token: 0x06001B8C RID: 7052 RVA: 0x00062A7A File Offset: 0x00060C7A
		public float? rootMassMultiplier { get; protected set; }

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001B8D RID: 7053 RVA: 0x00062A83 File Offset: 0x00060C83
		// (set) Token: 0x06001B8E RID: 7054 RVA: 0x00062A8B File Offset: 0x00060C8B
		public float? rootDragMultiplier { get; protected set; }

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001B8F RID: 7055 RVA: 0x00062A94 File Offset: 0x00060C94
		// (set) Token: 0x06001B90 RID: 7056 RVA: 0x00062A9C File Offset: 0x00060C9C
		public float? rootAngularDragMultiplier { get; protected set; }

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001B91 RID: 7057 RVA: 0x00062AA5 File Offset: 0x00060CA5
		// (set) Token: 0x06001B92 RID: 7058 RVA: 0x00062AAD File Offset: 0x00060CAD
		public float? carjackForceMultiplier { get; protected set; }

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001B93 RID: 7059 RVA: 0x00062AB6 File Offset: 0x00060CB6
		// (set) Token: 0x06001B94 RID: 7060 RVA: 0x00062ABE File Offset: 0x00060CBE
		public float? wheelMassOverride { get; protected set; }

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001B95 RID: 7061 RVA: 0x00062AC7 File Offset: 0x00060CC7
		// (set) Token: 0x06001B96 RID: 7062 RVA: 0x00062ACF File Offset: 0x00060CCF
		public float? wheelMassMultiplier { get; protected set; }

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001B97 RID: 7063 RVA: 0x00062AD8 File Offset: 0x00060CD8
		// (set) Token: 0x06001B98 RID: 7064 RVA: 0x00062AE0 File Offset: 0x00060CE0
		public float? wheelDampingRate { get; protected set; }

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001B99 RID: 7065 RVA: 0x00062AE9 File Offset: 0x00060CE9
		// (set) Token: 0x06001B9A RID: 7066 RVA: 0x00062AF1 File Offset: 0x00060CF1
		public float? wheelStiffnessTractionMultiplier { get; protected set; }

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001B9B RID: 7067 RVA: 0x00062AFA File Offset: 0x00060CFA
		// (set) Token: 0x06001B9C RID: 7068 RVA: 0x00062B02 File Offset: 0x00060D02
		public float? wheelSuspensionForce { get; protected set; }

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001B9D RID: 7069 RVA: 0x00062B0B File Offset: 0x00060D0B
		// (set) Token: 0x06001B9E RID: 7070 RVA: 0x00062B13 File Offset: 0x00060D13
		public float? wheelSuspensionDamper { get; protected set; }

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001B9F RID: 7071 RVA: 0x00062B1C File Offset: 0x00060D1C
		// (set) Token: 0x06001BA0 RID: 7072 RVA: 0x00062B24 File Offset: 0x00060D24
		public VehiclePhysicsProfileAsset.Friction? forwardFriction { get; protected set; }

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001BA1 RID: 7073 RVA: 0x00062B2D File Offset: 0x00060D2D
		// (set) Token: 0x06001BA2 RID: 7074 RVA: 0x00062B35 File Offset: 0x00060D35
		public VehiclePhysicsProfileAsset.Friction? sidewaysFriction { get; protected set; }

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001BA3 RID: 7075 RVA: 0x00062B3E File Offset: 0x00060D3E
		// (set) Token: 0x06001BA4 RID: 7076 RVA: 0x00062B46 File Offset: 0x00060D46
		public float? motorTorqueMultiplier { get; protected set; }

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001BA5 RID: 7077 RVA: 0x00062B4F File Offset: 0x00060D4F
		// (set) Token: 0x06001BA6 RID: 7078 RVA: 0x00062B57 File Offset: 0x00060D57
		public float? motorTorqueClampMultiplier { get; protected set; }

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x00062B60 File Offset: 0x00060D60
		// (set) Token: 0x06001BA8 RID: 7080 RVA: 0x00062B68 File Offset: 0x00060D68
		public float? brakeTorqueMultiplier { get; protected set; }

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001BA9 RID: 7081 RVA: 0x00062B71 File Offset: 0x00060D71
		// (set) Token: 0x06001BAA RID: 7082 RVA: 0x00062B79 File Offset: 0x00060D79
		public float? brakeTorqueTractionMultiplier { get; protected set; }

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001BAB RID: 7083 RVA: 0x00062B82 File Offset: 0x00060D82
		// (set) Token: 0x06001BAC RID: 7084 RVA: 0x00062B8A File Offset: 0x00060D8A
		public VehiclePhysicsProfileAsset.EDriveModel? wheelDriveModel { get; protected set; }

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001BAD RID: 7085 RVA: 0x00062B93 File Offset: 0x00060D93
		// (set) Token: 0x06001BAE RID: 7086 RVA: 0x00062B9B File Offset: 0x00060D9B
		public VehiclePhysicsProfileAsset.EDriveModel? wheelBrakeModel { get; protected set; }

		// Token: 0x06001BAF RID: 7087 RVA: 0x00062BA4 File Offset: 0x00060DA4
		protected VehiclePhysicsProfileAsset.Friction? readFriction(DatDictionary data, string key)
		{
			if (data.ContainsKey(key))
			{
				DatDictionary dictionary = data.GetDictionary(key);
				return new VehiclePhysicsProfileAsset.Friction?(new VehiclePhysicsProfileAsset.Friction
				{
					extremumSlip = dictionary.ParseFloat("Extremum_Slip", 0f),
					extremumValue = dictionary.ParseFloat("Extremum_Value", 0f),
					asymptoteSlip = dictionary.ParseFloat("Asymptote_Slip", 0f),
					asymptoteValue = dictionary.ParseFloat("Asymptote_Value", 0f),
					stiffness = dictionary.ParseFloat("Stiffness", 0f)
				});
			}
			return default(VehiclePhysicsProfileAsset.Friction?);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x00062C50 File Offset: 0x00060E50
		public void applyTo(InteractableVehicle vehicle)
		{
			Rigidbody component = vehicle.GetComponent<Rigidbody>();
			if (component != null)
			{
				if (this.rootMassOverride != null)
				{
					component.mass = this.rootMassOverride.Value;
				}
				else if (this.rootMassMultiplier != null)
				{
					component.mass *= this.rootMassMultiplier.Value;
				}
				if (this.rootDragMultiplier != null)
				{
					component.drag *= this.rootDragMultiplier.Value;
				}
				if (this.rootAngularDragMultiplier != null)
				{
					component.angularDrag *= this.rootAngularDragMultiplier.Value;
				}
			}
			bool flag = this.wheelMassOverride != null && vehicle.asset.wheelColliderMassOverride == null;
			bool flag2 = this.wheelSuspensionForce != null || this.wheelSuspensionDamper != null;
			foreach (Wheel wheel in vehicle.tires)
			{
				if (!(wheel.wheel == null))
				{
					if (this.wheelStiffnessTractionMultiplier != null)
					{
						wheel.stiffnessTractionMultiplier = this.wheelStiffnessTractionMultiplier.Value;
					}
					if (this.wheelDampingRate != null)
					{
						wheel.wheel.wheelDampingRate = this.wheelDampingRate.Value;
					}
					if (flag2)
					{
						JointSpring suspensionSpring = wheel.wheel.suspensionSpring;
						if (this.wheelSuspensionForce != null)
						{
							suspensionSpring.spring = this.wheelSuspensionForce.Value;
						}
						if (this.wheelSuspensionDamper != null)
						{
							suspensionSpring.damper = this.wheelSuspensionDamper.Value;
						}
						wheel.wheel.suspensionSpring = suspensionSpring;
					}
					if (this.sidewaysFriction != null)
					{
						wheel.stiffnessSideways = this.sidewaysFriction.Value.stiffness;
						WheelFrictionCurve sidewaysFriction = wheel.wheel.sidewaysFriction;
						this.sidewaysFriction.Value.applyTo(ref sidewaysFriction);
						wheel.wheel.sidewaysFriction = sidewaysFriction;
					}
					if (this.forwardFriction != null)
					{
						wheel.stiffnessForward = this.forwardFriction.Value.stiffness;
						WheelFrictionCurve forwardFriction = wheel.wheel.forwardFriction;
						this.forwardFriction.Value.applyTo(ref forwardFriction);
						wheel.wheel.forwardFriction = forwardFriction;
					}
					if (flag)
					{
						wheel.wheel.mass = this.wheelMassOverride.Value;
					}
					else if (this.wheelMassMultiplier != null)
					{
						wheel.wheel.mass *= this.wheelMassMultiplier.Value;
					}
					if (this.motorTorqueMultiplier != null)
					{
						wheel.motorTorqueMultiplier = this.motorTorqueMultiplier.Value;
					}
					if (this.motorTorqueClampMultiplier != null)
					{
						wheel.motorTorqueClampMultiplier = this.motorTorqueClampMultiplier.Value;
					}
					if (this.brakeTorqueMultiplier != null)
					{
						wheel.brakeTorqueMultiplier = this.brakeTorqueMultiplier.Value;
					}
					if (this.brakeTorqueTractionMultiplier != null)
					{
						wheel.brakeTorqueTractionMultiplier = this.brakeTorqueTractionMultiplier.Value;
					}
					if (this.wheelDriveModel != null && wheel.index >= 0)
					{
						switch (this.wheelDriveModel.Value)
						{
						case VehiclePhysicsProfileAsset.EDriveModel.Front:
							wheel.isPowered = (wheel.index < 2);
							goto IL_401;
						case VehiclePhysicsProfileAsset.EDriveModel.All:
							wheel.isPowered = true;
							goto IL_401;
						}
						wheel.isPowered = (wheel.index >= 2);
					}
					IL_401:
					if (this.wheelBrakeModel != null && wheel.index >= 0)
					{
						switch (this.wheelBrakeModel.Value)
						{
						case VehiclePhysicsProfileAsset.EDriveModel.Front:
							wheel.hasBrakes = (wheel.index < 2);
							goto IL_473;
						case VehiclePhysicsProfileAsset.EDriveModel.Rear:
							wheel.hasBrakes = (wheel.index >= 2);
							goto IL_473;
						}
						wheel.hasBrakes = true;
					}
				}
				IL_473:;
			}
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x000630E4 File Offset: 0x000612E4
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (data.ContainsKey("Root_Mass"))
			{
				this.rootMassOverride = new float?(data.ParseFloat("Root_Mass", 0f));
			}
			if (data.ContainsKey("Root_Mass_Multiplier"))
			{
				this.rootMassMultiplier = new float?(data.ParseFloat("Root_Mass_Multiplier", 0f));
			}
			if (data.ContainsKey("Root_Drag_Multiplier"))
			{
				this.rootDragMultiplier = new float?(data.ParseFloat("Root_Drag_Multiplier", 0f));
			}
			if (data.ContainsKey("Root_Angular_Drag_Multiplier"))
			{
				this.rootAngularDragMultiplier = new float?(data.ParseFloat("Root_Angular_Drag_Multiplier", 0f));
			}
			if (data.ContainsKey("Carjack_Force_Multiplier"))
			{
				this.carjackForceMultiplier = new float?(data.ParseFloat("Carjack_Force_Multiplier", 0f));
			}
			if (data.ContainsKey("Wheel_Mass"))
			{
				this.wheelMassOverride = new float?(data.ParseFloat("Wheel_Mass", 0f));
			}
			if (data.ContainsKey("Wheel_Mass_Multiplier"))
			{
				this.wheelMassMultiplier = new float?(data.ParseFloat("Wheel_Mass_Multiplier", 0f));
			}
			if (data.ContainsKey("Wheel_Damping_Rate"))
			{
				this.wheelDampingRate = new float?(data.ParseFloat("Wheel_Damping_Rate", 0f));
			}
			if (data.ContainsKey("Wheel_Stiffness_Traction_Multiplier"))
			{
				this.wheelStiffnessTractionMultiplier = new float?(data.ParseFloat("Wheel_Stiffness_Traction_Multiplier", 0f));
			}
			if (data.ContainsKey("Wheel_Suspension_Force"))
			{
				this.wheelSuspensionForce = new float?(data.ParseFloat("Wheel_Suspension_Force", 0f));
			}
			if (data.ContainsKey("Wheel_Suspension_Damper"))
			{
				this.wheelSuspensionDamper = new float?(data.ParseFloat("Wheel_Suspension_Damper", 0f));
			}
			this.sidewaysFriction = this.readFriction(data, "Wheel_Friction_Sideways");
			this.forwardFriction = this.readFriction(data, "Wheel_Friction_Forward");
			if (data.ContainsKey("Motor_Torque_Multiplier"))
			{
				this.motorTorqueMultiplier = new float?(data.ParseFloat("Motor_Torque_Multiplier", 0f));
			}
			if (data.ContainsKey("Motor_Torque_Clamp_Multiplier"))
			{
				this.motorTorqueClampMultiplier = new float?(data.ParseFloat("Motor_Torque_Clamp_Multiplier", 0f));
			}
			if (data.ContainsKey("Brake_Torque_Multiplier"))
			{
				this.brakeTorqueMultiplier = new float?(data.ParseFloat("Brake_Torque_Multiplier", 0f));
			}
			if (data.ContainsKey("Brake_Torque_Traction_Multiplier"))
			{
				this.brakeTorqueTractionMultiplier = new float?(data.ParseFloat("Brake_Torque_Traction_Multiplier", 0f));
			}
			if (data.ContainsKey("Wheel_Drive_Model"))
			{
				this.wheelDriveModel = new VehiclePhysicsProfileAsset.EDriveModel?(data.ParseEnum<VehiclePhysicsProfileAsset.EDriveModel>("Wheel_Drive_Model", VehiclePhysicsProfileAsset.EDriveModel.Front));
			}
			if (data.ContainsKey("Wheel_Brake_Model"))
			{
				this.wheelBrakeModel = new VehiclePhysicsProfileAsset.EDriveModel?(data.ParseEnum<VehiclePhysicsProfileAsset.EDriveModel>("Wheel_Brake_Model", VehiclePhysicsProfileAsset.EDriveModel.Front));
			}
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x000633BE File Offset: 0x000615BE
		[Conditional("LOG_VEHICLE_PHYSICS_PROFILE")]
		private void log(InteractableVehicle vehicle, string format, params object[] args)
		{
			UnturnedLog.info(vehicle.asset.name + ": " + format, args);
		}

		// Token: 0x04000CFB RID: 3323
		public static AssetReference<VehiclePhysicsProfileAsset> defaultProfile_Boat = new AssetReference<VehiclePhysicsProfileAsset>(new Guid("47258d0dcad14cb8be26e24c1ef3449e"));

		// Token: 0x04000CFC RID: 3324
		public static AssetReference<VehiclePhysicsProfileAsset> defaultProfile_Car = new AssetReference<VehiclePhysicsProfileAsset>(new Guid("6b91a94f01b6472eaca31d9420ec2367"));

		// Token: 0x04000CFD RID: 3325
		public static AssetReference<VehiclePhysicsProfileAsset> defaultProfile_Helicopter = new AssetReference<VehiclePhysicsProfileAsset>(new Guid("bb9f9f0204c4462ca7d976b87d1336d4"));

		// Token: 0x04000CFE RID: 3326
		public static AssetReference<VehiclePhysicsProfileAsset> defaultProfile_Plane = new AssetReference<VehiclePhysicsProfileAsset>(new Guid("93a47d6d40454335b4784e803628ac54"));

		// Token: 0x02000929 RID: 2345
		public struct Friction
		{
			// Token: 0x06004A88 RID: 19080 RVA: 0x001B1A4E File Offset: 0x001AFC4E
			public void applyTo(ref WheelFrictionCurve frictionCurve)
			{
				frictionCurve.extremumSlip = this.extremumSlip;
				frictionCurve.extremumValue = this.extremumValue;
				frictionCurve.asymptoteSlip = frictionCurve.asymptoteSlip;
				frictionCurve.asymptoteValue = frictionCurve.asymptoteValue;
			}

			// Token: 0x04003288 RID: 12936
			public float extremumSlip;

			// Token: 0x04003289 RID: 12937
			public float extremumValue;

			// Token: 0x0400328A RID: 12938
			public float asymptoteSlip;

			// Token: 0x0400328B RID: 12939
			public float asymptoteValue;

			// Token: 0x0400328C RID: 12940
			public float stiffness;
		}

		// Token: 0x0200092A RID: 2346
		public enum EDriveModel
		{
			// Token: 0x0400328E RID: 12942
			Front,
			// Token: 0x0400328F RID: 12943
			Rear,
			// Token: 0x04003290 RID: 12944
			All
		}
	}
}
