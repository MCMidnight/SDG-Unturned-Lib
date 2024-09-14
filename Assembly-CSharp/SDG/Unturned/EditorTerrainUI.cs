using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000787 RID: 1927
	public class EditorTerrainUI
	{
		// Token: 0x06003F95 RID: 16277 RVA: 0x001410E2 File Offset: 0x0013F2E2
		public void open()
		{
			if (EditorTerrainUI.active)
			{
				return;
			}
			EditorTerrainUI.active = true;
			EditorTerrainUI.container.AnimateIntoView();
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x001410FC File Offset: 0x0013F2FC
		public void close()
		{
			if (!EditorTerrainUI.active)
			{
				return;
			}
			EditorTerrainUI.active = false;
			this.heightV2.Close();
			this.materialsV2.Close();
			this.detailsV2.Close();
			this.tiles.Close();
			EditorTerrainUI.container.AnimateOutOfView(1f, 0f);
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x00141157 File Offset: 0x0013F357
		public void GoToHeightsTab()
		{
			this.detailsV2.Close();
			this.materialsV2.Close();
			this.tiles.Close();
			this.heightV2.Open();
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x00141185 File Offset: 0x0013F385
		public void GoToMaterialsTab()
		{
			this.heightV2.Close();
			this.detailsV2.Close();
			this.tiles.Close();
			this.materialsV2.Open();
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x001411B3 File Offset: 0x0013F3B3
		public void GoToFoliageTab()
		{
			this.heightV2.Close();
			this.materialsV2.Close();
			this.tiles.Close();
			this.detailsV2.Open();
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x001411E1 File Offset: 0x0013F3E1
		public void GoToTilesTab()
		{
			this.heightV2.Close();
			this.materialsV2.Close();
			this.detailsV2.Close();
			this.tiles.Open();
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0014120F File Offset: 0x0013F40F
		private void onClickedHeightButton(ISleekElement button)
		{
			this.GoToHeightsTab();
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x00141217 File Offset: 0x0013F417
		private void onClickedMaterialsButton(ISleekElement button)
		{
			this.GoToMaterialsTab();
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x0014121F File Offset: 0x0013F41F
		private void onClickedDetailsButton(ISleekElement button)
		{
			this.GoToFoliageTab();
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x00141227 File Offset: 0x0013F427
		private void OnClickedTilesButton(ISleekElement button)
		{
			this.GoToTilesTab();
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x00141230 File Offset: 0x0013F430
		public EditorTerrainUI()
		{
			Local local = Localization.read("/Editor/EditorTerrain.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorTerrain/EditorTerrain.unity3d");
			EditorTerrainUI.container = new SleekFullscreenBox();
			EditorTerrainUI.container.PositionOffset_X = 10f;
			EditorTerrainUI.container.PositionOffset_Y = 10f;
			EditorTerrainUI.container.PositionScale_X = 1f;
			EditorTerrainUI.container.SizeOffset_X = -20f;
			EditorTerrainUI.container.SizeOffset_Y = -20f;
			EditorTerrainUI.container.SizeScale_X = 1f;
			EditorTerrainUI.container.SizeScale_Y = 1f;
			EditorUI.window.AddChild(EditorTerrainUI.container);
			EditorTerrainUI.active = false;
			SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(bundle.load<Texture2D>("Height"));
			sleekButtonIcon.PositionOffset_Y = 40f;
			sleekButtonIcon.SizeOffset_X = -5f;
			sleekButtonIcon.SizeOffset_Y = 30f;
			sleekButtonIcon.SizeScale_X = 0.25f;
			sleekButtonIcon.text = local.format("HeightButtonText") + " [1]";
			sleekButtonIcon.tooltip = local.format("HeightButtonTooltip");
			sleekButtonIcon.onClickedButton += new ClickedButton(this.onClickedHeightButton);
			EditorTerrainUI.container.AddChild(sleekButtonIcon);
			SleekButtonIcon sleekButtonIcon2 = new SleekButtonIcon(bundle.load<Texture2D>("Materials"));
			sleekButtonIcon2.PositionOffset_X = 5f;
			sleekButtonIcon2.PositionOffset_Y = 40f;
			sleekButtonIcon2.PositionScale_X = 0.25f;
			sleekButtonIcon2.SizeOffset_X = -10f;
			sleekButtonIcon2.SizeOffset_Y = 30f;
			sleekButtonIcon2.SizeScale_X = 0.25f;
			sleekButtonIcon2.text = local.format("MaterialsButtonText") + " [2]";
			sleekButtonIcon2.tooltip = local.format("MaterialsButtonTooltip");
			sleekButtonIcon2.onClickedButton += new ClickedButton(this.onClickedMaterialsButton);
			EditorTerrainUI.container.AddChild(sleekButtonIcon2);
			SleekButtonIcon sleekButtonIcon3 = new SleekButtonIcon(bundle.load<Texture2D>("Details"));
			sleekButtonIcon3.PositionOffset_X = 5f;
			sleekButtonIcon3.PositionOffset_Y = 40f;
			sleekButtonIcon3.PositionScale_X = 0.5f;
			sleekButtonIcon3.SizeOffset_X = -10f;
			sleekButtonIcon3.SizeOffset_Y = 30f;
			sleekButtonIcon3.SizeScale_X = 0.25f;
			sleekButtonIcon3.text = local.format("DetailsButtonText") + " [3]";
			sleekButtonIcon3.tooltip = local.format("DetailsButtonTooltip");
			sleekButtonIcon3.onClickedButton += new ClickedButton(this.onClickedDetailsButton);
			EditorTerrainUI.container.AddChild(sleekButtonIcon3);
			ISleekButton sleekButton = Glazier.Get().CreateButton();
			sleekButton.PositionOffset_X = 5f;
			sleekButton.PositionOffset_Y = 40f;
			sleekButton.PositionScale_X = 0.75f;
			sleekButton.SizeOffset_X = -5f;
			sleekButton.SizeOffset_Y = 30f;
			sleekButton.SizeScale_X = 0.25f;
			sleekButton.Text = local.format("TilesButton_Label") + " [4]";
			sleekButton.TooltipText = local.format("TilesButton_Tooltip");
			sleekButton.OnClicked += new ClickedButton(this.OnClickedTilesButton);
			EditorTerrainUI.container.AddChild(sleekButton);
			this.heightV2 = new EditorTerrainHeightUI();
			this.heightV2.PositionOffset_X = 10f;
			this.heightV2.PositionOffset_Y = 90f;
			this.heightV2.PositionScale_X = 1f;
			this.heightV2.SizeOffset_X = -20f;
			this.heightV2.SizeOffset_Y = -100f;
			this.heightV2.SizeScale_X = 1f;
			this.heightV2.SizeScale_Y = 1f;
			EditorUI.window.AddChild(this.heightV2);
			this.materialsV2 = new EditorTerrainMaterialsUI();
			this.materialsV2.PositionOffset_X = 10f;
			this.materialsV2.PositionOffset_Y = 90f;
			this.materialsV2.PositionScale_X = 1f;
			this.materialsV2.SizeOffset_X = -20f;
			this.materialsV2.SizeOffset_Y = -100f;
			this.materialsV2.SizeScale_X = 1f;
			this.materialsV2.SizeScale_Y = 1f;
			EditorUI.window.AddChild(this.materialsV2);
			this.detailsV2 = new EditorTerrainDetailsUI();
			this.detailsV2.PositionOffset_X = 10f;
			this.detailsV2.PositionOffset_Y = 90f;
			this.detailsV2.PositionScale_X = 1f;
			this.detailsV2.SizeOffset_X = -20f;
			this.detailsV2.SizeOffset_Y = -100f;
			this.detailsV2.SizeScale_X = 1f;
			this.detailsV2.SizeScale_Y = 1f;
			EditorUI.window.AddChild(this.detailsV2);
			this.tiles = new EditorTerrainTilesUI();
			this.tiles.PositionOffset_X = 10f;
			this.tiles.PositionOffset_Y = 90f;
			this.tiles.PositionScale_X = 1f;
			this.tiles.SizeOffset_X = -20f;
			this.tiles.SizeOffset_Y = -100f;
			this.tiles.SizeScale_X = 1f;
			this.tiles.SizeScale_Y = 1f;
			EditorUI.window.AddChild(this.tiles);
			bundle.unload();
		}

		// Token: 0x0400286C RID: 10348
		private static SleekFullscreenBox container;

		// Token: 0x0400286D RID: 10349
		public static bool active;

		// Token: 0x0400286E RID: 10350
		private EditorTerrainHeightUI heightV2;

		// Token: 0x0400286F RID: 10351
		private EditorTerrainMaterialsUI materialsV2;

		// Token: 0x04002870 RID: 10352
		private EditorTerrainDetailsUI detailsV2;

		// Token: 0x04002871 RID: 10353
		private EditorTerrainTilesUI tiles;
	}
}
