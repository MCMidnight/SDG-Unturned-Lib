using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x020006FB RID: 1787
	public class DismissWorkshopArticle : MonoBehaviour
	{
		// Token: 0x06003B33 RID: 15155 RVA: 0x00114FC7 File Offset: 0x001131C7
		private void onClick()
		{
			this.article.SetActive(false);
			LocalNews.dismissWorkshopItem(this.id);
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x00114FE0 File Offset: 0x001131E0
		private void Start()
		{
			this.targetButton.onClick.AddListener(new UnityAction(this.onClick));
		}

		// Token: 0x0400251E RID: 9502
		public ulong id;

		// Token: 0x0400251F RID: 9503
		public Button targetButton;

		// Token: 0x04002520 RID: 9504
		public GameObject article;
	}
}
