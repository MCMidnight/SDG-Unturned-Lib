using System;

namespace SDG.Unturned
{
	// Token: 0x0200033B RID: 827
	public class NPCPlayerLifeWaterCondition : NPCLogicCondition
	{
		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060018D6 RID: 6358 RVA: 0x00059F59 File Offset: 0x00058159
		// (set) Token: 0x060018D7 RID: 6359 RVA: 0x00059F61 File Offset: 0x00058161
		public int water { get; protected set; }

		// Token: 0x060018D8 RID: 6360 RVA: 0x00059F6A File Offset: 0x0005816A
		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<int>((int)player.life.water, this.water);
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00059F83 File Offset: 0x00058183
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return null;
			}
			return Local.FormatText(this.text, player.life.water, this.water);
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x00059FBA File Offset: 0x000581BA
		public NPCPlayerLifeWaterCondition(int newWater, ENPCLogicType newLogicType, string newText) : base(newLogicType, newText, false)
		{
			this.water = newWater;
		}
	}
}
