using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	// Token: 0x0200009B RID: 155
	[Obsolete]
	public interface ILandscaleHoleVolumeInteractionHandler
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060003CD RID: 973
		bool landscapeHoleAutoIgnoreTerrainCollision { get; }

		// Token: 0x060003CE RID: 974
		void landscapeHoleBeginCollision(LandscapeHoleVolume volume, List<TerrainCollider> terrainColliders);

		// Token: 0x060003CF RID: 975
		void landscapeHoleEndCollision(LandscapeHoleVolume volume, List<TerrainCollider> terrainColliders);
	}
}
