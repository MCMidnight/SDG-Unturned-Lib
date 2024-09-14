using System;

namespace SDG.Unturned
{
	// Token: 0x02000334 RID: 820
	public class NPCPlayerLifeFoodReward : INPCReward
	{
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x00059C44 File Offset: 0x00057E44
		// (set) Token: 0x060018B4 RID: 6324 RVA: 0x00059C4C File Offset: 0x00057E4C
		public int value { get; protected set; }

		// Token: 0x060018B5 RID: 6325 RVA: 0x00059C55 File Offset: 0x00057E55
		public override void GrantReward(Player player)
		{
			player.life.serverModifyFood((float)this.value);
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x00059C69 File Offset: 0x00057E69
		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Food");
			}
			return Local.FormatText(this.text, this.value);
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x00059CA3 File Offset: 0x00057EA3
		public NPCPlayerLifeFoodReward(int newValue, string newText) : base(newText)
		{
			this.value = newValue;
		}
	}
}
