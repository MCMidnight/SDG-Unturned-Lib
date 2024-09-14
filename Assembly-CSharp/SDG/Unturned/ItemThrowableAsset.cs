using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000305 RID: 773
	public class ItemThrowableAsset : ItemWeaponAsset
	{
		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001760 RID: 5984 RVA: 0x000556B4 File Offset: 0x000538B4
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001761 RID: 5985 RVA: 0x000556BC File Offset: 0x000538BC
		public GameObject throwable
		{
			get
			{
				return this._throwable;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x000556C4 File Offset: 0x000538C4
		public ushort explosion
		{
			get
			{
				return this._explosion;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001763 RID: 5987 RVA: 0x000556CC File Offset: 0x000538CC
		public bool isExplosive
		{
			get
			{
				return this._isExplosive;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x000556D4 File Offset: 0x000538D4
		public bool isFlash
		{
			get
			{
				return this._isFlash;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001765 RID: 5989 RVA: 0x000556DC File Offset: 0x000538DC
		public bool isSticky
		{
			get
			{
				return this._isSticky;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x000556E4 File Offset: 0x000538E4
		public bool explodeOnImpact
		{
			get
			{
				return this._explodeOnImpact;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001767 RID: 5991 RVA: 0x000556EC File Offset: 0x000538EC
		public float fuseLength
		{
			get
			{
				return this._fuseLength;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x000556F4 File Offset: 0x000538F4
		public override bool shouldFriendlySentryTargetUser
		{
			get
			{
				return this.isExplosive || this.isFlash || this.explodeOnImpact;
			}
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x0005570E File Offset: 0x0005390E
		public override bool canBeUsedInSafezone(SafezoneNode safezone, bool byAdmin)
		{
			return !safezone.noWeapons;
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x0005571C File Offset: 0x0005391C
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this._isFlash)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Throwable_Flash"), 2000);
			}
			if (this._isSticky)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Throwable_Sticky"), 2000);
			}
			if (this._explodeOnImpact)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Throwable_ExplodeOnImpact"), 2000);
			}
			if (this._isExplosive || this._isFlash)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Throwable_FuseLength", string.Format("{0:0.0} s", this._fuseLength)), 2000);
			}
			if (this._isExplosive)
			{
				base.BuildExplosiveDescription(builder, itemInstance);
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x000557F4 File Offset: 0x000539F4
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._use = bundle.load<AudioClip>("Use");
			this._throwable = bundle.load<GameObject>("Throwable");
			this._explosion = data.ParseGuidOrLegacyId("Explosion", out this.explosionEffectGuid);
			this._isExplosive = data.ContainsKey("Explosive");
			this._isFlash = data.ContainsKey("Flash");
			this._isSticky = data.ContainsKey("Sticky");
			this._explodeOnImpact = data.ContainsKey("Explode_On_Impact");
			if (data.ContainsKey("Fuse_Length"))
			{
				this._fuseLength = data.ParseFloat("Fuse_Length", 0f);
			}
			else if (this.isExplosive || this.isFlash)
			{
				this._fuseLength = 2.5f;
			}
			else
			{
				this._fuseLength = 180f;
			}
			this.explosionLaunchSpeed = data.ParseFloat("Explosion_Launch_Speed", this.playerDamageMultiplier.damage * 0.1f);
			this.strongThrowForce = data.ParseFloat("Strong_Throw_Force", 1100f);
			this.weakThrowForce = data.ParseFloat("Weak_Throw_Force", 600f);
			this.boostForceMultiplier = data.ParseFloat("Boost_Throw_Force_Multiplier", 1.4f);
		}

		// Token: 0x04000A57 RID: 2647
		protected AudioClip _use;

		// Token: 0x04000A58 RID: 2648
		protected GameObject _throwable;

		// Token: 0x04000A59 RID: 2649
		public Guid explosionEffectGuid;

		// Token: 0x04000A5A RID: 2650
		private ushort _explosion;

		// Token: 0x04000A5B RID: 2651
		private bool _isExplosive;

		// Token: 0x04000A5C RID: 2652
		private bool _isFlash;

		// Token: 0x04000A5D RID: 2653
		private bool _isSticky;

		// Token: 0x04000A5E RID: 2654
		private bool _explodeOnImpact;

		// Token: 0x04000A5F RID: 2655
		private float _fuseLength;

		// Token: 0x04000A60 RID: 2656
		public float strongThrowForce;

		// Token: 0x04000A61 RID: 2657
		public float weakThrowForce;

		// Token: 0x04000A62 RID: 2658
		public float boostForceMultiplier;

		// Token: 0x04000A63 RID: 2659
		public float explosionLaunchSpeed;
	}
}
