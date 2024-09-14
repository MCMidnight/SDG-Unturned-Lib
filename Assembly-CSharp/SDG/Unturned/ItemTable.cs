using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004CE RID: 1230
	public class ItemTable
	{
		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002591 RID: 9617 RVA: 0x00095644 File Offset: 0x00093844
		public List<ItemTier> tiers
		{
			get
			{
				return this._tiers;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002592 RID: 9618 RVA: 0x0009564C File Offset: 0x0009384C
		// (set) Token: 0x06002593 RID: 9619 RVA: 0x00095654 File Offset: 0x00093854
		public Color color
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						ushort num = 0;
						while ((int)num < LevelItems.spawns[(int)b, (int)b2].Count)
						{
							ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int)b, (int)b2][(int)num];
							if (itemSpawnpoint.type == EditorSpawns.selectedItem)
							{
								itemSpawnpoint.node.GetComponent<Renderer>().material.color = this.color;
							}
							num += 1;
						}
						EditorSpawns.itemSpawn.GetComponent<Renderer>().material.color = this.color;
					}
				}
			}
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x00095704 File Offset: 0x00093904
		public void addTier(string name)
		{
			if (this.tiers.Count == 255)
			{
				return;
			}
			for (int i = 0; i < this.tiers.Count; i++)
			{
				if (this.tiers[i].name == name)
				{
					return;
				}
			}
			if (this.tiers.Count == 0)
			{
				this.tiers.Add(new ItemTier(new List<ItemSpawn>(), name, 1f));
				return;
			}
			this.tiers.Add(new ItemTier(new List<ItemSpawn>(), name, 0f));
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x00095798 File Offset: 0x00093998
		public void removeTier(int tierIndex)
		{
			this.updateChance(tierIndex, 0f);
			this.tiers.RemoveAt(tierIndex);
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x000957B2 File Offset: 0x000939B2
		public void addItem(byte tierIndex, ushort id)
		{
			this.tiers[(int)tierIndex].addItem(id);
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000957C6 File Offset: 0x000939C6
		public void removeItem(byte tierIndex, byte itemIndex)
		{
			this.tiers[(int)tierIndex].removeItem(itemIndex);
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x000957DC File Offset: 0x000939DC
		public ushort getItem()
		{
			if (this.tableID != 0)
			{
				return SpawnTableTool.ResolveLegacyId(this.tableID, EAssetType.ITEM, new Func<string>(this.OnGetSpawnTableErrorContext));
			}
			float value = Random.value;
			if (this.tiers.Count == 0)
			{
				return 0;
			}
			int i = 0;
			while (i < this.tiers.Count)
			{
				if (value < this.tiers[i].chance)
				{
					ItemTier itemTier = this.tiers[i];
					if (itemTier.table.Count > 0)
					{
						return itemTier.table[Random.Range(0, itemTier.table.Count)].item;
					}
					return 0;
				}
				else
				{
					i++;
				}
			}
			ItemTier itemTier2 = this.tiers[Random.Range(0, this.tiers.Count)];
			if (itemTier2.table.Count > 0)
			{
				return itemTier2.table[Random.Range(0, itemTier2.table.Count)].item;
			}
			return 0;
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000958D8 File Offset: 0x00093AD8
		public void buildTable()
		{
			List<ItemTier> list = new List<ItemTier>();
			for (int i = 0; i < this.tiers.Count; i++)
			{
				if (list.Count == 0)
				{
					list.Add(this.tiers[i]);
				}
				else
				{
					bool flag = false;
					for (int j = 0; j < list.Count; j++)
					{
						if (this.tiers[i].chance < list[j].chance)
						{
							flag = true;
							list.Insert(j, this.tiers[i]);
							break;
						}
					}
					if (!flag)
					{
						list.Add(this.tiers[i]);
					}
				}
			}
			float num = 0f;
			for (int k = 0; k < list.Count; k++)
			{
				num += list[k].chance;
				list[k].chance = num;
			}
			this._tiers = list;
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x000959C8 File Offset: 0x00093BC8
		public void updateChance(int tierIndex, float chance)
		{
			float num = chance - this.tiers[tierIndex].chance;
			this.tiers[tierIndex].chance = chance;
			float num2 = Mathf.Abs(num);
			while (num2 > 0.001f)
			{
				int num3 = 0;
				for (int i = 0; i < this.tiers.Count; i++)
				{
					if (((num < 0f && this.tiers[i].chance < 1f) || (num > 0f && this.tiers[i].chance > 0f)) && i != tierIndex)
					{
						num3++;
					}
				}
				if (num3 == 0)
				{
					break;
				}
				float num4 = num2 / (float)num3;
				for (int j = 0; j < this.tiers.Count; j++)
				{
					if (((num < 0f && this.tiers[j].chance < 1f) || (num > 0f && this.tiers[j].chance > 0f)) && j != tierIndex)
					{
						if (num > 0f)
						{
							if (this.tiers[j].chance >= num4)
							{
								num2 -= num4;
								this.tiers[j].chance -= num4;
							}
							else
							{
								num2 -= this.tiers[j].chance;
								this.tiers[j].chance = 0f;
							}
						}
						else if (this.tiers[j].chance <= 1f - num4)
						{
							num2 -= num4;
							this.tiers[j].chance += num4;
						}
						else
						{
							num2 -= 1f - this.tiers[j].chance;
							this.tiers[j].chance = 1f;
						}
					}
				}
			}
			float num5 = 0f;
			for (int k = 0; k < this.tiers.Count; k++)
			{
				num5 += this.tiers[k].chance;
			}
			for (int l = 0; l < this.tiers.Count; l++)
			{
				this.tiers[l].chance /= num5;
			}
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x00095C3F File Offset: 0x00093E3F
		public ItemTable(string newName)
		{
			this._tiers = new List<ItemTier>();
			this._color = Color.white;
			this.name = newName;
			this.tableID = 0;
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x00095C6B File Offset: 0x00093E6B
		public ItemTable(List<ItemTier> newTiers, Color newColor, string newName, ushort newTableID)
		{
			this._tiers = newTiers;
			this._color = newColor;
			this.name = newName;
			this.tableID = newTableID;
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x00095C90 File Offset: 0x00093E90
		private string OnGetSpawnTableErrorContext()
		{
			return string.Concat(new string[]
			{
				"\"",
				Level.info.name,
				"\" item table \"",
				this.name,
				"\""
			});
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x00095CCB File Offset: 0x00093ECB
		internal string OnGetSpawnTableValidationErrorContext()
		{
			return string.Concat(new string[]
			{
				"\"",
				Level.info.name,
				" item table \"",
				this.name,
				"\" validation"
			});
		}

		// Token: 0x0400134F RID: 4943
		private List<ItemTier> _tiers;

		// Token: 0x04001350 RID: 4944
		private Color _color;

		// Token: 0x04001351 RID: 4945
		public string name;

		// Token: 0x04001352 RID: 4946
		public ushort tableID;
	}
}
