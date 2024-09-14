using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001DE RID: 478
	[NetInvokableGeneratedClass(typeof(InteractableLibrary))]
	public static class InteractableLibrary_NetMethods
	{
		// Token: 0x06000E85 RID: 3717 RVA: 0x00032768 File Offset: 0x00030968
		private static void ReceiveAmount_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableLibrary interactableLibrary = voidNetObj as InteractableLibrary;
			if (interactableLibrary == null)
			{
				return;
			}
			uint newAmount;
			SystemNetPakReaderEx.ReadUInt32(context.reader, ref newAmount);
			interactableLibrary.ReceiveAmount(newAmount);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0003279C File Offset: 0x0003099C
		[NetInvokableGeneratedMethod("ReceiveAmount", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAmount_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableLibrary_NetMethods.ReceiveAmount_DeferredRead));
				return;
			}
			InteractableLibrary interactableLibrary = obj as InteractableLibrary;
			if (interactableLibrary == null)
			{
				return;
			}
			uint newAmount;
			SystemNetPakReaderEx.ReadUInt32(reader, ref newAmount);
			interactableLibrary.ReceiveAmount(newAmount);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x000327FB File Offset: 0x000309FB
		[NetInvokableGeneratedMethod("ReceiveAmount", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAmount_Write(NetPakWriter writer, uint newAmount)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, newAmount);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00032808 File Offset: 0x00030A08
		[NetInvokableGeneratedMethod("ReceiveTransferLibraryRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTransferLibraryRequest_Read(in ServerInvocationContext context)
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
			InteractableLibrary interactableLibrary = obj as InteractableLibrary;
			if (interactableLibrary == null)
			{
				return;
			}
			byte transaction;
			SystemNetPakReaderEx.ReadUInt8(reader, ref transaction);
			uint delta;
			SystemNetPakReaderEx.ReadUInt32(reader, ref delta);
			interactableLibrary.ReceiveTransferLibraryRequest(context, transaction, delta);
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00032860 File Offset: 0x00030A60
		[NetInvokableGeneratedMethod("ReceiveTransferLibraryRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTransferLibraryRequest_Write(NetPakWriter writer, byte transaction, uint delta)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, transaction);
			SystemNetPakWriterEx.WriteUInt32(writer, delta);
		}
	}
}
