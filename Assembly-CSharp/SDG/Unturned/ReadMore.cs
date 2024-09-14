using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x020006FD RID: 1789
	public class ReadMore : MonoBehaviour
	{
		// Token: 0x06003B39 RID: 15161 RVA: 0x001150A8 File Offset: 0x001132A8
		public void Refresh()
		{
			base.GetComponent<Text>().text = (this.targetContent.activeSelf ? this.offText : this.onText);
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x001150D0 File Offset: 0x001132D0
		private void onClick()
		{
			this.targetContent.SetActive(!this.targetContent.activeSelf);
			this.Refresh();
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x001150F1 File Offset: 0x001132F1
		private void Start()
		{
			this.targetButton.onClick.AddListener(new UnityAction(this.onClick));
		}

		// Token: 0x04002524 RID: 9508
		public Button targetButton;

		// Token: 0x04002525 RID: 9509
		public GameObject targetContent;

		// Token: 0x04002526 RID: 9510
		public string onText;

		// Token: 0x04002527 RID: 9511
		public string offText;
	}
}
