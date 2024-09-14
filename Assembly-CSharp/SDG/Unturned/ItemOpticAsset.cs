using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002F7 RID: 759
	public class ItemOpticAsset : ItemAsset
	{
		/// <summary>
		/// Factor e.g. 2 is a 2x multiplier.
		/// Prior to 2022-04-11 this was the target field of view. (90/fov)
		/// </summary>
		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x060016BF RID: 5823 RVA: 0x00053DE4 File Offset: 0x00051FE4
		// (set) Token: 0x060016C0 RID: 5824 RVA: 0x00053DEC File Offset: 0x00051FEC
		public float zoom { get; private set; }

		// Token: 0x060016C1 RID: 5825 RVA: 0x00053DF8 File Offset: 0x00051FF8
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this.zoom != 1f)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_ZoomFactor", this.zoom), 10000);
			}
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x00053E49 File Offset: 0x00052049
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.zoom = Mathf.Max(1f, data.ParseFloat("Zoom", 0f));
		}
	}
}
