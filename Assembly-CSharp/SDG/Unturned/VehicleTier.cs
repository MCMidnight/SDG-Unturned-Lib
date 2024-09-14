using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000515 RID: 1301
	public class VehicleTier
	{
		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x000AD7F4 File Offset: 0x000AB9F4
		public List<VehicleSpawn> table
		{
			get
			{
				return this._table;
			}
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x000AD7FC File Offset: 0x000AB9FC
		public void addVehicle(ushort id)
		{
			if (this.table.Count == 255)
			{
				return;
			}
			byte b = 0;
			while ((int)b < this.table.Count)
			{
				if (this.table[(int)b].vehicle == id)
				{
					return;
				}
				b += 1;
			}
			this.table.Add(new VehicleSpawn(id));
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x000AD859 File Offset: 0x000ABA59
		public void removeVehicle(byte index)
		{
			this.table.RemoveAt((int)index);
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x000AD867 File Offset: 0x000ABA67
		public VehicleTier(List<VehicleSpawn> newTable, string newName, float newChance)
		{
			this._table = newTable;
			this.name = newName;
			this.chance = newChance;
		}

		// Token: 0x040015A6 RID: 5542
		private List<VehicleSpawn> _table;

		// Token: 0x040015A7 RID: 5543
		public string name;

		// Token: 0x040015A8 RID: 5544
		public float chance;
	}
}
