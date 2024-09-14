using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001AC RID: 428
	internal class ItemStoreCartMenu : SleekFullscreenBox
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x0002CDE0 File Offset: 0x0002AFE0
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x0002CDE8 File Offset: 0x0002AFE8
		public bool IsOpen { get; private set; }

		// Token: 0x06000D74 RID: 3444 RVA: 0x0002CDF1 File Offset: 0x0002AFF1
		public void Open()
		{
			this.IsOpen = true;
			base.AnimateIntoView();
			this.RefreshCartEntries();
			ItemStore.Get().OnCartChanged += new Action(this.OnCartChanged);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0002CE1C File Offset: 0x0002B01C
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			base.AnimateOutOfView(0f, 1f);
			ItemStore.Get().OnCartChanged -= new Action(this.OnCartChanged);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0002CE54 File Offset: 0x0002B054
		public override void OnDestroy()
		{
			ItemStore.Get().OnCartChanged -= new Action(this.OnCartChanged);
			base.OnDestroy();
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0002CE74 File Offset: 0x0002B074
		public ItemStoreCartMenu()
		{
			ItemStoreCartMenu.instance = this;
			Local localization = ItemStoreMenu.instance.localization;
			base.PositionScale_Y = 1f;
			base.PositionOffset_X = 10f;
			base.PositionOffset_Y = 10f;
			base.SizeOffset_X = -20f;
			base.SizeOffset_Y = -20f;
			base.SizeScale_X = 1f;
			base.SizeScale_Y = 1f;
			ISleekElement sleekElement = Glazier.Get().CreateFrame();
			sleekElement.PositionScale_X = 0.5f;
			sleekElement.PositionOffset_Y = 10f;
			sleekElement.SizeScale_X = 0.5f;
			sleekElement.SizeScale_Y = 1f;
			sleekElement.SizeOffset_Y = -20f;
			base.AddChild(sleekElement);
			this.scrollView = Glazier.Get().CreateScrollView();
			this.scrollView.SizeScale_X = 1f;
			this.scrollView.SizeScale_Y = 1f;
			this.scrollView.SizeOffset_Y = -110f;
			this.scrollView.ScaleContentToWidth = true;
			this.scrollView.ReduceWidthWhenScrollbarVisible = false;
			sleekElement.AddChild(this.scrollView);
			ISleekElement sleekElement2 = Glazier.Get().CreateFrame();
			sleekElement2.SizeScale_X = 1f;
			sleekElement2.SizeOffset_X = -30f;
			sleekElement2.SizeOffset_Y = 105f;
			sleekElement2.PositionScale_Y = 1f;
			sleekElement2.PositionOffset_Y = -105f;
			sleekElement.AddChild(sleekElement2);
			this.totalPriceBox = new SleekItemStorePriceBox();
			this.totalPriceBox.PositionScale_X = 0.8f;
			this.totalPriceBox.SizeScale_X = 0.2f;
			this.totalPriceBox.SizeOffset_Y = 50f;
			sleekElement2.AddChild(this.totalPriceBox);
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_X = -5f;
			sleekLabel.SizeScale_X = 0.8f;
			sleekLabel.SizeOffset_X = -5f;
			sleekLabel.SizeOffset_Y = 50f;
			sleekLabel.FontSize = 3;
			sleekLabel.TextAlignment = 5;
			sleekLabel.Text = localization.format("TotalPrice_Label");
			sleekLabel.TextContrastContext = 2;
			sleekElement2.AddChild(sleekLabel);
			this.startPurchaseButton = Glazier.Get().CreateButton();
			this.startPurchaseButton.PositionOffset_Y = 55f;
			this.startPurchaseButton.SizeScale_X = 1f;
			this.startPurchaseButton.SizeOffset_Y = 50f;
			this.startPurchaseButton.FontSize = 3;
			this.startPurchaseButton.Text = localization.format("StartPurchase_Label");
			this.startPurchaseButton.TooltipText = localization.format("StartPurchase_Tooltip");
			this.startPurchaseButton.OnClicked += new ClickedButton(this.OnClickedStartPurchase);
			sleekElement2.AddChild(this.startPurchaseButton);
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

		// Token: 0x06000D78 RID: 3448 RVA: 0x0002D1D0 File Offset: 0x0002B3D0
		private void RefreshCartEntries()
		{
			this.scrollView.RemoveAllChildren();
			int num = 0;
			this.entries.Clear();
			foreach (ItemStore.CartEntry cartEntry in ItemStore.Get().GetCart())
			{
				ItemStore.Listing listing;
				if (!ItemStore.Get().FindListing(cartEntry.itemdefid, out listing))
				{
					UnturnedLog.warn("Item store itemdefid {0} x{1} in cart without listing", new object[]
					{
						cartEntry.itemdefid,
						cartEntry.quantity
					});
				}
				else
				{
					SleekItemStoreCartEntry sleekItemStoreCartEntry = new SleekItemStoreCartEntry(cartEntry, listing);
					sleekItemStoreCartEntry.SizeOffset_X = -30f;
					sleekItemStoreCartEntry.SizeScale_X = 1f;
					sleekItemStoreCartEntry.SizeOffset_Y = 50f;
					sleekItemStoreCartEntry.PositionOffset_Y = (float)num;
					num += 55;
					this.scrollView.AddChild(sleekItemStoreCartEntry);
					this.entries.Add(sleekItemStoreCartEntry);
				}
			}
			this.scrollView.ContentSizeOffset = new Vector2(0f, (float)(num - 5));
			this.OnCartChanged();
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0002D2F0 File Offset: 0x0002B4F0
		private void OnCartChanged()
		{
			ulong num = 0UL;
			ulong num2 = 0UL;
			foreach (SleekItemStoreCartEntry sleekItemStoreCartEntry in this.entries)
			{
				ulong num3;
				ulong num4;
				sleekItemStoreCartEntry.GetTotalPrice(out num3, out num4);
				num += num3;
				num2 += num4;
			}
			this.totalPriceBox.SetPrice(num, num2, 1);
			this.startPurchaseButton.IsClickable = !ItemStore.Get().IsCartEmpty;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0002D37C File Offset: 0x0002B57C
		private void OnClickedStartPurchase(ISleekElement button)
		{
			MenuSurvivorsClothingUI.open();
			this.Close();
			ItemStore.Get().StartPurchase();
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0002D393 File Offset: 0x0002B593
		private void OnClickedBackButton(ISleekElement button)
		{
			ItemStoreMenu.instance.Open();
			this.Close();
		}

		// Token: 0x04000516 RID: 1302
		public static ItemStoreCartMenu instance;

		// Token: 0x04000518 RID: 1304
		private ISleekScrollView scrollView;

		// Token: 0x04000519 RID: 1305
		private List<SleekItemStoreCartEntry> entries = new List<SleekItemStoreCartEntry>();

		// Token: 0x0400051A RID: 1306
		private SleekItemStorePriceBox totalPriceBox;

		// Token: 0x0400051B RID: 1307
		private ISleekButton startPurchaseButton;
	}
}
