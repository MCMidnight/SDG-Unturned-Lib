using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000162 RID: 354
	internal class GlazierConstraintFrame_IMGUI : GlazierElementBase_IMGUI, ISleekConstraintFrame, ISleekElement
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x0001F0A2 File Offset: 0x0001D2A2
		// (set) Token: 0x060008E0 RID: 2272 RVA: 0x0001F0AA File Offset: 0x0001D2AA
		public ESleekConstraint Constraint
		{
			get
			{
				return this._constraint;
			}
			set
			{
				this._constraint = value;
				this.isTransformDirty = true;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x0001F0BA File Offset: 0x0001D2BA
		// (set) Token: 0x060008E2 RID: 2274 RVA: 0x0001F0C2 File Offset: 0x0001D2C2
		public float AspectRatio
		{
			get
			{
				return this._aspectRatio;
			}
			set
			{
				this._aspectRatio = value;
				this.isTransformDirty = true;
			}
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001F0D4 File Offset: 0x0001D2D4
		protected override Rect CalculateDrawRect()
		{
			Rect result = base.CalculateDrawRect();
			if (this.Constraint == 1)
			{
				if (result.width < result.height * this._aspectRatio)
				{
					float num = result.width / this._aspectRatio;
					result.y += (result.height - num) * 0.5f;
					result.height = num;
				}
				else
				{
					float num2 = result.height * this._aspectRatio;
					result.x += (result.width - num2) * 0.5f;
					result.width = num2;
				}
			}
			return result;
		}

		// Token: 0x04000366 RID: 870
		private ESleekConstraint _constraint;

		// Token: 0x04000367 RID: 871
		private float _aspectRatio = 1f;
	}
}
