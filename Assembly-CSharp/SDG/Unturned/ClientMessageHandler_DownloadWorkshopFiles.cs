using System;
using System.Collections.Generic;
using SDG.NetPak;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200026A RID: 618
	internal static class ClientMessageHandler_DownloadWorkshopFiles
	{
		// Token: 0x06001268 RID: 4712 RVA: 0x0003F70C File Offset: 0x0003D90C
		internal static void ReadMessage(NetPakReader reader)
		{
			Provider.isWaitingForWorkshopResponse = false;
			ENPCHoliday enpcholiday;
			reader.ReadEnum(out enpcholiday);
			UnturnedLog.info(string.Format("Server holiday: {0}", enpcholiday));
			ClientMessageHandler_DownloadWorkshopFiles.requiredFiles.Clear();
			SystemNetPakReaderEx.ReadList<Provider.ServerRequiredWorkshopFile>(reader, ClientMessageHandler_DownloadWorkshopFiles.requiredFiles, new SystemNetPakReaderEx.ReadListItemWithReader<Provider.ServerRequiredWorkshopFile>(ClientMessageHandler_DownloadWorkshopFiles.ReadRequiredWorkshopFile), ClientMessageHandler_DownloadWorkshopFiles.MAX_FILES);
			string text;
			SystemNetPakReaderEx.ReadString(reader, ref text, 11);
			UnturnedLog.info("Server name: \"" + text + "\"");
			string text2;
			SystemNetPakReaderEx.ReadString(reader, ref text2, 11);
			UnturnedLog.info("Server level name: \"" + text2 + "\"");
			bool flag;
			reader.ReadBit(ref flag);
			UnturnedLog.info(string.Format("Server is PvP: {0}", flag));
			bool flag2;
			reader.ReadBit(ref flag2);
			UnturnedLog.info(string.Format("Server allows admin cheat codes: {0}", flag2));
			bool flag3;
			reader.ReadBit(ref flag3);
			UnturnedLog.info(string.Format("Server is VAC secure: {0}", flag3));
			bool flag4;
			reader.ReadBit(ref flag4);
			UnturnedLog.info(string.Format("Server is BattlEye secure: {0}", flag4));
			bool flag5;
			reader.ReadBit(ref flag5);
			UnturnedLog.info(string.Format("Server requires gold: {0}", flag5));
			EGameMode egameMode;
			reader.ReadEnum(out egameMode);
			UnturnedLog.info(string.Format("Server difficulty: {0}", egameMode));
			ECameraMode ecameraMode;
			reader.ReadEnum(out ecameraMode);
			UnturnedLog.info(string.Format("Server camera mode: {0}", ecameraMode));
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			UnturnedLog.info(string.Format("Server max players: {0}", b));
			string text3;
			SystemNetPakReaderEx.ReadString(reader, ref text3, 11);
			UnturnedLog.info("Server bookmark host: \"" + text3 + "\"");
			string text4;
			SystemNetPakReaderEx.ReadString(reader, ref text4, 11);
			UnturnedLog.info("Server thumbnail URL: \"" + text4 + "\"");
			string text5;
			SystemNetPakReaderEx.ReadString(reader, ref text5, 11);
			UnturnedLog.info("Server description: \"" + text5 + "\"");
			IPv4Address pv4Address;
			uint num = Provider.clientTransport.TryGetIPv4Address(out pv4Address) ? pv4Address.value : 0U;
			if (num == 0U)
			{
				UnturnedLog.warn("Unable to determine server IP for download restrictions");
			}
			Provider.CachedWorkshopResponse cachedWorkshopResponse = null;
			foreach (Provider.CachedWorkshopResponse cachedWorkshopResponse2 in Provider.cachedWorkshopResponses)
			{
				if (cachedWorkshopResponse2.server == Provider.server)
				{
					cachedWorkshopResponse = cachedWorkshopResponse2;
					break;
				}
			}
			if (cachedWorkshopResponse == null)
			{
				cachedWorkshopResponse = new Provider.CachedWorkshopResponse();
				cachedWorkshopResponse.server = Provider.server;
				Provider.cachedWorkshopResponses.Add(cachedWorkshopResponse);
			}
			cachedWorkshopResponse.holiday = enpcholiday;
			cachedWorkshopResponse.serverName = text;
			cachedWorkshopResponse.levelName = text2;
			cachedWorkshopResponse.isPvP = flag;
			cachedWorkshopResponse.allowAdminCheatCodes = flag2;
			cachedWorkshopResponse.isVACSecure = flag3;
			cachedWorkshopResponse.isBattlEyeSecure = flag4;
			cachedWorkshopResponse.isGold = flag5;
			cachedWorkshopResponse.gameMode = egameMode;
			cachedWorkshopResponse.cameraMode = ecameraMode;
			cachedWorkshopResponse.maxPlayers = b;
			cachedWorkshopResponse.bookmarkHost = text3;
			cachedWorkshopResponse.thumbnailUrl = text4;
			cachedWorkshopResponse.serverDescription = text5;
			cachedWorkshopResponse.ip = num;
			cachedWorkshopResponse.requiredFiles = ClientMessageHandler_DownloadWorkshopFiles.requiredFiles;
			cachedWorkshopResponse.realTime = Time.realtimeSinceStartup;
			Provider.receiveWorkshopResponse(cachedWorkshopResponse);
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0003FA48 File Offset: 0x0003DC48
		private static bool ReadRequiredWorkshopFile(NetPakReader reader, out Provider.ServerRequiredWorkshopFile requiredFile)
		{
			requiredFile = default(Provider.ServerRequiredWorkshopFile);
			return SystemNetPakReaderEx.ReadUInt64(reader, ref requiredFile.fileId) && SystemNetPakReaderEx.ReadDateTime(reader, ref requiredFile.timestamp);
		}

		// Token: 0x040005D1 RID: 1489
		private static List<Provider.ServerRequiredWorkshopFile> requiredFiles = new List<Provider.ServerRequiredWorkshopFile>();

		// Token: 0x040005D2 RID: 1490
		private static readonly NetLength MAX_FILES = new NetLength(255U);
	}
}
