using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000514 RID: 1300
	public class VehicleTable
	{
		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x060028A6 RID: 10406 RVA: 0x000AD0E6 File Offset: 0x000AB2E6
		public List<VehicleTier> tiers
		{
			get
			{
				return this._tiers;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x060028A7 RID: 10407 RVA: 0x000AD0EE File Offset: 0x000AB2EE
		// (set) Token: 0x060028A8 RID: 10408 RVA: 0x000AD0F8 File Offset: 0x000AB2F8
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
				while ((int)num < LevelVehicles.spawns.Count)
				{
					VehicleSpawnpoint vehicleSpawnpoint = LevelVehicles.spawns[(int)num];
					if (vehicleSpawnpoint.type == EditorSpawns.selectedVehicle)
					{
						vehicleSpawnpoint.node.GetComponent<Renderer>().material.color = this.color;
						vehicleSpawnpoint.node.Find("Arrow").GetComponent<Renderer>().material.color = this.color;
					}
					num += 1;
				}
				EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = this.color;
				EditorSpawns.vehicleSpawn.Find("Arrow").GetComponent<Renderer>().material.color = this.color;
			}
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000AD1BC File Offset: 0x000AB3BC
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
				this.tiers.Add(new VehicleTier(new List<VehicleSpawn>(), name, 1f));
				return;
			}
			this.tiers.Add(new VehicleTier(new List<VehicleSpawn>(), name, 0f));
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000AD250 File Offset: 0x000AB450
		public void removeTier(int tierIndex)
		{
			this.updateChance(tierIndex, 0f);
			this.tiers.RemoveAt(tierIndex);
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000AD26A File Offset: 0x000AB46A
		public void addVehicle(byte tierIndex, ushort id)
		{
			this.tiers[(int)tierIndex].addVehicle(id);
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000AD27E File Offset: 0x000AB47E
		public void removeVehicle(byte tierIndex, byte vehicleIndex)
		{
			this.tiers[(int)tierIndex].removeVehicle(vehicleIndex);
		}

		/// <summary>
		/// Resolve spawn table asset if set, otherwise find asset by legacy in-editor ID configuration.
		/// Returned asset is not necessarily a vehicle asset yet: It can also be a VehicleRedirectorAsset which the
		/// vehicle spawner requires to properly set paint color.
		/// </summary>
		// Token: 0x060028AD RID: 10413 RVA: 0x000AD292 File Offset: 0x000AB492
		public Asset GetRandomAsset()
		{
			if (this.tableID != 0)
			{
				return SpawnTableTool.Resolve(this.tableID, EAssetType.VEHICLE, new Func<string>(this.OnGetSpawnTableErrorContext));
			}
			return Assets.find(EAssetType.VEHICLE, this.GetRandomLegacyVehicleId());
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000AD2C4 File Offset: 0x000AB4C4
		public void buildTable()
		{
			List<VehicleTier> list = new List<VehicleTier>();
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

		// Token: 0x060028AF RID: 10415 RVA: 0x000AD3B4 File Offset: 0x000AB5B4
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

		// Token: 0x060028B0 RID: 10416 RVA: 0x000AD62B File Offset: 0x000AB82B
		public VehicleTable(string newName)
		{
			this._tiers = new List<VehicleTier>();
			this._color = Color.white;
			this.name = newName;
			this.tableID = 0;
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000AD657 File Offset: 0x000AB857
		public VehicleTable(List<VehicleTier> newTiers, Color newColor, string newName, ushort newTableID)
		{
			this._tiers = newTiers;
			this._color = newColor;
			this.name = newName;
			this.tableID = newTableID;
		}

		/// <summary>
		/// Used when spawn table asset is not assigned. Pick a random legacy ID using in-editor list of spawns.
		/// </summary>
		// Token: 0x060028B2 RID: 10418 RVA: 0x000AD67C File Offset: 0x000AB87C
		private ushort GetRandomLegacyVehicleId()
		{
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
					VehicleTier vehicleTier = this.tiers[i];
					if (vehicleTier.table.Count > 0)
					{
						return vehicleTier.table[Random.Range(0, vehicleTier.table.Count)].vehicle;
					}
					return 0;
				}
				else
				{
					i++;
				}
			}
			VehicleTier vehicleTier2 = this.tiers[Random.Range(0, this.tiers.Count)];
			if (vehicleTier2.table.Count > 0)
			{
				return vehicleTier2.table[Random.Range(0, vehicleTier2.table.Count)].vehicle;
			}
			return 0;
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x000AD755 File Offset: 0x000AB955
		private string OnGetSpawnTableErrorContext()
		{
			return string.Concat(new string[]
			{
				"\"",
				Level.info.name,
				"\" vehicle table \"",
				this.name,
				"\""
			});
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000AD790 File Offset: 0x000AB990
		internal string OnGetSpawnTableValidationErrorContext()
		{
			return string.Concat(new string[]
			{
				"\"",
				Level.info.name,
				"\" vehicle table \"",
				this.name,
				"\" validation"
			});
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x000AD7CB File Offset: 0x000AB9CB
		[Obsolete("GetRandomAsset should be used instead because it properly supports guids in spawn assets.")]
		public ushort getVehicle()
		{
			if (this.tableID != 0)
			{
				return SpawnTableTool.ResolveLegacyId(this.tableID, EAssetType.VEHICLE, new Func<string>(this.OnGetSpawnTableErrorContext));
			}
			return this.GetRandomLegacyVehicleId();
		}

		// Token: 0x040015A2 RID: 5538
		private List<VehicleTier> _tiers;

		// Token: 0x040015A3 RID: 5539
		private Color _color;

		// Token: 0x040015A4 RID: 5540
		public string name;

		// Token: 0x040015A5 RID: 5541
		public ushort tableID;
	}
}
