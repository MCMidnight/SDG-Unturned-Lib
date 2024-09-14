using System;
using System.Collections.Generic;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Manages render queue for transparent materials on non-stationary objects.
	/// Updates one material per frame.
	/// </summary>
	// Token: 0x020004B7 RID: 1207
	public class DynamicWaterTransparentSort : MonoBehaviour
	{
		// Token: 0x06002536 RID: 9526 RVA: 0x00094277 File Offset: 0x00092477
		public static DynamicWaterTransparentSort Get()
		{
			if (DynamicWaterTransparentSort.instance == null)
			{
				GameObject gameObject = new GameObject("DynamicWaterTransparentSort");
				Object.DontDestroyOnLoad(gameObject);
				gameObject.hideFlags = HideFlags.HideAndDontSave;
				DynamicWaterTransparentSort.instance = gameObject.AddComponent<DynamicWaterTransparentSort>();
			}
			return DynamicWaterTransparentSort.instance;
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000942B0 File Offset: 0x000924B0
		public object Register(Transform transform, Material material)
		{
			if (transform == null || material == null)
			{
				return null;
			}
			DynamicWaterTransparentSort.TransparentObject transparentObject = new DynamicWaterTransparentSort.TransparentObject(transform, material);
			transparentObject.UpdatePosition();
			transparentObject.UpdateRenderQueue(LevelLighting.isSea);
			this.managedObjects.Add(transparentObject);
			return transparentObject;
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x000942F8 File Offset: 0x000924F8
		public void Unregister(object handle)
		{
			if (handle == null)
			{
				return;
			}
			for (int i = this.managedObjects.Count - 1; i >= 0; i--)
			{
				if (this.managedObjects[i] == handle)
				{
					this.managedObjects.RemoveAtFast(i);
					return;
				}
			}
		}

		/// <summary>
		/// Callback when camera above/under water changes.
		/// </summary>
		// Token: 0x06002539 RID: 9529 RVA: 0x00094340 File Offset: 0x00092540
		private void HandleIsSeaChanged(bool isSea)
		{
			for (int i = this.managedObjects.Count - 1; i >= 0; i--)
			{
				DynamicWaterTransparentSort.TransparentObject transparentObject = this.managedObjects[i];
				if (transparentObject.IsValid)
				{
					transparentObject.UpdateRenderQueue(isSea);
				}
				else
				{
					this.managedObjects.RemoveAtFast(i);
				}
			}
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x0009438F File Offset: 0x0009258F
		private void Start()
		{
			LevelLighting.isSeaChanged += this.HandleIsSeaChanged;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000943C2 File Offset: 0x000925C2
		private void OnDestroy()
		{
			LevelLighting.isSeaChanged -= this.HandleIsSeaChanged;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Remove(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000943F5 File Offset: 0x000925F5
		private void OnLogMemoryUsage(List<string> results)
		{
			results.Add(string.Format("Water transparent sort managed objects: {0}", this.managedObjects.Count));
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x00094418 File Offset: 0x00092618
		private void Update()
		{
			if (this.managedObjects.Count < 1)
			{
				return;
			}
			this.updateIndex++;
			if (this.updateIndex >= this.managedObjects.Count)
			{
				this.updateIndex = 0;
			}
			DynamicWaterTransparentSort.TransparentObject transparentObject = this.managedObjects[this.updateIndex];
			if (transparentObject.IsValid)
			{
				transparentObject.UpdatePosition();
				transparentObject.UpdateRenderQueue(LevelLighting.isSea);
				return;
			}
			this.managedObjects.RemoveAtFast(this.updateIndex);
		}

		// Token: 0x040012E1 RID: 4833
		private int updateIndex;

		// Token: 0x040012E2 RID: 4834
		private List<DynamicWaterTransparentSort.TransparentObject> managedObjects = new List<DynamicWaterTransparentSort.TransparentObject>();

		// Token: 0x040012E3 RID: 4835
		private static DynamicWaterTransparentSort instance;

		// Token: 0x02000949 RID: 2377
		private class TransparentObject
		{
			// Token: 0x06004ADC RID: 19164 RVA: 0x001B22E7 File Offset: 0x001B04E7
			public TransparentObject(Transform transform, Material material)
			{
				this.transform = transform;
				this.material = material;
			}

			// Token: 0x17000BD2 RID: 3026
			// (get) Token: 0x06004ADD RID: 19165 RVA: 0x001B22FD File Offset: 0x001B04FD
			public bool IsValid
			{
				get
				{
					return this.transform != null && this.material != null;
				}
			}

			// Token: 0x06004ADE RID: 19166 RVA: 0x001B231B File Offset: 0x001B051B
			public void UpdatePosition()
			{
				this.wasTransformUnderwater = WaterUtility.isPointUnderwater(this.transform.position);
			}

			// Token: 0x06004ADF RID: 19167 RVA: 0x001B2334 File Offset: 0x001B0534
			public void UpdateRenderQueue(bool isCameraUnderwater)
			{
				if (this.wasTransformUnderwater)
				{
					if (isCameraUnderwater)
					{
						this.material.renderQueue = 3100;
						return;
					}
					this.material.renderQueue = 2900;
					return;
				}
				else
				{
					if (isCameraUnderwater)
					{
						this.material.renderQueue = 2900;
						return;
					}
					this.material.renderQueue = 3100;
					return;
				}
			}

			// Token: 0x04003303 RID: 13059
			public Transform transform;

			// Token: 0x04003304 RID: 13060
			public Material material;

			// Token: 0x04003305 RID: 13061
			public bool wasTransformUnderwater;
		}
	}
}
