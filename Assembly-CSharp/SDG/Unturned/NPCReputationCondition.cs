using System;

namespace SDG.Unturned
{
	// Token: 0x02000342 RID: 834
	public class NPCReputationCondition : NPCLogicCondition
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x0600190C RID: 6412 RVA: 0x0005A3B2 File Offset: 0x000585B2
		// (set) Token: 0x0600190D RID: 6413 RVA: 0x0005A3BA File Offset: 0x000585BA
		public int reputation { get; protected set; }

		// Token: 0x0600190E RID: 6414 RVA: 0x0005A3C3 File Offset: 0x000585C3
		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<int>(player.skills.reputation, this.reputation);
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x0005A3DC File Offset: 0x000585DC
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				this.text = PlayerNPCQuestUI.localization.read("Condition_Reputation");
			}
			string text = player.skills.reputation.ToString();
			if (player.skills.reputation > 0)
			{
				text = "+" + text;
			}
			string text2 = this.reputation.ToString();
			if (this.reputation > 0)
			{
				text2 = "+" + text2;
			}
			return Local.FormatText(this.text, text, text2);
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x0005A46A File Offset: 0x0005866A
		public NPCReputationCondition(int newReputation, ENPCLogicType newLogicType, string newText) : base(newLogicType, newText, false)
		{
			this.reputation = newReputation;
		}
	}
}
