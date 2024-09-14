using System;

namespace SDG.Unturned
{
	// Token: 0x02000322 RID: 802
	public class NPCDateCounterCondition : NPCLogicCondition
	{
		// Token: 0x06001832 RID: 6194 RVA: 0x000589C0 File Offset: 0x00056BC0
		public override bool isConditionMet(Player player)
		{
			long a = LightingManager.DateCounter % this.divisor;
			return NPCTool.doesLogicPass<long>(base.logicType, a, this.value);
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x000589EC File Offset: 0x00056BEC
		public NPCDateCounterCondition(long newValue, long newDivisor, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newLogicType, newText, newShouldReset)
		{
			this.value = newValue;
			this.divisor = newDivisor;
		}

		// Token: 0x04000AEA RID: 2794
		protected long value;

		// Token: 0x04000AEB RID: 2795
		protected long divisor;
	}
}
