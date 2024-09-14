using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000780 RID: 1920
	public class EditorSpawnsVehiclesUI
	{
		// Token: 0x06003F10 RID: 16144 RVA: 0x00139767 File Offset: 0x00137967
		public static void open()
		{
			if (EditorSpawnsVehiclesUI.active)
			{
				return;
			}
			EditorSpawnsVehiclesUI.active = true;
			EditorSpawns.isSpawning = true;
			EditorSpawns.spawnMode = ESpawnMode.ADD_VEHICLE;
			EditorSpawnsVehiclesUI.container.AnimateIntoView();
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x0013978D File Offset: 0x0013798D
		public static void close()
		{
			if (!EditorSpawnsVehiclesUI.active)
			{
				return;
			}
			EditorSpawnsVehiclesUI.active = false;
			EditorSpawns.isSpawning = false;
			EditorSpawnsVehiclesUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x001397B8 File Offset: 0x001379B8
		public static void updateTables()
		{
			if (EditorSpawnsVehiclesUI.tableButtons != null)
			{
				for (int i = 0; i < EditorSpawnsVehiclesUI.tableButtons.Length; i++)
				{
					EditorSpawnsVehiclesUI.tableScrollBox.RemoveChild(EditorSpawnsVehiclesUI.tableButtons[i]);
				}
			}
			EditorSpawnsVehiclesUI.tableButtons = new ISleekButton[LevelVehicles.tables.Count];
			EditorSpawnsVehiclesUI.tableScrollBox.ContentSizeOffset = new Vector2(0f, (float)(EditorSpawnsVehiclesUI.tableButtons.Length * 40 - 10));
			for (int j = 0; j < EditorSpawnsVehiclesUI.tableButtons.Length; j++)
			{
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_X = 240f;
				sleekButton.PositionOffset_Y = (float)(j * 40);
				sleekButton.SizeOffset_X = 200f;
				sleekButton.SizeOffset_Y = 30f;
				sleekButton.Text = j.ToString() + " " + LevelVehicles.tables[j].name;
				sleekButton.OnClicked += new ClickedButton(EditorSpawnsVehiclesUI.onClickedTableButton);
				EditorSpawnsVehiclesUI.tableScrollBox.AddChild(sleekButton);
				EditorSpawnsVehiclesUI.tableButtons[j] = sleekButton;
			}
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x001398C0 File Offset: 0x00137AC0
		public static void updateSelection()
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				VehicleTable vehicleTable = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle];
				EditorSpawnsVehiclesUI.selectedBox.Text = vehicleTable.name;
				EditorSpawnsVehiclesUI.tableNameField.Text = vehicleTable.name;
				EditorSpawnsVehiclesUI.tableIDField.Value = vehicleTable.tableID;
				EditorSpawnsVehiclesUI.tableColorPicker.state = vehicleTable.color;
				if (EditorSpawnsVehiclesUI.tierButtons != null)
				{
					for (int i = 0; i < EditorSpawnsVehiclesUI.tierButtons.Length; i++)
					{
						EditorSpawnsVehiclesUI.spawnsScrollBox.RemoveChild(EditorSpawnsVehiclesUI.tierButtons[i]);
					}
				}
				EditorSpawnsVehiclesUI.tierButtons = new ISleekButton[vehicleTable.tiers.Count];
				for (int j = 0; j < EditorSpawnsVehiclesUI.tierButtons.Length; j++)
				{
					VehicleTier vehicleTier = vehicleTable.tiers[j];
					ISleekButton sleekButton = Glazier.Get().CreateButton();
					sleekButton.PositionOffset_X = 240f;
					sleekButton.PositionOffset_Y = (float)(170 + j * 70);
					sleekButton.SizeOffset_X = 200f;
					sleekButton.SizeOffset_Y = 30f;
					sleekButton.Text = vehicleTier.name;
					sleekButton.OnClicked += new ClickedButton(EditorSpawnsVehiclesUI.onClickedTierButton);
					EditorSpawnsVehiclesUI.spawnsScrollBox.AddChild(sleekButton);
					ISleekSlider sleekSlider = Glazier.Get().CreateSlider();
					sleekSlider.PositionOffset_Y = 40f;
					sleekSlider.SizeOffset_X = 200f;
					sleekSlider.SizeOffset_Y = 20f;
					sleekSlider.Orientation = 0;
					sleekSlider.Value = vehicleTier.chance;
					sleekSlider.AddLabel(Mathf.RoundToInt(vehicleTier.chance * 100f).ToString() + "%", 0);
					sleekSlider.OnValueChanged += new Dragged(EditorSpawnsVehiclesUI.onDraggedChanceSlider);
					sleekButton.AddChild(sleekSlider);
					EditorSpawnsVehiclesUI.tierButtons[j] = sleekButton;
				}
				EditorSpawnsVehiclesUI.tierNameField.PositionOffset_Y = (float)(170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70);
				EditorSpawnsVehiclesUI.addTierButton.PositionOffset_Y = (float)(170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 40);
				EditorSpawnsVehiclesUI.removeTierButton.PositionOffset_Y = (float)(170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 40);
				if (EditorSpawnsVehiclesUI.vehicleButtons != null)
				{
					for (int k = 0; k < EditorSpawnsVehiclesUI.vehicleButtons.Length; k++)
					{
						EditorSpawnsVehiclesUI.spawnsScrollBox.RemoveChild(EditorSpawnsVehiclesUI.vehicleButtons[k]);
					}
				}
				if ((int)EditorSpawnsVehiclesUI.selectedTier < vehicleTable.tiers.Count)
				{
					EditorSpawnsVehiclesUI.tierNameField.Text = vehicleTable.tiers[(int)EditorSpawnsVehiclesUI.selectedTier].name;
					EditorSpawnsVehiclesUI.vehicleButtons = new ISleekButton[vehicleTable.tiers[(int)EditorSpawnsVehiclesUI.selectedTier].table.Count];
					for (int l = 0; l < EditorSpawnsVehiclesUI.vehicleButtons.Length; l++)
					{
						ISleekButton sleekButton2 = Glazier.Get().CreateButton();
						sleekButton2.PositionOffset_X = 240f;
						sleekButton2.PositionOffset_Y = (float)(170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + l * 40);
						sleekButton2.SizeOffset_X = 200f;
						sleekButton2.SizeOffset_Y = 30f;
						VehicleAsset vehicleAsset = VehicleTool.FindVehicleByLegacyIdAndHandleRedirects(vehicleTable.tiers[(int)EditorSpawnsVehiclesUI.selectedTier].table[l].vehicle);
						string text = "?";
						if (vehicleAsset != null)
						{
							if (string.IsNullOrEmpty(vehicleAsset.vehicleName))
							{
								text = vehicleAsset.name;
							}
							else
							{
								text = vehicleAsset.vehicleName;
							}
						}
						sleekButton2.Text = vehicleTable.tiers[(int)EditorSpawnsVehiclesUI.selectedTier].table[l].vehicle.ToString() + " " + text;
						sleekButton2.OnClicked += new ClickedButton(EditorSpawnsVehiclesUI.onClickVehicleButton);
						EditorSpawnsVehiclesUI.spawnsScrollBox.AddChild(sleekButton2);
						EditorSpawnsVehiclesUI.vehicleButtons[l] = sleekButton2;
					}
				}
				else
				{
					EditorSpawnsVehiclesUI.tierNameField.Text = "";
					EditorSpawnsVehiclesUI.vehicleButtons = new ISleekButton[0];
				}
				EditorSpawnsVehiclesUI.vehicleIDField.PositionOffset_Y = (float)(170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40);
				EditorSpawnsVehiclesUI.addVehicleButton.PositionOffset_Y = (float)(170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40 + 40);
				EditorSpawnsVehiclesUI.removeVehicleButton.PositionOffset_Y = (float)(170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40 + 40);
				EditorSpawnsVehiclesUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, (float)(170 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40 + 70));
				return;
			}
			EditorSpawnsVehiclesUI.selectedBox.Text = "";
			EditorSpawnsVehiclesUI.tableNameField.Text = "";
			EditorSpawnsVehiclesUI.tableIDField.Value = 0;
			EditorSpawnsVehiclesUI.tableColorPicker.state = Color.white;
			if (EditorSpawnsVehiclesUI.tierButtons != null)
			{
				for (int m = 0; m < EditorSpawnsVehiclesUI.tierButtons.Length; m++)
				{
					EditorSpawnsVehiclesUI.spawnsScrollBox.RemoveChild(EditorSpawnsVehiclesUI.tierButtons[m]);
				}
			}
			EditorSpawnsVehiclesUI.tierButtons = null;
			EditorSpawnsVehiclesUI.tierNameField.Text = "";
			EditorSpawnsVehiclesUI.tierNameField.PositionOffset_Y = 170f;
			EditorSpawnsVehiclesUI.addTierButton.PositionOffset_Y = 210f;
			EditorSpawnsVehiclesUI.removeTierButton.PositionOffset_Y = 210f;
			if (EditorSpawnsVehiclesUI.vehicleButtons != null)
			{
				for (int n = 0; n < EditorSpawnsVehiclesUI.vehicleButtons.Length; n++)
				{
					EditorSpawnsVehiclesUI.spawnsScrollBox.RemoveChild(EditorSpawnsVehiclesUI.vehicleButtons[n]);
				}
			}
			EditorSpawnsVehiclesUI.vehicleButtons = null;
			EditorSpawnsVehiclesUI.vehicleIDField.PositionOffset_Y = 250f;
			EditorSpawnsVehiclesUI.addVehicleButton.PositionOffset_Y = 290f;
			EditorSpawnsVehiclesUI.removeVehicleButton.PositionOffset_Y = 290f;
			EditorSpawnsVehiclesUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, 320f);
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x00139E98 File Offset: 0x00138098
		private static void onClickedTableButton(ISleekElement button)
		{
			if (EditorSpawns.selectedVehicle != (byte)(button.PositionOffset_Y / 40f))
			{
				EditorSpawns.selectedVehicle = (byte)(button.PositionOffset_Y / 40f);
				EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color;
				EditorSpawns.vehicleSpawn.Find("Arrow").GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color;
			}
			else
			{
				EditorSpawns.selectedVehicle = byte.MaxValue;
				EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = Color.white;
				EditorSpawns.vehicleSpawn.Find("Arrow").GetComponent<Renderer>().material.color = Color.white;
			}
			EditorSpawnsVehiclesUI.updateSelection();
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x00139F72 File Offset: 0x00138172
		private static void onVehicleColorPicked(SleekColorPicker picker, Color color)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color = color;
			}
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x00139F9A File Offset: 0x0013819A
		private static void onTableIDFieldTyped(ISleekUInt16Field field, ushort state)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tableID = state;
			}
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x00139FC4 File Offset: 0x001381C4
		private static void onClickedTierButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				if (EditorSpawnsVehiclesUI.selectedTier != (byte)((button.PositionOffset_Y - 170f) / 70f))
				{
					EditorSpawnsVehiclesUI.selectedTier = (byte)((button.PositionOffset_Y - 170f) / 70f);
				}
				else
				{
					EditorSpawnsVehiclesUI.selectedTier = byte.MaxValue;
				}
				EditorSpawnsVehiclesUI.updateSelection();
			}
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x0013A028 File Offset: 0x00138228
		private static void onClickVehicleButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				EditorSpawnsVehiclesUI.selectVehicle = (byte)((button.PositionOffset_Y - 170f - (float)(EditorSpawnsVehiclesUI.tierButtons.Length * 70) - 80f) / 40f);
				EditorSpawnsVehiclesUI.updateSelection();
			}
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x0013A078 File Offset: 0x00138278
		private static void onDraggedChanceSlider(ISleekSlider slider, float state)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				int num = Mathf.FloorToInt((slider.Parent.PositionOffset_Y - 170f) / 70f);
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].updateChance(num, state);
				for (int i = 0; i < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers.Count; i++)
				{
					VehicleTier vehicleTier = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers[i];
					ISleekSlider sleekSlider = (ISleekSlider)EditorSpawnsVehiclesUI.tierButtons[i].GetChildAtIndex(0);
					if (i != num)
					{
						sleekSlider.Value = vehicleTier.chance;
					}
					sleekSlider.UpdateLabel(Mathf.RoundToInt(vehicleTier.chance * 100f).ToString() + "%");
				}
			}
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x0013A15C File Offset: 0x0013835C
		private static void onTypedNameField(ISleekField field, string state)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				EditorSpawnsVehiclesUI.selectedBox.Text = state;
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].name = state;
				EditorSpawnsVehiclesUI.tableButtons[(int)EditorSpawns.selectedVehicle].Text = EditorSpawns.selectedVehicle.ToString() + " " + state;
			}
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x0013A1C0 File Offset: 0x001383C0
		private static void onClickedAddTableButton(ISleekElement button)
		{
			if (EditorSpawnsVehiclesUI.tableNameField.Text != "")
			{
				LevelVehicles.addTable(EditorSpawnsVehiclesUI.tableNameField.Text);
				EditorSpawnsVehiclesUI.tableNameField.Text = "";
				EditorSpawnsVehiclesUI.updateTables();
				EditorSpawnsVehiclesUI.tableScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x0013A210 File Offset: 0x00138410
		private static void onClickedRemoveTableButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				LevelVehicles.removeTable();
				EditorSpawnsVehiclesUI.updateTables();
				EditorSpawnsVehiclesUI.updateSelection();
				EditorSpawnsVehiclesUI.tableScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x0013A23C File Offset: 0x0013843C
		private static void onTypedTierNameField(ISleekField field, string state)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count && (int)EditorSpawnsVehiclesUI.selectedTier < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers.Count)
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers[(int)EditorSpawnsVehiclesUI.selectedTier].name = state;
				EditorSpawnsVehiclesUI.tierButtons[(int)EditorSpawnsVehiclesUI.selectedTier].Text = state;
			}
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x0013A2B0 File Offset: 0x001384B0
		private static void onClickedAddTierButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count && EditorSpawnsVehiclesUI.tierNameField.Text != "")
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].addTier(EditorSpawnsVehiclesUI.tierNameField.Text);
				EditorSpawnsVehiclesUI.tierNameField.Text = "";
				EditorSpawnsVehiclesUI.updateSelection();
			}
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x0013A318 File Offset: 0x00138518
		private static void onClickedRemoveTierButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count && (int)EditorSpawnsVehiclesUI.selectedTier < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers.Count)
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].removeTier((int)EditorSpawnsVehiclesUI.selectedTier);
				EditorSpawnsVehiclesUI.updateSelection();
			}
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x0013A374 File Offset: 0x00138574
		private static void onClickedAddVehicleButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count && (int)EditorSpawnsVehiclesUI.selectedTier < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers.Count)
			{
				if (VehicleTool.FindVehicleByLegacyIdAndHandleRedirects(EditorSpawnsVehiclesUI.vehicleIDField.Value) != null)
				{
					LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].addVehicle(EditorSpawnsVehiclesUI.selectedTier, EditorSpawnsVehiclesUI.vehicleIDField.Value);
					EditorSpawnsVehiclesUI.updateSelection();
					EditorSpawnsVehiclesUI.spawnsScrollBox.ScrollToBottom();
				}
				EditorSpawnsVehiclesUI.vehicleIDField.Value = 0;
			}
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x0013A400 File Offset: 0x00138600
		private static void onClickedRemoveVehicleButton(ISleekElement button)
		{
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count && (int)EditorSpawnsVehiclesUI.selectedTier < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers.Count && (int)EditorSpawnsVehiclesUI.selectVehicle < LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].tiers[(int)EditorSpawnsVehiclesUI.selectedTier].table.Count)
			{
				LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].removeVehicle(EditorSpawnsVehiclesUI.selectedTier, EditorSpawnsVehiclesUI.selectVehicle);
				EditorSpawnsVehiclesUI.updateSelection();
				EditorSpawnsVehiclesUI.spawnsScrollBox.ScrollToBottom();
			}
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x0013A49A File Offset: 0x0013869A
		private static void onDraggedRadiusSlider(ISleekSlider slider, float state)
		{
			EditorSpawns.radius = (byte)((float)EditorSpawns.MIN_REMOVE_SIZE + state * (float)EditorSpawns.MAX_REMOVE_SIZE);
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x0013A4B1 File Offset: 0x001386B1
		private static void onDraggedRotationSlider(ISleekSlider slider, float state)
		{
			EditorSpawns.rotation = state * 360f;
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x0013A4BF File Offset: 0x001386BF
		private static void onClickedAddButton(ISleekElement button)
		{
			EditorSpawns.spawnMode = ESpawnMode.ADD_VEHICLE;
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x0013A4C7 File Offset: 0x001386C7
		private static void onClickedRemoveButton(ISleekElement button)
		{
			EditorSpawns.spawnMode = ESpawnMode.REMOVE_VEHICLE;
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x0013A4D0 File Offset: 0x001386D0
		public EditorSpawnsVehiclesUI()
		{
			Local local = Localization.read("/Editor/EditorSpawnsVehicles.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawnsVehicles/EditorSpawnsVehicles.unity3d");
			EditorSpawnsVehiclesUI.container = new SleekFullscreenBox();
			EditorSpawnsVehiclesUI.container.PositionOffset_X = 10f;
			EditorSpawnsVehiclesUI.container.PositionOffset_Y = 10f;
			EditorSpawnsVehiclesUI.container.PositionScale_X = 1f;
			EditorSpawnsVehiclesUI.container.SizeOffset_X = -20f;
			EditorSpawnsVehiclesUI.container.SizeOffset_Y = -20f;
			EditorSpawnsVehiclesUI.container.SizeScale_X = 1f;
			EditorSpawnsVehiclesUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorSpawnsVehiclesUI.container);
			EditorSpawnsVehiclesUI.active = false;
			EditorSpawnsVehiclesUI.tableScrollBox = Glazier.Get().CreateScrollView();
			EditorSpawnsVehiclesUI.tableScrollBox.PositionOffset_X = -470f;
			EditorSpawnsVehiclesUI.tableScrollBox.PositionOffset_Y = 120f;
			EditorSpawnsVehiclesUI.tableScrollBox.PositionScale_X = 1f;
			EditorSpawnsVehiclesUI.tableScrollBox.SizeOffset_X = 470f;
			EditorSpawnsVehiclesUI.tableScrollBox.SizeOffset_Y = 200f;
			EditorSpawnsVehiclesUI.tableScrollBox.ScaleContentToWidth = true;
			EditorSpawnsVehiclesUI.container.AddChild(EditorSpawnsVehiclesUI.tableScrollBox);
			EditorSpawnsVehiclesUI.tableNameField = Glazier.Get().CreateStringField();
			EditorSpawnsVehiclesUI.tableNameField.PositionOffset_X = -230f;
			EditorSpawnsVehiclesUI.tableNameField.PositionOffset_Y = 330f;
			EditorSpawnsVehiclesUI.tableNameField.PositionScale_X = 1f;
			EditorSpawnsVehiclesUI.tableNameField.SizeOffset_X = 230f;
			EditorSpawnsVehiclesUI.tableNameField.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.tableNameField.MaxLength = 64;
			EditorSpawnsVehiclesUI.tableNameField.AddLabel(local.format("TableNameFieldLabelText"), 0);
			EditorSpawnsVehiclesUI.tableNameField.OnTextChanged += new Typed(EditorSpawnsVehiclesUI.onTypedNameField);
			EditorSpawnsVehiclesUI.container.AddChild(EditorSpawnsVehiclesUI.tableNameField);
			EditorSpawnsVehiclesUI.addTableButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsVehiclesUI.addTableButton.PositionOffset_X = -230f;
			EditorSpawnsVehiclesUI.addTableButton.PositionOffset_Y = 370f;
			EditorSpawnsVehiclesUI.addTableButton.PositionScale_X = 1f;
			EditorSpawnsVehiclesUI.addTableButton.SizeOffset_X = 110f;
			EditorSpawnsVehiclesUI.addTableButton.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.addTableButton.text = local.format("AddTableButtonText");
			EditorSpawnsVehiclesUI.addTableButton.tooltip = local.format("AddTableButtonTooltip");
			EditorSpawnsVehiclesUI.addTableButton.onClickedButton += new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddTableButton);
			EditorSpawnsVehiclesUI.container.AddChild(EditorSpawnsVehiclesUI.addTableButton);
			EditorSpawnsVehiclesUI.removeTableButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsVehiclesUI.removeTableButton.PositionOffset_X = -110f;
			EditorSpawnsVehiclesUI.removeTableButton.PositionOffset_Y = 370f;
			EditorSpawnsVehiclesUI.removeTableButton.PositionScale_X = 1f;
			EditorSpawnsVehiclesUI.removeTableButton.SizeOffset_X = 110f;
			EditorSpawnsVehiclesUI.removeTableButton.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.removeTableButton.text = local.format("RemoveTableButtonText");
			EditorSpawnsVehiclesUI.removeTableButton.tooltip = local.format("RemoveTableButtonTooltip");
			EditorSpawnsVehiclesUI.removeTableButton.onClickedButton += new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveTableButton);
			EditorSpawnsVehiclesUI.container.AddChild(EditorSpawnsVehiclesUI.removeTableButton);
			EditorSpawnsVehiclesUI.tableButtons = null;
			EditorSpawnsVehiclesUI.updateTables();
			EditorSpawnsVehiclesUI.spawnsScrollBox = Glazier.Get().CreateScrollView();
			EditorSpawnsVehiclesUI.spawnsScrollBox.PositionOffset_X = -470f;
			EditorSpawnsVehiclesUI.spawnsScrollBox.PositionOffset_Y = 410f;
			EditorSpawnsVehiclesUI.spawnsScrollBox.PositionScale_X = 1f;
			EditorSpawnsVehiclesUI.spawnsScrollBox.SizeOffset_X = 470f;
			EditorSpawnsVehiclesUI.spawnsScrollBox.SizeOffset_Y = -410f;
			EditorSpawnsVehiclesUI.spawnsScrollBox.SizeScale_Y = 1f;
			EditorSpawnsVehiclesUI.spawnsScrollBox.ScaleContentToWidth = true;
			EditorSpawnsVehiclesUI.spawnsScrollBox.ContentSizeOffset = new Vector2(0f, 1000f);
			EditorSpawnsVehiclesUI.container.AddChild(EditorSpawnsVehiclesUI.spawnsScrollBox);
			EditorSpawnsVehiclesUI.tableColorPicker = new SleekColorPicker();
			EditorSpawnsVehiclesUI.tableColorPicker.PositionOffset_X = 200f;
			EditorSpawnsVehiclesUI.tableColorPicker.onColorPicked = new ColorPicked(EditorSpawnsVehiclesUI.onVehicleColorPicked);
			EditorSpawnsVehiclesUI.spawnsScrollBox.AddChild(EditorSpawnsVehiclesUI.tableColorPicker);
			EditorSpawnsVehiclesUI.tableIDField = Glazier.Get().CreateUInt16Field();
			EditorSpawnsVehiclesUI.tableIDField.PositionOffset_X = 240f;
			EditorSpawnsVehiclesUI.tableIDField.PositionOffset_Y = 130f;
			EditorSpawnsVehiclesUI.tableIDField.SizeOffset_X = 200f;
			EditorSpawnsVehiclesUI.tableIDField.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.tableIDField.OnValueChanged += new TypedUInt16(EditorSpawnsVehiclesUI.onTableIDFieldTyped);
			EditorSpawnsVehiclesUI.tableIDField.AddLabel(local.format("TableIDFieldLabelText"), 0);
			EditorSpawnsVehiclesUI.spawnsScrollBox.AddChild(EditorSpawnsVehiclesUI.tableIDField);
			EditorSpawnsVehiclesUI.tierNameField = Glazier.Get().CreateStringField();
			EditorSpawnsVehiclesUI.tierNameField.PositionOffset_X = 240f;
			EditorSpawnsVehiclesUI.tierNameField.SizeOffset_X = 200f;
			EditorSpawnsVehiclesUI.tierNameField.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.tierNameField.MaxLength = 64;
			EditorSpawnsVehiclesUI.tierNameField.AddLabel(local.format("TierNameFieldLabelText"), 0);
			EditorSpawnsVehiclesUI.tierNameField.OnTextChanged += new Typed(EditorSpawnsVehiclesUI.onTypedTierNameField);
			EditorSpawnsVehiclesUI.spawnsScrollBox.AddChild(EditorSpawnsVehiclesUI.tierNameField);
			EditorSpawnsVehiclesUI.addTierButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsVehiclesUI.addTierButton.PositionOffset_X = 240f;
			EditorSpawnsVehiclesUI.addTierButton.SizeOffset_X = 95f;
			EditorSpawnsVehiclesUI.addTierButton.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.addTierButton.text = local.format("AddTierButtonText");
			EditorSpawnsVehiclesUI.addTierButton.tooltip = local.format("AddTierButtonTooltip");
			EditorSpawnsVehiclesUI.addTierButton.onClickedButton += new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddTierButton);
			EditorSpawnsVehiclesUI.spawnsScrollBox.AddChild(EditorSpawnsVehiclesUI.addTierButton);
			EditorSpawnsVehiclesUI.removeTierButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsVehiclesUI.removeTierButton.PositionOffset_X = 345f;
			EditorSpawnsVehiclesUI.removeTierButton.SizeOffset_X = 95f;
			EditorSpawnsVehiclesUI.removeTierButton.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.removeTierButton.text = local.format("RemoveTierButtonText");
			EditorSpawnsVehiclesUI.removeTierButton.tooltip = local.format("RemoveTierButtonTooltip");
			EditorSpawnsVehiclesUI.removeTierButton.onClickedButton += new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveTierButton);
			EditorSpawnsVehiclesUI.spawnsScrollBox.AddChild(EditorSpawnsVehiclesUI.removeTierButton);
			EditorSpawnsVehiclesUI.vehicleIDField = Glazier.Get().CreateUInt16Field();
			EditorSpawnsVehiclesUI.vehicleIDField.PositionOffset_X = 240f;
			EditorSpawnsVehiclesUI.vehicleIDField.SizeOffset_X = 200f;
			EditorSpawnsVehiclesUI.vehicleIDField.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.vehicleIDField.AddLabel(local.format("VehicleIDFieldLabelText"), 0);
			EditorSpawnsVehiclesUI.spawnsScrollBox.AddChild(EditorSpawnsVehiclesUI.vehicleIDField);
			EditorSpawnsVehiclesUI.addVehicleButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsVehiclesUI.addVehicleButton.PositionOffset_X = 240f;
			EditorSpawnsVehiclesUI.addVehicleButton.SizeOffset_X = 95f;
			EditorSpawnsVehiclesUI.addVehicleButton.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.addVehicleButton.text = local.format("AddVehicleButtonText");
			EditorSpawnsVehiclesUI.addVehicleButton.tooltip = local.format("AddVehicleButtonTooltip");
			EditorSpawnsVehiclesUI.addVehicleButton.onClickedButton += new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddVehicleButton);
			EditorSpawnsVehiclesUI.spawnsScrollBox.AddChild(EditorSpawnsVehiclesUI.addVehicleButton);
			EditorSpawnsVehiclesUI.removeVehicleButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsVehiclesUI.removeVehicleButton.PositionOffset_X = 345f;
			EditorSpawnsVehiclesUI.removeVehicleButton.SizeOffset_X = 95f;
			EditorSpawnsVehiclesUI.removeVehicleButton.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.removeVehicleButton.text = local.format("RemoveVehicleButtonText");
			EditorSpawnsVehiclesUI.removeVehicleButton.tooltip = local.format("RemoveVehicleButtonTooltip");
			EditorSpawnsVehiclesUI.removeVehicleButton.onClickedButton += new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveVehicleButton);
			EditorSpawnsVehiclesUI.spawnsScrollBox.AddChild(EditorSpawnsVehiclesUI.removeVehicleButton);
			EditorSpawnsVehiclesUI.selectedBox = Glazier.Get().CreateBox();
			EditorSpawnsVehiclesUI.selectedBox.PositionOffset_X = -230f;
			EditorSpawnsVehiclesUI.selectedBox.PositionOffset_Y = 80f;
			EditorSpawnsVehiclesUI.selectedBox.PositionScale_X = 1f;
			EditorSpawnsVehiclesUI.selectedBox.SizeOffset_X = 230f;
			EditorSpawnsVehiclesUI.selectedBox.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.selectedBox.AddLabel(local.format("SelectionBoxLabelText"), 0);
			EditorSpawnsVehiclesUI.container.AddChild(EditorSpawnsVehiclesUI.selectedBox);
			EditorSpawnsVehiclesUI.tierButtons = null;
			EditorSpawnsVehiclesUI.vehicleButtons = null;
			EditorSpawnsVehiclesUI.updateSelection();
			EditorSpawnsVehiclesUI.radiusSlider = Glazier.Get().CreateSlider();
			EditorSpawnsVehiclesUI.radiusSlider.PositionOffset_Y = -130f;
			EditorSpawnsVehiclesUI.radiusSlider.PositionScale_Y = 1f;
			EditorSpawnsVehiclesUI.radiusSlider.SizeOffset_X = 200f;
			EditorSpawnsVehiclesUI.radiusSlider.SizeOffset_Y = 20f;
			EditorSpawnsVehiclesUI.radiusSlider.Value = (float)(EditorSpawns.radius - EditorSpawns.MIN_REMOVE_SIZE) / (float)EditorSpawns.MAX_REMOVE_SIZE;
			EditorSpawnsVehiclesUI.radiusSlider.Orientation = 0;
			EditorSpawnsVehiclesUI.radiusSlider.AddLabel(local.format("RadiusSliderLabelText"), 1);
			EditorSpawnsVehiclesUI.radiusSlider.OnValueChanged += new Dragged(EditorSpawnsVehiclesUI.onDraggedRadiusSlider);
			EditorSpawnsVehiclesUI.container.AddChild(EditorSpawnsVehiclesUI.radiusSlider);
			EditorSpawnsVehiclesUI.rotationSlider = Glazier.Get().CreateSlider();
			EditorSpawnsVehiclesUI.rotationSlider.PositionOffset_Y = -100f;
			EditorSpawnsVehiclesUI.rotationSlider.PositionScale_Y = 1f;
			EditorSpawnsVehiclesUI.rotationSlider.SizeOffset_X = 200f;
			EditorSpawnsVehiclesUI.rotationSlider.SizeOffset_Y = 20f;
			EditorSpawnsVehiclesUI.rotationSlider.Value = EditorSpawns.rotation / 360f;
			EditorSpawnsVehiclesUI.rotationSlider.Orientation = 0;
			EditorSpawnsVehiclesUI.rotationSlider.AddLabel(local.format("RotationSliderLabelText"), 1);
			EditorSpawnsVehiclesUI.rotationSlider.OnValueChanged += new Dragged(EditorSpawnsVehiclesUI.onDraggedRotationSlider);
			EditorSpawnsVehiclesUI.container.AddChild(EditorSpawnsVehiclesUI.rotationSlider);
			EditorSpawnsVehiclesUI.addButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorSpawnsVehiclesUI.addButton.PositionOffset_Y = -70f;
			EditorSpawnsVehiclesUI.addButton.PositionScale_Y = 1f;
			EditorSpawnsVehiclesUI.addButton.SizeOffset_X = 200f;
			EditorSpawnsVehiclesUI.addButton.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.addButton.text = local.format("AddButtonText", ControlsSettings.tool_0);
			EditorSpawnsVehiclesUI.addButton.tooltip = local.format("AddButtonTooltip");
			EditorSpawnsVehiclesUI.addButton.onClickedButton += new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddButton);
			EditorSpawnsVehiclesUI.container.AddChild(EditorSpawnsVehiclesUI.addButton);
			EditorSpawnsVehiclesUI.removeButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorSpawnsVehiclesUI.removeButton.PositionOffset_Y = -30f;
			EditorSpawnsVehiclesUI.removeButton.PositionScale_Y = 1f;
			EditorSpawnsVehiclesUI.removeButton.SizeOffset_X = 200f;
			EditorSpawnsVehiclesUI.removeButton.SizeOffset_Y = 30f;
			EditorSpawnsVehiclesUI.removeButton.text = local.format("RemoveButtonText", ControlsSettings.tool_1);
			EditorSpawnsVehiclesUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
			EditorSpawnsVehiclesUI.removeButton.onClickedButton += new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveButton);
			EditorSpawnsVehiclesUI.container.AddChild(EditorSpawnsVehiclesUI.removeButton);
			bundle.unload();
		}

		// Token: 0x040027E7 RID: 10215
		private static SleekFullscreenBox container;

		// Token: 0x040027E8 RID: 10216
		public static bool active;

		// Token: 0x040027E9 RID: 10217
		private static ISleekScrollView tableScrollBox;

		// Token: 0x040027EA RID: 10218
		private static ISleekScrollView spawnsScrollBox;

		// Token: 0x040027EB RID: 10219
		private static ISleekButton[] tableButtons;

		// Token: 0x040027EC RID: 10220
		private static ISleekButton[] tierButtons;

		// Token: 0x040027ED RID: 10221
		private static ISleekButton[] vehicleButtons;

		// Token: 0x040027EE RID: 10222
		private static SleekColorPicker tableColorPicker;

		// Token: 0x040027EF RID: 10223
		private static ISleekUInt16Field tableIDField;

		// Token: 0x040027F0 RID: 10224
		private static ISleekField tierNameField;

		// Token: 0x040027F1 RID: 10225
		private static SleekButtonIcon addTierButton;

		// Token: 0x040027F2 RID: 10226
		private static SleekButtonIcon removeTierButton;

		// Token: 0x040027F3 RID: 10227
		private static ISleekUInt16Field vehicleIDField;

		// Token: 0x040027F4 RID: 10228
		private static SleekButtonIcon addVehicleButton;

		// Token: 0x040027F5 RID: 10229
		private static SleekButtonIcon removeVehicleButton;

		// Token: 0x040027F6 RID: 10230
		private static ISleekBox selectedBox;

		// Token: 0x040027F7 RID: 10231
		private static ISleekField tableNameField;

		// Token: 0x040027F8 RID: 10232
		private static SleekButtonIcon addTableButton;

		// Token: 0x040027F9 RID: 10233
		private static SleekButtonIcon removeTableButton;

		// Token: 0x040027FA RID: 10234
		private static ISleekSlider radiusSlider;

		// Token: 0x040027FB RID: 10235
		private static ISleekSlider rotationSlider;

		// Token: 0x040027FC RID: 10236
		private static SleekButtonIcon addButton;

		// Token: 0x040027FD RID: 10237
		private static SleekButtonIcon removeButton;

		// Token: 0x040027FE RID: 10238
		private static byte selectedTier;

		// Token: 0x040027FF RID: 10239
		private static byte selectVehicle;
	}
}
