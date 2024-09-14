using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200038F RID: 911
	public class HumanClothes : MonoBehaviour
	{
		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001C4D RID: 7245 RVA: 0x000651AF File Offset: 0x000633AF
		// (set) Token: 0x06001C4E RID: 7246 RVA: 0x000651B7 File Offset: 0x000633B7
		public Transform hatModel { get; private set; }

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001C4F RID: 7247 RVA: 0x000651C0 File Offset: 0x000633C0
		// (set) Token: 0x06001C50 RID: 7248 RVA: 0x000651C8 File Offset: 0x000633C8
		public Transform backpackModel { get; private set; }

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001C51 RID: 7249 RVA: 0x000651D1 File Offset: 0x000633D1
		// (set) Token: 0x06001C52 RID: 7250 RVA: 0x000651D9 File Offset: 0x000633D9
		public Transform vestModel { get; private set; }

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001C53 RID: 7251 RVA: 0x000651E2 File Offset: 0x000633E2
		// (set) Token: 0x06001C54 RID: 7252 RVA: 0x000651EA File Offset: 0x000633EA
		public Transform maskModel { get; private set; }

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001C55 RID: 7253 RVA: 0x000651F3 File Offset: 0x000633F3
		// (set) Token: 0x06001C56 RID: 7254 RVA: 0x000651FB File Offset: 0x000633FB
		public Transform glassesModel { get; private set; }

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001C57 RID: 7255 RVA: 0x00065204 File Offset: 0x00063404
		// (set) Token: 0x06001C58 RID: 7256 RVA: 0x0006520C File Offset: 0x0006340C
		public Transform hairModel { get; private set; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001C59 RID: 7257 RVA: 0x00065215 File Offset: 0x00063415
		// (set) Token: 0x06001C5A RID: 7258 RVA: 0x0006521D File Offset: 0x0006341D
		public Transform beardModel { get; private set; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001C5B RID: 7259 RVA: 0x00065226 File Offset: 0x00063426
		// (set) Token: 0x06001C5C RID: 7260 RVA: 0x0006522E File Offset: 0x0006342E
		public bool isVisual
		{
			get
			{
				return this._isVisual;
			}
			set
			{
				if (this.isVisual != value)
				{
					this._isVisual = value;
					this.markAllDirty(true);
				}
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001C5D RID: 7261 RVA: 0x00065247 File Offset: 0x00063447
		// (set) Token: 0x06001C5E RID: 7262 RVA: 0x0006524F File Offset: 0x0006344F
		public bool isMythic
		{
			get
			{
				return this._isMythic;
			}
			set
			{
				if (this.isMythic != value)
				{
					this._isMythic = value;
					this.markAllDirty(true);
				}
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001C5F RID: 7263 RVA: 0x00065268 File Offset: 0x00063468
		// (set) Token: 0x06001C60 RID: 7264 RVA: 0x00065270 File Offset: 0x00063470
		public bool hand
		{
			get
			{
				return this._isLeftHanded;
			}
			set
			{
				if (this._isLeftHanded != value)
				{
					this._isLeftHanded = value;
					this.markAllDirty(true);
				}
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001C61 RID: 7265 RVA: 0x00065289 File Offset: 0x00063489
		// (set) Token: 0x06001C62 RID: 7266 RVA: 0x00065291 File Offset: 0x00063491
		public bool hasBackpack
		{
			get
			{
				return this._hasBackpack;
			}
			set
			{
				if (value != this._hasBackpack)
				{
					this._hasBackpack = value;
					if (this.backpackModel != null)
					{
						this.backpackModel.gameObject.SetActive(this.hasBackpack);
					}
				}
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001C63 RID: 7267 RVA: 0x000652C7 File Offset: 0x000634C7
		// (set) Token: 0x06001C64 RID: 7268 RVA: 0x000652CF File Offset: 0x000634CF
		public int visualShirt
		{
			get
			{
				return this._visualShirt;
			}
			set
			{
				if (this.visualShirt != value)
				{
					this._visualShirt = value;
				}
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001C65 RID: 7269 RVA: 0x000652E1 File Offset: 0x000634E1
		// (set) Token: 0x06001C66 RID: 7270 RVA: 0x000652E9 File Offset: 0x000634E9
		public int visualPants
		{
			get
			{
				return this._visualPants;
			}
			set
			{
				if (this.visualPants != value)
				{
					this._visualPants = value;
				}
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001C67 RID: 7271 RVA: 0x000652FB File Offset: 0x000634FB
		// (set) Token: 0x06001C68 RID: 7272 RVA: 0x00065303 File Offset: 0x00063503
		public int visualHat
		{
			get
			{
				return this._visualHat;
			}
			set
			{
				if (this.visualHat != value)
				{
					this._visualHat = value;
				}
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001C69 RID: 7273 RVA: 0x00065315 File Offset: 0x00063515
		// (set) Token: 0x06001C6A RID: 7274 RVA: 0x0006531D File Offset: 0x0006351D
		public int visualBackpack
		{
			get
			{
				return this._visualBackpack;
			}
			set
			{
				if (this.visualBackpack != value)
				{
					this._visualBackpack = value;
				}
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x0006532F File Offset: 0x0006352F
		// (set) Token: 0x06001C6C RID: 7276 RVA: 0x00065337 File Offset: 0x00063537
		public int visualVest
		{
			get
			{
				return this._visualVest;
			}
			set
			{
				if (this.visualVest != value)
				{
					this._visualVest = value;
				}
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001C6D RID: 7277 RVA: 0x00065349 File Offset: 0x00063549
		// (set) Token: 0x06001C6E RID: 7278 RVA: 0x00065351 File Offset: 0x00063551
		public int visualMask
		{
			get
			{
				return this._visualMask;
			}
			set
			{
				if (this.visualMask != value)
				{
					this._visualMask = value;
				}
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001C6F RID: 7279 RVA: 0x00065363 File Offset: 0x00063563
		// (set) Token: 0x06001C70 RID: 7280 RVA: 0x0006536B File Offset: 0x0006356B
		public int visualGlasses
		{
			get
			{
				return this._visualGlasses;
			}
			set
			{
				if (this.visualGlasses != value)
				{
					this._visualGlasses = value;
				}
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001C71 RID: 7281 RVA: 0x0006537D File Offset: 0x0006357D
		// (set) Token: 0x06001C72 RID: 7282 RVA: 0x00065385 File Offset: 0x00063585
		public ItemShirtAsset shirtAsset
		{
			get
			{
				return this._shirtAsset;
			}
			internal set
			{
				this._shirtAsset = value;
				this.shirtDirty = true;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001C73 RID: 7283 RVA: 0x00065395 File Offset: 0x00063595
		// (set) Token: 0x06001C74 RID: 7284 RVA: 0x0006539D File Offset: 0x0006359D
		public ItemPantsAsset pantsAsset
		{
			get
			{
				return this._pantsAsset;
			}
			internal set
			{
				this._pantsAsset = value;
				this.pantsDirty = true;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001C75 RID: 7285 RVA: 0x000653AD File Offset: 0x000635AD
		// (set) Token: 0x06001C76 RID: 7286 RVA: 0x000653B5 File Offset: 0x000635B5
		public ItemHatAsset hatAsset
		{
			get
			{
				return this._hatAsset;
			}
			internal set
			{
				this._hatAsset = value;
				this.hatDirty = true;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001C77 RID: 7287 RVA: 0x000653C5 File Offset: 0x000635C5
		// (set) Token: 0x06001C78 RID: 7288 RVA: 0x000653CD File Offset: 0x000635CD
		public ItemBackpackAsset backpackAsset
		{
			get
			{
				return this._backpackAsset;
			}
			internal set
			{
				this._backpackAsset = value;
				this.backpackDirty = true;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001C79 RID: 7289 RVA: 0x000653DD File Offset: 0x000635DD
		// (set) Token: 0x06001C7A RID: 7290 RVA: 0x000653E5 File Offset: 0x000635E5
		public ItemVestAsset vestAsset
		{
			get
			{
				return this._vestAsset;
			}
			internal set
			{
				this._vestAsset = value;
				this.vestDirty = true;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001C7B RID: 7291 RVA: 0x000653F5 File Offset: 0x000635F5
		// (set) Token: 0x06001C7C RID: 7292 RVA: 0x000653FD File Offset: 0x000635FD
		public ItemMaskAsset maskAsset
		{
			get
			{
				return this._maskAsset;
			}
			internal set
			{
				this._maskAsset = value;
				this.maskDirty = true;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001C7D RID: 7293 RVA: 0x0006540D File Offset: 0x0006360D
		// (set) Token: 0x06001C7E RID: 7294 RVA: 0x00065415 File Offset: 0x00063615
		public ItemGlassesAsset glassesAsset
		{
			get
			{
				return this._glassesAsset;
			}
			internal set
			{
				this._glassesAsset = value;
				this.glassesDirty = true;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001C7F RID: 7295 RVA: 0x00065425 File Offset: 0x00063625
		// (set) Token: 0x06001C80 RID: 7296 RVA: 0x0006543C File Offset: 0x0006363C
		public Guid shirtGuid
		{
			get
			{
				ItemShirtAsset shirtAsset = this._shirtAsset;
				if (shirtAsset == null)
				{
					return Guid.Empty;
				}
				return shirtAsset.GUID;
			}
			set
			{
				this._shirtAsset = (Assets.find(value) as ItemShirtAsset);
				this.shirtDirty = true;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001C81 RID: 7297 RVA: 0x00065456 File Offset: 0x00063656
		// (set) Token: 0x06001C82 RID: 7298 RVA: 0x00065469 File Offset: 0x00063669
		public ushort shirt
		{
			get
			{
				ItemShirtAsset shirtAsset = this._shirtAsset;
				if (shirtAsset == null)
				{
					return 0;
				}
				return shirtAsset.id;
			}
			set
			{
				this._shirtAsset = (Assets.find(EAssetType.ITEM, value) as ItemShirtAsset);
				this.shirtDirty = true;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001C83 RID: 7299 RVA: 0x00065484 File Offset: 0x00063684
		// (set) Token: 0x06001C84 RID: 7300 RVA: 0x0006549B File Offset: 0x0006369B
		public Guid pantsGuid
		{
			get
			{
				ItemPantsAsset pantsAsset = this._pantsAsset;
				if (pantsAsset == null)
				{
					return Guid.Empty;
				}
				return pantsAsset.GUID;
			}
			set
			{
				this._pantsAsset = (Assets.find(value) as ItemPantsAsset);
				this.pantsDirty = true;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001C85 RID: 7301 RVA: 0x000654B5 File Offset: 0x000636B5
		// (set) Token: 0x06001C86 RID: 7302 RVA: 0x000654C8 File Offset: 0x000636C8
		public ushort pants
		{
			get
			{
				ItemPantsAsset pantsAsset = this._pantsAsset;
				if (pantsAsset == null)
				{
					return 0;
				}
				return pantsAsset.id;
			}
			set
			{
				this._pantsAsset = (Assets.find(EAssetType.ITEM, value) as ItemPantsAsset);
				this.pantsDirty = true;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001C87 RID: 7303 RVA: 0x000654E3 File Offset: 0x000636E3
		// (set) Token: 0x06001C88 RID: 7304 RVA: 0x000654FA File Offset: 0x000636FA
		public Guid hatGuid
		{
			get
			{
				ItemHatAsset hatAsset = this._hatAsset;
				if (hatAsset == null)
				{
					return Guid.Empty;
				}
				return hatAsset.GUID;
			}
			set
			{
				this._hatAsset = (Assets.find(value) as ItemHatAsset);
				this.hatDirty = true;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001C89 RID: 7305 RVA: 0x00065514 File Offset: 0x00063714
		// (set) Token: 0x06001C8A RID: 7306 RVA: 0x00065527 File Offset: 0x00063727
		public ushort hat
		{
			get
			{
				ItemHatAsset hatAsset = this._hatAsset;
				if (hatAsset == null)
				{
					return 0;
				}
				return hatAsset.id;
			}
			set
			{
				this._hatAsset = (Assets.find(EAssetType.ITEM, value) as ItemHatAsset);
				this.hatDirty = true;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001C8B RID: 7307 RVA: 0x00065542 File Offset: 0x00063742
		// (set) Token: 0x06001C8C RID: 7308 RVA: 0x00065559 File Offset: 0x00063759
		public Guid backpackGuid
		{
			get
			{
				ItemBackpackAsset backpackAsset = this._backpackAsset;
				if (backpackAsset == null)
				{
					return Guid.Empty;
				}
				return backpackAsset.GUID;
			}
			set
			{
				this._backpackAsset = (Assets.find(value) as ItemBackpackAsset);
				this.backpackDirty = true;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001C8D RID: 7309 RVA: 0x00065573 File Offset: 0x00063773
		// (set) Token: 0x06001C8E RID: 7310 RVA: 0x00065586 File Offset: 0x00063786
		public ushort backpack
		{
			get
			{
				ItemBackpackAsset backpackAsset = this._backpackAsset;
				if (backpackAsset == null)
				{
					return 0;
				}
				return backpackAsset.id;
			}
			set
			{
				this._backpackAsset = (Assets.find(EAssetType.ITEM, value) as ItemBackpackAsset);
				this.backpackDirty = true;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001C8F RID: 7311 RVA: 0x000655A1 File Offset: 0x000637A1
		// (set) Token: 0x06001C90 RID: 7312 RVA: 0x000655B8 File Offset: 0x000637B8
		public Guid vestGuid
		{
			get
			{
				ItemVestAsset vestAsset = this._vestAsset;
				if (vestAsset == null)
				{
					return Guid.Empty;
				}
				return vestAsset.GUID;
			}
			set
			{
				this._vestAsset = (Assets.find(value) as ItemVestAsset);
				this.vestDirty = true;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x000655D2 File Offset: 0x000637D2
		// (set) Token: 0x06001C92 RID: 7314 RVA: 0x000655E5 File Offset: 0x000637E5
		public ushort vest
		{
			get
			{
				ItemVestAsset vestAsset = this._vestAsset;
				if (vestAsset == null)
				{
					return 0;
				}
				return vestAsset.id;
			}
			set
			{
				this._vestAsset = (Assets.find(EAssetType.ITEM, value) as ItemVestAsset);
				this.vestDirty = true;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x00065600 File Offset: 0x00063800
		// (set) Token: 0x06001C94 RID: 7316 RVA: 0x00065617 File Offset: 0x00063817
		public Guid maskGuid
		{
			get
			{
				ItemMaskAsset maskAsset = this._maskAsset;
				if (maskAsset == null)
				{
					return Guid.Empty;
				}
				return maskAsset.GUID;
			}
			set
			{
				this._maskAsset = (Assets.find(value) as ItemMaskAsset);
				this.maskDirty = true;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x00065631 File Offset: 0x00063831
		// (set) Token: 0x06001C96 RID: 7318 RVA: 0x00065644 File Offset: 0x00063844
		public ushort mask
		{
			get
			{
				ItemMaskAsset maskAsset = this._maskAsset;
				if (maskAsset == null)
				{
					return 0;
				}
				return maskAsset.id;
			}
			set
			{
				this._maskAsset = (Assets.find(EAssetType.ITEM, value) as ItemMaskAsset);
				this.maskDirty = true;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x0006565F File Offset: 0x0006385F
		// (set) Token: 0x06001C98 RID: 7320 RVA: 0x00065676 File Offset: 0x00063876
		public Guid glassesGuid
		{
			get
			{
				ItemGlassesAsset glassesAsset = this._glassesAsset;
				if (glassesAsset == null)
				{
					return Guid.Empty;
				}
				return glassesAsset.GUID;
			}
			set
			{
				this._glassesAsset = (Assets.find(value) as ItemGlassesAsset);
				this.glassesDirty = true;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001C99 RID: 7321 RVA: 0x00065690 File Offset: 0x00063890
		// (set) Token: 0x06001C9A RID: 7322 RVA: 0x000656A3 File Offset: 0x000638A3
		public ushort glasses
		{
			get
			{
				ItemGlassesAsset glassesAsset = this._glassesAsset;
				if (glassesAsset == null)
				{
					return 0;
				}
				return glassesAsset.id;
			}
			set
			{
				this._glassesAsset = (Assets.find(EAssetType.ITEM, value) as ItemGlassesAsset);
				this.glassesDirty = true;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x000656BE File Offset: 0x000638BE
		// (set) Token: 0x06001C9C RID: 7324 RVA: 0x000656C6 File Offset: 0x000638C6
		public byte face
		{
			get
			{
				return this._face;
			}
			set
			{
				if (this.face != value)
				{
					this._face = value;
					this.faceDirty = true;
				}
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x000656DF File Offset: 0x000638DF
		// (set) Token: 0x06001C9E RID: 7326 RVA: 0x000656E7 File Offset: 0x000638E7
		public byte hair
		{
			get
			{
				return this._hair;
			}
			set
			{
				if (this.hair != value)
				{
					this._hair = value;
					this.hairDirty = true;
				}
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001C9F RID: 7327 RVA: 0x00065700 File Offset: 0x00063900
		// (set) Token: 0x06001CA0 RID: 7328 RVA: 0x00065708 File Offset: 0x00063908
		public byte beard
		{
			get
			{
				return this._beard;
			}
			set
			{
				if (this.beard != value)
				{
					this._beard = value;
					this.beardDirty = true;
				}
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x00065721 File Offset: 0x00063921
		// (set) Token: 0x06001CA2 RID: 7330 RVA: 0x00065729 File Offset: 0x00063929
		public Color skin
		{
			get
			{
				return this._skinColor;
			}
			set
			{
				this._skinColor = value;
				this.skinColorDirty = true;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x00065739 File Offset: 0x00063939
		// (set) Token: 0x06001CA4 RID: 7332 RVA: 0x00065741 File Offset: 0x00063941
		public Color color
		{
			get
			{
				return this._hairColor;
			}
			set
			{
				this._hairColor = value;
			}
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x0006574C File Offset: 0x0006394C
		private void markAllDirty(bool isDirty)
		{
			this.hairDirty = isDirty;
			this.beardDirty = isDirty;
			this.skinColorDirty = isDirty;
			this.faceDirty = isDirty;
			this.shirtDirty = isDirty;
			this.pantsDirty = isDirty;
			this.hatDirty = isDirty;
			this.backpackDirty = isDirty;
			this.vestDirty = isDirty;
			this.maskDirty = isDirty;
			this.glassesDirty = isDirty;
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000657A8 File Offset: 0x000639A8
		private void ApplyHairOverride(ItemGearAsset itemAsset, Transform rootModel)
		{
			if (string.IsNullOrEmpty(itemAsset.hairOverride))
			{
				return;
			}
			Transform transform = rootModel.FindChildRecursive(itemAsset.hairOverride);
			if (transform == null)
			{
				Assets.reportError(itemAsset, "cannot find hair override \"{0}\"", itemAsset.hairOverride);
				return;
			}
			Renderer component = transform.GetComponent<Renderer>();
			if (component != null)
			{
				component.sharedMaterial = this.materialHair;
				return;
			}
			Assets.reportError(itemAsset, "hair override \"{0}\" does not have a renderer component", itemAsset.hairOverride);
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0006581C File Offset: 0x00063A1C
		private void ApplySkinOverride(ItemClothingAsset itemAsset, Transform rootModel)
		{
			if (string.IsNullOrEmpty(itemAsset.skinOverride))
			{
				return;
			}
			Transform transform = rootModel.FindChildRecursive(itemAsset.skinOverride);
			if (transform == null)
			{
				Assets.reportError(itemAsset, "cannot find skin override \"{0}\"", itemAsset.skinOverride);
				return;
			}
			Renderer component = transform.GetComponent<Renderer>();
			if (component != null)
			{
				component.sharedMaterial = this.materialClothing;
				return;
			}
			Assets.reportError(itemAsset, "skin override \"{0}\" does not have a renderer component", itemAsset.skinOverride);
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x00065890 File Offset: 0x00063A90
		public void apply()
		{
		}

		/// <summary>
		/// Center mythical effect hook horizontally, but maintain vertical placement.
		/// Lots of hats/masks/glasses have off-center effects intentionally, but community
		/// feedback suggests centering to make effects like circling atoms look better.
		/// </summary>
		// Token: 0x06001CA9 RID: 7337 RVA: 0x000658A0 File Offset: 0x00063AA0
		private void centerHeadEffect(Transform skull, Transform model)
		{
			Transform transform = model.Find("Effect");
			if (transform == null)
			{
				transform = new GameObject("Effect").transform;
				transform.parent = model;
				transform.localPosition = new Vector3(-0.45f, 0f, 0f);
				transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
				transform.localScale = Vector3.one;
				return;
			}
			Vector3 localPosition = transform.localPosition;
			localPosition.y = 0f;
			localPosition.z = 0f;
			transform.localPosition = localPosition;
		}

		/// <summary>
		/// Set mesh of all character mesh renderers.
		/// Tries to match renderer index to mesh LOD index.
		/// </summary>
		// Token: 0x06001CAA RID: 7338 RVA: 0x00065940 File Offset: 0x00063B40
		private void setCharacterMeshes(Mesh[] meshes)
		{
			if (meshes == null || meshes.Length < 1)
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.characterMeshRenderers)
				{
					if (!(skinnedMeshRenderer == null))
					{
						skinnedMeshRenderer.sharedMesh = null;
					}
				}
				return;
			}
			int num = 0;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in this.characterMeshRenderers)
			{
				if (!(skinnedMeshRenderer2 == null))
				{
					if (num < meshes.Length)
					{
						skinnedMeshRenderer2.sharedMesh = meshes[num];
					}
					else
					{
						skinnedMeshRenderer2.sharedMesh = meshes[meshes.Length - 1];
					}
					num++;
				}
			}
		}

		/// <summary>
		/// Set material of all character mesh renderers.
		/// </summary>
		// Token: 0x06001CAB RID: 7339 RVA: 0x000659CC File Offset: 0x00063BCC
		private void setCharacterMaterial(Material material)
		{
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.characterMeshRenderers)
			{
				if (!(skinnedMeshRenderer == null))
				{
					skinnedMeshRenderer.sharedMaterial = material;
				}
			}
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x00065A04 File Offset: 0x00063C04
		private void Awake()
		{
			this.spine = base.transform.Find("Skeleton").Find("Spine");
			this.skull = this.spine.Find("Skull");
			this.upperBones = new Transform[]
			{
				this.spine,
				this.spine.Find("Left_Shoulder/Left_Arm"),
				this.spine.Find("Left_Shoulder/Left_Arm/Left_Hand"),
				this.spine.Find("Right_Shoulder/Right_Arm"),
				this.spine.Find("Right_Shoulder/Right_Arm/Right_Hand")
			};
			this.upperSystems = new MythicalEffectController[this.upperBones.Length];
			this.lowerBones = new Transform[]
			{
				this.spine.parent.Find("Left_Hip/Left_Leg"),
				this.spine.parent.Find("Left_Hip/Left_Leg/Left_Foot"),
				this.spine.parent.Find("Right_Hip/Right_Leg"),
				this.spine.parent.Find("Right_Hip/Right_Leg/Right_Foot")
			};
			this.lowerSystems = new MythicalEffectController[this.lowerBones.Length];
			Object x = base.transform.Find("Model_0");
			Transform x2 = base.transform.Find("Model_1");
			this.characterMeshRenderers = new SkinnedMeshRenderer[(x2 == null) ? 1 : 2];
			if (x != null)
			{
				this.characterMeshRenderers[0] = base.transform.Find("Model_0").GetComponent<SkinnedMeshRenderer>();
			}
			if (x2 != null)
			{
				this.characterMeshRenderers[1] = base.transform.Find("Model_1").GetComponent<SkinnedMeshRenderer>();
			}
			this.setCharacterMaterial(this.materialClothing);
			this.markAllDirty(true);
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x00065BD4 File Offset: 0x00063DD4
		private void OnDestroy()
		{
			if (this.materialClothing != null)
			{
				Object.DestroyImmediate(this.materialClothing);
				this.materialClothing = null;
			}
			if (this.materialHair != null)
			{
				Object.DestroyImmediate(this.materialHair);
				this.materialHair = null;
			}
		}

		// Token: 0x04000D6A RID: 3434
		private static Shader shader;

		// Token: 0x04000D6B RID: 3435
		private static Shader clothingShader;

		// Token: 0x04000D6C RID: 3436
		private Mesh[] humanMeshes;

		// Token: 0x04000D6D RID: 3437
		private Material materialClothing;

		// Token: 0x04000D6E RID: 3438
		private Material materialHair;

		// Token: 0x04000D6F RID: 3439
		private Transform spine;

		// Token: 0x04000D70 RID: 3440
		private Transform skull;

		// Token: 0x04000D71 RID: 3441
		private Transform[] upperBones;

		// Token: 0x04000D72 RID: 3442
		private MythicalEffectController[] upperSystems;

		// Token: 0x04000D73 RID: 3443
		private Transform[] lowerBones;

		// Token: 0x04000D74 RID: 3444
		private MythicalEffectController[] lowerSystems;

		// Token: 0x04000D7C RID: 3452
		public bool isMine;

		// Token: 0x04000D7D RID: 3453
		public bool isView;

		// Token: 0x04000D7E RID: 3454
		public bool canWearPro;

		// Token: 0x04000D7F RID: 3455
		public bool isRagdoll;

		// Token: 0x04000D80 RID: 3456
		private SkinnedMeshRenderer[] characterMeshRenderers;

		// Token: 0x04000D81 RID: 3457
		private bool _isVisual = true;

		// Token: 0x04000D82 RID: 3458
		private bool _isMythic = true;

		// Token: 0x04000D83 RID: 3459
		private bool _isLeftHanded;

		// Token: 0x04000D84 RID: 3460
		private bool _hasBackpack = true;

		// Token: 0x04000D85 RID: 3461
		private bool isUpper;

		// Token: 0x04000D86 RID: 3462
		private bool isLower;

		// Token: 0x04000D87 RID: 3463
		private ItemShirtAsset visualShirtAsset;

		// Token: 0x04000D88 RID: 3464
		private ItemPantsAsset visualPantsAsset;

		// Token: 0x04000D89 RID: 3465
		private ItemHatAsset visualHatAsset;

		// Token: 0x04000D8A RID: 3466
		private ItemBackpackAsset visualBackpackAsset;

		// Token: 0x04000D8B RID: 3467
		private ItemVestAsset visualVestAsset;

		// Token: 0x04000D8C RID: 3468
		private ItemMaskAsset visualMaskAsset;

		// Token: 0x04000D8D RID: 3469
		private ItemGlassesAsset visualGlassesAsset;

		// Token: 0x04000D8E RID: 3470
		private int _visualShirt;

		// Token: 0x04000D8F RID: 3471
		private int _visualPants;

		// Token: 0x04000D90 RID: 3472
		private int _visualHat;

		// Token: 0x04000D91 RID: 3473
		public int _visualBackpack;

		// Token: 0x04000D92 RID: 3474
		public int _visualVest;

		// Token: 0x04000D93 RID: 3475
		public int _visualMask;

		// Token: 0x04000D94 RID: 3476
		public int _visualGlasses;

		// Token: 0x04000D95 RID: 3477
		private ItemShirtAsset _shirtAsset;

		// Token: 0x04000D96 RID: 3478
		private ItemPantsAsset _pantsAsset;

		// Token: 0x04000D97 RID: 3479
		private ItemHatAsset _hatAsset;

		// Token: 0x04000D98 RID: 3480
		private ItemBackpackAsset _backpackAsset;

		// Token: 0x04000D99 RID: 3481
		private ItemVestAsset _vestAsset;

		// Token: 0x04000D9A RID: 3482
		private ItemMaskAsset _maskAsset;

		// Token: 0x04000D9B RID: 3483
		private ItemGlassesAsset _glassesAsset;

		// Token: 0x04000D9C RID: 3484
		private byte _face = byte.MaxValue;

		// Token: 0x04000D9D RID: 3485
		private byte _hair;

		// Token: 0x04000D9E RID: 3486
		private byte _beard;

		// Token: 0x04000D9F RID: 3487
		private Color _skinColor;

		// Token: 0x04000DA0 RID: 3488
		private Color _hairColor;

		// Token: 0x04000DA1 RID: 3489
		private bool hasHair;

		// Token: 0x04000DA2 RID: 3490
		private bool hasBeard;

		// Token: 0x04000DA3 RID: 3491
		private bool usingHumanMeshes = true;

		// Token: 0x04000DA4 RID: 3492
		private bool usingHumanMaterials = true;

		// Token: 0x04000DA5 RID: 3493
		private bool hairDirty;

		// Token: 0x04000DA6 RID: 3494
		private bool beardDirty;

		// Token: 0x04000DA7 RID: 3495
		private bool skinColorDirty;

		// Token: 0x04000DA8 RID: 3496
		private bool faceDirty;

		// Token: 0x04000DA9 RID: 3497
		private bool shirtDirty;

		// Token: 0x04000DAA RID: 3498
		private bool pantsDirty;

		// Token: 0x04000DAB RID: 3499
		private bool hatDirty;

		// Token: 0x04000DAC RID: 3500
		private bool backpackDirty;

		// Token: 0x04000DAD RID: 3501
		private bool vestDirty;

		// Token: 0x04000DAE RID: 3502
		private bool maskDirty;

		// Token: 0x04000DAF RID: 3503
		private bool glassesDirty;

		// Token: 0x04000DB0 RID: 3504
		internal static readonly int skinColorPropertyID = Shader.PropertyToID("_SkinColor");

		// Token: 0x04000DB1 RID: 3505
		internal static readonly int flipShirtPropertyID = Shader.PropertyToID("_FlipShirt");

		// Token: 0x04000DB2 RID: 3506
		internal static readonly int faceAlbedoTexturePropertyID = Shader.PropertyToID("_FaceAlbedoTexture");

		// Token: 0x04000DB3 RID: 3507
		internal static readonly int faceEmissionTexturePropertyID = Shader.PropertyToID("_FaceEmissionTexture");

		// Token: 0x04000DB4 RID: 3508
		internal static readonly int shirtAlbedoTexturePropertyID = Shader.PropertyToID("_ShirtAlbedoTexture");

		// Token: 0x04000DB5 RID: 3509
		internal static readonly int shirtEmissionTexturePropertyID = Shader.PropertyToID("_ShirtEmissionTexture");

		// Token: 0x04000DB6 RID: 3510
		internal static readonly int shirtMetallicTexturePropertyID = Shader.PropertyToID("_ShirtMetallicTexture");

		// Token: 0x04000DB7 RID: 3511
		internal static readonly int pantsAlbedoTexturePropertyID = Shader.PropertyToID("_PantsAlbedoTexture");

		// Token: 0x04000DB8 RID: 3512
		internal static readonly int pantsEmissionTexturePropertyID = Shader.PropertyToID("_PantsEmissionTexture");

		// Token: 0x04000DB9 RID: 3513
		internal static readonly int pantsMetallicTexturePropertyID = Shader.PropertyToID("_PantsMetallicTexture");
	}
}
