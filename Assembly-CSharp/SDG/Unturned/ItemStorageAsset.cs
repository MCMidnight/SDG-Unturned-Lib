using System;

namespace SDG.Unturned
{
	// Token: 0x02000300 RID: 768
	public class ItemStorageAsset : ItemBarricadeAsset
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001725 RID: 5925 RVA: 0x00054CEE File Offset: 0x00052EEE
		public byte storage_x
		{
			get
			{
				return this._storage_x;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001726 RID: 5926 RVA: 0x00054CF6 File Offset: 0x00052EF6
		public byte storage_y
		{
			get
			{
				return this._storage_y;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001727 RID: 5927 RVA: 0x00054CFE File Offset: 0x00052EFE
		public bool isDisplay
		{
			get
			{
				return this._isDisplay;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001728 RID: 5928 RVA: 0x00054D06 File Offset: 0x00052F06
		// (set) Token: 0x06001729 RID: 5929 RVA: 0x00054D0E File Offset: 0x00052F0E
		public bool shouldCloseWhenOutsideRange { get; protected set; }

		// Token: 0x0600172A RID: 5930 RVA: 0x00054D17 File Offset: 0x00052F17
		public override byte[] getState(EItemOrigin origin)
		{
			if (this.isDisplay)
			{
				return new byte[21];
			}
			return new byte[17];
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x00054D30 File Offset: 0x00052F30
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this.storage_x > 0 && this.storage_y > 0)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_StorageDimensions", this.storage_x, this.storage_y), 2000);
			}
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x00054D94 File Offset: 0x00052F94
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._storage_x = data.ParseUInt8("Storage_X", 0);
			if (this.storage_x < 1)
			{
				this._storage_x = 1;
			}
			this._storage_y = data.ParseUInt8("Storage_Y", 0);
			if (this.storage_y < 1)
			{
				this._storage_y = 1;
			}
			this._isDisplay = data.ContainsKey("Display");
			this.shouldCloseWhenOutsideRange = data.ParseBool("Should_Close_When_Outside_Range", false);
		}

		// Token: 0x04000A33 RID: 2611
		protected byte _storage_x;

		// Token: 0x04000A34 RID: 2612
		protected byte _storage_y;

		// Token: 0x04000A35 RID: 2613
		protected bool _isDisplay;
	}
}
