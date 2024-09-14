using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020006B8 RID: 1720
	public class SteamWhitelistID
	{
		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x0600397C RID: 14716 RVA: 0x0010DBB1 File Offset: 0x0010BDB1
		public CSteamID steamID
		{
			get
			{
				return this._steamID;
			}
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x0010DBB9 File Offset: 0x0010BDB9
		public SteamWhitelistID(CSteamID newSteamID, string newTag, CSteamID newJudgeID)
		{
			this._steamID = newSteamID;
			this.tag = newTag;
			this.judgeID = newJudgeID;
		}

		// Token: 0x04002218 RID: 8728
		private CSteamID _steamID;

		// Token: 0x04002219 RID: 8729
		public string tag;

		// Token: 0x0400221A RID: 8730
		public CSteamID judgeID;
	}
}
