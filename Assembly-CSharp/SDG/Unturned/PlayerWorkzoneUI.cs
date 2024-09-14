using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007CF RID: 1999
	public class PlayerWorkzoneUI
	{
		// Token: 0x06004406 RID: 17414 RVA: 0x00186401 File Offset: 0x00184601
		public static void open()
		{
			if (PlayerWorkzoneUI.active)
			{
				return;
			}
			PlayerWorkzoneUI.active = true;
			Player.player.workzone.isBuilding = true;
			PlayerWorkzoneUI.container.AnimateIntoView();
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x0018642B File Offset: 0x0018462B
		public static void close()
		{
			if (!PlayerWorkzoneUI.active)
			{
				return;
			}
			PlayerWorkzoneUI.active = false;
			Player.player.workzone.isBuilding = false;
			PlayerWorkzoneUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x00186460 File Offset: 0x00184660
		private static void onDragStarted(Vector2 minViewportPoint, Vector2 maxViewportPoint)
		{
			Vector2 vector = PlayerUI.window.ViewportToNormalizedPosition(minViewportPoint);
			Vector2 vector2 = PlayerUI.window.ViewportToNormalizedPosition(maxViewportPoint);
			if (vector2.y < vector.y)
			{
				float y = vector2.y;
				vector2.y = vector.y;
				vector.y = y;
			}
			PlayerWorkzoneUI.dragBox.PositionScale_X = vector.x;
			PlayerWorkzoneUI.dragBox.PositionScale_Y = vector.y;
			PlayerWorkzoneUI.dragBox.SizeScale_X = vector2.x - vector.x;
			PlayerWorkzoneUI.dragBox.SizeScale_Y = vector2.y - vector.y;
			PlayerWorkzoneUI.dragBox.IsVisible = true;
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x00186508 File Offset: 0x00184708
		private static void onDragStopped()
		{
			PlayerWorkzoneUI.dragBox.IsVisible = false;
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x00186515 File Offset: 0x00184715
		private static void onTypedSnapTransformField(ISleekFloat32Field field, float value)
		{
			Player.player.workzone.snapTransform = value;
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x00186527 File Offset: 0x00184727
		private static void onTypedSnapRotationField(ISleekFloat32Field field, float value)
		{
			Player.player.workzone.snapRotation = value;
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x00186539 File Offset: 0x00184739
		private static void onClickedTransformButton(ISleekElement button)
		{
			Player.player.workzone.dragMode = EDragMode.TRANSFORM;
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x0018654B File Offset: 0x0018474B
		private static void onClickedRotateButton(ISleekElement button)
		{
			Player.player.workzone.dragMode = EDragMode.ROTATE;
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x0018655D File Offset: 0x0018475D
		private static void onSwappedStateCoordinate(SleekButtonState button, int index)
		{
			Player.player.workzone.dragCoordinate = (EDragCoordinate)index;
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x00186570 File Offset: 0x00184770
		public PlayerWorkzoneUI()
		{
			Local local = Localization.read("/Editor/EditorLevelObjects.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevelObjects/EditorLevelObjects.unity3d");
			PlayerWorkzoneUI.container = new SleekFullscreenBox();
			PlayerWorkzoneUI.container.PositionOffset_X = 10f;
			PlayerWorkzoneUI.container.PositionOffset_Y = 10f;
			PlayerWorkzoneUI.container.PositionScale_X = 1f;
			PlayerWorkzoneUI.container.SizeOffset_X = -20f;
			PlayerWorkzoneUI.container.SizeOffset_Y = -20f;
			PlayerWorkzoneUI.container.SizeScale_X = 1f;
			PlayerWorkzoneUI.container.SizeScale_Y = 1f;
			PlayerUI.window.AddChild(PlayerWorkzoneUI.container);
			PlayerWorkzoneUI.active = false;
			Player.player.workzone.onDragStarted = new DragStarted(PlayerWorkzoneUI.onDragStarted);
			Player.player.workzone.onDragStopped = new DragStopped(PlayerWorkzoneUI.onDragStopped);
			PlayerWorkzoneUI.dragBox = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
			PlayerWorkzoneUI.dragBox.TintColor = new Color(1f, 1f, 0f, 0.2f);
			PlayerUI.window.AddChild(PlayerWorkzoneUI.dragBox);
			PlayerWorkzoneUI.dragBox.IsVisible = false;
			PlayerWorkzoneUI.snapTransformField = Glazier.Get().CreateFloat32Field();
			PlayerWorkzoneUI.snapTransformField.PositionOffset_Y = -190f;
			PlayerWorkzoneUI.snapTransformField.PositionScale_Y = 1f;
			PlayerWorkzoneUI.snapTransformField.SizeOffset_X = 200f;
			PlayerWorkzoneUI.snapTransformField.SizeOffset_Y = 30f;
			PlayerWorkzoneUI.snapTransformField.Value = Player.player.workzone.snapTransform;
			PlayerWorkzoneUI.snapTransformField.AddLabel(local.format("SnapTransformLabelText"), 1);
			PlayerWorkzoneUI.snapTransformField.OnValueChanged += new TypedSingle(PlayerWorkzoneUI.onTypedSnapTransformField);
			PlayerWorkzoneUI.container.AddChild(PlayerWorkzoneUI.snapTransformField);
			PlayerWorkzoneUI.snapRotationField = Glazier.Get().CreateFloat32Field();
			PlayerWorkzoneUI.snapRotationField.PositionOffset_Y = -150f;
			PlayerWorkzoneUI.snapRotationField.PositionScale_Y = 1f;
			PlayerWorkzoneUI.snapRotationField.SizeOffset_X = 200f;
			PlayerWorkzoneUI.snapRotationField.SizeOffset_Y = 30f;
			PlayerWorkzoneUI.snapRotationField.Value = Player.player.workzone.snapRotation;
			PlayerWorkzoneUI.snapRotationField.AddLabel(local.format("SnapRotationLabelText"), 1);
			PlayerWorkzoneUI.snapRotationField.OnValueChanged += new TypedSingle(PlayerWorkzoneUI.onTypedSnapRotationField);
			PlayerWorkzoneUI.container.AddChild(PlayerWorkzoneUI.snapRotationField);
			PlayerWorkzoneUI.transformButton = new SleekButtonIcon(bundle.load<Texture2D>("Transform"));
			PlayerWorkzoneUI.transformButton.PositionOffset_Y = -110f;
			PlayerWorkzoneUI.transformButton.PositionScale_Y = 1f;
			PlayerWorkzoneUI.transformButton.SizeOffset_X = 200f;
			PlayerWorkzoneUI.transformButton.SizeOffset_Y = 30f;
			PlayerWorkzoneUI.transformButton.text = local.format("TransformButtonText", ControlsSettings.tool_0);
			PlayerWorkzoneUI.transformButton.tooltip = local.format("TransformButtonTooltip");
			PlayerWorkzoneUI.transformButton.onClickedButton += new ClickedButton(PlayerWorkzoneUI.onClickedTransformButton);
			PlayerWorkzoneUI.container.AddChild(PlayerWorkzoneUI.transformButton);
			PlayerWorkzoneUI.rotateButton = new SleekButtonIcon(bundle.load<Texture2D>("Rotate"));
			PlayerWorkzoneUI.rotateButton.PositionOffset_Y = -70f;
			PlayerWorkzoneUI.rotateButton.PositionScale_Y = 1f;
			PlayerWorkzoneUI.rotateButton.SizeOffset_X = 200f;
			PlayerWorkzoneUI.rotateButton.SizeOffset_Y = 30f;
			PlayerWorkzoneUI.rotateButton.text = local.format("RotateButtonText", ControlsSettings.tool_1);
			PlayerWorkzoneUI.rotateButton.tooltip = local.format("RotateButtonTooltip");
			PlayerWorkzoneUI.rotateButton.onClickedButton += new ClickedButton(PlayerWorkzoneUI.onClickedRotateButton);
			PlayerWorkzoneUI.container.AddChild(PlayerWorkzoneUI.rotateButton);
			PlayerWorkzoneUI.coordinateButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(local.format("CoordinateButtonTextGlobal"), bundle.load<Texture>("Global")),
				new GUIContent(local.format("CoordinateButtonTextLocal"), bundle.load<Texture>("Local"))
			});
			PlayerWorkzoneUI.coordinateButton.PositionOffset_Y = -30f;
			PlayerWorkzoneUI.coordinateButton.PositionScale_Y = 1f;
			PlayerWorkzoneUI.coordinateButton.SizeOffset_X = 200f;
			PlayerWorkzoneUI.coordinateButton.SizeOffset_Y = 30f;
			PlayerWorkzoneUI.coordinateButton.tooltip = local.format("CoordinateButtonTooltip");
			PlayerWorkzoneUI.coordinateButton.onSwappedState = new SwappedState(PlayerWorkzoneUI.onSwappedStateCoordinate);
			PlayerWorkzoneUI.container.AddChild(PlayerWorkzoneUI.coordinateButton);
			bundle.unload();
		}

		// Token: 0x04002D79 RID: 11641
		private static SleekFullscreenBox container;

		// Token: 0x04002D7A RID: 11642
		public static bool active;

		// Token: 0x04002D7B RID: 11643
		private static ISleekImage dragBox;

		// Token: 0x04002D7C RID: 11644
		private static ISleekFloat32Field snapTransformField;

		// Token: 0x04002D7D RID: 11645
		private static ISleekFloat32Field snapRotationField;

		// Token: 0x04002D7E RID: 11646
		private static SleekButtonIcon transformButton;

		// Token: 0x04002D7F RID: 11647
		private static SleekButtonIcon rotateButton;

		// Token: 0x04002D80 RID: 11648
		public static SleekButtonState coordinateButton;
	}
}
