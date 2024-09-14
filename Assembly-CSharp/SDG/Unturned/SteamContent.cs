using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200068E RID: 1678
	public class SteamContent
	{
		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06003886 RID: 14470 RVA: 0x0010B63F File Offset: 0x0010983F
		public PublishedFileId_t publishedFileID
		{
			get
			{
				return this._publishedFileID;
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06003887 RID: 14471 RVA: 0x0010B647 File Offset: 0x00109847
		public string path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06003888 RID: 14472 RVA: 0x0010B64F File Offset: 0x0010984F
		public ESteamUGCType type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x0010B657 File Offset: 0x00109857
		public SteamContent(PublishedFileId_t newPublishedFileID, string newPath, ESteamUGCType newType)
		{
			this._publishedFileID = newPublishedFileID;
			this._path = newPath;
			this._type = newType;
		}

		// Token: 0x0400217F RID: 8575
		private PublishedFileId_t _publishedFileID;

		// Token: 0x04002180 RID: 8576
		private string _path;

		// Token: 0x04002181 RID: 8577
		private ESteamUGCType _type;
	}
}
