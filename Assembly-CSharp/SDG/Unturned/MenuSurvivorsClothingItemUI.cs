using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007AD RID: 1965
	public class MenuSurvivorsClothingItemUI
	{
		// Token: 0x060041E1 RID: 16865 RVA: 0x00164360 File Offset: 0x00162560
		public static void open()
		{
			if (MenuSurvivorsClothingItemUI.active)
			{
				return;
			}
			MenuSurvivorsClothingItemUI.active = true;
			MenuSurvivorsClothingItemUI.container.AnimateIntoView();
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x0016437A File Offset: 0x0016257A
		public static void close()
		{
			if (!MenuSurvivorsClothingItemUI.active)
			{
				return;
			}
			MenuSurvivorsClothingItemUI.active = false;
			MenuSurvivorsClothingItemUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x0016439E File Offset: 0x0016259E
		public static void viewItem()
		{
			MenuSurvivorsClothingItemUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.quantity, MenuSurvivorsClothingItemUI.instance);
		}

		// Token: 0x060041E4 RID: 16868 RVA: 0x001643B4 File Offset: 0x001625B4
		public static void viewItem(int newItem, ushort newQuantity, ulong newInstance)
		{
			UnturnedLog.info(string.Concat(new string[]
			{
				"View: ",
				newItem.ToString(),
				" x",
				newQuantity.ToString(),
				" [",
				newInstance.ToString(),
				"]"
			}));
			MenuSurvivorsClothingItemUI.item = newItem;
			MenuSurvivorsClothingItemUI.quantity = newQuantity;
			MenuSurvivorsClothingItemUI.instance = newInstance;
			MenuSurvivorsClothingItemUI.packageBox.updateInventory(MenuSurvivorsClothingItemUI.instance, MenuSurvivorsClothingItemUI.item, newQuantity, false, true);
			if (MenuSurvivorsClothingItemUI.packageBox.itemAsset == null && MenuSurvivorsClothingItemUI.packageBox.vehicleAsset == null)
			{
				MenuSurvivorsClothingItemUI.useButton.IsVisible = false;
				MenuSurvivorsClothingItemUI.inspectButton.IsVisible = false;
				MenuSurvivorsClothingItemUI.marketButton.IsVisible = false;
				MenuSurvivorsClothingItemUI.scrapButton.IsVisible = false;
				MenuSurvivorsClothingItemUI.deleteButton.IsVisible = true;
				MenuSurvivorsClothingItemUI.descriptionBox.SizeOffset_Y = -60f;
				MenuSurvivorsClothingItemUI.deleteButton.PositionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.SizeOffset_Y - 50f;
				MenuSurvivorsClothingItemUI.deleteButton.SizeScale_X = 0.5f;
				MenuSurvivorsClothingItemUI.infoLabel.Text = MenuSurvivorsClothingItemUI.localization.format("Unknown");
				return;
			}
			if (MenuSurvivorsClothingItemUI.packageBox.itemAsset != null && MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.KEY)
			{
				if ((MenuSurvivorsClothingItemUI.packageBox.itemAsset as ItemKeyAsset).exchangeWithTargetItem)
				{
					MenuSurvivorsClothingItemUI.useButton.IsVisible = true;
					MenuSurvivorsClothingItemUI.useButton.Text = MenuSurvivorsClothingItemUI.localization.format("Target_Item_Text");
					MenuSurvivorsClothingItemUI.useButton.TooltipText = MenuSurvivorsClothingItemUI.localization.format("Target_Item_Tooltip");
				}
				else
				{
					MenuSurvivorsClothingItemUI.useButton.IsVisible = false;
				}
				MenuSurvivorsClothingItemUI.inspectButton.IsVisible = false;
			}
			else if (MenuSurvivorsClothingItemUI.packageBox.itemAsset != null && MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.BOX)
			{
				MenuSurvivorsClothingItemUI.useButton.IsVisible = true;
				MenuSurvivorsClothingItemUI.inspectButton.IsVisible = false;
				MenuSurvivorsClothingItemUI.useButton.Text = MenuSurvivorsClothingItemUI.localization.format("Contents_Text");
				MenuSurvivorsClothingItemUI.useButton.TooltipText = MenuSurvivorsClothingItemUI.localization.format("Contents_Tooltip");
			}
			else
			{
				MenuSurvivorsClothingItemUI.useButton.IsVisible = true;
				MenuSurvivorsClothingItemUI.inspectButton.IsVisible = true;
				bool flag;
				if (MenuSurvivorsClothingItemUI.packageBox.itemAsset == null || MenuSurvivorsClothingItemUI.packageBox.itemAsset.proPath == null || MenuSurvivorsClothingItemUI.packageBox.itemAsset.proPath.Length == 0)
				{
					flag = Characters.isSkinEquipped(MenuSurvivorsClothingItemUI.instance);
				}
				else
				{
					flag = Characters.isCosmeticEquipped(MenuSurvivorsClothingItemUI.instance);
				}
				MenuSurvivorsClothingItemUI.useButton.Text = MenuSurvivorsClothingItemUI.localization.format(flag ? "Dequip_Text" : "Equip_Text");
				MenuSurvivorsClothingItemUI.useButton.TooltipText = MenuSurvivorsClothingItemUI.localization.format(flag ? "Dequip_Tooltip" : "Equip_Tooltip");
			}
			MenuSurvivorsClothingItemUI.marketButton.IsVisible = Provider.provider.economyService.getInventoryMarketable(MenuSurvivorsClothingItemUI.item);
			int inventoryScraps = Provider.provider.economyService.getInventoryScraps(MenuSurvivorsClothingItemUI.item);
			MenuSurvivorsClothingItemUI.scrapButton.Text = MenuSurvivorsClothingItemUI.localization.format("Scrap_Text", inventoryScraps);
			MenuSurvivorsClothingItemUI.scrapButton.TooltipText = MenuSurvivorsClothingItemUI.localization.format("Scrap_Tooltip", inventoryScraps);
			MenuSurvivorsClothingItemUI.scrapButton.IsVisible = (inventoryScraps > 0);
			MenuSurvivorsClothingItemUI.descriptionBox.SizeOffset_Y = 0f;
			if (MenuSurvivorsClothingItemUI.useButton.IsVisible || MenuSurvivorsClothingItemUI.inspectButton.IsVisible)
			{
				MenuSurvivorsClothingItemUI.descriptionBox.SizeOffset_Y -= 60f;
				MenuSurvivorsClothingItemUI.useButton.PositionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.SizeOffset_Y - 50f;
				MenuSurvivorsClothingItemUI.inspectButton.PositionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.SizeOffset_Y - 50f;
			}
			if (MenuSurvivorsClothingItemUI.scrapButton.IsVisible)
			{
				MenuSurvivorsClothingItemUI.deleteButton.SizeScale_X = 0.25f;
			}
			else
			{
				MenuSurvivorsClothingItemUI.deleteButton.SizeScale_X = 0.5f;
			}
			if (MenuSurvivorsClothingItemUI.marketButton.IsVisible || MenuSurvivorsClothingItemUI.deleteButton.IsVisible || MenuSurvivorsClothingItemUI.scrapButton.IsVisible)
			{
				MenuSurvivorsClothingItemUI.descriptionBox.SizeOffset_Y -= 60f;
				MenuSurvivorsClothingItemUI.marketButton.PositionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.SizeOffset_Y - 50f;
				MenuSurvivorsClothingItemUI.deleteButton.PositionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.SizeOffset_Y - 50f;
				MenuSurvivorsClothingItemUI.scrapButton.PositionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.SizeOffset_Y - 50f;
			}
			MenuSurvivorsClothingItemUI.infoLabel.Text = string.Concat(new string[]
			{
				"<color=",
				Palette.hex(Provider.provider.economyService.getInventoryColor(MenuSurvivorsClothingItemUI.item)),
				">",
				Provider.provider.economyService.getInventoryType(MenuSurvivorsClothingItemUI.item),
				"</color>\n\n",
				Provider.provider.economyService.getInventoryDescription(MenuSurvivorsClothingItemUI.item)
			});
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x001648AC File Offset: 0x00162AAC
		private static void onClickedUseButton(ISleekElement button)
		{
			if (MenuSurvivorsClothingItemUI.packageBox.itemAsset != null && MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.KEY)
			{
				ushort id = MenuSurvivorsClothingItemUI.packageBox.itemAsset.id;
				EEconFilterMode newFilterMode;
				if (id <= 992)
				{
					if (id - 845 <= 1)
					{
						newFilterMode = EEconFilterMode.STAT_TRACKER;
						goto IL_9D;
					}
					if (id == 992)
					{
						newFilterMode = EEconFilterMode.STAT_TRACKER_REMOVAL;
						goto IL_9D;
					}
				}
				else
				{
					if (id == 993)
					{
						newFilterMode = EEconFilterMode.RAGDOLL_EFFECT_REMOVAL;
						goto IL_9D;
					}
					if (id == 1524 || id - 1868 <= 4)
					{
						newFilterMode = EEconFilterMode.RAGDOLL_EFFECT;
						goto IL_9D;
					}
				}
				UnturnedLog.warn("Unknown tool " + MenuSurvivorsClothingItemUI.packageBox.itemAsset.name);
				newFilterMode = EEconFilterMode.STAT_TRACKER;
				IL_9D:
				MenuSurvivorsClothingUI.setFilter(newFilterMode, MenuSurvivorsClothingItemUI.instance);
				MenuSurvivorsClothingUI.open();
				MenuSurvivorsClothingItemUI.close();
				return;
			}
			if (MenuSurvivorsClothingItemUI.packageBox.itemAsset != null && MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.BOX)
			{
				MenuSurvivorsClothingBoxUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.quantity, MenuSurvivorsClothingItemUI.instance);
				MenuSurvivorsClothingBoxUI.open();
				MenuSurvivorsClothingItemUI.close();
				return;
			}
			Characters.ToggleEquipItemByInstanceId(MenuSurvivorsClothingItemUI.instance);
			MenuSurvivorsClothingItemUI.viewItem();
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x001649B9 File Offset: 0x00162BB9
		private static void onClickedInspectButton(ISleekElement button)
		{
			MenuSurvivorsClothingInspectUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.instance);
			MenuSurvivorsClothingInspectUI.open();
			MenuSurvivorsClothingItemUI.close();
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x001649D4 File Offset: 0x00162BD4
		private static void onClickedMarketButton(ISleekElement button)
		{
			if (!Provider.provider.economyService.canOpenInventory)
			{
				MenuUI.alert(MenuSurvivorsClothingItemUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.economyService.open(MenuSurvivorsClothingItemUI.instance);
		}

		// Token: 0x060041E8 RID: 16872 RVA: 0x00164A10 File Offset: 0x00162C10
		private static void onClickedDeleteButton(ISleekElement button)
		{
			MenuSurvivorsClothingDeleteUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.instance, MenuSurvivorsClothingItemUI.quantity, EDeleteMode.DELETE, 0UL);
			MenuSurvivorsClothingDeleteUI.open();
			MenuSurvivorsClothingItemUI.close();
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x00164A34 File Offset: 0x00162C34
		private static void onClickedScrapButton(ISleekElement button)
		{
			if (Provider.provider.economyService.getInventoryMythicID(MenuSurvivorsClothingItemUI.item) != 0 || !InputEx.GetKey(ControlsSettings.other))
			{
				MenuSurvivorsClothingDeleteUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.instance, MenuSurvivorsClothingItemUI.quantity, EDeleteMode.SALVAGE, 0UL);
				MenuSurvivorsClothingDeleteUI.open();
				MenuSurvivorsClothingItemUI.close();
				return;
			}
			MenuSurvivorsClothingDeleteUI.salvageItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.instance);
			MenuSurvivorsClothingItemUI.onClickedBackButton(null);
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x00164A9A File Offset: 0x00162C9A
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuSurvivorsClothingUI.open();
			MenuSurvivorsClothingItemUI.close();
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x00164AA8 File Offset: 0x00162CA8
		public MenuSurvivorsClothingItemUI()
		{
			MenuSurvivorsClothingItemUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsClothingItem.dat");
			MenuSurvivorsClothingItemUI.container = new SleekFullscreenBox();
			MenuSurvivorsClothingItemUI.container.PositionOffset_X = 10f;
			MenuSurvivorsClothingItemUI.container.PositionOffset_Y = 10f;
			MenuSurvivorsClothingItemUI.container.PositionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.container.SizeOffset_X = -20f;
			MenuSurvivorsClothingItemUI.container.SizeOffset_Y = -20f;
			MenuSurvivorsClothingItemUI.container.SizeScale_X = 1f;
			MenuSurvivorsClothingItemUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuSurvivorsClothingItemUI.container);
			MenuSurvivorsClothingItemUI.active = false;
			MenuSurvivorsClothingItemUI.inventory = Glazier.Get().CreateConstraintFrame();
			MenuSurvivorsClothingItemUI.inventory.PositionScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.inventory.PositionOffset_Y = 10f;
			MenuSurvivorsClothingItemUI.inventory.SizeScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.inventory.SizeScale_Y = 1f;
			MenuSurvivorsClothingItemUI.inventory.SizeOffset_Y = -20f;
			MenuSurvivorsClothingItemUI.inventory.Constraint = 1;
			MenuSurvivorsClothingItemUI.container.AddChild(MenuSurvivorsClothingItemUI.inventory);
			ISleekConstraintFrame sleekConstraintFrame = Glazier.Get().CreateConstraintFrame();
			sleekConstraintFrame.SizeScale_X = 1f;
			sleekConstraintFrame.SizeScale_Y = 0.5f;
			sleekConstraintFrame.SizeOffset_Y = -5f;
			sleekConstraintFrame.Constraint = 1;
			MenuSurvivorsClothingItemUI.inventory.AddChild(sleekConstraintFrame);
			MenuSurvivorsClothingItemUI.packageBox = new SleekInventory();
			MenuSurvivorsClothingItemUI.packageBox.SizeScale_X = 1f;
			MenuSurvivorsClothingItemUI.packageBox.SizeScale_Y = 1f;
			sleekConstraintFrame.AddChild(MenuSurvivorsClothingItemUI.packageBox);
			MenuSurvivorsClothingItemUI.descriptionBox = Glazier.Get().CreateBox();
			MenuSurvivorsClothingItemUI.descriptionBox.PositionOffset_Y = 10f;
			MenuSurvivorsClothingItemUI.descriptionBox.PositionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.descriptionBox.SizeScale_X = 1f;
			MenuSurvivorsClothingItemUI.descriptionBox.SizeScale_Y = 1f;
			MenuSurvivorsClothingItemUI.packageBox.AddChild(MenuSurvivorsClothingItemUI.descriptionBox);
			MenuSurvivorsClothingItemUI.infoLabel = Glazier.Get().CreateLabel();
			MenuSurvivorsClothingItemUI.infoLabel.AllowRichText = true;
			MenuSurvivorsClothingItemUI.infoLabel.PositionOffset_X = 5f;
			MenuSurvivorsClothingItemUI.infoLabel.PositionOffset_Y = 5f;
			MenuSurvivorsClothingItemUI.infoLabel.SizeScale_X = 1f;
			MenuSurvivorsClothingItemUI.infoLabel.SizeScale_Y = 1f;
			MenuSurvivorsClothingItemUI.infoLabel.SizeOffset_X = -10f;
			MenuSurvivorsClothingItemUI.infoLabel.SizeOffset_Y = -10f;
			MenuSurvivorsClothingItemUI.infoLabel.TextAlignment = 0;
			MenuSurvivorsClothingItemUI.infoLabel.TextColor = 4;
			MenuSurvivorsClothingItemUI.infoLabel.TextContrastContext = 1;
			MenuSurvivorsClothingItemUI.descriptionBox.AddChild(MenuSurvivorsClothingItemUI.infoLabel);
			MenuSurvivorsClothingItemUI.useButton = Glazier.Get().CreateButton();
			MenuSurvivorsClothingItemUI.useButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.useButton.SizeOffset_X = -5f;
			MenuSurvivorsClothingItemUI.useButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingItemUI.useButton.SizeScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.useButton.OnClicked += new ClickedButton(MenuSurvivorsClothingItemUI.onClickedUseButton);
			MenuSurvivorsClothingItemUI.descriptionBox.AddChild(MenuSurvivorsClothingItemUI.useButton);
			MenuSurvivorsClothingItemUI.useButton.FontSize = 3;
			MenuSurvivorsClothingItemUI.useButton.IsVisible = false;
			MenuSurvivorsClothingItemUI.inspectButton = Glazier.Get().CreateButton();
			MenuSurvivorsClothingItemUI.inspectButton.PositionOffset_X = 5f;
			MenuSurvivorsClothingItemUI.inspectButton.PositionScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.inspectButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.inspectButton.SizeOffset_X = -5f;
			MenuSurvivorsClothingItemUI.inspectButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingItemUI.inspectButton.SizeScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.inspectButton.Text = MenuSurvivorsClothingItemUI.localization.format("Inspect_Text");
			MenuSurvivorsClothingItemUI.inspectButton.TooltipText = MenuSurvivorsClothingItemUI.localization.format("Inspect_Tooltip");
			MenuSurvivorsClothingItemUI.inspectButton.OnClicked += new ClickedButton(MenuSurvivorsClothingItemUI.onClickedInspectButton);
			MenuSurvivorsClothingItemUI.descriptionBox.AddChild(MenuSurvivorsClothingItemUI.inspectButton);
			MenuSurvivorsClothingItemUI.inspectButton.FontSize = 3;
			MenuSurvivorsClothingItemUI.inspectButton.IsVisible = false;
			MenuSurvivorsClothingItemUI.marketButton = Glazier.Get().CreateButton();
			MenuSurvivorsClothingItemUI.marketButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.marketButton.SizeOffset_X = -5f;
			MenuSurvivorsClothingItemUI.marketButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingItemUI.marketButton.SizeScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.marketButton.Text = MenuSurvivorsClothingItemUI.localization.format("Market_Text");
			MenuSurvivorsClothingItemUI.marketButton.TooltipText = MenuSurvivorsClothingItemUI.localization.format("Market_Tooltip");
			MenuSurvivorsClothingItemUI.marketButton.OnClicked += new ClickedButton(MenuSurvivorsClothingItemUI.onClickedMarketButton);
			MenuSurvivorsClothingItemUI.descriptionBox.AddChild(MenuSurvivorsClothingItemUI.marketButton);
			MenuSurvivorsClothingItemUI.marketButton.FontSize = 3;
			MenuSurvivorsClothingItemUI.marketButton.IsVisible = false;
			MenuSurvivorsClothingItemUI.deleteButton = Glazier.Get().CreateButton();
			MenuSurvivorsClothingItemUI.deleteButton.PositionOffset_X = 5f;
			MenuSurvivorsClothingItemUI.deleteButton.PositionScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.deleteButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.deleteButton.SizeOffset_X = -5f;
			MenuSurvivorsClothingItemUI.deleteButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingItemUI.deleteButton.SizeScale_X = 0.5f;
			MenuSurvivorsClothingItemUI.deleteButton.Text = MenuSurvivorsClothingItemUI.localization.format("Delete_Text");
			MenuSurvivorsClothingItemUI.deleteButton.TooltipText = MenuSurvivorsClothingItemUI.localization.format("Delete_Tooltip");
			MenuSurvivorsClothingItemUI.deleteButton.OnClicked += new ClickedButton(MenuSurvivorsClothingItemUI.onClickedDeleteButton);
			MenuSurvivorsClothingItemUI.descriptionBox.AddChild(MenuSurvivorsClothingItemUI.deleteButton);
			MenuSurvivorsClothingItemUI.deleteButton.FontSize = 3;
			MenuSurvivorsClothingItemUI.scrapButton = Glazier.Get().CreateButton();
			MenuSurvivorsClothingItemUI.scrapButton.PositionOffset_X = 5f;
			MenuSurvivorsClothingItemUI.scrapButton.PositionScale_X = 0.75f;
			MenuSurvivorsClothingItemUI.scrapButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.scrapButton.SizeOffset_X = -5f;
			MenuSurvivorsClothingItemUI.scrapButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingItemUI.scrapButton.SizeScale_X = 0.25f;
			MenuSurvivorsClothingItemUI.scrapButton.OnClicked += new ClickedButton(MenuSurvivorsClothingItemUI.onClickedScrapButton);
			MenuSurvivorsClothingItemUI.descriptionBox.AddChild(MenuSurvivorsClothingItemUI.scrapButton);
			MenuSurvivorsClothingItemUI.scrapButton.FontSize = 3;
			MenuSurvivorsClothingItemUI.scrapButton.IsVisible = false;
			MenuSurvivorsClothingItemUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuSurvivorsClothingItemUI.backButton.PositionOffset_Y = -50f;
			MenuSurvivorsClothingItemUI.backButton.PositionScale_Y = 1f;
			MenuSurvivorsClothingItemUI.backButton.SizeOffset_X = 200f;
			MenuSurvivorsClothingItemUI.backButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingItemUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsClothingItemUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuSurvivorsClothingItemUI.backButton.onClickedButton += new ClickedButton(MenuSurvivorsClothingItemUI.onClickedBackButton);
			MenuSurvivorsClothingItemUI.backButton.fontSize = 3;
			MenuSurvivorsClothingItemUI.backButton.iconColor = 2;
			MenuSurvivorsClothingItemUI.container.AddChild(MenuSurvivorsClothingItemUI.backButton);
		}

		// Token: 0x04002AFA RID: 11002
		private static Local localization;

		// Token: 0x04002AFB RID: 11003
		private static SleekFullscreenBox container;

		// Token: 0x04002AFC RID: 11004
		public static bool active;

		// Token: 0x04002AFD RID: 11005
		private static SleekButtonIcon backButton;

		// Token: 0x04002AFE RID: 11006
		private static int item;

		// Token: 0x04002AFF RID: 11007
		private static ushort quantity;

		// Token: 0x04002B00 RID: 11008
		private static ulong instance;

		// Token: 0x04002B01 RID: 11009
		private static ISleekConstraintFrame inventory;

		// Token: 0x04002B02 RID: 11010
		private static SleekInventory packageBox;

		// Token: 0x04002B03 RID: 11011
		private static ISleekBox descriptionBox;

		// Token: 0x04002B04 RID: 11012
		private static ISleekLabel infoLabel;

		// Token: 0x04002B05 RID: 11013
		private static ISleekButton useButton;

		// Token: 0x04002B06 RID: 11014
		private static ISleekButton inspectButton;

		// Token: 0x04002B07 RID: 11015
		private static ISleekButton marketButton;

		// Token: 0x04002B08 RID: 11016
		private static ISleekButton deleteButton;

		// Token: 0x04002B09 RID: 11017
		private static ISleekButton scrapButton;
	}
}
