using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000713 RID: 1811
	public class SleekEconIcon : SleekWrapper
	{
		// Token: 0x06003BDE RID: 15326 RVA: 0x00119430 File Offset: 0x00117630
		public void SetItemDefId(int itemdefid)
		{
			if (itemdefid < 1)
			{
				this.internalImage.IsVisible = false;
				this.isExpectingIconReadyCallback = false;
				return;
			}
			ushort inventorySkinID = Provider.provider.economyService.getInventorySkinID(itemdefid);
			if (inventorySkinID > 0)
			{
				SkinAsset skinAsset = Assets.find(EAssetType.SKIN, inventorySkinID) as SkinAsset;
				if (skinAsset != null)
				{
					Guid guid;
					Guid guid2;
					Provider.provider.economyService.getInventoryTargetID(itemdefid, out guid, out guid2);
					ItemAsset itemAsset = Assets.find<ItemAsset>(guid);
					VehicleAsset vehicleAsset = VehicleTool.FindVehicleByGuidAndHandleRedirects(guid2);
					if (vehicleAsset != null)
					{
						this.internalImage.IsVisible = false;
						VehicleTool.getIcon(vehicleAsset.id, skinAsset.id, vehicleAsset, skinAsset, 400, 400, false, new VehicleIconReady(this.OnIconReady));
						this.isExpectingIconReadyCallback = true;
						return;
					}
					if (itemAsset != null)
					{
						this.internalImage.IsVisible = false;
						ItemTool.getIcon(itemAsset.id, skinAsset.id, 100, itemAsset.getState(), itemAsset, skinAsset, string.Empty, string.Empty, 400, 400, true, false, new ItemIconReady(this.OnIconReady));
						this.isExpectingIconReadyCallback = true;
						return;
					}
				}
			}
			Texture2D texture2D = Provider.provider.economyService.LoadItemIcon(itemdefid);
			this.internalImage.Texture = texture2D;
			this.internalImage.IsVisible = (texture2D != null);
			this.isExpectingIconReadyCallback = false;
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x00119577 File Offset: 0x00117777
		public void SetIsBoxMythicalIcon()
		{
			this.internalImage.Texture = Resources.Load<Texture2D>("Economy/Mystery/Icon_Large");
			this.internalImage.IsVisible = true;
			this.isExpectingIconReadyCallback = false;
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06003BE0 RID: 15328 RVA: 0x001195A1 File Offset: 0x001177A1
		// (set) Token: 0x06003BE1 RID: 15329 RVA: 0x001195AE File Offset: 0x001177AE
		public SleekColor color
		{
			get
			{
				return this.internalImage.TintColor;
			}
			set
			{
				this.internalImage.TintColor = value;
			}
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x001195BC File Offset: 0x001177BC
		public override void OnDestroy()
		{
			this.internalImage = null;
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x001195C8 File Offset: 0x001177C8
		public SleekEconIcon()
		{
			this.internalImage = Glazier.Get().CreateImage();
			this.internalImage.SizeScale_X = 1f;
			this.internalImage.SizeScale_Y = 1f;
			base.AddChild(this.internalImage);
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x00119617 File Offset: 0x00117817
		private void OnIconReady(Texture2D texture)
		{
			if (this.internalImage != null && this.isExpectingIconReadyCallback)
			{
				this.internalImage.Texture = texture;
				this.internalImage.IsVisible = (texture != null);
			}
		}

		// Token: 0x04002578 RID: 9592
		private ISleekImage internalImage;

		// Token: 0x04002579 RID: 9593
		private bool isExpectingIconReadyCallback;
	}
}
