using System;
using SDG.NetPak;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000203 RID: 515
	[NetInvokableGeneratedClass(typeof(Player))]
	public static class Player_NetMethods
	{
		// Token: 0x06000FF4 RID: 4084 RVA: 0x00037B88 File Offset: 0x00035D88
		[NetInvokableGeneratedMethod("ReceiveScreenshotDestination", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveScreenshotDestination_Read(in ClientInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			player.ReceiveScreenshotDestination(context);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00037BC8 File Offset: 0x00035DC8
		[NetInvokableGeneratedMethod("ReceiveScreenshotRelay", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveScreenshotRelay_Read(in ServerInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			if (!context.IsOwnerOf(player.channel))
			{
				context.Kick(string.Format("not owner of {0}", player));
				return;
			}
			player.ReceiveScreenshotRelay(context);
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00037C28 File Offset: 0x00035E28
		[NetInvokableGeneratedMethod("ReceiveTakeScreenshot", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTakeScreenshot_Read(in ClientInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			player.ReceiveTakeScreenshot();
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00037C67 File Offset: 0x00035E67
		[NetInvokableGeneratedMethod("ReceiveTakeScreenshot", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTakeScreenshot_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00037C6C File Offset: 0x00035E6C
		[NetInvokableGeneratedMethod("ReceiveBrowserRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBrowserRequest_Read(in ClientInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			string msg;
			SystemNetPakReaderEx.ReadString(reader, ref msg, 11);
			string url;
			SystemNetPakReaderEx.ReadString(reader, ref url, 11);
			player.ReceiveBrowserRequest(msg, url);
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00037CC7 File Offset: 0x00035EC7
		[NetInvokableGeneratedMethod("ReceiveBrowserRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBrowserRequest_Write(NetPakWriter writer, string msg, string url)
		{
			SystemNetPakWriterEx.WriteString(writer, msg, 11);
			SystemNetPakWriterEx.WriteString(writer, url, 11);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00037CE0 File Offset: 0x00035EE0
		[NetInvokableGeneratedMethod("ReceiveHintMessage", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveHintMessage_Read(in ClientInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			string message;
			SystemNetPakReaderEx.ReadString(reader, ref message, 11);
			float durationSeconds;
			SystemNetPakReaderEx.ReadFloat(reader, ref durationSeconds);
			player.ReceiveHintMessage(message, durationSeconds);
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00037D39 File Offset: 0x00035F39
		[NetInvokableGeneratedMethod("ReceiveHintMessage", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveHintMessage_Write(NetPakWriter writer, string message, float durationSeconds)
		{
			SystemNetPakWriterEx.WriteString(writer, message, 11);
			SystemNetPakWriterEx.WriteFloat(writer, durationSeconds);
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00037D50 File Offset: 0x00035F50
		[NetInvokableGeneratedMethod("ReceiveRelayToServer", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveRelayToServer_Read(in ClientInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			uint ip;
			SystemNetPakReaderEx.ReadUInt32(reader, ref ip);
			ushort port;
			SystemNetPakReaderEx.ReadUInt16(reader, ref port);
			CSteamID serverCode;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref serverCode);
			string password;
			SystemNetPakReaderEx.ReadString(reader, ref password, 11);
			bool shouldShowMenu;
			reader.ReadBit(ref shouldShowMenu);
			player.ReceiveRelayToServer(ip, port, serverCode, password, shouldShowMenu);
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00037DCA File Offset: 0x00035FCA
		[NetInvokableGeneratedMethod("ReceiveRelayToServer", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveRelayToServer_Write(NetPakWriter writer, uint ip, ushort port, CSteamID serverCode, string password, bool shouldShowMenu)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, ip);
			SystemNetPakWriterEx.WriteUInt16(writer, port);
			SteamworksNetPakWriterEx.WriteSteamID(writer, serverCode);
			SystemNetPakWriterEx.WriteString(writer, password, 11);
			writer.WriteBit(shouldShowMenu);
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00037DF8 File Offset: 0x00035FF8
		[NetInvokableGeneratedMethod("ReceiveSetPluginWidgetFlags", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSetPluginWidgetFlags_Read(in ClientInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			uint newFlags;
			SystemNetPakReaderEx.ReadUInt32(reader, ref newFlags);
			player.ReceiveSetPluginWidgetFlags(newFlags);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00037E44 File Offset: 0x00036044
		[NetInvokableGeneratedMethod("ReceiveSetPluginWidgetFlags", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSetPluginWidgetFlags_Write(NetPakWriter writer, uint newFlags)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, newFlags);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00037E50 File Offset: 0x00036050
		[NetInvokableGeneratedMethod("ReceiveAdminUsageFlags", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAdminUsageFlags_Read(in ServerInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			if (!context.IsOwnerOf(player.channel))
			{
				context.Kick(string.Format("not owner of {0}", player));
				return;
			}
			uint newFlagsBitmask;
			SystemNetPakReaderEx.ReadUInt32(reader, ref newFlagsBitmask);
			player.ReceiveAdminUsageFlags(context, newFlagsBitmask);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00037EBD File Offset: 0x000360BD
		[NetInvokableGeneratedMethod("ReceiveAdminUsageFlags", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAdminUsageFlags_Write(NetPakWriter writer, uint newFlagsBitmask)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, newFlagsBitmask);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00037EC8 File Offset: 0x000360C8
		[NetInvokableGeneratedMethod("ReceiveBattlEyeLogsRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBattlEyeLogsRequest_Read(in ServerInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			if (!context.IsOwnerOf(player.channel))
			{
				context.Kick(string.Format("not owner of {0}", player));
				return;
			}
			player.ReceiveBattlEyeLogsRequest();
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00037F27 File Offset: 0x00036127
		[NetInvokableGeneratedMethod("ReceiveBattlEyeLogsRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBattlEyeLogsRequest_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00037F2C File Offset: 0x0003612C
		[NetInvokableGeneratedMethod("ReceiveTerminalRelay", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTerminalRelay_Read(in ClientInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			string internalMessage;
			SystemNetPakReaderEx.ReadString(reader, ref internalMessage, 11);
			player.ReceiveTerminalRelay(internalMessage);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00037F7A File Offset: 0x0003617A
		[NetInvokableGeneratedMethod("ReceiveTerminalRelay", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTerminalRelay_Write(NetPakWriter writer, string internalMessage)
		{
			SystemNetPakWriterEx.WriteString(writer, internalMessage, 11);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00037F88 File Offset: 0x00036188
		[NetInvokableGeneratedMethod("ReceiveTeleport", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTeleport_Read(in ClientInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			Vector3 position;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref position, 13, 7);
			byte angle;
			SystemNetPakReaderEx.ReadUInt8(reader, ref angle);
			player.ReceiveTeleport(position, angle);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00037FE2 File Offset: 0x000361E2
		[NetInvokableGeneratedMethod("ReceiveTeleport", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTeleport_Write(NetPakWriter writer, Vector3 position, byte angle)
		{
			UnityNetPakWriterEx.WriteClampedVector3(writer, position, 13, 7);
			SystemNetPakWriterEx.WriteUInt8(writer, angle);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00037FF8 File Offset: 0x000361F8
		[NetInvokableGeneratedMethod("ReceiveStat", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveStat_Read(in ClientInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			EPlayerStat stat;
			reader.ReadEnum(out stat);
			player.ReceiveStat(stat);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x00038044 File Offset: 0x00036244
		[NetInvokableGeneratedMethod("ReceiveStat", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveStat_Write(NetPakWriter writer, EPlayerStat stat)
		{
			writer.WriteEnum(stat);
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x00038050 File Offset: 0x00036250
		[NetInvokableGeneratedMethod("ReceiveAchievementUnlocked", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAchievementUnlocked_Read(in ClientInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			string id;
			SystemNetPakReaderEx.ReadString(reader, ref id, 11);
			player.ReceiveAchievementUnlocked(id);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0003809E File Offset: 0x0003629E
		[NetInvokableGeneratedMethod("ReceiveAchievementUnlocked", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAchievementUnlocked_Write(NetPakWriter writer, string id)
		{
			SystemNetPakWriterEx.WriteString(writer, id, 11);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x000380AC File Offset: 0x000362AC
		[NetInvokableGeneratedMethod("ReceiveUIMessage", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUIMessage_Read(in ClientInvocationContext context)
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
			Player player = obj as Player;
			if (player == null)
			{
				return;
			}
			EPlayerMessage message;
			reader.ReadEnum(out message);
			player.ReceiveUIMessage(message);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x000380F8 File Offset: 0x000362F8
		[NetInvokableGeneratedMethod("ReceiveUIMessage", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUIMessage_Write(NetPakWriter writer, EPlayerMessage message)
		{
			writer.WriteEnum(message);
		}
	}
}
