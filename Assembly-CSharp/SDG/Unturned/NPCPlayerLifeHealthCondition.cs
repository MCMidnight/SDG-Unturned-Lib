using System;

namespace SDG.Unturned
{
	// Token: 0x02000335 RID: 821
	public class NPCPlayerLifeHealthCondition : NPCLogicCondition
	{
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x00059CB3 File Offset: 0x00057EB3
		// (set) Token: 0x060018B9 RID: 6329 RVA: 0x00059CBB File Offset: 0x00057EBB
		public int health { get; protected set; }

		// Token: 0x060018BA RID: 6330 RVA: 0x00059CC4 File Offset: 0x00057EC4
		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<int>((int)player.life.health, this.health);
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00059CDD File Offset: 0x00057EDD
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return null;
			}
			return Local.FormatText(this.text, player.life.health, this.health);
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00059D14 File Offset: 0x00057F14
		public NPCPlayerLifeHealthCondition(int newHealth, ENPCLogicType newLogicType, string newText) : base(newLogicType, newText, false)
		{
			this.health = newHealth;
		}
	}
}
