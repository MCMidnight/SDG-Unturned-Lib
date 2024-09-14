using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000453 RID: 1107
	public class InteractableForage : Interactable
	{
		// Token: 0x06002163 RID: 8547 RVA: 0x00080CA7 File Offset: 0x0007EEA7
		public override void use()
		{
			ResourceManager.forage(base.transform.parent);
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x00080CBC File Offset: 0x0007EEBC
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.asset != null && !string.IsNullOrEmpty(this.asset.interactabilityText))
			{
				message = EPlayerMessage.INTERACT;
				text = this.asset.interactabilityText;
			}
			else
			{
				message = EPlayerMessage.FORAGE;
				text = string.Empty;
			}
			color = Color.white;
			return true;
		}

		// Token: 0x04001064 RID: 4196
		internal ResourceAsset asset;
	}
}
