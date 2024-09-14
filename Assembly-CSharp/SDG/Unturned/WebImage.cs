using System;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000739 RID: 1849
	public class WebImage : MonoBehaviour
	{
		// Token: 0x06003CCA RID: 15562 RVA: 0x00121624 File Offset: 0x0011F824
		public void setAddressAndRefresh(string newURL, bool newShouldCache, bool forceRefresh)
		{
			if (forceRefresh)
			{
				Provider.destroyCachedIcon(newURL);
			}
			else if (this.url != null && this.shouldCache && newShouldCache && this.url.Equals(newURL, 3))
			{
				return;
			}
			this.url = newURL;
			this.shouldCache = newShouldCache;
			this.Refresh();
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x00121678 File Offset: 0x0011F878
		private void onImageReady(Texture2D texture, bool responsibleForDestroy)
		{
			this.cleanupResources();
			if (responsibleForDestroy)
			{
				this.texture = texture;
			}
			this.targetImage.enabled = true;
			if (texture != null)
			{
				this.sprite = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0.5f, 0.5f), 100f);
				this.sprite.name = texture.name + "Sprite";
				this.sprite.hideFlags = HideFlags.HideAndDontSave;
				this.targetImage.sprite = this.sprite;
				AspectRatioFitter component = base.GetComponent<AspectRatioFitter>();
				if (component != null)
				{
					component.aspectRatio = (float)texture.width / (float)texture.height;
				}
			}
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x00121748 File Offset: 0x0011F948
		public void Refresh()
		{
			if (this.targetImage == null)
			{
				return;
			}
			this.targetImage.enabled = false;
			if (string.IsNullOrEmpty(this.url))
			{
				return;
			}
			Provider.refreshIcon(new Provider.IconQueryParams(this.url, new Provider.IconQueryCallback(this.onImageReady), this.shouldCache));
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x001217A0 File Offset: 0x0011F9A0
		protected void cleanupResources()
		{
			if (this.texture != null)
			{
				Object.Destroy(this.texture);
				this.texture = null;
			}
			if (this.sprite != null)
			{
				Object.Destroy(this.sprite);
				this.sprite = null;
			}
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x001217ED File Offset: 0x0011F9ED
		protected virtual void Start()
		{
			this.Refresh();
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x001217F5 File Offset: 0x0011F9F5
		protected void OnDestroy()
		{
			this.cleanupResources();
		}

		// Token: 0x04002615 RID: 9749
		public Image targetImage;

		// Token: 0x04002616 RID: 9750
		public string url;

		// Token: 0x04002617 RID: 9751
		public bool shouldCache = true;

		/// <summary>
		/// If set, we are responsible for destroying texture.
		/// </summary>
		// Token: 0x04002618 RID: 9752
		protected Texture2D texture;

		// Token: 0x04002619 RID: 9753
		protected Sprite sprite;
	}
}
