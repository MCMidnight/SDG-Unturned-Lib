using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002D1 RID: 721
	public class ItemAsset : Asset, ISkinableAsset
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x060014EA RID: 5354 RVA: 0x0004D7D1 File Offset: 0x0004B9D1
		public bool shouldVerifyHash
		{
			get
			{
				return this._shouldVerifyHash;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x060014EB RID: 5355 RVA: 0x0004D7D9 File Offset: 0x0004B9D9
		internal override bool ShouldVerifyHash
		{
			get
			{
				return this._shouldVerifyHash;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x0004D7E1 File Offset: 0x0004B9E1
		public override string FriendlyName
		{
			get
			{
				if (!string.IsNullOrEmpty(this._itemName))
				{
					return this._itemName;
				}
				return this.name;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0004D7FD File Offset: 0x0004B9FD
		public string itemName
		{
			get
			{
				return this._itemName;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x0004D805 File Offset: 0x0004BA05
		public string itemDescription
		{
			get
			{
				return this._itemDescription;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x0004D80D File Offset: 0x0004BA0D
		public string proPath
		{
			get
			{
				return this._proPath;
			}
		}

		/// <summary>
		/// Useable subclass.
		/// </summary>
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x0004D815 File Offset: 0x0004BA15
		// (set) Token: 0x060014F1 RID: 5361 RVA: 0x0004D81D File Offset: 0x0004BA1D
		public Type useableType { get; protected set; }

		/// <summary>
		/// Can this useable be equipped by players?
		/// True for most items, but allows modders to create sentry-only weapons.
		/// </summary>
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x0004D826 File Offset: 0x0004BA26
		// (set) Token: 0x060014F3 RID: 5363 RVA: 0x0004D82E File Offset: 0x0004BA2E
		public bool canPlayerEquip { get; protected set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x0004D837 File Offset: 0x0004BA37
		// (set) Token: 0x060014F5 RID: 5365 RVA: 0x0004D83F File Offset: 0x0004BA3F
		[Obsolete("Renamed to canPlayerEquip")]
		public bool isUseable
		{
			get
			{
				return this.canPlayerEquip;
			}
			set
			{
				this.canPlayerEquip = value;
			}
		}

		/// <summary>
		/// Can this useable be equipped while underwater?
		/// </summary>
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x0004D848 File Offset: 0x0004BA48
		// (set) Token: 0x060014F7 RID: 5367 RVA: 0x0004D850 File Offset: 0x0004BA50
		public bool canUseUnderwater { get; protected set; }

		// Token: 0x060014F8 RID: 5368 RVA: 0x0004D859 File Offset: 0x0004BA59
		public byte[] getState()
		{
			return this.getState(false);
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x0004D862 File Offset: 0x0004BA62
		public byte[] getState(bool isFull)
		{
			return this.getState(isFull ? EItemOrigin.ADMIN : EItemOrigin.WORLD);
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0004D871 File Offset: 0x0004BA71
		public virtual byte[] getState(EItemOrigin origin)
		{
			return new byte[0];
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0004D87C File Offset: 0x0004BA7C
		public virtual void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			Local localization = PlayerDashboardInventoryUI.localization;
			string text = "Rarity_";
			int num = (int)this.rarity;
			string arg = localization.format(text + num.ToString());
			Local localization2 = PlayerDashboardInventoryUI.localization;
			string text2 = "Type_";
			num = (int)this.type;
			string arg2 = localization2.format(text2 + num.ToString());
			builder.Append(string.Concat(new string[]
			{
				"<color=",
				Palette.hex(ItemTool.getRarityColorUI(this.rarity)),
				">",
				PlayerDashboardInventoryUI.localization.format("Rarity_Type_Label", arg, arg2),
				"</color>"
			}), 0);
			builder.Append(this._itemDescription, 200);
			if (this.showQuality)
			{
				Color32 color = ItemTool.getQualityColor((float)itemInstance.quality / 100f);
				builder.Append(string.Concat(new string[]
				{
					"<color=",
					Palette.hex(color),
					">",
					PlayerDashboardInventoryUI.localization.format("Quality", itemInstance.quality),
					"</color>"
				}), 400);
			}
			if (this.amount > 1)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_AmountWithCapacity", itemInstance.amount, this.amount), 400);
			}
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this.equipableMovementSpeedMultiplier != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_EquipableMovementSpeedModifier", PlayerDashboardInventoryUI.FormatStatModifier(this.equipableMovementSpeedMultiplier, true, true)), 10000 + this.DescSort_HigherIsBeneficial(this.equipableMovementSpeedMultiplier));
			}
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x0004DA34 File Offset: 0x0004BC34
		public override string GetTypeFriendlyName()
		{
			string text = base.GetTypeFriendlyName();
			if (text.StartsWith("Item "))
			{
				text = text.Substring(5) + " Item";
			}
			return text;
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060014FD RID: 5373 RVA: 0x0004DA68 File Offset: 0x0004BC68
		public byte count
		{
			get
			{
				float num;
				float num2;
				if (Provider.modeConfigData != null)
				{
					if (this.type == EItemType.MAGAZINE)
					{
						num = Provider.modeConfigData.Items.Magazine_Bullets_Full_Chance;
						num2 = Provider.modeConfigData.Items.Magazine_Bullets_Multiplier;
					}
					else
					{
						num = Provider.modeConfigData.Items.Crate_Bullets_Full_Chance;
						num2 = Provider.modeConfigData.Items.Crate_Bullets_Multiplier;
					}
				}
				else
				{
					num = 0.9f;
					num2 = 1f;
				}
				if (Random.value < num)
				{
					return this.amount;
				}
				return (byte)Mathf.CeilToInt((float)Random.Range((int)this.countMin, (int)(this.countMax + 1)) * num2);
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060014FE RID: 5374 RVA: 0x0004DB04 File Offset: 0x0004BD04
		public byte quality
		{
			get
			{
				if (Random.value < ((Provider.modeConfigData != null) ? Provider.modeConfigData.Items.Quality_Full_Chance : 0.9f))
				{
					return 100;
				}
				return (byte)Mathf.CeilToInt((float)Random.Range((int)this.qualityMin, (int)(this.qualityMax + 1)) * ((Provider.modeConfigData != null) ? Provider.modeConfigData.Items.Quality_Multiplier : 1f));
			}
		}

		/// <summary>
		/// Which parent to use when attaching an equipped/useable item to the player.
		/// </summary>
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060014FF RID: 5375 RVA: 0x0004DB70 File Offset: 0x0004BD70
		// (set) Token: 0x06001500 RID: 5376 RVA: 0x0004DB78 File Offset: 0x0004BD78
		public EEquipableModelParent EquipableModelParent { get; protected set; }

		/// <summary>
		/// If true, equipable prefab is a child of the left hand rather than the right.
		/// Defaults to false.
		/// </summary>
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06001501 RID: 5377 RVA: 0x0004DB81 File Offset: 0x0004BD81
		[Obsolete("Replaced by EquipableModelParent property's LeftHook option.")]
		public bool ShouldAttachEquippedModelToLeftHand
		{
			get
			{
				return this.EquipableModelParent == EEquipableModelParent.LeftHook;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x0004DB8C File Offset: 0x0004BD8C
		public GameObject item
		{
			get
			{
				return this._item;
			}
		}

		/// <summary>
		/// Name to use when instantiating item prefab.
		/// By default the asset legacy id is used, but it can be overridden because some
		/// modders rely on the name for Unity's legacy animation component. For example
		/// in Toothy Deerryte's case there were a lot of duplicate animations to work
		/// around the id naming, simplified by overriding name.
		/// </summary>
		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06001503 RID: 5379 RVA: 0x0004DB94 File Offset: 0x0004BD94
		// (set) Token: 0x06001504 RID: 5380 RVA: 0x0004DB9C File Offset: 0x0004BD9C
		public string instantiatedItemName { get; protected set; }

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06001505 RID: 5381 RVA: 0x0004DBA5 File Offset: 0x0004BDA5
		public AudioClip equip
		{
			get
			{
				return this._equip;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x0004DBAD File Offset: 0x0004BDAD
		public AnimationClip[] animations
		{
			get
			{
				return this._animations;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x0004DBB5 File Offset: 0x0004BDB5
		public List<Blueprint> blueprints
		{
			get
			{
				return this._blueprints;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x0004DBBD File Offset: 0x0004BDBD
		public List<Action> actions
		{
			get
			{
				return this._actions;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x0004DBC5 File Offset: 0x0004BDC5
		// (set) Token: 0x0600150A RID: 5386 RVA: 0x0004DBCD File Offset: 0x0004BDCD
		public bool overrideShowQuality { get; protected set; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x0004DBD6 File Offset: 0x0004BDD6
		public virtual bool showQuality
		{
			get
			{
				return this.overrideShowQuality;
			}
		}

		/// <summary>
		/// When a player dies with this item, should an item drop be spawned?
		/// </summary>
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x0004DBDE File Offset: 0x0004BDDE
		// (set) Token: 0x0600150D RID: 5389 RVA: 0x0004DBE6 File Offset: 0x0004BDE6
		public bool shouldDropOnDeath { get; protected set; }

		/// <summary>
		/// Can player click the drop button on this item?
		/// </summary>
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x0004DBEF File Offset: 0x0004BDEF
		// (set) Token: 0x0600150F RID: 5391 RVA: 0x0004DBF7 File Offset: 0x0004BDF7
		public bool allowManualDrop { get; protected set; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x0004DC00 File Offset: 0x0004BE00
		public Texture albedoBase
		{
			get
			{
				return this._albedoBase;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06001511 RID: 5393 RVA: 0x0004DC08 File Offset: 0x0004BE08
		public Texture metallicBase
		{
			get
			{
				return this._metallicBase;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x0004DC10 File Offset: 0x0004BE10
		public Texture emissionBase
		{
			get
			{
				return this._emissionBase;
			}
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0004DC18 File Offset: 0x0004BE18
		public void applySkinBaseTextures(Material material)
		{
			if (this.sharedSkinLookupID > 0 && this.sharedSkinLookupID != this.id)
			{
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, this.sharedSkinLookupID) as ItemAsset;
				if (itemAsset != null)
				{
					itemAsset.applySkinBaseTextures(material);
					return;
				}
			}
			material.SetTexture("_AlbedoBase", this.albedoBase);
			material.SetTexture("_MetallicBase", this.metallicBase);
			material.SetTexture("_EmissionBase", this.emissionBase);
		}

		/// <summary>
		/// If this item is compatible with skins for another item, lookup that item's ID instead.
		/// </summary>
		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x0004DC8C File Offset: 0x0004BE8C
		// (set) Token: 0x06001515 RID: 5397 RVA: 0x0004DC94 File Offset: 0x0004BE94
		public ushort sharedSkinLookupID { get; protected set; }

		/// <summary>
		/// Defaults to true. If false, skin material and mesh are not applied when <see cref="P:SDG.Unturned.ItemAsset.sharedSkinLookupID" /> is
		/// set. For example, a custom axe can transfer the kill counter and ragdoll effect from a vanilla item's skin
		/// without also transferring the material and mesh.
		/// </summary>
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x0004DC9D File Offset: 0x0004BE9D
		// (set) Token: 0x06001517 RID: 5399 RVA: 0x0004DCA5 File Offset: 0x0004BEA5
		public bool SharedSkinShouldApplyVisuals { get; protected set; }

		/// <summary>
		/// Should friendly-mode sentry guns target a player who has this item equipped?
		/// </summary>
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x0004DCAE File Offset: 0x0004BEAE
		public virtual bool shouldFriendlySentryTargetUser
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Kept in case any plugins refer to it.
		/// Renamed to shouldFriendlySentryTargetUser.
		/// </summary>
		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06001519 RID: 5401 RVA: 0x0004DCB1 File Offset: 0x0004BEB1
		[Obsolete]
		public virtual bool isDangerous
		{
			get
			{
				return this.shouldFriendlySentryTargetUser;
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x0004DCB9 File Offset: 0x0004BEB9
		[Obsolete("canBeUsedInSafezone now has special cases for admins")]
		public virtual bool canBeUsedInSafezone(SafezoneNode safezone)
		{
			return this.canBeUsedInSafezone(safezone, false);
		}

		/// <summary>
		/// Should players be allowed to start primary/secondary use of this item while inside given safezone?
		/// If returns false the primary/secondary inputs are set to false.
		/// </summary>
		// Token: 0x0600151B RID: 5403 RVA: 0x0004DCC3 File Offset: 0x0004BEC3
		public virtual bool canBeUsedInSafezone(SafezoneNode safezone, bool byAdmin)
		{
			return !safezone.noWeapons || !this.shouldFriendlySentryTargetUser;
		}

		/// <summary>
		/// Should this item be deleted when using and quality hits zero?
		/// e.g. final melee hit shatters the weapon.
		/// </summary>
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x0004DCD8 File Offset: 0x0004BED8
		// (set) Token: 0x0600151D RID: 5405 RVA: 0x0004DCE0 File Offset: 0x0004BEE0
		public bool shouldDeleteAtZeroQuality { get; protected set; }

		/// <summary>
		/// Should the game destroy all child colliders on the item when requested?
		/// Physics items in the world and on character preview don't request destroy,
		/// but items attached to the character do. Mods might be using colliders
		/// in unexpected ways (e.g., riot shield) so they can disable this default.
		/// </summary>
		// Token: 0x1700031B RID: 795
		// (get) Token: 0x0600151E RID: 5406 RVA: 0x0004DCE9 File Offset: 0x0004BEE9
		// (set) Token: 0x0600151F RID: 5407 RVA: 0x0004DCF1 File Offset: 0x0004BEF1
		public bool shouldDestroyItemColliders { get; protected set; }

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x0004DCFA File Offset: 0x0004BEFA
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.ITEM;
			}
		}

		/// <summary>
		/// Are there any official skins for this item type?
		/// Skips checking for base textures if item cannot have skins.
		/// </summary>
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x0004DCFD File Offset: 0x0004BEFD
		protected virtual bool doesItemTypeHaveSkins
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Find useableType by useable name.
		/// </summary>
		// Token: 0x06001522 RID: 5410 RVA: 0x0004DD00 File Offset: 0x0004BF00
		private void updateUseableType()
		{
			if (string.IsNullOrEmpty(this.useable))
			{
				this.useableType = null;
				return;
			}
			this.useableType = Assets.useableTypes.getType(this.useable);
			if (this.useableType == null)
			{
				Assets.reportError(this, "cannot find useable type \"{0}\"", this.useable);
				return;
			}
			if (!typeof(Useable).IsAssignableFrom(this.useableType))
			{
				Assets.reportError(this, "type \"{0}\" is not useable", this.useableType);
				this.useableType = null;
			}
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x0004DD87 File Offset: 0x0004BF87
		public ItemAsset()
		{
			this._animations = new AnimationClip[0];
			this._blueprints = new List<Blueprint>();
			this._actions = new List<Action>();
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0004DDBC File Offset: 0x0004BFBC
		private void initAnimations(GameObject anim)
		{
			Animation component = anim.GetComponent<Animation>();
			if (component == null)
			{
				Assets.reportError(this, "missing Animation component on 'Animations' GameObject");
				return;
			}
			this._animations = new AnimationClip[component.GetClipCount()];
			if (this.animations.Length < 1)
			{
				Assets.reportError(this, "animation clips list is empty");
				return;
			}
			int num = 0;
			bool flag = false;
			bool flag2 = false;
			foreach (object obj in component)
			{
				AnimationState animationState = (AnimationState)obj;
				this.animations[num] = animationState.clip;
				num++;
				flag |= (animationState.clip == null);
				flag2 = (flag2 || (animationState.clip != null && animationState.clip.name == "Equip"));
			}
			if (flag)
			{
				Assets.reportError(this, "has invalid animation clip references");
			}
			if (!flag2)
			{
				Assets.reportError(this, "missing 'Equip' animation clip");
			}
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0004DECC File Offset: 0x0004C0CC
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.isPro = data.ContainsKey("Pro");
			if (this.id < 2000 && !base.OriginAllowsVanillaLegacyId && !data.ContainsKey("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 2000");
			}
			this._itemName = localization.read("Name");
			if (string.IsNullOrEmpty(this._itemName))
			{
				this._itemName = string.Empty;
			}
			this._itemDescription = localization.format("Description");
			this._itemDescription = ItemTool.filterRarityRichText(this.itemDescription);
			RichTextUtil.replaceNewlineMarkup(ref this._itemDescription);
			this.instantiatedItemName = data.GetString("Instantiated_Item_Name_Override", this.id.ToString());
			this.type = (EItemType)Enum.Parse(typeof(EItemType), data.GetString("Type", null), true);
			if (data.ContainsKey("Rarity"))
			{
				this.rarity = (EItemRarity)Enum.Parse(typeof(EItemRarity), data.GetString("Rarity", null), true);
			}
			else
			{
				this.rarity = EItemRarity.COMMON;
			}
			if (this.isPro)
			{
				this.econIconUseId = data.ParseBool("Econ_Icon_Use_Id", false);
				if (this.type == EItemType.SHIRT)
				{
					this._proPath = "/Shirts";
				}
				else if (this.type == EItemType.PANTS)
				{
					this._proPath = "/Pants";
				}
				else if (this.type == EItemType.HAT)
				{
					this._proPath = "/Hats";
				}
				else if (this.type == EItemType.BACKPACK)
				{
					this._proPath = "/Backpacks";
				}
				else if (this.type == EItemType.VEST)
				{
					this._proPath = "/Vests";
				}
				else if (this.type == EItemType.MASK)
				{
					this._proPath = "/Masks";
				}
				else if (this.type == EItemType.GLASSES)
				{
					this._proPath = "/Glasses";
				}
				else if (this.type == EItemType.KEY)
				{
					this._proPath = "/Keys";
				}
				else if (this.type == EItemType.BOX)
				{
					this._proPath = "/Boxes";
				}
				this._proPath = this._proPath + "/" + this.name;
			}
			this.size_x = data.ParseUInt8("Size_X", 0);
			if (this.size_x < 1)
			{
				this.size_x = 1;
			}
			this.size_y = data.ParseUInt8("Size_Y", 0);
			if (this.size_y < 1)
			{
				this.size_y = 1;
			}
			this.iconCameraOrthographicSize = data.ParseFloat("Size_Z", -1f);
			this.isEligibleForAutomaticIconMeasurements = data.ParseBool("Use_Auto_Icon_Measurements", true);
			this.econIconCameraOrthographicSize = data.ParseFloat("Size2_Z", -1f);
			this.sharedSkinLookupID = data.ParseUInt16("Shared_Skin_Lookup_ID", this.id);
			this.SharedSkinShouldApplyVisuals = data.ParseBool("Shared_Skin_Apply_Visuals", true);
			this.amount = data.ParseUInt8("Amount", 0);
			if (this.amount < 1)
			{
				this.amount = 1;
			}
			this.countMin = data.ParseUInt8("Count_Min", 0);
			if (this.countMin < 1)
			{
				this.countMin = 1;
			}
			this.countMax = data.ParseUInt8("Count_Max", 0);
			if (this.countMax < 1)
			{
				this.countMax = 1;
			}
			if (data.ContainsKey("Quality_Min"))
			{
				this.qualityMin = data.ParseUInt8("Quality_Min", 0);
			}
			else
			{
				this.qualityMin = 10;
			}
			if (data.ContainsKey("Quality_Max"))
			{
				this.qualityMax = data.ParseUInt8("Quality_Max", 0);
			}
			else
			{
				this.qualityMax = 90;
			}
			EEquipableModelParent equipableModelParent;
			if (data.TryParseEnum<EEquipableModelParent>("EquipableModelParent", out equipableModelParent))
			{
				this.EquipableModelParent = equipableModelParent;
			}
			else if (data.ContainsKey("Backward"))
			{
				this.EquipableModelParent = EEquipableModelParent.LeftHook;
			}
			else
			{
				this.EquipableModelParent = EEquipableModelParent.RightHook;
			}
			this.shouldLeftHandedCharactersMirrorEquippedItem = data.ParseBool("Left_Handed_Characters_Mirror_Equipable", true);
			this.isEligibleForAutoStatDescriptions = data.ParseBool("Use_Auto_Stat_Descriptions", true);
			this.shouldProcedurallyAnimateInertia = data.ParseBool("Procedurally_Animate_Inertia", true);
			this.useable = data.GetString("Useable", null);
			this.updateUseableType();
			bool defaultValue = this.useableType != null;
			this.canPlayerEquip = data.ParseBool("Can_Player_Equip", defaultValue);
			this.equipableMovementSpeedMultiplier = data.ParseFloat("Equipable_Movement_Speed_Multiplier", 1f);
			if (this.canPlayerEquip)
			{
				this._equip = base.LoadRedirectableAsset<AudioClip>(bundle, "Equip", data, "EquipAudioClip");
				this.inspectAudio = data.ReadAudioReference("InspectAudioDef", bundle);
				MasterBundleReference<GameObject> masterBundleReference = data.readMasterBundleReference("EquipablePrefab", bundle);
				if (masterBundleReference.isValid)
				{
					this.equipablePrefab = masterBundleReference.loadAsset(true);
				}
				if (!this.isPro)
				{
					GameObject gameObject = bundle.load<GameObject>("Animations");
					if (gameObject != null)
					{
						this.initAnimations(gameObject);
					}
					else
					{
						this._animations = new AnimationClip[0];
					}
				}
			}
			if (data.ContainsKey("InventoryAudio"))
			{
				this.inventoryAudio = data.ReadAudioReference("InventoryAudio", bundle);
			}
			else
			{
				this.inventoryAudio = this.GetDefaultInventoryAudio();
			}
			this.slot = data.ParseEnum<ESlotType>("Slot", ESlotType.NONE);
			bool defaultValue2 = this.slot != ESlotType.PRIMARY;
			this.canUseUnderwater = data.ParseBool("Can_Use_Underwater", defaultValue2);
			if (this.type == EItemType.GUN || this.type == EItemType.MELEE || ItemAsset.shouldAlwaysLoadItemPrefab)
			{
				this._item = bundle.load<GameObject>("Item");
				if (this.item == null)
				{
					throw new NotSupportedException("missing 'Item' GameObject");
				}
				if (this.item.transform.Find("Icon") != null && this.item.transform.Find("Icon").GetComponent<Camera>() != null)
				{
					throw new NotSupportedException("'Icon' has a camera attached!");
				}
				if (this.id < 2000 && (this.type == EItemType.GUN || this.type == EItemType.MELEE) && this.item.transform.Find("Stat_Tracker") == null)
				{
					Assets.reportError(this, "missing 'Stat_Tracker' Transform");
				}
				AssetValidation.searchGameObjectForErrors(this, this.item);
			}
			byte b = data.ParseUInt8("Blueprints", 0);
			byte b2 = data.ParseUInt8("Actions", 0);
			bool flag = data.ParseBool("Add_Default_Actions", b2 == 0);
			this._blueprints = new List<Blueprint>((int)b);
			this._actions = new List<Action>((int)b2);
			for (byte b3 = 0; b3 < b; b3 += 1)
			{
				if (!data.ContainsKey("Blueprint_" + b3.ToString() + "_Type"))
				{
					throw new NotSupportedException("Missing blueprint type");
				}
				EBlueprintType newType = (EBlueprintType)Enum.Parse(typeof(EBlueprintType), data.GetString("Blueprint_" + b3.ToString() + "_Type", null), true);
				byte b4 = data.ParseUInt8("Blueprint_" + b3.ToString() + "_Supplies", 0);
				if (b4 < 1)
				{
					b4 = 1;
				}
				BlueprintSupply[] array = new BlueprintSupply[(int)b4];
				byte b5 = 0;
				while ((int)b5 < array.Length)
				{
					ushort newID = data.ParseUInt16(string.Concat(new string[]
					{
						"Blueprint_",
						b3.ToString(),
						"_Supply_",
						b5.ToString(),
						"_ID"
					}), 0);
					bool newCritical = data.ContainsKey(string.Concat(new string[]
					{
						"Blueprint_",
						b3.ToString(),
						"_Supply_",
						b5.ToString(),
						"_Critical"
					}));
					byte b6 = data.ParseUInt8(string.Concat(new string[]
					{
						"Blueprint_",
						b3.ToString(),
						"_Supply_",
						b5.ToString(),
						"_Amount"
					}), 0);
					if (b6 < 1)
					{
						b6 = 1;
					}
					array[(int)b5] = new BlueprintSupply(newID, newCritical, b6);
					b5 += 1;
				}
				byte b7 = data.ParseUInt8("Blueprint_" + b3.ToString() + "_Outputs", 0);
				BlueprintOutput[] array2;
				if (b7 > 0)
				{
					array2 = new BlueprintOutput[(int)b7];
					byte b8 = 0;
					while ((int)b8 < array2.Length)
					{
						ushort newID2 = data.ParseUInt16(string.Concat(new string[]
						{
							"Blueprint_",
							b3.ToString(),
							"_Output_",
							b8.ToString(),
							"_ID"
						}), 0);
						byte b9 = data.ParseUInt8(string.Concat(new string[]
						{
							"Blueprint_",
							b3.ToString(),
							"_Output_",
							b8.ToString(),
							"_Amount"
						}), 0);
						if (b9 < 1)
						{
							b9 = 1;
						}
						EItemOrigin newOrigin = data.ParseEnum<EItemOrigin>(string.Concat(new string[]
						{
							"Blueprint_",
							b3.ToString(),
							"_Output_",
							b8.ToString(),
							"_Origin"
						}), EItemOrigin.CRAFT);
						array2[(int)b8] = new BlueprintOutput(newID2, b9, newOrigin);
						b8 += 1;
					}
				}
				else
				{
					array2 = new BlueprintOutput[1];
					ushort num = data.ParseUInt16("Blueprint_" + b3.ToString() + "_Product", 0);
					if (num == 0)
					{
						num = this.id;
					}
					byte b10 = data.ParseUInt8("Blueprint_" + b3.ToString() + "_Products", 0);
					if (b10 < 1)
					{
						b10 = 1;
					}
					EItemOrigin newOrigin2 = data.ParseEnum<EItemOrigin>("Blueprint_" + b3.ToString() + "_Origin", EItemOrigin.CRAFT);
					array2[0] = new BlueprintOutput(num, b10, newOrigin2);
				}
				ushort newTool = data.ParseUInt16("Blueprint_" + b3.ToString() + "_Tool", 0);
				bool newToolCritical = data.ContainsKey("Blueprint_" + b3.ToString() + "_Tool_Critical");
				Guid newBuildEffectGuid;
				ushort newBuild = data.ParseGuidOrLegacyId("Blueprint_" + b3.ToString() + "_Build", out newBuildEffectGuid);
				byte b11 = data.ParseUInt8("Blueprint_" + b3.ToString() + "_Level", 0);
				EBlueprintSkill newSkill = EBlueprintSkill.NONE;
				if (b11 > 0)
				{
					newSkill = (EBlueprintSkill)Enum.Parse(typeof(EBlueprintSkill), data.GetString("Blueprint_" + b3.ToString() + "_Skill", null), true);
				}
				bool newTransferState = data.ContainsKey("Blueprint_" + b3.ToString() + "_State_Transfer");
				string @string = data.GetString("Blueprint_" + b3.ToString() + "_Map", null);
				INPCCondition[] array3 = new INPCCondition[(int)data.ParseUInt8("Blueprint_" + b3.ToString() + "_Conditions", 0)];
				NPCTool.readConditions(data, localization, "Blueprint_" + b3.ToString() + "_Condition_", array3, this);
				NPCRewardsList newQuestRewardsList = default(NPCRewardsList);
				newQuestRewardsList.Parse(data, localization, this, "Blueprint_" + b3.ToString() + "_Rewards", "Blueprint_" + b3.ToString() + "_Reward_");
				Blueprint blueprint = new Blueprint(this, b3, newType, array, array2, newTool, newToolCritical, newBuild, newBuildEffectGuid, b11, newSkill, newTransferState, @string, array3, newQuestRewardsList);
				blueprint.canBeVisibleWhenSearchedWithoutRequiredItems = data.ParseBool(string.Format("Blueprint_{0}_Searchable", b3), true);
				this.blueprints.Add(blueprint);
			}
			for (byte b12 = 0; b12 < b2; b12 += 1)
			{
				if (!data.ContainsKey("Action_" + b12.ToString() + "_Type"))
				{
					throw new NotSupportedException("Missing action type");
				}
				EActionType newType2 = (EActionType)Enum.Parse(typeof(EActionType), data.GetString("Action_" + b12.ToString() + "_Type", null), true);
				byte b13 = data.ParseUInt8("Action_" + b12.ToString() + "_Blueprints", 0);
				if (b13 < 1)
				{
					b13 = 1;
				}
				ActionBlueprint[] array4 = new ActionBlueprint[(int)b13];
				byte b14 = 0;
				while ((int)b14 < array4.Length)
				{
					byte newID3 = data.ParseUInt8(string.Concat(new string[]
					{
						"Action_",
						b12.ToString(),
						"_Blueprint_",
						b14.ToString(),
						"_Index"
					}), 0);
					bool newLink = data.ContainsKey(string.Concat(new string[]
					{
						"Action_",
						b12.ToString(),
						"_Blueprint_",
						b14.ToString(),
						"_Link"
					}));
					array4[(int)b14] = new ActionBlueprint(newID3, newLink);
					b14 += 1;
				}
				string string2 = data.GetString("Action_" + b12.ToString() + "_Key", null);
				string newText;
				string newTooltip;
				if (string.IsNullOrEmpty(string2))
				{
					string key = "Action_" + b12.ToString() + "_Text";
					if (localization.has(key))
					{
						newText = localization.format(key);
					}
					else
					{
						newText = data.GetString(key, null);
					}
					string key2 = "Action_" + b12.ToString() + "_Tooltip";
					if (localization.has(key2))
					{
						newTooltip = localization.format(key2);
					}
					else
					{
						newTooltip = data.GetString(key2, null);
					}
				}
				else
				{
					newText = string.Empty;
					newTooltip = string.Empty;
				}
				ushort num2 = data.ParseUInt16("Action_" + b12.ToString() + "_Source", 0);
				if (num2 == 0)
				{
					num2 = this.id;
				}
				this.actions.Add(new Action(num2, newType2, array4, newText, newTooltip, string2));
			}
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				byte b15 = 0;
				while ((int)b15 < this.blueprints.Count)
				{
					Blueprint blueprint2 = this.blueprints[(int)b15];
					if (blueprint2.type == EBlueprintType.REPAIR)
					{
						if (!flag3)
						{
							flag3 = true;
							Action action = new Action(this.id, EActionType.BLUEPRINT, new ActionBlueprint[]
							{
								new ActionBlueprint(b15, true)
							}, null, null, "Repair");
							this.actions.Insert(0, action);
						}
					}
					else if (blueprint2.type == EBlueprintType.AMMO)
					{
						flag2 = true;
					}
					else if (blueprint2.supplies.Length == 1 && blueprint2.supplies[0].id == this.id && !flag4)
					{
						flag4 = true;
						Action action2 = new Action(this.id, EActionType.BLUEPRINT, new ActionBlueprint[]
						{
							new ActionBlueprint(b15, this.type == EItemType.GUN || this.type == EItemType.MELEE)
						}, null, null, "Salvage");
						this.actions.Add(action2);
					}
					b15 += 1;
				}
				if (flag2)
				{
					List<ActionBlueprint> list = new List<ActionBlueprint>();
					byte b16 = 0;
					while ((int)b16 < this.blueprints.Count)
					{
						if (this.blueprints[(int)b16].type == EBlueprintType.AMMO)
						{
							ActionBlueprint actionBlueprint = new ActionBlueprint(b16, true);
							list.Add(actionBlueprint);
						}
						b16 += 1;
					}
					Action action3 = new Action(this.id, EActionType.BLUEPRINT, list.ToArray(), null, null, "Refill");
					this.actions.Add(action3);
				}
			}
			this._shouldVerifyHash = !data.ContainsKey("Bypass_Hash_Verification");
			this.overrideShowQuality = data.ContainsKey("Override_Show_Quality");
			this.shouldDropOnDeath = data.ParseBool("Should_Drop_On_Death", true);
			this.allowManualDrop = data.ParseBool("Allow_Manual_Drop", true);
			this.shouldDeleteAtZeroQuality = data.ParseBool("Should_Delete_At_Zero_Quality", false);
			this.shouldDestroyItemColliders = data.ParseBool("Destroy_Item_Colliders", true);
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0004EE6F File Offset: 0x0004D06F
		protected virtual AudioReference GetDefaultInventoryAudio()
		{
			if (this.size_x < 2 && this.size_y < 2)
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/LightGrab.asset");
			}
			return new AudioReference("core.masterbundle", "Sounds/Inventory/RoughGrab.asset");
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x0004EEA2 File Offset: 0x0004D0A2
		protected int DescSort_HigherIsBeneficial(float value)
		{
			if (value <= 1f)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x0004EEAF File Offset: 0x0004D0AF
		protected int DescSort_LowerIsBeneficial(float value)
		{
			if (value >= 1f)
			{
				return 1;
			}
			return -1;
		}

		/// <summary>
		/// Helper for plugins that want item prefabs server-side.
		/// e.g. Allows item icons to be captured on dedicated server.
		/// </summary>
		// Token: 0x0400086C RID: 2156
		public static CommandLineFlag shouldAlwaysLoadItemPrefab = new CommandLineFlag(false, "-AlwaysLoadItemPrefab");

		// Token: 0x0400086D RID: 2157
		protected bool _shouldVerifyHash;

		// Token: 0x0400086E RID: 2158
		protected string _itemName;

		// Token: 0x0400086F RID: 2159
		protected string _itemDescription;

		// Token: 0x04000870 RID: 2160
		public EItemType type;

		// Token: 0x04000871 RID: 2161
		public EItemRarity rarity;

		// Token: 0x04000872 RID: 2162
		protected string _proPath;

		/// <summary>
		/// Hack for Kuwait aura icons.
		/// </summary>
		// Token: 0x04000873 RID: 2163
		public bool econIconUseId;

		// Token: 0x04000874 RID: 2164
		public bool isPro;

		// Token: 0x04000875 RID: 2165
		public string useable;

		// Token: 0x04000878 RID: 2168
		public ESlotType slot;

		// Token: 0x0400087A RID: 2170
		public byte size_x;

		// Token: 0x0400087B RID: 2171
		public byte size_y;

		/// <summary>
		/// Vertical half size of icon camera.
		/// Values less than zero are disabled.
		/// </summary>
		// Token: 0x0400087C RID: 2172
		public float iconCameraOrthographicSize;

		/// <summary>
		/// Vertical half size of economy icon camera.
		/// </summary>
		// Token: 0x0400087D RID: 2173
		public float econIconCameraOrthographicSize;

		/// <summary>
		/// Should the newer automatic placement and orthographic size for axis-aligned icon cameras be used?
		/// Enabled by default, but optionally disabled for manual adjustment.
		/// </summary>
		// Token: 0x0400087E RID: 2174
		public bool isEligibleForAutomaticIconMeasurements;

		// Token: 0x0400087F RID: 2175
		public byte amount;

		// Token: 0x04000880 RID: 2176
		public byte countMin;

		// Token: 0x04000881 RID: 2177
		public byte countMax;

		// Token: 0x04000882 RID: 2178
		public byte qualityMin;

		// Token: 0x04000883 RID: 2179
		public byte qualityMax;

		// Token: 0x04000885 RID: 2181
		[Obsolete("Renamed to ShouldAttachEquippedModelToLeftHand")]
		public bool isBackward;

		/// <summary>
		/// Whether viewmodel should procedurally animate inertia of equipped item.
		/// Useful for low-quality older animations, but modders may wish to disable for high-quality newer animations.
		/// </summary>
		// Token: 0x04000886 RID: 2182
		public bool shouldProcedurallyAnimateInertia;

		/// <summary>
		/// Defaults to true. If false, the equipped item model is flipped to counteract the flipped character.
		/// </summary>
		// Token: 0x04000887 RID: 2183
		public bool shouldLeftHandedCharactersMirrorEquippedItem;

		/// <summary>
		/// If true, stats like damage, accuracy, health, etc. are automatically appended to the description.
		/// Defaults to true.
		/// </summary>
		// Token: 0x04000888 RID: 2184
		public bool isEligibleForAutoStatDescriptions;

		// Token: 0x04000889 RID: 2185
		protected GameObject _item;

		/// <summary>
		/// Optional alternative item prefab specifically for the PlayerEquipment prefab spawned.
		/// </summary>
		// Token: 0x0400088A RID: 2186
		public GameObject equipablePrefab;

		/// <summary>
		/// Movement speed multiplier while the item is equipped in the hands.
		/// </summary>
		// Token: 0x0400088C RID: 2188
		public float equipableMovementSpeedMultiplier = 1f;

		// Token: 0x0400088D RID: 2189
		protected AudioClip _equip;

		// Token: 0x0400088E RID: 2190
		protected AnimationClip[] _animations;

		/// <summary>
		/// Sound to play when inspecting the equipped item.
		/// </summary>
		// Token: 0x0400088F RID: 2191
		public AudioReference inspectAudio;

		/// <summary>
		/// Sound to play when moving or rotating the item in the inventory.
		/// </summary>
		// Token: 0x04000890 RID: 2192
		public AudioReference inventoryAudio;

		// Token: 0x04000891 RID: 2193
		protected List<Blueprint> _blueprints;

		// Token: 0x04000892 RID: 2194
		protected List<Action> _actions;

		// Token: 0x04000896 RID: 2198
		protected Texture2D _albedoBase;

		// Token: 0x04000897 RID: 2199
		protected Texture2D _metallicBase;

		// Token: 0x04000898 RID: 2200
		protected Texture2D _emissionBase;

		/// sortOrder values for description lines.
		/// Difference in value greater than 100 creates an empty line.
		// Token: 0x0400089D RID: 2205
		protected const int DescSort_RarityAndType = 0;

		// Token: 0x0400089E RID: 2206
		protected const int DescSort_LoreText = 200;

		// Token: 0x0400089F RID: 2207
		protected const int DescSort_QualityAndAmount = 400;

		// Token: 0x040008A0 RID: 2208
		protected const int DescSort_Important = 2000;

		// Token: 0x040008A1 RID: 2209
		protected const int DescSort_ItemStat = 10000;

		// Token: 0x040008A2 RID: 2210
		protected const int DescSort_ClothingStat = 10000;

		// Token: 0x040008A3 RID: 2211
		protected const int DescSort_ConsumeableStat = 10000;

		// Token: 0x040008A4 RID: 2212
		protected const int DescSort_GunStat = 10000;

		// Token: 0x040008A5 RID: 2213
		protected const int DescSort_GunAttachmentStat = 10000;

		// Token: 0x040008A6 RID: 2214
		protected const int DescSort_MeleeStat = 10000;

		// Token: 0x040008A7 RID: 2215
		protected const int DescSort_RefillStat = 10000;

		/// <summary>
		/// Properties common to Gun and Melee.
		/// </summary>
		// Token: 0x040008A8 RID: 2216
		protected const int DescSort_Weapon_NonExplosive_Common = 10000;

		// Token: 0x040008A9 RID: 2217
		protected const int DescSort_TrapKeyword = 10001;

		// Token: 0x040008AA RID: 2218
		protected const int DescSort_TrapStat = 10002;

		// Token: 0x040008AB RID: 2219
		protected const int DescSort_FarmableText = 15000;

		/// <summary>
		/// Properties common to Barricade and Structure.
		/// </summary>
		// Token: 0x040008AC RID: 2220
		protected const int DescSort_BuildableCommon = 20000;

		// Token: 0x040008AD RID: 2221
		protected const int DescSort_ExplosiveBulletDamage = 30000;

		// Token: 0x040008AE RID: 2222
		protected const int DescSort_ExplosiveChargeDamage = 30000;

		// Token: 0x040008AF RID: 2223
		protected const int DescSort_ExplosiveTrapDamage = 30000;

		/// <summary>
		/// Properties common to Gun, Consumable, and Throwable.
		/// </summary>
		// Token: 0x040008B0 RID: 2224
		protected const int DescSort_Weapon_Explosive_RangeAndDamage = 30000;

		/// <summary>
		/// Properties common to Gun and Melee.
		/// </summary>
		// Token: 0x040008B1 RID: 2225
		protected const int DescSort_Weapon_NonExplosive_PlayerDamage = 30000;

		/// <summary>
		/// Properties common to Gun and Melee.
		/// </summary>
		// Token: 0x040008B2 RID: 2226
		protected const int DescSort_Weapon_NonExplosive_ZombieDamage = 31000;

		/// <summary>
		/// Properties common to Gun and Melee.
		/// </summary>
		// Token: 0x040008B3 RID: 2227
		protected const int DescSort_Weapon_NonExplosive_AnimalDamage = 32000;

		/// <summary>
		/// Properties common to Gun and Melee.
		/// </summary>
		// Token: 0x040008B4 RID: 2228
		protected const int DescSort_Weapon_NonExplosive_OtherDamage = 33000;

		// Token: 0x040008B5 RID: 2229
		protected const int DescSort_Beneficial = -1;

		// Token: 0x040008B6 RID: 2230
		protected const int DescSort_Detrimental = 1;
	}
}
