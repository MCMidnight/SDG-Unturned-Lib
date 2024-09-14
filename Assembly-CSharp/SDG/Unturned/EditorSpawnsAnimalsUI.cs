using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200077C RID: 1916
	public class EditorSpawnsAnimalsUI
	{
		// Token: 0x06003ED5 RID: 16085 RVA: 0x00135FBF File Offset: 0x001341BF
		public static void open()
		{
			if (EditorSpawnsAnimalsUI.active)
			{
				return;
			}
			EditorSpawnsAnimalsUI.active = true;
			EditorSpawns.isSpawning = true;
			EditorSpawns.spawnMode = ESpawnMode.ADD_ANIMAL;
			EditorSpawnsAnimalsUI.container.AnimateIntoView();
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x00135FE6 File Offset: 0x001341E6
		public static void close()
		{
			if (!EditorSpawnsAnimalsUI.active)
			{
				return;
			}
			EditorSpawnsAnimalsUI.active = false;
			EditorSpawns.isSpawning = false;
			EditorSpawnsAnimalsUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x00136010 File Offset: 0x00134210
		public static void updateTables()
		{
			if (EditorSpawnsAnimalsUI.tableButtons != null)
			{
				for (int i = 0; i < EditorSpawnsAnimalsUI.tableButtons.Length; i++)
				{
					EditorSpawnsAnimalsUI.tableScrollBox.RemoveChild(EditorSpawnsAnimalsUI.tableButtons[i]);
				}
			}
			EditorSpawnsAnimalsUI.tableButtons = new ISleekButton[LevelAnimals.tables.Count];
			EditorSpawnsAnimalsUI.tableScrollBox.ContentSizeOffset = new Vector2(0f, (float)(EditorSpawnsAnimalsUI.tableButtons.Length * 40 - 10));
			for (int j = 0; j < EditorSpawnsAnimalsUI.tableButtons.Length; j++)
			{
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_X = 240f;
				sleekButton.PositionOffset_Y = (float)(j * 40);
				sleekButton.SizeOffset_X = 200f;
				sleekButton.SizeOffset_Y = 30f;
				sleekButton.Text = j.ToString() + " " + LevelAnimals.tables[j].name;
				sleekButton.OnClicked += new ClickedButton(EditorSpawnsAnimalsUI.onClickedTableButton);
				EditorSpawnsAnimalsUI.tableScrollBox.AddChild(sleekButton);
				EditorSpawnsAnimalsUI.tableButtons[j] = sleekButton;
			}
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x00136118 File Offset: 0x00134318
		public static void updateSelection()
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
			{
				AnimalTable animalTable = LevelAnimals.tables[(int)EditorSpawns.selectedAnimal];
				EditorSpawnsAnimalsUI.selectedBox.Text = animalTable.name;
				EditorSpawnsAnimalsUI.tableNameField.Text = animalTable.name;
				EditorSpawnsAnimalsUI.tableIDField.Value = animalTable.tableID;
				EditorSpawnsAnimalsUI.tableColorPicker.state = animalTable.color;
				if (EditorSpawnsAnimalsUI.tierButtons != null)
				{
					for (int i = 0; i < EditorSpawnsAnimalsUI.tierButtons.Length; i++)
					{
						EditorSpawnsAnimalsUI.spawnsScrollBox.RemoveChild(EditorSpawnsAnimalsUI.tierButtons[i]);
					}
				}
				EditorSpawnsAnimalsUI.tierButtons = new ISleekButton[animalTable.tiers.Count];
				for (int j = 0; j < EditorSpawnsAnimalsUI.tierButtons.Length; j++)
				{
					AnimalTier animalTier = animalTable.tiers[j];
					ISleekButton sleekButton = Glazier.Get().CreateButton();
					sleekButton.PositionOffset_X = 240f;
					sleekButton.PositionOffset_Y = (float)(170 + j * 70);
					sleekButton.SizeOffset_X = 200f;
					sleekButton.SizeOffset_Y = 30f;
					sleekButton.Text = animalTier.name;
					sleekButton.OnClicked += new ClickedButton(EditorSpawnsAnimalsUI.onClickedTierButton);
					EditorSpawnsAnimalsUI.spawnsScrollBox.AddChild(sleekButton);
					ISleekSlider sleekSlider = Glazier.Get().CreateSlider();
					sleekSlider.PositionOffset_Y = 40f;
					sleekSlider.SizeOffset_X = 200f;
					sleekSlider.SizeOffset_Y = 20f;
					sleekSlider.Orientation = 0;
					sleekSlider.Value = animalTier.chance;
					sleekSlider.AddLabel(Mathf.RoundToInt(animalTier.chance * 100f).ToString() + "%", 0);
					sleekSlider.OnValueChanged += new Dragged(EditorSpawnsAnimalsUI.onDraggedChanceSlider);
					sleekButton.AddChild(sleekSlider);
					EditorSpawnsAnimalsUI.tierButtons[j] = sleekButton;
				}
				EditorSpawnsAnimalsUI.tierNameField.PositionOffset_Y = (float)(170 + EditorSpawnsAnimalsUI.tierButtons.Length * 70);
				EditorSpawnsAnimalsUI.addTierButton.PositionOffset_Y = (float)(170 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 40);
				EditorSpawnsAnimalsUI.removeTierButton.PositionOffset_Y = (float)(170 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 40);
				if (EditorSpawnsAnimalsUI.animalButtons != null)
				{
					for (int k = 0; k < EditorSpawnsAnimalsUI.animalButtons.Length; k++)
					{
						EditorSpawnsAnimalsUI.spawnsScrollBox.RemoveChild(EditorSpawnsAnimalsUI.animalButtons[k]);
					}
				}
				if ((int)EditorSpawnsAnimalsUI.selectedTier < animalTable.tiers.Count)
				{
					EditorSpawnsAnimalsUI.tierNameField.Text = animalTable.tiers[(int)EditorSpawnsAnimalsUI.selectedTier].name;
					EditorSpawnsAnimalsUI.animalButtons = new ISleekButton[animalTable.tiers[(int)EditorSpawnsAnimalsUI.selectedTier].table.Count];
					for (int l = 0; l < EditorSpawnsAnimalsUI.animalButtons.Length; l++)
					{
						ISleekButton sleekButton2 = Glazier.Get().CreateButton();
						sleekButton2.PositionOffset_X = 240f;
						sleekButton2.PositionOffset_Y = (float)(170 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 80 + l * 40);
						sleekButton2.SizeOffset_X = 200f;
						sleekButton2.SizeOffset_Y = 30f;
						AnimalAsset animalAsset = Assets.find(EAssetType.ANIMAL, animalTable.tiers[(int)EditorSpawnsAnimalsUI.selectedTier].table[l].animal) as AnimalAsset;
						string text = "?";
						if (animalAsset != null)
						{
							if (string.IsNullOrEmpty(animalAsset.animalName))
							{
								text = animalAsset.name;
							}
							else
							{
								text = animalAsset.animalName;
							}
						}
						sleekButton2.Text = animalTable.tiers[(int)EditorSpawnsAnimalsUI.selectedTier].table[l].animal.ToString() + " " + text;
						sleekButton2.OnClicked += new ClickedButton(EditorSpawnsAnimalsUI.onClickAnimalButton);
						EditorSpawnsAnimalsUI.spawnsScrollBox.AddChild(sleekButton2);
						EditorSpawnsAnimalsUI.animalButtons[l] = sleekButton2;
					}
				}
				else
				{
					EditorSpawnsAnimalsUI.tierNameField.Text = "";
					EditorSpawnsAnimalsUI.animalButtons = new ISleekButton[0];
				}
				EditorSpawnsAnimalsUI.animalIDField.PositionOffset_Y = (float)(170 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 80 + EditorSpawnsAnimalsUI.animalButtons.Length * 40);
				EditorSpawnsAnimalsUI.addAnimalButton.PositionOffset_Y = (float)(170 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 80 + EditorSpawnsAnimalsUI.animalButtons.Length * 40 + 40);
				EditorSpawnsAnimalsUI.removeAnimalButton.PositionOffset_Y = (float)(170 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 80 + EditorSpawnsAnimalsUI.animalButtons.Length * 40 + 40);
				EditorSpawnsAnimalsUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, (float)(170 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 80 + EditorSpawnsAnimalsUI.animalButtons.Length * 40 + 70));
				return;
			}
			EditorSpawnsAnimalsUI.selectedBox.Text = "";
			EditorSpawnsAnimalsUI.tableNameField.Text = "";
			EditorSpawnsAnimalsUI.tableIDField.Value = 0;
			EditorSpawnsAnimalsUI.tableColorPicker.state = Color.white;
			if (EditorSpawnsAnimalsUI.tierButtons != null)
			{
				for (int m = 0; m < EditorSpawnsAnimalsUI.tierButtons.Length; m++)
				{
					EditorSpawnsAnimalsUI.spawnsScrollBox.RemoveChild(EditorSpawnsAnimalsUI.tierButtons[m]);
				}
			}
			EditorSpawnsAnimalsUI.tierButtons = null;
			EditorSpawnsAnimalsUI.tierNameField.Text = "";
			EditorSpawnsAnimalsUI.tierNameField.PositionOffset_Y = 170f;
			EditorSpawnsAnimalsUI.addTierButton.PositionOffset_Y = 210f;
			EditorSpawnsAnimalsUI.removeTierButton.PositionOffset_Y = 210f;
			if (EditorSpawnsAnimalsUI.animalButtons != null)
			{
				for (int n = 0; n < EditorSpawnsAnimalsUI.animalButtons.Length; n++)
				{
					EditorSpawnsAnimalsUI.spawnsScrollBox.RemoveChild(EditorSpawnsAnimalsUI.animalButtons[n]);
				}
			}
			EditorSpawnsAnimalsUI.animalButtons = null;
			EditorSpawnsAnimalsUI.animalIDField.PositionOffset_Y = 250f;
			EditorSpawnsAnimalsUI.addAnimalButton.PositionOffset_Y = 290f;
			EditorSpawnsAnimalsUI.removeAnimalButton.PositionOffset_Y = 290f;
			EditorSpawnsAnimalsUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, 320f);
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x001366F8 File Offset: 0x001348F8
		private static void onClickedTableButton(ISleekElement button)
		{
			if (EditorSpawns.selectedAnimal != (byte)(button.PositionOffset_Y / 40f))
			{
				EditorSpawns.selectedAnimal = (byte)(button.PositionOffset_Y / 40f);
				EditorSpawns.animalSpawn.GetComponent<Renderer>().material.color = LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].color;
			}
			else
			{
				EditorSpawns.selectedAnimal = byte.MaxValue;
				EditorSpawns.animalSpawn.GetComponent<Renderer>().material.color = Color.white;
			}
			EditorSpawnsAnimalsUI.updateSelection();
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x0013677D File Offset: 0x0013497D
		private static void onAnimalColorPicked(SleekColorPicker picker, Color color)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
			{
				LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].color = color;
			}
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x001367A5 File Offset: 0x001349A5
		private static void onTableIDFieldTyped(ISleekUInt16Field field, ushort state)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
			{
				LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].tableID = state;
			}
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x001367D0 File Offset: 0x001349D0
		private static void onClickedTierButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
			{
				if (EditorSpawnsAnimalsUI.selectedTier != (byte)((button.PositionOffset_Y - 170f) / 70f))
				{
					EditorSpawnsAnimalsUI.selectedTier = (byte)((button.PositionOffset_Y - 170f) / 70f);
				}
				else
				{
					EditorSpawnsAnimalsUI.selectedTier = byte.MaxValue;
				}
				EditorSpawnsAnimalsUI.updateSelection();
			}
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x00136834 File Offset: 0x00134A34
		private static void onClickAnimalButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
			{
				EditorSpawnsAnimalsUI.selectAnimal = (byte)((button.PositionOffset_Y - 170f - (float)(EditorSpawnsAnimalsUI.tierButtons.Length * 70) - 80f) / 40f);
				EditorSpawnsAnimalsUI.updateSelection();
			}
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x00136884 File Offset: 0x00134A84
		private static void onDraggedChanceSlider(ISleekSlider slider, float state)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
			{
				int num = Mathf.FloorToInt((slider.Parent.PositionOffset_Y - 170f) / 70f);
				LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].updateChance(num, state);
				for (int i = 0; i < LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].tiers.Count; i++)
				{
					AnimalTier animalTier = LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].tiers[i];
					ISleekSlider sleekSlider = (ISleekSlider)EditorSpawnsAnimalsUI.tierButtons[i].GetChildAtIndex(0);
					if (i != num)
					{
						sleekSlider.Value = animalTier.chance;
					}
					sleekSlider.UpdateLabel(Mathf.RoundToInt(animalTier.chance * 100f).ToString() + "%");
				}
			}
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x00136968 File Offset: 0x00134B68
		private static void onTypedNameField(ISleekField field, string state)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
			{
				EditorSpawnsAnimalsUI.selectedBox.Text = state;
				LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].name = state;
				EditorSpawnsAnimalsUI.tableButtons[(int)EditorSpawns.selectedAnimal].Text = EditorSpawns.selectedAnimal.ToString() + " " + state;
			}
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x001369CC File Offset: 0x00134BCC
		private static void onClickedAddTableButton(ISleekElement button)
		{
			if (EditorSpawnsAnimalsUI.tableNameField.Text != "")
			{
				LevelAnimals.addTable(EditorSpawnsAnimalsUI.tableNameField.Text);
				EditorSpawnsAnimalsUI.tableNameField.Text = "";
				EditorSpawnsAnimalsUI.updateTables();
				EditorSpawnsAnimalsUI.tableScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x00136A1C File Offset: 0x00134C1C
		private static void onClickedRemoveTableButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
			{
				LevelAnimals.removeTable();
				EditorSpawnsAnimalsUI.updateTables();
				EditorSpawnsAnimalsUI.updateSelection();
				EditorSpawnsAnimalsUI.tableScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x00136A48 File Offset: 0x00134C48
		private static void onTypedTierNameField(ISleekField field, string state)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count && (int)EditorSpawnsAnimalsUI.selectedTier < LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].tiers.Count)
			{
				LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].tiers[(int)EditorSpawnsAnimalsUI.selectedTier].name = state;
				EditorSpawnsAnimalsUI.tierButtons[(int)EditorSpawnsAnimalsUI.selectedTier].Text = state;
			}
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x00136ABC File Offset: 0x00134CBC
		private static void onClickedAddTierButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count && EditorSpawnsAnimalsUI.tierNameField.Text != "")
			{
				LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].addTier(EditorSpawnsAnimalsUI.tierNameField.Text);
				EditorSpawnsAnimalsUI.tierNameField.Text = "";
				EditorSpawnsAnimalsUI.updateSelection();
			}
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x00136B24 File Offset: 0x00134D24
		private static void onClickedRemoveTierButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count && (int)EditorSpawnsAnimalsUI.selectedTier < LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].tiers.Count)
			{
				LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].removeTier((int)EditorSpawnsAnimalsUI.selectedTier);
				EditorSpawnsAnimalsUI.updateSelection();
			}
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x00136B80 File Offset: 0x00134D80
		private static void onClickedAddAnimalButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count && (int)EditorSpawnsAnimalsUI.selectedTier < LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].tiers.Count)
			{
				if (Assets.find(EAssetType.ANIMAL, EditorSpawnsAnimalsUI.animalIDField.Value) is AnimalAsset)
				{
					LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].addAnimal(EditorSpawnsAnimalsUI.selectedTier, EditorSpawnsAnimalsUI.animalIDField.Value);
					EditorSpawnsAnimalsUI.updateSelection();
					EditorSpawnsAnimalsUI.spawnsScrollBox.ScrollToBottom();
				}
				EditorSpawnsAnimalsUI.animalIDField.Value = 0;
			}
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x00136C14 File Offset: 0x00134E14
		private static void onClickedRemoveAnimalButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count && (int)EditorSpawnsAnimalsUI.selectedTier < LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].tiers.Count && (int)EditorSpawnsAnimalsUI.selectAnimal < LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].tiers[(int)EditorSpawnsAnimalsUI.selectedTier].table.Count)
			{
				LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].removeAnimal(EditorSpawnsAnimalsUI.selectedTier, EditorSpawnsAnimalsUI.selectAnimal);
				EditorSpawnsAnimalsUI.updateSelection();
				EditorSpawnsAnimalsUI.spawnsScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x00136CAE File Offset: 0x00134EAE
		private static void onDraggedRadiusSlider(ISleekSlider slider, float state)
		{
			EditorSpawns.radius = (byte)((float)EditorSpawns.MIN_REMOVE_SIZE + state * (float)EditorSpawns.MAX_REMOVE_SIZE);
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x00136CC5 File Offset: 0x00134EC5
		private static void onClickedAddButton(ISleekElement button)
		{
			EditorSpawns.spawnMode = ESpawnMode.ADD_ANIMAL;
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x00136CCE File Offset: 0x00134ECE
		private static void onClickedRemoveButton(ISleekElement button)
		{
			EditorSpawns.spawnMode = ESpawnMode.REMOVE_ANIMAL;
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x00136CD8 File Offset: 0x00134ED8
		public EditorSpawnsAnimalsUI()
		{
			Local local = Localization.read("/Editor/EditorSpawnsAnimals.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawnsAnimals/EditorSpawnsAnimals.unity3d");
			EditorSpawnsAnimalsUI.container = new SleekFullscreenBox();
			EditorSpawnsAnimalsUI.container.PositionOffset_X = 10f;
			EditorSpawnsAnimalsUI.container.PositionOffset_Y = 10f;
			EditorSpawnsAnimalsUI.container.PositionScale_X = 1f;
			EditorSpawnsAnimalsUI.container.SizeOffset_X = -20f;
			EditorSpawnsAnimalsUI.container.SizeOffset_Y = -20f;
			EditorSpawnsAnimalsUI.container.SizeScale_X = 1f;
			EditorSpawnsAnimalsUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorSpawnsAnimalsUI.container);
			EditorSpawnsAnimalsUI.active = false;
			EditorSpawnsAnimalsUI.tableScrollBox = Glazier.Get().CreateScrollView();
			EditorSpawnsAnimalsUI.tableScrollBox.PositionOffset_X = -470f;
			EditorSpawnsAnimalsUI.tableScrollBox.PositionOffset_Y = 120f;
			EditorSpawnsAnimalsUI.tableScrollBox.PositionScale_X = 1f;
			EditorSpawnsAnimalsUI.tableScrollBox.SizeOffset_X = 470f;
			EditorSpawnsAnimalsUI.tableScrollBox.SizeOffset_Y = 200f;
			EditorSpawnsAnimalsUI.tableScrollBox.ScaleContentToWidth = true;
			EditorSpawnsAnimalsUI.container.AddChild(EditorSpawnsAnimalsUI.tableScrollBox);
			EditorSpawnsAnimalsUI.tableNameField = Glazier.Get().CreateStringField();
			EditorSpawnsAnimalsUI.tableNameField.PositionOffset_X = -230f;
			EditorSpawnsAnimalsUI.tableNameField.PositionOffset_Y = 330f;
			EditorSpawnsAnimalsUI.tableNameField.PositionScale_X = 1f;
			EditorSpawnsAnimalsUI.tableNameField.SizeOffset_X = 230f;
			EditorSpawnsAnimalsUI.tableNameField.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.tableNameField.MaxLength = 64;
			EditorSpawnsAnimalsUI.tableNameField.AddLabel(local.format("TableNameFieldLabelText"), 0);
			EditorSpawnsAnimalsUI.tableNameField.OnTextChanged += new Typed(EditorSpawnsAnimalsUI.onTypedNameField);
			EditorSpawnsAnimalsUI.container.AddChild(EditorSpawnsAnimalsUI.tableNameField);
			EditorSpawnsAnimalsUI.addTableButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsAnimalsUI.addTableButton.PositionOffset_X = -230f;
			EditorSpawnsAnimalsUI.addTableButton.PositionOffset_Y = 370f;
			EditorSpawnsAnimalsUI.addTableButton.PositionScale_X = 1f;
			EditorSpawnsAnimalsUI.addTableButton.SizeOffset_X = 110f;
			EditorSpawnsAnimalsUI.addTableButton.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.addTableButton.text = local.format("AddTableButtonText");
			EditorSpawnsAnimalsUI.addTableButton.tooltip = local.format("AddTableButtonTooltip");
			EditorSpawnsAnimalsUI.addTableButton.onClickedButton += new ClickedButton(EditorSpawnsAnimalsUI.onClickedAddTableButton);
			EditorSpawnsAnimalsUI.container.AddChild(EditorSpawnsAnimalsUI.addTableButton);
			EditorSpawnsAnimalsUI.removeTableButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsAnimalsUI.removeTableButton.PositionOffset_X = -110f;
			EditorSpawnsAnimalsUI.removeTableButton.PositionOffset_Y = 370f;
			EditorSpawnsAnimalsUI.removeTableButton.PositionScale_X = 1f;
			EditorSpawnsAnimalsUI.removeTableButton.SizeOffset_X = 110f;
			EditorSpawnsAnimalsUI.removeTableButton.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.removeTableButton.text = local.format("RemoveTableButtonText");
			EditorSpawnsAnimalsUI.removeTableButton.tooltip = local.format("RemoveTableButtonTooltip");
			EditorSpawnsAnimalsUI.removeTableButton.onClickedButton += new ClickedButton(EditorSpawnsAnimalsUI.onClickedRemoveTableButton);
			EditorSpawnsAnimalsUI.container.AddChild(EditorSpawnsAnimalsUI.removeTableButton);
			EditorSpawnsAnimalsUI.tableButtons = null;
			EditorSpawnsAnimalsUI.updateTables();
			EditorSpawnsAnimalsUI.spawnsScrollBox = Glazier.Get().CreateScrollView();
			EditorSpawnsAnimalsUI.spawnsScrollBox.PositionOffset_X = -470f;
			EditorSpawnsAnimalsUI.spawnsScrollBox.PositionOffset_Y = 410f;
			EditorSpawnsAnimalsUI.spawnsScrollBox.PositionScale_X = 1f;
			EditorSpawnsAnimalsUI.spawnsScrollBox.SizeOffset_X = 470f;
			EditorSpawnsAnimalsUI.spawnsScrollBox.SizeOffset_Y = -410f;
			EditorSpawnsAnimalsUI.spawnsScrollBox.SizeScale_Y = 1f;
			EditorSpawnsAnimalsUI.spawnsScrollBox.ScaleContentToWidth = true;
			EditorSpawnsAnimalsUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, 1000f);
			EditorSpawnsAnimalsUI.container.AddChild(EditorSpawnsAnimalsUI.spawnsScrollBox);
			EditorSpawnsAnimalsUI.tableColorPicker = new SleekColorPicker();
			EditorSpawnsAnimalsUI.tableColorPicker.PositionOffset_X = 200f;
			EditorSpawnsAnimalsUI.tableColorPicker.onColorPicked = new ColorPicked(EditorSpawnsAnimalsUI.onAnimalColorPicked);
			EditorSpawnsAnimalsUI.spawnsScrollBox.AddChild(EditorSpawnsAnimalsUI.tableColorPicker);
			EditorSpawnsAnimalsUI.tableIDField = Glazier.Get().CreateUInt16Field();
			EditorSpawnsAnimalsUI.tableIDField.PositionOffset_X = 240f;
			EditorSpawnsAnimalsUI.tableIDField.PositionOffset_Y = 130f;
			EditorSpawnsAnimalsUI.tableIDField.SizeOffset_X = 200f;
			EditorSpawnsAnimalsUI.tableIDField.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.tableIDField.OnValueChanged += new TypedUInt16(EditorSpawnsAnimalsUI.onTableIDFieldTyped);
			EditorSpawnsAnimalsUI.tableIDField.AddLabel(local.format("TableIDFieldLabelText"), 0);
			EditorSpawnsAnimalsUI.spawnsScrollBox.AddChild(EditorSpawnsAnimalsUI.tableIDField);
			EditorSpawnsAnimalsUI.tierNameField = Glazier.Get().CreateStringField();
			EditorSpawnsAnimalsUI.tierNameField.PositionOffset_X = 240f;
			EditorSpawnsAnimalsUI.tierNameField.SizeOffset_X = 200f;
			EditorSpawnsAnimalsUI.tierNameField.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.tierNameField.MaxLength = 64;
			EditorSpawnsAnimalsUI.tierNameField.AddLabel(local.format("TierNameFieldLabelText"), 0);
			EditorSpawnsAnimalsUI.tierNameField.OnTextChanged += new Typed(EditorSpawnsAnimalsUI.onTypedTierNameField);
			EditorSpawnsAnimalsUI.spawnsScrollBox.AddChild(EditorSpawnsAnimalsUI.tierNameField);
			EditorSpawnsAnimalsUI.addTierButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsAnimalsUI.addTierButton.PositionOffset_X = 240f;
			EditorSpawnsAnimalsUI.addTierButton.SizeOffset_X = 95f;
			EditorSpawnsAnimalsUI.addTierButton.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.addTierButton.text = local.format("AddTierButtonText");
			EditorSpawnsAnimalsUI.addTierButton.tooltip = local.format("AddTierButtonTooltip");
			EditorSpawnsAnimalsUI.addTierButton.onClickedButton += new ClickedButton(EditorSpawnsAnimalsUI.onClickedAddTierButton);
			EditorSpawnsAnimalsUI.spawnsScrollBox.AddChild(EditorSpawnsAnimalsUI.addTierButton);
			EditorSpawnsAnimalsUI.removeTierButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsAnimalsUI.removeTierButton.PositionOffset_X = 345f;
			EditorSpawnsAnimalsUI.removeTierButton.SizeOffset_X = 95f;
			EditorSpawnsAnimalsUI.removeTierButton.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.removeTierButton.text = local.format("RemoveTierButtonText");
			EditorSpawnsAnimalsUI.removeTierButton.tooltip = local.format("RemoveTierButtonTooltip");
			EditorSpawnsAnimalsUI.removeTierButton.onClickedButton += new ClickedButton(EditorSpawnsAnimalsUI.onClickedRemoveTierButton);
			EditorSpawnsAnimalsUI.spawnsScrollBox.AddChild(EditorSpawnsAnimalsUI.removeTierButton);
			EditorSpawnsAnimalsUI.animalIDField = Glazier.Get().CreateUInt16Field();
			EditorSpawnsAnimalsUI.animalIDField.PositionOffset_X = 240f;
			EditorSpawnsAnimalsUI.animalIDField.SizeOffset_X = 200f;
			EditorSpawnsAnimalsUI.animalIDField.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.animalIDField.AddLabel(local.format("AnimalIDFieldLabelText"), 0);
			EditorSpawnsAnimalsUI.spawnsScrollBox.AddChild(EditorSpawnsAnimalsUI.animalIDField);
			EditorSpawnsAnimalsUI.addAnimalButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsAnimalsUI.addAnimalButton.PositionOffset_X = 240f;
			EditorSpawnsAnimalsUI.addAnimalButton.SizeOffset_X = 95f;
			EditorSpawnsAnimalsUI.addAnimalButton.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.addAnimalButton.text = local.format("AddAnimalButtonText");
			EditorSpawnsAnimalsUI.addAnimalButton.tooltip = local.format("AddAnimalButtonTooltip");
			EditorSpawnsAnimalsUI.addAnimalButton.onClickedButton += new ClickedButton(EditorSpawnsAnimalsUI.onClickedAddAnimalButton);
			EditorSpawnsAnimalsUI.spawnsScrollBox.AddChild(EditorSpawnsAnimalsUI.addAnimalButton);
			EditorSpawnsAnimalsUI.removeAnimalButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsAnimalsUI.removeAnimalButton.PositionOffset_X = 345f;
			EditorSpawnsAnimalsUI.removeAnimalButton.SizeOffset_X = 95f;
			EditorSpawnsAnimalsUI.removeAnimalButton.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.removeAnimalButton.text = local.format("RemoveAnimalButtonText");
			EditorSpawnsAnimalsUI.removeAnimalButton.tooltip = local.format("RemoveAnimalButtonTooltip");
			EditorSpawnsAnimalsUI.removeAnimalButton.onClickedButton += new ClickedButton(EditorSpawnsAnimalsUI.onClickedRemoveAnimalButton);
			EditorSpawnsAnimalsUI.spawnsScrollBox.AddChild(EditorSpawnsAnimalsUI.removeAnimalButton);
			EditorSpawnsAnimalsUI.selectedBox = Glazier.Get().CreateBox();
			EditorSpawnsAnimalsUI.selectedBox.PositionOffset_X = -230f;
			EditorSpawnsAnimalsUI.selectedBox.PositionOffset_Y = 80f;
			EditorSpawnsAnimalsUI.selectedBox.PositionScale_X = 1f;
			EditorSpawnsAnimalsUI.selectedBox.SizeOffset_X = 230f;
			EditorSpawnsAnimalsUI.selectedBox.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.selectedBox.AddLabel(local.format("SelectionBoxLabelText"), 0);
			EditorSpawnsAnimalsUI.container.AddChild(EditorSpawnsAnimalsUI.selectedBox);
			EditorSpawnsAnimalsUI.tierButtons = null;
			EditorSpawnsAnimalsUI.animalButtons = null;
			EditorSpawnsAnimalsUI.updateSelection();
			EditorSpawnsAnimalsUI.radiusSlider = Glazier.Get().CreateSlider();
			EditorSpawnsAnimalsUI.radiusSlider.PositionOffset_Y = -100f;
			EditorSpawnsAnimalsUI.radiusSlider.PositionScale_Y = 1f;
			EditorSpawnsAnimalsUI.radiusSlider.SizeOffset_X = 200f;
			EditorSpawnsAnimalsUI.radiusSlider.SizeOffset_Y = 20f;
			EditorSpawnsAnimalsUI.radiusSlider.Value = (float)(EditorSpawns.radius - EditorSpawns.MIN_REMOVE_SIZE) / (float)EditorSpawns.MAX_REMOVE_SIZE;
			EditorSpawnsAnimalsUI.radiusSlider.Orientation = 0;
			EditorSpawnsAnimalsUI.radiusSlider.AddLabel(local.format("RadiusSliderLabelText"), 1);
			EditorSpawnsAnimalsUI.radiusSlider.OnValueChanged += new Dragged(EditorSpawnsAnimalsUI.onDraggedRadiusSlider);
			EditorSpawnsAnimalsUI.container.AddChild(EditorSpawnsAnimalsUI.radiusSlider);
			EditorSpawnsAnimalsUI.addButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsAnimalsUI.addButton.PositionOffset_Y = -70f;
			EditorSpawnsAnimalsUI.addButton.PositionScale_Y = 1f;
			EditorSpawnsAnimalsUI.addButton.SizeOffset_X = 200f;
			EditorSpawnsAnimalsUI.addButton.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.addButton.text = local.format("AddButtonText", ControlsSettings.tool_0);
			EditorSpawnsAnimalsUI.addButton.tooltip = local.format("AddButtonTooltip");
			EditorSpawnsAnimalsUI.addButton.onClickedButton += new ClickedButton(EditorSpawnsAnimalsUI.onClickedAddButton);
			EditorSpawnsAnimalsUI.container.AddChild(EditorSpawnsAnimalsUI.addButton);
			EditorSpawnsAnimalsUI.removeButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsAnimalsUI.removeButton.PositionOffset_Y = -30f;
			EditorSpawnsAnimalsUI.removeButton.PositionScale_Y = 1f;
			EditorSpawnsAnimalsUI.removeButton.SizeOffset_X = 200f;
			EditorSpawnsAnimalsUI.removeButton.SizeOffset_Y = 30f;
			EditorSpawnsAnimalsUI.removeButton.text = local.format("RemoveButtonText", ControlsSettings.tool_1);
			EditorSpawnsAnimalsUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
			EditorSpawnsAnimalsUI.removeButton.onClickedButton += new ClickedButton(EditorSpawnsAnimalsUI.onClickedRemoveButton);
			EditorSpawnsAnimalsUI.container.AddChild(EditorSpawnsAnimalsUI.removeButton);
			bundle.unload();
		}

		// Token: 0x040027AA RID: 10154
		private static SleekFullscreenBox container;

		// Token: 0x040027AB RID: 10155
		public static bool active;

		// Token: 0x040027AC RID: 10156
		private static ISleekScrollView tableScrollBox;

		// Token: 0x040027AD RID: 10157
		private static ISleekScrollView spawnsScrollBox;

		// Token: 0x040027AE RID: 10158
		private static ISleekButton[] tableButtons;

		// Token: 0x040027AF RID: 10159
		private static ISleekButton[] tierButtons;

		// Token: 0x040027B0 RID: 10160
		private static ISleekButton[] animalButtons;

		// Token: 0x040027B1 RID: 10161
		private static SleekColorPicker tableColorPicker;

		// Token: 0x040027B2 RID: 10162
		private static ISleekUInt16Field tableIDField;

		// Token: 0x040027B3 RID: 10163
		private static ISleekField tierNameField;

		// Token: 0x040027B4 RID: 10164
		private static SleekButtonIcon addTierButton;

		// Token: 0x040027B5 RID: 10165
		private static SleekButtonIcon removeTierButton;

		// Token: 0x040027B6 RID: 10166
		private static ISleekUInt16Field animalIDField;

		// Token: 0x040027B7 RID: 10167
		private static SleekButtonIcon addAnimalButton;

		// Token: 0x040027B8 RID: 10168
		private static SleekButtonIcon removeAnimalButton;

		// Token: 0x040027B9 RID: 10169
		private static ISleekBox selectedBox;

		// Token: 0x040027BA RID: 10170
		private static ISleekField tableNameField;

		// Token: 0x040027BB RID: 10171
		private static SleekButtonIcon addTableButton;

		// Token: 0x040027BC RID: 10172
		private static SleekButtonIcon removeTableButton;

		// Token: 0x040027BD RID: 10173
		private static ISleekSlider radiusSlider;

		// Token: 0x040027BE RID: 10174
		private static SleekButtonIcon addButton;

		// Token: 0x040027BF RID: 10175
		private static SleekButtonIcon removeButton;

		// Token: 0x040027C0 RID: 10176
		private static byte selectedTier;

		// Token: 0x040027C1 RID: 10177
		private static byte selectAnimal;
	}
}
