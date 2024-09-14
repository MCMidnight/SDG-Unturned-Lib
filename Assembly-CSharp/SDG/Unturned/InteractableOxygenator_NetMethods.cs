using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001E3 RID: 483
	[NetInvokableGeneratedClass(typeof(InteractableOxygenator))]
	public static class InteractableOxygenator_NetMethods
	{
		// Token: 0x06000E9E RID: 3742 RVA: 0x00032C64 File Offset: 0x00030E64
		private static void ReceivePowered_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableOxygenator interactableOxygenator = voidNetObj as InteractableOxygenator;
			if (interactableOxygenator == null)
			{
				return;
			}
			bool newPowered;
			context.reader.ReadBit(ref newPowered);
			interactableOxygenator.ReceivePowered(newPowered);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00032C98 File Offset: 0x00030E98
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableOxygenator_NetMethods.ReceivePowered_DeferredRead));
				return;
			}
			InteractableOxygenator interactableOxygenator = obj as InteractableOxygenator;
			if (interactableOxygenator == null)
			{
				return;
			}
			bool newPowered;
			reader.ReadBit(ref newPowered);
			interactableOxygenator.ReceivePowered(newPowered);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00032CF7 File Offset: 0x00030EF7
		[NetInvokableGeneratedMethod("ReceivePowered", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePowered_Write(NetPakWriter writer, bool newPowered)
		{
			writer.WriteBit(newPowered);
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00032D04 File Offset: 0x00030F04
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
			InteractableOxygenator interactableOxygenator = obj as InteractableOxygenator;
			if (interactableOxygenator == null)
			{
				return;
			}
			bool desiredPowered;
			reader.ReadBit(ref desiredPowered);
			interactableOxygenator.ReceiveToggleRequest(context, desiredPowered);
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00032D51 File Offset: 0x00030F51
		[NetInvokableGeneratedMethod("ReceiveToggleRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToggleRequest_Write(NetPakWriter writer, bool desiredPowered)
		{
			writer.WriteBit(desiredPowered);
		}
	}
}
