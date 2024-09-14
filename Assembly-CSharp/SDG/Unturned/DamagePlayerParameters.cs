using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Payload for the DamageTool.damagePlayer function.
	/// </summary>
	// Token: 0x020003EE RID: 1006
	public struct DamagePlayerParameters
	{
		// Token: 0x06001DEB RID: 7659 RVA: 0x0006CEDC File Offset: 0x0006B0DC
		public DamagePlayerParameters(Player player)
		{
			this.player = player;
			this.cause = EDeathCause.SUICIDE;
			this.limb = ELimb.SPINE;
			this.killer = CSteamID.Nil;
			this.direction = Vector3.up;
			this.damage = 0f;
			this.times = 1f;
			this.respectArmor = false;
			this.applyGlobalArmorMultiplier = true;
			this.trackKill = false;
			this.ragdollEffect = ERagdollEffect.NONE;
			this.bleedingModifier = DamagePlayerParameters.Bleeding.Default;
			this.bonesModifier = DamagePlayerParameters.Bones.None;
			this.foodModifier = 0f;
			this.waterModifier = 0f;
			this.virusModifier = 0f;
			this.hallucinationModifier = 0f;
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x0006CF84 File Offset: 0x0006B184
		public static DamagePlayerParameters make(Player player, EDeathCause cause, Vector3 direction, IDamageMultiplier multiplier, ELimb limb)
		{
			return new DamagePlayerParameters(player)
			{
				cause = cause,
				limb = limb,
				direction = direction,
				damage = multiplier.multiply(limb)
			};
		}

		// Token: 0x04000E10 RID: 3600
		public Player player;

		// Token: 0x04000E11 RID: 3601
		public EDeathCause cause;

		// Token: 0x04000E12 RID: 3602
		public ELimb limb;

		// Token: 0x04000E13 RID: 3603
		public CSteamID killer;

		// Token: 0x04000E14 RID: 3604
		public Vector3 direction;

		// Token: 0x04000E15 RID: 3605
		public float damage;

		// Token: 0x04000E16 RID: 3606
		public float times;

		/// <summary>
		/// Should armor worn on matching limb be factored in?
		/// </summary>
		// Token: 0x04000E17 RID: 3607
		public bool respectArmor;

		/// <summary>
		/// Should game mode config damage multiplier be factored in?
		/// </summary>
		// Token: 0x04000E18 RID: 3608
		public bool applyGlobalArmorMultiplier;

		/// <summary>
		/// If player dies should it count towards quests?
		/// </summary>
		// Token: 0x04000E19 RID: 3609
		public bool trackKill;

		/// <summary>
		/// Effect to apply to ragdoll if dead.
		/// </summary>
		// Token: 0x04000E1A RID: 3610
		public ERagdollEffect ragdollEffect;

		// Token: 0x04000E1B RID: 3611
		public DamagePlayerParameters.Bleeding bleedingModifier;

		// Token: 0x04000E1C RID: 3612
		public DamagePlayerParameters.Bones bonesModifier;

		// Token: 0x04000E1D RID: 3613
		public float foodModifier;

		// Token: 0x04000E1E RID: 3614
		public float waterModifier;

		// Token: 0x04000E1F RID: 3615
		public float virusModifier;

		// Token: 0x04000E20 RID: 3616
		public float hallucinationModifier;

		// Token: 0x02000932 RID: 2354
		public enum Bleeding
		{
			// Token: 0x040032A4 RID: 12964
			Default,
			// Token: 0x040032A5 RID: 12965
			Always,
			// Token: 0x040032A6 RID: 12966
			Never,
			// Token: 0x040032A7 RID: 12967
			Heal
		}

		// Token: 0x02000933 RID: 2355
		public enum Bones
		{
			// Token: 0x040032A9 RID: 12969
			None,
			// Token: 0x040032AA RID: 12970
			Always,
			// Token: 0x040032AB RID: 12971
			Heal
		}
	}
}
