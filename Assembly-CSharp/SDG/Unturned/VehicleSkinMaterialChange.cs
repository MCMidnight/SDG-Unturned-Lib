using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000480 RID: 1152
	internal struct VehicleSkinMaterialChange
	{
		// Token: 0x0400117E RID: 4478
		public Renderer renderer;

		// Token: 0x0400117F RID: 4479
		public Material originalMaterial;

		/// <summary>
		/// If true, set sharedMaterial. If false, set material.
		/// </summary>
		// Token: 0x04001180 RID: 4480
		public bool shared;
	}
}
