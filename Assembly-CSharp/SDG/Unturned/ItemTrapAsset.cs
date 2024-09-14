using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000309 RID: 777
	public class ItemTrapAsset : ItemBarricadeAsset
	{
		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001777 RID: 6007 RVA: 0x00055A0A File Offset: 0x00053C0A
		public float range2
		{
			get
			{
				return this._range2;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001778 RID: 6008 RVA: 0x00055A12 File Offset: 0x00053C12
		public ushort explosion2
		{
			get
			{
				return this._explosion2;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x00055A1A File Offset: 0x00053C1A
		public bool isBroken
		{
			get
			{
				return this._isBroken;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x0600177A RID: 6010 RVA: 0x00055A22 File Offset: 0x00053C22
		public bool isExplosive
		{
			get
			{
				return this._isExplosive;
			}
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x00055A2C File Offset: 0x00053C2C
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this.isExplosive)
			{
				int sortOrder = 30000;
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionBlastRadius", MeasurementTool.FormatLengthString(this.range2)), sortOrder++);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionPlayerDamage", Mathf.RoundToInt(this.playerDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionZombieDamage", Mathf.RoundToInt(this.zombieDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionAnimalDamage", Mathf.RoundToInt(this.animalDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionBarricadeDamage", Mathf.RoundToInt(this.barricadeDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionStructureDamage", Mathf.RoundToInt(this.structureDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionVehicleDamage", Mathf.RoundToInt(this.vehicleDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionResourceDamage", Mathf.RoundToInt(this.resourceDamage)), sortOrder);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionObjectDamage", Mathf.RoundToInt(this.objectDamage)), sortOrder);
				return;
			}
			if (this.isBroken)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Trap_BreaksBones"), 10001);
			}
			if (this.damageTires)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Trap_DamagesTires"), 10001);
			}
			if (this.playerDamage > 0f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Trap_PlayerDamage", Mathf.RoundToInt(this.playerDamage)), 10002);
			}
			if (this.zombieDamage > 0f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Trap_ZombieDamage", Mathf.RoundToInt(this.zombieDamage)), 10002);
			}
			if (this.animalDamage > 0f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Trap_AnimalDamage", Mathf.RoundToInt(this.animalDamage)), 10002);
			}
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x00055CA8 File Offset: 0x00053EA8
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._range2 = data.ParseFloat("Range2", 0f);
			this.playerDamage = data.ParseFloat("Player_Damage", 0f);
			this.zombieDamage = data.ParseFloat("Zombie_Damage", 0f);
			this.animalDamage = data.ParseFloat("Animal_Damage", 0f);
			this.barricadeDamage = data.ParseFloat("Barricade_Damage", 0f);
			this.structureDamage = data.ParseFloat("Structure_Damage", 0f);
			this.vehicleDamage = data.ParseFloat("Vehicle_Damage", 0f);
			this.resourceDamage = data.ParseFloat("Resource_Damage", 0f);
			if (data.ContainsKey("Object_Damage"))
			{
				this.objectDamage = data.ParseFloat("Object_Damage", 0f);
			}
			else
			{
				this.objectDamage = this.resourceDamage;
			}
			this.trapSetupDelay = data.ParseFloat("Trap_Setup_Delay", 0.25f);
			this.trapCooldown = data.ParseFloat("Trap_Cooldown", 0f);
			this._explosion2 = data.ParseGuidOrLegacyId("Explosion2", out this.trapDetonationEffectGuid);
			this.explosionLaunchSpeed = data.ParseFloat("Explosion_Launch_Speed", this.playerDamage * 0.1f);
			this._isBroken = data.ContainsKey("Broken");
			this._isExplosive = data.ContainsKey("Explosive");
			this.damageTires = data.ContainsKey("Damage_Tires");
		}

		// Token: 0x04000A69 RID: 2665
		protected float _range2;

		// Token: 0x04000A6A RID: 2666
		public float playerDamage;

		// Token: 0x04000A6B RID: 2667
		public float zombieDamage;

		// Token: 0x04000A6C RID: 2668
		public float animalDamage;

		// Token: 0x04000A6D RID: 2669
		public float barricadeDamage;

		// Token: 0x04000A6E RID: 2670
		public float structureDamage;

		// Token: 0x04000A6F RID: 2671
		public float vehicleDamage;

		// Token: 0x04000A70 RID: 2672
		public float resourceDamage;

		// Token: 0x04000A71 RID: 2673
		public float objectDamage;

		/// <summary>
		/// Seconds after placement before damage can be dealt.
		/// </summary>
		// Token: 0x04000A72 RID: 2674
		public float trapSetupDelay;

		/// <summary>
		/// Seconds interval between damage dealt.
		/// i.e., will not cause damage if less than this amount of time passed since the last damage.
		/// </summary>
		// Token: 0x04000A73 RID: 2675
		public float trapCooldown;

		// Token: 0x04000A74 RID: 2676
		public float explosionLaunchSpeed;

		// Token: 0x04000A75 RID: 2677
		public Guid trapDetonationEffectGuid;

		// Token: 0x04000A76 RID: 2678
		private ushort _explosion2;

		// Token: 0x04000A77 RID: 2679
		protected bool _isBroken;

		// Token: 0x04000A78 RID: 2680
		protected bool _isExplosive;

		// Token: 0x04000A79 RID: 2681
		public bool damageTires;
	}
}
