using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002EA RID: 746
	public class ItemGunAsset : ItemWeaponAsset
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x0600161F RID: 5663 RVA: 0x00051B52 File Offset: 0x0004FD52
		public AudioClip shoot
		{
			get
			{
				return this._shoot;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001620 RID: 5664 RVA: 0x00051B5A File Offset: 0x0004FD5A
		public AudioClip reload
		{
			get
			{
				return this._reload;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001621 RID: 5665 RVA: 0x00051B62 File Offset: 0x0004FD62
		public AudioClip hammer
		{
			get
			{
				return this._hammer;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001622 RID: 5666 RVA: 0x00051B6A File Offset: 0x0004FD6A
		public AudioClip aim
		{
			get
			{
				return this._aim;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001623 RID: 5667 RVA: 0x00051B72 File Offset: 0x0004FD72
		public AudioClip minigun
		{
			get
			{
				return this._minigun;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001624 RID: 5668 RVA: 0x00051B7A File Offset: 0x0004FD7A
		public AudioClip chamberJammedSound
		{
			get
			{
				return this._chamberJammedSound;
			}
		}

		/// <summary>
		/// Sound to play when input is pressed but weapon has a fire delay.
		/// </summary>
		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001625 RID: 5669 RVA: 0x00051B82 File Offset: 0x0004FD82
		// (set) Token: 0x06001626 RID: 5670 RVA: 0x00051B8A File Offset: 0x0004FD8A
		public AudioClip fireDelaySound { get; protected set; }

		/// <summary>
		/// Maximum distance the gunshot can be heard.
		/// </summary>
		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001627 RID: 5671 RVA: 0x00051B93 File Offset: 0x0004FD93
		// (set) Token: 0x06001628 RID: 5672 RVA: 0x00051B9B File Offset: 0x0004FD9B
		public float gunshotRolloffDistance { get; protected set; }

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001629 RID: 5673 RVA: 0x00051BA4 File Offset: 0x0004FDA4
		public GameObject projectile
		{
			get
			{
				return this._projectile;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x0600162A RID: 5674 RVA: 0x00051BAC File Offset: 0x0004FDAC
		public override bool shouldFriendlySentryTargetUser
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Override Rangefinder attachment's maximum range.
		/// Defaults to range value.
		/// </summary>
		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x0600162B RID: 5675 RVA: 0x00051BAF File Offset: 0x0004FDAF
		// (set) Token: 0x0600162C RID: 5676 RVA: 0x00051BB7 File Offset: 0x0004FDB7
		public float rangeRangefinder { get; protected set; }

		/// <summary>
		/// Can this weapon instantly kill players by headshots?
		/// Only valid when game config also enables this.
		/// </summary>
		// Token: 0x170003AA RID: 938
		// (get) Token: 0x0600162D RID: 5677 RVA: 0x00051BC0 File Offset: 0x0004FDC0
		// (set) Token: 0x0600162E RID: 5678 RVA: 0x00051BC8 File Offset: 0x0004FDC8
		public bool instakillHeadshots { get; protected set; }

		/// <summary>
		/// Can this weapon be fired without consuming ammo?
		/// Some mods use this for turrets.
		/// </summary>
		// Token: 0x170003AB RID: 939
		// (get) Token: 0x0600162F RID: 5679 RVA: 0x00051BD1 File Offset: 0x0004FDD1
		// (set) Token: 0x06001630 RID: 5680 RVA: 0x00051BD9 File Offset: 0x0004FDD9
		public bool infiniteAmmo { get; protected set; }

		/// <summary>
		/// Ammo quantity to consume per shot fired.
		/// </summary>
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001631 RID: 5681 RVA: 0x00051BE2 File Offset: 0x0004FDE2
		// (set) Token: 0x06001632 RID: 5682 RVA: 0x00051BEA File Offset: 0x0004FDEA
		public byte ammoPerShot { get; protected set; }

		/// <summary>
		/// Simulation steps to wait after input before firing.
		/// </summary>
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001633 RID: 5683 RVA: 0x00051BF3 File Offset: 0x0004FDF3
		// (set) Token: 0x06001634 RID: 5684 RVA: 0x00051BFB File Offset: 0x0004FDFB
		public int fireDelay { get; protected set; }

		/// <summary>
		/// Can magazine be changed by player?
		/// </summary>
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001635 RID: 5685 RVA: 0x00051C04 File Offset: 0x0004FE04
		// (set) Token: 0x06001636 RID: 5686 RVA: 0x00051C0C File Offset: 0x0004FE0C
		public bool allowMagazineChange { get; protected set; }

		/// <summary>
		/// Can player ADS while sprinting and vice versa?
		/// </summary>
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001637 RID: 5687 RVA: 0x00051C15 File Offset: 0x0004FE15
		// (set) Token: 0x06001638 RID: 5688 RVA: 0x00051C1D File Offset: 0x0004FE1D
		public bool canAimDuringSprint { get; protected set; }

		/// <summary>
		/// Seconds from pressing "aim" to fully aiming down sights.
		/// </summary>
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001639 RID: 5689 RVA: 0x00051C26 File Offset: 0x0004FE26
		// (set) Token: 0x0600163A RID: 5690 RVA: 0x00051C2E File Offset: 0x0004FE2E
		public float aimInDuration { get; protected set; }

		/// <summary>
		/// If true, Aim_Start and Aim_Stop animations are scaled according to actual aim duration.
		/// </summary>
		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x0600163B RID: 5691 RVA: 0x00051C37 File Offset: 0x0004FE37
		// (set) Token: 0x0600163C RID: 5692 RVA: 0x00051C3F File Offset: 0x0004FE3F
		public bool shouldScaleAimAnimations { get; protected set; }

		// Token: 0x0600163D RID: 5693 RVA: 0x00051C48 File Offset: 0x0004FE48
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			ushort id = BitConverter.ToUInt16(itemInstance.state, 8);
			ItemMagazineAsset itemMagazineAsset = Assets.find(EAssetType.ITEM, id) as ItemMagazineAsset;
			if (itemMagazineAsset != null)
			{
				if (!string.IsNullOrEmpty(itemMagazineAsset.itemName))
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("Ammo", string.Concat(new string[]
					{
						"<color=",
						Palette.hex(ItemTool.getRarityColorUI(itemMagazineAsset.rarity)),
						">",
						itemMagazineAsset.itemName,
						"</color>"
					}), itemInstance.state[10], itemMagazineAsset.amount), 2000);
				}
				else
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("Ammo", string.Concat(new string[]
					{
						"<color=",
						Palette.hex(ItemTool.getRarityColorUI(this.rarity)),
						">",
						base.itemName,
						"</color>"
					}), itemInstance.state[10], itemMagazineAsset.amount), 2000);
				}
			}
			else
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("Ammo", PlayerDashboardInventoryUI.localization.format("None"), 0, 0), 2000);
			}
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			ushort num = BitConverter.ToUInt16(itemInstance.state, 0);
			ushort num2 = BitConverter.ToUInt16(itemInstance.state, 2);
			ushort num3 = BitConverter.ToUInt16(itemInstance.state, 4);
			ushort num4 = BitConverter.ToUInt16(itemInstance.state, 6);
			ItemSightAsset itemSightAsset = Assets.find(EAssetType.ITEM, num) as ItemSightAsset;
			ItemTacticalAsset itemTacticalAsset = Assets.find(EAssetType.ITEM, num2) as ItemTacticalAsset;
			ItemGripAsset itemGripAsset = Assets.find(EAssetType.ITEM, num3) as ItemGripAsset;
			ItemBarrelAsset itemBarrelAsset = Assets.find(EAssetType.ITEM, num4) as ItemBarrelAsset;
			if (itemSightAsset != null && (this.hasSight || num != this.sightID))
			{
				if (!string.IsNullOrEmpty(itemSightAsset.itemName))
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_SightAttachment", string.Concat(new string[]
					{
						"<color=",
						Palette.hex(ItemTool.getRarityColorUI(itemSightAsset.rarity)),
						">",
						itemSightAsset.itemName,
						"</color>"
					})), 2000);
				}
			}
			else if (this.hasSight)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_SightAttachment", PlayerDashboardInventoryUI.localization.format("None")), 2000);
			}
			if (itemTacticalAsset != null && (this.hasTactical || num2 != this.tacticalID))
			{
				if (!string.IsNullOrEmpty(itemTacticalAsset.itemName))
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_TacticalAttachment", string.Concat(new string[]
					{
						"<color=",
						Palette.hex(ItemTool.getRarityColorUI(itemTacticalAsset.rarity)),
						">",
						itemTacticalAsset.itemName,
						"</color>"
					})), 2000);
				}
			}
			else if (this.hasTactical)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_TacticalAttachment", PlayerDashboardInventoryUI.localization.format("None")), 2000);
			}
			if (itemGripAsset != null && (this.hasGrip || num3 != this.gripID))
			{
				if (!string.IsNullOrEmpty(itemGripAsset.itemName))
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_GripAttachment", string.Concat(new string[]
					{
						"<color=",
						Palette.hex(ItemTool.getRarityColorUI(itemGripAsset.rarity)),
						">",
						itemGripAsset.itemName,
						"</color>"
					})), 2000);
				}
			}
			else if (this.hasGrip)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_GripAttachment", PlayerDashboardInventoryUI.localization.format("None")), 2000);
			}
			if (itemBarrelAsset != null && (this.hasBarrel || num4 != this.barrelID))
			{
				if (!string.IsNullOrEmpty(itemBarrelAsset.itemName))
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_BarrelAttachment", string.Concat(new string[]
					{
						"<color=",
						Palette.hex(ItemTool.getRarityColorUI(itemBarrelAsset.rarity)),
						">",
						itemBarrelAsset.itemName,
						"</color>"
					})), 2000);
				}
			}
			else if (this.hasBarrel)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_BarrelAttachment", PlayerDashboardInventoryUI.localization.format("None")), 2000);
			}
			float f = 50f / (float)Mathf.Max(1, (int)(this.firerate + 1)) * 60f;
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Firerate", Mathf.RoundToInt(f)), 10000);
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Spread", string.Format("{0:N1}", 57.29578f * this.baseSpreadAngleRadians)), 10000);
			if (this.spreadAim != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Spread_Aim", string.Format("{0:N1}", 57.29578f * this.baseSpreadAngleRadians * this.spreadAim)), 10000);
			}
			if (this.aimingRecoilMultiplier != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_RecoilModifier_Aiming", PlayerDashboardInventoryUI.FormatStatModifier(this.aimingRecoilMultiplier, false, false)), 10000 + base.DescSort_LowerIsBeneficial(this.aimingRecoilMultiplier));
			}
			if (this.damageFalloffRange != 1f && this.damageFalloffMultiplier != 1f)
			{
				string arg = MeasurementTool.FormatLengthString(this.range * this.damageFalloffRange);
				string arg2 = MeasurementTool.FormatLengthString(this.range * this.damageFalloffMaxRange);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_DamageFalloff", arg, arg2, string.Format("{0:P}", this.damageFalloffMultiplier)), 10000);
			}
			if (this._projectile != null)
			{
				base.BuildExplosiveDescription(builder, itemInstance);
				return;
			}
			base.BuildNonExplosiveDescription(builder, itemInstance);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x000522C8 File Offset: 0x000504C8
		public override byte[] getState(EItemOrigin origin)
		{
			byte[] magazineState = this.getMagazineState(this.getMagazineID());
			byte[] array = new byte[]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				1,
				100,
				100,
				100,
				100,
				100
			};
			array[0] = this.sightState[0];
			array[1] = this.sightState[1];
			array[2] = this.tacticalState[0];
			array[3] = this.tacticalState[1];
			array[4] = this.gripState[0];
			array[5] = this.gripState[1];
			array[6] = this.barrelState[0];
			array[7] = this.barrelState[1];
			array[8] = magazineState[0];
			array[9] = magazineState[1];
			array[10] = ((origin != EItemOrigin.WORLD || Random.value < ((Provider.modeConfigData != null) ? Provider.modeConfigData.Items.Gun_Bullets_Full_Chance : 0.9f)) ? this.ammoMax : ((byte)Mathf.CeilToInt((float)Random.Range((int)this.ammoMin, (int)(this.ammoMax + 1)) * ((Provider.modeConfigData != null) ? Provider.modeConfigData.Items.Gun_Bullets_Multiplier : 1f))));
			array[11] = (byte)this.firemode;
			return array;
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x000523D0 File Offset: 0x000505D0
		public byte[] getState(ushort sight, ushort tactical, ushort grip, ushort barrel, ushort magazine, byte ammo)
		{
			byte[] bytes = BitConverter.GetBytes(sight);
			byte[] bytes2 = BitConverter.GetBytes(tactical);
			byte[] bytes3 = BitConverter.GetBytes(grip);
			byte[] bytes4 = BitConverter.GetBytes(barrel);
			byte[] bytes5 = BitConverter.GetBytes(magazine);
			byte[] array = new byte[]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				1,
				100,
				100,
				100,
				100,
				100
			};
			array[0] = bytes[0];
			array[1] = bytes[1];
			array[2] = bytes2[0];
			array[3] = bytes2[1];
			array[4] = bytes3[0];
			array[5] = bytes3[1];
			array[6] = bytes4[0];
			array[7] = bytes4[1];
			array[8] = bytes5[0];
			array[9] = bytes5[1];
			array[10] = ammo;
			array[11] = (byte)this.firemode;
			return array;
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x00052465 File Offset: 0x00050665
		// (set) Token: 0x06001641 RID: 5697 RVA: 0x0005246D File Offset: 0x0005066D
		public ushort sightID
		{
			get
			{
				return this._sightID;
			}
			set
			{
				this._sightID = value;
				this.sightState = BitConverter.GetBytes(this.sightID);
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001642 RID: 5698 RVA: 0x00052487 File Offset: 0x00050687
		// (set) Token: 0x06001643 RID: 5699 RVA: 0x0005248F File Offset: 0x0005068F
		public ushort tacticalID
		{
			get
			{
				return this._tacticalID;
			}
			set
			{
				this._tacticalID = value;
				this.tacticalState = BitConverter.GetBytes(this.tacticalID);
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001644 RID: 5700 RVA: 0x000524A9 File Offset: 0x000506A9
		// (set) Token: 0x06001645 RID: 5701 RVA: 0x000524B1 File Offset: 0x000506B1
		public ushort gripID
		{
			get
			{
				return this._gripID;
			}
			set
			{
				this._gripID = value;
				this.gripState = BitConverter.GetBytes(this.gripID);
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001646 RID: 5702 RVA: 0x000524CB File Offset: 0x000506CB
		// (set) Token: 0x06001647 RID: 5703 RVA: 0x000524D3 File Offset: 0x000506D3
		public ushort barrelID
		{
			get
			{
				return this._barrelID;
			}
			set
			{
				this._barrelID = value;
				this.barrelState = BitConverter.GetBytes(this.barrelID);
			}
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x000524F0 File Offset: 0x000506F0
		public ushort getMagazineID()
		{
			if (Level.info != null && this.magazineReplacements != null)
			{
				foreach (MagazineReplacement magazineReplacement in this.magazineReplacements)
				{
					if (magazineReplacement.map == Level.info.name)
					{
						return magazineReplacement.id;
					}
				}
			}
			return this.magazineID;
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0005254D File Offset: 0x0005074D
		private byte[] getMagazineState(ushort id)
		{
			return BitConverter.GetBytes(id);
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x0600164A RID: 5706 RVA: 0x00052555 File Offset: 0x00050755
		// (set) Token: 0x0600164B RID: 5707 RVA: 0x0005255D File Offset: 0x0005075D
		public ushort[] attachmentCalibers { get; private set; }

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x0600164C RID: 5708 RVA: 0x00052566 File Offset: 0x00050766
		// (set) Token: 0x0600164D RID: 5709 RVA: 0x0005256E File Offset: 0x0005076E
		public ushort[] magazineCalibers { get; private set; }

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x0600164E RID: 5710 RVA: 0x00052577 File Offset: 0x00050777
		// (set) Token: 0x0600164F RID: 5711 RVA: 0x0005257F File Offset: 0x0005077F
		public float baseSpreadAngleRadians { get; private set; }

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001650 RID: 5712 RVA: 0x00052588 File Offset: 0x00050788
		// (set) Token: 0x06001651 RID: 5713 RVA: 0x00052590 File Offset: 0x00050790
		public float muzzleVelocity { get; protected set; }

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001652 RID: 5714 RVA: 0x00052599 File Offset: 0x00050799
		// (set) Token: 0x06001653 RID: 5715 RVA: 0x000525A1 File Offset: 0x000507A1
		public float bulletGravityMultiplier { get; protected set; }

		// Token: 0x06001654 RID: 5716 RVA: 0x000525AA File Offset: 0x000507AA
		public EffectAsset FindMuzzleEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this.muzzleGuid, this.muzzle);
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x000525BD File Offset: 0x000507BD
		public EffectAsset FindShellEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this.shellGuid, this.shell);
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001656 RID: 5718 RVA: 0x000525D0 File Offset: 0x000507D0
		public override bool showQuality
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Is this gun setup to have a change of jamming?
		/// </summary>
		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001657 RID: 5719 RVA: 0x000525D3 File Offset: 0x000507D3
		// (set) Token: 0x06001658 RID: 5720 RVA: 0x000525DB File Offset: 0x000507DB
		public bool canEverJam { get; protected set; }

		/// <summary>
		/// [0, 1] quality percentage that jamming will start happening.
		/// </summary>
		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001659 RID: 5721 RVA: 0x000525E4 File Offset: 0x000507E4
		// (set) Token: 0x0600165A RID: 5722 RVA: 0x000525EC File Offset: 0x000507EC
		public float jamQualityThreshold { get; protected set; }

		/// <summary>
		/// [0, 1] percentage of the time that shots will jam the gun when at 0% quality.
		/// Chance of jamming is blended between 0% at jamQualityThreshold and jamMaxChance% at 0% quality.
		/// </summary>
		// Token: 0x170003BE RID: 958
		// (get) Token: 0x0600165B RID: 5723 RVA: 0x000525F5 File Offset: 0x000507F5
		// (set) Token: 0x0600165C RID: 5724 RVA: 0x000525FD File Offset: 0x000507FD
		public float jamMaxChance { get; protected set; }

		/// <summary>
		/// Name of the animation to play when unjamming chamber.
		/// </summary>
		// Token: 0x170003BF RID: 959
		// (get) Token: 0x0600165D RID: 5725 RVA: 0x00052606 File Offset: 0x00050806
		// (set) Token: 0x0600165E RID: 5726 RVA: 0x0005260E File Offset: 0x0005080E
		public string unjamChamberAnimName { get; protected set; }

		// Token: 0x0600165F RID: 5727 RVA: 0x00052617 File Offset: 0x00050817
		public void GrantShootQuestRewards(Player player)
		{
			this.shootQuestRewards.Grant(player);
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001660 RID: 5728 RVA: 0x00052625 File Offset: 0x00050825
		protected override bool doesItemTypeHaveSkins
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00052630 File Offset: 0x00050830
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._shoot = bundle.load<AudioClip>("Shoot");
			this._reload = bundle.load<AudioClip>("Reload");
			this._hammer = bundle.load<AudioClip>("Hammer");
			this._aim = bundle.load<AudioClip>("Aim");
			this._minigun = bundle.load<AudioClip>("Minigun");
			this._chamberJammedSound = bundle.load<AudioClip>("ChamberJammed");
			this.fireDelaySound = bundle.load<AudioClip>("FireDelay");
			this._projectile = bundle.load<GameObject>("Projectile");
			this.ammoMin = data.ParseUInt8("Ammo_Min", 0);
			this.ammoMax = data.ParseUInt8("Ammo_Max", 0);
			this.sightID = data.ParseUInt16("Sight", 0);
			this.tacticalID = data.ParseUInt16("Tactical", 0);
			this.gripID = data.ParseUInt16("Grip", 0);
			this.barrelID = data.ParseUInt16("Barrel", 0);
			this.magazineID = data.ParseUInt16("Magazine", 0);
			int num = data.ParseInt32("Magazine_Replacements", 0);
			this.magazineReplacements = new MagazineReplacement[num];
			for (int i = 0; i < num; i++)
			{
				ushort id = data.ParseUInt16("Magazine_Replacement_" + i.ToString() + "_ID", 0);
				string @string = data.GetString("Magazine_Replacement_" + i.ToString() + "_Map", null);
				MagazineReplacement magazineReplacement = default(MagazineReplacement);
				magazineReplacement.id = id;
				magazineReplacement.map = @string;
				this.magazineReplacements[i] = magazineReplacement;
			}
			this.unplace = data.ParseFloat("Unplace", 0f);
			this.replace = data.ParseFloat("Replace", 1f);
			this.hasSight = data.ContainsKey("Hook_Sight");
			this.hasTactical = data.ContainsKey("Hook_Tactical");
			this.hasGrip = data.ContainsKey("Hook_Grip");
			this.hasBarrel = data.ContainsKey("Hook_Barrel");
			int num2 = data.ParseInt32("Magazine_Calibers", 0);
			if (num2 > 0)
			{
				this.magazineCalibers = new ushort[num2];
				for (int j = 0; j < num2; j++)
				{
					this.magazineCalibers[j] = data.ParseUInt16("Magazine_Caliber_" + j.ToString(), 0);
				}
				int num3 = data.ParseInt32("Attachment_Calibers", 0);
				if (num3 > 0)
				{
					this.attachmentCalibers = new ushort[num3];
					for (int k = 0; k < num3; k++)
					{
						this.attachmentCalibers[k] = data.ParseUInt16("Attachment_Caliber_" + k.ToString(), 0);
					}
				}
				else
				{
					this.attachmentCalibers = this.magazineCalibers;
				}
			}
			else
			{
				this.magazineCalibers = new ushort[1];
				this.magazineCalibers[0] = data.ParseUInt16("Caliber", 0);
				this.attachmentCalibers = this.magazineCalibers;
			}
			this.firerate = data.ParseUInt8("Firerate", 0);
			this.action = (EAction)Enum.Parse(typeof(EAction), data.GetString("Action", null), true);
			if (data.ContainsKey("Delete_Empty_Magazines"))
			{
				this.shouldDeleteEmptyMagazines = true;
			}
			else
			{
				bool defaultValue = this.action == EAction.Pump || this.action == EAction.Rail || this.action == EAction.String || this.action == EAction.Rocket || this.action == EAction.Break;
				this.shouldDeleteEmptyMagazines = data.ParseBool("Should_Delete_Empty_Magazines", defaultValue);
			}
			this.requiresNonZeroAttachmentCaliber = data.ParseBool("Requires_NonZero_Attachment_Caliber", false);
			this.bursts = data.ParseInt32("Bursts", 0);
			this.hasSafety = data.ContainsKey("Safety");
			this.hasSemi = data.ContainsKey("Semi");
			this.hasAuto = data.ContainsKey("Auto");
			this.hasBurst = (this.bursts > 0);
			this.isTurret = data.ContainsKey("Turret");
			if (this.hasAuto)
			{
				this.firemode = EFiremode.AUTO;
			}
			else if (this.hasSemi)
			{
				this.firemode = EFiremode.SEMI;
			}
			else if (this.hasBurst)
			{
				this.firemode = EFiremode.BURST;
			}
			else if (this.hasSafety)
			{
				this.firemode = EFiremode.SAFETY;
			}
			this.spreadAim = data.ParseFloat("Spread_Aim", 0f);
			if (data.ContainsKey("Spread_Angle_Degrees"))
			{
				this.baseSpreadAngleRadians = 0.017453292f * data.ParseFloat("Spread_Angle_Degrees", 0f);
				this.spreadHip = Mathf.Tan(this.baseSpreadAngleRadians);
			}
			else
			{
				this.spreadHip = data.ParseFloat("Spread_Hip", 0f);
				this.baseSpreadAngleRadians = Mathf.Atan(this.spreadHip);
				if (ItemGunAsset.shouldLogSpreadConversion)
				{
					UnturnedLog.info(string.Format("Converted \"{0}\" Spread_Hip {1} to {2} degrees", this.FriendlyName, this.spreadHip, this.baseSpreadAngleRadians * 57.29578f));
				}
			}
			this.spreadSprint = data.ParseFloat("Spread_Sprint", 1.25f);
			this.spreadCrouch = data.ParseFloat("Spread_Crouch", 0.85f);
			this.spreadProne = data.ParseFloat("Spread_Prone", 0.7f);
			this.spreadSwimming = data.ParseFloat("Spread_Swimming", 1.1f);
			this.spreadMidair = data.ParseFloat("Spread_Midair", 1.5f);
			this.recoilMin_x = data.ParseFloat("Recoil_Min_X", 0f);
			this.recoilMin_y = data.ParseFloat("Recoil_Min_Y", 0f);
			this.recoilMax_x = data.ParseFloat("Recoil_Max_X", 0f);
			this.recoilMax_y = data.ParseFloat("Recoil_Max_Y", 0f);
			this.aimingRecoilMultiplier = data.ParseFloat("Aiming_Recoil_Multiplier", 1f);
			this.recover_x = data.ParseFloat("Recover_X", 0f);
			this.recover_y = data.ParseFloat("Recover_Y", 0f);
			this.recoilSprint = data.ParseFloat("Recoil_Sprint", 1.25f);
			this.recoilCrouch = data.ParseFloat("Recoil_Crouch", 0.85f);
			this.recoilProne = data.ParseFloat("Recoil_Prone", 0.7f);
			this.recoilSwimming = data.ParseFloat("Recoil_Swimming", 1.1f);
			this.recoilMidair = data.ParseFloat("Recoil_Midair", 1f);
			this.shakeMin_x = data.ParseFloat("Shake_Min_X", 0f);
			this.shakeMin_y = data.ParseFloat("Shake_Min_Y", 0f);
			this.shakeMin_z = data.ParseFloat("Shake_Min_Z", 0f);
			this.shakeMax_x = data.ParseFloat("Shake_Max_X", 0f);
			this.shakeMax_y = data.ParseFloat("Shake_Max_Y", 0f);
			this.shakeMax_z = data.ParseFloat("Shake_Max_Z", 0f);
			this.ballisticSteps = data.ParseUInt8("Ballistic_Steps", 0);
			this.ballisticTravel = data.ParseFloat("Ballistic_Travel", 0f);
			bool flag = data.ContainsKey("Ballistic_Steps") && this.ballisticSteps > 0;
			bool flag2 = data.ContainsKey("Ballistic_Travel") && this.ballisticTravel > 0.1f;
			if (flag && flag2)
			{
				float num4 = Mathf.Abs((float)this.ballisticSteps * this.ballisticTravel - this.range);
				if (num4 > 0.1f)
				{
					Assets.reportError(this, "range and manual ballistic range are mismatched by " + num4.ToString() + "m. Recommended to only have one or the other specified!");
				}
			}
			else if (flag)
			{
				this.ballisticTravel = this.range / (float)this.ballisticSteps;
			}
			else if (flag2)
			{
				this.ballisticSteps = (byte)Mathf.CeilToInt(this.range / this.ballisticTravel);
			}
			else
			{
				this.ballisticTravel = 10f;
				this.ballisticSteps = (byte)Mathf.CeilToInt(this.range / this.ballisticTravel);
			}
			this.muzzleVelocity = this.ballisticTravel * PlayerInput.TOCK_PER_SECOND;
			float num5;
			if (data.TryParseFloat("Ballistic_Drop", out num5))
			{
				if (num5 < 1E-06f)
				{
					this.bulletGravityMultiplier = 0f;
				}
				else
				{
					float num6 = 0f;
					Vector2 right = Vector2.right;
					for (int l = 0; l < (int)this.ballisticSteps; l++)
					{
						num6 += right.y * this.ballisticTravel;
						right.y -= num5;
						right.Normalize();
					}
					float num7 = (float)this.ballisticSteps * 0.02f;
					float num8 = 2f * num6 / (num7 * num7);
					this.bulletGravityMultiplier = num8 / -9.81f;
					if (Assets.shouldValidateAssets)
					{
						UnturnedLog.info(string.Format("Converted \"{0}\" Ballistic_Drop {1} to Bullet_Gravity_Multiplier {2}", this.FriendlyName, num5, this.bulletGravityMultiplier));
					}
				}
			}
			else
			{
				this.bulletGravityMultiplier = data.ParseFloat("Bullet_Gravity_Multiplier", 4f);
			}
			if (data.ContainsKey("Ballistic_Force"))
			{
				this.ballisticForce = data.ParseFloat("Ballistic_Force", 0f);
			}
			else
			{
				this.ballisticForce = 0.002f;
			}
			this.damageFalloffRange = data.ParseFloat("Damage_Falloff_Range", 1f);
			this.damageFalloffMaxRange = data.ParseFloat("Damage_Falloff_Max_Range", 1f);
			this.damageFalloffMultiplier = data.ParseFloat("Damage_Falloff_Multiplier", 1f);
			this.projectileLifespan = data.ParseFloat("Projectile_Lifespan", 30f);
			this.projectilePenetrateBuildables = data.ContainsKey("Projectile_Penetrate_Buildables");
			this.projectileExplosionLaunchSpeed = data.ParseFloat("Projectile_Explosion_Launch_Speed", this.playerDamageMultiplier.damage * 0.1f);
			this.reloadTime = data.ParseFloat("Reload_Time", 0f);
			this.hammerTime = data.ParseFloat("Hammer_Time", 0f);
			this.muzzle = data.ParseGuidOrLegacyId("Muzzle", out this.muzzleGuid);
			this.explosion = data.ParseGuidOrLegacyId("Explosion", out this.projectileExplosionEffectGuid);
			if (data.ContainsKey("Shell"))
			{
				this.shell = data.ParseGuidOrLegacyId("Shell", out this.shellGuid);
			}
			else if (this.action == EAction.Pump || this.action == EAction.Break)
			{
				this.shellGuid = new Guid("0dc9bf936ce0409585fe9525287c7a7d");
			}
			else if (this.action != EAction.Rail)
			{
				this.shellGuid = new Guid("f380a6a6f41f422c9f5b9ac13e3b13e8");
			}
			if (data.ContainsKey("Alert_Radius"))
			{
				this.alertRadius = data.ParseFloat("Alert_Radius", 0f);
			}
			else
			{
				this.alertRadius = 48f;
			}
			if (data.ContainsKey("Range_Rangefinder"))
			{
				this.rangeRangefinder = data.ParseFloat("Range_Rangefinder", 0f);
			}
			else
			{
				this.rangeRangefinder = data.ParseFloat("Range", 0f);
			}
			this.instakillHeadshots = data.ParseBool("Instakill_Headshots", false);
			this.infiniteAmmo = data.ParseBool("Infinite_Ammo", false);
			this.ammoPerShot = data.ParseUInt8("Ammo_Per_Shot", 1);
			this.fireDelay = Mathf.RoundToInt(data.ParseFloat("Fire_Delay_Seconds", 0f) * PlayerInput.TOCK_PER_SECOND);
			this.allowMagazineChange = data.ParseBool("Allow_Magazine_Change", true);
			this.canAimDuringSprint = data.ParseBool("Can_Aim_During_Sprint", false);
			this.aimingMovementSpeedMultiplier = data.ParseFloat("Aiming_Movement_Speed_Multiplier", this.canAimDuringSprint ? 1f : 0.75f);
			this.canEverJam = data.ContainsKey("Can_Ever_Jam");
			if (this.canEverJam)
			{
				this.jamQualityThreshold = data.ParseFloat("Jam_Quality_Threshold", 0.4f);
				this.jamMaxChance = data.ParseFloat("Jam_Max_Chance", 0.1f);
				this.unjamChamberAnimName = data.GetString("Unjam_Chamber_Anim", "UnjamChamber");
			}
			float defaultValue2;
			if (this.action == EAction.String)
			{
				defaultValue2 = 16f;
			}
			else if (this.action == EAction.Rocket)
			{
				defaultValue2 = 64f;
			}
			else
			{
				defaultValue2 = 512f;
			}
			this.gunshotRolloffDistance = data.ParseFloat("Gunshot_Rolloff_Distance", defaultValue2);
			this.shootQuestRewards.Parse(data, localization, this, "Shoot_Quest_Rewards", "Shoot_Quest_Reward_");
			this.aimInDuration = data.ParseFloat("Aim_In_Duration", 0.2f);
			this.shouldScaleAimAnimations = data.ParseBool("Scale_Aim_Animation_Speed", true);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x0005328C File Offset: 0x0005148C
		protected override AudioReference GetDefaultInventoryAudio()
		{
			if (this.name.Contains("Bow", 3))
			{
				return base.GetDefaultInventoryAudio();
			}
			if (this.size_x <= 2 && this.size_y <= 2)
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/SmallGunAttachment.asset");
			}
			return new AudioReference("core.masterbundle", "Sounds/Inventory/LargeGunAttachment.asset");
		}

		// Token: 0x04000951 RID: 2385
		protected AudioClip _shoot;

		// Token: 0x04000952 RID: 2386
		protected AudioClip _reload;

		// Token: 0x04000953 RID: 2387
		protected AudioClip _hammer;

		// Token: 0x04000954 RID: 2388
		protected AudioClip _aim;

		// Token: 0x04000955 RID: 2389
		protected AudioClip _minigun;

		// Token: 0x04000956 RID: 2390
		protected AudioClip _chamberJammedSound;

		// Token: 0x04000959 RID: 2393
		protected GameObject _projectile;

		// Token: 0x0400095A RID: 2394
		public float alertRadius;

		// Token: 0x04000964 RID: 2404
		public byte ammoMin;

		// Token: 0x04000965 RID: 2405
		public byte ammoMax;

		// Token: 0x04000966 RID: 2406
		private ushort _sightID;

		// Token: 0x04000967 RID: 2407
		private byte[] sightState;

		// Token: 0x04000968 RID: 2408
		private ushort _tacticalID;

		// Token: 0x04000969 RID: 2409
		private byte[] tacticalState;

		// Token: 0x0400096A RID: 2410
		private ushort _gripID;

		// Token: 0x0400096B RID: 2411
		private byte[] gripState;

		// Token: 0x0400096C RID: 2412
		private ushort _barrelID;

		// Token: 0x0400096D RID: 2413
		private byte[] barrelState;

		// Token: 0x0400096E RID: 2414
		private ushort magazineID;

		// Token: 0x0400096F RID: 2415
		private MagazineReplacement[] magazineReplacements;

		// Token: 0x04000970 RID: 2416
		public float unplace;

		// Token: 0x04000971 RID: 2417
		public float replace;

		// Token: 0x04000972 RID: 2418
		public bool hasSight;

		// Token: 0x04000973 RID: 2419
		public bool hasTactical;

		// Token: 0x04000974 RID: 2420
		public bool hasGrip;

		// Token: 0x04000975 RID: 2421
		public bool hasBarrel;

		// Token: 0x04000978 RID: 2424
		public byte firerate;

		// Token: 0x04000979 RID: 2425
		public EAction action;

		// Token: 0x0400097A RID: 2426
		public bool shouldDeleteEmptyMagazines;

		/// <summary>
		/// Defaults to false. If true, attachments must specify at least one non-zero caliber.
		/// Requested by Great Hero J to block vanilla attachments in VGR.
		/// </summary>
		// Token: 0x0400097B RID: 2427
		public bool requiresNonZeroAttachmentCaliber;

		// Token: 0x0400097C RID: 2428
		public bool hasSafety;

		// Token: 0x0400097D RID: 2429
		public bool hasSemi;

		// Token: 0x0400097E RID: 2430
		public bool hasAuto;

		// Token: 0x0400097F RID: 2431
		public bool hasBurst;

		// Token: 0x04000980 RID: 2432
		public bool isTurret;

		// Token: 0x04000981 RID: 2433
		public int bursts;

		// Token: 0x04000982 RID: 2434
		internal EFiremode firemode;

		// Token: 0x04000983 RID: 2435
		public float spreadAim;

		// Token: 0x04000984 RID: 2436
		[Obsolete("Replaced by baseSpreadAngleRadians")]
		public float spreadHip;

		/// <summary>
		/// Spread multiplier while sprinting.
		/// </summary>
		// Token: 0x04000986 RID: 2438
		public float spreadSprint;

		/// <summary>
		/// Spread multiplier while crouched.
		/// </summary>
		// Token: 0x04000987 RID: 2439
		public float spreadCrouch;

		/// <summary>
		/// Spread multiplier while prone.
		/// </summary>
		// Token: 0x04000988 RID: 2440
		public float spreadProne;

		/// <summary>
		/// Spread multiplier while swimming.
		/// </summary>
		// Token: 0x04000989 RID: 2441
		public float spreadSwimming;

		/// <summary>
		/// Spread multiplier while not grounded.
		/// </summary>
		// Token: 0x0400098A RID: 2442
		public float spreadMidair;

		// Token: 0x0400098B RID: 2443
		public float recoilMin_x;

		// Token: 0x0400098C RID: 2444
		public float recoilMin_y;

		// Token: 0x0400098D RID: 2445
		public float recoilMax_x;

		// Token: 0x0400098E RID: 2446
		public float recoilMax_y;

		/// <summary>
		/// Recoil magnitude multiplier while the gun is aiming down sights.
		/// </summary>
		// Token: 0x0400098F RID: 2447
		public float aimingRecoilMultiplier;

		/// <summary>
		/// Recoil magnitude while sprinting.
		/// </summary>
		// Token: 0x04000990 RID: 2448
		public float recoilSprint;

		/// <summary>
		/// Recoil magnitude while crouched.
		/// </summary>
		// Token: 0x04000991 RID: 2449
		public float recoilCrouch;

		/// <summary>
		/// Recoil magnitude while prone.
		/// </summary>
		// Token: 0x04000992 RID: 2450
		public float recoilProne;

		/// <summary>
		/// Recoil magnitude while swimming.
		/// </summary>
		// Token: 0x04000993 RID: 2451
		public float recoilSwimming;

		/// <summary>
		/// Recoil magnitude while not grounded.
		/// </summary>
		// Token: 0x04000994 RID: 2452
		public float recoilMidair;

		// Token: 0x04000995 RID: 2453
		public float recover_x;

		// Token: 0x04000996 RID: 2454
		public float recover_y;

		// Token: 0x04000997 RID: 2455
		public float shakeMin_x;

		// Token: 0x04000998 RID: 2456
		public float shakeMin_y;

		// Token: 0x04000999 RID: 2457
		public float shakeMin_z;

		// Token: 0x0400099A RID: 2458
		public float shakeMax_x;

		// Token: 0x0400099B RID: 2459
		public float shakeMax_y;

		// Token: 0x0400099C RID: 2460
		public float shakeMax_z;

		// Token: 0x0400099D RID: 2461
		public byte ballisticSteps;

		// Token: 0x0400099E RID: 2462
		public float ballisticTravel;

		// Token: 0x040009A1 RID: 2465
		public float ballisticForce;

		/// <summary>
		/// [0, 1] percentage of maximum range where damage begins decreasing toward falloff multiplier.
		/// </summary>
		// Token: 0x040009A2 RID: 2466
		public float damageFalloffRange;

		/// <summary>
		/// [0, 1] percentage of maximum range where damage finishes decreasing toward falloff multiplier.
		/// </summary>
		// Token: 0x040009A3 RID: 2467
		public float damageFalloffMaxRange;

		/// <summary>
		/// [0, 1] percentage of damage to apply at damageFalloffMaxRange.
		/// </summary>
		// Token: 0x040009A4 RID: 2468
		public float damageFalloffMultiplier;

		/// <summary>
		/// Seconds before physics projectile is destroyed.
		/// </summary>
		// Token: 0x040009A5 RID: 2469
		public float projectileLifespan;

		// Token: 0x040009A6 RID: 2470
		public bool projectilePenetrateBuildables;

		// Token: 0x040009A7 RID: 2471
		public float projectileExplosionLaunchSpeed;

		// Token: 0x040009A8 RID: 2472
		public float reloadTime;

		// Token: 0x040009A9 RID: 2473
		public float hammerTime;

		// Token: 0x040009AA RID: 2474
		public Guid muzzleGuid;

		// Token: 0x040009AB RID: 2475
		[Obsolete]
		public ushort muzzle;

		// Token: 0x040009AC RID: 2476
		public Guid shellGuid;

		// Token: 0x040009AD RID: 2477
		[Obsolete]
		public ushort shell;

		// Token: 0x040009AE RID: 2478
		public Guid projectileExplosionEffectGuid;

		// Token: 0x040009AF RID: 2479
		public ushort explosion;

		/// <summary>
		/// Movement speed multiplier while the gun is aiming down sights.
		/// </summary>
		// Token: 0x040009B4 RID: 2484
		public float aimingMovementSpeedMultiplier;

		// Token: 0x040009B5 RID: 2485
		protected NPCRewardsList shootQuestRewards;

		// Token: 0x040009B6 RID: 2486
		private static CommandLineFlag shouldLogSpreadConversion = new CommandLineFlag(false, "-LogGunSpreadConversion");
	}
}
