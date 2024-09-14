using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002F0 RID: 752
	public class ItemMaskAsset : ItemGearAsset
	{
		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06001695 RID: 5781 RVA: 0x00053959 File Offset: 0x00051B59
		public GameObject mask
		{
			get
			{
				return this._mask;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001696 RID: 5782 RVA: 0x00053961 File Offset: 0x00051B61
		public bool isEarpiece
		{
			get
			{
				return this._isEarpiece;
			}
		}

		/// <summary>
		/// Multiplier for how quickly deadzones deplete a gasmask's filter quality.
		/// e.g., 2 is faster (2x) and 0.5 is slower.
		/// </summary>
		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001697 RID: 5783 RVA: 0x00053969 File Offset: 0x00051B69
		// (set) Token: 0x06001698 RID: 5784 RVA: 0x00053971 File Offset: 0x00051B71
		public float FilterDegradationRateMultiplier { get; protected set; } = 1f;

		// Token: 0x06001699 RID: 5785 RVA: 0x0005397C File Offset: 0x00051B7C
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this.FilterDegradationRateMultiplier != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_FilterDegradationRateMultiplier", PlayerDashboardInventoryUI.FormatStatModifier(this.FilterDegradationRateMultiplier, true, false)), 10000 + base.DescSort_LowerIsBeneficial(this.FilterDegradationRateMultiplier));
			}
			if (this.isEarpiece)
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_Clothing_Earpiece"), true), 9999);
			}
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00053A05 File Offset: 0x00051C05
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.FilterDegradationRateMultiplier = data.ParseFloat("FilterDegradationRateMultiplier", 1f);
			if (!this.isPro)
			{
				this._isEarpiece = data.ContainsKey("Earpiece");
			}
		}

		// Token: 0x040009D9 RID: 2521
		protected GameObject _mask;

		// Token: 0x040009DA RID: 2522
		private bool _isEarpiece;
	}
}
