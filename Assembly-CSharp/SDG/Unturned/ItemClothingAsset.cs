using System;

namespace SDG.Unturned
{
	// Token: 0x020002D9 RID: 729
	public class ItemClothingAsset : ItemAsset
	{
		/// <summary>
		/// Multiplier to incoming damage. Defaults to 1.0.
		/// </summary>
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600158D RID: 5517 RVA: 0x0005025F File Offset: 0x0004E45F
		public float armor
		{
			get
			{
				return this._armor;
			}
		}

		/// <summary>
		/// Multiplier to explosive damage. Defaults to <see cref="P:SDG.Unturned.ItemClothingAsset.armor" /> value if Armor_Explosion isn't specified.
		/// </summary>
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600158E RID: 5518 RVA: 0x00050267 File Offset: 0x0004E467
		public float explosionArmor
		{
			get
			{
				return this._explosionArmor;
			}
		}

		/// <summary>
		/// Armor against falling damage. Defaults to 1.0, i.e., take the normal amount of damage.
		/// </summary>
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x0600158F RID: 5519 RVA: 0x0005026F File Offset: 0x0004E46F
		// (set) Token: 0x06001590 RID: 5520 RVA: 0x00050277 File Offset: 0x0004E477
		public float fallingDamageMultiplier { get; protected set; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06001591 RID: 5521 RVA: 0x00050280 File Offset: 0x0004E480
		public override bool showQuality
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001592 RID: 5522 RVA: 0x00050283 File Offset: 0x0004E483
		public bool proofWater
		{
			get
			{
				return this._proofWater;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001593 RID: 5523 RVA: 0x0005028B File Offset: 0x0004E48B
		public bool proofFire
		{
			get
			{
				return this._proofFire;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06001594 RID: 5524 RVA: 0x00050293 File Offset: 0x0004E493
		public bool proofRadiation
		{
			get
			{
				return this._proofRadiation;
			}
		}

		/// <summary>
		/// If true on any worn clothing item, bones never break when falling.
		/// Defaults to false.
		/// </summary>
		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06001595 RID: 5525 RVA: 0x0005029B File Offset: 0x0004E49B
		// (set) Token: 0x06001596 RID: 5526 RVA: 0x000502A3 File Offset: 0x0004E4A3
		public bool preventsFallingBrokenBones { get; protected set; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001597 RID: 5527 RVA: 0x000502AC File Offset: 0x0004E4AC
		// (set) Token: 0x06001598 RID: 5528 RVA: 0x000502B4 File Offset: 0x0004E4B4
		public bool visibleOnRagdoll { get; protected set; }

		// Token: 0x06001599 RID: 5529 RVA: 0x000502BD File Offset: 0x0004E4BD
		public bool shouldBeVisible(bool isRagdoll)
		{
			return !isRagdoll || this.visibleOnRagdoll;
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x0600159A RID: 5530 RVA: 0x000502CA File Offset: 0x0004E4CA
		// (set) Token: 0x0600159B RID: 5531 RVA: 0x000502D2 File Offset: 0x0004E4D2
		public bool hairVisible { get; protected set; }

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x0600159C RID: 5532 RVA: 0x000502DB File Offset: 0x0004E4DB
		// (set) Token: 0x0600159D RID: 5533 RVA: 0x000502E3 File Offset: 0x0004E4E3
		public bool beardVisible { get; protected set; }

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x0600159E RID: 5534 RVA: 0x000502EC File Offset: 0x0004E4EC
		// (set) Token: 0x0600159F RID: 5535 RVA: 0x000502F4 File Offset: 0x0004E4F4
		public bool shouldDestroyClothingColliders { get; protected set; }

		/// <summary>
		/// If set, find a child meshrenderer with this name and change its material to the character skin material.
		/// </summary>
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060015A0 RID: 5536 RVA: 0x000502FD File Offset: 0x0004E4FD
		// (set) Token: 0x060015A1 RID: 5537 RVA: 0x00050305 File Offset: 0x0004E505
		public string skinOverride { get; protected set; }

		/// <summary>
		/// The player can be wearing both a "real" in-game item and a cosmetic item in the same clothing slot.
		/// If true, the real item is shown rather than the cosmetic item. For example, night vision goggles
		/// are shown over any glasses cosmetic because of their gameplay-related green glow.
		/// </summary>
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x060015A2 RID: 5538 RVA: 0x0005030E File Offset: 0x0004E50E
		public bool TakesPriorityOverCosmetic
		{
			get
			{
				if (!this.hasPriorityOverCosmeticOverride)
				{
					return this.GetDefaultTakesPriorityOverCosmetic();
				}
				return this.priorityOverCosmeticOverride;
			}
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x00050328 File Offset: 0x0004E528
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this.type == EItemType.HAT || this.type == EItemType.SHIRT || this.type == EItemType.PANTS || this.type == EItemType.VEST)
			{
				if (this._armor != 1f)
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Clothing_Armor", PlayerDashboardInventoryUI.FormatStatModifier(this._armor, false, false)), 10000 + base.DescSort_LowerIsBeneficial(this._armor));
				}
				if (this._explosionArmor != 1f)
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Clothing_ExplosionArmor", PlayerDashboardInventoryUI.FormatStatModifier(this._explosionArmor, false, false)), 10000 + base.DescSort_LowerIsBeneficial(this._explosionArmor));
				}
			}
			if (this.movementSpeedMultiplier != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ClothingMovementSpeedModifier", PlayerDashboardInventoryUI.FormatStatModifier(this.movementSpeedMultiplier, true, true)), 10000 + base.DescSort_HigherIsBeneficial(this.movementSpeedMultiplier));
			}
			if (this.fallingDamageMultiplier != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_FallingDamageModifier", PlayerDashboardInventoryUI.FormatStatModifier(this.fallingDamageMultiplier, true, false)), 10000 + base.DescSort_LowerIsBeneficial(this.fallingDamageMultiplier));
			}
			if (this._proofFire)
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_Clothing_FireProof"), true), 9999);
			}
			if (this._proofRadiation)
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_Clothing_RadiationProof"), true), 9999);
			}
			if (this.preventsFallingBrokenBones)
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_Clothing_FallingBoneBreakingProof"), true), 9999);
			}
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x000504F0 File Offset: 0x0004E6F0
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (this.isPro)
			{
				this._armor = 1f;
				this._explosionArmor = 1f;
				this.fallingDamageMultiplier = 1f;
			}
			else
			{
				this._armor = data.ParseFloat("Armor", 0f);
				if (data.ContainsKey("Armor"))
				{
					this._armor = data.ParseFloat("Armor", 0f);
				}
				else
				{
					this._armor = 1f;
				}
				if (data.ContainsKey("Armor_Explosion"))
				{
					this._explosionArmor = data.ParseFloat("Armor_Explosion", 0f);
				}
				else
				{
					this._explosionArmor = this.armor;
				}
				this.fallingDamageMultiplier = data.ParseFloat("Falling_Damage_Multiplier", 1f);
				this._proofWater = data.ContainsKey("Proof_Water");
				this._proofFire = data.ContainsKey("Proof_Fire");
				this._proofRadiation = data.ContainsKey("Proof_Radiation");
				this.preventsFallingBrokenBones = data.ParseBool("Prevents_Falling_Broken_Bones", false);
				this.movementSpeedMultiplier = data.ParseFloat("Movement_Speed_Multiplier", 1f);
			}
			this.visibleOnRagdoll = data.ParseBool("Visible_On_Ragdoll", true);
			this.hairVisible = data.ParseBool("Hair_Visible", true);
			this.beardVisible = data.ParseBool("Beard_Visible", true);
			this.shouldMirrorLeftHandedModel = data.ParseBool("Mirror_Left_Handed_Model", true);
			if (data.ContainsKey("WearAudio"))
			{
				this.wearAudio = data.ReadAudioReference("WearAudio", bundle);
			}
			else if (this.type == EItemType.BACKPACK || this.type == EItemType.VEST)
			{
				this.wearAudio = new AudioReference("core.masterbundle", "Sounds/Zipper.mp3");
			}
			else
			{
				this.wearAudio = new AudioReference("core.masterbundle", "Sounds/Sleeve.mp3");
			}
			this.shouldDestroyClothingColliders = data.ParseBool("Destroy_Clothing_Colliders", true);
			this.hasPriorityOverCosmeticOverride = data.TryParseBool("Priority_Over_Cosmetic", out this.priorityOverCosmeticOverride);
			this.skinOverride = data.GetString("Skin_Override", null);
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00050700 File Offset: 0x0004E900
		protected override AudioReference GetDefaultInventoryAudio()
		{
			if (this.size_x <= 1 || this.size_y <= 1)
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/LightCloth.asset");
			}
			if (this.rarity == EItemRarity.COMMON || this.rarity == EItemRarity.UNCOMMON)
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/LightClothEquipment.asset");
			}
			return new AudioReference("core.masterbundle", "Sounds/Inventory/MediumClothEquipment.asset");
		}

