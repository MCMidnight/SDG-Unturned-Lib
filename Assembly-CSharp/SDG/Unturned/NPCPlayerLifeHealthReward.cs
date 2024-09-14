using System;

namespace SDG.Unturned
{
	// Token: 0x02000336 RID: 822
	public class NPCPlayerLifeHealthReward : INPCReward
	{
		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060018BD RID: 6333 RVA: 0x00059D26 File Offset: 0x00057F26
		// (set) Token: 0x060018BE RID: 6334 RVA: 0x00059D2E File Offset: 0x00057F2E
		public int value { get; protected set; }

		// Token: 0x060018BF RID: 6335 RVA: 0x00059D37 File Offset: 0x00057F37
		public override void GrantReward(Player player)
		{
			player.life.serverModifyHealth((float)this.value);
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x00059D4B File Offset: 0x00057F4B
		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Health");
			}
			return Local.FormatText(this.text, this.value);
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x00059D85 File Offset: 0x00057F85
		public NPCPlayerLifeHealthReward(int newValue, string newText) : base(newText)
		{
			this.value = newValue;
		}
	}
}
