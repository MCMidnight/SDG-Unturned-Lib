using System;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	// Token: 0x0200007D RID: 125
	public interface IShapeVolume
	{
		// Token: 0x06000309 RID: 777
		bool containsPoint(Vector3 point);

		/// <summary>
		/// Not necessarily cheap to calculate - probably best to cache.
		/// </summary>
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600030A RID: 778
		Bounds worldBounds { get; }

		/// <summary>
		/// Internal cubic meter volume.
		/// </summary>
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600030B RID: 779
		float internalVolume { get; }

		/// <summary>
		/// Surface square meters area.
		/// </summary>
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600030C RID: 780
		float surfaceArea { get; }
	}
}
