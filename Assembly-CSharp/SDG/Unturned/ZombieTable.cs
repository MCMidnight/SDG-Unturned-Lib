using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200052A RID: 1322
	public class ZombieTable
	{
		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002949 RID: 10569 RVA: 0x000AFD62 File Offset: 0x000ADF62
		public ZombieSlot[] slots
		{
			get
			{
				return this._slots;
			}
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x0600294A RID: 10570 RVA: 0x000AFD6A File Offset: 0x000ADF6A
		// (set) Token: 0x0600294B RID: 10571 RVA: 0x000AFD74 File Offset: 0x000ADF74
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
						while ((int)num < LevelZombies.spawns[(int)b, (int)b2].Count)
						{
							ZombieSpawnpoint zombieSpawnpoint = LevelZombies.spawns[(int)b, (int)b2][(int)num];
							if (zombieSpawnpoint.type == EditorSpawns.selectedZombie)
							{
								zombieSpawnpoint.node.GetComponent<Renderer>().material.color = this.color;
							}
							num += 1;
						}
						EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = this.color;
					}
				}
			}
		}

		/// <summary>
		/// ID unique to this zombie table in the level. If this table is deleted the ID will not be recycled. Used to
		/// refer to zombie table from external files, e.g., NPC zombie kills condition.
		/// </summary>
		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x0600294C RID: 10572 RVA: 0x000AFE22 File Offset: 0x000AE022
		// (set) Token: 0x0600294D RID: 10573 RVA: 0x000AFE2A File Offset: 0x000AE02A
		public int tableUniqueId { get; private set; }

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x0600294E RID: 10574 RVA: 0x000AFE33 File Offset: 0x000AE033
		// (set) Token: 0x0600294F RID: 10575 RVA: 0x000AFE3C File Offset: 0x000AE03C
		public string difficultyGUID
		{
			get
			{
				return this._difficultyGUID;
			}
			set
			{
				this._difficultyGUID = value;
				try
				{
					this.difficulty = new AssetReference<ZombieDifficultyAsset>(new Guid(this.difficultyGUID));
				}
				catch
				{
					this.difficulty = AssetReference<ZombieDifficultyAsset>.invalid;
				}
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002950 RID: 10576 RVA: 0x000AFE88 File Offset: 0x000AE088
		// (set) Token: 0x06002951 RID: 10577 RVA: 0x000AFE90 File Offset: 0x000AE090
		public AssetReference<ZombieDifficultyAsset> difficulty { get; private set; }

		// Token: 0x06002952 RID: 10578 RVA: 0x000AFE9C File Offset: 0x000AE09C
		public ZombieDifficultyAsset resolveDifficulty()
		{
			if (this.cachedDifficulty == null && this.difficulty.isValid)
			{
				this.cachedDifficulty = Assets.find<ZombieDifficultyAsset>(this.difficulty);
			}
			return this.cachedDifficulty;
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x000AFED8 File Offset: 0x000AE0D8
		public void addCloth(byte slotIndex, ushort id)
		{
			this.slots[(int)slotIndex].addCloth(id);
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x000AFEE8 File Offset: 0x000AE0E8
		public void removeCloth(byte slotIndex, byte clothIndex)
		{
			this.slots[(int)slotIndex].removeCloth(clothIndex);
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x000AFEF8 File Offset: 0x000AE0F8
		internal void GetSpawnClothingParameters(out byte shirt, out byte pants, out byte hat, out byte gear)
		{
			shirt = byte.MaxValue;
			if (this.slots[0].table.Count > 0 && Random.value < this.slots[0].chance)
			{
				shirt = (byte)Random.Range(0, this.slots[0].table.Count);
			}
			pants = byte.MaxValue;
			if (this.slots[1].table.Count > 0 && Random.value < this.slots[1].chance)
			{
				pants = (byte)Random.Range(0, this.slots[1].table.Count);
			}
			hat = byte.MaxValue;
			if (this.slots[2].table.Count > 0 && Random.value < this.slots[2].chance)
			{
				hat = (byte)Random.Range(0, this.slots[2].table.Count);
			}
			gear = byte.MaxValue;
			if (this.slots[3].table.Count > 0 && Random.value < this.slots[3].chance)
			{
				gear = (byte)Random.Range(0, this.slots[3].table.Count);
			}
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000B0034 File Offset: 0x000AE234
		public ZombieTable(string newName)
		{
			this._slots = new ZombieSlot[4];
			for (int i = 0; i < this.slots.Length; i++)
			{
				this.slots[i] = new ZombieSlot(1f, new List<ZombieCloth>());
			}
			this._color = Color.white;
			this.name = newName;
			this.isMega = false;
			this.health = 100;
			this.damage = 15;
			this.lootIndex = 0;
			this.lootID = 0;
			this.xp = 3U;
			this.regen = 10f;
			this.difficultyGUID = string.Empty;
			this.tableUniqueId = LevelZombies.GenerateTableUniqueId();
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x000B00DC File Offset: 0x000AE2DC
		public ZombieTable(ZombieSlot[] newSlots, Color newColor, string newName, bool newMega, ushort newHealth, byte newDamage, byte newLootIndex, ushort newLootID, uint newXP, float newRegen, string newDifficultyGUID, int newTableUniqueId)
		{
			this._slots = newSlots;
			this._color = newColor;
			this.name = newName;
			this.isMega = newMega;
			this.health = newHealth;
			this.damage = newDamage;
			this.lootIndex = newLootIndex;
			this.lootID = newLootID;
			this.xp = newXP;
			this.regen = newRegen;
			this.difficultyGUID = newDifficultyGUID;
			this.tableUniqueId = newTableUniqueId;
		}

		// Token: 0x0400160B RID: 5643
		private ZombieSlot[] _slots;

		// Token: 0x0400160C RID: 5644
		private Color _color;

		// Token: 0x0400160D RID: 5645
		public string name;

		// Token: 0x0400160F RID: 5647
		public bool isMega;

		// Token: 0x04001610 RID: 5648
		public ushort health;

		// Token: 0x04001611 RID: 5649
		public byte damage;

		// Token: 0x04001612 RID: 5650
		public byte lootIndex;

		// Token: 0x04001613 RID: 5651
		public ushort lootID;

		// Token: 0x04001614 RID: 5652
		public uint xp;

		// Token: 0x04001615 RID: 5653
		public float regen;

		// Token: 0x04001616 RID: 5654
		private string _difficultyGUID;

		// Token: 0x04001618 RID: 5656
		private ZombieDifficultyAsset cachedDifficulty;
	}
}
