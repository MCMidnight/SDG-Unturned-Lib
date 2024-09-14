using System;

namespace SDG.Unturned
{
	// Token: 0x020003F3 RID: 1011
	public class ZombieDamageMultiplier : IDamageMultiplier
	{
		// Token: 0x06001DF2 RID: 7666 RVA: 0x0006D1B0 File Offset: 0x0006B3B0
		public float multiply(ELimb limb)
		{
			switch (limb)
			{
			case ELimb.LEFT_FOOT:
				return this.damage * this.leg;
			case ELimb.LEFT_LEG:
				return this.damage * this.leg;
			case ELimb.RIGHT_FOOT:
				return this.damage * this.leg;
			case ELimb.RIGHT_LEG:
				return this.damage * this.leg;
			case ELimb.LEFT_HAND:
				return this.damage * this.arm;
			case ELimb.LEFT_ARM:
				return this.damage * this.arm;
			case ELimb.RIGHT_HAND:
				return this.damage * this.arm;
			case ELimb.RIGHT_ARM:
				return this.damage * this.arm;
			case ELimb.SPINE:
				return this.damage * this.spine;
			case ELimb.SKULL:
				return this.damage * this.skull;
			}
			return this.damage;
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x0006D292 File Offset: 0x0006B492
		public ZombieDamageMultiplier(float newDamage, float newLeg, float newArm, float newSpine, float newSkull)
		{
			this.damage = newDamage;
			this.leg = newLeg;
			this.arm = newArm;
			this.spine = newSpine;
			this.skull = newSkull;
		}

		// Token: 0x04000E48 RID: 3656
		public float damage;

		// Token: 0x04000E49 RID: 3657
		public float leg;

		// Token: 0x04000E4A RID: 3658
		public float arm;

		// Token: 0x04000E4B RID: 3659
		public float spine;

		// Token: 0x04000E4C RID: 3660
		public float skull;
	}
}
