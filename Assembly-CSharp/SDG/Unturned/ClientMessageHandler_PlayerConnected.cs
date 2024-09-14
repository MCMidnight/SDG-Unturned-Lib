using System;
using System.Collections.Generic;
using SDG.NetPak;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200026F RID: 623
	internal static class ClientMessageHandler_PlayerConnected
	{
		// Token: 0x0600126F RID: 4719 RVA: 0x0003FBCC File Offset: 0x0003DDCC
		internal static void ReadMessage(NetPakReader reader)
		{
			NetId netId;
			reader.ReadNetId(out netId);
			CSteamID newSteamID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref newSteamID);
			byte newCharacterID;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newCharacterID);
			string newPlayerName;
			SystemNetPakReaderEx.ReadString(reader, ref newPlayerName, 11);
			string newCharacterName;
			SystemNetPakReaderEx.ReadString(reader, ref newCharacterName, 11);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 7);
			byte angle;
			SystemNetPakReaderEx.ReadUInt8(reader, ref angle);
			bool isPro;
			reader.ReadBit(ref isPro);
			bool isAdmin;
			reader.ReadBit(ref isAdmin);
			byte channel;
			SystemNetPakReaderEx.ReadUInt8(reader, ref channel);
			CSteamID newGroup;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref newGroup);
			string newNickName;
			SystemNetPakReaderEx.ReadString(reader, ref newNickName, 11);
			byte face;
			SystemNetPakReaderEx.ReadUInt8(reader, ref face);
			byte hair;
			SystemNetPakReaderEx.ReadUInt8(reader, ref hair);
			byte beard;
			SystemNetPakReaderEx.ReadUInt8(reader, ref beard);
			Color32 c;
			UnityNetPakReaderEx.ReadColor32RGB(reader, ref c);
			Color32 c2;
			UnityNetPakReaderEx.ReadColor32RGB(reader, ref c2);
			Color32 c3;
			UnityNetPakReaderEx.ReadColor32RGB(reader, ref c3);
			bool hand;
			reader.ReadBit(ref hand);
			int shirtItem;
			SystemNetPakReaderEx.ReadInt32(reader, ref shirtItem);
			int pantsItem;
			SystemNetPakReaderEx.ReadInt32(reader, ref pantsItem);
			int hatItem;
			SystemNetPakReaderEx.ReadInt32(reader, ref hatItem);
			int backpackItem;
			SystemNetPakReaderEx.ReadInt32(reader, ref backpackItem);
			int vestItem;
			SystemNetPakReaderEx.ReadInt32(reader, ref vestItem);
			int maskItem;
			SystemNetPakReaderEx.ReadInt32(reader, ref maskItem);
			int glassesItem;
			SystemNetPakReaderEx.ReadInt32(reader, ref glassesItem);
			ClientMessageHandler_PlayerConnected.skinItems.Clear();
			SystemNetPakReaderEx.ReadList<int>(reader, ClientMessageHandler_PlayerConnected.skinItems, delegate(out int item)
			{
				return SystemNetPakReaderEx.ReadInt32(reader, ref item);
			}, ClientMessageHandler_PlayerConnected.MAX_LENGTH);
			ClientMessageHandler_PlayerConnected.skinTags.Clear();
			SystemNetPakReaderEx.ReadList<string>(reader, ClientMessageHandler_PlayerConnected.skinTags, delegate(out string tag)
			{
				return SystemNetPakReaderEx.ReadString(reader, ref tag, 11);
			}, ClientMessageHandler_PlayerConnected.MAX_LENGTH);
			ClientMessageHandler_PlayerConnected.skinDynamicProps.Clear();
			SystemNetPakReaderEx.ReadList<string>(reader, ClientMessageHandler_PlayerConnected.skinDynamicProps, delegate(out string dynProp)
			{
				return SystemNetPakReaderEx.ReadString(reader, ref dynProp, 11);
			}, ClientMessageHandler_PlayerConnected.MAX_LENGTH);
			EPlayerSkillset skillset;
			reader.ReadEnum(out skillset);
			string language;
			SystemNetPakReaderEx.ReadString(reader, ref language, 11);
			Provider.addPlayer(null, netId, new SteamPlayerID(newSteamID, newCharacterID, newPlayerName, newCharacterName, newNickName, newGroup), point, angle, isPro, isAdmin, (int)channel, face, hair, beard, c, c2, c3, hand, shirtItem, pantsItem, hatItem, backpackItem, vestItem, maskItem, glassesItem, ClientMessageHandler_PlayerConnected.skinItems.ToArray(), ClientMessageHandler_PlayerConnected.skinTags.ToArray(), ClientMessageHandler_PlayerConnected.skinDynamicProps.ToArray(), skillset, language, CSteamID.Nil, EClientPlatform.Windows).player.InitializePlayer();
		}

		// Token: 0x040005D3 RID: 1491
		private static List<int> skinItems = new List<int>();

		// Token: 0x040005D4 RID: 1492
		private static List<string> skinTags = new List<string>();

		// Token: 0x040005D5 RID: 1493
		private static List<string> skinDynamicProps = new List<string>();

		// Token: 0x040005D6 RID: 1494
		private static readonly NetLength MAX_LENGTH = new NetLength(255U);
	}
}
