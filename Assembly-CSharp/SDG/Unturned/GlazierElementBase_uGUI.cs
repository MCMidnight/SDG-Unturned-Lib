using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Unturned
{
	/// <summary>
	/// Base class for uGUI implementations of primitive building block widgets.
	/// </summary>
	// Token: 0x0200017B RID: 379
	internal abstract class GlazierElementBase_uGUI : GlazierElementBase
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x00022F8F File Offset: 0x0002118F
		// (set) Token: 0x06000A51 RID: 2641 RVA: 0x00022F97 File Offset: 0x00021197
		public Glazier_uGUI glazier { get; private set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x00022FA0 File Offset: 0x000211A0
		// (set) Token: 0x06000A53 RID: 2643 RVA: 0x00022FA8 File Offset: 0x000211A8
		public override bool IsVisible
		{
			get
			{
				return base.IsVisible;
			}
			set
			{
				if (this._isVisible != value)
				{
					this._isVisible = value;
					this.gameObject.SetActive(value);
				}
			}
		}

		// Token: 0x17000185 RID: 389
		// (set) Token: 0x06000A54 RID: 2644 RVA: 0x00022FC6 File Offset: 0x000211C6
		public override bool UseManualLayout
		{
			set
			{
				this.isTransformDirty |= (this._useManualLayout != value);
				this._useManualLayout = value;
			}
		}

		// Token: 0x17000186 RID: 390
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x00022FE8 File Offset: 0x000211E8
		public override bool UseWidthLayoutOverride
		{
			set
			{
				bool flag = this._useWidthLayoutOverride != value;
				this.isTransformDirty = (this.isTransformDirty || flag);
				this._useWidthLayoutOverride = value;
				if (flag)
				{
					if (this.ShouldHaveLayoutElementComponent)
					{
						this.transform.GetOrAddComponent<LayoutElement>();
						return;
					}
					this.transform.DestroyComponentIfExists<LayoutElement>();
				}
			}
		}

		// Token: 0x17000187 RID: 391
		// (set) Token: 0x06000A56 RID: 2646 RVA: 0x0002303C File Offset: 0x0002123C
		public override bool UseHeightLayoutOverride
		{
			set
			{
				bool flag = this._useHeightLayoutOverride != value;
				this.isTransformDirty = (this.isTransformDirty || flag);
				this._useHeightLayoutOverride = value;
				if (flag)
				{
					if (this.ShouldHaveLayoutElementComponent)
					{
						this.transform.GetOrAddComponent<LayoutElement>();
						return;
					}
					this.transform.DestroyComponentIfExists<LayoutElement>();
				}
			}
		}

		// Token: 0x17000188 RID: 392
		// (set) Token: 0x06000A57 RID: 2647 RVA: 0x00023090 File Offset: 0x00021290
		public override ESleekChildLayout UseChildAutoLayout
		{
			set
			{
				bool flag = this._useChildAutoLayout != value;
				this._useChildAutoLayout = value;
				if (flag)
				{
					if (this._useChildAutoLayout == 2)
					{
						HorizontalLayoutGroup orAddComponent = this.transform.GetOrAddComponent<HorizontalLayoutGroup>();
						orAddComponent.childForceExpandWidth = this._expandChildren;
						orAddComponent.childForceExpandHeight = false;
					}
					else
					{
						this.transform.DestroyComponentIfExists<HorizontalLayoutGroup>();
					}
					if (this._useChildAutoLayout == 1)
					{
						VerticalLayoutGroup orAddComponent2 = this.transform.GetOrAddComponent<VerticalLayoutGroup>();
						orAddComponent2.childForceExpandWidth = false;
						orAddComponent2.childForceExpandHeight = this._expandChildren;
					}
					else
					{
						this.transform.DestroyComponentIfExists<VerticalLayoutGroup>();
					}
					this.ApplyChildPerpendicularAlignment();
					this.ApplyChildLayoutPadding();
				}
			}
		}

		// Token: 0x17000189 RID: 393
		// (set) Token: 0x06000A58 RID: 2648 RVA: 0x00023124 File Offset: 0x00021324
		public override ESleekChildPerpendicularAlignment ChildPerpendicularAlignment
		{
			set
			{
				this._childPerpendicularAlignment = value;
				this.ApplyChildPerpendicularAlignment();
			}
		}

		// Token: 0x1700018A RID: 394
		// (set) Token: 0x06000A59 RID: 2649 RVA: 0x00023134 File Offset: 0x00021334
		public override bool ExpandChildren
		{
			set
			{
				bool flag = this._expandChildren != value;
				this._expandChildren = value;
				if (flag)
				{
					ESleekChildLayout useChildAutoLayout = this._useChildAutoLayout;
					if (useChildAutoLayout != 1)
					{
						if (useChildAutoLayout == 2)
						{
							this.transform.GetComponent<HorizontalLayoutGroup>().childForceExpandWidth = this._expandChildren;
							return;
						}
					}
					else
					{
						this.transform.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = this._expandChildren;
					}
				}
			}
		}

		// Token: 0x1700018B RID: 395
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x00023192 File Offset: 0x00021392
		public override bool IgnoreLayout
		{
			set
			{
				bool flag = this._ignoreLayout != value;
				this._ignoreLayout = value;
				if (flag)
				{
					if (this.ShouldHaveLayoutElementComponent)
					{
						this.transform.GetOrAddComponent<LayoutElement>().ignoreLayout = true;
						return;
					}
					this.transform.DestroyComponentIfExists<LayoutElement>();
				}
			}
		}

		// Token: 0x1700018C RID: 396
		// (set) Token: 0x06000A5B RID: 2651 RVA: 0x000231CE File Offset: 0x000213CE
		public override float ChildAutoLayoutPadding
		{
			set
			{
				this._childAutoLayoutPadding = value;
				this.ApplyChildLayoutPadding();
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x000231DD File Offset: 0x000213DD
		public override ISleekElement Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x000231E5 File Offset: 0x000213E5
		public GlazierElementBase_uGUI(Glazier_uGUI glazier)
		{
			this.glazier = glazier;
		}

		/// <summary>
		/// Called after constructor when not populating from component pool.
		/// </summary>
		// Token: 0x06000A5E RID: 2654 RVA: 0x00023200 File Offset: 0x00021400
		public virtual void ConstructNew()
		{
			this.gameObject = new GameObject(base.GetType().Name, new Type[]
			{
				typeof(RectTransform)
			});
			this.transform = this.gameObject.GetRectTransform();
			this.transform.pivot = new Vector2(0f, 1f);
		}

		/// <summary>
		/// Called after constructor when re-using components from pool.
		/// </summary>
		// Token: 0x06000A5F RID: 2655 RVA: 0x00023261 File Offset: 0x00021461
		public void ConstructFromPool(GlazierElementBase_uGUI.PoolData poolData)
		{
			this.gameObject = poolData.gameObject;
			this.transform = this.gameObject.GetRectTransform();
			this.gameObject.SetActive(true);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0002328C File Offset: 0x0002148C
		public override int FindIndexOfChild(ISleekElement child)
		{
			return this._children.IndexOf((GlazierElementBase_uGUI)child.AttachmentRoot);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000232A4 File Offset: 0x000214A4
		public override ISleekElement GetChildAtIndex(int index)
		{
			return this._children[index];
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x000232B4 File Offset: 0x000214B4
		public override void RemoveChild(ISleekElement child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			GlazierElementBase_uGUI glazierElementBase_uGUI = child.AttachmentRoot as GlazierElementBase_uGUI;
			if (glazierElementBase_uGUI != null)
			{
				glazierElementBase_uGUI._parent = null;
				glazierElementBase_uGUI.InternalDestroy();
				this._children.Remove(glazierElementBase_uGUI);
				return;
			}
			UnturnedLog.warn("{0} cannot remove non-IMGUI element {1}", new object[]
			{
				base.GetType().Name,
				child.AttachmentRoot.GetType().Name
			});
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0002332C File Offset: 0x0002152C
		public override void RemoveAllChildren()
		{
			foreach (GlazierElementBase_uGUI glazierElementBase_uGUI in this._children)
			{
				glazierElementBase_uGUI._parent = null;
				glazierElementBase_uGUI.InternalDestroy();
			}
			this._children.Clear();
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00023390 File Offset: 0x00021590
		protected override void UpdateChildren()
		{
			foreach (GlazierElementBase_uGUI glazierElementBase_uGUI in this._children)
			{
				if (glazierElementBase_uGUI.IsVisible)
				{
					glazierElementBase_uGUI.Update();
				}
			}
		}

		/// <summary>
		/// Synchronize uGUI component colors with background/text/image etc. colors.
		/// Called when custom UI colors are changed, and after constructor.
		/// </summary>
		// Token: 0x06000A65 RID: 2661 RVA: 0x000233EC File Offset: 0x000215EC
		public virtual void SynchronizeColors()
		{
		}

		/// <summary>
		/// Synchronize uGUI component sprites with theme sprites.
		/// Called when custom UI theme is changed, and after constructor.
		/// </summary>
		// Token: 0x06000A66 RID: 2662 RVA: 0x000233EE File Offset: 0x000215EE
		public virtual void SynchronizeTheme()
		{
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x000233F0 File Offset: 0x000215F0
		public override void AddChild(ISleekElement child)
		{
			GlazierElementBase_uGUI glazierElementBase_uGUI = child.AttachmentRoot as GlazierElementBase_uGUI;
			if (glazierElementBase_uGUI == null)
			{
				UnturnedLog.warn("{0} cannot add non-uGUI element {1}", new object[]
				{
					base.GetType().Name,
					child.AttachmentRoot.GetType().Name
				});
				return;
			}
			if (glazierElementBase_uGUI._parent == this)
			{
				return;
			}
			if (glazierElementBase_uGUI._parent != null)
			{
				glazierElementBase_uGUI._parent._children.Remove(glazierElementBase_uGUI);
			}
			this._children.Add(glazierElementBase_uGUI);
			glazierElementBase_uGUI._parent = this;
			glazierElementBase_uGUI.transform.SetParent(this.AttachmentTransform, false);
			glazierElementBase_uGUI.UpdateDirtyTransform();
			glazierElementBase_uGUI.EnableComponents();
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00023494 File Offset: 0x00021694
		public override Vector2 ViewportToNormalizedPosition(Vector2 viewportPosition)
		{
			Rect absoluteRect = this.transform.GetAbsoluteRect();
			return new Vector2((viewportPosition.x * (float)Screen.width - absoluteRect.xMin) / absoluteRect.width, ((1f - viewportPosition.y) * (float)Screen.height - absoluteRect.yMin) / absoluteRect.height);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x000234F4 File Offset: 0x000216F4
		public override Vector2 GetNormalizedCursorPosition()
		{
			Vector2 vector = Input.mousePosition;
			Rect absoluteRect = this.transform.GetAbsoluteRect();
			return new Vector2((vector.x - absoluteRect.xMin) / absoluteRect.width, ((float)Screen.height - vector.y - absoluteRect.yMin) / absoluteRect.height);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00023550 File Offset: 0x00021750
		public override Vector2 GetAbsoluteSize()
		{
			return this.transform.GetAbsoluteRect().size;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00023570 File Offset: 0x00021770
		public override void SetAsFirstSibling()
		{
			if (this._parent != null)
			{
				this.transform.SetAsFirstSibling();
				if (this._parent._children.Remove(this))
				{
					this._parent._children.Insert(0, this);
				}
			}
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x000235AC File Offset: 0x000217AC
		protected override void UpdateDirtyTransform()
		{
			this.isTransformDirty = false;
			if (this._useManualLayout)
			{
				this.transform.anchorMin = new Vector2(base.PositionScale_X, 1f - base.PositionScale_Y - base.SizeScale_Y);
				this.transform.anchorMax = new Vector2(base.PositionScale_X + base.SizeScale_X, 1f - base.PositionScale_Y);
				this.transform.anchoredPosition = new Vector2(base.PositionOffset_X, -base.PositionOffset_Y);
				this.transform.sizeDelta = new Vector2(base.SizeOffset_X, base.SizeOffset_Y);
			}
			else
			{
				this.transform.anchorMin = new Vector2(0f, 1f);
				this.transform.anchorMax = new Vector2(0f, 1f);
				this.transform.anchoredPosition = Vector2.zero;
				this.transform.sizeDelta = Vector2.zero;
			}
			if (this._useWidthLayoutOverride || this._useHeightLayoutOverride)
			{
				LayoutElement component = this.transform.GetComponent<LayoutElement>();
				if (component != null)
				{
					component.preferredWidth = (this._useWidthLayoutOverride ? base.SizeOffset_X : 0f);
					component.preferredHeight = (this._useHeightLayoutOverride ? base.SizeOffset_Y : 0f);
					component.minWidth = component.preferredWidth;
					component.minHeight = component.preferredHeight;
				}
			}
		}

		/// <returns>False if element couldn't be released into pool and should be destroyed.</returns>
		// Token: 0x06000A6D RID: 2669 RVA: 0x0002371F File Offset: 0x0002191F
		protected virtual bool ReleaseIntoPool()
		{
			return false;
		}

		/// <summary>
		/// Unity recommends enabling components after parenting into the destination hierarchy.
		/// </summary>
		// Token: 0x06000A6E RID: 2670 RVA: 0x00023722 File Offset: 0x00021922
		protected virtual void EnableComponents()
		{
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00023724 File Offset: 0x00021924
		protected void PopulateBasePoolData(GlazierElementBase_uGUI.PoolData poolData)
		{
			this.transform.SetParent(null, false);
			Object.DontDestroyOnLoad(this.gameObject);
			poolData.gameObject = this.gameObject;
			this.gameObject = null;
			this.transform = null;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00023758 File Offset: 0x00021958
		public override void InternalDestroy()
		{
			this.RemoveAllChildren();
			if (this.gameObject != null && (this.ShouldHaveLayoutElementComponent || this._useChildAutoLayout > 0 || !this.ReleaseIntoPool()) && this.gameObject != null)
			{
				Object.Destroy(this.gameObject);
				this.gameObject = null;
			}
		}

		/// <summary>
		/// RectTransform children should be attached to. Overridden by ScrollView content panel.
		/// </summary>
		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x000237B7 File Offset: 0x000219B7
		public virtual RectTransform AttachmentTransform
		{
			get
			{
				return this.transform;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x000237BF File Offset: 0x000219BF
		// (set) Token: 0x06000A73 RID: 2675 RVA: 0x000237C7 File Offset: 0x000219C7
		public GameObject gameObject { get; private set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x000237D0 File Offset: 0x000219D0
		// (set) Token: 0x06000A75 RID: 2677 RVA: 0x000237D8 File Offset: 0x000219D8
		public RectTransform transform { get; private set; }

		/// <summary>
		/// This helper property's purpose is to:
		/// - Ensure other properties don't accidentally remove LayoutElement if others need it.
		/// - Ensure LayoutElement is destroyed before returning to pool.
		/// </summary>
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x000237E1 File Offset: 0x000219E1
		private bool ShouldHaveLayoutElementComponent
		{
			get
			{
				return this._useWidthLayoutOverride || this._useHeightLayoutOverride || this._ignoreLayout;
			}
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x000237FC File Offset: 0x000219FC
		private void ApplyChildPerpendicularAlignment()
		{
			if (this._useChildAutoLayout == 2)
			{
				HorizontalLayoutGroup component = this.transform.GetComponent<HorizontalLayoutGroup>();
				switch (this._childPerpendicularAlignment)
				{
				default:
					component.childAlignment = 3;
					return;
				case 1:
					component.childAlignment = 0;
					return;
				case 2:
					component.childAlignment = 6;
					break;
				}
			}
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00023850 File Offset: 0x00021A50
		private void ApplyChildLayoutPadding()
		{
			int num = Mathf.RoundToInt(this._childAutoLayoutPadding);
			RectOffset padding = new RectOffset(num, num, num, num);
			ESleekChildLayout useChildAutoLayout = this._useChildAutoLayout;
			if (useChildAutoLayout != 1)
			{
				if (useChildAutoLayout == 2)
				{
					this.transform.GetComponent<HorizontalLayoutGroup>().padding = padding;
					return;
				}
			}
			else
			{
				this.transform.GetComponent<VerticalLayoutGroup>().padding = padding;
			}
		}

		// Token: 0x040003FB RID: 1019
		public GlazierElementBase_uGUI _parent;

		// Token: 0x040003FE RID: 1022
		internal List<GlazierElementBase_uGUI> _children = new List<GlazierElementBase_uGUI>();

		// Token: 0x0200087A RID: 2170
		public class PoolData
		{
			// Token: 0x04003193 RID: 12691
			public GameObject gameObject;
		}
	}
}
