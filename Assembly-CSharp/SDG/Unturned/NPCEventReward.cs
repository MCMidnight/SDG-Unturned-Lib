using System;

namespace SDG.Unturned
{
	// Token: 0x02000324 RID: 804
	public class NPCEventReward : INPCReward
	{
		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x0600183A RID: 6202 RVA: 0x00058AFC File Offset: 0x00056CFC
		// (set) Token: 0x0600183B RID: 6203 RVA: 0x00058B04 File Offset: 0x00056D04
		public string id { get; protected set; }

		// Token: 0x0600183C RID: 6204 RVA: 0x00058B0D File Offset: 0x00056D0D
		public override void GrantReward(Player player)
		{
			NPCEventManager.broadcastEvent(player, this.id, true);
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x00058B1C File Offset: 0x00056D1C
		public NPCEventReward(string newID, string newText) : base(newText)
		{
			this.id = newID;
		}
	}
}
