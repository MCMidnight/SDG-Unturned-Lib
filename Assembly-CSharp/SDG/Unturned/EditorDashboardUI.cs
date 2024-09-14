using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000771 RID: 1905
	public class EditorDashboardUI
	{
		// Token: 0x06003E40 RID: 15936 RVA: 0x0012F48B File Offset: 0x0012D68B
		private void onClickedTerrainButton(ISleekElement button)
		{
			this.terrainMenu.open();
			EditorEnvironmentUI.close();
			EditorSpawnsUI.close();
			EditorLevelUI.close();
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x0012F4A7 File Offset: 0x0012D6A7
		private void onClickedEnvironmentButton(ISleekElement button)
		{
			this.terrainMenu.close();
			EditorEnvironmentUI.open();
			EditorSpawnsUI.close();
			EditorLevelUI.close();
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x0012F4C3 File Offset: 0x0012D6C3
		private void onClickedSpawnsButton(ISleekElement button)
		{
			this.terrainMenu.close();
			EditorEnvironmentUI.close();
			EditorSpawnsUI.open();
			EditorLevelUI.close();
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x0012F4DF File Offset: 0x0012D6DF
		private void onClickedLevelButton(ISleekElement button)
		{
			this.terrainMenu.close();
			EditorEnvironmentUI.close();
			EditorSpawnsUI.close();
			EditorLevelUI.open();
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x0012F4FB File Offset: 0x0012D6FB
		public void OnDestroy()
		{
			this.environmentUI.OnDestroy();
			this.levelUI.OnDestroy();
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x0012F514 File Offset: 0x0012D714
		public EditorDashboardUI()
		{
			EditorDashboardUI.localization = Localization.read("/Editor/EditorDashboard.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorDashboard/EditorDashboard.unity3d");
			EditorDashboardUI.container = new SleekFullscreenBox();
			EditorDashboardUI.container.PositionOffset_X = 10f;
			EditorDashboardUI.container.PositionOffset_Y = 10f;
			EditorDashboardUI.container.SizeOffset_X = -20f;
			EditorDashboardUI.container.SizeOffset_Y = -20f;
			EditorDashboardUI.container.SizeScale_X = 1f;
			EditorDashboardUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorDashboardUI.container);
			EditorDashboardUI.terrainButton = new SleekButtonIcon(bundle.load<Texture2D>("Terrain"));
			EditorDashboardUI.terrainButton.SizeOffset_X = -5f;
			EditorDashboardUI.terrainButton.SizeOffset_Y = 30f;
			EditorDashboardUI.terrainButton.SizeScale_X = 0.25f;
			EditorDashboardUI.terrainButton.text = EditorDashboardUI.localization.format("TerrainButtonText");
			EditorDashboardUI.terrainButton.tooltip = EditorDashboardUI.localization.format("TerrainButtonTooltip");
			EditorDashboardUI.terrainButton.onClickedButton += new ClickedButton(this.onClickedTerrainButton);
			EditorDashboardUI.container.AddChild(EditorDashboardUI.terrainButton);
			EditorDashboardUI.environmentButton = new SleekButtonIcon(bundle.load<Texture2D>("Environment"));
			EditorDashboardUI.environmentButton.PositionOffset_X = 5f;
			EditorDashboardUI.environmentButton.PositionScale_X = 0.25f;
			EditorDashboardUI.environmentButton.SizeOffset_X = -10f;
			EditorDashboardUI.environmentButton.SizeOffset_Y = 30f;
			EditorDashboardUI.environmentButton.SizeScale_X = 0.25f;
			EditorDashboardUI.environmentButton.text = EditorDashboardUI.localization.format("EnvironmentButtonText");
			EditorDashboardUI.environmentButton.tooltip = EditorDashboardUI.localization.format("EnvironmentButtonTooltip");
			EditorDashboardUI.environmentButton.onClickedButton += new ClickedButton(this.onClickedEnvironmentButton);
			EditorDashboardUI.container.AddChild(EditorDashboardUI.environmentButton);
			EditorDashboardUI.spawnsButton = new SleekButtonIcon(bundle.load<Texture2D>("Spawns"));
			EditorDashboardUI.spawnsButton.PositionOffset_X = 5f;
			EditorDashboardUI.spawnsButton.PositionScale_X = 0.5f;
			EditorDashboardUI.spawnsButton.SizeOffset_X = -10f;
			EditorDashboardUI.spawnsButton.SizeOffset_Y = 30f;
			EditorDashboardUI.spawnsButton.SizeScale_X = 0.25f;
			EditorDashboardUI.spawnsButton.text = EditorDashboardUI.localization.format("SpawnsButtonText");
			EditorDashboardUI.spawnsButton.tooltip = EditorDashboardUI.localization.format("SpawnsButtonTooltip");
			EditorDashboardUI.spawnsButton.onClickedButton += new ClickedButton(this.onClickedSpawnsButton);
			EditorDashboardUI.container.AddChild(EditorDashboardUI.spawnsButton);
			EditorDashboardUI.levelButton = new SleekButtonIcon(bundle.load<Texture2D>("Level"));
			EditorDashboardUI.levelButton.PositionOffset_X = 5f;
			EditorDashboardUI.levelButton.PositionScale_X = 0.75f;
			EditorDashboardUI.levelButton.SizeOffset_X = -5f;
			EditorDashboardUI.levelButton.SizeOffset_Y = 30f;
			EditorDashboardUI.levelButton.SizeScale_X = 0.25f;
			EditorDashboardUI.levelButton.text = EditorDashboardUI.localization.format("LevelButtonText");
			EditorDashboardUI.levelButton.tooltip = EditorDashboardUI.localization.format("LevelButtonTooltip");
			EditorDashboardUI.levelButton.onClickedButton += new ClickedButton(this.onClickedLevelButton);
			EditorDashboardUI.container.AddChild(EditorDashboardUI.levelButton);
			bundle.unload();
			new EditorPauseUI();
			this.terrainMenu = new EditorTerrainUI();
			this.environmentUI = new EditorEnvironmentUI();
			new EditorSpawnsUI();
			this.levelUI = new EditorLevelUI();
		}

		// Token: 0x0400271A RID: 10010
		private static SleekFullscreenBox container;

		// Token: 0x0400271B RID: 10011
		public static Local localization;

		// Token: 0x0400271C RID: 10012
		private static SleekButtonIcon terrainButton;

		// Token: 0x0400271D RID: 10013
		private static SleekButtonIcon environmentButton;

		// Token: 0x0400271E RID: 10014
		private static SleekButtonIcon spawnsButton;

		// Token: 0x0400271F RID: 10015
		private static SleekButtonIcon levelButton;

		// Token: 0x04002720 RID: 10016
		internal EditorTerrainUI terrainMenu;

		// Token: 0x04002721 RID: 10017
		private EditorEnvironmentUI environmentUI;

		// Token: 0x04002722 RID: 10018
		private EditorLevelUI levelUI;
	}
}
