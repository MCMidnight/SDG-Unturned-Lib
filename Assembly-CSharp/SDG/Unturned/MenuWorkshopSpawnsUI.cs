using System;
using System.IO;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x020007B6 RID: 1974
	public class MenuWorkshopSpawnsUI
	{
		// Token: 0x06004242 RID: 16962 RVA: 0x00169839 File Offset: 0x00167A39
		public static void open()
		{
			if (MenuWorkshopSpawnsUI.active)
			{
				return;
			}
			MenuWorkshopSpawnsUI.active = true;
			Localization.refresh();
			MenuWorkshopSpawnsUI.refresh();
			MenuWorkshopSpawnsUI.container.AnimateIntoView();
		}

		// Token: 0x06004243 RID: 16963 RVA: 0x0016985D File Offset: 0x00167A5D
		public static void close()
		{
			if (!MenuWorkshopSpawnsUI.active)
			{
				return;
			}
			MenuWorkshopSpawnsUI.active = false;
			MenuWorkshopSpawnsUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06004244 RID: 16964 RVA: 0x00169884 File Offset: 0x00167A84
		private static SpawnAsset FindCurrentAsset()
		{
			ushort id;
			if (ushort.TryParse(MenuWorkshopSpawnsUI.viewIDField.Text, ref id))
			{
				return Assets.find(EAssetType.SPAWN, id) as SpawnAsset;
			}
			Guid guid;
			if (Guid.TryParse(MenuWorkshopSpawnsUI.viewIDField.Text, ref guid))
			{
				return Assets.find(guid) as SpawnAsset;
			}
			return null;
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x001698D4 File Offset: 0x00167AD4
		private static void refresh()
		{
			MenuWorkshopSpawnsUI.rawField.IsVisible = false;
			MenuWorkshopSpawnsUI.rootsBox.IsVisible = true;
			MenuWorkshopSpawnsUI.tablesBox.IsVisible = true;
			MenuWorkshopSpawnsUI.rootsBox.RemoveAllChildren();
			MenuWorkshopSpawnsUI.tablesBox.RemoveAllChildren();
			MenuWorkshopSpawnsUI.asset = MenuWorkshopSpawnsUI.FindCurrentAsset();
			switch (MenuWorkshopSpawnsUI.typeButton.state)
			{
			case 0:
				MenuWorkshopSpawnsUI.type = EAssetType.ITEM;
				break;
			case 1:
				MenuWorkshopSpawnsUI.type = EAssetType.VEHICLE;
				break;
			case 2:
				MenuWorkshopSpawnsUI.type = EAssetType.ANIMAL;
				break;
			default:
				MenuWorkshopSpawnsUI.type = EAssetType.NONE;
				return;
			}
			int num = 120;
			MenuWorkshopSpawnsUI.rootsBox.PositionOffset_Y = (float)num;
			num += 40;
			if (MenuWorkshopSpawnsUI.asset != null)
			{
				MenuWorkshopSpawnsUI.rootsBox.Text = MenuWorkshopSpawnsUI.localization.format("Roots_Box", MenuWorkshopSpawnsUI.asset.name);
				int i = 0;
				while (i < MenuWorkshopSpawnsUI.asset.roots.Count)
				{
					SpawnTable spawnTable = MenuWorkshopSpawnsUI.asset.roots[i];
					SpawnAsset spawnAsset;
					if (spawnTable.legacySpawnId != 0)
					{
						spawnAsset = (Assets.find(EAssetType.SPAWN, spawnTable.legacySpawnId) as SpawnAsset);
						goto IL_116;
					}
					if (!GuidExtension.IsEmpty(spawnTable.targetGuid))
					{
						spawnAsset = (Assets.find(spawnTable.targetGuid) as SpawnAsset);
						goto IL_116;
					}
					IL_39A:
					i++;
					continue;
					IL_116:
					ISleekButton sleekButton = Glazier.Get().CreateButton();
					sleekButton.PositionOffset_Y = (float)(40 + i * 40);
					sleekButton.SizeOffset_X = -260f;
					sleekButton.SizeScale_X = 1f;
					sleekButton.SizeOffset_Y = 30f;
					sleekButton.OnClicked += new ClickedButton(MenuWorkshopSpawnsUI.onClickedRootButton);
					MenuWorkshopSpawnsUI.rootsBox.AddChild(sleekButton);
					num += 40;
					if (spawnAsset != null)
					{
						sleekButton.Text = spawnAsset.name;
						if (spawnTable.legacySpawnId != 0)
						{
							sleekButton.TooltipText = string.Format("{0} - {1}", spawnTable.legacySpawnId, spawnAsset.GetOriginName());
						}
						else
						{
							sleekButton.TooltipText = string.Format("{0:N} - {1}", spawnTable.targetGuid, spawnAsset.GetOriginName());
						}
					}
					else if (spawnTable.legacySpawnId != 0)
					{
						sleekButton.Text = string.Format("{0} ?", spawnTable.legacySpawnId);
					}
					else
					{
						sleekButton.Text = string.Format("{0:N} ?", spawnTable.targetGuid);
					}
					ISleekInt32Field sleekInt32Field = Glazier.Get().CreateInt32Field();
					sleekInt32Field.PositionOffset_X = 10f;
					sleekInt32Field.PositionScale_X = 1f;
					sleekInt32Field.SizeOffset_X = 55f;
					sleekInt32Field.SizeOffset_Y = 30f;
					sleekInt32Field.Value = spawnTable.weight;
					sleekInt32Field.TooltipText = MenuWorkshopSpawnsUI.localization.format("Weight_Tooltip");
					sleekInt32Field.OnValueChanged += new TypedInt32(MenuWorkshopSpawnsUI.onTypedRootWeightField);
					sleekButton.AddChild(sleekInt32Field);
					ISleekBox sleekBox = Glazier.Get().CreateBox();
					sleekBox.PositionOffset_X = 75f;
					sleekBox.PositionScale_X = 1f;
					sleekBox.SizeOffset_X = 55f;
					sleekBox.SizeOffset_Y = 30f;
					sleekBox.Text = spawnTable.normalizedWeight.ToString("P");
					sleekBox.TooltipText = MenuWorkshopSpawnsUI.localization.format("Chance_Tooltip");
					sleekButton.AddChild(sleekBox);
					SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(MenuWorkshopEditorUI.icons.load<Texture2D>("Remove"));
					sleekButtonIcon.PositionOffset_X = 140f;
					sleekButtonIcon.PositionScale_X = 1f;
					sleekButtonIcon.SizeOffset_X = 120f;
					sleekButtonIcon.SizeOffset_Y = 30f;
					sleekButtonIcon.text = MenuWorkshopSpawnsUI.localization.format("Remove_Root_Button");
					sleekButtonIcon.tooltip = MenuWorkshopSpawnsUI.localization.format("Remove_Root_Button_Tooltip");
					sleekButtonIcon.onClickedButton += new ClickedButton(MenuWorkshopSpawnsUI.onClickedRemoveRootButton);
					sleekButton.AddChild(sleekButtonIcon);
					goto IL_39A;
				}
				MenuWorkshopSpawnsUI.addRootIDField.PositionOffset_Y = (float)num;
				MenuWorkshopSpawnsUI.addRootSpawnButton.PositionOffset_Y = (float)num;
				num += 40;
				MenuWorkshopSpawnsUI.addRootIDField.IsVisible = true;
				MenuWorkshopSpawnsUI.addRootSpawnButton.IsVisible = true;
			}
			else
			{
				MenuWorkshopSpawnsUI.rootsBox.Text = MenuWorkshopSpawnsUI.localization.format("Roots_Box", MenuWorkshopSpawnsUI.viewIDField.Text + " ?");
				MenuWorkshopSpawnsUI.addRootIDField.IsVisible = false;
				MenuWorkshopSpawnsUI.addRootSpawnButton.IsVisible = false;
			}
			num += 40;
			MenuWorkshopSpawnsUI.tablesBox.PositionOffset_Y = (float)num;
			num += 40;
			if (MenuWorkshopSpawnsUI.asset != null)
			{
				MenuWorkshopSpawnsUI.tablesBox.Text = MenuWorkshopSpawnsUI.localization.format("Tables_Box", MenuWorkshopSpawnsUI.asset.name);
				for (int j = 0; j < MenuWorkshopSpawnsUI.asset.tables.Count; j++)
				{
					SpawnTable spawnTable2 = MenuWorkshopSpawnsUI.asset.tables[j];
					Asset asset;
					SpawnAsset spawnAsset2;
					bool flag;
					if (spawnTable2.legacySpawnId != 0)
					{
						asset = null;
						spawnAsset2 = (Assets.find(EAssetType.SPAWN, spawnTable2.legacySpawnId) as SpawnAsset);
						flag = true;
					}
					else if (spawnTable2.legacyAssetId != 0)
					{
						asset = Assets.find(MenuWorkshopSpawnsUI.type, spawnTable2.legacyAssetId);
						spawnAsset2 = null;
						flag = false;
					}
					else
					{
						asset = Assets.find(spawnTable2.targetGuid);
						spawnAsset2 = (asset as SpawnAsset);
						flag = (spawnAsset2 != null);
					}
					ISleekElement sleekElement;
					if (flag)
					{
						ISleekButton sleekButton2 = Glazier.Get().CreateButton();
						sleekButton2.PositionOffset_Y = (float)(40 + j * 40);
						sleekButton2.SizeOffset_X = -260f;
						sleekButton2.SizeScale_X = 1f;
						sleekButton2.SizeOffset_Y = 30f;
						sleekButton2.OnClicked += new ClickedButton(MenuWorkshopSpawnsUI.onClickedTableButton);
						MenuWorkshopSpawnsUI.tablesBox.AddChild(sleekButton2);
						sleekElement = sleekButton2;
						num += 40;
						if (spawnAsset2 != null)
						{
							sleekButton2.Text = spawnAsset2.name;
							if (spawnTable2.legacySpawnId != 0)
							{
								sleekButton2.TooltipText = string.Format("{0} - {1}", spawnTable2.legacySpawnId, spawnAsset2.GetOriginName());
							}
							else
							{
								sleekButton2.TooltipText = string.Format("{0:N} - {1}", spawnTable2.targetGuid, spawnAsset2.GetOriginName());
							}
						}
						else if (spawnTable2.legacySpawnId != 0)
						{
							sleekButton2.Text = string.Format("{0} ?", spawnTable2.legacySpawnId);
						}
						else
						{
							sleekButton2.Text = string.Format("{0:N} ?", spawnTable2.targetGuid);
						}
					}
					else
					{
						ISleekBox sleekBox2 = Glazier.Get().CreateBox();
						sleekBox2.PositionOffset_Y = (float)(40 + j * 40);
						sleekBox2.SizeOffset_X = -260f;
						sleekBox2.SizeScale_X = 1f;
						sleekBox2.SizeOffset_Y = 30f;
						MenuWorkshopSpawnsUI.tablesBox.AddChild(sleekBox2);
						sleekElement = sleekBox2;
						num += 40;
						if (asset != null)
						{
							sleekBox2.Text = asset.FriendlyName;
							ItemAsset itemAsset = asset as ItemAsset;
							if (itemAsset != null)
							{
								sleekBox2.TextColor = ItemTool.getRarityColorUI(itemAsset.rarity);
							}
							else
							{
								VehicleAsset vehicleAsset = asset as VehicleAsset;
								if (vehicleAsset != null)
								{
									sleekBox2.TextColor = ItemTool.getRarityColorUI(vehicleAsset.rarity);
								}
							}
							if (spawnTable2.legacyAssetId != 0)
							{
								sleekBox2.TooltipText = string.Format("{0} - {1}", spawnTable2.legacyAssetId, asset.GetOriginName());
							}
							else
							{
								sleekBox2.TooltipText = string.Format("{0:N} - {1}", spawnTable2.targetGuid, asset.GetOriginName());
							}
						}
						else if (spawnTable2.legacyAssetId != 0)
						{
							sleekBox2.Text = string.Format("{0} ?", spawnTable2.legacyAssetId);
						}
						else
						{
							sleekBox2.Text = string.Format("{0:N} ?", spawnTable2.targetGuid);
						}
					}
					if (sleekElement != null)
					{
						ISleekInt32Field sleekInt32Field2 = Glazier.Get().CreateInt32Field();
						sleekInt32Field2.PositionOffset_X = 10f;
						sleekInt32Field2.PositionScale_X = 1f;
						sleekInt32Field2.SizeOffset_X = 55f;
						sleekInt32Field2.SizeOffset_Y = 30f;
						sleekInt32Field2.Value = spawnTable2.weight;
						sleekInt32Field2.TooltipText = MenuWorkshopSpawnsUI.localization.format("Weight_Tooltip");
						sleekInt32Field2.OnValueChanged += new TypedInt32(MenuWorkshopSpawnsUI.onTypedTableWeightField);
						sleekElement.AddChild(sleekInt32Field2);
						float num2 = spawnTable2.normalizedWeight;
						if (j > 0)
						{
							num2 -= MenuWorkshopSpawnsUI.asset.tables[j - 1].normalizedWeight;
						}
						ISleekBox sleekBox3 = Glazier.Get().CreateBox();
						sleekBox3.PositionOffset_X = 75f;
						sleekBox3.PositionScale_X = 1f;
						sleekBox3.SizeOffset_X = 55f;
						sleekBox3.SizeOffset_Y = 30f;
						sleekBox3.Text = num2.ToString("P");
						sleekBox3.TooltipText = MenuWorkshopSpawnsUI.localization.format("Chance_Tooltip");
						sleekElement.AddChild(sleekBox3);
						SleekButtonIcon sleekButtonIcon2 = new SleekButtonIcon(MenuWorkshopEditorUI.icons.load<Texture2D>("Remove"));
						sleekButtonIcon2.PositionOffset_X = 140f;
						sleekButtonIcon2.PositionScale_X = 1f;
						sleekButtonIcon2.SizeOffset_X = 120f;
						sleekButtonIcon2.SizeOffset_Y = 30f;
						sleekButtonIcon2.text = MenuWorkshopSpawnsUI.localization.format("Remove_Table_Button");
						sleekButtonIcon2.tooltip = MenuWorkshopSpawnsUI.localization.format("Remove_Table_Button_Tooltip");
						sleekButtonIcon2.onClickedButton += new ClickedButton(MenuWorkshopSpawnsUI.onClickedRemoveTableButton);
						sleekElement.AddChild(sleekButtonIcon2);
					}
				}
				MenuWorkshopSpawnsUI.addTableIDField.PositionOffset_Y = (float)num;
				MenuWorkshopSpawnsUI.addTableAssetButton.PositionOffset_Y = (float)num;
				MenuWorkshopSpawnsUI.addTableSpawnButton.PositionOffset_Y = (float)num;
				num += 40;
				MenuWorkshopSpawnsUI.addTableIDField.IsVisible = true;
				MenuWorkshopSpawnsUI.addTableAssetButton.IsVisible = true;
				MenuWorkshopSpawnsUI.addTableSpawnButton.IsVisible = true;
			}
			else
			{
				MenuWorkshopSpawnsUI.tablesBox.Text = MenuWorkshopSpawnsUI.localization.format("Tables_Box", MenuWorkshopSpawnsUI.viewIDField.Text + " ?");
				MenuWorkshopSpawnsUI.addTableIDField.IsVisible = false;
				MenuWorkshopSpawnsUI.addTableAssetButton.IsVisible = false;
				MenuWorkshopSpawnsUI.addTableSpawnButton.IsVisible = false;
			}
			if (MenuWorkshopSpawnsUI.asset != null)
			{
				MenuWorkshopSpawnsUI.applyWeightsButton.PositionOffset_Y = (float)num;
				num += 40;
				MenuWorkshopSpawnsUI.applyWeightsButton.IsVisible = true;
			}
			else
			{
				MenuWorkshopSpawnsUI.applyWeightsButton.IsVisible = false;
			}
			MenuWorkshopSpawnsUI.spawnsBox.ContentSizeOffset = new Vector2(0f, (float)(num - 10));
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x0016A2F8 File Offset: 0x001684F8
		private static string getRaw(SpawnAsset asset)
		{
			string result = null;
			using (StringWriter stringWriter = new StringWriter())
			{
				using (DatWriter datWriter = new DatWriter(stringWriter))
				{
					datWriter.WriteKeyValue("GUID", asset.GUID, null);
					datWriter.WriteKeyValue("Type", "Spawn", null);
					if (asset.id != 0)
					{
						datWriter.WriteKeyValue("ID", asset.id, null);
					}
					bool flag = false;
					if (asset.roots != null)
					{
						foreach (SpawnTable spawnTable in asset.roots)
						{
							if (spawnTable.isLink && (spawnTable.weight > 0 || spawnTable.isOverride))
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						datWriter.WriteEmptyLine();
						datWriter.WriteListStart("Roots");
						foreach (SpawnTable spawnTable2 in asset.roots)
						{
							if (spawnTable2.isLink && (spawnTable2.weight > 0 || spawnTable2.isOverride))
							{
								datWriter.WriteDictionaryStart();
								spawnTable2.Write(datWriter, MenuWorkshopSpawnsUI.type);
								datWriter.WriteDictionaryEnd();
							}
						}
						datWriter.WriteListEnd();
					}
					bool flag2 = false;
					if (asset.tables != null)
					{
						foreach (SpawnTable spawnTable3 in asset.tables)
						{
							if (!spawnTable3.isLink && spawnTable3.weight > 0)
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag2)
					{
						datWriter.WriteEmptyLine();
						datWriter.WriteListStart("Tables");
						foreach (SpawnTable spawnTable4 in asset.tables)
						{
							if (!spawnTable4.isLink && spawnTable4.weight > 0)
							{
								datWriter.WriteDictionaryStart();
								spawnTable4.Write(datWriter, MenuWorkshopSpawnsUI.type);
								datWriter.WriteDictionaryEnd();
							}
						}
						datWriter.WriteListEnd();
					}
					result = stringWriter.ToString();
				}
			}
			return result;
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x0016A5AC File Offset: 0x001687AC
		private static void raw()
		{
			MenuWorkshopSpawnsUI.rawField.IsVisible = true;
			MenuWorkshopSpawnsUI.rootsBox.IsVisible = false;
			MenuWorkshopSpawnsUI.tablesBox.IsVisible = false;
			MenuWorkshopSpawnsUI.addRootIDField.IsVisible = false;
			MenuWorkshopSpawnsUI.addRootSpawnButton.IsVisible = false;
			MenuWorkshopSpawnsUI.addTableIDField.IsVisible = false;
			MenuWorkshopSpawnsUI.addTableAssetButton.IsVisible = false;
			MenuWorkshopSpawnsUI.addTableSpawnButton.IsVisible = false;
			MenuWorkshopSpawnsUI.applyWeightsButton.IsVisible = false;
			MenuWorkshopSpawnsUI.asset = MenuWorkshopSpawnsUI.FindCurrentAsset();
			string text;
			if (MenuWorkshopSpawnsUI.asset != null)
			{
				text = MenuWorkshopSpawnsUI.getRaw(MenuWorkshopSpawnsUI.asset);
			}
			else
			{
				text = "?";
			}
			MenuWorkshopSpawnsUI.rawField.Text = text;
			GUIUtility.systemCopyBuffer = text;
			MenuWorkshopSpawnsUI.spawnsBox.ContentSizeOffset = new Vector2(0f, 1080f);
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x0016A66C File Offset: 0x0016886C
		private static void write()
		{
			MenuWorkshopSpawnsUI.asset = MenuWorkshopSpawnsUI.FindCurrentAsset();
			if (MenuWorkshopSpawnsUI.asset == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(MenuWorkshopSpawnsUI.asset.absoluteOriginFilePath) || !File.Exists(MenuWorkshopSpawnsUI.asset.absoluteOriginFilePath))
			{
				return;
			}
			string raw = MenuWorkshopSpawnsUI.getRaw(MenuWorkshopSpawnsUI.asset);
			File.WriteAllText(MenuWorkshopSpawnsUI.asset.absoluteOriginFilePath, raw);
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x0016A6C9 File Offset: 0x001688C9
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuWorkshopUI.open();
			MenuWorkshopSpawnsUI.close();
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x0016A6D5 File Offset: 0x001688D5
		private static void onClickedViewButton(ISleekElement button)
		{
			MenuWorkshopSpawnsUI.refresh();
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x0016A6DC File Offset: 0x001688DC
		private static void onClickedRawButton(ISleekElement button)
		{
			MenuWorkshopSpawnsUI.raw();
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x0016A6E4 File Offset: 0x001688E4
		private static void onClickedNewButton(ISleekElement button)
		{
			ushort legacyId;
			ushort.TryParse(MenuWorkshopSpawnsUI.viewIDField.Text, ref legacyId);
			SpawnAsset spawnAsset = Assets.CreateAtRuntime<SpawnAsset>(legacyId);
			if (spawnAsset != null)
			{
				MenuWorkshopSpawnsUI.viewIDField.Text = spawnAsset.GUID.ToString("N");
				MenuWorkshopSpawnsUI.refresh();
			}
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x0016A72C File Offset: 0x0016892C
		private static void onClickedWriteButton(ISleekElement button)
		{
			MenuWorkshopSpawnsUI.write();
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x0016A734 File Offset: 0x00168934
		private static void onClickedRootButton(ISleekElement button)
		{
			int num = MenuWorkshopSpawnsUI.rootsBox.FindIndexOfChild(button);
			SpawnTable spawnTable = MenuWorkshopSpawnsUI.asset.roots[num];
			if (spawnTable.legacySpawnId != 0)
			{
				MenuWorkshopSpawnsUI.viewIDField.Text = spawnTable.legacySpawnId.ToString();
			}
			else
			{
				MenuWorkshopSpawnsUI.viewIDField.Text = spawnTable.targetGuid.ToString("N");
			}
			MenuWorkshopSpawnsUI.refresh();
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x0016A79C File Offset: 0x0016899C
		private static void onClickedTableButton(ISleekElement button)
		{
			int num = MenuWorkshopSpawnsUI.tablesBox.FindIndexOfChild(button);
			SpawnTable spawnTable = MenuWorkshopSpawnsUI.asset.tables[num];
			if (spawnTable.legacySpawnId != 0)
			{
				MenuWorkshopSpawnsUI.viewIDField.Text = spawnTable.legacySpawnId.ToString();
			}
			else
			{
				MenuWorkshopSpawnsUI.viewIDField.Text = spawnTable.targetGuid.ToString("N");
			}
			MenuWorkshopSpawnsUI.refresh();
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x0016A804 File Offset: 0x00168A04
		private static void onTypedRootWeightField(ISleekInt32Field field, int state)
		{
			int num = MenuWorkshopSpawnsUI.rootsBox.FindIndexOfChild(field.Parent);
			MenuWorkshopSpawnsUI.asset.roots[num].weight = state;
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x0016A838 File Offset: 0x00168A38
		private static void onClickedAddRootSpawnButton(ISleekElement button)
		{
			ushort id;
			SpawnAsset spawnAsset;
			Guid guid;
			if (ushort.TryParse(MenuWorkshopSpawnsUI.addRootIDField.Text, ref id))
			{
				spawnAsset = (Assets.find(EAssetType.SPAWN, id) as SpawnAsset);
			}
			else if (Guid.TryParse(MenuWorkshopSpawnsUI.addRootIDField.Text, ref guid))
			{
				spawnAsset = Assets.find<SpawnAsset>(guid);
			}
			else
			{
				spawnAsset = null;
			}
			if (spawnAsset == null)
			{
				UnturnedLog.info("Spawns editor unable to find parent spawn asset matching \"" + MenuWorkshopSpawnsUI.addRootIDField.Text + "\"");
				return;
			}
			foreach (SpawnTable spawnTable in MenuWorkshopSpawnsUI.asset.roots)
			{
				if ((spawnTable.legacySpawnId != 0 && spawnTable.legacySpawnId == spawnAsset.id) || spawnTable.targetGuid == spawnAsset.GUID)
				{
					UnturnedLog.info("Spawns editor current asset " + MenuWorkshopSpawnsUI.asset.FriendlyName + " already contains parent " + spawnAsset.FriendlyName);
					return;
				}
			}
			SpawnTable spawnTable2 = new SpawnTable();
			spawnTable2.targetGuid = spawnAsset.GUID;
			spawnTable2.isLink = true;
			MenuWorkshopSpawnsUI.asset.roots.Add(spawnTable2);
			SpawnTable spawnTable3 = new SpawnTable();
			spawnTable3.targetGuid = MenuWorkshopSpawnsUI.asset.GUID;
			spawnTable3.isLink = true;
			spawnAsset.tables.Add(spawnTable3);
			spawnAsset.markTablesDirty();
			MenuWorkshopSpawnsUI.addRootIDField.Text = string.Empty;
			MenuWorkshopSpawnsUI.refresh();
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x0016A9B0 File Offset: 0x00168BB0
		private static void onClickedRemoveRootButton(ISleekElement button)
		{
			int parentIndex = MenuWorkshopSpawnsUI.rootsBox.FindIndexOfChild(button.Parent);
			MenuWorkshopSpawnsUI.asset.EditorRemoveParentAtIndex(parentIndex);
			MenuWorkshopSpawnsUI.refresh();
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x0016A9E0 File Offset: 0x00168BE0
		private static void onTypedTableWeightField(ISleekInt32Field field, int state)
		{
			int tableIndex = MenuWorkshopSpawnsUI.tablesBox.FindIndexOfChild(field.Parent);
			MenuWorkshopSpawnsUI.asset.setTableWeightAtIndex(tableIndex, state);
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x0016AA0C File Offset: 0x00168C0C
		private static void onClickedAddTableAssetButton(ISleekElement button)
		{
			ushort id;
			Asset asset;
			Guid guid;
			if (ushort.TryParse(MenuWorkshopSpawnsUI.addTableIDField.Text, ref id))
			{
				asset = Assets.find(MenuWorkshopSpawnsUI.type, id);
			}
			else if (Guid.TryParse(MenuWorkshopSpawnsUI.addTableIDField.Text, ref guid))
			{
				asset = Assets.find(guid);
			}
			else
			{
				asset = null;
			}
			if (asset == null)
			{
				UnturnedLog.info("Spawns editor unable to find child asset matching \"" + MenuWorkshopSpawnsUI.addRootIDField.Text + "\"");
				return;
			}
			foreach (SpawnTable spawnTable in MenuWorkshopSpawnsUI.asset.tables)
			{
				if ((spawnTable.legacyAssetId != 0 && spawnTable.legacyAssetId == asset.id) || spawnTable.targetGuid == asset.GUID)
				{
					UnturnedLog.info("Spawns editor current asset " + MenuWorkshopSpawnsUI.asset.FriendlyName + " already contains child asset " + asset.FriendlyName);
					return;
				}
			}
			MenuWorkshopSpawnsUI.asset.EditorAddChild(asset);
			MenuWorkshopSpawnsUI.addTableIDField.Text = string.Empty;
			MenuWorkshopSpawnsUI.refresh();
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x0016AB30 File Offset: 0x00168D30
		private static void onClickedAddTableSpawnButton(ISleekElement button)
		{
			ushort id;
			SpawnAsset spawnAsset;
			Guid guid;
			if (ushort.TryParse(MenuWorkshopSpawnsUI.addTableIDField.Text, ref id))
			{
				spawnAsset = (Assets.find(EAssetType.SPAWN, id) as SpawnAsset);
			}
			else if (Guid.TryParse(MenuWorkshopSpawnsUI.addTableIDField.Text, ref guid))
			{
				spawnAsset = (Assets.find(guid) as SpawnAsset);
			}
			else
			{
				spawnAsset = null;
			}
			if (spawnAsset == null)
			{
				UnturnedLog.info("Spawns editor unable to find child spawn matching \"" + MenuWorkshopSpawnsUI.addTableIDField.Text + "\"");
				return;
			}
			foreach (SpawnTable spawnTable in MenuWorkshopSpawnsUI.asset.tables)
			{
				if ((spawnTable.legacySpawnId != 0 && spawnTable.legacySpawnId == spawnAsset.id) || spawnTable.targetGuid == spawnAsset.GUID)
				{
					UnturnedLog.info("Spawns editor current asset " + MenuWorkshopSpawnsUI.asset.FriendlyName + " already contains child spawn " + spawnAsset.FriendlyName);
					return;
				}
			}
			MenuWorkshopSpawnsUI.asset.EditorAddChild(spawnAsset);
			MenuWorkshopSpawnsUI.addTableIDField.Text = string.Empty;
			MenuWorkshopSpawnsUI.refresh();
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x0016AC5C File Offset: 0x00168E5C
		private static void onClickedRemoveTableButton(ISleekElement button)
		{
			int childIndex = MenuWorkshopSpawnsUI.tablesBox.FindIndexOfChild(button.Parent);
			MenuWorkshopSpawnsUI.asset.EditorRemoveChildAtIndex(childIndex);
			MenuWorkshopSpawnsUI.refresh();
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x0016AC8A File Offset: 0x00168E8A
		private static void onClickedApplyWeightsButton(ISleekElement button)
		{
			MenuWorkshopSpawnsUI.asset.sortAndNormalizeWeights();
			MenuWorkshopSpawnsUI.refresh();
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x0016AC9C File Offset: 0x00168E9C
		public MenuWorkshopSpawnsUI()
		{
			MenuWorkshopSpawnsUI.localization = Localization.read("/Menu/Workshop/MenuWorkshopSpawns.dat");
			MenuWorkshopSpawnsUI.container = new SleekFullscreenBox();
			MenuWorkshopSpawnsUI.container.PositionOffset_X = 10f;
			MenuWorkshopSpawnsUI.container.PositionOffset_Y = 10f;
			MenuWorkshopSpawnsUI.container.PositionScale_Y = 1f;
			MenuWorkshopSpawnsUI.container.SizeOffset_X = -20f;
			MenuWorkshopSpawnsUI.container.SizeOffset_Y = -20f;
			MenuWorkshopSpawnsUI.container.SizeScale_X = 1f;
			MenuWorkshopSpawnsUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuWorkshopSpawnsUI.container);
			MenuWorkshopSpawnsUI.active = false;
			MenuWorkshopSpawnsUI.spawnsBox = Glazier.Get().CreateScrollView();
			MenuWorkshopSpawnsUI.spawnsBox.PositionOffset_X = -315f;
			MenuWorkshopSpawnsUI.spawnsBox.PositionOffset_Y = 100f;
			MenuWorkshopSpawnsUI.spawnsBox.PositionScale_X = 0.5f;
			MenuWorkshopSpawnsUI.spawnsBox.SizeOffset_X = 630f;
			MenuWorkshopSpawnsUI.spawnsBox.SizeOffset_Y = -200f;
			MenuWorkshopSpawnsUI.spawnsBox.SizeScale_Y = 1f;
			MenuWorkshopSpawnsUI.spawnsBox.ScaleContentToWidth = true;
			MenuWorkshopSpawnsUI.container.AddChild(MenuWorkshopSpawnsUI.spawnsBox);
			MenuWorkshopSpawnsUI.typeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuWorkshopSpawnsUI.localization.format("Type_Item")),
				new GUIContent(MenuWorkshopSpawnsUI.localization.format("Type_Vehicle")),
				new GUIContent(MenuWorkshopSpawnsUI.localization.format("Type_Animal"))
			});
			MenuWorkshopSpawnsUI.typeButton.SizeOffset_X = 600f;
			MenuWorkshopSpawnsUI.typeButton.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.typeButton.tooltip = MenuWorkshopSpawnsUI.localization.format("Type_Tooltip");
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.typeButton);
			MenuWorkshopSpawnsUI.viewIDField = Glazier.Get().CreateStringField();
			MenuWorkshopSpawnsUI.viewIDField.PositionOffset_Y = 40f;
			MenuWorkshopSpawnsUI.viewIDField.SizeOffset_X = 160f;
			MenuWorkshopSpawnsUI.viewIDField.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.viewIDField.PlaceholderText = MenuWorkshopSpawnsUI.localization.format("ID_Field_Hint");
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.viewIDField);
			MenuWorkshopSpawnsUI.viewButton = Glazier.Get().CreateButton();
			MenuWorkshopSpawnsUI.viewButton.PositionOffset_X = 170f;
			MenuWorkshopSpawnsUI.viewButton.PositionOffset_Y = 40f;
			MenuWorkshopSpawnsUI.viewButton.SizeOffset_X = 100f;
			MenuWorkshopSpawnsUI.viewButton.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.viewButton.Text = MenuWorkshopSpawnsUI.localization.format("View_Button");
			MenuWorkshopSpawnsUI.viewButton.TooltipText = MenuWorkshopSpawnsUI.localization.format("View_Button_Tooltip");
			MenuWorkshopSpawnsUI.viewButton.OnClicked += new ClickedButton(MenuWorkshopSpawnsUI.onClickedViewButton);
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.viewButton);
			MenuWorkshopSpawnsUI.rawButton = Glazier.Get().CreateButton();
			MenuWorkshopSpawnsUI.rawButton.PositionOffset_X = 280f;
			MenuWorkshopSpawnsUI.rawButton.PositionOffset_Y = 40f;
			MenuWorkshopSpawnsUI.rawButton.SizeOffset_X = 100f;
			MenuWorkshopSpawnsUI.rawButton.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.rawButton.Text = MenuWorkshopSpawnsUI.localization.format("Raw_Button");
			MenuWorkshopSpawnsUI.rawButton.TooltipText = MenuWorkshopSpawnsUI.localization.format("Raw_Button_Tooltip");
			MenuWorkshopSpawnsUI.rawButton.OnClicked += new ClickedButton(MenuWorkshopSpawnsUI.onClickedRawButton);
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.rawButton);
			MenuWorkshopSpawnsUI.newButton = Glazier.Get().CreateButton();
			MenuWorkshopSpawnsUI.newButton.PositionOffset_X = 390f;
			MenuWorkshopSpawnsUI.newButton.PositionOffset_Y = 40f;
			MenuWorkshopSpawnsUI.newButton.SizeOffset_X = 100f;
			MenuWorkshopSpawnsUI.newButton.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.newButton.Text = MenuWorkshopSpawnsUI.localization.format("New_Button");
			MenuWorkshopSpawnsUI.newButton.TooltipText = MenuWorkshopSpawnsUI.localization.format("New_Button_Tooltip");
			MenuWorkshopSpawnsUI.newButton.OnClicked += new ClickedButton(MenuWorkshopSpawnsUI.onClickedNewButton);
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.newButton);
			MenuWorkshopSpawnsUI.writeButton = Glazier.Get().CreateButton();
			MenuWorkshopSpawnsUI.writeButton.PositionOffset_X = 500f;
			MenuWorkshopSpawnsUI.writeButton.PositionOffset_Y = 40f;
			MenuWorkshopSpawnsUI.writeButton.SizeOffset_X = 100f;
			MenuWorkshopSpawnsUI.writeButton.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.writeButton.Text = MenuWorkshopSpawnsUI.localization.format("Write_Button");
			MenuWorkshopSpawnsUI.writeButton.TooltipText = MenuWorkshopSpawnsUI.localization.format("Write_Button_Tooltip");
			MenuWorkshopSpawnsUI.writeButton.OnClicked += new ClickedButton(MenuWorkshopSpawnsUI.onClickedWriteButton);
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.writeButton);
			MenuWorkshopSpawnsUI.addRootIDField = Glazier.Get().CreateStringField();
			MenuWorkshopSpawnsUI.addRootIDField.SizeOffset_X = 470f;
			MenuWorkshopSpawnsUI.addRootIDField.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.addRootIDField.PlaceholderText = MenuWorkshopSpawnsUI.localization.format("ID_Field_Hint");
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.addRootIDField);
			MenuWorkshopSpawnsUI.addRootSpawnButton = new SleekButtonIcon(MenuWorkshopEditorUI.icons.load<Texture2D>("Add"));
			MenuWorkshopSpawnsUI.addRootSpawnButton.PositionOffset_X = 480f;
			MenuWorkshopSpawnsUI.addRootSpawnButton.SizeOffset_X = 120f;
			MenuWorkshopSpawnsUI.addRootSpawnButton.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.addRootSpawnButton.text = MenuWorkshopSpawnsUI.localization.format("Add_Root_Spawn_Button");
			MenuWorkshopSpawnsUI.addRootSpawnButton.tooltip = MenuWorkshopSpawnsUI.localization.format("Add_Root_Spawn_Button_Tooltip");
			MenuWorkshopSpawnsUI.addRootSpawnButton.onClickedButton += new ClickedButton(MenuWorkshopSpawnsUI.onClickedAddRootSpawnButton);
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.addRootSpawnButton);
			MenuWorkshopSpawnsUI.addTableIDField = Glazier.Get().CreateStringField();
			MenuWorkshopSpawnsUI.addTableIDField.SizeOffset_X = 340f;
			MenuWorkshopSpawnsUI.addTableIDField.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.addTableIDField.PlaceholderText = MenuWorkshopSpawnsUI.localization.format("ID_Field_Hint");
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.addTableIDField);
			MenuWorkshopSpawnsUI.addTableAssetButton = new SleekButtonIcon(MenuWorkshopEditorUI.icons.load<Texture2D>("Add"));
			MenuWorkshopSpawnsUI.addTableAssetButton.PositionOffset_X = 350f;
			MenuWorkshopSpawnsUI.addTableAssetButton.SizeOffset_X = 120f;
			MenuWorkshopSpawnsUI.addTableAssetButton.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.addTableAssetButton.text = MenuWorkshopSpawnsUI.localization.format("Add_Table_Asset_Button");
			MenuWorkshopSpawnsUI.addTableAssetButton.tooltip = MenuWorkshopSpawnsUI.localization.format("Add_Table_Asset_Button_Tooltip");
			MenuWorkshopSpawnsUI.addTableAssetButton.onClickedButton += new ClickedButton(MenuWorkshopSpawnsUI.onClickedAddTableAssetButton);
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.addTableAssetButton);
			MenuWorkshopSpawnsUI.addTableSpawnButton = new SleekButtonIcon(MenuWorkshopEditorUI.icons.load<Texture2D>("Add"));
			MenuWorkshopSpawnsUI.addTableSpawnButton.PositionOffset_X = 480f;
			MenuWorkshopSpawnsUI.addTableSpawnButton.SizeOffset_X = 120f;
			MenuWorkshopSpawnsUI.addTableSpawnButton.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.addTableSpawnButton.text = MenuWorkshopSpawnsUI.localization.format("Add_Table_Spawn_Button");
			MenuWorkshopSpawnsUI.addTableSpawnButton.tooltip = MenuWorkshopSpawnsUI.localization.format("Add_Table_Spawn_Button_Tooltip");
			MenuWorkshopSpawnsUI.addTableSpawnButton.onClickedButton += new ClickedButton(MenuWorkshopSpawnsUI.onClickedAddTableSpawnButton);
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.addTableSpawnButton);
			MenuWorkshopSpawnsUI.applyWeightsButton = Glazier.Get().CreateButton();
			MenuWorkshopSpawnsUI.applyWeightsButton.SizeOffset_X = 600f;
			MenuWorkshopSpawnsUI.applyWeightsButton.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.applyWeightsButton.Text = MenuWorkshopSpawnsUI.localization.format("Apply_Weights_Button");
			MenuWorkshopSpawnsUI.applyWeightsButton.TooltipText = MenuWorkshopSpawnsUI.localization.format("Apply_Weights_Button_Tooltip");
			MenuWorkshopSpawnsUI.applyWeightsButton.OnClicked += new ClickedButton(MenuWorkshopSpawnsUI.onClickedApplyWeightsButton);
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.applyWeightsButton);
			MenuWorkshopSpawnsUI.rootsBox = Glazier.Get().CreateBox();
			MenuWorkshopSpawnsUI.rootsBox.PositionOffset_Y = 40f;
			MenuWorkshopSpawnsUI.rootsBox.SizeOffset_X = 600f;
			MenuWorkshopSpawnsUI.rootsBox.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.rootsBox);
			MenuWorkshopSpawnsUI.tablesBox = Glazier.Get().CreateBox();
			MenuWorkshopSpawnsUI.tablesBox.PositionOffset_Y = 80f;
			MenuWorkshopSpawnsUI.tablesBox.SizeOffset_X = 600f;
			MenuWorkshopSpawnsUI.tablesBox.SizeOffset_Y = 30f;
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.tablesBox);
			MenuWorkshopSpawnsUI.rawField = Glazier.Get().CreateStringField();
			MenuWorkshopSpawnsUI.rawField.PositionOffset_Y = 80f;
			MenuWorkshopSpawnsUI.rawField.SizeOffset_X = 600f;
			MenuWorkshopSpawnsUI.rawField.SizeOffset_Y = 1000f;
			MenuWorkshopSpawnsUI.rawField.IsMultiline = true;
			MenuWorkshopSpawnsUI.rawField.MaxLength = 4096;
			MenuWorkshopSpawnsUI.rawField.TextAlignment = 0;
			MenuWorkshopSpawnsUI.spawnsBox.AddChild(MenuWorkshopSpawnsUI.rawField);
			MenuWorkshopSpawnsUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuWorkshopSpawnsUI.backButton.PositionOffset_Y = -50f;
			MenuWorkshopSpawnsUI.backButton.PositionScale_Y = 1f;
			MenuWorkshopSpawnsUI.backButton.SizeOffset_X = 200f;
			MenuWorkshopSpawnsUI.backButton.SizeOffset_Y = 50f;
			MenuWorkshopSpawnsUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuWorkshopSpawnsUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuWorkshopSpawnsUI.backButton.onClickedButton += new ClickedButton(MenuWorkshopSpawnsUI.onClickedBackButton);
			MenuWorkshopSpawnsUI.backButton.fontSize = 3;
			MenuWorkshopSpawnsUI.backButton.iconColor = 2;
			MenuWorkshopSpawnsUI.container.AddChild(MenuWorkshopSpawnsUI.backButton);
		}

		// Token: 0x04002B77 RID: 11127
		private static Local localization;

		// Token: 0x04002B78 RID: 11128
		private static SleekFullscreenBox container;

		// Token: 0x04002B79 RID: 11129
		public static bool active;

		// Token: 0x04002B7A RID: 11130
		private static SleekButtonIcon backButton;

		// Token: 0x04002B7B RID: 11131
		private static ISleekScrollView spawnsBox;

		// Token: 0x04002B7C RID: 11132
		private static SleekButtonState typeButton;

		// Token: 0x04002B7D RID: 11133
		private static ISleekField viewIDField;

		// Token: 0x04002B7E RID: 11134
		private static ISleekButton viewButton;

		// Token: 0x04002B7F RID: 11135
		private static ISleekButton rawButton;

		// Token: 0x04002B80 RID: 11136
		private static ISleekButton newButton;

		// Token: 0x04002B81 RID: 11137
		private static ISleekButton writeButton;

		// Token: 0x04002B82 RID: 11138
		private static ISleekBox rootsBox;

		// Token: 0x04002B83 RID: 11139
		private static ISleekBox tablesBox;

		// Token: 0x04002B84 RID: 11140
		private static ISleekField rawField;

		// Token: 0x04002B85 RID: 11141
		private static ISleekField addRootIDField;

		// Token: 0x04002B86 RID: 11142
		private static SleekButtonIcon addRootSpawnButton;

		// Token: 0x04002B87 RID: 11143
		private static ISleekField addTableIDField;

		// Token: 0x04002B88 RID: 11144
		private static SleekButtonIcon addTableAssetButton;

		// Token: 0x04002B89 RID: 11145
		private static SleekButtonIcon addTableSpawnButton;

		// Token: 0x04002B8A RID: 11146
		private static ISleekButton applyWeightsButton;

		// Token: 0x04002B8B RID: 11147
		private static SpawnAsset asset;

		// Token: 0x04002B8C RID: 11148
		private static EAssetType type;
	}
}
