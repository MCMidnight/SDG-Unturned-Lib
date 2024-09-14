using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001E5 RID: 485
	[NetInvokableGeneratedClass(typeof(InteractableSafezone))]
	public static class InteractableSafezone_NetMethods
	{
		// Token: 0x06000EA6 RID: 3750 RVA: 0x00032DFC File Offset: 0x00030FFC
		private static void ReceivePowered_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableSafezone interactableSafezone = voidNetObj as InteractableSafezone;
			if (interactableSafezone == null)
			{
				return;
			}
			bool newPowered;
			context.reader.ReadBit(ref newPowered);
			interactableSafezone.ReceivePowered(newPowered);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00032E30 File Offset: 0x00031030
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableSafezone_NetMethods.ReceivePowered_DeferredRead));
				return;
			}
			InteractableSafezone interactableSafezone = obj as InteractableSafezone;
			if (interactableSafezone == null)
			{
				return;
			}
			bool newPowered;
			reader.ReadBit(ref newPowered);
			interactableSafezone.ReceivePowered(newPowered);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00032E8F File Offset: 0x0003108F
		[NetInvokableGeneratedMethod("ReceivePowered", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePowered_Write(NetPakWriter writer, bool newPowered)
		{
			writer.WriteBit(newPowered);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00032E9C File Offset: 0x0003109C
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
			InteractableSafezone interactableSafezone = obj as InteractableSafezone;
			if (interactableSafezone == null)
			{
				return;
			}
			bool desiredPowered;
			reader.ReadBit(ref desiredPowered);
			interactableSafezone.ReceiveToggleRequest(context, desiredPowered);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00032EE9 File Offset: 0x000310E9
		[NetInvokableGeneratedMethod("ReceiveToggleRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToggleRequest_Write(NetPakWriter writer, bool desiredPowered)
		{
			writer.WriteBit(desiredPowered);
		}
	}
}
