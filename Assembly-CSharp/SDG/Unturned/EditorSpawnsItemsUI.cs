using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200077D RID: 1917
	public class EditorSpawnsItemsUI
	{
		// Token: 0x06003EEB RID: 16107 RVA: 0x00137725 File Offset: 0x00135925
		public static void open()
		{
			if (EditorSpawnsItemsUI.active)
			{
				return;
			}
			EditorSpawnsItemsUI.active = true;
			EditorSpawns.isSpawning = true;
			EditorSpawns.spawnMode = ESpawnMode.ADD_ITEM;
			EditorSpawnsItemsUI.container.AnimateIntoView();
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x0013774B File Offset: 0x0013594B
		public static void close()
		{
			if (!EditorSpawnsItemsUI.active)
			{
				return;
			}
			EditorSpawnsItemsUI.active = false;
			EditorSpawns.isSpawning = false;
			EditorSpawnsItemsUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x00137778 File Offset: 0x00135978
		public static void updateTables()
		{
			if (EditorSpawnsItemsUI.tableButtons != null)
			{
				for (int i = 0; i < EditorSpawnsItemsUI.tableButtons.Length; i++)
				{
					EditorSpawnsItemsUI.tableScrollBox.RemoveChild(EditorSpawnsItemsUI.tableButtons[i]);
				}
			}
			EditorSpawnsItemsUI.tableButtons = new ISleekButton[LevelItems.tables.Count];
			EditorSpawnsItemsUI.tableScrollBox.ContentSizeOffset = new Vector2(0f, (float)(EditorSpawnsItemsUI.tableButtons.Length * 40 - 10));
			for (int j = 0; j < EditorSpawnsItemsUI.tableButtons.Length; j++)
			{
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_X = 240f;
				sleekButton.PositionOffset_Y = (float)(j * 40);
				sleekButton.SizeOffset_X = 200f;
				sleekButton.SizeOffset_Y = 30f;
				sleekButton.Text = j.ToString() + " " + LevelItems.tables[j].name;
				sleekButton.OnClicked += new ClickedButton(EditorSpawnsItemsUI.onClickedTableButton);
				EditorSpawnsItemsUI.tableScrollBox.AddChild(sleekButton);
				EditorSpawnsItemsUI.tableButtons[j] = sleekButton;
			}
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x00137880 File Offset: 0x00135A80
		public static void updateSelection()
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				ItemTable itemTable = LevelItems.tables[(int)EditorSpawns.selectedItem];
				EditorSpawnsItemsUI.selectedBox.Text = itemTable.name;
				EditorSpawnsItemsUI.tableNameField.Text = itemTable.name;
				EditorSpawnsItemsUI.tableIDField.Value = itemTable.tableID;
				EditorSpawnsItemsUI.tableColorPicker.state = itemTable.color;
				if (EditorSpawnsItemsUI.tierButtons != null)
				{
					for (int i = 0; i < EditorSpawnsItemsUI.tierButtons.Length; i++)
					{
						EditorSpawnsItemsUI.spawnsScrollBox.RemoveChild(EditorSpawnsItemsUI.tierButtons[i]);
					}
				}
				EditorSpawnsItemsUI.tierButtons = new ISleekButton[itemTable.tiers.Count];
				for (int j = 0; j < EditorSpawnsItemsUI.tierButtons.Length; j++)
				{
					ItemTier itemTier = itemTable.tiers[j];
					ISleekButton sleekButton = Glazier.Get().CreateButton();
					sleekButton.PositionOffset_X = 240f;
					sleekButton.PositionOffset_Y = (float)(170 + j * 70);
					sleekButton.SizeOffset_X = 200f;
					sleekButton.SizeOffset_Y = 30f;
					sleekButton.Text = itemTier.name;
					sleekButton.OnClicked += new ClickedButton(EditorSpawnsItemsUI.onClickedTierButton);
					EditorSpawnsItemsUI.spawnsScrollBox.AddChild(sleekButton);
					ISleekSlider sleekSlider = Glazier.Get().CreateSlider();
					sleekSlider.PositionOffset_Y = 40f;
					sleekSlider.SizeOffset_X = 200f;
					sleekSlider.SizeOffset_Y = 20f;
					sleekSlider.Orientation = 0;
					sleekSlider.Value = itemTier.chance;
					sleekSlider.AddLabel(Mathf.RoundToInt(itemTier.chance * 100f).ToString() + "%", 0);
					sleekSlider.OnValueChanged += new Dragged(EditorSpawnsItemsUI.onDraggedChanceSlider);
					sleekButton.AddChild(sleekSlider);
					EditorSpawnsItemsUI.tierButtons[j] = sleekButton;
				}
				EditorSpawnsItemsUI.tierNameField.PositionOffset_Y = (float)(170 + EditorSpawnsItemsUI.tierButtons.Length * 70);
				EditorSpawnsItemsUI.addTierButton.PositionOffset_Y = (float)(170 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 40);
				EditorSpawnsItemsUI.removeTierButton.PositionOffset_Y = (float)(170 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 40);
				if (EditorSpawnsItemsUI.itemButtons != null)
				{
					for (int k = 0; k < EditorSpawnsItemsUI.itemButtons.Length; k++)
					{
						EditorSpawnsItemsUI.spawnsScrollBox.RemoveChild(EditorSpawnsItemsUI.itemButtons[k]);
					}
				}
				if ((int)EditorSpawnsItemsUI.selectedTier < itemTable.tiers.Count)
				{
					EditorSpawnsItemsUI.tierNameField.Text = itemTable.tiers[(int)EditorSpawnsItemsUI.selectedTier].name;
					EditorSpawnsItemsUI.itemButtons = new ISleekButton[itemTable.tiers[(int)EditorSpawnsItemsUI.selectedTier].table.Count];
					for (int l = 0; l < EditorSpawnsItemsUI.itemButtons.Length; l++)
					{
						ISleekButton sleekButton2 = Glazier.Get().CreateButton();
						sleekButton2.PositionOffset_X = 240f;
						sleekButton2.PositionOffset_Y = (float)(170 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 80 + l * 40);
						sleekButton2.SizeOffset_X = 200f;
						sleekButton2.SizeOffset_Y = 30f;
						ItemAsset itemAsset = Assets.find(EAssetType.ITEM, itemTable.tiers[(int)EditorSpawnsItemsUI.selectedTier].table[l].item) as ItemAsset;
						string text = "?";
						if (itemAsset != null)
						{
							if (string.IsNullOrEmpty(itemAsset.itemName))
							{
								text = itemAsset.name;
							}
							else
							{
								text = itemAsset.itemName;
							}
						}
						sleekButton2.Text = itemTable.tiers[(int)EditorSpawnsItemsUI.selectedTier].table[l].item.ToString() + " " + text;
						sleekButton2.OnClicked += new ClickedButton(EditorSpawnsItemsUI.onClickItemButton);
						EditorSpawnsItemsUI.spawnsScrollBox.AddChild(sleekButton2);
						EditorSpawnsItemsUI.itemButtons[l] = sleekButton2;
					}
				}
				else
				{
					EditorSpawnsItemsUI.tierNameField.Text = "";
					EditorSpawnsItemsUI.itemButtons = new ISleekButton[0];
				}
				EditorSpawnsItemsUI.itemIDField.PositionOffset_Y = (float)(170 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 80 + EditorSpawnsItemsUI.itemButtons.Length * 40);
				EditorSpawnsItemsUI.addItemButton.PositionOffset_Y = (float)(170 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 80 + EditorSpawnsItemsUI.itemButtons.Length * 40 + 40);
				EditorSpawnsItemsUI.removeItemButton.PositionOffset_Y = (float)(170 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 80 + EditorSpawnsItemsUI.itemButtons.Length * 40 + 40);
				EditorSpawnsItemsUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, (float)(170 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 80 + EditorSpawnsItemsUI.itemButtons.Length * 40 + 70));
				return;
			}
			EditorSpawnsItemsUI.selectedBox.Text = "";
			EditorSpawnsItemsUI.tableNameField.Text = "";
			EditorSpawnsItemsUI.tableIDField.Value = 0;
			EditorSpawnsItemsUI.tableColorPicker.state = Color.white;
			if (EditorSpawnsItemsUI.tierButtons != null)
			{
				for (int m = 0; m < EditorSpawnsItemsUI.tierButtons.Length; m++)
				{
					EditorSpawnsItemsUI.spawnsScrollBox.RemoveChild(EditorSpawnsItemsUI.tierButtons[m]);
				}
			}
			EditorSpawnsItemsUI.tierButtons = null;
			EditorSpawnsItemsUI.tierNameField.Text = "";
			EditorSpawnsItemsUI.tierNameField.PositionOffset_Y = 170f;
			EditorSpawnsItemsUI.addTierButton.PositionOffset_Y = 210f;
			EditorSpawnsItemsUI.removeTierButton.PositionOffset_Y = 210f;
			if (EditorSpawnsItemsUI.itemButtons != null)
			{
				for (int n = 0; n < EditorSpawnsItemsUI.itemButtons.Length; n++)
				{
					EditorSpawnsItemsUI.spawnsScrollBox.RemoveChild(EditorSpawnsItemsUI.itemButtons[n]);
				}
			}
			EditorSpawnsItemsUI.itemButtons = null;
			EditorSpawnsItemsUI.itemIDField.PositionOffset_Y = 250f;
			EditorSpawnsItemsUI.addItemButton.PositionOffset_Y = 290f;
			EditorSpawnsItemsUI.removeItemButton.PositionOffset_Y = 290f;
			EditorSpawnsItemsUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, 320f);
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x00137E60 File Offset: 0x00136060
		private static void onClickedTableButton(ISleekElement button)
		{
			if (EditorSpawns.selectedItem != (byte)(button.PositionOffset_Y / 40f))
			{
				EditorSpawns.selectedItem = (byte)(button.PositionOffset_Y / 40f);
				EditorSpawns.itemSpawn.GetComponent<Renderer>().material.color = LevelItems.tables[(int)EditorSpawns.selectedItem].color;
			}
			else
			{
				EditorSpawns.selectedItem = byte.MaxValue;
				EditorSpawns.itemSpawn.GetComponent<Renderer>().material.color = Color.white;
			}
			EditorSpawnsItemsUI.updateSelection();
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x00137EE5 File Offset: 0x001360E5
		private static void onItemColorPicked(SleekColorPicker picker, Color color)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				LevelItems.tables[(int)EditorSpawns.selectedItem].color = color;
			}
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x00137F0D File Offset: 0x0013610D
		private static void onTableIDFieldTyped(ISleekUInt16Field field, ushort state)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				LevelItems.tables[(int)EditorSpawns.selectedItem].tableID = state;
			}
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x00137F38 File Offset: 0x00136138
		private static void onClickedTierButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				if (EditorSpawnsItemsUI.selectedTier != (byte)((button.PositionOffset_Y - 170f) / 70f))
				{
					EditorSpawnsItemsUI.selectedTier = (byte)((button.PositionOffset_Y - 170f) / 70f);
				}
				else
				{
					EditorSpawnsItemsUI.selectedTier = byte.MaxValue;
				}
				EditorSpawnsItemsUI.updateSelection();
			}
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x00137F9C File Offset: 0x0013619C
		private static void onClickItemButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				EditorSpawnsItemsUI.selectItem = (byte)((button.PositionOffset_Y - 170f - (float)(EditorSpawnsItemsUI.tierButtons.Length * 70) - 80f) / 40f);
				EditorSpawnsItemsUI.updateSelection();
			}
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x00137FEC File Offset: 0x001361EC
		private static void onDraggedChanceSlider(ISleekSlider slider, float state)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				int num = Mathf.FloorToInt((slider.Parent.PositionOffset_Y - 170f) / 70f);
				LevelItems.tables[(int)EditorSpawns.selectedItem].updateChance(num, state);
				for (int i = 0; i < LevelItems.tables[(int)EditorSpawns.selectedItem].tiers.Count; i++)
				{
					ItemTier itemTier = LevelItems.tables[(int)EditorSpawns.selectedItem].tiers[i];
					ISleekSlider sleekSlider = (ISleekSlider)EditorSpawnsItemsUI.tierButtons[i].GetChildAtIndex(0);
					if (i != num)
					{
						sleekSlider.Value = itemTier.chance;
					}
					sleekSlider.UpdateLabel(Mathf.RoundToInt(itemTier.chance * 100f).ToString() + "%");
				}
			}
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x001380D0 File Offset: 0x001362D0
		private static void onTypedTableNameField(ISleekField field, string state)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				EditorSpawnsItemsUI.selectedBox.Text = state;
				LevelItems.tables[(int)EditorSpawns.selectedItem].name = state;
				EditorSpawnsItemsUI.tableButtons[(int)EditorSpawns.selectedItem].Text = EditorSpawns.selectedItem.ToString() + " " + state;
			}
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x00138134 File Offset: 0x00136334
		private static void onClickedAddTableButton(ISleekElement button)
		{
			if (EditorSpawnsItemsUI.tableNameField.Text != "")
			{
				LevelItems.addTable(EditorSpawnsItemsUI.tableNameField.Text);
				EditorSpawnsItemsUI.tableNameField.Text = "";
				EditorSpawnsItemsUI.updateTables();
				EditorSpawnsItemsUI.tableScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x00138184 File Offset: 0x00136384
		private static void onClickedRemoveTableButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				LevelItems.removeTable();
				EditorSpawnsItemsUI.updateTables();
				EditorSpawnsItemsUI.updateSelection();
				EditorSpawnsItemsUI.tableScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x001381B0 File Offset: 0x001363B0
		private static void onTypedTierNameField(ISleekField field, string state)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count && (int)EditorSpawnsItemsUI.selectedTier < LevelItems.tables[(int)EditorSpawns.selectedItem].tiers.Count)
			{
				LevelItems.tables[(int)EditorSpawns.selectedItem].tiers[(int)EditorSpawnsItemsUI.selectedTier].name = state;
				EditorSpawnsItemsUI.tierButtons[(int)EditorSpawnsItemsUI.selectedTier].Text = state;
			}
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x00138224 File Offset: 0x00136424
		private static void onClickedAddTierButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count && EditorSpawnsItemsUI.tierNameField.Text != "")
			{
				LevelItems.tables[(int)EditorSpawns.selectedItem].addTier(EditorSpawnsItemsUI.tierNameField.Text);
				EditorSpawnsItemsUI.tierNameField.Text = "";
				EditorSpawnsItemsUI.updateSelection();
			}
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x0013828C File Offset: 0x0013648C
		private static void onClickedRemoveTierButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count && (int)EditorSpawnsItemsUI.selectedTier < LevelItems.tables[(int)EditorSpawns.selectedItem].tiers.Count)
			{
				LevelItems.tables[(int)EditorSpawns.selectedItem].removeTier((int)EditorSpawnsItemsUI.selectedTier);
				EditorSpawnsItemsUI.updateSelection();
			}
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x001382E8 File Offset: 0x001364E8
		private static void onClickedAddItemButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count && (int)EditorSpawnsItemsUI.selectedTier < LevelItems.tables[(int)EditorSpawns.selectedItem].tiers.Count)
			{
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, EditorSpawnsItemsUI.itemIDField.Value) as ItemAsset;
				if (itemAsset != null && !itemAsset.isPro)
				{
					LevelItems.tables[(int)EditorSpawns.selectedItem].addItem(EditorSpawnsItemsUI.selectedTier, EditorSpawnsItemsUI.itemIDField.Value);
					EditorSpawnsItemsUI.updateSelection();
					EditorSpawnsItemsUI.spawnsScrollBox.ScrollToBottom();
				}
				EditorSpawnsItemsUI.itemIDField.Value = 0;
			}
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x00138384 File Offset: 0x00136584
		private static void onClickedRemoveItemButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count && (int)EditorSpawnsItemsUI.selectedTier < LevelItems.tables[(int)EditorSpawns.selectedItem].tiers.Count && (int)EditorSpawnsItemsUI.selectItem < LevelItems.tables[(int)EditorSpawns.selectedItem].tiers[(int)EditorSpawnsItemsUI.selectedTier].table.Count)
			{
				LevelItems.tables[(int)EditorSpawns.selectedItem].removeItem(EditorSpawnsItemsUI.selectedTier, EditorSpawnsItemsUI.selectItem);
				EditorSpawnsItemsUI.updateSelection();
				EditorSpawnsItemsUI.spawnsScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x0013841E File Offset: 0x0013661E
		private static void onDraggedRadiusSlider(ISleekSlider slider, float state)
		{
			EditorSpawns.radius = (byte)((float)EditorSpawns.MIN_REMOVE_SIZE + state * (float)EditorSpawns.MAX_REMOVE_SIZE);
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x00138435 File Offset: 0x00136635
		private static void onClickedAddButton(ISleekElement button)
		{
			EditorSpawns.spawnMode = ESpawnMode.ADD_ITEM;
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x0013843D File Offset: 0x0013663D
		private static void onClickedRemoveButton(ISleekElement button)
		{
			EditorSpawns.spawnMode = ESpawnMode.REMOVE_ITEM;
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x00138448 File Offset: 0x00136648
		public EditorSpawnsItemsUI()
		{
			Local local = Localization.read("/Editor/EditorSpawnsItems.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawnsItems/EditorSpawnsItems.unity3d");
			EditorSpawnsItemsUI.container = new SleekFullscreenBox();
			EditorSpawnsItemsUI.container.PositionOffset_X = 10f;
			EditorSpawnsItemsUI.container.PositionOffset_Y = 10f;
			EditorSpawnsItemsUI.container.PositionScale_X = 1f;
			EditorSpawnsItemsUI.container.SizeOffset_X = -20f;
			EditorSpawnsItemsUI.container.SizeOffset_Y = -20f;
			EditorSpawnsItemsUI.container.SizeScale_X = 1f;
			EditorSpawnsItemsUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorSpawnsItemsUI.container);
			EditorSpawnsItemsUI.active = false;
			EditorSpawnsItemsUI.tableScrollBox = Glazier.Get().CreateScrollView();
			EditorSpawnsItemsUI.tableScrollBox.PositionOffset_X = -470f;
			EditorSpawnsItemsUI.tableScrollBox.PositionOffset_Y = 120f;
			EditorSpawnsItemsUI.tableScrollBox.PositionScale_X = 1f;
			EditorSpawnsItemsUI.tableScrollBox.SizeOffset_X = 470f;
			EditorSpawnsItemsUI.tableScrollBox.SizeOffset_Y = 200f;
			EditorSpawnsItemsUI.tableScrollBox.ScaleContentToWidth = true;
			EditorSpawnsItemsUI.container.AddChild(EditorSpawnsItemsUI.tableScrollBox);
			EditorSpawnsItemsUI.tableNameField = Glazier.Get().CreateStringField();
			EditorSpawnsItemsUI.tableNameField.PositionOffset_X = -230f;
			EditorSpawnsItemsUI.tableNameField.PositionOffset_Y = 330f;
			EditorSpawnsItemsUI.tableNameField.PositionScale_X = 1f;
			EditorSpawnsItemsUI.tableNameField.SizeOffset_X = 230f;
			EditorSpawnsItemsUI.tableNameField.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.tableNameField.MaxLength = 64;
			EditorSpawnsItemsUI.tableNameField.AddLabel(local.format("TableNameFieldLabelText"), 0);
			EditorSpawnsItemsUI.tableNameField.OnTextChanged += new Typed(EditorSpawnsItemsUI.onTypedTableNameField);
			EditorSpawnsItemsUI.container.AddChild(EditorSpawnsItemsUI.tableNameField);
			EditorSpawnsItemsUI.addTableButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsItemsUI.addTableButton.PositionOffset_X = -230f;
			EditorSpawnsItemsUI.addTableButton.PositionOffset_Y = 370f;
			EditorSpawnsItemsUI.addTableButton.PositionScale_X = 1f;
			EditorSpawnsItemsUI.addTableButton.SizeOffset_X = 110f;
			EditorSpawnsItemsUI.addTableButton.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.addTableButton.text = local.format("AddTableButtonText");
			EditorSpawnsItemsUI.addTableButton.tooltip = local.format("AddTableButtonTooltip");
			EditorSpawnsItemsUI.addTableButton.onClickedButton += new ClickedButton(EditorSpawnsItemsUI.onClickedAddTableButton);
			EditorSpawnsItemsUI.container.AddChild(EditorSpawnsItemsUI.addTableButton);
			EditorSpawnsItemsUI.removeTableButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsItemsUI.removeTableButton.PositionOffset_X = -110f;
			EditorSpawnsItemsUI.removeTableButton.PositionOffset_Y = 370f;
			EditorSpawnsItemsUI.removeTableButton.PositionScale_X = 1f;
			EditorSpawnsItemsUI.removeTableButton.SizeOffset_X = 110f;
			EditorSpawnsItemsUI.removeTableButton.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.removeTableButton.text = local.format("RemoveTableButtonText");
			EditorSpawnsItemsUI.removeTableButton.tooltip = local.format("RemoveTableButtonTooltip");
			EditorSpawnsItemsUI.removeTableButton.onClickedButton += new ClickedButton(EditorSpawnsItemsUI.onClickedRemoveTableButton);
			EditorSpawnsItemsUI.container.AddChild(EditorSpawnsItemsUI.removeTableButton);
			EditorSpawnsItemsUI.tableButtons = null;
			EditorSpawnsItemsUI.updateTables();
			EditorSpawnsItemsUI.spawnsScrollBox = Glazier.Get().CreateScrollView();
			EditorSpawnsItemsUI.spawnsScrollBox.PositionOffset_X = -470f;
			EditorSpawnsItemsUI.spawnsScrollBox.PositionOffset_Y = 410f;
			EditorSpawnsItemsUI.spawnsScrollBox.PositionScale_X = 1f;
			EditorSpawnsItemsUI.spawnsScrollBox.SizeOffset_X = 470f;
			EditorSpawnsItemsUI.spawnsScrollBox.SizeOffset_Y = -410f;
			EditorSpawnsItemsUI.spawnsScrollBox.SizeScale_Y = 1f;
			EditorSpawnsItemsUI.spawnsScrollBox.ScaleContentToWidth = true;
			EditorSpawnsItemsUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, 1000f);
			EditorSpawnsItemsUI.container.AddChild(EditorSpawnsItemsUI.spawnsScrollBox);
			EditorSpawnsItemsUI.tableColorPicker = new SleekColorPicker();
			EditorSpawnsItemsUI.tableColorPicker.PositionOffset_X = 200f;
			EditorSpawnsItemsUI.tableColorPicker.onColorPicked = new ColorPicked(EditorSpawnsItemsUI.onItemColorPicked);
			EditorSpawnsItemsUI.spawnsScrollBox.AddChild(EditorSpawnsItemsUI.tableColorPicker);
			EditorSpawnsItemsUI.tableIDField = Glazier.Get().CreateUInt16Field();
			EditorSpawnsItemsUI.tableIDField.PositionOffset_X = 240f;
			EditorSpawnsItemsUI.tableIDField.PositionOffset_Y = 130f;
			EditorSpawnsItemsUI.tableIDField.SizeOffset_X = 200f;
			EditorSpawnsItemsUI.tableIDField.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.tableIDField.OnValueChanged += new TypedUInt16(EditorSpawnsItemsUI.onTableIDFieldTyped);
			EditorSpawnsItemsUI.tableIDField.AddLabel(local.format("TableIDFieldLabelText"), 0);
			EditorSpawnsItemsUI.spawnsScrollBox.AddChild(EditorSpawnsItemsUI.tableIDField);
			EditorSpawnsItemsUI.tierNameField = Glazier.Get().CreateStringField();
			EditorSpawnsItemsUI.tierNameField.PositionOffset_X = 240f;
			EditorSpawnsItemsUI.tierNameField.SizeOffset_X = 200f;
			EditorSpawnsItemsUI.tierNameField.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.tierNameField.MaxLength = 64;
			EditorSpawnsItemsUI.tierNameField.AddLabel(local.format("TierNameFieldLabelText"), 0);
			EditorSpawnsItemsUI.tierNameField.OnTextChanged += new Typed(EditorSpawnsItemsUI.onTypedTierNameField);
			EditorSpawnsItemsUI.spawnsScrollBox.AddChild(EditorSpawnsItemsUI.tierNameField);
			EditorSpawnsItemsUI.addTierButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsItemsUI.addTierButton.PositionOffset_X = 240f;
			EditorSpawnsItemsUI.addTierButton.SizeOffset_X = 95f;
			EditorSpawnsItemsUI.addTierButton.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.addTierButton.text = local.format("AddTierButtonText");
			EditorSpawnsItemsUI.addTierButton.tooltip = local.format("AddTierButtonTooltip");
			EditorSpawnsItemsUI.addTierButton.onClickedButton += new ClickedButton(EditorSpawnsItemsUI.onClickedAddTierButton);
			EditorSpawnsItemsUI.spawnsScrollBox.AddChild(EditorSpawnsItemsUI.addTierButton);
			EditorSpawnsItemsUI.removeTierButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsItemsUI.removeTierButton.PositionOffset_X = 345f;
			EditorSpawnsItemsUI.removeTierButton.SizeOffset_X = 95f;
			EditorSpawnsItemsUI.removeTierButton.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.removeTierButton.text = local.format("RemoveTierButtonText");
			EditorSpawnsItemsUI.removeTierButton.tooltip = local.format("RemoveTierButtonTooltip");
			EditorSpawnsItemsUI.removeTierButton.onClickedButton += new ClickedButton(EditorSpawnsItemsUI.onClickedRemoveTierButton);
			EditorSpawnsItemsUI.spawnsScrollBox.AddChild(EditorSpawnsItemsUI.removeTierButton);
			EditorSpawnsItemsUI.itemIDField = Glazier.Get().CreateUInt16Field();
			EditorSpawnsItemsUI.itemIDField.PositionOffset_X = 240f;
			EditorSpawnsItemsUI.itemIDField.SizeOffset_X = 200f;
			EditorSpawnsItemsUI.itemIDField.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.itemIDField.AddLabel(local.format("ItemIDFieldLabelText"), 0);
			EditorSpawnsItemsUI.spawnsScrollBox.AddChild(EditorSpawnsItemsUI.itemIDField);
			EditorSpawnsItemsUI.addItemButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsItemsUI.addItemButton.PositionOffset_X = 240f;
			EditorSpawnsItemsUI.addItemButton.SizeOffset_X = 95f;
			EditorSpawnsItemsUI.addItemButton.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.addItemButton.text = local.format("AddItemButtonText");
			EditorSpawnsItemsUI.addItemButton.tooltip = local.format("AddItemButtonTooltip");
			EditorSpawnsItemsUI.addItemButton.onClickedButton += new ClickedButton(EditorSpawnsItemsUI.onClickedAddItemButton);
			EditorSpawnsItemsUI.spawnsScrollBox.AddChild(EditorSpawnsItemsUI.addItemButton);
			EditorSpawnsItemsUI.removeItemButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsItemsUI.removeItemButton.PositionOffset_X = 345f;
			EditorSpawnsItemsUI.removeItemButton.SizeOffset_X = 95f;
			EditorSpawnsItemsUI.removeItemButton.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.removeItemButton.text = local.format("RemoveItemButtonText");
			EditorSpawnsItemsUI.removeItemButton.tooltip = local.format("RemoveItemButtonTooltip");
			EditorSpawnsItemsUI.removeItemButton.onClickedButton += new ClickedButton(EditorSpawnsItemsUI.onClickedRemoveItemButton);
			EditorSpawnsItemsUI.spawnsScrollBox.AddChild(EditorSpawnsItemsUI.removeItemButton);
			EditorSpawnsItemsUI.selectedBox = Glazier.Get().CreateBox();
			EditorSpawnsItemsUI.selectedBox.PositionOffset_X = -230f;
			EditorSpawnsItemsUI.selectedBox.PositionOffset_Y = 80f;
			EditorSpawnsItemsUI.selectedBox.PositionScale_X = 1f;
			EditorSpawnsItemsUI.selectedBox.SizeOffset_X = 230f;
			EditorSpawnsItemsUI.selectedBox.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.selectedBox.AddLabel(local.format("SelectionBoxLabelText"), 0);
			EditorSpawnsItemsUI.container.AddChild(EditorSpawnsItemsUI.selectedBox);
			EditorSpawnsItemsUI.tierButtons = null;
			EditorSpawnsItemsUI.itemButtons = null;
			EditorSpawnsItemsUI.updateSelection();
			EditorSpawnsItemsUI.radiusSlider = Glazier.Get().CreateSlider();
			EditorSpawnsItemsUI.radiusSlider.PositionOffset_Y = -100f;
			EditorSpawnsItemsUI.radiusSlider.PositionScale_Y = 1f;
			EditorSpawnsItemsUI.radiusSlider.SizeOffset_X = 200f;
			EditorSpawnsItemsUI.radiusSlider.SizeOffset_Y = 20f;
			EditorSpawnsItemsUI.radiusSlider.Value = (float)(EditorSpawns.radius - EditorSpawns.MIN_REMOVE_SIZE) / (float)EditorSpawns.MAX_REMOVE_SIZE;
			EditorSpawnsItemsUI.radiusSlider.Orientation = 0;
			EditorSpawnsItemsUI.radiusSlider.AddLabel(local.format("RadiusSliderLabelText"), 1);
			EditorSpawnsItemsUI.radiusSlider.OnValueChanged += new Dragged(EditorSpawnsItemsUI.onDraggedRadiusSlider);
			EditorSpawnsItemsUI.container.AddChild(EditorSpawnsItemsUI.radiusSlider);
			EditorSpawnsItemsUI.addButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsItemsUI.addButton.PositionOffset_Y = -70f;
			EditorSpawnsItemsUI.addButton.PositionScale_Y = 1f;
			EditorSpawnsItemsUI.addButton.SizeOffset_X = 200f;
			EditorSpawnsItemsUI.addButton.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.addButton.text = local.format("AddButtonText", ControlsSettings.tool_0);
			EditorSpawnsItemsUI.addButton.tooltip = local.format("AddButtonTooltip");
			EditorSpawnsItemsUI.addButton.onClickedButton += new ClickedButton(EditorSpawnsItemsUI.onClickedAddButton);
			EditorSpawnsItemsUI.container.AddChild(EditorSpawnsItemsUI.addButton);
			EditorSpawnsItemsUI.removeButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsItemsUI.removeButton.PositionOffset_Y = -30f;
			EditorSpawnsItemsUI.removeButton.PositionScale_Y = 1f;
			EditorSpawnsItemsUI.removeButton.SizeOffset_X = 200f;
			EditorSpawnsItemsUI.removeButton.SizeOffset_Y = 30f;
			EditorSpawnsItemsUI.removeButton.text = local.format("RemoveButtonText", ControlsSettings.tool_1);
			EditorSpawnsItemsUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
			EditorSpawnsItemsUI.removeButton.onClickedButton += new ClickedButton(EditorSpawnsItemsUI.onClickedRemoveButton);
			EditorSpawnsItemsUI.container.AddChild(EditorSpawnsItemsUI.removeButton);
			bundle.unload();
		}

		// Token: 0x040027C2 RID: 10178
		private static SleekFullscreenBox container;

		// Token: 0x040027C3 RID: 10179
		public static bool active;

		// Token: 0x040027C4 RID: 10180
		private static ISleekScrollView tableScrollBox;

		// Token: 0x040027C5 RID: 10181
		private static ISleekScrollView spawnsScrollBox;

		// Token: 0x040027C6 RID: 10182
		private static ISleekButton[] tableButtons;

		// Token: 0x040027C7 RID: 10183
		private static ISleekButton[] tierButtons;

		// Token: 0x040027C8 RID: 10184
		private static ISleekButton[] itemButtons;

		// Token: 0x040027C9 RID: 10185
		private static SleekColorPicker tableColorPicker;

		// Token: 0x040027CA RID: 10186
		private static ISleekUInt16Field tableIDField;

		// Token: 0x040027CB RID: 10187
		private static ISleekField tierNameField;

		// Token: 0x040027CC RID: 10188
		private static SleekButtonIcon addTierButton;

		// Token: 0x040027CD RID: 10189
		private static SleekButtonIcon removeTierButton;

		// Token: 0x040027CE RID: 10190
		private static ISleekUInt16Field itemIDField;

		// Token: 0x040027CF RID: 10191
		private static SleekButtonIcon addItemButton;

		// Token: 0x040027D0 RID: 10192
		private static SleekButtonIcon removeItemButton;

		// Token: 0x040027D1 RID: 10193
		private static ISleekBox selectedBox;

		// Token: 0x040027D2 RID: 10194
		private static ISleekField tableNameField;

		// Token: 0x040027D3 RID: 10195
		private static SleekButtonIcon addTableButton;

		// Token: 0x040027D4 RID: 10196
		private static SleekButtonIcon removeTableButton;

		// Token: 0x040027D5 RID: 10197
		private static ISleekSlider radiusSlider;

		// Token: 0x040027D6 RID: 10198
		private static SleekButtonIcon addButton;

		// Token: 0x040027D7 RID: 10199
		private static SleekButtonIcon removeButton;

		// Token: 0x040027D8 RID: 10200
		private static byte selectedTier;

		// Token: 0x040027D9 RID: 10201
		private static byte selectItem;
	}
}
