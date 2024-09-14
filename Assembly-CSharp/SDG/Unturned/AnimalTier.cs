using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020004A9 RID: 1193
	public class AnimalTier
	{
		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002504 RID: 9476 RVA: 0x0009397E File Offset: 0x00091B7E
		public List<AnimalSpawn> table
		{
			get
			{
				return this._table;
			}
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x00093988 File Offset: 0x00091B88
		public void addAnimal(ushort id)
		{
			if (this.table.Count == 255)
			{
				return;
			}
			byte b = 0;
			while ((int)b < this.table.Count)
			{
				if (this.table[(int)b].animal == id)
				{
					return;
				}
				b += 1;
			}
			this.table.Add(new AnimalSpawn(id));
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x000939E5 File Offset: 0x00091BE5
		public void removeAnimal(byte index)
		{
			this.table.RemoveAt((int)index);
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x000939F3 File Offset: 0x00091BF3
		public AnimalTier(List<AnimalSpawn> newTable, string newName, float newChance)
		{
			this._table = newTable;
			this.name = newName;
			this.chance = newChance;
		}

		// Token: 0x040012CC RID: 4812
		private List<AnimalSpawn> _table;

		// Token: 0x040012CD RID: 4813
		public string name;

		// Token: 0x040012CE RID: 4814
		public float chance;
	}
}
