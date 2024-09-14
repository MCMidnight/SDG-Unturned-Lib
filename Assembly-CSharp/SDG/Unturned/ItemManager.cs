using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200056A RID: 1386
	public class ItemManager : SteamCaller
	{
		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002C2B RID: 11307 RVA: 0x000BE440 File Offset: 0x000BC640
		public static ItemManager instance
		{
			get
			{
				return ItemManager.manager;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002C2C RID: 11308 RVA: 0x000BE447 File Offset: 0x000BC647
		// (set) Token: 0x06002C2D RID: 11309 RVA: 0x000BE44E File Offset: 0x000BC64E
		public static ItemRegion[,] regions { get; private set; }

		/// <summary>
		/// Kept for plugin backwards compatibility.
		/// This one is problematic because on the client physics can move items between regions.
		/// </summary>
		// Token: 0x06002C2E RID: 11310 RVA: 0x000BE458 File Offset: 0x000BC658
		public static void getItemsInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search, List<InteractableItem> result)
		{
			if (ItemManager.regions == null)
			{
				return;
			}
			for (int i = 0; i < search.Count; i++)
			{
				RegionCoordinate regionCoordinate = search[i];
				if (ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y] != null)
				{
					for (int j = 0; j < ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].drops.Count; j++)
					{
						ItemDrop itemDrop = ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].drops[j];
						if ((itemDrop.model.position - center).sqrMagnitude < sqrRadius)
						{
							result.Add(itemDrop.interactableItem);
						}
					}
				}
			}
		}

		/// <summary>
		/// Find physically simulated items within radius.
		/// </summary>
		// Token: 0x06002C2F RID: 11311 RVA: 0x000BE51C File Offset: 0x000BC71C
		public static void findSimulatedItemsInRadius(Vector3 center, float sqrRadius, List<InteractableItem> result)
		{
			if (ItemManager.clampedItems == null)
			{
				return;
			}
			foreach (InteractableItem interactableItem in ItemManager.clampedItems)
			{
				if (!(interactableItem == null) && (interactableItem.transform.position - center).sqrMagnitude <= sqrRadius)
				{
					result.Add(interactableItem);
				}
			}
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x000BE59C File Offset: 0x000BC79C
		public static void takeItem(Transform item, byte to_x, byte to_y, byte to_rot, byte to_page)
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(item.position, out b, out b2))
			{
				ItemRegion itemRegion = ItemManager.regions[(int)b, (int)b2];
				for (int i = 0; i < itemRegion.drops.Count; i++)
				{
					if (itemRegion.drops[i].model == item)
					{
						ItemManager.SendTakeItemRequest.Invoke(ENetReliability.Unreliable, b, b2, itemRegion.drops[i].instanceID, to_x, to_y, to_rot, to_page);
						return;
					}
				}
			}
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x000BE61C File Offset: 0x000BC81C
		public static void dropItem(Item item, Vector3 point, bool playEffect, bool isDropped, bool wideSpread)
		{
			if (ItemManager.regions == null || ItemManager.manager == null)
			{
				return;
			}
			if (wideSpread)
			{
				point.x += Random.Range(-0.75f, 0.75f);
				point.z += Random.Range(-0.75f, 0.75f);
			}
			else
			{
				point.x += Random.Range(-0.125f, 0.125f);
				point.z += Random.Range(-0.125f, 0.125f);
			}
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(point, out b, out b2))
			{
				ItemAsset asset = item.GetAsset();
				if (asset != null && !asset.isPro)
				{
					if (point.y > 0f)
					{
						RaycastHit raycastHit;
						Physics.SphereCast(new Ray(point + Vector3.up, Vector3.down), 0.1f, out raycastHit, 2048f, RayMasks.BLOCK_ITEM);
						if (raycastHit.collider != null)
						{
							point.y = raycastHit.point.y;
						}
					}
					bool flag = true;
					ServerSpawningItemDropHandler serverSpawningItemDropHandler = ItemManager.onServerSpawningItemDrop;
					if (serverSpawningItemDropHandler != null)
					{
						serverSpawningItemDropHandler(item, ref point, ref flag);
					}
					if (!flag)
					{
						return;
					}
					ItemData itemData = new ItemData(item, ItemManager.instanceCount += 1U, point, isDropped);
					ItemManager.regions[(int)b, (int)b2].items.Add(itemData);
					ItemManager.SendItem.Invoke(ENetReliability.Reliable, Regions.GatherClientConnections(b, b2, ItemManager.ITEM_REGIONS), b, b2, item.id, item.amount, item.quality, item.state, point, itemData.instanceID, playEffect);
				}
			}
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x000BE7B2 File Offset: 0x000BC9B2
		[Obsolete]
		public void tellTakeItem(CSteamID steamID, byte x, byte y, uint instanceID)
		{
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x000BE7B4 File Offset: 0x000BC9B4
		private static void PlayInventoryAudio(ItemAsset item, Vector3 position)
		{
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x000BE7B8 File Offset: 0x000BC9B8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveDestroyItem(byte x, byte y, uint instanceID, bool shouldPlayEffect)
		{
			if (!Provider.isServer && !ItemManager.regions[(int)x, (int)y].isNetworked)
			{
				return;
			}
			ItemRegion itemRegion = ItemManager.regions[(int)x, (int)y];
			ushort num = 0;
			while ((int)num < itemRegion.drops.Count)
			{
				if (itemRegion.drops[(int)num].instanceID == instanceID)
				{
					ItemDropRemoved itemDropRemoved = ItemManager.onItemDropRemoved;
					if (itemDropRemoved != null)
					{
						itemDropRemoved(itemRegion.drops[(int)num].model, itemRegion.drops[(int)num].interactableItem);
					}
					if (shouldPlayEffect)
					{
						ItemManager.PlayInventoryAudio(itemRegion.drops[(int)num].interactableItem.asset, itemRegion.drops[(int)num].model.position);
					}
					Object.Destroy(itemRegion.drops[(int)num].model.gameObject);
					itemRegion.drops.RemoveAt((int)num);
					return;
				}
				num += 1;
			}
			ItemManager.CancelInstantiationByInstanceId(instanceID);
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x000BE8B8 File Offset: 0x000BCAB8
		[Obsolete]
		public void askTakeItem(CSteamID steamID, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			ItemManager.ReceiveTakeItemRequest(serverInvocationContext, x, y, instanceID, to_x, to_y, to_rot, to_page);
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x000BE8E0 File Offset: 0x000BCAE0
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 10, legacyName = "askTakeItem")]
		public static void ReceiveTakeItemRequest(in ServerInvocationContext context, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page)
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
			if (player.animator.gesture == EPlayerGesture.ARREST_START)
			{
				return;
			}
			ItemRegion itemRegion = ItemManager.regions[(int)x, (int)y];
			ushort num = 0;
			while ((int)num < itemRegion.items.Count)
			{
				ItemData itemData = itemRegion.items[(int)num];
				if (itemData.instanceID == instanceID)
				{
					if ((itemData.point - player.transform.position).sqrMagnitude > 400f)
					{
						return;
					}
					bool flag = true;
					if (ItemManager.onTakeItemRequested != null)
					{
						try
						{
							ItemManager.onTakeItemRequested(player, x, y, instanceID, to_x, to_y, to_rot, to_page, itemData, ref flag);
						}
						catch (Exception e)
						{
							UnturnedLog.exception(e, "Caught exception invoking onTakeItemRequested:");
						}
					}
					if (!flag)
					{
						return;
					}
					bool flag2;
					if (to_page == 255)
					{
						flag2 = player.inventory.tryAddItem(ItemManager.regions[(int)x, (int)y].items[(int)num].item, true);
					}
					else
					{
						flag2 = player.inventory.tryAddItem(ItemManager.regions[(int)x, (int)y].items[(int)num].item, to_x, to_y, to_page, to_rot);
					}
					if (flag2)
					{
						if (!player.equipment.wasTryingToSelect && !player.equipment.HasValidUseable)
						{
							player.animator.sendGesture(EPlayerGesture.PICKUP, true);
						}
						ItemManager.regions[(int)x, (int)y].items.RemoveAt((int)num);
						player.sendStat(EPlayerStat.FOUND_ITEMS);
						ItemManager.SendDestroyItem.Invoke(ENetReliability.Reliable, Regions.GatherClientConnections(x, y, ItemManager.ITEM_REGIONS), x, y, instanceID, true);
						return;
					}
					player.sendMessage(EPlayerMessage.SPACE);
					return;
				}
				else
				{
					num += 1;
				}
			}
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x000BEAAC File Offset: 0x000BCCAC
		[Obsolete]
		public void tellClearRegionItems(CSteamID steamID, byte x, byte y)
		{
			ItemManager.ReceiveClearRegionItems(x, y);
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x000BEAB5 File Offset: 0x000BCCB5
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellClearRegionItems")]
		public static void ReceiveClearRegionItems(byte x, byte y)
		{
			if (!Provider.isServer && !ItemManager.regions[(int)x, (int)y].isNetworked)
			{
				return;
			}
			ItemManager.DestroyAllInRegion(ItemManager.regions[(int)x, (int)y]);
			ItemManager.CancelInstantiationsInRegion(x, y);
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x000BEAEC File Offset: 0x000BCCEC
		public static void askClearRegionItems(byte x, byte y)
		{
			if (Provider.isServer)
			{
				if (!Regions.checkSafe((int)x, (int)y))
				{
					return;
				}
				ItemRegion itemRegion = ItemManager.regions[(int)x, (int)y];
				if (itemRegion.items.Count > 0)
				{
					itemRegion.items.Clear();
					ItemManager.SendClearRegionItems.Invoke(ENetReliability.Reliable, Regions.GatherClientConnections(x, y, ItemManager.ITEM_REGIONS), x, y);
				}
			}
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x000BEB4C File Offset: 0x000BCD4C
		public static void askClearAllItems()
		{
			if (Provider.isServer)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						ItemManager.askClearRegionItems(b, b2);
					}
				}
			}
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x000BEB8C File Offset: 0x000BCD8C
		public static void ServerClearItemsInSphere(Vector3 center, float radius)
		{
			ItemManager.clearItemRegions.Clear();
			Regions.getRegionsInRadius(center, radius, ItemManager.clearItemRegions);
			float num = MathfEx.Square(radius);
			foreach (RegionCoordinate regionCoordinate in ItemManager.clearItemRegions)
			{
				ItemRegion itemRegion = ItemManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y];
				for (int i = itemRegion.items.Count - 1; i >= 0; i--)
				{
					ItemData itemData = itemRegion.items[i];
					if ((itemData.point - center).sqrMagnitude <= num)
					{
						uint instanceID = itemData.instanceID;
						itemRegion.items.RemoveAt(i);
						ItemManager.SendDestroyItem.Invoke(ENetReliability.Reliable, Regions.GatherClientConnections(regionCoordinate.x, regionCoordinate.y, ItemManager.ITEM_REGIONS), regionCoordinate.x, regionCoordinate.y, instanceID, false);
					}
				}
			}
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x000BEC9C File Offset: 0x000BCE9C
		private void spawnItem(byte x, byte y, ushort id, byte amount, byte quality, byte[] state, Vector3 point, uint instanceID, bool shouldPlayEffect)
		{
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, id) as ItemAsset;
			if (itemAsset != null)
			{
				Transform transform = new GameObject().transform;
				transform.name = id.ToString();
				transform.transform.position = point;
				Transform item = ItemTool.getItem(id, 0, quality, state, false, itemAsset, null);
				item.parent = transform;
				InteractableItem interactableItem = item.gameObject.AddComponent<InteractableItem>();
				interactableItem.item = new Item(id, amount, quality, state);
				interactableItem.asset = itemAsset;
				item.position = point + Vector3.up * 0.75f;
				item.rotation = Quaternion.Euler((float)(-90 + Random.Range(-15, 15)), (float)Random.Range(0, 360), (float)Random.Range(-15, 15));
				item.gameObject.AddComponent<Rigidbody>();
				item.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
				item.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
				item.GetComponent<Rigidbody>().drag = 0.5f;
				item.GetComponent<Rigidbody>().angularDrag = 0.1f;
				if (LevelObjects.loads[(int)x, (int)y] != -1)
				{
					item.GetComponent<Rigidbody>().useGravity = false;
					item.GetComponent<Rigidbody>().isKinematic = true;
				}
				ItemDrop itemDrop = new ItemDrop(transform, interactableItem, instanceID);
				ItemManager.regions[(int)x, (int)y].drops.Add(itemDrop);
				ItemDropAdded itemDropAdded = ItemManager.onItemDropAdded;
				if (itemDropAdded != null)
				{
					itemDropAdded(item, interactableItem);
				}
				if (shouldPlayEffect)
				{
					ItemManager.PlayInventoryAudio(itemAsset, point);
				}
			}
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x000BEE14 File Offset: 0x000BD014
		[Obsolete]
		public void tellItem(CSteamID steamID, byte x, byte y, ushort id, byte amount, byte quality, byte[] state, Vector3 point, uint instanceID)
		{
			ItemManager.ReceiveItem(x, y, id, amount, quality, state, point, instanceID, false);
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x000BEE38 File Offset: 0x000BD038
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveItem(byte x, byte y, ushort id, byte amount, byte quality, byte[] state, Vector3 point, uint instanceID, bool shouldPlayEffect)
		{
			if (!Regions.checkSafe((int)x, (int)y))
			{
				return;
			}
			if (!ItemManager.regions[(int)x, (int)y].isNetworked)
			{
				return;
			}
			float sortOrder = 0f;
			if (MainCamera.instance != null)
			{
				sortOrder = (MainCamera.instance.transform.position - point).sqrMagnitude;
			}
			ItemInstantiationParameters itemInstantiationParameters = default(ItemInstantiationParameters);
			itemInstantiationParameters.region_x = x;
			itemInstantiationParameters.region_y = y;
			itemInstantiationParameters.assetId = id;
			itemInstantiationParameters.amount = amount;
			itemInstantiationParameters.quality = quality;
			itemInstantiationParameters.state = state;
			itemInstantiationParameters.point = point;
			itemInstantiationParameters.instanceID = instanceID;
			itemInstantiationParameters.sortOrder = sortOrder;
			itemInstantiationParameters.shouldPlayEffect = shouldPlayEffect;
			ItemManager.pendingInstantiations.Insert(ItemManager.pendingInstantiations.FindInsertionIndex(itemInstantiationParameters), itemInstantiationParameters);
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x000BEF08 File Offset: 0x000BD108
		[Obsolete]
		public void tellItems(CSteamID steamID)
		{
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x000BEF0C File Offset: 0x000BD10C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveItems(in ClientInvocationContext context)
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
			byte b3;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b3);
			if (b3 == 0)
			{
				if (ItemManager.regions[(int)b, (int)b2].isNetworked)
				{
					return;
				}
				ItemManager.DestroyAllInRegion(ItemManager.regions[(int)b, (int)b2]);
			}
			else if (!ItemManager.regions[(int)b, (int)b2].isNetworked)
			{
				return;
			}
			ItemManager.regions[(int)b, (int)b2].isNetworked = true;
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num);
			if (num > 0)
			{
				float sortOrder;
				SystemNetPakReaderEx.ReadFloat(reader, ref sortOrder);
				ItemManager.instantiationsToInsert.Clear();
				for (ushort num2 = 0; num2 < num; num2 += 1)
				{
					ItemInstantiationParameters itemInstantiationParameters = default(ItemInstantiationParameters);
					itemInstantiationParameters.region_x = b;
					itemInstantiationParameters.region_y = b2;
					itemInstantiationParameters.sortOrder = sortOrder;
					SystemNetPakReaderEx.ReadUInt16(reader, ref itemInstantiationParameters.assetId);
					SystemNetPakReaderEx.ReadUInt8(reader, ref itemInstantiationParameters.amount);
					SystemNetPakReaderEx.ReadUInt8(reader, ref itemInstantiationParameters.quality);
					byte b4;
					SystemNetPakReaderEx.ReadUInt8(reader, ref b4);
					byte[] array = new byte[(int)b4];
					reader.ReadBytes(array);
					itemInstantiationParameters.state = array;
					UnityNetPakReaderEx.ReadClampedVector3(reader, ref itemInstantiationParameters.point, 13, 7);
					SystemNetPakReaderEx.ReadUInt32(reader, ref itemInstantiationParameters.instanceID);
					ItemManager.instantiationsToInsert.Add(itemInstantiationParameters);
				}
				ItemManager.pendingInstantiations.InsertRange(ItemManager.pendingInstantiations.FindInsertionIndex(ItemManager.instantiationsToInsert[0]), ItemManager.instantiationsToInsert);
			}
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x000BF08F File Offset: 0x000BD28F
		[Obsolete]
		public void askItems(CSteamID steamID, byte x, byte y)
		{
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x000BF094 File Offset: 0x000BD294
		internal void askItems(ITransportConnection transportConnection, byte x, byte y, float sortOrder)
		{
			if (ItemManager.regions[(int)x, (int)y].items.Count > 0)
			{
				byte packet = 0;
				int index = 0;
				int count = 0;
				while (index < ItemManager.regions[(int)x, (int)y].items.Count)
				{
					int num = 0;
					while (count < ItemManager.regions[(int)x, (int)y].items.Count)
					{
						num += 4 + ItemManager.regions[(int)x, (int)y].items[count].item.state.Length + 12 + 4;
						int count2 = count;
						count = count2 + 1;
						if (num > Block.BUFFER_SIZE / 2)
						{
							break;
						}
					}
					ItemManager.SendItems.Invoke(ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
					{
						SystemNetPakWriterEx.WriteUInt8(writer, x);
						SystemNetPakWriterEx.WriteUInt8(writer, y);
						SystemNetPakWriterEx.WriteUInt8(writer, packet);
						int index;
						SystemNetPakWriterEx.WriteUInt16(writer, (ushort)(count - index));
						SystemNetPakWriterEx.WriteFloat(writer, sortOrder);
						while (index < count)
						{
							ItemData itemData = ItemManager.regions[(int)x, (int)y].items[index];
							SystemNetPakWriterEx.WriteUInt16(writer, itemData.item.id);
							SystemNetPakWriterEx.WriteUInt8(writer, itemData.item.amount);
							SystemNetPakWriterEx.WriteUInt8(writer, itemData.item.quality);
							SystemNetPakWriterEx.WriteUInt8(writer, (byte)itemData.item.state.Length);
							writer.WriteBytes(itemData.item.state);
							UnityNetPakWriterEx.WriteClampedVector3(writer, itemData.point, 13, 7);
							SystemNetPakWriterEx.WriteUInt32(writer, itemData.instanceID);
							index = index;
							index++;
						}
					});
					byte packet2 = packet;
					packet = packet2 + 1;
				}
				return;
			}
			ItemManager.SendItems.Invoke(ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt8(writer, x);
				SystemNetPakWriterEx.WriteUInt8(writer, y);
				SystemNetPakWriterEx.WriteUInt8(writer, 0);
				SystemNetPakWriterEx.WriteUInt16(writer, 0);
			});
		}

		/// <summary>
		/// Despawn any old items in the current despawn region.
		/// </summary>
		/// <returns>True if the region had items to search through.</returns>
		// Token: 0x06002C43 RID: 11331 RVA: 0x000BF220 File Offset: 0x000BD420
		private bool despawnItems()
		{
			if (Level.info == null || Level.info.type == ELevelType.ARENA)
			{
				return false;
			}
			if (ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items.Count > 0)
			{
				for (int i = 0; i < ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items.Count; i++)
				{
					if (Time.realtimeSinceStartup - ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items[i].lastDropped > (ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items[i].isDropped ? Provider.modeConfigData.Items.Despawn_Dropped_Time : Provider.modeConfigData.Items.Despawn_Natural_Time))
					{
						uint instanceID = ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items[i].instanceID;
						ItemManager.regions[(int)ItemManager.despawnItems_X, (int)ItemManager.despawnItems_Y].items.RemoveAt(i);
						ItemManager.SendDestroyItem.Invoke(ENetReliability.Reliable, Regions.GatherClientConnections(ItemManager.despawnItems_X, ItemManager.despawnItems_Y, ItemManager.ITEM_REGIONS), ItemManager.despawnItems_X, ItemManager.despawnItems_Y, instanceID, false);
						break;
					}
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Attempt to respawn an item in the current respawn region.
		/// </summary>
		/// <returns>True if an item was succesfully respawned.</returns>
		// Token: 0x06002C44 RID: 11332 RVA: 0x000BF37C File Offset: 0x000BD57C
		private bool respawnItems()
		{
			if (Level.info == null || Level.info.type == ELevelType.ARENA)
			{
				return false;
			}
			if (LevelItems.spawns[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].Count > 0 && Time.realtimeSinceStartup - ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].lastRespawn > Provider.modeConfigData.Items.Respawn_Time)
			{
				int count = ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].items.Count;
				int num = (int)((float)LevelItems.spawns[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].Count * Provider.modeConfigData.Items.Spawn_Chance);
				bool flag = false;
				for (int i = count; i < num; i++)
				{
					ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y][Random.Range(0, LevelItems.spawns[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].Count)];
					bool flag2 = true;
					if (!SafezoneManager.checkPointValid(itemSpawnpoint.point))
					{
						flag2 = false;
					}
					ushort num2 = 0;
					while ((int)num2 < ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].items.Count)
					{
						if ((ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].items[(int)num2].point - itemSpawnpoint.point).sqrMagnitude < 4f)
						{
							flag2 = false;
							break;
						}
						num2 += 1;
					}
					if (flag2)
					{
						ushort item = LevelItems.getItem(itemSpawnpoint);
						if (Assets.find(EAssetType.ITEM, item) is ItemAsset)
						{
							Item item2 = new Item(item, EItemOrigin.WORLD);
							Vector3 point = itemSpawnpoint.point;
							bool flag3 = true;
							ServerSpawningItemDropHandler serverSpawningItemDropHandler = ItemManager.onServerSpawningItemDrop;
							if (serverSpawningItemDropHandler != null)
							{
								serverSpawningItemDropHandler(item2, ref point, ref flag3);
							}
							if (!flag3)
							{
								goto IL_2C8;
							}
							ItemData itemData = new ItemData(item2, ItemManager.instanceCount += 1U, itemSpawnpoint.point, false);
							ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].items.Add(itemData);
							ItemManager.SendItem.Invoke(ENetReliability.Reliable, Regions.GatherClientConnections(ItemManager.respawnItems_X, ItemManager.respawnItems_Y, ItemManager.ITEM_REGIONS), ItemManager.respawnItems_X, ItemManager.respawnItems_Y, item2.id, item2.amount, item2.quality, item2.state, point, itemData.instanceID, false);
						}
						else if (Assets.shouldLoadAnyAssets)
						{
							UnturnedLog.error(string.Concat(new string[]
							{
								"Failed to respawn an item with ID ",
								item.ToString(),
								" from type ",
								LevelItems.tables[(int)itemSpawnpoint.type].name,
								" [",
								itemSpawnpoint.type.ToString(),
								"]"
							}));
						}
						flag = true;
					}
					IL_2C8:;
				}
				if (flag)
				{
					ItemManager.regions[(int)ItemManager.respawnItems_X, (int)ItemManager.respawnItems_Y].lastRespawn = Time.realtimeSinceStartup;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x000BF680 File Offset: 0x000BD880
		private void generateItems(byte x, byte y)
		{
			if (Level.info == null || Level.info.type == ELevelType.ARENA)
			{
				return;
			}
			List<ItemData> list = new List<ItemData>();
			if (LevelItems.spawns[(int)x, (int)y].Count > 0)
			{
				List<ItemSpawnpoint> list2 = new List<ItemSpawnpoint>();
				for (int i = 0; i < LevelItems.spawns[(int)x, (int)y].Count; i++)
				{
					ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int)x, (int)y][i];
					if (SafezoneManager.checkPointValid(itemSpawnpoint.point))
					{
						list2.Add(itemSpawnpoint);
					}
				}
				while ((float)list.Count < (float)LevelItems.spawns[(int)x, (int)y].Count * Provider.modeConfigData.Items.Spawn_Chance && list2.Count > 0)
				{
					int num = Random.Range(0, list2.Count);
					ItemSpawnpoint itemSpawnpoint2 = list2[num];
					list2.RemoveAt(num);
					ushort item = LevelItems.getItem(itemSpawnpoint2);
					if (Assets.find(EAssetType.ITEM, item) is ItemAsset)
					{
						Item item2 = new Item(item, EItemOrigin.WORLD);
						Vector3 point = itemSpawnpoint2.point;
						bool flag = true;
						ServerSpawningItemDropHandler serverSpawningItemDropHandler = ItemManager.onServerSpawningItemDrop;
						if (serverSpawningItemDropHandler != null)
						{
							serverSpawningItemDropHandler(item2, ref point, ref flag);
						}
						if (flag)
						{
							list.Add(new ItemData(item2, ItemManager.instanceCount += 1U, point, false));
						}
					}
					else if (Assets.shouldLoadAnyAssets)
					{
						UnturnedLog.error(string.Concat(new string[]
						{
							"Failed to generate an item with ID ",
							item.ToString(),
							" from type ",
							LevelItems.tables[(int)itemSpawnpoint2.type].name,
							" [",
							itemSpawnpoint2.type.ToString(),
							"]"
						}));
					}
				}
			}
			for (int j = 0; j < ItemManager.regions[(int)x, (int)y].items.Count; j++)
			{
				if (ItemManager.regions[(int)x, (int)y].items[j].isDropped)
				{
					list.Add(ItemManager.regions[(int)x, (int)y].items[j]);
				}
			}
			ItemManager.regions[(int)x, (int)y].items = list;
		}

		/// <summary>
		/// Not ideal, but there was a problem because onLevelLoaded was not resetting these after disconnecting.
		/// </summary>
		// Token: 0x06002C46 RID: 11334 RVA: 0x000BF8B0 File Offset: 0x000BDAB0
		internal static void ClearNetworkStuff()
		{
			ItemManager.pendingInstantiations = new List<ItemInstantiationParameters>();
			ItemManager.instantiationsToInsert = new List<ItemInstantiationParameters>();
			ItemManager.regionsPendingDestroy = new List<ItemRegion>();
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x000BF8D0 File Offset: 0x000BDAD0
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP)
			{
				ItemManager.regions = new ItemRegion[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						ItemManager.regions[(int)b, (int)b2] = new ItemRegion();
					}
				}
				ItemManager.clampedItems = new List<InteractableItem>();
				ItemManager.instanceCount = 0U;
				ItemManager.clampItemIndex = 0;
				ItemManager.despawnItems_X = 0;
				ItemManager.despawnItems_Y = 0;
				ItemManager.respawnItems_X = 0;
				ItemManager.respawnItems_Y = 0;
				for (byte b3 = 0; b3 < Regions.WORLD_SIZE; b3 += 1)
				{
					for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
					{
						this.generateItems(b3, b4);
					}
				}
			}
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x000BF988 File Offset: 0x000BDB88
		private void onRegionActivated(byte x, byte y)
		{
			if (ItemManager.regions != null && ItemManager.regions[(int)x, (int)y] != null)
			{
				for (int i = 0; i < ItemManager.regions[(int)x, (int)y].drops.Count; i++)
				{
					ItemDrop itemDrop = ItemManager.regions[(int)x, (int)y].drops[i];
					if (itemDrop != null && !(itemDrop.interactableItem == null))
					{
						Rigidbody component = itemDrop.interactableItem.GetComponent<Rigidbody>();
						if (!(component == null))
						{
							component.useGravity = true;
							component.isKinematic = false;
						}
					}
				}
			}
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x000BFA18 File Offset: 0x000BDC18
		private void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y, byte step, ref bool canIncrementIndex)
		{
			if (step == 0)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						if (player.channel.IsLocalPlayer && ItemManager.regions[(int)b, (int)b2].isNetworked && !Regions.checkArea(b, b2, new_x, new_y, ItemManager.ITEM_REGIONS))
						{
							if (ItemManager.regions[(int)b, (int)b2].drops.Count > 0)
							{
								ItemManager.regions[(int)b, (int)b2].isPendingDestroy = true;
								ItemManager.regionsPendingDestroy.Add(ItemManager.regions[(int)b, (int)b2]);
							}
							ItemManager.CancelInstantiationsInRegion(b, b2);
							ItemManager.regions[(int)b, (int)b2].isNetworked = false;
						}
						if (Provider.isServer && player.movement.loadedRegions[(int)b, (int)b2].isItemsLoaded && !Regions.checkArea(b, b2, new_x, new_y, ItemManager.ITEM_REGIONS))
						{
							player.movement.loadedRegions[(int)b, (int)b2].isItemsLoaded = false;
						}
					}
				}
			}
			if (step == 5 && Provider.isServer && Regions.checkSafe((int)new_x, (int)new_y))
			{
				Vector3 position = player.transform.position;
				for (int i = (int)(new_x - ItemManager.ITEM_REGIONS); i <= (int)(new_x + ItemManager.ITEM_REGIONS); i++)
				{
					for (int j = (int)(new_y - ItemManager.ITEM_REGIONS); j <= (int)(new_y + ItemManager.ITEM_REGIONS); j++)
					{
						if (Regions.checkSafe((int)((byte)i), (int)((byte)j)) && !player.movement.loadedRegions[i, j].isItemsLoaded)
						{
							if (player.channel.IsLocalPlayer)
							{
								this.generateItems((byte)i, (byte)j);
							}
							player.movement.loadedRegions[i, j].isItemsLoaded = true;
							float sortOrder = Regions.HorizontalDistanceFromCenterSquared(i, j, position);
							this.askItems(player.channel.owner.transportConnection, (byte)i, (byte)j, sortOrder);
						}
					}
				}
			}
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x000BFC1F File Offset: 0x000BDE1F
		private void onPlayerCreated(Player player)
		{
			PlayerMovement movement = player.movement;
			movement.onRegionUpdated = (PlayerRegionUpdated)Delegate.Combine(movement.onRegionUpdated, new PlayerRegionUpdated(this.onRegionUpdated));
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x000BFC48 File Offset: 0x000BDE48
		private static void DestroyAllInRegion(ItemRegion region)
		{
			if (region.drops.Count > 0)
			{
				region.DestroyAll();
			}
			if (region.isPendingDestroy)
			{
				region.isPendingDestroy = false;
				ItemManager.regionsPendingDestroy.RemoveFast(region);
			}
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x000BFC7C File Offset: 0x000BDE7C
		private static void CancelInstantiationsInRegion(byte x, byte y)
		{
			for (int i = ItemManager.pendingInstantiations.Count - 1; i >= 0; i--)
			{
				if (ItemManager.pendingInstantiations[i].region_x == x && ItemManager.pendingInstantiations[i].region_y == y)
				{
					ItemManager.pendingInstantiations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x000BFCD4 File Offset: 0x000BDED4
		private static void CancelInstantiationByInstanceId(uint instanceId)
		{
			for (int i = ItemManager.pendingInstantiations.Count - 1; i >= 0; i--)
			{
				if (ItemManager.pendingInstantiations[i].instanceID == instanceId)
				{
					ItemManager.pendingInstantiations.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x000BFD18 File Offset: 0x000BDF18
		private void Update()
		{
			if (!Provider.isServer && ItemManager.clampedItems != null && ItemManager.clampedItems.Count > 0)
			{
				if (ItemManager.clampItemIndex >= ItemManager.clampedItems.Count)
				{
					ItemManager.clampItemIndex = 0;
				}
				InteractableItem interactableItem = ItemManager.clampedItems[ItemManager.clampItemIndex];
				if (interactableItem != null)
				{
					interactableItem.clampRange();
					ItemManager.clampItemIndex++;
				}
				else
				{
					ItemManager.clampedItems.RemoveAtFast(ItemManager.clampItemIndex);
				}
			}
			if (!Level.isLoaded)
			{
				return;
			}
			bool flag;
			do
			{
				flag = this.despawnItems();
				ItemManager.despawnItems_X += 1;
				if (ItemManager.despawnItems_X >= Regions.WORLD_SIZE)
				{
					ItemManager.despawnItems_X = 0;
					ItemManager.despawnItems_Y += 1;
					if (ItemManager.despawnItems_Y >= Regions.WORLD_SIZE)
					{
						goto Block_8;
					}
				}
			}
			while (!flag);
			for (;;)
			{
				IL_C0:
				bool flag2 = this.respawnItems();
				ItemManager.respawnItems_X += 1;
				if (ItemManager.respawnItems_X >= Regions.WORLD_SIZE)
				{
					ItemManager.respawnItems_X = 0;
					ItemManager.respawnItems_Y += 1;
					if (ItemManager.respawnItems_Y >= Regions.WORLD_SIZE)
					{
						break;
					}
				}
				if (flag2)
				{
					return;
				}
			}
			ItemManager.respawnItems_Y = 0;
			return;
			Block_8:
			ItemManager.despawnItems_Y = 0;
			goto IL_C0;
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x000BFE30 File Offset: 0x000BE030
		private void Start()
		{
			ItemManager.manager = this;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			LevelObjects.onRegionActivated = (RegionActivated)Delegate.Combine(LevelObjects.onRegionActivated, new RegionActivated(this.onRegionActivated));
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x000BFEC4 File Offset: 0x000BE0C4
		private void OnLogMemoryUsage(List<string> results)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			ItemRegion[,] regions = ItemManager.regions;
			int upperBound = regions.GetUpperBound(0);
			int upperBound2 = regions.GetUpperBound(1);
			for (int i = regions.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = regions.GetLowerBound(1); j <= upperBound2; j++)
				{
					ItemRegion itemRegion = regions[i, j];
					if (itemRegion.items.Count > 0)
					{
						num++;
					}
					num2 += itemRegion.items.Count;
					if (itemRegion.drops.Count > 0)
					{
						num3++;
					}
					num4 += itemRegion.drops.Count;
				}
			}
			results.Add(string.Format("Item regions: {0}", num));
			results.Add(string.Format("Dropped items: {0}", num2));
			results.Add(string.Format("Item regions with physical items: {0}", num3));
			results.Add(string.Format("Dropped items with physics: {0}", num4));
		}

		// Token: 0x040017BA RID: 6074
		public static readonly byte ITEM_REGIONS = 1;

		// Token: 0x040017BB RID: 6075
		public static ServerSpawningItemDropHandler onServerSpawningItemDrop;

		// Token: 0x040017BC RID: 6076
		public static TakeItemRequestHandler onTakeItemRequested;

		// Token: 0x040017BD RID: 6077
		public static ItemDropAdded onItemDropAdded;

		// Token: 0x040017BE RID: 6078
		public static ItemDropRemoved onItemDropRemoved;

		// Token: 0x040017BF RID: 6079
		private static ItemManager manager;

		/// <summary>
		/// List of all interactable items. Originally only used to clamp their distance from the drop point to ensure
		/// clients can always pick them up, but now used to find items within a radius for nearby menu as well.
		/// </summary>
		// Token: 0x040017C1 RID: 6081
		public static List<InteractableItem> clampedItems;

		// Token: 0x040017C2 RID: 6082
		private static List<ItemInstantiationParameters> pendingInstantiations = new List<ItemInstantiationParameters>();

		// Token: 0x040017C3 RID: 6083
		private static List<ItemInstantiationParameters> instantiationsToInsert = new List<ItemInstantiationParameters>();

		// Token: 0x040017C4 RID: 6084
		private static List<ItemRegion> regionsPendingDestroy = new List<ItemRegion>();

		// Token: 0x040017C5 RID: 6085
		private static uint instanceCount;

		// Token: 0x040017C6 RID: 6086
		private static int clampItemIndex;

		// Token: 0x040017C7 RID: 6087
		private static byte despawnItems_X;

		// Token: 0x040017C8 RID: 6088
		private static byte despawnItems_Y;

		// Token: 0x040017C9 RID: 6089
		private static byte respawnItems_X;

		// Token: 0x040017CA RID: 6090
		private static byte respawnItems_Y;

		// Token: 0x040017CB RID: 6091
		private static readonly ClientStaticMethod<byte, byte, uint, bool> SendDestroyItem = ClientStaticMethod<byte, byte, uint, bool>.Get(new ClientStaticMethod<byte, byte, uint, bool>.ReceiveDelegate(ItemManager.ReceiveDestroyItem));

		// Token: 0x040017CC RID: 6092
		private static readonly ServerStaticMethod<byte, byte, uint, byte, byte, byte, byte> SendTakeItemRequest = ServerStaticMethod<byte, byte, uint, byte, byte, byte, byte>.Get(new ServerStaticMethod<byte, byte, uint, byte, byte, byte, byte>.ReceiveDelegateWithContext(ItemManager.ReceiveTakeItemRequest));

		// Token: 0x040017CD RID: 6093
		private static readonly ClientStaticMethod<byte, byte> SendClearRegionItems = ClientStaticMethod<byte, byte>.Get(new ClientStaticMethod<byte, byte>.ReceiveDelegate(ItemManager.ReceiveClearRegionItems));

		// Token: 0x040017CE RID: 6094
		private static List<RegionCoordinate> clearItemRegions = new List<RegionCoordinate>(4);

		// Token: 0x040017CF RID: 6095
		private static readonly ClientStaticMethod<byte, byte, ushort, byte, byte, byte[], Vector3, uint, bool> SendItem = ClientStaticMethod<byte, byte, ushort, byte, byte, byte[], Vector3, uint, bool>.Get(new ClientStaticMethod<byte, byte, ushort, byte, byte, byte[], Vector3, uint, bool>.ReceiveDelegate(ItemManager.ReceiveItem));

		// Token: 0x040017D0 RID: 6096
		private static readonly ClientStaticMethod SendItems = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(ItemManager.ReceiveItems));
	}
}
