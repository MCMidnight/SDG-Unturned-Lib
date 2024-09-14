using System;

namespace SDG.Unturned
{
	// Token: 0x02000429 RID: 1065
	public class PlayerSavedata
	{
		// Token: 0x06002029 RID: 8233 RVA: 0x0007C4C4 File Offset: 0x0007A6C4
		public static void writeData(SteamPlayerID playerID, string path, Data data)
		{
			if (PlayerSavedata.hasSync)
			{
				ReadWrite.writeData(string.Concat(new string[]
				{
					"/Sync/",
					playerID.steamID.ToString(),
					"_",
					playerID.characterID.ToString(),
					"/",
					Level.info.name,
					path
				}), false, data);
				return;
			}
			ServerSavedata.writeData(string.Concat(new string[]
			{
				"/Players/",
				playerID.steamID.ToString(),
				"_",
				playerID.characterID.ToString(),
				"/",
				Level.info.name,
				path
			}), data);
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x0007C598 File Offset: 0x0007A798
		public static Data readData(SteamPlayerID playerID, string path)
		{
			if (PlayerSavedata.hasSync)
			{
				return ReadWrite.readData(string.Concat(new string[]
				{
					"/Sync/",
					playerID.steamID.ToString(),
					"_",
					playerID.characterID.ToString(),
					"/",
					Level.info.name,
					path
				}), false);
			}
			return ServerSavedata.readData(string.Concat(new string[]
			{
				"/Players/",
				playerID.steamID.ToString(),
				"_",
				playerID.characterID.ToString(),
				"/",
				Level.info.name,
				path
			}));
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x0007C66C File Offset: 0x0007A86C
		public static void writeBlock(SteamPlayerID playerID, string path, Block block)
		{
			if (PlayerSavedata.hasSync)
			{
				ReadWrite.writeBlock(string.Concat(new string[]
				{
					"/Sync/",
					playerID.steamID.ToString(),
					"_",
					playerID.characterID.ToString(),
					"/",
					Level.info.name,
					path
				}), false, block);
				return;
			}
			ServerSavedata.writeBlock(string.Concat(new string[]
			{
				"/Players/",
				playerID.steamID.ToString(),
				"_",
				playerID.characterID.ToString(),
				"/",
				Level.info.name,
				path
			}), block);
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x0007C740 File Offset: 0x0007A940
		public static Block readBlock(SteamPlayerID playerID, string path, byte prefix)
		{
			if (PlayerSavedata.hasSync)
			{
				return ReadWrite.readBlock(string.Concat(new string[]
				{
					"/Sync/",
					playerID.steamID.ToString(),
					"_",
					playerID.characterID.ToString(),
					"/",
					Level.info.name,
					path
				}), false, prefix);
			}
			return ServerSavedata.readBlock(string.Concat(new string[]
			{
				"/Players/",
				playerID.steamID.ToString(),
				"_",
				playerID.characterID.ToString(),
				"/",
				Level.info.name,
				path
			}), prefix);
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x0007C814 File Offset: 0x0007AA14
		public static River openRiver(SteamPlayerID playerID, string path, bool isReading)
		{
			if (PlayerSavedata.hasSync)
			{
				return new River(string.Concat(new string[]
				{
					"/Sync/",
					playerID.steamID.ToString(),
					"_",
					playerID.characterID.ToString(),
					"/",
					Level.info.name,
					path
				}), true, false, isReading);
			}
			return ServerSavedata.openRiver(string.Concat(new string[]
			{
				"/Players/",
				playerID.steamID.ToString(),
				"_",
				playerID.characterID.ToString(),
				"/",
				Level.info.name,
				path
			}), isReading);
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x0007C8EC File Offset: 0x0007AAEC
		public static void deleteFile(SteamPlayerID playerID, string path)
		{
			if (PlayerSavedata.hasSync)
			{
				ReadWrite.deleteFile(string.Concat(new string[]
				{
					"/Sync/",
					playerID.steamID.ToString(),
					"_",
					playerID.characterID.ToString(),
					"/",
					Level.info.name,
					path
				}), false);
				return;
			}
			ServerSavedata.deleteFile(string.Concat(new string[]
			{
				"/Players/",
				playerID.steamID.ToString(),
				"_",
				playerID.characterID.ToString(),
				"/",
				Level.info.name,
				path
			}));
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x0007C9C0 File Offset: 0x0007ABC0
		public static bool fileExists(SteamPlayerID playerID, string path)
		{
			if (PlayerSavedata.hasSync)
			{
				return ReadWrite.fileExists(string.Concat(new string[]
				{
					"/Sync/",
					playerID.steamID.ToString(),
					"_",
					playerID.characterID.ToString(),
					"/",
					Level.info.name,
					path
				}), false);
			}
			return ServerSavedata.fileExists(string.Concat(new string[]
			{
				"/Players/",
				playerID.steamID.ToString(),
				"_",
				playerID.characterID.ToString(),
				"/",
				Level.info.name,
				path
			}));
		}

		/// <summary>
		/// Delete all savedata folders for player's characters.
		/// </summary>
		// Token: 0x06002030 RID: 8240 RVA: 0x0007CA94 File Offset: 0x0007AC94
		public static void deleteFolder(SteamPlayerID playerID)
		{
			int num = (int)(Customization.FREE_CHARACTERS + Customization.PRO_CHARACTERS);
			if (PlayerSavedata.hasSync)
			{
				for (int i = 0; i < num; i++)
				{
					string path = "/Sync/" + playerID.steamID.ToString() + "_" + i.ToString();
					if (ReadWrite.folderExists(path, false))
					{
						ReadWrite.deleteFolder(path, false);
					}
				}
				return;
			}
			for (int j = 0; j < num; j++)
			{
				string path2 = "/Players/" + playerID.steamID.ToString() + "_" + j.ToString();
				if (ServerSavedata.folderExists(path2))
				{
					ServerSavedata.deleteFolder(path2);
				}
			}
		}

		// Token: 0x04000FC0 RID: 4032
		public static bool hasSync;
	}
}
