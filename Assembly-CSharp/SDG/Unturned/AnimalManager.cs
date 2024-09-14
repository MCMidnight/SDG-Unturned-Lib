using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000530 RID: 1328
	public class AnimalManager : SteamCaller
	{
		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002987 RID: 10631 RVA: 0x000B0E4E File Offset: 0x000AF04E
		public static List<Animal> animals
		{
			get
			{
				return AnimalManager._animals;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002988 RID: 10632 RVA: 0x000B0E55 File Offset: 0x000AF055
		public static List<PackInfo> packs
		{
			get
			{
				return AnimalManager._packs;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002989 RID: 10633 RVA: 0x000B0E5C File Offset: 0x000AF05C
		public static List<Animal> tickingAnimals
		{
			get
			{
				return AnimalManager._tickingAnimals;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x0600298A RID: 10634 RVA: 0x000B0E64 File Offset: 0x000AF064
		public static uint maxInstances
		{
			get
			{
				switch (Level.info.size)
				{
				case ELevelSize.TINY:
					return Provider.modeConfigData.Animals.Max_Instances_Tiny;
				case ELevelSize.SMALL:
					return Provider.modeConfigData.Animals.Max_Instances_Small;
				case ELevelSize.MEDIUM:
					return Provider.modeConfigData.Animals.Max_Instances_Medium;
				case ELevelSize.LARGE:
					return Provider.modeConfigData.Animals.Max_Instances_Large;
				case ELevelSize.INSANE:
					return Provider.modeConfigData.Animals.Max_Instances_Insane;
				default:
					return 0U;
				}
			}
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x000B0EEC File Offset: 0x000AF0EC
		public static bool giveAnimal(Player player, ushort id)
		{
			if (Assets.find(EAssetType.ANIMAL, id) is AnimalAsset)
			{
				Vector3 vector = player.transform.position + player.transform.forward * 6f;
				RaycastHit raycastHit;
				Physics.Raycast(vector + Vector3.up * 16f, Vector3.down, out raycastHit, 32f, RayMasks.BLOCK_VEHICLE);
				if (raycastHit.collider != null)
				{
					vector = raycastHit.point;
				}
				AnimalManager.spawnAnimal(id, vector, player.transform.rotation);
				return true;
			}
			return false;
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000B0F88 File Offset: 0x000AF188
		public static void spawnAnimal(ushort id, Vector3 point, Quaternion angle)
		{
			foreach (Animal animal2 in AnimalManager.animals)
			{
				if (animal2.id == id && animal2.isDead)
				{
					animal2.sendRevive(point, Random.Range(0f, 360f));
					return;
				}
			}
			if (Assets.find(EAssetType.ANIMAL, id) is AnimalAsset)
			{
				Animal animal = AnimalManager.manager.addAnimal(id, point, angle.eulerAngles.y, false);
				AnimalSpawnpoint animalSpawnpoint = new AnimalSpawnpoint(0, point);
				PackInfo packInfo = new PackInfo();
				animal.pack = packInfo;
				packInfo.animals.Add(animal);
				packInfo.spawns.Add(animalSpawnpoint);
				AnimalManager.packs.Add(packInfo);
				AnimalManager.SendSingleAnimal.Invoke(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), delegate(NetPakWriter writer)
				{
					AnimalManager.WriteSingleAnimal(animal, writer);
				});
			}
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000B1098 File Offset: 0x000AF298
		public static void getAnimalsInRadius(Vector3 center, float sqrRadius, List<Animal> result)
		{
			if (AnimalManager.animals == null)
			{
				return;
			}
			for (int i = 0; i < AnimalManager.animals.Count; i++)
			{
				Animal animal = AnimalManager.animals[i];
				if ((animal.transform.position - center).sqrMagnitude < sqrRadius)
				{
					result.Add(animal);
				}
			}
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x000B10F1 File Offset: 0x000AF2F1
		[Obsolete]
		public void tellAnimalAlive(CSteamID steamID, ushort index, Vector3 newPosition, byte newAngle)
		{
			AnimalManager.ReceiveAnimalAlive(index, newPosition, newAngle);
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x000B10FC File Offset: 0x000AF2FC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellAnimalAlive")]
		public static void ReceiveAnimalAlive(ushort index, Vector3 newPosition, byte newAngle)
		{
			if ((int)index >= AnimalManager.animals.Count)
			{
				return;
			}
			AnimalManager.animals[(int)index].tellAlive(newPosition, newAngle);
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000B111E File Offset: 0x000AF31E
		[Obsolete]
		public void tellAnimalDead(CSteamID steamID, ushort index, Vector3 newRagdoll, byte newRagdollEffect)
		{
			AnimalManager.ReceiveAnimalDead(index, newRagdoll, (ERagdollEffect)newRagdollEffect);
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x000B1129 File Offset: 0x000AF329
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellAnimalDead")]
		public static void ReceiveAnimalDead(ushort index, Vector3 newRagdoll, ERagdollEffect newRagdollEffect)
		{
			if ((int)index >= AnimalManager.animals.Count)
			{
				return;
			}
			AnimalManager.animals[(int)index].tellDead(newRagdoll, newRagdollEffect);
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x000B114B File Offset: 0x000AF34B
		[Obsolete]
		public void tellAnimalStates(CSteamID steamID)
		{
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000B1150 File Offset: 0x000AF350
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveAnimalStates(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint num;
			SystemNetPakReaderEx.ReadUInt32(reader, ref num);
			if (num <= AnimalManager.seq)
			{
				return;
			}
			AnimalManager.seq = num;
			ushort num2;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num2);
			if (num2 < 1)
			{
				return;
			}
			for (ushort num3 = 0; num3 < num2; num3 += 1)
			{
				ushort num4;
				SystemNetPakReaderEx.ReadUInt16(reader, ref num4);
				Vector3 newPosition;
				UnityNetPakReaderEx.ReadClampedVector3(reader, ref newPosition, 13, 7);
				float newAngle;
				SystemNetPakReaderEx.ReadDegrees(reader, ref newAngle, 8);
				if ((int)num4 < AnimalManager.animals.Count)
				{
					AnimalManager.animals[(int)num4].tellState(newPosition, newAngle);
				}
			}
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000B11D9 File Offset: 0x000AF3D9
		[Obsolete]
		public void askAnimalStartle(CSteamID steamID, ushort index)
		{
			AnimalManager.ReceiveAnimalStartle(index);
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000B11E1 File Offset: 0x000AF3E1
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askAnimalStartle")]
		public static void ReceiveAnimalStartle(ushort index)
		{
			if ((int)index >= AnimalManager.animals.Count)
			{
				return;
			}
			AnimalManager.animals[(int)index].PlayStartleAnimation();
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x000B1201 File Offset: 0x000AF401
		[Obsolete]
		public void askAnimalAttack(CSteamID steamID, ushort index)
		{
			AnimalManager.ReceiveAnimalAttack(index);
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x000B1209 File Offset: 0x000AF409
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askAnimalAttack")]
		public static void ReceiveAnimalAttack(ushort index)
		{
			if ((int)index >= AnimalManager.animals.Count)
			{
				return;
			}
			AnimalManager.animals[(int)index].askAttack();
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x000B1229 File Offset: 0x000AF429
		[Obsolete]
		public void askAnimalPanic(CSteamID steamID, ushort index)
		{
			AnimalManager.ReceiveAnimalPanic(index);
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x000B1231 File Offset: 0x000AF431
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askAnimalPanic")]
		public static void ReceiveAnimalPanic(ushort index)
		{
			if ((int)index >= AnimalManager.animals.Count)
			{
				return;
			}
			AnimalManager.animals[(int)index].askPanic();
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x000B1251 File Offset: 0x000AF451
		[Obsolete]
		public void tellAnimals(CSteamID steamID)
		{
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x000B1254 File Offset: 0x000AF454
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveMultipleAnimals(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num);
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				AnimalManager.ReadSingleAnimal(reader);
			}
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x000B1284 File Offset: 0x000AF484
		private static void ReadSingleAnimal(NetPakReader reader)
		{
			ushort id;
			SystemNetPakReaderEx.ReadUInt16(reader, ref id);
			Vector3 point;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref point, 13, 7);
			float angle;
			SystemNetPakReaderEx.ReadDegrees(reader, ref angle, 8);
			bool isDead;
			reader.ReadBit(ref isDead);
			AnimalManager.manager.addAnimal(id, point, angle, isDead);
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x000B12C8 File Offset: 0x000AF4C8
		[Obsolete]
		public void tellAnimal(CSteamID steamID)
		{
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000B12CA File Offset: 0x000AF4CA
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveSingleAnimal(in ClientInvocationContext context)
		{
			AnimalManager.ReadSingleAnimal(context.reader);
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x000B12D7 File Offset: 0x000AF4D7
		[Obsolete]
		public void askAnimals(CSteamID steamID)
		{
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x000B12D9 File Offset: 0x000AF4D9
		internal static void SendInitialGlobalState(ITransportConnection transportConnection)
		{
			AnimalManager.SendMultipleAnimals.Invoke(ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt16(writer, (ushort)AnimalManager.animals.Count);
				ushort num = 0;
				while ((int)num < AnimalManager.animals.Count)
				{
					AnimalManager.WriteSingleAnimal(AnimalManager.animals[(int)num], writer);
					num += 1;
				}
			});
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000B1306 File Offset: 0x000AF506
		[Obsolete]
		public void sendAnimal(Animal animal, NetPakWriter writer)
		{
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x000B1308 File Offset: 0x000AF508
		private static void WriteSingleAnimal(Animal animal, NetPakWriter writer)
		{
			SystemNetPakWriterEx.WriteUInt16(writer, animal.id);
			UnityNetPakWriterEx.WriteClampedVector3(writer, animal.transform.position, 13, 7);
			SystemNetPakWriterEx.WriteDegrees(writer, animal.transform.eulerAngles.y, 8);
			writer.WriteBit(animal.isDead);
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x000B135C File Offset: 0x000AF55C
		public static void sendAnimalAlive(Animal animal, Vector3 newPosition, byte newAngle)
		{
			AnimalManager.SendAnimalAlive.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), animal.index, newPosition, newAngle);
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x000B1376 File Offset: 0x000AF576
		public static void sendAnimalDead(Animal animal, Vector3 newRagdoll, ERagdollEffect newRagdollEffect = ERagdollEffect.NONE)
		{
			AnimalManager.SendAnimalDead.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), animal.index, newRagdoll, newRagdollEffect);
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x000B1390 File Offset: 0x000AF590
		public static void sendAnimalStartle(Animal animal)
		{
			AnimalManager.SendAnimalStartle.InvokeAndLoopback(ENetReliability.Unreliable, Provider.GatherRemoteClientConnections(), animal.index);
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x000B13A8 File Offset: 0x000AF5A8
		public static void sendAnimalAttack(Animal animal)
		{
			AnimalManager.SendAnimalAttack.InvokeAndLoopback(ENetReliability.Unreliable, Provider.GatherRemoteClientConnections(), animal.index);
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x000B13C0 File Offset: 0x000AF5C0
		public static void sendAnimalPanic(Animal animal)
		{
			AnimalManager.SendAnimalPanic.InvokeAndLoopback(ENetReliability.Unreliable, Provider.GatherRemoteClientConnections(), animal.index);
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x000B13D8 File Offset: 0x000AF5D8
		public static void dropLoot(Animal animal)
		{
			if (animal == null || animal.asset == null || animal.transform == null)
			{
				return;
			}
			if (animal.asset.rewardID != 0)
			{
				int num = Random.Range((int)animal.asset.rewardMin, (int)(animal.asset.rewardMax + 1));
				num = Mathf.Clamp(num, 0, 100);
				for (int i = 0; i < num; i++)
				{
					ushort num2 = SpawnTableTool.ResolveLegacyId(animal.asset.rewardID, EAssetType.ITEM, new Func<string>(animal.asset.OnGetRewardSpawnTableErrorContext));
					if (num2 != 0)
					{
						ItemManager.dropItem(new Item(num2, EItemOrigin.NATURE), animal.transform.position, false, true, true);
					}
				}
				return;
			}
			if (animal.asset.meat != 0)
			{
				int num3 = Random.Range(2, 5);
				for (int j = 0; j < num3; j++)
				{
					ItemManager.dropItem(new Item(animal.asset.meat, EItemOrigin.NATURE), animal.transform.position, false, true, true);
				}
			}
			if (animal.asset.pelt != 0)
			{
				int num4 = Random.Range(2, 5);
				for (int k = 0; k < num4; k++)
				{
					ItemManager.dropItem(new Item(animal.asset.pelt, EItemOrigin.NATURE), animal.transform.position, false, true, true);
				}
			}
		}

		/// <summary>
		/// Spawns an animal into the world.
		/// </summary>
		/// <param name="id">The ID of the animal.</param>
		/// <param name="point">Position to spawn the animal.</param>
		/// <param name="angle">Angle to spawn the animal.</param>
		/// <param name="isDead">Whether the animal is dead or not.</param>
		// Token: 0x060029A9 RID: 10665 RVA: 0x000B151C File Offset: 0x000AF71C
		private Animal addAnimal(ushort id, Vector3 point, float angle, bool isDead)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			AnimalAsset animalAsset = Assets.find(EAssetType.ANIMAL, id) as AnimalAsset;
			if (animalAsset != null)
			{
				GameObject dedicated = animalAsset.dedicated;
				Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
				GameObject gameObject = Object.Instantiate<GameObject>(dedicated, point, rotation);
				gameObject.name = id.ToString();
				Animal animal = gameObject.AddComponent<Animal>();
				animal.index = (ushort)AnimalManager.animals.Count;
				animal.id = id;
				animal.isDead = isDead;
				animal.init();
				AnimalManager.animals.Add(animal);
				return animal;
			}
			return null;
		}

		/// <summary>
		/// Gets the animal at a specific index.
		/// </summary>
		/// <param name="index">The index of the animal.</param>
		/// <returns></returns>
		// Token: 0x060029AA RID: 10666 RVA: 0x000B15A5 File Offset: 0x000AF7A5
		public static Animal getAnimal(ushort index)
		{
			if ((int)index >= AnimalManager.animals.Count)
			{
				return null;
			}
			return AnimalManager.animals[(int)index];
		}

		/// <summary>
		/// Find replacement spawnpoint for an animal and teleport it there.
		/// </summary>
		// Token: 0x060029AB RID: 10667 RVA: 0x000B15C4 File Offset: 0x000AF7C4
		public static void TeleportAnimalBackIntoMap(Animal animal)
		{
			Vector3? vector = default(Vector3?);
			if (animal.pack != null)
			{
				if (animal.pack.animals != null)
				{
					foreach (Animal animal2 in animal.pack.animals)
					{
						if (!(animal == animal2) && !animal2.isDead)
						{
							Vector3 position = animal2.transform.position;
							if (UndergroundAllowlist.IsPositionWithinValidHeight(position, 0.1f))
							{
								vector = new Vector3?(position);
								break;
							}
						}
					}
				}
				if (vector == null && animal.pack.spawns != null && animal.pack.spawns.Count > 0)
				{
					vector = new Vector3?(animal.pack.spawns[animal.pack.spawns.GetRandomIndex<AnimalSpawnpoint>()].point);
				}
			}
			if (vector == null)
			{
				if (LevelAnimals.spawns != null && LevelAnimals.spawns.Count > 0)
				{
					vector = new Vector3?(LevelAnimals.spawns[LevelAnimals.spawns.GetRandomIndex<AnimalSpawnpoint>()].point);
				}
				else
				{
					Vector3 position2 = animal.transform.position;
					position2.y = Level.HEIGHT - 10f;
					vector = new Vector3?(position2);
				}
			}
			EffectAsset effectAsset = ZombieManager.Souls_1_Ref.Find();
			if (effectAsset != null)
			{
				EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
				{
					relevantDistance = 16f,
					position = animal.transform.position + Vector3.up
				});
			}
			animal.transform.position = vector.Value + Vector3.up;
		}

		/// <summary>
		/// Used in arena mode to reset all animals to dead.
		/// </summary>
		// Token: 0x060029AC RID: 10668 RVA: 0x000B178C File Offset: 0x000AF98C
		public static void askClearAllAnimals()
		{
			foreach (Animal animal in AnimalManager.animals)
			{
				EPlayerKill eplayerKill;
				uint num;
				animal.askDamage(ushort.MaxValue, Vector3.up, out eplayerKill, out num, false, false, ERagdollEffect.NONE);
			}
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x000B17EC File Offset: 0x000AF9EC
		private void respawnAnimals()
		{
			if (Level.info == null || Level.info.type == ELevelType.ARENA)
			{
				return;
			}
			if ((int)AnimalManager.respawnPackIndex >= AnimalManager.packs.Count)
			{
				AnimalManager.respawnPackIndex = (ushort)(AnimalManager.packs.Count - 1);
			}
			PackInfo packInfo = AnimalManager.packs[(int)AnimalManager.respawnPackIndex];
			AnimalManager.respawnPackIndex += 1;
			if ((int)AnimalManager.respawnPackIndex >= AnimalManager.packs.Count)
			{
				AnimalManager.respawnPackIndex = 0;
			}
			if (packInfo == null)
			{
				return;
			}
			for (int i = 0; i < packInfo.animals.Count; i++)
			{
				Animal animal = packInfo.animals[i];
				if (animal == null || animal.IsAlive || Time.realtimeSinceStartup - animal.lastDead < Provider.modeConfigData.Animals.Respawn_Time)
				{
					return;
				}
			}
			List<AnimalSpawnpoint> list = new List<AnimalSpawnpoint>();
			for (int j = 0; j < packInfo.spawns.Count; j++)
			{
				list.Add(packInfo.spawns[j]);
			}
			for (int k = 0; k < packInfo.animals.Count; k++)
			{
				Animal animal2 = packInfo.animals[k];
				if (!(animal2 == null))
				{
					int num = Random.Range(0, list.Count);
					AnimalSpawnpoint animalSpawnpoint = list[num];
					list.RemoveAt(num);
					Vector3 point = animalSpawnpoint.point;
					point.y += 0.1f;
					animal2.sendRevive(point, Random.Range(0f, 360f));
				}
			}
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x000B1970 File Offset: 0x000AFB70
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP)
			{
				AnimalManager.seq = 0U;
				AnimalManager._animals = new List<Animal>();
				AnimalManager._packs = null;
				AnimalManager.updates = 0;
				AnimalManager.tickIndex = 0;
				AnimalManager._tickingAnimals = new List<Animal>();
				if (Provider.isServer)
				{
					AnimalManager._packs = new List<PackInfo>();
					if (LevelAnimals.spawns.Count > 0 && Level.info != null && Level.info.type != ELevelType.ARENA)
					{
						for (int i = 0; i < LevelAnimals.spawns.Count; i++)
						{
							AnimalSpawnpoint animalSpawnpoint = LevelAnimals.spawns[i];
							int num = -1;
							for (int j = AnimalManager.packs.Count - 1; j >= 0; j--)
							{
								List<AnimalSpawnpoint> spawns = AnimalManager.packs[j].spawns;
								for (int k = 0; k < spawns.Count; k++)
								{
									if ((spawns[k].point - animalSpawnpoint.point).sqrMagnitude < 256f)
									{
										if (num == -1)
										{
											spawns.Add(animalSpawnpoint);
										}
										else
										{
											List<AnimalSpawnpoint> spawns2 = AnimalManager.packs[num].spawns;
											for (int l = 0; l < spawns2.Count; l++)
											{
												spawns.Add(spawns2[l]);
											}
											AnimalManager.packs.RemoveAtFast(num);
										}
										num = j;
										break;
									}
								}
							}
							if (num == -1)
							{
								PackInfo packInfo = new PackInfo();
								packInfo.spawns.Add(animalSpawnpoint);
								AnimalManager.packs.Add(packInfo);
							}
						}
						List<AnimalManager.ValidAnimalSpawnsInfo> list = new List<AnimalManager.ValidAnimalSpawnsInfo>();
						for (int m = 0; m < AnimalManager.packs.Count; m++)
						{
							PackInfo packInfo2 = AnimalManager.packs[m];
							List<AnimalSpawnpoint> list2 = new List<AnimalSpawnpoint>();
							for (int n = 0; n < packInfo2.spawns.Count; n++)
							{
								list2.Add(packInfo2.spawns[n]);
							}
							list.Add(new AnimalManager.ValidAnimalSpawnsInfo
							{
								spawns = list2,
								pack = packInfo2
							});
						}
						while ((long)AnimalManager.animals.Count < (long)((ulong)AnimalManager.maxInstances) && list.Count > 0)
						{
							int num2 = Random.Range(0, list.Count);
							AnimalManager.ValidAnimalSpawnsInfo validAnimalSpawnsInfo = list[num2];
							int num3 = Random.Range(0, validAnimalSpawnsInfo.spawns.Count);
							AnimalSpawnpoint animalSpawnpoint2 = validAnimalSpawnsInfo.spawns[num3];
							validAnimalSpawnsInfo.spawns.RemoveAt(num3);
							if (validAnimalSpawnsInfo.spawns.Count == 0)
							{
								list.RemoveAt(num2);
							}
							Vector3 point = animalSpawnpoint2.point;
							point.y += 0.1f;
							ushort id;
							if (validAnimalSpawnsInfo.pack.animals.Count > 0)
							{
								id = validAnimalSpawnsInfo.pack.animals[0].id;
							}
							else
							{
								id = LevelAnimals.getAnimal(animalSpawnpoint2);
							}
							Animal animal = this.addAnimal(id, point, Random.Range(0f, 360f), false);
							if (animal != null)
							{
								animal.pack = validAnimalSpawnsInfo.pack;
								validAnimalSpawnsInfo.pack.animals.Add(animal);
							}
						}
						for (int num4 = AnimalManager.packs.Count - 1; num4 >= 0; num4--)
						{
							if (AnimalManager.packs[num4].animals.Count <= 0)
							{
								AnimalManager.packs.RemoveAt(num4);
							}
						}
					}
				}
			}
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x000B1CF0 File Offset: 0x000AFEF0
		private void OnDrawGizmos()
		{
			if (AnimalManager.packs == null)
			{
				return;
			}
			for (int i = 0; i < AnimalManager.packs.Count; i++)
			{
				PackInfo packInfo = AnimalManager.packs[i];
				if (packInfo != null && packInfo.spawns != null && packInfo.animals != null)
				{
					Vector3 averageSpawnPoint = packInfo.getAverageSpawnPoint();
					Vector3 averageAnimalPoint = packInfo.getAverageAnimalPoint();
					Vector3 wanderDirection = packInfo.getWanderDirection();
					Gizmos.color = Color.gray;
					for (int j = 0; j < packInfo.spawns.Count; j++)
					{
						AnimalSpawnpoint animalSpawnpoint = packInfo.spawns[j];
						if (animalSpawnpoint != null)
						{
							Gizmos.DrawLine(averageSpawnPoint, animalSpawnpoint.point);
						}
					}
					Gizmos.color = Color.yellow;
					Gizmos.DrawLine(averageSpawnPoint, averageAnimalPoint);
					for (int k = 0; k < packInfo.animals.Count; k++)
					{
						Animal animal = packInfo.animals[k];
						if (!(animal == null))
						{
							Gizmos.color = (animal.isDead ? Color.red : Color.green);
							Gizmos.DrawLine(averageAnimalPoint, animal.transform.position);
							if (animal.IsAlive)
							{
								Gizmos.color = Color.magenta;
								Gizmos.DrawLine(animal.transform.position, animal.target);
							}
						}
					}
					Gizmos.color = Color.cyan;
					Gizmos.DrawLine(averageAnimalPoint, averageAnimalPoint + wanderDirection * 4f);
				}
			}
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x000B1E64 File Offset: 0x000B0064
		private void sendAnimalStates()
		{
			AnimalManager.seq += 1U;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (steamPlayer != null && !(steamPlayer.player == null))
				{
					ushort updateCount = 0;
					this.animalsToSend.Clear();
					for (int j = 0; j < AnimalManager.animals.Count; j++)
					{
						Animal animal = AnimalManager.animals[j];
						if (!(animal == null) && animal.isUpdated)
						{
							this.animalsToSend.Add(animal);
							ushort updateCount2 = updateCount;
							updateCount = updateCount2 + 1;
						}
					}
					if (updateCount != 0)
					{
						AnimalManager.SendAnimalStates.Invoke(ENetReliability.Unreliable, steamPlayer.transportConnection, delegate(NetPakWriter writer)
						{
							SystemNetPakWriterEx.WriteUInt32(writer, AnimalManager.seq);
							SystemNetPakWriterEx.WriteUInt16(writer, updateCount);
							foreach (Animal animal3 in this.animalsToSend)
							{
								SystemNetPakWriterEx.WriteUInt16(writer, animal3.index);
								UnityNetPakWriterEx.WriteClampedVector3(writer, animal3.transform.position, 13, 7);
								SystemNetPakWriterEx.WriteDegrees(writer, animal3.transform.eulerAngles.y, 8);
							}
						});
					}
				}
			}
			for (int k = 0; k < AnimalManager.animals.Count; k++)
			{
				Animal animal2 = AnimalManager.animals[k];
				if (!(animal2 == null))
				{
					animal2.isUpdated = false;
				}
			}
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x000B1F90 File Offset: 0x000B0190
		private void Update()
		{
			if (!Provider.isServer || !Level.isLoaded)
			{
				return;
			}
			if (AnimalManager.animals == null || AnimalManager.animals.Count == 0)
			{
				return;
			}
			if (AnimalManager.tickingAnimals == null)
			{
				return;
			}
			if (AnimalManager.tickIndex >= AnimalManager.tickingAnimals.Count)
			{
				AnimalManager.tickIndex = 0;
			}
			int num = AnimalManager.tickIndex;
			int num2 = num + 25;
			if (num2 >= AnimalManager.tickingAnimals.Count)
			{
				num2 = AnimalManager.tickingAnimals.Count;
			}
			AnimalManager.tickIndex = num2;
			for (int i = num2 - 1; i >= num; i--)
			{
				Animal animal = AnimalManager.tickingAnimals[i];
				if (animal == null)
				{
					UnturnedLog.error("Missing animal " + i.ToString());
				}
				else
				{
					animal.tick();
				}
			}
			if (Time.realtimeSinceStartup - AnimalManager.lastTick > Provider.UPDATE_TIME)
			{
				AnimalManager.lastTick += Provider.UPDATE_TIME;
				if (Time.realtimeSinceStartup - AnimalManager.lastTick > Provider.UPDATE_TIME)
				{
					AnimalManager.lastTick = Time.realtimeSinceStartup;
				}
				this.sendAnimalStates();
			}
			this.respawnAnimals();
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x000B2094 File Offset: 0x000B0294
		private void Start()
		{
			AnimalManager.manager = this;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x000B20E8 File Offset: 0x000B02E8
		private void OnLogMemoryUsage(List<string> results)
		{
			results.Add(string.Format("Animals: {0}", AnimalManager.animals.Count));
			results.Add(string.Format("Animal packs: {0}", AnimalManager.packs.Count));
			results.Add(string.Format("Ticking animals: {0}", AnimalManager.tickingAnimals.Count));
		}

		// Token: 0x04001669 RID: 5737
		private static AnimalManager manager;

		// Token: 0x0400166A RID: 5738
		private static List<Animal> _animals;

		// Token: 0x0400166B RID: 5739
		private static List<PackInfo> _packs;

		// Token: 0x0400166C RID: 5740
		private static int tickIndex;

		// Token: 0x0400166D RID: 5741
		private static List<Animal> _tickingAnimals;

		// Token: 0x0400166E RID: 5742
		public static ushort updates;

		// Token: 0x0400166F RID: 5743
		private static ushort respawnPackIndex;

		// Token: 0x04001670 RID: 5744
		private static float lastTick;

		// Token: 0x04001671 RID: 5745
		private static readonly ClientStaticMethod<ushort, Vector3, byte> SendAnimalAlive = ClientStaticMethod<ushort, Vector3, byte>.Get(new ClientStaticMethod<ushort, Vector3, byte>.ReceiveDelegate(AnimalManager.ReceiveAnimalAlive));

		// Token: 0x04001672 RID: 5746
		private static readonly ClientStaticMethod<ushort, Vector3, ERagdollEffect> SendAnimalDead = ClientStaticMethod<ushort, Vector3, ERagdollEffect>.Get(new ClientStaticMethod<ushort, Vector3, ERagdollEffect>.ReceiveDelegate(AnimalManager.ReceiveAnimalDead));

		// Token: 0x04001673 RID: 5747
		private static uint seq;

		// Token: 0x04001674 RID: 5748
		private static readonly ClientStaticMethod SendAnimalStates = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(AnimalManager.ReceiveAnimalStates));

		// Token: 0x04001675 RID: 5749
		private static readonly ClientStaticMethod<ushort> SendAnimalStartle = ClientStaticMethod<ushort>.Get(new ClientStaticMethod<ushort>.ReceiveDelegate(AnimalManager.ReceiveAnimalStartle));

		// Token: 0x04001676 RID: 5750
		private static readonly ClientStaticMethod<ushort> SendAnimalAttack = ClientStaticMethod<ushort>.Get(new ClientStaticMethod<ushort>.ReceiveDelegate(AnimalManager.ReceiveAnimalAttack));

		// Token: 0x04001677 RID: 5751
		private static readonly ClientStaticMethod<ushort> SendAnimalPanic = ClientStaticMethod<ushort>.Get(new ClientStaticMethod<ushort>.ReceiveDelegate(AnimalManager.ReceiveAnimalPanic));

		// Token: 0x04001678 RID: 5752
		private static readonly ClientStaticMethod SendMultipleAnimals = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(AnimalManager.ReceiveMultipleAnimals));

		// Token: 0x04001679 RID: 5753
		private static readonly ClientStaticMethod SendSingleAnimal = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(AnimalManager.ReceiveSingleAnimal));

		// Token: 0x0400167A RID: 5754
		private List<Animal> animalsToSend = new List<Animal>();

		// Token: 0x02000968 RID: 2408
		private class ValidAnimalSpawnsInfo
		{
			// Token: 0x0400335A RID: 13146
			public List<AnimalSpawnpoint> spawns;

			// Token: 0x0400335B RID: 13147
			public PackInfo pack;
		}
	}
}
