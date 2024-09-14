using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200030E RID: 782
	public class ItemWeaponAsset : ItemAsset
	{
		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001788 RID: 6024 RVA: 0x00055EBA File Offset: 0x000540BA
		// (set) Token: 0x06001789 RID: 6025 RVA: 0x00055EC2 File Offset: 0x000540C2
		public byte[] bladeIDs { get; protected set; }

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x00055ECB File Offset: 0x000540CB
		// (set) Token: 0x0600178B RID: 6027 RVA: 0x00055ED3 File Offset: 0x000540D3
		public DamagePlayerParameters.Bleeding playerDamageBleeding { get; protected set; }

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x00055EDC File Offset: 0x000540DC
		// (set) Token: 0x0600178D RID: 6029 RVA: 0x00055EE4 File Offset: 0x000540E4
		public DamagePlayerParameters.Bones playerDamageBones { get; protected set; }

		/// <summary>
		/// Added to player's food value.
		/// </summary>
		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x0600178E RID: 6030 RVA: 0x00055EED File Offset: 0x000540ED
		// (set) Token: 0x0600178F RID: 6031 RVA: 0x00055EF5 File Offset: 0x000540F5
		public float playerDamageFood { get; protected set; }

		/// <summary>
		/// Added to player's water value.
		/// </summary>
		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x00055EFE File Offset: 0x000540FE
		// (set) Token: 0x06001791 RID: 6033 RVA: 0x00055F06 File Offset: 0x00054106
		public float playerDamageWater { get; protected set; }

		/// <summary>
		/// Added to player's virus value.
		/// </summary>
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001792 RID: 6034 RVA: 0x00055F0F File Offset: 0x0005410F
		// (set) Token: 0x06001793 RID: 6035 RVA: 0x00055F17 File Offset: 0x00054117
		public float playerDamageVirus { get; protected set; }

		/// <summary>
		/// Added to player's hallucination value.
		/// </summary>
		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001794 RID: 6036 RVA: 0x00055F20 File Offset: 0x00054120
		// (set) Token: 0x06001795 RID: 6037 RVA: 0x00055F28 File Offset: 0x00054128
		public float playerDamageHallucination { get; protected set; }

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001796 RID: 6038 RVA: 0x00055F31 File Offset: 0x00054131
		// (set) Token: 0x06001797 RID: 6039 RVA: 0x00055F39 File Offset: 0x00054139
		public EZombieStunOverride zombieStunOverride { get; protected set; }

		/// <summary>
		/// Get animal or player damage based on game mode config.
		/// </summary>
		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001798 RID: 6040 RVA: 0x00055F44 File Offset: 0x00054144
		public IDamageMultiplier animalOrPlayerDamageMultiplier
		{
			get
			{
				if (!Provider.modeConfigData.Animals.Weapons_Use_Player_Damage)
				{
					return this.animalDamageMultiplier;
				}
				return this.playerDamageMultiplier;
			}
		}

		/// <summary>
		/// Get zombie or player damage based on game mode config.
		/// </summary>
		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001799 RID: 6041 RVA: 0x00055F74 File Offset: 0x00054174
		public IDamageMultiplier zombieOrPlayerDamageMultiplier
		{
			get
			{
				if (!Provider.modeConfigData.Zombies.Weapons_Use_Player_Damage)
				{
					return this.zombieDamageMultiplier;
				}
				return this.playerDamageMultiplier;
			}
		}

		/// <summary>
		/// Should player/animal/zombie surface be nulled on hit?
		/// Requested by spyjack for a chainsaw-style shield that was overboard with the blood.
		/// </summary>
		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x0600179A RID: 6042 RVA: 0x00055FA3 File Offset: 0x000541A3
		// (set) Token: 0x0600179B RID: 6043 RVA: 0x00055FAB File Offset: 0x000541AB
		public bool allowFleshFx { get; protected set; }

		/// <summary>
		/// Should this weapon bypass the DamageTool.allowedToDamagePlayer test?
		/// Used by weapons that heal players in PvE.
		/// </summary>
		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x00055FB4 File Offset: 0x000541B4
		// (set) Token: 0x0600179D RID: 6045 RVA: 0x00055FBC File Offset: 0x000541BC
		public bool bypassAllowedToDamagePlayer { get; protected set; }

		// Token: 0x0600179E RID: 6046 RVA: 0x00055FC8 File Offset: 0x000541C8
		public bool hasBladeID(byte bladeID)
		{
			if (this.bladeIDs != null)
			{
				for (int i = 0; i < this.bladeIDs.Length; i++)
				{
					if (this.bladeIDs[i] == bladeID)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x00056000 File Offset: 0x00054200
		public void initPlayerDamageParameters(ref DamagePlayerParameters parameters)
		{
			parameters.bleedingModifier = this.playerDamageBleeding;
			parameters.bonesModifier = this.playerDamageBones;
			parameters.foodModifier = this.playerDamageFood;
			parameters.waterModifier = this.playerDamageWater;
			parameters.virusModifier = this.playerDamageVirus;
			parameters.hallucinationModifier = this.playerDamageHallucination;
		}

		/// <summary>
		/// Please refer to ItemWeaponAsset.BuildDescription for an explanation of why this is necessary.
		/// </summary>
		// Token: 0x060017A0 RID: 6048 RVA: 0x00056058 File Offset: 0x00054258
		protected void BuildExplosiveDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			int sortOrder = 30000;
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionBlastRadius", MeasurementTool.FormatLengthString(this.range)), sortOrder++);
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionPlayerDamage", Mathf.RoundToInt(this.playerDamageMultiplier.damage)), sortOrder);
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionZombieDamage", Mathf.RoundToInt(this.zombieDamageMultiplier.damage)), sortOrder);
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionAnimalDamage", Mathf.RoundToInt(this.animalDamageMultiplier.damage)), sortOrder);
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionBarricadeDamage", Mathf.RoundToInt(this.barricadeDamage)), sortOrder);
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionStructureDamage", Mathf.RoundToInt(this.structureDamage)), sortOrder);
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionVehicleDamage", Mathf.RoundToInt(this.vehicleDamage)), sortOrder);
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionResourceDamage", Mathf.RoundToInt(this.resourceDamage)), sortOrder);
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ExplosionObjectDamage", Mathf.RoundToInt(this.objectDamage)), sortOrder);
		}

		/// <summary>
		/// Please refer to ItemWeaponAsset.BuildDescription for an explanation of why this is necessary.
		/// </summary>
		// Token: 0x060017A1 RID: 6049 RVA: 0x000561D8 File Offset: 0x000543D8
		protected void BuildNonExplosiveDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			if (this.range > 0f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponRange", MeasurementTool.FormatLengthString(this.range)), 10000);
			}
			int sortOrder = 30000;
			if (this.playerDamageMultiplier.damage > 0f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_Head", Mathf.FloorToInt(this.playerDamageMultiplier.damage * this.playerDamageMultiplier.skull)), sortOrder++);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_Body", Mathf.FloorToInt(this.playerDamageMultiplier.damage * this.playerDamageMultiplier.spine)), sortOrder++);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_Arm", Mathf.FloorToInt(this.playerDamageMultiplier.damage * this.playerDamageMultiplier.arm)), sortOrder++);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_Leg", Mathf.FloorToInt(this.playerDamageMultiplier.damage * this.playerDamageMultiplier.leg)), sortOrder++);
			}
			int num = Mathf.RoundToInt(this.playerDamageFood);
			if (num > 0)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_FoodPositive", num.ToString()), sortOrder);
			}
			else if (num < 0)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_FoodNegative", (-num).ToString()), sortOrder);
			}
			int num2 = Mathf.RoundToInt(this.playerDamageWater);
			if (num2 > 0)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_WaterPositive", num2.ToString()), sortOrder);
			}
			else if (num2 < 0)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_WaterNegative", (-num2).ToString()), sortOrder);
			}
			int num3 = Mathf.RoundToInt(this.playerDamageVirus);
			if (num3 > 0)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_VirusPositive", num3.ToString()), sortOrder);
			}
			else if (num3 < 0)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_VirusNegative", (-num3).ToString()), sortOrder);
			}
			int num4 = Mathf.RoundToInt(this.playerDamageHallucination);
			if (num4 > 0)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_HallucinationPositive", string.Format("{0} s", num4)), sortOrder);
			}
			else if (num4 < 0)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Player_HallucinationNegative", string.Format("{0} s", -num4)), sortOrder);
			}
			DamagePlayerParameters.Bleeding playerDamageBleeding = this.playerDamageBleeding;
			if (playerDamageBleeding != DamagePlayerParameters.Bleeding.Always)
			{
				if (playerDamageBleeding == DamagePlayerParameters.Bleeding.Heal)
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponBleeding_Heal"), sortOrder);
				}
			}
			else
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponBleeding_Always"), sortOrder);
			}
			DamagePlayerParameters.Bones playerDamageBones = this.playerDamageBones;
			if (playerDamageBones != DamagePlayerParameters.Bones.Always)
			{
				if (playerDamageBones == DamagePlayerParameters.Bones.Heal)
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponBones_Heal"), sortOrder);
				}
			}
			else
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponBones_Always"), sortOrder);
			}
			if (this.isInvulnerable)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Invulnerable"), 10000);
			}
			if (this.zombieDamageMultiplier.damage > 0f)
			{
				int num5 = 31000;
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Zombie_Head", Mathf.FloorToInt(this.zombieDamageMultiplier.damage * this.zombieDamageMultiplier.skull)), num5++);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Zombie_Body", Mathf.FloorToInt(this.zombieDamageMultiplier.damage * this.zombieDamageMultiplier.spine)), num5++);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Zombie_Arm", Mathf.FloorToInt(this.zombieDamageMultiplier.damage * this.zombieDamageMultiplier.arm)), num5++);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Zombie_Leg", Mathf.FloorToInt(this.zombieDamageMultiplier.damage * this.zombieDamageMultiplier.leg)), num5++);
			}
			if (this.animalDamageMultiplier.damage > 0f)
			{
				int num6 = 32000;
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Animal_Head", Mathf.FloorToInt(this.animalDamageMultiplier.damage * this.animalDamageMultiplier.skull)), num6++);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Animal_Body", Mathf.FloorToInt(this.animalDamageMultiplier.damage * this.animalDamageMultiplier.spine)), num6++);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Animal_Limb", Mathf.FloorToInt(this.animalDamageMultiplier.damage * this.animalDamageMultiplier.leg)), num6++);
			}
			if (this.barricadeDamage > 0f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Barricade", Mathf.FloorToInt(this.barricadeDamage)), 33000);
			}
			if (this.structureDamage > 0f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Structure", Mathf.FloorToInt(this.structureDamage)), 33000);
			}
			if (this.vehicleDamage > 0f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Vehicle", Mathf.FloorToInt(this.vehicleDamage)), 33000);
			}
			if (this.resourceDamage > 0f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Resource", Mathf.FloorToInt(this.resourceDamage)), 33000);
			}
			if (this.objectDamage > 0f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WeaponDamage_Object", Mathf.FloorToInt(this.objectDamage)), 33000);
			}
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x0005683C File Offset: 0x00054A3C
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			bool shouldRestrictToLegacyContent = builder.shouldRestrictToLegacyContent;
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00056850 File Offset: 0x00054A50
		public ItemWeaponAsset()
		{
			this.playerDamageMultiplier = new PlayerDamageMultiplier(0f, 0f, 0f, 0f, 0f);
			this.zombieDamageMultiplier = new ZombieDamageMultiplier(0f, 0f, 0f, 0f, 0f);
			this.animalDamageMultiplier = new AnimalDamageMultiplier(0f, 0f, 0f, 0f);
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x000568CC File Offset: 0x00054ACC
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			int num = data.ParseInt32("BladeIDs", 0);
			if (num > 0)
			{
				this.bladeIDs = new byte[num];
				for (int i = 0; i < num; i++)
				{
					this.bladeIDs[i] = data.ParseUInt8("BladeID_" + i.ToString(), 0);
				}
			}
			else
			{
				this.bladeIDs = new byte[1];
				this.bladeIDs[0] = data.ParseUInt8("BladeID", 0);
			}
			this.range = data.ParseFloat("Range", 0f);
			this.playerDamageMultiplier = new PlayerDamageMultiplier(data.ParseFloat("Player_Damage", 0f), data.ParseFloat("Player_Leg_Multiplier", 0f), data.ParseFloat("Player_Arm_Multiplier", 0f), data.ParseFloat("Player_Spine_Multiplier", 0f), data.ParseFloat("Player_Skull_Multiplier", 0f));
			this.playerDamageBleeding = data.ParseEnum<DamagePlayerParameters.Bleeding>("Player_Damage_Bleeding", DamagePlayerParameters.Bleeding.Default);
			this.playerDamageBones = data.ParseEnum<DamagePlayerParameters.Bones>("Player_Damage_Bones", DamagePlayerParameters.Bones.None);
			this.playerDamageFood = data.ParseFloat("Player_Damage_Food", 0f);
			this.playerDamageWater = data.ParseFloat("Player_Damage_Water", 0f);
			this.playerDamageVirus = data.ParseFloat("Player_Damage_Virus", 0f);
			this.playerDamageHallucination = data.ParseFloat("Player_Damage_Hallucination", 0f);
			this.zombieDamageMultiplier = new ZombieDamageMultiplier(data.ParseFloat("Zombie_Damage", 0f), data.ParseFloat("Zombie_Leg_Multiplier", 0f), data.ParseFloat("Zombie_Arm_Multiplier", 0f), data.ParseFloat("Zombie_Spine_Multiplier", 0f), data.ParseFloat("Zombie_Skull_Multiplier", 0f));
			this.animalDamageMultiplier = new AnimalDamageMultiplier(data.ParseFloat("Animal_Damage", 0f), data.ParseFloat("Animal_Leg_Multiplier", 0f), data.ParseFloat("Animal_Spine_Multiplier", 0f), data.ParseFloat("Animal_Skull_Multiplier", 0f));
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
			this.durability = data.ParseFloat("Durability", 0f);
			this.wear = data.ParseUInt8("Wear", 0);
			if (this.wear < 1)
			{
				this.wear = 1;
			}
			this.isInvulnerable = data.ContainsKey("Invulnerable");
			if (data.ContainsKey("Allow_Flesh_Fx"))
			{
				this.allowFleshFx = data.ParseBool("Allow_Flesh_Fx", false);
			}
			else
			{
				this.allowFleshFx = true;
			}
			if (data.ContainsKey("Stun_Zombie_Always"))
			{
				this.zombieStunOverride = EZombieStunOverride.Always;
			}
			else if (data.ContainsKey("Stun_Zombie_Never"))
			{
				this.zombieStunOverride = EZombieStunOverride.Never;
			}
			else
			{
				this.zombieStunOverride = EZombieStunOverride.None;
			}
			this.bypassAllowedToDamagePlayer = data.ParseBool("Bypass_Allowed_To_Damage_Player", false);
		}

		// Token: 0x04000A7C RID: 2684
		public float range;

		// Token: 0x04000A7E RID: 2686
		public PlayerDamageMultiplier playerDamageMultiplier;

		// Token: 0x04000A85 RID: 2693
		public ZombieDamageMultiplier zombieDamageMultiplier;

		// Token: 0x04000A87 RID: 2695
		public AnimalDamageMultiplier animalDamageMultiplier;

		// Token: 0x04000A88 RID: 2696
		public float barricadeDamage;

		// Token: 0x04000A89 RID: 2697
		public float structureDamage;

		// Token: 0x04000A8A RID: 2698
		public float vehicleDamage;

		// Token: 0x04000A8B RID: 2699
		public float resourceDamage;

		// Token: 0x04000A8C RID: 2700
		public float objectDamage;

		// Token: 0x04000A8D RID: 2701
		public float durability;

		// Token: 0x04000A8E RID: 2702
		public byte wear;

		// Token: 0x04000A8F RID: 2703
		public bool isInvulnerable;
	}
}
