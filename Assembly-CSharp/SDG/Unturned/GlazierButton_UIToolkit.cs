using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x02000193 RID: 403
	internal class GlazierButton_UIToolkit : GlazierElementBase_UIToolkit, ISleekButton, ISleekElement, ISleekLabel, ISleekWithTooltip
	{
		// Token: 0x170001EA RID: 490
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x00028399 File Offset: 0x00026599
		public override bool UseManualLayout
		{
			set
			{
				base.UseManualLayout = value;
				this.labelElement.style.position = (this._useManualLayout ? 1 : 0);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x000283C8 File Offset: 0x000265C8
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x000283D5 File Offset: 0x000265D5
		public string Text
		{
			get
			{
				return this.labelElement.text;
			}
			set
			{
				this.labelElement.text = value;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x000283E3 File Offset: 0x000265E3
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x000283EB File Offset: 0x000265EB
		public string TooltipText { get; set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x000283F4 File Offset: 0x000265F4
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x000283FC File Offset: 0x000265FC
		public FontStyle FontStyle
		{
			get
			{
				return this._fontStyle;
			}
			set
			{
				this._fontStyle = value;
				this.labelElement.style.unityFontStyleAndWeight = GlazierUtils_UIToolkit.GetFontStyle(this._fontStyle);
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x00028420 File Offset: 0x00026620
		// (set) Token: 0x06000BED RID: 3053 RVA: 0x00028428 File Offset: 0x00026628
		public TextAnchor TextAlignment
		{
			get
			{
				return this._fontAlignment;
			}
			set
			{
				this._fontAlignment = value;
				this.labelElement.style.unityTextAlign = GlazierUtils_UIToolkit.GetTextAlignment(this._fontAlignment);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0002844C File Offset: 0x0002664C
		// (set) Token: 0x06000BEF RID: 3055 RVA: 0x00028454 File Offset: 0x00026654
		public ESleekFontSize FontSize
		{
			get
			{
				return this._fontSize;
			}
			set
			{
				this._fontSize = value;
				this.labelElement.style.fontSize = GlazierUtils_UIToolkit.GetFontSize(this._fontSize);
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x00028478 File Offset: 0x00026678
		// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x00028480 File Offset: 0x00026680
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

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06000BF2 RID: 3058 RVA: 0x00028490 File Offset: 0x00026690
		// (remove) Token: 0x06000BF3 RID: 3059 RVA: 0x000284C8 File Offset: 0x000266C8
		public event ClickedButton OnClicked;

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x06000BF4 RID: 3060 RVA: 0x00028500 File Offset: 0x00026700
		// (remove) Token: 0x06000BF5 RID: 3061 RVA: 0x00028538 File Offset: 0x00026738
		public event ClickedButton OnRightClicked;

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0002856D File Offset: 0x0002676D
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x0002857A File Offset: 0x0002677A
		public bool IsClickable
		{
			get
			{
				return this.buttonElement.enabledSelf;
			}
			set
			{
				this.buttonElement.SetEnabled(value);
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x00028588 File Offset: 0x00026788
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x00028598 File Offset: 0x00026798
		public bool IsRaycastTarget
		{
			get
			{
				return this.buttonElement.pickingMode == 0;
			}
			set
			{
				this.buttonElement.pickingMode = (value ? 0 : 1);
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x000285AC File Offset: 0x000267AC
		// (set) Token: 0x06000BFB RID: 3067 RVA: 0x000285B4 File Offset: 0x000267B4
		public SleekColor TextColor
		{
			get
			{
				return this._textColor;
			}
			set
			{
				this._textColor = value;
				this.labelElement.style.color = this._textColor;
				this.SynchronizeTextContrast();
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x000285DE File Offset: 0x000267DE
		// (set) Token: 0x06000BFD RID: 3069 RVA: 0x000285EB File Offset: 0x000267EB
		public bool AllowRichText
		{
			get
			{
				return this.labelElement.enableRichText;
			}
			set
			{
				this.labelElement.enableRichText = value;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x000285F9 File Offset: 0x000267F9
		// (set) Token: 0x06000BFF RID: 3071 RVA: 0x00028601 File Offset: 0x00026801
		public SleekColor BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this._backgroundColor = value;
				this.buttonElement.style.unityBackgroundImageTintColor = this._backgroundColor;
			}
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00028628 File Offset: 0x00026828
		public GlazierButton_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
			this.buttonElement = new VisualElement();
			this.buttonElement.userData = this;
			this.buttonElement.AddToClassList("unturned-button");
			this.clickable = new Clickable(new Action<EventBase>(this.OnClickedWithEventInfo));
			GlazierUtils_UIToolkit.AddClickableActivators(this.clickable);
			VisualElementExtensions.AddManipulator(this.buttonElement, this.clickable);
			this.labelElement = new Label();
			this.labelElement.pickingMode = 1;
			this.labelElement.AddToClassList("unturned-box-label");
			this.labelElement.enableRichText = false;
			this.buttonElement.Add(this.labelElement);
			this.visualElement = this.buttonElement;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00028703 File Offset: 0x00026903
		internal override void SynchronizeColors()
		{
			this.labelElement.style.color = this._textColor;
			this.buttonElement.style.unityBackgroundImageTintColor = this._backgroundColor;
			this.SynchronizeTextContrast();
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00028741 File Offset: 0x00026941
		internal override bool GetTooltipParameters(out string tooltipText, out Color tooltipColor)
		{
			tooltipText = this.TooltipText;
			tooltipColor = this._textColor;
			return true;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0002875D File Offset: 0x0002695D
		private void SynchronizeTextContrast()
		{
			GlazierUtils_UIToolkit.ApplyTextContrast(this.labelElement.style, this._contrastContext, this._textColor.Get().a);
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00028788 File Offset: 0x00026988
		private void OnClickedWithEventInfo(EventBase eventBase)
		{
			IMouseEvent mouseEvent = eventBase as IMouseEvent;
			if (mouseEvent != null)
			{
				int button = mouseEvent.button;
				if (button != 0)
				{
					if (button != 1)
					{
						return;
					}
					ClickedButton onRightClicked = this.OnRightClicked;
					if (onRightClicked == null)
					{
						return;
					}
					onRightClicked.Invoke(this);
				}
				else
				{
					ClickedButton onClicked = this.OnClicked;
					if (onClicked == null)
					{
						return;
					}
					onClicked.Invoke(this);
					return;
				}
			}
		}

		// Token: 0x0400048C RID: 1164
		private FontStyle _fontStyle;

		// Token: 0x0400048D RID: 1165
		private TextAnchor _fontAlignment = 4;

		// Token: 0x0400048E RID: 1166
		private ESleekFontSize _fontSize;

		// Token: 0x0400048F RID: 1167
		private ETextContrastContext _contrastContext;

		// Token: 0x04000492 RID: 1170
		private SleekColor _textColor = GlazierConst.DefaultButtonForegroundColor;

		// Token: 0x04000493 RID: 1171
		private SleekColor _backgroundColor = GlazierConst.DefaultButtonBackgroundColor;

		// Token: 0x04000494 RID: 1172
		private VisualElement buttonElement;

		// Token: 0x04000495 RID: 1173
		private Clickable clickable;

		// Token: 0x04000496 RID: 1174
		private Label labelElement;
	}
}
