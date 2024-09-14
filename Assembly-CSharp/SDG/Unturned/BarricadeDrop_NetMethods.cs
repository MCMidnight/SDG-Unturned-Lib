using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001D3 RID: 467
	[NetInvokableGeneratedClass(typeof(BarricadeDrop))]
	public static class BarricadeDrop_NetMethods
	{
		// Token: 0x06000E14 RID: 3604 RVA: 0x000311F4 File Offset: 0x0002F3F4
		private static void ReceiveHealth_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			BarricadeDrop barricadeDrop = voidNetObj as BarricadeDrop;
			if (barricadeDrop == null)
			{
				return;
			}
			byte hp;
			SystemNetPakReaderEx.ReadUInt8(context.reader, ref hp);
			barricadeDrop.ReceiveHealth(hp);
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00031224 File Offset: 0x0002F424
		[NetInvokableGeneratedMethod("ReceiveHealth", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveHealth_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(BarricadeDrop_NetMethods.ReceiveHealth_DeferredRead));
				return;
			}
			BarricadeDrop barricadeDrop = obj as BarricadeDrop;
			if (barricadeDrop == null)
			{
				return;
			}
			byte hp;
			SystemNetPakReaderEx.ReadUInt8(reader, ref hp);
			barricadeDrop.ReceiveHealth(hp);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0003127D File Offset: 0x0002F47D
		[NetInvokableGeneratedMethod("ReceiveHealth", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveHealth_Write(NetPakWriter writer, byte hp)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, hp);
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00031288 File Offset: 0x0002F488
		private static void ReceiveTransform_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			BarricadeDrop barricadeDrop = voidNetObj as BarricadeDrop;
			if (barricadeDrop == null)
			{
				return;
			}
			NetPakReader reader = context.reader;
			byte old_x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref old_x);
			byte old_y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref old_y);
			ushort oldPlant;
			SystemNetPakReaderEx.ReadUInt16(reader, ref oldPlant);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 11);
			Quaternion rotation;
			UnityNetPakReaderEx.ReadQuaternion(reader, ref rotation, 9);
			barricadeDrop.ReceiveTransform(context, old_x, old_y, oldPlant, point, rotation);
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x000312E8 File Offset: 0x0002F4E8
		[NetInvokableGeneratedMethod("ReceiveTransform", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTransform_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(BarricadeDrop_NetMethods.ReceiveTransform_DeferredRead));
				return;
			}
			BarricadeDrop barricadeDrop = obj as BarricadeDrop;
			if (barricadeDrop == null)
			{
				return;
			}
			byte old_x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref old_x);
			byte old_y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref old_y);
			ushort oldPlant;
			SystemNetPakReaderEx.ReadUInt16(reader, ref oldPlant);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 11);
			Quaternion rotation;
			UnityNetPakReaderEx.ReadQuaternion(reader, ref rotation, 9);
			barricadeDrop.ReceiveTransform(context, old_x, old_y, oldPlant, point, rotation);
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00031374 File Offset: 0x0002F574
		[NetInvokableGeneratedMethod("ReceiveTransform", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTransform_Write(NetPakWriter writer, byte old_x, byte old_y, ushort oldPlant, Vector3 point, Quaternion rotation)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, old_x);
			SystemNetPakWriterEx.WriteUInt8(writer, old_y);
			SystemNetPakWriterEx.WriteUInt16(writer, oldPlant);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 11);
			UnityNetPakWriterEx.WriteQuaternion(writer, rotation, 9);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x000313A8 File Offset: 0x0002F5A8
		[NetInvokableGeneratedMethod("ReceiveTransformRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTransformRequest_Read(in ServerInvocationContext context)
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
			BarricadeDrop barricadeDrop = obj as BarricadeDrop;
			if (barricadeDrop == null)
			{
				return;
			}
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 11);
			Quaternion rotation;
			UnityNetPakReaderEx.ReadQuaternion(reader, ref rotation, 9);
			barricadeDrop.ReceiveTransformRequest(context, point, rotation);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00031400 File Offset: 0x0002F600
		[NetInvokableGeneratedMethod("ReceiveTransformRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTransformRequest_Write(NetPakWriter writer, Vector3 point, Quaternion rotation)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 11);
			UnityNetPakWriterEx.WriteQuaternion(writer, rotation, 9);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00031418 File Offset: 0x0002F618
		private static void ReceiveOwnerAndGroup_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			BarricadeDrop barricadeDrop = voidNetObj as BarricadeDrop;
			if (barricadeDrop == null)
			{
				return;
			}
			NetPakReader reader = context.reader;
			ulong newOwner;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newOwner);
			ulong newGroup;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newGroup);
			barricadeDrop.ReceiveOwnerAndGroup(newOwner, newGroup);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00031450 File Offset: 0x0002F650
		[NetInvokableGeneratedMethod("ReceiveOwnerAndGroup", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveOwnerAndGroup_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(BarricadeDrop_NetMethods.ReceiveOwnerAndGroup_DeferredRead));
				return;
			}
			BarricadeDrop barricadeDrop = obj as BarricadeDrop;
			if (barricadeDrop == null)
			{
				return;
			}
			ulong newOwner;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newOwner);
			ulong newGroup;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newGroup);
			barricadeDrop.ReceiveOwnerAndGroup(newOwner, newGroup);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x000314B4 File Offset: 0x0002F6B4
		[NetInvokableGeneratedMethod("ReceiveOwnerAndGroup", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveOwnerAndGroup_Write(NetPakWriter writer, ulong newOwner, ulong newGroup)
		{
			SystemNetPakWriterEx.WriteUInt64(writer, newOwner);
			SystemNetPakWriterEx.WriteUInt64(writer, newGroup);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x000314C8 File Offset: 0x0002F6C8
		private static void ReceiveUpdateState_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			BarricadeDrop barricadeDrop = voidNetObj as BarricadeDrop;
			if (barricadeDrop == null)
			{
				return;
			}
			NetPakReader reader = context.reader;
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			barricadeDrop.ReceiveUpdateState(array);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00031504 File Offset: 0x0002F704
		[NetInvokableGeneratedMethod("ReceiveUpdateState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUpdateState_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(BarricadeDrop_NetMethods.ReceiveUpdateState_DeferredRead));
				return;
			}
			BarricadeDrop barricadeDrop = obj as BarricadeDrop;
			if (barricadeDrop == null)
			{
				return;
			}
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			barricadeDrop.ReceiveUpdateState(array);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00031570 File Offset: 0x0002F770
		[NetInvokableGeneratedMethod("ReceiveUpdateState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUpdateState_Write(NetPakWriter writer, byte[] newState)
		{
			byte b = (byte)newState.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(newState, (int)b);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00031594 File Offset: 0x0002F794
		[NetInvokableGeneratedMethod("ReceiveSalvageRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSalvageRequest_Read(in ServerInvocationContext context)
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
			BarricadeDrop barricadeDrop = obj as BarricadeDrop;
			if (barricadeDrop == null)
			{
				return;
			}
			barricadeDrop.ReceiveSalvageRequest(context);
		}
	}
}
