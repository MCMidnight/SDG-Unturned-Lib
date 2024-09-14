using System;

namespace SDG.Unturned
{
	// Token: 0x020002F4 RID: 756
	public enum EBoxProbabilityModel
	{
		/// <summary>
		/// Each quality tier has different rarities.
		/// Legendary: 5% Epic: 20% Rare: 75%
		/// </summary>
		// Token: 0x040009EB RID: 2539
		Original,
		/// <summary>
		/// Each item has an equal chance regardless of quality.
		/// </summary>
		// Token: 0x040009EC RID: 2540
		Equalized
	}
}
