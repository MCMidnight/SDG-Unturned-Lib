using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200016C RID: 364
	internal class GlazierScrollView_IMGUI : GlazierElementBase_IMGUI, ISleekScrollView, ISleekElement
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x00020102 File Offset: 0x0001E302
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x0002010A File Offset: 0x0001E30A
		public bool ScaleContentToWidth
		{
			get
			{
				return this._scaleContentToWidth;
			}
			set
			{
				this._scaleContentToWidth = value;
				this.isTransformDirty = true;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x0002011A File Offset: 0x0001E31A
		// (set) Token: 0x06000948 RID: 2376 RVA: 0x00020122 File Offset: 0x0001E322
		public bool ScaleContentToHeight
		{
			get
			{
				return this._scaleContentToHeight;
			}
			set
			{
				this._scaleContentToHeight = value;
				this.isTransformDirty = true;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x00020132 File Offset: 0x0001E332
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x0002013A File Offset: 0x0001E33A
		public float ContentScaleFactor
		{
			get
			{
				return this._contentScaleFactor;
			}
			set
			{
				this._contentScaleFactor = value;
				this.isTransformDirty = true;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x0002014A File Offset: 0x0001E34A
		// (set) Token: 0x0600094C RID: 2380 RVA: 0x00020152 File Offset: 0x0001E352
		public bool ReduceWidthWhenScrollbarVisible
		{
			get
			{
				return this._reduceWidthWhenScrollbarVisible;
			}
			set
			{
				this._reduceWidthWhenScrollbarVisible = value;
				this.isTransformDirty = true;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00020162 File Offset: 0x0001E362
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x0002016A File Offset: 0x0001E36A
		public ESleekScrollbarVisibility VerticalScrollbarVisibility { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00020173 File Offset: 0x0001E373
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x0002017B File Offset: 0x0001E37B
		public Vector2 ContentSizeOffset
		{
			get
			{
				return this._contentSizeOffset;
			}
			set
			{
				this._contentSizeOffset = value;
				this.isTransformDirty = true;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x0002018C File Offset: 0x0001E38C
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x00020200 File Offset: 0x0001E400
		public Vector2 NormalizedStateCenter
		{
			get
			{
				if (this.isTransformDirty)
				{
					this.UpdateDirtyTransform();
				}
				return new Vector2((this.state.x + this.drawRect.width * 0.5f) / this.contentRect.width, (this.state.y + this.drawRect.height * 0.5f) / this.contentRect.height);
			}
			set
			{
				if (this.isTransformDirty)
				{
					this.UpdateDirtyTransform();
				}
				this.state = new Vector2(value.x * this.contentRect.width - this.drawRect.width * 0.5f, value.y * this.contentRect.height - this.drawRect.height * 0.5f);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x0002026E File Offset: 0x0001E46E
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x00020276 File Offset: 0x0001E476
		public bool HandleScrollWheel { get; set; } = true;

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x0002027F File Offset: 0x0001E47F
		// (set) Token: 0x06000956 RID: 2390 RVA: 0x00020287 File Offset: 0x0001E487
		public SleekColor BackgroundColor { get; set; } = GlazierConst.DefaultScrollViewBackgroundColor;

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x00020290 File Offset: 0x0001E490
		// (set) Token: 0x06000958 RID: 2392 RVA: 0x00020298 File Offset: 0x0001E498
		public SleekColor ForegroundColor { get; set; } = GlazierConst.DefaultScrollViewForegroundColor;

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06000959 RID: 2393 RVA: 0x000202A4 File Offset: 0x0001E4A4
		// (remove) Token: 0x0600095A RID: 2394 RVA: 0x000202DC File Offset: 0x0001E4DC
		public event Action<Vector2> OnNormalizedValueChanged;

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x00020311 File Offset: 0x0001E511
		public float NormalizedVerticalPosition
		{
			get
			{
				if (this.isTransformDirty)
				{
					this.UpdateDirtyTransform();
				}
				return this.state.y / (this.contentRect.height - this.drawRect.height);
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x00020344 File Offset: 0x0001E544
		public float NormalizedViewportHeight
		{
			get
			{
				if (this.isTransformDirty)
				{
					this.UpdateDirtyTransform();
				}
				return this.drawRect.height / this.contentRect.height;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0002036B File Offset: 0x0001E56B
		// (set) Token: 0x0600095E RID: 2398 RVA: 0x00020373 File Offset: 0x0001E573
		public bool ContentUseManualLayout { get; set; } = true;

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x0002037C File Offset: 0x0001E57C
		// (set) Token: 0x06000960 RID: 2400 RVA: 0x00020384 File Offset: 0x0001E584
		public bool AlignContentToBottom { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x0002038D File Offset: 0x0001E58D
		// (set) Token: 0x06000962 RID: 2402 RVA: 0x00020395 File Offset: 0x0001E595
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

		// Token: 0x06000963 RID: 2403 RVA: 0x0002039E File Offset: 0x0001E59E
		public void ScrollToTop()
		{
			this.state = new Vector2(this.state.x, this.state.y);
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x000203C1 File Offset: 0x0001E5C1
		public void ScrollToBottom()
		{
			this.state = new Vector2(this.state.x, this.contentRect.height);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x000203E4 File Offset: 0x0001E5E4
		public override void OnGUI()
		{
			GUI.backgroundColor = this.BackgroundColor;
			Vector2 rhs = GUI.BeginScrollView(this.drawRect, this.state, this.viewRect);
			if (this.state != rhs)
			{
				this.state = rhs;
				if (this.OnNormalizedValueChanged != null)
				{
					Vector2 vector = new Vector2(this.state.x / (this.contentRect.width - this.drawRect.width), this.state.y / (this.contentRect.height - this.drawRect.height));
					this.OnNormalizedValueChanged.Invoke(vector);
				}
			}
			base.ChildrenOnGUI();
			GUI.EndScrollView(this.HandleScrollWheel);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x000204A0 File Offset: 0x0001E6A0
		protected override void TransformChildDrawPositionIntoParentSpace(ref Vector2 position)
		{
			position.x += this.drawRect.x;
			position.x -= this.state.x;
			position.y += this.drawRect.y;
			position.y -= this.state.y;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00020501 File Offset: 0x0001E701
		protected override Rect GetLayoutRect()
		{
			return this.contentRect;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0002050C File Offset: 0x0001E70C
		protected override void UpdateDirtyTransform()
		{
			base.UpdateDirtyTransform();
			float userInterfaceScale = GraphicsSettings.userInterfaceScale;
			this.contentRect.width = this.ContentSizeOffset.x * userInterfaceScale;
			this.contentRect.height = this.ContentSizeOffset.y * userInterfaceScale;
			if (this.ScaleContentToWidth)
			{
				this.contentRect.width = this.contentRect.width + this.drawRect.width * this.ContentScaleFactor;
			}
			if (this.ScaleContentToHeight)
			{
				this.contentRect.height = this.contentRect.height + this.drawRect.height * this.ContentScaleFactor;
			}
			bool flag = this.contentRect.height >= this.drawRect.height;
			if (flag && this.ReduceWidthWhenScrollbarVisible && this.ScaleContentToWidth)
			{
				this.contentRect.width = this.contentRect.width - 30f;
			}
			if (this.contentRect.width >= this.drawRect.width && this.ScaleContentToHeight)
			{
				this.contentRect.height = this.contentRect.height - 30f;
			}
			this.viewRect = this.contentRect;
			if (flag && !this.ReduceWidthWhenScrollbarVisible && this.ScaleContentToWidth)
			{
				this.viewRect.width = this.viewRect.width - 30f;
			}
		}

		// Token: 0x04000390 RID: 912
		private bool _scaleContentToWidth;

		// Token: 0x04000391 RID: 913
		private bool _scaleContentToHeight;

		// Token: 0x04000392 RID: 914
		private float _contentScaleFactor = 1f;

		// Token: 0x04000393 RID: 915
		private bool _reduceWidthWhenScrollbarVisible = true;

		// Token: 0x04000395 RID: 917
		private Vector2 _contentSizeOffset;

		// Token: 0x04000396 RID: 918
		private Vector2 state;

		// Token: 0x0400039D RID: 925
		private bool _isRaycastTarget = true;

		// Token: 0x0400039E RID: 926
		private Rect contentRect;

		// Token: 0x0400039F RID: 927
		private Rect viewRect;
	}
}
