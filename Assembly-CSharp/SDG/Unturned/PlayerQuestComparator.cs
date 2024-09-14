using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000648 RID: 1608
	public class PlayerQuestComparator : IComparer<PlayerQuest>
	{
		// Token: 0x060034B9 RID: 13497 RVA: 0x000F42BA File Offset: 0x000F24BA
		public int Compare(PlayerQuest a, PlayerQuest b)
		{
			return (int)(a.id - b.id);
		}
	}
}
