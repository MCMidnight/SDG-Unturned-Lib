using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002F2 RID: 754
	public class ItemMeleeAsset : ItemWeaponAsset
	{
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x0600169D RID: 5789 RVA: 0x00053A5A File Offset: 0x00051C5A
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x00053A62 File Offset: 0x00051C62
		public override byte[] getState(EItemOrigin origin)
		{
			if (this.isLight)
			{
				return new byte[]
				{
					1
				};
			}
			return new byte[0];
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x0600169F RID: 5791 RVA: 0x00053A7D File Offset: 0x00051C7D
		public float strength
		{
			get
			{
				return this._strength;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060016A0 RID: 5792 RVA: 0x00053A85 File Offset: 0x00051C85
		public float weak
		{
			get
			{
				return this._weak;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060016A1 RID: 5793 RVA: 0x00053A8D File Offset: 0x00051C8D
		public float strong
		{
			get
			{
				return this._strong;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060016A2 RID: 5794 RVA: 0x00053A95 File Offset: 0x00051C95
		public byte stamina
		{
			get
			{
				return this._stamina;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060016A3 RID: 5795 RVA: 0x00053A9D File Offset: 0x00051C9D
		public bool isRepair
		{
			get
			{
				return this._isRepair;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060016A4 RID: 5796 RVA: 0x00053AA5 File Offset: 0x00051CA5
		public bool isRepeated
		{
			get
			{
				return this._isRepeated;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060016A5 RID: 5797 RVA: 0x00053AAD File Offset: 0x00051CAD
		public bool isLight
		{
			get
			{
				return this._isLight;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x060016A6 RID: 5798 RVA: 0x00053AB5 File Offset: 0x00051CB5
		// (set) Token: 0x060016A7 RID: 5799 RVA: 0x00053ABD File Offset: 0x00051CBD
		public PlayerSpotLightConfig lightConfig { get; protected set; }

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060016A8 RID: 5800 RVA: 0x00053AC6 File Offset: 0x00051CC6
		public override bool showQuality
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x060016A9 RID: 5801 RVA: 0x00053AC9 File Offset: 0x00051CC9
		public override bool shouldFriendlySentryTargetUser
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x060016AA RID: 5802 RVA: 0x00053ACC File Offset: 0x00051CCC
		// (set) Token: 0x060016AB RID: 5803 RVA: 0x00053AD4 File Offset: 0x00051CD4
		public float alertRadius { get; protected set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x060016AC RID: 5804 RVA: 0x00053ADD File Offset: 0x00051CDD
		protected override bool doesItemTypeHaveSkins
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x00053AE0 File Offset: 0x00051CE0
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (!this._isRepeated)
			{
				if (this.strength != 1f)
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Melee_StrongAttackModifier", PlayerDashboardInventoryUI.FormatStatModifier(this.strength, true, true)), 10000 + base.DescSort_HigherIsBeneficial(this.strength));
				}
				if (this.stamina > 0)
				{
					string arg = PlayerDashboardInventoryUI.FormatStatColor(this.stamina.ToString(), false);
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Melee_StrongAttackStamina", arg), 10000);
				}
			}
			base.BuildNonExplosiveDescription(builder, itemInstance);
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x00053B8C File Offset: 0x00051D8C
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._use = base.LoadRedirectableAsset<AudioClip>(bundle, "Use", data, "AttackAudioClip");
			this._strength = data.ParseFloat("Strength", 0f);
			this._weak = data.ParseFloat("Weak", 0.5f);
			this._strong = data.ParseFloat("Strong", 0.33f);
			this._stamina = data.ParseUInt8("Stamina", 0);
			this._isRepair = data.ContainsKey("Repair");
			this._isRepeated = data.ContainsKey("Repeated");
			this._isLight = data.ContainsKey("Light");
			if (this.isLight)
			{
				this.lightConfig = new PlayerSpotLightConfig(data);
			}
			if (data.ContainsKey("Alert_Radius"))
			{
				this.alertRadius = data.ParseFloat("Alert_Radius", 0f);
			}
			else
			{
				this.alertRadius = 8f;
			}
			this.impactAudio = data.ReadAudioReference("ImpactAudioDef", bundle);
		}

		// Token: 0x040009DC RID: 2524
		protected AudioClip _use;

		// Token: 0x040009DD RID: 2525
		private float _strength;

		// Token: 0x040009DE RID: 2526
		private float _weak;

		// Token: 0x040009DF RID: 2527
		private float _strong;

		// Token: 0x040009E0 RID: 2528
		private byte _stamina;

		// Token: 0x040009E1 RID: 2529
		private bool _isRepair;

		// Token: 0x040009E2 RID: 2530
		private bool _isRepeated;

		// Token: 0x040009E3 RID: 2531
		private bool _isLight;

		// Token: 0x040009E6 RID: 2534
		public AudioReference impactAudio;
	}
}
