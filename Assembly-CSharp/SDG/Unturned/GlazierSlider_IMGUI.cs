using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200016D RID: 365
	internal class GlazierSlider_IMGUI : GlazierElementBase_IMGUI, ISleekSlider, ISleekElement
	{
		// Token: 0x14000039 RID: 57
		// (add) Token: 0x0600096A RID: 2410 RVA: 0x000206B4 File Offset: 0x0001E8B4
		// (remove) Token: 0x0600096B RID: 2411 RVA: 0x000206EC File Offset: 0x0001E8EC
		public event Dragged OnValueChanged;

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x00020721 File Offset: 0x0001E921
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x00020729 File Offset: 0x0001E929
		public ESleekOrientation Orientation { get; set; } = 1;

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x00020732 File Offset: 0x0001E932
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x0002073A File Offset: 0x0001E93A
		public float Value
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				this.scroll = this.Value * 0.75f;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x00020755 File Offset: 0x0001E955
		// (set) Token: 0x06000971 RID: 2417 RVA: 0x0002075D File Offset: 0x0001E95D
		public SleekColor BackgroundColor { get; set; } = GlazierConst.DefaultSliderBackgroundColor;

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x00020766 File Offset: 0x0001E966
		// (set) Token: 0x06000973 RID: 2419 RVA: 0x0002076E File Offset: 0x0001E96E
		public SleekColor ForegroundColor { get; set; } = GlazierConst.DefaultSliderForegroundColor;

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x00020777 File Offset: 0x0001E977
		// (set) Token: 0x06000975 RID: 2421 RVA: 0x0002077F File Offset: 0x0001E97F
		public bool IsInteractable { get; set; } = true;

		// Token: 0x06000976 RID: 2422 RVA: 0x00020788 File Offset: 0x0001E988
		public override void OnGUI()
		{
			bool enabled = GUI.enabled;
			GUI.enabled = this.IsInteractable;
			float num = GlazierUtils_IMGUI.drawSlider(this.drawRect, this.Orientation, this.scroll, 0.25f, this.BackgroundColor);
			GUI.enabled = enabled;
			if (num != this.scroll)
			{
				this._state = num / 0.75f;
				if (this.Value < 0f)
				{
					this.Value = 0f;
				}
				else if (this.Value > 1f)
				{
					this.Value = 1f;
				}
				Dragged onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged.Invoke(this, this.Value);
				}
			}
			this.scroll = num;
			base.ChildrenOnGUI();
		}

		// Token: 0x040003A2 RID: 930
		private const float NormalizedHandleSize = 0.25f;

		// Token: 0x040003A3 RID: 931
		private float scroll;

		// Token: 0x040003A4 RID: 932
		private float _state;
	}
}
