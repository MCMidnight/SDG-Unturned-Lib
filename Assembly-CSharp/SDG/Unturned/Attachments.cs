using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000283 RID: 643
	public class Attachments : MonoBehaviour
	{
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x00044A8D File Offset: 0x00042C8D
		public ItemGunAsset gunAsset
		{
			get
			{
				return this._gunAsset;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x00044A95 File Offset: 0x00042C95
		public SkinAsset skinAsset
		{
			get
			{
				return this._skinAsset;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060012DD RID: 4829 RVA: 0x00044A9D File Offset: 0x00042C9D
		public ushort sightID
		{
			get
			{
				return this._sightID;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x00044AA5 File Offset: 0x00042CA5
		public ushort tacticalID
		{
			get
			{
				return this._tacticalID;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060012DF RID: 4831 RVA: 0x00044AAD File Offset: 0x00042CAD
		public ushort gripID
		{
			get
			{
				return this._gripID;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x00044AB5 File Offset: 0x00042CB5
		public ushort barrelID
		{
			get
			{
				return this._barrelID;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x00044ABD File Offset: 0x00042CBD
		public ushort magazineID
		{
			get
			{
				return this._magazineID;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060012E2 RID: 4834 RVA: 0x00044AC5 File Offset: 0x00042CC5
		public ItemSightAsset sightAsset
		{
			get
			{
				return this._sightAsset;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060012E3 RID: 4835 RVA: 0x00044ACD File Offset: 0x00042CCD
		public ItemTacticalAsset tacticalAsset
		{
			get
			{
				return this._tacticalAsset;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060012E4 RID: 4836 RVA: 0x00044AD5 File Offset: 0x00042CD5
		public ItemGripAsset gripAsset
		{
			get
			{
				return this._gripAsset;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x00044ADD File Offset: 0x00042CDD
		public ItemBarrelAsset barrelAsset
		{
			get
			{
				return this._barrelAsset;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x00044AE5 File Offset: 0x00042CE5
		public ItemMagazineAsset magazineAsset
		{
			get
			{
				return this._magazineAsset;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x00044AED File Offset: 0x00042CED
		public Transform sightModel
		{
			get
			{
				return this._sightModel;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060012E8 RID: 4840 RVA: 0x00044AF5 File Offset: 0x00042CF5
		public Transform tacticalModel
		{
			get
			{
				return this._tacticalModel;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x00044AFD File Offset: 0x00042CFD
		public Transform gripModel
		{
			get
			{
				return this._gripModel;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060012EA RID: 4842 RVA: 0x00044B05 File Offset: 0x00042D05
		public Transform barrelModel
		{
			get
			{
				return this._barrelModel;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x00044B0D File Offset: 0x00042D0D
		public Transform magazineModel
		{
			get
			{
				return this._magazineModel;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x00044B15 File Offset: 0x00042D15
		public Transform sightHook
		{
			get
			{
				return this._sightHook;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x00044B1D File Offset: 0x00042D1D
		public Transform viewHook
		{
			get
			{
				return this._viewHook;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x00044B25 File Offset: 0x00042D25
		public Transform tacticalHook
		{
			get
			{
				return this._tacticalHook;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x00044B2D File Offset: 0x00042D2D
		public Transform gripHook
		{
			get
			{
				return this._gripHook;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x00044B35 File Offset: 0x00042D35
		public Transform barrelHook
		{
			get
			{
				return this._barrelHook;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x00044B3D File Offset: 0x00042D3D
		public Transform magazineHook
		{
			get
			{
				return this._magazineHook;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x00044B45 File Offset: 0x00042D45
		public Transform ejectHook
		{
			get
			{
				return this._ejectHook;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x00044B4D File Offset: 0x00042D4D
		public Transform lightHook
		{
			get
			{
				return this._lightHook;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x00044B55 File Offset: 0x00042D55
		public Transform light2Hook
		{
			get
			{
				return this._light2Hook;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x00044B5D File Offset: 0x00042D5D
		public Transform aimHook
		{
			get
			{
				return this._aimHook;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060012F6 RID: 4854 RVA: 0x00044B65 File Offset: 0x00042D65
		public Transform scopeHook
		{
			get
			{
				return this._scopeHook;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x00044B6D File Offset: 0x00042D6D
		public Transform reticuleHook
		{
			get
			{
				return this._reticuleHook;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060012F8 RID: 4856 RVA: 0x00044B75 File Offset: 0x00042D75
		public Transform leftHook
		{
			get
			{
				return this._leftHook;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x00044B7D File Offset: 0x00042D7D
		public Transform rightHook
		{
			get
			{
				return this._rightHook;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x00044B85 File Offset: 0x00042D85
		public Transform nockHook
		{
			get
			{
				return this._nockHook;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x00044B8D File Offset: 0x00042D8D
		public Transform restHook
		{
			get
			{
				return this._restHook;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060012FC RID: 4860 RVA: 0x00044B95 File Offset: 0x00042D95
		public LineRenderer rope
		{
			get
			{
				return this._rope;
			}
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x00044BA0 File Offset: 0x00042DA0
		public void applyVisual()
		{
			if (this.isSkinned != this.wasSkinned)
			{
				this.wasSkinned = this.isSkinned;
				if (this.tempSightMaterial != null)
				{
					HighlighterTool.rematerialize(this.sightModel, this.tempSightMaterial, out this.tempSightMaterial);
				}
				if (this.tempTacticalMaterial != null)
				{
					HighlighterTool.rematerialize(this.tacticalModel, this.tempTacticalMaterial, out this.tempTacticalMaterial);
				}
				if (this.tempGripMaterial != null)
				{
					HighlighterTool.rematerialize(this.gripModel, this.tempGripMaterial, out this.tempGripMaterial);
				}
				if (this.tempBarrelMaterial != null)
				{
					HighlighterTool.rematerialize(this.barrelModel, this.tempBarrelMaterial, out this.tempBarrelMaterial);
				}
				if (this.tempMagazineMaterial != null)
				{
					HighlighterTool.rematerialize(this.magazineModel, this.tempMagazineMaterial, out this.tempMagazineMaterial);
				}
			}
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00044C83 File Offset: 0x00042E83
		public void updateGun(ItemGunAsset newGunAsset, SkinAsset newSkinAsset)
		{
			this._gunAsset = newGunAsset;
			this._skinAsset = newSkinAsset;
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00044C94 File Offset: 0x00042E94
		public static void parseFromItemState(byte[] state, out ushort sight, out ushort tactical, out ushort grip, out ushort barrel, out ushort magazine)
		{
			if (state == null || state.Length < 10)
			{
				sight = 0;
				tactical = 0;
				grip = 0;
				barrel = 0;
				magazine = 0;
				return;
			}
			sight = BitConverter.ToUInt16(state, 0);
			tactical = BitConverter.ToUInt16(state, 2);
			grip = BitConverter.ToUInt16(state, 4);
			barrel = BitConverter.ToUInt16(state, 6);
			magazine = BitConverter.ToUInt16(state, 8);
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x00044CEC File Offset: 0x00042EEC
		public void updateAttachments(byte[] state, bool viewmodel)
		{
			if (state == null || state.Length != 18)
			{
				return;
			}
			base.transform.localScale = Vector3.one;
			Attachments.parseFromItemState(state, out this._sightID, out this._tacticalID, out this._gripID, out this._barrelID, out this._magazineID);
			this.DestroySkinMaterials();
			if (this.sightModel != null)
			{
				Object.Destroy(this.sightModel.gameObject);
				this._sightModel = null;
			}
			try
			{
				this._sightAsset = (Assets.find(EAssetType.ITEM, this.sightID) as ItemSightAsset);
			}
			catch
			{
				this._sightAsset = null;
			}
			this.tempSightMaterial = null;
			if (this.sightAsset != null && this.sightHook != null && this.sightAsset.sight != null)
			{
				this._sightModel = Object.Instantiate<GameObject>(this.sightAsset.sight).transform;
				this.sightModel.name = this.sightAsset.instantiatedAttachmentName;
				this.sightModel.transform.parent = this.sightHook;
				this.sightModel.transform.localPosition = Vector3.zero;
				this.sightModel.transform.localRotation = Quaternion.identity;
				this.sightModel.localScale = Vector3.one;
				if (this.shouldDestroyColliders && this.sightAsset.shouldDestroyAttachmentColliders)
				{
					PrefabUtil.DestroyCollidersInChildren(this.sightModel.gameObject, true);
				}
				this.sightModel.DestroyRigidbody();
				if (viewmodel)
				{
					Layerer.viewmodel(this.sightModel);
				}
				if (this.gunAsset != null && this.skinAsset != null && this.skinAsset.secondarySkins != null)
				{
					Material material = null;
					if (!this.skinAsset.secondarySkins.TryGetValue(this.sightID, ref material) && this.skinAsset.hasSight && this.sightAsset.isPaintable)
					{
						if (this.sightAsset.albedoBase != null && this.skinAsset.attachmentSkin != null)
						{
							this.instantiatedSightSkin = Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
							this.sightAsset.applySkinBaseTextures(this.instantiatedSightSkin);
							this.skinAsset.SetMaterialProperties(this.instantiatedSightSkin);
							material = this.instantiatedSightSkin;
						}
						else if (this.skinAsset.tertiarySkin != null)
						{
							this.instantiatedSightSkin = Object.Instantiate<Material>(this.skinAsset.tertiarySkin);
							this.skinAsset.SetMaterialProperties(this.instantiatedSightSkin);
							material = this.instantiatedSightSkin;
						}
					}
					if (material != null)
					{
						HighlighterTool.rematerialize(this.sightModel, material, out this.tempSightMaterial);
					}
				}
			}
			if (this.tacticalModel != null)
			{
				Object.Destroy(this.tacticalModel.gameObject);
				this._tacticalModel = null;
			}
			try
			{
				this._tacticalAsset = (Assets.find(EAssetType.ITEM, this.tacticalID) as ItemTacticalAsset);
			}
			catch
			{
				this._tacticalAsset = null;
			}
			this.tempTacticalMaterial = null;
			if (this.tacticalAsset != null && this.tacticalHook != null && this.tacticalAsset.tactical != null)
			{
				this._tacticalModel = Object.Instantiate<GameObject>(this.tacticalAsset.tactical).transform;
				this.tacticalModel.name = this.tacticalAsset.instantiatedAttachmentName;
				this.tacticalModel.transform.parent = this.tacticalHook;
				this.tacticalModel.transform.localPosition = Vector3.zero;
				this.tacticalModel.transform.localRotation = Quaternion.identity;
				this.tacticalModel.localScale = Vector3.one;
				if (this.shouldDestroyColliders && this.tacticalAsset.shouldDestroyAttachmentColliders)
				{
					PrefabUtil.DestroyCollidersInChildren(this.tacticalModel.gameObject, true);
				}
				this.tacticalModel.DestroyRigidbody();
				if (viewmodel)
				{
					Layerer.viewmodel(this.tacticalModel);
				}
				if (this.gunAsset != null && this.skinAsset != null && this.skinAsset.secondarySkins != null)
				{
					Material material2 = null;
					if (!this.skinAsset.secondarySkins.TryGetValue(this.tacticalID, ref material2) && this.skinAsset.hasTactical && this.tacticalAsset.isPaintable)
					{
						if (this.tacticalAsset.albedoBase != null && this.skinAsset.attachmentSkin != null)
						{
							this.instantiatedTacticalSkin = Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
							this.tacticalAsset.applySkinBaseTextures(this.instantiatedTacticalSkin);
							this.skinAsset.SetMaterialProperties(this.instantiatedTacticalSkin);
							material2 = this.instantiatedTacticalSkin;
						}
						else if (this.skinAsset.tertiarySkin != null)
						{
							this.instantiatedTacticalSkin = Object.Instantiate<Material>(this.skinAsset.tertiarySkin);
							this.skinAsset.SetMaterialProperties(this.instantiatedTacticalSkin);
							material2 = this.instantiatedTacticalSkin;
						}
					}
					if (material2 != null)
					{
						HighlighterTool.rematerialize(this.tacticalModel, material2, out this.tempTacticalMaterial);
					}
				}
			}
			if (this.gripModel != null)
			{
				Object.Destroy(this.gripModel.gameObject);
				this._gripModel = null;
			}
			try
			{
				this._gripAsset = (Assets.find(EAssetType.ITEM, this.gripID) as ItemGripAsset);
			}
			catch
			{
				this._gripAsset = null;
			}
			this.tempGripMaterial = null;
			if (this.gripAsset != null && this.gripHook != null && this.gripAsset.grip != null)
			{
				this._gripModel = Object.Instantiate<GameObject>(this.gripAsset.grip).transform;
				this.gripModel.name = this.gripAsset.instantiatedAttachmentName;
				this.gripModel.transform.parent = this.gripHook;
				this.gripModel.transform.localPosition = Vector3.zero;
				this.gripModel.transform.localRotation = Quaternion.identity;
				this.gripModel.localScale = Vector3.one;
				if (this.shouldDestroyColliders && this.gripAsset.shouldDestroyAttachmentColliders)
				{
					PrefabUtil.DestroyCollidersInChildren(this.gripModel.gameObject, true);
				}
				this.gripModel.DestroyRigidbody();
				if (viewmodel)
				{
					Layerer.viewmodel(this.gripModel);
				}
				if (this.gunAsset != null && this.skinAsset != null && this.skinAsset.secondarySkins != null)
				{
					Material material3 = null;
					if (!this.skinAsset.secondarySkins.TryGetValue(this.gripID, ref material3) && this.skinAsset.hasGrip && this.gripAsset.isPaintable)
					{
						if (this.gripAsset.albedoBase != null && this.skinAsset.attachmentSkin != null)
						{
							this.instantiatedGripSkin = Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
							this.gripAsset.applySkinBaseTextures(this.instantiatedGripSkin);
							this.skinAsset.SetMaterialProperties(this.instantiatedGripSkin);
							material3 = this.instantiatedGripSkin;
						}
						else if (this.skinAsset.tertiarySkin != null)
						{
							this.instantiatedGripSkin = Object.Instantiate<Material>(this.skinAsset.tertiarySkin);
							this.skinAsset.SetMaterialProperties(this.instantiatedGripSkin);
							material3 = this.instantiatedGripSkin;
						}
					}
					if (material3 != null)
					{
						HighlighterTool.rematerialize(this.gripModel, material3, out this.tempGripMaterial);
					}
				}
			}
			if (this.barrelModel != null)
			{
				Object.Destroy(this.barrelModel.gameObject);
				this._barrelModel = null;
			}
			try
			{
				this._barrelAsset = (Assets.find(EAssetType.ITEM, this.barrelID) as ItemBarrelAsset);
			}
			catch
			{
				this._barrelAsset = null;
			}
			this.tempBarrelMaterial = null;
			if (this.barrelAsset != null && this.barrelHook != null && this.barrelAsset.barrel != null)
			{
				this._barrelModel = Object.Instantiate<GameObject>(this.barrelAsset.barrel).transform;
				this.barrelModel.name = this.barrelAsset.instantiatedAttachmentName;
				this.barrelModel.transform.parent = this.barrelHook;
				this.barrelModel.transform.localPosition = Vector3.zero;
				this.barrelModel.transform.localRotation = Quaternion.identity;
				this.barrelModel.localScale = Vector3.one;
				if (this.shouldDestroyColliders && this.barrelAsset.shouldDestroyAttachmentColliders)
				{
					PrefabUtil.DestroyCollidersInChildren(this.barrelModel.gameObject, true);
				}
				this.barrelModel.DestroyRigidbody();
				if (viewmodel)
				{
					Layerer.viewmodel(this.barrelModel);
				}
				if (this.gunAsset != null && this.skinAsset != null && this.skinAsset.secondarySkins != null)
				{
					Material material4 = null;
					if (!this.skinAsset.secondarySkins.TryGetValue(this.barrelID, ref material4) && this.skinAsset.hasBarrel && this.barrelAsset.isPaintable)
					{
						if (this.barrelAsset.albedoBase != null && this.skinAsset.attachmentSkin != null)
						{
							this.instantiatedBarrelSkin = Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
							this.barrelAsset.applySkinBaseTextures(this.instantiatedBarrelSkin);
							this.skinAsset.SetMaterialProperties(this.instantiatedBarrelSkin);
							material4 = this.instantiatedBarrelSkin;
						}
						else if (this.skinAsset.tertiarySkin != null)
						{
							this.instantiatedBarrelSkin = Object.Instantiate<Material>(this.skinAsset.tertiarySkin);
							this.skinAsset.SetMaterialProperties(this.instantiatedBarrelSkin);
							material4 = this.instantiatedBarrelSkin;
						}
					}
					if (material4 != null)
					{
						HighlighterTool.rematerialize(this.barrelModel, material4, out this.tempBarrelMaterial);
					}
				}
			}
			if (this.magazineModel != null)
			{
				Object.Destroy(this.magazineModel.gameObject);
				this._magazineModel = null;
			}
			try
			{
				this._magazineAsset = (Assets.find(EAssetType.ITEM, this.magazineID) as ItemMagazineAsset);
			}
			catch
			{
				this._magazineAsset = null;
			}
			this.tempMagazineMaterial = null;
			if (this.magazineAsset != null && this.magazineHook != null && this.magazineAsset.magazine != null)
			{
				Transform transform = null;
				if (this.magazineAsset.calibers.Length != 0)
				{
					transform = this.magazineHook.Find("Caliber_" + this.magazineAsset.calibers[0].ToString());
				}
				if (transform == null)
				{
					transform = this.magazineHook;
				}
				this._magazineModel = Object.Instantiate<GameObject>(this.magazineAsset.magazine).transform;
				this.magazineModel.name = this.magazineAsset.instantiatedAttachmentName;
				this.magazineModel.transform.parent = transform;
				this.magazineModel.transform.localPosition = Vector3.zero;
				this.magazineModel.transform.localRotation = Quaternion.identity;
				this.magazineModel.localScale = Vector3.one;
				if (this.shouldDestroyColliders && this.magazineAsset.shouldDestroyAttachmentColliders)
				{
					PrefabUtil.DestroyCollidersInChildren(this.magazineModel.gameObject, true);
				}
				this.magazineModel.DestroyRigidbody();
				if (viewmodel)
				{
					Layerer.viewmodel(this.magazineModel);
				}
				if (this.gunAsset != null && this.skinAsset != null && this.skinAsset.secondarySkins != null)
				{
					Material material5 = null;
					if (!this.skinAsset.secondarySkins.TryGetValue(this.magazineID, ref material5) && this.skinAsset.hasMagazine && this.magazineAsset.isPaintable)
					{
						if (this.magazineAsset.albedoBase != null && this.skinAsset.attachmentSkin != null)
						{
							this.instantiatedMagazineSkin = Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
							this.magazineAsset.applySkinBaseTextures(this.instantiatedMagazineSkin);
							this.skinAsset.SetMaterialProperties(this.instantiatedMagazineSkin);
							material5 = this.instantiatedMagazineSkin;
						}
						else if (this.skinAsset.tertiarySkin != null)
						{
							this.instantiatedMagazineSkin = Object.Instantiate<Material>(this.skinAsset.tertiarySkin);
							this.skinAsset.SetMaterialProperties(this.instantiatedMagazineSkin);
							material5 = this.instantiatedMagazineSkin;
						}
					}
					if (material5 != null)
					{
						HighlighterTool.rematerialize(this.magazineModel, material5, out this.tempMagazineMaterial);
					}
				}
			}
			if (this.tacticalModel != null && this.tacticalModel.childCount > 0)
			{
				Transform transform2 = this.tacticalModel.Find("Model_0");
				this._lightHook = ((transform2 != null) ? transform2.Find("Light") : null);
				this._light2Hook = ((transform2 != null) ? transform2.Find("Light2") : null);
				if (viewmodel)
				{
					if (this.lightHook != null)
					{
						this.lightHook.tag = "Viewmodel";
						this.lightHook.gameObject.layer = 11;
						Transform transform3 = this.lightHook.Find("Light");
						if (transform3 != null)
						{
							transform3.tag = "Viewmodel";
							transform3.gameObject.layer = 11;
						}
					}
					if (this.light2Hook != null)
					{
						this.light2Hook.tag = "Viewmodel";
						this.light2Hook.gameObject.layer = 11;
						Transform transform4 = this.light2Hook.Find("Light");
						if (transform4 != null)
						{
							transform4.tag = "Viewmodel";
							transform4.gameObject.layer = 11;
						}
					}
				}
				else
				{
					LightLODTool.applyLightLOD(this.lightHook);
					LightLODTool.applyLightLOD(this.light2Hook);
				}
			}
			else
			{
				this._lightHook = null;
				this._light2Hook = null;
			}
			if (this.sightModel != null)
			{
				Transform transform5 = this.sightModel.Find("Model_0");
				this._aimHook = ((transform5 != null) ? transform5.Find("Aim") : null);
				if (this.aimHook != null)
				{
					Transform transform6 = this.aimHook.parent.Find("Reticule");
					if (transform6 != null)
					{
						Renderer component = transform6.GetComponent<Renderer>();
						if (component != null)
						{
							this.reticuleMaterial = component.material;
							if (this.reticuleMaterial != null)
							{
								Color criticalHitmarkerColor = OptionsSettings.criticalHitmarkerColor;
								criticalHitmarkerColor.a = 1f;
								this.reticuleMaterial.SetColor("_Color", criticalHitmarkerColor);
								this.reticuleMaterial.SetColor("_EmissionColor", criticalHitmarkerColor);
							}
						}
					}
				}
				this._reticuleHook = ((transform5 != null) ? transform5.Find("Reticule") : null);
			}
			else
			{
				this._aimHook = null;
				this._reticuleHook = null;
			}
			if (this.aimHook != null)
			{
				this._scopeHook = this.aimHook.Find("Scope");
			}
			else
			{
				this._scopeHook = null;
			}
			if (this.rope != null && viewmodel)
			{
				this.rope.tag = "Viewmodel";
				this.rope.gameObject.layer = 11;
			}
			this.wasSkinned = true;
			this.applyVisual();
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x00045CB0 File Offset: 0x00043EB0
		private void Awake()
		{
			this._sightHook = base.transform.Find("Sight");
			this._viewHook = base.transform.Find("View");
			this._tacticalHook = base.transform.Find("Tactical");
			this._gripHook = base.transform.Find("Grip");
			this._barrelHook = base.transform.Find("Barrel");
			this._magazineHook = base.transform.Find("Magazine");
			this._ejectHook = base.transform.Find("Eject");
			this._leftHook = base.transform.Find("Left");
			this._rightHook = base.transform.Find("Right");
			this._nockHook = base.transform.Find("Nock");
			this._restHook = base.transform.Find("Rest");
			Transform transform = base.transform.Find("Rope");
			if (transform != null)
			{
				this._rope = (LineRenderer)transform.GetComponent<Renderer>();
			}
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x00045DDC File Offset: 0x00043FDC
		private void DestroySkinMaterials()
		{
			if (this.instantiatedSightSkin != null)
			{
				Object.Destroy(this.instantiatedSightSkin);
				this.instantiatedSightSkin = null;
			}
			if (this.instantiatedTacticalSkin != null)
			{
				Object.Destroy(this.instantiatedTacticalSkin);
				this.instantiatedTacticalSkin = null;
			}
			if (this.instantiatedGripSkin != null)
			{
				Object.Destroy(this.instantiatedGripSkin);
				this.instantiatedGripSkin = null;
			}
			if (this.instantiatedBarrelSkin != null)
			{
				Object.Destroy(this.instantiatedBarrelSkin);
				this.instantiatedBarrelSkin = null;
			}
			if (this.instantiatedMagazineSkin != null)
			{
				Object.Destroy(this.instantiatedMagazineSkin);
				this.instantiatedMagazineSkin = null;
			}
			if (this.reticuleMaterial != null)
			{
				Object.Destroy(this.reticuleMaterial);
				this.reticuleMaterial = null;
			}
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x00045EA9 File Offset: 0x000440A9
		private void OnDestroy()
		{
			this.DestroySkinMaterials();
		}

		// Token: 0x04000623 RID: 1571
		private ItemGunAsset _gunAsset;

		// Token: 0x04000624 RID: 1572
		private SkinAsset _skinAsset;

		// Token: 0x04000625 RID: 1573
		private ushort _sightID;

		// Token: 0x04000626 RID: 1574
		private ushort _tacticalID;

		// Token: 0x04000627 RID: 1575
		private ushort _gripID;

		// Token: 0x04000628 RID: 1576
		private ushort _barrelID;

		// Token: 0x04000629 RID: 1577
		private ushort _magazineID;

		// Token: 0x0400062A RID: 1578
		private ItemSightAsset _sightAsset;

		// Token: 0x0400062B RID: 1579
		private ItemTacticalAsset _tacticalAsset;

		// Token: 0x0400062C RID: 1580
		private ItemGripAsset _gripAsset;

		// Token: 0x0400062D RID: 1581
		private ItemBarrelAsset _barrelAsset;

		// Token: 0x0400062E RID: 1582
		private ItemMagazineAsset _magazineAsset;

		// Token: 0x0400062F RID: 1583
		private Transform _sightModel;

		// Token: 0x04000630 RID: 1584
		private Transform _tacticalModel;

		// Token: 0x04000631 RID: 1585
		private Transform _gripModel;

		// Token: 0x04000632 RID: 1586
		private Transform _barrelModel;

		// Token: 0x04000633 RID: 1587
		private Transform _magazineModel;

		// Token: 0x04000634 RID: 1588
		private Transform _sightHook;

		// Token: 0x04000635 RID: 1589
		private Transform _viewHook;

		// Token: 0x04000636 RID: 1590
		private Transform _tacticalHook;

		// Token: 0x04000637 RID: 1591
		private Transform _gripHook;

		// Token: 0x04000638 RID: 1592
		private Transform _barrelHook;

		// Token: 0x04000639 RID: 1593
		private Transform _magazineHook;

		// Token: 0x0400063A RID: 1594
		private Transform _ejectHook;

		// Token: 0x0400063B RID: 1595
		private Transform _lightHook;

		// Token: 0x0400063C RID: 1596
		private Transform _light2Hook;

		// Token: 0x0400063D RID: 1597
		private Transform _aimHook;

		// Token: 0x0400063E RID: 1598
		private Transform _scopeHook;

		// Token: 0x0400063F RID: 1599
		private Transform _reticuleHook;

		// Token: 0x04000640 RID: 1600
		private Transform _leftHook;

		// Token: 0x04000641 RID: 1601
		private Transform _rightHook;

		// Token: 0x04000642 RID: 1602
		private Transform _nockHook;

		// Token: 0x04000643 RID: 1603
		private Transform _restHook;

		// Token: 0x04000644 RID: 1604
		private LineRenderer _rope;

		// Token: 0x04000645 RID: 1605
		public bool isSkinned;

		// Token: 0x04000646 RID: 1606
		public bool shouldDestroyColliders;

		// Token: 0x04000647 RID: 1607
		private bool wasSkinned;

		// Token: 0x04000648 RID: 1608
		private Material tempSightMaterial;

		// Token: 0x04000649 RID: 1609
		private Material tempTacticalMaterial;

		// Token: 0x0400064A RID: 1610
		private Material tempGripMaterial;

		// Token: 0x0400064B RID: 1611
		private Material tempBarrelMaterial;

		// Token: 0x0400064C RID: 1612
		private Material tempMagazineMaterial;

		// Token: 0x0400064D RID: 1613
		private Material instantiatedSightSkin;

		// Token: 0x0400064E RID: 1614
		private Material instantiatedTacticalSkin;

		// Token: 0x0400064F RID: 1615
		private Material instantiatedGripSkin;

		// Token: 0x04000650 RID: 1616
		private Material instantiatedBarrelSkin;

		// Token: 0x04000651 RID: 1617
		private Material instantiatedMagazineSkin;

		// Token: 0x04000652 RID: 1618
		private Material reticuleMaterial;
	}
}
