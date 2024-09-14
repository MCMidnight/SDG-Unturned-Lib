using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Special asset type that isn't (shouldn't be) returned by asset Find methods. Instead, if found when resolving
	/// an asset legacy ID or GUID, Find is called again with the target specified by this asset.
	/// </summary>
	// Token: 0x02000360 RID: 864
	public class RedirectorAsset : Asset
	{
		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001A1B RID: 6683 RVA: 0x0005DD0A File Offset: 0x0005BF0A
		public override EAssetType assetCategory
		{
			get
			{
				return this.assetCategoryOverride;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001A1C RID: 6684 RVA: 0x0005DD12 File Offset: 0x0005BF12
		public Guid TargetGuid
		{
			get
			{
				return this._targetGuid;
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0005DD1C File Offset: 0x0005BF1C
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.assetCategoryOverride = data.ParseEnum<EAssetType>("AssetCategory", EAssetType.NONE);
			if (this.id > 0 && this.assetCategoryOverride == EAssetType.NONE)
			{
				Assets.reportError(this, "legacy ID was assigned but AssetCategory was not");
			}
			if (!data.TryParseGuid("TargetAsset", out this._targetGuid))
			{
				Assets.reportError(this, "unable to parse TargetAsset");
			}
		}

		// Token: 0x04000BED RID: 3053
		protected EAssetType assetCategoryOverride;

		// Token: 0x04000BEE RID: 3054
		private Guid _targetGuid;
	}
}
