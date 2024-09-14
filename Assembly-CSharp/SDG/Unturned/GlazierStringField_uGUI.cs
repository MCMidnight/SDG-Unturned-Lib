using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x0200018A RID: 394
	internal class GlazierStringField_uGUI : GlazierElementBase_uGUI, ISleekField, ISleekElement, ISleekLabel, ISleekWithTooltip
	{
		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06000B35 RID: 2869 RVA: 0x00025CA8 File Offset: 0x00023EA8
		// (remove) Token: 0x06000B36 RID: 2870 RVA: 0x00025CE0 File Offset: 0x00023EE0
		public event Entered OnTextSubmitted;

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06000B37 RID: 2871 RVA: 0x00025D18 File Offset: 0x00023F18
		// (remove) Token: 0x06000B38 RID: 2872 RVA: 0x00025D50 File Offset: 0x00023F50
		public event Typed OnTextChanged;

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x06000B39 RID: 2873 RVA: 0x00025D88 File Offset: 0x00023F88
		// (remove) Token: 0x06000B3A RID: 2874 RVA: 0x00025DC0 File Offset: 0x00023FC0
		public event Escaped OnTextEscaped;

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x00025DF5 File Offset: 0x00023FF5
		// (set) Token: 0x06000B3C RID: 2876 RVA: 0x00025E05 File Offset: 0x00024005
		public bool IsPasswordField
		{
			get
			{
				return this.fieldComponent.contentType == 7;
			}
			set
			{
				this.fieldComponent.contentType = (value ? 7 : 0);
				this.fieldComponent.ForceLabelUpdate();
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00025E24 File Offset: 0x00024024
		// (set) Token: 0x06000B3E RID: 2878 RVA: 0x00025E31 File Offset: 0x00024031
		public string PlaceholderText
		{
			get
			{
				return this.placeholderComponent.text;
			}
			set
			{
				this.placeholderComponent.text = value;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00025E3F File Offset: 0x0002403F
		// (set) Token: 0x06000B40 RID: 2880 RVA: 0x00025E4F File Offset: 0x0002404F
		public bool IsMultiline
		{
			get
			{
				return this.fieldComponent.lineType > 0;
			}
			set
			{
				this.fieldComponent.lineType = (value ? 2 : 0);
				this.fieldComponent.lineLimit = (value ? 0 : 1);
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x00025E75 File Offset: 0x00024075
		// (set) Token: 0x06000B42 RID: 2882 RVA: 0x00025E82 File Offset: 0x00024082
		public string Text
		{
			get
			{
				return this.fieldComponent.text;
			}
			set
			{
				this.fieldComponent.SetTextWithoutNotify(value);
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x00025E90 File Offset: 0x00024090
		// (set) Token: 0x06000B44 RID: 2884 RVA: 0x00025EB0 File Offset: 0x000240B0
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

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x00025EFE File Offset: 0x000240FE
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x00025F06 File Offset: 0x00024106
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
				this.placeholderComponent.fontStyle = this.textComponent.fontStyle;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x00025F3B File Offset: 0x0002413B
		// (set) Token: 0x06000B48 RID: 2888 RVA: 0x00025F43 File Offset: 0x00024143
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
				this.placeholderComponent.alignment = this.textComponent.alignment;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x00025F78 File Offset: 0x00024178
		// (set) Token: 0x06000B4A RID: 2890 RVA: 0x00025F80 File Offset: 0x00024180
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
				this.placeholderComponent.fontSize = GlazierUtils_uGUI.GetFontSize(this._fontSize);
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00025FB5 File Offset: 0x000241B5
		// (set) Token: 0x06000B4C RID: 2892 RVA: 0x00025FC0 File Offset: 0x000241C0
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
				this.placeholderComponent.fontSharedMaterial = this.textComponent.fontSharedMaterial;
				this.placeholderComponent.characterSpacing = this.textComponent.characterSpacing;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0002602F File Offset: 0x0002422F
		// (set) Token: 0x06000B4E RID: 2894 RVA: 0x00026038 File Offset: 0x00024238
		public SleekColor TextColor
		{
			get
			{
				return this._textColor;
			}
			set
			{
				this._textColor = value;
				this.placeholderComponent.color = this._textColor.Get() * 0.5f;
				this.textComponent.color = this._textColor;
				if (this.tooltipComponent != null)
				{
					this.tooltipComponent.color = this._textColor;
				}
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x000260A6 File Offset: 0x000242A6
		// (set) Token: 0x06000B50 RID: 2896 RVA: 0x000260A9 File Offset: 0x000242A9
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

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x000260AB File Offset: 0x000242AB
		// (set) Token: 0x06000B52 RID: 2898 RVA: 0x000260B3 File Offset: 0x000242B3
		public SleekColor BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this._backgroundColor = value;
				this.SynchronizeBackgroundColor();
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x000260C2 File Offset: 0x000242C2
		// (set) Token: 0x06000B54 RID: 2900 RVA: 0x000260CF File Offset: 0x000242CF
		public bool IsClickable
		{
			get
			{
				return this.fieldComponent.interactable;
			}
			set
			{
				this.fieldComponent.interactable = value;
				this.SynchronizeBackgroundColor();
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x000260E3 File Offset: 0x000242E3
		// (set) Token: 0x06000B56 RID: 2902 RVA: 0x000260F0 File Offset: 0x000242F0
		public int MaxLength
		{
			get
			{
				return this.fieldComponent.characterLimit;
			}
			set
			{
				this.fieldComponent.characterLimit = value;
			}
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x000260FE File Offset: 0x000242FE
		public void FocusControl()
		{
			this.fieldComponent.ActivateInputField();
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0002610B File Offset: 0x0002430B
		public void ClearFocus()
		{
			this.fieldComponent.DeactivateInputField(false);
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00026119 File Offset: 0x00024319
		public GlazierStringField_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00026124 File Offset: 0x00024324
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.imageComponent = base.gameObject.AddComponent<Image>();
			this.imageComponent.enabled = false;
			this.imageComponent.type = 1;
			this.imageComponent.raycastTarget = true;
			GameObject gameObject = new GameObject("Viewport", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.transform.SetParent(base.transform, false);
			RectTransform rectTransform = gameObject.GetRectTransform();
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(2f, 2f);
			rectTransform.offsetMax = new Vector2(-2f, -2f);
			gameObject.AddComponent<RectMask2D>();
			GameObject gameObject2 = new GameObject("Placeholder", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject2.transform.SetParent(rectTransform, false);
			gameObject2.GetRectTransform().reset();
			this.placeholderComponent = gameObject2.AddComponent<TextMeshProUGUI>();
			this.placeholderComponent.enabled = false;
			this.placeholderComponent.raycastTarget = false;
			this.placeholderComponent.font = GlazierResources_uGUI.Font;
			this.placeholderComponent.margin = GlazierConst_uGUI.DefaultTextMargin;
			this.placeholderComponent.extraPadding = true;
			this.placeholderComponent.richText = false;
			GameObject gameObject3 = new GameObject("Text", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject3.transform.SetParent(rectTransform, false);
			gameObject3.GetRectTransform().reset();
			this.textComponent = gameObject3.AddComponent<TextMeshProUGUI>();
			this.textComponent.enabled = false;
			this.textComponent.raycastTarget = false;
			this.textComponent.font = GlazierResources_uGUI.Font;
			this.textComponent.margin = GlazierConst_uGUI.DefaultTextMargin;
			this.textComponent.extraPadding = true;
			this.textComponent.richText = false;
			this.fieldComponent = base.gameObject.AddComponent<TMP_InputField>();
			this.fieldComponent.enabled = false;
			this.fieldComponent.textViewport = rectTransform;
			this.fieldComponent.textComponent = this.textComponent;
			this.fieldComponent.placeholder = this.placeholderComponent;
			this.fieldComponent.transition = 2;
			this.fieldComponent.onSubmit.AddListener(new UnityAction<string>(this.OnUnitySubmit));
			this.fieldComponent.onValueChanged.AddListener(new UnityAction<string>(this.OnUnityValueChanged));
			this.fieldComponent.caretWidth = 2;
			this.fieldComponent.customCaretColor = true;
			this.fieldComponent.isRichTextEditingAllowed = false;
			this.fieldComponent.richText = false;
			this.fieldComponent.asteriskChar = '*';
			this._backgroundColor = GlazierConst.DefaultFieldBackgroundColor;
			this._textColor = GlazierConst.DefaultFieldForegroundColor;
			this.TextContrastContext = 0;
			this.FontStyle = 0;
			this.TextAlignment = 4;
			this.FontSize = 2;
			this.MaxLength = 100;
			this.IsMultiline = false;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00026428 File Offset: 0x00024628
		private void SynchronizeBackgroundColor()
		{
			Color color = this._backgroundColor;
			if (!this.IsClickable)
			{
				color.a *= 0.25f;
			}
			this.imageComponent.color = color;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00026468 File Offset: 0x00024668
		public override void SynchronizeColors()
		{
			this.SynchronizeBackgroundColor();
			this.placeholderComponent.color = this._textColor.Get() * 0.5f;
			this.textComponent.color = this._textColor;
			if (this.tooltipComponent != null)
			{
				this.tooltipComponent.color = this._textColor;
			}
			this.fieldComponent.caretColor = OptionsSettings.foregroundColor;
			Color caretColor = this.fieldComponent.caretColor;
			caretColor.a = 0.5f;
			this.fieldComponent.selectionColor = caretColor;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0002650C File Offset: 0x0002470C
		public override void SynchronizeTheme()
		{
			this.imageComponent.sprite = GlazierResources_uGUI.Theme.BoxSprite;
			SpriteState spriteState = default(SpriteState);
			spriteState.disabledSprite = this.imageComponent.sprite;
			spriteState.highlightedSprite = GlazierResources_uGUI.Theme.BoxHighlightedSprite;
			spriteState.pressedSprite = GlazierResources_uGUI.Theme.BoxHighlightedSprite;
			spriteState.selectedSprite = GlazierResources_uGUI.Theme.BoxSelectedSprite;
			this.fieldComponent.spriteState = spriteState;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00026596 File Offset: 0x00024796
		protected override void EnableComponents()
		{
			this.imageComponent.enabled = true;
			this.placeholderComponent.enabled = true;
			this.textComponent.enabled = true;
			this.fieldComponent.enabled = true;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x000265C8 File Offset: 0x000247C8
		protected virtual void OnUnitySubmit(string input)
		{
			Entered onTextSubmitted = this.OnTextSubmitted;
			if (onTextSubmitted == null)
			{
				return;
			}
			onTextSubmitted.Invoke(this);
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x000265DB File Offset: 0x000247DB
		protected virtual void OnUnityValueChanged(string input)
		{
			Typed onTextChanged = this.OnTextChanged;
			if (onTextChanged == null)
			{
				return;
			}
			onTextChanged.Invoke(this, input);
		}

		// Token: 0x04000449 RID: 1097
		private GlazieruGUITooltip tooltipComponent;

		// Token: 0x0400044A RID: 1098
		private FontStyle _fontStyle;

		// Token: 0x0400044B RID: 1099
		private TextAnchor _fontAlignment;

		// Token: 0x0400044C RID: 1100
		private ESleekFontSize _fontSize;

		// Token: 0x0400044D RID: 1101
		private ETextContrastContext _contrastContext;

		// Token: 0x0400044E RID: 1102
		private SleekColor _textColor;

		// Token: 0x0400044F RID: 1103
		private SleekColor _backgroundColor;

		// Token: 0x04000450 RID: 1104
		protected TMP_InputField fieldComponent;

		// Token: 0x04000451 RID: 1105
		private Image imageComponent;

		// Token: 0x04000452 RID: 1106
		private TextMeshProUGUI placeholderComponent;

		// Token: 0x04000453 RID: 1107
		private TextMeshProUGUI textComponent;
	}
}
