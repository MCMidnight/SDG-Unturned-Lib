using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001AE RID: 430
	internal class ItemStoreMenu : SleekFullscreenBox
	{
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0002DDF1 File Offset: 0x0002BFF1
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x0002DDF9 File Offset: 0x0002BFF9
		public Local localization { get; private set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x0002DE02 File Offset: 0x0002C002
		// (set) Token: 0x06000D8D RID: 3469 RVA: 0x0002DE0A File Offset: 0x0002C00A
		public Bundle icons { get; private set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x0002DE13 File Offset: 0x0002C013
		// (set) Token: 0x06000D8F RID: 3471 RVA: 0x0002DE1B File Offset: 0x0002C01B
		public bool IsOpen { get; private set; }

		// Token: 0x06000D90 RID: 3472 RVA: 0x0002DE24 File Offset: 0x0002C024
		public void Open()
		{
			this.IsOpen = true;
			base.AnimateIntoView();
			this.viewCartButton.IsVisible = !ItemStore.Get().IsCartEmpty;
			if (this.areListingsDirty)
			{
				this.areListingsDirty = false;
				this.FilterListings();
				return;
			}
			this.RefreshListingsInCart();
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0002DE72 File Offset: 0x0002C072
		public void OpenNewItems()
		{
			this.searchField.Text = string.Empty;
			this.categoryFilter = ItemStoreMenu.ECategoryFilter.New;
			this.areListingsDirty = true;
			this.Open();
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0002DE98 File Offset: 0x0002C098
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			base.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0002DEBC File Offset: 0x0002C0BC
		public ItemStoreMenu()
		{
			this.localization = Localization.read("/Menu/Survivors/ItemStoreMenu.dat");
			this.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Survivors/ItemStore/ItemStore.unity3d");
			ItemStoreMenu.instance = this;
			base.PositionScale_Y = 1f;
			base.PositionOffset_X = 10f;
			base.PositionOffset_Y = 10f;
			base.SizeOffset_X = -20f;
			base.SizeOffset_Y = -20f;
			base.SizeScale_X = 1f;
			base.SizeScale_Y = 1f;
			ISleekConstraintFrame sleekConstraintFrame = Glazier.Get().CreateConstraintFrame();
			sleekConstraintFrame.PositionOffset_Y = 70f;
			sleekConstraintFrame.PositionScale_X = 0.5f;
			sleekConstraintFrame.SizeScale_X = 0.5f;
			sleekConstraintFrame.SizeScale_Y = 1f;
			sleekConstraintFrame.SizeOffset_Y = -105f;
			sleekConstraintFrame.Constraint = 1;
			base.AddChild(sleekConstraintFrame);
			this.listingButtons = new SleekItemStoreListing[25];
			for (int i = 0; i < 25; i++)
			{
				SleekItemStoreListing sleekItemStoreListing = new SleekItemStoreListing();
				sleekItemStoreListing.PositionOffset_X = 5f;
				sleekItemStoreListing.PositionOffset_Y = 5f;
				sleekItemStoreListing.PositionScale_X = (float)(i % 5) * 0.2f;
				sleekItemStoreListing.PositionScale_Y = (float)Mathf.FloorToInt((float)i / 5f) * 0.2f;
				sleekItemStoreListing.SizeOffset_X = -10f;
				sleekItemStoreListing.SizeOffset_Y = -10f;
				sleekItemStoreListing.SizeScale_X = 0.2f;
				sleekItemStoreListing.SizeScale_Y = 0.2f;
				sleekConstraintFrame.AddChild(sleekItemStoreListing);
				this.listingButtons[i] = sleekItemStoreListing;
			}
			this.categoryButtonsFrame = Glazier.Get().CreateFrame();
			this.categoryButtonsFrame.PositionOffset_Y = -70f;
			this.categoryButtonsFrame.SizeScale_X = 1f;
			this.categoryButtonsFrame.SizeOffset_Y = 30f;
			sleekConstraintFrame.AddChild(this.categoryButtonsFrame);
			this.searchField = Glazier.Get().CreateStringField();
			this.searchField.PositionOffset_Y = -35f;
			this.searchField.SizeOffset_X = -110f;
			this.searchField.SizeOffset_Y = 30f;
			this.searchField.SizeScale_X = 1f;
			this.searchField.PlaceholderText = MenuSurvivorsClothingUI.localization.format("Search_Field_Hint");
			this.searchField.OnTextSubmitted += new Entered(this.OnEnteredSearchField);
			sleekConstraintFrame.AddChild(this.searchField);
			ISleekButton sleekButton = Glazier.Get().CreateButton();
			sleekButton.PositionOffset_X = -105f;
			sleekButton.PositionOffset_Y = -35f;
			sleekButton.PositionScale_X = 1f;
			sleekButton.SizeOffset_X = 100f;
			sleekButton.SizeOffset_Y = 30f;
			sleekButton.Text = MenuSurvivorsClothingUI.localization.format("Search");
			sleekButton.TooltipText = MenuSurvivorsClothingUI.localization.format("Search_Tooltip");
			sleekButton.OnClicked += new ClickedButton(this.OnClickedSearchButton);
			sleekConstraintFrame.AddChild(sleekButton);
			this.pageBox = Glazier.Get().CreateBox();
			this.pageBox.PositionOffset_X = -50f;
			this.pageBox.PositionOffset_Y = 5f;
			this.pageBox.PositionScale_X = 0.5f;
			this.pageBox.PositionScale_Y = 1f;
			this.pageBox.SizeOffset_X = 100f;
			this.pageBox.SizeOffset_Y = 30f;
			this.pageBox.FontSize = 3;
			sleekConstraintFrame.AddChild(this.pageBox);
			SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(MenuSurvivorsClothingUI.icons.load<Texture2D>("Left"));
			sleekButtonIcon.PositionOffset_X = -85f;
			sleekButtonIcon.PositionOffset_Y = 5f;
			sleekButtonIcon.PositionScale_X = 0.5f;
			sleekButtonIcon.PositionScale_Y = 1f;
			sleekButtonIcon.SizeOffset_X = 30f;
			sleekButtonIcon.SizeOffset_Y = 30f;
			sleekButtonIcon.tooltip = MenuSurvivorsClothingUI.localization.format("Left_Tooltip");
			sleekButtonIcon.iconColor = 2;
			sleekButtonIcon.onClickedButton += new ClickedButton(this.OnClickedLeftPageButton);
			sleekConstraintFrame.AddChild(sleekButtonIcon);
			SleekButtonIcon sleekButtonIcon2 = new SleekButtonIcon(MenuSurvivorsClothingUI.icons.load<Texture2D>("Right"));
			sleekButtonIcon2.PositionOffset_X = 55f;
			sleekButtonIcon2.PositionOffset_Y = 5f;
			sleekButtonIcon2.PositionScale_X = 0.5f;
			sleekButtonIcon2.PositionScale_Y = 1f;
			sleekButtonIcon2.SizeOffset_X = 30f;
			sleekButtonIcon2.SizeOffset_Y = 30f;
			sleekButtonIcon2.tooltip = MenuSurvivorsClothingUI.localization.format("Right_Tooltip");
			sleekButtonIcon2.iconColor = 2;
			sleekButtonIcon2.onClickedButton += new ClickedButton(this.OnClickedRightPageButton);
			sleekConstraintFrame.AddChild(sleekButtonIcon2);
			this.viewCartButton = Glazier.Get().CreateButton();
			this.viewCartButton.PositionOffset_Y = -110f;
			this.viewCartButton.PositionScale_Y = 1f;
			this.viewCartButton.SizeOffset_X = 200f;
			this.viewCartButton.SizeOffset_Y = 50f;
			this.viewCartButton.Text = this.localization.format("ViewCart_Label");
			this.viewCartButton.TooltipText = this.localization.format("ViewCart_Tooltip");
			this.viewCartButton.OnClicked += new ClickedButton(this.OnClickedViewCartButton);
			this.viewCartButton.FontSize = 3;
			base.AddChild(this.viewCartButton);
			ISleekSprite sleekSprite = Glazier.Get().CreateSprite(this.icons.load<Sprite>("Cart"));
			sleekSprite.PositionOffset_X = 5f;
			sleekSprite.PositionOffset_Y = 5f;
			sleekSprite.SizeOffset_X = 40f;
			sleekSprite.SizeOffset_Y = 40f;
			sleekSprite.TintColor = 2;
			sleekSprite.DrawMethod = 2;
			this.viewCartButton.AddChild(sleekSprite);
			SleekButtonIcon sleekButtonIcon3 = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			sleekButtonIcon3.PositionOffset_Y = -50f;
			sleekButtonIcon3.PositionScale_Y = 1f;
			sleekButtonIcon3.SizeOffset_X = 200f;
			sleekButtonIcon3.SizeOffset_Y = 50f;
			sleekButtonIcon3.text = MenuDashboardUI.localization.format("BackButtonText");
			sleekButtonIcon3.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			sleekButtonIcon3.onClickedButton += new ClickedButton(this.OnClickedBackButton);
			sleekButtonIcon3.fontSize = 3;
			sleekButtonIcon3.iconColor = 2;
			base.AddChild(sleekButtonIcon3);
			this.cartMenu = new ItemStoreCartMenu();
			MenuUI.container.AddChild(this.cartMenu);
			this.detailsMenu = new ItemStoreDetailsMenu();
			MenuUI.container.AddChild(this.detailsMenu);
			ItemStore.Get().OnPricesReceived += new Action(this.OnPricesReceived);
			ItemStore.Get().OnPurchaseResult += new Action<ItemStore.EPurchaseResult>(this.OnPurchaseResult);
			ItemStore.Get().RequestPrices();
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0002E57F File Offset: 0x0002C77F
		public override void OnDestroy()
		{
			ItemStore.Get().OnPricesReceived -= new Action(this.OnPricesReceived);
			ItemStore.Get().OnPurchaseResult -= new Action<ItemStore.EPurchaseResult>(this.OnPurchaseResult);
			base.OnDestroy();
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0002E5B3 File Offset: 0x0002C7B3
		private void OnPricesReceived()
		{
			this.filteredListings = new List<ItemStore.Listing>(ItemStore.Get().GetListings().Length);
			this.CreateFilterCategoryButtons();
			this.areListingsDirty = true;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0002E5D9 File Offset: 0x0002C7D9
		private void OnPurchaseResult(ItemStore.EPurchaseResult result)
		{
			if (result == ItemStore.EPurchaseResult.UnableToInitialize)
			{
				MenuUI.alert(this.localization.format("PurchaseResult_UnableToInitialize"));
				return;
			}
			if (result != ItemStore.EPurchaseResult.Denied)
			{
				return;
			}
			MenuUI.alert(this.localization.format("PurchaseResult_Denied"));
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0002E610 File Offset: 0x0002C810
		private void BuildListingsFromIndices(int[] listingIndices)
		{
			ItemStore.Listing[] listings = ItemStore.Get().GetListings();
			foreach (int num in listingIndices)
			{
				this.filteredListings.Add(listings[num]);
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0002E650 File Offset: 0x0002C850
		private void BuildCategoryListings()
		{
			if (this.categoryFilter == ItemStoreMenu.ECategoryFilter.Specials)
			{
				this.BuildListingsFromIndices(ItemStore.Get().GetDiscountedListingIndices());
				return;
			}
			if (this.categoryFilter == ItemStoreMenu.ECategoryFilter.New)
			{
				this.BuildListingsFromIndices(ItemStore.Get().GetNewListingIndices());
				return;
			}
			if (this.categoryFilter == ItemStoreMenu.ECategoryFilter.Featured)
			{
				this.BuildListingsFromIndices(ItemStore.Get().GetFeaturedListingIndices());
				return;
			}
			ItemStore.Listing[] listings = ItemStore.Get().GetListings();
			this.filteredListings.AddRange(listings);
			if (this.categoryFilter == ItemStoreMenu.ECategoryFilter.Bundles)
			{
				for (int i = this.filteredListings.Count - 1; i >= 0; i--)
				{
					if (!Provider.provider.economyService.getInventoryType(this.filteredListings[i].itemdefid).EndsWith("Bundle"))
					{
						this.filteredListings.RemoveAtFast(i);
					}
				}
			}
		}

		/// <summary>
		/// Remove items that do not match search text.
		/// </summary>
		// Token: 0x06000D99 RID: 3481 RVA: 0x0002E71C File Offset: 0x0002C91C
		private void ApplySearchTextFilter()
		{
			string text = this.searchField.Text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			TokenSearchFilter? tokenSearchFilter = TokenSearchFilter.parse(text);
			if (tokenSearchFilter == null)
			{
				return;
			}
			for (int i = this.filteredListings.Count - 1; i >= 0; i--)
			{
				int itemdefid = this.filteredListings[i].itemdefid;
				string inventoryName = Provider.provider.economyService.getInventoryName(itemdefid);
				if (!tokenSearchFilter.Value.matches(inventoryName))
				{
					string inventoryType = Provider.provider.economyService.getInventoryType(itemdefid);
					if (!tokenSearchFilter.Value.matches(inventoryType))
					{
						this.filteredListings.RemoveAtFast(i);
					}
				}
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0002E7D1 File Offset: 0x0002C9D1
		private void SortListings()
		{
			this.filteredListings.Sort(delegate(ItemStore.Listing lhs, ItemStore.Listing rhs)
			{
				string inventoryName = Provider.provider.economyService.getInventoryName(lhs.itemdefid);
				string inventoryName2 = Provider.provider.economyService.getInventoryName(rhs.itemdefid);
				return inventoryName.CompareTo(inventoryName2);
			});
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0002E800 File Offset: 0x0002CA00
		private void FilterListings()
		{
			this.filteredListings.Clear();
			this.BuildCategoryListings();
			this.ApplySearchTextFilter();
			this.SortListings();
			this.pageCount = MathfEx.GetPageCount(this.filteredListings.Count, this.listingButtons.Length);
			if (this.pageIndex >= this.pageCount)
			{
				this.pageIndex = this.pageCount - 1;
			}
			this.RefreshPage();
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0002E86C File Offset: 0x0002CA6C
		private void RefreshListingsInCart()
		{
			SleekItemStoreListing[] array = this.listingButtons;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RefreshInCart();
			}
		}

		/// <summary>
		/// Note SetListing also calls RefreshInCart.
		/// </summary>
		// Token: 0x06000D9D RID: 3485 RVA: 0x0002E898 File Offset: 0x0002CA98
		private void RefreshPage()
		{
			this.pageBox.Text = MenuSurvivorsClothingUI.localization.format("Page", this.pageIndex + 1, this.pageCount);
			int num = this.pageIndex * this.listingButtons.Length;
			int num2 = Mathf.Min(this.filteredListings.Count - num, this.listingButtons.Length);
			for (int i = 0; i < num2; i++)
			{
				int num3 = num + i;
				this.listingButtons[i].SetListing(this.filteredListings[num3]);
			}
			for (int j = num2; j < this.listingButtons.Length; j++)
			{
				this.listingButtons[j].ClearListing();
			}
		}

		/// <summary>
		/// Cannot be created until store data is available.
		/// </summary>
		// Token: 0x06000D9E RID: 3486 RVA: 0x0002E950 File Offset: 0x0002CB50
		private void CreateFilterCategoryButtons()
		{
			ItemStore itemStore = ItemStore.Get();
			bool hasDiscountedListings = itemStore.HasDiscountedListings;
			bool hasNewListings = itemStore.HasNewListings;
			bool hasFeaturedListings = itemStore.HasFeaturedListings;
			int num = 2 + (hasNewListings ? 1 : 0) + (hasDiscountedListings ? 1 : 0) + (hasFeaturedListings ? 1 : 0);
			float num2 = 0f;
			float num3 = 1f / (float)num;
			if (hasNewListings)
			{
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionScale_X = num2;
				sleekButton.SizeScale_X = num3;
				sleekButton.SizeScale_Y = 1f;
				sleekButton.Text = this.localization.format("FilterNewButton_Label") + " x" + itemStore.GetNewListingIndices().Length.ToString();
				sleekButton.TooltipText = this.localization.format("FilterNewButton_Tooltip");
				sleekButton.OnClicked += new ClickedButton(this.OnClickedFilterNew);
				this.categoryButtonsFrame.AddChild(sleekButton);
				num2 += num3;
			}
			if (hasFeaturedListings)
			{
				ISleekButton sleekButton2 = Glazier.Get().CreateButton();
				sleekButton2.PositionScale_X = num2;
				sleekButton2.SizeScale_X = num3;
				sleekButton2.SizeScale_Y = 1f;
				sleekButton2.Text = this.localization.format("FilterFeaturedButton_Label") + " x" + itemStore.GetFeaturedListingIndices().Length.ToString();
				sleekButton2.TooltipText = this.localization.format("FilterFeaturedButton_Label");
				sleekButton2.OnClicked += new ClickedButton(this.OnClickedFilterFeatured);
				this.categoryButtonsFrame.AddChild(sleekButton2);
				num2 += num3;
			}
			ISleekButton sleekButton3 = Glazier.Get().CreateButton();
			sleekButton3.PositionScale_X = num2;
			sleekButton3.SizeScale_X = num3;
			sleekButton3.SizeScale_Y = 1f;
			sleekButton3.Text = this.localization.format("FilterAllButton_Label");
			sleekButton3.TooltipText = this.localization.format("FilterAllButton_Tooltip");
			sleekButton3.OnClicked += new ClickedButton(this.OnClickedFilterAll);
			this.categoryButtonsFrame.AddChild(sleekButton3);
			num2 += num3;
			ISleekButton sleekButton4 = Glazier.Get().CreateButton();
			sleekButton4.PositionScale_X = num2;
			sleekButton4.SizeScale_X = num3;
			sleekButton4.SizeScale_Y = 1f;
			sleekButton4.Text = this.localization.format("FilterBundlesButton_Label");
			sleekButton4.TooltipText = this.localization.format("FilterBundlesButton_Tooltip");
			sleekButton4.OnClicked += new ClickedButton(this.OnClickedFilterBundles);
			this.categoryButtonsFrame.AddChild(sleekButton4);
			num2 += num3;
			if (hasDiscountedListings)
			{
				ISleekButton sleekButton5 = Glazier.Get().CreateButton();
				sleekButton5.PositionScale_X = num2;
				sleekButton5.SizeScale_X = num3;
				sleekButton5.SizeScale_Y = 1f;
				sleekButton5.Text = this.localization.format("FilterSpecialsButton_Label") + " x" + itemStore.GetDiscountedListingIndices().Length.ToString();
				sleekButton5.TooltipText = this.localization.format("FilterSpecialsButton_Tooltip");
				sleekButton5.OnClicked += new ClickedButton(this.OnClickedFilterSpecials);
				this.categoryButtonsFrame.AddChild(sleekButton5);
				num2 += num3;
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0002EC83 File Offset: 0x0002CE83
		private void OnClickedLeftPageButton(ISleekElement button)
		{
			if (this.pageIndex > 0)
			{
				this.pageIndex--;
			}
			else
			{
				this.pageIndex = this.pageCount - 1;
			}
			this.RefreshPage();
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0002ECB2 File Offset: 0x0002CEB2
		private void OnClickedRightPageButton(ISleekElement button)
		{
			if (this.pageIndex < this.pageCount - 1)
			{
				this.pageIndex++;
			}
			else
			{
				this.pageIndex = 0;
			}
			this.RefreshPage();
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0002ECE1 File Offset: 0x0002CEE1
		private void OnClickedFilterAll(ISleekElement button)
		{
			this.categoryFilter = ItemStoreMenu.ECategoryFilter.None;
			this.FilterListings();
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0002ECF0 File Offset: 0x0002CEF0
		private void OnClickedFilterBundles(ISleekElement button)
		{
			this.categoryFilter = ItemStoreMenu.ECategoryFilter.Bundles;
			this.FilterListings();
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0002ECFF File Offset: 0x0002CEFF
		private void OnClickedFilterSpecials(ISleekElement button)
		{
			this.categoryFilter = ItemStoreMenu.ECategoryFilter.Specials;
			this.FilterListings();
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0002ED0E File Offset: 0x0002CF0E
		private void OnClickedFilterNew(ISleekElement button)
		{
			this.categoryFilter = ItemStoreMenu.ECategoryFilter.New;
			this.FilterListings();
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0002ED1D File Offset: 0x0002CF1D
		private void OnClickedFilterFeatured(ISleekElement button)
		{
			this.categoryFilter = ItemStoreMenu.ECategoryFilter.Featured;
			this.FilterListings();
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0002ED2C File Offset: 0x0002CF2C
		private void OnEnteredSearchField(ISleekField field)
		{
			this.FilterListings();
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0002ED34 File Offset: 0x0002CF34
		private void OnClickedSearchButton(ISleekElement button)
		{
			this.FilterListings();
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0002ED3C File Offset: 0x0002CF3C
		private void OnClickedViewCartButton(ISleekElement button)
		{
			this.cartMenu.Open();
			this.Close();
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0002ED4F File Offset: 0x0002CF4F
		private void OnClickedBackButton(ISleekElement button)
		{
			MenuSurvivorsClothingUI.open();
			this.Close();
		}

		// Token: 0x0400052A RID: 1322
		public static ItemStoreMenu instance;

		// Token: 0x0400052E RID: 1326
		private ItemStoreCartMenu cartMenu;

		// Token: 0x0400052F RID: 1327
		private ItemStoreDetailsMenu detailsMenu;

		// Token: 0x04000530 RID: 1328
		private SleekItemStoreListing[] listingButtons;

		// Token: 0x04000531 RID: 1329
		private ISleekElement categoryButtonsFrame;

		// Token: 0x04000532 RID: 1330
		private ISleekField searchField;

		/// <summary>
		/// Displays the current page number.
		/// </summary>
		// Token: 0x04000533 RID: 1331
		private ISleekBox pageBox;

		/// <summary>
		/// Only visible when cart is not empty.
		/// </summary>
		// Token: 0x04000534 RID: 1332
		private ISleekButton viewCartButton;

		// Token: 0x04000535 RID: 1333
		private List<ItemStore.Listing> filteredListings;

		/// <summary>
		/// [0, pageCount)
		/// </summary>
		// Token: 0x04000536 RID: 1334
		private int pageIndex;

		// Token: 0x04000537 RID: 1335
		private int pageCount;

		/// <summary>
		/// If true, listings should be re-filtered when opening the menu.
		/// </summary>
		// Token: 0x04000538 RID: 1336
		private bool areListingsDirty;

		// Token: 0x04000539 RID: 1337
		private ItemStoreMenu.ECategoryFilter categoryFilter;

		// Token: 0x02000881 RID: 2177
		private enum ECategoryFilter
		{
			// Token: 0x040031A6 RID: 12710
			None,
			/// <summary>
			/// Collections of multiple items. 
			/// </summary>
			// Token: 0x040031A7 RID: 12711
			Bundles,
			/// <summary>
			/// Discounted items.
			/// </summary>
			// Token: 0x040031A8 RID: 12712
			Specials,
			/// <summary>
			/// Items marked as new in the Status.json file.
			/// </summary>
			// Token: 0x040031A9 RID: 12713
			New,
			/// <summary>
			/// Items marked as featured in the Status.json file.
			/// </summary>
			// Token: 0x040031AA RID: 12714
			Featured
		}
	}
}
