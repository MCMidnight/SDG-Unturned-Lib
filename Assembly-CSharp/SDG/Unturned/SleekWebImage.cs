using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000733 RID: 1843
	public class SleekWebImage : SleekWrapper
	{
		// Token: 0x06003CAE RID: 15534 RVA: 0x00120FD6 File Offset: 0x0011F1D6
		public void Refresh(string url, bool shouldCache = true)
		{
			Provider.refreshIcon(new Provider.IconQueryParams(url, new Provider.IconQueryCallback(this.OnImageReady), shouldCache));
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06003CAF RID: 15535 RVA: 0x00120FF0 File Offset: 0x0011F1F0
		// (set) Token: 0x06003CB0 RID: 15536 RVA: 0x00120FFD File Offset: 0x0011F1FD
		public SleekColor color
		{
			get
			{
				return this.internalImage.TintColor;
			}
			set
			{
				this.internalImage.TintColor = value;
			}
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x0012100B File Offset: 0x0011F20B
		public override void OnDestroy()
		{
			this.internalImage = null;
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x00121014 File Offset: 0x0011F214
		public void Clear()
		{
			this.internalImage.Texture = null;
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x00121024 File Offset: 0x0011F224
		public SleekWebImage()
		{
			this.internalImage = Glazier.Get().CreateImage();
			this.internalImage.SizeScale_X = 1f;
			this.internalImage.SizeScale_Y = 1f;
			base.AddChild(this.internalImage);
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x00121080 File Offset: 0x0011F280
		private void OnImageReady(Texture2D icon, bool responsibleForDestroy)
		{
			if (this.useImageDimensions && icon != null)
			{
				if (this.maxImageDimensionsWidth > 0.5f && (float)icon.width > this.maxImageDimensionsWidth && icon.height > 0)
				{
					float num = (float)icon.width / (float)icon.height;
					base.SizeOffset_X = this.maxImageDimensionsWidth;
					base.SizeOffset_Y = this.maxImageDimensionsWidth / num;
				}
				else
				{
					base.SizeOffset_X = (float)icon.width;
					base.SizeOffset_Y = (float)icon.height;
				}
			}
			if (this.internalImage != null)
			{
				this.internalImage.SetTextureAndShouldDestroy(icon, responsibleForDestroy);
				return;
			}
			if (responsibleForDestroy)
			{
				Object.Destroy(icon);
			}
		}

		/// <summary>
		/// If true, SizeOffset_X and SizeOffset_Y are used when image is available.
		/// Defaults to false.
		/// </summary>
		// Token: 0x040025FF RID: 9727
		public bool useImageDimensions;

		/// <summary>
		/// If useImageDimensions is on and image width exceeds this value, scale down
		/// respecting aspect ratio.
		/// </summary>
		// Token: 0x04002600 RID: 9728
		public float maxImageDimensionsWidth = -1f;

		// Token: 0x04002601 RID: 9729
		private ISleekImage internalImage;
	}
}
