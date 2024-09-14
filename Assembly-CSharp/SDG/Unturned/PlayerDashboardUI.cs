using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007C5 RID: 1989
	public class PlayerDashboardUI
	{
		// Token: 0x0600435D RID: 17245 RVA: 0x0017BBA8 File Offset: 0x00179DA8
		public static void open()
		{
			if (PlayerDashboardUI.active)
			{
				return;
			}
			PlayerDashboardUI.active = true;
			if (PlayerDashboardInventoryUI.active)
			{
				PlayerDashboardInventoryUI.active = false;
				PlayerDashboardInventoryUI.open();
			}
			else if (PlayerDashboardCraftingUI.active)
			{
				PlayerDashboardCraftingUI.active = false;
				PlayerDashboardCraftingUI.open();
			}
			else if (PlayerDashboardSkillsUI.active)
			{
				PlayerDashboardSkillsUI.active = false;
				PlayerDashboardSkillsUI.open();
			}
			else if (PlayerDashboardInformationUI.active)
			{
				PlayerDashboardInformationUI.active = false;
				PlayerDashboardInformationUI.open();
			}
			PlayerDashboardUI.container.AnimateIntoView();
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0017BC1C File Offset: 0x00179E1C
		public static void close()
		{
			if (!PlayerDashboardUI.active)
			{
				return;
			}
			PlayerDashboardUI.active = false;
			if (PlayerDashboardInventoryUI.active)
			{
				PlayerDashboardInventoryUI.close();
				PlayerDashboardInventoryUI.active = true;
			}
			else if (PlayerDashboardCraftingUI.active)
			{
				PlayerDashboardCraftingUI.close();
				PlayerDashboardCraftingUI.active = true;
			}
			else if (PlayerDashboardSkillsUI.active)
			{
				PlayerDashboardSkillsUI.close();
				PlayerDashboardSkillsUI.active = true;
			}
			else if (PlayerDashboardInformationUI.active)
			{
				PlayerDashboardInformationUI.close();
				PlayerDashboardInformationUI.active = true;
			}
			PlayerDashboardUI.container.AnimateOutOfView(0f, -1f);
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x0017BC99 File Offset: 0x00179E99
		private static void onClickedInventoryButton(ISleekElement button)
		{
			PlayerDashboardCraftingUI.close();
			PlayerDashboardSkillsUI.close();
			PlayerDashboardInformationUI.close();
			if (PlayerDashboardInventoryUI.active)
			{
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
				return;
			}
			PlayerDashboardInventoryUI.open();
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x0017BCC1 File Offset: 0x00179EC1
		private static void onClickedCraftingButton(ISleekElement button)
		{
			PlayerDashboardInventoryUI.close();
			PlayerDashboardSkillsUI.close();
			PlayerDashboardInformationUI.close();
			if (PlayerDashboardCraftingUI.active)
			{
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
				return;
			}
			PlayerDashboardCraftingUI.open();
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x0017BCE9 File Offset: 0x00179EE9
		private static void onClickedSkillsButton(ISleekElement button)
		{
			PlayerDashboardInventoryUI.close();
			PlayerDashboardCraftingUI.close();
			PlayerDashboardInformationUI.close();
			if (PlayerDashboardSkillsUI.active)
			{
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
				return;
			}
			PlayerDashboardSkillsUI.open();
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x0017BD11 File Offset: 0x00179F11
		private static void onClickedInformationButton(ISleekElement button)
		{
			PlayerDashboardInventoryUI.close();
			PlayerDashboardCraftingUI.close();
			PlayerDashboardSkillsUI.close();
			if (PlayerDashboardInformationUI.active)
			{
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
				return;
			}
			PlayerDashboardInformationUI.open();
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x0017BD3C File Offset: 0x00179F3C
		private void createDisabledLabel(SleekButtonIcon parentButton, Local localization)
		{
			parentButton.isClickable = false;
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_X = parentButton.PositionOffset_X;
			sleekLabel.PositionScale_X = parentButton.PositionScale_X;
			sleekLabel.SizeOffset_X = -parentButton.SizeOffset_X;
			sleekLabel.SizeOffset_Y = parentButton.SizeOffset_Y;
			sleekLabel.SizeScale_X = parentButton.SizeScale_X;
			sleekLabel.Text = localization.format("Crafting_Disabled");
			sleekLabel.TextColor = 6;
			sleekLabel.FontSize = 4;
			sleekLabel.TextContrastContext = 1;
			PlayerDashboardUI.container.AddChild(sleekLabel);
		}

		/// <summary>
		/// Temporary to unbind events because this class is static for now. (sigh)
		/// </summary>
		// Token: 0x06004364 RID: 17252 RVA: 0x0017BDCE File Offset: 0x00179FCE
		public void OnDestroy()
		{
			this.infoUI.OnDestroy();
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x0017BDDC File Offset: 0x00179FDC
		public PlayerDashboardUI()
		{
			Local local = Localization.read("/Player/PlayerDashboard.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboard/PlayerDashboard.unity3d");
			PlayerDashboardUI.container = new SleekFullscreenBox();
			PlayerDashboardUI.container.PositionScale_Y = -1f;
			PlayerDashboardUI.container.PositionOffset_X = 10f;
			PlayerDashboardUI.container.PositionOffset_Y = 10f;
			PlayerDashboardUI.container.SizeOffset_X = -20f;
			PlayerDashboardUI.container.SizeOffset_Y = -20f;
			PlayerDashboardUI.container.SizeScale_X = 1f;
			PlayerDashboardUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerDashboardUI.container);
			PlayerDashboardUI.active = false;
			PlayerDashboardUI.inventoryButton = new SleekButtonIcon(bundle.load<Texture2D>("Inventory"));
			PlayerDashboardUI.inventoryButton.SizeOffset_X = -5f;
			PlayerDashboardUI.inventoryButton.SizeOffset_Y = 50f;
			PlayerDashboardUI.inventoryButton.SizeScale_X = 0.25f;
			PlayerDashboardUI.inventoryButton.text = local.format("Inventory", ControlsSettings.inventory);
			PlayerDashboardUI.inventoryButton.tooltip = local.format("Inventory_Tooltip");
			PlayerDashboardUI.inventoryButton.onClickedButton += new ClickedButton(PlayerDashboardUI.onClickedInventoryButton);
			PlayerDashboardUI.inventoryButton.fontSize = 3;
			PlayerDashboardUI.inventoryButton.iconColor = 2;
			PlayerDashboardUI.container.AddChild(PlayerDashboardUI.inventoryButton);
			PlayerDashboardUI.craftingButton = new SleekButtonIcon(bundle.load<Texture2D>("Crafting"));
			PlayerDashboardUI.craftingButton.PositionOffset_X = 5f;
			PlayerDashboardUI.craftingButton.PositionScale_X = 0.25f;
			PlayerDashboardUI.craftingButton.SizeOffset_X = -10f;
			PlayerDashboardUI.craftingButton.SizeOffset_Y = 50f;
			PlayerDashboardUI.craftingButton.SizeScale_X = 0.25f;
			PlayerDashboardUI.craftingButton.text = local.format("Crafting", ControlsSettings.crafting);
			PlayerDashboardUI.craftingButton.tooltip = local.format("Crafting_Tooltip");
			PlayerDashboardUI.craftingButton.iconColor = 2;
			PlayerDashboardUI.craftingButton.fontSize = 3;
			PlayerDashboardUI.container.AddChild(PlayerDashboardUI.craftingButton);
			if (Level.info != null && !Level.info.configData.Allow_Crafting)
			{
				this.createDisabledLabel(PlayerDashboardUI.craftingButton, local);
			}
			else
			{
				PlayerDashboardUI.craftingButton.onClickedButton += new ClickedButton(PlayerDashboardUI.onClickedCraftingButton);
			}
			PlayerDashboardUI.skillsButton = new SleekButtonIcon(bundle.load<Texture2D>("Skills"));
			PlayerDashboardUI.skillsButton.PositionOffset_X = 5f;
			PlayerDashboardUI.skillsButton.PositionScale_X = 0.5f;
			PlayerDashboardUI.skillsButton.SizeOffset_X = -10f;
			PlayerDashboardUI.skillsButton.SizeOffset_Y = 50f;
			PlayerDashboardUI.skillsButton.SizeScale_X = 0.25f;
			PlayerDashboardUI.skillsButton.text = local.format("Skills", ControlsSettings.skills);
			PlayerDashboardUI.skillsButton.tooltip = local.format("Skills_Tooltip");
			PlayerDashboardUI.skillsButton.iconColor = 2;
			PlayerDashboardUI.skillsButton.fontSize = 3;
			PlayerDashboardUI.container.AddChild(PlayerDashboardUI.skillsButton);
			if (Level.info != null && !Level.info.configData.Allow_Skills)
			{
				this.createDisabledLabel(PlayerDashboardUI.skillsButton, local);
			}
			else
			{
				PlayerDashboardUI.skillsButton.onClickedButton += new ClickedButton(PlayerDashboardUI.onClickedSkillsButton);
			}
			PlayerDashboardUI.informationButton = new SleekButtonIcon(bundle.load<Texture2D>("Information"));
			PlayerDashboardUI.informationButton.PositionOffset_X = 5f;
			PlayerDashboardUI.informationButton.PositionScale_X = 0.75f;
			PlayerDashboardUI.informationButton.SizeOffset_X = -5f;
			PlayerDashboardUI.informationButton.SizeOffset_Y = 50f;
			PlayerDashboardUI.informationButton.SizeScale_X = 0.25f;
			PlayerDashboardUI.informationButton.text = local.format("Information", ControlsSettings.map);
			PlayerDashboardUI.informationButton.tooltip = local.format("Information_Tooltip");
			PlayerDashboardUI.informationButton.iconColor = 2;
			PlayerDashboardUI.informationButton.fontSize = 3;
			PlayerDashboardUI.container.AddChild(PlayerDashboardUI.informationButton);
			if (Level.info != null && !Level.info.configData.Allow_Information)
			{
				this.createDisabledLabel(PlayerDashboardUI.informationButton, local);
			}
			else
			{
				PlayerDashboardUI.informationButton.onClickedButton += new ClickedButton(PlayerDashboardUI.onClickedInformationButton);
			}
			if (Level.info != null && Level.info.type == ELevelType.HORDE)
			{
				PlayerDashboardUI.inventoryButton.SizeScale_X = 0.5f;
				PlayerDashboardUI.craftingButton.IsVisible = false;
				PlayerDashboardUI.skillsButton.IsVisible = false;
				PlayerDashboardUI.informationButton.PositionScale_X = 0.5f;
				PlayerDashboardUI.informationButton.SizeScale_X = 0.5f;
			}
			bundle.unload();
			new PlayerDashboardInventoryUI();
			new PlayerDashboardCraftingUI();
			new PlayerDashboardSkillsUI();
			this.infoUI = new PlayerDashboardInformationUI();
		}

		// Token: 0x04002C95 RID: 11413
		public static SleekFullscreenBox container;

		// Token: 0x04002C96 RID: 11414
		public static bool active;

		// Token: 0x04002C97 RID: 11415
		private static SleekButtonIcon inventoryButton;

		// Token: 0x04002C98 RID: 11416
		private static SleekButtonIcon craftingButton;

		// Token: 0x04002C99 RID: 11417
		private static SleekButtonIcon skillsButton;

		// Token: 0x04002C9A RID: 11418
		private static SleekButtonIcon informationButton;

		// Token: 0x04002C9B RID: 11419
		private PlayerDashboardInformationUI infoUI;
	}
}
