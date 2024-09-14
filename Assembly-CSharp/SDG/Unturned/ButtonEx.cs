using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000177 RID: 375
	internal class ButtonEx : Button
	{
		// Token: 0x06000A20 RID: 2592 RVA: 0x00022611 File Offset: 0x00020811
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != 1)
			{
				base.OnPointerClick(eventData);
				return;
			}
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			this.onRightClick.Invoke();
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00022640 File Offset: 0x00020840
		public override void OnPointerDown(PointerEventData eventData)
		{
			PointerEventData.InputButton button = eventData.button;
			eventData.button = 0;
			base.OnPointerDown(eventData);
			eventData.button = button;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0002266C File Offset: 0x0002086C
		public override void OnPointerUp(PointerEventData eventData)
		{
			PointerEventData.InputButton button = eventData.button;
			eventData.button = 0;
			base.OnPointerUp(eventData);
			eventData.button = button;
		}

		// Token: 0x040003E6 RID: 998
		public Button.ButtonClickedEvent onRightClick = new Button.ButtonClickedEvent();
	}
}
