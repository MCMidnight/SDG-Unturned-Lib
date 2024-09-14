using System;

namespace SDG.Unturned
{
	// Token: 0x0200042D RID: 1069
	public class ServerSavedata
	{
		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x0007DEB8 File Offset: 0x0007C0B8
		public static string directoryName
		{
			get
			{
				return "Servers";
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002097 RID: 8343 RVA: 0x0007DEBF File Offset: 0x0007C0BF
		public static string directory
		{
			get
			{
				return "/Servers";
			}
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x0007DEC6 File Offset: 0x0007C0C6
		public static string transformPath(string path)
		{
			return ServerSavedata.directory + "/" + Provider.serverID + path;
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x0007DEDD File Offset: 0x0007C0DD
		public static void serializeJSON<T>(string path, T instance)
		{
			ReadWrite.serializeJSON<T>(ServerSavedata.directory + "/" + Provider.serverID + path, false, instance);
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x0007DEFB File Offset: 0x0007C0FB
		public static T deserializeJSON<T>(string path)
		{
			return ReadWrite.deserializeJSON<T>(ServerSavedata.directory + "/" + Provider.serverID + path, false);
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x0007DF18 File Offset: 0x0007C118
		public static void populateJSON(string path, object target)
		{
			ReadWrite.populateJSON(ServerSavedata.directory + "/" + Provider.serverID + path, target, true);
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x0007DF36 File Offset: 0x0007C136
		public static void writeData(string path, Data data)
		{
			ReadWrite.writeData(ServerSavedata.directory + "/" + Provider.serverID + path, false, data);
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x0007DF54 File Offset: 0x0007C154
		public static Data readData(string path)
		{
			return ReadWrite.readData(ServerSavedata.directory + "/" + Provider.serverID + path, false);
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x0007DF71 File Offset: 0x0007C171
		public static void writeBlock(string path, Block block)
		{
			ReadWrite.writeBlock(ServerSavedata.directory + "/" + Provider.serverID + path, false, block);
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x0007DF8F File Offset: 0x0007C18F
		public static Block readBlock(string path, byte prefix)
		{
			return ReadWrite.readBlock(ServerSavedata.directory + "/" + Provider.serverID + path, false, prefix);
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x0007DFAD File Offset: 0x0007C1AD
		public static River openRiver(string path, bool isReading)
		{
			return new River(ServerSavedata.directory + "/" + Provider.serverID + path, true, false, isReading);
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x0007DFCC File Offset: 0x0007C1CC
		public static void deleteFile(string path)
		{
			ReadWrite.deleteFile(ServerSavedata.directory + "/" + Provider.serverID + path, false);
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x0007DFE9 File Offset: 0x0007C1E9
		public static bool fileExists(string path)
		{
			return ReadWrite.fileExists(ServerSavedata.directory + "/" + Provider.serverID + path, false);
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x0007E006 File Offset: 0x0007C206
		public static void createFolder(string path)
		{
			ReadWrite.createFolder(ServerSavedata.directory + "/" + Provider.serverID + path);
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x0007E022 File Offset: 0x0007C222
		public static void deleteFolder(string path)
		{
			ReadWrite.deleteFolder(ServerSavedata.directory + "/" + Provider.serverID + path);
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x0007E03E File Offset: 0x0007C23E
		public static bool folderExists(string path)
		{
			return ReadWrite.folderExists(ServerSavedata.directory + "/" + Provider.serverID + path);
		}
	}
}
