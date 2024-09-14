using System;

namespace SDG.Unturned
{
	// Token: 0x020003EC RID: 1004
	public class AnimalDamageMultiplier : IDamageMultiplier
	{
		// Token: 0x06001DE3 RID: 7651 RVA: 0x0006CD5C File Offset: 0x0006AF5C
		public float multiply(ELimb limb)
		{
			switch (limb)
			{
			case ELimb.LEFT_BACK:
				return this.damage * this.leg;
			case ELimb.RIGHT_BACK:
				return this.damage * this.leg;
			case ELimb.LEFT_FRONT:
				return this.damage * this.leg;
			case ELimb.RIGHT_FRONT:
				return this.damage * this.leg;
			case ELimb.SPINE:
				return this.damage * this.spine;
			case ELimb.SKULL:
				return this.damage * this.skull;
			default:
				return this.damage;
			}
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x0006CDE5 File Offset: 0x0006AFE5
		public AnimalDamageMultiplier(float newDamage, float newLeg, float newSpine, float newSkull)
		{
			this.damage = newDamage;
			this.leg = newLeg;
			this.spine = newSpine;
			this.skull = newSkull;
		}

		// Token: 0x04000E01 RID: 3585
		public static readonly float MULTIPLIER_EASY = 1.25f;

		// Token: 0x04000E02 RID: 3586
		public static readonly float MULTIPLIER_HARD = 0.75f;

		// Token: 0x04000E03 RID: 3587
		public float damage;

		// Token: 0x04000E04 RID: 3588
		public float leg;

		// Token: 0x04000E05 RID: 3589
		public float spine;

		// Token: 0x04000E06 RID: 3590
		public float skull;
	}
}
