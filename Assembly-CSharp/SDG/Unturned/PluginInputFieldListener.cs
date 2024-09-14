using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	// Token: 0x02000587 RID: 1415
	public class PluginInputFieldListener : MonoBehaviour
	{
		// Token: 0x06002D4E RID: 11598 RVA: 0x000C530B File Offset: 0x000C350B
		protected void Start()
		{
			if (this.targetInputField != null)
			{
				this.targetInputField.onEndEdit.AddListener(new UnityAction<string>(this.onEndEdit));
			}
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000C5337 File Offset: 0x000C3537
		private void onEndEdit(string text)
		{
			if (this.targetInputField != null)
			{
				EffectManager.sendEffectTextCommitted(this.targetInputField.name, text);
			}
		}

		// Token: 0x04001871 RID: 6257
		public InputField targetInputField;
	}
}
