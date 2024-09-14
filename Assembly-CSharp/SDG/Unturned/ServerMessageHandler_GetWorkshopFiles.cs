using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200027A RID: 634
	internal static class ServerMessageHandler_GetWorkshopFiles
	{
		// Token: 0x06001284 RID: 4740 RVA: 0x00040DD4 File Offset: 0x0003EFD4
		internal static void ReadMessage(ITransportConnection transportConnection, NetPakReader reader)
		{
			byte[] array;
			int num;
			if (!reader.ReadBytesPtr(960, ref array, ref num))
			{
				Provider.refuseGarbageConnection(transportConnection, "missing empty buffer");
				return;
			}
			string text;
			if (!SystemNetPakReaderEx.ReadString(reader, ref text, 11))
			{
				Provider.refuseGarbageConnection(transportConnection, "failed to read header string");
				return;
			}
			if (!string.Equals(text, "Hello!", 4))
			{
				Provider.refuseGarbageConnection(transportConnection, "invalid header string");
				return;
			}
			int hashCode = transportConnection.GetHashCode();
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			bool flag = false;
			int i = Provider.workshopRequests.Count - 1;
			while (i >= 0)
			{
				Provider.WorkshopRequestLog workshopRequestLog = Provider.workshopRequests[i];
				bool flag2 = realtimeSinceStartup - workshopRequestLog.realTime < 30f;
				if (workshopRequestLog.sender == hashCode)
				{
					workshopRequestLog.realTime = realtimeSinceStartup;
					Provider.workshopRequests[i] = workshopRequestLog;
					if (flag2)
					{
						if (NetMessages.shouldLogBadMessages)
						{
							UnturnedLog.info(string.Format("Ignoring GetWorkshopFiles message from {0} because they requested recently", transportConnection));
						}
						return;
					}
					flag = true;
					break;
				}
				else
				{
					if (!flag2)
					{
						Provider.workshopRequests.RemoveAtFast(i);
					}
					i--;
				}
			}
			if (!flag)
			{
				Provider.WorkshopRequestLog workshopRequestLog2 = default(Provider.WorkshopRequestLog);
				workshopRequestLog2.sender = hashCode;
				workshopRequestLog2.realTime = realtimeSinceStartup;
				Provider.workshopRequests.Add(workshopRequestLog2);
			}
			NetMessages.SendMessageToClient(EClientMessage.DownloadWorkshopFiles, ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				writer.WriteEnum(Provider.authorityHoliday);
				SystemNetPakWriterEx.WriteList<Provider.ServerRequiredWorkshopFile>(writer, Provider.serverRequiredWorkshopFiles, new SystemNetPakWriterEx.WriteListItemWithWriter<Provider.ServerRequiredWorkshopFile>(ServerMessageHandler_GetWorkshopFiles.WriteServerRequiredWorkshopFile), ServerMessageHandler_GetWorkshopFiles.MAX_FILES);
				SystemNetPakWriterEx.WriteString(writer, Provider.serverName, 11);
				SystemNetPakWriterEx.WriteString(writer, Provider.map, 11);
				writer.WriteBit(Provider.isPvP);
				writer.WriteBit(Provider.hasCheats);
				writer.WriteBit(Provider.isVacActive);
				writer.WriteBit(Provider.isBattlEyeActive);
				writer.WriteBit(Provider.isGold);
				writer.WriteEnum(Provider.mode);
				writer.WriteEnum(Provider.cameraMode);
				SystemNetPakWriterEx.WriteUInt8(writer, Provider.maxPlayers);
				SystemNetPakWriterEx.WriteString(writer, Provider.configData.Browser.BookmarkHost, 11);
				SystemNetPakWriterEx.WriteString(writer, Provider.configData.Browser.Thumbnail, 11);
				SystemNetPakWriterEx.WriteString(writer, Provider.configData.Browser.Desc_Server_List, 11);
			});
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00040F27 File Offset: 0x0003F127
		private static bool WriteServerRequiredWorkshopFile(NetPakWriter writer, Provider.ServerRequiredWorkshopFile item)
		{
			return SystemNetPakWriterEx.WriteUInt64(writer, item.fileId) && SystemNetPakWriterEx.WriteDateTime(writer, item.timestamp);
		}

		// Token: 0x040005E6 RID: 1510
		private static readonly NetLength MAX_FILES = new NetLength(255U);
	}
}
