using System;
using System.Collections.Generic;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Foliage;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x02000782 RID: 1922
	internal class EditorTerrainDetailsUI : SleekFullscreenBox
	{
		// Token: 0x06003F41 RID: 16193 RVA: 0x0013C92F File Offset: 0x0013AB2F
		public void Open()
		{
			base.AnimateIntoView();
			EditorInteract.instance.SetActiveTool(this.tool);
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x0013C947 File Offset: 0x0013AB47
		public void Close()
		{
			base.AnimateOutOfView(1f, 0f);
			DevkitFoliageToolOptions.save();
			EditorInteract.instance.SetActiveTool(null);
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x0013C96C File Offset: 0x0013AB6C
		public override void OnUpdate()
		{
			base.OnUpdate();
			this.modeButton.state = (int)this.tool.mode;
			this.brushRadiusField.Value = DevkitFoliageToolOptions.instance.brushRadius;
			this.brushFalloffField.Value = DevkitFoliageToolOptions.instance.brushFalloff;
			this.brushStrengthField.Value = DevkitFoliageToolOptions.instance.brushStrength;
			this.densityTargetField.Value = DevkitFoliageToolOptions.instance.densityTarget;
			int bakeQueueProgress = FoliageSystem.bakeQueueProgress;
			int bakeQueueTotal = FoliageSystem.bakeQueueTotal;
			if (bakeQueueProgress == bakeQueueTotal || bakeQueueTotal < 1)
			{
				this.bakeProgressLabel.IsVisible = false;
			}
			else
			{
				float num = (float)bakeQueueProgress / (float)bakeQueueTotal;
				this.bakeProgressLabel.IsVisible = true;
				this.bakeProgressLabel.Text = string.Concat(new string[]
				{
					bakeQueueProgress.ToString(),
					"/",
					bakeQueueTotal.ToString(),
					" [",
					num.ToString("P"),
					"]"
				});
			}
			if (this.tool.mode == FoliageEditor.EFoliageMode.PAINT)
			{
				this.hintLabel.Text = this.localization.format("Hint_Paint", "Shift", "Ctrl", "Alt");
				this.hintLabel.IsVisible = true;
			}
			else
			{
				this.hintLabel.IsVisible = false;
			}
			this.UpdateOffsets();
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x0013CAC8 File Offset: 0x0013ACC8
		public EditorTerrainDetailsUI()
		{
			this.localization = Localization.read("/Editor/EditorTerrainDetails.dat");
			DevkitFoliageToolOptions.load();
			this.tool = new FoliageEditor();
			this.searchInfoAssets = new List<FoliageInfoAsset>();
			this.searchCollectionAssets = new List<FoliageInfoCollectionAsset>();
			this.maxPreviewSamplesField = Glazier.Get().CreateUInt32Field();
			this.maxPreviewSamplesField.PositionScale_Y = 1f;
			this.maxPreviewSamplesField.SizeOffset_X = 200f;
			this.maxPreviewSamplesField.SizeOffset_Y = 30f;
			this.maxPreviewSamplesField.AddLabel(this.localization.format("MaxPreviewSamples"), 1);
			this.maxPreviewSamplesField.Value = DevkitFoliageToolOptions.instance.maxPreviewSamples;
			this.maxPreviewSamplesField.OnValueChanged += new TypedUInt32(this.OnMaxPreviewSamplesTyped);
			base.AddChild(this.maxPreviewSamplesField);
			this.surfaceMaskField = Glazier.Get().CreateUInt32Field();
			this.surfaceMaskField.PositionScale_Y = 1f;
			this.surfaceMaskField.SizeOffset_X = 200f;
			this.surfaceMaskField.SizeOffset_Y = 30f;
			this.surfaceMaskField.AddLabel("Surface Mask (sorry this is not user-friendly at the moment)", 1);
			this.surfaceMaskField.Value = (uint)DevkitFoliageToolOptions.instance.surfaceMask;
			this.surfaceMaskField.OnValueChanged += new TypedUInt32(this.OnSurfaceMaskTyped);
			base.AddChild(this.surfaceMaskField);
			this.densityTargetField = Glazier.Get().CreateFloat32Field();
			this.densityTargetField.PositionScale_Y = 1f;
			this.densityTargetField.SizeOffset_X = 200f;
			this.densityTargetField.SizeOffset_Y = 30f;
			this.densityTargetField.AddLabel(this.localization.format("DensityTarget"), 1);
			this.densityTargetField.Value = DevkitFoliageToolOptions.instance.densityTarget;
			this.densityTargetField.OnValueChanged += new TypedSingle(this.OnDensityTargetTyped);
			base.AddChild(this.densityTargetField);
			this.brushStrengthField = Glazier.Get().CreateFloat32Field();
			this.brushStrengthField.PositionScale_Y = 1f;
			this.brushStrengthField.SizeOffset_X = 200f;
			this.brushStrengthField.SizeOffset_Y = 30f;
			this.brushStrengthField.AddLabel(this.localization.format("BrushStrength", "V"), 1);
			this.brushStrengthField.Value = DevkitFoliageToolOptions.instance.brushStrength;
			this.brushStrengthField.OnValueChanged += new TypedSingle(this.OnBrushStrengthTyped);
			base.AddChild(this.brushStrengthField);
			this.brushFalloffField = Glazier.Get().CreateFloat32Field();
			this.brushFalloffField.PositionScale_Y = 1f;
			this.brushFalloffField.SizeOffset_X = 200f;
			this.brushFalloffField.SizeOffset_Y = 30f;
			this.brushFalloffField.AddLabel(this.localization.format("BrushFalloff", "F"), 1);
			this.brushFalloffField.Value = DevkitFoliageToolOptions.instance.brushFalloff;
			this.brushFalloffField.OnValueChanged += new TypedSingle(this.OnBrushFalloffTyped);
			base.AddChild(this.brushFalloffField);
			this.brushRadiusField = Glazier.Get().CreateFloat32Field();
			this.brushRadiusField.PositionScale_Y = 1f;
			this.brushRadiusField.SizeOffset_X = 200f;
			this.brushRadiusField.SizeOffset_Y = 30f;
			this.brushRadiusField.AddLabel(this.localization.format("BrushRadius", "B"), 1);
			this.brushRadiusField.Value = DevkitFoliageToolOptions.instance.brushRadius;
			this.brushRadiusField.OnValueChanged += new TypedSingle(this.OnBrushRadiusTyped);
			base.AddChild(this.brushRadiusField);
			this.modeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(this.localization.format("Mode_Paint", "Q")),
				new GUIContent(this.localization.format("Mode_Exact", "W")),
				new GUIContent(this.localization.format("Mode_Bake", "E"))
			});
			this.modeButton.PositionScale_Y = 1f;
			this.modeButton.SizeOffset_X = 200f;
			this.modeButton.SizeOffset_Y = 30f;
			this.modeButton.AddLabel(this.localization.format("Mode_Label"), 1);
			this.modeButton.state = (int)this.tool.mode;
			SleekButtonState sleekButtonState = this.modeButton;
			sleekButtonState.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState.onSwappedState, new SwappedState(this.OnSwappedMode));
			base.AddChild(this.modeButton);
			float num = 0f;
			this.bakeCancelButton = Glazier.Get().CreateButton();
			this.bakeCancelButton.PositionScale_X = 1f;
			this.bakeCancelButton.PositionScale_Y = 1f;
			this.bakeCancelButton.SizeOffset_X = 200f;
			this.bakeCancelButton.PositionOffset_X = -this.bakeCancelButton.SizeOffset_X;
			this.bakeCancelButton.SizeOffset_Y = 30f;
			num -= this.bakeCancelButton.SizeOffset_Y;
			this.bakeCancelButton.PositionOffset_Y = num;
			num -= 10f;
			this.bakeCancelButton.Text = this.localization.format("Bake_Cancel");
			this.bakeCancelButton.OnClicked += new ClickedButton(this.OnBakeCancelButtonClicked);
			base.AddChild(this.bakeCancelButton);
			this.bakeLocalButton = Glazier.Get().CreateButton();
			this.bakeLocalButton.PositionScale_X = 1f;
			this.bakeLocalButton.PositionScale_Y = 1f;
			this.bakeLocalButton.SizeOffset_X = 200f;
			this.bakeLocalButton.PositionOffset_X = -this.bakeLocalButton.SizeOffset_X;
			this.bakeLocalButton.SizeOffset_Y = 30f;
			num -= this.bakeLocalButton.SizeOffset_Y;
			this.bakeLocalButton.PositionOffset_Y = num;
			num -= 10f;
			this.bakeLocalButton.Text = this.localization.format("Bake_Local");
			this.bakeLocalButton.OnClicked += new ClickedButton(this.OnBakeLocalButtonClicked);
			base.AddChild(this.bakeLocalButton);
			this.bakeGlobalButton = Glazier.Get().CreateButton();
			this.bakeGlobalButton.PositionScale_X = 1f;
			this.bakeGlobalButton.PositionScale_Y = 1f;
			this.bakeGlobalButton.SizeOffset_X = 200f;
			this.bakeGlobalButton.PositionOffset_X = -this.bakeGlobalButton.SizeOffset_X;
			this.bakeGlobalButton.SizeOffset_Y = 30f;
			num -= this.bakeGlobalButton.SizeOffset_Y;
			this.bakeGlobalButton.PositionOffset_Y = num;
			num -= 10f;
			this.bakeGlobalButton.Text = this.localization.format("Bake_Global");
			this.bakeGlobalButton.OnClicked += new ClickedButton(this.OnBakeGlobalButtonClicked);
			base.AddChild(this.bakeGlobalButton);
			this.bakeClearToggle = Glazier.Get().CreateToggle();
			this.bakeClearToggle.PositionScale_X = 1f;
			this.bakeClearToggle.PositionScale_Y = 1f;
			this.bakeClearToggle.SizeOffset_X = 40f;
			this.bakeClearToggle.PositionOffset_X = -200f;
			this.bakeClearToggle.SizeOffset_Y = 40f;
			num -= this.bakeClearToggle.SizeOffset_Y;
			this.bakeClearToggle.PositionOffset_Y = num;
			num -= 10f;
			this.bakeClearToggle.AddLabel(this.localization.format("Bake_Clear"), 1);
			this.bakeClearToggle.Value = DevkitFoliageToolOptions.instance.bakeClear;
			this.bakeClearToggle.OnValueChanged += new Toggled(this.OnBakeClearClicked);
			base.AddChild(this.bakeClearToggle);
			this.bakeApplyScaleToggle = Glazier.Get().CreateToggle();
			this.bakeApplyScaleToggle.PositionScale_X = 1f;
			this.bakeApplyScaleToggle.PositionScale_Y = 1f;
			this.bakeApplyScaleToggle.SizeOffset_X = 40f;
			this.bakeApplyScaleToggle.PositionOffset_X = -200f;
			this.bakeApplyScaleToggle.SizeOffset_Y = 40f;
			num -= this.bakeApplyScaleToggle.SizeOffset_Y;
			this.bakeApplyScaleToggle.PositionOffset_Y = num;
			num -= 10f;
			this.bakeApplyScaleToggle.AddLabel(this.localization.format("Bake_ApplyScale"), 1);
			this.bakeApplyScaleToggle.Value = DevkitFoliageToolOptions.instance.bakeApplyScale;
			this.bakeApplyScaleToggle.OnValueChanged += new Toggled(this.OnBakeApplyScaleClicked);
			base.AddChild(this.bakeApplyScaleToggle);
			this.bakeObjectsToggle = Glazier.Get().CreateToggle();
			this.bakeObjectsToggle.PositionScale_X = 1f;
			this.bakeObjectsToggle.PositionScale_Y = 1f;
			this.bakeObjectsToggle.SizeOffset_X = 40f;
			this.bakeObjectsToggle.PositionOffset_X = -200f;
			this.bakeObjectsToggle.SizeOffset_Y = 40f;
			num -= this.bakeObjectsToggle.SizeOffset_Y;
			this.bakeObjectsToggle.PositionOffset_Y = num;
			num -= 10f;
			this.bakeObjectsToggle.AddLabel(this.localization.format("Bake_Objects"), 1);
			this.bakeObjectsToggle.Value = DevkitFoliageToolOptions.instance.bakeObjects;
			this.bakeObjectsToggle.OnValueChanged += new Toggled(this.OnBakeObjectsClicked);
			base.AddChild(this.bakeObjectsToggle);
			this.bakeResourcesToggle = Glazier.Get().CreateToggle();
			this.bakeResourcesToggle.PositionScale_X = 1f;
			this.bakeResourcesToggle.PositionScale_Y = 1f;
			this.bakeResourcesToggle.SizeOffset_X = 40f;
			this.bakeResourcesToggle.PositionOffset_X = -200f;
			this.bakeResourcesToggle.SizeOffset_Y = 40f;
			num -= this.bakeResourcesToggle.SizeOffset_Y;
			this.bakeResourcesToggle.PositionOffset_Y = num;
			num -= 10f;
			this.bakeResourcesToggle.AddLabel(this.localization.format("Bake_Resources"), 1);
			this.bakeResourcesToggle.Value = DevkitFoliageToolOptions.instance.bakeResources;
			this.bakeResourcesToggle.OnValueChanged += new Toggled(this.OnBakeResourcesClicked);
			base.AddChild(this.bakeResourcesToggle);
			this.bakeInstancedMeshesToggle = Glazier.Get().CreateToggle();
			this.bakeInstancedMeshesToggle.PositionScale_X = 1f;
			this.bakeInstancedMeshesToggle.PositionScale_Y = 1f;
			this.bakeInstancedMeshesToggle.SizeOffset_X = 40f;
			this.bakeInstancedMeshesToggle.PositionOffset_X = -200f;
			this.bakeInstancedMeshesToggle.SizeOffset_Y = 40f;
			num -= this.bakeInstancedMeshesToggle.SizeOffset_Y;
			this.bakeInstancedMeshesToggle.PositionOffset_Y = num;
			num -= 10f;
			this.bakeInstancedMeshesToggle.AddLabel(this.localization.format("Bake_InstancedMeshes"), 1);
			this.bakeInstancedMeshesToggle.Value = DevkitFoliageToolOptions.instance.bakeInstancedMeshes;
			this.bakeInstancedMeshesToggle.OnValueChanged += new Toggled(this.OnBakeInstancedMeshesClicked);
			base.AddChild(this.bakeInstancedMeshesToggle);
			this.bakeProgressLabel = Glazier.Get().CreateLabel();
			this.bakeProgressLabel.PositionOffset_X = -100f;
			this.bakeProgressLabel.PositionScale_X = 0.5f;
			this.bakeProgressLabel.PositionScale_Y = 0.9f;
			this.bakeProgressLabel.SizeOffset_X = 200f;
			this.bakeProgressLabel.SizeOffset_Y = 30f;
			this.bakeProgressLabel.TextContrastContext = 2;
			this.bakeProgressLabel.IsVisible = false;
			base.AddChild(this.bakeProgressLabel);
			this.hintLabel = Glazier.Get().CreateLabel();
			this.hintLabel.PositionScale_Y = 1f;
			this.hintLabel.PositionOffset_Y = -30f;
			this.hintLabel.SizeScale_X = 1f;
			this.hintLabel.SizeOffset_Y = 30f;
			this.hintLabel.TextContrastContext = 2;
			this.hintLabel.IsVisible = false;
			base.AddChild(this.hintLabel);
			this.selectedAssetBox = Glazier.Get().CreateBox();
			this.selectedAssetBox.PositionScale_X = 1f;
			this.selectedAssetBox.SizeOffset_X = 200f;
			this.selectedAssetBox.PositionOffset_X = -this.selectedAssetBox.SizeOffset_X;
			this.selectedAssetBox.SizeOffset_Y = 30f;
			this.selectedAssetBox.AddLabel(this.localization.format("SelectedAsset", "Alt"), 0);
			base.AddChild(this.selectedAssetBox);
			this.searchTypeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(this.localization.format("SearchType_Assets")),
				new GUIContent(this.localization.format("SearchType_Collections"))
			});
			this.searchTypeButton.PositionScale_X = 1f;
			this.searchTypeButton.SizeOffset_X = 200f;
			this.searchTypeButton.PositionOffset_X = -this.searchTypeButton.SizeOffset_X;
			this.searchTypeButton.PositionOffset_Y = 40f;
			this.searchTypeButton.SizeOffset_Y = 30f;
			SleekButtonState sleekButtonState2 = this.searchTypeButton;
			sleekButtonState2.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState2.onSwappedState, new SwappedState(this.OnSwappedSearchType));
			this.searchTypeButton.AddLabel(this.localization.format("SearchType_Label"), 0);
			base.AddChild(this.searchTypeButton);
			this.searchField = Glazier.Get().CreateStringField();
			this.searchField.PositionOffset_X = -200f;
			this.searchField.PositionOffset_Y = 80f;
			this.searchField.PositionScale_X = 1f;
			this.searchField.SizeOffset_X = 200f;
			this.searchField.SizeOffset_Y = 30f;
			this.searchField.PlaceholderText = this.localization.format("SearchHint");
			this.searchField.OnTextSubmitted += new Entered(this.OnNameFilterEntered);
			base.AddChild(this.searchField);
			this.assetScrollView = Glazier.Get().CreateScrollView();
			this.assetScrollView.PositionScale_X = 1f;
			this.assetScrollView.SizeOffset_X = 200f;
			this.assetScrollView.PositionOffset_X = -this.assetScrollView.SizeOffset_X;
			this.assetScrollView.PositionOffset_Y = 120f;
			this.assetScrollView.SizeOffset_Y = -120f;
			this.assetScrollView.SizeScale_Y = 1f;
			this.assetScrollView.ScaleContentToWidth = true;
			base.AddChild(this.assetScrollView);
			this.RefreshAssets();
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x0013D9AC File Offset: 0x0013BBAC
		private void UpdateOffsets()
		{
			this.selectedAssetBox.IsVisible = (this.tool.mode != FoliageEditor.EFoliageMode.BAKE);
			this.searchTypeButton.IsVisible = this.selectedAssetBox.IsVisible;
			this.searchField.IsVisible = this.selectedAssetBox.IsVisible;
			this.assetScrollView.IsVisible = this.selectedAssetBox.IsVisible;
			this.bakeInstancedMeshesToggle.IsVisible = (this.tool.mode == FoliageEditor.EFoliageMode.BAKE);
			this.bakeResourcesToggle.IsVisible = this.bakeInstancedMeshesToggle.IsVisible;
			this.bakeObjectsToggle.IsVisible = this.bakeInstancedMeshesToggle.IsVisible;
			this.bakeApplyScaleToggle.IsVisible = this.bakeInstancedMeshesToggle.IsVisible;
			this.bakeClearToggle.IsVisible = this.bakeInstancedMeshesToggle.IsVisible;
			this.bakeGlobalButton.IsVisible = this.bakeInstancedMeshesToggle.IsVisible;
			this.bakeCancelButton.IsVisible = this.bakeInstancedMeshesToggle.IsVisible;
			this.bakeLocalButton.IsVisible = this.bakeInstancedMeshesToggle.IsVisible;
			float num = 0f;
			num -= this.modeButton.SizeOffset_Y;
			this.modeButton.PositionOffset_Y = num;
			num -= 10f;
			this.maxPreviewSamplesField.IsVisible = (this.tool.mode == FoliageEditor.EFoliageMode.PAINT);
			if (this.maxPreviewSamplesField.IsVisible)
			{
				num -= this.maxPreviewSamplesField.SizeOffset_Y;
				this.maxPreviewSamplesField.PositionOffset_Y = num;
				num -= 10f;
			}
			this.surfaceMaskField.IsVisible = (this.tool.mode != FoliageEditor.EFoliageMode.BAKE);
			if (this.surfaceMaskField.IsVisible)
			{
				num -= this.surfaceMaskField.SizeOffset_Y;
				this.surfaceMaskField.PositionOffset_Y = num;
				num -= 10f;
			}
			this.densityTargetField.IsVisible = (this.tool.mode == FoliageEditor.EFoliageMode.PAINT);
			this.brushStrengthField.IsVisible = this.densityTargetField.IsVisible;
			this.brushFalloffField.IsVisible = this.densityTargetField.IsVisible;
			this.brushRadiusField.IsVisible = this.densityTargetField.IsVisible;
			if (this.densityTargetField.IsVisible)
			{
				num -= this.densityTargetField.SizeOffset_Y;
				this.densityTargetField.PositionOffset_Y = num;
				num -= 10f;
				num -= this.brushStrengthField.SizeOffset_Y;
				this.brushStrengthField.PositionOffset_Y = num;
				num -= 10f;
				num -= this.brushFalloffField.SizeOffset_Y;
				this.brushFalloffField.PositionOffset_Y = num;
				num -= 10f;
				num -= this.brushRadiusField.SizeOffset_Y;
				this.brushRadiusField.PositionOffset_Y = num;
				num -= 10f;
			}
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x0013DC78 File Offset: 0x0013BE78
		private void OnSwappedMode(SleekButtonState element, int index)
		{
			this.tool.mode = (FoliageEditor.EFoliageMode)index;
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x0013DC86 File Offset: 0x0013BE86
		private void OnMaxPreviewSamplesTyped(ISleekUInt32Field field, uint state)
		{
			DevkitFoliageToolOptions.instance.maxPreviewSamples = state;
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x0013DC93 File Offset: 0x0013BE93
		private void OnSurfaceMaskTyped(ISleekUInt32Field field, uint state)
		{
			DevkitFoliageToolOptions.instance.surfaceMask = (ERayMask)state;
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x0013DCA0 File Offset: 0x0013BEA0
		private void OnDensityTargetTyped(ISleekFloat32Field field, float state)
		{
			DevkitFoliageToolOptions.instance.densityTarget = state;
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x0013DCAD File Offset: 0x0013BEAD
		private void OnBrushStrengthTyped(ISleekFloat32Field field, float state)
		{
			DevkitFoliageToolOptions.instance.brushStrength = state;
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x0013DCBA File Offset: 0x0013BEBA
		private void OnBrushFalloffTyped(ISleekFloat32Field field, float state)
		{
			DevkitFoliageToolOptions.instance.brushFalloff = state;
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x0013DCC7 File Offset: 0x0013BEC7
		private void OnBrushRadiusTyped(ISleekFloat32Field field, float state)
		{
			DevkitFoliageToolOptions.instance.brushRadius = state;
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x0013DCD4 File Offset: 0x0013BED4
		private void OnBakeInstancedMeshesClicked(ISleekToggle element, bool state)
		{
			DevkitFoliageToolOptions.instance.bakeInstancedMeshes = state;
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x0013DCE1 File Offset: 0x0013BEE1
		private void OnBakeResourcesClicked(ISleekToggle element, bool state)
		{
			DevkitFoliageToolOptions.instance.bakeResources = state;
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x0013DCEE File Offset: 0x0013BEEE
		private void OnBakeObjectsClicked(ISleekToggle element, bool state)
		{
			DevkitFoliageToolOptions.instance.bakeObjects = state;
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x0013DCFB File Offset: 0x0013BEFB
		private void OnBakeClearClicked(ISleekToggle element, bool state)
		{
			DevkitFoliageToolOptions.instance.bakeClear = state;
		}

		// Token: 0x06003F51 RID: 16209 RVA: 0x0013DD08 File Offset: 0x0013BF08
		private void OnBakeApplyScaleClicked(ISleekToggle element, bool state)
		{
			DevkitFoliageToolOptions.instance.bakeApplyScale = state;
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x0013DD18 File Offset: 0x0013BF18
		private void RefreshAssets()
		{
			this.searchInfoAssets.Clear();
			this.searchCollectionAssets.Clear();
			this.assetScrollView.RemoveAllChildren();
			float num = 0f;
			if (this.searchTypeButton.state == 0)
			{
				Assets.find<FoliageInfoAsset>(this.searchInfoAssets);
				string searchText = this.searchField.Text;
				if (!string.IsNullOrEmpty(searchText))
				{
					ListEx.RemoveSwap<FoliageInfoAsset>(this.searchInfoAssets, (FoliageInfoAsset asset) => asset.name.IndexOf(searchText, 1) == -1);
				}
				this.searchInfoAssets.Sort((FoliageInfoAsset lhs, FoliageInfoAsset rhs) => lhs.name.CompareTo(rhs.name));
				using (List<FoliageInfoAsset>.Enumerator enumerator = this.searchInfoAssets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FoliageInfoAsset foliageInfoAsset = enumerator.Current;
						ISleekButton sleekButton = Glazier.Get().CreateButton();
						sleekButton.PositionOffset_Y = num;
						sleekButton.SizeScale_X = 1f;
						sleekButton.SizeOffset_Y = 30f;
						sleekButton.Text = foliageInfoAsset.name;
						sleekButton.OnClicked += new ClickedButton(this.OnInfoAssetClicked);
						this.assetScrollView.AddChild(sleekButton);
						num += sleekButton.SizeOffset_Y;
					}
					goto IL_256;
				}
			}
			if (this.searchTypeButton.state == 1)
			{
				Assets.find<FoliageInfoCollectionAsset>(this.searchCollectionAssets);
				string searchText = this.searchField.Text;
				if (!string.IsNullOrEmpty(searchText))
				{
					ListEx.RemoveSwap<FoliageInfoCollectionAsset>(this.searchCollectionAssets, (FoliageInfoCollectionAsset asset) => asset.name.IndexOf(searchText, 1) == -1);
				}
				this.searchCollectionAssets.Sort((FoliageInfoCollectionAsset lhs, FoliageInfoCollectionAsset rhs) => lhs.name.CompareTo(rhs.name));
				foreach (FoliageInfoCollectionAsset foliageInfoCollectionAsset in this.searchCollectionAssets)
				{
					ISleekButton sleekButton2 = Glazier.Get().CreateButton();
					sleekButton2.PositionOffset_Y = num;
					sleekButton2.SizeScale_X = 1f;
					sleekButton2.SizeOffset_Y = 30f;
					sleekButton2.Text = foliageInfoCollectionAsset.name;
					sleekButton2.OnClicked += new ClickedButton(this.OnCollectionAssetClicked);
					this.assetScrollView.AddChild(sleekButton2);
					num += sleekButton2.SizeOffset_Y;
				}
			}
			IL_256:
			this.assetScrollView.ContentSizeOffset = new Vector2(0f, num);
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x0013DFB0 File Offset: 0x0013C1B0
		private void OnSwappedSearchType(SleekButtonState element, int index)
		{
			this.RefreshAssets();
		}

		// Token: 0x06003F54 RID: 16212 RVA: 0x0013DFB8 File Offset: 0x0013C1B8
		private void OnNameFilterEntered(ISleekField field)
		{
			this.RefreshAssets();
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x0013DFC0 File Offset: 0x0013C1C0
		private void OnInfoAssetClicked(ISleekElement button)
		{
			int num = this.assetScrollView.FindIndexOfChild(button);
			this.tool.selectedInstanceAsset = this.searchInfoAssets[num];
			this.tool.selectedCollectionAsset = null;
			ISleekLabel sleekLabel = this.selectedAssetBox;
			FoliageInfoAsset selectedInstanceAsset = this.tool.selectedInstanceAsset;
			sleekLabel.Text = ((selectedInstanceAsset != null) ? selectedInstanceAsset.name : null);
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x0013E020 File Offset: 0x0013C220
		private void OnCollectionAssetClicked(ISleekElement button)
		{
			int num = this.assetScrollView.FindIndexOfChild(button);
			this.tool.selectedInstanceAsset = null;
			this.tool.selectedCollectionAsset = this.searchCollectionAssets[num];
			ISleekLabel sleekLabel = this.selectedAssetBox;
			FoliageInfoCollectionAsset selectedCollectionAsset = this.tool.selectedCollectionAsset;
			sleekLabel.Text = ((selectedCollectionAsset != null) ? selectedCollectionAsset.name : null);
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x0013E080 File Offset: 0x0013C280
		private FoliageBakeSettings getBakeSettings()
		{
			return new FoliageBakeSettings
			{
				bakeInstancesMeshes = DevkitFoliageToolOptions.instance.bakeInstancedMeshes,
				bakeResources = DevkitFoliageToolOptions.instance.bakeResources,
				bakeObjects = DevkitFoliageToolOptions.instance.bakeObjects,
				bakeClear = DevkitFoliageToolOptions.instance.bakeClear,
				bakeApplyScale = DevkitFoliageToolOptions.instance.bakeApplyScale
			};
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x0013E0EB File Offset: 0x0013C2EB
		private void OnBakeGlobalButtonClicked(ISleekElement button)
		{
			FoliageSystem.bakeGlobal(this.getBakeSettings());
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x0013E0F8 File Offset: 0x0013C2F8
		private void OnBakeLocalButtonClicked(ISleekElement button)
		{
			FoliageSystem.bakeLocal(this.getBakeSettings());
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x0013E105 File Offset: 0x0013C305
		private void OnBakeCancelButtonClicked(ISleekElement button)
		{
			FoliageSystem.bakeCancel();
		}

		// Token: 0x0400281D RID: 10269
		private Local localization;

		// Token: 0x0400281E RID: 10270
		private FoliageEditor tool;

		// Token: 0x0400281F RID: 10271
		private List<FoliageInfoAsset> searchInfoAssets;

		// Token: 0x04002820 RID: 10272
		private List<FoliageInfoCollectionAsset> searchCollectionAssets;

		// Token: 0x04002821 RID: 10273
		private ISleekBox selectedAssetBox;

		// Token: 0x04002822 RID: 10274
		private SleekButtonState searchTypeButton;

		// Token: 0x04002823 RID: 10275
		private ISleekField searchField;

		// Token: 0x04002824 RID: 10276
		private ISleekScrollView assetScrollView;

		// Token: 0x04002825 RID: 10277
		private SleekButtonState modeButton;

		// Token: 0x04002826 RID: 10278
		private ISleekFloat32Field brushRadiusField;

		// Token: 0x04002827 RID: 10279
		private ISleekFloat32Field brushFalloffField;

		// Token: 0x04002828 RID: 10280
		private ISleekFloat32Field brushStrengthField;

		// Token: 0x04002829 RID: 10281
		private ISleekFloat32Field densityTargetField;

		// Token: 0x0400282A RID: 10282
		private ISleekUInt32Field surfaceMaskField;

		// Token: 0x0400282B RID: 10283
		private ISleekUInt32Field maxPreviewSamplesField;

		// Token: 0x0400282C RID: 10284
		private ISleekToggle bakeInstancedMeshesToggle;

		// Token: 0x0400282D RID: 10285
		private ISleekToggle bakeResourcesToggle;

		// Token: 0x0400282E RID: 10286
		private ISleekToggle bakeObjectsToggle;

		// Token: 0x0400282F RID: 10287
		private ISleekToggle bakeClearToggle;

		// Token: 0x04002830 RID: 10288
		private ISleekToggle bakeApplyScaleToggle;

		// Token: 0x04002831 RID: 10289
		private ISleekButton bakeGlobalButton;

		// Token: 0x04002832 RID: 10290
		private ISleekButton bakeLocalButton;

		// Token: 0x04002833 RID: 10291
		private ISleekButton bakeCancelButton;

		// Token: 0x04002834 RID: 10292
		private ISleekLabel bakeProgressLabel;

		// Token: 0x04002835 RID: 10293
		private ISleekLabel hintLabel;
	}
}
