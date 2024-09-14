using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200047C RID: 1148
	public class PropellerModel
	{
		// Token: 0x04001169 RID: 4457
		public Transform transform;

		/// <summary>
		/// Material on Model_0, the low-speed actual blade.
		/// </summary>
		// Token: 0x0400116A RID: 4458
		public Material bladeMaterial;

		/// <summary>
		/// Material on Model_1, the high-speed blurred outline.
		/// </summary>
		// Token: 0x0400116B RID: 4459
		public Material motionBlurMaterial;

		/// <summary>
		/// transform's localRotation when the vehicle was instantiated.
		/// </summary>
		// Token: 0x0400116C RID: 4460
		public Quaternion baseLocationRotation;
	}
}
