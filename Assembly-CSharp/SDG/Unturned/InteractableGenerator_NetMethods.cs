using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001DD RID: 477
	[NetInvokableGeneratedClass(typeof(InteractableGenerator))]
	public static class InteractableGenerator_NetMethods
	{
		// Token: 0x06000E7D RID: 3709 RVA: 0x000325D0 File Offset: 0x000307D0
		private static void ReceiveFuel_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableGenerator interactableGenerator = voidNetObj as InteractableGenerator;
			if (interactableGenerator == null)
			{
				return;
			}
			ushort newFuel;
			SystemNetPakReaderEx.ReadUInt16(context.reader, ref newFuel);
			interactableGenerator.ReceiveFuel(newFuel);
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00032604 File Offset: 0x00030804
		[NetInvokableGeneratedMethod("ReceiveFuel", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveFuel_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableGenerator_NetMethods.ReceiveFuel_DeferredRead));
				return;
			}
			InteractableGenerator interactableGenerator = obj as InteractableGenerator;
			if (interactableGenerator == null)
			{
				return;
			}
			ushort newFuel;
			SystemNetPakReaderEx.ReadUInt16(reader, ref newFuel);
			interactableGenerator.ReceiveFuel(newFuel);
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00032663 File Offset: 0x00030863
		[NetInvokableGeneratedMethod("ReceiveFuel", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveFuel_Write(NetPakWriter writer, ushort newFuel)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, newFuel);
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00032670 File Offset: 0x00030870
		private static void ReceivePowered_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableGenerator interactableGenerator = voidNetObj as InteractableGenerator;
			if (interactableGenerator == null)
			{
				return;
			}
			bool newPowered;
			context.reader.ReadBit(ref newPowered);
			interactableGenerator.ReceivePowered(newPowered);
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x000326A4 File Offset: 0x000308A4
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableGenerator_NetMethods.ReceivePowered_DeferredRead));
				return;
			}
			InteractableGenerator interactableGenerator = obj as InteractableGenerator;
			if (interactableGenerator == null)
			{
				return;
			}
			bool newPowered;
			reader.ReadBit(ref newPowered);
			interactableGenerator.ReceivePowered(newPowered);
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00032703 File Offset: 0x00030903
		[NetInvokableGeneratedMethod("ReceivePowered", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePowered_Write(NetPakWriter writer, bool newPowered)
		{
			writer.WriteBit(newPowered);
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00032710 File Offset: 0x00030910
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
			InteractableGenerator interactableGenerator = obj as InteractableGenerator;
			if (interactableGenerator == null)
			{
				return;
			}
			bool desiredPowered;
			reader.ReadBit(ref desiredPowered);
			interactableGenerator.ReceiveToggleRequest(context, desiredPowered);
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0003275D File Offset: 0x0003095D
		[NetInvokableGeneratedMethod("ReceiveToggleRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToggleRequest_Write(NetPakWriter writer, bool desiredPowered)
		{
			writer.WriteBit(desiredPowered);
		}
	}
}
