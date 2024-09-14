using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200043B RID: 1083
	public class GameMode
	{
		// Token: 0x060020B3 RID: 8371 RVA: 0x0007E2E2 File Offset: 0x0007C4E2
		public virtual GameObject getPlayerGameObject(SteamPlayerID playerID)
		{
			return Object.Instantiate<GameObject>(Resources.Load<GameObject>("Characters/Player_Dedicated"));
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x0007E2F3 File Offset: 0x0007C4F3
		public GameMode()
		{
			UnturnedLog.info(this);
		}
	}
}
