using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007C1 RID: 1985
	public class PlayerDashboardCraftingUI
	{
		// Token: 0x060042C1 RID: 17089 RVA: 0x00171274 File Offset: 0x0016F474
		public static void open()
		{
			if (PlayerDashboardCraftingUI.active)
			{
				return;
			}
			PlayerDashboardCraftingUI.active = true;
			PlayerDashboardCraftingUI.updateSelection(PlayerDashboardCraftingUI.filteredBlueprintsOverride, PlayerDashboardCraftingUI.blueprintTypeFilterIndex, PlayerDashboardCraftingUI.hideUncraftable, PlayerDashboardCraftingUI.itemNameFilter);
			PlayerDashboardCraftingUI.container.AnimateIntoView();
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x001712A7 File Offset: 0x0016F4A7
		public static void close()
		{
			if (!PlayerDashboardCraftingUI.active)
			{
				return;
			}
			PlayerDashboardCraftingUI.active = false;
			PlayerDashboardCraftingUI.filteredBlueprintsOverride = null;
			PlayerDashboardCraftingUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x001712D4 File Offset: 0x0016F4D4
		private static bool DoesAnyItemNameContainString(Blueprint blueprint, string text)
		{
			byte b = 0;
			while ((int)b < blueprint.outputs.Length)
			{
				BlueprintOutput blueprintOutput = blueprint.outputs[(int)b];
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, blueprintOutput.id) as ItemAsset;
				if (itemAsset != null && itemAsset.itemName != null && itemAsset.itemName.IndexOf(text, 5) != -1)
				{
					return true;
				}
				b += 1;
			}
			if (blueprint.tool != 0)
			{
				ItemAsset itemAsset2 = Assets.find(EAssetType.ITEM, blueprint.tool) as ItemAsset;
				if (itemAsset2 != null && itemAsset2.itemName != null && itemAsset2.itemName.IndexOf(text, 5) != -1)
				{
					return true;
				}
			}
			byte b2 = 0;
			while ((int)b2 < blueprint.supplies.Length)
			{
				BlueprintSupply blueprintSupply = blueprint.supplies[(int)b2];
				ItemAsset itemAsset3 = Assets.find(EAssetType.ITEM, blueprintSupply.id) as ItemAsset;
				if (itemAsset3 != null && itemAsset3.itemName != null && itemAsset3.itemName.IndexOf(text, 5) != -1)
				{
					return true;
				}
				b2 += 1;
			}
			return false;
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x001713BC File Offset: 0x0016F5BC
		public static void updateSelection()
		{
			PlayerDashboardCraftingUI.updateSelection(PlayerDashboardCraftingUI.filteredBlueprintsOverride, PlayerDashboardCraftingUI.blueprintTypeFilterIndex, PlayerDashboardCraftingUI.hideUncraftable, PlayerDashboardCraftingUI.itemNameFilter);
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x001713D8 File Offset: 0x0016F5D8
		private static void updateSelection(Blueprint[] newFilteredBlueprintsOverride, byte newBlueprintTypeFilterIndex, bool newHideUncraftable, string newItemNameFilter)
		{
			bool flag = PowerTool.checkFires(Player.player.transform.position, 16f);
			bool flag2 = !string.IsNullOrEmpty(newItemNameFilter);
			List<Blueprint> list;
			if (newFilteredBlueprintsOverride == null)
			{
				list = new List<Blueprint>();
				List<ItemAsset> list2 = new List<ItemAsset>();
				Assets.find<ItemAsset>(list2);
				using (List<ItemAsset>.Enumerator enumerator = list2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ItemAsset itemAsset = enumerator.Current;
						if (itemAsset != null)
						{
							foreach (Blueprint blueprint in itemAsset.blueprints)
							{
								if (flag2 ? PlayerDashboardCraftingUI.DoesAnyItemNameContainString(blueprint, newItemNameFilter) : (blueprint.type == (EBlueprintType)newBlueprintTypeFilterIndex))
								{
									list.Add(blueprint);
								}
							}
						}
					}
					goto IL_C6;
				}
			}
			list = new List<Blueprint>(newFilteredBlueprintsOverride);
			IL_C6:
			if (Level.info != null && Level.info.configData != null && !Level.info.configData.Allow_Crafting)
			{
				newFilteredBlueprintsOverride = new Blueprint[0];
				list.Clear();
			}
			PlayerDashboardCraftingUI.visibleBlueprints.Clear();
			foreach (Blueprint blueprint2 in list)
			{
				if ((blueprint2.skill != EBlueprintSkill.REPAIR || (uint)blueprint2.level <= Provider.modeConfigData.Gameplay.Repair_Level_Max) && (string.IsNullOrEmpty(blueprint2.map) || blueprint2.map.Equals(Level.info.name, 3)) && blueprint2.areConditionsMet(Player.player) && !Player.player.crafting.isBlueprintBlacklisted(blueprint2) && (Provider.modeConfigData.Gameplay.Allow_Freeform_Buildables || Provider.modeConfigData.Gameplay.Allow_Freeform_Buildables_On_Vehicles || !blueprint2.IsOutputFreeformBuildable))
				{
					ItemAsset sourceItem = blueprint2.sourceItem;
					int num = 0;
					bool flag3 = false;
					blueprint2.hasSupplies = true;
					blueprint2.hasSkills = (blueprint2.skill == EBlueprintSkill.NONE || (blueprint2.skill == EBlueprintSkill.CRAFT && Player.player.skills.skills[2][1].level >= blueprint2.level) || (blueprint2.skill == EBlueprintSkill.COOK && flag && Player.player.skills.skills[2][3].level >= blueprint2.level) || (blueprint2.skill == EBlueprintSkill.REPAIR && Player.player.skills.skills[2][7].level >= blueprint2.level));
					List<InventorySearch>[] array = new List<InventorySearch>[blueprint2.supplies.Length];
					for (int i = 0; i < blueprint2.supplies.Length; i++)
					{
						BlueprintSupply blueprintSupply = blueprint2.supplies[i];
						List<InventorySearch> list3 = Player.player.inventory.search(blueprintSupply.id, false, true);
						ushort num2 = 0;
						foreach (InventorySearch inventorySearch in list3)
						{
							num2 += (ushort)inventorySearch.jar.item.amount;
						}
						num += (int)num2;
						blueprintSupply.hasAmount = num2;
						if (blueprint2.type == EBlueprintType.AMMO)
						{
							if (blueprintSupply.hasAmount == 0)
							{
								blueprint2.hasSupplies = false;
								flag3 = true;
							}
						}
						else if (blueprintSupply.hasAmount < blueprintSupply.amount)
						{
							blueprint2.hasSupplies = false;
							flag3 |= blueprintSupply.isCritical;
						}
						array[i] = list3;
					}
					if (blueprint2.tool != 0)
					{
						InventorySearch inventorySearch2 = Player.player.inventory.has(blueprint2.tool);
						blueprint2.tools = ((inventorySearch2 != null) ? 1 : 0);
						blueprint2.hasTool = (inventorySearch2 != null);
						flag3 |= (inventorySearch2 == null && blueprint2.toolCritical);
					}
					else
					{
						blueprint2.tools = 1;
						blueprint2.hasTool = true;
					}
					if (blueprint2.type == EBlueprintType.REPAIR)
					{
						List<InventorySearch> list4 = Player.player.inventory.search(sourceItem.id, false, false);
						byte b = byte.MaxValue;
						int num3 = -1;
						for (int j = 0; j < list4.Count; j++)
						{
							if (list4[j].jar.item.quality < b)
							{
								b = list4[j].jar.item.quality;
								num3 = j;
							}
						}
						if (num3 >= 0)
						{
							blueprint2.items = (ushort)list4[num3].jar.item.quality;
							num++;
						}
						else
						{
							blueprint2.items = 0;
						}
						blueprint2.hasItem = (num3 >= 0);
					}
					else if (blueprint2.type == EBlueprintType.AMMO)
					{
						List<InventorySearch> list5 = Player.player.inventory.search(sourceItem.id, true, true);
						int num4 = -1;
						int num5 = -1;
						for (int k = 0; k < list5.Count; k++)
						{
							if ((int)list5[k].jar.item.amount > num4 && list5[k].jar.item.amount < sourceItem.amount)
							{
								num4 = (int)list5[k].jar.item.amount;
								num5 = k;
							}
						}
						if (num5 >= 0)
						{
							if (list5[num5].jar.item.id == blueprint2.supplies[0].id)
							{
								BlueprintSupply blueprintSupply2 = blueprint2.supplies[0];
								blueprintSupply2.hasAmount -= (ushort)num4;
							}
							blueprint2.supplies[0].amount = (ushort)((byte)((int)sourceItem.amount - num4));
							blueprint2.items = (ushort)list5[num5].jar.item.amount;
							num++;
						}
						else
						{
							blueprint2.supplies[0].amount = 0;
							blueprint2.items = 0;
						}
						blueprint2.hasItem = (num5 >= 0);
						if (num5 < 0)
						{
							blueprint2.products = 0;
						}
						else if (blueprint2.items + blueprint2.supplies[0].hasAmount > (ushort)sourceItem.amount)
						{
							blueprint2.products = (ushort)sourceItem.amount;
						}
						else
						{
							blueprint2.products = blueprint2.items + blueprint2.supplies[0].hasAmount;
						}
					}
					else
					{
						blueprint2.hasItem = true;
					}
					if (!flag3 || (flag2 && blueprint2.canBeVisibleWhenSearchedWithoutRequiredItems))
					{
						if (newHideUncraftable)
						{
							bool ignoringBlueprint = Player.player.crafting.getIgnoringBlueprint(blueprint2);
							if (blueprint2.hasSupplies && blueprint2.hasTool && blueprint2.hasItem && blueprint2.hasSkills && !ignoringBlueprint)
							{
								PlayerDashboardCraftingUI.visibleBlueprints.Add(blueprint2);
							}
						}
						else if (newFilteredBlueprintsOverride != null)
						{
							if (blueprint2.hasSupplies && blueprint2.hasTool && blueprint2.hasItem && blueprint2.hasSkills)
							{
								PlayerDashboardCraftingUI.visibleBlueprints.Insert(0, blueprint2);
							}
							else
							{
								PlayerDashboardCraftingUI.visibleBlueprints.Add(blueprint2);
							}
						}
						else if (blueprint2.hasSupplies && blueprint2.hasTool && blueprint2.hasItem && blueprint2.hasSkills)
						{
							PlayerDashboardCraftingUI.visibleBlueprints.Insert(0, blueprint2);
						}
						else if ((flag2 && blueprint2.canBeVisibleWhenSearchedWithoutRequiredItems) || ((blueprint2.type == EBlueprintType.AMMO || blueprint2.type == EBlueprintType.REPAIR || num != 0) && blueprint2.hasItem))
						{
							PlayerDashboardCraftingUI.visibleBlueprints.Add(blueprint2);
						}
					}
				}
			}
			PlayerDashboardCraftingUI.filteredBlueprintsOverride = newFilteredBlueprintsOverride;
			PlayerDashboardCraftingUI.blueprintTypeFilterIndex = newBlueprintTypeFilterIndex;
			PlayerDashboardCraftingUI.hideUncraftable = newHideUncraftable;
			PlayerDashboardCraftingUI.itemNameFilter = newItemNameFilter;
			PlayerDashboardCraftingUI.blueprintsScrollBox.ForceRebuildElements();
			PlayerDashboardCraftingUI.infoBox.IsVisible = (PlayerDashboardCraftingUI.visibleBlueprints.Count == 0);
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x00171C0C File Offset: 0x0016FE0C
		private static void onInventoryResized(byte page, byte newWidth, byte newHeight)
		{
			if (PlayerDashboardCraftingUI.active)
			{
				PlayerDashboardCraftingUI.updateSelection();
			}
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x00171C1A File Offset: 0x0016FE1A
		private static void onCraftingUpdated()
		{
			if (PlayerDashboardCraftingUI.active)
			{
				PlayerDashboardCraftingUI.updateSelection();
			}
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x00171C28 File Offset: 0x0016FE28
		private static void onClickedTypeButton(ISleekElement button)
		{
			byte newBlueprintTypeFilterIndex = (byte)((button.PositionOffset_X + (float)(-(float)(PlayerDashboardCraftingUI.TYPES * -30 + 5))) / 60f);
			PlayerDashboardCraftingUI.searchField.Text = "";
			PlayerDashboardCraftingUI.updateSelection(null, newBlueprintTypeFilterIndex, PlayerDashboardCraftingUI.hideUncraftable, string.Empty);
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x00171C70 File Offset: 0x0016FE70
		private static void onToggledHideUncraftableToggle(ISleekToggle toggle, bool state)
		{
			PlayerDashboardCraftingUI.updateSelection(PlayerDashboardCraftingUI.filteredBlueprintsOverride, PlayerDashboardCraftingUI.blueprintTypeFilterIndex, state, PlayerDashboardCraftingUI.itemNameFilter);
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x00171C87 File Offset: 0x0016FE87
		private static void onEnteredSearchField(ISleekField field)
		{
			PlayerDashboardCraftingUI.updateSelection(null, PlayerDashboardCraftingUI.blueprintTypeFilterIndex, PlayerDashboardCraftingUI.hideUncraftable, PlayerDashboardCraftingUI.searchField.Text);
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x00171CA3 File Offset: 0x0016FEA3
		private static void onClickedSearchButton(ISleekElement button)
		{
			PlayerDashboardCraftingUI.updateSelection(null, PlayerDashboardCraftingUI.blueprintTypeFilterIndex, PlayerDashboardCraftingUI.hideUncraftable, PlayerDashboardCraftingUI.searchField.Text);
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x00171CC0 File Offset: 0x0016FEC0
		private static void clickedBlueprint(Blueprint blueprint, bool all)
		{
			if (!blueprint.hasSupplies)
			{
				return;
			}
			if (!blueprint.hasTool)
			{
				return;
			}
			if (!blueprint.hasItem)
			{
				return;
			}
			if (!blueprint.hasSkills)
			{
				return;
			}
			if (Player.player.equipment.isBusy)
			{
				return;
			}
			Player.player.crafting.sendCraft(blueprint.sourceItem.id, blueprint.id, all);
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x00171D24 File Offset: 0x0016FF24
		private static void onClickedBlueprintButton(Blueprint blueprint)
		{
			PlayerDashboardCraftingUI.clickedBlueprint(blueprint, InputEx.GetKey(ControlsSettings.other));
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x00171D36 File Offset: 0x0016FF36
		private static void onClickedBlueprintCraftAllButton(Blueprint blueprint)
		{
			PlayerDashboardCraftingUI.clickedBlueprint(blueprint, true);
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x00171D3F File Offset: 0x0016FF3F
		private static ISleekElement onCreateBlueprint(Blueprint blueprint)
		{
			SleekBlueprint sleekBlueprint = new SleekBlueprint(blueprint);
			sleekBlueprint.onClickedCraftButton += PlayerDashboardCraftingUI.onClickedBlueprintButton;
			sleekBlueprint.onClickedCraftAllButton += PlayerDashboardCraftingUI.onClickedBlueprintCraftAllButton;
			return sleekBlueprint;
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x00171D6C File Offset: 0x0016FF6C
		public PlayerDashboardCraftingUI()
		{
			if (PlayerDashboardCraftingUI.icons != null)
			{
				PlayerDashboardCraftingUI.icons.unload();
			}
			PlayerDashboardCraftingUI.localization = Localization.read("/Player/PlayerDashboardCrafting.dat");
			PlayerDashboardCraftingUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboardCrafting/PlayerDashboardCrafting.unity3d");
			PlayerDashboardCraftingUI.container = new SleekFullscreenBox();
			PlayerDashboardCraftingUI.container.PositionScale_Y = 1f;
			PlayerDashboardCraftingUI.container.PositionOffset_X = 10f;
			PlayerDashboardCraftingUI.container.PositionOffset_Y = 10f;
			PlayerDashboardCraftingUI.container.SizeOffset_X = -20f;
			PlayerDashboardCraftingUI.container.SizeOffset_Y = -20f;
			PlayerDashboardCraftingUI.container.SizeScale_X = 1f;
			PlayerDashboardCraftingUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerDashboardCraftingUI.container);
			PlayerDashboardCraftingUI.active = false;
			PlayerDashboardCraftingUI.blueprintTypeFilterIndex = byte.MaxValue;
			PlayerDashboardCraftingUI.hideUncraftable = false;
			PlayerDashboardCraftingUI.itemNameFilter = string.Empty;
			PlayerDashboardCraftingUI.backdropBox = Glazier.Get().CreateBox();
			PlayerDashboardCraftingUI.backdropBox.PositionOffset_Y = 60f;
			PlayerDashboardCraftingUI.backdropBox.SizeOffset_Y = -60f;
			PlayerDashboardCraftingUI.backdropBox.SizeScale_X = 1f;
			PlayerDashboardCraftingUI.backdropBox.SizeScale_Y = 1f;
			PlayerDashboardCraftingUI.backdropBox.BackgroundColor = new SleekColor(1, 0.5f);
			PlayerDashboardCraftingUI.container.AddChild(PlayerDashboardCraftingUI.backdropBox);
			PlayerDashboardCraftingUI.visibleBlueprints = new List<Blueprint>();
			PlayerDashboardCraftingUI.blueprintsScrollBox = new SleekList<Blueprint>();
			PlayerDashboardCraftingUI.blueprintsScrollBox.PositionOffset_X = 10f;
			PlayerDashboardCraftingUI.blueprintsScrollBox.PositionOffset_Y = 110f;
			PlayerDashboardCraftingUI.blueprintsScrollBox.SizeOffset_X = -20f;
			PlayerDashboardCraftingUI.blueprintsScrollBox.SizeOffset_Y = -120f;
			PlayerDashboardCraftingUI.blueprintsScrollBox.SizeScale_X = 1f;
			PlayerDashboardCraftingUI.blueprintsScrollBox.SizeScale_Y = 1f;
			PlayerDashboardCraftingUI.blueprintsScrollBox.itemHeight = 195;
			PlayerDashboardCraftingUI.blueprintsScrollBox.itemPadding = 10;
			PlayerDashboardCraftingUI.blueprintsScrollBox.onCreateElement = new SleekList<Blueprint>.CreateElement(PlayerDashboardCraftingUI.onCreateBlueprint);
			PlayerDashboardCraftingUI.blueprintsScrollBox.SetData(PlayerDashboardCraftingUI.visibleBlueprints);
			PlayerDashboardCraftingUI.backdropBox.AddChild(PlayerDashboardCraftingUI.blueprintsScrollBox);
			for (int i = 0; i < PlayerDashboardCraftingUI.TYPES; i++)
			{
				SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(PlayerDashboardCraftingUI.icons.load<Texture2D>("Blueprint_" + i.ToString()));
				sleekButtonIcon.PositionOffset_X = (float)(PlayerDashboardCraftingUI.TYPES * -30 + 5 + i * 60);
				sleekButtonIcon.PositionOffset_Y = 10f;
				sleekButtonIcon.PositionScale_X = 0.5f;
				sleekButtonIcon.SizeOffset_X = 50f;
				sleekButtonIcon.SizeOffset_Y = 50f;
				sleekButtonIcon.tooltip = PlayerDashboardCraftingUI.localization.format("Type_" + i.ToString() + "_Tooltip");
				sleekButtonIcon.iconColor = 2;
				sleekButtonIcon.onClickedButton += new ClickedButton(PlayerDashboardCraftingUI.onClickedTypeButton);
				PlayerDashboardCraftingUI.backdropBox.AddChild(sleekButtonIcon);
			}
			PlayerDashboardCraftingUI.hideUncraftableToggle = Glazier.Get().CreateToggle();
			PlayerDashboardCraftingUI.hideUncraftableToggle.PositionOffset_X = -80f;
			PlayerDashboardCraftingUI.hideUncraftableToggle.PositionOffset_Y = 65f;
			PlayerDashboardCraftingUI.hideUncraftableToggle.PositionScale_X = 1f;
			PlayerDashboardCraftingUI.hideUncraftableToggle.SizeOffset_X = 40f;
			PlayerDashboardCraftingUI.hideUncraftableToggle.SizeOffset_Y = 40f;
			PlayerDashboardCraftingUI.hideUncraftableToggle.AddLabel(PlayerDashboardCraftingUI.localization.format("Hide_Uncraftable_Toggle_Label"), 0);
			PlayerDashboardCraftingUI.hideUncraftableToggle.Value = PlayerDashboardCraftingUI.hideUncraftable;
			PlayerDashboardCraftingUI.hideUncraftableToggle.OnValueChanged += new Toggled(PlayerDashboardCraftingUI.onToggledHideUncraftableToggle);
			PlayerDashboardCraftingUI.backdropBox.AddChild(PlayerDashboardCraftingUI.hideUncraftableToggle);
			PlayerDashboardCraftingUI.searchField = Glazier.Get().CreateStringField();
			PlayerDashboardCraftingUI.searchField.PositionOffset_X = 10f;
			PlayerDashboardCraftingUI.searchField.PositionOffset_Y = 70f;
			PlayerDashboardCraftingUI.searchField.SizeOffset_X = -410f;
			PlayerDashboardCraftingUI.searchField.SizeOffset_Y = 30f;
			PlayerDashboardCraftingUI.searchField.SizeScale_X = 1f;
			PlayerDashboardCraftingUI.searchField.PlaceholderText = PlayerDashboardCraftingUI.localization.format("Search_Field_Hint");
			PlayerDashboardCraftingUI.searchField.OnTextSubmitted += new Entered(PlayerDashboardCraftingUI.onEnteredSearchField);
			PlayerDashboardCraftingUI.backdropBox.AddChild(PlayerDashboardCraftingUI.searchField);
			PlayerDashboardCraftingUI.searchButton = Glazier.Get().CreateButton();
			PlayerDashboardCraftingUI.searchButton.PositionOffset_X = -390f;
			PlayerDashboardCraftingUI.searchButton.PositionOffset_Y = 70f;
			PlayerDashboardCraftingUI.searchButton.PositionScale_X = 1f;
			PlayerDashboardCraftingUI.searchButton.SizeOffset_X = 100f;
			PlayerDashboardCraftingUI.searchButton.SizeOffset_Y = 30f;
			PlayerDashboardCraftingUI.searchButton.Text = PlayerDashboardCraftingUI.localization.format("Search");
			PlayerDashboardCraftingUI.searchButton.TooltipText = PlayerDashboardCraftingUI.localization.format("Search_Tooltip");
			PlayerDashboardCraftingUI.searchButton.OnClicked += new ClickedButton(PlayerDashboardCraftingUI.onClickedSearchButton);
			PlayerDashboardCraftingUI.backdropBox.AddChild(PlayerDashboardCraftingUI.searchButton);
			PlayerDashboardCraftingUI.infoBox = Glazier.Get().CreateBox();
			PlayerDashboardCraftingUI.infoBox.PositionOffset_X = 10f;
			PlayerDashboardCraftingUI.infoBox.PositionOffset_Y = 110f;
			PlayerDashboardCraftingUI.infoBox.SizeOffset_X = -20f;
			PlayerDashboardCraftingUI.infoBox.SizeOffset_Y = 50f;
			PlayerDashboardCraftingUI.infoBox.SizeScale_X = 1f;
			PlayerDashboardCraftingUI.infoBox.Text = PlayerDashboardCraftingUI.localization.format("No_Blueprints");
			PlayerDashboardCraftingUI.infoBox.FontSize = 3;
			PlayerDashboardCraftingUI.backdropBox.AddChild(PlayerDashboardCraftingUI.infoBox);
			PlayerDashboardCraftingUI.infoBox.IsVisible = false;
			PlayerDashboardCraftingUI.filteredBlueprintsOverride = null;
			PlayerDashboardCraftingUI.blueprintTypeFilterIndex = 0;
			PlayerDashboardCraftingUI.hideUncraftable = false;
			PlayerDashboardCraftingUI.itemNameFilter = string.Empty;
			PlayerInventory inventory = Player.player.inventory;
			inventory.onInventoryResized = (InventoryResized)Delegate.Combine(inventory.onInventoryResized, new InventoryResized(PlayerDashboardCraftingUI.onInventoryResized));
			PlayerCrafting crafting = Player.player.crafting;
			crafting.onCraftingUpdated = (CraftingUpdated)Delegate.Combine(crafting.onCraftingUpdated, new CraftingUpdated(PlayerDashboardCraftingUI.onCraftingUpdated));
		}

		// Token: 0x04002C00 RID: 11264
		private static readonly int TYPES = 10;

		// Token: 0x04002C01 RID: 11265
		public static Local localization;

		// Token: 0x04002C02 RID: 11266
		private static SleekFullscreenBox container;

		// Token: 0x04002C03 RID: 11267
		public static Bundle icons;

		// Token: 0x04002C04 RID: 11268
		public static bool active;

		// Token: 0x04002C05 RID: 11269
		private static ISleekBox backdropBox;

		// Token: 0x04002C06 RID: 11270
		private static ISleekField searchField;

		// Token: 0x04002C07 RID: 11271
		private static ISleekButton searchButton;

		// Token: 0x04002C08 RID: 11272
		private static List<Blueprint> visibleBlueprints;

		// Token: 0x04002C09 RID: 11273
		private static SleekList<Blueprint> blueprintsScrollBox;

		// Token: 0x04002C0A RID: 11274
		private static ISleekBox infoBox;

		// Token: 0x04002C0B RID: 11275
		private static ISleekToggle hideUncraftableToggle;

		/// <summary>
		/// Used by inventory item context menu to override which blueprints are shown.
		/// </summary>
		// Token: 0x04002C0C RID: 11276
		public static Blueprint[] filteredBlueprintsOverride;

		// Token: 0x04002C0D RID: 11277
		private static byte blueprintTypeFilterIndex;

		// Token: 0x04002C0E RID: 11278
		private static bool hideUncraftable;

		// Token: 0x04002C0F RID: 11279
		private static string itemNameFilter;
	}
}
