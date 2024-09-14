using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004AF RID: 1199
	public class CartographyVolume : LevelVolume<CartographyVolume, CartographyVolumeManager>
	{
		// Token: 0x06002516 RID: 9494 RVA: 0x00093C0C File Offset: 0x00091E0C
		public void GetSatelliteCaptureTransform(out Vector3 position, out Quaternion rotation)
		{
			position = base.transform.TransformPoint(new Vector3(0f, 0.5f, 0f));
			rotation = base.transform.rotation * Quaternion.Euler(90f, 0f, 0f);
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x00093C68 File Offset: 0x00091E68
		protected override void Awake()
		{
			this.supportsSphereShape = false;
			base.Awake();
		}
	}
}
