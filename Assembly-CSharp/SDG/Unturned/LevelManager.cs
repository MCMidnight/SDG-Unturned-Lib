using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000573 RID: 1395
	public class LevelManager : SteamCaller
	{
		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002C69 RID: 11369 RVA: 0x000C01C2 File Offset: 0x000BE3C2
		public static LevelManager instance
		{
			get
			{
				return LevelManager.manager;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002C6A RID: 11370 RVA: 0x000C01C9 File Offset: 0x000BE3C9
		public static ELevelType levelType
		{
			get
			{
				return LevelManager._levelType;
			}
		}

		/// <summary>
		/// Is the active level an Arena mode map?
		/// </summary>
		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002C6B RID: 11371 RVA: 0x000C01D0 File Offset: 0x000BE3D0
		public static bool isArenaMode
		{
			get
			{
				return LevelManager.levelType == ELevelType.ARENA;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002C6C RID: 11372 RVA: 0x000C01DA File Offset: 0x000BE3DA
		public static Vector3 arenaCurrentCenter
		{
			get
			{
				return LevelManager._arenaCurrentCenter;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002C6D RID: 11373 RVA: 0x000C01E1 File Offset: 0x000BE3E1
		public static Vector3 arenaOriginCenter
		{
			get
			{
				return LevelManager._arenaOriginCenter;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002C6E RID: 11374 RVA: 0x000C01E8 File Offset: 0x000BE3E8
		public static Vector3 arenaTargetCenter
		{
			get
			{
				return LevelManager._arenaTargetCenter;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002C6F RID: 11375 RVA: 0x000C01EF File Offset: 0x000BE3EF
		public static float arenaCurrentRadius
		{
			get
			{
				return LevelManager._arenaCurrentRadius;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002C70 RID: 11376 RVA: 0x000C01F6 File Offset: 0x000BE3F6
		public static float arenaOriginRadius
		{
			get
			{
				return LevelManager._arenaOriginRadius;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002C71 RID: 11377 RVA: 0x000C01FD File Offset: 0x000BE3FD
		public static float arenaTargetRadius
		{
			get
			{
				return LevelManager._arenaTargetRadius;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002C72 RID: 11378 RVA: 0x000C0204 File Offset: 0x000BE404
		public static float arenaCompactorSpeed
		{
			get
			{
				return LevelManager._arenaCompactorSpeed;
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002C73 RID: 11379 RVA: 0x000C020B File Offset: 0x000BE40B
		private static uint minPlayers
		{
			get
			{
				return Provider.modeConfigData.Events.Arena_Min_Players;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002C74 RID: 11380 RVA: 0x000C021C File Offset: 0x000BE41C
		public static float compactorSpeed
		{
			get
			{
				switch (Level.info.size)
				{
				case ELevelSize.TINY:
					return Provider.modeConfigData.Events.Arena_Compactor_Speed_Tiny;
				case ELevelSize.SMALL:
					return Provider.modeConfigData.Events.Arena_Compactor_Speed_Small;
				case ELevelSize.MEDIUM:
					return Provider.modeConfigData.Events.Arena_Compactor_Speed_Medium;
				case ELevelSize.LARGE:
					return Provider.modeConfigData.Events.Arena_Compactor_Speed_Large;
				case ELevelSize.INSANE:
					return Provider.modeConfigData.Events.Arena_Compactor_Speed_Insane;
				default:
					return 0f;
				}
			}
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x000C02A8 File Offset: 0x000BE4A8
		public static bool isPlayerInArena(Player player)
		{
			if (LevelManager.arenaState == EArenaState.CLEAR || LevelManager.arenaState == EArenaState.PLAY || LevelManager.arenaState == EArenaState.FINALE || LevelManager.arenaState == EArenaState.RESTART)
			{
				foreach (ArenaPlayer arenaPlayer in LevelManager.arenaPlayers)
				{
					if (arenaPlayer.steamPlayer != null && arenaPlayer.steamPlayer.player == player)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x000C0338 File Offset: 0x000BE538
		private void findGroups()
		{
			LevelManager.nonGroups = 0;
			LevelManager.arenaGroups.Clear();
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer steamPlayer = Provider.clients[i];
				if (steamPlayer != null && !(steamPlayer.player == null) && !steamPlayer.player.life.isDead)
				{
					if (!steamPlayer.player.quests.isMemberOfAGroup)
					{
						LevelManager.nonGroups++;
					}
					else if (!LevelManager.arenaGroups.Contains(steamPlayer.player.quests.groupID))
					{
						LevelManager.arenaGroups.Add(steamPlayer.player.quests.groupID);
					}
				}
			}
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x000C03F8 File Offset: 0x000BE5F8
		private void updateGroups(SteamPlayer steamPlayer)
		{
			if (!steamPlayer.player.quests.isMemberOfAGroup)
			{
				LevelManager.nonGroups--;
				return;
			}
			for (int i = LevelManager.arenaPlayers.Count - 1; i >= 0; i--)
			{
				if (LevelManager.arenaPlayers[i].steamPlayer.player.quests.isMemberOfSameGroupAs(steamPlayer.player))
				{
					return;
				}
			}
			LevelManager.arenaGroups.Remove(steamPlayer.player.quests.groupID);
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x000C047E File Offset: 0x000BE67E
		private void arenaLobby()
		{
			this.findGroups();
			if ((long)(LevelManager.nonGroups + LevelManager.arenaGroups.Count) < (long)((ulong)LevelManager.minPlayers))
			{
				if (LevelManager.arenaMessage != EArenaMessage.LOBBY)
				{
					LevelManager.SendArenaMessage.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), EArenaMessage.LOBBY);
				}
				return;
			}
			LevelManager.arenaState = EArenaState.CLEAR;
		}

		/// <summary>
		/// Find a new smaller circle within the old circle and clamp it to the playable level area.
		/// </summary>
		// Token: 0x06002C79 RID: 11385 RVA: 0x000C04C0 File Offset: 0x000BE6C0
		private void getArenaTarget(Vector3 currentCenter, float currentRadius, out Vector3 targetCenter, out float targetRadius)
		{
			targetCenter = currentCenter;
			targetRadius = currentRadius * Provider.modeConfigData.Events.Arena_Compactor_Shrink_Factor;
			float f = Random.Range(0f, 6.2831855f);
			float num = Mathf.Cos(f);
			float num2 = Mathf.Sin(f);
			float num3 = Random.Range(0f, currentRadius - targetRadius);
			targetCenter += new Vector3(num * num3, 0f, num2 * num3);
			if (targetCenter.x - targetRadius < (float)(-Level.size / 2 + Level.border))
			{
				targetRadius = targetCenter.x - (float)(-Level.size / 2 + Level.border);
			}
			if (targetCenter.x + targetRadius > (float)(Level.size / 2 - Level.border))
			{
				targetRadius = (float)(Level.size / 2 - Level.border) - targetCenter.x;
			}
			if (targetCenter.z - targetRadius < (float)(-Level.size / 2 + Level.border))
			{
				targetRadius = targetCenter.z - (float)(-Level.size / 2 + Level.border);
			}
			if (targetCenter.z + targetRadius > (float)(Level.size / 2 - Level.border))
			{
				targetRadius = (float)(Level.size / 2 - Level.border) - targetCenter.z;
			}
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x000C0604 File Offset: 0x000BE804
		private void arenaClear()
		{
			AnimalManager.askClearAllAnimals();
			VehicleManager.askVehicleDestroyAll();
			BarricadeManager.askClearAllBarricades();
			StructureManager.askClearAllStructures();
			ItemManager.askClearAllItems();
			EffectManager.askEffectClearAll();
			ObjectManager.askClearAllObjects();
			ResourceManager.askClearAllResources();
			LevelManager.arenaPlayers.Clear();
			Vector3 vector = Vector3.zero;
			float num = (float)Level.size / 2f;
			if (Level.info.configData.Use_Arena_Compactor)
			{
				ArenaCompactorVolume randomVolumeOrNull = VolumeManager<ArenaCompactorVolume, ArenaCompactorVolumeManager>.Get().GetRandomVolumeOrNull();
				if (randomVolumeOrNull != null)
				{
					vector = randomVolumeOrNull.transform.position;
					vector.y = 0f;
					num = randomVolumeOrNull.GetSphereRadius();
				}
			}
			else
			{
				num = 16384f;
			}
			float compactorSpeed = LevelManager.compactorSpeed;
			Vector3 arg;
			float arg2;
			if (Level.info.configData.Use_Arena_Compactor)
			{
				if (Provider.modeConfigData.Events.Arena_Use_Compactor_Pause)
				{
					this.getArenaTarget(vector, num, out arg, out arg2);
				}
				else
				{
					arg = vector;
					arg2 = 0.5f;
				}
			}
			else
			{
				arg = vector;
				arg2 = num;
			}
			LevelManager.SendArenaOrigin.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vector, num, vector, num, arg, arg2, compactorSpeed, (byte)(Provider.modeConfigData.Events.Arena_Clear_Timer + Provider.modeConfigData.Events.Arena_Compactor_Delay_Timer));
			LevelManager.arenaState = EArenaState.WARMUP;
			LevelManager.SendLevelTimer.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (byte)Provider.modeConfigData.Events.Arena_Clear_Timer);
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x000C074C File Offset: 0x000BE94C
		private void arenaWarmUp()
		{
			if (LevelManager.arenaMessage != EArenaMessage.WARMUP)
			{
				LevelManager.SendArenaMessage.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), EArenaMessage.WARMUP);
			}
			if (LevelManager.countTimerMessages >= 0)
			{
				return;
			}
			this.findGroups();
			if ((long)(LevelManager.nonGroups + LevelManager.arenaGroups.Count) < (long)((ulong)LevelManager.minPlayers))
			{
				LevelManager.arenaState = EArenaState.LOBBY;
				return;
			}
			LevelManager.arenaState = EArenaState.SPAWN;
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x000C07A8 File Offset: 0x000BE9A8
		private void arenaSpawn()
		{
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					if (LevelItems.spawns[(int)b, (int)b2].Count > 0)
					{
						for (int i = 0; i < LevelItems.spawns[(int)b, (int)b2].Count; i++)
						{
							ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int)b, (int)b2][i];
							ushort item = LevelItems.getItem(itemSpawnpoint);
							if (item != 0)
							{
								ItemManager.dropItem(new Item(item, EItemOrigin.ADMIN), itemSpawnpoint.point, false, false, false);
							}
						}
					}
				}
			}
			List<VehicleSpawnpoint> spawns = LevelVehicles.spawns;
			for (int j = 0; j < spawns.Count; j++)
			{
				VehicleSpawnpoint vehicleSpawnpoint = spawns[j];
				Asset randomAssetForSpawnpoint = LevelVehicles.GetRandomAssetForSpawnpoint(vehicleSpawnpoint);
				if (randomAssetForSpawnpoint != null)
				{
					Vector3 point = vehicleSpawnpoint.point;
					point.y += 1f;
					VehicleManager.spawnVehicleV2(randomAssetForSpawnpoint, point, Quaternion.Euler(0f, vehicleSpawnpoint.angle, 0f));
				}
			}
			foreach (AnimalSpawnpoint animalSpawnpoint in LevelAnimals.spawns)
			{
				ushort animal = LevelAnimals.getAnimal(animalSpawnpoint);
				if (animal != 0)
				{
					Vector3 point2 = animalSpawnpoint.point;
					point2.y += 0.1f;
					AnimalManager.spawnAnimal(animal, point2, Quaternion.Euler(0f, (float)Random.Range(0, 360), 0f));
				}
			}
			List<PlayerSpawnpoint> altSpawns = LevelPlayers.getAltSpawns();
			float num = LevelManager.arenaCurrentRadius - SafezoneNode.MIN_SIZE;
			num *= num;
			for (int k = altSpawns.Count - 1; k >= 0; k--)
			{
				if (MathfEx.HorizontalDistanceSquared(altSpawns[k].point, LevelManager.arenaCurrentCenter) > num)
				{
					altSpawns.RemoveAt(k);
				}
			}
			List<SteamPlayer> list = new List<SteamPlayer>(Provider.clients);
			while (altSpawns.Count > 0 && list.Count != 0)
			{
				int num2 = Random.Range(0, list.Count);
				SteamPlayer steamPlayer = list[num2];
				list.RemoveAtFast(num2);
				if (steamPlayer != null && !(steamPlayer.player == null) && !steamPlayer.player.life.isDead)
				{
					int num3 = Random.Range(0, altSpawns.Count);
					PlayerSpawnpoint playerSpawnpoint = altSpawns[num3];
					altSpawns.RemoveAt(num3);
					ArenaPlayer arenaPlayer = new ArenaPlayer(steamPlayer);
					arenaPlayer.steamPlayer.player.life.sendRevive();
					arenaPlayer.steamPlayer.player.teleportToLocationUnsafe(playerSpawnpoint.point, playerSpawnpoint.angle);
					LevelManager.arenaPlayers.Add(arenaPlayer);
					foreach (ArenaLoadout arenaLoadout in Level.info.configData.Arena_Loadouts)
					{
						for (ushort num4 = 0; num4 < arenaLoadout.Amount; num4 += 1)
						{
							ushort num5 = SpawnTableTool.ResolveLegacyId(arenaLoadout.Table_ID, EAssetType.ITEM, new Func<string>(this.OnGetArenaLoadoutsSpawnTableErrorContext));
							if (num5 != 0)
							{
								arenaPlayer.steamPlayer.player.inventory.forceAddItemAuto(new Item(num5, true), true, false, true, false);
							}
						}
					}
				}
			}
			this.arenaAirdrop();
			LevelManager.arenaState = EArenaState.PLAY;
			LevelManager.SendLevelNumber.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (byte)LevelManager.arenaPlayers.Count);
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x000C0B44 File Offset: 0x000BED44
		private string OnGetArenaLoadoutsSpawnTableErrorContext()
		{
			return "level config arena loadout";
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x000C0B4C File Offset: 0x000BED4C
		private void arenaAirdrop()
		{
			if (!Provider.modeConfigData.Events.Use_Airdrops)
			{
				return;
			}
			Vector3 arenaTargetCenter = LevelManager.arenaTargetCenter;
			float arenaTargetRadius = LevelManager.arenaTargetRadius;
			float num = arenaTargetRadius * arenaTargetRadius;
			List<AirdropDevkitNode> list = new List<AirdropDevkitNode>();
			foreach (AirdropDevkitNode airdropDevkitNode in LevelManager.airdropNodes)
			{
				if ((airdropDevkitNode.transform.position - arenaTargetCenter).sqrMagnitude < num)
				{
					list.Add(airdropDevkitNode);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			AirdropDevkitNode airdropDevkitNode2 = list[Random.Range(0, list.Count)];
			LevelManager.airdrop(airdropDevkitNode2.transform.position, airdropDevkitNode2.id, Provider.modeConfigData.Events.Airdrop_Speed);
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x000C0C28 File Offset: 0x000BEE28
		private void arenaPlay()
		{
			if (LevelManager.arenaMessage != EArenaMessage.PLAY)
			{
				LevelManager.SendArenaMessage.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), EArenaMessage.PLAY);
			}
			if ((long)(LevelManager.nonGroups + LevelManager.arenaGroups.Count) >= (long)((ulong)LevelManager.minPlayers))
			{
				float time = Time.time;
				float deltaTime = Time.deltaTime;
				for (int i = LevelManager.arenaPlayers.Count - 1; i >= 0; i--)
				{
					ArenaPlayer arenaPlayer = LevelManager.arenaPlayers[i];
					if (arenaPlayer.steamPlayer == null || arenaPlayer.steamPlayer.player == null)
					{
						ulong[] playersIDs = new ulong[1];
						playersIDs[0] = arenaPlayer.steamPlayer.playerID.steamID.m_SteamID;
						LevelManager.SendArenaPlayer.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), delegate(NetPakWriter writer)
						{
							LevelManager.WriteArenaPlayer(writer, playersIDs, EArenaMessage.ABANDONED);
						});
						LevelManager.arenaPlayers.RemoveAt(i);
						this.updateGroups(arenaPlayer.steamPlayer);
						LevelManager.SendLevelNumber.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (byte)LevelManager.arenaPlayers.Count);
					}
					else
					{
						if (MathfEx.HorizontalDistanceSquared(arenaPlayer.steamPlayer.player.transform.position, LevelManager.arenaCurrentCenter) > LevelManager.arenaSqrRadius || LevelManager.arenaCurrentRadius < 1f)
						{
							if (time - arenaPlayer.lastAreaDamage > 1f)
							{
								float num = Provider.modeConfigData.Events.Arena_Compactor_Extra_Damage_Per_Second * arenaPlayer.timeOutsideArea;
								byte amount = MathfEx.RoundAndClampToByte(Provider.modeConfigData.Events.Arena_Compactor_Damage + num);
								EPlayerKill eplayerKill;
								arenaPlayer.steamPlayer.player.life.askDamage(amount, Vector3.up * 10f, EDeathCause.ARENA, ELimb.SPINE, CSteamID.Nil, out eplayerKill, false, ERagdollEffect.NONE, true, true);
								arenaPlayer.lastAreaDamage = time;
							}
							arenaPlayer.timeOutsideArea += deltaTime;
						}
						else
						{
							arenaPlayer.timeOutsideArea = 0f;
						}
						if (arenaPlayer.hasDied)
						{
							ulong[] playersIDs = new ulong[1];
							playersIDs[0] = arenaPlayer.steamPlayer.playerID.steamID.m_SteamID;
							LevelManager.SendArenaPlayer.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), delegate(NetPakWriter writer)
							{
								LevelManager.WriteArenaPlayer(writer, playersIDs, EArenaMessage.DIED);
							});
							LevelManager.arenaPlayers.RemoveAt(i);
							this.updateGroups(arenaPlayer.steamPlayer);
							LevelManager.SendLevelNumber.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (byte)LevelManager.arenaPlayers.Count);
						}
					}
				}
				return;
			}
			LevelManager.arenaState = EArenaState.FINALE;
			LevelManager.lastFinaleMessage = Time.realtimeSinceStartup;
			if (LevelManager.arenaPlayers.Count > 0)
			{
				ulong[] playersIDs = new ulong[LevelManager.arenaPlayers.Count];
				for (int j = 0; j < LevelManager.arenaPlayers.Count; j++)
				{
					playersIDs[j] = LevelManager.arenaPlayers[j].steamPlayer.playerID.steamID.m_SteamID;
				}
				LevelManager.arenaMessage = EArenaMessage.LOSE;
				LevelManager.SendArenaPlayer.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), delegate(NetPakWriter writer)
				{
					LevelManager.WriteArenaPlayer(writer, playersIDs, EArenaMessage.WIN);
				});
				return;
			}
			LevelManager.SendArenaMessage.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), EArenaMessage.LOSE);
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x000C0F61 File Offset: 0x000BF161
		private void arenaFinale()
		{
			if (Time.realtimeSinceStartup - LevelManager.lastFinaleMessage > Provider.modeConfigData.Events.Arena_Finale_Timer)
			{
				LevelManager.arenaState = EArenaState.RESTART;
			}
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x000C0F88 File Offset: 0x000BF188
		private void arenaRestart()
		{
			LevelManager.arenaState = EArenaState.INTERMISSION;
			LevelManager.SendLevelTimer.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), (byte)Provider.modeConfigData.Events.Arena_Restart_Timer);
			for (int i = LevelManager.arenaPlayers.Count - 1; i >= 0; i--)
			{
				ArenaPlayer arenaPlayer = LevelManager.arenaPlayers[i];
				if (!arenaPlayer.hasDied && arenaPlayer.steamPlayer != null && !(arenaPlayer.steamPlayer.player == null))
				{
					arenaPlayer.steamPlayer.player.sendStat(EPlayerStat.ARENA_WINS);
					EPlayerKill eplayerKill;
					arenaPlayer.steamPlayer.player.life.askDamage(101, Vector3.up * 101f, EDeathCause.ARENA, ELimb.SPINE, CSteamID.Nil, out eplayerKill, false, ERagdollEffect.NONE, true, true);
				}
			}
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x000C1048 File Offset: 0x000BF248
		private void arenaIntermission()
		{
			if (LevelManager.arenaMessage != EArenaMessage.INTERMISSION)
			{
				LevelManager.SendArenaMessage.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), EArenaMessage.INTERMISSION);
			}
			if (LevelManager.countTimerMessages >= 0)
			{
				return;
			}
			LevelManager.arenaState = EArenaState.LOBBY;
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x000C1074 File Offset: 0x000BF274
		private void arenaTick()
		{
			if (Time.realtimeSinceStartup > LevelManager.nextAreaModify)
			{
				LevelManager._arenaCurrentRadius = LevelManager.arenaCurrentRadius - Time.deltaTime * LevelManager.arenaCompactorSpeed;
				if (LevelManager.arenaCurrentRadius < LevelManager.arenaTargetRadius)
				{
					LevelManager._arenaCurrentRadius = LevelManager.arenaTargetRadius;
					if (Provider.isServer && Level.info.configData.Use_Arena_Compactor && Provider.modeConfigData.Events.Arena_Use_Compactor_Pause)
					{
						float compactorSpeed = LevelManager.compactorSpeed;
						Vector3 arg;
						float arg2;
						this.getArenaTarget(LevelManager.arenaTargetCenter, LevelManager.arenaTargetRadius, out arg, out arg2);
						LevelManager.SendArenaOrigin.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), LevelManager.arenaTargetCenter, LevelManager.arenaTargetRadius, LevelManager.arenaTargetCenter, LevelManager.arenaTargetRadius, arg, arg2, compactorSpeed, (byte)Provider.modeConfigData.Events.Arena_Compactor_Pause_Timer);
					}
				}
				LevelManager.arenaSqrRadius = LevelManager.arenaCurrentRadius * LevelManager.arenaCurrentRadius;
				float t = Mathf.InverseLerp(LevelManager.arenaTargetRadius, LevelManager.arenaOriginRadius, LevelManager.arenaCurrentRadius);
				LevelManager._arenaCurrentCenter = Vector3.Lerp(LevelManager.arenaTargetCenter, LevelManager.arenaOriginCenter, t);
			}
			if (LevelManager.countTimerMessages >= 0 && Time.realtimeSinceStartup - LevelManager.lastTimerMessage > 1f)
			{
				LevelNumberUpdated levelNumberUpdated = LevelManager.onLevelNumberUpdated;
				if (levelNumberUpdated != null)
				{
					levelNumberUpdated(LevelManager.countTimerMessages);
				}
				LevelManager.lastTimerMessage = Time.realtimeSinceStartup;
				LevelManager.countTimerMessages--;
				EArenaMessage earenaMessage = LevelManager.arenaMessage;
			}
			if (Provider.isServer)
			{
				switch (LevelManager.arenaState)
				{
				case EArenaState.LOBBY:
					this.arenaLobby();
					return;
				case EArenaState.CLEAR:
					this.arenaClear();
					return;
				case EArenaState.WARMUP:
					this.arenaWarmUp();
					return;
				case EArenaState.SPAWN:
					this.arenaSpawn();
					return;
				case EArenaState.PLAY:
					this.arenaPlay();
					return;
				case EArenaState.FINALE:
					this.arenaFinale();
					return;
				case EArenaState.RESTART:
					this.arenaRestart();
					return;
				case EArenaState.INTERMISSION:
					this.arenaIntermission();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x000C1230 File Offset: 0x000BF430
		private void arenaInit()
		{
			LevelManager._arenaCurrentCenter = Vector3.zero;
			LevelManager._arenaTargetCenter = Vector3.zero;
			LevelManager._arenaCurrentRadius = 16384f;
			LevelManager._arenaTargetRadius = 16384f;
			LevelManager._arenaCompactorSpeed = 0f;
			if (Provider.isServer)
			{
				LevelManager.arenaState = EArenaState.LOBBY;
				LevelManager.arenaGroups = new List<CSteamID>();
				LevelManager.arenaPlayers = new List<ArenaPlayer>();
			}
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x000C1290 File Offset: 0x000BF490
		[Obsolete]
		public void tellArenaOrigin(CSteamID steamID, Vector3 newArenaCurrentCenter, float newArenaCurrentRadius, Vector3 newArenaOriginCenter, float newArenaOriginRadius, Vector3 newArenaTargetCenter, float newArenaTargetRadius, float newArenaCompactorSpeed, byte delay)
		{
			LevelManager.ReceiveArenaOrigin(newArenaCurrentCenter, newArenaCurrentRadius, newArenaOriginCenter, newArenaOriginRadius, newArenaTargetCenter, newArenaTargetRadius, newArenaCompactorSpeed, delay);
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x000C12A8 File Offset: 0x000BF4A8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellArenaOrigin")]
		public static void ReceiveArenaOrigin(Vector3 newArenaCurrentCenter, float newArenaCurrentRadius, Vector3 newArenaOriginCenter, float newArenaOriginRadius, Vector3 newArenaTargetCenter, float newArenaTargetRadius, float newArenaCompactorSpeed, byte delay)
		{
			LevelManager._arenaCurrentCenter = newArenaCurrentCenter;
			LevelManager._arenaCurrentRadius = newArenaCurrentRadius;
			LevelManager.arenaSqrRadius = LevelManager.arenaCurrentRadius * LevelManager.arenaCurrentRadius;
			LevelManager._arenaOriginCenter = newArenaOriginCenter;
			LevelManager._arenaOriginRadius = newArenaOriginRadius;
			LevelManager._arenaTargetCenter = newArenaTargetCenter;
			LevelManager._arenaTargetRadius = newArenaTargetRadius;
			LevelManager._arenaCompactorSpeed = newArenaCompactorSpeed;
			if (delay == 0)
			{
				LevelManager.nextAreaModify = 0f;
				return;
			}
			LevelManager.nextAreaModify = Time.realtimeSinceStartup + (float)delay;
		}

		// Token: 0x06002C87 RID: 11399 RVA: 0x000C130F File Offset: 0x000BF50F
		[Obsolete]
		public void tellArenaMessage(CSteamID steamID, byte newArenaMessage)
		{
			LevelManager.ReceiveArenaMessage((EArenaMessage)newArenaMessage);
		}

		// Token: 0x06002C88 RID: 11400 RVA: 0x000C1317 File Offset: 0x000BF517
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellArenaMessage")]
		public static void ReceiveArenaMessage(EArenaMessage newArenaMessage)
		{
			LevelManager.arenaMessage = newArenaMessage;
			ArenaMessageUpdated arenaMessageUpdated = LevelManager.onArenaMessageUpdated;
			if (arenaMessageUpdated == null)
			{
				return;
			}
			arenaMessageUpdated(LevelManager.arenaMessage);
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x000C1333 File Offset: 0x000BF533
		[Obsolete]
		public void tellArenaPlayer(CSteamID steamID, ulong[] newPlayerIDs, byte newArenaMessage)
		{
			ArenaPlayerUpdated arenaPlayerUpdated = LevelManager.onArenaPlayerUpdated;
			if (arenaPlayerUpdated == null)
			{
				return;
			}
			arenaPlayerUpdated(newPlayerIDs, (EArenaMessage)newArenaMessage);
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x000C1348 File Offset: 0x000BF548
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveArenaPlayer(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			ulong[] array = new ulong[(int)b];
			for (byte b2 = 0; b2 < b; b2 += 1)
			{
				SystemNetPakReaderEx.ReadUInt64(reader, ref array[(int)b2]);
			}
			EArenaMessage newArenaMessage;
			reader.ReadEnum(out newArenaMessage);
			ArenaPlayerUpdated arenaPlayerUpdated = LevelManager.onArenaPlayerUpdated;
			if (arenaPlayerUpdated == null)
			{
				return;
			}
			arenaPlayerUpdated(array, newArenaMessage);
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x000C13A8 File Offset: 0x000BF5A8
		private static void WriteArenaPlayer(NetPakWriter writer, ulong[] newPlayerIDs, EArenaMessage newArenaMessage)
		{
			byte b = (byte)newPlayerIDs.Length;
			SystemNetPakWriterEx.WriteUInt8(writer, b);
			for (byte b2 = 0; b2 < b; b2 += 1)
			{
				SystemNetPakWriterEx.WriteUInt64(writer, newPlayerIDs[(int)b2]);
			}
			writer.WriteEnum(newArenaMessage);
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x000C13E1 File Offset: 0x000BF5E1
		[Obsolete]
		public void tellLevelNumber(CSteamID steamID, byte newLevelNumber)
		{
			LevelManager.ReceiveLevelNumber(newLevelNumber);
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x000C13E9 File Offset: 0x000BF5E9
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellLevelNumber")]
		public static void ReceiveLevelNumber(byte newLevelNumber)
		{
			LevelManager.countTimerMessages = -1;
			LevelNumberUpdated levelNumberUpdated = LevelManager.onLevelNumberUpdated;
			if (levelNumberUpdated == null)
			{
				return;
			}
			levelNumberUpdated((int)newLevelNumber);
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x000C1401 File Offset: 0x000BF601
		[Obsolete]
		public void tellLevelTimer(CSteamID steamID, byte newTimerCount)
		{
			LevelManager.ReceiveLevelTimer(newTimerCount);
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x000C1409 File Offset: 0x000BF609
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellLevelTimer")]
		public static void ReceiveLevelTimer(byte newTimerCount)
		{
			LevelManager.countTimerMessages = (int)newTimerCount;
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x000C1411 File Offset: 0x000BF611
		[Obsolete]
		public void askArenaState(CSteamID steamID)
		{
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002C91 RID: 11409 RVA: 0x000C1413 File Offset: 0x000BF613
		public static bool hasAirdrop
		{
			get
			{
				return LevelManager._hasAirdrop;
			}
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x000C141C File Offset: 0x000BF61C
		public static void airdrop(Vector3 point, ushort id, float speed)
		{
			if (id == 0)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			if (Random.value < 0.5f)
			{
				vector.x = (float)(Level.size / 2) * -Mathf.Sign(point.x);
				vector.z = (float)Random.Range(0, (int)(Level.size / 2)) * -Mathf.Sign(point.z);
			}
			else
			{
				vector.x = (float)Random.Range(0, (int)(Level.size / 2)) * -Mathf.Sign(point.x);
				vector.z = (float)(Level.size / 2) * -Mathf.Sign(point.z);
			}
			float y = point.y + 450f;
			point.y = 0f;
			Vector3 normalized = (point - vector).normalized;
			vector += normalized * -2048f;
			float num = (point - vector).magnitude / speed;
			vector.y = y;
			float airdrop_Force = Provider.modeConfigData.Events.Airdrop_Force;
			LevelManager.manager.airdropSpawn(id, vector, normalized, speed, airdrop_Force, num);
			LevelManager.SendAirdropState.Invoke(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vector, normalized, speed, airdrop_Force, num);
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x000C154C File Offset: 0x000BF74C
		private void airdropTick()
		{
			for (int i = LevelManager.airdrops.Count - 1; i >= 0; i--)
			{
				AirdropInfo airdropInfo = LevelManager.airdrops[i];
				airdropInfo.state += airdropInfo.direction * airdropInfo.speed * Time.deltaTime;
				airdropInfo.delay -= Time.deltaTime;
				if (airdropInfo.model != null)
				{
					airdropInfo.model.position = airdropInfo.state;
				}
				if (airdropInfo.dropped)
				{
					if (Mathf.Abs(airdropInfo.state.x) > (float)(Level.size / 2 + 2048) || Mathf.Abs(airdropInfo.state.z) > (float)(Level.size / 2 + 2048))
					{
						if (airdropInfo.model != null)
						{
							Object.Destroy(airdropInfo.model.gameObject);
						}
						LevelManager.airdrops.RemoveAt(i);
					}
				}
				else if (airdropInfo.delay <= 0f)
				{
					airdropInfo.dropped = true;
					LevelAsset asset = Level.getAsset();
					AssetReference<AirdropAsset> assetReference = (asset != null) ? asset.airdropRef : AssetReference<AirdropAsset>.invalid;
					if (assetReference.isNull)
					{
						assetReference = AirdropAsset.defaultAirdrop;
					}
					AirdropAsset airdropAsset = assetReference.Find();
					MasterBundleReference<GameObject> masterBundleReference = (airdropAsset != null) ? airdropAsset.model : MasterBundleReference<GameObject>.invalid;
					if (masterBundleReference.isNull)
					{
						masterBundleReference = new MasterBundleReference<GameObject>("core.masterbundle", "Level/Carepackage.prefab");
					}
					Transform transform = Object.Instantiate<GameObject>(masterBundleReference.loadAsset(true), airdropInfo.dropPosition, Quaternion.identity).transform;
					transform.name = "Carepackage";
					Carepackage orAddComponent = transform.GetOrAddComponent<Carepackage>();
					orAddComponent.id = airdropInfo.id;
					if (airdropAsset != null)
					{
						orAddComponent.barricadeAsset = airdropAsset.barricadeRef.Find();
					}
					ConstantForce component = transform.GetComponent<ConstantForce>();
					if (component != null)
					{
						component.force = new Vector3(0f, airdropInfo.force, 0f);
					}
					LevelManager.airdrops.RemoveAt(i);
				}
			}
			if (Provider.isServer && LevelManager.levelType == ELevelType.SURVIVAL && Provider.modeConfigData.Events.Use_Airdrops && LevelManager.airdropNodes.Count > 0)
			{
				if (!LevelManager.hasAirdrop)
				{
					LevelManager.airdropFrequency = (uint)(Random.Range(Provider.modeConfigData.Events.Airdrop_Frequency_Min, Provider.modeConfigData.Events.Airdrop_Frequency_Max) * LightingManager.cycle);
					LevelManager._hasAirdrop = true;
					LevelManager.lastAirdrop = Time.realtimeSinceStartup;
				}
				if (LevelManager.airdropFrequency > 0U)
				{
					if (Time.realtimeSinceStartup - LevelManager.lastAirdrop > 1f)
					{
						LevelManager.airdropFrequency -= 1U;
						LevelManager.lastAirdrop = Time.realtimeSinceStartup;
						return;
					}
				}
				else
				{
					AirdropDevkitNode airdropDevkitNode = LevelManager.airdropNodes[Random.Range(0, LevelManager.airdropNodes.Count)];
					LevelManager.airdrop(airdropDevkitNode.transform.position, airdropDevkitNode.id, Provider.modeConfigData.Events.Airdrop_Speed);
					LevelManager._hasAirdrop = false;
				}
			}
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x000C1854 File Offset: 0x000BFA54
		private void airdropInit()
		{
			LevelManager.lastAirdrop = Time.realtimeSinceStartup;
			LevelManager.airdrops = new List<AirdropInfo>();
			if (Provider.isServer)
			{
				LevelManager.airdropNodes = new List<AirdropDevkitNode>();
				foreach (AirdropDevkitNode airdropDevkitNode in AirdropDevkitNodeSystem.Get().GetAllNodes())
				{
					if (airdropDevkitNode.id != 0)
					{
						LevelManager.airdropNodes.Add(airdropDevkitNode);
					}
				}
				LevelManager.load();
			}
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x000C18DC File Offset: 0x000BFADC
		private void airdropSpawn(ushort id, Vector3 state, Vector3 direction, float speed, float force, float delay)
		{
			AirdropInfo airdropInfo = new AirdropInfo();
			airdropInfo.id = id;
			airdropInfo.state = state;
			airdropInfo.direction = direction;
			airdropInfo.speed = speed;
			airdropInfo.force = force;
			airdropInfo.delay = delay;
			airdropInfo.dropped = false;
			airdropInfo.dropPosition = state + direction * speed * delay;
			LevelManager.airdrops.Add(airdropInfo);
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x000C1949 File Offset: 0x000BFB49
		[Obsolete]
		public void tellAirdropState(CSteamID steamID, Vector3 state, Vector3 direction, float speed, float force, float delay)
		{
			LevelManager.ReceiveAirdropState(state, direction, speed, force, delay);
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x000C1958 File Offset: 0x000BFB58
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellAirdropState")]
		public static void ReceiveAirdropState(Vector3 state, Vector3 direction, float speed, float force, float delay)
		{
			LevelManager.manager.airdropSpawn(0, state, direction, speed, force, delay);
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x000C196B File Offset: 0x000BFB6B
		[Obsolete]
		public void askAirdropState(CSteamID steamID)
		{
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x000C1970 File Offset: 0x000BFB70
		internal static void SendInitialGlobalState(SteamPlayer client)
		{
			if (Level.info.type == ELevelType.ARENA)
			{
				LevelManager.SendArenaOrigin.Invoke(ENetReliability.Reliable, client.transportConnection, LevelManager.arenaCurrentCenter, LevelManager.arenaCurrentRadius, LevelManager.arenaOriginCenter, LevelManager.arenaOriginRadius, LevelManager.arenaTargetCenter, LevelManager.arenaTargetRadius, LevelManager.arenaCompactorSpeed, 0);
				LevelManager.SendArenaMessage.Invoke(ENetReliability.Reliable, client.transportConnection, LevelManager.arenaMessage);
				if (LevelManager.countTimerMessages > 0)
				{
					LevelManager.SendLevelTimer.Invoke(ENetReliability.Reliable, client.transportConnection, (byte)LevelManager.countTimerMessages);
				}
				else
				{
					LevelManager.SendLevelNumber.Invoke(ENetReliability.Reliable, client.transportConnection, (byte)LevelManager.arenaPlayers.Count);
				}
			}
			for (int i = 0; i < LevelManager.airdrops.Count; i++)
			{
				AirdropInfo airdropInfo = LevelManager.airdrops[i];
				LevelManager.SendAirdropState.Invoke(ENetReliability.Reliable, client.transportConnection, airdropInfo.state, airdropInfo.direction, airdropInfo.speed, airdropInfo.force, airdropInfo.delay);
			}
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x000C1A68 File Offset: 0x000BFC68
		private void onLevelLoaded(int level)
		{
			LevelManager.isInit = false;
			if (level > Level.BUILD_INDEX_SETUP && Level.info != null)
			{
				LevelManager.isInit = true;
				LevelManager._levelType = Level.info.type;
				if (LevelManager.levelType == ELevelType.ARENA)
				{
					this.arenaInit();
				}
				if (LevelManager.levelType != ELevelType.HORDE)
				{
					this.airdropInit();
				}
			}
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x000C1ABB File Offset: 0x000BFCBB
		private void Update()
		{
			if (!LevelManager.isInit)
			{
				return;
			}
			if (LevelManager.levelType == ELevelType.ARENA)
			{
				this.arenaTick();
			}
			if (LevelManager.levelType != ELevelType.HORDE)
			{
				this.airdropTick();
			}
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x000C1AE1 File Offset: 0x000BFCE1
		private void Start()
		{
			LevelManager.manager = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x000C1B0C File Offset: 0x000BFD0C
		public static void load()
		{
			bool flag = true;
			if (LevelSavedata.fileExists("/Events.dat"))
			{
				River river = LevelSavedata.openRiver("/Events.dat", true);
				if (river.readByte() > 0)
				{
					LevelManager.airdropFrequency = river.readUInt32();
					LevelManager._hasAirdrop = river.readBoolean();
					flag = false;
				}
				river.closeRiver();
			}
			if (flag)
			{
				LevelManager._hasAirdrop = false;
			}
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x000C1B63 File Offset: 0x000BFD63
		public static void save()
		{
			River river = LevelSavedata.openRiver("/Events.dat", false);
			river.writeByte(LevelManager.SAVEDATA_VERSION);
			river.writeUInt32(LevelManager.airdropFrequency);
			river.writeBoolean(LevelManager.hasAirdrop);
			river.closeRiver();
		}

		// Token: 0x040017F7 RID: 6135
		public static readonly byte SAVEDATA_VERSION = 1;

		// Token: 0x040017F8 RID: 6136
		private static LevelManager manager;

		// Token: 0x040017F9 RID: 6137
		private static bool isInit;

		// Token: 0x040017FA RID: 6138
		private static ELevelType _levelType;

		// Token: 0x040017FB RID: 6139
		private static AudioClip timer;

		// Token: 0x040017FC RID: 6140
		private static float lastFinaleMessage;

		// Token: 0x040017FD RID: 6141
		private static float lastTimerMessage;

		// Token: 0x040017FE RID: 6142
		private static float nextAreaModify;

		// Token: 0x040017FF RID: 6143
		private static int countTimerMessages;

		// Token: 0x04001800 RID: 6144
		public static EArenaState arenaState;

		// Token: 0x04001801 RID: 6145
		public static EArenaMessage arenaMessage;

		// Token: 0x04001802 RID: 6146
		private static int nonGroups;

		// Token: 0x04001803 RID: 6147
		public static List<CSteamID> arenaGroups;

		// Token: 0x04001804 RID: 6148
		public static List<ArenaPlayer> arenaPlayers;

		// Token: 0x04001805 RID: 6149
		private static Vector3 _arenaCurrentCenter;

		// Token: 0x04001806 RID: 6150
		private static Vector3 _arenaOriginCenter;

		// Token: 0x04001807 RID: 6151
		private static Vector3 _arenaTargetCenter;

		// Token: 0x04001808 RID: 6152
		private static float _arenaCurrentRadius;

		// Token: 0x04001809 RID: 6153
		private static float _arenaOriginRadius;

		// Token: 0x0400180A RID: 6154
		private static float _arenaTargetRadius;

		// Token: 0x0400180B RID: 6155
		private static float _arenaCompactorSpeed;

		// Token: 0x0400180C RID: 6156
		private static float arenaSqrRadius;

		// Token: 0x0400180D RID: 6157
		private static Transform arenaCurrentArea;

		// Token: 0x0400180E RID: 6158
		private static Transform arenaTargetArea;

		// Token: 0x0400180F RID: 6159
		public static ArenaMessageUpdated onArenaMessageUpdated;

		// Token: 0x04001810 RID: 6160
		public static ArenaPlayerUpdated onArenaPlayerUpdated;

		// Token: 0x04001811 RID: 6161
		public static LevelNumberUpdated onLevelNumberUpdated;

		// Token: 0x04001812 RID: 6162
		private static readonly ClientStaticMethod<Vector3, float, Vector3, float, Vector3, float, float, byte> SendArenaOrigin = ClientStaticMethod<Vector3, float, Vector3, float, Vector3, float, float, byte>.Get(new ClientStaticMethod<Vector3, float, Vector3, float, Vector3, float, float, byte>.ReceiveDelegate(LevelManager.ReceiveArenaOrigin));

		// Token: 0x04001813 RID: 6163
		private static readonly ClientStaticMethod<EArenaMessage> SendArenaMessage = ClientStaticMethod<EArenaMessage>.Get(new ClientStaticMethod<EArenaMessage>.ReceiveDelegate(LevelManager.ReceiveArenaMessage));

		// Token: 0x04001814 RID: 6164
		private static readonly ClientStaticMethod SendArenaPlayer = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(LevelManager.ReceiveArenaPlayer));

		// Token: 0x04001815 RID: 6165
		private static readonly ClientStaticMethod<byte> SendLevelNumber = ClientStaticMethod<byte>.Get(new ClientStaticMethod<byte>.ReceiveDelegate(LevelManager.ReceiveLevelNumber));

		// Token: 0x04001816 RID: 6166
		private static readonly ClientStaticMethod<byte> SendLevelTimer = ClientStaticMethod<byte>.Get(new ClientStaticMethod<byte>.ReceiveDelegate(LevelManager.ReceiveLevelTimer));

		// Token: 0x04001817 RID: 6167
		private static List<AirdropDevkitNode> airdropNodes;

		// Token: 0x04001818 RID: 6168
		private static List<AirdropInfo> airdrops;

		// Token: 0x04001819 RID: 6169
		public static uint airdropFrequency;

		// Token: 0x0400181A RID: 6170
		private static bool _hasAirdrop;

		// Token: 0x0400181B RID: 6171
		private static float lastAirdrop;

		// Token: 0x0400181C RID: 6172
		private static readonly ClientStaticMethod<Vector3, Vector3, float, float, float> SendAirdropState = ClientStaticMethod<Vector3, Vector3, float, float, float>.Get(new ClientStaticMethod<Vector3, Vector3, float, float, float>.ReceiveDelegate(LevelManager.ReceiveAirdropState));
	}
}
