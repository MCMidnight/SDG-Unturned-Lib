using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001F7 RID: 503
	[NetInvokableGeneratedClass(typeof(PlayerEquipment))]
	public static class PlayerEquipment_NetMethods
	{
		// Token: 0x06000F43 RID: 3907 RVA: 0x00035130 File Offset: 0x00033330
		[NetInvokableGeneratedMethod("ReceiveItemHotkeySuggeston", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveItemHotkeySuggeston_Read(in ClientInvocationContext context)
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
			PlayerEquipment playerEquipment = obj as PlayerEquipment;
			if (playerEquipment == null)
			{
				return;
			}
			byte hotkeyIndex;
			SystemNetPakReaderEx.ReadUInt8(reader, ref hotkeyIndex);
			Guid expectedAssetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref expectedAssetGuid);
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerEquipment.ReceiveItemHotkeySuggeston(context, hotkeyIndex, expectedAssetGuid, page, x, y);
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x000351A9 File Offset: 0x000333A9
		[NetInvokableGeneratedMethod("ReceiveItemHotkeySuggeston", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveItemHotkeySuggeston_Write(NetPakWriter writer, byte hotkeyIndex, Guid expectedAssetGuid, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, hotkeyIndex);
			SystemNetPakWriterEx.WriteGuid(writer, expectedAssetGuid);
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x000351D8 File Offset: 0x000333D8
		[NetInvokableGeneratedMethod("ReceiveToggleVisionRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveToggleVisionRequest_Read(in ServerInvocationContext context)
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
			PlayerEquipment playerEquipment = obj as PlayerEquipment;
			if (playerEquipment == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerEquipment.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerEquipment));
				return;
			}
			playerEquipment.ReceiveToggleVisionRequest();
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x00035237 File Offset: 0x00033437
		[NetInvokableGeneratedMethod("ReceiveToggleVisionRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToggleVisionRequest_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0003523C File Offset: 0x0003343C
		[NetInvokableGeneratedMethod("ReceiveToggleVision", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveToggleVision_Read(in ClientInvocationContext context)
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
			PlayerEquipment playerEquipment = obj as PlayerEquipment;
			if (playerEquipment == null)
			{
				return;
			}
			playerEquipment.ReceiveToggleVision();
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0003527B File Offset: 0x0003347B
		[NetInvokableGeneratedMethod("ReceiveToggleVision", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveToggleVision_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00035280 File Offset: 0x00033480
		[NetInvokableGeneratedMethod("ReceiveSlot", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSlot_Read(in ClientInvocationContext context)
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
			PlayerEquipment playerEquipment = obj as PlayerEquipment;
			if (playerEquipment == null)
			{
				return;
			}
			byte slot;
			SystemNetPakReaderEx.ReadUInt8(reader, ref slot);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			playerEquipment.ReceiveSlot(slot, id, array);
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x000352F4 File Offset: 0x000334F4
		[NetInvokableGeneratedMethod("ReceiveSlot", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSlot_Write(NetPakWriter writer, byte slot, ushort id, byte[] state)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, slot);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00035328 File Offset: 0x00033528
		[NetInvokableGeneratedMethod("ReceiveUpdateStateTemp", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUpdateStateTemp_Read(in ClientInvocationContext context)
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
			PlayerEquipment playerEquipment = obj as PlayerEquipment;
			if (playerEquipment == null)
			{
				return;
			}
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			playerEquipment.ReceiveUpdateStateTemp(array);
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00035388 File Offset: 0x00033588
		[NetInvokableGeneratedMethod("ReceiveUpdateStateTemp", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUpdateStateTemp_Write(NetPakWriter writer, byte[] newState)
		{
			byte b = (byte)newState.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(newState, (int)b);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x000353AC File Offset: 0x000335AC
		[NetInvokableGeneratedMethod("ReceiveUpdateState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUpdateState_Read(in ClientInvocationContext context)
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
			PlayerEquipment playerEquipment = obj as PlayerEquipment;
			if (playerEquipment == null)
			{
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			playerEquipment.ReceiveUpdateState(page, index, array);
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00035420 File Offset: 0x00033620
		[NetInvokableGeneratedMethod("ReceiveUpdateState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUpdateState_Write(NetPakWriter writer, byte page, byte index, byte[] newState)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, index);
			byte b = (byte)newState.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(newState, (int)b);
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00035454 File Offset: 0x00033654
		[NetInvokableGeneratedMethod("ReceiveEquip", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEquip_Read(in ClientInvocationContext context)
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
			PlayerEquipment playerEquipment = obj as PlayerEquipment;
			if (playerEquipment == null)
			{
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			Guid newAssetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref newAssetGuid);
			byte newQuality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newQuality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			NetId useableNetId;
			reader.ReadNetId(out useableNetId);
			playerEquipment.ReceiveEquip(page, x, y, newAssetGuid, newQuality, array, useableNetId);
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x000354F4 File Offset: 0x000336F4
		[NetInvokableGeneratedMethod("ReceiveEquip", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEquip_Write(NetPakWriter writer, byte page, byte x, byte y, Guid newAssetGuid, byte newQuality, byte[] newState, NetId useableNetId)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteGuid(writer, newAssetGuid);
			SystemNetPakWriterEx.WriteUInt8(writer, newQuality);
			byte b = (byte)newState.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(newState, (int)b);
			writer.WriteNetId(useableNetId);
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0003554C File Offset: 0x0003374C
		[NetInvokableGeneratedMethod("ReceiveEquipRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveEquipRequest_Read(in ServerInvocationContext context)
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
			PlayerEquipment playerEquipment = obj as PlayerEquipment;
			if (playerEquipment == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerEquipment.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerEquipment));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerEquipment.ReceiveEquipRequest(page, x, y);
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x000355CE File Offset: 0x000337CE
		[NetInvokableGeneratedMethod("ReceiveEquipRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveEquipRequest_Write(NetPakWriter writer, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}
	}
}
