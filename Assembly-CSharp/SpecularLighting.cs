using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
[RequireComponent(typeof(WaterBase))]
[ExecuteInEditMode]
public class SpecularLighting : MonoBehaviour
{
	// Token: 0x06000025 RID: 37 RVA: 0x0000296B File Offset: 0x00000B6B
	public void Start()
	{
		this.waterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002990 File Offset: 0x00000B90
	public void Update()
	{
		if (!this.waterBase)
		{
			this.waterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
		}
		if (this.specularLight && this.waterBase.sharedMaterial)
		{
			this.waterBase.sharedMaterial.SetVector("_WorldLightDir", this.specularLight.transform.forward);
		}
	}

	// Token: 0x04000015 RID: 21
	public Transform specularLight;

	// Token: 0x04000016 RID: 22
	private WaterBase waterBase;
}
