using System;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Tools;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000774 RID: 1908
	internal class EditorEnvironmentNodesUI : SleekFullscreenBox
	{
		// Token: 0x06003E67 RID: 15975 RVA: 0x00131162 File Offset: 0x0012F362
		public void Open()
		{
			this.SyncSettings();
			base.AnimateIntoView();
			EditorInteract.instance.SetActiveTool(this.tool);
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x00131180 File Offset: 0x0012F380
		public void Close()
		{
			base.AnimateOutOfView(1f, 0f);
			DevkitSelectionToolOptions.save();
			EditorInteract.instance.SetActiveTool(null);
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x001311A4 File Offset: 0x0012F3A4
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
			TempNodeBase tempNodeBase = (gameObject != null) ? gameObject.GetComponent<TempNodeBase>() : null;
			if (tempNodeBase != null)
			{
				this.focusedItemMenu = tempNodeBase.CreateMenu();
				if (this.focusedItemMenu != null)
				{
					this.focusedItemMenu.PositionOffset_Y = this.snapTransformField.PositionOffset_Y - 10f - this.focusedItemMenu.SizeOffset_Y;
					this.focusedItemMenu.PositionScale_Y = 1f;
					base.AddChild(this.focusedItemMenu);
				}
			}
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x00131264 File Offset: 0x0012F464
		public EditorEnvironmentNodesUI()
		{
			DevkitSelectionToolOptions.load();
			this.tool = new NodesEditor();
			this.localization = Localization.read("/Editor/EditorLevelNodes.dat");
			Local local = Localization.read("/Editor/EditorLevelObjects.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevelObjects/EditorLevelObjects.unity3d");
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
			float num2 = 0f;
			ISleekElement sleekElement = Glazier.Get().CreateFrame();
			sleekElement.PositionScale_X = 1f;
			sleekElement.SizeOffset_X = 200f;
			sleekElement.PositionOffset_X = -sleekElement.SizeOffset_X;
			sleekElement.SizeScale_Y = 1f;
			base.AddChild(sleekElement);
			ISleekButton sleekButton = Glazier.Get().CreateButton();
			sleekButton.PositionOffset_Y = num2;
			sleekButton.SizeScale_X = 1f;
			sleekButton.SizeOffset_Y = 30f;
			sleekButton.Text = "Airdrop Marker";
			sleekButton.OnClicked += delegate(ISleekElement button)
			{
				this.tool.activeNodeSystem = AirdropDevkitNodeSystem.Get();
			};
			sleekElement.AddChild(sleekButton);
			num2 += sleekButton.SizeOffset_Y;
			ISleekButton sleekButton2 = Glazier.Get().CreateButton();
			sleekButton2.PositionOffset_Y = num2;
			sleekButton2.SizeScale_X = 1f;
			sleekButton2.SizeOffset_Y = 30f;
			sleekButton2.Text = "Named Location";
			sleekButton2.OnClicked += delegate(ISleekElement button)
			{
				this.tool.activeNodeSystem = LocationDevkitNodeSystem.Get();
			};
			sleekElement.AddChild(sleekButton2);
			num2 += sleekButton2.SizeOffset_Y;
			ISleekButton sleekButton3 = Glazier.Get().CreateButton();
			sleekButton3.PositionOffset_Y = num2;
			sleekButton3.SizeScale_X = 1f;
			sleekButton3.SizeOffset_Y = 30f;
			sleekButton3.Text = "Spawnpoint";
			sleekButton3.OnClicked += delegate(ISleekElement button)
			{
				this.tool.activeNodeSystem = SpawnpointSystemV2.Get();
			};
			sleekElement.AddChild(sleekButton3);
			num2 += sleekButton3.SizeOffset_Y;
		}

		/// <summary>
		/// Other menus can modify DevkitSelectionToolOptions so we need to sync our menu when opened.
		/// </summary>
		// Token: 0x06003E6B RID: 15979 RVA: 0x0013190C File Offset: 0x0012FB0C
		private void SyncSettings()
		{
			this.surfaceMaskField.Value = (uint)DevkitSelectionToolOptions.instance.selectionMask;
			this.coordinateButton.state = (DevkitSelectionToolOptions.instance.localSpace ? 1 : 0);
			this.snapRotationField.Value = DevkitSelectionToolOptions.instance.snapRotation;
			this.snapTransformField.Value = DevkitSelectionToolOptions.instance.snapPosition;
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x00131973 File Offset: 0x0012FB73
		private void OnSurfaceMaskTyped(ISleekUInt32Field field, uint state)
		{
			DevkitSelectionToolOptions.instance.selectionMask = (ERayMask)state;
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x00131980 File Offset: 0x0012FB80
		private void OnTypedSnapTransformField(ISleekFloat32Field field, float value)
		{
			DevkitSelectionToolOptions.instance.snapPosition = value;
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x0013198D File Offset: 0x0012FB8D
		private void OnTypedSnapRotationField(ISleekFloat32Field field, float value)
		{
			DevkitSelectionToolOptions.instance.snapRotation = value;
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x0013199A File Offset: 0x0012FB9A
		private void OnTransformClicked(ISleekElement button)
		{
			this.tool.mode = SelectionTool.ESelectionMode.POSITION;
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x001319A8 File Offset: 0x0012FBA8
		private void OnRotateClicked(ISleekElement button)
		{
			this.tool.mode = SelectionTool.ESelectionMode.ROTATION;
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x001319B6 File Offset: 0x0012FBB6
		private void OnScaleClicked(ISleekElement button)
		{
			this.tool.mode = SelectionTool.ESelectionMode.SCALE;
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x001319C4 File Offset: 0x0012FBC4
		private void OnSwappedStateCoordinate(SleekButtonState button, int index)
		{
			DevkitSelectionToolOptions.instance.localSpace = (index > 0);
		}

		// Token: 0x04002745 RID: 10053
		internal Local localization;

		// Token: 0x04002746 RID: 10054
		private NodesEditor tool;

		// Token: 0x04002747 RID: 10055
		private GameObject focusedGameObject;

		// Token: 0x04002748 RID: 10056
		private ISleekElement focusedItemMenu;

		// Token: 0x04002749 RID: 10057
		private ISleekFloat32Field snapTransformField;

		// Token: 0x0400274A RID: 10058
		private ISleekFloat32Field snapRotationField;

		// Token: 0x0400274B RID: 10059
		private SleekButtonIcon transformButton;

		// Token: 0x0400274C RID: 10060
		private SleekButtonIcon rotateButton;

		// Token: 0x0400274D RID: 10061
		private SleekButtonIcon scaleButton;

		// Token: 0x0400274E RID: 10062
		private SleekButtonState coordinateButton;

		// Token: 0x0400274F RID: 10063
		private ISleekUInt32Field surfaceMaskField;
	}
}
