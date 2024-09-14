using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020006B7 RID: 1719
	public class SteamWhitelist
	{
		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06003974 RID: 14708 RVA: 0x0010D987 File Offset: 0x0010BB87
		public static List<SteamWhitelistID> list
		{
			get
			{
				return SteamWhitelist._list;
			}
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x0010D990 File Offset: 0x0010BB90
		public static void whitelist(CSteamID steamID, string tag, CSteamID judgeID)
		{
			for (int i = 0; i < SteamWhitelist.list.Count; i++)
			{
				if (SteamWhitelist.list[i].steamID == steamID)
				{
					SteamWhitelist.list[i].tag = tag;
					SteamWhitelist.list[i].judgeID = judgeID;
					return;
				}
			}
			SteamWhitelist.list.Add(new SteamWhitelistID(steamID, tag, judgeID));
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x0010DA00 File Offset: 0x0010BC00
		public static bool unwhitelist(CSteamID steamID)
		{
			for (int i = 0; i < SteamWhitelist.list.Count; i++)
			{
				if (SteamWhitelist.list[i].steamID == steamID)
				{
					if (Provider.isWhitelisted)
					{
						Provider.kick(steamID, "Removed from whitelist.");
					}
					SteamWhitelist.list.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x0010DA5C File Offset: 0x0010BC5C
		public static bool checkWhitelisted(CSteamID steamID)
		{
			for (int i = 0; i < SteamWhitelist.list.Count; i++)
			{
				if (SteamWhitelist.list[i].steamID == steamID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x0010DA9C File Offset: 0x0010BC9C
		public static void load()
		{
			SteamWhitelist._list = new List<SteamWhitelistID>();
			if (ServerSavedata.fileExists("/Server/Whitelist.dat"))
			{
				River river = ServerSavedata.openRiver("/Server/Whitelist.dat", true);
				if (river.readByte() > 1)
				{
					ushort num = river.readUInt16();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						CSteamID newSteamID = river.readSteamID();
						string newTag = river.readString();
						CSteamID newJudgeID = river.readSteamID();
						SteamWhitelistID steamWhitelistID = new SteamWhitelistID(newSteamID, newTag, newJudgeID);
						SteamWhitelist.list.Add(steamWhitelistID);
					}
				}
				river.closeRiver();
			}
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x0010DB20 File Offset: 0x0010BD20
		public static void save()
		{
			River river = ServerSavedata.openRiver("/Server/Whitelist.dat", false);
			river.writeByte(SteamWhitelist.SAVEDATA_VERSION);
			river.writeUInt16((ushort)SteamWhitelist.list.Count);
			ushort num = 0;
			while ((int)num < SteamWhitelist.list.Count)
			{
				SteamWhitelistID steamWhitelistID = SteamWhitelist.list[(int)num];
				river.writeSteamID(steamWhitelistID.steamID);
				river.writeString(steamWhitelistID.tag);
				river.writeSteamID(steamWhitelistID.judgeID);
				num += 1;
			}
			river.closeRiver();
		}

		// Token: 0x04002216 RID: 8726
		public static readonly byte SAVEDATA_VERSION = 2;

		// Token: 0x04002217 RID: 8727
		private static List<SteamWhitelistID> _list;
	}
}
