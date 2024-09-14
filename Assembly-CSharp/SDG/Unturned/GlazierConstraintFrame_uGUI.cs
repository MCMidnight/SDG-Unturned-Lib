using System;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000179 RID: 377
	internal class GlazierConstraintFrame_uGUI : GlazierElementBase_uGUI, ISleekConstraintFrame, ISleekElement
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x00022E1C File Offset: 0x0002101C
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x00022E24 File Offset: 0x00021024
		public ESleekConstraint Constraint
		{
			get
			{
				return this._constraint;
			}
			set
			{
				if (this._constraint != null)
				{
					throw new NotSupportedException();
				}
				this._constraint = value;
				if (this.Constraint == 1)
				{
					this.aspectRatioFitter = this.contentTransform.gameObject.AddComponent<AspectRatioFitter>();
					this.aspectRatioFitter.aspectMode = 3;
					this.aspectRatioFitter.aspectRatio = this._aspectRatio;
				}
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x00022E82 File Offset: 0x00021082
		// (set) Token: 0x06000A4B RID: 2635 RVA: 0x00022E8A File Offset: 0x0002108A
		public float AspectRatio
		{
			get
			{
				return this._aspectRatio;
			}
			set
			{
				this._aspectRatio = value;
				if (this.aspectRatioFitter != null)
				{
					this.aspectRatioFitter.aspectRatio = value;
				}
			}
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00022EAD File Offset: 0x000210AD
		public GlazierConstraintFrame_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00022EC4 File Offset: 0x000210C4
		public override void ConstructNew()
		{
			base.ConstructNew();
			GameObject gameObject = new GameObject("Content", new Type[]
			{
				typeof(RectTransform)
			});
			this.contentTransform = gameObject.GetRectTransform();
			this.contentTransform.SetParent(base.transform, false);
			this.contentTransform.anchorMin = new Vector2(0f, 0f);
			this.contentTransform.anchorMax = new Vector2(1f, 1f);
			this.contentTransform.anchoredPosition = Vector2.zero;
			this.contentTransform.sizeDelta = Vector2.zero;
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x00022F67 File Offset: 0x00021167
		public override RectTransform AttachmentTransform
		{
			get
			{
				return this.contentTransform;
			}
		}

		// Token: 0x040003F3 RID: 1011
		private ESleekConstraint _constraint;

		// Token: 0x040003F4 RID: 1012
		private float _aspectRatio = 1f;

		// Token: 0x040003F5 RID: 1013
		private RectTransform contentTransform;

		// Token: 0x040003F6 RID: 1014
		private AspectRatioFitter aspectRatioFitter;
	}
}
