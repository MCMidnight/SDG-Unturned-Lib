using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000175 RID: 373
	internal class Glazier_IMGUI : GlazierBase, IGlazier
	{
		// Token: 0x060009E5 RID: 2533 RVA: 0x00021D67 File Offset: 0x0001FF67
		public ISleekBox CreateBox()
		{
			return new GlazierBox_IMGUI();
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00021D6E File Offset: 0x0001FF6E
		public ISleekButton CreateButton()
		{
			return new GlazierButton_IMGUI();
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00021D75 File Offset: 0x0001FF75
		public ISleekElement CreateFrame()
		{
			return new GlazierElementBase_IMGUI();
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00021D7C File Offset: 0x0001FF7C
		public ISleekConstraintFrame CreateConstraintFrame()
		{
			return new GlazierConstraintFrame_IMGUI();
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00021D83 File Offset: 0x0001FF83
		public ISleekImage CreateImage()
		{
			return new GlazierImage_IMGUI(null);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00021D8B File Offset: 0x0001FF8B
		public ISleekImage CreateImage(Texture texture)
		{
			return new GlazierImage_IMGUI(texture);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00021D93 File Offset: 0x0001FF93
		public ISleekSprite CreateSprite()
		{
			return new GlazierSprite_IMGUI(null);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00021D9B File Offset: 0x0001FF9B
		public ISleekSprite CreateSprite(Sprite sprite)
		{
			return new GlazierSprite_IMGUI(sprite);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00021DA3 File Offset: 0x0001FFA3
		public ISleekLabel CreateLabel()
		{
			return new GlazierLabel_IMGUI();
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00021DAA File Offset: 0x0001FFAA
		public ISleekScrollView CreateScrollView()
		{
			return new GlazierScrollView_IMGUI();
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00021DB1 File Offset: 0x0001FFB1
		public ISleekSlider CreateSlider()
		{
			return new GlazierSlider_IMGUI();
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00021DB8 File Offset: 0x0001FFB8
		public ISleekField CreateStringField()
		{
			return new GlazierStringField_IMGUI();
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00021DBF File Offset: 0x0001FFBF
		public ISleekToggle CreateToggle()
		{
			return new GlazierToggle_IMGUI();
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00021DC6 File Offset: 0x0001FFC6
		public ISleekUInt8Field CreateUInt8Field()
		{
			return new GlazierUInt8Field_IMGUI();
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00021DCD File Offset: 0x0001FFCD
		public ISleekUInt16Field CreateUInt16Field()
		{
			return new GlazierUInt16Field_IMGUI();
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00021DD4 File Offset: 0x0001FFD4
		public ISleekUInt32Field CreateUInt32Field()
		{
			return new GlazierUInt32Field_IMGUI();
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00021DDB File Offset: 0x0001FFDB
		public ISleekInt32Field CreateInt32Field()
		{
			return new GlazierInt32Field_IMGUI();
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00021DE2 File Offset: 0x0001FFE2
		public ISleekFloat32Field CreateFloat32Field()
		{
			return new GlazierFloat32Field_IMGUI();
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00021DE9 File Offset: 0x0001FFE9
		public ISleekFloat64Field CreateFloat64Field()
		{
			return new GlazierFloat64Field_IMGUI();
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00021DF0 File Offset: 0x0001FFF0
		public ISleekElement CreateProxyImplementation(SleekWrapper owner)
		{
			return new GlazierProxy_IMGUI(owner);
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x00021DF8 File Offset: 0x0001FFF8
		public bool SupportsDepth
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x00021DFB File Offset: 0x0001FFFB
		public bool SupportsRichTextAlpha
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x00021DFE File Offset: 0x0001FFFE
		public bool SupportsAutomaticLayout
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x00021E01 File Offset: 0x00020001
		// (set) Token: 0x060009FD RID: 2557 RVA: 0x00021E0C File Offset: 0x0002000C
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
				this._root = value;
				if (this._root == null)
				{
					this.rootImpl = null;
					return;
				}
				this.rootImpl = (this._root.AttachmentRoot as GlazierElementBase_IMGUI);
				if (this.rootImpl != null)
				{
					this.rootImpl.isTransformDirty = true;
					return;
				}
				UnturnedLog.warn("Root must be an IMGUI element: {0}", new object[]
				{
					this._root.GetType().Name
				});
			}
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00021E88 File Offset: 0x00020088
		public static Glazier_IMGUI CreateGlazier()
		{
			GameObject gameObject = new GameObject("Glazier");
			Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<Glazier_IMGUI>();
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00021EA0 File Offset: 0x000200A0
		private void LateUpdate()
		{
			if (this.rootImpl != null)
			{
				if (Screen.width != this.cachedScreenWidth || Screen.height != this.cachedScreenHeight)
				{
					this.cachedScreenWidth = Screen.width;
					this.cachedScreenHeight = Screen.height;
					this.rootImpl.isTransformDirty = true;
				}
				this.rootImpl.Update();
			}
			if (OptionsSettings.debug)
			{
				base.UpdateDebugStats();
				base.UpdateDebugString();
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00021F10 File Offset: 0x00020110
		private void CursorOnGUI()
		{
			if (!this.Root.ShouldDrawCursor)
			{
				return;
			}
			Rect rect = new Rect(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y, 20f, 20f);
			GUI.color = SleekCustomization.cursorColor;
			GUI.DrawTexture(rect, Glazier_IMGUI.defaultCursor);
			GUI.color = Color.white;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00021F78 File Offset: 0x00020178
		private void TooltipOnGUI()
		{
			if (!this.Root.ShouldDrawTooltip)
			{
				return;
			}
			string tooltip = GUI.tooltip;
			if (tooltip != this.lastTooltip)
			{
				this.lastTooltip = tooltip;
				this.startedTooltip = Time.realtimeSinceStartup;
				this.tooltipContent = new GUIContent(tooltip);
				this.tooltipShadowContent = RichTextUtil.makeShadowContent(this.tooltipContent);
			}
			if (!string.IsNullOrWhiteSpace(tooltip) && Time.realtimeSinceStartup - this.startedTooltip > 0.5f)
			{
				Rect area = new Rect(0f, (float)Screen.height - Input.mousePosition.y, 400f, 200f);
				Color fontColor = OptionsSettings.fontColor;
				if (Input.mousePosition.x > (float)Screen.width - area.width - 30f)
				{
					area.x = Input.mousePosition.x - 30f - area.width;
					GlazierUtils_IMGUI.drawLabel(area, 1, 2, 12, this.tooltipShadowContent, fontColor, this.tooltipContent, 3);
					return;
				}
				area.x = Input.mousePosition.x + 30f;
				GlazierUtils_IMGUI.drawLabel(area, 1, 0, 12, this.tooltipShadowContent, fontColor, this.tooltipContent, 3);
			}
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x000220AB File Offset: 0x000202AB
		protected override void OnEnable()
		{
			base.OnEnable();
			base.useGUILayout = false;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x000220BC File Offset: 0x000202BC
		private void OnGUI()
		{
			GUI.skin = GlazierResources_IMGUI.ActiveSkin;
			if (this._root != null && this._root.isEnabled && this.rootImpl != null)
			{
				this.rootImpl.OnGUI();
			}
			if (Event.current.type == 7)
			{
				if (OptionsSettings.debug)
				{
					GlazierUtils_IMGUI.drawLabel(new Rect(0f, 0f, 800f, 30f), 0, 0, 12, false, base.debugStringColor, base.debugString, 2);
				}
				this.CursorOnGUI();
				this.TooltipOnGUI();
			}
		}

		// Token: 0x040003D4 RID: 980
		private SleekWindow _root;

		// Token: 0x040003D5 RID: 981
		private GlazierElementBase_IMGUI rootImpl;

		// Token: 0x040003D6 RID: 982
		private int cachedScreenWidth = -1;

		// Token: 0x040003D7 RID: 983
		private int cachedScreenHeight = -1;

		// Token: 0x040003D8 RID: 984
		private string lastTooltip;

		// Token: 0x040003D9 RID: 985
		private float startedTooltip;

		// Token: 0x040003DA RID: 986
		private GUIContent tooltipContent;

		// Token: 0x040003DB RID: 987
		private GUIContent tooltipShadowContent;

		// Token: 0x040003DC RID: 988
		private static StaticResourceRef<Texture2D> defaultCursor = new StaticResourceRef<Texture2D>("UI/Glazier_IMGUI/Cursor");
	}
}
