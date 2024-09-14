using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Cloud;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Cloud
{
	// Token: 0x0200002B RID: 43
	public class SteamworksCloudService : Service, ICloudService, IService
	{
		// Token: 0x0600010A RID: 266 RVA: 0x00004828 File Offset: 0x00002A28
		public bool read(string path, byte[] data)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			int fileSize = SteamRemoteStorage.GetFileSize(path);
			return data.Length >= fileSize && SteamRemoteStorage.FileRead(path, data, fileSize) == fileSize;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000486E File Offset: 0x00002A6E
		public bool write(string path, byte[] data, int size)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			return SteamRemoteStorage.FileWrite(path, data, size);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00004894 File Offset: 0x00002A94
		public bool getSize(string path, out int size)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			size = SteamRemoteStorage.GetFileSize(path);
			return true;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000048AD File Offset: 0x00002AAD
		public bool exists(string path, out bool exists)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			exists = SteamRemoteStorage.FileExists(path);
			return true;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000048C6 File Offset: 0x00002AC6
		public bool delete(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return SteamRemoteStorage.FileDelete(path);
		}
	}
}
