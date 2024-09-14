using System;
using SDG.Framework.Landscapes;

namespace SDG.Unturned
{
	// Token: 0x02000785 RID: 1925
	internal class TerrainTileLayer : SleekWrapper
	{
		// Token: 0x06003F84 RID: 16260 RVA: 0x001402A4 File Offset: 0x0013E4A4
		public void UpdateSelectedTile()
		{
			LandscapeTile selectedTile = TerrainEditor.selectedTile;
			if (selectedTile == null)
			{
				this.nameButton.Text = string.Empty;
				return;
			}
			AssetReference<LandscapeMaterialAsset> assetReference = selectedTile.materials[this.layerIndex];
			LandscapeMaterialAsset landscapeMaterialAsset = assetReference.Find();
			if (landscapeMaterialAsset != null)
			{
				this.nameButton.Text = landscapeMaterialAsset.FriendlyName;
				return;
			}
			if (assetReference.isNull)
			{
				this.nameButton.Text = EditorTerrainTilesUI.localization.format("LayerNull");
				return;
			}
			this.nameButton.Text = assetReference.GUID.ToString("N");
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x00140340 File Offset: 0x0013E540
		public TerrainTileLayer(EditorTerrainTilesUI owner, int layerIndex)
		{
			this.owner = owner;
			this.layerIndex = layerIndex;
			this.layerBox = Glazier.Get().CreateBox();
			this.layerBox.SizeOffset_X = 30f;
			this.layerBox.SizeScale_Y = 1f;
			this.layerBox.Text = layerIndex.ToString();
			base.AddChild(this.layerBox);
			this.nameButton = Glazier.Get().CreateButton();
			this.nameButton.PositionOffset_X = 30f;
			this.nameButton.SizeScale_X = 1f;
			this.nameButton.SizeScale_Y = 1f;
			this.nameButton.SizeOffset_X = -30f;
			this.nameButton.OnClicked += new ClickedButton(this.OnClicked);
			base.AddChild(this.nameButton);
			this.UpdateSelectedTile();
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x00140428 File Offset: 0x0013E628
		private void OnClicked(ISleekElement element)
		{
			this.owner.SetSelectedLayerIndex(this.layerIndex);
		}

		// Token: 0x0400285B RID: 10331
		private EditorTerrainTilesUI owner;

		// Token: 0x0400285C RID: 10332
		private int layerIndex;

		// Token: 0x0400285D RID: 10333
		private ISleekBox layerBox;

		// Token: 0x0400285E RID: 10334
		private ISleekButton nameButton;
	}
}
