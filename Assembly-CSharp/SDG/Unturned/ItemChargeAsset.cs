using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002D8 RID: 728
	public class ItemChargeAsset : ItemBarricadeAsset
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001587 RID: 5511 RVA: 0x0004FF8F File Offset: 0x0004E18F
		public float range2
		{
			get
			{
				return this._range2;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x0004FF97 File Offset: 0x0004E197
		public Guid DetonationEffectGuid
		{
			get
			{
				return this._detonationEffectGuid;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001589 RID: 5513 RVA: 0x0004FF9F File Offset: 0x0004E19F
		public ushort explosion2
		{
			[Obsolete]
			get
			{
				return this._explosion2;
			}
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x0004FFA8 File Offset: 0x0004E1A8
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
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
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0005012C File Offset: 0x0004E32C
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
			this.explosionLaunchSpeed = data.ParseFloat("Explosion_Launch_Speed", this.playerDamage * 0.1f);
			if (data.ContainsKey("Object_Damage"))
			{
				this.objectDamage = data.ParseFloat("Object_Damage", 0f);
			}
			else
			{
				this.objectDamage = this.resourceDamage;
			}
			this._explosion2 = data.ParseGuidOrLegacyId("Explosion2", out this._detonationEffectGuid);
		}

		// Token: 0x040008F3 RID: 2291
		protected float _range2;

		// Token: 0x040008F4 RID: 2292
		public float playerDamage;

		// Token: 0x040008F5 RID: 2293
		public float zombieDamage;

		// Token: 0x040008F6 RID: 2294
		public float animalDamage;

		// Token: 0x040008F7 RID: 2295
		public float barricadeDamage;

		// Token: 0x040008F8 RID: 2296
		public float structureDamage;

		// Token: 0x040008F9 RID: 2297
		public float vehicleDamage;

		// Token: 0x040008FA RID: 2298
		public float resourceDamage;

		// Token: 0x040008FB RID: 2299
		public float objectDamage;

		// Token: 0x040008FC RID: 2300
		public float explosionLaunchSpeed;

		// Token: 0x040008FD RID: 2301
		private Guid _detonationEffectGuid;

		// Token: 0x040008FE RID: 2302
		private ushort _explosion2;
	}
}
