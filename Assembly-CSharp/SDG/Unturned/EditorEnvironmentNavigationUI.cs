using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000773 RID: 1907
	public class EditorEnvironmentNavigationUI
	{
		// Token: 0x06003E5B RID: 15963 RVA: 0x00130885 File Offset: 0x0012EA85
		public static void open()
		{
			if (EditorEnvironmentNavigationUI.active)
			{
				return;
			}
			EditorEnvironmentNavigationUI.active = true;
			EditorNavigation.isPathfinding = true;
			EditorUI.message(EEditorMessage.NAVIGATION);
			EditorEnvironmentNavigationUI.container.AnimateIntoView();
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x001308AB File Offset: 0x0012EAAB
		public static void close()
		{
			if (!EditorEnvironmentNavigationUI.active)
			{
				return;
			}
			EditorEnvironmentNavigationUI.active = false;
			EditorNavigation.isPathfinding = false;
			EditorEnvironmentNavigationUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x001308D8 File Offset: 0x0012EAD8
		public static void updateSelection(Flag flag)
		{
			if (flag != null)
			{
				EditorEnvironmentNavigationUI.widthSlider.Value = flag.width;
				EditorEnvironmentNavigationUI.heightSlider.Value = flag.height;
				EditorEnvironmentNavigationUI.navBox.Text = flag.graph.graphIndex.ToString();
				EditorEnvironmentNavigationUI.difficultyGUIDField.Text = flag.data.difficultyGUID;
				EditorEnvironmentNavigationUI.maxZombiesField.Value = flag.data.maxZombies;
				EditorEnvironmentNavigationUI.maxBossZombiesField.Value = flag.data.maxBossZombies;
				EditorEnvironmentNavigationUI.spawnZombiesToggle.Value = flag.data.spawnZombies;
				EditorEnvironmentNavigationUI.hyperAgroToggle.Value = flag.data.hyperAgro;
			}
			EditorEnvironmentNavigationUI.widthSlider.IsVisible = (flag != null);
			EditorEnvironmentNavigationUI.heightSlider.IsVisible = (flag != null);
			EditorEnvironmentNavigationUI.navBox.IsVisible = (flag != null);
			EditorEnvironmentNavigationUI.difficultyGUIDField.IsVisible = (flag != null);
			EditorEnvironmentNavigationUI.maxZombiesField.IsVisible = (flag != null);
			EditorEnvironmentNavigationUI.maxBossZombiesField.IsVisible = (flag != null);
			EditorEnvironmentNavigationUI.spawnZombiesToggle.IsVisible = (flag != null);
			EditorEnvironmentNavigationUI.hyperAgroToggle.IsVisible = (flag != null);
			EditorEnvironmentNavigationUI.bakeNavigationButton.IsVisible = (flag != null);
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x00130A0C File Offset: 0x0012EC0C
		private static void onDraggedWidthSlider(ISleekSlider slider, float state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.width = state;
				EditorNavigation.flag.buildMesh();
			}
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x00130A2A File Offset: 0x0012EC2A
		private static void onDraggedHeightSlider(ISleekSlider slider, float state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.height = state;
				EditorNavigation.flag.buildMesh();
			}
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x00130A48 File Offset: 0x0012EC48
		private static void onDifficultyGUIDFieldTyped(ISleekField field, string state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.data.difficultyGUID = state;
			}
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x00130A61 File Offset: 0x0012EC61
		private static void onMaxZombiesFieldTyped(ISleekUInt8Field field, byte state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.data.maxZombies = state;
			}
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x00130A7A File Offset: 0x0012EC7A
		private static void onMaxBossZombiesFieldTyped(ISleekInt32Field field, int state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.data.maxBossZombies = state;
			}
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x00130A93 File Offset: 0x0012EC93
		private static void onToggledSpawnZombiesToggle(ISleekToggle toggle, bool state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.data.spawnZombies = state;
			}
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x00130AAC File Offset: 0x0012ECAC
		private static void onToggledHyperAgroToggle(ISleekToggle toggle, bool state)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.data.hyperAgro = state;
			}
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x00130AC5 File Offset: 0x0012ECC5
		private static void onClickedBakeNavigationButton(ISleekElement button)
		{
			if (EditorNavigation.flag != null)
			{
				EditorNavigation.flag.bakeNavigation();
			}
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x00130AD8 File Offset: 0x0012ECD8
		public EditorEnvironmentNavigationUI()
		{
			Local local = Localization.read("/Editor/EditorEnvironmentNavigation.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorEnvironmentNavigation/EditorEnvironmentNavigation.unity3d");
			EditorEnvironmentNavigationUI.container = new SleekFullscreenBox();
			EditorEnvironmentNavigationUI.container.PositionOffset_X = 10f;
			EditorEnvironmentNavigationUI.container.PositionOffset_Y = 10f;
			EditorEnvironmentNavigationUI.container.PositionScale_X = 1f;
			EditorEnvironmentNavigationUI.container.SizeOffset_X = -20f;
			EditorEnvironmentNavigationUI.container.SizeOffset_Y = -20f;
			EditorEnvironmentNavigationUI.container.SizeScale_X = 1f;
			EditorEnvironmentNavigationUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorEnvironmentNavigationUI.container);
			EditorEnvironmentNavigationUI.active = false;
			EditorEnvironmentNavigationUI.widthSlider = Glazier.Get().CreateSlider();
			EditorEnvironmentNavigationUI.widthSlider.PositionOffset_X = -200f;
			EditorEnvironmentNavigationUI.widthSlider.PositionOffset_Y = 80f;
			EditorEnvironmentNavigationUI.widthSlider.PositionScale_X = 1f;
			EditorEnvironmentNavigationUI.widthSlider.SizeOffset_X = 200f;
			EditorEnvironmentNavigationUI.widthSlider.SizeOffset_Y = 20f;
			EditorEnvironmentNavigationUI.widthSlider.Orientation = 0;
			EditorEnvironmentNavigationUI.widthSlider.AddLabel(local.format("Width_Label"), 0);
			EditorEnvironmentNavigationUI.widthSlider.OnValueChanged += new Dragged(EditorEnvironmentNavigationUI.onDraggedWidthSlider);
			EditorEnvironmentNavigationUI.container.AddChild(EditorEnvironmentNavigationUI.widthSlider);
			EditorEnvironmentNavigationUI.widthSlider.IsVisible = false;
			EditorEnvironmentNavigationUI.heightSlider = Glazier.Get().CreateSlider();
			EditorEnvironmentNavigationUI.heightSlider.PositionOffset_X = -200f;
			EditorEnvironmentNavigationUI.heightSlider.PositionOffset_Y = 110f;
			EditorEnvironmentNavigationUI.heightSlider.PositionScale_X = 1f;
			EditorEnvironmentNavigationUI.heightSlider.SizeOffset_X = 200f;
			EditorEnvironmentNavigationUI.heightSlider.SizeOffset_Y = 20f;
			EditorEnvironmentNavigationUI.heightSlider.Orientation = 0;
			EditorEnvironmentNavigationUI.heightSlider.AddLabel(local.format("Height_Label"), 0);
			EditorEnvironmentNavigationUI.heightSlider.OnValueChanged += new Dragged(EditorEnvironmentNavigationUI.onDraggedHeightSlider);
			EditorEnvironmentNavigationUI.container.AddChild(EditorEnvironmentNavigationUI.heightSlider);
			EditorEnvironmentNavigationUI.heightSlider.IsVisible = false;
			EditorEnvironmentNavigationUI.navBox = Glazier.Get().CreateBox();
			EditorEnvironmentNavigationUI.navBox.PositionOffset_X = -200f;
			EditorEnvironmentNavigationUI.navBox.PositionOffset_Y = 140f;
			EditorEnvironmentNavigationUI.navBox.PositionScale_X = 1f;
			EditorEnvironmentNavigationUI.navBox.SizeOffset_X = 200f;
			EditorEnvironmentNavigationUI.navBox.SizeOffset_Y = 30f;
			EditorEnvironmentNavigationUI.navBox.AddLabel(local.format("Nav_Label"), 0);
			EditorEnvironmentNavigationUI.container.AddChild(EditorEnvironmentNavigationUI.navBox);
			EditorEnvironmentNavigationUI.navBox.IsVisible = false;
			EditorEnvironmentNavigationUI.difficultyGUIDField = Glazier.Get().CreateStringField();
			EditorEnvironmentNavigationUI.difficultyGUIDField.PositionOffset_X = -200f;
			EditorEnvironmentNavigationUI.difficultyGUIDField.PositionOffset_Y = 180f;
			EditorEnvironmentNavigationUI.difficultyGUIDField.PositionScale_X = 1f;
			EditorEnvironmentNavigationUI.difficultyGUIDField.SizeOffset_X = 200f;
			EditorEnvironmentNavigationUI.difficultyGUIDField.SizeOffset_Y = 30f;
			EditorEnvironmentNavigationUI.difficultyGUIDField.MaxLength = 32;
			EditorEnvironmentNavigationUI.difficultyGUIDField.OnTextChanged += new Typed(EditorEnvironmentNavigationUI.onDifficultyGUIDFieldTyped);
			EditorEnvironmentNavigationUI.difficultyGUIDField.AddLabel(local.format("Difficulty_GUID_Field_Label"), 0);
			EditorEnvironmentNavigationUI.container.AddChild(EditorEnvironmentNavigationUI.difficultyGUIDField);
			EditorEnvironmentNavigationUI.difficultyGUIDField.IsVisible = false;
			EditorEnvironmentNavigationUI.maxZombiesField = Glazier.Get().CreateUInt8Field();
			EditorEnvironmentNavigationUI.maxZombiesField.PositionOffset_X = -200f;
			EditorEnvironmentNavigationUI.maxZombiesField.PositionOffset_Y = 220f;
			EditorEnvironmentNavigationUI.maxZombiesField.PositionScale_X = 1f;
			EditorEnvironmentNavigationUI.maxZombiesField.SizeOffset_X = 200f;
			EditorEnvironmentNavigationUI.maxZombiesField.SizeOffset_Y = 30f;
			EditorEnvironmentNavigationUI.maxZombiesField.OnValueChanged += new TypedByte(EditorEnvironmentNavigationUI.onMaxZombiesFieldTyped);
			EditorEnvironmentNavigationUI.maxZombiesField.AddLabel(local.format("Max_Zombies_Field_Label"), 0);
			EditorEnvironmentNavigationUI.container.AddChild(EditorEnvironmentNavigationUI.maxZombiesField);
			EditorEnvironmentNavigationUI.maxZombiesField.IsVisible = false;
			EditorEnvironmentNavigationUI.maxBossZombiesField = Glazier.Get().CreateInt32Field();
			EditorEnvironmentNavigationUI.maxBossZombiesField.PositionOffset_X = -200f;
			EditorEnvironmentNavigationUI.maxBossZombiesField.PositionOffset_Y = 260f;
			EditorEnvironmentNavigationUI.maxBossZombiesField.PositionScale_X = 1f;
			EditorEnvironmentNavigationUI.maxBossZombiesField.SizeOffset_X = 200f;
			EditorEnvironmentNavigationUI.maxBossZombiesField.SizeOffset_Y = 30f;
			EditorEnvironmentNavigationUI.maxBossZombiesField.OnValueChanged += new TypedInt32(EditorEnvironmentNavigationUI.onMaxBossZombiesFieldTyped);
			EditorEnvironmentNavigationUI.maxBossZombiesField.AddLabel(local.format("Max_Boss_Zombies_Field_Label"), 0);
			EditorEnvironmentNavigationUI.container.AddChild(EditorEnvironmentNavigationUI.maxBossZombiesField);
			EditorEnvironmentNavigationUI.maxBossZombiesField.IsVisible = false;
			EditorEnvironmentNavigationUI.spawnZombiesToggle = Glazier.Get().CreateToggle();
			EditorEnvironmentNavigationUI.spawnZombiesToggle.PositionOffset_X = -200f;
			EditorEnvironmentNavigationUI.spawnZombiesToggle.PositionOffset_Y = 300f;
			EditorEnvironmentNavigationUI.spawnZombiesToggle.PositionScale_X = 1f;
			EditorEnvironmentNavigationUI.spawnZombiesToggle.SizeOffset_X = 40f;
			EditorEnvironmentNavigationUI.spawnZombiesToggle.SizeOffset_Y = 40f;
			EditorEnvironmentNavigationUI.spawnZombiesToggle.OnValueChanged += new Toggled(EditorEnvironmentNavigationUI.onToggledSpawnZombiesToggle);
			EditorEnvironmentNavigationUI.spawnZombiesToggle.AddLabel(local.format("Spawn_Zombies_Toggle_Label"), 1);
			EditorEnvironmentNavigationUI.container.AddChild(EditorEnvironmentNavigationUI.spawnZombiesToggle);
			EditorEnvironmentNavigationUI.spawnZombiesToggle.IsVisible = false;
			EditorEnvironmentNavigationUI.hyperAgroToggle = Glazier.Get().CreateToggle();
			EditorEnvironmentNavigationUI.hyperAgroToggle.PositionOffset_X = -200f;
			EditorEnvironmentNavigationUI.hyperAgroToggle.PositionOffset_Y = 350f;
			EditorEnvironmentNavigationUI.hyperAgroToggle.PositionScale_X = 1f;
			EditorEnvironmentNavigationUI.hyperAgroToggle.SizeOffset_X = 40f;
			EditorEnvironmentNavigationUI.hyperAgroToggle.SizeOffset_Y = 40f;
			EditorEnvironmentNavigationUI.hyperAgroToggle.OnValueChanged += new Toggled(EditorEnvironmentNavigationUI.onToggledHyperAgroToggle);
			EditorEnvironmentNavigationUI.hyperAgroToggle.AddLabel(local.format("Hyper_Agro_Label"), 1);
			EditorEnvironmentNavigationUI.container.AddChild(EditorEnvironmentNavigationUI.hyperAgroToggle);
			EditorEnvironmentNavigationUI.hyperAgroToggle.IsVisible = false;
			EditorEnvironmentNavigationUI.bakeNavigationButton = new SleekButtonIcon(bundle.load<Texture2D>("Navigation"));
			EditorEnvironmentNavigationUI.bakeNavigationButton.PositionOffset_X = -200f;
			EditorEnvironmentNavigationUI.bakeNavigationButton.PositionOffset_Y = -30f;
			EditorEnvironmentNavigationUI.bakeNavigationButton.PositionScale_X = 1f;
			EditorEnvironmentNavigationUI.bakeNavigationButton.PositionScale_Y = 1f;
			EditorEnvironmentNavigationUI.bakeNavigationButton.SizeOffset_X = 200f;
			EditorEnvironmentNavigationUI.bakeNavigationButton.SizeOffset_Y = 30f;
			EditorEnvironmentNavigationUI.bakeNavigationButton.text = local.format("Bake_Navigation");
			EditorEnvironmentNavigationUI.bakeNavigationButton.tooltip = local.format("Bake_Navigation_Tooltip");
			EditorEnvironmentNavigationUI.bakeNavigationButton.onClickedButton += new ClickedButton(EditorEnvironmentNavigationUI.onClickedBakeNavigationButton);
			EditorEnvironmentNavigationUI.container.AddChild(EditorEnvironmentNavigationUI.bakeNavigationButton);
			EditorEnvironmentNavigationUI.bakeNavigationButton.IsVisible = false;
			bundle.unload();
		}

		// Token: 0x0400273A RID: 10042
		private static SleekFullscreenBox container;

		// Token: 0x0400273B RID: 10043
		public static bool active;

		// Token: 0x0400273C RID: 10044
		private static ISleekSlider widthSlider;

		// Token: 0x0400273D RID: 10045
		private static ISleekSlider heightSlider;

		// Token: 0x0400273E RID: 10046
		private static ISleekBox navBox;

		// Token: 0x0400273F RID: 10047
		private static ISleekField difficultyGUIDField;

		// Token: 0x04002740 RID: 10048
		private static ISleekUInt8Field maxZombiesField;

		// Token: 0x04002741 RID: 10049
		private static ISleekInt32Field maxBossZombiesField;

		// Token: 0x04002742 RID: 10050
		private static ISleekToggle spawnZombiesToggle;

		// Token: 0x04002743 RID: 10051
		private static ISleekToggle hyperAgroToggle;

		// Token: 0x04002744 RID: 10052
		private static SleekButtonIcon bakeNavigationButton;
	}
}
