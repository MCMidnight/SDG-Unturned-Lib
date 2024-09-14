using System;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000A7 RID: 167
	public struct LandscapePreviewSample
	{
		// Token: 0x0600044F RID: 1103 RVA: 0x000118F0 File Offset: 0x0000FAF0
		public LandscapePreviewSample(Vector3 newPosition, float newWeight)
		{
			this.position = newPosition;
			this.weight = newWeight;
		}

		// Token: 0x040001CC RID: 460
		public Vector3 position;

		// Token: 0x040001CD RID: 461
		public float weight;
	}
}
