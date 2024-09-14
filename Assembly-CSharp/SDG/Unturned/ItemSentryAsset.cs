using System;

namespace SDG.Unturned
{
	// Token: 0x020002FD RID: 765
	public class ItemSentryAsset : ItemStorageAsset
	{
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x000548E3 File Offset: 0x00052AE3
		public ESentryMode sentryMode
		{
			get
			{
				return this._sentryMode;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001708 RID: 5896 RVA: 0x000548EB File Offset: 0x00052AEB
		// (set) Token: 0x06001709 RID: 5897 RVA: 0x000548F3 File Offset: 0x00052AF3
		public bool requiresPower { get; protected set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x0600170A RID: 5898 RVA: 0x000548FC File Offset: 0x00052AFC
		// (set) Token: 0x0600170B RID: 5899 RVA: 0x00054904 File Offset: 0x00052B04
		public bool infiniteAmmo { get; protected set; }

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x0005490D File Offset: 0x00052B0D
		// (set) Token: 0x0600170D RID: 5901 RVA: 0x00054915 File Offset: 0x00052B15
		public bool infiniteQuality { get; protected set; }

		// Token: 0x0600170E RID: 5902 RVA: 0x00054920 File Offset: 0x00052B20
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (data.ContainsKey("Mode"))
			{
				this._sentryMode = (ESentryMode)Enum.Parse(typeof(ESentryMode), data.GetString("Mode", null), true);
			}
			else
			{
				this._sentryMode = ESentryMode.NEUTRAL;
			}
			this.requiresPower = data.ParseBool("Requires_Power", true);
			this.infiniteAmmo = data.ParseBool("Infinite_Ammo", false);
			this.infiniteQuality = data.ParseBool("Infinite_Quality", false);
			this.detectionRadius = data.ParseFloat("Detection_Radius", 48f);
			this.targetLossRadius = data.ParseFloat("Target_Loss_Radius", this.detectionRadius * 1.2f);
			this.targetAcquiredEffect = data.readAssetReference("Target_Acquired_Effect", ItemSentryAsset.defaultTargetAcquiredEffect);
			this.targetLostEffect = data.readAssetReference("Target_Lost_Effect", ItemSentryAsset.defaultTargetLostEffect);
		}

		// Token: 0x04000A18 RID: 2584
		protected ESentryMode _sentryMode;

		/// <summary>
		/// Players/zombies within this range are treated as potential targets while scanning.
		/// </summary>
		// Token: 0x04000A1C RID: 2588
		public float detectionRadius;

		/// <summary>
		/// Will not lose current target within this range. Prevents target from popping in and out of range.
		/// </summary>
		// Token: 0x04000A1D RID: 2589
		public float targetLossRadius;

		// Token: 0x04000A1E RID: 2590
		public AssetReference<EffectAsset> targetAcquiredEffect;

		// Token: 0x04000A1F RID: 2591
		public AssetReference<EffectAsset> targetLostEffect;

		// Token: 0x04000A20 RID: 2592
		private static AssetReference<EffectAsset> defaultTargetAcquiredEffect = new AssetReference<EffectAsset>("ab5f0056b54545c8a051159659da8bea");

		// Token: 0x04000A21 RID: 2593
		private static AssetReference<EffectAsset> defaultTargetLostEffect = new AssetReference<EffectAsset>("288b98b718084699ba3653c592e57803");
	}
}
