using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000204 RID: 516
	[NetInvokableGeneratedClass(typeof(ResourceManager))]
	public static class ResourceManager_NetMethods
	{
		// Token: 0x0600100E RID: 4110 RVA: 0x00038104 File Offset: 0x00036304
		[NetInvokableGeneratedMethod("ReceiveClearRegionResources", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveClearRegionResources_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ResourceManager.ReceiveClearRegionResources(x, y);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0003812F File Offset: 0x0003632F
		[NetInvokableGeneratedMethod("ReceiveClearRegionResources", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveClearRegionResources_Write(NetPakWriter writer, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00038144 File Offset: 0x00036344
		[NetInvokableGeneratedMethod("ReceiveForageRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveForageRequest_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(reader, ref index);
			ResourceManager.ReceiveForageRequest(context, x, y, index);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0003817A File Offset: 0x0003637A
		[NetInvokableGeneratedMethod("ReceiveForageRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveForageRequest_Write(NetPakWriter writer, byte x, byte y, ushort index)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt16(writer, index);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00038194 File Offset: 0x00036394
		[NetInvokableGeneratedMethod("ReceiveResourceDead", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveResourceDead_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(reader, ref index);
			Vector3 ragdoll;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref ragdoll, 13, 7);
			ResourceManager.ReceiveResourceDead(x, y, index, ragdoll);
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x000381D6 File Offset: 0x000363D6
		[NetInvokableGeneratedMethod("ReceiveResourceDead", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveResourceDead_Write(NetPakWriter writer, byte x, byte y, ushort index, Vector3 ragdoll)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt16(writer, index);
			UnityNetPakWriterEx.WriteClampedVector3(writer, ragdoll, 13, 7);
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x000381FC File Offset: 0x000363FC
		[NetInvokableGeneratedMethod("ReceiveResourceAlive", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveResourceAlive_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ushort index;
			SystemNetPakReaderEx.ReadUInt16(reader, ref index);
			ResourceManager.ReceiveResourceAlive(x, y, index);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00038231 File Offset: 0x00036431
		[NetInvokableGeneratedMethod("ReceiveResourceAlive", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveResourceAlive_Write(NetPakWriter writer, byte x, byte y, ushort index)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt16(writer, index);
		}
	}
}
