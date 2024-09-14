using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000775 RID: 1909
	public class EditorEnvironmentRoadsUI
	{
		// Token: 0x06003E76 RID: 15990 RVA: 0x00131A0A File Offset: 0x0012FC0A
		public static void open()
		{
			if (EditorEnvironmentRoadsUI.active)
			{
				return;
			}
			EditorEnvironmentRoadsUI.active = true;
			EditorRoads.isPaving = true;
			EditorUI.message(EEditorMessage.ROADS);
			EditorEnvironmentRoadsUI.container.AnimateIntoView();
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x00131A30 File Offset: 0x0012FC30
		public static void close()
		{
			if (!EditorEnvironmentRoadsUI.active)
			{
				return;
			}
			EditorEnvironmentRoadsUI.active = false;
			EditorRoads.isPaving = false;
			EditorEnvironmentRoadsUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x00131A5C File Offset: 0x0012FC5C
		public static void updateSelection(Road road, RoadJoint joint)
		{
			if (road != null && joint != null)
			{
				EditorEnvironmentRoadsUI.offsetField.Value = joint.offset;
				EditorEnvironmentRoadsUI.loopToggle.Value = road.isLoop;
				EditorEnvironmentRoadsUI.ignoreTerrainToggle.Value = joint.ignoreTerrain;
				EditorEnvironmentRoadsUI.modeButton.state = (int)joint.mode;
				EditorEnvironmentRoadsUI.roadIndexBox.Text = LevelRoads.getRoadIndex(road).ToString();
			}
			EditorEnvironmentRoadsUI.offsetField.IsVisible = (road != null);
			EditorEnvironmentRoadsUI.loopToggle.IsVisible = (road != null);
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.IsVisible = (road != null);
			EditorEnvironmentRoadsUI.modeButton.IsVisible = (road != null);
			EditorEnvironmentRoadsUI.roadIndexBox.IsVisible = (road != null);
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x00131B10 File Offset: 0x0012FD10
		private static void updateSelection()
		{
			if ((int)EditorRoads.selected < LevelRoads.materials.Length)
			{
				RoadMaterial roadMaterial = LevelRoads.materials[(int)EditorRoads.selected];
				EditorEnvironmentRoadsUI.selectedBox.Text = roadMaterial.material.mainTexture.name;
				EditorEnvironmentRoadsUI.widthField.Value = roadMaterial.width;
				EditorEnvironmentRoadsUI.heightField.Value = roadMaterial.height;
				EditorEnvironmentRoadsUI.depthField.Value = roadMaterial.depth;
				EditorEnvironmentRoadsUI.offset2Field.Value = roadMaterial.offset;
				EditorEnvironmentRoadsUI.concreteToggle.Value = roadMaterial.isConcrete;
			}
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x00131BA1 File Offset: 0x0012FDA1
		private static void onClickedRoadButton(ISleekElement button)
		{
			EditorRoads.selected = (byte)(button.Parent.PositionOffset_Y / 70f);
			if (EditorRoads.road != null)
			{
				EditorRoads.road.material = EditorRoads.selected;
			}
			EditorEnvironmentRoadsUI.updateSelection();
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x00131BD5 File Offset: 0x0012FDD5
		private static void onTypedWidthField(ISleekFloat32Field field, float state)
		{
			LevelRoads.materials[(int)EditorRoads.selected].width = state;
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x00131BE8 File Offset: 0x0012FDE8
		private static void onTypedHeightField(ISleekFloat32Field field, float state)
		{
			LevelRoads.materials[(int)EditorRoads.selected].height = state;
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x00131BFB File Offset: 0x0012FDFB
		private static void onTypedDepthField(ISleekFloat32Field field, float state)
		{
			LevelRoads.materials[(int)EditorRoads.selected].depth = state;
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x00131C0E File Offset: 0x0012FE0E
		private static void onTypedOffset2Field(ISleekFloat32Field field, float state)
		{
			LevelRoads.materials[(int)EditorRoads.selected].offset = state;
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x00131C21 File Offset: 0x0012FE21
		private static void onToggledConcreteToggle(ISleekToggle toggle, bool state)
		{
			LevelRoads.materials[(int)EditorRoads.selected].isConcrete = state;
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x00131C34 File Offset: 0x0012FE34
		private static void onClickedBakeRoadsButton(ISleekElement button)
		{
			LevelRoads.bakeRoads();
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x00131C3B File Offset: 0x0012FE3B
		private static void onTypedOffsetField(ISleekFloat32Field field, float state)
		{
			EditorRoads.joint.offset = state;
			EditorRoads.road.updatePoints();
		}

		// Token: 0x06003E82 RID: 16002 RVA: 0x00131C52 File Offset: 0x0012FE52
		private static void onToggledLoopToggle(ISleekToggle toggle, bool state)
		{
			EditorRoads.road.isLoop = state;
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x00131C5F File Offset: 0x0012FE5F
		private static void onToggledIgnoreTerrainToggle(ISleekToggle toggle, bool state)
		{
			EditorRoads.joint.ignoreTerrain = state;
			EditorRoads.road.updatePoints();
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x00131C76 File Offset: 0x0012FE76
		private static void onSwappedStateMode(SleekButtonState button, int index)
		{
			EditorRoads.joint.mode = (ERoadMode)index;
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x00131C84 File Offset: 0x0012FE84
		public EditorEnvironmentRoadsUI()
		{
			Local local = Localization.read("/Editor/EditorEnvironmentRoads.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorEnvironmentRoads/EditorEnvironmentRoads.unity3d");
			EditorEnvironmentRoadsUI.container = new SleekFullscreenBox();
			EditorEnvironmentRoadsUI.container.PositionOffset_X = 10f;
			EditorEnvironmentRoadsUI.container.PositionOffset_Y = 10f;
			EditorEnvironmentRoadsUI.container.PositionScale_X = 1f;
			EditorEnvironmentRoadsUI.container.SizeOffset_X = -20f;
			EditorEnvironmentRoadsUI.container.SizeOffset_Y = -20f;
			EditorEnvironmentRoadsUI.container.SizeScale_X = 1f;
			EditorEnvironmentRoadsUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorEnvironmentRoadsUI.container);
			EditorEnvironmentRoadsUI.active = false;
			EditorEnvironmentRoadsUI.roadScrollBox = Glazier.Get().CreateScrollView();
			EditorEnvironmentRoadsUI.roadScrollBox.PositionOffset_X = -400f;
			EditorEnvironmentRoadsUI.roadScrollBox.PositionOffset_Y = 120f;
			EditorEnvironmentRoadsUI.roadScrollBox.PositionScale_X = 1f;
			EditorEnvironmentRoadsUI.roadScrollBox.SizeOffset_X = 400f;
			EditorEnvironmentRoadsUI.roadScrollBox.SizeOffset_Y = -160f;
			EditorEnvironmentRoadsUI.roadScrollBox.SizeScale_Y = 1f;
			EditorEnvironmentRoadsUI.roadScrollBox.ScaleContentToWidth = true;
			EditorEnvironmentRoadsUI.roadScrollBox.ContentSizeOffset = new Vector2(0f, (float)(LevelRoads.materials.Length * 70 + 200));
			EditorEnvironmentRoadsUI.container.AddChild(EditorEnvironmentRoadsUI.roadScrollBox);
			for (int i = 0; i < LevelRoads.materials.Length; i++)
			{
				ISleekImage sleekImage = Glazier.Get().CreateImage();
				sleekImage.PositionOffset_X = 200f;
				sleekImage.PositionOffset_Y = (float)(i * 70);
				sleekImage.SizeOffset_X = 64f;
				sleekImage.SizeOffset_Y = 64f;
				sleekImage.Texture = LevelRoads.materials[i].material.mainTexture;
				EditorEnvironmentRoadsUI.roadScrollBox.AddChild(sleekImage);
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_X = 70f;
				sleekButton.SizeOffset_X = 100f;
				sleekButton.SizeOffset_Y = 64f;
				sleekButton.Text = LevelRoads.materials[i].material.mainTexture.name;
				sleekButton.OnClicked += new ClickedButton(EditorEnvironmentRoadsUI.onClickedRoadButton);
				sleekImage.AddChild(sleekButton);
			}
			EditorEnvironmentRoadsUI.widthField = Glazier.Get().CreateFloat32Field();
			EditorEnvironmentRoadsUI.widthField.PositionOffset_X = 200f;
			EditorEnvironmentRoadsUI.widthField.PositionOffset_Y = (float)(LevelRoads.materials.Length * 70);
			EditorEnvironmentRoadsUI.widthField.SizeOffset_X = 170f;
			EditorEnvironmentRoadsUI.widthField.SizeOffset_Y = 30f;
			EditorEnvironmentRoadsUI.widthField.AddLabel(local.format("WidthFieldLabelText"), 0);
			EditorEnvironmentRoadsUI.widthField.OnValueChanged += new TypedSingle(EditorEnvironmentRoadsUI.onTypedWidthField);
			EditorEnvironmentRoadsUI.roadScrollBox.AddChild(EditorEnvironmentRoadsUI.widthField);
			EditorEnvironmentRoadsUI.heightField = Glazier.Get().CreateFloat32Field();
			EditorEnvironmentRoadsUI.heightField.PositionOffset_X = 200f;
			EditorEnvironmentRoadsUI.heightField.PositionOffset_Y = (float)(LevelRoads.materials.Length * 70 + 40);
			EditorEnvironmentRoadsUI.heightField.SizeOffset_X = 170f;
			EditorEnvironmentRoadsUI.heightField.SizeOffset_Y = 30f;
			EditorEnvironmentRoadsUI.heightField.AddLabel(local.format("HeightFieldLabelText"), 0);
			EditorEnvironmentRoadsUI.heightField.OnValueChanged += new TypedSingle(EditorEnvironmentRoadsUI.onTypedHeightField);
			EditorEnvironmentRoadsUI.roadScrollBox.AddChild(EditorEnvironmentRoadsUI.heightField);
			EditorEnvironmentRoadsUI.depthField = Glazier.Get().CreateFloat32Field();
			EditorEnvironmentRoadsUI.depthField.PositionOffset_X = 200f;
			EditorEnvironmentRoadsUI.depthField.PositionOffset_Y = (float)(LevelRoads.materials.Length * 70 + 80);
			EditorEnvironmentRoadsUI.depthField.SizeOffset_X = 170f;
			EditorEnvironmentRoadsUI.depthField.SizeOffset_Y = 30f;
			EditorEnvironmentRoadsUI.depthField.AddLabel(local.format("DepthFieldLabelText"), 0);
			EditorEnvironmentRoadsUI.depthField.OnValueChanged += new TypedSingle(EditorEnvironmentRoadsUI.onTypedDepthField);
			EditorEnvironmentRoadsUI.roadScrollBox.AddChild(EditorEnvironmentRoadsUI.depthField);
			EditorEnvironmentRoadsUI.offset2Field = Glazier.Get().CreateFloat32Field();
			EditorEnvironmentRoadsUI.offset2Field.PositionOffset_X = 200f;
			EditorEnvironmentRoadsUI.offset2Field.PositionOffset_Y = (float)(LevelRoads.materials.Length * 70 + 120);
			EditorEnvironmentRoadsUI.offset2Field.SizeOffset_X = 170f;
			EditorEnvironmentRoadsUI.offset2Field.SizeOffset_Y = 30f;
			EditorEnvironmentRoadsUI.offset2Field.AddLabel(local.format("OffsetFieldLabelText"), 0);
			EditorEnvironmentRoadsUI.offset2Field.OnValueChanged += new TypedSingle(EditorEnvironmentRoadsUI.onTypedOffset2Field);
			EditorEnvironmentRoadsUI.roadScrollBox.AddChild(EditorEnvironmentRoadsUI.offset2Field);
			EditorEnvironmentRoadsUI.concreteToggle = Glazier.Get().CreateToggle();
			EditorEnvironmentRoadsUI.concreteToggle.PositionOffset_X = 200f;
			EditorEnvironmentRoadsUI.concreteToggle.PositionOffset_Y = (float)(LevelRoads.materials.Length * 70 + 160);
			EditorEnvironmentRoadsUI.concreteToggle.SizeOffset_X = 40f;
			EditorEnvironmentRoadsUI.concreteToggle.SizeOffset_Y = 40f;
			EditorEnvironmentRoadsUI.concreteToggle.AddLabel(local.format("ConcreteToggleLabelText"), 1);
			EditorEnvironmentRoadsUI.concreteToggle.OnValueChanged += new Toggled(EditorEnvironmentRoadsUI.onToggledConcreteToggle);
			EditorEnvironmentRoadsUI.roadScrollBox.AddChild(EditorEnvironmentRoadsUI.concreteToggle);
			EditorEnvironmentRoadsUI.selectedBox = Glazier.Get().CreateBox();
			EditorEnvironmentRoadsUI.selectedBox.PositionOffset_X = -200f;
			EditorEnvironmentRoadsUI.selectedBox.PositionOffset_Y = 80f;
			EditorEnvironmentRoadsUI.selectedBox.PositionScale_X = 1f;
			EditorEnvironmentRoadsUI.selectedBox.SizeOffset_X = 200f;
			EditorEnvironmentRoadsUI.selectedBox.SizeOffset_Y = 30f;
			EditorEnvironmentRoadsUI.selectedBox.AddLabel(local.format("SelectionBoxLabelText"), 0);
			EditorEnvironmentRoadsUI.container.AddChild(EditorEnvironmentRoadsUI.selectedBox);
			EditorEnvironmentRoadsUI.updateSelection();
			EditorEnvironmentRoadsUI.bakeRoadsButton = new SleekButtonIcon(bundle.load<Texture2D>("Roads"));
			EditorEnvironmentRoadsUI.bakeRoadsButton.PositionOffset_X = -200f;
			EditorEnvironmentRoadsUI.bakeRoadsButton.PositionOffset_Y = -30f;
			EditorEnvironmentRoadsUI.bakeRoadsButton.PositionScale_X = 1f;
			EditorEnvironmentRoadsUI.bakeRoadsButton.PositionScale_Y = 1f;
			EditorEnvironmentRoadsUI.bakeRoadsButton.SizeOffset_X = 200f;
			EditorEnvironmentRoadsUI.bakeRoadsButton.SizeOffset_Y = 30f;
			EditorEnvironmentRoadsUI.bakeRoadsButton.text = local.format("BakeRoadsButtonText");
			EditorEnvironmentRoadsUI.bakeRoadsButton.tooltip = local.format("BakeRoadsButtonTooltip");
			EditorEnvironmentRoadsUI.bakeRoadsButton.onClickedButton += new ClickedButton(EditorEnvironmentRoadsUI.onClickedBakeRoadsButton);
			EditorEnvironmentRoadsUI.container.AddChild(EditorEnvironmentRoadsUI.bakeRoadsButton);
			EditorEnvironmentRoadsUI.offsetField = Glazier.Get().CreateFloat32Field();
			EditorEnvironmentRoadsUI.offsetField.PositionOffset_Y = -210f;
			EditorEnvironmentRoadsUI.offsetField.PositionScale_Y = 1f;
			EditorEnvironmentRoadsUI.offsetField.SizeOffset_X = 200f;
			EditorEnvironmentRoadsUI.offsetField.SizeOffset_Y = 30f;
			EditorEnvironmentRoadsUI.offsetField.AddLabel(local.format("OffsetFieldLabelText"), 1);
			EditorEnvironmentRoadsUI.offsetField.OnValueChanged += new TypedSingle(EditorEnvironmentRoadsUI.onTypedOffsetField);
			EditorEnvironmentRoadsUI.container.AddChild(EditorEnvironmentRoadsUI.offsetField);
			EditorEnvironmentRoadsUI.offsetField.IsVisible = false;
			EditorEnvironmentRoadsUI.loopToggle = Glazier.Get().CreateToggle();
			EditorEnvironmentRoadsUI.loopToggle.PositionOffset_Y = -170f;
			EditorEnvironmentRoadsUI.loopToggle.PositionScale_Y = 1f;
			EditorEnvironmentRoadsUI.loopToggle.SizeOffset_X = 40f;
			EditorEnvironmentRoadsUI.loopToggle.SizeOffset_Y = 40f;
			EditorEnvironmentRoadsUI.loopToggle.AddLabel(local.format("LoopToggleLabelText"), 1);
			EditorEnvironmentRoadsUI.loopToggle.OnValueChanged += new Toggled(EditorEnvironmentRoadsUI.onToggledLoopToggle);
			EditorEnvironmentRoadsUI.container.AddChild(EditorEnvironmentRoadsUI.loopToggle);
			EditorEnvironmentRoadsUI.loopToggle.IsVisible = false;
			EditorEnvironmentRoadsUI.ignoreTerrainToggle = Glazier.Get().CreateToggle();
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.PositionOffset_Y = -120f;
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.PositionScale_Y = 1f;
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.SizeOffset_X = 40f;
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.SizeOffset_Y = 40f;
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.AddLabel(local.format("IgnoreTerrainToggleLabelText"), 1);
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.OnValueChanged += new Toggled(EditorEnvironmentRoadsUI.onToggledIgnoreTerrainToggle);
			EditorEnvironmentRoadsUI.container.AddChild(EditorEnvironmentRoadsUI.ignoreTerrainToggle);
			EditorEnvironmentRoadsUI.ignoreTerrainToggle.IsVisible = false;
			EditorEnvironmentRoadsUI.modeButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(local.format("Mirror")),
				new GUIContent(local.format("Aligned")),
				new GUIContent(local.format("Free"))
			});
			EditorEnvironmentRoadsUI.modeButton.PositionOffset_Y = -70f;
			EditorEnvironmentRoadsUI.modeButton.PositionScale_Y = 1f;
			EditorEnvironmentRoadsUI.modeButton.SizeOffset_X = 200f;
			EditorEnvironmentRoadsUI.modeButton.SizeOffset_Y = 30f;
			EditorEnvironmentRoadsUI.modeButton.tooltip = local.format("ModeButtonTooltipText");
			EditorEnvironmentRoadsUI.modeButton.onSwappedState = new SwappedState(EditorEnvironmentRoadsUI.onSwappedStateMode);
			EditorEnvironmentRoadsUI.container.AddChild(EditorEnvironmentRoadsUI.modeButton);
			EditorEnvironmentRoadsUI.modeButton.IsVisible = false;
			EditorEnvironmentRoadsUI.roadIndexBox = Glazier.Get().CreateBox();
			EditorEnvironmentRoadsUI.roadIndexBox.PositionOffset_Y = -30f;
			EditorEnvironmentRoadsUI.roadIndexBox.PositionScale_Y = 1f;
			EditorEnvironmentRoadsUI.roadIndexBox.SizeOffset_X = 200f;
			EditorEnvironmentRoadsUI.roadIndexBox.SizeOffset_Y = 30f;
			EditorEnvironmentRoadsUI.roadIndexBox.AddLabel(local.format("RoadIndexLabelText"), 1);
			EditorEnvironmentRoadsUI.container.AddChild(EditorEnvironmentRoadsUI.roadIndexBox);
			EditorEnvironmentRoadsUI.roadIndexBox.IsVisible = false;
			bundle.unload();
		}

		// Token: 0x04002750 RID: 10064
		private static SleekFullscreenBox container;

		// Token: 0x04002751 RID: 10065
		public static bool active;

		// Token: 0x04002752 RID: 10066
		private static ISleekScrollView roadScrollBox;

		// Token: 0x04002753 RID: 10067
		private static ISleekBox selectedBox;

		// Token: 0x04002754 RID: 10068
		private static ISleekFloat32Field widthField;

		// Token: 0x04002755 RID: 10069
		private static ISleekFloat32Field heightField;

		// Token: 0x04002756 RID: 10070
		private static ISleekFloat32Field depthField;

		// Token: 0x04002757 RID: 10071
		private static ISleekFloat32Field offset2Field;

		// Token: 0x04002758 RID: 10072
		private static ISleekToggle concreteToggle;

		// Token: 0x04002759 RID: 10073
		private static SleekButtonIcon bakeRoadsButton;

		// Token: 0x0400275A RID: 10074
		private static ISleekFloat32Field offsetField;

		// Token: 0x0400275B RID: 10075
		private static ISleekToggle loopToggle;

		// Token: 0x0400275C RID: 10076
		private static ISleekToggle ignoreTerrainToggle;

		// Token: 0x0400275D RID: 10077
		private static SleekButtonState modeButton;

		// Token: 0x0400275E RID: 10078
		private static ISleekBox roadIndexBox;
	}
}
