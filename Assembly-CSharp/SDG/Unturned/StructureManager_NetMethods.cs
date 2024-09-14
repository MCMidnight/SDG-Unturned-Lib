using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000207 RID: 519
	[NetInvokableGeneratedClass(typeof(StructureManager))]
	public static class StructureManager_NetMethods
	{
		// Token: 0x06001024 RID: 4132 RVA: 0x00038578 File Offset: 0x00036778
		[NetInvokableGeneratedMethod("ReceiveDestroyStructure", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDestroyStructure_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId netId;
			reader.ReadNetId(out netId);
			Vector3 ragdoll;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref ragdoll, 13, 7);
			bool wasPickedUp;
			reader.ReadBit(ref wasPickedUp);
			StructureManager.ReceiveDestroyStructure(context, netId, ragdoll, wasPickedUp);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x000385B1 File Offset: 0x000367B1
		[NetInvokableGeneratedMethod("ReceiveDestroyStructure", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDestroyStructure_Write(NetPakWriter writer, NetId netId, Vector3 ragdoll, bool wasPickedUp)
		{
			writer.WriteNetId(netId);
			UnityNetPakWriterEx.WriteClampedVector3(writer, ragdoll, 13, 7);
			writer.WriteBit(wasPickedUp);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x000385D0 File Offset: 0x000367D0
		[NetInvokableGeneratedMethod("ReceiveClearRegionStructures", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveClearRegionStructures_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			StructureManager.ReceiveClearRegionStructures(x, y);
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x000385FB File Offset: 0x000367FB
		[NetInvokableGeneratedMethod("ReceiveClearRegionStructures", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveClearRegionStructures_Write(NetPakWriter writer, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00038610 File Offset: 0x00036810
		[NetInvokableGeneratedMethod("ReceiveSingleStructure", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSingleStructure_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			Guid id;
			SystemNetPakReaderEx.ReadGuid(reader, ref id);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 11);
			Quaternion rotation;
			UnityNetPakReaderEx.ReadSpecialYawOrQuaternion(reader, ref rotation, 23, 9);
			ulong owner;
			SystemNetPakReaderEx.ReadUInt64(reader, ref owner);
			ulong group;
			SystemNetPakReaderEx.ReadUInt64(reader, ref group);
			NetId netId;
			reader.ReadNetId(out netId);
			StructureManager.ReceiveSingleStructure(x, y, id, point, rotation, owner, group, netId);
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00038684 File Offset: 0x00036884
		[NetInvokableGeneratedMethod("ReceiveSingleStructure", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSingleStructure_Write(NetPakWriter writer, byte x, byte y, Guid id, Vector3 point, Quaternion rotation, ulong owner, ulong group, NetId netId)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteGuid(writer, id);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 11);
			UnityNetPakWriterEx.WriteSpecialYawOrQuaternion(writer, rotation, 23, 9);
			SystemNetPakWriterEx.WriteUInt64(writer, owner);
			SystemNetPakWriterEx.WriteUInt64(writer, group);
			writer.WriteNetId(netId);
		}
	}
}
