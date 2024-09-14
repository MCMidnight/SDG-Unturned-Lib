using System;
using System.Collections.Generic;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Foliage;
using SDG.Framework.Landscapes;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x02000784 RID: 1924
	internal class EditorTerrainMaterialsUI : SleekFullscreenBox
	{
		// Token: 0x06003F68 RID: 16232 RVA: 0x0013EB01 File Offset: 0x0013CD01
		public void Open()
		{
			base.AnimateIntoView();
			TerrainEditor.toolMode = TerrainEditor.EDevkitLandscapeToolMode.SPLATMAP;
			EditorInteract.instance.SetActiveTool(EditorInteract.instance.terrainTool);
			if (FoliageSystem.instance != null)
			{
				FoliageSystem.instance.hiddenByMaterialEditor = true;
			}
			this.RefreshAssets();
		}

		// Token: 0x06003F69 RID: 16233 RVA: 0x0013EB41 File Offset: 0x0013CD41
		public void Close()
		{
			base.AnimateOutOfView(1f, 0f);
			DevkitLandscapeToolSplatmapOptions.save();
			EditorInteract.instance.SetActiveTool(null);
			if (FoliageSystem.instance != null)
			{
				FoliageSystem.instance.hiddenByMaterialEditor = false;
			}
		}

		// Token: 0x06003F6A RID: 16234 RVA: 0x0013EB7C File Offset: 0x0013CD7C
		public override void OnUpdate()
		{
			base.OnUpdate();
			this.modeButton.state = (int)TerrainEditor.splatmapMode;
			this.brushRadiusField.Value = DevkitLandscapeToolSplatmapOptions.instance.brushRadius;
			this.brushFalloffField.Value = DevkitLandscapeToolSplatmapOptions.instance.brushFalloff;
			this.brushStrengthField.Value = EditorInteract.instance.terrainTool.splatmapBrushStrength;
			this.weightTargetField.Value = DevkitLandscapeToolSplatmapOptions.instance.weightTarget;
			LandscapeMaterialAsset landscapeMaterialAsset = TerrainEditor.splatmapMaterialTarget.Find();
			if (this.selectedMaterialAsset != landscapeMaterialAsset)
			{
				this.selectedMaterialAsset = landscapeMaterialAsset;
				if (this.selectedMaterialAsset != null)
				{
					this.selectedAssetBox.icon = Assets.load<Texture2D>(this.selectedMaterialAsset.texture);
					this.selectedAssetBox.text = this.selectedMaterialAsset.FriendlyName;
				}
				else
				{
					this.selectedAssetBox.icon = null;
					this.selectedAssetBox.text = string.Empty;
				}
			}
			if (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.PAINT)
			{
				this.hintLabel.Text = this.localization.format("Hint_Paint", "Shift", "Ctrl", "Alt");
				this.hintLabel.IsVisible = true;
			}
			else if (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.CUT)
			{
				this.hintLabel.Text = this.localization.format("Hint_Cut", "Shift");
				this.hintLabel.IsVisible = true;
			}
			else
			{
				this.hintLabel.IsVisible = false;
			}
			this.UpdateLowerLeftOffset();
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x0013ECF4 File Offset: 0x0013CEF4
		public EditorTerrainMaterialsUI()
		{
			this.localization = Localization.read("/Editor/EditorTerrainMaterials.dat");
			DevkitLandscapeToolSplatmapOptions.load();
			this.searchAssets = new List<LandscapeMaterialAsset>();
			this.hintLabel = Glazier.Get().CreateLabel();
			this.hintLabel.PositionScale_Y = 1f;
			this.hintLabel.PositionOffset_Y = -30f;
			this.hintLabel.SizeScale_X = 1f;
			this.hintLabel.SizeOffset_Y = 30f;
			this.hintLabel.TextContrastContext = 2;
			this.hintLabel.IsVisible = false;
			base.AddChild(this.hintLabel);
			this.modeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(this.localization.format("Mode_Paint", "Q")),
				new GUIContent(this.localization.format("Mode_Auto", "W")),
				new GUIContent(this.localization.format("Mode_Smooth", "E")),
				new GUIContent(this.localization.format("Mode_Cut", "R"))
			});
			this.modeButton.PositionScale_Y = 1f;
			this.modeButton.SizeOffset_X = 200f;
			this.modeButton.SizeOffset_Y = 30f;
			this.modeButton.AddLabel(this.localization.format("Mode_Label"), 1);
			this.modeButton.state = (int)TerrainEditor.splatmapMode;
			SleekButtonState sleekButtonState = this.modeButton;
			sleekButtonState.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState.onSwappedState, new SwappedState(this.OnSwappedMode));
			base.AddChild(this.modeButton);
			this.brushRadiusField = Glazier.Get().CreateFloat32Field();
			this.brushRadiusField.PositionScale_Y = 1f;
			this.brushRadiusField.SizeOffset_X = 200f;
			this.brushRadiusField.SizeOffset_Y = 30f;
			this.brushRadiusField.AddLabel(this.localization.format("BrushRadius", "B"), 1);
			this.brushRadiusField.Value = DevkitLandscapeToolSplatmapOptions.instance.brushRadius;
			this.brushRadiusField.OnValueChanged += new TypedSingle(this.OnBrushRadiusTyped);
			base.AddChild(this.brushRadiusField);
			this.brushFalloffField = Glazier.Get().CreateFloat32Field();
			this.brushFalloffField.PositionScale_Y = 1f;
			this.brushFalloffField.SizeOffset_X = 200f;
			this.brushFalloffField.SizeOffset_Y = 30f;
			this.brushFalloffField.AddLabel(this.localization.format("BrushFalloff", "F"), 1);
			this.brushFalloffField.Value = DevkitLandscapeToolSplatmapOptions.instance.brushFalloff;
			this.brushFalloffField.OnValueChanged += new TypedSingle(this.OnBrushFalloffTyped);
			base.AddChild(this.brushFalloffField);
			this.brushStrengthField = Glazier.Get().CreateFloat32Field();
			this.brushStrengthField.PositionScale_Y = 1f;
			this.brushStrengthField.SizeOffset_X = 200f;
			this.brushStrengthField.SizeOffset_Y = 30f;
			this.brushStrengthField.AddLabel(this.localization.format("BrushStrength", "V"), 1);
			this.brushStrengthField.Value = DevkitLandscapeToolSplatmapOptions.instance.brushStrength;
			this.brushStrengthField.OnValueChanged += new TypedSingle(this.OnBrushStrengthTyped);
			base.AddChild(this.brushStrengthField);
			this.smoothMethodButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(this.localization.format("SmoothMethod_BrushAverage")),
				new GUIContent(this.localization.format("SmoothMethod_PixelAverage"))
			});
			this.smoothMethodButton.PositionScale_Y = 1f;
			this.smoothMethodButton.SizeOffset_X = 200f;
			this.smoothMethodButton.SizeOffset_Y = 30f;
			this.smoothMethodButton.AddLabel(this.localization.format("SmoothMethod_Label"), 1);
			this.smoothMethodButton.state = (int)DevkitLandscapeToolSplatmapOptions.instance.smoothMethod;
			SleekButtonState sleekButtonState2 = this.smoothMethodButton;
			sleekButtonState2.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState2.onSwappedState, new SwappedState(this.OnSwappedSmoothMethod));
			base.AddChild(this.smoothMethodButton);
			this.autoRayMaskField = Glazier.Get().CreateUInt32Field();
			this.autoRayMaskField.PositionScale_Y = 1f;
			this.autoRayMaskField.SizeOffset_X = 200f;
			this.autoRayMaskField.SizeOffset_Y = 30f;
			this.autoRayMaskField.AddLabel("Ray Mask (sorry this is not user-friendly at the moment)", 1);
			this.autoRayMaskField.Value = (uint)DevkitLandscapeToolSplatmapOptions.instance.autoRayMask;
			this.autoRayMaskField.OnValueChanged += new TypedUInt32(this.OnAutoRayMaskTyped);
			base.AddChild(this.autoRayMaskField);
			this.autoRayLengthField = Glazier.Get().CreateFloat32Field();
			this.autoRayLengthField.PositionScale_Y = 1f;
			this.autoRayLengthField.SizeOffset_X = 200f;
			this.autoRayLengthField.SizeOffset_Y = 30f;
			this.autoRayLengthField.AddLabel(this.localization.format("AutoRayLength"), 1);
			this.autoRayLengthField.Value = DevkitLandscapeToolSplatmapOptions.instance.autoRayLength;
			this.autoRayLengthField.OnValueChanged += new TypedSingle(this.OnAutoRayLengthTyped);
			base.AddChild(this.autoRayLengthField);
			this.autoRayRadiusField = Glazier.Get().CreateFloat32Field();
			this.autoRayRadiusField.PositionScale_Y = 1f;
			this.autoRayRadiusField.SizeOffset_X = 200f;
			this.autoRayRadiusField.SizeOffset_Y = 30f;
			this.autoRayRadiusField.AddLabel(this.localization.format("AutoRayRadius"), 1);
			this.autoRayRadiusField.Value = DevkitLandscapeToolSplatmapOptions.instance.autoRayRadius;
			this.autoRayRadiusField.OnValueChanged += new TypedSingle(this.OnAutoRayRadiusTyped);
			base.AddChild(this.autoRayRadiusField);
			this.useAutoFoundationToggle = Glazier.Get().CreateToggle();
			this.useAutoFoundationToggle.PositionScale_Y = 1f;
			this.useAutoFoundationToggle.SizeOffset_X = 40f;
			this.useAutoFoundationToggle.SizeOffset_Y = 40f;
			this.useAutoFoundationToggle.Value = DevkitLandscapeToolSplatmapOptions.instance.useAutoFoundation;
			this.useAutoFoundationToggle.OnValueChanged += new Toggled(this.OnClickedUseAutoFoundation);
			this.useAutoFoundationToggle.AddLabel(this.localization.format("UseAutoFoundation"), 1);
			base.AddChild(this.useAutoFoundationToggle);
			this.autoMaxAngleBeginField = Glazier.Get().CreateFloat32Field();
			this.autoMaxAngleBeginField.PositionScale_Y = 1f;
			this.autoMaxAngleBeginField.SizeOffset_X = 100f;
			this.autoMaxAngleBeginField.SizeOffset_Y = 30f;
			this.autoMaxAngleBeginField.Value = DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleBegin;
			this.autoMaxAngleBeginField.OnValueChanged += new TypedSingle(this.OnAutoMaxAngleBeginTyped);
			base.AddChild(this.autoMaxAngleBeginField);
			this.autoMaxAngleEndField = Glazier.Get().CreateFloat32Field();
			this.autoMaxAngleEndField.PositionOffset_X = 100f;
			this.autoMaxAngleEndField.PositionScale_Y = 1f;
			this.autoMaxAngleEndField.SizeOffset_X = 100f;
			this.autoMaxAngleEndField.SizeOffset_Y = 30f;
			this.autoMaxAngleEndField.Value = DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleEnd;
			this.autoMaxAngleEndField.OnValueChanged += new TypedSingle(this.OnAutoMaxAngleEndTyped);
			this.autoMaxAngleEndField.AddLabel(this.localization.format("MaxAngleRange"), 1);
			base.AddChild(this.autoMaxAngleEndField);
			this.autoMinAngleBeginField = Glazier.Get().CreateFloat32Field();
			this.autoMinAngleBeginField.PositionScale_Y = 1f;
			this.autoMinAngleBeginField.SizeOffset_X = 100f;
			this.autoMinAngleBeginField.SizeOffset_Y = 30f;
			this.autoMinAngleBeginField.Value = DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleBegin;
			this.autoMinAngleBeginField.OnValueChanged += new TypedSingle(this.OnAutoMinAngleBeginTyped);
			base.AddChild(this.autoMinAngleBeginField);
			this.autoMinAngleEndField = Glazier.Get().CreateFloat32Field();
			this.autoMinAngleEndField.PositionOffset_X = 100f;
			this.autoMinAngleEndField.PositionScale_Y = 1f;
			this.autoMinAngleEndField.SizeOffset_X = 100f;
			this.autoMinAngleEndField.SizeOffset_Y = 30f;
			this.autoMinAngleEndField.Value = DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleEnd;
			this.autoMinAngleEndField.OnValueChanged += new TypedSingle(this.OnAutoMinAngleEndTyped);
			this.autoMinAngleEndField.AddLabel(this.localization.format("MinAngleRange"), 1);
			base.AddChild(this.autoMinAngleEndField);
			this.useAutoSlopeToggle = Glazier.Get().CreateToggle();
			this.useAutoSlopeToggle.PositionScale_Y = 1f;
			this.useAutoSlopeToggle.SizeOffset_X = 40f;
			this.useAutoSlopeToggle.SizeOffset_Y = 40f;
			this.useAutoSlopeToggle.Value = DevkitLandscapeToolSplatmapOptions.instance.useAutoSlope;
			this.useAutoSlopeToggle.OnValueChanged += new Toggled(this.OnClickedUseAutoSlope);
			this.useAutoSlopeToggle.AddLabel(this.localization.format("UseAutoSlope"), 1);
			base.AddChild(this.useAutoSlopeToggle);
			this.useWeightTargetToggle = Glazier.Get().CreateToggle();
			this.useWeightTargetToggle.PositionScale_Y = 1f;
			this.useWeightTargetToggle.SizeOffset_X = 40f;
			this.useWeightTargetToggle.SizeOffset_Y = 40f;
			this.useWeightTargetToggle.Value = DevkitLandscapeToolSplatmapOptions.instance.useWeightTarget;
			this.useWeightTargetToggle.OnValueChanged += new Toggled(this.OnClickedUseWeightTarget);
			base.AddChild(this.useWeightTargetToggle);
			this.weightTargetField = Glazier.Get().CreateFloat32Field();
			this.weightTargetField.PositionOffset_X = 40f;
			this.weightTargetField.PositionScale_Y = 1f;
			this.weightTargetField.SizeOffset_X = 160f;
			this.weightTargetField.SizeOffset_Y = 30f;
			this.weightTargetField.Value = DevkitLandscapeToolSplatmapOptions.instance.weightTarget;
			this.weightTargetField.AddLabel(this.localization.format("WeightTarget", "G"), 1);
			this.weightTargetField.OnValueChanged += new TypedSingle(this.OnWeightTargetTyped);
			base.AddChild(this.weightTargetField);
			this.maxPreviewSamplesField = Glazier.Get().CreateUInt32Field();
			this.maxPreviewSamplesField.PositionScale_Y = 1f;
			this.maxPreviewSamplesField.SizeOffset_X = 200f;
			this.maxPreviewSamplesField.SizeOffset_Y = 30f;
			this.maxPreviewSamplesField.AddLabel(this.localization.format("MaxPreviewSamples"), 1);
			this.maxPreviewSamplesField.Value = DevkitLandscapeToolSplatmapOptions.instance.maxPreviewSamples;
			this.maxPreviewSamplesField.OnValueChanged += new TypedUInt32(this.OnMaxPreviewSamplesTyped);
			base.AddChild(this.maxPreviewSamplesField);
			this.previewMethodButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(this.localization.format("PreviewMethod_BrushAlpha")),
				new GUIContent(this.localization.format("PreviewMethod_Weight"))
			});
			this.previewMethodButton.PositionScale_Y = 1f;
			this.previewMethodButton.SizeOffset_X = 200f;
			this.previewMethodButton.SizeOffset_Y = 30f;
			this.previewMethodButton.AddLabel(this.localization.format("PreviewMethod_Label"), 1);
			this.previewMethodButton.state = (int)DevkitLandscapeToolSplatmapOptions.instance.previewMethod;
			SleekButtonState sleekButtonState3 = this.previewMethodButton;
			sleekButtonState3.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState3.onSwappedState, new SwappedState(this.OnSwappedPreviewMethod));
			base.AddChild(this.previewMethodButton);
			this.highlightHolesToggle = Glazier.Get().CreateToggle();
			this.highlightHolesToggle.PositionScale_Y = 1f;
			this.highlightHolesToggle.SizeOffset_X = 40f;
			this.highlightHolesToggle.SizeOffset_Y = 40f;
			this.highlightHolesToggle.OnValueChanged += new Toggled(this.OnClickedHighlightHoles);
			this.highlightHolesToggle.IsVisible = false;
			this.highlightHolesToggle.AddLabel(this.localization.format("HighlightHoles_Label"), 1);
			base.AddChild(this.highlightHolesToggle);
			this.UpdateLowerLeftOffset();
			float num = 0f;
			this.selectedAssetBox = new SleekBoxIcon(null, 64);
			this.selectedAssetBox.PositionScale_X = 1f;
			this.selectedAssetBox.SizeOffset_X = 300f;
			this.selectedAssetBox.PositionOffset_X = -this.selectedAssetBox.SizeOffset_X;
			this.selectedAssetBox.SizeOffset_Y = 74f;
			this.selectedAssetBox.AddLabel(this.localization.format("SelectedAsset", "Alt"), 0);
			base.AddChild(this.selectedAssetBox);
			num += this.selectedAssetBox.SizeOffset_Y + 10f;
			this.onlyUsedMaterialsToggle = Glazier.Get().CreateToggle();
			this.onlyUsedMaterialsToggle.PositionScale_X = 1f;
			this.onlyUsedMaterialsToggle.SizeOffset_X = 40f;
			this.onlyUsedMaterialsToggle.PositionOffset_X = -300f;
			this.onlyUsedMaterialsToggle.SizeOffset_Y = 40f;
			this.onlyUsedMaterialsToggle.PositionOffset_Y = num;
			this.onlyUsedMaterialsToggle.AddLabel(this.localization.format("OnlyUsedMaterials"), 1);
			this.onlyUsedMaterialsToggle.Value = true;
			this.onlyUsedMaterialsToggle.OnValueChanged += new Toggled(this.OnClickedOnlyUsedMaterials);
			base.AddChild(this.onlyUsedMaterialsToggle);
			num += this.onlyUsedMaterialsToggle.SizeOffset_Y + 10f;
			this.searchField = Glazier.Get().CreateStringField();
			this.searchField.PositionOffset_X = -300f;
			this.searchField.PositionOffset_Y = num;
			this.searchField.PositionScale_X = 1f;
			this.searchField.SizeOffset_X = 300f;
			this.searchField.SizeOffset_Y = 30f;
			this.searchField.PlaceholderText = this.localization.format("SearchHint");
			this.searchField.OnTextSubmitted += new Entered(this.OnNameFilterEntered);
			base.AddChild(this.searchField);
			num += this.searchField.SizeOffset_Y + 10f;
			this.assetScrollView = Glazier.Get().CreateScrollView();
			this.assetScrollView.PositionScale_X = 1f;
			this.assetScrollView.SizeOffset_X = 300f;
			this.assetScrollView.PositionOffset_X = -this.assetScrollView.SizeOffset_X;
			this.assetScrollView.PositionOffset_Y = num;
			this.assetScrollView.SizeOffset_Y = -num;
			this.assetScrollView.SizeScale_Y = 1f;
			this.assetScrollView.ScaleContentToWidth = true;
			base.AddChild(this.assetScrollView);
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x0013FC08 File Offset: 0x0013DE08
		private void OnSwappedMode(SleekButtonState element, int index)
		{
			TerrainEditor.splatmapMode = (TerrainEditor.EDevkitLandscapeToolSplatmapMode)index;
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x0013FC10 File Offset: 0x0013DE10
		private void OnBrushStrengthTyped(ISleekFloat32Field field, float state)
		{
			EditorInteract.instance.terrainTool.splatmapBrushStrength = state;
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x0013FC22 File Offset: 0x0013DE22
		private void OnBrushFalloffTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.brushFalloff = state;
		}

		// Token: 0x06003F6F RID: 16239 RVA: 0x0013FC2F File Offset: 0x0013DE2F
		private void OnBrushRadiusTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.brushRadius = state;
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x0013FC3C File Offset: 0x0013DE3C
		private void OnMaxPreviewSamplesTyped(ISleekUInt32Field field, uint state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.maxPreviewSamples = state;
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x0013FC49 File Offset: 0x0013DE49
		private void OnClickedUseWeightTarget(ISleekToggle toggle, bool state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.useWeightTarget = state;
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x0013FC56 File Offset: 0x0013DE56
		private void OnWeightTargetTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.weightTarget = state;
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x0013FC63 File Offset: 0x0013DE63
		private void OnSwappedSmoothMethod(SleekButtonState element, int index)
		{
			DevkitLandscapeToolSplatmapOptions.instance.smoothMethod = (EDevkitLandscapeToolSplatmapSmoothMethod)index;
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x0013FC70 File Offset: 0x0013DE70
		private void OnSwappedPreviewMethod(SleekButtonState element, int index)
		{
			DevkitLandscapeToolSplatmapOptions.instance.previewMethod = (EDevkitLandscapeToolSplatmapPreviewMethod)index;
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x0013FC7D File Offset: 0x0013DE7D
		private void OnClickedUseAutoSlope(ISleekToggle toggle, bool state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.useAutoSlope = state;
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x0013FC8A File Offset: 0x0013DE8A
		private void OnAutoMinAngleBeginTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleBegin = state;
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x0013FC97 File Offset: 0x0013DE97
		private void OnAutoMinAngleEndTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleEnd = state;
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x0013FCA4 File Offset: 0x0013DEA4
		private void OnAutoMaxAngleBeginTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleBegin = state;
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x0013FCB1 File Offset: 0x0013DEB1
		private void OnAutoMaxAngleEndTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleEnd = state;
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x0013FCBE File Offset: 0x0013DEBE
		private void OnClickedUseAutoFoundation(ISleekToggle toggle, bool state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.useAutoFoundation = state;
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x0013FCCB File Offset: 0x0013DECB
		private void OnAutoRayRadiusTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.autoRayRadius = state;
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x0013FCD8 File Offset: 0x0013DED8
		private void OnAutoRayLengthTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.autoRayLength = state;
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x0013FCE5 File Offset: 0x0013DEE5
		private void OnAutoRayMaskTyped(ISleekUInt32Field field, uint state)
		{
			DevkitLandscapeToolSplatmapOptions.instance.autoRayMask = (ERayMask)state;
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x0013FCF2 File Offset: 0x0013DEF2
		private void OnClickedHighlightHoles(ISleekToggle toggle, bool state)
		{
			Landscape.HighlightHoles = state;
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x0013FCFA File Offset: 0x0013DEFA
		private void OnClickedOnlyUsedMaterials(ISleekToggle toggle, bool state)
		{
			this.RefreshAssets();
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x0013FD02 File Offset: 0x0013DF02
		private void OnNameFilterEntered(ISleekField field)
		{
			this.RefreshAssets();
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x0013FD0C File Offset: 0x0013DF0C
		private void OnAssetClicked(ISleekElement button)
		{
			int num = this.assetScrollView.FindIndexOfChild(button);
			TerrainEditor.splatmapMaterialTarget = new AssetReference<LandscapeMaterialAsset>(this.searchAssets[num].GUID);
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x0013FD44 File Offset: 0x0013DF44
		private void RefreshAssets()
		{
			this.searchAssets.Clear();
			this.assetScrollView.RemoveAllChildren();
			float num = 0f;
			if (this.onlyUsedMaterialsToggle.Value)
			{
				Landscape.GetUniqueMaterials(this.searchAssets);
			}
			else
			{
				Assets.find<LandscapeMaterialAsset>(this.searchAssets);
			}
			string searchText = this.searchField.Text;
			if (!string.IsNullOrEmpty(searchText))
			{
				ListEx.RemoveSwap<LandscapeMaterialAsset>(this.searchAssets, (LandscapeMaterialAsset asset) => asset.FriendlyName.IndexOf(searchText, 1) == -1);
			}
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

		// Token: 0x06003F83 RID: 16259 RVA: 0x0013FEB8 File Offset: 0x0013E0B8
		private void UpdateLowerLeftOffset()
		{
			float num = 0f;
			num -= this.modeButton.SizeOffset_Y;
			this.modeButton.PositionOffset_Y = num;
			num -= 10f;
			num -= this.previewMethodButton.SizeOffset_Y;
			this.previewMethodButton.PositionOffset_Y = num;
			num -= 10f;
			num -= this.maxPreviewSamplesField.SizeOffset_Y;
			this.maxPreviewSamplesField.PositionOffset_Y = num;
			num -= 10f;
			this.smoothMethodButton.IsVisible = (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.SMOOTH);
			if (this.smoothMethodButton.IsVisible)
			{
				num -= this.smoothMethodButton.SizeOffset_Y;
				this.smoothMethodButton.PositionOffset_Y = num;
				num -= 10f;
			}
			this.autoRayRadiusField.IsVisible = (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.PAINT && DevkitLandscapeToolSplatmapOptions.instance.useAutoFoundation);
			this.autoRayLengthField.IsVisible = this.autoRayRadiusField.IsVisible;
			this.autoRayMaskField.IsVisible = this.autoRayRadiusField.IsVisible;
			if (this.autoRayRadiusField.IsVisible)
			{
				num -= this.autoRayMaskField.SizeOffset_Y;
				this.autoRayMaskField.PositionOffset_Y = num;
				num -= this.autoRayLengthField.SizeOffset_Y;
				this.autoRayLengthField.PositionOffset_Y = num;
				num -= this.autoRayRadiusField.SizeOffset_Y;
				this.autoRayRadiusField.PositionOffset_Y = num;
			}
			this.useAutoFoundationToggle.IsVisible = (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.PAINT);
			if (this.useAutoFoundationToggle.IsVisible)
			{
				num -= this.useAutoFoundationToggle.SizeOffset_Y;
				this.useAutoFoundationToggle.PositionOffset_Y = num;
				num -= 10f;
			}
			this.autoMinAngleBeginField.IsVisible = (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.PAINT && DevkitLandscapeToolSplatmapOptions.instance.useAutoSlope);
			this.autoMinAngleEndField.IsVisible = this.autoMinAngleBeginField.IsVisible;
			this.autoMaxAngleBeginField.IsVisible = this.autoMinAngleBeginField.IsVisible;
			this.autoMaxAngleEndField.IsVisible = this.autoMinAngleBeginField.IsVisible;
			if (this.autoMinAngleBeginField.IsVisible)
			{
				num -= this.autoMaxAngleBeginField.SizeOffset_Y;
				this.autoMaxAngleBeginField.PositionOffset_Y = num;
				this.autoMaxAngleEndField.PositionOffset_Y = num;
				num -= this.autoMinAngleBeginField.SizeOffset_Y;
				this.autoMinAngleBeginField.PositionOffset_Y = num;
				this.autoMinAngleEndField.PositionOffset_Y = num;
			}
			this.useAutoSlopeToggle.IsVisible = (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.PAINT);
			if (this.useAutoSlopeToggle.IsVisible)
			{
				num -= this.useAutoSlopeToggle.SizeOffset_Y;
				this.useAutoSlopeToggle.PositionOffset_Y = num;
				num -= 10f;
			}
			this.useWeightTargetToggle.IsVisible = (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.PAINT);
			this.weightTargetField.IsVisible = this.useWeightTargetToggle.IsVisible;
			if (this.useWeightTargetToggle.IsVisible)
			{
				num -= this.useWeightTargetToggle.SizeOffset_Y;
				this.useWeightTargetToggle.PositionOffset_Y = num;
				this.weightTargetField.PositionOffset_Y = num + 5f;
				num -= 10f;
			}
			this.brushStrengthField.IsVisible = (TerrainEditor.splatmapMode != TerrainEditor.EDevkitLandscapeToolSplatmapMode.CUT);
			this.brushFalloffField.IsVisible = this.brushStrengthField.IsVisible;
			if (this.brushStrengthField.IsVisible)
			{
				num -= this.brushStrengthField.SizeOffset_Y;
				this.brushStrengthField.PositionOffset_Y = num;
				num -= 10f;
				num -= this.brushFalloffField.SizeOffset_Y;
				this.brushFalloffField.PositionOffset_Y = num;
				num -= 10f;
			}
			num -= this.brushRadiusField.SizeOffset_Y;
			this.brushRadiusField.PositionOffset_Y = num;
			num -= 10f;
			this.highlightHolesToggle.IsVisible = (TerrainEditor.splatmapMode == TerrainEditor.EDevkitLandscapeToolSplatmapMode.CUT);
			if (this.highlightHolesToggle.IsVisible)
			{
				num -= this.highlightHolesToggle.SizeOffset_Y;
				this.highlightHolesToggle.PositionOffset_Y = num;
				num -= 10f;
			}
		}

		// Token: 0x04002840 RID: 10304
		private Local localization;

		// Token: 0x04002841 RID: 10305
		private LandscapeMaterialAsset selectedMaterialAsset;

		// Token: 0x04002842 RID: 10306
		private List<LandscapeMaterialAsset> searchAssets;

		// Token: 0x04002843 RID: 10307
		private ISleekLabel hintLabel;

		// Token: 0x04002844 RID: 10308
		private SleekButtonState modeButton;

		// Token: 0x04002845 RID: 10309
		private ISleekFloat32Field brushRadiusField;

		// Token: 0x04002846 RID: 10310
		private ISleekFloat32Field brushFalloffField;

		// Token: 0x04002847 RID: 10311
		private ISleekFloat32Field brushStrengthField;

		// Token: 0x04002848 RID: 10312
		private ISleekToggle useWeightTargetToggle;

		// Token: 0x04002849 RID: 10313
		private ISleekFloat32Field weightTargetField;

		// Token: 0x0400284A RID: 10314
		private ISleekUInt32Field maxPreviewSamplesField;

		// Token: 0x0400284B RID: 10315
		private SleekButtonState smoothMethodButton;

		// Token: 0x0400284C RID: 10316
		private SleekButtonState previewMethodButton;

		// Token: 0x0400284D RID: 10317
		private ISleekToggle highlightHolesToggle;

		// Token: 0x0400284E RID: 10318
		private ISleekToggle useAutoSlopeToggle;

		// Token: 0x0400284F RID: 10319
		private ISleekFloat32Field autoMinAngleBeginField;

		// Token: 0x04002850 RID: 10320
		private ISleekFloat32Field autoMinAngleEndField;

		// Token: 0x04002851 RID: 10321
		private ISleekFloat32Field autoMaxAngleBeginField;

		// Token: 0x04002852 RID: 10322
		private ISleekFloat32Field autoMaxAngleEndField;

		// Token: 0x04002853 RID: 10323
		private ISleekToggle useAutoFoundationToggle;

		// Token: 0x04002854 RID: 10324
		private ISleekFloat32Field autoRayRadiusField;

		// Token: 0x04002855 RID: 10325
		private ISleekFloat32Field autoRayLengthField;

		// Token: 0x04002856 RID: 10326
		private ISleekUInt32Field autoRayMaskField;

		// Token: 0x04002857 RID: 10327
		private SleekBoxIcon selectedAssetBox;

		// Token: 0x04002858 RID: 10328
		private ISleekToggle onlyUsedMaterialsToggle;

		// Token: 0x04002859 RID: 10329
		private ISleekField searchField;

		// Token: 0x0400285A RID: 10330
		private ISleekScrollView assetScrollView;
	}
}
