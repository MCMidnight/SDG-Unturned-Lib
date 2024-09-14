using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Unturned
{
	// Token: 0x020003F7 RID: 1015
	public class DecalRenderer : MonoBehaviour
	{
		// Token: 0x06001E07 RID: 7687 RVA: 0x0006D504 File Offset: 0x0006B704
		private void OnEnable()
		{
			this.cam = base.GetComponent<Camera>();
			if (this.cam != null && this.buffer == null)
			{
				this.buffer = new CommandBuffer();
				this.buffer.name = "Decals";
				this.cam.AddCommandBuffer(CameraEvent.BeforeLighting, this.buffer);
				this.ambientEquatorID = Shader.PropertyToID("_DecalHackAmbientEquator");
				this.ambientSkyID = Shader.PropertyToID("_DecalHackAmbientSky");
				this.ambientGroundID = Shader.PropertyToID("_DecalHackAmbientGround");
			}
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x0006D590 File Offset: 0x0006B790
		public void OnDisable()
		{
			if (this.cam != null && this.buffer != null)
			{
				this.cam.RemoveCommandBuffer(CameraEvent.BeforeLighting, this.buffer);
				this.buffer = null;
			}
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x0006D5C4 File Offset: 0x0006B7C4
		private void OnPreRender()
		{
			if (this.cam == null || this.buffer == null)
			{
				return;
			}
			if (GraphicsSettings.renderMode != ERenderMode.DEFERRED)
			{
				return;
			}
			this.buffer.Clear();
			int nameID = Shader.PropertyToID("_NormalsCopy");
			this.buffer.GetTemporaryRT(nameID, -1, -1);
			this.buffer.Blit(BuiltinRenderTextureType.GBuffer2, nameID);
			this.buffer.SetGlobalVector(this.ambientEquatorID, RenderSettings.ambientEquatorColor.linear);
			this.buffer.SetGlobalVector(this.ambientSkyID, RenderSettings.ambientSkyColor.linear);
			this.buffer.SetGlobalVector(this.ambientGroundID, RenderSettings.ambientGroundColor.linear);
			float num = 128f + GraphicsSettings.normalizedDrawDistance * 128f;
			this.buffer.SetRenderTarget(DecalRenderer.DIFFUSE, BuiltinRenderTextureType.CameraTarget);
			foreach (Decal decal in DecalSystem.decalsDiffuse)
			{
				if (!(decal.material == null))
				{
					float num2 = num * decal.lodBias;
					float num3 = num2 * num2;
					if ((decal.transform.position - this.cam.transform.position).sqrMagnitude <= num3)
					{
						this.buffer.DrawMesh(this.cube, decal.transform.localToWorldMatrix, decal.material);
					}
				}
			}
		}

		// Token: 0x04000E60 RID: 3680
		private static readonly RenderTargetIdentifier[] DIFFUSE = new RenderTargetIdentifier[]
		{
			BuiltinRenderTextureType.GBuffer0,
			BuiltinRenderTextureType.CameraTarget
		};

		// Token: 0x04000E61 RID: 3681
		public Mesh cube;

		// Token: 0x04000E62 RID: 3682
		private Camera cam;

		// Token: 0x04000E63 RID: 3683
		private CommandBuffer buffer;

		// Token: 0x04000E64 RID: 3684
		private int ambientEquatorID;

		// Token: 0x04000E65 RID: 3685
		private int ambientSkyID;

		// Token: 0x04000E66 RID: 3686
		private int ambientGroundID;
	}
}
