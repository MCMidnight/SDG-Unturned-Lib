using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001FA RID: 506
	[NetInvokableGeneratedClass(typeof(PlayerInventory))]
	public static class PlayerInventory_NetMethods
	{
		// Token: 0x06000F5E RID: 3934 RVA: 0x00035884 File Offset: 0x00033A84
		[NetInvokableGeneratedMethod("ReceiveDragItem", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDragItem_Read(in ServerInvocationContext context)
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
			PlayerInventory playerInventory = obj as PlayerInventory;
			if (playerInventory == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerInventory.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerInventory));
				return;
			}
			byte page_;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page_);
			byte x_;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x_);
			byte y_;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y_);
			byte page_2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page_2);
			byte x_2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x_2);
			byte y_2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y_2);
			byte rot_;
			SystemNetPakReaderEx.ReadUInt8(reader, ref rot_);
			playerInventory.ReceiveDragItem(page_, x_, y_, page_2, x_2, y_2, rot_);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00035932 File Offset: 0x00033B32
		[NetInvokableGeneratedMethod("ReceiveDragItem", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDragItem_Write(NetPakWriter writer, byte page_0, byte x_0, byte y_0, byte page_1, byte x_1, byte y_1, byte rot_1)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page_0);
			SystemNetPakWriterEx.WriteUInt8(writer, x_0);
			SystemNetPakWriterEx.WriteUInt8(writer, y_0);
			SystemNetPakWriterEx.WriteUInt8(writer, page_1);
			SystemNetPakWriterEx.WriteUInt8(writer, x_1);
			SystemNetPakWriterEx.WriteUInt8(writer, y_1);
			SystemNetPakWriterEx.WriteUInt8(writer, rot_1);
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00035970 File Offset: 0x00033B70
		[NetInvokableGeneratedMethod("ReceiveSwapItem", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSwapItem_Read(in ServerInvocationContext context)
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
			PlayerInventory playerInventory = obj as PlayerInventory;
			if (playerInventory == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerInventory.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerInventory));
				return;
			}
			byte page_;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page_);
			byte x_;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x_);
			byte y_;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y_);
			byte rot_;
			SystemNetPakReaderEx.ReadUInt8(reader, ref rot_);
			byte page_2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page_2);
			byte x_2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x_2);
			byte y_2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y_2);
			byte rot_2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref rot_2);
			playerInventory.ReceiveSwapItem(page_, x_, y_, rot_, page_2, x_2, y_2, rot_2);
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x00035A2C File Offset: 0x00033C2C
		[NetInvokableGeneratedMethod("ReceiveSwapItem", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSwapItem_Write(NetPakWriter writer, byte page_0, byte x_0, byte y_0, byte rot_0, byte page_1, byte x_1, byte y_1, byte rot_1)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page_0);
			SystemNetPakWriterEx.WriteUInt8(writer, x_0);
			SystemNetPakWriterEx.WriteUInt8(writer, y_0);
			SystemNetPakWriterEx.WriteUInt8(writer, rot_0);
			SystemNetPakWriterEx.WriteUInt8(writer, page_1);
			SystemNetPakWriterEx.WriteUInt8(writer, x_1);
			SystemNetPakWriterEx.WriteUInt8(writer, y_1);
			SystemNetPakWriterEx.WriteUInt8(writer, rot_1);
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00035A80 File Offset: 0x00033C80
		[NetInvokableGeneratedMethod("ReceiveDropItem", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDropItem_Read(in ServerInvocationContext context)
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
			PlayerInventory playerInventory = obj as PlayerInventory;
			if (playerInventory == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerInventory.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerInventory));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerInventory.ReceiveDropItem(page, x, y);
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00035B02 File Offset: 0x00033D02
		[NetInvokableGeneratedMethod("ReceiveDropItem", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDropItem_Write(NetPakWriter writer, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x00035B1C File Offset: 0x00033D1C
		[NetInvokableGeneratedMethod("ReceiveUpdateAmount", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUpdateAmount_Read(in ClientInvocationContext context)
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
			PlayerInventory playerInventory = obj as PlayerInventory;
			if (playerInventory == null)
			{
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			byte amount;
			SystemNetPakReaderEx.ReadUInt8(reader, ref amount);
			playerInventory.ReceiveUpdateAmount(page, index, amount);
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00035B7E File Offset: 0x00033D7E
		[NetInvokableGeneratedMethod("ReceiveUpdateAmount", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUpdateAmount_Write(NetPakWriter writer, byte page, byte index, byte amount)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, index);
			SystemNetPakWriterEx.WriteUInt8(writer, amount);
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00035B98 File Offset: 0x00033D98
		[NetInvokableGeneratedMethod("ReceiveUpdateQuality", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUpdateQuality_Read(in ClientInvocationContext context)
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
			PlayerInventory playerInventory = obj as PlayerInventory;
			if (playerInventory == null)
			{
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			playerInventory.ReceiveUpdateQuality(page, index, quality);
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00035BFA File Offset: 0x00033DFA
		[NetInvokableGeneratedMethod("ReceiveUpdateQuality", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUpdateQuality_Write(NetPakWriter writer, byte page, byte index, byte quality)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, index);
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x00035C14 File Offset: 0x00033E14
		[NetInvokableGeneratedMethod("ReceiveUpdateInvState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUpdateInvState_Read(in ClientInvocationContext context)
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
			PlayerInventory playerInventory = obj as PlayerInventory;
			if (playerInventory == null)
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
			playerInventory.ReceiveUpdateInvState(page, index, array);
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x00035C88 File Offset: 0x00033E88
		[NetInvokableGeneratedMethod("ReceiveUpdateInvState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUpdateInvState_Write(NetPakWriter writer, byte page, byte index, byte[] state)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, index);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00035CBC File Offset: 0x00033EBC
		[NetInvokableGeneratedMethod("ReceiveItemAdd", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveItemAdd_Read(in ClientInvocationContext context)
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
			PlayerInventory playerInventory = obj as PlayerInventory;
			if (playerInventory == null)
			{
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			byte rot;
			SystemNetPakReaderEx.ReadUInt8(reader, ref rot);
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			byte amount;
			SystemNetPakReaderEx.ReadUInt8(reader, ref amount);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			playerInventory.ReceiveItemAdd(page, x, y, rot, id, amount, quality, array);
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00035D68 File Offset: 0x00033F68
		[NetInvokableGeneratedMethod("ReceiveItemAdd", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveItemAdd_Write(NetPakWriter writer, byte page, byte x, byte y, byte rot, ushort id, byte amount, byte quality, byte[] state)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
			SystemNetPakWriterEx.WriteUInt8(writer, rot);
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, amount);
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x00035DCC File Offset: 0x00033FCC
		[NetInvokableGeneratedMethod("ReceiveItemRemove", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveItemRemove_Read(in ClientInvocationContext context)
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
			PlayerInventory playerInventory = obj as PlayerInventory;
			if (playerInventory == null)
			{
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerInventory.ReceiveItemRemove(page, x, y);
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x00035E2E File Offset: 0x0003402E
		[NetInvokableGeneratedMethod("ReceiveItemRemove", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveItemRemove_Write(NetPakWriter writer, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00035E48 File Offset: 0x00034048
		[NetInvokableGeneratedMethod("ReceiveSize", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSize_Read(in ClientInvocationContext context)
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
			PlayerInventory playerInventory = obj as PlayerInventory;
			if (playerInventory == null)
			{
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte newWidth;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newWidth);
			byte newHeight;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newHeight);
			playerInventory.ReceiveSize(page, newWidth, newHeight);
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x00035EAA File Offset: 0x000340AA
		[NetInvokableGeneratedMethod("ReceiveSize", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSize_Write(NetPakWriter writer, byte page, byte newWidth, byte newHeight)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, newWidth);
			SystemNetPakWriterEx.WriteUInt8(writer, newHeight);
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00035EC4 File Offset: 0x000340C4
		[NetInvokableGeneratedMethod("ReceiveStoraging", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveStoraging_Read(in ClientInvocationContext context)
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
			PlayerInventory playerInventory = obj as PlayerInventory;
			if (playerInventory == null)
			{
				return;
			}
			playerInventory.ReceiveStoraging(context);
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00035F04 File Offset: 0x00034104
		[NetInvokableGeneratedMethod("ReceiveInventory", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveInventory_Read(in ClientInvocationContext context)
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
			PlayerInventory playerInventory = obj as PlayerInventory;
			if (playerInventory == null)
			{
				return;
			}
			playerInventory.ReceiveInventory(context);
		}
	}
}
