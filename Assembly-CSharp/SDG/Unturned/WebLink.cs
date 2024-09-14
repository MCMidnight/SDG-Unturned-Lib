using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x0200073A RID: 1850
	public class WebLink : MonoBehaviour
	{
		// Token: 0x06003CD1 RID: 15569 RVA: 0x0012180C File Offset: 0x0011FA0C
		private void onClick()
		{
			string text;
			if (WebUtils.ParseThirdPartyUrl(this.url, out text, true, true))
			{
				Provider.openURL(text);
				return;
			}
			UnturnedLog.warn("Ignoring potentially unsafe web link component url {0}", new object[]
			{
				this.url
			});
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x0012184A File Offset: 0x0011FA4A
		private void Start()
		{
			this.targetButton.onClick.AddListener(new UnityAction(this.onClick));
		}

		// Token: 0x0400261A RID: 9754
		public Button targetButton;

		// Token: 0x0400261B RID: 9755
		public string url;
	}
}
