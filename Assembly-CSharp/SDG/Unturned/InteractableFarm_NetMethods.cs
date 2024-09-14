using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001DB RID: 475
	[NetInvokableGeneratedClass(typeof(InteractableFarm))]
	public static class InteractableFarm_NetMethods
	{
		// Token: 0x06000E74 RID: 3700 RVA: 0x000323F8 File Offset: 0x000305F8
		private static void ReceivePlanted_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableFarm interactableFarm = voidNetObj as InteractableFarm;
			if (interactableFarm == null)
			{
				return;
			}
			uint newPlanted;
			SystemNetPakReaderEx.ReadUInt32(context.reader, ref newPlanted);
			interactableFarm.ReceivePlanted(newPlanted);
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0003242C File Offset: 0x0003062C
		[NetInvokableGeneratedMethod("ReceivePlanted", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePlanted_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableFarm_NetMethods.ReceivePlanted_DeferredRead));
				return;
			}
			InteractableFarm interactableFarm = obj as InteractableFarm;
			if (interactableFarm == null)
			{
				return;
			}
			uint newPlanted;
			SystemNetPakReaderEx.ReadUInt32(reader, ref newPlanted);
			interactableFarm.ReceivePlanted(newPlanted);
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x0003248B File Offset: 0x0003068B
		[NetInvokableGeneratedMethod("ReceivePlanted", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePlanted_Write(NetPakWriter writer, uint newPlanted)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, newPlanted);
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00032498 File Offset: 0x00030698
		[NetInvokableGeneratedMethod("ReceiveHarvestRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveHarvestRequest_Read(in ServerInvocationContext context)
		{
			NetId key;
			if (!context.reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			InteractableFarm interactableFarm = obj as InteractableFarm;
			if (interactableFarm == null)
			{
				return;
			}
			interactableFarm.ReceiveHarvestRequest(context);
		}
	}
}
