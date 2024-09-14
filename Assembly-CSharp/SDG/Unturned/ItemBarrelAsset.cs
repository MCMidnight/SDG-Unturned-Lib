using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002D4 RID: 724
	public class ItemBarrelAsset : ItemCaliberAsset
	{
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x0004EFFC File Offset: 0x0004D1FC
		public AudioClip shoot
		{
			get
			{
				return this._shoot;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x0004F004 File Offset: 0x0004D204
		public GameObject barrel
		{
			get
			{
				return this._barrel;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x0004F00C File Offset: 0x0004D20C
		public bool isBraked
		{
			get
			{
				return this._isBraked;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0004F014 File Offset: 0x0004D214
		public bool isSilenced
		{
			get
			{
				return this._isSilenced;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06001537 RID: 5431 RVA: 0x0004F01C File Offset: 0x0004D21C
		public float volume
		{
			get
			{
				return this._volume;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x0004F024 File Offset: 0x0004D224
		public byte durability
		{
			get
			{
				return this._durability;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06001539 RID: 5433 RVA: 0x0004F02C File Offset: 0x0004D22C
		public override bool showQuality
		{
			get
			{
				return this.durability > 0;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x0004F037 File Offset: 0x0004D237
		public float ballisticDrop
		{
			get
			{
				return this._ballisticDrop;
			}
		}

		/// <summary>
		/// Multiplier for the maximum distance the gunshot can be heard.
		/// </summary>
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x0600153B RID: 5435 RVA: 0x0004F03F File Offset: 0x0004D23F
		// (set) Token: 0x0600153C RID: 5436 RVA: 0x0004F047 File Offset: 0x0004D247
		public float gunshotRolloffDistanceMultiplier { get; protected set; }

		// Token: 0x0600153D RID: 5437 RVA: 0x0004F050 File Offset: 0x0004D250
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this._ballisticDrop != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_BulletGravityModifier", PlayerDashboardInventoryUI.FormatStatModifier(this._ballisticDrop, true, false)), 10000 + base.DescSort_LowerIsBeneficial(this._ballisticDrop));
			}
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0004F0B0 File Offset: 0x0004D2B0
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._shoot = bundle.load<AudioClip>("Shoot");
			this._barrel = base.loadRequiredAsset<GameObject>(bundle, "Barrel");
			this._isBraked = data.ContainsKey("Braked");
			this._isSilenced = data.ContainsKey("Silenced");
			this._volume = data.ParseFloat("Volume", 1f);
			this._durability = data.ParseUInt8("Durability", 0);
			if (data.ContainsKey("Ballistic_Drop"))
			{
				this._ballisticDrop = data.ParseFloat("Ballistic_Drop", 0f);
			}
			else
			{
				this._ballisticDrop = 1f;
			}
			float defaultValue = this.isSilenced ? 0.5f : 1f;
			this.gunshotRolloffDistanceMultiplier = data.ParseFloat("Gunshot_Rolloff_Distance_Multiplier", defaultValue);
		}

		// Token: 0x040008BA RID: 2234
		protected AudioClip _shoot;

		// Token: 0x040008BB RID: 2235
		protected GameObject _barrel;

		// Token: 0x040008BC RID: 2236
		private bool _isBraked;

		// Token: 0x040008BD RID: 2237
		private bool _isSilenced;

		// Token: 0x040008BE RID: 2238
		private float _volume;

		// Token: 0x040008BF RID: 2239
		private byte _durability;

		// Token: 0x040008C0 RID: 2240
		private float _ballisticDrop;
	}
}
