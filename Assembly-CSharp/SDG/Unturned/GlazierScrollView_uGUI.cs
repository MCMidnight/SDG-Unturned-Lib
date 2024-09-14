using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000187 RID: 391
	internal class GlazierScrollView_uGUI : GlazierElementBase_uGUI, ISleekScrollView, ISleekElement
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x0002480C File Offset: 0x00022A0C
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x00024814 File Offset: 0x00022A14
		public bool ScaleContentToWidth
		{
			get
			{
				return this._scaleContentToWidth;
			}
			set
			{
				this._scaleContentToWidth = value;
				this.contentTransform.anchorMax = new Vector2(this._scaleContentToWidth ? this._contentScaleFactor : 0f, 1f);
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x00024847 File Offset: 0x00022A47
		// (set) Token: 0x06000AEB RID: 2795 RVA: 0x0002484F File Offset: 0x00022A4F
		public bool ScaleContentToHeight
		{
			get
			{
				return this._scaleContentToHeight;
			}
			set
			{
				this._scaleContentToHeight = value;
				this.contentTransform.anchorMin = new Vector2(0f, this._scaleContentToHeight ? (1f - this._contentScaleFactor) : 1f);
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x00024888 File Offset: 0x00022A88
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x00024890 File Offset: 0x00022A90
		public float ContentScaleFactor
		{
			get
			{
				return this._contentScaleFactor;
			}
			set
			{
				this._contentScaleFactor = value;
				this.contentTransform.anchorMin = new Vector2(0f, this._scaleContentToHeight ? (1f - this._contentScaleFactor) : 1f);
				this.contentTransform.anchorMax = new Vector2(this._scaleContentToWidth ? this.ContentScaleFactor : 0f, 1f);
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x000248FE File Offset: 0x00022AFE
		// (set) Token: 0x06000AEF RID: 2799 RVA: 0x00024906 File Offset: 0x00022B06
		public bool ReduceWidthWhenScrollbarVisible
		{
			get
			{
				return this._reduceWidthWhenScrollbarVisible;
			}
			set
			{
				this._reduceWidthWhenScrollbarVisible = value;
				this.scrollRectComponent.verticalScrollbarVisibility = (value ? 2 : 1);
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x00024921 File Offset: 0x00022B21
		// (set) Token: 0x06000AF1 RID: 2801 RVA: 0x0002492C File Offset: 0x00022B2C
		public ESleekScrollbarVisibility VerticalScrollbarVisibility
		{
			get
			{
				return this._verticalScrollbarVisibility;
			}
			set
			{
				if (this._verticalScrollbarVisibility != value)
				{
					this._verticalScrollbarVisibility = value;
					this.verticalScrollbarBackgroundImage.gameObject.SetActive(this._verticalScrollbarVisibility != 1);
					this.verticalScrollbarHandleImage.gameObject.SetActive(this._verticalScrollbarVisibility != 1);
				}
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00024981 File Offset: 0x00022B81
		// (set) Token: 0x06000AF3 RID: 2803 RVA: 0x0002498E File Offset: 0x00022B8E
		public Vector2 ContentSizeOffset
		{
			get
			{
				return this.contentTransform.sizeDelta;
			}
			set
			{
				this.contentTransform.sizeDelta = value;
				this.scrollRectComponent.Rebuild(2);
				this.ClampScrollBars();
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x000249B0 File Offset: 0x00022BB0
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x00024A14 File Offset: 0x00022C14
		public Vector2 NormalizedStateCenter
		{
			get
			{
				Rect absoluteRect = this.scrollRectComponent.viewport.GetAbsoluteRect();
				Rect absoluteRect2 = this.contentTransform.GetAbsoluteRect();
				Vector2 center = absoluteRect.center;
				return new Vector2((center.x - absoluteRect2.xMin) / absoluteRect2.width, (center.y - absoluteRect2.yMin) / absoluteRect2.height);
			}
			set
			{
				Rect absoluteRect = this.scrollRectComponent.viewport.GetAbsoluteRect();
				Rect absoluteRect2 = this.contentTransform.GetAbsoluteRect();
				this.contentTransform.anchoredPosition = new Vector2(value.x * -absoluteRect2.width + absoluteRect.width * 0.5f, value.y * absoluteRect2.height - absoluteRect.height * 0.5f);
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x00024A87 File Offset: 0x00022C87
		// (set) Token: 0x06000AF7 RID: 2807 RVA: 0x00024A94 File Offset: 0x00022C94
		public bool HandleScrollWheel
		{
			get
			{
				return this.scrollRectComponent.HandleScrollWheel;
			}
			set
			{
				this.scrollRectComponent.HandleScrollWheel = value;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x00024AA2 File Offset: 0x00022CA2
		// (set) Token: 0x06000AF9 RID: 2809 RVA: 0x00024AAA File Offset: 0x00022CAA
		public SleekColor BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this._backgroundColor = value;
				this.horizontalScrollbarBackgroundImage.color = this._backgroundColor;
				this.verticalScrollbarBackgroundImage.color = this._backgroundColor;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x00024ADF File Offset: 0x00022CDF
		// (set) Token: 0x06000AFB RID: 2811 RVA: 0x00024AE7 File Offset: 0x00022CE7
		public SleekColor ForegroundColor
		{
			get
			{
				return this._foregroundColor;
			}
			set
			{
				this._foregroundColor = value;
				this.horizontalScrollbarHandleImage.color = this._foregroundColor;
				this.verticalScrollbarHandleImage.color = this._foregroundColor;
			}
		}

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06000AFC RID: 2812 RVA: 0x00024B1C File Offset: 0x00022D1C
		// (remove) Token: 0x06000AFD RID: 2813 RVA: 0x00024B54 File Offset: 0x00022D54
		public event Action<Vector2> OnNormalizedValueChanged;

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x00024B89 File Offset: 0x00022D89
		public float NormalizedVerticalPosition
		{
			get
			{
				return 1f - this.scrollRectComponent.verticalNormalizedPosition;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x00024B9C File Offset: 0x00022D9C
		public float NormalizedViewportHeight
		{
			get
			{
				return this.scrollRectComponent.verticalScrollbar.size;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x00024BAE File Offset: 0x00022DAE
		// (set) Token: 0x06000B01 RID: 2817 RVA: 0x00024BB8 File Offset: 0x00022DB8
		public bool ContentUseManualLayout
		{
			get
			{
				return this._contentUseManualLayout;
			}
			set
			{
				if (this._contentUseManualLayout != value)
				{
					this._contentUseManualLayout = value;
					if (this._contentUseManualLayout)
					{
						this.contentTransform.DestroyComponentIfExists<VerticalLayoutGroup>();
						this.contentTransform.DestroyComponentIfExists<ContentSizeFitter>();
						return;
					}
					this.contentTransform.gameObject.AddComponent<VerticalLayoutGroup>();
					this.contentTransform.gameObject.AddComponent<ContentSizeFitter>().verticalFit = 2;
				}
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x00024C1B File Offset: 0x00022E1B
		// (set) Token: 0x06000B03 RID: 2819 RVA: 0x00024C23 File Offset: 0x00022E23
		public bool AlignContentToBottom
		{
			get
			{
				return this._alignContentToBottom;
			}
			set
			{
				this._alignContentToBottom = value;
				this.contentTransform.pivot = new Vector2(0f, this._alignContentToBottom ? 0f : 1f);
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00024C55 File Offset: 0x00022E55
		// (set) Token: 0x06000B05 RID: 2821 RVA: 0x00024C62 File Offset: 0x00022E62
		public bool IsRaycastTarget
		{
			get
			{
				return this.contentImage.raycastTarget;
			}
			set
			{
				this.contentImage.raycastTarget = value;
			}
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00024C70 File Offset: 0x00022E70
		public void ScrollToTop()
		{
			this.scrollRectComponent.verticalNormalizedPosition = 1f;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00024C82 File Offset: 0x00022E82
		public void ScrollToBottom()
		{
			this.scrollRectComponent.verticalNormalizedPosition = 0f;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00024C94 File Offset: 0x00022E94
		public GlazierScrollView_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00024CB8 File Offset: 0x00022EB8
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.scrollRectComponent = base.gameObject.AddComponent<ScrollRectEx>();
			this.scrollRectComponent.movementType = 2;
			this.scrollRectComponent.inertia = false;
			this.scrollRectComponent.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnUnityValueChanged));
			this.scrollRectComponent.scrollSensitivity = 40f * GlazierBase.ScrollViewSensitivityMultiplier;
			GameObject gameObject = new GameObject("Viewport", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform rectTransform = gameObject.GetRectTransform();
			rectTransform.SetParent(base.transform, false);
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.sizeDelta = Vector2.zero;
			gameObject.AddComponent<RectMask2D>();
			this.scrollRectComponent.viewport = rectTransform;
			GameObject gameObject2 = new GameObject("Content", new Type[]
			{
				typeof(RectTransform)
			});
			this.contentTransform = gameObject2.GetRectTransform();
			this.contentTransform.SetParent(rectTransform, false);
			this.contentTransform.pivot = new Vector2(0f, 1f);
			this.contentTransform.anchorMin = new Vector2(0f, 1f);
			this.contentTransform.anchorMax = new Vector2(0f, 1f);
			this.contentTransform.anchoredPosition = Vector2.zero;
			this.contentTransform.sizeDelta = Vector2.zero;
			this.scrollRectComponent.content = this.contentTransform;
			this.contentImage = gameObject2.AddComponent<Image>();
			this.contentImage.color = new Color(1f, 1f, 1f, 0f);
			GameObject gameObject3 = new GameObject("Horizontal Scrollbar", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform rectTransform2 = gameObject3.GetRectTransform();
			rectTransform2.SetParent(base.transform, false);
			rectTransform2.pivot = new Vector2(0f, 0f);
			rectTransform2.anchorMin = new Vector2(0f, 0f);
			rectTransform2.anchorMax = new Vector2(1f, 0f);
			rectTransform2.anchoredPosition = Vector2.zero;
			rectTransform2.sizeDelta = new Vector2(0f, 20f);
			GameObject gameObject4 = new GameObject("Background", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform rectTransform3 = gameObject4.GetRectTransform();
			rectTransform3.SetParent(rectTransform2, false);
			rectTransform3.anchorMin = new Vector2(0f, 0.5f);
			rectTransform3.anchorMax = new Vector2(1f, 0.5f);
			rectTransform3.anchoredPosition = Vector2.zero;
			rectTransform3.sizeDelta = new Vector2(-20f, 6f);
			this.horizontalScrollbarBackgroundImage = gameObject4.AddComponent<Image>();
			this.horizontalScrollbarBackgroundImage.type = 1;
			this.horizontalScrollbarBackgroundImage.raycastTarget = true;
			RectTransform rectTransform4 = new GameObject("Handle Padding", new Type[]
			{
				typeof(RectTransform)
			}).GetRectTransform();
			rectTransform4.SetParent(rectTransform2, false);
			rectTransform4.anchorMin = Vector2.zero;
			rectTransform4.anchorMax = Vector2.one;
			rectTransform4.anchoredPosition = Vector2.zero;
			rectTransform4.sizeDelta = new Vector2(-20f, 0f);
			GameObject gameObject5 = new GameObject("Handle", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform rectTransform5 = gameObject5.GetRectTransform();
			rectTransform5.SetParent(rectTransform4, false);
			rectTransform5.anchoredPosition = Vector2.zero;
			rectTransform5.sizeDelta = new Vector2(20f, 0f);
			this.horizontalScrollbarHandleImage = gameObject5.AddComponent<Image>();
			this.horizontalScrollbarHandleImage.type = 1;
			this.horizontalScrollbarHandleImage.raycastTarget = true;
			this.horizontalScrollbarComponent = gameObject3.AddComponent<Scrollbar>();
			this.horizontalScrollbarComponent.SetDirection(0, false);
			this.horizontalScrollbarComponent.handleRect = rectTransform5;
			this.horizontalScrollbarComponent.transition = 2;
			this.horizontalScrollbarComponent.targetGraphic = this.horizontalScrollbarHandleImage;
			this.scrollRectComponent.horizontalScrollbarSpacing = 10f;
			this.scrollRectComponent.horizontalScrollbarVisibility = 2;
			this.scrollRectComponent.horizontalScrollbar = this.horizontalScrollbarComponent;
			GameObject gameObject6 = new GameObject("Vertical Scrollbar", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform rectTransform6 = gameObject6.GetRectTransform();
			rectTransform6.SetParent(base.transform, false);
			rectTransform6.pivot = new Vector2(1f, 1f);
			rectTransform6.anchorMin = new Vector2(1f, 0f);
			rectTransform6.anchorMax = new Vector2(1f, 1f);
			rectTransform6.anchoredPosition = Vector2.zero;
			rectTransform6.sizeDelta = new Vector2(20f, 0f);
			GameObject gameObject7 = new GameObject("Background", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform rectTransform7 = gameObject7.GetRectTransform();
			rectTransform7.SetParent(rectTransform6, false);
			rectTransform7.anchorMin = new Vector2(0.5f, 0f);
			rectTransform7.anchorMax = new Vector2(0.5f, 1f);
			rectTransform7.anchoredPosition = Vector2.zero;
			rectTransform7.sizeDelta = new Vector2(6f, -20f);
			this.verticalScrollbarBackgroundImage = gameObject7.AddComponent<Image>();
			this.verticalScrollbarBackgroundImage.type = 1;
			this.verticalScrollbarBackgroundImage.raycastTarget = true;
			RectTransform rectTransform8 = new GameObject("Handle Padding", new Type[]
			{
				typeof(RectTransform)
			}).GetRectTransform();
			rectTransform8.SetParent(rectTransform6, false);
			rectTransform8.anchorMin = Vector2.zero;
			rectTransform8.anchorMax = Vector2.one;
			rectTransform8.anchoredPosition = Vector2.zero;
			rectTransform8.sizeDelta = new Vector2(0f, -20f);
			GameObject gameObject8 = new GameObject("Handle", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform rectTransform9 = gameObject8.GetRectTransform();
			rectTransform9.SetParent(rectTransform8, false);
			rectTransform9.anchoredPosition = Vector2.zero;
			rectTransform9.sizeDelta = new Vector2(0f, 20f);
			this.verticalScrollbarHandleImage = gameObject8.AddComponent<Image>();
			this.verticalScrollbarHandleImage.type = 1;
			this.verticalScrollbarHandleImage.raycastTarget = true;
			this.verticalScrollbarComponent = gameObject6.AddComponent<Scrollbar>();
			this.verticalScrollbarComponent.SetDirection(2, false);
			this.verticalScrollbarComponent.handleRect = rectTransform9;
			this.verticalScrollbarComponent.transition = 2;
			this.verticalScrollbarComponent.targetGraphic = this.verticalScrollbarHandleImage;
			this.scrollRectComponent.verticalScrollbarSpacing = 10f;
			this.scrollRectComponent.verticalScrollbarVisibility = 2;
			this.scrollRectComponent.verticalScrollbar = this.verticalScrollbarComponent;
			this.HandleScrollWheel = true;
			this._backgroundColor = GlazierConst.DefaultScrollViewBackgroundColor;
			this._foregroundColor = GlazierConst.DefaultScrollViewForegroundColor;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x000253B4 File Offset: 0x000235B4
		public override void SynchronizeColors()
		{
			this.horizontalScrollbarBackgroundImage.color = this._backgroundColor;
			this.horizontalScrollbarHandleImage.color = this._foregroundColor;
			this.verticalScrollbarBackgroundImage.color = this._backgroundColor;
			this.verticalScrollbarHandleImage.color = this._foregroundColor;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0002541C File Offset: 0x0002361C
		public override void SynchronizeTheme()
		{
			SpriteState spriteState = default(SpriteState);
			spriteState.disabledSprite = GlazierResources_uGUI.Theme.BoxSprite;
			spriteState.highlightedSprite = GlazierResources_uGUI.Theme.BoxHighlightedSprite;
			spriteState.selectedSprite = GlazierResources_uGUI.Theme.BoxSelectedSprite;
			spriteState.pressedSprite = GlazierResources_uGUI.Theme.BoxPressedSprite;
			this.horizontalScrollbarBackgroundImage.sprite = GlazierResources_uGUI.Theme.SliderBackgroundSprite;
			this.horizontalScrollbarHandleImage.sprite = GlazierResources_uGUI.Theme.BoxSprite;
			this.horizontalScrollbarComponent.spriteState = spriteState;
			this.verticalScrollbarBackgroundImage.sprite = GlazierResources_uGUI.Theme.SliderBackgroundSprite;
			this.verticalScrollbarHandleImage.sprite = GlazierResources_uGUI.Theme.BoxSprite;
			this.verticalScrollbarComponent.spriteState = spriteState;
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00025504 File Offset: 0x00023704
		public override RectTransform AttachmentTransform
		{
			get
			{
				return this.contentTransform;
			}
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x0002550C File Offset: 0x0002370C
		private void ClampScrollBars()
		{
			Vector2 vector = this.scrollRectComponent.normalizedPosition;
			vector = MathfEx.Clamp01(vector);
			this.scrollRectComponent.normalizedPosition = vector;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00025538 File Offset: 0x00023738
		private void OnUnityValueChanged(Vector2 value)
		{
			value.y = 1f - value.y;
			Action<Vector2> onNormalizedValueChanged = this.OnNormalizedValueChanged;
			if (onNormalizedValueChanged == null)
			{
				return;
			}
			onNormalizedValueChanged.Invoke(value);
		}

		// Token: 0x04000425 RID: 1061
		private bool _scaleContentToWidth;

		// Token: 0x04000426 RID: 1062
		private bool _scaleContentToHeight;

		// Token: 0x04000427 RID: 1063
		private float _contentScaleFactor = 1f;

		// Token: 0x04000428 RID: 1064
		private bool _reduceWidthWhenScrollbarVisible = true;

		// Token: 0x04000429 RID: 1065
		private ESleekScrollbarVisibility _verticalScrollbarVisibility;

		// Token: 0x0400042A RID: 1066
		private SleekColor _backgroundColor;

		// Token: 0x0400042B RID: 1067
		private SleekColor _foregroundColor;

		// Token: 0x0400042D RID: 1069
		protected bool _contentUseManualLayout = true;

		// Token: 0x0400042E RID: 1070
		protected bool _alignContentToBottom;

		// Token: 0x0400042F RID: 1071
		private ScrollRectEx scrollRectComponent;

		// Token: 0x04000430 RID: 1072
		private RectTransform contentTransform;

		// Token: 0x04000431 RID: 1073
		private Image contentImage;

		// Token: 0x04000432 RID: 1074
		private Image horizontalScrollbarBackgroundImage;

		// Token: 0x04000433 RID: 1075
		private Image horizontalScrollbarHandleImage;

		// Token: 0x04000434 RID: 1076
		private Scrollbar horizontalScrollbarComponent;

		// Token: 0x04000435 RID: 1077
		private Image verticalScrollbarBackgroundImage;

		// Token: 0x04000436 RID: 1078
		private Image verticalScrollbarHandleImage;

		// Token: 0x04000437 RID: 1079
		private Scrollbar verticalScrollbarComponent;
	}
}
