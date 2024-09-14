using System;

namespace SDG.Unturned
{
	// Token: 0x0200033D RID: 829
	public class NPCPlayerSpawnpointReward : INPCReward
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x0005A03B File Offset: 0x0005823B
		// (set) Token: 0x060018E1 RID: 6369 RVA: 0x0005A043 File Offset: 0x00058243
		public string id { get; protected set; }

		// Token: 0x060018E2 RID: 6370 RVA: 0x0005A04C File Offset: 0x0005824C
		public override void GrantReward(Player player)
		{
			player.quests.npcSpawnId = this.id;
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x0005A05F File Offset: 0x0005825F
		public NPCPlayerSpawnpointReward(string newID, string newText) : base(newText)
		{
			this.id = newID;
		}
	}
}
