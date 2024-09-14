using System;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000738 RID: 1848
	public class ToggleWorkshopSubscription : MonoBehaviour
	{
		// Token: 0x06003CC6 RID: 15558 RVA: 0x00121520 File Offset: 0x0011F720
		public void Refresh()
		{
			bool subscribed = Provider.provider.workshopService.getSubscribed(this.parentFileId.m_PublishedFileId);
			this.targetLabel.text = (subscribed ? this.unsubscribeText : this.subscribeText);
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x00121564 File Offset: 0x0011F764
		private void onClick()
		{
			bool flag = !Provider.provider.workshopService.getSubscribed(this.parentFileId.m_PublishedFileId);
			Provider.provider.workshopService.setSubscribed(this.parentFileId.m_PublishedFileId, flag);
			if (this.childFileIds != null && this.childFileIds.Length != 0 && flag)
			{
				foreach (PublishedFileId_t publishedFileId_t in this.childFileIds)
				{
					Provider.provider.workshopService.setSubscribed(publishedFileId_t.m_PublishedFileId, flag);
				}
			}
			this.Refresh();
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x001215FC File Offset: 0x0011F7FC
		private void Start()
		{
			this.targetButton.onClick.AddListener(new UnityAction(this.onClick));
		}

		// Token: 0x0400260F RID: 9743
		public PublishedFileId_t parentFileId;

		// Token: 0x04002610 RID: 9744
		public PublishedFileId_t[] childFileIds;

		// Token: 0x04002611 RID: 9745
		public Button targetButton;

		// Token: 0x04002612 RID: 9746
		public Text targetLabel;

		// Token: 0x04002613 RID: 9747
		public string subscribeText;

		// Token: 0x04002614 RID: 9748
		public string unsubscribeText;
	}
}
