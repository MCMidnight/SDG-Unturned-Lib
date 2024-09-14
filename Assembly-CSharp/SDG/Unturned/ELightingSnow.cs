using System;

namespace SDG.Unturned
{
	// Token: 0x020004E6 RID: 1254
	public enum ELightingSnow
	{
		/// <summary>
		/// Corresponds to not active and not blending with new weather system. 
		/// </summary>
		// Token: 0x04001441 RID: 5185
		NONE,
		/// <summary>
		/// Corresponds to transitioning in with new weather system. 
		/// </summary>
		// Token: 0x04001442 RID: 5186
		PRE_BLIZZARD,
		/// <summary>
		/// Corresponds to active with new weather system. 
		/// </summary>
		// Token: 0x04001443 RID: 5187
		BLIZZARD,
		/// <summary>
		/// Corresponds to transitioning out with new weather system. 
		/// </summary>
		// Token: 0x04001444 RID: 5188
		POST_BLIZZARD
	}
}
