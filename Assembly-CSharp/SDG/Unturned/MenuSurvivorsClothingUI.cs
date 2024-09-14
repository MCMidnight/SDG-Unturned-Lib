using System;
using System.Collections.Generic;
using SDG.Provider;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007B0 RID: 1968
	public class MenuSurvivorsClothingUI
	{
		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x060041ED RID: 16877 RVA: 0x001651A1 File Offset: 0x001633A1
		private static int numberOfPages
		{
			get
			{
				return MathfEx.GetPageCount(MenuSurvivorsClothingUI.filteredItems.Count, 25);
			}
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x001651B4 File Offset: 0x001633B4
		public static void open()
		{
			if (MenuSurvivorsClothingUI.active)
			{
				return;
			}
			MenuSurvivorsClothingUI.active = true;
			Characters.apply(false, true);
			MenuSurvivorsClothingUI.container.AnimateIntoView();
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x001651D8 File Offset: 0x001633D8
		public static void close()
		{
			if (!MenuSurvivorsClothingUI.active)
			{
				return;
			}
			MenuSurvivorsClothingUI.active = false;
			if (!MenuSurvivorsClothingBoxUI.active && !MenuSurvivorsClothingInspectUI.active && !MenuSurvivorsClothingDeleteUI.active && !MenuSurvivorsClothingItemUI.active)
			{
				Characters.apply(true, true);
			}
			MenuSurvivorsClothingUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x0016522C File Offset: 0x0016342C
		public static void setFilter(EEconFilterMode newFilterMode, ulong newFilterInstigator)
		{
			MenuSurvivorsClothingUI.setCrafting(false);
			MenuSurvivorsClothingUI.filterMode = newFilterMode;
			MenuSurvivorsClothingUI.filterInstigator = newFilterInstigator;
			MenuSurvivorsClothingUI.filterBox.IsVisible = (MenuSurvivorsClothingUI.filterMode > EEconFilterMode.SEARCH);
			MenuSurvivorsClothingUI.cancelFilterButton.IsVisible = (MenuSurvivorsClothingUI.filterMode > EEconFilterMode.SEARCH);
			if (MenuSurvivorsClothingUI.filterMode != EEconFilterMode.SEARCH)
			{
				MenuSurvivorsClothingUI.searchField.Text = string.Empty;
			}
			if (MenuSurvivorsClothingUI.filterMode == EEconFilterMode.STAT_TRACKER || MenuSurvivorsClothingUI.filterMode == EEconFilterMode.STAT_TRACKER_REMOVAL || MenuSurvivorsClothingUI.filterMode == EEconFilterMode.RAGDOLL_EFFECT_REMOVAL || MenuSurvivorsClothingUI.filterMode == EEconFilterMode.RAGDOLL_EFFECT)
			{
				int inventoryItem = Provider.provider.economyService.getInventoryItem(MenuSurvivorsClothingUI.filterInstigator);
				string inventoryName = Provider.provider.economyService.getInventoryName(inventoryItem);
				Color inventoryColor = Provider.provider.economyService.getInventoryColor(inventoryItem);
				string arg = string.Concat(new string[]
				{
					"<color=",
					Palette.hex(inventoryColor),
					">",
					inventoryName,
					"</color>"
				});
				MenuSurvivorsClothingUI.filterBox.Text = MenuSurvivorsClothingUI.localization.format("Filter_Item_Target", arg);
			}
			MenuSurvivorsClothingUI.updateFilterAndPage();
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x00165334 File Offset: 0x00163534
		private static void updateFilterAndPage()
		{
			MenuSurvivorsClothingUI.updateFilter();
			if (MenuSurvivorsClothingUI.pageIndex >= MenuSurvivorsClothingUI.numberOfPages)
			{
				MenuSurvivorsClothingUI.pageIndex = MenuSurvivorsClothingUI.numberOfPages - 1;
			}
			MenuSurvivorsClothingUI.updatePage();
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x00165358 File Offset: 0x00163558
		public static void viewPage(int newPage)
		{
			MenuSurvivorsClothingUI.pageIndex = newPage;
			MenuSurvivorsClothingUI.updatePage();
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x00165368 File Offset: 0x00163568
		private static void onClickedInventory(SleekInventory button)
		{
			int num = MenuSurvivorsClothingUI.packageButtons.Length * MenuSurvivorsClothingUI.pageIndex;
			int num2 = MenuSurvivorsClothingUI.inventory.FindIndexOfChild(button);
			if (num + num2 < MenuSurvivorsClothingUI.filteredItems.Count)
			{
				int item = button.item;
				ulong instance = button.instance;
				ushort quantity = button.quantity;
				if (MenuSurvivorsClothingUI.filterMode == EEconFilterMode.STAT_TRACKER || MenuSurvivorsClothingUI.filterMode == EEconFilterMode.STAT_TRACKER_REMOVAL || MenuSurvivorsClothingUI.filterMode == EEconFilterMode.RAGDOLL_EFFECT_REMOVAL || MenuSurvivorsClothingUI.filterMode == EEconFilterMode.RAGDOLL_EFFECT)
				{
					bool flag = MenuSurvivorsClothingUI.filterMode == EEconFilterMode.STAT_TRACKER || MenuSurvivorsClothingUI.filterMode == EEconFilterMode.RAGDOLL_EFFECT;
					MenuSurvivorsClothingDeleteUI.viewItem(item, instance, 1, flag ? EDeleteMode.TAG_TOOL_ADD : EDeleteMode.TAG_TOOL_REMOVE, MenuSurvivorsClothingUI.filterInstigator);
					MenuSurvivorsClothingDeleteUI.open();
					MenuSurvivorsClothingUI.setFilter(EEconFilterMode.SEARCH, 0UL);
					MenuSurvivorsClothingUI.close();
					return;
				}
				if (Provider.preferenceData.Allow_Ctrl_Shift_Alt_Salvage && InputEx.GetKey(KeyCode.LeftControl) && InputEx.GetKey(KeyCode.LeftShift) && InputEx.GetKey(KeyCode.LeftAlt))
				{
					MenuSurvivorsClothingDeleteUI.salvageItem(item, instance);
					return;
				}
				if (InputEx.GetKey(ControlsSettings.other) && MenuSurvivorsClothingUI.packageButtons[num2].itemAsset != null)
				{
					if (button.itemAsset.type == EItemType.BOX)
					{
						MenuSurvivorsClothingItemUI.viewItem(item, quantity, instance);
						MenuSurvivorsClothingBoxUI.viewItem(item, quantity, instance);
						MenuSurvivorsClothingBoxUI.open();
						MenuSurvivorsClothingUI.close();
						return;
					}
					Characters.ToggleEquipItemByInstanceId(instance);
					return;
				}
				else
				{
					MenuSurvivorsClothingItemUI.viewItem(item, quantity, instance);
					MenuSurvivorsClothingItemUI.open();
					MenuSurvivorsClothingUI.close();
				}
			}
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x001654A6 File Offset: 0x001636A6
		private static void onEnteredSearchField(ISleekField field)
		{
			MenuSurvivorsClothingUI.updateFilterAndPage();
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x001654AD File Offset: 0x001636AD
		private static void onClickedSearchButton(ISleekElement button)
		{
			MenuSurvivorsClothingUI.updateFilterAndPage();
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x001654B4 File Offset: 0x001636B4
		private static void onClickedCancelFilterButton(ISleekElement button)
		{
			MenuSurvivorsClothingUI.setFilter(EEconFilterMode.SEARCH, 0UL);
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x001654BE File Offset: 0x001636BE
		private static void onClickedLeftButton(ISleekElement button)
		{
			if (MenuSurvivorsClothingUI.pageIndex > 0)
			{
				MenuSurvivorsClothingUI.viewPage(MenuSurvivorsClothingUI.pageIndex - 1);
				return;
			}
			if (MenuSurvivorsClothingUI.numberOfPages > 0)
			{
				MenuSurvivorsClothingUI.viewPage(MenuSurvivorsClothingUI.numberOfPages - 1);
			}
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x001654E9 File Offset: 0x001636E9
		private static void onClickedRightButton(ISleekElement button)
		{
			if (MenuSurvivorsClothingUI.pageIndex < MenuSurvivorsClothingUI.numberOfPages - 1)
			{
				MenuSurvivorsClothingUI.viewPage(MenuSurvivorsClothingUI.pageIndex + 1);
				return;
			}
			if (MenuSurvivorsClothingUI.numberOfPages > 0)
			{
				MenuSurvivorsClothingUI.viewPage(0);
			}
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x00165514 File Offset: 0x00163714
		private static void onClickedOptionsButton(ISleekElement button)
		{
			MenuSurvivorsClothingUI.optionsPanel.IsVisible = !MenuSurvivorsClothingUI.optionsPanel.IsVisible;
			MenuSurvivorsClothingUI.optionsButton.icon = MenuSurvivorsClothingUI.icons.load<Texture2D>(MenuSurvivorsClothingUI.optionsPanel.IsVisible ? "Right" : "Left");
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x00165564 File Offset: 0x00163764
		private static void onToggledSearchDescriptions(ISleekToggle toggle, bool state)
		{
			MenuSurvivorsClothingUI.searchDescriptions = state;
			MenuSurvivorsClothingUI.updateFilterAndPage();
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x00165571 File Offset: 0x00163771
		private static void onChangedSortMode(SleekButtonState button, int state)
		{
			MenuSurvivorsClothingUI.sortMode = (MenuSurvivorsClothingUI.ESortMode)state;
			MenuSurvivorsClothingUI.updateFilterAndPage();
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x0016557E File Offset: 0x0016377E
		private static void onToggledReverseSortOrder(ISleekToggle toggle, bool state)
		{
			MenuSurvivorsClothingUI.reverseSortOrder = state;
			MenuSurvivorsClothingUI.updateFilterAndPage();
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x0016558B File Offset: 0x0016378B
		private static void onToggledFilterEquipped(ISleekToggle toggle, bool state)
		{
			MenuSurvivorsClothingUI.filterEquipped = state;
			MenuSurvivorsClothingUI.updateFilterAndPage();
		}

		// Token: 0x060041FE RID: 16894 RVA: 0x00165598 File Offset: 0x00163798
		private static void onClickedRefreshButton(ISleekElement button)
		{
			Provider.provider.economyService.refreshInventory();
		}

		// Token: 0x060041FF RID: 16895 RVA: 0x001655A9 File Offset: 0x001637A9
		private static void onClickedGrantPackagePromoButton(ISleekElement button)
		{
			button.IsVisible = false;
			GrantPackagePromo.SendRequest();
		}

		// Token: 0x06004200 RID: 16896 RVA: 0x001655B7 File Offset: 0x001637B7
		public static void prepareForCraftResult()
		{
			MenuSurvivorsClothingUI.isCrafting = true;
			MenuUI.openAlert(MenuSurvivorsClothingUI.localization.format("Alert_Crafting"), false);
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x001655D4 File Offset: 0x001637D4
		private void onClickedCraftButton(ISleekElement button)
		{
			if (MenuSurvivorsClothingUI.isCrafting)
			{
				return;
			}
			int num = MenuSurvivorsClothingUI.craftingScrollBox.FindIndexOfChild(button);
			if (num == -1)
			{
				return;
			}
			EconCraftOption econCraftOption = this.econCraftOptions[num];
			List<EconExchangePair> destroy;
			if (!Provider.provider.economyService.getInventoryPackages(19000, econCraftOption.scrapsNeeded, out destroy))
			{
				return;
			}
			MenuSurvivorsClothingUI.prepareForCraftResult();
			Provider.provider.economyService.exchangeInventory(econCraftOption.generate, destroy);
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x00165641 File Offset: 0x00163841
		private static void onInventoryRefreshed()
		{
			MenuSurvivorsClothingUI.infoBox.IsVisible = false;
			MenuSurvivorsClothingUI.updateFilter();
			if (MenuSurvivorsClothingUI.pageIndex >= MenuSurvivorsClothingUI.numberOfPages)
			{
				MenuSurvivorsClothingUI.pageIndex = MenuSurvivorsClothingUI.numberOfPages - 1;
			}
			MenuSurvivorsClothingUI.updatePage();
			MenuSurvivorsClothingUI.grantPackagePromoButton.IsVisible = GrantPackagePromo.IsEligible();
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x0016567F File Offset: 0x0016387F
		public static void onInventoryDropped(int item, ushort quantity, ulong instance)
		{
			MenuUI.closeAll();
			MenuUI.alert(MenuSurvivorsClothingUI.localization.format("Origin_Drop"), instance, item, quantity);
			MenuSurvivorsClothingItemUI.viewItem(item, quantity, instance);
			MenuSurvivorsClothingItemUI.open();
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x001656AA File Offset: 0x001638AA
		private static void onCharacterUpdated(byte index, Character character)
		{
			MenuSurvivorsClothingUI.updatePage();
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x001656B4 File Offset: 0x001638B4
		private static void OnPricesReceived()
		{
			if (ItemStore.Get().HasNewListings && !ItemStoreSavedata.WasNewListingsPageSeen())
			{
				MenuSurvivorsClothingUI.itemstoreNewLabel = Glazier.Get().CreateLabel();
				MenuSurvivorsClothingUI.itemstoreNewLabel.SizeScale_X = 1f;
				MenuSurvivorsClothingUI.itemstoreNewLabel.SizeScale_Y = 1f;
				MenuSurvivorsClothingUI.itemstoreNewLabel.TextContrastContext = 1;
				MenuSurvivorsClothingUI.itemstoreNewLabel.TextAlignment = 2;
				MenuSurvivorsClothingUI.itemstoreNewLabel.TextColor = Color.green;
				MenuSurvivorsClothingUI.itemstoreNewLabel.Text = Provider.localization.format("New");
				MenuSurvivorsClothingUI.itemstoreButton.AddChild(MenuSurvivorsClothingUI.itemstoreNewLabel);
				return;
			}
			if (ItemStore.Get().HasDiscountedListings)
			{
				ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
				sleekLabel.SizeScale_X = 1f;
				sleekLabel.SizeScale_Y = 1f;
				sleekLabel.TextContrastContext = 1;
				sleekLabel.TextAlignment = 2;
				sleekLabel.TextColor = Color.green;
				sleekLabel.Text = MenuSurvivorsClothingUI.localization.format("Itemstore_Sale");
				MenuSurvivorsClothingUI.itemstoreButton.AddChild(sleekLabel);
			}
		}

		/// <summary>
		/// Remove items that do not match search text.
		/// </summary>
		// Token: 0x06004206 RID: 16902 RVA: 0x001657C8 File Offset: 0x001639C8
		private static void applySearchTextFilter()
		{
			string text = MenuSurvivorsClothingUI.searchField.Text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			TokenSearchFilter? tokenSearchFilter = TokenSearchFilter.parse(text);
			if (tokenSearchFilter == null)
			{
				return;
			}
			for (int i = MenuSurvivorsClothingUI.filteredItems.Count - 1; i >= 0; i--)
			{
				SteamItemDetails_t steamItemDetails_t = MenuSurvivorsClothingUI.filteredItems[i];
				bool flag = false;
				string inventoryName = Provider.provider.economyService.getInventoryName(steamItemDetails_t.m_iDefinition.m_SteamItemDef);
				if (tokenSearchFilter.Value.matches(inventoryName))
				{
					flag = true;
				}
				else
				{
					string inventoryType = Provider.provider.economyService.getInventoryType(steamItemDetails_t.m_iDefinition.m_SteamItemDef);
					if (tokenSearchFilter.Value.matches(inventoryType))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					if (MenuSurvivorsClothingUI.searchDescriptions)
					{
						string inventoryDescription = Provider.provider.economyService.getInventoryDescription(steamItemDetails_t.m_iDefinition.m_SteamItemDef);
						if (tokenSearchFilter.Value.matches(inventoryDescription))
						{
							flag = true;
						}
					}
					if (!flag)
					{
						MenuSurvivorsClothingUI.filteredItems.RemoveAtFast(i);
					}
				}
			}
		}

		/// <summary>
		/// Removed items that are not equipped.
		/// </summary>
		// Token: 0x06004207 RID: 16903 RVA: 0x001658DC File Offset: 0x00163ADC
		private static void applyEquippedFilter()
		{
			if (!MenuSurvivorsClothingUI.filterEquipped)
			{
				return;
			}
			for (int i = MenuSurvivorsClothingUI.filteredItems.Count - 1; i >= 0; i--)
			{
				if (!Characters.isEquipped(MenuSurvivorsClothingUI.filteredItems[i].m_itemId.m_SteamItemInstanceID))
				{
					MenuSurvivorsClothingUI.filteredItems.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x00165930 File Offset: 0x00163B30
		private static void sortFilteredItems()
		{
			IComparer<SteamItemDetails_t> comparer;
			switch (MenuSurvivorsClothingUI.sortMode)
			{
			default:
				comparer = null;
				break;
			case MenuSurvivorsClothingUI.ESortMode.Rarity:
				comparer = new EconSortMode_Rarity();
				break;
			case MenuSurvivorsClothingUI.ESortMode.Name:
				comparer = new EconSortMode_Name();
				break;
			case MenuSurvivorsClothingUI.ESortMode.Type:
				comparer = new EconSortMode_Type();
				break;
			}
			if (comparer != null)
			{
				MenuSurvivorsClothingUI.filteredItems.Sort(comparer);
			}
			if (MenuSurvivorsClothingUI.reverseSortOrder)
			{
				MenuSurvivorsClothingUI.filteredItems.Reverse();
			}
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x00165994 File Offset: 0x00163B94
		private static void updateFilter()
		{
			if (MenuSurvivorsClothingUI.filterMode == EEconFilterMode.STAT_TRACKER)
			{
				MenuSurvivorsClothingUI.filteredItems = new List<SteamItemDetails_t>();
				using (List<SteamItemDetails_t>.Enumerator enumerator = Provider.provider.economyService.inventory.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SteamItemDetails_t steamItemDetails_t = enumerator.Current;
						Guid inventoryItemGuid = Provider.provider.economyService.getInventoryItemGuid(steamItemDetails_t.m_iDefinition.m_SteamItemDef);
						int inventorySkinID = (int)Provider.provider.economyService.getInventorySkinID(steamItemDetails_t.m_iDefinition.m_SteamItemDef);
						if (inventoryItemGuid != default(Guid) && inventorySkinID != 0)
						{
							MenuSurvivorsClothingUI.filteredItems.Add(steamItemDetails_t);
						}
					}
					goto IL_286;
				}
			}
			if (MenuSurvivorsClothingUI.filterMode == EEconFilterMode.STAT_TRACKER_REMOVAL)
			{
				MenuSurvivorsClothingUI.filteredItems = new List<SteamItemDetails_t>();
				using (List<SteamItemDetails_t>.Enumerator enumerator = Provider.provider.economyService.inventory.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SteamItemDetails_t steamItemDetails_t2 = enumerator.Current;
						EStatTrackerType estatTrackerType;
						int num;
						if (Provider.provider.economyService.getInventoryStatTrackerValue(steamItemDetails_t2.m_itemId.m_SteamItemInstanceID, out estatTrackerType, out num) && estatTrackerType != EStatTrackerType.NONE)
						{
							MenuSurvivorsClothingUI.filteredItems.Add(steamItemDetails_t2);
						}
					}
					goto IL_286;
				}
			}
			if (MenuSurvivorsClothingUI.filterMode == EEconFilterMode.RAGDOLL_EFFECT_REMOVAL)
			{
				MenuSurvivorsClothingUI.filteredItems = new List<SteamItemDetails_t>();
				using (List<SteamItemDetails_t>.Enumerator enumerator = Provider.provider.economyService.inventory.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SteamItemDetails_t steamItemDetails_t3 = enumerator.Current;
						ERagdollEffect eragdollEffect;
						if (Provider.provider.economyService.getInventoryRagdollEffect(steamItemDetails_t3.m_itemId.m_SteamItemInstanceID, out eragdollEffect) && eragdollEffect != ERagdollEffect.NONE)
						{
							MenuSurvivorsClothingUI.filteredItems.Add(steamItemDetails_t3);
						}
					}
					goto IL_286;
				}
			}
			if (MenuSurvivorsClothingUI.filterMode == EEconFilterMode.RAGDOLL_EFFECT)
			{
				MenuSurvivorsClothingUI.filteredItems = new List<SteamItemDetails_t>();
				using (List<SteamItemDetails_t>.Enumerator enumerator = Provider.provider.economyService.inventory.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SteamItemDetails_t steamItemDetails_t4 = enumerator.Current;
						Guid inventoryItemGuid2 = Provider.provider.economyService.getInventoryItemGuid(steamItemDetails_t4.m_iDefinition.m_SteamItemDef);
						int inventorySkinID2 = (int)Provider.provider.economyService.getInventorySkinID(steamItemDetails_t4.m_iDefinition.m_SteamItemDef);
						if (inventoryItemGuid2 != default(Guid) && inventorySkinID2 != 0)
						{
							ERagdollEffect eragdollEffect2;
							Provider.provider.economyService.getInventoryRagdollEffect(steamItemDetails_t4.m_itemId.m_SteamItemInstanceID, out eragdollEffect2);
							if (eragdollEffect2 == ERagdollEffect.NONE)
							{
								MenuSurvivorsClothingUI.filteredItems.Add(steamItemDetails_t4);
							}
						}
					}
					goto IL_286;
				}
			}
			MenuSurvivorsClothingUI.filteredItems = new List<SteamItemDetails_t>(Provider.provider.economyService.inventory);
			IL_286:
			MenuSurvivorsClothingUI.applySearchTextFilter();
			MenuSurvivorsClothingUI.applyEquippedFilter();
			MenuSurvivorsClothingUI.sortFilteredItems();
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x00165C6C File Offset: 0x00163E6C
		public static void updatePage()
		{
			MenuSurvivorsClothingUI.availableBox.Text = ItemTool.filterRarityRichText(MenuSurvivorsClothingUI.localization.format("Craft_Available", Provider.provider.economyService.countInventoryPackages(19000)));
			MenuSurvivorsClothingUI.pageBox.Text = MenuSurvivorsClothingUI.localization.format("Page", MenuSurvivorsClothingUI.pageIndex + 1, MenuSurvivorsClothingUI.numberOfPages);
			if (MenuSurvivorsClothingUI.packageButtons == null)
			{
				return;
			}
			int num = MenuSurvivorsClothingUI.packageButtons.Length * MenuSurvivorsClothingUI.pageIndex;
			for (int i = 0; i < MenuSurvivorsClothingUI.packageButtons.Length; i++)
			{
				if (num + i < MenuSurvivorsClothingUI.filteredItems.Count)
				{
					MenuSurvivorsClothingUI.packageButtons[i].updateInventory(MenuSurvivorsClothingUI.filteredItems[num + i].m_itemId.m_SteamItemInstanceID, MenuSurvivorsClothingUI.filteredItems[num + i].m_iDefinition.m_SteamItemDef, MenuSurvivorsClothingUI.filteredItems[num + i].m_unQuantity, true, false);
				}
				else
				{
					MenuSurvivorsClothingUI.packageButtons[i].updateInventory(0UL, 0, 0, false, false);
				}
			}
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x00165D78 File Offset: 0x00163F78
		private static void onDraggedCharacterSlider(ISleekSlider slider, float state)
		{
			Characters.characterYaw = state * 360f;
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x00165D86 File Offset: 0x00163F86
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuSurvivorsUI.open();
			MenuSurvivorsClothingUI.close();
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x00165D92 File Offset: 0x00163F92
		private static void onClickedItemstoreButton(ISleekElement button)
		{
			if (MenuSurvivorsClothingUI.itemstoreNewLabel != null)
			{
				MenuSurvivorsClothingUI.itemstoreButton.RemoveChild(MenuSurvivorsClothingUI.itemstoreNewLabel);
				MenuSurvivorsClothingUI.itemstoreNewLabel = null;
				ItemStoreSavedata.MarkNewListingsPageSeen();
				ItemStore.Get().ViewNewItems();
				return;
			}
			ItemStore.Get().ViewStore();
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x00165DCC File Offset: 0x00163FCC
		private static void setCrafting(bool isCrafting)
		{
			MenuSurvivorsClothingUI.inventory.IsVisible = !isCrafting;
			MenuSurvivorsClothingUI.crafting.IsVisible = isCrafting;
			MenuSurvivorsClothingUI.craftingButton.icon = (MenuSurvivorsClothingUI.inventory.IsVisible ? MenuSurvivorsClothingUI.icons.load<Texture2D>("Crafting") : MenuSurvivorsClothingUI.icons.load<Texture2D>("Backpack"));
			MenuSurvivorsClothingUI.craftingButton.text = MenuSurvivorsClothingUI.localization.format(MenuSurvivorsClothingUI.inventory.IsVisible ? "Crafting" : "Backpack");
			MenuSurvivorsClothingUI.craftingButton.tooltip = MenuSurvivorsClothingUI.localization.format(MenuSurvivorsClothingUI.inventory.IsVisible ? "Crafting_Tooltip" : "Backpack_Tooltip");
		}

		// Token: 0x0600420F RID: 16911 RVA: 0x00165E80 File Offset: 0x00164080
		private static void onClickedCraftingButton(ISleekElement button)
		{
			if (MenuSurvivorsClothingUI.craftingNewLabel != null)
			{
				MenuSurvivorsClothingUI.itemstoreButton.RemoveChild(MenuSurvivorsClothingUI.craftingNewLabel);
				MenuSurvivorsClothingUI.craftingNewLabel = null;
				ItemStoreSavedata.MarkNewCraftingPageSeen();
			}
			MenuSurvivorsClothingUI.setCrafting(!MenuSurvivorsClothingUI.crafting.IsVisible);
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x00165EB8 File Offset: 0x001640B8
		private static void onInventoryExchanged(List<SteamItemDetails_t> grantedItems)
		{
			if (!MenuSurvivorsClothingUI.isCrafting)
			{
				return;
			}
			MenuSurvivorsClothingUI.isCrafting = false;
			MenuUI.closeAlert();
			for (int i = grantedItems.Count - 1; i >= 0; i--)
			{
				if (grantedItems[i].m_iDefinition.m_SteamItemDef == 19000)
				{
					grantedItems.RemoveAtFast(i);
				}
			}
			MenuUI.alertNewItems(MenuSurvivorsClothingUI.localization.format("Origin_Craft"), grantedItems);
			SteamItemDetails_t steamItemDetails_t = grantedItems[0];
			MenuSurvivorsClothingItemUI.viewItem(steamItemDetails_t.m_iDefinition.m_SteamItemDef, steamItemDetails_t.m_unQuantity, steamItemDetails_t.m_itemId.m_SteamItemInstanceID);
			MenuSurvivorsClothingItemUI.open();
			MenuSurvivorsClothingUI.close();
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x00165F52 File Offset: 0x00164152
		private static void onInventoryPurchased(List<SteamItemDetails_t> grantedItems)
		{
			MenuUI.closeAlert();
			MenuUI.alertPurchasedItems(MenuSurvivorsClothingUI.localization.format("Origin_Purchase"), grantedItems);
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x00165F6E File Offset: 0x0016416E
		private static void onInventoryExchangeFailed()
		{
			if (!MenuSurvivorsClothingUI.isCrafting)
			{
				return;
			}
			UnturnedLog.info("Crafting failed");
			MenuSurvivorsClothingUI.isCrafting = false;
			MenuUI.closeAlert();
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x00165F90 File Offset: 0x00164190
		public void OnDestroy()
		{
			this.boxUI.OnDestroy();
			TempSteamworksEconomy economyService = Provider.provider.economyService;
			economyService.onInventoryExchanged = (TempSteamworksEconomy.InventoryExchanged)Delegate.Remove(economyService.onInventoryExchanged, new TempSteamworksEconomy.InventoryExchanged(MenuSurvivorsClothingUI.onInventoryExchanged));
			TempSteamworksEconomy economyService2 = Provider.provider.economyService;
			economyService2.onInventoryPurchased = (TempSteamworksEconomy.InventoryExchanged)Delegate.Remove(economyService2.onInventoryPurchased, new TempSteamworksEconomy.InventoryExchanged(MenuSurvivorsClothingUI.onInventoryPurchased));
			TempSteamworksEconomy economyService3 = Provider.provider.economyService;
			economyService3.onInventoryExchangeFailed = (TempSteamworksEconomy.InventoryExchangeFailed)Delegate.Remove(economyService3.onInventoryExchangeFailed, new TempSteamworksEconomy.InventoryExchangeFailed(MenuSurvivorsClothingUI.onInventoryExchangeFailed));
			TempSteamworksEconomy economyService4 = Provider.provider.economyService;
			economyService4.onInventoryRefreshed = (TempSteamworksEconomy.InventoryRefreshed)Delegate.Remove(economyService4.onInventoryRefreshed, new TempSteamworksEconomy.InventoryRefreshed(MenuSurvivorsClothingUI.onInventoryRefreshed));
			TempSteamworksEconomy economyService5 = Provider.provider.economyService;
			economyService5.onInventoryDropped = (TempSteamworksEconomy.InventoryDropped)Delegate.Remove(economyService5.onInventoryDropped, new TempSteamworksEconomy.InventoryDropped(MenuSurvivorsClothingUI.onInventoryDropped));
			Characters.onCharacterUpdated = (CharacterUpdated)Delegate.Remove(Characters.onCharacterUpdated, new CharacterUpdated(MenuSurvivorsClothingUI.onCharacterUpdated));
			ItemStore.Get().OnPricesReceived -= new Action(MenuSurvivorsClothingUI.OnPricesReceived);
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x001660B8 File Offset: 0x001642B8
		public MenuSurvivorsClothingUI()
		{
			MenuSurvivorsClothingUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsClothing.dat");
			if (MenuSurvivorsClothingUI.icons != null)
			{
				MenuSurvivorsClothingUI.icons.unload();
				MenuSurvivorsClothingUI.icons = null;
			}
			MenuSurvivorsClothingUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Survivors/MenuSurvivorsClothing/MenuSurvivorsClothing.unity3d");
			MenuSurvivorsClothingUI.container = new SleekFullscreenBox();
			MenuSurvivorsClothingUI.container.PositionOffset_X = 10f;
			MenuSurvivorsClothingUI.container.PositionOffset_Y = 10f;
			MenuSurvivorsClothingUI.container.PositionScale_Y = 1f;
			MenuSurvivorsClothingUI.container.SizeOffset_X = -20f;
			MenuSurvivorsClothingUI.container.SizeOffset_Y = -20f;
			MenuSurvivorsClothingUI.container.SizeScale_X = 1f;
			MenuSurvivorsClothingUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuSurvivorsClothingUI.container);
			MenuSurvivorsClothingUI.active = false;
			MenuSurvivorsClothingUI.pageIndex = 0;
			MenuSurvivorsClothingUI.filterMode = EEconFilterMode.SEARCH;
			MenuSurvivorsClothingUI.inventory = Glazier.Get().CreateConstraintFrame();
			MenuSurvivorsClothingUI.inventory.PositionOffset_Y = 80f;
			MenuSurvivorsClothingUI.inventory.PositionScale_X = 0.5f;
			MenuSurvivorsClothingUI.inventory.SizeScale_X = 0.5f;
			MenuSurvivorsClothingUI.inventory.SizeScale_Y = 1f;
			MenuSurvivorsClothingUI.inventory.SizeOffset_Y = -120f;
			MenuSurvivorsClothingUI.inventory.Constraint = 1;
			MenuSurvivorsClothingUI.container.AddChild(MenuSurvivorsClothingUI.inventory);
			MenuSurvivorsClothingUI.crafting = Glazier.Get().CreateConstraintFrame();
			MenuSurvivorsClothingUI.crafting.PositionOffset_Y = 40f;
			MenuSurvivorsClothingUI.crafting.PositionScale_X = 0.5f;
			MenuSurvivorsClothingUI.crafting.SizeScale_X = 0.5f;
			MenuSurvivorsClothingUI.crafting.SizeScale_Y = 1f;
			MenuSurvivorsClothingUI.crafting.SizeOffset_Y = -80f;
			MenuSurvivorsClothingUI.crafting.Constraint = 1;
			MenuSurvivorsClothingUI.container.AddChild(MenuSurvivorsClothingUI.crafting);
			MenuSurvivorsClothingUI.crafting.IsVisible = false;
			MenuSurvivorsClothingUI.packageButtons = new SleekInventory[25];
			for (int i = 0; i < MenuSurvivorsClothingUI.packageButtons.Length; i++)
			{
				SleekInventory sleekInventory = new SleekInventory();
				sleekInventory.PositionOffset_X = 5f;
				sleekInventory.PositionOffset_Y = 5f;
				sleekInventory.PositionScale_X = (float)(i % 5) * 0.2f;
				sleekInventory.PositionScale_Y = (float)Mathf.FloorToInt((float)i / 5f) * 0.2f;
				sleekInventory.SizeOffset_X = -10f;
				sleekInventory.SizeOffset_Y = -10f;
				sleekInventory.SizeScale_X = 0.2f;
				sleekInventory.SizeScale_Y = 0.2f;
				sleekInventory.onClickedInventory = new ClickedInventory(MenuSurvivorsClothingUI.onClickedInventory);
				MenuSurvivorsClothingUI.inventory.AddChild(sleekInventory);
				MenuSurvivorsClothingUI.packageButtons[i] = sleekInventory;
			}
			MenuSurvivorsClothingUI.searchField = Glazier.Get().CreateStringField();
			MenuSurvivorsClothingUI.searchField.PositionOffset_X = 45f;
			MenuSurvivorsClothingUI.searchField.PositionOffset_Y = -35f;
			MenuSurvivorsClothingUI.searchField.SizeOffset_X = -160f;
			MenuSurvivorsClothingUI.searchField.SizeOffset_Y = 30f;
			MenuSurvivorsClothingUI.searchField.SizeScale_X = 1f;
			MenuSurvivorsClothingUI.searchField.PlaceholderText = MenuSurvivorsClothingUI.localization.format("Search_Field_Hint");
			MenuSurvivorsClothingUI.searchField.OnTextSubmitted += new Entered(MenuSurvivorsClothingUI.onEnteredSearchField);
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.searchField);
			MenuSurvivorsClothingUI.searchButton = Glazier.Get().CreateButton();
			MenuSurvivorsClothingUI.searchButton.PositionOffset_X = -105f;
			MenuSurvivorsClothingUI.searchButton.PositionOffset_Y = -35f;
			MenuSurvivorsClothingUI.searchButton.PositionScale_X = 1f;
			MenuSurvivorsClothingUI.searchButton.SizeOffset_X = 100f;
			MenuSurvivorsClothingUI.searchButton.SizeOffset_Y = 30f;
			MenuSurvivorsClothingUI.searchButton.Text = MenuSurvivorsClothingUI.localization.format("Search");
			MenuSurvivorsClothingUI.searchButton.TooltipText = MenuSurvivorsClothingUI.localization.format("Search_Tooltip");
			MenuSurvivorsClothingUI.searchButton.OnClicked += new ClickedButton(MenuSurvivorsClothingUI.onClickedSearchButton);
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.searchButton);
			MenuSurvivorsClothingUI.filterBox = Glazier.Get().CreateBox();
			MenuSurvivorsClothingUI.filterBox.PositionOffset_X = 5f;
			MenuSurvivorsClothingUI.filterBox.PositionOffset_Y = -75f;
			MenuSurvivorsClothingUI.filterBox.SizeOffset_X = -120f;
			MenuSurvivorsClothingUI.filterBox.SizeOffset_Y = 30f;
			MenuSurvivorsClothingUI.filterBox.SizeScale_X = 1f;
			MenuSurvivorsClothingUI.filterBox.AllowRichText = true;
			MenuSurvivorsClothingUI.filterBox.TextColor = 4;
			MenuSurvivorsClothingUI.filterBox.TextContrastContext = 1;
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.filterBox);
			MenuSurvivorsClothingUI.filterBox.IsVisible = false;
			MenuSurvivorsClothingUI.cancelFilterButton = Glazier.Get().CreateButton();
			MenuSurvivorsClothingUI.cancelFilterButton.PositionOffset_X = -105f;
			MenuSurvivorsClothingUI.cancelFilterButton.PositionOffset_Y = -75f;
			MenuSurvivorsClothingUI.cancelFilterButton.PositionScale_X = 1f;
			MenuSurvivorsClothingUI.cancelFilterButton.SizeOffset_X = 100f;
			MenuSurvivorsClothingUI.cancelFilterButton.SizeOffset_Y = 30f;
			MenuSurvivorsClothingUI.cancelFilterButton.Text = MenuSurvivorsClothingUI.localization.format("Cancel_Filter");
			MenuSurvivorsClothingUI.cancelFilterButton.TooltipText = MenuSurvivorsClothingUI.localization.format("Cancel_Filter_Tooltip");
			MenuSurvivorsClothingUI.cancelFilterButton.OnClicked += new ClickedButton(MenuSurvivorsClothingUI.onClickedCancelFilterButton);
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.cancelFilterButton);
			MenuSurvivorsClothingUI.cancelFilterButton.IsVisible = false;
			MenuSurvivorsClothingUI.pageBox = Glazier.Get().CreateBox();
			MenuSurvivorsClothingUI.pageBox.PositionOffset_X = -145f;
			MenuSurvivorsClothingUI.pageBox.PositionOffset_Y = 5f;
			MenuSurvivorsClothingUI.pageBox.PositionScale_X = 1f;
			MenuSurvivorsClothingUI.pageBox.PositionScale_Y = 1f;
			MenuSurvivorsClothingUI.pageBox.SizeOffset_X = 100f;
			MenuSurvivorsClothingUI.pageBox.SizeOffset_Y = 30f;
			MenuSurvivorsClothingUI.pageBox.FontSize = 3;
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.pageBox);
			MenuSurvivorsClothingUI.infoBox = Glazier.Get().CreateBox();
			MenuSurvivorsClothingUI.infoBox.PositionOffset_X = 5f;
			MenuSurvivorsClothingUI.infoBox.PositionOffset_Y = -25f;
			MenuSurvivorsClothingUI.infoBox.PositionScale_Y = 0.5f;
			MenuSurvivorsClothingUI.infoBox.SizeScale_X = 1f;
			MenuSurvivorsClothingUI.infoBox.SizeOffset_X = -10f;
			MenuSurvivorsClothingUI.infoBox.SizeOffset_Y = 50f;
			MenuSurvivorsClothingUI.infoBox.Text = MenuSurvivorsClothingUI.localization.format("No_Items");
			MenuSurvivorsClothingUI.infoBox.FontSize = 3;
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.infoBox);
			MenuSurvivorsClothingUI.infoBox.IsVisible = !Provider.provider.economyService.isInventoryAvailable;
			MenuSurvivorsClothingUI.leftButton = new SleekButtonIcon(MenuSurvivorsClothingUI.icons.load<Texture2D>("Left"));
			MenuSurvivorsClothingUI.leftButton.PositionOffset_X = -185f;
			MenuSurvivorsClothingUI.leftButton.PositionOffset_Y = 5f;
			MenuSurvivorsClothingUI.leftButton.PositionScale_X = 1f;
			MenuSurvivorsClothingUI.leftButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingUI.leftButton.SizeOffset_X = 30f;
			MenuSurvivorsClothingUI.leftButton.SizeOffset_Y = 30f;
			MenuSurvivorsClothingUI.leftButton.tooltip = MenuSurvivorsClothingUI.localization.format("Left_Tooltip");
			MenuSurvivorsClothingUI.leftButton.iconColor = 2;
			MenuSurvivorsClothingUI.leftButton.onClickedButton += new ClickedButton(MenuSurvivorsClothingUI.onClickedLeftButton);
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.leftButton);
			MenuSurvivorsClothingUI.rightButton = new SleekButtonIcon(MenuSurvivorsClothingUI.icons.load<Texture2D>("Right"));
			MenuSurvivorsClothingUI.rightButton.PositionOffset_X = -35f;
			MenuSurvivorsClothingUI.rightButton.PositionOffset_Y = 5f;
			MenuSurvivorsClothingUI.rightButton.PositionScale_X = 1f;
			MenuSurvivorsClothingUI.rightButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingUI.rightButton.SizeOffset_X = 30f;
			MenuSurvivorsClothingUI.rightButton.SizeOffset_Y = 30f;
			MenuSurvivorsClothingUI.rightButton.tooltip = MenuSurvivorsClothingUI.localization.format("Right_Tooltip");
			MenuSurvivorsClothingUI.rightButton.iconColor = 2;
			MenuSurvivorsClothingUI.rightButton.onClickedButton += new ClickedButton(MenuSurvivorsClothingUI.onClickedRightButton);
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.rightButton);
			MenuSurvivorsClothingUI.optionsButton = new SleekButtonIcon(MenuSurvivorsClothingUI.icons.load<Texture2D>("Left"));
			MenuSurvivorsClothingUI.optionsButton.PositionOffset_X = 5f;
			MenuSurvivorsClothingUI.optionsButton.PositionOffset_Y = -35f;
			MenuSurvivorsClothingUI.optionsButton.SizeOffset_X = 30f;
			MenuSurvivorsClothingUI.optionsButton.SizeOffset_Y = 30f;
			MenuSurvivorsClothingUI.optionsButton.tooltip = MenuSurvivorsClothingUI.localization.format("Advanced_Options_Tooltip");
			MenuSurvivorsClothingUI.optionsButton.iconColor = 2;
			MenuSurvivorsClothingUI.optionsButton.onClickedButton += new ClickedButton(MenuSurvivorsClothingUI.onClickedOptionsButton);
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.optionsButton);
			MenuSurvivorsClothingUI.optionsPanel = Glazier.Get().CreateFrame();
			MenuSurvivorsClothingUI.optionsPanel.PositionOffset_X = -200f;
			MenuSurvivorsClothingUI.optionsPanel.PositionOffset_Y = -35f;
			MenuSurvivorsClothingUI.optionsPanel.SizeOffset_X = 200f;
			MenuSurvivorsClothingUI.optionsPanel.SizeOffset_Y = 400f;
			MenuSurvivorsClothingUI.optionsPanel.IsVisible = false;
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.optionsPanel);
			MenuSurvivorsClothingUI.searchDescriptionsToggle = Glazier.Get().CreateToggle();
			MenuSurvivorsClothingUI.searchDescriptionsToggle.SizeOffset_X = 40f;
			MenuSurvivorsClothingUI.searchDescriptionsToggle.SizeOffset_Y = 40f;
			MenuSurvivorsClothingUI.searchDescriptionsToggle.AddLabel(MenuSurvivorsClothingUI.localization.format("Search_Descriptions_Label"), 1);
			MenuSurvivorsClothingUI.searchDescriptionsToggle.Value = MenuSurvivorsClothingUI.searchDescriptions;
			MenuSurvivorsClothingUI.searchDescriptionsToggle.OnValueChanged += new Toggled(MenuSurvivorsClothingUI.onToggledSearchDescriptions);
			MenuSurvivorsClothingUI.optionsPanel.AddChild(MenuSurvivorsClothingUI.searchDescriptionsToggle);
			MenuSurvivorsClothingUI.sortModeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuSurvivorsClothingUI.localization.format("Sort_Mode_Date")),
				new GUIContent(MenuSurvivorsClothingUI.localization.format("Sort_Mode_Rarity")),
				new GUIContent(MenuSurvivorsClothingUI.localization.format("Sort_Mode_Name"))
			});
			MenuSurvivorsClothingUI.sortModeButton.PositionOffset_Y = 50f;
			MenuSurvivorsClothingUI.sortModeButton.SizeOffset_X = 100f;
			MenuSurvivorsClothingUI.sortModeButton.SizeOffset_Y = 30f;
			MenuSurvivorsClothingUI.sortModeButton.AddLabel(MenuSurvivorsClothingUI.localization.format("Sort_Mode_Label"), 1);
			MenuSurvivorsClothingUI.sortModeButton.tooltip = MenuSurvivorsClothingUI.localization.format("Sort_Mode_Tooltip");
			MenuSurvivorsClothingUI.sortModeButton.state = (int)MenuSurvivorsClothingUI.sortMode;
			MenuSurvivorsClothingUI.sortModeButton.onSwappedState = new SwappedState(MenuSurvivorsClothingUI.onChangedSortMode);
			MenuSurvivorsClothingUI.optionsPanel.AddChild(MenuSurvivorsClothingUI.sortModeButton);
			MenuSurvivorsClothingUI.reverseSortOrderToggle = Glazier.Get().CreateToggle();
			MenuSurvivorsClothingUI.reverseSortOrderToggle.PositionOffset_Y = 90f;
			MenuSurvivorsClothingUI.reverseSortOrderToggle.SizeOffset_X = 40f;
			MenuSurvivorsClothingUI.reverseSortOrderToggle.SizeOffset_Y = 40f;
			MenuSurvivorsClothingUI.reverseSortOrderToggle.AddLabel(MenuSurvivorsClothingUI.localization.format("Reverse_Sort_Order_Label"), 1);
			MenuSurvivorsClothingUI.reverseSortOrderToggle.Value = MenuSurvivorsClothingUI.reverseSortOrder;
			MenuSurvivorsClothingUI.reverseSortOrderToggle.OnValueChanged += new Toggled(MenuSurvivorsClothingUI.onToggledReverseSortOrder);
			MenuSurvivorsClothingUI.optionsPanel.AddChild(MenuSurvivorsClothingUI.reverseSortOrderToggle);
			MenuSurvivorsClothingUI.filterEquippedToggle = Glazier.Get().CreateToggle();
			MenuSurvivorsClothingUI.filterEquippedToggle.PositionOffset_Y = 140f;
			MenuSurvivorsClothingUI.filterEquippedToggle.SizeOffset_X = 40f;
			MenuSurvivorsClothingUI.filterEquippedToggle.SizeOffset_Y = 40f;
			MenuSurvivorsClothingUI.filterEquippedToggle.AddLabel(MenuSurvivorsClothingUI.localization.format("Filter_Equipped_Label"), 1);
			MenuSurvivorsClothingUI.filterEquippedToggle.Value = MenuSurvivorsClothingUI.filterEquipped;
			MenuSurvivorsClothingUI.filterEquippedToggle.OnValueChanged += new Toggled(MenuSurvivorsClothingUI.onToggledFilterEquipped);
			MenuSurvivorsClothingUI.optionsPanel.AddChild(MenuSurvivorsClothingUI.filterEquippedToggle);
			MenuSurvivorsClothingUI.refreshButton = new SleekButtonIcon(MenuSurvivorsClothingUI.icons.load<Texture2D>("Refresh"));
			MenuSurvivorsClothingUI.refreshButton.PositionOffset_X = 5f;
			MenuSurvivorsClothingUI.refreshButton.PositionOffset_Y = 5f;
			MenuSurvivorsClothingUI.refreshButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingUI.refreshButton.SizeOffset_X = 30f;
			MenuSurvivorsClothingUI.refreshButton.SizeOffset_Y = 30f;
			MenuSurvivorsClothingUI.refreshButton.tooltip = MenuSurvivorsClothingUI.localization.format("Refresh_Tooltip");
			MenuSurvivorsClothingUI.refreshButton.iconColor = 2;
			MenuSurvivorsClothingUI.refreshButton.onClickedButton += new ClickedButton(MenuSurvivorsClothingUI.onClickedRefreshButton);
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.refreshButton);
			MenuSurvivorsClothingUI.grantPackagePromoButton = Glazier.Get().CreateButton();
			MenuSurvivorsClothingUI.grantPackagePromoButton.PositionOffset_Y = -280f;
			MenuSurvivorsClothingUI.grantPackagePromoButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingUI.grantPackagePromoButton.SizeOffset_X = 200f;
			MenuSurvivorsClothingUI.grantPackagePromoButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingUI.grantPackagePromoButton.Text = "Claim Unturned II Access";
			MenuSurvivorsClothingUI.grantPackagePromoButton.OnClicked += new ClickedButton(MenuSurvivorsClothingUI.onClickedGrantPackagePromoButton);
			MenuSurvivorsClothingUI.grantPackagePromoButton.IsVisible = false;
			MenuSurvivorsClothingUI.container.AddChild(MenuSurvivorsClothingUI.grantPackagePromoButton);
			MenuSurvivorsClothingUI.characterSlider = Glazier.Get().CreateSlider();
			MenuSurvivorsClothingUI.characterSlider.PositionOffset_X = 45f;
			MenuSurvivorsClothingUI.characterSlider.PositionOffset_Y = 10f;
			MenuSurvivorsClothingUI.characterSlider.PositionScale_Y = 1f;
			MenuSurvivorsClothingUI.characterSlider.SizeOffset_X = -240f;
			MenuSurvivorsClothingUI.characterSlider.SizeOffset_Y = 20f;
			MenuSurvivorsClothingUI.characterSlider.SizeScale_X = 1f;
			MenuSurvivorsClothingUI.characterSlider.Orientation = 0;
			MenuSurvivorsClothingUI.characterSlider.OnValueChanged += new Dragged(MenuSurvivorsClothingUI.onDraggedCharacterSlider);
			MenuSurvivorsClothingUI.inventory.AddChild(MenuSurvivorsClothingUI.characterSlider);
			MenuSurvivorsClothingUI.availableBox = Glazier.Get().CreateBox();
			MenuSurvivorsClothingUI.availableBox.SizeScale_X = 1f;
			MenuSurvivorsClothingUI.availableBox.SizeOffset_Y = 30f;
			MenuSurvivorsClothingUI.availableBox.AllowRichText = true;
			MenuSurvivorsClothingUI.availableBox.TextColor = 4;
			MenuSurvivorsClothingUI.availableBox.TextContrastContext = 1;
			MenuSurvivorsClothingUI.crafting.AddChild(MenuSurvivorsClothingUI.availableBox);
			MenuSurvivorsClothingUI.craftingScrollBox = Glazier.Get().CreateScrollView();
			MenuSurvivorsClothingUI.craftingScrollBox.PositionOffset_Y = 40f;
			MenuSurvivorsClothingUI.craftingScrollBox.SizeScale_X = 1f;
			MenuSurvivorsClothingUI.craftingScrollBox.SizeScale_Y = 1f;
			MenuSurvivorsClothingUI.craftingScrollBox.SizeOffset_Y = -40f;
			MenuSurvivorsClothingUI.crafting.AddChild(MenuSurvivorsClothingUI.craftingScrollBox);
			List<EconCraftOption> list = new List<EconCraftOption>();
			list.Add(new EconCraftOption("Craft_Common_Cosmetic", 10003, 2));
			list.Add(new EconCraftOption("Craft_Common_Skin", 10006, 2));
			list.Add(new EconCraftOption("Craft_Uncommon_Cosmetic", 10004, 13));
			list.Add(new EconCraftOption("Craft_Uncommon_Skin", 10007, 13));
			list.Add(new EconCraftOption("Craft_Stat_Tracker_Total_Kills", 19001, 30));
			list.Add(new EconCraftOption("Craft_Stat_Tracker_Player_Kills", 19002, 30));
			list.Add(new EconCraftOption("Craft_Ragdoll_Effect_Zero_Kelvin", 19003, 50));
			list.Add(new EconCraftOption("Craft_Ragdoll_Effect_Jaded", 19013, 50));
			list.Add(new EconCraftOption("Craft_Mythical_Skin", 19043, 1000));
			list.Add(new EconCraftOption("Craft_Stat_Tracker_Removal_Tool", 19004, 15));
			list.Add(new EconCraftOption("Craft_Ragdoll_Effect_Removal_Tool", 19005, 15));
			this.econCraftOptions = list;
			if (HolidayUtil.getActiveHoliday() == ENPCHoliday.PRIDE_MONTH)
			{
				this.econCraftOptions.Add(new EconCraftOption("Craft_ProgressPridePin", 1333, 5));
				this.econCraftOptions.Add(new EconCraftOption("Craft_ProgressPrideJersey", 1334, 5));
			}
			MenuSurvivorsClothingUI.craftingButtons = new ISleekButton[this.econCraftOptions.Count];
			for (int j = 0; j < MenuSurvivorsClothingUI.craftingButtons.Length; j++)
			{
				EconCraftOption econCraftOption = this.econCraftOptions[j];
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_Y = (float)(j * 30);
				sleekButton.SizeScale_X = 1f;
				sleekButton.SizeOffset_Y = 30f;
				sleekButton.AllowRichText = true;
				sleekButton.TextColor = 4;
				sleekButton.TextContrastContext = 1;
				sleekButton.Text = ItemTool.filterRarityRichText(MenuSurvivorsClothingUI.localization.format("Craft_Entry", MenuSurvivorsClothingUI.localization.format(econCraftOption.token), econCraftOption.scrapsNeeded));
				sleekButton.OnClicked += new ClickedButton(this.onClickedCraftButton);
				MenuSurvivorsClothingUI.craftingScrollBox.AddChild(sleekButton);
				MenuSurvivorsClothingUI.craftingButtons[j] = sleekButton;
			}
			MenuSurvivorsClothingUI.craftingScrollBox.ScaleContentToWidth = true;
			MenuSurvivorsClothingUI.craftingScrollBox.ContentSizeOffset = new Vector2(0f, (float)(this.econCraftOptions.Count * 30));
			MenuSurvivorsClothingUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuSurvivorsClothingUI.backButton.PositionOffset_Y = -50f;
			MenuSurvivorsClothingUI.backButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingUI.backButton.SizeOffset_X = 200f;
			MenuSurvivorsClothingUI.backButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsClothingUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuSurvivorsClothingUI.backButton.onClickedButton += new ClickedButton(MenuSurvivorsClothingUI.onClickedBackButton);
			MenuSurvivorsClothingUI.backButton.fontSize = 3;
			MenuSurvivorsClothingUI.backButton.iconColor = 2;
			MenuSurvivorsClothingUI.container.AddChild(MenuSurvivorsClothingUI.backButton);
			MenuSurvivorsClothingUI.itemstoreButton = Glazier.Get().CreateButton();
			MenuSurvivorsClothingUI.itemstoreButton.PositionOffset_Y = -170f;
			MenuSurvivorsClothingUI.itemstoreButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingUI.itemstoreButton.SizeOffset_X = 200f;
			MenuSurvivorsClothingUI.itemstoreButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingUI.itemstoreButton.Text = MenuSurvivorsClothingUI.localization.format("Itemstore");
			MenuSurvivorsClothingUI.itemstoreButton.TooltipText = MenuSurvivorsClothingUI.localization.format("Itemstore_Tooltip");
			MenuSurvivorsClothingUI.itemstoreButton.OnClicked += new ClickedButton(MenuSurvivorsClothingUI.onClickedItemstoreButton);
			MenuSurvivorsClothingUI.itemstoreButton.FontSize = 3;
			MenuSurvivorsClothingUI.container.AddChild(MenuSurvivorsClothingUI.itemstoreButton);
			if (!Provider.provider.economyService.doesCountryAllowRandomItems && Provider.provider.economyService.hasCountryDetails)
			{
				ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
				sleekLabel.PositionOffset_X = 210f;
				sleekLabel.PositionOffset_Y = -170f;
				sleekLabel.PositionScale_Y = 1f;
				sleekLabel.SizeOffset_X = 200f;
				sleekLabel.SizeOffset_Y = 50f;
				sleekLabel.TextAlignment = 3;
				sleekLabel.Text = MenuSurvivorsClothingUI.localization.format("Itemstore_Region_Box_Disabled", Provider.provider.economyService.getCountryWarningId());
				MenuSurvivorsClothingUI.container.AddChild(sleekLabel);
			}
			MenuSurvivorsClothingUI.craftingButton = new SleekButtonIcon(MenuSurvivorsClothingUI.icons.load<Texture2D>("Crafting"));
			MenuSurvivorsClothingUI.craftingButton.PositionOffset_Y = -110f;
			MenuSurvivorsClothingUI.craftingButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingUI.craftingButton.SizeOffset_X = 200f;
			MenuSurvivorsClothingUI.craftingButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingUI.craftingButton.text = MenuSurvivorsClothingUI.localization.format("Crafting");
			MenuSurvivorsClothingUI.craftingButton.tooltip = MenuSurvivorsClothingUI.localization.format("Crafting_Tooltip");
			MenuSurvivorsClothingUI.craftingButton.onClickedButton += new ClickedButton(MenuSurvivorsClothingUI.onClickedCraftingButton);
			MenuSurvivorsClothingUI.craftingButton.fontSize = 3;
			MenuSurvivorsClothingUI.craftingButton.iconColor = 2;
			MenuSurvivorsClothingUI.container.AddChild(MenuSurvivorsClothingUI.craftingButton);
			if (!ItemStoreSavedata.WasNewCraftingPageSeen())
			{
				MenuSurvivorsClothingUI.craftingNewLabel = Glazier.Get().CreateLabel();
				MenuSurvivorsClothingUI.craftingNewLabel.SizeScale_X = 1f;
				MenuSurvivorsClothingUI.craftingNewLabel.SizeScale_Y = 1f;
				MenuSurvivorsClothingUI.craftingNewLabel.TextContrastContext = 1;
				MenuSurvivorsClothingUI.craftingNewLabel.TextAlignment = 2;
				MenuSurvivorsClothingUI.craftingNewLabel.TextColor = Color.green;
				MenuSurvivorsClothingUI.craftingNewLabel.Text = Provider.localization.format("New");
				MenuSurvivorsClothingUI.craftingButton.AddChild(MenuSurvivorsClothingUI.craftingNewLabel);
			}
			TempSteamworksEconomy economyService = Provider.provider.economyService;
			economyService.onInventoryExchanged = (TempSteamworksEconomy.InventoryExchanged)Delegate.Combine(economyService.onInventoryExchanged, new TempSteamworksEconomy.InventoryExchanged(MenuSurvivorsClothingUI.onInventoryExchanged));
			TempSteamworksEconomy economyService2 = Provider.provider.economyService;
			economyService2.onInventoryPurchased = (TempSteamworksEconomy.InventoryExchanged)Delegate.Combine(economyService2.onInventoryPurchased, new TempSteamworksEconomy.InventoryExchanged(MenuSurvivorsClothingUI.onInventoryPurchased));
			TempSteamworksEconomy economyService3 = Provider.provider.economyService;
			economyService3.onInventoryExchangeFailed = (TempSteamworksEconomy.InventoryExchangeFailed)Delegate.Combine(economyService3.onInventoryExchangeFailed, new TempSteamworksEconomy.InventoryExchangeFailed(MenuSurvivorsClothingUI.onInventoryExchangeFailed));
			TempSteamworksEconomy economyService4 = Provider.provider.economyService;
			economyService4.onInventoryRefreshed = (TempSteamworksEconomy.InventoryRefreshed)Delegate.Combine(economyService4.onInventoryRefreshed, new TempSteamworksEconomy.InventoryRefreshed(MenuSurvivorsClothingUI.onInventoryRefreshed));
			TempSteamworksEconomy economyService5 = Provider.provider.economyService;
			economyService5.onInventoryDropped = (TempSteamworksEconomy.InventoryDropped)Delegate.Combine(economyService5.onInventoryDropped, new TempSteamworksEconomy.InventoryDropped(MenuSurvivorsClothingUI.onInventoryDropped));
			Characters.onCharacterUpdated = (CharacterUpdated)Delegate.Combine(Characters.onCharacterUpdated, new CharacterUpdated(MenuSurvivorsClothingUI.onCharacterUpdated));
			ItemStore.Get().OnPricesReceived += new Action(MenuSurvivorsClothingUI.OnPricesReceived);
			MenuSurvivorsClothingUI.updateFilter();
			MenuSurvivorsClothingUI.updatePage();
			this.itemUI = new MenuSurvivorsClothingItemUI();
			this.inspectUI = new MenuSurvivorsClothingInspectUI();
			this.deleteUI = new MenuSurvivorsClothingDeleteUI();
			this.boxUI = new MenuSurvivorsClothingBoxUI();
			this.itemStoreUI = new ItemStoreMenu();
			MenuUI.container.AddChild(this.itemStoreUI);
		}

		// Token: 0x04002B13 RID: 11027
		public static Local localization;

		// Token: 0x04002B14 RID: 11028
		public static Bundle icons;

		// Token: 0x04002B15 RID: 11029
		private static SleekFullscreenBox container;

		// Token: 0x04002B16 RID: 11030
		public static bool active;

		// Token: 0x04002B17 RID: 11031
		public static bool isCrafting;

		// Token: 0x04002B18 RID: 11032
		private static SleekButtonIcon backButton;

		// Token: 0x04002B19 RID: 11033
		private static ISleekButton itemstoreButton;

		// Token: 0x04002B1A RID: 11034
		private static ISleekLabel itemstoreNewLabel;

		// Token: 0x04002B1B RID: 11035
		private static SleekButtonIcon craftingButton;

		// Token: 0x04002B1C RID: 11036
		private static ISleekLabel craftingNewLabel;

		// Token: 0x04002B1D RID: 11037
		private static List<SteamItemDetails_t> filteredItems;

		// Token: 0x04002B1E RID: 11038
		private static ISleekConstraintFrame inventory;

		// Token: 0x04002B1F RID: 11039
		private static ISleekConstraintFrame crafting;

		// Token: 0x04002B20 RID: 11040
		private static SleekInventory[] packageButtons;

		// Token: 0x04002B21 RID: 11041
		private static ISleekBox availableBox;

		// Token: 0x04002B22 RID: 11042
		private static ISleekScrollView craftingScrollBox;

		// Token: 0x04002B23 RID: 11043
		private static ISleekButton[] craftingButtons;

		// Token: 0x04002B24 RID: 11044
		private static ISleekBox pageBox;

		// Token: 0x04002B25 RID: 11045
		private static ISleekBox infoBox;

		// Token: 0x04002B26 RID: 11046
		private static ISleekField searchField;

		// Token: 0x04002B27 RID: 11047
		private static ISleekButton searchButton;

		// Token: 0x04002B28 RID: 11048
		private static ISleekBox filterBox;

		// Token: 0x04002B29 RID: 11049
		private static ISleekButton cancelFilterButton;

		// Token: 0x04002B2A RID: 11050
		private static SleekButtonIcon leftButton;

		// Token: 0x04002B2B RID: 11051
		private static SleekButtonIcon rightButton;

		// Token: 0x04002B2C RID: 11052
		private static SleekButtonIcon refreshButton;

		/// <summary>
		/// Toggle button to open/close advanced filters panel.
		/// </summary>
		// Token: 0x04002B2D RID: 11053
		private static SleekButtonIcon optionsButton;

		/// <summary>
		/// On/off checkbox for including description text in filter.
		/// </summary>
		// Token: 0x04002B2E RID: 11054
		private static ISleekToggle searchDescriptionsToggle;

		/// <summary>
		/// Switch between sort modes.
		/// </summary>
		// Token: 0x04002B2F RID: 11055
		private static SleekButtonState sortModeButton;

		/// <summary>
		/// On/off checkbox to reverse sort results.
		/// </summary>
		// Token: 0x04002B30 RID: 11056
		private static ISleekToggle reverseSortOrderToggle;

		/// <summary>
		/// On/off checkbox to show only equipped items.
		/// </summary>
		// Token: 0x04002B31 RID: 11057
		private static ISleekToggle filterEquippedToggle;

		/// <summary>
		/// Container for advanced options.
		/// </summary>
		// Token: 0x04002B32 RID: 11058
		private static ISleekElement optionsPanel;

		// Token: 0x04002B33 RID: 11059
		private static ISleekSlider characterSlider;

		// Token: 0x04002B34 RID: 11060
		private static ISleekButton grantPackagePromoButton;

		// Token: 0x04002B35 RID: 11061
		private static int pageIndex;

		// Token: 0x04002B36 RID: 11062
		private static EEconFilterMode filterMode;

		// Token: 0x04002B37 RID: 11063
		private static ulong filterInstigator;

		/// <summary>
		/// Whether to include description text in filter.
		/// </summary>
		// Token: 0x04002B38 RID: 11064
		private static bool searchDescriptions;

		/// <summary>
		/// How to sort filtered items.
		/// </summary>
		// Token: 0x04002B39 RID: 11065
		private static MenuSurvivorsClothingUI.ESortMode sortMode;

		/// <summary>
		/// Should sorted list be reversed?
		/// </summary>
		// Token: 0x04002B3A RID: 11066
		private static bool reverseSortOrder;

		/// <summary>
		/// Should only equipped items be shown?
		/// </summary>
		// Token: 0x04002B3B RID: 11067
		private static bool filterEquipped;

		// Token: 0x04002B3C RID: 11068
		private MenuSurvivorsClothingItemUI itemUI;

		// Token: 0x04002B3D RID: 11069
		private MenuSurvivorsClothingInspectUI inspectUI;

		// Token: 0x04002B3E RID: 11070
		private MenuSurvivorsClothingDeleteUI deleteUI;

		// Token: 0x04002B3F RID: 11071
		private MenuSurvivorsClothingBoxUI boxUI;

		// Token: 0x04002B40 RID: 11072
		private ItemStoreMenu itemStoreUI;

		// Token: 0x04002B41 RID: 11073
		private List<EconCraftOption> econCraftOptions;

		// Token: 0x02000A0B RID: 2571
		private enum ESortMode
		{
			// Token: 0x04003507 RID: 13575
			Date,
			// Token: 0x04003508 RID: 13576
			Rarity,
			// Token: 0x04003509 RID: 13577
			Name,
			// Token: 0x0400350A RID: 13578
			Type
		}
	}
}
