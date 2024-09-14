using System;

namespace SDG.Unturned
{
	// Token: 0x02000333 RID: 819
	public class NPCPlayerLifeFoodCondition : NPCLogicCondition
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x00059BD1 File Offset: 0x00057DD1
		// (set) Token: 0x060018AF RID: 6319 RVA: 0x00059BD9 File Offset: 0x00057DD9
		public int food { get; protected set; }

		// Token: 0x060018B0 RID: 6320 RVA: 0x00059BE2 File Offset: 0x00057DE2
		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<int>((int)player.life.food, this.food);
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x00059BFB File Offset: 0x00057DFB
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return null;
			}
			return Local.FormatText(this.text, player.life.food, this.food);
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x00059C32 File Offset: 0x00057E32
		public NPCPlayerLifeFoodCondition(int newFood, ENPCLogicType newLogicType, string newText) : base(newLogicType, newText, false)
		{
			this.food = newFood;
		}
	}
}
