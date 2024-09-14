using System;
using SDG.Provider;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000717 RID: 1815
	public class SleekInventory : SleekWrapper
	{
		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06003BF2 RID: 15346 RVA: 0x00119E36 File Offset: 0x00118036
		public ItemAsset itemAsset
		{
			get
			{
				return this._itemAsset;
			}
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x06003BF3 RID: 15347 RVA: 0x00119E3E File Offset: 0x0011803E
		public VehicleAsset vehicleAsset
		{
			get
			{
				return this._vehicleAsset;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06003BF4 RID: 15348 RVA: 0x00119E46 File Offset: 0x00118046
		// (set) Token: 0x06003BF5 RID: 15349 RVA: 0x00119E4E File Offset: 0x0011804E
		public ulong instance { get; protected set; }

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06003BF6 RID: 15350 RVA: 0x00119E57 File Offset: 0x00118057
		// (set) Token: 0x06003BF7 RID: 15351 RVA: 0x00119E5F File Offset: 0x0011805F
		public int item { get; protected set; }

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06003BF8 RID: 15352 RVA: 0x00119E68 File Offset: 0x00118068
		// (set) Token: 0x06003BF9 RID: 15353 RVA: 0x00119E70 File Offset: 0x00118070
		public ushort quantity { get; protected set; }

		// Token: 0x06003BFA RID: 15354 RVA: 0x00119E7C File Offset: 0x0011807C
		public void updateInventory(ulong newInstance, int newItem, ushort newQuantity, bool isClickable, bool isLarge)
		{
			this.instance = newInstance;
			this.item = newItem;
			this.quantity = newQuantity;
			this.button.IsClickable = isClickable;
			if (isLarge)
			{
				this.iconFrame.SizeOffset_Y = -70f;
				this.nameLabel.FontSize = 4;
				this.nameLabel.PositionOffset_Y = -70f;
				this.nameLabel.SizeOffset_Y = 70f;
				this.equippedIcon.SizeOffset_X = 20f;
				this.equippedIcon.SizeOffset_Y = 20f;
				this.statTrackerLabel.FontSize = 2;
				this.ragdollEffectLabel.FontSize = 2;
				this.particleEffectLabel.FontSize = 2;
			}
			else
			{
				this.iconFrame.SizeOffset_Y = -50f;
				this.nameLabel.FontSize = 2;
				this.nameLabel.PositionOffset_Y = -50f;
				this.nameLabel.SizeOffset_Y = 50f;
				this.equippedIcon.SizeOffset_X = 10f;
				this.equippedIcon.SizeOffset_Y = 10f;
				this.statTrackerLabel.FontSize = 0;
				this.ragdollEffectLabel.FontSize = 0;
				this.particleEffectLabel.FontSize = 0;
			}
			if (this.item != 0)
			{
				if (this.item < 0)
				{
					this._itemAsset = null;
					this._vehicleAsset = null;
					this.icon.SetIsBoxMythicalIcon();
					this.icon.IsVisible = true;
					this.nameLabel.Text = MenuSurvivorsClothingUI.localization.format("Mystery_" + this.item.ToString() + "_Text");
					this.button.TooltipText = MenuSurvivorsClothingUI.localization.format("Mystery_Tooltip");
					this.button.BackgroundColor = SleekColor.BackgroundIfLight(Palette.MYTHICAL);
					this.button.TextColor = Palette.MYTHICAL;
					this.nameLabel.TextColor = Palette.MYTHICAL;
					this.nameLabel.TextContrastContext = 1;
					this.equippedIcon.IsVisible = false;
				}
				else
				{
					Guid guid;
					Guid guid2;
					Provider.provider.economyService.getInventoryTargetID(this.item, out guid, out guid2);
					if (guid == default(Guid) && guid2 == default(Guid))
					{
						this._itemAsset = null;
						this._vehicleAsset = null;
						this.icon.SetItemDefId(-1);
						this.icon.IsVisible = false;
						this.nameLabel.Text = "itemdefid: " + this.item.ToString();
						this.button.TooltipText = "itemdefid: " + this.item.ToString();
						this.button.BackgroundColor = 1;
						this.button.TextColor = 3;
						this.nameLabel.TextColor = 3;
						this.nameLabel.TextContrastContext = 0;
						this.equippedIcon.IsVisible = false;
						this.statTrackerLabel.IsVisible = false;
						this.ragdollEffectLabel.IsVisible = false;
						this.particleEffectLabel.IsVisible = false;
					}
					else
					{
						this._itemAsset = Assets.find<ItemAsset>(guid);
						this._vehicleAsset = VehicleTool.FindVehicleByGuidAndHandleRedirects(guid2);
						this.icon.SetItemDefId(this.item);
						this.icon.IsVisible = true;
						this.nameLabel.Text = Provider.provider.economyService.getInventoryName(this.item);
						if (this.quantity > 1)
						{
							ISleekLabel sleekLabel = this.nameLabel;
							sleekLabel.Text = sleekLabel.Text + " x" + this.quantity.ToString();
						}
						this.button.TooltipText = Provider.provider.economyService.getInventoryType(this.item);
						Color inventoryColor = Provider.provider.economyService.getInventoryColor(this.item);
						this.button.BackgroundColor = SleekColor.BackgroundIfLight(inventoryColor);
						this.button.TextColor = inventoryColor;
						this.nameLabel.TextColor = inventoryColor;
						this.nameLabel.TextContrastContext = 1;
						bool isVisible;
						if (this.itemAsset == null || this.itemAsset.proPath == null || this.itemAsset.proPath.Length == 0)
						{
							isVisible = Characters.isSkinEquipped(this.instance);
						}
						else
						{
							isVisible = Characters.isCosmeticEquipped(this.instance);
						}
						this.equippedIcon.IsVisible = isVisible;
						if (this.equippedIcon.IsVisible && this.equippedIcon.Texture == null)
						{
							this.equippedIcon.Texture = MenuSurvivorsClothingUI.icons.load<Texture2D>("Equip");
						}
					}
				}
				this.nameLabel.IsVisible = true;
				EStatTrackerType type;
				int num;
				if (!Provider.provider.economyService.getInventoryStatTrackerValue(this.instance, out type, out num))
				{
					this.statTrackerLabel.IsVisible = false;
				}
				else
				{
					this.statTrackerLabel.IsVisible = true;
					this.statTrackerLabel.TextColor = Provider.provider.economyService.getStatTrackerColor(type);
					this.statTrackerLabel.Text = num.ToString("D7");
				}
				ERagdollEffect eragdollEffect;
				if (!Provider.provider.economyService.getInventoryRagdollEffect(this.instance, out eragdollEffect))
				{
					this.ragdollEffectLabel.IsVisible = false;
				}
				else
				{
					this.ragdollEffectLabel.IsVisible = true;
					switch (eragdollEffect)
					{
					case ERagdollEffect.ZERO_KELVIN:
						this.ragdollEffectLabel.TextColor = new Color(0f, 1f, 1f);
						this.ragdollEffectLabel.Text = "0 Kelvin";
						break;
					case ERagdollEffect.JADED:
						this.ragdollEffectLabel.TextColor = new Color32(76, 166, 90, byte.MaxValue);
						this.ragdollEffectLabel.Text = "Jaded";
						break;
					case ERagdollEffect.SOUL_CRYSTAL_GREEN:
						this.ragdollEffectLabel.TextColor = Palette.MYTHICAL;
						this.ragdollEffectLabel.Text = "Green Soul Crystal";
						break;
					case ERagdollEffect.SOUL_CRYSTAL_MAGENTA:
						this.ragdollEffectLabel.TextColor = Palette.MYTHICAL;
						this.ragdollEffectLabel.Text = "Magenta Soul Crystal";
						break;
					case ERagdollEffect.SOUL_CRYSTAL_RED:
						this.ragdollEffectLabel.TextColor = Palette.MYTHICAL;
						this.ragdollEffectLabel.Text = "Red Soul Crystal";
						break;
					case ERagdollEffect.SOUL_CRYSTAL_YELLOW:
						this.ragdollEffectLabel.TextColor = Palette.MYTHICAL;
						this.ragdollEffectLabel.Text = "Yellow Soul Crystal";
						break;
					default:
						this.ragdollEffectLabel.TextColor = Color.red;
						this.ragdollEffectLabel.Text = eragdollEffect.ToString();
						break;
					}
				}
				ushort num2 = Provider.provider.economyService.getInventoryMythicID(this.item);
				if (num2 == 0)
				{
					num2 = Provider.provider.economyService.getInventoryParticleEffect(this.instance);
				}
				if (num2 == 0)
				{
					this.particleEffectLabel.IsVisible = false;
				}
				else
				{
					this.particleEffectLabel.IsVisible = true;
					MythicAsset mythicAsset = Assets.find(EAssetType.MYTHIC, num2) as MythicAsset;
					if (mythicAsset != null)
					{
						this.particleEffectLabel.Text = mythicAsset.particleTagName;
					}
					else
					{
						this.particleEffectLabel.Text = num2.ToString();
					}
				}
				if (!string.IsNullOrEmpty(this.extraTooltip))
				{
					ISleekButton sleekButton = this.button;
					sleekButton.TooltipText = sleekButton.TooltipText + "\n" + this.extraTooltip;
					return;
				}
			}
			else
			{
				this._itemAsset = null;
				this.button.TooltipText = "";
				this.button.BackgroundColor = 1;
				this.button.TextColor = 3;
				this.icon.IsVisible = false;
				this.nameLabel.IsVisible = false;
				this.equippedIcon.IsVisible = false;
				this.statTrackerLabel.IsVisible = false;
				this.ragdollEffectLabel.IsVisible = false;
				this.particleEffectLabel.IsVisible = false;
			}
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x0011A69A File Offset: 0x0011889A
		private void onClickedButton(ISleekElement button)
		{
			ClickedInventory clickedInventory = this.onClickedInventory;
			if (clickedInventory == null)
			{
				return;
			}
			clickedInventory(this);
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x0011A6B0 File Offset: 0x001188B0
		public SleekInventory()
		{
			this.button = Glazier.Get().CreateButton();
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			this.button.OnClicked += new ClickedButton(this.onClickedButton);
			base.AddChild(this.button);
			this.button.IsClickable = false;
			this.iconFrame = Glazier.Get().CreateConstraintFrame();
			this.iconFrame.PositionOffset_X = 5f;
			this.iconFrame.PositionOffset_Y = 5f;
			this.iconFrame.SizeScale_X = 1f;
			this.iconFrame.SizeScale_Y = 1f;
			this.iconFrame.SizeOffset_X = -10f;
			this.iconFrame.Constraint = 1;
			base.AddChild(this.iconFrame);
			this.icon = new SleekEconIcon();
			this.icon.SizeScale_X = 1f;
			this.icon.SizeScale_Y = 1f;
			this.iconFrame.AddChild(this.icon);
			this.icon.IsVisible = false;
			this.equippedIcon = Glazier.Get().CreateImage();
			this.equippedIcon.PositionOffset_X = 5f;
			this.equippedIcon.PositionOffset_Y = 5f;
			this.equippedIcon.TintColor = 2;
			base.AddChild(this.equippedIcon);
			this.equippedIcon.IsVisible = false;
			this.ragdollEffectLabel = Glazier.Get().CreateLabel();
			this.ragdollEffectLabel.PositionOffset_Y = -30f;
			this.ragdollEffectLabel.PositionScale_Y = 1f;
			this.ragdollEffectLabel.SizeOffset_Y = 30f;
			this.ragdollEffectLabel.SizeScale_X = 1f;
			this.ragdollEffectLabel.TextAlignment = 8;
			this.ragdollEffectLabel.TextContrastContext = 1;
			this.ragdollEffectLabel.FontStyle = 2;
			base.AddChild(this.ragdollEffectLabel);
			this.ragdollEffectLabel.IsVisible = false;
			this.particleEffectLabel = Glazier.Get().CreateLabel();
			this.particleEffectLabel.SizeOffset_Y = 30f;
			this.particleEffectLabel.SizeScale_X = 1f;
			this.particleEffectLabel.TextColor = Palette.MYTHICAL;
			this.particleEffectLabel.TextAlignment = 2;
			this.particleEffectLabel.TextContrastContext = 1;
			base.AddChild(this.particleEffectLabel);
			this.particleEffectLabel.IsVisible = false;
			this.statTrackerLabel = Glazier.Get().CreateLabel();
			this.statTrackerLabel.PositionOffset_Y = -30f;
			this.statTrackerLabel.PositionScale_Y = 1f;
			this.statTrackerLabel.SizeOffset_Y = 30f;
			this.statTrackerLabel.SizeScale_X = 1f;
			this.statTrackerLabel.TextAlignment = 6;
			this.statTrackerLabel.FontStyle = 2;
			this.statTrackerLabel.TextContrastContext = 1;
			base.AddChild(this.statTrackerLabel);
			this.statTrackerLabel.IsVisible = false;
			this.nameLabel = Glazier.Get().CreateLabel();
			this.nameLabel.PositionScale_Y = 1f;
			this.nameLabel.SizeScale_X = 1f;
			base.AddChild(this.nameLabel);
			this.nameLabel.IsVisible = false;
		}

		// Token: 0x04002583 RID: 9603
		private ItemAsset _itemAsset;

		// Token: 0x04002584 RID: 9604
		private VehicleAsset _vehicleAsset;

		// Token: 0x04002585 RID: 9605
		private ISleekButton button;

		// Token: 0x04002586 RID: 9606
		private ISleekConstraintFrame iconFrame;

		// Token: 0x04002587 RID: 9607
		private SleekEconIcon icon;

		// Token: 0x04002588 RID: 9608
		private ISleekLabel nameLabel;

		// Token: 0x04002589 RID: 9609
		private ISleekImage equippedIcon;

		// Token: 0x0400258A RID: 9610
		private ISleekLabel statTrackerLabel;

		// Token: 0x0400258B RID: 9611
		private ISleekLabel ragdollEffectLabel;

		// Token: 0x0400258C RID: 9612
		private ISleekLabel particleEffectLabel;

		// Token: 0x0400258D RID: 9613
		public ClickedInventory onClickedInventory;

		/// <summary>
		/// Hack, we put this string on a newline for box probabilities.
		/// </summary>
		// Token: 0x04002591 RID: 9617
		public string extraTooltip;
	}
}
