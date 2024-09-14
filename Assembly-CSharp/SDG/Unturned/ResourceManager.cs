using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200058B RID: 1419
	public class ResourceManager : SteamCaller
	{
		// Token: 0x06002D5A RID: 11610 RVA: 0x000C53FD File Offset: 0x000C35FD
		[Obsolete]
		public void tellClearRegionResources(CSteamID steamID, byte x, byte y)
		{
			ResourceManager.ReceiveClearRegionResources(x, y);
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000C5408 File Offset: 0x000C3608
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellClearRegionResources")]
		public static void ReceiveClearRegionResources(byte x, byte y)
		{
			if (!Provider.isServer && !ResourceManager.regions[(int)x, (int)y].isNetworked)
			{
				return;
			}
			for (int i = 0; i < LevelGround.trees[(int)x, (int)y].Count; i++)
			{
				LevelGround.trees[(int)x, (int)y][i].revive();
			}
		}

		/// <summary>
		/// Revive all trees in a specific region.
		/// </summary>
		// Token: 0x06002D5C RID: 11612 RVA: 0x000C5463 File Offset: 0x000C3663
		public static void askClearRegionResources(byte x, byte y)
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (!Regions.checkSafe((int)x, (int)y))
			{
				return;
			}
			if (LevelGround.trees[(int)x, (int)y].Count > 0)
			{
				ResourceManager.SendClearRegionResources.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), x, y);
			}
		}

		/// <summary>
		/// Revive trees worldwide. Used between arena rounds.
		/// </summary>
		// Token: 0x06002D5D RID: 11613 RVA: 0x000C54A0 File Offset: 0x000C36A0
		public static void askClearAllResources()
		{
			if (!Provider.isServer)
			{
				return;
			}
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					ResourceManager.askClearRegionResources(b, b2);
				}
			}
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000C54E0 File Offset: 0x000C36E0
		public static void getResourcesInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search, List<Transform> result)
		{
			if (ResourceManager.regions == null)
			{
				return;
			}
			for (int i = 0; i < search.Count; i++)
			{
				RegionCoordinate regionCoordinate = search[i];
				if (ResourceManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y] != null)
				{
					for (int j = 0; j < LevelGround.trees[(int)regionCoordinate.x, (int)regionCoordinate.y].Count; j++)
					{
						ResourceSpawnpoint resourceSpawnpoint = LevelGround.trees[(int)regionCoordinate.x, (int)regionCoordinate.y][j];
						if (!(resourceSpawnpoint.model == null) && !resourceSpawnpoint.isDead && (resourceSpawnpoint.point - center).sqrMagnitude < sqrRadius)
						{
							result.Add(resourceSpawnpoint.model);
						}
					}
				}
			}
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x000C55AC File Offset: 0x000C37AC
		public static void damage(Transform resource, Vector3 direction, float damage, float times, float drop, out EPlayerKill kill, out uint xp, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown, bool trackKill = true)
		{
			xp = 0U;
			kill = EPlayerKill.NONE;
			ushort num = (ushort)(damage * times);
			bool flag = true;
			DamageResourceRequestHandler damageResourceRequestHandler = ResourceManager.onDamageResourceRequested;
			if (damageResourceRequestHandler != null)
			{
				damageResourceRequestHandler(instigatorSteamID, resource, ref num, ref flag, damageOrigin);
			}
			if (!flag || num < 1)
			{
				return;
			}
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(resource.position, out b, out b2))
			{
				List<ResourceSpawnpoint> list = LevelGround.trees[(int)b, (int)b2];
				ushort num2 = 0;
				while ((int)num2 < list.Count)
				{
					if (resource == list[(int)num2].model)
					{
						if (list[(int)num2].isDead || !list[(int)num2].canBeDamaged)
						{
							break;
						}
						list[(int)num2].askDamage(num);
						if (list[(int)num2].isDead)
						{
							kill = EPlayerKill.RESOURCE;
							ResourceAsset asset = list[(int)num2].asset;
							if (list[(int)num2].asset != null)
							{
								EffectAsset effectAsset = asset.FindExplosionEffectAsset();
								if (effectAsset != null)
								{
									EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
									{
										position = list[(int)num2].GetEffectSpawnPosition(),
										relevantDistance = EffectManager.MEDIUM
									});
								}
								if (!asset.isForage)
								{
									float num3 = Provider.modeConfigData.Objects.Resource_Drops_Multiplier;
									num3 *= drop;
									if (asset.rewardID != 0)
									{
										direction.y = 0f;
										direction.Normalize();
										int num4 = Mathf.CeilToInt((float)Random.Range((int)asset.rewardMin, (int)(asset.rewardMax + 1)) * num3);
										num4 = Mathf.Clamp(num4, 0, 100);
										for (int i = 0; i < num4; i++)
										{
											ushort num5 = SpawnTableTool.ResolveLegacyId(asset.rewardID, EAssetType.ITEM, new Func<string>(asset.OnGetRewardSpawnTableErrorContext));
											if (num5 != 0)
											{
												if (asset.hasDebris)
												{
													ItemManager.dropItem(new Item(num5, EItemOrigin.NATURE), resource.position + direction * (float)(2 + i) + new Vector3(0f, 2f, 0f), false, true, true);
												}
												else
												{
													ItemManager.dropItem(new Item(num5, EItemOrigin.NATURE), resource.position + new Vector3(Random.Range(-2f, 2f), 2f, Random.Range(-2f, 2f)), false, true, true);
												}
											}
										}
									}
									else
									{
										if (asset.log != 0)
										{
											int num6 = Mathf.CeilToInt((float)Random.Range(3, 7) * num3);
											num6 = Mathf.Clamp(num6, 0, 100);
											for (int j = 0; j < num6; j++)
											{
												ItemManager.dropItem(new Item(asset.log, EItemOrigin.NATURE), resource.position + direction * (float)(2 + j * 2) + Vector3.up, false, true, true);
											}
										}
										if (asset.stick != 0)
										{
											int num7 = Mathf.CeilToInt((float)Random.Range(2, 5) * num3);
											num7 = Mathf.Clamp(num7, 0, 100);
											for (int k = 0; k < num7; k++)
											{
												float f = Random.Range(0f, 6.2831855f);
												ItemManager.dropItem(new Item(asset.stick, EItemOrigin.NATURE), resource.position + new Vector3(Mathf.Sin(f) * 3f, 1f, Mathf.Cos(f) * 3f), false, true, true);
											}
										}
									}
									xp = asset.rewardXP;
									Vector3 point = list[(int)num2].point;
									Guid guid = asset.GUID;
									for (int l = 0; l < Provider.clients.Count; l++)
									{
										SteamPlayer steamPlayer = Provider.clients[l];
										if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead && (steamPlayer.player.transform.position - point).sqrMagnitude < 90000f)
										{
											steamPlayer.player.quests.trackTreeKill(guid);
										}
									}
								}
							}
							ResourceManager.ServerSetResourceDead(b, b2, num2, direction * (float)num);
							return;
						}
						break;
					}
					else
					{
						num2 += 1;
					}
				}
			}
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x000C5A10 File Offset: 0x000C3C10
		public static void forage(Transform resource)
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(resource.position, out b, out b2))
			{
				List<ResourceSpawnpoint> list = LevelGround.trees[(int)b, (int)b2];
				ushort num = 0;
				while ((int)num < list.Count)
				{
					if (resource == list[(int)num].model)
					{
						ResourceManager.SendForageRequest.Invoke(ENetReliability.Unreliable, b, b2, num);
						return;
					}
					num += 1;
				}
			}
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x000C5A70 File Offset: 0x000C3C70
		[Obsolete]
		public void askForage(CSteamID steamID, byte x, byte y, ushort index)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			ResourceManager.ReceiveForageRequest(serverInvocationContext, x, y, index);
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x000C5A90 File Offset: 0x000C3C90
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 10, legacyName = "askForage")]
		public static void ReceiveForageRequest(in ServerInvocationContext context, byte x, byte y, ushort index)
		{
			if (!Regions.checkSafe((int)x, (int)y))
			{
				return;
			}
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			List<ResourceSpawnpoint> list = LevelGround.trees[(int)x, (int)y];
			if ((int)index >= list.Count)
			{
				return;
			}
			if (list[(int)index].isDead)
			{
				return;
			}
			if ((list[(int)index].point - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			ResourceAsset asset = list[(int)index].asset;
			if (asset == null || !asset.isForage)
			{
				return;
			}
			list[(int)index].askDamage(1);
			EffectAsset effectAsset = asset.FindExplosionEffectAsset();
			if (effectAsset != null)
			{
				EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
				{
					position = list[(int)index].GetEffectSpawnPosition(),
					relevantDistance = EffectManager.MEDIUM
				});
			}
			ushort num;
			if (asset.rewardID != 0)
			{
				num = SpawnTableTool.ResolveLegacyId(asset.rewardID, EAssetType.ITEM, new Func<string>(asset.OnGetRewardSpawnTableErrorContext));
			}
			else
			{
				num = asset.log;
			}
			if (num != 0)
			{
				player.inventory.forceAddItem(new Item(num, EItemOrigin.NATURE), true);
				if (Random.value < player.skills.mastery(2, 5))
				{
					player.inventory.forceAddItem(new Item(num, EItemOrigin.NATURE), true);
				}
			}
			player.sendStat(EPlayerStat.FOUND_PLANTS);
			player.skills.askPay(asset.forageRewardExperience);
			ResourceManager.ServerSetResourceDead(x, y, index, Vector3.zero);
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x000C5C0C File Offset: 0x000C3E0C
		[Obsolete]
		public void tellResourceDead(CSteamID steamID, byte x, byte y, ushort index, Vector3 ragdoll)
		{
			ResourceManager.ReceiveResourceDead(x, y, index, ragdoll);
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x000C5C1C File Offset: 0x000C3E1C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellResourceDead")]
		public static void ReceiveResourceDead(byte x, byte y, ushort index, Vector3 ragdoll)
		{
			if ((int)index >= LevelGround.trees[(int)x, (int)y].Count)
			{
				return;
			}
			if (!Provider.isServer && !ResourceManager.regions[(int)x, (int)y].isNetworked)
			{
				return;
			}
			LevelGround.trees[(int)x, (int)y][(int)index].kill(ragdoll);
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x000C5C71 File Offset: 0x000C3E71
		[Obsolete]
		public void tellResourceAlive(CSteamID steamID, byte x, byte y, ushort index)
		{
			ResourceManager.ReceiveResourceAlive(x, y, index);
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x000C5C7C File Offset: 0x000C3E7C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellResourceAlive")]
		public static void ReceiveResourceAlive(byte x, byte y, ushort index)
		{
			if ((int)index >= LevelGround.trees[(int)x, (int)y].Count)
			{
				return;
			}
			if (!Provider.isServer && !ResourceManager.regions[(int)x, (int)y].isNetworked)
			{
				return;
			}
			LevelGround.trees[(int)x, (int)y][(int)index].revive();
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x000C5CD0 File Offset: 0x000C3ED0
		[Obsolete]
		public void tellResources(CSteamID steamID, byte x, byte y, bool[] resources)
		{
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x000C5CD4 File Offset: 0x000C3ED4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveResources(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte b2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b2);
			if (!Regions.checkSafe((int)b, (int)b2))
			{
				return;
			}
			if (ResourceManager.regions[(int)b, (int)b2].isNetworked)
			{
				return;
			}
			ResourceManager.regions[(int)b, (int)b2].isNetworked = true;
			List<ResourceSpawnpoint> list = LevelGround.trees[(int)b, (int)b2];
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num);
			num = MathfEx.Min(num, (ushort)list.Count);
			ushort num2 = 0;
			bool flag;
			while (num2 < num && reader.ReadBit(ref flag))
			{
				if (flag)
				{
					list[(int)num2].wipe();
				}
				else
				{
					list[(int)num2].revive();
				}
				num2 += 1;
			}
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x000C5D8C File Offset: 0x000C3F8C
		internal void SendRegion(SteamPlayer client, byte x, byte y)
		{
			List<ResourceSpawnpoint> regionTrees = LevelGround.trees[(int)x, (int)y];
			ResourceManager.SendResources.Invoke(ENetReliability.Reliable, client.transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt8(writer, x);
				SystemNetPakWriterEx.WriteUInt8(writer, y);
				ushort num = (ushort)regionTrees.Count;
				SystemNetPakWriterEx.WriteUInt16(writer, num);
				for (ushort num2 = 0; num2 < num; num2 += 1)
				{
					writer.WriteBit(regionTrees[(int)num2].isDead);
				}
			});
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000C5DE8 File Offset: 0x000C3FE8
		public static ResourceSpawnpoint getResourceSpawnpoint(byte x, byte y, ushort index)
		{
			if (!Regions.checkSafe((int)x, (int)y))
			{
				return null;
			}
			List<ResourceSpawnpoint> list = LevelGround.trees[(int)x, (int)y];
			if ((int)index >= list.Count)
			{
				return null;
			}
			return list[(int)index];
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000C5E20 File Offset: 0x000C4020
		public static Transform getResource(byte x, byte y, ushort index)
		{
			ResourceSpawnpoint resourceSpawnpoint = ResourceManager.getResourceSpawnpoint(x, y, index);
			if (resourceSpawnpoint == null)
			{
				return null;
			}
			if (resourceSpawnpoint.model != null)
			{
				return resourceSpawnpoint.model;
			}
			return resourceSpawnpoint.stump;
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000C5E58 File Offset: 0x000C4058
		public static bool tryGetRegion(Transform resource, out byte x, out byte y, out ushort index)
		{
			x = 0;
			y = 0;
			index = 0;
			if (Regions.tryGetCoordinate(resource.position, out x, out y))
			{
				List<ResourceSpawnpoint> list = LevelGround.trees[(int)x, (int)y];
				index = 0;
				while ((int)index < list.Count)
				{
					if (resource == list[(int)index].model || resource == list[(int)index].stump)
					{
						return true;
					}
					index += 1;
				}
			}
			return false;
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000C5ED0 File Offset: 0x000C40D0
		private bool overlapTreeColliders(ResourceSpawnpoint tree, int layerMask)
		{
			this.treeColliders.Clear();
			if (tree.model == null)
			{
				return false;
			}
			tree.model.GetComponentsInChildren<Collider>(true, this.treeColliders);
			foreach (Collider collider in this.treeColliders)
			{
				BoxCollider boxCollider = collider as BoxCollider;
				if (boxCollider != null)
				{
					if (boxCollider.OverlapBoxSingle(layerMask, QueryTriggerInteraction.Collide) != null)
					{
						return true;
					}
				}
				else
				{
					SphereCollider sphereCollider = collider as SphereCollider;
					if (sphereCollider != null)
					{
						if (sphereCollider.OverlapSphereSingle(layerMask, QueryTriggerInteraction.Collide) != null)
						{
							return true;
						}
					}
					else
					{
						CapsuleCollider capsuleCollider = collider as CapsuleCollider;
						if (capsuleCollider != null && capsuleCollider.OverlapCapsuleSingle(layerMask, QueryTriggerInteraction.Collide) != null)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000C5FAC File Offset: 0x000C41AC
		public static void ServerSetResourceAlive(byte x, byte y, ushort index)
		{
			ResourceManager.SendResourceAlive.InvokeAndLoopback(ENetReliability.Reliable, ResourceManager.GatherRemoteClients(x, y), x, y, index);
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000C5FC3 File Offset: 0x000C41C3
		public static void ServerSetResourceDead(byte x, byte y, ushort index, Vector3 baseForce)
		{
			ResourceManager.SendResourceDead.InvokeAndLoopback(ENetReliability.Reliable, ResourceManager.GatherRemoteClients(x, y), x, y, index, baseForce);
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000C5FDC File Offset: 0x000C41DC
		private bool respawnResources()
		{
			if (LevelGround.trees[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].Count > 0)
			{
				if ((int)ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex >= LevelGround.trees[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].Count)
				{
					ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex = (ushort)(LevelGround.trees[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].Count - 1);
				}
				ResourceSpawnpoint resourceSpawnpoint = LevelGround.trees[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y][(int)ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex];
				if (resourceSpawnpoint.checkCanReset(Provider.modeConfigData.Objects.Resource_Reset_Multiplier))
				{
					int num = 1536;
					if (Provider.modeConfigData.Objects.Items_Obstruct_Tree_Respawns)
					{
						num |= 134217728;
					}
					if (!this.overlapTreeColliders(resourceSpawnpoint, num))
					{
						ResourceManager.ServerSetResourceAlive(ResourceManager.respawnResources_X, ResourceManager.respawnResources_Y, ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex);
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x000C6110 File Offset: 0x000C4310
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP)
			{
				ResourceManager.regions = new ResourceRegion[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						ResourceManager.regions[(int)b, (int)b2] = new ResourceRegion();
					}
				}
				ResourceManager.respawnResources_X = 0;
				ResourceManager.respawnResources_Y = 0;
			}
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000C6178 File Offset: 0x000C4378
		private void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y, byte step, ref bool canIncrementIndex)
		{
			if (step == 0)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						if (Provider.isServer)
						{
							if (player.movement.loadedRegions[(int)b, (int)b2].isResourcesLoaded && !Regions.checkArea(b, b2, new_x, new_y, ResourceManager.RESOURCE_REGIONS))
							{
								player.movement.loadedRegions[(int)b, (int)b2].isResourcesLoaded = false;
							}
						}
						else if (player.channel.IsLocalPlayer && ResourceManager.regions[(int)b, (int)b2].isNetworked && !Regions.checkArea(b, b2, new_x, new_y, ResourceManager.RESOURCE_REGIONS))
						{
							ResourceManager.regions[(int)b, (int)b2].isNetworked = false;
						}
					}
				}
			}
			if (step == 3 && Regions.checkSafe((int)new_x, (int)new_y))
			{
				for (int i = (int)(new_x - ResourceManager.RESOURCE_REGIONS); i <= (int)(new_x + ResourceManager.RESOURCE_REGIONS); i++)
				{
					for (int j = (int)(new_y - ResourceManager.RESOURCE_REGIONS); j <= (int)(new_y + ResourceManager.RESOURCE_REGIONS); j++)
					{
						if (Regions.checkSafe((int)((byte)i), (int)((byte)j)) && !player.movement.loadedRegions[i, j].isResourcesLoaded)
						{
							player.movement.loadedRegions[i, j].isResourcesLoaded = true;
							this.SendRegion(player.channel.owner, (byte)i, (byte)j);
						}
					}
				}
			}
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x000C62E5 File Offset: 0x000C44E5
		private void onPlayerCreated(Player player)
		{
			PlayerMovement movement = player.movement;
			movement.onRegionUpdated = (PlayerRegionUpdated)Delegate.Combine(movement.onRegionUpdated, new PlayerRegionUpdated(this.onRegionUpdated));
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x000C6310 File Offset: 0x000C4510
		private void Update()
		{
			if (!Provider.isServer || !Level.isLoaded)
			{
				return;
			}
			bool flag = true;
			while (flag)
			{
				flag = this.respawnResources();
				ResourceRegion resourceRegion = ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y];
				resourceRegion.respawnResourceIndex += 1;
				if ((int)ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex >= LevelGround.trees[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].Count)
				{
					ResourceManager.regions[(int)ResourceManager.respawnResources_X, (int)ResourceManager.respawnResources_Y].respawnResourceIndex = 0;
				}
				ResourceManager.respawnResources_X += 1;
				if (ResourceManager.respawnResources_X >= Regions.WORLD_SIZE)
				{
					ResourceManager.respawnResources_X = 0;
					ResourceManager.respawnResources_Y += 1;
					if (ResourceManager.respawnResources_Y >= Regions.WORLD_SIZE)
					{
						ResourceManager.respawnResources_Y = 0;
						flag = false;
					}
				}
			}
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000C63F0 File Offset: 0x000C45F0
		private void Start()
		{
			ResourceManager.manager = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000C6443 File Offset: 0x000C4643
		private static PooledTransportConnectionList GatherRemoteClients(byte x, byte y)
		{
			return Regions.GatherRemoteClientConnections(x, y, ResourceManager.RESOURCE_REGIONS);
		}

		// Token: 0x0400187A RID: 6266
		public static readonly byte RESOURCE_REGIONS = 2;

		// Token: 0x0400187B RID: 6267
		public static DamageResourceRequestHandler onDamageResourceRequested;

		// Token: 0x0400187C RID: 6268
		private static ResourceManager manager;

		// Token: 0x0400187D RID: 6269
		private static ResourceRegion[,] regions;

		// Token: 0x0400187E RID: 6270
		private static byte respawnResources_X;

		// Token: 0x0400187F RID: 6271
		private static byte respawnResources_Y;

		// Token: 0x04001880 RID: 6272
		private static readonly ClientStaticMethod<byte, byte> SendClearRegionResources = ClientStaticMethod<byte, byte>.Get(new ClientStaticMethod<byte, byte>.ReceiveDelegate(ResourceManager.ReceiveClearRegionResources));

		// Token: 0x04001881 RID: 6273
		private static readonly ServerStaticMethod<byte, byte, ushort> SendForageRequest = ServerStaticMethod<byte, byte, ushort>.Get(new ServerStaticMethod<byte, byte, ushort>.ReceiveDelegateWithContext(ResourceManager.ReceiveForageRequest));

		// Token: 0x04001882 RID: 6274
		private static readonly ClientStaticMethod<byte, byte, ushort, Vector3> SendResourceDead = ClientStaticMethod<byte, byte, ushort, Vector3>.Get(new ClientStaticMethod<byte, byte, ushort, Vector3>.ReceiveDelegate(ResourceManager.ReceiveResourceDead));

		// Token: 0x04001883 RID: 6275
		private static readonly ClientStaticMethod<byte, byte, ushort> SendResourceAlive = ClientStaticMethod<byte, byte, ushort>.Get(new ClientStaticMethod<byte, byte, ushort>.ReceiveDelegate(ResourceManager.ReceiveResourceAlive));

		// Token: 0x04001884 RID: 6276
		private static readonly ClientStaticMethod SendResources = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(ResourceManager.ReceiveResources));

		// Token: 0x04001885 RID: 6277
		private List<Collider> treeColliders = new List<Collider>();
	}
}
