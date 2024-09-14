using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001DC RID: 476
	[NetInvokableGeneratedClass(typeof(InteractableFire))]
	public static class InteractableFire_NetMethods
	{
		// Token: 0x06000E78 RID: 3704 RVA: 0x000324D8 File Offset: 0x000306D8
		private static void ReceiveLit_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableFire interactableFire = voidNetObj as InteractableFire;
			if (interactableFire == null)
			{
				return;
			}
			bool newLit;
			context.reader.ReadBit(ref newLit);
			interactableFire.ReceiveLit(newLit);
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0003250C File Offset: 0x0003070C
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableFire_NetMethods.ReceiveLit_DeferredRead));
				return;
			}
			InteractableFire interactableFire = obj as InteractableFire;
			if (interactableFire == null)
			{
				return;
			}
			bool newLit;
			reader.ReadBit(ref newLit);
			interactableFire.ReceiveLit(newLit);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0003256B File Offset: 0x0003076B
		[NetInvokableGeneratedMethod("ReceiveLit", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLit_Write(NetPakWriter writer, bool newLit)
		{
			writer.WriteBit(newLit);
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00032578 File Offset: 0x00030778
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
			InteractableFire interactableFire = obj as InteractableFire;
			if (interactableFire == null)
			{
				return;
			}
			bool desiredLit;
			reader.ReadBit(ref desiredLit);
			interactableFire.ReceiveToggleRequest(context, desiredLit);
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x000325C5 File Offset: 0x000307C5
		[NetInvokableGeneratedMethod("ReceiveToggleRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToggleRequest_Write(NetPakWriter writer, bool desiredLit)
		{
			writer.WriteBit(desiredLit);
		}
	}
}
