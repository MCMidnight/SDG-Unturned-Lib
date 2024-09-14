using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
[ExecuteInEditMode]
public class WaterBase : MonoBehaviour
{
	// Token: 0x06000028 RID: 40 RVA: 0x00002A1C File Offset: 0x00000C1C
	public void UpdateShader()
	{
		if (this.waterQuality > WaterQuality.Medium)
		{
			this.sharedMaterial.shader.maximumLOD = 501;
		}
		else if (this.waterQuality > WaterQuality.Low)
		{
			this.sharedMaterial.shader.maximumLOD = 301;
		}
		else
		{
			this.sharedMaterial.shader.maximumLOD = 201;
		}
		if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			this.edgeBlend = false;
		}
		if (this.edgeBlend)
		{
			Shader.EnableKeyword("WATER_EDGEBLEND_ON");
			Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
			if (Camera.main)
			{
				Camera.main.depthTextureMode |= DepthTextureMode.Depth;
				return;
			}
		}
		else
		{
			Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
			Shader.DisableKeyword("WATER_EDGEBLEND_ON");
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002ADC File Offset: 0x00000CDC
	public void WaterTileBeingRendered(Transform tr, Camera currentCam)
	{
		if (currentCam && this.edgeBlend)
		{
			currentCam.depthTextureMode |= DepthTextureMode.Depth;
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002AFC File Offset: 0x00000CFC
	public void Update()
	{
		if (this.sharedMaterial)
		{
			this.UpdateShader();
		}
	}

	// Token: 0x0400001B RID: 27
	public Material sharedMaterial;

	// Token: 0x0400001C RID: 28
	public WaterQuality waterQuality = WaterQuality.High;

	// Token: 0x0400001D RID: 29
	public bool edgeBlend = true;
}
