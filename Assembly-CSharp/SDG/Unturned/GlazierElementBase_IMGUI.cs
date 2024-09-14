using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Base class for IMGUI implementations of primitive building block widgets.
	/// </summary>
	// Token: 0x02000163 RID: 355
	internal class GlazierElementBase_IMGUI : GlazierElementBase
	{
		// Token: 0x060008E5 RID: 2277 RVA: 0x0001F189 File Offset: 0x0001D389
		public virtual void OnGUI()
		{
			this.ChildrenOnGUI();
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0001F191 File Offset: 0x0001D391
		public override int FindIndexOfChild(ISleekElement child)
		{
			return this._children.IndexOf((GlazierElementBase_IMGUI)child.AttachmentRoot);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001F1A9 File Offset: 0x0001D3A9
		public override ISleekElement GetChildAtIndex(int index)
		{
			return this._children[index];
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001F1B8 File Offset: 0x0001D3B8
		public override void RemoveChild(ISleekElement child)
		{
			GlazierElementBase_IMGUI glazierElementBase_IMGUI = child.AttachmentRoot as GlazierElementBase_IMGUI;
			if (glazierElementBase_IMGUI != null)
			{
				glazierElementBase_IMGUI._parent = null;
				glazierElementBase_IMGUI.InternalDestroy();
				this._children.Remove(glazierElementBase_IMGUI);
				return;
			}
			UnturnedLog.warn("{0} cannot remove non-IMGUI element {1}", new object[]
			{
				base.GetType().Name,
				child.AttachmentRoot.GetType().Name
			});
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001F220 File Offset: 0x0001D420
		public override void RemoveAllChildren()
		{
			foreach (GlazierElementBase_IMGUI glazierElementBase_IMGUI in this._children)
			{
				glazierElementBase_IMGUI._parent = null;
				glazierElementBase_IMGUI.InternalDestroy();
			}
			this._children.Clear();
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001F284 File Offset: 0x0001D484
		protected override void UpdateChildren()
		{
			foreach (GlazierElementBase_IMGUI glazierElementBase_IMGUI in this._children)
			{
				if (glazierElementBase_IMGUI.IsVisible)
				{
					glazierElementBase_IMGUI.Update();
				}
			}
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001F2E0 File Offset: 0x0001D4E0
		public override void AddChild(ISleekElement child)
		{
			GlazierElementBase_IMGUI glazierElementBase_IMGUI = child.AttachmentRoot as GlazierElementBase_IMGUI;
			if (glazierElementBase_IMGUI == null)
			{
				UnturnedLog.warn("{0} cannot add non-IMGUI element {1}", new object[]
				{
					base.GetType().Name,
					child.AttachmentRoot.GetType().Name
				});
				return;
			}
			if (glazierElementBase_IMGUI._parent == this)
			{
				return;
			}
			if (glazierElementBase_IMGUI._parent != null)
			{
				glazierElementBase_IMGUI._parent._children.Remove(glazierElementBase_IMGUI);
			}
			this._children.Add(glazierElementBase_IMGUI);
			glazierElementBase_IMGUI._parent = this;
			glazierElementBase_IMGUI.UpdateDirtyTransform();
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001F36B File Offset: 0x0001D56B
		public override void InternalDestroy()
		{
			this.RemoveAllChildren();
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001F374 File Offset: 0x0001D574
		public override Vector2 ViewportToNormalizedPosition(Vector2 viewportPosition)
		{
			Rect drawRectInScreenSpace = this.GetDrawRectInScreenSpace();
			Vector2 result;
			if (drawRectInScreenSpace.width > 0f)
			{
				result.x = (viewportPosition.x * (float)Screen.width - drawRectInScreenSpace.xMin) / drawRectInScreenSpace.width;
			}
			else
			{
				result.x = 0.5f;
			}
			if (drawRectInScreenSpace.height > 0f)
			{
				result.y = ((1f - viewportPosition.y) * (float)Screen.height - drawRectInScreenSpace.yMin) / drawRectInScreenSpace.height;
			}
			else
			{
				result.y = 0.5f;
			}
			return result;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001F410 File Offset: 0x0001D610
		public override Vector2 GetNormalizedCursorPosition()
		{
			Vector2 vector = Input.mousePosition;
			Rect drawRectInScreenSpace = this.GetDrawRectInScreenSpace();
			Vector2 result;
			if (drawRectInScreenSpace.width > 0f)
			{
				result.x = (vector.x - drawRectInScreenSpace.xMin) / drawRectInScreenSpace.width;
			}
			else
			{
				result.x = 0.5f;
			}
			if (drawRectInScreenSpace.height > 0f)
			{
				result.y = ((float)Screen.height - vector.y - drawRectInScreenSpace.yMin) / drawRectInScreenSpace.height;
			}
			else
			{
				result.y = 0.5f;
			}
			return result;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0001F4AC File Offset: 0x0001D6AC
		public override Vector2 GetAbsoluteSize()
		{
			return this.GetDrawRectInScreenSpace().size;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0001F4C7 File Offset: 0x0001D6C7
		public override void SetAsFirstSibling()
		{
			if (this._parent != null && this._parent._children.Remove(this))
			{
				this._parent._children.Insert(0, this);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x0001F4F6 File Offset: 0x0001D6F6
		public override ISleekElement Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0001F4FE File Offset: 0x0001D6FE
		protected virtual void TransformChildDrawPositionIntoParentSpace(ref Vector2 position)
		{
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001F500 File Offset: 0x0001D700
		protected Rect GetDrawRectInScreenSpace()
		{
			Rect result = this.drawRect;
			Vector2 position = result.position;
			for (GlazierElementBase_IMGUI parent = this._parent; parent != null; parent = parent._parent)
			{
				parent.TransformChildDrawPositionIntoParentSpace(ref position);
			}
			result.position = position;
			return result;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001F540 File Offset: 0x0001D740
		protected virtual Rect GetLayoutRect()
		{
			return this.drawRect;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001F548 File Offset: 0x0001D748
		protected virtual Rect CalculateDrawRect()
		{
			if (this._parent != null)
			{
				float userInterfaceScale = GraphicsSettings.userInterfaceScale;
				Rect layoutRect = this._parent.GetLayoutRect();
				layoutRect.x += base.PositionOffset_X * userInterfaceScale + layoutRect.width * base.PositionScale_X;
				layoutRect.y += base.PositionOffset_Y * userInterfaceScale + layoutRect.height * base.PositionScale_Y;
				layoutRect.width = base.SizeOffset_X * userInterfaceScale + layoutRect.width * base.SizeScale_X;
				layoutRect.height = base.SizeOffset_Y * userInterfaceScale + layoutRect.height * base.SizeScale_Y;
				return layoutRect;
			}
			if (Screen.width == 5760 && Screen.height == 1080)
			{
				return new Rect(1920f, 0f, 1920f, 1080f);
			}
			return new Rect(base.PositionOffset_X, base.PositionOffset_Y, (float)Screen.width, (float)Screen.height);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001F648 File Offset: 0x0001D848
		protected override void UpdateDirtyTransform()
		{
			this.isTransformDirty = false;
			this.drawRect = this.CalculateDrawRect();
			foreach (GlazierElementBase_IMGUI glazierElementBase_IMGUI in this._children)
			{
				glazierElementBase_IMGUI.isTransformDirty = true;
			}
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001F6AC File Offset: 0x0001D8AC
		protected void ChildrenOnGUI()
		{
			for (int i = 0; i < this._children.Count; i++)
			{
				GlazierElementBase_IMGUI glazierElementBase_IMGUI = this._children[i];
				if (glazierElementBase_IMGUI.IsVisible)
				{
					glazierElementBase_IMGUI.OnGUI();
				}
			}
		}

		// Token: 0x04000368 RID: 872
		public GlazierElementBase_IMGUI _parent;

		/// <summary>
		/// Position passed into the GUI draw methods.
		/// </summary>
		// Token: 0x04000369 RID: 873
		public Rect drawRect;

		// Token: 0x0400036A RID: 874
		private List<GlazierElementBase_IMGUI> _children = new List<GlazierElementBase_IMGUI>();
	}
}
