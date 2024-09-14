using System;

namespace SDG.Unturned
{
	// Token: 0x02000425 RID: 1061
	public class LevelSavedata
	{
		// Token: 0x06002005 RID: 8197 RVA: 0x0007BA36 File Offset: 0x00079C36
		public static string transformName(string name)
		{
			return ServerSavedata.transformPath("/Level/" + name);
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x0007BA48 File Offset: 0x00079C48
		public static void writeData(string path, Data data)
		{
			ServerSavedata.writeData("/Level/" + Level.info.name + path, data);
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x0007BA65 File Offset: 0x00079C65
		public static Data readData(string path)
		{
			return ServerSavedata.readData("/Level/" + Level.info.name + path);
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x0007BA81 File Offset: 0x00079C81
		public static void writeBlock(string path, Block block)
		{
			ServerSavedata.writeBlock("/Level/" + Level.info.name + path, block);
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x0007BA9E File Offset: 0x00079C9E
		public static Block readBlock(string path, byte prefix)
		{
			return ServerSavedata.readBlock("/Level/" + Level.info.name + path, prefix);
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x0007BABB File Offset: 0x00079CBB
		public static River openRiver(string path, bool isReading)
		{
			return ServerSavedata.openRiver("/Level/" + Level.info.name + path, isReading);
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x0007BAD8 File Offset: 0x00079CD8
		public static void deleteFile(string path)
		{
			ServerSavedata.deleteFile("/Level/" + Level.info.name + path);
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x0007BAF4 File Offset: 0x00079CF4
		public static bool fileExists(string path)
		{
			return ServerSavedata.fileExists("/Level/" + Level.info.name + path);
		}
	}
}
