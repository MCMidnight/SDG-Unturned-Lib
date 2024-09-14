using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020004CF RID: 1231
	public class ItemTier
	{
		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x0600259F RID: 9631 RVA: 0x00095D06 File Offset: 0x00093F06
		public List<ItemSpawn> table
		{
			get
			{
				return this._table;
			}
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x00095D10 File Offset: 0x00093F10
		public void addItem(ushort id)
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
			this.table.Add(new ItemSpawn(id));
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x00095D6D File Offset: 0x00093F6D
		public void removeItem(byte index)
		{
			this.table.RemoveAt((int)index);
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x00095D7B File Offset: 0x00093F7B
		public ItemTier(List<ItemSpawn> newTable, string newName, float newChance)
		{
			this._table = newTable;
			this.name = newName;
			this.chance = newChance;
		}

		// Token: 0x04001353 RID: 4947
		private List<ItemSpawn> _table;

		// Token: 0x04001354 RID: 4948
		public string name;

		// Token: 0x04001355 RID: 4949
		public float chance;
	}
}
