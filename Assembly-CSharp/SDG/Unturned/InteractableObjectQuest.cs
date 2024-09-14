using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200045F RID: 1119
	public class InteractableObjectQuest : InteractableObjectTriggerableBase
	{
		// Token: 0x0600220A RID: 8714 RVA: 0x00083F50 File Offset: 0x00082150
		public override void use()
		{
			EffectAsset effectAsset = base.objectAsset.FindInteractabilityEffectAsset();
			if (effectAsset != null && Time.realtimeSinceStartup - this.lastEffect > 1f)
			{
				this.lastEffect = Time.realtimeSinceStartup;
				Transform transform = base.transform.Find("Effect");
				if (transform != null)
				{
					EffectManager.effect(effectAsset, transform.position, transform.forward);
				}
				else
				{
					EffectManager.effect(effectAsset, base.transform.position, base.transform.forward);
				}
			}
			ObjectManager.useObjectQuest(base.transform);
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x00083FE1 File Offset: 0x000821E1
		public override bool checkUseable()
		{
			return base.objectAsset.areInteractabilityConditionsMet(Player.player);
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x00083FF4 File Offset: 0x000821F4
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			int i = 0;
			while (i < base.objectAsset.interactabilityConditions.Length)
			{
				INPCCondition inpccondition = base.objectAsset.interactabilityConditions[i];
				if (!inpccondition.isConditionMet(Player.player))
				{
					text = inpccondition.formatCondition(Player.player);
					color = Color.white;
					if (string.IsNullOrEmpty(text))
					{
						message = EPlayerMessage.NONE;
						return false;
					}
					message = EPlayerMessage.CONDITION;
					return true;
				}
				else
				{
					i++;
				}
			}
			message = EPlayerMessage.INTERACT;
			text = base.objectAsset.interactabilityText;
			color = Color.white;
			return true;
		}

		// Token: 0x040010C6 RID: 4294
		private float lastEffect;
	}
}
