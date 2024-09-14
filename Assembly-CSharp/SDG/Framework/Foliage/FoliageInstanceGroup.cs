using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000EE RID: 238
	public struct FoliageInstanceGroup
	{
		// Token: 0x060005D5 RID: 1493 RVA: 0x00016298 File Offset: 0x00014498
		public FoliageInstanceGroup(AssetReference<FoliageInstancedMeshInfoAsset> newAssetReference, Matrix4x4 newMatrix, bool newClearWhenBaked)
		{
			this.assetReference = newAssetReference;
			this.matrix = newMatrix;
			this.clearWhenBaked = newClearWhenBaked;
		}

		// Token: 0x04000226 RID: 550
		public AssetReference<FoliageInstancedMeshInfoAsset> assetReference;

		// Token: 0x04000227 RID: 551
		public Matrix4x4 matrix;

		// Token: 0x04000228 RID: 552
		public bool clearWhenBaked;
	}
}
