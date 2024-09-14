using System;

namespace SDG.Unturned
{
	// Token: 0x020003F2 RID: 1010
	public class PlayerDamageMultiplier : IDamageMultiplier
	{
		// Token: 0x06001DF0 RID: 7664 RVA: 0x0006D090 File Offset: 0x0006B290
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
			case ELimb.LEFT_BACK:
			case ELimb.RIGHT_BACK:
			case ELimb.LEFT_FRONT:
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

		// Token: 0x06001DF1 RID: 7665 RVA: 0x0006D180 File Offset: 0x0006B380
		public PlayerDamageMultiplier(float newDamage, float newLeg, float newArm, float newSpine, float newSkull)
		{
			this.damage = newDamage;
			this.leg = newLeg;
			this.arm = newArm;
			this.spine = newSpine;
			this.skull = newSkull;
		}

		// Token: 0x04000E43 RID: 3651
		public float damage;

		// Token: 0x04000E44 RID: 3652
		public float leg;

		// Token: 0x04000E45 RID: 3653
		public float arm;

		// Token: 0x04000E46 RID: 3654
		public float spine;

		// Token: 0x04000E47 RID: 3655
		public float skull;
	}
}
