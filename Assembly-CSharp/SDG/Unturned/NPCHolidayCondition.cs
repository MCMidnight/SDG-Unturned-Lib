using System;

namespace SDG.Unturned
{
	// Token: 0x0200032B RID: 811
	public class NPCHolidayCondition : NPCLogicCondition
	{
		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600185A RID: 6234 RVA: 0x00058DDF File Offset: 0x00056FDF
		// (set) Token: 0x0600185B RID: 6235 RVA: 0x00058DE7 File Offset: 0x00056FE7
		public ENPCHoliday holiday { get; protected set; }

		// Token: 0x0600185C RID: 6236 RVA: 0x00058DF0 File Offset: 0x00056FF0
		public override bool isConditionMet(Player player)
		{
			return base.doesLogicPass<ENPCHoliday>(HolidayUtil.getActiveHoliday(), this.holiday);
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x00058E03 File Offset: 0x00057003
		public NPCHolidayCondition(ENPCHoliday newHoliday, ENPCLogicType newLogicType) : base(newLogicType, null, false)
		{
			this.holiday = newHoliday;
		}
	}
}
