using System;

namespace SDG.Unturned
{
	// Token: 0x020006DF RID: 1759
	public class BarricadesConfigData
	{
		// Token: 0x06003AF8 RID: 15096 RVA: 0x00113AC1 File Offset: 0x00111CC1
		public float getArmorMultiplier(EArmorTier armorTier)
		{
			if (armorTier == EArmorTier.LOW || armorTier != EArmorTier.HIGH)
			{
				return this.Armor_Lowtier_Multiplier;
			}
			return this.Armor_Hightier_Multiplier;
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x00113AD8 File Offset: 0x00111CD8
		public BarricadesConfigData(EGameMode mode)
		{
			this.Decay_Time = 604800U;
			this.Armor_Lowtier_Multiplier = 1f;
			this.Armor_Hightier_Multiplier = 0.5f;
			this.Gun_Lowcal_Damage_Multiplier = 1f;
			this.Gun_Highcal_Damage_Multiplier = 1f;
			this.Melee_Damage_Multiplier = 1f;
			this.Melee_Repair_Multiplier = 1f;
			this.Allow_Item_Placement_On_Vehicle = true;
			this.Allow_Trap_Placement_On_Vehicle = true;
			this.Max_Item_Distance_From_Hull = 64f;
			this.Max_Trap_Distance_From_Hull = 16f;
		}

		// Token: 0x04002430 RID: 9264
		public uint Decay_Time;

		// Token: 0x04002431 RID: 9265
		public float Armor_Lowtier_Multiplier;

		// Token: 0x04002432 RID: 9266
		public float Armor_Hightier_Multiplier;

		// Token: 0x04002433 RID: 9267
		public float Gun_Lowcal_Damage_Multiplier;

		// Token: 0x04002434 RID: 9268
		public float Gun_Highcal_Damage_Multiplier;

		// Token: 0x04002435 RID: 9269
		public float Melee_Damage_Multiplier;

		// Token: 0x04002436 RID: 9270
		public float Melee_Repair_Multiplier;

		/// <summary>
		/// Should players be allowed to build on their vehicles?
		/// </summary>
		// Token: 0x04002437 RID: 9271
		public bool Allow_Item_Placement_On_Vehicle;

		/// <summary>
		/// Should players be allowed to build traps (e.g. barbed wire) on their vehicles?
		/// </summary>
		// Token: 0x04002438 RID: 9272
		public bool Allow_Trap_Placement_On_Vehicle;

		/// <summary>
		/// Furthest away from colliders a player can build an item onto their vehicle.
		/// </summary>
		// Token: 0x04002439 RID: 9273
		public float Max_Item_Distance_From_Hull;

		/// <summary>
		/// Furthest away from colliders a player can build a trap (e.g. barbed wire) onto their vehicle.
		/// </summary>
		// Token: 0x0400243A RID: 9274
		public float Max_Trap_Distance_From_Hull;
	}
}
