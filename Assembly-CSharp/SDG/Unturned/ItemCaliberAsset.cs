using System;

namespace SDG.Unturned
{
	// Token: 0x020002D7 RID: 727
	public class ItemCaliberAsset : ItemAsset
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06001571 RID: 5489 RVA: 0x0004FADA File Offset: 0x0004DCDA
		public ushort[] calibers
		{
			get
			{
				return this._calibers;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x0004FAE2 File Offset: 0x0004DCE2
		public float recoil_x
		{
			get
			{
				return this._recoil_x;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x0004FAEA File Offset: 0x0004DCEA
		public float recoil_y
		{
			get
			{
				return this._recoil_y;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x0004FAF2 File Offset: 0x0004DCF2
		public float spread
		{
			get
			{
				return this._spread;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06001575 RID: 5493 RVA: 0x0004FAFA File Offset: 0x0004DCFA
		public float sway
		{
			get
			{
				return this._sway;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x0004FB02 File Offset: 0x0004DD02
		public float shake
		{
			get
			{
				return this._shake;
			}
		}

		/// <summary>
		/// For backwards compatibility this is *subtracted* from the gun's firerate, so a positive number decreases
		/// the time between shots and a negative number increases the time between shots.
		/// </summary>
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06001577 RID: 5495 RVA: 0x0004FB0A File Offset: 0x0004DD0A
		public int FirerateOffset
		{
			get
			{
				return this._firerateOffset;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x0004FB12 File Offset: 0x0004DD12
		public bool isPaintable
		{
			get
			{
				return this._isPaintable;
			}
		}

		/// <summary>
		/// Multiplier for normal bullet damage.
		/// </summary>
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001579 RID: 5497 RVA: 0x0004FB1A File Offset: 0x0004DD1A
		// (set) Token: 0x0600157A RID: 5498 RVA: 0x0004FB22 File Offset: 0x0004DD22
		public float ballisticDamageMultiplier { get; protected set; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x0004FB2B File Offset: 0x0004DD2B
		public bool ShouldOnlyAffectAimWhileProne
		{
			get
			{
				return this._isBipod;
			}
		}

		/// <summary>
		/// If true, gun can damage entities with Invulnerable tag. Defaults to false.
		/// </summary>
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x0600157C RID: 5500 RVA: 0x0004FB33 File Offset: 0x0004DD33
		// (set) Token: 0x0600157D RID: 5501 RVA: 0x0004FB3B File Offset: 0x0004DD3B
		public bool CanDamageInvulernableEntities { get; protected set; }

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x0600157E RID: 5502 RVA: 0x0004FB44 File Offset: 0x0004DD44
		// (set) Token: 0x0600157F RID: 5503 RVA: 0x0004FB4C File Offset: 0x0004DD4C
		public bool shouldDestroyAttachmentColliders { get; protected set; }

		/// <summary>
		/// Name to use when instantiating attachment prefab.
		/// By default the asset guid is used, but it can be overridden because some
		/// modders rely on the name for Unity's legacy animation component. For example
		/// in Toothy Deerryte's case there were a lot of duplicate animations to work
		/// around the guid naming, simplified by overriding name.
		/// </summary>
		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x0004FB55 File Offset: 0x0004DD55
		// (set) Token: 0x06001581 RID: 5505 RVA: 0x0004FB5D File Offset: 0x0004DD5D
		public string instantiatedAttachmentName { get; protected set; }

		// Token: 0x06001582 RID: 5506 RVA: 0x0004FB68 File Offset: 0x0004DD68
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this._recoil_x != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_RecoilModifier_X", PlayerDashboardInventoryUI.FormatStatModifier(this._recoil_x, false, false)), 10000 + base.DescSort_LowerIsBeneficial(this._recoil_x));
			}
			if (this._recoil_y != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_RecoilModifier_Y", PlayerDashboardInventoryUI.FormatStatModifier(this._recoil_y, false, false)), 10000 + base.DescSort_LowerIsBeneficial(this._recoil_y));
			}
			if (this._spread != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_SpreadModifier", PlayerDashboardInventoryUI.FormatStatModifier(this._spread, false, false)), 10000 + base.DescSort_LowerIsBeneficial(this._spread));
			}
			if (this._sway != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_SwayModifier", PlayerDashboardInventoryUI.FormatStatModifier(this._sway, true, false)), 10000 + base.DescSort_LowerIsBeneficial(this._sway));
			}
			if (this.aimingRecoilMultiplier != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_RecoilModifier_Aiming", PlayerDashboardInventoryUI.FormatStatModifier(this.aimingRecoilMultiplier, false, false)), 10000 + base.DescSort_LowerIsBeneficial(this.aimingRecoilMultiplier));
			}
			if (this.aimDurationMultiplier != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_AimDurationModifier", PlayerDashboardInventoryUI.FormatStatModifier(this.aimDurationMultiplier, false, false)), 10000 + base.DescSort_LowerIsBeneficial(this.aimDurationMultiplier));
			}
			if (this.aimingMovementSpeedMultiplier != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_AimingMovementSpeedModifier", PlayerDashboardInventoryUI.FormatStatModifier(this.aimingMovementSpeedMultiplier, true, true)), 10000 + base.DescSort_HigherIsBeneficial(this.aimingMovementSpeedMultiplier));
			}
			if (this.ballisticDamageMultiplier != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_BulletDamageModifier", PlayerDashboardInventoryUI.FormatStatModifier(this.ballisticDamageMultiplier, true, true)), 10000 + base.DescSort_HigherIsBeneficial(this.ballisticDamageMultiplier));
			}
			if (this.CanDamageInvulernableEntities)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_InvulnerableModifier"), 10000);
			}
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0004FDBC File Offset: 0x0004DFBC
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._calibers = new ushort[(int)data.ParseUInt8("Calibers", 0)];
			byte b = 0;
			while ((int)b < this.calibers.Length)
			{
				this._calibers[(int)b] = data.ParseUInt16("Caliber_" + b.ToString(), 0);
				b += 1;
			}
			this._recoil_x = data.ParseFloat("Recoil_X", 1f);
			this._recoil_y = data.ParseFloat("Recoil_Y", 1f);
			this.aimingRecoilMultiplier = data.ParseFloat("Aiming_Recoil_Multiplier", 1f);
			this.aimDurationMultiplier = data.ParseFloat("Aim_Duration_Multiplier", 1f);
			this._spread = data.ParseFloat("Spread", 1f);
			this._sway = data.ParseFloat("Sway", 1f);
			this._shake = data.ParseFloat("Shake", 1f);
			this._firerateOffset = data.ParseInt32("Firerate", 0);
			float defaultValue = data.ParseFloat("Damage", 1f);
			this.ballisticDamageMultiplier = data.ParseFloat("Ballistic_Damage_Multiplier", defaultValue);
			this.aimingMovementSpeedMultiplier = data.ParseFloat("Aiming_Movement_Speed_Multiplier", 1f);
			this._isPaintable = data.ContainsKey("Paintable");
			this._isBipod = data.ContainsKey("Bipod");
			this.CanDamageInvulernableEntities = data.ParseBool("Invulnerable", false);
			this.shouldDestroyAttachmentColliders = data.ParseBool("Destroy_Attachment_Colliders", true);
			this.instantiatedAttachmentName = data.GetString("Instantiated_Attachment_Name_Override", this.GUID.ToString("N"));
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0004FF69 File Offset: 0x0004E169
		protected override AudioReference GetDefaultInventoryAudio()
		{
			return new AudioReference("core.masterbundle", "Sounds/Inventory/SmallGunAttachment.asset");
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x0004FF7A File Offset: 0x0004E17A
		[Obsolete("Changed type to int")]
		public byte firerate
		{
			get
			{
				return MathfEx.ClampToByte(this._firerateOffset);
			}
		}

		// Token: 0x040008E3 RID: 2275
		private ushort[] _calibers;

		// Token: 0x040008E4 RID: 2276
		private float _recoil_x;

		// Token: 0x040008E5 RID: 2277
		private float _recoil_y;

		/// <summary>
		/// Recoil magnitude multiplier while the gun is aiming down sights.
		/// </summary>
		// Token: 0x040008E6 RID: 2278
		public float aimingRecoilMultiplier;

		/// <summary>
		/// Multiplier for gun's Aim_In_Duration.
		/// </summary>
		// Token: 0x040008E7 RID: 2279
		public float aimDurationMultiplier;

		// Token: 0x040008E8 RID: 2280
		private float _spread;

		// Token: 0x040008E9 RID: 2281
		private float _sway;

		// Token: 0x040008EA RID: 2282
		private float _shake;

		// Token: 0x040008EB RID: 2283
		private int _firerateOffset;

		// Token: 0x040008EC RID: 2284
		protected bool _isPaintable;

		/// <summary>
		/// Movement speed multiplier while the gun is aiming down sights.
		/// </summary>
		// Token: 0x040008EE RID: 2286
		public float aimingMovementSpeedMultiplier;

		// Token: 0x040008EF RID: 2287
		protected bool _isBipod;
	}
}
