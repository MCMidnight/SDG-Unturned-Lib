using System;

namespace SDG.Unturned
{
	// Token: 0x02000339 RID: 825
	public class NPCPlayerLifeVirusCondition : NPCLogicCondition
	{
		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060018CC RID: 6348 RVA: 0x00059E77 File Offset: 0x00058077
		// (set) Token: 0x060018CD RID: 6349 RVA: 0x00059E7F File Offset: 0x0005807F
		public int virus { get; protected set; }

		// Token: 0x060018CE RID: 6350 RVA: 0x00059E88 File Offset: 0x00058088
		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<int>((int)player.life.virus, this.virus);
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x00059EA1 File Offset: 0x000580A1
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return null;
			}
			return Local.FormatText(this.text, player.life.virus, this.virus);
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x00059ED8 File Offset: 0x000580D8
		public NPCPlayerLifeVirusCondition(int newVirus, ENPCLogicType newLogicType, string newText) : base(newLogicType, newText, false)
		{
			this.virus = newVirus;
		}
	}
}
