using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Examine a store listing with description text.
	/// </summary>
	// Token: 0x020001AD RID: 429
	internal class ItemStoreDetailsMenu : SleekFullscreenBox
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x0002D3A5 File Offset: 0x0002B5A5
		// (set) Token: 0x06000D7D RID: 3453 RVA: 0x0002D3AD File Offset: 0x0002B5AD
		public bool IsOpen { get; private set; }

		// Token: 0x06000D7E RID: 3454 RVA: 0x0002D3B8 File Offset: 0x0002B5B8
		public void Open(ItemStore.Listing listing)
		{
			this.IsOpen = true;
			base.AnimateIntoView();
			this.listing = listing;
			this.quantityInCart = ItemStore.Get().GetQuantityInCart(listing.itemdefid);
			this.iconImage.SetItemDefId(listing.itemdefid);
			Color inventoryColor = Provider.provider.economyService.getInventoryColor(listing.itemdefid);
			this.nameLabel.TextColor = inventoryColor;
			this.nameLabel.Text = Provider.provider.economyService.getInventoryName(listing.itemdefid);
			string inventoryType = Provider.provider.economyService.getInventoryType(listing.itemdefid);
			string inventoryDescription = Provider.provider.economyService.getInventoryDescription(listing.itemdefid);
			this.descriptionLabel.Text = RichTextUtil.wrapWithColor(inventoryType, inventoryColor) + "\n\n" + inventoryDescription;
			this.RefreshQuantity();
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0002D496 File Offset: 0x0002B696
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			base.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0002D4B8 File Offset: 0x0002B6B8
		public ItemStoreDetailsMenu()
		{
			Local localization = ItemStoreMenu.instance.localization;
			ItemStoreDetailsMenu.instance = this;
			base.PositionScale_Y = 1f;
			base.PositionOffset_X = 10f;
			base.PositionOffset_Y = 10f;
			base.SizeOffset_X = -20f;
			base.SizeOffset_Y = -20f;
			base.SizeScale_X = 1f;
			base.SizeScale_Y = 1f;
			ISleekElement sleekElement = Glazier.Get().CreateFrame();
			sleekElement.PositionScale_X = 0.6f;
			sleekElement.PositionOffset_Y = 10f;
			sleekElement.SizeScale_X = 0.3f;
			sleekElement.SizeScale_Y = 1f;
			sleekElement.SizeOffset_Y = -20f;
			base.AddChild(sleekElement);
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.SizeScale_X = 1f;
			sleekBox.SizeScale_Y = 0.4f;
			sleekBox.SizeOffset_Y = -30f;
			sleekElement.AddChild(sleekBox);
			ISleekConstraintFrame sleekConstraintFrame = Glazier.Get().CreateConstraintFrame();
			sleekConstraintFrame.PositionOffset_X = 5f;
			sleekConstraintFrame.PositionOffset_Y = 5f;
			sleekConstraintFrame.SizeScale_X = 1f;
			sleekConstraintFrame.SizeScale_Y = 1f;
			sleekConstraintFrame.SizeOffset_Y = -70f;
			sleekConstraintFrame.Constraint = 1;
			sleekBox.AddChild(sleekConstraintFrame);
			this.iconImage = new SleekEconIcon();
			this.iconImage.SizeScale_X = 1f;
			this.iconImage.SizeScale_Y = 1f;
			sleekConstraintFrame.AddChild(this.iconImage);
			this.nameLabel = Glazier.Get().CreateLabel();
			this.nameLabel.PositionScale_Y = 1f;
			this.nameLabel.PositionOffset_Y = -70f;
			this.nameLabel.SizeScale_X = 1f;
			this.nameLabel.SizeOffset_Y = 70f;
			this.nameLabel.TextContrastContext = 1;
			this.nameLabel.FontSize = 4;
			sleekBox.AddChild(this.nameLabel);
			ISleekElement sleekElement2 = Glazier.Get().CreateFrame();
			sleekElement2.PositionOffset_Y = -25f;
			sleekElement2.PositionScale_Y = 0.4f;
			sleekElement2.SizeOffset_Y = 50f;
			sleekElement2.SizeScale_X = 1f;
			sleekElement.AddChild(sleekElement2);
			this.priceBox = new SleekItemStorePriceBox();
			this.priceBox.PositionScale_X = 0.75f;
			this.priceBox.SizeScale_X = 0.25f;
			this.priceBox.SizeScale_Y = 1f;
			sleekElement2.AddChild(this.priceBox);
			this.addToCartButton = Glazier.Get().CreateButton();
			this.addToCartButton.SizeScale_X = 0.75f;
			this.addToCartButton.SizeScale_Y = 1f;
			this.addToCartButton.FontSize = 3;
			this.addToCartButton.Text = localization.format("AddToCart_Label");
			this.addToCartButton.TooltipText = localization.format("AddToCart_Tooltip");
			this.addToCartButton.OnClicked += new ClickedButton(this.OnClickedAddToCart);
			sleekElement2.AddChild(this.addToCartButton);
			this.removeFromCartButton = Glazier.Get().CreateButton();
			this.removeFromCartButton.SizeScale_X = 0.5f;
			this.removeFromCartButton.SizeScale_Y = 1f;
			this.removeFromCartButton.FontSize = 3;
			this.removeFromCartButton.Text = localization.format("RemoveFromCart_Label");
			this.removeFromCartButton.TooltipText = localization.format("RemoveFromCart_Tooltip");
			this.removeFromCartButton.OnClicked += new ClickedButton(this.OnClickedRemoveFromCart);
			sleekElement2.AddChild(this.removeFromCartButton);
			this.quantityField = Glazier.Get().CreateInt32Field();
			this.quantityField.PositionScale_X = 0.5f;
			this.quantityField.SizeScale_X = 0.25f;
			this.quantityField.SizeOffset_X = -25f;
			this.quantityField.SizeScale_Y = 1f;
			this.quantityField.OnValueChanged += new TypedInt32(this.OnTypedQuantity);
			sleekElement2.AddChild(this.quantityField);
			this.incrementQuantityButton = Glazier.Get().CreateButton();
			this.incrementQuantityButton.PositionScale_X = 0.75f;
			this.incrementQuantityButton.PositionOffset_X = -25f;
			this.incrementQuantityButton.SizeOffset_X = 25f;
			this.incrementQuantityButton.SizeOffset_Y = 25f;
			this.incrementQuantityButton.FontSize = 3;
			this.incrementQuantityButton.Text = "+";
			this.incrementQuantityButton.OnClicked += new ClickedButton(this.OnClickedIncrementQuantity);
			sleekElement2.AddChild(this.incrementQuantityButton);
			this.decrementQuantityButton = Glazier.Get().CreateButton();
			this.decrementQuantityButton.PositionScale_X = 0.75f;
			this.decrementQuantityButton.PositionOffset_X = -25f;
			this.decrementQuantityButton.PositionOffset_Y = 25f;
			this.decrementQuantityButton.SizeOffset_X = 25f;
			this.decrementQuantityButton.SizeOffset_Y = 25f;
			this.decrementQuantityButton.FontSize = 3;
			this.decrementQuantityButton.Text = "-";
			this.decrementQuantityButton.OnClicked += new ClickedButton(this.OnClickedDecrementQuantity);
			sleekElement2.AddChild(this.decrementQuantityButton);
			ISleekBox sleekBox2 = Glazier.Get().CreateBox();
			sleekBox2.PositionScale_Y = 0.4f;
			sleekBox2.PositionOffset_Y = 30f;
			sleekBox2.SizeOffset_Y = -30f;
			sleekBox2.SizeScale_X = 1f;
			sleekBox2.SizeScale_Y = 0.6f;
			sleekElement.AddChild(sleekBox2);
			this.descriptionLabel = Glazier.Get().CreateLabel();
			this.descriptionLabel.PositionOffset_X = 5f;
			this.descriptionLabel.PositionOffset_Y = 5f;
			this.descriptionLabel.SizeScale_X = 1f;
			this.descriptionLabel.SizeScale_Y = 1f;
			this.descriptionLabel.SizeOffset_X = -10f;
			this.descriptionLabel.SizeOffset_Y = -10f;
			this.descriptionLabel.TextAlignment = 0;
			this.descriptionLabel.AllowRichText = true;
			this.descriptionLabel.TextColor = 4;
			this.descriptionLabel.TextContrastContext = 1;
			sleekBox2.AddChild(this.descriptionLabel);
			this.viewCartButton = Glazier.Get().CreateButton();
			this.viewCartButton.PositionOffset_Y = -110f;
			this.viewCartButton.PositionScale_Y = 1f;
			this.viewCartButton.SizeOffset_X = 200f;
			this.viewCartButton.SizeOffset_Y = 50f;
			this.viewCartButton.Text = ItemStoreMenu.instance.localization.format("ViewCart_Label");
			this.viewCartButton.TooltipText = ItemStoreMenu.instance.localization.format("ViewCart_Tooltip");
			this.viewCartButton.OnClicked += new ClickedButton(this.OnClickedViewCartButton);
			this.viewCartButton.FontSize = 3;
			base.AddChild(this.viewCartButton);
			ISleekSprite sleekSprite = Glazier.Get().CreateSprite(ItemStoreMenu.instance.icons.load<Sprite>("Cart"));
			sleekSprite.PositionOffset_X = 5f;
			sleekSprite.PositionOffset_Y = 5f;
			sleekSprite.SizeOffset_X = 40f;
			sleekSprite.SizeOffset_Y = 40f;
			sleekSprite.TintColor = 2;
			sleekSprite.DrawMethod = 2;
			this.viewCartButton.AddChild(sleekSprite);
			SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			sleekButtonIcon.PositionOffset_Y = -50f;
			sleekButtonIcon.PositionScale_Y = 1f;
			sleekButtonIcon.SizeOffset_X = 200f;
			sleekButtonIcon.SizeOffset_Y = 50f;
			sleekButtonIcon.text = MenuDashboardUI.localization.format("BackButtonText");
			sleekButtonIcon.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			sleekButtonIcon.onClickedButton += new ClickedButton(this.OnClickedBackButton);
			sleekButtonIcon.fontSize = 3;
			sleekButtonIcon.iconColor = 2;
			base.AddChild(sleekButtonIcon);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0002DCBC File Offset: 0x0002BEBC
		private void RefreshQuantity()
		{
			this.priceBox.SetPrice(this.listing.basePrice, this.listing.currentPrice, this.quantityInCart);
			bool flag = this.quantityInCart > 0;
			this.addToCartButton.IsVisible = !flag;
			this.removeFromCartButton.IsVisible = flag;
			this.quantityField.Value = this.quantityInCart;
			this.quantityField.IsVisible = flag;
			this.incrementQuantityButton.IsVisible = flag;
			this.decrementQuantityButton.IsVisible = flag;
			this.viewCartButton.IsVisible = !ItemStore.Get().IsCartEmpty;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0002DD62 File Offset: 0x0002BF62
		private void SetQuantityInCart(int value)
		{
			this.quantityInCart = value;
			ItemStore.Get().SetQuantityInCart(this.listing.itemdefid, this.quantityInCart);
			this.RefreshQuantity();
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0002DD8C File Offset: 0x0002BF8C
		private void OnClickedAddToCart(ISleekElement button)
		{
			this.SetQuantityInCart(1);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0002DD95 File Offset: 0x0002BF95
		private void OnClickedRemoveFromCart(ISleekElement button)
		{
			this.SetQuantityInCart(0);
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0002DD9E File Offset: 0x0002BF9E
		private void OnTypedQuantity(ISleekInt32Field field, int value)
		{
			this.SetQuantityInCart(Mathf.Max(0, value));
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0002DDAD File Offset: 0x0002BFAD
		private void OnClickedIncrementQuantity(ISleekElement button)
		{
			this.SetQuantityInCart(this.quantityInCart + 1);
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0002DDBD File Offset: 0x0002BFBD
		private void OnClickedDecrementQuantity(ISleekElement button)
		{
			this.SetQuantityInCart(this.quantityInCart - 1);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0002DDCD File Offset: 0x0002BFCD
		private void OnClickedViewCartButton(ISleekElement button)
		{
			ItemStoreCartMenu.instance.Open();
			this.Close();
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0002DDDF File Offset: 0x0002BFDF
		private void OnClickedBackButton(ISleekElement button)
		{
			ItemStoreMenu.instance.Open();
			this.Close();
		}

		// Token: 0x0400051C RID: 1308
		public static ItemStoreDetailsMenu instance;

		// Token: 0x0400051E RID: 1310
		private ItemStore.Listing listing;

		// Token: 0x0400051F RID: 1311
		private int quantityInCart;

		// Token: 0x04000520 RID: 1312
		private ISleekLabel nameLabel;

		// Token: 0x04000521 RID: 1313
		private ISleekLabel descriptionLabel;

		// Token: 0x04000522 RID: 1314
		private SleekEconIcon iconImage;

		// Token: 0x04000523 RID: 1315
		private SleekItemStorePriceBox priceBox;

		// Token: 0x04000524 RID: 1316
		private ISleekButton addToCartButton;

		// Token: 0x04000525 RID: 1317
		private ISleekButton removeFromCartButton;

		// Token: 0x04000526 RID: 1318
		private ISleekInt32Field quantityField;

		// Token: 0x04000527 RID: 1319
		private ISleekButton incrementQuantityButton;

		// Token: 0x04000528 RID: 1320
		private ISleekButton decrementQuantityButton;

		/// <summary>
		/// Only visible when cart is not empty.
		/// </summary>
		// Token: 0x04000529 RID: 1321
		private ISleekButton viewCartButton;
	}
}
