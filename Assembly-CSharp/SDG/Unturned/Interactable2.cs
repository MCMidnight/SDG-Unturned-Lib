using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000446 RID: 1094
	public class Interactable2 : MonoBehaviour
	{
		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060020E4 RID: 8420 RVA: 0x0007F090 File Offset: 0x0007D290
		public bool hasOwnership
		{
			get
			{
				return OwnershipTool.checkToggle(this.owner, this.group);
			}
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x0007F0A3 File Offset: 0x0007D2A3
		public virtual bool checkHint(out EPlayerMessage message, out float data)
		{
			message = EPlayerMessage.NONE;
			data = 0f;
			return false;
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x0007F0B0 File Offset: 0x0007D2B0
		public virtual void use()
		{
		}

		// Token: 0x0400101F RID: 4127
		public ulong owner;

		// Token: 0x04001020 RID: 4128
		public ulong group;

		// Token: 0x04001021 RID: 4129
		public float salvageDurationMultiplier = 1f;
	}
}
