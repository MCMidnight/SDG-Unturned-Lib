using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200061F RID: 1567
	public class DrivingPlayerInputPacket : PlayerInputPacket
	{
		// Token: 0x060032A4 RID: 12964 RVA: 0x000E4CD0 File Offset: 0x000E2ED0
		public override void read(SteamChannel channel, NetPakReader reader)
		{
			base.read(channel, reader);
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref this.position, 13, 8);
			UnityNetPakReaderEx.ReadQuaternion(reader, ref this.rotation, 11);
			SystemNetPakReaderEx.ReadUnsignedClampedFloat(reader, 8, 2, ref this.speed);
			SystemNetPakReaderEx.ReadClampedFloat(reader, 9, 2, ref this.forwardVelocity);
			SystemNetPakReaderEx.ReadSignedNormalizedFloat(reader, 2, ref this.steeringInput);
			SystemNetPakReaderEx.ReadClampedFloat(reader, 9, 2, ref this.velocityInput);
			if (this.vehicle != null && this.vehicle.asset != null)
			{
				if (this.vehicle.asset.replicatedWheelIndices != null)
				{
					foreach (int num in this.vehicle.asset.replicatedWheelIndices)
					{
						Wheel wheelAtIndex = this.vehicle.GetWheelAtIndex(num);
						if (wheelAtIndex == null)
						{
							UnturnedLog.error(string.Format("Missing wheel for replicated index: {0}", num));
							float num2;
							SystemNetPakReaderEx.ReadUnsignedNormalizedFloat(reader, 4, ref num2);
							PhysicsMaterialNetId physicsMaterialNetId;
							reader.ReadPhysicsMaterialNetId(out physicsMaterialNetId);
						}
						else
						{
							float replicatedSuspensionState;
							if (SystemNetPakReaderEx.ReadUnsignedNormalizedFloat(reader, 4, ref replicatedSuspensionState))
							{
								wheelAtIndex.replicatedSuspensionState = replicatedSuspensionState;
							}
							reader.ReadPhysicsMaterialNetId(out wheelAtIndex.replicatedGroundMaterial);
						}
					}
				}
				if (this.vehicle.asset.UsesEngineRpmAndGears)
				{
					uint num3;
					reader.ReadBits(3, ref num3);
					int num4 = (int)(num3 - 1U);
					num4 = Mathf.Clamp(num4, -1, this.vehicle.asset.forwardGearRatios.Length);
					this.vehicle.GearNumber = num4;
					float t;
					SystemNetPakReaderEx.ReadUnsignedNormalizedFloat(reader, 7, ref t);
					this.vehicle.ReplicatedEngineRpm = Mathf.Lerp(this.vehicle.asset.EngineIdleRpm, this.vehicle.asset.EngineMaxRpm, t);
				}
			}
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x000E4E78 File Offset: 0x000E3078
		public override void write(NetPakWriter writer)
		{
			base.write(writer);
			UnityNetPakWriterEx.WriteClampedVector3(writer, this.position, 13, 8);
			UnityNetPakWriterEx.WriteQuaternion(writer, this.rotation, 11);
			SystemNetPakWriterEx.WriteUnsignedClampedFloat(writer, this.speed, 8, 2);
			SystemNetPakWriterEx.WriteClampedFloat(writer, this.forwardVelocity, 9, 2);
			SystemNetPakWriterEx.WriteSignedNormalizedFloat(writer, this.steeringInput, 2);
			SystemNetPakWriterEx.WriteClampedFloat(writer, this.velocityInput, 9, 2);
			if (this.vehicle != null && this.vehicle.asset != null)
			{
				if (this.vehicle.asset.replicatedWheelIndices != null)
				{
					foreach (int num in this.vehicle.asset.replicatedWheelIndices)
					{
						Wheel wheelAtIndex = this.vehicle.GetWheelAtIndex(num);
						if (wheelAtIndex == null)
						{
							UnturnedLog.error(string.Format("Missing wheel for replicated index: {0}", num));
							SystemNetPakWriterEx.WriteUnsignedNormalizedFloat(writer, 0f, 4);
							writer.WritePhysicsMaterialNetId(PhysicsMaterialNetId.NULL);
						}
						else
						{
							SystemNetPakWriterEx.WriteUnsignedNormalizedFloat(writer, wheelAtIndex.replicatedSuspensionState, 4);
							writer.WritePhysicsMaterialNetId(wheelAtIndex.replicatedGroundMaterial);
						}
					}
				}
				if (this.vehicle.asset.UsesEngineRpmAndGears)
				{
					uint num2 = (uint)(this.vehicle.GearNumber + 1);
					writer.WriteBits(num2, 3);
					float num3 = Mathf.InverseLerp(this.vehicle.asset.EngineIdleRpm, this.vehicle.asset.EngineMaxRpm, this.vehicle.ReplicatedEngineRpm);
					SystemNetPakWriterEx.WriteUnsignedNormalizedFloat(writer, num3, 7);
				}
			}
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x000E5000 File Offset: 0x000E3200
		public DrivingPlayerInputPacket(InteractableVehicle vehicle)
		{
			this.vehicle = vehicle;
		}

		// Token: 0x04001CEF RID: 7407
		public Vector3 position;

		// Token: 0x04001CF0 RID: 7408
		public Quaternion rotation;

		// Token: 0x04001CF1 RID: 7409
		public float speed;

		// Token: 0x04001CF2 RID: 7410
		public float forwardVelocity;

		// Token: 0x04001CF3 RID: 7411
		public float steeringInput;

		// Token: 0x04001CF4 RID: 7412
		public float velocityInput;

		// Token: 0x04001CF5 RID: 7413
		internal InteractableVehicle vehicle;
	}
}
