using System;

namespace SDG.Unturned
{
	// Token: 0x02000343 RID: 835
	public class NPCReputationReward : INPCReward
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001911 RID: 6417 RVA: 0x0005A47C File Offset: 0x0005867C
		// (set) Token: 0x06001912 RID: 6418 RVA: 0x0005A484 File Offset: 0x00058684
		public int value { get; protected set; }

		// Token: 0x06001913 RID: 6419 RVA: 0x0005A48D File Offset: 0x0005868D
		public override void GrantReward(Player player)
		{
			player.skills.askRep(this.value);
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x0005A4A0 File Offset: 0x000586A0
		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Reputation");
			}
			string text = this.value.ToString();
			if (this.value > 0)
			{
				text = "+" + text;
			}
			return Local.FormatText(this.text, text);
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x0005A4FF File Offset: 0x000586FF
		public NPCReputationReward(int newValue, string newText) : base(newText)
		{
			this.value = newValue;
		}
	}
}
