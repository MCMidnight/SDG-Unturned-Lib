using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000176 RID: 374
	internal class GlazierBox_uGUI : GlazierElementBase_uGUI, ISleekBox, ISleekLabel, ISleekElement, ISleekWithTooltip
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x00022172 File Offset: 0x00020372
		// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0002217F File Offset: 0x0002037F
		public string Text
		{
			get
			{
				return this.textComponent.text;
			}
			set
			{
				this.textComponent.text = value;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0002218D File Offset: 0x0002038D
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x000221AC File Offset: 0x000203AC
		public string TooltipText
		{
			get
			{
				if (!(this.tooltipComponent != null))
				{
					return null;
				}
				return this.tooltipComponent.text;
			}
			set
			{
				if (this.tooltipComponent == null)
				{
					this.tooltipComponent = base.gameObject.AddComponent<GlazieruGUITooltip>();
					this.tooltipComponent.color = this._textColor;
				}
				this.tooltipComponent.text = value;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x000221FA File Offset: 0x000203FA
		// (set) Token: 0x06000A0B RID: 2571 RVA: 0x00022202 File Offset: 0x00020402
		public FontStyle FontStyle
		{
			get
			{
				return this._fontStyle;
			}
			set
			{
				this._fontStyle = value;
				this.textComponent.fontStyle = GlazierUtils_uGUI.GetFontStyleFlags(this._fontStyle);
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x00022221 File Offset: 0x00020421
		// (set) Token: 0x06000A0D RID: 2573 RVA: 0x00022229 File Offset: 0x00020429
		public TextAnchor TextAlignment
		{
			get
			{
				return this._fontAlignment;
			}
			set
			{
				this._fontAlignment = value;
				this.textComponent.alignment = GlazierUtils_uGUI.TextAnchorToTMP(this._fontAlignment);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x00022248 File Offset: 0x00020448
		// (set) Token: 0x06000A0F RID: 2575 RVA: 0x00022250 File Offset: 0x00020450
		public ESleekFontSize FontSize
		{
			get
			{
				return this._fontSize;
			}
			set
			{
				this._fontSize = value;
				this.textComponent.fontSize = GlazierUtils_uGUI.GetFontSize(this._fontSize);
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0002226F File Offset: 0x0002046F
		// (set) Token: 0x06000A11 RID: 2577 RVA: 0x00022278 File Offset: 0x00020478
		public ETextContrastContext TextContrastContext
		{
			get
			{
				return this._contrastContext;
			}
			set
			{
				this._contrastContext = value;
				ETextContrastStyle shadowStyle = SleekShadowStyle.ContextToStyle(value);
				this.textComponent.fontSharedMaterial = base.glazier.GetFontMaterial(shadowStyle);
				this.textComponent.characterSpacing = GlazierUtils_uGUI.GetCharacterSpacing(shadowStyle);
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x000222BB File Offset: 0x000204BB
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x000222C4 File Offset: 0x000204C4
		public SleekColor TextColor
		{
			get
			{
				return this._textColor;
			}
			set
			{
				this._textColor = value;
				this.textComponent.color = this._textColor;
				if (this.tooltipComponent != null)
				{
					this.tooltipComponent.color = this._textColor;
				}
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00022312 File Offset: 0x00020512
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x0002231F File Offset: 0x0002051F
		public bool AllowRichText
		{
			get
			{
				return this.textComponent.richText;
			}
			set
			{
				this.textComponent.richText = value;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x0002232D File Offset: 0x0002052D
		// (set) Token: 0x06000A17 RID: 2583 RVA: 0x00022335 File Offset: 0x00020535
		public SleekColor BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this._backgroundColor = value;
				this.imageComponent.color = this._backgroundColor;
			}
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00022354 File Offset: 0x00020554
		private void PostConstructBox()
		{
			this.TextAlignment = 4;
			this.FontSize = 2;
			this.TextContrastContext = 0;
			this.FontStyle = 0;
			this.AllowRichText = false;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0002237C File Offset: 0x0002057C
		protected override bool ReleaseIntoPool()
		{
			if (this.imageComponent == null || this.textComponent == null)
			{
				return false;
			}
			if (this.tooltipComponent != null)
			{
				Object.Destroy(this.tooltipComponent);
				this.tooltipComponent = null;
			}
			this.imageComponent.enabled = false;
			this.textComponent.enabled = false;
			GlazierBox_uGUI.BoxPoolData boxPoolData = new GlazierBox_uGUI.BoxPoolData();
			base.PopulateBasePoolData(boxPoolData);
			boxPoolData.imageComponent = this.imageComponent;
			this.imageComponent = null;
			boxPoolData.textComponent = this.textComponent;
			this.textComponent = null;
			base.glazier.ReleaseBoxToPool(boxPoolData);
			return true;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0002241F File Offset: 0x0002061F
		protected override void EnableComponents()
		{
			this.imageComponent.enabled = true;
			this.textComponent.enabled = true;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00022439 File Offset: 0x00020639
		public GlazierBox_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00022458 File Offset: 0x00020658
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.imageComponent = base.gameObject.AddComponent<Image>();
			this.imageComponent.enabled = false;
			this.imageComponent.type = 1;
			this.imageComponent.raycastTarget = true;
			GameObject gameObject = new GameObject("BoxText", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.transform.SetParent(base.transform, false);
			gameObject.GetRectTransform().reset();
			this.textComponent = gameObject.AddComponent<TextMeshProUGUI>();
			this.textComponent.enabled = false;
			this.textComponent.raycastTarget = false;
			this.textComponent.font = GlazierResources_uGUI.Font;
			this.textComponent.overflowMode = 3;
			this.textComponent.margin = GlazierConst_uGUI.DefaultTextMargin;
			this.textComponent.extraPadding = true;
			this.PostConstructBox();
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00022544 File Offset: 0x00020744
		public void ConstructFromBoxPool(GlazierBox_uGUI.BoxPoolData poolData)
		{
			base.ConstructFromPool(poolData);
			this.imageComponent = poolData.imageComponent;
			this.textComponent = poolData.textComponent;
			this.textComponent.rectTransform.reset();
			this.textComponent.text = string.Empty;
			this.PostConstructBox();
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00022598 File Offset: 0x00020798
		public override void SynchronizeColors()
		{
			this.imageComponent.color = this._backgroundColor;
			this.textComponent.color = this._textColor;
			if (this.tooltipComponent != null)
			{
				this.tooltipComponent.color = this._textColor;
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x000225F5 File Offset: 0x000207F5
		public override void SynchronizeTheme()
		{
			this.imageComponent.sprite = GlazierResources_uGUI.Theme.BoxSprite;
		}

		// Token: 0x040003DD RID: 989
		private GlazieruGUITooltip tooltipComponent;

		// Token: 0x040003DE RID: 990
		private FontStyle _fontStyle;

		// Token: 0x040003DF RID: 991
		private TextAnchor _fontAlignment;

		// Token: 0x040003E0 RID: 992
		private ESleekFontSize _fontSize;

		// Token: 0x040003E1 RID: 993
		private ETextContrastContext _contrastContext;

		// Token: 0x040003E2 RID: 994
		private SleekColor _textColor = GlazierConst.DefaultBoxForegroundColor;

		// Token: 0x040003E3 RID: 995
		private SleekColor _backgroundColor = GlazierConst.DefaultBoxBackgroundColor;

		// Token: 0x040003E4 RID: 996
		private Image imageComponent;

		// Token: 0x040003E5 RID: 997
		private TextMeshProUGUI textComponent;

		// Token: 0x02000878 RID: 2168
		public class BoxPoolData : GlazierElementBase_uGUI.PoolData
		{
			// Token: 0x0400318E RID: 12686
			public Image imageComponent;

			// Token: 0x0400318F RID: 12687
			public TextMeshProUGUI textComponent;
		}
	}
}
