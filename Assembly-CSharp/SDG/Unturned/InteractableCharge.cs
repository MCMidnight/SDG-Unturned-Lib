using System;
using System.Collections.Generic;
using HighlightingSystem;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200044C RID: 1100
	public class InteractableCharge : Interactable
	{
		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x0600210C RID: 8460 RVA: 0x0007F83B File Offset: 0x0007DA3B
		public bool hasOwnership
		{
			get
			{
				return OwnershipTool.checkToggle(this.owner, this.group);
			}
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x0007F850 File Offset: 0x0007DA50
		public override void updateState(Asset asset, byte[] state)
		{
			this.range2 = ((ItemChargeAsset)asset).range2;
			this.playerDamage = ((ItemChargeAsset)asset).playerDamage;
			this.zombieDamage = ((ItemChargeAsset)asset).zombieDamage;
			this.animalDamage = ((ItemChargeAsset)asset).animalDamage;
			this.barricadeDamage = ((ItemChargeAsset)asset).barricadeDamage;
			this.structureDamage = ((ItemChargeAsset)asset).structureDamage;
			this.vehicleDamage = ((ItemChargeAsset)asset).vehicleDamage;
			this.resourceDamage = ((ItemChargeAsset)asset).resourceDamage;
			this.objectDamage = ((ItemChargeAsset)asset).objectDamage;
			this.detonationEffectGuid = ((ItemChargeAsset)asset).DetonationEffectGuid;
			this.explosion2 = ((ItemChargeAsset)asset).explosion2;
			this.explosionLaunchSpeed = ((ItemChargeAsset)asset).explosionLaunchSpeed;
			this.isSelected = false;
			this.isTargeted = false;
			this.unhighlight();
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x0007F93D File Offset: 0x0007DB3D
		public override bool checkInteractable()
		{
			return false;
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x0007F940 File Offset: 0x0007DB40
		public void detonate(CSteamID killer)
		{
			Player player = PlayerTool.getPlayer(killer);
			this.Detonate(player);
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x0007F95C File Offset: 0x0007DB5C
		public void Detonate(Player instigatingPlayer)
		{
			EffectAsset effectAsset = Assets.FindEffectAssetByGuidOrLegacyId(this.detonationEffectGuid, this.explosion2);
			if (effectAsset != null)
			{
				EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
				{
					relevantDistance = EffectManager.LARGE,
					position = base.transform.position
				});
			}
			ExplosionParameters explosionParameters = new ExplosionParameters(base.transform.position, this.range2, EDeathCause.CHARGE);
			explosionParameters.playerDamage = this.playerDamage;
			explosionParameters.zombieDamage = this.zombieDamage;
			explosionParameters.animalDamage = this.animalDamage;
			explosionParameters.barricadeDamage = this.barricadeDamage;
			explosionParameters.structureDamage = this.structureDamage;
			explosionParameters.vehicleDamage = this.vehicleDamage;
			explosionParameters.resourceDamage = this.resourceDamage;
			explosionParameters.objectDamage = this.objectDamage;
			explosionParameters.damageOrigin = EDamageOrigin.Charge_Explosion;
			explosionParameters.launchSpeed = this.explosionLaunchSpeed;
			if (instigatingPlayer != null)
			{
				explosionParameters.killer = instigatingPlayer.channel.owner.playerID.steamID;
				explosionParameters.ragdollEffect = instigatingPlayer.equipment.getUseableRagdollEffect();
			}
			List<EPlayerKill> list;
			DamageTool.explode(explosionParameters, out list);
			if (instigatingPlayer != null)
			{
				foreach (EPlayerKill kill in list)
				{
					instigatingPlayer.sendStat(kill);
				}
			}
			BarricadeManager.damage(base.transform, 5f, 1f, false, explosionParameters.killer, EDamageOrigin.Charge_Self_Destruct);
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002111 RID: 8465 RVA: 0x0007FAEC File Offset: 0x0007DCEC
		// (set) Token: 0x06002112 RID: 8466 RVA: 0x0007FAF4 File Offset: 0x0007DCF4
		public bool isSelected { get; private set; }

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002113 RID: 8467 RVA: 0x0007FAFD File Offset: 0x0007DCFD
		// (set) Token: 0x06002114 RID: 8468 RVA: 0x0007FB05 File Offset: 0x0007DD05
		public bool isTargeted { get; private set; }

		// Token: 0x06002115 RID: 8469 RVA: 0x0007FB0E File Offset: 0x0007DD0E
		public void select()
		{
			if (this.isSelected)
			{
				return;
			}
			this.isSelected = true;
			this.updateHighlight();
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x0007FB26 File Offset: 0x0007DD26
		public void deselect()
		{
			if (!this.isSelected)
			{
				return;
			}
			this.isSelected = false;
			this.updateHighlight();
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x0007FB3E File Offset: 0x0007DD3E
		public void target()
		{
			if (this.isTargeted)
			{
				return;
			}
			this.isTargeted = true;
			this.updateHighlight();
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x0007FB56 File Offset: 0x0007DD56
		public void untarget()
		{
			if (!this.isTargeted)
			{
				return;
			}
			this.isTargeted = false;
			this.updateHighlight();
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x0007FB6E File Offset: 0x0007DD6E
		public void highlight()
		{
			if (this.highlighter != null)
			{
				return;
			}
			this.highlighter = base.gameObject.AddComponent<Highlighter>();
			this.updateHighlight();
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x0007FB96 File Offset: 0x0007DD96
		public void unhighlight()
		{
			if (this.highlighter == null)
			{
				return;
			}
			Object.DestroyImmediate(this.highlighter);
			this.highlighter = null;
			this.isSelected = false;
			this.isTargeted = false;
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x0007FBC8 File Offset: 0x0007DDC8
		private void updateHighlight()
		{
			if (this.highlighter == null)
			{
				return;
			}
			if (this.isSelected)
			{
				if (this.isTargeted)
				{
					this.highlighter.ConstantOn(Color.cyan, 0.25f);
					return;
				}
				this.highlighter.ConstantOn(Color.green, 0.25f);
				return;
			}
			else
			{
				if (this.isTargeted)
				{
					this.highlighter.ConstantOn(Color.yellow, 0.25f);
					return;
				}
				this.highlighter.ConstantOn(Color.red, 0.25f);
				return;
			}
		}

		// Token: 0x04001033 RID: 4147
		public ulong owner;

		// Token: 0x04001034 RID: 4148
		public ulong group;

		// Token: 0x04001035 RID: 4149
		private float range2;

		// Token: 0x04001036 RID: 4150
		private float playerDamage;

		// Token: 0x04001037 RID: 4151
		private float zombieDamage;

		// Token: 0x04001038 RID: 4152
		private float animalDamage;

		// Token: 0x04001039 RID: 4153
		private float barricadeDamage;

		// Token: 0x0400103A RID: 4154
		private float structureDamage;

		// Token: 0x0400103B RID: 4155
		private float vehicleDamage;

		// Token: 0x0400103C RID: 4156
		private float resourceDamage;

		// Token: 0x0400103D RID: 4157
		private float objectDamage;

		// Token: 0x0400103E RID: 4158
		public Guid detonationEffectGuid;

		/// <summary>
		/// Kept because lots of modders have been these scripts in Unity,
		/// so removing legacy effect id would break their content.
		/// Note: unsure about this one because it is private and not serialized.
		/// </summary>
		// Token: 0x0400103F RID: 4159
		private ushort explosion2;

		// Token: 0x04001040 RID: 4160
		private float explosionLaunchSpeed;

		// Token: 0x04001043 RID: 4163
		private Highlighter highlighter;
	}
}
