using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020005F6 RID: 1526
	public class GroupInfo
	{
		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06003040 RID: 12352 RVA: 0x000D4BD4 File Offset: 0x000D2DD4
		// (set) Token: 0x06003041 RID: 12353 RVA: 0x000D4BDC File Offset: 0x000D2DDC
		public CSteamID groupID { get; private set; }

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06003042 RID: 12354 RVA: 0x000D4BE5 File Offset: 0x000D2DE5
		public bool useMaxGroupMembersLimit
		{
			get
			{
				return Provider.modeConfigData.Gameplay.Max_Group_Members > 0U;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06003043 RID: 12355 RVA: 0x000D4BF9 File Offset: 0x000D2DF9
		public bool hasSpaceForMoreMembersInGroup
		{
			get
			{
				return !this.useMaxGroupMembersLimit || this.members < Provider.modeConfigData.Gameplay.Max_Group_Members;
			}
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x000D4C1C File Offset: 0x000D2E1C
		public GroupInfo(CSteamID newGroupID, string newName, uint newMembers)
		{
			this.groupID = newGroupID;
			this.name = newName;
			this.members = newMembers;
		}

		// Token: 0x04001B65 RID: 7013
		public string name;

		// Token: 0x04001B66 RID: 7014
		public uint members;
	}
}
