using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000206 RID: 518
	[NetInvokableGeneratedClass(typeof(StructureDrop))]
	public static class StructureDrop_NetMethods
	{
		// Token: 0x06001018 RID: 4120 RVA: 0x0003827C File Offset: 0x0003647C
		private static void ReceiveHealth_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			StructureDrop structureDrop = voidNetObj as StructureDrop;
			if (structureDrop == null)
			{
				return;
			}
			byte hp;
			SystemNetPakReaderEx.ReadUInt8(context.reader, ref hp);
			structureDrop.ReceiveHealth(hp);
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x000382AC File Offset: 0x000364AC
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(StructureDrop_NetMethods.ReceiveHealth_DeferredRead));
				return;
			}
			StructureDrop structureDrop = obj as StructureDrop;
			if (structureDrop == null)
			{
				return;
			}
			byte hp;
			SystemNetPakReaderEx.ReadUInt8(reader, ref hp);
			structureDrop.ReceiveHealth(hp);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00038305 File Offset: 0x00036505
		[NetInvokableGeneratedMethod("ReceiveHealth", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveHealth_Write(NetPakWriter writer, byte hp)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, hp);
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00038310 File Offset: 0x00036510
		private static void ReceiveTransform_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			StructureDrop structureDrop = voidNetObj as StructureDrop;
			if (structureDrop == null)
			{
				return;
			}
			NetPakReader reader = context.reader;
			byte old_x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref old_x);
			byte old_y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref old_y);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 11);
			Quaternion rotation;
			UnityNetPakReaderEx.ReadSpecialYawOrQuaternion(reader, ref rotation, 23, 9);
			structureDrop.ReceiveTransform(context, old_x, old_y, point, rotation);
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00038368 File Offset: 0x00036568
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(StructureDrop_NetMethods.ReceiveTransform_DeferredRead));
				return;
			}
			StructureDrop structureDrop = obj as StructureDrop;
			if (structureDrop == null)
			{
				return;
			}
			byte old_x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref old_x);
			byte old_y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref old_y);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 11);
			Quaternion rotation;
			UnityNetPakReaderEx.ReadSpecialYawOrQuaternion(reader, ref rotation, 23, 9);
			structureDrop.ReceiveTransform(context, old_x, old_y, point, rotation);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x000383EB File Offset: 0x000365EB
		[NetInvokableGeneratedMethod("ReceiveTransform", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTransform_Write(NetPakWriter writer, byte old_x, byte old_y, Vector3 point, Quaternion rotation)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, old_x);
			SystemNetPakWriterEx.WriteUInt8(writer, old_y);
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 11);
			UnityNetPakWriterEx.WriteSpecialYawOrQuaternion(writer, rotation, 23, 9);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00038418 File Offset: 0x00036618
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
			StructureDrop structureDrop = obj as StructureDrop;
			if (structureDrop == null)
			{
				return;
			}
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 11);
			Quaternion rotation;
			UnityNetPakReaderEx.ReadSpecialYawOrQuaternion(reader, ref rotation, 23, 9);
			structureDrop.ReceiveTransformRequest(context, point, rotation);
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00038472 File Offset: 0x00036672
		[NetInvokableGeneratedMethod("ReceiveTransformRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTransformRequest_Write(NetPakWriter writer, Vector3 point, Quaternion rotation)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, point, 13, 11);
			UnityNetPakWriterEx.WriteSpecialYawOrQuaternion(writer, rotation, 23, 9);
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0003848C File Offset: 0x0003668C
		private static void ReceiveOwnerAndGroup_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			StructureDrop structureDrop = voidNetObj as StructureDrop;
			if (structureDrop == null)
			{
				return;
			}
			NetPakReader reader = context.reader;
			ulong newOwner;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newOwner);
			ulong newGroup;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newGroup);
			structureDrop.ReceiveOwnerAndGroup(newOwner, newGroup);
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x000384C4 File Offset: 0x000366C4
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(StructureDrop_NetMethods.ReceiveOwnerAndGroup_DeferredRead));
				return;
			}
			StructureDrop structureDrop = obj as StructureDrop;
			if (structureDrop == null)
			{
				return;
			}
			ulong newOwner;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newOwner);
			ulong newGroup;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newGroup);
			structureDrop.ReceiveOwnerAndGroup(newOwner, newGroup);
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00038528 File Offset: 0x00036728
		[NetInvokableGeneratedMethod("ReceiveOwnerAndGroup", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveOwnerAndGroup_Write(NetPakWriter writer, ulong newOwner, ulong newGroup)
		{
			SystemNetPakWriterEx.WriteUInt64(writer, newOwner);
			SystemNetPakWriterEx.WriteUInt64(writer, newGroup);
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0003853C File Offset: 0x0003673C
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
			StructureDrop structureDrop = obj as StructureDrop;
			if (structureDrop == null)
			{
				return;
			}
			structureDrop.ReceiveSalvageRequest(context);
		}
	}
}
