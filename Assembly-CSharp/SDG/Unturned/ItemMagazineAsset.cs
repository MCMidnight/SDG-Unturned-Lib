using System;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x020002EE RID: 750
	public class ItemMagazineAsset : ItemCaliberAsset
	{
		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x0600166F RID: 5743 RVA: 0x00053385 File Offset: 0x00051585
		public GameObject magazine
		{
			get
			{
				return this._magazine;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x0005338D File Offset: 0x0005158D
		public byte pellets
		{
			get
			{
				return this._pellets;
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001671 RID: 5745 RVA: 0x00053395 File Offset: 0x00051595
		public byte stuck
		{
			get
			{
				return this._stuck;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x0005339D File Offset: 0x0005159D
		public float range
		{
			get
			{
				return this._range;
			}
		}

		/// <summary>
		/// Multiplier for explosive projectile damage.
		/// </summary>
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001673 RID: 5747 RVA: 0x000533A5 File Offset: 0x000515A5
		// (set) Token: 0x06001674 RID: 5748 RVA: 0x000533AD File Offset: 0x000515AD
		public float projectileDamageMultiplier { get; protected set; }

		/// <summary>
		/// Multiplier for explosive projectile's blast radius.
		/// </summary>
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001675 RID: 5749 RVA: 0x000533B6 File Offset: 0x000515B6
		// (set) Token: 0x06001676 RID: 5750 RVA: 0x000533BE File Offset: 0x000515BE
		public float projectileBlastRadiusMultiplier { get; protected set; }

		/// <summary>
		/// Multiplier for explosive projectile's initial force.
		/// </summary>
		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001677 RID: 5751 RVA: 0x000533C7 File Offset: 0x000515C7
		// (set) Token: 0x06001678 RID: 5752 RVA: 0x000533CF File Offset: 0x000515CF
		public float projectileLaunchForceMultiplier { get; protected set; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x000533D8 File Offset: 0x000515D8
		public ushort explosion
		{
			get
			{
				return this._explosion;
			}
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x000533E0 File Offset: 0x000515E0
		public bool IsExplosionEffectRefNull()
		{
			return this.explosion == 0 && GuidExtension.IsEmpty(this.explosionEffectGuid);
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x000533F7 File Offset: 0x000515F7
		public EffectAsset FindExplosionEffect()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this.explosionEffectGuid, this.explosion);
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x0600167C RID: 5756 RVA: 0x0005340A File Offset: 0x0005160A
		// (set) Token: 0x0600167D RID: 5757 RVA: 0x00053412 File Offset: 0x00051612
		public bool spawnExplosionOnDedicatedServer { get; protected set; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x0600167E RID: 5758 RVA: 0x0005341B File Offset: 0x0005161B
		public ushort tracer
		{
			[Obsolete]
			get
			{
				return this._tracer;
			}
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00053423 File Offset: 0x00051623
		public EffectAsset FindTracerEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this.tracerEffectGuid, this._tracer);
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001680 RID: 5760 RVA: 0x00053436 File Offset: 0x00051636
		public Guid ImpactEffectGuid
		{
			get
			{
				return this._impactEffectGuid;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001681 RID: 5761 RVA: 0x0005343E File Offset: 0x0005163E
		public ushort impact
		{
			[Obsolete]
			get
			{
				return this._impact;
			}
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00053446 File Offset: 0x00051646
		public bool IsImpactEffectRefNull()
		{
			return this._impact == 0 && GuidExtension.IsEmpty(this._impactEffectGuid);
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x0005345D File Offset: 0x0005165D
		public EffectAsset FindImpactEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this._impactEffectGuid, this._impact);
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001684 RID: 5764 RVA: 0x00053470 File Offset: 0x00051670
		public override bool showQuality
		{
			get
			{
				return this.stuck > 0;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x0005347B File Offset: 0x0005167B
		public float speed
		{
			get
			{
				return this._speed;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001686 RID: 5766 RVA: 0x00053483 File Offset: 0x00051683
		public bool isExplosive
		{
			get
			{
				return this._isExplosive;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06001687 RID: 5767 RVA: 0x0005348B File Offset: 0x0005168B
		public bool deleteEmpty
		{
			get
			{
				return this._deleteEmpty;
			}
		}

		/// <summary>
		/// Should amount be filled to capacity when detached?
		/// </summary>
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06001688 RID: 5768 RVA: 0x00053493 File Offset: 0x00051693
		// (set) Token: 0x06001689 RID: 5769 RVA: 0x0005349B File Offset: 0x0005169B
		public bool shouldFillAfterDetach { get; protected set; }

		// Token: 0x0600168A RID: 5770 RVA: 0x000534A4 File Offset: 0x000516A4
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this._pellets > 1)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_PelletCount", this._pellets), 10000);
			}
			if (this.isExplosive)
			{
				int sortOrder = 30000;
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosiveBullet"), true), sortOrder++);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionBlastRadius", MeasurementTool.FormatLengthString(this.range)), sortOrder++);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionPlayerDamage", Mathf.RoundToInt(this.playerDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionZombieDamage", Mathf.RoundToInt(this.zombieDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionAnimalDamage", Mathf.RoundToInt(this.animalDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionBarricadeDamage", Mathf.RoundToInt(this.barricadeDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionStructureDamage", Mathf.RoundToInt(this.structureDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionVehicleDamage", Mathf.RoundToInt(this.vehicleDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionResourceDamage", Mathf.RoundToInt(this.resourceDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionObjectDamage", Mathf.RoundToInt(this.objectDamage)), sortOrder);
			}
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x00053684 File Offset: 0x00051884
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._magazine = base.loadRequiredAsset<GameObject>(bundle, "Magazine");
			this._pellets = data.ParseUInt8("Pellets", 0);
			if (this.pellets < 1)
			{
				this._pellets = 1;
			}
			this._stuck = data.ParseUInt8("Stuck", 0);
			this.projectileDamageMultiplier = data.ParseFloat("Projectile_Damage_Multiplier", 1f);
			this.projectileBlastRadiusMultiplier = data.ParseFloat("Projectile_Blast_Radius_Multiplier", 1f);
			this.projectileLaunchForceMultiplier = data.ParseFloat("Projectile_Launch_Force_Multiplier", 1f);
			this._range = data.ParseFloat("Range", 0f);
			this.playerDamage = data.ParseFloat("Player_Damage", 0f);
			this.zombieDamage = data.ParseFloat("Zombie_Damage", 0f);
			this.animalDamage = data.ParseFloat("Animal_Damage", 0f);
			this.barricadeDamage = data.ParseFloat("Barricade_Damage", 0f);
			this.structureDamage = data.ParseFloat("Structure_Damage", 0f);
			this.vehicleDamage = data.ParseFloat("Vehicle_Damage", 0f);
			this.resourceDamage = data.ParseFloat("Resource_Damage", 0f);
			this.explosionLaunchSpeed = data.ParseFloat("Explosion_Launch_Speed", this.playerDamage * 0.1f);
			this._explosion = data.ParseGuidOrLegacyId("Explosion", out this.explosionEffectGuid);
			if (data.ContainsKey("Object_Damage"))
			{
				this.objectDamage = data.ParseFloat("Object_Damage", 0f);
			}
			else
			{
				this.objectDamage = this.resourceDamage;
			}
			this._tracer = data.ParseGuidOrLegacyId("Tracer", out this.tracerEffectGuid);
			this._impact = data.ParseGuidOrLegacyId("Impact", out this._impactEffectGuid);
			this._speed = data.ParseFloat("Speed", 0f);
			if (this.speed < 0.01f)
			{
				this._speed = 1f;
			}
			this._isExplosive = data.ContainsKey("Explosive");
			this.spawnExplosionOnDedicatedServer = data.ContainsKey("Spawn_Explosion_On_Dedicated_Server");
			this._deleteEmpty = data.ContainsKey("Delete_Empty");
			this.shouldFillAfterDetach = data.ParseBool("Should_Fill_After_Detach", false);
		}

		// Token: 0x040009BB RID: 2491
		protected GameObject _magazine;

		// Token: 0x040009BC RID: 2492
		private byte _pellets;

		// Token: 0x040009BD RID: 2493
		private byte _stuck;

		// Token: 0x040009BE RID: 2494
		protected float _range;

		// Token: 0x040009C2 RID: 2498
		public float playerDamage;

		// Token: 0x040009C3 RID: 2499
		public float zombieDamage;

		// Token: 0x040009C4 RID: 2500
		public float animalDamage;

		// Token: 0x040009C5 RID: 2501
		public float barricadeDamage;

		// Token: 0x040009C6 RID: 2502
		public float structureDamage;

		// Token: 0x040009C7 RID: 2503
		public float vehicleDamage;

		// Token: 0x040009C8 RID: 2504
		public float resourceDamage;

		// Token: 0x040009C9 RID: 2505
		public float objectDamage;

		// Token: 0x040009CA RID: 2506
		public float explosionLaunchSpeed;

		// Token: 0x040009CB RID: 2507
		public Guid explosionEffectGuid;

		// Token: 0x040009CC RID: 2508
		private ushort _explosion;

		// Token: 0x040009CE RID: 2510
		public Guid tracerEffectGuid;

		// Token: 0x040009CF RID: 2511
		private ushort _tracer;

		// Token: 0x040009D0 RID: 2512
		private Guid _impactEffectGuid;

		// Token: 0x040009D1 RID: 2513
		private ushort _impact;

		// Token: 0x040009D2 RID: 2514
		private float _speed;

		// Token: 0x040009D3 RID: 2515
		protected bool _isExplosive;

		// Token: 0x040009D4 RID: 2516
		private bool _deleteEmpty;
	}
}
