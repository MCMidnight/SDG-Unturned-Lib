using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000455 RID: 1109
	public class InteractableItem : Interactable
	{
		// Token: 0x06002182 RID: 8578 RVA: 0x00081388 File Offset: 0x0007F588
		public override void use()
		{
			ItemManager.takeItem(base.transform.parent, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x000813AA File Offset: 0x0007F5AA
		public override bool checkHighlight(out Color color)
		{
			color = ItemTool.getRarityColorHighlight(this.asset.rarity);
			return true;
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x000813C3 File Offset: 0x0007F5C3
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.ITEM;
			text = this.asset.itemName;
			color = ItemTool.getRarityColorUI(this.asset.rarity);
			return true;
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x000813EC File Offset: 0x0007F5EC
		public void clampRange()
		{
			if (this.wasReset)
			{
				return;
			}
			if ((base.transform.position - base.transform.parent.position).sqrMagnitude > 400f)
			{
				base.transform.position = base.transform.parent.position;
				this.wasReset = true;
				ItemManager.clampedItems.RemoveFast(this);
				Object.Destroy(base.GetComponent<Rigidbody>());
			}
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x0008146A File Offset: 0x0007F66A
		private void OnEnable()
		{
			ItemManager.clampedItems.Add(this);
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x00081477 File Offset: 0x0007F677
		private void OnDisable()
		{
			if (this.wasReset)
			{
				return;
			}
			ItemManager.clampedItems.RemoveFast(this);
		}

		// Token: 0x04001074 RID: 4212
		public Item item;

		// Token: 0x04001075 RID: 4213
		public ItemJar jar;

		// Token: 0x04001076 RID: 4214
		public ItemAsset asset;

		// Token: 0x04001077 RID: 4215
		private bool wasReset;
	}
}
