using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001ED RID: 493
	[NetInvokableGeneratedClass(typeof(ItemManager))]
	public static class ItemManager_NetMethods
	{
		// Token: 0x06000ED2 RID: 3794 RVA: 0x00033774 File Offset: 0x00031974
		[NetInvokableGeneratedMethod("ReceiveDestroyItem", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDestroyItem_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			bool shouldPlayEffect;
			reader.ReadBit(ref shouldPlayEffect);
			ItemManager.ReceiveDestroyItem(x, y, instanceID, shouldPlayEffect);
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x000337B3 File Offset: 0x000319B3
		[NetInvokableGeneratedMethod("ReceiveDestroyItem", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDestroyItem_Write(NetPakWriter writer, byte x, byte y, uint instanceID, bool shouldPlayEffect)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			writer.WriteBit(shouldPlayEffect);
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x000337D8 File Offset: 0x000319D8
		[NetInvokableGeneratedMethod("ReceiveTakeItemRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTakeItemRequest_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			byte to_x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref to_x);
			byte to_y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref to_y);
			byte to_rot;
			SystemNetPakReaderEx.ReadUInt8(reader, ref to_rot);
			byte to_page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref to_page);
			ItemManager.ReceiveTakeItemRequest(context, x, y, instanceID, to_x, to_y, to_rot, to_page);
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00033839 File Offset: 0x00031A39
		[NetInvokableGeneratedMethod("ReceiveTakeItemRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTakeItemRequest_Write(NetPakWriter writer, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			SystemNetPakWriterEx.WriteUInt8(writer, to_x);
			SystemNetPakWriterEx.WriteUInt8(writer, to_y);
			SystemNetPakWriterEx.WriteUInt8(writer, to_rot);
			SystemNetPakWriterEx.WriteUInt8(writer, to_page);
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00033878 File Offset: 0x00031A78
		[NetInvokableGeneratedMethod("ReceiveClearRegionItems", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveClearRegionItems_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ItemManager.ReceiveClearRegionItems(x, y);
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x000338A3 File Offset: 0x00031AA3
		[NetInvokableGeneratedMethod("ReceiveClearRegionItems", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveClearRegionItems_Write(NetPakWriter writer, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x000338B8 File Offset: 0x00031AB8
		[NetInvokableGeneratedMethod("ReceiveItem", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveItem_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			byte amount;
			SystemNetPakReaderEx.ReadUInt8(reader, ref amount);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 7);
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			bool shouldPlayEffect;
			reader.ReadBit(ref shouldPlayEffect);
			ItemManager.ReceiveItem(x, y, id, amount, quality, array, point, instanceID, shouldPlayEffect);
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00033944 File Offset: 0x00031B44
		[NetInvokableGeneratedMethod("ReceiveItem", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveItem_Write(NetPakWriter writer, byte x, byte y, ushort id, byte amount, byte quality, byte[] state, Vector3 point, uint instanceID, bool shouldPlayEffect)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, amount);
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 7);
			SystemNetPakWriterEx.WriteUInt32(writer, instanceID);
			writer.WriteBit(shouldPlayEffect);
		}
	}
}
