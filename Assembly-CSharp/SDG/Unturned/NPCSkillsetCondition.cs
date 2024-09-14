using System;

namespace SDG.Unturned
{
	// Token: 0x02000348 RID: 840
	public class NPCSkillsetCondition : NPCLogicCondition
	{
		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001931 RID: 6449 RVA: 0x0005A93B File Offset: 0x00058B3B
		// (set) Token: 0x06001932 RID: 6450 RVA: 0x0005A943 File Offset: 0x00058B43
		public EPlayerSkillset skillset { get; protected set; }

		// Token: 0x06001933 RID: 6451 RVA: 0x0005A94C File Offset: 0x00058B4C
		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<EPlayerSkillset>(player.channel.owner.skillset, this.skillset);
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0005A96A File Offset: 0x00058B6A
		public NPCSkillsetCondition(EPlayerSkillset newSkillset, ENPCLogicType newLogicType, string newText) : base(newLogicType, newText, false)
		{
			this.skillset = newSkillset;
		}
	}
}
