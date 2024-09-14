using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001EA RID: 490
	[NetInvokableGeneratedClass(typeof(InteractableStorage))]
	public static class InteractableStorage_NetMethods
	{
		// Token: 0x06000EC3 RID: 3779 RVA: 0x00033390 File Offset: 0x00031590
		[NetInvokableGeneratedMethod("ReceiveInteractRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveInteractRequest_Read(in ServerInvocationContext context)
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
			InteractableStorage interactableStorage = obj as InteractableStorage;
			if (interactableStorage == null)
			{
				return;
			}
			bool quickGrab;
			reader.ReadBit(ref quickGrab);
			interactableStorage.ReceiveInteractRequest(context, quickGrab);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x000333DD File Offset: 0x000315DD
		[NetInvokableGeneratedMethod("ReceiveInteractRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveInteractRequest_Write(NetPakWriter writer, bool quickGrab)
		{
			writer.WriteBit(quickGrab);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x000333E8 File Offset: 0x000315E8
		private static void ReceiveDisplay_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableStorage interactableStorage = voidNetObj as InteractableStorage;
			if (interactableStorage == null)
			{
				return;
			}
			NetPakReader reader = context.reader;
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			ushort skin;
			SystemNetPakReaderEx.ReadUInt16(reader, ref skin);
			ushort mythic;
			SystemNetPakReaderEx.ReadUInt16(reader, ref mythic);
			string tags;
			SystemNetPakReaderEx.ReadString(reader, ref tags, 11);
			string dynamicProps;
			SystemNetPakReaderEx.ReadString(reader, ref dynamicProps, 11);
			interactableStorage.ReceiveDisplay(id, quality, array, skin, mythic, tags, dynamicProps);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00033470 File Offset: 0x00031670
		[NetInvokableGeneratedMethod("ReceiveDisplay", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDisplay_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableStorage_NetMethods.ReceiveDisplay_DeferredRead));
				return;
			}
			InteractableStorage interactableStorage = obj as InteractableStorage;
			if (interactableStorage == null)
			{
				return;
			}
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			ushort skin;
			SystemNetPakReaderEx.ReadUInt16(reader, ref skin);
			ushort mythic;
			SystemNetPakReaderEx.ReadUInt16(reader, ref mythic);
			string tags;
			SystemNetPakReaderEx.ReadString(reader, ref tags, 11);
			string dynamicProps;
			SystemNetPakReaderEx.ReadString(reader, ref dynamicProps, 11);
			interactableStorage.ReceiveDisplay(id, quality, array, skin, mythic, tags, dynamicProps);
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00033528 File Offset: 0x00031728
		[NetInvokableGeneratedMethod("ReceiveDisplay", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDisplay_Write(NetPakWriter writer, ushort id, byte quality, byte[] state, ushort skin, ushort mythic, string tags, string dynamicProps)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
			SystemNetPakWriterEx.WriteUInt16(writer, skin);
			SystemNetPakWriterEx.WriteUInt16(writer, mythic);
			SystemNetPakWriterEx.WriteString(writer, tags, 11);
			SystemNetPakWriterEx.WriteString(writer, dynamicProps, 11);
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00033584 File Offset: 0x00031784
		private static void ReceiveRotDisplay_DeferredRead(object voidNetObj, in ClientInvocationContext context)
		{
			InteractableStorage interactableStorage = voidNetObj as InteractableStorage;
			if (interactableStorage == null)
			{
				return;
			}
			byte rotComp;
			SystemNetPakReaderEx.ReadUInt8(context.reader, ref rotComp);
			interactableStorage.ReceiveRotDisplay(rotComp);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x000335B8 File Offset: 0x000317B8
		[NetInvokableGeneratedMethod("ReceiveRotDisplay", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveRotDisplay_Read(in ClientInvocationContext context)
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
				NetInvocationDeferralRegistry.Defer(key, context, new NetInvokeDeferred(InteractableStorage_NetMethods.ReceiveRotDisplay_DeferredRead));
				return;
			}
			InteractableStorage interactableStorage = obj as InteractableStorage;
			if (interactableStorage == null)
			{
				return;
			}
			byte rotComp;
			SystemNetPakReaderEx.ReadUInt8(reader, ref rotComp);
			interactableStorage.ReceiveRotDisplay(rotComp);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00033617 File Offset: 0x00031817
		[NetInvokableGeneratedMethod("ReceiveRotDisplay", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveRotDisplay_Write(NetPakWriter writer, byte rotComp)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, rotComp);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00033624 File Offset: 0x00031824
		[NetInvokableGeneratedMethod("ReceiveRotDisplayRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveRotDisplayRequest_Read(in ServerInvocationContext context)
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
			InteractableStorage interactableStorage = obj as InteractableStorage;
			if (interactableStorage == null)
			{
				return;
			}
			byte rotComp;
			SystemNetPakReaderEx.ReadUInt8(reader, ref rotComp);
			interactableStorage.ReceiveRotDisplayRequest(context, rotComp);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00033671 File Offset: 0x00031871
		[NetInvokableGeneratedMethod("ReceiveRotDisplayRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveRotDisplayRequest_Write(NetPakWriter writer, byte rotComp)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, rotComp);
		}
	}
}
