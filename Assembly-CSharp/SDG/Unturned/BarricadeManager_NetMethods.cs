using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001D4 RID: 468
	[NetInvokableGeneratedClass(typeof(BarricadeManager))]
	public static class BarricadeManager_NetMethods
	{
		// Token: 0x06000E23 RID: 3619 RVA: 0x000315D0 File Offset: 0x0002F7D0
		[NetInvokableGeneratedMethod("ReceiveDestroyBarricade", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDestroyBarricade_Read(in ClientInvocationContext context)
		{
			NetId netId;
			context.reader.ReadNetId(out netId);
			BarricadeManager.ReceiveDestroyBarricade(context, netId);
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x000315F2 File Offset: 0x0002F7F2
		[NetInvokableGeneratedMethod("ReceiveDestroyBarricade", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDestroyBarricade_Write(NetPakWriter writer, NetId netId)
		{
			writer.WriteNetId(netId);
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x000315FC File Offset: 0x0002F7FC
		[NetInvokableGeneratedMethod("ReceiveClearRegionBarricades", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveClearRegionBarricades_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			BarricadeManager.ReceiveClearRegionBarricades(x, y);
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00031627 File Offset: 0x0002F827
		[NetInvokableGeneratedMethod("ReceiveClearRegionBarricades", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveClearRegionBarricades_Write(NetPakWriter writer, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0003163C File Offset: 0x0002F83C
		[NetInvokableGeneratedMethod("ReceiveSingleBarricade", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSingleBarricade_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId parentNetId;
			reader.ReadNetId(out parentNetId);
			Guid assetId;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetId);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 11);
			Quaternion rotation;
			UnityNetPakReaderEx.ReadQuaternion(reader, ref rotation, 9);
			ulong owner;
			SystemNetPakReaderEx.ReadUInt64(reader, ref owner);
			ulong group;
			SystemNetPakReaderEx.ReadUInt64(reader, ref group);
			NetId netId;
			reader.ReadNetId(out netId);
			BarricadeManager.ReceiveSingleBarricade(context, parentNetId, assetId, array, point, rotation, owner, group, netId);
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x000316C0 File Offset: 0x0002F8C0
		[NetInvokableGeneratedMethod("ReceiveSingleBarricade", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSingleBarricade_Write(NetPakWriter writer, NetId parentNetId, Guid assetId, byte[] state, Vector3 point, Quaternion rotation, ulong owner, ulong group, NetId netId)
		{
			writer.WriteNetId(parentNetId);
			SystemNetPakWriterEx.WriteGuid(writer, assetId);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 11);
			UnityNetPakWriterEx.WriteQuaternion(writer, rotation, 9);
			SystemNetPakWriterEx.WriteUInt64(writer, owner);
			SystemNetPakWriterEx.WriteUInt64(writer, group);
			writer.WriteNetId(netId);
		}
	}
}
