using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x0200017F RID: 383
	internal class GlazierImage_uGUI : GlazierElementBase_uGUI, ISleekImage, ISleekElement
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x00023BC4 File Offset: 0x00021DC4
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x00023BCC File Offset: 0x00021DCC
		public Texture Texture
		{
			get
			{
				return this.desiredTexture;
			}
			set
			{
				if (this.desiredTexture != value)
				{
					this.internalSetTexture(value, this.ShouldDestroyTexture);
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00023BE9 File Offset: 0x00021DE9
		// (set) Token: 0x06000A91 RID: 2705 RVA: 0x00023BF1 File Offset: 0x00021DF1
		public float RotationAngle
		{
			get
			{
				return this._angle;
			}
			set
			{
				this._angle = value;
				this.pivotTransform.localRotation = Quaternion.Euler(0f, 0f, -this._angle);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x00023C1B File Offset: 0x00021E1B
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x00023C24 File Offset: 0x00021E24
		public bool CanRotate
		{
			get
			{
				return this._isAngled;
			}
			set
			{
				this._isAngled = value;
				if (this._isAngled)
				{
					this.pivotTransform.localRotation = Quaternion.Euler(0f, 0f, -this._angle);
					return;
				}
				this.pivotTransform.localRotation = Quaternion.identity;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x00023C72 File Offset: 0x00021E72
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x00023C7A File Offset: 0x00021E7A
		public bool ShouldDestroyTexture { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x00023C83 File Offset: 0x00021E83
		// (set) Token: 0x06000A97 RID: 2711 RVA: 0x00023C8B File Offset: 0x00021E8B
		public SleekColor TintColor
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
				this.SynchronizeColors();
			}
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00023C9A File Offset: 0x00021E9A
		public void UpdateTexture(Texture2D newTexture)
		{
			if (this.desiredTexture != newTexture)
			{
				this.internalSetTexture(newTexture, this.ShouldDestroyTexture);
			}
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00023CB7 File Offset: 0x00021EB7
		public void SetTextureAndShouldDestroy(Texture2D newTexture, bool newShouldDestroyTexture)
		{
			if (this.desiredTexture != newTexture || this.ShouldDestroyTexture != newShouldDestroyTexture)
			{
				this.internalSetTexture(newTexture, newShouldDestroyTexture);
			}
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00023CD8 File Offset: 0x00021ED8
		public override void InternalDestroy()
		{
			if (this.ShouldDestroyTexture && this.desiredTexture != null)
			{
				Object.Destroy(this.desiredTexture);
				this.desiredTexture = null;
			}
			base.InternalDestroy();
		}

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06000A9B RID: 2715 RVA: 0x00023D08 File Offset: 0x00021F08
		// (remove) Token: 0x06000A9C RID: 2716 RVA: 0x00023D40 File Offset: 0x00021F40
		private event Action _onImageClicked;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06000A9D RID: 2717 RVA: 0x00023D75 File Offset: 0x00021F75
		// (remove) Token: 0x06000A9E RID: 2718 RVA: 0x00023D92 File Offset: 0x00021F92
		public event Action OnClicked
		{
			add
			{
				if (this.buttonComponent == null)
				{
					this.CreateButton();
				}
				this._onImageClicked += value;
			}
			remove
			{
				this._onImageClicked -= value;
			}
		}

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06000A9F RID: 2719 RVA: 0x00023D9C File Offset: 0x00021F9C
		// (remove) Token: 0x06000AA0 RID: 2720 RVA: 0x00023DD4 File Offset: 0x00021FD4
		private event Action _onImageRightClicked;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06000AA1 RID: 2721 RVA: 0x00023E09 File Offset: 0x00022009
		// (remove) Token: 0x06000AA2 RID: 2722 RVA: 0x00023E26 File Offset: 0x00022026
		public event Action OnRightClicked
		{
			add
			{
				if (this.buttonComponent == null)
				{
					this.CreateButton();
				}
				this._onImageRightClicked += value;
			}
			remove
			{
				this._onImageRightClicked -= value;
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00023E30 File Offset: 0x00022030
		protected override bool ReleaseIntoPool()
		{
			if (!(this.buttonComponent == null))
			{
				return false;
			}
			if (this.rawImageComponent == null)
			{
				return false;
			}
			this.rawImageComponent.enabled = false;
			GlazierImage_uGUI.ImagePoolData imagePoolData = new GlazierImage_uGUI.ImagePoolData();
			base.PopulateBasePoolData(imagePoolData);
			imagePoolData.pivotTransform = this.pivotTransform;
			this.pivotTransform = null;
			imagePoolData.rawImageComponent = this.rawImageComponent;
			this.rawImageComponent = null;
			base.glazier.ReleaseImageToPool(imagePoolData);
			return true;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00023EA9 File Offset: 0x000220A9
		public GlazierImage_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00023EC0 File Offset: 0x000220C0
		public override void ConstructNew()
		{
			base.ConstructNew();
			GameObject gameObject = new GameObject("Pivot", new Type[]
			{
				typeof(RectTransform)
			});
			this.pivotTransform = gameObject.GetRectTransform();
			this.pivotTransform.SetParent(base.transform, false);
			this.pivotTransform.anchorMin = Vector2.zero;
			this.pivotTransform.anchorMax = Vector2.one;
			this.pivotTransform.anchoredPosition = Vector2.zero;
			this.pivotTransform.sizeDelta = Vector2.zero;
			this.rawImageComponent = gameObject.AddComponent<RawImage>();
			this.rawImageComponent.enabled = true;
			this.rawImageComponent.raycastTarget = false;
			this.rawImageComponent.texture = GlazierResources.PixelTexture;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00023F88 File Offset: 0x00022188
		public void ConstructFromImagePool(GlazierImage_uGUI.ImagePoolData poolData)
		{
			base.ConstructFromPool(poolData);
			this.pivotTransform = poolData.pivotTransform;
			this.rawImageComponent = poolData.rawImageComponent;
			this.pivotTransform.anchorMin = Vector2.zero;
			this.pivotTransform.anchorMax = Vector2.one;
			this.pivotTransform.anchoredPosition = Vector2.zero;
			this.pivotTransform.sizeDelta = Vector2.zero;
			this.pivotTransform.localRotation = Quaternion.identity;
			this.rawImageComponent.texture = GlazierResources.PixelTexture;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0002401C File Offset: 0x0002221C
		public override void SynchronizeColors()
		{
			if (this.desiredTexture != null)
			{
				this.rawImageComponent.color = this._color;
				this.rawImageComponent.enabled = true;
				return;
			}
			if (this.rawImageComponent.raycastTarget)
			{
				this.rawImageComponent.color = ColorEx.BlackZeroAlpha;
				this.rawImageComponent.enabled = true;
				return;
			}
			this.rawImageComponent.enabled = false;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00024090 File Offset: 0x00022290
		protected override void EnableComponents()
		{
			this.rawImageComponent.enabled = (this.desiredTexture != null || this.rawImageComponent.raycastTarget);
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x000240BC File Offset: 0x000222BC
		private void CreateButton()
		{
			this.rawImageComponent.raycastTarget = true;
			this.buttonComponent = base.gameObject.AddComponent<ButtonEx>();
			this.buttonComponent.transition = 0;
			this.buttonComponent.onClick.AddListener(new UnityAction(this.OnUnityClick));
			this.buttonComponent.onRightClick.AddListener(new UnityAction(this.OnUnityRightClick));
			this.SynchronizeColors();
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00024130 File Offset: 0x00022330
		private void internalSetTexture(Texture newTexture, bool newShouldDestroyTexture)
		{
			if (this.rawImageComponent == null)
			{
				if (newShouldDestroyTexture && newTexture != null)
				{
					Object.Destroy(newTexture);
				}
				return;
			}
			if (this.ShouldDestroyTexture && this.desiredTexture != null)
			{
				Object.Destroy(this.desiredTexture);
				this.desiredTexture = null;
			}
			this.desiredTexture = newTexture;
			this.ShouldDestroyTexture = newShouldDestroyTexture;
			this.rawImageComponent.texture = ((this.desiredTexture != null) ? this.desiredTexture : GlazierResources.PixelTexture);
			this.SynchronizeColors();
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x000241C5 File Offset: 0x000223C5
		private void OnUnityClick()
		{
			Action onImageClicked = this._onImageClicked;
			if (onImageClicked == null)
			{
				return;
			}
			onImageClicked.Invoke();
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x000241D7 File Offset: 0x000223D7
		private void OnUnityRightClick()
		{
			Action onImageRightClicked = this._onImageRightClicked;
			if (onImageRightClicked == null)
			{
				return;
			}
			onImageRightClicked.Invoke();
		}

		// Token: 0x04000404 RID: 1028
		private float _angle;

		// Token: 0x04000405 RID: 1029
		private bool _isAngled;

		// Token: 0x04000407 RID: 1031
		private SleekColor _color = 0;

		/// <summary>
		/// The base transform does not rotate, instead a child transform is created with the pivot in the center.
		/// </summary>
		// Token: 0x0400040A RID: 1034
		private RectTransform pivotTransform;

		/// <summary>
		/// To work around a uGUI bug we always a sign a texture, even if desiredTexture is null.
		/// </summary>
		// Token: 0x0400040B RID: 1035
		private Texture desiredTexture;

		// Token: 0x0400040C RID: 1036
		private RawImage rawImageComponent;

		// Token: 0x0400040D RID: 1037
		private ButtonEx buttonComponent;

		// Token: 0x0200087B RID: 2171
		public class ImagePoolData : GlazierElementBase_uGUI.PoolData
		{
			// Token: 0x04003194 RID: 12692
			public RectTransform pivotTransform;

			// Token: 0x04003195 RID: 12693
			public RawImage rawImageComponent;
		}
	}
}
