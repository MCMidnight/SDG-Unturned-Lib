using System;

namespace SDG.Unturned
{
	// Token: 0x0200033A RID: 826
	public class NPCPlayerLifeVirusReward : INPCReward
	{
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060018D1 RID: 6353 RVA: 0x00059EEA File Offset: 0x000580EA
		// (set) Token: 0x060018D2 RID: 6354 RVA: 0x00059EF2 File Offset: 0x000580F2
		public int value { get; protected set; }

		// Token: 0x060018D3 RID: 6355 RVA: 0x00059EFB File Offset: 0x000580FB
		public override void GrantReward(Player player)
		{
			player.life.serverModifyVirus((float)this.value);
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x00059F0F File Offset: 0x0005810F
		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Virus");
			}
			return Local.FormatText(this.text, this.value);
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00059F49 File Offset: 0x00058149
		public NPCPlayerLifeVirusReward(int newValue, string newText) : base(newText)
		{
			this.value = newValue;
		}
	}
}
