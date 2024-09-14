using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020001B2 RID: 434
	internal class SleekItemStorePriceBox : SleekWrapper
	{
		// Token: 0x06000DBB RID: 3515 RVA: 0x0002FBEC File Offset: 0x0002DDEC
		public void SetPrice(ulong basePrice, ulong currentPrice, int quantity)
		{
			uint num = (uint)Mathf.Max(quantity, 1);
			if (currentPrice == basePrice)
			{
				this.basePriceLabel.IsVisible = false;
				this.discountStrikethrough.IsVisible = false;
				this.percentageLabel.IsVisible = false;
				this.currentPriceLabel.PositionScale_X = 0f;
				this.currentPriceLabel.PositionScale_Y = 0f;
				this.currentPriceLabel.SizeScale_X = 1f;
				this.currentPriceLabel.SizeScale_Y = 1f;
				this.currentPriceLabel.Text = ItemStore.Get().FormatPrice(currentPrice * (ulong)num);
				if (quantity > 1)
				{
					this.backdropBox.TooltipText = string.Format("{0} x {1} = {2}", ItemStore.Get().FormatPrice(currentPrice), quantity, this.currentPriceLabel.Text);
					return;
				}
				this.backdropBox.TooltipText = this.currentPriceLabel.Text;
				return;
			}
			else
			{
				this.basePriceLabel.IsVisible = true;
				this.discountStrikethrough.IsVisible = true;
				this.percentageLabel.IsVisible = true;
				this.currentPriceLabel.PositionScale_X = 0.5f;
				this.currentPriceLabel.PositionScale_Y = 0.5f;
				this.currentPriceLabel.SizeScale_X = 0.5f;
				this.currentPriceLabel.SizeScale_Y = 0.5f;
				ulong num2 = basePrice * (ulong)num;
				ulong num3 = currentPrice * (ulong)num;
				this.basePriceLabel.Text = ItemStore.Get().FormatPrice(num2);
				this.currentPriceLabel.Text = ItemStore.Get().FormatPrice(num3);
				this.percentageLabel.Text = ItemStore.Get().FormatDiscount(num3, num2);
				if (quantity > 1)
				{
					string text = string.Format("{0} x {1} = {2}", ItemStore.Get().FormatPrice(basePrice), quantity, this.basePriceLabel.Text);
					string text2 = string.Format("{0} x {1} = {2}", ItemStore.Get().FormatPrice(currentPrice), quantity, this.currentPriceLabel.Text);
					this.backdropBox.TooltipText = string.Concat(new string[]
					{
						RichTextUtil.wrapWithColor(text, Color.gray),
						"\n",
						RichTextUtil.wrapWithColor(this.percentageLabel.Text, Color.green),
						"\n",
						RichTextUtil.wrapWithColor(text2, ItemStore.PremiumColor)
					});
					return;
				}
				this.backdropBox.TooltipText = string.Concat(new string[]
				{
					RichTextUtil.wrapWithColor(this.basePriceLabel.Text, Color.gray),
					"\n",
					RichTextUtil.wrapWithColor(this.percentageLabel.Text, Color.green),
					"\n",
					RichTextUtil.wrapWithColor(this.currentPriceLabel.Text, ItemStore.PremiumColor)
				});
				return;
			}
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0002FEA4 File Offset: 0x0002E0A4
		public SleekItemStorePriceBox()
		{
			this.backdropBox = Glazier.Get().CreateBox();
			this.backdropBox.SizeScale_X = 1f;
			this.backdropBox.SizeScale_Y = 1f;
			this.backdropBox.TextColor = ItemStore.PremiumColor;
			base.AddChild(this.backdropBox);
			this.basePriceLabel = Glazier.Get().CreateLabel();
			this.basePriceLabel.PositionScale_X = 0.5f;
			this.basePriceLabel.SizeScale_X = 0.5f;
			this.basePriceLabel.SizeScale_Y = 0.5f;
			this.basePriceLabel.FontSize = 3;
			this.basePriceLabel.TextColor = Color.gray;
			base.AddChild(this.basePriceLabel);
			this.discountStrikethrough = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
			this.discountStrikethrough.PositionScale_X = 0.5f;
			this.discountStrikethrough.PositionScale_Y = 0.25f;
			this.discountStrikethrough.PositionOffset_Y = -1f;
			this.discountStrikethrough.SizeOffset_Y = 1f;
			this.discountStrikethrough.SizeScale_X = 0.5f;
			this.discountStrikethrough.CanRotate = true;
			this.discountStrikethrough.RotationAngle = -15f;
			this.discountStrikethrough.TintColor = Palette.COLOR_R;
			base.AddChild(this.discountStrikethrough);
			this.currentPriceLabel = Glazier.Get().CreateLabel();
			this.currentPriceLabel.SizeScale_X = 1f;
			this.currentPriceLabel.FontSize = 3;
			this.currentPriceLabel.TextColor = ItemStore.PremiumColor;
			base.AddChild(this.currentPriceLabel);
			this.percentageLabel = Glazier.Get().CreateLabel();
			this.percentageLabel.SizeScale_X = 0.5f;
			this.percentageLabel.SizeScale_Y = 1f;
			this.percentageLabel.FontSize = 3;
			this.percentageLabel.TextColor = Color.green;
			base.AddChild(this.percentageLabel);
		}

		// Token: 0x0400054E RID: 1358
		private ISleekBox backdropBox;

		// Token: 0x0400054F RID: 1359
		private ISleekLabel basePriceLabel;

		// Token: 0x04000550 RID: 1360
		private ISleekLabel currentPriceLabel;

		// Token: 0x04000551 RID: 1361
		private ISleekImage discountStrikethrough;

		// Token: 0x04000552 RID: 1362
		private ISleekLabel percentageLabel;
	}
}
