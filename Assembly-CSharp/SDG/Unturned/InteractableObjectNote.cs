using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200045D RID: 1117
	public class InteractableObjectNote : InteractableObjectTriggerableBase
	{
		// Token: 0x060021F6 RID: 8694 RVA: 0x00083C1C File Offset: 0x00081E1C
		public override void use()
		{
			PlayerBarricadeSignUI.open(base.objectAsset.interactabilityText);
			PlayerLifeUI.close();
			ObjectManager.useObjectQuest(base.transform);
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x00083C3E File Offset: 0x00081E3E
		public override bool checkUseable()
		{
			return !PlayerUI.window.showCursor;
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x00083C4D File Offset: 0x00081E4D
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (base.objectAsset.interactabilityHint == EObjectInteractabilityHint.USE)
			{
				message = EPlayerMessage.USE;
			}
			else
			{
				message = EPlayerMessage.NONE;
			}
			text = "";
			color = Color.white;
			return !PlayerUI.window.showCursor;
		}
	}
}
