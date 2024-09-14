using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001E2 RID: 482
	[NetInvokableGeneratedClass(typeof(InteractableOven))]
	public static class InteractableOven_NetMethods
	{
		// Token: 0x06000E99 RID: 3737 RVA: 0x00032B6C File Offset: 0x00030D6C
		private static void ReceiveLit_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableOven interactableOven = voidNetObj as InteractableOven;
			if (interactableOven == null)
			{
				return;
			}
			bool newLit;
			context.reader.ReadBit(ref newLit);
			interactableOven.ReceiveLit(newLit);
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00032BA0 File Offset: 0x00030DA0
		[NetInvokableGeneratedMethod("ReceiveLit", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLit_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableOven_NetMethods.ReceiveLit_DeferredRead));
				return;
			}
			InteractableOven interactableOven = obj as InteractableOven;
			if (interactableOven == null)
			{
				return;
			}
			bool newLit;
			reader.ReadBit(ref newLit);
			interactableOven.ReceiveLit(newLit);
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00032BFF File Offset: 0x00030DFF
		[NetInvokableGeneratedMethod("ReceiveLit", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLit_Write(NetPakWriter writer, bool newLit)
		{
			writer.WriteBit(newLit);
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00032C0C File Offset: 0x00030E0C
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
			InteractableOven interactableOven = obj as InteractableOven;
			if (interactableOven == null)
			{
				return;
			}
			bool desiredLit;
			reader.ReadBit(ref desiredLit);
			interactableOven.ReceiveToggleRequest(context, desiredLit);
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00032C59 File Offset: 0x00030E59
		[NetInvokableGeneratedMethod("ReceiveToggleRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToggleRequest_Write(NetPakWriter writer, bool desiredLit)
		{
			writer.WriteBit(desiredLit);
		}
	}
}
