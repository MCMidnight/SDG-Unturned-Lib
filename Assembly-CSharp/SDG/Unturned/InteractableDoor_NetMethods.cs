using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001DA RID: 474
	[NetInvokableGeneratedClass(typeof(InteractableDoor))]
	public static class InteractableDoor_NetMethods
	{
		// Token: 0x06000E6F RID: 3695 RVA: 0x00032300 File Offset: 0x00030500
		private static void ReceiveOpen_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableDoor interactableDoor = voidNetObj as InteractableDoor;
			if (interactableDoor == null)
			{
				return;
			}
			bool newOpen;
			context.reader.ReadBit(ref newOpen);
			interactableDoor.ReceiveOpen(newOpen);
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00032334 File Offset: 0x00030534
		[NetInvokableGeneratedMethod("ReceiveOpen", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveOpen_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableDoor_NetMethods.ReceiveOpen_DeferredRead));
				return;
			}
			InteractableDoor interactableDoor = obj as InteractableDoor;
			if (interactableDoor == null)
			{
				return;
			}
			bool newOpen;
			reader.ReadBit(ref newOpen);
			interactableDoor.ReceiveOpen(newOpen);
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00032393 File Offset: 0x00030593
		[NetInvokableGeneratedMethod("ReceiveOpen", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveOpen_Write(NetPakWriter writer, bool newOpen)
		{
			writer.WriteBit(newOpen);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x000323A0 File Offset: 0x000305A0
		[NetInvokableGeneratedMethod("ReceiveToggleRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveToggleRequest_Read(in ServerInvocationContext context)
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
			InteractableDoor interactableDoor = obj as InteractableDoor;
			if (interactableDoor == null)
			{
				return;
			}
			bool desiredOpen;
			reader.ReadBit(ref desiredOpen);
			interactableDoor.ReceiveToggleRequest(context, desiredOpen);
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x000323ED File Offset: 0x000305ED
		[NetInvokableGeneratedMethod("ReceiveToggleRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToggleRequest_Write(NetPakWriter writer, bool desiredOpen)
		{
			writer.WriteBit(desiredOpen);
		}
	}
}
