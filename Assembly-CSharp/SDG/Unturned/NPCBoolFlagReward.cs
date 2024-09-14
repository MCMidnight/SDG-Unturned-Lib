using System;

namespace SDG.Unturned
{
	// Token: 0x0200031D RID: 797
	public class NPCBoolFlagReward : INPCReward
	{
		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600180F RID: 6159 RVA: 0x0005860A File Offset: 0x0005680A
		// (set) Token: 0x06001810 RID: 6160 RVA: 0x00058612 File Offset: 0x00056812
		public ushort id { get; protected set; }

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001811 RID: 6161 RVA: 0x0005861B File Offset: 0x0005681B
		// (set) Token: 0x06001812 RID: 6162 RVA: 0x00058623 File Offset: 0x00056823
		public bool value { get; protected set; }

		// Token: 0x06001813 RID: 6163 RVA: 0x0005862C File Offset: 0x0005682C
		public override void GrantReward(Player player)
		{
			player.quests.sendSetFlag(this.id, this.value ? 1 : 0);
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x0005864C File Offset: 0x0005684C
		public NPCBoolFlagReward(ushort newID, bool newValue, string newText) : base(newText)
		{
			this.id = newID;
			this.value = newValue;
		}
	}
}
