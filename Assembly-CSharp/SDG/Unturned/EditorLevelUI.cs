using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000779 RID: 1913
	public class EditorLevelUI
	{
		// Token: 0x06003EAD RID: 16045 RVA: 0x00133FC4 File Offset: 0x001321C4
		public static void open()
		{
			if (EditorLevelUI.active)
			{
				return;
			}
			EditorLevelUI.active = true;
			EditorLevelUI.container.AnimateIntoView();
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x00133FDE File Offset: 0x001321DE
		public static void close()
		{
			if (!EditorLevelUI.active)
			{
				return;
			}
			EditorLevelUI.active = false;
			EditorLevelObjectsUI.close();
			EditorLevelVisibilityUI.close();
			EditorLevelPlayersUI.close();
			EditorLevelUI.volumesUI.Close();
			EditorLevelUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003EAF RID: 16047 RVA: 0x0013401B File Offset: 0x0013221B
		private void onClickedObjectsButton(ISleekElement button)
		{
			EditorLevelObjectsUI.open();
			EditorLevelVisibilityUI.close();
			EditorLevelPlayersUI.close();
			EditorLevelUI.volumesUI.Close();
		}

		// Token: 0x06003EB0 RID: 16048 RVA: 0x00134036 File Offset: 0x00132236
		private void onClickedVisibilityButton(ISleekElement button)
		{
			EditorLevelObjectsUI.close();
			EditorLevelVisibilityUI.open();
			EditorLevelPlayersUI.close();
			EditorLevelUI.volumesUI.Close();
		}

		// Token: 0x06003EB1 RID: 16049 RVA: 0x00134051 File Offset: 0x00132251
		private void onClickedPlayersButton(ISleekElement button)
		{
			EditorLevelObjectsUI.close();
			EditorLevelVisibilityUI.close();
			EditorLevelPlayersUI.open();
			EditorLevelUI.volumesUI.Close();
		}

		// Token: 0x06003EB2 RID: 16050 RVA: 0x0013406C File Offset: 0x0013226C
		private void OnClickedVolumesButton(ISleekElement button)
		{
			EditorLevelObjectsUI.close();
			EditorLevelVisibilityUI.close();
			EditorLevelPlayersUI.close();
			EditorLevelUI.volumesUI.Open();
		}

		// Token: 0x06003EB3 RID: 16051 RVA: 0x00134087 File Offset: 0x00132287
		public void OnDestroy()
		{
			this.objectsUI.OnDestroy();
			EditorLevelUI.volumesUI.OnDestroy();
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x001340A0 File Offset: 0x001322A0
		public EditorLevelUI()
		{
			Local local = Localization.read("/Editor/EditorLevel.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevel/EditorLevel.unity3d");
			EditorLevelUI.container = new SleekFullscreenBox();
			EditorLevelUI.container.PositionOffset_X = 10f;
			EditorLevelUI.container.PositionOffset_Y = 10f;
			EditorLevelUI.container.PositionScale_X = 1f;
			EditorLevelUI.container.SizeOffset_X = -20f;
			EditorLevelUI.container.SizeOffset_Y = -20f;
			EditorLevelUI.container.SizeScale_X = 1f;
			EditorLevelUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorLevelUI.container);
			EditorLevelUI.active = false;
			EditorLevelUI.objectsButton = new SleekButtonIcon(bundle.load<Texture2D>("Objects"));
			EditorLevelUI.objectsButton.PositionOffset_Y = 40f;
			EditorLevelUI.objectsButton.SizeOffset_X = -5f;
			EditorLevelUI.objectsButton.SizeOffset_Y = 30f;
			EditorLevelUI.objectsButton.SizeScale_X = 0.25f;
			EditorLevelUI.objectsButton.text = local.format("ObjectsButtonText");
			EditorLevelUI.objectsButton.tooltip = local.format("ObjectsButtonTooltip");
			EditorLevelUI.objectsButton.onClickedButton += new ClickedButton(this.onClickedObjectsButton);
			EditorLevelUI.container.AddChild(EditorLevelUI.objectsButton);
			EditorLevelUI.visibilityButton = new SleekButtonIcon(bundle.load<Texture2D>("Visibility"));
			EditorLevelUI.visibilityButton.PositionOffset_X = 5f;
			EditorLevelUI.visibilityButton.PositionOffset_Y = 40f;
			EditorLevelUI.visibilityButton.PositionScale_X = 0.25f;
			EditorLevelUI.visibilityButton.SizeOffset_X = -10f;
			EditorLevelUI.visibilityButton.SizeOffset_Y = 30f;
			EditorLevelUI.visibilityButton.SizeScale_X = 0.25f;
			EditorLevelUI.visibilityButton.text = local.format("VisibilityButtonText");
			EditorLevelUI.visibilityButton.tooltip = local.format("VisibilityButtonTooltip");
			EditorLevelUI.visibilityButton.onClickedButton += new ClickedButton(this.onClickedVisibilityButton);
			EditorLevelUI.container.AddChild(EditorLevelUI.visibilityButton);
			EditorLevelUI.playersButton = new SleekButtonIcon(bundle.load<Texture2D>("Players"));
			EditorLevelUI.playersButton.PositionOffset_Y = 40f;
			EditorLevelUI.playersButton.PositionOffset_X = 5f;
			EditorLevelUI.playersButton.PositionScale_X = 0.5f;
			EditorLevelUI.playersButton.SizeOffset_X = -10f;
			EditorLevelUI.playersButton.SizeOffset_Y = 30f;
			EditorLevelUI.playersButton.SizeScale_X = 0.25f;
			EditorLevelUI.playersButton.text = local.format("PlayersButtonText");
			EditorLevelUI.playersButton.tooltip = local.format("PlayersButtonTooltip");
			EditorLevelUI.playersButton.onClickedButton += new ClickedButton(this.onClickedPlayersButton);
			EditorLevelUI.container.AddChild(EditorLevelUI.playersButton);
			EditorLevelUI.volumesButton = new SleekButtonIcon(null);
			EditorLevelUI.volumesButton.PositionOffset_Y = 40f;
			EditorLevelUI.volumesButton.PositionOffset_X = 5f;
			EditorLevelUI.volumesButton.PositionScale_X = 0.75f;
			EditorLevelUI.volumesButton.SizeOffset_X = -5f;
			EditorLevelUI.volumesButton.SizeOffset_Y = 30f;
			EditorLevelUI.volumesButton.SizeScale_X = 0.25f;
			EditorLevelUI.volumesButton.text = local.format("VolumesButtonText");
			EditorLevelUI.volumesButton.tooltip = local.format("VolumesButtonTooltip");
			EditorLevelUI.volumesButton.onClickedButton += new ClickedButton(this.OnClickedVolumesButton);
			EditorLevelUI.container.AddChild(EditorLevelUI.volumesButton);
			bundle.unload();
			this.objectsUI = new EditorLevelObjectsUI();
			this.objectsUI.PositionOffset_X = 10f;
			this.objectsUI.PositionOffset_Y = 90f;
			this.objectsUI.PositionScale_X = 1f;
			this.objectsUI.SizeOffset_X = -20f;
			this.objectsUI.SizeOffset_Y = -100f;
			this.objectsUI.SizeScale_X = 1f;
			this.objectsUI.SizeScale_Y = 1f;
			EditorUI.window.AddChild(this.objectsUI);
			new EditorLevelVisibilityUI();
			new EditorLevelPlayersUI();
			EditorLevelUI.volumesUI = new EditorVolumesUI();
			EditorLevelUI.volumesUI.PositionOffset_X = 10f;
			EditorLevelUI.volumesUI.PositionOffset_Y = 90f;
			EditorLevelUI.volumesUI.PositionScale_X = 1f;
			EditorLevelUI.volumesUI.SizeOffset_X = -20f;
			EditorLevelUI.volumesUI.SizeOffset_Y = -100f;
			EditorLevelUI.volumesUI.SizeScale_X = 1f;
			EditorLevelUI.volumesUI.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorLevelUI.volumesUI);
		}

		// Token: 0x04002781 RID: 10113
		private static SleekFullscreenBox container;

		// Token: 0x04002782 RID: 10114
		public static bool active;

		// Token: 0x04002783 RID: 10115
		private static SleekButtonIcon objectsButton;

		// Token: 0x04002784 RID: 10116
		private static SleekButtonIcon visibilityButton;

		// Token: 0x04002785 RID: 10117
		private static SleekButtonIcon playersButton;

		// Token: 0x04002786 RID: 10118
		private static SleekButtonIcon volumesButton;

		// Token: 0x04002787 RID: 10119
		private EditorLevelObjectsUI objectsUI;

		// Token: 0x04002788 RID: 10120
		private static EditorVolumesUI volumesUI;
	}
}
