using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002D5 RID: 725
	public class ItemBarricadeAsset : ItemPlaceableAsset
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x0004F192 File Offset: 0x0004D392
		public GameObject barricade
		{
			get
			{
				return this._barricade;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x0004F19A File Offset: 0x0004D39A
		[Obsolete("Only one of Barricade.prefab or Clip.prefab are loaded now as _barricade")]
		public GameObject clip
		{
			get
			{
				return this._barricade;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x0004F1A2 File Offset: 0x0004D3A2
		public GameObject nav
		{
			get
			{
				return this._nav;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001543 RID: 5443 RVA: 0x0004F1AA File Offset: 0x0004D3AA
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0004F1B4 File Offset: 0x0004D3B4
		public override byte[] getState(EItemOrigin origin)
		{
			if (this.build == EBuild.DOOR || this.build == EBuild.GATE || this.build == EBuild.SHUTTER || this.build == EBuild.HATCH)
			{
				return new byte[17];
			}
			if (this.build == EBuild.BED)
			{
				return new byte[8];
			}
			if (this.build == EBuild.FARM)
			{
				byte[] array = new byte[4];
				BitConverter.TryWriteBytes(array, Provider.time);
				return array;
			}
			if (this.build == EBuild.TORCH || this.build == EBuild.CAMPFIRE || this.build == EBuild.OVEN || this.build == EBuild.SPOT || this.build == EBuild.SAFEZONE || this.build == EBuild.OXYGENATOR || this.build == EBuild.BARREL_RAIN || this.build == EBuild.CAGE)
			{
				return new byte[1];
			}
			if (this.build == EBuild.OIL)
			{
				return new byte[2];
			}
			if (this.build == EBuild.SIGN || this.build == EBuild.SIGN_WALL || this.build == EBuild.NOTE)
			{
				return new byte[17];
			}
			if (this.build == EBuild.STEREO)
			{
				return new byte[17];
			}
			if (this.build == EBuild.MANNEQUIN)
			{
				return new byte[73];
			}
			return new byte[0];
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06001545 RID: 5445 RVA: 0x0004F2D7 File Offset: 0x0004D4D7
		public EBuild build
		{
			get
			{
				return this._build;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x0004F2DF File Offset: 0x0004D4DF
		public ushort health
		{
			get
			{
				return this._health;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06001547 RID: 5447 RVA: 0x0004F2E7 File Offset: 0x0004D4E7
		public float range
		{
			get
			{
				return this._range;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x0004F2EF File Offset: 0x0004D4EF
		public float radius
		{
			get
			{
				return this._radius;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001549 RID: 5449 RVA: 0x0004F2F7 File Offset: 0x0004D4F7
		public float offset
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600154A RID: 5450 RVA: 0x0004F2FF File Offset: 0x0004D4FF
		public Guid explosionGuid
		{
			get
			{
				return this._explosionGuid;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x0004F307 File Offset: 0x0004D507
		public ushort explosion
		{
			[Obsolete]
			get
			{
				return this._explosion;
			}
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0004F30F File Offset: 0x0004D50F
		public EffectAsset FindExplosionEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this._explosionGuid, this._explosion);
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x0004F322 File Offset: 0x0004D522
		public bool isLocked
		{
			get
			{
				return this._isLocked;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x0004F32A File Offset: 0x0004D52A
		public bool isVulnerable
		{
			get
			{
				return this._isVulnerable;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x0600154F RID: 5455 RVA: 0x0004F332 File Offset: 0x0004D532
		// (set) Token: 0x06001550 RID: 5456 RVA: 0x0004F33A File Offset: 0x0004D53A
		public EArmorTier armorTier { get; protected set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001551 RID: 5457 RVA: 0x0004F343 File Offset: 0x0004D543
		public bool bypassClaim
		{
			get
			{
				return this._bypassClaim;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x0004F34B File Offset: 0x0004D54B
		// (set) Token: 0x06001553 RID: 5459 RVA: 0x0004F353 File Offset: 0x0004D553
		public bool allowPlacementOnVehicle { get; protected set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x0004F35C File Offset: 0x0004D55C
		public bool isRepairable
		{
			get
			{
				return this._isRepairable;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06001555 RID: 5461 RVA: 0x0004F364 File Offset: 0x0004D564
		public bool proofExplosion
		{
			get
			{
				return this._proofExplosion;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x0004F36C File Offset: 0x0004D56C
		public bool isUnpickupable
		{
			get
			{
				return this._isUnpickupable;
			}
		}

		/// <summary>
		/// Defaults to false, except for explosive charges which bypass claims.
		/// If true the item can be placed inside player clip volumes. (out of bounds)
		/// </summary>
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001557 RID: 5463 RVA: 0x0004F374 File Offset: 0x0004D574
		// (set) Token: 0x06001558 RID: 5464 RVA: 0x0004F37C File Offset: 0x0004D57C
		public bool AllowPlacementInsideClipVolumes { get; private set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001559 RID: 5465 RVA: 0x0004F385 File Offset: 0x0004D585
		public bool isSalvageable
		{
			get
			{
				return this._isSalvageable;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x0004F38D File Offset: 0x0004D58D
		// (set) Token: 0x0600155B RID: 5467 RVA: 0x0004F395 File Offset: 0x0004D595
		public float salvageDurationMultiplier { get; protected set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x0004F39E File Offset: 0x0004D59E
		public bool isSaveable
		{
			get
			{
				return this._isSaveable;
			}
		}

		/// <summary>
		/// Should door colliders remain active while animation is playing?
		/// Enabled by modders trying to make stuff like elevators.
		/// </summary>
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x0600155D RID: 5469 RVA: 0x0004F3A6 File Offset: 0x0004D5A6
		// (set) Token: 0x0600155E RID: 5470 RVA: 0x0004F3AE File Offset: 0x0004D5AE
		public bool allowCollisionWhileAnimating { get; protected set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x0600155F RID: 5471 RVA: 0x0004F3B7 File Offset: 0x0004D5B7
		public override bool shouldFriendlySentryTargetUser
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0004F3BA File Offset: 0x0004D5BA
		public override bool canBeUsedInSafezone(SafezoneNode safezone, bool byAdmin)
		{
			return !safezone.noBuildables;
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x0004F3C5 File Offset: 0x0004D5C5
		// (set) Token: 0x06001562 RID: 5474 RVA: 0x0004F3CD File Offset: 0x0004D5CD
		public bool useWaterHeightTransparentSort { get; protected set; }

		/// <summary>
		/// Vehicle to place.
		/// Supports redirects by VehicleRedirectorAsset. If redirector's SpawnPaintColor is set, that color is used.
		/// </summary>
		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06001563 RID: 5475 RVA: 0x0004F3D6 File Offset: 0x0004D5D6
		public Guid VehicleGuid
		{
			get
			{
				return this._vehicleGuid;
			}
		}

		/// <summary>
		/// Legacy ID of vehicle to place.
		/// Supports redirects by VehicleRedirectorAsset. If redirector's SpawnPaintColor is set, that color is used.
		/// </summary>
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06001564 RID: 5476 RVA: 0x0004F3DE File Offset: 0x0004D5DE
		public ushort VehicleId
		{
			[Obsolete]
			get
			{
				return this._vehicleId;
			}
		}

		/// <summary>
		/// Returned asset is not necessarily a vehicle asset yet: It can also be a VehicleRedirectorAsset which the
		/// vehicle spawner requires to properly set paint color.
		/// </summary>
		// Token: 0x06001565 RID: 5477 RVA: 0x0004F3E6 File Offset: 0x0004D5E6
		internal Asset FindVehicleAsset()
		{
			return Assets.FindBaseVehicleAssetByGuidOrLegacyId(this._vehicleGuid, this._vehicleId);
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0004F3FC File Offset: 0x0004D5FC
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this.build == EBuild.VEHICLE)
			{
				return;
			}
			if (this._health > 0)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_Health", this._health), 20000);
			}
			EArmorTier armorTier = this.armorTier;
			if (armorTier != EArmorTier.LOW)
			{
				if (armorTier == EArmorTier.HIGH)
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_ArmorTier_High"), 20000);
				}
			}
			else
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_ArmorTier_Low"), 20000);
			}
			if (this._isUnpickupable)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_CannotPickup"), 20000);
			}
			else if (!this._isSalvageable)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_CannotSalvage"), 20000);
			}
			if (!this.isRepairable)
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_CannotRepair"), false), 20001);
			}
			if (this.proofExplosion)
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_ExplosionProof"), true), 19999);
			}
			if (this.isLocked)
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_Lockable"), true), 19999);
			}
			if (!this._isVulnerable)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_Invulnerable"), 19999);
			}
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0004F584 File Offset: 0x0004D784
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			bool flag;
			if (data.ParseBool("Has_Clip_Prefab", true))
			{
				this._barricade = bundle.load<GameObject>("Clip");
				if (this.barricade == null)
				{
					flag = true;
					Assets.reportError(this, "missing \"Clip\" GameObject, loading \"Barricade\" GameObject instead");
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				this._barricade = bundle.load<GameObject>("Barricade");
				if (this.barricade == null)
				{
					Assets.reportError(this, "missing \"Barricade\" GameObject");
				}
				else
				{
					ServerPrefabUtil.RemoveClientComponents(this._barricade);
				}
			}
			if (this.barricade != null)
			{
				if (Assets.shouldValidateAssets)
				{
					AssetValidation.searchGameObjectForErrors(this, this.barricade);
				}
				this.barricade.transform.localPosition = Vector3.zero;
				this.barricade.transform.localRotation = Quaternion.identity;
			}
			this._nav = bundle.load<GameObject>("Nav");
			this._use = base.LoadRedirectableAsset<AudioClip>(bundle, "Use", data, "PlacementAudioClip");
			this._build = (EBuild)Enum.Parse(typeof(EBuild), data.GetString("Build", null), true);
			if ((this.build == EBuild.DOOR || this.build == EBuild.GATE || this.build == EBuild.SHUTTER) && this.barricade != null && this.barricade.transform.Find("Placeholder") == null)
			{
				Assets.reportError(this, "missing 'Placeholder' Collider");
			}
			this._health = data.ParseUInt16("Health", 0);
			this._range = data.ParseFloat("Range", 0f);
			this._radius = data.ParseFloat("Radius", 0f);
			this._offset = data.ParseFloat("Offset", 0f);
			if (this.radius > 0.05f && Mathf.Abs(this.radius - this.offset) < 0.05f)
			{
				this._radius -= 0.05f;
			}
			this._explosion = data.ParseGuidOrLegacyId("Explosion", out this._explosionGuid);
			if (this.build == EBuild.VEHICLE)
			{
				this._vehicleId = this._explosion;
				this._vehicleGuid = this._explosionGuid;
			}
			this.canBeDamaged = data.ParseBool("Can_Be_Damaged", true);
			bool defaultValue = this.build != EBuild.BEACON;
			this.eligibleForPooling = data.ParseBool("Eligible_For_Pooling", defaultValue);
			this._isLocked = data.ContainsKey("Locked");
			this._isVulnerable = data.ContainsKey("Vulnerable");
			this._bypassClaim = data.ContainsKey("Bypass_Claim");
			bool defaultValue2 = this.build != EBuild.BED && this.build != EBuild.SENTRY && this.build != EBuild.SENTRY_FREEFORM;
			this.allowPlacementOnVehicle = data.ParseBool("Allow_Placement_On_Vehicle", defaultValue2);
			this._isRepairable = !data.ContainsKey("Unrepairable");
			this._proofExplosion = data.ContainsKey("Proof_Explosion");
			this._isUnpickupable = data.ContainsKey("Unpickupable");
			this.shouldBypassPickupOwnership = data.ParseBool("Bypass_Pickup_Ownership", this.build == EBuild.CHARGE);
			this.AllowPlacementInsideClipVolumes = data.ParseBool("Allow_Placement_Inside_Clip_Volumes", this.build == EBuild.CHARGE);
			this._isSalvageable = !data.ContainsKey("Unsalvageable");
			this.salvageDurationMultiplier = data.ParseFloat("Salvage_Duration_Multiplier", 1f);
			this._isSaveable = !data.ContainsKey("Unsaveable");
			this.allowCollisionWhileAnimating = data.ParseBool("Allow_Collision_While_Animating", false);
			this.useWaterHeightTransparentSort = data.ContainsKey("Use_Water_Height_Transparent_Sort");
			if (data.ContainsKey("Armor_Tier"))
			{
				this.armorTier = (EArmorTier)Enum.Parse(typeof(EArmorTier), data.GetString("Armor_Tier", null), true);
				return;
			}
			if (this.name.Contains("Metal"))
			{
				this.armorTier = EArmorTier.HIGH;
				return;
			}
			this.armorTier = EArmorTier.LOW;
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0004F98C File Offset: 0x0004DB8C
		protected override AudioReference GetDefaultInventoryAudio()
		{
			if (this.name.Contains("Seed", 3))
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/Seeds.asset");
			}
			if (this.name.Contains("Metal", 3))
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/SmallMetal.asset");
			}
			if (this.size_x <= 1 || this.size_y <= 1)
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/LightMetalEquipment.asset");
			}
			if (this.size_x <= 2 || this.size_y <= 2)
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/MediumMetalEquipment.asset");
			}
			return new AudioReference("core.masterbundle", "Sounds/Inventory/HeavyMetalEquipment.asset");
		}

		// Token: 0x040008C2 RID: 2242
		protected GameObject _barricade;

		// Token: 0x040008C3 RID: 2243
		protected GameObject _nav;

		// Token: 0x040008C4 RID: 2244
		protected AudioClip _use;

		// Token: 0x040008C5 RID: 2245
		protected EBuild _build;

		// Token: 0x040008C6 RID: 2246
		protected ushort _health;

		// Token: 0x040008C7 RID: 2247
		protected float _range;

		// Token: 0x040008C8 RID: 2248
		protected float _radius;

		// Token: 0x040008C9 RID: 2249
		protected float _offset;

		// Token: 0x040008CA RID: 2250
		private Guid _explosionGuid;

		// Token: 0x040008CB RID: 2251
		protected ushort _explosion;

		/// <summary>
		/// If false this barricade cannot take damage.
		/// </summary>
		// Token: 0x040008CC RID: 2252
		public bool canBeDamaged = true;

		/// <summary>
		/// Modded barricades can disable pooling if they have custom incompatible logic.
		/// </summary>
		// Token: 0x040008CD RID: 2253
		public bool eligibleForPooling = true;

		// Token: 0x040008CE RID: 2254
		protected bool _isLocked;

		// Token: 0x040008CF RID: 2255
		protected bool _isVulnerable;

		// Token: 0x040008D1 RID: 2257
		protected bool _bypassClaim;

		// Token: 0x040008D3 RID: 2259
		protected bool _isRepairable;

		// Token: 0x040008D4 RID: 2260
		protected bool _proofExplosion;

		// Token: 0x040008D5 RID: 2261
		protected bool _isUnpickupable;

		/// <summary>
		/// Defaults to false, except for explosive charges which bypass claims.
		/// Requested by Renaxon for collectible barricades that raiders can steal without destroying.
		/// </summary>
		// Token: 0x040008D6 RID: 2262
		public bool shouldBypassPickupOwnership;

		// Token: 0x040008D8 RID: 2264
		protected bool _isSalvageable;

		// Token: 0x040008DA RID: 2266
		protected bool _isSaveable;

		// Token: 0x040008DD RID: 2269
		private Guid _vehicleGuid;

		// Token: 0x040008DE RID: 2270
		private ushort _vehicleId;
	}
}
