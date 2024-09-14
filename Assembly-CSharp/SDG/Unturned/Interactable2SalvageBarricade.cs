using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000448 RID: 1096
	public class Interactable2SalvageBarricade : Interactable2
	{
		// Token: 0x060020E9 RID: 8425 RVA: 0x0007F0D0 File Offset: 0x0007D2D0
		public override bool checkHint(out EPlayerMessage message, out float data)
		{
			message = EPlayerMessage.SALVAGE;
			if (this.hp != null)
			{
				data = (float)this.hp.hp / 100f;
			}
			else
			{
				data = 0f;
			}
			return this.shouldBypassPickupOwnership || base.hasOwnership;
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x0007F11F File Offset: 0x0007D31F
		public override void use()
		{
			BarricadeManager.salvageBarricade(this.root);
		}

		// Token: 0x04001023 RID: 4131
		public Transform root;

		// Token: 0x04001024 RID: 4132
		public Interactable2HP hp;

		// Token: 0x04001025 RID: 4133
		public bool shouldBypassPickupOwnership;
	}
}
