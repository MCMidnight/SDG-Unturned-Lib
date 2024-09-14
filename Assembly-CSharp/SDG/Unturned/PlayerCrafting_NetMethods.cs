using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001F6 RID: 502
	[NetInvokableGeneratedClass(typeof(PlayerCrafting))]
	public static class PlayerCrafting_NetMethods
	{
		// Token: 0x06000F3D RID: 3901 RVA: 0x00034FB0 File Offset: 0x000331B0
		[NetInvokableGeneratedMethod("ReceiveStripAttachments", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveStripAttachments_Read(in ServerInvocationContext context)
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
			PlayerCrafting playerCrafting = obj as PlayerCrafting;
			if (playerCrafting == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerCrafting.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerCrafting));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerCrafting.ReceiveStripAttachments(page, x, y);
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x00035032 File Offset: 0x00033232
		[NetInvokableGeneratedMethod("ReceiveStripAttachments", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveStripAttachments_Write(NetPakWriter writer, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0003504C File Offset: 0x0003324C
		[NetInvokableGeneratedMethod("ReceiveRefreshCrafting", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveRefreshCrafting_Read(in ClientInvocationContext context)
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
			PlayerCrafting playerCrafting = obj as PlayerCrafting;
			if (playerCrafting == null)
			{
				return;
			}
			playerCrafting.ReceiveRefreshCrafting();
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0003508B File Offset: 0x0003328B
		[NetInvokableGeneratedMethod("ReceiveRefreshCrafting", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveRefreshCrafting_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00035090 File Offset: 0x00033290
		[NetInvokableGeneratedMethod("ReceiveCraft", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveCraft_Read(in ServerInvocationContext context)
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
			PlayerCrafting playerCrafting = obj as PlayerCrafting;
			if (playerCrafting == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerCrafting.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerCrafting));
				return;
			}
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			bool force;
			reader.ReadBit(ref force);
			playerCrafting.ReceiveCraft(context, id, index, force);
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x00035113 File Offset: 0x00033313
		[NetInvokableGeneratedMethod("ReceiveCraft", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveCraft_Write(NetPakWriter writer, ushort id, byte index, bool force)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, index);
			writer.WriteBit(force);
		}
	}
}
