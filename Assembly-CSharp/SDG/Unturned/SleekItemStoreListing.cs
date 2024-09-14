using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001B0 RID: 432
	internal class SleekItemStoreListing : SleekWrapper
	{
		// Token: 0x06000DB4 RID: 3508 RVA: 0x0002F44C File Offset: 0x0002D64C
		public SleekItemStoreListing()
		{
			this.button = Glazier.Get().CreateButton();
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			this.button.OnClicked += new ClickedButton(this.OnClickedButton);
			base.AddChild(this.button);
			ISleekConstraintFrame sleekConstraintFrame = Glazier.Get().CreateConstraintFrame();
			sleekConstraintFrame.PositionOffset_X = 5f;
			sleekConstraintFrame.PositionOffset_Y = 5f;
			sleekConstraintFrame.SizeScale_X = 1f;
			sleekConstraintFrame.SizeScale_Y = 1f;
			sleekConstraintFrame.SizeOffset_X = -10f;
			sleekConstraintFrame.SizeOffset_Y = -50f;
			sleekConstraintFrame.Constraint = 1;
			base.AddChild(sleekConstraintFrame);
			this.iconImage = new SleekEconIcon();
			this.iconImage.SizeScale_X = 1f;
			this.iconImage.SizeScale_Y = 1f;
			sleekConstraintFrame.AddChild(this.iconImage);
			this.nameAndPriceLabel = Glazier.Get().CreateLabel();
			this.nameAndPriceLabel.PositionScale_Y = 1f;
			this.nameAndPriceLabel.PositionOffset_Y = -50f;
			this.nameAndPriceLabel.SizeScale_X = 1f;
			this.nameAndPriceLabel.SizeOffset_Y = 50f;
			this.nameAndPriceLabel.TextContrastContext = 1;
			this.nameAndPriceLabel.TextAlignment = 6;
			this.nameAndPriceLabel.AllowRichText = true;
			base.AddChild(this.nameAndPriceLabel);
			this.cartImage = Glazier.Get().CreateSprite(ItemStoreMenu.instance.icons.load<Sprite>("Cart"));
			this.cartImage.PositionOffset_X = 5f;
			this.cartImage.PositionOffset_Y = 5f;
			this.cartImage.SizeOffset_X = 20f;
			this.cartImage.SizeOffset_Y = 20f;
			this.cartImage.DrawMethod = 2;
			this.cartImage.TintColor = 2;
			base.AddChild(this.cartImage);
			this.stampLabel = Glazier.Get().CreateLabel();
			this.stampLabel.SizeScale_X = 1f;
			this.stampLabel.SizeOffset_Y = 50f;
			this.stampLabel.TextContrastContext = 1;
			this.stampLabel.TextAlignment = 2;
			this.stampLabel.TextColor = Color.green;
			base.AddChild(this.stampLabel);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0002F6BA File Offset: 0x0002D8BA
		public void RefreshInCart()
		{
			this.cartImage.IsVisible = (this.button.IsClickable && ItemStore.Get().GetQuantityInCart(this.listing.itemdefid) > 0);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0002F6F0 File Offset: 0x0002D8F0
		public void SetListing(ItemStore.Listing listing)
		{
			this.button.IsClickable = true;
			this.iconImage.IsVisible = true;
			this.nameAndPriceLabel.IsVisible = true;
			this.listing = listing;
			Color inventoryColor = Provider.provider.economyService.getInventoryColor(listing.itemdefid);
			string inventoryName = Provider.provider.economyService.getInventoryName(listing.itemdefid);
			string text = ItemStore.Get().FormatPrice(listing.currentPrice);
			this.nameAndPriceLabel.Text = RichTextUtil.wrapWithColor(inventoryName, inventoryColor) + "\n" + RichTextUtil.wrapWithColor(text, ItemStore.PremiumColor);
			this.nameAndPriceLabel.TextColor = inventoryColor;
			this.button.BackgroundColor = SleekColor.BackgroundIfLight(inventoryColor);
			this.button.TextColor = inventoryColor;
			this.button.TooltipText = Provider.provider.economyService.getInventoryType(listing.itemdefid);
			this.iconImage.SetItemDefId(listing.itemdefid);
			if (listing.isNew && !ItemStoreSavedata.WasNewListingSeen(listing.itemdefid))
			{
				this.hasNewLabel = true;
				this.stampLabel.Text = Provider.localization.format("New");
				this.stampLabel.IsVisible = true;
			}
			else if (listing.currentPrice < listing.basePrice)
			{
				this.hasNewLabel = false;
				this.stampLabel.Text = MenuSurvivorsClothingUI.localization.format("Itemstore_Sale") + "\n" + ItemStore.Get().FormatDiscount(listing.currentPrice, listing.basePrice);
				this.stampLabel.IsVisible = true;
			}
			else
			{
				this.hasNewLabel = false;
				this.stampLabel.IsVisible = false;
			}
			this.RefreshInCart();
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0002F8AC File Offset: 0x0002DAAC
		public void ClearListing()
		{
			this.button.IsClickable = false;
			this.iconImage.IsVisible = false;
			this.nameAndPriceLabel.IsVisible = false;
			this.cartImage.IsVisible = false;
			this.button.TooltipText = null;
			this.stampLabel.IsVisible = false;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0002F901 File Offset: 0x0002DB01
		private void OnClickedButton(ISleekElement button)
		{
			if (this.hasNewLabel)
			{
				ItemStoreSavedata.MarkNewListingSeen(this.listing.itemdefid);
				this.stampLabel.IsVisible = false;
			}
			ItemStoreDetailsMenu.instance.Open(this.listing);
			ItemStoreMenu.instance.Close();
		}

		// Token: 0x04000545 RID: 1349
		private ItemStore.Listing listing;

		// Token: 0x04000546 RID: 1350
		private bool hasNewLabel;

		// Token: 0x04000547 RID: 1351
		private ISleekButton button;

		// Token: 0x04000548 RID: 1352
		private SleekEconIcon iconImage;

		// Token: 0x04000549 RID: 1353
		private ISleekLabel nameAndPriceLabel;

		/// <summary>
		/// Icon visible when this listing is in the cart.
		/// </summary>
		// Token: 0x0400054A RID: 1354
		private ISleekSprite cartImage;

		/// <summary>
		/// "SALE" or "NEW" text visible when applicable.
		/// </summary>
		// Token: 0x0400054B RID: 1355
		private ISleekLabel stampLabel;
	}
}
