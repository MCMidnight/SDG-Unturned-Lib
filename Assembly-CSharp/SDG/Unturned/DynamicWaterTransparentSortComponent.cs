using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Registers renderers with DynamicWaterTransparentSort manager.
	/// </summary>
	// Token: 0x020004B8 RID: 1208
	public class DynamicWaterTransparentSortComponent : MonoBehaviour
	{
		// Token: 0x0600253F RID: 9535 RVA: 0x000944AC File Offset: 0x000926AC
		private void Awake()
		{
			if (this.renderers != null && this.renderers.Length != 0)
			{
				this.managedMaterials = new List<DynamicWaterTransparentSortComponent.ManagedMaterial>(this.renderers.Length);
				foreach (Renderer renderer in this.renderers)
				{
					Transform transform = renderer.transform;
					foreach (Material instantiatedMaterial in renderer.materials)
					{
						DynamicWaterTransparentSortComponent.ManagedMaterial managedMaterial = default(DynamicWaterTransparentSortComponent.ManagedMaterial);
						managedMaterial.rendererTransform = transform;
						managedMaterial.instantiatedMaterial = instantiatedMaterial;
						this.managedMaterials.Add(managedMaterial);
					}
				}
			}
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x00094540 File Offset: 0x00092740
		private void OnEnable()
		{
			if (this.managedMaterials != null)
			{
				DynamicWaterTransparentSort dynamicWaterTransparentSort = DynamicWaterTransparentSort.Get();
				for (int i = this.managedMaterials.Count - 1; i >= 0; i--)
				{
					DynamicWaterTransparentSortComponent.ManagedMaterial managedMaterial = this.managedMaterials[i];
					managedMaterial.handle = dynamicWaterTransparentSort.Register(managedMaterial.rendererTransform, managedMaterial.instantiatedMaterial);
					this.managedMaterials[i] = managedMaterial;
				}
			}
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x000945A8 File Offset: 0x000927A8
		private void OnDisable()
		{
			if (this.managedMaterials != null)
			{
				DynamicWaterTransparentSort dynamicWaterTransparentSort = DynamicWaterTransparentSort.Get();
				foreach (DynamicWaterTransparentSortComponent.ManagedMaterial managedMaterial in this.managedMaterials)
				{
					dynamicWaterTransparentSort.Unregister(managedMaterial.handle);
				}
			}
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x00094610 File Offset: 0x00092810
		private void OnDestroy()
		{
			if (this.managedMaterials != null)
			{
				foreach (DynamicWaterTransparentSortComponent.ManagedMaterial managedMaterial in this.managedMaterials)
				{
					Object.Destroy(managedMaterial.instantiatedMaterial);
				}
				this.managedMaterials = null;
			}
		}

		// Token: 0x040012E4 RID: 4836
		public Renderer[] renderers;

		// Token: 0x040012E5 RID: 4837
		private List<DynamicWaterTransparentSortComponent.ManagedMaterial> managedMaterials;

		// Token: 0x0200094A RID: 2378
		private struct ManagedMaterial
		{
			// Token: 0x04003306 RID: 13062
			public Transform rendererTransform;

			// Token: 0x04003307 RID: 13063
			public Material instantiatedMaterial;

			// Token: 0x04003308 RID: 13064
			public object handle;
		}
	}
}
