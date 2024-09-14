using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Payload for the DamageTool.damageAnimal function.
	/// </summary>
	// Token: 0x020003ED RID: 1005
	public struct DamageAnimalParameters
	{
		// Token: 0x06001DE6 RID: 7654 RVA: 0x0006CE20 File Offset: 0x0006B020
		public DamageAnimalParameters(Animal animal, Vector3 direction, float damage)
		{
			this.animal = animal;
			this.direction = direction;
			this.damage = damage;
			this.applyGlobalArmorMultiplier = true;
			this.limb = ELimb.SPINE;
			this.times = 1f;
			this.ragdollEffect = ERagdollEffect.NONE;
			this.AlertPosition = default(Vector3?);
			this.instigator = null;
		}

		/// <summary>
		/// If not null and damage is applied, <see cref="M:SDG.Unturned.Animal.alertDamagedFromPoint(UnityEngine.Vector3)" /> is called with this position.
		/// </summary>
		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x0006CE79 File Offset: 0x0006B079
		// (set) Token: 0x06001DE8 RID: 7656 RVA: 0x0006CE81 File Offset: 0x0006B081
		public Vector3? AlertPosition { readonly get; set; }

		// Token: 0x06001DE9 RID: 7657 RVA: 0x0006CE8C File Offset: 0x0006B08C
		public static DamageAnimalParameters makeInstakill(Animal animal)
		{
			return new DamageAnimalParameters(animal, Vector3.up, 65000f)
			{
				applyGlobalArmorMultiplier = false
			};
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x0006CEB4 File Offset: 0x0006B0B4
		public static DamageAnimalParameters make(Animal animal, Vector3 direction, IDamageMultiplier multiplier, ELimb limb)
		{
			float num = multiplier.multiply(limb);
			return new DamageAnimalParameters(animal, direction, num)
			{
				limb = limb
			};
		}

		// Token: 0x04000E07 RID: 3591
		public Animal animal;

		// Token: 0x04000E08 RID: 3592
		public Vector3 direction;

		// Token: 0x04000E09 RID: 3593
		public float damage;

		/// <summary>
		/// Should game mode config damage multiplier be factored in?
		/// </summary>
		// Token: 0x04000E0A RID: 3594
		public bool applyGlobalArmorMultiplier;

		// Token: 0x04000E0B RID: 3595
		public ELimb limb;

		// Token: 0x04000E0C RID: 3596
		public float times;

		// Token: 0x04000E0D RID: 3597
		public ERagdollEffect ragdollEffect;

		// Token: 0x04000E0F RID: 3599
		public object instigator;
	}
}
