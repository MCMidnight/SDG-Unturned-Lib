using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200030A RID: 778
	public class ItemVehiclePaintToolAsset : ItemToolAsset
	{
		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x0600177E RID: 6014 RVA: 0x00055E3A File Offset: 0x0005403A
		// (set) Token: 0x0600177F RID: 6015 RVA: 0x00055E42 File Offset: 0x00054042
		public Color32 PaintColor { get; protected set; }

		// Token: 0x06001780 RID: 6016 RVA: 0x00055E4C File Offset: 0x0005404C
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			Color32 paintColor;
			if (data.TryParseColor32RGB("PaintColor", out paintColor))
			{
				this.PaintColor = paintColor;
				return;
			}
			Assets.reportError(this, "missing PaintColor");
		}
	}
}
