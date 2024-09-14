using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x02000195 RID: 405
	internal class ConstraintFrameChildElement : VisualElement
	{
		// Token: 0x06000C07 RID: 3079 RVA: 0x000287E4 File Offset: 0x000269E4
		public void OnParentGeometryChanged(GeometryChangedEvent geometryChangedEvent)
		{
			if (this.constraint == null)
			{
				base.style.left = 0f;
				base.style.right = 0f;
				base.style.top = 0f;
				base.style.bottom = 0f;
				return;
			}
			Rect newRect = geometryChangedEvent.newRect;
			if (newRect.width < newRect.height * this.aspectRatio)
			{
				base.style.left = 0f;
				base.style.right = 0f;
				float num = newRect.width / this.aspectRatio / newRect.height;
				StyleLength styleLength = Length.Percent((1f - num) * 50f);
				base.style.top = styleLength;
				base.style.bottom = styleLength;
				return;
			}
			float num2 = newRect.height * this.aspectRatio / newRect.width;
			StyleLength styleLength2 = Length.Percent((1f - num2) * 50f);
			base.style.left = styleLength2;
			base.style.right = styleLength2;
			base.style.top = 0f;
			base.style.bottom = 0f;
		}

		// Token: 0x04000498 RID: 1176
		public ESleekConstraint constraint;

		// Token: 0x04000499 RID: 1177
		public float aspectRatio = 1f;
	}
}
