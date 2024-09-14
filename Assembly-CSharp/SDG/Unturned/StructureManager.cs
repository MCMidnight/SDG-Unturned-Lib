using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200059C RID: 1436
	public class StructureManager : SteamCaller
	{
		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06002DC3 RID: 11715 RVA: 0x000C6F53 File Offset: 0x000C5153
		public static StructureManager instance
		{
			get
			{
				return StructureManager.manager;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002DC4 RID: 11716 RVA: 0x000C6F5A File Offset: 0x000C515A
		// (set) Token: 0x06002DC5 RID: 11717 RVA: 0x000C6F61 File Offset: 0x000C5161
		public static StructureRegion[,] regions { get; private set; }

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000C6F6C File Offset: 0x000C516C
		public static void getStructuresInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search, List<Transform> result)
		{
			if (StructureManager.regions == null)
			{
				return;
			}
			for (int i = 0; i < search.Count; i++)
			{
				RegionCoordinate regionCoordinate = search[i];
				if (StructureManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y] != null)
				{
					foreach (StructureDrop structureDrop in StructureManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].drops)
					{
						if ((structureDrop.model.position - center).sqrMagnitude < sqrRadius)
						{
							result.Add(structureDrop.model);
						}
					}
				}
			}
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000C7038 File Offset: 0x000C5238
		[Obsolete]
		public void tellStructureOwnerAndGroup(CSteamID steamID, byte x, byte y, ushort index, ulong newOwner, ulong newGroup)
		{
			throw new NotSupportedException("Moved into instance method as part of structure NetId rewrite");
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000C7044 File Offset: 0x000C5244
		public static void changeOwnerAndGroup(Transform transform, ulong newOwner, ulong newGroup)
		{
			byte x;
			byte y;
			StructureRegion structureRegion;
			if (!StructureManager.tryGetRegion(transform, out x, out y, out structureRegion))
			{
				return;
			}
			StructureDrop structureDrop = structureRegion.FindStructureByRootTransform(transform);
			if (structureDrop == null)
			{
				return;
			}
			StructureDrop.SendOwnerAndGroup.InvokeAndLoopback(structureDrop.GetNetId(), ENetReliability.Reliable, StructureManager.GatherRemoteClientConnections(x, y), newOwner, newGroup);
			structureDrop.serversideData.owner = newOwner;
			structureDrop.serversideData.group = newGroup;
			StructureManager.sendHealthChanged(x, y, structureDrop);
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x000C70A8 File Offset: 0x000C52A8
		public static void transformStructure(Transform transform, Vector3 point, Quaternion rotation)
		{
			StructureDrop structureDrop = StructureDrop.FindByRootFast(transform);
			if (structureDrop == null)
			{
				return;
			}
			StructureDrop.SendTransformRequest.Invoke(structureDrop.GetNetId(), ENetReliability.Reliable, point, rotation);
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x000C70D3 File Offset: 0x000C52D3
		[Obsolete]
		public void tellTransformStructure(CSteamID steamID, byte x, byte y, uint instanceID, Vector3 point, byte angle_x, byte angle_y, byte angle_z)
		{
			throw new NotSupportedException("Moved into instance method as part of structure NetId rewrite");
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000C70E0 File Offset: 0x000C52E0
		public static bool ServerSetStructureTransform(Transform transform, Vector3 position, Quaternion rotation)
		{
			byte x;
			byte y;
			StructureRegion structureRegion;
			if (!StructureManager.tryGetRegion(transform, out x, out y, out structureRegion))
			{
				return false;
			}
			StructureDrop structureDrop = structureRegion.FindStructureByRootTransform(transform);
			if (structureDrop == null)
			{
				return false;
			}
			StructureManager.InternalSetStructureTransform(x, y, structureDrop, position, rotation);
			return true;
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000C7115 File Offset: 0x000C5315
		internal static void InternalSetStructureTransform(byte x, byte y, StructureDrop drop, Vector3 point, Quaternion rotation)
		{
			StructureDrop.SendTransform.InvokeAndLoopback(drop.GetNetId(), ENetReliability.Reliable, StructureManager.GatherRemoteClientConnections(x, y), x, y, point, rotation);
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000C7134 File Offset: 0x000C5334
		[Obsolete]
		public void askTransformStructure(CSteamID steamID, byte x, byte y, uint instanceID, Vector3 point, byte angle_x, byte angle_y, byte angle_z)
		{
			throw new NotSupportedException("Moved into instance method as part of structure NetId rewrite");
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x000C7140 File Offset: 0x000C5340
		[Obsolete]
		public void tellStructureHealth(CSteamID steamID, byte x, byte y, ushort index, byte hp)
		{
			throw new NotSupportedException("Moved into instance method as part of structure NetId rewrite");
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x000C714C File Offset: 0x000C534C
		public static void salvageStructure(Transform transform)
		{
			StructureDrop structureDrop = StructureManager.FindStructureByRootTransform(transform);
			if (structureDrop != null)
			{
				StructureDrop.SendSalvageRequest.Invoke(structureDrop.GetNetId(), ENetReliability.Reliable);
			}
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000C7174 File Offset: 0x000C5374
		[Obsolete]
		public void askSalvageStructure(CSteamID steamID, byte x, byte y, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of structure NetId rewrite");
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000C7180 File Offset: 0x000C5380
		public static void damage(Transform transform, Vector3 direction, float damage, float times, bool armor, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown)
		{
			byte x;
			byte y;
			StructureRegion structureRegion;
			if (!StructureManager.tryGetRegion(transform, out x, out y, out structureRegion))
			{
				return;
			}
			StructureDrop structureDrop = structureRegion.FindStructureByRootTransform(transform);
			if (structureDrop == null)
			{
				return;
			}
			if (structureDrop.serversideData.structure.isDead)
			{
				return;
			}
			ItemStructureAsset asset = structureDrop.asset;
			if (asset == null)
			{
				return;
			}
			if (!asset.canBeDamaged)
			{
				return;
			}
			if (armor)
			{
				times *= Provider.modeConfigData.Structures.getArmorMultiplier(asset.armorTier);
			}
			ushort num = (ushort)(damage * times);
			bool flag = true;
			DamageStructureRequestHandler damageStructureRequestHandler = StructureManager.onDamageStructureRequested;
			if (damageStructureRequestHandler != null)
			{
				damageStructureRequestHandler(instigatorSteamID, transform, ref num, ref flag, damageOrigin);
			}
			if (!flag || num < 1)
			{
				return;
			}
			structureDrop.serversideData.structure.askDamage(num);
			if (structureDrop.serversideData.structure.isDead)
			{
				EffectAsset effectAsset = asset.FindExplosionEffectAsset();
				if (effectAsset != null)
				{
					EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
					{
						position = transform.position + Vector3.down * StructureManager.HEIGHT,
						relevantDistance = EffectManager.MEDIUM
					});
				}
				asset.SpawnItemDropsOnDestroy(transform.position);
				StructureManager.destroyStructure(structureDrop, x, y, direction * (float)num, false);
				return;
			}
			StructureManager.sendHealthChanged(x, y, structureDrop);
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000C72B2 File Offset: 0x000C54B2
		[Obsolete("Please replace the methods which take an index")]
		public static void destroyStructure(StructureRegion region, byte x, byte y, ushort index, Vector3 ragdoll)
		{
			StructureManager.destroyStructure(region.drops[(int)index], x, y, ragdoll, false);
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x000C72CA File Offset: 0x000C54CA
		public static void destroyStructure(StructureDrop structure, byte x, byte y, Vector3 ragdoll)
		{
			StructureManager.destroyStructure(structure, x, y, ragdoll, false);
		}

		/// <summary>
		/// Remove structure instance on server and client.
		/// </summary>
		// Token: 0x06002DD4 RID: 11732 RVA: 0x000C72D8 File Offset: 0x000C54D8
		public static void destroyStructure(StructureDrop structure, byte x, byte y, Vector3 ragdoll, bool wasPickedUp)
		{
			StructureRegion structureRegion;
			if (StructureManager.tryGetRegion(x, y, out structureRegion))
			{
				structureRegion.structures.Remove(structure.serversideData);
				StructureManager.SendDestroyStructure.InvokeAndLoopback(ENetReliability.Reliable, StructureManager.GatherRemoteClientConnections(x, y), structure.GetNetId(), ragdoll, wasPickedUp);
			}
		}

		/// <summary>
		/// Used by ownership change and damaged event to tell relevant clients the new health.
		/// </summary>
		// Token: 0x06002DD5 RID: 11733 RVA: 0x000C7320 File Offset: 0x000C5520
		private static void sendHealthChanged(byte x, byte y, StructureDrop structure)
		{
			StructureDrop.SendHealth.Invoke(structure.GetNetId(), ENetReliability.Unreliable, Provider.GatherClientConnectionsMatchingPredicate((SteamPlayer client) => client.player != null && OwnershipTool.checkToggle(client.playerID.steamID, structure.serversideData.owner, client.player.quests.groupID, structure.serversideData.group) && Regions.checkArea(x, y, client.player.movement.region_x, client.player.movement.region_y, StructureManager.STRUCTURE_REGIONS)), (byte)Mathf.RoundToInt((float)structure.serversideData.structure.health / (float)structure.asset.health * 100f));
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x000C73A4 File Offset: 0x000C55A4
		public static void repair(Transform structure, float damage, float times)
		{
			StructureManager.repair(structure, damage, times, default(CSteamID));
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000C73C4 File Offset: 0x000C55C4
		public static void repair(Transform transform, float damage, float times, CSteamID instigatorSteamID = default(CSteamID))
		{
			byte x;
			byte y;
			StructureRegion structureRegion;
			if (!StructureManager.tryGetRegion(transform, out x, out y, out structureRegion))
			{
				return;
			}
			StructureDrop structureDrop = structureRegion.FindStructureByRootTransform(transform);
			if (structureDrop == null)
			{
				return;
			}
			if (!structureDrop.serversideData.structure.isDead && !structureDrop.serversideData.structure.isRepaired)
			{
				float value = damage * times;
				bool flag = true;
				RepairStructureRequestHandler onRepairRequested = StructureManager.OnRepairRequested;
				if (onRepairRequested != null)
				{
					onRepairRequested(instigatorSteamID, transform, ref value, ref flag);
				}
				ushort num = MathfEx.RoundAndClampToUShort(value);
				if (!flag || num < 1)
				{
					return;
				}
				structureDrop.serversideData.structure.askRepair(num);
				StructureManager.sendHealthChanged(x, y, structureDrop);
				RepairedStructureHandler onRepaired = StructureManager.OnRepaired;
				if (onRepaired == null)
				{
					return;
				}
				onRepaired(instigatorSteamID, transform, (float)num);
			}
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x000C7470 File Offset: 0x000C5670
		public static StructureDrop FindStructureByRootTransform(Transform transform)
		{
			byte b;
			byte b2;
			StructureRegion structureRegion;
			if (StructureManager.tryGetRegion(transform, out b, out b2, out structureRegion))
			{
				return structureRegion.FindStructureByRootTransform(transform);
			}
			return null;
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000C7494 File Offset: 0x000C5694
		[Obsolete("Please use FindStructureByRootTransform instead")]
		public static bool tryGetInfo(Transform structure, out byte x, out byte y, out ushort index, out StructureRegion region)
		{
			x = 0;
			y = 0;
			index = 0;
			region = null;
			if (StructureManager.tryGetRegion(structure, out x, out y, out region))
			{
				index = 0;
				while ((int)index < region.drops.Count)
				{
					if (structure == region.drops[(int)index].model)
					{
						return true;
					}
					index += 1;
				}
			}
			return false;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000C74F8 File Offset: 0x000C56F8
		[Obsolete("Please use FindStructureByRootTransform instead")]
		public static bool tryGetInfo(Transform structure, out byte x, out byte y, out ushort index, out StructureRegion region, out StructureDrop drop)
		{
			x = 0;
			y = 0;
			index = 0;
			region = null;
			drop = null;
			if (StructureManager.tryGetRegion(structure, out x, out y, out region))
			{
				index = 0;
				while ((int)index < region.drops.Count)
				{
					if (structure == region.drops[(int)index].model)
					{
						drop = region.drops[(int)index];
						return true;
					}
					index += 1;
				}
			}
			return false;
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000C7570 File Offset: 0x000C5770
		public static bool tryGetRegion(Transform structure, out byte x, out byte y, out StructureRegion region)
		{
			x = 0;
			y = 0;
			region = null;
			if (structure == null)
			{
				return false;
			}
			if (Regions.tryGetCoordinate(structure.position, out x, out y))
			{
				region = StructureManager.regions[(int)x, (int)y];
				return true;
			}
			return false;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x000C75A8 File Offset: 0x000C57A8
		public static bool tryGetRegion(byte x, byte y, out StructureRegion region)
		{
			region = null;
			if (Regions.checkSafe((int)x, (int)y))
			{
				region = StructureManager.regions[(int)x, (int)y];
				return true;
			}
			return false;
		}

		/// <summary>
		/// Legacy function for UseableStructure.
		/// </summary>
		// Token: 0x06002DDD RID: 11741 RVA: 0x000C75C8 File Offset: 0x000C57C8
		public static bool dropStructure(Structure structure, Vector3 point, float angle_x, float angle_y, float angle_z, ulong owner, ulong group)
		{
			if (structure.asset == null)
			{
				return false;
			}
			bool flag = true;
			DeployStructureRequestHandler deployStructureRequestHandler = StructureManager.onDeployStructureRequested;
			if (deployStructureRequestHandler != null)
			{
				deployStructureRequestHandler(structure, structure.asset, ref point, ref angle_x, ref angle_y, ref angle_z, ref owner, ref group, ref flag);
			}
			if (!flag)
			{
				return false;
			}
			Quaternion rotation = Quaternion.Euler(-90f, angle_y, 0f);
			return StructureManager.dropReplicatedStructure(structure, point, rotation, owner, group);
		}

		/// <summary>
		/// Spawn a new structure and replicate it.
		/// </summary>
		// Token: 0x06002DDE RID: 11742 RVA: 0x000C7628 File Offset: 0x000C5828
		public static bool dropReplicatedStructure(Structure structure, Vector3 point, Quaternion rotation, ulong owner, ulong group)
		{
			byte b;
			byte b2;
			if (!Regions.tryGetCoordinate(point, out b, out b2))
			{
				return false;
			}
			StructureRegion structureRegion;
			if (!StructureManager.tryGetRegion(b, b2, out structureRegion))
			{
				return false;
			}
			StructureData structureData = new StructureData(structure, point, rotation, owner, group, Provider.time, StructureManager.instanceCount += 1U);
			NetId netId = NetIdRegistry.ClaimBlock(2U);
			if (StructureManager.manager.spawnStructure(structureRegion, structure.asset.GUID, structureData.point, structureData.rotation, 100, structureData.owner, structureData.group, netId) != null)
			{
				StructureDrop tail = structureRegion.drops.GetTail<StructureDrop>();
				tail.serversideData = structureData;
				structureRegion.structures.Add(structureData);
				StructureManager.SendSingleStructure.Invoke(ENetReliability.Reliable, StructureManager.GatherRemoteClientConnections(b, b2), b, b2, structure.asset.GUID, structureData.point, structureData.rotation, structureData.owner, structureData.group, netId);
				StructureSpawnedHandler structureSpawnedHandler = StructureManager.onStructureSpawned;
				if (structureSpawnedHandler != null)
				{
					structureSpawnedHandler(structureRegion, tail);
				}
			}
			return true;
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x000C771E File Offset: 0x000C591E
		[Obsolete]
		public void tellTakeStructure(CSteamID steamID, byte x, byte y, ushort index, Vector3 ragdoll)
		{
			throw new NotSupportedException("Removed during structure NetId rewrite");
		}

		/// <summary>
		/// Not an instance method because structure might not exist yet, in which case we cancel instantiation.
		/// </summary>
		// Token: 0x06002DE0 RID: 11744 RVA: 0x000C772C File Offset: 0x000C592C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveDestroyStructure(in ClientInvocationContext context, NetId netId, Vector3 ragdoll, bool wasPickedUp)
		{
			StructureDrop structureDrop = NetIdRegistry.Get<StructureDrop>(netId);
			if (structureDrop == null)
			{
				StructureManager.CancelInstantiationByNetId(netId);
				return;
			}
			byte b;
			byte b2;
			StructureRegion structureRegion;
			if (!StructureManager.tryGetRegion(structureDrop.model, out b, out b2, out structureRegion))
			{
				return;
			}
			StructureManager.instance.DestroyOrReleaseStructure(structureDrop);
			structureDrop.model.position = Vector3.zero;
			structureDrop.ReleaseNetId();
			structureRegion.drops.Remove(structureDrop);
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000C778C File Offset: 0x000C598C
		[Obsolete]
		public void tellClearRegionStructures(CSteamID steamID, byte x, byte y)
		{
			StructureManager.ReceiveClearRegionStructures(x, y);
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000C7795 File Offset: 0x000C5995
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellClearRegionStructures")]
		public static void ReceiveClearRegionStructures(byte x, byte y)
		{
			if (!Provider.isServer && !StructureManager.regions[(int)x, (int)y].isNetworked)
			{
				return;
			}
			StructureRegion region = StructureManager.regions[(int)x, (int)y];
			StructureManager.DestroyAllInRegion(region);
			StructureManager.CancelInstantiationsInRegion(region);
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000C77CC File Offset: 0x000C59CC
		public static void askClearRegionStructures(byte x, byte y)
		{
			if (Provider.isServer)
			{
				if (!Regions.checkSafe((int)x, (int)y))
				{
					return;
				}
				StructureRegion structureRegion = StructureManager.regions[(int)x, (int)y];
				if (structureRegion.drops.Count > 0)
				{
					structureRegion.structures.Clear();
					StructureManager.SendClearRegionStructures.InvokeAndLoopback(ENetReliability.Reliable, StructureManager.GatherRemoteClientConnections(x, y), x, y);
				}
			}
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000C7824 File Offset: 0x000C5A24
		public static void askClearAllStructures()
		{
			if (Provider.isServer)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						StructureManager.askClearRegionStructures(b, b2);
					}
				}
			}
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000C7864 File Offset: 0x000C5A64
		private Transform spawnStructure(StructureRegion region, Guid assetId, Vector3 point, Quaternion rotation, byte hp, ulong owner, ulong group, NetId netId)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			ItemStructureAsset itemStructureAsset = Assets.find(assetId) as ItemStructureAsset;
			if (!Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(assetId, itemStructureAsset, "Structure");
			}
			if (itemStructureAsset == null || itemStructureAsset.structure == null)
			{
				return null;
			}
			Transform transform = null;
			try
			{
				if (itemStructureAsset.eligibleForPooling)
				{
					int instanceID = itemStructureAsset.structure.GetInstanceID();
					Stack<GameObject> orAddNew = DictionaryEx.GetOrAddNew<int, Stack<GameObject>>(this.pool, instanceID);
					while (orAddNew.Count > 0)
					{
						GameObject gameObject = orAddNew.Pop();
						if (gameObject != null)
						{
							transform = gameObject.transform;
							transform.SetPositionAndRotation(point, rotation);
							gameObject.SetActive(true);
							break;
						}
					}
				}
				if (transform == null)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(itemStructureAsset.structure, point, rotation);
					transform = gameObject2.transform;
					gameObject2.name = itemStructureAsset.id.ToString();
					if (Provider.isServer && itemStructureAsset.nav != null)
					{
						Transform transform2 = Object.Instantiate<GameObject>(itemStructureAsset.nav).transform;
						transform2.name = "Nav";
						transform2.parent = transform;
						transform2.localPosition = Vector3.zero;
						transform2.localRotation = Quaternion.identity;
					}
				}
				if (!itemStructureAsset.isUnpickupable)
				{
					Interactable2HP orAddComponent = transform.GetOrAddComponent<Interactable2HP>();
					orAddComponent.hp = hp;
					Interactable2SalvageStructure orAddComponent2 = transform.GetOrAddComponent<Interactable2SalvageStructure>();
					orAddComponent2.hp = orAddComponent;
					orAddComponent2.owner = owner;
					orAddComponent2.group = group;
					orAddComponent2.salvageDurationMultiplier = itemStructureAsset.salvageDurationMultiplier;
				}
				StructureDrop structureDrop = new StructureDrop(transform, itemStructureAsset);
				transform.GetOrAddComponent<StructureRefComponent>().tempNotSureIfStructureShouldBeAComponentYet = structureDrop;
				structureDrop.AssignNetId(netId);
				region.drops.Add(structureDrop);
				if (transform != null)
				{
					try
					{
						StructureManager.housingConnections.LinkConnections(structureDrop);
					}
					catch (Exception e)
					{
						UnturnedLog.exception(e, "Caught exception while linking housing connections:");
					}
				}
			}
			catch (Exception e2)
			{
				UnturnedLog.warn("Exception while spawning structure: {0}", new object[]
				{
					itemStructureAsset
				});
				UnturnedLog.exception(e2);
				if (transform != null)
				{
					Object.Destroy(transform.gameObject);
					transform = null;
				}
			}
			return transform;
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000C7A78 File Offset: 0x000C5C78
		[Obsolete]
		public void tellStructure(CSteamID steamID, byte x, byte y, ushort id, Vector3 point, byte angle_x, byte angle_y, byte angle_z, ulong owner, ulong group, uint instanceID)
		{
			throw new NotSupportedException("Structures no longer function without a unique NetId");
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000C7A84 File Offset: 0x000C5C84
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellStructure")]
		public static void ReceiveSingleStructure(byte x, byte y, Guid id, Vector3 point, Quaternion rotation, ulong owner, ulong group, NetId netId)
		{
			StructureRegion structureRegion;
			if (StructureManager.tryGetRegion(x, y, out structureRegion))
			{
				if (!Provider.isServer && !structureRegion.isNetworked)
				{
					return;
				}
				float sortOrder = 0f;
				if (MainCamera.instance != null)
				{
					sortOrder = (MainCamera.instance.transform.position - point).sqrMagnitude;
				}
				StructureInstantiationParameters structureInstantiationParameters = new StructureInstantiationParameters
				{
					region = structureRegion,
					assetId = id,
					position = point,
					rotation = rotation,
					hp = 100,
					owner = owner,
					group = group,
					netId = netId,
					sortOrder = sortOrder
				};
				NetInvocationDeferralRegistry.MarkDeferred(structureInstantiationParameters.netId, 2U);
				StructureManager.pendingInstantiations.Insert(StructureManager.pendingInstantiations.FindInsertionIndex(structureInstantiationParameters), structureInstantiationParameters);
			}
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000C7B57 File Offset: 0x000C5D57
		[Obsolete]
		public void tellStructures(CSteamID steamID)
		{
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000C7B5C File Offset: 0x000C5D5C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveMultipleStructures(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			byte b2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b2);
			StructureRegion structureRegion;
			if (!StructureManager.tryGetRegion(b, b2, out structureRegion))
			{
				return;
			}
			byte b3;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b3);
			if (b3 == 0)
			{
				if (structureRegion.isNetworked)
				{
					return;
				}
				StructureManager.DestroyAllInRegion(StructureManager.regions[(int)b, (int)b2]);
			}
			else if (!structureRegion.isNetworked)
			{
				return;
			}
			structureRegion.isNetworked = true;
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num);
			if (num > 0)
			{
				float sortOrder;
				SystemNetPakReaderEx.ReadFloat(reader, ref sortOrder);
				StructureManager.instantiationsToInsert.Clear();
				for (ushort num2 = 0; num2 < num; num2 += 1)
				{
					StructureInstantiationParameters structureInstantiationParameters = default(StructureInstantiationParameters);
					structureInstantiationParameters.region = structureRegion;
					structureInstantiationParameters.sortOrder = sortOrder;
					SystemNetPakReaderEx.ReadGuid(reader, ref structureInstantiationParameters.assetId);
					UnityNetPakReaderEx.ReadClampedVector3(reader, ref structureInstantiationParameters.position, 13, 11);
					UnityNetPakReaderEx.ReadSpecialYawOrQuaternion(reader, ref structureInstantiationParameters.rotation, 23, 9);
					SystemNetPakReaderEx.ReadUInt8(reader, ref structureInstantiationParameters.hp);
					SystemNetPakReaderEx.ReadUInt64(reader, ref structureInstantiationParameters.owner);
					SystemNetPakReaderEx.ReadUInt64(reader, ref structureInstantiationParameters.group);
					reader.ReadNetId(out structureInstantiationParameters.netId);
					NetInvocationDeferralRegistry.MarkDeferred(structureInstantiationParameters.netId, 2U);
					StructureManager.instantiationsToInsert.Add(structureInstantiationParameters);
				}
				StructureManager.pendingInstantiations.InsertRange(StructureManager.pendingInstantiations.FindInsertionIndex(StructureManager.instantiationsToInsert[0]), StructureManager.instantiationsToInsert);
			}
			Level.isLoadingStructures = false;
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000C7CC9 File Offset: 0x000C5EC9
		[Obsolete]
		public void askStructures(CSteamID steamID, byte x, byte y)
		{
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000C7CCC File Offset: 0x000C5ECC
		internal void askStructures(ITransportConnection transportConnection, byte x, byte y, float sortOrder)
		{
			StructureRegion region;
			if (!StructureManager.tryGetRegion(x, y, out region))
			{
				return;
			}
			if (region.drops.Count > 0)
			{
				byte packet = 0;
				int index = 0;
				int count = 0;
				while (index < region.drops.Count)
				{
					int num = 0;
					while (count < region.drops.Count)
					{
						num += 44;
						int count2 = count;
						count = count2 + 1;
						if (num > Block.BUFFER_SIZE / 2)
						{
							break;
						}
					}
					StructureManager.SendMultipleStructures.Invoke(ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
					{
						SystemNetPakWriterEx.WriteUInt8(writer, x);
						SystemNetPakWriterEx.WriteUInt8(writer, y);
						SystemNetPakWriterEx.WriteUInt8(writer, packet);
						int index;
						SystemNetPakWriterEx.WriteUInt16(writer, (ushort)(count - index));
						SystemNetPakWriterEx.WriteFloat(writer, sortOrder);
						while (index < count)
						{
							StructureData serversideData = region.drops[index].serversideData;
							SystemNetPakWriterEx.WriteGuid(writer, serversideData.structure.asset.GUID);
							UnityNetPakWriterEx.WriteClampedVector3(writer, serversideData.point, 13, 11);
							UnityNetPakWriterEx.WriteSpecialYawOrQuaternion(writer, serversideData.rotation, 23, 9);
							SystemNetPakWriterEx.WriteUInt8(writer, (byte)Mathf.RoundToInt((float)serversideData.structure.health / (float)serversideData.structure.asset.health * 100f));
							SystemNetPakWriterEx.WriteUInt64(writer, serversideData.owner);
							SystemNetPakWriterEx.WriteUInt64(writer, serversideData.group);
							writer.WriteNetId(region.drops[index].GetNetId());
							index = index;
							index++;
						}
					});
					byte packet2 = packet;
					packet = packet2 + 1;
				}
				return;
			}
			StructureManager.SendMultipleStructures.Invoke(ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt8(writer, x);
				SystemNetPakWriterEx.WriteUInt8(writer, y);
				SystemNetPakWriterEx.WriteUInt8(writer, 0);
				SystemNetPakWriterEx.WriteUInt16(writer, 0);
			});
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000C7DF0 File Offset: 0x000C5FF0
		private static void updateActivity(StructureRegion region, CSteamID owner, CSteamID group)
		{
			foreach (StructureDrop structureDrop in region.drops)
			{
				StructureData serversideData = structureDrop.serversideData;
				if (OwnershipTool.checkToggle(owner, serversideData.owner, group, serversideData.group))
				{
					serversideData.objActiveDate = Provider.time;
				}
			}
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000C7E64 File Offset: 0x000C6064
		private static void updateActivity(CSteamID owner, CSteamID group)
		{
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					StructureManager.updateActivity(StructureManager.regions[(int)b, (int)b2], owner, group);
				}
			}
		}

		/// <summary>
		/// Not ideal, but there was a problem because onLevelLoaded was not resetting these after disconnecting.
		/// </summary>
		// Token: 0x06002DEE RID: 11758 RVA: 0x000C7EA6 File Offset: 0x000C60A6
		internal static void ClearNetworkStuff()
		{
			StructureManager.pendingInstantiations = new List<StructureInstantiationParameters>();
			StructureManager.instantiationsToInsert = new List<StructureInstantiationParameters>();
			StructureManager.regionsPendingDestroy = new List<StructureRegion>();
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000C7EC8 File Offset: 0x000C60C8
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP)
			{
				StructureManager.regions = new StructureRegion[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						StructureManager.regions[(int)b, (int)b2] = new StructureRegion();
					}
				}
				StructureManager.instanceCount = 0U;
				this.pool = new Dictionary<int, Stack<GameObject>>();
				StructureManager.housingConnections = new HousingConnections();
				if (Provider.isServer)
				{
					StructureManager.load();
				}
			}
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000C7F4C File Offset: 0x000C614C
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
							if (player.movement.loadedRegions[(int)b, (int)b2].isStructuresLoaded && !Regions.checkArea(b, b2, new_x, new_y, StructureManager.STRUCTURE_REGIONS))
							{
								player.movement.loadedRegions[(int)b, (int)b2].isStructuresLoaded = false;
							}
						}
						else if (player.channel.IsLocalPlayer && StructureManager.regions[(int)b, (int)b2].isNetworked && !Regions.checkArea(b, b2, new_x, new_y, StructureManager.STRUCTURE_REGIONS))
						{
							if (StructureManager.regions[(int)b, (int)b2].drops.Count > 0)
							{
								StructureManager.regions[(int)b, (int)b2].isPendingDestroy = true;
								StructureManager.regionsPendingDestroy.Add(StructureManager.regions[(int)b, (int)b2]);
							}
							StructureManager.CancelInstantiationsInRegion(StructureManager.regions[(int)b, (int)b2]);
							StructureManager.regions[(int)b, (int)b2].isNetworked = false;
						}
					}
				}
			}
			if (step == 1 && Regions.checkSafe((int)new_x, (int)new_y))
			{
				Vector3 position = player.transform.position;
				for (int i = (int)(new_x - StructureManager.STRUCTURE_REGIONS); i <= (int)(new_x + StructureManager.STRUCTURE_REGIONS); i++)
				{
					for (int j = (int)(new_y - StructureManager.STRUCTURE_REGIONS); j <= (int)(new_y + StructureManager.STRUCTURE_REGIONS); j++)
					{
						if (Regions.checkSafe((int)((byte)i), (int)((byte)j)) && !player.movement.loadedRegions[i, j].isStructuresLoaded)
						{
							player.movement.loadedRegions[i, j].isStructuresLoaded = true;
							float sortOrder = Regions.HorizontalDistanceFromCenterSquared(i, j, position);
							this.askStructures(player.channel.owner.transportConnection, (byte)i, (byte)j, sortOrder);
						}
					}
				}
			}
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000C8144 File Offset: 0x000C6344
		private void onPlayerCreated(Player player)
		{
			PlayerMovement movement = player.movement;
			movement.onRegionUpdated = (PlayerRegionUpdated)Delegate.Combine(movement.onRegionUpdated, new PlayerRegionUpdated(this.onRegionUpdated));
			if (Provider.isServer)
			{
				StructureManager.updateActivity(player.channel.owner.playerID.steamID, player.quests.groupID);
			}
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000C81A4 File Offset: 0x000C63A4
		private void Start()
		{
			StructureManager.manager = this;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000C8218 File Offset: 0x000C6418
		private void OnLogMemoryUsage(List<string> results)
		{
			int num = 0;
			int num2 = 0;
			StructureRegion[,] regions = StructureManager.regions;
			int upperBound = regions.GetUpperBound(0);
			int upperBound2 = regions.GetUpperBound(1);
			for (int i = regions.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = regions.GetLowerBound(1); j <= upperBound2; j++)
				{
					StructureRegion structureRegion = regions[i, j];
					if (structureRegion.drops.Count > 0)
					{
						num++;
					}
					num2 += structureRegion.drops.Count;
				}
			}
			results.Add(string.Format("Structure regions: {0}", num));
			results.Add(string.Format("Structures placed: {0}", num2));
			if (StructureManager.housingConnections != null)
			{
				StructureManager.housingConnections.OnLogMemoryUsage(results);
			}
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000C82DC File Offset: 0x000C64DC
		public static void load()
		{
			bool flag = false;
			if (LevelSavedata.fileExists("/Structures.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				River river = LevelSavedata.openRiver("/Structures.dat", true);
				byte b = river.readByte();
				if (b > 3)
				{
					StructureManager.serverActiveDate = river.readUInt32();
				}
				else
				{
					StructureManager.serverActiveDate = Provider.time;
				}
				if (b < 7)
				{
					StructureManager.instanceCount = 0U;
				}
				else
				{
					StructureManager.instanceCount = river.readUInt32();
				}
				if (b > 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						for (byte b3 = 0; b3 < Regions.WORLD_SIZE; b3 += 1)
						{
							StructureRegion region = StructureManager.regions[(int)b2, (int)b3];
							StructureManager.loadRegion(b, river, region);
						}
					}
				}
				if (b < 6)
				{
					flag = true;
				}
				river.closeRiver();
			}
			else
			{
				flag = true;
			}
			if (flag && LevelObjects.buildables != null)
			{
				int num = 0;
				for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
				{
					for (byte b5 = 0; b5 < Regions.WORLD_SIZE; b5 += 1)
					{
						List<LevelBuildableObject> list = LevelObjects.buildables[(int)b4, (int)b5];
						if (list != null && list.Count != 0)
						{
							StructureRegion structureRegion = StructureManager.regions[(int)b4, (int)b5];
							for (int i = 0; i < list.Count; i++)
							{
								LevelBuildableObject levelBuildableObject = list[i];
								if (levelBuildableObject != null)
								{
									ItemStructureAsset itemStructureAsset = levelBuildableObject.asset as ItemStructureAsset;
									if (itemStructureAsset != null)
									{
										Structure structure = new Structure(itemStructureAsset, itemStructureAsset.health);
										StructureData structureData = new StructureData(structure, levelBuildableObject.point, levelBuildableObject.rotation, 0UL, 0UL, uint.MaxValue, StructureManager.instanceCount += 1U);
										NetId netId = NetIdRegistry.ClaimBlock(2U);
										if (StructureManager.manager.spawnStructure(structureRegion, itemStructureAsset.GUID, structureData.point, structureData.rotation, (byte)Mathf.RoundToInt((float)structure.health / (float)itemStructureAsset.health * 100f), 0UL, 0UL, netId) != null)
										{
											structureRegion.drops.GetTail<StructureDrop>().serversideData = structureData;
											structureRegion.structures.Add(structureData);
											num++;
										}
										else
										{
											UnturnedLog.warn(string.Format("Failed to spawn default structure object {0} at {1}", itemStructureAsset.name, levelBuildableObject.point));
										}
									}
								}
							}
						}
					}
				}
				UnturnedLog.info(string.Format("Spawned {0} default structures from level", num));
			}
			Level.isLoadingStructures = false;
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x000C8554 File Offset: 0x000C6754
		public static void save()
		{
			River river = LevelSavedata.openRiver("/Structures.dat", false);
			river.writeByte(9);
			river.writeUInt32(Provider.time);
			river.writeUInt32(StructureManager.instanceCount);
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					StructureRegion region = StructureManager.regions[(int)b, (int)b2];
					StructureManager.saveRegion(river, region);
				}
			}
			river.closeRiver();
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000C85C8 File Offset: 0x000C67C8
		private static void loadRegion(byte version, River river, StructureRegion region)
		{
			ushort num = river.readUInt16();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				ItemStructureAsset itemStructureAsset;
				if (version < 8)
				{
					ushort id = river.readUInt16();
					itemStructureAsset = (Assets.find(EAssetType.ITEM, id) as ItemStructureAsset);
				}
				else
				{
					itemStructureAsset = (Assets.find(river.readGUID()) as ItemStructureAsset);
				}
				uint newInstanceID;
				if (version < 7)
				{
					newInstanceID = (StructureManager.instanceCount += 1U);
				}
				else
				{
					newInstanceID = river.readUInt32();
				}
				ushort num3 = river.readUInt16();
				Vector3 vector = river.readSingleVector3();
				Quaternion quaternion;
				if (version < 9)
				{
					byte b = 0;
					if (version > 4)
					{
						b = river.readByte();
					}
					byte b2 = river.readByte();
					byte b3 = 0;
					if (version > 4)
					{
						b3 = river.readByte();
					}
					if (version < 5)
					{
						quaternion = Quaternion.Euler(-90f, (float)(b2 * 2), 0f);
					}
					else
					{
						quaternion = Quaternion.Euler((float)b * 2f, (float)b2 * 2f, (float)b3 * 2f);
					}
				}
				else
				{
					quaternion = river.readSingleQuaternion();
				}
				ulong num4 = 0UL;
				ulong num5 = 0UL;
				if (version > 2)
				{
					num4 = river.readUInt64();
					num5 = river.readUInt64();
				}
				uint newObjActiveDate;
				if (version > 3)
				{
					newObjActiveDate = river.readUInt32();
					if (Provider.time - StructureManager.serverActiveDate > Provider.modeConfigData.Structures.Decay_Time / 2U)
					{
						newObjActiveDate = Provider.time;
					}
				}
				else
				{
					newObjActiveDate = Provider.time;
				}
				if (itemStructureAsset != null)
				{
					NetId netId = NetIdRegistry.ClaimBlock(2U);
					if (StructureManager.manager.spawnStructure(region, itemStructureAsset.GUID, vector, quaternion, (byte)Mathf.RoundToInt((float)num3 / (float)itemStructureAsset.health * 100f), num4, num5, netId) != null)
					{
						StructureDrop tail = region.drops.GetTail<StructureDrop>();
						StructureData structureData = new StructureData(new Structure(itemStructureAsset, num3), vector, quaternion, num4, num5, newObjActiveDate, newInstanceID);
						tail.serversideData = structureData;
						region.structures.Add(structureData);
					}
				}
			}
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000C8790 File Offset: 0x000C6990
		private static void saveRegion(River river, StructureRegion region)
		{
			uint time = Provider.time;
			ushort num = 0;
			foreach (StructureDrop structureDrop in region.drops)
			{
				StructureData serversideData = structureDrop.serversideData;
				if ((Provider.modeConfigData.Structures.Decay_Time == 0U || time < serversideData.objActiveDate || time - serversideData.objActiveDate < Provider.modeConfigData.Structures.Decay_Time) && serversideData.structure.asset.isSaveable)
				{
					num += 1;
				}
			}
			river.writeUInt16(num);
			foreach (StructureDrop structureDrop2 in region.drops)
			{
				StructureData serversideData2 = structureDrop2.serversideData;
				if ((Provider.modeConfigData.Structures.Decay_Time == 0U || time < serversideData2.objActiveDate || time - serversideData2.objActiveDate < Provider.modeConfigData.Structures.Decay_Time) && serversideData2.structure.asset.isSaveable)
				{
					river.writeGUID(structureDrop2.asset.GUID);
					river.writeUInt32(serversideData2.instanceID);
					river.writeUInt16(serversideData2.structure.health);
					river.writeSingleVector3(serversideData2.point);
					river.writeSingleQuaternion(serversideData2.rotation);
					river.writeUInt64(serversideData2.owner);
					river.writeUInt64(serversideData2.group);
					river.writeUInt32(serversideData2.objActiveDate);
				}
			}
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000C8944 File Offset: 0x000C6B44
		public static PooledTransportConnectionList GatherRemoteClientConnections(byte x, byte y)
		{
			return Regions.GatherRemoteClientConnections(x, y, StructureManager.STRUCTURE_REGIONS);
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000C8952 File Offset: 0x000C6B52
		[Obsolete("Replaced by GatherRemoteClientConnections")]
		public static IEnumerable<ITransportConnection> EnumerateClients_Remote(byte x, byte y)
		{
			return StructureManager.GatherRemoteClientConnections(x, y);
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000C895B File Offset: 0x000C6B5B
		private static void DestroyAllInRegion(StructureRegion region)
		{
			if (region.drops.Count > 0)
			{
				region.DestroyAll();
			}
			if (region.isPendingDestroy)
			{
				region.isPendingDestroy = false;
				StructureManager.regionsPendingDestroy.RemoveFast(region);
			}
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x000C898C File Offset: 0x000C6B8C
		private static void CancelInstantiationsInRegion(StructureRegion region)
		{
			for (int i = StructureManager.pendingInstantiations.Count - 1; i >= 0; i--)
			{
				if (StructureManager.pendingInstantiations[i].region == region)
				{
					NetInvocationDeferralRegistry.Cancel(StructureManager.pendingInstantiations[i].netId, 2U);
					StructureManager.pendingInstantiations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000C89E4 File Offset: 0x000C6BE4
		private static void CancelInstantiationByNetId(NetId netId)
		{
			for (int i = StructureManager.pendingInstantiations.Count - 1; i >= 0; i--)
			{
				if (StructureManager.pendingInstantiations[i].netId == netId)
				{
					NetInvocationDeferralRegistry.Cancel(netId, 2U);
					StructureManager.pendingInstantiations.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000C8A34 File Offset: 0x000C6C34
		internal void DestroyOrReleaseStructure(StructureDrop drop)
		{
			try
			{
				StructureManager.housingConnections.UnlinkConnections(drop);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception while unlinking housing connections:");
			}
			EffectManager.ClearAttachments(drop.model);
			if (drop.asset.eligibleForPooling)
			{
				drop.model.gameObject.SetActive(false);
				int instanceID = drop.asset.structure.GetInstanceID();
				DictionaryEx.GetOrAddNew<int, Stack<GameObject>>(this.pool, instanceID).Push(drop.model.gameObject);
				return;
			}
			Object.Destroy(drop.model.gameObject);
		}

		// Token: 0x040018AD RID: 6317
		public const byte SAVEDATA_VERSION_INITIAL = 8;

		// Token: 0x040018AE RID: 6318
		public const byte SAVEDATA_VERSION_REPLACE_EULER_ANGLES_WITH_QUATERNION = 9;

		// Token: 0x040018AF RID: 6319
		private const byte SAVEDATA_VERSION_NEWEST = 9;

		// Token: 0x040018B0 RID: 6320
		public static readonly byte SAVEDATA_VERSION = 9;

		// Token: 0x040018B1 RID: 6321
		public static readonly byte STRUCTURE_REGIONS = 2;

		// Token: 0x040018B2 RID: 6322
		public static readonly float WALL = 2.125f;

		// Token: 0x040018B3 RID: 6323
		public static readonly float PILLAR = 3.1f;

		// Token: 0x040018B4 RID: 6324
		public static readonly float HEIGHT = 2.125f;

		// Token: 0x040018B5 RID: 6325
		public static DeployStructureRequestHandler onDeployStructureRequested;

		// Token: 0x040018B6 RID: 6326
		[Obsolete("Please use StructureDrop.OnSalvageRequested_Global instead")]
		public static SalvageStructureRequestHandler onSalvageStructureRequested;

		// Token: 0x040018B7 RID: 6327
		public static DamageStructureRequestHandler onDamageStructureRequested;

		// Token: 0x040018B8 RID: 6328
		public static RepairStructureRequestHandler OnRepairRequested;

		// Token: 0x040018B9 RID: 6329
		public static RepairedStructureHandler OnRepaired;

		// Token: 0x040018BA RID: 6330
		public static StructureSpawnedHandler onStructureSpawned;

		// Token: 0x040018BB RID: 6331
		public static TransformStructureRequestHandler onTransformRequested;

		// Token: 0x040018BC RID: 6332
		private static StructureManager manager;

		// Token: 0x040018BE RID: 6334
		internal static HousingConnections housingConnections;

		// Token: 0x040018BF RID: 6335
		private static List<StructureInstantiationParameters> pendingInstantiations;

		// Token: 0x040018C0 RID: 6336
		private static List<StructureInstantiationParameters> instantiationsToInsert;

		// Token: 0x040018C1 RID: 6337
		private static List<StructureRegion> regionsPendingDestroy;

		// Token: 0x040018C2 RID: 6338
		private static uint instanceCount;

		// Token: 0x040018C3 RID: 6339
		private static uint serverActiveDate;

		// Token: 0x040018C4 RID: 6340
		private static readonly ClientStaticMethod<NetId, Vector3, bool> SendDestroyStructure = ClientStaticMethod<NetId, Vector3, bool>.Get(new ClientStaticMethod<NetId, Vector3, bool>.ReceiveDelegateWithContext(StructureManager.ReceiveDestroyStructure));

		// Token: 0x040018C5 RID: 6341
		private static readonly ClientStaticMethod<byte, byte> SendClearRegionStructures = ClientStaticMethod<byte, byte>.Get(new ClientStaticMethod<byte, byte>.ReceiveDelegate(StructureManager.ReceiveClearRegionStructures));

		// Token: 0x040018C6 RID: 6342
		private static readonly ClientStaticMethod<byte, byte, Guid, Vector3, Quaternion, ulong, ulong, NetId> SendSingleStructure = ClientStaticMethod<byte, byte, Guid, Vector3, Quaternion, ulong, ulong, NetId>.Get(new ClientStaticMethod<byte, byte, Guid, Vector3, Quaternion, ulong, ulong, NetId>.ReceiveDelegate(StructureManager.ReceiveSingleStructure));

		// Token: 0x040018C7 RID: 6343
		private static ClientStaticMethod SendMultipleStructures = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(StructureManager.ReceiveMultipleStructures));

		/// <summary>
		/// Maps prefab unique id to inactive list.
		/// </summary>
		// Token: 0x040018C8 RID: 6344
		private Dictionary<int, Stack<GameObject>> pool;

		// Token: 0x040018C9 RID: 6345
		internal const int POSITION_FRAC_BIT_COUNT = 11;

		/// <summary>
		/// Sending yaw only costs 1 bit (flag) plus yaw bits, so compared to the old 24-bit rotation we may as well
		/// make it high-precision. Quaternion mode uses 1+27 bits!
		/// </summary>
		// Token: 0x040018CA RID: 6346
		internal const int YAW_BIT_COUNT = 23;

		/// <summary>
		/// +0 = StructureDrop
		/// +1 = root transform
		/// </summary>
		// Token: 0x040018CB RID: 6347
		internal const int NETIDS_PER_STRUCTURE = 2;
	}
}
