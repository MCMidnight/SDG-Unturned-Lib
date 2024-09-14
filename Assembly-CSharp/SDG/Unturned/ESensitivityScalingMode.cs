using System;

namespace SDG.Unturned
{
	// Token: 0x020006C3 RID: 1731
	public enum ESensitivityScalingMode
	{
		/// <summary>
		/// Project current field of view onto screen compared to desired field of view.
		/// </summary>
		// Token: 0x0400224C RID: 8780
		ProjectionRatio,
		/// <summary>
		/// Multiply sensitivity according to scope/optic zoom. For example an 8x zoom has 1/8th sensitivity.
		/// </summary>
		// Token: 0x0400224D RID: 8781
		ZoomFactor,
		/// <summary>
		/// Preserve how sensitivity felt prior to 3.22.8.0 update.
		/// </summary>
		// Token: 0x0400224E RID: 8782
		Legacy,
		/// <summary>
		/// Do not adjust sensitivity while aiming.
		/// </summary>
		// Token: 0x0400224F RID: 8783
		None
	}
}
