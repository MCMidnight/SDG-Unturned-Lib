using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Unturned
{
	// Token: 0x0200018C RID: 396
	internal class GlazieruGUITooltip : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06000B73 RID: 2931 RVA: 0x00026A6A File Offset: 0x00024C6A
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this.onStack)
			{
				this.onStack = true;
				GlazieruGUITooltip.activeTooltips.Add(this);
			}
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00026A86 File Offset: 0x00024C86
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.onStack)
			{
				this.onStack = false;
				GlazieruGUITooltip.activeTooltips.Remove(this);
			}
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00026AA3 File Offset: 0x00024CA3
		public static GlazieruGUITooltip GetTooltip()
		{
			if (GlazieruGUITooltip.activeTooltips.Count > 0)
			{
				return GlazieruGUITooltip.activeTooltips[GlazieruGUITooltip.activeTooltips.Count - 1];
			}
			return null;
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00026ACA File Offset: 0x00024CCA
		private void OnDisable()
		{
			if (this.onStack)
			{
				this.onStack = false;
				GlazieruGUITooltip.activeTooltips.Remove(this);
			}
		}

		// Token: 0x0400045B RID: 1115
		public string text;

		// Token: 0x0400045C RID: 1116
		public Color color = Color.white;

		// Token: 0x0400045D RID: 1117
		private bool onStack;

		// Token: 0x0400045E RID: 1118
		private static List<GlazieruGUITooltip> activeTooltips = new List<GlazieruGUITooltip>();
	}
}
