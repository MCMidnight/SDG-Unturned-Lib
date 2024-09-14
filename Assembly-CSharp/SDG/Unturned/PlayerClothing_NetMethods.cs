using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001F5 RID: 501
	[NetInvokableGeneratedClass(typeof(PlayerClothing))]
	public static class PlayerClothing_NetMethods
	{
		// Token: 0x06000F0A RID: 3850 RVA: 0x000341EC File Offset: 0x000323EC
		[NetInvokableGeneratedMethod("ReceiveShirtQuality", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveShirtQuality_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			playerClothing.ReceiveShirtQuality(quality);
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00034238 File Offset: 0x00032438
		[NetInvokableGeneratedMethod("ReceiveShirtQuality", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveShirtQuality_Write(NetPakWriter writer, byte quality)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00034244 File Offset: 0x00032444
		[NetInvokableGeneratedMethod("ReceivePantsQuality", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePantsQuality_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			playerClothing.ReceivePantsQuality(quality);
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x00034290 File Offset: 0x00032490
		[NetInvokableGeneratedMethod("ReceivePantsQuality", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePantsQuality_Write(NetPakWriter writer, byte quality)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0003429C File Offset: 0x0003249C
		[NetInvokableGeneratedMethod("ReceiveHatQuality", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveHatQuality_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			playerClothing.ReceiveHatQuality(quality);
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x000342E8 File Offset: 0x000324E8
		[NetInvokableGeneratedMethod("ReceiveHatQuality", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveHatQuality_Write(NetPakWriter writer, byte quality)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x000342F4 File Offset: 0x000324F4
		[NetInvokableGeneratedMethod("ReceiveBackpackQuality", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBackpackQuality_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			playerClothing.ReceiveBackpackQuality(quality);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00034340 File Offset: 0x00032540
		[NetInvokableGeneratedMethod("ReceiveBackpackQuality", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBackpackQuality_Write(NetPakWriter writer, byte quality)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0003434C File Offset: 0x0003254C
		[NetInvokableGeneratedMethod("ReceiveVestQuality", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVestQuality_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			playerClothing.ReceiveVestQuality(quality);
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00034398 File Offset: 0x00032598
		[NetInvokableGeneratedMethod("ReceiveVestQuality", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVestQuality_Write(NetPakWriter writer, byte quality)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x000343A4 File Offset: 0x000325A4
		[NetInvokableGeneratedMethod("ReceiveMaskQuality", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveMaskQuality_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			playerClothing.ReceiveMaskQuality(quality);
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x000343F0 File Offset: 0x000325F0
		[NetInvokableGeneratedMethod("ReceiveMaskQuality", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveMaskQuality_Write(NetPakWriter writer, byte quality)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x000343FC File Offset: 0x000325FC
		[NetInvokableGeneratedMethod("ReceiveGlassesQuality", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveGlassesQuality_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			playerClothing.ReceiveGlassesQuality(quality);
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x00034448 File Offset: 0x00032648
		[NetInvokableGeneratedMethod("ReceiveGlassesQuality", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveGlassesQuality_Write(NetPakWriter writer, byte quality)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00034454 File Offset: 0x00032654
		[NetInvokableGeneratedMethod("ReceiveWearShirt", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveWearShirt_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			Guid id;
			SystemNetPakReaderEx.ReadGuid(reader, ref id);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			bool playEffect;
			reader.ReadBit(ref playEffect);
			playerClothing.ReceiveWearShirt(id, quality, array, playEffect);
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x000344D4 File Offset: 0x000326D4
		[NetInvokableGeneratedMethod("ReceiveWearShirt", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveWearShirt_Write(NetPakWriter writer, Guid id, byte quality, byte[] state, bool playEffect)
		{
			SystemNetPakWriterEx.WriteGuid(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
			writer.WriteBit(playEffect);
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x00034510 File Offset: 0x00032710
		[NetInvokableGeneratedMethod("ReceiveSwapShirtRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSwapShirtRequest_Read(in ServerInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerClothing.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerClothing));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerClothing.ReceiveSwapShirtRequest(page, x, y);
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00034592 File Offset: 0x00032792
		[NetInvokableGeneratedMethod("ReceiveSwapShirtRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSwapShirtRequest_Write(NetPakWriter writer, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x000345AC File Offset: 0x000327AC
		[NetInvokableGeneratedMethod("ReceiveWearPants", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveWearPants_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			Guid id;
			SystemNetPakReaderEx.ReadGuid(reader, ref id);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			bool playEffect;
			reader.ReadBit(ref playEffect);
			playerClothing.ReceiveWearPants(id, quality, array, playEffect);
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0003462C File Offset: 0x0003282C
		[NetInvokableGeneratedMethod("ReceiveWearPants", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveWearPants_Write(NetPakWriter writer, Guid id, byte quality, byte[] state, bool playEffect)
		{
			SystemNetPakWriterEx.WriteGuid(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
			writer.WriteBit(playEffect);
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00034668 File Offset: 0x00032868
		[NetInvokableGeneratedMethod("ReceiveSwapPantsRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSwapPantsRequest_Read(in ServerInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerClothing.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerClothing));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerClothing.ReceiveSwapPantsRequest(page, x, y);
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x000346EA File Offset: 0x000328EA
		[NetInvokableGeneratedMethod("ReceiveSwapPantsRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSwapPantsRequest_Write(NetPakWriter writer, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00034704 File Offset: 0x00032904
		[NetInvokableGeneratedMethod("ReceiveWearHat", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveWearHat_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			Guid id;
			SystemNetPakReaderEx.ReadGuid(reader, ref id);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			bool playEffect;
			reader.ReadBit(ref playEffect);
			playerClothing.ReceiveWearHat(id, quality, array, playEffect);
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00034784 File Offset: 0x00032984
		[NetInvokableGeneratedMethod("ReceiveWearHat", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveWearHat_Write(NetPakWriter writer, Guid id, byte quality, byte[] state, bool playEffect)
		{
			SystemNetPakWriterEx.WriteGuid(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
			writer.WriteBit(playEffect);
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x000347C0 File Offset: 0x000329C0
		[NetInvokableGeneratedMethod("ReceiveSwapHatRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSwapHatRequest_Read(in ServerInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerClothing.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerClothing));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerClothing.ReceiveSwapHatRequest(page, x, y);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00034842 File Offset: 0x00032A42
		[NetInvokableGeneratedMethod("ReceiveSwapHatRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSwapHatRequest_Write(NetPakWriter writer, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0003485C File Offset: 0x00032A5C
		[NetInvokableGeneratedMethod("ReceiveWearBackpack", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveWearBackpack_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			Guid id;
			SystemNetPakReaderEx.ReadGuid(reader, ref id);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			bool playEffect;
			reader.ReadBit(ref playEffect);
			playerClothing.ReceiveWearBackpack(id, quality, array, playEffect);
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x000348DC File Offset: 0x00032ADC
		[NetInvokableGeneratedMethod("ReceiveWearBackpack", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveWearBackpack_Write(NetPakWriter writer, Guid id, byte quality, byte[] state, bool playEffect)
		{
			SystemNetPakWriterEx.WriteGuid(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
			writer.WriteBit(playEffect);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00034918 File Offset: 0x00032B18
		[NetInvokableGeneratedMethod("ReceiveSwapBackpackRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSwapBackpackRequest_Read(in ServerInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerClothing.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerClothing));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerClothing.ReceiveSwapBackpackRequest(page, x, y);
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0003499A File Offset: 0x00032B9A
		[NetInvokableGeneratedMethod("ReceiveSwapBackpackRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSwapBackpackRequest_Write(NetPakWriter writer, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x000349B4 File Offset: 0x00032BB4
		[NetInvokableGeneratedMethod("ReceiveVisualToggleState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVisualToggleState_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			EVisualToggleType type;
			reader.ReadEnum(out type);
			bool toggle;
			reader.ReadBit(ref toggle);
			playerClothing.ReceiveVisualToggleState(type, toggle);
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x00034A0B File Offset: 0x00032C0B
		[NetInvokableGeneratedMethod("ReceiveVisualToggleState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVisualToggleState_Write(NetPakWriter writer, EVisualToggleType type, bool toggle)
		{
			writer.WriteEnum(type);
			writer.WriteBit(toggle);
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00034A20 File Offset: 0x00032C20
		[NetInvokableGeneratedMethod("ReceiveVisualToggleRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVisualToggleRequest_Read(in ServerInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerClothing.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerClothing));
				return;
			}
			EVisualToggleType type;
			reader.ReadEnum(out type);
			playerClothing.ReceiveVisualToggleRequest(type);
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00034A8C File Offset: 0x00032C8C
		[NetInvokableGeneratedMethod("ReceiveVisualToggleRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVisualToggleRequest_Write(NetPakWriter writer, EVisualToggleType type)
		{
			writer.WriteEnum(type);
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00034A98 File Offset: 0x00032C98
		[NetInvokableGeneratedMethod("ReceiveWearVest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveWearVest_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			Guid id;
			SystemNetPakReaderEx.ReadGuid(reader, ref id);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			bool playEffect;
			reader.ReadBit(ref playEffect);
			playerClothing.ReceiveWearVest(id, quality, array, playEffect);
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00034B18 File Offset: 0x00032D18
		[NetInvokableGeneratedMethod("ReceiveWearVest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveWearVest_Write(NetPakWriter writer, Guid id, byte quality, byte[] state, bool playEffect)
		{
			SystemNetPakWriterEx.WriteGuid(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
			writer.WriteBit(playEffect);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x00034B54 File Offset: 0x00032D54
		[NetInvokableGeneratedMethod("ReceiveSwapVestRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSwapVestRequest_Read(in ServerInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerClothing.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerClothing));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerClothing.ReceiveSwapVestRequest(page, x, y);
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00034BD6 File Offset: 0x00032DD6
		[NetInvokableGeneratedMethod("ReceiveSwapVestRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSwapVestRequest_Write(NetPakWriter writer, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00034BF0 File Offset: 0x00032DF0
		[NetInvokableGeneratedMethod("ReceiveWearMask", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveWearMask_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			Guid id;
			SystemNetPakReaderEx.ReadGuid(reader, ref id);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			bool playEffect;
			reader.ReadBit(ref playEffect);
			playerClothing.ReceiveWearMask(id, quality, array, playEffect);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00034C70 File Offset: 0x00032E70
		[NetInvokableGeneratedMethod("ReceiveWearMask", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveWearMask_Write(NetPakWriter writer, Guid id, byte quality, byte[] state, bool playEffect)
		{
			SystemNetPakWriterEx.WriteGuid(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
			writer.WriteBit(playEffect);
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00034CAC File Offset: 0x00032EAC
		[NetInvokableGeneratedMethod("ReceiveSwapMaskRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSwapMaskRequest_Read(in ServerInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerClothing.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerClothing));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerClothing.ReceiveSwapMaskRequest(page, x, y);
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00034D2E File Offset: 0x00032F2E
		[NetInvokableGeneratedMethod("ReceiveSwapMaskRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSwapMaskRequest_Write(NetPakWriter writer, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00034D48 File Offset: 0x00032F48
		[NetInvokableGeneratedMethod("ReceiveWearGlasses", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveWearGlasses_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			Guid id;
			SystemNetPakReaderEx.ReadGuid(reader, ref id);
			byte quality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref quality);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte[] array = new byte[(int)b];
			reader.ReadBytes(array);
			bool playEffect;
			reader.ReadBit(ref playEffect);
			playerClothing.ReceiveWearGlasses(id, quality, array, playEffect);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00034DC8 File Offset: 0x00032FC8
		[NetInvokableGeneratedMethod("ReceiveWearGlasses", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveWearGlasses_Write(NetPakWriter writer, Guid id, byte quality, byte[] state, bool playEffect)
		{
			SystemNetPakWriterEx.WriteGuid(writer, id);
			SystemNetPakWriterEx.WriteUInt8(writer, quality);
			byte b = (byte)state.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			writer.WriteBytes(state, (int)b);
			writer.WriteBit(playEffect);
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00034E04 File Offset: 0x00033004
		[NetInvokableGeneratedMethod("ReceiveSwapGlassesRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSwapGlassesRequest_Read(in ServerInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerClothing.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerClothing));
				return;
			}
			byte page;
			SystemNetPakReaderEx.ReadUInt8(reader, ref page);
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			playerClothing.ReceiveSwapGlassesRequest(page, x, y);
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00034E86 File Offset: 0x00033086
		[NetInvokableGeneratedMethod("ReceiveSwapGlassesRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSwapGlassesRequest_Write(NetPakWriter writer, byte page, byte x, byte y)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, page);
			SystemNetPakWriterEx.WriteUInt8(writer, x);
			SystemNetPakWriterEx.WriteUInt8(writer, y);
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00034EA0 File Offset: 0x000330A0
		[NetInvokableGeneratedMethod("ReceiveClothingState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveClothingState_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			playerClothing.ReceiveClothingState(context);
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00034EE0 File Offset: 0x000330E0
		[NetInvokableGeneratedMethod("ReceiveFaceState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveFaceState_Read(in ClientInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			playerClothing.ReceiveFaceState(index);
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00034F2C File Offset: 0x0003312C
		[NetInvokableGeneratedMethod("ReceiveFaceState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveFaceState_Write(NetPakWriter writer, byte index)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, index);
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00034F38 File Offset: 0x00033138
		[NetInvokableGeneratedMethod("ReceiveSwapFaceRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSwapFaceRequest_Read(in ServerInvocationContext context)
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
			PlayerClothing playerClothing = obj as PlayerClothing;
			if (playerClothing == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerClothing.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerClothing));
				return;
			}
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			playerClothing.ReceiveSwapFaceRequest(index);
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00034FA4 File Offset: 0x000331A4
		[NetInvokableGeneratedMethod("ReceiveSwapFaceRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSwapFaceRequest_Write(NetPakWriter writer, byte index)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, index);
		}
	}
}
