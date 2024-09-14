using System;

namespace SDG.Unturned
{
	// Token: 0x02000304 RID: 772
	public class ItemTankAsset : ItemBarricadeAsset
	{
		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x0600175A RID: 5978 RVA: 0x00055597 File Offset: 0x00053797
		public ETankSource source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x0600175B RID: 5979 RVA: 0x0005559F File Offset: 0x0005379F
		public ushort resource
		{
			get
			{
				return this._resource;
			}
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x000555A8 File Offset: 0x000537A8
		public override byte[] getState(EItemOrigin origin)
		{
			byte[] array = new byte[2];
			if (origin == EItemOrigin.ADMIN)
			{
				array[0] = this.resourceState[0];
				array[1] = this.resourceState[1];
			}
			return array;
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x000555D8 File Offset: 0x000537D8
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			ETankSource source = this.source;
			if (source != ETankSource.WATER)
			{
				if (source == ETankSource.FUEL)
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_FuelCapacity", this.resource), 2000);
					return;
				}
			}
			else
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_WaterCapacity", this.resource), 2000);
			}
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0005564C File Offset: 0x0005384C
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._source = (ETankSource)Enum.Parse(typeof(ETankSource), data.GetString("Source", null), true);
			this._resource = data.ParseUInt16("Resource", 0);
			this.resourceState = BitConverter.GetBytes(this.resource);
		}

		// Token: 0x04000A54 RID: 2644
		protected ETankSource _source;

		// Token: 0x04000A55 RID: 2645
		protected ushort _resource;

		// Token: 0x04000A56 RID: 2646
		private byte[] resourceState;
	}
}
