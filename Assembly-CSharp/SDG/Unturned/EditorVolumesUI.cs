using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000789 RID: 1929
	internal class EditorVolumesUI : SleekFullscreenBox
	{
		// Token: 0x06003FA4 RID: 16292 RVA: 0x0014192D File Offset: 0x0013FB2D
		public void Open()
		{
			this.SyncSettings();
			base.AnimateIntoView();
			EditorInteract.instance.SetActiveTool(this.tool);
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x0014194B File Offset: 0x0013FB4B
		public void Close()
		{
			base.AnimateOutOfView(1f, 0f);
			DevkitSelectionToolOptions.save();
			EditorInteract.instance.SetActiveTool(null);
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x00141970 File Offset: 0x0013FB70
		public override void OnUpdate()
		{
			base.OnUpdate();
			GameObject mostRecentGameObject = DevkitSelectionManager.mostRecentGameObject;
			if (this.focusedGameObject == mostRecentGameObject)
			{
				return;
			}
			if (this.focusedItemMenu != null)
			{
				base.RemoveChild(this.focusedItemMenu);
				this.focusedItemMenu = null;
			}
			this.focusedGameObject = mostRecentGameObject;
			GameObject gameObject = this.focusedGameObject;
			VolumeBase volumeBase = (gameObject != null) ? gameObject.GetComponent<VolumeBase>() : null;
			if (volumeBase != null)
			{
				this.focusedItemMenu = volumeBase.CreateMenu();
				if (this.focusedItemMenu != null)
				{
					this.focusedItemMenu.PositionOffset_Y = this.snapTransformField.PositionOffset_Y - 10f - this.focusedItemMenu.SizeOffset_Y;
					this.focusedItemMenu.PositionScale_Y = 1f;
					base.AddChild(this.focusedItemMenu);
				}
			}
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x00141A30 File Offset: 0x0013FC30
		public EditorVolumesUI()
		{
			DevkitSelectionToolOptions.load();
			this.tool = new VolumesEditor();
			this.localization = Localization.read("/Editor/EditorLevelVolumes.dat");
			Local local = Localization.read("/Editor/EditorLevelObjects.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevelObjects/EditorLevelObjects.unity3d");
			List<VolumeManagerBase> list = new List<VolumeManagerBase>(VolumeManagerBase.allManagers);
			list.Sort((VolumeManagerBase lhs, VolumeManagerBase rhs) => lhs.FriendlyName.CompareTo(rhs.FriendlyName));
			float num = 0f;
			this.surfaceMaskField = Glazier.Get().CreateUInt32Field();
			this.surfaceMaskField.PositionScale_Y = 1f;
			this.surfaceMaskField.SizeOffset_X = 200f;
			this.surfaceMaskField.SizeOffset_Y = 30f;
			num -= this.surfaceMaskField.SizeOffset_Y;
			this.surfaceMaskField.PositionOffset_Y = num;
			num -= 10f;
			this.surfaceMaskField.AddLabel("Surface Mask (sorry this is not user-friendly at the moment)", 1);
			this.surfaceMaskField.OnValueChanged += new TypedUInt32(this.OnSurfaceMaskTyped);
			base.AddChild(this.surfaceMaskField);
			this.coordinateButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(local.format("CoordinateButtonTextGlobal"), bundle.load<Texture>("Global")),
				new GUIContent(local.format("CoordinateButtonTextLocal"), bundle.load<Texture>("Local"))
			});
			this.coordinateButton.PositionScale_Y = 1f;
			this.coordinateButton.SizeOffset_X = 200f;
			this.coordinateButton.SizeOffset_Y = 30f;
			num -= this.coordinateButton.SizeOffset_Y;
			this.coordinateButton.PositionOffset_Y = num;
			num -= 10f;
			this.coordinateButton.tooltip = local.format("CoordinateButtonTooltip");
			this.coordinateButton.onSwappedState = new SwappedState(this.OnSwappedStateCoordinate);
			base.AddChild(this.coordinateButton);
			this.scaleButton = new SleekButtonIcon(bundle.load<Texture2D>("Scale"));
			this.scaleButton.PositionScale_Y = 1f;
			this.scaleButton.SizeOffset_X = 200f;
			this.scaleButton.SizeOffset_Y = 30f;
			num -= this.scaleButton.SizeOffset_Y;
			this.scaleButton.PositionOffset_Y = num;
			num -= 10f;
			this.scaleButton.text = local.format("ScaleButtonText", ControlsSettings.tool_3);
			this.scaleButton.tooltip = local.format("ScaleButtonTooltip");
			this.scaleButton.onClickedButton += new ClickedButton(this.OnScaleClicked);
			base.AddChild(this.scaleButton);
			this.rotateButton = new SleekButtonIcon(bundle.load<Texture2D>("Rotate"));
			this.rotateButton.PositionScale_Y = 1f;
			this.rotateButton.SizeOffset_X = 200f;
			this.rotateButton.SizeOffset_Y = 30f;
			num -= this.rotateButton.SizeOffset_Y;
			this.rotateButton.PositionOffset_Y = num;
			num -= 10f;
			this.rotateButton.text = local.format("RotateButtonText", ControlsSettings.tool_1);
			this.rotateButton.tooltip = local.format("RotateButtonTooltip");
			this.rotateButton.onClickedButton += new ClickedButton(this.OnRotateClicked);
			base.AddChild(this.rotateButton);
			this.transformButton = new SleekButtonIcon(bundle.load<Texture2D>("Transform"));
			this.transformButton.PositionScale_Y = 1f;
			this.transformButton.SizeOffset_X = 200f;
			this.transformButton.SizeOffset_Y = 30f;
			num -= this.transformButton.SizeOffset_Y;
			this.transformButton.PositionOffset_Y = num;
			num -= 10f;
			this.transformButton.text = local.format("TransformButtonText", ControlsSettings.tool_0);
			this.transformButton.tooltip = local.format("TransformButtonTooltip");
			this.transformButton.onClickedButton += new ClickedButton(this.OnTransformClicked);
			base.AddChild(this.transformButton);
			this.snapRotationField = Glazier.Get().CreateFloat32Field();
			this.snapRotationField.PositionScale_Y = 1f;
			this.snapRotationField.SizeOffset_X = 200f;
			this.snapRotationField.SizeOffset_Y = 30f;
			num -= this.snapRotationField.SizeOffset_Y;
			this.snapRotationField.PositionOffset_Y = num;
			num -= 10f;
			this.snapRotationField.AddLabel(local.format("SnapRotationLabelText"), 1);
			this.snapRotationField.OnValueChanged += new TypedSingle(this.OnTypedSnapRotationField);
			base.AddChild(this.snapRotationField);
			this.snapTransformField = Glazier.Get().CreateFloat32Field();
			this.snapTransformField.PositionScale_Y = 1f;
			this.snapTransformField.SizeOffset_X = 200f;
			this.snapTransformField.SizeOffset_Y = 30f;
			num -= this.snapTransformField.SizeOffset_Y;
			this.snapTransformField.PositionOffset_Y = num;
			num -= 10f;
			this.snapTransformField.AddLabel(local.format("SnapTransformLabelText"), 1);
			this.snapTransformField.OnValueChanged += new TypedSingle(this.OnTypedSnapTransformField);
			base.AddChild(this.snapTransformField);
			bundle.unload();
			this.enableUnderwaterEffectsToggle = Glazier.Get().CreateToggle();
			this.enableUnderwaterEffectsToggle.PositionOffset_X = 400f;
			this.enableUnderwaterEffectsToggle.PositionOffset_Y = -40f;
			this.enableUnderwaterEffectsToggle.PositionScale_Y = 1f;
			this.enableUnderwaterEffectsToggle.SizeOffset_X = 40f;
			this.enableUnderwaterEffectsToggle.SizeOffset_Y = 40f;
			this.enableUnderwaterEffectsToggle.AddLabel(this.localization.format("WantsUnderwaterEffects"), 1);
			this.enableUnderwaterEffectsToggle.Value = LevelLighting.EditorWantsUnderwaterEffects;
			this.enableUnderwaterEffectsToggle.IsVisible = false;
			this.enableUnderwaterEffectsToggle.OnValueChanged += new Toggled(this.OnUnderwaterEffectsToggled);
			base.AddChild(this.enableUnderwaterEffectsToggle);
			this.enableWaterSurfaceToggle = Glazier.Get().CreateToggle();
			this.enableWaterSurfaceToggle.PositionOffset_X = 400f;
			this.enableWaterSurfaceToggle.PositionOffset_Y = -90f;
			this.enableWaterSurfaceToggle.PositionScale_Y = 1f;
			this.enableWaterSurfaceToggle.SizeOffset_X = 40f;
			this.enableWaterSurfaceToggle.SizeOffset_Y = 40f;
			this.enableWaterSurfaceToggle.AddLabel(this.localization.format("WantsWaterSurface"), 1);
			this.enableWaterSurfaceToggle.Value = LevelLighting.EditorWantsWaterSurface;
			this.enableWaterSurfaceToggle.IsVisible = false;
			this.enableWaterSurfaceToggle.OnValueChanged += new Toggled(this.OnWaterSurfaceToggled);
			base.AddChild(this.enableWaterSurfaceToggle);
			this.refreshCullingVolumesButton = Glazier.Get().CreateButton();
			this.refreshCullingVolumesButton.PositionOffset_X = 400f;
			this.refreshCullingVolumesButton.PositionOffset_Y = -30f;
			this.refreshCullingVolumesButton.PositionScale_Y = 1f;
			this.refreshCullingVolumesButton.SizeOffset_X = 200f;
			this.refreshCullingVolumesButton.SizeOffset_Y = 30f;
			this.refreshCullingVolumesButton.Text = this.localization.format("RefreshCullingVolumes");
			this.refreshCullingVolumesButton.TooltipText = this.localization.format("RefreshCullingVolumes_Tooltip");
			this.refreshCullingVolumesButton.IsVisible = false;
			this.refreshCullingVolumesButton.OnClicked += new ClickedButton(this.OnRefreshCullingVolumesClicked);
			base.AddChild(this.refreshCullingVolumesButton);
			this.previewCullingToggle = Glazier.Get().CreateToggle();
			this.previewCullingToggle.PositionOffset_X = 400f;
			this.previewCullingToggle.PositionOffset_Y = -80f;
			this.previewCullingToggle.PositionScale_Y = 1f;
			this.previewCullingToggle.SizeOffset_X = 40f;
			this.previewCullingToggle.SizeOffset_Y = 40f;
			this.previewCullingToggle.AddLabel(this.localization.format("PreviewCulling"), 1);
			this.previewCullingToggle.Value = EditorVolumesUI.EditorWantsToPreviewCulling;
			this.previewCullingToggle.IsVisible = false;
			this.previewCullingToggle.OnValueChanged += new Toggled(this.OnPreviewCullingToggled);
			base.AddChild(this.previewCullingToggle);
			float num2 = 0f;
			this.selectedTypeBox = Glazier.Get().CreateBox();
			this.selectedTypeBox.PositionScale_X = 1f;
			this.selectedTypeBox.PositionOffset_Y = num2;
			this.selectedTypeBox.SizeOffset_X = 300f;
			this.selectedTypeBox.PositionOffset_X = -this.selectedTypeBox.SizeOffset_X;
			this.selectedTypeBox.SizeOffset_Y = 30f;
			this.selectedTypeBox.AddLabel(this.localization.format("SelectedType_Label"), 0);
			base.AddChild(this.selectedTypeBox);
			num2 += this.selectedTypeBox.SizeOffset_Y + 10f;
			this.activeVisibilityButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(this.localization.format("Visibility_Hidden")),
				new GUIContent(this.localization.format("Visibility_Wireframe")),
				new GUIContent(this.localization.format("Visibility_Solid"))
			});
			this.activeVisibilityButton.PositionScale_X = 1f;
			this.activeVisibilityButton.PositionOffset_Y = num2;
			this.activeVisibilityButton.SizeOffset_X = 300f;
			this.activeVisibilityButton.PositionOffset_X = -this.activeVisibilityButton.SizeOffset_X;
			this.activeVisibilityButton.SizeOffset_Y = 30f;
			this.activeVisibilityButton.AddLabel(this.localization.format("ActiveVisibility_Label"), 0);
			SleekButtonState sleekButtonState = this.activeVisibilityButton;
			sleekButtonState.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState.onSwappedState, new SwappedState(this.OnSwappedActiveVisibility));
			this.activeVisibilityButton.IsVisible = false;
			base.AddChild(this.activeVisibilityButton);
			num2 += this.selectedTypeBox.SizeOffset_Y + 10f;
			this.typeScrollView = Glazier.Get().CreateScrollView();
			this.typeScrollView.PositionScale_X = 1f;
			this.typeScrollView.SizeOffset_X = 300f;
			this.typeScrollView.PositionOffset_X = -this.typeScrollView.SizeOffset_X;
			this.typeScrollView.PositionOffset_Y = num2;
			this.typeScrollView.SizeOffset_Y = -num2;
			this.typeScrollView.SizeScale_Y = 1f;
			this.typeScrollView.ScaleContentToWidth = true;
			base.AddChild(this.typeScrollView);
			int num3 = 0;
			float num4 = 0f;
			this.volumeButtons = new VolumeTypeButton[list.Count];
			foreach (VolumeManagerBase volumeType in list)
			{
				VolumeTypeButton volumeTypeButton = new VolumeTypeButton(this, volumeType);
				volumeTypeButton.PositionOffset_Y = num4;
				volumeTypeButton.SizeScale_X = 1f;
				volumeTypeButton.SizeOffset_Y = 30f;
				this.typeScrollView.AddChild(volumeTypeButton);
				this.volumeButtons[num3] = volumeTypeButton;
				num4 += volumeTypeButton.SizeOffset_Y;
				num3++;
			}
			this.typeScrollView.ContentSizeOffset = new Vector2(0f, num4);
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x001425A8 File Offset: 0x001407A8
		internal void SetSelectedType(VolumeManagerBase type)
		{
			this.selectedTypeBox.Text = type.FriendlyName;
			this.tool.activeVolumeManager = type;
			this.activeVisibilityButton.state = (int)type.Visibility;
			this.activeVisibilityButton.IsVisible = true;
			this.enableUnderwaterEffectsToggle.IsVisible = (type is WaterVolumeManager);
			this.enableWaterSurfaceToggle.IsVisible = this.enableUnderwaterEffectsToggle.IsVisible;
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x00142619 File Offset: 0x00140819
		internal void RefreshSelectedVisibility()
		{
			if (this.tool.activeVolumeManager != null)
			{
				this.activeVisibilityButton.state = (int)this.tool.activeVolumeManager.Visibility;
			}
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x00142643 File Offset: 0x00140843
		private void OnTypedSnapTransformField(ISleekFloat32Field field, float value)
		{
			DevkitSelectionToolOptions.instance.snapPosition = value;
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x00142650 File Offset: 0x00140850
		private void OnTypedSnapRotationField(ISleekFloat32Field field, float value)
		{
			DevkitSelectionToolOptions.instance.snapRotation = value;
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0014265D File Offset: 0x0014085D
		private void OnTransformClicked(ISleekElement button)
		{
			this.tool.mode = SelectionTool.ESelectionMode.POSITION;
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x0014266B File Offset: 0x0014086B
		private void OnRotateClicked(ISleekElement button)
		{
			this.tool.mode = SelectionTool.ESelectionMode.ROTATION;
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x00142679 File Offset: 0x00140879
		private void OnScaleClicked(ISleekElement button)
		{
			this.tool.mode = SelectionTool.ESelectionMode.SCALE;
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x00142687 File Offset: 0x00140887
		private void OnSwappedStateCoordinate(SleekButtonState button, int index)
		{
			DevkitSelectionToolOptions.instance.localSpace = (index > 0);
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x00142698 File Offset: 0x00140898
		private void OnSwappedActiveVisibility(SleekButtonState button, int state)
		{
			if (this.tool.activeVolumeManager != null)
			{
				this.tool.activeVolumeManager.Visibility = (ELevelVolumeVisibility)state;
			}
			VolumeTypeButton[] array = this.volumeButtons;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RefreshVisibility();
			}
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x001426E0 File Offset: 0x001408E0
		private void OnUnderwaterEffectsToggled(ISleekToggle toggle, bool state)
		{
			LevelLighting.EditorWantsUnderwaterEffects = state;
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x001426E8 File Offset: 0x001408E8
		private void OnWaterSurfaceToggled(ISleekToggle toggle, bool state)
		{
			LevelLighting.EditorWantsWaterSurface = state;
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x001426F0 File Offset: 0x001408F0
		private void OnRefreshCullingVolumesClicked(ISleekElement button)
		{
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x001426F2 File Offset: 0x001408F2
		private void OnPreviewCullingToggled(ISleekToggle toggle, bool state)
		{
			EditorVolumesUI.EditorWantsToPreviewCulling = state;
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x001426FA File Offset: 0x001408FA
		private void OnSurfaceMaskTyped(ISleekUInt32Field field, uint state)
		{
			DevkitSelectionToolOptions.instance.selectionMask = (ERayMask)state;
		}

		/// <summary>
		/// Other menus can modify DevkitSelectionToolOptions so we need to sync our menu when opened.
		/// </summary>
		// Token: 0x06003FB6 RID: 16310 RVA: 0x00142708 File Offset: 0x00140908
		private void SyncSettings()
		{
			this.surfaceMaskField.Value = (uint)DevkitSelectionToolOptions.instance.selectionMask;
			this.coordinateButton.state = (DevkitSelectionToolOptions.instance.localSpace ? 1 : 0);
			this.snapRotationField.Value = DevkitSelectionToolOptions.instance.snapRotation;
			this.snapTransformField.Value = DevkitSelectionToolOptions.instance.snapPosition;
		}

		// Token: 0x04002876 RID: 10358
		internal Local localization;

		// Token: 0x04002877 RID: 10359
		private VolumesEditor tool;

		// Token: 0x04002878 RID: 10360
		private GameObject focusedGameObject;

		// Token: 0x04002879 RID: 10361
		private ISleekElement focusedItemMenu;

		// Token: 0x0400287A RID: 10362
		private ISleekFloat32Field snapTransformField;

		// Token: 0x0400287B RID: 10363
		private ISleekFloat32Field snapRotationField;

		// Token: 0x0400287C RID: 10364
		private SleekButtonIcon transformButton;

		// Token: 0x0400287D RID: 10365
		private SleekButtonIcon rotateButton;

		// Token: 0x0400287E RID: 10366
		private SleekButtonIcon scaleButton;

		// Token: 0x0400287F RID: 10367
		private SleekButtonState coordinateButton;

		// Token: 0x04002880 RID: 10368
		private ISleekUInt32Field surfaceMaskField;

		// Token: 0x04002881 RID: 10369
		private ISleekBox selectedTypeBox;

		// Token: 0x04002882 RID: 10370
		private SleekButtonState activeVisibilityButton;

		// Token: 0x04002883 RID: 10371
		private ISleekScrollView typeScrollView;

		// Token: 0x04002884 RID: 10372
		private VolumeTypeButton[] volumeButtons;

		// Token: 0x04002885 RID: 10373
		private ISleekToggle enableUnderwaterEffectsToggle;

		// Token: 0x04002886 RID: 10374
		private ISleekToggle enableWaterSurfaceToggle;

		// Token: 0x04002887 RID: 10375
		private ISleekButton refreshCullingVolumesButton;

		// Token: 0x04002888 RID: 10376
		private ISleekToggle previewCullingToggle;

		// Token: 0x04002889 RID: 10377
		internal static bool EditorWantsToPreviewCulling;
	}
}
