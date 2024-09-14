using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200071A RID: 1818
	public class SleekItem : SleekWrapper
	{
		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06003C05 RID: 15365 RVA: 0x0011AA10 File Offset: 0x00118C10
		public ItemJar jar
		{
			get
			{
				return this._jar;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06003C06 RID: 15366 RVA: 0x0011AA18 File Offset: 0x00118C18
		public int hotkey
		{
			get
			{
				return (int)this._hotkey;
			}
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x0011AA20 File Offset: 0x00118C20
		public void enable()
		{
			this.button.IsRaycastTarget = true;
			SleekColor backgroundColor = this.button.BackgroundColor;
			backgroundColor.SetAlpha(1f);
			this.button.BackgroundColor = backgroundColor;
			SleekColor color = this.icon.color;
			color.SetAlpha(1f);
			this.icon.color = color;
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x0011AA84 File Offset: 0x00118C84
		public void disable()
		{
			this.button.IsRaycastTarget = false;
			SleekColor backgroundColor = this.button.BackgroundColor;
			backgroundColor.SetAlpha(0.5f);
			this.button.BackgroundColor = backgroundColor;
			SleekColor color = this.icon.color;
			color.SetAlpha(0.5f);
			this.icon.color = color;
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x0011AAE5 File Offset: 0x00118CE5
		public void setEnabled(bool enabled)
		{
			if (enabled)
			{
				this.enable();
				return;
			}
			this.disable();
		}

		/// <summary>
		/// Set this item as the dragging preview.
		/// </summary>
		// Token: 0x06003C0A RID: 15370 RVA: 0x0011AAF7 File Offset: 0x00118CF7
		public void SetIsDragItem()
		{
			this.button.IsRaycastTarget = false;
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x0011AB08 File Offset: 0x00118D08
		public void updateHotkey(byte index)
		{
			this._hotkey = index;
			if (this.hotkey == 255)
			{
				this.hotkeyLabel.Text = "";
				this.hotkeyLabel.IsVisible = false;
				return;
			}
			this.hotkeyLabel.Text = ControlsSettings.getEquipmentHotkeyText(this.hotkey);
			this.hotkeyLabel.IsVisible = true;
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x0011AB68 File Offset: 0x00118D68
		public void updateItem(ItemJar newJar)
		{
			if (this._jar != null && this._jar.item != null && this._jar.item.id != newJar.item.id)
			{
				this.icon.Clear();
			}
			this._jar = newJar;
			ItemAsset asset = this.jar.GetAsset();
			if (asset != null)
			{
				if (!this.isTemporary)
				{
					this.button.TooltipText = asset.itemName;
				}
				if (this.jar.rot % 2 == 0)
				{
					base.SizeOffset_X = (float)(asset.size_x * 50);
					base.SizeOffset_Y = (float)(asset.size_y * 50);
					this.icon.PositionOffset_X = 0f;
					this.icon.PositionOffset_Y = 0f;
				}
				else
				{
					base.SizeOffset_X = (float)(asset.size_y * 50);
					base.SizeOffset_Y = (float)(asset.size_x * 50);
					int num = Mathf.Abs((int)(asset.size_y - asset.size_x));
					if (asset.size_x > asset.size_y)
					{
						this.icon.PositionOffset_X = (float)(-(float)num * 25);
						this.icon.PositionOffset_Y = (float)(num * 25);
					}
					else
					{
						this.icon.PositionOffset_X = (float)(num * 25);
						this.icon.PositionOffset_Y = (float)(-(float)num * 25);
					}
				}
				this.icon.rot = this.jar.rot;
				this.icon.SizeOffset_X = (float)(asset.size_x * 50);
				this.icon.SizeOffset_Y = (float)(asset.size_y * 50);
				this.icon.Refresh(this.jar.item.id, this.jar.item.quality, this.jar.item.state, asset);
				if (asset.size_x == 1 || asset.size_y == 1)
				{
					this.amountLabel.PositionOffset_X = 0f;
					this.amountLabel.PositionOffset_Y = -30f;
					this.amountLabel.SizeOffset_X = 0f;
					this.amountLabel.FontSize = 1;
					this.hotkeyLabel.FontSize = 1;
				}
				else
				{
					this.amountLabel.PositionOffset_X = 5f;
					this.amountLabel.PositionOffset_Y = -35f;
					this.amountLabel.SizeOffset_X = -10f;
					this.amountLabel.FontSize = 2;
					this.hotkeyLabel.FontSize = 2;
				}
				Color rarityColorUI = ItemTool.getRarityColorUI(asset.rarity);
				this.button.BackgroundColor = SleekColor.BackgroundIfLight(rarityColorUI);
				this.button.TextColor = rarityColorUI;
				if (asset.showQuality)
				{
					if (asset.size_x == 1 || asset.size_y == 1)
					{
						this.qualityImage.PositionOffset_X = -15f;
						this.qualityImage.PositionOffset_Y = -15f;
						this.qualityImage.SizeOffset_X = 10f;
						this.qualityImage.SizeOffset_Y = 10f;
						this.qualityImage.Texture = PlayerDashboardInventoryUI.icons.load<Texture2D>("Quality_1");
					}
					else
					{
						this.qualityImage.PositionOffset_X = -30f;
						this.qualityImage.PositionOffset_Y = -30f;
						this.qualityImage.SizeOffset_X = 20f;
						this.qualityImage.SizeOffset_Y = 20f;
						this.qualityImage.Texture = PlayerDashboardInventoryUI.icons.load<Texture2D>("Quality_0");
					}
					this.qualityImage.TintColor = ItemTool.getQualityColor((float)this.jar.item.quality / 100f);
					this.amountLabel.Text = this.jar.item.quality.ToString() + "%";
					this.amountLabel.TextColor = this.qualityImage.TintColor;
					this.qualityImage.IsVisible = true;
					this.amountLabel.IsVisible = true;
					return;
				}
				this.qualityImage.IsVisible = false;
				if (asset.amount > 1)
				{
					this.amountLabel.Text = "x" + this.jar.item.amount.ToString();
					this.amountLabel.TextColor = 3;
					this.amountLabel.IsVisible = true;
					return;
				}
				this.amountLabel.IsVisible = false;
			}
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x0011AFCC File Offset: 0x001191CC
		private void onClickedButton(ISleekElement button)
		{
			DraggedItem draggedItem = this.onDraggedItem;
			if (draggedItem == null)
			{
				return;
			}
			draggedItem(this);
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x0011AFDF File Offset: 0x001191DF
		private void onRightClickedButton(ISleekElement button)
		{
			ClickedItem clickedItem = this.onClickedItem;
			if (clickedItem == null)
			{
				return;
			}
			clickedItem(this);
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x0011AFF4 File Offset: 0x001191F4
		public SleekItem(ItemJar jar)
		{
			this.button = Glazier.Get().CreateButton();
			this.button.PositionOffset_X = 1f;
			this.button.PositionOffset_Y = 1f;
			this.button.SizeOffset_X = -2f;
			this.button.SizeOffset_Y = -2f;
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			this.button.OnClicked += new ClickedButton(this.onClickedButton);
			this.button.OnRightClicked += new ClickedButton(this.onRightClickedButton);
			base.AddChild(this.button);
			this.icon = new SleekItemIcon();
			base.AddChild(this.icon);
			this.icon.isAngled = true;
			this.amountLabel = Glazier.Get().CreateLabel();
			this.amountLabel.PositionScale_Y = 1f;
			this.amountLabel.SizeOffset_Y = 30f;
			this.amountLabel.SizeScale_X = 1f;
			this.amountLabel.TextAlignment = 6;
			this.amountLabel.TextContrastContext = 1;
			base.AddChild(this.amountLabel);
			this.amountLabel.IsVisible = false;
			this.qualityImage = Glazier.Get().CreateImage();
			this.qualityImage.PositionScale_X = 1f;
			this.qualityImage.PositionScale_Y = 1f;
			base.AddChild(this.qualityImage);
			this.qualityImage.IsVisible = false;
			this.hotkeyLabel = Glazier.Get().CreateLabel();
			this.hotkeyLabel.PositionOffset_X = 5f;
			this.hotkeyLabel.PositionOffset_Y = 5f;
			this.hotkeyLabel.SizeOffset_X = -10f;
			this.hotkeyLabel.SizeOffset_Y = 30f;
			this.hotkeyLabel.SizeScale_X = 1f;
			this.hotkeyLabel.TextAlignment = 2;
			this.hotkeyLabel.TextContrastContext = 1;
			base.AddChild(this.hotkeyLabel);
			this.hotkeyLabel.IsVisible = false;
			this.updateItem(jar);
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x0011B230 File Offset: 0x00119430
		public SleekItem()
		{
			this.button = Glazier.Get().CreateButton();
			this.button.PositionOffset_X = 1f;
			this.button.PositionOffset_Y = 1f;
			this.button.SizeOffset_X = -2f;
			this.button.SizeOffset_Y = -2f;
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			base.AddChild(this.button);
			this.icon = new SleekItemIcon();
			base.AddChild(this.icon);
			this.icon.isAngled = true;
			this.amountLabel = Glazier.Get().CreateLabel();
			this.amountLabel.PositionScale_Y = 1f;
			this.amountLabel.SizeOffset_Y = 30f;
			this.amountLabel.SizeScale_X = 1f;
			this.amountLabel.TextAlignment = 6;
			this.amountLabel.TextContrastContext = 1;
			base.AddChild(this.amountLabel);
			this.amountLabel.IsVisible = false;
			this.qualityImage = Glazier.Get().CreateImage();
			this.qualityImage.PositionScale_X = 1f;
			this.qualityImage.PositionScale_Y = 1f;
			base.AddChild(this.qualityImage);
			this.qualityImage.IsVisible = false;
			this.hotkeyLabel = Glazier.Get().CreateLabel();
			this.hotkeyLabel.PositionOffset_X = 5f;
			this.hotkeyLabel.PositionOffset_Y = 5f;
			this.hotkeyLabel.SizeOffset_X = -10f;
			this.hotkeyLabel.SizeOffset_Y = 30f;
			this.hotkeyLabel.SizeScale_X = 1f;
			this.hotkeyLabel.TextAlignment = 2;
			this.hotkeyLabel.TextContrastContext = 1;
			base.AddChild(this.hotkeyLabel);
			this.hotkeyLabel.IsVisible = false;
			this.isTemporary = true;
		}

		// Token: 0x04002592 RID: 9618
		private ItemJar _jar;

		// Token: 0x04002593 RID: 9619
		private byte _hotkey = byte.MaxValue;

		// Token: 0x04002594 RID: 9620
		private ISleekButton button;

		// Token: 0x04002595 RID: 9621
		private SleekItemIcon icon;

		// Token: 0x04002596 RID: 9622
		private ISleekLabel amountLabel;

		// Token: 0x04002597 RID: 9623
		private ISleekImage qualityImage;

		// Token: 0x04002598 RID: 9624
		private ISleekLabel hotkeyLabel;

		// Token: 0x04002599 RID: 9625
		public ClickedItem onClickedItem;

		// Token: 0x0400259A RID: 9626
		public DraggedItem onDraggedItem;

		// Token: 0x0400259B RID: 9627
		private bool isTemporary;
	}
}
