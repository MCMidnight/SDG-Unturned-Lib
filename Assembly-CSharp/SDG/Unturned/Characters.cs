using System;
using System.Collections.Generic;
using SDG.Provider;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005B0 RID: 1456
	public class Characters : MonoBehaviour
	{
		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002F58 RID: 12120 RVA: 0x000D05D0 File Offset: 0x000CE7D0
		// (set) Token: 0x06002F59 RID: 12121 RVA: 0x000D05D7 File Offset: 0x000CE7D7
		public static byte selected
		{
			get
			{
				return Characters._selected;
			}
			set
			{
				Characters._selected = value;
				CharacterUpdated characterUpdated = Characters.onCharacterUpdated;
				if (characterUpdated != null)
				{
					characterUpdated(Characters.selected, Characters.active);
				}
				Characters.apply();
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06002F5A RID: 12122 RVA: 0x000D05FE File Offset: 0x000CE7FE
		public static Character active
		{
			get
			{
				return Characters.list[(int)Characters.selected];
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06002F5B RID: 12123 RVA: 0x000D060B File Offset: 0x000CE80B
		// (set) Token: 0x06002F5C RID: 12124 RVA: 0x000D0612 File Offset: 0x000CE812
		public static Character[] list { get; private set; }

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002F5D RID: 12125 RVA: 0x000D061A File Offset: 0x000CE81A
		public static List<ulong> packageSkins
		{
			get
			{
				return Characters._packageSkins;
			}
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x000D0621 File Offset: 0x000CE821
		public static void rename(string name)
		{
			Characters.active.name = name;
			CharacterUpdated characterUpdated = Characters.onCharacterUpdated;
			if (characterUpdated == null)
			{
				return;
			}
			characterUpdated(Characters.selected, Characters.active);
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000D0647 File Offset: 0x000CE847
		public static void skillify(EPlayerSkillset skillset)
		{
			Characters.active.skillset = skillset;
			CharacterUpdated characterUpdated = Characters.onCharacterUpdated;
			if (characterUpdated != null)
			{
				characterUpdated(Characters.selected, Characters.active);
			}
			Characters.active.applyHero();
			Characters.apply();
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000D067D File Offset: 0x000CE87D
		public static void growFace(byte face)
		{
			Characters.active.face = face;
			Characters.apply(false, false);
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x000D0691 File Offset: 0x000CE891
		public static void growHair(byte hair)
		{
			Characters.active.hair = hair;
			Characters.apply(false, false);
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x000D06A5 File Offset: 0x000CE8A5
		public static void growBeard(byte beard)
		{
			Characters.active.beard = beard;
			Characters.apply(false, false);
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000D06B9 File Offset: 0x000CE8B9
		public static void paintSkin(Color color)
		{
			Characters.active.skin = color;
			Characters.apply(false, false);
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000D06CD File Offset: 0x000CE8CD
		public static void paintColor(Color color)
		{
			Characters.active.color = color;
			Characters.apply(false, false);
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x000D06E1 File Offset: 0x000CE8E1
		public static void renick(string nick)
		{
			Characters.active.nick = nick;
			CharacterUpdated characterUpdated = Characters.onCharacterUpdated;
			if (characterUpdated == null)
			{
				return;
			}
			characterUpdated(Characters.selected, Characters.active);
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x000D0707 File Offset: 0x000CE907
		public static void paintMarkerColor(Color color)
		{
			Characters.active.markerColor = color;
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x000D0714 File Offset: 0x000CE914
		public static void group(CSteamID group)
		{
			if (Characters.active.group == group)
			{
				Characters.active.group = CSteamID.Nil;
			}
			else
			{
				Characters.active.group = group;
			}
			CharacterUpdated characterUpdated = Characters.onCharacterUpdated;
			if (characterUpdated == null)
			{
				return;
			}
			characterUpdated(Characters.selected, Characters.active);
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x000D0768 File Offset: 0x000CE968
		public static void ungroup()
		{
			Characters.active.group = CSteamID.Nil;
			CharacterUpdated characterUpdated = Characters.onCharacterUpdated;
			if (characterUpdated == null)
			{
				return;
			}
			characterUpdated(Characters.selected, Characters.active);
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x000D0792 File Offset: 0x000CE992
		public static void hand(bool state)
		{
			Characters.active.hand = state;
			Characters.apply(false, false);
			CharacterUpdated characterUpdated = Characters.onCharacterUpdated;
			if (characterUpdated == null)
			{
				return;
			}
			characterUpdated(Characters.selected, Characters.active);
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x000D07BF File Offset: 0x000CE9BF
		public static bool isSkinEquipped(ulong instance)
		{
			return instance != 0UL && Characters.packageSkins.IndexOf(instance) != -1;
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000D07D8 File Offset: 0x000CE9D8
		public static bool isCosmeticEquipped(ulong instance)
		{
			return instance != 0UL && (Characters.active.packageBackpack == instance || Characters.active.packageGlasses == instance || Characters.active.packageHat == instance || Characters.active.packageMask == instance || Characters.active.packagePants == instance || Characters.active.packageShirt == instance || Characters.active.packageVest == instance);
		}

		/// <summary>
		/// Is cosmetic or skin equipped?
		/// </summary>
		// Token: 0x06002F6C RID: 12140 RVA: 0x000D0847 File Offset: 0x000CEA47
		public static bool isEquipped(ulong instanceID)
		{
			return Characters.isSkinEquipped(instanceID) || Characters.isCosmeticEquipped(instanceID);
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000D085C File Offset: 0x000CEA5C
		public static void ToggleEquipItemByInstanceId(ulong itemInstanceId)
		{
			int inventoryItem = Provider.provider.economyService.getInventoryItem(itemInstanceId);
			if (inventoryItem == 0)
			{
				return;
			}
			Guid guid;
			Guid guid2;
			Provider.provider.economyService.getInventoryTargetID(inventoryItem, out guid, out guid2);
			if (guid == default(Guid) && guid2 == default(Guid))
			{
				return;
			}
			ItemAsset itemAsset = Assets.find<ItemAsset>(guid);
			if (itemAsset == null || itemAsset.proPath == null || itemAsset.proPath.Length == 0)
			{
				if (Provider.provider.economyService.getInventorySkinID(inventoryItem) == 0)
				{
					return;
				}
				if (!Characters.packageSkins.Remove(itemInstanceId))
				{
					for (int i = 0; i < Characters.packageSkins.Count; i++)
					{
						ulong num = Characters.packageSkins[i];
						if (num != 0UL)
						{
							int inventoryItem2 = Provider.provider.economyService.getInventoryItem(num);
							if (inventoryItem2 != 0)
							{
								Guid guid3;
								Guid guid4;
								Provider.provider.economyService.getInventoryTargetID(inventoryItem2, out guid3, out guid4);
								if ((guid != default(Guid) && guid == guid3) || (guid2 != default(Guid) && guid2 == guid4))
								{
									Characters.packageSkins.RemoveAt(i);
									break;
								}
							}
						}
					}
					Characters.packageSkins.Add(itemInstanceId);
				}
			}
			if (itemAsset != null)
			{
				if (itemAsset.type == EItemType.SHIRT)
				{
					if (Characters.active.packageShirt == itemInstanceId)
					{
						Characters.active.packageShirt = 0UL;
					}
					else
					{
						Characters.active.packageShirt = itemInstanceId;
					}
				}
				else if (itemAsset.type == EItemType.PANTS)
				{
					if (Characters.active.packagePants == itemInstanceId)
					{
						Characters.active.packagePants = 0UL;
					}
					else
					{
						Characters.active.packagePants = itemInstanceId;
					}
				}
				else if (itemAsset.type == EItemType.HAT)
				{
					if (Characters.active.packageHat == itemInstanceId)
					{
						Characters.active.packageHat = 0UL;
					}
					else
					{
						Characters.active.packageHat = itemInstanceId;
					}
				}
				else if (itemAsset.type == EItemType.BACKPACK)
				{
					if (Characters.active.packageBackpack == itemInstanceId)
					{
						Characters.active.packageBackpack = 0UL;
					}
					else
					{
						Characters.active.packageBackpack = itemInstanceId;
					}
				}
				else if (itemAsset.type == EItemType.VEST)
				{
					if (Characters.active.packageVest == itemInstanceId)
					{
						Characters.active.packageVest = 0UL;
					}
					else
					{
						Characters.active.packageVest = itemInstanceId;
					}
				}
				else if (itemAsset.type == EItemType.MASK)
				{
					if (Characters.active.packageMask == itemInstanceId)
					{
						Characters.active.packageMask = 0UL;
					}
					else
					{
						Characters.active.packageMask = itemInstanceId;
					}
				}
				else if (itemAsset.type == EItemType.GLASSES)
				{
					if (Characters.active.packageGlasses == itemInstanceId)
					{
						Characters.active.packageGlasses = 0UL;
					}
					else
					{
						Characters.active.packageGlasses = itemInstanceId;
					}
				}
			}
			Characters.apply(false, true);
			CharacterUpdated characterUpdated = Characters.onCharacterUpdated;
			if (characterUpdated == null)
			{
				return;
			}
			characterUpdated(Characters.selected, Characters.active);
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000D0B3C File Offset: 0x000CED3C
		public static bool getPackageForItemID(ushort itemID, out ulong itemInstanceId)
		{
			itemInstanceId = 0UL;
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, itemID) as ItemAsset;
			if (itemAsset == null)
			{
				return false;
			}
			for (int i = 0; i < Characters.packageSkins.Count; i++)
			{
				itemInstanceId = Characters.packageSkins[i];
				if (itemInstanceId != 0UL)
				{
					int inventoryItem = Provider.provider.economyService.getInventoryItem(itemInstanceId);
					if (inventoryItem != 0)
					{
						Guid inventoryItemGuid = Provider.provider.economyService.getInventoryItemGuid(inventoryItem);
						if (!(inventoryItemGuid == default(Guid)) && itemAsset.GUID == inventoryItemGuid)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000D0BD0 File Offset: 0x000CEDD0
		private static bool getSlot0StatTrackerValue(out EStatTrackerType type, out int kills)
		{
			type = EStatTrackerType.NONE;
			kills = -1;
			ulong instance;
			return Characters.getPackageForItemID(Characters.active.primaryItem, out instance) && Provider.provider.economyService.getInventoryStatTrackerValue(instance, out type, out kills);
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000D0C0C File Offset: 0x000CEE0C
		private static bool getSlot1StatTrackerValue(out EStatTrackerType type, out int kills)
		{
			type = EStatTrackerType.NONE;
			kills = -1;
			ulong instance;
			return Characters.getPackageForItemID(Characters.active.secondaryItem, out instance) && Provider.provider.economyService.getInventoryStatTrackerValue(instance, out type, out kills);
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000D0C48 File Offset: 0x000CEE48
		private static void apply(byte slot, bool showItems)
		{
			if (Characters.slots[(int)slot] != null)
			{
				Object.Destroy(Characters.slots[(int)slot].gameObject);
			}
			if (!showItems)
			{
				return;
			}
			ushort num = 0;
			byte[] state = null;
			if (slot == 0)
			{
				num = Characters.active.primaryItem;
				state = Characters.active.primaryState;
			}
			else if (slot == 1)
			{
				num = Characters.active.secondaryItem;
				state = Characters.active.secondaryState;
			}
			if (num == 0)
			{
				return;
			}
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, num) as ItemAsset;
			if (itemAsset != null)
			{
				ushort skin = 0;
				ushort num2 = 0;
				for (int i = 0; i < Characters.packageSkins.Count; i++)
				{
					ulong num3 = Characters.packageSkins[i];
					if (num3 != 0UL)
					{
						int inventoryItem = Provider.provider.economyService.getInventoryItem(num3);
						if (inventoryItem != 0)
						{
							Guid inventoryItemGuid = Provider.provider.economyService.getInventoryItemGuid(inventoryItem);
							if (!(inventoryItemGuid == default(Guid)) && itemAsset.GUID == inventoryItemGuid)
							{
								skin = Provider.provider.economyService.getInventorySkinID(inventoryItem);
								num2 = Provider.provider.economyService.getInventoryMythicID(inventoryItem);
								if (num2 == 0)
								{
									num2 = Provider.provider.economyService.getInventoryParticleEffect(num3);
									break;
								}
								break;
							}
						}
					}
				}
				GetStatTrackerValueHandler statTrackerCallback = null;
				if (slot == 0)
				{
					statTrackerCallback = new GetStatTrackerValueHandler(Characters.getSlot0StatTrackerValue);
				}
				else if (slot == 1)
				{
					statTrackerCallback = new GetStatTrackerValueHandler(Characters.getSlot1StatTrackerValue);
				}
				Transform item = ItemTool.getItem(num, skin, 100, state, false, itemAsset, statTrackerCallback);
				if (slot == 0)
				{
					if (itemAsset.type == EItemType.MELEE)
					{
						item.transform.parent = Characters.primaryMeleeSlot;
					}
					else if (itemAsset.slot == ESlotType.PRIMARY)
					{
						item.transform.parent = Characters.primaryLargeGunSlot;
					}
					else
					{
						item.transform.parent = Characters.primarySmallGunSlot;
					}
				}
				else if (slot == 1)
				{
					if (itemAsset.type == EItemType.MELEE)
					{
						item.transform.parent = Characters.secondaryMeleeSlot;
					}
					else
					{
						item.transform.parent = Characters.secondaryGunSlot;
					}
				}
				item.localPosition = Vector3.zero;
				item.localRotation = Quaternion.Euler(0f, 0f, 90f);
				item.localScale = Vector3.one;
				Object.Destroy(item.GetComponent<Collider>());
				if (num2 != 0)
				{
					ItemTool.ApplyMythicalEffect(item, num2, EEffectType.THIRD);
				}
				Characters.slots[(int)slot] = item;
			}
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000D0E9C File Offset: 0x000CF09C
		public static void apply()
		{
			Characters.apply(true, true);
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x000D0EA8 File Offset: 0x000CF0A8
		public static void apply(bool showItems, bool showCosmetics)
		{
			if (Characters.active == null)
			{
				UnturnedLog.error("Failed to find an active character.");
				return;
			}
			if (Characters.clothes == null)
			{
				UnturnedLog.error("Failed to find character clothes.");
				return;
			}
			try
			{
				Characters.applyInternal(showItems, showCosmetics);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e);
			}
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000D0F00 File Offset: 0x000CF100
		private static void applyInternal(bool showItems, bool showCosmetics)
		{
			Characters.character.localScale = new Vector3((float)(Characters.active.hand ? -1 : 1), 1f, 1f);
			if (showItems)
			{
				Characters.clothes.shirt = Characters.active.shirt;
				Characters.clothes.pants = Characters.active.pants;
				Characters.clothes.hat = Characters.active.hat;
				Characters.clothes.backpack = Characters.active.backpack;
				Characters.clothes.vest = Characters.active.vest;
				Characters.clothes.mask = Characters.active.mask;
				Characters.clothes.glasses = Characters.active.glasses;
			}
			else
			{
				Characters.clothes.shirt = 0;
				Characters.clothes.pants = 0;
				Characters.clothes.hat = 0;
				Characters.clothes.backpack = 0;
				Characters.clothes.vest = 0;
				Characters.clothes.mask = 0;
				Characters.clothes.glasses = 0;
			}
			if (showCosmetics)
			{
				if (Characters.active.packageShirt != 0UL)
				{
					Characters.clothes.visualShirt = Provider.provider.economyService.getInventoryItem(Characters.active.packageShirt);
				}
				else
				{
					Characters.clothes.visualShirt = 0;
				}
				if (Characters.active.packagePants != 0UL)
				{
					Characters.clothes.visualPants = Provider.provider.economyService.getInventoryItem(Characters.active.packagePants);
				}
				else
				{
					Characters.clothes.visualPants = 0;
				}
				if (Characters.active.packageHat != 0UL)
				{
					Characters.clothes.visualHat = Provider.provider.economyService.getInventoryItem(Characters.active.packageHat);
				}
				else
				{
					Characters.clothes.visualHat = 0;
				}
				if (Characters.active.packageBackpack != 0UL)
				{
					Characters.clothes.visualBackpack = Provider.provider.economyService.getInventoryItem(Characters.active.packageBackpack);
				}
				else
				{
					Characters.clothes.visualBackpack = 0;
				}
				if (Characters.active.packageVest != 0UL)
				{
					Characters.clothes.visualVest = Provider.provider.economyService.getInventoryItem(Characters.active.packageVest);
				}
				else
				{
					Characters.clothes.visualVest = 0;
				}
				if (Characters.active.packageMask != 0UL)
				{
					Characters.clothes.visualMask = Provider.provider.economyService.getInventoryItem(Characters.active.packageMask);
				}
				else
				{
					Characters.clothes.visualMask = 0;
				}
				if (Characters.active.packageGlasses != 0UL)
				{
					Characters.clothes.visualGlasses = Provider.provider.economyService.getInventoryItem(Characters.active.packageGlasses);
				}
				else
				{
					Characters.clothes.visualGlasses = 0;
				}
			}
			else
			{
				Characters.clothes.visualShirt = 0;
				Characters.clothes.visualPants = 0;
				Characters.clothes.visualHat = 0;
				Characters.clothes.visualBackpack = 0;
				Characters.clothes.visualVest = 0;
				Characters.clothes.visualMask = 0;
				Characters.clothes.visualGlasses = 0;
			}
			Characters.clothes.face = Characters.active.face;
			Characters.clothes.hair = Characters.active.hair;
			Characters.clothes.beard = Characters.active.beard;
			Characters.clothes.skin = Characters.active.skin;
			Characters.clothes.color = Characters.active.color;
			Characters.clothes.hand = Characters.active.hand;
			Characters.clothes.apply();
			byte b = 0;
			while ((int)b < Characters.slots.Length)
			{
				Characters.apply(b, showItems);
				b += 1;
			}
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000D12B0 File Offset: 0x000CF4B0
		private static void onInventoryRefreshed()
		{
			if (Characters.clothes != null && Characters.list != null && Characters.packageSkins != null)
			{
				for (int i = Characters.packageSkins.Count - 1; i >= 0; i--)
				{
					ulong num = Characters.packageSkins[i];
					if (num != 0UL && Provider.provider.economyService.getInventoryItem(num) == 0)
					{
						Characters.packageSkins.RemoveAt(i);
					}
				}
				for (int j = 0; j < Characters.list.Length; j++)
				{
					Character character = Characters.list[j];
					if (character != null)
					{
						if (character.packageShirt != 0UL && Provider.provider.economyService.getInventoryItem(character.packageShirt) == 0)
						{
							character.packageShirt = 0UL;
						}
						if (character.packagePants != 0UL && Provider.provider.economyService.getInventoryItem(character.packagePants) == 0)
						{
							character.packagePants = 0UL;
						}
						if (character.packageHat != 0UL && Provider.provider.economyService.getInventoryItem(character.packageHat) == 0)
						{
							character.packageHat = 0UL;
						}
						if (character.packageBackpack != 0UL && Provider.provider.economyService.getInventoryItem(character.packageBackpack) == 0)
						{
							character.packageBackpack = 0UL;
						}
						if (character.packageVest != 0UL && Provider.provider.economyService.getInventoryItem(character.packageVest) == 0)
						{
							character.packageVest = 0UL;
						}
						if (character.packageMask != 0UL && Provider.provider.economyService.getInventoryItem(character.packageMask) == 0)
						{
							character.packageMask = 0UL;
						}
						if (character.packageGlasses != 0UL && Provider.provider.economyService.getInventoryItem(character.packageGlasses) == 0)
						{
							character.packageGlasses = 0UL;
						}
					}
				}
				if (!Characters.initialApply)
				{
					Characters.initialApply = true;
					Characters.apply();
				}
			}
			if (Characters.hasDropped)
			{
				return;
			}
			Characters.hasDropped = true;
			if (Characters.hasPlayed)
			{
				Provider.provider.economyService.dropInventory();
			}
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000D1491 File Offset: 0x000CF691
		private void Update()
		{
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x000D1494 File Offset: 0x000CF694
		internal void customStart()
		{
			Characters.character = GameObject.Find("Hero").transform;
			Characters.clothes = Characters.character.GetComponent<HumanClothes>();
			Characters.clothes.isView = true;
			Characters.slots = new Transform[(int)PlayerInventory.SLOTS];
			Characters.primaryMeleeSlot = Characters.character.Find("Skeleton").Find("Spine").Find("Primary_Melee");
			Characters.primaryLargeGunSlot = Characters.character.Find("Skeleton").Find("Spine").Find("Primary_Large_Gun");
			Characters.primarySmallGunSlot = Characters.character.Find("Skeleton").Find("Spine").Find("Primary_Small_Gun");
			Characters.secondaryMeleeSlot = Characters.character.Find("Skeleton").Find("Right_Hip").Find("Right_Leg").Find("Secondary_Melee");
			Characters.secondaryGunSlot = Characters.character.Find("Skeleton").Find("Right_Hip").Find("Right_Leg").Find("Secondary_Gun");
			Characters.characterOffset = Characters.character.transform.eulerAngles.y;
			Characters._characterYaw = Characters.characterOffset;
			Characters.characterYaw = 0f;
			Characters.hasDropped = false;
			if (!Characters.hasLoaded)
			{
				TempSteamworksEconomy economyService = Provider.provider.economyService;
				economyService.onInventoryRefreshed = (TempSteamworksEconomy.InventoryRefreshed)Delegate.Combine(economyService.onInventoryRefreshed, new TempSteamworksEconomy.InventoryRefreshed(Characters.onInventoryRefreshed));
			}
			Characters.load();
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000D1624 File Offset: 0x000CF824
		public static void load()
		{
			Characters.initialApply = false;
			Provider.provider.economyService.refreshInventory();
			if (Characters.list != null)
			{
				byte b = 0;
				while ((int)b < Characters.list.Length)
				{
					if (Characters.list[(int)b] != null)
					{
						CharacterUpdated characterUpdated = Characters.onCharacterUpdated;
						if (characterUpdated != null)
						{
							characterUpdated(b, Characters.list[(int)b]);
						}
					}
					b += 1;
				}
				return;
			}
			Characters.list = new Character[(int)(Customization.FREE_CHARACTERS + Customization.PRO_CHARACTERS)];
			Characters._packageSkins = new List<ulong>();
			if (ReadWrite.fileExists("/Characters.dat", true))
			{
				Block block = ReadWrite.readBlock("/Characters.dat", true, 0);
				if (block != null)
				{
					byte b2 = block.readByte();
					if (b2 >= 12)
					{
						if (b2 >= 14)
						{
							ushort num = block.readUInt16();
							for (ushort num2 = 0; num2 < num; num2 += 1)
							{
								ulong num3 = block.readUInt64();
								if (num3 != 0UL)
								{
									Characters.packageSkins.Add(num3);
								}
							}
						}
						Characters._selected = block.readByte();
						if ((int)Characters._selected >= Characters.list.Length || (!Provider.isPro && Characters.selected >= Customization.FREE_CHARACTERS))
						{
							Characters._selected = 0;
						}
						byte b3 = 0;
						while ((int)b3 < Characters.list.Length)
						{
							ushort newShirt = block.readUInt16();
							ushort newPants = block.readUInt16();
							ushort newHat = block.readUInt16();
							ushort newBackpack = block.readUInt16();
							ushort newVest = block.readUInt16();
							ushort newMask = block.readUInt16();
							ushort newGlasses = block.readUInt16();
							ulong newPackageShirt = block.readUInt64();
							ulong newPackagePants = block.readUInt64();
							ulong newPackageHat = block.readUInt64();
							ulong newPackageBackpack = block.readUInt64();
							ulong newPackageVest = block.readUInt64();
							ulong newPackageMask = block.readUInt64();
							ulong newPackageGlasses = block.readUInt64();
							ushort newPrimaryItem = block.readUInt16();
							byte[] newPrimaryState = block.readByteArray();
							ushort newSecondaryItem = block.readUInt16();
							byte[] newSecondaryState = block.readByteArray();
							byte b4 = block.readByte();
							byte b5 = block.readByte();
							byte b6 = block.readByte();
							Color color = block.readColor();
							Color color2 = block.readColor();
							Color newMarkerColor;
							if (b2 > 20)
							{
								newMarkerColor = block.readColor();
							}
							else
							{
								newMarkerColor = Customization.MARKER_COLORS[Random.Range(0, Customization.MARKER_COLORS.Length)];
							}
							bool newHand = block.readBoolean();
							string newName = block.readString();
							if (b2 < 19)
							{
								newName = Provider.clientName;
							}
							string newNick = block.readString();
							CSteamID csteamID = block.readSteamID();
							byte b7 = block.readByte();
							if (!Provider.provider.communityService.checkGroup(csteamID))
							{
								csteamID = CSteamID.Nil;
							}
							if (b7 >= Customization.SKILLSETS)
							{
								b7 = 0;
							}
							if (b2 < 16)
							{
								b7 = (byte)Random.Range(1, (int)Customization.SKILLSETS);
							}
							if (b2 > 16 && b2 < 20)
							{
								block.readBoolean();
							}
							if (!Provider.isPro)
							{
								if (b3 >= Customization.FREE_CHARACTERS)
								{
									newName = Provider.clientName;
									newNick = Provider.clientName;
								}
								if (b4 >= Customization.FACES_FREE)
								{
									b4 = (byte)Random.Range(0, (int)Customization.FACES_FREE);
								}
								if (b5 >= Customization.HAIRS_FREE)
								{
									b5 = (byte)Random.Range(0, (int)Customization.HAIRS_FREE);
								}
								if (b6 >= Customization.BEARDS_FREE)
								{
									b6 = 0;
								}
								if (!Customization.checkSkin(color))
								{
									color = Customization.SKINS[Random.Range(0, Customization.SKINS.Length)];
								}
								if (!Customization.checkColor(color2))
								{
									color2 = Customization.COLORS[Random.Range(0, Customization.COLORS.Length)];
								}
							}
							Characters.list[(int)b3] = new Character(newShirt, newPants, newHat, newBackpack, newVest, newMask, newGlasses, newPackageShirt, newPackagePants, newPackageHat, newPackageBackpack, newPackageVest, newPackageMask, newPackageGlasses, newPrimaryItem, newPrimaryState, newSecondaryItem, newSecondaryState, b4, b5, b6, color, color2, newMarkerColor, newHand, newName, newNick, csteamID, (EPlayerSkillset)b7);
							CharacterUpdated characterUpdated2 = Characters.onCharacterUpdated;
							if (characterUpdated2 != null)
							{
								characterUpdated2(b3, Characters.list[(int)b3]);
							}
							b3 += 1;
						}
					}
					else
					{
						byte b8 = 0;
						while ((int)b8 < Characters.list.Length)
						{
							Characters.list[(int)b8] = new Character();
							CharacterUpdated characterUpdated3 = Characters.onCharacterUpdated;
							if (characterUpdated3 != null)
							{
								characterUpdated3(b8, Characters.list[(int)b8]);
							}
							b8 += 1;
						}
					}
				}
			}
			else
			{
				Characters._selected = 0;
			}
			byte b9 = 0;
			while ((int)b9 < Characters.list.Length)
			{
				if (Characters.list[(int)b9] == null)
				{
					Characters.list[(int)b9] = new Character();
					CharacterUpdated characterUpdated4 = Characters.onCharacterUpdated;
					if (characterUpdated4 != null)
					{
						characterUpdated4(b9, Characters.list[(int)b9]);
					}
				}
				b9 += 1;
			}
			Characters.apply();
			Characters.hasLoaded = true;
			UnturnedLog.info("Loaded characters");
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x000D1A5C File Offset: 0x000CFC5C
		public static void save()
		{
			if (!Characters.hasLoaded)
			{
				return;
			}
			Block block = new Block();
			block.writeByte(Characters.SAVEDATA_VERSION);
			block.writeUInt16((ushort)Characters.packageSkins.Count);
			ushort num = 0;
			while ((int)num < Characters.packageSkins.Count)
			{
				ulong value = Characters.packageSkins[(int)num];
				block.writeUInt64(value);
				num += 1;
			}
			block.writeByte(Characters.selected);
			byte b = 0;
			while ((int)b < Characters.list.Length)
			{
				Character character = Characters.list[(int)b];
				if (character == null)
				{
					character = new Character();
				}
				block.writeUInt16(character.shirt);
				block.writeUInt16(character.pants);
				block.writeUInt16(character.hat);
				block.writeUInt16(character.backpack);
				block.writeUInt16(character.vest);
				block.writeUInt16(character.mask);
				block.writeUInt16(character.glasses);
				block.writeUInt64(character.packageShirt);
				block.writeUInt64(character.packagePants);
				block.writeUInt64(character.packageHat);
				block.writeUInt64(character.packageBackpack);
				block.writeUInt64(character.packageVest);
				block.writeUInt64(character.packageMask);
				block.writeUInt64(character.packageGlasses);
				block.writeUInt16(character.primaryItem);
				block.writeByteArray(character.primaryState);
				block.writeUInt16(character.secondaryItem);
				block.writeByteArray(character.secondaryState);
				block.writeByte(character.face);
				block.writeByte(character.hair);
				block.writeByte(character.beard);
				block.writeColor(character.skin);
				block.writeColor(character.color);
				block.writeColor(character.markerColor);
				block.writeBoolean(character.hand);
				block.writeString(character.name);
				block.writeString(character.nick);
				block.writeSteamID(character.group);
				block.writeByte((byte)character.skillset);
				b += 1;
			}
			ReadWrite.writeBlock("/Characters.dat", true, block);
		}

		// Token: 0x04001987 RID: 6535
		public static readonly byte SAVEDATA_VERSION = 21;

		// Token: 0x04001988 RID: 6536
		private static bool hasLoaded;

		// Token: 0x04001989 RID: 6537
		private static bool initialApply;

		// Token: 0x0400198A RID: 6538
		public static bool hasPlayed;

		// Token: 0x0400198B RID: 6539
		private static bool hasDropped;

		// Token: 0x0400198C RID: 6540
		public static CharacterUpdated onCharacterUpdated;

		// Token: 0x0400198D RID: 6541
		private static byte _selected;

		// Token: 0x0400198F RID: 6543
		private static Transform character;

		// Token: 0x04001990 RID: 6544
		public static HumanClothes clothes;

		// Token: 0x04001991 RID: 6545
		private static Transform[] slots;

		// Token: 0x04001992 RID: 6546
		private static Transform primaryMeleeSlot;

		// Token: 0x04001993 RID: 6547
		private static Transform primaryLargeGunSlot;

		// Token: 0x04001994 RID: 6548
		private static Transform primarySmallGunSlot;

		// Token: 0x04001995 RID: 6549
		private static Transform secondaryMeleeSlot;

		// Token: 0x04001996 RID: 6550
		private static Transform secondaryGunSlot;

		// Token: 0x04001997 RID: 6551
		private static List<ulong> _packageSkins;

		// Token: 0x04001998 RID: 6552
		private static float characterOffset;

		// Token: 0x04001999 RID: 6553
		private static float _characterYaw;

		// Token: 0x0400199A RID: 6554
		public static float characterYaw;
	}
}
