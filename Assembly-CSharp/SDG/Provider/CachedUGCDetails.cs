using System;
using Steamworks;

namespace SDG.Provider
{
	/// <summary>
	/// Details of a workshop item that the game may want to refer to later.
	/// Cached during client startup after getting installed items, and while
	/// downloading UGC for the dedicated server.
	/// </summary>
	// Token: 0x02000037 RID: 55
	public struct CachedUGCDetails
	{
		/// <summary>
		/// Some workshop thieves use an empty title, in which case we show the file ID as title text.
		/// </summary>
		// Token: 0x0600018B RID: 395 RVA: 0x00007818 File Offset: 0x00005A18
		public string GetTitle()
		{
			if (!string.IsNullOrEmpty(this.name))
			{
				return this.name;
			}
			return this.fileId.ToString();
		}

		// Token: 0x040000B2 RID: 178
		public PublishedFileId_t fileId;

		// Token: 0x040000B3 RID: 179
		public string name;

		// Token: 0x040000B4 RID: 180
		public byte compatibilityVersion;

		/// <summary>
		/// Banned workshop files are shown in red.
		/// </summary>
		// Token: 0x040000B5 RID: 181
		public bool isBannedOrPrivate;

		/// <summary>
		/// Used on dedicated server to test whether map has been updated, and whether local copy of file is out-of-date.
		/// </summary>
		// Token: 0x040000B6 RID: 182
		public uint updateTimestamp;
	}
}
