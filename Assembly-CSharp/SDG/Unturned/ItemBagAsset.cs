using System;

namespace SDG.Unturned
{
	// Token: 0x020002D3 RID: 723
	public class ItemBagAsset : ItemClothingAsset
	{
		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x0004EF4C File Offset: 0x0004D14C
		public byte width
		{
			get
			{
				return this._width;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x0600152F RID: 5423 RVA: 0x0004EF54 File Offset: 0x0004D154
		public byte height
		{
			get
			{
				return this._height;
			}
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0004EF5C File Offset: 0x0004D15C
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this.width > 0 && this.height > 0)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_StorageDimensions", this.width, this.height), 2000);
			}
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0004EFBD File Offset: 0x0004D1BD
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (!this.isPro)
			{
				this._width = data.ParseUInt8("Width", 0);
				this._height = data.ParseUInt8("Height", 0);
			}
		}

		// Token: 0x040008B8 RID: 2232
		private byte _width;

		// Token: 0x040008B9 RID: 2233
		private byte _height;
	}
}
