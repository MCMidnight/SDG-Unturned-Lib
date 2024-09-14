using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000188 RID: 392
	internal class GlazierSlider_uGUI : GlazierElementBase_uGUI, ISleekSlider, ISleekElement
	{
		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06000B0F RID: 2831 RVA: 0x00025560 File Offset: 0x00023760
		// (remove) Token: 0x06000B10 RID: 2832 RVA: 0x00025598 File Offset: 0x00023798
		public event Dragged OnValueChanged;

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x000255CD File Offset: 0x000237CD
		// (set) Token: 0x06000B12 RID: 2834 RVA: 0x000255D5 File Offset: 0x000237D5
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

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x000255ED File Offset: 0x000237ED
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x000255FA File Offset: 0x000237FA
		public float Value
		{
			get
			{
				return this.scrollbarComponent.value;
			}
			set
			{
				this.scrollbarComponent.SetValueWithoutNotify(value);
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00025608 File Offset: 0x00023808
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x00025610 File Offset: 0x00023810
		public SleekColor BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this._backgroundColor = value;
				this.backgroundImage.color = this._backgroundColor;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x0002562F File Offset: 0x0002382F
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x00025637 File Offset: 0x00023837
		public SleekColor ForegroundColor
		{
			get
			{
				return this._foregroundColor;
			}
			set
			{
				this._foregroundColor = value;
				this.handleImage.color = this._foregroundColor;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00025656 File Offset: 0x00023856
		// (set) Token: 0x06000B1A RID: 2842 RVA: 0x00025663 File Offset: 0x00023863
		public bool IsInteractable
		{
			get
			{
				return this.scrollbarComponent.interactable;
			}
			set
			{
				this.scrollbarComponent.interactable = value;
				this.SynchronizeColors();
			}
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00025677 File Offset: 0x00023877
		public GlazierSlider_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00025688 File Offset: 0x00023888
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.scrollbarComponent = base.gameObject.AddComponent<Scrollbar>();
			this.scrollbarComponent.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderComponentValueChanged));
			GameObject gameObject = new GameObject("Background", new Type[]
			{
				typeof(RectTransform)
			});
			this.backgroundTransform = gameObject.GetRectTransform();
			this.backgroundTransform.SetParent(base.transform, false);
			this.backgroundTransform.anchoredPosition = Vector2.zero;
			this.backgroundImage = gameObject.AddComponent<Image>();
			this.backgroundImage.type = 1;
			this.backgroundImage.raycastTarget = true;
			GameObject gameObject2 = new GameObject("Handle", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform rectTransform = gameObject2.GetRectTransform();
			rectTransform.SetParent(base.transform, false);
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.sizeDelta = Vector2.zero;
			this.handleImage = gameObject2.AddComponent<Image>();
			this.handleImage.type = 1;
			this.handleImage.raycastTarget = true;
			this.scrollbarComponent.handleRect = rectTransform;
			this.scrollbarComponent.size = 0.25f;
			this.scrollbarComponent.transition = 2;
			this.scrollbarComponent.targetGraphic = this.handleImage;
			this._orientation = 1;
			this.UpdateOrientation();
			this._backgroundColor = GlazierConst.DefaultSliderBackgroundColor;
			this._foregroundColor = GlazierConst.DefaultSliderForegroundColor;
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00025800 File Offset: 0x00023A00
		public override void SynchronizeColors()
		{
			Color color = this._backgroundColor;
			Color color2 = this._foregroundColor;
			if (!this.IsInteractable)
			{
				color.a *= 0.25f;
				color2.a *= 0.25f;
			}
			this.backgroundImage.color = color;
			this.handleImage.color = color2;
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00025868 File Offset: 0x00023A68
		public override void SynchronizeTheme()
		{
			this.backgroundImage.sprite = GlazierResources_uGUI.Theme.SliderBackgroundSprite;
			this.handleImage.sprite = GlazierResources_uGUI.Theme.BoxSprite;
			SpriteState spriteState = default(SpriteState);
			spriteState.disabledSprite = this.handleImage.sprite;
			spriteState.highlightedSprite = GlazierResources_uGUI.Theme.BoxHighlightedSprite;
			spriteState.selectedSprite = GlazierResources_uGUI.Theme.BoxSelectedSprite;
			spriteState.pressedSprite = GlazierResources_uGUI.Theme.BoxPressedSprite;
			this.scrollbarComponent.spriteState = spriteState;
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0002590C File Offset: 0x00023B0C
		private void UpdateOrientation()
		{
			ESleekOrientation orientation = this.Orientation;
			if (orientation == null)
			{
				this.backgroundTransform.anchorMin = new Vector2(0f, 0.5f);
				this.backgroundTransform.anchorMax = new Vector2(1f, 0.5f);
				this.backgroundTransform.sizeDelta = new Vector2(-20f, 6f);
				this.scrollbarComponent.SetDirection(0, false);
				return;
			}
			if (orientation != 1)
			{
				return;
			}
			this.backgroundTransform.anchorMin = new Vector2(0.5f, 0f);
			this.backgroundTransform.anchorMax = new Vector2(0.5f, 1f);
			this.backgroundTransform.sizeDelta = new Vector2(6f, -20f);
			this.scrollbarComponent.SetDirection(3, false);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x000259DF File Offset: 0x00023BDF
		private void OnSliderComponentValueChanged(float value)
		{
			Dragged onValueChanged = this.OnValueChanged;
			if (onValueChanged == null)
			{
				return;
			}
			onValueChanged.Invoke(this, value);
		}

		// Token: 0x04000439 RID: 1081
		private ESleekOrientation _orientation = 1;

		// Token: 0x0400043A RID: 1082
		private SleekColor _backgroundColor;

		// Token: 0x0400043B RID: 1083
		private SleekColor _foregroundColor;

		// Token: 0x0400043C RID: 1084
		private Scrollbar scrollbarComponent;

		// Token: 0x0400043D RID: 1085
		private Image backgroundImage;

		// Token: 0x0400043E RID: 1086
		private Image handleImage;

		// Token: 0x0400043F RID: 1087
		private RectTransform backgroundTransform;
	}
}
