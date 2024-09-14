using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001EB RID: 491
	[NetInvokableGeneratedClass(typeof(InteractableTank))]
	public static class InteractableTank_NetMethods
	{
		// Token: 0x06000ECD RID: 3789 RVA: 0x0003367C File Offset: 0x0003187C
		private static void ReceiveAmount_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableTank interactableTank = voidNetObj as InteractableTank;
			if (interactableTank == null)
			{
				return;
			}
			ushort newAmount;
			SystemNetPakReaderEx.ReadUInt16(context.reader, ref newAmount);
			interactableTank.ReceiveAmount(newAmount);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x000336B0 File Offset: 0x000318B0
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableTank_NetMethods.ReceiveAmount_DeferredRead));
				return;
			}
			InteractableTank interactableTank = obj as InteractableTank;
			if (interactableTank == null)
			{
				return;
			}
			ushort newAmount;
			SystemNetPakReaderEx.ReadUInt16(reader, ref newAmount);
			interactableTank.ReceiveAmount(newAmount);
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0003370F File Offset: 0x0003190F
		[NetInvokableGeneratedMethod("ReceiveAmount", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAmount_Write(NetPakWriter writer, ushort newAmount)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, newAmount);
		}
	}
}
