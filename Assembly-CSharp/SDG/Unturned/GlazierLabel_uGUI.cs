using System;
using TMPro;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000181 RID: 385
	internal class GlazierLabel_uGUI : GlazierElementBase_uGUI, ISleekLabel, ISleekElement
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x000242DB File Offset: 0x000224DB
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x000242E8 File Offset: 0x000224E8
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

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x000242F6 File Offset: 0x000224F6
		// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x000242FE File Offset: 0x000224FE
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

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0002431D File Offset: 0x0002251D
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x00024325 File Offset: 0x00022525
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

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00024344 File Offset: 0x00022544
		// (set) Token: 0x06000ABC RID: 2748 RVA: 0x0002434C File Offset: 0x0002254C
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

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x0002436B File Offset: 0x0002256B
		// (set) Token: 0x06000ABE RID: 2750 RVA: 0x00024374 File Offset: 0x00022574
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

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x000243B7 File Offset: 0x000225B7
		// (set) Token: 0x06000AC0 RID: 2752 RVA: 0x000243BF File Offset: 0x000225BF
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
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x000243DE File Offset: 0x000225DE
		// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x000243EB File Offset: 0x000225EB
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

		// Token: 0x06000AC3 RID: 2755 RVA: 0x000243F9 File Offset: 0x000225F9
		private void PostConstructLabel()
		{
			this.TextAlignment = 4;
			this.FontSize = 2;
			this.TextContrastContext = 0;
			this.FontStyle = 0;
			this.AllowRichText = false;
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00024420 File Offset: 0x00022620
		protected override bool ReleaseIntoPool()
		{
			if (this.textComponent == null)
			{
				return false;
			}
			this.textComponent.enabled = false;
			GlazierLabel_uGUI.LabelPoolData labelPoolData = new GlazierLabel_uGUI.LabelPoolData();
			base.PopulateBasePoolData(labelPoolData);
			labelPoolData.textComponent = this.textComponent;
			this.textComponent = null;
			base.glazier.ReleaseLabelToPool(labelPoolData);
			return true;
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00024476 File Offset: 0x00022676
		protected override void EnableComponents()
		{
			this.textComponent.enabled = true;
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00024484 File Offset: 0x00022684
		public GlazierLabel_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00024498 File Offset: 0x00022698
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.textComponent = base.gameObject.AddComponent<TextMeshProUGUI>();
			this.textComponent.raycastTarget = false;
			this.textComponent.font = GlazierResources_uGUI.Font;
			this.textComponent.overflowMode = 3;
			this.textComponent.margin = GlazierConst_uGUI.DefaultTextMargin;
			this.textComponent.extraPadding = true;
			this.PostConstructLabel();
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0002450B File Offset: 0x0002270B
		public void ConstructFromLabelPool(GlazierLabel_uGUI.LabelPoolData poolData)
		{
			base.ConstructFromPool(poolData);
			this.textComponent = poolData.textComponent;
			this.textComponent.text = string.Empty;
			this.PostConstructLabel();
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00024536 File Offset: 0x00022736
		public override void SynchronizeColors()
		{
			this.textComponent.color = this._textColor;
		}

		// Token: 0x04000410 RID: 1040
		private FontStyle _fontStyle;

		// Token: 0x04000411 RID: 1041
		private TextAnchor _fontAlignment;

		// Token: 0x04000412 RID: 1042
		private ESleekFontSize _fontSize;

		// Token: 0x04000413 RID: 1043
		private ETextContrastContext _contrastContext;

		// Token: 0x04000414 RID: 1044
		private SleekColor _textColor = GlazierConst.DefaultLabelForegroundColor;

		// Token: 0x04000415 RID: 1045
		private TextMeshProUGUI textComponent;

		// Token: 0x0200087C RID: 2172
		public class LabelPoolData : GlazierElementBase_uGUI.PoolData
		{
			// Token: 0x04003196 RID: 12694
			public TextMeshProUGUI textComponent;
		}
	}
}
