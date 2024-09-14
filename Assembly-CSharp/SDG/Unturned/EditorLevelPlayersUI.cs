using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200077E RID: 1918
	public class EditorLevelPlayersUI
	{
		// Token: 0x06003F01 RID: 16129 RVA: 0x00138E95 File Offset: 0x00137095
		public static void open()
		{
			if (EditorLevelPlayersUI.active)
			{
				return;
			}
			EditorLevelPlayersUI.active = true;
			EditorSpawns.isSpawning = true;
			EditorSpawns.spawnMode = ESpawnMode.ADD_PLAYER;
			EditorLevelPlayersUI.container.AnimateIntoView();
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x00138EBB File Offset: 0x001370BB
		public static void close()
		{
			if (!EditorLevelPlayersUI.active)
			{
				return;
			}
			EditorLevelPlayersUI.active = false;
			EditorSpawns.isSpawning = false;
			EditorLevelPlayersUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x00138EE5 File Offset: 0x001370E5
		private static void onToggledAltToggle(ISleekToggle toggle, bool state)
		{
			EditorSpawns.selectedAlt = state;
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x00138EED File Offset: 0x001370ED
		private static void onDraggedRadiusSlider(ISleekSlider slider, float state)
		{
			EditorSpawns.radius = (byte)((float)EditorSpawns.MIN_REMOVE_SIZE + state * (float)EditorSpawns.MAX_REMOVE_SIZE);
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x00138F04 File Offset: 0x00137104
		private static void onDraggedRotationSlider(ISleekSlider slider, float state)
		{
			EditorSpawns.rotation = state * 360f;
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x00138F12 File Offset: 0x00137112
		private static void onClickedAddButton(ISleekElement button)
		{
			EditorSpawns.spawnMode = ESpawnMode.ADD_PLAYER;
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x00138F1A File Offset: 0x0013711A
		private static void onClickedRemoveButton(ISleekElement button)
		{
			EditorSpawns.spawnMode = ESpawnMode.REMOVE_PLAYER;
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x00138F24 File Offset: 0x00137124
		public EditorLevelPlayersUI()
		{
			Local local = Localization.read("/Editor/EditorLevelPlayers.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevelPlayers/EditorLevelPlayers.unity3d");
			EditorLevelPlayersUI.container = new SleekFullscreenBox();
			EditorLevelPlayersUI.container.PositionOffset_X = 10f;
			EditorLevelPlayersUI.container.PositionOffset_Y = 10f;
			EditorLevelPlayersUI.container.PositionScale_X = 1f;
			EditorLevelPlayersUI.container.SizeOffset_X = -20f;
			EditorLevelPlayersUI.container.SizeOffset_Y = -20f;
			EditorLevelPlayersUI.container.SizeScale_X = 1f;
			EditorLevelPlayersUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorLevelPlayersUI.container);
			EditorLevelPlayersUI.active = false;
			EditorLevelPlayersUI.altToggle = Glazier.Get().CreateToggle();
			EditorLevelPlayersUI.altToggle.PositionOffset_Y = -180f;
			EditorLevelPlayersUI.altToggle.PositionScale_Y = 1f;
			EditorLevelPlayersUI.altToggle.SizeOffset_X = 40f;
			EditorLevelPlayersUI.altToggle.SizeOffset_Y = 40f;
			EditorLevelPlayersUI.altToggle.Value = EditorSpawns.selectedAlt;
			EditorLevelPlayersUI.altToggle.AddLabel(local.format("AltLabelText"), 1);
			EditorLevelPlayersUI.altToggle.OnValueChanged += new Toggled(EditorLevelPlayersUI.onToggledAltToggle);
			EditorLevelPlayersUI.container.AddChild(EditorLevelPlayersUI.altToggle);
			EditorLevelPlayersUI.radiusSlider = Glazier.Get().CreateSlider();
			EditorLevelPlayersUI.radiusSlider.PositionOffset_Y = -130f;
			EditorLevelPlayersUI.radiusSlider.PositionScale_Y = 1f;
			EditorLevelPlayersUI.radiusSlider.SizeOffset_X = 200f;
			EditorLevelPlayersUI.radiusSlider.SizeOffset_Y = 20f;
			EditorLevelPlayersUI.radiusSlider.Value = (float)(EditorSpawns.radius - EditorSpawns.MIN_REMOVE_SIZE) / (float)EditorSpawns.MAX_REMOVE_SIZE;
			EditorLevelPlayersUI.radiusSlider.Orientation = 0;
			EditorLevelPlayersUI.radiusSlider.AddLabel(local.format("RadiusSliderLabelText"), 1);
			EditorLevelPlayersUI.radiusSlider.OnValueChanged += new Dragged(EditorLevelPlayersUI.onDraggedRadiusSlider);
			EditorLevelPlayersUI.container.AddChild(EditorLevelPlayersUI.radiusSlider);
			EditorLevelPlayersUI.rotationSlider = Glazier.Get().CreateSlider();
			EditorLevelPlayersUI.rotationSlider.PositionOffset_Y = -100f;
			EditorLevelPlayersUI.rotationSlider.PositionScale_Y = 1f;
			EditorLevelPlayersUI.rotationSlider.SizeOffset_X = 200f;
			EditorLevelPlayersUI.rotationSlider.SizeOffset_Y = 20f;
			EditorLevelPlayersUI.rotationSlider.Value = EditorSpawns.rotation / 360f;
			EditorLevelPlayersUI.rotationSlider.Orientation = 0;
			EditorLevelPlayersUI.rotationSlider.AddLabel(local.format("RotationSliderLabelText"), 1);
			EditorLevelPlayersUI.rotationSlider.OnValueChanged += new Dragged(EditorLevelPlayersUI.onDraggedRotationSlider);
			EditorLevelPlayersUI.container.AddChild(EditorLevelPlayersUI.rotationSlider);
			EditorLevelPlayersUI.addButton = new SleekButtonIcon(bundle.load<Texture2D>("Add"));
			EditorLevelPlayersUI.addButton.PositionOffset_Y = -70f;
			EditorLevelPlayersUI.addButton.PositionScale_Y = 1f;
			EditorLevelPlayersUI.addButton.SizeOffset_X = 200f;
			EditorLevelPlayersUI.addButton.SizeOffset_Y = 30f;
			EditorLevelPlayersUI.addButton.text = local.format("AddButtonText", ControlsSettings.tool_0);
			EditorLevelPlayersUI.addButton.tooltip = local.format("AddButtonTooltip");
			EditorLevelPlayersUI.addButton.onClickedButton += new ClickedButton(EditorLevelPlayersUI.onClickedAddButton);
			EditorLevelPlayersUI.container.AddChild(EditorLevelPlayersUI.addButton);
			EditorLevelPlayersUI.removeButton = new SleekButtonIcon(bundle.load<Texture2D>("Remove"));
			EditorLevelPlayersUI.removeButton.PositionOffset_Y = -30f;
			EditorLevelPlayersUI.removeButton.PositionScale_Y = 1f;
			EditorLevelPlayersUI.removeButton.SizeOffset_X = 200f;
			EditorLevelPlayersUI.removeButton.SizeOffset_Y = 30f;
			EditorLevelPlayersUI.removeButton.text = local.format("RemoveButtonText", ControlsSettings.tool_1);
			EditorLevelPlayersUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
			EditorLevelPlayersUI.removeButton.onClickedButton += new ClickedButton(EditorLevelPlayersUI.onClickedRemoveButton);
			EditorLevelPlayersUI.container.AddChild(EditorLevelPlayersUI.removeButton);
			bundle.unload();
		}

		// Token: 0x040027DA RID: 10202
		private static SleekFullscreenBox container;

		// Token: 0x040027DB RID: 10203
		public static bool active;

		// Token: 0x040027DC RID: 10204
		private static ISleekToggle altToggle;

		// Token: 0x040027DD RID: 10205
		private static ISleekSlider radiusSlider;

		// Token: 0x040027DE RID: 10206
		private static ISleekSlider rotationSlider;

		// Token: 0x040027DF RID: 10207
		private static SleekButtonIcon addButton;

		// Token: 0x040027E0 RID: 10208
		private static SleekButtonIcon removeButton;
	}
}
