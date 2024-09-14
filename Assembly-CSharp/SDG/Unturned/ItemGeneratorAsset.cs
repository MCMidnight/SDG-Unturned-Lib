using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002E5 RID: 741
	public class ItemGeneratorAsset : ItemBarricadeAsset
	{
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06001605 RID: 5637 RVA: 0x000517D2 File Offset: 0x0004F9D2
		public ushort capacity
		{
			get
			{
				return this._capacity;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x000517DA File Offset: 0x0004F9DA
		public float wirerange
		{
			get
			{
				return this._wirerange;
			}
		}

		/// <summary>
		/// Seconds to wait between burning one unit of fuel.
		/// </summary>
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001607 RID: 5639 RVA: 0x000517E2 File Offset: 0x0004F9E2
		public float burn
		{
			get
			{
				return this._burn;
			}
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x000517EA File Offset: 0x0004F9EA
		public override byte[] getState(EItemOrigin origin)
		{
			return new byte[3];
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x000517F4 File Offset: 0x0004F9F4
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_FuelCapacity", this.capacity), 2000);
			if (this.burn > 0f)
			{
				int num = Mathf.RoundToInt(3600f / this.burn);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_FuelBurnRate", num), 2000);
				float num2 = this.burn * (float)this.capacity / 3600f;
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_FuelMaxRuntime", num2.ToString("0.00")), 2000);
			}
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x000518AC File Offset: 0x0004FAAC
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._capacity = data.ParseUInt16("Capacity", 0);
			this._wirerange = data.ParseFloat("Wirerange", 0f);
			if (this.wirerange > PowerTool.MAX_POWER_RANGE + 0.1f)
			{
				Assets.reportError(this, "Wirerange is further than the max supported power range of " + PowerTool.MAX_POWER_RANGE.ToString());
			}
			this._burn = data.ParseFloat("Burn", 0f);
		}

		// Token: 0x04000943 RID: 2371
		protected ushort _capacity;

		// Token: 0x04000944 RID: 2372
		protected float _wirerange;

		// Token: 0x04000945 RID: 2373
		protected float _burn;
	}
}
