using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x020001A1 RID: 417
	internal class GlazierScrollView_UIToolkit : GlazierElementBase_UIToolkit, ISleekScrollView, ISleekElement
	{
		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00029FF2 File Offset: 0x000281F2
		// (set) Token: 0x06000C80 RID: 3200 RVA: 0x00029FFA File Offset: 0x000281FA
		public bool ScaleContentToWidth
		{
			get
			{
				return this._scaleContentToWidth;
			}
			set
			{
				this._scaleContentToWidth = value;
				this.SynchronizeContentContainerStyle();
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x0002A009 File Offset: 0x00028209
		// (set) Token: 0x06000C82 RID: 3202 RVA: 0x0002A011 File Offset: 0x00028211
		public bool ScaleContentToHeight
		{
			get
			{
				return this._scaleContentToHeight;
			}
			set
			{
				this._scaleContentToHeight = value;
				this.SynchronizeContentContainerStyle();
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0002A020 File Offset: 0x00028220
		// (set) Token: 0x06000C84 RID: 3204 RVA: 0x0002A028 File Offset: 0x00028228
		public float ContentScaleFactor
		{
			get
			{
				return this._contentScaleFactor;
			}
			set
			{
				this._contentScaleFactor = value;
				this.SynchronizeContentContainerStyle();
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0002A037 File Offset: 0x00028237
		// (set) Token: 0x06000C86 RID: 3206 RVA: 0x0002A03F File Offset: 0x0002823F
		public bool ReduceWidthWhenScrollbarVisible
		{
			get
			{
				return this._reduceWidthWhenScrollbarVisible;
			}
			set
			{
				this._reduceWidthWhenScrollbarVisible = value;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x0002A048 File Offset: 0x00028248
		// (set) Token: 0x06000C88 RID: 3208 RVA: 0x0002A050 File Offset: 0x00028250
		public ESleekScrollbarVisibility VerticalScrollbarVisibility
		{
			get
			{
				return this._verticalScrollbarVisibility;
			}
			set
			{
				this._verticalScrollbarVisibility = value;
				this.control.verticalScrollerVisibility = ((this._verticalScrollbarVisibility == 1) ? 2 : 0);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x0002A071 File Offset: 0x00028271
		// (set) Token: 0x06000C8A RID: 3210 RVA: 0x0002A079 File Offset: 0x00028279
		public Vector2 ContentSizeOffset
		{
			get
			{
				return this._contentSizeOffset;
			}
			set
			{
				this._contentSizeOffset = value;
				this.SynchronizeContentContainerStyle();
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x0002A088 File Offset: 0x00028288
		// (set) Token: 0x06000C8C RID: 3212 RVA: 0x0002A0B5 File Offset: 0x000282B5
		public Vector2 NormalizedStateCenter
		{
			get
			{
				return new Vector2(this.NormalizedHorizontalPosition + this.NormalizedViewportWidth * 0.5f, this.NormalizedVerticalPosition + this.NormalizedViewportHeight * 0.5f);
			}
			set
			{
				this.NormalizedHorizontalPosition = value.x - this.NormalizedViewportWidth * 0.5f;
				this.NormalizedVerticalPosition = value.y - this.NormalizedViewportHeight * 0.5f;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0002A0E9 File Offset: 0x000282E9
		// (set) Token: 0x06000C8E RID: 3214 RVA: 0x0002A0EC File Offset: 0x000282EC
		public bool HandleScrollWheel
		{
			get
			{
				return false;
			}
			set
			{
				this._handleScrollWheel = value;
				this.control.mouseWheelScrollSize = (this._handleScrollWheel ? (600f * GlazierBase.ScrollViewSensitivityMultiplier) : 0f);
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0002A11A File Offset: 0x0002831A
		// (set) Token: 0x06000C90 RID: 3216 RVA: 0x0002A122 File Offset: 0x00028322
		public SleekColor BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this._backgroundColor = value;
				this.horizontalTracker.style.unityBackgroundImageTintColor = this._backgroundColor;
				this.verticalTracker.style.unityBackgroundImageTintColor = this._backgroundColor;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x0002A161 File Offset: 0x00028361
		// (set) Token: 0x06000C92 RID: 3218 RVA: 0x0002A169 File Offset: 0x00028369
		public SleekColor ForegroundColor
		{
			get
			{
				return this._foregroundColor;
			}
			set
			{
				this._foregroundColor = value;
				this.horizontalDragger.style.unityBackgroundImageTintColor = this._foregroundColor;
				this.verticalDragger.style.unityBackgroundImageTintColor = this._foregroundColor;
			}
		}

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x06000C93 RID: 3219 RVA: 0x0002A1A8 File Offset: 0x000283A8
		// (remove) Token: 0x06000C94 RID: 3220 RVA: 0x0002A1E0 File Offset: 0x000283E0
		public event Action<Vector2> OnNormalizedValueChanged;

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0002A218 File Offset: 0x00028418
		// (set) Token: 0x06000C96 RID: 3222 RVA: 0x0002A248 File Offset: 0x00028448
		private float NormalizedHorizontalPosition
		{
			get
			{
				Scroller horizontalScroller = this.control.horizontalScroller;
				return Mathf.InverseLerp(horizontalScroller.lowValue, horizontalScroller.highValue, horizontalScroller.value);
			}
			set
			{
				Scroller horizontalScroller = this.control.horizontalScroller;
				horizontalScroller.value = Mathf.Lerp(horizontalScroller.lowValue, horizontalScroller.highValue, value);
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0002A27C File Offset: 0x0002847C
		// (set) Token: 0x06000C98 RID: 3224 RVA: 0x0002A2AC File Offset: 0x000284AC
		public float NormalizedVerticalPosition
		{
			get
			{
				Scroller verticalScroller = this.control.verticalScroller;
				return Mathf.InverseLerp(verticalScroller.lowValue, verticalScroller.highValue, verticalScroller.value);
			}
			private set
			{
				Scroller verticalScroller = this.control.verticalScroller;
				verticalScroller.value = Mathf.Lerp(verticalScroller.lowValue, verticalScroller.highValue, value);
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0002A2E0 File Offset: 0x000284E0
		private float NormalizedViewportWidth
		{
			get
			{
				return this.control.contentViewport.layout.width / this.control.contentContainer.localBound.width;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x0002A320 File Offset: 0x00028520
		public float NormalizedViewportHeight
		{
			get
			{
				return this.control.contentViewport.layout.height / this.control.contentContainer.localBound.height;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0002A35E File Offset: 0x0002855E
		// (set) Token: 0x06000C9C RID: 3228 RVA: 0x0002A366 File Offset: 0x00028566
		public bool ContentUseManualLayout
		{
			get
			{
				return this._contentUseManualLayout;
			}
			set
			{
				this._contentUseManualLayout = value;
				this.SynchronizeContentContainerStyle();
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0002A375 File Offset: 0x00028575
		// (set) Token: 0x06000C9E RID: 3230 RVA: 0x0002A37D File Offset: 0x0002857D
		public bool AlignContentToBottom
		{
			get
			{
				return this._alignContentToBottom;
			}
			set
			{
				if (this._alignContentToBottom != value)
				{
					this._alignContentToBottom = value;
					this.contentViewport.style.justifyContent = (this._alignContentToBottom ? 2 : 1);
				}
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x0002A3B5 File Offset: 0x000285B5
		// (set) Token: 0x06000CA0 RID: 3232 RVA: 0x0002A3BD File Offset: 0x000285BD
		public bool IsRaycastTarget
		{
			get
			{
				return this._isRaycastTarget;
			}
			set
			{
				this._isRaycastTarget = value;
			}
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0002A3C6 File Offset: 0x000285C6
		public void ScrollToTop()
		{
			this.control.verticalScroller.value = this.control.verticalScroller.lowValue;
			this.wantsToScrollToBottom = false;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0002A3EF File Offset: 0x000285EF
		public void ScrollToBottom()
		{
			this.wantsToScrollToBottom = true;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0002A3F8 File Offset: 0x000285F8
		public GlazierScrollView_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
			this.control = new ScrollView();
			this.control.userData = this;
			this.control.AddToClassList("unturned-scroll-view");
			this.control.horizontalScroller.valueChanged += new Action<float>(this.OnHorizontalValueChanged);
			this.control.verticalScroller.valueChanged += new Action<float>(this.OnVerticalValueChanged);
			this.control.mouseWheelScrollSize = 600f * GlazierBase.ScrollViewSensitivityMultiplier;
			this.contentViewport = this.control.contentViewport;
			this.contentContainer = this.control.contentContainer;
			this.control.pickingMode = 1;
			UQueryExtensions.Q(this.control, null, ScrollView.contentAndVerticalScrollUssClassName).pickingMode = 1;
			this.contentViewport.pickingMode = 1;
			VisualElement visualElement = UQueryExtensions.Q(UQueryExtensions.Q(this.control.horizontalScroller, null, "unity-base-slider__input"), null, "unity-base-slider__drag-container");
			this.horizontalTracker = UQueryExtensions.Q(visualElement, null, "unity-base-slider__tracker");
			this.horizontalDragger = UQueryExtensions.Q(visualElement, null, "unity-base-slider__dragger");
			VisualElement visualElement2 = UQueryExtensions.Q(UQueryExtensions.Q(this.control.verticalScroller, null, "unity-base-slider__input"), null, "unity-base-slider__drag-container");
			this.verticalTracker = UQueryExtensions.Q(visualElement2, null, "unity-base-slider__tracker");
			this.verticalDragger = UQueryExtensions.Q(visualElement2, null, "unity-base-slider__dragger");
			this.visualElement = this.control;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0002A5A7 File Offset: 0x000287A7
		public override void Update()
		{
			base.Update();
			if (this.wantsToScrollToBottom)
			{
				this.wantsToScrollToBottom = false;
				this.control.verticalScroller.value = this.control.verticalScroller.highValue;
			}
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0002A5E0 File Offset: 0x000287E0
		internal override void SynchronizeColors()
		{
			this.horizontalTracker.style.unityBackgroundImageTintColor = this._backgroundColor;
			this.horizontalDragger.style.unityBackgroundImageTintColor = this._foregroundColor;
			this.verticalTracker.style.unityBackgroundImageTintColor = this._backgroundColor;
			this.verticalDragger.style.unityBackgroundImageTintColor = this._foregroundColor;
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0002A659 File Offset: 0x00028859
		private void OnHorizontalValueChanged(float value)
		{
			Action<Vector2> onNormalizedValueChanged = this.OnNormalizedValueChanged;
			if (onNormalizedValueChanged == null)
			{
				return;
			}
			onNormalizedValueChanged.Invoke(new Vector2(this.NormalizedHorizontalPosition, this.NormalizedVerticalPosition));
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0002A67C File Offset: 0x0002887C
		private void OnVerticalValueChanged(float value)
		{
			Action<Vector2> onNormalizedValueChanged = this.OnNormalizedValueChanged;
			if (onNormalizedValueChanged == null)
			{
				return;
			}
			onNormalizedValueChanged.Invoke(new Vector2(this.NormalizedHorizontalPosition, this.NormalizedVerticalPosition));
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0002A6A0 File Offset: 0x000288A0
		private void SynchronizeContentContainerStyle()
		{
			if (this._contentUseManualLayout)
			{
				this.contentContainer.style.position = 1;
				float num = (this.ContentScaleFactor - 1f) * 100f;
				this.contentContainer.style.right = Length.Percent(this._scaleContentToWidth ? (-num) : 100f);
				this.contentContainer.style.bottom = Length.Percent(this._scaleContentToHeight ? (-num) : 100f);
				this.contentContainer.style.marginRight = -this._contentSizeOffset.x;
				this.contentContainer.style.marginBottom = -this._contentSizeOffset.y;
				return;
			}
			this.contentContainer.style.position = 0;
			this.contentContainer.style.right = 4;
			this.contentContainer.style.bottom = 4;
			this.contentContainer.style.marginRight = 4;
			this.contentContainer.style.marginBottom = 4;
		}

		// Token: 0x040004BD RID: 1213
		private bool _scaleContentToWidth;

		// Token: 0x040004BE RID: 1214
		private bool _scaleContentToHeight;

		// Token: 0x040004BF RID: 1215
		private float _contentScaleFactor = 1f;

		// Token: 0x040004C0 RID: 1216
		private bool _reduceWidthWhenScrollbarVisible = true;

		// Token: 0x040004C1 RID: 1217
		private ESleekScrollbarVisibility _verticalScrollbarVisibility;

		// Token: 0x040004C2 RID: 1218
		private Vector2 _contentSizeOffset;

		// Token: 0x040004C3 RID: 1219
		private const float MOUSE_WHEEL_SCROLL_SIZE = 600f;

		// Token: 0x040004C4 RID: 1220
		private bool _handleScrollWheel = true;

		// Token: 0x040004C5 RID: 1221
		private SleekColor _backgroundColor = GlazierConst.DefaultScrollViewBackgroundColor;

		// Token: 0x040004C6 RID: 1222
		private SleekColor _foregroundColor = GlazierConst.DefaultScrollViewForegroundColor;

		// Token: 0x040004C8 RID: 1224
		protected bool _contentUseManualLayout = true;

		// Token: 0x040004C9 RID: 1225
		protected bool _alignContentToBottom;

		// Token: 0x040004CA RID: 1226
		private bool _isRaycastTarget = true;

		// Token: 0x040004CB RID: 1227
		private ScrollView control;

		// Token: 0x040004CC RID: 1228
		private VisualElement contentViewport;

		// Token: 0x040004CD RID: 1229
		private VisualElement contentContainer;

		// Token: 0x040004CE RID: 1230
		private VisualElement horizontalTracker;

		// Token: 0x040004CF RID: 1231
		private VisualElement horizontalDragger;

		// Token: 0x040004D0 RID: 1232
		private VisualElement verticalTracker;

		// Token: 0x040004D1 RID: 1233
		private VisualElement verticalDragger;

		// Token: 0x040004D2 RID: 1234
		private bool wantsToScrollToBottom;
	}
}
