using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using SDG.Framework.Modules;
using SDG.NetPak;
using SDG.NetTransport;
using SDG.Provider;
using Steamworks;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200027F RID: 639
	internal static class ServerMessageHandler_ReadyToConnect
	{
		// Token: 0x0600128B RID: 4747 RVA: 0x000412F1 File Offset: 0x0003F4F1
		[Conditional("LOG_CONNECT_ARGS")]
		private static void LogRead(string key, object value)
		{
			UnturnedLog.info("{0} = {1}", new object[]
			{
				key,
				value
			});
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0004130C File Offset: 0x0003F50C
		internal static void ReadMessage(ITransportConnection transportConnection, NetPakReader reader)
		{
			if (Provider.findPendingPlayer(transportConnection) != null)
			{
				Provider.reject(transportConnection, ESteamRejection.ALREADY_PENDING);
				return;
			}
			if (Provider.findPlayer(transportConnection) != null)
			{
				Provider.reject(transportConnection, ESteamRejection.ALREADY_CONNECTED);
				return;
			}
			byte newCharacterID;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newCharacterID);
			string text;
			SystemNetPakReaderEx.ReadString(reader, ref text, 11);
			text = text.Trim();
			string text2;
			SystemNetPakReaderEx.ReadString(reader, ref text2, 11);
			text2 = text2.Trim();
			byte[] array = new byte[20];
			reader.ReadBytes(array, 20);
			byte[] array2 = new byte[20];
			reader.ReadBytes(array2, 20);
			byte[] array3 = new byte[20];
			reader.ReadBytes(array3, 20);
			byte[] array4 = new byte[20];
			reader.ReadBytes(array4, 20);
			EClientPlatform clientPlatform;
			reader.ReadEnum(out clientPlatform);
			uint version;
			SystemNetPakReaderEx.ReadUInt32(reader, ref version);
			bool newPro;
			reader.ReadBit(ref newPro);
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num);
			string text3;
			SystemNetPakReaderEx.ReadString(reader, ref text3, 11);
			text3 = text3.Trim();
			CSteamID newGroup;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref newGroup);
			byte newFace;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newFace);
			byte newHair;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newHair);
			byte newBeard;
			SystemNetPakReaderEx.ReadUInt8(reader, ref newBeard);
			Color32 color;
			UnityNetPakReaderEx.ReadColor32RGB(reader, ref color);
			Color32 c;
			UnityNetPakReaderEx.ReadColor32RGB(reader, ref c);
			Color32 c2;
			UnityNetPakReaderEx.ReadColor32RGB(reader, ref c2);
			bool newHand;
			reader.ReadBit(ref newHand);
			ulong newPackageShirt;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newPackageShirt);
			ulong newPackagePants;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newPackagePants);
			ulong newPackageHat;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newPackageHat);
			ulong newPackageBackpack;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newPackageBackpack);
			ulong newPackageVest;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newPackageVest);
			ulong newPackageMask;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newPackageMask);
			ulong newPackageGlasses;
			SystemNetPakReaderEx.ReadUInt64(reader, ref newPackageGlasses);
			ServerMessageHandler_ReadyToConnect.pendingPackageSkins.Clear();
			SystemNetPakReaderEx.ReadList<ulong>(reader, ServerMessageHandler_ReadyToConnect.pendingPackageSkins, new SystemNetPakReaderEx.ReadListItem<ulong>(reader.ReadUInt64), Provider.MAX_SKINS_LENGTH);
			EPlayerSkillset newSkillset;
			reader.ReadEnum(out newSkillset);
			string text4;
			SystemNetPakReaderEx.ReadString(reader, ref text4, 11);
			string newLanguage;
			SystemNetPakReaderEx.ReadString(reader, ref newLanguage, 11);
			CSteamID newLobbyID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref newLobbyID);
			uint num2;
			SystemNetPakReaderEx.ReadUInt32(reader, ref num2);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			if (b > 8)
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_HASH_ASSEMBLY);
				return;
			}
			byte[][] array5 = new byte[(int)b][];
			for (byte b2 = 0; b2 < b; b2 += 1)
			{
				array5[(int)b2] = new byte[20];
				reader.ReadBytes(array5[(int)b2]);
			}
			byte[] array6 = new byte[20];
			reader.ReadBytes(array6, 20);
			CSteamID csteamID;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref csteamID);
			if (Provider.findPendingPlayerBySteamId(csteamID) != null)
			{
				Provider.reject(transportConnection, ESteamRejection.ALREADY_PENDING);
				return;
			}
			if (PlayerTool.getSteamPlayer(csteamID) != null)
			{
				Provider.reject(transportConnection, ESteamRejection.ALREADY_CONNECTED);
				return;
			}
			if (!Provider.modeConfigData.Players.Allow_Per_Character_Saves)
			{
				newCharacterID = 0;
			}
			SteamPlayerID steamPlayerID = new SteamPlayerID(csteamID, newCharacterID, text, text2, text3, newGroup, array5);
			if (!Provider.canClientVersionJoinServer(version))
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_VERSION, Provider.APP_VERSION);
				return;
			}
			if (num2 != Level.packedVersion)
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_LEVEL_VERSION, Level.version);
				return;
			}
			if (string.IsNullOrWhiteSpace(steamPlayerID.playerName) || NameTool.containsRichText(steamPlayerID.playerName) || StringExtension.ContainsNewLine(steamPlayerID.playerName))
			{
				Provider.reject(transportConnection, ESteamRejection.NAME_PLAYER_INVALID);
				return;
			}
			if (string.IsNullOrWhiteSpace(steamPlayerID.characterName) || NameTool.containsRichText(steamPlayerID.characterName) || StringExtension.ContainsNewLine(steamPlayerID.characterName))
			{
				Provider.reject(transportConnection, ESteamRejection.NAME_CHARACTER_INVALID);
				return;
			}
			if (string.IsNullOrWhiteSpace(steamPlayerID.nickName) || NameTool.containsRichText(steamPlayerID.nickName) || StringExtension.ContainsNewLine(steamPlayerID.nickName))
			{
				Provider.reject(transportConnection, ESteamRejection.NAME_PRIVATE_INVALID);
				return;
			}
			if (steamPlayerID.playerName.Length < 2)
			{
				Provider.reject(transportConnection, ESteamRejection.NAME_PLAYER_SHORT);
				return;
			}
			if (steamPlayerID.characterName.Length < 2)
			{
				Provider.reject(transportConnection, ESteamRejection.NAME_CHARACTER_SHORT);
				return;
			}
			if (steamPlayerID.playerName.Length > 32)
			{
				Provider.reject(transportConnection, ESteamRejection.NAME_PLAYER_LONG);
				return;
			}
			if (steamPlayerID.characterName.Length > 32)
			{
				Provider.reject(transportConnection, ESteamRejection.NAME_CHARACTER_LONG);
				return;
			}
			if (steamPlayerID.nickName.Length > 32)
			{
				Provider.reject(transportConnection, ESteamRejection.NAME_PRIVATE_LONG);
				return;
			}
			long num3;
			double num4;
			if (long.TryParse(steamPlayerID.playerName, 511, CultureInfo.InvariantCulture, ref num3) || double.TryParse(steamPlayerID.playerName, 511, CultureInfo.InvariantCulture, ref num4))
			{
				Provider.reject(transportConnection, ESteamRejection.NAME_PLAYER_NUMBER);
				return;
			}
			long num5;
			double num6;
			if (long.TryParse(steamPlayerID.characterName, 511, CultureInfo.InvariantCulture, ref num5) || double.TryParse(steamPlayerID.characterName, 511, CultureInfo.InvariantCulture, ref num6))
			{
				Provider.reject(transportConnection, ESteamRejection.NAME_CHARACTER_NUMBER);
				return;
			}
			long num7;
			double num8;
			if (long.TryParse(steamPlayerID.nickName, 511, CultureInfo.InvariantCulture, ref num7) || double.TryParse(steamPlayerID.nickName, 511, CultureInfo.InvariantCulture, ref num8))
			{
				Provider.reject(transportConnection, ESteamRejection.NAME_PRIVATE_NUMBER);
				return;
			}
			if (Provider.filterName)
			{
				if (!NameTool.isValid(steamPlayerID.playerName))
				{
					Provider.reject(transportConnection, ESteamRejection.NAME_PLAYER_INVALID);
					return;
				}
				if (!NameTool.isValid(steamPlayerID.characterName))
				{
					Provider.reject(transportConnection, ESteamRejection.NAME_CHARACTER_INVALID);
					return;
				}
				if (!NameTool.isValid(steamPlayerID.nickName))
				{
					Provider.reject(transportConnection, ESteamRejection.NAME_PRIVATE_INVALID);
					return;
				}
			}
			if (steamPlayerID.steamID.m_SteamID != 76561198036822957UL && steamPlayerID.steamID.m_SteamID != 76561198267201306UL)
			{
				if (ServerMessageHandler_ReadyToConnect.IsNameBlockedByNelsonFilter(text))
				{
					Provider.reject(transportConnection, ESteamRejection.NAME_PLAYER_INVALID);
					return;
				}
				if (ServerMessageHandler_ReadyToConnect.IsNameBlockedByNelsonFilter(text2))
				{
					Provider.reject(transportConnection, ESteamRejection.NAME_CHARACTER_INVALID);
					return;
				}
			}
			uint remoteIP;
			transportConnection.TryGetIPv4Address(out remoteIP);
			bool flag;
			string reason;
			uint duration;
			Provider.checkBanStatus(steamPlayerID, remoteIP, out flag, out reason, out duration);
			if (flag)
			{
				Provider.notifyBannedInternal(transportConnection, reason, duration);
				return;
			}
			bool flag2 = SteamWhitelist.checkWhitelisted(csteamID);
			if (Provider.isWhitelisted && !flag2)
			{
				Provider.reject(transportConnection, ESteamRejection.WHITELISTED);
				return;
			}
			if (Provider.clients.Count + 1 > (int)Provider.maxPlayers && Provider.pending.Count + 1 > (int)Provider.queueSize)
			{
				Provider.reject(transportConnection, ESteamRejection.SERVER_FULL);
				return;
			}
			if (array.Length != 20)
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_PASSWORD);
				return;
			}
			if (array2.Length != 20)
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_HASH_LEVEL);
				return;
			}
			if (array3.Length != 20)
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_HASH_ASSEMBLY);
				return;
			}
			if (array4.Length != 20)
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_HASH_RESOURCES);
				return;
			}
			if (Provider.configData.Server.Validate_EconInfo_Hash && !Hash.verifyHash(array6, TempSteamworksEconomy.econInfoHash) && !steamPlayerID.BypassIntegrityChecks)
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_HASH_ECON);
				return;
			}
			ModuleDependency[] array7;
			if (string.IsNullOrEmpty(text4))
			{
				array7 = new ModuleDependency[0];
			}
			else
			{
				string[] array8 = text4.Split(';', 0);
				array7 = new ModuleDependency[array8.Length];
				for (int i = 0; i < array7.Length; i++)
				{
					string[] array9 = array8[i].Split(',', 0);
					if (array9.Length == 2)
					{
						array7[i] = new ModuleDependency();
						array7[i].Name = array9[0];
						uint.TryParse(array9[1], 511, CultureInfo.InvariantCulture, ref array7[i].Version_Internal);
					}
				}
			}
			List<Module> critMods = Provider.critMods;
			Provider.critMods.Clear();
			ModuleHook.getRequiredModules(critMods);
			bool flag3 = true;
			for (int j = 0; j < array7.Length; j++)
			{
				bool flag4 = false;
				if (array7[j] != null)
				{
					for (int k = 0; k < critMods.Count; k++)
					{
						if (critMods[k] != null && critMods[k].config != null && critMods[k].config.Name == array7[j].Name && critMods[k].config.Version_Internal >= array7[j].Version_Internal)
						{
							flag4 = true;
							break;
						}
					}
				}
				if (!flag4)
				{
					flag3 = false;
					break;
				}
			}
			if (!flag3)
			{
				Provider.reject(transportConnection, ESteamRejection.CLIENT_MODULE_DESYNC);
				return;
			}
			bool flag5 = true;
			for (int l = 0; l < critMods.Count; l++)
			{
				bool flag6 = false;
				if (critMods[l] != null && critMods[l].config != null)
				{
					for (int m = 0; m < array7.Length; m++)
					{
						if (array7[m] != null && array7[m].Name == critMods[l].config.Name && array7[m].Version_Internal >= critMods[l].config.Version_Internal)
						{
							flag6 = true;
							break;
						}
					}
				}
				if (!flag6)
				{
					flag5 = false;
					break;
				}
			}
			if (!flag5)
			{
				Provider.reject(transportConnection, ESteamRejection.SERVER_MODULE_DESYNC);
				return;
			}
			if (!string.IsNullOrEmpty(Provider.serverPassword) && !Hash.verifyHash(array, Provider._serverPasswordHash))
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_PASSWORD);
				return;
			}
			if (!Hash.verifyHash(array2, Level.hash) && !steamPlayerID.BypassIntegrityChecks)
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_HASH_LEVEL);
				return;
			}
			if (!PlayerHashValidation.IsAssemblyHashValid(array3, clientPlatform) && !steamPlayerID.BypassIntegrityChecks)
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_HASH_ASSEMBLY);
				return;
			}
			if (!PlayerHashValidation.IsResourcesHashValid(array4, clientPlatform) && !steamPlayerID.BypassIntegrityChecks)
			{
				Provider.reject(transportConnection, ESteamRejection.WRONG_HASH_RESOURCES);
				return;
			}
			if ((uint)num > Provider.configData.Server.Max_Ping_Milliseconds)
			{
				Provider.reject(transportConnection, ESteamRejection.PING, Provider.configData.Server.Max_Ping_Milliseconds.ToString());
				return;
			}
			if (Provider.modeConfigData.Players.Enable_Terrain_Color_Kick && ServerMessageHandler_ReadyToConnect.IsSkinColorWithinThresholdOfTerrainColor(color))
			{
				Provider.reject(transportConnection, ESteamRejection.SKIN_COLOR_WITHIN_THRESHOLD_OF_TERRAIN_COLOR);
				return;
			}
			SteamPending steamPending = new SteamPending(transportConnection, steamPlayerID, newPro, newFace, newHair, newBeard, color, c, c2, newHand, newPackageShirt, newPackagePants, newPackageHat, newPackageBackpack, newPackageVest, newPackageMask, newPackageGlasses, ServerMessageHandler_ReadyToConnect.pendingPackageSkins.ToArray(), newSkillset, newLanguage, newLobbyID, clientPlatform);
			byte queuePosition;
			bool flag7;
			if (!Provider.isWhitelisted && flag2)
			{
				if (Provider.pending.Count == 0)
				{
					Provider.pending.Add(steamPending);
					queuePosition = 0;
					flag7 = true;
				}
				else
				{
					Provider.pending.Insert(1, steamPending);
					queuePosition = 1;
					flag7 = false;
				}
			}
			else
			{
				queuePosition = MathfEx.ClampToByte(Provider.pending.Count);
				Provider.pending.Add(steamPending);
				flag7 = (queuePosition == 0);
			}
			UnturnedLog.info(string.Format("Added {0} to queue position {1} (shouldVerify: {2})", steamPlayerID, queuePosition, flag7));
			steamPending.lastNotifiedQueuePosition = (int)queuePosition;
			NetMessages.SendMessageToClient(EClientMessage.QueuePositionChanged, ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt8(writer, queuePosition);
			});
			if (flag7)
			{
				Provider.verifyNextPlayerInQueue();
			}
		}

		/// <summary>
		/// Kick players maybe trying to impersonate me. I guess nobody else named Nelson is allowed in the game!
		/// 2023-09-19: relaxed this a bit by trimming names and using Equals/Starts/Ends rather than Contains
		/// because there was a player with Nelson in their username.
		/// </summary>
		// Token: 0x0600128D RID: 4749 RVA: 0x00041CE0 File Offset: 0x0003FEE0
		private static bool IsNameBlockedByNelsonFilter(string name)
		{
			return name.Equals("Nelson", 3) || (name.StartsWith("SDG", 3) && name.EndsWith("Nelson", 3));
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x00041D14 File Offset: 0x0003FF14
		private static bool IsSkinColorWithinThresholdOfTerrainColor(Color32 skinColor)
		{
			LevelAsset asset = Level.getAsset();
			if (asset == null || asset.terrainColorRules == null || asset.terrainColorRules.Count < 1)
			{
				return false;
			}
			float inputHue;
			float inputSaturation;
			float inputValue;
			Color.RGBToHSV(skinColor, out inputHue, out inputSaturation, out inputValue);
			foreach (LevelAsset.TerrainColorRule terrainColorRule in asset.terrainColorRules)
			{
				if (terrainColorRule != null && terrainColorRule.CompareColors(inputHue, inputSaturation, inputValue) == LevelAsset.TerrainColorRule.EComparisonResult.TooSimilar)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040005E7 RID: 1511
		private static List<ulong> pendingPackageSkins = new List<ulong>();
	}
}
