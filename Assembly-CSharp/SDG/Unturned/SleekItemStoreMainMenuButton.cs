using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Displays a single random item. Placed under the other main menu buttons.
	/// </summary>
	// Token: 0x020001B1 RID: 433
	internal class SleekItemStoreMainMenuButton : SleekWrapper
	{
		// Token: 0x06000DB9 RID: 3513 RVA: 0x0002F944 File Offset: 0x0002DB44
		public SleekItemStoreMainMenuButton(ItemStore.Listing listing, SleekItemStoreMainMenuButton.ELabelType labelType)
		{
			this.listing = listing;
			Color inventoryColor = Provider.provider.economyService.getInventoryColor(listing.itemdefid);
			ISleekButton sleekButton = Glazier.Get().CreateButton();
			sleekButton.SizeScale_X = 1f;
			sleekButton.SizeScale_Y = 1f;
			sleekButton.OnClicked += new ClickedButton(this.OnClickedItemButton);
			sleekButton.TextColor = inventoryColor;
			sleekButton.TooltipText = Provider.provider.economyService.getInventoryType(listing.itemdefid);
			sleekButton.BackgroundColor = SleekColor.BackgroundIfLight(inventoryColor);
			base.AddChild(sleekButton);
			SleekEconIcon sleekEconIcon = new SleekEconIcon();
			sleekEconIcon.PositionOffset_X = 5f;
			sleekEconIcon.PositionOffset_Y = 5f;
			sleekEconIcon.SizeOffset_X = 40f;
			sleekEconIcon.SizeOffset_Y = 40f;
			sleekEconIcon.SetItemDefId(listing.itemdefid);
			sleekButton.AddChild(sleekEconIcon);
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_X = 50f;
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.SizeScale_Y = 1f;
			sleekLabel.SizeOffset_X = -50f;
			sleekLabel.TextAlignment = 3;
			sleekLabel.FontSize = 3;
			sleekLabel.Text = Provider.provider.economyService.getInventoryName(listing.itemdefid);
			sleekLabel.TextColor = inventoryColor;
			sleekLabel.TextContrastContext = 1;
			sleekButton.AddChild(sleekLabel);
			ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
			sleekLabel2.SizeScale_X = 1f;
			sleekLabel2.SizeScale_Y = 1f;
			sleekLabel2.TextAlignment = 8;
			sleekLabel2.TextColor = ItemStore.PremiumColor;
			sleekLabel2.Text = ItemStore.Get().FormatPrice(listing.currentPrice);
			sleekLabel2.TextContrastContext = 1;
			sleekButton.AddChild(sleekLabel2);
			if (labelType != SleekItemStoreMainMenuButton.ELabelType.None)
			{
				ISleekLabel sleekLabel3 = Glazier.Get().CreateLabel();
				sleekLabel3.SizeScale_X = 1f;
				sleekLabel3.SizeScale_Y = 1f;
				sleekLabel3.TextAlignment = 2;
				sleekLabel3.TextColor = Color.green;
				sleekLabel3.TextContrastContext = 1;
				sleekButton.AddChild(sleekLabel3);
				if (labelType == SleekItemStoreMainMenuButton.ELabelType.New)
				{
					this.hasNewLabel = true;
					sleekLabel3.Text = Provider.localization.format("New");
					return;
				}
				if (labelType != SleekItemStoreMainMenuButton.ELabelType.Sale)
				{
					return;
				}
				sleekLabel3.Text = MenuSurvivorsClothingUI.localization.format("Itemstore_Sale") + "\n" + ItemStore.Get().FormatDiscount(listing.currentPrice, listing.basePrice);
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0002FBB4 File Offset: 0x0002DDB4
		private void OnClickedItemButton(ISleekElement button)
		{
			if (this.hasNewLabel)
			{
				ItemStoreSavedata.MarkNewListingSeen(this.listing.itemdefid);
			}
			ItemStore.Get().ViewItem(this.listing.itemdefid);
			base.IsVisible = false;
		}

		// Token: 0x0400054C RID: 1356
		private ItemStore.Listing listing;

		// Token: 0x0400054D RID: 1357
		private bool hasNewLabel;

		// Token: 0x02000883 RID: 2179
		public enum ELabelType
		{
			// Token: 0x040031AE RID: 12718
			None,
			// Token: 0x040031AF RID: 12719
			New,
			// Token: 0x040031B0 RID: 12720
			Sale
		}
	}
}
