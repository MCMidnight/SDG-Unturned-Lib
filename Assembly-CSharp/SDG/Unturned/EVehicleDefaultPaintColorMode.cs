using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Controls how vehicle's default paint color (if applicable) is chosen.
	/// </summary>
	// Token: 0x02000377 RID: 887
	internal enum EVehicleDefaultPaintColorMode
	{
		/// <summary>
		/// Not configured.
		/// </summary>
		// Token: 0x04000C5C RID: 3164
		None,
		/// <summary>
		/// Pick from the DefaultPaintColors list.
		/// </summary>
		// Token: 0x04000C5D RID: 3165
		List,
		/// <summary>
		/// Pick a random HSV using VehicleRandomPaintColorConfiguration.
		/// </summary>
		// Token: 0x04000C5E RID: 3166
		RandomHueOrGrayscale
	}
}
