using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200020A RID: 522
	[NetInvokableGeneratedClass(typeof(UseableBarricade))]
	public static class UseableBarricade_NetMethods
	{
		// Token: 0x0600102E RID: 4142 RVA: 0x00038768 File Offset: 0x00036968
		[NetInvokableGeneratedMethod("ReceiveBarricadeVehicle", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBarricadeVehicle_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableBarricade useableBarricade = obj as UseableBarricade;
			if (useableBarricade == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableBarricade.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableBarricade));
				return;
			}
			Vector3 newPoint;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newPoint, 13, 11);
			float newAngle_X;
			SystemNetPakReaderEx.ReadFloat(reader, ref newAngle_X);
			float newAngle_Y;
			SystemNetPakReaderEx.ReadFloat(reader, ref newAngle_Y);
			float newAngle_Z;
			SystemNetPakReaderEx.ReadFloat(reader, ref newAngle_Z);
			NetId regionNetId;
			reader.ReadNetId(out regionNetId);
			useableBarricade.ReceiveBarricadeVehicle(context, newPoint, newAngle_X, newAngle_Y, newAngle_Z, regionNetId);
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x00038805 File Offset: 0x00036A05
		[NetInvokableGeneratedMethod("ReceiveBarricadeVehicle", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBarricadeVehicle_Write(NetPakWriter writer, Vector3 newPoint, float newAngle_X, float newAngle_Y, float newAngle_Z, NetId regionNetId)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, newPoint, 13, 11);
			SystemNetPakWriterEx.WriteFloat(writer, newAngle_X);
			SystemNetPakWriterEx.WriteFloat(writer, newAngle_Y);
			SystemNetPakWriterEx.WriteFloat(writer, newAngle_Z);
			writer.WriteNetId(regionNetId);
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00038838 File Offset: 0x00036A38
		[NetInvokableGeneratedMethod("ReceiveBarricadeNone", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBarricadeNone_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableBarricade useableBarricade = obj as UseableBarricade;
			if (useableBarricade == null)
			{
				return;
			}
			if (!context.IsOwnerOf(useableBarricade.channel))
			{
				context.Kick(string.Format("not owner of {0}", useableBarricade));
				return;
			}
			Vector3 newPoint;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newPoint, 13, 11);
			float newAngle_X;
			SystemNetPakReaderEx.ReadFloat(reader, ref newAngle_X);
			float newAngle_Y;
			SystemNetPakReaderEx.ReadFloat(reader, ref newAngle_Y);
			float newAngle_Z;
			SystemNetPakReaderEx.ReadFloat(reader, ref newAngle_Z);
			useableBarricade.ReceiveBarricadeNone(newPoint, newAngle_X, newAngle_Y, newAngle_Z);
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x000388C9 File Offset: 0x00036AC9
		[NetInvokableGeneratedMethod("ReceiveBarricadeNone", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBarricadeNone_Write(NetPakWriter writer, Vector3 newPoint, float newAngle_X, float newAngle_Y, float newAngle_Z)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, newPoint, 13, 11);
			SystemNetPakWriterEx.WriteFloat(writer, newAngle_X);
			SystemNetPakWriterEx.WriteFloat(writer, newAngle_Y);
			SystemNetPakWriterEx.WriteFloat(writer, newAngle_Z);
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x000388F0 File Offset: 0x00036AF0
		[NetInvokableGeneratedMethod("ReceivePlayBuild", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlayBuild_Read(in ClientInvocationContext context)
		{
			NetId key;
			if (!context.reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			UseableBarricade useableBarricade = obj as UseableBarricade;
			if (useableBarricade == null)
			{
				return;
			}
			useableBarricade.ReceivePlayBuild();
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0003892F File Offset: 0x00036B2F
		[NetInvokableGeneratedMethod("ReceivePlayBuild", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlayBuild_Write(NetPakWriter writer)
		{
		}
	}
}
