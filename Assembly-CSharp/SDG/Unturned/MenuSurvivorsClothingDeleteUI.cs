using System;
using System.Collections.Generic;
using SDG.Provider;

namespace SDG.Unturned
{
	// Token: 0x020007AB RID: 1963
	public class MenuSurvivorsClothingDeleteUI
	{
		// Token: 0x060041D2 RID: 16850 RVA: 0x00163215 File Offset: 0x00161415
		public static void open()
		{
			if (MenuSurvivorsClothingDeleteUI.active)
			{
				return;
			}
			MenuSurvivorsClothingDeleteUI.active = true;
			MenuSurvivorsClothingDeleteUI.container.AnimateIntoView();
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x0016322F File Offset: 0x0016142F
		public static void close()
		{
			if (!MenuSurvivorsClothingDeleteUI.active)
			{
				return;
			}
			MenuSurvivorsClothingDeleteUI.active = false;
			MenuSurvivorsClothingDeleteUI.container.AnimateOutOfView(0f, 1f);
		}

		/// <summary>
		/// Note: inventory service does not support exchanging multiple items simultaneously.
		/// </summary>
		// Token: 0x060041D4 RID: 16852 RVA: 0x00163254 File Offset: 0x00161454
		public static void salvageItem(int itemID, ulong instanceID)
		{
			int scrapExchangeItem = Provider.provider.economyService.getScrapExchangeItem(itemID);
			if (scrapExchangeItem < 1)
			{
				UnturnedLog.warn("Unable to find exchange target for salvaging itemdef {0} ({1})", new object[]
				{
					itemID,
					instanceID
				});
				return;
			}
			TempSteamworksEconomy economyService = Provider.provider.economyService;
			int generate = scrapExchangeItem;
			List<EconExchangePair> list = new List<EconExchangePair>();
			list.Add(new EconExchangePair(instanceID, 1));
			economyService.exchangeInventory(generate, list);
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x001632BC File Offset: 0x001614BC
		public static void applyTagTool(int itemID, ulong targetID, ulong toolID)
		{
			List<EconExchangePair> list = new List<EconExchangePair>();
			list.Add(new EconExchangePair(targetID, 1));
			list.Add(new EconExchangePair(toolID, 1));
			List<EconExchangePair> destroy = list;
			Provider.provider.economyService.exchangeInventory(itemID, destroy);
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x001632FC File Offset: 0x001614FC
		public static void viewItem(int newItem, ulong newInstance, ushort newQuantity, EDeleteMode newMode, ulong newInstigator)
		{
			MenuSurvivorsClothingDeleteUI.item = newItem;
			MenuSurvivorsClothingDeleteUI.instance = newInstance;
			MenuSurvivorsClothingDeleteUI.quantity = newQuantity;
			MenuSurvivorsClothingDeleteUI.mode = newMode;
			MenuSurvivorsClothingDeleteUI.instigator = newInstigator;
			MenuSurvivorsClothingDeleteUI.deleteBox.SizeOffset_Y = 130f;
			MenuSurvivorsClothingDeleteUI.yesButton.TooltipText = MenuSurvivorsClothingDeleteUI.localization.format((MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.SALVAGE) ? "Yes_Salvage_Tooltip" : ((MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.TAG_TOOL_ADD || MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.TAG_TOOL_REMOVE) ? "Yes_Tag_Tool_Tooltip" : "Yes_Delete_Tooltip"));
			if (MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.TAG_TOOL_ADD || MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.TAG_TOOL_REMOVE)
			{
				int inventoryItem = Provider.provider.economyService.getInventoryItem(MenuSurvivorsClothingDeleteUI.instigator);
				MenuSurvivorsClothingDeleteUI.intentLabel.Text = MenuSurvivorsClothingDeleteUI.localization.format("Intent_Tag_Tool", string.Concat(new string[]
				{
					"<color=",
					Palette.hex(Provider.provider.economyService.getInventoryColor(inventoryItem)),
					">",
					Provider.provider.economyService.getInventoryName(inventoryItem),
					"</color>"
				}), string.Concat(new string[]
				{
					"<color=",
					Palette.hex(Provider.provider.economyService.getInventoryColor(MenuSurvivorsClothingDeleteUI.item)),
					">",
					Provider.provider.economyService.getInventoryName(MenuSurvivorsClothingDeleteUI.item),
					"</color>"
				}));
			}
			else
			{
				MenuSurvivorsClothingDeleteUI.intentLabel.Text = MenuSurvivorsClothingDeleteUI.localization.format((MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.SALVAGE) ? "Intent_Salvage" : "Intent_Delete", string.Concat(new string[]
				{
					"<color=",
					Palette.hex(Provider.provider.economyService.getInventoryColor(MenuSurvivorsClothingDeleteUI.item)),
					">",
					Provider.provider.economyService.getInventoryName(MenuSurvivorsClothingDeleteUI.item),
					"</color>"
				}));
			}
			MenuSurvivorsClothingDeleteUI.confirmLabel.Text = MenuSurvivorsClothingDeleteUI.localization.format("Confirm", MenuSurvivorsClothingDeleteUI.localization.format((MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.SALVAGE) ? "Salvage" : "Delete"));
			MenuSurvivorsClothingDeleteUI.confirmLabel.IsVisible = (MenuSurvivorsClothingDeleteUI.mode != EDeleteMode.TAG_TOOL_ADD && MenuSurvivorsClothingDeleteUI.mode != EDeleteMode.TAG_TOOL_REMOVE);
			MenuSurvivorsClothingDeleteUI.confirmField.PlaceholderText = MenuSurvivorsClothingDeleteUI.localization.format((MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.SALVAGE) ? "Salvage" : "Delete");
			MenuSurvivorsClothingDeleteUI.confirmField.Text = string.Empty;
			MenuSurvivorsClothingDeleteUI.confirmField.IsVisible = (MenuSurvivorsClothingDeleteUI.mode != EDeleteMode.TAG_TOOL_ADD && MenuSurvivorsClothingDeleteUI.mode != EDeleteMode.TAG_TOOL_REMOVE);
			if (MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.TAG_TOOL_ADD || MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.TAG_TOOL_REMOVE)
			{
				MenuSurvivorsClothingDeleteUI.yesButton.PositionOffset_X = -65f;
				MenuSurvivorsClothingDeleteUI.yesButton.PositionScale_X = 0.5f;
				MenuSurvivorsClothingDeleteUI.noButton.PositionOffset_X = 5f;
				MenuSurvivorsClothingDeleteUI.noButton.PositionScale_X = 0.5f;
			}
			else
			{
				MenuSurvivorsClothingDeleteUI.yesButton.PositionOffset_X = -135f;
				MenuSurvivorsClothingDeleteUI.yesButton.PositionScale_X = 1f;
				MenuSurvivorsClothingDeleteUI.noButton.PositionOffset_X = -65f;
				MenuSurvivorsClothingDeleteUI.noButton.PositionScale_X = 1f;
			}
			if (MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.TAG_TOOL_ADD)
			{
				MenuSurvivorsClothingDeleteUI.warningLabel.Text = MenuSurvivorsClothingDeleteUI.localization.format("Warning_UndoableWithTool");
			}
			else
			{
				MenuSurvivorsClothingDeleteUI.warningLabel.Text = MenuSurvivorsClothingDeleteUI.localization.format("Warning");
			}
			MenuSurvivorsClothingDeleteUI.quantityField.Value = 1;
			MenuSurvivorsClothingDeleteUI.quantityField.MaxValue = MenuSurvivorsClothingDeleteUI.quantity;
			if (MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.DELETE && MenuSurvivorsClothingDeleteUI.quantity > 1)
			{
				MenuSurvivorsClothingDeleteUI.quantityLabel.Text = MenuSurvivorsClothingDeleteUI.localization.format("Quantity", MenuSurvivorsClothingDeleteUI.quantity);
				MenuSurvivorsClothingDeleteUI.deleteBox.SizeOffset_Y += 50f;
				MenuSurvivorsClothingDeleteUI.quantityLabel.IsVisible = true;
				MenuSurvivorsClothingDeleteUI.quantityField.IsVisible = true;
				return;
			}
			MenuSurvivorsClothingDeleteUI.quantityLabel.IsVisible = false;
			MenuSurvivorsClothingDeleteUI.quantityField.IsVisible = false;
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x001636F0 File Offset: 0x001618F0
		private static void onClickedYesButton(ISleekElement button)
		{
			if (MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.SALVAGE)
			{
				if (MenuSurvivorsClothingDeleteUI.confirmField.Text != MenuSurvivorsClothingDeleteUI.localization.format("Salvage"))
				{
					return;
				}
				MenuSurvivorsClothingDeleteUI.salvageItem(MenuSurvivorsClothingDeleteUI.item, MenuSurvivorsClothingDeleteUI.instance);
			}
			else if (MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.DELETE)
			{
				if (MenuSurvivorsClothingDeleteUI.confirmField.Text != MenuSurvivorsClothingDeleteUI.localization.format("Delete"))
				{
					return;
				}
				Provider.provider.economyService.consumeItem(MenuSurvivorsClothingDeleteUI.instance, (uint)MathfEx.Min(MenuSurvivorsClothingDeleteUI.quantityField.Value, MenuSurvivorsClothingDeleteUI.quantity));
			}
			MenuSurvivorsClothingUI.open();
			MenuSurvivorsClothingDeleteUI.close();
			if (MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.TAG_TOOL_ADD || MenuSurvivorsClothingDeleteUI.mode == EDeleteMode.TAG_TOOL_REMOVE)
			{
				MenuSurvivorsClothingUI.prepareForCraftResult();
				MenuSurvivorsClothingDeleteUI.applyTagTool(MenuSurvivorsClothingDeleteUI.item, MenuSurvivorsClothingDeleteUI.instance, MenuSurvivorsClothingDeleteUI.instigator);
			}
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x001637BA File Offset: 0x001619BA
		private static void onClickedNoButton(ISleekElement button)
		{
			MenuSurvivorsClothingItemUI.open();
			MenuSurvivorsClothingDeleteUI.close();
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x001637C8 File Offset: 0x001619C8
		public MenuSurvivorsClothingDeleteUI()
		{
			MenuSurvivorsClothingDeleteUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsClothingDelete.dat");
			MenuSurvivorsClothingDeleteUI.container = new SleekFullscreenBox();
			MenuSurvivorsClothingDeleteUI.container.PositionOffset_X = 10f;
			MenuSurvivorsClothingDeleteUI.container.PositionOffset_Y = 10f;
			MenuSurvivorsClothingDeleteUI.container.PositionScale_Y = 1f;
			MenuSurvivorsClothingDeleteUI.container.SizeOffset_X = -20f;
			MenuSurvivorsClothingDeleteUI.container.SizeOffset_Y = -20f;
			MenuSurvivorsClothingDeleteUI.container.SizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuSurvivorsClothingDeleteUI.container);
			MenuSurvivorsClothingDeleteUI.active = false;
			MenuSurvivorsClothingDeleteUI.inventory = Glazier.Get().CreateConstraintFrame();
			MenuSurvivorsClothingDeleteUI.inventory.PositionScale_X = 0.5f;
			MenuSurvivorsClothingDeleteUI.inventory.PositionOffset_Y = 10f;
			MenuSurvivorsClothingDeleteUI.inventory.SizeScale_X = 0.5f;
			MenuSurvivorsClothingDeleteUI.inventory.SizeScale_Y = 1f;
			MenuSurvivorsClothingDeleteUI.inventory.SizeOffset_Y = -20f;
			MenuSurvivorsClothingDeleteUI.inventory.Constraint = 1;
			MenuSurvivorsClothingDeleteUI.container.AddChild(MenuSurvivorsClothingDeleteUI.inventory);
			MenuSurvivorsClothingDeleteUI.deleteBox = Glazier.Get().CreateBox();
			MenuSurvivorsClothingDeleteUI.deleteBox.PositionOffset_Y = -65f;
			MenuSurvivorsClothingDeleteUI.deleteBox.PositionScale_Y = 0.5f;
			MenuSurvivorsClothingDeleteUI.deleteBox.SizeOffset_Y = 130f;
			MenuSurvivorsClothingDeleteUI.deleteBox.SizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.inventory.AddChild(MenuSurvivorsClothingDeleteUI.deleteBox);
			MenuSurvivorsClothingDeleteUI.intentLabel = Glazier.Get().CreateLabel();
			MenuSurvivorsClothingDeleteUI.intentLabel.AllowRichText = true;
			MenuSurvivorsClothingDeleteUI.intentLabel.PositionOffset_X = 5f;
			MenuSurvivorsClothingDeleteUI.intentLabel.PositionOffset_Y = 0f;
			MenuSurvivorsClothingDeleteUI.intentLabel.SizeOffset_X = -10f;
			MenuSurvivorsClothingDeleteUI.intentLabel.SizeOffset_Y = 30f;
			MenuSurvivorsClothingDeleteUI.intentLabel.SizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.intentLabel.TextColor = 4;
			MenuSurvivorsClothingDeleteUI.intentLabel.TextContrastContext = 1;
			MenuSurvivorsClothingDeleteUI.deleteBox.AddChild(MenuSurvivorsClothingDeleteUI.intentLabel);
			MenuSurvivorsClothingDeleteUI.warningLabel = Glazier.Get().CreateLabel();
			MenuSurvivorsClothingDeleteUI.warningLabel.PositionOffset_X = 5f;
			MenuSurvivorsClothingDeleteUI.warningLabel.PositionOffset_Y = 20f;
			MenuSurvivorsClothingDeleteUI.warningLabel.SizeOffset_X = -10f;
			MenuSurvivorsClothingDeleteUI.warningLabel.SizeOffset_Y = 30f;
			MenuSurvivorsClothingDeleteUI.warningLabel.SizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.deleteBox.AddChild(MenuSurvivorsClothingDeleteUI.warningLabel);
			MenuSurvivorsClothingDeleteUI.confirmLabel = Glazier.Get().CreateLabel();
			MenuSurvivorsClothingDeleteUI.confirmLabel.PositionOffset_X = 5f;
			MenuSurvivorsClothingDeleteUI.confirmLabel.PositionOffset_Y = 40f;
			MenuSurvivorsClothingDeleteUI.confirmLabel.SizeOffset_X = -10f;
			MenuSurvivorsClothingDeleteUI.confirmLabel.SizeOffset_Y = 30f;
			MenuSurvivorsClothingDeleteUI.confirmLabel.SizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.deleteBox.AddChild(MenuSurvivorsClothingDeleteUI.confirmLabel);
			MenuSurvivorsClothingDeleteUI.confirmField = Glazier.Get().CreateStringField();
			MenuSurvivorsClothingDeleteUI.confirmField.PositionOffset_X = 5f;
			MenuSurvivorsClothingDeleteUI.confirmField.PositionOffset_Y = 75f;
			MenuSurvivorsClothingDeleteUI.confirmField.SizeOffset_X = -150f;
			MenuSurvivorsClothingDeleteUI.confirmField.SizeOffset_Y = 50f;
			MenuSurvivorsClothingDeleteUI.confirmField.SizeScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.confirmField.FontSize = 3;
			MenuSurvivorsClothingDeleteUI.deleteBox.AddChild(MenuSurvivorsClothingDeleteUI.confirmField);
			MenuSurvivorsClothingDeleteUI.yesButton = Glazier.Get().CreateButton();
			MenuSurvivorsClothingDeleteUI.yesButton.PositionOffset_X = -135f;
			MenuSurvivorsClothingDeleteUI.yesButton.PositionOffset_Y = 75f;
			MenuSurvivorsClothingDeleteUI.yesButton.PositionScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.yesButton.SizeOffset_X = 60f;
			MenuSurvivorsClothingDeleteUI.yesButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingDeleteUI.yesButton.FontSize = 3;
			MenuSurvivorsClothingDeleteUI.yesButton.Text = MenuSurvivorsClothingDeleteUI.localization.format("Yes");
			MenuSurvivorsClothingDeleteUI.yesButton.OnClicked += new ClickedButton(MenuSurvivorsClothingDeleteUI.onClickedYesButton);
			MenuSurvivorsClothingDeleteUI.deleteBox.AddChild(MenuSurvivorsClothingDeleteUI.yesButton);
			MenuSurvivorsClothingDeleteUI.noButton = Glazier.Get().CreateButton();
			MenuSurvivorsClothingDeleteUI.noButton.PositionOffset_X = -65f;
			MenuSurvivorsClothingDeleteUI.noButton.PositionOffset_Y = 75f;
			MenuSurvivorsClothingDeleteUI.noButton.PositionScale_X = 1f;
			MenuSurvivorsClothingDeleteUI.noButton.SizeOffset_X = 60f;
			MenuSurvivorsClothingDeleteUI.noButton.SizeOffset_Y = 50f;
			MenuSurvivorsClothingDeleteUI.noButton.FontSize = 3;
			MenuSurvivorsClothingDeleteUI.noButton.Text = MenuSurvivorsClothingDeleteUI.localization.format("No");
			MenuSurvivorsClothingDeleteUI.noButton.TooltipText = MenuSurvivorsClothingDeleteUI.localization.format("No_Tooltip");
			MenuSurvivorsClothingDeleteUI.noButton.OnClicked += new ClickedButton(MenuSurvivorsClothingDeleteUI.onClickedNoButton);
			MenuSurvivorsClothingDeleteUI.deleteBox.AddChild(MenuSurvivorsClothingDeleteUI.noButton);
			MenuSurvivorsClothingDeleteUI.quantityLabel = Glazier.Get().CreateLabel();
			MenuSurvivorsClothingDeleteUI.quantityLabel.PositionOffset_X = 5f;
			MenuSurvivorsClothingDeleteUI.quantityLabel.PositionOffset_Y = -35f;
			MenuSurvivorsClothingDeleteUI.quantityLabel.PositionScale_Y = 1f;
			MenuSurvivorsClothingDeleteUI.quantityLabel.SizeOffset_X = -10f;
			MenuSurvivorsClothingDeleteUI.quantityLabel.SizeOffset_Y = 30f;
			MenuSurvivorsClothingDeleteUI.quantityLabel.SizeScale_X = 0.75f;
			MenuSurvivorsClothingDeleteUI.quantityLabel.TextAlignment = 5;
			MenuSurvivorsClothingDeleteUI.quantityLabel.IsVisible = false;
			MenuSurvivorsClothingDeleteUI.deleteBox.AddChild(MenuSurvivorsClothingDeleteUI.quantityLabel);
			MenuSurvivorsClothingDeleteUI.quantityField = Glazier.Get().CreateUInt16Field();
			MenuSurvivorsClothingDeleteUI.quantityField.PositionOffset_X = 5f;
			MenuSurvivorsClothingDeleteUI.quantityField.PositionOffset_Y = -35f;
			MenuSurvivorsClothingDeleteUI.quantityField.PositionScale_X = 0.75f;
			MenuSurvivorsClothingDeleteUI.quantityField.PositionScale_Y = 1f;
			MenuSurvivorsClothingDeleteUI.quantityField.SizeOffset_X = -10f;
			MenuSurvivorsClothingDeleteUI.quantityField.SizeOffset_Y = 30f;
			MenuSurvivorsClothingDeleteUI.quantityField.SizeScale_X = 0.25f;
			MenuSurvivorsClothingDeleteUI.quantityField.Value = 1;
			MenuSurvivorsClothingDeleteUI.quantityField.MinValue = 1;
			MenuSurvivorsClothingDeleteUI.quantityField.IsVisible = false;
			MenuSurvivorsClothingDeleteUI.deleteBox.AddChild(MenuSurvivorsClothingDeleteUI.quantityField);
		}

		// Token: 0x04002ADC RID: 10972
		private static Local localization;

		// Token: 0x04002ADD RID: 10973
		private static SleekFullscreenBox container;

		// Token: 0x04002ADE RID: 10974
		public static bool active;

		// Token: 0x04002ADF RID: 10975
		private static int item;

		// Token: 0x04002AE0 RID: 10976
		private static ulong instance;

		// Token: 0x04002AE1 RID: 10977
		private static ushort quantity;

		// Token: 0x04002AE2 RID: 10978
		private static EDeleteMode mode;

		// Token: 0x04002AE3 RID: 10979
		private static ulong instigator;

		// Token: 0x04002AE4 RID: 10980
		private static ISleekConstraintFrame inventory;

		// Token: 0x04002AE5 RID: 10981
		private static ISleekBox deleteBox;

		// Token: 0x04002AE6 RID: 10982
		private static ISleekLabel intentLabel;

		// Token: 0x04002AE7 RID: 10983
		private static ISleekLabel warningLabel;

		// Token: 0x04002AE8 RID: 10984
		private static ISleekLabel confirmLabel;

		// Token: 0x04002AE9 RID: 10985
		private static ISleekField confirmField;

		// Token: 0x04002AEA RID: 10986
		private static ISleekButton yesButton;

		// Token: 0x04002AEB RID: 10987
		private static ISleekButton noButton;

		// Token: 0x04002AEC RID: 10988
		private static ISleekLabel quantityLabel;

		// Token: 0x04002AED RID: 10989
		private static ISleekUInt16Field quantityField;
	}
}