		/// <summary>
		/// The player can be wearing both a "real" in-game item and a cosmetic item in the same clothing slot.
		/// This is called on the real item if <see cref="F:SDG.Unturned.ItemClothingAsset.priorityOverCosmeticOverride" /> has not been set.
		/// If true, the real item is shown rather than the cosmetic item. If false, the cosmetic item can be seen.
		/// </summary>
		// Token: 0x060015A6 RID: 5542 RVA: 0x0005075F File Offset: 0x0004E95F
		protected virtual bool GetDefaultTakesPriorityOverCosmetic()
		{
			return false;
		}

		// Token: 0x040008FF RID: 2303
		protected float _armor;

		// Token: 0x04000900 RID: 2304
		protected float _explosionArmor;

		// Token: 0x04000902 RID: 2306
		private bool _proofWater;

		// Token: 0x04000903 RID: 2307
		private bool _proofFire;

		// Token: 0x04000904 RID: 2308
		private bool _proofRadiation;

		/// <summary>
		/// Left-handed character skeleton is mirrored, so most item models are mirrored again to preserve appearance.
		/// Unfortunately this does not work well for some items e.g. the particle system on Elver/Dango glasses.
		/// </summary>
		// Token: 0x04000909 RID: 2313
		internal bool shouldMirrorLeftHandedModel;

		// Token: 0x0400090A RID: 2314
		public float movementSpeedMultiplier = 1f;

		/// <summary>
		/// Sound to play when equipped.
		/// </summary>
		// Token: 0x0400090B RID: 2315
		public AudioReference wearAudio;

		/// <summary>
		/// Overrides value of TakesPriorityOverCosmetic if <see cref="F:SDG.Unturned.ItemClothingAsset.hasPriorityOverCosmeticOverride" /> is true.
		/// </summary>
		// Token: 0x0400090E RID: 2318
		protected bool priorityOverCosmeticOverride;

		/// <summary>
		/// If true, the value of <see cref="F:SDG.Unturned.ItemClothingAsset.priorityOverCosmeticOverride" /> is used rather than <see cref="M:SDG.Unturned.ItemClothingAsset.GetDefaultTakesPriorityOverCosmetic" />.
		/// Defaults to false. True if <see cref="F:SDG.Unturned.ItemClothingAsset.priorityOverCosmeticOverride" /> is set.
		/// </summary>
		// Token: 0x0400090F RID: 2319
		protected bool hasPriorityOverCosmeticOverride;
	}
}
