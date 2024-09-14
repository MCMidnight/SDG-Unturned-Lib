using System;

namespace SDG.Unturned
{
	// Token: 0x0200031A RID: 794
	public class NPCAchievementReward : INPCReward
	{
		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x060017F9 RID: 6137 RVA: 0x00058410 File Offset: 0x00056610
		// (set) Token: 0x060017FA RID: 6138 RVA: 0x00058418 File Offset: 0x00056618
		public string id { get; protected set; }

		// Token: 0x060017FB RID: 6139 RVA: 0x00058421 File Offset: 0x00056621
		public override void GrantReward(Player player)
		{
			player.sendAchievementUnlocked(this.id);
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x0005842F File Offset: 0x0005662F
		public NPCAchievementReward(string newID, string newText) : base(newText)
		{
			this.id = newID;
		}
	}
}
