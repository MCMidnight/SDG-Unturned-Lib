using System;

namespace SDG.Unturned
{
	// Token: 0x020004E5 RID: 1253
	public enum ELightingRain
	{
		/// <summary>
		/// Corresponds to not active and not blending with new weather system. 
		/// </summary>
		// Token: 0x0400143C RID: 5180
		NONE,
		/// <summary>
		/// Corresponds to transitioning in with new weather system. 
		/// </summary>
		// Token: 0x0400143D RID: 5181
		PRE_DRIZZLE,
		/// <summary>
		/// Corresponds to active with new weather system. 
		/// </summary>
		// Token: 0x0400143E RID: 5182
		DRIZZLE,
		/// <summary>
		/// Corresponds to transitioning out with new weather system. 
		/// </summary>
		// Token: 0x0400143F RID: 5183
		POST_DRIZZLE
	}
}
