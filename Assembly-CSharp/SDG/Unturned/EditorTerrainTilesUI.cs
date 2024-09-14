using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Landscapes;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000786 RID: 1926
	internal class EditorTerrainTilesUI : SleekFullscreenBox
	{
		// Token: 0x06003F87 RID: 16263 RVA: 0x0014043B File Offset: 0x0013E63B
		public void Open()
		{
			base.AnimateIntoView();
			TerrainEditor.toolMode = TerrainEditor.EDevkitLandscapeToolMode.TILE;
			EditorInteract.instance.SetActiveTool(EditorInteract.instance.terrainTool);
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x0014045D File Offset: 0x0013E65D
		public void Close()
		{
			base.AnimateOutOfView(1f, 0f);
			TerrainEditor.selectedTile = null;
			EditorInteract.instance.SetActiveTool(null);
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x00140480 File Offset: 0x0013E680
		public override void OnDestroy()
		{
			TerrainEditor.selectedTileChanged -= this.OnSelectedTileChanged;
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x00140494 File Offset: 0x0013E694
		public EditorTerrainTilesUI()
		{
			EditorTerrainTilesUI.localization = Localization.read("/Editor/EditorTerrainTiles.dat");
			TerrainEditor.selectedTileChanged += this.OnSelectedTileChanged;
			this.hintLabel = Glazier.Get().CreateLabel();
			this.hintLabel.PositionScale_Y = 1f;
			this.hintLabel.PositionOffset_Y = -30f;
			this.hintLabel.SizeScale_X = 1f;
			this.hintLabel.SizeOffset_Y = 30f;
			this.hintLabel.TextContrastContext = 2;
			this.hintLabel.Text = EditorTerrainTilesUI.localization.format("Hint_Remove", "Delete");
			base.AddChild(this.hintLabel);
			float num = 0f;
			this.repairEdgesButton = Glazier.Get().CreateButton();
			this.repairEdgesButton.PositionScale_X = 1f;
			this.repairEdgesButton.PositionScale_Y = 1f;
			this.repairEdgesButton.PositionOffset_X = -560f;
			this.repairEdgesButton.SizeOffset_X = 250f;
			this.repairEdgesButton.SizeOffset_Y = 30f;
			num -= this.repairEdgesButton.SizeOffset_Y;
			this.repairEdgesButton.PositionOffset_Y = num;
			num -= 10f;
			this.repairEdgesButton.Text = EditorTerrainTilesUI.localization.format("RepairEdges_Label");
			this.repairEdgesButton.OnClicked += new ClickedButton(this.OnClickedRepairEdges);
			this.repairEdgesButton.TooltipText = EditorTerrainTilesUI.localization.format("RepairEdges_Tooltip");
			base.AddChild(this.repairEdgesButton);
			this.applyToAllTilesButton = new SleekButtonIconConfirm(null, EditorTerrainTilesUI.localization.format("ApplyToAllTiles_Confirm_Label"), EditorTerrainTilesUI.localization.format("ApplyToAllTiles_Confirm_Tooltip"), EditorTerrainTilesUI.localization.format("ApplyToAllTiles_Deny_Label"), EditorTerrainTilesUI.localization.format("ApplyToAllTiles_Deny_Tooltip"));
			this.applyToAllTilesButton.PositionScale_X = 1f;
			this.applyToAllTilesButton.PositionScale_Y = 1f;
			this.applyToAllTilesButton.PositionOffset_X = -560f;
			this.applyToAllTilesButton.SizeOffset_X = 250f;
			this.applyToAllTilesButton.SizeOffset_Y = 30f;
			num -= this.applyToAllTilesButton.SizeOffset_Y;
			this.applyToAllTilesButton.PositionOffset_Y = num;
			num -= 10f;
			this.applyToAllTilesButton.text = EditorTerrainTilesUI.localization.format("ApplyToAllTiles_Label");
			this.applyToAllTilesButton.tooltip = EditorTerrainTilesUI.localization.format("ApplyToAllTiles_Tooltip");
			SleekButtonIconConfirm sleekButtonIconConfirm = this.applyToAllTilesButton;
			sleekButtonIconConfirm.onConfirmed = (Confirm)Delegate.Combine(sleekButtonIconConfirm.onConfirmed, new Confirm(this.OnApplyToAllTiles));
			base.AddChild(this.applyToAllTilesButton);
			this.resetSplatmapButton = new SleekButtonIconConfirm(null, EditorTerrainTilesUI.localization.format("ResetSplatmap_Confirm_Label"), EditorTerrainTilesUI.localization.format("ResetSplatmap_Confirm_Tooltip"), EditorTerrainTilesUI.localization.format("ResetSplatmap_Deny_Label"), EditorTerrainTilesUI.localization.format("ResetSplatmap_Deny_Tooltip"));
			this.resetSplatmapButton.PositionScale_X = 1f;
			this.resetSplatmapButton.PositionScale_Y = 1f;
			this.resetSplatmapButton.PositionOffset_X = -560f;
			this.resetSplatmapButton.SizeOffset_X = 250f;
			this.resetSplatmapButton.SizeOffset_Y = 30f;
			num -= this.resetSplatmapButton.SizeOffset_Y;
			this.resetSplatmapButton.PositionOffset_Y = num;
			num -= 10f;
			this.resetSplatmapButton.text = EditorTerrainTilesUI.localization.format("ResetSplatmap_Label");
			this.resetSplatmapButton.tooltip = EditorTerrainTilesUI.localization.format("ResetSplatmap_Tooltip");
			SleekButtonIconConfirm sleekButtonIconConfirm2 = this.resetSplatmapButton;
			sleekButtonIconConfirm2.onConfirmed = (Confirm)Delegate.Combine(sleekButtonIconConfirm2.onConfirmed, new Confirm(this.OnResetSplatmap));
			base.AddChild(this.resetSplatmapButton);
			this.resetHeightmapButton = new SleekButtonIconConfirm(null, EditorTerrainTilesUI.localization.format("ResetHeightmap_Confirm_Label"), EditorTerrainTilesUI.localization.format("ResetHeightmap_Confirm_Tooltip"), EditorTerrainTilesUI.localization.format("ResetHeightmap_Deny_Label"), EditorTerrainTilesUI.localization.format("ResetHeightmap_Deny_Tooltip"));
			this.resetHeightmapButton.PositionScale_X = 1f;
			this.resetHeightmapButton.PositionScale_Y = 1f;
			this.resetHeightmapButton.PositionOffset_X = -560f;
			this.resetHeightmapButton.SizeOffset_X = 250f;
			this.resetHeightmapButton.SizeOffset_Y = 30f;
			num -= this.resetHeightmapButton.SizeOffset_Y;
			this.resetHeightmapButton.PositionOffset_Y = num;
			num -= 10f;
			this.resetHeightmapButton.text = EditorTerrainTilesUI.localization.format("ResetHeightmap_Label");
			this.resetHeightmapButton.tooltip = EditorTerrainTilesUI.localization.format("ResetHeightmap_Tooltip");
			SleekButtonIconConfirm sleekButtonIconConfirm3 = this.resetHeightmapButton;
			sleekButtonIconConfirm3.onConfirmed = (Confirm)Delegate.Combine(sleekButtonIconConfirm3.onConfirmed, new Confirm(this.OnResetHeightmap));
			base.AddChild(this.resetHeightmapButton);
			this.layers = new TerrainTileLayer[Landscape.SPLATMAP_LAYERS];
			for (int i = this.layers.Length - 1; i >= 0; i--)
			{
				TerrainTileLayer terrainTileLayer = new TerrainTileLayer(this, i);
				this.layers[i] = terrainTileLayer;
				terrainTileLayer.PositionScale_X = 1f;
				terrainTileLayer.PositionScale_Y = 1f;
				terrainTileLayer.PositionOffset_X = -560f;
				terrainTileLayer.SizeOffset_X = 250f;
				terrainTileLayer.SizeOffset_Y = 30f;
				num -= terrainTileLayer.SizeOffset_Y;
				terrainTileLayer.PositionOffset_Y = num;
				base.AddChild(terrainTileLayer);
			}
			int num2 = 300;
			float num3 = 0f;
			this.selectedLayerBox = new SleekBoxIcon(null, 64);
			this.selectedLayerBox.SizeOffset_X = (float)num2;
			this.selectedLayerBox.PositionOffset_X = -this.selectedLayerBox.SizeOffset_X;
			this.selectedLayerBox.SizeOffset_Y = 74f;
			this.selectedLayerBox.PositionScale_X = 1f;
			this.selectedLayerBox.AddLabel(EditorTerrainTilesUI.localization.format("SelectedLayer"), 0);
			base.AddChild(this.selectedLayerBox);
			num3 += this.selectedLayerBox.SizeOffset_Y + 10f;
			this.layerGuidField = Glazier.Get().CreateStringField();
			this.layerGuidField.SizeOffset_X = (float)num2;
			this.layerGuidField.PositionOffset_X = -this.layerGuidField.SizeOffset_X;
			this.layerGuidField.PositionOffset_Y = num3;
			this.layerGuidField.SizeOffset_Y = 30f;
			this.layerGuidField.PositionScale_X = 1f;
			this.layerGuidField.MaxLength = 32;
			this.layerGuidField.AddLabel(EditorTerrainTilesUI.localization.format("LayerGuid"), 0);
			this.layerGuidField.OnTextSubmitted += new Entered(this.OnLayerGuidEntered);
			base.AddChild(this.layerGuidField);
			num3 += this.layerGuidField.SizeOffset_Y + 10f;
			this.resetAssetButton = Glazier.Get().CreateButton();
			this.resetAssetButton.SizeOffset_X = (float)num2;
			this.resetAssetButton.PositionOffset_X = -this.resetAssetButton.SizeOffset_X;
			this.resetAssetButton.PositionOffset_Y = num3;
			this.resetAssetButton.SizeOffset_Y = 30f;
			this.resetAssetButton.PositionScale_X = 1f;
			this.resetAssetButton.Text = EditorTerrainTilesUI.localization.format("ResetAsset");
			this.resetAssetButton.OnClicked += new ClickedButton(this.OnClickedResetAsset);
			base.AddChild(this.resetAssetButton);
			num3 += this.resetAssetButton.SizeOffset_Y + 10f;
			this.searchAssets = new List<LandscapeMaterialAsset>();
			this.assetScrollView = Glazier.Get().CreateScrollView();
			this.assetScrollView.PositionOffset_Y = num3;
			this.assetScrollView.PositionScale_X = 1f;
			this.assetScrollView.SizeOffset_X = (float)num2;
			this.assetScrollView.SizeOffset_Y = -num3;
			this.assetScrollView.PositionOffset_X = -this.assetScrollView.SizeOffset_X;
			this.assetScrollView.SizeScale_Y = 1f;
			this.assetScrollView.ScaleContentToWidth = true;
			base.AddChild(this.assetScrollView);
			this.RefreshAssets();
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x00140CC0 File Offset: 0x0013EEC0
		private void OnSelectedTileChanged(LandscapeTile oldSelectedTile, LandscapeTile newSelectedTile)
		{
			TerrainTileLayer[] array = this.layers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UpdateSelectedTile();
			}
			this.SetSelectedLayerIndex(-1);
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x00140CF4 File Offset: 0x0013EEF4
		public void SetSelectedLayerIndex(int layerIndex)
		{
			this.selectedLayerIndex = layerIndex;
			LandscapeTile selectedTile = TerrainEditor.selectedTile;
			if (selectedTile != null && layerIndex >= 0)
			{
				AssetReference<LandscapeMaterialAsset> assetReference = selectedTile.materials[layerIndex];
				LandscapeMaterialAsset landscapeMaterialAsset = assetReference.Find();
				if (landscapeMaterialAsset != null)
				{
					this.selectedLayerBox.icon = Assets.load<Texture2D>(landscapeMaterialAsset.texture);
					this.selectedLayerBox.text = landscapeMaterialAsset.FriendlyName;
				}
				else if (assetReference.isNull)
				{
					this.selectedLayerBox.icon = null;
					this.selectedLayerBox.text = EditorTerrainTilesUI.localization.format("LayerNull");
				}
				else
				{
					this.selectedLayerBox.icon = null;
					this.selectedLayerBox.text = EditorTerrainTilesUI.localization.format("LayerMissing");
				}
				this.layerGuidField.Text = assetReference.ToString();
				return;
			}
			this.selectedLayerBox.icon = null;
			this.selectedLayerBox.text = string.Empty;
			this.layerGuidField.Text = string.Empty;
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x00140DF8 File Offset: 0x0013EFF8
		private void OnLayerGuidEntered(ISleekField field)
		{
			LandscapeTile selectedTile = TerrainEditor.selectedTile;
			if (selectedTile == null || this.selectedLayerIndex < 0)
			{
				return;
			}
			AssetReference<LandscapeMaterialAsset> assetReference;
			AssetReference<LandscapeMaterialAsset>.TryParse(field.Text, out assetReference);
			selectedTile.materials[this.selectedLayerIndex] = assetReference;
			selectedTile.updatePrototypes();
			this.layers[this.selectedLayerIndex].UpdateSelectedTile();
			this.SetSelectedLayerIndex(this.selectedLayerIndex);
			LevelHierarchy.MarkDirty();
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x00140E64 File Offset: 0x0013F064
		private void OnClickedResetAsset(ISleekElement button)
		{
			LandscapeTile selectedTile = TerrainEditor.selectedTile;
			if (selectedTile == null || this.selectedLayerIndex < 0)
			{
				return;
			}
			selectedTile.materials[this.selectedLayerIndex] = AssetReference<LandscapeMaterialAsset>.invalid;
			selectedTile.updatePrototypes();
			this.layers[this.selectedLayerIndex].UpdateSelectedTile();
			this.SetSelectedLayerIndex(this.selectedLayerIndex);
			LevelHierarchy.MarkDirty();
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x00140EC4 File Offset: 0x0013F0C4
		private void OnAssetClicked(ISleekElement button)
		{
			LandscapeTile selectedTile = TerrainEditor.selectedTile;
			if (selectedTile == null || this.selectedLayerIndex < 0)
			{
				return;
			}
			int num = this.assetScrollView.FindIndexOfChild(button);
			selectedTile.materials[this.selectedLayerIndex] = this.searchAssets[num].getReferenceTo<LandscapeMaterialAsset>();
			selectedTile.updatePrototypes();
			this.layers[this.selectedLayerIndex].UpdateSelectedTile();
			this.SetSelectedLayerIndex(this.selectedLayerIndex);
			LevelHierarchy.MarkDirty();
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x00140F3C File Offset: 0x0013F13C
		private void RefreshAssets()
		{
			this.searchAssets.Clear();
			this.assetScrollView.RemoveAllChildren();
			float num = 0f;
			Assets.find<LandscapeMaterialAsset>(this.searchAssets);
			this.searchAssets.Sort((LandscapeMaterialAsset lhs, LandscapeMaterialAsset rhs) => lhs.name.CompareTo(rhs.name));
			foreach (LandscapeMaterialAsset landscapeMaterialAsset in this.searchAssets)
			{
				SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(Assets.load<Texture2D>(landscapeMaterialAsset.texture), 64);
				sleekButtonIcon.PositionOffset_Y = num;
				sleekButtonIcon.SizeScale_X = 1f;
				sleekButtonIcon.SizeOffset_Y = 74f;
				sleekButtonIcon.text = landscapeMaterialAsset.FriendlyName;
				sleekButtonIcon.onClickedButton += new ClickedButton(this.OnAssetClicked);
				this.assetScrollView.AddChild(sleekButtonIcon);
				num += sleekButtonIcon.SizeOffset_Y;
			}
			this.assetScrollView.ContentSizeOffset = new Vector2(0f, num);
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x00141054 File Offset: 0x0013F254
		private void OnResetHeightmap(SleekButtonIconConfirm button)
		{
			LandscapeTile selectedTile = TerrainEditor.selectedTile;
			if (selectedTile == null)
			{
				return;
			}
			selectedTile.resetHeightmap();
			LevelHierarchy.MarkDirty();
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x00141078 File Offset: 0x0013F278
		private void OnResetSplatmap(SleekButtonIconConfirm button)
		{
			LandscapeTile selectedTile = TerrainEditor.selectedTile;
			if (selectedTile == null)
			{
				return;
			}
			selectedTile.resetSplatmap();
			LevelHierarchy.MarkDirty();
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x0014109C File Offset: 0x0013F29C
		private void OnApplyToAllTiles(SleekButtonIconConfirm button)
		{
			LandscapeTile selectedTile = TerrainEditor.selectedTile;
			if (selectedTile == null)
			{
				return;
			}
			Landscape.CopyLayersToAllTiles(selectedTile);
			LevelHierarchy.MarkDirty();
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x001410C0 File Offset: 0x0013F2C0
		private void OnClickedRepairEdges(ISleekElement button)
		{
			LandscapeTile selectedTile = TerrainEditor.selectedTile;
			if (selectedTile == null)
			{
				return;
			}
			Landscape.reconcileNeighbors(selectedTile);
			LevelHierarchy.MarkDirty();
		}

		// Token: 0x0400285F RID: 10335
		internal static Local localization;

		// Token: 0x04002860 RID: 10336
		private ISleekLabel hintLabel;

		// Token: 0x04002861 RID: 10337
		private TerrainTileLayer[] layers;

		// Token: 0x04002862 RID: 10338
		private SleekButtonIconConfirm resetHeightmapButton;

		// Token: 0x04002863 RID: 10339
		private SleekButtonIconConfirm resetSplatmapButton;

		// Token: 0x04002864 RID: 10340
		private SleekButtonIconConfirm applyToAllTilesButton;

		// Token: 0x04002865 RID: 10341
		private ISleekButton repairEdgesButton;

		// Token: 0x04002866 RID: 10342
		internal int selectedLayerIndex;

		// Token: 0x04002867 RID: 10343
		private List<LandscapeMaterialAsset> searchAssets;

		// Token: 0x04002868 RID: 10344
		private SleekBoxIcon selectedLayerBox;

		// Token: 0x04002869 RID: 10345
		private ISleekField layerGuidField;

		// Token: 0x0400286A RID: 10346
		private ISleekButton resetAssetButton;

		// Token: 0x0400286B RID: 10347
		private ISleekScrollView assetScrollView;
	}
}
