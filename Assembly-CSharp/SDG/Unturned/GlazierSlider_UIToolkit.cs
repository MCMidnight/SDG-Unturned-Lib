using System;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x020001A2 RID: 418
	internal class GlazierSlider_UIToolkit : GlazierElementBase_UIToolkit, ISleekSlider, ISleekElement
	{
		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06000CA9 RID: 3241 RVA: 0x0002A7E8 File Offset: 0x000289E8
		// (remove) Token: 0x06000CAA RID: 3242 RVA: 0x0002A820 File Offset: 0x00028A20
		public event Dragged OnValueChanged;

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x0002A855 File Offset: 0x00028A55
		// (set) Token: 0x06000CAC RID: 3244 RVA: 0x0002A85D File Offset: 0x00028A5D
		public ESleekOrientation Orientation
		{
			get
			{
				return this._orientation;
			}
			set
			{
				if (this._orientation != value)
				{
					this._orientation = value;
					this.UpdateOrientation();
				}
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x0002A875 File Offset: 0x00028A75
		// (set) Token: 0x06000CAE RID: 3246 RVA: 0x0002A882 File Offset: 0x00028A82
		public float Value
		{
			get
			{
				return this.control.value;
			}
			set
			{
				this.control.SetValueWithoutNotify(value);
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x0002A890 File Offset: 0x00028A90
		// (set) Token: 0x06000CB0 RID: 3248 RVA: 0x0002A898 File Offset: 0x00028A98
		public SleekColor BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this._backgroundColor = value;
				this.trackerElement.style.unityBackgroundImageTintColor = this._backgroundColor;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000CB1 RID: 3249 RVA: 0x0002A8BC File Offset: 0x00028ABC
		// (set) Token: 0x06000CB2 RID: 3250 RVA: 0x0002A8C4 File Offset: 0x00028AC4
		public SleekColor ForegroundColor
		{
			get
			{
				return this._foregroundColor;
			}
			set
			{
				this._foregroundColor = value;
				this.draggerElement.style.unityBackgroundImageTintColor = this._foregroundColor;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x0002A8E8 File Offset: 0x00028AE8
		// (set) Token: 0x06000CB4 RID: 3252 RVA: 0x0002A8F5 File Offset: 0x00028AF5
		public bool IsInteractable
		{
			get
			{
				return this.control.enabledSelf;
			}
			set
			{
				this.control.SetEnabled(value);
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0002A904 File Offset: 0x00028B04
		public GlazierSlider_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
			this.control = new Slider();
			this.control.userData = this;
			this.control.AddToClassList("unturned-slider");
			this.control.lowValue = 0f;
			this.control.highValue = 1f;
			INotifyValueChangedExtensions.RegisterValueChangedCallback<float>(this.control, new EventCallback<ChangeEvent<float>>(this.OnControlValueChanged));
			this.UpdateOrientation();
			VisualElement visualElement = UQueryExtensions.Q(UQueryExtensions.Q(this.control, null, "unity-base-slider__input"), null, "unity-base-slider__drag-container");
			this.trackerElement = UQueryExtensions.Q(visualElement, null, "unity-base-slider__tracker");
			this.draggerElement = UQueryExtensions.Q(visualElement, null, "unity-base-slider__dragger");
			this.visualElement = this.control;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0002A9E7 File Offset: 0x00028BE7
		internal override void SynchronizeColors()
		{
			this.trackerElement.style.unityBackgroundImageTintColor = this._backgroundColor;
			this.draggerElement.style.unityBackgroundImageTintColor = this._foregroundColor;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0002AA20 File Offset: 0x00028C20
		private void UpdateOrientation()
		{
			ESleekOrientation orientation = this._orientation;
			if (orientation == null)
			{
				this.control.direction = 0;
				this.control.inverted = false;
				return;
			}
			if (orientation != 1)
			{
				return;
			}
			this.control.direction = 1;
			this.control.inverted = true;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0002AA6D File Offset: 0x00028C6D
		private void OnControlValueChanged(ChangeEvent<float> changeEvent)
		{
			Dragged onValueChanged = this.OnValueChanged;
			if (onValueChanged == null)
			{
				return;
			}
			onValueChanged.Invoke(this, changeEvent.newValue);
		}

		// Token: 0x040004D4 RID: 1236
		private ESleekOrientation _orientation = 1;

		// Token: 0x040004D5 RID: 1237
		private SleekColor _backgroundColor = GlazierConst.DefaultSliderBackgroundColor;

		// Token: 0x040004D6 RID: 1238
		private SleekColor _foregroundColor = GlazierConst.DefaultSliderForegroundColor;

		// Token: 0x040004D7 RID: 1239
		private Slider control;

		// Token: 0x040004D8 RID: 1240
		private VisualElement trackerElement;

		// Token: 0x040004D9 RID: 1241
		private VisualElement draggerElement;
	}
}
