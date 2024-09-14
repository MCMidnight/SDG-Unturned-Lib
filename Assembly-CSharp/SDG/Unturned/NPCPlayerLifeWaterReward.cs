using System;

namespace SDG.Unturned
{
	// Token: 0x0200033C RID: 828
	public class NPCPlayerLifeWaterReward : INPCReward
	{
		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060018DB RID: 6363 RVA: 0x00059FCC File Offset: 0x000581CC
		// (set) Token: 0x060018DC RID: 6364 RVA: 0x00059FD4 File Offset: 0x000581D4
		public int value { get; protected set; }

		// Token: 0x060018DD RID: 6365 RVA: 0x00059FDD File Offset: 0x000581DD
		public override void GrantReward(Player player)
		{
			player.life.serverModifyWater((float)this.value);
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x00059FF1 File Offset: 0x000581F1
		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Water");
			}
			return Local.FormatText(this.text, this.value);
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x0005A02B File Offset: 0x0005822B
		public NPCPlayerLifeWaterReward(int newValue, string newText) : base(newText)
		{
			this.value = newValue;
		}
	}
}
