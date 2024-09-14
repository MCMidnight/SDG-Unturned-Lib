using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001E8 RID: 488
	[NetInvokableGeneratedClass(typeof(InteractableSpot))]
	public static class InteractableSpot_NetMethods
	{
		// Token: 0x06000EB4 RID: 3764 RVA: 0x000330A8 File Offset: 0x000312A8
		private static void ReceivePowered_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableSpot interactableSpot = voidNetObj as InteractableSpot;
			if (interactableSpot == null)
			{
				return;
			}
			bool newPowered;
			context.reader.ReadBit(ref newPowered);
			interactableSpot.ReceivePowered(newPowered);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x000330DC File Offset: 0x000312DC
		[NetInvokableGeneratedMethod("ReceivePowered", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePowered_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableSpot_NetMethods.ReceivePowered_DeferredRead));
				return;
			}
			InteractableSpot interactableSpot = obj as InteractableSpot;
			if (interactableSpot == null)
			{
				return;
			}
			bool newPowered;
			reader.ReadBit(ref newPowered);
			interactableSpot.ReceivePowered(newPowered);
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0003313B File Offset: 0x0003133B
		[NetInvokableGeneratedMethod("ReceivePowered", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePowered_Write(NetPakWriter writer, bool newPowered)
		{
			writer.WriteBit(newPowered);
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00033148 File Offset: 0x00031348
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
			InteractableSpot interactableSpot = obj as InteractableSpot;
			if (interactableSpot == null)
			{
				return;
			}
			bool desiredPowered;
			reader.ReadBit(ref desiredPowered);
			interactableSpot.ReceiveToggleRequest(context, desiredPowered);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00033195 File Offset: 0x00031395
		[NetInvokableGeneratedMethod("ReceiveToggleRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToggleRequest_Write(NetPakWriter writer, bool desiredPowered)
		{
			writer.WriteBit(desiredPowered);
		}
	}
}
