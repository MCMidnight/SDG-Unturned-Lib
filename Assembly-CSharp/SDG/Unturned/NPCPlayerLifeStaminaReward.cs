using System;

namespace SDG.Unturned
{
	// Token: 0x02000338 RID: 824
	public class NPCPlayerLifeStaminaReward : INPCReward
	{
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060018C7 RID: 6343 RVA: 0x00059E08 File Offset: 0x00058008
		// (set) Token: 0x060018C8 RID: 6344 RVA: 0x00059E10 File Offset: 0x00058010
		public int value { get; protected set; }

		// Token: 0x060018C9 RID: 6345 RVA: 0x00059E19 File Offset: 0x00058019
		public override void GrantReward(Player player)
		{
			player.life.serverModifyStamina((float)this.value);
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x00059E2D File Offset: 0x0005802D
		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Stamina");
			}
			return Local.FormatText(this.text, this.value);
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x00059E67 File Offset: 0x00058067
		public NPCPlayerLifeStaminaReward(int newValue, string newText) : base(newText)
		{
			this.value = newValue;
		}
	}
}
