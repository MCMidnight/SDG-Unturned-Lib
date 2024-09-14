using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Payload for the DamageTool.damageZombie function.
	/// </summary>
	// Token: 0x020003F4 RID: 1012
	public struct DamageZombieParameters
	{
		// Token: 0x06001DF4 RID: 7668 RVA: 0x0006D2C0 File Offset: 0x0006B4C0
		public DamageZombieParameters(Zombie zombie, Vector3 direction, float damage)
		{
			this.zombie = zombie;
			this.direction = direction;
			this.damage = damage;
			this.respectArmor = false;
			this.allowBackstab = false;
			this.applyGlobalArmorMultiplier = true;
			this.limb = ELimb.SPINE;
			this.times = 1f;
			this.zombieStunOverride = EZombieStunOverride.None;
			this.ragdollEffect = ERagdollEffect.NONE;
			this.AlertPosition = default(Vector3?);
			this.instigator = null;
		}

		/// <summary>
		/// Equivalent to the "armor" parameter of the legacy damage function.
		/// </summary>
		// Token: 0x1700061A RID: 1562
		// (set) Token: 0x06001DF5 RID: 7669 RVA: 0x0006D32E File Offset: 0x0006B52E
		public bool legacyArmor
		{
			set
			{
				this.respectArmor = value;
				this.allowBackstab = value;
			}
		}

		/// <summary>
		/// If not null and damage is applied, <see cref="M:SDG.Unturned.Zombie.alert(SDG.Unturned.Player)" /> is called with this position (startle: true).
		/// </summary>
		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x0006D33E File Offset: 0x0006B53E
		// (set) Token: 0x06001DF7 RID: 7671 RVA: 0x0006D346 File Offset: 0x0006B546
		public Vector3? AlertPosition { readonly get; set; }

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0006D350 File Offset: 0x0006B550
		public static DamageZombieParameters makeInstakill(Zombie zombie)
		{
			return new DamageZombieParameters(zombie, Vector3.up, 65000f)
			{
				applyGlobalArmorMultiplier = false
			};
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x0006D378 File Offset: 0x0006B578
		public static DamageZombieParameters make(Zombie zombie, Vector3 direction, IDamageMultiplier multiplier, ELimb limb)
		{
			float num = multiplier.multiply(limb);
			return new DamageZombieParameters(zombie, direction, num)
			{
				limb = limb
			};
		}

		// Token: 0x04000E4D RID: 3661
		public Zombie zombie;

		// Token: 0x04000E4E RID: 3662
		public Vector3 direction;

		// Token: 0x04000E4F RID: 3663
		public float damage;

		// Token: 0x04000E50 RID: 3664
		public bool respectArmor;

		/// <summary>
		/// Should game mode config damage multiplier be factored in?
		/// </summary>
		// Token: 0x04000E51 RID: 3665
		public bool applyGlobalArmorMultiplier;

		// Token: 0x04000E52 RID: 3666
		public bool allowBackstab;

		// Token: 0x04000E53 RID: 3667
		public ELimb limb;

		// Token: 0x04000E54 RID: 3668
		public float times;

		// Token: 0x04000E55 RID: 3669
		public EZombieStunOverride zombieStunOverride;

		// Token: 0x04000E56 RID: 3670
		public ERagdollEffect ragdollEffect;

		// Token: 0x04000E58 RID: 3672
		public object instigator;
	}
}
