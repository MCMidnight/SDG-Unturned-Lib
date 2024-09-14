using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000615 RID: 1557
	public class PlayerCrafting : PlayerCaller
	{
		// Token: 0x060031F3 RID: 12787 RVA: 0x000DD858 File Offset: 0x000DBA58
		public bool isBlueprintBlacklisted(Blueprint blueprint)
		{
			LevelAsset asset = Level.getAsset();
			return asset != null && asset.isBlueprintBlacklisted(blueprint);
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x000DD878 File Offset: 0x000DBA78
		private bool stripAttachments(byte page, ItemJar jar)
		{
			ItemAsset asset = jar.GetAsset();
			if (asset != null && asset.type == EItemType.GUN && jar.item.state != null && jar.item.state.Length == 18)
			{
				if (((ItemGunAsset)asset).hasSight)
				{
					ushort num = BitConverter.ToUInt16(jar.item.state, 0);
					if (num != 0 && num != ((ItemGunAsset)asset).sightID)
					{
						base.player.inventory.forceAddItem(new Item(num, false, jar.item.state[13]), true);
						jar.item.state[0] = 0;
						jar.item.state[1] = 0;
						jar.item.state[13] = 0;
					}
				}
				if (((ItemGunAsset)asset).hasTactical)
				{
					ushort num2 = BitConverter.ToUInt16(jar.item.state, 2);
					if (num2 != 0)
					{
						base.player.inventory.forceAddItem(new Item(num2, false, jar.item.state[14]), true);
						jar.item.state[2] = 0;
						jar.item.state[3] = 0;
						jar.item.state[14] = 0;
					}
				}
				if (((ItemGunAsset)asset).hasGrip)
				{
					ushort num3 = BitConverter.ToUInt16(jar.item.state, 4);
					if (num3 != 0)
					{
						base.player.inventory.forceAddItem(new Item(num3, false, jar.item.state[15]), true);
						jar.item.state[4] = 0;
						jar.item.state[5] = 0;
						jar.item.state[15] = 0;
					}
				}
				if (((ItemGunAsset)asset).hasBarrel)
				{
					ushort num4 = BitConverter.ToUInt16(jar.item.state, 6);
					if (num4 != 0)
					{
						base.player.inventory.forceAddItem(new Item(num4, false, jar.item.state[16]), true);
						jar.item.state[6] = 0;
						jar.item.state[7] = 0;
						jar.item.state[16] = 0;
					}
				}
				if (((ItemGunAsset)asset).allowMagazineChange)
				{
					ushort num5 = BitConverter.ToUInt16(jar.item.state, 8);
					if (num5 != 0 && jar.item.state[10] > 0)
					{
						base.player.inventory.forceAddItem(new Item(num5, jar.item.state[10], jar.item.state[17]), true);
						jar.item.state[8] = 0;
						jar.item.state[9] = 0;
						jar.item.state[10] = 0;
						jar.item.state[17] = 0;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x000DDB4C File Offset: 0x000DBD4C
		public void removeItem(byte page, ItemJar jar)
		{
			base.player.inventory.removeItem(page, base.player.inventory.getIndex(page, jar.x, jar.y));
			this.stripAttachments(page, jar);
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x000DDB85 File Offset: 0x000DBD85
		[Obsolete]
		public void askStripAttachments(CSteamID steamID, byte page, byte x, byte y)
		{
			this.ReceiveStripAttachments(page, x, y);
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x000DDB94 File Offset: 0x000DBD94
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askStripAttachments")]
		public void ReceiveStripAttachments(byte page, byte x, byte y)
		{
			if (page < PlayerInventory.SLOTS || page >= PlayerInventory.PAGES - 1)
			{
				return;
			}
			if (base.player.equipment.checkSelection(page, x, y))
			{
				if (base.player.equipment.isBusy)
				{
					return;
				}
				base.player.equipment.dequip();
			}
			byte index = base.player.inventory.getIndex(page, x, y);
			if (index == 255)
			{
				return;
			}
			ItemJar item = base.player.inventory.getItem(page, index);
			if (item == null)
			{
				return;
			}
			if (!this.stripAttachments(page, item))
			{
				return;
			}
			base.player.inventory.sendUpdateInvState(page, x, y, item.item.state);
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x000DDC49 File Offset: 0x000DBE49
		public void sendStripAttachments(byte page, byte x, byte y)
		{
			PlayerCrafting.SendStripAttachments.Invoke(base.GetNetId(), ENetReliability.Unreliable, page, x, y);
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x000DDC5F File Offset: 0x000DBE5F
		[Obsolete]
		public void tellCraft(CSteamID steamID)
		{
			this.ReceiveRefreshCrafting();
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x000DDC67 File Offset: 0x000DBE67
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellCraft")]
		public void ReceiveRefreshCrafting()
		{
			CraftingUpdated craftingUpdated = this.onCraftingUpdated;
			if (craftingUpdated == null)
			{
				return;
			}
			craftingUpdated();
		}

		/// <summary>
		/// Requested for plugin use.
		/// Notifies owner they should refresh the crafting menu.
		/// </summary>
		// Token: 0x060031FB RID: 12795 RVA: 0x000DDC79 File Offset: 0x000DBE79
		public void ServerRefreshOwnerCrafting()
		{
			PlayerCrafting.SendRefreshCrafting.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection());
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x000DDC97 File Offset: 0x000DBE97
		[Obsolete]
		public void askCraft(CSteamID steamID, ushort id, byte index, bool force)
		{
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x000DDC9C File Offset: 0x000DBE9C
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10)]
		public void ReceiveCraft(in ServerInvocationContext context, ushort id, byte index, bool force)
		{
			if (Level.info != null && Level.info.configData != null && !Level.info.configData.Allow_Crafting)
			{
				return;
			}
			if (base.player.equipment.isBusy)
			{
				return;
			}
			bool flag = true;
			if (PlayerCrafting.onCraftBlueprintRequested != null)
			{
				PlayerCrafting.onCraftBlueprintRequested(this, ref id, ref index, ref flag);
			}
			else
			{
				PlayerCraftingRequestHandler playerCraftingRequestHandler = this.onCraftingRequested;
				if (playerCraftingRequestHandler != null)
				{
					playerCraftingRequestHandler(this, ref id, ref index, ref flag);
				}
			}
			if (!flag)
			{
				return;
			}
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, id) as ItemAsset;
			if (itemAsset == null)
			{
				return;
			}
			if ((int)index >= itemAsset.blueprints.Count)
			{
				return;
			}
			Blueprint blueprint = itemAsset.blueprints[(int)index];
			if (this.isBlueprintBlacklisted(blueprint))
			{
				return;
			}
			if (blueprint.skill == EBlueprintSkill.REPAIR && (uint)blueprint.level > Provider.modeConfigData.Gameplay.Repair_Level_Max)
			{
				return;
			}
			if (!string.IsNullOrEmpty(blueprint.map) && !blueprint.map.Equals(Level.info.name, 3))
			{
				return;
			}
			if (!Provider.modeConfigData.Gameplay.Allow_Freeform_Buildables && !Provider.modeConfigData.Gameplay.Allow_Freeform_Buildables_On_Vehicles && blueprint.IsOutputFreeformBuildable)
			{
				return;
			}
			if (blueprint.tool != 0 && base.player.inventory.has(blueprint.tool) == null)
			{
				return;
			}
			if (blueprint.skill != EBlueprintSkill.NONE)
			{
				bool flag2 = PowerTool.checkFires(base.transform.position, 16f);
				if ((blueprint.skill == EBlueprintSkill.CRAFT && base.player.skills.skills[2][1].level < blueprint.level) || (blueprint.skill == EBlueprintSkill.COOK && (!flag2 || base.player.skills.skills[2][3].level < blueprint.level)) || (blueprint.skill == EBlueprintSkill.REPAIR && base.player.skills.skills[2][7].level < blueprint.level))
				{
					return;
				}
			}
			bool flag3 = false;
			for (int i = 0; i < 64; i++)
			{
				if (!blueprint.areConditionsMet(base.player))
				{
					return;
				}
				List<InventorySearch>[] array = new List<InventorySearch>[blueprint.supplies.Length];
				for (int j = 0; j < blueprint.supplies.Length; j++)
				{
					BlueprintSupply blueprintSupply = blueprint.supplies[j];
					List<InventorySearch> list = base.player.inventory.search(blueprintSupply.id, false, true);
					if (list.Count == 0)
					{
						return;
					}
					ushort num = 0;
					foreach (InventorySearch inventorySearch in list)
					{
						num += (ushort)inventorySearch.jar.item.amount;
					}
					if (num < blueprintSupply.amount && blueprint.type != EBlueprintType.AMMO)
					{
						return;
					}
					if (blueprint.type == EBlueprintType.AMMO)
					{
						list.Sort(PlayerCrafting.amountAscendingComparator);
					}
					else
					{
						list.Sort(PlayerCrafting.qualityAscendingComparator);
					}
					array[j] = list;
				}
				if (blueprint.type == EBlueprintType.REPAIR)
				{
					List<InventorySearch> list2 = base.player.inventory.search(itemAsset.id, false, false);
					byte b = byte.MaxValue;
					int num2 = -1;
					for (int k = 0; k < list2.Count; k++)
					{
						if (list2[k].jar.item.quality < b)
						{
							b = list2[k].jar.item.quality;
							num2 = k;
						}
					}
					if (num2 < 0)
					{
						return;
					}
					InventorySearch inventorySearch2 = list2[num2];
					if (base.player.equipment.checkSelection(inventorySearch2.page, inventorySearch2.jar.x, inventorySearch2.jar.y))
					{
						base.player.equipment.dequip();
					}
					byte b2 = 0;
					while ((int)b2 < array.Length)
					{
						BlueprintSupply blueprintSupply2 = blueprint.supplies[(int)b2];
						List<InventorySearch> list3 = array[(int)b2];
						byte b3 = 0;
						while ((ushort)b3 < blueprintSupply2.amount)
						{
							InventorySearch inventorySearch3 = list3[(int)b3];
							if (base.player.equipment.checkSelection(inventorySearch3.page, inventorySearch3.jar.x, inventorySearch3.jar.y))
							{
								base.player.equipment.dequip();
							}
							this.removeItem(inventorySearch3.page, inventorySearch3.jar);
							if (inventorySearch3.page < PlayerInventory.SLOTS)
							{
								base.player.equipment.sendSlot(inventorySearch3.page);
							}
							b3 += 1;
						}
						b2 += 1;
					}
					base.player.inventory.sendUpdateQuality(inventorySearch2.page, inventorySearch2.jar.x, inventorySearch2.jar.y, 100);
					if (itemAsset.type == EItemType.REFILL && inventorySearch2.jar.item.state.Length == 1 && inventorySearch2.jar.item.state[0] == 3)
					{
						inventorySearch2.jar.item.state[0] = 1;
						base.player.inventory.sendUpdateInvState(inventorySearch2.page, inventorySearch2.jar.x, inventorySearch2.jar.y, inventorySearch2.jar.item.state);
					}
					blueprint.ApplyConditions(base.player);
					blueprint.GrantRewards(base.player);
					PlayerCrafting.SendRefreshCrafting.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection());
				}
				else if (blueprint.type == EBlueprintType.AMMO)
				{
					List<InventorySearch> list4 = base.player.inventory.search(itemAsset.id, true, true);
					int num3 = -1;
					int num4 = -1;
					for (int l = 0; l < list4.Count; l++)
					{
						if ((int)list4[l].jar.item.amount > num3 && list4[l].jar.item.amount < itemAsset.amount)
						{
							num3 = (int)list4[l].jar.item.amount;
							num4 = l;
						}
					}
					if (num4 < 0)
					{
						return;
					}
					InventorySearch inventorySearch4 = list4[num4];
					int num5 = (int)itemAsset.amount - num3;
					if (base.player.equipment.checkSelection(inventorySearch4.page, inventorySearch4.jar.x, inventorySearch4.jar.y))
					{
						base.player.equipment.dequip();
					}
					List<InventorySearch> list5 = array[0];
					byte b4 = 0;
					while ((int)b4 < list5.Count)
					{
						InventorySearch inventorySearch5 = list5[(int)b4];
						if (inventorySearch5.jar != inventorySearch4.jar)
						{
							if (base.player.equipment.checkSelection(inventorySearch5.page, inventorySearch5.jar.x, inventorySearch5.jar.y))
							{
								base.player.equipment.dequip();
							}
							if ((int)inventorySearch5.jar.item.amount > num5)
							{
								base.player.inventory.sendUpdateAmount(inventorySearch5.page, inventorySearch5.jar.x, inventorySearch5.jar.y, (byte)((int)inventorySearch5.jar.item.amount - num5));
								num5 = 0;
								break;
							}
							num5 -= (int)inventorySearch5.jar.item.amount;
							base.player.inventory.sendUpdateAmount(inventorySearch5.page, inventorySearch5.jar.x, inventorySearch5.jar.y, 0);
							Asset asset = inventorySearch5.GetAsset();
							bool flag4;
							if (asset != null)
							{
								if (asset is ItemSupplyAsset)
								{
									flag4 = true;
								}
								else
								{
									ItemMagazineAsset itemMagazineAsset = asset as ItemMagazineAsset;
									flag4 = (itemMagazineAsset == null || itemMagazineAsset.deleteEmpty);
								}
							}
							else
							{
								flag4 = true;
							}
							if (flag4)
							{
								this.removeItem(inventorySearch5.page, inventorySearch5.jar);
								if (inventorySearch5.page < PlayerInventory.SLOTS)
								{
									base.player.equipment.sendSlot(inventorySearch5.page);
								}
							}
							if (num5 == 0)
							{
								break;
							}
						}
						b4 += 1;
					}
					base.player.inventory.sendUpdateAmount(inventorySearch4.page, inventorySearch4.jar.x, inventorySearch4.jar.y, (byte)((int)itemAsset.amount - num5));
					blueprint.ApplyConditions(base.player);
					blueprint.GrantRewards(base.player);
					PlayerCrafting.SendRefreshCrafting.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection());
				}
				else
				{
					byte b5 = 0;
					while ((int)b5 < array.Length)
					{
						BlueprintSupply blueprintSupply3 = blueprint.supplies[(int)b5];
						List<InventorySearch> list6 = array[(int)b5];
						if (list6.Count < (int)blueprintSupply3.amount)
						{
							return;
						}
						byte b6 = 0;
						while ((ushort)b6 < blueprintSupply3.amount)
						{
							InventorySearch inventorySearch6 = list6[(int)b6];
							if (base.player.equipment.checkSelection(inventorySearch6.page, inventorySearch6.jar.x, inventorySearch6.jar.y))
							{
								base.player.equipment.dequip();
							}
							this.removeItem(inventorySearch6.page, inventorySearch6.jar);
							if (inventorySearch6.page < PlayerInventory.SLOTS)
							{
								base.player.equipment.sendSlot(inventorySearch6.page);
							}
							b6 += 1;
						}
						b5 += 1;
					}
					foreach (BlueprintOutput blueprintOutput in blueprint.outputs)
					{
						ItemAsset itemAsset2 = Assets.find(EAssetType.ITEM, blueprintOutput.id) as ItemAsset;
						for (int n = 0; n < (int)blueprintOutput.amount; n++)
						{
							if (blueprint.transferState)
							{
								Item item = new Item(blueprintOutput.id, array[0][0].jar.item.amount, array[0][0].jar.item.quality, array[0][0].jar.item.state);
								if (itemAsset.type == EItemType.GUN && itemAsset2 != null && itemAsset2.type == EItemType.GUN && item.state.Length >= 12)
								{
									ItemGunAsset itemGunAsset = itemAsset2 as ItemGunAsset;
									if (itemGunAsset != null)
									{
										item.state[11] = (byte)itemGunAsset.firemode;
									}
								}
								base.player.inventory.forceAddItem(item, true);
							}
							else
							{
								base.player.inventory.forceAddItem(new Item(blueprintOutput.id, blueprintOutput.origin), true);
							}
						}
					}
					blueprint.ApplyConditions(base.player);
					blueprint.GrantRewards(base.player);
					PlayerCrafting.SendRefreshCrafting.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection());
				}
				if (!flag3)
				{
					flag3 = true;
					base.player.sendStat(EPlayerStat.FOUND_CRAFTS);
					EffectAsset effectAsset = blueprint.FindBuildEffectAsset();
					if (effectAsset != null)
					{
						EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
						{
							position = base.transform.position,
							relevantDistance = EffectManager.SMALL
						});
						if (Provider.isServer)
						{
							AlertTool.alert(base.transform.position, 8f);
						}
					}
				}
				if (!force || blueprint.type == EBlueprintType.REPAIR || blueprint.type == EBlueprintType.AMMO)
				{
					return;
				}
			}
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x000DE818 File Offset: 0x000DCA18
		public void sendCraft(ushort id, byte index, bool force)
		{
			PlayerCrafting.SendCraft.Invoke(base.GetNetId(), ENetReliability.Unreliable, id, index, force);
		}

		/// <summary>
		/// Get whether this player is ignoring a blueprint.
		/// </summary>
		// Token: 0x060031FF RID: 12799 RVA: 0x000DE830 File Offset: 0x000DCA30
		public bool getIgnoringBlueprint(Blueprint blueprint)
		{
			if (blueprint == null)
			{
				return false;
			}
			using (List<IgnoredCraftingBlueprint>.Enumerator enumerator = this.ignoredBlueprints.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.matchesBlueprint(blueprint))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Set whether this player is ignoring a blueprint.
		/// This is a kludge to help with accidentally crafting items like blindfolds.
		/// </summary>
		// Token: 0x06003200 RID: 12800 RVA: 0x000DE890 File Offset: 0x000DCA90
		public void setIgnoringBlueprint(Blueprint blueprint, bool isIgnoring)
		{
			if (blueprint == null)
			{
				return;
			}
			for (int i = this.ignoredBlueprints.Count - 1; i >= 0; i--)
			{
				if (this.ignoredBlueprints[i].matchesBlueprint(blueprint))
				{
					if (!isIgnoring)
					{
						this.ignoredBlueprints.RemoveAtFast(i);
					}
					return;
				}
			}
			IgnoredCraftingBlueprint ignoredCraftingBlueprint = new IgnoredCraftingBlueprint();
			ignoredCraftingBlueprint.itemId = blueprint.sourceItem.id;
			ignoredCraftingBlueprint.blueprintIndex = blueprint.id;
			this.ignoredBlueprints.Add(ignoredCraftingBlueprint);
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x000DE90C File Offset: 0x000DCB0C
		internal void InitializePlayer()
		{
			if (base.channel.IsLocalPlayer)
			{
				this.load();
			}
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x000DE921 File Offset: 0x000DCB21
		private void OnDestroy()
		{
			if (base.channel.IsLocalPlayer)
			{
				this.save();
			}
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x000DE938 File Offset: 0x000DCB38
		private void load()
		{
			if (ReadWrite.fileExists("/Cloud/Ignored_Blueprints.dat", false))
			{
				Block block = ReadWrite.readBlock("/Cloud/Ignored_Blueprints.dat", false, 0);
				block.readByte();
				int num = block.readInt32();
				num = Mathf.Min(num, 10000);
				this.ignoredBlueprints.Capacity = this.ignoredBlueprints.Count + num;
				for (int i = 0; i < num; i++)
				{
					ushort num2 = block.readUInt16();
					byte blueprintIndex = block.readByte();
					if (num2 != 0)
					{
						IgnoredCraftingBlueprint ignoredCraftingBlueprint = new IgnoredCraftingBlueprint();
						ignoredCraftingBlueprint.itemId = num2;
						ignoredCraftingBlueprint.blueprintIndex = blueprintIndex;
						this.ignoredBlueprints.Add(ignoredCraftingBlueprint);
					}
				}
			}
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x000DE9D8 File Offset: 0x000DCBD8
		private void save()
		{
			Block block = new Block();
			block.writeByte(PlayerCrafting.SAVEDATA_VERSION);
			block.writeInt32(this.ignoredBlueprints.Count);
			foreach (IgnoredCraftingBlueprint ignoredCraftingBlueprint in this.ignoredBlueprints)
			{
				block.writeUInt16(ignoredCraftingBlueprint.itemId);
				block.writeByte(ignoredCraftingBlueprint.blueprintIndex);
			}
			ReadWrite.writeBlock("/Cloud/Ignored_Blueprints.dat", false, block);
		}

		// Token: 0x04001C63 RID: 7267
		private static readonly byte SAVEDATA_VERSION = 1;

		// Token: 0x04001C64 RID: 7268
		private static InventorySearchQualityAscendingComparator qualityAscendingComparator = new InventorySearchQualityAscendingComparator();

		// Token: 0x04001C65 RID: 7269
		private static InventorySearchAmountAscendingComparator amountAscendingComparator = new InventorySearchAmountAscendingComparator();

		// Token: 0x04001C66 RID: 7270
		[Obsolete("Use the static onCraftBlueprintRequested for ease-of-use instead.")]
		public PlayerCraftingRequestHandler onCraftingRequested;

		// Token: 0x04001C67 RID: 7271
		public static PlayerCraftingRequestHandler onCraftBlueprintRequested;

		// Token: 0x04001C68 RID: 7272
		public CraftingUpdated onCraftingUpdated;

		// Token: 0x04001C69 RID: 7273
		private static readonly ServerInstanceMethod<byte, byte, byte> SendStripAttachments = ServerInstanceMethod<byte, byte, byte>.Get(typeof(PlayerCrafting), "ReceiveStripAttachments");

		// Token: 0x04001C6A RID: 7274
		private static readonly ClientInstanceMethod SendRefreshCrafting = ClientInstanceMethod.Get(typeof(PlayerCrafting), "ReceiveRefreshCrafting");

		// Token: 0x04001C6B RID: 7275
		private static readonly ServerInstanceMethod<ushort, byte, bool> SendCraft = ServerInstanceMethod<ushort, byte, bool>.Get(typeof(PlayerCrafting), "ReceiveCraft");

		// Token: 0x04001C6C RID: 7276
		private List<IgnoredCraftingBlueprint> ignoredBlueprints = new List<IgnoredCraftingBlueprint>();
	}
}
