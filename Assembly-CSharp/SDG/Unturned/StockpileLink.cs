using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000736 RID: 1846
	public class StockpileLink : MonoBehaviour
	{
		// Token: 0x06003CC0 RID: 15552 RVA: 0x0012143A File Offset: 0x0011F63A
		private void onClick()
		{
			ItemStore.Get().ViewItem(this.itemdefid);
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x0012144C File Offset: 0x0011F64C
		private void Start()
		{
			this.targetButton.onClick.AddListener(new UnityAction(this.onClick));
		}

		// Token: 0x0400260B RID: 9739
		public Button targetButton;

		// Token: 0x0400260C RID: 9740
		public int itemdefid;
	}
}
