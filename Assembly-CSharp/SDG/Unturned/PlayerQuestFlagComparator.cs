using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x0200064A RID: 1610
	public class PlayerQuestFlagComparator : IComparer<PlayerQuestFlag>
	{
		// Token: 0x060034C1 RID: 13505 RVA: 0x000F433B File Offset: 0x000F253B
		public int Compare(PlayerQuestFlag a, PlayerQuestFlag b)
		{
			return (int)(a.id - b.id);
		}
	}
}
