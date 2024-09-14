using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004DA RID: 1242
	public class LevelAnimals
	{
		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06002614 RID: 9748 RVA: 0x000987A0 File Offset: 0x000969A0
		[Obsolete("Was the parent of all animals in the past, but now empty for TransformHierarchy performance.")]
		public static Transform models
		{
			get
			{
				if (LevelAnimals._models == null)
				{
					LevelAnimals._models = new GameObject().transform;
					LevelAnimals._models.name = "Animals";
					LevelAnimals._models.parent = Level.spawns;
					LevelAnimals._models.tag = "Logic";
					LevelAnimals._models.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing LevelAnimals.models which has been deprecated.", Array.Empty<object>());
				}
				return LevelAnimals._models;
			}
		}

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06002615 RID: 9749 RVA: 0x0009881A File Offset: 0x00096A1A
		public static List<AnimalTable> tables
		{
			get
			{
				return LevelAnimals._tables;
			}
		}

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06002616 RID: 9750 RVA: 0x00098821 File Offset: 0x00096A21
		public static List<AnimalSpawnpoint> spawns
		{
			get
			{
				return LevelAnimals._spawns;
			}
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x00098828 File Offset: 0x00096A28
		public static void setEnabled(bool isEnabled)
		{
			if (LevelAnimals.spawns == null)
			{
				return;
			}
			for (int i = 0; i < LevelAnimals.spawns.Count; i++)
			{
				LevelAnimals.spawns[i].setEnabled(isEnabled);
			}
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x00098863 File Offset: 0x00096A63
		public static void addTable(string name)
		{
			if (LevelAnimals.tables.Count == 255)
			{
				return;
			}
			LevelAnimals.tables.Add(new AnimalTable(name));
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x00098888 File Offset: 0x00096A88
		public static void removeTable()
		{
			LevelAnimals.tables.RemoveAt((int)EditorSpawns.selectedAnimal);
			List<AnimalSpawnpoint> list = new List<AnimalSpawnpoint>();
			for (int i = 0; i < LevelAnimals.spawns.Count; i++)
			{
				AnimalSpawnpoint animalSpawnpoint = LevelAnimals.spawns[i];
				if (animalSpawnpoint.type == EditorSpawns.selectedAnimal)
				{
					Object.Destroy(animalSpawnpoint.node.gameObject);
				}
				else
				{
					if (animalSpawnpoint.type > EditorSpawns.selectedAnimal)
					{
						AnimalSpawnpoint animalSpawnpoint2 = animalSpawnpoint;
						animalSpawnpoint2.type -= 1;
					}
					list.Add(animalSpawnpoint);
				}
			}
			LevelAnimals._spawns = list;
			EditorSpawns.selectedAnimal = 0;
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
			{
				EditorSpawns.animalSpawn.GetComponent<Renderer>().material.color = LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].color;
			}
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x00098952 File Offset: 0x00096B52
		public static void addSpawn(Vector3 point)
		{
			if ((int)EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count)
			{
				return;
			}
			LevelAnimals.spawns.Add(new AnimalSpawnpoint(EditorSpawns.selectedAnimal, point));
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x0009897C File Offset: 0x00096B7C
		public static void removeSpawn(Vector3 point, float radius)
		{
			radius *= radius;
			List<AnimalSpawnpoint> list = new List<AnimalSpawnpoint>();
			for (int i = 0; i < LevelAnimals.spawns.Count; i++)
			{
				AnimalSpawnpoint animalSpawnpoint = LevelAnimals.spawns[i];
				if ((animalSpawnpoint.point - point).sqrMagnitude < radius)
				{
					Object.Destroy(animalSpawnpoint.node.gameObject);
				}
				else
				{
					list.Add(animalSpawnpoint);
				}
			}
			LevelAnimals._spawns = list;
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x000989EB File Offset: 0x00096BEB
		public static ushort getAnimal(AnimalSpawnpoint spawn)
		{
			return LevelAnimals.getAnimal(spawn.type);
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000989F8 File Offset: 0x00096BF8
		public static ushort getAnimal(byte type)
		{
			return LevelAnimals.tables[(int)type].getAnimal();
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x00098A0C File Offset: 0x00096C0C
		public static void load()
		{
			if (Level.isEditor || Provider.isServer)
			{
				LevelAnimals._tables = new List<AnimalTable>();
				LevelAnimals._spawns = new List<AnimalSpawnpoint>();
				if (ReadWrite.fileExists(Level.info.path + "/Spawns/Fauna.dat", false, false))
				{
					River river = new River(Level.info.path + "/Spawns/Fauna.dat", false);
					byte b = river.readByte();
					byte b2 = river.readByte();
					for (byte b3 = 0; b3 < b2; b3 += 1)
					{
						Color newColor = river.readColor();
						string text = river.readString();
						ushort num;
						if (b > 2)
						{
							num = river.readUInt16();
						}
						else
						{
							num = 0;
						}
						List<AnimalTier> list = new List<AnimalTier>();
						byte b4 = river.readByte();
						for (byte b5 = 0; b5 < b4; b5 += 1)
						{
							string newName = river.readString();
							float newChance = river.readSingle();
							List<AnimalSpawn> list2 = new List<AnimalSpawn>();
							byte b6 = river.readByte();
							for (byte b7 = 0; b7 < b6; b7 += 1)
							{
								ushort newAnimal = river.readUInt16();
								list2.Add(new AnimalSpawn(newAnimal));
							}
							list.Add(new AnimalTier(list2, newName, newChance));
						}
						AnimalTable animalTable = new AnimalTable(list, newColor, text, num);
						LevelAnimals.tables.Add(animalTable);
						if (!Level.isEditor)
						{
							animalTable.buildTable();
						}
						if (animalTable.tableID != 0 && SpawnTableTool.ResolveLegacyId(num, EAssetType.ANIMAL, new Func<string>(animalTable.OnGetSpawnTableValidationErrorContext)) == 0 && Assets.shouldLoadAnyAssets)
						{
							Assets.reportError(string.Concat(new string[]
							{
								Level.info.name,
								" animal table \"",
								text,
								"\" references invalid spawn table ",
								num.ToString(),
								"!"
							}));
						}
					}
					ushort num2 = river.readUInt16();
					for (int i = 0; i < (int)num2; i++)
					{
						byte newType = river.readByte();
						Vector3 newPoint = river.readSingleVector3();
						LevelAnimals.spawns.Add(new AnimalSpawnpoint(newType, newPoint));
					}
					river.closeRiver();
				}
			}
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x00098C14 File Offset: 0x00096E14
		public static void save()
		{
			River river = new River(Level.info.path + "/Spawns/Fauna.dat", false);
			river.writeByte(LevelAnimals.SAVEDATA_VERSION);
			river.writeByte((byte)LevelAnimals.tables.Count);
			byte b = 0;
			while ((int)b < LevelAnimals.tables.Count)
			{
				AnimalTable animalTable = LevelAnimals.tables[(int)b];
				river.writeColor(animalTable.color);
				river.writeString(animalTable.name);
				river.writeUInt16(animalTable.tableID);
				river.writeByte((byte)animalTable.tiers.Count);
				byte b2 = 0;
				while ((int)b2 < animalTable.tiers.Count)
				{
					AnimalTier animalTier = animalTable.tiers[(int)b2];
					river.writeString(animalTier.name);
					river.writeSingle(animalTier.chance);
					river.writeByte((byte)animalTier.table.Count);
					byte b3 = 0;
					while ((int)b3 < animalTier.table.Count)
					{
						AnimalSpawn animalSpawn = animalTier.table[(int)b3];
						river.writeUInt16(animalSpawn.animal);
						b3 += 1;
					}
					b2 += 1;
				}
				b += 1;
			}
			river.writeUInt16((ushort)LevelAnimals.spawns.Count);
			for (int i = 0; i < LevelAnimals.spawns.Count; i++)
			{
				AnimalSpawnpoint animalSpawnpoint = LevelAnimals.spawns[i];
				river.writeByte(animalSpawnpoint.type);
				river.writeSingleVector3(animalSpawnpoint.point);
			}
			river.closeRiver();
		}

		// Token: 0x040013A8 RID: 5032
		public static readonly byte SAVEDATA_VERSION = 3;

		// Token: 0x040013A9 RID: 5033
		private static Transform _models;

		// Token: 0x040013AA RID: 5034
		private static List<AnimalTable> _tables;

		// Token: 0x040013AB RID: 5035
		private static List<AnimalSpawnpoint> _spawns;
	}
}
