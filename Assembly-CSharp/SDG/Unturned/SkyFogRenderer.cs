using System;
using System.Collections.Generic;
using SDG.Framework.Water;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SDG.Unturned
{
	// Token: 0x02000156 RID: 342
	public sealed class SkyFogRenderer : PostProcessEffectRenderer<SkyFog>
	{
		// Token: 0x06000891 RID: 2193 RVA: 0x0001E028 File Offset: 0x0001C228
		public override void Init()
		{
			base.Init();
			this.shader = Shader.Find("Hidden/Custom/SkyFog");
			this.fogColorId = Shader.PropertyToID("_FogColor");
			this.skyColorId = Shader.PropertyToID("_SkyColor");
			this.equatorColorId = Shader.PropertyToID("_EquatorColor");
			this.groundColorId = Shader.PropertyToID("_GroundColor");
			this.inverseProjectionMatrixId = Shader.PropertyToID("_InverseProjectionMatrix");
			this.cameraToWorldMatrixId = Shader.PropertyToID("_CameraToWorld");
			this.waterColorId = Shader.PropertyToID("_WaterColor");
			this.isCameraUnderwaterId = Shader.PropertyToID("_IsCameraUnderwater");
			this.waterCountId = Shader.PropertyToID("_WaterCount");
			this.waterMatricesId = Shader.PropertyToID("_WaterMatrices");
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0001E0EC File Offset: 0x0001C2EC
		public override void Render(PostProcessRenderContext context)
		{
			PropertySheet propertySheet = context.propertySheets.Get(this.shader);
			propertySheet.properties.SetColor(this.fogColorId, RenderSettings.fogColor);
			propertySheet.properties.SetColor(this.skyColorId, RenderSettings.skybox.GetColor(this.skyColorId));
			propertySheet.properties.SetColor(this.equatorColorId, RenderSettings.skybox.GetColor(this.equatorColorId));
			propertySheet.properties.SetColor(this.groundColorId, RenderSettings.skybox.GetColor(this.groundColorId));
			propertySheet.properties.SetMatrix(this.inverseProjectionMatrixId, context.camera.projectionMatrix.inverse);
			propertySheet.properties.SetMatrix(this.cameraToWorldMatrixId, context.camera.cameraToWorldMatrix);
			List<WaterVolume> list = VolumeManager<WaterVolume, WaterVolumeManager>.Get().InternalGetAllVolumes();
			int num = LevelLighting.enableUnderwaterEffects ? Mathf.Min(list.Count, 16) : 0;
			bool flag = LevelLighting.isSea && num > 0;
			propertySheet.properties.SetColor(this.waterColorId, LevelLighting.getSeaColor("_BaseColor"));
			propertySheet.properties.SetFloat(this.isCameraUnderwaterId, flag ? 1f : 0f);
			propertySheet.properties.SetInt(this.waterCountId, num);
			for (int i = 0; i < num; i++)
			{
				SkyFogRenderer.waterMatrices[i] = list[i].transform.worldToLocalMatrix;
			}
			propertySheet.properties.SetMatrixArray(this.waterMatricesId, SkyFogRenderer.waterMatrices);
			RuntimeUtilities.BlitFullscreenTriangle(context.command, context.source, context.destination, propertySheet, 0, false, default(Rect?), false);
		}

		// Token: 0x0400033E RID: 830
		private Shader shader;

		// Token: 0x0400033F RID: 831
		private int fogColorId;

		// Token: 0x04000340 RID: 832
		private int skyColorId;

		// Token: 0x04000341 RID: 833
		private int equatorColorId;

		// Token: 0x04000342 RID: 834
		private int groundColorId;

		// Token: 0x04000343 RID: 835
		private int inverseProjectionMatrixId;

		// Token: 0x04000344 RID: 836
		private int cameraToWorldMatrixId;

		// Token: 0x04000345 RID: 837
		private int waterColorId;

		// Token: 0x04000346 RID: 838
		private int isCameraUnderwaterId;

		// Token: 0x04000347 RID: 839
		private int waterCountId;

		// Token: 0x04000348 RID: 840
		private int waterMatricesId;

		// Token: 0x04000349 RID: 841
		private const int MAX_WATER_COUNT = 16;

		// Token: 0x0400034A RID: 842
		private static Matrix4x4[] waterMatrices = new Matrix4x4[16];
	}
}
