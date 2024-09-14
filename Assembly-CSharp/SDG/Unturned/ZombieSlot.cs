using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000526 RID: 1318
	public class ZombieSlot
	{
		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002936 RID: 10550 RVA: 0x000AF7CC File Offset: 0x000AD9CC
		public List<ZombieCloth> table
		{
			get
			{
				return this._table;
			}
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x000AF7D4 File Offset: 0x000AD9D4
		public void addCloth(ushort id)
		{
			if (this.table.Count == 255)
			{
				return;
			}
			byte b = 0;
			while ((int)b < this.table.Count)
			{
				if (this.table[(int)b].item == id)
				{
					return;
				}
				b += 1;
			}
			this.table.Add(new ZombieCloth(id));
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000AF831 File Offset: 0x000ADA31
		public void removeCloth(byte index)
		{
			this.table.RemoveAt((int)index);
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x000AF83F File Offset: 0x000ADA3F
		public ZombieSlot(float newChance, List<ZombieCloth> newTable)
		{
			this._table = newTable;
			this.chance = newChance;
		}

		// Token: 0x040015F3 RID: 5619
		private List<ZombieCloth> _table;

		// Token: 0x040015F4 RID: 5620
		public float chance;
	}
}
