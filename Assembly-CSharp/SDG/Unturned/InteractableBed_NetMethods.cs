using System;
using SDG.NetPak;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020001D9 RID: 473
	[NetInvokableGeneratedClass(typeof(InteractableBed))]
	public static class InteractableBed_NetMethods
	{
		// Token: 0x06000E6B RID: 3691 RVA: 0x00032220 File Offset: 0x00030420
		private static void ReceiveClaim_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableBed interactableBed = voidNetObj as InteractableBed;
			if (interactableBed == null)
			{
				return;
			}
			CSteamID newOwner;
			SteamworksNetPakReaderEx.ReadSteamID(context.reader, ref newOwner);
			interactableBed.ReceiveClaim(newOwner);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00032254 File Offset: 0x00030454
		[NetInvokableGeneratedMethod("ReceiveClaim", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveClaim_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableBed_NetMethods.ReceiveClaim_DeferredRead));
				return;
			}
			InteractableBed interactableBed = obj as InteractableBed;
			if (interactableBed == null)
			{
				return;
			}
			CSteamID newOwner;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref newOwner);
			interactableBed.ReceiveClaim(newOwner);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x000322B3 File Offset: 0x000304B3
		[NetInvokableGeneratedMethod("ReceiveClaim", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveClaim_Write(NetPakWriter writer, CSteamID newOwner)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, newOwner);
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x000322C0 File Offset: 0x000304C0
		[NetInvokableGeneratedMethod("ReceiveClaimRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveClaimRequest_Read(in ServerInvocationContext context)
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
			InteractableBed interactableBed = obj as InteractableBed;
			if (interactableBed == null)
			{
				return;
			}
			interactableBed.ReceiveClaimRequest(context);
		}
	}
}
