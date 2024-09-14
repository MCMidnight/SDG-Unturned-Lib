using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x020001A3 RID: 419
	internal class GlazierSprite_UIToolkit : GlazierElementBase_UIToolkit, ISleekSprite, ISleekElement
	{
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x0002AA86 File Offset: 0x00028C86
		// (set) Token: 0x06000CBA RID: 3258 RVA: 0x0002AA8E File Offset: 0x00028C8E
		public Sprite Sprite
		{
			get
			{
				return this._sprite;
			}
			set
			{
				this._sprite = value;
				this.SynchronizeImage();
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x0002AA9D File Offset: 0x00028C9D
		// (set) Token: 0x06000CBC RID: 3260 RVA: 0x0002AAA5 File Offset: 0x00028CA5
		public SleekColor TintColor
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
				this.control.tintColor = this._color;
				this.control.style.unityBackgroundImageTintColor = this._color;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x0002AADF File Offset: 0x00028CDF
		// (set) Token: 0x06000CBE RID: 3262 RVA: 0x0002AAE7 File Offset: 0x00028CE7
		public ESleekSpriteType DrawMethod
		{
			get
			{
				return this._drawMethod;
			}
			set
			{
				this._drawMethod = value;
				this.SynchronizeImage();
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0002AAF6 File Offset: 0x00028CF6
		// (set) Token: 0x06000CC0 RID: 3264 RVA: 0x0002AAFD File Offset: 0x00028CFD
		public bool IsRaycastTarget
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0002AB04 File Offset: 0x00028D04
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x0002AB0C File Offset: 0x00028D0C
		public Vector2Int TileRepeatHintForUITK
		{
			get
			{
				return this._tileRepeat;
			}
			set
			{
				if (this._tileRepeat != value)
				{
					this._tileRepeat = value;
					this.SynchronizeImage();
				}
			}
		}

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x06000CC3 RID: 3267 RVA: 0x0002AB2C File Offset: 0x00028D2C
		// (remove) Token: 0x06000CC4 RID: 3268 RVA: 0x0002AB64 File Offset: 0x00028D64
		private event Action _onImageClicked;

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x06000CC5 RID: 3269 RVA: 0x0002AB99 File Offset: 0x00028D99
		// (remove) Token: 0x06000CC6 RID: 3270 RVA: 0x0002ABB0 File Offset: 0x00028DB0
		public event Action OnClicked
		{
			add
			{
				if (this.clickable == null)
				{
					this.CreateClickable();
				}
				this._onImageClicked += value;
			}
			remove
			{
				this._onImageClicked -= value;
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0002ABBC File Offset: 0x00028DBC
		public GlazierSprite_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
			this.control = new Image();
			this.control.userData = this;
			this.control.AddToClassList("unturned-sprite");
			this.control.pickingMode = 1;
			this.control.scaleMode = 0;
			this.visualElement = this.control;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0002AC28 File Offset: 0x00028E28
		internal override void SynchronizeColors()
		{
			this.control.tintColor = this._color;
			this.control.style.unityBackgroundImageTintColor = this._color;
			if (this.hackTiledImages != null)
			{
				foreach (Image image in this.hackTiledImages)
				{
					image.tintColor = this._color;
				}
			}
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0002ACBC File Offset: 0x00028EBC
		private void CreateClickable()
		{
			this.control.pickingMode = 0;
			this.clickable = new Clickable(new Action<EventBase>(this.OnClickedWithEventInfo));
			GlazierUtils_UIToolkit.AddClickableActivators(this.clickable);
			VisualElementExtensions.AddManipulator(this.control, this.clickable);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0002AD08 File Offset: 0x00028F08
		private void SynchronizeImage()
		{
			switch (this._drawMethod)
			{
			case 0:
				this.control.sprite = null;
				this.control.style.backgroundImage = 1;
				this.UpdateTiledImages();
				return;
			case 1:
			{
				this.control.sprite = null;
				IStyle style = this.control.style;
				Sprite sprite = this._sprite;
				style.backgroundImage = ((sprite != null) ? sprite.texture : null);
				this.DestroyTiledImages();
				return;
			}
			case 2:
				this.control.sprite = this._sprite;
				this.control.style.backgroundImage = 1;
				this.DestroyTiledImages();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0002ADBF File Offset: 0x00028FBF
		private void DestroyTiledImages()
		{
			if (this.tiledImagesContainer != null)
			{
				this.tiledImagesContainer.RemoveFromHierarchy();
				this.tiledImagesContainer = null;
			}
			this.hackTiledImages = null;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0002ADE4 File Offset: 0x00028FE4
		private void UpdateTiledImages()
		{
			int num = this._tileRepeat.x * this._tileRepeat.y;
			if (num < 1 || this._sprite == null)
			{
				if (this.tiledImagesContainer != null)
				{
					this.tiledImagesContainer.RemoveFromHierarchy();
				}
				return;
			}
			if (this.tiledImagesContainer == null)
			{
				this.tiledImagesContainer = new VisualElement();
				this.tiledImagesContainer.AddToClassList("unturned-empty");
				this.tiledImagesContainer.pickingMode = 1;
				this.tiledImagesContainer.style.position = 1;
				this.tiledImagesContainer.style.left = 0f;
				this.tiledImagesContainer.style.right = 0f;
				this.tiledImagesContainer.style.top = 0f;
				this.tiledImagesContainer.style.bottom = 0f;
			}
			if (this.tiledImagesContainer.parent != this.visualElement)
			{
				this.visualElement.Add(this.tiledImagesContainer);
				this.tiledImagesContainer.SendToBack();
			}
			if (this.hackTiledImages == null)
			{
				this.hackTiledImages = new List<Image>(num);
			}
			else
			{
				this.hackTiledImages.Capacity = Mathf.Max(this.hackTiledImages.Capacity, num);
			}
			if (this.hackTiledImages.Count > num)
			{
				for (int i = this.hackTiledImages.Count - 1; i >= num; i--)
				{
					this.hackTiledImages[i].RemoveFromHierarchy();
				}
			}
			else if (this.hackTiledImages.Count < num)
			{
				for (int j = this.hackTiledImages.Count; j < num; j++)
				{
					Image image = new Image();
					image.AddToClassList("unturned-sprite");
					image.style.position = 1;
					image.pickingMode = 1;
					image.scaleMode = 0;
					this.tiledImagesContainer.Add(image);
					this.hackTiledImages.Add(image);
				}
			}
			float num2 = 100f / (float)this._tileRepeat.x;
			float num3 = 100f / (float)this._tileRepeat.y;
			for (int k = 0; k < num; k++)
			{
				Image image2 = this.hackTiledImages[k];
				if (image2.parent == null)
				{
					this.tiledImagesContainer.Add(image2);
				}
				image2.sprite = this._sprite;
				image2.tintColor = this._color;
				int num4 = k / this._tileRepeat.x;
				int num5 = k % this._tileRepeat.x;
				image2.style.left = Length.Percent((float)num5 * num2);
				image2.style.top = Length.Percent((float)num4 * num3);
				image2.style.width = Length.Percent(num2);
				image2.style.height = Length.Percent(num3);
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0002B0F4 File Offset: 0x000292F4
		private void OnClickedWithEventInfo(EventBase eventBase)
		{
			Action onImageClicked = this._onImageClicked;
			if (onImageClicked == null)
			{
				return;
			}
			onImageClicked.Invoke();
		}

		// Token: 0x040004DA RID: 1242
		private Sprite _sprite;

		// Token: 0x040004DB RID: 1243
		private SleekColor _color = 0;

		// Token: 0x040004DC RID: 1244
		private ESleekSpriteType _drawMethod;

		// Token: 0x040004DD RID: 1245
		private Vector2Int _tileRepeat;

		// Token: 0x040004DF RID: 1247
		private Image control;

		// Token: 0x040004E0 RID: 1248
		private Clickable clickable;

		// Token: 0x040004E1 RID: 1249
		private VisualElement tiledImagesContainer;

		// Token: 0x040004E2 RID: 1250
		private List<Image> hackTiledImages;
	}
}
