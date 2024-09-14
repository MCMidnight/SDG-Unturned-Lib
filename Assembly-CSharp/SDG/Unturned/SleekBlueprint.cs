using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006FF RID: 1791
	public class SleekBlueprint : SleekWrapper
	{
		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06003B41 RID: 15169 RVA: 0x001151FF File Offset: 0x001133FF
		public Blueprint blueprint
		{
			get
			{
				return this._blueprint;
			}
		}

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06003B42 RID: 15170 RVA: 0x00115208 File Offset: 0x00113408
		// (remove) Token: 0x06003B43 RID: 15171 RVA: 0x00115240 File Offset: 0x00113440
		public event SleekBlueprint.Clicked onClickedCraftButton;

		// Token: 0x140000DE RID: 222
		// (add) Token: 0x06003B44 RID: 15172 RVA: 0x00115278 File Offset: 0x00113478
		// (remove) Token: 0x06003B45 RID: 15173 RVA: 0x001152B0 File Offset: 0x001134B0
		public event SleekBlueprint.Clicked onClickedCraftAllButton;

		// Token: 0x06003B46 RID: 15174 RVA: 0x001152E5 File Offset: 0x001134E5
		private void onToggledIgnoring(ISleekToggle toggle, bool toggleState)
		{
			Player.player.crafting.setIgnoringBlueprint(this.blueprint, !toggleState);
			this.refreshIgnoring();
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x00115308 File Offset: 0x00113508
		private void refreshIgnoring()
		{
			bool ignoringBlueprint = Player.player.crafting.getIgnoringBlueprint(this.blueprint);
			if (this.backgroundButton != null)
			{
				this.backgroundButton.IsClickable = (!ignoringBlueprint && this.isCraftable);
			}
			if (this.craftButton != null)
			{
				this.craftButton.IsClickable = !ignoringBlueprint;
				this.craftAllButton.IsClickable = this.craftButton.IsClickable;
			}
			this.ignoreToggleButton.Value = !ignoringBlueprint;
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x00115388 File Offset: 0x00113588
		public SleekBlueprint(Blueprint newBlueprint)
		{
			this._blueprint = newBlueprint;
			bool supportsDepth = Glazier.Get().SupportsDepth;
			if (supportsDepth)
			{
				this.backgroundButton = Glazier.Get().CreateButton();
				this.backgroundButton.SizeScale_X = 1f;
				this.backgroundButton.SizeScale_Y = 1f;
				this.backgroundButton.OnClicked += new ClickedButton(this.onClickedBackgroundButton);
				base.AddChild(this.backgroundButton);
			}
			else
			{
				ISleekBox sleekBox = Glazier.Get().CreateBox();
				sleekBox.SizeScale_X = 1f;
				sleekBox.SizeScale_Y = 1f;
				base.AddChild(sleekBox);
			}
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_X = 5f;
			sleekLabel.PositionOffset_Y = 5f;
			sleekLabel.SizeOffset_X = -10f;
			sleekLabel.SizeOffset_Y = 30f;
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.TextColor = (this.isCraftable ? 3 : 6);
			sleekLabel.TextContrastContext = (this.isCraftable ? 0 : 1);
			sleekLabel.FontSize = 3;
			base.AddChild(sleekLabel);
			if (this.blueprint.skill != EBlueprintSkill.NONE)
			{
				ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
				sleekLabel2.PositionOffset_X = 5f;
				sleekLabel2.PositionOffset_Y = -35f;
				sleekLabel2.PositionScale_Y = 1f;
				sleekLabel2.SizeOffset_X = -10f;
				sleekLabel2.SizeOffset_Y = 30f;
				sleekLabel2.SizeScale_X = 1f;
				sleekLabel2.Text = PlayerDashboardCraftingUI.localization.format("Skill_" + ((int)this.blueprint.skill).ToString(), PlayerDashboardSkillsUI.localization.format("Level_" + this.blueprint.level.ToString()));
				sleekLabel2.TextColor = (this.blueprint.hasSkills ? 3 : 6);
				sleekLabel2.TextContrastContext = (this.blueprint.hasSkills ? 0 : 1);
				sleekLabel2.FontSize = 3;
				base.AddChild(sleekLabel2);
			}
			this.container = Glazier.Get().CreateFrame();
			this.container.PositionOffset_Y = 40f;
			this.container.PositionScale_X = 0.5f;
			this.container.SizeOffset_Y = -45f;
			this.container.SizeScale_Y = 1f;
			base.AddChild(this.container);
			int num = 0;
			for (int i = 0; i < this.blueprint.supplies.Length; i++)
			{
				BlueprintSupply blueprintSupply = this.blueprint.supplies[i];
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, blueprintSupply.id) as ItemAsset;
				if (itemAsset != null)
				{
					ISleekLabel sleekLabel3 = sleekLabel;
					sleekLabel3.Text += itemAsset.itemName;
					SleekItemIcon sleekItemIcon = new SleekItemIcon();
					sleekItemIcon.PositionOffset_X = (float)num;
					sleekItemIcon.PositionOffset_Y = (float)(-itemAsset.size_y * 25);
					sleekItemIcon.PositionScale_Y = 0.5f;
					sleekItemIcon.SizeOffset_X = (float)(itemAsset.size_x * 50);
					sleekItemIcon.SizeOffset_Y = (float)(itemAsset.size_y * 50);
					this.container.AddChild(sleekItemIcon);
					sleekItemIcon.Refresh(blueprintSupply.id, 100, itemAsset.getState(false), itemAsset);
					ISleekLabel sleekLabel4 = Glazier.Get().CreateLabel();
					sleekLabel4.PositionOffset_X = -100f;
					sleekLabel4.PositionOffset_Y = -30f;
					sleekLabel4.PositionScale_Y = 1f;
					sleekLabel4.SizeOffset_X = 100f;
					sleekLabel4.SizeOffset_Y = 30f;
					sleekLabel4.SizeScale_X = 1f;
					sleekLabel4.TextAlignment = 5;
					sleekLabel4.Text = blueprintSupply.hasAmount.ToString() + "/" + blueprintSupply.amount.ToString();
					sleekLabel4.TextContrastContext = 1;
					sleekItemIcon.AddChild(sleekLabel4);
					ISleekLabel sleekLabel5 = sleekLabel;
					sleekLabel5.Text = string.Concat(new string[]
					{
						sleekLabel5.Text,
						" ",
						blueprintSupply.hasAmount.ToString(),
						"/",
						blueprintSupply.amount.ToString()
					});
					if (this.blueprint.type == EBlueprintType.AMMO)
					{
						if (blueprintSupply.hasAmount == 0 || blueprintSupply.amount == 0)
						{
							sleekLabel4.TextColor = 6;
						}
					}
					else if (blueprintSupply.hasAmount < blueprintSupply.amount)
					{
						sleekLabel4.TextColor = 6;
					}
					num += (int)(itemAsset.size_x * 50 + 25);
					if (i < this.blueprint.supplies.Length - 1 || this.blueprint.tool != 0 || this.blueprint.type == EBlueprintType.REPAIR || this.blueprint.type == EBlueprintType.AMMO)
					{
						ISleekLabel sleekLabel6 = sleekLabel;
						sleekLabel6.Text += " + ";
						ISleekImage sleekImage = Glazier.Get().CreateImage(PlayerDashboardCraftingUI.icons.load<Texture2D>("Plus"));
						sleekImage.PositionOffset_X = (float)num;
						sleekImage.PositionOffset_Y = -20f;
						sleekImage.PositionScale_Y = 0.5f;
						sleekImage.SizeOffset_X = 40f;
						sleekImage.SizeOffset_Y = 40f;
						sleekImage.TintColor = 2;
						this.container.AddChild(sleekImage);
						num += 65;
					}
				}
			}
			if (this.blueprint.tool != 0)
			{
				ItemAsset itemAsset2 = Assets.find(EAssetType.ITEM, this.blueprint.tool) as ItemAsset;
				if (itemAsset2 != null)
				{
					ISleekLabel sleekLabel7 = sleekLabel;
					sleekLabel7.Text += itemAsset2.itemName;
					SleekItemIcon sleekItemIcon2 = new SleekItemIcon();
					sleekItemIcon2.PositionOffset_X = (float)num;
					sleekItemIcon2.PositionOffset_Y = (float)(-itemAsset2.size_y * 25);
					sleekItemIcon2.PositionScale_Y = 0.5f;
					sleekItemIcon2.SizeOffset_X = (float)(itemAsset2.size_x * 50);
					sleekItemIcon2.SizeOffset_Y = (float)(itemAsset2.size_y * 50);
					this.container.AddChild(sleekItemIcon2);
					sleekItemIcon2.Refresh(this.blueprint.tool, 100, itemAsset2.getState(), itemAsset2);
					ISleekLabel sleekLabel8 = Glazier.Get().CreateLabel();
					sleekLabel8.PositionOffset_X = -100f;
					sleekLabel8.PositionOffset_Y = -30f;
					sleekLabel8.PositionScale_Y = 1f;
					sleekLabel8.SizeOffset_X = 100f;
					sleekLabel8.SizeOffset_Y = 30f;
					sleekLabel8.SizeScale_X = 1f;
					sleekLabel8.TextAlignment = 5;
					sleekLabel8.Text = this.blueprint.tools.ToString() + "/1";
					sleekLabel8.TextContrastContext = 1;
					sleekItemIcon2.AddChild(sleekLabel8);
					ISleekLabel sleekLabel9 = sleekLabel;
					sleekLabel9.Text = sleekLabel9.Text + " " + this.blueprint.tools.ToString() + "/1";
					if (!this.blueprint.hasTool)
					{
						sleekLabel8.TextColor = 6;
					}
					num += (int)(itemAsset2.size_x * 50 + 25);
					if (this.blueprint.type == EBlueprintType.REPAIR || this.blueprint.type == EBlueprintType.AMMO)
					{
						ISleekLabel sleekLabel10 = sleekLabel;
						sleekLabel10.Text += " + ";
						ISleekImage sleekImage2 = Glazier.Get().CreateImage(PlayerDashboardCraftingUI.icons.load<Texture2D>("Plus"));
						sleekImage2.PositionOffset_X = (float)num;
						sleekImage2.PositionOffset_Y = -20f;
						sleekImage2.PositionScale_Y = 0.5f;
						sleekImage2.SizeOffset_X = 40f;
						sleekImage2.SizeOffset_Y = 40f;
						sleekImage2.TintColor = 2;
						this.container.AddChild(sleekImage2);
						num += 65;
					}
				}
			}
			if (this.blueprint.type == EBlueprintType.REPAIR || this.blueprint.type == EBlueprintType.AMMO)
			{
				ItemAsset itemAsset3 = Assets.find(EAssetType.ITEM, this.blueprint.outputs[0].id) as ItemAsset;
				if (itemAsset3 != null)
				{
					ISleekLabel sleekLabel11 = sleekLabel;
					sleekLabel11.Text += itemAsset3.itemName;
					SleekItemIcon sleekItemIcon3 = new SleekItemIcon();
					sleekItemIcon3.PositionOffset_X = (float)num;
					sleekItemIcon3.PositionOffset_Y = (float)(-itemAsset3.size_y * 25);
					sleekItemIcon3.PositionScale_Y = 0.5f;
					sleekItemIcon3.SizeOffset_X = (float)(itemAsset3.size_x * 50);
					sleekItemIcon3.SizeOffset_Y = (float)(itemAsset3.size_y * 50);
					this.container.AddChild(sleekItemIcon3);
					sleekItemIcon3.Refresh(this.blueprint.outputs[0].id, 100, itemAsset3.getState(), itemAsset3);
					ISleekLabel sleekLabel12 = Glazier.Get().CreateLabel();
					sleekLabel12.PositionOffset_X = -100f;
					sleekLabel12.PositionOffset_Y = -30f;
					sleekLabel12.PositionScale_Y = 1f;
					sleekLabel12.SizeOffset_X = 100f;
					sleekLabel12.SizeOffset_Y = 30f;
					sleekLabel12.SizeScale_X = 1f;
					sleekLabel12.TextAlignment = 5;
					if (this.blueprint.type == EBlueprintType.REPAIR)
					{
						ISleekLabel sleekLabel13 = sleekLabel;
						sleekLabel13.Text = sleekLabel13.Text + " " + this.blueprint.items.ToString() + "%";
						sleekLabel12.Text = this.blueprint.items.ToString() + "%";
						sleekLabel12.TextColor = ItemTool.getQualityColor((float)this.blueprint.items / 100f);
					}
					else if (this.blueprint.type == EBlueprintType.AMMO)
					{
						ISleekLabel sleekLabel5 = sleekLabel;
						sleekLabel5.Text = string.Concat(new string[]
						{
							sleekLabel5.Text,
							" ",
							this.blueprint.items.ToString(),
							"/",
							this.blueprint.products.ToString()
						});
						sleekLabel12.Text = this.blueprint.items.ToString() + "/" + itemAsset3.amount.ToString();
					}
					if (!this.blueprint.hasItem)
					{
						sleekLabel12.TextColor = 6;
					}
					sleekLabel12.TextContrastContext = 1;
					sleekItemIcon3.AddChild(sleekLabel12);
					num += (int)(itemAsset3.size_x * 50 + 25);
				}
			}
			ISleekLabel sleekLabel14 = sleekLabel;
			sleekLabel14.Text += " = ";
			ISleekImage sleekImage3 = Glazier.Get().CreateImage(PlayerDashboardCraftingUI.icons.load<Texture2D>("Equals"));
			sleekImage3.PositionOffset_X = (float)num;
			sleekImage3.PositionOffset_Y = -20f;
			sleekImage3.PositionScale_Y = 0.5f;
			sleekImage3.SizeOffset_X = 40f;
			sleekImage3.SizeOffset_Y = 40f;
			sleekImage3.TintColor = 2;
			this.container.AddChild(sleekImage3);
			num += 65;
			for (int j = 0; j < this.blueprint.outputs.Length; j++)
			{
				BlueprintOutput blueprintOutput = this.blueprint.outputs[j];
				ItemAsset itemAsset4 = Assets.find(EAssetType.ITEM, blueprintOutput.id) as ItemAsset;
				if (itemAsset4 != null)
				{
					ISleekLabel sleekLabel15 = sleekLabel;
					sleekLabel15.Text += itemAsset4.itemName;
					SleekItemIcon sleekItemIcon4 = new SleekItemIcon();
					sleekItemIcon4.PositionOffset_X = (float)num;
					sleekItemIcon4.PositionOffset_Y = (float)(-itemAsset4.size_y * 25);
					sleekItemIcon4.PositionScale_Y = 0.5f;
					sleekItemIcon4.SizeOffset_X = (float)(itemAsset4.size_x * 50);
					sleekItemIcon4.SizeOffset_Y = (float)(itemAsset4.size_y * 50);
					this.container.AddChild(sleekItemIcon4);
					sleekItemIcon4.Refresh(blueprintOutput.id, 100, itemAsset4.getState(), itemAsset4);
					ISleekLabel sleekLabel16 = Glazier.Get().CreateLabel();
					sleekLabel16.PositionOffset_X = -100f;
					sleekLabel16.PositionOffset_Y = -30f;
					sleekLabel16.PositionScale_Y = 1f;
					sleekLabel16.SizeOffset_X = 100f;
					sleekLabel16.SizeOffset_Y = 30f;
					sleekLabel16.SizeScale_X = 1f;
					sleekLabel16.TextAlignment = 5;
					sleekLabel16.TextContrastContext = 1;
					if (this.blueprint.type == EBlueprintType.REPAIR)
					{
						ISleekLabel sleekLabel17 = sleekLabel;
						sleekLabel17.Text += " 100%";
						sleekLabel16.Text = "100%";
						sleekLabel16.TextColor = Palette.COLOR_G;
					}
					else if (this.blueprint.type == EBlueprintType.AMMO)
					{
						ItemAsset itemAsset5 = Assets.find(EAssetType.ITEM, blueprintOutput.id) as ItemAsset;
						if (itemAsset5 != null)
						{
							ISleekLabel sleekLabel5 = sleekLabel;
							sleekLabel5.Text = string.Concat(new string[]
							{
								sleekLabel5.Text,
								" ",
								this.blueprint.products.ToString(),
								"/",
								itemAsset5.amount.ToString()
							});
							sleekLabel16.Text = this.blueprint.products.ToString() + "/" + itemAsset5.amount.ToString();
						}
					}
					else
					{
						ISleekLabel sleekLabel18 = sleekLabel;
						sleekLabel18.Text = sleekLabel18.Text + " x" + blueprintOutput.amount.ToString();
						sleekLabel16.Text = "x" + blueprintOutput.amount.ToString();
					}
					sleekItemIcon4.AddChild(sleekLabel16);
					num += (int)(itemAsset4.size_x * 50);
					if (j < this.blueprint.outputs.Length - 1)
					{
						num += 25;
						ISleekLabel sleekLabel19 = sleekLabel;
						sleekLabel19.Text += " + ";
						ISleekImage sleekImage4 = Glazier.Get().CreateImage(PlayerDashboardCraftingUI.icons.load<Texture2D>("Plus"));
						sleekImage4.PositionOffset_X = (float)num;
						sleekImage4.PositionOffset_Y = -20f;
						sleekImage4.PositionScale_Y = 0.5f;
						sleekImage4.SizeOffset_X = 40f;
						sleekImage4.SizeOffset_Y = 40f;
						sleekImage4.TintColor = 2;
						this.container.AddChild(sleekImage4);
						num += 65;
					}
				}
			}
			this.container.PositionOffset_X = (float)(-(float)num / 2);
			this.container.SizeOffset_X = (float)num;
			if (!supportsDepth)
			{
				this.craftButton = Glazier.Get().CreateButton();
				this.craftButton.PositionOffset_X = -70f;
				this.craftButton.PositionOffset_Y = -35f;
				this.craftButton.PositionScale_X = 0.75f;
				this.craftButton.PositionScale_Y = 1f;
				this.craftButton.SizeOffset_X = 140f;
				this.craftButton.SizeOffset_Y = 30f;
				this.craftButton.Text = PlayerDashboardCraftingUI.localization.format("Craft");
				this.craftButton.TextColor = sleekLabel.TextColor;
				this.craftButton.OnClicked += new ClickedButton(this.onClickedButton);
				base.AddChild(this.craftButton);
				this.craftAllButton = Glazier.Get().CreateButton();
				this.craftAllButton.PositionOffset_X = -70f;
				this.craftAllButton.PositionOffset_Y = -35f;
				this.craftAllButton.PositionScale_X = 0.25f;
				this.craftAllButton.PositionScale_Y = 1f;
				this.craftAllButton.SizeOffset_X = 140f;
				this.craftAllButton.SizeOffset_Y = 30f;
				this.craftAllButton.Text = PlayerDashboardCraftingUI.localization.format("Craft_All");
				this.craftAllButton.TextColor = sleekLabel.TextColor;
				this.craftAllButton.OnClicked += new ClickedButton(this.onClickedAltButton);
				base.AddChild(this.craftAllButton);
			}
			this.ignoreToggleButton = Glazier.Get().CreateToggle();
			this.ignoreToggleButton.PositionOffset_X = -50f;
			this.ignoreToggleButton.PositionOffset_Y = -40f;
			this.ignoreToggleButton.PositionScale_X = 1f;
			this.ignoreToggleButton.PositionScale_Y = 1f;
			this.ignoreToggleButton.SizeOffset_X = 40f;
			this.ignoreToggleButton.SizeOffset_Y = 40f;
			this.ignoreToggleButton.OnValueChanged += new Toggled(this.onToggledIgnoring);
			base.AddChild(this.ignoreToggleButton);
			this.refreshIgnoring();
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x001163B5 File Offset: 0x001145B5
		private void onClickedBackgroundButton(ISleekElement internalButton)
		{
			SleekBlueprint.Clicked clicked = this.onClickedCraftButton;
			if (clicked == null)
			{
				return;
			}
			clicked(this.blueprint);
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06003B4A RID: 15178 RVA: 0x001163CD File Offset: 0x001145CD
		private bool isCraftable
		{
			get
			{
				return this.blueprint.hasSupplies && this.blueprint.hasTool && this.blueprint.hasItem && this.blueprint.hasSkills;
			}
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x00116403 File Offset: 0x00114603
		private void onClickedButton(ISleekElement internalButton)
		{
			SleekBlueprint.Clicked clicked = this.onClickedCraftButton;
			if (clicked == null)
			{
				return;
			}
			clicked(this.blueprint);
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x0011641B File Offset: 0x0011461B
		private void onClickedAltButton(ISleekElement internalButton)
		{
			SleekBlueprint.Clicked clicked = this.onClickedCraftAllButton;
			if (clicked == null)
			{
				return;
			}
			clicked(this.blueprint);
		}

		// Token: 0x0400252C RID: 9516
		private Blueprint _blueprint;

		// Token: 0x0400252D RID: 9517
		private ISleekButton craftButton;

		// Token: 0x0400252E RID: 9518
		private ISleekButton craftAllButton;

		// Token: 0x0400252F RID: 9519
		private ISleekButton backgroundButton;

		// Token: 0x04002530 RID: 9520
		private ISleekElement container;

		// Token: 0x04002531 RID: 9521
		private ISleekToggle ignoreToggleButton;

		// Token: 0x020009EE RID: 2542
		// (Invoke) Token: 0x06004CEA RID: 19690
		public delegate void Clicked(Blueprint blueprint);
	}
}
