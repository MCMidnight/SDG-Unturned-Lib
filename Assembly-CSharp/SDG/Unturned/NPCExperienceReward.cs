using System;

namespace SDG.Unturned
{
	// Token: 0x02000326 RID: 806
	public class NPCExperienceReward : INPCReward
	{
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001844 RID: 6212 RVA: 0x00058BDC File Offset: 0x00056DDC
		// (set) Token: 0x06001845 RID: 6213 RVA: 0x00058BE4 File Offset: 0x00056DE4
		public uint value { get; protected set; }

		// Token: 0x06001846 RID: 6214 RVA: 0x00058BED File Offset: 0x00056DED
		public override void GrantReward(Player player)
		{
			player.skills.askAward(this.value);
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x00058C00 File Offset: 0x00056E00
		public override string formatReward(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Reward_Experience");
			}
			string arg = "+" + this.value.ToString();
			return Local.FormatText(this.text, arg);
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x00058C54 File Offset: 0x00056E54
		public NPCExperienceReward(uint newValue, string newText) : base(newText)
		{
			this.value = newValue;
		}
	}
}
