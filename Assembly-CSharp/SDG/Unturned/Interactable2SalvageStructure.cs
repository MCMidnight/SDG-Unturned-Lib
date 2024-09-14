using System;

namespace SDG.Unturned
{
	// Token: 0x02000449 RID: 1097
	public class Interactable2SalvageStructure : Interactable2
	{
		// Token: 0x060020EC RID: 8428 RVA: 0x0007F134 File Offset: 0x0007D334
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
			return base.hasOwnership;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x0007F170 File Offset: 0x0007D370
		public override void use()
		{
			StructureManager.salvageStructure(base.transform);
		}

		// Token: 0x04001026 RID: 4134
		public Interactable2HP hp;
	}
}
