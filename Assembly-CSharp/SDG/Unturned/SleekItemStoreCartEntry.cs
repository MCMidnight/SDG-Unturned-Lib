using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001AF RID: 431
	internal class SleekItemStoreCartEntry : SleekWrapper
	{
		// Token: 0x06000DAA RID: 3498 RVA: 0x0002ED5C File Offset: 0x0002CF5C
		public void GetTotalPrice(out ulong basePrice, out ulong currentPrice)
		{
			basePrice = this.listing.basePrice * (ulong)this.cartEntry.quantity;
			currentPrice = this.listing.currentPrice * (ulong)this.cartEntry.quantity;
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0002ED94 File Offset: 0x0002CF94
		public SleekItemStoreCartEntry(ItemStore.CartEntry cartEntry, ItemStore.Listing listing)
		{
			Local localization = ItemStoreMenu.instance.localization;
			this.cartEntry = cartEntry;
			this.listing = listing;
			this.itemButton = Glazier.Get().CreateButton();
			this.itemButton.SizeScale_X = 0.4f;
			this.itemButton.SizeScale_Y = 1f;
			this.itemButton.OnClicked += new ClickedButton(this.OnClickedItemButton);
			this.itemButton.TooltipText = Provider.provider.economyService.getInventoryType(listing.itemdefid);
			base.AddChild(this.itemButton);
			this.iconImage = new SleekEconIcon();
			this.iconImage.PositionOffset_X = 5f;
			this.iconImage.PositionOffset_Y = 5f;
			this.iconImage.SizeOffset_X = 40f;
			this.iconImage.SizeOffset_Y = 40f;
			this.iconImage.SetItemDefId(listing.itemdefid);
			this.itemButton.AddChild(this.iconImage);
			this.nameLabel = Glazier.Get().CreateLabel();
			this.nameLabel.PositionOffset_X = 50f;
			this.nameLabel.SizeScale_X = 1f;
			this.nameLabel.SizeScale_Y = 1f;
			this.nameLabel.SizeOffset_X = -50f;
			this.nameLabel.TextAlignment = 3;
			this.nameLabel.FontSize = 3;
			this.nameLabel.Text = Provider.provider.economyService.getInventoryName(listing.itemdefid);
			this.nameLabel.TextContrastContext = 1;
			this.itemButton.AddChild(this.nameLabel);
			this.addToCartButton = Glazier.Get().CreateButton();
			this.addToCartButton.PositionScale_X = 0.4f;
			this.addToCartButton.SizeScale_X = 0.6f;
			this.addToCartButton.SizeScale_Y = 1f;
			this.addToCartButton.Text = localization.format("AddToCart_Label");
			this.addToCartButton.TooltipText = localization.format("AddToCart_Tooltip");
			this.addToCartButton.OnClicked += new ClickedButton(this.OnClickedAddToCart);
			this.addToCartButton.BackgroundColor = new SleekColor(1, 0.5f);
			base.AddChild(this.addToCartButton);
			this.removeFromCartButton = Glazier.Get().CreateButton();
			this.removeFromCartButton.PositionScale_X = 0.4f;
			this.removeFromCartButton.SizeScale_X = 0.2f;
			this.removeFromCartButton.SizeScale_Y = 1f;
			this.removeFromCartButton.Text = localization.format("RemoveFromCart_Label");
			this.removeFromCartButton.TooltipText = localization.format("RemoveFromCart_Tooltip");
			this.removeFromCartButton.OnClicked += new ClickedButton(this.OnClickedRemoveFromCart);
			base.AddChild(this.removeFromCartButton);
			this.quantityField = Glazier.Get().CreateInt32Field();
			this.quantityField.PositionScale_X = 0.6f;
			this.quantityField.SizeScale_X = 0.2f;
			this.quantityField.SizeOffset_X = -25f;
			this.quantityField.SizeScale_Y = 1f;
			this.quantityField.OnValueChanged += new TypedInt32(this.OnTypedQuantity);
			base.AddChild(this.quantityField);
			this.incrementQuantityButton = Glazier.Get().CreateButton();
			this.incrementQuantityButton.PositionScale_X = 0.8f;
			this.incrementQuantityButton.PositionOffset_X = -25f;
			this.incrementQuantityButton.SizeOffset_X = 25f;
			this.incrementQuantityButton.SizeOffset_Y = 25f;
			this.incrementQuantityButton.FontSize = 3;
			this.incrementQuantityButton.Text = "+";
			this.incrementQuantityButton.OnClicked += new ClickedButton(this.OnClickedIncrementQuantity);
			base.AddChild(this.incrementQuantityButton);
			this.decrementQuantityButton = Glazier.Get().CreateButton();
			this.decrementQuantityButton.PositionScale_X = 0.8f;
			this.decrementQuantityButton.PositionOffset_X = -25f;
			this.decrementQuantityButton.PositionOffset_Y = 25f;
			this.decrementQuantityButton.SizeOffset_X = 25f;
			this.decrementQuantityButton.SizeOffset_Y = 25f;
			this.decrementQuantityButton.FontSize = 3;
			this.decrementQuantityButton.Text = "-";
			this.decrementQuantityButton.OnClicked += new ClickedButton(this.OnClickedDecrementQuantity);
			base.AddChild(this.decrementQuantityButton);
			this.priceBox = new SleekItemStorePriceBox();
			this.priceBox.PositionScale_X = 0.8f;
			this.priceBox.SizeScale_X = 0.2f;
			this.priceBox.SizeOffset_Y = 50f;
			base.AddChild(this.priceBox);
			this.RefreshQuantity();
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0002F268 File Offset: 0x0002D468
		private void RefreshQuantity()
		{
			this.priceBox.SetPrice(this.listing.basePrice, this.listing.currentPrice, this.cartEntry.quantity);
			this.quantityField.Value = this.cartEntry.quantity;
			bool flag = this.cartEntry.quantity > 0;
			this.addToCartButton.IsVisible = !flag;
			this.removeFromCartButton.IsVisible = flag;
			this.quantityField.Value = this.cartEntry.quantity;
			this.quantityField.IsVisible = flag;
			this.incrementQuantityButton.IsVisible = flag;
			this.decrementQuantityButton.IsVisible = flag;
			this.priceBox.IsVisible = flag;
			Color inventoryColor = Provider.provider.economyService.getInventoryColor(this.listing.itemdefid);
			this.itemButton.TextColor = inventoryColor;
			inventoryColor.a = (flag ? 1f : 0.5f);
			SleekColor backgroundColor = SleekColor.BackgroundIfLight(inventoryColor);
			backgroundColor.SetAlpha(inventoryColor.a);
			this.itemButton.BackgroundColor = backgroundColor;
			this.iconImage.color = new SleekColor(0, flag ? 1f : 0.5f);
			this.nameLabel.TextColor = inventoryColor;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0002F3BA File Offset: 0x0002D5BA
		private void SetQuantityInCart(int value)
		{
			this.cartEntry.quantity = value;
			ItemStore.Get().SetQuantityInCart(this.cartEntry.itemdefid, value);
			this.RefreshQuantity();
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0002F3E4 File Offset: 0x0002D5E4
		private void OnClickedItemButton(ISleekElement button)
		{
			ItemStoreDetailsMenu.instance.Open(this.listing);
			ItemStoreCartMenu.instance.Close();
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0002F400 File Offset: 0x0002D600
		private void OnClickedAddToCart(ISleekElement button)
		{
			this.SetQuantityInCart(1);
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0002F409 File Offset: 0x0002D609
		private void OnClickedRemoveFromCart(ISleekElement button)
		{
			this.SetQuantityInCart(0);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0002F412 File Offset: 0x0002D612
		private void OnTypedQuantity(ISleekInt32Field field, int value)
		{
			this.SetQuantityInCart(Mathf.Max(0, value));
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0002F421 File Offset: 0x0002D621
		private void OnClickedIncrementQuantity(ISleekElement button)
		{
			this.SetQuantityInCart(this.cartEntry.quantity + 1);
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0002F436 File Offset: 0x0002D636
		private void OnClickedDecrementQuantity(ISleekElement button)
		{
			this.SetQuantityInCart(this.cartEntry.quantity - 1);
		}

		// Token: 0x0400053A RID: 1338
		private ItemStore.CartEntry cartEntry;

		// Token: 0x0400053B RID: 1339
		private ItemStore.Listing listing;

		// Token: 0x0400053C RID: 1340
		private ISleekButton itemButton;

		// Token: 0x0400053D RID: 1341
		private SleekEconIcon iconImage;

		// Token: 0x0400053E RID: 1342
		private ISleekLabel nameLabel;

		// Token: 0x0400053F RID: 1343
		private SleekItemStorePriceBox priceBox;

		// Token: 0x04000540 RID: 1344
		private ISleekButton addToCartButton;

		// Token: 0x04000541 RID: 1345
		private ISleekButton removeFromCartButton;

		// Token: 0x04000542 RID: 1346
		private ISleekInt32Field quantityField;

		// Token: 0x04000543 RID: 1347
		private ISleekButton incrementQuantityButton;

		// Token: 0x04000544 RID: 1348
		private ISleekButton decrementQuantityButton;
	}
}
