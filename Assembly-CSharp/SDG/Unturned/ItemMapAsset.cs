using System;

namespace SDG.Unturned
{
	// Token: 0x020002EF RID: 751
	public class ItemMapAsset : ItemAsset
	{
		/// <summary>
		/// Does having this item show the compass?
		/// </summary>
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x0600168D RID: 5773 RVA: 0x000538E0 File Offset: 0x00051AE0
		// (set) Token: 0x0600168E RID: 5774 RVA: 0x000538E8 File Offset: 0x00051AE8
		public bool enablesCompass { get; protected set; }

		/// <summary>
		/// Does having this item show the chart?
		/// </summary>
		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x0600168F RID: 5775 RVA: 0x000538F1 File Offset: 0x00051AF1
		// (set) Token: 0x06001690 RID: 5776 RVA: 0x000538F9 File Offset: 0x00051AF9
		public bool enablesChart { get; protected set; }

		/// <summary>
		/// Does having this item show the satellite?
		/// </summary>
		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001691 RID: 5777 RVA: 0x00053902 File Offset: 0x00051B02
		// (set) Token: 0x06001692 RID: 5778 RVA: 0x0005390A File Offset: 0x00051B0A
		public bool enablesMap { get; protected set; }

		// Token: 0x06001693 RID: 5779 RVA: 0x00053913 File Offset: 0x00051B13
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.enablesCompass = data.ContainsKey("Enables_Compass");
			this.enablesChart = data.ContainsKey("Enables_Chart");
			this.enablesMap = data.ContainsKey("Enables_Map");
		}
	}
}
