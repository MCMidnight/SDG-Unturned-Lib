using System;
using System.Collections.Generic;
using SDG.Provider;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007A9 RID: 1961
	public class MenuSurvivorsClothingBoxUI
	{
		/// <summary>
		/// Format qualityRarities as ##.#
		/// Does not use 'P' format because localized strings unfortunately already had % sign.
		/// </summary>
		// Token: 0x060041BF RID: 16831 RVA: 0x001618B0 File Offset: 0x0015FAB0
		private static string formatQualityRarity(EItemRarity rarity)
		{
			return (MenuSurvivorsClothingBoxUI.qualityRarities[rarity] * 100f).ToString("0.0");
		}

		/// <summary>
		/// Skip unboxing animation.
		/// Initial call rotates to just before the item, next call skips entirely.
		/// </summary>
		// Token: 0x060041C0 RID: 16832 RVA: 0x001618DC File Offset: 0x0015FADC
		public static void skipAnimation()
		{
			if (!MenuSurvivorsClothingBoxUI.isUnboxing)
			{
				return;
			}
			if (MenuSurvivorsClothingBoxUI.target == -1)
			{
				return;
			}
			if (MenuSurvivorsClothingBoxUI.rotation == MenuSurvivorsClothingBoxUI.target)
			{
				MenuSurvivorsClothingBoxUI.lastAngle -= 1f;
				return;
			}
			float num = 1.5707964f;
			float num2 = (float)MenuSurvivorsClothingBoxUI.target / (float)MenuSurvivorsClothingBoxUI.numBoxEntries * 3.1415927f * 2f;
			if (MenuSurvivorsClothingBoxUI.angle > num2 - num)
			{
				MenuSurvivorsClothingBoxUI.rotation = MenuSurvivorsClothingBoxUI.target;
				MenuSurvivorsClothingBoxUI.lastAngle -= 1f;
				return;
			}
			MenuSurvivorsClothingBoxUI.angle = num2 - Random.Range(num / 2f, num);
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x00161970 File Offset: 0x0015FB70
		public static void open()
		{
			if (MenuSurvivorsClothingBoxUI.active)
			{
				return;
			}
			MenuSurvivorsClothingBoxUI.active = true;
			MenuSurvivorsClothingBoxUI.container.AnimateIntoView();
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x0016198A File Offset: 0x0015FB8A
		public static void close()
		{
			if (!MenuSurvivorsClothingBoxUI.active)
			{
				return;
			}
			MenuSurvivorsClothingBoxUI.active = false;
			MenuSurvivorsClothingBoxUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x001619B0 File Offset: 0x0015FBB0
		public static void viewItem(int newItem, ushort newQuantity, ulong newInstance)
		{
			MenuSurvivorsClothingBoxUI.item = newItem;
			MenuSurvivorsClothingBoxUI.instance = newInstance;
			MenuSurvivorsClothingBoxUI.unboxedItems = null;
			MenuSurvivorsClothingBoxUI.didUnboxMythical = false;
			MenuSurvivorsClothingBoxUI.angle = 0f;
			MenuSurvivorsClothingBoxUI.lastRotation = 0;
			MenuSurvivorsClothingBoxUI.rotation = 0;
			MenuSurvivorsClothingBoxUI.target = -1;
			MenuSurvivorsClothingBoxUI.disabledBox.IsVisible = false;
			MenuSurvivorsClothingBoxUI.keyButton.IsVisible = true;
			MenuSurvivorsClothingBoxUI.unboxButton.IsVisible = true;
			MenuSurvivorsClothingBoxUI.boxButton.updateInventory(MenuSurvivorsClothingBoxUI.instance, MenuSurvivorsClothingBoxUI.item, newQuantity, false, true);
			MenuSurvivorsClothingBoxUI.boxAsset = Assets.find<ItemBoxAsset>(Provider.provider.economyService.getInventoryItemGuid(MenuSurvivorsClothingBoxUI.item));
			if (MenuSurvivorsClothingBoxUI.boxAsset != null)
			{
				MenuSurvivorsClothingBoxUI.organizeBoxEntries();
				MenuSurvivorsClothingBoxUI.synchronizeTotalProbabilities();
				string key = null;
				string key2 = null;
				EBoxItemOrigin itemOrigin = MenuSurvivorsClothingBoxUI.boxAsset.itemOrigin;
				if (itemOrigin != EBoxItemOrigin.Unbox)
				{
					if (itemOrigin == EBoxItemOrigin.Unwrap)
					{
						key = "Unwrap_Text";
						key2 = "Unwrap_Tooltip";
					}
				}
				else
				{
					key = "Unbox_Text";
					key2 = "Unbox_Tooltip";
				}
				if (!Provider.provider.economyService.doesCountryAllowRandomItems)
				{
					if (Provider.provider.economyService.hasCountryDetails)
					{
						MenuSurvivorsClothingBoxUI.disabledBox.IsVisible = true;
						MenuSurvivorsClothingBoxUI.disabledBox.Text = MenuSurvivorsClothingBoxUI.localization.format("Region_Disabled", Provider.provider.economyService.getCountryWarningId());
					}
					else
					{
						MenuSurvivorsClothingBoxUI.disabledBox.IsVisible = false;
					}
					MenuSurvivorsClothingBoxUI.unboxButton.IsVisible = false;
					MenuSurvivorsClothingBoxUI.keyButton.IsVisible = false;
				}
				else if (MenuSurvivorsClothingBoxUI.boxAsset.destroy == 0)
				{
					MenuSurvivorsClothingBoxUI.keyButton.IsVisible = false;
					MenuSurvivorsClothingBoxUI.unboxButton.icon = null;
					MenuSurvivorsClothingBoxUI.unboxButton.PositionOffset_X = 0f;
					MenuSurvivorsClothingBoxUI.unboxButton.PositionScale_X = 0.3f;
					MenuSurvivorsClothingBoxUI.unboxButton.SizeOffset_X = 0f;
					MenuSurvivorsClothingBoxUI.unboxButton.SizeScale_X = 0.4f;
					MenuSurvivorsClothingBoxUI.unboxButton.text = MenuSurvivorsClothingBoxUI.localization.format(key);
					MenuSurvivorsClothingBoxUI.unboxButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format(key2);
					MenuSurvivorsClothingBoxUI.unboxButton.IsVisible = true;
					MenuSurvivorsClothingBoxUI.keyAsset = null;
				}
				else
				{
					MenuSurvivorsClothingBoxUI.keyButton.IsVisible = true;
					MenuSurvivorsClothingBoxUI.unboxButton.icon = MenuSurvivorsClothingBoxUI.icons.load<Texture2D>("Unbox");
					MenuSurvivorsClothingBoxUI.unboxButton.PositionOffset_X = 5f;
					MenuSurvivorsClothingBoxUI.unboxButton.PositionScale_X = 0.5f;
					MenuSurvivorsClothingBoxUI.unboxButton.SizeOffset_X = -5f;
					MenuSurvivorsClothingBoxUI.unboxButton.SizeScale_X = 0.2f;
					MenuSurvivorsClothingBoxUI.unboxButton.text = MenuSurvivorsClothingBoxUI.localization.format(key);
					MenuSurvivorsClothingBoxUI.unboxButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format(key2);
					MenuSurvivorsClothingBoxUI.unboxButton.IsVisible = true;
					MenuSurvivorsClothingBoxUI.keyAsset = Assets.find<ItemKeyAsset>(Provider.provider.economyService.getInventoryItemGuid(MenuSurvivorsClothingBoxUI.boxAsset.destroy));
					if (MenuSurvivorsClothingBoxUI.keyAsset != null)
					{
						MenuSurvivorsClothingBoxUI.keyButton.icon = Provider.provider.economyService.LoadItemIcon(MenuSurvivorsClothingBoxUI.boxAsset.destroy);
					}
				}
				MenuSurvivorsClothingBoxUI.size = 6.2831855f / (float)MenuSurvivorsClothingBoxUI.numBoxEntries / 2.75f;
				MenuSurvivorsClothingBoxUI.finalBox.PositionScale_Y = 0.5f - MenuSurvivorsClothingBoxUI.size / 2f;
				MenuSurvivorsClothingBoxUI.finalBox.SizeScale_X = MenuSurvivorsClothingBoxUI.size;
				MenuSurvivorsClothingBoxUI.finalBox.SizeScale_Y = MenuSurvivorsClothingBoxUI.size;
				if (MenuSurvivorsClothingBoxUI.dropButtons != null)
				{
					for (int i = 0; i < MenuSurvivorsClothingBoxUI.dropButtons.Length; i++)
					{
						MenuSurvivorsClothingBoxUI.inventory.RemoveChild(MenuSurvivorsClothingBoxUI.dropButtons[i]);
					}
				}
				MenuSurvivorsClothingBoxUI.dropButtons = new SleekInventory[MenuSurvivorsClothingBoxUI.numBoxEntries];
				for (int j = 0; j < MenuSurvivorsClothingBoxUI.numBoxEntries; j++)
				{
					MenuSurvivorsClothingBoxUI.BoxEntry boxEntry = MenuSurvivorsClothingBoxUI.boxEntries[j];
					float num = 6.2831855f * (float)j / (float)MenuSurvivorsClothingBoxUI.numBoxEntries + 3.1415927f;
					SleekInventory sleekInventory = new SleekInventory();
					sleekInventory.PositionScale_X = 0.5f + Mathf.Cos(-num) * (0.5f - MenuSurvivorsClothingBoxUI.size / 2f) - MenuSurvivorsClothingBoxUI.size / 2f;
					sleekInventory.PositionScale_Y = 0.5f + Mathf.Sin(-num) * (0.5f - MenuSurvivorsClothingBoxUI.size / 2f) - MenuSurvivorsClothingBoxUI.size / 2f;
					sleekInventory.SizeScale_X = MenuSurvivorsClothingBoxUI.size;
					sleekInventory.SizeScale_Y = MenuSurvivorsClothingBoxUI.size;
					if (boxEntry.probability > -0.5f)
					{
						sleekInventory.extraTooltip = boxEntry.probability.ToString("P");
					}
					sleekInventory.updateInventory(0UL, boxEntry.id, 1, false, false);
					MenuSurvivorsClothingBoxUI.inventory.AddChild(sleekInventory);
					MenuSurvivorsClothingBoxUI.dropButtons[j] = sleekInventory;
				}
			}
			Color inventoryColor = Provider.provider.economyService.getInventoryColor(MenuSurvivorsClothingBoxUI.item);
			MenuSurvivorsClothingBoxUI.keyButton.backgroundColor = SleekColor.BackgroundIfLight(inventoryColor);
			MenuSurvivorsClothingBoxUI.keyButton.textColor = inventoryColor;
			MenuSurvivorsClothingBoxUI.unboxButton.backgroundColor = MenuSurvivorsClothingBoxUI.keyButton.backgroundColor;
			MenuSurvivorsClothingBoxUI.unboxButton.textColor = inventoryColor;
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x00161E88 File Offset: 0x00160088
		private static void synchronizeTotalProbabilities()
		{
			MenuSurvivorsClothingBoxUI.rareLabel.IsVisible = (MenuSurvivorsClothingBoxUI.boxAsset.probabilityModel == EBoxProbabilityModel.Original);
			MenuSurvivorsClothingBoxUI.epicLabel.IsVisible = (MenuSurvivorsClothingBoxUI.boxAsset.probabilityModel == EBoxProbabilityModel.Original);
			MenuSurvivorsClothingBoxUI.legendaryLabel.IsVisible = (MenuSurvivorsClothingBoxUI.boxAsset.probabilityModel == EBoxProbabilityModel.Original);
			MenuSurvivorsClothingBoxUI.equalizedLabel.IsVisible = (MenuSurvivorsClothingBoxUI.boxAsset.probabilityModel == EBoxProbabilityModel.Equalized);
			MenuSurvivorsClothingBoxUI.bonusLabel.IsVisible = MenuSurvivorsClothingBoxUI.boxAsset.containsBonusItems;
		}

		// Token: 0x060041C5 RID: 16837 RVA: 0x00161F08 File Offset: 0x00160108
		private static void organizeBoxEntries()
		{
			int[] drops = MenuSurvivorsClothingBoxUI.boxAsset.drops;
			int num = drops.Length;
			MenuSurvivorsClothingBoxUI.numBoxEntries = num;
			MenuSurvivorsClothingBoxUI.boxEntries = new List<MenuSurvivorsClothingBoxUI.BoxEntry>(MenuSurvivorsClothingBoxUI.numBoxEntries);
			Dictionary<EItemRarity, int> dictionary = new Dictionary<EItemRarity, int>();
			dictionary.Add(EItemRarity.RARE, 0);
			dictionary.Add(EItemRarity.EPIC, 0);
			dictionary.Add(EItemRarity.LEGENDARY, 0);
			dictionary.Add(EItemRarity.MYTHICAL, 0);
			Dictionary<EItemRarity, int> dictionary2 = dictionary;
			for (int i = 0; i < num; i++)
			{
				int num2 = drops[i];
				EItemRarity eitemRarity;
				if (num2 < 0)
				{
					eitemRarity = EItemRarity.MYTHICAL;
				}
				else
				{
					eitemRarity = Provider.provider.economyService.getGameRarity(num2);
				}
				Dictionary<EItemRarity, int> dictionary3 = dictionary2;
				EItemRarity eitemRarity2 = eitemRarity;
				int num3 = dictionary3[eitemRarity2];
				dictionary3[eitemRarity2] = num3 + 1;
				MenuSurvivorsClothingBoxUI.BoxEntry boxEntry = new MenuSurvivorsClothingBoxUI.BoxEntry
				{
					id = num2,
					rarity = eitemRarity,
					probability = -1f
				};
				MenuSurvivorsClothingBoxUI.boxEntries.Add(boxEntry);
			}
			float num4 = 0f;
			for (int j = 0; j < num; j++)
			{
				MenuSurvivorsClothingBoxUI.BoxEntry boxEntry2 = MenuSurvivorsClothingBoxUI.boxEntries[j];
				if (boxEntry2.rarity == EItemRarity.MYTHICAL)
				{
					boxEntry2.probability = MenuSurvivorsClothingBoxUI.qualityRarities[EItemRarity.MYTHICAL];
				}
				else
				{
					if (MenuSurvivorsClothingBoxUI.boxAsset.probabilityModel == EBoxProbabilityModel.Original)
					{
						int num5 = dictionary2[boxEntry2.rarity];
						float probability = MenuSurvivorsClothingBoxUI.qualityRarities[boxEntry2.rarity] / (float)num5;
						boxEntry2.probability = probability;
					}
					else
					{
						int num6 = num - 1;
						boxEntry2.probability = 1f / (float)num6;
					}
					num4 += boxEntry2.probability;
				}
				MenuSurvivorsClothingBoxUI.boxEntries[j] = boxEntry2;
			}
			if (Mathf.Abs(num4 - 1f) > 0.01f)
			{
				UnturnedLog.warn("Unable to guess box probabilities ({0})", new object[]
				{
					num4
				});
				for (int k = 0; k < num; k++)
				{
					MenuSurvivorsClothingBoxUI.BoxEntry boxEntry3 = MenuSurvivorsClothingBoxUI.boxEntries[k];
					boxEntry3.probability = -1f;
					MenuSurvivorsClothingBoxUI.boxEntries[k] = boxEntry3;
				}
			}
			MenuSurvivorsClothingBoxUI.boxEntries.Sort(new MenuSurvivorsClothingBoxUI.BoxEntryComparer());
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x00162107 File Offset: 0x00160307
		private static void onClickedKeyButton(ISleekElement button)
		{
			ItemStore.Get().ViewItem(MenuSurvivorsClothingBoxUI.boxAsset.destroy);
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x00162120 File Offset: 0x00160320
		private static void onClickedUnboxButton(ISleekElement button)
		{
			if (MenuSurvivorsClothingBoxUI.boxAsset.destroy == 0)
			{
				TempSteamworksEconomy economyService = Provider.provider.economyService;
				int generate = MenuSurvivorsClothingBoxUI.boxAsset.generate;
				List<EconExchangePair> list = new List<EconExchangePair>();
				list.Add(new EconExchangePair(MenuSurvivorsClothingBoxUI.instance, 1));
				economyService.exchangeInventory(generate, list);
			}
			else
			{
				ulong inventoryPackage = Provider.provider.economyService.getInventoryPackage(MenuSurvivorsClothingBoxUI.boxAsset.destroy);
				if (inventoryPackage == 0UL)
				{
					return;
				}
				List<EconExchangePair> list2 = new List<EconExchangePair>();
				list2.Add(new EconExchangePair(MenuSurvivorsClothingBoxUI.instance, 1));
				list2.Add(new EconExchangePair(inventoryPackage, 1));
				List<EconExchangePair> destroy = list2;
				Provider.provider.economyService.exchangeInventory(MenuSurvivorsClothingBoxUI.boxAsset.generate, destroy);
			}
			MenuSurvivorsClothingBoxUI.isUnboxing = true;
			MenuSurvivorsClothingBoxUI.backButton.IsVisible = false;
			MenuSurvivorsClothingBoxUI.lastUnbox = Time.realtimeSinceStartup;
			MenuSurvivorsClothingBoxUI.lastAngle = Time.realtimeSinceStartup;
			MenuSurvivorsClothingBoxUI.keyButton.IsVisible = false;
			MenuSurvivorsClothingBoxUI.unboxButton.IsVisible = false;
		}

		/// <summary>
		/// Does client know about all the granted items?
		/// If not, either something is bad in the econ config (uh oh!) or client is out of date.
		/// </summary>
		// Token: 0x060041C8 RID: 16840 RVA: 0x00162204 File Offset: 0x00160404
		private static bool hasAssetsForGrantedItems(List<SteamItemDetails_t> grantedItems)
		{
			foreach (SteamItemDetails_t steamItemDetails_t in grantedItems)
			{
				Guid guid;
				Guid guid2;
				Provider.provider.economyService.getInventoryTargetID(steamItemDetails_t.m_iDefinition.m_SteamItemDef, out guid, out guid2);
				if (guid != default(Guid))
				{
					if (Assets.find<ItemAsset>(guid) == null)
					{
						return false;
					}
				}
				else
				{
					if (!(guid2 != default(Guid)))
					{
						return false;
					}
					if (VehicleTool.FindVehicleByGuidAndHandleRedirects(guid2) == null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x001622B0 File Offset: 0x001604B0
		private static bool wasGrantedMythical(List<SteamItemDetails_t> grantedItems)
		{
			foreach (SteamItemDetails_t steamItemDetails_t in grantedItems)
			{
				if (Provider.provider.economyService.getInventoryMythicID(steamItemDetails_t.m_iDefinition.m_SteamItemDef) != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x0016231C File Offset: 0x0016051C
		private static int getIndexOfGrantedItemInDrops(List<SteamItemDetails_t> grantedItems)
		{
			for (int i = 1; i < MenuSurvivorsClothingBoxUI.numBoxEntries; i++)
			{
				int id = MenuSurvivorsClothingBoxUI.boxEntries[i].id;
				using (List<SteamItemDetails_t>.Enumerator enumerator = grantedItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.m_iDefinition.m_SteamItemDef == id)
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x00162398 File Offset: 0x00160598
		private static void exchangeErrorAlert(string message)
		{
			MenuSurvivorsClothingBoxUI.isUnboxing = false;
			MenuSurvivorsClothingBoxUI.backButton.IsVisible = true;
			MenuUI.alert(message);
			MenuSurvivorsClothingUI.open();
			MenuSurvivorsClothingBoxUI.close();
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x001623BC File Offset: 0x001605BC
		private static void onInventoryExchanged(List<SteamItemDetails_t> grantedItems)
		{
			if (!MenuSurvivorsClothingBoxUI.isUnboxing)
			{
				return;
			}
			if (!MenuSurvivorsClothingBoxUI.hasAssetsForGrantedItems(grantedItems))
			{
				MenuSurvivorsClothingBoxUI.exchangeErrorAlert(MenuSurvivorsClothingBoxUI.localization.format("Exchange_Missing_Assets"));
				return;
			}
			MenuSurvivorsClothingUI.updatePage();
			int num;
			if (MenuSurvivorsClothingBoxUI.wasGrantedMythical(grantedItems))
			{
				MenuSurvivorsClothingBoxUI.didUnboxMythical = true;
				num = 0;
			}
			else
			{
				MenuSurvivorsClothingBoxUI.didUnboxMythical = false;
				num = MenuSurvivorsClothingBoxUI.getIndexOfGrantedItemInDrops(grantedItems);
				if (num < 0)
				{
					MenuSurvivorsClothingBoxUI.exchangeErrorAlert(MenuSurvivorsClothingBoxUI.localization.format("Exchange_Not_In_Drops"));
					return;
				}
			}
			MenuSurvivorsClothingBoxUI.unboxedItems = grantedItems;
			MenuSurvivorsClothingBoxUI.unboxedItems.Sort(new EconItemRarityComparer());
			if (MenuSurvivorsClothingBoxUI.rotation < MenuSurvivorsClothingBoxUI.numBoxEntries * 2)
			{
				MenuSurvivorsClothingBoxUI.target = MenuSurvivorsClothingBoxUI.numBoxEntries * 3 + num;
				return;
			}
			MenuSurvivorsClothingBoxUI.target = ((int)((float)MenuSurvivorsClothingBoxUI.rotation / (float)MenuSurvivorsClothingBoxUI.numBoxEntries) + 2) * MenuSurvivorsClothingBoxUI.numBoxEntries + num;
		}

		// Token: 0x060041CD RID: 16845 RVA: 0x0016247C File Offset: 0x0016067C
		public static void update()
		{
			if (!MenuSurvivorsClothingBoxUI.isUnboxing)
			{
				return;
			}
			if (Time.realtimeSinceStartup - MenuSurvivorsClothingBoxUI.lastUnbox > (float)Provider.CLIENT_TIMEOUT)
			{
				MenuSurvivorsClothingBoxUI.isUnboxing = false;
				MenuSurvivorsClothingBoxUI.backButton.IsVisible = true;
				MenuUI.alert(MenuSurvivorsClothingBoxUI.localization.format("Exchange_Timed_Out"));
				MenuSurvivorsClothingUI.open();
				MenuSurvivorsClothingBoxUI.close();
				return;
			}
			if (MenuSurvivorsClothingBoxUI.rotation == MenuSurvivorsClothingBoxUI.target)
			{
				if (Time.realtimeSinceStartup - MenuSurvivorsClothingBoxUI.lastAngle > 0.5f)
				{
					MenuSurvivorsClothingBoxUI.isUnboxing = false;
					MenuSurvivorsClothingBoxUI.backButton.IsVisible = true;
					string key = null;
					EBoxItemOrigin itemOrigin = MenuSurvivorsClothingBoxUI.boxAsset.itemOrigin;
					if (itemOrigin != EBoxItemOrigin.Unbox)
					{
						if (itemOrigin == EBoxItemOrigin.Unwrap)
						{
							key = "Origin_Unwrap";
						}
					}
					else
					{
						key = "Origin_Unbox";
					}
					MenuUI.alertNewItems(MenuSurvivorsClothingBoxUI.localization.format(key), MenuSurvivorsClothingBoxUI.unboxedItems);
					SteamItemDetails_t steamItemDetails_t = MenuSurvivorsClothingBoxUI.unboxedItems[0];
					MenuSurvivorsClothingItemUI.viewItem(steamItemDetails_t.m_iDefinition.m_SteamItemDef, steamItemDetails_t.m_unQuantity, steamItemDetails_t.m_itemId.m_SteamItemInstanceID);
					MenuSurvivorsClothingItemUI.open();
					MenuSurvivorsClothingBoxUI.close();
					string path = MenuSurvivorsClothingBoxUI.didUnboxMythical ? "Economy/Sounds/Mythical" : "Economy/Sounds/Unbox";
					MainCamera.instance.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>(path), 0.66f);
					return;
				}
			}
			else
			{
				if (MenuSurvivorsClothingBoxUI.rotation < MenuSurvivorsClothingBoxUI.target - MenuSurvivorsClothingBoxUI.numBoxEntries || MenuSurvivorsClothingBoxUI.target == -1)
				{
					if (MenuSurvivorsClothingBoxUI.angle < 12.566371f)
					{
						MenuSurvivorsClothingBoxUI.angle += (Time.realtimeSinceStartup - MenuSurvivorsClothingBoxUI.lastAngle) * MenuSurvivorsClothingBoxUI.size * Mathf.Lerp(80f, 20f, MenuSurvivorsClothingBoxUI.angle / 12.566371f);
					}
					else
					{
						MenuSurvivorsClothingBoxUI.angle += (Time.realtimeSinceStartup - MenuSurvivorsClothingBoxUI.lastAngle) * MenuSurvivorsClothingBoxUI.size * 20f;
					}
				}
				else
				{
					MenuSurvivorsClothingBoxUI.angle += (Time.realtimeSinceStartup - MenuSurvivorsClothingBoxUI.lastAngle) * Mathf.Max(((float)MenuSurvivorsClothingBoxUI.target - MenuSurvivorsClothingBoxUI.angle / (6.2831855f / (float)MenuSurvivorsClothingBoxUI.numBoxEntries)) / (float)MenuSurvivorsClothingBoxUI.numBoxEntries, 0.05f) * MenuSurvivorsClothingBoxUI.size * 20f;
				}
				MenuSurvivorsClothingBoxUI.lastAngle = Time.realtimeSinceStartup;
				MenuSurvivorsClothingBoxUI.rotation = (int)(MenuSurvivorsClothingBoxUI.angle / (6.2831855f / (float)MenuSurvivorsClothingBoxUI.numBoxEntries));
				if (MenuSurvivorsClothingBoxUI.rotation == MenuSurvivorsClothingBoxUI.target)
				{
					MenuSurvivorsClothingBoxUI.angle = (float)MenuSurvivorsClothingBoxUI.rotation * (6.2831855f / (float)MenuSurvivorsClothingBoxUI.numBoxEntries);
				}
				for (int i = 0; i < MenuSurvivorsClothingBoxUI.numBoxEntries; i++)
				{
					float num = 6.2831855f * (float)i / (float)MenuSurvivorsClothingBoxUI.numBoxEntries + 3.1415927f;
					MenuSurvivorsClothingBoxUI.dropButtons[i].PositionScale_X = 0.5f + Mathf.Cos(MenuSurvivorsClothingBoxUI.angle - num) * (0.5f - MenuSurvivorsClothingBoxUI.size / 2f) - MenuSurvivorsClothingBoxUI.size / 2f;
					MenuSurvivorsClothingBoxUI.dropButtons[i].PositionScale_Y = 0.5f + Mathf.Sin(MenuSurvivorsClothingBoxUI.angle - num) * (0.5f - MenuSurvivorsClothingBoxUI.size / 2f) - MenuSurvivorsClothingBoxUI.size / 2f;
				}
				if (MenuSurvivorsClothingBoxUI.rotation != MenuSurvivorsClothingBoxUI.lastRotation)
				{
					MenuSurvivorsClothingBoxUI.lastRotation = MenuSurvivorsClothingBoxUI.rotation;
					MenuSurvivorsClothingBoxUI.boxButton.PositionScale_Y = 0.25f;
					MenuSurvivorsClothingBoxUI.boxButton.AnimatePositionScale(0.3f, 0.3f, 1, 20f);
					MenuSurvivorsClothingBoxUI.finalBox.PositionOffset_X = -20f;
					MenuSurvivorsClothingBoxUI.finalBox.PositionOffset_Y = -20f;
					MenuSurvivorsClothingBoxUI.finalBox.SizeOffset_X = 40f;
					MenuSurvivorsClothingBoxUI.finalBox.SizeOffset_Y = 40f;
					MenuSurvivorsClothingBoxUI.finalBox.AnimatePositionOffset(-10f, -10f, 1, 1f);
					MenuSurvivorsClothingBoxUI.finalBox.AnimateSizeOffset(20f, 20f, 1, 1f);
					MenuSurvivorsClothingBoxUI.boxButton.updateInventory(0UL, MenuSurvivorsClothingBoxUI.boxEntries[MenuSurvivorsClothingBoxUI.rotation % MenuSurvivorsClothingBoxUI.numBoxEntries].id, 1, false, true);
					if (MenuSurvivorsClothingBoxUI.rotation == MenuSurvivorsClothingBoxUI.target)
					{
						MainCamera.instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Economy/Sounds/Drop"), 0.33f);
						return;
					}
					MainCamera.instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Economy/Sounds/Tick"), 0.33f);
				}
			}
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x0016289A File Offset: 0x00160A9A
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuSurvivorsClothingItemUI.open();
			MenuSurvivorsClothingBoxUI.close();
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x001628A6 File Offset: 0x00160AA6
		public void OnDestroy()
		{
			TempSteamworksEconomy economyService = Provider.provider.economyService;
			economyService.onInventoryExchanged = (TempSteamworksEconomy.InventoryExchanged)Delegate.Remove(economyService.onInventoryExchanged, new TempSteamworksEconomy.InventoryExchanged(MenuSurvivorsClothingBoxUI.onInventoryExchanged));
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x001628D4 File Offset: 0x00160AD4
		public MenuSurvivorsClothingBoxUI()
		{
			MenuSurvivorsClothingBoxUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsClothingBox.dat");
			if (MenuSurvivorsClothingBoxUI.icons != null)
			{
				MenuSurvivorsClothingBoxUI.icons.unload();
			}
			MenuSurvivorsClothingBoxUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Survivors/MenuSurvivorsClothingBox/MenuSurvivorsClothingBox.unity3d");
			MenuSurvivorsClothingBoxUI.container = new SleekFullscreenBox();
			MenuSurvivorsClothingBoxUI.container.PositionOffset_X = 10f;
			MenuSurvivorsClothingBoxUI.container.PositionOffset_Y = 10f;
			MenuSurvivorsClothingBoxUI.container.PositionScale_Y = 1f;
			MenuSurvivorsClothingBoxUI.container.SizeOffset_X = -20f;
			MenuSurvivorsClothingBoxUI.container.SizeOffset_Y = -20f;
			MenuSurvivorsClothingBoxUI.container.SizeScale_X = 1f;
			MenuSurvivorsClothingBoxUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuSurvivorsClothingBoxUI.container);
			MenuSurvivorsClothingBoxUI.active = false;
			MenuSurvivorsClothingBoxUI.inventory = Glazier.Get().CreateConstraintFrame();
			MenuSurvivorsClothingBoxUI.inventory.PositionScale_X = 0.5f;
			MenuSurvivorsClothingBoxUI.inventory.PositionOffset_Y = 10f;
			MenuSurvivorsClothingBoxUI.inventory.SizeScale_X = 0.5f;
			MenuSurvivorsClothingBoxUI.inventory.SizeScale_Y = 1f;
			MenuSurvivorsClothingBoxUI.inventory.SizeOffset_Y = -20f;
			MenuSurvivorsClothingBoxUI.inventory.Constraint = 1;
			MenuSurvivorsClothingBoxUI.container.AddChild(MenuSurvivorsClothingBoxUI.inventory);
			MenuSurvivorsClothingBoxUI.finalBox = Glazier.Get().CreateBox();
			MenuSurvivorsClothingBoxUI.finalBox.PositionOffset_X = -10f;
			MenuSurvivorsClothingBoxUI.finalBox.PositionOffset_Y = -10f;
			MenuSurvivorsClothingBoxUI.finalBox.SizeOffset_X = 20f;
			MenuSurvivorsClothingBoxUI.finalBox.SizeOffset_Y = 20f;
			MenuSurvivorsClothingBoxUI.inventory.AddChild(MenuSurvivorsClothingBoxUI.finalBox);
			MenuSurvivorsClothingBoxUI.boxButton = new SleekInventory();
			MenuSurvivorsClothingBoxUI.boxButton.PositionOffset_Y = -30f;
			MenuSurvivorsClothingBoxUI.boxButton.PositionScale_X = 0.3f;
			MenuSurvivorsClothingBoxUI.boxButton.PositionScale_Y = 0.3f;
			MenuSurvivorsClothingBoxUI.boxButton.SizeScale_X = 0.4f;
			MenuSurvivorsClothingBoxUI.boxButton.SizeScale_Y = 0.4f;
			MenuSurvivorsClothingBoxUI.inventory.AddChild(MenuSurvivorsClothingBoxUI.boxButton);
			MenuSurvivorsClothingBoxUI.keyButton = new SleekButtonIcon(null, 40);
			MenuSurvivorsClothingBoxUI.keyButton.PositionOffset_Y = -20f;
			MenuSurvivorsClothingBoxUI.keyButton.PositionScale_X = 0.3f;
			MenuSurvivorsClothingBoxUI.keyButton.PositionScale_Y = 0.7f;
			MenuSurvivorsClothingBoxUI.keyButton.SizeOffset_X = -5f;
			MenuSurvivorsClothingBoxUI.keyButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingBoxUI.keyButton.SizeScale_X = 0.2f;
			MenuSurvivorsClothingBoxUI.keyButton.text = MenuSurvivorsClothingBoxUI.localization.format("Key_Text");
			MenuSurvivorsClothingBoxUI.keyButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format("Key_Tooltip");
			MenuSurvivorsClothingBoxUI.keyButton.onClickedButton += new ClickedButton(MenuSurvivorsClothingBoxUI.onClickedKeyButton);
			MenuSurvivorsClothingBoxUI.keyButton.fontSize = 3;
			MenuSurvivorsClothingBoxUI.keyButton.shadowStyle = 1;
			MenuSurvivorsClothingBoxUI.inventory.AddChild(MenuSurvivorsClothingBoxUI.keyButton);
			MenuSurvivorsClothingBoxUI.keyButton.IsVisible = false;
			MenuSurvivorsClothingBoxUI.unboxButton = new SleekButtonIcon(null);
			MenuSurvivorsClothingBoxUI.unboxButton.PositionOffset_X = 5f;
			MenuSurvivorsClothingBoxUI.unboxButton.PositionOffset_Y = -20f;
			MenuSurvivorsClothingBoxUI.unboxButton.PositionScale_X = 0.5f;
			MenuSurvivorsClothingBoxUI.unboxButton.PositionScale_Y = 0.7f;
			MenuSurvivorsClothingBoxUI.unboxButton.SizeOffset_X = -5f;
			MenuSurvivorsClothingBoxUI.unboxButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingBoxUI.unboxButton.SizeScale_X = 0.2f;
			MenuSurvivorsClothingBoxUI.unboxButton.text = MenuSurvivorsClothingBoxUI.localization.format("Unbox_Text");
			MenuSurvivorsClothingBoxUI.unboxButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format("Unbox_Tooltip");
			MenuSurvivorsClothingBoxUI.unboxButton.onClickedButton += new ClickedButton(MenuSurvivorsClothingBoxUI.onClickedUnboxButton);
			MenuSurvivorsClothingBoxUI.unboxButton.fontSize = 3;
			MenuSurvivorsClothingBoxUI.unboxButton.shadowStyle = 1;
			MenuSurvivorsClothingBoxUI.inventory.AddChild(MenuSurvivorsClothingBoxUI.unboxButton);
			MenuSurvivorsClothingBoxUI.unboxButton.IsVisible = false;
			MenuSurvivorsClothingBoxUI.disabledBox = Glazier.Get().CreateBox();
			MenuSurvivorsClothingBoxUI.disabledBox.PositionOffset_Y = -20f;
			MenuSurvivorsClothingBoxUI.disabledBox.PositionScale_X = 0.3f;
			MenuSurvivorsClothingBoxUI.disabledBox.PositionScale_Y = 0.7f;
			MenuSurvivorsClothingBoxUI.disabledBox.SizeOffset_Y = 50f;
			MenuSurvivorsClothingBoxUI.disabledBox.SizeScale_X = 0.4f;
			MenuSurvivorsClothingBoxUI.inventory.AddChild(MenuSurvivorsClothingBoxUI.disabledBox);
			MenuSurvivorsClothingBoxUI.disabledBox.IsVisible = false;
			MenuSurvivorsClothingBoxUI.rareLabel = Glazier.Get().CreateLabel();
			MenuSurvivorsClothingBoxUI.rareLabel.PositionOffset_X = 50f;
			MenuSurvivorsClothingBoxUI.rareLabel.PositionOffset_Y = 50f;
			MenuSurvivorsClothingBoxUI.rareLabel.SizeOffset_X = 200f;
			MenuSurvivorsClothingBoxUI.rareLabel.SizeOffset_Y = 30f;
			MenuSurvivorsClothingBoxUI.rareLabel.Text = MenuSurvivorsClothingBoxUI.localization.format("Rarity_Rare", MenuSurvivorsClothingBoxUI.formatQualityRarity(EItemRarity.RARE));
			MenuSurvivorsClothingBoxUI.rareLabel.TextColor = ItemTool.getRarityColorUI(EItemRarity.RARE);
			MenuSurvivorsClothingBoxUI.rareLabel.TextAlignment = 3;
			MenuSurvivorsClothingBoxUI.rareLabel.TextContrastContext = 2;
			MenuSurvivorsClothingBoxUI.container.AddChild(MenuSurvivorsClothingBoxUI.rareLabel);
			MenuSurvivorsClothingBoxUI.epicLabel = Glazier.Get().CreateLabel();
			MenuSurvivorsClothingBoxUI.epicLabel.PositionOffset_X = 50f;
			MenuSurvivorsClothingBoxUI.epicLabel.PositionOffset_Y = 70f;
			MenuSurvivorsClothingBoxUI.epicLabel.SizeOffset_X = 200f;
			MenuSurvivorsClothingBoxUI.epicLabel.SizeOffset_Y = 30f;
			MenuSurvivorsClothingBoxUI.epicLabel.Text = MenuSurvivorsClothingBoxUI.localization.format("Rarity_Epic", MenuSurvivorsClothingBoxUI.formatQualityRarity(EItemRarity.EPIC));
			MenuSurvivorsClothingBoxUI.epicLabel.TextColor = ItemTool.getRarityColorUI(EItemRarity.EPIC);
			MenuSurvivorsClothingBoxUI.epicLabel.TextAlignment = 3;
			MenuSurvivorsClothingBoxUI.epicLabel.TextContrastContext = 2;
			MenuSurvivorsClothingBoxUI.container.AddChild(MenuSurvivorsClothingBoxUI.epicLabel);
			MenuSurvivorsClothingBoxUI.legendaryLabel = Glazier.Get().CreateLabel();
			MenuSurvivorsClothingBoxUI.legendaryLabel.PositionOffset_X = 50f;
			MenuSurvivorsClothingBoxUI.legendaryLabel.PositionOffset_Y = 90f;
			MenuSurvivorsClothingBoxUI.legendaryLabel.SizeOffset_X = 200f;
			MenuSurvivorsClothingBoxUI.legendaryLabel.SizeOffset_Y = 30f;
			MenuSurvivorsClothingBoxUI.legendaryLabel.Text = MenuSurvivorsClothingBoxUI.localization.format("Rarity_Legendary", MenuSurvivorsClothingBoxUI.formatQualityRarity(EItemRarity.LEGENDARY));
			MenuSurvivorsClothingBoxUI.legendaryLabel.TextColor = ItemTool.getRarityColorUI(EItemRarity.LEGENDARY);
			MenuSurvivorsClothingBoxUI.legendaryLabel.TextAlignment = 3;
			MenuSurvivorsClothingBoxUI.legendaryLabel.TextContrastContext = 2;
			MenuSurvivorsClothingBoxUI.container.AddChild(MenuSurvivorsClothingBoxUI.legendaryLabel);
			MenuSurvivorsClothingBoxUI.mythicalLabel = Glazier.Get().CreateLabel();
			MenuSurvivorsClothingBoxUI.mythicalLabel.PositionOffset_X = 50f;
			MenuSurvivorsClothingBoxUI.mythicalLabel.PositionOffset_Y = 110f;
			MenuSurvivorsClothingBoxUI.mythicalLabel.SizeOffset_X = 200f;
			MenuSurvivorsClothingBoxUI.mythicalLabel.SizeOffset_Y = 30f;
			MenuSurvivorsClothingBoxUI.mythicalLabel.Text = MenuSurvivorsClothingBoxUI.localization.format("Rarity_Mythical", MenuSurvivorsClothingBoxUI.formatQualityRarity(EItemRarity.MYTHICAL));
			MenuSurvivorsClothingBoxUI.mythicalLabel.TextColor = ItemTool.getRarityColorUI(EItemRarity.MYTHICAL);
			MenuSurvivorsClothingBoxUI.mythicalLabel.TextAlignment = 3;
			MenuSurvivorsClothingBoxUI.mythicalLabel.TextContrastContext = 2;
			MenuSurvivorsClothingBoxUI.container.AddChild(MenuSurvivorsClothingBoxUI.mythicalLabel);
			MenuSurvivorsClothingBoxUI.equalizedLabel = Glazier.Get().CreateLabel();
			MenuSurvivorsClothingBoxUI.equalizedLabel.PositionOffset_X = 50f;
			MenuSurvivorsClothingBoxUI.equalizedLabel.PositionOffset_Y = 50f;
			MenuSurvivorsClothingBoxUI.equalizedLabel.SizeOffset_X = 200f;
			MenuSurvivorsClothingBoxUI.equalizedLabel.SizeOffset_Y = 30f;
			MenuSurvivorsClothingBoxUI.equalizedLabel.Text = MenuSurvivorsClothingBoxUI.localization.format("Rarity_Equalized");
			MenuSurvivorsClothingBoxUI.equalizedLabel.TextAlignment = 3;
			MenuSurvivorsClothingBoxUI.equalizedLabel.TextContrastContext = 2;
			MenuSurvivorsClothingBoxUI.container.AddChild(MenuSurvivorsClothingBoxUI.equalizedLabel);
			MenuSurvivorsClothingBoxUI.bonusLabel = Glazier.Get().CreateLabel();
			MenuSurvivorsClothingBoxUI.bonusLabel.PositionOffset_X = 50f;
			MenuSurvivorsClothingBoxUI.bonusLabel.PositionOffset_Y = 130f;
			MenuSurvivorsClothingBoxUI.bonusLabel.SizeOffset_X = 200f;
			MenuSurvivorsClothingBoxUI.bonusLabel.SizeOffset_Y = 30f;
			MenuSurvivorsClothingBoxUI.bonusLabel.Text = MenuSurvivorsClothingBoxUI.localization.format("Rarity_Bonus_Items", (MenuSurvivorsClothingBoxUI.BONUS_ITEM_RARITY * 100f).ToString("0.0"));
			MenuSurvivorsClothingBoxUI.bonusLabel.TextAlignment = 3;
			MenuSurvivorsClothingBoxUI.bonusLabel.TextContrastContext = 2;
			MenuSurvivorsClothingBoxUI.container.AddChild(MenuSurvivorsClothingBoxUI.bonusLabel);
			MenuSurvivorsClothingBoxUI.dropButtons = null;
			TempSteamworksEconomy economyService = Provider.provider.economyService;
			economyService.onInventoryExchanged = (TempSteamworksEconomy.InventoryExchanged)Delegate.Combine(economyService.onInventoryExchanged, new TempSteamworksEconomy.InventoryExchanged(MenuSurvivorsClothingBoxUI.onInventoryExchanged));
			MenuSurvivorsClothingBoxUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuSurvivorsClothingBoxUI.backButton.PositionOffset_Y = -50f;
			MenuSurvivorsClothingBoxUI.backButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingBoxUI.backButton.SizeOffset_X = 200f;
			MenuSurvivorsClothingBoxUI.backButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingBoxUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsClothingBoxUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuSurvivorsClothingBoxUI.backButton.onClickedButton += new ClickedButton(MenuSurvivorsClothingBoxUI.onClickedBackButton);
			MenuSurvivorsClothingBoxUI.backButton.fontSize = 3;
			MenuSurvivorsClothingBoxUI.backButton.iconColor = 2;
			MenuSurvivorsClothingBoxUI.container.AddChild(MenuSurvivorsClothingBoxUI.backButton);
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x001631C4 File Offset: 0x001613C4
		// Note: this type is marked as 'beforefieldinit'.
		static MenuSurvivorsClothingBoxUI()
		{
			Dictionary<EItemRarity, float> dictionary = new Dictionary<EItemRarity, float>();
			dictionary.Add(EItemRarity.RARE, 0.75f);
			dictionary.Add(EItemRarity.EPIC, 0.2f);
			dictionary.Add(EItemRarity.LEGENDARY, 0.05f);
			dictionary.Add(EItemRarity.MYTHICAL, 0.03f);
			MenuSurvivorsClothingBoxUI.qualityRarities = dictionary;
			MenuSurvivorsClothingBoxUI.BONUS_ITEM_RARITY = 0.1f;
		}

		// Token: 0x04002AB3 RID: 10931
		private static Dictionary<EItemRarity, float> qualityRarities;

		// Token: 0x04002AB4 RID: 10932
		private static readonly float BONUS_ITEM_RARITY;

		// Token: 0x04002AB5 RID: 10933
		private static Bundle icons;

		// Token: 0x04002AB6 RID: 10934
		private static Local localization;

		// Token: 0x04002AB7 RID: 10935
		private static SleekFullscreenBox container;

		// Token: 0x04002AB8 RID: 10936
		public static bool active;

		// Token: 0x04002AB9 RID: 10937
		private static SleekButtonIcon backButton;

		// Token: 0x04002ABA RID: 10938
		public static bool isUnboxing;

		// Token: 0x04002ABB RID: 10939
		private static float lastUnbox;

		// Token: 0x04002ABC RID: 10940
		private static float lastAngle;

		// Token: 0x04002ABD RID: 10941
		private static float angle;

		// Token: 0x04002ABE RID: 10942
		private static int lastRotation;

		// Token: 0x04002ABF RID: 10943
		private static int rotation;

		// Token: 0x04002AC0 RID: 10944
		private static int target;

		// Token: 0x04002AC1 RID: 10945
		private static int item;

		// Token: 0x04002AC2 RID: 10946
		private static ulong instance;

		/// <summary>
		/// Items server told us we unboxed, but we wait for the animation to finish before showing.
		/// Typically one, but some newer boxes have bonus items occassionally.
		/// </summary>
		// Token: 0x04002AC3 RID: 10947
		private static List<SteamItemDetails_t> unboxedItems;

		/// <summary>
		/// Is one of the unboxed items mythical rarity?
		/// </summary>
		// Token: 0x04002AC4 RID: 10948
		private static bool didUnboxMythical;

		/// <summary>
		/// Items in the box.
		/// </summary>
		// Token: 0x04002AC5 RID: 10949
		private static List<MenuSurvivorsClothingBoxUI.BoxEntry> boxEntries;

		// Token: 0x04002AC6 RID: 10950
		private static int numBoxEntries;

		// Token: 0x04002AC7 RID: 10951
		private static ItemBoxAsset boxAsset;

		// Token: 0x04002AC8 RID: 10952
		private static ItemKeyAsset keyAsset;

		// Token: 0x04002AC9 RID: 10953
		private static float size;

		// Token: 0x04002ACA RID: 10954
		private static ISleekConstraintFrame inventory;

		// Token: 0x04002ACB RID: 10955
		private static ISleekBox finalBox;

		// Token: 0x04002ACC RID: 10956
		private static SleekInventory boxButton;

		// Token: 0x04002ACD RID: 10957
		private static SleekButtonIcon keyButton;

		// Token: 0x04002ACE RID: 10958
		private static SleekButtonIcon unboxButton;

		// Token: 0x04002ACF RID: 10959
		private static ISleekBox disabledBox;

		// Token: 0x04002AD0 RID: 10960
		private static ISleekLabel rareLabel;

		// Token: 0x04002AD1 RID: 10961
		private static ISleekLabel epicLabel;

		// Token: 0x04002AD2 RID: 10962
		private static ISleekLabel legendaryLabel;

		// Token: 0x04002AD3 RID: 10963
		private static ISleekLabel mythicalLabel;

		// Token: 0x04002AD4 RID: 10964
		private static ISleekLabel equalizedLabel;

		// Token: 0x04002AD5 RID: 10965
		private static ISleekLabel bonusLabel;

		// Token: 0x04002AD6 RID: 10966
		private static SleekInventory[] dropButtons;

		/// <summary>
		/// Internal struct menu uses to sort items in box.
		/// </summary>
		// Token: 0x02000A09 RID: 2569
		private struct BoxEntry
		{
			/// <summary>
			/// Item definition id.
			/// </summary>
			// Token: 0x04003503 RID: 13571
			public int id;

			/// <summary>
			/// Rarity used to sort mythical &gt; legendary &gt; epic &gt; rare.
			/// </summary>
			// Token: 0x04003504 RID: 13572
			public EItemRarity rarity;

			/// <summary>
			/// [0, 1] calculated chance of this item being unboxed.
			/// Shown to player in item tooltips.
			/// </summary>
			// Token: 0x04003505 RID: 13573
			public float probability;
		}

		/// <summary>
		/// Sorts box entries from highest to lowest rarity.
		/// </summary>
		// Token: 0x02000A0A RID: 2570
		private class BoxEntryComparer : Comparer<MenuSurvivorsClothingBoxUI.BoxEntry>
		{
			// Token: 0x06004D4E RID: 19790 RVA: 0x001B947C File Offset: 0x001B767C
			public override int Compare(MenuSurvivorsClothingBoxUI.BoxEntry x, MenuSurvivorsClothingBoxUI.BoxEntry y)
			{
				int num = x.rarity.CompareTo(y.rarity);
				if (num == 0)
				{
					string inventoryName = Provider.provider.economyService.getInventoryName(x.id);
					string inventoryName2 = Provider.provider.economyService.getInventoryName(y.id);
					return -inventoryName.CompareTo(inventoryName2);
				}
				return -num;
			}
		}
	}
}
