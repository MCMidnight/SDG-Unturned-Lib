using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001F2 RID: 498
	[NetInvokableGeneratedClass(typeof(ObjectManager))]
	public static class ObjectManager_NetMethods
	{
		// Token: 0x06000EF4 RID: 3828 RVA: 0x00033DFC File Offset: 0x00031FFC
		[NetInvokableGeneratedMethod("ReceiveObjectRubble", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveObjectRubble_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(reader, ref index);
			byte section;
			SystemNetPakReaderEx.ReadUInt8(reader, ref section);
			bool isAlive;
			reader.ReadBit(ref isAlive);
			Vector3 ragdoll;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref ragdoll, 13, 7);
			ObjectManager.ReceiveObjectRubble(x, y, index, section, isAlive, ragdoll);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00033E54 File Offset: 0x00032054
		[NetInvokableGeneratedMethod("ReceiveObjectRubble", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveObjectRubble_Write(NetPakWriter writer, byte x, byte y, ushort index, byte section, bool isAlive, Vector3 ragdoll)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt16(writer, index);
			SystemNetPakWriterEx.WriteUInt8(writer, section);
			writer.WriteBit(isAlive);
			UnityNetPakWriterEx.WriteClampedVector3(writer, ragdoll, 13, 7);
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00033E8C File Offset: 0x0003208C
		[NetInvokableGeneratedMethod("ReceiveTalkWithNpcRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTalkWithNpcRequest_Read(in ServerInvocationContext context)
		{
			NetId netId;
			context.reader.ReadNetId(out netId);
			ObjectManager.ReceiveTalkWithNpcRequest(context, netId);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00033EAE File Offset: 0x000320AE
		[NetInvokableGeneratedMethod("ReceiveTalkWithNpcRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTalkWithNpcRequest_Write(NetPakWriter writer, NetId netId)
		{
			writer.WriteNetId(netId);
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00033EB8 File Offset: 0x000320B8
		[NetInvokableGeneratedMethod("ReceiveUseObjectQuest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUseObjectQuest_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(reader, ref index);
			ObjectManager.ReceiveUseObjectQuest(context, x, y, index);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00033EEE File Offset: 0x000320EE
		[NetInvokableGeneratedMethod("ReceiveUseObjectQuest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUseObjectQuest_Write(NetPakWriter writer, byte x, byte y, ushort index)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt16(writer, index);
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00033F08 File Offset: 0x00032108
		[NetInvokableGeneratedMethod("ReceiveUseObjectDropper", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUseObjectDropper_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(reader, ref index);
			ObjectManager.ReceiveUseObjectDropper(context, x, y, index);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00033F3E File Offset: 0x0003213E
		[NetInvokableGeneratedMethod("ReceiveUseObjectDropper", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUseObjectDropper_Write(NetPakWriter writer, byte x, byte y, ushort index)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt16(writer, index);
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00033F58 File Offset: 0x00032158
		[NetInvokableGeneratedMethod("ReceiveObjectResourceState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveObjectResourceState_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(reader, ref index);
			ushort amount;
			SystemNetPakReaderEx.ReadUInt16(reader, ref amount);
			ObjectManager.ReceiveObjectResourceState(x, y, index, amount);
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00033F97 File Offset: 0x00032197
		[NetInvokableGeneratedMethod("ReceiveObjectResourceState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveObjectResourceState_Write(NetPakWriter writer, byte x, byte y, ushort index, ushort amount)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt16(writer, index);
			SystemNetPakWriterEx.WriteUInt16(writer, amount);
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00033FBC File Offset: 0x000321BC
		[NetInvokableGeneratedMethod("ReceiveObjectBinaryState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveObjectBinaryState_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(reader, ref index);
			bool isUsed;
			reader.ReadBit(ref isUsed);
			ObjectManager.ReceiveObjectBinaryState(x, y, index, isUsed);
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00033FFB File Offset: 0x000321FB
		[NetInvokableGeneratedMethod("ReceiveObjectBinaryState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveObjectBinaryState_Write(NetPakWriter writer, byte x, byte y, ushort index, bool isUsed)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt16(writer, index);
			writer.WriteBit(isUsed);
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00034020 File Offset: 0x00032220
		[NetInvokableGeneratedMethod("ReceiveToggleObjectBinaryStateRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveToggleObjectBinaryStateRequest_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(reader, ref index);
			bool isUsed;
			reader.ReadBit(ref isUsed);
			ObjectManager.ReceiveToggleObjectBinaryStateRequest(context, x, y, index, isUsed);
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00034060 File Offset: 0x00032260
		[NetInvokableGeneratedMethod("ReceiveToggleObjectBinaryStateRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToggleObjectBinaryStateRequest_Write(NetPakWriter writer, byte x, byte y, ushort index, bool isUsed)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt16(writer, index);
			writer.WriteBit(isUsed);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00034084 File Offset: 0x00032284
		[NetInvokableGeneratedMethod("ReceiveClearRegionObjects", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveClearRegionObjects_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ObjectManager.ReceiveClearRegionObjects(x, y);
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x000340AF File Offset: 0x000322AF
		[NetInvokableGeneratedMethod("ReceiveClearRegionObjects", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveClearRegionObjects_Write(NetPakWriter writer, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}
	}
}
