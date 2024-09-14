using System;

namespace SDG.Unturned
{
	// Token: 0x0200055C RID: 1372
	internal enum EHousingPlacementResult
	{
		// Token: 0x0400176F RID: 5999
		Success,
		// Token: 0x04001770 RID: 6000
		MissingSlot,
		// Token: 0x04001771 RID: 6001
		Obstructed,
		// Token: 0x04001772 RID: 6002
		MissingPillar,
		/// <summary>
		/// Floors must be placed touching the terrain, or a fake-terrain object like a grassy cliff model.
		/// </summary>
		// Token: 0x04001773 RID: 6003
		MissingGround,
		/// <summary>
		/// Pillars can be partly underground or inside a designated allowed underground area. Otherwise,
		/// if the very top of the pillar is underground placement is blocked. (public issue #4250)
		/// </summary>
		// Token: 0x04001774 RID: 6004
		ObstructedByGround
	}
}
