using System;

namespace SDG.Unturned
{
	// Token: 0x02000330 RID: 816
	public class NPCLogicCondition : INPCCondition
	{
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001892 RID: 6290 RVA: 0x00059966 File Offset: 0x00057B66
		// (set) Token: 0x06001893 RID: 6291 RVA: 0x0005996E File Offset: 0x00057B6E
		public ENPCLogicType logicType { get; protected set; }

		// Token: 0x06001894 RID: 6292 RVA: 0x00059977 File Offset: 0x00057B77
		protected bool doesLogicPass<T>(T a, T b) where T : IComparable
		{
			return NPCTool.doesLogicPass<T>(this.logicType, a, b);
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x00059986 File Offset: 0x00057B86
		public NPCLogicCondition(ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newText, newShouldReset)
		{
			this.logicType = newLogicType;
		}
	}
}
