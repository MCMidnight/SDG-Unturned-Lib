using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000581 RID: 1409
	public class ObjectManager : SteamCaller
	{
		// Token: 0x06002D0A RID: 11530 RVA: 0x000C3468 File Offset: 0x000C1668
		public static void getObjectsInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search, List<Transform> result)
		{
			if (LevelObjects.objects == null)
			{
				return;
			}
			for (int i = 0; i < search.Count; i++)
			{
				RegionCoordinate regionCoordinate = search[i];
				if (LevelObjects.objects[(int)regionCoordinate.x, (int)regionCoordinate.y] != null)
				{
					for (int j = 0; j < LevelObjects.objects[(int)regionCoordinate.x, (int)regionCoordinate.y].Count; j++)
					{
						LevelObject levelObject = LevelObjects.objects[(int)regionCoordinate.x, (int)regionCoordinate.y][j];
						if (!(levelObject.transform == null) && (levelObject.transform.position - center).sqrMagnitude < sqrRadius)
						{
							result.Add(levelObject.transform);
						}
					}
				}
			}
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000C352E File Offset: 0x000C172E
		[Obsolete]
		public void tellObjectRubble(CSteamID steamID, byte x, byte y, ushort index, byte section, bool isAlive, Vector3 ragdoll)
		{
			ObjectManager.ReceiveObjectRubble(x, y, index, section, isAlive, ragdoll);
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000C3540 File Offset: 0x000C1740
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellObjectRubble")]
		public static void ReceiveObjectRubble(byte x, byte y, ushort index, byte section, bool isAlive, Vector3 ragdoll)
		{
			if (!Regions.checkSafe((int)x, (int)y))
			{
				return;
			}
			ObjectRegion objectRegion = ObjectManager.regions[(int)x, (int)y];
			if (objectRegion == null)
			{
				return;
			}
			if (!Provider.isServer && !objectRegion.isNetworked)
			{
				return;
			}
			if (LevelObjects.objects == null)
			{
				return;
			}
			List<LevelObject> list = LevelObjects.objects[(int)x, (int)y];
			if (list == null || (int)index >= list.Count)
			{
				return;
			}
			LevelObject levelObject = list[(int)index];
			if (levelObject == null)
			{
				return;
			}
			InteractableObjectRubble rubble = levelObject.rubble;
			if (rubble != null)
			{
				rubble.updateRubble(section, isAlive, true, ragdoll);
			}
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000C35C2 File Offset: 0x000C17C2
		private static void trackKill()
		{
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000C35C4 File Offset: 0x000C17C4
		public static void damage(Transform obj, Vector3 direction, byte section, float damage, float times, out EPlayerKill kill, out uint xp, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown, bool trackKill = true)
		{
			kill = EPlayerKill.NONE;
			xp = 0U;
			ushort num = (ushort)(damage * times);
			bool flag = true;
			DamageObjectRequestHandler damageObjectRequestHandler = ObjectManager.onDamageObjectRequested;
			if (damageObjectRequestHandler != null)
			{
				damageObjectRequestHandler(instigatorSteamID, obj, section, ref num, ref flag, damageOrigin);
			}
			if (!flag || num < 1)
			{
				return;
			}
			byte b;
			byte b2;
			ushort num2;
			if (ObjectManager.tryGetRegion(obj, out b, out b2, out num2))
			{
				LevelObject levelObject = LevelObjects.objects[(int)b, (int)b2][(int)num2];
				if (levelObject != null && levelObject.rubble != null && levelObject.canDamageRubble)
				{
					InteractableObjectRubble rubble = levelObject.rubble;
					if (rubble.IsSectionIndexValid(section) && !rubble.isSectionDead(section))
					{
						rubble.askDamage(section, num);
						if (rubble.isSectionDead(section))
						{
							kill = EPlayerKill.OBJECT;
							if (levelObject.asset != null)
							{
								xp = levelObject.asset.rubbleRewardXP;
							}
							byte[] state = levelObject.state;
							if (section == 255)
							{
								state[state.Length - 1] = 0;
							}
							else
							{
								state[state.Length - 1] = (state[state.Length - 1] & ~Types.SHIFTS[(int)section]);
							}
							ObjectManager.SendObjectRubble.InvokeAndLoopback(ENetReliability.Reliable, ObjectManager.GatherRemoteClientConnections(b, b2), b, b2, num2, section, false, direction * (float)num);
						}
						if (trackKill && levelObject.asset != null && rubble.isAllDead())
						{
							Vector3 position = obj.position;
							byte nav;
							LevelNavigation.tryGetBounds(position, out nav);
							Guid guid = levelObject.asset.GUID;
							for (int i = 0; i < Provider.clients.Count; i++)
							{
								SteamPlayer steamPlayer = Provider.clients[i];
								if (!(steamPlayer.player == null) && !(steamPlayer.player.movement == null) && !(steamPlayer.player.life == null) && !steamPlayer.player.life.isDead && (steamPlayer.player.transform.position - position).sqrMagnitude < 90000f)
								{
									steamPlayer.player.quests.trackObjectKill(guid, nav);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000C37EC File Offset: 0x000C19EC
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 5)]
		public static void ReceiveTalkWithNpcRequest(in ServerInvocationContext context, NetId netId)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			player.quests.ClearActiveNpc();
			InteractableObjectNPC npcFromObjectNetId = InteractableObjectNPC.GetNpcFromObjectNetId(netId);
			if (npcFromObjectNetId == null)
			{
				return;
			}
			if ((npcFromObjectNetId.transform.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			if (!npcFromObjectNetId.objectAsset.areConditionsMet(player))
			{
				return;
			}
			DialogueAsset dialogueAsset = npcFromObjectNetId.npcAsset.FindDialogueAsset();
			if (dialogueAsset == null)
			{
				return;
			}
			player.quests.ApproveTalkWithNpcRequest(npcFromObjectNetId, dialogueAsset);
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000C388C File Offset: 0x000C1A8C
		public static void useObjectQuest(Transform transform)
		{
			byte arg;
			byte arg2;
			ushort arg3;
			if (ObjectManager.tryGetRegion(transform, out arg, out arg2, out arg3))
			{
				ObjectManager.SendUseObjectQuest.Invoke(ENetReliability.Reliable, arg, arg2, arg3);
			}
		}

		/// <summary>
		/// Invoked when askUseObjectQuest succeeds.
		/// </summary>
		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06002D11 RID: 11537 RVA: 0x000C38B8 File Offset: 0x000C1AB8
		// (remove) Token: 0x06002D12 RID: 11538 RVA: 0x000C38EC File Offset: 0x000C1AEC
		public static event Action<Player, InteractableObject> OnQuestObjectUsed;

		// Token: 0x06002D13 RID: 11539 RVA: 0x000C3920 File Offset: 0x000C1B20
		[Obsolete]
		public void askUseObjectQuest(CSteamID steamID, byte x, byte y, ushort index)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			ObjectManager.ReceiveUseObjectQuest(serverInvocationContext, x, y, index);
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000C3940 File Offset: 0x000C1B40
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 10, legacyName = "askUseObjectQuest")]
		public static void ReceiveUseObjectQuest(in ServerInvocationContext context, byte x, byte y, ushort index)
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
			if ((int)index >= LevelObjects.objects[(int)x, (int)y].Count)
			{
				return;
			}
			if (LevelObjects.objects[(int)x, (int)y][(int)index] == null || LevelObjects.objects[(int)x, (int)y][(int)index].transform == null)
			{
				return;
			}
			if ((LevelObjects.objects[(int)x, (int)y][(int)index].transform.position - player.transform.position).sqrMagnitude > 1600f)
			{
				return;
			}
			InteractableObject interactable = LevelObjects.objects[(int)x, (int)y][(int)index].interactable;
			if (interactable != null && (interactable is InteractableObjectQuest || interactable is InteractableObjectNote))
			{
				if (!interactable.objectAsset.areConditionsMet(player))
				{
					return;
				}
				if (!interactable.objectAsset.areInteractabilityConditionsMet(player))
				{
					return;
				}
				interactable.objectAsset.ApplyInteractabilityConditions(player);
				interactable.objectAsset.GrantInteractabilityRewards(player);
				ObjectManager.OnQuestObjectUsed.TryInvoke("OnQuestObjectUsed", player, interactable);
				((InteractableObjectTriggerableBase)interactable).InvokeUsedEventForModHooks();
			}
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000C3A80 File Offset: 0x000C1C80
		public static void useObjectDropper(Transform transform)
		{
			byte arg;
			byte arg2;
			ushort arg3;
			if (ObjectManager.tryGetRegion(transform, out arg, out arg2, out arg3))
			{
				ObjectManager.SendUseObjectDropper.Invoke(ENetReliability.Unreliable, arg, arg2, arg3);
			}
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000C3AAC File Offset: 0x000C1CAC
		[Obsolete]
		public void askUseObjectDropper(CSteamID steamID, byte x, byte y, ushort index)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			ObjectManager.ReceiveUseObjectDropper(serverInvocationContext, x, y, index);
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000C3ACC File Offset: 0x000C1CCC
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 5, legacyName = "askUseObjectDropper")]
		public static void ReceiveUseObjectDropper(in ServerInvocationContext context, byte x, byte y, ushort index)
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
			if ((int)index >= LevelObjects.objects[(int)x, (int)y].Count)
			{
				return;
			}
			if (LevelObjects.objects[(int)x, (int)y][(int)index] == null || LevelObjects.objects[(int)x, (int)y][(int)index].transform == null)
			{
				return;
			}
			if ((LevelObjects.objects[(int)x, (int)y][(int)index].transform.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			InteractableObjectDropper interactableObjectDropper = LevelObjects.objects[(int)x, (int)y][(int)index].interactable as InteractableObjectDropper;
			if (interactableObjectDropper != null && interactableObjectDropper.isUsable)
			{
				if (!interactableObjectDropper.objectAsset.areConditionsMet(player))
				{
					return;
				}
				if (!interactableObjectDropper.objectAsset.areInteractabilityConditionsMet(player))
				{
					return;
				}
				interactableObjectDropper.objectAsset.ApplyInteractabilityConditions(player);
				interactableObjectDropper.objectAsset.GrantInteractabilityRewards(player);
				interactableObjectDropper.drop();
				interactableObjectDropper.InvokeUsedEventForModHooks();
			}
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x000C3BF7 File Offset: 0x000C1DF7
		[Obsolete]
		public void tellObjectResource(CSteamID steamID, byte x, byte y, ushort index, ushort amount)
		{
			ObjectManager.ReceiveObjectResourceState(x, y, index, amount);
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000C3C04 File Offset: 0x000C1E04
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellObjectResource")]
		public static void ReceiveObjectResourceState(byte x, byte y, ushort index, ushort amount)
		{
			if (!Regions.checkSafe((int)x, (int)y))
			{
				return;
			}
			ObjectRegion objectRegion = ObjectManager.regions[(int)x, (int)y];
			if (!Provider.isServer && !objectRegion.isNetworked)
			{
				return;
			}
			if ((int)index >= LevelObjects.objects[(int)x, (int)y].Count)
			{
				return;
			}
			InteractableObjectResource interactableObjectResource = LevelObjects.objects[(int)x, (int)y][(int)index].interactable as InteractableObjectResource;
			if (interactableObjectResource != null)
			{
				interactableObjectResource.updateAmount(amount);
			}
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000C3C7C File Offset: 0x000C1E7C
		public static void updateObjectResource(Transform transform, ushort amount, bool shouldSend)
		{
			byte b;
			byte b2;
			ushort num;
			if (ObjectManager.tryGetRegion(transform, out b, out b2, out num))
			{
				if (shouldSend)
				{
					ObjectManager.SendObjectResourceState.InvokeAndLoopback(ENetReliability.Reliable, ObjectManager.GatherRemoteClientConnections(b, b2), b, b2, num, amount);
				}
				byte[] bytes = BitConverter.GetBytes(amount);
				LevelObjects.objects[(int)b, (int)b2][(int)num].state[0] = bytes[0];
				LevelObjects.objects[(int)b, (int)b2][(int)num].state[1] = bytes[1];
			}
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x000C3CF0 File Offset: 0x000C1EF0
		public static void forceObjectBinaryState(Transform transform, bool isUsed)
		{
			byte b;
			byte b2;
			ushort num;
			if (ObjectManager.tryGetRegion(transform, out b, out b2, out num))
			{
				InteractableObjectBinaryState interactableObjectBinaryState = LevelObjects.objects[(int)b, (int)b2][(int)num].interactable as InteractableObjectBinaryState;
				if (interactableObjectBinaryState != null && interactableObjectBinaryState.isUsable)
				{
					ObjectManager.SendObjectBinaryState.InvokeAndLoopback(ENetReliability.Reliable, ObjectManager.GatherRemoteClientConnections(b, b2), b, b2, num, isUsed);
					LevelObjects.objects[(int)b, (int)b2][(int)num].state[0] = (interactableObjectBinaryState.isUsed ? 1 : 0);
				}
			}
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x000C3D78 File Offset: 0x000C1F78
		public static void toggleObjectBinaryState(Transform transform, bool isUsed)
		{
			byte arg;
			byte arg2;
			ushort arg3;
			if (ObjectManager.tryGetRegion(transform, out arg, out arg2, out arg3))
			{
				ObjectManager.SendToggleObjectBinaryStateRequest.Invoke(ENetReliability.Unreliable, arg, arg2, arg3, isUsed);
			}
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000C3DA2 File Offset: 0x000C1FA2
		[Obsolete]
		public void tellToggleObjectBinaryState(CSteamID steamID, byte x, byte y, ushort index, bool isUsed)
		{
			ObjectManager.ReceiveObjectBinaryState(x, y, index, isUsed);
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000C3DB0 File Offset: 0x000C1FB0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellToggleObjectBinaryState")]
		public static void ReceiveObjectBinaryState(byte x, byte y, ushort index, bool isUsed)
		{
			if (!Regions.checkSafe((int)x, (int)y))
			{
				return;
			}
			ObjectRegion objectRegion = ObjectManager.regions[(int)x, (int)y];
			if (!Provider.isServer && !objectRegion.isNetworked)
			{
				return;
			}
			if ((int)index >= LevelObjects.objects[(int)x, (int)y].Count)
			{
				return;
			}
			InteractableObjectBinaryState interactableObjectBinaryState = LevelObjects.objects[(int)x, (int)y][(int)index].interactable as InteractableObjectBinaryState;
			if (interactableObjectBinaryState != null)
			{
				interactableObjectBinaryState.updateToggle(isUsed);
			}
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000C3E28 File Offset: 0x000C2028
		[Obsolete]
		public void askToggleObjectBinaryState(CSteamID steamID, byte x, byte y, ushort index, bool isUsed)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			ObjectManager.ReceiveToggleObjectBinaryStateRequest(serverInvocationContext, x, y, index, isUsed);
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000C3E4C File Offset: 0x000C204C
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2, legacyName = "askToggleObjectBinaryState")]
		public static void ReceiveToggleObjectBinaryStateRequest(in ServerInvocationContext context, byte x, byte y, ushort index, bool isUsed)
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
			if ((int)index >= LevelObjects.objects[(int)x, (int)y].Count)
			{
				return;
			}
			if (LevelObjects.objects[(int)x, (int)y][(int)index] == null || LevelObjects.objects[(int)x, (int)y][(int)index].transform == null)
			{
				return;
			}
			InteractableObjectBinaryState interactableObjectBinaryState = LevelObjects.objects[(int)x, (int)y][(int)index].interactable as InteractableObjectBinaryState;
			if (interactableObjectBinaryState == null)
			{
				return;
			}
			if (!interactableObjectBinaryState.isUsable)
			{
				return;
			}
			if (interactableObjectBinaryState.isUsed == isUsed)
			{
				return;
			}
			if (interactableObjectBinaryState.modHookCounter <= 0)
			{
				if ((LevelObjects.objects[(int)x, (int)y][(int)index].transform.position - player.transform.position).sqrMagnitude > 400f)
				{
					return;
				}
				if (interactableObjectBinaryState.objectAsset.interactabilityRemote)
				{
					return;
				}
			}
			if (!interactableObjectBinaryState.objectAsset.areConditionsMet(player))
			{
				return;
			}
			if (!interactableObjectBinaryState.objectAsset.areInteractabilityConditionsMet(player))
			{
				return;
			}
			interactableObjectBinaryState.objectAsset.ApplyInteractabilityConditions(player);
			interactableObjectBinaryState.objectAsset.GrantInteractabilityRewards(player);
			ObjectManager.SendObjectBinaryState.InvokeAndLoopback(ENetReliability.Reliable, ObjectManager.GatherRemoteClientConnections(x, y), x, y, index, isUsed);
			LevelObjects.objects[(int)x, (int)y][(int)index].state[0] = (interactableObjectBinaryState.isUsed ? 1 : 0);
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x000C3FCE File Offset: 0x000C21CE
		[Obsolete]
		public void tellClearRegionObjects(CSteamID steamID, byte x, byte y)
		{
			ObjectManager.ReceiveClearRegionObjects(x, y);
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000C3FD8 File Offset: 0x000C21D8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellClearRegionObjects")]
		public static void ReceiveClearRegionObjects(byte x, byte y)
		{
			if (!Provider.isServer && !ObjectManager.regions[(int)x, (int)y].isNetworked)
			{
				return;
			}
			for (int i = 0; i < LevelObjects.objects[(int)x, (int)y].Count; i++)
			{
				LevelObject levelObject = LevelObjects.objects[(int)x, (int)y][i];
				if (levelObject.state != null && levelObject.state.Length != 0)
				{
					levelObject.state = levelObject.asset.getState();
					if (levelObject.interactable != null)
					{
						levelObject.interactable.updateState(levelObject.asset, levelObject.state);
					}
					if (levelObject.rubble != null)
					{
						levelObject.rubble.updateState(levelObject.asset, levelObject.state);
					}
				}
			}
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x000C40A1 File Offset: 0x000C22A1
		public static void askClearRegionObjects(byte x, byte y)
		{
			if (Provider.isServer)
			{
				if (!Regions.checkSafe((int)x, (int)y))
				{
					return;
				}
				if (LevelObjects.objects[(int)x, (int)y].Count > 0)
				{
					ObjectManager.SendClearRegionObjects.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), x, y);
				}
			}
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x000C40DC File Offset: 0x000C22DC
		public static void askClearAllObjects()
		{
			if (Provider.isServer)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						ObjectManager.askClearRegionObjects(b, b2);
					}
				}
			}
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x000C4119 File Offset: 0x000C2319
		[Obsolete]
		public void tellObjects(CSteamID steamID)
		{
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x000C411C File Offset: 0x000C231C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveObjects(in ClientInvocationContext context)
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
			if (ObjectManager.regions[(int)b, (int)b2].isNetworked)
			{
				return;
			}
			ObjectManager.regions[(int)b, (int)b2].isNetworked = true;
			ushort num;
			while (SystemNetPakReaderEx.ReadUInt16(reader, ref num) && num != 65535)
			{
				byte b3;
				SystemNetPakReaderEx.ReadUInt8(reader, ref b3);
				byte[] array = new byte[(int)b3];
				reader.ReadBytes(array);
				LevelObject levelObject = LevelObjects.objects[(int)b, (int)b2][(int)num];
				if (levelObject.interactable != null)
				{
					levelObject.interactable.updateState(levelObject.asset, array);
				}
				if (levelObject.rubble != null)
				{
					levelObject.rubble.updateState(levelObject.asset, array);
				}
			}
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x000C4203 File Offset: 0x000C2403
		[Obsolete]
		public void askObjects(CSteamID steamID, byte x, byte y)
		{
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x000C4208 File Offset: 0x000C2408
		internal void askObjects(ITransportConnection transportConnection, byte x, byte y)
		{
			ObjectManager.SendObjects.Invoke(ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt8(writer, x);
				SystemNetPakWriterEx.WriteUInt8(writer, y);
				ushort num = 0;
				while ((int)num < LevelObjects.objects[(int)x, (int)y].Count)
				{
					LevelObject levelObject = LevelObjects.objects[(int)x, (int)y][(int)num];
					if (levelObject.state != null && levelObject.state.Length != 0)
					{
						SystemNetPakWriterEx.WriteUInt16(writer, num);
						byte b = (byte)levelObject.state.Length;
						SystemNetPakWriterEx.WriteUInt8(writer, b);
						writer.WriteBytes(levelObject.state, (int)b);
					}
					num += 1;
				}
				SystemNetPakWriterEx.WriteUInt16(writer, ushort.MaxValue);
			});
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x000C4244 File Offset: 0x000C2444
		public static LevelObject getObject(byte x, byte y, ushort index)
		{
			if (!Regions.checkSafe((int)x, (int)y))
			{
				return null;
			}
			List<LevelObject> list = LevelObjects.objects[(int)x, (int)y];
			if ((int)index >= list.Count)
			{
				return null;
			}
			return list[(int)index];
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x000C427C File Offset: 0x000C247C
		public static bool tryGetRegion(Transform transform, out byte x, out byte y, out ushort index)
		{
			x = 0;
			y = 0;
			index = 0;
			if (Regions.tryGetCoordinate(transform.position, out x, out y))
			{
				List<LevelObject> list = LevelObjects.objects[(int)x, (int)y];
				index = 0;
				while ((int)index < list.Count)
				{
					if (transform == list[(int)index].transform)
					{
						return true;
					}
					index += 1;
				}
			}
			return false;
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x000C42E0 File Offset: 0x000C24E0
		private bool updateObjects()
		{
			if (Level.info == null || Level.info.type == ELevelType.ARENA)
			{
				return false;
			}
			if (LevelObjects.objects[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].Count <= 0)
			{
				return true;
			}
			if ((int)ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex >= LevelObjects.objects[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].Count)
			{
				ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex = (ushort)(LevelObjects.objects[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].Count - 1);
			}
			LevelObject levelObject = LevelObjects.objects[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y][(int)ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex];
			if (levelObject == null || levelObject.asset == null)
			{
				return false;
			}
			if (levelObject.interactable != null && levelObject.asset.interactabilityReset >= 1f)
			{
				if (levelObject.asset.interactability == EObjectInteractability.BINARY_STATE)
				{
					if (((InteractableObjectBinaryState)levelObject.interactable).checkCanReset(Provider.modeConfigData.Objects.Binary_State_Reset_Multiplier))
					{
						ObjectManager.SendObjectBinaryState.InvokeAndLoopback(ENetReliability.Reliable, ObjectManager.GatherRemoteClientConnections(ObjectManager.updateObjects_X, ObjectManager.updateObjects_Y), ObjectManager.updateObjects_X, ObjectManager.updateObjects_Y, ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex, false);
						LevelObjects.objects[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y][(int)ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex].state[0] = 0;
					}
				}
				else if ((levelObject.asset.interactability == EObjectInteractability.WATER || levelObject.asset.interactability == EObjectInteractability.FUEL) && ((InteractableObjectResource)levelObject.interactable).checkCanReset((levelObject.asset.interactability == EObjectInteractability.WATER) ? Provider.modeConfigData.Objects.Water_Reset_Multiplier : Provider.modeConfigData.Objects.Fuel_Reset_Multiplier))
				{
					ushort num = (ushort)Mathf.Min((int)(((InteractableObjectResource)levelObject.interactable).amount + ((levelObject.asset.interactability == EObjectInteractability.WATER) ? 1 : 500)), (int)((InteractableObjectResource)levelObject.interactable).capacity);
					ObjectManager.SendObjectResourceState.InvokeAndLoopback(ENetReliability.Reliable, ObjectManager.GatherRemoteClientConnections(ObjectManager.updateObjects_X, ObjectManager.updateObjects_Y), ObjectManager.updateObjects_X, ObjectManager.updateObjects_Y, ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex, num);
					byte[] bytes = BitConverter.GetBytes(num);
					LevelObjects.objects[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y][(int)ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex].state[0] = bytes[0];
					LevelObjects.objects[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y][(int)ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex].state[1] = bytes[1];
				}
			}
			if (levelObject.rubble != null && levelObject.asset.rubbleReset >= 1f && levelObject.asset.rubble == EObjectRubble.DESTROY)
			{
				byte b = levelObject.rubble.checkCanReset(Provider.modeConfigData.Objects.Rubble_Reset_Multiplier);
				if (b != 255)
				{
					byte[] state = LevelObjects.objects[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y][(int)ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex].state;
					state[state.Length - 1] = (state[state.Length - 1] | Types.SHIFTS[(int)b]);
					ObjectManager.SendObjectRubble.InvokeAndLoopback(ENetReliability.Reliable, ObjectManager.GatherRemoteClientConnections(ObjectManager.updateObjects_X, ObjectManager.updateObjects_Y), ObjectManager.updateObjects_X, ObjectManager.updateObjects_Y, ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex, b, true, Vector3.zero);
				}
			}
			return false;
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x000C46F8 File Offset: 0x000C28F8
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP)
			{
				ObjectManager.regions = new ObjectRegion[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						ObjectManager.regions[(int)b, (int)b2] = new ObjectRegion();
					}
				}
				ObjectManager.updateObjects_X = 0;
				ObjectManager.updateObjects_Y = 0;
				if (Provider.isServer)
				{
					ObjectManager.load();
				}
			}
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000C476C File Offset: 0x000C296C
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
							if (player.movement.loadedRegions[(int)b, (int)b2].isObjectsLoaded && !Regions.checkArea(b, b2, new_x, new_y, ObjectManager.OBJECT_REGIONS))
							{
								player.movement.loadedRegions[(int)b, (int)b2].isObjectsLoaded = false;
							}
						}
						else if (player.channel.IsLocalPlayer && ObjectManager.regions[(int)b, (int)b2].isNetworked && !Regions.checkArea(b, b2, new_x, new_y, ObjectManager.OBJECT_REGIONS))
						{
							ObjectManager.regions[(int)b, (int)b2].isNetworked = false;
						}
					}
				}
			}
			if (step == 4 && Regions.checkSafe((int)new_x, (int)new_y))
			{
				for (int i = (int)(new_x - ObjectManager.OBJECT_REGIONS); i <= (int)(new_x + ObjectManager.OBJECT_REGIONS); i++)
				{
					for (int j = (int)(new_y - ObjectManager.OBJECT_REGIONS); j <= (int)(new_y + ObjectManager.OBJECT_REGIONS); j++)
					{
						if (Regions.checkSafe((int)((byte)i), (int)((byte)j)) && !player.movement.loadedRegions[i, j].isObjectsLoaded)
						{
							player.movement.loadedRegions[i, j].isObjectsLoaded = true;
							this.askObjects(player.channel.owner.transportConnection, (byte)i, (byte)j);
						}
					}
				}
			}
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x000C48DE File Offset: 0x000C2ADE
		private void onPlayerCreated(Player player)
		{
			PlayerMovement movement = player.movement;
			movement.onRegionUpdated = (PlayerRegionUpdated)Delegate.Combine(movement.onRegionUpdated, new PlayerRegionUpdated(this.onRegionUpdated));
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x000C4908 File Offset: 0x000C2B08
		private void Update()
		{
			if (!Level.isLoaded)
			{
				return;
			}
			if (!Provider.isServer)
			{
				return;
			}
			bool flag = true;
			while (flag)
			{
				flag = this.updateObjects();
				ObjectRegion objectRegion = ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y];
				objectRegion.updateObjectIndex += 1;
				if ((int)ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex >= LevelObjects.objects[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].Count)
				{
					ObjectManager.regions[(int)ObjectManager.updateObjects_X, (int)ObjectManager.updateObjects_Y].updateObjectIndex = 0;
				}
				ObjectManager.updateObjects_X += 1;
				if (ObjectManager.updateObjects_X >= Regions.WORLD_SIZE)
				{
					ObjectManager.updateObjects_X = 0;
					ObjectManager.updateObjects_Y += 1;
					if (ObjectManager.updateObjects_Y >= Regions.WORLD_SIZE)
					{
						ObjectManager.updateObjects_Y = 0;
						flag = false;
					}
				}
			}
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000C49EC File Offset: 0x000C2BEC
		private void Start()
		{
			ObjectManager.manager = this;
			Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x000C4A40 File Offset: 0x000C2C40
		public static void load()
		{
			if (LevelSavedata.fileExists("/Objects.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				River river = LevelSavedata.openRiver("/Objects.dat", true);
				river.readByte();
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						ObjectManager.loadRegion(river, LevelObjects.objects[(int)b, (int)b2]);
					}
				}
				river.closeRiver();
			}
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x000C4AB4 File Offset: 0x000C2CB4
		public static void save()
		{
			River river = LevelSavedata.openRiver("/Objects.dat", false);
			river.writeByte(ObjectManager.SAVEDATA_VERSION);
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					ObjectManager.saveRegion(river, LevelObjects.objects[(int)b, (int)b2]);
				}
			}
			river.closeRiver();
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x000C4B14 File Offset: 0x000C2D14
		private static void loadRegion(River river, List<LevelObject> objects)
		{
			for (;;)
			{
				ushort num = river.readUInt16();
				if (num == 65535)
				{
					return;
				}
				ushort num2 = river.readUInt16();
				byte[] array = river.readBytes();
				if ((int)num >= objects.Count)
				{
					break;
				}
				LevelObject levelObject = objects[(int)num];
				if (num2 == levelObject.id)
				{
					levelObject.state = array;
					if (!(levelObject.transform == null) && levelObject.asset != null)
					{
						if (levelObject.interactable != null)
						{
							if (levelObject.interactable is InteractableObjectBinaryState)
							{
								if (levelObject.asset.interactabilityReset >= 1f)
								{
									array[0] = 0;
								}
							}
							else if (levelObject.interactable is InteractableObjectResource)
							{
								if (levelObject.asset.rubble == EObjectRubble.DESTROY)
								{
									if (array.Length < 3)
									{
										array = levelObject.asset.getState();
										levelObject.state = array;
									}
								}
								else if (array.Length < 2)
								{
									array = levelObject.asset.getState();
									levelObject.state = array;
								}
							}
							levelObject.interactable.updateState(levelObject.asset, array);
						}
						if (levelObject.rubble != null)
						{
							array[array.Length - 1] = byte.MaxValue;
							levelObject.rubble.updateState(levelObject.asset, array);
						}
					}
				}
			}
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000C4C44 File Offset: 0x000C2E44
		private static void saveRegion(River river, List<LevelObject> objects)
		{
			ushort num = 0;
			while ((int)num < objects.Count)
			{
				LevelObject levelObject = objects[(int)num];
				if (levelObject.state != null && levelObject.state.Length != 0)
				{
					river.writeUInt16(num);
					river.writeUInt16(levelObject.id);
					river.writeBytes(levelObject.state);
				}
				num += 1;
			}
			river.writeUInt16(ushort.MaxValue);
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000C4CA6 File Offset: 0x000C2EA6
		public static PooledTransportConnectionList GatherRemoteClientConnections(byte x, byte y)
		{
			return Regions.GatherRemoteClientConnections(x, y, ObjectManager.OBJECT_REGIONS);
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000C4CB4 File Offset: 0x000C2EB4
		[Obsolete("Replaced by GatherRemoteClients")]
		public static IEnumerable<ITransportConnection> EnumerateClients_Remote(byte x, byte y)
		{
			return ObjectManager.GatherRemoteClientConnections(x, y);
		}

		// Token: 0x04001853 RID: 6227
		public static readonly byte SAVEDATA_VERSION = 1;

		// Token: 0x04001854 RID: 6228
		public static readonly byte OBJECT_REGIONS = 2;

		// Token: 0x04001855 RID: 6229
		public static DamageObjectRequestHandler onDamageObjectRequested;

		// Token: 0x04001856 RID: 6230
		private static ObjectManager manager;

		// Token: 0x04001857 RID: 6231
		private static ObjectRegion[,] regions;

		// Token: 0x04001858 RID: 6232
		private static byte updateObjects_X;

		// Token: 0x04001859 RID: 6233
		private static byte updateObjects_Y;

		// Token: 0x0400185A RID: 6234
		private static readonly ClientStaticMethod<byte, byte, ushort, byte, bool, Vector3> SendObjectRubble = ClientStaticMethod<byte, byte, ushort, byte, bool, Vector3>.Get(new ClientStaticMethod<byte, byte, ushort, byte, bool, Vector3>.ReceiveDelegate(ObjectManager.ReceiveObjectRubble));

		// Token: 0x0400185B RID: 6235
		internal static readonly ServerStaticMethod<NetId> SendTalkWithNpcRequest = ServerStaticMethod<NetId>.Get(new ServerStaticMethod<NetId>.ReceiveDelegateWithContext(ObjectManager.ReceiveTalkWithNpcRequest));

		// Token: 0x0400185D RID: 6237
		private static readonly ServerStaticMethod<byte, byte, ushort> SendUseObjectQuest = ServerStaticMethod<byte, byte, ushort>.Get(new ServerStaticMethod<byte, byte, ushort>.ReceiveDelegateWithContext(ObjectManager.ReceiveUseObjectQuest));

		// Token: 0x0400185E RID: 6238
		private static readonly ServerStaticMethod<byte, byte, ushort> SendUseObjectDropper = ServerStaticMethod<byte, byte, ushort>.Get(new ServerStaticMethod<byte, byte, ushort>.ReceiveDelegateWithContext(ObjectManager.ReceiveUseObjectDropper));

		// Token: 0x0400185F RID: 6239
		private static readonly ClientStaticMethod<byte, byte, ushort, ushort> SendObjectResourceState = ClientStaticMethod<byte, byte, ushort, ushort>.Get(new ClientStaticMethod<byte, byte, ushort, ushort>.ReceiveDelegate(ObjectManager.ReceiveObjectResourceState));

		// Token: 0x04001860 RID: 6240
		private static readonly ClientStaticMethod<byte, byte, ushort, bool> SendObjectBinaryState = ClientStaticMethod<byte, byte, ushort, bool>.Get(new ClientStaticMethod<byte, byte, ushort, bool>.ReceiveDelegate(ObjectManager.ReceiveObjectBinaryState));

		// Token: 0x04001861 RID: 6241
		private static readonly ServerStaticMethod<byte, byte, ushort, bool> SendToggleObjectBinaryStateRequest = ServerStaticMethod<byte, byte, ushort, bool>.Get(new ServerStaticMethod<byte, byte, ushort, bool>.ReceiveDelegateWithContext(ObjectManager.ReceiveToggleObjectBinaryStateRequest));

		// Token: 0x04001862 RID: 6242
		private static readonly ClientStaticMethod<byte, byte> SendClearRegionObjects = ClientStaticMethod<byte, byte>.Get(new ClientStaticMethod<byte, byte>.ReceiveDelegate(ObjectManager.ReceiveClearRegionObjects));

		// Token: 0x04001863 RID: 6243
		private static readonly ClientStaticMethod SendObjects = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(ObjectManager.ReceiveObjects));
	}
}
