using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x02000192 RID: 402
	internal class GlazierBox_UIToolkit : GlazierElementBase_UIToolkit, ISleekBox, ISleekLabel, ISleekElement, ISleekWithTooltip
	{
		// Token: 0x170001E0 RID: 480
		// (set) Token: 0x06000BCE RID: 3022 RVA: 0x00028099 File Offset: 0x00026299
		public override bool UseManualLayout
		{
			set
			{
				base.UseManualLayout = value;
				this.labelElement.style.position = (this._useManualLayout ? 1 : 0);
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x000280C8 File Offset: 0x000262C8
		// (set) Token: 0x06000BD0 RID: 3024 RVA: 0x000280D8 File Offset: 0x000262D8
		public string Text
		{
			get
			{
				return this.labelElement.text;
			}
			set
			{
				this.labelElement.text = value;
				bool flag = !string.IsNullOrEmpty(value);
				this.labelElement.visible = flag;
				this.labelElement.style.visibility = (flag ? 0 : 1);
				this.labelElement.style.display = (flag ? 0 : 1);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x0002813F File Offset: 0x0002633F
		// (set) Token: 0x06000BD2 RID: 3026 RVA: 0x00028147 File Offset: 0x00026347
		public string TooltipText { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x00028150 File Offset: 0x00026350
		// (set) Token: 0x06000BD4 RID: 3028 RVA: 0x00028158 File Offset: 0x00026358
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

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x0002817C File Offset: 0x0002637C
		// (set) Token: 0x06000BD6 RID: 3030 RVA: 0x00028184 File Offset: 0x00026384
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

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x000281A8 File Offset: 0x000263A8
		// (set) Token: 0x06000BD8 RID: 3032 RVA: 0x000281B0 File Offset: 0x000263B0
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

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x000281D4 File Offset: 0x000263D4
		// (set) Token: 0x06000BDA RID: 3034 RVA: 0x000281DC File Offset: 0x000263DC
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

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x000281EB File Offset: 0x000263EB
		// (set) Token: 0x06000BDC RID: 3036 RVA: 0x000281F3 File Offset: 0x000263F3
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

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0002821D File Offset: 0x0002641D
		// (set) Token: 0x06000BDE RID: 3038 RVA: 0x0002822A File Offset: 0x0002642A
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

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x00028238 File Offset: 0x00026438
		// (set) Token: 0x06000BE0 RID: 3040 RVA: 0x00028240 File Offset: 0x00026440
		public SleekColor BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this._backgroundColor = value;
				this.boxElement.style.unityBackgroundImageTintColor = this._backgroundColor;
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00028264 File Offset: 0x00026464
		public GlazierBox_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
			this.boxElement = new VisualElement();
			this.boxElement.AddToClassList("unturned-box");
			this.boxElement.userData = this;
			this.labelElement = new Label();
			this.labelElement.pickingMode = 1;
			this.labelElement.AddToClassList("unturned-box-label");
			this.labelElement.enableRichText = false;
			this.boxElement.Add(this.labelElement);
			this.Text = string.Empty;
			this.visualElement = this.boxElement;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00028317 File Offset: 0x00026517
		internal override void SynchronizeColors()
		{
			this.labelElement.style.color = this._textColor;
			this.boxElement.style.unityBackgroundImageTintColor = this._backgroundColor;
			this.SynchronizeTextContrast();
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00028355 File Offset: 0x00026555
		internal override bool GetTooltipParameters(out string tooltipText, out Color tooltipColor)
		{
			tooltipText = this.TooltipText;
			tooltipColor = this._textColor;
			return true;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00028371 File Offset: 0x00026571
		private void SynchronizeTextContrast()
		{
			GlazierUtils_UIToolkit.ApplyTextContrast(this.labelElement.style, this._contrastContext, this._textColor.Get().a);
		}

		// Token: 0x04000483 RID: 1155
		private FontStyle _fontStyle;

		// Token: 0x04000484 RID: 1156
		private TextAnchor _fontAlignment = 4;

		// Token: 0x04000485 RID: 1157
		private ESleekFontSize _fontSize;

		// Token: 0x04000486 RID: 1158
		private ETextContrastContext _contrastContext;

		// Token: 0x04000487 RID: 1159
		private SleekColor _textColor = GlazierConst.DefaultBoxForegroundColor;

		// Token: 0x04000488 RID: 1160
		private SleekColor _backgroundColor = GlazierConst.DefaultBoxBackgroundColor;

		// Token: 0x04000489 RID: 1161
		private VisualElement boxElement;

		// Token: 0x0400048A RID: 1162
		private Label labelElement;
	}
}
