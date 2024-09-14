using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004A8 RID: 1192
	public class AnimalTable
	{
		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060024F6 RID: 9462 RVA: 0x000932F4 File Offset: 0x000914F4
		public List<AnimalTier> tiers
		{
			get
			{
				return this._tiers;
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060024F7 RID: 9463 RVA: 0x000932FC File Offset: 0x000914FC
		// (set) Token: 0x060024F8 RID: 9464 RVA: 0x00093304 File Offset: 0x00091504
		public Color color
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
				ushort num = 0;
				while ((int)num < LevelAnimals.spawns.Count)
				{
					AnimalSpawnpoint animalSpawnpoint = LevelAnimals.spawns[(int)num];
					if (animalSpawnpoint.type == EditorSpawns.selectedAnimal)
					{
						animalSpawnpoint.node.GetComponent<Renderer>().material.color = this.color;
					}
					num += 1;
				}
				EditorSpawns.animalSpawn.GetComponent<Renderer>().material.color = this.color;
			}
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x0009337C File Offset: 0x0009157C
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
				this.tiers.Add(new AnimalTier(new List<AnimalSpawn>(), name, 1f));
				return;
			}
			this.tiers.Add(new AnimalTier(new List<AnimalSpawn>(), name, 0f));
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x00093410 File Offset: 0x00091610
		public void removeTier(int tierIndex)
		{
			this.updateChance(tierIndex, 0f);
			this.tiers.RemoveAt(tierIndex);
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x0009342A File Offset: 0x0009162A
		public void addAnimal(byte tierIndex, ushort id)
		{
			this.tiers[(int)tierIndex].addAnimal(id);
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x0009343E File Offset: 0x0009163E
		public void removeAnimal(byte tierIndex, byte animalIndex)
		{
			this.tiers[(int)tierIndex].removeAnimal(animalIndex);
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x00093454 File Offset: 0x00091654
		public ushort getAnimal()
		{
			if (this.tableID != 0)
			{
				return SpawnTableTool.ResolveLegacyId(this.tableID, EAssetType.ANIMAL, new Func<string>(this.OnGetSpawnTableErrorContext));
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
					AnimalTier animalTier = this.tiers[i];
					if (animalTier.table.Count > 0)
					{
						return animalTier.table[Random.Range(0, animalTier.table.Count)].animal;
					}
					return 0;
				}
				else
				{
					i++;
				}
			}
			AnimalTier animalTier2 = this.tiers[Random.Range(0, this.tiers.Count)];
			if (animalTier2.table.Count > 0)
			{
				return animalTier2.table[Random.Range(0, animalTier2.table.Count)].animal;
			}
			return 0;
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x00093550 File Offset: 0x00091750
		public void buildTable()
		{
			List<AnimalTier> list = new List<AnimalTier>();
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

		// Token: 0x060024FF RID: 9471 RVA: 0x00093640 File Offset: 0x00091840
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

		// Token: 0x06002500 RID: 9472 RVA: 0x000938B7 File Offset: 0x00091AB7
		public AnimalTable(string newName)
		{
			this._tiers = new List<AnimalTier>();
			this._color = Color.white;
			this.name = newName;
			this.tableID = 0;
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000938E3 File Offset: 0x00091AE3
		public AnimalTable(List<AnimalTier> newTiers, Color newColor, string newName, ushort newTableID)
		{
			this._tiers = newTiers;
			this._color = newColor;
			this.name = newName;
			this.tableID = newTableID;
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x00093908 File Offset: 0x00091B08
		private string OnGetSpawnTableErrorContext()
		{
			return string.Concat(new string[]
			{
				"\"",
				Level.info.name,
				"\" animal table \"",
				this.name,
				"\""
			});
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x00093943 File Offset: 0x00091B43
		internal string OnGetSpawnTableValidationErrorContext()
		{
			return string.Concat(new string[]
			{
				"\"",
				Level.info.name,
				"\" animal table \"",
				this.name,
				"\" validation"
			});
		}

		// Token: 0x040012C8 RID: 4808
		private List<AnimalTier> _tiers;

		// Token: 0x040012C9 RID: 4809
		private Color _color;

		// Token: 0x040012CA RID: 4810
		public string name;

		// Token: 0x040012CB RID: 4811
		public ushort tableID;
	}
}
