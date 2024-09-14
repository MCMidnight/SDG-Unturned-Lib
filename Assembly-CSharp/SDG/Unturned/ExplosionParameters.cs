using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Payload for the DamageTool.explode function.
	/// Moved into struct because the amount of arguments to that function were insane, but now is not the time to completely refactor damage.
	/// </summary>
	// Token: 0x020003F0 RID: 1008
	public struct ExplosionParameters
	{
		// Token: 0x06001DED RID: 7661 RVA: 0x0006CFC4 File Offset: 0x0006B1C4
		public ExplosionParameters(Vector3 point, float damageRadius, EDeathCause cause, CSteamID killer)
		{
			this.point = point;
			this.damageRadius = damageRadius;
			this.cause = cause;
			this.killer = killer;
			this.damageType = EExplosionDamageType.CONVENTIONAL;
			this.alertRadius = 32f;
			this.playImpactEffect = true;
			this.penetrateBuildables = false;
			this.damageOrigin = EDamageOrigin.Unknown;
			this.ragdollEffect = ERagdollEffect.NONE;
			this.playerDamage = 0f;
			this.zombieDamage = 0f;
			this.animalDamage = 0f;
			this.barricadeDamage = 0f;
			this.structureDamage = 0f;
			this.vehicleDamage = 0f;
			this.resourceDamage = 0f;
			this.objectDamage = 0f;
			this.launchSpeed = 0f;
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x0006D07F File Offset: 0x0006B27F
		public ExplosionParameters(Vector3 point, float damageRadius, EDeathCause cause)
		{
			this = new ExplosionParameters(point, damageRadius, cause, CSteamID.Nil);
		}

		// Token: 0x04000E30 RID: 3632
		public Vector3 point;

		// Token: 0x04000E31 RID: 3633
		public float damageRadius;

		// Token: 0x04000E32 RID: 3634
		public EDeathCause cause;

		// Token: 0x04000E33 RID: 3635
		public CSteamID killer;

		// Token: 0x04000E34 RID: 3636
		public EExplosionDamageType damageType;

		// Token: 0x04000E35 RID: 3637
		public float alertRadius;

		// Token: 0x04000E36 RID: 3638
		public bool playImpactEffect;

		// Token: 0x04000E37 RID: 3639
		public bool penetrateBuildables;

		// Token: 0x04000E38 RID: 3640
		public EDamageOrigin damageOrigin;

		// Token: 0x04000E39 RID: 3641
		public ERagdollEffect ragdollEffect;

		// Token: 0x04000E3A RID: 3642
		public float playerDamage;

		// Token: 0x04000E3B RID: 3643
		public float zombieDamage;

		// Token: 0x04000E3C RID: 3644
		public float animalDamage;

		// Token: 0x04000E3D RID: 3645
		public float barricadeDamage;

		// Token: 0x04000E3E RID: 3646
		public float structureDamage;

		// Token: 0x04000E3F RID: 3647
		public float vehicleDamage;

		// Token: 0x04000E40 RID: 3648
		public float resourceDamage;

		// Token: 0x04000E41 RID: 3649
		public float objectDamage;

		/// <summary>
		/// Speed to launch players away from blast position.
		/// </summary>
		// Token: 0x04000E42 RID: 3650
		public float launchSpeed;
	}
}
