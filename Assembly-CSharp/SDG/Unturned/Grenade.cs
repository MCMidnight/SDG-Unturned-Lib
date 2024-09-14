using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000742 RID: 1858
	public class Grenade : MonoBehaviour, IExplodableThrowable
	{
		// Token: 0x06003CEC RID: 15596 RVA: 0x00122118 File Offset: 0x00120318
		public void Explode()
		{
			if (this.shouldDestroySelf)
			{
				Object.Destroy(base.gameObject);
			}
			if (!Provider.isServer)
			{
				return;
			}
			List<EPlayerKill> list;
			DamageTool.explode(new ExplosionParameters(base.transform.position, this.range, EDeathCause.GRENADE, this.killer)
			{
				playerDamage = this.playerDamage,
				zombieDamage = this.zombieDamage,
				animalDamage = this.animalDamage,
				barricadeDamage = this.barricadeDamage,
				structureDamage = this.structureDamage,
				vehicleDamage = this.vehicleDamage,
				resourceDamage = this.resourceDamage,
				objectDamage = this.objectDamage,
				damageOrigin = EDamageOrigin.Grenade_Explosion,
				launchSpeed = this.explosionLaunchSpeed
			}, out list);
			EffectAsset effectAsset = Assets.FindEffectAssetByGuidOrLegacyId(this.explosionEffectGuid, this.explosion);
			if (effectAsset != null)
			{
				EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
				{
					position = base.transform.position,
					relevantDistance = EffectManager.LARGE,
					wasInstigatedByPlayer = true
				});
			}
			Player player = PlayerTool.getPlayer(this.killer);
			if (player != null)
			{
				foreach (EPlayerKill kill in list)
				{
					player.sendStat(kill);
				}
			}
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x0012228C File Offset: 0x0012048C
		private void Start()
		{
			if (this.fuseLength >= 0f)
			{
				base.Invoke("Explode", this.fuseLength);
			}
		}

		// Token: 0x04002631 RID: 9777
		public CSteamID killer;

		// Token: 0x04002632 RID: 9778
		public float range;

		// Token: 0x04002633 RID: 9779
		public float playerDamage;

		// Token: 0x04002634 RID: 9780
		public float zombieDamage;

		// Token: 0x04002635 RID: 9781
		public float animalDamage;

		// Token: 0x04002636 RID: 9782
		public float barricadeDamage;

		// Token: 0x04002637 RID: 9783
		public float structureDamage;

		// Token: 0x04002638 RID: 9784
		public float vehicleDamage;

		// Token: 0x04002639 RID: 9785
		public float resourceDamage;

		// Token: 0x0400263A RID: 9786
		public float objectDamage;

		// Token: 0x0400263B RID: 9787
		public Guid explosionEffectGuid;

		/// <summary>
		/// Kept because lots of modders have been using this script in Unity,
		/// so removing legacy effect id would break their content.
		/// </summary>
		// Token: 0x0400263C RID: 9788
		public ushort explosion;

		// Token: 0x0400263D RID: 9789
		public float fuseLength = 2.5f;

		// Token: 0x0400263E RID: 9790
		public float explosionLaunchSpeed;

		/// <summary>
		/// Hack for modders using grenade component as a way to deal radial damage. Not a good long term solution but
		/// widely requested for the meantime until I get the chance to rewrite some of the health stuff.
		/// </summary>
		// Token: 0x0400263F RID: 9791
		public bool shouldDestroySelf = true;
	}
}
