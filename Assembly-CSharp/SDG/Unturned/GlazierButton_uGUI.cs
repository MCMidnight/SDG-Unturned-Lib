using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000178 RID: 376
	internal class GlazierButton_uGUI : GlazierElementBase_uGUI, ISleekButton, ISleekElement, ISleekLabel, ISleekWithTooltip
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x000226A8 File Offset: 0x000208A8
		// (set) Token: 0x06000A25 RID: 2597 RVA: 0x000226B5 File Offset: 0x000208B5
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

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x000226C3 File Offset: 0x000208C3
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x000226E0 File Offset: 0x000208E0
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

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x0002272E File Offset: 0x0002092E
		// (set) Token: 0x06000A29 RID: 2601 RVA: 0x00022736 File Offset: 0x00020936
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

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x00022755 File Offset: 0x00020955
		// (set) Token: 0x06000A2B RID: 2603 RVA: 0x0002275D File Offset: 0x0002095D
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

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x0002277C File Offset: 0x0002097C
		// (set) Token: 0x06000A2D RID: 2605 RVA: 0x00022784 File Offset: 0x00020984
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

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x000227A3 File Offset: 0x000209A3
		// (set) Token: 0x06000A2F RID: 2607 RVA: 0x000227AC File Offset: 0x000209AC
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

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06000A30 RID: 2608 RVA: 0x000227F0 File Offset: 0x000209F0
		// (remove) Token: 0x06000A31 RID: 2609 RVA: 0x00022828 File Offset: 0x00020A28
		public event ClickedButton OnClicked;

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06000A32 RID: 2610 RVA: 0x00022860 File Offset: 0x00020A60
		// (remove) Token: 0x06000A33 RID: 2611 RVA: 0x00022898 File Offset: 0x00020A98
		public event ClickedButton OnRightClicked;

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x000228CD File Offset: 0x00020ACD
		// (set) Token: 0x06000A35 RID: 2613 RVA: 0x000228DA File Offset: 0x00020ADA
		public bool IsClickable
		{
			get
			{
				return this.buttonComponent.interactable;
			}
			set
			{
				this.buttonComponent.interactable = value;
				this.SynchronizeColors();
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x000228EE File Offset: 0x00020AEE
		// (set) Token: 0x06000A37 RID: 2615 RVA: 0x000228FB File Offset: 0x00020AFB
		public bool IsRaycastTarget
		{
			get
			{
				return this.imageComponent.raycastTarget;
			}
			set
			{
				this.imageComponent.raycastTarget = value;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00022909 File Offset: 0x00020B09
		// (set) Token: 0x06000A39 RID: 2617 RVA: 0x00022914 File Offset: 0x00020B14
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

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00022962 File Offset: 0x00020B62
		// (set) Token: 0x06000A3B RID: 2619 RVA: 0x0002296F File Offset: 0x00020B6F
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

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0002297D File Offset: 0x00020B7D
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x00022985 File Offset: 0x00020B85
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

		// Token: 0x06000A3E RID: 2622 RVA: 0x000229A4 File Offset: 0x00020BA4
		private void PostConstructButton()
		{
			this.TextAlignment = 4;
			this.FontSize = 2;
			this.TextContrastContext = 0;
			this.FontStyle = 0;
			this.AllowRichText = false;
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x000229CC File Offset: 0x00020BCC
		protected override bool ReleaseIntoPool()
		{
			if (this.imageComponent == null || this.buttonComponent == null || this.textComponent == null)
			{
				return false;
			}
			if (this.tooltipComponent != null)
			{
				Object.Destroy(this.tooltipComponent);
				this.tooltipComponent = null;
			}
			this.imageComponent.enabled = false;
			this.buttonComponent.enabled = false;
			this.textComponent.enabled = false;
			GlazierButton_uGUI.ButtonPoolData buttonPoolData = new GlazierButton_uGUI.ButtonPoolData();
			base.PopulateBasePoolData(buttonPoolData);
			buttonPoolData.imageComponent = this.imageComponent;
			this.imageComponent = null;
			buttonPoolData.buttonComponent = this.buttonComponent;
			this.buttonComponent = null;
			buttonPoolData.textComponent = this.textComponent;
			this.textComponent = null;
			base.glazier.ReleaseButtonToPool(buttonPoolData);
			return true;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00022A9C File Offset: 0x00020C9C
		protected override void EnableComponents()
		{
			this.imageComponent.enabled = true;
			this.buttonComponent.enabled = true;
			this.textComponent.enabled = true;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00022AC2 File Offset: 0x00020CC2
		public GlazierButton_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00022AE4 File Offset: 0x00020CE4
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.imageComponent = base.gameObject.AddComponent<Image>();
			this.imageComponent.enabled = false;
			this.imageComponent.type = 1;
			this.buttonComponent = base.gameObject.AddComponent<ButtonEx>();
			this.buttonComponent.enabled = false;
			this.buttonComponent.transition = 2;
			this.buttonComponent.onClick.AddListener(new UnityAction(this.OnUnityButtonClicked));
			this.buttonComponent.onRightClick.AddListener(new UnityAction(this.OnUnityButtonRightClicked));
			GameObject gameObject = new GameObject("ButtonText", new Type[]
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
			this.PostConstructButton();
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00022C24 File Offset: 0x00020E24
		public void ConstructFromButtonPool(GlazierButton_uGUI.ButtonPoolData poolData)
		{
			base.ConstructFromPool(poolData);
			this.imageComponent = poolData.imageComponent;
			this.buttonComponent = poolData.buttonComponent;
			this.textComponent = poolData.textComponent;
			this.imageComponent.raycastTarget = true;
			this.textComponent.text = string.Empty;
			this.textComponent.rectTransform.reset();
			this.buttonComponent.interactable = true;
			this.buttonComponent.onClick.RemoveAllListeners();
			this.buttonComponent.onRightClick.RemoveAllListeners();
			this.buttonComponent.onClick.AddListener(new UnityAction(this.OnUnityButtonClicked));
			this.buttonComponent.onRightClick.AddListener(new UnityAction(this.OnUnityButtonRightClicked));
			this.PostConstructButton();
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00022CF4 File Offset: 0x00020EF4
		public override void SynchronizeColors()
		{
			Color color = this._backgroundColor;
			if (!this.IsClickable)
			{
				color.a *= 0.25f;
			}
			this.imageComponent.color = color;
			this.textComponent.color = this.TextColor;
			if (this.tooltipComponent != null)
			{
				this.tooltipComponent.color = this._textColor;
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00022D6C File Offset: 0x00020F6C
		public override void SynchronizeTheme()
		{
			this.imageComponent.sprite = GlazierResources_uGUI.Theme.BoxSprite;
			SpriteState spriteState = default(SpriteState);
			spriteState.disabledSprite = this.imageComponent.sprite;
			spriteState.highlightedSprite = GlazierResources_uGUI.Theme.BoxHighlightedSprite;
			spriteState.selectedSprite = GlazierResources_uGUI.Theme.BoxSelectedSprite;
			spriteState.pressedSprite = GlazierResources_uGUI.Theme.BoxPressedSprite;
			this.buttonComponent.spriteState = spriteState;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00022DF6 File Offset: 0x00020FF6
		private void OnUnityButtonClicked()
		{
			ClickedButton onClicked = this.OnClicked;
			if (onClicked == null)
			{
				return;
			}
			onClicked.Invoke(this);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00022E09 File Offset: 0x00021009
		private void OnUnityButtonRightClicked()
		{
			ClickedButton onRightClicked = this.OnRightClicked;
			if (onRightClicked == null)
			{
				return;
			}
			onRightClicked.Invoke(this);
		}

		// Token: 0x040003E7 RID: 999
		private GlazieruGUITooltip tooltipComponent;

		// Token: 0x040003E8 RID: 1000
		private FontStyle _fontStyle;

		// Token: 0x040003E9 RID: 1001
		private TextAnchor _fontAlignment;

		// Token: 0x040003EA RID: 1002
		private ESleekFontSize _fontSize;

		// Token: 0x040003EB RID: 1003
		private ETextContrastContext _contrastContext;

		// Token: 0x040003EE RID: 1006
		private SleekColor _textColor = GlazierConst.DefaultButtonForegroundColor;

		// Token: 0x040003EF RID: 1007
		private SleekColor _backgroundColor = GlazierConst.DefaultButtonBackgroundColor;

		// Token: 0x040003F0 RID: 1008
		private Image imageComponent;

		// Token: 0x040003F1 RID: 1009
		private ButtonEx buttonComponent;

		// Token: 0x040003F2 RID: 1010
		private TextMeshProUGUI textComponent;

		// Token: 0x02000879 RID: 2169
		public class ButtonPoolData : GlazierElementBase_uGUI.PoolData
		{
			// Token: 0x04003190 RID: 12688
			public Image imageComponent;

			// Token: 0x04003191 RID: 12689
			public ButtonEx buttonComponent;

			// Token: 0x04003192 RID: 12690
			public TextMeshProUGUI textComponent;
		}
	}
}
