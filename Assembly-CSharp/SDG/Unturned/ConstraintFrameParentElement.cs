using System;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x02000194 RID: 404
	internal class ConstraintFrameParentElement : VisualElement
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x000287D1 File Offset: 0x000269D1
		public override VisualElement contentContainer
		{
			get
			{
				return this._contentContainerOverride;
			}
		}

		// Token: 0x04000497 RID: 1175
		public VisualElement _contentContainerOverride;
	}
}
