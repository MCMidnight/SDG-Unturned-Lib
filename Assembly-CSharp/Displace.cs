using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
[ExecuteInEditMode]
[RequireComponent(typeof(WaterBase))]
public class Displace : MonoBehaviour
{
	// Token: 0x06000005 RID: 5 RVA: 0x0000207A File Offset: 0x0000027A
	public void Awake()
	{
		if (base.enabled)
		{
			this.OnEnable();
			return;
		}
		this.OnDisable();
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002091 File Offset: 0x00000291
	public void OnEnable()
	{
		Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
		Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000020A7 File Offset: 0x000002A7
	public void OnDisable()
	{
		Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
		Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
	}
}
