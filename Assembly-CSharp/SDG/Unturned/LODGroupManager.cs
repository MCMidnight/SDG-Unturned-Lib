using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200057C RID: 1404
	internal class LODGroupManager
	{
		// Token: 0x06002CEF RID: 11503 RVA: 0x000C2F23 File Offset: 0x000C1123
		public static LODGroupManager Get()
		{
			return LODGroupManager.instance;
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x000C2F2C File Offset: 0x000C112C
		public void Register(LODGroupAdditionalData component)
		{
			if (component.LODBiasOverride == LODGroupAdditionalData.ELODBiasOverride.None)
			{
				return;
			}
			LODGroup component2 = component.GetComponent<LODGroup>();
			if (component2 == null)
			{
				UnturnedLog.warn("Additional Data without LOD Group: {0}", new object[]
				{
					component.GetSceneHierarchyPath()
				});
				return;
			}
			LODGroupManager.ComponentData componentData = this.components.AddDefaulted<LODGroupManager.ComponentData>();
			componentData.extensionComponent = component;
			componentData.unityComponent = component2;
			componentData.originalLODs = componentData.unityComponent.GetLODs();
			componentData.modifiedLODs = componentData.unityComponent.GetLODs();
			this.UpdateComponent(componentData);
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x000C2FB0 File Offset: 0x000C11B0
		public void Unregister(LODGroupAdditionalData component)
		{
			for (int i = this.components.Count - 1; i >= 0; i--)
			{
				if (this.components[i].extensionComponent == component)
				{
					this.components.RemoveAtFast(i);
					return;
				}
			}
		}

		/// <summary>
		/// Called after lod bias may have changed.
		/// </summary>
		// Token: 0x06002CF2 RID: 11506 RVA: 0x000C2FFC File Offset: 0x000C11FC
		public void SynchronizeLODBias()
		{
			float lodBias = QualitySettings.lodBias;
			if (MathfEx.IsNearlyEqual(this.cachedLODBias, lodBias, 0.01f))
			{
				return;
			}
			this.cachedLODBias = lodBias;
			foreach (LODGroupManager.ComponentData data in this.components)
			{
				this.UpdateComponent(data);
			}
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x000C3070 File Offset: 0x000C1270
		private void UpdateComponent(LODGroupManager.ComponentData data)
		{
			for (int i = 0; i < data.originalLODs.Length; i++)
			{
				ref LOD ptr = ref data.originalLODs[i];
				data.modifiedLODs[i].screenRelativeTransitionHeight = ptr.screenRelativeTransitionHeight * this.cachedLODBias;
			}
			for (int j = 1; j < data.originalLODs.Length; j++)
			{
				ref LOD ptr2 = ref data.modifiedLODs[j - 1];
				ref LOD ptr3 = ref data.modifiedLODs[j];
				ptr3.screenRelativeTransitionHeight = MathfEx.Min(0.999f, ptr3.screenRelativeTransitionHeight, ptr2.screenRelativeTransitionHeight - 0.001f);
			}
			data.unityComponent.SetLODs(data.modifiedLODs);
		}

		// Token: 0x0400184A RID: 6218
		private static LODGroupManager instance = new LODGroupManager();

		// Token: 0x0400184B RID: 6219
		private List<LODGroupManager.ComponentData> components = new List<LODGroupManager.ComponentData>();

		// Token: 0x0400184C RID: 6220
		private float cachedLODBias = 1f;

		// Token: 0x0200097D RID: 2429
		private class ComponentData
		{
			// Token: 0x04003387 RID: 13191
			public LODGroupAdditionalData extensionComponent;

			// Token: 0x04003388 RID: 13192
			public LODGroup unityComponent;

			// Token: 0x04003389 RID: 13193
			public LOD[] originalLODs;

			// Token: 0x0400338A RID: 13194
			public LOD[] modifiedLODs;
		}
	}
}
