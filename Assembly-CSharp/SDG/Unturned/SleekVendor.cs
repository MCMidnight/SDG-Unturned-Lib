using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000732 RID: 1842
	public class SleekVendor : SleekWrapper
	{
		// Token: 0x140000E3 RID: 227
		// (add) Token: 0x06003CA8 RID: 15528 RVA: 0x00120918 File Offset: 0x0011EB18
		// (remove) Token: 0x06003CA9 RID: 15529 RVA: 0x00120950 File Offset: 0x0011EB50
		public event ClickedButton onClickedButton;

		// Token: 0x06003CAA RID: 15530 RVA: 0x00120988 File Offset: 0x0011EB88
		protected string formatCost(uint value)
		{
			if (this.element.outerAsset.currency.isValid)
			{
				ItemCurrencyAsset itemCurrencyAsset = this.element.outerAsset.currency.Find();
				if (itemCurrencyAsset != null && !string.IsNullOrEmpty(itemCurrencyAsset.valueFormat))
				{
					return string.Format(itemCurrencyAsset.valueFormat, value);
				}
			}
			return value.ToString();
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x001209F4 File Offset: 0x0011EBF4
		public void updateAmount()
		{
			if (this.element == null || this.amountLabel == null)
			{
				return;
			}
			VendorBuying vendorBuying = this.element as VendorBuying;
			if (vendorBuying != null)
			{
				ushort num;
				byte b;
				vendorBuying.format(Player.player, out num, out b);
				this.button.IsClickable = (num >= (ushort)b);
				this.amountLabel.Text = PlayerNPCVendorUI.localization.format("Amount_Buy", num, b);
			}
			else
			{
				VendorSellingBase vendorSellingBase = this.element as VendorSellingBase;
				if (vendorSellingBase != null)
				{
					ushort num2;
					vendorSellingBase.format(Player.player, out num2);
					this.button.IsClickable = vendorSellingBase.canBuy(Player.player);
					this.amountLabel.Text = PlayerNPCVendorUI.localization.format("Amount_Sell", num2);
				}
			}
			this.amountLabel.TextColor = (this.button.IsClickable ? 3 : 6);
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x00120AE0 File Offset: 0x0011ECE0
		public SleekVendor(VendorElement newElement)
		{
			this.element = newElement;
			this.button = Glazier.Get().CreateButton();
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			this.button.OnClicked += new ClickedButton(this.onClickedInternalButton);
			base.AddChild(this.button);
			float num = 0f;
			base.SizeOffset_Y = 60f;
			if (this.element.hasIcon)
			{
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, this.element.id) as ItemAsset;
				if (itemAsset != null)
				{
					SleekItemIcon sleekItemIcon = new SleekItemIcon();
					sleekItemIcon.PositionOffset_X = 5f;
					sleekItemIcon.PositionOffset_Y = 5f;
					if (itemAsset.size_y == 1)
					{
						sleekItemIcon.SizeOffset_X = (float)(itemAsset.size_x * 100);
						sleekItemIcon.SizeOffset_Y = (float)(itemAsset.size_y * 100);
					}
					else
					{
						sleekItemIcon.SizeOffset_X = (float)(itemAsset.size_x * 50);
						sleekItemIcon.SizeOffset_Y = (float)(itemAsset.size_y * 50);
					}
					num = sleekItemIcon.PositionOffset_X + sleekItemIcon.SizeOffset_X;
					base.AddChild(sleekItemIcon);
					sleekItemIcon.Refresh(this.element.id, 100, itemAsset.getState(false), itemAsset, Mathf.RoundToInt(sleekItemIcon.SizeOffset_X), Mathf.RoundToInt(sleekItemIcon.SizeOffset_Y));
					base.SizeOffset_Y = sleekItemIcon.SizeOffset_Y + 10f;
				}
			}
			else
			{
				VendorSellingVehicle vendorSellingVehicle = this.element as VendorSellingVehicle;
				if (vendorSellingVehicle != null)
				{
					Color32? color = vendorSellingVehicle.paintColor;
					if (color == null)
					{
						VehicleRedirectorAsset vehicleRedirectorAsset = vendorSellingVehicle.FindAsset() as VehicleRedirectorAsset;
						if (vehicleRedirectorAsset != null)
						{
							color = vehicleRedirectorAsset.SpawnPaintColor;
						}
					}
					if (color != null)
					{
						ISleekImage sleekImage = Glazier.Get().CreateImage();
						sleekImage.PositionOffset_X = 10f;
						sleekImage.PositionOffset_Y = 10f;
						sleekImage.SizeOffset_X = 20f;
						sleekImage.SizeOffset_Y = 40f;
						sleekImage.Texture = GlazierResources.PixelTexture;
						sleekImage.TintColor = color.Value;
						base.AddChild(sleekImage);
						num = sleekImage.PositionOffset_X + sleekImage.SizeOffset_X;
					}
				}
			}
			string displayName = this.element.displayName;
			if (!string.IsNullOrEmpty(displayName))
			{
				ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
				sleekLabel.PositionOffset_X = num + 5f;
				sleekLabel.PositionOffset_Y = 5f;
				sleekLabel.SizeOffset_X = -num - 10f;
				sleekLabel.SizeOffset_Y = 30f;
				sleekLabel.SizeScale_X = 1f;
				sleekLabel.Text = displayName;
				sleekLabel.FontSize = 3;
				sleekLabel.TextAlignment = 0;
				sleekLabel.TextColor = ItemTool.getRarityColorUI(this.element.rarity);
				sleekLabel.TextContrastContext = 1;
				base.AddChild(sleekLabel);
			}
			string displayDesc = this.element.displayDesc;
			if (!string.IsNullOrEmpty(displayDesc))
			{
				ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
				sleekLabel2.PositionOffset_X = num + 5f;
				sleekLabel2.PositionOffset_Y = 25f;
				sleekLabel2.SizeOffset_X = -num - 10f;
				sleekLabel2.SizeOffset_Y = -30f;
				sleekLabel2.SizeScale_X = 1f;
				sleekLabel2.SizeScale_Y = 1f;
				sleekLabel2.TextAlignment = 0;
				sleekLabel2.TextColor = 4;
				sleekLabel2.AllowRichText = true;
				sleekLabel2.TextContrastContext = 1;
				sleekLabel2.Text = displayDesc;
				base.AddChild(sleekLabel2);
			}
			ISleekLabel sleekLabel3 = Glazier.Get().CreateLabel();
			sleekLabel3.PositionOffset_X = num + 5f;
			sleekLabel3.PositionOffset_Y = -35f;
			sleekLabel3.PositionScale_Y = 1f;
			sleekLabel3.SizeOffset_X = -num - 10f;
			sleekLabel3.SizeOffset_Y = 30f;
			sleekLabel3.SizeScale_X = 1f;
			sleekLabel3.TextAlignment = 8;
			base.AddChild(sleekLabel3);
			if (this.element is VendorBuying)
			{
				sleekLabel3.Text = PlayerNPCVendorUI.localization.format("Price", this.formatCost(this.element.cost));
			}
			else
			{
				sleekLabel3.Text = PlayerNPCVendorUI.localization.format("Cost", this.formatCost(this.element.cost));
			}
			this.amountLabel = Glazier.Get().CreateLabel();
			this.amountLabel.PositionOffset_X = num + 5f;
			this.amountLabel.PositionOffset_Y = -35f;
			this.amountLabel.PositionScale_Y = 1f;
			this.amountLabel.SizeOffset_X = -num - 10f;
			this.amountLabel.SizeOffset_Y = 30f;
			this.amountLabel.SizeScale_X = 1f;
			this.amountLabel.TextAlignment = 6;
			base.AddChild(this.amountLabel);
			this.updateAmount();
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x00120FC3 File Offset: 0x0011F1C3
		private void onClickedInternalButton(ISleekElement internalButton)
		{
			ClickedButton clickedButton = this.onClickedButton;
			if (clickedButton == null)
			{
				return;
			}
			clickedButton.Invoke(this);
		}

		// Token: 0x040025FC RID: 9724
		private VendorElement element;

		// Token: 0x040025FD RID: 9725
		private ISleekButton button;

		// Token: 0x040025FE RID: 9726
		private ISleekLabel amountLabel;
	}
}
