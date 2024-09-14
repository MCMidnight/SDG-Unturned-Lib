using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x0200019D RID: 413
	internal class GlazierLabel_UIToolkit : GlazierElementBase_UIToolkit, ISleekLabel, ISleekElement
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x00029C77 File Offset: 0x00027E77
		// (set) Token: 0x06000C5E RID: 3166 RVA: 0x00029C7F File Offset: 0x00027E7F
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				this.labelElement.text = this._text;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x00029C99 File Offset: 0x00027E99
		// (set) Token: 0x06000C60 RID: 3168 RVA: 0x00029CA1 File Offset: 0x00027EA1
		public string tooltipText
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				this._tooltip = value;
				this.labelElement.tooltip = this._tooltip;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x00029CBB File Offset: 0x00027EBB
		// (set) Token: 0x06000C62 RID: 3170 RVA: 0x00029CC3 File Offset: 0x00027EC3
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

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x00029CE7 File Offset: 0x00027EE7
		// (set) Token: 0x06000C64 RID: 3172 RVA: 0x00029CEF File Offset: 0x00027EEF
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x00029D13 File Offset: 0x00027F13
		// (set) Token: 0x06000C66 RID: 3174 RVA: 0x00029D1B File Offset: 0x00027F1B
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

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00029D3F File Offset: 0x00027F3F
		// (set) Token: 0x06000C68 RID: 3176 RVA: 0x00029D47 File Offset: 0x00027F47
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

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x00029D56 File Offset: 0x00027F56
		// (set) Token: 0x06000C6A RID: 3178 RVA: 0x00029D5E File Offset: 0x00027F5E
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

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00029D88 File Offset: 0x00027F88
		// (set) Token: 0x06000C6C RID: 3180 RVA: 0x00029D95 File Offset: 0x00027F95
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

		// Token: 0x17000213 RID: 531
		// (set) Token: 0x06000C6D RID: 3181 RVA: 0x00029DA3 File Offset: 0x00027FA3
		public override bool UseManualLayout
		{
			set
			{
				base.UseManualLayout = value;
				this.labelElement.style.position = (this._useManualLayout ? 1 : 0);
			}
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00029DD4 File Offset: 0x00027FD4
		public GlazierLabel_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
			this.containerElement = new VisualElement();
			this.containerElement.userData = this;
			this.containerElement.AddToClassList("unturned-label");
			this.containerElement.pickingMode = 1;
			this.labelElement = new Label();
			this.labelElement.AddToClassList("unturned-box-label");
			this.labelElement.pickingMode = 1;
			this.labelElement.enableRichText = false;
			this.containerElement.Add(this.labelElement);
			this.visualElement = this.containerElement;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00029E93 File Offset: 0x00028093
		internal override void SynchronizeColors()
		{
			this.labelElement.style.color = this._textColor;
			this.SynchronizeTextContrast();
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00029EB6 File Offset: 0x000280B6
		private void SynchronizeTextContrast()
		{
			GlazierUtils_UIToolkit.ApplyTextContrast(this.labelElement.style, this._contrastContext, this._textColor.Get().a);
		}

		// Token: 0x040004B1 RID: 1201
		private string _text = string.Empty;

		// Token: 0x040004B2 RID: 1202
		private string _tooltip = string.Empty;

		// Token: 0x040004B3 RID: 1203
		private FontStyle _fontStyle;

		// Token: 0x040004B4 RID: 1204
		private TextAnchor _fontAlignment = 4;

		// Token: 0x040004B5 RID: 1205
		private ESleekFontSize _fontSize;

		// Token: 0x040004B6 RID: 1206
		private ETextContrastContext _contrastContext;

		// Token: 0x040004B7 RID: 1207
		private SleekColor _textColor = GlazierConst.DefaultLabelForegroundColor;

		// Token: 0x040004B8 RID: 1208
		private VisualElement containerElement;

		// Token: 0x040004B9 RID: 1209
		private Label labelElement;
	}
}
