using System;
using SDG.NetPak;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001FF RID: 511
	[NetInvokableGeneratedClass(typeof(PlayerQuests))]
	public static class PlayerQuests_NetMethods
	{
		// Token: 0x06000F9E RID: 3998 RVA: 0x000367E0 File Offset: 0x000349E0
		[NetInvokableGeneratedMethod("ReceiveCutsceneMode", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveCutsceneMode_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			bool newCutsceneMode;
			reader.ReadBit(ref newCutsceneMode);
			playerQuests.ReceiveCutsceneMode(newCutsceneMode);
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0003682C File Offset: 0x00034A2C
		[NetInvokableGeneratedMethod("ReceiveCutsceneMode", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveCutsceneMode_Write(NetPakWriter writer, bool newCutsceneMode)
		{
			writer.WriteBit(newCutsceneMode);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00036838 File Offset: 0x00034A38
		[NetInvokableGeneratedMethod("ReceiveMarkerState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveMarkerState_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			bool newIsMarkerPlaced;
			reader.ReadBit(ref newIsMarkerPlaced);
			Vector3 newMarkerPosition;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newMarkerPosition, 13, 7);
			string newMarkerTextOverride;
			SystemNetPakReaderEx.ReadString(reader, ref newMarkerTextOverride, 11);
			playerQuests.ReceiveMarkerState(newIsMarkerPlaced, newMarkerPosition, newMarkerTextOverride);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0003689F File Offset: 0x00034A9F
		[NetInvokableGeneratedMethod("ReceiveMarkerState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveMarkerState_Write(NetPakWriter writer, bool newIsMarkerPlaced, Vector3 newMarkerPosition, string newMarkerTextOverride)
		{
			writer.WriteBit(newIsMarkerPlaced);
			UnityNetPakWriterEx.WriteClampedVector3(writer, newMarkerPosition, 13, 7);
			SystemNetPakWriterEx.WriteString(writer, newMarkerTextOverride, 11);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x000368C0 File Offset: 0x00034AC0
		[NetInvokableGeneratedMethod("ReceiveSetMarkerRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSetMarkerRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			bool newIsMarkerPlaced;
			reader.ReadBit(ref newIsMarkerPlaced);
			Vector3 newMarkerPosition;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref newMarkerPosition, 13, 7);
			playerQuests.ReceiveSetMarkerRequest(newIsMarkerPlaced, newMarkerPosition);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0003693A File Offset: 0x00034B3A
		[NetInvokableGeneratedMethod("ReceiveSetMarkerRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSetMarkerRequest_Write(NetPakWriter writer, bool newIsMarkerPlaced, Vector3 newMarkerPosition)
		{
			writer.WriteBit(newIsMarkerPlaced);
			UnityNetPakWriterEx.WriteClampedVector3(writer, newMarkerPosition, 13, 7);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00036950 File Offset: 0x00034B50
		[NetInvokableGeneratedMethod("ReceiveRadioFrequencyState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveRadioFrequencyState_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			uint newRadioFrequency;
			SystemNetPakReaderEx.ReadUInt32(reader, ref newRadioFrequency);
			playerQuests.ReceiveRadioFrequencyState(newRadioFrequency);
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0003699C File Offset: 0x00034B9C
		[NetInvokableGeneratedMethod("ReceiveRadioFrequencyState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveRadioFrequencyState_Write(NetPakWriter writer, uint newRadioFrequency)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, newRadioFrequency);
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x000369A8 File Offset: 0x00034BA8
		[NetInvokableGeneratedMethod("ReceiveSetRadioFrequencyRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSetRadioFrequencyRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			uint newRadioFrequency;
			SystemNetPakReaderEx.ReadUInt32(reader, ref newRadioFrequency);
			playerQuests.ReceiveSetRadioFrequencyRequest(newRadioFrequency);
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00036A14 File Offset: 0x00034C14
		[NetInvokableGeneratedMethod("ReceiveSetRadioFrequencyRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSetRadioFrequencyRequest_Write(NetPakWriter writer, uint newRadioFrequency)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, newRadioFrequency);
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00036A20 File Offset: 0x00034C20
		[NetInvokableGeneratedMethod("ReceiveGroupState", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveGroupState_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			CSteamID newGroupID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref newGroupID);
			EPlayerGroupRank newGroupRank;
			reader.ReadEnum(out newGroupRank);
			playerQuests.ReceiveGroupState(newGroupID, newGroupRank);
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00036A77 File Offset: 0x00034C77
		[NetInvokableGeneratedMethod("ReceiveGroupState", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveGroupState_Write(NetPakWriter writer, CSteamID newGroupID, EPlayerGroupRank newGroupRank)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, newGroupID);
			writer.WriteEnum(newGroupRank);
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00036A8C File Offset: 0x00034C8C
		[NetInvokableGeneratedMethod("ReceiveAcceptGroupInvitationRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAcceptGroupInvitationRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			CSteamID newGroupID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref newGroupID);
			playerQuests.ReceiveAcceptGroupInvitationRequest(newGroupID);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00036AF8 File Offset: 0x00034CF8
		[NetInvokableGeneratedMethod("ReceiveAcceptGroupInvitationRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAcceptGroupInvitationRequest_Write(NetPakWriter writer, CSteamID newGroupID)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, newGroupID);
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00036B04 File Offset: 0x00034D04
		[NetInvokableGeneratedMethod("ReceiveDeclineGroupInvitationRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDeclineGroupInvitationRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			CSteamID newGroupID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref newGroupID);
			playerQuests.ReceiveDeclineGroupInvitationRequest(newGroupID);
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00036B70 File Offset: 0x00034D70
		[NetInvokableGeneratedMethod("ReceiveDeclineGroupInvitationRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDeclineGroupInvitationRequest_Write(NetPakWriter writer, CSteamID newGroupID)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, newGroupID);
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x00036B7C File Offset: 0x00034D7C
		[NetInvokableGeneratedMethod("ReceiveLeaveGroupRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveLeaveGroupRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			playerQuests.ReceiveLeaveGroupRequest();
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00036BDB File Offset: 0x00034DDB
		[NetInvokableGeneratedMethod("ReceiveLeaveGroupRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveLeaveGroupRequest_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00036BE0 File Offset: 0x00034DE0
		[NetInvokableGeneratedMethod("ReceiveDeleteGroupRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDeleteGroupRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			playerQuests.ReceiveDeleteGroupRequest();
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00036C3F File Offset: 0x00034E3F
		[NetInvokableGeneratedMethod("ReceiveDeleteGroupRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDeleteGroupRequest_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00036C44 File Offset: 0x00034E44
		[NetInvokableGeneratedMethod("ReceiveCreateGroupRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveCreateGroupRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			playerQuests.ReceiveCreateGroupRequest();
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00036CA3 File Offset: 0x00034EA3
		[NetInvokableGeneratedMethod("ReceiveCreateGroupRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveCreateGroupRequest_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00036CA8 File Offset: 0x00034EA8
		[NetInvokableGeneratedMethod("ReceiveAddGroupInviteClient", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAddGroupInviteClient_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			CSteamID newGroupID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref newGroupID);
			playerQuests.ReceiveAddGroupInviteClient(newGroupID);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00036CF4 File Offset: 0x00034EF4
		[NetInvokableGeneratedMethod("ReceiveAddGroupInviteClient", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAddGroupInviteClient_Write(NetPakWriter writer, CSteamID newGroupID)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, newGroupID);
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00036D00 File Offset: 0x00034F00
		[NetInvokableGeneratedMethod("ReceiveRemoveGroupInviteClient", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveRemoveGroupInviteClient_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			CSteamID newGroupID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref newGroupID);
			playerQuests.ReceiveRemoveGroupInviteClient(newGroupID);
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00036D4C File Offset: 0x00034F4C
		[NetInvokableGeneratedMethod("ReceiveRemoveGroupInviteClient", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveRemoveGroupInviteClient_Write(NetPakWriter writer, CSteamID newGroupID)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, newGroupID);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00036D58 File Offset: 0x00034F58
		[NetInvokableGeneratedMethod("ReceiveAddGroupInviteRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAddGroupInviteRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			CSteamID targetID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref targetID);
			playerQuests.ReceiveAddGroupInviteRequest(targetID);
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00036DC4 File Offset: 0x00034FC4
		[NetInvokableGeneratedMethod("ReceiveAddGroupInviteRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAddGroupInviteRequest_Write(NetPakWriter writer, CSteamID targetID)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, targetID);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00036DD0 File Offset: 0x00034FD0
		[NetInvokableGeneratedMethod("ReceivePromoteRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePromoteRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			CSteamID targetID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref targetID);
			playerQuests.ReceivePromoteRequest(targetID);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00036E3C File Offset: 0x0003503C
		[NetInvokableGeneratedMethod("ReceivePromoteRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePromoteRequest_Write(NetPakWriter writer, CSteamID targetID)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, targetID);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00036E48 File Offset: 0x00035048
		[NetInvokableGeneratedMethod("ReceiveDemoteRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveDemoteRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			CSteamID targetID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref targetID);
			playerQuests.ReceiveDemoteRequest(targetID);
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00036EB4 File Offset: 0x000350B4
		[NetInvokableGeneratedMethod("ReceiveDemoteRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveDemoteRequest_Write(NetPakWriter writer, CSteamID targetID)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, targetID);
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00036EC0 File Offset: 0x000350C0
		[NetInvokableGeneratedMethod("ReceiveKickFromGroup", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveKickFromGroup_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			CSteamID targetID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref targetID);
			playerQuests.ReceiveKickFromGroup(targetID);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00036F2C File Offset: 0x0003512C
		[NetInvokableGeneratedMethod("ReceiveKickFromGroup", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveKickFromGroup_Write(NetPakWriter writer, CSteamID targetID)
		{
			SteamworksNetPakWriterEx.WriteSteamID(writer, targetID);
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00036F38 File Offset: 0x00035138
		[NetInvokableGeneratedMethod("ReceiveRenameGroupRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveRenameGroupRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			string newName;
			SystemNetPakReaderEx.ReadString(reader, ref newName, 11);
			playerQuests.ReceiveRenameGroupRequest(newName);
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00036FA6 File Offset: 0x000351A6
		[NetInvokableGeneratedMethod("ReceiveRenameGroupRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveRenameGroupRequest_Write(NetPakWriter writer, string newName)
		{
			SystemNetPakWriterEx.WriteString(writer, newName, 11);
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00036FB4 File Offset: 0x000351B4
		[NetInvokableGeneratedMethod("ReceiveSellToVendor", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSellToVendor_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			bool asManyAsPossible;
			reader.ReadBit(ref asManyAsPossible);
			playerQuests.ReceiveSellToVendor(context, assetGuid, index, asManyAsPossible);
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00037037 File Offset: 0x00035237
		[NetInvokableGeneratedMethod("ReceiveSellToVendor", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSellToVendor_Write(NetPakWriter writer, Guid assetGuid, byte index, bool asManyAsPossible)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			SystemNetPakWriterEx.WriteUInt8(writer, index);
			writer.WriteBit(asManyAsPossible);
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00037054 File Offset: 0x00035254
		[NetInvokableGeneratedMethod("ReceiveBuyFromVendor", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBuyFromVendor_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			bool asManyAsPossible;
			reader.ReadBit(ref asManyAsPossible);
			playerQuests.ReceiveBuyFromVendor(context, assetGuid, index, asManyAsPossible);
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x000370D7 File Offset: 0x000352D7
		[NetInvokableGeneratedMethod("ReceiveBuyFromVendor", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBuyFromVendor_Write(NetPakWriter writer, Guid assetGuid, byte index, bool asManyAsPossible)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			SystemNetPakWriterEx.WriteUInt8(writer, index);
			writer.WriteBit(asManyAsPossible);
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x000370F4 File Offset: 0x000352F4
		[NetInvokableGeneratedMethod("ReceiveSetFlag", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSetFlag_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			short value;
			SystemNetPakReaderEx.ReadInt16(reader, ref value);
			playerQuests.ReceiveSetFlag(id, value);
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0003714B File Offset: 0x0003534B
		[NetInvokableGeneratedMethod("ReceiveSetFlag", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSetFlag_Write(NetPakWriter writer, ushort id, short value)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, id);
			SystemNetPakWriterEx.WriteInt16(writer, value);
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00037160 File Offset: 0x00035360
		[NetInvokableGeneratedMethod("ReceiveRemoveFlag", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveRemoveFlag_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			playerQuests.ReceiveRemoveFlag(id);
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x000371AC File Offset: 0x000353AC
		[NetInvokableGeneratedMethod("ReceiveRemoveFlag", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveRemoveFlag_Write(NetPakWriter writer, ushort id)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, id);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x000371B8 File Offset: 0x000353B8
		[NetInvokableGeneratedMethod("ReceiveAddQuest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAddQuest_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			playerQuests.ReceiveAddQuest(assetGuid);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00037204 File Offset: 0x00035404
		[NetInvokableGeneratedMethod("ReceiveAddQuest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAddQuest_Write(NetPakWriter writer, Guid assetGuid)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00037210 File Offset: 0x00035410
		[NetInvokableGeneratedMethod("ReceiveRemoveQuest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveRemoveQuest_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			bool wasCompleted;
			reader.ReadBit(ref wasCompleted);
			playerQuests.ReceiveRemoveQuest(assetGuid, wasCompleted);
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00037267 File Offset: 0x00035467
		[NetInvokableGeneratedMethod("ReceiveRemoveQuest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveRemoveQuest_Write(NetPakWriter writer, Guid assetGuid, bool wasCompleted)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			writer.WriteBit(wasCompleted);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0003727C File Offset: 0x0003547C
		[NetInvokableGeneratedMethod("ReceiveTrackQuest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTrackQuest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			playerQuests.ReceiveTrackQuest(assetGuid);
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x000372E8 File Offset: 0x000354E8
		[NetInvokableGeneratedMethod("ReceiveTrackQuest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTrackQuest_Write(NetPakWriter writer, Guid assetGuid)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x000372F4 File Offset: 0x000354F4
		[NetInvokableGeneratedMethod("ReceiveAbandonQuestRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveAbandonQuestRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			playerQuests.ReceiveAbandonQuestRequest(assetGuid);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00037360 File Offset: 0x00035560
		[NetInvokableGeneratedMethod("ReceiveAbandonQuestRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveAbandonQuestRequest_Write(NetPakWriter writer, Guid assetGuid)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0003736C File Offset: 0x0003556C
		[NetInvokableGeneratedMethod("ReceiveChooseDialogueResponseRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveChooseDialogueResponseRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			playerQuests.ReceiveChooseDialogueResponseRequest(context, assetGuid, index);
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x000373E4 File Offset: 0x000355E4
		[NetInvokableGeneratedMethod("ReceiveChooseDialogueResponseRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveChooseDialogueResponseRequest_Write(NetPakWriter writer, Guid assetGuid, byte index)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			SystemNetPakWriterEx.WriteUInt8(writer, index);
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x000373F8 File Offset: 0x000355F8
		[NetInvokableGeneratedMethod("ReceiveChooseDefaultNextDialogueRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveChooseDefaultNextDialogueRequest_Read(in ServerInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerQuests.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerQuests));
				return;
			}
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			playerQuests.ReceiveChooseDefaultNextDialogueRequest(context, assetGuid, index);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00037470 File Offset: 0x00035670
		[NetInvokableGeneratedMethod("ReceiveChooseDefaultNextDialogueRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveChooseDefaultNextDialogueRequest_Write(NetPakWriter writer, Guid assetGuid, byte index)
		{
			SystemNetPakWriterEx.WriteGuid(writer, assetGuid);
			SystemNetPakWriterEx.WriteUInt8(writer, index);
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00037484 File Offset: 0x00035684
		[NetInvokableGeneratedMethod("ReceiveQuests", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveQuests_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			playerQuests.ReceiveQuests(context);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x000374C4 File Offset: 0x000356C4
		[NetInvokableGeneratedMethod("ReceiveTalkWithNpcResponse", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveTalkWithNpcResponse_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			NetId targetNpcNetId;
			reader.ReadNetId(out targetNpcNetId);
			Guid dialogueAssetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref dialogueAssetGuid);
			byte messageIndex;
			SystemNetPakReaderEx.ReadUInt8(reader, ref messageIndex);
			bool hasNextDialogue;
			reader.ReadBit(ref hasNextDialogue);
			playerQuests.ReceiveTalkWithNpcResponse(context, targetNpcNetId, dialogueAssetGuid, messageIndex, hasNextDialogue);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00037532 File Offset: 0x00035732
		[NetInvokableGeneratedMethod("ReceiveTalkWithNpcResponse", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveTalkWithNpcResponse_Write(NetPakWriter writer, NetId targetNpcNetId, Guid dialogueAssetGuid, byte messageIndex, bool hasNextDialogue)
		{
			writer.WriteNetId(targetNpcNetId);
			SystemNetPakWriterEx.WriteGuid(writer, dialogueAssetGuid);
			SystemNetPakWriterEx.WriteUInt8(writer, messageIndex);
			writer.WriteBit(hasNextDialogue);
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x00037558 File Offset: 0x00035758
		[NetInvokableGeneratedMethod("ReceiveOpenDialogue", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveOpenDialogue_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			Guid dialogueAssetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref dialogueAssetGuid);
			byte messageIndex;
			SystemNetPakReaderEx.ReadUInt8(reader, ref messageIndex);
			bool hasNextDialogue;
			reader.ReadBit(ref hasNextDialogue);
			playerQuests.ReceiveOpenDialogue(context, dialogueAssetGuid, messageIndex, hasNextDialogue);
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x000375BB File Offset: 0x000357BB
		[NetInvokableGeneratedMethod("ReceiveOpenDialogue", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveOpenDialogue_Write(NetPakWriter writer, Guid dialogueAssetGuid, byte messageIndex, bool hasNextDialogue)
		{
			SystemNetPakWriterEx.WriteGuid(writer, dialogueAssetGuid);
			SystemNetPakWriterEx.WriteUInt8(writer, messageIndex);
			writer.WriteBit(hasNextDialogue);
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x000375D8 File Offset: 0x000357D8
		[NetInvokableGeneratedMethod("ReceiveOpenVendor", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveOpenVendor_Read(in ClientInvocationContext context)
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
			PlayerQuests playerQuests = obj as PlayerQuests;
			if (playerQuests == null)
			{
				return;
			}
			Guid vendorAssetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref vendorAssetGuid);
			Guid dialogueAssetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref dialogueAssetGuid);
			byte messageIndex;
			SystemNetPakReaderEx.ReadUInt8(reader, ref messageIndex);
			bool hasNextDialogue;
			reader.ReadBit(ref hasNextDialogue);
			playerQuests.ReceiveOpenVendor(context, vendorAssetGuid, dialogueAssetGuid, messageIndex, hasNextDialogue);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00037646 File Offset: 0x00035846
		[NetInvokableGeneratedMethod("ReceiveOpenVendor", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveOpenVendor_Write(NetPakWriter writer, Guid vendorAssetGuid, Guid dialogueAssetGuid, byte messageIndex, bool hasNextDialogue)
		{
			SystemNetPakWriterEx.WriteGuid(writer, vendorAssetGuid);
			SystemNetPakWriterEx.WriteGuid(writer, dialogueAssetGuid);
			SystemNetPakWriterEx.WriteUInt8(writer, messageIndex);
			writer.WriteBit(hasNextDialogue);
		}
	}
}
