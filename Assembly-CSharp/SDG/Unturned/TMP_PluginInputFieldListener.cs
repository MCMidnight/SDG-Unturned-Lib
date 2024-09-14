using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	// Token: 0x02000588 RID: 1416
	public class TMP_PluginInputFieldListener : MonoBehaviour
	{
		// Token: 0x06002D51 RID: 11601 RVA: 0x000C5360 File Offset: 0x000C3560
		protected void Start()
		{
			if (this.targetInputField != null)
			{
				this.targetInputField.onEndEdit.AddListener(new UnityAction<string>(this.onEndEdit));
			}
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000C538C File Offset: 0x000C358C
		private void onEndEdit(string text)
		{
			if (this.targetInputField != null)
			{
				EffectManager.sendEffectTextCommitted(this.targetInputField.name, text);
			}
		}

		// Token: 0x04001872 RID: 6258
		public TMP_InputField targetInputField;
	}
}
