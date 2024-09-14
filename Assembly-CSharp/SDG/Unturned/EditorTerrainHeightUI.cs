using System;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Foliage;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000783 RID: 1923
	internal class EditorTerrainHeightUI : SleekFullscreenBox
	{
		// Token: 0x06003F5B RID: 16219 RVA: 0x0013E10C File Offset: 0x0013C30C
		public void Open()
		{
			base.AnimateIntoView();
			TerrainEditor.toolMode = TerrainEditor.EDevkitLandscapeToolMode.HEIGHTMAP;
			EditorInteract.instance.SetActiveTool(EditorInteract.instance.terrainTool);
			if (FoliageSystem.instance != null)
			{
				FoliageSystem.instance.hiddenByHeightEditor = true;
			}
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x0013E146 File Offset: 0x0013C346
		public void Close()
		{
			base.AnimateOutOfView(1f, 0f);
			DevkitLandscapeToolHeightmapOptions.save();
			EditorInteract.instance.SetActiveTool(null);
			if (FoliageSystem.instance != null)
			{
				FoliageSystem.instance.hiddenByHeightEditor = false;
			}
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x0013E180 File Offset: 0x0013C380
		public override void OnUpdate()
		{
			base.OnUpdate();
			this.modeButton.state = (int)TerrainEditor.heightmapMode;
			this.brushRadiusField.Value = DevkitLandscapeToolHeightmapOptions.instance.brushRadius;
			this.brushFalloffField.Value = DevkitLandscapeToolHeightmapOptions.instance.brushFalloff;
			this.brushStrengthField.Value = EditorInteract.instance.terrainTool.heightmapBrushStrength;
			this.flattenTargetField.Value = DevkitLandscapeToolHeightmapOptions.instance.flattenTarget;
			if (TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.ADJUST)
			{
				this.hintLabel.Text = this.localization.format("Hint_Adjust", "Shift");
				this.hintLabel.IsVisible = true;
			}
			else if (TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.FLATTEN)
			{
				this.hintLabel.Text = this.localization.format("Hint_Flatten", "Alt");
				this.hintLabel.IsVisible = true;
			}
			else if (TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.RAMP)
			{
				this.hintLabel.Text = this.localization.format("Hint_Ramp", "R");
				this.hintLabel.IsVisible = true;
			}
			else
			{
				this.hintLabel.IsVisible = false;
			}
			this.UpdateLowerLeftOffset();
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x0013E2B0 File Offset: 0x0013C4B0
		public EditorTerrainHeightUI()
		{
			this.localization = Localization.read("/Editor/EditorTerrainHeight.dat");
			DevkitLandscapeToolHeightmapOptions.load();
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
				new GUIContent(this.localization.format("Mode_Adjust", "Q")),
				new GUIContent(this.localization.format("Mode_Flatten", "W")),
				new GUIContent(this.localization.format("Mode_Smooth", "E")),
				new GUIContent(this.localization.format("Mode_Ramp", "R"))
			});
			this.modeButton.PositionScale_Y = 1f;
			this.modeButton.SizeOffset_X = 200f;
			this.modeButton.SizeOffset_Y = 30f;
			this.modeButton.AddLabel(this.localization.format("Mode_Label"), 1);
			this.modeButton.state = (int)TerrainEditor.heightmapMode;
			SleekButtonState sleekButtonState = this.modeButton;
			sleekButtonState.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState.onSwappedState, new SwappedState(this.OnSwappedMode));
			base.AddChild(this.modeButton);
			this.brushRadiusField = Glazier.Get().CreateFloat32Field();
			this.brushRadiusField.PositionScale_Y = 1f;
			this.brushRadiusField.SizeOffset_X = 200f;
			this.brushRadiusField.SizeOffset_Y = 30f;
			this.brushRadiusField.AddLabel(this.localization.format("BrushRadius", "B"), 1);
			this.brushRadiusField.Value = DevkitLandscapeToolHeightmapOptions.instance.brushRadius;
			this.brushRadiusField.OnValueChanged += new TypedSingle(this.OnBrushRadiusTyped);
			base.AddChild(this.brushRadiusField);
			this.brushFalloffField = Glazier.Get().CreateFloat32Field();
			this.brushFalloffField.PositionScale_Y = 1f;
			this.brushFalloffField.SizeOffset_X = 200f;
			this.brushFalloffField.SizeOffset_Y = 30f;
			this.brushFalloffField.AddLabel(this.localization.format("BrushFalloff", "F"), 1);
			this.brushFalloffField.Value = DevkitLandscapeToolHeightmapOptions.instance.brushFalloff;
			this.brushFalloffField.OnValueChanged += new TypedSingle(this.OnBrushFalloffTyped);
			base.AddChild(this.brushFalloffField);
			this.brushStrengthField = Glazier.Get().CreateFloat32Field();
			this.brushStrengthField.PositionScale_Y = 1f;
			this.brushStrengthField.SizeOffset_X = 200f;
			this.brushStrengthField.SizeOffset_Y = 30f;
			this.brushStrengthField.AddLabel(this.localization.format("BrushStrength", "V"), 1);
			this.brushStrengthField.Value = DevkitLandscapeToolHeightmapOptions.instance.brushStrength;
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
			this.smoothMethodButton.state = (int)DevkitLandscapeToolHeightmapOptions.instance.smoothMethod;
			SleekButtonState sleekButtonState2 = this.smoothMethodButton;
			sleekButtonState2.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState2.onSwappedState, new SwappedState(this.OnSwappedSmoothMethod));
			base.AddChild(this.smoothMethodButton);
			this.flattenTargetField = Glazier.Get().CreateFloat32Field();
			this.flattenTargetField.PositionScale_Y = 1f;
			this.flattenTargetField.SizeOffset_X = 200f;
			this.flattenTargetField.SizeOffset_Y = 30f;
			this.flattenTargetField.AddLabel(this.localization.format("FlattenTarget", "Alt"), 1);
			this.flattenTargetField.Value = DevkitLandscapeToolHeightmapOptions.instance.flattenTarget;
			this.flattenTargetField.OnValueChanged += new TypedSingle(this.OnFlattenTargetTyped);
			base.AddChild(this.flattenTargetField);
			this.flattenMethodButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(this.localization.format("FlattenMethod_Regular")),
				new GUIContent(this.localization.format("FlattenMethod_Min")),
				new GUIContent(this.localization.format("FlattenMethod_Max"))
			});
			this.flattenMethodButton.PositionScale_Y = 1f;
			this.flattenMethodButton.SizeOffset_X = 200f;
			this.flattenMethodButton.SizeOffset_Y = 30f;
			this.flattenMethodButton.AddLabel(this.localization.format("FlattenMethod_Label"), 1);
			this.flattenMethodButton.state = (int)DevkitLandscapeToolHeightmapOptions.instance.flattenMethod;
			SleekButtonState sleekButtonState3 = this.flattenMethodButton;
			sleekButtonState3.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState3.onSwappedState, new SwappedState(this.OnSwappedFlattenMethod));
			base.AddChild(this.flattenMethodButton);
			this.maxPreviewSamplesField = Glazier.Get().CreateUInt32Field();
			this.maxPreviewSamplesField.PositionScale_Y = 1f;
			this.maxPreviewSamplesField.SizeOffset_X = 200f;
			this.maxPreviewSamplesField.SizeOffset_Y = 30f;
			this.maxPreviewSamplesField.AddLabel(this.localization.format("MaxPreviewSamples"), 1);
			this.maxPreviewSamplesField.Value = DevkitLandscapeToolHeightmapOptions.instance.maxPreviewSamples;
			this.maxPreviewSamplesField.OnValueChanged += new TypedUInt32(this.OnMaxPreviewSamplesTyped);
			base.AddChild(this.maxPreviewSamplesField);
			this.UpdateLowerLeftOffset();
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x0013E920 File Offset: 0x0013CB20
		private void OnSwappedMode(SleekButtonState element, int index)
		{
			TerrainEditor.heightmapMode = (TerrainEditor.EDevkitLandscapeToolHeightmapMode)index;
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x0013E928 File Offset: 0x0013CB28
		private void OnBrushStrengthTyped(ISleekFloat32Field field, float state)
		{
			EditorInteract.instance.terrainTool.heightmapBrushStrength = state;
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x0013E93A File Offset: 0x0013CB3A
		private void OnBrushFalloffTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolHeightmapOptions.instance.brushFalloff = state;
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x0013E947 File Offset: 0x0013CB47
		private void OnBrushRadiusTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolHeightmapOptions.instance.brushRadius = state;
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x0013E954 File Offset: 0x0013CB54
		private void OnFlattenTargetTyped(ISleekFloat32Field field, float state)
		{
			DevkitLandscapeToolHeightmapOptions.instance.flattenTarget = state;
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x0013E961 File Offset: 0x0013CB61
		private void OnMaxPreviewSamplesTyped(ISleekUInt32Field field, uint state)
		{
			DevkitLandscapeToolHeightmapOptions.instance.maxPreviewSamples = state;
		}

		// Token: 0x06003F65 RID: 16229 RVA: 0x0013E96E File Offset: 0x0013CB6E
		private void OnSwappedSmoothMethod(SleekButtonState element, int index)
		{
			DevkitLandscapeToolHeightmapOptions.instance.smoothMethod = (EDevkitLandscapeToolHeightmapSmoothMethod)index;
		}

		// Token: 0x06003F66 RID: 16230 RVA: 0x0013E97B File Offset: 0x0013CB7B
		private void OnSwappedFlattenMethod(SleekButtonState element, int index)
		{
			DevkitLandscapeToolHeightmapOptions.instance.flattenMethod = (EDevkitLandscapeToolHeightmapFlattenMethod)index;
		}

		// Token: 0x06003F67 RID: 16231 RVA: 0x0013E988 File Offset: 0x0013CB88
		private void UpdateLowerLeftOffset()
		{
			float num = 0f;
			num -= this.modeButton.SizeOffset_Y;
			this.modeButton.PositionOffset_Y = num;
			num -= 10f;
			num -= this.maxPreviewSamplesField.SizeOffset_Y;
			this.maxPreviewSamplesField.PositionOffset_Y = num;
			num -= 10f;
			this.smoothMethodButton.IsVisible = (TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.SMOOTH);
			if (this.smoothMethodButton.IsVisible)
			{
				num -= this.smoothMethodButton.SizeOffset_Y;
				this.smoothMethodButton.PositionOffset_Y = num;
				num -= 10f;
			}
			this.flattenMethodButton.IsVisible = (TerrainEditor.heightmapMode == TerrainEditor.EDevkitLandscapeToolHeightmapMode.FLATTEN);
			this.flattenTargetField.IsVisible = this.flattenMethodButton.IsVisible;
			if (this.flattenMethodButton.IsVisible)
			{
				num -= this.flattenMethodButton.SizeOffset_Y;
				this.flattenMethodButton.PositionOffset_Y = num;
				num -= 10f;
				num -= this.flattenTargetField.SizeOffset_Y;
				this.flattenTargetField.PositionOffset_Y = num;
				num -= 10f;
			}
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

		// Token: 0x04002836 RID: 10294
		private Local localization;

		// Token: 0x04002837 RID: 10295
		private ISleekLabel hintLabel;

		// Token: 0x04002838 RID: 10296
		private SleekButtonState modeButton;

		// Token: 0x04002839 RID: 10297
		private ISleekFloat32Field brushRadiusField;

		// Token: 0x0400283A RID: 10298
		private ISleekFloat32Field brushFalloffField;

		// Token: 0x0400283B RID: 10299
		private ISleekFloat32Field brushStrengthField;

		// Token: 0x0400283C RID: 10300
		private ISleekFloat32Field flattenTargetField;

		// Token: 0x0400283D RID: 10301
		private ISleekUInt32Field maxPreviewSamplesField;

		// Token: 0x0400283E RID: 10302
		private SleekButtonState smoothMethodButton;

		// Token: 0x0400283F RID: 10303
		private SleekButtonState flattenMethodButton;
	}
}
