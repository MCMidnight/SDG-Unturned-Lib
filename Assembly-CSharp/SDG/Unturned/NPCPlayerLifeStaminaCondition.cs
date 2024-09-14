using System;

namespace SDG.Unturned
{
	// Token: 0x02000337 RID: 823
	public class NPCPlayerLifeStaminaCondition : NPCLogicCondition
	{
		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060018C2 RID: 6338 RVA: 0x00059D95 File Offset: 0x00057F95
		// (set) Token: 0x060018C3 RID: 6339 RVA: 0x00059D9D File Offset: 0x00057F9D
		public int stamina { get; protected set; }

		// Token: 0x060018C4 RID: 6340 RVA: 0x00059DA6 File Offset: 0x00057FA6
		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<int>((int)player.life.stamina, this.stamina);
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x00059DBF File Offset: 0x00057FBF
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return null;
			}
			return Local.FormatText(this.text, player.life.stamina, this.stamina);
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x00059DF6 File Offset: 0x00057FF6
		public NPCPlayerLifeStaminaCondition(int newStamina, ENPCLogicType newLogicType, string newText) : base(newLogicType, newText, false)
		{
			this.stamina = newStamina;
		}
	}
}
