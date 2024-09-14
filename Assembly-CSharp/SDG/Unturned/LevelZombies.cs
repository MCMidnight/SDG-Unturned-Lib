using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004F7 RID: 1271
	public class LevelZombies
	{
		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x060027EF RID: 10223 RVA: 0x000A8348 File Offset: 0x000A6548
		[Obsolete("Was the parent of all zombies in the past, but now empty for TransformHierarchy performance.")]
		public static Transform models
		{
			get
			{
				if (LevelZombies._models == null)
				{
					LevelZombies._models = new GameObject().transform;
					LevelZombies._models.name = "Zombies";
					LevelZombies._models.parent = Level.spawns;
					LevelZombies._models.tag = "Logic";
					LevelZombies._models.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing LevelZombies.models which has been deprecated.", Array.Empty<object>());
				}
				return LevelZombies._models;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060027F0 RID: 10224 RVA: 0x000A83C2 File Offset: 0x000A65C2
		public static List<ZombieSpawnpoint>[] zombies
		{
			get
			{
				return LevelZombies._zombies;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060027F1 RID: 10225 RVA: 0x000A83C9 File Offset: 0x000A65C9
		public static List<ZombieSpawnpoint>[,] spawns
		{
			get
			{
				return LevelZombies._spawns;
			}
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000A83D0 File Offset: 0x000A65D0
		internal static int GenerateTableUniqueId()
		{
			int result = LevelZombies.nextTableUniqueId;
			LevelZombies.nextTableUniqueId++;
			return result;
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000A83E4 File Offset: 0x000A65E4
		public static void setEnabled(bool isEnabled)
		{
			if (LevelZombies.spawns == null)
			{
				return;
			}
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					for (int i = 0; i < LevelZombies.spawns[(int)b, (int)b2].Count; i++)
					{
						LevelZombies.spawns[(int)b, (int)b2][i].setEnabled(isEnabled);
					}
				}
			}
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x000A844F File Offset: 0x000A664F
		public static void addTable(string name)
		{
			if (LevelZombies.tables.Count == 255)
			{
				return;
			}
			LevelZombies.tables.Add(new ZombieTable(name));
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x000A8474 File Offset: 0x000A6674
		public static void removeTable()
		{
			LevelZombies.tables.RemoveAt((int)EditorSpawns.selectedZombie);
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					List<ZombieSpawnpoint> list = new List<ZombieSpawnpoint>();
					for (int i = 0; i < LevelZombies.spawns[(int)b, (int)b2].Count; i++)
					{
						ZombieSpawnpoint zombieSpawnpoint = LevelZombies.spawns[(int)b, (int)b2][i];
						if (zombieSpawnpoint.type == EditorSpawns.selectedZombie)
						{
							Object.Destroy(zombieSpawnpoint.node.gameObject);
						}
						else
						{
							if (zombieSpawnpoint.type > EditorSpawns.selectedZombie)
							{
								ZombieSpawnpoint zombieSpawnpoint2 = zombieSpawnpoint;
								zombieSpawnpoint2.type -= 1;
							}
							list.Add(zombieSpawnpoint);
						}
					}
					LevelZombies._spawns[(int)b, (int)b2] = list;
				}
			}
			EditorSpawns.selectedZombie = 0;
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = LevelZombies.tables[(int)EditorSpawns.selectedZombie].color;
			}
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x000A8588 File Offset: 0x000A6788
		public static void addSpawn(Vector3 point)
		{
			byte b;
			byte b2;
			if (!Regions.tryGetCoordinate(point, out b, out b2))
			{
				return;
			}
			if ((int)EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
			{
				return;
			}
			LevelZombies.spawns[(int)b, (int)b2].Add(new ZombieSpawnpoint(EditorSpawns.selectedZombie, point));
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x000A85D0 File Offset: 0x000A67D0
		public static void removeSpawn(Vector3 point, float radius)
		{
			radius *= radius;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					List<ZombieSpawnpoint> list = new List<ZombieSpawnpoint>();
					for (int i = 0; i < LevelZombies.spawns[(int)b, (int)b2].Count; i++)
					{
						ZombieSpawnpoint zombieSpawnpoint = LevelZombies.spawns[(int)b, (int)b2][i];
						if ((zombieSpawnpoint.point - point).sqrMagnitude < radius)
						{
							Object.Destroy(zombieSpawnpoint.node.gameObject);
						}
						else
						{
							list.Add(zombieSpawnpoint);
						}
					}
					LevelZombies._spawns[(int)b, (int)b2] = list;
				}
			}
		}

		/// <returns>-1 if table was not found.</returns>
		// Token: 0x060027F8 RID: 10232 RVA: 0x000A8684 File Offset: 0x000A6884
		public static int FindTableIndexByUniqueId(int uniqueId)
		{
			if (LevelZombies.tables != null && uniqueId > 0)
			{
				for (int i = 0; i < LevelZombies.tables.Count; i++)
				{
					ZombieTable zombieTable = LevelZombies.tables[i];
					if (zombieTable != null && zombieTable.tableUniqueId == uniqueId)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000A86CC File Offset: 0x000A68CC
		public static void load()
		{
			LevelZombies.tables = new List<ZombieTable>();
			LevelZombies.nextTableUniqueId = 1;
			if (ReadWrite.fileExists(Level.info.path + "/Spawns/Zombies.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Spawns/Zombies.dat", false, false, 0);
				byte b = block.readByte();
				if (b > 3 && b < 5)
				{
					block.readSteamID();
				}
				if (b >= 10)
				{
					LevelZombies.nextTableUniqueId = block.readInt32();
				}
				if (b > 2)
				{
					byte b2 = block.readByte();
					for (byte b3 = 0; b3 < b2; b3 += 1)
					{
						int newTableUniqueId;
						if (b >= 10)
						{
							newTableUniqueId = block.readInt32();
						}
						else
						{
							newTableUniqueId = LevelZombies.GenerateTableUniqueId();
						}
						Color newColor = block.readColor();
						string newName = block.readString();
						bool flag = block.readBoolean();
						ushort newHealth = block.readUInt16();
						byte newDamage = block.readByte();
						byte newLootIndex = block.readByte();
						ushort newLootID;
						if (b > 6)
						{
							newLootID = block.readUInt16();
						}
						else
						{
							newLootID = 0;
						}
						uint newXP;
						if (b > 7)
						{
							newXP = block.readUInt32();
						}
						else if (flag)
						{
							newXP = 40U;
						}
						else
						{
							newXP = 3U;
						}
						float newRegen = 10f;
						if (b > 5)
						{
							newRegen = block.readSingle();
						}
						string newDifficultyGUID = string.Empty;
						if (b > 8)
						{
							newDifficultyGUID = block.readString();
						}
						ZombieSlot[] array = new ZombieSlot[4];
						byte b4 = block.readByte();
						for (byte b5 = 0; b5 < b4; b5 += 1)
						{
							List<ZombieCloth> list = new List<ZombieCloth>();
							float newChance = block.readSingle();
							byte b6 = block.readByte();
							for (byte b7 = 0; b7 < b6; b7 += 1)
							{
								ushort num = block.readUInt16();
								if (Assets.find(EAssetType.ITEM, num) is ItemAsset)
								{
									list.Add(new ZombieCloth(num));
								}
							}
							array[(int)b5] = new ZombieSlot(newChance, list);
						}
						LevelZombies.tables.Add(new ZombieTable(array, newColor, newName, flag, newHealth, newDamage, newLootIndex, newLootID, newXP, newRegen, newDifficultyGUID, newTableUniqueId));
					}
				}
				else
				{
					byte b8 = block.readByte();
					for (byte b9 = 0; b9 < b8; b9 += 1)
					{
						int newTableUniqueId2 = LevelZombies.GenerateTableUniqueId();
						Color newColor2 = block.readColor();
						string newName2 = block.readString();
						byte newLootIndex2 = block.readByte();
						ZombieSlot[] array2 = new ZombieSlot[4];
						byte b10 = block.readByte();
						for (byte b11 = 0; b11 < b10; b11 += 1)
						{
							List<ZombieCloth> list2 = new List<ZombieCloth>();
							float newChance2 = block.readSingle();
							byte b12 = block.readByte();
							for (byte b13 = 0; b13 < b12; b13 += 1)
							{
								ushort num2 = block.readUInt16();
								if (Assets.find(EAssetType.ITEM, num2) is ItemAsset)
								{
									list2.Add(new ZombieCloth(num2));
								}
							}
							array2[(int)b11] = new ZombieSlot(newChance2, list2);
						}
						LevelZombies.tables.Add(new ZombieTable(array2, newColor2, newName2, false, 100, 15, newLootIndex2, 0, 5U, 10f, string.Empty, newTableUniqueId2));
					}
				}
			}
			LevelZombies._spawns = new List<ZombieSpawnpoint>[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
			for (byte b14 = 0; b14 < Regions.WORLD_SIZE; b14 += 1)
			{
				for (byte b15 = 0; b15 < Regions.WORLD_SIZE; b15 += 1)
				{
					LevelZombies.spawns[(int)b14, (int)b15] = new List<ZombieSpawnpoint>();
				}
			}
			if (!Level.isEditor)
			{
				if (Provider.isServer)
				{
					LevelZombies._zombies = new List<ZombieSpawnpoint>[LevelNavigation.bounds.Count];
					for (int i = 0; i < LevelZombies.zombies.Length; i++)
					{
						LevelZombies.zombies[i] = new List<ZombieSpawnpoint>();
					}
					if (ReadWrite.fileExists(Level.info.path + "/Spawns/Animals.dat", false, false))
					{
						River river = new River(Level.info.path + "/Spawns/Animals.dat", false);
						if (river.readByte() > 0)
						{
							for (byte b16 = 0; b16 < Regions.WORLD_SIZE; b16 += 1)
							{
								for (byte b17 = 0; b17 < Regions.WORLD_SIZE; b17 += 1)
								{
									ushort num3 = river.readUInt16();
									for (ushort num4 = 0; num4 < num3; num4 += 1)
									{
										byte newType = river.readByte();
										Vector3 vector = river.readSingleVector3();
										byte b18;
										if (LevelNavigation.tryGetBounds(vector, out b18) && LevelNavigation.checkNavigation(vector))
										{
											LevelZombies.zombies[(int)b18].Add(new ZombieSpawnpoint(newType, vector));
										}
									}
								}
							}
						}
						river.closeRiver();
						return;
					}
					for (byte b19 = 0; b19 < Regions.WORLD_SIZE; b19 += 1)
					{
						for (byte b20 = 0; b20 < Regions.WORLD_SIZE; b20 += 1)
						{
							if (ReadWrite.fileExists(string.Concat(new string[]
							{
								Level.info.path,
								"/Spawns/Animals_",
								b19.ToString(),
								"_",
								b20.ToString(),
								".dat"
							}), false, false))
							{
								River river2 = new River(string.Concat(new string[]
								{
									Level.info.path,
									"/Spawns/Animals_",
									b19.ToString(),
									"_",
									b20.ToString(),
									".dat"
								}), false);
								if (river2.readByte() > 0)
								{
									ushort num5 = river2.readUInt16();
									for (ushort num6 = 0; num6 < num5; num6 += 1)
									{
										byte newType2 = river2.readByte();
										Vector3 vector2 = river2.readSingleVector3();
										byte b21;
										if (LevelNavigation.tryGetBounds(vector2, out b21) && LevelNavigation.checkNavigation(vector2))
										{
											LevelZombies.zombies[(int)b21].Add(new ZombieSpawnpoint(newType2, vector2));
										}
									}
									river2.closeRiver();
								}
							}
						}
					}
				}
				return;
			}
			if (ReadWrite.fileExists(Level.info.path + "/Spawns/Animals.dat", false, false))
			{
				River river3 = new River(Level.info.path + "/Spawns/Animals.dat", false);
				if (river3.readByte() > 0)
				{
					for (byte b22 = 0; b22 < Regions.WORLD_SIZE; b22 += 1)
					{
						for (byte b23 = 0; b23 < Regions.WORLD_SIZE; b23 += 1)
						{
							ushort num7 = river3.readUInt16();
							for (ushort num8 = 0; num8 < num7; num8 += 1)
							{
								byte newType3 = river3.readByte();
								Vector3 newPoint = river3.readSingleVector3();
								LevelZombies.spawns[(int)b22, (int)b23].Add(new ZombieSpawnpoint(newType3, newPoint));
							}
						}
					}
				}
				river3.closeRiver();
				return;
			}
			for (byte b24 = 0; b24 < Regions.WORLD_SIZE; b24 += 1)
			{
				for (byte b25 = 0; b25 < Regions.WORLD_SIZE; b25 += 1)
				{
					LevelZombies.spawns[(int)b24, (int)b25] = new List<ZombieSpawnpoint>();
					if (ReadWrite.fileExists(string.Concat(new string[]
					{
						Level.info.path,
						"/Spawns/Animals_",
						b24.ToString(),
						"_",
						b25.ToString(),
						".dat"
					}), false, false))
					{
						River river4 = new River(string.Concat(new string[]
						{
							Level.info.path,
							"/Spawns/Animals_",
							b24.ToString(),
							"_",
							b25.ToString(),
							".dat"
						}), false);
						if (river4.readByte() > 0)
						{
							ushort num9 = river4.readUInt16();
							for (ushort num10 = 0; num10 < num9; num10 += 1)
							{
								byte newType4 = river4.readByte();
								Vector3 newPoint2 = river4.readSingleVector3();
								LevelZombies.spawns[(int)b24, (int)b25].Add(new ZombieSpawnpoint(newType4, newPoint2));
							}
							river4.closeRiver();
						}
					}
				}
			}
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x000A8E40 File Offset: 0x000A7040
		public static void save()
		{
			Block block = new Block();
			block.writeByte(LevelZombies.SAVEDATA_TABLE_VERSION);
			block.writeInt32(LevelZombies.nextTableUniqueId);
			block.writeByte((byte)LevelZombies.tables.Count);
			byte b = 0;
			while ((int)b < LevelZombies.tables.Count)
			{
				ZombieTable zombieTable = LevelZombies.tables[(int)b];
				block.writeInt32(zombieTable.tableUniqueId);
				block.writeColor(zombieTable.color);
				block.writeString(zombieTable.name);
				block.writeBoolean(zombieTable.isMega);
				block.writeUInt16(zombieTable.health);
				block.writeByte(zombieTable.damage);
				block.writeByte(zombieTable.lootIndex);
				block.writeUInt16(zombieTable.lootID);
				block.writeUInt32(zombieTable.xp);
				block.writeSingle(zombieTable.regen);
				block.writeString(zombieTable.difficultyGUID);
				block.write((byte)zombieTable.slots.Length);
				byte b2 = 0;
				while ((int)b2 < zombieTable.slots.Length)
				{
					ZombieSlot zombieSlot = zombieTable.slots[(int)b2];
					block.writeSingle(zombieSlot.chance);
					block.writeByte((byte)zombieSlot.table.Count);
					byte b3 = 0;
					while ((int)b3 < zombieSlot.table.Count)
					{
						ZombieCloth zombieCloth = zombieSlot.table[(int)b3];
						block.writeUInt16(zombieCloth.item);
						b3 += 1;
					}
					b2 += 1;
				}
				b += 1;
			}
			ReadWrite.writeBlock(Level.info.path + "/Spawns/Zombies.dat", false, false, block);
			River river = new River(Level.info.path + "/Spawns/Animals.dat", false);
			river.writeByte(LevelZombies.SAVEDATA_SPAWN_VERSION);
			for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
			{
				for (byte b5 = 0; b5 < Regions.WORLD_SIZE; b5 += 1)
				{
					List<ZombieSpawnpoint> list = LevelZombies.spawns[(int)b4, (int)b5];
					river.writeUInt16((ushort)list.Count);
					ushort num = 0;
					while ((int)num < list.Count)
					{
						ZombieSpawnpoint zombieSpawnpoint = list[(int)num];
						river.writeByte(zombieSpawnpoint.type);
						river.writeSingleVector3(zombieSpawnpoint.point);
						num += 1;
					}
				}
			}
			river.closeRiver();
		}

		// Token: 0x0400151C RID: 5404
		public const byte SAVEDATA_TABLE_VERSION_OLDER = 9;

		// Token: 0x0400151D RID: 5405
		public const byte SAVEDATA_TABLE_VERSION_ADDED_UNIQUE_ID = 10;

		// Token: 0x0400151E RID: 5406
		private const byte SAVEDATA_TABLE_VERSION_NEWEST = 10;

		// Token: 0x0400151F RID: 5407
		public static readonly byte SAVEDATA_TABLE_VERSION = 10;

		// Token: 0x04001520 RID: 5408
		public static readonly byte SAVEDATA_SPAWN_VERSION = 1;

		// Token: 0x04001521 RID: 5409
		private static Transform _models;

		// Token: 0x04001522 RID: 5410
		public static List<ZombieTable> tables;

		// Token: 0x04001523 RID: 5411
		private static List<ZombieSpawnpoint>[] _zombies;

		// Token: 0x04001524 RID: 5412
		private static List<ZombieSpawnpoint>[,] _spawns;

		// Token: 0x04001525 RID: 5413
		private static int nextTableUniqueId;
	}
}
