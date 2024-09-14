using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000186 RID: 390
	internal class ScrollRectEx : ScrollRect
	{
		// Token: 0x06000AE6 RID: 2790 RVA: 0x000247AC File Offset: 0x000229AC
		public override void OnScroll(PointerEventData data)
		{
			if (this.HandleScrollWheel)
			{
				base.OnScroll(data);
				return;
			}
			if (base.transform.parent != null)
			{
				ScrollRect componentInParent = base.transform.parent.GetComponentInParent<ScrollRect>();
				if (componentInParent != null)
				{
					componentInParent.OnScroll(data);
				}
			}
		}

		// Token: 0x04000424 RID: 1060
		[SerializeField]
		public bool HandleScrollWheel = true;
	}
}
