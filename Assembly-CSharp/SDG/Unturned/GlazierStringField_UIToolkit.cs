using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x020001A4 RID: 420
	internal class GlazierStringField_UIToolkit : GlazierElementBase_UIToolkit, ISleekField, ISleekElement, ISleekLabel, ISleekWithTooltip
	{
		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06000CCE RID: 3278 RVA: 0x0002B108 File Offset: 0x00029308
		// (remove) Token: 0x06000CCF RID: 3279 RVA: 0x0002B140 File Offset: 0x00029340
		public event Entered OnTextSubmitted;

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06000CD0 RID: 3280 RVA: 0x0002B178 File Offset: 0x00029378
		// (remove) Token: 0x06000CD1 RID: 3281 RVA: 0x0002B1B0 File Offset: 0x000293B0
		public event Typed OnTextChanged;

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06000CD2 RID: 3282 RVA: 0x0002B1E8 File Offset: 0x000293E8
		// (remove) Token: 0x06000CD3 RID: 3283 RVA: 0x0002B220 File Offset: 0x00029420
		public event Escaped OnTextEscaped;

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x0002B255 File Offset: 0x00029455
		// (set) Token: 0x06000CD5 RID: 3285 RVA: 0x0002B262 File Offset: 0x00029462
		public bool IsPasswordField
		{
			get
			{
				return this.control.isPasswordField;
			}
			set
			{
				this.control.isPasswordField = value;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x0002B270 File Offset: 0x00029470
		// (set) Token: 0x06000CD7 RID: 3287 RVA: 0x0002B27D File Offset: 0x0002947D
		public string PlaceholderText
		{
			get
			{
				return this.placeholderLabel.text;
			}
			set
			{
				this.placeholderLabel.text = value;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x0002B28B File Offset: 0x0002948B
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x0002B298 File Offset: 0x00029498
		public bool IsMultiline
		{
			get
			{
				return this.control.multiline;
			}
			set
			{
				this.control.multiline = value;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0002B2A6 File Offset: 0x000294A6
		// (set) Token: 0x06000CDB RID: 3291 RVA: 0x0002B2B3 File Offset: 0x000294B3
		public string Text
		{
			get
			{
				return this.control.text;
			}
			set
			{
				this.control.SetValueWithoutNotify(value);
				this.SynchronizePlaceholderVisible();
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x0002B2C7 File Offset: 0x000294C7
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x0002B2CF File Offset: 0x000294CF
		public string TooltipText { get; set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x0002B2D8 File Offset: 0x000294D8
		// (set) Token: 0x06000CDF RID: 3295 RVA: 0x0002B2E0 File Offset: 0x000294E0
		public FontStyle FontStyle
		{
			get
			{
				return this._fontStyle;
			}
			set
			{
				this._fontStyle = value;
				this.inputElement.style.unityFontStyleAndWeight = GlazierUtils_UIToolkit.GetFontStyle(this._fontStyle);
				this.placeholderLabel.style.unityFontStyleAndWeight = GlazierUtils_UIToolkit.GetFontStyle(this._fontStyle);
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x0002B31F File Offset: 0x0002951F
		// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x0002B327 File Offset: 0x00029527
		public TextAnchor TextAlignment
		{
			get
			{
				return this._fontAlignment;
			}
			set
			{
				this._fontAlignment = value;
				this.inputElement.style.unityTextAlign = GlazierUtils_UIToolkit.GetTextAlignment(this._fontAlignment);
				this.placeholderLabel.style.unityTextAlign = GlazierUtils_UIToolkit.GetTextAlignment(this._fontAlignment);
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0002B366 File Offset: 0x00029566
		// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x0002B370 File Offset: 0x00029570
		public ESleekFontSize FontSize
		{
			get
			{
				return this._fontSize;
			}
			set
			{
				this._fontSize = value;
				StyleLength fontSize = GlazierUtils_UIToolkit.GetFontSize(this._fontSize);
				this.inputElement.style.fontSize = fontSize;
				this.placeholderLabel.style.fontSize = fontSize;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0002B3B2 File Offset: 0x000295B2
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x0002B3BA File Offset: 0x000295BA
		public ETextContrastContext TextContrastContext
		{
			get
			{
				return this._contrastContext;
			}
			set
			{
				this._contrastContext = value;
				this.SynchronizeTextContrast();
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0002B3C9 File Offset: 0x000295C9
		// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x0002B3D4 File Offset: 0x000295D4
		public SleekColor TextColor
		{
			get
			{
				return this._textColor;
			}
			set
			{
				this._textColor = value;
				Color color = this._textColor;
				this.inputElement.style.color = color;
				this.placeholderLabel.style.color = color * 0.5f;
				this.SynchronizeTextContrast();
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x0002B430 File Offset: 0x00029630
		// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x0002B433 File Offset: 0x00029633
		public bool AllowRichText
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x0002B435 File Offset: 0x00029635
		// (set) Token: 0x06000CEB RID: 3307 RVA: 0x0002B43D File Offset: 0x0002963D
		public SleekColor BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this._backgroundColor = value;
				this.inputElement.style.unityBackgroundImageTintColor = this._backgroundColor;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0002B461 File Offset: 0x00029661
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x0002B46E File Offset: 0x0002966E
		public bool IsClickable
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

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x0002B47C File Offset: 0x0002967C
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x0002B489 File Offset: 0x00029689
		public int MaxLength
		{
			get
			{
				return this.control.maxLength;
			}
			set
			{
				this.control.maxLength = value;
			}
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0002B497 File Offset: 0x00029697
		public void FocusControl()
		{
			if (this.control.focusController.focusedElement != this.control)
			{
				this.control.Focus();
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0002B4BC File Offset: 0x000296BC
		public void ClearFocus()
		{
			this.control.Blur();
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0002B4CC File Offset: 0x000296CC
		public GlazierStringField_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
			this.control = new TextField();
			this.control.userData = this;
			this.control.AddToClassList("unturned-field");
			INotifyValueChangedExtensions.RegisterValueChangedCallback<string>(this.control, new EventCallback<ChangeEvent<string>>(this.OnControlValueChanged));
			this.control.RegisterCallback<KeyUpEvent>(new EventCallback<KeyUpEvent>(this.OnControlKeyUp), 0);
			this.control.maxLength = 100;
			this.inputElement = UQueryExtensions.Q(this.control, null, TextField.inputUssClassName);
			this.placeholderLabel = new Label();
			this.placeholderLabel.AddToClassList("unturned-field__placeholder");
			this.placeholderLabel.pickingMode = 1;
			this.control.Add(this.placeholderLabel);
			this.visualElement = this.control;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0002B5BD File Offset: 0x000297BD
		protected virtual void OnControlValueChanged(ChangeEvent<string> changeEvent)
		{
			Typed onTextChanged = this.OnTextChanged;
			if (onTextChanged != null)
			{
				onTextChanged.Invoke(this, changeEvent.newValue);
			}
			this.SynchronizePlaceholderVisible();
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0002B5DD File Offset: 0x000297DD
		protected virtual void OnSubmitted()
		{
			Entered onTextSubmitted = this.OnTextSubmitted;
			if (onTextSubmitted != null)
			{
				onTextSubmitted.Invoke(this);
			}
			this.SynchronizePlaceholderVisible();
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0002B5F8 File Offset: 0x000297F8
		internal override void SynchronizeColors()
		{
			Color color = this._textColor;
			this.inputElement.style.color = color;
			this.inputElement.style.unityBackgroundImageTintColor = this._backgroundColor;
			this.placeholderLabel.style.color = color * 0.5f;
			this.SynchronizeTextContrast();
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0002B668 File Offset: 0x00029868
		internal override bool GetTooltipParameters(out string tooltipText, out Color tooltipColor)
		{
			tooltipText = this.TooltipText;
			tooltipColor = this._textColor;
			return true;
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0002B684 File Offset: 0x00029884
		private void SynchronizePlaceholderVisible()
		{
			this.placeholderLabel.visible = string.IsNullOrEmpty(this.control.text);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0002B6A4 File Offset: 0x000298A4
		private void SynchronizeTextContrast()
		{
			float a = this._textColor.Get().a;
			GlazierUtils_UIToolkit.ApplyTextContrast(this.inputElement.style, this._contrastContext, a);
			GlazierUtils_UIToolkit.ApplyTextContrast(this.placeholderLabel.style, this._contrastContext, a);
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0002B6F0 File Offset: 0x000298F0
		private void OnControlKeyUp(KeyUpEvent keyUpEvent)
		{
			if (keyUpEvent.keyCode != KeyCode.Escape)
			{
				if ((keyUpEvent.keyCode == KeyCode.Return || keyUpEvent.keyCode == KeyCode.KeypadEnter) && !this.IsMultiline)
				{
					this.OnSubmitted();
					this.control.Blur();
				}
				return;
			}
			this.control.Blur();
			Escaped onTextEscaped = this.OnTextEscaped;
			if (onTextEscaped == null)
			{
				return;
			}
			onTextEscaped.Invoke(this);
		}

		// Token: 0x040004E7 RID: 1255
		private FontStyle _fontStyle;

		// Token: 0x040004E8 RID: 1256
		private TextAnchor _fontAlignment = 4;

		// Token: 0x040004E9 RID: 1257
		private ESleekFontSize _fontSize;

		// Token: 0x040004EA RID: 1258
		private ETextContrastContext _contrastContext;

		// Token: 0x040004EB RID: 1259
		private SleekColor _textColor = GlazierConst.DefaultFieldForegroundColor;

		// Token: 0x040004EC RID: 1260
		private SleekColor _backgroundColor = GlazierConst.DefaultFieldBackgroundColor;

		// Token: 0x040004ED RID: 1261
		private TextField control;

		// Token: 0x040004EE RID: 1262
		private Label placeholderLabel;

		// Token: 0x040004EF RID: 1263
		private VisualElement inputElement;
	}
}
