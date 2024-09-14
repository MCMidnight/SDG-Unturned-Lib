using System;

namespace SDG.Unturned
{
	// Token: 0x020006E0 RID: 1760
	public class StructuresConfigData
	{
		// Token: 0x06003AFA RID: 15098 RVA: 0x00113B5C File Offset: 0x00111D5C
		public float getArmorMultiplier(EArmorTier armorTier)
		{
			if (armorTier == EArmorTier.LOW || armorTier != EArmorTier.HIGH)
			{
				return this.Armor_Lowtier_Multiplier;
			}
			return this.Armor_Hightier_Multiplier;
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x00113B74 File Offset: 0x00111D74
		public StructuresConfigData(EGameMode mode)
		{
			this.Decay_Time = 604800U;
			this.Armor_Lowtier_Multiplier = 1f;
			this.Armor_Hightier_Multiplier = 0.5f;
			this.Gun_Lowcal_Damage_Multiplier = 1f;
			this.Gun_Highcal_Damage_Multiplier = 1f;
			this.Melee_Damage_Multiplier = 1f;
			this.Melee_Repair_Multiplier = 1f;
		}

		// Token: 0x0400243B RID: 9275
		public uint Decay_Time;

		// Token: 0x0400243C RID: 9276
		public float Armor_Lowtier_Multiplier;

		// Token: 0x0400243D RID: 9277
		public float Armor_Hightier_Multiplier;

		// Token: 0x0400243E RID: 9278
		public float Gun_Lowcal_Damage_Multiplier;

		// Token: 0x0400243F RID: 9279
		public float Gun_Highcal_Damage_Multiplier;

		// Token: 0x04002440 RID: 9280
		public float Melee_Damage_Multiplier;

		// Token: 0x04002441 RID: 9281
		public float Melee_Repair_Multiplier;
	}
}
