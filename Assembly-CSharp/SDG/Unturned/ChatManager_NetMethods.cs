using System;
using SDG.NetPak;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001D5 RID: 469
	[NetInvokableGeneratedClass(typeof(ChatManager))]
	public static class ChatManager_NetMethods
	{
		// Token: 0x06000E29 RID: 3625 RVA: 0x00031728 File Offset: 0x0002F928
		[NetInvokableGeneratedMethod("ReceiveVoteStart", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVoteStart_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			CSteamID origin;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref origin);
			CSteamID target;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref target);
			byte votesNeeded;
			SystemNetPakReaderEx.ReadUInt8(reader, ref votesNeeded);
			ChatManager.ReceiveVoteStart(origin, target, votesNeeded);
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0003175D File Offset: 0x0002F95D
		[NetInvokableGeneratedMethod("ReceiveVoteStart", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVoteStart_Write(NetPakWriter writer, CSteamID origin, CSteamID target, byte votesNeeded)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, origin);
			SteamworksNetPakWriterEx.WriteSteamID(writer, target);
			SystemNetPakWriterEx.WriteUInt8(writer, votesNeeded);
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00031778 File Offset: 0x0002F978
		[NetInvokableGeneratedMethod("ReceiveVoteUpdate", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVoteUpdate_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte voteYes;
			SystemNetPakReaderEx.ReadUInt8(reader, ref voteYes);
			byte voteNo;
			SystemNetPakReaderEx.ReadUInt8(reader, ref voteNo);
			ChatManager.ReceiveVoteUpdate(voteYes, voteNo);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x000317A3 File Offset: 0x0002F9A3
		[NetInvokableGeneratedMethod("ReceiveVoteUpdate", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVoteUpdate_Write(NetPakWriter writer, byte voteYes, byte voteNo)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, voteYes);
			SystemNetPakWriterEx.WriteUInt8(writer, voteNo);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x000317B8 File Offset: 0x0002F9B8
		[NetInvokableGeneratedMethod("ReceiveVoteStop", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVoteStop_Read(in ClientInvocationContext context)
		{
			EVotingMessage message;
			context.reader.ReadEnum(out message);
			ChatManager.ReceiveVoteStop(message);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x000317D9 File Offset: 0x0002F9D9
		[NetInvokableGeneratedMethod("ReceiveVoteStop", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVoteStop_Write(NetPakWriter writer, EVotingMessage message)
		{
			writer.WriteEnum(message);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x000317E4 File Offset: 0x0002F9E4
		[NetInvokableGeneratedMethod("ReceiveVoteMessage", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveVoteMessage_Read(in ClientInvocationContext context)
		{
			EVotingMessage message;
			context.reader.ReadEnum(out message);
			ChatManager.ReceiveVoteMessage(message);
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00031805 File Offset: 0x0002FA05
		[NetInvokableGeneratedMethod("ReceiveVoteMessage", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveVoteMessage_Write(NetPakWriter writer, EVotingMessage message)
		{
			writer.WriteEnum(message);
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00031810 File Offset: 0x0002FA10
		[NetInvokableGeneratedMethod("ReceiveSubmitVoteRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSubmitVoteRequest_Read(in ServerInvocationContext context)
		{
			bool vote;
			context.reader.ReadBit(ref vote);
			ChatManager.ReceiveSubmitVoteRequest(context, vote);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00031832 File Offset: 0x0002FA32
		[NetInvokableGeneratedMethod("ReceiveSubmitVoteRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSubmitVoteRequest_Write(NetPakWriter writer, bool vote)
		{
			writer.WriteBit(vote);
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0003183C File Offset: 0x0002FA3C
		[NetInvokableGeneratedMethod("ReceiveCallVoteRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveCallVoteRequest_Read(in ServerInvocationContext context)
		{
			CSteamID target;
			SteamworksNetPakReaderEx.ReadSteamID(context.reader, ref target);
			ChatManager.ReceiveCallVoteRequest(context, target);
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0003185E File Offset: 0x0002FA5E
		[NetInvokableGeneratedMethod("ReceiveCallVoteRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveCallVoteRequest_Write(NetPakWriter writer, CSteamID target)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, target);
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00031868 File Offset: 0x0002FA68
		[NetInvokableGeneratedMethod("ReceiveChatEntry", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveChatEntry_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			CSteamID owner;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref owner);
			string iconURL;
			SystemNetPakReaderEx.ReadString(reader, ref iconURL, 11);
			EChatMode mode;
			reader.ReadEnum(out mode);
			Color color;
			UnityNetPakReaderEx.ReadColor32RGB(reader, ref color);
			bool rich;
			reader.ReadBit(ref rich);
			string text;
			SystemNetPakReaderEx.ReadString(reader, ref text, 11);
			ChatManager.ReceiveChatEntry(owner, iconURL, mode, color, rich, text);
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x000318C1 File Offset: 0x0002FAC1
		[NetInvokableGeneratedMethod("ReceiveChatEntry", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveChatEntry_Write(NetPakWriter writer, CSteamID owner, string iconURL, EChatMode mode, Color color, bool rich, string text)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, owner);
			SystemNetPakWriterEx.WriteString(writer, iconURL, 11);
			writer.WriteEnum(mode);
			UnityNetPakWriterEx.WriteColor32RGB(writer, color);
			writer.WriteBit(rich);
			SystemNetPakWriterEx.WriteString(writer, text, 11);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00031900 File Offset: 0x0002FB00
		[NetInvokableGeneratedMethod("ReceiveChatRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveChatRequest_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte flags;
			SystemNetPakReaderEx.ReadUInt8(reader, ref flags);
			string text;
			SystemNetPakReaderEx.ReadString(reader, ref text, 11);
			ChatManager.ReceiveChatRequest(context, flags, text);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0003192E File Offset: 0x0002FB2E
		[NetInvokableGeneratedMethod("ReceiveChatRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveChatRequest_Write(NetPakWriter writer, byte flags, string text)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, flags);
			SystemNetPakWriterEx.WriteString(writer, text, 11);
		}
	}
}
