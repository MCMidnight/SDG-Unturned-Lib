using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000778 RID: 1912
	public class EditorLevelObjectsUI : SleekFullscreenBox
	{
		// Token: 0x06003E90 RID: 16016 RVA: 0x00132ACE File Offset: 0x00130CCE
		public static void open()
		{
			if (EditorLevelObjectsUI.active)
			{
				return;
			}
			EditorLevelObjectsUI.active = true;
			EditorObjects.isBuilding = true;
			EditorUI.message(EEditorMessage.OBJECTS);
			EditorLevelObjectsUI.container.AnimateIntoView();
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x00132AF4 File Offset: 0x00130CF4
		public static void close()
		{
			if (!EditorLevelObjectsUI.active)
			{
				return;
			}
			EditorLevelObjectsUI.active = false;
			EditorObjects.isBuilding = false;
			EditorLevelObjectsUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x00132B20 File Offset: 0x00130D20
		public override void OnUpdate()
		{
			base.OnUpdate();
			GameObject mostRecentSelectedGameObject = EditorObjects.GetMostRecentSelectedGameObject();
			if (this.focusedGameObject == mostRecentSelectedGameObject)
			{
				return;
			}
			this.focusedGameObject = mostRecentSelectedGameObject;
			EditorLevelObjectsUI.focusedLevelObject = LevelObjects.FindLevelObject(this.focusedGameObject);
			if (EditorLevelObjectsUI.focusedLevelObject != null)
			{
				EditorLevelObjectsUI.materialPaletteOverrideField.IsVisible = true;
				EditorLevelObjectsUI.materialIndexOverrideField.IsVisible = true;
				EditorLevelObjectsUI.materialPaletteOverrideField.Text = EditorLevelObjectsUI.focusedLevelObject.customMaterialOverride.ToString();
				EditorLevelObjectsUI.materialIndexOverrideField.Value = EditorLevelObjectsUI.focusedLevelObject.materialIndexOverride;
				return;
			}
			EditorLevelObjectsUI.isOwnedCullingVolumeAllowedToggle.IsVisible = false;
			EditorLevelObjectsUI.materialPaletteOverrideField.IsVisible = false;
			EditorLevelObjectsUI.materialIndexOverrideField.IsVisible = false;
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x00132BD4 File Offset: 0x00130DD4
		private static void updateSelection(string search, bool large, bool medium, bool small, bool barricades, bool structures, bool npcs)
		{
			if (EditorLevelObjectsUI.assets == null)
			{
				return;
			}
			EditorLevelObjectsUI.assets.Clear();
			EditorObjectSearchFilter editorObjectSearchFilter = EditorObjectSearchFilter.parse(search);
			if (large || medium || small || npcs)
			{
				EditorLevelObjectsUI.tempObjectAssets.Clear();
				Assets.find<ObjectAsset>(EditorLevelObjectsUI.tempObjectAssets);
				foreach (ObjectAsset objectAsset in EditorLevelObjectsUI.tempObjectAssets)
				{
					if ((large || objectAsset.type != EObjectType.LARGE) && (medium || objectAsset.type != EObjectType.MEDIUM) && (small || (objectAsset.type != EObjectType.SMALL && objectAsset.type != EObjectType.DECAL)) && (npcs || objectAsset.type != EObjectType.NPC) && (editorObjectSearchFilter == null || !editorObjectSearchFilter.ignores(objectAsset)))
					{
						EditorLevelObjectsUI.assets.Add(objectAsset);
					}
				}
			}
			if (barricades || structures)
			{
				List<ItemAsset> list = new List<ItemAsset>();
				Assets.find<ItemAsset>(list);
				foreach (ItemAsset itemAsset in list)
				{
					if (itemAsset is ItemBarricadeAsset)
					{
						if (!barricades)
						{
							continue;
						}
					}
					else if (!(itemAsset is ItemStructureAsset) || !structures)
					{
						continue;
					}
					if (editorObjectSearchFilter == null || !editorObjectSearchFilter.ignores(itemAsset))
					{
						EditorLevelObjectsUI.assets.Add(itemAsset);
					}
				}
			}
			EditorLevelObjectsUI.assets.Sort(EditorLevelObjectsUI.comparator);
			EditorLevelObjectsUI.assetsScrollBox.NotifyDataChanged();
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x00132D40 File Offset: 0x00130F40
		private static void onAssetsRefreshed()
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.Text, EditorLevelObjectsUI.largeToggle.Value, EditorLevelObjectsUI.mediumToggle.Value, EditorLevelObjectsUI.smallToggle.Value, EditorLevelObjectsUI.barricadesToggle.Value, EditorLevelObjectsUI.structuresToggle.Value, EditorLevelObjectsUI.npcsToggle.Value);
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x00132D98 File Offset: 0x00130F98
		private static ISleekElement onCreateAssetButton(Asset item)
		{
			string text = string.Empty;
			ObjectAsset objectAsset = item as ObjectAsset;
			ItemAsset itemAsset = item as ItemAsset;
			if (objectAsset != null)
			{
				text = objectAsset.objectName;
			}
			else if (itemAsset != null)
			{
				text = itemAsset.itemName;
			}
			ISleekButton sleekButton = Glazier.Get().CreateButton();
			sleekButton.Text = text;
			sleekButton.OnClicked += new ClickedButton(EditorLevelObjectsUI.onClickedAssetButton);
			return sleekButton;
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x00132DF4 File Offset: 0x00130FF4
		private static void onClickedAssetButton(ISleekElement button)
		{
			int num = Mathf.FloorToInt(button.PositionOffset_Y / 40f);
			EditorObjects.selectedObjectAsset = (EditorLevelObjectsUI.assets[num] as ObjectAsset);
			EditorObjects.selectedItemAsset = (EditorLevelObjectsUI.assets[num] as ItemAsset);
			if (EditorObjects.selectedObjectAsset != null)
			{
				EditorLevelObjectsUI.selectedBox.Text = EditorObjects.selectedObjectAsset.objectName;
				return;
			}
			if (EditorObjects.selectedItemAsset != null)
			{
				EditorLevelObjectsUI.selectedBox.Text = EditorObjects.selectedItemAsset.itemName;
			}
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x00132E74 File Offset: 0x00131074
		private static void onDragStarted(Vector2 minViewportPoint, Vector2 maxViewportPoint)
		{
			Vector2 vector = EditorUI.window.ViewportToNormalizedPosition(minViewportPoint);
			Vector2 vector2 = EditorUI.window.ViewportToNormalizedPosition(maxViewportPoint);
			if (vector2.y < vector.y)
			{
				float y = vector2.y;
				vector2.y = vector.y;
				vector.y = y;
			}
			EditorLevelObjectsUI.dragBox.PositionScale_X = vector.x;
			EditorLevelObjectsUI.dragBox.PositionScale_Y = vector.y;
			EditorLevelObjectsUI.dragBox.SizeScale_X = vector2.x - vector.x;
			EditorLevelObjectsUI.dragBox.SizeScale_Y = vector2.y - vector.y;
			EditorLevelObjectsUI.dragBox.IsVisible = true;
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x00132F1C File Offset: 0x0013111C
		private static void onDragStopped()
		{
			EditorLevelObjectsUI.dragBox.IsVisible = false;
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x00132F2C File Offset: 0x0013112C
		private static void onEnteredSearchField(ISleekField field)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.Text, EditorLevelObjectsUI.largeToggle.Value, EditorLevelObjectsUI.mediumToggle.Value, EditorLevelObjectsUI.smallToggle.Value, EditorLevelObjectsUI.barricadesToggle.Value, EditorLevelObjectsUI.structuresToggle.Value, EditorLevelObjectsUI.npcsToggle.Value);
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x00132F84 File Offset: 0x00131184
		private static void onClickedSearchButton(ISleekElement button)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.Text, EditorLevelObjectsUI.largeToggle.Value, EditorLevelObjectsUI.mediumToggle.Value, EditorLevelObjectsUI.smallToggle.Value, EditorLevelObjectsUI.barricadesToggle.Value, EditorLevelObjectsUI.structuresToggle.Value, EditorLevelObjectsUI.npcsToggle.Value);
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x00132FDC File Offset: 0x001311DC
		private static void onToggledLargeToggle(ISleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.Text, state, EditorLevelObjectsUI.mediumToggle.Value, EditorLevelObjectsUI.smallToggle.Value, EditorLevelObjectsUI.barricadesToggle.Value, EditorLevelObjectsUI.structuresToggle.Value, EditorLevelObjectsUI.npcsToggle.Value);
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x0013302C File Offset: 0x0013122C
		private static void onToggledMediumToggle(ISleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.Text, EditorLevelObjectsUI.largeToggle.Value, state, EditorLevelObjectsUI.smallToggle.Value, EditorLevelObjectsUI.barricadesToggle.Value, EditorLevelObjectsUI.structuresToggle.Value, EditorLevelObjectsUI.npcsToggle.Value);
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x0013307C File Offset: 0x0013127C
		private static void onToggledSmallToggle(ISleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.Text, EditorLevelObjectsUI.largeToggle.Value, EditorLevelObjectsUI.mediumToggle.Value, state, EditorLevelObjectsUI.barricadesToggle.Value, EditorLevelObjectsUI.structuresToggle.Value, EditorLevelObjectsUI.npcsToggle.Value);
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x001330CC File Offset: 0x001312CC
		private static void onToggledBarricadesToggle(ISleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.Text, EditorLevelObjectsUI.largeToggle.Value, EditorLevelObjectsUI.mediumToggle.Value, EditorLevelObjectsUI.smallToggle.Value, state, EditorLevelObjectsUI.structuresToggle.Value, EditorLevelObjectsUI.npcsToggle.Value);
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x0013311C File Offset: 0x0013131C
		private static void onToggledStructuresToggle(ISleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.Text, EditorLevelObjectsUI.largeToggle.Value, EditorLevelObjectsUI.mediumToggle.Value, EditorLevelObjectsUI.smallToggle.Value, EditorLevelObjectsUI.barricadesToggle.Value, state, EditorLevelObjectsUI.npcsToggle.Value);
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x0013316C File Offset: 0x0013136C
		private static void onToggledNPCsToggle(ISleekToggle toggle, bool state)
		{
			EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.Text, EditorLevelObjectsUI.largeToggle.Value, EditorLevelObjectsUI.mediumToggle.Value, EditorLevelObjectsUI.smallToggle.Value, EditorLevelObjectsUI.barricadesToggle.Value, EditorLevelObjectsUI.structuresToggle.Value, state);
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x001331BC File Offset: 0x001313BC
		private static void OnIsOwnedCullingVolumeAllowedChanged(ISleekToggle toggle, bool value)
		{
			foreach (GameObject rootGameObject in EditorObjects.EnumerateSelectedGameObjects())
			{
				LevelObject levelObject = LevelObjects.FindLevelObject(rootGameObject);
				if (levelObject != null)
				{
					levelObject.isOwnedCullingVolumeAllowed = value;
					levelObject.ReapplyOwnedCullingVolumeAllowed();
				}
			}
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x00133218 File Offset: 0x00131418
		private static void OnTypedMaterialPaletteOverride(ISleekField field, string value)
		{
			AssetReference<MaterialPaletteAsset> customMaterialOverride = new AssetReference<MaterialPaletteAsset>(value);
			foreach (GameObject rootGameObject in EditorObjects.EnumerateSelectedGameObjects())
			{
				LevelObject levelObject = LevelObjects.FindLevelObject(rootGameObject);
				if (levelObject != null)
				{
					levelObject.customMaterialOverride = customMaterialOverride;
					levelObject.ReapplyMaterialOverrides();
				}
			}
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x0013327C File Offset: 0x0013147C
		private static void OnTypedMaterialIndexOverride(ISleekInt32Field field, int value)
		{
			foreach (GameObject rootGameObject in EditorObjects.EnumerateSelectedGameObjects())
			{
				LevelObject levelObject = LevelObjects.FindLevelObject(rootGameObject);
				if (levelObject != null)
				{
					levelObject.materialIndexOverride = value;
					levelObject.ReapplyMaterialOverrides();
				}
			}
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x001332D8 File Offset: 0x001314D8
		private static void onTypedSnapTransformField(ISleekFloat32Field field, float value)
		{
			EditorObjects.snapTransform = value;
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x001332E0 File Offset: 0x001314E0
		private static void onTypedSnapRotationField(ISleekFloat32Field field, float value)
		{
			EditorObjects.snapRotation = value;
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x001332E8 File Offset: 0x001314E8
		private static void onClickedTransformButton(ISleekElement button)
		{
			EditorObjects.dragMode = EDragMode.TRANSFORM;
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x001332F0 File Offset: 0x001314F0
		private static void onClickedRotateButton(ISleekElement button)
		{
			EditorObjects.dragMode = EDragMode.ROTATE;
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x001332F8 File Offset: 0x001314F8
		private static void onClickedScaleButton(ISleekElement button)
		{
			EditorObjects.dragMode = EDragMode.SCALE;
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x00133300 File Offset: 0x00131500
		private static void onSwappedStateCoordinate(SleekButtonState button, int index)
		{
			EditorObjects.dragCoordinate = (EDragCoordinate)index;
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x00133308 File Offset: 0x00131508
		public override void OnDestroy()
		{
			Assets.onAssetsRefreshed = (AssetsRefreshed)Delegate.Remove(Assets.onAssetsRefreshed, new AssetsRefreshed(EditorLevelObjectsUI.onAssetsRefreshed));
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x0013332C File Offset: 0x0013152C
		public EditorLevelObjectsUI()
		{
			Local local = Localization.read("/Editor/EditorLevelObjects.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevelObjects/EditorLevelObjects.unity3d");
			EditorLevelObjectsUI.container = this;
			EditorLevelObjectsUI.active = false;
			EditorLevelObjectsUI.assets = new List<Asset>();
			EditorLevelObjectsUI.selectedBox = Glazier.Get().CreateBox();
			EditorLevelObjectsUI.selectedBox.PositionOffset_X = -230f;
			EditorLevelObjectsUI.selectedBox.PositionScale_X = 1f;
			EditorLevelObjectsUI.selectedBox.SizeOffset_X = 230f;
			EditorLevelObjectsUI.selectedBox.SizeOffset_Y = 30f;
			EditorLevelObjectsUI.selectedBox.AddLabel(local.format("SelectionBoxLabelText"), 0);
			base.AddChild(EditorLevelObjectsUI.selectedBox);
			EditorLevelObjectsUI.searchField = Glazier.Get().CreateStringField();
			EditorLevelObjectsUI.searchField.PositionOffset_X = -230f;
			EditorLevelObjectsUI.searchField.PositionOffset_Y = 40f;
			EditorLevelObjectsUI.searchField.PositionScale_X = 1f;
			EditorLevelObjectsUI.searchField.SizeOffset_X = 160f;
			EditorLevelObjectsUI.searchField.SizeOffset_Y = 30f;
			EditorLevelObjectsUI.searchField.PlaceholderText = local.format("Search_Field_Hint");
			EditorLevelObjectsUI.searchField.OnTextSubmitted += new Entered(EditorLevelObjectsUI.onEnteredSearchField);
			base.AddChild(EditorLevelObjectsUI.searchField);
			EditorLevelObjectsUI.searchButton = Glazier.Get().CreateButton();
			EditorLevelObjectsUI.searchButton.PositionOffset_X = -60f;
			EditorLevelObjectsUI.searchButton.PositionOffset_Y = 40f;
			EditorLevelObjectsUI.searchButton.PositionScale_X = 1f;
			EditorLevelObjectsUI.searchButton.SizeOffset_X = 60f;
			EditorLevelObjectsUI.searchButton.SizeOffset_Y = 30f;
			EditorLevelObjectsUI.searchButton.Text = local.format("Search");
			EditorLevelObjectsUI.searchButton.TooltipText = local.format("Search_Tooltip");
			EditorLevelObjectsUI.searchButton.OnClicked += new ClickedButton(EditorLevelObjectsUI.onClickedSearchButton);
			base.AddChild(EditorLevelObjectsUI.searchButton);
			EditorLevelObjectsUI.largeToggle = Glazier.Get().CreateToggle();
			EditorLevelObjectsUI.largeToggle.PositionOffset_X = -230f;
			EditorLevelObjectsUI.largeToggle.PositionOffset_Y = 80f;
			EditorLevelObjectsUI.largeToggle.PositionScale_X = 1f;
			EditorLevelObjectsUI.largeToggle.SizeOffset_X = 40f;
			EditorLevelObjectsUI.largeToggle.SizeOffset_Y = 40f;
			EditorLevelObjectsUI.largeToggle.AddLabel(local.format("LargeLabel"), 1);
			EditorLevelObjectsUI.largeToggle.Value = true;
			EditorLevelObjectsUI.largeToggle.OnValueChanged += new Toggled(EditorLevelObjectsUI.onToggledLargeToggle);
			base.AddChild(EditorLevelObjectsUI.largeToggle);
			EditorLevelObjectsUI.mediumToggle = Glazier.Get().CreateToggle();
			EditorLevelObjectsUI.mediumToggle.PositionOffset_X = -230f;
			EditorLevelObjectsUI.mediumToggle.PositionOffset_Y = 130f;
			EditorLevelObjectsUI.mediumToggle.PositionScale_X = 1f;
			EditorLevelObjectsUI.mediumToggle.SizeOffset_X = 40f;
			EditorLevelObjectsUI.mediumToggle.SizeOffset_Y = 40f;
			EditorLevelObjectsUI.mediumToggle.AddLabel(local.format("MediumLabel"), 1);
			EditorLevelObjectsUI.mediumToggle.Value = true;
			EditorLevelObjectsUI.mediumToggle.OnValueChanged += new Toggled(EditorLevelObjectsUI.onToggledMediumToggle);
			base.AddChild(EditorLevelObjectsUI.mediumToggle);
			EditorLevelObjectsUI.smallToggle = Glazier.Get().CreateToggle();
			EditorLevelObjectsUI.smallToggle.PositionOffset_X = -230f;
			EditorLevelObjectsUI.smallToggle.PositionOffset_Y = 180f;
			EditorLevelObjectsUI.smallToggle.PositionScale_X = 1f;
			EditorLevelObjectsUI.smallToggle.SizeOffset_X = 40f;
			EditorLevelObjectsUI.smallToggle.SizeOffset_Y = 40f;
			EditorLevelObjectsUI.smallToggle.AddLabel(local.format("SmallLabel"), 1);
			EditorLevelObjectsUI.smallToggle.Value = true;
			EditorLevelObjectsUI.smallToggle.OnValueChanged += new Toggled(EditorLevelObjectsUI.onToggledSmallToggle);
			base.AddChild(EditorLevelObjectsUI.smallToggle);
			EditorLevelObjectsUI.barricadesToggle = Glazier.Get().CreateToggle();
			EditorLevelObjectsUI.barricadesToggle.PositionOffset_X = -130f;
			EditorLevelObjectsUI.barricadesToggle.PositionOffset_Y = 80f;
			EditorLevelObjectsUI.barricadesToggle.PositionScale_X = 1f;
			EditorLevelObjectsUI.barricadesToggle.SizeOffset_X = 40f;
			EditorLevelObjectsUI.barricadesToggle.SizeOffset_Y = 40f;
			EditorLevelObjectsUI.barricadesToggle.AddLabel(local.format("BarricadesLabel"), 1);
			EditorLevelObjectsUI.barricadesToggle.Value = false;
			EditorLevelObjectsUI.barricadesToggle.OnValueChanged += new Toggled(EditorLevelObjectsUI.onToggledBarricadesToggle);
			base.AddChild(EditorLevelObjectsUI.barricadesToggle);
			EditorLevelObjectsUI.structuresToggle = Glazier.Get().CreateToggle();
			EditorLevelObjectsUI.structuresToggle.PositionOffset_X = -130f;
			EditorLevelObjectsUI.structuresToggle.PositionOffset_Y = 130f;
			EditorLevelObjectsUI.structuresToggle.PositionScale_X = 1f;
			EditorLevelObjectsUI.structuresToggle.SizeOffset_X = 40f;
			EditorLevelObjectsUI.structuresToggle.SizeOffset_Y = 40f;
			EditorLevelObjectsUI.structuresToggle.AddLabel(local.format("StructuresLabel"), 1);
			EditorLevelObjectsUI.structuresToggle.Value = false;
			EditorLevelObjectsUI.structuresToggle.OnValueChanged += new Toggled(EditorLevelObjectsUI.onToggledStructuresToggle);
			base.AddChild(EditorLevelObjectsUI.structuresToggle);
			EditorLevelObjectsUI.npcsToggle = Glazier.Get().CreateToggle();
			EditorLevelObjectsUI.npcsToggle.PositionOffset_X = -130f;
			EditorLevelObjectsUI.npcsToggle.PositionOffset_Y = 180f;
			EditorLevelObjectsUI.npcsToggle.PositionScale_X = 1f;
			EditorLevelObjectsUI.npcsToggle.SizeOffset_X = 40f;
			EditorLevelObjectsUI.npcsToggle.SizeOffset_Y = 40f;
			EditorLevelObjectsUI.npcsToggle.AddLabel(local.format("NPCsLabel"), 1);
			EditorLevelObjectsUI.npcsToggle.Value = false;
			EditorLevelObjectsUI.npcsToggle.OnValueChanged += new Toggled(EditorLevelObjectsUI.onToggledNPCsToggle);
			base.AddChild(EditorLevelObjectsUI.npcsToggle);
			EditorLevelObjectsUI.assetsScrollBox = new SleekList<Asset>();
			EditorLevelObjectsUI.assetsScrollBox.PositionOffset_X = -230f;
			EditorLevelObjectsUI.assetsScrollBox.PositionOffset_Y = 230f;
			EditorLevelObjectsUI.assetsScrollBox.PositionScale_X = 1f;
			EditorLevelObjectsUI.assetsScrollBox.SizeOffset_X = 230f;
			EditorLevelObjectsUI.assetsScrollBox.SizeOffset_Y = -230f;
			EditorLevelObjectsUI.assetsScrollBox.SizeScale_Y = 1f;
			EditorLevelObjectsUI.assetsScrollBox.itemHeight = 30;
			EditorLevelObjectsUI.assetsScrollBox.itemPadding = 10;
			EditorLevelObjectsUI.assetsScrollBox.onCreateElement = new SleekList<Asset>.CreateElement(EditorLevelObjectsUI.onCreateAssetButton);
			EditorLevelObjectsUI.assetsScrollBox.SetData(EditorLevelObjectsUI.assets);
			base.AddChild(EditorLevelObjectsUI.assetsScrollBox);
			EditorObjects.selectedObjectAsset = null;
			EditorObjects.selectedItemAsset = null;
			EditorObjects.onDragStarted = new DragStarted(EditorLevelObjectsUI.onDragStarted);
			EditorObjects.onDragStopped = new DragStopped(EditorLevelObjectsUI.onDragStopped);
			EditorLevelObjectsUI.dragBox = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
			EditorLevelObjectsUI.dragBox.TintColor = new Color(1f, 1f, 0f, 0.2f);
			EditorUI.window.AddChild(EditorLevelObjectsUI.dragBox);
			EditorLevelObjectsUI.dragBox.IsVisible = false;
			EditorLevelObjectsUI.isOwnedCullingVolumeAllowedToggle = Glazier.Get().CreateToggle();
			EditorLevelObjectsUI.isOwnedCullingVolumeAllowedToggle.PositionOffset_Y = -350f;
			EditorLevelObjectsUI.isOwnedCullingVolumeAllowedToggle.PositionScale_Y = 1f;
			EditorLevelObjectsUI.isOwnedCullingVolumeAllowedToggle.AddLabel(local.format("IsOwnedCullingVolumeAllowed_Label"), 1);
			EditorLevelObjectsUI.isOwnedCullingVolumeAllowedToggle.TooltipText = local.format("IsOwnedCullingVolumeAllowed_Tooltip");
			EditorLevelObjectsUI.isOwnedCullingVolumeAllowedToggle.OnValueChanged += new Toggled(EditorLevelObjectsUI.OnIsOwnedCullingVolumeAllowedChanged);
			EditorLevelObjectsUI.isOwnedCullingVolumeAllowedToggle.IsVisible = false;
			base.AddChild(EditorLevelObjectsUI.isOwnedCullingVolumeAllowedToggle);
			EditorLevelObjectsUI.materialPaletteOverrideField = Glazier.Get().CreateStringField();
			EditorLevelObjectsUI.materialPaletteOverrideField.PositionOffset_Y = -310f;
			EditorLevelObjectsUI.materialPaletteOverrideField.PositionScale_Y = 1f;
			EditorLevelObjectsUI.materialPaletteOverrideField.SizeOffset_X = 200f;
			EditorLevelObjectsUI.materialPaletteOverrideField.SizeOffset_Y = 30f;
			EditorLevelObjectsUI.materialPaletteOverrideField.AddLabel(local.format("MaterialPaletteOverride_Label"), 1);
			EditorLevelObjectsUI.materialPaletteOverrideField.TooltipText = local.format("MaterialPaletteOverride_Tooltip");
			EditorLevelObjectsUI.materialPaletteOverrideField.OnTextChanged += new Typed(EditorLevelObjectsUI.OnTypedMaterialPaletteOverride);
			EditorLevelObjectsUI.materialPaletteOverrideField.IsVisible = false;
			base.AddChild(EditorLevelObjectsUI.materialPaletteOverrideField);
			EditorLevelObjectsUI.materialIndexOverrideField = Glazier.Get().CreateInt32Field();
			EditorLevelObjectsUI.materialIndexOverrideField.PositionOffset_Y = -270f;
			EditorLevelObjectsUI.materialIndexOverrideField.PositionScale_Y = 1f;
			EditorLevelObjectsUI.materialIndexOverrideField.SizeOffset_X = 200f;
			EditorLevelObjectsUI.materialIndexOverrideField.SizeOffset_Y = 30f;
			EditorLevelObjectsUI.materialIndexOverrideField.AddLabel(local.format("MaterialIndexOverride_Label"), 1);
			EditorLevelObjectsUI.materialIndexOverrideField.TooltipText = local.format("MaterialIndexOverride_Tooltip");
			EditorLevelObjectsUI.materialIndexOverrideField.OnValueChanged += new TypedInt32(EditorLevelObjectsUI.OnTypedMaterialIndexOverride);
			EditorLevelObjectsUI.materialIndexOverrideField.IsVisible = false;
			base.AddChild(EditorLevelObjectsUI.materialIndexOverrideField);
			EditorLevelObjectsUI.snapTransformField = Glazier.Get().CreateFloat32Field();
			EditorLevelObjectsUI.snapTransformField.PositionOffset_Y = -230f;
			EditorLevelObjectsUI.snapTransformField.PositionScale_Y = 1f;
			EditorLevelObjectsUI.snapTransformField.SizeOffset_X = 200f;
			EditorLevelObjectsUI.snapTransformField.SizeOffset_Y = 30f;
			EditorLevelObjectsUI.snapTransformField.Value = EditorObjects.snapTransform;
			EditorLevelObjectsUI.snapTransformField.AddLabel(local.format("SnapTransformLabelText"), 1);
			EditorLevelObjectsUI.snapTransformField.OnValueChanged += new TypedSingle(EditorLevelObjectsUI.onTypedSnapTransformField);
			base.AddChild(EditorLevelObjectsUI.snapTransformField);
			EditorLevelObjectsUI.snapRotationField = Glazier.Get().CreateFloat32Field();
			EditorLevelObjectsUI.snapRotationField.PositionOffset_Y = -190f;
			EditorLevelObjectsUI.snapRotationField.PositionScale_Y = 1f;
			EditorLevelObjectsUI.snapRotationField.SizeOffset_X = 200f;
			EditorLevelObjectsUI.snapRotationField.SizeOffset_Y = 30f;
			EditorLevelObjectsUI.snapRotationField.Value = EditorObjects.snapRotation;
			EditorLevelObjectsUI.snapRotationField.AddLabel(local.format("SnapRotationLabelText"), 1);
			EditorLevelObjectsUI.snapRotationField.OnValueChanged += new TypedSingle(EditorLevelObjectsUI.onTypedSnapRotationField);
			base.AddChild(EditorLevelObjectsUI.snapRotationField);
			EditorLevelObjectsUI.transformButton = new SleekButtonIcon(bundle.load<Texture2D>("Transform"));
			EditorLevelObjectsUI.transformButton.PositionOffset_Y = -150f;
			EditorLevelObjectsUI.transformButton.PositionScale_Y = 1f;
			EditorLevelObjectsUI.transformButton.SizeOffset_X = 200f;
			EditorLevelObjectsUI.transformButton.SizeOffset_Y = 30f;
			EditorLevelObjectsUI.transformButton.text = local.format("TransformButtonText", ControlsSettings.tool_0);
			EditorLevelObjectsUI.transformButton.tooltip = local.format("TransformButtonTooltip");
			EditorLevelObjectsUI.transformButton.onClickedButton += new ClickedButton(EditorLevelObjectsUI.onClickedTransformButton);
			base.AddChild(EditorLevelObjectsUI.transformButton);
			EditorLevelObjectsUI.rotateButton = new SleekButtonIcon(bundle.load<Texture2D>("Rotate"));
			EditorLevelObjectsUI.rotateButton.PositionOffset_Y = -110f;
			EditorLevelObjectsUI.rotateButton.PositionScale_Y = 1f;
			EditorLevelObjectsUI.rotateButton.SizeOffset_X = 200f;
			EditorLevelObjectsUI.rotateButton.SizeOffset_Y = 30f;
			EditorLevelObjectsUI.rotateButton.text = local.format("RotateButtonText", ControlsSettings.tool_1);
			EditorLevelObjectsUI.rotateButton.tooltip = local.format("RotateButtonTooltip");
			EditorLevelObjectsUI.rotateButton.onClickedButton += new ClickedButton(EditorLevelObjectsUI.onClickedRotateButton);
			base.AddChild(EditorLevelObjectsUI.rotateButton);
			EditorLevelObjectsUI.scaleButton = new SleekButtonIcon(bundle.load<Texture2D>("Scale"));
			EditorLevelObjectsUI.scaleButton.PositionOffset_Y = -70f;
			EditorLevelObjectsUI.scaleButton.PositionScale_Y = 1f;
			EditorLevelObjectsUI.scaleButton.SizeOffset_X = 200f;
			EditorLevelObjectsUI.scaleButton.SizeOffset_Y = 30f;
			EditorLevelObjectsUI.scaleButton.text = local.format("ScaleButtonText", ControlsSettings.tool_3);
			EditorLevelObjectsUI.scaleButton.tooltip = local.format("ScaleButtonTooltip");
			EditorLevelObjectsUI.scaleButton.onClickedButton += new ClickedButton(EditorLevelObjectsUI.onClickedScaleButton);
			base.AddChild(EditorLevelObjectsUI.scaleButton);
			EditorLevelObjectsUI.coordinateButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(local.format("CoordinateButtonTextGlobal"), bundle.load<Texture>("Global")),
				new GUIContent(local.format("CoordinateButtonTextLocal"), bundle.load<Texture>("Local"))
			});
			EditorLevelObjectsUI.coordinateButton.PositionOffset_Y = -30f;
			EditorLevelObjectsUI.coordinateButton.PositionScale_Y = 1f;
			EditorLevelObjectsUI.coordinateButton.SizeOffset_X = 200f;
			EditorLevelObjectsUI.coordinateButton.SizeOffset_Y = 30f;
			EditorLevelObjectsUI.coordinateButton.tooltip = local.format("CoordinateButtonTooltip");
			EditorLevelObjectsUI.coordinateButton.onSwappedState = new SwappedState(EditorLevelObjectsUI.onSwappedStateCoordinate);
			base.AddChild(EditorLevelObjectsUI.coordinateButton);
			bundle.unload();
			EditorLevelObjectsUI.onAssetsRefreshed();
			Assets.onAssetsRefreshed = (AssetsRefreshed)Delegate.Combine(Assets.onAssetsRefreshed, new AssetsRefreshed(EditorLevelObjectsUI.onAssetsRefreshed));
		}

		// Token: 0x04002766 RID: 10086
		private static SleekFullscreenBox container;

		// Token: 0x04002767 RID: 10087
		public static bool active;

		// Token: 0x04002768 RID: 10088
		private static List<ObjectAsset> tempObjectAssets = new List<ObjectAsset>();

		// Token: 0x04002769 RID: 10089
		private static List<Asset> assets;

		// Token: 0x0400276A RID: 10090
		private static AssetNameAscendingComparator comparator = new AssetNameAscendingComparator();

		// Token: 0x0400276B RID: 10091
		private static SleekList<Asset> assetsScrollBox;

		// Token: 0x0400276C RID: 10092
		private static ISleekBox selectedBox;

		// Token: 0x0400276D RID: 10093
		private static ISleekField searchField;

		// Token: 0x0400276E RID: 10094
		private static ISleekButton searchButton;

		// Token: 0x0400276F RID: 10095
		private static ISleekToggle largeToggle;

		// Token: 0x04002770 RID: 10096
		private static ISleekToggle mediumToggle;

		// Token: 0x04002771 RID: 10097
		private static ISleekToggle smallToggle;

		// Token: 0x04002772 RID: 10098
		private static ISleekToggle barricadesToggle;

		// Token: 0x04002773 RID: 10099
		private static ISleekToggle structuresToggle;

		// Token: 0x04002774 RID: 10100
		private static ISleekToggle npcsToggle;

		// Token: 0x04002775 RID: 10101
		private static ISleekImage dragBox;

		// Token: 0x04002776 RID: 10102
		private static ISleekToggle isOwnedCullingVolumeAllowedToggle;

		// Token: 0x04002777 RID: 10103
		private static ISleekField materialPaletteOverrideField;

		// Token: 0x04002778 RID: 10104
		private static ISleekInt32Field materialIndexOverrideField;

		// Token: 0x04002779 RID: 10105
		private static ISleekFloat32Field snapTransformField;

		// Token: 0x0400277A RID: 10106
		private static ISleekFloat32Field snapRotationField;

		// Token: 0x0400277B RID: 10107
		private static SleekButtonIcon transformButton;

		// Token: 0x0400277C RID: 10108
		private static SleekButtonIcon rotateButton;

		// Token: 0x0400277D RID: 10109
		private static SleekButtonIcon scaleButton;

		// Token: 0x0400277E RID: 10110
		public static SleekButtonState coordinateButton;

		// Token: 0x0400277F RID: 10111
		private GameObject focusedGameObject;

		// Token: 0x04002780 RID: 10112
		private static LevelObject focusedLevelObject;
	}
}
