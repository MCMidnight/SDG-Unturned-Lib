using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001E9 RID: 489
	[NetInvokableGeneratedClass(typeof(InteractableStereo))]
	public static class InteractableStereo_NetMethods
	{
		// Token: 0x06000EB9 RID: 3769 RVA: 0x000331A0 File Offset: 0x000313A0
		private static void ReceiveTrack_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableStereo interactableStereo = voidNetObj as InteractableStereo;
			if (interactableStereo == null)
			{
				return;
			}
			Guid newTrack;
			SystemNetPakReaderEx.ReadGuid(context.reader, ref newTrack);
			interactableStereo.ReceiveTrack(newTrack);
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x000331D4 File Offset: 0x000313D4
		[NetInvokableGeneratedMethod("ReceiveTrack", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTrack_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableStereo_NetMethods.ReceiveTrack_DeferredRead));
				return;
			}
			InteractableStereo interactableStereo = obj as InteractableStereo;
			if (interactableStereo == null)
			{
				return;
			}
			Guid newTrack;
			SystemNetPakReaderEx.ReadGuid(reader, ref newTrack);
			interactableStereo.ReceiveTrack(newTrack);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00033233 File Offset: 0x00031433
		[NetInvokableGeneratedMethod("ReceiveTrack", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTrack_Write(NetPakWriter writer, Guid newTrack)
		{
			SystemNetPakWriterEx.WriteGuid(writer, newTrack);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00033240 File Offset: 0x00031440
		[NetInvokableGeneratedMethod("ReceiveTrackRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTrackRequest_Read(in ServerInvocationContext context)
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
			InteractableStereo interactableStereo = obj as InteractableStereo;
			if (interactableStereo == null)
			{
				return;
			}
			Guid newTrack;
			SystemNetPakReaderEx.ReadGuid(reader, ref newTrack);
			interactableStereo.ReceiveTrackRequest(context, newTrack);
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0003328D File Offset: 0x0003148D
		[NetInvokableGeneratedMethod("ReceiveTrackRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTrackRequest_Write(NetPakWriter writer, Guid newTrack)
		{
			SystemNetPakWriterEx.WriteGuid(writer, newTrack);
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00033298 File Offset: 0x00031498
		private static void ReceiveChangeVolume_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableStereo interactableStereo = voidNetObj as InteractableStereo;
			if (interactableStereo == null)
			{
				return;
			}
			byte newVolume;
			SystemNetPakReaderEx.ReadUInt8(context.reader, ref newVolume);
			interactableStereo.ReceiveChangeVolume(newVolume);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x000332CC File Offset: 0x000314CC
		[NetInvokableGeneratedMethod("ReceiveChangeVolume", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveChangeVolume_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableStereo_NetMethods.ReceiveChangeVolume_DeferredRead));
				return;
			}
			InteractableStereo interactableStereo = obj as InteractableStereo;
			if (interactableStereo == null)
			{
				return;
			}
			byte newVolume;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newVolume);
			interactableStereo.ReceiveChangeVolume(newVolume);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0003332B File Offset: 0x0003152B
		[NetInvokableGeneratedMethod("ReceiveChangeVolume", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveChangeVolume_Write(NetPakWriter writer, byte newVolume)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, newVolume);
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00033338 File Offset: 0x00031538
		[NetInvokableGeneratedMethod("ReceiveChangeVolumeRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveChangeVolumeRequest_Read(in ServerInvocationContext context)
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
			InteractableStereo interactableStereo = obj as InteractableStereo;
			if (interactableStereo == null)
			{
				return;
			}
			byte newVolume;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newVolume);
			interactableStereo.ReceiveChangeVolumeRequest(context, newVolume);
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00033385 File Offset: 0x00031585
		[NetInvokableGeneratedMethod("ReceiveChangeVolumeRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveChangeVolumeRequest_Write(NetPakWriter writer, byte newVolume)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, newVolume);
		}
	}
}
