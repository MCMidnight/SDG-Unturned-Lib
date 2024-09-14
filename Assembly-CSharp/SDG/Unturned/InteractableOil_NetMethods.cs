using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001E1 RID: 481
	[NetInvokableGeneratedClass(typeof(InteractableOil))]
	public static class InteractableOil_NetMethods
	{
		// Token: 0x06000E96 RID: 3734 RVA: 0x00032ACC File Offset: 0x00030CCC
		private static void ReceiveFuel_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableOil interactableOil = voidNetObj as InteractableOil;
			if (interactableOil == null)
			{
				return;
			}
			ushort newFuel;
			SystemNetPakReaderEx.ReadUInt16(context.reader, ref newFuel);
			interactableOil.ReceiveFuel(newFuel);
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00032B00 File Offset: 0x00030D00
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableOil_NetMethods.ReceiveFuel_DeferredRead));
				return;
			}
			InteractableOil interactableOil = obj as InteractableOil;
			if (interactableOil == null)
			{
				return;
			}
			ushort newFuel;
			SystemNetPakReaderEx.ReadUInt16(reader, ref newFuel);
			interactableOil.ReceiveFuel(newFuel);
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00032B5F File Offset: 0x00030D5F
		[NetInvokableGeneratedMethod("ReceiveFuel", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveFuel_Write(NetPakWriter writer, ushort newFuel)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, newFuel);
		}
	}
}
