using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000781 RID: 1921
	public class EditorSpawnsZombiesUI
	{
		// Token: 0x06003F27 RID: 16167 RVA: 0x0013AFC3 File Offset: 0x001391C3
		public static void open()
		{
			if (EditorSpawnsZombiesUI.active)
			{
				return;
			}
			EditorSpawnsZombiesUI.active = true;
			EditorSpawns.isSpawning = true;
			EditorSpawns.spawnMode = ESpawnMode.ADD_ZOMBIE;
			EditorSpawnsZombiesUI.container.AnimateIntoView();
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x0013AFE9 File Offset: 0x001391E9
		public static void close()
		{
			if (!EditorSpawnsZombiesUI.active)
			{
				return;
			}
			EditorSpawnsZombiesUI.active = false;
			EditorSpawns.isSpawning = false;
			EditorSpawnsZombiesUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x0013B014 File Offset: 0x00139214
		public static void updateTables()
		{
			if (EditorSpawnsZombiesUI.tableButtons != null)
			{
				for (int i = 0; i < EditorSpawnsZombiesUI.tableButtons.Length; i++)
				{
					EditorSpawnsZombiesUI.tableScrollBox.RemoveChild(EditorSpawnsZombiesUI.tableButtons[i]);
				}
			}
			EditorSpawnsZombiesUI.tableButtons = new ISleekButton[LevelZombies.tables.Count];
			EditorSpawnsZombiesUI.tableScrollBox.ContentSizeOffset = new Vector2(0f, (float)(EditorSpawnsZombiesUI.tableButtons.Length * 40 - 10));
			for (int j = 0; j < EditorSpawnsZombiesUI.tableButtons.Length; j++)
			{
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_X = 240f;
				sleekButton.PositionOffset_Y = (float)(j * 40);
				sleekButton.SizeOffset_X = 200f;
				sleekButton.SizeOffset_Y = 30f;
				sleekButton.Text = string.Format("{0} {1} ({2})", j, LevelZombies.tables[j].name, LevelZombies.tables[j].tableUniqueId);
				sleekButton.OnClicked += new ClickedButton(EditorSpawnsZombiesUI.onClickedTableButton);
				EditorSpawnsZombiesUI.tableScrollBox.AddChild(sleekButton);
				EditorSpawnsZombiesUI.tableButtons[j] = sleekButton;
			}
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x0013B130 File Offset: 0x00139330
		public static void updateSelection()
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				ZombieTable zombieTable = LevelZombies.tables[(int)EditorSpawns.selectedZombie];
				EditorSpawnsZombiesUI.selectedBox.Text = zombieTable.name;
				EditorSpawnsZombiesUI.tableNameField.Text = zombieTable.name;
				EditorSpawnsZombiesUI.tableColorPicker.state = zombieTable.color;
				EditorSpawnsZombiesUI.megaToggle.Value = zombieTable.isMega;
				EditorSpawnsZombiesUI.healthField.Value = zombieTable.health;
				EditorSpawnsZombiesUI.damageField.Value = zombieTable.damage;
				EditorSpawnsZombiesUI.lootIndexField.Value = zombieTable.lootIndex;
				EditorSpawnsZombiesUI.lootIDField.Value = zombieTable.lootID;
				EditorSpawnsZombiesUI.xpField.Value = zombieTable.xp;
				EditorSpawnsZombiesUI.regenField.Value = zombieTable.regen;
				EditorSpawnsZombiesUI.difficultyGUIDField.Text = zombieTable.difficultyGUID;
				if (EditorSpawnsZombiesUI.slotButtons != null)
				{
					for (int i = 0; i < EditorSpawnsZombiesUI.slotButtons.Length; i++)
					{
						EditorSpawnsZombiesUI.spawnsScrollBox.RemoveChild(EditorSpawnsZombiesUI.slotButtons[i]);
					}
				}
				EditorSpawnsZombiesUI.slotButtons = new ISleekButton[zombieTable.slots.Length];
				for (int j = 0; j < EditorSpawnsZombiesUI.slotButtons.Length; j++)
				{
					ZombieSlot zombieSlot = zombieTable.slots[j];
					ISleekButton sleekButton = Glazier.Get().CreateButton();
					sleekButton.PositionOffset_X = 240f;
					sleekButton.PositionOffset_Y = (float)(460 + j * 70);
					sleekButton.SizeOffset_X = 200f;
					sleekButton.SizeOffset_Y = 30f;
					sleekButton.Text = EditorSpawnsZombiesUI.localization.format("Slot_" + j.ToString());
					sleekButton.OnClicked += new ClickedButton(EditorSpawnsZombiesUI.onClickedSlotButton);
					EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(sleekButton);
					ISleekSlider sleekSlider = Glazier.Get().CreateSlider();
					sleekSlider.PositionOffset_Y = 40f;
					sleekSlider.SizeOffset_X = 200f;
					sleekSlider.SizeOffset_Y = 20f;
					sleekSlider.Orientation = 0;
					sleekSlider.Value = zombieSlot.chance;
					sleekSlider.AddLabel(Mathf.RoundToInt(zombieSlot.chance * 100f).ToString() + "%", 0);
					sleekSlider.OnValueChanged += new Dragged(EditorSpawnsZombiesUI.onDraggedChanceSlider);
					sleekButton.AddChild(sleekSlider);
					EditorSpawnsZombiesUI.slotButtons[j] = sleekButton;
				}
				if (EditorSpawnsZombiesUI.clothButtons != null)
				{
					for (int k = 0; k < EditorSpawnsZombiesUI.clothButtons.Length; k++)
					{
						EditorSpawnsZombiesUI.spawnsScrollBox.RemoveChild(EditorSpawnsZombiesUI.clothButtons[k]);
					}
				}
				if ((int)EditorSpawnsZombiesUI.selectedSlot < zombieTable.slots.Length)
				{
					EditorSpawnsZombiesUI.clothButtons = new ISleekButton[zombieTable.slots[(int)EditorSpawnsZombiesUI.selectedSlot].table.Count];
					for (int l = 0; l < EditorSpawnsZombiesUI.clothButtons.Length; l++)
					{
						ISleekButton sleekButton2 = Glazier.Get().CreateButton();
						sleekButton2.PositionOffset_X = 240f;
						sleekButton2.PositionOffset_Y = (float)(460 + EditorSpawnsZombiesUI.slotButtons.Length * 70 + l * 40);
						sleekButton2.SizeOffset_X = 200f;
						sleekButton2.SizeOffset_Y = 30f;
						ItemAsset itemAsset = Assets.find(EAssetType.ITEM, zombieTable.slots[(int)EditorSpawnsZombiesUI.selectedSlot].table[l].item) as ItemAsset;
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
						sleekButton2.Text = zombieTable.slots[(int)EditorSpawnsZombiesUI.selectedSlot].table[l].item.ToString() + " " + text;
						sleekButton2.OnClicked += new ClickedButton(EditorSpawnsZombiesUI.onClickItemButton);
						EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(sleekButton2);
						EditorSpawnsZombiesUI.clothButtons[l] = sleekButton2;
					}
				}
				else
				{
					EditorSpawnsZombiesUI.clothButtons = new ISleekButton[0];
				}
				EditorSpawnsZombiesUI.itemIDField.PositionOffset_Y = (float)(460 + EditorSpawnsZombiesUI.slotButtons.Length * 70 + EditorSpawnsZombiesUI.clothButtons.Length * 40);
				EditorSpawnsZombiesUI.addItemButton.PositionOffset_Y = (float)(460 + EditorSpawnsZombiesUI.slotButtons.Length * 70 + EditorSpawnsZombiesUI.clothButtons.Length * 40 + 40);
				EditorSpawnsZombiesUI.removeItemButton.PositionOffset_Y = (float)(460 + EditorSpawnsZombiesUI.slotButtons.Length * 70 + EditorSpawnsZombiesUI.clothButtons.Length * 40 + 40);
				EditorSpawnsZombiesUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, (float)(460 + EditorSpawnsZombiesUI.slotButtons.Length * 70 + EditorSpawnsZombiesUI.clothButtons.Length * 40 + 70));
				return;
			}
			EditorSpawnsZombiesUI.selectedBox.Text = "";
			EditorSpawnsZombiesUI.tableNameField.Text = "";
			EditorSpawnsZombiesUI.tableColorPicker.state = Color.white;
			EditorSpawnsZombiesUI.megaToggle.Value = false;
			EditorSpawnsZombiesUI.healthField.Value = 0;
			EditorSpawnsZombiesUI.damageField.Value = 0;
			EditorSpawnsZombiesUI.lootIndexField.Value = 0;
			EditorSpawnsZombiesUI.lootIDField.Value = 0;
			EditorSpawnsZombiesUI.xpField.Value = 0U;
			EditorSpawnsZombiesUI.regenField.Value = 0f;
			EditorSpawnsZombiesUI.difficultyGUIDField.Text = string.Empty;
			if (EditorSpawnsZombiesUI.slotButtons != null)
			{
				for (int m = 0; m < EditorSpawnsZombiesUI.slotButtons.Length; m++)
				{
					EditorSpawnsZombiesUI.spawnsScrollBox.RemoveChild(EditorSpawnsZombiesUI.slotButtons[m]);
				}
			}
			EditorSpawnsZombiesUI.slotButtons = null;
			if (EditorSpawnsZombiesUI.clothButtons != null)
			{
				for (int n = 0; n < EditorSpawnsZombiesUI.clothButtons.Length; n++)
				{
					EditorSpawnsZombiesUI.spawnsScrollBox.RemoveChild(EditorSpawnsZombiesUI.clothButtons[n]);
				}
			}
			EditorSpawnsZombiesUI.clothButtons = null;
			EditorSpawnsZombiesUI.itemIDField.PositionOffset_Y = 460f;
			EditorSpawnsZombiesUI.addItemButton.PositionOffset_Y = 500f;
			EditorSpawnsZombiesUI.removeItemButton.PositionOffset_Y = 500f;
			EditorSpawnsZombiesUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, 530f);
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x0013B704 File Offset: 0x00139904
		private static void onClickedTableButton(ISleekElement button)
		{
			if (EditorSpawns.selectedZombie != (byte)(button.PositionOffset_Y / 40f))
			{
				EditorSpawns.selectedZombie = (byte)(button.PositionOffset_Y / 40f);
				EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = LevelZombies.tables[(int)EditorSpawns.selectedZombie].color;
			}
			else
			{
				EditorSpawns.selectedZombie = byte.MaxValue;
				EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = Color.white;
			}
			EditorSpawnsZombiesUI.updateSelection();
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x0013B789 File Offset: 0x00139989
		private static void onZombieColorPicked(SleekColorPicker picker, Color color)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].color = color;
			}
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x0013B7B1 File Offset: 0x001399B1
		private static void onToggledMegaToggle(ISleekToggle toggle, bool state)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].isMega = state;
			}
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x0013B7D9 File Offset: 0x001399D9
		private static void onHealthFieldTyped(ISleekUInt16Field field, ushort state)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].health = state;
			}
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x0013B801 File Offset: 0x00139A01
		private static void onDamageFieldTyped(ISleekUInt8Field field, byte state)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].damage = state;
			}
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x0013B829 File Offset: 0x00139A29
		private static void onLootIndexFieldTyped(ISleekUInt8Field field, byte state)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count && (int)state < LevelItems.tables.Count)
			{
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].lootIndex = state;
			}
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x0013B85E File Offset: 0x00139A5E
		private static void onLootIDFieldTyped(ISleekUInt16Field field, ushort state)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].lootID = state;
			}
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x0013B886 File Offset: 0x00139A86
		private static void onXPFieldTyped(ISleekUInt32Field field, uint state)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].xp = state;
			}
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x0013B8AE File Offset: 0x00139AAE
		private static void onRegenFieldTyped(ISleekFloat32Field field, float state)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].regen = state;
			}
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x0013B8D6 File Offset: 0x00139AD6
		private static void onDifficultyGUIDFieldTyped(ISleekField field, string state)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].difficultyGUID = state;
			}
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x0013B8FE File Offset: 0x00139AFE
		private static void onClickedSlotButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				EditorSpawnsZombiesUI.selectedSlot = (byte)((button.PositionOffset_Y - 460f) / 70f);
				EditorSpawnsZombiesUI.updateSelection();
			}
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x0013B92E File Offset: 0x00139B2E
		private static void onClickItemButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				EditorSpawnsZombiesUI.selectItem = (byte)((button.PositionOffset_Y - 460f - (float)(EditorSpawnsZombiesUI.slotButtons.Length * 70)) / 40f);
				EditorSpawnsZombiesUI.updateSelection();
			}
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x0013B96C File Offset: 0x00139B6C
		private static void onDraggedChanceSlider(ISleekSlider slider, float state)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				int num = Mathf.FloorToInt((slider.Parent.PositionOffset_Y - 460f) / 70f);
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].slots[num].chance = state;
				((ISleekSlider)EditorSpawnsZombiesUI.slotButtons[num].GetChildAtIndex(0)).UpdateLabel(Mathf.RoundToInt(state * 100f).ToString() + "%");
			}
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x0013B9F8 File Offset: 0x00139BF8
		private static void onTypedNameField(ISleekField field, string state)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				EditorSpawnsZombiesUI.selectedBox.Text = state;
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].name = state;
				EditorSpawnsZombiesUI.tableButtons[(int)EditorSpawns.selectedZombie].Text = EditorSpawns.selectedZombie.ToString() + " " + state + string.Format(" ({0})", LevelZombies.tables[(int)EditorSpawns.selectedZombie].tableUniqueId);
			}
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x0013BA80 File Offset: 0x00139C80
		private static void onClickedAddTableButton(ISleekElement button)
		{
			if (EditorSpawnsZombiesUI.tableNameField.Text != "")
			{
				LevelZombies.addTable(EditorSpawnsZombiesUI.tableNameField.Text);
				EditorSpawnsZombiesUI.tableNameField.Text = "";
				EditorSpawnsZombiesUI.updateTables();
				EditorSpawnsZombiesUI.tableScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x0013BAD0 File Offset: 0x00139CD0
		private static void onClickedRemoveTableButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				LevelZombies.removeTable();
				EditorSpawnsZombiesUI.updateTables();
				EditorSpawnsZombiesUI.updateSelection();
				EditorSpawnsZombiesUI.tableScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x0013BAFC File Offset: 0x00139CFC
		private static void onClickedAddItemButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, EditorSpawnsZombiesUI.itemIDField.Value) as ItemAsset;
				if (itemAsset != null)
				{
					if (EditorSpawnsZombiesUI.selectedSlot == 0 && itemAsset.type != EItemType.SHIRT)
					{
						return;
					}
					if (EditorSpawnsZombiesUI.selectedSlot == 1 && itemAsset.type != EItemType.PANTS)
					{
						return;
					}
					if ((EditorSpawnsZombiesUI.selectedSlot == 2 || EditorSpawnsZombiesUI.selectedSlot == 3) && itemAsset.type != EItemType.HAT && itemAsset.type != EItemType.BACKPACK && itemAsset.type != EItemType.VEST && itemAsset.type != EItemType.MASK && itemAsset.type != EItemType.GLASSES)
					{
						return;
					}
					LevelZombies.tables[(int)EditorSpawns.selectedZombie].addCloth(EditorSpawnsZombiesUI.selectedSlot, EditorSpawnsZombiesUI.itemIDField.Value);
					EditorSpawnsZombiesUI.updateSelection();
					EditorSpawnsZombiesUI.spawnsScrollBox.ScrollToBottom();
				}
				EditorSpawnsZombiesUI.itemIDField.Value = 0;
			}
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x0013BBD8 File Offset: 0x00139DD8
		private static void onClickedRemoveItemButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count && (int)EditorSpawnsZombiesUI.selectItem < LevelZombies.tables[(int)EditorSpawns.selectedZombie].slots[(int)EditorSpawnsZombiesUI.selectedSlot].table.Count)
			{
				LevelZombies.tables[(int)EditorSpawns.selectedZombie].removeCloth(EditorSpawnsZombiesUI.selectedSlot, EditorSpawnsZombiesUI.selectItem);
				EditorSpawnsZombiesUI.updateSelection();
				EditorSpawnsZombiesUI.spawnsScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x0013BC4E File Offset: 0x00139E4E
		private static void onDraggedRadiusSlider(ISleekSlider slider, float state)
		{
			EditorSpawns.radius = (byte)((float)EditorSpawns.MIN_REMOVE_SIZE + state * (float)EditorSpawns.MAX_REMOVE_SIZE);
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x0013BC65 File Offset: 0x00139E65
		private static void onClickedAddButton(ISleekElement button)
		{
			EditorSpawns.spawnMode = ESpawnMode.ADD_ZOMBIE;
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x0013BC6D File Offset: 0x00139E6D
		private static void onClickedRemoveButton(ISleekElement button)
		{
			EditorSpawns.spawnMode = ESpawnMode.REMOVE_ZOMBIE;
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x0013BC78 File Offset: 0x00139E78
		public EditorSpawnsZombiesUI()
		{
			EditorSpawnsZombiesUI.localization = Localization.read("/Editor/EditorSpawnsZombies.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawnsZombies/EditorSpawnsZombies.unity3d");
			EditorSpawnsZombiesUI.container = new SleekFullscreenBox();
			EditorSpawnsZombiesUI.container.PositionOffset_X = 10f;
			EditorSpawnsZombiesUI.container.PositionOffset_Y = 10f;
			EditorSpawnsZombiesUI.container.PositionScale_X = 1f;
			EditorSpawnsZombiesUI.container.SizeOffset_X = -20f;
			EditorSpawnsZombiesUI.container.SizeOffset_Y = -20f;
			EditorSpawnsZombiesUI.container.SizeScale_X = 1f;
			EditorSpawnsZombiesUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorSpawnsZombiesUI.container);
			EditorSpawnsZombiesUI.active = false;
			EditorSpawnsZombiesUI.tableScrollBox = Glazier.Get().CreateScrollView();
			EditorSpawnsZombiesUI.tableScrollBox.PositionOffset_X = -470f;
			EditorSpawnsZombiesUI.tableScrollBox.PositionOffset_Y = 120f;
			EditorSpawnsZombiesUI.tableScrollBox.PositionScale_X = 1f;
			EditorSpawnsZombiesUI.tableScrollBox.SizeOffset_X = 470f;
			EditorSpawnsZombiesUI.tableScrollBox.SizeOffset_Y = 200f;
			EditorSpawnsZombiesUI.container.AddChild(EditorSpawnsZombiesUI.tableScrollBox);
			EditorSpawnsZombiesUI.tableNameField = Glazier.Get().CreateStringField();
			EditorSpawnsZombiesUI.tableNameField.PositionOffset_X = -230f;
			EditorSpawnsZombiesUI.tableNameField.PositionOffset_Y = 330f;
			EditorSpawnsZombiesUI.tableNameField.PositionScale_X = 1f;
			EditorSpawnsZombiesUI.tableNameField.SizeOffset_X = 230f;
			EditorSpawnsZombiesUI.tableNameField.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.tableNameField.MaxLength = 64;
			EditorSpawnsZombiesUI.tableNameField.AddLabel(EditorSpawnsZombiesUI.localization.format("TableNameFieldLabelText"), 0);
			EditorSpawnsZombiesUI.tableNameField.OnTextChanged += new Typed(EditorSpawnsZombiesUI.onTypedNameField);
			EditorSpawnsZombiesUI.container.AddChild(EditorSpawnsZombiesUI.tableNameField);
			EditorSpawnsZombiesUI.addTableButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsZombiesUI.addTableButton.PositionOffset_X = -230f;
			EditorSpawnsZombiesUI.addTableButton.PositionOffset_Y = 370f;
			EditorSpawnsZombiesUI.addTableButton.PositionScale_X = 1f;
			EditorSpawnsZombiesUI.addTableButton.SizeOffset_X = 110f;
			EditorSpawnsZombiesUI.addTableButton.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.addTableButton.text = EditorSpawnsZombiesUI.localization.format("AddTableButtonText");
			EditorSpawnsZombiesUI.addTableButton.tooltip = EditorSpawnsZombiesUI.localization.format("AddTableButtonTooltip");
			EditorSpawnsZombiesUI.addTableButton.onClickedButton += new ClickedButton(EditorSpawnsZombiesUI.onClickedAddTableButton);
			EditorSpawnsZombiesUI.container.AddChild(EditorSpawnsZombiesUI.addTableButton);
			EditorSpawnsZombiesUI.removeTableButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsZombiesUI.removeTableButton.PositionOffset_X = -110f;
			EditorSpawnsZombiesUI.removeTableButton.PositionOffset_Y = 370f;
			EditorSpawnsZombiesUI.removeTableButton.PositionScale_X = 1f;
			EditorSpawnsZombiesUI.removeTableButton.SizeOffset_X = 110f;
			EditorSpawnsZombiesUI.removeTableButton.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.removeTableButton.text = EditorSpawnsZombiesUI.localization.format("RemoveTableButtonText");
			EditorSpawnsZombiesUI.removeTableButton.tooltip = EditorSpawnsZombiesUI.localization.format("RemoveTableButtonTooltip");
			EditorSpawnsZombiesUI.removeTableButton.onClickedButton += new ClickedButton(EditorSpawnsZombiesUI.onClickedRemoveTableButton);
			EditorSpawnsZombiesUI.container.AddChild(EditorSpawnsZombiesUI.removeTableButton);
			EditorSpawnsZombiesUI.tableButtons = null;
			EditorSpawnsZombiesUI.updateTables();
			EditorSpawnsZombiesUI.spawnsScrollBox = Glazier.Get().CreateScrollView();
			EditorSpawnsZombiesUI.spawnsScrollBox.PositionOffset_X = -470f;
			EditorSpawnsZombiesUI.spawnsScrollBox.PositionOffset_Y = 410f;
			EditorSpawnsZombiesUI.spawnsScrollBox.PositionScale_X = 1f;
			EditorSpawnsZombiesUI.spawnsScrollBox.SizeOffset_X = 470f;
			EditorSpawnsZombiesUI.spawnsScrollBox.SizeOffset_Y = -410f;
			EditorSpawnsZombiesUI.spawnsScrollBox.SizeScale_Y = 1f;
			EditorSpawnsZombiesUI.spawnsScrollBox.ScaleContentToWidth = true;
			EditorSpawnsZombiesUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, 1000f);
			EditorSpawnsZombiesUI.container.AddChild(EditorSpawnsZombiesUI.spawnsScrollBox);
			EditorSpawnsZombiesUI.tableColorPicker = new SleekColorPicker();
			EditorSpawnsZombiesUI.tableColorPicker.PositionOffset_X = 200f;
			EditorSpawnsZombiesUI.tableColorPicker.onColorPicked = new ColorPicked(EditorSpawnsZombiesUI.onZombieColorPicked);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.tableColorPicker);
			EditorSpawnsZombiesUI.megaToggle = Glazier.Get().CreateToggle();
			EditorSpawnsZombiesUI.megaToggle.PositionOffset_X = 240f;
			EditorSpawnsZombiesUI.megaToggle.PositionOffset_Y = 130f;
			EditorSpawnsZombiesUI.megaToggle.SizeOffset_X = 40f;
			EditorSpawnsZombiesUI.megaToggle.SizeOffset_Y = 40f;
			EditorSpawnsZombiesUI.megaToggle.OnValueChanged += new Toggled(EditorSpawnsZombiesUI.onToggledMegaToggle);
			EditorSpawnsZombiesUI.megaToggle.AddLabel(EditorSpawnsZombiesUI.localization.format("MegaToggleLabelText"), 0);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.megaToggle);
			EditorSpawnsZombiesUI.healthField = Glazier.Get().CreateUInt16Field();
			EditorSpawnsZombiesUI.healthField.PositionOffset_X = 240f;
			EditorSpawnsZombiesUI.healthField.PositionOffset_Y = 180f;
			EditorSpawnsZombiesUI.healthField.SizeOffset_X = 200f;
			EditorSpawnsZombiesUI.healthField.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.healthField.OnValueChanged += new TypedUInt16(EditorSpawnsZombiesUI.onHealthFieldTyped);
			EditorSpawnsZombiesUI.healthField.AddLabel(EditorSpawnsZombiesUI.localization.format("HealthFieldLabelText"), 0);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.healthField);
			EditorSpawnsZombiesUI.damageField = Glazier.Get().CreateUInt8Field();
			EditorSpawnsZombiesUI.damageField.PositionOffset_X = 240f;
			EditorSpawnsZombiesUI.damageField.PositionOffset_Y = 220f;
			EditorSpawnsZombiesUI.damageField.SizeOffset_X = 200f;
			EditorSpawnsZombiesUI.damageField.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.damageField.OnValueChanged += new TypedByte(EditorSpawnsZombiesUI.onDamageFieldTyped);
			EditorSpawnsZombiesUI.damageField.AddLabel(EditorSpawnsZombiesUI.localization.format("DamageFieldLabelText"), 0);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.damageField);
			EditorSpawnsZombiesUI.lootIndexField = Glazier.Get().CreateUInt8Field();
			EditorSpawnsZombiesUI.lootIndexField.PositionOffset_X = 240f;
			EditorSpawnsZombiesUI.lootIndexField.PositionOffset_Y = 260f;
			EditorSpawnsZombiesUI.lootIndexField.SizeOffset_X = 200f;
			EditorSpawnsZombiesUI.lootIndexField.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.lootIndexField.OnValueChanged += new TypedByte(EditorSpawnsZombiesUI.onLootIndexFieldTyped);
			EditorSpawnsZombiesUI.lootIndexField.AddLabel(EditorSpawnsZombiesUI.localization.format("LootIndexFieldLabelText"), 0);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.lootIndexField);
			EditorSpawnsZombiesUI.lootIDField = Glazier.Get().CreateUInt16Field();
			EditorSpawnsZombiesUI.lootIDField.PositionOffset_X = 240f;
			EditorSpawnsZombiesUI.lootIDField.PositionOffset_Y = 300f;
			EditorSpawnsZombiesUI.lootIDField.SizeOffset_X = 200f;
			EditorSpawnsZombiesUI.lootIDField.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.lootIDField.OnValueChanged += new TypedUInt16(EditorSpawnsZombiesUI.onLootIDFieldTyped);
			EditorSpawnsZombiesUI.lootIDField.AddLabel(EditorSpawnsZombiesUI.localization.format("LootIDFieldLabelText"), 0);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.lootIDField);
			EditorSpawnsZombiesUI.xpField = Glazier.Get().CreateUInt32Field();
			EditorSpawnsZombiesUI.xpField.PositionOffset_X = 240f;
			EditorSpawnsZombiesUI.xpField.PositionOffset_Y = 340f;
			EditorSpawnsZombiesUI.xpField.SizeOffset_X = 200f;
			EditorSpawnsZombiesUI.xpField.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.xpField.OnValueChanged += new TypedUInt32(EditorSpawnsZombiesUI.onXPFieldTyped);
			EditorSpawnsZombiesUI.xpField.AddLabel(EditorSpawnsZombiesUI.localization.format("XPFieldLabelText"), 0);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.xpField);
			EditorSpawnsZombiesUI.regenField = Glazier.Get().CreateFloat32Field();
			EditorSpawnsZombiesUI.regenField.PositionOffset_X = 240f;
			EditorSpawnsZombiesUI.regenField.PositionOffset_Y = 380f;
			EditorSpawnsZombiesUI.regenField.SizeOffset_X = 200f;
			EditorSpawnsZombiesUI.regenField.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.regenField.OnValueChanged += new TypedSingle(EditorSpawnsZombiesUI.onRegenFieldTyped);
			EditorSpawnsZombiesUI.regenField.AddLabel(EditorSpawnsZombiesUI.localization.format("RegenFieldLabelText"), 0);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.regenField);
			EditorSpawnsZombiesUI.difficultyGUIDField = Glazier.Get().CreateStringField();
			EditorSpawnsZombiesUI.difficultyGUIDField.PositionOffset_X = 240f;
			EditorSpawnsZombiesUI.difficultyGUIDField.PositionOffset_Y = 420f;
			EditorSpawnsZombiesUI.difficultyGUIDField.SizeOffset_X = 200f;
			EditorSpawnsZombiesUI.difficultyGUIDField.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.difficultyGUIDField.MaxLength = 32;
			EditorSpawnsZombiesUI.difficultyGUIDField.OnTextChanged += new Typed(EditorSpawnsZombiesUI.onDifficultyGUIDFieldTyped);
			EditorSpawnsZombiesUI.difficultyGUIDField.AddLabel(EditorSpawnsZombiesUI.localization.format("DifficultyGUIDFieldLabelText"), 0);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.difficultyGUIDField);
			EditorSpawnsZombiesUI.itemIDField = Glazier.Get().CreateUInt16Field();
			EditorSpawnsZombiesUI.itemIDField.PositionOffset_X = 240f;
			EditorSpawnsZombiesUI.itemIDField.SizeOffset_X = 200f;
			EditorSpawnsZombiesUI.itemIDField.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.itemIDField.AddLabel(EditorSpawnsZombiesUI.localization.format("ItemIDFieldLabelText"), 0);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.itemIDField);
			EditorSpawnsZombiesUI.addItemButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsZombiesUI.addItemButton.PositionOffset_X = 240f;
			EditorSpawnsZombiesUI.addItemButton.SizeOffset_X = 95f;
			EditorSpawnsZombiesUI.addItemButton.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.addItemButton.text = EditorSpawnsZombiesUI.localization.format("AddItemButtonText");
			EditorSpawnsZombiesUI.addItemButton.tooltip = EditorSpawnsZombiesUI.localization.format("AddItemButtonTooltip");
			EditorSpawnsZombiesUI.addItemButton.onClickedButton += new ClickedButton(EditorSpawnsZombiesUI.onClickedAddItemButton);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.addItemButton);
			EditorSpawnsZombiesUI.removeItemButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsZombiesUI.removeItemButton.PositionOffset_X = 345f;
			EditorSpawnsZombiesUI.removeItemButton.SizeOffset_X = 95f;
			EditorSpawnsZombiesUI.removeItemButton.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.removeItemButton.text = EditorSpawnsZombiesUI.localization.format("RemoveItemButtonText");
			EditorSpawnsZombiesUI.removeItemButton.tooltip = EditorSpawnsZombiesUI.localization.format("RemoveItemButtonTooltip");
			EditorSpawnsZombiesUI.removeItemButton.onClickedButton += new ClickedButton(EditorSpawnsZombiesUI.onClickedRemoveItemButton);
			EditorSpawnsZombiesUI.spawnsScrollBox.AddChild(EditorSpawnsZombiesUI.removeItemButton);
			EditorSpawnsZombiesUI.selectedBox = Glazier.Get().CreateBox();
			EditorSpawnsZombiesUI.selectedBox.PositionOffset_X = -230f;
			EditorSpawnsZombiesUI.selectedBox.PositionOffset_Y = 80f;
			EditorSpawnsZombiesUI.selectedBox.PositionScale_X = 1f;
			EditorSpawnsZombiesUI.selectedBox.SizeOffset_X = 230f;
			EditorSpawnsZombiesUI.selectedBox.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.selectedBox.AddLabel(EditorSpawnsZombiesUI.localization.format("SelectionBoxLabelText"), 0);
			EditorSpawnsZombiesUI.container.AddChild(EditorSpawnsZombiesUI.selectedBox);
			EditorSpawnsZombiesUI.slotButtons = null;
			EditorSpawnsZombiesUI.clothButtons = null;
			EditorSpawnsZombiesUI.updateSelection();
			EditorSpawnsZombiesUI.radiusSlider = Glazier.Get().CreateSlider();
			EditorSpawnsZombiesUI.radiusSlider.PositionOffset_Y = -100f;
			EditorSpawnsZombiesUI.radiusSlider.PositionScale_Y = 1f;
			EditorSpawnsZombiesUI.radiusSlider.SizeOffset_X = 200f;
			EditorSpawnsZombiesUI.radiusSlider.SizeOffset_Y = 20f;
			EditorSpawnsZombiesUI.radiusSlider.Value = (float)(EditorSpawns.radius - EditorSpawns.MIN_REMOVE_SIZE) / (float)EditorSpawns.MAX_REMOVE_SIZE;
			EditorSpawnsZombiesUI.radiusSlider.Orientation = 0;
			EditorSpawnsZombiesUI.radiusSlider.AddLabel(EditorSpawnsZombiesUI.localization.format("RadiusSliderLabelText"), 1);
			EditorSpawnsZombiesUI.radiusSlider.OnValueChanged += new Dragged(EditorSpawnsZombiesUI.onDraggedRadiusSlider);
			EditorSpawnsZombiesUI.container.AddChild(EditorSpawnsZombiesUI.radiusSlider);
			EditorSpawnsZombiesUI.addButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsZombiesUI.addButton.PositionOffset_Y = -70f;
			EditorSpawnsZombiesUI.addButton.PositionScale_Y = 1f;
			EditorSpawnsZombiesUI.addButton.SizeOffset_X = 200f;
			EditorSpawnsZombiesUI.addButton.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.addButton.text = EditorSpawnsZombiesUI.localization.format("AddButtonText", ControlsSettings.tool_0);
			EditorSpawnsZombiesUI.addButton.tooltip = EditorSpawnsZombiesUI.localization.format("AddButtonTooltip");
			EditorSpawnsZombiesUI.addButton.onClickedButton += new ClickedButton(EditorSpawnsZombiesUI.onClickedAddButton);
			EditorSpawnsZombiesUI.container.AddChild(EditorSpawnsZombiesUI.addButton);
			EditorSpawnsZombiesUI.removeButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsZombiesUI.removeButton.PositionOffset_Y = -30f;
			EditorSpawnsZombiesUI.removeButton.PositionScale_Y = 1f;
			EditorSpawnsZombiesUI.removeButton.SizeOffset_X = 200f;
			EditorSpawnsZombiesUI.removeButton.SizeOffset_Y = 30f;
			EditorSpawnsZombiesUI.removeButton.text = EditorSpawnsZombiesUI.localization.format("RemoveButtonText", ControlsSettings.tool_1);
			EditorSpawnsZombiesUI.removeButton.tooltip = EditorSpawnsZombiesUI.localization.format("RemoveButtonTooltip");
			EditorSpawnsZombiesUI.removeButton.onClickedButton += new ClickedButton(EditorSpawnsZombiesUI.onClickedRemoveButton);
			EditorSpawnsZombiesUI.container.AddChild(EditorSpawnsZombiesUI.removeButton);
			bundle.unload();
		}

		// Token: 0x04002800 RID: 10240
		private static SleekFullscreenBox container;

		// Token: 0x04002801 RID: 10241
		private static Local localization;

		// Token: 0x04002802 RID: 10242
		public static bool active;

		// Token: 0x04002803 RID: 10243
		private static ISleekScrollView tableScrollBox;

		// Token: 0x04002804 RID: 10244
		private static ISleekScrollView spawnsScrollBox;

		// Token: 0x04002805 RID: 10245
		private static ISleekButton[] tableButtons;

		// Token: 0x04002806 RID: 10246
		private static ISleekButton[] slotButtons;

		// Token: 0x04002807 RID: 10247
		private static ISleekButton[] clothButtons;

		// Token: 0x04002808 RID: 10248
		private static SleekColorPicker tableColorPicker;

		// Token: 0x04002809 RID: 10249
		private static ISleekToggle megaToggle;

		// Token: 0x0400280A RID: 10250
		private static ISleekUInt16Field healthField;

		// Token: 0x0400280B RID: 10251
		private static ISleekUInt8Field damageField;

		// Token: 0x0400280C RID: 10252
		private static ISleekUInt8Field lootIndexField;

		// Token: 0x0400280D RID: 10253
		private static ISleekUInt16Field lootIDField;

		// Token: 0x0400280E RID: 10254
		private static ISleekUInt32Field xpField;

		// Token: 0x0400280F RID: 10255
		private static ISleekFloat32Field regenField;

		// Token: 0x04002810 RID: 10256
		private static ISleekField difficultyGUIDField;

		// Token: 0x04002811 RID: 10257
		private static ISleekUInt16Field itemIDField;

		// Token: 0x04002812 RID: 10258
		private static SleekButtonIcon addItemButton;

		// Token: 0x04002813 RID: 10259
		private static SleekButtonIcon removeItemButton;

		// Token: 0x04002814 RID: 10260
		private static ISleekBox selectedBox;

		// Token: 0x04002815 RID: 10261
		private static ISleekField tableNameField;

		// Token: 0x04002816 RID: 10262
		private static SleekButtonIcon addTableButton;

		// Token: 0x04002817 RID: 10263
		private static SleekButtonIcon removeTableButton;

		// Token: 0x04002818 RID: 10264
		private static ISleekSlider radiusSlider;

		// Token: 0x04002819 RID: 10265
		private static SleekButtonIcon addButton;

		// Token: 0x0400281A RID: 10266
		private static SleekButtonIcon removeButton;

		// Token: 0x0400281B RID: 10267
		private static byte selectedSlot;

		// Token: 0x0400281C RID: 10268
		private static byte selectItem;
	}
}
