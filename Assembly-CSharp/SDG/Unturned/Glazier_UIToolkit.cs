using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x020001AA RID: 426
	internal class Glazier_UIToolkit : GlazierBase, IGlazier
	{
		// Token: 0x06000D28 RID: 3368 RVA: 0x0002BF3C File Offset: 0x0002A13C
		public ISleekBox CreateBox()
		{
			GlazierBox_UIToolkit glazierBox_UIToolkit = new GlazierBox_UIToolkit(this);
			this.liveElements.Add(glazierBox_UIToolkit);
			glazierBox_UIToolkit.SynchronizeColors();
			return glazierBox_UIToolkit;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0002BF64 File Offset: 0x0002A164
		public ISleekButton CreateButton()
		{
			GlazierButton_UIToolkit glazierButton_UIToolkit = new GlazierButton_UIToolkit(this);
			this.liveElements.Add(glazierButton_UIToolkit);
			glazierButton_UIToolkit.SynchronizeColors();
			return glazierButton_UIToolkit;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0002BF8C File Offset: 0x0002A18C
		public ISleekElement CreateFrame()
		{
			GlazierEmpty_UIToolkit glazierEmpty_UIToolkit = new GlazierEmpty_UIToolkit(this);
			this.liveElements.Add(glazierEmpty_UIToolkit);
			glazierEmpty_UIToolkit.SynchronizeColors();
			return glazierEmpty_UIToolkit;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0002BFB4 File Offset: 0x0002A1B4
		public ISleekConstraintFrame CreateConstraintFrame()
		{
			GlazierConstraintFrame_UIToolkit glazierConstraintFrame_UIToolkit = new GlazierConstraintFrame_UIToolkit(this);
			this.liveElements.Add(glazierConstraintFrame_UIToolkit);
			glazierConstraintFrame_UIToolkit.SynchronizeColors();
			return glazierConstraintFrame_UIToolkit;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0002BFDC File Offset: 0x0002A1DC
		public ISleekImage CreateImage()
		{
			return this.CreateImage(null);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0002BFE8 File Offset: 0x0002A1E8
		public ISleekImage CreateImage(Texture texture)
		{
			GlazierImage_UIToolkit glazierImage_UIToolkit = new GlazierImage_UIToolkit(this);
			this.liveElements.Add(glazierImage_UIToolkit);
			glazierImage_UIToolkit.Texture = texture;
			glazierImage_UIToolkit.SynchronizeColors();
			return glazierImage_UIToolkit;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0002C017 File Offset: 0x0002A217
		public ISleekSprite CreateSprite()
		{
			return this.CreateSprite(null);
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0002C020 File Offset: 0x0002A220
		public ISleekSprite CreateSprite(Sprite sprite)
		{
			GlazierSprite_UIToolkit glazierSprite_UIToolkit = new GlazierSprite_UIToolkit(this);
			this.liveElements.Add(glazierSprite_UIToolkit);
			glazierSprite_UIToolkit.Sprite = sprite;
			glazierSprite_UIToolkit.SynchronizeColors();
			return glazierSprite_UIToolkit;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0002C050 File Offset: 0x0002A250
		public ISleekLabel CreateLabel()
		{
			GlazierLabel_UIToolkit glazierLabel_UIToolkit = new GlazierLabel_UIToolkit(this);
			this.liveElements.Add(glazierLabel_UIToolkit);
			glazierLabel_UIToolkit.SynchronizeColors();
			return glazierLabel_UIToolkit;
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0002C078 File Offset: 0x0002A278
		public ISleekScrollView CreateScrollView()
		{
			GlazierScrollView_UIToolkit glazierScrollView_UIToolkit = new GlazierScrollView_UIToolkit(this);
			this.liveElements.Add(glazierScrollView_UIToolkit);
			glazierScrollView_UIToolkit.SynchronizeColors();
			return glazierScrollView_UIToolkit;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0002C0A0 File Offset: 0x0002A2A0
		public ISleekSlider CreateSlider()
		{
			GlazierSlider_UIToolkit glazierSlider_UIToolkit = new GlazierSlider_UIToolkit(this);
			this.liveElements.Add(glazierSlider_UIToolkit);
			glazierSlider_UIToolkit.SynchronizeColors();
			return glazierSlider_UIToolkit;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0002C0C8 File Offset: 0x0002A2C8
		public ISleekField CreateStringField()
		{
			GlazierStringField_UIToolkit glazierStringField_UIToolkit = new GlazierStringField_UIToolkit(this);
			this.liveElements.Add(glazierStringField_UIToolkit);
			glazierStringField_UIToolkit.SynchronizeColors();
			return glazierStringField_UIToolkit;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0002C0F0 File Offset: 0x0002A2F0
		public ISleekToggle CreateToggle()
		{
			GlazierToggle_UIToolkit glazierToggle_UIToolkit = new GlazierToggle_UIToolkit(this);
			this.liveElements.Add(glazierToggle_UIToolkit);
			glazierToggle_UIToolkit.SynchronizeColors();
			return glazierToggle_UIToolkit;
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0002C118 File Offset: 0x0002A318
		public ISleekUInt8Field CreateUInt8Field()
		{
			GlazierUInt8Field_UIToolkit glazierUInt8Field_UIToolkit = new GlazierUInt8Field_UIToolkit(this);
			this.liveElements.Add(glazierUInt8Field_UIToolkit);
			glazierUInt8Field_UIToolkit.SynchronizeColors();
			return glazierUInt8Field_UIToolkit;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0002C140 File Offset: 0x0002A340
		public ISleekUInt16Field CreateUInt16Field()
		{
			GlazierUInt16Field_UIToolkit glazierUInt16Field_UIToolkit = new GlazierUInt16Field_UIToolkit(this);
			this.liveElements.Add(glazierUInt16Field_UIToolkit);
			glazierUInt16Field_UIToolkit.SynchronizeColors();
			return glazierUInt16Field_UIToolkit;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0002C168 File Offset: 0x0002A368
		public ISleekUInt32Field CreateUInt32Field()
		{
			GlazierUInt32Field_UIToolkit glazierUInt32Field_UIToolkit = new GlazierUInt32Field_UIToolkit(this);
			this.liveElements.Add(glazierUInt32Field_UIToolkit);
			glazierUInt32Field_UIToolkit.SynchronizeColors();
			return glazierUInt32Field_UIToolkit;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0002C190 File Offset: 0x0002A390
		public ISleekInt32Field CreateInt32Field()
		{
			GlazierInt32Field_UIToolkit glazierInt32Field_UIToolkit = new GlazierInt32Field_UIToolkit(this);
			this.liveElements.Add(glazierInt32Field_UIToolkit);
			glazierInt32Field_UIToolkit.SynchronizeColors();
			return glazierInt32Field_UIToolkit;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0002C1B8 File Offset: 0x0002A3B8
		public ISleekFloat32Field CreateFloat32Field()
		{
			GlazierFloat32Field_UIToolkit glazierFloat32Field_UIToolkit = new GlazierFloat32Field_UIToolkit(this);
			this.liveElements.Add(glazierFloat32Field_UIToolkit);
			glazierFloat32Field_UIToolkit.SynchronizeColors();
			return glazierFloat32Field_UIToolkit;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0002C1E0 File Offset: 0x0002A3E0
		public ISleekFloat64Field CreateFloat64Field()
		{
			GlazierFloat64Field_UIToolkit glazierFloat64Field_UIToolkit = new GlazierFloat64Field_UIToolkit(this);
			this.liveElements.Add(glazierFloat64Field_UIToolkit);
			glazierFloat64Field_UIToolkit.SynchronizeColors();
			return glazierFloat64Field_UIToolkit;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0002C208 File Offset: 0x0002A408
		public ISleekElement CreateProxyImplementation(SleekWrapper owner)
		{
			GlazierProxy_UIToolkit glazierProxy_UIToolkit = new GlazierProxy_UIToolkit(this, owner);
			this.liveElements.Add(glazierProxy_UIToolkit);
			glazierProxy_UIToolkit.SynchronizeColors();
			return glazierProxy_UIToolkit;
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x0002C231 File Offset: 0x0002A431
		public override bool ShouldGameProcessKeyDown
		{
			get
			{
				return base.ShouldGameProcessKeyDown && !(this.focusController.focusedElement is TextField);
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x0002C253 File Offset: 0x0002A453
		public bool SupportsDepth
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x0002C256 File Offset: 0x0002A456
		public bool SupportsRichTextAlpha
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x0002C259 File Offset: 0x0002A459
		public bool SupportsAutomaticLayout
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x0002C25C File Offset: 0x0002A45C
		// (set) Token: 0x06000D41 RID: 3393 RVA: 0x0002C264 File Offset: 0x0002A464
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
				if (this.rootImpl != null)
				{
					this.gameLayer.Remove(this.rootImpl.visualElement);
				}
				this._root = value;
				if (this._root == null)
				{
					this.rootImpl = null;
					return;
				}
				this.rootImpl = (this._root.AttachmentRoot as GlazierElementBase_UIToolkit);
				if (this.rootImpl != null)
				{
					this.gameLayer.Add(this.rootImpl.visualElement);
					return;
				}
				UnturnedLog.warn("Root must be a UIToolkit element: {0}", new object[]
				{
					this._root.GetType().Name
				});
			}
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0002C308 File Offset: 0x0002A508
		public static Glazier_UIToolkit CreateGlazier()
		{
			GameObject gameObject = new GameObject("Glazier");
			Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<Glazier_UIToolkit>();
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0002C320 File Offset: 0x0002A520
		protected override void OnEnable()
		{
			base.OnEnable();
			OptionsSettings.OnThemeChanged += new Action(this.OnThemeChanged);
			OptionsSettings.OnCustomColorsChanged += new Action(this.OnCustomColorsChanged);
			this.document = base.gameObject.AddComponent<UIDocument>();
			this.document.panelSettings = Resources.Load<PanelSettings>("UI/Glazier_UIToolkit/PanelSettings");
			this.document.panelSettings.themeStyleSheet = GlazierResources_UIToolkit.Theme;
			this.document.visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/Glazier_UIToolkit/DefaultVisualTree");
			this.focusController = this.document.rootVisualElement.focusController;
			this.gameLayer = new VisualElement();
			this.gameLayer.AddToClassList("unturned-glazier-layer");
			this.gameLayer.pickingMode = 1;
			this.document.rootVisualElement.Add(this.gameLayer);
			this.overlayLayer = new VisualElement();
			this.overlayLayer.AddToClassList("unturned-glazier-layer");
			this.overlayLayer.pickingMode = 1;
			this.document.rootVisualElement.Add(this.overlayLayer);
			this.CreateDebugLabel();
			this.CreateCursor();
			this.CreateTooltip();
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0002C448 File Offset: 0x0002A648
		protected void LateUpdate()
		{
			float userInterfaceScale = GraphicsSettings.userInterfaceScale;
			if (MathfEx.IsNearlyEqual(userInterfaceScale, 1f, 0.001f))
			{
				this.document.panelSettings.scale = 1f;
			}
			else
			{
				this.document.panelSettings.scale = userInterfaceScale;
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
						if (this.focusController.focusedElement != null)
						{
							this.focusController.focusedElement.Blur();
						}
					}
				}
				this.rootImpl.IsVisible = this.Root.isEnabled;
			}
			this.UpdateDebugLabel();
			this.UpdateCursor();
			this.UpdateTooltip();
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0002C517 File Offset: 0x0002A717
		internal void RemoveDestroyedElement(GlazierElementBase_UIToolkit element)
		{
			this.liveElements.Remove(element);
		}

		/// <summary>
		/// Sanity check all returned elements have a gameObject.
		/// </summary>
		// Token: 0x06000D46 RID: 3398 RVA: 0x0002C526 File Offset: 0x0002A726
		[Conditional("VALIDATE_GLAZIER_USE_AFTER_DESTROY")]
		private void ValidateNewElement(GlazierElementBase_UIToolkit element)
		{
			if (element.visualElement == null)
			{
				throw new Exception("UIToolkit element constructed with null visual element");
			}
		}

		/// <summary>
		/// Create software cursor visual element.
		/// </summary>
		// Token: 0x06000D47 RID: 3399 RVA: 0x0002C53B File Offset: 0x0002A73B
		private void CreateCursor()
		{
			this.cursorImage = new Image();
			this.cursorImage.AddToClassList("unturned-cursor");
			this.cursorImage.pickingMode = 1;
			this.overlayLayer.Add(this.cursorImage);
		}

		/// <summary>
		/// Create green label in the upper-left.
		/// </summary>
		// Token: 0x06000D48 RID: 3400 RVA: 0x0002C578 File Offset: 0x0002A778
		private void CreateDebugLabel()
		{
			this.debugLabel = new Label();
			this.debugLabel.AddToClassList("unturned-debug");
			this.debugLabel.pickingMode = 1;
			this.debugLabel.visible = false;
			GlazierUtils_UIToolkit.ApplyTextContrast(this.debugLabel.style, 2, 1f);
			this.overlayLayer.Add(this.debugLabel);
		}

		/// <summary>
		/// Create tooltip visual element.
		/// </summary>
		// Token: 0x06000D49 RID: 3401 RVA: 0x0002C5E0 File Offset: 0x0002A7E0
		private void CreateTooltip()
		{
			this.tooltipLabel = new Label();
			this.tooltipLabel.AddToClassList("unturned-tooltip");
			this.tooltipLabel.pickingMode = 1;
			this.tooltipLabel.visible = false;
			GlazierUtils_UIToolkit.ApplyTextContrast(this.debugLabel.style, 3, 1f);
			this.overlayLayer.Add(this.tooltipLabel);
			this.SynchronizeTooltipShadowColor();
		}

		/// <summary>
		/// Update upper-left green text.
		/// </summary>
		// Token: 0x06000D4A RID: 3402 RVA: 0x0002C650 File Offset: 0x0002A850
		private void UpdateDebugLabel()
		{
			if (OptionsSettings.debug && this._root != null && (this._root.isEnabled || this._root.drawCursorWhileDisabled))
			{
				base.UpdateDebugStats();
				base.UpdateDebugString();
				this.debugLabel.style.color = base.debugStringColor;
				this.debugLabel.text = base.debugString;
				this.debugLabel.visible = true;
				return;
			}
			this.debugLabel.visible = false;
		}

		/// <summary>
		/// Update software cursor visual element.
		/// </summary>
		// Token: 0x06000D4B RID: 3403 RVA: 0x0002C6D8 File Offset: 0x0002A8D8
		private void UpdateCursor()
		{
			this.cursorImage.visible = this.Root.ShouldDrawCursor;
			this.cursorImage.style.unityBackgroundImageTintColor = SleekCustomization.cursorColor;
			Vector2 normalizedMousePosition = InputEx.NormalizedMousePosition;
			normalizedMousePosition.y = 1f - normalizedMousePosition.y;
			this.cursorImage.style.left = Length.Percent(normalizedMousePosition.x * 100f);
			this.cursorImage.style.top = Length.Percent(normalizedMousePosition.y * 100f);
		}

		/// <summary>
		/// Find hovered element and update tooltip visibility/text.
		/// </summary>
		// Token: 0x06000D4C RID: 3404 RVA: 0x0002C77C File Offset: 0x0002A97C
		private void UpdateTooltip()
		{
			Vector2 vector = Input.mousePosition;
			vector.y = (float)Screen.height - vector.y;
			IPanel panel = this.document.rootVisualElement.panel;
			Vector2 vector2 = RuntimePanelUtils.ScreenToPanel(panel, vector);
			VisualElement visualElement = panel.Pick(vector2);
			object obj;
			if (visualElement != null)
			{
				obj = visualElement.userData;
				if (obj == null)
				{
					obj = visualElement.FindAncestorUserData();
				}
			}
			else
			{
				obj = null;
			}
			GlazierElementBase_UIToolkit glazierElementBase_UIToolkit = obj as GlazierElementBase_UIToolkit;
			if (glazierElementBase_UIToolkit != this.previousTooltipElement)
			{
				this.previousTooltipElement = glazierElementBase_UIToolkit;
				this.tooltipFocusTimer = 0f;
			}
			if (glazierElementBase_UIToolkit != null)
			{
				this.tooltipFocusTimer += Time.unscaledDeltaTime;
			}
			string text;
			Color color;
			if (this.Root.ShouldDrawTooltip && glazierElementBase_UIToolkit != null && this.tooltipFocusTimer >= 0.5f && glazierElementBase_UIToolkit.GetTooltipParameters(out text, out color) && !string.IsNullOrEmpty(text))
			{
				Vector2 normalizedMousePosition = InputEx.NormalizedMousePosition;
				normalizedMousePosition.y = 1f - normalizedMousePosition.y;
				this.tooltipLabel.style.top = Length.Percent(normalizedMousePosition.y * 100f);
				if (normalizedMousePosition.x > 0.7f)
				{
					this.tooltipLabel.style.left = 1;
					this.tooltipLabel.style.right = Length.Percent((1f - normalizedMousePosition.x) * 100f);
					this.tooltipLabel.style.marginLeft = 0f;
					this.tooltipLabel.style.marginRight = 10f;
					this.tooltipLabel.style.unityTextAlign = 2;
				}
				else
				{
					this.tooltipLabel.style.left = Length.Percent(normalizedMousePosition.x * 100f);
					this.tooltipLabel.style.right = 1;
					this.tooltipLabel.style.marginLeft = 30f;
					this.tooltipLabel.style.marginRight = 0f;
					this.tooltipLabel.style.unityTextAlign = 0;
				}
				this.tooltipLabel.text = text;
				this.tooltipLabel.style.color = color;
				this.tooltipLabel.visible = true;
				return;
			}
			this.tooltipLabel.visible = false;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0002CA08 File Offset: 0x0002AC08
		private void SynchronizeTooltipShadowColor()
		{
			Color shadowColor = SleekCustomization.shadowColor;
			shadowColor.a = 0.5f;
			this.tooltipLabel.style.unityBackgroundImageTintColor = shadowColor;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0002CA3D File Offset: 0x0002AC3D
		private void OnThemeChanged()
		{
			this.document.panelSettings.themeStyleSheet = GlazierResources_UIToolkit.Theme;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0002CA54 File Offset: 0x0002AC54
		private void OnCustomColorsChanged()
		{
			foreach (GlazierElementBase_UIToolkit glazierElementBase_UIToolkit in this.liveElements)
			{
				glazierElementBase_UIToolkit.SynchronizeColors();
			}
			this.SynchronizeTooltipShadowColor();
		}

		// Token: 0x040004FF RID: 1279
		private SleekWindow _root;

		// Token: 0x04000500 RID: 1280
		private GlazierElementBase_UIToolkit rootImpl;

		// Token: 0x04000501 RID: 1281
		private UIDocument document;

		// Token: 0x04000502 RID: 1282
		private FocusController focusController;

		// Token: 0x04000503 RID: 1283
		private HashSet<GlazierElementBase_UIToolkit> liveElements = new HashSet<GlazierElementBase_UIToolkit>();

		/// <summary>
		/// Container for SleekWindow element.
		/// </summary>
		// Token: 0x04000504 RID: 1284
		private VisualElement gameLayer;

		/// <summary>
		/// Container for top-level visual elements.
		/// </summary>
		// Token: 0x04000505 RID: 1285
		private VisualElement overlayLayer;

		// Token: 0x04000506 RID: 1286
		private Label debugLabel;

		// Token: 0x04000507 RID: 1287
		private Image cursorImage;

		// Token: 0x04000508 RID: 1288
		private Label tooltipLabel;

		/// <summary>
		/// Element under the cursor on the previous frame.
		/// </summary>
		// Token: 0x04000509 RID: 1289
		private GlazierElementBase_UIToolkit previousTooltipElement;

		/// <summary>
		/// Duration in seconds the cursor has been over the element.
		/// </summary>
		// Token: 0x0400050A RID: 1290
		private float tooltipFocusTimer;

		// Token: 0x0400050B RID: 1291
		private bool wasCursorLocked;
	}
}
