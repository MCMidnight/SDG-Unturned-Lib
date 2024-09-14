using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000744 RID: 1860
	public class Rocket : MonoBehaviour
	{
		// Token: 0x06003CF1 RID: 15601 RVA: 0x00122300 File Offset: 0x00120500
		private void OnTriggerEnter(Collider other)
		{
			if (this.isExploded)
			{
				return;
			}
			if (other.isTrigger)
			{
				return;
			}
			if (this.ignoreTransform != null && (other.transform == this.ignoreTransform || other.transform.IsChildOf(this.ignoreTransform)))
			{
				return;
			}
			this.isExploded = true;
			if (Provider.isServer)
			{
				List<EPlayerKill> list;
				DamageTool.explode(new ExplosionParameters(this.lastPos, this.range, EDeathCause.MISSILE, this.killer)
				{
					playerDamage = this.playerDamage,
					zombieDamage = this.zombieDamage,
					animalDamage = this.animalDamage,
					barricadeDamage = this.barricadeDamage,
					structureDamage = this.structureDamage,
					vehicleDamage = this.vehicleDamage,
					resourceDamage = this.resourceDamage,
					objectDamage = this.objectDamage,
					damageOrigin = EDamageOrigin.Rocket_Explosion,
					penetrateBuildables = this.penetrateBuildables,
					ragdollEffect = this.ragdollEffect,
					launchSpeed = this.explosionLaunchSpeed
				}, out list);
				EffectManager.triggerEffect(new TriggerEffectParameters(Assets.FindEffectAssetByGuidOrLegacyId(this.explosionEffectGuid, this.explosion))
				{
					position = this.lastPos,
					relevantDistance = EffectManager.LARGE,
					wasInstigatedByPlayer = true
				});
				Player player = PlayerTool.getPlayer(this.killer);
				if (player != null)
				{
					foreach (EPlayerKill kill in list)
					{
						player.sendStat(kill);
					}
				}
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x001224C4 File Offset: 0x001206C4
		private void FixedUpdate()
		{
			this.lastPos = base.transform.position;
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x001224D7 File Offset: 0x001206D7
		private void Awake()
		{
			this.lastPos = base.transform.position;
		}

		// Token: 0x04002641 RID: 9793
		public CSteamID killer;

		// Token: 0x04002642 RID: 9794
		public float range;

		// Token: 0x04002643 RID: 9795
		public float playerDamage;

		// Token: 0x04002644 RID: 9796
		public float zombieDamage;

		// Token: 0x04002645 RID: 9797
		public float animalDamage;

		// Token: 0x04002646 RID: 9798
		public float barricadeDamage;

		// Token: 0x04002647 RID: 9799
		public float structureDamage;

		// Token: 0x04002648 RID: 9800
		public float vehicleDamage;

		// Token: 0x04002649 RID: 9801
		public float resourceDamage;

		// Token: 0x0400264A RID: 9802
		public float objectDamage;

		// Token: 0x0400264B RID: 9803
		public Guid explosionEffectGuid;

		/// <summary>
		/// Kept because lots of modders have been using this script in Unity,
		/// so removing legacy effect id would break their content.
		/// </summary>
		// Token: 0x0400264C RID: 9804
		public ushort explosion;

		// Token: 0x0400264D RID: 9805
		public bool penetrateBuildables;

		// Token: 0x0400264E RID: 9806
		public Transform ignoreTransform;

		// Token: 0x0400264F RID: 9807
		public ERagdollEffect ragdollEffect;

		// Token: 0x04002650 RID: 9808
		public float explosionLaunchSpeed;

		// Token: 0x04002651 RID: 9809
		private bool isExploded;

		// Token: 0x04002652 RID: 9810
		private Vector3 lastPos;
	}
}
