using System;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000F1 RID: 241
	public struct FoliagePreviewSample
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x000169C2 File Offset: 0x00014BC2
		public FoliagePreviewSample(Vector3 newPosition, Color newColor)
		{
			this.position = newPosition;
			this.color = newColor;
		}

		// Token: 0x04000234 RID: 564
		public Vector3 position;

		// Token: 0x04000235 RID: 565
		public Color color;
	}
}
