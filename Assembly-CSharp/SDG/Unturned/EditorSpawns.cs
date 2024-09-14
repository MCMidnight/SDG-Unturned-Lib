using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000409 RID: 1033
	public class EditorSpawns : MonoBehaviour
	{
		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x000703FE File Offset: 0x0006E5FE
		// (set) Token: 0x06001E85 RID: 7813 RVA: 0x00070408 File Offset: 0x0006E608
		public static bool isSpawning
		{
			get
			{
				return EditorSpawns._isSpawning;
			}
			set
			{
				EditorSpawns._isSpawning = value;
				if (!EditorSpawns.isSpawning)
				{
					EditorSpawns.itemSpawn.gameObject.SetActive(false);
					EditorSpawns.playerSpawn.gameObject.SetActive(false);
					EditorSpawns.playerSpawnAlt.gameObject.SetActive(false);
					EditorSpawns.zombieSpawn.gameObject.SetActive(false);
					EditorSpawns.vehicleSpawn.gameObject.SetActive(false);
					EditorSpawns.animalSpawn.gameObject.SetActive(false);
					EditorSpawns.remove.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x00070492 File Offset: 0x0006E692
		// (set) Token: 0x06001E87 RID: 7815 RVA: 0x0007049C File Offset: 0x0006E69C
		public static bool selectedAlt
		{
			get
			{
				return EditorSpawns._selectedAlt;
			}
			set
			{
				EditorSpawns._selectedAlt = value;
				EditorSpawns.playerSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER && EditorSpawns.isSpawning && !EditorSpawns.selectedAlt);
				EditorSpawns.playerSpawnAlt.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER && EditorSpawns.isSpawning && EditorSpawns.selectedAlt);
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x000704FE File Offset: 0x0006E6FE
		public static Transform itemSpawn
		{
			get
			{
				return EditorSpawns._itemSpawn;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x00070505 File Offset: 0x0006E705
		public static Transform playerSpawn
		{
			get
			{
				return EditorSpawns._playerSpawn;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x0007050C File Offset: 0x0006E70C
		public static Transform playerSpawnAlt
		{
			get
			{
				return EditorSpawns._playerSpawnAlt;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x00070513 File Offset: 0x0006E713
		public static Transform zombieSpawn
		{
			get
			{
				return EditorSpawns._zombieSpawn;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001E8C RID: 7820 RVA: 0x0007051A File Offset: 0x0006E71A
		public static Transform vehicleSpawn
		{
			get
			{
				return EditorSpawns._vehicleSpawn;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x00070521 File Offset: 0x0006E721
		public static Transform animalSpawn
		{
			get
			{
				return EditorSpawns._animalSpawn;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001E8E RID: 7822 RVA: 0x00070528 File Offset: 0x0006E728
		public static Transform remove
		{
			get
			{
				return EditorSpawns._remove;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x0007052F File Offset: 0x0006E72F
		// (set) Token: 0x06001E90 RID: 7824 RVA: 0x00070538 File Offset: 0x0006E738
		public static float rotation
		{
			get
			{
				return EditorSpawns._rotation;
			}
			set
			{
				EditorSpawns._rotation = value;
				if (EditorSpawns.playerSpawn != null)
				{
					EditorSpawns.playerSpawn.transform.rotation = Quaternion.Euler(0f, EditorSpawns.rotation, 0f);
				}
				if (EditorSpawns.playerSpawnAlt != null)
				{
					EditorSpawns.playerSpawnAlt.transform.rotation = Quaternion.Euler(0f, EditorSpawns.rotation, 0f);
				}
				if (EditorSpawns.vehicleSpawn != null)
				{
					EditorSpawns.vehicleSpawn.transform.rotation = Quaternion.Euler(0f, EditorSpawns.rotation, 0f);
				}
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x000705DB File Offset: 0x0006E7DB
		// (set) Token: 0x06001E92 RID: 7826 RVA: 0x000705E2 File Offset: 0x0006E7E2
		public static byte radius
		{
			get
			{
				return EditorSpawns._radius;
			}
			set
			{
				EditorSpawns._radius = value;
				if (EditorSpawns.remove != null)
				{
					EditorSpawns.remove.localScale = new Vector3((float)(EditorSpawns.radius * 2), (float)(EditorSpawns.radius * 2), (float)(EditorSpawns.radius * 2));
				}
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x0007061E File Offset: 0x0006E81E
		// (set) Token: 0x06001E94 RID: 7828 RVA: 0x00070628 File Offset: 0x0006E828
		public static ESpawnMode spawnMode
		{
			get
			{
				return EditorSpawns._spawnMode;
			}
			set
			{
				EditorSpawns._spawnMode = value;
				EditorSpawns.itemSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM && EditorSpawns.isSpawning);
				EditorSpawns.playerSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER && EditorSpawns.isSpawning && !EditorSpawns.selectedAlt);
				EditorSpawns.playerSpawnAlt.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER && EditorSpawns.isSpawning && EditorSpawns.selectedAlt);
				EditorSpawns.zombieSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE && EditorSpawns.isSpawning);
				EditorSpawns.vehicleSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE && EditorSpawns.isSpawning);
				EditorSpawns.animalSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL && EditorSpawns.isSpawning);
				EditorSpawns.remove.gameObject.SetActive((EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM || EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ANIMAL) && EditorSpawns.isSpawning);
			}
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x00070748 File Offset: 0x0006E948
		private void Update()
		{
			if (!EditorSpawns.isSpawning)
			{
				return;
			}
			if (!EditorInteract.isFlying && Glazier.Get().ShouldGameProcessInput)
			{
				if (InputEx.GetKeyDown(ControlsSettings.tool_0))
				{
					if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM)
					{
						EditorSpawns.spawnMode = ESpawnMode.ADD_ITEM;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER)
					{
						EditorSpawns.spawnMode = ESpawnMode.ADD_PLAYER;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE)
					{
						EditorSpawns.spawnMode = ESpawnMode.ADD_ZOMBIE;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE)
					{
						EditorSpawns.spawnMode = ESpawnMode.ADD_VEHICLE;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ANIMAL)
					{
						EditorSpawns.spawnMode = ESpawnMode.ADD_ANIMAL;
					}
				}
				if (InputEx.GetKeyDown(ControlsSettings.tool_1))
				{
					if (EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM)
					{
						EditorSpawns.spawnMode = ESpawnMode.REMOVE_ITEM;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER)
					{
						EditorSpawns.spawnMode = ESpawnMode.REMOVE_PLAYER;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE)
					{
						EditorSpawns.spawnMode = ESpawnMode.REMOVE_ZOMBIE;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE)
					{
						EditorSpawns.spawnMode = ESpawnMode.REMOVE_VEHICLE;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL)
					{
						EditorSpawns.spawnMode = ESpawnMode.REMOVE_ANIMAL;
					}
				}
				if (EditorInteract.worldHit.transform != null)
				{
					if (EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM)
					{
						EditorSpawns.itemSpawn.position = EditorInteract.worldHit.point;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER)
					{
						EditorSpawns.playerSpawn.position = EditorInteract.worldHit.point;
						EditorSpawns.playerSpawnAlt.position = EditorInteract.worldHit.point;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE)
					{
						EditorSpawns.zombieSpawn.position = EditorInteract.worldHit.point + Vector3.up;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE)
					{
						EditorSpawns.vehicleSpawn.position = EditorInteract.worldHit.point;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL)
					{
						EditorSpawns.animalSpawn.position = EditorInteract.worldHit.point;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM || EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ANIMAL)
					{
						EditorSpawns.remove.position = EditorInteract.worldHit.point;
					}
				}
				if (InputEx.GetKeyDown(ControlsSettings.primary) && EditorInteract.worldHit.transform != null)
				{
					Vector3 point = EditorInteract.worldHit.point;
					if (EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM)
					{
						if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
						{
							LevelItems.addSpawn(point);
							return;
						}
					}
					else
					{
						if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM)
						{
							LevelItems.removeSpawn(point, (float)EditorSpawns.radius);
							return;
						}
						if (EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER)
						{
							LevelPlayers.addSpawn(point, EditorSpawns.rotation, EditorSpawns.selectedAlt);
							return;
						}
						if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER)
						{
							LevelPlayers.removeSpawn(point, (float)EditorSpawns.radius);
							return;
						}
						if (EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE)
						{
							if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
							{
								LevelZombies.addSpawn(point);
								return;
							}
						}
						else
						{
							if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE)
							{
								LevelZombies.removeSpawn(point, (float)EditorSpawns.radius);
								return;
							}
							if (EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE)
							{
								LevelVehicles.addSpawn(point, EditorSpawns.rotation);
								return;
							}
							if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE)
							{
								LevelVehicles.removeSpawn(point, (float)EditorSpawns.radius);
								return;
							}
							if (EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL)
							{
								LevelAnimals.addSpawn(point);
								return;
							}
							if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ANIMAL)
							{
								LevelAnimals.removeSpawn(point, (float)EditorSpawns.radius);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x00070A78 File Offset: 0x0006EC78
		private void Start()
		{
			EditorSpawns._isSpawning = false;
			EditorSpawns._itemSpawn = ((GameObject)Object.Instantiate(Resources.Load("Edit/Item"))).transform;
			EditorSpawns.itemSpawn.name = "Item Spawn";
			EditorSpawns.itemSpawn.parent = Level.editing;
			EditorSpawns.itemSpawn.gameObject.SetActive(false);
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				EditorSpawns.itemSpawn.GetComponent<Renderer>().material.color = LevelItems.tables[(int)EditorSpawns.selectedItem].color;
			}
			EditorSpawns._playerSpawn = ((GameObject)Object.Instantiate(Resources.Load("Edit/Player"))).transform;
			EditorSpawns.playerSpawn.name = "Player Spawn";
			EditorSpawns.playerSpawn.parent = Level.editing;
			EditorSpawns.playerSpawn.gameObject.SetActive(false);
			EditorSpawns._playerSpawnAlt = ((GameObject)Object.Instantiate(Resources.Load("Edit/Player_Alt"))).transform;
			EditorSpawns.playerSpawnAlt.name = "Player Spawn Alt";
			EditorSpawns.playerSpawnAlt.parent = Level.editing;
			EditorSpawns.playerSpawnAlt.gameObject.SetActive(false);
			EditorSpawns._zombieSpawn = ((GameObject)Object.Instantiate(Resources.Load("Edit/Zombie"))).transform;
			EditorSpawns.zombieSpawn.name = "Zombie Spawn";
			EditorSpawns.zombieSpawn.parent = Level.editing;
			EditorSpawns.zombieSpawn.gameObject.SetActive(false);
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = LevelZombies.tables[(int)EditorSpawns.selectedZombie].color;
			}
			EditorSpawns._vehicleSpawn = ((GameObject)Object.Instantiate(Resources.Load("Edit/Vehicle"))).transform;
			EditorSpawns.vehicleSpawn.name = "Vehicle Spawn";
			EditorSpawns.vehicleSpawn.parent = Level.editing;
			EditorSpawns.vehicleSpawn.gameObject.SetActive(false);
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color;
				EditorSpawns.vehicleSpawn.Find("Arrow").GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color;
			}
			EditorSpawns._animalSpawn = ((GameObject)Object.Instantiate(Resources.Load("Edit/Animal"))).transform;
			EditorSpawns._animalSpawn.name = "Animal Spawn";
			EditorSpawns._animalSpawn.parent = Level.editing;
			EditorSpawns._animalSpawn.gameObject.SetActive(false);
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
			{
				EditorSpawns.animalSpawn.GetComponent<Renderer>().material.color = LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].color;
			}
			EditorSpawns._remove = ((GameObject)Object.Instantiate(Resources.Load("Edit/Remove"))).transform;
			EditorSpawns.remove.name = "Remove";
			EditorSpawns.remove.parent = Level.editing;
			EditorSpawns.remove.gameObject.SetActive(false);
			EditorSpawns.spawnMode = ESpawnMode.ADD_ITEM;
			EditorSpawns.load();
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x00070DC0 File Offset: 0x0006EFC0
		public static void load()
		{
			if (ReadWrite.fileExists(Level.info.path + "/Editor/Spawns.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Spawns.dat", false, false, 1);
				EditorSpawns.rotation = block.readSingle();
				EditorSpawns.radius = block.readByte();
				return;
			}
			EditorSpawns.rotation = 0f;
			EditorSpawns.radius = EditorSpawns.MIN_REMOVE_SIZE;
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x00070E30 File Offset: 0x0006F030
		public static void save()
		{
			Block block = new Block();
			block.writeByte(EditorSpawns.SAVEDATA_VERSION);
			block.writeSingle(EditorSpawns.rotation);
			block.writeByte(EditorSpawns.radius);
			ReadWrite.writeBlock(Level.info.path + "/Editor/Spawns.dat", false, false, block);
		}

		// Token: 0x04000EB9 RID: 3769
		public static readonly byte SAVEDATA_VERSION = 1;

		// Token: 0x04000EBA RID: 3770
		public static readonly byte MIN_REMOVE_SIZE = 2;

		// Token: 0x04000EBB RID: 3771
		public static readonly byte MAX_REMOVE_SIZE = 30;

		// Token: 0x04000EBC RID: 3772
		private static bool _isSpawning;

		// Token: 0x04000EBD RID: 3773
		public static byte selectedItem;

		// Token: 0x04000EBE RID: 3774
		public static byte selectedZombie;

		// Token: 0x04000EBF RID: 3775
		public static byte selectedVehicle;

		// Token: 0x04000EC0 RID: 3776
		public static byte selectedAnimal;

		// Token: 0x04000EC1 RID: 3777
		private static bool _selectedAlt;

		// Token: 0x04000EC2 RID: 3778
		private static Transform _itemSpawn;

		// Token: 0x04000EC3 RID: 3779
		private static Transform _playerSpawn;

		// Token: 0x04000EC4 RID: 3780
		private static Transform _playerSpawnAlt;

		// Token: 0x04000EC5 RID: 3781
		private static Transform _zombieSpawn;

		// Token: 0x04000EC6 RID: 3782
		private static Transform _vehicleSpawn;

		// Token: 0x04000EC7 RID: 3783
		private static Transform _animalSpawn;

		// Token: 0x04000EC8 RID: 3784
		private static Transform _remove;

		// Token: 0x04000EC9 RID: 3785
		private static float _rotation;

		// Token: 0x04000ECA RID: 3786
		private static byte _radius;

		// Token: 0x04000ECB RID: 3787
		private static ESpawnMode _spawnMode;
	}
}
