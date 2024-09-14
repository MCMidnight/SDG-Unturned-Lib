using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x0200018B RID: 395
	internal class GlazierToggle_uGUI : GlazierElementBase_uGUI, ISleekToggle, ISleekElement, ISleekWithTooltip
	{
		// Token: 0x14000053 RID: 83
		// (add) Token: 0x06000B61 RID: 2913 RVA: 0x000265F0 File Offset: 0x000247F0
		// (remove) Token: 0x06000B62 RID: 2914 RVA: 0x00026628 File Offset: 0x00024828
		public event Toggled OnValueChanged;

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0002665D File Offset: 0x0002485D
		// (set) Token: 0x06000B64 RID: 2916 RVA: 0x0002666A File Offset: 0x0002486A
		public bool Value
		{
			get
			{
				return this.toggleComponent.isOn;
			}
			set
			{
				this.toggleComponent.SetIsOnWithoutNotify(value);
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00026678 File Offset: 0x00024878
		// (set) Token: 0x06000B66 RID: 2918 RVA: 0x00026698 File Offset: 0x00024898
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
					this.tooltipComponent.color = new SleekColor(3);
				}
				this.tooltipComponent.text = value;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x000266E6 File Offset: 0x000248E6
		// (set) Token: 0x06000B68 RID: 2920 RVA: 0x000266EE File Offset: 0x000248EE
		public SleekColor BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this._backgroundColor = value;
				this.backgroundImageComponent.color = this._backgroundColor;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0002670D File Offset: 0x0002490D
		// (set) Token: 0x06000B6A RID: 2922 RVA: 0x00026715 File Offset: 0x00024915
		public SleekColor ForegroundColor
		{
			get
			{
				return this._foregroundColor;
			}
			set
			{
				this._foregroundColor = value;
				this.foregroundImageComponent.color = this._foregroundColor;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x00026734 File Offset: 0x00024934
		// (set) Token: 0x06000B6C RID: 2924 RVA: 0x00026741 File Offset: 0x00024941
		public bool IsInteractable
		{
			get
			{
				return this.toggleComponent.interactable;
			}
			set
			{
				this.toggleComponent.interactable = value;
				this.SynchronizeColors();
			}
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00026755 File Offset: 0x00024955
		public GlazierToggle_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00026760 File Offset: 0x00024960
		public override void ConstructNew()
		{
			base.ConstructNew();
			base.SizeOffset_X = 40f;
			base.SizeOffset_Y = 40f;
			GameObject gameObject = new GameObject("Background", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.transform.SetParent(base.transform, false);
			RectTransform rectTransform = gameObject.GetRectTransform();
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(20f, 20f);
			this.backgroundImageComponent = gameObject.AddComponent<Image>();
			this.backgroundImageComponent.enabled = false;
			this.backgroundImageComponent.raycastTarget = true;
			GameObject gameObject2 = new GameObject("Foreground", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject2.transform.SetParent(base.transform, false);
			gameObject2.GetRectTransform().reset();
			this.foregroundImageComponent = gameObject2.AddComponent<Image>();
			this.foregroundImageComponent.enabled = false;
			this.foregroundImageComponent.raycastTarget = false;
			this.toggleComponent = base.gameObject.AddComponent<Toggle>();
			this.toggleComponent.enabled = false;
			this.toggleComponent.transition = 2;
			this.toggleComponent.targetGraphic = this.backgroundImageComponent;
			this.toggleComponent.graphic = this.foregroundImageComponent;
			this.toggleComponent.onValueChanged.AddListener(new UnityAction<bool>(this.uGUIonValueChanged));
			this._backgroundColor = GlazierConst.DefaultToggleBackgroundColor;
			this._foregroundColor = GlazierConst.DefaultToggleForegroundColor;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00026900 File Offset: 0x00024B00
		public override void SynchronizeColors()
		{
			Color color = this._backgroundColor;
			Color color2 = this._foregroundColor;
			if (!this.IsInteractable)
			{
				color.a *= 0.25f;
				color2.a *= 0.25f;
			}
			this.backgroundImageComponent.color = color;
			this.foregroundImageComponent.color = color2;
			if (this.tooltipComponent != null)
			{
				this.tooltipComponent.color = new SleekColor(3);
			}
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0002698C File Offset: 0x00024B8C
		public override void SynchronizeTheme()
		{
			this.backgroundImageComponent.sprite = GlazierResources_uGUI.Theme.BoxSprite;
			this.foregroundImageComponent.sprite = GlazierResources_uGUI.Theme.ToggleForegroundSprite;
			SpriteState spriteState = default(SpriteState);
			spriteState.highlightedSprite = GlazierResources_uGUI.Theme.BoxHighlightedSprite;
			spriteState.selectedSprite = GlazierResources_uGUI.Theme.BoxSelectedSprite;
			spriteState.disabledSprite = this.backgroundImageComponent.sprite;
			spriteState.pressedSprite = GlazierResources_uGUI.Theme.BoxPressedSprite;
			this.toggleComponent.spriteState = spriteState;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00026A30 File Offset: 0x00024C30
		protected override void EnableComponents()
		{
			this.backgroundImageComponent.enabled = true;
			this.foregroundImageComponent.enabled = true;
			this.toggleComponent.enabled = true;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00026A56 File Offset: 0x00024C56
		private void uGUIonValueChanged(bool isOn)
		{
			Toggled onValueChanged = this.OnValueChanged;
			if (onValueChanged == null)
			{
				return;
			}
			onValueChanged.Invoke(this, isOn);
		}

		// Token: 0x04000455 RID: 1109
		private GlazieruGUITooltip tooltipComponent;

		// Token: 0x04000456 RID: 1110
		private SleekColor _backgroundColor;

		// Token: 0x04000457 RID: 1111
		private SleekColor _foregroundColor;

		// Token: 0x04000458 RID: 1112
		private Image backgroundImageComponent;

		// Token: 0x04000459 RID: 1113
		private Image foregroundImageComponent;

		// Token: 0x0400045A RID: 1114
		private Toggle toggleComponent;
	}
}
