using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000191 RID: 401
	internal class Glazier_uGUI : GlazierBase, IGlazier
	{
		// Token: 0x06000B99 RID: 2969 RVA: 0x00026F38 File Offset: 0x00025138
		public ISleekBox CreateBox()
		{
			GlazierBox_uGUI glazierBox_uGUI = new GlazierBox_uGUI(this);
			this.elements.Add(glazierBox_uGUI);
			GlazierBox_uGUI.BoxPoolData boxPoolData = this.ClaimElementFromPool<GlazierBox_uGUI.BoxPoolData>(this.boxPool);
			if (boxPoolData == null)
			{
				glazierBox_uGUI.ConstructNew();
			}
			else
			{
				glazierBox_uGUI.ConstructFromBoxPool(boxPoolData);
			}
			glazierBox_uGUI.SynchronizeTheme();
			glazierBox_uGUI.SynchronizeColors();
			return glazierBox_uGUI;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00026F84 File Offset: 0x00025184
		public ISleekButton CreateButton()
		{
			GlazierButton_uGUI glazierButton_uGUI = new GlazierButton_uGUI(this);
			this.elements.Add(glazierButton_uGUI);
			GlazierButton_uGUI.ButtonPoolData buttonPoolData = this.ClaimElementFromPool<GlazierButton_uGUI.ButtonPoolData>(this.buttonPool);
			if (buttonPoolData == null)
			{
				glazierButton_uGUI.ConstructNew();
			}
			else
			{
				glazierButton_uGUI.ConstructFromButtonPool(buttonPoolData);
			}
			glazierButton_uGUI.SynchronizeTheme();
			glazierButton_uGUI.SynchronizeColors();
			return glazierButton_uGUI;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00026FD0 File Offset: 0x000251D0
		public ISleekElement CreateFrame()
		{
			GlazierEmpty_uGUI glazierEmpty_uGUI = new GlazierEmpty_uGUI(this);
			this.elements.Add(glazierEmpty_uGUI);
			GlazierElementBase_uGUI.PoolData poolData = this.ClaimElementFromPool<GlazierElementBase_uGUI.PoolData>(this.framePool);
			if (poolData == null)
			{
				glazierEmpty_uGUI.ConstructNew();
			}
			else
			{
				glazierEmpty_uGUI.ConstructFromPool(poolData);
			}
			return glazierEmpty_uGUI;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00027010 File Offset: 0x00025210
		public ISleekConstraintFrame CreateConstraintFrame()
		{
			GlazierConstraintFrame_uGUI glazierConstraintFrame_uGUI = new GlazierConstraintFrame_uGUI(this);
			glazierConstraintFrame_uGUI.ConstructNew();
			this.elements.Add(glazierConstraintFrame_uGUI);
			return glazierConstraintFrame_uGUI;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00027037 File Offset: 0x00025237
		public ISleekImage CreateImage()
		{
			return this.CreateImage(null);
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00027040 File Offset: 0x00025240
		public ISleekImage CreateImage(Texture texture)
		{
			GlazierImage_uGUI glazierImage_uGUI = new GlazierImage_uGUI(this);
			this.elements.Add(glazierImage_uGUI);
			GlazierImage_uGUI.ImagePoolData imagePoolData = this.ClaimElementFromPool<GlazierImage_uGUI.ImagePoolData>(this.imagePool);
			if (imagePoolData == null)
			{
				glazierImage_uGUI.ConstructNew();
			}
			else
			{
				glazierImage_uGUI.ConstructFromImagePool(imagePoolData);
			}
			glazierImage_uGUI.Texture = texture;
			glazierImage_uGUI.SynchronizeColors();
			return glazierImage_uGUI;
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0002708D File Offset: 0x0002528D
		public ISleekSprite CreateSprite()
		{
			return this.CreateSprite(null);
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00027098 File Offset: 0x00025298
		public ISleekSprite CreateSprite(Sprite sprite)
		{
			GlazierSprite_uGUI glazierSprite_uGUI = new GlazierSprite_uGUI(this, sprite);
			glazierSprite_uGUI.ConstructNew();
			this.elements.Add(glazierSprite_uGUI);
			glazierSprite_uGUI.Sprite = sprite;
			glazierSprite_uGUI.SynchronizeColors();
			return glazierSprite_uGUI;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x000270D0 File Offset: 0x000252D0
		public ISleekLabel CreateLabel()
		{
			GlazierLabel_uGUI glazierLabel_uGUI = new GlazierLabel_uGUI(this);
			this.elements.Add(glazierLabel_uGUI);
			GlazierLabel_uGUI.LabelPoolData labelPoolData = this.ClaimElementFromPool<GlazierLabel_uGUI.LabelPoolData>(this.labelPool);
			if (labelPoolData == null)
			{
				glazierLabel_uGUI.ConstructNew();
			}
			else
			{
				glazierLabel_uGUI.ConstructFromLabelPool(labelPoolData);
			}
			glazierLabel_uGUI.SynchronizeColors();
			return glazierLabel_uGUI;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00027118 File Offset: 0x00025318
		public ISleekScrollView CreateScrollView()
		{
			GlazierScrollView_uGUI glazierScrollView_uGUI = new GlazierScrollView_uGUI(this);
			glazierScrollView_uGUI.ConstructNew();
			this.elements.Add(glazierScrollView_uGUI);
			glazierScrollView_uGUI.SynchronizeTheme();
			glazierScrollView_uGUI.SynchronizeColors();
			return glazierScrollView_uGUI;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0002714C File Offset: 0x0002534C
		public ISleekSlider CreateSlider()
		{
			GlazierSlider_uGUI glazierSlider_uGUI = new GlazierSlider_uGUI(this);
			glazierSlider_uGUI.ConstructNew();
			this.elements.Add(glazierSlider_uGUI);
			glazierSlider_uGUI.SynchronizeTheme();
			glazierSlider_uGUI.SynchronizeColors();
			return glazierSlider_uGUI;
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00027180 File Offset: 0x00025380
		public ISleekField CreateStringField()
		{
			GlazierStringField_uGUI glazierStringField_uGUI = new GlazierStringField_uGUI(this);
			glazierStringField_uGUI.ConstructNew();
			this.elements.Add(glazierStringField_uGUI);
			glazierStringField_uGUI.SynchronizeTheme();
			glazierStringField_uGUI.SynchronizeColors();
			return glazierStringField_uGUI;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x000271B4 File Offset: 0x000253B4
		public ISleekToggle CreateToggle()
		{
			GlazierToggle_uGUI glazierToggle_uGUI = new GlazierToggle_uGUI(this);
			glazierToggle_uGUI.ConstructNew();
			this.elements.Add(glazierToggle_uGUI);
			glazierToggle_uGUI.SynchronizeTheme();
			glazierToggle_uGUI.SynchronizeColors();
			return glazierToggle_uGUI;
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x000271E8 File Offset: 0x000253E8
		public ISleekUInt8Field CreateUInt8Field()
		{
			GlazierUInt8Field_uGUI glazierUInt8Field_uGUI = new GlazierUInt8Field_uGUI(this);
			glazierUInt8Field_uGUI.ConstructNew();
			this.elements.Add(glazierUInt8Field_uGUI);
			glazierUInt8Field_uGUI.SynchronizeTheme();
			glazierUInt8Field_uGUI.SynchronizeColors();
			return glazierUInt8Field_uGUI;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0002721C File Offset: 0x0002541C
		public ISleekUInt16Field CreateUInt16Field()
		{
			GlazierUInt16Field_uGUI glazierUInt16Field_uGUI = new GlazierUInt16Field_uGUI(this);
			glazierUInt16Field_uGUI.ConstructNew();
			this.elements.Add(glazierUInt16Field_uGUI);
			glazierUInt16Field_uGUI.SynchronizeTheme();
			glazierUInt16Field_uGUI.SynchronizeColors();
			return glazierUInt16Field_uGUI;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00027250 File Offset: 0x00025450
		public ISleekUInt32Field CreateUInt32Field()
		{
			GlazierUInt32Field_uGUI glazierUInt32Field_uGUI = new GlazierUInt32Field_uGUI(this);
			glazierUInt32Field_uGUI.ConstructNew();
			this.elements.Add(glazierUInt32Field_uGUI);
			glazierUInt32Field_uGUI.SynchronizeTheme();
			glazierUInt32Field_uGUI.SynchronizeColors();
			return glazierUInt32Field_uGUI;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00027284 File Offset: 0x00025484
		public ISleekInt32Field CreateInt32Field()
		{
			GlazierInt32Field_uGUI glazierInt32Field_uGUI = new GlazierInt32Field_uGUI(this);
			glazierInt32Field_uGUI.ConstructNew();
			this.elements.Add(glazierInt32Field_uGUI);
			glazierInt32Field_uGUI.SynchronizeTheme();
			glazierInt32Field_uGUI.SynchronizeColors();
			return glazierInt32Field_uGUI;
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x000272B8 File Offset: 0x000254B8
		public ISleekFloat32Field CreateFloat32Field()
		{
			GlazierFloat32Field_uGUI glazierFloat32Field_uGUI = new GlazierFloat32Field_uGUI(this);
			glazierFloat32Field_uGUI.ConstructNew();
			this.elements.Add(glazierFloat32Field_uGUI);
			glazierFloat32Field_uGUI.SynchronizeTheme();
			glazierFloat32Field_uGUI.SynchronizeColors();
			return glazierFloat32Field_uGUI;
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x000272EC File Offset: 0x000254EC
		public ISleekFloat64Field CreateFloat64Field()
		{
			GlazierFloat64Field_uGUI glazierFloat64Field_uGUI = new GlazierFloat64Field_uGUI(this);
			glazierFloat64Field_uGUI.ConstructNew();
			this.elements.Add(glazierFloat64Field_uGUI);
			glazierFloat64Field_uGUI.SynchronizeTheme();
			glazierFloat64Field_uGUI.SynchronizeColors();
			return glazierFloat64Field_uGUI;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00027320 File Offset: 0x00025520
		public ISleekElement CreateProxyImplementation(SleekWrapper owner)
		{
			GlazierProxy_uGUI glazierProxy_uGUI = new GlazierProxy_uGUI(this);
			this.elements.Add(glazierProxy_uGUI);
			GlazierElementBase_uGUI.PoolData poolData = this.ClaimElementFromPool<GlazierElementBase_uGUI.PoolData>(this.framePool);
			if (poolData == null)
			{
				glazierProxy_uGUI.ConstructNew();
			}
			else
			{
				glazierProxy_uGUI.ConstructFromPool(poolData);
			}
			glazierProxy_uGUI.InitOwner(owner);
			return glazierProxy_uGUI;
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x00027367 File Offset: 0x00025567
		public bool SupportsDepth
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x0002736A File Offset: 0x0002556A
		public bool SupportsRichTextAlpha
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0002736D File Offset: 0x0002556D
		public bool SupportsAutomaticLayout
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x00027370 File Offset: 0x00025570
		// (set) Token: 0x06000BB1 RID: 2993 RVA: 0x00027378 File Offset: 0x00025578
		public SleekWindow Root
		{
			get
			{
				return this._root;
			}
			set
			{
				if (this._root == value)
				{
					return;
				}
				if (this.rootImpl != null && this.rootImpl.transform != null)
				{
					this.rootImpl.transform.SetParent(null, false);
				}
				this._root = value;
				if (this._root != null)
				{
					this.rootImpl = (this._root.AttachmentRoot as GlazierElementBase_uGUI);
					if (this.rootImpl != null)
					{
						this.rootImpl.transform.SetParent(base.transform, false);
						this.rootImpl.transform.SetAsFirstSibling();
					}
					else
					{
						UnturnedLog.warn("Root must be a uGUI element: {0}", new object[]
						{
							this._root.GetType().Name
						});
					}
				}
				else
				{
					this.rootImpl = null;
				}
				this.canvas.sortingOrder = ((this._root != null && this._root.hackSortOrder) ? 29000 : 15);
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00027467 File Offset: 0x00025667
		public static Glazier_uGUI CreateGlazier()
		{
			GameObject gameObject = new GameObject("Glazier");
			Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<Glazier_uGUI>();
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0002747E File Offset: 0x0002567E
		internal void ReleaseBoxToPool(GlazierBox_uGUI.BoxPoolData poolData)
		{
			this.boxPool.Add(poolData);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0002748C File Offset: 0x0002568C
		internal void ReleaseButtonToPool(GlazierButton_uGUI.ButtonPoolData poolData)
		{
			this.buttonPool.Add(poolData);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0002749A File Offset: 0x0002569A
		internal void ReleaseEmptyToPool(GlazierElementBase_uGUI.PoolData poolData)
		{
			this.framePool.Add(poolData);
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x000274A8 File Offset: 0x000256A8
		internal void ReleaseImageToPool(GlazierImage_uGUI.ImagePoolData poolData)
		{
			this.imagePool.Add(poolData);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000274B6 File Offset: 0x000256B6
		internal void ReleaseLabelToPool(GlazierLabel_uGUI.LabelPoolData poolData)
		{
			this.labelPool.Add(poolData);
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x000274C4 File Offset: 0x000256C4
		internal Material GetFontMaterial(ETextContrastStyle shadowStyle)
		{
			switch (shadowStyle)
			{
			default:
				return this.fontMaterial_Default;
			case 1:
				return this.fontMaterial_Outline;
			case 2:
				return this.fontMaterial_Shadow;
			case 3:
				return this.fontMaterial_Tooltip;
			}
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x000274F8 File Offset: 0x000256F8
		protected override void OnEnable()
		{
			base.OnEnable();
			OptionsSettings.OnCustomColorsChanged += new Action(this.OnCustomColorsChanged);
			OptionsSettings.OnThemeChanged += new Action(this.OnThemeChanged);
			this.canvas = base.gameObject.AddComponent<Canvas>();
			this.canvas.renderMode = 0;
			this.canvas.sortingOrder = 15;
			if (Glazier_uGUI.clPixelPerfect.hasValue)
			{
				this.canvas.pixelPerfect = (Glazier_uGUI.clPixelPerfect.value > 0);
			}
			base.gameObject.AddComponent<GraphicRaycaster>();
			this.CreateFontMaterials();
			this.CreateDebugText();
			this.CreateCursor();
			this.CreateTooltip();
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0002759F File Offset: 0x0002579F
		private void OnDestroy()
		{
			this.DestroyFontMaterials();
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x000275A8 File Offset: 0x000257A8
		private void CreateDebugText()
		{
			GameObject gameObject = new GameObject("Debug", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform rectTransform = gameObject.GetRectTransform();
			rectTransform.SetParent(base.transform, false);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.sizeDelta = new Vector2(800f, 30f);
			rectTransform.anchoredPosition = Vector2.zero;
			ETextContrastStyle shadowStyle = SleekShadowStyle.ContextToStyle(2);
			this.debugTextComponent = gameObject.AddComponent<TextMeshProUGUI>();
			this.debugTextComponent.font = GlazierResources_uGUI.Font;
			this.debugTextComponent.fontSharedMaterial = this.GetFontMaterial(shadowStyle);
			this.debugTextComponent.characterSpacing = GlazierUtils_uGUI.GetCharacterSpacing(shadowStyle);
			this.debugTextComponent.fontSize = GlazierUtils_uGUI.GetFontSize(2);
			this.debugTextComponent.fontStyle = GlazierUtils_uGUI.GetFontStyleFlags(0);
			this.debugTextComponent.raycastTarget = false;
			this.debugTextComponent.alignment = 257;
			this.debugTextComponent.margin = GlazierConst_uGUI.DefaultTextMargin;
			this.debugTextComponent.extraPadding = true;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x000276EC File Offset: 0x000258EC
		private void CreateFontMaterials()
		{
			this.fontMaterial_Default = Resources.Load<Material>("UI/Glazier_uGUI/Font_Default");
			this.fontMaterial_Outline = Object.Instantiate<Material>(Resources.Load<Material>("UI/Glazier_uGUI/Font_Outline"));
			this.fontMaterial_Shadow = Object.Instantiate<Material>(Resources.Load<Material>("UI/Glazier_uGUI/Font_Shadow"));
			this.fontMaterial_Tooltip = Object.Instantiate<Material>(Resources.Load<Material>("UI/Glazier_uGUI/Font_Tooltip"));
			this.SynchronizeFontMaterials();
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00027750 File Offset: 0x00025950
		private void SynchronizeFontMaterials()
		{
			Color shadowColor;
			Color color = shadowColor = SleekCustomization.shadowColor;
			shadowColor.a = 0.25f;
			Color value = color;
			value.a = 0.75f;
			this.fontMaterial_Outline.SetColor("_OutlineColor", shadowColor);
			this.fontMaterial_Outline.SetColor("_UnderlayColor", value);
			Color color2 = color;
			color2.a = 0.75f;
			this.fontMaterial_Shadow.SetColor("_UnderlayColor", value);
			Color value2 = color;
			shadowColor.a = 1f;
			Color value3 = color;
			value3.a = 1f;
			this.fontMaterial_Tooltip.SetColor("_OutlineColor", value2);
			this.fontMaterial_Tooltip.SetColor("_UnderlayColor", value3);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x000277FE File Offset: 0x000259FE
		private void DestroyFontMaterials()
		{
			Object.Destroy(this.fontMaterial_Outline);
			Object.Destroy(this.fontMaterial_Shadow);
			Object.Destroy(this.fontMaterial_Tooltip);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00027824 File Offset: 0x00025A24
		private void CreateCursor()
		{
			GameObject gameObject = new GameObject("Cursor", new Type[]
			{
				typeof(RectTransform)
			});
			this.cursorTransform = gameObject.GetRectTransform();
			this.cursorTransform.SetParent(base.transform, false);
			this.cursorTransform.anchorMin = Vector2.zero;
			this.cursorTransform.anchorMax = Vector2.zero;
			this.cursorTransform.pivot = new Vector2(0f, 1f);
			this.cursorTransform.sizeDelta = new Vector2(20f, 20f);
			this.cursorImage = gameObject.AddComponent<RawImage>();
			this.cursorImage.texture = Glazier_uGUI.defaultCursor;
			this.cursorImage.raycastTarget = false;
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.overrideSorting = true;
			canvas.sortingOrder = 30000;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00027908 File Offset: 0x00025B08
		private void CreateTooltip()
		{
			this.tooltipGameObject = new GameObject("Tooltip", new Type[]
			{
				typeof(RectTransform)
			});
			this.tooltipTransform = this.tooltipGameObject.GetRectTransform();
			this.tooltipTransform.SetParent(base.transform, false);
			VerticalLayoutGroup verticalLayoutGroup = this.tooltipGameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			verticalLayoutGroup.padding = new RectOffset(5, 5, 5, 5);
			ContentSizeFitter contentSizeFitter = this.tooltipGameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = 2;
			contentSizeFitter.verticalFit = 2;
			this.tooltipShadowImage = this.tooltipGameObject.AddComponent<Image>();
			this.tooltipShadowImage.raycastTarget = false;
			this.tooltipShadowImage.type = 1;
			this.tooltipShadowImage.sprite = GlazierResources_uGUI.TooltipShadowSprite;
			this.SynchronizeTooltipShadowColor();
			GameObject gameObject = new GameObject("Text", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.GetRectTransform().SetParent(this.tooltipTransform, false);
			ETextContrastStyle shadowStyle = SleekShadowStyle.ContextToStyle(3);
			this.tooltipTextComponent = gameObject.AddComponent<TextMeshProUGUI>();
			this.tooltipTextComponent.font = GlazierResources_uGUI.Font;
			this.tooltipTextComponent.fontSharedMaterial = this.GetFontMaterial(shadowStyle);
			this.tooltipTextComponent.characterSpacing = GlazierUtils_uGUI.GetCharacterSpacing(shadowStyle);
			this.tooltipTextComponent.fontSize = GlazierUtils_uGUI.GetFontSize(2);
			this.tooltipTextComponent.fontStyle = GlazierUtils_uGUI.GetFontStyleFlags(0);
			this.tooltipTextComponent.raycastTarget = false;
			this.tooltipTextComponent.margin = GlazierConst_uGUI.DefaultTextMargin;
			this.tooltipTextComponent.extraPadding = true;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00027AB4 File Offset: 0x00025CB4
		private void SynchronizeTooltipShadowColor()
		{
			Color shadowColor = SleekCustomization.shadowColor;
			shadowColor.a = 0.5f;
			this.tooltipShadowImage.color = shadowColor;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00027AE0 File Offset: 0x00025CE0
		private void UpdateDebug()
		{
			if (OptionsSettings.debug && this._root != null && (this._root.isEnabled || this._root.drawCursorWhileDisabled))
			{
				base.UpdateDebugStats();
				base.UpdateDebugString();
				this.debugTextComponent.color = base.debugStringColor;
				this.debugTextComponent.text = base.debugString;
				this.debugTextComponent.enabled = true;
				return;
			}
			this.debugTextComponent.enabled = false;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00027B60 File Offset: 0x00025D60
		private void UpdateCursor()
		{
			bool shouldDrawCursor = this.Root.ShouldDrawCursor;
			if (shouldDrawCursor != this.wasCursorVisible)
			{
				this.wasCursorVisible = shouldDrawCursor;
				this.cursorImage.gameObject.SetActive(shouldDrawCursor);
			}
			this.cursorImage.color = SleekCustomization.cursorColor;
			Vector2 normalizedMousePosition = InputEx.NormalizedMousePosition;
			this.cursorTransform.anchorMin = normalizedMousePosition;
			this.cursorTransform.anchorMax = normalizedMousePosition;
			this.cursorTransform.anchoredPosition = Vector2.zero;
			this.cursorImage.texture = Glazier_uGUI.defaultCursor;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00027BF0 File Offset: 0x00025DF0
		private void UpdateTooltip()
		{
			bool shouldDrawTooltip = this.Root.ShouldDrawTooltip;
			GlazieruGUITooltip tooltip = GlazieruGUITooltip.GetTooltip();
			if (tooltip != this.lastTooltip)
			{
				this.lastTooltip = tooltip;
				this.startedTooltip = Time.realtimeSinceStartup;
			}
			if (shouldDrawTooltip && tooltip != null && !string.IsNullOrEmpty(tooltip.text) && Time.realtimeSinceStartup - this.startedTooltip > 0.5f)
			{
				Vector2 normalizedMousePosition = InputEx.NormalizedMousePosition;
				this.tooltipTransform.anchorMin = normalizedMousePosition;
				this.tooltipTransform.anchorMax = normalizedMousePosition;
				if (normalizedMousePosition.x > 0.7f)
				{
					this.tooltipTransform.anchoredPosition = new Vector2(-10f, 0f);
					this.tooltipTransform.pivot = new Vector2(1f, 1f);
					this.tooltipTextComponent.alignment = 260;
				}
				else
				{
					this.tooltipTransform.anchoredPosition = new Vector2(30f, 0f);
					this.tooltipTransform.pivot = new Vector2(0f, 1f);
					this.tooltipTextComponent.alignment = 257;
				}
				this.tooltipTextComponent.text = tooltip.text;
				this.tooltipTextComponent.color = tooltip.color;
				this.tooltipGameObject.SetActive(true);
				return;
			}
			this.tooltipGameObject.SetActive(false);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00027D58 File Offset: 0x00025F58
		private void OnCustomColorsChanged()
		{
			foreach (GlazierElementBase_uGUI glazierElementBase_uGUI in this.EnumerateLiveElements())
			{
				glazierElementBase_uGUI.SynchronizeColors();
			}
			this.SynchronizeFontMaterials();
			this.SynchronizeTooltipShadowColor();
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00027DB0 File Offset: 0x00025FB0
		private void OnThemeChanged()
		{
			foreach (GlazierElementBase_uGUI glazierElementBase_uGUI in this.EnumerateLiveElements())
			{
				glazierElementBase_uGUI.SynchronizeTheme();
				glazierElementBase_uGUI.SynchronizeColors();
			}
		}

		/// <summary>
		/// Enumerate elements that are not in the pool.
		/// </summary>
		// Token: 0x06000BC7 RID: 3015 RVA: 0x00027E00 File Offset: 0x00026000
		private IEnumerable<GlazierElementBase_uGUI> EnumerateLiveElements()
		{
			int num;
			for (int index = this.elements.Count - 1; index >= 0; index = num)
			{
				GlazierElementBase_uGUI glazierElementBase_uGUI = this.elements[index];
				if (glazierElementBase_uGUI.gameObject == null)
				{
					this.elements.RemoveAtFast(index);
				}
				else
				{
					yield return glazierElementBase_uGUI;
				}
				num = index - 1;
			}
			yield break;
		}

		/// <summary>
		/// Sanity check all returned elements have a gameObject.
		/// </summary>
		// Token: 0x06000BC8 RID: 3016 RVA: 0x00027E10 File Offset: 0x00026010
		[Conditional("VALIDATE_GLAZIER_USE_AFTER_DESTROY")]
		private void ValidateNewElement(GlazierElementBase_uGUI element)
		{
			if (element.gameObject == null)
			{
				throw new Exception("uGUI element constructed with null gameObject");
			}
			if (element.transform == null)
			{
				throw new Exception("uGUI element constructed with null transform");
			}
			if (element.gameObject.GetComponent<LayoutElement>() != null)
			{
				throw new Exception("uGUI GameObject has a LayoutElement component, likely not removed before returning to the pool");
			}
			if (element.gameObject.GetComponent<LayoutGroup>() != null)
			{
				throw new Exception("uGUI GameObject has a LayoutGroup component, likely not removed before returning to the pool");
			}
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00027E8C File Offset: 0x0002608C
		[Conditional("VALIDATE_GLAZIER_USE_AFTER_DESTROY")]
		private void ValidateGameObjectReturnedToPool(GameObject gameObject)
		{
			if (gameObject == null)
			{
				throw new Exception("uGUI element returned null gameObject to pool");
			}
			if (gameObject.GetComponent<LayoutElement>() != null)
			{
				throw new Exception("uGUI GameObject has a LayoutElement component, should have been removed before returning to the pool");
			}
			if (gameObject.GetComponent<LayoutGroup>() != null)
			{
				throw new Exception("uGUI GameObject has a LayoutGroup component, should have been removed before returning to the pool");
			}
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x00027EE0 File Offset: 0x000260E0
		private T ClaimElementFromPool<T>(List<T> pool) where T : GlazierElementBase_uGUI.PoolData
		{
			while (pool.Count > 0)
			{
				int num = Random.Range(0, pool.Count - 1);
				T t = pool[num];
				pool.RemoveAtFast(num);
				if (t != null && !(t.gameObject == null))
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00027F3C File Offset: 0x0002613C
		private void LateUpdate()
		{
			float userInterfaceScale = GraphicsSettings.userInterfaceScale;
			if (MathfEx.IsNearlyEqual(userInterfaceScale, 1f, 0.001f))
			{
				if (this.canvasScaler != null)
				{
					Object.Destroy(this.canvasScaler);
					this.canvasScaler = null;
				}
			}
			else
			{
				if (this.canvasScaler == null)
				{
					this.canvasScaler = base.gameObject.AddComponent<CanvasScaler>();
					this.canvasScaler.uiScaleMode = 0;
				}
				this.canvasScaler.scaleFactor = userInterfaceScale;
			}
			if (this.rootImpl != null)
			{
				this.rootImpl.Update();
				bool isCursorLocked = this._root.isCursorLocked;
				if (this.wasCursorLocked != isCursorLocked)
				{
					this.wasCursorLocked = isCursorLocked;
					if (isCursorLocked)
					{
						EventSystem.current.SetSelectedGameObject(null);
					}
				}
				this.rootImpl.gameObject.SetActive(this._root.isEnabled);
			}
			this.UpdateDebug();
			this.UpdateCursor();
			this.UpdateTooltip();
		}

		// Token: 0x04000467 RID: 1127
		private SleekWindow _root;

		// Token: 0x04000468 RID: 1128
		private GlazierElementBase_uGUI rootImpl;

		// Token: 0x04000469 RID: 1129
		private List<GlazierElementBase_uGUI> elements = new List<GlazierElementBase_uGUI>();

		// Token: 0x0400046A RID: 1130
		private List<GlazierBox_uGUI.BoxPoolData> boxPool = new List<GlazierBox_uGUI.BoxPoolData>();

		// Token: 0x0400046B RID: 1131
		private List<GlazierElementBase_uGUI.PoolData> framePool = new List<GlazierElementBase_uGUI.PoolData>();

		// Token: 0x0400046C RID: 1132
		private List<GlazierButton_uGUI.ButtonPoolData> buttonPool = new List<GlazierButton_uGUI.ButtonPoolData>();

		// Token: 0x0400046D RID: 1133
		private List<GlazierImage_uGUI.ImagePoolData> imagePool = new List<GlazierImage_uGUI.ImagePoolData>();

		// Token: 0x0400046E RID: 1134
		private List<GlazierLabel_uGUI.LabelPoolData> labelPool = new List<GlazierLabel_uGUI.LabelPoolData>();

		// Token: 0x0400046F RID: 1135
		private Canvas canvas;

		// Token: 0x04000470 RID: 1136
		private CanvasScaler canvasScaler;

		// Token: 0x04000471 RID: 1137
		private TextMeshProUGUI debugTextComponent;

		// Token: 0x04000472 RID: 1138
		private RectTransform cursorTransform;

		// Token: 0x04000473 RID: 1139
		private RawImage cursorImage;

		// Token: 0x04000474 RID: 1140
		private GameObject tooltipGameObject;

		// Token: 0x04000475 RID: 1141
		private RectTransform tooltipTransform;

		// Token: 0x04000476 RID: 1142
		private TextMeshProUGUI tooltipTextComponent;

		// Token: 0x04000477 RID: 1143
		private Image tooltipShadowImage;

		// Token: 0x04000478 RID: 1144
		private GlazieruGUITooltip lastTooltip;

		// Token: 0x04000479 RID: 1145
		private float startedTooltip;

		// Token: 0x0400047A RID: 1146
		private bool wasCursorVisible;

		// Token: 0x0400047B RID: 1147
		private bool wasCursorLocked;

		// Token: 0x0400047C RID: 1148
		private Material fontMaterial_Default;

		// Token: 0x0400047D RID: 1149
		private Material fontMaterial_Outline;

		// Token: 0x0400047E RID: 1150
		private Material fontMaterial_Shadow;

		// Token: 0x0400047F RID: 1151
		private Material fontMaterial_Tooltip;

		// Token: 0x04000480 RID: 1152
		private static StaticResourceRef<Texture2D> defaultCursor = new StaticResourceRef<Texture2D>("UI/Glazier_uGUI/Cursor");

		// Token: 0x04000481 RID: 1153
		private static CommandLineInt clPixelPerfect = new CommandLineInt("-uGUIPixelPerfect");
	}
}
