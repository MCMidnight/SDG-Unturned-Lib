using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
[ExecuteInEditMode]
public class WaterTile : MonoBehaviour
{
	// Token: 0x0600002C RID: 44 RVA: 0x00002B27 File Offset: 0x00000D27
	public void Start()
	{
		this.AcquireComponents();
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002B30 File Offset: 0x00000D30
	private void AcquireComponents()
	{
		if (!this.reflection)
		{
			if (base.transform.parent)
			{
				this.reflection = base.transform.parent.GetComponent<PlanarReflection>();
				return;
			}
			this.reflection = base.transform.GetComponent<PlanarReflection>();
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002B84 File Offset: 0x00000D84
	public void OnWillRenderObject()
	{
		Camera current = Camera.current;
		if (this.reflection)
		{
			this.reflection.WaterTileBeingRendered(base.transform, current);
		}
		if (this.waterBase)
		{
			this.waterBase.WaterTileBeingRendered(base.transform, current);
		}
	}

	// Token: 0x0400001E RID: 30
	public PlanarReflection reflection;

	// Token: 0x0400001F RID: 31
	public WaterBase waterBase;
}
