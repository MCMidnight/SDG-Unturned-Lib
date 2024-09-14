using System;

namespace SDG.Unturned
{
	// Token: 0x02000325 RID: 805
	public class NPCExperienceCondition : NPCLogicCondition
	{
		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x0600183E RID: 6206 RVA: 0x00058B2C File Offset: 0x00056D2C
		// (set) Token: 0x0600183F RID: 6207 RVA: 0x00058B34 File Offset: 0x00056D34
		public uint experience { get; protected set; }

		// Token: 0x06001840 RID: 6208 RVA: 0x00058B3D File Offset: 0x00056D3D
		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<uint>(player.skills.experience, this.experience);
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00058B56 File Offset: 0x00056D56
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			player.skills.askSpend(this.experience);
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x00058B74 File Offset: 0x00056D74
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Condition_Experience");
			}
			return Local.FormatText(this.text, player.skills.experience, this.experience);
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x00058BC9 File Offset: 0x00056DC9
		public NPCExperienceCondition(uint newExperience, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newLogicType, newText, newShouldReset)
		{
			this.experience = newExperience;
		}
	}
}
