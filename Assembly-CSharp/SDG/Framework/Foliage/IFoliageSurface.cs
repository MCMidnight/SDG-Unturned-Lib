using System;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x02000104 RID: 260
	public interface IFoliageSurface
	{
		// Token: 0x060006AA RID: 1706
		FoliageBounds getFoliageSurfaceBounds();

		// Token: 0x060006AB RID: 1707
		bool getFoliageSurfaceInfo(Vector3 position, out Vector3 surfacePosition, out Vector3 surfaceNormal);

		// Token: 0x060006AC RID: 1708
		void bakeFoliageSurface(FoliageBakeSettings bakeSettings, FoliageTile foliageTile);
	}
}
