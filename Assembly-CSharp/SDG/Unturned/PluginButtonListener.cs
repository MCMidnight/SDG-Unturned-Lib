using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000586 RID: 1414
	public class PluginButtonListener : MonoBehaviour
	{
		// Token: 0x06002D4B RID: 11595 RVA: 0x000C52B7 File Offset: 0x000C34B7
		protected void Start()
		{
			if (this.targetButton != null)
			{
				this.targetButton.onClick.AddListener(new UnityAction(this.onTargetButtonClicked));
			}
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x000C52E3 File Offset: 0x000C34E3
		private void onTargetButtonClicked()
		{
			if (this.targetButton != null)
			{
				EffectManager.sendEffectClicked(this.targetButton.name);
			}
		}

		// Token: 0x04001870 RID: 6256
		public Button targetButton;
	}
}
