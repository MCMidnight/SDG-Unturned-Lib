using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace SDG.Unturned
{
	// Token: 0x02000709 RID: 1801
	public class SleekCameraImage : SleekWrapper
	{
		// Token: 0x06003BAB RID: 15275 RVA: 0x00117779 File Offset: 0x00115979
		public void SetCamera(Camera camera)
		{
			if (this.targetCamera != null)
			{
				this.DestroyRenderTexture();
			}
			this.targetCamera = camera;
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x00117798 File Offset: 0x00115998
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (this.targetCamera == null)
			{
				return;
			}
			Vector2 absoluteSize = base.GetAbsoluteSize();
			int num = Mathf.CeilToInt(absoluteSize.x);
			int num2 = Mathf.CeilToInt(absoluteSize.y);
			if (num < 1 || num2 < 1)
			{
				return;
			}
			if (this.renderTexture != null && (this.renderTexture.width != num || this.renderTexture.height != num2))
			{
				this.DestroyRenderTexture();
			}
			if (this.renderTexture == null)
			{
				GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB;
				GraphicsFormat depthStencilFormat = GraphicsFormat.D24_UNorm_S8_UInt;
				this.renderTexture = new RenderTexture(num, num2, colorFormat, depthStencilFormat);
				this.renderTexture.hideFlags = HideFlags.HideAndDontSave;
				this.renderTexture.filterMode = FilterMode.Point;
				this.targetCamera.targetTexture = this.renderTexture;
				this.internalImage.Texture = this.renderTexture;
			}
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x0011786D File Offset: 0x00115A6D
		public override void OnDestroy()
		{
			this.DestroyRenderTexture();
			base.OnDestroy();
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x0011787C File Offset: 0x00115A7C
		public SleekCameraImage()
		{
			this.internalImage = Glazier.Get().CreateImage();
			this.internalImage.SizeScale_X = 1f;
			this.internalImage.SizeScale_Y = 1f;
			base.AddChild(this.internalImage);
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x001178CC File Offset: 0x00115ACC
		private void DestroyRenderTexture()
		{
			if (this.targetCamera != null)
			{
				this.targetCamera.targetTexture = null;
			}
			if (this.internalImage != null)
			{
				this.internalImage.Texture = null;
			}
			if (this.renderTexture != null)
			{
				Object.Destroy(this.renderTexture);
				this.renderTexture = null;
			}
		}

		// Token: 0x0400254F RID: 9551
		public ISleekImage internalImage;

		// Token: 0x04002550 RID: 9552
		private RenderTexture renderTexture;

		// Token: 0x04002551 RID: 9553
		private Camera targetCamera;
	}
}
