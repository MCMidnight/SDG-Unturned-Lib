using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004E4 RID: 1252
	public class LevelItems
	{
		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x0600269D RID: 9885 RVA: 0x0009DB00 File Offset: 0x0009BD00
		[Obsolete("Was the parent of all items in the past, but now empty for TransformHierarchy performance.")]
		public static Transform models
		{
			get
			{
				if (LevelItems._models == null)
				{
					LevelItems._models = new GameObject().transform;
					LevelItems._models.name = "Items";
					LevelItems._models.parent = Level.spawns;
					LevelItems._models.tag = "Logic";
					LevelItems._models.gameObject.layer = 8;
				}
				return LevelItems._models;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x0600269E RID: 9886 RVA: 0x0009DB6B File Offset: 0x0009BD6B
		public static List<ItemTable> tables
		{
			get
			{
				return LevelItems._tables;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x0600269F RID: 9887 RVA: 0x0009DB72 File Offset: 0x0009BD72
		public static List<ItemSpawnpoint>[,] spawns
		{
			get
			{
				return LevelItems._spawns;
			}
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x0009DB7C File Offset: 0x0009BD7C
		public static void setEnabled(bool isEnabled)
		{
			if (LevelItems.spawns == null)
			{
				return;
			}
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					for (int i = 0; i < LevelItems.spawns[(int)b, (int)b2].Count; i++)
					{
						LevelItems.spawns[(int)b, (int)b2][i].setEnabled(isEnabled);
					}
				}
			}
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x0009DBE7 File Offset: 0x0009BDE7
		public static void addTable(string name)
		{
			if (LevelItems.tables.Count == 255)
			{
				return;
			}
			LevelItems.tables.Add(new ItemTable(name));
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x0009DC0C File Offset: 0x0009BE0C
		public static void removeTable()
		{
			LevelItems.tables.RemoveAt((int)EditorSpawns.selectedItem);
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					List<ItemSpawnpoint> list = new List<ItemSpawnpoint>();
					for (int i = 0; i < LevelItems.spawns[(int)b, (int)b2].Count; i++)
					{
						ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int)b, (int)b2][i];
						if (itemSpawnpoint.type == EditorSpawns.selectedItem)
						{
							Object.Destroy(itemSpawnpoint.node.gameObject);
						}
						else
						{
							if (itemSpawnpoint.type > EditorSpawns.selectedItem)
							{
								ItemSpawnpoint itemSpawnpoint2 = itemSpawnpoint;
								itemSpawnpoint2.type -= 1;
							}
							list.Add(itemSpawnpoint);
						}
					}
					LevelItems._spawns[(int)b, (int)b2] = list;
				}
			}
			for (int j = 0; j < LevelZombies.tables.Count; j++)
			{
				ZombieTable zombieTable = LevelZombies.tables[j];
				if (zombieTable.lootIndex > EditorSpawns.selectedItem)
				{
					ZombieTable zombieTable2 = zombieTable;
					zombieTable2.lootIndex -= 1;
				}
			}
			EditorSpawns.selectedItem = 0;
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				EditorSpawns.itemSpawn.GetComponent<Renderer>().material.color = LevelItems.tables[(int)EditorSpawns.selectedItem].color;
			}
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x0009DD64 File Offset: 0x0009BF64
		public static void addSpawn(Vector3 point)
		{
			byte b;
			byte b2;
			if (!Regions.tryGetCoordinate(point, out b, out b2))
			{
				return;
			}
			if ((int)EditorSpawns.selectedItem >= LevelItems.tables.Count)
			{
				return;
			}
			LevelItems.spawns[(int)b, (int)b2].Add(new ItemSpawnpoint(EditorSpawns.selectedItem, point));
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x0009DDAC File Offset: 0x0009BFAC
		public static void removeSpawn(Vector3 point, float radius)
		{
			radius *= radius;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					List<ItemSpawnpoint> list = new List<ItemSpawnpoint>();
					for (int i = 0; i < LevelItems.spawns[(int)b, (int)b2].Count; i++)
					{
						ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int)b, (int)b2][i];
						if ((itemSpawnpoint.point - point).sqrMagnitude < radius)
						{
							Object.Destroy(itemSpawnpoint.node.gameObject);
						}
						else
						{
							list.Add(itemSpawnpoint);
						}
					}
					LevelItems._spawns[(int)b, (int)b2] = list;
				}
			}
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x0009DE60 File Offset: 0x0009C060
		public static ushort getItem(ItemSpawnpoint spawn)
		{
			return LevelItems.getItem(spawn.type);
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x0009DE6D File Offset: 0x0009C06D
		public static ushort getItem(byte type)
		{
			return LevelItems.tables[(int)type].getItem();
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x0009DE80 File Offset: 0x0009C080
		public static void load()
		{
			if (Level.isEditor || Provider.isServer)
			{
				LevelItems._tables = new List<ItemTable>();
				LevelItems._spawns = new List<ItemSpawnpoint>[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				if (ReadWrite.fileExists(Level.info.path + "/Spawns/Items.dat", false, false))
				{
					Block block = ReadWrite.readBlock(Level.info.path + "/Spawns/Items.dat", false, false, 0);
					byte b = block.readByte();
					if (b > 1 && b < 3)
					{
						block.readSteamID();
					}
					byte b2 = block.readByte();
					for (byte b3 = 0; b3 < b2; b3 += 1)
					{
						Color newColor = block.readColor();
						string text = block.readString();
						ushort num;
						if (b > 3)
						{
							num = block.readUInt16();
						}
						else
						{
							num = 0;
						}
						List<ItemTier> list = new List<ItemTier>();
						byte b4 = block.readByte();
						for (byte b5 = 0; b5 < b4; b5 += 1)
						{
							string newName = block.readString();
							float newChance = block.readSingle();
							List<ItemSpawn> list2 = new List<ItemSpawn>();
							byte b6 = block.readByte();
							for (byte b7 = 0; b7 < b6; b7 += 1)
							{
								ushort num2 = block.readUInt16();
								ItemAsset itemAsset = Assets.find(EAssetType.ITEM, num2) as ItemAsset;
								if (itemAsset != null && !itemAsset.isPro)
								{
									list2.Add(new ItemSpawn(num2));
								}
							}
							if (list2.Count > 0)
							{
								list.Add(new ItemTier(list2, newName, newChance));
							}
						}
						ItemTable itemTable = new ItemTable(list, newColor, text, num);
						LevelItems.tables.Add(itemTable);
						if (!Level.isEditor)
						{
							itemTable.buildTable();
						}
						if (itemTable.tableID != 0 && SpawnTableTool.ResolveLegacyId(num, EAssetType.ITEM, new Func<string>(itemTable.OnGetSpawnTableValidationErrorContext)) == 0 && Assets.shouldLoadAnyAssets)
						{
							Assets.reportError(string.Concat(new string[]
							{
								Level.info.name,
								" item table \"",
								text,
								"\" references invalid spawn table ",
								num.ToString(),
								"!"
							}));
						}
					}
				}
				for (byte b8 = 0; b8 < Regions.WORLD_SIZE; b8 += 1)
				{
					for (byte b9 = 0; b9 < Regions.WORLD_SIZE; b9 += 1)
					{
						LevelItems.spawns[(int)b8, (int)b9] = new List<ItemSpawnpoint>();
					}
				}
				if (ReadWrite.fileExists(Level.info.path + "/Spawns/Jars.dat", false, false))
				{
					River river = new River(Level.info.path + "/Spawns/Jars.dat", false);
					if (river.readByte() > 0)
					{
						for (byte b10 = 0; b10 < Regions.WORLD_SIZE; b10 += 1)
						{
							for (byte b11 = 0; b11 < Regions.WORLD_SIZE; b11 += 1)
							{
								ushort num3 = river.readUInt16();
								for (ushort num4 = 0; num4 < num3; num4 += 1)
								{
									byte newType = river.readByte();
									Vector3 newPoint = river.readSingleVector3();
									LevelItems.spawns[(int)b10, (int)b11].Add(new ItemSpawnpoint(newType, newPoint));
								}
							}
						}
					}
					river.closeRiver();
					return;
				}
				for (byte b12 = 0; b12 < Regions.WORLD_SIZE; b12 += 1)
				{
					for (byte b13 = 0; b13 < Regions.WORLD_SIZE; b13 += 1)
					{
						LevelItems.spawns[(int)b12, (int)b13] = new List<ItemSpawnpoint>();
						if (ReadWrite.fileExists(string.Concat(new string[]
						{
							Level.info.path,
							"/Spawns/Items_",
							b12.ToString(),
							"_",
							b13.ToString(),
							".dat"
						}), false, false))
						{
							River river2 = new River(string.Concat(new string[]
							{
								Level.info.path,
								"/Spawns/Items_",
								b12.ToString(),
								"_",
								b13.ToString(),
								".dat"
							}), false);
							if (river2.readByte() > 0)
							{
								ushort num5 = river2.readUInt16();
								for (ushort num6 = 0; num6 < num5; num6 += 1)
								{
									byte newType2 = river2.readByte();
									Vector3 newPoint2 = river2.readSingleVector3();
									LevelItems.spawns[(int)b12, (int)b13].Add(new ItemSpawnpoint(newType2, newPoint2));
								}
							}
							river2.closeRiver();
						}
					}
				}
			}
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x0009E2C8 File Offset: 0x0009C4C8
		public static void save()
		{
			Block block = new Block();
			block.writeByte(LevelItems.SAVEDATA_VERSION);
			block.writeByte((byte)LevelItems.tables.Count);
			byte b = 0;
			while ((int)b < LevelItems.tables.Count)
			{
				ItemTable itemTable = LevelItems.tables[(int)b];
				block.writeColor(itemTable.color);
				block.writeString(itemTable.name);
				block.writeUInt16(itemTable.tableID);
				block.write((byte)itemTable.tiers.Count);
				byte b2 = 0;
				while ((int)b2 < itemTable.tiers.Count)
				{
					ItemTier itemTier = itemTable.tiers[(int)b2];
					block.writeString(itemTier.name);
					block.writeSingle(itemTier.chance);
					block.writeByte((byte)itemTier.table.Count);
					byte b3 = 0;
					while ((int)b3 < itemTier.table.Count)
					{
						ItemSpawn itemSpawn = itemTier.table[(int)b3];
						block.writeUInt16(itemSpawn.item);
						b3 += 1;
					}
					b2 += 1;
				}
				b += 1;
			}
			ReadWrite.writeBlock(Level.info.path + "/Spawns/Items.dat", false, false, block);
			River river = new River(Level.info.path + "/Spawns/Jars.dat", false);
			river.writeByte(LevelItems.SAVEDATA_VERSION);
			for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
			{
				for (byte b5 = 0; b5 < Regions.WORLD_SIZE; b5 += 1)
				{
					List<ItemSpawnpoint> list = LevelItems.spawns[(int)b4, (int)b5];
					river.writeUInt16((ushort)list.Count);
					ushort num = 0;
					while ((int)num < list.Count)
					{
						ItemSpawnpoint itemSpawnpoint = list[(int)num];
						river.writeByte(itemSpawnpoint.type);
						river.writeSingleVector3(itemSpawnpoint.point);
						num += 1;
					}
				}
			}
			river.closeRiver();
		}

		// Token: 0x04001437 RID: 5175
		public static readonly byte SAVEDATA_VERSION = 4;

		// Token: 0x04001438 RID: 5176
		private static Transform _models;

		// Token: 0x04001439 RID: 5177
		private static List<ItemTable> _tables;

		// Token: 0x0400143A RID: 5178
		private static List<ItemSpawnpoint>[,] _spawns;
	}
}
