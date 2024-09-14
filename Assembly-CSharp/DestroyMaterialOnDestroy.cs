using System;
using UnityEngine;

/// <summary>
/// Hacky workaround to fix item skin material leak. Unfortunately none of the original item skin code destroyed
/// instantiated materials, and did not keep a reference to the instantiated materials, so until that code gets a
/// rewrite this will take care of cleanup.
/// </summary>
// Token: 0x02000002 RID: 2
public class DestroyMaterialOnDestroy : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void OnDestroy()
	{
		Object.Destroy(this.instantiatedMaterial);
	}

	// Token: 0x04000001 RID: 1
	public Material instantiatedMaterial;
}
