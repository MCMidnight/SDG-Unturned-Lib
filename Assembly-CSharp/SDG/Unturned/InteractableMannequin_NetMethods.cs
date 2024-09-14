using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001DF RID: 479
	[NetInvokableGeneratedClass(typeof(InteractableMannequin))]
	public static class InteractableMannequin_NetMethods
	{
		// Token: 0x06000E8A RID: 3722 RVA: 0x00032874 File Offset: 0x00030A74
		private static void ReceivePose_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableMannequin interactableMannequin = voidNetObj as InteractableMannequin;
			if (interactableMannequin == null)
			{
				return;
			}
			byte poseComp;
			SystemNetPakReaderEx.ReadUInt8(context.reader, ref poseComp);
			interactableMannequin.ReceivePose(poseComp);
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x000328A8 File Offset: 0x00030AA8
		[NetInvokableGeneratedMethod("ReceivePose", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePose_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableMannequin_NetMethods.ReceivePose_DeferredRead));
				return;
			}
			InteractableMannequin interactableMannequin = obj as InteractableMannequin;
			if (interactableMannequin == null)
			{
				return;
			}
			byte poseComp;
			SystemNetPakReaderEx.ReadUInt8(reader, ref poseComp);
			interactableMannequin.ReceivePose(poseComp);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00032907 File Offset: 0x00030B07
		[NetInvokableGeneratedMethod("ReceivePose", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePose_Write(NetPakWriter writer, byte poseComp)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, poseComp);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00032914 File Offset: 0x00030B14
		[NetInvokableGeneratedMethod("ReceivePoseRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePoseRequest_Read(in ServerInvocationContext context)
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
			InteractableMannequin interactableMannequin = obj as InteractableMannequin;
			if (interactableMannequin == null)
			{
				return;
			}
			byte poseComp;
			SystemNetPakReaderEx.ReadUInt8(reader, ref poseComp);
			interactableMannequin.ReceivePoseRequest(context, poseComp);
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00032961 File Offset: 0x00030B61
		[NetInvokableGeneratedMethod("ReceivePoseRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePoseRequest_Write(NetPakWriter writer, byte poseComp)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, poseComp);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0003296C File Offset: 0x00030B6C
		private static void ReceiveUpdate_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableMannequin interactableMannequin = voidNetObj as InteractableMannequin;
			if (interactableMannequin == null)
			{
				return;
			}
			NetPakReader reader = context.reader;
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			interactableMannequin.ReceiveUpdate(array);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x000329B0 File Offset: 0x00030BB0
		[NetInvokableGeneratedMethod("ReceiveUpdate", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUpdate_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableMannequin_NetMethods.ReceiveUpdate_DeferredRead));
				return;
			}
			InteractableMannequin interactableMannequin = obj as InteractableMannequin;
			if (interactableMannequin == null)
			{
				return;
			}
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			interactableMannequin.ReceiveUpdate(array);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00032A24 File Offset: 0x00030C24
		[NetInvokableGeneratedMethod("ReceiveUpdate", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUpdate_Write(NetPakWriter writer, byte[] state)
		{
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00032A48 File Offset: 0x00030C48
		[NetInvokableGeneratedMethod("ReceiveUpdateRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUpdateRequest_Read(in ServerInvocationContext context)
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
			InteractableMannequin interactableMannequin = obj as InteractableMannequin;
			if (interactableMannequin == null)
			{
				return;
			}
			EMannequinUpdateMode updateMode;
			reader.ReadEnum(out updateMode);
			interactableMannequin.ReceiveUpdateRequest(context, updateMode);
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00032A95 File Offset: 0x00030C95
		[NetInvokableGeneratedMethod("ReceiveUpdateRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUpdateRequest_Write(NetPakWriter writer, EMannequinUpdateMode updateMode)
		{
			writer.WriteEnum(updateMode);
		}
	}
}
