using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	/// <summary>
	/// Base class for UIToolkit implementations of primitive building block widgets.
	/// </summary>
	// Token: 0x02000197 RID: 407
	internal abstract class GlazierElementBase_UIToolkit : GlazierElementBase
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00028A86 File Offset: 0x00026C86
		// (set) Token: 0x06000C0F RID: 3087 RVA: 0x00028A8E File Offset: 0x00026C8E
		public Glazier_UIToolkit glazier { get; private set; }

		// Token: 0x170001FA RID: 506
		// (set) Token: 0x06000C10 RID: 3088 RVA: 0x00028A98 File Offset: 0x00026C98
		public override bool IsVisible
		{
			set
			{
				if (this._isVisible != value)
				{
					this._isVisible = value;
					this.visualElement.visible = this._isVisible;
					this.visualElement.style.visibility = (this._isVisible ? 0 : 1);
					this.visualElement.style.display = (this._isVisible ? 0 : 1);
				}
			}
		}

		// Token: 0x170001FB RID: 507
		// (set) Token: 0x06000C11 RID: 3089 RVA: 0x00028B08 File Offset: 0x00026D08
		public override bool UseManualLayout
		{
			set
			{
				this.isTransformDirty |= (this._useManualLayout != value);
				this._useManualLayout = value;
			}
		}

		// Token: 0x170001FC RID: 508
		// (set) Token: 0x06000C12 RID: 3090 RVA: 0x00028B2A File Offset: 0x00026D2A
		public override bool UseWidthLayoutOverride
		{
			set
			{
				this.isTransformDirty |= (this._useWidthLayoutOverride != value);
				this._useWidthLayoutOverride = value;
			}
		}

		// Token: 0x170001FD RID: 509
		// (set) Token: 0x06000C13 RID: 3091 RVA: 0x00028B4C File Offset: 0x00026D4C
		public override bool UseHeightLayoutOverride
		{
			set
			{
				this.isTransformDirty |= (this._useHeightLayoutOverride != value);
				this._useHeightLayoutOverride = value;
			}
		}

		// Token: 0x170001FE RID: 510
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x00028B6E File Offset: 0x00026D6E
		public override ESleekChildLayout UseChildAutoLayout
		{
			set
			{
				base.UseChildAutoLayout = value;
				this.visualElement.style.flexDirection = ((value == 2) ? 2 : 1);
				this.ApplyChildPerpendicularAlignment();
			}
		}

		// Token: 0x170001FF RID: 511
		// (set) Token: 0x06000C15 RID: 3093 RVA: 0x00028B9F File Offset: 0x00026D9F
		public override ESleekChildPerpendicularAlignment ChildPerpendicularAlignment
		{
			set
			{
				base.ChildPerpendicularAlignment = value;
				this.ApplyChildPerpendicularAlignment();
			}
		}

		// Token: 0x17000200 RID: 512
		// (set) Token: 0x06000C16 RID: 3094 RVA: 0x00028BB0 File Offset: 0x00026DB0
		public override bool ExpandChildren
		{
			set
			{
				bool flag = this._expandChildren != value;
				this._expandChildren = value;
				if (flag)
				{
					StyleFloat flexGrow = this._expandChildren ? 1f : 1;
					foreach (GlazierElementBase_UIToolkit glazierElementBase_UIToolkit in this._children)
					{
						glazierElementBase_UIToolkit.visualElement.style.flexGrow = flexGrow;
					}
				}
			}
		}

		// Token: 0x17000201 RID: 513
		// (set) Token: 0x06000C17 RID: 3095 RVA: 0x00028C3C File Offset: 0x00026E3C
		public override bool IgnoreLayout
		{
			set
			{
				this.isTransformDirty |= (this._ignoreLayout != value);
				this._ignoreLayout = value;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x00028C5E File Offset: 0x00026E5E
		public override ISleekElement Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00028C66 File Offset: 0x00026E66
		public GlazierElementBase_UIToolkit(Glazier_UIToolkit glazier)
		{
			this.glazier = glazier;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00028C80 File Offset: 0x00026E80
		public override int FindIndexOfChild(ISleekElement child)
		{
			return this._children.IndexOf((GlazierElementBase_UIToolkit)child.AttachmentRoot);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00028C98 File Offset: 0x00026E98
		public override ISleekElement GetChildAtIndex(int index)
		{
			return this._children[index];
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00028CA8 File Offset: 0x00026EA8
		public override void RemoveChild(ISleekElement child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			GlazierElementBase_UIToolkit glazierElementBase_UIToolkit = child.AttachmentRoot as GlazierElementBase_UIToolkit;
			if (glazierElementBase_UIToolkit != null)
			{
				glazierElementBase_UIToolkit._parent = null;
				glazierElementBase_UIToolkit.InternalDestroy();
				this._children.Remove(glazierElementBase_UIToolkit);
				return;
			}
			UnturnedLog.warn("{0} cannot remove non-UIToolkit element {1}", new object[]
			{
				base.GetType().Name,
				child.AttachmentRoot.GetType().Name
			});
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00028D20 File Offset: 0x00026F20
		public override void RemoveAllChildren()
		{
			foreach (GlazierElementBase_UIToolkit glazierElementBase_UIToolkit in this._children)
			{
				glazierElementBase_UIToolkit._parent = null;
				glazierElementBase_UIToolkit.InternalDestroy();
			}
			this._children.Clear();
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00028D84 File Offset: 0x00026F84
		protected override void UpdateChildren()
		{
			foreach (GlazierElementBase_UIToolkit glazierElementBase_UIToolkit in this._children)
			{
				if (glazierElementBase_UIToolkit.IsVisible)
				{
					glazierElementBase_UIToolkit.Update();
				}
			}
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00028DE0 File Offset: 0x00026FE0
		public override void AddChild(ISleekElement child)
		{
			GlazierElementBase_UIToolkit glazierElementBase_UIToolkit = child.AttachmentRoot as GlazierElementBase_UIToolkit;
			if (glazierElementBase_UIToolkit == null)
			{
				UnturnedLog.warn("{0} cannot add non-UIToolkit element {1}", new object[]
				{
					base.GetType().Name,
					child.AttachmentRoot.GetType().Name
				});
				return;
			}
			if (glazierElementBase_UIToolkit._parent == this)
			{
				return;
			}
			if (glazierElementBase_UIToolkit._parent != null)
			{
				glazierElementBase_UIToolkit._parent._children.Remove(glazierElementBase_UIToolkit);
			}
			this._children.Add(glazierElementBase_UIToolkit);
			glazierElementBase_UIToolkit._parent = this;
			glazierElementBase_UIToolkit.visualElement.style.flexGrow = (this._expandChildren ? 1f : 1);
			this.visualElement.Add(glazierElementBase_UIToolkit.visualElement);
			glazierElementBase_UIToolkit.UpdateDirtyTransform();
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00028EA8 File Offset: 0x000270A8
		public override Vector2 ViewportToNormalizedPosition(Vector2 viewportPosition)
		{
			if (this.visualElement.panel == null)
			{
				return Vector2.zero;
			}
			Rect worldBound = this.visualElement.worldBound;
			if (Mathf.Approximately(worldBound.width, 0f) || Mathf.Approximately(worldBound.height, 0f))
			{
				return Vector2.zero;
			}
			Rect worldBound2 = this.visualElement.panel.visualTree.worldBound;
			return new Vector2((viewportPosition.x * worldBound2.width - worldBound.xMin) / worldBound.width, ((1f - viewportPosition.y) * worldBound2.height - worldBound.yMin) / worldBound.height);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00028F60 File Offset: 0x00027160
		public override Vector2 GetNormalizedCursorPosition()
		{
			if (this.visualElement.panel == null)
			{
				return Vector2.zero;
			}
			Rect worldBound = this.visualElement.panel.visualTree.worldBound;
			if (Mathf.Approximately(worldBound.width, 0f) || Mathf.Approximately(worldBound.height, 0f))
			{
				return Vector2.zero;
			}
			Vector2 normalizedMousePosition = InputEx.NormalizedMousePosition;
			Rect worldBound2 = this.visualElement.worldBound;
			return new Vector2((normalizedMousePosition.x - worldBound2.xMin / worldBound.width) / (worldBound2.width / worldBound.width), (1f - normalizedMousePosition.y - worldBound2.yMin / worldBound.height) / (worldBound2.height / worldBound.height));
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0002902C File Offset: 0x0002722C
		public override Vector2 GetAbsoluteSize()
		{
			if (this.visualElement.panel == null)
			{
				return Vector2.zero;
			}
			Rect worldBound = this.visualElement.panel.visualTree.worldBound;
			if (Mathf.Approximately(worldBound.width, 0f) || Mathf.Approximately(worldBound.height, 0f))
			{
				return Vector2.zero;
			}
			Rect worldBound2 = this.visualElement.worldBound;
			return new Vector2(worldBound2.width / worldBound.width * (float)Screen.width, worldBound2.height / worldBound.height * (float)Screen.height);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x000290CB File Offset: 0x000272CB
		public override void SetAsFirstSibling()
		{
			if (this._parent != null)
			{
				this.visualElement.SendToBack();
				if (this._parent._children.Remove(this))
				{
					this._parent._children.Insert(0, this);
				}
			}
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00029105 File Offset: 0x00027305
		public override void InternalDestroy()
		{
			this.RemoveAllChildren();
			this.visualElement.RemoveFromHierarchy();
			this.glazier.RemoveDestroyedElement(this);
		}

		/// <summary>
		/// Synchronize control colors with background/text/image etc. colors.
		/// Called when custom UI colors are changed, and after constructor.
		/// </summary>
		// Token: 0x06000C25 RID: 3109 RVA: 0x00029124 File Offset: 0x00027324
		internal virtual void SynchronizeColors()
		{
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00029126 File Offset: 0x00027326
		internal virtual bool GetTooltipParameters(out string tooltipText, out Color tooltipColor)
		{
			tooltipText = null;
			tooltipColor = default(Color);
			return false;
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00029134 File Offset: 0x00027334
		protected override void UpdateDirtyTransform()
		{
			this.isTransformDirty = false;
			this.visualElement.style.position = ((this._useManualLayout || this._ignoreLayout) ? 1 : 0);
			if (this._useManualLayout)
			{
				this.visualElement.style.left = Length.Percent(base.PositionScale_X * 100f);
				this.visualElement.style.top = Length.Percent(base.PositionScale_Y * 100f);
				this.visualElement.style.right = Length.Percent((1f - base.SizeScale_X - base.PositionScale_X) * 100f);
				this.visualElement.style.bottom = Length.Percent((1f - base.SizeScale_Y - base.PositionScale_Y) * 100f);
				this.visualElement.style.marginLeft = base.PositionOffset_X;
				this.visualElement.style.marginTop = base.PositionOffset_Y;
				this.visualElement.style.marginRight = -base.PositionOffset_X - base.SizeOffset_X;
				this.visualElement.style.marginBottom = -base.PositionOffset_Y - base.SizeOffset_Y;
				this.visualElement.style.width = 1;
				this.visualElement.style.height = 1;
				return;
			}
			this.visualElement.style.left = 1;
			this.visualElement.style.right = 1;
			this.visualElement.style.top = 1;
			this.visualElement.style.bottom = 1;
			this.visualElement.style.marginLeft = 1;
			this.visualElement.style.marginTop = 1;
			this.visualElement.style.marginRight = 1;
			this.visualElement.style.marginBottom = 1;
			this.visualElement.style.width = (this._useWidthLayoutOverride ? base.SizeOffset_X : 1);
			this.visualElement.style.height = (this._useHeightLayoutOverride ? base.SizeOffset_Y : 1);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x000293E0 File Offset: 0x000275E0
		private void ApplyChildPerpendicularAlignment()
		{
			if (this._useChildAutoLayout == 2)
			{
				switch (this._childPerpendicularAlignment)
				{
				default:
					this.visualElement.style.alignItems = 1;
					return;
				case 1:
					this.visualElement.style.alignItems = 1;
					return;
				case 2:
					this.visualElement.style.alignItems = 3;
					break;
				}
			}
		}

		// Token: 0x0400049D RID: 1181
		public GlazierElementBase_UIToolkit _parent;

		/// <summary>
		/// Set by child.
		/// </summary>
		// Token: 0x0400049E RID: 1182
		public VisualElement visualElement;

		// Token: 0x0400049F RID: 1183
		private List<GlazierElementBase_UIToolkit> _children = new List<GlazierElementBase_UIToolkit>();
	}
}
