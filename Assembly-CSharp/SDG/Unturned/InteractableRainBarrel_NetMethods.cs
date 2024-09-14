using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001E4 RID: 484
	[NetInvokableGeneratedClass(typeof(InteractableRainBarrel))]
	public static class InteractableRainBarrel_NetMethods
	{
		// Token: 0x06000EA3 RID: 3747 RVA: 0x00032D5C File Offset: 0x00030F5C
		private static void ReceiveFull_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableRainBarrel interactableRainBarrel = voidNetObj as InteractableRainBarrel;
			if (interactableRainBarrel == null)
			{
				return;
			}
			bool newFull;
			context.reader.ReadBit(ref newFull);
			interactableRainBarrel.ReceiveFull(newFull);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00032D90 File Offset: 0x00030F90
		[NetInvokableGeneratedMethod("ReceiveFull", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveFull_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableRainBarrel_NetMethods.ReceiveFull_DeferredRead));
				return;
			}
			InteractableRainBarrel interactableRainBarrel = obj as InteractableRainBarrel;
			if (interactableRainBarrel == null)
			{
				return;
			}
			bool newFull;
			reader.ReadBit(ref newFull);
			interactableRainBarrel.ReceiveFull(newFull);
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00032DEF File Offset: 0x00030FEF
		[NetInvokableGeneratedMethod("ReceiveFull", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveFull_Write(NetPakWriter writer, bool newFull)
		{
			writer.WriteBit(newFull);
		}
	}
}
