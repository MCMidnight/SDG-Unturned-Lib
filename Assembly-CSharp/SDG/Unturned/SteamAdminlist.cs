using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000683 RID: 1667
	public class SteamAdminlist
	{
		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06003815 RID: 14357 RVA: 0x00109514 File Offset: 0x00107714
		public static List<SteamAdminID> list
		{
			get
			{
				return SteamAdminlist._list;
			}
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x0010951C File Offset: 0x0010771C
		public static void admin(CSteamID playerID, CSteamID judgeID)
		{
			for (int i = 0; i < SteamAdminlist.list.Count; i++)
			{
				if (SteamAdminlist.list[i].playerID == playerID)
				{
					SteamAdminlist.list[i].judgeID = judgeID;
					return;
				}
			}
			SteamAdminlist.list.Add(new SteamAdminID(playerID, judgeID));
			SteamPlayer client = PlayerTool.getSteamPlayer(playerID);
			if (client != null)
			{
				client.isAdmin = true;
				NetMessages.SendMessageToClients(EClientMessage.Admined, ENetReliability.Reliable, Provider.GatherRemoteClientConnectionsMatchingPredicate((SteamPlayer potentialRecipient) => potentialRecipient == client || !Provider.hideAdmins), delegate(NetPakWriter writer)
				{
					SystemNetPakWriterEx.WriteUInt8(writer, (byte)client.channel);
				});
			}
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x001095C4 File Offset: 0x001077C4
		public static void unadmin(CSteamID playerID)
		{
			SteamPlayer client = PlayerTool.getSteamPlayer(playerID);
			if (client != null && client.isAdmin)
			{
				client.isAdmin = false;
				NetMessages.SendMessageToClients(EClientMessage.Unadmined, ENetReliability.Reliable, Provider.GatherRemoteClientConnectionsMatchingPredicate((SteamPlayer potentialRecipient) => potentialRecipient == client || !Provider.hideAdmins), delegate(NetPakWriter writer)
				{
					SystemNetPakWriterEx.WriteUInt8(writer, (byte)client.channel);
				});
			}
			for (int i = 0; i < SteamAdminlist.list.Count; i++)
			{
				if (SteamAdminlist.list[i].playerID == playerID)
				{
					SteamAdminlist.list.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x00109664 File Offset: 0x00107864
		public static bool checkAC(CSteamID playerID)
		{
			UnturnedLog.info(playerID);
			byte[] array = Hash.SHA1(playerID);
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				if (i > 0)
				{
					text += ", ";
				}
				text += array[i].ToString();
			}
			UnturnedLog.info(text);
			return false;
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x001096C4 File Offset: 0x001078C4
		public static bool checkAdmin(CSteamID playerID)
		{
			if (playerID == SteamAdminlist.ownerID)
			{
				return true;
			}
			for (int i = 0; i < SteamAdminlist.list.Count; i++)
			{
				if (SteamAdminlist.list[i].playerID == playerID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x00109710 File Offset: 0x00107910
		public static void load()
		{
			SteamAdminlist._list = new List<SteamAdminID>();
			SteamAdminlist.ownerID = CSteamID.Nil;
			if (ServerSavedata.fileExists("/Server/Adminlist.dat"))
			{
				River river = ServerSavedata.openRiver("/Server/Adminlist.dat", true);
				if (river.readByte() > 1)
				{
					ushort num = river.readUInt16();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						CSteamID newPlayerID = river.readSteamID();
						CSteamID newJudgeID = river.readSteamID();
						SteamAdminID steamAdminID = new SteamAdminID(newPlayerID, newJudgeID);
						SteamAdminlist.list.Add(steamAdminID);
					}
				}
				river.closeRiver();
			}
		}

		// Token: 0x0600381B RID: 14363 RVA: 0x00109794 File Offset: 0x00107994
		public static void save()
		{
			River river = ServerSavedata.openRiver("/Server/Adminlist.dat", false);
			river.writeByte(SteamAdminlist.SAVEDATA_VERSION);
			river.writeUInt16((ushort)SteamAdminlist.list.Count);
			ushort num = 0;
			while ((int)num < SteamAdminlist.list.Count)
			{
				SteamAdminID steamAdminID = SteamAdminlist.list[(int)num];
				river.writeSteamID(steamAdminID.playerID);
				river.writeSteamID(steamAdminID.judgeID);
				num += 1;
			}
			river.closeRiver();
		}

		// Token: 0x04002149 RID: 8521
		public static readonly byte SAVEDATA_VERSION = 2;

		// Token: 0x0400214A RID: 8522
		private static List<SteamAdminID> _list;

		// Token: 0x0400214B RID: 8523
		public static CSteamID ownerID;
	}
}
